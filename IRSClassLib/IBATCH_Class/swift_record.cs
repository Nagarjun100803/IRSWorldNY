using System;
using System.Collections;
using System.Data;
using System.Text;



namespace IRSClassLib
{
    public class swift_record 
    {
        #region File layout from parse_swftfile
        /*
	    select	out_seqno	=	substring( text_data,	1,	5),
		    rembank_cd	=	dbo.func_swft_getcorrbank_cd (substring( text_data, 198, 12), '', '', substring( text_data, 40, 3), 1),
		    protbank_cd	=	dbo.func_swft_getcorrbank_cd ('', substring( text_data, 280, 12), substring( text_data, 210, 12), substring( text_data, 40, 3), 2),
		    --Start RFS# 07262-001 and 07254-001
        	    --Author: Andrew
        	    --Date:   09/26/2007
		    rem_name	=	ltrim(rtrim(substring( text_data,	58,	35))),
		    --End RFS# 07262-001 and 07254-001
		    fx_currcd	=	substring( text_data,	40,	3),
		    fx_amount	=	convert( numeric (18, 2), convert( numeric (18, 2), substring( text_data,	43,	15))	/	100),		
		    trxnref_num	=	substring( text_data,	18,	16),		
		    acct_num	=	dbo.func_clean_acctno(substring( text_data,	521,	20)),		
		    div_cd		=	'2',
		    orig_cd		=	'1',
		    source_cd	=	'5',		
		    date_sent	=	convert( datetime, substring( text_data,	8,	2) + '/' + substring( text_data,	10,	2) + '/' + substring( text_data,	6,	2)),
		    date_recv	=	convert( datetime, substring( text_data,	14,	2) + '/' + substring( text_data,	16,	2) + '/' + substring( text_data,	12,	2)),
		    value_date	=	convert( datetime, substring( text_data,	36,	2) + '/' + substring( text_data,	38,	2) + '/' + substring( text_data,	34,	2)),
		    rembank_relcd	=	case when dbo.func_swft_getcorrbank_cd ('', substring( text_data, 280, 12), substring( text_data, 210, 12), substring( text_data, 40, 3), 2) = '0000000000' then
						    '3'
					    else
						    '4'
					    end,
		    due_cd		=	isnull(case when dbo.func_swft_getcorrbank_cd ('', substring( text_data, 280, 12), substring( text_data, 210, 12), substring( text_data, 40, 3), 2) = '0000000000' then
						    (select top 1 fld_due_cd from tbl_grs_corrbankdtl where fld_corrbank_cd = dbo.func_swft_getcorrbank_cd (substring( text_data, 198, 12), '', '', substring( text_data, 40, 3), 1) and fld_curr_cd = substring( text_data, 40, 3))
					    when dbo.func_swft_getcorrbank_cd ('', substring( text_data, 280, 12), substring( text_data, 210, 12), substring( text_data, 40, 3), 2) <> '0000000000' then
						    (select top 1 fld_due_cd from tbl_grs_corrbankdtl where fld_corrbank_cd = dbo.func_swft_getcorrbank_cd ('', substring( text_data, 280, 12), substring( text_data, 210, 12), substring( text_data, 40, 3), 2) and fld_curr_cd = substring( text_data, 40, 3))
					    else
						    '0'
					    end, '0'),
		    rate_cd		=	case when substring( text_data, 40, 3) = 'PHP' then '0' else '3' end,
		    benf_lname	=	ltrim( substring( substring( text_data,	555,	29), charindex( ' ', substring( text_data,	555,	29)), len(substring( text_data,	555,	29)))),
		    benf_fname	=	left( substring( text_data,	555,	29), case when (charindex( ' ', substring( text_data,	555,	29)) - 1) > 0 then charindex( ' ', substring( text_data,	555,	29)) - 1 else 0 end),
		    benf_midinit	=	substring( text_data,	583,	1),
		    benf_address1	=	replace(replace( replace( substring( text_data,	590,	25), char (9), ''), char (10), ''), char (13), ''),
		    benf_address2	=	replace(replace( replace( substring( text_data,	615,	25), char (9), ''), char (10), ''), char (13), ''),
		    benf_address3	=	replace(replace( replace( substring( text_data,	640,	25), char (9), ''), char (10), ''), char (13), ''),
		    benf_name	=	substring( text_data,	555,	35),
		    allot		=	'',
		    sender_lname	=	'',
		    sender_fname	=	'',
		    sender_mi	=	'',				
		    flag		=	'1',
		    kbi_acctname	=	Null,
		    kbi_status	=	Null,
		    kbi_currency	=	Null,
		    --Start RFS# 07262-001 and 07254-001
        	    --Author: Andrew
        	    --Date:   09/26/2007
		    rem_address	=	ltrim(rtrim(ltrim(substring( text_data,	93,	35))) + ' ' + rtrim(ltrim(substring( text_data,	128,	35))) + ' ' + rtrim(ltrim(substring( text_data,	163,	35)))),--substring( text_data,	129,	50),
		    --End RFS# 07262-001 and 07254-001
		    paybank_name	=	case when charindex('PHILIPPINE NATIONAL BANK', substring ( text_data, 380, 120)) > 0 then ''
					         when charindex('PNBMPHMM', substring ( text_data, 380, 120)) > 0 then ''
					    else
						    ltrim(rtrim(substring( text_data,	380,	120)))
					    end,
		    rem_acctnum	=	rtrim(ltrim(substring( text_data,	700,	35))),
		    rem_birthplace	=	rtrim(ltrim(substring( text_data,	735,	35))),
		    rem_birthdate	=	(
					    case when rtrim(ltrim(substring( text_data,	770,	35))) <> '' then
						    (
						    case when isdate(substring(rtrim(ltrim(substring( text_data,	770,	35))), 5, 2) + '/' + substring(rtrim(ltrim(substring( text_data,	770,	35))), 7, 2) + '/' + substring(rtrim(ltrim(substring( text_data,	770,	35))), 1, 4)) = 1 then
							    convert( datetime, substring(rtrim(ltrim(substring( text_data,	770,	35))), 5, 2) + '/' + substring(rtrim(ltrim(substring( text_data,	770,	35))), 7, 2) + '/' + substring(rtrim(ltrim(substring( text_data,	770,	35))), 1, 4), 101)
						    else	Null	end
						    )
					    else	Null	end
					    ),
		    rem_1oinfo	=	case when rtrim(ltrim(substring ( text_data, 805, 35))) <> '' then rtrim(ltrim(substring ( text_data, 805, 35))) else Null end,
		    rem_2oinfo	=	case when rtrim(ltrim(substring ( text_data, 840, 71))) <> '' then rtrim(ltrim(substring ( text_data, 840, 71))) else Null end,		
		    pnbr_cd		=	null,
		    imp_cd		=	null,
		    acct_type	=	null,
		    pay_mode	=	null
         * 
         * 
	        [Out_SeqNo] [char] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	        [RemBank_Cd] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [ProtBank_Cd] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Rem_Name] [varchar] (140) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [FX_CurrCd] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [FX_Amount] [decimal](19, 4) NOT NULL CONSTRAINT [DF_tbl_GRS_Swftout_FX_Amount] DEFAULT (0),
	        [TrxnRef_Num] [varchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Acct_Num] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Div_Cd] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Orig_Cd] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Source_Cd] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Date_Sent] [datetime] NULL ,
	        [Date_Received] [datetime] NULL ,
	        [Value_Date] [datetime] NULL ,
	        [RemBank_RelCd] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Due_Cd] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Rate_Cd] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Benf_Lname] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Benf_Fname] [varchar] (80) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Benf_Midinit] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Benf_Address1] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Benf_Address2] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Benf_Address3] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [fld_FullName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Allot] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Sndr_Lname] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Sndr_Fname] [varchar] (14) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Sndr_Midinit] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [Flag] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [KBI_AcctNm] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [KBI_Acct_Status] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [KBI_Acct_Curr] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [rem_address] [varchar] (110) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [paybank_name] [varchar] (120) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [rem_acctnum] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [rem_birthplace] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [rem_birthdate] [datetime] NULL ,
	        [rem_1oinfo] [varchar] (35) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [rem_2oinfo] [varchar] (71) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [pnbr_cd] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [imp_cd] [varchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [acct_type] [varchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [pay_mode] [varchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	        [file_nm] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
         * */
        #endregion

