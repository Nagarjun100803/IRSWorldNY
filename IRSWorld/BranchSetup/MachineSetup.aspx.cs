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

namespace IRSWorld.BranchSetup
{
    public partial class MachineSetup : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(MachineSetup));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_machine_setup("INSERT");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["MACHINE_SETUP_SEQ_ID"] = field_group;

            return this.do_machine_setup("UPDATE");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string seq_id)
        {
            return this.update_record(seq_id);
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_machine_setup("COMMIT");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_machine_setup("ROLLBACK");
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["MACHINESETUP_MESSAGE"] != null)
            {
                return (string)Session["MACHINESETUP_MESSAGE"];
            }
            return string.Empty;
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["MACHINESETUP_MESSAGE"] = null;
            Session["MACHINESETUP_MESSAGE"] = msg;
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_MACHINESETUP_RECORD(string column_values)
        {
            irsMachineSetup machinesetup = this.MACHINE_SETUP();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (machinesetup.set_record(s) == false)
                {
                    this.set_message(machinesetup.messages); return false;
                }

                Session["IRS_MACHINE_SETUP"] = machinesetup;

                return true;
            }

            machinesetup = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_MACHINESETUP_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_MACHINESETUP_RECORD(string client_seq_id)
        {
            irsMachineSetup machinesetup = this.MACHINE_SETUP();
            try
            {
                if (machinesetup != null)
                {
                    if (machinesetup.get_record(client_seq_id) == true)
                    {
                        return machinesetup.getdataRecord();
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
                Session["IRS_MACHINE_SETUP"] = machinesetup;
            }
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string MACHINE_SETUP_SEQ_ID()
        {
            if (Session["MACHINE_SETUP_SEQ_ID"] != null)
            {
                return (string)Session["MACHINE_SETUP_SEQ_ID"];
            }

            return "0";
        }

       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_getdropdownmachine");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "MachineSetup");
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
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_getdropdownmachine2");
                    return commons.getDataSet(sqcommands, "MachineSetup");
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

        #region PRIVATE METHOD: MACHINE_SETUP

        private irsMachineSetup MACHINE_SETUP()
        {
            if (Session["IRS_MACHINE_SETUP"] == null)
            {
                Session["IRS_MACHINE_SETUP"] = new irsMachineSetup();
            }

            return (irsMachineSetup)Session["IRS_MACHINE_SETUP"];
        }

        private bool do_machine_setup(string instruction)
        {
            irsMachineSetup ims = this.MACHINE_SETUP();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            ims.selectedRow = -1;
                            if (ims.insert_record() == false)
                            {
                                this.set_message(ims.messages); return false;
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
                            ims.seq_id = this.MACHINE_SETUP_SEQ_ID();
                            ims.selectedRow = 0;

                            if (ims.update_record() == false)
                            {
                                this.set_message(ims.messages); return false;
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
                            if (this.SET_MACHINESETUP_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (ims.commit_record() == false)
                            {
                                this.set_message(ims.messages); return false;
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
                            ims.rollback_record(); return true;
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

                Session["IRS_MACHINE_SETUP"] = null;
                Session["IRS_MACHINE_SETUP"] = ims;
                Session["MACHINE_SETUP_SEQ_ID"] = null;
                Session["MACHINE_SETUP_SEQ_ID"] = ims.seq_id;
                ims = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion
    }
}
