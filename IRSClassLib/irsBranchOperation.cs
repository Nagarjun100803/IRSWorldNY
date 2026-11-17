using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsBranchOperation : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        //private string _region_id = null;
        private string _main_seq_id = null;
        private string _branch_id = null;
        private string _branch = null;
        private string _email = null;
        private string _description = null;

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

        //public string region_id
        //{
        //    get { return this._region_id; }
        //    set { _region_id = value; }
        //}

        public string branch_id
        {
            get { return this._branch_id; }
            set { _branch_id = value; }
        }

        public string branch
        {
            get { return this._branch; }
            set { _branch = value; }
        }

        public string email
        {
            get { return this._email; }
            set { _email = value; }
        }

        public string description
        {
            get { return this._description; }
            set { _description = value; }
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
                //_region_id      = this.getRecord(base.getColumn("RegionID"));
                _branch_id      = this.getRecord(base.getColumn("BranchID"));
                _branch         = this.getRecord(base.getColumn("Branch"));
                _email          = this.getRecord(base.getColumn("email"));
                _description    = this.getRecord(base.getColumn("description"));


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

        public irsBranchOperation()
		{
            this.TableName = "TBL_BRANCH_OPERATION";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_BRANCH_OPERATION WHERE SEQ_ID = " + id, false);
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

        public bool commit_record_tmp(string userID, string actionType, string paramValues)
        {
            //this.TableName = "TBL_BRANCH_OPERATION_TMP";
            //this.addMode = true;
            //if (base.save() == true)
            //{
            //    this.seq_id = base.IdentityValue.ToString(); return true;
            //}
            //this.get_error_dbase(); return false;

            this.TableName = "TBL_BRANCH_OPERATION_TMP";
            this.IdentityColumn = "SEQ_ID";
            string sqlstmt = "SELECT * FROM TBL_BRANCH_OPERATION_TMP WHERE 1=0";
            base.getUpdateDS(sqlstmt);
            //set parameter
            string pbrid = "";
            string pbranch = "";
            string pemail = "";
            string pdesc = "";
            string[] paramarr = paramValues.Split('^');
            foreach (string s in paramarr)
            {

                if(s.Split('~')[0] == "MAIN_SEQ_ID")
                {
                    _main_seq_id = s.Split('~')[1];
                }
                if (s.Split('~')[0] == "BranchID")
                {
                    pbrid = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Branch")
                {
                    pbranch = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Email")
                {
                    pemail = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Description")
                {
                    pdesc = s.Split('~')[1];
                }
            }

            if (isUpdatable)
            {
                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    uprow["BranchID"] = pbrid;
                    uprow["Branch"] = pbranch;
                    uprow["Email"] = pemail;
                    uprow["description"] = pdesc;                  
                    uprow["MAIN_SEQ_ID"] = Convert.ToInt32(seq_id);
                    uprow["Maker"] = userID;
                    uprow["DateAdded"] = dateNow;
                    uprow["CurrStatus"] = "For Approval";
                    uprow["StatusDate"] = dateNow;
                    uprow["ActionType"] = actionType;
                    uprow["Module"] = "Branch Administration >> Branch Operation";
                    uprow["MenuName"] = "Branch Operation";
                    table.Rows.Add(uprow);
                }

                base.addMode = false;
                return base.save(sqlHelperWs.RETAIN_DATASET);
            }
            return false;


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
