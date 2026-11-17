using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Configuration;

namespace IRSClassLib
{
    public class irsVANTerritory : sqlHelperWs
    {
        
       #region PRIVATE VARIABLE
        
        private DataSet data_set                = null;
        private string  error_message           = null;

        private string _VANTerritoryID = null;
        private string  _country                = null;	
        private string  _bank                   = null;
        private string  _accountHolding         = null;
        private string  _address                = null;
        private string  _accountNumber          = null;
        private string  _accountName            = null;
        private string  _IBAN                   = null;
        private string  _SWIFTbic               = null;
        private string  _BSB                    = null;
        private string  _bankCode               = null;
        private string  _branchCode             = null;
        private string _cleRIB = null;
        private string  _clearingCode	        = null;
        private string  _giroNumber	            = null;
        private string  _sortCode	            = null;
        private string _checkDigit = null;
        private string  _bankGiroCreditNumber   = null;
        private string _DateCreated = null;
        private string _DateUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        private string _isDeleted = null;
        private string _Currency = null;
        private string _CountryID = null;
        
        #endregion

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string VANTerritoryID
        {
            get { return this._VANTerritoryID; }
            set { _VANTerritoryID = value; }
        }

        public string country
        {
            get { return this._country; }
            set { _country = value; }
        }

        public string bank
        {
            get { return this._bank; }
            set { _bank = value; }
        }

        public string accountHolding
        {
            get { return this._accountHolding; }
            set { _accountHolding = value; }
        }

        public string address
        {
            get { return this._address; }
            set { _address = value; }
        }

        public string accountNumber
        {
            get { return this._accountNumber; }
            set { _accountNumber = value; }
        }

        public string accountName
        {
            get { return this._accountName; }
            set { _accountName = value; }
        }

        public string IBAN
        {
            get { return this._IBAN; }
            set { _IBAN = value; }
        }

        public string SWIFTbic
        {
            get { return this._SWIFTbic; }
            set { _SWIFTbic = value; }
        }

        public string BSB
        {
            get { return this._BSB; }
            set { _BSB = value; }
        }

        public string bankCode
        {
            get { return this._bankCode; }
            set { _bankCode = value; }
        }

        public string branchCode
        {
            get { return this._branchCode; }
            set { _branchCode = value; }
        }

        public string cleRIB
        {
            get { return this._cleRIB; }
            set { _cleRIB = value; }
        }

        public string clearingCode
        {
            get { return this._clearingCode; }
            set { _clearingCode = value; }
        }

        public string giroNumber
        {
            get { return this._giroNumber; }
            set { _giroNumber = value; }
        }

        public string sortCode
        {
            get { return this._sortCode; }
            set { _sortCode = value; }
        }

        public string checkDigit
        {
            get { return this._checkDigit; }
            set { _checkDigit = value; }
        }

        public string bankGiroCreditNumber
        {
            get { return this._bankGiroCreditNumber; }
            set { _bankGiroCreditNumber = value; }
        }

        public string isDeleted
        {
            get { return this._isDeleted; }
            set { _isDeleted = value; }
        }

        public string Currency
        {
            get { return this._Currency; }
            set { _Currency = value; }
        }

        public string CountryID
        {
            get { return this._CountryID; }
            set { _CountryID = value; }
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

                _VANTerritoryID = this.getRecord(base.getColumn("VANTerritoryID"));
                _country = this.getRecord(base.getColumn("country"));
                _bank = this.getRecord(base.getColumn("bank"));
                _accountHolding = this.getRecord(base.getColumn("accountHolding"));
                _address = this.getRecord(base.getColumn("address"));
                _accountNumber = this.getRecord(base.getColumn("accountNumber"));
                _accountName = this.getRecord(base.getColumn("accountName"));
                _IBAN = this.getRecord(base.getColumn("IBAN"));
                _SWIFTbic = this.getRecord(base.getColumn("SWIFTbic"));
                _BSB = this.getRecord(base.getColumn("BSB"));
                _bankCode = this.getRecord(base.getColumn("bankCode"));
                _branchCode = this.getRecord(base.getColumn("branchCode"));
                _cleRIB = this.getRecord(base.getColumn("cleRIB"));
                _clearingCode = this.getRecord(base.getColumn("clearingCode"));
                _giroNumber = this.getRecord(base.getColumn("giroNumber"));
                _sortCode = this.getRecord(base.getColumn("sortCode"));
                _checkDigit = this.getRecord(base.getColumn("checkDigit"));   
                _bankGiroCreditNumber = this.getRecord(base.getColumn("bankGiroCreditNumber"));
                //_DateCreated = this.getRecord(base.getColumn("DateCreated"));
                //_DateUpdated = this.getRecord(base.getColumn("DateUpdated"));   
                _isDeleted = this.getRecord(base.getColumn("isDeleted"));   
                _Currency = this.getRecord(base.getColumn("Currency"));   
                _CountryID = this.getRecord(base.getColumn("CountryID"));   
                
                        
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

        public irsVANTerritory()
		{
            this.TableName = "VanTerritory";
            this.IdentityColumn = "VANTerritoryID";
            this.error_message = null;
		}

        public override void getUpdateDS(int id)
        {
            //SqlCommand command = getSqlCommand("select * from VanTerritory where VANTerritoryID = " + id, false);
            //getUpdateDataSet(command);
            
            //SqlCommand command = getSqlCommand("spz_getVanTerritoryDetails");
            //command.Parameters.Add("@VANTerritoryID", SqlDbType.Int, 4).Value = id;
            //getUpdateDataSet(command);
            base.getUpdateDS("spz_getVanTerritoryDetails", new object[] { id });
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
            base.IdentityValue = this.VANTerritoryID;

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
                this.VANTerritoryID = base.IdentityValue.ToString(); return true;
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
