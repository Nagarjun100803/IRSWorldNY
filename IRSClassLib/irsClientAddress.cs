using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientAddress : sqlHelperWs
	{
		#region PRIVATE VARIABLES

		private DataSet data_set				= null;
		private string error_message			= null;

        private string _seq_id					= null;
        private string _cifid					= null;
        private string _address1				= null;
        private string _address2				= null;
        private string _address3				= null;
        private string _address4				= null;
        private string _address5				= null;
        private string _address6				= null;
        private string _country_code			= null;
        private string _zip_code_seqid			= null;
        private string _po_box					= null;
        private string _po_box_district			= null;
        private string _deleted					= null;
        private string _address_type			= null;
        private string _remarks					= null;
        private string _preferredmailadd		= null;
		private string _postal_code				= null;
		public bool		ins_flag				= false;

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

        public string address3
        {
            get { return this._address3; }
            set { _address3 = value; }
        }

        public string address4
        {
            get { return this._address4; }
            set { _address4 = value; }
        }

        public string address5
        {
            get { return this._address5; }
            set { _address5 = value; }
        }

        public string address6
        {
            get { return this._address6; }
            set { _address6 = value; }
        }

		public string country_code
		{
			get { return this._country_code; }
			set { _country_code = value; }
		}

		public string zip_code_seqid
		{
			get { return this._zip_code_seqid; }
			set { _zip_code_seqid = value; }
		}

		public string po_box
		{
			get { return this._po_box; }
			set { _po_box = value; }
		}

		public string po_box_district
		{
			get { return this._po_box_district; }
			set { _po_box_district = value; }
		}

		public string deleted
		{
			get { return this._deleted; }
			set { _deleted = value; }
		}

		public string address_type
		{
			get { return this._address_type; }
			set { _address_type = value; }
		}

		public string remarks
		{
			get { return this._remarks; }
			set { _remarks = value; }
		}

		public string preferredmailadd
		{
			get { return this._preferredmailadd; }
			set { _preferredmailadd = value; }
		}
		
		public string postal_code
		{
			get { return this._postal_code; }
			set { _postal_code = value; }
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

                _seq_id					= this.getRecord(base.getColumn("SEQ_ID"));
                _cifid					= this.getRecord(base.getColumn("CIFID"));
                _address1				= this.getRecord(base.getColumn("Address1"));
                _address2				= this.getRecord(base.getColumn("Address2"));
				_address3				= this.getRecord(base.getColumn("Address3"));
                _address4				= this.getRecord(base.getColumn("Address4"));
                _address5				= this.getRecord(base.getColumn("Address5"));
                _address6				= this.getRecord(base.getColumn("Address6"));
				_country_code			= this.getRecord(base.getColumn("CountryCode"));
				_zip_code_seqid			= this.getRecord(base.getColumn("ZipCodeSeqId"));
                _po_box					= this.getRecord(base.getColumn("POBox"));
				_po_box_district		= this.getRecord(base.getColumn("POBoxDistrict"));
                _deleted				= this.getRecord(base.getColumn("Deleted"));
                _address_type			= this.getRecord(base.getColumn("AddressType"));
                _remarks			    = this.getRecord(base.getColumn("Remarks"));
				_preferredmailadd		= this.getRecord(base.getColumn("PreferredMailAdd"));
				_postal_code			= this.getRecord(base.getColumn("PostalCode"));

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

		public irsClientAddress()
		{
			this.TableName      = "TBL_CIF_ADDRESS";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

		public override void getUpdateDS(int id)
		{
			SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_ADDRESS WHERE SEQ_ID = " + id, false);
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
			string addr = this.count_record(param_values);

			String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE CIFID = '" + ((addr.Split('&'))[0]).Trim() + "' AND Address1 = '" + ((addr.Split('&'))[1]).Trim() + "'";
			sql += "AND Address2 = '" + ((addr.Split('&'))[2]).Trim() + "' AND ZipCodeSeqId = '" + ((addr.Split('&'))[3]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";

			return (Convert.ToInt32(execScalar(sql)) > 0);
		}

		public string count_record(string param_values)
		{
			string[] cols = param_values.Split('|');
			string addr_cifid = null;
			string addr1 = null;
			string addr2 = null;
			string zipid = null;

			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				if (vals[0].ToUpper() == "CIFID")		 addr_cifid = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "ADDRESS1")	 addr1 = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "ADDRESS2")	 addr2 = (cols[i].Split('~'))[1];
				if (vals[0].ToUpper() == "ZIPCODESEQID") zipid = (cols[i].Split('~'))[1];
			}

			return addr_cifid + '&' + addr1 + '&' + addr2 + '&' + zipid;
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
