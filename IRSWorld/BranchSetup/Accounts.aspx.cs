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
using System.Text;
using System.Linq;

namespace IRSWorld.BranchSetup
{
    public partial class Accounts : System.Web.UI.Page
    {
        private irsAccounts irsAccounts
        {
            get
            {
                if (Session["Accounts"] == null)
                {
                    Session["Accounts"] = new irsAccounts();
                }
                return (irsAccounts)Session["Accounts"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Accounts));
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
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void drop_irsUsers()
        {
            //Session["IRS_USERS"] = null; why ??
            Session["Accounts"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            string location = "";
            bool sysAdm = clientData.isSystemAdmin;
            bool brAdm = clientData.isBranchAdmin;

            if (sysAdm) { location = ""; }
            else if (brAdm)
            {
                location = clientData.Location;
            }
            return irsAccounts.GetBranches(location);

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getStartUpLocation()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccountCurrency(string brcode)
        {
            return irsAccounts.GetAccountCurrency(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getIBFTAccount(string brcode, short acctcurr)
        {
            return irsAccounts.GetIBFTAccount(brcode, acctcurr);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getNSVSAccount(string brcode, short acctcurr)
        {
            return irsAccounts.GetNSVSAccount(brcode, acctcurr);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds, String brCode, int acctcurrency, string acct_type)
        {
            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            bool deleted;
            bool ok = false;
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

                        if (acct_type == "IBFT")
                        {
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                ok = irsAccounts.SaveIBFTAccount(ht, brCode, acctcurrency, deleted);
                            }//need to approve
                            else
                            {
                                ok = irsAccounts.saveToTempIBFTAccount(ht, brCode, acctcurrency, deleted, userID);
                            }
                        }
                        else if (acct_type == "NSVS")
                        {
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                ok = irsAccounts.SaveNSVSAccount(ht, brCode, acctcurrency, deleted);
                            }//need to approve
                            else
                            {
                                ok = irsAccounts.saveToTempNSVSAccount(ht, brCode, acctcurrency, deleted, userID);
                            }
                        }
                        else if (acct_type == "GL")
                        {
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                ok = irsAccounts.SaveGLAccount(ht, brCode, acctcurrency, deleted);
                            }//need to approve
                            else
                            {
                                ok = irsAccounts.saveToTempGLAccount(ht, brCode, acctcurrency, deleted, userID);
                            }
                        }
                    }
                    if (!ok)
                    {
                        throw new Exception(irsAccounts.Message);
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

            if (!irsAccounts.saveUpdates())
            {
                throw new Exception(irsAccounts.Message);
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
        public Object[] getDdlData()
        {
            String errmsg = "";
            sqlHelperWs sql = new sqlHelperWs();
            DataSet ds = sql.getDataSet(sql.getSqlCommand("select * from tbl_lookup where fldgroup = 'AccountType'",false));
            if (ds == null) throw new Exception("GET DDL DATA: " + errmsg);

            String[] curropts = null;
            if (ds.Tables.Count > 0)
            {
                String[] ss = create_options(ds.Tables[0].Rows, "fldValue", "fldCode");
                curropts = new String[ss.Length + 1];
                curropts[0] = "0~ ";
                int j = 1;
                for (int i = 0; i < ss.Length; i++)
                {
                    curropts[j++] = ss[i];
                }
            }


            return curropts;
        }

        private String[] create_options(DataRowCollection rows, String val_fld, String data_fld)
        {
            ArrayList al = new ArrayList();
            foreach (DataRow row in rows)
            {
                al.Add(row[val_fld].ToString().Trim() + "~" + row[data_fld].ToString().Trim());
            }
            String[] ss = new String[al.Count];
            al.CopyTo(ss);
            return ss;
        }




        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_acct_num(string acct_num)
        {
            return irsAccounts.check_acct_num(acct_num);
        }

        //FOR GL ACCOUNT

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getGLFileType(string fldgroup)
        {
            return irsAccounts.getGLFileType(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getGLCurrency(string fldgroup)
        {
            return irsAccounts.getGLCurrency(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchCode(string branchtype)
        {
            return irsAccounts.getBranchCode(branchtype);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getIbatchFileType(string fldgroup)
        {
            return irsAccounts.getIbatchFileType(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getGLAccount(string brcode, short acctcurr)
        {
            return irsAccounts.getGLAccount(brcode, acctcurr);
        }

    }
}