        internal ArrayList errmsg;
        private irsSqlCommons com;

        internal String out_seqno;
        internal String rembank_cd;
        public String protbank_cd;
        internal String rem_name;
        internal String fx_currcd;
        internal decimal fx_amount;
        internal String trxnref_num;
        internal String acct_num;
        internal String div_cd;
        internal String orig_cd;
        internal String source_cd;
        internal DateTime date_sent;
        internal DateTime date_recv;
        internal DateTime value_date;
        internal String rembank_relcd;
        internal String due_cd;
        internal String rate_cd;
        internal String benf_lname;
        internal String benf_fname;
        internal String benf_midinit;
        internal String benf_address1;
        internal String benf_address2;
        internal String benf_address3;
        internal String benf_name;
        internal String allot;
        internal String sender_lname;
        internal String sender_fname;
        internal String sender_mi;
        internal String flag;
        internal String kbi_acctname;
        internal String kbi_status;
        internal String kbi_currency;
        internal String rem_address;
        internal String paybank_name;
        internal String rem_acctnum;
        internal String rem_birthplace;
        internal DateTime rem_birthdate;
        internal String rem_1oinfo;
        internal String rem_2oinfo;
        internal String pnbr_cd;
        internal String imp_cd;
        internal String acct_type;
        internal String pay_mode;

