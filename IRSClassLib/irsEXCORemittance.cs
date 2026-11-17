using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
namespace IRSClassLib
{
    public class irsEXCORemittance : sqlHelperWs 
    {
        private string error_message;
        private string _remit_seq_id;

        private bool _charge_locally;
        private string _user_id;
        private string _branch_code;

        private int _tran_status;
        private string _acct_cur;
        private string _charge_id;
        private irsCalculator rem_cal;
        private string tranx_summary;

        private DateTime _transaction_date;

        public string ProductChargeId
        {
            get { return _charge_id.ToString(); }
            set { _charge_id = value; }
        }

        public int TransStatus
        {
            get { return _tran_status; }
        }

        public string RemittanceId
        {
            get { return _remit_seq_id; }
        }

        public bool ChargeLocally
        {
            get { return _charge_locally; }
            set { _charge_locally = value; }
        }

        public string AccountCurrency
        {
            get { return _acct_cur; }
            set { _acct_cur = value; }
        }

        public string TranxSummary
        {
            get { return tranx_summary; }
        }

        public string RemitMessage
        {
            get { return this.error_message; }
            set { this.error_message = value; }
        }

        public string Charge_Id
        {
            get { return this._charge_id; }
        }

        #region PUBLIC METHODS
        public bool Remit(int acct_cur, double pay_amount, string[] remit_param, int rate_type)
        {
            this._acct_cur = acct_cur.ToString(); ;

            if (this._charge_id != null && Convert.ToInt32(this._charge_id) > 0)
            {
                if (this.insertRecord())
                {
                    if (this.set_record(remit_param))
                    {
                        int remit_method = 0;
                        if (uprow["RemitMethod"] != null) remit_method = Convert.ToInt32(uprow["RemitMethod"]);
                        if (rate_type > 0)
                        {
                            this.computeCharges(remit_method, (double)pay_amount, acct_cur, rate_type);
                        }
                        else this.computeCharges(remit_method, (double)pay_amount, acct_cur);

                        if (this.commitRecord())
                        {
                            string txn_no = this.IdentityValue.ToString();
                            this.rem_cal.commit_remittance_charge_details(Convert.ToInt32(this.IdentityValue));
                            return true;
                        }
                        else
                        {
                            this.error_message = base.Message;
                        }
                    }
                }
            }
            else
            {
                this.error_message = "ChargesId cannot be 0 or null.";
            }

            if (this.error_message == "") this.error_message = base.Message;
            return false;
        }

        public bool Remit(int acct_cur, double pay_amount, int charge_id, string[] remit_param, bool isOBP)
        {
            if (!isOBP)
            {
                return this.Remit(acct_cur, pay_amount, charge_id, remit_param);
            }
            else
            {
                return this.Remit(acct_cur, pay_amount, charge_id, remit_param, 3);
            }
        }

        public bool Remit(int acct_cur, double pay_amount, int charge_id, string[] remit_param, bool isOBP, int compute)
        {
            if (!isOBP)
            {
                return this.Remit(acct_cur, pay_amount, charge_id, remit_param, compute);
            }
            else
            {
                //return this.Remit(acct_cur, pay_amount, charge_id, remit_param, 3);
            }

            return false;
        }

        //public bool Remit(int acct_cur, double pay_amount, int charge_id, string[] remit_param, int compute_method)
        //{
        //    irsOBPSTransactions  obp = new irsOBPSTransactions ();
        //    irsCalculator  calc = new irsCalculator (this._branch_code, charge_id, acct_cur, false, compute_method);

        //    string param = this.setObpParam(remit_param, calc, Convert.ToDecimal(pay_amount));

        //    return false;
        //}

