using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
    public class irsMachineSetup : sqlHelperWs
    {

        #region PRIVATE VARIABLE
        
        private DataSet data_set         = null;
        private string error_message     = null;
        private string _seq_id           = null;
        private string _machine_name     = null;
        private string _mac_address      = null;
        private string _branch_code      = null;
        private string _touch_screen     = null;
        private string _card_reader      = null;
        private string _cash_dispenser   = null;
        private string _default_currency = null;
        private string _isdeleted        = null;

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

        public string machine_name
        {
            get { return this._machine_name; }
            set { _machine_name = value; }
        }

        public string mac_address
        {
            get { return this._mac_address; }
            set { _mac_address = value; }
        }

        public string branch_code
        {
            get { return this._branch_code; }
            set { _branch_code = value; }
        }

        public string touch_screen
        {
            get { return this._touch_screen; }
            set { _touch_screen = value; }
        }

        public string card_reader
        {
            get { return this._card_reader; }
            set { _card_reader = value; }
        }

        public string cash_dispenser
        {
            get { return this._cash_dispenser; }
            set { _cash_dispenser = value; }
        }

        public string default_currency
        {
            get { return this._default_currency; }
            set { _default_currency = value; }
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
                _machine_name = this.getRecord(base.getColumn("MachineName"));
                _mac_address = this.getRecord(base.getColumn("MacAddress"));
                _branch_code = this.getRecord(base.getColumn("BranchCode"));
                _touch_screen = this.getRecord(base.getColumn("TouchScreen"));
                _card_reader = this.getRecord(base.getColumn("CardReader"));
                _cash_dispenser = this.getRecord(base.getColumn("CashDispenser"));
                _default_currency = this.getRecord(base.getColumn("DefaultCurrency"));
                _isdeleted = this.getRecord(base.getColumn("isDeleted"));
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

        public irsMachineSetup()
		{            
			this.TableName      = "TBL_MACHINES";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_MACHINES WHERE SEQ_ID = " + id, false);
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
