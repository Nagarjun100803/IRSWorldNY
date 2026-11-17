using System;
using System.Collections;
using System.IO;
using System.Text;

namespace IRSClassLib
{
    public class mt103_decoder
    {
        #region Memory Stream Class
        private class memio
        {
            internal String Message;

            private MemoryStream ms;
            private bool eof;
            internal bool EOF
            {
                get { return eof; }
            }

            internal bool is_initialized
            {
                get { return (ms != null); }
            }

            internal bool setPosition(int pos)
            {
                if (is_initialized)
                {
                    eof = (pos >= ms.Length -1);
                    if (pos < ms.Length)
                    {
                        ms.Position = pos;
                        return true;
                    }
                }
                return false;
            }

            internal bool writeLn(String buff)
            {
                if (!is_initialized) return false;
                try
                {
                    byte[] b = Encoding.Default.GetBytes(buff + Environment.NewLine);
                    ms.Write(b, 0, b.Length);
                    return true;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }

                return false;
            }

            internal String readLn()
            {
                if (eof) return null;

                byte[] bb = new byte[4096];
                bool cr = false;
                int ix = 0;

                while (true)
                {
                    int i = ms.ReadByte();
                    if (i == -1)
                    {
                        eof = true;
                        break;
                    }

                    byte b = Convert.ToByte(i);
                    if (b == '\n' & cr) break;
                    if (b == '\r') cr = true;

                    bb[ix] = b;
                    ix++;
                }

                if (!eof)
                {
                    return Encoding.Default.GetString(bb, 0, ix - 1);
                }
                return null;
            }

            internal void close()
            {
                ms.Close();
                ms.Dispose();
                ms = null;
            }

            internal memio()
            {
                ms = new MemoryStream();
                eof = false;
            }

            ~memio()
            {
                if (ms != null) this.close();
                this.Message = null;
            }
        }
        #endregion
        private memio tempfle;

        internal int record_count;

        internal bool has_record;
        internal bool is_complete;
        internal bool is_corrupt;
        internal int read_count;

        internal String parse_message;

        #region Swift tags
        internal const String TAG_BEGIN = "{1:F2";
        internal const String TAG_20 = ":20:";
        internal const String TAG_23B = ":23B:";
        internal const String TAG_32A = ":32A:";
        internal const String TAG_50 = ":50:";
        internal const String TAG_50A = ":50A:";
        internal const String TAG_50F = ":50F:";
        internal const String TAG_50K = ":50K:";
        internal const String TAG_53A = ":53A:";
        internal const String TAG_53B = ":53B:";
        internal const String TAG_53D = ":53D:";
        internal const String TAG_54A = ":54A:";
        internal const String TAG_54B = ":54B:";
        internal const String TAG_54D = ":54D:";
        internal const String TAG_57A = ":57A:";
        internal const String TAG_57B = ":57B:";
        internal const String TAG_57C = ":57C:";
        internal const String TAG_57D = ":57D:";
        internal const String TAG_59 = ":59:";
        internal const String TAG_59A = ":59A:";

        internal const String TAG_END = "-}{5";
        #endregion
        private String tag;

        #region work area inner class
        private class workarea
        {
            internal String out_seqno;
            internal String rem_bankcd;
            internal String trxnrefnum;
            internal String value_date;
            internal String currcd;
            internal String amount;
            internal String rem_name_1;
            internal String rem_name_2;
            internal String rem_name_3;
            internal String rem_name_4;
            internal String rem_bdate;
            internal String rem_bplace;
            internal String rem_acctno;
            internal String cov_bankcd;
            internal String prot_bankcd;
            internal String pay_bank_1;
            internal String pay_bank_2;
            internal String pay_bank_3;
            internal String pay_bank_4;
            internal String pay_bank_5;
            internal String acct_num;
            internal String benf_name;
            internal String benf_addr1;
            internal String benf_addr2;
            internal String benf_addr3;
            internal String other_info_1;
            internal String other_info_2;
            internal String date_sent;
            internal String date_recv;

