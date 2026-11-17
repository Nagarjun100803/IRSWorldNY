using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsInternetRemProducts : sqlHelperWs
	{
		#region PRIVATE VARIABLE

		private DataSet data_set		= null;
		private string error_message	= null;
		private string _seq_id			= null;
		private string _remitter_cifid	= null;
		private string _bene_cifid		= null;
		private string _product_type	= null;
		private string _target_curr		= null;
		private string _account_no		= null;
		private string _other_bank_name = null;
		private string _branch_code		= null;
		private string _bank_name		= null;
		private string _dstn_country	= null;
		private string _account_type	= null;
		private string _account_curr	= null;
		private string _gfmc_no			= null;
		private string _gfmc_curr_code	= null;

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

		public string remitter_cifid
		{
			get { return this._remitter_cifid; }
			set { _remitter_cifid = value; }
		}

		public string bene_cifid
		{
			get { return this._bene_cifid; }
			set { _bene_cifid = value; }
		}

		public string product_type
		{
			get { return this._product_type; }
			set { _product_type = value; }
		}
		
		public string target_curr
		{
			get { return this._target_curr; }
			set { _target_curr = value; }
		}

		public string account_no
		{
			get { return this._account_no; }
			set { _account_no = value; }
		}
		
		public string other_bank_name
		{
			get { return this._other_bank_name; }
			set { _other_bank_name = value; }
		}

		public string branch_code
		{
			get { return this._branch_code; }
			set { _branch_code = value; }
		}

		public string bank_name
		{
			get { return this._bank_name; }
			set { _bank_name = value; }
		}

		public string dstn_country
		{
			get { return this._dstn_country; }
			set { _dstn_country = value; }
		}

		public string account_type
		{
			get { return this._account_type; }
			set { _account_type = value; }
		}

		public string account_curr
		{
			get { return this._account_curr; }
			set { _account_curr = value; }
		}
		
		public string gfmc_no
		{
			get { return this._gfmc_no; }
			set { _gfmc_no = value; }
		}

		public string gfmc_curr_code
		{
			get { return this._gfmc_curr_code; }
			set { _gfmc_curr_code = value; }
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

                _seq_id		     = this.getRecord(base.getColumn("SEQ_ID"));
                _remitter_cifid  = this.getRecord(base.getColumn("RemitterCIFID"));
                _bene_cifid		 = this.getRecord(base.getColumn("BeneCIFID"));
                _product_type	 = this.getRecord(base.getColumn("ProductType"));
                _target_curr	 = this.getRecord(base.getColumn("TargetCurrency"));
                _account_no		 = this.getRecord(base.getColumn("AccountNo"));
				_other_bank_name = this.getRecord(base.getColumn("OtherBankName"));
				_branch_code	 = this.getRecord(base.getColumn("BranchCode"));
				_bank_name		 = this.getRecord(base.getColumn("BankName"));
				_dstn_country	 = this.getRecord(base.getColumn("DstnCountry"));
				_account_type	 = this.getRecord(base.getColumn("AccountType"));
				_account_curr	 = this.getRecord(base.getColumn("AccountCurrency"));
				_gfmc_no		 = this.getRecord(base.getColumn("GFMCNo"));
				_gfmc_curr_code  = this.getRecord(base.getColumn("GFMCCurrCode"));

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

        public irsInternetRemProducts()
        {
            this.TableName = "TBL_INTERNET_REMITTER_PRODUCTS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
			SqlCommand command = getSqlCommand("SELECT * FROM TBL_INTERNET_REMITTER_PRODUCTS WHERE SEQ_ID = " + id, false);
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
