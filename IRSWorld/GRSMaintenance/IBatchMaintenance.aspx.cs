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

namespace IRSWorld.GRSMaintenance
{
    public partial class IBatchMaintenance : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(IBatchMaintenance));
        }
        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_batchmaint("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string _seq_id)
        {
            Session["BATCHMAINT_SEQ_ID"] = _seq_id;

            return this.do_batchmaint("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string _seq_id)
        {
            Session["BATCHMAINT_SEQ_ID"] = _seq_id;

            return this.do_batchmaint("DELETE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_batchmaint("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["BATCHMAINT_MESSAGE"] != null)
            {
                return (string)Session["BATCHMAINT_MESSAGE"];
            }
            return string.Empty;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["BATCHMAINT_MESSAGE"] = null;
            Session["BATCHMAINT_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_BATCHMAINT_RECORD(string column_values)
        {
            irsBatchMaint  batchmaint = this.BATCHMAINT();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (batchmaint.set_record(s) == false)
                {
                    this.set_message(batchmaint.messages); return false;
                }

                Session["IRS_BATCHMAINT"] = batchmaint;

                return true;
            }

            batchmaint = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_BATCHMAINT_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_BATCHMAINT_RECORD(string client_seq_id)
        {
            irsBatchMaint  batchmaint = this.BATCHMAINT();
            try
            {
                if (batchmaint != null)
                {
                    if (batchmaint.get_record(client_seq_id) == true)
                    {
                        return batchmaint.getdataRecord();
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
                Session["IRS_BATHCMAINT"] = batchmaint;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string BATCHMAINT_SEQ_ID()
        {
            if (Session["BATCHMAINT_SEQ_ID"] != null)
            {
                return (string)Session["BATCHMAINT_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getbatchmaint_filter()
        {
            // look up in filter ddl
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_ibatch_filemapper_filter");
                    return commons.getDataSet(sqcommands, "CIF_BATCHMAINT");
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
        // getfld_value commented

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet check_batchmaintvalue(string targetfield, string sourcefield)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("[sp_check_filemapper_rem]");
                    sqcommands.Parameters.Add("@v_targetfield", SqlDbType.VarChar, 50).Value = targetfield;
                    sqcommands.Parameters.Add("@v_sourcefield", SqlDbType.VarChar, 50).Value = sourcefield;
                    return commons.getDataSet(sqcommands, "TBL_FILEMAPPER_REM");
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

        #region PRIVATE METHOD: LOOK_UP

        private irsBatchMaint BATCHMAINT()
        {
            if (Session["IRS_BATCHMAINT"] == null)
            {
                Session["IRS_BATCHMAINT"] = new irsBatchMaint();
            }

            return (irsBatchMaint)Session["IRS_BATCHMAINT"];            
        }

        private bool do_batchmaint(string instruction)
        {
            irsBatchMaint cad = this.BATCHMAINT();

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
                            cad.seq_id = this.BATCHMAINT_SEQ_ID();
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

                    if (instruction == "DELETE")
                    {
                        try
                        {
                            cad.seq_id = this.BATCHMAINT_SEQ_ID();
                            cad.selectedRow = 0;
                            
                            //if (cad.delete_record() == false)
                            //{
                            //    this.set_message(cad.messages); return false;
                            //}

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
                            if (this.SET_BATCHMAINT_RECORD(this.parameter_values) == false)
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

                Session["IRS_BATCHMAINT"] = null;
                Session["IRS_BATCHMAINT"] = cad;
                Session["LOOK_UP_BATCHMAINT"] = null;
                Session["LOOK_UP_BATCHMAINT"] = cad.seq_id;
                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion

    }
}