        public bool Remit(int acct_cur, double pay_amount, int charge_id, string[] remit_param, int compute)
        {
            this.initializeInsertRemittance();
            this._acct_cur = acct_cur.ToString();

            if (charge_id > 0)
            {
                this._charge_id = charge_id.ToString();
                if (this.insertRecord())
                {
                    if (this.set_record(remit_param))
                    {
                        int remit_method = 0;
                        if (uprow["RemitMethod"] != null) remit_method = Convert.ToInt32(uprow["RemitMethod"]);
                        this.computeCharges(compute, remit_method, (double)pay_amount, acct_cur);

                        if (this.commitRecord())
                        {
                            string txn_no = this.IdentityValue.ToString();
                            this.rem_cal.commit_remittance_charge_details(Convert.ToInt32(this.IdentityValue));
                            return true;
                        }
                    }
                }
            }
            else
            {
                this.error_message = "ChargesId should not be 0 or null.";
            }

            if (this.error_message == "") this.error_message = Message;
            return false;
        }

        public bool Remit(int acct_cur, double pay_amount, int charge_id, string[] remit_param)
        {
            this.initializeInsertRemittance();
            this._acct_cur = acct_cur.ToString();

            if (charge_id > 0)
            {
                this._charge_id = charge_id.ToString();
                if (this.insertRecord())
                {
                    if (this.set_record(remit_param))
                    {
                        int remit_method = 0;
                        if (uprow["RemitMethod"] != null) remit_method = Convert.ToInt32(uprow["RemitMethod"]);
                        this.computeCharges(remit_method, (double)pay_amount, acct_cur);

                        if (this.commitRecord())
                        {
                            string txn_no = this.IdentityValue.ToString();
                            this.rem_cal.commit_remittance_charge_details(Convert.ToInt32(this.IdentityValue));
                            return true;
                        }
                    }
                }
            }
            else
            {
                this.error_message = "ChargesId should not be 0 or null.";
            }

            if (this.error_message == "") this.error_message = Message;
            return false;
        }

        public bool Remit(int acct_cur, double pay_amount, string[] remit_param)
        {
            this.initializeInsertRemittance();
            this._acct_cur = acct_cur.ToString();

            if (this._charge_id != null && Convert.ToInt32(this._charge_id) > 0)
            {
                if (this.insertRecord())
                {
                    if (this.set_record(remit_param))
                    {
                        int remit_method = 0;
                        if (uprow["RemitMethod"] != null) remit_method = Convert.ToInt32(uprow["RemitMethod"]);
                        this.computeCharges(remit_method, (double)pay_amount, acct_cur);

                        if (this.commitRecord())
                        {
                            string txn_no = this.IdentityValue.ToString();
                            this.rem_cal.commit_remittance_charge_details(Convert.ToInt32(this.IdentityValue));                            
                            return true;
                            
                        }
                    }
                }
            }
            else
            {
                this.error_message = "ChargesId should not be 0 or null.";
            }

            if (this.error_message == "") this.error_message = Message;
            return false;
        }

        public bool RemitAlRajhi(int acct_cur, double pay_amount, string[] remit_param,string filename)
        {
            this.initializeInsertRemittance();
            this._acct_cur = acct_cur.ToString();

            if (this._charge_id != null && Convert.ToInt32(this._charge_id) > 0)
            {
                if (this.insertRecord())
                {
                    if (this.set_record(remit_param))
                    {
                        int remit_method = 0;
                        if (uprow["RemitMethod"] != null) remit_method = Convert.ToInt32(uprow["RemitMethod"]);
                        this.computeCharges(remit_method, (double)pay_amount, acct_cur);
                        string extrefno = uprow["ExtlReferenceNo"].ToString();
                        if (this.commitRecord())
                        {
                            //string txn_no = this.IdentityValue.ToString();
                            string txn_no = this.get_transaction_id( new irsInternetPage().getTransactionDate("A06",false), extrefno);

                            if (txn_no.Trim().Length == 0) return false;

                            this.rem_cal.commit_remittance_charge_details(Convert.ToInt32(txn_no));

                            try
                            {

                                this._remit_seq_id = txn_no;
                                string br_seq_id = this.get_branch_seq_id();
                                string file_seq_id  = this.get_file_inventory_id(extrefno);
                                    if (file_seq_id.Length > 0 &&
                                         br_seq_id.Trim().Length > 0)
                                    {
                                       bool pass = this.doOtherDetails(br_seq_id,1,"GA06",file_seq_id,"","","",DateTime.Now.ToString());
                                     
                                    }
                            }
                            catch(Exception e)
                            {
                                  this.error_message = "Error in writing data "  + e.StackTrace.ToString();
                            }


                            
                            irsEXCOAlRajhiResponse resp = new irsEXCOAlRajhiResponse();
                            if (resp.update_alrajhi_reference(extrefno, txn_no, filename))
                                return true;
                            else
                                return false;
                        }
                    }
                }
            }
            else
            {
                this.error_message = "ChargesId should not be 0 or null.";
            }

            if (this.error_message == "") this.error_message = Message;
            return false;
        }

