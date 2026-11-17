using System;
using System.Data;
using System.Data.SqlClient;
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
    public partial class Branch_Definition : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Branch_Definition));
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


        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_branch_defintion("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["BRANCH_DEFINITION_SEQ_ID"] = field_group;

            return this.do_branch_defintion("UPDATE");
        }

        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_branch_defintion("DELETE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record_old()
        {
            return true;
        }
        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_branch_defintion("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_branch_defintion("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["BRANCHDEFINITION_MESSAGE"] != null)
            {
                return (string)Session["BRANCHDEFINITION_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["BRANCHDEFINITION_MESSAGE"] = null;
            Session["BRANCHDEFINITION_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_BRANCHDEFINITION_RECORD(string column_values)
        {
            irsBranchDefinition branchdefinition = this.BRANCH_DEFINITION();
            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (branchdefinition.set_record(s) == false)
                {
                    this.set_message(branchdefinition.messages); return false;
                }

                Session["IRS_BRANCH_DEFINITION"] = branchdefinition;

                return true;
            }
            branchdefinition = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_BRANCHDEFINTION_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_BRANCH_DEFINITION_RECORD(string seq_id)
        {
            irsBranchDefinition branchdefinition = this.BRANCH_DEFINITION();
            try
            {
                if (branchdefinition != null)
                {
                    if (branchdefinition.get_record(seq_id) == true)
                    {
                        return branchdefinition.getdataRecord();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return null;
            }
            finally
            {
                Session["IRS_BRANCH_DEFINITION"] = branchdefinition;
            }
        }
     
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string BRANCH_DEFINITION_SEQ_ID()
        {
            if (Session["BRANCH_DEFINITION_SEQ_ID"] != null)
            {
                return (string)Session["BRANCH_DEFINITION_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source(string fldgroup)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_branchdef");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "BRANCH_DEFINITION");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source2(string fldgroup)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_branchdef2");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;
                    return commons.getDataSet(sqcommands, "BRANCH_DEFINITION");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source3()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_branchdef3");
                    return commons.getDataSet(sqcommands, "Get_BranchName");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string[] getTimeZone()
        {
            String[] ss = new irsTimeZone().Timezones;
            return ss;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet check_branches(string branchCode, int branchType)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_checkBranchCode");
                    sqcommands.Parameters.Add("@BranchCode", SqlDbType.VarChar, 4).Value = branchCode;
                    sqcommands.Parameters.Add("@BranchType", SqlDbType.SmallInt, 2).Value = branchType;
                    return commons.getDataSet(sqcommands, "Branches_BranchCode");
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_coveringbankcode()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("proc_get_coveringbankcode");
                    sqcommands.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output; 
                    return commons.getDataSet(sqcommands, "COVERINGBANKCODE");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }
        #endregion

        #region PRIVATE METHOD: BRANCH_DEFINITION
        private irsBranchDefinition BRANCH_DEFINITION()
        {
            if (Session["IRS_BRANCH_DEFINITION"] == null)
            {
                Session["IRS_BRANCH_DEFINITION"] = new irsBranchDefinition();
            }
            return (irsBranchDefinition)Session["IRS_BRANCH_DEFINITION"];
        }
       
        private bool do_branch_defintion (string instruction)
        {
            irsBranchDefinition cad = this.BRANCH_DEFINITION();
            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        cad = new irsBranchDefinition();
                        cad.actiontype = "ADD";
                        //try
                        //{
                        //    cad.selectedRow = -1;
                        //    if (cad.insert_record() == false)
                        //    {
                        //        this.set_message(cad.messages); return false;
                        //    }
                        //    return true;
                        //}
                        //catch (Exception e)
                        //{
                        //    this.set_message(e.Message); return false;
                        //}
                        return true;
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            cad.seq_id = this.BRANCH_DEFINITION_SEQ_ID();
                            cad.selectedRow = 0;
                            cad.actiontype = "EDIT";
                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
                    if (instruction == "DELETE")
                    {
                        cad.selectedRow = 0;
                        cad.actiontype = "DELETE";
                        try
                        {
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");


                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                this.set_message("Delete functionality is not allowed when the ParamApproverFlag is not enabled."); return false;

                            }//need to approve
                            else
                            {
                                string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                this.parameter_values = this.parameter_values + "|MAIN_SEQ_ID~" + cad.seq_id + "|ActionType~" + cad.actiontype + "|Maker~" + userID + "|DateAdded~" + dateNow + "|StatusDate~" + dateNow + "|CurrStatus~For Approval" + "|Module~Branch Administration >> Branch Code Table" + "|MenuName~Branch Code Table";
                                if (cad.saveToTemp(this.parameter_values.Split('|')) == false)
                                {
                                    this.set_message(cad.messages); return false;
                                }
                                cad = null;
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
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");

                            if(dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                if (this.SET_BRANCHDEFINITION_RECORD(this.parameter_values) == false)
                                {
                                    return false;
                                }

                                if (cad.commit_record() == false)
                                {
                                    this.set_message(cad.messages); return false;
                                }
                            }//need to approve
                            else
                            {
                                string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                this.parameter_values = this.parameter_values + "|MAIN_SEQ_ID~" + cad.seq_id + "|ActionType~" + cad.actiontype + "|Maker~" + userID + "|DateAdded~" + dateNow + "|StatusDate~" + dateNow + "|CurrStatus~For Approval" + "|Module~Branch Administration >> Branch Code Table" + "|MenuName~Branch Code Table";
                                if (cad.saveToTemp(this.parameter_values.Split('|')) == false)
                                {
                                    this.set_message(cad.messages); return false;
                                }
                                cad = null;
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
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            cad.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch {}
            finally
            {
                Session["IRS_BRANCH_DEFINITION"] = null;
                Session["IRS_BRANCH_DEFINITION"] = cad;
                Session["BRANCH_DEFINITION_SEQ_ID"] = null;
                //Session["BRANCH_DEFINITION_SEQ_ID"] = cad.seq_id;
                cad = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAsterisk(string branchType)
        {
            DataTable dtLookup = new irsInternetPage().getLookup("BranchCodeSetup");
            DataTable dtDynamicValidation = new DataTable();
            DataTable dt = new DataTable();
            DataRow dr;
            int i = 0;
            string asterisk = string.Empty;
            bool found = false;
            dt.Columns.Add("Asterisk");

            if (dtLookup != null && dtLookup.Rows.Count > 0)
            {
                for (i = 0; i <= (dtLookup.Rows.Count - 1); i++)
                {
                    if (branchType == dtLookup.Rows[i]["FldCode"].ToString())
                    {
                        found = true;
                        dr = dt.NewRow();
                        dr[0] = dtLookup.Rows[i]["FldDescription"].ToString();
                        dt.Rows.Add(dr);
                    }
                }
            }

            if (!found)
            {
                dtDynamicValidation = new irsBranchDefinition().getBranchCodeSetupMandatory("");
                if (dtDynamicValidation != null && dtDynamicValidation.Rows.Count > 0)
                {
                    for (i = 0; i <= (dtDynamicValidation.Rows.Count - 1); i++)
                    {
                        found = true;
                        asterisk += dtDynamicValidation.Rows[i]["AsteriskField"].ToString() + "~";
                    }
                    if (found)
                    {
                        asterisk = asterisk.Substring(0, asterisk.Length - 1); //Remove ending ~
                        dr = dt.NewRow();
                        dr[0] = asterisk;
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }
    }
}