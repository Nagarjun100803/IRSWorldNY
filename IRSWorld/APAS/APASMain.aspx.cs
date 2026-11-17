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
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using IRSWorld.TranClass;
namespace IRSWorld.APAS
{
    public partial class APASMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASMain));
        }

        #region  PRIVATE METHODS, VARIABLES, PROPERTIES
        private string _tbl_name = "v_APAS_trxn";
        private string _param = null;
        private string _error = null;

        private irsIRIS doAPAS
        {
            get
            {
                if (Session["APAS"] == null)
                {
                    Session["APAS"] = new irsIRIS();
                }
                return (irsIRIS)Session["APAS"];
            }
        }
        private irsAPAS objAPAS
        {
            get
            {
                if (Session["objAPAS"] == null)
                {
                    Session["objAPAS"] = new irsAPAS();
                }
                return (irsAPAS)Session["objAPAS"];
            }
        }
        private void set_error()
        {
            Session["ServerError"] = null;
            Session["ServerError"] = this._error;
        }
        private irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }
                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }
        private irsTimeZone irsTimeZone
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_TIME_ZONE"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_TIME_ZONE"] = new irsTimeZone();
                }
                return (irsTimeZone)System.Web.HttpContext.Current.Session["IRS_TIME_ZONE"];
            }
        }
        
        private void irs_clientdata(Object[] data)
        {
            System.Web.HttpContext.Current.Session["CLIENTDATA"] = data;            
        }

        #endregion
        #region PUBLIC METHODS, PROPERTIES

        #endregion

        #region PUBLIC AJAX METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object clientdata()
        { return (Object)System.Web.HttpContext.Current.Session["CLIENTDATA"]; }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getClientDataBranchCode()
        {
            string branchcode = irsClientData.Location;
            return branchcode;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int RoleID()
        {
            return irsClientData.Role; 
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] verify_password(String userid, String pwd)
        {
            string brcode   = "";
            int tries       = 1;
            string location = "";
            short role      = 0;
            string approver = "";
            string user     = "";
            
            irsUsers iusr = new irsUsers();
            Object[] oo = iusr.validate(userid, pwd, Convert.ToByte(tries), true);
            bool ok = (bool)oo[0];
            if (ok)
            {
                brcode = iusr.getColumn("Location").ToString();
                role = Convert.ToInt16(iusr.getColumn("Role"));
                if (irsClientData.Location != brcode)
                {
                    oo[0] = false;
                    oo[1] = "The user is not within the location.";
                }
                else
                {
                    string[] approvers = getApproverRoles().Split(',');
                    for (int x = 0; x <= approvers.Length - 1; x++)
                    {
                        if (role != Convert.ToInt16(approvers[x]))//approver
                        {
                            oo[0] = false;
                            oo[1] = "The users role is not a member of allowed approver.";
                        }
                        else
                        {
                            oo[0] = true;
                            oo[1] = "";
                            break;
                        }
                    }
                }
            }

            approver        = userid; 
            location        = irsClientData.Location;
            user            = irsClientData.UserId;
            DateTime _date  = irsTimeZone.branchLocalTime(location);

            Object[] o = new Object[] {oo, brcode, role, approver, user, _date};
            //object[] o = new object[] {t,brcode,role};
            this.irs_clientdata(o);
            return o;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string do_process_apas(string instruction, string column_values, string trxn_id)
        {            
            bool ibool = false;
            if (trxn_id == "AUDIT")
            {
                objAPAS.TableName = "TBL_APAS_AUDIT_LOG";
            }
            else
            {
                objAPAS.TableName = "TBL_REMITTANCE_APAS";
            }

            switch (instruction)
            {
                case "INSERT": ibool = objAPAS.insert_record(column_values); break;
                case "UPDATE": ibool = objAPAS.update_record(column_values, (Object)trxn_id); break;
            }
            if (!ibool)
            { 
                this._error = objAPAS.messages; 
                this.set_error(); 
            }
            return "success";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool fundTransfer(string source_acct, string target_acct, string refno, string amt,Int32 seq_id)
        {
            bool ok = objAPAS.fundTransfer(source_acct, target_acct, refno, amt,seq_id);
            if (!ok)
            {
                throw new Exception(objAPAS.Message);
            }
            return true;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_irsclient_data(string param)
        {
            string s      = null;
            switch (param)
            {
                case "UID": s       = irsClientData.UserId;     break;
                case "CIF": s       = irsClientData.CIFID;      break;
                case "LOCATION": s  = irsClientData.Location;   break;
                case "IP": s        = irsClientData.IPAddress;  break;
            }
            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_time_zone()
        {
            return irsTimeZone.branchLocalTime(irsClientData.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_details(string param)
        {
            try
            {
                this._param = param;
                DataSet ds = new DataSet();
                doAPAS.table_name = _tbl_name;
                doAPAS.param = _param;
                ds = doAPAS.getDataSet();
                if (ds != null)
                { return ds; }               
            }
            catch{}
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_apas_trxnstat()
        {
            try
            {   
                DataSet ds = new DataSet();
                doAPAS.table_name = "TBL_LOOKUP";
                doAPAS.param = " where FldGroup = 'APASTransactionStatus' And FldCode != 4";
                ds = doAPAS.getDataSet();
                if (ds != null)
                { return ds;}                
            }
            catch{}
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRequiredID(string branchcode)
        {
            return objAPAS.getRequiredID(branchcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getPartnerAccountNo(string partnercode, int currencycode)
        {
            return objAPAS.getPartnerAccountNo(partnercode, currencycode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet CheckIfLockTransaction(Int32 SEQ_ID)
        {
            return objAPAS.CheckIfLockTransaction(SEQ_ID); 
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUsername(string CIFID)
        {
            return objAPAS.getUsername(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchName(string brcode)
        {
            return objAPAS.getBranchName(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAllOnBehalfBranches(string parentcode)
        {
            return objAPAS.getAllOnBehalfBranches(parentcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getAllowedCurrencies(string branchcode)
        {
            return objAPAS.getAllowedCurrencies(branchcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getAgentPayoutCurrency(int currcode, string brcode , bool isHeadOffice)
        {
            return objAPAS.getAgentPayoutCurrency(currcode, brcode, isHeadOffice);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isHeadOffice(string brcode)
        {
            return objAPAS.isHeadOffice(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getMotherBranchCode(string brcode)
        {
            return objAPAS.getMotherBranchCode(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCurrencyCode(int currcode)
        {
            return objAPAS.getCurrencyCode(currcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getORDAccountNo(int currcode,int moduleid)
        {
            return objAPAS.getORDAccountNo(currcode,moduleid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ordtrunkline()
        {
            return objAPAS.ordtrunkline();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchAccountNo(int currcode, string branchcode)
        {
            return objAPAS.getBranchAccountNo(currcode, branchcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool ShowReport(string refno)
        {
            //string report_path = Server.MapPath("../Reports/APAS/APASReceipt.rpt");
            try
            {
                //DataSet ds = objAPAS.getAPASReceipt(refno);
                Session["APASRefNo"] = refno;
                Session["APASReceipt"] = "APAS";


                //irsReports _report = new irsReports(ds.Tables[0], report_path, true);
                //HttpContext.Current.Session["IRS_ReportsUI"] = getReportDataSet("APASReceipt", ds);
                //_report.display_report();
                return true;

            }
            catch (Exception ex)
            {
                objAPAS.Message = ex.Message;
                return false;
            }
            //return false;
        }

        private string rptpath;
        public ReportDocument getReportDataSet(string rptnm, DataSet ds)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument repDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            try
            {
                this.rptpath = Server.MapPath("../Reports/APAS/APASReceipt.rpt");
                repDoc.Load(this.rptpath);
                repDoc.SetDataSource(ds.Tables[0]);
                return repDoc;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTranxStatus(string refno)
        {
            return Convert.ToString(objAPAS.getTranxStatus(refno));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string CIFClass(string cifid)
        {
            return objAPAS.CIFClass(cifid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchNamePerUser(string uid)
        {
            return objAPAS.getBranchNamePerUser(uid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_branch_localtime(string brcode)
        {
            DateTime dt = new irsTimeZone().branchLocalTime(brcode);
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet CheckIfCancelTransaction(int seq_id)
        {
            return objAPAS.CheckIfCancelTransaction(seq_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet CheckIfWithAmendments(int seq_id)
        {
            return objAPAS.CheckIfWithAmendments(seq_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getApproverRoles()
        {
            return new AppSettingsReader().GetValue("ApproverRoles", Type.GetType("System.String")).ToString(); 
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getORDBranchCode()
        {
            return new AppSettingsReader().GetValue("ORDBranchCode", Type.GetType("System.String")).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAgent()
        {
            return new AppSettingsReader().GetValue("Agent", Type.GetType("System.String")).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public decimal getTransactionLimit(string brcode, string currcode)
        {
            return objAPAS.getTransactionLimit(brcode, currcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isPawUser(int roleid)
        {
            return new AppSettingsReader().GetValue("PawUsers", Type.GetType("System.String")).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isHold(int seq_id)
        {
            return objAPAS.isHold(seq_id);
        }





        #endregion
    }
}