        public bool Remit(int acct_cur, string charge_id, double pay_amount, string[] remit_param)
        {
            this.initializeInsertRemittance();
            this._charge_id = charge_id;
            this._acct_cur = acct_cur.ToString();

            if (acct_cur > 0)
            {
                if (this._charge_id != null && Convert.ToInt32(this._charge_id) > 0)
                {
                    if (this.insertRecord())
                    {
                        if (this.set_record(remit_param))
                        {
                            int remit_method = 0;
                            if (uprow["RemitMethod"] != null) remit_method = Convert.ToInt32(uprow["RemitMethod"]);
                            this.computeCharges(remit_method, (double)pay_amount, acct_cur);

                            if (this.commitRecord())
                            {
                                string txn_no = this.IdentityValue.ToString();
                                this.rem_cal.commit_remittance_charge_details(Convert.ToInt32(this.IdentityValue));
                                return true;
                            }
                        }
                    }
                }
                else
                {
                    this.error_message = "No applicable charge for this transaction.";
                }
            }
            else this.error_message = "ERROR: CIF ACCOUNTS - Account Currency should not be 0";

            if (this.error_message == "") this.error_message = Message;
            return false;
        }

        public bool doOtherDetails(string file_seq_id)
        {
            if (this.doOtherDetails("", 0, "", file_seq_id, "", "", "", ""))
            {
                this.getTranxSummary(_remit_seq_id);
                return true;
            }
            return false;
        }