            public override string ToString()
            {
                date_sent = (value_date == null) ? "".PadRight(6, ' ') : value_date.PadRight(6, ' ');
                date_recv = (value_date == null) ? "".PadRight(6, ' ') : value_date.PadRight(6, ' '); 

                String rem_name = 
                    ((rem_name_1 == null) ? "".PadRight(35, ' ') : rem_name_1.PadRight(35, ' ')) +
                    ((rem_name_2 == null) ? "".PadRight(35, ' ') : rem_name_2.PadRight(35, ' ')) +
                    ((rem_name_3 == null) ? "".PadRight(35, ' ') : rem_name_3.PadRight(35, ' ')) +
                    ((rem_name_4 == null) ? "".PadRight(35, ' ') : rem_name_4.PadRight(35, ' '));

                String pay_bank =
                    ((pay_bank_1 == null) ? "".PadRight(35, ' ') : pay_bank_1.PadRight(35, ' ')) + 
                    ((pay_bank_2 == null) ? "".PadRight(35, ' ') : pay_bank_2.PadRight(35, ' ')) + 
                    ((pay_bank_3 == null) ? "".PadRight(35, ' ') : pay_bank_3.PadRight(35, ' ')) + 
                    String.Concat
                    (
                        ((pay_bank_4 == null) ? "" : pay_bank_4),
                        ((pay_bank_5 == null) ? "" : pay_bank_5)
                    ).PadRight(35, ' ');

                String benf_info = 
                    ((benf_name == null) ? "".PadRight(35, ' ') : benf_name.PadRight(35, ' ')) + 
                    ((benf_addr1 == null) ? "".PadRight(35, ' ') : benf_addr1.PadRight(35, ' ')) + 
                    ((benf_addr2 == null) ? "".PadRight(35, ' ') : benf_addr2.PadRight(35, ' ')) + 
                    ((benf_addr3 == null) ? "".PadRight(35, ' ') : benf_addr3.PadRight(35, ' '));

                if (prot_bankcd == null || prot_bankcd == String.Empty) prot_bankcd = " ";
                if (cov_bankcd == null || cov_bankcd == String.Empty) cov_bankcd = " ";

                return
                    ((out_seqno == null) ? "".PadRight(5, ' ') : out_seqno.PadRight(5, ' ')) +          //  1   -   5
                    ((date_sent == null) ? "".PadRight(6, ' ') : date_sent.PadRight(6, ' ')) +          //  6   -   11
                    ((date_recv == null) ? "".PadRight(6, ' ') : date_recv.PadRight(6, ' ')) +          //  12  -   17
                    ((trxnrefnum == null) ? "".PadRight(16, ' ') : trxnrefnum.PadRight(16, ' ')) +      //  18  -   33
                    ((value_date == null) ? "".PadRight(6, ' ') : value_date.PadRight(6, ' ')) +        //  34  -   39
                    ((currcd == null) ? "".PadRight(3, ' ') : currcd.PadRight(3, ' ')) +                //  40  -   42
                    ((amount == null) ? "".PadLeft(15, '0') : amount.PadLeft(15, '0')) +                 //  43  -   57
                    ((rem_name == null) ? "".PadRight(140, ' ') : rem_name.PadRight(140, ' ')) +        //  58  -   197
                    ((rem_bankcd == null) ? "".PadRight(12, ' ') : rem_bankcd.PadRight(12, ' ')) +      //  198 -   209
                    ((cov_bankcd == null) ? "".PadRight(70, ' ') : cov_bankcd.PadRight(70, ' ')) +      //  210 -   279
                    ((prot_bankcd == null) ? "".PadRight(100, ' ') : prot_bankcd.PadRight(100, ' ')) +  //  280 -   379
                    ((pay_bank == null) ? "".PadRight(140, ' ') : pay_bank.PadRight(140, ' ')) +        //  380 -   519  
                    ((acct_num == null) ? "".PadRight(35, ' ') : acct_num.PadRight(35, ' ')) +          //  520 -   554
                    ((benf_info == null) ? "".PadRight(140, ' ') : benf_info.PadRight(140, ' ')) +      
                    "".PadRight(5, ' ') + // tvExtra                                                    
                    ((rem_acctno == null) ? "".PadRight(35, ' ') : rem_acctno.PadRight(35, ' ')) +          
                    ((rem_bplace == null) ? "".PadRight(35, ' ') : rem_bplace.PadRight(35, ' ')) +
                    ((rem_bdate == null) ? "".PadRight(35, ' ') : rem_bdate.PadRight(35, ' ')) +
                    ((other_info_1 == null) ? "".PadRight(35, ' ') : other_info_1.PadRight(35, ' ')) +
                    ((other_info_2 == null) ? "".PadRight(71, ' ') : other_info_2.PadRight(71, ' '));
                /*
                 * Format_cell(tvOutputSeqNo, 5, "L") & 
                 * Format_cell(tvDateSent, 6, "L") & 
                 * Format_cell(tvDateRecv, 6, "L") & _
                 * Format_cell(tvtrxnrefnum, 16, "L") & 
                 * Format_cell(tvValDate, 6, "L") & 
                 * Format_cell(tvcurrcd, 3, "L") & _
                 * Format_cell(tvAmount, 15, "R") & 
                 * Format_cell(tvRem_Name, 140, "L") & 
                 * Format_cell(tvRemBankCd, 12, "L") & 
                 * Format_cell(tvCovBankCd, 70, "L") & _
                 * Format_cell(tvProtBankCd, 100, "L") & 
                 * Format_cell(tvPayBank, 140, "L") & _
                 * Format_cell(tvAcct_Num, 35, "L") & 
                 * Format_cell(tvBenf_Info, 140, "L") & 
                 * Format_cell(tvExtra, 5, "L") & _
                 * Format_cell(F_Fld50_RemAcctNo, 35, "L") & 
                 * Format_cell(F_Fld50_RemBPlace, 35, "L") & 
                 * Format_cell(F_Fld50_RemBDate, 35, "L") & _
                 * Format_cell(F_Fld50_OtherInfo1, 35, "L") & 
                 * Format_cell(F_Fld50_OtherInfo2, 71, "L") & vbCrLf
                */
            }

