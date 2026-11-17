using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsInternetRemCards : sqlHelperWs
	{
		#region PRIVATE VARIABLE

		private DataSet data_set	  = null;
		private string error_message  = null;
		private string _seq_id		  = null;
		private string _cardno		  = null;
		private string _cardname	  = null;
		private string _expirydate	  = null;
		private string _bankcode	  = null;
		private string _created_by	  = null;
		private string _remitter_cifid		  = null;
		private string _issuedate	  = null;
		private string _cardtype	  = null;
		private string _cardclass	  = null;
		private string _country		  = null;
		private string _currency	  = null;
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

		public string cardno
		{
			get { return this._cardno; }
			set { _cardno = value; }
		}

		public string cardname
		{
			get { return this._cardname; }
			set { _cardname = value; }
		}

		public string expirydate
		{
			get { return this._expirydate; }
			set { _expirydate = value; }
		}

		public string bankcode
		{
			get { return this._bankcode; }
			set { _bankcode = value; }
		}

		public string remitter_cifid
		{
			get { return this._remitter_cifid; }
			set { _remitter_cifid = value; }
		}

		public string issuedate
		{
			get { return this._issuedate; }
			set { _issuedate = value; }
		}

		public string cardtype
		{
			get { return this._cardtype; }
			set { _cardtype = value; }
		}

		public string cardclass
		{
			get { return this._cardclass; }
			set { _cardclass = value; }
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

                _seq_id		   = this.getRecord(base.getColumn("SEQ_ID"));
                _created_by    = this.getRecord(base.getColumn("CreatedBy"));
                _cardno		   = this.getRecord(base.getColumn("CardNo"));
                _cardname	   = this.getRecord(base.getColumn("CardName"));
                _expirydate	   = this.getRecord(base.getColumn("ExpiryDate"));
                _bankcode	   = this.getRecord(base.getColumn("BankCode"));
				_remitter_cifid= this.getRecord(base.getColumn("RemitterCIFID"));
				_issuedate	   = this.getRecord(base.getColumn("IssueDate"));
				_currency	   = this.getRecord(base.getColumn("Currency"));
				_country	   = this.getRecord(base.getColumn("Country"));
				_cardtype	   = this.getRecord(base.getColumn("CardType"));
				_cardclass	   = this.getRecord(base.getColumn("CardClass"));

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

        public irsInternetRemCards()
        {
            this.TableName = "TBL_INTERNET_REMITTER_CARDS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_INTERNET_REMITTER_CARDS WHERE SEQ_ID = " + id, false);
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
