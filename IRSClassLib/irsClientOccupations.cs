using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientOccupations : sqlHelperWs
	{
		#region PRIVATE VARIABLES

		private DataSet data_set		= null;
		private string error_message	= null;

		private string _seq_id			= null;
		private string _cifid			= null;
		private string _occupation_type = null;
		private string _worker_type		= null;
		private string _position		= null;
		private string _division		= null;
		private string _sector			= null;
		private string _employer		= null;
		private string _salary			= null;
		private string _currency		= null;
		private string _remarks			= null;
		private string _occupation		= null;
		private string _detailed_desc   = null;
		private string _source_of_funds = null;

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

		public string occupation_type
		{
			get { return this._occupation_type; }
			set { _occupation_type = value; }
		}

		public string worker_type
		{
			get { return this._worker_type; }
			set { _worker_type = value; }
		}

		public string position
		{
			get { return this._position; }
			set { _position = value; }
		}

		public string division
		{
			get { return this._division; }
			set { _division = value; }
		}

		public string sector
		{
			get { return this._sector; }
			set { _sector = value; }
		}

		public string employer
		{
			get { return this._employer; }
			set { _employer = value; }
		}

		public string salary
		{
			get { return this._salary; }
			set { _salary = value; }
		}

		public string currency
		{
			get { return this._currency; }
			set { _currency = value; }
		}

		public string remarks
		{
			get { return this._remarks; }
			set { _remarks = value; }
		}

		public string occupation
		{
			get { return this._occupation; }
			set { _occupation = value; }
		}

		public string detailed_desc
		{
			get { return this._detailed_desc; }
			set { _detailed_desc = value; }
		}

		public string source_of_funds
		{
			get { return this._source_of_funds; }
			set { _source_of_funds = value; }
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
                _occupation_type	= this.getRecord(base.getColumn("OccupationType"));
                _worker_type		= this.getRecord(base.getColumn("WorkerType"));
				_position			= this.getRecord(base.getColumn("Position"));
                _division			= this.getRecord(base.getColumn("Division"));
                _sector				= this.getRecord(base.getColumn("Sector"));
                _employer			= this.getRecord(base.getColumn("Employer"));
				_salary				= this.getRecord(base.getColumn("Salary"));
                _currency			= this.getRecord(base.getColumn("Currency"));
                _remarks			= this.getRecord(base.getColumn("Remarks"));
				_occupation			= this.getRecord(base.getColumn("Occupation"));
				_detailed_desc		= this.getRecord(base.getColumn("DetailedDesc"));
				_source_of_funds    = this.getRecord(base.getColumn("SourceOfFunds"));

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

		public irsClientOccupations()
		{            
			this.TableName      = "TBL_CIF_OCCUPATIONS";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

		public override void getUpdateDS(int id)
		{
			SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_OCCUPATIONS WHERE SEQ_ID = " + id, false);
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
