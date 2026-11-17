using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace IRSClassLib
{
    [Serializable]
    public class irsRemittanceBackend : sqlHelper
    {
        private decimal _remitting_amount           = 0;
        private decimal gross_rem_amount           = 0;
        private decimal net_rem_amount             = 0;
        private decimal net_to_bene_currency       = 0;
        private decimal net_to_bene_amount         = 0;
        private decimal total_local_charge         = 0;
        private decimal forex_remitting_to_account = 0;
        private decimal forex_remitting_to_local   = 0;
        private decimal forex_remitting_to_usd     = 0;
        private decimal forex_remitting_to_php     = 0;
        private decimal forex_remitting_to_eur     = 0;
        private decimal forex_local_to_remitting   = 0;
        private decimal forex_local_to_usd         = 0;
        private decimal forex_local_to_php         = 0;
        private decimal total_backend_charges      = 0;

        
        private int local_currency       = 0;
        private int _remit_method        = 0;
        private int _product_charge_id   = 0;
        private int _account_sequence_id = 0;
        private int _remitting_currency  = 0;
        private int _transaction_purpose = 0;
        private int _transaction_source  = 0;
        private int _source_of_fund      = 0;
        private int _receipt_mode        = 0;
        private int _account_currency    = 0;
        private int _transaction_status  = 0;
        private int _computation_type    = 0;

        private string _external_refno   = null;
        private string _remitter_id      = null;
        private string _beneficiary_id   = null;
        private string _special_message  = null;

        private string remitting_branch  = null;
        private string reference_number  = null;
        private string user_id = null;        

        private irsCalculator icalc = null;
        private DateTime transaction_date;
        private StringBuilder imsg = null;

        public int computation_type
        {
            set { this._computation_type = value; }
        }

        public decimal remitting_amount
        {
            set { this._remitting_amount = value; }
        }

        public int transaction_status
        {
            set { this._transaction_status = value; }
        }

        public int remit_method
        {
            set { this._remit_method = value; }
        }

        public int product_charge_id
        {
            set { this._product_charge_id = value; }
        }

        public int account_currency
        {
            set { this._account_currency = value; }
        }

        public int account_sequence_id
        {
            set { this._account_sequence_id = value; }
        }

        public int remitting_currency
        {
            set { this._remitting_currency = value; }
        }

        public int transaction_purpose
        {
            set { this._transaction_purpose = value; }
        }

        public int source_of_fund
        {
            set { this._source_of_fund = value; }
        }

        public string external_refno
        {
            set { this._external_refno = value; }
        }

        public int receipt_mode
        {
            set { this._receipt_mode = value; }
        }

        public string remitter_id
        {
            set { this._remitter_id = value; }
        }

        public string beneficiary_id
        {
            set { this._beneficiary_id = value; }
        }

        public string special_message
        {
            set { this._special_message = value; }
        }

        public irsRemittanceBackend()
        {
            this.icalc = null;
            this.imsg  = null;            
        }

        ~irsRemittanceBackend()
        {
            this.icalc = null;
            this.imsg  = null;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName      = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";

            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);
        }

        public string read_message()
        {
            return imsg.ToString();
        }

        private bool credit_online_transaction_ok(int transaction_id)
        {
            /* FUTURE CHECKING. INCLUDE GFC AGGREGATE */

            DataTable dt = null;

            bool return_value = true;

            string s = "SELECT " +
                             "A.referenceno, " +
                             "A.nettobenecurrency, " +
                             "A.nettobeneamount, " +
                             "B.accountno " +
                       "FROM TBL_REMITTANCE A " +
                       "INNER JOIN TBL_CIF_ACCOUNTS B ON B.SEQ_ID = A.accountseqid " +
                       "WHERE A.SEQ_ID = " + transaction_id.ToString();


            dt = getDataSet(getSqlCommand(s, false), "TRANSACTIONS").Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                /*
                bool ibool = new irsLinkToGFC().insert_GFC_Aggregate(Convert.ToString(dt.Rows[0]["referenceno"]).Replace("'", "''"),
                                                                     Convert.ToString(dt.Rows[0]["accountno"]).Replace("'", "''"),
                                                                     Convert.ToInt32(dt.Rows[0]["nettobenecurrency"]),
                                                                     Convert.ToDouble(dt.Rows[0]["nettobeneamount"]));
                */
            }

            dt = new irsRemTeller().Remit2(transaction_id).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Message"] != null && Convert.ToString(dt.Rows[0]["Message"]).Length > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["TranStatusDesc"]).ToUpper().IndexOf("ERROR") > -1)
                    {
                        this.imsg = new StringBuilder();
                        this.imsg.Append("16~" + Convert.ToString(dt.Rows[0]["Message"]));

                        return_value = false;
                    }
                }
            }
            else
            {
                this.imsg = new StringBuilder();
                this.imsg.Append("UNABLE TO CREDIT ONLINE. CONNECTION ERROR.");

                return_value = false;
            }

            return return_value;
        }

        public string insert_record()
        {
            bool ibool = false;

            this.set_charge_setup_content();
            this.compute_charges();

            base.Message = null;

            if (base.newRow() == true)
            {
                ibool = this.set_column_content_and_commit();
            }
            else
            {
                this.imsg = new StringBuilder();
                this.imsg.Append(base.Message);
            }

            if (ibool == false)
            {
                if (this.imsg != null)
                {
                    if (this.imsg.Length > 0)
                    {
                        return string.Empty + "~" + this.imsg.ToString();
                    }
                }
            }

            return this.reference_number + "~" + string.Empty;
        }

        private bool set_column_content_and_commit()
        {
            ArrayList error_list = new ArrayList();

            if (!base.setColumn("UserId",             this.user_id))             { error_list.Add("UserId :" + this.Message); }
            if (!base.setColumn("RemitMethod",        this._remit_method))       { error_list.Add("RemitMethod :" + this.Message); }
            if (!base.setColumn("LocalCurrency",      this.local_currency))      { error_list.Add("LocalCurrency :" + this.Message); }
            if (!base.setColumn("TranStatus",         this._transaction_status)) { error_list.Add("TranStatus :" + this.Message); }
            if (!base.setColumn("RemittingBranch",    this.remitting_branch))    { error_list.Add("RemittingBranch :" + this.Message); }
            if (!base.setColumn("TranxDate",          this.transaction_date))    { error_list.Add("TranxDate :" + this.Message); }

            if (!base.setColumn("TransactionSource",  this._transaction_source))  { error_list.Add("TransactionSource :"  + this.Message);}
            if (!base.setColumn("ReceiptMode",        this._receipt_mode))        { error_list.Add("ReceiptMode :"        + this.Message);}
            if (!base.setColumn("SourceOfFunds",      this._source_of_fund))      { error_list.Add("SourceOfFunds :"      + this.Message);}
            if (!base.setColumn("TransactionPurpose", this._transaction_purpose)) { error_list.Add("TransactionPurpose :" + this.Message);}
            if (!base.setColumn("SpecialMessage",     this._special_message))     { error_list.Add("SpecialMessage :"     + this.Message);}            
            
            if (!base.setColumn("ExtlReferenceNo",   this._external_refno))      { error_list.Add("ExtlReferenceNo :"   + this.Message);}            
            if (!base.setColumn("ProductChargesID",  this._product_charge_id))   { error_list.Add("ProductChargesID :"  + this.Message);}
            if (!base.setColumn("RemitterCIFID",     this._remitter_id))         { error_list.Add("RemitterCIFID :"     + this.Message);}
            if (!base.setColumn("BeneficiaryCIFID",  this._beneficiary_id))      { error_list.Add("BeneficiaryCIFID :"  + this.Message);}
            if (!base.setColumn("AccountSEQID",      this._account_sequence_id)) { error_list.Add("AccountSEQID :"      + this.Message);}
            if (!base.setColumn("RemittingCurrency", this._remitting_currency))  { error_list.Add("RemittingCurrency :" + this.Message);}            

            if (!base.setColumn("GrossRemAmount",          this.gross_rem_amount))           { error_list.Add("GrossRemAmount :"          + this.Message); }
            if (!base.setColumn("NetRemAmount",            this.net_rem_amount))             { error_list.Add("NetRemAmount :"            + this.Message); }
            if (!base.setColumn("NetToBeneCurrency",       this.net_to_bene_currency))       { error_list.Add("NetToBeneCurrency :"       + this.Message); }
            if (!base.setColumn("NetToBeneAmount",         this.net_to_bene_amount))         { error_list.Add("NetToBeneAmount :"         + this.Message); }
            if (!base.setColumn("TotalLocalCharge",        this.total_local_charge))         { error_list.Add("TotalLocalCharge :"        + this.Message); }
            if (!base.setColumn("ForexRemittingToAccount", this.forex_remitting_to_account)) { error_list.Add("ForexRemittingToAccount :" + this.Message); }
            if (!base.setColumn("ForexRemittingToLocal",   this.forex_remitting_to_local))   { error_list.Add("ForexRemittingToLocal :"   + this.Message); }
            if (!base.setColumn("ForexRemittingToUSD",     this.forex_remitting_to_usd))     { error_list.Add("ForexRemittingToUSD :"     + this.Message); }
            if (!base.setColumn("ForexRemittingToPHP",     this.forex_remitting_to_php))     { error_list.Add("ForexRemittingToPHP :"     + this.Message); }
            if (!base.setColumn("ForexRemittingToEUR",     this.forex_remitting_to_eur))     { error_list.Add("ForexRemittingToEUR :"     + this.Message); }
            if (!base.setColumn("ForexLocalToRemitting",   this.forex_local_to_remitting))   { error_list.Add("ForexLocalToRemitting :"   + this.Message); }
            if (!base.setColumn("ForexLocalToUSD",         this.forex_local_to_usd))         { error_list.Add("ForexLocalToUSD :"         + this.Message); }
            if (!base.setColumn("ForexLocalToPHP",         this.forex_local_to_php))         { error_list.Add("ForexLocalToPHP :"         + this.Message); }
            if (!base.setColumn("total_backend_charges",   this.total_backend_charges))      { error_list.Add("total_backend_charges :"   + this.Message); }

            if (error_list.Count > 0)
            {
                this.imsg = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    this.imsg.Append((String)errors + "\n");
                }

                return false;
            }

            if (base.save() == false)
            {
                this.imsg = new StringBuilder();
                this.imsg.Append(base.Message);

                return false;
            }

            int transaction_id = 0;

            if (base.IdentityValue != null)
            {
                transaction_id = Convert.ToInt32(base.IdentityValue);
            }

            this.reference_number = this.get_reference_no();
            this.icalc.commit_remittance_charge_details(transaction_id);
            
            return this.credit_online_transaction_ok(transaction_id);
        }

        private string get_reference_no()
        {
            base.EnforceConstraint = false;

            DataTable dt =  base.getDataSet(base.getSqlCommand("SELECT ReferenceNo FROM TBL_REMITTANCE WHERE SEQ_ID = " + Convert.ToInt32(base.IdentityValue).ToString(), false), "TBL_REFERENCE_NO").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToString(dt.Rows[0]["ReferenceNo"]);
                }
            }

            return string.Empty;
        }

        private void set_charge_setup_content()
        {
            base.EnforceConstraint = false;

            DataTable dt = base.getDataSet(base.getSqlCommand("SELECT * FROM TBL_CHARGE_MASTER WHERE SEQ_ID = " + this._product_charge_id.ToString(), false), "TBL_CHARGE_SETUP").Tables[0];

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    this._transaction_status = Convert.ToInt32(dt.Rows[0]["DefaultStatus"]);
                    this._remitting_currency = Convert.ToInt32(dt.Rows[0]["Currency"]);
                }
            }
        }

        private void compute_charges()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                this.remitting_branch = loggin_data.Location;
                this.user_id          = loggin_data.UserId;
                this.transaction_date = new irsTimeZone().branchLocalTime(loggin_data.Location);
            }

            this.icalc = new irsCalculator(this._product_charge_id, this._account_currency, false, this._computation_type);
            this.icalc.compute_charges(this._remitting_amount, false);

            this.gross_rem_amount           = icalc.get_remittance_gross_branch();
            this.net_rem_amount             = icalc.get_remittance_net_remit_amt();
            this.net_to_bene_amount         = icalc.get_remittance_net_to_bene_amt();
            this.net_to_bene_currency       = icalc.get_beneficiary_account_currency();
            this.total_local_charge         = icalc.get_remittance_total_charges_amt();
            this.local_currency             = icalc.get_branch_local_currency();
            this.forex_remitting_to_account = icalc.get_forex_target_to_account();
            this.forex_remitting_to_local   = icalc.get_forex_target_to_source();
            this.forex_remitting_to_usd     = icalc.get_forex_target_to_usd();
            this.forex_remitting_to_php     = icalc.get_forex_target_to_php();
            this.forex_remitting_to_eur     = icalc.get_forex_target_to_eur();
            this.forex_local_to_remitting   = icalc.get_forex_source_to_target();
            this.forex_local_to_usd         = icalc.get_forex_source_to_usd();
            this.forex_local_to_php         = icalc.get_forex_source_to_php();
            this.total_backend_charges      = icalc.get_total_backend_charges();
        }
    }   
}
