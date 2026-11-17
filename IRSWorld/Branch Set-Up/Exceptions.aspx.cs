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

namespace IRSWorld.BranchSetup
{
    [Serializable]
    public partial class Exceptions : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Exceptions));
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_exception("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string seq_id)
        {
            Session["EXCEPTION_SEQ_ID"] = seq_id;
            return this.do_exception("UPDATE");
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
            return this.do_exception("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_exception("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getUserLoc()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return client_data.Location.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getNegBranchCode()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];

            sqlHelperWs commons = new sqlHelperWs();
            Object o = null;
            try
            {
                if (commons != null)
                {
                    o = commons.execScalar("SELECT NegBranchCode FROM TBL_BRANCHES WHERE BranchCode = '" + client_data.Location.ToString() + "'");
                    return o.ToString();
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
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["EXCEPTION_MESSAGE"] != null)
            {
                return (string)Session["EXCEPTION_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["EXCEPTION_MESSAGE"] = null;
            Session["EXCEPTION_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_EXCEPTION_RECORD(string column_values)
        {
            irsException exception = this.EXCEPTION();
            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (exception.set_record(s) == false)
                {
                    this.set_message(exception.messages); return false;
                }
                Session["IRS_EXCEPTION"] = exception;
                return true;
            }
            exception = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_EXCEPTION_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_EXCEPTION_RECORD(string client_seq_id)
        {
            irsException exception = this.EXCEPTION();

            try
            {
                if (exception != null)
                {
                    if (exception.get_record(client_seq_id) == true)
                    {
                        return exception.getdataRecord();
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
                Session["IRS_EXCEPTION"] = exception;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string EXCEPTION_SEQ_ID()
        {
            if (Session["EXCEPTION_SEQ_ID"] != null)
            {
                return (string)Session["EXCEPTION_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remitter(int seq_id)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_getRemitter");
                    sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
                    return commons.getDataSet(sqcommands, "Remitter_SEQID");
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
        public DataSet check_remitter(string cifid)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_checkRemitter");
                    sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
                    return commons.getDataSet(sqcommands, "Remitter_CIFID");
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

        #region PRIVATE METHOD: EXCEPTION
        private irsException EXCEPTION()
        {
            if (Session["IRS_EXCEPTION"] == null)
            {
                Session["IRS_EXCEPTION"] = new irsException();
            }
            return (irsException)Session["IRS_EXCEPTION"];
        }

        private bool do_exception(string instruction)
        {
            irsException exc = this.EXCEPTION();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            exc.selectedRow = -1;
                            if (exc.insert_record() == false)
                            {
                                this.set_message(exc.messages); return false;
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
                            exc.seq_id = this.EXCEPTION_SEQ_ID();
                            exc.selectedRow = 0;

                            if (exc.update_record() == false)
                            {
                                this.set_message(exc.messages); return false;
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
                            if (this.SET_EXCEPTION_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }
                            if (exc.commit_record() == false)
                            {
                                this.set_message(exc.messages); return false;
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
                            exc.rollback_record(); return true;
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
                Session["IRS_EXCEPTION"] = null;
                Session["IRS_EXCEPTION"] = exc;
                Session["EXCEPTION_SEQ_ID"] = null;
                Session["EXCEPTION_SEQ_ID"] = exc.seq_id;

                exc = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion 
    }
}