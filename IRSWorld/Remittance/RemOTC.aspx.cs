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
using System.Drawing;
using System.Drawing.Printing;
using System.ComponentModel;
using System.IO;
using IRSClassLib;
using IRSWorld.TranClass;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using System.Collections.Generic;
using System.Linq;

namespace IRSWorld.Remittance
{
    public partial class RemOTC : System.Web.UI.Page
    {
        private sqlHelperWs sqlkonek = new sqlHelperWs();
        private int invalidAccountCode = 0;
        private csAuditLog logger = new csAuditLog();
        private string acct_algo = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemOTC));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_door_saving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_account_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_open_accounts));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_check_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ord_data_entry));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_card_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranGFMCCommons));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(remit_benef_product));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_debit_account_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_card_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(commit_beneficiary_zipcode));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_denomination));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_ach_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_bpo_create_accounts));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_exp_ids));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_states_chk));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ibft_scr));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(instapay_scr));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Refinitiv)); /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */

            //preload_idtypes();

            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        private String get_branchCode()
        {
            if (Session["IRS_CLIENT_DATA"] != null) return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location; return null;
        }

        private DateTime get_transaction_rate_date()
        {
            return Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));
        }

        private irsGRSBatchEntries grsbatchentry()
        {
            if (System.Web.HttpContext.Current.Session["IBATCH_HEADER"] != null)
            {
                return (irsGRSBatchEntries)System.Web.HttpContext.Current.Session["IBATCH_HEADER"];
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void clear_otc_session(int x)
        {
            if ((object)Session["IRS_OVERRIDE_LOGDATA"] != null || (string)Session["IRS_OVERRIDE_LOGDATA"] != string.Empty)
            {
                string hitlist_cnt = (string)Session["IRS_OVERRIDE_LOGDATA"];
                string[] hitlist_arr = null;

                if (hitlist_cnt != null && hitlist_cnt != string.Empty && hitlist_cnt.Length > 0)
                {
                    hitlist_arr = hitlist_cnt.Split('~');
                }

                if (hitlist_arr != null && hitlist_arr.Length > 0)
                {
                    for (int i = 0; i < hitlist_arr.Length; i++)
                    {
                        if (hitlist_arr[i].IndexOf("25") > 0)
                        {
                            Session["OTC_HIT_LIST_VAL"] = hitlist_arr[i] + "~"; break;
                        }
                        if (hitlist_arr[i].IndexOf("50") > 0)
                        {
                            Session["OTC_HIT_LIST_VAL"] = hitlist_arr[i] + "~"; break;
                        } /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
                    }
                }
            }

            if (x > 0)
            {
                /* CANCEL TRANSACTION */
                System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"] = null;
                System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;
                System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS"] = null;
                System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] = null; //NEW DDC APRIL 28 2016       
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                
            }
            else
            {
                System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;
                System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS"] = null;
                if (Session["OTC_HIT_LIST_VAL"].ToString().Contains("50"))
                { }
                else { System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; }

                /* NEW DDC APRIL 28 2016 */
                if (System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"].ToString()).Equals(false))
                    {
                        System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"] = null;
                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"];
                    }
                }
                /* NEW DDC APRIL 28 2016 */
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_gfc_remiter_card_id(string gfc_remitter_card_id)
        {
            if (gfc_remitter_card_id != null && gfc_remitter_card_id.Length == 19)
            {
                return sqlkonek.getDataSet(sqlkonek.getSqlCommand("SELECT CIFID FROM TBL_CIF_IDS WHERE IDNO = '" + gfc_remitter_card_id.Substring(0, 16) + "'", false));
            }

            return null;
        }

        //Instapay Alyssa 2019
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_instapay_setup()
        {
            DataTable dt = sqlkonek.getDataSet("SELECT FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'INSTAPAYMaxAmount' ", false, new Object[] { }).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                string instapay_maxamount = dt.Rows[0]["FLDDESCRIPTION"].ToString();
                return instapay_maxamount;
            }
            return string.Empty;
        }//Instapay Alyssa 2019

        //Instapay Alyssa 2019
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool get_InstapayBank(string bank_name)
        {
            DataTable dt = null;

            try
            {
                dt = sqlkonek.getDataSet("SELECT BranchCode FROM TBL_BRANCHES WHERE BRANCHTYPE = 12 AND BRANCHCODE = " + bank_name.ToString(), false, new Object[] { }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["BranchCode"] != null && dt.Rows[0]["BranchCode"].ToString().Trim() != "" && dt.Rows[0]["BranchCode"].ToString().Trim().Length > 2)
                    {
                        return true;
                    }
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
        }        //Instapay Alyssa 2019

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_br_and_acct(int account_seq_id, int account_currency, int br_seq_id)
        {
            DataTable dt1 = null;
            DataTable dt2 = null;

            try
            {
                if (account_seq_id > 0)
                {
                    dt1 = sqlkonek.getDataSet("select currency  from tbl_cif_accounts where seq_id =" + account_seq_id.ToString(), false, new object[] { }).Tables[0];

                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt1.Rows[0]["currency"].ToString()) == true)
                        {
                            return "Invalid beneficiary account currency. ";
                        }

                        if (Convert.ToInt32(dt1.Rows[0]["currency"]) != account_currency)
                        {
                            return "Invalid beneficiary account currency. ";
                        }
                    }
                    else
                    {
                        return "Beneficiary account not found.";
                    }
                }

                if (br_seq_id > 0)
                {
                    dt2 = sqlkonek.getDataSet("select * from tbl_branches where seq_id = " + br_seq_id.ToString(), false, new object[] { }).Tables[0];

                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt2.Rows[0]["BranchType"].ToString()) == true)
                        {
                            return "Invalid domestic branch.";
                        }

                        if (Convert.ToInt32(dt2.Rows[0]["BranchType"].ToString()) == 2 || Convert.ToInt32(dt2.Rows[0]["BranchType"].ToString()) == 5 || Convert.ToInt32(dt2.Rows[0]["BranchType"].ToString()) == 11 || Convert.ToInt32(dt2.Rows[0]["BranchType"].ToString()) == 12) //Instapay
                        {
                            return string.Empty;
                        }
                        else
                        {
                            return "Invalid domestic branch.";
                        }
                    }
                    else
                    {
                        return "domestic branch not found.";
                    }
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                return "error encountered: " + e.Message;
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_loyalty_info(string loyalty_number)
        {
            return new IRSWorld.Internet.InternetRemitter().get_cifid_seqid_loyalty(loyalty_number);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_id_presented(string idno)
        {
            DataSet ds = sqlkonek.getDataSet("SELECT CIFID FROM tbl_CIF_IDs WHERE IDNo = '" + idno + "'", false, new Object[] { });
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }
            return null;
        }

        // CHR 12/27/2012 - Preload lookups for id-types, to avoid blocking
        private void preload_idtypes()
        {
            if (Session["DS_IDTYPES"] == null || ((DataSet)Session["DS_IDTYPES"]).Tables.Count == 0 || ((DataSet)Session["DS_IDTYPES"]).Tables[0].Rows.Count == 0)
            {
                DataSet ds = sqlkonek.getDataSet("SELECT FLDVALUE, FLDDESCRIPTION FROM tbl_lookup WHERE FLDGROUP = 'IDTYPE'", false, new Object[] { });
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Session["DS_IDTYPES"] = ds;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_id_place_issued(string remitter_id)
        {
            irsClientData login_cred = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            if (login_cred != null && login_cred.Location != null)
            {
                DataTable dt = sqlkonek.getDataSet("otc_id_issuance", true, new object[] { login_cred.Location, remitter_id }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    string id_issueance_str = "Please update remitter id place of issuance. \n\n  Click Update Client ID link.\n\n";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        id_issueance_str += dt.Rows[i]["IDNO"].ToString() + " - " + dt.Rows[i]["FLDDESCRIPTION"].ToString() + "\n";
                    }

                    return id_issueance_str;
                }
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string chk_rcc_rem_declare(string gross_rem_amt, string remitter_id)
        {
            DataTable dt = null;
            DateTime user_date_time;

            string user_branch_code = null;

            try
            {
                user_branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                user_date_time = new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location);

                if (Convert.ToInt32(sqlkonek.getDataSet("select count(1) from tbl_branches where timezone like '%Canada%' and branchcode = '" + user_branch_code + "'", false, new Object[] { }).Tables[0].Rows[0][0]) <= 0)
                {
                    return null;
                }

                dt = sqlkonek.getDataSet("otc_rcc_declaration", true, new object[] { remitter_id, Convert.ToDecimal(gross_rem_amt), user_date_time }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["msg_out"].ToString() != null || dt.Rows[0]["msg_out"].ToString() != string.Empty)
                    {
                        return dt.Rows[0]["msg_out"].ToString();
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
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string chk_is_rcc_trans(string gross_rem_amt, string remitter_id)
        {
            int remitter_id_cnt = 0;
            string user_branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;

            DataTable dt1 = null;

            try
            {
                if (Convert.ToInt32(sqlkonek.getDataSet("select count(1) from tbl_branches where timezone like '%Canada%' and branchcode = '" + user_branch_code + "'", false, new Object[] { }).Tables[0].Rows[0][0]) < 1)
                {
                    return string.Empty;
                }

                string sql = "SELECT DISTINCT " +
                                "TBL_CIF_IDS.IDNO, " +
                                "TBL_SUB_LOOKUP.FLDCODE ID_DESC, " +
                                "CONVERT(VARCHAR, TBL_CIF_IDS.EXPIRYDATE,101) EXPIRYDATE, " +
                                "ISNULL(TBL_CIF_IDS.PlaceOfIssuance,'') " +
                            "FROM TBL_CIF_IDS " +
                            "LEFT JOIN TBL_SUB_LOOKUP ON TBL_SUB_LOOKUP.FLDVALUE = TBL_CIF_IDS.IDTYPE AND TBL_SUB_LOOKUP.FLDGROUP = 'IDTYPE' AND TBL_SUB_LOOKUP.BRANCHCODE = '" + user_branch_code + "' " +
                            "WHERE " +
                                "ISNULL(TBL_CIF_IDS.DELETED, 0) = 0 AND " +
                                "TBL_CIF_IDS.CIFID = '" + remitter_id + "'";

                dt1 = sqlkonek.getDataSet(sql, false, new Object[] { }).Tables[0];

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    remitter_id_cnt = dt1.Rows.Count;
                }

                dt1 = sqlkonek.getDataSet("select isnull(agentcheckidamount, 0) agentcheckidamount from tbl_branch_setup where branchcode = '" + user_branch_code + "'", false, new Object[] { }).Tables[0];

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(gross_rem_amt) >= Convert.ToDecimal(dt1.Rows[0]["agentcheckidamount"]) && remitter_id_cnt == 0)
                    {
                        return "Please present a valid ID for verification. Proceed ?";
                    }
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
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_remitter_ids_required(string remitter_id)
        {
            //String req_id_br = "\n";

            DataTable dt = null;

            DataSet ds = null;
            DataSet dsa = null;

            try
            {
                /* CHECK IF ABUK HI GRADE CIF. NO NEED TO CHECK REMITTER IDS IF FOUND. */
                dsa = sqlkonek.getDataSet("otc_abuk_no_ids", true, new object[] { remitter_id });

                if (dsa != null && dsa.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsa.Tables[0].Rows[0][0]) > 0)
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception ej)
            {
                return "0~Error during required-id validation. " + ej.Message;
            }
            finally
            {
                if (dsa != null)
                {
                    dsa.Dispose();
                    dsa = null;
                }
            }

            //DataSet ds1 = null;
            //DataSet ds2 = null;
            //DataSet ds3 = null;

            /*
                1	Required (All required IDs by branch must be presented by the client ? or the client can present at least 1 id ?)
                2	Any 1-Required
                3	Any 2-Required
                4	Optional
             */

            try
            {
                DateTime branch_local_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));

                string user_branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;

                //ds = sqlkonek.getDataSet("otc_req_ids", true, new Object[] { remitter_id });
                ds = sqlkonek.getDataSet("otc_req_ids", true, new Object[] { remitter_id, user_branch_code, branch_local_date });

                //ds1 = sqlkonek.getDataSet("SELECT requiredID FROM TBL_BRANCH_SETUP WHERE branchcode = '" + remitter_id.Substring(1, 3) + "'", false, new Object[] { });
                //ds2 = sqlkonek.getDataSet("otc_val_ids", true, new Object[] { remitter_id.Substring(1, 3) });
                //ds3 = sqlkonek.getDataSet("remitter_show_all_ids", true, new Object[] { remitter_id });

                //int id_cnt = Convert.ToInt32(ds1.Tables[0].Rows[0]["requiredID"]);

                //if (id_cnt == 4)
                //{
                //    return string.Empty;
                //}

                if (Convert.ToInt32(sqlkonek.getDataSet("select count(1) from tbl_branches where timezone like '%Canada%' and branchcode = '" + user_branch_code + "'", false, new Object[] { }).Tables[0].Rows[0][0]) > 0)
                {
                    return string.Empty; /* NEW OCTOBER 13 2014 DDC */
                }

                if (Convert.ToInt32(sqlkonek.getDataSet("SELECT COUNT(1) FROM TBL_CIF_IDS WHERE CIFID = '" + remitter_id + "'", false, new Object[] { }).Tables[0].Rows[0][0]) == 0)
                {
                    return "0~Remitter identification card not found.";
                }

                //if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                //{
                //    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                //    {
                //        req_id_br += ds2.Tables[0].Rows[i]["FldCode"].ToString() + "\n";
                //    }
                //}

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];

                    DateTime dtExpiry;
                    string expired_id_container = string.Empty;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dtExpiry = Convert.ToDateTime(dt.Rows[i]["EXPIRYDATE"].ToString());

                        if (dtExpiry <= Convert.ToDateTime(branch_local_date.ToString("MM/dd/yyyy")))
                        {
                            expired_id_container += "\n";
                            expired_id_container += "\nIdentification Card: " + dt.Rows[i]["FLDDESCRIPTION"].ToString().ToUpper();
                            expired_id_container += "\nIdentification Number: " + dt.Rows[i]["IDNO"].ToString();
                            expired_id_container += "\nExpiration Date: " + dt.Rows[i]["EXPIRYDATE"].ToString();
                            expired_id_container += "\n";
                        }
                    }

                    if (string.IsNullOrEmpty(expired_id_container) == false)
                    {
                        return "0~Remitter Identification Card expired. \n" + expired_id_container;
                    }
                }


                //if (id_cnt == 2 && ds2 != null && ds2.Tables[0].Rows.Count > 0 && ds3 != null && ds3.Tables[0].Rows.Count > 0)
                //{
                //    for (int x = 0; x < ds2.Tables[0].Rows.Count; x++)
                //    {
                //        for (int y = 0; y < ds3.Tables[0].Rows.Count; y++)
                //        {
                //            if (Convert.ToInt32(ds2.Tables[0].Rows[x]["FLDVALUE"]) == Convert.ToInt32(ds3.Tables[0].Rows[y]["FLDVALUE"]))
                //            {
                //                return string.Empty;
                //            }
                //        }
                //    }

                //    return "0~Remitter must have valid ID (Any 1 - Required ID) \n" + req_id_br;
                //}
                //else if (id_cnt == 3 && ds2 != null && ds2.Tables[0].Rows.Count > 0 && ds3 != null && ds3.Tables[0].Rows.Count > 0)
                //{
                //    int loc_cnt = 0;

                //    for (int x = 0; x < ds2.Tables[0].Rows.Count; x++)
                //    {
                //        for (int y = 0; y < ds3.Tables[0].Rows.Count; y++)
                //        {
                //            if (Convert.ToInt32(ds2.Tables[0].Rows[x]["FLDVALUE"]) == Convert.ToInt32(ds3.Tables[0].Rows[y]["FLDVALUE"]))
                //            {
                //                loc_cnt++;

                //                if (loc_cnt >= 2)
                //                {
                //                    return string.Empty;
                //                }
                //            }
                //        }
                //    }

                //    return "0~Remitter must have valid ID (Any 2 - Required ID) \n" + req_id_br;
                //}

                //if (id_cnt == 2 && ds3 != null && ds3.Tables[0].Rows.Count == 0)
                //{
                //    return "0~Remitter must have valid ID (Any 1 - Required ID) \n" + req_id_br; 
                //}
                //else if (id_cnt == 3 && ds3 != null && ds3.Tables[0].Rows.Count == 0)
                //{
                //    return "0~Remitter must have valid ID (Any 2 - Required ID)\n" + req_id_br;
                //}

                return string.Empty;

            }
            catch (Exception e)
            {
                return "0~Error during required-id validation. " + e.Message;
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

                //if (ds1 != null)
                //{
                //    ds1.Dispose();
                //    ds1 = null;
                //}

                //if (ds2 != null)
                //{
                //    ds2.Dispose();
                //    ds2 = null;
                //}

                //if (ds3 != null)
                //{
                //    ds3.Dispose();
                //    ds3 = null;
                //}
            }
        }


        //END CHR 12/27/2012

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_remitter_ids_required_ORIG(string remitter_id)
        {
            DataTable dt = null;
            DataSet dsids = null;
            short vcount = 0;
            short lc_gfc_count = 0; // RFS 13067-005


            if (Session["DS_IDTYPES"] != null && ((DataSet)Session["DS_IDTYPES"]).Tables.Count > 0 && ((DataSet)Session["DS_IDTYPES"]).Tables[0].Rows.Count > 0)
            {
                dsids = (DataSet)Session["DS_IDTYPES"];
            }

            string return_string = "1";

            try
            {
                DateTime branch_local_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));

                string s = "SELECT DISTINCT " +
                                "CIFID, " +
                                "IDTYPE, " +
                                "IDNO, " +
                                "CONVERT(VARCHAR, EXPIRYDATE,101) AS EXPIRYDATE " +
                            "FROM TBL_CIF_IDS " +
                            "WHERE ISNULL(Deleted, 0) = 0 AND CIFID = '" + remitter_id.Replace("'", "''") + "'";
                DataSet ds = sqlkonek.getDataSet(s, false, new Object[] { });

                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        string expired_id_container = string.Empty;
                        DateTime dtExpiry;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            short id_type = -1;
                            if (dt.Rows[i]["IDTYPE"] != DBNull.Value)
                            {
                                id_type = Convert.ToInt16(dt.Rows[i]["IDTYPE"]);
                            }
                            if (id_type == -1) continue;

                            String idno = "";
                            try
                            {
                                if (dt.Rows[i]["IDNO"] != DBNull.Value && dt.Rows[i]["IDNO"].ToString().Trim().Length > 0)
                                {
                                    idno = dt.Rows[i]["IDNO"].ToString();
                                }
                            }
                            catch { }
                            if (idno.Length == 0) continue;
                            String t = "";
                            try
                            {
                                DataRow[] idrow = dsids.Tables[0].Select("FLDVALUE = " + id_type);
                                if (idrow.Length > 0 && idrow[0]["FLDDESCRIPTION"] != DBNull.Value)
                                {
                                    t = idrow[0]["FLDDESCRIPTION"].ToString();
                                }
                            }
                            catch { }
                            // added by CHR 12/26/2012
                            // to exempt non expiring id like sss from checking
                            if (t.Length == 0) continue;
                            if (t.Length > 0 && t.IndexOf("SSS") > -1)
                            {
                                vcount++;
                                continue;
                            }
                            // end CHR 
                            dtExpiry = new DateTime(1900, 1, 1);
                            if (dt.Rows[i]["EXPIRYDATE"] != DBNull.Value && dt.Rows[i]["EXPIRYDATE"].ToString() != string.Empty)
                            {
                                dtExpiry = Convert.ToDateTime(dt.Rows[i]["EXPIRYDATE"].ToString());
                            }

                            if (dtExpiry <= Convert.ToDateTime(branch_local_date.ToString("MM/dd/yyyy")))
                            {
                                // RFS13067-005
                                if (t.ToUpper().IndexOf("LOYALTY CARD") > -1 | t.ToUpper().IndexOf("GLOBAL FILIPINO CARD") > -1)
                                {
                                    lc_gfc_count++;
                                }
                                //END RFS13067-005
                                expired_id_container += "\n";
                                expired_id_container += "\nIdentification Card: " + t;
                                expired_id_container += "\nIdentification Number: " + dt.Rows[i]["IDNO"].ToString();
                                expired_id_container += "\nExpiration Date: " + dtExpiry;
                                expired_id_container += "\n";
                            }
                            else
                            {
                                vcount++;
                            }
                        }

                        if (vcount > 0 & lc_gfc_count == 0) // RFS13067-005
                        {
                        }
                        else if (expired_id_container != null && expired_id_container.Trim().Length > 0)
                        {
                            return_string = "0~" + "Remitter identificaton card expired.\n" + expired_id_container;
                        }
                    }
                    else
                    {
                        return_string = "0~Remittance required identification card not found.";
                    }
                }
                else
                {
                    return_string = "0~Error Accessing the required-id table. " + sqlkonek.Message;
                }
            }
            catch (Exception e)
            {
                return_string = "0~Error during required-id validation. " + e.Message;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }

            return return_string;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_is_post_dated(string check_date)
        {
            this.logs("", "Remittance.RemOTC.check_is_post_dated", "Check date if postdated: " + check_date.ToString(), check_date.ToString(), "");

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));

                string s = "SELECT COUNT(*) AS holidays_count " +
                           "FROM TBL_HOLIDAYS " +
                           "WHERE " +
                                "ISNULL(DELETED, 0) = 0 AND " +
                                "ISNULL(HOLIDAY, 0) = 1 AND " +
                                "BRANCHCODE = '" + ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location + "' AND " +
                                "CONVERT(VARCHAR,HOL_DATE, 101) = '" + branch_current_date.ToString("MM/dd/yyyy") + "'";

                DataTable dt = sqlkonek.getDataSet(s, false, new object[] { }).Tables[0];

                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["holidays_count"]) > 0)
                        {
                            this.logs("", "Remittance.RemOTC.check_is_post_dated", "Check date is postdated!~" + check_date.ToString(), check_date.ToString(), "");
                            return true;
                        }
                    }
                }
                catch
                {
                    this.logs("", "Remittance.RemOTC.check_is_post_dated", "ERROR: Check date if postdated!~" + check_date.ToString(), check_date.ToString(), "");
                    return false;
                }
                finally
                {
                    if (dt != null)
                    {
                        dt = null;
                    }
                }

                if (Convert.ToDateTime(check_date) > branch_current_date)
                {
                    this.logs("", "Remittance.RemOTC.check_is_post_dated", "Check date is postdated!~" + check_date.ToString(), check_date.ToString(), "");
                    return true;
                }
            }
            this.logs("", "Remittance.RemOTC.check_is_post_dated", "Check date is not postdated!~" + check_date.ToString(), check_date.ToString(), "");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_account_bank_name(int account_seq_id)
        {
            sqlkonek.EnforceConstraint = false;
            return sqlkonek.getDataSet("sp_account_bank_name", true, new object[] { account_seq_id });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_instapay_prod_description()
        {
            sqlkonek.EnforceConstraint = false;
            return sqlkonek.getDataSet("get_instapay_prod_description", true, new object[] { });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bpo_bank_name(int branch_seq_id)
        {
            sqlkonek.EnforceConstraint = false;

            string s = "SELECT ISNULL(UPPER(BranchCode), '') + ' - ' + ISNULL(UPPER(branchname),'') AS branch_name, seq_id as branch_seq_id  FROM TBL_BRANCHES WHERE branchtype IN (2,3,5) AND seq_id = " + branch_seq_id.ToString();

            return sqlkonek.getDataSet(s, false, new object[] { });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_transaction_last_setup(string remitter_id, string beneficiary_id)
        {
            //string s = string.Empty;

            //s = "SELECT DISTINCT " +
            //        " ISNULL(A.transactionsource, 0)   AS transactionsource, " +
            //        " ISNULL(A.transactionpurpose, 0)  AS transactionpurpose, " +
            //        " ISNULL(A.receiptmode, 0)         AS receiptmode, " +
            //        " ISNULL(A.sourceoffunds, 0)       AS sourceoffunds, " +
            //        " ISNULL(B.chargetype, 0)          AS chargetype " +
            //    " FROM tbl_remittance A " +
            //    " INNER JOIN tbl_remittance_charges_data B ON B.txn_seq_id = A.seq_id " +
            //    " WHERE A.seq_id = (SELECT MAX(seq_id) FROM TBL_REMITTANCE WHERE remittercifid = '" + remitter_id.Replace("'", "''") + "' AND beneficiarycifid = '" + beneficiary_id.Replace("''", "''") + "')";

            //return sqlkonek.getDataSet(s, false, new object[] { });

            DataSet ds = new DataSet();
            irsSqlCommons commons = new irsSqlCommons();
            irsClientData irs_client_trans = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                //============================================
                //RFC-20324 : RCC Aggregation requirement
                //ds = commons.execStoredProcedure("proc_load_transaction_last_setup", new object[] { remitter_id, beneficiary_id });
                ds = commons.execStoredProcedure("proc_load_transaction_last_setup", new object[] { remitter_id, beneficiary_id, irs_client_trans.Location });
                //============================================
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(irs_client_trans.UserId, "OTC", ex.Message.ToString(), "", "", csAuditLog.Category.Select);
            }
            finally
            {
                commons = null;
                irs_client_trans = null;
            }
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool location_is_serviceable(string beneficiary_id)
        {
            DataTable dt = null;

            string s = string.Empty;

            try
            {
                s = "select count(1) as got_area from tbl_cif_address " +
                    "where isnull(deleted,0) = 0 and " +
                    "isnull(preferredmailadd,0) = 1 and " +
                    "isnull(zipcodeseqid,0) > 0 and " +
                    "exists (select 1 from tbl_osa where isnull(osa_ind, 0) = 0 and zip_seq_id = zipcodeseqid ) and " +
                    "cifid = '" + beneficiary_id.Replace("'", "''") + "'";

                dt = sqlkonek.getDataSet(s, false, new object[] { }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["got_area"]) > 0)
                    {
                        return true;
                    }
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remitter_benef_trans_setup(string remitter_id, string beneficiary_id, int product_code, int implementation_code)
        {
            //string s = string.Empty;

            //s = "SELECT " +
            //    "ISNULL(A.accountseqid, 0) AS account_seqid, " +
            //    "ISNULL(A.currency, 0)     AS destination_currency, " +
            //    "ISNULL(B.currency, 0)     AS account_currency, " +
            //    "ISNULL(C.chargetype, 0)   AS charge_type, " +
            //    "ISNULL(D.seq_id, 0)       AS destination_branch_seq, " +
            //    "ISNULL(A.accounttype, 0)  AS account_type " +
            //"FROM TBL_CIF_BENEFICIARIES A " +
            //"LEFT JOIN TBL_CIF_ACCOUNTS  B ON B.seq_id = A.accountseqid " +
            //"LEFT JOIN TBL_CIF_REMITTERS C ON C.cifid  = A.remittercifid " +
            //"LEFT JOIN TBL_BRANCHES      E ON E.seq_id = B.OtherBankSeqId " +
            //"LEFT JOIN TBL_BRANCHES      D ON D.branchcode = ISNULL(ISNULL(NULLIF(A.branchcode,''), ISNULL(NULLIF(B.BranchCode,''), B.DomBankCode)), E.BranchCode) " +
            //"WHERE " +
            //"    A.productcode        = " + product_code.ToString() + " AND " +
            //"    A.implementationcode = " + implementation_code.ToString() + " AND " +
            //"    A.remittercifid      = '" + remitter_id.Replace("'", "''") + "' AND " +
            //"    A.cifid              = '" + beneficiary_id.Replace("'", "''") + "'";

            //return sqlkonek.getDataSet(s, false, new object[] { });
            DataSet ds = new DataSet();
            irsSqlCommons commons = new irsSqlCommons();
            irsClientData irs_client_trans = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                ds = commons.execStoredProcedure("proc_get_remitter_benef_trans_setup", new object[] { remitter_id, beneficiary_id, product_code, implementation_code });
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(irs_client_trans.UserId, "OTC", ex.Message.ToString(), "", "", csAuditLog.Category.Select);
            }
            finally
            {
                commons = null;
                irs_client_trans = null;
            }
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool pnb_account_ok(string account_no)
        {
            irsAccountAlgo algo = null;
            try
            {
                algo = new irsAccountAlgo();
                bool ret_algo = algo.checkAccount(account_no);
                this.acct_algo = algo.AlgoType;
                return ret_algo;
            }
            catch
            {
                return false;
            }
            finally
            {
                algo = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool got_merchant_account(string account_no)
        {
            //if (this.pnb_account_ok(account_no) == true)
            //{
            //    return new irsOBPSTransactions().fOBPAccount(account_no);
            //}
            //return false;

            return new irsOBPSTransactions().fOBPAccount(account_no);
        }

        /* E2EAPICHANGE 20220209 Removed pnb_account_dormant */
        
        /* BOYET MARCH 20 2017 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_acct_stat_pnb_allied(string account_no, string beneficiary_id)
        {
            irsAccountInquiry inquire = null;
            string message = string.Empty;
            string fc_abc = string.Empty;

            try
            {
                /* FOR APPROVAL TAG */
                bool forApprovalTag = false;
                forApprovalTag = new irsAccountInquiry().isForApprovalTag();

                if (!forApprovalTag)
                {
                    inquire = new irsAccountInquiry();
                    /* FOR CBIP */
                    string[] arr;
                    message = inquire.getAccountStatus(account_no);
                    arr = message.Split('|');
                    if (arr.Length < 7)
                    {
                        /* FOR CBIP 20170825 */
                        int i = 0;
                        bool b = false;
                        DataTable dt = new irsAccountInquiry().BlackOutMode();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (i = 0; i <= (dt.Rows.Count - 1); i++)
                            {
                                if (arr[2].IndexOf(dt.Rows[i]["FldDescription"].ToString().Trim()) >= 0)
                                {
                                    b = true;
                                }
                            }
                        }
                        if (!b)
                        {
                            return "Exception:|" + arr[2];
                        }
                        else
                        {
                            return "|||";
                        }
                    }
                    string acctType = arr[6].ToString();
                    string acctStatus = string.Empty;

                    if (acctType == "2")
                    {
                        acctStatus = new irsInternetPage().getLookupValue("AccountStatusIM", "FldCode", arr[0].ToString(), "FldValue");
                        if (acctStatus != "1" && acctStatus != "10")
                        {
                            return "Exception:|" + this.get_account_status_UserMessage(account_no, acctStatus, acctType);
                        }
                    }
                    else
                    {
                        acctStatus = new irsInternetPage().getLookupValue("AccountStatusST", "FldCode", arr[0].ToString(), "FldValue");
                        if (acctStatus != "1")
                        {
                            return "Exception:|" + this.get_account_status_UserMessage(account_no, acctStatus, acctType);
                        }
                    }
                    return "|||" + arr[3].ToString();
                }
                else
                {
                    this.logs("", "OTC Account Validation", "ForApprovalTag enabled for this branch", "get_acct_stat_pnb_allied", "");
                    return "|||";
                }
            }
            catch (Exception e)
            {
                return "Exception:|" + e.Message;
            }
            finally
            {
                inquire = null;
            }
        }
        
        /* E2EAPICHANGE 20220209 Removed get_account_status */

        private void logs(string userid, string module, string description, string newvalue, string oldvalue)
        {
            irsClientData login_cred = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            if (login_cred != null && login_cred.UserId != string.Empty && userid == string.Empty)
            {
                userid = login_cred.UserId;
            }

            this.logger.writeAudit(userid, module, description, newvalue, oldvalue, csAuditLog.Category.Insert);
        }

        private string get_account_status_UserMessage(string accountNo, string accountStatus, string accountType)
        {
            string s = string.Empty;
            DataSet ds = null;

            /* FOR CBIP */
            if (accountType == "2")
            {
                s = "select FldDescription from tbl_lookup where fldgroup = 'FCStatusUserMessageIM' and fldcode = '" + Convert.ToString(accountStatus) + "'";
            }
            else
            {
                s = "select FldDescription from tbl_lookup where fldgroup = 'FCStatusUserMessageST' and fldcode = '" + Convert.ToString(accountStatus) + "'";
            }

            try
            {
                ds = sqlkonek.getDataSet(sqlkonek.getSqlCommand(s, false), "TBL_LOOKUP");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0]["FldDescription"].ToString();
                    }
                }
            }
            catch
            {
                return "Unable to retrieve account status.";
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }

            /* FOR CBIP */
            if (accountStatus == "0")
            {
                return "Invalid Account Number.";
            }

            return string.Empty;
        }

        private bool get_account_status_if_OTC_blocked(string accountNo, string accountStatus, string accountType)
        {
            String s = String.Empty;
            DataSet ds = null;

            /* FOR CBIP */
            if (accountType == "2")
            {
                s = "select FldDescription from tbl_lookup where fldgroup = 'FCStatusBlockedinOTCIM' and fldcode = '" + Convert.ToString(accountStatus) + "'";
            }
            else
            {
                s = "select FldDescription from tbl_lookup where fldgroup = 'FCStatusBlockedinOTCST' and fldcode = '" + Convert.ToString(accountStatus) + "'";
            }

            try
            {
                ds = sqlkonek.getDataSet(sqlkonek.getSqlCommand(s, false), "TBL_LOOKUP");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch { }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
            return false;
        }
        /*
         * added by :   neil
         * date     :   jan 2012
         * ends here
         */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool current_irsworld_session_expired()
        {
            irsClientData irs_client = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                if (irs_client != null && irs_client.UserId != null && irs_client.UserId.Trim().Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (irs_client != null)
                {
                    irs_client = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_gfc_account_seq_id(string account_no, string beneficiary_id)
        {
            string s = "SELECT SEQ_ID FROM TBL_CIF_ACCOUNTS WHERE CIFID = '" + beneficiary_id.Replace("'", "''") + "' AND ACCOUNTNO = '" + account_no.Replace("'", "''") + "'";

            DataTable dt = sqlkonek.getDataSet(sqlkonek.getSqlCommand(s, false), "GFC_ACCOUNTS").Tables[0];

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["SEQ_ID"]);
                }

                return 0;
            }
            catch
            {
                return 0;
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
        public bool beneficiary_has_gfc(string beneficiary_id)
        {
            string s = "SELECT COUNT(*) AS gfc_count " +
                       "FROM TBL_CIF_ACCOUNTS " +
                       "WHERE ISNULL(isgfc, 0) > 0 AND CIFID = '" + beneficiary_id.Replace("'", "''") + "'";

            DataTable dt = sqlkonek.getDataSet(sqlkonek.getSqlCommand(s, false), "GFC_ACCOUNTS").Tables[0];

            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["gfc_count"]) > 0)
                    {
                        return true;
                    }
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


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int beneficiary_in_door_area(string beneficiary_id)
        {
            string s = string.Empty;

            s = "SELECT COUNT(*) AS count_door " +
                "FROM TBL_CIF_ADDRESS A " +
                "LEFT JOIN TBL_OSA B ON B.zip_seq_id = A.zipcodeseqid " +
                "WHERE " +
                "   ISNULL(A.Deleted, 0) = 0 AND " +
                "   ISNULL(A.PreferredMailAdd, 0) > 0 AND " +
                "   ISNULL(A.zipcodeseqid, 0) > 0 AND " +
                "   ISNULL(B.osa_ind, 0) = 0 AND " +
                "   ISNULL(B.zip_seq_id, 0) > 0 AND " +
                "   CIFID = '" + beneficiary_id.Replace("'", "''") + "'";

            try
            {
                if (Convert.ToInt32(sqlkonek.getDataSet(sqlkonek.getSqlCommand(s, false)).Tables[0].Rows[0]["count_door"]) > 0)
                {
                    return 1;
                }
            }
            catch
            {
                return 0;
            }

            return 0;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_client_record(string remitter_id, string beneficiary_id)
        {
            /* VAPT IAG 20191007 */
            string RequestTime = System.DateTime.Now.ToString();
            /*********************/

            try
            {
                //======================================================================================
                /* RFC 992 - PESONET */
                sqlkonek.EnforceConstraint = false;
                return sqlkonek.getDataSet("proc_Get_Client_Record_OTC", true, new object[] { remitter_id, beneficiary_id });
                //======================================================================================
            }
            catch
            {
                return null;
            }
            finally
            {
                /* VAPT IAG 20191007 */
                new csAuditLog().writeVAPTLogger("", "OTC", "GET CIF RECORDS", remitter_id, beneficiary_id, "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rates_is_ok()
        {
            string[] s = this.get_user_branchcode().Split('~');

            if (Convert.ToString(s[0]) == "1170")
            {
                return new irsCurrencyRates().isComplete(Convert.ToString(s[0]), this.get_transaction_rate_date(), grsbatchentry().p_ratetype);
            }
            else
            {
                return new irsCurrencyRates().isComplete(Convert.ToString(s[0]), this.get_transaction_rate_date());
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string send_otc_receipt(int transaction_id, string reference_no, string CIFID)
        {
            CrystalReportUtil cru = new CrystalReportUtil();
            ReportDocument rptReceiptRemit = new ReportDocument();

            string msg_return = string.Empty;

            try
            {
                rptReceiptRemit = cru.generate_otc_report(((irsClientData)Session["IRS_CLIENT_DATA"]).Location, transaction_id.ToString(), out msg_return);

                if (rptReceiptRemit == null)
                {
                    return "Unable to send receipt.\n\n Conversion of report to PDF format failed.\n\n" + msg_return;
                }

                if (cru.OREmailSending(rptReceiptRemit, reference_no, CIFID, out msg_return) == true)
                {
                    msg_return = string.Empty;
                }
                else if (msg_return != null && msg_return != string.Empty && msg_return.Trim().Length > 1) /* CBIP BOYET 10062017*/
                {
                    return msg_return; /* CBIP BOYET 10062017*/
                }

                if (msg_return.Trim().ToUpper() == "FAILED TO SEND O.R. THRU EMAIL.")
                {
                    msg_return = "Failed to send O.R thru email.";
                }
            }
            catch (Exception ex)
            {
                msg_return = ex.Message;
            }
            finally
            {
                if (cru != null)
                {
                    cru = null;
                }

                if (rptReceiptRemit != null)
                {
                    /* CBIP BOYET 10062017*/
                    rptReceiptRemit.Close();
                    rptReceiptRemit.Dispose();
                    rptReceiptRemit = null;
                    /* CBIP BOYET 10062017*/
                }
            }
            /* FOR CBIP */
            new csAuditLog().writeAudit("", "OTC EMAIL", "TransactionID=[" + transaction_id + "] ReferenceNo=[" + reference_no + "] CIFID=[" + CIFID + "]", "ReturnValue=[" + msg_return + "]", "send_otc_receipt", csAuditLog.Category.Select);
            return msg_return.ToLower();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_otc_rate_caption(int source_currency, int target_currency)
        {
            string s = string.Empty;
            string branch_code = string.Empty;

            if (Session["IRS_CLIENT_DATA"] != null)
            {
                branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            }

            if (branch_code.Trim() == "1170")
            {
                s = "SELECT TOP 1 UPPER(ISNULL(B.FldDescription, '') + ' TO ' +  ISNULL(C.FldDescription, '')) + ' = ' + CAST(A.SourceToTarget AS VARCHAR) AS captions " +
                    " FROM TBL_CURRENCY_RATES A " +
                    " LEFT JOIN TBL_LOOKUP B ON  B.FldValue = A.SourceCurrency AND B.FldGroup = 'CURRENCY' " +
                    " LEFT JOIN TBL_LOOKUP C ON  C.FldValue = A.TargetCurrency AND C.FldGroup = 'CURRENCY' " +
                    " WHERE A.SourceCurrency = " + source_currency.ToString() +
                    " AND A.TargetCurrency   = " + target_currency.ToString() +
                    " AND A.branchcode       = '" + branch_code + "' " +
                    " AND A.CurrencyType = '" + grsbatchentry().p_ratetype + "' " +
                    " GROUP BY A.DateInserted, A.TranxDate, B.FldDescription, C.FldDescription, A.SourceToTarget " +
                    " ORDER BY A.DateInserted DESC";
            }
            else
            {
                s = "SELECT TOP 1 UPPER(ISNULL(B.FldDescription, '') + ' TO ' +  ISNULL(C.FldDescription, '')) + ' = ' + CAST(A.SourceToTarget AS VARCHAR) AS captions " +
                    " FROM TBL_CURRENCY_RATES A " +
                    " LEFT JOIN TBL_LOOKUP B ON  B.FldValue = A.SourceCurrency AND B.FldGroup = 'CURRENCY' " +
                    " LEFT JOIN TBL_LOOKUP C ON  C.FldValue = A.TargetCurrency AND C.FldGroup = 'CURRENCY' " +
                    " WHERE A.SourceCurrency = " + source_currency.ToString() +
                    " AND A.TargetCurrency   = " + target_currency.ToString() +
                    " AND A.branchcode       = '" + branch_code + "' " +
                    " GROUP BY A.DateInserted, A.TranxDate, B.FldDescription, C.FldDescription, A.SourceToTarget " +
                    " ORDER BY A.DateInserted DESC";
            }

            DataSet ds = null;

            try
            {
                ds = sqlkonek.getDataSet(s, false, new object[] { });

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return "REMITTANCE INFORMATION - " + ds.Tables[0].Rows[0]["captions"].ToString();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }

            return string.Empty;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int otc_user(int product_charges_id)
        {
            int i = 0; irsClientData loggin_data = null;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                string s = "SELECT COUNT(*) access_count FROM TBL_CHARGES_USERS WHERE UserType = 1 AND ProductChargesID = " + product_charges_id.ToString() + " AND Role = " + loggin_data.Role.ToString();

                DataTable dt = sqlkonek.getDataSet(s, false, new object[] { }).Tables[0];

                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["access_count"]) > 0)
                        {
                            i = 1;
                        }
                    }
                }
                catch
                {
                    return 0;
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

            return i;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_lookup_data(string fldgroup, int field_value)
        {
            string s = "SELECT SEQ_ID AS sequence_id, UPPER(FldCode) AS field_code ,FldValue AS field_value, UPPER(FldDescription) AS field_description  FROM TBL_LOOKUP WHERE FldGroup ='" + fldgroup.Replace("'", "''") + "' AND FldValue = " + field_value.ToString();
            return sqlkonek.getDataSet(s, false, new object[] { });
        }

        /* E2EAPICHANGE 20220209 Removed credit_to_flexcube */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_branch_defaults()
        {
            string[] array_branch_code = this.get_user_branchcode().Split('~');

            if (array_branch_code != null && array_branch_code.Length > 0)
            {
                try
                {
                    sqlkonek.EnforceConstraint = false;
                    return sqlkonek.getDataSet("spGetBranchDefaultSetup", true, new object[] { array_branch_code[0] });
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_user_branchcode()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                if (loggin_data != null)
                {
                    return loggin_data.Location + "~" + new irsInternetPage().getTransactionDate(loggin_data.Location, true).ToString("MM/dd/yyyy");
                }
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_product_setup_description(int product_code, int implementation_type, int payment_type, int currency, int charge_type, int computation_direction, int receipt_mode, int purpose, int transaction_source, int source_of_fund)
        {
            string[] array_branch_code = this.get_user_branchcode().Split('~');
            string branch_code = string.Empty;

            if (array_branch_code != null && array_branch_code.Length > 0)
            {
                branch_code = array_branch_code[0];
            }

            try
            {
                sqlkonek.EnforceConstraint = false;

                return sqlkonek.getDataSet("sp_otc_rem_product_setup", true,
                            new object[]
                                            {
                                                "DISPLAY_DESC",
                                                branch_code.Replace("'", "''"),
                                                product_code,
                                                implementation_type,
                                                payment_type,
                                                currency,
                                                charge_type,
                                                computation_direction,
                                                receipt_mode,
                                                purpose,
                                                transaction_source,
                                                source_of_fund,
                                                string.Empty
                                            }
                         );
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_product_setup_code(string branch_code, int product_code, int implementation_type, int payment_type, int currency, int charge_type)
        {
            try
            {
                sqlkonek.EnforceConstraint = false;

                return sqlkonek.getDataSet("sp_otc_rem_product_setup", true,
                            new object[]
                                            {
                                                "PRODUCTGROUPING",
                                                branch_code.Replace("'", "''"),
                                                product_code,
                                                implementation_type,
                                                payment_type,
                                                currency,
                                                charge_type,
                                                0,
                                                0,
                                                0,
                                                0,
                                                0,
                                                string.Empty
                                            }
                        );
            }
            catch
            {
                return null;
            }
            finally
            {
                this.clear_otc_session(0);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_compute_mode_source(string branch_code, int product_code, int implementation_type, int payment_type, int currency, int charge_type)
        {
            try
            {
                sqlkonek.EnforceConstraint = false;

                return sqlkonek.getDataSet("sp_otc_rem_product_setup", true,
                                new object[]
                                                    {
                                                        "COMPUTETYPE",
                                                        branch_code.Replace("'", "''"),
                                                        product_code,
                                                        implementation_type,
                                                        payment_type,
                                                        currency,
                                                        charge_type,
                                                        0,
                                                        0,
                                                        0,
                                                        0,
                                                        0,
                                                        string.Empty
                                                    }
                        );
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_charge_count(string branch_code, int product_code, int implementation_type, int payment_type, int currency, int charge_type)
        {
            try
            {
                sqlkonek.EnforceConstraint = false;


                return Convert.ToInt32(sqlkonek.getDataSet("sp_otc_rem_product_setup", true,
                             new object[]
                                                    {
                                                        "CSETUPCOUNT",
                                                        branch_code.Replace("'", "''"),
                                                        product_code,
                                                        implementation_type,
                                                        payment_type,
                                                        currency,
                                                        charge_type,
                                                        0,
                                                        0,
                                                        0,
                                                        0,
                                                        0,
                                                        string.Empty
                                                    }
                     ).Tables[0].Rows[0][0]);
            }
            catch
            {
                return 0;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string compute_override(int product_charge_id, int account_currency, decimal remit_amount)
        {
            irsCalculator calc = null;
            int computation_direction = 3;

            try
            {
                if (System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] != null)
                {
                    try
                    {
                        computation_direction = (int)System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"];
                    }
                    catch
                    {
                        System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] = null;
                    }
                }

                if (System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS"] != null)
                {
                    calc = (irsCalculator)System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS"];
                }
                else
                {
                    calc = new irsCalculator(product_charge_id, account_currency, false, computation_direction);
                }

                calc.recompute_irsw_charges(remit_amount);

                Session["REMITTANCE_CALCULATOR_CLASS"] = calc;
                Session["REMITTANCE_CALCULATOR_CLASS_REM_SAVING"] = calc;

                return calc.get_remittance_net_to_bene_amt().ToString("#,###.00") + "~" +
                        calc.get_remittance_total_charges_amt().ToString("#,###.00") + "~" +
                        calc.get_remittance_net_to_bene_amt_php_equivalent().ToString("#,###.00") + "~" +
                        calc.get_beneficiary_account_currency() + "~" +
                        calc.get_branch_local_currency() + "~" +
                        calc.get_remittance_gross_branch().ToString("#,###.00") + "~" +
                        calc.get_remittance_net_branch() + "~" +
                        calc.get_remittance_net_to_bene_amt() + "~" +
                        calc.get_remittance_total_charges_amt() + "~" +
                        calc.get_forex_target_to_account() + "~" +
                        calc.get_forex_target_to_source() + "~" +
                        calc.get_forex_target_to_usd() + "~" +
                        calc.get_forex_target_to_php() + "~" +
                        calc.get_forex_target_to_eur() + "~" +
                        calc.get_forex_source_to_target() + "~" +
                        calc.get_forex_source_to_usd() + "~" +
                        calc.get_forex_source_to_php() + "~" +
                        calc.get_total_backend_charges();
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (calc != null)
                {
                    calc = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string compute_remittance(int product_charge_id, int account_currency, int charge_locally, decimal remit_amount)
        {
            irsCalculator calc = null;
            irsOverrideLog irs = null;

            int computation_direction = 3;

            try
            {
                if (Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] != null)
                {
                    try
                    {
                        computation_direction = (int)Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"];
                    }
                    catch
                    {
                        Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] = null;
                    }
                }

                calc = new irsCalculator(product_charge_id, account_currency, Convert.ToBoolean(charge_locally), computation_direction);
                calc.compute_irsw_charges(remit_amount);

                /* ADDED BY BOYET 03/12/2014 TO SET HITLIST VALUE TO SESSION VARIABLE  */

                if (Session["IRS_OVERRIDE_LOGDATA"] != null || (string)Session["IRS_OVERRIDE_LOGDATA"] != string.Empty)
                {
                    string hitlist_cnt = (string)Session["IRS_OVERRIDE_LOGDATA"];
                    string[] hitlist_arr = null;

                    if (hitlist_cnt != null && hitlist_cnt != string.Empty && hitlist_cnt.Length > 0)
                    {
                        hitlist_arr = hitlist_cnt.Split('~');
                    }

                    if (hitlist_arr != null && hitlist_arr.Length > 0)
                    {
                        for (int i = 0; i < hitlist_arr.Length; i++)
                        {
                            /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
                            if (hitlist_arr[i].IndexOf("50") > 0)
                            {
                                /* ADDED BY BOYET APRIL 28 2016 */

                                if (System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] != null)
                                {
                                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"].ToString()).Equals(true))
                                    {
                                        Session["OTC_HIT_LIST_VAL"] = hitlist_arr[i] + "~"; break;
                                    }
                                }
                            }/* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
                            if (hitlist_arr[i].IndexOf("25") > 0)
                            {
                                /* ADDED BY BOYET APRIL 28 2016 */

                                if (System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] != null)
                                {
                                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"].ToString()).Equals(true))
                                    {
                                        Session["OTC_HIT_LIST_VAL"] = hitlist_arr[i] + "~"; break;
                                    }
                                }
                                //ORIGINAL Session["OTC_HIT_LIST_VAL"] = hitlist_arr[i] + "~"; break;
                            }
                        }
                    }
                }
                /* END 03/12/2014*/

                irs = new irsOverrideLog("", "");
                irs.ResetLogSession();

                Session["REMITTANCE_CALCULATOR_CLASS"] = calc;
                Session["REMITTANCE_CALCULATOR_CLASS_REM_SAVING"] = calc;

                /* ADDED BY DDC APRIL 28 2016 */

                try
                {
                    if (System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] != null)
                    {
                        if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"].ToString()).Equals(false))
                        {
                            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"];
                        }
                    }
                }
                catch
                {
                }
                /* END */

                return calc.get_remittance_net_to_bene_amt().ToString("#,###.00") + "~" +
                        calc.get_remittance_total_charges_amt().ToString("#,###.00") + "~" +
                        calc.get_remittance_net_to_bene_amt_php_equivalent().ToString("#,###.00") + "~" +
                        calc.get_beneficiary_account_currency() + "~" +
                        calc.get_branch_local_currency() + "~" +
                        calc.get_remittance_gross_branch().ToString("#,###.00") + "~" +
                        calc.get_remittance_net_branch() + "~" +
                        calc.get_remittance_net_to_bene_amt() + "~" +
                        calc.get_remittance_total_charges_amt() + "~" +
                        calc.get_forex_target_to_account() + "~" +
                        calc.get_forex_target_to_source() + "~" +
                        calc.get_forex_target_to_usd() + "~" +
                        calc.get_forex_target_to_php() + "~" +
                        calc.get_forex_target_to_eur() + "~" +
                        calc.get_forex_source_to_target() + "~" +
                        calc.get_forex_source_to_usd() + "~" +
                        calc.get_forex_source_to_php() + "~" +
                        calc.get_total_backend_charges();

            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (calc != null)
                {
                    calc = null;
                }

                if (irs != null)
                {
                    irs = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_breakdown()
        {
            if (Session["REMITTANCE_CALCULATOR_CLASS"] != null)
            {
                irsCalculator calc = null;

                try
                {
                    calc = (irsCalculator)Session["REMITTANCE_CALCULATOR_CLASS"]; return calc.get_transaction_charge_screen();
                }
                catch
                {
                    return "<center><b>REMITTANCE CALCULATOR IS NOT PROPERLY INITIALIZED. MAKE SURE THAT THE AMOUNT ENTERED AND CHARGE TYPE ARE VALID.</b></center>";
                }
                finally
                {
                    if (calc != null)
                    {
                        calc = null;
                    }
                }
            }

            return "<center><b>REMITTANCE CALCULATOR IS NOT PROPERLY INITIALIZED. MAKE SURE THAT THE AMOUNT ENTERED AND CHARGE TYPE ARE VALID.</b></center>";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_charge_id(string branch_code, int product_code, int implementation_type, int payment_type, int currency, int charge_type, int compute_mode)
        {
            try
            {
                sqlkonek.EnforceConstraint = false;

                return sqlkonek.getDataSet("sp_otc_rem_product_setup", true,
                                                new object[]
                                                        {
                                                            "CHARGESEQUENCE",
                                                            branch_code.Replace("'", "''"),
                                                            product_code,
                                                            implementation_type,
                                                            payment_type,
                                                            currency,
                                                            charge_type,
                                                            compute_mode,
                                                            0,
                                                            0,
                                                            0,
                                                            0,
                                                            string.Empty
                                                        }
                                        );
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string remitter_has_benef(string remitter_id)
        {
            sqlkonek.EnforceConstraint = false;

            if (Convert.ToInt32(sqlkonek.getDataSet("SELECT COUNT(*) AS count_benef FROM TBL_CIF_BENEFICIARIES WHERE RemitterCIFID = '" + remitter_id.Replace("'", "''") + "'", false, new object[] { }).Tables[0].Rows[0][0]) > 0)
            {
                return remitter_id;
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_overseas_datetime()
        {
            return new Internet.InternetPage().getLocalTime();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_ho_datetime()
        {
            return new Internet.InternetPage().getServerTime();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_remitter_phone_enrollment(string CIFID) //Added by Troy 20120808
        {
            DataTable dt = new DataTable();
            DataTable dtCIF = new DataTable();
            bool bPhoneEnrollment = false;
            string sReturn = string.Empty;

            try
            {
                dt = new irsInternetPage().getDefaults(new Internet.InternetPage().getLocation());
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["PhoneEnrollment"] != DBNull.Value)
                    {
                        bPhoneEnrollment = (dt.Rows[0]["PhoneEnrollment"].ToString().Trim() == "1") ? true : false;
                    }
                }

                if (bPhoneEnrollment == false)
                {
                    sReturn = "true";
                }
                else
                {
                    dtCIF = new irsInternetPage().getCIFInfo(CIFID);
                    if (bPhoneEnrollment == true)
                    {
                        if (dtCIF != null && dtCIF.Rows.Count > 0)
                        {
                            if (dtCIF.Rows[0]["isPhoneRemitter"] != DBNull.Value && dtCIF.Rows[0]["isPhoneRemitter"].Equals(true))
                            {
                                sReturn = "true";
                            }
                            else
                            {
                                sReturn = "false";
                            }
                        }
                    }
                    else
                    {
                        sReturn = "true";
                    }
                    new csAuditLog().writeAudit("", "OTC", sReturn, "check_remitter_phone_enrollment", CIFID, csAuditLog.Category.Select);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "OTC", ex.Message, "check_remitter_phone_enrollment", CIFID, csAuditLog.Category.Select);
                sReturn = "true";
            }
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_remitter_web_enrollment(string CIFID) //Added by Troy 20120903
        {
            DataTable dt = new DataTable();
            DataTable dtCIF = new DataTable();
            bool bWebEnrollment = false;
            string sReturn = string.Empty;

            try
            {
                dt = new irsInternetPage().getDefaults(new Internet.InternetPage().getLocation());
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["WebEnrollment"] != DBNull.Value)
                    {
                        bWebEnrollment = (dt.Rows[0]["WebEnrollment"].ToString().Trim() == "1") ? true : false;
                    }
                }

                if (bWebEnrollment == false)
                {
                    sReturn = "true";
                }
                else
                {
                    dtCIF = new irsInternetPage().getCIFInfo(CIFID);
                    if (bWebEnrollment == true)
                    {
                        if (dtCIF != null && dtCIF.Rows.Count > 0)
                        {
                            if (dtCIF.Rows[0]["isWebRemitter"] != DBNull.Value && dtCIF.Rows[0]["isWebRemitter"].Equals(true))
                            {
                                sReturn = "true";
                            }
                            else
                            {
                                sReturn = "false";
                            }
                        }
                    }
                    else
                    {
                        sReturn = "true";
                    }
                    new csAuditLog().writeAudit("", "OTC", sReturn, "check_remitter_web_enrollment", CIFID, csAuditLog.Category.Select);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "OTC", ex.Message, "check_remitter_web_enrollment", CIFID, csAuditLog.Category.Select);
                sReturn = "true";
            }
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet set_remit_cls(int transaction_id)
        {
            int implem_code = 0;
            int product_code = 0;
            int default_status = 0;

            DataSet ds = null;

            try
            {
                ds = sqlkonek.getDataSet("select * from tbl_charge_master where seq_id = (select productchargesid from tbl_remittance where seq_id = " + transaction_id.ToString() + ")", false, new object[] { });

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    implem_code = Convert.ToInt32(ds.Tables[0].Rows[0]["ImplementationType"]);
                    product_code = Convert.ToInt32(ds.Tables[0].Rows[0]["ProductCode"]);
                    default_status = Convert.ToInt32(ds.Tables[0].Rows[0]["DefaultStatus"]);

                    ds.Dispose();
                    ds = null;
                }

                if (implem_code == 2 || product_code == 10 || product_code == 24 || product_code == 27)
                {
                    return null;
                }

                if (default_status == 14 || default_status == 15)
                {
                    return null;
                }

                ds = sqlkonek.getDataSet("otc_remote_ovr_chk_pending", true, new object[] { transaction_id });

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["cnt_pending"]) > 0)
                    {
                        return null;
                    }
                }

                if (transaction_id > 0)
                {
                    return new irsRemTeller().Remit2(transaction_id, true);
                }

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_otc_preferred_tag(string remitter_id, string beneficiary_id, int remitter_add_sequence_id, int remitter_con_sequence_id, int beneficiary_add_sequence_id, int beneficiary_con_sequence_id)
        {
            SqlCommand command = null;

            try
            {
                command = sqlkonek.getSqlCommand("sp_otc_update_preferred_tag");

                command.Parameters.Add("@remitter_id", SqlDbType.VarChar).Value = remitter_id;
                command.Parameters.Add("@beneficiary_id", SqlDbType.VarChar).Value = beneficiary_id;
                command.Parameters.Add("@remitter_add_sequence_id", SqlDbType.BigInt).Value = remitter_add_sequence_id;
                command.Parameters.Add("@remitter_con_sequence_id", SqlDbType.BigInt).Value = remitter_con_sequence_id;
                command.Parameters.Add("@beneficiary_add_sequence_id", SqlDbType.BigInt).Value = beneficiary_add_sequence_id;
                command.Parameters.Add("@beneficiary_con_sequence_id", SqlDbType.BigInt).Value = beneficiary_con_sequence_id;

                if (command != null)
                {
                    int i = sqlkonek.sqlUpdate(command);
                }
            }
            catch
            {
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }

        /* DDC APRIL 20 2016*/
        public string get_client_hitlist_record()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR_QUERY"] != null)
                {
                    return (string)System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR_QUERY"];
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        private string cif_enrollment_pending_ovr_hit_list(string last_name, string first_name, string middle_name, string branch_code, string cif_classification)
        {
            DataTable dt = null;

            string enroll_phonetics_1 = string.Empty;
            string enroll_phonetics_2 = string.Empty;
            string enroll_phonetics_3 = string.Empty;
            string enroll_phonetics_4 = string.Empty;
            string enroll_phonetics_5 = string.Empty;
            string enroll_phonetics_6 = string.Empty;
            string enroll_phonetics_7 = string.Empty;
            string enroll_phonetics_8 = string.Empty;

            string uwnames_1 = string.Empty;
            string uwnames_2 = string.Empty;
            string uwnames_3 = string.Empty;
            string uwnames_4 = string.Empty;

            string return_hitlist = string.Empty;

            irsClientData client_data = null;

            try
            {
                return_hitlist = string.Empty;

                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                }
                else
                {
                    return string.Empty;
                }

                enroll_phonetics_1 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics1"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics1"]) : string.Empty;
                enroll_phonetics_2 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics2"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics2"]) : string.Empty;
                enroll_phonetics_3 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics3"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics3"]) : string.Empty;
                enroll_phonetics_4 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics4"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics4"]) : string.Empty;
                enroll_phonetics_5 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics5"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics5"]) : string.Empty;
                enroll_phonetics_6 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics6"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics6"]) : string.Empty;
                enroll_phonetics_7 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics13"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics13"]) : string.Empty;
                enroll_phonetics_8 = (string.IsNullOrEmpty(Session["strNewEnrolledCIFPhonetics14"].ToString()).Equals(false)) ? Convert.ToString(Session["strNewEnrolledCIFPhonetics14"]) : string.Empty;

                uwnames_1 = (string.IsNullOrEmpty(Session["strFirstName"].ToString()).Equals(false)) ? Convert.ToString(Session["strFirstName"]) : string.Empty;
                uwnames_2 = (string.IsNullOrEmpty(Session["strLastName"].ToString()).Equals(false)) ? Convert.ToString(Session["strLastName"]) : string.Empty;
                uwnames_3 = (string.IsNullOrEmpty(Session["strMiddleName"].ToString()).Equals(false)) ? Convert.ToString(Session["strMiddleName"]) : string.Empty;
                uwnames_4 = (string.IsNullOrEmpty(Session["strPercentageScore"].ToString()).Equals(false)) ? Convert.ToString(Session["strPercentageScore"]) : string.Empty;

                if (uwnames_4.ToUpper() == "UWPM" || uwnames_4.ToUpper() == "UWFL")
                {
                    dt = sqlkonek.getDataSet("sp_check_and_get_cif_percentage_validations", true, new object[] { client_data.UserId, client_data.Location }).Tables[0];

                    string hitlist_query = string.Empty;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        hitlist_query = "sp_check_and_get_cif_percentage_validations~";
                        hitlist_query += client_data.UserId + "~";
                        hitlist_query += client_data.Location + "~";

                        System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR_QUERY"] = null;
                        System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR_QUERY"] = hitlist_query;

                        int j = dt.Rows.Count;

                        if (dt.Rows.Count > 2)
                        {
                            j = 2;
                        }

                        return_hitlist += "CIF CLASS: " + cif_classification.ToUpper();
                        return_hitlist += Environment.NewLine;
                        return_hitlist += "CIF FULLNAME: " + first_name.ToUpper() + " " + middle_name.ToUpper() + " " + last_name.ToUpper();
                        return_hitlist += Environment.NewLine;

                        for (int i = 0; i < j; i++)
                        {
                            return_hitlist += "\nFILE TYPE: " + dt.Rows[i]["FileFormat"].ToString().Trim().ToUpper() +
                                              "\nFIRST NAME: " + dt.Rows[i]["FirstName"].ToString().Trim().ToUpper() +
                                              "\nMIDDLE NAME: " + dt.Rows[i]["MiddleName"].ToString().Trim().ToUpper() +
                                              "\nLAST NAME: " + dt.Rows[i]["LastName"].ToString().Trim().ToUpper() +
                                              "\nALIAS: " + dt.Rows[i]["AliasFullName"].ToString().Trim().ToUpper() +
                                              Environment.NewLine +
                                              Environment.NewLine;
                        }
                    }
                }
                else if (uwnames_4 == null || uwnames_4 == string.Empty || uwnames_4.ToUpper() != "UWPM" || uwnames_4.ToUpper() != "UWFL")
                {
                    dt = sqlkonek.getDataSet("sp_check_and_get_cif_hitlist_validations", true, new object[] { first_name, last_name, middle_name, branch_code, enroll_phonetics_1, enroll_phonetics_2, enroll_phonetics_3, enroll_phonetics_4, enroll_phonetics_5, enroll_phonetics_6, enroll_phonetics_7, enroll_phonetics_8 }).Tables[0];

                    string hitlist_query = string.Empty;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        hitlist_query = "sp_check_and_get_cif_hitlist_validations~";
                        hitlist_query += first_name + "~";
                        hitlist_query += last_name + "~";
                        hitlist_query += middle_name + "~";
                        hitlist_query += branch_code + "~";
                        hitlist_query += enroll_phonetics_1 + "~";
                        hitlist_query += enroll_phonetics_2 + "~";
                        hitlist_query += enroll_phonetics_3 + "~";
                        hitlist_query += enroll_phonetics_4 + "~";
                        hitlist_query += enroll_phonetics_5 + "~";
                        hitlist_query += enroll_phonetics_6 + "~";
                        hitlist_query += enroll_phonetics_7 + "~";
                        hitlist_query += enroll_phonetics_8 + "~";
                        hitlist_query += first_name + "~";

                        System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR_QUERY"] = null;
                        System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR_QUERY"] = hitlist_query;

                        int j = dt.Rows.Count;

                        if (dt.Rows.Count > 2)
                        {
                            j = 2;
                        }

                        return_hitlist += "CIF CLASS: " + cif_classification.ToUpper();
                        return_hitlist += Environment.NewLine;
                        return_hitlist += "CIF FULLNAME: " + first_name.ToUpper() + " " + middle_name.ToUpper() + " " + last_name.ToUpper();
                        return_hitlist += Environment.NewLine;

                        for (int i = 0; i < j; i++)
                        {
                            return_hitlist += "\nFILE TYPE: " + dt.Rows[i]["FileFormat"].ToString().Trim().ToUpper() +
                                              "\nFIRST NAME: " + dt.Rows[i]["FirstName"].ToString().Trim().ToUpper() +
                                              "\nMIDDLE NAME: " + dt.Rows[i]["MiddleName"].ToString().Trim().ToUpper() +
                                              "\nLAST NAME: " + dt.Rows[i]["LastName"].ToString().Trim().ToUpper() +
                                              "\nALIAS: " + dt.Rows[i]["AliasFullName"].ToString().Trim().ToUpper() +
                                              Environment.NewLine +
                                              Environment.NewLine;
                        }
                    }
                }

                return return_hitlist;
            }
            catch
            {
                return string.Empty;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_cif_enrollment_pending_ovr_refinitiv(string remitter, string beneficiary)
        {
            DataSet ds = null;
            DataSet ds1 = null;

            IRSWorld.Internet.InternetRemitter internet_remit = null;

            string last_name = string.Empty;
            string first_name = string.Empty;
            string middle_name = string.Empty;
            string branch_code = string.Empty;
            string pending_over_stat = string.Empty;
            string override_type_rem = string.Empty;
            string override_type_ben = string.Empty;
            string hit_names_container = string.Empty;
            string hit_name_final = string.Empty;
            string remitter_ciftype = "1";
            string beneficiary_ciftype = "1";
            string linked_ciftype = string.Empty;
            try
            {
                DataTable d1 = sqlkonek.getDataSet("PROC_OTC_CIFTYPE", true, new object[] { remitter, beneficiary }).Tables[0];
                if (d1 != null && d1.Rows.Count > 0)
                {
                    remitter_ciftype = d1.Rows[0]["remitter_ciftype"].ToString();
                    beneficiary_ciftype = d1.Rows[0]["beneficiary_ciftype"].ToString();
                }
            }
            catch
            {
                remitter_ciftype = "1";
                beneficiary_ciftype = "1";
            }
            try
            {
                hit_names_container = string.Empty;
                hit_name_final = string.Empty;

                System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] = null;

                ds = sqlkonek.getDataSet("otc_remote_enrollment_ovr_refinitiv", true, new object[] { remitter, beneficiary }, new string[] { });

                /* REMITTER */
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string cif_classification = "REMITTER";

                    last_name = ds.Tables[0].Rows[0]["LastName"].ToString();
                    first_name = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    middle_name = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                    branch_code = ds.Tables[0].Rows[0]["BranchCode"].ToString();

                    hit_names_container = last_name + " " + first_name + " " + middle_name;

                    hit_name_final = hit_names_container;
                }

                /* BENEFICIARY */
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    string cif_classification = "BENEFICIARY";

                    last_name = ds.Tables[1].Rows[0]["LastName"].ToString();
                    first_name = ds.Tables[1].Rows[0]["FirstName"].ToString();
                    middle_name = ds.Tables[1].Rows[0]["MiddleName"].ToString();
                    branch_code = ds.Tables[1].Rows[0]["BranchCode"].ToString();

                    hit_names_container = last_name + " " + first_name + " " + middle_name;

                    hit_name_final += hit_names_container;
                }

                if (hit_name_final != "")
                {
                    return hit_name_final;
                }

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_cif_enrollment_pending_ovr(string remitter, string beneficiary)
        {
            DataSet ds = null;

            IRSWorld.Internet.InternetRemitter internet_remit = null;

            string last_name = string.Empty;
            string first_name = string.Empty;
            string middle_name = string.Empty;
            string branch_code = string.Empty;
            string pending_over_stat = string.Empty;
            string override_type_rem = string.Empty;
            string override_type_ben = string.Empty;
            string hit_names_container = string.Empty;
            string hit_name_final = string.Empty;
            string remitter_ciftype = "1";
            string beneficiary_ciftype = "1";
            try
            {
                DataTable d1 = sqlkonek.getDataSet("PROC_OTC_CIFTYPE", true, new object[] { remitter, beneficiary }).Tables[0];
                if (d1 != null && d1.Rows.Count > 0)
                {
                    remitter_ciftype = d1.Rows[0]["remitter_ciftype"].ToString();
                    beneficiary_ciftype = d1.Rows[0]["beneficiary_ciftype"].ToString();
                }
            }
            catch
            {
                remitter_ciftype = "1";
                beneficiary_ciftype = "1";
            }
            try
            {
                hit_names_container = string.Empty;
                hit_name_final = string.Empty;

                System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] = null;

                ds = sqlkonek.getDataSet("otc_remote_enrollment_ovr", true, new object[] { remitter, beneficiary }, new string[] { });

                if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0)
                {
                    internet_remit = new IRSWorld.Internet.InternetRemitter();
                }

                /* REMITTER ENROLLMENT OVERRIDE TYPE LOOKUP*/
                if (ds != null && ds.Tables[3].Rows.Count > 0)
                {
                    override_type_rem = ds.Tables[3].Rows[0]["enroll_rem"].ToString();
                }
                /* REMITTER ENROLLMENT OVERRIDE TYPE LOOKUP*/


                /* BENEFICIARY ENROLLMENT OVERRIDE TYPE LOOKUP*/
                if (ds != null && ds.Tables[4].Rows.Count > 0)
                {
                    override_type_ben = ds.Tables[4].Rows[0]["enroll_ben"].ToString();
                }
                /* BENEFICIARY ENROLLMENT OVERRIDE TYPE LOOKUP*/

                /* PENDING OVR STATUS LOOKUP */
                if (ds != null && ds.Tables[2].Rows.Count > 0)
                {
                    pending_over_stat = ds.Tables[2].Rows[0]["tran_status"].ToString();
                }
                /* PENDING OVR STATUS LOOKUP */

                /* REMITTER */
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string cif_classification = "REMITTER";

                    last_name = ds.Tables[0].Rows[0]["LastName"].ToString();
                    first_name = ds.Tables[0].Rows[0]["FirstName"].ToString();
                    middle_name = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                    branch_code = ds.Tables[0].Rows[0]["BranchCode"].ToString();

                    //ORIGINAL CODE COMMENTED JANUARY 05 2021 hit_names_container = internet_remit.get_hitlist(last_name, first_name, middle_name, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, remitter_ciftype);

                    /* RFC-8800 JANUARY 05 2021 ADDED INTERCHANGE OF LAST NAME AND FIRST NAME */
                    hit_names_container = internet_remit.get_hitlist(first_name, last_name, middle_name, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, remitter_ciftype);

                    if ( string.IsNullOrEmpty(hit_names_container).Equals(true))
                    {
                        hit_names_container = internet_remit.get_hitlist(last_name, first_name, middle_name, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, remitter_ciftype);
                    }
                    /* RFC-8800 JANUARY 05 2021 ADDED INTERCHANGE OF LAST NAME AND FIRST NAME */

                    if (string.IsNullOrEmpty(hit_names_container).Equals(false))
                    {
                        hit_names_container = this.cif_enrollment_pending_ovr_hit_list(last_name, first_name, middle_name, branch_code, cif_classification);
                    }

                    if (string.IsNullOrEmpty(hit_names_container).Equals(false))
                    {
                        System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] += string.Empty + "|" + override_type_rem + "~";
                    }

                    hit_name_final = hit_names_container;
                    hit_names_container = string.Empty;
                }

                /* BENEFICIARY */
                if (ds != null && ds.Tables[1].Rows.Count > 0)
                {
                    string cif_classification = "BENEFICIARY";

                    last_name = ds.Tables[1].Rows[0]["LastName"].ToString();
                    first_name = ds.Tables[1].Rows[0]["FirstName"].ToString();
                    middle_name = ds.Tables[1].Rows[0]["MiddleName"].ToString();
                    branch_code = ds.Tables[1].Rows[0]["BranchCode"].ToString();

                    //ORIGINAL CODE COMMENTED JANUARY 05 2021 hit_names_container = internet_remit.get_hitlist(last_name, first_name, middle_name, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, beneficiary_ciftype);
                    
                    /* RFC-8800 JANUARY 05 2021 ADDED INTERCHANGE OF LAST NAME AND FIRST NAME */
                    hit_names_container = internet_remit.get_hitlist(first_name, last_name, middle_name, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, beneficiary_ciftype);

                    if ( string.IsNullOrEmpty(hit_names_container).Equals(true))
                    {
                        hit_names_container = internet_remit.get_hitlist(last_name, first_name, middle_name, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, beneficiary_ciftype);
                    }
                    /* RFC-8800 JANUARY 05 2021 ADDED INTERCHANGE OF LAST NAME AND FIRST NAME */

                    if (string.IsNullOrEmpty(hit_names_container).Equals(false))
                    {
                        hit_names_container = this.cif_enrollment_pending_ovr_hit_list(last_name, first_name, middle_name, branch_code, cif_classification);
                    }

                    if (string.IsNullOrEmpty(hit_names_container).Equals(false))
                    {
                        System.Web.HttpContext.Current.Session["IRS_ENROLLMENT_PENDING_OVR"] += string.Empty + "|" + override_type_ben + "~";
                    }

                    hit_name_final += hit_names_container;
                    hit_names_container = string.Empty;
                }

                if (string.IsNullOrEmpty(hit_name_final).Equals(false) && string.IsNullOrEmpty(pending_over_stat).Equals(false))
                {
                    return pending_over_stat + '~' + hit_name_final;
                }

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }
        /* DDC APRIL 20 2016*/

        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool got_client_hit_list_all_refinitiv(string remitter_id, string beneficiary_id)
        {
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = null;
            DataTable remitter_dt = null;
            DataTable beneficiary_dt = null;
            bool remitter_down = false;
            bool beneficiary_down = false;
            System.Web.HttpContext.Current.Session["REFINITIV_IS_DOWN"] = null;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual_Remitter = false;
            bool isRefinitivIndividual_Beneficiary = false;

            List<string> RemitterSsecondary = new List<string>();
            DataTable dtCIFRemitter = irsrefinitiv.getCIFInfo(remitter_id);
            if (dtCIFRemitter.Rows[0]["CIFType"] != DBNull.Value && dtCIFRemitter.Rows[0]["CIFType"].ToString() != string.Empty)
            {
                if (dtCIFRemitter.Rows[0]["CIFType"].ToString().Trim() == "1")
                {
                    isRefinitivIndividual_Remitter = true;
                }
                else
                {
                    isRefinitivIndividual_Remitter = false;
                }
            }

            List<string> BeneficiairySsecondary = new List<string>();
            DataTable dtCIFBenef = irsrefinitiv.getCIFInfo(beneficiary_id);
            if (dtCIFBenef.Rows[0]["CIFType"] != DBNull.Value && dtCIFBenef.Rows[0]["CIFType"].ToString() != string.Empty)
            {
                if (dtCIFBenef.Rows[0]["CIFType"].ToString().Trim() == "1")
                {
                    isRefinitivIndividual_Beneficiary = true;
                }
                else
                {
                    isRefinitivIndividual_Beneficiary = false;
                }
            }

            string isRefinitivEnabled = sqlkonek.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
            if (isRefinitivEnabled == "1")
            {
                DataSet ds = new DataSet();
                IEnumerable<irsRefinitiv.ScreeningResponseDto> remitter_refinitiv = null;
                IEnumerable<irsRefinitiv.ScreeningResponseDto> beneficiary_refinitiv = null;
                try
                {
                    remitter_dt = new irsRefinitiv().DisplayWatchlist(branch_code, remitter_refinitiv, remitter_id, false, "", "", "", isRefinitivIndividual_Remitter, RemitterSsecondary, out remitter_down);
                    if (remitter_down == true)
                    {
                        System.Web.HttpContext.Current.Session["REFINITIV_IS_DOWN"] = "Refinitiv is down.";
                        return true;
                    }

                    beneficiary_dt = new irsRefinitiv().DisplayWatchlist(branch_code, beneficiary_refinitiv, beneficiary_id, false, "", "", "", isRefinitivIndividual_Beneficiary, RemitterSsecondary, out beneficiary_down);
                    if (beneficiary_down == true)
                    {
                        System.Web.HttpContext.Current.Session["REFINITIV_IS_DOWN"] = "Refinitiv is down.";
                        return true;
                    }

                    if (remitter_dt != null && remitter_dt.Rows.Count > 0)
                    {
                        if (remitter_dt.Rows[0][2] == DBNull.Value || remitter_dt.Rows[0][2].ToString() == string.Empty) //NO HITS FOR REMITTER
                        {
                            refinitivcs.saveWatchlist_UpdateCIFID(remitter_dt.Rows[0][1].ToString(), remitter_id);

                            if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0)
                            {
                                if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) // NO HIT FOR BENEFICIARY
                                {
                                    refinitivcs.saveWatchlist_UpdateCIFID(beneficiary_dt.Rows[0][1].ToString(), beneficiary_id);
                                    return false;
                                }
                                else //WITH HITS ONLY IN BENEFICIAIRY
                                {
                                    int hitCount_beneficiary = 0;
                                    hitCount_beneficiary = beneficiary_dt.Rows.Count;
                                    irsrefinitiv.SaveWatchlist(beneficiary_dt, beneficiary_id, false, hitCount_beneficiary);

                                    ds.Tables.AddRange(new DataTable[] { beneficiary_dt });
                                    dt = ds.Tables[0];
                                    refinitivcs.get_refinitiv_dt(dt);
                                    return true;
                                }
                            }
                            else { dt = remitter_dt; refinitivcs.get_refinitiv_dt(dt); return true; }
                        }
                        else // WITH REMITTER HITS 
                        {
                            int hitCount_remitter = 0;
                            hitCount_remitter = remitter_dt.Rows.Count;
                            irsrefinitiv.SaveWatchlist(remitter_dt, remitter_id, false, hitCount_remitter);

                            if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0)
                            {
                                if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) // NO BENEFICIAIRY HITS
                                {
                                    refinitivcs.saveWatchlist_UpdateCIFID(beneficiary_dt.Rows[0][1].ToString(), beneficiary_id);
                                    dt = remitter_dt; refinitivcs.get_refinitiv_dt(dt); return true;
                                }
                                else //WITH BENEFICIARY HITS
                                {
                                    int hitCount_beneficiary = 0;
                                    hitCount_beneficiary = beneficiary_dt.Rows.Count;
                                    irsrefinitiv.SaveWatchlist(beneficiary_dt, beneficiary_id, false, hitCount_beneficiary);
                                    remitter_dt.Merge(beneficiary_dt);
                                    refinitivcs.get_refinitiv_dt(remitter_dt);
                                    return true;
                                }
                            }
                            else { dt = remitter_dt; refinitivcs.get_refinitiv_dt(dt); return true; }
                        }
                    }
                    else //NO REMITTER HITS
                    {
                        if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0)
                        {
                            if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) //NO HITS FOR BENEFICIAIRY
                            {
                                refinitivcs.saveWatchlist_UpdateCIFID(beneficiary_dt.Rows[0][1].ToString(), beneficiary_id);
                                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                return false;
                            }
                            else
                            {
                                int hitCount_beneficiary = 0;
                                hitCount_beneficiary = beneficiary_dt.Rows.Count;
                                irsrefinitiv.SaveWatchlist(beneficiary_dt, beneficiary_id, false, hitCount_beneficiary);
                                dt = beneficiary_dt;
                                refinitivcs.get_refinitiv_dt(dt);
                                return true;
                            }
                        }
                        else { System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; return false; }
                    }
                }
                catch
                {
                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
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
            else /* NEW REFINITIV */
            {
                /* ORIGINAL HITLIST / WATCHLIST */
                try
                {
                    Session["OTC_HIT_LIST_VAL"] = null;

                    dt = sqlkonek.getDataSet("sp_otc_hit_listing", true, new object[] { remitter_id, beneficiary_id }).Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
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
        }
        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool got_client_hit_list_all(string remitter_id, string beneficiary_id)
        {
            DataTable dt = null;

            try
            {
                Session["OTC_HIT_LIST_VAL"] = null;

                dt = sqlkonek.getDataSet("sp_otc_hit_listing", true, new object[] { remitter_id, beneficiary_id }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_cif_hit_list(string remitter_id, string beneficiary_id)
        {
            DataTable dt = null;
            SqlCommand command = null;

            string remitter_hit = null;
            string beneficiary_hit = null;
            string hit_list_content = null;
            string branch_code = null;

            try
            {
                Session["InternetARMModule"] = "OTC";

                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                }

                command = sqlkonek.getSqlCommand("spGetBranchDefaultSetup");

                if (command != null)
                {
                    command.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = branch_code;
                }

                dt = sqlkonek.getDataSet(command).Tables[0];

                remitter_hit = new Internet.InternetPage().getCIFHit(remitter_id);
                beneficiary_hit = new Internet.InternetPage().getCIFHit(beneficiary_id);

                if (beneficiary_hit != null && beneficiary_hit.Trim().ToUpper() != "OK")
                {
                    hit_list_content = beneficiary_hit;
                }

                if (remitter_hit != null && remitter_hit.Trim().ToUpper() != "OK")
                {
                    hit_list_content = remitter_hit;
                }

                if (dt != null && dt.Rows.Count > 0 && hit_list_content != null)
                {
                    if (Convert.ToInt32(dt.Rows[0]["StreamLining"]) == 0)
                    {
                        if (hit_list_content != "OK")
                        {
                            return "STREAM_LINING_NOT_ACTIVE~" + hit_list_content;
                        }
                    }
                    else
                    {
                        if (hit_list_content != "OK")
                        {
                            return hit_list_content;
                        }
                    }
                }

                if (hit_list_content != null && hit_list_content != "OK")
                {
                    return hit_list_content;
                }

                return "OK";
            }
            catch
            {
                return "OK";
            }
            finally
            {
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void addToPromoCounter(string refno, int chargetype)
        {
            try
            {
                string isPromoPeriod = sqlkonek.getDataSet("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END WHERE GETDATE() BETWEEN (SELECT FldDescription FROM tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 0) AND (SELECT FldDescription FROM tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 1) ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();

                if (isPromoPeriod == "1")
                {
                    //int promo_charge_type = int.Parse(sqlkonek.getDataSet("SELECT TOP 1 ISNULL(FldValue,0) FROM tbl_lookup WHERE FldGroup = 'ChargeType' AND FldCode = '3PL1-PROMO'", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString());
                    //if (chargetype == promo_charge_type)
                    //{
                    DataTable dt = sqlkonek.getDataSet("SELECT TOP 1 RemitterCIFID,TranxDate FROM tbl_remittance WHERE ReferenceNo = '" + refno + "'", false, new object[] { }).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        string rem_cifid = dt.Rows[0].ItemArray[0].ToString();
                        string tranxdate = dt.Rows[0].ItemArray[1].ToString();

                        dt = sqlkonek.getDataSet("SELECT TransactionCounter from [3PL1PROMO] WHERE RemitterCIFID = '" + rem_cifid + "' AND (LastTransactionDate BETWEEN (SELECT FldDescription from tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 0) AND (SELECT FldDescription from tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 1)) ", false, new object[] { }).Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            sqlkonek.execScalar("UPDATE [3PL1PROMO] SET TransactionCounter = (CASE WHEN TransactionCounter >= 3 THEN 0 ELSE TransactionCounter + 1 END), LastTransactionDate = '" + tranxdate + "' WHERE RemitterCIFID = '" + rem_cifid + "' AND (LastTransactionDate BETWEEN (SELECT FldDescription from tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 0) AND (SELECT FldDescription from tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 1))");
                        }
                        else
                        {
                            sqlkonek.execScalar("INSERT INTO [3PL1PROMO] VALUES('" + rem_cifid + "',1,'" + tranxdate + "')");
                        }
                    }
                    //}
                }
            }
            catch { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool promoVerification(string rem_cifid, int chargetype)
        {
            try
            {
                string isPromoPeriod = sqlkonek.getDataSet("SELECT CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END WHERE GETDATE() BETWEEN (SELECT FldDescription FROM tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 0) AND (SELECT FldDescription FROM tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 1) ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();

                if (isPromoPeriod == "1")
                {
                    DataTable dt = sqlkonek.getDataSet("SELECT TransactionCounter from [3PL1PROMO] WHERE RemitterCIFID = '" + rem_cifid + "' AND TransactionCounter > 2 AND (LastTransactionDate BETWEEN (SELECT FldDescription from tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 0) AND (SELECT FldDescription from tbl_lookup WHERE FldGroup = '3PL1PROMO' AND FldValue = 1)) ", false, new object[] { }).Tables[0];

                    if (dt.Rows.Count > 0) return true;
                }
            }
            catch { }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_contact_info(string remitter_id, string beneficiary_id)
        {
            try
            {
                return sqlkonek.getDataSet("otc_contact_nos", true, new object[] { remitter_id, beneficiary_id });
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string risk_rating(string remid)
        {
            string s = Convert.ToString(sqlkonek.execScalar("SELECT dbo.f_getLookupValue('RiskRating',RiskRating) AS RiskRating FROM TBL_CIF_REMITTERS WHERE CIFID='" + remid + "'"));
            if (s.IndexOf("DBNull") > -1)
            {
                return string.Empty;
            }
            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool submitted_screen_activated(string brcode)
        {
            string fldgroup = Convert.ToString(sqlkonek.execScalar("select fldgroup from tbl_sub_lookup where branchcode='" + brcode + "' and fldgroup='SubmittedDocuments'"));
            if (fldgroup == "" || fldgroup == null)
            {
                return false;
            }
            else
            {
                return true;
            }
            return false;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool proofofadd(string remitterid)
        {
            DataSet dsProofAdd = sqlkonek.getDataSet(sqlkonek.getSqlCommand("select * from tbl_cif_ids where cifid='" + remitterid + "' order by seq_id", false));  //Convert.ToString(sqlkonek("select fldgroup from tbl_sub_lookup where branchcode='" + brcode + "' and fldgroup='SubmittedDocuments'"));
            DataTable dtProofAdd;
            if (dsProofAdd != null)
            {
                dtProofAdd = dsProofAdd.Tables[0];
            }
            else
            {
                return false;
            }


            if (dtProofAdd != null)
            {
                if (dtProofAdd.Rows.Count > 1)
                {
                    string idno = Convert.ToString(dtProofAdd.Rows[1]["IDNo"]);
                    string idtype = Convert.ToString(dtProofAdd.Rows[1]["IDType"]);
                    //Session["idtype2"] = Convert.ToString(dtProofAdd.Rows[0]["IDType"]);
                    if (idno == null)
                    {
                        idno = "";
                    }

                    if (idtype == null)
                    {
                        idtype = "";
                    }


                    if (idno != string.Empty || idtype != string.Empty)
                    {
                        return true;
                    }

                }
                return false;
            }
            return false;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool proofofidentification(string remitterid)
        {
            DataSet dsProofofidentification = sqlkonek.getDataSet(sqlkonek.getSqlCommand("select * from tbl_cif_ids where cifid='" + remitterid + "' order by seq_id", false));  //Convert.ToString(sqlkonek("select fldgroup from tbl_sub_lookup where branchcode='" + brcode + "' and fldgroup='SubmittedDocuments'"));
            DataTable dtProofofidentification;
            if (dsProofofidentification != null)
            {
                dtProofofidentification = dsProofofidentification.Tables[0];
            }
            else
            {
                return false;
            }


            if (dtProofofidentification != null)
            {
                if (dtProofofidentification.Rows.Count > 0)
                {
                    string idno = "";
                    string idtype = "";
                    idtype = Convert.ToString(dtProofofidentification.Rows[0]["IDType"]);//Convert.ToString(Session["idtype2"]);
                    if (dtProofofidentification.Rows.Count > 0)
                    {
                        idno = Convert.ToString(dtProofofidentification.Rows[0]["IDNo"]);
                    }

                    if (idno == null)
                    {
                        idno = "";
                    }

                    if (idtype == null)
                    {
                        idtype = "";
                    }


                    if (idno != string.Empty && idtype != string.Empty)
                    {
                        Session["idtype2"] = null;
                        return true;
                    }

                }
                return false;
            }
            return false;
        }

        //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Check_ACH_Extract_Setup()
        {
            DataTable dt_return = null;
            string BranchCode = "";

            try
            {
                BranchCode = get_branchCode();

                sqlkonek.EnforceConstraint = false;
                dt_return = sqlkonek.execStoredProcedureTable("proc_Check_ACH_Extract_Setup", new string[] { BranchCode });
                if (dt_return != null)
                {
                    if (dt_return.Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                this.logs("", "Remittance.RemOTC.Check_ACH_Extract_Setup", e.Message.ToString().Trim(), "", BranchCode);
            }
            finally
            {
                dt_return = null;
                BranchCode = "";
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Check_IsRemitter_ACH(int paymode)
        {
            DataTable dt_return = null;
            try
            {
                sqlkonek.EnforceConstraint = false;
                dt_return = sqlkonek.execStoredProcedureTable("proc_Check_IsRemitter_ACH_Extract", new object[] { paymode });
                if (dt_return != null)
                {
                    if (dt_return.Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                this.logs("", "Remittance.RemOTC.Check_IsRemitter_ACH", e.Message.ToString().Trim(), "", "PayMode : " + paymode.ToString());
            }
            finally
            {
                dt_return = null;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void Save_RemAccount_Details(string Refno, int RemAccountType, string RemAccountNo, string RemRoutingNo, string RemBankName)
        {
            int i_return = 0;

            try
            {
                sqlkonek.EnforceConstraint = false;
                i_return = sqlkonek.execStoredProcedureUpdate("proc_Save_Internet_ACHWorks", new object[] { Refno, RemAccountType, RemAccountNo, RemRoutingNo, RemBankName });
                if (i_return < 0)
                {
                    this.logs("", "Remittance.RemOTC.Save_RemAccount_Details", "Problem in excuting save process for reference no : " + Refno + "; AccountNo : " + RemAccountNo + "; Routing No : " + RemRoutingNo + "; Bank Name : " + RemBankName, "", "");
                }
            }
            catch (Exception e)
            {
                this.logs("", "Remittance.RemOTC.Save_RemAccount_Details", e.Message.ToString().Trim(), i_return.ToString(), Refno);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getForexPerSourceTarget(string SourceCurrency, string TargetCurrency)
        {
            string branchCode = this.get_branchCode();
            DateTime dtBranchTime = new irsTimeZone().branchLocalTime(branchCode);
            DataSet ds = sqlkonek.getDataSet("osp_getForexPerSourceTarget", true, new object[] { branchCode, dtBranchTime.ToString("yyyy-MM-dd"), SourceCurrency, TargetCurrency });
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        /* RFS BLOCK TIME DEPOSIT */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAccountTD(string AccountNo)
        {
            return new Internet.InternetRemitter().isAccountTD(AccountNo, false);
        }

        /* 3D THRESHOLD START */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d(string currency)
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d(currency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d_inc(string currency)
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d_inc(currency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d_min(string currency)
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d_min(currency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private string get3DThresholdFlag()
        {
            return new IRSWorld.TranClass.RemTranExtra().get_threshold_3d_flag();
        }
        /* 3D THRESHOLD END */

        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Instapay_validation(string remcifid, string benecifid)
        {

            return new IRSWorld.TranClass.instapay_scr().Instapay_validation(remcifid, benecifid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Instapay_BankValidation(string branchcode)
        {
            return new IRSWorld.TranClass.instapay_scr().Instapay_BankValidation(branchcode);
        }
        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
    }
}