using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
    [Serializable]
    public class irsFaultlogComment : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _issue_reference = null;
        private string _date_posted = null;
        private string _user_id= null;
        private string _comment = null;

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
        public string issue_reference
        {
            get { return this._issue_reference; }
            set { _issue_reference = value; }
        }

        public string date_posted
        {
            get { return this.date_posted; }
            set { _date_posted = value; }
        }

        public string user_id
        {
            get { return this._user_id; }
            set { _user_id = value; }
        }

        public string comment
        {
            get { return this._comment; }
            set { _comment = value; }
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
                return s.Replace("'","''");
            }
            
            return null;
        }




        public bool get_record(string seq_id)
        {
            base.IdentityValue = seq_id; return get_record();                
        }

        public bool get_record()
		{           
            this.getUpdateDS(Convert.ToInt32(base.IdentityValue));            

            if (dataset != null && dataset.Tables[0].Rows.Count > 0)
            {
                base.selectedRow = 0;

                _seq_id             = this.getRecord(base.getColumn("SEQ_ID"));
                _issue_reference    = this.getRecord(base.getColumn("IssueReference"));
                _date_posted        = this.getRecord(base.getColumn("DatePosted"));
                _user_id            = this.getRecord(base.getColumn("UserId"));
                _comment            = this.getRecord(base.getColumn("Comment"));

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

        public irsFaultlogComment()
		{
            this.TableName = "TBL_FAULTLOG_COMMENT";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_FAULTLOG_COMMENT WHERE SEQ_ID = " + id, false);
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
