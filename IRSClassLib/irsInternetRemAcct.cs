using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsInternetRemAcct : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _accountno = null;
        private string _branchcode = null;
        //private string _branchtype = null;
        private string _created_by = null;
        private string _remitter_cifid = null;
        private string _accounttype = null;
        private string _currency = null;
        private string _accountstatus = null;
        #endregion

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }

        public string created_by
        {
            get { return this._created_by; }
            set { _created_by = value; }
        }

        public string accountno
        {
            get { return this._accountno; }
            set { _accountno = value; }
        }

        public string branchcode
        {
            get { return this._branchcode; }
            set { _branchcode = value; }
        }

        public string remitter_cifid
        {
            get { return this._remitter_cifid; }
            set { _remitter_cifid = value; }
        }

        public string accounttype
        {
            get { return this._accounttype; }
            set { _accounttype = value; }
        }

        public string currency
        {
            get { return this._currency; }
            set { _currency = value; }
        }


        public string accountstatus
        {
            get { return this._accountstatus; }
            set { _accountstatus = value; }
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
                _created_by = this.getRecord(base.getColumn("CreatedBy"));
                _accountno = this.getRecord(base.getColumn("AccountNo"));
                _branchcode = this.getRecord(base.getColumn("BranchCode"));
                _remitter_cifid = this.getRecord(base.getColumn("RemitterCIFID"));
                _currency = this.getRecord(base.getColumn("Currency"));
                _accounttype = this.getRecord(base.getColumn("AccountType"));
                _accountstatus = this.getRecord(base.getColumn("AccountStatus"));

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

        public irsInternetRemAcct()
        {
            this.TableName = "TBL_INTERNET_REMITTER_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_INTERNET_REMITTER_ACCOUNTS WHERE SEQ_ID = " + id, false);
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
