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

namespace IRSWorld.Branch_Set_Up
{
    public partial class Branch_Definition : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(Branch_Definition));
        }

        
        #region AJAX PUBLIC METHOD

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_branch_defintion("INSERT");
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["BRANCH_DEFINITION_SEQ_ID"] = field_group;

            return this.do_branch_defintion("UPDATE");
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record()
        {
            return true;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_branch_defintion("COMMIT");
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_branch_defintion("ROLLBACK");
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["BRANCHDEFINITION_MESSAGE"] != null)
            {
                return (string)Session["BRANCHDEFINITION_MESSAGE"];
            }
            return string.Empty;
            
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["BRANCHDEFINITION_MESSAGE"] = null;
            Session["BRANCHDEFINITION_MESSAGE"] = msg;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
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

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_BRANCH_DEFINITION_RECORD(string client_seq_id)
        {
            irsBranchDefinition branchdefinition = this.BRANCH_DEFINITION();
            try
            {
                if (branchdefinition != null)
                {
                    if (branchdefinition.get_record(client_seq_id) == true)
                    {
                        return branchdefinition.getdataRecord();
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
                Session["IRS_BRANCH_DEFINITION"] = branchdefinition;
            }
        }
     
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string BRANCH_DEFINITION_SEQ_ID()
        {
            if (Session["BRANCH_DEFINITION_SEQ_ID"] != null)
            {
                return (string)Session["BRANCH_DEFINITION_SEQ_ID"];
            }

            return "0";
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
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
                            cad.seq_id = this.BRANCH_DEFINITION_SEQ_ID();
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
                            if (this.SET_BRANCHDEFINITION_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (cad.commit_record() == false)
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
        
                Session["IRS_BRANCH_DEFINITION"] = null;
                Session["IRS_BRANCH_DEFINITION"] = cad;
                Session["BRANCH_DEFINITION_SEQ_ID"] = null;
                Session["BRANCH_DEFINITION_SEQ_ID"] = cad.seq_id;
             
                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion

 
    }


}
