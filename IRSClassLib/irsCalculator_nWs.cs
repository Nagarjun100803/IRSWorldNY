/*                    
AUTHOR: DANIEL D. COLCOL III
DATE CODED: FEB. 2010
USAGE: FOR OVER THE COUNTER REMITTANCE, AUTOMATED REMITTANCE MACHINE, BILLS PAYMENT CHARGE COMPUTATION            
source_currency = BRANCH / AGENT LOCAL CURRENCY
target_currency = BRANCH / AGENT REMITTING CURRENCY
*/

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsCalculator_nWs : sqlHelper
    {
        #region "PRIVATE VARIABLES"

        private bool commit_charges_success = false;

        private DateTime transaction_date;

        private DataTable dt_charge_data = null;
        private DataTable dt_computed_charges = null;
        private DataTable dt_rates = null;
        private DataTable dt_charges = null;
        private DataTable dt_computed_charges_over = null;

        private int pinoy_currency = 1;
        private int kano_currency = 2;
        private int euro_currency = 5;

        private decimal remittance_reload_amt = 0;
        private decimal computed_amt = 0;

        private bool compute_peso_charges = false;
        private bool compute_charges_locally = false;
        private bool obps_rounded = false;
        private bool compute_obps = false;
        private bool waived_charges = false;
        private bool compute_override = false;
        private bool compute_override_reloaded = false;
        private bool charges_based_on_account_currency = false;

        private string transacting_branch = null;

        private int product_charges_id = 0;
        private int rem_txn_id = 0;
        private int source_currency = 0;
        private int target_currency = 0;

        /* CURRENCY RATE VALUE */
        private decimal currency_rate_target_to_source = 0;
        private decimal currency_rate_target_to_usd = 0;
        private decimal currency_rate_target_to_php = 0;
        private decimal currency_rate_target_to_eur = 0;
        private decimal currency_rate_source_to_target = 0;
        private decimal currency_rate_source_to_usd = 0;
        private decimal currency_rate_source_to_php = 0;
        private decimal currency_rate_target_to_account = 0;
        private decimal currency_rate_account_to_target = 0;
        private decimal currency_rate_usd_to_php = 0;
        /* END CURRENCY RATE VALUE */

        /* CURRENCY CODE & DESCRIPTION - TARGET */
        private int currency_rate_target_to_account_curr_code = 0;
        private int currency_rate_target_to_source_curr_code = 0;
        private int currency_rate_target_to_usd_curr_code = 0;
        private int currency_rate_target_to_php_curr_code = 0;
        private int currency_rate_target_to_eur_curr_code = 0;

        private string currency_rate_target_to_account_curr_code_desc = string.Empty;
        private string currency_rate_target_to_source_curr_code_desc = string.Empty;
        private string currency_rate_target_to_usd_curr_code_desc = string.Empty;
        private string currency_rate_target_to_php_curr_code_desc = string.Empty;
        private string currency_rate_target_to_eur_curr_code_desc = string.Empty;
        /* END CURRENCY CODE - TARGET */

        /* CURRENCY CODE & DESCRIPTION - SOURCE  */
        private int currency_rate_source_to_target_curr_code = 0;
        private int currency_rate_source_to_usd_code = 0;
        private int currency_rate_source_to_php_code = 0;

        private string currency_rate_source_to_target_curr_code_desc = string.Empty;
        private string currency_rate_source_to_usd_code_desc = string.Empty;
        private string currency_rate_source_to_php_code_desc = string.Empty;
        /* END CURRENCY CODE & DESCRIPTION - SOURCE  */

        /* REMITTANCE CONVERTED AMONT */
        private decimal converted_remitted_amount = 0;
        private decimal com_target_to_peso_factor = 0;
        private decimal com_peso_to_target_factor = 0;
        private decimal com_local_to_usd_factor = 0;
        private decimal com_target_to_usd_factor = 0;
        private decimal com_usd_to_php_factor = 0;
        /* END REMITTANCE CONVERTED AMONT */

        /* REMITTANCE NET AND GROSS COMPUTED AMOUNT */
        private decimal remittance_base_amount = 0;
        private decimal remittance_gross_branch = 0;
        private decimal remittance_net_branch = 0;
        private decimal remittance_net_local = 0;
        private decimal remittance_net_remit_amt = 0;
        private decimal remittance_total_charges = 0;
        private decimal remittance_net_to_bene_total_per_acct_ccy = 0;
        private decimal remittance_net_remit_amt_php_equivalent = 0;
        private decimal remittance_net_to_bene_amt_php_equivalent = 0;
        private decimal remittance_total_backend_charge = 0;
        private decimal remittance_total_benef_charge = 0;
        /* END REMITTANCE NET AND GROSS COMPUTED AMOUNT */

        private bool record_is_reloaded = false;

        private decimal remittance_account_currency_rate = 0; /* REMITTANCE DESTINATION ACCOUNT CURRENCY RATE (BENEFICIARY ACCOUNT, MERCHANT, BANK CK, OTHER BANK ACCOUNT)*/
        private int remittance_account_currency = 0; /* REMITTANCE DESTINATION ACCOUNT CURRENCY (BENEFICIARY ACCOUNT, MERCHANT, BANK CK, OTHER BANK ACCOUNT)*/
        private int remittance_computing_method = 0;
        private decimal obps_actual_payment = 0;

        private string txn_db_table_name = string.Empty;

        #endregion


        #region "PUBLIC METHOD"

        public irsCalculator_nWs(int transaction_sequence_id)
        {
            this.clean_all_vars();

            base.EnforceConstraint = false;

            this.record_is_reloaded = true;
            this.construct_working_table();
            this.rem_txn_id = transaction_sequence_id;

            string s = "SELECT TOP 1 * FROM V_REMITTANCE_TRANSACTION WHERE SEQ_REMITTANCE =" + transaction_sequence_id;

            DataTable dt = getDataSet(getSqlCommand(s, false), "TBL_RELOAD_TRANSACTIONS").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = Convert.ToInt32(dt.Rows[0]["ProductChargesId"]);

                    this.compute_charges_locally = Convert.ToBoolean(dt.Rows[0]["RemittanceChargeLocally"]);

                    if (Convert.ToInt32(dt.Rows[0]["ComputationLevel"]) == 1)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["GrossRemAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = this.remittance_reload_amt * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["ComputationLevel"]) == 2)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["NetRemAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = this.remittance_reload_amt * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["ComputationLevel"]) == 3)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["NettoBeneAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = Convert.ToDecimal(dt.Rows[0]["NetRemAmount"]) * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }

                    this.charges_based_on_account_currency = Convert.ToBoolean(dt.Rows[0]["BeneCurrFlag"]);
                    this.product_charges_id = Convert.ToInt32(dt.Rows[0]["ProductChargesId"]);
                    this.source_currency = Convert.ToInt32(dt.Rows[0]["LocalCurrency"]);
                    this.target_currency = Convert.ToInt32(dt.Rows[0]["RemittingCurrency"]);
                    this.remittance_computing_method = Convert.ToInt32(dt.Rows[0]["ComputationLevel"]);
                    this.remittance_account_currency = Convert.ToInt32(dt.Rows[0]["NettoBeneCurrency"]);
                    this.remittance_account_currency_rate = Convert.ToDecimal(dt.Rows[0]["ForexRemittingToAccount"]);
                    this.transacting_branch = Convert.ToString(dt.Rows[0]["RemittingBranch"]);
                    this.transaction_date = Convert.ToDateTime(dt.Rows[0]["TranxDate"]);
                    this.remittance_net_to_bene_total_per_acct_ccy = Convert.ToDecimal(dt.Rows[0]["NettoBeneAmount"]);

                    this.currency_rate_target_to_account = Convert.ToDecimal(dt.Rows[0]["ForexRemittingToAccount"]);
                    this.currency_rate_target_to_account_curr_code = Convert.ToInt32(dt.Rows[0]["NettoBeneCurrency"]);
                    this.currency_rate_target_to_account_curr_code_desc = this.get_description("Currency", 0, this.currency_rate_target_to_account_curr_code);

                    this.load_product_charges_data(transaction_sequence_id);
                    this.load_product_computed_charges(transaction_sequence_id);
                    this.load_currency_factor(transaction_sequence_id);

                    this.com_target_to_peso_factor = this.get_account_to_peso_factor();
                    this.com_peso_to_target_factor = this.get_peso_to_target_factor();
                    this.com_local_to_usd_factor = this.get_local_to_usd_factor();
                    this.com_target_to_usd_factor = this.get_target_to_usd_factor();
                    this.com_usd_to_php_factor = this.get_usd_to_peso_factor();

                    if (dt.Rows[0]["total_backend_charges"] != DBNull.Value)
                    {
                        this.remittance_total_backend_charge = Convert.ToDecimal(dt.Rows[0]["total_backend_charges"]);
                    }
                }
            }
        }

        public irsCalculator_nWs(int transaction_sequence_id, int compute_method)
        {
            this.clean_all_vars();

            base.EnforceConstraint = false;

            this.record_is_reloaded = true;
            this.construct_working_table();
            this.rem_txn_id = transaction_sequence_id;
            this.remittance_computing_method = compute_method;

            string s = "SELECT TOP 1 * FROM V_REMITTANCE_TRANSACTION WHERE SEQ_REMITTANCE =" + transaction_sequence_id;

            DataTable dt = getDataSet(getSqlCommand(s, false), "TBL_RELOAD_TRANSACTIONS").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = Convert.ToInt32(dt.Rows[0]["ProductChargesId"]);

                    this.compute_charges_locally = Convert.ToBoolean(dt.Rows[0]["RemittanceChargeLocally"]);

                    if (compute_method == 1)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["GrossRemAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = this.remittance_reload_amt * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }
                    else if (compute_method == 2)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["NetRemAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = this.remittance_reload_amt * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }
                    else if (compute_method == 3)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["NettoBeneAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = Convert.ToDecimal(dt.Rows[0]["NetRemAmount"]) * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }

                    this.charges_based_on_account_currency = Convert.ToBoolean(dt.Rows[0]["BeneCurrFlag"]);
                    this.product_charges_id = Convert.ToInt32(dt.Rows[0]["ProductChargesId"]);
                    this.source_currency = Convert.ToInt32(dt.Rows[0]["LocalCurrency"]);
                    this.target_currency = Convert.ToInt32(dt.Rows[0]["RemittingCurrency"]);
                    this.remittance_account_currency = Convert.ToInt32(dt.Rows[0]["NettoBeneCurrency"]);
                    this.remittance_account_currency_rate = Convert.ToDecimal(dt.Rows[0]["ForexRemittingToAccount"]);
                    this.transacting_branch = Convert.ToString(dt.Rows[0]["RemittingBranch"]);
                    this.transaction_date = Convert.ToDateTime(dt.Rows[0]["TranxDate"]);
                    this.remittance_net_to_bene_total_per_acct_ccy = Convert.ToDecimal(dt.Rows[0]["NettoBeneAmount"]);
                    this.currency_rate_target_to_account = Convert.ToDecimal(dt.Rows[0]["ForexRemittingToAccount"]);
                    this.currency_rate_target_to_account_curr_code = Convert.ToInt32(dt.Rows[0]["NettoBeneCurrency"]);
                    this.currency_rate_target_to_account_curr_code_desc = this.get_description("Currency", 0, this.currency_rate_target_to_account_curr_code);

                    this.load_product_charges_data(transaction_sequence_id);
                    this.load_product_computed_charges(transaction_sequence_id);
                    this.load_currency_factor(transaction_sequence_id);

                    this.com_target_to_peso_factor = this.get_account_to_peso_factor();
                    this.com_peso_to_target_factor = this.get_peso_to_target_factor();
                    this.com_local_to_usd_factor = this.get_local_to_usd_factor();
                    this.com_target_to_usd_factor = this.get_target_to_usd_factor();
                    this.com_usd_to_php_factor = this.get_usd_to_peso_factor();

                    if (dt.Rows[0]["total_backend_charges"] != DBNull.Value)
                    {
                        this.remittance_total_backend_charge = Convert.ToDecimal(dt.Rows[0]["total_backend_charges"]);
                    }
                }
            }
        }

        public irsCalculator_nWs(int product_charge_id, int account_currency, string branch_code, bool charge_locally, int rate_type)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;

            this.record_is_reloaded = false;
            this.compute_charges_locally = charge_locally;
            this.construct_working_table();

            this.remittance_account_currency = account_currency;
            this.product_charges_id = product_charge_id;
            this.transaction_date = new irsTimeZone().branchLocalTime(branch_code);
            this.transacting_branch = branch_code;

            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
            this.load_branch_local_currency(); /* FIRST  */
            this.load_product_charges_data();  /* SECOND */
            this.load_currency_special_rates(rate_type);       /* THIRD  */
            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

            this.com_target_to_peso_factor = this.get_account_to_peso_factor();
            this.com_peso_to_target_factor = this.get_peso_to_target_factor();
            this.com_local_to_usd_factor = this.get_local_to_usd_factor();
            this.com_target_to_usd_factor = this.get_target_to_usd_factor();
            this.com_usd_to_php_factor = this.get_usd_to_peso_factor();

        }

        public irsCalculator_nWs(int product_charge_id, int account_currency, string branch_code, bool charge_locally, int rate_type, int compute_method, int remitting_currency)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;

            this.record_is_reloaded = false;
            this.compute_charges_locally = charge_locally;
            this.construct_working_table();

            this.remittance_account_currency = account_currency;
            this.product_charges_id = product_charge_id;
            this.transaction_date = new irsTimeZone().branchLocalTime_nWs(branch_code);
            this.transacting_branch = branch_code;
            this.remittance_computing_method = compute_method;

            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
            this.load_branch_local_currency(); /* FIRST  */
            this.load_product_charges_data();  /* SECOND */
            this.load_currency_special_rates(rate_type);       /* THIRD  */
            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

            this.com_target_to_peso_factor = this.get_account_to_peso_factor();
            this.com_peso_to_target_factor = this.get_peso_to_target_factor();
            this.com_local_to_usd_factor = this.get_local_to_usd_factor();
            this.com_target_to_usd_factor = this.get_target_to_usd_factor();
            this.com_usd_to_php_factor = this.get_usd_to_peso_factor();
        }
        public irsCalculator_nWs(int product_charge_id, int account_currency, string branch_code, bool charge_locally)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;

            this.record_is_reloaded = false;
            this.compute_charges_locally = charge_locally;
            this.construct_working_table();

            this.remittance_account_currency = account_currency;
            this.product_charges_id = product_charge_id;
            this.transaction_date = new irsTimeZone().branchLocalTime(branch_code);
            this.transacting_branch = branch_code;

            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
            this.load_branch_local_currency(); /* FIRST  */
            this.load_product_charges_data();  /* SECOND */
            this.load_currency_factor();       /* THIRD  */
            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

            this.com_target_to_peso_factor = this.get_account_to_peso_factor();
            this.com_peso_to_target_factor = this.get_peso_to_target_factor();
            this.com_local_to_usd_factor = this.get_local_to_usd_factor();
            this.com_target_to_usd_factor = this.get_target_to_usd_factor();
        }
        public irsCalculator_nWs(int product_charge_id, int account_currency, string branch_code, int compute_method, bool charge_locally)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;


            this.record_is_reloaded = false;
            this.compute_charges_locally = charge_locally;
            this.construct_working_table();

            this.remittance_account_currency = account_currency;
            this.product_charges_id = product_charge_id;
            this.transaction_date = new irsTimeZone().branchLocalTime_nWs(branch_code);
            this.transacting_branch = branch_code;

            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
            this.load_branch_local_currency(); /* FIRST  */
            this.load_product_charges_data();  /* SECOND */
            this.load_currency_factor();       /* THIRD  */
            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

            this.remittance_computing_method = compute_method;
            this.com_target_to_peso_factor = this.get_account_to_peso_factor();
            this.com_peso_to_target_factor = this.get_peso_to_target_factor();
            this.com_local_to_usd_factor = this.get_local_to_usd_factor();
            this.com_target_to_usd_factor = this.get_target_to_usd_factor();
            this.com_usd_to_php_factor = this.get_usd_to_peso_factor();
        }

        public irsCalculator_nWs(string branch, int product_charge_id, int account_currency, bool charge_locally, int compute_method)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;

            this.record_is_reloaded = false;
            this.compute_charges_locally = charge_locally;
            this.construct_working_table();

            this.remittance_account_currency = account_currency;
            this.product_charges_id = product_charge_id;

            try
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                    this.transacting_branch = loggin_data.Location;
                    this.transaction_date = new irsTimeZone().branchLocalTime(loggin_data.Location);
                }
            }
            catch { }

            this.transacting_branch = branch;
            this.transaction_date = new irsTimeZone().branchLocalTime_nWs(branch);

            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
            this.load_branch_local_currency(); /* FIRST  */
            this.load_product_charges_data();  /* SECOND */
            this.load_currency_factor();       /* THIRD  */
            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

            this.remittance_computing_method = compute_method;
            this.com_target_to_peso_factor = this.get_account_to_peso_factor();
            this.com_peso_to_target_factor = this.get_peso_to_target_factor();
            this.com_local_to_usd_factor = this.get_local_to_usd_factor();
            this.com_target_to_usd_factor = this.get_target_to_usd_factor();
            this.com_usd_to_php_factor = this.get_usd_to_peso_factor();
        }

        public irsCalculator_nWs(int product_charge_id, int account_currency, bool charge_locally, int compute_method)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;

            this.record_is_reloaded = false;
            this.compute_charges_locally = charge_locally;
            this.construct_working_table();

            this.remittance_account_currency = account_currency;
            this.product_charges_id = product_charge_id;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                this.transacting_branch = loggin_data.Location;
                this.transaction_date = new irsTimeZone().branchLocalTime(loggin_data.Location);
            }

            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
            this.load_branch_local_currency(); /* FIRST  */
            this.load_product_charges_data();  /* SECOND */
            this.load_currency_factor();       /* THIRD  */
            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

            this.remittance_computing_method = compute_method;

            this.com_target_to_peso_factor = this.get_account_to_peso_factor();
            this.com_peso_to_target_factor = this.get_peso_to_target_factor();
            this.com_local_to_usd_factor = this.get_local_to_usd_factor();
            this.com_target_to_usd_factor = this.get_target_to_usd_factor();
            this.com_usd_to_php_factor = this.get_usd_to_peso_factor();
        }
        public irsCalculator_nWs(int product_charge_id, int account_currency, bool charge_locally)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;
            this.record_is_reloaded = false;
            this.compute_charges_locally = charge_locally;
            this.construct_working_table();

            this.remittance_account_currency = account_currency;
            this.product_charges_id = product_charge_id;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                this.transacting_branch = loggin_data.Location;
                this.transaction_date = new irsTimeZone().branchLocalTime(loggin_data.Location);
            }

            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
            this.load_branch_local_currency(); /* FIRST  */
            this.load_product_charges_data();  /* SECOND */
            this.load_currency_factor();       /* THIRD  */
            /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

            this.com_target_to_peso_factor = this.get_account_to_peso_factor();
            this.com_peso_to_target_factor = this.get_peso_to_target_factor();
            this.com_local_to_usd_factor = this.get_local_to_usd_factor();
            this.com_target_to_usd_factor = this.get_target_to_usd_factor();
            this.com_usd_to_php_factor = this.get_usd_to_peso_factor();
        }

        public irsCalculator_nWs(int product_charge_id, int tranx_id, int recompute)
        {
            this.clean_all_vars();

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = product_charge_id;
            }
            catch
            {
            }

            base.EnforceConstraint = false;

            this.record_is_reloaded = false;

            this.construct_working_table();
            this.rem_txn_id = tranx_id;



            this.product_charges_id = product_charge_id;

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                this.transacting_branch = loggin_data.Location;
                this.transaction_date = new irsTimeZone().branchLocalTime(loggin_data.Location);
            }

            string s = "SELECT TOP 1 * FROM V_REMITTANCE_TRANSACTION WHERE SEQ_REMITTANCE =" + tranx_id;

            DataTable dt = getDataSet(getSqlCommand(s, false), "TBL_RELOAD_TRANSACTIONS").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    this.compute_charges_locally = Convert.ToBoolean(dt.Rows[0]["RemittanceChargeLocally"]);

                    if (Convert.ToInt32(dt.Rows[0]["ComputationLevel"]) == 1)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["GrossRemAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = this.remittance_reload_amt * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["ComputationLevel"]) == 2)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["NetRemAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = this.remittance_reload_amt * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }
                    else if (Convert.ToInt32(dt.Rows[0]["ComputationLevel"]) == 3)
                    {
                        this.remittance_reload_amt = Convert.ToDecimal(dt.Rows[0]["NettoBeneAmount"]);
                        this.remittance_net_remit_amt_php_equivalent = Convert.ToDecimal(dt.Rows[0]["NetRemAmount"]) * Convert.ToDecimal(dt.Rows[0]["ForexRemittingToPHP"]);
                    }

                    this.charges_based_on_account_currency = Convert.ToBoolean(dt.Rows[0]["BeneCurrFlag"]);
                    this.source_currency = Convert.ToInt32(dt.Rows[0]["LocalCurrency"]);
                    this.target_currency = Convert.ToInt32(dt.Rows[0]["RemittingCurrency"]);
                    this.remittance_computing_method = Convert.ToInt32(dt.Rows[0]["ComputationLevel"]);
                    this.remittance_account_currency = Convert.ToInt32(dt.Rows[0]["NettoBeneCurrency"]);
                    this.remittance_account_currency_rate = Convert.ToDecimal(dt.Rows[0]["ForexRemittingToAccount"]);
                    this.transacting_branch = Convert.ToString(dt.Rows[0]["RemittingBranch"]);
                    this.transaction_date = Convert.ToDateTime(dt.Rows[0]["TranxDate"]);
                    this.remittance_net_to_bene_total_per_acct_ccy = Convert.ToDecimal(dt.Rows[0]["NettoBeneAmount"]);

                    this.currency_rate_target_to_account = Convert.ToDecimal(dt.Rows[0]["ForexRemittingToAccount"]);
                    this.currency_rate_target_to_account_curr_code = Convert.ToInt32(dt.Rows[0]["NettoBeneCurrency"]);
                    this.currency_rate_target_to_account_curr_code_desc = this.get_description("Currency", 0, this.currency_rate_target_to_account_curr_code);

                    /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */
                    this.load_branch_local_currency(); /* FIRST  */
                    this.load_product_charges_data();  /* SECOND */
                    this.load_currency_factor(tranx_id);
                    /* WARNING! DO NOT INTERCHANGE THE POSITION OF THE FOLLOWING METHOD. */

                    this.com_target_to_peso_factor = this.get_account_to_peso_factor();
                    this.com_peso_to_target_factor = this.get_peso_to_target_factor();
                    this.com_local_to_usd_factor = this.get_local_to_usd_factor();
                    this.com_target_to_usd_factor = this.get_target_to_usd_factor();
                    this.com_usd_to_php_factor = this.get_usd_to_peso_factor();

                    if (dt.Rows[0]["total_backend_charges"] != DBNull.Value)
                    {
                        this.remittance_total_backend_charge = Convert.ToDecimal(dt.Rows[0]["total_backend_charges"]);
                    }
                }
            }
        }

        public override void getUpdateDS(int id)
        {
            base.TableName = this.txn_db_table_name;
            base.IdentityColumn = "SEQ_ID";
            base.getUpdateDataSet(getSqlCommand("SELECT * FROM " + this.txn_db_table_name + " WHERE SEQ_ID = " + id, false));
        }

        /*
        public void commit_remittance_charge_details(int txn_sequence_id)
        {
            this.commit_transaction_charge_entries(txn_sequence_id, "TBL_REMITTANCE_CHARGES_DATA");
            this.commit_transaction_charge_entries(txn_sequence_id, "TBL_REMITTANCE_COMPUTED_CHARGES");
        }
         */

        public void commit_remittance_charge_details(int txn_sequence_id)
        {
            this.commit_charges_success = false;
            this.commit_remittance_charges_data(txn_sequence_id);
            this.commit_transaction_charge_entries(txn_sequence_id, "TBL_REMITTANCE_COMPUTED_CHARGES");

            /* RETRY COMMIT TBL_REMITTANCE_COMPUTED_CHARGES # 1 */
            if (this.commit_charges_success == false)
            {
                this.commit_transaction_charge_entries(txn_sequence_id, "TBL_REMITTANCE_COMPUTED_CHARGES");
            }
            /* END */

            /* RETRY COMMIT TBL_REMITTANCE_COMPUTED_CHARGES # 2 */
            if (this.commit_charges_success == false)
            {
                this.commit_transaction_charge_entries(txn_sequence_id, "TBL_REMITTANCE_COMPUTED_CHARGES");
            }
            /* END */

            if (this.commit_charges_success == true)
            {
                this.log_error("commit_remittance_charge_details: TBL_REMITTANCE_COMPUTED_CHARGES: STATUS: SAVED:" + txn_sequence_id.ToString());
            }
            
        }


        public bool obps_is_rounded()
        {
            return this.obps_rounded;
        }
        public decimal get_remittance_net_to_bene_amt_php_equivalent()
        {
            return this.remittance_net_to_bene_amt_php_equivalent;
        }
        public decimal get_remittance_net_remit_php_equivalent()
        {
            return this.remittance_net_remit_amt_php_equivalent;
        }

        public decimal get_remittance_base_amount()
        {
            return this.remittance_base_amount;
        }

        public decimal get_remittance_gross_branch()
        {
            return this.remittance_gross_branch;
        }

        public decimal get_remittance_net_branch()
        {
            return this.remittance_net_branch;
        }

        public decimal get_remittance_net_local()
        {
            return this.remittance_net_local;
        }

        public decimal get_remittance_net_remit_amt()
        {
            return this.remittance_net_remit_amt;
        }

        public decimal get_remittance_total_charges()
        {
            return this.remittance_total_charges;
        }

        public decimal get_remittance_net_to_bene_amt()
        {
            return this.remittance_net_to_bene_total_per_acct_ccy;
        }
        public decimal get_remittance_net_to_merchant_amt()
        {
            return this.remittance_net_to_bene_total_per_acct_ccy;
        }

        public decimal get_remittance_total_charges_amt()
        {
            return this.remittance_total_charges;
        }

        public int get_forex_remitting_currency_code()
        {
            return currency_rate_target_to_source_curr_code;
        }

        public string get_forex_remitting_currency_desc()
        {
            return this.currency_rate_target_to_source_curr_code_desc;
        }

        public decimal get_forex_remitting_to_account()
        {
            return this.get_forex_target_to_account();
        }

        public decimal get_forex_target_to_source()
        {
            return this.currency_rate_target_to_source;
        }

        public decimal get_forex_target_to_usd()
        {
            return this.currency_rate_target_to_usd;
        }

        public decimal get_forex_target_to_php()
        {
            return this.currency_rate_target_to_php;
        }

        public decimal get_forex_target_to_eur()
        {
            return currency_rate_target_to_eur;
        }



        public string get_forex_remitting_to_account_code_desc()
        {
            return this.get_forex_target_to_account_code_desc();
        }

        public string get_forex_target_to_usd_curr_code_desc()
        {
            return this.currency_rate_target_to_usd_curr_code_desc;
        }

        public string get_forex_target_to_php_code_desc()
        {
            return this.currency_rate_target_to_php_curr_code_desc; ;
        }

        public string get_forex_target_to_eur_code_desc()
        {
            return currency_rate_target_to_eur_curr_code_desc; ;
        }

        public int get_forex_remitting_to_account_code()
        {
            return this.get_forex_target_to_account_code();
        }

        public int get_target_to_usd_curr_code()
        {
            return this.currency_rate_target_to_usd_curr_code;
        }

        public int get_target_to_php_curr_code()
        {
            return this.currency_rate_target_to_php_curr_code;
        }

        public int get_target_to_eur_curr_code()
        {
            return this.currency_rate_target_to_eur_curr_code;
        }

        public int get_beneficiary_account_currency()
        {
            return this.remittance_account_currency;
        }

        public decimal get_total_backend_charges()
        {
            return this.remittance_total_backend_charge;
        }

        public decimal get_total_benef_charges()
        {
            return this.remittance_total_benef_charge;
        }

        /* END FOREX TARGET */


        /* FOREX SOURCE */
        public decimal get_forex_source_to_target()
        {
            return this.currency_rate_source_to_target;
        }

        public int get_forex_source_to_target_code()
        {
            return this.currency_rate_source_to_target_curr_code;
        }

        public string get_forex_source_to_target_code_desc()
        {
            return this.currency_rate_source_to_target_curr_code_desc;
        }

        public decimal get_forex_source_to_php()
        {
            return this.currency_rate_source_to_php;
        }
        public int get_forex_source_to_php_code()
        {
            return currency_rate_source_to_php_code;
        }

        public string get_forex_source_to_php_code_desc()
        {
            return currency_rate_source_to_php_code_desc;
        }

        public decimal get_forex_source_to_usd()
        {
            return this.currency_rate_source_to_usd;
        }
        public int get_forex_source_to_usd_code()
        {
            return currency_rate_source_to_usd_code;
        }
        public string get_forex_source_to_usd_code_desc()
        {
            return currency_rate_source_to_usd_code_desc;
        }

        public decimal get_forex_target_to_account()
        {
            return this.currency_rate_target_to_account;
        }

        public int get_forex_target_to_account_code()
        {
            return currency_rate_target_to_account_curr_code;
        }

        public string get_forex_target_to_account_code_desc()
        {
            return currency_rate_target_to_account_curr_code_desc;
        }


        /*END FOREX SOURCE */

        public decimal get_obps_actual_payment()
        {
            return this.obps_actual_payment;
        }

        public int get_branch_local_currency()
        {
            return this.source_currency;
        }

        public void reload_computation_entries(bool record_is_obps)
        {
            if (record_is_obps == false)
            {
                this.recompute_irsw_charges(this.remittance_reload_amt);
            }
            else if (record_is_obps == true)
            {
                this.recompute_obps_charges(this.remittance_reload_amt);
            }
        }

        public void reload_computation_entries(bool record_is_obps, bool ORD)
        {
            if (record_is_obps == false)
            {
                this.recompute_irsw_charges(this.remittance_reload_amt, ORD);
            }
        }

        public void reload_computation_and_waived_charges(bool record_is_obps)
        {
            this.waived_charges = true;
            this.record_is_reloaded = true;
            this.compute_override = false;
            this.reload_computation_entries(record_is_obps);
        }
        public void reload_computation_and_waived_charges(bool record_is_obps, bool ORD)
        {
            this.waived_charges = true;
            this.record_is_reloaded = true;
            this.compute_override = false;
            this.reload_computation_entries(record_is_obps, ORD);
        }

        public void reload_computation_and_override_rates_charges(bool record_is_obps)
        {
            this.waived_charges = false;
            this.compute_override = true;
            this.record_is_reloaded = true;

            if (record_is_obps == false)
            {
                this.recompute_irsw_charges(this.remittance_reload_amt);
            }
            else if (record_is_obps == true)
            {
                this.recompute_obps_charges(this.remittance_reload_amt);
            }
        }
        public void reload_computation_and_override_rates_charges(bool record_is_obps, bool ORD)
        {
            this.waived_charges = false;
            this.compute_override = true;
            this.record_is_reloaded = true;

            if (record_is_obps == false)
            {
                this.recompute_irsw_charges(this.remittance_reload_amt, ORD);
            }

        }
        /* DEPRECATED METHOD. */
        public void reload_computation_entries(int remittance_sequence_id, bool record_is_obps)
        {
            this.reload_computation_entries(record_is_obps);
        }

        public void reload_computation_entries(int remittance_sequence_id, bool record_is_obps, bool ORD)
        {
            this.reload_computation_entries(record_is_obps, ORD);
        }
        public void compute_charges(decimal remitted_amount, bool record_is_obps)
        {
            this.waived_charges = false;
            this.record_is_reloaded = false;
            this.compute_override = false;

            if (record_is_obps == false)
            {
                this.compute_irsw_charges(remitted_amount);
            }
            else
            {
                this.compute_obps_charges(remitted_amount);
            }
        }

        public void compute_waived_charges(decimal remitted_amount, bool record_is_obps)
        {
            this.waived_charges = true;
            this.record_is_reloaded = false;
            this.compute_override = false;

            if (record_is_obps == false)
            {
                this.compute_irsw_charges(remitted_amount);
            }
            else
            {
                this.compute_obps_charges(remitted_amount);
            }
        }
        public void recompute_irsw_charges(decimal remitted_amount)
        {
            this.recompute_transaction_charges(this.remittance_computing_method, remitted_amount);
        }
        public void recompute_irsw_charges(decimal remitted_amount, bool ORD)
        {
            this.recompute_transaction_charges_ord(this.remittance_computing_method, remitted_amount);
        }
        public void compute_irsw_charges(decimal remitted_amount)
        {
            this.compute_transaction_charges(this.remittance_computing_method, remitted_amount);
        }
        public void compute_irsw_charges(decimal remitted_amount, bool ORD)
        {
            this.compute_transaction_charges_ord(this.remittance_computing_method, remitted_amount);
        }
        public void compute_irsw_charges()
        {
            this.compute_transaction_charges(this.remittance_computing_method, this.remittance_reload_amt);
        }

        public void compute_obps_charges(decimal remitted_amount)
        {
            this.compute_obps_charges(this.remittance_computing_method, remitted_amount);
        }
        public void recompute_obps_charges(decimal remitted_amount)
        {
            this.recompute_obps_charges(this.remittance_computing_method, remitted_amount);
        }
        public void recompute_obps_charges(int computing_method, decimal remitted_amount)
        {
            decimal amt_based_on_merchant_currency = 0; this.compute_obps = true;

            if (this.record_is_reloaded == true)
            {
                amt_based_on_merchant_currency = (remitted_amount * (1 / this.remittance_account_currency_rate));
                amt_based_on_merchant_currency = this.get_obps_rounded_amount(Math.Round(amt_based_on_merchant_currency, 2));
                amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

                this.recompute_transaction_charges(computing_method, amt_based_on_merchant_currency);
                this.obps_actual_payment = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);

                return;
            }

            if (this.record_is_reloaded == false && this.compute_override == true)
            {
                if (this.pinoy_currency == this.remittance_account_currency)
                {
                    amt_based_on_merchant_currency = remitted_amount;
                }

                amt_based_on_merchant_currency = (remitted_amount * (1 / this.get_obps_merchants_curr_source_to_target()));
                this.remittance_net_to_bene_total_per_acct_ccy = amt_based_on_merchant_currency;
                amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

                amt_based_on_merchant_currency = this.get_amt_equiv_remitting_ccy(amt_based_on_merchant_currency);
                amt_based_on_merchant_currency = this.get_obps_rounded_amount(Math.Round(amt_based_on_merchant_currency, 2));
                amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

                this.recompute_transaction_charges(computing_method, amt_based_on_merchant_currency);
                this.obps_actual_payment = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);

                return;
            }

            amt_based_on_merchant_currency = remitted_amount / this.get_obps_merchants_curr_source_to_target();
            this.remittance_net_to_bene_total_per_acct_ccy = amt_based_on_merchant_currency;
            amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

            amt_based_on_merchant_currency = this.get_amt_equiv_remitting_ccy(amt_based_on_merchant_currency);
            amt_based_on_merchant_currency = this.get_obps_rounded_amount(Math.Round(amt_based_on_merchant_currency, 2));
            amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

            this.recompute_transaction_charges(computing_method, amt_based_on_merchant_currency);
            this.remittance_net_remit_amt_php_equivalent = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);
            this.obps_actual_payment = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);
        }
        public void compute_obps_charges(int computing_method, decimal remitted_amount)
        {
            decimal amt_based_on_merchant_currency = 0; this.compute_obps = true;

            if (this.record_is_reloaded == true)
            {
                amt_based_on_merchant_currency = (remitted_amount * (1 / this.remittance_account_currency_rate));
                amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

                this.compute_transaction_charges(computing_method, amt_based_on_merchant_currency);
                this.remittance_net_remit_amt_php_equivalent = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);
                this.obps_actual_payment = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);

                return;
            }

            if (this.record_is_reloaded == false && this.compute_override == true)
            {
                if (this.pinoy_currency == this.remittance_account_currency)
                {
                    amt_based_on_merchant_currency = remitted_amount;
                }

                amt_based_on_merchant_currency = (remitted_amount * (1 / this.get_obps_merchants_curr_source_to_target()));
                this.remittance_net_to_bene_total_per_acct_ccy = amt_based_on_merchant_currency;
                amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

                amt_based_on_merchant_currency = this.get_amt_equiv_remitting_ccy(amt_based_on_merchant_currency);
                amt_based_on_merchant_currency = this.get_obps_rounded_amount(Math.Round(amt_based_on_merchant_currency, 2));
                amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

                this.compute_transaction_charges(computing_method, amt_based_on_merchant_currency);
                this.remittance_net_remit_amt_php_equivalent = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);
                this.obps_actual_payment = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);

                return;
            }

            amt_based_on_merchant_currency = remitted_amount / this.get_obps_merchants_curr_source_to_target();
            this.remittance_net_to_bene_total_per_acct_ccy = amt_based_on_merchant_currency;
            amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

            amt_based_on_merchant_currency = this.get_amt_equiv_remitting_ccy(amt_based_on_merchant_currency);
            amt_based_on_merchant_currency = this.get_obps_rounded_amount(Math.Round(amt_based_on_merchant_currency, 2));
            amt_based_on_merchant_currency = Math.Round(amt_based_on_merchant_currency, 2);

            this.compute_transaction_charges(computing_method, amt_based_on_merchant_currency);
            this.remittance_net_remit_amt_php_equivalent = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);
            this.obps_actual_payment = this.currency_rate_target_to_php * Math.Round(this.remittance_net_branch, 2);
        }

        private decimal get_obps_merchants_curr_source_to_target()
        {
            SqlCommand sqcommands = null;
            DataTable dt = null;

            try
            {
                if (this.remittance_account_currency == this.pinoy_currency)
                {
                    return Convert.ToDecimal(1);
                }
                sqcommands = base.getSqlCommand("sp_txn_get_amt_equiv_in_rem_ccy");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar).Value = this.transacting_branch;
                sqcommands.Parameters.Add("@source_currency", SqlDbType.Int).Value = this.remittance_account_currency;
                sqcommands.Parameters.Add("@target_currency", SqlDbType.Int).Value = 1;

                dt = base.getDataSet(sqcommands, "ACCOUNT_EQUIV").Tables[0];

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToDecimal(dt.Rows[0]["SourceToTarget"]);
                    }
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
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        private void override_computed_charges()
        {
            if (dt_computed_charges != null)
            {
                foreach (DataRow dr in this.dt_computed_charges.Rows)
                {
                    dr.BeginEdit();
                    dr["OverrideAmount"] = 0;
                    dr["OverrideFlag"] = true;
                    dr.EndEdit();
                }
            }
        }


        public DataSet get_dataset_record_for_override()
        {
            DataSet ds = new DataSet(); ds.Clear(); ds.EnforceConstraints = false;

            DataTable dt_rem_to_account = new DataTable("REM_TO_ACCOUNT_FOREX");
            DataTable dt_charge_clone = this.dt_charges.Clone();
            DataTable dt_computed_charges_clone = this.dt_computed_charges.Clone();

            dt_charge_clone.Rows.Clear();
            dt_charge_clone.Constraints.Clear();

            dt_computed_charges_clone.Rows.Clear();
            dt_computed_charges_clone.Constraints.Clear();

            dt_rem_to_account.Columns.Add("remitting_to_account", System.Type.GetType("System.Decimal"));
            dt_rem_to_account.Columns.Add("account_to_remitting", System.Type.GetType("System.Decimal"));
            dt_rem_to_account.Columns.Add("remitting_to_account_currency", System.Type.GetType("System.String"));
            dt_rem_to_account.Columns.Add("account_to_remitting_currency", System.Type.GetType("System.String"));

            DataRow dr = dt_rem_to_account.NewRow();

            dr["account_to_remitting"] = 1 / this.currency_rate_target_to_account;
            dr["remitting_to_account"] = this.currency_rate_target_to_account;
            dr["remitting_to_account_currency"] = this.get_description("Currency", 0, this.remittance_account_currency);
            dr["account_to_remitting_currency"] = this.get_description("Currency", 0, this.target_currency);

            dt_rem_to_account.Rows.Add(dr);

            if (this.record_is_reloaded == false)
            {
                if (this.waived_charges == true && this.compute_override == false)
                {
                    for (int j = 0; j < this.dt_computed_charges.Rows.Count; j++)
                    {
                        DataRow rows = dt_computed_charges_clone.NewRow();

                        rows["SEQ_ID"] = dt_computed_charges.Rows[j]["SEQ_ID"];
                        rows["Txn_SEQ_ID"] = dt_computed_charges.Rows[j]["Txn_SEQ_ID"];
                        rows["ChargeName"] = dt_computed_charges.Rows[j]["ChargeName"];
                        rows["ChargeNameDesc"] = dt_computed_charges.Rows[j]["ChargeNameDesc"];
                        rows["Currency"] = dt_computed_charges.Rows[j]["Currency"];
                        rows["CurrencyShortCode"] = dt_computed_charges.Rows[j]["CurrencyShortCode"];
                        rows["Amount"] = dt_computed_charges.Rows[j]["Amount"];
                        rows["ChargeFlag"] = dt_computed_charges.Rows[j]["ChargeFlag"];
                        rows["BeneChargeFlag"] = dt_computed_charges.Rows[j]["BeneChargeFlag"];
                        rows["OverrideFlag"] = 1;
                        rows["OverrideAmount"] = 0;

                        dt_computed_charges_clone.Rows.Add(rows);

                    }

                    if (dt_computed_charges_clone != null)
                    {
                        dt_computed_charges = dt_computed_charges_clone;
                    }
                }
            }

            if (dt_rem_to_account != null)
            {
                ds.Merge(dt_rem_to_account);
            }

            if (this.waived_charges == true && this.compute_override == false)
            {
                if (dt_computed_charges != null)
                {
                    ds.Merge(dt_computed_charges);
                }
            }
            else
            {
                if (this.dt_computed_charges != null)
                {
                    ds.Merge(this.dt_computed_charges);
                }
            }

            ds.AcceptChanges(); return ds;
        }

        public void set_dataset_record_for_override(string override_values)
        {
            if (override_values != null && override_values.Length > 0)
            {
                this.load_override_data_to_datable(override_values.Split('|'));
            }
        }

        public string get_transaction_charge_screen()
        {
            StringBuilder s = new StringBuilder();

            string caption_merchant_beneficiary = "Net to Beneficiary - (A/C Ccy.)";

            if (this.compute_obps == true)
            {
                caption_merchant_beneficiary = "Net to Merchant - (A/C Ccy.)";
            }

            caption_merchant_beneficiary = caption_merchant_beneficiary.ToUpper();

            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_usd_curr_code_desc + " TO USD</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_php_curr_code_desc + " TO PHP</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_eur_curr_code_desc + " TO EUR</td>");
            s.Append("</tr>");
            s.Append("<tr>");


            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.currency_rate_target_to_usd.ToString("#,###.000000000")); s.Append("</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.currency_rate_target_to_php.ToString("#,###.000000000")); s.Append("</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.currency_rate_target_to_eur.ToString("#,###.000000000")); s.Append("</td>");
            s.Append("</tr>");
            s.Append("</table>");
            s.Append("<br style='line-height:3px' />");

            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>LOCAL CURRENCY</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>FOREX RATE</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>REMITTING CURRENCY</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(this.currency_rate_source_to_target_curr_code_desc + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(this.currency_rate_source_to_target.ToString("#,###.000000000") + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_source_curr_code_desc + "</td>");
            s.Append("</tr>");
            s.Append("</table>");

            s.Append("<br style='line-height:3px' />");

            s.Append("<div id = 'div_computed_calc' style= 'overflow:auto; height:auto; width: 100%'>");
            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>CHARGE NAME</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>CURRENCY</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>AMOUNT</td>");
            s.Append("</tr>");

            string charge_name = string.Empty;
            string charge_curr = string.Empty;
            string charge_amount = string.Empty;

            bool override_flag = false;

            if (this.dt_computed_charges != null && this.dt_computed_charges.Rows.Count > 0)
            {
                foreach (DataRow dr in this.dt_computed_charges.Rows)
                {
                    charge_name = this.get_description("ChargeName", 1, Convert.ToInt32(dr["ChargeName"]));
                    charge_curr = this.get_description("Currency", 0, Convert.ToInt32(dr["Currency"]));
                    override_flag = bool.Parse(dr["OverrideFlag"].ToString());

                    if (override_flag == true)
                    {
                        charge_amount = Convert.ToDecimal(dr["OverrideAmount"]).ToString("#,###.00");
                    }
                    else
                    {
                        charge_amount = Convert.ToDecimal(dr["Amount"]).ToString("#,###.00");
                    }

                    s.Append("<tr>");
                    s.Append("<td class='cell_desc' style = 'text-align: left'>"); s.Append(charge_name.ToUpper() + "</td>");
                    s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(charge_curr.ToUpper() + "</td>");
                    s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(charge_amount + "</td>");
                    s.Append("</tr>");
                }
            }

            if (this.remittance_total_backend_charge > 0)
            {
                charge_name = this.get_description("ChargeName", 1, 3);
                charge_curr = this.get_description("Currency", 0, this.target_currency);
                charge_amount = this.remittance_total_backend_charge.ToString("#,###.00");

                s.Append("<tr>");
                s.Append("<td class='cell_desc' style = 'text-align: left'>"); s.Append(charge_name.ToUpper() + "</td>");
                s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(charge_curr.ToUpper() + "</td>");
                s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(charge_amount + "</td>");
                s.Append("</tr>");
            }

            s.Append("</table>");
            s.Append("</div>");
            s.Append("<br style='line-height:3px' />");
            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>DESCRIPTION</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>CURRENCY</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>AMOUNT</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>NET AMOUNT - (RMT CCY.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.target_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.remittance_net_branch.ToString("#,###.00") + "</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>TOTAL CHARGE - (RMT CCY.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.target_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.remittance_total_charges.ToString("#,###.00") + "</td>");
            s.Append("</tr>");

            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>TOTAL CHARGE - (LOCAL CCY.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.source_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(Convert.ToDecimal(this.remittance_total_charges * this.get_currency_rate(this.target_currency, this.source_currency)).ToString("#,###.00") + "</td>");
            s.Append("</tr>");

            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>TOTAL AMOUNT - (RMT CCY.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.target_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.remittance_gross_branch.ToString("#,###.00") + "</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>NET RMT AMOUNT - (PHP EQV.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>PHP</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.remittance_net_remit_amt_php_equivalent.ToString("#,###.00") + "</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>"); s.Append(caption_merchant_beneficiary + "</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.remittance_account_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.remittance_net_to_bene_total_per_acct_ccy.ToString("#,###.00") + "</td>");
            s.Append("</tr>");
            s.Append("</table>");

            return s.ToString();
        }
        public string get_transaction_charge_screen_ord()
        {
            StringBuilder s = new StringBuilder();

            string caption_merchant_beneficiary = "Net to Beneficiary - (A/C Ccy.)";

            if (this.compute_obps == true)
            {
                caption_merchant_beneficiary = "Net to Merchant - (A/C Ccy.)";
            }

            caption_merchant_beneficiary = caption_merchant_beneficiary.ToUpper();

            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_usd_curr_code_desc + " TO USD</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_php_curr_code_desc + " TO PHP</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_eur_curr_code_desc + " TO EUR</td>");
            s.Append("</tr>");
            s.Append("<tr>");


            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.currency_rate_target_to_usd.ToString("#,###.000000000")); s.Append("</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.currency_rate_target_to_php.ToString("#,###.000000000")); s.Append("</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.currency_rate_target_to_eur.ToString("#,###.000000000")); s.Append("</td>");
            s.Append("</tr>");
            s.Append("</table>");
            s.Append("<br style='line-height:3px' />");

            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>LOCAL CURRENCY</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>FOREX RATE</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>REMITTING CURRENCY</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(this.currency_rate_source_to_target_curr_code_desc + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(this.currency_rate_source_to_target.ToString("#,###.000000000") + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(this.currency_rate_target_to_source_curr_code_desc + "</td>");
            s.Append("</tr>");
            s.Append("</table>");

            s.Append("<br style='line-height:3px' />");

            s.Append("<div id = 'div_computed_calc' style= 'overflow:auto; height:auto; width: 100%'>");
            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>CHARGE NAME</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>CURRENCY</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>AMOUNT</td>");
            s.Append("</tr>");

            string charge_name = string.Empty;
            string charge_curr = string.Empty;
            string charge_amount = string.Empty;

            bool override_flag = false;

            if (this.dt_computed_charges != null && this.dt_computed_charges.Rows.Count > 0)
            {
                foreach (DataRow dr in this.dt_computed_charges.Rows)
                {
                    charge_name = this.get_description("ChargeName", 1, Convert.ToInt32(dr["ChargeName"]));
                    charge_curr = this.get_description("Currency", 0, Convert.ToInt32(dr["Currency"]));
                    override_flag = bool.Parse(dr["OverrideFlag"].ToString());

                    if (override_flag == true)
                    {
                        charge_amount = Convert.ToDecimal(dr["OverrideAmount"]).ToString("#,###.00");
                    }
                    else
                    {
                        charge_amount = Convert.ToDecimal(dr["Amount"]).ToString("#,###.00");
                    }

                    s.Append("<tr>");
                    s.Append("<td class='cell_desc' style = 'text-align: left'>"); s.Append(charge_name.ToUpper() + "</td>");
                    s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(charge_curr.ToUpper() + "</td>");
                    s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(charge_amount + "</td>");
                    s.Append("</tr>");
                }
            }

            if (this.remittance_total_backend_charge > 0)
            {
                charge_name = this.get_description("ChargeName", 1, 3);
                charge_curr = this.get_description("Currency", 0, this.target_currency);
                charge_amount = this.remittance_total_backend_charge.ToString("#,###.00");

                s.Append("<tr>");
                s.Append("<td class='cell_desc' style = 'text-align: left'>"); s.Append(charge_name.ToUpper() + "</td>");
                s.Append("<td class='cell_desc' style = 'text-align: center'>"); s.Append(charge_curr.ToUpper() + "</td>");
                s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(charge_amount + "</td>");
                s.Append("</tr>");
            }

            s.Append("</table>");
            s.Append("</div>");
            s.Append("<br style='line-height:3px' />");
            s.Append("<table width='100%' style='border: solid 1px #336699'");
            s.Append("<tr>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>DESCRIPTION</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>CURRENCY</td>");
            s.Append("<td class='cell_caption' style = 'text-align: center'>AMOUNT</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>GROSS AMOUNT - (RMT CCY.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.target_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.remittance_gross_branch.ToString("#,###.00") + "</td>");
            s.Append("</tr>");
            s.Append("<tr>");

            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>TOTAL CHARGE - (BENEF CCY.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.remittance_account_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(Convert.ToDecimal(this.get_total_benef_charges()).ToString("#,###.00") + "</td>");
            s.Append("</tr>");


            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>GROSS BENEF AMOUNT - (BENEF CCY.)</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.remittance_account_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(Convert.ToDecimal(this.remittance_gross_branch * this.currency_rate_target_to_account).ToString("#,###.00") + "</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: left'>"); s.Append(caption_merchant_beneficiary + "</td>");
            s.Append("<td class='cell_desc' style='width: auto; text-align: center'>" + this.get_description("Currency", 0, this.remittance_account_currency) + "</td>");
            s.Append("<td class='cell_desc' style = 'text-align: right'>"); s.Append(this.remittance_net_to_bene_total_per_acct_ccy.ToString("#,###.00") + "</td>");
            s.Append("</tr>");
            s.Append("</table>");

            return s.ToString();
        }

        #endregion


        #region "PRIVATE METHOD"

        private void set_key_value_pair(string tbl_name, int txn_sequence_id, DataTable dt)
        {
            ArrayList error_list = new ArrayList();

            if (tbl_name.ToUpper() == "TBL_REMITTANCE_COMPUTED_CHARGES")
            {
                if (dt == null)
                {
                    if (this.dt_computed_charges != null)
                    {
                        for (int i = 0; i < this.dt_computed_charges.Rows.Count; i++)
                        {
                            if (base.newRow() == true)
                            {
                                if (!base.setColumn("Txn_SEQ_ID", txn_sequence_id)) { error_list.Add("Txn_SEQ_ID: " + base.Message); }
                                if (!base.setColumn("ChargeName", Convert.ToInt32(this.dt_computed_charges.Rows[i]["ChargeName"]))) { error_list.Add("ChargeName: " + base.Message); }
                                if (!base.setColumn("Currency", Convert.ToInt32(this.dt_computed_charges.Rows[i]["Currency"]))) { error_list.Add("Currency: " + base.Message); }
                                if (!base.setColumn("ChargeFlag", Convert.ToInt32(this.dt_computed_charges.Rows[i]["ChargeFlag"]))) { error_list.Add("ChargeFlag: " + base.Message); }
                                if (!base.setColumn("BeneChargeFlag", Convert.ToInt32(this.dt_computed_charges.Rows[i]["BeneChargeFlag"]))) { error_list.Add("BeneChargeFlag: " + base.Message); }
                                if (!base.setColumn("OverrideFlag", Convert.ToInt32(this.dt_computed_charges.Rows[i]["OverrideFlag"]))) { error_list.Add("OverrideFlag: " + base.Message); }
                                if (!base.setColumn("OverrideAmount", Convert.ToInt32(this.dt_computed_charges.Rows[i]["OverrideAmount"]))) { error_list.Add("OverrideAmount " + base.Message); }
                                if (!base.setColumn("Amount", Convert.ToDecimal(this.dt_computed_charges.Rows[i]["Amount"]))) { error_list.Add("Amount: " + base.Message); }
                            }
                            if (base.save() == false)
                            {
                                error_list.Add("TBL_REMITTANCE_COMPUTED_CHARGES - COMMIT FAILED - " + base.Message); base.cancel();
                                this.log_error("ERROR SAVING TBL_REMITTANCE_COMPUTED_CHARGES: " + txn_sequence_id.ToString() + " : " + base.Message);
                            }
                            else
                            {
                                this.commit_charges_success = true;
                            }
                        }
                    }
                }
                else
                {
                    //RELOAD COMPUTATION RATES & EDIT CONTENT  IF POSSIBLE

                    if (dt.Rows.Count > 0)
                    {
                        if (this.dt_computed_charges != null)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (Convert.ToInt32(dt.Rows[i]["ChargeName"]) == Convert.ToInt32(this.dt_computed_charges.Rows[i]["ChargeName"]))
                                {
                                    base.selectedRow = 0;

                                    if (base.edit(Convert.ToInt32(dt.Rows[i]["SEQ_ID"])) == true)
                                    {
                                        if (!base.setColumn("ChargeName", Convert.ToInt32(this.dt_computed_charges.Rows[i]["ChargeName"]))) { error_list.Add("ChargeName: " + base.Message); }
                                        if (!base.setColumn("Currency", Convert.ToInt32(this.dt_computed_charges.Rows[i]["Currency"]))) { error_list.Add("Currency: " + base.Message); }
                                        if (!base.setColumn("ChargeFlag", Convert.ToInt32(this.dt_computed_charges.Rows[i]["ChargeFlag"]))) { error_list.Add("ChargeFlag: " + base.Message); }
                                        if (!base.setColumn("BeneChargeFlag", Convert.ToInt32(this.dt_computed_charges.Rows[i]["BeneChargeFlag"]))) { error_list.Add("BeneChargeFlag: " + base.Message); }
                                        if (!base.setColumn("OverrideFlag", Convert.ToInt32(this.dt_computed_charges.Rows[i]["OverrideFlag"]))) { error_list.Add("OverrideFlag: " + base.Message); }
                                        if (!base.setColumn("OverrideAmount", Convert.ToInt32(this.dt_computed_charges.Rows[i]["OverrideAmount"]))) { error_list.Add("OverrideAmount " + base.Message); }
                                        if (!base.setColumn("Amount", Convert.ToDecimal(this.dt_computed_charges.Rows[i]["Amount"]))) { error_list.Add("Amount: " + base.Message); }

                                        if (base.save() == false)
                                        {
                                            error_list.Add("TBL_REMITTANCE_COMPUTED_CHARGES - COMMIT FAILED - " + base.Message); base.cancel();
                                            this.log_error("ERROR SAVING TBL_REMITTANCE_COMPUTED_CHARGES: " + txn_sequence_id.ToString() + " : " + base.Message);
                                        }
                                        else
                                        {
                                            this.commit_charges_success = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (tbl_name.ToUpper() == "TBL_REMITTANCE_CHARGES_DATA")
            {
                this.requery_charges_data(txn_sequence_id);

                if (dt == null)
                {
                    if (this.dt_charge_data != null)
                    {
                        for (int i = 0; i < this.dt_charge_data.Rows.Count; i++)
                        {
                            if (base.newRow() == true)
                            {
                                if (!base.setColumn("Txn_SEQ_ID", txn_sequence_id)) { error_list.Add("Txn_SEQ_ID: " + base.Message); }
                                if (!base.setColumn("ProductChargesId", Convert.ToInt32(this.dt_charge_data.Rows[i]["ProductChargesId"]))) { error_list.Add("ProductChargesId: " + base.Message); }
                                if (!base.setColumn("BranchCode", Convert.ToString(this.dt_charge_data.Rows[i]["BranchCode"]))) { error_list.Add("BranchCode: " + base.Message); }
                                if (!base.setColumn("ChargesSeqId", Convert.ToInt32(this.dt_charge_data.Rows[i]["ChargesSeqId"]))) { error_list.Add("ChargesSeqId: " + base.Message); }
                                if (!base.setColumn("TieredChargesSeqId", Convert.ToInt32(this.dt_charge_data.Rows[i]["TieredChargesSeqId"]))) { error_list.Add("TieredChargesSeqId: " + base.Message); }
                                if (!base.setColumn("Currency", Convert.ToInt32(this.dt_charge_data.Rows[i]["Currency"]))) { error_list.Add("Currency: " + base.Message); }
                                if (!base.setColumn("ChargeType", Convert.ToInt32(this.dt_charge_data.Rows[i]["ChargeType"]))) { error_list.Add("ChargeType: " + base.Message); }
                                if (!base.setColumn("ComputationLevel", Convert.ToInt32(this.dt_charge_data.Rows[i]["ComputationLevel"]))) { error_list.Add("ComputationLevel: " + base.Message); }
                                if (!base.setColumn("ComputationType", Convert.ToInt32(this.dt_charge_data.Rows[i]["ComputationType"]))) { error_list.Add("ComputationType: " + base.Message); }
                                if (!base.setColumn("ChargeName", Convert.ToInt32(this.dt_charge_data.Rows[i]["ChargeName"]))) { error_list.Add("ChargeName: " + base.Message); }
                                if (!base.setColumn("ChargeValue", Convert.ToDecimal(this.dt_charge_data.Rows[i]["ChargeValue"]))) { error_list.Add("ChargeValue: " + base.Message); }
                                if (!base.setColumn("MinimumAmount", Convert.ToDecimal(this.dt_charge_data.Rows[i]["MinimumAmount"]))) { error_list.Add("MinimumAmount: " + base.Message); }
                                if (!base.setColumn("Denominator", Convert.ToDecimal(this.dt_charge_data.Rows[i]["Denominator"]))) { error_list.Add("Denominator: " + base.Message); }
                                if (!base.setColumn("AmountFrom", Convert.ToDecimal(this.dt_charge_data.Rows[i]["AmountFrom"]))) { error_list.Add("AmountFrom: " + base.Message); }
                                if (!base.setColumn("AmountTo", Convert.ToDecimal(this.dt_charge_data.Rows[i]["AmountTo"]))) { error_list.Add("AmountTo: " + base.Message); }
                                if (!base.setColumn("ChargeAmount", Convert.ToDecimal(this.dt_charge_data.Rows[i]["ChargeAmount"]))) { error_list.Add("ChargeAmount: " + base.Message); }
                                if (!base.setColumn("ChargeToBenef", Convert.ToInt32(this.dt_charge_data.Rows[i]["ChargeToBenef"]))) { error_list.Add("ChargeToBenef: " + base.Message); }
                                if (!base.setColumn("ProductCode", Convert.ToDecimal(this.dt_charge_data.Rows[i]["ProductCode"]))) { error_list.Add("ProductCode: " + base.Message); }
                                if (!base.setColumn("ImplementationType", Convert.ToDecimal(this.dt_charge_data.Rows[i]["ImplementationType"]))) { error_list.Add("ImplementationType: " + base.Message); }
                                if (!base.setColumn("PayMode", Convert.ToInt32(this.dt_charge_data.Rows[i]["PayMode"]))) { error_list.Add("PayMode: " + base.Message); }
                                if (!base.setColumn("ChargeLocally", Convert.ToInt32(this.dt_charge_data.Rows[i]["ChargeLocally"]))) { error_list.Add("ChargeLocally: " + base.Message); }
                                if (!base.setColumn("BeneCurrFlag", Convert.ToBoolean(this.dt_charge_data.Rows[i]["BeneCurrFlag"]))) { error_list.Add("BeneCurrFlag: " + base.Message); }

                                if (base.save() == false)
                                {
                                    error_list.Add("TBL_REMITTANCE_CHARGES_DATA - COMMIT FAILED - " + base.Message); base.cancel();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void commit_transaction_charge_entries(int txn_sequence_id, string tbl_name)
        {
            this.txn_db_table_name = tbl_name;

            DataTable dt = null;

            try
            {
                dt = base.getDataSet(base.getSqlCommand("SELECT * FROM " + tbl_name + " WHERE Txn_SEQ_ID = " + txn_sequence_id + " and deleted= 0", false), "SUNAKO_" + tbl_name).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.set_key_value_pair(tbl_name, txn_sequence_id, dt); return;
                }

                this.set_key_value_pair(tbl_name, txn_sequence_id, null); return;

            }
            catch (Exception e)
            {
                this.commit_charges_success = false;
                this.log_error(e.Message + Environment.NewLine + e.StackTrace);
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

        private decimal get_obps_rounded_amount(decimal amt_paid)
        {
            if (amt_paid == 0)
            {
                return 0;
            }

            string SQL = "SELECT ISNULL(BRANCHDEFCODE,0) AS BRANCHDEFCODE FROM TBL_BRANCH_DEFAULTS WHERE BRANCHCODE ='" + this.transacting_branch + "'" + " AND BRANCHDEFVALUE = 'RoundupOBPSAmount'";

            DataTable dt = getDataSet(getSqlCommand(SQL, false), "TBL_LOCAL_BRANCH_DEF").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["BRANCHDEFCODE"]) == 1)
                    {
                        this.obps_rounded = true;

                        decimal rounded_amt = decimal.Round(amt_paid, 0);

                        if (rounded_amt < amt_paid)
                        {
                            return rounded_amt += 1;
                        }
                        else
                        {
                            return rounded_amt;
                        }
                    }
                }
            }

            return amt_paid;
        }

        private void compute_total_base_on_currency()
        {
            if (this.compute_obps == false)
            {
                if (this.record_is_reloaded == false)
                {
                    if (this.compute_override == false)
                    {
                        this.remittance_net_remit_amt_php_equivalent = this.get_forex_target_to_php() * this.remittance_net_branch;
                        this.remittance_net_to_bene_total_per_acct_ccy = this.currency_rate_target_to_account * this.remittance_net_local;
                        this.remittance_net_to_bene_amt_php_equivalent = this.get_forex_target_to_php() * this.remittance_net_local;
                    }
                    else
                    {
                        if (this.pinoy_currency == this.remittance_account_currency)
                        {
                            this.remittance_net_remit_amt_php_equivalent = this.currency_rate_target_to_account * this.remittance_net_branch;
                            this.remittance_net_to_bene_total_per_acct_ccy = this.currency_rate_target_to_account * this.remittance_net_local;
                            this.remittance_net_to_bene_amt_php_equivalent = this.currency_rate_target_to_account * this.remittance_net_local;
                        }
                        else
                        {
                            this.remittance_net_remit_amt_php_equivalent = ((1 / this.currency_rate_target_to_account) * this.remittance_net_branch);
                            this.remittance_net_to_bene_total_per_acct_ccy = ((1 / this.currency_rate_target_to_account) * this.remittance_net_remit_amt);
                            this.remittance_net_to_bene_amt_php_equivalent = this.get_forex_target_to_php() * this.remittance_net_remit_amt;
                        }
                    }
                }
                else
                {
                    if (this.pinoy_currency == this.remittance_account_currency)
                    {
                        this.remittance_net_remit_amt_php_equivalent = this.currency_rate_target_to_account * this.remittance_net_branch;
                        this.remittance_net_to_bene_total_per_acct_ccy = this.currency_rate_target_to_account * this.remittance_net_local;
                        this.remittance_net_to_bene_amt_php_equivalent = this.get_forex_target_to_php() * this.remittance_net_local;
                    }
                    else
                    {
                        this.remittance_net_remit_amt_php_equivalent = ((1 / this.currency_rate_target_to_account) * this.remittance_net_branch);
                        this.remittance_net_to_bene_total_per_acct_ccy = ((1 / this.currency_rate_target_to_account) * this.remittance_net_local);
                        this.remittance_net_to_bene_amt_php_equivalent = this.get_forex_target_to_php() * this.remittance_net_local;
                    }
                }
            }
        }

        private decimal get_currency_rate(int source_curr, int target_curr)
        {
            //string query_string = "SELECT * FROM V_TBL_CURRENCY_RATES WHERE TranxDate IN (SELECT MAX(TRANXDATE) FROM V_TBL_CURRENCY_RATES WHERE BranchCode = '" + this.transacting_branch + "') AND SourceCurrency =" + source_curr.ToString() + " AND TargetCurrency = " + target_curr.ToString() + " AND BranchCode = '" + this.transacting_branch + "'";
            //Modified by Troy 20131122. Optimized SP to provide faster result.
            string query_string = "DECLARE @Date DATETIME " +
                                    "SELECT @Date = MAX(TranxDate) FROM tbl_Currency_Rates WHERE BranchCode = '" + this.transacting_branch + "' " +
                                    "SELECT * FROM V_TBL_CURRENCY_RATES WHERE TranxDate = @Date AND SourceCurrency =" + source_curr.ToString() + " AND TargetCurrency = " + target_curr.ToString() + " AND BranchCode = '" + this.transacting_branch + "'";

            DataTable dt = getDataSet(getSqlCommand(query_string, false), "CURR_RATES").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToDecimal(dt.Rows[0]["SourceToTarget"]);
                }
            }

            return 0;
        }
        private void recompute_net_to_beneficiary_or_merchant()
        {
            DataRow[] dr_filter = null;

            decimal computed_amount = 0;
            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            this.remittance_total_benef_charge = 0;
            string filterExp = null;

            /* COMPUTE PHP CHARGES */
            this.compute_peso_charges = true;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and ChargeFlag = 1 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                            computed_amount = computed_amount * this.com_peso_to_target_factor;
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }

                        if (Convert.ToInt32(dr_filter[0]["BeneChargeFlag"]) == 1 && computed_amount > 0)
                        {
                            total_local_charges += computed_amount;
                        }
                        else
                        {
                            total_back_end_charges += computed_amount;
                        }
                    }
                    else
                    {
                        dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN (2,3,4,8,12) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString());

                        for (int i = 0; i < dr_filter.Length; i++)
                        {
                            if (this.waived_charges == true && this.compute_override == false)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 1, 0);
                            }

                            if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1 && computed_amount > 0)
                            {
                                total_local_charges += computed_amount;
                            }
                            else
                            {
                                total_back_end_charges += computed_amount;
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here
                        filterExp = "ChargeLocally = 1 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + ")) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString();
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1 && computed_amount > 0)
                            {
                                total_local_charges += computed_amount;
                            }
                            else
                            {
                                total_back_end_charges += computed_amount;
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                            }
                        }
                    }
                }
            }

            //Code End here
            /* END COMPUTE PHP CHARGES */

            /* COMPUTE SUB TOTALS */
            this.remittance_net_remit_amt = this.remittance_base_amount + total_local_charges;
            this.remittance_net_branch = this.remittance_net_remit_amt;
            this.remittance_net_branch *= 1;
            this.remittance_base_amount = this.remittance_net_branch;
            this.remittance_gross_branch = this.remittance_net_branch;
            this.remittance_net_remit_amt -= total_local_charges;
            this.remittance_net_local = this.remittance_net_remit_amt;
            /* END COMPUTE SUB TOTALS */

            /* COMPUTE BRANCH CHARGES */

            this.compute_peso_charges = false;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and ChargeFlag = 0 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            total_branch_charges += computed_amount;
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }
                    }
                    else
                    {
                        dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (2,3,4,8,12) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString());

                        for (int i = 0; i < dr_filter.Length; i++)
                        {
                            if (this.waived_charges == true && this.compute_override == false)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                total_branch_charges += computed_amount;
                                this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 0);
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here
                        filterExp = "ChargeLocally = 0 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + ")) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString();
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                total_branch_charges += computed_amount;
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                            }
                        }
                    }
                }
            }
            //Code End here

            this.remittance_total_charges = total_branch_charges;
            this.remittance_total_charges += total_back_end_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }

            if (this.compute_charges_locally == true)
            {
                this.remittance_gross_branch = this.remittance_net_branch;
            }
            else
            {
                this.remittance_gross_branch = this.remittance_net_branch + this.remittance_total_charges;
            }
            /* END COMPUTE BRANCH CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }
        private void recompute_net_to_beneficiary_or_merchant_ord()
        {
            DataRow[] dr_filter = null;

            decimal computed_amount = 0;
            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            this.remittance_total_benef_charge = 0;
            string filterExp = null;

            /* COMPUTE PHP CHARGES */
            this.compute_peso_charges = true;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and ChargeFlag = 1 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                            computed_amount = computed_amount * this.com_peso_to_target_factor;
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }

                        if (Convert.ToInt32(dr_filter[0]["BeneChargeFlag"]) == 1 && computed_amount > 0)
                        {
                            total_local_charges += computed_amount;
                        }
                        else
                        {
                            total_back_end_charges += computed_amount;
                        }
                    }
                    else
                    {
                        dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN (2,3,4,8,12) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString());

                        for (int i = 0; i < dr_filter.Length; i++)
                        {
                            if (this.waived_charges == true && this.compute_override == false)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 1, 0);
                            }

                            if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1 && computed_amount > 0)
                            {
                                total_local_charges += computed_amount;
                            }
                            else
                            {
                                total_back_end_charges += computed_amount;
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here
                        filterExp = "ChargeLocally = 1 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + ")) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString();
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1 && computed_amount > 0)
                            {
                                total_local_charges += computed_amount;
                            }
                            else
                            {
                                total_back_end_charges += computed_amount;
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                            }
                        }
                    }
                }
            }

            //Code End here
            /* END COMPUTE PHP CHARGES */

            /* COMPUTE SUB TOTALS */
            this.remittance_net_remit_amt = this.remittance_base_amount + total_local_charges;
            this.remittance_net_branch = this.remittance_net_remit_amt;
            this.remittance_net_branch *= 1;
            this.remittance_base_amount = this.remittance_net_branch;
            this.remittance_gross_branch = this.remittance_net_branch;
            this.remittance_net_remit_amt -= total_local_charges;
            this.remittance_net_local = this.remittance_net_remit_amt;
            /* END COMPUTE SUB TOTALS */

            /* COMPUTE BRANCH CHARGES */

            this.compute_peso_charges = false;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and ChargeFlag = 0 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            total_branch_charges += computed_amount; this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }
                    }
                    else
                    {
                        dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (2,3,4,8,12) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString());

                        for (int i = 0; i < dr_filter.Length; i++)
                        {
                            if (this.waived_charges == true && this.compute_override == false)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                total_branch_charges += computed_amount; this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 0);
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here
                        filterExp = "ChargeLocally = 0 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + ")) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString();
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                total_branch_charges += computed_amount; this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                            }
                        }
                    }
                }
            }
            //Code End here

            this.remittance_total_benef_charge += total_branch_charges;
            total_branch_charges = total_branch_charges * (1 / this.currency_rate_target_to_account);
            this.remittance_total_charges = total_branch_charges;
            this.remittance_total_charges += total_back_end_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }

            if (this.compute_charges_locally == true)
            {
                this.remittance_gross_branch = this.remittance_net_branch;
            }
            else
            {
                this.remittance_gross_branch = this.remittance_net_branch + this.remittance_total_charges;
            }
            /* END COMPUTE BRANCH CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }
        private void compute_net_to_beneficiary_or_merchant()
        {
            DataRow[] dr_filter = null;

            decimal computed_amount = 0;
            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            /* COMPUTE PHP CHARGES */

            this.compute_peso_charges = true;

            dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN(15) ");

            for (int i = 0; i < dr_filter.Length; i++)
            {
                if (this.waived_charges == true && this.compute_override == false)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 1);
                }

                if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1 && computed_amount > 0)
                {
                    total_local_charges += computed_amount;
                }
                else
                {
                    total_back_end_charges += computed_amount;
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here
            string filterExp = "ChargeLocally = 1 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";

            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 1);
                }

                if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1 && computed_amount > 0)
                {
                    total_local_charges += computed_amount;
                }
                else
                {
                    total_back_end_charges += computed_amount;
                }
            }
            //Code End here
            /* END COMPUTE PHP CHARGES */

            /* COMPUTE SUB TOTALS */
            this.remittance_net_remit_amt = this.remittance_base_amount + total_local_charges;
            this.remittance_net_branch = this.remittance_net_remit_amt;
            this.remittance_net_branch *= 1;
            this.remittance_base_amount = this.remittance_net_branch;
            this.remittance_gross_branch = this.remittance_net_branch;
            this.remittance_net_remit_amt -= total_local_charges;
            this.remittance_net_local = this.remittance_net_remit_amt;
            /* END COMPUTE SUB TOTALS */

            /* COMPUTE BRANCH CHARGES */

            this.compute_peso_charges = false;

            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN(15) ");

            for (int i = 0; i < dr_filter.Length; i++)
            {
                if (this.waived_charges == true && this.compute_override == false)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    total_branch_charges += computed_amount;
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here
            filterExp = "ChargeLocally = 0 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    total_branch_charges += computed_amount;
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                }
            }
            //Code End here

            this.remittance_total_charges = total_branch_charges;
            this.remittance_total_charges += total_back_end_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }

            if (this.compute_charges_locally == true)
            {
                this.remittance_gross_branch = this.remittance_net_branch;
            }
            else
            {
                this.remittance_gross_branch = this.remittance_net_branch + this.remittance_total_charges;
            }
            /* END COMPUTE BRANCH CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }
        private void compute_net_to_beneficiary_or_merchant_ord()
        {
            DataRow[] dr_filter = null;

            decimal computed_amount = 0;
            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            /* COMPUTE PHP CHARGES */

            this.compute_peso_charges = true;

            dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN(15) ");

            for (int i = 0; i < dr_filter.Length; i++)
            {
                if (this.waived_charges == true && this.compute_override == false)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 1);
                }

                if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1 && computed_amount > 0)
                {
                    total_local_charges += computed_amount;
                }
                else
                {
                    total_back_end_charges += computed_amount;
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here
            string filterExp = "ChargeLocally = 1 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 1);
                }

                if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1 && computed_amount > 0)
                {
                    total_local_charges += computed_amount;
                }
                else
                {
                    total_back_end_charges += computed_amount;
                }
            }
            //Code End here
            /* END COMPUTE PHP CHARGES */

            /* COMPUTE SUB TOTALS */
            this.remittance_net_remit_amt = this.remittance_base_amount + total_local_charges;
            this.remittance_net_branch = this.remittance_net_remit_amt;
            this.remittance_net_branch *= 1;
            this.remittance_base_amount = this.remittance_net_branch;
            this.remittance_gross_branch = this.remittance_net_branch;
            this.remittance_net_remit_amt -= total_local_charges;
            this.remittance_net_local = this.remittance_net_remit_amt;
            /* END COMPUTE SUB TOTALS */

            /* COMPUTE BRANCH CHARGES */

            this.compute_peso_charges = false;

            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN(15) ");

            for (int i = 0; i < dr_filter.Length; i++)
            {
                if (this.waived_charges == true && this.compute_override == false)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    total_branch_charges += computed_amount;
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here
            filterExp = "ChargeLocally = 0 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    total_branch_charges += computed_amount; this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                }
            }
            //Code End here
            this.remittance_total_benef_charge += total_branch_charges;
            total_branch_charges = total_branch_charges * (1 / this.currency_rate_target_to_account);
            this.remittance_total_charges = total_branch_charges;
            this.remittance_total_charges += total_back_end_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }

            if (this.compute_charges_locally == true)
            {
                this.remittance_gross_branch = this.remittance_net_branch;
            }
            else
            {
                this.remittance_gross_branch = this.remittance_net_branch + this.remittance_total_charges;
            }
            /* END COMPUTE BRANCH CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }
        private decimal compute_override_charges(DataRow drow)
        {
            decimal override_amount = 0;
            DataRow[] drfilter = null;

            drfilter = dt_computed_charges.Select("ChargeName = " + drow["ChargeName"].ToString());

            if (drfilter != null && drfilter.Length > 0)
            {
                override_amount = decimal.Parse((drfilter[0]["OverrideAmount"]).ToString());

                if (this.record_is_reloaded == true && this.compute_override_reloaded == false && this.waived_charges == false)
                {
                    return override_amount;
                }

                if (override_amount != decimal.Parse((drfilter[0]["Amount"]).ToString()))
                {
                    drfilter[0].BeginEdit();
                    drfilter[0]["OverrideFlag"] = true;
                    drfilter[0]["Amount"] = override_amount;
                    drfilter[0].EndEdit();
                }
                else
                {
                    if (decimal.Parse((drfilter[0]["OverrideFlag"]).ToString()) == 0)
                    {
                        override_amount = this.compute_charges(Convert.ToInt32(drow["ComputationType"]), drow);

                        if (override_amount != decimal.Parse((drfilter[0]["Amount"]).ToString()))
                        {
                            drfilter[0].BeginEdit();
                            drfilter[0]["OverrideFlag"] = true;
                            drfilter[0]["Amount"] = override_amount;
                            drfilter[0]["OverrideAmount"] = override_amount;
                            drfilter[0].EndEdit();
                        }
                    }
                }
            }

            drfilter = null;

            return override_amount;
        }
        private void check_override_values(DataRow drow)
        {
            DataRow[] drfilter = null;
            this.compute_override = false;

            drfilter = dt_computed_charges.Select("ChargeName = " + drow["ChargeName"].ToString());

            if (drfilter != null && drfilter.Length > 0)
            {
                if (bool.Parse((drfilter[0]["OverrideFlag"]).ToString()) == true)
                {
                    this.compute_override = true;
                }
            }
        }
        private void recompute_transaction_charges(int computing_method, decimal remitted_amount)
        {
            DataRow[] dr_filter = null;

            string filterExp = null;

            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            this.remittance_total_charges = 0;
            this.record_is_reloaded = false;
            this.converted_remitted_amount = remitted_amount;

            if (computing_method.Equals(1) || computing_method.Equals(2))
            {
                /* INTIAILIZATION OF AMOUNT FOR GROSS & NET COMPUTE DIRECTION */

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;
            }
            else if (computing_method.Equals(3))
            {
                /* NET TO BENEF INITIALIZATION */

                if (this.compute_obps == false)
                {
                    if (this.record_is_reloaded == false)
                    {
                        this.converted_remitted_amount = this.get_amt_equiv_remitting_ccy(remitted_amount);
                    }
                }

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;

                this.recompute_net_to_beneficiary_or_merchant();

                return;

                /* END START NET TO BENEF COMPUTATION */
            }
            /* COMPUTE BRANCH CHARGES */

            this.compute_peso_charges = false;

            decimal computed_amount = 0;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and ChargeFlag = 0 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            total_branch_charges += computed_amount;
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }
                    }
                    else
                    {
                        if (computing_method.Equals(1))
                        {
                            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (5,6,7,9,11) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString()); /* GROSS */
                        }

                        if (computing_method.Equals(2))
                        {
                            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (2,3,4,8,12) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString()); /* NET */
                        }

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            for (int i = 0; i < dr_filter.Length; i++)
                            {
                                if (this.waived_charges == true)
                                {
                                    computed_amount = 0;
                                }
                                else
                                {
                                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                                }

                                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                                {
                                    total_branch_charges += computed_amount;
                                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 1);
                                }
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here

                        filterExp = "ChargeLocally = 0 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + ")) and  ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString();
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                total_branch_charges += computed_amount;
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 1);
                            }
                        }
                    }
                }
            }
            //Code End here

            /* END COMPUTE BRANCH CHARGES */


            /* COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */
            this.remittance_total_charges += total_branch_charges;

            if (computing_method.Equals(1))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount;
                    this.remittance_net_branch = this.remittance_base_amount - total_branch_charges;
                    this.remittance_base_amount -= total_branch_charges;
                }
            }

            if (computing_method.Equals(2))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount + total_branch_charges;
                    this.remittance_net_branch = this.converted_remitted_amount;
                }
            }

            /* END COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */

            /* COMPUTE LOCAL PHP CHARGES */

            this.compute_peso_charges = true;
            this.remittance_net_remit_amt = this.remittance_net_branch;
            this.remittance_net_remit_amt *= 1;
            this.remittance_base_amount *= 1;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and chargeflag = 1 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                            computed_amount = computed_amount * this.com_peso_to_target_factor;
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }

                        if (Convert.ToInt32(dr_filter[0]["BeneChargeFlag"]) == 1)
                        {
                            total_local_charges += computed_amount;
                        }
                        else
                        {
                            total_back_end_charges += computed_amount;
                        }
                    }
                    else
                    {
                        dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN (5,6,7,9,11) and ChargeName =" + dt_computed_charges.Rows[x]["ChargeName"].ToString());

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            for (int i = 0; i < dr_filter.Length; i++)
                            {
                                if (this.waived_charges == true && this.compute_override == false)
                                {
                                    computed_amount = 0;
                                }
                                else
                                {
                                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                                }

                                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                                {
                                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 0);
                                }

                                if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1)
                                {
                                    total_local_charges += computed_amount;
                                }
                                else
                                {
                                    total_back_end_charges += computed_amount;
                                }
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here

                        filterExp = "ChargeLocally = 1 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1)
                            {
                                total_local_charges += computed_amount;
                            }
                            else
                            {
                                total_back_end_charges += computed_amount;
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                            }
                        }
                    }
                }
            }
            //Code End here

            this.remittance_total_charges += total_back_end_charges;
            this.remittance_net_local = this.remittance_net_remit_amt - total_local_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                if (computing_method.Equals(1))
                {
                    this.remittance_net_branch -= total_back_end_charges;
                    this.remittance_net_local -= total_back_end_charges;
                }

                if (computing_method.Equals(2))
                {
                    this.remittance_gross_branch += total_back_end_charges;
                }

                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }
            /* END COMPUTE LOCAL PHP CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }

        public void recompute_transaction_charges(int computing_method, decimal remitted_amount, bool file)
        {
            DataRow[] dr_filter = null;

            string filterExp = null;

            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            this.remittance_total_charges = 0;
            this.record_is_reloaded = false;
            this.converted_remitted_amount = remitted_amount;

            if (computing_method.Equals(1) || computing_method.Equals(2))
            {
                /* INTIAILIZATION OF AMOUNT FOR GROSS & NET COMPUTE DIRECTION */

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;
            }
            else if (computing_method.Equals(3))
            {
                /* NET TO BENEF INITIALIZATION */

                if (this.compute_obps == false)
                {
                    if (this.record_is_reloaded == false)
                    {
                        this.converted_remitted_amount = this.get_amt_equiv_remitting_ccy(remitted_amount);
                    }
                }

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;

                this.recompute_net_to_beneficiary_or_merchant();

                return;

                /* END START NET TO BENEF COMPUTATION */
            }
            /* COMPUTE BRANCH CHARGES */

            this.compute_peso_charges = false;

            decimal computed_amount = 0;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and ChargeFlag = 0 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            total_branch_charges += computed_amount;
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }
                    }
                    else
                    {
                        if (computing_method.Equals(1))
                        {
                            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (5,6,7,9,11) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString()); /* GROSS */
                        }

                        if (computing_method.Equals(2))
                        {
                            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (2,3,4,8,12) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString()); /* NET */
                        }

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            for (int i = 0; i < dr_filter.Length; i++)
                            {
                                if (this.waived_charges == true)
                                {
                                    computed_amount = 0;
                                }
                                else
                                {
                                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                                }

                                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                                {
                                    total_branch_charges += computed_amount;
                                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 1);
                                }
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here

                        filterExp = "ChargeLocally = 0 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + ")) and  ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString();
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                total_branch_charges += computed_amount;
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 1);
                            }
                        }
                    }
                }
            }
            //Code End here

            /* END COMPUTE BRANCH CHARGES */


            /* COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */
            this.remittance_total_charges += total_branch_charges;

            if (computing_method.Equals(1))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount;
                    this.remittance_net_branch = this.remittance_base_amount - total_branch_charges;
                    this.remittance_base_amount -= total_branch_charges;
                }
            }

            if (computing_method.Equals(2))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount + total_branch_charges;
                    this.remittance_net_branch = this.converted_remitted_amount;
                }
            }

            /* END COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */

            /* COMPUTE LOCAL PHP CHARGES */

            this.compute_peso_charges = true;
            this.remittance_net_remit_amt = this.remittance_net_branch;
            this.remittance_net_remit_amt *= 1;
            this.remittance_base_amount *= 1;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and chargeflag = 1 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                            computed_amount = computed_amount * this.com_peso_to_target_factor;
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }

                        if (Convert.ToInt32(dr_filter[0]["BeneChargeFlag"]) == 1)
                        {
                            total_local_charges += computed_amount;
                        }
                        else
                        {
                            total_back_end_charges += computed_amount;
                        }
                    }
                    else
                    {
                        dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN (5,6,7,9,11) and ChargeName =" + dt_computed_charges.Rows[x]["ChargeName"].ToString());

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            for (int i = 0; i < dr_filter.Length; i++)
                            {
                                if (this.waived_charges == true && this.compute_override == false)
                                {
                                    computed_amount = 0;
                                }
                                else
                                {
                                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                                }

                                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                                {
                                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 0);
                                }

                                if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1)
                                {
                                    total_local_charges += computed_amount;
                                }
                                else
                                {
                                    total_back_end_charges += computed_amount;
                                }
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here

                        filterExp = "ChargeLocally = 1 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1)
                            {
                                total_local_charges += computed_amount;
                            }
                            else
                            {
                                total_back_end_charges += computed_amount;
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                            }
                        }
                    }
                }
            }
            //Code End here

            this.remittance_total_charges += total_back_end_charges;
            this.remittance_net_local = this.remittance_net_remit_amt - total_local_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                if (computing_method.Equals(1))
                {
                    this.remittance_net_branch -= total_back_end_charges;
                    this.remittance_net_local -= total_back_end_charges;
                }

                if (computing_method.Equals(2))
                {
                    this.remittance_gross_branch += total_back_end_charges;
                }

                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }
            /* END COMPUTE LOCAL PHP CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }

        private void recompute_transaction_charges_ord(int computing_method, decimal remitted_amount)
        {
            DataRow[] dr_filter = null;

            string filterExp = null;

            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            this.remittance_total_benef_charge = 0;
            this.remittance_total_charges = 0;
            this.record_is_reloaded = false;
            this.converted_remitted_amount = remitted_amount;

            if (computing_method.Equals(1) || computing_method.Equals(2))
            {
                /* INTIAILIZATION OF AMOUNT FOR GROSS & NET COMPUTE DIRECTION */
                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;
            }
            else if (computing_method.Equals(3))
            {
                /* NET TO BENEF INITIALIZATION */

                if (this.compute_obps == false)
                {
                    if (this.record_is_reloaded == false)
                    {
                        this.converted_remitted_amount = this.get_amt_equiv_remitting_ccy(remitted_amount);
                    }
                }

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;
                this.recompute_net_to_beneficiary_or_merchant_ord();

                return;

                /* END START NET TO BENEF COMPUTATION */
            }
            /* COMPUTE BRANCH CHARGES */

            this.compute_peso_charges = false;

            decimal computed_amount = 0;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and ChargeFlag = 0 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            total_branch_charges += computed_amount;
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }
                    }
                    else
                    {
                        if (computing_method.Equals(1))
                        {
                            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (5,6,7,9,11) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString()); /* GROSS */
                        }

                        if (computing_method.Equals(2))
                        {
                            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN (2,3,4,8,12) and ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString()); /* NET */
                        }

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            for (int i = 0; i < dr_filter.Length; i++)
                            {
                                if (this.waived_charges == true)
                                {
                                    computed_amount = 0;
                                }
                                else
                                {
                                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                                }

                                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                                {
                                    total_branch_charges += computed_amount;
                                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 1);
                                }
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here

                        filterExp = "ChargeLocally = 0 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + ")) and  ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString();
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                total_branch_charges += computed_amount;
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 1);
                            }
                        }
                    }
                }
            }
            //Code End here

            /* END COMPUTE BRANCH CHARGES */


            /* COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */
            this.remittance_total_benef_charge += total_branch_charges;
            total_branch_charges = total_branch_charges * (1 / this.currency_rate_target_to_account);
            this.remittance_total_charges += total_branch_charges;

            if (computing_method.Equals(1))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount;
                    this.remittance_net_branch = this.remittance_base_amount - total_branch_charges;
                    this.remittance_base_amount -= total_branch_charges;
                }
            }

            if (computing_method.Equals(2))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount + total_branch_charges;
                    this.remittance_net_branch = this.converted_remitted_amount;
                }
            }

            /* END COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */

            /* COMPUTE LOCAL PHP CHARGES */
            this.compute_peso_charges = true;
            this.remittance_net_remit_amt = this.remittance_net_branch;
            this.remittance_net_remit_amt *= 1;
            this.remittance_base_amount *= 1;

            if (this.dt_computed_charges != null)
            {
                for (int x = 0; x < dt_computed_charges.Rows.Count; x++)
                {
                    dr_filter = this.dt_computed_charges_over.Select("ChargeName = " + dt_computed_charges.Rows[x]["ChargeName"].ToString() + " and chargeflag = 1 and OverrideFlag =1 ");

                    if (dr_filter != null && dr_filter.Length > 0)
                    {
                        if (this.waived_charges == true)
                        {
                            computed_amount = 0;
                        }
                        else
                        {
                            computed_amount = Convert.ToDecimal(dr_filter[0]["OverrideAmount"]);
                            computed_amount = computed_amount * this.com_peso_to_target_factor;
                        }

                        if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                        {
                            this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 1, 0);
                        }

                        if (Convert.ToInt32(dr_filter[0]["BeneChargeFlag"]) == 1)
                        {
                            total_local_charges += computed_amount;
                        }
                        else
                        {
                            total_back_end_charges += computed_amount;
                        }
                    }
                    else
                    {
                        dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN (5,6,7,9,11) and ChargeName =" + dt_computed_charges.Rows[x]["ChargeName"].ToString());

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            for (int i = 0; i < dr_filter.Length; i++)
                            {
                                if (this.waived_charges == true && this.compute_override == false)
                                {
                                    computed_amount = 0;
                                }
                                else
                                {
                                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                                }

                                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                                {
                                    this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 0);
                                }

                                if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1)
                                {
                                    total_local_charges += computed_amount;
                                }
                                else
                                {
                                    total_back_end_charges += computed_amount;
                                }
                            }
                        }

                        //Added by Sig  04/08/2010
                        // To handle commulative computation
                        //Code Start Here

                        filterExp = "ChargeLocally = 1 AND ComputationType  IN (22,23) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
                        dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

                        if (dr_filter != null && dr_filter.Length > 0)
                        {
                            computed_amount = 0;

                            if (this.waived_charges == true)
                            {
                                computed_amount = 0;
                            }
                            else
                            {
                                computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                            }

                            if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1)
                            {
                                total_local_charges += computed_amount;
                            }
                            else
                            {
                                total_back_end_charges += computed_amount;
                            }

                            if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                            {
                                this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                            }
                        }
                    }
                }
            }
            //Code End here

            this.remittance_total_charges += total_back_end_charges;
            this.remittance_net_local = this.remittance_net_remit_amt - total_local_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                if (computing_method.Equals(1))
                {
                    this.remittance_net_branch -= total_back_end_charges;
                    this.remittance_net_local -= total_back_end_charges;
                }

                if (computing_method.Equals(2))
                {
                    this.remittance_gross_branch += total_back_end_charges;
                }

                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }
            /* END COMPUTE LOCAL PHP CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }

        private void compute_transaction_charges(int computing_method, decimal remitted_amount)
        {
            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            this.remittance_total_charges = 0;
            this.converted_remitted_amount = remitted_amount;

            if (this.waived_charges == true)
            {
                this.override_computed_charges();
            }

            if (computing_method.Equals(1) || computing_method.Equals(2))
            {
                /* INTIAILIZATION OF AMOUNT FOR GROSS & NET COMPUTE DIRECTION */

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;
            }
            else if (computing_method.Equals(3))
            {
                /* NET TO BENEF INITIALIZATION */

                if (this.compute_obps == false)
                {
                    if (this.record_is_reloaded == false)
                    {
                        this.converted_remitted_amount = this.get_amt_equiv_remitting_ccy(remitted_amount);
                    }
                }

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;

                this.compute_net_to_beneficiary_or_merchant();

                return;

                /* END START NET TO BENEF COMPUTATION */
            }

            DataRow[] dr_filter = null;

            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN(15) "); /* GROSS  AND NET */

            /* COMPUTE BRANCH CHARGES */
            this.compute_peso_charges = false;

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                for (int i = 0; i < dr_filter.Length; i++)
                {
                    if (this.waived_charges == true && this.compute_override == false)
                    {
                        computed_amount = 0;
                    }
                    else
                    {
                        computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                    }

                    if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                    {
                        total_branch_charges += computed_amount; this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 0);
                    }
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here

            string filterExp = "ChargeLocally = 0 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    total_branch_charges += computed_amount;
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                }
            }
            //Code End here

            /* END COMPUTE BRANCH CHARGES */


            /* COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */
            this.remittance_total_charges += total_branch_charges;

            if (computing_method.Equals(1))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount;
                    this.remittance_net_branch = this.remittance_base_amount - total_branch_charges;
                    this.remittance_base_amount -= total_branch_charges;
                }
            }

            if (computing_method.Equals(2))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount + total_branch_charges;
                    this.remittance_net_branch = this.converted_remitted_amount;
                }
            }
            /* END COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */

            /* COMPUTE LOCAL PHP CHARGES */

            this.compute_peso_charges = true;
            this.remittance_net_remit_amt = this.remittance_net_branch;
            this.remittance_net_remit_amt *= 1;
            this.remittance_base_amount *= 1;

            dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN(15)");

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                for (int i = 0; i < dr_filter.Length; i++)
                {
                    if (this.waived_charges == true && this.compute_override == false)
                    {
                        computed_amount = 0;
                    }
                    else
                    {
                        computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                    }

                    if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                    {
                        this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 1);
                    }

                    if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1)
                    {
                        total_local_charges += computed_amount;
                    }
                    else
                    {
                        total_back_end_charges += computed_amount;
                    }
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here
            filterExp = "ChargeLocally = 1 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 1);
                }
                if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1)
                {
                    total_local_charges += computed_amount;
                }
                else
                {
                    total_back_end_charges += computed_amount;
                }
            }
            //Code End here

            this.remittance_total_charges += total_back_end_charges;
            this.remittance_net_local = this.remittance_net_remit_amt - total_local_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                if (computing_method.Equals(1))
                {
                    this.remittance_net_branch -= total_back_end_charges;
                    this.remittance_net_local -= total_back_end_charges;
                }

                if (computing_method.Equals(2))
                {
                    this.remittance_gross_branch += total_back_end_charges;
                }

                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }
            /* END COMPUTE LOCAL PHP CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }

        public void compute_transaction_charges(int computing_method, decimal remitted_amount, bool ord)
        {
            if (ord)
            {
                this.compute_transaction_charges_ord(computing_method, remitted_amount);
            }
            else
            {
                this.compute_transaction_charges(computing_method, remitted_amount);
            }
        }

        public void compute_transaction_charges_ord(int computing_method, decimal remitted_amount)
        {
            decimal total_branch_charges = 0;
            decimal total_local_charges = 0;
            decimal total_back_end_charges = 0;

            this.remittance_total_benef_charge = 0;
            this.remittance_total_charges = 0;
            this.converted_remitted_amount = remitted_amount;

            if (this.waived_charges == true)
            {
                this.override_computed_charges();
            }

            if (computing_method.Equals(1) || computing_method.Equals(2))
            {
                /* INTIAILIZATION OF AMOUNT FOR GROSS & NET COMPUTE DIRECTION */

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;
            }
            else if (computing_method.Equals(3))
            {
                /* NET TO BENEF INITIALIZATION */

                if (this.compute_obps == false)
                {
                    if (this.record_is_reloaded == false)
                    {
                        this.converted_remitted_amount = this.get_amt_equiv_remitting_ccy(remitted_amount);
                    }
                }

                this.remittance_base_amount = this.converted_remitted_amount;
                this.remittance_gross_branch = this.converted_remitted_amount;
                this.remittance_net_branch = this.converted_remitted_amount;
                this.remittance_net_local = this.converted_remitted_amount;

                this.compute_net_to_beneficiary_or_merchant_ord();

                return;

                /* END START NET TO BENEF COMPUTATION */
            }

            DataRow[] dr_filter = null;

            dr_filter = this.dt_charges.Select("ChargeLocally = 0 AND ComputationType NOT IN(15) "); /* GROSS  AND NET */

            /* COMPUTE BRANCH CHARGES */
            this.compute_peso_charges = false;

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                for (int i = 0; i < dr_filter.Length; i++)
                {
                    if (this.waived_charges == true && this.compute_override == false)
                    {
                        computed_amount = 0;
                    }
                    else
                    {
                        computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                    }

                    if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                    {

                        total_branch_charges += computed_amount;
                        this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 0);
                    }
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here

            string filterExp = "ChargeLocally = 0 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    total_branch_charges += computed_amount;
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 0);
                }
            }
            //Code End here

            /* END COMPUTE BRANCH CHARGES */


            /* COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */
            this.remittance_total_benef_charge += total_branch_charges;
            total_branch_charges = total_branch_charges * (1 / this.currency_rate_target_to_account);
            this.remittance_total_charges += total_branch_charges;

            if (computing_method.Equals(1))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount;
                    this.remittance_net_branch = this.remittance_base_amount - total_branch_charges;
                    this.remittance_base_amount -= total_branch_charges;
                }
            }

            if (computing_method.Equals(2))
            {
                if (this.compute_charges_locally == false)
                {
                    this.remittance_gross_branch = this.remittance_base_amount + total_branch_charges;
                    this.remittance_net_branch = this.converted_remitted_amount;
                }
            }
            /* END COMPUTE SUB TOTALS - NET BRANCH REMITTANCE */

            /* COMPUTE LOCAL PHP CHARGES */
            this.compute_peso_charges = true;
            this.remittance_net_remit_amt = this.remittance_net_branch;
            this.remittance_net_remit_amt *= 1;
            this.remittance_base_amount *= 1;

            dr_filter = this.dt_charges.Select("ChargeLocally = 1 AND ComputationType NOT IN (15)");

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                for (int i = 0; i < dr_filter.Length; i++)
                {
                    if (this.waived_charges == true && this.compute_override == false)
                    {
                        computed_amount = 0;
                    }
                    else
                    {
                        computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[i]["ComputationType"]), dr_filter[i]);
                    }

                    if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                    {
                        this.insert_remittance_computed_charges(dr_filter[i], computed_amount, 0, 1);
                    }

                    if (Convert.ToInt32(dr_filter[i]["ChargeToBenef"]) == 1)
                    {
                        total_local_charges += computed_amount;
                    }
                    else
                    {
                        total_back_end_charges += computed_amount;
                    }
                }
            }

            //Added by Sig  04/08/2010
            // To handle commulative computation
            //Code Start Here
            filterExp = "ChargeLocally = 1 AND ComputationType  IN (15) and ((AmountFrom <=" + this.remittance_base_amount + " and AmountTo= 0 ) or (AmountFrom <=" + this.remittance_base_amount + " and AmountTo >=" + this.remittance_base_amount + " ) or (AmountFrom >0  and AmountTo<=" + this.remittance_base_amount + "))";
            dr_filter = this.dt_charges.Select(filterExp, "AmountFrom"); /* GROSS */

            if (dr_filter != null && dr_filter.Length > 0)
            {
                decimal computed_amount = 0;

                if (this.waived_charges == true)
                {
                    computed_amount = 0;
                }
                else
                {
                    computed_amount = this.compute_charges(Convert.ToInt32(dr_filter[0]["ComputationType"]), dr_filter);
                }

                if ((computed_amount > 0 && this.waived_charges == false) || (computed_amount >= 0 && this.waived_charges == true))
                {
                    this.insert_remittance_computed_charges(dr_filter[0], computed_amount, 0, 1);
                }
                if (Convert.ToInt32(dr_filter[0]["ChargeToBenef"]) == 1)
                {
                    total_local_charges += computed_amount;
                }
                else
                {
                    total_back_end_charges += computed_amount;
                }
            }
            //Code End here

            this.remittance_total_charges += total_back_end_charges;
            this.remittance_net_local = this.remittance_net_remit_amt - total_local_charges;
            this.remittance_total_backend_charge = total_back_end_charges;

            if (total_back_end_charges > 0)
            {
                if (computing_method.Equals(1))
                {
                    this.remittance_net_branch -= total_back_end_charges;
                    this.remittance_net_local -= total_back_end_charges;
                }

                if (computing_method.Equals(2))
                {
                    this.remittance_gross_branch += total_back_end_charges;
                }

                DataRow[] dr = this.dt_computed_charges.Select("ChargeName = 3");

                if (dr != null && dr.Length > 0)
                {
                    dr[0].BeginEdit();
                    dr[0]["Amount"] = total_back_end_charges;
                    dr[0]["ComputationType"] = 1;
                    dr[0].EndEdit();
                }
            }
            /* END COMPUTE LOCAL PHP CHARGES */

            /* COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
            this.compute_total_base_on_currency();
            /* END COMPUTE PESO & ACCOUNT CURRENCY EQUIVALENT VALUE */
        }
        private void load_branch_local_currency()
        {
            string SQL = "SELECT * FROM TBL_BRANCH_DEFAULTS WHERE BranchCode = '" + this.transacting_branch + "' AND BranchDefValue = 'LocalCurrency'";

            DataTable dt = getDataSet(getSqlCommand(SQL, false), "TBL_LOCAL_BRANCH_DEF").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    this.source_currency = Convert.ToInt32(dt.Rows[0]["BranchDefCode"]);
                }
            }
        }

        private decimal get_target_to_account_rate()
        {
            DataTable dt = null;
            SqlCommand sqcommands = null;

            try
            {
                sqcommands = base.getSqlCommand("sp_txn_get_amt_equiv_in_rem_ccy");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar).Value = this.transacting_branch;
                sqcommands.Parameters.Add("@source_currency", SqlDbType.Int).Value = this.target_currency;
                sqcommands.Parameters.Add("@target_currency", SqlDbType.Int).Value = this.remittance_account_currency;

                dt = base.getDataSet(sqcommands, "ACCOUNT_EQUIV").Tables[0];

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToDecimal(dt.Rows[0]["SourceToTarget"]);
                    }
                }

                return 0;
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
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return 0;
        }

        private decimal get_amt_equiv_remitting_ccy(decimal amt)
        {
            if (this.record_is_reloaded == true)
            {
                return this.remittance_account_currency_rate;
            }

            decimal account_equivalent = 0;

            if (this.remittance_account_currency != 0)
            {
                if (this.remittance_account_currency == this.target_currency)
                {
                    account_equivalent = 1;
                }
                else if (this.remittance_account_currency == this.pinoy_currency)
                {
                    account_equivalent = this.currency_rate_target_to_php;
                }
                else if (this.remittance_account_currency == this.kano_currency)
                {
                    account_equivalent = this.currency_rate_target_to_usd;
                }
                else if (this.remittance_account_currency == this.euro_currency)
                {
                    account_equivalent = this.currency_rate_target_to_eur;
                }
                else
                {
                    account_equivalent = this.get_target_to_account_rate();
                }
            }


            if (account_equivalent > 0)
            {
                return (amt * (1 / account_equivalent));
            }

            return 0;
        }

        private string get_description(string fldgroup, int fldisplay, int fldvalue)
        {
            string SQL = "SELECT FldCode, FldDescription FROM TBL_LOOKUP WHERE FldGroup ='" + fldgroup + "'" + " AND FldValue = " + fldvalue;

            DataTable dt = getDataSet(getSqlCommand(SQL, false)).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                if (fldisplay == 1)
                {
                    return Convert.ToString(dt.Rows[0]["FldDescription"]);
                }
                else
                {
                    return Convert.ToString(dt.Rows[0]["FldCode"]);
                }
            }
            return string.Empty;
        }

        private void insert_remittance_computed_charges(DataRow drows)
        {
            if (record_is_reloaded == false)
            {
                return;
            }

            if (this.dt_computed_charges != null && bool.Parse((drows["OverrideFlag"]).ToString()) == true)
            {
                DataRow dr2 = dt_computed_charges.NewRow();

                dr2["SEQ_ID"] = 0;
                dr2["Txn_SEQ_ID"] = 0;
                dr2["ChargeName"] = drows["ChargeName"];
                dr2["ChargeNameDesc"] = drows["ChargeNameDesc"];
                dr2["Currency"] = drows["Currency"];
                dr2["CurrencyShortCode"] = drows["CurrencyShortCode"];
                dr2["Amount"] = drows["Amount"];
                dr2["ChargeFlag"] = drows["ChargeFlag"];
                dr2["OverrideFlag"] = drows["OverrideFlag"];
                dr2["OverrideAmount"] = drows["OverrideAmount"];
                dr2["BeneChargeFlag"] = drows["BeneChargeFlag"];

                dt_computed_charges.Rows.Add(dr2);
            }
        }
        private void insert_remittance_charges_data(DataRow drows)
        {
            if (this.dt_charge_data != null)
            {
                DataRow dr = this.dt_charge_data.NewRow();

                dr["BranchCode"] = this.transacting_branch;
                dr["ComputationLevel"] = this.remittance_computing_method;

                dr["SEQ_ID"] = 0;
                dr["Txn_SEQ_ID"] = 0;

                dr["ProductChargesId"] = drows["ProductChargesId"];
                dr["ChargesSeqId"] = drows["ChargesSeqId"];
                dr["TieredChargesSeqId"] = drows["TieredChargesSeqId"];
                dr["ProductCode"] = drows["ProductCode"];
                dr["ImplementationType"] = drows["ImplementationType"];
                dr["PayMode"] = drows["PayMode"];
                dr["Currency"] = drows["Currency"];
                dr["ChargeType"] = drows["ChargeType"];
                dr["ComputationType"] = drows["ComputationType"];
                dr["ChargeName"] = drows["ChargeName"];
                dr["ChargeValue"] = drows["ChargeValue"];
                dr["MinimumAmount"] = drows["MinimumAmount"];
                dr["Denominator"] = drows["Denominator"];
                dr["ChargeToBenef"] = drows["ChargeToBenef"];
                dr["AmountFrom"] = drows["AmountFrom"];
                dr["AmountTo"] = drows["AmountTo"];
                dr["ChargeAmount"] = drows["ChargeAmount"];
                dr["ChargeLocally"] = drows["ChargeLocally"];
                dr["BeneCurrFlag"] = drows["BeneCurrFlag"];

                this.dt_charge_data.Rows.Add(dr);
            }
        }
        private void insert_remittance_charges_data(DataRow drows, int txt_seq_id)
        {
            if (this.dt_charge_data != null)
            {
                DataRow dr = this.dt_charge_data.NewRow();

                dr["Txn_SEQ_ID"] = txt_seq_id;
                dr["SEQ_ID"] = drows["SEQ_ID"];
                dr["ProductChargesId"] = drows["ProductChargesId"];
                dr["BranchCode"] = drows["BranchCode"];
                dr["ChargesSeqId"] = drows["ChargesSeqId"];
                dr["TieredChargesSeqId"] = drows["TieredChargesSeqId"];
                dr["ProductCode"] = drows["ProductCode"];
                dr["ImplementationType"] = drows["ImplementationType"];
                dr["PayMode"] = drows["PayMode"];
                dr["Currency"] = drows["Currency"];
                dr["ChargeType"] = drows["ChargeType"];
                dr["ComputationLevel"] = drows["ComputationLevel"];
                dr["ComputationType"] = drows["ComputationType"];
                dr["ChargeName"] = drows["ChargeName"];
                dr["ChargeValue"] = drows["ChargeValue"];
                dr["MinimumAmount"] = drows["MinimumAmount"];
                dr["Denominator"] = drows["Denominator"];
                dr["ChargeToBenef"] = drows["ChargeToBenef"];
                dr["AmountFrom"] = drows["AmountFrom"];
                dr["AmountTo"] = drows["AmountTo"];
                dr["ChargeAmount"] = drows["ChargeAmount"];
                dr["ChargeLocally"] = drows["ChargeLocally"];
                dr["BeneCurrFlag"] = drows["BeneCurrFlag"];

                this.dt_charge_data.Rows.Add(dr);
            }
        }
        private void insert_remittance_computed_charges(DataRow drow, decimal computed_amount, int override_flag, int charge_flag)
        {
            DataRow[] drfilter = null;

            drfilter = dt_computed_charges.Select("ChargeName = " + drow["ChargeName"].ToString());

            if (drfilter != null && drfilter.Length > 0)
            {
                if (this.compute_peso_charges == true)
                {
                    computed_amount = decimal.Round((computed_amount * this.com_target_to_peso_factor), 7);
                }

                if (override_flag == 1 || this.waived_charges == true)
                {
                    drfilter[0].BeginEdit();
                    drfilter[0]["OverrideAmount"] = computed_amount;
                    drfilter[0]["OverrideFlag"] = 1;
                    drfilter[0].EndEdit();
                    drfilter = null;
                }
                else
                {
                    drfilter[0].BeginEdit();
                    drfilter[0]["Amount"] = computed_amount;
                    drfilter[0].EndEdit();
                    drfilter = null;
                }

                return;
            }

            DataRow dr = null;

            if (this.dt_computed_charges != null)
            {
                dr = this.dt_computed_charges.NewRow();

                if (override_flag == 1 || Convert.ToInt32(drow["ComputationType"]) == 14)
                {
                    dr["OverrideFlag"] = 1;
                }
                else
                {
                    dr["OverrideFlag"] = 0;
                }

                dr["SEQ_ID"] = 0;
                dr["Txn_SEQ_ID"] = 0;
                dr["OverrideAmount"] = 0;
                dr["ChargeFlag"] = charge_flag;
                dr["ChargeName"] = drow["ChargeName"];
                dr["ChargeNameDesc"] = this.get_description("ChargeName", 1, int.Parse((drow["ChargeName"]).ToString()));

                if (this.compute_peso_charges == true)
                {
                    dr["Currency"] = 1;
                    dr["CurrencyShortCode"] = this.get_description("Currency", 0, 1);
                    dr["Amount"] = Decimal.Round((computed_amount * this.com_target_to_peso_factor), 7);
                }
                else
                {
                    dr["Currency"] = drow["Currency"];
                    dr["CurrencyShortCode"] = this.get_description("Currency", 0, int.Parse((drow["Currency"]).ToString()));
                    dr["Amount"] = computed_amount;
                }

                dr["BeneChargeFlag"] = drow["ChargeToBenef"];

                if (this.waived_charges == true && this.compute_override == false)
                {
                    dr["Amount"] = 0;
                    dr["OverrideAmount"] = 0;
                }

                if (this.compute_override == true)
                {
                    dr["OverrideAmount"] = drow["ChargeValue"];
                }

                if (this.waived_charges == true || this.compute_override == true)
                {
                    dr["OverrideFlag"] = 1;
                }

                this.dt_computed_charges.Rows.Add(dr);
            }

            dr = null;
        }


        private void construct_working_table()
        {
            if (this.dt_charge_data == null)
            {
                this.dt_charge_data = new DataTable("REM_CHARGES_DATA");

                this.dt_charge_data.Columns.Add("SEQ_ID", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("Txn_SEQ_ID", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ProductChargesId", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("BranchCode", System.Type.GetType("System.String"));
                this.dt_charge_data.Columns.Add("ChargesSeqId", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("TieredChargesSeqId", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ProductCode", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ImplementationType", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("PayMode", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("Currency", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ChargeType", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ComputationLevel", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ComputationType", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ChargeName", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("ChargeValue", System.Type.GetType("System.Decimal"));
                this.dt_charge_data.Columns.Add("MinimumAmount", System.Type.GetType("System.Decimal"));
                this.dt_charge_data.Columns.Add("Denominator", System.Type.GetType("System.Decimal"));
                this.dt_charge_data.Columns.Add("ChargeToBenef", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("AmountFrom", System.Type.GetType("System.Decimal"));
                this.dt_charge_data.Columns.Add("AmountTo", System.Type.GetType("System.Decimal"));
                this.dt_charge_data.Columns.Add("ChargeAmount", System.Type.GetType("System.Decimal"));
                this.dt_charge_data.Columns.Add("ChargeLocally", System.Type.GetType("System.Int32"));
                this.dt_charge_data.Columns.Add("BeneCurrFlag", System.Type.GetType("System.Boolean"));
            }

            if (this.dt_computed_charges == null)
            {
                this.dt_computed_charges = new DataTable("REM_COMPUTED_CHARGES");
                this.dt_computed_charges.Columns.Add("SEQ_ID", System.Type.GetType("System.Int32"));
                this.dt_computed_charges.Columns.Add("Txn_SEQ_ID", System.Type.GetType("System.Int32"));
                this.dt_computed_charges.Columns.Add("ChargeName", System.Type.GetType("System.Int32"));
                this.dt_computed_charges.Columns.Add("ChargeNameDesc", System.Type.GetType("System.String"));
                this.dt_computed_charges.Columns.Add("Currency", System.Type.GetType("System.Int32"));
                this.dt_computed_charges.Columns.Add("CurrencyShortCode", System.Type.GetType("System.String"));
                this.dt_computed_charges.Columns.Add("Amount", System.Type.GetType("System.Decimal"));
                this.dt_computed_charges.Columns.Add("ChargeFlag", System.Type.GetType("System.Int32"));
                this.dt_computed_charges.Columns.Add("BeneChargeFlag", System.Type.GetType("System.Int32"));
                this.dt_computed_charges.Columns.Add("OverrideFlag", System.Type.GetType("System.Boolean"));
                this.dt_computed_charges.Columns.Add("OverrideAmount", System.Type.GetType("System.Decimal"));
                this.dt_computed_charges.Columns.Add("ComputationType", System.Type.GetType("System.Int32"));
            }
        }

        private decimal get_local_to_usd_factor()
        {
            return this.currency_rate_source_to_usd;
        }

        private decimal get_target_to_usd_factor()
        {
            return this.currency_rate_target_to_usd;
        }

        private decimal get_usd_to_target_factor()
        {
            if (this.currency_rate_target_to_usd > 0)
            {
                return 1 / this.currency_rate_target_to_usd;
            }
            return 0;
        }

        private decimal get_account_to_peso_factor()
        {
            return this.currency_rate_target_to_php;
        }

        private decimal get_peso_to_target_factor()
        {
            if (this.currency_rate_target_to_php > 0)
            {
                return 1 / this.currency_rate_target_to_php;
            }
            return 0;
        }

        private decimal get_usd_to_peso_factor()
        {
            return this.currency_rate_usd_to_php;
        }

        private decimal get_target_to_peso_factor()
        {
            if (this.currency_rate_target_to_php > 0)
            {
                return 1 / currency_rate_target_to_php;
            }
            return 0;
        }

        private void load_product_charges_data()
        {
            base.EnforceConstraint = false;

            SqlCommand sqcommands = base.getSqlCommand("sp_calculator_product_charges");

            sqcommands.Parameters.Add("@product_charges_id", SqlDbType.Int).Value = this.product_charges_id;

            this.dt_charges = base.getDataSet(sqcommands, "PRODUCT_CHARGES").Tables[0];

            if (this.dt_charges != null && this.dt_charges.Rows.Count > 0)
            {
                this.target_currency = Convert.ToInt32(this.dt_charges.Rows[0]["Currency"]);
                if (this.remittance_computing_method == 0)
                {
                    this.remittance_computing_method = Convert.ToInt32(this.dt_charges.Rows[0]["ComputationLevel"]);
                    this.charges_based_on_account_currency = Convert.ToBoolean(dt_charges.Rows[0]["BeneCurrFlag"]);
                }
                for (int i = 0; i < this.dt_charges.Rows.Count; i++)
                {
                    this.insert_remittance_charges_data(this.dt_charges.Rows[i]);
                }
            }

            if (sqcommands != null)
            {
                sqcommands.Dispose();
                sqcommands = null;
            }
        }
        private void delete_product_charges_data(int tranx_id)
        {
            try
            {
                execScalar("sp_calculator_delete_product_charges", new object[] { tranx_id });
            }

            catch (Exception ex)
            {
                throw (ex);
            }
        }

        private void load_product_computed_charges(int remittance_sequence_id)
        {
            base.EnforceConstraint = false;

            SqlCommand sqcommands = base.getSqlCommand("sp_reload_computed_charges");

            sqcommands.Parameters.Add("@remittance_sequence_id", SqlDbType.Int).Value = remittance_sequence_id;

            this.dt_computed_charges = base.getDataSet(sqcommands, "RELOAD_COMPUTED_CHARGES").Tables[0];
            this.load_override_data_to_datable();

            if (sqcommands != null)
            {
                sqcommands.Dispose();
                sqcommands = null;
            }
        }
        private void load_product_charges_data(int remittance_sequence_id)
        {
            base.EnforceConstraint = false;

            SqlCommand sqcommands = base.getSqlCommand("sp_calculator_reload_product_charges");

            sqcommands.Parameters.Add("@remittance_sequence_id", SqlDbType.Int).Value = remittance_sequence_id;

            this.dt_charges = base.getDataSet(sqcommands, "RELOAD_PRODUCT_CHARGES").Tables[0];

            if (this.dt_charges != null && this.dt_charges.Rows.Count > 0)
            {
                this.remittance_computing_method = Convert.ToInt32(this.dt_charges.Rows[0]["ComputationLevel"]);

                for (int i = 0; i < this.dt_charges.Rows.Count; i++)
                {
                    this.insert_remittance_charges_data(this.dt_charges.Rows[i]);
                }
            }

            if (sqcommands != null)
            {
                sqcommands.Dispose();
                sqcommands = null;
            }
        }

        private decimal convert_remitted_amt_value(decimal remitted_amount)
        {
            if (this.currency_rate_source_to_target > 0)
            {
                return (1 / this.currency_rate_source_to_target) * remitted_amount;
            }
            return 0;
        }


        private void set_currency_factor_to_variable()
        {
            if (this.dt_rates != null && this.dt_rates.Rows.Count > 0)
            {
                this.currency_rate_source_to_target = Convert.ToDecimal(this.dt_rates.Rows[0]["SourceToTarget"]);
                this.currency_rate_source_to_target_curr_code = Convert.ToInt32(this.dt_rates.Rows[0]["SourceCurrency"]);
                this.currency_rate_source_to_target_curr_code_desc = Convert.ToString(this.dt_rates.Rows[0]["SourceCurrShort"]);
                this.currency_rate_target_to_source = Convert.ToDecimal(this.dt_rates.Rows[0]["TargetToSource"]);
                this.currency_rate_target_to_source_curr_code = Convert.ToInt32(this.dt_rates.Rows[0]["TargetCurrency"]);
                this.currency_rate_target_to_source_curr_code_desc = Convert.ToString(this.dt_rates.Rows[0]["TargetCurrShort"]);

                foreach (DataRow dr in this.dt_rates.Rows)
                {
                    decimal rate_value = Convert.ToDecimal(dr["currency_factor_value"]);

                    switch (Convert.ToString(dr["currency_factor_code"]))
                    {
                        case "currency_rate_target_to_usd":

                            this.currency_rate_target_to_usd = rate_value;
                            this.currency_rate_target_to_usd_curr_code = Convert.ToInt32(this.dt_rates.Rows[0]["TargetCurrency"]);
                            this.currency_rate_target_to_usd_curr_code_desc = Convert.ToString(this.dt_rates.Rows[0]["TargetCurrShort"]);
                            break;

                        case "currency_rate_target_to_php":

                            this.currency_rate_target_to_php = rate_value;
                            this.currency_rate_target_to_php_curr_code = Convert.ToInt32(this.dt_rates.Rows[0]["TargetCurrency"]);
                            this.currency_rate_target_to_php_curr_code_desc = Convert.ToString(this.dt_rates.Rows[0]["TargetCurrShort"]);
                            break;

                        case "currency_rate_target_to_eur":

                            this.currency_rate_target_to_eur = rate_value;
                            this.currency_rate_target_to_eur_curr_code = Convert.ToInt32(this.dt_rates.Rows[0]["TargetCurrency"]);
                            this.currency_rate_target_to_eur_curr_code_desc = Convert.ToString(this.dt_rates.Rows[0]["TargetCurrShort"]);
                            break;

                        case "currency_rate_source_to_usd":

                            this.currency_rate_source_to_usd = rate_value;
                            this.currency_rate_source_to_usd_code = Convert.ToInt32(dr["SourceCurrency"]);
                            this.currency_rate_source_to_usd_code_desc = Convert.ToString(dr["SourceCurrShort"]);
                            break;

                        case "currency_rate_source_to_php":

                            this.currency_rate_source_to_php = rate_value;
                            this.currency_rate_source_to_php_code = Convert.ToInt32(dr["SourceCurrency"]);
                            this.currency_rate_source_to_php_code_desc = Convert.ToString(dr["SourceCurrShort"]);
                            break;

                        case "currency_rate_usd_to_php":

                            this.currency_rate_usd_to_php = rate_value;
                            break;
                    }
                }
            }
        }
        private void load_override_data_to_datable()
        {
            if (dt_computed_charges != null)
            {
                dt_computed_charges_over = this.dt_computed_charges.Clone();
                dt_computed_charges_over.Rows.Clear();
                dt_computed_charges_over.Constraints.Clear();

                foreach (DataRow drow in dt_computed_charges.Rows)
                {

                    DataRow dr = dt_computed_charges_over.NewRow();

                    dr["SEQ_ID"] = drow["SEQ_ID"];
                    dr["Txn_SEQ_ID"] = drow["Txn_SEQ_ID"];
                    dr["ChargeName"] = drow["ChargeName"];
                    dr["ChargeNameDesc"] = drow["ChargeNameDesc"];
                    dr["Currency"] = drow["Currency"];
                    dr["CurrencyShortCode"] = drow["CurrencyShortCode"];
                    dr["Amount"] = drow["Amount"];
                    dr["ChargeFlag"] = drow["ChargeFlag"];
                    dr["OverrideAmount"] = drow["OverrideAmount"];
                    dr["OverrideFlag"] = drow["OverrideFlag"];
                    dr["BeneChargeFlag"] = drow["BeneChargeFlag"];
                    dt_computed_charges_over.Rows.Add(dr);
                }
            }
        }
        private void load_override_data_to_datable(string[] s)
        {
            string icharges = null;
            string[] charge_array = null;

            if (s != null && s.Length > 0)
            {
                this.compute_override = true;

                if (this.record_is_reloaded == true)
                {
                    this.compute_override_reloaded = true;
                }

                if (dt_computed_charges != null)
                {
                    dt_computed_charges_over = this.dt_computed_charges.Clone();
                    dt_computed_charges_over.Rows.Clear();
                    dt_computed_charges_over.Constraints.Clear();
                }

                for (int i = 0; i < s.Length; i++)
                {
                    string[] iarrays = s[i].Split('~');

                    if (iarrays[0] == "FOREX_RATES")
                    {
                        if (iarrays[1] == "remitting_to_account_rate")
                        {
                            this.currency_rate_target_to_account = Convert.ToDecimal(iarrays[2]);
                        }
                        if (iarrays[1] == "account_to_remitting_rate")
                        {
                            this.currency_rate_account_to_target = Convert.ToDecimal(iarrays[2]);
                        }
                    }

                    if (iarrays[0] == "CHARGES_DATA")
                    {
                        icharges += iarrays[0] + "~" + iarrays[1] + "~" + iarrays[2] + "|";
                    }
                }

                if (this.remittance_account_currency == this.pinoy_currency)
                {
                    this.currency_rate_target_to_php = this.currency_rate_target_to_account;
                    this.com_peso_to_target_factor = 1 / this.currency_rate_target_to_php;
                    this.com_target_to_peso_factor = this.currency_rate_target_to_account;
                }

                if (this.remittance_account_currency == this.kano_currency)
                {
                    this.currency_rate_target_to_usd = this.currency_rate_target_to_account;
                }

                if (this.remittance_account_currency == this.euro_currency)
                {
                    this.currency_rate_target_to_eur = this.currency_rate_target_to_account;
                }

                charge_array = icharges.Split('|');

                for (int j = 0; j < charge_array.Length; j++)
                {
                    string[] charge_det = charge_array[j].Split('~');

                    if (charge_array[j] == string.Empty)
                    {
                        break;
                    }

                    if (Convert.ToInt32(this.dt_computed_charges.Rows[j]["ChargeName"]) == Convert.ToInt32(charge_det[1]))
                    {
                        DataRow dr = dt_computed_charges_over.NewRow();

                        dr["SEQ_ID"] = dt_computed_charges.Rows[j]["SEQ_ID"];
                        dr["Txn_SEQ_ID"] = dt_computed_charges.Rows[j]["Txn_SEQ_ID"];
                        dr["ChargeName"] = dt_computed_charges.Rows[j]["ChargeName"];
                        dr["ChargeNameDesc"] = dt_computed_charges.Rows[j]["ChargeNameDesc"];
                        dr["Currency"] = dt_computed_charges.Rows[j]["Currency"];
                        dr["CurrencyShortCode"] = dt_computed_charges.Rows[j]["CurrencyShortCode"];
                        dr["Amount"] = dt_computed_charges.Rows[j]["Amount"];
                        dr["ChargeFlag"] = dt_computed_charges.Rows[j]["ChargeFlag"];

                        if (Convert.ToDecimal(dt_computed_charges.Rows[j]["Amount"]) != Convert.ToDecimal(charge_det[2]))
                        {
                            dr["OverrideAmount"] = Convert.ToDecimal(charge_det[2]);
                            dr["OverrideFlag"] = 1;
                        }

                        dr["BeneChargeFlag"] = dt_computed_charges.Rows[j]["BeneChargeFlag"];
                        dt_computed_charges_over.Rows.Add(dr);
                    }
                }
            }
        }

        private void load_currency_special_rates(int rate_type)
        {
            SqlCommand sqcommands = null;

            this.currency_rate_target_to_account = this.get_target_to_account_rate();
            this.currency_rate_target_to_account_curr_code = this.remittance_account_currency;
            this.currency_rate_target_to_account_curr_code_desc = this.get_description("Currency", 0, this.remittance_account_currency);

            try
            {
                sqcommands = base.getSqlCommand("sp_calculator_curr_special_rates");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar).Value = this.transacting_branch;
                sqcommands.Parameters.Add("@SourceCurrency", SqlDbType.SmallInt).Value = this.source_currency;
                sqcommands.Parameters.Add("@TargetCurrency", SqlDbType.SmallInt).Value = this.target_currency;
                sqcommands.Parameters.Add("@RateType", SqlDbType.SmallInt).Value = rate_type;

                this.dt_rates = base.getDataSet(sqcommands, "CURRENCY_FACTOR").Tables[0];
                this.set_currency_factor_to_variable();

            }
            catch
            {
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        private void load_currency_factor()
        {
            SqlCommand sqcommands = null;

            this.currency_rate_target_to_account = this.get_target_to_account_rate();
            this.currency_rate_target_to_account_curr_code = this.remittance_account_currency;
            this.currency_rate_target_to_account_curr_code_desc = this.get_description("Currency", 0, this.remittance_account_currency);

            try
            {
                sqcommands = base.getSqlCommand("sp_calculator_curr_rates");
                sqcommands.Parameters.Add("@branch_code", SqlDbType.VarChar).Value = this.transacting_branch;
                sqcommands.Parameters.Add("@SourceCurrency", SqlDbType.SmallInt).Value = this.source_currency;
                sqcommands.Parameters.Add("@TargetCurrency", SqlDbType.SmallInt).Value = this.target_currency;

                this.dt_rates = base.getDataSet(sqcommands, "CURRENCY_FACTOR").Tables[0];
                this.set_currency_factor_to_variable();

            }
            catch
            {
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        private void load_currency_factor(int remittance_sequence_id)
        {
            SqlCommand sqcommands = null;

            try
            {
                base.EnforceConstraint = false;

                sqcommands = base.getSqlCommand("sp_calculator_reload_curr_rates");
                sqcommands.Parameters.Add("@remittance_sequence_id", SqlDbType.Int).Value = remittance_sequence_id;

                this.dt_rates = base.getDataSet(sqcommands, "RELOAD_CURRENCY_FACTOR").Tables[0];
                this.set_currency_factor_to_variable();

            }
            catch
            {
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        #endregion


        #region "COMPUTATION TYPE"
        //added by sig jayme to handled comulative charges
        private decimal compute_charges(int computation_type, DataRow[] dr)
        {
            if (computation_type > 0)
            {
                //if (computation_type.Equals(22) || computation_type.Equals(23)) //Added by Sig Jayme 04/08/2010
                if (computation_type.Equals(15)) //Added by Sig Jayme 04/08/2010
                {
                    return this.comp_type_tiered_fix_amount_commulative(dr);
                }
            }
            return 0;
        }

        private decimal compute_charges(int computation_type, DataRow dr)
        {
            if (computation_type > 0)
            {
                if (computation_type > 0)
                {
                    if (computation_type.Equals(1) || computation_type.Equals(9))
                    {
                        return this.comp_type_fixed_amount(dr);
                    }

                    if (computation_type.Equals(2))
                    {
                        return this.comp_type_rate_actual(dr);
                    }

                    if (computation_type.Equals(3))
                    {
                        return this.comp_type_rate_round_to_tenth(dr);
                    }

                    if (computation_type.Equals(4))
                    {
                        return this.comp_type_rate_round_to_int(dr);
                    }

                    if (computation_type.Equals(5))
                    {
                        return this.comp_type_tiered_fix_amount(dr, true);
                    }

                    if (computation_type.Equals(6))
                    {
                        return this.comp_type_05_cents(dr);
                    }

                    if (computation_type.Equals(10))
                    {
                        return this.comp_type_tiered_fix_amount(dr, false);
                    }

                    if (computation_type.Equals(11))
                    {
                        return this.comp_type_hk_php_charge_per_hk(dr);
                    }

                    if (computation_type.Equals(6))
                    {
                        return this.comp_type_per_amount(dr, false);
                    }

                    if (computation_type.Equals(8))
                    {
                        return this.comp_type_per_usd_amount(dr);
                    }

                    if (computation_type.Equals(12))
                    {
                        return this.comp_type_usd_fixed_amount(dr);
                    }

                    if (computation_type.Equals(13))
                    {
                        return this.comp_type_per_amount_round_to_int(dr);
                    }

                    if (computation_type.Equals(14))
                    {
                        return this.comp_type_per_amount(dr, true);
                    }
                }
            }

            return 0;
        }

        private decimal comp_type_fixed_amount(DataRow dr)
        {
            this.computed_amt = 0;

            if (this.remittance_base_amount > 0)
            {
                this.computed_amt = Convert.ToDecimal(dr["ChargeValue"]);

                if (Convert.ToInt32(dr["ComputationType"]) == 1)
                {
                    if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                    {
                        this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                    }
                }

                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt *= this.com_peso_to_target_factor;
                    }
                }
            }

            return computed_amt;
        }

        private decimal comp_type_rate_actual(DataRow dr)
        {
            this.computed_amt = 0;

            if (this.remittance_base_amount > 0)
            {
                if (this.compute_peso_charges == false)
                {
                    this.computed_amt = (Convert.ToDecimal(dr["ChargeValue"]) / 100) * this.remittance_base_amount;

                    if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                    {
                        this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                    }
                }
                else
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt = (Convert.ToDecimal(dr["ChargeValue"]) / 100);
                        this.computed_amt *= (this.remittance_base_amount * this.com_target_to_peso_factor);

                        if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                        {
                            this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                        }

                        this.computed_amt *= this.com_peso_to_target_factor;

                    }
                    else
                    {
                        this.computed_amt = (Convert.ToDecimal(dr["ChargeValue"]) / 100);

                        if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                        {
                            this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                        }
                    }
                }
            }
            return this.computed_amt;
        }

        private decimal comp_type_rate_round_to_tenth(DataRow dr)
        {
            this.computed_amt = 0;

            if (this.remittance_base_amount > 0)
            {
                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt = decimal.Round((Convert.ToDecimal(dr["ChargeValue"]) / 100) * (this.remittance_base_amount * this.com_target_to_peso_factor), 1);

                        if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                        {
                            this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                        }

                        this.computed_amt *= this.com_peso_to_target_factor; return this.computed_amt;
                    }
                }

                this.computed_amt = decimal.Round((Convert.ToDecimal(dr["ChargeValue"]) / 100) * this.remittance_base_amount, 1);

                if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                {
                    this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]); return this.computed_amt;
                }
            }

            return this.computed_amt;
        }

        private decimal comp_type_rate_round_to_int(DataRow dr)
        {
            this.computed_amt = 0;

            if (this.remittance_base_amount > 0)
            {
                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt = Decimal.Round((Convert.ToDecimal(dr["ChargeValue"]) / 100) * (this.remittance_base_amount * this.com_target_to_peso_factor), 0);

                        if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                        {
                            this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                        }

                        this.computed_amt *= this.com_peso_to_target_factor; return this.computed_amt;
                    }
                }

                this.computed_amt = Decimal.Round((Convert.ToDecimal(dr["ChargeValue"]) / 100) * this.remittance_base_amount, 0);

                if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                {
                    this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]); return this.computed_amt;
                }
            }

            return this.computed_amt;
        }


        private decimal comp_type_per_amount(DataRow dr, bool isRounded_to_Ceiling)
        {
            this.computed_amt = 0;

            decimal tempo_amt = this.remittance_base_amount;

            if (this.remittance_base_amount > 0 && Convert.ToDecimal(dr["Denominator"]) > 0)
            {
                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        tempo_amt = this.remittance_base_amount * this.com_target_to_peso_factor;
                    }
                }

                if (tempo_amt < 1)
                {
                    return this.computed_amt;
                }

                if (isRounded_to_Ceiling == true)
                {
                    this.computed_amt = Convert.ToDecimal(dr["ChargeValue"]) * Math.Ceiling(tempo_amt / Convert.ToDecimal(dr["Denominator"]));
                }
                else
                {
                    this.computed_amt = Convert.ToDecimal(dr["ChargeValue"]) * (tempo_amt / Convert.ToDecimal(dr["Denominator"]));
                }

                if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                {
                    this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                }

                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt *= this.com_peso_to_target_factor;
                    }
                }
            }

            return this.computed_amt;
        }

        private decimal comp_type_05_cents(DataRow dr)
        {
            decimal tempo_amt = 0;
            decimal tempo_divisor = 0.05M;

            this.computed_amt = 0;

            if (this.remittance_base_amount > 0)
            {
                tempo_amt = this.remittance_base_amount;

                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        tempo_amt = this.remittance_base_amount * this.com_target_to_peso_factor;
                    }
                }

                this.computed_amt = decimal.Round((Convert.ToDecimal(dr["ChargeValue"]) / 100) * tempo_amt, 2);

                if (this.computed_amt % tempo_divisor > Convert.ToDecimal(0.025))
                {
                    this.computed_amt = (this.computed_amt - (this.computed_amt % tempo_divisor)) + tempo_divisor;
                }
                else
                {
                    this.computed_amt -= (this.computed_amt % tempo_divisor);
                }

                if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                {
                    this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                }

                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt *= this.com_peso_to_target_factor;
                    }
                }
            }

            return this.computed_amt;
        }

        private decimal comp_type_per_usd_amount(DataRow dr)
        {
            decimal conv_input_amt = 0;

            this.computed_amt = 0;

            if (this.remittance_base_amount > 0 && Convert.ToDecimal(dr["Denominator"]) > 0)
            {
                if (this.compute_peso_charges == false)
                {
                    conv_input_amt = this.remittance_base_amount * this.com_local_to_usd_factor;
                }
                else
                {
                    if (this.target_currency == this.kano_currency)
                    {
                        conv_input_amt = this.remittance_base_amount;
                    }
                    else
                    {
                        conv_input_amt = this.remittance_base_amount * this.com_target_to_usd_factor;
                    }
                }

                this.computed_amt = Convert.ToDecimal(dr["ChargeValue"]) * (conv_input_amt / Convert.ToDecimal(dr["Denominator"]));

                if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                {
                    this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                }
                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt *= this.com_peso_to_target_factor;
                    }
                }
            }

            return computed_amt;
        }

        private decimal comp_type_hk_php_charge_per_hk(DataRow dr)
        {
            decimal tempo_amt = 0;

            this.computed_amt = 0;

            if (this.target_currency != this.pinoy_currency)
            {
                return 0;
            }

            if (this.remittance_base_amount > 0)
            {
                if (this.compute_peso_charges == false)
                {
                    tempo_amt = remittance_base_amount * 1;

                    this.computed_amt = (Convert.ToDecimal(dr["ChargeValue"]) / 100) * tempo_amt;

                    if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                    {
                        this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                    }
                }
            }
            return this.computed_amt;
        }

        private decimal comp_type_usd_fixed_amount(DataRow dr)
        {
            this.computed_amt = 0;

            if (this.remittance_base_amount > 0)
            {
                if (Convert.ToInt32(dr["ComputationType"]) == 19)
                {
                    this.computed_amt = Convert.ToDecimal(dr["ChargeValue"]) * (1 / this.currency_rate_source_to_usd);

                    if (this.computed_amt < Convert.ToDecimal(dr["MinimumAmount"]))
                    {
                        this.computed_amt = Convert.ToDecimal(dr["MinimumAmount"]);
                    }
                }
            }
            return this.computed_amt;
        }

        private decimal comp_type_tiered_fix_amount(DataRow dr, bool amount_is_fixed)
        {
            this.computed_amt = this.remittance_base_amount;

            decimal charge_amount = 0;
            decimal starting_amt = 0;
            decimal ending_amt = 0;

            if ((this.charges_based_on_account_currency == true) && (this.remittance_computing_method != 3))
                return charge_amount;
            if ((this.charges_based_on_account_currency == false) && (this.remittance_computing_method == 3))
                return charge_amount;

            if (this.compute_peso_charges == true)
            {
                if (this.pinoy_currency != this.target_currency)
                {
                    this.computed_amt = this.remittance_base_amount * this.get_target_to_peso_factor();
                }
            }

            if (this.computed_amt < 1)
            {
                return 0;
            }

            if (dr != null)
            {
                starting_amt = Convert.ToDecimal(dr["AmountFrom"]);
                ending_amt = Convert.ToDecimal(dr["AmountTo"]);

                if (starting_amt <= this.computed_amt && ending_amt >= this.computed_amt)
                {
                    charge_amount = Convert.ToDecimal(dr["ChargeAmount"]);
                }
            }

            if (charge_amount > 0)
            {
                if (amount_is_fixed == true)
                {
                    if (this.compute_peso_charges == true)
                    {
                        charge_amount *= this.com_target_to_peso_factor;
                    }
                }
                else
                {
                    if (this.compute_peso_charges == true)
                    {
                        if (this.pinoy_currency != this.target_currency)
                        {
                            charge_amount = ((charge_amount / 100) * this.computed_amt) * this.com_target_to_peso_factor;
                        }
                    }
                    else
                    {
                        charge_amount = ((charge_amount / 100) * this.computed_amt);
                    }
                }
            }

            return charge_amount;
        }

        private decimal comp_type_per_amount_round_to_int(DataRow dr)
        {
            decimal charge_amount = 0;

            this.computed_amt = 0;

            if (this.remittance_base_amount > 0 && Convert.ToDecimal(dr["Denominator"]) > 0)
            {
                this.computed_amt = this.remittance_base_amount;

                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        this.computed_amt = this.remittance_base_amount * this.com_target_to_peso_factor;
                    }
                }

                if (this.computed_amt < 1)
                {
                    return 0;
                }

                charge_amount = Convert.ToDecimal(dr["ChargeValue"]) * (this.computed_amt / Convert.ToDecimal(dr["Denominator"]));

                if (this.computed_amt % Convert.ToDecimal(dr["Denominator"]) == 0)
                {
                    charge_amount = Convert.ToDecimal(dr["ChargeValue"]) * (this.computed_amt / Convert.ToDecimal(dr["Denominator"]));
                }
                else
                {
                    if (this.computed_amt > Convert.ToDecimal(dr["Denominator"]))
                    {
                        charge_amount = Convert.ToDecimal(dr["ChargeValue"]) * Math.Ceiling(this.computed_amt / Convert.ToDecimal(dr["Denominator"]));
                    }
                    else
                    {
                        charge_amount = Math.Round(Convert.ToDecimal(dr["ChargeValue"]) * (this.computed_amt / Convert.ToDecimal(dr["Denominator"])), 2);
                    }
                }

                if (charge_amount < Convert.ToDecimal(dr["MinimumAmount"]))
                {
                    charge_amount = Convert.ToDecimal(dr["MinimumAmount"]);
                }

                if (this.compute_peso_charges == true)
                {
                    if (this.pinoy_currency != this.target_currency)
                    {
                        charge_amount *= this.com_peso_to_target_factor;
                    }
                }
            }

            return charge_amount;
        }

        //Added by Sig Jayme 04/08/10
        //To handle Commulative  Tiered computation
        private decimal comp_type_tiered_fix_amount_commulative(DataRow[] dr)
        {
            this.computed_amt = this.remittance_base_amount;

            decimal charge_amount = 0;
            decimal alternative_charge_amount = 0;
            decimal total_charge_amount = 0;
            decimal prev_ending_amt = 0;
            decimal ending_amt = 0;

            this.computed_amt = this.remittance_base_amount * this.com_target_to_usd_factor; // get dollar equivalent

            if (this.computed_amt < 1)
            {
                return 0;
            }

            if (dr != null && dr.Length > 0)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    ending_amt = Convert.ToDecimal(dr[i]["AmountTo"]);
                    charge_amount = Convert.ToDecimal(dr[i]["ChargeAmount"]);
                    alternative_charge_amount = Convert.ToDecimal(dr[i]["MinimumAmount"]);


                    if (prev_ending_amt > 0)
                    {
                        ending_amt -= prev_ending_amt;
                        computed_amt -= prev_ending_amt;
                    }

                    prev_ending_amt = ending_amt;

                    if (charge_amount > 0)
                    {
                        decimal amount_multiplier = 0;
                        if (i == (dr.Length - 1))
                            amount_multiplier = computed_amt;
                        else
                        {
                            if (ending_amt < computed_amt)
                                amount_multiplier = ending_amt;
                            else
                                amount_multiplier = computed_amt;
                        }
                        charge_amount = (charge_amount * (amount_multiplier * this.com_usd_to_php_factor));
                        if (this.compute_peso_charges == true)
                        {
                            if (this.pinoy_currency != this.target_currency)
                            {
                                charge_amount *= this.com_peso_to_target_factor;
                            }
                        }


                        if (charge_amount < alternative_charge_amount)
                            charge_amount = alternative_charge_amount;

                        total_charge_amount += charge_amount;

                    }
                }


            }
            return total_charge_amount;
        }
        #endregion

        #region "CLEAN CALCULATOR SESSION AND VARIABLE"

        private void clean_all_vars()
        {
            if (this.dt_charge_data != null)
            {
                this.dt_charge_data.Clear();
                this.dt_charge_data.Dispose();
            }

            if (this.dt_computed_charges != null)
            {
                this.dt_computed_charges.Clear();
                this.dt_computed_charges.Dispose();
            }

            if (this.dt_rates != null)
            {
                this.dt_rates.Clear();
                this.dt_rates.Dispose();
            }

            if (this.dt_charges != null)
            {
                this.dt_charges.Clear();
                this.dt_charges.Dispose();
            }

            if (dt_computed_charges_over != null)
            {
                this.dt_computed_charges_over.Clear();
                this.dt_computed_charges_over.Dispose();
            }

            this.dt_charge_data = null;
            this.dt_computed_charges = null;
            this.dt_rates = null;
            this.dt_charges = null;
            this.dt_computed_charges_over = null;

            this.pinoy_currency = 1;
            this.kano_currency = 2;
            this.euro_currency = 5;
            this.remittance_reload_amt = 0;
            this.computed_amt = 0;
            this.compute_peso_charges = false;
            this.compute_charges_locally = false;
            this.obps_rounded = false;
            this.compute_obps = false;
            this.waived_charges = false;
            this.compute_override = false;
            this.compute_override_reloaded = false;
            this.charges_based_on_account_currency = false;
            this.transacting_branch = null;
            this.product_charges_id = 0;
            this.rem_txn_id = 0;
            this.source_currency = 0;
            this.target_currency = 0;

            /* CURRENCY RATE VALUE */
            this.currency_rate_target_to_source = 0;
            this.currency_rate_target_to_usd = 0;
            this.currency_rate_target_to_php = 0;
            this.currency_rate_target_to_eur = 0;
            this.currency_rate_source_to_target = 0;
            this.currency_rate_source_to_usd = 0;
            this.currency_rate_source_to_php = 0;
            this.currency_rate_target_to_account = 0;
            this.currency_rate_account_to_target = 0;
            this.currency_rate_usd_to_php = 0;
            /* END CURRENCY RATE VALUE */

            /* CURRENCY CODE & DESCRIPTION - TARGET */
            this.currency_rate_target_to_account_curr_code = 0;
            this.currency_rate_target_to_source_curr_code = 0;
            this.currency_rate_target_to_usd_curr_code = 0;
            this.currency_rate_target_to_php_curr_code = 0;
            this.currency_rate_target_to_eur_curr_code = 0;

            this.currency_rate_target_to_account_curr_code_desc = string.Empty;
            this.currency_rate_target_to_source_curr_code_desc = string.Empty;
            this.currency_rate_target_to_usd_curr_code_desc = string.Empty;
            this.currency_rate_target_to_php_curr_code_desc = string.Empty;
            this.currency_rate_target_to_eur_curr_code_desc = string.Empty;
            /* END CURRENCY CODE - TARGET */

            /* CURRENCY CODE & DESCRIPTION - SOURCE  */
            this.currency_rate_source_to_target_curr_code = 0;
            this.currency_rate_source_to_usd_code = 0;
            this.currency_rate_source_to_php_code = 0;

            this.currency_rate_source_to_target_curr_code_desc = string.Empty;
            this.currency_rate_source_to_usd_code_desc = string.Empty;
            this.currency_rate_source_to_php_code_desc = string.Empty;
            /* END CURRENCY CODE & DESCRIPTION - SOURCE  */

            /* REMITTANCE CONVERTED AMONT */
            this.converted_remitted_amount = 0;
            this.com_target_to_peso_factor = 0;
            this.com_peso_to_target_factor = 0;
            this.com_local_to_usd_factor = 0;
            this.com_target_to_usd_factor = 0;
            this.com_usd_to_php_factor = 0;
            /* END REMITTANCE CONVERTED AMONT */

            /* REMITTANCE NET AND GROSS COMPUTED AMOUNT */
            this.remittance_base_amount = 0;
            this.remittance_gross_branch = 0;
            this.remittance_net_branch = 0;
            this.remittance_net_local = 0;
            this.remittance_net_remit_amt = 0;
            this.remittance_total_charges = 0;
            this.remittance_net_to_bene_total_per_acct_ccy = 0;
            this.remittance_net_remit_amt_php_equivalent = 0;
            this.remittance_net_to_bene_amt_php_equivalent = 0;
            this.remittance_total_backend_charge = 0;
            this.remittance_total_benef_charge = 0;
            /* END REMITTANCE NET AND GROSS COMPUTED AMOUNT */

            this.record_is_reloaded = false;
            this.remittance_account_currency_rate = 0; /* REMITTANCE DESTINATION ACCOUNT CURRENCY RATE (BENEFICIARY ACCOUNT, MERCHANT, BANK CK, OTHER BANK ACCOUNT)*/
            this.remittance_account_currency = 0; /* REMITTANCE DESTINATION ACCOUNT CURRENCY (BENEFICIARY ACCOUNT, MERCHANT, BANK CK, OTHER BANK ACCOUNT)*/
            this.remittance_computing_method = 0;
            this.obps_actual_payment = 0;
            this.txn_db_table_name = string.Empty;

            try
            {
                System.Web.HttpContext.Current.Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_ID_HANDLER"] = null;
            }
            catch
            {
            }
        }

        #endregion

        #region "REQUERY CHARGES DATA"

        private void requery_charges_data(long transaction_sequence_id)
        {
            object product_seq = base.execScalar("select productchargesid from tbl_remittance where seq_id = " + transaction_sequence_id);

            if (product_seq != null && string.IsNullOrEmpty(Convert.ToString(product_seq)) == false)
            {
                if (this.dt_charge_data != null)
                {
                    this.dt_charge_data.Rows.Clear();
                }
                else
                {
                    this.dt_charge_data = new DataTable("REM_CHARGES_DATA");

                    this.dt_charge_data.Columns.Add("SEQ_ID", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("Txn_SEQ_ID", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ProductChargesId", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("BranchCode", System.Type.GetType("System.String"));
                    this.dt_charge_data.Columns.Add("ChargesSeqId", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("TieredChargesSeqId", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ProductCode", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ImplementationType", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("PayMode", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("Currency", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ChargeType", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ComputationLevel", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ComputationType", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ChargeName", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("ChargeValue", System.Type.GetType("System.Decimal"));
                    this.dt_charge_data.Columns.Add("MinimumAmount", System.Type.GetType("System.Decimal"));
                    this.dt_charge_data.Columns.Add("Denominator", System.Type.GetType("System.Decimal"));
                    this.dt_charge_data.Columns.Add("ChargeToBenef", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("AmountFrom", System.Type.GetType("System.Decimal"));
                    this.dt_charge_data.Columns.Add("AmountTo", System.Type.GetType("System.Decimal"));
                    this.dt_charge_data.Columns.Add("ChargeAmount", System.Type.GetType("System.Decimal"));
                    this.dt_charge_data.Columns.Add("ChargeLocally", System.Type.GetType("System.Int32"));
                    this.dt_charge_data.Columns.Add("BeneCurrFlag", System.Type.GetType("System.Boolean"));
                    this.dt_charge_data.Columns.Add("ChargeCurrency", System.Type.GetType("System.Int32"));
                }

                this.product_charges_id = Convert.ToInt32(product_seq);
                this.load_product_charges_data();
            }
        }

        #endregion

        #region LOG AUDIT & NEW YORK
        private void log_error(string error_message)
        {
            csAuditLog cs_audit = new csAuditLog();

            try
            {
                cs_audit.writeAudit("CALCULATOR", "CALCULATOR", "CALCULATOR LOG", error_message, string.Empty, csAuditLog.Category.Insert);
            }
            catch
            {
            }
            finally
            {
                if (cs_audit != null)
                {
                    cs_audit = null;
                }
            }
        }
        private void commit_remittance_charges_data(int txn_sequence_id)
        {
            int i = 0;

            try
            {
                i = base.execStoredProcedureUpdate("insert_calculator_product_charges", new object[] { txn_sequence_id });

                if (Convert.ToInt32(i) > 0)
                {
                    this.log_error("commit_remittance_charges_data method: status: saved: " + txn_sequence_id); return;
                }

                /* RETRY 1 */
                if (Convert.ToInt32(i) < 0)
                {
                    i = base.execStoredProcedureUpdate("insert_calculator_product_charges", new object[] { txn_sequence_id });
                }

                if (Convert.ToInt32(i) > 0)
                {
                    this.log_error("commit_remittance_charges_data method: status: saved: " + txn_sequence_id); return;
                }
                /* RETRY 1 */


                /* RETRY 2 */
                if (Convert.ToInt32(i) < 0)
                {
                    i = base.execStoredProcedureUpdate("insert_calculator_product_charges", new object[] { txn_sequence_id });
                }

                if (Convert.ToInt32(i) > 0)
                {
                    this.log_error("commit_remittance_charges_data method: status: saved: " + txn_sequence_id); return;
                }
                /* RETRY 2 */

                /* RETRY 3 */
                if (Convert.ToInt32(i) < 0)
                {
                    i = base.execStoredProcedureUpdate("insert_calculator_product_charges", new object[] { txn_sequence_id });
                }

                if (Convert.ToInt32(i) > 0)
                {
                    this.log_error("commit_remittance_charges_data method: status: saved: " + txn_sequence_id); return;
                }
                /* RETRY 3 */

                if (Convert.ToInt32(i) < 0)
                {
                    this.log_error("commit_remittance_charges_data method: unable to save: " + txn_sequence_id.ToString() + " - " + base.Message);
                }
            }
            catch
            {
                this.commit_transaction_charge_entries(txn_sequence_id, "TBL_REMITTANCE_CHARGES_DATA");
            }
        }


        #endregion

    }
}
