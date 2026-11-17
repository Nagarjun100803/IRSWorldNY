using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientCards : sqlHelperWs
	{
		#region PRIVATE VARIABLES

		private DataSet data_set		= null;
		private string error_message	= null;

		private string _seq_id			= null;
		private string _cifid			= null;
		private string _card_type		= null;
		private string _card_no			= null;
		private string _card_name		= null;
		private string _issue_date		= null;
		private string _expiry_date		= null;
		private string _bank_code		= null;
		private string _card_class		= null;
		private string _issue_number	= null;
		private string _remarks			= null;
		private string _scanned_card	= null;
		private string _country			= null;
		private string _currency		= null;
		private string _gfmc_acct_seqid = null;
		private string _is_arm		    = null;
		public bool		ins_flag		= false;

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

		public string cifid
		{
			get { return this._cifid; }
			set { _cifid = value; }
		}

		public string card_type
		{
			get { return this._card_type; }
			set { _card_type = value; }
		}

		public string card_no
		{
			get { return this._card_no; }
			set { _card_no = value; }
		}

		public string card_name
		{
			get { return this._card_name; }
			set { _card_name = value; }
		}

		public string issue_date
		{
			get { return this._issue_date; }
			set { _issue_date = value; }
		}

		public string expiry_date
		{
			get { return this._expiry_date; }
			set { _expiry_date = value; }
		}

		public string bank_code
		{
			get { return this._bank_code; }
			set { _bank_code = value; }
		}

		public string card_class
		{
			get { return this._card_class; }
			set { _card_class = value; }
		}

		public string issue_number
		{
			get { return this._issue_number; }
			set { _issue_number = value; }
		}

		public string remarks
		{
			get { return this._remarks; }
			set { _remarks = value; }
		}

		public string scanned_card
		{
			get { return this._scanned_card; }
			set { _scanned_card = value; }
		}

		public string country
		{
			get { return this._country; }
			set { _country = value; }
		}

		public string currency
		{
			get { return this._currency; }
			set { _currency = value; }
		}

		public string gfmc_acct_seqid
		{
			get { return this._gfmc_acct_seqid; }
			set { _gfmc_acct_seqid = value; }
		}

		public string is_arm
		{
			get { return this._is_arm; }
			set { _is_arm = value; }
		}
		#endregion


		#region PUBLIC METHOD

        public DataTable getClientCard(String cifid, String cardname, String cardno)
        {
            String sql = "SELECT * FROM " + TableName + " WHERE CIFID = '" + cifid + "' AND CardNo = '" + cardno + "' AND (CardName = 'GFMC' OR CardName = 'GFC')";
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];

            return null;
        }

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

                _seq_id			= this.getRecord(base.getColumn("SEQ_ID"));
                _cifid			= this.getRecord(base.getColumn("CIFID"));
                _card_type		= this.getRecord(base.getColumn("CardType"));
                _card_no		= this.getRecord(base.getColumn("CardNo"));
                _card_name		= this.getRecord(base.getColumn("CardName"));
                _issue_date		= this.getRecord(base.getColumn("IssueDate"));
				_expiry_date	= this.getRecord(base.getColumn("ExpiryDate"));
                _bank_code		= this.getRecord(base.getColumn("BankCode"));
                _card_class		= this.getRecord(base.getColumn("CardClass"));
                _issue_number   = this.getRecord(base.getColumn("IssueNumber"));
                _remarks		= this.getRecord(base.getColumn("Remarks"));
				_scanned_card	= this.getRecord(base.getColumn("ScannedCard"));
				_country		= this.getRecord(base.getColumn("Country"));
				_currency		= this.getRecord(base.getColumn("Currency"));
				_gfmc_acct_seqid= this.getRecord(base.getColumn("GFMCAcctSeqId"));
				_is_arm			= this.getRecord(base.getColumn("IsArm")); 

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

		public irsClientCards()
		{            
			this.TableName      = "TBL_CIF_CARDS";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

		public override void getUpdateDS(int id)
		{
			SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_CARDS WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(command);
		}

		public bool insert_record()
		{
			ins_flag = true;

			if (base.newRow() == true)
			{
				return true;
			}
			this.get_error_dbase(); return false;
		}

		public bool update_record()
		{
			ins_flag = false;

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

		public bool is_gfc(string cardno)
		{
			sqlHelperWs commons = new sqlHelperWs();
			
			object x = commons.execScalar("sp_cif_isCard_gfc", new string[] { cardno });
			return Convert.ToBoolean(x);
		}

		public bool is_existing(string param_values)
		{
			string card = this.count_record(param_values);

			String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE CIFID = '" + ((card.Split('&'))[0]).Trim() + "' AND CardNo = '" + ((card.Split('&'))[1]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";
			
			return (Convert.ToInt32(execScalar(sql)) > 0);
		}

		public string count_record(string param_values)
		{
			string[] cols = param_values.Split('|');
			string card_cifid = null;
			string cardno = null;
			
			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				if (vals[0].ToUpper() == "CIFID") card_cifid = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "CARDNO") cardno = (cols[i].Split('~'))[1];
			}

			return card_cifid + '&' + cardno;
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