        internal bool has_bad_amount;
        internal bool has_invalid_seqno
        {
            get { return (out_seqno.Trim().Length < 1); }
        }
        internal bool has_invalid_datesent;
        internal bool has_invalid_daterecv;
        internal bool has_invalid_currency;
        internal bool is_valid
        {
            get { return (errmsg != null && errmsg.Count == 0); }
        }

        public string Prot_Bank_Cd
        {
            get { return protbank_cd; }
        }

        internal String getColumnList()
        {
            return 
	            "out_seqno, " +
		        "rembank_cd, " +
		        "protbank_cd, " +
		        "rem_name, " +
		        "fx_currcd, " +
		        "fx_amount, " +
		        "trxnref_num, " + 
		        "acct_num, " + 
		        "div_cd, " +
		        "orig_cd, " +
		        "source_cd, " +
		        "date_sent, " +
		        "date_received, " +
		        "value_date, " +
		        "rembank_relcd, " +
		        "due_cd, " +
		        "rate_cd, " +
		        "benf_lname, " +
		        "benf_fname, " +
		        "benf_midinit, " +
		        "benf_address1, " +
		        "benf_address2, " +
		        "benf_address3, " +
                "fld_FullName, " +  // benf_name
		        "allot, " +
                "Sndr_Lname, " +
                "Sndr_Fname, " +
                "Sndr_Midinit, " +
		        "flag, " +
                "KBI_AcctNm, " +
                "KBI_Acct_Status, " +
                "KBI_Acct_Curr, " +
		        "rem_address, " +
		        "paybank_name, " +
		        "rem_acctnum, " +
		        "rem_birthplace, " +
		        "rem_birthdate, " +
                "rem_1oinfo, " +
		        "rem_2oinfo, " +
		        "pnbr_cd, " +
		        "imp_cd, " +
		        "acct_type, " +
		        "pay_mode";
        }

        internal String getValueList()
        {
            return
                "'" + out_seqno + "', " +
                "'" + rembank_cd + "', " +
                "'" + protbank_cd + "', " +
                "'" + rem_name + "', " +
                "'" + fx_currcd + "', " +
                fx_amount.ToString("#############.00") + ", " +
                "'" + trxnref_num + "', " +
                "'" + acct_num + "', " +
                "'" + div_cd + "', " +
                "'" + orig_cd + "', " +
                "'" + source_cd + "', " +
                "'" + date_sent.ToString("MM/dd/yy") + "', " +
                "'" + date_recv.ToString("MM/dd/yy") + "', " +
                "'" + value_date.ToString("MM/dd/yy") + "', " +
                "'" + rembank_relcd + "', " +
                "'" + due_cd + "', " +
                "'" + rate_cd + "', " +
                "'" + benf_lname + "', " +
                "'" + benf_fname + "', " +
                "'" + benf_midinit + "', " +
                "'" + benf_address1 + "', " +
                "'" + benf_address2 + "', " +
                "'" + benf_address3 + "', " +
                "'" + benf_name + "', " +
                "'" + allot + "', " +
                "'" + sender_lname + "', " +
                "'" + sender_fname + "', " +
                "'" + sender_mi + "', " +
                "'" + flag + "', " +
                "NULL, " + // kbi_acctname
                "NULL, " + // kbi_status
                "NULL, " + // kbi_currency 
                "'" + rem_address + "', " +
                "'" + paybank_name + "', " +
                "'" + rem_acctnum + "', " +
                "'" + rem_birthplace + "', " +
                ((rem_birthdate.Equals(DateTime.MinValue)) ? "NULL, " : "'" + rem_birthdate.ToString("MM/dd/yyyy") + "', ") +
                ((rem_1oinfo == null) ? "NULL, " : "'" + rem_1oinfo + "', ") +
                ((rem_2oinfo == null) ? "NULL, " : "'" + rem_2oinfo + "', ") +
                "NULL, " + // pnbr_cd
                "NULL, " + // imp_cd
                "NULL, " + // acct_type
                "NULL"; // pay_mode
        }

