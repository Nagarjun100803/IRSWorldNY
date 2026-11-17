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

namespace IRSWorld.GRSMaintenance
{
    public partial class TranxFileMaintenance : System.Web.UI.Page
    {
        private irsTranxFileMaintenance irsTranxFileMaintenance
        {
            get
            {
                if (Session["FILE_MAIL"] == null)
                {
                    Session["FILE_MAIL"] = new irsTranxFileMaintenance();
                }
                return (irsTranxFileMaintenance)Session["FILE_MAIL"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(TranxFileMaintenance));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBranches()
        {
            //sqlHelperWs commons = new sqlHelperWs();
            //SqlCommand sqcommands = null;

            //try
            //{
            //    sqcommands = commons.getSqlCommand("sp_GetBranches");
            //    //sqcommands.Parameters.Add("@fldgroup", SqlDbType.VarChar, 100).Value = fldgroup;

            //    return commons.getDataSet(sqcommands, "TBLBranches");
            //}
            //catch { }
            //finally
            //{
            //    commons = null;
            //    sqcommands.Dispose();
            //    sqcommands = null;
            //}
            //return null;
            return irsTranxFileMaintenance.getBranches();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void branchSelect(int brSeqId)
        {
            irsTranxFileMaintenance.BranchEmail = brSeqId;
            Session["FILE_MAIL"] = irsTranxFileMaintenance;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFileType(string fldgroup)
        {
            //sqlHelperWs commons = new sqlHelperWs();
            //SqlCommand sqcommands = null;

            //try
            //{
            //    sqcommands = commons.getSqlCommand("sp_get_FldDescription");
            //    sqcommands.Parameters.Add("@fldgroup", SqlDbType.VarChar, 100).Value = fldgroup;

            //    return commons.getDataSet(sqcommands, "tblFileType");
            //}
            //catch { }
            //finally
            //{
            //    commons = null;
            //    sqcommands.Dispose();
            //    sqcommands = null;
            //}
            //return null;
            return irsTranxFileMaintenance.getFileType(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFileFormat(int fldvalue)
        {
            //sqlHelperWs commons = new sqlHelperWs();
            //SqlCommand sqcommands = null;

            //try
            //{
            //    sqcommands = commons.getSqlCommand("sp_getFileFormat");
            //    sqcommands.Parameters.Add("@fldvalue", SqlDbType.Int).Value = fldvalue;
            //    return commons.getDataSet(sqcommands, "tblFileFormat");
            //}
            //catch { }
            //finally
            //{
            //    commons = null;
            //    sqcommands.Dispose();
            //    sqcommands = null;
            //}

            //return null;
            return irsTranxFileMaintenance.getFileFormat(fldvalue);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProductCode(string fldgroup)
        {
            return irsTranxFileMaintenance.getProductCode(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getImplementationType(string fldgroup)
        {
            return irsTranxFileMaintenance.getImplementationType(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable GetEMGridValue(string filetype, string fileformat)
        {
            return irsTranxFileMaintenance.GetEMGridValue(filetype, fileformat);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable GetMSGridValue(string filetype, string fileformat)
        {
            return irsTranxFileMaintenance.GetMSGridValue(filetype, fileformat);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable GetBSGridValue(string filetype, string fileformat)
        {
            return irsTranxFileMaintenance.GetBSGridValue(filetype, fileformat);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEditsEM(String adds, String dels, String eds, String filetype, String fileformat)
        {
            
            char sep = '^';
            bool deleted;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    //if (s.Length > 0)
                    //{
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
                                ok = irsTranxFileMaintenance.save_file_email(ht, filetype, fileformat, deleted);
                            }//need to approve
                            else
                            {
                                string brCode = irsTranxFileMaintenance.getBranch();
                                if (new irsInternetPage().getParamForApproval(brCode, "Email Setup"))
                                {
                                    throw new Exception("Cannot continue. There are pending for approval under branch " + brCode);
                                }

                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                ok = irsTranxFileMaintenance.save_file_email_tmp(ht, filetype, fileformat, deleted, userID);

                                if (ok)
                                {
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
                            }

                            }

                        


                            if (!ok)
                            {
                                throw new Exception("Save Changes: " + irsTranxFileMaintenance.Message);
                            }
                        }
                    //}
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

            if (!irsTranxFileMaintenance.saveUpdates())
            {
                throw new Exception("Save changes: " + irsTranxFileMaintenance.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEditsMS(String adds, String dels, String eds, String filetype, String fileformat)
        {

            char sep = '^';
            bool deleted;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    //if (s.Length > 0)
                    //{
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
                            ok = irsTranxFileMaintenance.save_file_products(ht, filetype, fileformat, deleted);
                        }//need to approve
                        else
                        {
                            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                            ok = irsTranxFileMaintenance.save_file_products_tmp(ht, filetype, fileformat, deleted, userID);
                            if (ok)
                            {
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
                            }

                        }





                        
                        if (!ok)
                        {
                            throw new Exception("Save Changes: " + irsTranxFileMaintenance.Message);
                        }
                    }
                    //}
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

            if (!irsTranxFileMaintenance.saveUpdates())
            {
                throw new Exception("Save changes: " + irsTranxFileMaintenance.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEditsBS(String adds, String dels, String eds, String filetype, String fileformat)
        {

            char sep = '^';
            bool deleted;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    //if (s.Length > 0)
                    //{
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
                            ok = irsTranxFileMaintenance.save_batch_setup(ht, filetype, fileformat, deleted);
                        }//need to approve
                        else
                        {
                            string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                            ok = irsTranxFileMaintenance.save_batch_setup_tmp(ht, filetype, fileformat, deleted, userID);
                            if (ok)
                            {
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
                            }

                        }
                        
                        if (!ok)
                        {
                            throw new Exception("Save Changes: " + irsTranxFileMaintenance.Message);
                        }
                    }
                    //}
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

            if (!irsTranxFileMaintenance.saveUpdates())
            {
                throw new Exception("Save changes: " + irsTranxFileMaintenance.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool DeleteMail(int indx, string filetype, string fileformat)
        {
            irsTranxFileMaintenance.delete_email(indx, filetype, fileformat);
            if (!irsTranxFileMaintenance.saveUpdates())
            {
                throw new Exception("Delete: " + irsTranxFileMaintenance.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool DeleteProd(int indx, string filetype, string fileformat)
        {
            irsTranxFileMaintenance.delete_product(indx, filetype, fileformat);
            if (!irsTranxFileMaintenance.saveUpdates())
            {
                throw new Exception("Delete: " + irsTranxFileMaintenance.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool DeleteBatch(int indx, string filetype, string fileformat)
        {
            irsTranxFileMaintenance.delete_batch(indx, filetype, fileformat);
            if (!irsTranxFileMaintenance.saveUpdates())
            {
                throw new Exception("Delete: " + irsTranxFileMaintenance.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getStartUpLocation()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }
    }
}
