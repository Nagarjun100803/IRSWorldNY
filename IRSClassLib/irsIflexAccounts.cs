using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace IRSClassLib
{
    public class irsIflexAccounts : sqlHelperWs
    {
    #region PRIVATE VARIABLE

        private DataSet data_set        = null;
        private string error_message    = null;

        private string _SEQ_ID          = null;
        private string _IflexAccount    = null;
        private string _Description     = null;
        private string _GLType          = null;
    #endregion

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

        public irsIflexAccounts()
        {
            this.TableName = "TBL_IFLEXACCOUNTS";
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

                _SEQ_ID = this.getRecord(base.getColumn("SEQ_ID"));
                _IflexAccount = this.getRecord(base.getColumn("IflexAccount"));
                _Description = this.getRecord(base.getColumn("Description"));
                _GLType = this.getRecord(base.getColumn("GLType"));

                this.data_set = dataset;
                return true;
            }
            return false;
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

        public override void getUpdateDS(int id)
        {
            SqlCommand sqlCmd_accountingentries = getSqlCommand("SELECT * FROM TBL_IFLEXACCOUNTS WHERE SEQ_ID = " + id, false);
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