        private void set_defaults()
        {
            div_cd = "2";
            orig_cd = "1";
            source_cd = "5";
            allot = String.Empty;
            sender_lname = String.Empty;
            sender_fname = String.Empty;
            sender_mi = String.Empty;
            flag = "1";
            kbi_acctname = null;
            kbi_status = null;
            kbi_currency = null;
            pnbr_cd	= null;
		    imp_cd =	null;
		    acct_type =	null;
            pay_mode = null;
        }

        private String getCurrency(String s)
        {
            has_invalid_currency = false;

            try
            {
                int i = (int)com.execScalar("SELECT COUNT(*) FROM  tbl_lookup where	fldgroup = 'ISO4217Currency' and FldCode = '" + s + "'");
                has_invalid_currency = (i < 1);

                if (has_invalid_currency) errmsg.Add(out_seqno + ": " + s + " invalid currency");
            }
            catch (Exception e)
            {
                errmsg.Add("getCurrency: " + s + " " + e.Message);
                has_invalid_currency = true;
            }

            return s;
        }

        private String getCorrBankCode(String swift_bankcd, String swift_protbankcd1, String swift_protbankcd2, String swift_currcd, String function)
        {
            try
            {
                Object o = com.execScalar
                    (
                        "SELECT dbo.func_swft_getcorrbank_cd (" +
                        "'" + swift_bankcd + "', " +
                        "'" + swift_protbankcd1 + "', " +
                        "'" + swift_protbankcd2 + "', " +
                        "'" + swift_currcd + "', " +
                        "'" + function + "')"
                    );
                return (String)o;
            }
            catch (Exception e)
            {
                errmsg.Add("Get Corr Bank: " + e.Message);
            }
            return "0000000000";
        }

        private decimal toAmount(String samt)
        {
            has_bad_amount = false;
            try
            {
                decimal damt = decimal.Parse(samt.Insert(13, "."));
                return damt;
            }
            catch (Exception e)
            {
                errmsg.Add(out_seqno + " Amount: Error parsing amount " + samt + " " + e.Message);
            }
            has_bad_amount = true;
            return 0.00M;
        }

        private String cleanAccount(String acctno)
        {
            try
            {
                return (String)com.execScalar("SELECT dbo.func_clean_acctno('" + acctno + "')");
            }
            catch (Exception e)
            {
                errmsg.Add("Clean Acctno: " + e.Message + " " + acctno);
            }
            return acctno;
        }

        private DateTime yymmdd_toDate(String sdate)
        {
            try
            {
                DateTime dt = DateTime.Parse(sdate.Substring(2, 2) + "/" + sdate.Substring(4, 2) + "/" + sdate.Substring(0, 2));
                return dt;
            }
            catch (Exception e)
            {
                errmsg.Add("To Date: " + e.Message + " " + sdate);
            }
            return DateTime.MinValue;
        }

        private String getDueCode(String protcd, String remcd, String curcd)
        {
            try
            {
                String cd = (protcd.Equals("0000000000")) ? remcd : protcd;
                String s = (String)com.execScalar("select top 1 fld_due_cd from tbl_grs_corrbank nolock where Corr_Bank_Code = '" + cd + "' and fld_curr_cd = '" + curcd + "'");
                if (s != null && s != String.Empty) return s;
            }
            catch (Exception e)
            {
                errmsg.Add("Due Code: " + e.Message);
            }
            return "0";
        }

        private String getLastName(String s)
        {
		    //benf_lname	=	ltrim( substring( substring( text_data,	555,	29), 
            //                  charindex( ' ', substring( text_data,	555,	29)), 
            //                  len(substring( text_data,	555,	29)))),
            int i = s.IndexOf(' ');
            return s.Substring(((i == -1) ? 0 : i + 1)); 
        }

