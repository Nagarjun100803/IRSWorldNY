using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsGRSAccountingEntries : sqlHelperWs 
    {

    #region PRIVATE VARIABLE
        private DataSet data_set = null;
        private string error_message = null;
        private string _SEQ_ID              = null;
        private string _TranxCcyCode        = null;
        private string _AcctCcyCode         = null;
        private string _ImplementationType  = null;
        private string _GLAcctCode          = null;
        private string _GLDesc              = null;
        private string _DebitCreditTag      = null;
        private string _TranxSource         = null;
        private string _RecordID            = null;
        private string _NSVS                = null;
        private string _FCCFCR              = null;
        private string _FCDURBU             = null;
        private string _HandlingType        = null;
    # endregion

    #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._SEQ_ID; }
            set { _SEQ_ID = value; }
        }

    #endregion

    #region PUBLIC METHOD
        public irsGRSAccountingEntries()
        {
            this.TableName = "TBL_GRS_ACCOUNTINGENTRIES";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
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

        public DataSet getDataRecord()
        {
            if (this.dataset != null)
            {
                return this.data_set;
            }
            return null;
        }

        public bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] != null || s[1].Trim().Length > 0)
                    {
                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
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

        public bool insert_record()
        {
            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase();
            return false;
        }

        public bool commit_record()
        {
            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool delete_record()
        {
            base.selectedRow = 0;
            if (base.Delete(sqlHelper.RETAIN_DATASET) == true)
            {
                return true;
            }

            this.get_error_dbase(); return false;
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
                _SEQ_ID             = this.getRecord(base.getColumn("SEQ_ID"));
                _TranxCcyCode       = this.getRecord(base.getColumn("TranxCcyCode"));
                _AcctCcyCode        = this.getRecord(base.getColumn("AcctCcyCode"));
                _ImplementationType = this.getRecord(base.getColumn("ImplementationType"));
                _GLAcctCode         = this.getRecord(base.getColumn("GLAcctCode"));
                _GLDesc             = this.getRecord(base.getColumn("GLDesc"));
                _DebitCreditTag     = this.getRecord(base.getColumn("DebitCreditTag"));
                _TranxSource        = this.getRecord(base.getColumn("TranxSource"));
                _RecordID           = this.getRecord(base.getColumn("RecordID"));
                _NSVS               = this.getRecord(base.getColumn("NSVS"));
                _FCCFCR             = this.getRecord(base.getColumn("FCCFCR"));
                _FCDURBU            = this.getRecord(base.getColumn("FCDURBU"));
                _HandlingType       = this.getRecord(base.getColumn("HandlingType"));  

                this.data_set = dataset;
                return true;
            }
            return false;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand sqlCmd_accountingentries = getSqlCommand("SELECT * FROM TBL_GRS_ACCOUNTINGENTRIES WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(sqlCmd_accountingentries);
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
