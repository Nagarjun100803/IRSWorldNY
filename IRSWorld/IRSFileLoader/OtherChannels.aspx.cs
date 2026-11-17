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

namespace IRSWorld.IRSFileLoader
{
    public partial class OtherChannels : System.Web.UI.Page
    {
        private irsViewOtherChannels vw;// = new irsViewOtherChannels();
        //private irsProcessOtherChannels proc;
        //private irsOtherChannelsDefaults defaults;
        //private irsClientData loginfo;
        private string defaults_records_val;
        // commented to remove warning - private string process_records_val;
        // commented to remove warning - private int record_processed;
        // commented to remove warning - private int record_count;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OtherChannels));
        }

        #region PRIVATE METHOD

        private irsViewOtherChannels VIEW_OTHER_CHANNELS()
        {
            if (Session["VIEW_OTHER_CHANNELS"] == null)
            {
                Session["VIEW_OTHER_CHANNELS"] = new irsViewOtherChannels();
            }

            return (irsViewOtherChannels)Session["VIEW_OTHER_CHANNELS"];
        }

        private irsOtherChannelsProcess PROCESS_OTHER_CHANNELS()
        {
            if (Session["PROCESS_OTHER_CHANNELS"] == null)
            {
                Session["PROCESS_OTHER_CHANNELS"] = new irsOtherChannelsProcess();
            }

            return (irsOtherChannelsProcess)Session["PROCESS_OTHER_CHANNELS"];
        }

        private irsOtherChannelsDefaults OTHERCHANNELS_DEFAULTS()
        {
            if (Session["OTHERCHANNELS_DEFAULTS"] == null)
            {
                Session["OTHERCHANNELS_DEFAULTS"] = new irsOtherChannelsDefaults();
            }

            return (irsOtherChannelsDefaults)Session["OTHERCHANNELS_DEFAULTS"];
        }

