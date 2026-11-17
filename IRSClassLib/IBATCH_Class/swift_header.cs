using System;
using System.Collections;
using System.Text;


namespace IRSClassLib
{
    public class swift_header :   defaultFileHeader
    {
        #region Header Format
        /*
         * HEADER RECORD FORMAT:
         * 1  - 3  |  3 | constant "HDR"               | mandatory
         * 4  - 9  |  6 | record count                 | mandatory - left padded with '0'
         * 10 - 10 |  1 | rate type                    | file dependent integer
         * 11 - 30 | 20 | user id                      | file dependent grs login userid right padded with space
         * 31 - 38 |  8 | file date                    | date file was created (MMddyyyy). (being used as actual tran date.
         * --------------------------------------------
         */
        #endregion

        public ArrayList error_msg;
        public String Path { get { return base.path; } }
        public String old_path;
        public String new_path;
        public bool reformatted;

        public int processed_items;
        public decimal processed_amount;
        public int total_items;
        public decimal total_amount;

        public Hashtable currtab;
        public Hashtable itemtab;

        public String Filename
        {
            get { return new csFileIo().getFilename(base.path); }
        }

        public long FileSize
        {
            get { return new csFileIo().getFileSize(Path); }
        }

        protected override void parsing_exit(string buff)
        {
            
            swift_record rec = new swift_record(buff);

            total_items++;
            total_amount += (rec.has_bad_amount) ? 0.00M : rec.fx_amount;

            if (rec.is_valid)
            {
                processed_items++;
                processed_amount += rec.fx_amount;

                if (!rec.has_invalid_currency)
                {
                    decimal d = 0.00M;
                    int i = 0;

                    if (currtab.ContainsKey(rec.fx_currcd))
                    {
                        d = (decimal)currtab[rec.fx_currcd];
                    }
                    d += rec.fx_amount;
                    currtab[rec.fx_currcd] = d;

                    if (itemtab.ContainsKey(rec.fx_currcd))
                    {
                        i = (int)itemtab[rec.fx_currcd];
                    }
                    i++;
                    itemtab[rec.fx_currcd] = i;
                }
            }
            else
            {
                if (rec.errmsg != null && rec.errmsg.Count > 0)
                {
                    foreach (Object msg in rec.errmsg)
                    {
                        log_error(msg.ToString());
                    }
                }
            }
        }

        private void log_error(String msg)
        {
            if (error_msg == null) error_msg = new ArrayList();
            error_msg.Add(msg);
        }

        public bool isRaw()
        {
            swift_decoder decoder = new swift_decoder();
            return decoder.isRaw(path);
        }

        public void reformat(String new_path)
        {
            csFileIo fio = new csFileIo();
            this.new_path = new_path;
            this.old_path = base.path;
            try
            {
                swift_decoder decoder = new swift_decoder();
                reformatted = decoder.reformat(old_path, new_path, error_msg = new ArrayList());

                if (decoder.header_count != decoder.actual_count)
                {
                    log_error("record count " + decoder.header_count + " did not match actual count " + decoder.actual_count);
                }

                if (reformatted)
                {
                    base.path = new_path;
                }
                return;
            }
            catch (Exception e)
            {
                log_error("REFORMAT: " + e.Message);
            }
            reformatted = false;
            new_path = null;
        }

        private void clear_counters()
        {
            processed_amount = 0.00M;
            total_amount = 0.00M;
            processed_items = 0;
            total_items = 0;
            currtab = new Hashtable();
            itemtab = new Hashtable();
        }

        private int count_records()
        {
            csFileIo fio = new csFileIo();
            if (fio.open_input(base.path, (int)csFileIo.access_modes.BINARY))
            {
                int i = 0;
                while (!fio.EOF)
                {
                    String s = fio.readLine();
                    if (s == null) continue;
                    if (s.Length == 1 && Encoding.Default.GetBytes(s)[0] == 0x1A) continue;
                    if (i == 0 && s.Substring(0, 3).Equals("HDR")) continue;
                    try
                    {
                        i++;
                    }
                    catch
                    { }
                }

                fio.close_input();
                return i;
            }

            return -1;
        }

