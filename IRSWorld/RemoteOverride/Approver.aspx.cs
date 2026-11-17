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
namespace IRSWorld.RemoteOverride
{
    public partial class Approver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Approver));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX METHOD        

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_items_forApproval(string _seqid)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            irsTimeZone it = new irsTimeZone();
            try
            {
                string branchcode = this.IRSClientData.Location;
                DateTime tranxdate = it.branchLocalTime(branchcode);
                return iro.check_items_forApproval(branchcode,Convert.ToInt32(_seqid ),this.IRSClientData.UserId  ,tranxdate );
            }
            catch 
            {
                return null;
            }
            finally
            {
                iro = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean check_items_forApproval()
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            irsTimeZone it = new irsTimeZone();
            try
            {             
                string branchcode = this.IRSClientData.Location;
                DateTime tranxdate = it.branchLocalTime(branchcode);
                string userid = this.IRSClientData.UserId;
                return iro.check_items_forApproval(branchcode, tranxdate,userid);
            }
            catch
            {
                return false;
            }
            finally
            {
                iro = null;
                it = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean check_approver_roles(string productchargesid)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            try
            {
                //return iro.check_items_forApproval("VNS");
                return iro.check_approver_roles(this.IRSClientData.UserId  , this.IRSClientData.Role.ToString(), productchargesid);
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(this.IRSClientData.UserId, "REMOTEOVERRIDE", e.StackTrace, "", "", csAuditLog.Category.Select);  
                return false;
            }
            finally
            {
                iro = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int validate_password(string _password)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            //1 valid 0 not valid
            try
            {
                return iro.validate_password(this.IRSClientData.Location,
                                             this.IRSClientData.UserId,
                                             _password
                                             );
            }
            catch 
            {
                return 0;
            }
            finally
            {
                iro = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int cancel_items_forApproval(string _seqid)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            try
            {
                //return iro.check_items_forApproval("VNS");
                string userid = this.IRSClientData.UserId;
                return iro.cancel_items(this.IRSClientData.Location, Convert.ToInt32(_seqid),userid);
            }
            catch 
            {
                return 0;
            }
            finally
            {
                iro = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int approve_items(string _password,string _seq_id, int stat)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            //2 approved 3 disapproved
            try
            {
                return iro.approve_items(this.IRSClientData.Location,this.IRSClientData.UserId , _seq_id, _password, stat);
            }
            catch 
            {
                return 4;
            }
            finally
            {
                iro = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_branch_and_tranxdate()
        {
            irsTimeZone it = new irsTimeZone();
            try
            {
               string branchcode = this.IRSClientData.Location;
               return this.IRSClientData.Location + ";" + it.branchLocalTime(branchcode);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                it = null;
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
        #endregion
    }
}
