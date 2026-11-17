using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.TranClass
{    
    /* REMITTANCE MAIN TABLE COMMIT CLASS */

    [Serializable()]
    public class commit_beneficiary_zipcode : sqlHelperWs
    {        
        public commit_beneficiary_zipcode()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_ADDRESS";
            this.IdentityColumn = "SEQ_ID";

            //commented by boyet to use faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_ADDRESS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_ADDRESS WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_zipcode(int preferred_add_seq_id, int zipcode_seq)
        {
            int i = this.beneficiary_address_seq_id(preferred_add_seq_id);

            if (i > 0)
            {
                base.selectedRow = 0;

                if (base.edit(i) == true)
                {
                    if (base.setColumn("ZipCodeSeqId", zipcode_seq) == true)
                    {
                        if (base.save() == false)
                        {
                            base.cancel();
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private int beneficiary_address_seq_id(int preferred_add_seq_id)
        {
            DataTable dt = null;

            string s = "SELECT * FROM TBL_CIF_ADDRESS WHERE SEQ_ID = " + preferred_add_seq_id.ToString();

            try
            {
                dt = getDataSet(getSqlCommand(s, false), "TBL_REMITTER_COUNT").Tables[0];

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
    }

    [Serializable()]
    public class remit_remitter_charge_type : sqlHelperWs
    {
        public remit_remitter_charge_type()
        {
            base.EnforceConstraint = false;
        }

        public void commit_remitter_charge_type(string remitter_id, int charge_type)
        {
            int i = this.check_if_remitter_exist(remitter_id);

            if (i > 0)
            {
                base.selectedRow = 0;

                if (base.edit(i) == true)
                {
                    if (base.setColumn("ChargeType", charge_type) == true)
                    {
                        if (base.save() == false)
                        {
                            base.cancel();
                        }
                    }
                }
            }
            else
            {
                if (base.newRow() == true)
                {
                    if (base.setColumn("ChargeType", charge_type) == true)
                    {
                        if (base.setColumn("CIFID", remitter_id) == true)
                        {
                            if (base.save() == false)
                            {
                                base.cancel();
                            }
                        }
                    }
                }
            }
        }

        public override void getUpdateDS(int id)
        {
            this.TableName      = "TBL_CIF_REMITTERS";
            this.IdentityColumn = "SEQ_ID";

            //commented by boyet for faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_REMITTERS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_REMITTERS WHERE SEQ_ID = " + id.ToString(), false);
        }

        private int check_if_remitter_exist(string remitter_id)
        {
            DataTable dt = null;
            
            string s = "SELECT * FROM TBL_CIF_REMITTERS WHERE CIFID = '" + remitter_id + "'";

            try
            {
                dt = getDataSet(getSqlCommand(s, false), "TBL_REMITTER_COUNT").Tables[0];

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
    }

    [Serializable()]
    public class remit_remitter_source_of_fund : sqlHelperWs
    {
        public remit_remitter_source_of_fund()
        {
            base.EnforceConstraint = false;
        }
        
        public void commit_remitter_source_of_fund(string remitter_id, int source_of_fund)
        {
            int i = this.check_if_remitter_exist(remitter_id);

            if (i > 0)
            {
                base.selectedRow = 0;

                if (base.edit(i) == true)
                {
                    if (base.setColumn("sourceoffunds", source_of_fund) == true)
                    {
                        if (base.save() == false)
                        {
                            base.cancel();
                        }
                    }
                }
                else
                {
                    if (base.newRow() == true)
                    {
                        if (base.setColumn("ChargeType", source_of_fund) == true)
                        {
                            if (base.setColumn("CIFID", remitter_id) == true)
                            {
                                if (base.save() == false)
                                {
                                    base.cancel();
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void getUpdateDS(int id)
        {
            this.TableName      = "TBL_CIF_OCCUPATIONS";
            this.IdentityColumn = "SEQ_ID";

            //SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_OCCUPATIONS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_OCCUPATIONS WHERE SEQ_ID = " + id.ToString(), false);
        }

        private int check_if_remitter_exist(string remitter_id)
        {
            DataTable dt = null;
            
            string s = "SELECT * FROM TBL_CIF_OCCUPATIONS WHERE ISNULL(deleted, 0) = 0 AND CIFID = '" + remitter_id + "'";

            try
            {
                dt = getDataSet(getSqlCommand(s, false), "TBL_REMITTER_COUNT").Tables[0];

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
    }

    [Serializable()]
    public class remit_benef_product : sqlHelperWs
    {
        private string branch_code = null;

        public remit_benef_product()
        {
            base.EnforceConstraint = false;
        }        

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string commit_beneficiary_product_setup(string remitter_id, string beneficiary_id,  int product_code, int implementation_type, int account_sequence, int source_of_fund, int currency_code, int account_type, int branch_sequence_id, int charge_type)
        {
            string return_value = string.Empty;

            ArrayList error_list = new ArrayList();
            StringBuilder sb = new StringBuilder();

            try
            {
                if (branch_sequence_id > 0)
                {
                    this.branch_code = this.get_branch_code_equiv(branch_sequence_id);
                }

                if (this.check_setup_exists(remitter_id, beneficiary_id, product_code, implementation_type, account_sequence, currency_code, account_type, branch_sequence_id) == false)
                {
                    if (base.newRow() == true)
                    {
                        if (!base.setColumn("cifid",              beneficiary_id))      {error_list.Add("cifid:"              + this.Message);};
                        if (!base.setColumn("remittercifid",      remitter_id))         {error_list.Add("remittercifid:"      + this.Message);};
                        if (!base.setColumn("productcode",        product_code))        {error_list.Add("productcode:"        + this.Message);};
                        if (!base.setColumn("implementationcode", implementation_type)) {error_list.Add("implementationcode:" + this.Message);};
                        if (!base.setColumn("deleted", 0)) { error_list.Add("deleted:" + this.Message); };

                        if (this.check_if_needs_account(product_code, implementation_type) == true)
                        {
                            if (!base.setColumn("accountseqid", account_sequence)) { error_list.Add("accountseqid:" + this.Message); };
                        }
                        else
                        {
                            if (currency_code > 0)
                            {
                                if (!base.setColumn("currency", currency_code))
                                {
                                    error_list.Add("currency:" + this.Message);
                                }
                            }

                            if (branch_sequence_id > 0)
                            {
                                if (!base.setColumn("BranchCode", this.branch_code))
                                {
                                    error_list.Add("BranchCode:" + this.Message);
                                }
                            }

                            if (account_type > 0)
                            {
                                if (!base.setColumn("AccountType", account_type))
                                {
                                    error_list.Add("AccountType:" + this.Message);
                                }
                            }
                        }
                    }

                    if (base.save() == true)
                    {
                        sb.Append("0~");
                    }
                    else
                    {
                        sb.Append(base.Message);
                        sb.Append("\n");
                    }

                    if ((base.Message != null && base.Message.Length > 0) || error_list.Count > 0)
                    {
                        if (error_list.Count > 0)
                        {
                            foreach (Object errors in error_list)
                            {
                                sb.Append((String)errors + "\n");
                            }
                        }

                        base.cancel();
                    }
                }
                else
                {
                    sb.Append("EXISTING BENEFICIARY SETUP.\n" );
                }

                if (source_of_fund > 0)
                {
                    new remit_remitter_source_of_fund().commit_remitter_source_of_fund(remitter_id, source_of_fund);
                }

                if (charge_type > 0)
                {
                    new remit_remitter_charge_type().commit_remitter_charge_type(remitter_id, charge_type);
                }

            }
            catch (Exception e)
            {
                sb.Append(e.Message);
            }

            return sb.ToString();

        }

        private bool check_setup_exists(string remitter_id, string beneficiary_id, int product_code, int implementation_type, int account_sequence, int currency_code, int account_type, int branch_sequence_id)
        {
            DataTable dt = null;

            string s = string.Empty;

            s = "SELECT COUNT(*) AS count_setup " +
                "FROM TBL_CIF_BENEFICIARIES " +
                "WHERE " +
                        "ISNULL(cifid, '')             = '" + beneficiary_id.Replace("'", "''") + "' AND " +
                        "ISNULL(remittercifid, '')     = '" + remitter_id.Replace("'", "''")    + "' AND " +
                        "ISNULL(productcode, 0)        = " + product_code.ToString()            + "  AND " +
                        "ISNULL(implementationcode, 0) = " + implementation_type.ToString()     + "  AND " +
                        "ISNULL(deleted, 0) = 0 ";

            if (this.check_if_needs_account(product_code, implementation_type) == true)
            {
                s += " AND ISNULL(accountseqid, 0) = " + account_sequence.ToString();
            }
            else
            {
                if (currency_code > 0)
                {
                    s += " AND ISNULL(currency, 0) = " + currency_code.ToString();
                }

                if (account_type > 0)
                {
                    s += " AND ISNULL(AccountType, 0) = " + account_type.ToString();
                }

                if (branch_sequence_id > 0)
                {
                    s += " AND ISNULL(BranchCode, '') = '" + this.branch_code + "'";
                }
            }

            try
            {
                dt = getDataSet(getSqlCommand(s, false), "COUNT_SETUP_ITEM").Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["count_setup"]) == 0)
                    {
                        return false;
                    }
                }

                return true;
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

        private string get_branch_code_equiv(int branch_seq_id)
        {
            return (string)getDataSet(getSqlCommand("SELECT BranchCode FROM TBL_BRANCHES WHERE SEQ_ID = " + branch_seq_id.ToString(), false)).Tables[0].Rows[0]["BranchCode"];
        }

        private bool check_if_needs_account(int product_code, int implementation_code)
        {
            if (product_code == 10) { if (implementation_code == 2) { return true; } }
            if (product_code == 22) { if (implementation_code == 2) { return true; } }
            if (product_code == 11) { if (implementation_code == 11){ return true; } }
            if (product_code == 11) { if (implementation_code == 1) { return true; } }
            if (product_code == 14) { if (implementation_code == 2) { return true; } }
            if (product_code == 16) { if (implementation_code == 10){ return true; } }
            if (product_code == 13) { if (implementation_code == 5) { return true; } }
            if (product_code == 12) { if (implementation_code == 4) { return true; } }
            if (product_code == 12) { if (implementation_code == 5) { return true; } }
            if (product_code ==  2) { if (implementation_code == 1) { return true; } }
            if (product_code ==  5) { if (implementation_code == 1) { return true; } }
            if (product_code ==  6) { if (implementation_code == 1) { return true; } }
            if (product_code == 258) { if (implementation_code == 29) { return true; } }  /* INSTAPAY ALYSSA SEPTEMBER 2019*/
            return false;
        }      

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_CIF_BENEFICIARIES";
            this.IdentityColumn = "SEQ_ID";

            // commented by boyet to use faster constructor SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_BENEFICIARIES WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_CIF_BENEFICIARIES WHERE SEQ_ID = " + id.ToString(), false);
        }    
    }

    [Serializable()]
    public class RemTranSaving: sqlHelperWs
    {
        private bool edit_mode        = false;
        private string seq_id         = null;
        private string error_message  = null;
        private int product_charge_id = 0;
        private DateTime remittance_transaction_date;
        private csAuditLog logger = new csAuditLog();

        private string UserID = string.Empty;
        private int RemitMethod = 4;
        private string RemittingBranch = string.Empty;
        private string TranxDate = string.Empty;
        private string RemitterCIFID = string.Empty;
        private string BeneficiaryCIFID = string.Empty;
        private int ProductChargesID = 0;
        private int RemittingCurrency = 0;
        private int NettoBeneCurrency = 0;
        private decimal GrossRemAmount = 0;
        private decimal NetRemAmount = 0;
        private int TransactionSource = 0;
        private int ReceiptMode = 0;
        private int TransactionPurpose = 0;
        private int SourceofFunds = 0;

        private void set_chk_dup_value(string[] s)
        {
            if (string.IsNullOrEmpty(s[0]).Equals(true) && string.IsNullOrEmpty(s[1]).Equals(true))
            {
                return;
            }
            
            switch (s[0].Trim().ToUpper())
            {
                case "REMITTERCIFID": this.RemitterCIFID = s[1].Trim(); break;
                case "BENEFICIARYCIFID": this.BeneficiaryCIFID = s[1].Trim(); break;
                case "PRODUCTCHARGESID": this.ProductChargesID = Convert.ToInt32(s[1].Trim()); break;
                case "REMITTINGCURRENCY": this.RemittingCurrency = Convert.ToInt32(s[1].Trim()); break;
                case "NETTOBENECURRENCY": this.NettoBeneCurrency = Convert.ToInt32(s[1].Trim()); break;
                case "GROSSREMAMOUNT": this.GrossRemAmount = Convert.ToDecimal(s[1].Trim()); break;
                case "NETREMAMOUNT": this.NetRemAmount = Convert.ToDecimal(s[1].Trim()); break;
                case "TRANSACTIONSOURCE": this.TransactionSource = Convert.ToInt32(s[1].Trim()); break;
                case "RECEIPTMODE": this.ReceiptMode = Convert.ToInt32(s[1].Trim()); break;
                case "TRANSACTIONPURPOSE": this.TransactionPurpose = Convert.ToInt32(s[1].Trim()); break;
                case "SOURCEOFFUNDS": this.SourceofFunds = Convert.ToInt32(s[1].Trim()); break;
            }
        }

        private bool chk_dup_transaction()
        {
            irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            bool dup_run = false;

            if (System.Web.HttpContext.Current.Session["DUP_TRAN_RESUME"] != null)
            {
                dup_run = (bool)System.Web.HttpContext.Current.Session["DUP_TRAN_RESUME"];

                if (dup_run == true)
                {
                    this.logs("", "RemTranSaving.chk_dup_transaction", "Inserting duplicate transaction triggered by user: " + loggin_data.UserId, (string)System.Web.HttpContext.Current.Session["TXN_COLUMN_VAL"], ""); return false;
                }
            }

            if (loggin_data != null)
            {
                this.TranxDate = new irsInternetPage().getTransactionDate(loggin_data.Location, true).ToString("MM/dd/yyyy");
                this.UserID = loggin_data.UserId;
                this.RemittingBranch = loggin_data.Location;
            }

            DataSet ds = getDataSet("otc_chk_dup", true, new object[]{
                                        this.UserID,
                                        this.RemitMethod,
                                        this.RemittingBranch,
                                        this.TranxDate,
                                        this.RemitterCIFID,
                                        this.BeneficiaryCIFID,
                                        this.ProductChargesID,
                                        this.RemittingCurrency,
                                        this.NettoBeneCurrency,
                                        this.GrossRemAmount,
                                        this.NetRemAmount,
                                        this.TransactionSource,
                                        this.ReceiptMode,
                                        this.TransactionPurpose,
                                        this.SourceofFunds});

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["dup_count"]) > 0)
                {
                    string dup_reference = string.Empty;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dup_reference += ds.Tables[0].Rows[i]["referenceno"].ToString();
                        dup_reference += " - ";
                        dup_reference += ds.Tables[0].Rows[i]["flddescription"].ToString();
                        dup_reference += "\n";
                    }

                    System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = "Duplicate transaction found! \n\nPlease review the following:\n\n" + dup_reference; return true;
                }
            }

            return false;
        }

        private void commit_non_online_status(int transaction_id)
        {
            if (transaction_id > 0)
            {
                DataTable dt = null;

                try
                {
                    dt = getDataSet(getSqlCommand("SELECT PRODUCTCODE, IMPLEMENTATIONTYPE, DEFAULTSTATUS FROM TBL_CHARGE_MASTER WHERE SEQ_ID = " + this.product_charge_id.ToString(), false), "T_REMITTANCE").Tables[0];

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            int product_code   = 0;
                            int implem_code    = 0;
                            int default_status = 0;

                            if (dt.Rows[0]["PRODUCTCODE"] != null)
                            {
                                product_code = Convert.ToInt32(dt.Rows[0]["PRODUCTCODE"]);
                            }

                            if (dt.Rows[0]["IMPLEMENTATIONTYPE"] != null)
                            {
                                implem_code = Convert.ToInt32(dt.Rows[0]["IMPLEMENTATIONTYPE"]);
                            }

                            if (dt.Rows[0]["DEFAULTSTATUS"] != null)
                            {
                                default_status = Convert.ToInt32(dt.Rows[0]["DEFAULTSTATUS"]);
                            }

                            if (implem_code == 2 || product_code == 10 || product_code == 24 || product_code == 27)
                            {
                                return; /* DO NOT REMOVE FROM THIS POSITION. DEFAULT STATUS CHECKING MUST FOLLOWED NEXT. IF DIRECT CREDIT TO PNB ACCOUNT DO NOT SEND TO NON ONLINE CHANGE STATUS. OTHER MODULE WILL TRIGGER ONLINE TRANSACTION */
                            }

                            if (default_status == 14 || default_status == 15)
                            {
                                return; /* DO NOT ALLOW CHANGING TRANSACTION STATUS OR SEND TO FLEXCUBE API IF TRANSACTION STATUS IS ON HOLD */
                            }

                            //remove this line to avoid the agent from doing auto credit/or tagging the transaction as posted central
                            //DataSet ds = new irsRemTeller().Remit2(transaction_id, true);
                            //end here
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
            }
        }
        private void logs(string userid, string module, string description, string newvalue, string oldvalue)
        {
            this.logger.writeAudit(userid,module,description,newvalue,oldvalue,csAuditLog.Category.Insert);
        }

        public RemTranSaving() 
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName      = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            this.error_message  = null;

            //commented by boyet to use faster constructor SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_remittance_sequence_id()
        {
            if (System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] != null)
            {
                return (int)System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"];
            }

            return 0;
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
        public bool insert_dup_record(string column_values)
        {
            System.Web.HttpContext.Current.Session["DUP_TRAN_RESUME"] = null;
            System.Web.HttpContext.Current.Session["DUP_TRAN_RESUME"] = true; 

            return this.insert_record(column_values);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record(string column_values)
        {
            System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;
            System.Web.HttpContext.Current.Session["TXN_COLUMN_VAL"] = column_values;

            irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            
            this.edit_mode = false;
            this.logs("", "RemTranSaving.insert_record", "Inserting to remittance table", column_values, "");
            
            bool ibool = false; string[] s = column_values.Split('|');

            if (base.newRow() == true)
            {
                ibool = this.set_record_and_commit(s, false);
            }

            if (ibool == false)
            {
                this.get_error_dbase();
                this.rollback_record();
                this.logs("", "RemTranSaving.insert_record", "Inserting to remittance table", "Error in set_record_and_commit returns false :" + base.Message, "");
            }
            else
            {
                this.commit_other_details(s[8].ToString());
                this.commit_non_online_status(Convert.ToInt32(base.IdentityValue));
                /*  
                    Added by    : Neil
                    Date        : Feb 2012
                 
                this.saveNewRemittanceTable("REMITTANCE_CALCULATOR_CLASS_REM_SAVING", Convert.ToInt32(this.get_array_value("ProductChargesID", s)), 
                                            (IRSClassLib.csRemittance.RemitMethod)Convert.ToInt32(this.get_array_value("RemitMethod", s)), 
                                            loggin_data.UserId, this.get_array_value("ExtlReferenceNo", s), this.remittance_transaction_date, 
                                            DateTime.Now,null, this.get_array_value("RemitterCIFID", s), Convert.ToInt32(this.get_array_value("RemitterAddressSeqID", s)), 
                                            Convert.ToInt32(this.get_array_value("RemitterContactSeqID", s)), this.get_array_value("BeneficiaryCIFID", s), 
                                            Convert.ToInt32(this.get_array_value("BeneficiaryAddressSeqID", s)), Convert.ToInt32(this.get_array_value("BeneficiaryContactSeqID", s)), 
                                            0, 0, false, 0, Convert.ToInt32(this.get_array_value("AccountSEQID", s)), null, 0, 0, 
                                            Convert.ToInt32(this.get_array_value("TransactionSource", s)), Convert.ToInt32(this.get_array_value("TransactionPurpose", s)), 
                                            this.get_array_value("OtherPurpose", s), Convert.ToInt32(this.get_array_value("ReceiptMode", s)), 
                                            Convert.ToInt32(this.get_array_value("SourceofFunds", s)), this.get_array_value("SpecialMessage", s),
                                            null, null, null, DateTime.Now, DateTime.Now, loggin_data.Location.ToString(),0);                                                                                        
                
            
            
                Added by    : Neil
                Date        : Feb 2012
             */
            }
            return ibool;
        }

        private String get_array_value(String column_values, string[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                string[] ss = s[i].Split('~');

                if (ss[0].Trim().ToUpper() == column_values.Trim().ToUpper())
                {
                    return (String)ss[1];
                }
            }
            return null;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_otc_transaction_status(string column_value, int transaction_id)
        {
            bool ibool = true;

            base.IdentityValue = transaction_id;
            base.selectedRow   = 0;
            base.Message       = null;

            this.logs("", "RemTranSaving.update_otc_transaction_status", "Update trxn status in remittance table", "Update trxn status in remittance table " + column_value + "seq_id=" + transaction_id.ToString(), "");

            string[] col_and_value = column_value.Split('|');

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                ArrayList error_list = new ArrayList();

                for (int i = 0; i < col_and_value.Length; i++)
                {
                    string[] s = col_and_value[i].Split('~');

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
                    error_list.Add(base.Message);
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

                    ibool = false;
                }
            }

            if (ibool == false)
            {
                this.get_error_dbase();
                this.rollback_record();
            }

            return ibool;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string column_values, int transaction_id)
        {
            bool ibool = false;            

            base.IdentityValue = transaction_id;
            base.selectedRow   = 0;

            string[] s = column_values.Split('|');

            this.edit_mode = true;
            this.seq_id = transaction_id.ToString();

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                ibool = this.set_record_and_commit(s,true);
            }

            if (ibool == false)
            {
                this.get_error_dbase();
                this.rollback_record();
            }

            return ibool;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet reload_remittance_record(int transaction_id)
        {
            this.seq_id = transaction_id.ToString(); 
            
            return base.getDataSet(getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + transaction_id.ToString(), false), "TBL_RECORD");
        }

        private bool commit_record()
        {
            System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = null;
            System.Web.HttpContext.Current.Session["TRAN_COLUMN_SESSION"] = null;
            System.Web.HttpContext.Current.Session["TXN_COLUMN_VAL"] = null;

            //actual checking of duplicate transaction added by boyet
            if (this.edit_mode == false && this.chk_dup_transaction() == true)
            {
                return false;
            }
            //end here

            if (base.save() == true)
            {
                /* VAPT 20180222 */
                try
                {
                    string log = string.Empty;
                    string sys = string.Empty;

                    if (System.Web.HttpContext.Current.Session["OTCLog"] != null)
                    {
                        log = System.Web.HttpContext.Current.Session["OTCLog"].ToString();
                    }
                    if (System.Web.HttpContext.Current.Session["OTCSys"] != null)
                    {
                        sys = System.Web.HttpContext.Current.Session["OTCSys"].ToString();
                    }
                    if (log != string.Empty || sys != string.Empty)
                    {
                        new csAuditLog().writeVAPTLogger("", "OTC", "CREATED AN OTC TRANSACTION", log, "", sys, csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                    }
                }
                catch { }

                System.Web.HttpContext.Current.Session["DUP_TRAN_RESUME"] = null; //RESET SESSION

                if (this.edit_mode == false) 
                {
                    this.seq_id = this.get_transaction_sequence_id(Convert.ToInt32(base.IdentityValue)).ToString(); 
                    
                    System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = Convert.ToInt32(this.seq_id);
                }
                else
                {
                    System.Web.HttpContext.Current.Session["NEW_TXN_SEQUENCE_ID"] = Convert.ToInt32(this.seq_id);
                }

                if (this.edit_mode == false)
                {
                    this.insert_transaction_log(Convert.ToInt32(this.seq_id));
                }

                this.commit_remittance_charge_details(Convert.ToInt32(this.seq_id)); return true;
            }

            return false;
        }

        private void insert_transaction_log(long transaction_id)
        {
            irsClientData loggin_data = null;
            long user_session_id = 0;
            string user_id = string.Empty;

            try
            {
                if (System.Web.HttpContext.Current.Session["SessionID"] == null)
                {
                    user_session_id = Math.Abs(System.Web.HttpContext.Current.Session.SessionID.GetHashCode());
                }
                else
                {
                    user_session_id =  Convert.ToInt64(System.Web.HttpContext.Current.Session["SessionID"]);
                }
            }
            catch
            {
                user_session_id = 0;
            }

            try
            {
                loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                if (loggin_data != null)
                {
                    user_id = loggin_data.UserId;
                    base.execStoredProcedureUpdate("insert_remittance_auditlogger", new object[] { transaction_id, user_session_id, user_id });
                }
            }
            catch
            {
            }
            finally
            {
                loggin_data = null;
            }
        }

        private void commit_remittance_charge_details(int transaction_id)
        {
            irsCalculator icalculate = null;

            if (System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS"] != null)
            {
                if (this.edit_mode == false)
                {
                    icalculate = (irsCalculator)System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS"];
                    icalculate.commit_remittance_charge_details(transaction_id);
                }
            }

            try
            {
                bool ibool = new irsOverrideLog("", "OTC").LogOverride(transaction_id);

                /* ADDED BY BOYET MARCH 14 2014*/

                this.log_hitlist(transaction_id);

                /* ADDED BY BOYET MARCH 14 2014*/

            }
            catch
            {
            }
            finally
            {
                icalculate = null;

                System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"] = null;
                System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;
                System.Web.HttpContext.Current.Session["REMITTANCE_CALCULATOR_CLASS"] = null;
            }
        }

        //=======================================================================================
        //RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
        //private void log_hitlist(int transaction_id)
        public void log_hitlist(int transaction_id)
        //=======================================================================================
        {
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsRefinitiv = new irsRefinitiv();
            string cifid = "";
            //System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;


            if (System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"] == null || (string)System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"] == string.Empty)
            {
                return;
            }

            DataSet ds = new sqlHelperWs().getDataSet("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + transaction_id, false, new object[] { });

            string sanction_list_ben_id = null;
            string sanction_list_rem_id = null;

            string watchlist_ben = null;
            string watchlist_rem = null;

            string beneficiary_id = null;
            string remitter_id = null;
            string override_refinitivisdown = null;

            //=======================================================================================
            //RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
            int remit_method = 0;
            //=======================================================================================

            try
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    beneficiary_id = ds.Tables[0].Rows[0]["BeneficiaryCIFID"].ToString();
                    remitter_id = ds.Tables[0].Rows[0]["RemitterCIFID"].ToString();

                    //=======================================================================================
                    //RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
                    remit_method = Convert.ToInt32(ds.Tables[0].Rows[0]["RemitMethod"].ToString());
                    //=======================================================================================

                    sanction_list_ben_id = null;
                    sanction_list_rem_id = null;

                    watchlist_ben = null;
                    watchlist_rem = null;

                    /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API */
                    string isRefinitivEnabled = refinitivcs.isRefinitivEnabled().ToString();
                    if (isRefinitivEnabled == "1")
                    {
                        DataTable dt_refinitiv = new DataTable();
                        DataTable dtCaseSytemID = new DataTable();
                        dt_refinitiv = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];

                        if (System.Web.HttpContext.Current.Session["REFINITIV_IS_DOWN"] == "Refinitiv is down.")
                        {
                            string[] override_user = null;
                            override_user = ((string)System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"]).Split('|');
                            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|50" + "~";
                            sanction_list_ben_id = beneficiary_id;
                            sanction_list_rem_id = remitter_id;
                        }
                        else
                        {

                            if (dt_refinitiv != null && dt_refinitiv.Rows.Count > 0)
                            {
                                DataView view = new DataView(dt_refinitiv);
                                DataTable distinctTable = view.ToTable(true, "CaseSystemID");
                                foreach (DataRow row in distinctTable.Rows)
                                {
                                    int cnt_hit = 0;
                                    dtCaseSytemID = irsRefinitiv.getRefinitivByCaseSystemID(row[0].ToString());
                                    if (dtCaseSytemID != null && dtCaseSytemID.Rows.Count > 0)
                                    {
                                        cifid = dtCaseSytemID.Rows[0]["CIFID"].ToString();
                                        string[] override_user = null;
                                        override_user = ((string)System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"]).Split('|');

                                        string hit_cifid = dtCaseSytemID.Rows[0]["CIFID"].ToString();
                                        string hit_type = dtCaseSytemID.Rows[0]["ProviderType"].ToString();

                                        if (beneficiary_id.Trim().ToUpper() == hit_cifid.Trim().ToUpper())
                                        {
                                            sanction_list_ben_id = hit_cifid;

                                            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|50" + "~";
                                            watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                        }

                                        if (remitter_id.Trim().ToUpper() == hit_cifid.Trim().ToUpper())
                                        {
                                            sanction_list_rem_id = hit_cifid;

                                            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|50" + "~";
                                            watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                        }
                                    }
                                    cnt_hit++;
                                }
                            }

                            //DataSet ds2 = new sqlHelperWs().getDataSet("sp_otc_hit_listing_refinitiv_log_hitlist", true, new object[] { remitter_id, beneficiary_id }, new string[] { });

                            //if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                            //{
                            //    // CJ Reset Value
                            //    string[] override_user = null;
                            //    override_user = ((string)System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"]).Split('|');
                            //    // CJ End
                            //    int cnt_hit = 0;

                            //    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                            //    {
                            //        string hit_cifid = ds2.Tables[0].Rows[i]["CIFID"].ToString();
                            //        string hit_type = ds2.Tables[0].Rows[i]["FldDescription"].ToString();

                            //        if (cnt_hit > 15)
                            //        {
                            //            break;
                            //        }

                            //        if (beneficiary_id.Trim().ToUpper() == hit_cifid.Trim().ToUpper())
                            //        {
                            //            sanction_list_ben_id = hit_cifid;

                            //            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|50" + "~";
                            //            watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                            //        }

                            //        if (remitter_id.Trim().ToUpper() == hit_cifid.Trim().ToUpper())
                            //        {
                            //            sanction_list_rem_id = hit_cifid;

                            //            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|50" + "~";
                            //            watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                            //        }

                            //        cnt_hit++;
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        DataSet ds2 = new sqlHelperWs().getDataSet("sp_otc_hit_listing", true, new object[] { remitter_id, beneficiary_id }, new string[] { });

                        if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                        {
                            // CJ Reset Value
                            string[] override_user = null;
                            override_user = ((string)System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"]).Split('|');
                            // CJ End
                            int cnt_hit = 0;

                            for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                            {
                                //=========================================================================================
                                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                                //string hit_cifid = ds2.Tables[0].Rows[i]["CIFID"].ToString();
                                //string hit_type = ds2.Tables[0].Rows[i]["FldDescription"].ToString();
                                string hit_cifid = ds2.Tables[0].Rows[i][2].ToString(); //CIFID
                                string hit_type = ds2.Tables[0].Rows[i][3].ToString(); //File Format
                                                                                       //=========================================================================================

                                if (cnt_hit > 15)
                                {
                                    break;
                                }

                                if (beneficiary_id.Trim().ToUpper() == hit_cifid.Trim().ToUpper())
                                {
                                    sanction_list_ben_id = hit_cifid;

                                    if (hit_type.Trim().ToUpper() == "OFAC")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|10" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    //=========================================================================================
                                    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                                    if (hit_type.Trim().ToUpper() == "OFAC-CON")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|10" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }
                                    //=========================================================================================

                                    if (hit_type.Trim().ToUpper() == "LOCAL")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|20" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "LOCAL COMPLIANCE")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|20" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "SANCTION")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|18" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "PEPS")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|28" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "CFSP") //GLOBAL FILE FOR EUROPE
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|16" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "UNSC") //SANCTION FILE FOR SG
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|18" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "AMLA (MOF)") //AMLA(MOF)
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|12" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "ASF")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|30" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "FRANCE")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|32" + "~";
                                        watchlist_ben += hit_type.Trim().ToUpper() + ", ";
                                    }

                                }

                                if (remitter_id.Trim().ToUpper() == hit_cifid.Trim().ToUpper())
                                {
                                    sanction_list_rem_id = hit_cifid;

                                    if (hit_type.Trim().ToUpper() == "OFAC")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|9" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    //=========================================================================================
                                    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                                    if (hit_type.Trim().ToUpper() == "OFAC-CON")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|9" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }
                                    //=========================================================================================

                                    if (hit_type.Trim().ToUpper() == "LOCAL")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|19" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "LOCAL COMPLIANCE")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|19" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "SANCTION")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|17" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "PEPS")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|27" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "CFSP") //GLOBAL FILE FOR EUROPE
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|15" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "UNSC") //SANCTION FILE FOR SG
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|17" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "AMLA (MOF)") //AMLA(MOF)
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|11" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "ASF")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|29" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                    if (hit_type.Trim().ToUpper() == "FRANCE")
                                    {
                                        System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += override_user[0] + "|31" + "~";
                                        watchlist_rem += hit_type.Trim().ToUpper() + ", ";
                                    }

                                }

                                cnt_hit++;
                            }
                        }
                    }

                    //=======================================================================================
                    //RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
                    bool ibool = false;

                    if (remit_method == 2 || remit_method == 10)
                    {
                        ibool = new irsOverrideLog("", "OBPS").LogOverride(transaction_id);
                    }
                    else
                    {
                        ibool = new irsOverrideLog("", "OTC").LogOverride(transaction_id);
                    }
                    //=======================================================================================

                    //AUTO-UNTAGGING FEATURE ON HIT LIST TABLE

                    //=========================================================================================
                    /* RFC-30454 Creation of new report in IRSW for OTC transactions with OFAC hits */
                    try
                    {
                            if (string.IsNullOrEmpty(sanction_list_ben_id).Equals(false) && string.IsNullOrEmpty(sanction_list_rem_id).Equals(false))
                            {
                                if (sanction_list_ben_id.Trim().ToUpper() == sanction_list_rem_id.Trim().ToUpper())
                                {
                                    //=========================================================================================
                                    //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                                    //new RemTranExtra().insert_sanction_list_discounting(sanction_list_rem_id, string.Empty, string.Empty, string.Empty, watchlist_rem, transaction_id.ToString());
                                    new RemTranExtra().insert_sanction_list_discounting(sanction_list_rem_id, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, watchlist_rem, transaction_id.ToString());
                                    //=========================================================================================
                                    System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"] = null; return;
                                }
                            }

                            if (string.IsNullOrEmpty(sanction_list_ben_id).Equals(false))
                            {
                                //=========================================================================================
                                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                                //new RemTranExtra().insert_sanction_list_discounting(sanction_list_ben_id, string.Empty, string.Empty, string.Empty, watchlist_ben, transaction_id.ToString());
                                new RemTranExtra().insert_sanction_list_discounting(sanction_list_ben_id, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, watchlist_ben, transaction_id.ToString());
                                //=========================================================================================
                            }

                            if (string.IsNullOrEmpty(sanction_list_rem_id).Equals(false))
                            {
                                //=========================================================================================
                                //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                                //new RemTranExtra().insert_sanction_list_discounting(sanction_list_rem_id, string.Empty, string.Empty, string.Empty, watchlist_rem, transaction_id.ToString());
                                new RemTranExtra().insert_sanction_list_discounting(sanction_list_rem_id, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, watchlist_rem, transaction_id.ToString());
                                //=========================================================================================
                            }

                        if (string.IsNullOrEmpty(sanction_list_rem_id).Equals(false))
                        {
                            //=========================================================================================
                            //* RFC-32829 : Request to enhance the OFAC alert and Report in IRSW 01/14/2024 */
                            //new RemTranExtra().insert_sanction_list_discounting(sanction_list_rem_id, string.Empty, string.Empty, string.Empty, watchlist_rem, transaction_id.ToString());
                            new RemTranExtra().insert_sanction_list_discounting(sanction_list_rem_id, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, watchlist_rem, transaction_id.ToString());
                            //=========================================================================================
                        }
                    }
                    catch (Exception e)
                    {
                        this.logs("", "WATCH LIST", e.Message.ToString(), e.StackTrace.ToString(), "");
                    }
                    //=========================================================================================

                }

                System.Web.HttpContext.Current.Session["OTC_HIT_LIST_VAL"] = null;
            }
            catch (Exception ex)
            {
                this.logs("", "WATCH LIST", ex.Message.ToString(), ex.StackTrace.ToString(), "");
            }
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();
            bool insertGUID = true; 

            System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;

            /* OBBA EXCISE TAX */
            string OBBAComputedTax = string.Empty;
            string OBBAPercentage = string.Empty;

            if (System.Web.HttpContext.Current.Session["OBBA_COMPUTEDTAX"] != null)
            {
                OBBAComputedTax = System.Web.HttpContext.Current.Session["OBBA_COMPUTEDTAX"].ToString();
            }
            if (System.Web.HttpContext.Current.Session["OBBA_PERCENTAGE"] != null)
            {
                OBBAPercentage = (string)System.Web.HttpContext.Current.Session["OBBA_PERCENTAGE"].ToString();
            }
            /* OBBA EXCISE TAX */

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                //new line inserted by boyet to detect duplicate transaction
                this.set_chk_dup_value(s);
                //end here

                switch (s[0].Trim().ToUpper())
                {
                    /* OBBA EXCISE TAX */
                    case "OBBACOMPUTEDTAX":
                        s[1] = OBBAComputedTax;
                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                        continue;
                    case "OBBAPERCENTAGE":
                        s[1] = OBBAPercentage;
                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                        continue;
                    case "SEQ_ID"          : continue;
                    case "TRANXDATE"       : continue;
                    case "REFERENCENO"     : continue;
                    case "USERID"          : continue;
                    case "REMITTINGBRANCH" : continue;
                    case "CHARGELOCALLY"   :

                        if (s[1] == "0" || s[1] == "" || s[1].Length == 0 || s[1].ToUpper() == "FALSE")
                        {
                            s[1] = "false";
                        }
                        else
                        {
                            s[1] = "true";
                        }

                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }

                        continue;
                }

                if (record_is_edit == false)
                {
                    if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                    {
                        irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                        this.remittance_transaction_date = new irsInternetPage().getTransactionDate(loggin_data.Location, true);

                        if (!base.setColumn("TranxDate", this.remittance_transaction_date))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                        if (!base.setColumn("RemittingBranch", loggin_data.Location))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                        if (!base.setColumn("UserID", loggin_data.UserId))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                    }

                    if (s != null && s.Length > 0)
                    {
                        if (s[0].Trim().ToUpper() == "PRODUCTCHARGESID")
                        {
                            this.product_charge_id = Convert.ToInt32(s[1].ToString());
                        }
                    }
                }
                if (insertGUID)
                {
                    Guid guid = Guid.NewGuid();
                    if (!base.setColumn("GUID", guid.ToString()))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                    insertGUID = false;                    
                    System.Web.HttpContext.Current.Session["RemittanceGUID"] = guid.ToString();      
                    
                }
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

            this.set_remitter_benef_contact_seq(this.RemitterCIFID, this.BeneficiaryCIFID, error_list);

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

            /* VAPT 20181016 */
            System.Web.HttpContext.Current.Session["OTCLog"] = null;
            System.Web.HttpContext.Current.Session["OTCSys"] = null;
            string[] arrVal = column_values;
            string log = string.Empty;
            string sys = string.Empty;
            int j = 0;
            string val = string.Empty;
            string[] arrDetails;

            if (arrVal != null && arrVal.Length > 0)
            {
                for (j = 0; j <= (arrVal.Length - 1); j++)
                {
                    val = new irsOBPSTransactions().getTransactionDetails(arrVal.GetValue(j).ToString(), "~");
                    arrDetails = val.Split('^');

                    if (arrVal.GetValue(j).ToString().Split('~').GetValue(0).ToString().Trim().ToUpper() == "TRANXDATE")
                    {
                        arrDetails[0] = this.remittance_transaction_date.ToString();
                    }

                    if (arrDetails.GetValue(0).ToString().Trim() != string.Empty)
                    {
                        log += arrVal.GetValue(j).ToString().Split('~').GetValue(0).ToString().Trim() + "=" + arrDetails.GetValue(0).ToString().Trim() + " ~ ";
                    }
                    else
                    {
                        if (arrDetails.GetValue(1).ToString().Trim() != string.Empty)
                        {
                            sys += arrVal.GetValue(j).ToString().Split('~').GetValue(0).ToString().Trim() + "=" + arrDetails.GetValue(1).ToString().Trim() + " ~ ";
                        }
                    }
                }
            }
            System.Web.HttpContext.Current.Session["OTCLog"] = log;
            System.Web.HttpContext.Current.Session["OTCSys"] = sys;

            return this.commit_record();            
        }

        private void set_remitter_benef_contact_seq(string remitter_id, string beneficiary_id, ArrayList error_list)
        {
            DataTable dt = getDataSet("otc_get_contact_nos_seqid", true, new object[] { remitter_id, beneficiary_id }).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                if (!base.setColumn("BeneficiaryContactSeqID", Convert.ToInt32(dt.Rows[0]["beneficiary_seq_id"])))
                {
                    error_list.Add("BeneficiaryContactSeqID:" + "Unable to set beneficiary mobile number.");
                }
                if (!base.setColumn("RemitterContactSeqID", Convert.ToInt32(dt.Rows[0]["remitter_seq_id"])))
                {
                    error_list.Add("RemitterContactSeqID:" + "Unable to set remitter mobile number.");
                }
            }
        }

        private void get_error_dbase()
        {            
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;
                System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = base.Message;                
            }
        }

        private void commit_other_details(string s)
        {
            string[] arrays = s.Split('~');

            if (arrays != null && arrays.Length > 0)
            {
                base.EnforceConstraint = false;

                DataTable dt = null;

                try
                {
                    dt = getDataSet(getSqlCommand("SELECT * FROM TBL_CHARGE_MASTER WHERE SEQ_ID = " + Convert.ToInt32(arrays[1]).ToString(), false), "GOT_IT").Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["ProductCode"]) == 12 && Convert.ToInt32(dt.Rows[0]["ImplementationType"]) == 4 || Convert.ToInt32(dt.Rows[0]["ProductCode"]) == 13 && Convert.ToInt32(dt.Rows[0]["ImplementationType"]) == 5)
                        {
                            rem_other_det details = new rem_other_det(); string result = details.insert_record("REM_SEQ_ID~" + base.IdentityValue.ToString());
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
            }
        }

        private int get_transaction_sequence_id(int commit_seq_id)
        {            
            DataTable dt = null;
            String guid = null;
            try
            {
                guid = (String)System.Web.HttpContext.Current.Session["RemittanceGUID"];
                string s = "SELECT SEQ_ID AS SEQ_ID " +
                            "FROM TBL_REMITTANCE " +
                            "WHERE GUID = '" + guid + "'";
                             
                dt = getDataSet(getSqlCommand(s, false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0] != null && Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        this.logs("", "RemTranSaving.get_transaction_sequence_id", "Returned value is " + dt.Rows[0][0].ToString(), "", "");
                        return Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
                else
                {
                    this.logs("", "RemTranSaving.get_transaction_sequence_id", "Requery: Returned value is " + commit_seq_id.ToString(), "", "");
                    System.Threading.Thread.Sleep(4000);
                    dt = getDataSet(getSqlCommand(s, false)).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0] != null && Convert.ToInt32(dt.Rows[0][0]) > 0)
                        {
                            this.logs("", "RemTranSaving.get_transaction_sequence_id", "Returned value is " + dt.Rows[0][0].ToString(), "", "");
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                    }
                }

                this.logs("", "RemTranSaving.get_transaction_sequence_id", "Returned value is " + commit_seq_id.ToString() +", but system returned zero.", "", "");
                return 0;
            }
            catch(Exception err)
            {
                this.logs("", "RemTranSaving.get_transaction_sequence_id", "ERROR while getting transaction sequence_id, mthod returned " + commit_seq_id.ToString() + " : " + err.Message.ToString(), "", "");
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

        private int get_trxn_sequence_id(int commit_seq_id)
        {
            irsClientData loggin_data = null;

            DataTable dt = null;

            try
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                }

                if (loggin_data != null)
                {
                    string s = "SELECT MAX(SEQ_ID) AS SEQ_ID " +
                                "FROM TBL_REMITTANCE " +
                                "WHERE " +
                                    "RemitMethod     = 4 AND " +
                                    "RemittingBranch = '" + loggin_data.Location.Replace("'", "''") + "' AND " +
                                    "UserId = '" + loggin_data.UserId + "' AND " +
                                    "CONVERT(VARCHAR,TranxDate,101) = '" + remittance_transaction_date.ToString("MM/dd/yyyy") + "'";

                        dt = getDataSet(getSqlCommand(s, false)).Tables[0];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0] != null && Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) != commit_seq_id)
                        {
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                    }
                }

                return commit_seq_id;
            }
            catch
            {
                return commit_seq_id;
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
        private int get_transaction_sequence_id_new_table(int commit_seq_id)
        {
            irsClientData loggin_data = null;

            DataTable dt = null;

            try
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                }

                if (loggin_data != null)
                {
                    string s = "SELECT MAX(SEQ_ID) AS SEQ_ID " +
                                "FROM TBL_REMITTANCE " +
                                "WHERE " +
                                    "RemitMethod     = 4 AND " +
                                    "RemittingBranch = '" + loggin_data.Location.Replace("'", "''") + "' AND " +
                                    "UserId = '" + loggin_data.UserId + "' AND " +
                                    "CONVERT(VARCHAR,TranxDate,101) = '" + remittance_transaction_date.ToString("MM/dd/yyyy") + "'";

                    dt = getDataSet(getSqlCommand(s, false)).Tables[0];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0] != null && Convert.ToInt32(dt.Rows[0][0]) > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0][0]) != commit_seq_id)
                        {
                            return Convert.ToInt32(dt.Rows[0][0]);
                        }
                    }
                }

                return commit_seq_id;
            }
            catch
            {
                return commit_seq_id;
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
        /*
        private csRemittance remSave = new csRemittance();

        private bool saveNewRemittanceTable(string calcSessionName, int productChargesID, csRemittance.RemitMethod remitMethod, 
                    string userID, string extlRefNo, DateTime tranxDate, 
                    DateTime clearingDate, string checkNo, string remitterID, int remitterAddressID, 
                    int remitterContactID,string beneficiaryID, int beneficiaryAddressID, 
                    int beneficiaryContactID, int relationship, int notificationMode, bool selfAllotee,
                    int remitterCardID, int beneficiaryAccountID, string KYCID, int KYCAddressID,
                    int computationLevel, int tranxSource, int tranxPurpose, string otherPupose,
                    int receiptMode, int sourceOfFunds, string specialMessage,
                    string batchNo, string merchantName, string merchantID,
                    DateTime obpDateFrom, DateTime obpDateTo, string location, int fileSEQID)
        {

            
            remSave.saveRemittance(calcSessionName, productChargesID, 
                                                remitMethod, userID, location, extlRefNo, tranxDate, 
                                                clearingDate, checkNo, remitterID, remitterAddressID, 
                                                remitterContactID, beneficiaryID, 
                                                beneficiaryAddressID, beneficiaryContactID,
                                                relationship, notificationMode, selfAllotee, 
                                                remitterCardID, beneficiaryAccountID, KYCID, 
                                                KYCAddressID, computationLevel, tranxSource, tranxPurpose, 
                                                otherPupose, receiptMode, sourceOfFunds, specialMessage, 
                                                batchNo, fileSEQID, merchantName, merchantID, 0 ,obpDateFrom, obpDateTo);

            System.Web.HttpContext.Current.Session["SEQUENCE_ID_NEW_TABLE"] = remSave.rem_seq_id;
            
            return true;
                            
        }
         */
        
    }

    /* END REMITTANCE MAIN TABLE COMMIT CLASS */

    [Serializable()]
    public class rem_door_saving : sqlHelperWs
    {
        private string seq_id = null;
        private string error_message = null;
        private csAuditLog logger = new csAuditLog();

        public rem_door_saving()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_REMITTANCE_3D_DETAILS";
            this.IdentityColumn = "cou_seq_id";
            this.error_message = null;
            
            //commented by boyet to use faster construct SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_3D_DETAILS WHERE cou_seq_id = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_REMITTANCE_3D_DETAILS WHERE cou_seq_id = " + id.ToString(), false);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record(string column_values)
        {
            bool ibool = false;
            this.logs("", "rem_door_saving.insert_record", "Inserting to remittance table", column_values, "");
            string[] s = column_values.Split('|');

            System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;

            if (base.newRow() == true)
            {
                ibool = this.set_record_and_commit(s);
            }

            if (ibool == false)
            {
                this.get_error_dbase();
                this.rollback_record();
                this.logs("", "rem_door_saving.insert_record", "Inserting to remittance table", "Error in set_record_and_commit returns false :" + base.Message, "");
            }

            return ibool;
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
        private void logs(string userid, string module, string description, string newvalue, string oldvalue)
        {
            this.logger.writeAudit(userid, module, description, newvalue, oldvalue, csAuditLog.Category.Insert);
        }
        private bool set_record_and_commit(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            System.Web.HttpContext.Current.Session["TRAN_ERROR_MSG"] = null;

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

        private void rollback_record()
        {
            base.cancel();
        }

        private bool commit_record()
        {
            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }

            return false;
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
    public class rem_other_det : sqlHelperWs
    {
        private string message_rem = string.Empty;
        private csAuditLog logger = new csAuditLog();

        public rem_other_det()
        {
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_REMITTANCE_OTHER_DETAILS";
            this.IdentityColumn = "SEQ_ID";

            //SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_OTHER_DETAILS WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);

            base.getUpdateDataSet("SELECT * FROM TBL_REMITTANCE_OTHER_DETAILS WHERE SEQ_ID = " + id.ToString(), false);

        }

        public string insert_record(string column_values)
        {
            bool ibool = false;
            this.logs("", "rem_other_det.insert_record", "Inserting to remittance table", column_values, "");
            string[] s = column_values.Split('|');
            string[] s2 = s[0].Split('~');

            if (!getCheckIfExists(s2[1].ToString()))
            {
                if (base.newRow() == true)
                {
                    ibool = this.set_record_and_commit(s);
                }
                else
                {
                    this.message_rem = base.Message;
                }

                if (ibool == false)
                {
                    this.rollback_record();
                    this.logs("", "rem_other_det.insert_record", "Inserting to remittance table", "Error in set_record_and_commit returns false :" + base.Message, "");
                    return "0~" + this.message_rem;
                }
            }
            return "1~";
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

        private void logs(string userid, string module, string description, string newvalue, string oldvalue)
        {
            this.logger.writeAudit(userid, module, description, newvalue, oldvalue, csAuditLog.Category.Insert);
        }
        private bool commit_record()
        {
            if (base.save() == false)
            {
                this.message_rem = base.Message; return false;
            }

            return true;
        }

        private void rollback_record()
        {
            base.cancel();
        }

        private bool set_record_and_commit(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            this.message_rem = string.Empty;

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

                this.message_rem = sb.ToString(); return false;

            }

            return this.commit_record();
        }
    }
}


