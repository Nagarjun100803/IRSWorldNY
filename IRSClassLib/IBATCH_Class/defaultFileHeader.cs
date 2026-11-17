using System;
using System.Collections;
using System.Text;

namespace IRSClassLib
{
    public class defaultFileHeader : IHeaderRecord
    {
        #region header format
        /*
         * HEADER RECORD FORMAT:
         * 1  - 3  |  3 | constant "HDR"               | mandatory
         * 4  - 9  |  6 | record count                 | mandatory - left padded with '0'
         * 10 - 10 |  1 | rate type                    | file dependent integer
         * 11 - 30 | 20 | user id                      | file dependent grs login userid right padded with space
         * 31 - 38 |  8 | file date                    | optional - date file was created (MMddyyyy).
         */
        #endregion
        protected String path;
        protected String hdr_const;
        protected int pos;

        public int record_count;
        public String rate_type;
        public String userid;

        public bool valid_rate;
        public bool valid_user;
        public bool hasDate;

        public bool incomplete;
        public int actual_count;
        public DateTime filedate;

        public bool HasDate
        {
            get { return hasDate; }
        }

        public DateTime FileDate
        {
            get { return filedate; }
        }

        public bool isHeader()
        {
            return ((hdr_const != null && hdr_const.Equals("HDR")) & record_count >= 0);
        }

        public string toHeader()
        {
            if (!isHeader()) return null;

            StringBuilder sb = new StringBuilder();
            sb.Append(hdr_const);
            sb.Append(record_count.ToString("000000"));
            if (rate_type.Length > 0)
            {
                sb.Append(rate_type);
            }
            if (userid.Length > 0)
            {
                sb.Append(userid.PadRight(20, ' '));
            }
            if (hasDate)
            {
                sb.Append(filedate.ToString("MMddyyyy"));
            }

            return sb.ToString();
        }

        public bool isHeader(String buff)
        {
            try
            {
                String s = this.toHeader();
                return (isHeader() & buff.Substring(0, s.Length).Equals(s));
            }
            catch { }
            return false;
        }

        private void getFileDate(String buff)
        {
            hasDate = false;
            if (buff.Length > pos)
            {
                try
                {
                    filedate = DateTime.Parse(buff.Substring(pos, 2) + "/" + buff.Substring(pos + 2, 2) + "/" + buff.Substring(pos + 4, 4));
                    hasDate = true;
                }
                catch
                { }
            }

            if (!hasDate)
            {
                filedate = new csFileIo().getCreationDate(path);
            }

            pos += ((hasDate) ? filedate.ToString("MMddyyyy").Length : 0);
        }

        private void getUserId(String buff)
        {
            userid = String.Empty;
            valid_user = false;
            if (buff.Length > pos)
            {
                try
                {
                    userid = buff.Substring(pos, 20);
                    valid_user = true;
                }
                catch
                { }
            }
            pos += userid.Length;
        }

        private void getRateType(String buff)
        {
            rate_type = String.Empty;
            valid_rate = false;
            if (buff.Length > pos)
            {
                try
                {
                    rate_type = buff.Substring(pos, 1);
                    valid_rate = true;
                }
                catch
                { }
            }
            pos += rate_type.Length;
        }

        protected virtual void parse_header(String buff)
        {
            hdr_const = (buff.Length > 3 && buff.Substring(0, 3).Equals("HDR")) ? hdr_const = "HDR" : String.Empty;
            try
            {
                record_count = int.Parse(buff.Substring(3, 6).Trim());
                pos = 9;
            }
            catch
            {
                record_count = -1;
                pos = 3;
            }

            getRateType(buff);
            getUserId(buff);
            getFileDate(buff);
        }

        protected virtual void parsing_exit(String buff)
        {
            return;
        }

        protected virtual void parse_header_exit(String buff)
        {
            return;
        }

        protected virtual int parse_header()
        {
            bool first = true;
            actual_count = 0;

            csFileIo tio = new csFileIo();
            if (tio.open_input(path, (int)csFileIo.access_modes.TEXT))
            {
                while (!tio.EOF)
                {
                    String buff = tio.readLine();
                    if (buff != null && buff.Length > 0)
                    {
                        if (buff.Length == 1 && Encoding.Default.GetBytes(buff)[0] == 26) continue;

                        if (first)
                        {
                            parse_header(buff);
                            parse_header_exit(buff);
                            first = false;
                            if (!isHeader()) break;
                        }
                        else
                        {
                            if (!isHeader(buff)) actual_count++;
                            parsing_exit(buff);
                        }
                    }
                }
                tio.close_input();
            }
            tio = null;

            return actual_count;
        }

        public virtual bool check_header(out String message)
        {
            message = String.Empty;
            incomplete = false;

            int count = parse_header();

            if (! isHeader())
            {
                message = "Unable to read header info";
                return false;
            }

            if (count != record_count)
            {
                incomplete = (count != record_count);
                message = "Record count not equals header count";
                return false;
            }

            return true;
        }

        public defaultFileHeader(String path)
        {
            this.path = path;
        }

        public defaultFileHeader(int record_count)
        {
            this.record_count = record_count;
            this.rate_type = String.Empty;
            this.userid = String.Empty;
            this.hasDate = false;
            this.valid_rate = false;
            this.valid_user = false;
        }

        public defaultFileHeader(int record_count, String rate_type, String userid, DateTime filedate)
        {
            this.record_count = record_count;
            this.rate_type = rate_type;
            this.userid = userid;
            this.filedate = filedate;
            this.hasDate = true;
            this.valid_user = true;
            this.valid_rate = true;
        }

        ~defaultFileHeader()
        {
            path = null;
            hdr_const = null;
            rate_type = null;
            userid = null;
        }

    }
}
