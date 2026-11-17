using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientIds : sqlHelperWs
	{
		#region PRIVATE VARIABLES

		private DataSet data_set			= null;
		private string error_message		= null;

		private string _seq_id				= null;
		private string _cifid				= null;
		private string _id_type				= null;
		private string _id_no				= null;
		private string _expiry_date			= null;
		private string _remarks				= null;
		private string _scanned_id			= null;
		private string _govt_id_class		= null;
		private string _visa_type			= null;
		private string _id_classification	= null;
        private string _Place_Of_Issuance   = null;
		public bool		ins_flag			= false;

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

		public string id_type
		{
			get { return this._id_type; }
			set { _id_type = value; }
		}

		public string id_no
		{
			get { return this._id_no; }
			set { _id_no = value; }
		}

		public string expiry_date
		{
			get { return this._expiry_date; }
			set { _expiry_date = value; }
		}

		public string remarks
		{
			get { return this._remarks; }
			set { _remarks = value; }
		}

		public string scanned_id
		{
			get { return this._scanned_id; }
			set { _scanned_id = value; }
		}

		public string govt_id_class
		{
			get { return this._govt_id_class; }
			set { _govt_id_class = value; }
		}

		public string visa_type
		{
			get { return this._visa_type; }
			set { _visa_type = value; }
		}

		public string id_classification
		{
			get { return this._id_classification; }
			set { _id_classification = value; }
		}

        public string Place_Of_Issuance
        {
            get { return this._Place_Of_Issuance; }
            set { _Place_Of_Issuance = value; }
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

                _seq_id				= this.getRecord(base.getColumn("SEQ_ID"));
                _cifid				= this.getRecord(base.getColumn("CIFID"));
                _id_type			= this.getRecord(base.getColumn("IdType"));
                _id_no				= this.getRecord(base.getColumn("IDNo"));
                _remarks			= this.getRecord(base.getColumn("Remarks"));
                _scanned_id			= this.getRecord(base.getColumn("ScannedID"));
				_govt_id_class		= this.getRecord(base.getColumn("GovtIdClass"));
				_visa_type			= this.getRecord(base.getColumn("VisaType"));
				_id_classification	= this.getRecord(base.getColumn("IDClassification"));
                _Place_Of_Issuance  = this.getRecord(base.getColumn("PlaceOfIssuance"));

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

		public irsClientIds()
		{            
			this.TableName      = "TBL_CIF_IDS";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

		public override void getUpdateDS(int id)
		{
			SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_IDS WHERE SEQ_ID = " + id, false);
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

		public bool is_existing(string param_values)
		{
			string ids = this.count_record(param_values);

			String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE CIFID = '" + ((ids.Split('&'))[0]).Trim() + "' AND IDNo = '" + ((ids.Split('&'))[1]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";

			return (Convert.ToInt32(execScalar(sql)) > 0);
		}

		public string count_record(string param_values)
		{
			string[] cols = param_values.Split('|');
			string id_cifid = null;
			string id_no = null;			

			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				if (vals[0].ToUpper() == "CIFID") id_cifid = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "IDNO") id_no = (cols[i].Split('~'))[1];
			}

			return id_cifid + '&' + id_no;
		}
		
		public bool is_existing_idtype(string param_values)
		{
			string id_type = this.count_idtype_record(param_values);

			String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE CIFID = '" + ((id_type.Split('&'))[0]).Trim() + "' AND IDType = '" + ((id_type.Split('&'))[1]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";

			return (Convert.ToInt32(execScalar(sql)) > 0);
		}

		public string count_idtype_record(string param_values)
		{
			string[] cols = param_values.Split('|');
			string id_cifid = null;
			string id_type = null;

			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				if (vals[0].ToUpper() == "CIFID") id_cifid = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "IDTYPE") id_type = (cols[i].Split('~'))[1];
			}

			return id_cifid + '&' + id_type;
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
