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
    public partial class BranchOperation : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BranchOperation));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet check_seqid(int branch_id)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_check_BRANCH_OPERATION_SEQ_ID");
                    sqcommands.Parameters.Add("@BranchID", SqlDbType.BigInt).Value = branch_id;
                    return commons.getDataSet(sqcommands, "RegionID");
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
        public DataSet verify_seqid(int seq_id)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT * FROM TBL_BRANCH_OPERATION WHERE SEQ_ID = '" + seq_id, false);

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
        public DataSet getdropdown_region()
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
            return this.do_BranchOperation("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["BranchOperation_SEQ_ID"] = field_group;

            return this.do_BranchOperation("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record()
        {
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_BranchOperation("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_BranchOperation("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["BranchOperation_MESSAGE"] != null)
            {
                return (string)Session["BranchOperation_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["BranchOperation_MESSAGE"] = null;
            Session["BranchOperation_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_BranchOperation_RECORD(string column_values)
        {
            irsBranchOperation BranchOperation = this.Branch_Operation();

            string[] s = column_values.Split('^');

            if (s != null && s.Length > 0)
            {
                if (BranchOperation.set_record(s) == false)
                {
                    this.set_message(BranchOperation.messages); return false;
                }

                Session["IRS_BranchOperation"] = BranchOperation;

                return true;
            }

            BranchOperation = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_BranchOperation_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_BranchOperation_RECORD(int client_seq_id)
        {
            irsBranchOperation BranchOperation = this.Branch_Operation();
            try
            {
                if (BranchOperation != null)
                {
                    if (BranchOperation.get_record(client_seq_id.ToString()) == true)
                    {
                        return BranchOperation.getdataRecord();
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
                Session["IRS_BranchOperation"] = BranchOperation;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string BranchOperation_SEQ_ID()
        {
            if (Session["BranchOperation_SEQ_ID"] != null)
            {
                return (string)Session["BranchOperation_SEQ_ID"];
            }

            return "0";
        }


        #endregion

        #region PRIVATE METHOD: BranchOperation

        private irsBranchOperation Branch_Operation()
        {
            if (Session["IRS_BranchOperation"] == null)
            {
                Session["IRS_BranchOperation"] = new irsBranchOperation();
            }

            return (irsBranchOperation)Session["IRS_BranchOperation"];
        }


        private bool do_BranchOperation(string instruction)
        {
            irsBranchOperation cad = this.Branch_Operation();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            Session["paramActionType"] = "ADD";
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
                            Session["paramActionType"] = "EDIT";
                            cad.seq_id = this.BranchOperation_SEQ_ID();
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
                            if (this.SET_BranchOperation_RECORD(this.parameter_values) == false)
                            {

                                return false;
                            }

                            bool ok = false;
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                if (cad.commit_record() == false)
                                {
                                    this.set_message(cad.messages); return false;
                                }

                                return true;
                            }//need to approve
                            else
                            {
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                string actionType = Session["paramActionType"] == "ADD" ? "ADD" : "EDIT";
                                string brCode = this.parameter_values.Split('^')[1].Split('~')[1];
                                if (new irsInternetPage().getParamForApproval(brCode, "Branch Operation"))
                                {
                                    Session["SubBranch_MESSAGE"] = "Cannot continue. There are pending for approval under branch " + brCode;
                                    return false;
                                }

                                if (cad.commit_record_tmp(userID, actionType, this.parameter_values) == false)
                                {
                                    this.set_message(cad.messages); return false;
                                }

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

                Session["IRS_BranchOperation"] = null;
                Session["IRS_BranchOperation"] = cad;
                Session["BranchOperation_SEQ_ID"] = null;
                Session["BranchOperation_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record_sub_branch()
        {
            return this.do_SubBranch("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record_sub_branch(string field_group)
        {
            Session["SubBranch_SEQ_ID"] = field_group;

            return this.do_SubBranch("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record_sub_branch()
        {
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record_sub_branch(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_SubBranch("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record_sub_branch()
        {
            return this.do_SubBranch("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message_sub_branch()
        {
            if (Session["SubBranch_MESSAGE"] != null)
            {
                return (string)Session["SubBranch_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message_sub_branch(string msg)
        {
            Session["SubBranch_MESSAGE"] = null;
            Session["SubBranch_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_SubBranch_RECORD(string column_values)
        {
            irsSubBranch SubBranch = this.Sub_Branch();

            string[] s = column_values.Split('^');

            if (s != null && s.Length > 0)
            {
                if (SubBranch.set_record(s) == false)
                {
                    this.set_message(SubBranch.messages); return false;
                }

                Session["IRS_SubBranch"] = SubBranch;

                return true;
            }

            SubBranch = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_SubBranch_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_SubBranch_RECORD(string client_seq_id)
        {
            irsSubBranch SubBranch = this.Sub_Branch();
            try
            {
                if (SubBranch != null)
                {
                    if (SubBranch.get_record(client_seq_id) == true)
                    {
                        return SubBranch.getdataRecord();
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
                Session["IRS_SubBranch"] = SubBranch;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string SubBranch_SEQ_ID()
        {
            if (Session["SubBranch_SEQ_ID"] != null)
            {
                return (string)Session["SubBranch_SEQ_ID"];
            }

            return "0";
        }


        #endregion

        #region PRIVATE METHOD: SubBranch

        private irsSubBranch Sub_Branch()
        {
            if (Session["IRS_SubBranch"] == null)
            {
                Session["IRS_SubBranch"] = new irsSubBranch();
            }

            return (irsSubBranch)Session["IRS_SubBranch"];
        }


        private bool do_SubBranch(string instruction)
        {
            irsSubBranch cad = this.Sub_Branch();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            Session["paramActionType"] = "ADD";
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
                            Session["paramActionType"] = "EDIT";
                            cad.seq_id = this.SubBranch_SEQ_ID();
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
                            if (this.SET_SubBranch_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            bool ok = false;
                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                if (cad.commit_record() == false)
                                {
                                    this.set_message(cad.messages); return false;
                                }

                                return true;
                            }//need to approve
                            else
                            {
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                string actionType = Session["paramActionType"] == "ADD" ? "ADD" : "EDIT";

                                if (cad.commit_record_tmp(this.parameter_values,userID, actionType) == false)
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

                Session["IRS_SubBranch"] = null;
                Session["IRS_SubBranch"] = cad;
                Session["SubBranch_SEQ_ID"] = null;
                Session["SubBranch_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion
    }
}
