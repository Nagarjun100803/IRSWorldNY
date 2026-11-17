using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientMerchants : sqlHelperWs
	{
		#region PRIVATE VARIABLES

		private DataSet data_set		= null;
		private string error_message	= null;
		
		private string _seq_id			= null;
		private string _cifid			= null;
		private string _product_code	= null;
		private string _account_seq_id	= null;
		private string _merchant_code   = null;
		private string _imp_code		= null;
		private string _mer_product_code = null;
		private string _merchant_type   = null;
		public bool ins_flag			= false;

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

		public string product_code
		{
			get { return this._product_code; }
			set { _product_code = value; }
		}

		public string account_seq_id
		{
			get { return this._account_seq_id; }
			set { _account_seq_id = value; }
		}

		public string merchant_code
		{
			get { return this._merchant_code; }
			set { _merchant_code = value; }
		}

		public string imp_code
		{
			get { return this._imp_code; }
			set { _imp_code = value; }
		}

		public string mer_product_code
		{
			get { return this._mer_product_code; }
			set { _mer_product_code = value; }
		}

		public string merchant_type
		{
			get { return this._merchant_type; }
			set { _merchant_type = value; }
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

                _seq_id         = this.getRecord(base.getColumn("SEQ_ID"));
                _cifid          = this.getRecord(base.getColumn("CIFID"));
                _product_code   = this.getRecord(base.getColumn("ProductCode"));
                _account_seq_id = this.getRecord(base.getColumn("AccountSeqId"));
				_merchant_code  = this.getRecord(base.getColumn("MerchantCode"));
				_imp_code		= this.getRecord(base.getColumn("ImplementationCode"));
				_mer_product_code = this.getRecord(base.getColumn("MerchantProductCode"));
				_merchant_type  = this.getRecord(base.getColumn("MerchantType"));

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

		public irsClientMerchants()
		{
			this.TableName = "TBL_CIF_MERCHANTS_INFO";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

		public override void getUpdateDS(int id)
		{
			SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_MERCHANTS_INFO WHERE SEQ_ID = " + id, false);
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

		public void create_gen_fields(string param_values)
		{
			string merc    = this.count_record(param_values);
			string mercode = ((merc.Split('&'))[5]).Trim();
			Int64 merpcode   = Convert.ToInt64(((merc.Split('&'))[4]).Trim());
			Int32 mertype = Convert.ToInt32(((merc.Split('&'))[6]).Trim());

			if (mertype == 1)
			{
				irsOBPSTransaction obptxn = new irsOBPSTransaction();
				obptxn.createGenericFields(mercode, merpcode);
			}			
		}

		public bool is_existing(string param_values)
		{
			string merc = this.count_record(param_values);

			String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE CIFID = '" + ((merc.Split('&'))[0]).Trim() + "' AND ProductCode = '" + ((merc.Split('&'))[1]).Trim() + "'";
				   sql += "AND AccountSeqId = '" + ((merc.Split('&'))[2]).Trim() + "' AND ImplementationCode = '" + ((merc.Split('&'))[3]).Trim() + "'";
				   sql += "AND MerchantProductCode = '" + ((merc.Split('&'))[4]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";
			return (Convert.ToInt32(execScalar(sql)) > 0);
		}

		public string count_record(string param_values)
		{
			string[] cols = param_values.Split('|');
			string mer_cifid = null;
			string prodcode  = null;
			string accid     = null;
			string impcode   = null;
			string merpcode  = null;
			string mercode   = null;
			string mertype   = null;

			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				if (vals[0].ToUpper() == "CIFID")				mer_cifid = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "PRODUCTCODE")			prodcode  = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "ACCOUNTSEQID")		accid     = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "IMPLEMENTATIONCODE")  impcode	  = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "MERCHANTPRODUCTCODE") merpcode  = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "MERCHANTCODE")		mercode   = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "MERCHANTTYPE")		mertype   = (cols[i].Split('~'))[1];
			}

			return mer_cifid + '&' + prodcode + '&' + accid + '&' + impcode + '&' + merpcode + '&' + mercode + '&' + mertype;
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
