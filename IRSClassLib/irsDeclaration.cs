using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsDeclaration : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _main_seq_id = null;
        //private string _region_id = null;
        private string _neg_branchcode = null;
        private string _branch_code = null;
        private string _declaration1 = null;
        private string _declaration2 = null;
        private string _declaration3 = null;
        private string _declaration4 = null;
        private string _declaration5 = null;

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

        public string neg_branchcode
        {
            get { return this._neg_branchcode; }
            set { _neg_branchcode = value; }
        }

        public string branch_code
        {
            get { return this._branch_code; }
            set { _branch_code = value; }
        }

        public string declaration1
        {
            get { return this._declaration1; }
            set { _declaration1 = value; }
        }

        public string declaration2
        {
            get { return this._declaration2; }
            set { _declaration2 = value; }
        }

        public string declaration3
        {
            get { return this._declaration3; }
            set { _declaration3 = value; }
        }

        public string declaration4
        {
            get { return this._declaration4; }
            set { _declaration4 = value; }
        }

        public string declaration5
        {
            get { return this._declaration5; }
            set { _declaration5 = value; }
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

                _seq_id = this.getRecord(base.getColumn("SEQ_ID"));
                _neg_branchcode = this.getRecord(base.getColumn("NegBranchCode"));
                _branch_code = this.getRecord(base.getColumn("BranchCode"));
                _declaration1 = this.getRecord(base.getColumn("Declaration1"));
                _declaration2 = this.getRecord(base.getColumn("Declaration2"));
                _declaration3 = this.getRecord(base.getColumn("Declaration3"));
                _declaration4 = this.getRecord(base.getColumn("Declaration4"));
                _declaration5 = this.getRecord(base.getColumn("Declaration5"));


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
                    s[1] =  "";
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

        public irsDeclaration()
        {
            this.TableName = "TBL_DECLARATION";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_DECLARATION WHERE SEQ_ID = " + id, false);
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
            //set param values
            string[] cols = paramValues.Split('^');
            foreach (string s in cols)
            {
                if (s.Split('~')[0] == "MAIN_SEQ_ID")
                {
                    _main_seq_id = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "NegBranchCode")
                {
                    _neg_branchcode = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "BranchCode")
                {
                    _branch_code = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Declaration1")
                {
                    _declaration1 = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Declaration2")
                {
                    _declaration2 = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Declaration3")
                {
                    _declaration3 = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Declaration4")
                {
                    _declaration4 = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Declaration5")
                {
                    _declaration5 = s.Split('~')[1];
                }
            }

            this.TableName = "TBL_DECLARATION_TMP";
            this.IdentityColumn = "SEQ_ID";
            string sqlstmt = "SELECT * FROM TBL_DECLARATION_TMP WHERE 1=0";
            base.getUpdateDS(sqlstmt);

            if (isUpdatable)
            {
                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    uprow["NegBranchCode"] = _neg_branchcode;
                    uprow["BranchCode"] = _branch_code;
                    uprow["Declaration1"] = _declaration1;
                    uprow["Declaration2"] = _declaration2;
                    uprow["Declaration3"] = _declaration3;
                    uprow["Declaration4"] = _declaration4;
                    uprow["Declaration5"] = _declaration5;

                    ////Comment out to as actiontype shpuld only be EDIT
                    ////if (seq_id == "")
                    ////{
                    ////    actionType = "ADD";
                    ////}

                    ////if (actionType != "ADD")
                    ////{
                    ////    uprow["MAIN_SEQ_ID"] = seq_id;
                    ////}

                    if (Convert.ToString(_main_seq_id).Trim() != "") {
                        uprow["MAIN_SEQ_ID"] = Convert.ToInt32(_main_seq_id);
                    }
                    
                    uprow["Maker"] = userID;
                    uprow["DateAdded"] = dateNow;
                    uprow["CurrStatus"] = "For Approval";
                    uprow["StatusDate"] = dateNow;
                    uprow["ActionType"] = actionType;
                    uprow["Module"] = "Branch Administration >> Receipt Declaration";
                    uprow["MenuName"] = "Receipt Declaration";
                    table.Rows.Add(uprow);
                }

                base.addMode = false;
                return base.save(sqlHelperWs.RETAIN_DATASET);
            }
            return false;

            //if (base.save() == true)
            //{
            //    this.seq_id = base.IdentityValue.ToString(); return true;
            //}
            //this.get_error_dbase(); return false;
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
