using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
	[Serializable]
	public class irsInternetRem : sqlHelperWs
    {

        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _title = null;
		private string _cifclass = null;
        private string _lastname = null;
        private string _firstname = null;
        private string _middlename = null;
        private string _alias = null;
        private string _gender = null;
        private string _age = null;
        private string _birthdate = null;
        private string _birthplace = null;
        private string _religion = null;
        private string _civilstatus = null;
        private string _citizenship = null;
        private string _occupation = null;
        private string _telno = null;
		private string _mobileno = null;
        private string _email = null;
        private string _address1 = null;
        private string _address2 = null;
        private string _zipcode = null;
        private string _countrycode = null;
        private string _reasonrequest = null;
        private string _datecreated = null;
        private string _branchcode = null;
        private string _deleted = null;
        private string _remitterclass = null;
		private string _bankname = null;
		private string _branchname = null;
		private string _accountno = null;
		private string _accountcurrency = null;
		private string _accounttype = null;
		private string _company = null;
		private string _created_by = null;
		private string _cifid = null;
		private string _remcifid = null;
		private string _detailed_desc = null;
		private string _worker_type = null;
		private string _salary = null;
		private string _salary_currency = null;
		private string _relationship = null;
		private string _notification_mode = null;
        private string _pobox = null;
        private string _userid = null;

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

        public string title
        {
            get { return this._title; }
            set { _title = value; }
        }

		public string cifclass
		{
			get { return this._cifclass; }
			set { _cifclass = value; }
		}

        public string lastname
        {
            get { return this._lastname; }
            set { _lastname = value; }
        }

        public string firstname
        {
            get { return this._firstname; }
            set { _firstname = value; }
        }

        public string middlename
        {
            get { return this._middlename; }
            set { _middlename = value; }
        }


        public string alias
        {
            get { return this._alias; }
            set { _alias = value; }
        }

        public string gender
        {
            get { return this._gender; }
            set { _gender = value; }
        }

        public string age
        {
            get { return this._age; }
            set { _age = value; }
        }

        public string birthdate
        {
            get { return this._birthdate; }
            set { _birthdate = value; }
        }

        public string birthplace
        {
            get { return this._birthplace; }
            set { _birthplace = value; }
        }

        public string religion
        {
            get { return this._religion; }
            set { _religion = value; }
        }

        public string civilstatus
        {
            get { return this._civilstatus; }
            set { _civilstatus = value; }
        }

        public string citizenship
        {
            get { return this._citizenship; }
            set { _citizenship = value; }
        }

        public string occupation
        {
            get { return this._occupation; }
            set { _occupation = value; }
        }

        public string telno
        {
            get { return this._telno; }
            set { _telno = value; }
		}

		public string mobileno
		{
			get { return this._mobileno; }
			set { _mobileno = value; }
		}

        public string email
        {
            get { return this._email; }
            set { _email = value; }
        }

        public string address1
        {
            get { return this._address1; }
            set { _address1 = value; }
        }

        public string address2
        {
            get { return this._address2; }
            set { _address2 = value; }
        }

        public string zipcode
        {
            get { return this._zipcode; }
            set { _zipcode = value; }
        }

        public string countrycode
        {
            get { return this._countrycode; }
            set { _countrycode = value; }
        }

        public string reasonrequest
        {
            get { return this._reasonrequest; }
            set { _reasonrequest = value; }
        }

        public string datecreated
        {
            get { return this._datecreated; }
            set { _datecreated = value; }
        }

        public string branchcode
        {
            get { return this._branchcode; }
            set { _branchcode = value; }
        }

        public string deleted
        {
            get { return this._deleted; }
            set { _deleted = value; }
        }

        public string remitterclass
        {
            get { return this._remitterclass; }
            set { _remitterclass = value; }
        }

		public string bankname
		{
			get { return this._bankname; }
			set { _bankname = value; }
		}

		public string branchname
		{
			get { return this._branchname; }
			set { _branchname = value; }
		}

		public string accountno
		{
			get { return this._accountno; }
			set { _accountno = value; }
		}

		public string accountcurrency
		{
			get { return this._accountcurrency; }
			set { _accountcurrency = value; }
		}

		public string accounttype
		{
			get { return this._accounttype; }
			set { _accounttype = value; }
		}

		public string created_by
		{
			get { return this._created_by; }
			set { _created_by = value; }
		}

		public string company
		{
			get { return this._company; }
			set { _company = value; }
		}

		public string cifid
		{
			get { return this._cifid; }
			set { _cifid = value; }
		}

		public string remcifid
		{
			get { return this._remcifid; }
			set { _remcifid = value; }
		}

		public string detailed_desc
		{
			get { return this._detailed_desc; }
			set { _detailed_desc = value; }
		}

		public string worker_type
		{
			get { return this._worker_type; }
			set { _worker_type = value; }
		}
		
		public string salary
		{
			get { return this._salary; }
			set { _salary = value; }
		}

		public string salary_currency
		{
			get { return this._salary_currency; }
			set { _salary_currency = value; }
		}

		public string relationship
		{
			get { return this._relationship; }
			set { _relationship = value; }
		}

		public string notification_mode
		{
			get { return this._notification_mode; }
			set { _notification_mode = value; }
		}

        public string pobox
        {
            get { return this._pobox; }
            set { _pobox = value; }
        }
        public string userid
        {
            get { return this._userid; }
            set { _userid = value; }
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

                _seq_id			 = this.getRecord(base.getColumn("SEQ_ID"));
                _title			 = this.getRecord(base.getColumn("Title"));
				_cifclass		 = this.getRecord(base.getColumn("CIFClass"));
                _lastname		 = this.getRecord(base.getColumn("LastName"));
                _firstname		 = this.getRecord(base.getColumn("FirstName"));
                _middlename		 = this.getRecord(base.getColumn("MiddleName"));
                _alias			 = this.getRecord(base.getColumn("Alias"));
                _gender			 = this.getRecord(base.getColumn("Gender"));
                _age			 = this.getRecord(base.getColumn("Age"));
                _birthdate		 = this.getRecord(base.getColumn("BirthDate"));
                _birthplace		 = this.getRecord(base.getColumn("BirthPlace"));
                _religion		 = this.getRecord(base.getColumn("Religion"));
                _civilstatus	 = this.getRecord(base.getColumn("CivilStatus"));
                _citizenship	 = this.getRecord(base.getColumn("Citizenship"));
                _occupation		 = this.getRecord(base.getColumn("Occupation"));
                _telno			 = this.getRecord(base.getColumn("TelNo"));
                _email			 = this.getRecord(base.getColumn("Email"));
				_company		 = this.getRecord(base.getColumn("CompanyName"));
                _address1		 = this.getRecord(base.getColumn("Address1"));
                _address2		 = this.getRecord(base.getColumn("Address2"));
                _zipcode		 = this.getRecord(base.getColumn("ZipCode"));
                _datecreated	 = this.getRecord(base.getColumn("DateCreated"));
                _deleted		 = this.getRecord(base.getColumn("Deleted"));
				_bankname		 = this.getRecord(base.getColumn("BankCode"));
				_branchname		 = this.getRecord(base.getColumn("BranchCode"));
				_accountno		 = this.getRecord(base.getColumn("AccountNo"));
				_accountcurrency = this.getRecord(base.getColumn("AccountCurrency"));
				_accounttype	 = this.getRecord(base.getColumn("AccountType"));
				_created_by		 = this.getRecord(base.getColumn("CreatedBy"));
				_cifid			 = this.getRecord(base.getColumn("CIFID"));
				_remcifid		 = this.getRecord(base.getColumn("RemitterCIFID"));
				_detailed_desc	 = this.getRecord(base.getColumn("DetailedDesc"));
				_worker_type	 = this.getRecord(base.getColumn("WorkerType"));
				_salary			 = this.getRecord(base.getColumn("Salary"));
				_salary_currency = this.getRecord(base.getColumn("SalaryCurrency"));
				_relationship	 = this.getRecord(base.getColumn("Relationship"));
				_notification_mode = this.getRecord(base.getColumn("NotificationMode"));
                _userid = this.getRecord(base.getColumn("UserId"));
                _pobox = this.getRecord(base.getColumn("POBox"));

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

        public irsInternetRem()
        {
            this.TableName = "TBL_INTERNET_REMITTER";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            String sql = "";
            if (id < 0 && id != -1)
            {
                sql = "SELECT * FROM v_internet_remitter_bene_details WHERE SEQ_ID = " + (id * -1) + " order by preferredmailadd desc";
            }
            else
            {
                sql = "SELECT * FROM TBL_INTERNET_REMITTER WHERE SEQ_ID = " + id;
            }
            SqlCommand command = getSqlCommand(sql, false);
            this.EnforceConstraint = false;
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