        private bool parseDelimitedHeader(String hdr, char delim, out String msg)
        {
            msg = String.Empty;
            String[] ss = hdr.Split(delim);
            try
            {
                base.hdr_const = "HDR";
                base.record_count = int.Parse(ss[1].Trim());
                base.rate_type = ss[2].Trim();
                base.userid = ss[3].Trim();
                base.filedate = DateTime.Parse(ss[4].Trim());
                base.hasDate = true;

                base.parse_header(base.toHeader());
                return true;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return false;
        }

        public const String newhdr = "INV";

        public String toDelimitedHeader(char delim)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(((isHeader()) ? base.hdr_const : newhdr) + delim);
            sb.Append(base.record_count.ToString("000000") + delim);
            sb.Append(base.rate_type.PadRight(1, ' ') + delim);
            sb.Append(base.userid.PadRight(20, ' ') + delim);
            sb.Append(base.FileDate.ToString("MM/dd/yyyy"));
            return sb.ToString();
        }

        public void create_newfile()
        {
            csFileIo fio = new csFileIo();
            String dir = fio.getDirectory(base.path);
            String fname = fio.getFilenameNoExt(base.path);
            String newfile = dir + @"\" + fname + ".tmp";
            if (fio.open_input(path, (int)csFileIo.access_modes.BINARY))
            {
                if (fio.open_output(newfile, (int)csFileIo.write_modes.CREATE))
                {
                    bool first = true;
                    fio.putLine(base.toHeader());
                    while (!fio.EOF)
                    {
                        String buff = fio.readLine();
                        if (buff == null) continue;
                        if (buff.Length == 1 && Encoding.Default.GetBytes(buff)[0] == 0x1A) continue;
                        if (first && buff.Substring(0, 3).Equals("HDR"))
                        {
                            first = false;
                            continue;
                        }
                        fio.putLine(buff);
                        first = false;
                    }
                    fio.close_output();
                }
                fio.close_input();
            }
            /*
            String oldfile = dir + @"\" + fname + ".old";
            fio.delete(oldfile);
            fio.rename(base.path, oldfile);
             * */
            fio.delete(base.path);
            fio.rename(fio.OutPutFileName, base.path);
        }


        public swift_header(String path) : base(path)
        {
            clear_counters();
        }

        public swift_header(String path, bool do_reformat) : base(path)
        {
            if (isRaw() & do_reformat)
            {
                reformat(new csFileIo().getDirectory(path) + @"\Archive\" + new csFileIo().getFilenameNoExt(path) + ".txt");
            }
            clear_counters();
        }

        public swift_header(int count, String rate_type, String userid, DateTime filedate) : base(count, rate_type, userid, filedate)
        {
            clear_counters();
        }

        public swift_header(String lpath, String rate_type, String userid, DateTime filedate) : base(0, rate_type, userid, filedate)
        {
            base.path = lpath;
            clear_counters();
            base.record_count = count_records();
        }

        private void do_reformat()
        {
            csFileIo tio = new csFileIo();
            if (isRaw())
            {
                reformat(tio.getDirectory(path) + @"\" + tio.getFilenameNoExt(path) + "_103.txt");
                if (reformatted)
                {
                    tio.delete(old_path);
                }
            }
        }

        public swift_header(String lpath, String hdr, char delim) : base(lpath)
        {
            clear_counters();
            String msg = String.Empty;
            if (parseDelimitedHeader(hdr, delim, out msg))
            {
                create_newfile();
                do_reformat();
            }
            else
            {
                throw new Exception(msg);
            }
        }


        ~swift_header()
        {
            error_msg = null;
        }

    }
}
