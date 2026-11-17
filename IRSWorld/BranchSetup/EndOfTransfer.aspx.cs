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
    public partial class EndOfTransfer : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EndOfTransfer));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_eot("INSERT");
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["ENDOFTRANSFER_SEQ_ID"] = field_group;

            return this.do_eot("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string _seq_id)
        {
            Session["ENDOFTRANSFER_SEQ_ID"] = _seq_id;

            return this.do_eot("DELETE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_eot("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_eot("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["ENDOFTRANSFER_MESSAGE"] != null)
            {
                return (string)Session["ENDOFTRANSFER_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["ENDOFTRANSFER_MESSAGE"] = null;
            Session["ENDOFTRANSFER_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_ENDOFTRANSFER_RECORD(string column_values)
        {
            irsEndOfTransfer eot = this.END_OF_TRANSFER();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (eot.set_record(s) == false)
                {
                    this.set_message(eot.messages); return false;
                }

                Session["IRS_ENDOFTRANSFER"] = eot;

                return true;
            }

            eot = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_ENDOFTRANSFER_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_ENDOFTRANSFER_RECORD(string client_seq_id)
        {
            irsEndOfTransfer eot = this.END_OF_TRANSFER();
            
            try 
            {
                if (eot != null)
                {
                    if (eot.get_record(client_seq_id) == true)
                    {
                        return eot.getdataRecord();
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
                Session["IRS_ENDOFTRANSFER"] = eot;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ENDOFTRANSFER_SEQ_ID()
        {
            if (Session["ENDOFTRANSFER_SEQ_ID"] != null)
            {
                return (string)Session["ENDOFTRANSFER_SEQ_ID"];
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
                    sqcommands = commons.getSqlCommand("sp_getdropdowneot");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "EndofTransfer");
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
        public DataSet check_cutoff(string cutoff)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_checkCutOff");
                    sqcommands.Parameters.Add("@CUTOFF", SqlDbType.VarChar, 16).Value = cutoff;
                    return commons.getDataSet(sqcommands, "Check_Cutoff");
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

        #region PRIVATE METHOD: END_OF_TRANSFER

        private irsEndOfTransfer END_OF_TRANSFER()
        {
            if (Session["IRS_ENDOFTRANSFER"] == null)
            {
                Session["IRS_ENDOFTRANSFER"] = new irsEndOfTransfer();
            }

            return (irsEndOfTransfer)Session["IRS_ENDOFTRANSFER"];
        }

        private bool do_eot(string instruction)
        {
            irsEndOfTransfer eot = this.END_OF_TRANSFER();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            eot.selectedRow = -1;
                            if (eot.insert_record() == false)
                            {
                                this.set_message(eot.messages); return false;
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
                            eot.seq_id = this.ENDOFTRANSFER_SEQ_ID();
                            eot.selectedRow = 0;

                            if (eot.update_record() == false)
                            {
                                this.set_message(eot.messages); return false;
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
                            if (this.SET_ENDOFTRANSFER_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (eot.commit_record() == false)
                            {
                                this.set_message(eot.messages); return false;
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
                            eot.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "DELETE")
                    {
                        try
                        {
                            eot.seq_id = this.ENDOFTRANSFER_SEQ_ID();
                            eot.selectedRow = 0;

                            if (eot.delete_record() == false)
                            {
                                this.set_message(eot.messages); return false;
                            }

                            return true;

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

                Session["IRS_ENDOFTRANSFER"] = null;
                Session["IRS_ENDOFTRANSFER"] = eot;
                Session["ENDOFTRANSFER_SEQ_ID"] = null;
                Session["ENDOFTRANSFER_SEQ_ID"] = eot.seq_id;
                eot = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion
    }
}
