using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsBranchPaymentSagePay : sqlHelper
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _url = null;
        private string _vps_protocol = null;
        private string _tx_type = null;
        private string _vendor = null;
        private string _description = null;
        private string _gift_aid = null;
        private string _apply_avs = null;
        private string _apply_3d = null;
        private string _account_type = null;
        private string _merchant_email = null;

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

        public string url
        {
            get { return this._url; }
            set { _url = value; }
        }

        public string vps_protocol
        {
            get { return this._vps_protocol; }
            set { _vps_protocol = value; }
        }

        public string tx_type
        {
            get { return this._tx_type; }
            set { _tx_type = value; }
        }

        public string vendor
        {
            get { return this._vendor; }
            set { _vendor = value; }
        }

        public string description
        {
            get { return this._description; }
            set { _description = value; }
        }


        public string gift_aid
        {
            get { return this._gift_aid; }
            set { _gift_aid = value; }
        }

        public string apply_avs
        {
            get { return this._apply_avs; }
            set { _apply_avs = value; }
        }

        public string apply_3d
        {
            get { return this._apply_3d; }
            set { _apply_3d = value; }
        }

        public string account_type
        {
            get { return this._account_type; }
            set { _account_type = value; }
        }
        public string merchant_email
        {
            get { return this._merchant_email; }
            set { _merchant_email = value; }
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
                _url            = this.getRecord(base.getColumn("Url"));
                _vps_protocol   = this.getRecord(base.getColumn("VPSProtocol"));
                _tx_type        = this.getRecord(base.getColumn("TxType"));
                _vendor         = this.getRecord(base.getColumn("Vendor"));
                _description    = this.getRecord(base.getColumn("Description"));
                _gift_aid       = this.getRecord(base.getColumn("GiftAidPayment"));
                _apply_avs      = this.getRecord(base.getColumn("ApplyAVSCV2"));
                _apply_3d       = this.getRecord(base.getColumn("Apply3DSecure"));
                _account_type   = this.getRecord(base.getColumn("AccountType"));
                _merchant_email = this.getRecord(base.getColumn("MerchantEmail"));



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

        public irsBranchPaymentSagePay()
		{            
			this.TableName      = "TBL_SAGEPAY_DEFAULTS";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_SAGEPAY_DEFAULTS WHERE SEQ_ID = " + id, false);
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
