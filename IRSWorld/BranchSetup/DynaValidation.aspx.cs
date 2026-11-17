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
    public partial class DynaValidation : System.Web.UI.Page
    {
        private irsDynaValidation irsDynaValidation
        {
            get
            { 
                if (Session["IRS_DYNA_VALIDATION"] == null)
                {
                    Session["IRS_DYNA_VALIDATION"] = new irsDynaValidation();
                }
                return (irsDynaValidation)Session["IRS_DYNA_VALIDATION"];
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
            AjaxPro.Utility.RegisterTypeForAjax(typeof(DynaValidation));
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
        public Object get_initial_data(String brcode)
        {
            String xbrcode = brcode;
            if (brcode == null || brcode.Length == 0)
            {
                xbrcode = (IRSClientData == null) ? null : IRSClientData.Location;
            }

            DataTable dt = irsDynaValidation.get_initial_data(xbrcode); //get_mandatory_fields(brcode, "-1", String.Empty);
            if (dt == null)
            {
                throw new Exception("INIT DATA: " + irsDynaValidation.Message);
            }

            return dt;
        }
     
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_ddlClass()
        {
            DataSet ds = irsDynaValidation.get_ddl_class();
            if (ds == null)
            {
                throw new Exception ("DDL Class: " + irsDynaValidation.Message);
            }
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string populate_cbobranch()
        {
            HtmlSelect ddl = new HtmlSelect();
            DataSet ds = new DataSet();
            irsMiscellaneous misc = new irsMiscellaneous();
            try
            {

                ddl.ID = "ddBranchCode";
                string branchcode = (!IRSClientData.isSystemAdmin) ? IRSClientData.Location : "";
                ds = this.select_branch(branchcode);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                   
                        ddl.DataSource = ds.Tables[0];
                        ddl.DataTextField = "BranchName";
                        ddl.DataValueField = "BranchCode";
                        ddl.DataBind();
                        setSelectedDDL(ddl);
                        ddl.Attributes.Add("onchange", "dyna_validation.load_all_dropdownlist2()");
                }
                return misc.render_htmlcontrol(ddl);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                ddl.Dispose();
                ds.Dispose();
                misc = null;
            }
        }

        private void setSelectedDDL(HtmlSelect ddl)
        {
            for (int i = 0; i < ddl.Items.Count; i++)
            {
                if (ddl.Items[i].Value.ToString().Contains(IRSClientData.Location))
                {
                    ddl.Items[i].Selected = true;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet select_branch(string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                irsDynaValidation irsdyna = new irsDynaValidation();
                ds = irsdyna.get_branches(branchcode);
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
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_ddlMenu(string cifclass)
        {
            DataSet ds = irsDynaValidation.get_ddl_menu(cifclass);
            if (ds == null)
            {
                throw new Exception ("DDL Class: " + irsDynaValidation.Message);
            }
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_datagrid(string branchcode,string classtype,string cifmenu)
        {

            if (branchcode.Trim().Length == 0)
                branchcode = IRSClientData.Location;
            //if (!IRSClientData.isSystemAdmin)
            //    branchcode = IRSClientData.Location;

            if (cifmenu.ToString().ToLower().IndexOf("null") > -1)
                cifmenu = "";
            DataSet ds = irsDynaValidation.load_dynaval_info(branchcode, classtype, cifmenu);
            if (ds == null)
            {
                throw new Exception("DDL Class: " + irsDynaValidation.Message);
            }
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object applyEdits(String adds, String dels, String eds,String ddc)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            string branchcode = "";
            string cifclass = "";
            string cifmenu = "";
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
                            branchcode = ht["BranchCode"].ToString();
                            if (ddc == "null")
                            {
                                cifclass = "-1";
                                cifmenu = "";
                            }
                            else
                            {
                                cifclass = ht["CifClass"].ToString();
                                cifmenu = ht["CifMenu"].ToString();
                            }
                            
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            bool ok = false;
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                ok = irsDynaValidation.applyUpdates(crud, row, ht);
                            }//need to approve
                            else
                            {
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                ok = irsDynaValidation.saveToTemp(crud, row, ht, userID);
                            }

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



                            if (!ok) errs.Add(" Row: " + row + " - " + irsDynaValidation.Message);
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
            /*
            if (!irsDynaValidation.saveUpdates())
            {
                throw new Exception("Save changes: " + irsDynaValidation.Message);
            }*/


            return irsDynaValidation.saveUpdates(branchcode, cifclass, cifmenu);
        }
    }


}