            ~workarea()
            {
                out_seqno = null;
                rem_bankcd = null;
                trxnrefnum = null;
                value_date = null;
                currcd = null;
                amount = null;
                rem_name_1 = null;
                rem_name_2 = null;
                rem_name_3 = null;
                rem_name_4 = null;
                rem_bdate = null;
                rem_bplace = null;
                rem_acctno = null;
                cov_bankcd = null;
                prot_bankcd = null;
                pay_bank_1 = null;
                pay_bank_2 = null;
                pay_bank_3 = null;
                pay_bank_4 = null;
                pay_bank_5 = null;
                acct_num = null;
                benf_name = null;
                benf_addr1 = null;
                benf_addr2 = null;
                benf_addr3 = null;
                other_info_1 = null;
                other_info_2 = null;
                date_sent = null;
                date_recv = null;            
            }
        }
#endregion

        private workarea tv;

        private void parse_tag(String txt)
        {
            if (txt.StartsWith(TAG_BEGIN))
            {
                tag = TAG_BEGIN;
                return;
            }

            if (txt.StartsWith(TAG_END))
            {
                tag = TAG_END;
                return;
            }

            int i = txt.IndexOf(':', 1);
            if (i > 0)
            {
                tag = txt.Substring(0, i + 1);
                return;
            }
        }

        private void parse_error(String errmsg)
        {
            is_corrupt = true;
            parse_message = tag + ": " + errmsg;
        }

