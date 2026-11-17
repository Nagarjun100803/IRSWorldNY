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
using System.Text;

using IRSClassLib;

namespace IRSWorld.Admin
{
    public partial class MenuProfiles : System.Web.UI.Page
    {

        
        private irsDefaultProfile IRSDefaultProfile
        {
            get
            {
                if (Session["IRS_DEFAULT_PROFILE"] == null)
                {
                    Session["IRS_DEFAULT_PROFILE"] = new irsDefaultProfile();
                }
                return (irsDefaultProfile)Session["IRS_DEFAULT_PROFILE"];
            }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(MenuProfiles));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_default_profile()
        {
            Session["IRS_DEFAULT_PROFILE"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaultProfile(int role)
        {
            DataTable dtmods = null;
            try
            {
                dtmods = IRSDefaultProfile.getDefaultProfile((short)role);
                if (dtmods == null)
                {
                    throw new Exception(IRSDefaultProfile.Message);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return dtmods;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getSetupData()
        {
            DataTable dtactypes = IRSDefaultProfile.getAccessTypes();
            if (dtactypes == null)
            {
                throw new Exception("Unable to load options for access types. " + IRSDefaultProfile.Message);
            }

            DataTable dtroles = IRSDefaultProfile.getUserRoles();
            if (dtroles == null)
            {
                throw new Exception("Unable to load options for user roles. " + IRSDefaultProfile.Message);
            }

            short first_role = Convert.ToInt16(dtroles.Rows[0]["fldValue"]);
            DataTable dtmods = getDefaultProfile(first_role);

            return new Object[] { dtactypes, dtroles, dtmods, first_role };
        }

        private DataTable getUpdateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("SEQ_ID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("MenuGroup", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("MenuParentID", typeof(System.Int32)));
            dt.Columns.Add(new DataColumn("AccessType", typeof(System.Int16)));
            dt.Columns.Add(new DataColumn("EffDate", typeof(System.DateTime)));
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable applyUpdates(String rec)
        {
            irsUsers irsUsers = new irsUsers();
            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
            if (rec != null && rec.Length > 0)
            {
                String[] ss = rec.Split('|');
                if (ss.Length > 1)
                {
                    String role = ss[0].Split('=')[1];
                    DataTable dt = getUpdateTable();
                    int errs = 0;
                    ArrayList al = new ArrayList();
                    for (int i = 1; i < ss.Length; i++)
                    {
                        String[] tt = ss[i].Split('~');
                        if (tt.Length < dt.Columns.Count)
                        {
                            errs++;
                            al.Add("Input column count < expected.");
                            continue;
                        }
                        try
                        {
                            DataRow row = dt.NewRow();
                            for (int j = 0; j < tt.Length - 1; j++)
                            {
                                row[j] = int.Parse(tt[j].Trim());
                            }
                            if (tt[tt.Length - 1].Length > 0)
                            {
                                row["EffDate"] = DateTime.Parse(tt[tt.Length - 1]);
                            }
                            else
                            {
                                row["EffDate"] = DBNull.Value;
                            }

                            dt.Rows.Add(row);
                        }
                        catch (Exception e)
                        {
                            errs++;
                            al.Add(e.Message);
                            continue;
                        }
                    }

                    if (errs > 0)
                    {
                        throw new Exception(IRSDefaultProfile.toMessage(al));
                    }

                    /* RFC-31620 - IRSW Maker-Checker Function for User Admin */

                    bool AdminApproverFlag = irsUsers.adminApproverSwitch();

                    DataTable rt = null;

                    if (!AdminApproverFlag)
                    { // OLD ROUTE
                        rt = IRSDefaultProfile.applyUpdates(role, dt);
                    } // NEED MAKER / CHECKER
                    else
                    {
                        rt = IRSDefaultProfile.applyUpdates_saveToTemp(role, dt);
                    }

                    if (rt == null)
                    {
                        throw new Exception(IRSDefaultProfile.Message);
                    }
                    return rt;
                }
            }

            throw new Exception("Input not in expected format."); ;
        }

        /* RFC-31620 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int isAdminApproverTag()
        {
            int ret = 0;
            if (Session["AdminApprovalFlagUser"] == null)
            {
                if (new irsUsers().adminApproverSwitch())
                {
                    ret = 1;
                }
            }
            return ret;
        }
    }
}
