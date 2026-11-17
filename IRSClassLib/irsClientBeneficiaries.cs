using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientBeneficiaries : sqlHelperWs
	{
		#region PRIVATE VARIABLES

		private DataSet data_set			= null;
		private string error_message		= null;

		private string _seq_id				= null;
		private string _cifid				= null;
		private string _remitter_cifid		= null;
		private string _relationship		= null;
		private string _notification_mode	= null;
		private string _product_code		= null;
		private string _implementation_code	= null;
		private string _account_seqid		= null;
		public bool   ins_flag				= false;
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

		public string remitter_cifid
		{
			get { return this._remitter_cifid; }
			set { _remitter_cifid = value; }
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

		public string product_code
		{
			get { return this._product_code; }
			set { _product_code = value; }
		}

		public string implementation_code
		{
			get { return this._implementation_code; }
			set { _implementation_code = value; }
		}

		public string account_seqid
		{
			get { return this._account_seqid; }
			set { _account_seqid = value; }
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

                _seq_id				 = this.getRecord(base.getColumn("SEQ_ID"));
                _cifid				 = this.getRecord(base.getColumn("CIFID"));
                _remitter_cifid		 = this.getRecord(base.getColumn("RemitterCIFID"));
                _relationship		 = this.getRecord(base.getColumn("Relationship"));
                _notification_mode   = this.getRecord(base.getColumn("NotificationMode"));
                _product_code		 = this.getRecord(base.getColumn("ProductCode"));
				_implementation_code = this.getRecord(base.getColumn("ImplementationCode"));
				_account_seqid		 = this.getRecord(base.getColumn("AccountSeqid"));

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

		public irsClientBeneficiaries()
		{            
			this.TableName      = "TBL_CIF_BENEFICIARIES";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

		public override void getUpdateDS(int id)
		{
			SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_BENEFICIARIES WHERE SEQ_ID = " + id, false);
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

		public bool is_existing(int isInsert, string param_values)
		{
			string bene_details = this.count_record(param_values);
			string sqid = "-1";

			if (isInsert != 1) sqid = base.IdentityValue.ToString();

			sqlHelperWs commons = new sqlHelperWs();
			return Convert.ToBoolean(commons.execScalar("sp_cif_is_existing_bene", new string[] { isInsert.ToString(), ((bene_details.Split('&'))[0]).Trim(), ((bene_details.Split('&'))[1]).Trim(), ((bene_details.Split('&'))[2]).Trim(), ((bene_details.Split('&'))[3]).Trim(), sqid }));
		}

		public string count_record(string param_values)
		{
			string[] cols = param_values.Split('|');
			string bcifid = null, rcifid = null, prod_code = null, acctseqid = null;

			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				if (vals[0].ToUpper() == "CIFID")				bcifid = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "REMITTERCIFID")		rcifid = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "PRODUCTCODE")			prod_code = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "ACCOUNTSEQID")		acctseqid = (cols[i].Split('~'))[1];
			}

			return bcifid + '&' + rcifid + '&' + prod_code + '&' + acctseqid;
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
