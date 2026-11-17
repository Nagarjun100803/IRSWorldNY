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
using System.Data.OleDb;
using IRSClassLib;
namespace IRSWorld.GRSMaintenance
{
    public partial class PUNLAMaintenance : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(PUNLAMaintenance));
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_PUNLA_Maint("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["PUNLA_SEQ_ID"] = field_group;

            return this.do_PUNLA_Maint("UPDATE");
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
            return this.do_PUNLA_Maint("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_PUNLA_Maint("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["PUNLA_MESSAGE"] != null)
            {
                return (string)Session["PUNLA_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["PUNLA_MESSAGE"] = null;
            Session["PUNLA_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_PUNLAMaint_RECORD(string column_values)
        {
            irsPUNLAMaint PUNLAMaint = this.PUNLA_Maint();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (PUNLAMaint.set_record(s) == false)
                {
                    this.set_message(PUNLAMaint.messages); return false;
                }

                Session["IRS_PUNLA"] = PUNLAMaint;

                return true;
            }

            PUNLAMaint = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_PUNLAMaint_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_PUNLA_Maint_RECORD(string client_seq_id)
        {
            irsPUNLAMaint PUNLAMaint = this.PUNLA_Maint();
            try
            {
                if (PUNLAMaint != null)
                {
                    if (PUNLAMaint.get_record(client_seq_id) == true)
                    {
                        return PUNLAMaint.getdataRecord();
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
                Session["IRS_PUNLA"] = PUNLAMaint;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string PUNLA_SEQ_ID()
        {
            if (Session["PUNLA_SEQ_ID"] != null)
            {
                return (string)Session["PUNLA_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getPUNLAMaint_filter()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_PUNLAfilter");
                    return commons.getDataSet(sqcommands, "TBL_PUNLAMaint");
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
        /*
                [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
                public DataSet getfld_value(string fld_agency)
                {
                    sqlHelper commons = new sqlHelper();
                    SqlCommand sqcommands = null;

                    try
                    {
                        if (commons != null)
                        {
                            sqcommands = commons.getSqlCommand("sp_getfldvalue");
                            sqcommands.Parameters.Add("@FldGroup", SqlDbType.VarChar, 100).Value = fld_grp;
                            return (commons.getDataSet(sqcommands, "getfld_value1"));
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
        */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet check_PUNLAMaintvalue(string accountno)
        {
            //edit edit edit
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("[sp_checkPUNLA]");
                    sqcommands.Parameters.Add("@v_AccountNo", SqlDbType.NChar, 200).Value = accountno;

                    return commons.getDataSet(sqcommands, "PUNLA_AccountNo");
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


        #endregion


        #region PRIVATE METHOD: PUNLA_Maint

        private irsPUNLAMaint PUNLA_Maint()
        {
            if (Session["IRS_PUNLA"] == null)
            {
                Session["IRS_PUNLA"] = new irsPUNLAMaint();
            }

            return (irsPUNLAMaint)Session["IRS_PUNLA"];
        }

        private bool do_PUNLA_Maint(string instruction)
        {
            irsPUNLAMaint cad = this.PUNLA_Maint();

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
                            cad.seq_id = this.PUNLA_SEQ_ID();
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
                            if (this.SET_PUNLAMaint_RECORD(this.parameter_values) == false)
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

                Session["IRS_PUNLA"] = null;
                Session["IRS_PUNLA"] = cad;
                Session["PUNLA_SEQ_ID"] = null;
                Session["PUNLA_SEQ_ID"] = cad.seq_id;
                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion



    }
}
