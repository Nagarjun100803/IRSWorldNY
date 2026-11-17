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
    public partial class Menu : System.Web.UI.Page
    {

        private irsMenu IRSMenu
        {
            get
            {
                if (Session["IRS_MENU"] == null)
                {
                    Session["IRS_MENU"] = new irsMenu();
                }
                return (irsMenu)Session["IRS_MENU"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Menu));
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
        public void drop_irsmenu()
        {
            Session["IRS_MENU"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object getMenuData()
        {
            irsMenu menu = new irsMenu();
            DataTable dt = menu.getMenuView(); //(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataTable cdt = dt.Clone();
                    foreach (DataRow row in dt.Rows)
                    {
                        // Parameter substitution loop
                        DataRow uprow = cdt.NewRow();
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dt.Columns[i].ColumnName.Equals("ModuleUrl") & row[i] != DBNull.Value)
                            {
                                String url = (String)row[i];
                                int idx = url.ToUpper().IndexOf("$USERID");
                                if (idx > -1)
                                {
                                    String uid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                    if (uid == null || uid.Length < 1) uid = "";
                                    url = url.Replace("$USERID", uid);
                                }
                                uprow[i] = url;
                            }
                            else
                            {
                                uprow[i] = row[i];
                            }
                        }
                        cdt.Rows.Add(uprow);
                    }
                    return cdt;
                }
                else
                    return dt;
            }

            throw new Exception("GET MENU: " + menu.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object applyChanges(String changes)
        {
            if (changes != null && changes.Length > 0)
            {
                String[] ss = changes.Split('~');
                if (ss.Length > 1)
                {
                    Hashtable ht = new Hashtable();
                    for (int i = 0; i < ss.Length; i++)
                    {
                        String[] cols = ss[i].Split('=');
                        if (cols.Length > 1)
                        {
                            ht.Add(cols[0], cols[1]);
                        }
                    }

                    //=======================================================================
                    //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
                    string action = string.Empty;
                    string oldval = string.Empty;
                    string newval = string.Empty;
                    string crudval = "";
                    string menunameval = "";

                    crudval = ht["crud"].ToString().ToUpper();
                    menunameval = ht["MenuName"].ToString();
                    newval = menunameval;

                    if (IRSMenu.IsMenuActionAllowed(crudval, menunameval))
                    {
                        action = crudval.ToUpper() + " NOT SUCCESSFUL. There is a pending approval for the same Menu Name : " + menunameval;
                        new csAuditLog().writeVAPTLogger("", "ADMIN", action, oldval, newval, "", csAuditLog.AuditType.Systems, csAuditLog.AlertType.Information);
                        throw new Exception(action);
                    }
                    //=======================================================================

                    if (IRSMenu.applyUpdates(ht))
                    {
                        DataTable dt = IRSMenu.saveUpdates();
                        if (dt != null) return dt;
                    }

                    throw new Exception("IRSMenu update error: " + IRSMenu.Message);
                }
            }

            throw new Exception("IRSMenu update: parameter invalid.");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int getNextMenuItemNo(String gid, String pid)
        {
            int gno = -1;
            int pno = -1;
            try
            {
                gno = int.Parse(gid);
                pno = int.Parse(pid);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            int ino = IRSMenu.getNextMenuItemNo(gno, pno);
            if (ino == -1)
            {
                throw new Exception(IRSMenu.Message);
            }

            return ino;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int getLastGroupNo()
        {
            int gid = IRSMenu.getLastGroupNo();
            if (gid > 0) return gid;

            throw new Exception("GetLastGroupNo: " + IRSMenu.Message);
        }

        //=======================================================================
        //RFC-33165 : Phase 2_IRSW Maker-Checker for Parameter Admin Function
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool ParamApproverFlag()
        {
            irsMenu menu_approval = new irsMenu();

            try
            {
                return menu_approval.ParamApproverFlag();
            }
            catch (Exception e) { }
            finally
            {
                menu_approval = null;
            }
            return false;
        }
        //=======================================================================

    }

}
