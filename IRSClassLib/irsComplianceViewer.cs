using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsComplianceViewer : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set        = null;
        private string error_message    = null;
        private string _seq_id          = null;
        private string _lastname        = null;
        private string _firstname       = null;
        private string _middlename      = null;
        private string _aliaslastname   = null;
        private string _aliasfirstname  = null;
        private string _suffix          = null;
        private string _title           = null;
        private string _otherinfo       = null;
        private string _dateofbirth     = null;
        private string _townofbirth     = null;
        private string _countryofbirth  = null;
        private string _nationality     = null;
        private string _position        = null;
        //private string _homeno          = null;
        private string _listedon        = null;
        private string _lastupdate      = null;
        private string _dbaname         = null;
        private string _phoneno         = null;
        private string _city            = null;
        private string _state           = null;
        private string _zip             = null;
        private string _country         = null;
        private string _remarks         = null;
        private string _grouptype       = null;
        //private string _aliastype       = null;
        private string _regime          = null;
        private string _groupid         = null;
        private string _passportdetails = null;
        private string _fileformat      = null;

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

                _seq_id         = this.getRecord(base.getColumn("SEQ_ID"));
                _lastname       = this.getRecord(base.getColumn("LastName"));
                _firstname      = this.getRecord(base.getColumn("FirstName"));
                _middlename     = this.getRecord(base.getColumn("MiddleName"));
                _aliaslastname  = this.getRecord(base.getColumn("AliasLastName"));
                _aliasfirstname = this.getRecord(base.getColumn("AliasFirstName"));
                _suffix         = this.getRecord(base.getColumn("Suffix"));
                _title          = this.getRecord(base.getColumn("Title"));
                _otherinfo      = this.getRecord(base.getColumn("OtherInfo"));
                _dateofbirth    = this.getRecord(base.getColumn("DateOfBirth"));
                _townofbirth    = this.getRecord(base.getColumn("TownOfBirth"));
                _countryofbirth = this.getRecord(base.getColumn("CountryOfBirth"));
                _nationality    = this.getRecord(base.getColumn("Nationality"));
                _position       = this.getRecord(base.getColumn("Position"));
                _listedon       = this.getRecord(base.getColumn("ListedOn"));
                _lastupdate     = this.getRecord(base.getColumn("LastUpdate"));
                _dbaname        = this.getRecord(base.getColumn("DBAName"));
                _phoneno        = this.getRecord(base.getColumn("PhoneNo"));
                _city           = this.getRecord(base.getColumn("City"));
                _state          = this.getRecord(base.getColumn("State"));
                _zip            = this.getRecord(base.getColumn("Zip"));
                _country        = this.getRecord(base.getColumn("Country"));
                _remarks        = this.getRecord(base.getColumn("Remarks"));
                _grouptype      = this.getRecord(base.getColumn("GroupType"));
                _regime         = this.getRecord(base.getColumn("Regime"));
                _groupid        = this.getRecord(base.getColumn("GroupID"));
                _passportdetails = this.getRecord(base.getColumn("PassportDetails"));
                _fileformat     = this.getRecord(base.getColumn("FileFormat"));

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

        public irsComplianceViewer()
		{            
			this.TableName      = "TBL_UNIFIEDWATCHLIST";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_UNIFIEDWATCHLIST WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(command);
		}

        //public bool insert_record()
        //{
        //    if (base.newRow() == true)
        //    {
        //        return true;
        //    }
        //    this.get_error_dbase(); return false;
        //}

        //public bool update_record()
        //{
        //    base.IdentityValue = this.seq_id;

        //    if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
        //    {
        //        return true;
        //    }
        //    this.get_error_dbase(); return false;
        //}

        //public bool commit_record()
        //{
        //    if (base.save() == true)
        //    {
        //        this.seq_id = base.IdentityValue.ToString(); return true;
        //    }
        //    this.get_error_dbase(); return false;
        //}

        //public void rollback_record()
        //{
        //    base.cancel();
        //}

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
