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
using IRSWorld.Remittance;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace IRSWorld
{
    public partial class RemOvrApproval : System.Web.UI.Page
    {
        private sqlHelperWs sqlkonek = new sqlHelperWs();
        private csAuditLog logger = new csAuditLog();
        private string remitter_cifid = string.Empty;
        private string beneficiary_cifid = string.Empty;
        private string override_type = string.Empty;
        private string enrollment_rem_ovr = string.Empty;
        private string enrollment_ben_ovr = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemOvrApproval));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Refinitiv)); /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_enroll_hit(string branch_code, string date_from, string date_to)
        {
            string return_string = "otc_remote_ovr_enrollhit~";

            try
            {
                if (string.IsNullOrEmpty(date_from).Equals(true))
                {
                    date_from = "";
                }
                else
                {
                    date_from = DateTime.Parse(date_from).ToString("MM/dd/yyyy");
                }

                if (string.IsNullOrEmpty(date_from).Equals(true))
                {
                    date_to = "";
                }
                else
                {
                    date_to = DateTime.Parse(date_to).ToString("MM/dd/yyyy");
                }

                if (string.IsNullOrEmpty(branch_code).Equals(true) || branch_code.ToUpper() == "NULL")
                {
                    branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                }

                return return_string + branch_code + "~" + date_from + "~" + date_to;
            }
            catch
            {
                return "otc_remote_ovr_enrollhit~0";
            }
        }
        /* FOR VALIDATE OVERRIDE */ // RFC - 24076 - to validate the selected pending override / CJ / 05/04/2022
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet validate_Pending_override(int transaction_id, string Module, int NatureOfOverride)
        {
            DataSet ds = null;
            ds = sqlkonek.getDataSet("val_pen_ovr", true, new object[] { transaction_id, Module, NatureOfOverride });
            if (ds.Tables.Count > 0)
            {
                return ds;
            }
            return ds;
        }
        // CJ FIX VALIDATE PENDING OVERRIDE 07/21/2022
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getuserid()
        {
            string userid = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            return userid;
        }
        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public int getactivevalidation()
        //{
        //    int validationtoggle = 0;
        //    irsSqlCommonsWs common = new irsSqlCommonsWs();
        //    DataSet ds = common.getDataSet(common.getSqlCommand("SELECT FldValue FROM tbl_lookup WHERE FldGroup = 'PendingOverrideToggle' and FldDescription = 'OnOff'", false));
        //    if (ds.Tables.Count > 0)
        //    {
        //        validationtoggle = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
        //        return validationtoggle;
        //    }
        //    return 0;
        //}
        // CJ Checking of toggle for Validation in Pending Override
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int getactivevalidation()
        {
            try
            {
                int validationtoggle = 0;
                irsSqlCommonsWs common = new irsSqlCommonsWs();
                DataSet ds = common.getDataSet(common.getSqlCommand("get_pen_ovr_val", true));
                if (ds.Tables.Count > 0)
                {
                    validationtoggle = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                    return validationtoggle;
                }
                return 0;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "OVERRIDE", "PENDING OVERRIDE ERROR", ex.Message + "~" + ex.StackTrace, "", csAuditLog.Category.Select);
                return 0;
            }
        }
        // CJ IR in Pending Override 07252022 validation for approve
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validateApproveCIF(int seq_id) //true = approved
        {
            try
            {
                string validationres = "";
                DataSet ds = null;
                ds = sqlkonek.getDataSet("val_app_cif", true, new object[] { seq_id });
                if (ds.Tables.Count > 0)
                {
                    validationres = ds.Tables[0].Rows[0][0].ToString();
                    if (validationres != "0")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "OVERRIDE", "PENDING OVERRIDE ERROR", ex.Message + "~" + ex.StackTrace, seq_id.ToString(), csAuditLog.Category.Select);
                return false;
            }
        }
        // CJ IR in Pending Override 07252022 validation for approve
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validateRejectCIF(int seq_id) //true = approved
        {
            try
            {
                string validationres = "";
                DataSet ds = null;
                ds = sqlkonek.getDataSet("val_rej_cif", true, new object[] { seq_id });
                if (ds.Tables.Count > 0)
                {
                    validationres = ds.Tables[0].Rows[0][0].ToString();
                    if (validationres != "0")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "OVERRIDE", "PENDING OVERRIDE ERROR", ex.Message + "~" + ex.StackTrace, seq_id.ToString(), csAuditLog.Category.Select);
                return false;
            }
        }
        /* FOR VALIDATE OVERRIDE*/
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_selected_client_hit(string client_id)
        {
            Remittance.RemOTC otc = null;
            
            string return_alternate = "sp_otc_hit_listing~0~0";
            string return_source = string.Empty;

            try
            {
                otc = new Remittance.RemOTC(); return_source = otc.check_cif_enrollment_pending_ovr(client_id, client_id);

                if (string.IsNullOrEmpty(return_source).Equals(false))
                {
                    return_source = otc.get_client_hitlist_record();

                    if (string.IsNullOrEmpty(return_source).Equals(false))
                    {
                        return return_source;
                    }
                }

                return return_alternate;
            }
            catch
            {
                return return_alternate;
            }
            finally
            {
                if (otc != null)
                {
                    otc.Dispose();
                    otc = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_client_hitlist_query()
        {
            string return_string = "sp_otc_hit_listing~0~0";

            Remittance.RemOTC otc = null;
            RemOvrApproval ovr = null;

            try
            {
                if (System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"] != null)
                {
                    ovr = (RemOvrApproval)System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"];
                }

                if (ovr != null)
                {
                    otc = new Remittance.RemOTC();

                    if (ovr.override_type == ovr.enrollment_rem_ovr)
                    {
                        otc.check_cif_enrollment_pending_ovr(ovr.remitter_cifid, string.Empty); return_string = otc.get_client_hitlist_record();
                    }
                    else if (ovr.override_type == ovr.enrollment_ben_ovr)
                    {
                        otc.check_cif_enrollment_pending_ovr(string.Empty, ovr.beneficiary_cifid); return_string = otc.get_client_hitlist_record();
                    }
                    else
                    {
                        return_string = "sp_otc_hit_listing~" + ovr.remitter_cifid + "~" + ovr.beneficiary_cifid;
                    }
                }

                if (string.IsNullOrEmpty(return_string).Equals(true))
                {
                    return "sp_otc_hit_listing~0~0";
                }

                return return_string;
            }
            catch
            {
                return return_string;
            }
            finally
            {
                if (ovr != null)
                {
                    ovr.Dispose();
                    ovr = null;
                }

                if (otc != null)
                {
                    otc.Dispose();
                    otc = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_pending_details(int transaction_seq_id, int override_nature)
        {
            DataTable dt = null;
            DataSet ds = null;

            try
            {
                System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"] = null;

                ds = sqlkonek.getDataSet("otc_remote_ovr_details", true, new object[] { transaction_seq_id, override_nature });

                if (ds != null)
                {
                    dt = ds.Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        this.remitter_cifid = dt.Rows[0]["rem_cifid_id"].ToString();
                        this.beneficiary_cifid = dt.Rows[0]["ben_cifid_id"].ToString();
                        this.override_type = dt.Rows[0]["override_type"].ToString();
                        this.enrollment_rem_ovr = dt.Rows[0]["enrollment_rem_ovr"].ToString();
                        this.enrollment_ben_ovr = dt.Rows[0]["enrollment_ben_ovr"].ToString();
                    }
                }

                System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"] = this;

                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_refinitiv_overridetype(int override_type)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            string user_role = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Role.ToString();

            try
            {
                ds_return = sqlkonek.execStoredProcedure("getRefinitivHit_NatureOfOverride", new object[] { override_type });
                if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
                {
                    return "1";
                }
                else { return "0"; }
            }
            catch
            {
                return "0";
            }
            finally
            {
                if (ds_return != null)
                {
                    ds_return.Dispose();
                    ds_return = null;
                }
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_override_branches()
        {
            string user_name = null;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                user_name = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            }

            return sqlkonek.getDataSet("otc_remote_override_branchlist", true, new object[] { user_name });
        }

        //=========================================================================================
        /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_reason_discounting()
        {
            DataSet ds = new DataSet();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                ds = commons.execStoredProcedure("proc_get_lookup_details", new object[] { "ReasonForDiscounting", "1", 0 });
            }
            catch
            {
                return null;
            }
            finally
            {
                commons = null;
            }
            return ds;
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_reason_discounting_bus()
        {
            DataSet ds = new DataSet();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                ds = commons.execStoredProcedure("proc_get_lookup_details", new object[] { "ReasonForDiscounting", "2", 0 });
            }
            catch
            {
                return null;
            }
            finally
            {
                commons = null;
            }
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_approver_disposition()
        {
            DataSet ds = new DataSet();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                ds = commons.execStoredProcedure("proc_get_lookup_details", new object[] { "ReasonForDisposition", "", 0 });
            }
            catch
            {
                return null;
            }
            finally
            {
                commons = null;
            }
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_discounting_details(int tranxid, string cifid)
        {
            DataTable dt = new DataTable();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                dt = commons.execStoredProcedureTable("proc_get_discounting_details", new object[] { tranxid, cifid });
            }
            catch
            {
                return null;
            }
            finally
            {
                commons = null;
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_watchlist_record_details(string recordno)
        {
            DataSet ds = new DataSet();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                ds = commons.execStoredProcedure("proc_get_unifiedwatchlist_details", new object[] { recordno });

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                commons = null;
            }
            return null;
        }
        //=========================================================================================

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string rem_ovr_approved(int transaction_id, int ovr_type, string reason_for_discounting, string ddl_reason_discounting, string ddl_reason_discounting_bus, string ddl_approver_disposition, string approver_remarks, int cifclass)
        {
            RemOvrApproval ovr = null;
            irsClientData irs_client_data = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
            string userid = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            ////=========================================================================================
            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
            //string reason_disc_val = this.transaction_reason_for_discounting(transaction_id, ovr_type, reason_for_discounting, ddl_reason_discounting);
            string reason_disc_val = this.transaction_reason_for_discounting(transaction_id, ovr_type, reason_for_discounting, ddl_reason_discounting, ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks, cifclass);
            ////=========================================================================================

            /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
            Refinitiv refinitivcs = new Refinitiv();
            string isRefinitivEnabled = "0";
            DataSet ds_natureofoverride = null;
            DataSet tnx_details = null;
            //string reason_for_discounting = "FALSE-POSITIVE";
            string RemitterHit = "false";
            string BeneficiaryHit = "false";
            /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */


            /* 
            - otc_remote_ovr_comrec
                1. Update tbl_remittance_override_logs 
                2. execute usp_auto_untag_hitlist (tbl_cifhitlist and tbl_cifhitlist_percentage)
            - scnd_otc_remote_ovr_aproved_qry = Count tbl_remittance_override_logs
                1. Overridetype  
                2. UserID is not null
                3. TranxStatus = Pending Override (10313)
                4. Status of TBL_PENDING_OVR_ADT_TRL = 1
            - get_pending_watchlist_type = get watchlist type detailes
            - chk_pending_override_notes = Count notes based on cifid and approverid and approve date
            - sanction_list_discounting
                1. INSERT details to tbl_cif_notes
                2. UPDATE tbl_cifhitlist_percentage : Deleted = 1
                3. UPDATE tbl_cifhitlist : Deleted = 1
            - Update_PENDING_OVR_ADT_TRL =  INSERT to TBL_PENDING_OVR_ADT_TRL
            - SP_INSERT_TO_NOTES - INSERT details to tbl_cif_notes for refinitiv 
            */

            if (reason_disc_val != null && string.IsNullOrEmpty(reason_disc_val).Equals(false))
            {
                return reason_disc_val;
            }
            
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DataSet ds = null;
                DataTable dtt = null;
                int xx = 0;
                if (getactivevalidation() == 0)
                {
                    int i = sqlkonek.execStoredProcedureUpdate("otc_remote_ovr_comrec", new object[] { ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, transaction_id, ovr_type });

                    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
                    isRefinitivEnabled = sqlkonek.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
                    if (isRefinitivEnabled == "1")
                    {
                        string refinitiv_overridetype = refinitivcs.load_refinitiv_overridetype(ovr_type).ToString();
                        if (refinitiv_overridetype == "1")
                        {
                            tnx_details = refinitivcs.getRefinitiv_transdetails(transaction_id);
                            DataSet Remitter_dsIfWithHit = refinitivcs.checkIfCIFwithHIT(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString());
                            if (Remitter_dsIfWithHit != null) { RemitterHit = Remitter_dsIfWithHit.Tables[0].Rows[0]["HIT"].ToString(); }
                            DataSet Beneficiary_dsIfWithHit = refinitivcs.checkIfCIFwithHIT(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString());
                            if (Beneficiary_dsIfWithHit != null) { BeneficiaryHit = Beneficiary_dsIfWithHit.Tables[0].Rows[0]["HIT"].ToString(); }
                            ds_natureofoverride = refinitivcs.getRefinitivHit_NatureOfOverride(ovr_type);
                            if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Remitter"))
                            {
                                if (RemitterHit == "true")
                                {
                                    refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), false, reason_for_discounting + "Approved by " + userid, userid);
                                    new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.rem_ovr_approved", "refinitivcs.ResolveWatchlist", "[Details, TranxID:" + transaction_id + "|RemitterID:" + tnx_details.Tables[0].Rows[0]["RemitterID"].ToString() + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);
                                }
                            }
                            else if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Beneficiary"))
                            {
                                if (BeneficiaryHit == "true")
                                {
                                    refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), false, reason_for_discounting + "Approved by " + userid, userid);
                                    new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.rem_ovr_approved", "refinitivcs.ResolveWatchlist", "[Details, TranxID:" + transaction_id + "|BeneficiaryID:" + tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString() + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);
                                }
                            }
                            else
                            {
                                if (RemitterHit == "true")
                                {
                                    refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), false, reason_for_discounting + "Approved by " + userid, userid);
                                    new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.rem_ovr_approved", "refinitivcs.ResolveWatchlist", "[Details, TranxID:" + transaction_id + "|RemitterID:" + tnx_details.Tables[0].Rows[0]["RemitterID"].ToString() + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);
                                }
                                if (BeneficiaryHit == "true")
                                {
                                    refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), false, reason_for_discounting + "Approved by " + userid, userid);
                                    new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.rem_ovr_approved", "refinitivcs.ResolveWatchlist", "[Details, TranxID:" + transaction_id + "|BeneficiaryID:" + tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString() + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);
                                }
                            }
                        }

                    }
                    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */


                    if (Convert.ToInt32(sqlkonek.getDataSet("scnd_otc_remote_ovr_aproved_qry", true, new object[] { transaction_id, ovr_type }).Tables[0].Rows[0][0]) > 0)
                    {
                        if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                        {
                            string client_id = string.Empty;
                            string client_id_rem = string.Empty;
                            string client_id_ben = string.Empty;
                            string teller_id = string.Empty;
                            string approver_id = string.Empty;
                            string date_created = string.Empty;
                            string date_approve = string.Empty;
                            string overrid_desc = string.Empty;
                            string watchlist = string.Empty;
                            string watchlist_rem = string.Empty;
                            string watchlist_ben = string.Empty;
                            string watchlist_rem_refinitiv = string.Empty;
                            string watchlist_ben_refinitiv = string.Empty;
                            string client_id_rem_refinitiv = string.Empty;
                            string client_id_ben_refinitiv = string.Empty;

                            DataTable dt2 = null;
                            DataRow[] dr = null;
                            DataRow[] dr_rem = null;
                            DataRow[] dr_ben = null;

                            try
                            {
                                dt2 = sqlkonek.getDataSet("get_pending_watchlist_type", true, new object[] { transaction_id }).Tables[0];

                                if (dt2 != null && dt2.Rows.Count > 0)
                                {
                                    dr = dt2.Select("override_typeval = " + ovr_type);
                                    dr_rem = dt2.Select("override_desc like '%Remitter%'");
                                    dr_ben = dt2.Select("override_desc like '%Beneficiary%'");

                                    foreach (DataRow s in dr_rem)
                                    {
                                        watchlist_rem += s["override_desc_short"] + ",";
                                    }

                                    foreach (DataRow s in dr_ben)
                                    {
                                        watchlist_ben += s["override_desc_short"] + ",";
                                    }
                                }

                                if (dr != null && dr.Length > 0)
                                {
                                    watchlist = (string)dr[0]["override_desc_short"];
                                    teller_id = (string)dr[0]["teller_id"];
                                    date_created = Convert.ToDateTime(dr[0]["transaction_date"]).ToString("MM/dd/yyy hh:mm:ss");
                                }
                                else
                                {
                                    return string.Empty;
                                }
                            }
                            catch
                            {
                                return string.Empty;
                            }
                            finally
                            {
                                if (dt2 != null)
                                {
                                    dt2.Dispose();
                                    dt2 = null;
                                }
                            }

                            if (System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"] != null)
                            {
                                ovr = (RemOvrApproval)System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"];
                            }

                            approver_id = irs_client_data.UserId;
                            date_approve = new irsTimeZone().branchLocalTime(irs_client_data.Location).ToString("MM/dd/yyyy hh:mm:ss");
                            overrid_desc = (string)dr[0]["override_desc"];

                            if (overrid_desc.IndexOf("Remitter") >= 0)
                            {
                                client_id = ovr.remitter_cifid;
                                watchlist = watchlist_rem;

                                int xr = 0;
                                DataTable dtr = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id, approver_id, date_approve, transaction_id }).Tables[0];

                                if (dtr != null && dtr.Rows.Count > 0 && Convert.ToInt32(dtr.Rows[0]["cnt_notes"]) == 0)
                                {
                                    xr = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting", new object[] { client_id, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                    if (xr > 0)
                                    {
                                        new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                    }
                                }
                                if (dtr != null)
                                {
                                    dtr.Dispose();
                                    dtr = null;
                                }
                            }
                            else if (overrid_desc.IndexOf("Beneficiary") >= 0)
                            {
                                client_id = ovr.beneficiary_cifid;
                                watchlist = watchlist_ben;
                                int xb = 0;
                                DataTable dtb = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id, approver_id, date_approve, transaction_id }).Tables[0];

                                if (dtb != null && dtb.Rows.Count > 0 && Convert.ToInt32(dtb.Rows[0]["cnt_notes"]) == 0)
                                {
                                    xb = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting", new object[] { client_id, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                    if (xb > 0)
                                    {
                                        new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                    }
                                }
                                if (dtb != null)
                                {
                                    dtb.Dispose();
                                    dtb = null;
                                }
                            }
                            else if (overrid_desc.IndexOf("REFINITIV") >= 0)
                            {
                                client_id_rem_refinitiv = ovr.remitter_cifid;
                                client_id_ben_refinitiv = ovr.beneficiary_cifid;

                                int x_r = 0;
                                DataTable dt_r = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id_rem_refinitiv, approver_id, date_approve, transaction_id }).Tables[0];
                                if (dt_r != null && dt_r.Rows.Count > 0 && Convert.ToInt32(dt_r.Rows[0]["cnt_notes"]) == 0)
                                {
                                    x_r = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting_refinitiv", new object[] { client_id_rem_refinitiv, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                    if (x_r > 0)
                                    {
                                        new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                    }
                                }
                                if (dt_r != null)
                                {
                                    dt_r.Dispose();
                                    dt_r = null;
                                }

                                int x_b = 0;
                                DataTable dt_b = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id_ben_refinitiv, approver_id, date_approve, transaction_id }).Tables[0];
                                if (dt_b != null && dt_b.Rows.Count > 0 && Convert.ToInt32(dt_b.Rows[0]["cnt_notes"]) == 0)
                                {
                                    x_b = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting_refinitiv", new object[] { client_id_ben_refinitiv, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                    if (x_b > 0)
                                    {
                                        new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                    }
                                }
                                if (dt_b != null)
                                {
                                    dt_b.Dispose();
                                    dt_b = null;
                                }
                            }
                            if (ds != null)
                            {
                                ds.Dispose();
                                ds = null;
                            }
                            xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { transaction_id, userid, DateTime.Now, true, "OTC", ovr_type, 0 });
                            if (xx < 0)
                            {
                                return "Unable to write log";
                            }
                            return string.Empty;
                        }
                        return "Session Expired";
                    }
                    return "Unable to Override Transaction";
                }
                else
                {
                    /* 
                 - scnd_otc_remote_ovr_aproved_qry = Count tbl_remittance_override_logs
                     1. Override Type  
                     2. UserID is not null
                     3. TranxStatus = Pending Override (10313)
                     4. Status of TBL_PENDING_OVR_ADT_TRL = 1
                 - otc_remote_ovr_comrec
                     1. Update tbl_remittance_override_logs 
                     2. execute usp_auto_untag_hitlist (tbl_cifhitlist and tbl_cifhitlist_percentage)
                 - otc_remote_ovr_aproved_qry = Count tbl_remittance_override_logs
                     1. Override Type
                     2. UserID is equal to Approver UserID
                 - otc_remote_ovr_cancel_qry = Validate if the transaction is not Cancelled Not Implemented
                 - get_pending_watchlist_type = get watchlist type detailes
                 - chk_pending_override_notes = Count notes based on cifid and approverid and approve date
                 - sanction_list_discounting
                     1. INSERT details to tbl_cif_notes
                     2. UPDATE tbl_cifhitlist_percentage : Deleted = 1
                     3. UPDATE tbl_cifhitlist : Deleted = 1
                 - Update_PENDING_OVR_ADT_TRL =  INSERT to TBL_PENDING_OVR_ADT_TRL
                 */

                    ds = validate_Pending_override(transaction_id, "OTC", ovr_type);
                    if (Convert.ToInt32(sqlkonek.getDataSet("scnd_otc_remote_ovr_aproved_qry", true, new object[] { transaction_id, ovr_type }).Tables[0].Rows[0][0]) > 0)
                    {
                        if (ds != null)
                        {
                            dtt = ds.Tables[0];
                            if (Convert.ToBoolean(ds.Tables[0].Rows[0][1]) == true)
                            {
                                this.log_remote_ovr("The transaction has already been approved. transactionid=" + transaction_id);
                                return "Transaction has been approved by " + dtt.Rows[0]["userid"].ToString();

                            }
                            else
                            {
                                this.log_remote_ovr("The transaction has already been rejected. transactionid=" + transaction_id);
                                return "Transaction has been rejected by " + dtt.Rows[0]["userid"].ToString();
                            }
                        }
                    }
                    else
                    {
                        int i = sqlkonek.execStoredProcedureUpdate("otc_remote_ovr_comrec", new object[] { ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, transaction_id, ovr_type });

                        /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
                        isRefinitivEnabled = sqlkonek.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
                        if (isRefinitivEnabled == "1")
                        {
                            string refinitiv_overridetype = refinitivcs.load_refinitiv_overridetype(ovr_type).ToString();
                            if (refinitiv_overridetype == "1")
                            {
                                //ds_natureofoverride = refinitivcs.getRefinitivHit_NatureOfOverride(ovr_type);
                                //tnx_details = refinitivcs.getRefinitiv_transdetails(transaction_id);
                                //if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Remitter")) { refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), false, reason_for_discounting, userid); }
                                //else if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Beneficiary")) { refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), false, reason_for_discounting, userid); }
                                //else
                                //{
                                //    refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), false, "FALSE-POSITIVE", userid);
                                //    refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), false, "FALSE-POSITIVE", userid);
                                //}

                                tnx_details = refinitivcs.getRefinitiv_transdetails(transaction_id);
                                DataSet Remitter_dsIfWithHit = refinitivcs.checkIfCIFwithHIT(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString());
                                if (Remitter_dsIfWithHit != null) { RemitterHit = Remitter_dsIfWithHit.Tables[0].Rows[0]["HIT"].ToString(); }
                                DataSet Beneficiary_dsIfWithHit = refinitivcs.checkIfCIFwithHIT(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString());
                                if (Beneficiary_dsIfWithHit != null) { BeneficiaryHit = Beneficiary_dsIfWithHit.Tables[0].Rows[0]["HIT"].ToString(); }
                                ds_natureofoverride = refinitivcs.getRefinitivHit_NatureOfOverride(ovr_type);
                                if (ds_natureofoverride != null)
                                {
                                    if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Remitter"))
                                    {
                                        if (RemitterHit == "true") { refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), false, reason_for_discounting, userid); }
                                    }
                                    if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Beneficiary"))
                                    {
                                        if (BeneficiaryHit == "true") { refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), false, reason_for_discounting, userid); }
                                    }
                                    else
                                    {
                                        if (RemitterHit == "true") { refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), false, "FALSE-POSITIVE", userid); }
                                        if (BeneficiaryHit == "true") { refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), false, "FALSE-POSITIVE", userid); }
                                    }
                                }
                            }
                        }
                        /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */

                        if (Convert.ToInt32(sqlkonek.getDataSet("otc_remote_ovr_aproved_qry", true, new object[] { transaction_id, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, ovr_type }).Tables[0].Rows[0][0]) > 0)
                        {
                            if (Convert.ToInt32(sqlkonek.getDataSet("otc_remote_ovr_cancel_qry", true, new object[] { transaction_id }).Tables[0].Rows[0]["cnt_cancel"]) > 0)
                            {
                                if (ds != null)
                                {
                                    dtt = ds.Tables[0];
                                    if (Convert.ToBoolean(ds.Tables[0].Rows[0][1]) == true)
                                    {
                                        this.log_remote_ovr("The transaction has already been approved. transactionid=" + transaction_id);
                                        return "Transaction has been approved by " + dtt.Rows[0]["userid"].ToString();

                                    }
                                    else
                                    {
                                        this.log_remote_ovr("The transaction has already been rejected. transactionid=" + transaction_id);
                                        return "Transaction has been rejected by " + dtt.Rows[0]["userid"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                                {
                                    string client_id = string.Empty;
                                    string teller_id = string.Empty;
                                    string approver_id = string.Empty;
                                    string date_created = string.Empty;
                                    string date_approve = string.Empty;
                                    string watchlist = string.Empty;
                                    string overrid_desc = string.Empty;
                                    string watchlist_rem = string.Empty;
                                    string watchlist_ben = string.Empty;
                                    string watchlist_rem_refinitiv = string.Empty;
                                    string watchlist_ben_refinitiv = string.Empty;
                                    string client_id_rem_refinitiv = string.Empty;
                                    string client_id_ben_refinitiv = string.Empty;

                                    DataTable dt2 = null;
                                    DataRow[] dr = null;
                                    DataRow[] dr_rem = null;
                                    DataRow[] dr_ben = null;

                                    try
                                    {
                                        dt2 = sqlkonek.getDataSet("get_pending_watchlist_type", true, new object[] { transaction_id }).Tables[0];

                                        if (dt2 != null && dt2.Rows.Count > 0)
                                        {
                                            dr = dt2.Select("override_typeval = " + ovr_type);
                                            dr_rem = dt2.Select("override_desc like '%Remitter%'");
                                            dr_ben = dt2.Select("override_desc like '%Beneficiary%'");

                                            foreach (DataRow s in dr_rem)
                                            {
                                                watchlist_rem += s["override_desc_short"] + ",";
                                            }

                                            foreach (DataRow s in dr_ben)
                                            {
                                                watchlist_ben += s["override_desc_short"] + ",";
                                            }
                                        }

                                        if (dr != null && dr.Length > 0)
                                        {
                                            watchlist = (string)dr[0]["override_desc_short"];
                                            teller_id = (string)dr[0]["teller_id"];
                                            date_created = Convert.ToDateTime(dr[0]["transaction_date"]).ToString("MM/dd/yyy hh:mm:ss");
                                        }
                                        else
                                        {
                                            xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { transaction_id, userid, DateTime.Now, true, "OTC", ovr_type, 0 });
                                            if (xx < 0)
                                            {
                                                return "Unable to write log";
                                            }
                                            return string.Empty;
                                        }
                                    }
                                    catch
                                    {
                                        xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { transaction_id, userid, DateTime.Now, true, "OTC", ovr_type, 0 });
                                        if (xx < 0)
                                        {
                                            return "Unable to write log";
                                        }
                                        return string.Empty;
                                    }
                                    finally
                                    {
                                        if (dt2 != null)
                                        {
                                            dt2.Dispose();
                                            dt2 = null;
                                        }
                                    }

                                    if (System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"] != null)
                                    {
                                        ovr = (RemOvrApproval)System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"];
                                    }

                                    approver_id = irs_client_data.UserId;
                                    date_approve = new irsTimeZone().branchLocalTime(irs_client_data.Location).ToString("MM/dd/yyyy hh:mm:ss");
                                    overrid_desc = (string)dr[0]["override_desc"];

                                    if (overrid_desc.IndexOf("Remitter") >= 0)
                                    {
                                        client_id = ovr.remitter_cifid;
                                        watchlist = watchlist_rem;

                                        int xr = 0;
                                        DataTable dtr = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id, approver_id, date_approve, transaction_id }).Tables[0];

                                        if (dtr != null && dtr.Rows.Count > 0 && Convert.ToInt32(dtr.Rows[0]["cnt_notes"]) == 0)
                                        {
                                            xr = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting", new object[] { client_id, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                            if (xr > 0)
                                            {
                                                new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                            }
                                        }
                                        if (dtr != null)
                                        {
                                            dtr.Dispose();
                                            dtr = null;
                                        }
                                    }
                                    else if (overrid_desc.IndexOf("Beneficiary") >= 0)
                                    {
                                        client_id = ovr.beneficiary_cifid;
                                        watchlist = watchlist_ben;
                                        int xb = 0;
                                        DataTable dtb = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id, approver_id, date_approve, transaction_id }).Tables[0];

                                        if (dtb != null && dtb.Rows.Count > 0 && Convert.ToInt32(dtb.Rows[0]["cnt_notes"]) == 0)
                                        {
                                            xb = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting", new object[] { client_id, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                            if (xb > 0)
                                            {
                                                new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "TOGGLE IS OFF SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                            }
                                        }
                                        if (dtb != null)
                                        {
                                            dtb.Dispose();
                                            dtb = null;
                                        }
                                    }
                                    else if (overrid_desc.IndexOf("REFINITIV") >= 0)
                                    {
                                        client_id_rem_refinitiv = ovr.remitter_cifid;
                                        client_id_ben_refinitiv = ovr.beneficiary_cifid;

                                        int x_r = 0;
                                        DataTable dt_r = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id_rem_refinitiv, approver_id, date_approve, transaction_id }).Tables[0];
                                        if (dt_r != null && dt_r.Rows.Count > 0 && Convert.ToInt32(dt_r.Rows[0]["cnt_notes"]) == 0)
                                        {
                                            x_r = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting_refinitiv", new object[] { client_id_rem_refinitiv, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                            if (x_r > 0)
                                            {
                                                new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "TOGGLE IS OFF SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                            }
                                        }
                                        if (dt_r != null)
                                        {
                                            dt_r.Dispose();
                                            dt_r = null;
                                        }

                                        int x_b = 0;
                                        DataTable dt_b = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id_ben_refinitiv, approver_id, date_approve, transaction_id }).Tables[0];
                                        if (dt_b != null && dt_b.Rows.Count > 0 && Convert.ToInt32(dt_b.Rows[0]["cnt_notes"]) == 0)
                                        {
                                            x_b = sqlkonek.execStoredProcedureUpdate("sanction_list_discounting_refinitiv", new object[] { client_id_ben_refinitiv, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });
                                            if (x_b > 0)
                                            {
                                                new csAuditLog().writeAudit(approver_id, "WATCH LIST", "SLIST_DISCOUNTING", "TOGGLE IS OFF SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                            }
                                        }
                                        if (dt_b != null)
                                        {
                                            dt_b.Dispose();
                                            dt_b = null;
                                        }
                                    }
                                    if (ds != null)
                                    {
                                        ds.Dispose();
                                        ds = null;
                                    }
                                    xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { transaction_id, userid, DateTime.Now, true, "OTC", ovr_type, 0 });
                                    if (xx < 0)
                                    {
                                        return "Unable to write log";
                                    }
                                    return string.Empty;
                                }
                                return "Session Expired";
                            }
                        }
                        else
                        {
                            if (ds != null)
                            {
                                dtt = ds.Tables[0];
                                if (Convert.ToBoolean(ds.Tables[0].Rows[0][1]) == true)
                                {
                                    this.log_remote_ovr("The transaction has already been approved. transactionid=" + transaction_id);
                                    return "Transaction has been approved by " + dtt.Rows[0]["userid"].ToString();

                                }
                                else
                                {
                                    this.log_remote_ovr("The transaction has already been rejected. transactionid=" + transaction_id);
                                    return "Transaction has been rejected by " + dtt.Rows[0]["userid"].ToString();
                                }
                            }
                        }

                        return "Unable to Override Transaction.";
                    }
                }                
                
                
            }

            return "Session Expired.";
        }
        //=========================================================================================

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string rem_ovr_cancel(int transaction_id, int ovr_type, string reason_for_discounting, string ddl_reason_discounting, string ddl_reason_discounting_bus, string ddl_approver_disposition, string approver_remarks, int cifclass)
        {
            /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
            Refinitiv refinitivcs = new Refinitiv();
            DataSet tnx_details = null;
            string isRefinitivEnabled = "0";
            string RemitterHit = "false";
            string BeneficiaryHit = "false";
            /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */

            DataSet ds = null;
            DataTable dtt = null;
            int xx = 0;
            string userid = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

            ////=========================================================================================
            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
            RemOvrApproval ovr = null;
            irsClientData irs_client_data = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
            string client_id = string.Empty;
            string teller_id = string.Empty;
            string approver_id = string.Empty;
            string date_created = string.Empty;
            string date_approve = string.Empty;
            string watchlist = string.Empty;
            string overrid_desc = string.Empty;
            string watchlist_rem = string.Empty;
            string watchlist_ben = string.Empty;

            DataTable dt2 = null;
            DataRow[] dr = null;
            DataRow[] dr_rem = null;
            DataRow[] dr_ben = null;

            string reason_disc_val = this.transaction_reason_for_discounting(transaction_id, ovr_type, reason_for_discounting, ddl_reason_discounting, ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks, cifclass);

            if (reason_disc_val != null && string.IsNullOrEmpty(reason_disc_val).Equals(false))
            {
                return reason_disc_val;
            }

            try
            {
                dt2 = sqlkonek.getDataSet("get_pending_watchlist_type", true, new object[] { transaction_id }).Tables[0];

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    dr = dt2.Select("override_typeval = " + ovr_type);
                    dr_rem = dt2.Select("override_desc like '%Remitter%'");
                    dr_ben = dt2.Select("override_desc like '%Beneficiary%'");

                    foreach (DataRow s in dr_rem)
                    {
                        watchlist_rem += s["override_desc_short"] + ",";
                    }

                    foreach (DataRow s in dr_ben)
                    {
                        watchlist_ben += s["override_desc_short"] + ",";
                    }
                }

                if (dr != null && dr.Length > 0)
                {
                    watchlist = (string)dr[0]["override_desc_short"];
                    teller_id = (string)dr[0]["teller_id"];
                    date_created = Convert.ToDateTime(dr[0]["transaction_date"]).ToString("MM/dd/yyy hh:mm:ss");
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }
            }

            if (System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"] != null)
            {
                ovr = (RemOvrApproval)System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"];
            }

            overrid_desc = (string)dr[0]["override_desc"];

            if (overrid_desc.IndexOf("Remitter") >= 0)
            {
                client_id = ovr.remitter_cifid;
                watchlist = watchlist_rem;
            }
            else
            {
                client_id = ovr.beneficiary_cifid;
                watchlist = watchlist_ben;
            }

            approver_id = irs_client_data.UserId;
            date_approve = new irsTimeZone().branchLocalTime(irs_client_data.Location).ToString("MM/dd/yyyy hh:mm:ss");
            ////=========================================================================================

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
                isRefinitivEnabled = sqlkonek.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
                if (isRefinitivEnabled == "1")
                {
                    tnx_details = refinitivcs.getRefinitiv_transdetails(transaction_id);
                    DataSet Remitter_dsIfWithHit = refinitivcs.checkIfCIFwithHIT(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString());
                    if (Remitter_dsIfWithHit != null) { RemitterHit = Remitter_dsIfWithHit.Tables[0].Rows[0]["HIT"].ToString(); }
                    DataSet Beneficiary_dsIfWithHit = refinitivcs.checkIfCIFwithHIT(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString());
                    if (Beneficiary_dsIfWithHit != null) { BeneficiaryHit = Beneficiary_dsIfWithHit.Tables[0].Rows[0]["HIT"].ToString(); }
                    if (RemitterHit == "true")
                    {
                        //refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), true, , userid);
                        //refinitivcs.Refinitiv_ArchiveDelete(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), false, userid);
                        refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString(), true, reason_disc_val + "Cancelled By" + userid, userid);
                        new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.rem_ovr_cancel", "refinitivcs.Refinitiv_ArchiveDelete", "[Details, TranxID:" + transaction_id + "|RemitterID:" + tnx_details.Tables[0].Rows[0]["RemitterID"].ToString() + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);
                    }
                    if (BeneficiaryHit == "true")
                    {
                        //refinitivcs.Refinitiv_ArchiveDelete(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), false, userid);
                        refinitivcs.ResolveWatchlist(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString(), true, reason_disc_val + "Cancelled By" + userid, userid);
                        new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.rem_ovr_cancel", "refinitivcs.Refinitiv_ArchiveDelete", "[Details, TranxID:" + transaction_id + "|BeneficiaryID:" + tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString() + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);
                    }

                }
                /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */


                if (getactivevalidation() == 0)
                {
                    int i = sqlkonek.execStoredProcedureUpdate("otc_remote_ovr_comcel", new object[] { ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, transaction_id });

                    if (Convert.ToInt32(sqlkonek.getDataSet("otc_remote_ovr_cancel_qry", true, new object[] { transaction_id }).Tables[0].Rows[0]["cnt_cancel"]) > 0)
                    {
                        ////=========================================================================================
                        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                        int x = 0;
                        DataTable dt = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id, approver_id, date_approve, transaction_id }).Tables[0];

                        if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["cnt_notes"]) == 0)
                        {
                            x = sqlkonek.execStoredProcedureUpdate("cancel_sanction_list_discounting", new object[] { client_id, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });

                            if (x > 0)
                            {
                                new csAuditLog().writeAudit(approver_id, "WATCH LIST", "CANCEL_SLIST_DISCOUNTING", "SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                            }
                        }

                        if (dt != null)
                        {
                            dt.Dispose();
                            dt = null;
                        }
                        ////=========================================================================================

                        xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { transaction_id, userid, DateTime.Now, false, "OTC", ovr_type, 0 });
                        if (xx < 0)
                        {
                            return "Unable to write log";
                        }
                        this.log_remote_ovr("Transaction is rejected using remote override. Transaction Sequence ID: " + transaction_id); return string.Empty;
                    }
                    return "Unable to Override Transaction";
                }
                else
                {
                    ds = validate_Pending_override(transaction_id, "OTC", ovr_type);

                    if (Convert.ToInt32(sqlkonek.getDataSet("scnd_otc_remote_ovr_aproved_qry", true, new object[] { transaction_id, ovr_type }).Tables[0].Rows[0][0]) > 0)
                    {
                        if (ds != null)
                        {
                            dtt = ds.Tables[0];
                            if (Convert.ToBoolean(ds.Tables[0].Rows[0][1]) == true)
                            {
                                this.log_remote_ovr("The transaction has already been approved. transactionid=" + transaction_id);
                                return "Transaction has been approved by " + dtt.Rows[0]["userid"].ToString();

                            }
                            else
                            {
                                this.log_remote_ovr("The transaction has already been rejected. transactionid=" + transaction_id);
                                return "Transaction has been rejected by " + dtt.Rows[0]["userid"].ToString();
                            }
                        }
                    }
                    else
                    {
                        int i = sqlkonek.execStoredProcedureUpdate("otc_remote_ovr_comcel", new object[] { ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, transaction_id });

                        if (Convert.ToInt32(sqlkonek.getDataSet("otc_remote_ovr_cancel_qry", true, new object[] { transaction_id }).Tables[0].Rows[0]["cnt_cancel"]) > 0)
                        {
                            if (Convert.ToInt32(sqlkonek.getDataSet("otc_remote_ovr_aproved_qry", true, new object[] { transaction_id, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, ovr_type }).Tables[0].Rows[0][0]) > 0)
                            {
                                if (ds != null)
                                {
                                    dtt = ds.Tables[0];
                                    if (Convert.ToBoolean(ds.Tables[0].Rows[0][1]) == true)
                                    {
                                        this.log_remote_ovr("The transaction has already been approved. transactionid=" + transaction_id);
                                        return "Transaction has been approved by " + dtt.Rows[0]["userid"].ToString();

                                    }
                                    else
                                    {
                                        this.log_remote_ovr("The transaction has already been rejected. transactionid=" + transaction_id);
                                        return "Transaction has been rejected by " + dtt.Rows[0]["userid"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                ////=========================================================================================
                                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                                int x = 0;
                                DataTable dt = sqlkonek.getDataSet("chk_pending_override_notes", true, new object[] { client_id, approver_id, date_approve, transaction_id }).Tables[0];

                                if (dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["cnt_notes"]) == 0)
                                {
                                    x = sqlkonek.execStoredProcedureUpdate("cancel_sanction_list_discounting", new object[] { client_id, teller_id, approver_id, ddl_reason_discounting, date_created, date_approve, watchlist.ToUpper(), reason_for_discounting, transaction_id.ToString(), ddl_reason_discounting_bus, ddl_approver_disposition, approver_remarks });

                                    if (x > 0)
                                    {
                                        new csAuditLog().writeAudit(approver_id, "WATCH LIST", "CANCEL_SLIST_DISCOUNTING", "SANCTION LIST DISCOUNTING CLIENT CIFID: " + client_id + " APPROVER ID: " + approver_id + " PROCESSOR ID: " + teller_id + " TRANSACTION SEQ ID:" + transaction_id.ToString(), "", csAuditLog.Category.Insert);
                                    }
                                }

                                if (dt != null)
                                {
                                    dt.Dispose();
                                    dt = null;
                                }
                                ////=========================================================================================

                                xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { transaction_id, userid, DateTime.Now, false, "OTC", ovr_type, 0 });
                                if (xx < 0)
                                {
                                    return "Unable to write log";
                                }
                                this.log_remote_ovr("Transaction is rejected using remote override. Transaction Sequence ID: " + transaction_id); return string.Empty;
                            }
                        }
                        else
                        {
                            if (ds != null)
                            {
                                dtt = ds.Tables[0];
                                if (Convert.ToBoolean(ds.Tables[0].Rows[0][1]) == true)
                                {
                                    this.log_remote_ovr("The transaction has already been approved. transactionid=" + transaction_id);
                                    return "Transaction has been approved by " + dtt.Rows[0]["userid"].ToString();

                                }
                                else
                                {
                                    this.log_remote_ovr("The transaction has already been rejected. transactionid=" + transaction_id);
                                    return "Transaction has been rejected by " + dtt.Rows[0]["userid"].ToString();
                                }
                            }
                        }
                    }
                }
                return "Unable to Cancel Transaction.";
            }
            return "Session Expired.";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_screen()
        {
            DataTable dt = null;

            string local_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location)).ToString("MM/dd/yyyy");
            string user_role = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Role.ToString();
            string user_location = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;

            try
            {
                dt = sqlkonek.getDataSet("otc_remote_over_chk", true, new object[] { user_role }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = Convert.ToInt32(dt.Rows[0]["cnt_roles"]);

                    if (i <= 0)
                    {
                        user_role = "-1";
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }

            return local_date + '~' + user_role + '~' + user_location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void override_ok(int transaction_id)
        {
            /* VAPT IAG 20191007 */
            string RequestTime = System.DateTime.Now.ToString();
            /*********************/

            int i = 0;

            string location_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location)).ToString("MM/dd/yyyy");

            irsRemTeller rem_teller = new irsRemTeller();
            DateTime dt_ach_check = Convert.ToDateTime(location_date);
            DataTable dt = null;

            try
            {
                dt = sqlkonek.getDataSet("otc_remote_ovr_ok", true, new object[] { transaction_id }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["payMode"]) == Convert.ToInt32(dt.Rows[0]["chk_paymode"]) || Convert.ToInt32(dt.Rows[0]["payMode"]) == Convert.ToInt32(dt.Rows[0]["ach_paymode"]))
                    {
                        if (string.IsNullOrEmpty(dt.Rows[0]["clearing_date"].ToString()).Equals(false) && Convert.ToDateTime(dt.Rows[0]["clearing_date"]) > dt_ach_check)
                        {
                            i = sqlkonek.execStoredProcedureUpdate("otc_remote_ovr_ach_chk_commit", new object[] { transaction_id }); return;
                        }
                    }

                    dt = sqlkonek.getDataSet("otc_remote_ovr_stat_chk", true, new object[] { transaction_id }).Tables[0];

                    if (dt != null && Convert.ToInt32(dt.Rows[0]["ok_to_process"]) == 1)
                    {
                        /* VAPT IAG 20191007 */
                        new csAuditLog().writeVAPTLogger("", "OVERRIDE", "APPROVER APPROVED OVERRIDE", "", "", "OK TO PROCESS~" + dt.Rows[0]["ok_to_process"].ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);

                        rem_teller.Remit2(transaction_id);

                        /* VAPT IAG 20191007 */
                        new csAuditLog().writeVAPTLogger("", "OVERRIDE", "APPROVER APPROVED TRANSACTION OVERRIDE", "", "", "irsRemTeller.Remit2~" + transaction_id.ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                    }
                }
            }
            catch
            {
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (rem_teller != null)
                {
                    rem_teller = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string approve_enrollment_hitlist(int client_seq_id)
        {
            /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API  */
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            Refinitiv refinitivcs = new Refinitiv();
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            string CIFID = string.Empty;
            string NOTE = string.Empty;
            /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API  */

            try
            {
                /* VAPT IAG 20191007 */
                string userid = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                string RequestTime = System.DateTime.Now.ToString();
                /*********************/
                int xx = 0;
                int i = 0;
                bool appr = false;
                if (client_seq_id < 1)
                {
                    return "Please select client from the list.";
                }

                if (this.is_allowed_to_ovr() == false)
                {
                    return "Invalid user role.";
                }
                else
                { 
                    i = sqlkonek.execStoredProcedureUpdate("otc_remote_ovr_enroll_approve_or_reject", new object[] { 1, client_seq_id });
                    
                }

                /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */

                string isRefinitivEnabled = sqlkonek.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
                if (isRefinitivEnabled == "1")
                {
                    //DataSet ds_reasonfordiscounting = refinitivcs.getRefinitiv_reasonfordiscounting("", client_seq_id);
                    DataTable dt = new csAuditLog().getCIFTXN(client_seq_id, 1);
                    CIFID = dt.Rows[0]["CIFID"].ToString();
                    NOTE = "FALSE-POSITIVE.";
                    refinitivcs.ResolveWatchlist(CIFID, false, NOTE, userid);
                    new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.approve_enrollment_hitlist", "refinitivcs.ResolveWatchlist", "[Details, CIFID:" + dt.Rows[0]["CIFID"].ToString() + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);

                }
                /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */

                if (i < 1)
                {
                    this.log_remote_ovr("Unable to approve enrolled client information."); return "Unable to approve enrolled client information.";
                }

                appr = validateApproveCIF(client_seq_id);
                if (appr)
                {
                    xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { client_seq_id, userid, DateTime.Now, true, "CIF", 0, 0 });
                    if (xx < 0)
                    {
                        return "CIF approved Successful. Error Message: Cannot write log";
                    }
                }
                /* VAPT 20181022 */
                try
                {
                    DataTable dt = new csAuditLog().getCIFTXN(client_seq_id, 1);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        new csAuditLog().writeVAPTLogger("", "OVERRIDE", "APPROVER APPROVED CIF OVERRIDE", dt.Rows[0]["CIFID"].ToString() + " - " + dt.Rows[0]["FullName"].ToString(), "", client_seq_id.ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                    }
                    else
                    {
                        new csAuditLog().writeVAPTLogger("", "OVERRIDE", "APPROVER APPROVED CIF OVERRIDE", "", "", client_seq_id.ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                    }
                }
                catch { }
                this.log_remote_ovr("Enrollment approval OK. CIF SEQ ID: " + client_seq_id.ToString()); return string.Empty;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "OVERRIDE", "PENDING OVERRIDE ERROR", ex.Message + "~" + ex.StackTrace, client_seq_id.ToString(), csAuditLog.Category.Select);
                return "Unable to Approve CIF. Check VAPT Log";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string cancel_enrollment_hitlist(int client_seq_id)
        {
            /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            Refinitiv refinitivcs = new Refinitiv();
            /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */

            try
            {
                string userid = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                int i = 0;
                int xx = 0;
                bool rejectval = false;
                if (client_seq_id < 1)
                {
                    return "Please select client from the list.";
                }

                /* RFC-8800 IRSWORLD NY */
                string remark_trans = get_cif_processed_tranx_remarks(client_seq_id);

                if (remark_trans.ToUpper() != "OK")
                {
                    return remark_trans;
                }
                /* RFC-8800 IRSWORLD NY */

                if (this.is_allowed_to_ovr() == false)
                {
                    return "Invalid user role.";
                }
                else
                {
                    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */
                    string isRefinitivEnabled = sqlkonek.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
                    if (isRefinitivEnabled == "1")
                    {
                        string getrefinitiv_clientcifid = refinitivcs.getRefinitiv_ClientCIFID(client_seq_id);
                        //refinitivcs.Refinitiv_ArchiveDelete(getrefinitiv_clientcifid, false, userid);
                        refinitivcs.ResolveWatchlist(getrefinitiv_clientcifid, true, "CIF REJECTED BY " + userid, userid);
                        new csAuditLog().writeAudit("", "REFINITIV", "Remittance.RemOvrApproval.cancel_enrollment_hitlist", "refinitivcs.ResolveWatchlist", "[Details, CIFID:" + getrefinitiv_clientcifid + "|ApproverID:" + userid, csAuditLog.Category.Refinitiv);

                    }
                    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */


                    i = sqlkonek.execStoredProcedureUpdate("otc_remote_ovr_enroll_approve_or_reject", new object[] { 0, client_seq_id }); // IF @apr_rej_flag = 0 THEN REJECT
                }

                if (i < 1)
                {
                    return "Unable to reject enrolled client information.";
                }
                else
                {
                    /* RFC-8800 */
                    new csAuditLog().writeVAPTLogger("", "OVERRIDE", "APPROVER REJECT CIF OVERRIDE", "", "", client_seq_id.ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                    /* RFC-8800 */
                }
                rejectval = validateRejectCIF(client_seq_id);
                if (rejectval)
                {
                    xx = sqlkonek.execStoredProcedureUpdate("Update_PENDING_OVR_ADT_TRL", new object[] { client_seq_id, userid, DateTime.Now, false, "CIF", 0, 0 });
                    if (xx < 0)
                    {
                        return "CIF Rejected successful. Error Message: Unable to write log";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "OVERRIDE", "PENDING OVERRIDE ERROR", ex.Message + "~" + ex.StackTrace, client_seq_id.ToString(), csAuditLog.Category.Select);
                return "Unable to Reject CIF. Check VAPT Log";
            }
        }

        private void log_remote_ovr(string msg)
        {
            try
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "REMOTE OVERRIDE", msg, string.Empty, string.Empty, csAuditLog.Category.Insert);
            }
            catch
            {
            }
        }

        private bool is_enrolled_has_no_transaction(int cif_seq_id)
        {
            DataTable dt = null;

            //FALSE - HAS TRANSACTION
            //TRUE - NO TRANSACTION

            try
            {
                dt = sqlkonek.getDataSet("otc_remote_ovr_enroll_check", true, new object[] { cif_seq_id }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return true;
            }

            return true;
        }

        private bool is_allowed_to_ovr()
        {
            DataTable dt = null;

            string user_role = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Role.ToString();

            try
            {
                dt = sqlkonek.getDataSet("otc_remote_over_chk", true, new object[] { user_role }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    int i = Convert.ToInt32(dt.Rows[0]["cnt_roles"]);

                    if (i <= 0)
                    {
                        return false;
                    }

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        /* RFC-8800 IRSWSORLD NY */
        private string get_cif_processed_tranx_remarks(int cif_seq_id)
        {
            DataTable dt = null;
            try
            {
                dt = sqlkonek.getDataSet("otc_remote_ovr_cif_enroll_check_tranx", true, new object[] { cif_seq_id }).Tables[0];

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["cif_tranx_rem"] != null && dt.Rows[0]["cif_tranx_rem"].ToString().ToUpper() != "OK")
                {
                    return dt.Rows[0]["cif_tranx_rem"].ToString();
                }

                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }
        /* RFC-8800 IRSWSORLD NY */

        //=========================================================================================
        /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
        public string transaction_reason_for_discounting(int transaction_id, int ovr_type, string santion_notes_other, string sanction_discounting_reason, string sanction_discounting_reason_bus, string sanction_disposition_reason, string sanction_disposition_reason_remarks, int cifclass)
        {
            DataTable dt = null;
            DataRow[] dr = null;

            DataSet tnx_details = null;
            DataTable dtCIF = null;
            string retval_watchlist = "";
            string isRefinitivEnabled = "0";
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            RemTranExtra RemExtra = new RemTranExtra();
            try
            {
                dt = sqlkonek.getDataSet("get_pending_watchlist_type", true, new object[] { transaction_id }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    dr = dt.Select("override_typeval = " + ovr_type);
                }

                ////=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                //if (dr != null && dr.Length > 0 && string.IsNullOrEmpty(sanction_notes).Equals(true))
                //{
                //    return "Please input Reason for Discounting.";
                //}

                //if (dr != null && dr.Length > 0 && sanction_notes == "99" && string.IsNullOrEmpty(santion_notes_other).Equals(true))
                //{
                //    return "Please input Reason Other Details.";
                //}
                //string retval_watchlist = "";

                isRefinitivEnabled = this.isRefinitivEnabled();
                if (ovr_type == 50)
                {
                    if (isRefinitivEnabled == "1")
                    {
                        retval_watchlist = Convert.ToString(validate_watchlist_refinitiv(transaction_id, sanction_discounting_reason.ToString(), sanction_discounting_reason_bus.ToString(), sanction_disposition_reason.ToString(), sanction_disposition_reason_remarks.ToString(), "2", cifclass.ToString()));
                    }
                    else
                    {
                        retval_watchlist = Convert.ToString(validate_watchlist(transaction_id, sanction_discounting_reason.ToString(), sanction_discounting_reason_bus.ToString(), sanction_disposition_reason.ToString(), sanction_disposition_reason_remarks.ToString(), "2", cifclass.ToString()));
                    }
                }
                else {
                    retval_watchlist = Convert.ToString(validate_watchlist(transaction_id, sanction_discounting_reason.ToString(), sanction_discounting_reason_bus.ToString(), sanction_disposition_reason.ToString(), sanction_disposition_reason_remarks.ToString(), "2", cifclass.ToString()));
                }

                if (retval_watchlist.Trim() != "")
                {
                    return retval_watchlist;
                }
                else {
                    return string.Empty;
                    //isRefinitivEnabled = this.isRefinitivEnabled();
                    //if (isRefinitivEnabled == "1")
                    //{
                    //    try
                    //    {
                    //        tnx_details = refinitivcs.getRefinitiv_transdetails(transaction_id);
                    //        if (cifclass == '1') { dtCIF = irsrefinitiv.getCIFInfo(tnx_details.Tables[0].Rows[0]["RemitterID"].ToString()); }
                    //        else { dtCIF = irsrefinitiv.getCIFInfo(tnx_details.Tables[0].Rows[0]["BeneficiaryID"].ToString()); }

                    //        if (dtCIF.Rows[0]["CIFType"] != DBNull.Value && dtCIF.Rows[0]["CIFType"].ToString() != string.Empty)
                    //        {
                    //            if (dtCIF.Rows[0]["CIFType"].ToString().Trim() == "1")
                    //            {
                    //                /* INDIVIDUAL */
                    //                if (sanction_disposition_reason == string.Empty)
                    //                { retval_watchlist = "Please provide sanction_disposition_reason"; }
                    //            }
                    //            else /* BUSINESS OR OTHERS */
                    //            {
                    //                if (sanction_discounting_reason_bus == string.Empty)
                    //                { retval_watchlist = "Please provide sanction_discounting_reason_bus"; }
                    //            }
                    //        }
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        return "Error encountered: " + e.Message;
                    //    }
                    //    finally
                    //    {
                    //        if (dtCIF != null)
                    //        {
                    //            dtCIF.Dispose();
                    //            dtCIF = null;
                    //        }
                    //    }
                    //}
                }
                ////=========================================================================================
            }
            catch (Exception e)
            {
                return "Error encountered: " + e.Message;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_reason_for_discounting(int transaction_id, int ovr_type)
        {
            string overrid_desc = string.Empty;

            DataTable dt1 = null;
            DataTable dt2 = null;
            DataRow[] dr = null;
            irsClientData irs_client_data = null;
            RemOvrApproval ovr = null;

            try
            {
                irs_client_data = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                dt1 = sqlkonek.getDataSet("get_pending_watchlist_type", true, new object[] { transaction_id }).Tables[0];

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    dr = dt1.Select("override_typeval = " + ovr_type);
                }

                if (dr != null && dr.Length > 0)
                {
                    overrid_desc = (string)dr[0]["override_desc"];
                }

                if (System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"] != null)
                {
                    ovr = (RemOvrApproval)System.Web.HttpContext.Current.Session["REM_OVR_APPROVAL_CLASS"];
                }

                if (ovr != null)
                {
                    string approver_id = irs_client_data.UserId;
                    string date_approve = new irsTimeZone().branchLocalTime(irs_client_data.Location).ToString("MM/dd/yyyy");

                    if (string.IsNullOrEmpty(overrid_desc).Equals(false) && overrid_desc.IndexOf("Remitter") >= 0)
                    {
                        dt2 = sqlkonek.getDataSet("get_pending_reason_for_discounting", true, new object[] { ovr.remitter_cifid, approver_id, date_approve }).Tables[0];
                    }
                    else if (string.IsNullOrEmpty(overrid_desc).Equals(false) && overrid_desc.IndexOf("Beneficiary") >= 0)
                    {
                        dt2 = sqlkonek.getDataSet("get_pending_reason_for_discounting", true, new object[] { ovr.beneficiary_cifid, approver_id, date_approve }).Tables[0];
                    }
                    else if (string.IsNullOrEmpty(overrid_desc).Equals(false) && overrid_desc.IndexOf("REFINITIV") >= 0)
                    {
                        dt2 = sqlkonek.getDataSet("get_pending_reason_for_discounting_refinitiv", true, new object[] { transaction_id, approver_id, date_approve }).Tables[0];
                    }
                    else 
                    {
                        return string.Empty;
                    }
                }

                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    return Convert.ToString(dt2.Rows[0]["reason_for_discounting"]);
                    
                }

                return string.Empty;

            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string validate_watchlist_refinitiv(int tranxid, string discounting_individual, string discounting_business, string disposition, string disposition_remarks, string inquirytype, string cifclass)
        {
            string retval = "";
            string retval_rem = "";
            string retval_ben = "";
            string remitterciftype = "";
            string beneficiaryciftype = "";
            string cifid = "";
            string Remittercifid = "";
            string Beneficiarycifid = "";
            int RemType = 0;
            int BenType = 0;
            DataTable sql = new DataTable();
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsRefinitiv = new irsRefinitiv();
            DataTable dt_refinitiv = new DataTable();
            DataTable dtCaseSytemID = new DataTable();
            DataTable dt_remitterciftype = new DataTable();
            DataTable dt_beneficiaryciftype = new DataTable();
            dt_refinitiv = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            
            /*    
            Inquiry Type:    
            1 - Inquire CIF / Watchlist Type    
            2 - Validate watchlist    
            
            CIF Type:    
            1 - Individual    
            2 - Business    
            */

            try
            {
                if (dt_refinitiv != null && dt_refinitiv.Rows.Count > 0)
                {
                    DataView view = new DataView(dt_refinitiv);
                    DataTable distinctTable = view.ToTable(true, "CaseSystemID");
                    foreach (DataRow row in distinctTable.Rows)
                    {
                        dtCaseSytemID = irsRefinitiv.getRefinitivByCaseSystemID(row[0].ToString());
                        if (dtCaseSytemID != null && dtCaseSytemID.Rows.Count > 0)
                        {
                            cifid = dtCaseSytemID.Rows[0]["CIFID"].ToString();
                            sql = commons.getDataSet("SELECT TOP 1 RemitterId, RemType, BeneficiaryID, BenType FROM TBL_REMITTANCE_IRSW WHERE TXN_SEQ_ID = " + tranxid, false, new object[] { }).Tables[0];
                            Remittercifid = sql.Rows[0]["RemitterId"].ToString();
                            remitterciftype = sql.Rows[0]["RemType"].ToString();
                            Beneficiarycifid = sql.Rows[0]["RemitterId"].ToString();
                            beneficiaryciftype = sql.Rows[0]["RemType"].ToString();
                            if (inquirytype == "")
                            {
                                if (Remittercifid == cifid)
                                {
                                    if (remitterciftype == "Individual")
                                    { retval_rem = "1"; }
                                    else if (remitterciftype == "Business")
                                    { retval_rem = "2"; }
                                    else { retval_rem = "0"; }
                                }
                                else
                                {
                                    retval_rem = "0";
                                }
                                if (Beneficiarycifid == cifid)
                                {
                                    if (beneficiaryciftype == "Individual")
                                    { retval_ben = "1"; }
                                    else if (beneficiaryciftype == "Business")
                                    { retval_ben = "2"; }
                                    else { retval_ben = "0"; }
                                }
                                else
                                {
                                    retval_ben = "0";
                                }

                            }
                            else if (inquirytype == "2")
                            {
                                if (Remittercifid == cifid)
                                {
                                    if (remitterciftype == "Individual")
                                    {
                                        if (discounting_individual == "" && disposition == "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Individual) and Approver Disposition.";
                                        }
                                        else if (discounting_individual == "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Individual).";
                                        }
                                        else if (discounting_individual != "" && disposition == "")
                                        {
                                            retval_rem = "Please provide Approver Disposition for watchlisted Remitter.";
                                        }
                                        else if (disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_rem = "Please provide Approver Remarks for watchlisted Remitter.";
                                        }
                                        else if (discounting_individual == "" && disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Individual) and Approver Remarks.";
                                        }
                                    }
                                    else
                                    {
                                        if (discounting_business == "" && disposition == "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Business) and Approver Disposition.";
                                        }
                                        else if (discounting_business == "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Business).";
                                        }
                                        else if (discounting_business != "" && disposition == "")
                                        {
                                            retval_rem = "Please provide Approver Disposition for watchlisted Remitter.";
                                        }
                                        else if (disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_rem = "Please provide Approver Remarks for watchlisted Remitter.";
                                        }
                                        else if (discounting_business == "" && disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Business) and Approver Remarks.";
                                        }
                                    }
                                }
                                if (Beneficiarycifid == cifid)
                                {
                                    if (beneficiaryciftype == "Individual")
                                    {
                                        if (discounting_individual == "" && disposition == "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Individual) and Approver Disposition.";
                                        }
                                        else if (discounting_individual == "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Individual).";
                                        }
                                        else if (discounting_individual != "" && disposition == "")
                                        {
                                            retval_ben = "Please provide Approver Disposition for watchlisted Remitter.";
                                        }
                                        else if (disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_ben = "Please provide Approver Remarks for watchlisted Remitter.";
                                        }
                                        else if (discounting_individual == "" && disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Individual) and Approver Remarks.";
                                        }
                                    }
                                    else
                                    {
                                        if (discounting_business == "" && disposition == "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Business) and Approver Disposition.";
                                        }
                                        else if (discounting_business == "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Business).";
                                        }
                                        else if (discounting_business != "" && disposition == "")
                                        {
                                            retval_ben = "Please provide Approver Disposition for watchlisted Remitter.";
                                        }
                                        else if (disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_ben = "Please provide Approver Remarks for watchlisted Remitter.";
                                        }
                                        else if (discounting_business == "" && disposition != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Business) and Approver Remarks.";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (inquirytype == "1")
                    {
                        retval = retval_rem + "~" + retval_ben;
                    }
                    if (inquirytype == "2")
                    {
                        if (retval_rem != "")
                        {
                            if (retval_ben != "")
                            {
                                retval = retval_rem + "~" + retval_ben;
                            }
                            else
                            { retval = retval_rem; }
                        }
                        else
                        {
                            if (retval_ben != "")
                            {
                                retval = retval_ben;
                            }
                            else
                            { retval = ""; }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "REMOTE OVERRIDE REFINITIV - validate_watchlist_refinitiv", e.Message + " | Tranx=" + tranxid, string.Empty, string.Empty, csAuditLog.Category.Insert);
            }
            finally
            {
                commons = null;
            }
            return retval;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string validate_watchlist(int tranxid, string discounting_individual, string discounting_business, string disposition, string disposition_remarks, string inquirytype, string cifclass)
        {
            DataTable dt = new DataTable();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            string retval = "";

            try
            {
                dt = commons.execStoredProcedureTable("proc_validate_pending_watchlist_details", new object[] { tranxid, discounting_individual, discounting_business, disposition, disposition_remarks, Convert.ToInt32(inquirytype), Convert.ToInt32(cifclass) });

                if (dt != null && dt.Rows.Count > 0)
                {
                    retval = dt.Rows[0][0].ToString();
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                commons = null;
            }
            return retval;
        }
        //=========================================================================================

        /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isRefinitivEnabled()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            string isRefinitivEnabled = "0";
            System.Web.HttpContext.Current.Session["RefinitivConfiguration_Flag"] = null;
            isRefinitivEnabled = commons.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
            if (isRefinitivEnabled == "1")
            {
                System.Web.HttpContext.Current.Session["RefinitivConfiguration_Flag"] = isRefinitivEnabled;
                return isRefinitivEnabled;
            }
            return isRefinitivEnabled;
        }

        /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_refinitiv(string cifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            RemTranExtra remtranextra = new RemTranExtra();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            String hitlist = "";
            bool down = false;
            DataTable dt = null;

            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual = false;

            List<string> secondary = new List<string>();
            DataTable dtCIF = irsrefinitiv.getCIFInfo(cifid);
            if (dtCIF.Rows[0]["CIFType"] != DBNull.Value && dtCIF.Rows[0]["CIFType"].ToString() != string.Empty)
            {
                if (dtCIF.Rows[0]["CIFType"].ToString().Trim() == "1")
                {
                    isRefinitivIndividual = true;
                }
                else
                {
                    isRefinitivIndividual = false;
                }
            }

            try
            {

                dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //NEWLY CREATED CIFID

                if (down == true)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Remarks", typeof(string));
                    dt.Rows[0][1] = "Refinitiv is down.";
                    return dt;
                }
                else
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "REMOTE OVERRIDE REFINITIV", ex.Message + " | CIFID=" + cifid, string.Empty, string.Empty, csAuditLog.Category.Insert);
                return dt;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }
        /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */

    }
}