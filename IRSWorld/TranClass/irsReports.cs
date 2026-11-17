/* ===========================================
 * AUTHOR: Jeffrey M. Buizon
 * DATE CREATED: 01/10/2010
 * DESCRIPTION: class for IRSWORLD reports
 * ============================================
 * 
 * REVISION: 
 * AUTHOR: Jeffrey M. Buizon
 * DATE MODIFIED: 01/12/2010
 * DESCRIPTION:
 * */
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
    public class irsReports : sqlHelperWs
    {
        #region CONSTANT VALUES
        /* values for datatypes
         * string
         * number
         * date
         * bit
         * blob
         */
        #endregion
        #region PRIVATE VARIABLE       
        private string error_message            = null;
        private string _error_message = null;
        public ArrayList rep_param             = new ArrayList();
        public ArrayList rep_datatypes         = new ArrayList ();
        public ArrayList rep_values            = new ArrayList();
        // commented to remove warning - private int _rep_length;
        private string exporttype;
        private string sp_name;
        private string _sp_name;
        private string rep_path;
        private string _rep_path;
        private string rep_id;
        private string _rep_id;
        private string rep_pdfpath;
        private string _rep_pdfpath;
        private string rep_sessionid;
        private string _rep_sessionid;
        private string  sprintToPrinter;
        private ArrayList rep_db_conn = new ArrayList();
        private string servername;
        private string dbname;
        private string username;
        private string password;
        public string errormessage ="";
        private DataSet rep_dataset = null;
        private string loggedbranch = "";
        #endregion       
        #region PROPERTY GET & SET
        public string messages
        {
            get { return this.error_message; }
            set { _error_message = value; }
        }
        
        public string p_server
        {
            get { return this.servername; }
        }
        public string p_database
        {
            get { return this.dbname; }
        }
        public string p_user
        {
            get { return this.username; }
        }
        public string p_password
        {
            get { return this.password; }
        }  
         public string p_prinToPrinter
        {
            get { return this.password; }
        }        
        public string p_sp_name
        {
            get { return this.sp_name; }
            set { _sp_name = value; }
        }
        public string p_rep_path
        {
            get { return this.rep_path; }
            set { _rep_path = value; }
        }
        public string p_rep_id
        {
            get { return this.rep_id; }
            set { _rep_id = value; }
        }
        public string p_rep_pdfpath
        {
            get { return this.rep_pdfpath; }
            set { _rep_pdfpath = value; }
        }
        public string p_rep_sessionid
        {
            get { return this.rep_sessionid; }
            set { _rep_sessionid = value; }
        }
        //public DataSet p_rep_dataset
        //{
        //    get { return this.rep_dataset; }            
        //}
        public string p_exporttype
        {
            get { return this.exporttype; }
        }
        #endregion
        #region PUBLIC METHOD
        #region Constructors
        public irsReports()
        {
            base.EnforceConstraint = false;
            this.TableName = "tbl_Reports";
            this.IdentityColumn = "SEQ_ID";
        }
        public irsReports(string mergeparam, string rep_values,string _exporttype)
        {
            base.EnforceConstraint = false;
            this.TableName = "tbl_Reports";
            this.IdentityColumn = "SEQ_ID";
            this.populate_report_values(rep_values);
            this.init_parameters(mergeparam);
            this.exporttype = _exporttype;
            this.load_report_dataset();
            

            if (IRSClientData != null)
            {
                new csAuditLog().writeAudit(IRSClientData.UserId.ToString() , "REPORT MODULE", "INITIALIZING " + "user:" + this.IRSClientData.UserId, "", "", csAuditLog.Category.Select);
                this.loggedbranch = IRSClientData.Location;
            }
            //this.writereportlogs("INITIALIZING " + "user:" + this.IRSClientData.UserId  );
        }

        public irsReports(DataTable dt, string s_rep_path, bool printToPrinter)
        {
            base.EnforceConstraint = false;
            this.TableName = "tbl_Reports";
            this.IdentityColumn = "SEQ_ID";
            System.Web.HttpContext.Current.Session["REP_DATASET"] = dt;
            this.rep_path = s_rep_path;
            this.init_rep_con();
            if (printToPrinter)
            {
                this.sprintToPrinter = "true";
            }
            else
            {
                this.sprintToPrinter = "false";
            }            
            this.init_rep_con();
            System.Web.HttpContext.Current.Session["REP_PRINTER"] = sprintToPrinter;
            HttpContext.Current.Session["IRS_Reports"] = load_crystal_report(dt);
        }
        public irsReports(DataTable dt, string s_rep_path, int printToPrinter)
        {
            base.EnforceConstraint = false;
            this.init_session();
            System.Web.HttpContext.Current.Session["REP_DATASET"] = dt;
            System.Web.HttpContext.Current.Session["REP_PATH"] = s_rep_path;            
            this.rep_path = s_rep_path;
            HttpContext.Current.Session["IRS_Reports"] = load_crystal_report(dt);
            if (printToPrinter == 1)
            {
                this.sprintToPrinter = "true";
            }
            else
            {
                this.sprintToPrinter = "false";
            }
            System.Web.HttpContext.Current.Session["REP_PRINTER"] = sprintToPrinter;
        }
        public irsReports(DataTable dt, int repid, int printToPrinter,string a)
        {
            base.EnforceConstraint = false;
            this.init_session();
            System.Web.HttpContext.Current.Session["REP_DATASET"] = dt;
            if (this.setSession(repid))
            {
                this.rep_id = repid.ToString() ;
                if (printToPrinter == 1)
                {
                    this.sprintToPrinter = "true";
                }
                else
                {
                    this.sprintToPrinter = "false";
                }
                System.Web.HttpContext.Current.Session["REP_PRINTER"] = sprintToPrinter;
            }
        }             
        #endregion
        public bool display_report()
        {
            try
            {
                this.init_rep_con();
                HttpContext.Current.Session["IRS_Reports"] = load_crystal_report((DataTable)HttpContext.Current.Session["REP_DATASET"]);
                HttpContext.Current.Response.Redirect("Report_Viewer.aspx");
                return true; 
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return false;
            }
        }
        ~irsReports()
        {

        }
        public ReportDocument load_crystal_report(DataTable dt)
        {
            ReportDocument repDoc = new ReportDocument();
            
            try
            {
                if (dt.Rows.Count > 0)
                {
                    repDoc.Load(this.p_rep_path);
                    repDoc.SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                    repDoc.SetDataSource(dt);                    
                    return repDoc;
                }

                return null;
            }
            catch (Exception e)
            {
                this.error_message = e.Message;    
                return null;
            }
            
        }
        public void getReportLookup()
        { 
            base.getUpdateDataSet("sp_get_reports_tbl");
        }
        public ArrayList  parse_parameters(string args,string delim)
        {
            ArrayList param_arr = new ArrayList();            

            foreach(string s in args.Split(   delim.ToCharArray()  ))
            {
                param_arr.Add(s); 
            }
            return param_arr;
        }
        public bool validate_report_parameter_length(string rep_params, string rep_datatype,string rep_values)
        {
            int rep_static_length = rep_params.Split(";".ToCharArray()  ).Length;
            if (rep_params.Split(";".ToCharArray() ).Length == rep_static_length && rep_datatype.Split(";".ToCharArray() ).Length == rep_static_length && rep_values.Split(";".ToCharArray() ).Length == rep_static_length )
            {
                error_message = "";
                return false;
            }
            else
            {
                error_message = "Invalid Parameter Length OR values";
                return true;
            }            
        }
        public bool check_datatypes_values()
        {
            
            bool pass = true ;
            for (int i = 0; i < rep_datatypes.Count; i++)
            {
                if (!this.eval_datatype(rep_datatypes[i].ToString(),rep_values[i].ToString()))
                {
                    pass = false;
                    this.error_message = "ERROR IN DATATYPE:" + rep_datatypes[i].ToString() + " VALUEOF " + rep_values[i].ToString();
                    break;
                }
                
               
            }
            return pass;
        }        
        public bool eval_datatype(string datatype, string rep_value)
        {
            bool pass = false;
            this.error_message = String.Empty;
            switch (datatype.ToLower())
            {
                case "string":
                    pass = true;
                    break;
                case "number":
                    int num;
                    pass = int.TryParse(rep_value, out  num);
                    
                    break;
                case "date":
                    DateTime date1;
                    pass = DateTime.TryParse(rep_value, out date1);
                    break;                
                default:
                    this.error_message = "ERROR IN DATATYPE:" + datatype + " VALUEOF " + rep_value;
                    pass = false;
                    break;
            }
            return pass;
        }
        
        #region DATABASE ACCESS OF REPORTS
        public string isokforload()
        {
            irsConfiguration conf = new irsConfiguration();
            string reportpath = null;
            string rootpath = conf.appsetting("ReportPath");
            reportpath = rootpath + p_rep_path.Replace(@"/", @"\");
            this.load_report_dataset();
            try
            {
                if (this.rep_dataset.Tables[0].Rows.Count == 0)
                {
                    return "";
                }

                if (new csFileIo().exist(reportpath))
                {
                    return reportpath;
                }
                return "";
            }
            catch (Exception e)
            {
                this.writereportlogs(e.StackTrace);
                return "";
            }

        }

        public DataTable check_report(string reportname)
        {
            string sql = "select count(1) from tbl_reports where textdisplay = '" + reportname + "'";

            return base.getDataSet(base.getSqlCommand(sql,false)).Tables[0];
        }
        public ReportDocument  getReportDataSet()
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument  repDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            
            irsConfiguration conf = new irsConfiguration();
            csAuditLog ca = new csAuditLog();
            DataTable dt1 = null;
            DataTable dt2 = null;
            try
            {

               
                string reportpath = "";
                if (this.p_sp_name.ToLower().Contains("otc"))
                {        
                    if (this.isDotMatrix(this.loggedbranch))
                    {
                        if (this.p_sp_name.ToLower().Contains("obp"))
                        {
                            this.sp_name = "sp_otc_obp_receipt_reprint_preprinted";
                           
                        }
                        else
                        {
                            this.sp_name = "sp_otc_reprint_receipt_preprinted";                         
                        }
                        this.rep_path = "/Standard/rptTranxReceipt.rpt";
                    }
                    else
                    {
                        if (this.p_sp_name.ToLower().Contains("guam"))
                        {
                            if (this.p_sp_name.Contains("OTC"))
                            {
                                this.rep_path = "/OTC/arpt_Tagalog_Receipt_OTC_GUAM.rpt";
                            }
                            else
                            {
                                this.rep_path = "/OTC/arpt_Tagalog_Receipt_OBP_GUAM.rpt";
                            }
                        }
                        else if (this.p_sp_name.ToLower().Contains("rcn"))
                        {
                            if (this.p_sp_name.Contains("OTC"))
                            {
                                this.rep_path = "/OTC/tagalog_OTC_RCN.rpt";
                            }
                            else
                            {
                                if (this.p_sp_name.Contains("Agent"))
                                    this.rep_path = "/OTC/rptReceipt_parameterize_OBP_Agent.rpt";
                            }
                        }
                        else if (this.p_sp_name.Contains("HK"))
                        {
                            if (this.GetReceiptType(this.loggedbranch) == 5)
                            {
                                this.rep_path = "/OTC/HK_receipt_onepage.rpt";
                            }
                            else 
                            {
                               this.rep_path = "/OTC/HK_OTC_reprint.rpt";
                            }
                        }
                        else
                        {  
                            if (this.isOTCReportHalfPage(IRSClientData.Location))
                            {
                                this.rep_path = "/OTC/rptReceipt_Half.rpt";
                            }
                        }
                    }
                    
                }
                
                
                

                this.load_report_dataset();

                reportpath = this.isokforload();

                if (this.checkars(this.loggedbranch) && this.sp_name.ToLower().Contains("obp") )
                {
                           
                    string rootpath = conf.appsetting("ReportPath");

                    if (this.isOTCReportHalfPage(IRSClientData.Location))
                    {
                        //if (this.isTagalogReport(this.loggedbranch))
                        //{
                        //    reportpath = rootpath + @"\OTC\rptReceipt_parameterize_half_OBP.rpt";
                        //}
                        //else
                        //{
                        //    reportpath = rootpath + @"\OTC\rptReceipt_ARSHalf.rpt";
                        //}       

                        if (this.GetReceiptType(this.loggedbranch) == 2)
                        {
                            reportpath = rootpath + @"\OTC\rptReceipt_parameterize_half_OBP.rpt";
                        }
                        else if (this.GetReceiptType(this.loggedbranch) == 3)
                        {
                            if (this.p_sp_name.ToLower().Contains("agent"))
                            {
                                reportpath = rootpath + @"\OTC\rptReceipt_parameterize_OBP_Agent.rpt";
                            }
                            else
                            {
                                reportpath = rootpath + @"\OTC\tagalog_OBP_RCN.rpt";
                            }
                        }
                        else if (this.GetReceiptType(this.loggedbranch) == 4)
                        {
                            reportpath = rootpath + @"\OTC\arpt_Tagalog_Receipt_OBP_GUAM.rpt";
                        }
                        else
                        {
                            reportpath = rootpath + @"\OTC\rptReceipt_ARSHalf.rpt";
                        }
                    }
                    else
                    {
                        if (this.GetReceiptType(this.loggedbranch) == 2)
                        {
                            reportpath = rootpath + @"\OTC\rptReceipt_parameterize_OBP.rpt";
                        }
                        else if (this.GetReceiptType(this.loggedbranch) == 3)
                        {
                            reportpath = rootpath + @"\OTC\tagalog_OBP_RCN.rpt";
                        }
                        else if (this.GetReceiptType(this.loggedbranch) == 4)
                        {
                            reportpath = rootpath + @"\OTC\arpt_Tagalog_Receipt_OBP_GUAM.rpt";
                        }
                        else
                        {
                            reportpath = rootpath + @"\OTC\rptReceipt_parameterize_OBP.rpt";
                        }
                        
                    }
                    
                }
                if (reportpath != "")
                {
                    
                    repDoc.Load(reportpath);
                    if (repDoc.Subreports.Count == 1)
                    {
                        repDoc.Subreports[0].SetDataSource(this.rep_dataset.Tables[1]);
                        repDoc.Subreports[0].SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                    }

                    repDoc.SetDataSource(this.rep_dataset.Tables[0]);
                    repDoc.SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);

                    if (this.p_sp_name.ToLower().Contains("rptpnb_eoddetails_preview"))
                    {   
                        
                        string branchode = this.get_param_val("@v_branchcode");
                        string tranxdate = this.get_param_val("@sDate");
                        dt1 = this.getbenecharge(branchode, Convert.ToDateTime(tranxdate)).Tables[0];
                        dt2 = this.getforexrate(branchode, Convert.ToDateTime(tranxdate)).Tables[0];
                        repDoc.SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                        repDoc.Subreports[0].SetDataSource(dt1);
                        repDoc.Subreports[0].SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                        repDoc.Subreports[1].SetDataSource(dt2);
                        repDoc.Subreports[1].SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                    }
                    if (this.p_sp_name.ToLower().Contains("rptpnb_preeoddetails_revised"))
                    {

                        string branchode = this.get_param_val("@v_branchcode");
                        string tranxdate = this.get_param_val("@pre_sDate");
                        dt1 = this.getprebenecharge(branchode, Convert.ToDateTime(tranxdate)).Tables[0];
                        dt2 = this.getforexrate(branchode, Convert.ToDateTime(tranxdate)).Tables[0];
                        repDoc.SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                        repDoc.Subreports[0].SetDataSource(dt1);
                        repDoc.Subreports[0].SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                        repDoc.Subreports[1].SetDataSource(dt2);
                        repDoc.Subreports[1].SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                    }
                    this.error_message = "";
                    

                    return repDoc;
                }
                else
                {
                    this.error_message = "ERROR: EMPTY DATA";
                    try
                    {
                        if (IRSClientData != null)
                        {
                            try
                            {
                                ca.writeAudit(p_user, "IRSREPORT CLASS", "EMPTY DATA", "", "", csAuditLog.Category.Select);
                            }
                            catch { }

                        }
                    }
                    catch { }
                    return null;
                }


            }
            catch (Exception e)
            {
                this.error_message = "ERROR: " +  e.Message + " " + e.StackTrace ;
                try
                {
                    ca.writeAudit(p_user, "IRSREPORT CLASS", e.Message + ":" + e.StackTrace, "", "", csAuditLog.Category.Select);
                }
                catch { }

                throw new Exception(this.error_message);
                
                
            }
            finally
            {
                conf = null;
                if (ca != null) ca = null;
                if (dt1 != null)
                {
                    dt1.Dispose(); dt1 = null;
                }
                if (dt2 != null)
                {
                    dt2.Dispose(); dt2 = null;
                }
            }
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
                ca.writeAudit(p_user
                                                , "OTCTR", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
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
                ////ca.writeAudit(this._userid                                                , "OTCGRT", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return 0;
        }
        public string getReportDataSetAndExportPath()
        {

            ReportDocument repDoc = new ReportDocument();
            DataSet dynaDS = new DataSet();
            csFileIo io = new csFileIo();
            try
            {
                SqlCommand cmd = new SqlCommand();
                dynaDS = null;
                cmd = base.getSqlCommand(this.p_sp_name, true);
                if (cmd != null)
                {
                    this.PrepareParameters(cmd);
                    dynaDS = base.getDataSet(cmd);
                    if (dynaDS.Tables.Count > 0)
                    {
                        if (io.exist(p_rep_path))
                        {
                            repDoc.Load(this.p_rep_path);
                            repDoc.SetDatabaseLogon(this.p_user, this.p_password, this.p_server, this.p_database);
                            repDoc.SetDataSource(dynaDS.Tables[0]);

                            if (display_report_in_pdf(repDoc))
                            {

                                if (IRSClientData != null)
                                {
                                    this.writereportlogs("//REPORTS/EXPORTDIR//" + io.getFilename(this.p_rep_pdfpath) + "user:" + this.IRSClientData.UserId);
                                }

                                return "//REPORTS/EXPORTDIR//" + io.getFilename(this.p_rep_pdfpath);
                            }
                            else
                            {
                                if (IRSClientData != null)
                                {
                                    this.writereportlogs("ERROR: " + this.Message + "user:" + this.IRSClientData.UserId);
                                }
                                return "ERROR: " + this.Message ;
                            }
                        }
                        else
                        {
                            this.error_message = "Invalid Report Path";
                            if (IRSClientData != null)
                            {
                                this.writereportlogs("Invalid Report Path" + "user:" + this.IRSClientData.UserId);
                            }
                            return "ERROR: Invalid Report Path";
                        }
                    }

                }
                if (IRSClientData != null)
                {
                    this.writereportlogs("ERROR: GENERATING REPORT" + "user:" + this.IRSClientData.UserId);
                }
                return "ERROR: GENERATING REPORT";
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
                if (IRSClientData != null)
                {
                    this.writereportlogs(e.Message + "user:" + this.IRSClientData.UserId);
                }
                return "ERROR:" + e.Message;
            }
            finally
            {
                io = null;
                repDoc.Dispose();
            }


        }
        public DataSet load_report_group_lookup(string branchcode,int isadmin)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                cmd = base.getSqlCommand( "sp_get_rep_group");
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 5).Value = branchcode;
                cmd.Parameters.Add("@v_isadmin", SqlDbType.Int).Value = isadmin;
                ds = base.getDataSet(cmd );
                return ds;
            }
            catch 
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }

        }
        private void create_report_path()
        {
            csFileIo io = new csFileIo();
            string temppath = HttpContext.Current.Server.MapPath("//REPORTS//EXPORTDIR");

            if (!io.directoryExist(temppath))
            {
                io.createDirectory(HttpContext.Current.Server.MapPath("~") + @"\REPORTS\EXPORTDIR");
            }

        }
        public bool ExportReport(ReportDocument rep)
        {
             this.rep_sessionid  = HttpContext.Current.Session.SessionID ;
             csFileIo io = new csFileIo();
             try
             {
                 this.create_report_path();
                 this.rep_pdfpath = io.getDirectory(this.p_rep_path) +  @"\"  + rep_sessionid + ".pdf";
                 rep.ExportToDisk(ExportFormatType.PortableDocFormat, this.p_rep_pdfpath );
                 return true;
             }
             catch (Exception e)
             {
                 this.error_message = e.Message;
                 return false;
             }
             finally
             {

                 io = null;
             }
        }
        public DataSet load_report_maingroup(string mgroupid)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                cmd = base.getSqlCommand("sp_get_reports");
                cmd.Parameters.Add("@maingroupid", SqlDbType.VarChar, 50).Value = mgroupid;
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }
        public DataSet load_report_per_branch(string subgroupid)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;              
                cmd = base.getSqlCommand("sp_get_reportpercategory");
                cmd.Parameters.Add("@v_subgroupid", SqlDbType.VarChar, 50).Value = subgroupid;                
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }
        public DataSet load_report_group_lookup_per_branch(string subgroupid, string branchcode, string role)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;

                if (branchcode == null)
                    branchcode = "";
                cmd = base.getSqlCommand("sp_get_rep_group_perbranch_revised");
                cmd.Parameters.Add("@v_subgroupid", SqlDbType.VarChar, 50).Value = subgroupid;
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 5).Value = branchcode;
                cmd.Parameters.Add("@v_role", SqlDbType.VarChar, 100).Value = role;
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }
        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            bool ok = false;
            this.load_sp_params(ht);
            ht["MainGroupID"] = "REPORTS";
            ht["MainGroupDESC"] = "REPORTS";
            if (!base.isUpdatable)
            {
                this.getReportLookup();
            }

            if (crud.Equals("DELETE"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"])); ;
                return base.Delete(sqlHelperWs.RETAIN_DATASET);
            }
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                ok = base.edit();
            }
            if (crud.Equals("ADD"))
            {
                ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            if (ok)
            {
                
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (!setColumn(key.ToString(), ht[key]))
                    {
                        ok = false;
                        break;
                    }
                    
                        
                   
                }
                if (ok & addMode)
                {
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }
            return ok;
        }
        public DataTable saveUpdates(string maingroupid)
        {
            if (base.save(sqlHelperWs.RETAIN_DATASET))
            {
                return this.load_report_maingroup(maingroupid).Tables[0];
            }

            return null;
        }
        public Object[] getFs(String root)
        {
            short seqno = 0;
            short parid = -1;
            String item = "IRSWorld-REPORTS";
            ArrayList al = new ArrayList();
            al.Add(new Object[] { seqno, parid, item, "R" });

            listDirectories(root, ref seqno, seqno, al);

            if (al.Count > 0)
            {
                Object[] lsdir = new Object[al.Count];
                al.CopyTo(lsdir);

                return lsdir;
            }

            return null;
        }
        public void listDirectories(String s, ref short seqno, short parid, ArrayList al)
        {
            String[] ff = Directory.GetFileSystemEntries(s);
            foreach (String f in ff)
            {
                FileAttributes fa = File.GetAttributes(f);
                if (fa > 0)
                {
                    if (fa == FileAttributes.Directory)
                    {
                        seqno++;
                        al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "D" });
                        listDirectories(f, ref seqno, seqno, al);
                    }
                    else
                    {
                        String ext = Path.GetExtension(f);
                        if (ext != null && ext.Length > 0)
                        {
                            ext = ext.ToUpper();
                            if (ext.Equals(".RPT") | ext.Equals(".rpt"))
                            {
                                seqno++;
                                al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "F" });
                            }
                        }
                    }
                }
            }
        }
        public bool display_report_in_pdf(ReportDocument oReportDoc)
        {
            try
            {
                MemoryStream stream = (MemoryStream)oReportDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                oReportDoc.Close();
                WriteToStream(stream,this.p_sp_name  ,"" );
                return false;
            }
            catch(Exception e)
            {
                this.error_message = e.Message;
                return false;
            }
        }
        public void clean_data_used_report()
        {
            
        }
        #endregion
        #endregion
        #region PRIVATE METHOD
        private irsClientData IRSClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }
                return null;
            }
        }
        private void WriteToStream(MemoryStream stream, string reptitle, string repdesc)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader(reptitle, repdesc);
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            HttpContext.Current.Response.End();
        }
        private void populate_report_values(string delimparam)
        {
            String[] sdelipar = delimparam.Split('^');
            this.clear_arrays();
            String[] sarr1 ;
            for (int i = 0; i < sdelipar.Length; i++)
            {   
                sarr1 = sdelipar[i].Split(';');
                if (sarr1[0].Length > 0)
                {
                    this.rep_param.Add(sarr1[0]);
                    this.rep_values.Add(sarr1[1]);
                }
            }
        }

        private void PrepareParameters(SqlCommand sqlcmd)
        {
            for (int i = 0; i < rep_param.Count; i++)
            {
                String pname = rep_param[i].ToString();
                pname = pname.Substring(pname.IndexOf('@'));
                SqlParameter p = null;
                try
                {
                    p = sqlcmd.Parameters[pname];
                }
                catch { }

                if (p != null && (p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput))
                {
                    p.Value = convertType(p.DbType, rep_values[i].ToString());
                }
            }
        }

        public DataTable get_sp_params(string spname)
        {
            SqlCommand sqlcmd = null;
            try
            {
                sqlcmd = base.deriveParameters(spname);

                string paramnames = null;
                string paramtypes = null;


                DataTable dt = new DataTable("REPORTSPTBL");
                DataRow r = null;


                dt.Columns.Add("ParameterNames");
                dt.Columns.Add("ParameterTypes");
                dt.Columns.Add("ParameterDisplay");



                foreach (SqlParameter p in sqlcmd.Parameters)
                {
                    if (p.Direction == ParameterDirection.Input)
                    {
                        paramnames += p.ParameterName + "$";
                        paramtypes += p.SqlDbType.ToString() + "$";
                        r = dt.NewRow();
                        r["ParameterNames"] = paramnames;
                        r["ParameterTypes"] = paramtypes;
                        r["Parameterdisplay"] = paramnames;
                        dt.Rows.Add(r);
                        dt.AcceptChanges(); 
                    }
                }

                return dt;

            }
            catch (Exception e)
            {
                this.error_message = e.Message;
            }
            finally { sqlcmd.Dispose(); sqlcmd = null; }
            return null;
        }
        private void load_sp_params(Hashtable ht)
        {
            SqlCommand sqlcmd = new SqlCommand();
            try
            {
                sqlcmd = base.getSqlCommand(ht["sp_Name"].ToString(), true);
                sqlcmd = base.deriveParameters(ht["sp_Name"].ToString());

                string paramnames = null;
                string paramtypes = null;
                foreach (SqlParameter p in sqlcmd.Parameters)
                {
                    if (p.Direction == ParameterDirection.Input)
                    {
                        paramnames += p.ParameterName + "$";
                        paramtypes += p.SqlDbType.ToString() + "$";
                    }
                }
                if (!ht["ParameterNames"].ToString().Contains("$") )
                {
                    ht["ParameterNames"] = paramnames;
                    ht["Parametertypes"] = paramtypes;
                    ht["ParameterDisplay"] = paramnames.Replace("@","");
                }
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
            }
            finally { sqlcmd.Dispose(); sqlcmd = null; }
        }
        private void clear_arrays()
        {
            try
            {
                this.rep_param.Remove(0);
                this.rep_datatypes.Remove(0);
                this.rep_values.Remove(0);
            }
            catch { }
        }
        private void get_error_dbase()
        {
            this.error_message = string.Empty;

            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }
        private bool setSession(int repid)
        {

            DataSet ds = new DataSet();
            SqlCommand cmd = base.getSqlCommand("sp_get_reportbyid", true);
            cmd.Parameters.Add("@v_id", SqlDbType.Int).Value = repid;
            ds = base.getDataSet(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Web.HttpContext.Current.Session["REP_PATH"] = ds.Tables[0].Rows[0][1].ToString();
                this.rep_path = ds.Tables[0].Rows[0][1].ToString();
                this.error_message = "";
                ds.Dispose();
                ds = null;
                cmd.Dispose();
                cmd = null;
                return true;
            }
            else
            {
                this.error_message = "CANNOT FIND REPORT ID";
                return false;
            }
        }
        private void init_session()
        {
            System.Web.HttpContext.Current.Session["REP_DATASET"] = null;
            System.Web.HttpContext.Current.Session["REP_PATH"] = null;
            System.Web.HttpContext.Current.Session["REP_PRINTER"] = null;
        }
        private void init_rep_con()
        {
            ArrayList arr = new ArrayList();
            string connstring = new irsConfiguration().ConnectionString;
            arr = this.parse_parameters(connstring, ";");
            this.load_rep_conn(arr);
            arr = null;
        }
        private void writereportlogs(string msg)
        {
            csFileIo io = new csFileIo();
            irsConfiguration conf = new irsConfiguration();
            string rep_path = conf.appsetting("ReportLogPath");
            string fullpath = rep_path + @"\reportlogs.txt";
            if (io.exist(fullpath ))
            {
                io.open_output(fullpath , (int)csFileIo.write_modes.APPEND);
            }
            else
            {
                io.open_output(fullpath, (int)csFileIo.write_modes.CREATE );
            }
            String s = "DATE:" + DateTime.Today.ToString() + " " + msg;
            io.putLine(s);
            io.close_output();

            io = null;
        }
        private void init_parameters(string mergeparam)
        {
            ArrayList arr = new ArrayList();
            arr = this.parse_parameters(mergeparam, "^");
            this.sp_name = arr[4].ToString();
            this.rep_path = arr[5].ToString();
            arr = null;
            this.init_rep_con();
        }
        private void load_rep_conn(ArrayList arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {

                if (arr[i].ToString().ToLower().Contains("server"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.servername = arr1[1].ToString();
                }
                if (arr[i].ToString().ToLower().Contains("database"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.dbname = arr1[1].ToString();
                }
                if (arr[i].ToString().ToLower().Contains("user"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.username = arr1[1].ToString();
                }
                if (arr[i].ToString().ToLower().Contains("password"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.password = arr1[1].ToString();
                }

            }
        }
        private void load_report_dataset()
        {
            SqlCommand cmd = base.deriveParameters(this.p_sp_name);

            this.rep_dataset  = null;
            base.EnforceConstraint = false;
            if (cmd != null)
            {
                this.PrepareParameters(cmd);
                if (this.rep_dataset != null)
                    this.rep_dataset = null;
                this.rep_dataset  = base.getDataSet(cmd,"TableDynamicReports");
                
            }
          
        }

        private DataSet getbenecharge(string branchcode, DateTime tranxdate)
        {
            irsEndOfDay eod = new irsEndOfDay();
            string param = tranxdate.ToString("MM/dd/yyyy") + "," + branchcode;
            return eod.execStoredProc("proc_BeneCharges", param.Split(','));
        }
        private DataSet getprebenecharge(string branchcode, DateTime tranxdate)
        {
            irsEndOfDay eod = new irsEndOfDay();
            string param = tranxdate.ToString("MM/dd/yyyy") + "," + branchcode;
            return eod.execStoredProc("proc_preBeneCharges", param.Split(','));
        }
        private DataSet getforexrate(string branchcode, DateTime tranxdate)
        {
            irsEndOfDay eod = new irsEndOfDay();
            string param = tranxdate.ToString("MM/dd/yyyy") + "," + branchcode;
            return eod.execStoredProc("rpt_EODForexRates_revised", param.Split(','));
        }
        private bool checkars(string branchcode)
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
                ca.writeAudit("checkars"
                                               , "checkars", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                return false;
            }
            finally
            {
                dt = null;
                ca = null;
            }
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
                ca.writeAudit(this.IRSClientData.UserId, "RVISP", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return false;
        }
        public string get_param_val(string paramname)
        {
            for (int i = 0; i < rep_param.Count; i++)
            {
                if (rep_param[i].ToString().ToUpper().Contains(paramname.ToUpper()))
                {
                    return rep_values[i].ToString(); 
                }
            }
            return "";
        }
        public DataSet getReport_DataSet()
        {
            return this.rep_dataset;
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
                ca.writeAudit(IRSClientData.UserId
                                                , "IRSRRHP", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return false;
        }
        
        #endregion

       
    }
}