        private String getFirstName(String s)
        {
            /*
	        benf_fname	=	left( substring( text_data,	555,	29), 
             *              case 
             *                  when (charindex( ' ', substring( text_data,	555,	29)) - 1) > 0 
             *                  then charindex( ' ', substring( text_data,	555,	29)) - 1 else 0 end),
             * */
            int i = s.IndexOf(' ');
            if (i > 0) return s.Substring(0, (i));
            return String.Empty;
        }

        private String getPayBankName(String s)
        {
            /*
		    paybank_name	=	
             * case when charindex('PHILIPPINE NATIONAL BANK', substring ( text_data, 380, 120)) > 0 then ''
				        when charindex('PNBMPHMM', substring ( text_data, 380, 120)) > 0 then ''
				    else
					    ltrim(rtrim(substring( text_data,	380,	120)))
				    end,
            */
            if (s.Contains("PHILIPPINE NATIONAL BANK") | s.Contains("PNBMPHMM")) return String.Empty;
            return s.Trim();
        }

        private DateTime getBirthDate(String s)
        {
            if (s.Trim() == String.Empty) return DateTime.MinValue;
            try
            {
                String sdt = s.Substring(4, 2) + "/" + s.Substring(6, 2) + "/" + s.Substring(0, 4);
                return DateTime.Parse(sdt);
            }
            catch { }
            return DateTime.MinValue;
        }

        private void parse_swift_record(String text_data)
        {
            set_defaults();
            out_seqno = text_data.Substring(0, 5);
            fx_currcd = getCurrency(text_data.Substring(39, 3));
            fx_amount = toAmount(text_data.Substring(42, 15));
            rembank_cd = getCorrBankCode(text_data.Substring(197, 12), "", "", fx_currcd, "1");
            protbank_cd = getCorrBankCode("", text_data.Substring(279, 12), text_data.Substring(209, 12), fx_currcd, "2");
            
            rem_name = text_data.Substring(57, 35).Trim();
            trxnref_num = text_data.Substring(17, 16);
            acct_num = cleanAccount(text_data.Substring(519, 20));
            date_sent = yymmdd_toDate(text_data.Substring(5, 6));
            has_invalid_datesent = (date_sent.CompareTo(DateTime.MinValue) == 0);
            date_recv = yymmdd_toDate(text_data.Substring(11, 6));
            has_invalid_daterecv = (date_recv.CompareTo(DateTime.MinValue) == 0);
            value_date = yymmdd_toDate(text_data.Substring(33, 6));
            rembank_relcd = (protbank_cd.Equals("0000000000")) ? "3" : "4";
            protbank_cd = (rembank_relcd == "3")? rembank_cd  : protbank_cd ;
            due_cd = "";//getDueCode(protbank_cd, rembank_cd, fx_currcd);
            rate_cd = (text_data.Substring(39, 3).Equals("PHP")) ? "0" : "3";
            benf_lname = getLastName(text_data.Substring(554, 29));
            benf_fname = getFirstName(text_data.Substring(554, 29));
            benf_midinit = text_data.Substring(582, 1);
            benf_address1 = text_data.Substring(589, 25).Replace("\t", "").Replace("\r", "").Replace("\n", "");
            benf_address2 = text_data.Substring(614, 25).Replace("\t", "").Replace("\r", "").Replace("\n", "");
            benf_address3 = text_data.Substring(639, 25).Replace("\t", "").Replace("\r", "").Replace("\n", "");
            benf_name = text_data.Substring(554, 35);
            rem_address = String.Concat(text_data.Substring(92, 35).Trim(), " ", text_data.Substring(127, 35).Trim(), " ", text_data.Substring(162, 35).Trim()).Trim();
            paybank_name = getPayBankName(text_data.Substring(379, 120));
            rem_acctnum = text_data.Substring(699, 35).Trim();
            rem_birthplace = text_data.Substring(734, 35);
            rem_birthdate = getBirthDate(text_data.Substring(770, 35));
            rem_1oinfo = (text_data.Substring(804, 35).Trim() == String.Empty) ? null : text_data.Substring(804, 35).Trim();
            rem_2oinfo = (text_data.Substring(839, 71).Trim() == String.Empty) ? null : text_data.Substring(839, 71).Trim();
        }

        public swift_record(String text_data)
        {
            errmsg = new ArrayList();
            com = new irsSqlCommons();
            try
            {
                parse_swift_record(text_data);
            }
            catch (Exception e)
            {
                errmsg.Add("Error parsing data: " + e.Message);
            }
        }

        ~swift_record()
        {
            errmsg = null;
            com = null;
        }
    }
}
