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
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.Branch_Set_Up
{
    public partial class Charges : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(Charges));
        }

        #region AJAX PUBLIC METHOD

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_charges("INSERT");
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["CHARGES_SEQ_ID"] = field_group;

            return this.do_charges("UPDATE");
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
            return this.do_charges("COMMIT");
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_charges("ROLLBACK");
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["CHARGES_MESSAGE"] != null)
            {
                return (string)Session["CHARGES_MESSAGE"];
            }
            return string.Empty;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["CHARGES_MESSAGE"] = null;
            Session["CHARGES_MESSAGE"] = msg;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_CHARGES_RECORD(string column_values)
        {
            irsCharges charges = this.CHARGES();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (charges.set_record(s) == false)
                {
                    this.set_message(charges.messages); return false;
                }

                Session["IRS_CHARGES"] = charges;

                return true;
            }

            charges = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_CARDS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_CHARGES_RECORD(string client_seq_id)
        {
            irsCharges charges = this.CHARGES();
            try
            {
                if (charges != null)
                {
                    if (charges.get_record(client_seq_id) == true)
                    {
                        return charges.getdataRecord();
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
                Session["IRS_CHARGES"] = charges;
            }
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.ReadWrite)]
        public string CHARGES_SEQ_ID()
        {
            if (Session["CHARGES_SEQ_ID"] != null)
            {
                return (string)Session["CHARGES_SEQ_ID"];
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
                    sqcommands = commons.getSqlCommand("sp_dropdown_charges");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;
                    return commons.getDataSet(sqcommands, "CHARGES");
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

        #region PRIVATE METHOD: CHARGES

        private irsCharges CHARGES()
        {
            if (Session["IRS_CHARGES"] == null)
            {
                Session["IRS_CHARGES"] = new irsCharges();
            }

            return (irsCharges)Session["IRS_CHARGES"];
        }

        private bool do_charges(string instruction)
        {
            irsCharges cad = this.CHARGES();

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
                            cad.seq_id = this.CHARGES_SEQ_ID();
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
                            if (this.SET_CHARGES_RECORD(this.parameter_values) == false)
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

                Session["IRS_CHARGES"] = null;
                Session["IRS_CHARGES"] = cad;
                Session["CHARGES_SEQ_ID"] = null;
                Session["CHARGES_SEQ_ID"] = cad.seq_id;
                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion

    }
}
