using System;
using System.Collections;
using System.Text;


namespace IRSClassLib
{
    internal class swift_decoder
    {
        private ArrayList errmsg;
        private String raw_path;
        private String new_path;

        #region Header Structure
        private class header_record
        {
            internal int hdr_length;
            private String hdr_const;
            internal int hdr_count;
            private String hdr_data;

            internal void setCount(int count)
            {
                hdr_count = count;
            }

            public override string ToString()
            {
                return hdr_const + hdr_count.ToString("000000") + hdr_data;
            }

            internal header_record(String buff)
            {
                hdr_length = buff.Length;
                hdr_const = buff.Substring(0, 3);
                try
                {
                    hdr_count = int.Parse(buff.Substring(3, 6));
                }
                catch
                {
                    hdr_count = -1;
                }
                hdr_data = buff.Substring(9);
            }
        }
        #endregion

        private header_record hdr;
        internal int actual_count;
        internal int header_count;

        public bool isRaw(String path)
        {
            return  new csFileIo().readAllText(path).Contains(mt103_decoder.TAG_BEGIN);
        }

        private bool is_header(String buff)
        {
            if (buff.StartsWith("HDR"))
            {
                try
                {
                    uint.Parse(buff.Substring(3, 6));
                    return true;
                }
                catch { }
            }

            return false;
        }

        private bool create_newpath(csFileIo fio, mt103_decoder decoder)
        {
            
            if (fio.open_output(new_path, (int)csFileIo.write_modes.CREATE))
            {
                if (hdr != null && hdr.hdr_length > 0)
                {
                    hdr.setCount(decoder.record_count);
                    fio.putLine(hdr.ToString());
                }

                while (!decoder.EOF)
                {
                    String s = decoder.readLine();
                    if (s == null) continue;
                    else                        
                        fio.putLine(s);
                }

                fio.close_output();
                decoder.close();

                return true;
            }
            else
            {
                errmsg.Add("create new_path: Error opening output " + new_path);
            }
            return false;
        }

        private bool reformat()
        {
            csFileIo fio = new csFileIo();
            int lno = 0;
            if (fio.open_input(raw_path, (int)csFileIo.access_modes.BINARY))
            {
                mt103_decoder decoder = new mt103_decoder();

                while (!fio.EOF)
                {
                    String buff = fio.readLine();
                    if (buff == null || buff.Length < 1) continue;
                    if (buff.Length == 1 && Encoding.Default.GetBytes(buff)[0] == 26) continue;
                    if (is_header(buff))
                    {
                        hdr = new header_record(buff);
                        header_count = hdr.hdr_count;
                        continue;
                    }
                    try
                    {
                        lno++;
                        decoder.reformat(buff);
                    }
                    catch (Exception e)
                    {
                        errmsg.Add(lno + ": " + e.Message);
                    }
                }
                fio.close_input();
                actual_count = lno;

                if (decoder.record_count > 0)
                {
                    return create_newpath(fio, decoder);
                }
                errmsg.Add("Reformat: No records created");
            }
            else
            {
                errmsg.Add("Reformat: unable to open input " + raw_path);
            }
            return false;
        }

        internal bool reformat(String raw_path, String new_path, ArrayList errmsg)
        {
            this.errmsg = errmsg;
            this.raw_path = raw_path;
            this.new_path = new_path;

            return reformat();
        }

        public swift_decoder()
        { }

        ~swift_decoder()
        {
            errmsg = null;
            hdr = null;
        }
    }
}
