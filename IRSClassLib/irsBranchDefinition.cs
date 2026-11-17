using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
    [Serializable]
    public class irsBranchDefinition : sqlHelperWs
    {

        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _actiontype = null;
        private string _branchtype = null;
        private string _branchcode = null;
        private string _branchname = null;
        private string _negbranchcode = null;
        private string _coveringbankcode = null;
        private string _fcbranchcode = null;
        private string _classification = null;
        private string _contactperson = null;
        private string _address = null;
        /* ALESSA */
        private string _zipcode = null;
        private string _state = null;
        private string _country = null;
        private string _phone = null;
        private string _email = null;
        private string _fax = null;
        private string _timezone = null;
        private string _chargegroup = null; 
        private string _brstn = null;
        private string _couriercode = null;
        private string _pchc = null;
        private string _dom_loc = null;
        private string _bankinformationcode = null;  /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */


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

        public string actiontype
        {
            get { return this._actiontype; }
            set { _actiontype = value; }
        }

        public string branchtype
        {
            get { return this._branchtype; }
            set { _branchtype = value; }
        }

        public string branchcode
        {
            get { return this._branchcode; }
            set { _branchcode = value; }
        }

        public string branchname
        {
            get { return this._branchname; }
            set { _branchname = value; }
        }

        public string negbranchcode
        {
            get { return this._negbranchcode; }
            set { _negbranchcode = value; }
        }


        public string coveringbankcode
        {
            get { return this._coveringbankcode; }
            set { _coveringbankcode = value; }
        }

        public string fcbranchcode
        {
            get { return this._fcbranchcode; }
            set { _fcbranchcode = value; }
        }

        public string classification
        {
            get { return this._classification; }
            set { _classification = value; }
        }

        public string contactperson
        {
            get { return this._contactperson; }
            set { _contactperson = value; }
        }

        public string address
        {
            get { return this._address; }
            set { _address = value; }
        }

        public string zipcode
        {
            get { return this._zipcode; }
            set { _zipcode = value; }
        }

        public string state
        {
            get { return this._state; }
            set { _state = value; }
        }

        public string country
        {
            get { return this._country; }
            set { _country = value; }
        }

        public string phone
        {
            get { return this._phone; }
            set { _phone = value; }
        }

        public string email
        {
            get { return this._email; }
            set { _email = value; }
        }

        public string fax
        {
            get { return this._fax; }
            set { _fax = value; }
        }

        public string timezone
        {
            get { return this._timezone; }
            set { _timezone = value; }
        }

        public string chargegroup
        {
            get { return this._chargegroup; }
            set { _chargegroup = value; }
        }
        public string brstn
        {
            get { return this._brstn; }
            set { _brstn = value; }
        }
        public string couriercode
        {
            get { return this._couriercode; }
            set { _couriercode = value; }
        }
        public string pchc 
        {
            get { return this._pchc; }
            set { _pchc = value; }
        }
        public string dom_loc 
        {
            get { return this._dom_loc; }
            set { _dom_loc = value; }
        }

        /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
        public string bankinformationcode
        {
            get { return this._bankinformationcode; }
            set { _bankinformationcode = value; }

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
                _branchtype     = this.getRecord(base.getColumn("BranchType"));
                _branchcode     = this.getRecord(base.getColumn("BranchCode"));
                _branchname     = this.getRecord(base.getColumn("BranchName"));
                _negbranchcode  = this.getRecord(base.getColumn("NegBranchCode"));
                _coveringbankcode = this.getRecord(base.getColumn("CoveringBankCode"));
                _fcbranchcode   = this.getRecord(base.getColumn("FCBranchCode"));
                _classification = this.getRecord(base.getColumn("Classification"));
                _contactperson  = this.getRecord(base.getColumn("ContactPerson"));
                _address        = this.getRecord(base.getColumn("Address"));
                /* ALESSA */
                _zipcode        = this.getRecord(base.getColumn("ZipCode"));
                _state          = this.getRecord(base.getColumn("State"));
                _country        = this.getRecord(base.getColumn("Country"));
                _phone          = this.getRecord(base.getColumn("Phone"));
                _email          = this.getRecord(base.getColumn("Email"));
                _fax            = this.getRecord(base.getColumn("Fax"));
                _timezone       = this.getRecord(base.getColumn("TimeZone"));
                _chargegroup    = this.getRecord(base.getColumn("ChargeGroup"));
                _brstn          = this.getRecord(base.getColumn("Brstn"));
                _couriercode    = this.getRecord(base.getColumn("CourierCode"));
                _pchc           = this.getRecord(base.getColumn("PCHC"));
                _dom_loc        = this.getRecord(base.getColumn("Dom_Br_Loc"));
                _bankinformationcode = this.getRecord(base.getColumn("BIC")); /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */

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

        public irsBranchDefinition()
		{            
			this.TableName      = "TBL_BRANCHES";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_BRANCHES WHERE SEQ_ID = " + id, false);
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

        public DataTable getBranchCodeSetupMandatory(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("bsp_getBranchCodeSetupMandatory", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (!base.hasRows)
            {
                if (IsAdd)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    selectedRow = 0;
                }
            }
            else
            {
                if (IsAdd)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    selectedRow = 0;
                }
            }
        }

        public bool saveToTemp(string[] column_values)
        {
            bool ok = false;
            base.addMode = true;
            this.setBase("SELECT TOP 1 * FROM tbl_branches_tmp", true);

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                {
                    s[1] = null;
                }

                if (s != null && s.Length > 0)
                {
                    setColumn(s[0].Trim(), s[1]);
                }
            }


            //SAVE TO TBL_CIF_MASTER
            //setColumn("LastName", Lastname);
            //setColumn("FirstName", Firstname);
            //setColumn("MiddleName", Middlename);
            //setColumn("FullName", Lastname + ", " + Firstname + " " + Middlename);
            //setColumn("BirthDate", BirthDate);
            //setColumn("Age", getAge(Convert.ToDateTime(BirthDate)));
            //setColumn("BranchCode", BranchCode);
            //setColumn("CIFClass", 2);
            //setColumn("CIFType", 1);
            //setColumn("CIFGUID", sGUID);
            //setColumn("isWebRemitter", true);
            //setColumn("Remarks", "Web Remittance Enrollment - Beneficiary");
            //setColumn("CreatedBy", UserID);
            //setColumn("DateCreated", DateTime.Now.ToShortDateString());
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            return ok;
        }


    }
}
