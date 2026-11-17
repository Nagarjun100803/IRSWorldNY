using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsException : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set      = null;
        private string error_message  = null;
        private string _seq_id        = null;
        private string _cif_id        = null;
        private string _remitter_name = null;
        private string _last_name = null;
        private string _first_name = null;
        private string _middle_name = null;
        private string _branch_code = null;
        private string _e_reason      = null;
        private string _c_reason      = null;
        private string _date_created  = null;
        private string _date_modified = null;
        private string _status        = null;
        private string _remarks       = null;

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

        public string cif_id
        {
            get { return this._cif_id; }
            set { _cif_id = value; }
        }

        public string remitter_name
        {
            get { return this._remitter_name; }
            set { _remitter_name = value; }
        }

        public string last_name
        {
            get { return this._last_name; }
            set { _last_name = value; }
        }

        public string first_name
        {
            get { return this._first_name; }
            set { _first_name = value; }
        }

        public string middle_name
        {
            get { return this._middle_name; }
            set { _middle_name = value; }
        }

        public string branch_code
        {
            get { return this._branch_code; }
            set { _branch_code = value; }
        }

        public string e_reason
        {
            get { return this._e_reason; }
            set { _e_reason = value; }
        }

        public string c_reason
        {
            get { return this._c_reason; }
            set { _c_reason = value; }
        }

        public string date_created
        {
            get { return this._date_created; }
            set { _date_created = value; }
        }

        public string date_modified
        {
            get { return this._date_modified; }
            set { _date_modified = value; }
        }

        public string status
        {
            get { return this._status; }
            set { _status = value; }
        }

        public string remarks
        {
            get { return this._remarks; }
            set { _remarks = value; }
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
                _cif_id = this.getRecord(base.getColumn("CIFID"));
                _remitter_name = this.getRecord(base.getColumn("RemitterName"));
                _branch_code = this.getRecord(base.getColumn("BranchCode"));
                _last_name = this.getRecord(base.getColumn("LastName"));
                _first_name = this.getRecord(base.getColumn("FirstName"));
                _middle_name = this.getRecord(base.getColumn("MiddleName"));
                _e_reason = this.getRecord(base.getColumn("EReason"));
                _c_reason = this.getRecord(base.getColumn("CReason"));
                _date_created = this.getRecord(base.getColumn("DateCreated"));
                _date_modified = this.getRecord(base.getColumn("DateModified"));
                _status = this.getRecord(base.getColumn("Status"));
                _remarks = this.getRecord(base.getColumn("Remarks"));

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

        public irsException()
		{            
			this.TableName      = "TBL_EXCEPTIONS";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_EXCEPTIONS WHERE SEQ_ID = " + id, false);
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
