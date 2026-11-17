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
using IRSWorld.TranClass;
using System.Linq;

namespace IRSWorld.BranchSetup
{
    public partial class Declaration : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Declaration));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet verify_seqid(int seq_id)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT * FROM TBL_DECLARATION WHERE SEQ_ID = '" + seq_id, false);

                    return commons.getDataSet(sqcommands, "SEQ_ID");
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
        public DataSet get_declaration_bybranchcode(string neg_branchcode, string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT TOP 1 * FROM TBL_DECLARATION WHERE BranchCode= '" + branchcode + "'" + " AND NegBranchCode = '" + neg_branchcode + "'", false);
                    //sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "GetBranches");
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
        public DataSet get_declaration_bynegbranchcode(string neg_branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT TOP 1 * FROM TBL_DECLARATION WHERE ISNULL(BranchCode,"+ "'" + "'"+ ")= " + "'" +"'" + " AND NegBranchCode = '" + neg_branchcode + "'", false);
                    //sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "GetBranches");
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
        public DataSet getdropdown_negbranchcode()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT DISTINCT NegBranchCode FROM TBL_BRANCHES WHERE NegBranchCode IS NOT NULL", false);
                    //sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "GetBranches");
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
        public DataSet getdropdown_branch2(string neg_branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT BranchCode, BranchCode + '-' + BranchName as BranchName FROM TBL_BRANCHES WHERE NegBranchCode = '" + neg_branchcode + "'", false);
                    //sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "GetBranches");
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
        public DataSet getdropdown_branch()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_GetBranches");
                    //sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "GetBranches");
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

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            Session["paramActionType"] = "ADD"; //Declaration only has EDIT button
            return this.do_Declaration("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["Declaration_SEQ_ID"] = field_group;
            Session["paramActionType"] = "EDIT";
            return this.do_Declaration("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record()
        {
            Session["paramActionType"] = "DELETE";
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_Declaration("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_Declaration("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["Declaration_MESSAGE"] != null)
            {
                return (string)Session["Declaration_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["Declaration_MESSAGE"] = null;
            Session["Declaration_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_Declaration_RECORD(string column_values)
        {
            irsDeclaration Declaration = this.Receipt_Declaration();

            string[] s = column_values.Split('^');

            if (s != null && s.Length > 0)
            {
                if (Declaration.set_record(s) == false)
                {
                    this.set_message(Declaration.messages); return false;
                }

                Session["IRS_Declaration"] = Declaration;

                return true;
            }

            Declaration = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_Declaration_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_Declaration_RECORD(int client_seq_id)
        {
            irsDeclaration Declaration = this.Receipt_Declaration();
            try
            {
                if (Declaration != null)
                {
                    if (Declaration.get_record(client_seq_id.ToString()) == true)
                    {
                        return Declaration.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_Declaration"] = Declaration;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void getUpdateDeclaration(int seq_id)
        {
            irsDeclaration cad = this.Receipt_Declaration();
            cad.getUpdateDS(seq_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Declaration_SEQ_ID()
        {
            if (Session["Declaration_SEQ_ID"] != null)
            {
                return (string)Session["Declaration_SEQ_ID"];
            }

            return "0";
        }


        #endregion

        #region PRIVATE METHOD: Declaration

        private irsDeclaration Receipt_Declaration()
        {
            if (Session["IRS_Declaration"] == null)
            {
                Session["IRS_Declaration"] = new irsDeclaration();
            }

            return (irsDeclaration)Session["IRS_Declaration"];
        }


        private bool do_Declaration(string instruction)
        {
            irsDeclaration cad = this.Receipt_Declaration();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cad.selectedRow = -1;
                            if (cad.insert_record() == false)
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

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            cad.seq_id = this.Declaration_SEQ_ID();
                            cad.selectedRow = 0;

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

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            ////Comment out to centralized checking of ParamApproverFlag
                            ////if (this.SET_Declaration_RECORD(this.parameter_values) == false)
                            ////{
                            ////    return false;
                            ////}
						    
                            ////DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            ////if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldCode"].ToString() == "0")
                            ////{
                            ////    if (cad.commit_record() == false)
                            ////    {
                            ////        this.set_message(cad.messages); return false;
                            ////    }

                            ////    return true;
                            ////}//need to approve

                            if (this.isParamApproval_declaration() == false)
                            {
                                if (this.SET_Declaration_RECORD(this.parameter_values) == false)
                                {
                                    return false;
                                }

                                if (cad.commit_record() == false)
                                {
                                    this.set_message(cad.messages); return false;
                                }
                                return true;
                            }
                            else
                            {
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                string actionType = "EDIT";//Session["paramActionType"].ToString();// tt[0].ToString().ToUpper();
                                if (cad.commit_record_tmp(userID, actionType, this.parameter_values) == false)
                                {
                                    this.set_message(cad.messages); return false;
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
            catch
            {
            }
            finally
            {

                Session["IRS_Declaration"] = null;
                Session["IRS_Declaration"] = cad;
                Session["Declaration_SEQ_ID"] = null;
                Session["Declaration_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isParamApproval_declaration()
        {
            try
            {
                DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                if (dtLookup.Rows.Count > 0 && dtLookup.Rows[0]["FldCode"].ToString() == "1")
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                this.set_message(e.Message);
            }
            return false;
        }

        #endregion
    }
}
