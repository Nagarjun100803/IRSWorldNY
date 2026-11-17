using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
    public class irsGiftsWebSettings : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set            = null;
        private string error_message        = null;
        private string _seq_id              = null;
        private string _ftp_url             = null;
        private string _username            = null;
        private string _password            = null;
        private string _folder              = null;
        private string _currency            = null;
        private string _branch_code         = null;
        private string _branch_prefix       = null;
        private string _region              = null;
        private string _branch              = null;
        private string _AccountOfficer      = null;
        private string _AccOfficerTel       = null;
        private string _AccOfficerEmail     = null;
        private string _ComplianceOfficer   = null;
        private string _CompOfficerTel      = null;
        private string _CompOfficerEmail    = null;
        private string _ExtractRuntime      = null;
        private string _IsDomestic          = null;

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

                _seq_id             = this.getRecord(base.getColumn("SEQ_ID"));
                _ftp_url            = this.getRecord(base.getColumn("FtpUrl"));
                _username           = this.getRecord(base.getColumn("UserName"));
                _password           = this.getRecord(base.getColumn("Password"));
                _folder             = this.getRecord(base.getColumn("Folder"));
                _currency           = this.getRecord(base.getColumn("Currency"));
                _branch_code        = this.getRecord(base.getColumn("BranchCode"));
                _branch_prefix      = this.getRecord(base.getColumn("BranchPrefix"));
                _region             = this.getRecord(base.getColumn("Region"));
                _branch             = this.getRecord(base.getColumn("Branch"));
                _AccountOfficer     = this.getRecord(base.getColumn("AccountOfficer"));
                _AccOfficerTel      = this.getRecord(base.getColumn("AccOfficerTel"));
                _AccOfficerEmail    = this.getRecord(base.getColumn("AccOfficerEmail"));
                _ComplianceOfficer  = this.getRecord(base.getColumn("ComplianceOfficer"));
                _CompOfficerTel     = this.getRecord(base.getColumn("CompOfficerTel"));
                _CompOfficerEmail   = this.getRecord(base.getColumn("CompOfficerEmail"));
                _ExtractRuntime     = this.getRecord(base.getColumn("ExtractRuntime"));
                _IsDomestic         = this.getRecord(base.getColumn("IsDomestic"));
                this.data_set       = dataset; return true;
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
                    if (s[1] != null || s[1].Trim().Length > 0)
                    {
                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
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

        public irsGiftsWebSettings()
		{            
			this.TableName      = "TBL_GIFTSWEB_SETTINGS";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT " +
	                                                "SEQ_ID, " +
	                                                "FtpUrl, " +
	                                                "UserName, " +
	                                                "[Password], " +
	                                                "Folder, " +
	                                                "Currency, " +
	                                                "BranchCode, " +
	                                                "BranchPrefix, " +
	                                                "Region, " +
	                                                "Branch, " +
	                                                "AccountOfficer, " +
	                                                "AccOfficerTel, " +
	                                                "AccOfficerEmail, " +
	                                                "ComplianceOfficer, " +
	                                                "CompOfficerTel, " +
	                                                "CompOfficerEmail, " +
                                                    "ExtractRuntime, " +
                                                    "IsDomestic " +
                                                "FROM TBL_GIFTSWEB_SETTINGS " +
                                                "WHERE SEQ_ID = " + id, false);
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
            base.selectedRow = 0;

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool delete_record()
        {
            base.selectedRow = 0;
            if (base.Delete(sqlHelperWs.RETAIN_DATASET) == true)
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
            string[] cols = paramValues.Split('|');
            foreach(string s in cols)
            {
                if (s.Split('~')[0] == "Region")
                {
                    _region = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Branch")
                {
                    _branch = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "FtpUrl")
                {
                    _ftp_url = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "UserName")
                {
                    _username = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Password")
                {
                    _password = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Folder")
                {
                    _folder = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "Currency")
                {
                    _currency = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "BranchCode")
                {
                    _branch_code = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "BranchPrefix")
                {
                    _branch_prefix = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "AccountOfficer")
                {
                    _AccountOfficer = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "AccOfficerTel")
                {
                    _AccOfficerTel = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "AccOfficerEmail")
                {
                    _AccOfficerEmail = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "ComplianceOfficer")
                {
                    _ComplianceOfficer = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "CompOfficerTel")
                {
                    _CompOfficerTel = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "CompOfficerEmail")
                {
                    _CompOfficerEmail = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "ExtractRuntime")
                {
                    _ExtractRuntime = s.Split('~')[1];
                }
                else if (s.Split('~')[0] == "IsDomestic")
                {
                    _IsDomestic = s.Split('~')[1];
                }
            }

            this.TableName = "tbl_giftsweb_settings_tmp";
            this.IdentityColumn = "SEQ_ID";
            string sqlstmt = "SELECT * FROM tbl_giftsweb_settings_tmp WHERE 1=0";
            base.getUpdateDS(sqlstmt);


            if (isUpdatable)
            {
                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    uprow["FtpUrl"] = _ftp_url;
                    uprow["UserName"] = _username;
                    uprow["Password"] = _password;
                    uprow["Folder"] = _folder;
                    uprow["Currency"] = _currency;
                    uprow["BranchCode"] = _branch_code;
                    uprow["BranchPrefix"] = _branch_prefix;
                    uprow["Region"] = _region;
                    uprow["Branch"] = _branch;
                    uprow["AccountOfficer"] = _AccountOfficer;
                    uprow["AccOfficerTel"] = _AccOfficerTel;
                    uprow["AccOfficerEmail"] = _AccOfficerEmail;
                    uprow["ComplianceOfficer"] = _ComplianceOfficer;
                    uprow["CompOfficerTel"] = _CompOfficerTel;
                    uprow["CompOfficerEmail"] = _CompOfficerEmail;
                    uprow["ExtractRuntime"] = _ExtractRuntime;
                    uprow["IsDomestic"] = _IsDomestic;
                    if(actionType != "ADD") {
                        uprow["MAIN_SEQ_ID"] = seq_id;
                    }
                    uprow["Maker"] = userID;
                    uprow["DateAdded"] = dateNow;
                    uprow["CurrStatus"] = "For Approval";
                    uprow["StatusDate"] = dateNow;
                    uprow["ActionType"] = actionType;
                    uprow["Module"] = "Branch Administration >> Giftsweb Settings";
                    uprow["MenuName"] = "Giftsweb Settings";
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
