using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsProductSetup : sqlHelper
    {
        #region PRIVATE VARIABLE
        private Hashtable _ht = new Hashtable();
        string _criteria = String.Empty;
        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
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
        #endregion  


        ////public irsProductSetup(Hashtable ht, string pTableName, string criteria)
        //public irsProductSetup()
        //{
        //    this._ht = ht;
        //    this._criteria = criteria;
        //    this.TableName = pTableName;
        //    this.IdentityColumn = "SEQ_ID";
        //}
        public irsProductSetup()
		{            
			this.TableName      = "tbl_prodimplem_setup";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM tbl_prodimplem_setup ", false);
            getUpdateDataSet(command);
        }
        public DataTable getProductSetup()
        {
            base.getUpdateDataSet("select * from "+this.TableName+" "+this._criteria+"");
            if (isUpdatable)
            {
                return base.table;
            }
            return null;
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
