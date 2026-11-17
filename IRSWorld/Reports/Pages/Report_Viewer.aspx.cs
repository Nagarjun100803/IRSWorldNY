using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using IRSWorld.TranClass;
using System.IO;

namespace IRSWorld.Reports.Pages
{
    public partial class Report_Viewer : System.Web.UI.Page
    {
        irsReports irsrep = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            string errmsg = "";
            csAuditLog audit = new csAuditLog();
            irsTimeZone it = new irsTimeZone();
            string _userid = "";
            string _branchcode = "";
            bool istagalogrep = false;
            DateTime _tranxdate = new DateTime();
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Report_Viewer));

            this.hdBranchCode.Value = IRSClientData.Location;
            this.hdUserID.Value = IRSClientData.UserId;

            try
            {
                if (IRSClientData == null)
                {
                    errmsg = "SESSION EXPIRED FROM CLIENTDATA";
                    try
                    {
                        audit.writeAudit("Report_Viewer", "Report_Viewer", "INITIALIZE", "", "", csAuditLog.Category.Email);
                    }
                    catch { }
                    ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "eval(spawn_errpage('SESSION EXPIRED'))", true);

                }
                else
                {
                    _userid = IRSClientData.UserId;
                    try
                    {
                        _branchcode = IRSClientData.Location;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            errmsg = "NO BRANCHCODE DEFINED";
                            audit.writeAudit("Report_Viewer", "Report_Viewer_ERROR", "PAGE LOAD", ex.Message, ex.StackTrace, csAuditLog.Category.Select);
                        }
                        catch
                        { }
                        ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "eval(spawn_errpage('" + errmsg + "'))", true);
                    }
                    try
                    {
                        _tranxdate = it.branchLocalTime(_branchcode);
                    }
                    catch
                    {
                        try
                        {
                            errmsg = "TIMEZONE IS NOT CONFIGURED FOR THIS BRANCH";
                            audit.writeAudit("Report_Viewer", "Report_Viewer_ERROR", "PAGE LOAD", errmsg, _branchcode, csAuditLog.Category.Select);
                        }
                        catch
                        { }
                        ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "eval(spawn_errpage('" + errmsg + "'))", true);

                    }
                }

            }
            catch (Exception el)
            {
                try
                {

                    audit.writeAudit("Report_Viewer", "Report_ViewerERROR", "PAGE LOAD", el.Message, el.StackTrace, csAuditLog.Category.Select);
                }
                catch
                { }

            }
            finally
            {
                it = null;
            }

            this.hdBranchCode.Value = _branchcode;
            this.hdUserID.Value = _userid;



            if (!Page.IsPostBack)
            {
                ReportDocument rptDoc = new ReportDocument();

                this.hdautoprint.Value = "false";

                string str_session = "";
                string exporttype = "RTF";
                DataTable dt = null;
                irsSqlCommonsWs com = new irsSqlCommonsWs();
                try
                {
                    dt = com.getLookup(new Object[] { "DefaultReportType", DBNull.Value, -1 });
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        exporttype = dt.Rows[0]["FldCode"].ToString();
                    }
                }
                catch (Exception ep)
                {
                    try
                    {
                        new csAuditLog().writeAudit("Report_Viewer", "Report_Viewer", "ERROR", ep.Message, ep.StackTrace, csAuditLog.Category.Email);
                    }
                    catch { }
                }
                finally
                {
                    if (dt != null)
                    {
                        dt.Dispose(); dt = null;
                    }
                    if (com != null)
                    {
                        com = null;
                    }
                }
                try
                {
                    string urlstring = Request.QueryString.ToString();
                    audit.writeAudit(_userid, "Report Module Logs", "Report Viewer", "STARTEDPAGE", urlstring, csAuditLog.Category.Select);

                    Boolean hasApas = false;
                    Boolean hasSession = false;
                    string merge = "";
                    string param = "";
                    if (urlstring.ToLower().Contains("mergeparam") &&
                           urlstring.ToLower().Contains("paramvalues"))
                    {
                        merge = Request.QueryString["mergeparam"].ToString();
                        param = Request.QueryString["paramvalues"].ToString();
                        exporttype = Request.QueryString["exporttype"].ToString();
                        this.hdautoprint.Value = "false";
                    }

                    /* VAPT 20180222 */
                    string[] arrMerge = merge.Split('^');
                    try
                    {
                        new csAuditLog().writeVAPTLogger("", "REPORTS", "USER HAD GENERATED A REPORT", arrMerge.GetValue(6).ToString(), "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                    }
                    catch (Exception ex)
                    {
                        if (urlstring.ToString().ToLower().Contains("trantype"))
                        {
                            new csAuditLog().writeVAPTLogger("", "REPORTS", "USER HAD GENERATED A RECEIPT", "", Request.QueryString["trantype"].ToString(), Request.QueryString["tranvalue"].ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Information);
                        }
                        else
                        {
                            if (arrMerge != null && arrMerge.Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "REPORTS", "USER HAD GENERATED A REPORT", merge, ex.Message, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            }
                        }
                    }

                    string BranchCode = _branchcode;
                    string strTranxdate = _tranxdate.ToString("MM/dd/yyyy");
                    try
                    {

                        Boolean pass = true;

                        try
                        {
                            hasSession = (System.Web.HttpContext.Current.Session["IRS_ReportsUI"].GetType() == "".GetType()) ? true : false;
                            str_session = System.Web.HttpContext.Current.Session["IRS_ReportsUI"].ToString();
                            if (urlstring.ToLower().Contains("trantype"))
                            {
                                hasSession = false;
                                str_session = "";
                            }
                        }
                        catch
                        {
                            hasSession = false;
                            str_session = "";
                        }

                        try
                        {
                            hasApas = (System.Web.HttpContext.Current.Session["APASReceipt"].ToString().ToUpper() == "APAS") ? true : false;
                        }
                        catch
                        {
                            hasApas = false;
                        }
                        if (urlstring.ToLower().Contains("mergeparam") &&
                            urlstring.ToLower().Contains("paramvalues"))
                        {
                            pass = this.isSpecialReport(merge, param);
                        }
                        if (urlstring.ToLower().Contains("mergeparam") && pass == false)
                        {

                            exporttype = Request.QueryString["exporttype"].ToString();
                            string s_autoprint = Request.QueryString["autoprint"].ToString();

                            if (!urlstring.ToLower().Contains("apas"))
                            {
                                irsHTMLReports irshrep = new irsHTMLReports();
                                try
                                {
                                    istagalogrep = this.check_tagalog_report(BranchCode);
                                    string surl = irshrep.getHTMLReportByReportMod(merge, param);
                                    if (surl.Trim().Length > 0 && exporttype.ToLower().Contains("html") && istagalogrep == false)
                                    {
                                        String url = "../HTMLReport/HTMLPrint.aspx?" + System.Web.HttpContext.Current.Request.QueryString.ToString();
                                        System.Web.HttpContext.Current.Response.Redirect(url);
                                        return;
                                    }
                                }
                                finally
                                {
                                    irshrep = null;
                                }



                                irsrep = new irsReports(merge, param, exporttype);
                                string spass = irsrep.isokforload();
                                if (spass.Length > 0)
                                {
                                    rptDoc = irsrep.getReportDataSet();
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "alert('NO RECORD(S) FOUND')", true);
                                    return;
                                }
                            }
                            else
                            {
                                string connstring = com.ConnectionString;
                                string[] arr_conn_details = connstring.Split(';');
                                string[] server = arr_conn_details[0].Split('=');
                                string[] database = arr_conn_details[2].Split('=');
                                string[] userid = arr_conn_details[3].Split('=');
                                string[] password = arr_conn_details[4].Split('=');

                                string values = Request.QueryString["value"].ToString();
                                string rptnm = Request.QueryString["rptnm"].ToString();
                                string spname = Request.QueryString["spname"].ToString();

                                irsAPASReports a = new irsAPASReports(userid[1], password[1], server[1], database[1]);

                                rptDoc = generate_report(values, rptnm, spname); ;
                            }

                        }
                        else if (hasApas)
                        {
                            string refno = Session["APASRefNo"].ToString();
                            rptDoc = generate_report(refno, "APASReceipt", "sp_RPT_APASReceipt"); ;
                            Session["APASReceipt"] = "";
                        }
                        else if (hasSession)
                        {
                            if (str_session.ToUpper().Contains("OBP"))
                            {
                                string[] rpt = str_session.Split(':');
                                rptDoc = new CrystalReportUtil().generate_obp_report(((irsClientData)Session["IRS_CLIENT_DATA"]).Location, rpt[1].ToString());
                                Session["IRS_ReportsUI"] = null;
                            }
                            else if (str_session.ToUpper().Contains("OTC"))
                            {
                                string[] rpt = str_session.Split(':');
                                rptDoc = new CrystalReportUtil().generate_otc_report(((irsClientData)Session["IRS_CLIENT_DATA"]).Location, rpt[1].ToString());

                                Session["IRS_ReportsUI"] = null;
                            }
                            else if (str_session.ToUpper().Contains("EHUD"))
                            {
                                string[] rpt = str_session.Split(':');
                                rptDoc = new CrystalReportUtil().GenerateEHUDReceipt(rpt[1].ToString());
                                Session["IRS_ReportsUI"] = null;
                            }
                        }

                        //*** ADDED BY TROY 20110629 ***//
                        else if (urlstring.ToString().ToUpper().Contains("ACCOUNTINQUIRY")) //ACCOUNT INQUIRY MODULE
                        {
                            rptDoc = new CrystalReportUtil().GenerateInquiryReport(Convert.ToInt32(Request.QueryString["ACCOUNTINQUIRY"].ToString()));
                        }
                        //*** ADDED BY TROY 20110629 ***//

                        else if (urlstring.ToString().ToLower().Contains("trantype"))
                        {

                            this.hdautoprint.Value = "true";
                            string trantype = Request.QueryString["trantype"].ToString();
                            string tranval = Request.QueryString["tranvalue"].ToString();

                            irsHTMLReports irshrep = new irsHTMLReports();
                            try
                            {
                                istagalogrep = this.check_tagalog_report(BranchCode);
                                string surl = irshrep.getHTMLReportByDirect("trantype=" + trantype.ToLower().Trim());
                                if (surl.Trim().Length > 0 && istagalogrep == false)
                                {
                                    String url = "../HTMLReport/HTMLPrint.aspx?" + System.Web.HttpContext.Current.Request.QueryString.ToString();
                                    System.Web.HttpContext.Current.Response.Redirect(url);
                                    return;
                                }
                            }
                            finally
                            {
                                irshrep = null;
                            }

                            if (trantype.ToUpper().Trim() == "OTC")
                            {

                                string tranxid = tranval;
                                rptDoc = new CrystalReportUtil().generate_otc_report(BranchCode, tranxid);
                            }
                            if (trantype.ToUpper().Trim() == "OTCDAILY")
                            {
                                string tranxid = tranval;
                                rptDoc = new CrystalReportUtil().generate_otc_dailyreport(tranxid);
                            }
                            else if (trantype.ToUpper().Trim() == "EOD")
                            {
                                rptDoc = new CrystalReportUtil().generate_eod_report(tranval);
                            }
                            else if (trantype.ToUpper().Trim() == "OBP")
                            {
                                rptDoc = new CrystalReportUtil().generate_obp_report(BranchCode, tranval);
                            }
                            else if (trantype.ToUpper().Trim() == "ORPS")
                            {
                                rptDoc = new CrystalReportUtil().generate_orps_report(BranchCode, tranval);
                                this.handle_export(exporttype, rptDoc, _userid);
                                return;

                            }
                            else if (trantype.ToUpper().Trim() == "NAIA")
                            {
                                rptDoc = new CrystalReportUtil().GenerateNAIAReceipt(tranval);
                                this.handle_export(exporttype, rptDoc, _userid);
                                return;

                            }
                        }
                        else
                        {

                            rptDoc = (ReportDocument)HttpContext.Current.Session["IRS_ReportsUI"];

                        }

                        if (rptDoc != null)
                        {
                            if (!this.handle_export(exporttype, rptDoc, _userid))
                            {
                                throw new Exception("Error in Exporting Report");
                            }
                            else
                            {
                                try
                                {
                                    audit.writeAudit(_userid, "Report Module Logs", "Report Viewer", "ENDEDEXTRACT", "", csAuditLog.Category.Select);
                                }
                                catch { }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //irsConfiguration ic = new irsConfiguration();
                        //try
                        //{

                        //    if (urlstring.ToLower().Contains("eod") && urlstring.ToLower().Contains("trantype"))
                        //    {
                        //        hdHasError.Value = ex.Message + ":" + ex.StackTrace;

                        //        // get data
                        //        string tranxdate = strTranxdate;
                        //        string connstring = ic.ConnectionString;
                        //        string[] arr_conn_details = connstring.Split(';');
                        //        string[] server = arr_conn_details[0].Split('=');
                        //        string[] database = arr_conn_details[2].Split('=');
                        //        string[] userid = arr_conn_details[3].Split('=');
                        //        string[] password = arr_conn_details[4].Split('=');
                        //        string connstring1 = ic.ConnectionString;
                        //        string[] arr_conn_details1 = connstring.Split(';');
                        //        string[] server1 = arr_conn_details[0].Split('=');
                        //        string[] database1 = arr_conn_details[2].Split('=');
                        //        string[] userid1 = arr_conn_details[3].Split('=');
                        //        string[] password1 = arr_conn_details[4].Split('=');
                        //        irsEndOfDay a = new irsEndOfDay(userid1[1], password1[1], server1[1], database1[1]);
                        //        string sp_name = "rptPNB_EODdetails";
                        //        string tdate = Convert.ToDateTime(tranxdate).ToString("MM/dd/yyyy");
                        //        string values = tdate + "," + BranchCode + "," + tranxdate;
                        //        string[] param1 = values.Split(',');
                        //        DataTable dt1 = a.execStoredProc(sp_name, param1).Tables[0];

                        //        if (dt1.Rows.Count == 0) return;

                        //        string smsg = "ITEMS FOR EOD.\n";
                        //        smsg = smsg + "    IMPLEM    \t|   AMOUNT\t|    ITEMS\n";


                        //        foreach (DataRow r in dt1.Rows)
                        //        {
                        //            smsg = smsg + this.padIt(r["Implementationtype"].ToString(), 30, false) + "\t| " + this.padIt(r["RemittanceAmount"].ToString(), 25, true) + "\t|" + this.padIt(r["NoofItems"].ToString(), 10, true) + "\n";
                        //        }

                        //        hdEODmsg.Value = smsg;
                        //        ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "eval(handle_eod_error())", true);
                        //    }
                        //    else
                        //    {

                        //        audit.writeAudit(_userid, "Report Module Error", "Report Viewer", ex.StackTrace, ex.Message, csAuditLog.Category.Select);

                        //        this.spawn_error_page();

                        //    }
                        //}
                        //catch (Exception exinner)
                        //{
                        //    try
                        //    {

                        //        audit.writeAudit(_userid, "Report Module Error", "Report Viewer", exinner.StackTrace, exinner.Message, csAuditLog.Category.Select);
                        //    }
                        //    catch { }
                        //}
                        //finally
                        //{
                        //    if (ic != null) ic = null;
                        //}              
                        Response.Write("<table align='center'><tr><td style='font-weight: bold; font-size: 2em; color: maroon; font-family: Arial'>ERROR CANNOT LOAD CRYSTAL REPORTS</td></tr></table>");
                    }
                }

                catch (Exception ex)
                {
                    try
                    {
                        audit.writeAudit(_userid, "Report Module Error", "Report Viewer", ex.StackTrace, ex.Message, csAuditLog.Category.Select);
                    }
                    catch { }


                }
                finally
                {

                    if (rptDoc != null)
                    {
                        rptDoc.Close();
                        rptDoc.Dispose();
                        rptDoc = null;
                    }
                    try
                    {
                        audit.writeAudit(_userid, "Report Module Logs", "Report Viewer", "ENDEDPAGE", "", csAuditLog.Category.Select);
                    }
                    catch { }

                    if (audit != null) { audit = null; }
                    GC.Collect();
                }
            }
        }

        private string padIt(string olds, int maxchar, bool isleft)
        {
            string stemp = olds;
            if (olds.Length == maxchar)
            {
                return olds;
            }
            if (isleft)
            {
                if (olds.Length < maxchar)
                {
                    stemp = olds.PadLeft(maxchar - olds.Length);
                }
                else
                {
                    stemp = olds.PadLeft(olds.Length - maxchar);
                }
                return stemp;
            }
            else
            {
                if (olds.Length < maxchar)
                {
                    stemp = olds.PadRight(maxchar - olds.Length);
                }
                else
                {
                    stemp = olds.PadRight(olds.Length - maxchar);
                }
                return stemp;
            }
            //return stemp.PadRight(maxchar) ;
        }
        /*FOR APAS*/
        #region FOR APAS
        private string rptpath;
        public DataSet execStoredProc(string spname, string[] param)
        {
            DataSet ds;
            try
            {
                ds = new sqlHelperWs().execStoredProcedure(spname, param);
                return ds;
            }
            finally { }
        }


        public ReportDocument generate_report(string values, string rptnm, string spnm)
        {
            string[] param = values.Split(',');
            DataSet ds = null;
            try
            {
                ds = execStoredProc(spnm, param);
                return getReportDataSet(rptnm, ds);
            }
            catch
            {
                return null;
            }
            finally
            {
                if (ds != null) { ds.Dispose(); ds = null; };
            }

        }


        public ReportDocument getReportDataSet(string rptnm, DataSet ds)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument repDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            try
            {
                this.rptpath = System.Web.HttpContext.Current.Server.MapPath("..\\APAS\\" + rptnm + ".rpt");
                repDoc.Load(this.rptpath);
                repDoc.SetDataSource(ds.Tables[0]);
                return repDoc;
            }
            catch
            {
                return null;
            }
            finally
            {
                repDoc.Dispose();
                repDoc = null;
            }
        }
        /*END FOR APAS*/
        #endregion
        #region "AJAX PUBLIC METHOD"
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String exec_forcepush(string userid, string branchcode)
        {
            irsReport_LookupUI ir = new irsReport_LookupUI();
            csAuditLog ca = new csAuditLog();
            irsTimeZone it = new irsTimeZone();
            try
            {

                string strTranxdate = it.branchLocalTime(branchcode).ToString("MM/dd/yyyy");
                if (ir.exec_forcepush(strTranxdate, branchcode))
                {
                    return "";
                }
                return "ERROR:CANNOT CONTINUE FORCE PUSH.";


            }
            catch (Exception e)
            {
                try
                {
                    ca.writeAudit(userid, "EOD", "Report Viewer", e.StackTrace, e.Message, csAuditLog.Category.Select);
                }
                catch { }

                return "ERROR:" + e.Message;
            }
            finally
            {
                if (ir != null) ir = null;
                if (ca != null) ca = null;
                if (it != null) it = null;
            }
        }


        #endregion
        #region PRIVATE METHOD
        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }
        private bool tag_eod_items()
        {
            try
            {
                irsReport_LookupUI ir = new irsReport_LookupUI();
                string BranchCode = IRSClientData.Location;
                string strTranxdate = new irsTimeZone().branchLocalTime_nWs(BranchCode).ToString("MM/dd/yyyy");
                if (ir.exec_forcepush(strTranxdate, BranchCode))
                {
                    return true;
                }
                return false;


            }
            catch (Exception e)
            {

                try
                {
                    new csAuditLog().writeAudit(IRSClientData.UserId, "EOD", "Report Viewer", e.StackTrace, e.Message, csAuditLog.Category.Select);
                }
                catch { }

                return false;
            }
        }

        private bool isSpecialReport(string _merge, string _params)
        {
            sqlHelperWs ws = new sqlHelperWs();
            DataTable dt = new DataTable();
            try
            {

                dt = ws.execStoredProcedure("proc_get_specialreport", new string[] { "" }).Tables[0];
                foreach (DataRow r in dt.Rows)
                {
                    string sarr = r["FLDDESCRIPTION"].ToString();
                    string[] scon = sarr.Split('^');
                    if (_merge.ToUpper().Contains(scon[0].ToUpper()))
                    {
                        this.set_handle_report(_merge, _params, r["FLDCODE"].ToString());
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                dt.Dispose();
                dt = null;
                ws = null;
            }
            return false;
        }
        private void set_handle_report(string _merge, string _params, string _fixedcode)
        {

            irsReports rep = new irsReports(_merge, _params, "false");

            string arr1 = rep.rep_values[0].ToString();
            System.Web.HttpContext.Current.Session["IRS_ReportsUI"] = null;
            System.Web.HttpContext.Current.Session["IRS_ReportsUI"] = _fixedcode + ":" + arr1;

            rep = null;
        }

        #endregion

        private void spawn_error_page()
        {
            if (IRSClientData == null)
            {
                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "eval(spawn_errpage('SESSION EXPIRED'))", true);

            }

        }

        private bool handle_export(string et, ReportDocument rptDoc, string userid)
        {
            irsConfiguration config = new irsConfiguration();
            csFileIo io = new csFileIo();
            string sguid = Guid.NewGuid().ToString();
            MemoryStream m = null;
            /* FOR CBIP */
            Stream s = null;
            byte[] byteArray = null;
            string Fname;
            string ct = "application/pdf";
            try
            {
                ExportFormatType ft = ExportFormatType.PortableDocFormat;
                Fname = "REP" + sguid;
                switch (et.ToLower())
                {
                    case "txt":
                        {
                            ft = ExportFormatType.RichText;
                            ct = "application/rtf";
                            break;
                        }
                    case "html":
                        {
                            ft = ExportFormatType.HTML32;
                            ct = "application/html";
                            break;
                        }
                    case "rtf":
                        {
                            ft = ExportFormatType.RichText;
                            ct = "application/rtf";
                            break;
                        }
                    case "doc":
                        {
                            ft = ExportFormatType.WordForWindows;
                            ct = "application/msword";
                            break;
                        }
                    case "xls":
                        {
                            ft = ExportFormatType.Excel;
                            ct = "application/vnd.ms-excel";
                            break;
                        }
                    case "pdf":
                        {
                            ft = ExportFormatType.PortableDocFormat;
                            ct = "application/pdf";
                            break;
                        }
                    default:
                        {
                            ft = ExportFormatType.RichText;
                            ct = "application/rtf";
                            break;
                        }
                }
                s = rptDoc.ExportToStream(ft);
                byteArray = new byte[s.Length];
                s.Read(byteArray, 0, Convert.ToInt32(s.Length - 1));
                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = ct;
                Response.BinaryWrite(byteArray);
                Response.Flush();
                Response.Close();
                rptDoc.Close();
                rptDoc.Dispose();
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    new csAuditLog().writeAudit(userid, "Report Module Logs", "ReportViewerError", e.Message, e.StackTrace, csAuditLog.Category.Select);
                }
                catch
                { }
                throw new Exception(e.Message, e.InnerException);

            }
            finally
            {
                if (config != null)
                {
                    config = null;
                }
                if (io != null)
                {
                    io = null;
                }
                if (m != null)
                {
                    m = null;
                }
                if (s != null)
                {
                    s = null;
                }
                try
                {
                    new csAuditLog().writeAudit(userid, "Report Module Logs", "Report Viewer", "EXTRACTEDPAGE", "", csAuditLog.Category.Select);
                }
                catch
                { }
            }

        }

        private bool handle_export1(string et, ReportDocument rptDoc, string userid)
        {
            irsConfiguration config = new irsConfiguration();
            csFileIo io = new csFileIo();
            string sguid = Guid.NewGuid().ToString();
            MemoryStream m = null;
            /* FOR CBIP */
            Stream s = null;
            string Fname;
            try
            {

                ExportFormatType ft = ExportFormatType.PortableDocFormat;
                string ct = "application/pdf";

                switch (et.ToLower())
                {
                    case "txt":
                        {
                            Fname = "REP" + sguid + ".txt";
                            ft = ExportFormatType.RichText;
                            ct = "application/rtf";
                            break;
                        }
                    case "html":
                        {
                            Fname = "REP" + sguid + ".htm";
                            ft = ExportFormatType.HTML32;
                            ct = "text/html";
                            break;
                        }
                    case "rtf":
                        {
                            Fname = "REP" + sguid + ".rtf";
                            ft = ExportFormatType.RichText;
                            ct = "application/rtf";
                            break;
                        }
                    case "doc":
                        {
                            Fname = "REP" + sguid + ".doc";
                            ft = ExportFormatType.WordForWindows;
                            ct = "application/msword";
                            break;
                        }
                    case "xls":
                        {
                            Fname = "REP" + sguid + ".xls";
                            ft = ExportFormatType.Excel;
                            ct = "application/vnd.ms-excel";
                            break;
                        }
                    case "pdf":
                        {
                            Fname = "REP" + sguid + ".pdf";
                            ft = ExportFormatType.PortableDocFormat;
                            ct = "application/pdf";
                            break;
                        }
                    default:
                        {
                            Fname = "REP" + sguid + ".rtf";
                            ft = ExportFormatType.RichText;
                            ct = "application/rtf";
                            break;
                        }
                }
                /* FOR CBIP */
                //m = (MemoryStream)rptDoc.ExportToStream(ft);
                s = rptDoc.ExportToStream(ft);
                byte[] b = new byte[s.Length + 1];
                s.Read(b, 0, Convert.ToInt32(s.Length));

                Response.ClearContent();
                Response.ClearHeaders();
                Response.ContentType = ct;
                Response.AddHeader("Content-Disposition", "inline; filename=" + Fname);
                //Response.BinaryWrite(m.ToArray());
                Response.BinaryWrite(b);
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return true;
            }
            catch (Exception e)
            {
                try
                {
                    new csAuditLog().writeAudit(userid, "Report Module Logs", "ReportViewerError", e.Message, e.StackTrace, csAuditLog.Category.Select);
                }
                catch
                { }
                throw new Exception(e.Message, e.InnerException);

            }
            finally
            {
                if (config != null)
                {
                    config = null;
                }
                if (io != null)
                {
                    io = null;
                }
                if (m != null)
                {
                    m = null;
                }
                if (s != null)
                {
                    s = null;
                }
                try
                {
                    new csAuditLog().writeAudit(userid, "Report Module Logs", "Report Viewer", "EXTRACTEDPAGE", "", csAuditLog.Category.Select);
                }
                catch
                { }
            }

        }

        private bool check_tagalog_report(string values)
        {
            string[] param = values.Split(',');
            DataSet ds = null;
            try
            {
                //ds = execStoredProc("proc_check_tagalog_report", param);
                ds = execStoredProc("proc_check_tagalog_rep", param);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt32(ds.Tables[0].Rows[0]["ReceiptType"].ToString()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (ds != null) { ds.Dispose(); ds = null; };
            }

        }

    }
}
