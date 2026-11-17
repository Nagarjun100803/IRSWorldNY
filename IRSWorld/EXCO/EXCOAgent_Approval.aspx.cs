using System;
using System.Text;
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

namespace IRSWorld.EXCO
{
    public partial class EXCOAgent_Approval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EXCOAgent_Approval));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            csAuditLog audit = new csAuditLog();
            irsTimeZone it = new irsTimeZone();
            try
            {
                string userid = this.IRSClientData.UserId;
                DateTime dt = it.branchLocalTime(this.IRSClientData.Location);
                audit.writeAudit(userid, "EXCOAPPROVALPL", "PAGE LOAD", dt.ToString(), "", csAuditLog.Category.Select);
            }
            catch { }
            finally { audit = null; it = null; }
        }
        #region PRIVATE METHOD
        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] == null)
                {
                    return null;   
                }
                return (irsClientData)Session["IRS_CLIENT_DATA"];
            }
        }
        private irsEXCOApproval IRSEXCOApproval
        {
            get
            {
                if (Session["IRS_EXCO_APPROVAL"] == null)
                {
                    string branch = this.IRSClientData.Location;
                    string userid = this.IRSClientData.UserId;
                    Session["IRS_EXCO_APPROVAL"] = new irsEXCOApproval(branch,userid);                    
                }
                return (irsEXCOApproval)Session["IRS_EXCO_APPROVAL"];
            }
        }
        #endregion
        #region AJAX PUBLIC METHODS
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_exco_switch(string branchcode)
        {
            try
            {                
                return this.IRSEXCOApproval.get_exco_switch(branchcode);
            }
            catch
            {
                return null;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_exco_branches()
        {
            try
            {
                string userid = this.IRSClientData.UserId;
                return this.IRSEXCOApproval.get_exco_branches();
            }
            catch
            {
                return null;
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_exco_files(string _branchcode)
        {
            try
            {                
                return this.IRSEXCOApproval.get_exco_files(_branchcode);
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_exco_accountbalance(string _branchcode)
        {
            try
            {             
                return this.IRSEXCOApproval.get_exco_accounts_with_balance(_branchcode);
            }
            catch
            {
                return null;
            }
        }

        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_accounts(string _branchcode)
        {
            try
            {
                return this.IRSEXCOApproval.validate_accounts(_branchcode);
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_exco_file_details(string _branch,int _selectedfile,int _currency)
        {
            try
            {
                string userid = this.IRSClientData.UserId;
                return this.IRSEXCOApproval.get_exco_file_details(_branch, _selectedfile,_currency);
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_exco_currency_rates(string _branch, int _sourcecurrency, int _targetcurrency)
        {
            try
            {                
                return this.IRSEXCOApproval.get_currency_rates(_branch, _sourcecurrency, _targetcurrency);
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string approve_items(string _branch, int _fileinvseqno,string _sarr_args,string _sarr_funds)
        {
            try
            {
                ArrayList arr = new ArrayList();

                arr = this.IRSEXCOApproval.approve_items(_branch, _fileinvseqno, _sarr_args, _sarr_funds);

                this.IRSEXCOApproval.ErrorMessage.Clear();

                return string.Join("^", (string[])arr.ToArray(typeof(string)));
            }
            catch //(Exception e) // commented to remove warning
            {
                return "";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string force_push_items(string _branch, int _fileinvseqno, string _sarr_args, string _sarr_funds)
        {
            try
            {
                ArrayList arr = new ArrayList();

                arr = this.IRSEXCOApproval.force_push_items(_branch, _fileinvseqno, _sarr_args, _sarr_funds);

                this.IRSEXCOApproval.ErrorMessage.Clear();

                return string.Join("^", (string[])arr.ToArray(typeof(string)));
            }
            catch //(Exception e) // commented to remove warning
            {
                return "";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_logged_branch()
        {
            try
            {
                return this.IRSClientData.Location;
            }
            catch
            {
                return null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool send_exco_email(string _branchcode,string _emaildetails,string _fileformat,string _filetype)
        {            
            try
            {
                return this.IRSEXCOApproval.send_exco_email(_branchcode,_emaildetails, _fileformat, _filetype);                
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