        public bool doOtherDetails(string br_seq_id, int acct_typ, string RecordId, string File_Seq_Id, string prot_bank, string rem_bank_cd, string ORDBatchNo, string date_sent)
        {
            base.TableName = "TBL_REMITTANCE_OTHER_DETAILS";
            base.IdentityColumn = "SEQ_ID";
            base.getUpdateDS("SELECT * FROM TBL_REMITTANCE_OTHER_DETAILS WHERE SEQ_ID = 0");

            try
            {
                if (base.newRow(true))
                {
                    uprow["REM_SEQ_ID"] = this._remit_seq_id;
                    uprow["BR_SEQ_ID"] = (br_seq_id == "" ? 0 : Convert.ToInt32(br_seq_id));
                    uprow["AccountType"] = acct_typ;
                    uprow["Currency"] = this._acct_cur;
                    uprow["RecordId"] = (RecordId.Length == 4) ? RecordId.Substring(0, 4) : RecordId;
                    uprow["FileInvSeqID"] = File_Seq_Id;
                    uprow["ProtBankCd"] = prot_bank;
                    //uprow["GLCode"] = GLCode;
                    uprow["RemittingBankCode"] = rem_bank_cd;
                    uprow["ORDBatchNo"] = ORDBatchNo;
                    uprow["DateSent"] = (date_sent != null && date_sent.Length > 0) ? Convert.ToDateTime(date_sent) : DateTime.Now;

                    base.table.Rows.Add(uprow);
                    base.addMode = false;
                    if (base.save())
                    {
                        this.getTranxSummary(_remit_seq_id);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            error_message = base.Message;
            return false;
        }

        public bool doOtherDetails(string br_seq_id, int acct_typ, string RecordId, string File_Seq_Id, string prot_bank, string rem_bank_cd, string ORDBatchNo, string date_sent, string GLCode)
        {
            base.TableName = "TBL_REMITTANCE_OTHER_DETAILS";
            base.IdentityColumn = "SEQ_ID";
            base.getUpdateDS("SELECT * FROM TBL_REMITTANCE_OTHER_DETAILS WHERE SEQ_ID = 0");

            try
            {
                if (base.newRow(true))
                {
                    uprow["REM_SEQ_ID"] = this._remit_seq_id;
                    uprow["BR_SEQ_ID"] = (br_seq_id == "" ? 0 : Convert.ToInt32(br_seq_id));
                    uprow["AccountType"] = acct_typ;
                    uprow["Currency"] = this._acct_cur;
                    uprow["RecordId"] = (RecordId.Length == 4) ? RecordId.Substring(0, 4) : RecordId;
                    uprow["FileInvSeqID"] = File_Seq_Id;
                    uprow["ProtBankCd"] = prot_bank;
                    uprow["GLCode"] = GLCode;
                    uprow["RemittingBankCode"] = rem_bank_cd;
                    uprow["ORDBatchNo"] = ORDBatchNo;
                    uprow["DateSent"] = (date_sent != null && date_sent.Length > 0) ? Convert.ToDateTime(date_sent) : DateTime.Now;

                    base.table.Rows.Add(uprow);
                    base.addMode = false;
                    if (base.save())
                    {
                        this.getTranxSummary(_remit_seq_id);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            error_message = base.Message;
            return false;
        }

        public bool doOtherDetails(string br_seq_id, int acct_typ, string RecordId, string File_Seq_Id, string prot_bank, string rem_bank_cd, string ORDBatchNo, string date_sent, double tbg_amt)
        {
            base.TableName = "TBL_REMITTANCE_OTHER_DETAILS";
            base.IdentityColumn = "SEQ_ID";
            base.getUpdateDS("SELECT * FROM TBL_REMITTANCE_OTHER_DETAILS WHERE SEQ_ID = 0");
            
            try
            {
                if (base.newRow(true))
                {
                    uprow["other_amount"] = tbg_amt;
                    uprow["REM_SEQ_ID"] = this._remit_seq_id;
                    uprow["BR_SEQ_ID"] = (br_seq_id == "" ? 0 : Convert.ToInt32(br_seq_id));
                    uprow["AccountType"] = acct_typ;
                    uprow["Currency"] = this._acct_cur;
                    uprow["RecordId"] = (RecordId.Length == 4) ? RecordId.Substring(0, 4) : RecordId;
                    uprow["FileInvSeqID"] = File_Seq_Id;
                    uprow["ProtBankCd"] = prot_bank;
                    //uprow["GLCode"] = GLCode;
                    uprow["RemittingBankCode"] = rem_bank_cd;
                    uprow["ORDBatchNo"] = ORDBatchNo;
                    uprow["DateSent"] = (date_sent != null && date_sent.Length > 0) ? Convert.ToDateTime(date_sent) : DateTime.Now;

                    base.table.Rows.Add(uprow);
                    base.addMode = false;
                    if (base.save())
                    {
                        this.getTranxSummary(_remit_seq_id);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            error_message = base.Message;
            return false;
        }

        public string getChargeId(string br_cd, int prod_cd, int implem_cd, int rem_cur, int charg_typ, int paymod, int comput_lvl)
        {
            if (chargesDefined(br_cd, prod_cd, implem_cd, rem_cur, charg_typ, paymod, comput_lvl))
                return this._charge_id;
            else return "";
        }

        public bool chargesDefined(int prod_cd, int implem_cd, int rem_cur, string charg_typ_nme, int paymod, int comput_lvl)
        {
            return chargesDefined(this._branch_code, prod_cd, implem_cd, rem_cur, charg_typ_nme, paymod, comput_lvl);
        }

        public bool chargesDefined(int prod_cd, int implem_cd, int rem_cur, int charg_typ, int paymod, int comput_lvl)
        {
            return this.chargesDefined(this._branch_code, prod_cd, implem_cd, rem_cur, charg_typ, paymod, comput_lvl);
        }

        public bool chargesDefined(string br_cd, int prod_cd, int implem_cd, int rem_cur, int charg_typ, int paymod, int comput_lvl)
        {
            try
            {
                object[] charges_param = new object[] { br_cd,
                                                    prod_cd,
                                                    implem_cd,
                                                    rem_cur,
                                                    charg_typ,
                                                    paymod };
                DataTable dt = execStoredProcedureTable("isp_getChargesID", charges_param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    this._tran_status = Convert.ToInt32(dt.Rows[0]["DefaultStatus"]);
                    this._charge_id = dt.Rows[0]["SEQ_ID"].ToString();
                    return (Convert.ToInt32(this._charge_id) > 0);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            this.error_message = "No applicable charge setup defined for this transaction.";
            return false;
        }

        public bool chargesDefined(string br_cd, int prod_cd, int implem_cd, int rem_cur, string charg_typ_nme, int paymod, int comput_lvl)
        {
            string charg_typ = "";
            try
            {
                object o = execScalar("SELECT FldValue FROM TBL_LOOKUP WHERE FldGroup = 'ChargeType' AND FldCode = '" + charg_typ_nme + "'");
                charg_typ = o.ToString();
            }
            catch 
            {
                throw new Exception("ChargeType not defined on the look up table.");
            }
            return chargesDefined(br_cd, prod_cd, implem_cd, rem_cur, Convert.ToInt32(charg_typ), paymod, comput_lvl);
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand cmdDefaults = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(cmdDefaults);
        }

        public bool check_items_in_rem(string _ext_refnum,DateTime _tranxdate,string _branchcode)
        {
            string param = _ext_refnum + "," + _tranxdate + "," + _branch_code;
            DataTable dt = this.execStoredProcedureTable("proc_check_exco_remitems", param.Split(','));
            if (dt != null)
            {
                try
                {
                    string txn_no = this.get_transaction_id(new irsInternetPage().getTransactionDate("A06", false), _ext_refnum);

                    irsEXCOAlRajhiResponse resp = new irsEXCOAlRajhiResponse();
                    bool pass = resp.update_alrajhi_reference(_ext_refnum, txn_no, this.get_file_filename(_ext_refnum));
                }
                catch
                {

                }
                 return (dt.Rows.Count > 0) ? true: false;                
            }
            
            return false;
        }

        public string setProductChargesID(string _branchcode,string _productcode, string _implementationcode,
                                        string _currency ,string _paymode)
        {
            string param = _branchcode + "," + _productcode + "," +
                            _implementationcode + "," + _currency + "," + _paymode; 


            DataTable dt = this.execStoredProcedureTable("proc_get_prod_chargesid",param.Split(','));
            if (dt.Rows.Count > 0)
            {
                return  dt.Rows[0]["SEQ_ID"].ToString();
            }
            return "";
        }
        #endregion

        #region PRIVATE METHODS
        private string setObpParam(string[] param, irsCalculator  calc, decimal amount)
        {
            StringBuilder sb = new StringBuilder();

            for (int ndx = 0; ndx < param.Length; ndx++)
            {
                if (param[ndx].Split('~').Length > 1)
                {
                    string[] par = param[ndx].Split('~');
                    string fld = par[0];
                    string val = par[1];

                    sb.Append(fld + "=" + val);
                    sb.Append(",");
                }
            }

            sb.Append(getCalcValues(calc, amount));

            return "";
        }

        private string getCalcValues(irsCalculator  calc, decimal amount)
        {
            StringBuilder sb = new StringBuilder();
            calc.compute_obps_charges(3, amount);

            sb.Append("GrossRemAmount=" + calc.get_remittance_gross_branch().ToString() + ","); //0                
            sb.Append("NetRemAmt=" + calc.get_remittance_net_branch().ToString().ToString().ToString() + ","); //1
            sb.Append("NetToBeneAmount=" + calc.get_remittance_net_to_bene_amt().ToString() + ","); //2
            sb.Append("TotalCharge=" + calc.get_remittance_total_charges_amt().ToString() + ","); //3
            sb.Append("ForexLocalToRemitting=" + calc.get_forex_source_to_target().ToString() + ","); //4
            sb.Append("ForexRemittingToUSD=" + calc.get_forex_target_to_usd().ToString() + ","); //5
            sb.Append("ForexRemittingToPHP=" + calc.get_forex_target_to_php().ToString() + ","); //6
            sb.Append("ForexLocalToUSD=" + calc.get_forex_source_to_usd().ToString() + ","); //7
            sb.Append("ForexLocalToPHP=" + calc.get_forex_source_to_php().ToString() + ","); //8
            sb.Append("ForexRemittingToAccount=" + calc.get_forex_target_to_account().ToString() + ","); //9
            sb.Append("ForexRemittingToLocal=" + calc.get_forex_target_to_source().ToString() + ","); //10
            sb.Append("ForexRemittingToEUR=" + calc.get_forex_target_to_eur().ToString() + ","); //11
            sb.Append("LocalCurrency=" + calc.get_branch_local_currency().ToString() + ","); //12
            sb.Append("total_backend_charges=" + calc.get_total_backend_charges().ToString()); //13

            return sb.ToString();
        }

        private void getTranxSummary(string txn_seq_id)
        {

            this.tranx_summary = "";
            try
            {
                object[] param = new object[] { txn_seq_id };
                DataTable dt = execStoredProcedureTable("sp_txn_summary_uploads", param);

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    this.tranx_summary += dt.Columns[i].ColumnName + "~" + dt.Rows[0][i].ToString();
                    if (i != (dt.Columns.Count - 1))
                        this.tranx_summary += "|";
                }
            }
            catch
            {
            }
        }

        private void initializeInsertRemittance()
        {
            base.TableName = "TBL_REMITTANCE";
            base.getUpdateDS("SELECT * FROM " + base.TableName + " WHERE SEQ_ID = 0");
            base.IdentityColumn = "SEQ_ID";

            try
            {
                this._transaction_date = new irsInternetPage().getTransactionDate (this._branch_code, false);
            }
            catch
            {
            }
        }

        private bool insertRecord()
        {
            if (base.newRow())
            {
                return true;
            }
            return false;
        }

        private bool commitRecord()
        {
            if (base.save())
            {
                this._remit_seq_id = base.IdentityValue.ToString(); return true;
            }
            return false;
        }

        private bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
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

            return true;
        }

        private void computeCharges(int remit_mode, double payment, int acct_cur, int rate_type)
        {
            switch (remit_mode)
            {
                case 6:
                    this.computeCharges(acct_cur, payment, new irsCalculator (Convert.ToInt32(this._charge_id), acct_cur, this._branch_code, ChargeLocally, rate_type));
                    break;

                case 1:
                    this.computeCharges(acct_cur, payment, new irsCalculator (Convert.ToInt32(this._charge_id), acct_cur, this._branch_code, ChargeLocally, rate_type, 1, Convert.ToInt32(uprow["RemittingCurrency"])), 1);
                    break;

                default:
                    this.computeCharges(acct_cur, payment, new irsCalculator (Convert.ToInt32(this._charge_id), acct_cur, ChargeLocally));
                    break;
            }
        }

        private void computeCharges(int compute, int remit_mode, double payment, int acct_cur)
        {
            switch (remit_mode)
            {
                case 6:
                    this.computeCharges(acct_cur, payment, new irsCalculator (Convert.ToInt32(this._charge_id), acct_cur, this._branch_code, ChargeLocally));
                    break;

                case 1:
                    this.computeCharges(acct_cur, payment, new irsCalculator (Convert.ToInt32(this._charge_id), acct_cur, this._branch_code, compute, ChargeLocally), compute);
                    break;

                default:
                    this.computeCharges(acct_cur, payment, new irsCalculator(Convert.ToInt32(this._charge_id), acct_cur, ChargeLocally));
                    break;
            }
        }

        private void computeCharges(int remit_mode, double payment, int acct_cur)
        {
            switch (remit_mode)
            {
                case 6:
                    this.computeCharges(acct_cur, payment, new irsCalculator (Convert.ToInt32(this._charge_id), acct_cur, this._branch_code, ChargeLocally));
                    break;

                case 1:
                    this.computeCharges(acct_cur, payment, new irsCalculator(Convert.ToInt32(this._charge_id), acct_cur, this._branch_code, 1, ChargeLocally));
                    break;

                default:
                    this.computeCharges(acct_cur, payment, new irsCalculator(Convert.ToInt32(this._charge_id), acct_cur, ChargeLocally));
                    break;
            }
        }

        private void computeCharges(int acct_cur, double payment, irsCalculator calc)
        {
            irsCalculator rem_cal = (irsCalculator)calc;
            try
            {
                rem_cal.compute_transaction_charges_ord(1, Convert.ToDecimal(payment));
                // Corrected to remove warning
                // Note: DateTime is struct it will never have null value
                //uprow["TranxDate"] = (this._transaction_date == null) ? new irsInternetPage().getTransactionDate(uprow["RemittingBranch"].ToString(), false) : this._transaction_date;
                uprow["TranxDate"] = (this._transaction_date == DateTime.MinValue) ? new irsInternetPage().getTransactionDate(uprow["RemittingBranch"].ToString(), false) : this._transaction_date;
                uprow["ProductChargesID"] = this._charge_id;
                uprow["LocalCurrency"] = rem_cal.get_branch_local_currency();
                uprow["GrossRemAmount"] = rem_cal.get_remittance_gross_branch();
                uprow["NetRemAmount"] = rem_cal.get_remittance_net_remit_amt();
                uprow["NettoBeneCurrency"] = acct_cur;
                uprow["NettoBeneAmount"] = rem_cal.get_remittance_net_to_bene_amt();
                uprow["TotalLocalCharge"] = rem_cal.get_remittance_total_charges_amt();
                uprow["ForexRemittingToAccount"] = rem_cal.get_forex_remitting_to_account();
                uprow["ForexRemittingToLocal"] = rem_cal.get_forex_target_to_source();
                uprow["ForexRemittingToUSD"] = rem_cal.get_forex_target_to_usd();
                uprow["ForexRemittingToPHP"] = rem_cal.get_forex_target_to_php();
                uprow["ForexRemittingToEUR"] = rem_cal.get_forex_target_to_eur();
                uprow["ForexLocalToRemitting"] = rem_cal.get_forex_source_to_target();
                uprow["ForexLocalToUSD"] = rem_cal.get_forex_source_to_usd();
                uprow["ForexLocalToPHP"] = rem_cal.get_forex_source_to_php();
                uprow["total_backend_charges"] = rem_cal.get_total_backend_charges();
                uprow["total_benef_charges"] = rem_cal.get_total_benef_charges();
                uprow["ForexUSDtoPHP"] = 0;
            }
            catch (Exception e)
            {
                if (e.Message.ToUpper().Contains("DIVIDE") & e.Message.ToUpper().Contains("ZERO"))
                {
                    error_message = "No rates defined for the branch " + this._branch_code;
                }
            }
            this.rem_cal = (irsCalculator)rem_cal;
        }

        private void computeCharges(int acct_cur, double payment, irsCalculator  calc, int compute_method)
        {
            irsCalculator rem_cal = (irsCalculator)calc;
            try
            {
                rem_cal.compute_transaction_charges_ord(compute_method, Convert.ToDecimal(payment));

                uprow["TranxDate"] = new irsInternetPage().getTransactionDate (uprow["RemittingBranch"].ToString(), false);
                uprow["ProductChargesID"] = this._charge_id;
                uprow["LocalCurrency"] = rem_cal.get_branch_local_currency();
                uprow["GrossRemAmount"] = rem_cal.get_remittance_gross_branch();
                uprow["NetRemAmount"] = rem_cal.get_remittance_net_remit_amt();
                uprow["NettoBeneCurrency"] = acct_cur;
                uprow["NettoBeneAmount"] = rem_cal.get_remittance_net_to_bene_amt();
                uprow["TotalLocalCharge"] = rem_cal.get_remittance_total_charges_amt();
                uprow["ForexRemittingToAccount"] = rem_cal.get_forex_remitting_to_account();
                uprow["ForexRemittingToLocal"] = rem_cal.get_forex_target_to_source();
                uprow["ForexRemittingToUSD"] = rem_cal.get_forex_target_to_usd();
                uprow["ForexRemittingToPHP"] = rem_cal.get_forex_target_to_php();
                uprow["ForexRemittingToEUR"] = rem_cal.get_forex_target_to_eur();
                uprow["ForexLocalToRemitting"] = rem_cal.get_forex_source_to_target();
                uprow["ForexLocalToUSD"] = rem_cal.get_forex_source_to_usd();
                uprow["ForexLocalToPHP"] = rem_cal.get_forex_source_to_php();
                uprow["total_backend_charges"] = rem_cal.get_total_backend_charges();
                uprow["total_benef_charges"] = rem_cal.get_total_benef_charges();
                uprow["ForexUSDtoPHP"] = 0;
            }
            catch (Exception e)
            {
                if (e.Message.ToUpper().Contains("DIVIDE") & e.Message.ToUpper().Contains("ZERO"))
                    throw new Exception("Rates not defined. Account Currency: " + acct_cur
                                        + ", Remitting Currency: " + uprow["RemittingCurrency"].ToString()
                                        + ", Branch: " + uprow["RemittingBranch"].ToString());
                else
                    throw e;
            }
            this.rem_cal = (irsCalculator)rem_cal;
        }
        /// <summary>
        /// added for IR IRSW012820140003- alrajhi reference numbers 
        /// </summary>
        /// <param name="tranxdate"></param>
        /// <param name="PON_num"></param>
        /// <returns>seq id of remittance table</returns>
        private string get_transaction_id(DateTime tranxdate, string PON_num)
        {
            
            try
            {
                object[] param = new object[] { tranxdate, PON_num };
                DataTable dt = execStoredProcedureTable("proc_check_valid_alrajhiremittance", param);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["seq_id"].ToString();
                    }
                    else
                        return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return "";

        }
        /// <summary>
        /// get file inventory seq id
        /// </summary>
        /// <param name="agentreference"> reference/PON of al rajhi</param>
        /// <returns></returns>
        private string get_file_inventory_id (string agentreference)
        {
            try
            {
                object[] param = new object[] { "A06", agentreference };
                DataTable dt = execStoredProcedureTable("proc_get_file_inventoryid", param);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["seq_id"].ToString();
                    }
                    else
                        return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return "";
        }
        private string get_file_filename(string agentreference)
        {
            try
            {
                object[] param = new object[] { "A06", agentreference };
                DataTable dt = execStoredProcedureTable("proc_get_file_inventoryid", param);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["FileName"].ToString();
                    }
                    else
                        return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return "";
        }

        private string get_branch_seq_id ()
        {
            try
            {
                object[] param = new object[] { "A06"};
                DataTable dt = execStoredProcedureTable("proc_get_branch_seqid", param);

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["seq_id"].ToString();
                    }
                    else
                        return "";
                }
            }
            catch (Exception e)
            {
                return "";
            }
            return "";
        }

         


        #endregion

        #region Constructor
        public irsEXCORemittance()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                this._branch_code = loggin_data.Location;
                this._user_id = loggin_data.UserId;
            }
            ChargeLocally = false;
            this.initializeInsertRemittance();
        }

        public irsEXCORemittance(bool charge_locally)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                this._branch_code = loggin_data.Location;
                this._user_id = loggin_data.UserId;
            }
            ChargeLocally = charge_locally;
            this.initializeInsertRemittance();
        }

        public irsEXCORemittance(string br_code, string user_id)
        {
            this._user_id = user_id;
            this._branch_code = br_code;
            ChargeLocally = false;
            this.initializeInsertRemittance();
        }

        public irsEXCORemittance(string br_code, string user_id, bool charge_locally)
        {
            this._user_id = user_id;
            this._branch_code = br_code;
            ChargeLocally = charge_locally;
            this.initializeInsertRemittance();
        }
        #endregion
    }
}
