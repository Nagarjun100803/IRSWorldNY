using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsInternetTransaction : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _user_id = null;
        private string _rem_method = null;
        private string _rem_station_id = null;
        private string _rem_branch = null;
        private string _ref_no = null;
        private string _ext_ref_no = null;
        private string _tran_status = null;
        private string _tran_date = null;
        private string _prod_charges_id = null;
        private string _rem_cifid = null;
        private string _bene_cifid = null;
        private string _acc_seqid = null;
        private string _rem_currency = null;
        private string _gross_rem_amount = null;
        private string _net_rem_amount = null;
        private string _net_to_bene_currency = null;
        private string _net_to_bene_amount = null;
        private string _total_local_charge = null;
        private string _tran_source = null;
        private string _receipt_mode = null;
        private string _tran_purpose = null;
        private string _other_purpose = null;
        private string _sp_message = null;
        private string _date_imp = null;
        private string _date_sent = null;
        private string _printed = null;
        private string _forsource_to_target = null;
        private string _fortarget_to_usd = null;
        private string _fortarget_to_php = null;
        private string _forsource_to_usd = null;
        private string _forsource_to_php = null;
        private string _fortarget_to_acc = null;
        private string _source_of_fund = null;

        #endregion

        #region PROPERTY GET & SET

        public string message
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }

        public string user_id
        {
            get { return this._user_id; }
            set { _user_id = value; }
        }

        public string rem_method
        {
            get { return this._rem_method; }
            set { _rem_method = value; }
        }

        public string rem_station_id
        {
            get { return this._rem_station_id; }
            set { _rem_station_id = value; }
        }

        public string rem_branch
        {
            get { return this._rem_branch; }
            set { _rem_branch = value; }
        }

        public string ref_no
        {
            get { return this._ref_no; }
            set { _ref_no = value; }
        }


        public string ext_ref_no
        {
            get { return this._ext_ref_no; }
            set { _ext_ref_no = value; }
        }

        public string tran_status
        {
            get { return this._tran_status; }
            set { _tran_status = value; }
        }

        public string tran_date
        {
            get { return this._tran_date; }
            set { _tran_date = value; }
        }

        public string prod_charges_id
        {
            get { return this._prod_charges_id; }
            set { _prod_charges_id = value; }
        }

        public string rem_cifid
        {
            get { return this._rem_cifid; }
            set { _rem_cifid = value; }
        }

        public string bene_cifid
        {
            get { return this._bene_cifid; }
            set { _bene_cifid = value; }
        }

        public string acc_seqid
        {
            get { return this._acc_seqid; }
            set { _acc_seqid = value; }
        }

        public string rem_currency
        {
            get { return this._rem_currency; }
            set { _rem_currency = value; }
        }

        public string gross_rem_amount
        {
            get { return this._gross_rem_amount; }
            set { _gross_rem_amount = value; }
        }

        public string net_rem_amount
        {
            get { return this._net_rem_amount; }
            set { _net_rem_amount = value; }
        }

        public string net_to_bene_currency
        {
            get { return this._net_to_bene_currency; }
            set { _net_to_bene_currency = value; }
        }
        
        public string net_to_bene_amount
        {
            get { return this._net_to_bene_amount; }
            set { _net_to_bene_amount = value; }
        }

        public string total_local_charge
        {
            get { return this._total_local_charge; }
            set { _total_local_charge = value; }
        }

        public string tran_source
        {
            get { return this._tran_source; }
            set { _tran_source = value; }
        }

        public string receipt_mode
        {
            get { return this._receipt_mode; }
            set { _receipt_mode = value; }
        }

        public string tran_purpose
        {
            get { return this._tran_purpose; }
            set { _tran_purpose = value; }
        }

        public string other_purpose
        {
            get { return this._other_purpose; }
            set { _other_purpose = value; }
        }

        public string sp_message
        {
            get { return this.sp_message; }
            set { sp_message = value; }
        }

        public string date_imp
        {
            get { return this._date_imp; }
            set { _date_imp = value; }
        }

        public string date_sent
        {
            get { return this._date_sent; }
            set { _date_sent = value; }
        }

        public string printed
        {
            get { return this._printed; }
            set { _printed = value; }
        }

        public string forsource_to_target
        {
            get { return this._forsource_to_target; }
            set { _forsource_to_target = value; }
        }

        public string fortarget_to_usd
        {
            get { return this._fortarget_to_usd; }
            set { _fortarget_to_usd = value; }
        }

        public string fortarget_to_php
        {
            get { return this._fortarget_to_php; }
            set { _fortarget_to_php = value; }
        }

        public string forsource_to_usd
        {
            get { return this._forsource_to_usd; }
            set { _forsource_to_usd = value; }
        }

        public string forsource_to_php
        {
            get { return this._forsource_to_php; }
            set { _forsource_to_php = value; }
        }

        public string fortarget_to_acc
        {
            get { return this._fortarget_to_acc; }
            set { _fortarget_to_acc = value; }
        }

        public string source_of_fund
        {
            get { return this._source_of_fund; }
            set { _source_of_fund = value; }
        }


        #endregion

        #region PUBLIC METHOD

        public DataSet getdataRecord()
        {
            if (this.dataset != null)
            {
                return this.data_set;
            }

            return null;
        }

        public string getRecord(object obj)
        {
            if (obj != null)
            {
                string s = Convert.ToString(obj);

                if (s != null && s.Trim() == string.Empty)
                {
                    return null;
                }
                return s.Replace("'", "''");
            }

            return null;
        }

        public bool get_record(string SEQ_ID)
        {
            base.IdentityValue = SEQ_ID; return get_record();
        }

        public bool get_record()
        {
            this.getUpdateDS(Convert.ToInt32(base.IdentityValue));

            if (dataset != null && dataset.Tables[0].Rows.Count > 0)
            {
                base.selectedRow = 0;

                _seq_id = this.getRecord(base.getColumn("SEQ_ID"));
                _user_id = this.getRecord(base.getColumn("UserID"));
                _rem_method = this.getRecord(base.getColumn("RemitMethod"));
                _rem_station_id = this.getRecord(base.getColumn("RemitStationID"));
                _rem_branch = this.getRecord(base.getColumn("RemittingBranch"));
                _ref_no = this.getRecord(base.getColumn("ReferenceNo"));
                _ext_ref_no = this.getRecord(base.getColumn("ExtlReferenceNo"));
                _tran_status = this.getRecord(base.getColumn("Transtatus"));
                _tran_date = this.getRecord(base.getColumn("TranxDate"));
                _prod_charges_id = this.getRecord(base.getColumn("ProductChargesID"));
                _rem_cifid = this.getRecord(base.getColumn("RemitterCIFID"));
                _bene_cifid = this.getRecord(base.getColumn("BeneficiaryCIFID"));
                _acc_seqid = this.getRecord(base.getColumn("AccountSEQID"));
                _rem_currency = this.getRecord(base.getColumn("RemittingCurrency"));
                _gross_rem_amount = this.getRecord(base.getColumn("GrossRemAmount"));
                _net_rem_amount = this.getRecord(base.getColumn("NetRemAmount"));
                _net_to_bene_currency = this.getRecord(base.getColumn("NettoBeneCurrency"));
                _net_to_bene_amount = this.getRecord(base.getColumn("NettoBeneAmount"));
                _total_local_charge = this.getRecord(base.getColumn("TotalLocalCharge"));
                _tran_source = this.getRecord(base.getColumn("TransactionSource"));
                _receipt_mode = this.getRecord(base.getColumn("ReceiptMode"));
                _tran_purpose = this.getRecord(base.getColumn("TransactionPurpose"));
                _other_purpose = this.getRecord(base.getColumn("OtherPurpose"));
                _sp_message = this.getRecord(base.getColumn("SpecialMessage"));
                _date_imp = this.getRecord(base.getColumn("DateImplemented"));
                _date_sent = this.getRecord(base.getColumn("DateSent"));
                _printed = this.getRecord(base.getColumn("printed"));
                _forsource_to_target = this.getRecord(base.getColumn("ForexSourceToTarget"));
                _fortarget_to_usd = this.getRecord(base.getColumn("ForexTargetToUSD"));
                _fortarget_to_php = this.getRecord(base.getColumn("ForexTargetToPHP"));
                _forsource_to_usd = this.getRecord(base.getColumn("ForexSourceToUSD"));
                _forsource_to_php = this.getRecord(base.getColumn("ForexSourceToPHP"));
                _fortarget_to_acc = this.getRecord(base.getColumn("ForexTargetToAccount"));
                _source_of_fund = this.getRecord(base.getColumn("SourceofFunds"));

                this.data_set = dataset; return true;
            }
            return false;
        }

        public bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                {
                    s[1] = null;
                }

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

                this.error_message = string.Empty;
                this.error_message = sb.ToString();

                return false;
            }

            return true;
        }

        public irsInternetTransaction()
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(command);
        }

        public bool insert_record()
        {
            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool update_record()
        {
            base.IdentityValue = this.seq_id;

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool commit_record()
        {
            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }
            this.get_error_dbase(); return false;
        }

        public void rollback_record()
        {
            base.cancel();
        }

        #endregion

        #region PRIVATE METHOD



        private void get_error_dbase()
        {
            this.error_message = string.Empty;

            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }
        #endregion
    }
}
