using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.TranClass
{
    [Serializable()]
    public class RemTranCommons : sqlHelperWs
    {
        public RemTranCommons()
        {
            base.EnforceConstraint = false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] user_can_override_rates(string user_name, string user_pwd)
        {
            irsUsers irsusers = new irsUsers();
            
            Object[] ireturns =  irsusers.validate(user_name, user_pwd, Convert.ToByte(1), true);

            bool logged_flag = false;
            string reason = string.Empty;

            if (ireturns != null)
            {
                return new Object[] { ireturns[0], ireturns[1]};
            }

            return new Object[] {false , "CAN NOT VALIDATE USER AT THE MOMENT."};
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_calculator_for_override_values(string irs_calculator_session_name)
        {
            if (System.Web.HttpContext.Current.Session[irs_calculator_session_name] != null)
            {
                return ((irsCalculator)System.Web.HttpContext.Current.Session[irs_calculator_session_name]).get_dataset_record_for_override();
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_calculator_override_values(string override_values, string irs_calculator_session_name)
        {
            if (System.Web.HttpContext.Current.Session[irs_calculator_session_name] != null)
            {
                irsCalculator icalc = (irsCalculator)System.Web.HttpContext.Current.Session[irs_calculator_session_name];

                icalc.set_dataset_record_for_override(override_values);
                System.Web.HttpContext.Current.Session[irs_calculator_session_name] = icalc;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_calculator_computed_values()
        {
            if (System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"] != null)
            {
                irsCalculator icalc = (irsCalculator)System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"];

                StringBuilder s = new StringBuilder();

                s.Append(icalc.get_branch_local_currency()        + "~");
                s.Append(icalc.get_remittance_gross_branch()      + "~");
                s.Append(icalc.get_remittance_net_remit_amt()     + "~");
                s.Append(icalc.get_remittance_net_to_bene_amt()   + "~");
                s.Append(icalc.get_remittance_total_charges_amt() + "~");
                s.Append(icalc.get_forex_target_to_account()      + "~");
                s.Append(icalc.get_forex_target_to_source()       + "~");
                s.Append(icalc.get_forex_target_to_usd()          + "~");
                s.Append(icalc.get_forex_target_to_php()          + "~");
                s.Append(icalc.get_forex_target_to_eur()          + "~");
                s.Append(icalc.get_forex_source_to_target()       + "~");
                s.Append(icalc.get_forex_source_to_usd()          + "~");
                s.Append(icalc.get_forex_source_to_php()          + "~");

                return s.ToString();
            }

            return string.Empty;
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_account_information(string SEQ_ID)
        {
            SqlCommand sqcommands = base.getSqlCommand("sp_txn_account_information");

            sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt).Value = Convert.ToInt32(SEQ_ID);

            DataSet ds = base.getDataSet(sqcommands, "ACCOUNT_INFO");

            if (ds != null)
            {
                return ds;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_beneficiary_account_information(string bene_id, string branch_type)
        {
            SqlCommand sqcommands = base.getSqlCommand("sp_txn_benef_accounts");

            sqcommands.Parameters.Add("@bene_id", SqlDbType.VarChar).Value = bene_id;
            sqcommands.Parameters.Add("@bank_type", SqlDbType.Int).Value = Convert.ToInt32(branch_type);

            DataSet ds = base.getDataSet(sqcommands, "BENEF_ACCOUNT_INFO");

            if (ds != null)
            {
                return ds;
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_charge_previous_transaction_result(string sequence_id)
        {
            System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"] = null;

            irsCalculator icalc = new irsCalculator(Convert.ToInt32(sequence_id));

            if (icalc != null)
            {
                icalc.reload_computation_entries(false);

                System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"] = icalc;

                return icalc.get_transaction_charge_screen();
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_override_computed_result(string remitted_amount)
        {
            if (System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"] != null)
            {
                irsCalculator icalc = (irsCalculator)System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"];

                if (icalc != null)
                {                    
                    icalc.compute_irsw_charges(Convert.ToDecimal(remitted_amount));
                    System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"] = icalc;
                    return icalc.get_transaction_charge_screen();
                }
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_charge_computed_result(string product_charge_id, string account_currency, string charge_locally, string remitted_amount)
        {

            System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"] = null;

            bool charge_local = false;

            if (charge_locally == "1" || charge_locally.ToUpper() == "TRUE")
            {
                charge_local = true;
            }

            irsCalculator icalc = new irsCalculator(Convert.ToInt32(product_charge_id), Convert.ToInt32(account_currency), Convert.ToBoolean(charge_local));

            if (icalc != null)
            {
                icalc.compute_irsw_charges(Convert.ToDecimal(remitted_amount));

                System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS_OBJECT"] = icalc;

                return icalc.get_transaction_charge_screen();
            }

            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_rem_screen_information(string remitters_id, string beneficiary_id)
        {
            SqlCommand sqcommands = base.getSqlCommand("sp_txn_rem_ben_info");

            sqcommands.Parameters.Add("@rem_id", SqlDbType.VarChar).Value = remitters_id;
            sqcommands.Parameters.Add("@ben_id", SqlDbType.VarChar).Value = beneficiary_id;

            DataSet ds = base.getDataSet(sqcommands, "CLIENT_INFO_BASIC");

            if (ds != null)
            {
                return ds;
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_customer_basic_info(string cifid, string account_seq_id)
        {
            if (account_seq_id != null)
            {
                if (account_seq_id == string.Empty || account_seq_id.Length == 0 || account_seq_id.ToUpper() == "NULL")
                {
                    account_seq_id = null;
                }
            }

            SqlCommand sqcommands = base.getSqlCommand("sp_txn_customer_basic_info");

            sqcommands.Parameters.Add("@cifid", SqlDbType.VarChar).Value = cifid;
            sqcommands.Parameters.Add("@account_seq_id", SqlDbType.VarChar).Value = account_seq_id;

            DataSet ds = base.getDataSet(sqcommands, "CLIENT_INFO_BASIC");

            if (ds != null)
            {
                return ds;
            }

            return null;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_customers_class_source()
        {
            SqlCommand sqcommands = null;

            try
            {
                sqcommands = base.getSqlCommand("SELECT FldValue, UPPER(ISNULL(FldDescription,'')) AS FldDescription FROM v_tbl_lookup_per_branch WHERE UPPER(FldGroup) = 'CIFCLASS' AND FldValue IN (1,2)", false);

                return base.getDataSet(sqcommands, "CIF_LOOKUP_CLASS");
            }
            catch
            {
                return null;
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_customers_type_source()
        {
            SqlCommand sqcommands = null;

            try
            {
                sqcommands = base.getSqlCommand("SELECT FldValue, UPPER(ISNULL(FldDescription,'')) AS FldDescription FROM v_tbl_lookup_per_branch WHERE UPPER(FldGroup) = 'CIFType'", false); return base.getDataSet(sqcommands, "CIF_LOOKUP_TYPE");
            }
            catch
            {
                return null;
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_sub_look_up_data_perbranch(string field_group,string branchcode)
        {
            SqlCommand sqcommands = null;
            try
            {
               // sqcommands = base.getSqlCommand("SELECT * FROM V_TBL_LOOKUP_PER_BRANCH WHERE FldGroup = '" + field_group + "'", false); return base.getDataSet(sqcommands, "SUB_LOOKUP_TYPE");

                 sqcommands = base.getSqlCommand("isp_getSubLookup");

                sqcommands.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = branchcode ;
                sqcommands.Parameters.Add("@FldGroup", SqlDbType.VarChar).Value = field_group;
                DataSet ds = base.getDataSet(sqcommands, "SUB_LOOKUP");

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
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_sub_look_up_data(string field_group)
        {
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM V_TBL_LOOKUP_PER_BRANCH WHERE FldGroup = '" + field_group + "'", false); return base.getDataSet(sqcommands, "SUB_LOOKUP_TYPE");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_sub_look_up_data_generic(string field_group)
        {
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM TBL_LOOKUP WHERE FldGroup = '" + field_group + "'", false); return base.getDataSet(sqcommands, "LOOKUP_TYPE");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cif_account_data(int account_seq_id)
        {
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM V_IRSWORLD_CIF_ACCOUNTS WHERE account_seq_id = " + account_seq_id, false); 
                return base.getDataSet(sqcommands, "REMITTANCE_TABLE");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_tbl_lookup_description(string field_group, string field_value)
        {
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM TBL_LOOKUP WHERE FldGroup = '" + field_group + "' AND FldValue = " + field_value, false); 
                
                DataTable dt =  base.getDataSet(sqcommands, "LOOKUP_DATA").Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToString(dt.Rows[0]["FldDescription"]).ToUpper();
                }

                return string.Empty;
                
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remittance_record(int transaction_seq_id)
        {
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + transaction_seq_id, false); return base.getDataSet(sqcommands, "REMITTANCE_TABLE");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remittance_product_charges(int product_charge_id)
        {
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM v_tbl_charges_setup WHERE ProductChargesID = " + product_charge_id, false);
                return base.getDataSet(sqcommands, "REMITTANCE_CHARGES");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_logged_branch_code()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]; return loggin_data.Location;
            }
            return string.Empty;
        }
    }
}
