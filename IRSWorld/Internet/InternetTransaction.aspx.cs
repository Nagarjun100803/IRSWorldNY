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

namespace IRSWorld.Internet
{
    public partial class InternetTransaction : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetTransaction));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCommons));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remname(string cifid)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_internet_remitter_infos");
                    sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;

                    return commons.getDataSet(sqcommands, "internettranrem");
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
        public DataSet get_benename(string cifid)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_internet_beneficiary_infos");
                    sqcommands.Parameters.Add("@BENCIFID", SqlDbType.VarChar, 16).Value = cifid;

                    return commons.getDataSet(sqcommands, "internettranbene");
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
        public DataSet get_account(string cifid)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_internet_tran_account");
                    sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;

                    return commons.getDataSet(sqcommands, "internettranaccount");
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
        public DataSet getdropdown_currency(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_RemTranCurrency");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "InternetGFMC");
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
        public DataSet get_conversions(string datenow)
        //public DataSet get_conversions()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_internet_get_conversion");
                    sqcommands.Parameters.Add("@dn", SqlDbType.DateTime).Value = datenow;

                    return commons.getDataSet(sqcommands, "internettranconvert");
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

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_internet_transaction("INSERT");
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_internet_transaction("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["INTERNETTRANSACTION_MESSAGE"] != null)
            {
                return (string)Session["INTERNETTRANSACTION_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["INTERNETTRANSACTION_MESSAGE"] = null;
            Session["INTERNETTRANSACTION_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_INTERNETTRANSACTION_RECORD(string column_values)
        {
            irsInternetTransaction internettransaction = this.INTERNET_TRANSACTION();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (internettransaction.set_record(s) == false)
                {
                    this.set_message(internettransaction.message); return false;
                }

                Session["IRS_INTERNET_TRANSACTION"] = internettransaction;

                return true;
            }

            internettransaction = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_INTERNETTRANSACTION_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_INTERNET_TRANSACTION_RECORD(string client_seq_id)
        {
            irsInternetTransaction internettransaction = this.INTERNET_TRANSACTION();
            try
            {
                if (internettransaction != null)
                {
                    if (internettransaction.get_record(client_seq_id) == true)
                    {
                        return internettransaction.getdataRecord();
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
                Session["IRS_INTERNET_TRANSACTION"] = internettransaction;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string INTERNET_TRANSACTION_SEQ_ID()
        {
            if (Session["INTERNET_TRANSACTION_SEQ_ID"] != null)
            {
                return (string)Session["INTERNET_TRANSACTION_SEQ_ID"];
            }
            return "0";
        }

        #endregion

        #region PRIVATE METHOD: INTERNET_TRANSACTION

        private irsInternetTransaction INTERNET_TRANSACTION()
        {
            if (Session["IRS_INTERNET_TRANSACTION"] == null)
            {
                Session["IRS_INTERNET_TRANSACTION"] = new irsInternetTransaction();
            }
            return (irsInternetTransaction)Session["IRS_INTERNET_TRANSACTION"];
        }

        private bool do_internet_transaction(string instruction)
        {
            irsInternetTransaction cad = this.INTERNET_TRANSACTION();

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
                                this.set_message(cad.message); return false;
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
                            cad.seq_id = this.INTERNET_TRANSACTION_SEQ_ID();
                            cad.selectedRow = 0;

                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.message); return false;
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
                            if (this.SET_INTERNETTRANSACTION_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (cad.commit_record() == false)
                            {
                                this.set_message(cad.message); return false;
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
                Session["IRS_INTERNET_TRANSACTION"] = null;
                Session["IRS_INTERNET_TRANSACTION"] = cad;
                Session["INTERNET_TRANSACTION_SEQ_ID"] = null;
                Session["INTERNET_TRANSACTION_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion
    }
}
