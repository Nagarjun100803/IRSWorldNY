using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web;
using System.IO;
namespace IRSWorld.TranClass
{
    [Serializable]
    public class CrystalReportUtil : sqlHelperWs
    {
        public string ErrorMessage = "";
        string _userid = "";
        string _branchcode = "";
        // commented to remove warning - string errmsg = ""; 
        DateTime _tranxdate;

        private string otc_rpt_err = string.Empty;

        public CrystalReportUtil()
        {

            if (IRSClientData == null)
            {
                // commented to remove warning - errmsg = "SESSION EXPIRED FROM CLIENTDATA";
                new csAuditLog().writeAudit("CRYSTALU", "CRYSTALINI", "INITIALIZE", "", "", csAuditLog.Category.Email);
                throw new Exception("SESSION EXPIRED");

            }
            else
            {
                _userid = IRSClientData.UserId;
                try
                {
                    _branchcode = IRSClientData.Location;
                }
                catch
                {
                    try
                    {
                        // commented to remove warning - errmsg = "NO BRANCHCODE DEFINED";
                        new csAuditLog().writeAudit("CRYSTALU", "CRYSTALINI", "INITIALIZE", "", "", csAuditLog.Category.Email);
                    }
                    catch
                    { }
                    throw new Exception("SESSION EXPIRED");
                }
                irsTimeZone it = new irsTimeZone();
                _tranxdate = it.branchLocalTime(_branchcode);
                it = null;
            }
        }
        #region PRIVATE METHODS
        private irsClientData IRSClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }

