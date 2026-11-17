using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace IRSClassLib
{
    [Serializable]
    public class irsClientMaster_nWs : sqlHelper
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;

        private string error_message    = null;
		private string _seq_id			= null;
		private string _cifid			= null;
		private string _cif_type		= null;
		private string _last_name		= null;
		private string _first_name		= null;
		private string _middle_name		= null;
		private string _gender			= null;
		private string _age				= null;
		private string _civil_status	= null;
		private string _citizenship		= null;
		private string _birth_date		= null;
		private string _birth_place		= null;
		private string _location_code	= null;
		private string _remarks			= null;
		private string _date_created	= null;
		private string _date_modified	= null;
		private string _deleted			= null;
		private string _created_by		= null;
		private string _modified_by		= null;
		private string _title			= null;
		private string _scanned_photo	= null;
		private string _alias			= null;
		private string _branch_code		= null;
		private string _full_name		= null;
		public bool	   mas_ins_flag		= false;
		public string dummy_cifid		= null;
		public string exist_msg			= null;
		public string _cifclass			= null;

        
        #endregion
        

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

		public string cifclass
		{
			get { return this._cifclass; }
			set { _cifclass = value; }
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

		public string cif_type
		{
			get { return this._cif_type; }
			set { _cif_type = value; }
		}

		public string last_name
		{
			get { return this._last_name; }
			set { _last_name = value; }
		}
				
		public string first_name
		{
			get { return this._first_name; }
			set { _first_name = value; }
		}
				
		public string middle_name
		{
			get { return this._middle_name; }
			set { _middle_name = value; }
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
				
		public string civil_status
		{
			get { return this._civil_status; }
			set { _civil_status = value; }
		}
				
		public string citizenship
		{
			get { return this._citizenship; }
			set { _citizenship = value; }
		}
				
		public string birth_date
		{
			get { return this._birth_date; }
			set { _birth_date = value; }
		}
		
		public string birth_place
		{
			get { return this._birth_place; }
			set { _birth_place = value; }
		}
				
		public string location_code
		{
			get { return this._location_code; }
			set { _location_code = value; }
		}
				
		public string remarks
		{
			get { return this._remarks; }
			set { _remarks = value; }
		}

		public string date_created
		{
			get { return this._date_created; }
			set { _date_created = value; }
		}

		public string date_modified
		{
			get { return this._date_modified; }
			set { _date_modified = value; }
		}

		public string deleted
		{
			get { return this._deleted; }
			set { _deleted = value; }
		}

		public string created_by
		{
			get { return this._created_by; }
			set { _created_by = value; }
		}

		public string modified_by
		{
			get { return this._modified_by; }
			set { _modified_by = value; }
		}

		public string title
		{
			get { return this._title; }
			set { _title = value; }
		}

		public string scanned_photo
		{
			get { return this._scanned_photo; }
			set { _scanned_photo = value; }
		}

		public string alias
		{
			get { return this._alias; }
			set { _alias = value; }
		}

		public string branch_code
		{
			get { return this._branch_code; }
			set { _branch_code = value; }
        }

		public string full_name
		{
			get { return this._full_name; }
			set { _full_name = value; }
		}

        #endregion
        

        #region PUBLIC METHOD

		public bool _INSERT_FLAG = true;

		public void INSERT_FLAG(bool flag)
		{
			this._INSERT_FLAG = flag;
		}

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
                _cif_type       = this.getRecord(base.getColumn("CIFType"));
                _last_name      = this.getRecord(base.getColumn("LastName"));
                _first_name     = this.getRecord(base.getColumn("FirstName"));
                _middle_name    = this.getRecord(base.getColumn("MiddleName"));
                _gender         = this.getRecord(base.getColumn("Gender"));
                _age            = this.getRecord(base.getColumn("Age"));
                _civil_status   = this.getRecord(base.getColumn("CivilStatus"));
                _citizenship    = this.getRecord(base.getColumn("Citizenship"));
                _birth_date     = this.getRecord(base.getColumn("BirthDate"));
                _birth_place    = this.getRecord(base.getColumn("BirthPlace"));
                _remarks        = this.getRecord(base.getColumn("Remarks"));
                _date_created   = this.getRecord(base.getColumn("DateCreated"));
                _date_modified  = this.getRecord(base.getColumn("DateModified"));
                _deleted        = this.getRecord(base.getColumn("Deleted"));
                _created_by     = this.getRecord(base.getColumn("CreatedBy"));
                _modified_by    = this.getRecord(base.getColumn("ModifiedBy"));
                _title          = this.getRecord(base.getColumn("Title"));
                _scanned_photo  = this.getRecord(base.getColumn("ScannedPhoto"));
                _alias          = this.getRecord(base.getColumn("Alias"));
                _branch_code    = this.getRecord(base.getColumn("BranchCode"));
				_full_name		= this.getRecord(base.getColumn("FullName"));

                this.data_set = dataset; return true;
                
            }
            return false;
		}


        public string construct_id(string[] column_values)
        {
            string cif_type  = string.Empty;
            string branch_code  = string.Empty;
			string branch_code_temp = string.Empty;

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
					if (s[0].ToUpper() == "CIFTYPE")    { cif_type = s[1].Trim(); }
                    if (s[0].ToUpper() == "BRANCHCODE") { branch_code = s[1].Trim(); }
                }
            }

			if (branch_code.Length > 3)
			{
				branch_code_temp = branch_code.Substring(1, 3);
			}
			else
			{
				branch_code_temp = branch_code;
			}

			return cif_type + branch_code_temp + DateTime.Today.Date.ToString("MMddyy") + this.construct_sequence_id(cif_type, branch_code);
        }

        public string construct_sequence_id(string cif_type, string branch_code)
        {
			SqlCommand sqcommand = getSqlCommand("sp_construct_cifid"); // sp_create_cifidmaster 
			sqcommand.Parameters.Add("@CIFType", SqlDbType.SmallInt).Value   = cif_type;
			sqcommand.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = branch_code;
			DataTable dt = getDataSet(sqcommand, "TBL_SEQ").Tables[0];

            string returns = string.Empty;
			
            if (dt != null && dt.Rows.Count > 0)
            {
                returns = Convert.ToInt32(dt.Rows[0]["SEQUENCE"]).ToString().PadLeft(6, '0'); dt.Dispose();
            }

            dt = null; return returns;
            
        }

        public bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();                       
            
            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
					if(s[0].ToUpper() == "CIFCLASS")
					{
						cifclass = s[1];

                        if (!_INSERT_FLAG)
                        {
                            continue;
                        }
					}
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
            
            if (base.addMode)
            {
                string fname = uprow["FirstName"].ToString() + ' ' + uprow["MiddleName"].ToString() + ' ' + uprow["LastName"].ToString().Trim();
                uprow["FullName"] = fname.Trim();
            }
            else
            {
                string fname = base.upds.Tables[0].Rows[selectedRow]["FirstName"].ToString() + ' ' + base.upds.Tables[0].Rows[selectedRow]["MiddleName"].ToString() + ' ' + base.upds.Tables[0].Rows[selectedRow]["LastName"].ToString();
                base.upds.Tables[0].Rows[selectedRow]["FullName"] = fname.Trim();
            }
			
            return true;
        }

        public irsClientMaster_nWs()
		{            
			this.TableName      = "TBL_CIF_MASTER";
			this.IdentityColumn = "SEQ_ID";
            this.error_message	= null;
		}

		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_MASTER WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(command);
		}

		public bool insert_record()
		{
			mas_ins_flag = true;

            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;			
		}

		public bool update_record()
		{
			mas_ins_flag = false;

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
                this.seq_id = base.IdentityValue.ToString();
                this._cifid = get_gcifid();
				return true;
            }
			this.get_error_dbase(); return false;
        }

		public bool insert_to_other_ids()
		{
			irsClientOtherIds otherid = new irsClientOtherIds();
			if (otherid.insert_record() == true)
			{
				string[] col_vals = null;
				col_vals = new string[] { "CIFID~" + get_gcifid(), "OtherIDs~" + get_gcifid(), "IRSWorldSystem~1"};

				if (otherid.set_record(col_vals) == true)
				{
					otherid.commit_record(); return true;
				}
				return false;
			}
			return false;
		}

		public bool is_existing_cclass(string cifid, string cclass)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return (Convert.ToInt32(commons.execScalar("sp_cif_count_cifclass", new string[] { cifid, cclass })) > 0);
		}

		public bool insert_cif_class(string cclass)
		{
			irsClientClassifications cif_class = new irsClientClassifications();
			if (cif_class.insert_record() == true)
			{
				string[] cols_vals_class = null;
				string gcifid = get_gcifid();
				cols_vals_class = new string[] { "CIFID~" + gcifid, "CIFClass~" + cclass, "Deleted~false"};

				if (this.is_existing_cclass(gcifid, cclass) == true) return false;

				if(cif_class.set_record(cols_vals_class) == true)
				{
					cif_class.commit_record(); return true;
				}
				return false;
			}
			return false;
		}

		public string get_gcifid()
		{
			string g_cifid = null;

			this.getUpdateDS(Convert.ToInt32(base.IdentityValue));

			if (dataset != null && dataset.Tables[0].Rows.Count > 0)
			{
				base.selectedRow = 0;

				g_cifid = this.getRecord(base.getColumn("CIFID"));
				return g_cifid;
			}
			return null;
		}

        public void rollback_record()
        {
            base.cancel();
        }

		public bool is_existing(string param_values)
		{
			sqlHelperWs commons = new sqlHelperWs();

			string[] mas_fields = new string[] { "LastName", "FirstName", "MiddleName", "BirthDate", "BranchCode", "CIFType"};
			string mas_fldvals  = null;
			string[] cols		= param_values.Split('|');			

			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				for (int j = 0; j < mas_fields.Length; j++)
				{
					if (mas_fields[j].ToUpper() == vals[0].ToUpper()) mas_fldvals += cols[i] + '|';
				}				
			}

			mas_fldvals = mas_fldvals.Substring(0, mas_fldvals.Length - 1);

			string[] fields = mas_fldvals.Split('|');
			string lname = String.Empty;
			string fname = String.Empty;
			string mname = String.Empty;
			string bdate = String.Empty;
			string bcode = String.Empty;
			Int32 ciftype = 0;

			for (int k = 0; k < fields.Length; k++)
			{
				string[] mas_vals = fields[k].Split('~');
								
				if (mas_vals[0].ToUpper() == "LASTNAME")   lname = mas_vals[1];
				if (mas_vals[0].ToUpper() == "FIRSTNAME")  fname = mas_vals[1];
				if (mas_vals[0].ToUpper() == "MIDDLENAME") mname = mas_vals[1];
				if (mas_vals[0].ToUpper() == "BIRTHDATE")  bdate = mas_vals[1];
				if (mas_vals[0].ToUpper() == "BRANCHCODE") bcode = mas_vals[1];
				if (mas_vals[0].ToUpper() == "CIFTYPE") ciftype = Convert.ToInt32(mas_vals[1]);
			}

			if (ciftype == 1)
			{
				return (Convert.ToInt32(commons.execScalar("sp_cif_is_existing", new string[] { lname, fname, mname, bdate, bcode })) > 0);
			}
			else
			{
				return this.is_existing_bus_details(lname, fname);
			}
		}

		public bool is_existing_bus_details(string lname, string fname)
		{
			String sql  = "SELECT COUNT(*) FROM " + TableName + " WHERE LASTNAME = '" + lname.Trim() + "'";
				   sql += "AND FIRSTNAME = '" + fname.Trim() + "' AND ISNULL(Deleted, '') != 'True' AND CIFType = 2";
			
			return (Convert.ToInt32(execScalar(sql)) > 0);
		}

		public string get_lookup_fldvalue(string flddesc, string fldgroup)
		{
			sqlHelperWs commons = new sqlHelperWs();

			object x = commons.execScalar("sp_lookup_getfld_value", new string[] { flddesc, fldgroup });

			return x.ToString();
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
/*
  
 -Remitter 
remitter id, erps id, vip card no., phone no., lastname, firstname

-Benef
casa no., lastname, firstname, gfmc no.

-ID cards
type, number
 
  
*/