        private void parse_begin_tag(String txt)
        {
            try
            {
                tv.out_seqno = txt.Substring(79, 5);
                tv.rem_bankcd = txt.Substring(102, 12);
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag20(String txt)
        {
            try
            {
                String s = txt.Substring(tag.Length).Trim();
                if (s.Length > 12)
                {
                    tv.trxnrefnum = s.Substring(0, 12);
                }
                else
                {
                    tv.trxnrefnum = s;
                }
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag32A(String txt)
        {
            try
            {
                tv.value_date = txt.Substring(tag.Length, 6).Trim();
                tv.currcd = txt.Substring(tag.Length + 6, 3);
                tv.amount = txt.Substring(tag.Length + 9).Trim();
                if (tv.amount.Contains(","))
                {
                    tv.amount = tv.amount.Replace(',', '.');
                }
                try
                { // 9(13)v99
                    tv.amount = decimal.Parse(tv.amount).ToString("0000000000000.00");
                    tv.amount = tv.amount.Replace(".", "");
                }
                catch { }
                #region long cut
                /*
                    if (tv.amount.EndsWith(",")) tv.amount = tv.amount + "00";
                    else
                    {
                        int i = tv.amount.IndexOf(',');
                        if (i < 0) tv.amount = tv.amount + ",00";
                        else
                        {
                            String s = tv.amount.Substring(i + 1);
                            if (s.Length == 1) tv.amount = tv.amount + "0";
                            else
                            {
                                tv.amount = tv.amount.Substring(0, i) + tv.amount.Substring(i, 3);
                            }
                        }
                    }
                     * */
                #endregion
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag50(String txt)
        {
            try
            {
                tv.rem_name_1 = txt.Substring(tag.Length).Trim();
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag50A(String txt)
        {
            try
            {
                String s = txt.Substring(tag.Length).Trim();
                if (s.StartsWith("/"))
                {
                    tv.rem_acctno = s.Substring(1);
                }
                else
                {
                    tv.rem_name_1 = s.Substring(0);
                }
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag50F(String txt)
        {
            try
            {
                String s = txt.Substring(tag.Length).Trim();
                if (s.StartsWith("/"))
                {
                    tv.rem_acctno = s.Substring(1);
                }
                else
                {
                    tv.other_info_1 = s.Substring(0);
                }
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag50K(String txt)
        {
            try
            {
                String s = txt.Substring(tag.Length).Trim();
                if (s.LastIndexOf("/") > -1)
                {
                    tv.rem_acctno = s.Substring(s.LastIndexOf("/") + 1);
                }
                else
                {
                    tv.rem_name_1 = s.Substring(0);
                }
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag53A(String txt)
        {
            try
            {
                tv.cov_bankcd = txt.Substring(tag.Length).Trim();
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag54A(String txt)
        {
            try
            {
                tv.prot_bankcd = txt.Substring(tag.Length).Trim();
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag57A(String txt)
        {
            try
            {
                tv.pay_bank_1 = txt.Substring(tag.Length).Trim();
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private void parse_tag59(String txt)
        {
            try
            {
                String s = txt.Substring(tag.Length).Trim();
                tv.acct_num = (s.StartsWith("/")) ? s.Substring(1) : s;
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        private int getStartIndex(String txt)
        {
            char[] cc = txt.ToCharArray();
            bool is_digit = false;

            for (int i = 0; i < cc.Length; i++)
            {
                if (char.IsDigit(cc[i]))
                {
                    is_digit = true;
                    continue;
                }
                if (cc[i] == '/' & is_digit)
                {
                    return i + 1;
                }
                is_digit = false;
            }
            return 0;
        }

        private void process_fields50(String txt)
        {
            if (tag.Equals(TAG_50F))
            {
                switch (txt.Substring(0, 1))
                {
                    case "1":
                        tv.rem_name_1 = txt.Substring(2).Trim();
                        break;

                    case "2":
                        tv.rem_name_2 = txt.Substring(2).Trim();
                        break;

                    case "3":
                        if (tv.rem_name_2 != null)
                        {
                            tv.rem_name_3 = txt.Substring(2).Trim();
                        }
                        break;

                    case "4":
                        tv.rem_bdate = txt.Substring(2).Trim();
                        break;

                    case "5":
                        if (tv.rem_bplace != null)
                        {
                            tv.rem_bplace = txt.Substring(2).Trim();
                        }
                        break;

                    default:
                        if (tv.other_info_2 != null)
                        {
                            tv.other_info_2 = tv.other_info_2 + txt;
                        }
                        else
                        {
                            tv.other_info_2 = txt;
                        }
                        break;
                }
            }
            else
            {
                int st = getStartIndex(txt);
                if (tv.rem_name_1 == null & tv.rem_name_2 == null & tv.rem_name_3 == null & tv.rem_name_4 == null)
                {
                    tv.rem_name_1 = txt.Substring(st);
                }
                else if (tv.rem_name_1 != null & tv.rem_name_2 == null & tv.rem_name_3 == null & tv.rem_name_4 == null)
                {
                    tv.rem_name_2 = txt.Substring(st);
                }
                else if (tv.rem_name_1 != null & tv.rem_name_2 != null & tv.rem_name_3 == null & tv.rem_name_4 == null)
                {
                    tv.rem_name_3 = txt.Substring(st);
                }
                else if (tv.rem_name_1 != null & tv.rem_name_2 != null & tv.rem_name_3 != null & tv.rem_name_4 == null)
                {
                    tv.rem_name_4 = txt.Substring(st);
                }
            }
        }

        private void process_fields57(String txt)
        {
            //int st = (txt.IndexOf("/") > -1) ? txt.IndexOf("/") + 1 : 0;
            int st = getStartIndex(txt);
            if (tv.pay_bank_1 == null & tv.pay_bank_2 == null & tv.pay_bank_3 == null & tv.pay_bank_4 == null & tv.pay_bank_5 == null)
            {
                tv.pay_bank_1 = txt.Substring(st).Trim();
            }
            else if (tv.pay_bank_1 != null & tv.pay_bank_2 == null & tv.pay_bank_3 == null & tv.pay_bank_4 == null & tv.pay_bank_5 == null)
            {
                tv.pay_bank_2 = txt.Substring(st).Trim();
            }
            else if (tv.pay_bank_1 != null & tv.pay_bank_2 != null & tv.pay_bank_3 == null & tv.pay_bank_4 == null & tv.pay_bank_5 == null)
            {
                tv.pay_bank_3 = txt.Substring(st).Trim();
            }
            else if (tv.pay_bank_1 != null & tv.pay_bank_2 != null & tv.pay_bank_3 != null & tv.pay_bank_4 == null & tv.pay_bank_5 == null)
            {
                tv.pay_bank_4 = txt.Substring(st).Trim();
            }
            else if (tv.pay_bank_1 != null & tv.pay_bank_2 != null & tv.pay_bank_3 != null & tv.pay_bank_4 != null & tv.pay_bank_5 == null)
            {
                tv.pay_bank_5 = txt.Substring(st).Trim();
            }
        }

        private void process_fields59(String txt)
        {
            //int st = (txt.IndexOf("/") > -1) ? txt.IndexOf("/") + 1 : 0;
            int st = getStartIndex(txt);
            if (tv.benf_name == null & tv.benf_addr1 == null & tv.benf_addr2 == null & tv.benf_addr3 == null)
            {
                tv.benf_name = txt.Substring(st);
            }
            else if (tv.benf_name != null & tv.benf_addr1 == null & tv.benf_addr2 == null & tv.benf_addr3 == null)
            {
                tv.benf_addr1 = txt.Substring(st);
            }
            else if (tv.benf_name != null & tv.benf_addr1 != null & tv.benf_addr2 == null & tv.benf_addr3 == null)
            {
                tv.benf_addr2 = txt.Substring(st);
            }
            else if (tv.benf_name != null & tv.benf_addr1 != null & tv.benf_addr2 != null & tv.benf_addr3 == null)
            {
                tv.benf_addr3 = txt.Substring(st);
            }
        }

        private void parse_tag_none(String txt)
        {
            try
            {
                if (tag.StartsWith(":50"))
                {
                    process_fields50(txt);
                }
                else if (tag.StartsWith(":57"))
                {
                    process_fields57(txt);
                }
                else if (tag.StartsWith(":59"))
                {
                    process_fields59(txt);
                }
            }
            catch (Exception e)
            {
                parse_error(e.Message);
            }
        }

        internal void reformat(String buff)
        {
            String txt = buff.Trim();
            is_corrupt = false;
            if (txt.StartsWith("{") | txt.StartsWith(":") | txt.StartsWith("-"))
            {
                parse_tag(txt);
                switch (tag)
                {
                    case TAG_BEGIN:
                        parse_begin_tag(txt);
                        has_record = true;
                        break;

                    case TAG_20:
                        parse_tag20(txt);
                        break;

                    case TAG_23B:
                        break;

                    case TAG_32A:
                        parse_tag32A(txt);
                        break;

                    case TAG_50:
                    case TAG_50K:
                        parse_tag50K(txt);
                        break;

                    case TAG_50A:
                        parse_tag50A(txt);
                        break;

                    case TAG_50F:
                        parse_tag50F(txt);
                        break;

                    case TAG_53A:
                    case TAG_53B:
                    case TAG_53D:
                        parse_tag53A(txt);
                        break;

                    case TAG_54A:
                    case TAG_54B:
                    case TAG_54D:
                        parse_tag54A(txt);
                        break;

                    case TAG_57A:
                    case TAG_57B:
                    case TAG_57C:
                    case TAG_57D:
                        parse_tag57A(txt);
                        break;

                    case TAG_59:
                    case TAG_59A:
                        parse_tag59(txt);
                        break;

                    case TAG_END:
                        is_complete = true;
                        if (flush())
                        {
                            int i = txt.IndexOf("$");
                            if (i > -1)
                            {
                                reformat(txt.Substring(i + 1)); // process next tag
                            }
                        }
                        break;
                }
            }
            else
            {
                parse_tag_none(txt);
            }

            if (is_corrupt) throw new Exception(parse_message);
        }

        public bool flush()
        {
            if (is_complete)
            {
                if (write_record(tv.ToString()))
                {
                    is_complete = false;
                    is_corrupt = false;
                    has_record = false;
                    tv = new workarea();
                    return true;
                }
                else
                {
                    parse_error(tempfle.Message);
                }
            }
            else
            {
                parse_error("Flush Error: The record must not be corrupted or incomplete");
            }
            return false;
        }

        private bool write_record(String buff)
        {
            if (tempfle == null)
            {
                tempfle = new memio();
            }
            bool ok = tempfle.writeLn(buff);
            if (ok) record_count++;
            return ok;
        }

        internal bool EOF
        {
            get { return tempfle.EOF; }
        }

        internal String readLine()
        {
            if (read_count == 0) tempfle.setPosition(0);
            String s = tempfle.readLn();
            if (s != null) read_count++;
            return s;
        }

        internal void close()
        {
            tempfle.close();
            tv = null;
        }

        internal mt103_decoder()
        {
            record_count = 0;
            read_count = 0;
            tag = null;
            has_record = false;
            is_complete = false;
            is_corrupt = false;
            tv = new workarea();
        }

        ~mt103_decoder()
        {
            tempfle = null;
            parse_message = null;
            tv = null;
        }

    }
}
