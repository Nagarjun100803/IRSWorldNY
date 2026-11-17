using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using System.Linq;

namespace IRSWorld.BranchSetup
{
    public partial class Holidays : System.Web.UI.Page
    {
        private irsHolidays irsHolidays
        {
            get
            {
                if (Session["HOLIDAYS"] == null)
                {
                    Session["HOLIDAYS"] = new irsHolidays();
                }
                return (irsHolidays)Session["HOLIDAYS"];
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Holidays));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);


        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getrolecode()
        {
            //1 - processor, 2-approver
            string roleid = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
            string role = new irsInternetPage().getLookupValue("User Roles", "FldValue", roleid, "FldDescription");
            if (role.Equals("Parameter Administrator"))
            {
                Session["loginrolecode"] = "1";
            }
            else if (role.Equals("Parameter Administrator Approver"))
            {
                Session["loginrolecode"] = "2";
            }
            else
            {
                Session["loginrolecode"] = "0";
            }

            return Convert.ToString(Session["loginrolecode"]);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getParamForApproval(string brcode)
        {
            bool withpending = new irsInternetPage().getParamForApproval(brcode, "Holidays");
            if (withpending)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_irsUsers()
        {
            //Session["IRS_USERS"] = null; ???
            Session["HOLIDAYS"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getHoliday(string brcode)
        {
            return irsHolidays.GetHolidays(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches(int roleid)
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            string location = "";
            bool sysAdm = clientData.isSystemAdmin;
            bool brAdm = clientData.isBranchAdmin;

            if (sysAdm) { location = ""; }
            else if (brAdm) { location = clientData.Location; }
            return irsHolidays.GetBranches(location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds,String brCode)
        {
            char sep = '^';
            bool deleted;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {
                        String[] uu = tt[i].Split('|');
                        int row = int.Parse(uu[0]);
                        Hashtable ht = new Hashtable();
                        for (int j = 1; j < uu.Length; j++)
                        {
                            String[] cols = uu[j].Split(sep);
                            if (cols.Length < 2) continue;
                            {
                                ht.Add(cols[0], cols[1]);
                            }
                        }
                        if (tt[0] == "delete") { deleted = true; } else { deleted = false; }

                        bool ok = false;
                        DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                        if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                        {
                            ok = irsHolidays.save_holidays(ht, brCode, deleted);
                        }//need to approve
                        else
                        {
                            if(new irsInternetPage().getParamForApproval(brCode, "Holidays"))
                            {
                                throw new Exception("Cannot continue. There are pending for approval under branch " + brCode);
                            }
                            else
                            {

                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                string actionType = tt[0].ToString().ToUpper();
                                ok = irsHolidays.save_holidays_totemp(ht, brCode, deleted, userID, actionType);
                            }

                        }


                        

                        if (!ok)
                        {
                            throw new Exception("Save Changes: " + irsHolidays.Message); 
                        }

                        //if (dels.Length > 0) { bool ok = irsHolidays.delete_holidays(ht); }
                        //else { bool ok = irsHolidays.save_holidays(ht,brCode); }
                    }
                }
            }
            if (errs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
            }

            if (!irsHolidays.saveUpdates())
            {
                throw new Exception("Save changes: " + irsHolidays.Message);
            }

            //email sending to approver
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                string[] sEmail = null;
                DataSet myDt = commons.getDataSet("sp_paramApproval_getApprEmailList", true, new object[] { });
                if (myDt.Tables[0].Rows.Count > 0)
                {
                    sEmail = myDt.Tables[0].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                }
                csMailer imailer = new csMailer();
                imailer.sendMail("[NOTIFICATION] IRSW Parameter Change for Approval", sEmail, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " - Change in IRSW parameter has been submitted for review and approval\n\n");
            }
            catch { }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool DeleteHolidays(int indx,string brcode)
        {
            irsHolidays.delete_holidays(indx,brcode);
            if (!irsHolidays.saveUpdates())
            {
                throw new Exception("Delete: " + irsHolidays.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String StartUpValue()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }

    }
}
