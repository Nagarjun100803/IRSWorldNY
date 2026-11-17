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

namespace IRSWorld.BillsPayment
{
    public partial class OBPSRemitter : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OBPSRemitter));
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_obps_remitter("INSERT");
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_obps_remitter("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["OBPSREMITTER_MESSAGE"] != null)
            {
                return (string)Session["OBPSREMITTER_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["OBPSREMITTER_MESSAGE"] = null;
            Session["OBPSREMITTER_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_OBPSREMITTER_RECORD(string column_values)
        {
            irsInternetRem obpsremitter = this.OBPS_REMITTER();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (obpsremitter.set_record(s) == false)
                {
                    this.set_message(obpsremitter.messages); return false;
                }

                Session["IRS_OBPS_REMITTER"] = obpsremitter;

                return true;
            }

            obpsremitter = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_OBPSREMITTER_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_OBPS_REMITTER_RECORD(string client_seq_id)
        {
            irsInternetRem obpsremitter = this.OBPS_REMITTER();
            try
            {
                if (obpsremitter != null)
                {
                    if (obpsremitter.get_record(client_seq_id) == true)
                    {
                        return obpsremitter.getdataRecord();
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
                Session["IRS_OBPS_REMITTER"] = obpsremitter;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string OBPS_REMITTER_SEQ_ID()
        {
            if (Session["OBPS_REMITTER_SEQ_ID"] != null)
            {
                return (string)Session["OBPS_REMITTER_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_description(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {                                       //sp_getFldDescription
                    sqcommands = commons.getSqlCommand("sp_internet_dropdown");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;
                    return commons.getDataSet(sqcommands, "OBPSRemitter");
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

        #region PRIVATE METHOD: OBPS_REMITTER

        private irsInternetRem OBPS_REMITTER()
        {
            if (Session["IRS_OBPS_REMITTER"] == null)
            {
                Session["IRS_OBPS_REMITTER"] = new irsInternetRem();
            }
            return (irsInternetRem)Session["IRS_OBPS_REMITTER"];
        }

        private bool do_obps_remitter(string instruction)
        {
            irsInternetRem cad = this.OBPS_REMITTER();

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
                            cad.seq_id = this.OBPS_REMITTER_SEQ_ID();
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
                            if (this.SET_OBPSREMITTER_RECORD(this.parameter_values) == false)
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
                }
            }
            catch
            {
            }
            finally
            {
                Session["IRS_OBPS_REMITTER"] = null;
                Session["IRS_OBPS_REMITTER"] = cad;
                Session["OBPS_REMITTER_SEQ_ID"] = null;
                Session["OBPS_REMITTER_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion
    }
}
