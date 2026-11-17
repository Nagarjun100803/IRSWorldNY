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
    public partial class BranchDefaults : System.Web.UI.Page
    {
        private irsBranchDefaults irsBranchDefaults
        {
            get
            {
                if (Session["BranchDefaults"] == null)
                {
                    Session["BranchDefaults"] = new irsBranchDefaults();
                }
                return (irsBranchDefaults)Session["BranchDefaults"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BranchDefaults));
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

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_irsUsers()
        {
            //Session["IRS_USERS"] = null; ??
            Session["BranchDefaults"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBenType()
        {
            return irsBranchDefaults.GetBenType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getNotifyMode()
        {
            return irsBranchDefaults.getNotifyMode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRelationToRemitter()
        {
            return irsBranchDefaults.getRelationToRemitter();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getCountry()
        {
            return irsBranchDefaults.getCountry();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getProductCode()
        {
            return irsBranchDefaults.getProductCode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getPayMode()
        {
            return irsBranchDefaults.getPayMode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRiskRating()
        {
            return irsBranchDefaults.getRiskRating();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GetBranches()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            string location="";
            bool sysAdm = clientData.isSystemAdmin;
            bool brAdm = clientData.isBranchAdmin;
            
            if (sysAdm) { location = ""; }
            else if (brAdm) { location = clientData.Location; }
            return irsBranchDefaults.GetBranches(location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getLocalCurrency(string brcode)
        {
            return irsBranchDefaults.getLocalCurrency(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccountCurrency(string brcode)
        {
            return irsBranchDefaults.getAccountCurrency(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getReceiptMode()
        {
            return irsBranchDefaults.getReceiptMode();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccountType()
        {
            return irsBranchDefaults.getAccountType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getCoveringBank()
        {
            return irsBranchDefaults.getCoveringBank();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRapidRemit()
        {
            return irsBranchDefaults.getRapidRemit();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTransactionSource()
        {
            return irsBranchDefaults.getTransactionSource();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTransactionPurpose()
        {
            return irsBranchDefaults.getTransactionPurpose();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getComputation()
        {
            return irsBranchDefaults.getComputation();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getSourceOfFunds()
        {
            return irsBranchDefaults.getSourceOfFunds();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaultSetup(string brcode)
        {
            return irsBranchDefaults.getDefaultSetup(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRoundUpOBPS()
        {
            return irsBranchDefaults.getRoundUpOBPS();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRemitAggregation1()
        {
            return irsBranchDefaults.getRemitAggregation1();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool saveDefaults(string data, string brcode)
        {
            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            //string location = "";  // commented to remove warning
            bool sysAdm = clientData.isSystemAdmin;
            bool brAdm = clientData.isBranchAdmin;
            bool ok = false;

            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
            {
                ok = irsBranchDefaults.save_defaults(data, brcode);
            }//need to approve
            else
            {
                ok = irsBranchDefaults.saveToTemp_defaults(data, brcode, userID);
            }

            //email sending to approver
            if (ok)
            {
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
            }

            return ok;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchDefaults(string brcode)
        {
            return irsBranchDefaults.getBranchDefaults(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getWorkerType()
        {
            return irsBranchDefaults.getWorkerType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRemitterType()
        {
            return irsBranchDefaults.getRemitterType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getChargeType()
        {
            return irsBranchDefaults.getChargeType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GFCgetChargeType()
        {
            return irsBranchDefaults.getChargeType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getVIPStatus()
        {
            return irsBranchDefaults.getVIPStatus();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getGender()
        {
            return irsBranchDefaults.getGender();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getCivilStatus()
        {
            return irsBranchDefaults.getCivilStatus();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getOccupation()
        {
            return irsBranchDefaults.getOccupation();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getCitizenship()
        {
            return irsBranchDefaults.getCitizenship();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getMail()
        {
            return irsBranchDefaults.getMail();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getContact()
        {
            return irsBranchDefaults.getContact();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getImplementationType()
        {
            return irsBranchDefaults.getImplementationType();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getChargeGroup()
        {
            return irsBranchDefaults.getChargeGroup();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getVIPType()
        {
            return irsBranchDefaults.getVIPType().Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getVIPTable(string branchcode)
        {
            return irsBranchDefaults.getVIPTable(branchcode).Tables[0];
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getStartUpLocation()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds, String brCode)
        {
            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
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
                            ok = irsBranchDefaults.save_vip(ht, brCode, deleted);
                        }//need to approve
                        else
                        {
                            ok = irsBranchDefaults.saveToTemp_vip(ht, brCode, deleted, userID);
                        }

                        if (!ok)
                        {
                            throw new Exception("Save Changes: " + irsBranchDefaults.Message);
                        }
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
        public bool checkSetupVIP(string branchcode, string viptype)
        {
            return irsBranchDefaults.checkSetupVIP(branchcode, viptype);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isAgent(string BranchCode)
        {
            DataTable dt = null;
            sqlHelperWs commons = new sqlHelperWs();

            try
            {
                string s = "SELECT * FROM TBL_BRANCHES WHERE BRANCHCODE='" + BranchCode.Trim() + "' AND BRANCHTYPE=4";

                //dt = commons.getDataSet(commons.getSqlCommand(s, false)).Tables[0];
                dt = commons.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            return false;
        }
    }
}