        private irsClientData IRS_CLIENT_DATA()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
                return (irsClientData)Session["IRS_CLIENT_DATA"];
            else
            {
                close_session();
                Response.Redirect("ErrorPage.aspx");
                return null;
            }
        }

        private void set_error_defaults(string msg)
        {
            Session["DEFAULTS_MSG"] = null;
            Session["DEFAULTS_MSG"] = msg;
        }

        private void set_error_process(string msg)
        {
            Session["PROCESS_MSG"] = null;
            Session["PROCESS_MSG"] = msg;
        }
        #endregion

        #region DATA

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet grid_otherchannels(int fSEQ_ID, string status)
        {
            Object[] param = new object[] { fSEQ_ID, status };

            vw = this.VIEW_OTHER_CHANNELS();
            return (DataSet)vw.execStoredProcedure("SP_GRIDVIEW_OTHERCHANNELS", param);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getDataSet(String sCmd, bool bStoreProc)
        {
            vw = this.VIEW_OTHER_CHANNELS();
            return (DataSet)vw.getDataSet(sCmd, bStoreProc);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet get_otherchannels_records(string rseq_id)
        {
            vw = this.VIEW_OTHER_CHANNELS();

            SqlCommand cmd = vw.getSqlCommand("SP_GET_OTHERCHANNELS_RECORDS");

            try
            {
                cmd.Parameters.Add("@rSEQ_ID", SqlDbType.Int).Value = rseq_id;
                return (DataSet)vw.getDataSet(cmd);
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet remit_currency_list()
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.ddRemitCurrencyVal();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void close_session()
        {
            Session.Abandon();
        }
        #endregion

        #region PROCESSING OTHERCHANNELS

        #region PUBLIC
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public int count_processed()
        {
            return (int)Session["RECORD_PROCESSED"];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public int count_record()
        {
            return (int)Session["RECORD_COUNT"];
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        //public bool process_otherchannels(int fSEQ_ID)
        //{
        //    irsProcessOtherChannels proc = this.PROCESS_OTHER_CHANNELS();
        //    proc.REMIT_BRANCH = IRS_CLIENT_DATA().Location;
        //    proc.USER_ID = IRS_CLIENT_DATA().UserId;
        //    proc.file_SEQ_ID = fSEQ_ID;

        //    try
        //    {
        //        object[] param = new object[] { fSEQ_ID };
        //        DataTable dt = proc.execStoredProcedureTable("SP_Select_OtherChannels_to_IRS", param);

        //        if (dt != null && dt.Rows.Count > 0)
        //        {
        //            proc.selectedRow = -1;
        //            record_processed = 0;
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                if (Convert.ToBoolean(dr["ValidTransaction"]))
        //                {
        //                    this.record_count++;
        //                    if (proc.insertRecord())
        //                    {
        //                        if (proc.setParamVal(dr))
        //                        {
        //                            if (proc.commitRecord())
        //                            {
        //                                this.record_processed++;
        //                                proc.setRecordProcessed(Convert.ToInt32(dr["SEQ_ID"]));
        //                            }
        //                            else
        //                            {
        //                                this.set_error_process(proc.message);
        //                                return false;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            this.set_error_process(proc.message);
        //                            return false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        this.set_error_process(proc.message);
        //                        return false;
        //                    }
        //                }
        //            }

        //            proc.setFileProcessed(fSEQ_ID);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        this.set_error_process(e.Message);
        //        return false;
        //    }

        //    if (record_processed > 0)
        //    {
        //        proc.setFileProcessed(fSEQ_ID);
        //        Session["RECORD_COUNT"] = (int)this.record_count;
        //        Session["RECORD_PROCESSED"] = (int)this.record_processed;
        //        return true;
        //    }
        //    else
        //    {
        //        this.set_error_process("UNABLE TO PROCESS OTHERCHANNELS.");
        //        return false;
        //    }
        //}
        #endregion

        #region PRIVATE
        #endregion

        #endregion

        #region OTHERCHANNELS DEFAULTS

        #region PUBLIC
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet get_defaults(int FileFormat)
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.getDefaults(FileFormat);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet dd_remit_currency_val()
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.ddRemitCurrencyVal();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet dd_tranx_source_val()
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.ddTranxSourceVal();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet dd_receipt_mode_val()
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.ddReceiptModeVal();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet dd_tranx_purpose_val()
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.ddTranxPurposeVal();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet dd_source_funds_val()
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.ddSourceFundsVal();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet dd_tranx_stat_val()
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return (DataSet)defaults.ddTranxStatVal();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string get_branch()
        {
            return (string)IRS_CLIENT_DATA().Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool defaults_exist(int fileFormat)
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            return defaults.defaultsExist(fileFormat);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool add_defaults()
        {
            return this.do_otherchannels_defaults("INSERT");
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool save_defaults(string val)
        {
            this.defaults_records_val = val;
            return this.do_otherchannels_defaults("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool update_defaults(int SEQ_ID)
        {
            this.set_defaults_SEQ_ID(SEQ_ID);
            return this.do_otherchannels_defaults("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool rollback_defaults()
        {
            return this.do_otherchannels_defaults("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public int get_defaults_SEQ_ID()
        {
            return (int)Session["DEFAULTS_SEQ_ID"];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void set_defaults_SEQ_ID(int val)
        {
            Session["DEFAULTS_SEQ_ID"] = val;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string get_defaults_error()
        {
            if (Session["DEFAULTS_MSG"] != null)
                return (string)Session["DEFAULTS_MSG"];
            else return string.Empty;
        }

        #endregion

        #region PRIVATE
        private bool do_otherchannels_defaults(string instruction)
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    switch (instruction)
                    {
                        case "INSERT":
                            try
                            {
                                defaults.selectedRow = -1;
                                if (defaults.insertRecord()) return true;
                                else
                                {
                                    this.set_error_defaults(defaults.message);
                                    return false;
                                }
                            }
                            catch (Exception e)
                            {
                                this.set_error_defaults(e.Message);
                                return false;
                            }

                        case "UPDATE":
                            try
                            {
                                defaults.SEQ_ID = this.get_defaults_SEQ_ID();
                                defaults.selectedRow = 0;

                                if (defaults.updateRecord())
                                    return true;
                                else
                                {
                                    this.set_error_defaults(defaults.message);
                                    return false;
                                }
                            }
                            catch (Exception e)
                            {
                                this.set_error_defaults(e.Message);
                                return false;
                            }

                        case "COMMIT":
                            try
                            {
                                if (this.parse_defaults_records(this.defaults_records_val))
                                {
                                    if (defaults.commitRecord())
                                        return true;
                                    else
                                    {
                                        this.set_error_defaults(defaults.message);
                                        return false;
                                    }
                                }
                                return false;
                            }
                            catch (Exception e)
                            {
                                this.set_error_defaults(e.Message);
                                return false;
                            }

                        case "ROLLBACK":
                            try
                            {
                                defaults.rollBack();
                                return true;
                            }
                            catch (Exception e)
                            {
                                this.set_error_defaults(e.Message);
                                return false;
                            }

                        default: break;
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["DEFAULTS_SEQ_ID"] = null;
                Session["DEFAULTS_SEQ_ID"] = defaults.SEQ_ID;
                Session["OTHERCHANNELS_DEFAULTS"] = null;
                Session["OTHERCHANNELS_DEFAULTS"] = defaults;
                Session["DEFAULTS_MSG"] = null;
                defaults = null;
            }
            this.set_error_defaults("UNABLE TO PERFORM OPERATION: " + instruction);
            return false;
        }

        private bool parse_defaults_records(string records)
        {
            irsOtherChannelsDefaults defaults = this.OTHERCHANNELS_DEFAULTS();
            string[] s = records.Split('$');

            if (s != null && s.Length > 0)
            {
                if (!defaults.set_record(s))
                {
                    this.set_error_defaults(defaults.message);
                    return false;
                }
                Session["OTHERCHANNELS_DEFAULTS"] = defaults;
                return true;
            }
            defaults = null;
            return true;
        }

        #endregion
        
        #endregion
    }
}