                return new irsClientData();
            }
        }
        #endregion
        #region IRSRECEIPTS

        private bool chk_email_str(string s)
        {
            try
            {
                new System.Net.Mail.MailAddress(s); return true;
            }
            catch
            {
                return false;
            }
        }

        public Boolean OREmailSending(ReportDocument rpt, string refno, string remID, out string errmsg)
        {
            csMailer mail = new csMailer();
            irsReceipts ir = new irsReceipts();
            irsConfiguration config = new irsConfiguration();
            irsClientMaster cifmas = new irsClientMaster();

            bool issend;
            string[] filelist = null;
            string[] filesend = new string[1];
            string[] EmailAdd = null;
            string remname = "";
            string strSubject = "";
            string strMsg = "";

            if (IRSClientData == null)
            {
                errmsg = "SESSION EXPIRED FROM CLIENTDATA";
                new csAuditLog().writeAudit("OREMAIL", "OR Email", mail.Message, errmsg, "", csAuditLog.Category.Email);
                return false;
            }
            else
            {
                _userid = IRSClientData.UserId;
                _branchcode = IRSClientData.Location;
            }

            DataTable dt = null;
            int cifseq = 0;
            try
            {
                dt = ir.getCIFContacts(remID);
                dt = dt.DefaultView.ToTable(true, "EmailAddress"); /* (new) get distinct email address only. */

                if (dt != null && dt.Rows.Count > 0)
                {
                    string emeyl = string.Empty;

                    EmailAdd = new string[dt.Rows.Count];

                    for (int i2 = 0; i2 <= dt.Rows.Count - 1; i2++)
                    {
                        emeyl = string.Empty;
                        //emeyl = dt.Rows[i2][5].ToString().Trim(); /* existing */
                        emeyl = dt.Rows[i2]["EmailAddress"].ToString().Trim(); /* (new) */

                        if (this.chk_email_str(emeyl) == true)
                        {
                            EmailAdd[i2] = emeyl;
                        }
                        else
                        {
                            new csAuditLog().writeAudit(_userid, "OR Email", "Invalid email format", remID + " - email address: " + emeyl, "", csAuditLog.Category.Email);
                        }
                    }
                }
                else
                {
                    errmsg = "Remitter has no Email Address.";
                    return false;
                }

                cifseq = ir.getCIFSEQ_ID(remID);

                if (cifmas != null)
                {
                    if (cifmas.get_record(cifseq.ToString()))
                    {
                        string lastname = cifmas.getdataRecord().Tables[0].Rows[0]["LastName"].ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                        string firstname = cifmas.getdataRecord().Tables[0].Rows[0]["FirstName"].ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                        string middlename = cifmas.getdataRecord().Tables[0].Rows[0]["MiddleName"].ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                        remname = lastname + " " + firstname + " " + middlename;
                    }
                    else
                    {
                        errmsg = "Failed to get remitter details.";
                        return false;
                    }
                }

                if (System.IO.Directory.Exists(config.appsetting("ORReceipt")) == false)
                {
                    System.IO.Directory.CreateDirectory(config.appsetting("ORReceipt"));
                }
                rpt.ExportToDisk(ExportFormatType.PortableDocFormat, config.appsetting("ORReceipt") + "\\" + refno + "-" + remname + ".pdf");



                filelist = System.IO.Directory.GetFiles(config.appsetting("ORReceipt"));
                for (int i = 0; i <= filelist.Length - 1; i++)
                {
                    if (filelist[i].ToString().IndexOf(refno) > 0)
                    {
                        filesend[0] = filelist[i];
                    }
                }
                DataTable dtORLookup = ir.ORLookup();
                if (dtORLookup == null || dtORLookup.Rows.Count == 0)
                {
                    errmsg = "Failed to get O.R. Email Details.";
                    return false;
                }
                strSubject = dtORLookup.Select("FldCode = 'Subject'")[0][4].ToString().Replace("<REFNO>", refno);
                strMsg = dtORLookup.Select("FldCode = 'EmailBody1'")[0][4].ToString().Replace("<REMITTERNAME>", remname) + Environment.NewLine + Environment.NewLine;
                strMsg += dtORLookup.Select("FldCode = 'EmailBody2'")[0][4].ToString().Replace("<REFNO>", refno) + Environment.NewLine + Environment.NewLine;
                strMsg += dtORLookup.Select("FldCode = 'EmailBody3'")[0][4].ToString() + Environment.NewLine;
                strMsg += dtORLookup.Select("FldCode = 'EmailBody4'")[0][4].ToString().Replace("<Branch>", _branchcode);
                issend = mail.sendMail(strSubject, EmailAdd, strMsg, filesend);

                if (issend)
                {
                    errmsg = "Transaction receipt sent thru email.";
                    new csAuditLog().writeAudit(_userid, "OR Email", mail.Message, errmsg, "", csAuditLog.Category.Email);
                }
                else
                {
                    errmsg = "Failed to send O.R. thru Email.";
                    new csAuditLog().writeAudit(_userid, "OR Email", mail.Message, errmsg, "", csAuditLog.Category.Email);
                }

                return issend;
            }
            catch (Exception e)
            {
                errmsg = e.Message;
                new csAuditLog().writeAudit(IRSClientData.UserId, "OR Email", mail.Message, errmsg, "", csAuditLog.Category.Email);
            }
            finally
            {
                if (filesend[0] != null)
                {
                    try
                    {
                        System.IO.File.Delete(filesend[0].ToString());
                    }
                    catch { new csAuditLog().writeAudit(_userid, "OR Email", "ERRORDELETING", "filesend[0].ToString()", "", csAuditLog.Category.Email); }
                }
                if (ir != null) ir = null;
                if (mail != null) mail = null;
                if (config != null) config = null;
                if (cifmas != null) cifmas = null;

            }
            return false;
        }
        #endregion
        #region IRSOTC

        public ReportDocument generate_otc_report(string branchcode, string tranxid, out string error_message)
        {

            this.otc_rpt_err = string.Empty;

            try
            {
                return generate_otc_report(branchcode, tranxid);
            }
            catch (Exception e)
            {
                error_message = e.Message; return null;
            }
            finally
            {
                error_message = this.otc_rpt_err;
            }
        }

        public ReportDocument generate_otc_report(string branchcode, string tranxid)
        {

            int receipt_type = this.GetReceiptType(branchcode);


            if (receipt_type == 2)
            {
                return this.generate_otc_tagalog_report(branchcode, tranxid);
            }
            else if (receipt_type == 3)
            {
                return this.generate_otc_tagalog_report_rcn(branchcode, tranxid);
            }
            else if (receipt_type == 4)
            {
                return this.generate_otc_tagalog_report_guam(branchcode, tranxid);
            }
            else if (receipt_type == 6)
            {
                return this.generate_otc_tagalog_report_newyork(branchcode, tranxid);
            }
            else
            {
                return this.generate_otc_pdf_report(branchcode, tranxid);
            }
        }

        public ReportDocument generate_otc_tagalog_report_newyork(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            string rptnm = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs();

            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "proc_tagalog_report";
                rptnm = "NYReceipt.rpt";
                reportpath = rootpath + (@"\NewYork\" + rptnm);
                
                /*
                 * if (branchcode == "TEL" || branchcode == "TLN")
                {
                    sp_name = "proc_reprint_tagalog_report_irsw_web_remit";
                }
                else
                {
                    sp_name = "proc_tagalog_report";
                }

                //=========================================
                //RFC-10175 : PNB NY Phone Remittance
                bool is_phoneremit_branch = false;
                DataTable dt_phoneremit = new DataTable();

                dt_phoneremit = com.execStoredProcedureTable("isp_getLookup", new object[] { "ACHBranchExtractSetup" });
                if (dt_phoneremit != null)
                {
                    if (dt_phoneremit.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_phoneremit.Rows.Count; i++)
                        {
                            if (dt_phoneremit.Rows[i]["FldCode"].ToString() == branchcode)
                            {
                                is_phoneremit_branch = true;
                            }
                        }
                    }
                }
                //=========================================

                string rptnm = "";
                if (!this.isDotMatrix(branchcode))
                {
                    if (this.isOTCReportHalfPage(branchcode))
                    {
                        if (branchcode == "NYC")
                        {
                            rptnm = "rptReceipt_NYC.rpt";
                        }
                        //=========================================
                        //RFC-10175 : PNB NY Phone Remittance
                        //else if (branchcode == "NYI")
                        //=========================================
                        else if (is_phoneremit_branch == true)
                        {
                            rptnm = "NYPhoneRemit_Receipt.rpt";
                        }
                        else
                        {
                            rptnm = "rptReceipt_parameterize_half.rpt";
                        }

                    }
                    else
                    {
                        rptnm = "rptReceipt_parameterize.rpt";
                    }

                    //=========================================
                    //RFC-10175 : PNB NY Phone Remittance
                    //if (branchcode == "NYI")
                    //=========================================
                    if (is_phoneremit_branch == true)
                    {
                        reportpath = rootpath + (@"\NewYork\" + rptnm);
                    }
                    else
                    {
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }
                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }
                */

                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');

                dt = a.execStoredProc(sp_name, param).Tables[0];

                if (dt == null || dt.Rows.Count == 0)
                {
                    this.otc_rpt_err = "Record not found \n\n generate_otc_tagalog_report_newyork"; return null;
                }

                ReportDocument repDoc = new ReportDocument();
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid, "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select); this.otc_rpt_err = e.Message + "\n" + e.InnerException + "\n\n" + "generate_otc_tagalog_report_newyork";

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }

        public ReportDocument generate_otc_tagalog_report(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs();

            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "";
                if (branchcode == "TEL" || branchcode == "TLN")
                {
                    sp_name = "proc_reprint_tagalog_report_irsw_web_remit";
                }
                else
                {
                    sp_name = "proc_tagalog_report";
                }

                //=========================================
                //RFC-10175 : PNB NY Phone Remittance
                bool is_phoneremit_branch = false;
                DataTable dt_phoneremit = new DataTable();

                dt_phoneremit = com.execStoredProcedureTable("isp_getLookup", new object[] { "ACHBranchExtractSetup" });
                if (dt_phoneremit != null)
                {
                    if (dt_phoneremit.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_phoneremit.Rows.Count; i++)
                        {
                            if (dt_phoneremit.Rows[i]["FldCode"].ToString() == branchcode)
                            {
                                is_phoneremit_branch = true;
                            }
                        }
                    }
                }
                //=========================================

                string rptnm = "";
                if (!this.isDotMatrix(branchcode))
                {
                    if (this.isOTCReportHalfPage(branchcode))
                    {
                        if (branchcode == "NYC")
                        {
                            rptnm = "rptReceipt_NYC.rpt";
                        }
                        //=========================================
                        //RFC-10175 : PNB NY Phone Remittance
                        //else if (branchcode == "NYI")
                        //=========================================
                        else if (is_phoneremit_branch == true)
                        {
                            rptnm = "NYPhoneRemit_Receipt.rpt";
                        }
                        else
                        {
                            rptnm = "rptReceipt_parameterize_half.rpt";
                        }

                    }
                    else
                    {
                        rptnm = "rptReceipt_parameterize.rpt";
                    }

                    //=========================================
                    //RFC-10175 : PNB NY Phone Remittance
                    //if (branchcode == "NYI")
                    //=========================================
                    if (is_phoneremit_branch == true)
                    {
                        reportpath = rootpath + (@"\NewYork\" + rptnm);
                    }
                    else
                    {
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }
                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }

                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');

                dt = a.execStoredProc(sp_name, param).Tables[0];

                if (dt == null || dt.Rows.Count == 0)
                {
                    this.otc_rpt_err = "Record not found \n\n generate_otc_tagalog_report"; return null;
                }

                ReportDocument repDoc = new ReportDocument();
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid
                                            , "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select); this.otc_rpt_err = e.Message + "\n" + e.InnerException + "\n\n" + "generate_otc_tagalog_report";

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }
        public ReportDocument generate_otc_tagalog_report_rcn(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "";
                //sp_name = "TXN_reprint_tagalogOTC_irsw_RCN";
                sp_name = "proc_tagalog_report";
                string rptnm = "";

                if (!this.isDotMatrix(branchcode))
                {
                    if (branchcode == "TEL" || branchcode == "TLN")
                    {
                        sp_name = "proc_tagalog_report_TLN";
                    }
                    else
                    {
                        sp_name = "proc_tagalog_report";
                    }
                    if (this.checkars(branchcode))
                    {
                        //sp_name = "TXN_Reprint_Tagalog_Agent_RCN";
                        sp_name = "proc_tagalog_report";
                        rptnm = "arpt_Reprint_Tagalog_Agent_RCN.rpt";
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }
                    else
                    {
                        rptnm = "tagalog_OTC_RCN.rpt";
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }


                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }
                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');

                dt = a.execStoredProc(sp_name, param).Tables[0];

                if (dt == null || dt.Rows.Count == 0)
                {
                    this.otc_rpt_err = "Record not found \n\n generate_otc_tagalog_report_rcn"; return null;
                }

                ReportDocument repDoc = new ReportDocument();
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid
                                            , "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select); this.otc_rpt_err = e.Message + "\n" + e.InnerException + "\n\ngenerate_otc_tagalog_report_rcn";

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }
        public ReportDocument generate_otc_tagalog_report_guam(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "";
                sp_name = "proc_tagalog_report";
                string rptnm = "";
                if (!this.isDotMatrix(branchcode))
                {

                    rptnm = "arpt_Tagalog_Receipt_OTC_GUAM.rpt";

                    reportpath = rootpath + (@"\OTC\" + rptnm);
                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }
                //if (!this.isDotMatrix(branchcode))
                //{


                //    rptnm = "arpt_Tagalog_Receipt_OTC_GUAM.rpt";

                //    reportpath = rootpath + (@"\OTC\" + rptnm);

                //}
                //else
                //{
                //    // for dot matrix
                //    sp_name = "sp_otc_obp_Receipt_preprinted";
                //    rptnm = "rptTranxReceipt.rpt";
                //    reportpath = rootpath + (@"\OTC\" + rptnm);
                //}
                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');

                dt = a.execStoredProc(sp_name, param).Tables[0];

                if (dt == null || dt.Rows.Count == 0)
                {
                    this.otc_rpt_err = "Record not found \n\n generate_otc_tagalog_report_guam"; return null;
                }

                ReportDocument repDoc = new ReportDocument();

                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid
                                            , "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select); this.otc_rpt_err = e.Message + "\n" + e.InnerException + "\n\ngenerate_otc_tagalog_report_guam";

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }
        public ReportDocument generate_otc_pdf_report(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "sp_otc_Receipt";
                string rptnm = "";

                if (!this.isDotMatrix(branchcode))
                {
                    if (this.isOTCReportHalfPage(branchcode))
                    {
                        rptnm = "rptReceipt_Half.rpt";
                    }
                    else
                    {
                        rptnm = "rptReceipt.rpt";
                    }
                    reportpath = rootpath + (@"\OTC\" + rptnm);
                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }
                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');

                dt = a.execStoredProc(sp_name, param).Tables[0];

                if (dt == null || dt.Rows.Count == 0)
                {
                    this.otc_rpt_err = "Record not found \n\ngenerate_otc_pdf_report"; return null;
                }

                ReportDocument repDoc = new ReportDocument();

                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);

                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid
                                            , "OTC REPORT PDF", e.StackTrace, e.Message, "", csAuditLog.Category.Select); this.otc_rpt_err = e.Message + "\n" + e.InnerException + "\n\ngenerate_otc_pdf_report";

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }
        public ReportDocument generate_obp_report(string branchcode, string tranxid)
        {
            int receipt_type = this.GetReceiptType(branchcode);


            if (receipt_type == 2)
            {
                return this.generate_obp_tagalog_report(branchcode, tranxid);
            }
            else if (receipt_type == 3)
            {
                return this.generate_obp_tagalog_report_rcn(branchcode, tranxid);
            }
            else if (receipt_type == 4)
            {
                return this.generate_obp_tagalog_report_guam(branchcode, tranxid);
            }
            else if (receipt_type == 6)
            {
                return this.generate_obp_tagalog_report_newyork(branchcode, tranxid);
            }
            else
            {
                return this.generate_obp_pdf_report(branchcode, tranxid);
            }
        }

        public ReportDocument generate_obp_tagalog_report_newyork(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            string rptnm = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs();

            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "sp_otc_tagalog_obp_Receipt";
                rptnm = "NYReceipt.rpt";
                reportpath = rootpath + (@"\NewYork\" + rptnm);

                /*
                if (branchcode == "TEL" || branchcode == "TLN")
                {
                    sp_name = "sp_otc_tagalog_obp_Receipt_TEL";
                }
                else
                {
                    sp_name = "sp_otc_tagalog_obp_Receipt";
                }

                //=========================================
                //RFC-10175 : PNB NY Phone Remittance
                bool is_phoneremit_branch = false;
                DataTable dt_phoneremit = new DataTable();

                dt_phoneremit = com.execStoredProcedureTable("isp_getLookup", new object[] { "ACHBranchExtractSetup" });
                if (dt_phoneremit != null)
                {
                    if (dt_phoneremit.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_phoneremit.Rows.Count; i++)
                        {
                            if (dt_phoneremit.Rows[i]["FldCode"].ToString() == branchcode)
                            {
                                is_phoneremit_branch = true;
                            }
                        }
                    }
                }
                //=========================================

                if (!this.isDotMatrix(branchcode))
                {

                    if (this.isOTCReportHalfPage(branchcode))
                    {
                        if (branchcode == "NYC")
                        {
                            rptnm = "rptReceipt_NYC.rpt";
                        }
                        //=========================================
                        //RFC-10175 : PNB NY Phone Remittance
                        //else if (branchcode == "NYI")
                        else if (is_phoneremit_branch == true)
                        //=========================================
                        {
                            rptnm = "NYPhoneRemit_Receipt.rpt";
                        }
                        else
                        {
                            rptnm = "rptReceipt_parameterize_half_OBP.rpt";
                        }
                    }
                    else
                    {
                        rptnm = "rptReceipt_parameterize_OBP.rpt";
                    }
                    //=========================================
                    //RFC-10175 : PNB NY Phone Remittance
                    //if (branchcode == "NYI")
                    if (is_phoneremit_branch == true)
                    //=========================================
                    {
                        reportpath = rootpath + (@"\NewYork\" + rptnm);
                    }
                    else
                    {
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }

                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_obp_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }
                */

                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid, "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select);

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }

        public ReportDocument generate_obp_tagalog_report(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs();

            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "sp_otc_tagalog_obp_Receipt";
                string rptnm = "";
                if (branchcode == "TEL" || branchcode == "TLN")
                {
                    sp_name = "sp_otc_tagalog_obp_Receipt_TEL";
                }
                else
                {
                    sp_name = "sp_otc_tagalog_obp_Receipt";
                }

                //=========================================
                //RFC-10175 : PNB NY Phone Remittance
                bool is_phoneremit_branch = false;
                DataTable dt_phoneremit = new DataTable();

                dt_phoneremit = com.execStoredProcedureTable("isp_getLookup", new object[] { "ACHBranchExtractSetup" });
                if (dt_phoneremit != null)
                {
                    if (dt_phoneremit.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_phoneremit.Rows.Count; i++)
                        {
                            if (dt_phoneremit.Rows[i]["FldCode"].ToString() == branchcode)
                            {
                                is_phoneremit_branch = true;
                            }
                        }
                    }
                }
                //=========================================

                if (!this.isDotMatrix(branchcode))
                {

                    if (this.isOTCReportHalfPage(branchcode))
                    {
                        if (branchcode == "NYC")
                        {
                            rptnm = "rptReceipt_NYC.rpt";
                        }
                        //=========================================
                        //RFC-10175 : PNB NY Phone Remittance
                        //else if (branchcode == "NYI")
                        else if (is_phoneremit_branch == true)
                        //=========================================
                        {
                            rptnm = "NYPhoneRemit_Receipt.rpt";
                        }
                        else
                        {
                            rptnm = "rptReceipt_parameterize_half_OBP.rpt";
                        }
                    }
                    else
                    {
                        rptnm = "rptReceipt_parameterize_OBP.rpt";
                    }
                    //=========================================
                    //RFC-10175 : PNB NY Phone Remittance
                    //if (branchcode == "NYI")
                    if (is_phoneremit_branch == true)
                    //=========================================
                    {
                        reportpath = rootpath + (@"\NewYork\" + rptnm);
                    }
                    else
                    {
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }

                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_obp_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }

                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid
                                            , "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select);

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }
        public ReportDocument generate_obp_tagalog_report_rcn(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                //string sp_name = "TXN_reprint_tagalogOBP_irsw_RCN";
                string sp_name = "sp_otc_tagalog_obp_Receipt";
                string rptnm = "";
                if (!this.isDotMatrix(branchcode))
                {
                    if (branchcode == "TEL" || branchcode == "TLN")
                    {
                        sp_name = "sp_otc_tagalog_obp_Receipt_TLN";
                    }
                    else
                    {
                        sp_name = "sp_otc_tagalog_obp_Receipt";
                    }

                    if (this.checkars(branchcode))
                    {
                        sp_name = "sp_otc_tagalog_obp_Receipt";
                        rptnm = "rptReceipt_parameterize_OBP_Agent.rpt";
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }
                    else
                    {
                        rptnm = "tagalog_OBP_RCN.rpt";
                        reportpath = rootpath + (@"\OTC\" + rptnm);
                    }
                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_obp_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }
                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid
                                            , "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select);

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }
        public ReportDocument generate_obp_tagalog_report_guam(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            string reportpath = "";
            try
            {
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "sp_otc_tagalog_obp_Receipt";
                string rptnm = "";
                if (!this.isDotMatrix(branchcode))
                {

                    rptnm = "arpt_Tagalog_Receipt_OBP_GUAM.rpt";
                    reportpath = rootpath + (@"\OTC\" + rptnm);
                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_obp_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                    reportpath = rootpath + (@"\Standard\" + rptnm);
                }

                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {

                    ca.writeAudit(_userid
                                            , "OTC", e.StackTrace, e.Message, "", csAuditLog.Category.Select);

                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }
        public ReportDocument generate_obp_pdf_report(string branchcode, string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            csAuditLog ca = new csAuditLog();
            DataTable dt = null;
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "sp_otc_obp_Receipt";
                string rptnm;
                if (!this.isDotMatrix(branchcode))
                {
                    if (this.isOTCReportHalfPage(branchcode))
                    {
                        if (this.checkars(branchcode))
                        {
                            rptnm = "rptReceipt_ARSHalf.rpt";
                        }
                        else
                        {
                            rptnm = "rptReceipt_Half.rpt";
                        }
                    }
                    else
                    {
                        if (this.checkars(branchcode))
                        {
                            rptnm = "rptARSReceipt.rpt";
                        }
                        else
                        {
                            rptnm = "rptReceipt.rpt";
                        }
                    }
                }
                else
                {
                    // for dot matrix
                    sp_name = "sp_otc_obp_Receipt_preprinted";
                    rptnm = "rptTranxReceipt.rpt";
                }
                string values = branchcode + "," + tranxid;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];

                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();

                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string reportpath = rootpath + (@"\OTC\" + rptnm);
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {
                    ca.writeAudit(_userid
                                               , "OBP", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }

        public bool checkars(string branchcode)
        {
            DataTable dt = new DataTable();
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "proc_check_ars";
                string values = branchcode;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                ca.writeAudit(this._userid
                                               , "checkars", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                return false;
            }
            finally
            {
                dt = null;
                ca = null;
            }
        }

        public ReportDocument generate_otc_dailyreport(string tranxid)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "sp_daily_record_keep_log";
                string rptnm = "rptDailyRecKeepingLog.rpt";
                string values = tranxid;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();

                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string reportpath = rootpath + (@"\STANDARD\" + rptnm);
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {
                    ca.writeAudit(_userid
                                            , "OTCDAILY", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }

        }

        public ReportDocument GenerateInquiryReport(int FCMethod)
        {
            ReportDocument rpt = new ReportDocument();
            ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
            ParameterValues paramVal = new ParameterValues();
            string[] arrAccountBalance = { "Account Number", "Account ID", "Account Name", "Account Description", "Account Currency", "Account Status", "Activity Today", "Current Balance", "Available Balance", "Total Float", "Total Hold", "Open Date" };
            string[] arrLastTransaction = { "Account Number", "Account Name", "Header", "Value1", "Value2", "Value3", "Value4", "Value5" };
            string[] arrHold = { "Account Number", "Account ID", "Account Name", "Total Hold Amount" };
            string[] arrFloat = { "Account Number", "Account ID", "Account Name", "Total Float Amount" };
            string[] arrAccountStatus = { "Account Number", "Account ID", "Account Name", "Account Status", "Branch Code", "Branch Name", "Account Type", "Account Currency" };
            string[] arrParams = (string[])System.Web.HttpContext.Current.Session["ACCOUNTINQUIRY"];
            string sRoot = new irsConfiguration().appsetting("ReportPath");
            string sReport = sRoot + @"\Inquiry\";
            int i = 0;
            string param = string.Empty;

            try
            {
                switch (FCMethod)
                {
                    case 1: //BALANCE INQUIRY
                        sReport += @"rptBalanceInquiry.rpt";
                        rpt.Load(sReport);
                        for (i = 0; i <= (arrAccountBalance.Length - 1); i++)
                        {
                            discreteVal.Value = arrParams.GetValue(i).ToString().Trim();
                            paramVal.Add(discreteVal);
                            rpt.DataDefinition.ParameterFields[arrAccountBalance.GetValue(i).ToString().Trim()].ApplyCurrentValues(paramVal);
                        }
                        break;
                    case 2: //LAST 5 TRANSACTION INQUIRY
                        sReport += @"rptLastTransaction.rpt";
                        rpt.Load(sReport);
                        for (i = 0; i <= (arrLastTransaction.Length - 1); i++)
                        {
                            /* FOR CBIP FIX 20171006 */
                            if (arrParams.GetValue(i) != null)
                            {
                                param = arrParams.GetValue(i).ToString().Replace("\t", "     ");
                            }
                            else
                            {
                                param = string.Empty;
                            }
                            discreteVal.Value = param;
                            paramVal.Add(discreteVal);
                            rpt.DataDefinition.ParameterFields[arrLastTransaction.GetValue(i).ToString().Trim()].ApplyCurrentValues(paramVal);
                        }
                        break;
                    case 3: //HOLD INQUIRY
                        sReport += @"rptHoldInquiry.rpt";
                        rpt.Load(sReport);
                        for (i = 0; i <= (arrHold.Length - 1); i++)
                        {
                            /* FOR CBIP FIX 20171006 */
                            if (arrParams.GetValue(i) != null)
                            {
                                param = arrParams.GetValue(i).ToString();
                            }
                            else
                            {
                                param = string.Empty;
                            }
                            discreteVal.Value = param;
                            paramVal.Add(discreteVal);
                            rpt.DataDefinition.ParameterFields[arrHold.GetValue(i).ToString().Trim()].ApplyCurrentValues(paramVal);
                        }
                        break;
                    case 4: //FLOAT INQUIRY
                        sReport += @"rptFloatInquiry.rpt";
                        rpt.Load(sReport);
                        for (i = 0; i <= (arrFloat.Length - 1); i++)
                        {
                            /* FOR CBIP FIX 20171006 */
                            if (arrParams.GetValue(i) != null)
                            {
                                param = arrParams.GetValue(i).ToString();
                            }
                            else
                            {
                                param = string.Empty;
                            }
                            discreteVal.Value = param;
                            paramVal.Add(discreteVal);
                            rpt.DataDefinition.ParameterFields[arrFloat.GetValue(i).ToString().Trim()].ApplyCurrentValues(paramVal);
                        }
                        break;
                    case 5: //ACOUNT STATUS INQUIRY
                        sReport += @"rptAccountStatus.rpt";
                        rpt.Load(sReport);
                        for (i = 0; i <= (arrAccountStatus.Length - 1); i++)
                        {
                            discreteVal.Value = arrParams.GetValue(i).ToString().Trim();
                            paramVal.Add(discreteVal);
                            rpt.DataDefinition.ParameterFields[arrAccountStatus.GetValue(i).ToString().Trim()].ApplyCurrentValues(paramVal);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }
            return rpt;
        }

        public ReportDocument GenerateEHUDReceipt(string ReferenceNo)
        {
            ReportDocument Ehudrcpt = new ReportDocument();
            irsOTC otc = new irsOTC();
            DataSet ds = null;
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                ds = otc.EHUDReceipt(ReferenceNo);

                if (ds.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                string repname = @"\ARS\EHUD\rptEHUDReceipt.rpt";
                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string reportpath = rootpath + repname;
                Ehudrcpt.Load(reportpath);
                Ehudrcpt.SetDataSource(ds);
                Ehudrcpt.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return Ehudrcpt;
            }
            catch (Exception e)
            {
                try
                {
                    ca.writeAudit(_userid
                                            , "EHUD", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch
                { }
                return null;
            }
            finally
            {
                if (otc != null)
                    otc = null;
                if (ds != null)
                {
                    ds.Dispose(); ds = null;
                }
                if (ca != null)
                {
                    ca = null;
                }
            }
        }
        public ReportDocument GenerateReceipt(ReportDocument rcpt, int trxID)
        {
            irsReceipts receipt = new irsReceipts();
            irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            string header1 = "", header2 = "", sbranchname = "", sbranchaddr = "", sbranchcontactno = "",
                sbranchemail = "", sID1 = "", sID2 = "", sID3 = "", sID4 = "";
            ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
            ParameterValues paramVal = new ParameterValues();
            csAuditLog audit = new csAuditLog();
            irsOTC otc = new irsOTC();
            DataTable dt = null;
            DataSet dsValidID = null;
            try
            {
                DataTable dtReceiptSetup = receipt.getReceipts(client_data.Location);
                if (dtReceiptSetup.Rows.Count > 0)
                {
                    header1 = dtReceiptSetup.Rows[0]["Header1"].ToString();
                    header2 = dtReceiptSetup.Rows[0]["Header2"].ToString();
                    sbranchname = dtReceiptSetup.Rows[0]["NameOfBranch"].ToString();
                    sbranchaddr = dtReceiptSetup.Rows[0]["Address"].ToString();
                    sbranchcontactno = dtReceiptSetup.Rows[0]["ContactNo"].ToString();
                    sbranchemail = dtReceiptSetup.Rows[0]["Email"].ToString();

                    dsValidID = new DataSet("ValidID");
                    dsValidID = receipt.GetValidID(client_data.Location);
                    if (dsValidID.Tables[0].Rows.Count > 0)
                    {
                        if ((int)dtReceiptSetup.Rows[0]["ValidID1"] != 0)
                        {
                            sID1 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID1"])[0][2].ToString();
                        }
                        if ((int)dtReceiptSetup.Rows[0]["ValidID2"] != 0)
                        {
                            sID2 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID2"])[0][2].ToString();
                        }
                        if ((int)dtReceiptSetup.Rows[0]["ValidID3"] != 0)
                        {
                            sID3 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID3"])[0][2].ToString();
                        }
                        if ((int)dtReceiptSetup.Rows[0]["ValidID4"] != 0)
                        {
                            sID4 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID4"])[0][2].ToString();
                        }
                    }
                }

                dt = otc.IRSReceipt(trxID);
                rcpt.SetDataSource(dt);
                discreteVal.Value = receipt.getReceiptNumber((int)trxID, client_data.Location);
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parReceiptNumber"].ApplyCurrentValues(paramVal);

                discreteVal.Value = header1;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parHeaderOne"].ApplyCurrentValues(paramVal);

                discreteVal.Value = header2;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parHeaderTwo"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchname;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchName"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchaddr;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchAddress"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchcontactno;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchContactNo"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchemail;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchEmailAdd"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID1;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID1"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID2;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID2"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID3;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID3"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID4;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID4"].ApplyCurrentValues(paramVal);
                return rcpt;
            }
            catch (Exception ex)
            {
                audit.writeAudit(_userid, "OTC Transaction", "GenerateReceipt", ex.Message + ":" + ex.StackTrace, trxID.ToString(), csAuditLog.Category.Select);
                throw ex;
            }
            finally
            {
                if (receipt != null) receipt = null;
                if (audit != null) audit = null;
                if (otc != null) otc = null;
                if (dsValidID != null) { dsValidID.Dispose(); dsValidID = null; }
                if (dt != null) { dt.Dispose(); dt = null; }
                GC.Collect();
            }

        }
        private bool isOTCReportHalfPage(string branchcode)
        {
            DataTable dt = new DataTable();
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "proc_check_reporthalfpage";
                string values = branchcode;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                ca.writeAudit(this._userid
                                                , "OTCRHP", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return false;
        }

        private bool isDotMatrix(string branchcode)
        {
            DataTable dt = new DataTable();
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "proc_check_islaser_receipt";
                string values = branchcode;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LaserPrintORNumber"].ToString() == "0")
                        return true;
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                ca.writeAudit(this._userid
                                                , "OTCRHP", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return false;
        }
        private int GetReceiptType(string branchcode)
        {
            DataTable dt = new DataTable();
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "proc_check_tagalog_report";
                string values = branchcode;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];

                return Convert.ToInt32(dt.Rows[0]["ReceiptType"]);

            }
            catch (Exception e)
            {
                ca.writeAudit(this._userid
                                                , "OTCGRT", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return 0;
        }
        private bool isTagalogReport(string branchcode)
        {
            DataTable dt = new DataTable();
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "proc_check_tagalog_report";
                string values = branchcode;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["ReceiptType"].ToString()) > 0)
                        return true;
                }
                return false;
            }
            catch (Exception e)
            {
                ca.writeAudit(this._userid
                                                , "OTCTR", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return false;
        }
        #endregion
        #region IRSOBP
        public ReportDocument GenerateOBPReceipt(ReportDocument obp, string trxID)
        {
            irsReceipts receipt = new irsReceipts();
            string header1 = "", header2 = "", sbranchname = "", sbranchaddr = "", sbranchcontactno = "",
                sbranchemail = "", sID1 = "", sID2 = "", sID3 = "", sID4 = "";
            ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
            ParameterValues paramVal = new ParameterValues();
            irsOBPSTransactions ir = (irsOBPSTransactions)System.Web.HttpContext.Current.Session["OBPSTransaction"];//new irsOBPSTransactions();
            csAuditLog audit = new csAuditLog();
            DataTable dt = null;
            DataSet dsValidID = null;
            try
            {
                DataTable dtReceiptSetup = receipt.getReceipts(_branchcode);
                if (dtReceiptSetup.Rows.Count > 0)
                {
                    header1 = dtReceiptSetup.Rows[0]["Header1"].ToString();
                    header2 = dtReceiptSetup.Rows[0]["Header2"].ToString();
                    sbranchname = dtReceiptSetup.Rows[0]["NameOfBranch"].ToString();
                    sbranchaddr = dtReceiptSetup.Rows[0]["Address"].ToString();
                    sbranchcontactno = dtReceiptSetup.Rows[0]["ContactNo"].ToString();
                    sbranchemail = dtReceiptSetup.Rows[0]["Email"].ToString();

                    dsValidID = new DataSet("ValidID");
                    dsValidID = receipt.GetValidID(_branchcode);
                    if (dsValidID.Tables[0].Rows.Count > 0)
                    {
                        if ((int)dtReceiptSetup.Rows[0]["ValidID1"] != 0)
                        {
                            sID1 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID1"])[0][2].ToString();
                        }
                        if ((int)dtReceiptSetup.Rows[0]["ValidID2"] != 0)
                        {
                            sID2 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID2"])[0][2].ToString();
                        }
                        if ((int)dtReceiptSetup.Rows[0]["ValidID3"] != 0)
                        {
                            sID3 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID3"])[0][2].ToString();
                        }
                        if ((int)dtReceiptSetup.Rows[0]["ValidID4"] != 0)
                        {
                            sID4 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID4"])[0][2].ToString();
                        }
                    }
                }

                dt = ir.OBPReceipt(trxID);

                obp.SetDataSource(dt);

                discreteVal.Value = receipt.getReceiptNumber(Convert.ToInt32(trxID), _branchcode);
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parReceiptNumber"].ApplyCurrentValues(paramVal);

                discreteVal.Value = dt.Rows[0]["ClearingDate"].ToString();
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parClearingDate"].ApplyCurrentValues(paramVal);

                discreteVal.Value = header1;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parHeaderOne"].ApplyCurrentValues(paramVal);

                discreteVal.Value = header2;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parHeaderTwo"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchname;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parBranchName"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchaddr;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parBranchAddress"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchcontactno;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parBranchContactNo"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchemail;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parBranchEmailAdd"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID1;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parID1"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID2;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parID2"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID3;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parID3"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID4;
                paramVal.Add(discreteVal);
                obp.DataDefinition.ParameterFields["parID4"].ApplyCurrentValues(paramVal);

                audit.writeAudit(_userid, "OBPS Transaction", "GenerateOBPReceipt", "", trxID, csAuditLog.Category.Select);
                return obp;
            }
            catch (Exception ex)
            {
                try
                {
                    audit.writeAudit(_userid, "OBPS Transaction", "GenerateOBPReceipt", ex.Message, trxID, csAuditLog.Category.Select);
                }
                catch { }
                throw ex;
            }
            finally
            {

                if (receipt != null) receipt = null;
                if (ir != null) ir = null;
                if (audit != null) audit = null;

                if (dt != null) { dt.Dispose(); dt = null; }
                if (dsValidID != null) { dsValidID.Dispose(); dsValidID = null; }
            }
            //return obp; // commented to remove warning
        }
        #endregion
        #region IRSNAIA
        public ReportDocument GenerateNAIAReceipt(string or_number)
        {
            //string errmsg = ""; // commented to remove warning
            DataTable dt = null;
            csAuditLog ca = new csAuditLog();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                irsEndOfDay a = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "USP_NAIARECEIPT";
                string rptnm = "NAIAReceipt.rpt";
                string values = or_number;
                string[] param = values.Split(',');
                dt = a.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();

                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string reportpath = rootpath + (@"\NAIA3\" + rptnm);
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                return repDoc;
            }
            catch (Exception e)
            {
                try
                {
                    ca.writeAudit(_userid
                                            , "OTCNAIA", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch
                { }
                return null;
            }
            finally { if (dt != null) { dt.Dispose(); dt = null; } if (ca != null) ca = null; }
        }
        #endregion
        #region IRSEOD
        public ReportDocument generate_eod_report(string tranxdate)
        {

            DataTable dt = null, dt2 = null, dt1 = null;
            irsEndOfDay eod = null;
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                eod = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "rptPNB_EODdetails_revised";
                string rptnm = "rptPNB_EOD_Details.rpt";
                string tdate = Convert.ToDateTime(tranxdate).ToString("MM/dd/yyyy");
                string values = tdate + "," + this.getbrcode() + "," + tranxdate;
                string[] param = values.Split(',');
                dt = eod.execStoredProc(sp_name, param).Tables[0];
                if (dt.Rows.Count == 0)
                {
                    try
                    {
                        new csAuditLog().writeAudit(_userid
                                          , "EOD", "NO DATA FOUND", "", "", csAuditLog.Category.Select);
                    }
                    catch { }
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();

                string rootpath = new irsConfiguration().appsetting("ReportPath");
                string reportpath = rootpath + (@"\IRSWorld_Setup\" + rptnm);
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);

                dt1 = this.getbenecharge(this.getbrcode(), Convert.ToDateTime(tranxdate)).Tables[0];
                dt2 = this.getforexrate(this.getbrcode(), Convert.ToDateTime(tranxdate)).Tables[0];
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                repDoc.Subreports[0].SetDataSource(dt1);
                repDoc.Subreports[0].SetDatabaseLogon(userid[1], password[1], server[1], database[1]);
                repDoc.Subreports[1].SetDataSource(dt2);
                repDoc.Subreports[1].SetDatabaseLogon(userid[1], password[1], server[1], database[1]);

                return repDoc;
            }
            catch (Exception e)
            {
                try
                {
                    new csAuditLog().writeAudit(_userid
                                            , "EOD", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch
                { }
                return null;
            }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }
                if (dt1 != null) { dt1.Dispose(); dt1 = null; }
                if (dt2 != null) { dt2.Dispose(); dt2 = null; }
                if (eod != null) eod = null;
            }

        }
        public ReportDocument generate_orps_report(string branchcode, string tranxdate)
        {

            irsEndOfDay eod = null;
            DataTable dt = null;
            irsConfiguration ic = new irsConfiguration();
            try
            {
                string connstring = new irsConfiguration().ConnectionString;
                string[] arr_conn_details = connstring.Split(';');
                string[] server = arr_conn_details[0].Split('=');
                string[] database = arr_conn_details[2].Split('=');
                string[] userid = arr_conn_details[3].Split('=');
                string[] password = arr_conn_details[4].Split('=');
                eod = new irsEndOfDay(userid[1], password[1], server[1], database[1]);
                string sp_name = "proc_pushedtoORPS";
                string rptnm = "rpt_PushedToORPS.rpt";
                string tdate = Convert.ToDateTime(tranxdate).ToString("MM/dd/yyyy");
                string values = tdate + "," + branchcode;
                string[] param = values.Split(',');
                dt = eod.execStoredProc(sp_name, param).Tables[0];

                if (dt.Rows.Count == 0)
                {
                    try
                    {
                        new csAuditLog().writeAudit(_userid
                                                , "ORPS", "NO DATA FOUND", "", "", csAuditLog.Category.Select);
                    }
                    catch
                    { }
                    return null;
                }
                ReportDocument repDoc = new ReportDocument();

                string rootpath = ic.appsetting("ReportPath");
                string reportpath = rootpath + (@"\Standard\" + rptnm);
                repDoc.Load(reportpath);
                repDoc.SetDataSource(dt);
                repDoc.SetDatabaseLogon(userid[1], password[1], server[1], database[1]);

                return repDoc;
            }
            catch (Exception e)
            {
                try
                {
                    new csAuditLog().writeAudit(_userid
                                            , "EOD", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch
                { }
                return null;
            }
            finally
            {
                if (dt != null) { dt.Dispose(); dt = null; }
                if (eod != null) eod = null;
                if (ic != null) ic = null;
            }

        }
        private DataSet getbenecharge(string branchcode, DateTime tranxdate)
        {
            irsEndOfDay eod = new irsEndOfDay();
            try
            {
                string param = tranxdate.ToString("MM/dd/yyyy") + "," + branchcode;
                return eod.execStoredProc("proc_BeneCharges", param.Split(','));
            }
            catch (Exception ex)
            {
                try
                {
                    new csAuditLog().writeAudit(_userid
                                            , "GETBENE", ex.StackTrace, eod.Message, ex.Message, csAuditLog.Category.Select);
                }
                catch
                { }
            }
            finally
            {
                if (eod != null) { eod = null; }
            }
            return null;
        }
        private DataSet getforexrate(string branchcode, DateTime tranxdate)
        {
            irsEndOfDay eod = new irsEndOfDay();
            try
            {
                string param = tranxdate.ToString("MM/dd/yyyy") + "," + branchcode;
                return eod.execStoredProc("rpt_EODForexRates_revised", param.Split(','));
            }
            catch (Exception ex)
            {
                try
                {
                    new csAuditLog().writeAudit(_userid
                                            , "GETFOREX", ex.StackTrace, eod.Message, ex.Message, csAuditLog.Category.Select);
                }
                catch
                { }
            }
            finally
            {
                if (eod != null) eod = null;
            }
            return null;
        }
        private string getbrcode()
        {
            return _branchcode;
        }
        #endregion
    }
}
