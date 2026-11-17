/*
 * AUTHOR       :   Jeffrey   M. Buizon
 * DATE CREATED :   04/04/2011
 * DESCRIPTION  :   Remote Override for IRSW
 */
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
    public partial class RemTeller : System.Web.UI.Page
    {
        // 1 : verifiying 2: approved 3: cancelled

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTeller));
        }

        #region AJAX METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string  tag_override_stat(string trandesc ,string _prodchargeid,string _splmsg,string _sdetails,string _overridetype)
        {            
            irsRemoteOverride iro = new irsRemoteOverride();            
            irsTimeZone it = new irsTimeZone();
            csAuditLog log = new csAuditLog();
            try
            {
                DateTime tranxdate = it.branchLocalTime(this.IRSClientData.Location);
                return iro.set_override_stat( IRSClientData.Location, trandesc,_prodchargeid, IRSClientData.UserId,_splmsg , _sdetails,_overridetype ,tranxdate);
            }
            catch (Exception e)
            {
                try
                {
                    log.writeAudit(this.IRSClientData.UserId, "REMOVEERR",
                                trandesc + ";" +
                                _prodchargeid + ";" +
                                 _splmsg + ";" +
                                _sdetails + ";" +
                                _overridetype,
                                e.Message + ":" + e.StackTrace,
                                "", csAuditLog.Category.Update);
                }
                catch { } 
                return "-1;ERROR:" + e.StackTrace ;
            }
            finally
            {
                iro = null;
                log = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_override_stat(string _overrideid)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            try
            {
                return iro.check_override_stat(_overrideid,IRSClientData.Location, IRSClientData.UserId);
            }
            catch (Exception e)
            {
                return "-1;" + e.Message + "; ERROR WHILE CHECKING";
            }
            finally
            {
                iro = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int cancel_override_stat(string _overrideid)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            try
            {
                return iro.cancel_override_stat(_overrideid, IRSClientData.Location, IRSClientData.UserId);
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
