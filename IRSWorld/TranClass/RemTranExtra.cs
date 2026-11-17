using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;
using Bharosa.VCrypt.Common.Util;
using IRSWorld.bharosa;
using System.Linq;
using System.Text.RegularExpressions;

namespace IRSWorld.TranClass
{
    [Serializable()]
    public class br_reqids : sqlHelperWs
    {
        public br_reqids()
        {
        }

        private bool check_if_exists(String BranchCode, String IDDescription, int Required)
        {
            string s = "SELECT COUNT(*) AS rec_count FROM TBL_VALID_ID WHERE BranchCode = '" + BranchCode + "' AND LEFT(IDDescription, 5) = LEFT('" + IDDescription + "',5) AND RIGHT(IDDescription,5) = RIGHT('" + IDDescription + "',5) AND Required = " + Required.ToString();

            if (Convert.ToInt32(base.getDataSet(s, false, new string[] { }).Tables[0].Rows[0]["rec_count"]) > 0)
            {
                return true;
            }

            return false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_VALID_ID";
            this.IdentityColumn = "SEQ_ID";

            base.getUpdateDataSet("SELECT * FROM TBL_VALID_ID WHERE SEQ_ID = " + id, false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_branches()
        {
            return getDataSet("req_id_branch_list", true, new Object[] { });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_branch_id_lookup(string branch_code)
        {
            return getDataSet("otc_val_ids_ddl", true, new Object[] { branch_code });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_branch_ids(String branch_code)
        {
            return getDataSet("branch_req_id_list", true, new Object[] { branch_code });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_record(String BranchCode, String IDDescription, int Required)
        {
            base.Message = String.Empty;

            if (this.check_if_exists(BranchCode, IDDescription, Required).Equals(true))
            {
                return "Record exists.";
            }

            if (base.newRow().Equals(true))
            {
                base.setColumn("BranchCode", BranchCode);
                base.setColumn("IDDescription", IDDescription);
                base.setColumn("Required", Required);
                base.setColumn("Deleted", 0);

                if (base.save().Equals(false))
                {
                    return base.Message;
                }
            }

            if (String.IsNullOrEmpty(base.Message) == false)
            {
                return base.Message;
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_record(String BranchCode, String IDDescription, int Required, int seq_id)
        {
            base.Message = String.Empty;
            base.selectedRow = 0;

            if (base.edit(seq_id).Equals(true))
            {
                base.setColumn("IDDescription", IDDescription);
                base.setColumn("Required", Required);
                base.setColumn("Deleted", 0);

                if (base.save().Equals(false))
                {
                    return base.Message;
                }
            }

            if (String.IsNullOrEmpty(base.Message) == false)
            {
                return base.Message;
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string delete_record(int seq_id)
        {
            base.Message = String.Empty;
            base.selectedRow = 0;

            if (base.edit(seq_id).Equals(true))
            {
                base.setColumn("Deleted", 1);

                if (base.save().Equals(false))
                {
                    return base.Message;
                }
            }

            if (String.IsNullOrEmpty(base.Message) == false)
            {
                return base.Message;
            }

            return string.Empty;
        }
    }

    [Serializable()]
    public class RemTranExtra: sqlHelperWs
    {
        public RemTranExtra()
        {
            base.EnforceConstraint = false;
        }

        /* 3D THRESHOLD START */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d(string currency)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;

            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("get_threshold_3d", new object[] { currency });

            if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
            {
                string threshold_maxamount = ds_return.Tables[0].Rows[0]["Threshold_3D"].ToString();

                if (threshold_maxamount != null || threshold_maxamount != "")
                {
                    return threshold_maxamount;
                }
                else { return string.Empty; }
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d_inc(string currency)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;

            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("get_threshold_3d", new object[] { currency });


            if (ds_return != null && ds_return.Tables[2].Rows.Count > 0)
            {
                string threshold_inc = ds_return.Tables[2].Rows[0]["Threshold_3D_INC"].ToString();

                if (threshold_inc != null || threshold_inc != "")
                {
                    return threshold_inc;
                }
                else { return string.Empty; }
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d_min(string currency)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;

            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("get_threshold_3d", new object[] { currency });

            if (ds_return != null && ds_return.Tables[1].Rows.Count > 0)
            {
                string threshold_min = ds_return.Tables[1].Rows[0]["Threshold_3D_MIN"].ToString();

                if (threshold_min != null || threshold_min != "")
                {
                    return threshold_min;
                }
                else { return null; }
            }
            return null;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_threshold_3d_flag()
        {
            string Thresholdflag = null;
            Thresholdflag = new irsConfiguration().appsetting("3DThreshold");

            try
            {
                if (Thresholdflag != null || Thresholdflag != "" || Thresholdflag != "0")
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch
            {
                return "0";
            }

        }
        /* 3D THRESHOLD END */



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void remote_queue_ovrtype(string override_type)
        {
            //===================================================
            //RFC -26582 : clear pending override session
            //System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;
            //===================================================

            string[] s = null;

            if (string.IsNullOrEmpty(override_type).Equals(false))
            {
                s = override_type.Split('~');
            }

            if (s != null && s.Length > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += string.Empty + "|" + s[i] + "~";
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] check_federal_disclosure(string remitterID, string trandate)
        {
            SqlCommand command = null;
            DataTable dt = null;
            try
            {                
                command = getSqlCommand("sp_federal_disclosure");
                if (command != null)
                {
                    command.Parameters.Add("@remitterID", SqlDbType.VarChar).Value = remitterID;
                    command.Parameters.Add("@trandate", SqlDbType.VarChar).Value = trandate;
                }
                dt = getDataSet(command).Tables[0];
                if (dt != null)
                {
                    writeLog("Federal disclosure alert. remitterID: " + remitterID + " tranxdate: " + trandate, "FEDERAL DISCLOSURE", "", "", csAuditLog.Category.Insert);
                    return new Object[] { dt.Rows[0]["ret"], dt.Rows[0]["message"] };
                }
                return new Object[] { "false", "error while executing sql" };
            }
            catch
            {
                return new Object[] { "false", "error while executing sql" };
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
        public bool branch_in_stream_lining(string branch_code)
        {
            SqlCommand command = null;
            DataTable dt = null;
            
            string br_code = null;

            try
            {
                if (branch_code != null && branch_code.Trim().Length > 0)
                {
                    br_code = branch_code;
                }

                if (br_code == null || br_code.Trim().Length < 1)
                {
                    if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                    {
                        br_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                    }
                }

                command = getSqlCommand("spGetBranchDefaultSetup");

                if (command != null)
                {
                    command.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = br_code;
                }

                dt = getDataSet(command).Tables[0];

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["StreamLining"] != null && Convert.ToInt32(dt.Rows[0]["StreamLining"]) > 0)
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
        public int chk_ho_approver(string user_name, string pass_word)
        {
            irsUsers irsusers = new irsUsers();

            Object[] ireturns = irsusers.validate(user_name, pass_word, Convert.ToByte(1), true);

            if (ireturns != null)
            {
                if ((bool)ireturns[0] == true)
                {
                    DataTable dt = getDataSet("otc_chk_ho_rule", true, new object[] { Convert.ToInt32(irsusers.table.Rows[0]["Role"]) }).Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["count_rec"]) > 0)
                        {
                            return 1;
                        }
                        else if (this.can_do_override(Convert.ToInt32(irsusers.table.Rows[0]["Role"])) == true)
                        {
                            return 2;
                        }
                        else
                        {
                            return 3;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }

            return 0;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool gfc_not_reach_max_transaction(string beneficiary_id, string gfc_account_no, int account_currency, double beneficiary_amount)
        {
            if (gfc_account_no != null && gfc_account_no.Trim() != string.Empty && gfc_account_no.Length > 0)
            {
                return new irsLinkToGFC().check_GFC_Aggregate(beneficiary_id, gfc_account_no, account_currency, beneficiary_amount);
            }

            return true;
        }

        private bool can_do_override(int role)
        {
            string s = "SELECT COUNT(*) FROM TBL_CHARGES_USERS WHERE USERTYPE = 2 AND ROLE = " + role;

            if (System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] != null)
            {
                s += " AND ProductChargesID = " + (int)System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"];
            }
            else
            {
                s += " AND ProductChargesID < 1";
            }

            if (Convert.ToInt32(getDataSet(getSqlCommand(s, false), "ROLE_MODEL").Tables[0].Rows[0][0]) > 0)
            {
                return true;
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object[] user_can_override_hit_list(string user_name, string user_pwd)
        {
            /* START RFC-32606 - Transaction Search and Local Transaction Override */
            bool validateApproverLocation = this.validateApproverLocation(user_name);

            if (validateApproverLocation == true)
            { /* END RFC-32606 - Transaction Search and Local Transaction Override */

                irsUsers irsusers = new irsUsers();

                Object[] ireturns = irsusers.validate(user_name, user_pwd, Convert.ToByte(1), true);


                if (ireturns != null)
                {
                    if ((bool)ireturns[0] == true)
                    {
                        /* VAPT 20190422 */
                        string s = "SELECT COUNT (*) FROM tbl_lookup WHERE FLDGROUP = 'ApproverGroup' AND FLDCODE = '" + irsusers.table.Rows[0]["Role"].ToString() + "'";

                        if (Convert.ToInt32(getDataSet(getSqlCommand(s, false), "ROLE_MODEL").Tables[0].Rows[0][0]) > 0)
                        {
                            /* VAPT 20180222 */
                            string userID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                            if (userID.Trim() != user_name.Trim())
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "USER MADE AN OVERRIDE USING USER ID (" + user_name + ")", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                //new irsOverrideLog("", "").StreamLining(user_name);
                                new irsOverrideLog("", "").Refinitiv(user_name);
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "USER CANNOT BE THE THE MAKER AND CHECKER AT THE SAME TIME. OVERRIDE FAILED.", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                                return new Object[] { false, "The Maker and Checker cannot be the same. Override Failed." };
                            }
                        }
                        else
                        {
                            /* VAPT 20180222 */
                            new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "USER ATTEMPTED TO OVERRIDE USING USER ID (" + user_name + ") WITH AN INVALID ACCESS RIGHTS.", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                            return new Object[] { false, "Invalid access rights." };
                        }
                    }
                    else
                    {
                        /* VAPT 20180222 */
                        new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "USER CANNOT OVERRIDE WITHOUT A VALID ACCESS RIGHT. OVERRIDE FAILED.", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                    }

                    return new Object[] { ireturns[0], ireturns[1] };
                }

                return new Object[] { false, "Can not validate user at the moment." };
            }
            /* START RFC-32606 - Transaction Search and Local Transaction Override */
            else
            {
                return new Object[] { false, "User Id is not allowed to approve this transaction." };
            } /* END RFC-32606 - Transaction Search and Local Transaction Override */
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string load_batch_data()
        {
            irsGRSBatchEntries irs_batch = null;

            string s = string.Empty;

            decimal runamt = 0;
            decimal totamt = 0;
            decimal totdiff = 0;

            int totitems   = 0;
            int curritems  = 0;

            try
            {
                irs_batch = (irsGRSBatchEntries)System.Web.HttpContext.Current.Session["IBATCH_HEADER"];

                if (irs_batch != null)
                {
                    curritems = irs_batch.p_noofitems    - irs_batch.p_curitems;
                    runamt    = irs_batch.p_total_amount - irs_batch.p_running_amount;
                    totamt    = irs_batch.p_total_amount;
                    totitems  = irs_batch.p_noofitems;
                    totdiff = totamt - runamt;

                    s = irs_batch.p_formattedbatchnum           + "|";
                    s += totamt.ToString("#.00")                       + "|";
                    s += totitems.ToString()                     + "|";
                    s += irs_batch.p_running_amount.ToString("#.00")   + "|";
                    s += irs_batch.p_curitems.ToString()         + "|";
                    s += totdiff.ToString("#.00") + "|";
                    s += Convert.ToString(totitems - curritems)  + "|";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_account_details(string account_number)
        {
            //return new irsAccountInquiry().getFC(account_number, irsAccountInquiry.FCMethod.AccountStatusInquiry, 0, true).ToUpper();
            return new irsAccountInquiry().getFC(account_number, irsAccountInquiry.FCMethod.AccountStatusAndBalInquiry, 0, true).ToUpper();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_flexcube_account_information(string account_number)
        {
            /* FOR CBIP */
            string accountDetails = new irsClientAccounts().get_acctdetails(account_number);
            string[] arrAccountDetails = null;

            if (accountDetails != "ERROR")
            {
                arrAccountDetails = accountDetails.Split('~');
                DataSet ds = getDataSet(getSqlCommand("SELECT SEQ_ID FROM TBL_BRANCHES WHERE BRANCHTYPE = 2 AND BRANCHCODE = '" + arrAccountDetails.GetValue(3).ToString() + "'", false));
                string branchCodeSEQID = string.Empty;
                if (ds != null && ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                    {
                        branchCodeSEQID = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
                /* branchcode, accounttype, currency, accountno, errormessage, branchseqid */
                return arrAccountDetails.GetValue(3).ToString() + "~" +
                    arrAccountDetails.GetValue(1).ToString() + "~" +
                    arrAccountDetails.GetValue(0).ToString() + "~" +
                    arrAccountDetails.GetValue(4).ToString() + "~" +
                    string.Empty + "~" + branchCodeSEQID;
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool set_to_session_computation_direction(int i)
        {
            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] = null;
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] = i;

                return true;
            }
            catch
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool set_to_session_charge_id(int i)
        {
            System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = null;

            if (i > 0)
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = i;
            }

            if (System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] != null)
            {
                return true;
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public decimal[] get_rem_aggregate_per_currency(string remitter_id, int remitting_currency, bool rem_aggregation_tag, bool cash_aggregation_enabled) //Modified RDG as Faultlog of Italy 02/12/2013
        {
            //string s = "SELECT " +
            //                "ISNULL(SUM(GrossRemAmount),0) AS GrossRemAmount  " +
            //           "FROM TBL_REMITTANCE " +
            //           "WHERE DATEDIFF(MONTH, TranxDate,'" + this.get_logged_data()[1].ToString() + "') <= 1 AND " +
            //           "RemitterCIFID     = '" + remitter_id + "' AND " +
            //           "RemittingCurrency = "  + remitting_currency.ToString() + " AND " +
            //           "RemittingBranch   = '" + this.get_logged_data()[0].ToString() + "'";

            //return (decimal)getDataSet(getSqlCommand(s, false), "AGGREGATES").Tables[0].Rows[0]["GrossRemAmount"];
            //chr - for cash aggregation
            //return (decimal)getDataSet("usp_RequiredIDLimit_aggregate", true, new object[] { this.get_logged_data()[0].ToString(), remitter_id, remitting_currency, this.get_logged_data()[1].ToString() }).Tables[0].Rows[0]["GrossRemAmount"];
            DataSet ds = getDataSet("usp_RequiredIDLimit_aggregate2", true, new object[] { this.get_logged_data()[0].ToString(), remitter_id, remitting_currency, this.get_logged_data()[1].ToString(), rem_aggregation_tag, cash_aggregation_enabled });
            if (ds != null && ds.Tables.Count > 0)
            {
                //for debug
                DataRow drow = ds.Tables[0].Rows[0];
                decimal d1 = (decimal)ds.Tables[0].Rows[0]["GrossRemAmount"];
                decimal d2 = (decimal)ds.Tables[0].Rows[0]["GrossCashAmount"];
                return new decimal[] { d1, d2 };
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void gfc_insert_otc_gfc_aggregate(int transaction_sequence_id)
        {
            this.insert_gfc_account_transaction(transaction_sequence_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet credit_remittance_online(int transaction_sequence_id)
        {
            DataSet ds = null;

            try
            {
                ds = new irsRemTeller().Remit2(transaction_sequence_id, true);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    /* REMOVE FOR CBIP, NOT NEEDED AS RM IS ALREADY AGGREGATING GFC TRANSACTIONS */
                    //this.insert_gfc_account_transaction(transaction_sequence_id); //EDITED BY DDC 02/23/2016
                    /*
                    if (ds.Tables[0].Rows[0][1].ToString().Trim() == "2")
                    {
                        this.insert_gfc_account_transaction(transaction_sequence_id);
                    }
                    */
                }

                return ds;
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
        public void writeLog(String Description)
        {
            writeLog(Description, "OTC", "", "", csAuditLog.Category.Select);
        }

        public void writeLog(String Description, String Module, String NewValue, String OldValue, csAuditLog.Category Category)
        {
            String userId = get_irsclient_data("UID");
            String sDescription = "Location=" + get_irsclient_data("LOCATION") + " : " + Description;
            new csAuditLog().writeAudit(userId, Module, sDescription, NewValue, OldValue, Category);
        }

        private String get_irsclient_data(String param)
        {
            if (irsClientData == null)
            {
                return null;
            }

            string s = null;


            switch (param)
            {
                case "UID": s = irsClientData.UserId; break;
                case "CIF": s = irsClientData.CIFID; break;
                case "LOCATION": s = irsClientData.Location; break;
                case "IP": s = irsClientData.IPAddress; break;
            }

            return s;

        }

        private irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }

                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        private void insert_gfc_account_transaction(int transaction_id)
        {
            /* FOR CBIP */
            /*
            string s = "SELECT " +
                             "A.referenceno, "       +
                             "A.nettobenecurrency, " +
                             "A.nettobeneamount, "   +
                             "B.accountno "          +
                       "FROM TBL_REMITTANCE A "    +
                       "INNER JOIN TBL_CIF_ACCOUNTS B ON B.SEQ_ID = A.accountseqid " +
                       "WHERE A.SEQ_ID = " + transaction_id.ToString();

            //DataTable dt = getDataSet(getSqlCommand(s, false), "TRANSACTIONS").Tables[0];
            
            DataTable dt = getDataSet(s, false, new object[] { }, new string[] { "TRANSACTIONS" }).Tables[0]; //EDITED BY DDC 02/23/2016

            if (dt != null && dt.Rows.Count > 0)
            {
                bool ibool = new irsLinkToGFC().insert_GFC_Aggregate(Convert.ToString(dt.Rows[0]["referenceno"]).Replace("'","''"),
                                                                     Convert.ToString(dt.Rows[0]["accountno"]).Replace("'", "''"), 
                                                                     Convert.ToInt32(dt.Rows[0]["nettobenecurrency"]),
                                                                     Convert.ToDouble(dt.Rows[0]["nettobeneamount"]));
            }

            if (dt != null)
            {
                dt.Dispose();
                dt = null;
            }
            */
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet branch_setups()
        {
            object[] login_data = this.get_logged_data();

            if (login_data != null && login_data.Length > 0)
            {
                //return getDataSet(getSqlCommand("SELECT * FROM TBL_BRANCH_SETUP WHERE ISNULL(deleted, 0) = 0 AND branchcode = '" + login_data[0].ToString() + "'", false), "BRANCH_SETUP_DATA");
                return getDataSet("SELECT * FROM TBL_BRANCH_SETUP WHERE ISNULL(deleted, 0) = 0 AND branchcode = '" + login_data[0].ToString() + "'", false, new Object[] { });
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int remitter_exceptions(string remitter_id)
        {
            DataTable dt = null;
            int ret = 0;

            try
            {
                //dt = getDataSet(getSqlCommand("SELECT COUNT(*) row_counts FROM TBL_EXCEPTIONS WHERE ISNULL(Status, 0) = 1 AND CIFID = '" + remitter_id + "'", false), "TBL_COUNTS").Tables[0];
                dt = new irsInternetPage().getExceptionCount(remitter_id, new Internet.InternetPage().getLocation()); //Added by Troy 20120904

                if (dt != null && dt.Rows.Count > 0)
                {
                    ret = Convert.ToInt32(dt.Rows[0][0]);
                }
                else
                {
                    ret = 0;
                }
            }
            catch (Exception ex)
            {
                ret = 0;
                writeLog("Remitter Exception: [RemitterID=" + remitter_id + "][Exception=" + ex.Message + "]", "REMITTER EXCEPTION", "remitter_exceptions", "", csAuditLog.Category.Select);
                return ret;
            }
            finally
            {
                /* VAPT IAG 20191007 */
                new csAuditLog().writeVAPTLogger("", "OTC", "CIF EXCEPTIONS", remitter_id, "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);

                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            writeLog("Remitter Exception Count: [RemitterID=" + remitter_id + "][ReturnValue=" + ret.ToString() + "]", "REMITTER EXCEPTION", "remitter_exceptions", "", csAuditLog.Category.Select);

            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet remitter_exception_list(string remitter_id)
        {
            //string s = "SELECT " +
            //                "A.CIFID, " +
            //                "ISNULL(UPPER(B.LastName),   '') AS LastName,   " +
            //                "ISNULL(UPPER(B.FirstName),  '') AS FirstName,  " +
            //                "ISNULL(UPPER(B.MiddleName), '') AS MiddleName, " +
            //                "ISNULL(UPPER(A.Ereason),    '') AS Ereason,    " +
            //                "ISNULL(UPPER(A.CReason),    '') AS CReason,    " +
            //                "ISNULL(UPPER(A.REMARKS),    '') AS REMARKS     " +
            //            "FROM TBL_EXCEPTIONS A " +
            //            "INNER JOIN TBL_CIF_MASTER B ON B.CIFID = A.CIFID " +
            //            "WHERE ISNULL(A.Status, 0) = 1 AND A.CIFID = '" + remitter_id + "'";

            DataSet ds = new DataSet();
            try
            {
                ds = new irsInternetPage().getException(remitter_id, new Internet.InternetPage().getLocation()); //Added by Troy 20120904
            }
            catch (Exception ex)
            {
                writeLog("Remitter Exception: [RemitterID=" + remitter_id + "][Exception=" + ex.Message + "]", "REMITTER EXCEPTION", "remitter_exception_list", "", csAuditLog.Category.Select);
            }
            return ds;
            //return getDataSet(getSqlCommand(s, true), "REMITTER_EXCEPTION_LIST");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_remitter_representative(string column_values)
        {
            return new remitters_proxy().insert_record(column_values);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String commit_transaction(String column_values)
        {
            rem_other_details rod = new rem_other_details();

            try
            {
                if (new irsInternetPage().getRecordIfDuplicate(rod.Table_Name, column_values)) //Edited by Troy 20111105. Multiple insert on table.
                {
                    return "1";
                }
                else
                {
                    if (rod.insert_record(column_values))
                    {
                        return "1"; // if successful
                    }
                    else
                    {
                        return rod.Error_Message;
                    }
                }
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
            finally
            {
                if (rod != null)
                {
                    rod = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string chk_branch_remote_allowed(string branch_code)
        {
            DataTable dt = null;

            string query = string.Empty;

            try
            {
                /* THIS IS TO CHECK IF BRANCH CODE SUPPLIED IS FROM BRANCH APPROVER OR IF BLANK GET VALUE FROM CURRENT LOG USER (PROVRBRANCHACCESS LOOKUP FLDGROUP)*/

                if (string.IsNullOrEmpty(branch_code).Equals(true))
                {
                    if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                    {
                        branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                    }
                }

                dt = base.getDataSet("otc_remote_override", true, new object[] { branch_code }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["br_allowed"]) > 0)
                    {
                        return "1~" + dt.Rows[0]["trans_status"].ToString();
                    }
                }

                return "0~0";
            }
            catch
            {
                return "0~0";
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


        /* RFC-32606 - Transaction Search and Local Transaction Override */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool validateApproverLocation(string user_name)
        {
            string tellerLocation = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            string tellerId = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;

            /* VAPT IAG 20191007 */
            string RequestTime = System.DateTime.Now.ToString();
            /*********************/

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("select Location from tbl_users where userid = '" + user_name + "'", false);
                ds = base.getDataSet(sqcommands, "tbl_users");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string approverLocation = ds.Tables[0].Rows[0][0].ToString();
                    if (approverLocation.Trim() != tellerLocation.Trim())
                    {
                        new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "USER CANNOT OVERRIDE WITH DIFFERENT LOCATION. OVERRIDE FAILED. OFFICE (" + user_name + ") IS TRYING TO OVERRIDE TRANX OF TELLER (" + tellerId + ")", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                        return false;
                    }
                    else
                    {
                        /* VAPT 20180222 */
                        new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "USER MADE AN OVERRIDE USING USER ID (" + user_name + ") ON TELLER (" + tellerId + ") ", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                        return true;
                    }
                }
                new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "USER ID OF APPROVER (" + user_name + ") IS NOT EXISTING", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                return false;
            }
            catch
            {
                new csAuditLog().writeVAPTLogger("", "CIF OVERRIDE", "UNABLE TO CONNECT TO DATABASE TO GET THE LOCATION OF APPROVER (" + user_name + ")", "", "", "", csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
                return false;
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }
        /* RFC-32606 - Transaction Search and Local Transaction Override */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] user_can_override_rates(string user_name, string user_pwd)
        {
            irsUsers irsusers = new irsUsers();
            /* START RFC-32606 - Transaction Search and Local Transaction Override */
            bool validateApproverLocation = this.validateApproverLocation(user_name);

            if (validateApproverLocation == true)
            { /* END RFC-32606 - Transaction Search and Local Transaction Override */

                Object[] ireturns = irsusers.validate(user_name, user_pwd, Convert.ToByte(1), true);

                if (ireturns != null)
                {
                    if ((bool)ireturns[0] == true)
                    {
                        if (this.can_do_override(Convert.ToInt32(irsusers.table.Rows[0]["Role"])) == true)
                        {
                            new irsOverrideLog("", "").Charges(user_name);
                            new irsOverrideLog("", "").FxRates(user_name);
                        }
                        else
                        {
                            return new Object[] { false, "INVALID ACCESS RIGHTS." };
                        }
                    }

                    return new Object[] { ireturns[0], ireturns[1] };
                }

                return new Object[] { false, "CAN NOT VALIDATE USER AT THE MOMENT." };
            }
            else
            {
                return new Object[] { false, "User Id is not allowed to approve this transaction." };
            } /* END RFC-32606 - Transaction Search and Local Transaction Override */
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_calculator_for_override_values(string irs_calculator_session_name)
        {
            if (System.Web.HttpContext.Current.Session[irs_calculator_session_name] != null)
            {
                irsCalculator calc = (irsCalculator)System.Web.HttpContext.Current.Session[irs_calculator_session_name]; return calc.get_dataset_record_for_override();
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_calculator_override_values(string override_values, string irs_calculator_session_name)
        {
            if (System.Web.HttpContext.Current.Session[irs_calculator_session_name] != null)
            {
                irsCalculator icalc = (irsCalculator)System.Web.HttpContext.Current.Session[irs_calculator_session_name]; icalc.set_dataset_record_for_override(override_values); System.Web.HttpContext.Current.Session[irs_calculator_session_name] = icalc;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_targetcurrency()
        {
            return base.getDataSet(base.getSqlCommand("SELECT Fldvalue,Fldcode FROM TBL_LOOKUP WHERE fldgroup = 'CURRENCY' AND fldvalue IN (1,2,5)", false));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cif_zipcode(string cifid)
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_getzipcode_details");
                sqcommands.Parameters.Add("@cifid", SqlDbType.VarChar).Value = cifid;

                ds = base.getDataSet(sqcommands, "ZIPCODE");

                if (ds != null)
                {
                    return ds;
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

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_branch_info(String SEQ_ID)
        {
            String sql = "SELECT UPPER(BranchCode) AS BranchCode, UPPER(BranchName) AS BranchName FROM TBL_BRANCHES WHERE SEQ_ID = " + SEQ_ID;
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand(sql, false);
                ds = base.getDataSet(sqcommands, "TBL_BRANCHES");

                if (ds != null)
                {
                    return ds.Tables[0];
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
        public object[] user_can_log_exceptions(string user_name, string user_pwd)
        {
            irsUsers irsusers = new irsUsers();

            Object[] ireturns = irsusers.validate(user_name, user_pwd, Convert.ToByte(1), true);

            if (ireturns != null)
            {
                if ((bool)ireturns[0] == true)
                {
                    //Edited by Troy 20120904
                    int i = 0;
                    DataTable dt = new irsInternetPage().getUserCanOverride();
                    bool bOk = false;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (i = 0; i <= (dt.Rows.Count - 1); i++)
                        {
                            if (dt.Rows[i]["FldValue"].ToString().Trim() == irsusers.table.Rows[0]["Role"].ToString().Trim())
                            {
                                bOk = true;
                            }
                        }
                    }
                    else
                    {
                        return new Object[] { false, "Invalid access rights." };
                    }
                    if (bOk == true)
                    {
                        new irsOverrideLog("", "").ExceptionsRemitter(user_name);
                    }
                    else
                    {
                        return new Object[] { false, "Invalid access rights." };
                    }

                    //if (Convert.ToInt32(irsusers.table.Rows[0]["Role"]) == 4 || Convert.ToInt32(irsusers.table.Rows[0]["Role"]) == 8) //Edited by Troy 20120904
                    //{
                    //    new irsOverrideLog("", "").ExceptionsRemitter(user_name);
                    //}
                    //else
                    //{
                    //    return new Object[] { false, "Invalid access rights." };
                    //}
                }

                return new Object[] { ireturns[0], ireturns[1] };
            }

            return new Object[] { false, "Can not validate user at the moment." };
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] user_can_waived_charges(string user_name, string user_pwd)
        {
            bool validateApproverLocation = this.validateApproverLocation(user_name);
            if (validateApproverLocation == true)
            {
                irsUsers irsusers = new irsUsers();

                Object[] ireturns = irsusers.validate(user_name, user_pwd, Convert.ToByte(1), true);

                if (ireturns != null)
                {
                    if ((bool)ireturns[0] == true)
                    {
                        if (this.can_do_override(Convert.ToInt32(irsusers.table.Rows[0]["Role"])) == true)
                        {
                            new irsOverrideLog("", "").WaiveCharges(user_name);
                        }
                        else
                        {
                            return new Object[] { false, "Invalid access rights." };
                        }
                    }

                    return new Object[] { ireturns[0], ireturns[1] };
                }

                return new Object[] { false, "Can not validate user at the moment." };
            }
            else { return new Object[] { false, "User Id is not allowed to approve this transaction." }; }
            /* RFC-32606 - Transaction Search and Local Transaction Override */
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] user_can_exec_aggregates(string user_name, string user_pwd)
        {
			/* START RFC-32606 - Transaction Search and Local Transaction Override */
            bool validateApproverLocation = this.validateApproverLocation(user_name);

            if (validateApproverLocation == true)
            { 
			/* END RFC-32606 - Transaction Search and Local Transaction Override */
            irsUsers irsusers = new irsUsers();

            Object[] ireturns = irsusers.validate(user_name, user_pwd, Convert.ToByte(1), true);

            if (ireturns != null)
            {
                if ((bool)ireturns[0] == true)
                {
                    //if (this.chk_ho_approver(user_name, user_pwd) == 1)
                    //{
                    //    new irsOverrideLog("", "").RemittanceLimit(user_name); return new Object[] { ireturns[0], ireturns[1] };
                    //}

                    if (this.can_do_override(Convert.ToInt32(irsusers.table.Rows[0]["Role"])) == true)
                    {
                        new irsOverrideLog("", "").RemittanceLimit(user_name);
                    }
                    else
                    {
                        return new Object[] { false, "Invalid access rights." };
                    }
                }

                return new Object[] { ireturns[0], ireturns[1] };
            }

            return new Object[] { false, "Can not validate user at the moment." };
			/* START RFC-32606 - Transaction Search and Local Transaction Override */
            }
            else
            {
                return new Object[] { false, "User Id is not allowed to approve this transaction." };
            } /* END RFC-32606 - Transaction Search and Local Transaction Override */

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string compute_waived(int product_charge_id, int account_currency, decimal remit_amount, string irs_calculator_session_name)
        {
            irsCalculator calc = null; int computation_direction = 3;

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

                calc.compute_waived_charges(remit_amount, false);

                System.Web.HttpContext.Current.Session[irs_calculator_session_name] = calc;

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
            catch
            {
                return "Calculator not responding.";
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
        public Object[] get_logged_data()
        {
            irsClientData loggin_data = null;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }

            if (loggin_data != null)
            {
                return new Object[] { loggin_data.Location, new irsTimeZone().branchLocalTime(loggin_data.Location).ToString("MM/dd/yyyy") };
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_search_sys_criteria()
        {
            //return base.getDataSet(base.getSqlCommand("SELECT Fldvalue,Fldcode FROM TBL_LOOKUP WHERE fldgroup = 'CURRENCY' AND fldvalue IN (1,2,5)", false));
            //return base.getDataSet("select FldDescription, FldValue from tbl_lookup where fldgroup = 'irsworldsystem'", false, new Object[] { });
            String branchcode = "";
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                branchcode = loggin_data.Location;
            }

            return base.getDataSet("proc_get_search_sys_criteria", true, new Object[] { branchcode });
        }

        /*RFS SANCTION LIST DISCOUNTING EUROPE JULY 2016 */

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
        //=========================================================================================

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool discounting_required()
        {
            return this.sanction_discount_required();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object[] user_can_override_hit_list_discounting(string user_name, string user_pwd, object sanction_discounting_reason, object sanction_discounting_reason_other, object sanction_discounting_reason_bus, object sanction_disposition_reason, object sanction_disposition_reason_remarks, object remitterid, object beneficiaryid, int generic_flag)
        {
            /* START RFC-32606 - Transaction Search and Local Transaction Override */
            bool validateApproverLocation = this.validateApproverLocation(user_name);

            if (validateApproverLocation == true)
            { /* END RFC-32606 - Transaction Search and Local Transaction Override */
                bool discounting_required = false;

                //=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */validate_watchlist
                string retval_watchlist = "";
                System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"] = null; //Teller Reason for Discounting Individual
                System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_BUS"] = null; //Teller Reason for Discounting Business
                System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_OTHERS"] = null; //Teller Other Discounting Remarks
                System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"] = null; //Approver ID
                System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION"] = null; //Approver Disposition Individual
                System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION_REMARKS"] = null; //Approver Disposition Remarks
                                                                                                               //=========================================================================================

                if (this.sanction_discount_required() == true)
                {
                    discounting_required = true;

                    if (generic_flag == 2)
                    {
                        //=========================================================================================
                        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                        //if (string.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason)).Equals(true) || String.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason).Trim()) == true || Convert.ToString(sanction_discounting_reason).Trim() == "" || sanction_discounting_reason == null || Convert.ToString(sanction_discounting_reason).Trim().ToUpper() == "NULL")
                        //{
                        //    return new Object[] { false, "Please input Reason for Discounting." };
                        //}

                        //if (Convert.ToString(sanction_discounting_reason).Trim() == "99")
                        //{
                        //    if (string.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason_other)).Equals(true) || String.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason_other).Trim()) == true || Convert.ToString(sanction_discounting_reason_other).Trim() == "" || sanction_discounting_reason_other == null || Convert.ToString(sanction_discounting_reason_other).Trim().ToUpper() == "NULL")
                        //    {
                        //        return new Object[] { false, "Please input Reason Other Details." };
                        //    }
                        //}

                        if (string.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason)).Equals(true) || String.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason).Trim()) == true || Convert.ToString(sanction_discounting_reason).Trim() == "" || sanction_discounting_reason == null || Convert.ToString(sanction_discounting_reason).Trim().ToUpper() == "NULL")
                        {
                            sanction_discounting_reason = "";
                        }

                        if (string.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason_bus)).Equals(true) || String.IsNullOrEmpty(Convert.ToString(sanction_discounting_reason_bus).Trim()) == true || Convert.ToString(sanction_discounting_reason_bus).Trim() == "" || sanction_discounting_reason_bus == null || Convert.ToString(sanction_discounting_reason_bus).Trim().ToUpper() == "NULL")
                        {
                            sanction_discounting_reason_bus = "";
                        }

                        if (string.IsNullOrEmpty(Convert.ToString(sanction_disposition_reason)).Equals(true) || String.IsNullOrEmpty(Convert.ToString(sanction_disposition_reason).Trim()) == true || Convert.ToString(sanction_disposition_reason).Trim() == "" || sanction_disposition_reason == null || Convert.ToString(sanction_disposition_reason).Trim().ToUpper() == "NULL")
                        {
                            sanction_disposition_reason = "";
                        }
                        retval_watchlist = Convert.ToString(validate_watchlist_refinitiv(remitterid.ToString(), beneficiaryid.ToString(), sanction_discounting_reason.ToString(), sanction_discounting_reason_bus.ToString(), sanction_disposition_reason.ToString(), sanction_disposition_reason_remarks.ToString(), "2"));
                       // retval_watchlist = Convert.ToString(validate_watchlist(remitterid.ToString(), beneficiaryid.ToString(), sanction_discounting_reason.ToString(), sanction_discounting_reason_bus.ToString(), sanction_disposition_reason.ToString(), sanction_disposition_reason_remarks.ToString(), "2"));
                        if (retval_watchlist.Trim() != "")
                        {
                            return new Object[] { false, retval_watchlist };
                        }
                        //=========================================================================================
                    }
                }

                irsUsers irsusers = new irsUsers();

                Object[] ireturns = irsusers.validate(user_name, user_pwd, Convert.ToByte(1), true);

                if (ireturns != null)
                {
                    if ((bool)ireturns[0] == true)
                    {
                        if (discounting_required == true)
                        {
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"] = sanction_discounting_reason;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_OTHERS"] = sanction_discounting_reason_other;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"] = user_name;
                            //=========================================================================================
                            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_BUS"] = sanction_discounting_reason_bus;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION"] = sanction_disposition_reason;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION_REMARKS"] = sanction_disposition_reason_remarks;
                            //=========================================================================================
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"] = user_name;
                        }

                        /* VAPT 20190422 */
                        string s = "SELECT COUNT (*) FROM tbl_lookup WHERE FLDGROUP = 'ApproverGroup' AND FLDCODE = '" + irsusers.table.Rows[0]["Role"].ToString() + "'";

                        if (Convert.ToInt32(getDataSet(getSqlCommand(s, false), "ROLE_MODEL").Tables[0].Rows[0][0]) > 0)
                        {
                            //new irsOverrideLog("", "").StreamLining(user_name);
                            new irsOverrideLog("", "").Refinitiv(user_name);
                        }
                        else
                        {
                            //=========================================================================================
                            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"] = null;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_OTHERS"] = null;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"] = null;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_BUS"] = null;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION"] = null;
                            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION_REMARKS"] = null;
                            //=========================================================================================
                            return new Object[] { false, "Invalid access rights." };
                        }
                    }

                    return new Object[] { ireturns[0], ireturns[1] };
                }

                return new Object[] { false, "Can not validate user at the moment." };
            }
            /* START RFC-32606 - Transaction Search and Local Transaction Override */
            else
            {
                return new Object[] { false, "User Id is not allowed to approve this transaction." };
            } /* END RFC-32606 - Transaction Search and Local Transaction Override */
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string validate_watchlist_refinitiv(string remitterid, string beneficiaryid, string discounting_individual, string discounting_business, string disposition, string disposition_remarks, string inquirytype)
        {
            string retval = "";
            string retval_rem = "";
            string retval_ben = "";
            int remitterciftype = 0;
            int beneficiaryciftype = 0;
            string cifid = "";
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsRefinitiv = new irsRefinitiv();
            DataTable dt_refinitiv = new DataTable();
            DataTable dtCaseSytemID = new DataTable();
            DataTable dt_remitterciftype = new DataTable();
            DataTable dt_beneficiaryciftype = new DataTable();
            dt_refinitiv = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];

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
                            if (inquirytype == "1")
                            {
                                if (remitterid == cifid)
                                {
                                    retval_rem = "0";
                                    string sql_remitter = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + remitterid + "' AND isnull(deleted,0) = 0";
                                    dt_remitterciftype = getDataSet(getSqlCommand(sql_remitter, false)).Tables[0];
                                    remitterciftype = Convert.ToInt32(dt_remitterciftype.Rows[0]["ciftype"]);
                                    retval_rem = Convert.ToString(remitterciftype);
                                }
                                if (beneficiaryid == cifid)
                                {
                                    retval_ben = "0";
                                    string sql_beneficiary = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + beneficiaryid + "' AND isnull(deleted,0) = 0";
                                    dt_beneficiaryciftype = getDataSet(getSqlCommand(sql_beneficiary, false)).Tables[0];
                                    beneficiaryciftype = Convert.ToInt32(dt_beneficiaryciftype.Rows[0]["ciftype"]);
                                    retval_ben = Convert.ToString(beneficiaryciftype);
                                }
                                if (retval_rem == "") { retval_rem = "0"; }
                                if (retval_ben == "") { retval_ben = "0"; }
                            }
                            else if (inquirytype == "2")
                            {
                                if (remitterid == cifid)
                                {
                                    string sql_remitter = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + remitterid + "' AND isnull(deleted,0) = 0";
                                    dt_remitterciftype = getDataSet(getSqlCommand(sql_remitter, false)).Tables[0];
                                    remitterciftype = Convert.ToInt32(dt_remitterciftype.Rows[0]["ciftype"]);
                                    //retval_rem = Convert.ToString(remitterciftype);
                                    if (remitterciftype == 1)
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
                                        else if (discounting_individual == "" && disposition == "99" && disposition_remarks != "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Individual)";
                                        }
                                        else if (discounting_individual != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_rem = "Please provide Approver Remarks.";
                                        }
                                        else if (discounting_individual == "" && disposition != "99" && disposition != "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Individual)";
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
                                        else if (discounting_business == "" && disposition == "99" && disposition_remarks != "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Business)";
                                        }
                                        else if (discounting_business != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_rem = "Please provide Approver Remarks.";
                                        }
                                        else if (discounting_business == "" && disposition != "99" && disposition != "")
                                        {
                                            retval_rem = "Please provide Teller Reason for Discounting (Business)";
                                        }
                                    }
                                }
                                if (beneficiaryid == cifid)
                                {
                                    string sql_beneficiary = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + beneficiaryid + "' AND isnull(deleted,0) = 0";
                                    dt_beneficiaryciftype = getDataSet(getSqlCommand(sql_beneficiary, false)).Tables[0];
                                    beneficiaryciftype = Convert.ToInt32(dt_beneficiaryciftype.Rows[0]["ciftype"]);
                                    //retval_ben = Convert.ToString(beneficiaryciftype);
                                    if (beneficiaryciftype == 1)
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
                                        else if (discounting_individual == "" && disposition == "99" && disposition_remarks != "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Individual)";
                                        }
                                        else if (discounting_individual != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_ben = "Please provide Approver Remarks.";
                                        }
                                        else if (discounting_individual == "" && disposition != "99" && disposition != "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Individual)";
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
                                        else if (discounting_business == "" && disposition == "99" && disposition_remarks != "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Business)";
                                        }
                                        else if (discounting_business != "" && disposition == "99" && disposition_remarks == "")
                                        {
                                            retval_ben = "Please provide Approver Remarks.";
                                        }
                                        else if (discounting_business == "" && disposition != "99" && disposition != "")
                                        {
                                            retval_ben = "Please provide Teller Reason for Discounting (Business)";
                                        }

                                    }
                                }

                            }
                        }
                    }
                    if (inquirytype == "1")
                    {
                        if (retval_rem == retval_ben)
                        {
                            retval = retval_rem + "~0";
                        }
                        else
                        {
                            retval = retval_rem + "~" + retval_ben;
                        }
                    }
                    if (inquirytype == "2")
                    {
                        if (retval_rem != "")
                        {
                            if (retval_ben != "")
                            {
                                if (retval_rem == retval_ben)
                                {
                                    retval = retval_rem;
                                }
                                else
                                {
                                    retval = retval_rem + "~" + retval_ben;
                                }
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
                writeLog("validate_watchlist_refinitiv", "WATCH LIST", e.Message + " REMIITER ID: " + remitterid + " |BENEFICIARY ID: " + beneficiaryid, "", csAuditLog.Category.Insert);
            }
            return retval;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string validate_watchlist_refinitivisdown(string remitterid, string beneficiaryid, string discounting_individual, string discounting_business, string disposition, string disposition_remarks, string inquirytype)
        {
            string retval = "";
            string retval_rem = "";
            string retval_ben = "";
            int remitterciftype = 0;
            int beneficiaryciftype = 0;
            string cifid = "";
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsRefinitiv = new irsRefinitiv();
            DataTable dt_refinitiv = new DataTable();
            DataTable dtCaseSytemID = new DataTable();
            DataTable dt_remitterciftype = new DataTable();
            DataTable dt_beneficiaryciftype = new DataTable();
            //dt_refinitiv = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];

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
                if (inquirytype == "1")
                {
                    if (remitterid != "")
                    {
                        retval_rem = "0";
                        string sql_remitter = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + remitterid + "' AND isnull(deleted,0) = 0";
                        dt_remitterciftype = getDataSet(getSqlCommand(sql_remitter, false)).Tables[0];
                        remitterciftype = Convert.ToInt32(dt_remitterciftype.Rows[0]["ciftype"]);
                        retval_rem = Convert.ToString(remitterciftype);
                    }
                    if (beneficiaryid != "")
                    {
                        retval_ben = "0";
                        string sql_beneficiary = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + beneficiaryid + "' AND isnull(deleted,0) = 0";
                        dt_beneficiaryciftype = getDataSet(getSqlCommand(sql_beneficiary, false)).Tables[0];
                        beneficiaryciftype = Convert.ToInt32(dt_beneficiaryciftype.Rows[0]["ciftype"]);
                        retval_ben = Convert.ToString(beneficiaryciftype);
                    }
                    if (retval_rem == "") { retval_rem = "0"; }
                    if (retval_ben == "") { retval_ben = "0"; }
                }
                else if (inquirytype == "2")
                {
                    if (remitterid != "")
                    {
                        string sql_remitter = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + remitterid + "' AND isnull(deleted,0) = 0";
                        dt_remitterciftype = getDataSet(getSqlCommand(sql_remitter, false)).Tables[0];
                        remitterciftype = Convert.ToInt32(dt_remitterciftype.Rows[0]["ciftype"]);
                        //retval_rem = Convert.ToString(remitterciftype);
                        if (remitterciftype == 1)
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
                            else if (discounting_individual == "" && disposition == "99" && disposition_remarks != "")
                            {
                                retval_rem = "Please provide Teller Reason for Discounting (Individual)";
                            }
                            else if (discounting_individual != "" && disposition == "99" && disposition_remarks == "")
                            {
                                retval_rem = "Please provide Approver Remarks.";
                            }
                            else if (discounting_individual == "" && disposition != "99" && disposition != "")
                            {
                                retval_rem = "Please provide Teller Reason for Discounting (Individual)";
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
                            else if (discounting_individual == "" && disposition == "99" && disposition_remarks != "")
                            {
                                retval_rem = "Please provide Teller Reason for Discounting (Business)";
                            }
                            else if (discounting_individual != "" && disposition == "99" && disposition_remarks == "")
                            {
                                retval_rem = "Please provide Approver Remarks.";
                            }
                            else if (discounting_individual == "" && disposition != "99" && disposition != "")
                            {
                                retval_rem = "Please provide Teller Reason for Discounting (Business)";
                            }
                        }
                    }
                    if (beneficiaryid != "")
                    {
                        string sql_beneficiary = "SELECT TOP 1 ciftype FROM TBL_CIF_MASTER WHERE cifid = '" + beneficiaryid + "' AND isnull(deleted,0) = 0";
                        dt_beneficiaryciftype = getDataSet(getSqlCommand(sql_beneficiary, false)).Tables[0];
                        beneficiaryciftype = Convert.ToInt32(dt_beneficiaryciftype.Rows[0]["ciftype"]);
                        //retval_ben = Convert.ToString(beneficiaryciftype);
                        if (beneficiaryciftype == 1)
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
                            else if (discounting_individual == "" && disposition == "99" && disposition_remarks != "")
                            {
                                retval_ben = "Please provide Teller Reason for Discounting (Individual)";
                            }
                            else if (discounting_individual != "" && disposition == "99" && disposition_remarks == "")
                            {
                                retval_ben = "Please provide Approver Remarks.";
                            }
                            else if (discounting_individual == "" && disposition != "99" && disposition != "")
                            {
                                retval_ben = "Please provide Teller Reason for Discounting (Individual)";
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
                            else if (discounting_business == "" && disposition == "99" && disposition_remarks != "")
                            {
                                retval_ben = "Please provide Teller Reason for Discounting (Business)";
                            }
                            else if (discounting_business != "" && disposition == "99" && disposition_remarks == "")
                            {
                                retval_ben = "Please provide Approver Remarks.";
                            }
                            else if (discounting_business == "" && disposition != "99" && disposition != "")
                            {
                                retval_ben = "Please provide Teller Reason for Discounting (Business)";
                            }
                        }
                    }
                }
                if (inquirytype == "1")
                {
                    if (retval_rem == retval_ben)
                    {
                        retval = retval_rem + "~0";
                    }
                    else
                    {
                        retval = retval_rem + "~" + retval_ben;
                    }
                }
                if (inquirytype == "2")
                {
                    if (retval_rem != "")
                    {
                        if (retval_ben != "")
                        {
                            if (retval_rem == retval_ben)
                            {
                                retval = retval_rem;
                            }
                            else
                            {
                                retval = retval_rem + "~" + retval_ben;
                            }
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
            catch (Exception e)
            {
                writeLog("validate_watchlist_refinitivisdown", "WATCH LIST", e.Message + " REMIITER ID: " + remitterid + " |BENEFICIARY ID: " + beneficiaryid, "", csAuditLog.Category.Insert);
            }
            return retval;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string validate_watchlist(string remitterid, string beneficiaryid, string discounting_individual, string discounting_business, string disposition, string disposition_remarks, string inquirytype)
        {
            DataTable dt = new DataTable();
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            Refinitiv refinitivcs = new Refinitiv();
            string isRefinitivEnabled = "0";
            string retval = "";

            try
            {
                isRefinitivEnabled = refinitivcs.isRefinitivEnabled().ToString();
                if (isRefinitivEnabled == "1")
                {
                    dt = commons.execStoredProcedureTable("proc_validate_watchlist_details_refinitiv", new object[] { remitterid, beneficiaryid, discounting_individual, discounting_business, disposition, disposition_remarks, Convert.ToInt32(inquirytype) });

                }
                else
                {
                    dt = commons.execStoredProcedureTable("proc_validate_watchlist_details", new object[] { remitterid, beneficiaryid, discounting_individual, discounting_business, disposition, disposition_remarks, Convert.ToInt32(inquirytype) });
                }

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

        ////=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        public void insert_sanction_list_discounting(string client_cifid, string approver_id, string sanction_list_discounting, string sanction_list_discounting_bus, string sanction_list_discounting_others, string sanction_list_disposition, string sanction_list_disposition_remarks, string watchlist, string transaction_id)
        {
            DateTime date_created_discounting = DateTime.Now;
            Refinitiv refinitivcs = new Refinitiv();
            //string procedure_name = "sanction_list_discounting";
            string procedure_name = "";
            string teller_id = string.Empty;

            string isRefinitivEnabled = refinitivcs.isRefinitivEnabled().ToString();
            if (isRefinitivEnabled == "1")
            {
                procedure_name = "sanction_list_discounting_refinitiv";
            }
            else { procedure_name = "sanction_list_discounting"; }


            try
            {
                if (this.sanction_discount_required() == false)
                {
                    return;
                }

                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"].ToString()).Equals(false))
                    {
                        sanction_list_discounting = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"];
                    }
                }

                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_OTHERS"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_OTHERS"].ToString()).Equals(false))
                    {
                        sanction_list_discounting_others = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_OTHERS"];
                    }
                }

                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"].ToString()).Equals(false))
                    {
                        approver_id = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"];
                    }
                }

                ////=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_BUS"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_BUS"].ToString()).Equals(false))
                    {
                        sanction_list_discounting_bus = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_BUS"];
                    }
                }

                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION"].ToString()).Equals(false))
                    {
                        sanction_list_disposition = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION"];
                    }
                }

                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION_REMARKS"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION_REMARKS"].ToString()).Equals(false))
                    {
                        sanction_list_disposition_remarks = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION_REMARKS"];
                    }
                }

                //if (string.IsNullOrEmpty(sanction_list_discounting).Equals(true))
                //{
                //    return;
                //}
                ////=========================================================================================

                if (irsClientData != null)
                {
                    teller_id = irsClientData.UserId;
                    date_created_discounting = new irsTimeZone().branchLocalTime(irsClientData.Location);
                }

                ////=========================================================================================
                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                //int i = base.execStoredProcedureUpdate(procedure_name, new object[] { client_cifid, teller_id, approver_id, sanction_list_discounting, date_created_discounting, date_created_discounting, watchlist, sanction_list_discounting_others, transaction_id });
                int i = base.execStoredProcedureUpdate(procedure_name, new object[] { client_cifid, teller_id, approver_id, sanction_list_discounting, date_created_discounting, date_created_discounting, watchlist, sanction_list_discounting_others, transaction_id, sanction_list_discounting_bus, sanction_list_disposition, sanction_list_disposition_remarks });
                ////=========================================================================================

                if (i > 0)
                {
                    writeLog("SLIST_DISCOUNTING", "WATCH LIST", " SANCTION LIST DISCOUNTING OK. CIFID: " + client_cifid + " APPROVER: " + approver_id + " TELLER ID: " + teller_id, "", csAuditLog.Category.Insert);
                }
                else
                {
                    writeLog("SLIST_DISCOUNTING", "WATCH LIST", " SANCTION LIST DISCOUNTING NO ROWS SAVED. CIFID: " + client_cifid + " APPROVER: " + approver_id + " TELLER ID: " + teller_id + " ERROR: " + base.Message, "", csAuditLog.Category.Insert);
                }
            }
            catch (Exception e)
            {
                writeLog("SLIST_DISCOUNTING", "WATCH LIST", e.Message + " CIFID: " + client_cifid + " APPROVER: " + approver_id + " TELLER ID: " + teller_id, "", csAuditLog.Category.Insert);
            }
        }

        public void insert_sanction_list_discounting_cif(string client_cifid, string approver_id, string sanction_list_notes)
        {
            DateTime date_created_discounting = DateTime.Now;

            //string procedure_name = "sanction_list_discounting_cif";
            string teller_id = string.Empty;

            Refinitiv refinitivcs = new Refinitiv();
            string procedure_name = "";
            string isRefinitivEnabled = refinitivcs.isRefinitivEnabled().ToString();
            if (isRefinitivEnabled == "1")
            {
                procedure_name = "sanction_list_discounting_cif_refinitiv";
            }
            else { procedure_name = "sanction_list_discounting_cif"; }

            try
            {
                /** DISCOUNTING NOTES (NOT APPLICABLE DURING ONBOARDING)
                if (this.sanction_discount_required() == false)
                {
                    return;
                }

                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"].ToString()).Equals(false))
                    {
                        sanction_list_notes = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"];
                    }
                }
                **/

                if (System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"] != null)
                {
                    if (string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"].ToString()).Equals(false))
                    {
                        approver_id = (string)System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"];
                    }
                }

                /** CHECKING OF NOTES (NOT APPLICABLE DURING ONBOARDING)
                if (string.IsNullOrEmpty(sanction_list_notes).Equals(true))
                {
                    return;
                }
                **/

                if (irsClientData != null)
                {
                    teller_id = irsClientData.UserId;
                    date_created_discounting = new irsTimeZone().branchLocalTime(irsClientData.Location);
                }

                int i = base.execStoredProcedureUpdate(procedure_name, new object[] { client_cifid, teller_id, approver_id, sanction_list_notes, date_created_discounting, date_created_discounting });

                if (i > 0)
                {
                    writeLog("SLIST_DISCOUNTING", "WATCH LIST", " SANCTION LIST DISCOUNTING OK. CIFID: " + client_cifid + " APPROVER: " + approver_id + " TELLER ID: " + teller_id, "", csAuditLog.Category.Insert);
                }
                else
                {
                    writeLog("SLIST_DISCOUNTING", "WATCH LIST", " SANCTION LIST DISCOUNTING NO ROWS SAVED. CIFID: " + client_cifid + " APPROVER: " + approver_id + " TELLER ID: " + teller_id + " ERROR: " + base.Message, "", csAuditLog.Category.Insert);
                }
            }
            catch (Exception e)
            {
                writeLog("SLIST_DISCOUNTING", "WATCH LIST", e.Message + " CIFID: " + client_cifid + " APPROVER: " + approver_id + " TELLER ID: " + teller_id, "", csAuditLog.Category.Insert);
            }
        }

        private bool sanction_discount_required()
        {
            DataTable dt = null;

            try
            {
                if (irsClientData != null)
                {
                    dt = getDataSet("sanction_discounting_required", true, new object[] { irsClientData.Location }).Tables[0];

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            return true;
                        }
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
        public void clear_pending_tranx_session()
        {
            //=========================================================================================
            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
            //System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"] = null;
            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_OTHERS"] = null;
            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_APPROVER"] = null;
            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION"] = null;
            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISPOSITION_REMARKS"] = null;
            //=========================================================================================
        }

        //=========================================================================================
        //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_pending_tranx_session(string sanction_discounting_reason, string sanction_discounting_reason_bus)
        {
            if (sanction_discounting_reason.Trim() == "null" || sanction_discounting_reason.Trim() == "NULL")
            {
                sanction_discounting_reason = "";
            }
            if (sanction_discounting_reason_bus.Trim() == "null" || sanction_discounting_reason_bus.Trim() == "NULL")
            {
                sanction_discounting_reason_bus = "";
            }

            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING"] = sanction_discounting_reason.Trim();
            System.Web.HttpContext.Current.Session["SANCTION_LIST_REASON_FOR_DISCOUNTING_BUS"] = sanction_discounting_reason_bus.Trim();
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

        //======================================================================
        //RFC-6084 : PhilSys No in IRSW 08252021
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_philsys_max_length()
        {
            DataTable dt = new DataTable();
            int philsys_max_length = 0;

            try
            {
                dt = execStoredProcedureTable("isp_getLookup", new string[] { "PhilSysNoMaxLength" });
                if (dt != null && dt.Rows.Count > 0)
                {
                    philsys_max_length = Convert.ToInt32(dt.Rows[0]["FldCode"].ToString());
                }
            }
            catch
            { }
            finally { dt = null; }
            return philsys_max_length;
        }

        //======================================================================
        /*END HERE*/
    }

    [Serializable()]
    public class rem_account_screen : sqlHelperWs
    {

        private string seq_id = null;
        private string error_message = null;

        public rem_account_screen()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;

            //commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_rural_bank_account(string beneficiary_cifid, string bank_branch_account, string account_number, int account_currency, int account_type)
        {
            return new rem_account_rural_bank().insert_new_accounts_rural_bank(beneficiary_cifid, bank_branch_account, account_number, account_currency, account_type);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_new_branch(string branch_code, string branch_name, int branch_type)
        {
            return new rem_new_branches().insert_new_branch(branch_code, branch_name, branch_type);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_server_error_messages()
        {
            this.error_message = (string)System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"];

            if (this.error_message != null)
            {
                if (this.error_message != string.Empty && this.error_message.Length > 0)
                {
                    return this.error_message;
                }
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneName(String CIFID)
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("select CIFID, LastName, FirstName, MiddleName from tbl_cif_master where cifid = '" + CIFID + "'", false);
                ds = base.getDataSet(sqcommands, "tbl_cif_master");

                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0];
                    }
                    else
                    {
                        throw new Exception("Cannot find Beneficiary CIFID");
                    }
                }
                else
                {
                    throw new Exception("Cannot find Beneficiary CIFID");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get beneficiary info, Reason : " + ex.Message);
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
        public bool has_accounts(string cifid, int branch_type)
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_get_accountlist");
                sqcommands.Parameters.Add("@v_CIFID", SqlDbType.VarChar).Value = cifid;
                sqcommands.Parameters.Add("@v_branch_type", SqlDbType.BigInt).Value = branch_type;

                ds = base.getDataSet(sqcommands, "ACCOUNTS");

                if (ds != null && ds.Tables[0].Rows.Count > 0)
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
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_dropdown_data(int branch_type, int pddts_flag)
        {
            /* pddts_flag: 0 = ALL OTHER BANKS; 1 = PDDTS; 2 = NON PDDTS */

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_otc_dropdown_data");
                sqcommands.Parameters.Add("@branch_type", SqlDbType.Int).Value = branch_type;
                sqcommands.Parameters.Add("@pddts_flag", SqlDbType.Int).Value = pddts_flag;

                ds = base.getDataSet(sqcommands, "LOOKUP");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDDLData(int Branch_Type)
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_get_rem_modal_accounts_ddl");
                sqcommands.Parameters.Add("@v_branch_type", SqlDbType.VarChar).Value = Branch_Type;

                ds = base.getDataSet(sqcommands, "LOOKUP");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
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
        public DataTable getAccountInfo(int acctSeqID)
        {
            string sql = string.Empty;

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {

                sql = "SELECT A.SEQ_ID, CIFID, A.AccountNo, A.DomBankCode, A.Remarks, A.Status, A.ForBankCode, A.Deleted, A.isGFC, A.KBIAccount, A.OtherBankSeqId, A.BranchType, " +
                         "      ISNULL(A.AccountType, 0) AS AccountType, " +
                         "      ISNULL(ISNULL(NULLIF(A.BranchCode, ''), NULLIF(A.DomBankCode, '')), B.BranchCode) AS BranchCode, " +
                         "      ISNULL(Currency, 0) AS Currency, A.branchname " +
                         "FROM TBL_CIF_ACCOUNTS A " +
                         "LEFT JOIN TBL_BRANCHES B ON B.SEQ_ID = A.OtherBankSeqId " +
                         "LEFT JOIN TBL_BRANCHES C ON C.BranchCode = ISNULL(ISNULL(NULLIF(A.BranchCode, ''), NULLIF(A.DomBankCode, '')), B.BranchCode) " +
                         "WHERE A.SEQ_ID = " + acctSeqID.ToString();


                sqcommands = base.getSqlCommand(sql, false);
                ds = base.getDataSet(sqcommands, "TBL_CIF_ACCOUNTS");

                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch
            {
                throw new Exception("No dataset returned");
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record(string column_values)
        {
            bool ibool = false;

            string[] s = column_values.Split('|');

            if (base.newRow() == true)
            {
                if (this.duplicate_account(s) == false)
                {
                    ibool = this.set_record_and_commit(s, false);
                }
                else
                {
                    ibool = false; System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = "DUPLICATE ACCOUNT ENTRY.";
                }
            }

            if (ibool == false)
            {
                this.get_error_dbase();
                this.rollback_record();
            }

            return ibool;
        }

        private bool commit_record()
        {
            System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = null;

            if (base.save() == true)
            {
                System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = Convert.ToInt32(base.IdentityValue);
                System.Web.HttpContext.Current.Session["TRAN_COLUMN_SESSION"] = null;

                this.seq_id = base.IdentityValue.ToString();
                return true;
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_seq_id()
        {
            return System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"].ToString();
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool duplicate_account(string[] column_values)
        {
            DataTable dt = null;

            string account_no = string.Empty;
            string account_type = string.Empty;
            string currency = string.Empty;
            string branch_code = string.Empty;
            string branch_type = string.Empty;
            string cifid = string.Empty;

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0 && s[0] != null)
                {
                    if (s[0] == "CIFID")
                    {
                        cifid = s[1].Replace("'", "''");
                    }
                    if (s[0] == "AccountNo")
                    {
                        account_no = s[1].Replace("'", "''");
                    }
                    else if (s[0] == "AccountType")
                    {
                        account_type = s[1].Replace("'", "''");
                    }
                    else if (s[0] == "Currency")
                    {
                        currency = s[1].Replace("'", "''");
                    }
                    else if (s[0] == "BranchCode")
                    {
                        branch_code = s[1].Replace("'", "''");
                    }
                    else if (s[0] == "BranchType")
                    {
                        branch_type = s[1].Replace("'", "''");
                    }
                }
            }

            string sql = "SELECT COUNT(*) AS account_count FROM TBL_CIF_ACCOUNTS WHERE ";

            sql += "AccountNo   = '" + account_no + "' AND ";
            sql += "CIFID       = '" + cifid + "' AND ";
            sql += "AccountType = " + account_type + " AND ";
            sql += "Currency    = " + currency + " AND ";
            sql += "BranchType  = " + branch_type + " AND ";

            if (Convert.ToInt32(branch_type) == 2)
            {
                sql += "BranchCode = '" + branch_code + "'";
            }
            else
            {
                sql += "OtherBankSeqId = " + branch_code;
            }

            try
            {
                dt = getDataSet(getSqlCommand(sql, false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["account_count"]) > 0)
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

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();

            bool isPNB = false;

            string[] branchType = column_values[5].Split('~');

            if (branchType[1].ToString() == "2")
            {
                isPNB = true;
            }

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[0] == "AccountNo" && isPNB)
                    {
                        if (!new irsAccountAlgo().checkAccount(s[1]))
                        {
                            error_list.Add("INVALID ACCOUNT NUMBER"); continue;
                        }
                    }

                    if (s[0] == "BranchCode")
                    {
                        if (isPNB == false)
                        {
                            DataTable dt = null;

                            if (s[1] != null && s[1].Trim().Length > 0)
                            {
                                dt = getDataSet(getSqlCommand("SELECT * FROM TBL_BRANCHES WHERE BranchType = 5 AND BranchCode = '" + s[1].Replace("'", "''").Trim() + "'", false)).Tables[0];
                            }

                            if (!base.setColumn(s[0].Trim(), s[1]))
                            {
                                error_list.Add(s[0] + ":" + this.Message);
                            }

                            if (dt != null)
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    if (!base.setColumn("OtherBankSeqId", Convert.ToInt32(dt.Rows[0]["SEQ_ID"])))
                                    {
                                        error_list.Add("OtherBankSeqId:" + this.Message);
                                    }
                                }

                                dt.Dispose();
                            }

                            dt = null; continue;
                        }
                    }

                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                    {
                        error_list.Add(s[0] + ":" + " Cannot be null");
                    }
                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (error_list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    sb.Append((String)errors + "\n");
                }

                this.error_message = string.Empty;
                this.error_message = sb.ToString();

                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = this.error_message;

                return false;
            }

            return this.commit_record();
        }

        private void get_error_dbase()
        {
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = base.Message;
            }
        }
    }

    [Serializable()]
    public class rem_modal_open_accounts : sqlHelperWs
    {
        public rem_modal_open_accounts()
        {
            base.EnforceConstraint = false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String commit_transaction(String column_values)
        {
            rem_other_details rod = new rem_other_details();

            try
            {
                if (new irsInternetPage().getRecordIfDuplicate(rod.Table_Name, column_values)) //Edited by Troy 20110912. Multiple insert on table.
                {
                    return "1";
                }
                else
                {
                    if (rod.insert_record(column_values))
                    {
                        return "1"; // if successful
                    }
                    else
                    {
                        return rod.Error_Message;
                    }
                }
                //if (rod.insert_record(column_values))
                //{
                //    return "1"; // if successful
                //}
                //else
                //{
                //    return rod.Error_Message;
                //}
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDDLData()
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_get_rem_modal_open_accounts_ddl");
                ds = base.getDataSet(sqcommands, "tbl_branches");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_branch_desc_name(int SeqID)
        {
            String sql = "SELECT BranchCode, BranchName FROM TBL_BRANCHES WHERE SEQ_ID = " + SeqID;

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand(sql, false);
                ds = base.getDataSet(sqcommands, "TBL_BRANCHES");

                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch
            {
                throw new Exception("No dataset returned");
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_branch_info(String SeqID)
        {
            String sql = "select BranchCode, BranchName from tbl_branches where SEQ_ID = " + SeqID;

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand(sql, false);
                ds = base.getDataSet(sqcommands, "TBL_BRANCHES");

                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch
            {
                throw new Exception("No dataset returned");
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }
    }

    [Serializable()]
    public class rem_ach_payments : sqlHelperWs
    {
        private int remittance_seq_id = 0;
        private int remittance_other_seq_id = 0;

        private string error_message = null;

        public rem_ach_payments()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_REMITTANCE_OTHER_DETAILS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;

            //commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_OTHER_DETAILS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_REMITTANCE_OTHER_DETAILS WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_transaction(string column_values)
        {
            bool ibool = false;

            string[] s = column_values.Split('|');

            if (this.check_if_record_exists(s) == false)
            {
                if (base.newRow() == true)
                {
                    ibool = this.set_record_and_commit(s, false);
                }
            }
            else
            {
                base.selectedRow = 0;

                if (base.edit(this.remittance_other_seq_id) == true)
                {
                    ibool = this.set_record_and_commit(s, false);
                }
            }

            if (ibool == false)
            {
                this.rollback_record(); return false;
            }

            return true;
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty || s[1].ToString().Trim().ToUpper() == "UNDEFINED")
                    {
                        s[1] = null;
                    }

                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (base.save() == false)
            {
                error_list.Add(this.Message); return false;
            }

            return true;
        }

        private bool check_if_record_exists(string[] column_values)
        {
            DataTable dt = null;

            try
            {
                for (int i = 0; i < column_values.Length; i++)
                {
                    string[] s = column_values[i].Split('~');

                    if (s[0] != null && s[0].ToString().Trim().Length > 0 && s[0].Trim().ToUpper().Equals("REM_SEQ_ID") == true)
                    {
                        this.remittance_seq_id = Convert.ToInt32(s[1]); continue;
                    }
                }

                dt = getDataSet(getSqlCommand("SELECT MAX(SEQ_ID) AS SEQ_ID FROM TBL_REMITTANCE_OTHER_DETAILS WHERE REM_SEQ_ID = " + this.remittance_seq_id.ToString(), false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.remittance_other_seq_id = Convert.ToInt32(dt.Rows[0]["SEQ_ID"]); return true;
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

    [Serializable()]
    public class rem_modal_check_payments : sqlHelperWs
    {
        private int remittance_seq_id = 0;
        private int remittance_other_seq_id = 0;
        private csAuditLog logger = new csAuditLog();

        public rem_modal_check_payments()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_REMITTANCE_CHECK_DETAILS";
            this.IdentityColumn = "SEQ_ID";

            //commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_CHECK_DETAILS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_REMITTANCE_CHECK_DETAILS WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_transaction(string column_values)
        {
            bool ibool = false;

            try
            {
                this.logs("", "TranClass.rem_modal_check_payments.commit_transaction()", "Inserting to remittance check details table", column_values, "");
                string[] s = column_values.Split('|');

                if (this.check_if_record_exists(s) == false)
                {
                    if (base.newRow() == true)
                    {
                        ibool = this.set_record_and_commit(s, false);
                    }
                }
                else
                {
                    base.selectedRow = 0;

                    if (base.edit(this.remittance_other_seq_id) == true)
                    {
                        ibool = this.set_record_and_commit(s, false);
                    }
                }

                if (ibool == false)
                {
                    this.rollback_record();
                    this.logs("", "TranClass.rem_modal_check_payments.commit_transaction()", "ERROR: " + this.Message, column_values, "");
                    return false;
                }
                this.logs("", "TranClass.rem_modal_check_payments.commit_transaction()", "SUCCESSFUL: " + this.Message, column_values, "");
            }
            catch (Exception ex)
            {
                this.rollback_record();
                this.logs("", "TranClass.rem_modal_check_payments.commit_transaction()", "ERROR: " + this.Message + ex.Message, column_values, "");
                return false;
            }
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_branch_info(String SeqID)
        {
            String sql = "select BranchCode, BranchName from tbl_branches where SEQ_ID = " + SeqID;

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand(sql, false);
                ds = base.getDataSet(sqcommands, "TBL_BRANCHES");

                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch
            {
                throw new Exception("No dataset returned");
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_clearing_date(string chek_date)
        {
            if (DateTime.Parse(chek_date) >= DateTime.Parse(get_branch_localtime().ToString("MM/dd/yyyy")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool checkCheckingDate(String column_values)
        {
            String[] ss = column_values.Split('|');
            String clearDate = "";
            foreach (String s in ss)
            {
                String[] col = s.Split('~');

                if (col[0] != "clearing_date")
                {
                    continue;
                }
                else
                {
                    clearDate = col[1];
                }
            }

            if (DateTime.Parse(clearDate) >= DateTime.Parse(get_branch_localtime().ToString("MM/dd/yyyy")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private String get_irsclient_data(String param)
        {
            if (irsClientData == null)
            {
                return null;
            }

            string s = null;


            switch (param)
            {
                case "UID": s = irsClientData.UserId; break;
                case "CIF": s = irsClientData.CIFID; break;
                case "LOCATION": s = irsClientData.Location; break;
                case "IP": s = irsClientData.IPAddress; break;
            }

            return s;

        }

        private DateTime get_branch_localtime()
        {
            DateTime dt = new irsTimeZone().branchLocalTime(get_irsclient_data("LOCATION"));
            return dt;
        }

        private irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }

                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty || s[1].ToString().Trim().ToUpper() == "UNDEFINED")
                    {
                        s[1] = null;
                    }

                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (base.save() == false)
            {
                error_list.Add(this.Message); return false;
            }

            return true;
        }

        private bool check_if_record_exists(string[] column_values)
        {
            DataTable dt = null;

            try
            {
                for (int i = 0; i < column_values.Length; i++)
                {
                    string[] s = column_values[i].Split('~');

                    if (s[0] != null && s[0].ToString().Trim().Length > 0 && s[0].Trim().ToUpper().Equals("REM_SEQ_ID") == true)
                    {
                        this.remittance_seq_id = Convert.ToInt32(s[1]); continue;
                    }
                }

                dt = getDataSet(getSqlCommand("SELECT MAX(SEQ_ID) AS SEQ_ID FROM TBL_REMITTANCE_CHECK_DETAILS WHERE REM_SEQ_ID = " + this.remittance_seq_id.ToString(), false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.remittance_other_seq_id = Convert.ToInt32(dt.Rows[0]["SEQ_ID"]); return true;
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
        private void logs(string userid, string module, string description, string newvalue, string oldvalue)
        {
            this.logger.writeAudit(userid, module, description, newvalue, oldvalue, csAuditLog.Category.Insert);
        }
    }

    [Serializable()]
    public class rem_modal_popups : sqlHelperWs
    {
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_targetcurrency()
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_get_targetcurrency");
                ds = base.getDataSet(sqcommands, "CURRENCY");

                if (ds != null)
                {
                    return ds;
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
        public DataSet get_cif_zipcode(string cifid)
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_getzipcode_details");
                sqcommands.Parameters.Add("@cifid", SqlDbType.VarChar).Value = cifid;

                ds = base.getDataSet(sqcommands, "ZIPCODE");

                if (ds != null)
                {
                    return ds;
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

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }
    }

    public class rem_other_details : sqlHelperWs
    {
        private string seq_id = null;
        private string error_message = null;
        private string _table_name = "TBL_REMITTANCE_OTHER_DETAILS";

        public rem_other_details()
        {
            base.EnforceConstraint = false;
        }

        public string Table_Name
        {
            get { return _table_name; }
            set { _table_name = value; }
        }

        public String Error_Message
        {
            get { return error_message; }
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = _table_name;
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;

            //commented by boyet for faster constructor SqlCommand command = getSqlCommand("SELECT * FROM " + _table_name + " WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM " + _table_name + " WHERE SEQ_ID = " + id.ToString(), false);
        }

        public string get_server_error_messages()
        {
            this.error_message = (string)System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"];

            if (this.error_message != null)
            {
                if (this.error_message != string.Empty && this.error_message.Length > 0)
                {
                    return this.error_message;
                }
            }

            return string.Empty;
        }

        public bool insert_record(string column_values)
        {
            bool ibool = false;

            string[] s = column_values.Split('|');
            string[] s2 = s[0].Split('~');

            if (!getCheckIfExists(s2[1].ToString()))
            {
                if (base.newRow() == true)
                {
                    ibool = this.set_record_and_commit(s, false);
                }

                if (ibool == false)
                {
                    this.get_error_dbase();
                    this.rollback_record();
                }

            }
            else { ibool = true; }
            return ibool;
        }

        private bool commit_record()
        {
            System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = null;

            if (base.save() == true)
            {
                System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = Convert.ToInt32(base.IdentityValue);
                System.Web.HttpContext.Current.Session["TRAN_COLUMN_SESSION"] = null;

                this.seq_id = base.IdentityValue.ToString();
                return true;
            }

            return false;
        }
        private bool getCheckIfExists(String transaction_id)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = base.getDataSet(getSqlCommand("SELECT * FROM TBL_REMITTANCE_other_details WHERE rem_SEQ_ID = " + transaction_id.ToString(), false), "TBL_RECORD");
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
                ds.Dispose();
                ds = null;
            }
            return false;
        }
        public String get_seq_id()
        {
            return System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"].ToString();
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                    {
                        continue;
                    }

                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (error_list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    sb.Append((String)errors + "\n");
                }

                this.error_message = string.Empty;
                this.error_message = sb.ToString();

                return false;
            }

            return this.commit_record();
        }

        private void get_error_dbase()
        {
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = base.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool chkCIFEmail(string cifID)
        {
            DataTable dt = null;

            try
            {
                dt = new irsReceipts().getCIFContacts(cifID);

                if (dt.Rows.Count > 0)
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

    public class rem_denomination : sqlHelperWs
    {
        private string seq_id = null;
        private string error_message = null;
        private string _table_name = "TBL_DENOMINATION";

        public rem_denomination()
        {
            base.EnforceConstraint = false;
        }

        public string Table_Name
        {
            get { return _table_name; }
            set { _table_name = value; }
        }

        public String Error_Message
        {
            get { return error_message; }
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = _table_name;
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;

            //commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM " + _table_name + " WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM " + _table_name + " WHERE SEQ_ID = " + id.ToString(), false);
        }

        public string get_server_error_messages()
        {
            this.error_message = (string)System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"];

            if (this.error_message != null)
            {
                if (this.error_message != string.Empty && this.error_message.Length > 0)
                {
                    return this.error_message;
                }
            }

            return string.Empty;
        }

        public bool insert_record(string column_values)
        {
            bool ibool = false;

            string[] s = column_values.Split('|');

            if (base.newRow() == true)
            {
                ibool = this.set_record_and_commit(s, false);
            }

            if (ibool == false)
            {
                this.get_error_dbase();
                this.rollback_record();
            }

            return ibool;
        }

        private bool commit_record()
        {
            System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = null;

            if (base.save() == true)
            {
                System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = Convert.ToInt32(base.IdentityValue);
                System.Web.HttpContext.Current.Session["TRAN_COLUMN_SESSION"] = null;

                this.seq_id = base.IdentityValue.ToString();
                return true;
            }

            return false;
        }

        public String get_seq_id()
        {
            return System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"].ToString();
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                    {
                        continue;
                    }
                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (error_list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    sb.Append((String)errors + "\n");
                }

                this.error_message = string.Empty;
                this.error_message = sb.ToString();

                return false;
            }

            return this.commit_record();
        }

        private void get_error_dbase()
        {
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = base.Message;
            }
        }
    }

    public class remitters_proxy : sqlHelperWs
    {
        public remitters_proxy()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_REMITTANCE_REPRESENTATIVE";
            this.IdentityColumn = "SEQ_ID";

            //commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_REPRESENTATIVE WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_REMITTANCE_REPRESENTATIVE WHERE SEQ_ID = " + id.ToString(), false);
        }

        public string insert_record(string column_values)
        {
            bool ibool = false; System.Web.HttpContext.Current.Session["REM_PROXY_ERROR"] = null;

            string[] s = column_values.Split('|');

            if (base.newRow() == true)
            {
                ibool = this.set_record_and_commit(s, false);
            }
            else
            {
                System.Web.HttpContext.Current.Session["REM_PROXY_ERROR"] = base.Message;
            }

            if (ibool == false)
            {
                this.rollback_record(); return "0~" + (string)System.Web.HttpContext.Current.Session["REM_PROXY_ERROR"];
            }

            return "1~";
        }

        private bool commit_record()
        {
            if (base.save() == false)
            {
                System.Web.HttpContext.Current.Session["REM_PROXY_ERROR"] = base.Message; return false;
            }

            return true;
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();

            System.Web.HttpContext.Current.Session["REM_PROXY_ERROR"] = null;

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (error_list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    sb.Append((String)errors + "\n");
                }

                System.Web.HttpContext.Current.Session["REM_PROXY_ERROR"] = sb.ToString(); return false;
            }

            return this.commit_record();
        }
    }

    [Serializable()]
    public class ord_data_entry : sqlHelperWs
    {
        public ord_data_entry()
        {
            base.EnforceConstraint = false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDDLData(string source)
        {
            string ssql = "";
            try
            {
                if (source == "")
                {
                    ssql = "select distinct corr_bank_code Value, corr_bank_name [Text] from tbl_grs_corrbank order by corr_bank_name " +
                "select distinct corr_bank_code Value, corr_bank_name [Text] from tbl_grs_corrbank order by corr_bank_name";
                }
                else
                {
                    ssql = "select FLDCODE Value, FldDescription [Text] from tbl_lookup where fldgroup = 'IBatch_Record_ID'";
                }

                SqlCommand sqcommands = null;
                DataSet ds = null;

                try
                {
                    sqcommands = base.getSqlCommand(ssql, false);
                    ds = base.getDataSet(sqcommands, "tbl_ord_branches");

                    if (ds != null)
                    {
                        return ds;
                    }
                    else
                    {
                        throw new Exception("No dataset returned");
                    }
                }
                catch
                {
                    throw new Exception("No dataset returned");
                }
                finally
                {
                    if (ds != null)
                    {
                        ds.Dispose();
                        ds = null;
                    }
                    if (sqcommands != null)
                    {
                        sqcommands.Dispose();
                        sqcommands = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getCorrbank(string corrid)
        {
            string ssql = "";

            try
            {
                ssql = "select distinct corr_bank_name from tbl_grs_corrbank where corr_bank_code = '" + corrid + "'";

                SqlCommand sqcommands = null;
                DataSet ds = null;

                try
                {
                    sqcommands = base.getSqlCommand(ssql, false);
                    ds = base.getDataSet(sqcommands, "tbl_ord_branches");

                    if (ds != null || ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        throw new Exception("No dataset returned");
                    }
                }
                catch
                {
                    throw new Exception("No dataset returned");
                }
                finally
                {
                    if (ds != null)
                    {
                        ds.Dispose();
                        ds = null;
                    }

                    if (sqcommands != null)
                    {
                        sqcommands.Dispose();
                        sqcommands = null;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get Corrbank data, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getBatchHeader()
        {
            irsGRSBatchEntries batchentry = null;

            if (System.Web.HttpContext.Current.Session["IBATCH_HEADER"] != null)
            {
                batchentry = (irsGRSBatchEntries)System.Web.HttpContext.Current.Session["IBATCH_HEADER"];
            }

            return getLookupdata("FldDescription", "IBatch", batchentry.p_fileformat.ToString());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String save_ord_data(String values)
        {
            string header = "";
            try
            {
                System.Web.HttpContext.Current.Session["ORD_Data"] = values;
                header = System.Web.HttpContext.Current.Session["IBATCH_HEADER"].ToString();
                return "Data Entry OK.";
            }
            catch
            {
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string commit_transactions(string column_values, decimal remitting_amount)
        {
            rem_other_details rod = new rem_other_details();
            irsGRSBatchEntries batchentry = null;

            string data = string.Empty;

            try
            {
                rod.Table_Name = "TBL_REMITTANCE_OTHER_DETAILS";

                if (System.Web.HttpContext.Current.Session["ORD_Data"] != null)
                {
                    data = System.Web.HttpContext.Current.Session["ORD_Data"].ToString();
                }

                if (column_values != null)
                {
                    data = data.Replace("<SEQ_ID>", column_values); column_values = data;
                }

                if (System.Web.HttpContext.Current.Session["IBATCH_HEADER"] != null)
                {
                    batchentry = (irsGRSBatchEntries)System.Web.HttpContext.Current.Session["IBATCH_HEADER"];
                }

                column_values += "|Batch_SEQ_ID~" + batchentry.p_batch_historyid;

                if (rod.insert_record(column_values) == true)
                {
                    batchentry.updateBatchInfo(batchentry.p_batch_seq_id, batchentry.p_transaction_date, remitting_amount); return "1";
                }
                else
                {
                    return rod.Error_Message;
                }
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
        }

        private string getLookupdata(string column, string fldgroup, string value)
        {
            string strvalue = string.Empty;

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("select distinct " + column + " from tbl_lookup Where fldgroup = '" + fldgroup + "' and fldvalue = '" + value + "'", false);
                ds = base.getDataSet(sqcommands, "tbl_lookup_data");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    strvalue = ds.Tables[0].Rows[0][0].ToString();
                }

                return string.Empty;

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

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }
    }

    [Serializable()]
    public class rem_card_screen : sqlHelperWs
    {
        public rem_card_screen()
        {
            base.EnforceConstraint = false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool has_cards(string CIFID)
        {
            DataSet ds = null;

            try
            {
                ds = base.execStoredProcedure("sp_rem_grid_cards", new object[] { CIFID });

                if (ds.Tables[0].Rows.Count >= 1)
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
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemName(string CIFID)
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("select distinct CIFID, LastName,FirstName,MiddleName from tbl_cif_master Where CIFID = '" + CIFID + "'", false);
                ds = base.getDataSet(sqcommands, "RemitterName");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
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

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDDLData()
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("select distinct FldCode Value, FldDescription [Text],FldValue from TBL_LOOKUP WHERE FLDGROUP = 'CardPaymentGateway' order by FldValue " +
                                                "select distinct FldValue Value, FldDescription [Text] from TBL_LOOKUP WHERE Fldgroup = 'CardType' order by FldValue", false);

                ds = base.getDataSet(sqcommands, "tbl_cards_info");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getCardInfo(int client_seq_id)
        {
            irsClientCards car = new irsClientCards();
            try
            {
                if (car != null)
                {
                    if (car.get_record(client_seq_id.ToString()) == true)
                    {
                        return car.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submit_payment_gateway(string cifid, string card_no, string card_name, string expiry_date, string security_code, string card_type, string currency_code, string amount, string remarks)
        {
            /* EDITED BY TROY 20120628 */
            string sReturn = string.Empty;
            try
            {
                try
                {
                    new IRSWorld.TranClass.RemTranExtra().writeLog("SWIPE CARD: [BranchCode=" + new IRSWorld.Internet.InternetPage().getLocation() + "]" +
                    "[CIFID=" + cifid + "]" +
                    "[CardNo=" + card_no + "]" +
                    "[CardName=" + card_name + "]" +
                    "[ExpiryDate=" + expiry_date + "]" +
                    "[CardType=" + card_type + "]" +
                    "[Currency=" + currency_code + "]" +
                    "[Amount=" + amount + "]" +
                    "[Remarks=" + remarks + "]",
                    "OTC Card Request", "submit_payment_gateway", "", csAuditLog.Category.PaymentGateway);

                    sReturn = new IRSWorld.Internet.InternetPage().submitPaymentGateway(
                        new IRSWorld.Internet.InternetPage().getLocation(),
                        cifid, card_no, card_name, expiry_date, security_code,
                        card_type, currency_code, amount, remarks);

                    if (sReturn.IndexOf("OK") >= 0)
                    {
                        new csAuditLog().writeAudit("", "OTC Card Successful", sReturn, "submit_payment_gateway", "", csAuditLog.Category.PaymentGateway);
                    }
                    else
                    {
                        new csAuditLog().writeAudit("", "OTC Card Exception", sReturn, "submit_payment_gateway", "", csAuditLog.Category.PaymentGateway);
                    }
                }
                catch (Exception ex)
                {
                    new csAuditLog().writeAudit("", "OTC Card Exception", "Error: " + ex.Message, "submit_payment_gateway", "", csAuditLog.Category.PaymentGateway);
                }
                return sReturn;
            }
            catch (Exception ex)
            {
                sReturn = ex.Message;
                new csAuditLog().writeAudit("", "OTC Card Exception", "Error: " + ex.Message, "", "", csAuditLog.Category.PaymentGateway);
                return sReturn;
            }
            /*
            irsClientData client_data = null;
            irsPaymentGateway payment_gate_way = new irsPaymentGateway();
            
            string branch_code  = null;
            string charge_group = null;
                        
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]; branch_code = client_data.Location;
            }

            if (System.Web.HttpContext.Current.Session["ChargeGroup"] == null)
            {
                charge_group = getChargeGroup(branch_code); System.Web.HttpContext.Current.Session["ChargeGroup"] = charge_group;
            }

            return payment_gate_way.createRequest(charge_group, branch_code, cifid, card_no, card_name, expiry_date, security_code, card_type, currency_code, amount, remarks, this.getBaseSiteUrl());
            */
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string submitPaymentGateway(string BranchCode, string CIFID, string CardNo, string CardName, string ExpiryDate, string SecurityCode, string CardType, string Currency, string Amount, string Remarks)
        {
            irsClientData client_data = null;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
            if (System.Web.HttpContext.Current.Session["ChargeGroup"] == null)
            {
                System.Web.HttpContext.Current.Session["ChargeGroup"] = getChargeGroup(client_data.Location);
            }
            //writeAudit("SWIPE CARD: [Region=" + System.Web.HttpContext.Current.Session["ChargeGroup"].ToString() + "]" + "[BranchCode=" + BranchCode + "]" + "[CIFID=" + CIFID + "]" + "[CardNo=" + CardNo + "]" + "[CardName=" + CardName + "]" + "[ExpiryDate=" + ExpiryDate + "]" + "[CardType=" + CardType + "]" + "[Currency=" + Currency + "]" + "[Amount=" + Amount + "]" + "[Remarks=" + Remarks + "]", "", "", csAuditLog.Category.PaymentGateway);
            return new irsPaymentGateway().createRequest(System.Web.HttpContext.Current.Session["ChargeGroup"].ToString(), client_data.Location, CIFID, CardNo, CardName, ExpiryDate, SecurityCode, CardType, Currency, Amount, Remarks, getBaseSiteUrl());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String commit_transactions(string SEQID, string RemAcctSeqID, string ResponseNo, string Response, string AuthCode, bool IsCard)
        {
            bool bReturn = false;
            TranClass.RemTranCardCommons irsCards = new IRSWorld.TranClass.RemTranCardCommons();
            try
            {
                bReturn = irsCards.registerCardResponse(SEQID, RemAcctSeqID, ResponseNo, Response, AuthCode, IsCard);
                if (bReturn)
                {
                    return "1"; // if successful
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool TransactionError(string refno)
        {
            int transtatus = 0;
            int tranxseqid = 0;
            try
            {
                transtatus = 9;
                //DataSet ds = base.getDataSet("SELECT * FROM TBL_REMITTANCE WHERE REFERENCENO = '" + refno + "'", false, new object[] { });
                tranxseqid = Convert.ToInt32(refno);
                execStoredProcedureTable("sp_trans_update_status", new object[] { tranxseqid, transtatus });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string getBaseSiteUrl()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
            return baseUrl;
        }

        private string getChargeGroup(string branchcode)
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM TBL_BRANCHES WHERE BRANCHCODE = '" + branchcode + "'", false);
                ds = base.getDataSet(sqcommands, "tbl_branches_info");

                if (ds != null)
                {
                    if (ds.Tables["tbl_branches_info"].Rows.Count > 0)
                    {
                        return ds.Tables["tbl_branches_info"].Rows[0]["ChargeGroup"].ToString();
                    }
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return "";
        }
    }

    [Serializable()]
    public class rem_debit_account_screen : sqlHelperWs
    {
        /* E2EAPICHANGE 20220209 Removed OAAMTranxStatus */

        public rem_debit_account_screen()
        {
            base.EnforceConstraint = false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool has_debit_accounts(string CIFID)
        {
            DataSet ds = null;

            try
            {
                ds = base.execStoredProcedure("sp_cif_grid_accounts", new object[] { CIFID });

                if (ds.Tables[0].Rows.Count >= 1)
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
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemName(string CIFID)
        {
            DataSet ds = null;
            SqlCommand sqcommands = null;

            try
            {
                sqcommands = base.getSqlCommand("select distinct CIFID, LastName,FirstName,MiddleName from tbl_cif_master Where CIFID = '" + CIFID + "'", false);
                ds = base.getDataSet(sqcommands, "RemitterName");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
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

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDDLData()
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("select distinct FldValue Value, FldDescription [Text],FldCode from TBL_LOOKUP WHERE FLDGROUP = 'AccountType' order by FldValue " +
                                                "select distinct FldValue Value, FldCode[Text],FldDescription from TBL_LOOKUP WHERE FLDGROUP = 'Currency' order by FldValue ", false);

                ds = base.getDataSet(sqcommands, "tbl_dacct_info");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String commit_transactions(string Refno, string accntNumber, int ccy, double Amount)
        {
            /* FOR CBIP */
            return "1";
            /*
            bool isGFCSave = false;            

            try
            {
                isGFCSave = new irsLinkToGFC().insert_GFC_Aggregate(Refno, accntNumber, ccy, Amount);

                if (isGFCSave == true)
                {                    
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
            */
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String commit_transaction_details(string SEQID, string RemAcctSeqID)
        {
            bool bReturn = false;
            try
            {
                TranClass.RemTranCardCommons irsCards = new IRSWorld.TranClass.RemTranCardCommons();
                bReturn = irsCards.registerCardResponse(SEQID, RemAcctSeqID, "", "", "", false);

                if (bReturn == true)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                return "error : " + ex.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDAcctInfo(int client_seq_id)
        {
            irsClientAccounts caa = new irsClientAccounts();

            try
            {
                if (caa != null)
                {
                    if (caa.get_record(client_seq_id.ToString()) == true)
                    {
                        return caa.getdataRecord();
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
                caa = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Decimal computeAmount(string amount, string TargetCCy, string SourceCCy)
        {
            irsCurrencyRates crt = new irsCurrencyRates();
            irsClientData cld = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            DataTable dt = null;

            try
            {
                decimal fxrates = 0;
                if (cld.Role != 10 && cld.Role != 25)
                {
                    dt = new DataTable("FxRates");
                    dt = crt.getCurrencyRates(new irsTimeZone().branchLocalTime(cld.Location), cld.Location);

                    fxrates = Convert.ToDecimal(dt.Select("SourceCurrency='" + SourceCCy + "' and TargetCurrency = '" + TargetCCy + "'", "DateInserted DESC")[0][5].ToString());

                    return Convert.ToDecimal(amount) * 1 / fxrates;
                }
                else
                {
                    irsInternetPage ip = new irsInternetPage();
                    string rate = ip.getExchangeRate(cld.Location, SourceCCy, TargetCCy, true);
                    string[] splitedrate = rate.Split(' ');
                    fxrates = Convert.ToDecimal(splitedrate[4]);
                    return Convert.ToDecimal(amount) * 1 / fxrates;
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

        /* E2EAPICHANGE 20220209 Removed oaam_verify_transaction, oaam_update_transaction_status */
    }

    [Serializable()]
    public class rem_modal_denomination : sqlHelperWs
    {
        public rem_modal_denomination()
        {
            base.EnforceConstraint = false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String commit_transaction(String column_values)
        {
            writeAudit("COMMIT", "START : " + column_values, "", "", csAuditLog.Category.Update);

            try
            {
                rem_denomination rd = new rem_denomination();

                if (rd.insert_record(column_values))
                {
                    writeAudit("COMMIT", "SUCCESSFULL : " + column_values, "", "", csAuditLog.Category.Update);
                    return "1"; // if successful
                }
                else
                {
                    writeAudit("COMMIT", "ERROR : " + column_values + " : " + rd.Error_Message, "", "", csAuditLog.Category.Update);
                    return rd.Error_Message;
                }
            }
            catch (Exception ex)
            {
                writeAudit("COMMIT", "ERROR : " + column_values + " : " + ex.Message, "", "", csAuditLog.Category.Update);
                return "error : " + ex.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDDLData()
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_get_rem_modal_open_accounts_ddl");
                ds = base.getDataSet(sqcommands, "tbl_branches");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get dropdown list data, Reason : " + ex.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool reverse_denomination(int seq_id, out String errmsg)
        {
            errmsg = "";
            try
            {
                int res = base.execStoredProcedureUpdate("sp_reverse_denomination", new Object[] { seq_id });

                if (res > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("No record updated " + base.Message);
                }
            }
            catch (Exception ex)
            {
                writeAudit("ERROR", "reverse_denomination : " + seq_id.ToString() + " " + ex.Message, "", "", csAuditLog.Category.Update);
                errmsg = ex.Message;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_remittance_details(int SeqID)
        {
            writeAudit("OPEN", "Start Inputing Cash Denomination " + SeqID.ToString(), "", "", csAuditLog.Category.Select);
            String sql = "SELECT *, remcurr = (select fldcode from tbl_lookup where fldgroup = 'currency' and fldvalue = Rem.RemittingCurrency) FROM TBL_REMITTANCE Rem WHERE SEQ_ID = " + SeqID;

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand(sql, false);
                ds = base.getDataSet(sqcommands, "TBL_REMITTANCE");

                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                writeAudit("ERROR", "get_remittance_details : " + SeqID.ToString() + " " + ex.Message, "", "", csAuditLog.Category.Select);
                throw new Exception("No dataset returned");
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_branch_info(String SeqID)
        {
            String sql = "select BranchCode, BranchName from tbl_branches where SEQ_ID = " + SeqID;

            SqlCommand sqcommands = null;
            DataSet ds = null;

            try
            {
                sqcommands = base.getSqlCommand(sql, false);
                ds = base.getDataSet(sqcommands, "TBL_BRANCHES");

                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch
            {
                throw new Exception("No dataset returned");
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        public void writeAudit(String Mode, String Description, String NewValue, String OldValue, csAuditLog.Category Category)
        {
            try
            {
                String userId = get_irsclient_data("UID");
                String sDescription = "Location=" + get_irsclient_data("LOCATION") + " : " + Description;
                new csAuditLog().writeAudit(userId, "CASH MANAGEMENT " + Mode, sDescription, NewValue, OldValue, Category);
            }
            catch { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Cancel(int SeqID)
        {
            writeAudit("CANCEL", "Cancelling : " + SeqID.ToString(), "", "", csAuditLog.Category.Select);
            if (canCancel(SeqID))
            {
                writeAudit("CANCEL", "SUCCESS : " + SeqID.ToString(), "", "", csAuditLog.Category.Select);
                return true;
            }
            else
            {
                writeAudit("CANCEL", "FAILED : " + SeqID.ToString(), "", "", csAuditLog.Category.Select);
                return false;
            }
        }

        private bool canCancel(int SeqID)
        {
            try
            {
                String sql = "";
                sql += "select s.* ";
                sql += "from tbl_lookup l inner join tbl_sub_lookup s on l.FldGroup = s.FldGroup and l.fldvalue = s.fldvalue ";
                sql += "where l.fldgroup = 'Cash Denomination' ";
                sql += "and l.fldcode = 'Cancellable' ";
                sql += "and s.branchcode = '" + get_irsclient_data("LOCATION") + "'";

                if (base.getDataSet(base.getSqlCommand(sql, false), "TBL_LOOKUP").Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                writeAudit("ERROR", "canCancel : " + SeqID.ToString() + " " + ex.Message, "", "", csAuditLog.Category.Select);
            }
            return false;
        }

        public irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }

                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        public String get_irsclient_data(String param)
        {
            string s = null;

            if (irsClientData.UserId != null && irsClientData.UserId.ToString().Trim() != "")
            {
                switch (param)
                {
                    case "UID": s = irsClientData.UserId; break;
                    case "CIF": s = irsClientData.CIFID; break;
                    case "LOCATION": s = irsClientData.Location; break;
                    case "IP": s = irsClientData.IPAddress; break;
                }
                return s;
            }
            else
            {
                throw new Exception("SESSION HAS EXPIRED.\r\nPlease log off and try relogin again.");
            }
        }

        #region FOR REMOTE OVERRIDE
        #region AJAX METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string tag_override_stat(string trandesc, string _prodchargeid, string _splmsg, string _sdetails, string _overridetype, string _overridedetails)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            irsTimeZone it = new irsTimeZone();
            csAuditLog log = new csAuditLog();
            try
            {
                DateTime tranxdate = it.branchLocalTime(this.IRSClientData.Location);
                return iro.set_override_stat(IRSClientData.Location, trandesc, _prodchargeid, IRSClientData.UserId, _splmsg, _sdetails, _overridetype, _overridedetails, tranxdate);
            }
            catch (Exception e)
            {
                try
                {
                    log.writeAudit(this.IRSClientData.UserId, "REMOVEERR",
                                trandesc + ";" +
                                _prodchargeid + ";" +
                                 _splmsg + ";" +
                                _sdetails + ";" +
                                _overridetype,
                                e.Message + ":" + e.StackTrace,
                                "", csAuditLog.Category.Update);
                }
                catch { }
                return "-1;ERROR:" + e.StackTrace;
            }
            finally
            {
                iro = null;
                log = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_override_stat(string _overrideid)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            try
            {
                return iro.check_override_stat(_overrideid, IRSClientData.Location, IRSClientData.UserId);
            }
            catch (Exception e)
            {
                return "-1;" + e.Message + "; ERROR WHILE CHECKING";
            }
            finally
            {
                iro = null;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int cancel_override_stat(string _overrideid)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            try
            {
                return iro.cancel_override_stat(_overrideid, IRSClientData.Location, IRSClientData.UserId);
            }
            catch
            {
                return 4;
            }
            finally
            {
                iro = null;
            }
        }
        #endregion
        #region PRIVATE METHOD
        private irsClientData IRSClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }
        #endregion

        #region OBPS MERCHANT DETAILS FOR OVERRIDE
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getEnrollMerchantInfo(string RemCIFID, string merName, string prodDesc)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            return iro.getEnrollMerchantInfo(RemCIFID, merName, prodDesc);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getOBPData(String sp, string merchcode, int prodcode)
        {
            irsRemoteOverride iro = new irsRemoteOverride();
            return iro.getOptionalField(sp, merchcode, prodcode);
        }

        #endregion

        #endregion
    }

    [Serializable()]
    public class rem_new_branches : sqlHelperWs
    {
        private string msg = string.Empty;

        public rem_new_branches()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_BRANCHES";
            this.IdentityColumn = "SEQ_ID";

            //commented by boyet for faster constructor SqlCommand command = getSqlCommand("SELECT * FROM TBL_BRANCHES WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_BRANCHES WHERE SEQ_ID = " + id.ToString(), false);
        }

        public string insert_new_branch(string branch_code, string branch_name, int branch_type)
        {
            bool got_error = false;

            if (this.check_if_exists(branch_code, branch_type) == false)
            {
                if (base.newRow() == true)
                {
                    if (!base.setColumn("BranchCode", branch_code))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("BranchName", branch_name))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("BranchType", branch_type))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("TimeZone", @"Asia/Manila"))
                    {
                        got_error = true;
                    }

                    if (got_error == false)
                    {
                        if (base.save() == true)
                        {
                            return "0~OK";
                        }
                    }

                    return "1~" + base.Message;

                }
                else
                {
                    return "1~" + base.Message;
                }
            }

            return "1~" + this.msg;
        }

        private bool check_if_exists(string branch_code, int branch_type)
        {
            DataTable dt = null;

            try
            {
                dt = getDataSet(getSqlCommand("SELECT * FROM TBL_BRANCHES WHERE BranchType = " + branch_type.ToString() + " AND BranchCode = '" + branch_code + "'", false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.msg = "Branch already exists : " + Convert.ToString(dt.Rows[0]["BranchCode"]) + " - " + Convert.ToString(dt.Rows[0]["BranchName"]); return true;
                }
            }
            catch (Exception e)
            {
                this.msg = e.Message; return true;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            return false;
        }
    }

    [Serializable()]
    public class rem_bpo_create_accounts : sqlHelperWs
    {
        public rem_bpo_create_accounts()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";

            // commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int insert_record(string cifid, string account_no, int account_type, int currency, int branch_seq_id, string branch_code)
        {
            bool got_error = false;

            if (account_type < 1)
            {
                account_type = 1;
            }

            /* FOR CBIP */
            if (string.IsNullOrEmpty(account_no) == false)
            {
                return 0;
            }

            if (branch_seq_id > 0 && string.IsNullOrEmpty(branch_code) == true)
            {
                DataTable dt3 = getDataSet(getSqlCommand("SELECT BRANCHCODE FROM TBL_BRANCHES WHERE SEQ_ID = " + branch_seq_id.ToString(), false)).Tables[0];

                if (dt3 != null && dt3.Rows.Count > 0)
                {
                    if (dt3.Rows[0]["BRANCHCODE"] != null)
                    {
                        branch_code = dt3.Rows[0]["BRANCHCODE"].ToString();
                    }

                    dt3.Dispose();
                    dt3 = null;
                }
            }

            DataTable dt = getDataSet(getSqlCommand("SELECT * FROM TBL_CIF_ACCOUNTS WHERE CIFID = '" + cifid +
                                                        "' AND ACCOUNTNO   = '" + account_no +
                                                        "' AND ACCOUNTTYPE = " + account_type.ToString() +
                                                        "  AND CURRENCY    = " + currency.ToString() +
                                                        "  AND BRANCHCODE  = '" + branch_code + "'", false)).Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count == 0)
                {
                    if (base.newRow() == true)
                    {
                        if (!base.setColumn("CIFID", cifid))
                        {
                            got_error = true;
                        }
                        if (!base.setColumn("accountno", account_no))
                        {
                            got_error = true;
                        }
                        if (!base.setColumn("accounttype", account_type))
                        {
                            got_error = true;
                        }
                        if (!base.setColumn("currency", currency))
                        {
                            got_error = true;
                        }
                        if (!base.setColumn("branchcode", branch_code))
                        {
                            got_error = true;
                        }

                        if (got_error == false)
                        {
                            if (base.save() == true)
                            {
                                DataTable dt2 = getDataSet(getSqlCommand("SELECT * FROM TBL_CIF_ACCOUNTS WHERE CIFID = '" + cifid +
                                                                            "' AND ACCOUNTNO   = '" + account_no +
                                                                            "' AND ACCOUNTTYPE = " + account_type.ToString() +
                                                                            "  AND CURRENCY    = " + currency.ToString() +
                                                                            "  AND BRANCHCODE  = '" + branch_code + "'", false)).Tables[0];

                                if (dt2 != null && dt2.Rows.Count > 0 && string.IsNullOrEmpty(dt2.Rows[0]["SEQ_ID"].ToString()) == false)
                                {
                                    return Convert.ToInt32(dt2.Rows[0]["SEQ_ID"]);
                                }
                                else
                                {
                                    return Convert.ToInt32(base.IdentityValue);
                                }
                            }
                        }
                    }
                }
                else
                {
                    return Convert.ToInt32(dt.Rows[0]["SEQ_ID"]);
                }

                dt.Dispose();
                dt = null;
            }

            return 0;
        }
    }

    [Serializable()]
    public class rem_account_rural_bank : sqlHelperWs
    {
        private string msg = string.Empty;

        public rem_account_rural_bank()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";

            //commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id.ToString(), false);
        }

        public string insert_new_accounts_rural_bank(string beneficiary_cifid, string bank_branch_account, string account_number, int account_currency, int account_type)
        {
            bool got_error = false;

            if (this.check_if_exists(beneficiary_cifid, bank_branch_account, account_number, account_currency, account_type) == false)
            {
                if (base.newRow() == true)
                {
                    if (!base.setColumn("CIFID", beneficiary_cifid))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("AccountNo", account_number))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("AccountType", account_type))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("Currency", account_currency))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("DomBankCode", "000"))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("branchname", bank_branch_account))
                    {
                        got_error = true;
                    }

                    if (!base.setColumn("branchtype", 5))
                    {
                        got_error = true;
                    }

                    if (got_error == false)
                    {
                        if (base.save() == true)
                        {
                            return "0~" + base.IdentityValue;
                        }
                    }

                    return "1~" + base.Message;
                }
            }

            return "1~" + base.Message;
        }

        private bool check_if_exists(string beneficiary_cifid, string bank_branch_account, string account_number, int account_currency, int account_type)
        {
            DataTable dt = null;

            try
            {
                dt = getDataSet(getSqlCommand("SELECT * FROM TBL_CIF_ACCOUNTS WHERE CIFID = '" + beneficiary_cifid + "' AND AccountNo = '" + account_number + "' AND AccountType = " + account_type.ToString() + " AND Currency = " + account_currency.ToString() + " AND DomBankCode = '000'", false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.msg = "Branch already exists : " + Convert.ToString(dt.Rows[0]["BranchCode"]) + " - " + Convert.ToString(dt.Rows[0]["BranchName"]); return true;
                }
            }
            catch (Exception e)
            {
                this.msg = e.Message; return true;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            return false;
        }
    }

    [Serializable()]
    public class rem_exp_ids : sqlHelperWs
    {
        private string error_msg = string.Empty;

        public rem_exp_ids()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_IDS";
            this.IdentityColumn = "SEQ_ID";

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_IDS WHERE ISNULL(deleted, 0) = 0 AND SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_rem_id_req(String branch_code)
        {
            return getDataSet("otc_val_ids", true, new Object[] { branch_code });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_rec(string client_id, string id_no, string id_type, string exp_dt, string place_issued)
        {
            this.error_msg = string.Empty;

            string validation_str = this.validate_input(1, client_id, id_no, id_type, exp_dt);

            if (string.IsNullOrEmpty(validation_str) == false)
            {
                return validation_str;
            }

            if (this.id_already_exists(client_id, id_no, id_type, exp_dt) == true)
            {
                return "Record already exists.";
            }

            if (string.IsNullOrEmpty(this.error_msg) == false)
            {
                return this.error_msg;
            }

            if (base.newRow() == true)
            {
                if (!base.setColumn("cifid", client_id)) { this.error_msg = "INVALID CIFID: " + client_id; }
                if (!base.setColumn("idno", id_no)) { this.error_msg += "\nINVALID ID TYPE: " + id_no; }
                if (!base.setColumn("deleted", 0)) { this.error_msg += "\nERROR ON DELETED COLUMN: " + client_id; }
                if (!base.setColumn("idtype", Convert.ToInt32(id_type))) { this.error_msg += "\nINVALID ID NO: " + id_type; }
                if (!base.setColumn("expirydate", Convert.ToDateTime(exp_dt))) { this.error_msg += "\nINVALID EXPIRY DATE: " + exp_dt; }
                if (!base.setColumn("PlaceOfIssuance", place_issued)) { this.error_msg += "\nINVALID PLACE OF ISSUANCE: " + place_issued; }

            }

            if (string.IsNullOrEmpty(this.error_msg).Equals(false))
            {
                return this.error_msg;
            }

            if (base.save() == false)
            {
                base.cancel(); return base.Message;
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string edit_rec(int client_seq_id, string client_id, string id_no, string id_type, string exp_dt, string place_issued)
        {
            string validation_str = this.validate_input(client_seq_id, client_id, id_no, id_type, exp_dt);

            if (string.IsNullOrEmpty(validation_str) == false)
            {
                return validation_str;
            }

            if (this.id_already_exists(client_id, id_no, id_type, exp_dt) == true)
            {
                return "Record already exists.";
            }

            if (string.IsNullOrEmpty(this.error_msg) == false)
            {
                return this.error_msg;
            }

            base.selectedRow = 0;

            if (base.edit(client_seq_id) == true)
            {
                if (!base.setColumn("cifid", client_id)) { this.error_msg = "INVALID CIFID: " + client_id; }
                if (!base.setColumn("idtype", Convert.ToInt32(id_type))) { this.error_msg += "\nINVALID ID TYPE: " + id_no; }
                if (!base.setColumn("idno", id_no)) { this.error_msg += "\nINVALID ID NO: " + id_type; }
                if (!base.setColumn("expirydate", Convert.ToDateTime(exp_dt))) { this.error_msg += "\nINVALID EXPIRY DATE: " + exp_dt; }
                if (!base.setColumn("PlaceOfIssuance", place_issued)) { this.error_msg += "\nINVALID PLACE OF ISSUANCE: " + place_issued; }
            }

            if (string.IsNullOrEmpty(this.error_msg).Equals(false))
            {
                return this.error_msg;
            }

            if (base.save() == false)
            {
                base.cancel(); return base.Message;
            }

            return string.Empty;
        }

        private bool id_already_exists(string client_id, string id_no, string id_type, string exp_dt)
        {
            string s = "SELECT COUNT(1) id_counter FROM TBL_CIF_IDS WHERE cifid ='" + client_id + "' AND idtype = '" + id_type + "' AND idno = '" + id_no + "' AND CONVERT(VARCHAR, expirydate, 101) = '" + exp_dt + "'";

            DataSet ds = null;

            try
            {
                ds = getDataSet(s, false, new Object[] { });

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0]["id_counter"]) > 0)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                this.error_msg = e.Message; return false;
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

        private string validate_input(int valid_seq_id, string client_id, string id_no, string id_type, string exp_dt)
        {
            if (valid_seq_id == 0)
            {
                return "Please select client ID to update";
            }

            if (string.IsNullOrEmpty(client_id) == true)
            {
                return "Client ID is required.";
            }

            if (string.IsNullOrEmpty(id_no) == true)
            {
                return "ID number is required.";
            }

            if (string.IsNullOrEmpty(id_type) == true)
            {
                return "ID type is required.";
            }

            if (string.IsNullOrEmpty(exp_dt) == true)
            {
                return "Expiration Date is required.";
            }

            DateTime dt;

            if (DateTime.TryParse(exp_dt, out dt) == false)
            {
                return "Expiration Date is invalid.";
            }

            irsClientData loggin_data = null;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }

            if (loggin_data != null)
            {
                if (Convert.ToDateTime(exp_dt) <= new irsTimeZone().branchLocalTime(loggin_data.Location))
                {
                    return "Expiration Date is invalid.";
                }
            }


            return string.Empty;
        }
    }

    [Serializable()]
    public class rem_states_chk : sqlHelperWs
    {
        public rem_states_chk()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ADDRESS";
            this.IdentityColumn = "SEQ_ID";

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ADDRESS WHERE SEQ_ID = " + id.ToString(), false);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_states()
        {
            return getDataSet("select fldvalue, fldcode, flddescription From tbl_lookup where fldgroup = 'states'", false, new object[] { });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool chk_state_valid(int rem_address_seq_id)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                if (loggin_data.Location.ToUpper() != "TEL")
                {
                    return true;
                }

                int i = Convert.ToInt32(getDataSet("select count(1) From tbl_lookup where fldgroup = 'states' and exists (select 1 from tbl_cif_address where seq_id = " + rem_address_seq_id + " and  ltrim(rtrim(Address5)) = ltrim(rtrim(tbl_lookup.fldcode)))", false, new object[] { }).Tables[0].Rows[0][0]);

                if (i > 0)
                {
                    return true;
                }
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_remitter_state(int remitter_addr_seq, string remitter_state)
        {
            bool ok = false;

            base.selectedRow = 0;

            ok = base.edit(remitter_addr_seq);

            if (ok == true)
            {
                ok = base.setColumn("Address5", remitter_state.Trim());
            }

            if (ok == true)
            {
                ok = base.save();
            }
            else
            {
                base.cancel();
            }

            return ok;
        }
    }

    [Serializable()]
    public class ibft_scr : sqlHelperWs
    {
        public ibft_scr()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_ibft_branch(string beneficiary_id, string account_no, string bank_code, int account_type, int account_currency)
        {
            string error_message = string.Empty;

            if (this.ibft_exists(beneficiary_id, account_no, bank_code, account_type, account_currency) == true)
            {
                return "Duplicate beneficiary account";
            }

            if (base.newRow() == true)
            {
                if (!base.setColumn("cifid", beneficiary_id)) { error_message = "INVALID BENEFICIARY ID: " + beneficiary_id; }
                if (!base.setColumn("AccountNo", account_no)) { error_message = "INVALID ACCOUNT NO: " + account_no; }
                if (!base.setColumn("AccountType", account_type)) { error_message = "INVALID ACCOUNT TYPE: " + account_type; }
                if (!base.setColumn("Currency", account_currency)) { error_message = "INVALID ACCOUNT CURRENCY: " + account_currency; }
                if (!base.setColumn("DomBankCode", bank_code)) { error_message = "INVALID BANK CODE: " + bank_code; }
                if (!base.setColumn("BranchType", 11)) { error_message = "INVALID BRANCH TYPE: 11"; }
            }

            if (error_message != null && error_message != "" && error_message != string.Empty && error_message.Length > 0)
            {
                return error_message;
            }

            if (base.save() == false)
            {
                base.cancel(); return base.Message;
            }

            return string.Empty;
        }

        public bool ibft_exists(string beneficiary_id, string account_no, string bank_code, int account_type, int account_currency)
        {
            DataTable dt = base.getDataSet("SELECT COUNT(1) FROM TBL_CIF_ACCOUNTS WHERE CIFID = '" + beneficiary_id +
                            "' AND AccountNo = '" + account_no +
                            "' AND AccountType = " + account_type.ToString() +
                            " AND Currency = " + account_currency.ToString() +
                            " AND DomBankCode = '" + bank_code + "'" +
                            " AND BranchType = 11", false, new object[] { }).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ibft_acct(string beneficiary_id)
        {
            return base.getDataSet("otc_ibft_acct_lst", true, new object[] { beneficiary_id });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_droplist()
        {
            DataSet ds = new DataSet();
            /* FOR CBIP */
            DataTable d1 = base.getDataSet("select FldValue, FldDescription From tbl_lookup where fldgroup = 'accounttype' and fldvalue in (1,2)", false, new object[] { }).Tables[0];
            DataTable d2 = base.getDataSet("select FldValue, FldDescription From tbl_lookup where fldgroup = 'currency' and fldvalue in (1,2,3)", false, new object[] { }).Tables[0];
            DataTable d3 = base.getDataSet("select branchcode as FldValue, branchname as FldDescription From tbl_branches where branchtype = 11", false, new object[] { }).Tables[0];

            d1.TableName = "TBL_1";
            d2.TableName = "TBL_2";
            d3.TableName = "TBL_3";

            ds.Merge(d1);
            ds.Merge(d2);
            ds.Merge(d3);

            ds.AcceptChanges();


            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_beneficiary_name(string beneficiary_id)
        {
            string sql = "select CIFID, UPPER(ISNULL(FIRSTNAME, '')) + ' ' + UPPER(ISNULL(MIDDLENAME, '')) + ' ' + UPPER(ISNULL(LASTNAME, '')) as beneficiary_name from tbl_cif_master where cifid = '" + beneficiary_id + "'";
            return base.getDataSet(sql, false, new object[] { });
        }
    }


    [Serializable()]
    public class otc_limit_apr : sqlHelperWs
    {
        public otc_limit_apr()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_OTC_LIMIT_APR";
            this.IdentityColumn = "SEQ_ID";

            base.getUpdateDataSet("SELECT * FROM TBL_OTC_LIMIT_APR WHERE SEQ_ID = " + id.ToString(), false);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string approve_limit(Int32 approval_seq_id, string user_id, string user_pass)
        {
            base.Message = string.Empty;

            if (approval_seq_id < 1)
            {
                return "Please select transaction to approve";
            }

            if (user_id == null || user_id == string.Empty)
            {
                return "Invalid user id.";
            }

            if (user_pass == null || user_pass == string.Empty)
            {
                return "Invalid password.";
            }

            if (base.edit(approval_seq_id) == true)
            {
                if (base.setColumn("approved_by", user_id) == false)
                {
                    return "Error encountered. Unable to save record.";
                }

                if (base.setColumn("dt_approved", DateTime.Now) == false)
                {
                    return "Error encountered. Unable to save record.";
                }

                if (base.save() == false)
                {
                    return base.Message;
                }
            }

            return string.Empty;
        }
    }

    [Serializable()]
    public class pnbmak_tag : sqlHelperWs
    {
        public pnbmak_tag()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id.ToString(), false);
        }
    }

    [Serializable()]
    public class instapay_scr : sqlHelperWs
    {
        public instapay_scr()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_instapay_branch(string beneficiary_id, string account_no, string bank_code, int account_type, int account_currency)
        {
            string error_message = string.Empty;

            if (this.instapay_exists(beneficiary_id, account_no, bank_code, account_type, account_currency) == true)
            {
                return "Duplicate beneficiary account";
            }

            if (base.newRow() == true)
            {
                if (!base.setColumn("cifid", beneficiary_id)) { error_message = "INVALID BENEFICIARY ID: " + beneficiary_id; }
                if (!base.setColumn("AccountNo", account_no)) { error_message = "INVALID ACCOUNT NO: " + account_no; }
                if (!base.setColumn("AccountType", account_type)) { error_message = "INVALID ACCOUNT TYPE: " + account_type; }
                if (!base.setColumn("Currency", account_currency)) { error_message = "INVALID ACCOUNT CURRENCY: " + account_currency; }
                if (!base.setColumn("DomBankCode", bank_code)) { error_message = "INVALID BANK CODE: " + bank_code; }
                if (!base.setColumn("BranchType", 12)) { error_message = "INVALID BRANCH TYPE: 12"; }
            }

            if (error_message != null && error_message != "" && error_message != string.Empty && error_message.Length > 0)
            {
                return error_message;
            }

            if (base.save() == false)
            {
                base.cancel(); return base.Message;
            }

            return string.Empty;
        }

        public bool instapay_exists(string beneficiary_id, string account_no, string bank_code, int account_type, int account_currency)
        {
            DataTable dt = base.getDataSet("SELECT COUNT(1) FROM TBL_CIF_ACCOUNTS WHERE CIFID = '" + beneficiary_id +
                            "' AND AccountNo = '" + account_no +
                            "' AND AccountType = " + account_type.ToString() +
                            " AND Currency = " + account_currency.ToString() +
                            " AND DomBankCode = '" + bank_code + "'" +
                            " AND BranchType in (11,12)", false, new object[] { }).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    return true;
                }
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_instapay_acct(string beneficiary_id)
        {
            return base.getDataSet("otc_instapay_acct_lst", true, new object[] { beneficiary_id });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_droplist()
        {
            DataSet ds = new DataSet();
            /* FOR CBIP */
            DataTable d1 = base.getDataSet("select FldValue, FldDescription From tbl_lookup where fldgroup = 'accounttype' and fldvalue in (1,2)", false, new object[] { }).Tables[0];
            DataTable d2 = base.getDataSet("select FldValue, FldDescription From tbl_lookup where fldgroup = 'currency' and fldvalue in (1)", false, new object[] { }).Tables[0];
            DataTable d3 = base.getDataSet("select branchcode as FldValue, branchname as FldDescription From tbl_branches where branchtype in (12,11) and ISNULL(COURIERCODE,'') != '' ", false, new object[] { }).Tables[0];

            d1.TableName = "TBL_1";
            d2.TableName = "TBL_2";
            d3.TableName = "TBL_3";

            ds.Merge(d1);
            ds.Merge(d2);
            ds.Merge(d3);

            ds.AcceptChanges();


            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_beneficiary_name(string beneficiary_id)
        {
            string sql = "select CIFID, UPPER(ISNULL(FIRSTNAME, '')) + ' ' + UPPER(ISNULL(MIDDLENAME, '')) + ' ' + UPPER(ISNULL(LASTNAME, '')) as beneficiary_name from tbl_cif_master where cifid = '" + beneficiary_id + "'";
            return base.getDataSet(sql, false, new object[] { });
        }

        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Instapay_validation(string remcifid, string benecifid)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            string msg_return = null;

            sqlkonek.EnforceConstraint = false;

            //string isInstapayPortal = ConfigurationManager.AppSettings["RAPI_IP_Flag"];
            string isInstapayPortal = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "FLAG", "FldDescription");

            if (isInstapayPortal == "1")
            {

                ds_return = sqlkonek.execStoredProcedure("sp_remben_instapay_validation", new object[] { remcifid, benecifid});


                if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
                {
                    msg_return = ds_return.Tables[0].Rows[0]["msg_return"].ToString();

                    if (msg_return != null || msg_return != "")
                    {
                        return msg_return;
                    }
                    else { return "OK"; }
                }
                return "OK";
            }
            return "OK";
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int Instapay_RAPI_IP_Flag(string cifid)
        {
           
            //string isInstapayPortal = ConfigurationManager.AppSettings["RAPI_IP_Flag"];
            string isInstapayPortal = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "FLAG", "FldDescription");
            if (isInstapayPortal == "1")
            {
                return 1;
            }
            else { return 0; }
        }
        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Instapay_BankValidation(string branchcode)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            string msg_return = null;

            sqlkonek.EnforceConstraint = false;

            //string isInstapayPortal = ConfigurationManager.AppSettings["RAPI_IP_Flag"];
            string isInstapayPortal = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "FLAG", "FldDescription");

            if (isInstapayPortal == "1")
            {

                ds_return = sqlkonek.execStoredProcedure("Instapay_BankValidation", new object[] { branchcode });


                if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
                {
                    msg_return = ds_return.Tables[0].Rows[0]["msg_return"].ToString();

                    if (msg_return != null || msg_return != "")
                    {
                        return msg_return;
                    }
                    else { return "OK"; }
                }
                return "OK";
            }
            return "OK";
        }
        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
    }
}