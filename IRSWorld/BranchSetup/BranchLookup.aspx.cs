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
using System.Linq;

namespace IRSWorld.BranchSetup
{
    public partial class BranchLookup : System.Web.UI.Page
    {
        private irsBranchLookup IRSBranchLookup
        {
            get
            {
                if (Session["IRS_BRANCH_SUBLOOKUP"] == null)
                {
                    Session["IRS_BRANCH_SUBLOOKUP"] = new irsBranchLookup();
                }
                return (irsBranchLookup)Session["IRS_BRANCH_SUBLOOKUP"];
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BranchLookup));
            this.txtbranchcode.Value = IRSClientData.Location;

            /* RFC-31620 */
            this.txtisadmin.Value = "false";
            if (IRSClientData.isSystemAdmin || IRSClientData.isBranchAdmin)
            {
                this.txtisadmin.Value = "true";
            }
            //this.txtisadmin.Value = (IRSClientData.isSystemAdmin) ? "true" : "false";

            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        private DataSet select_branch_lookup(string branchcode)
        {
            DataSet ds = new DataSet();                   

            try
            {
                ds = IRSBranchLookup.get_branchsublookup(branchcode); 
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
          
            }

        }
        private DataSet select_branch(string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = IRSBranchLookup.get_branches(branchcode);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object get_branch_sub_lookup(string branchcode )
        {
            DataTable dt = new DataTable();                
            try
            {
                dt = this.select_branch_lookup(branchcode).Tables[1];
                return dt;
            }
            catch
            {
                return null;
            }
        }
  
  
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string populate_cbofldgroup()
        {
            HtmlSelect ddl = new HtmlSelect();            
            irsMiscellaneous misc = new irsMiscellaneous();
            DataSet ds = new DataSet();
            try
            {

                ddl.ID = "ddFldGroup";
                ds = this.select_branch_lookup("");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ddl.DataSource = ds.Tables[0];
                    ddl.DataTextField = "fldGroup";
                    ddl.DataValueField = "fldGroup";
                    ddl.DataBind();
                    ddl.Items[0].Selected = true;
                    ddl.Style.Add("width", "28%");
                    ddl.Attributes.Add("onchange", "show_groups()");
                    return misc.render_htmlcontrol(ddl);
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {

                return "ERROR:" + e.Message;

            }
            finally
            {
                
                ds.Dispose();
                ddl.Dispose(); 
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_branchsublookup_data(string branchcode,int isadmin)
        {
            
            DataSet ds = new DataSet();
            try
            {
                ds = IRSBranchLookup.get_branchsublookupbygroup(branchcode, isadmin);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch //(Exception e) // commented to remove warning
            {
                return null;
            }
            finally
            {
                ds.Dispose();
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_branchsublookup_bysubgroup(string branchcode, string subgroupid)
        {

            DataSet ds = new DataSet();
            try
            {
                ds = IRSBranchLookup.get_branchsublookupbysubgroup(branchcode, subgroupid); 
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            catch 
            {
                return null;
            }
            finally
            {
                ds.Dispose();
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_datagrid(string branchcode,string fldgroup)
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSBranchLookup.get_branchsublookup(branchcode);
                ds = IRSBranchLookup.get_branchsublookupbysubgroup(branchcode,fldgroup);
                return ds;
            }
            catch
            {
                return null;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet load_search_datagrid(string branchcode, string fldgroup, string searchval, string searchtype)
        {
            DataSet ds = new DataSet();
            try
            {
                //ds = IRSBranchLookup.get_branchsublookup(branchcode);
                ds = IRSBranchLookup.get_branchsublookupbyfilter(branchcode, fldgroup, searchval, searchtype);
                return ds;
            }
            catch
            {
                return null;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void applyEdits(String adds, String dels, String eds)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };            
            int norec = 0;
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {
                        
                        String[] uu = tt[i].Split('|');
                        if ((uu[0].ToString().Length == 0))
                        {
                            norec++;
                        }
                        else
                        {
                            int row = int.Parse(uu[0].Replace(",", ""));
                            Hashtable ht = new Hashtable();
                            for (int j = 1; j < (uu.Length - norec); j++)
                            {

                                String[] cols = uu[j].Split(sep);
                                if (cols.Length < 2)
                                {
                                    ht.Add(cols[0], "");
                                    continue;
                                }

                                ht.Add(cols[0], cols[1]);
                            }

                            bool ok = false;
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                ok = IRSBranchLookup.applyUpdates(crud, row, ht);
                            }//need to approve
                            else
                            {
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                ok = IRSBranchLookup.applyUpdates_tmp(crud, row, ht, userID);
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


                            
                            if (!ok) errs.Add(" Row: " + row + " - " + IRSBranchLookup.Message);
                        }
                    }
                }
            }
            if (errs.Count > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
            }
            
            this.IRSBranchLookup.saveUpdates();
    

            //return IRSBranchLookup.saveUpdates();
        }
        private void clean_inputs(String inputparams)
        {
        }

    }
}
