using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsHitViewer : sqlHelperWs 
    {
        #region PRIVATE VARIABLE
        private DataSet data_set = null;
        private string error_message = null;
        private string branchsetupmatching = null;

        private string _SEQ_ID = null;
        private string _CIFID = null;
        private string _UW_SEQ_ID = null;
        private string _FileFormat = null;
        private string _isDeleted = null;
        private string _CIFField = null;
        private string _UWField = null;
        private string _BranchCode = null;
        private string _MatchingType = null;
        private string _isExactMatch = null;

        #endregion

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._SEQ_ID; }
            set { _SEQ_ID = value; }
        }
        public string is_deleted
        {
            get { return this._isDeleted; }
            set { _isDeleted = value; }
        }
        public string fullname_cif
        {
            get { return this._CIFField; }
            set { _CIFField = value; }
        }
        public string fullname_uni
        {
            get { return this._UWField; }
            set { _UWField = value; }
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

                _SEQ_ID = this.getRecord(base.getColumn("SEQ_ID"));
                _CIFID = this.getRecord(base.getColumn("CIFID")); ;
                _UW_SEQ_ID = this.getRecord(base.getColumn("UW_SEQ_ID"));
                _FileFormat = this.getRecord(base.getColumn("FileFormat"));
                _isDeleted = this.getRecord(base.getColumn("isDeleted"));
                _CIFField = this.getRecord(base.getColumn("CIFField"));
                _UWField = this.getRecord(base.getColumn("UWField"));
                _BranchCode = this.getRecord(base.getColumn("BranchCode"));
                _MatchingType = this.getRecord(base.getColumn("MatchingType"));
                _isExactMatch = this.getRecord(base.getColumn("isExactMatch"));

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

        public irsHitViewer(string tblName)
		{                     
            //this.TableName = "tbl_cifhitlist";      
            branchsetupmatching = tblName;
            this.TableName = tblName;
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

        public  override void getUpdateDS(int id)
        {
            if (branchsetupmatching == "tbl_cifhitlist")
            {
                SqlCommand command = getSqlCommand("SELECT * FROM tbl_cifhitlist WHERE SEQ_ID = " + id, false);
                getUpdateDataSet(command);
            }

            if (branchsetupmatching == "tbl_cifhitlist_percentage")
            {
                SqlCommand command = getSqlCommand("SELECT * FROM tbl_cifhitlist_percentage WHERE SEQ_ID = " + id, false);
                getUpdateDataSet(command);
            }

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

    public class irsAcctHitViewer : sqlHelperWs
    {
        #region PRIVATE VARIABLE
        private DataSet data_set = null;
        private string error_message = null;

        private string _SEQ_ID = null;
        private string _isDeleted = null;
        private string _AccountNo = null;

        //private string _SEQ_ID = null;
        //private string _CIFID = null;
        //private string _ReferenceNo = null;
        //private string _AW_SEQ_ID = null;
        //private string _FileFormat = null;
        //private string _isDeleted = null;
        //private string _CIFField = null;
        //private string _AccountNo = null;
        //private string _BranchCode = null;
        //private string _MatchingType = null;
        //private string _isExactMatch = null;

        #endregion

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._SEQ_ID; }
            set { _SEQ_ID = value; }
        }
        public string is_deleted
        {
            get { return this._isDeleted; }
            set { _isDeleted = value; }
        }
        //public string fullname_cif
        //{
        //    get { return this._CIFField; }
        //    set { _CIFField = value; }
        //}
        public string fullname_acctno
        {
            get { return this._AccountNo; }
            set { _AccountNo = value; }
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

                _SEQ_ID = this.getRecord(base.getColumn("SEQ_ID"));
                _AccountNo = this.getRecord(base.getColumn("AccountNo"));
                _isDeleted = this.getRecord(base.getColumn("isDeleted"));

                //_SEQ_ID = this.getRecord(base.getColumn("SEQ_ID"));
                //_CIFID = this.getRecord(base.getColumn("CIFID")); ;
                //_ReferenceNo = this.getRecord(base.getColumn("ReferenceNo"));
                //_AW_SEQ_ID = this.getRecord(base.getColumn("AW_SEQ_ID"));
                //_FileFormat = this.getRecord(base.getColumn("FileFormat"));
                //_isDeleted = this.getRecord(base.getColumn("isDeleted"));
                //_CIFField = this.getRecord(base.getColumn("CIFField"));
                //_AccountNo = this.getRecord(base.getColumn("AccountNo"));
                //_BranchCode = this.getRecord(base.getColumn("BranchCode"));
                //_MatchingType = this.getRecord(base.getColumn("MatchingType"));
                //_isExactMatch = this.getRecord(base.getColumn("isExactMatch"));

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

        public irsAcctHitViewer()
		{            
			//this.TableName      = "tbl_accounthitlist";
            this.TableName      = "tbl_accountwatchlist";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
        public  override void getUpdateDS(int id)
        {
            //SqlCommand command = getSqlCommand("SELECT * FROM tbl_accounthitlist WHERE SEQ_ID = " + id, false);
            SqlCommand command = getSqlCommand("SELECT * FROM tbl_accountwatchlist WHERE SEQ_ID = " + id, false);
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
