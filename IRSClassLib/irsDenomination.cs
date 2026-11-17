using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsDenomination : sqlHelper
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _txn_seq_id = null;
        private string _denom_code = null;
        private string _denom_value = null;

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

        public string txn_seq_id
        {
            get { return this._txn_seq_id; }
            set { _txn_seq_id = value; }
        }

        public string denom_code
        {
            get { return this._denom_code; }
            set { _denom_code = value; }
        }

        public string denom_value  
        {
            get { return this._denom_value; }
            set { _denom_value = value; }
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

                _seq_id         = this.getRecord(base.getColumn("SEQ_ID"));
                _txn_seq_id     = this.getRecord(base.getColumn("Txn_SeqId"));
                _denom_code     = this.getRecord(base.getColumn("Denom_Code"));
                _denom_value    = this.getRecord(base.getColumn("Denom_Value"));
                


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

        public irsDenomination()
		{
            this.TableName = "TBL_REMITTANCE_CM_BREAK_DOWN";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_CM_BREAK_DOWN WHERE Txn_SeqId = " + id, false);
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
