using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientAccounts : sqlHelperWs
    {
        #region PRIVATE VARIABLES
        private DataSet data_set		= null;
        private string error_message	= null;
        private string _seq_id			= null;
        private string _cifid			= null;
        private string _account_no		= null;
        private string _account_type	= null;
        private string _currency		= null;
        private string _branch_code		= null;
        private string _dom_bank_code	= null;
        private string _remarks			= null;
        private string _status			= null;
        private string _kbi_account		= null;
        private string _otherbank_seqid = null;
		private string _branch_type 	= null;
        public bool ins_flag			= false;
        private string client_cifid = string.Empty;
        private string client_acct_no = string.Empty;
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

        public string account_no
        {
            get { return this._account_no; }
            set { _account_no = value; }
        }

        public string account_type
        {
            get { return this._account_type; }
            set { _account_type = value; }
        }

        public string currency
        {
            get { return this._currency; }
            set { _currency = value; }
        }

        public string branch_code
        {
            get { return this._branch_code; }
            set { _branch_code = value; }
        }

        public string dom_bank_code
        {
            get { return this._dom_bank_code; }
            set { _dom_bank_code = value; }
        }

        public string remarks
        {
            get { return this._remarks; }
            set { _remarks = value; }
        }

        public string status
        {
            get { return this._status; }
            set { _status = value; }
        }

        public string kbi_account
        {
            get { return this._kbi_account; }
            set { _kbi_account = value; }
        }

        public string otherbank_seqid
        {
            get { return this._otherbank_seqid; }
            set { _otherbank_seqid = value; }
        }

		public string branch_type
		{
			get { return this._branch_type; }
			set { _branch_type = value; }
		}
        #endregion

        #region PUBLIC METHOD
        public string _algo_type = null;

        public void ALGO_TYPE(string algo)
        {
            this._algo_type = algo;
        }

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

                _seq_id			= this.getRecord(base.getColumn("SEQ_ID"));
                _cifid			= this.getRecord(base.getColumn("CIFID"));
                _account_no		= this.getRecord(base.getColumn("AccountNo"));
                _account_type	= this.getRecord(base.getColumn("AccountType"));
                _currency		= this.getRecord(base.getColumn("Currency"));
                _branch_code	= this.getRecord(base.getColumn("BranchCode"));
                _dom_bank_code	= this.getRecord(base.getColumn("DomBankCode"));
                _remarks		= this.getRecord(base.getColumn("Remarks"));
                _status			= this.getRecord(base.getColumn("Status"));
                _kbi_account	= this.getRecord(base.getColumn("KBIAccount"));
                _otherbank_seqid= this.getRecord(base.getColumn("OtherBankSeqId"));
				_branch_type	= this.getRecord(base.getColumn("BranchType"));

                this.data_set = dataset; return true;
            }
            return false;
        }

        public bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            /* ADDED BY DDC MAY 12 2016 */
            this.client_cifid = string.Empty;
            this.client_acct_no = string.Empty;
            /* ADDED BY DDC MAY 12 2016 */

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    /* ADDED BY DDC MAY 12 2016 */
                    try
                    {
                        if (s[0].ToString().ToUpper().Equals("CIFID"))
                        {
                            this.client_cifid = s[1].Trim();
                        }

                        if (s[0].ToString().ToUpper().Equals("ACCOUNTNO"))
                        {
                            this.client_acct_no = s[1].Trim();
                        }
                    }
                    catch
                    {
                        this.client_cifid = string.Empty;
                        this.client_acct_no = string.Empty;
                    }
                    /* ADDED BY DDC MAY 12 2016 */

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

        public irsClientAccounts()
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + id, false);
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
                //this.seq_id = base.IdentityValue.ToString(); return true; commented by DDC MAY 12 2016

                /* ADDED BY DDC MAY 12 2016*/
                this.seq_id = this.get_prodset_acct_sequence().ToString(); return true;
                /* ADDED BY DDC MAY 12 2016*/
            }

            this.get_error_dbase(); return false;
        }

        public void rollback_record()
        {
            base.cancel();
        }

        public bool is_gfc(string fcacct, string kbiacct)
        {
            object x = base.execScalar("sp_cif_isAcct_gfc", new string[] { fcacct, kbiacct });
            return Convert.ToBoolean(x);
        }

        public bool is_existing(string param_values)
        {
            string acc = this.count_record(param_values);

            /*
             * checking by person
             String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE CIFID = '" + ((acc.Split('&'))[0]).Trim() + "' AND AccountNo = '" + ((acc.Split('&'))[1]).Trim() + "' OR KBIAccount = '" + ((acc.Split('&'))[1]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";
             * checking by table
             String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE AccountNo = '" + ((acc.Split('&'))[1]).Trim() + "' OR KBIAccount = '" + ((acc.Split('&'))[1]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";
             */

            if (((acc.Split('&'))[1]).Trim().Length != 0 && ((acc.Split('&'))[1]).Trim() != null && ((acc.Split('&'))[1]).Trim().ToUpper() != "NULL")
            {
                String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE CIFID = '" + ((acc.Split('&'))[0]).Trim() + "' AND AccountNo = '" + ((acc.Split('&'))[1]).Trim() + "' OR KBIAccount = '" + ((acc.Split('&'))[1]).Trim() + "' AND ISNULL(Deleted, '') != 'True'";

                return (Convert.ToInt32(execScalar(sql)) > 0);
            }
            return false;
        }

        public string count_record(string param_values)
        {
            string[] cols = param_values.Split('|');
            string acc_no = null;
            string acc_cifid = null;
            string kbi_acct = null;

            for (int i = 0; i < cols.Length; i++)
            {
                string[] vals = cols[i].Split('~');

                if (vals[0].ToUpper() == "CIFID") acc_cifid = (cols[i].Split('~'))[1];
                if (vals[0].ToUpper() == "ACCOUNTNO") acc_no = (cols[i].Split('~'))[1];
                if (vals[0].ToUpper() == "KBIACCOUNT") kbi_acct = (cols[i].Split('~'))[1];
            }

            return acc_cifid + '&' + acc_no + '&' + kbi_acct;
        }

        public int get_seqid_existing_account(string param_values)
        {
            string acc = this.count_record(param_values);

            //String sql = "SELECT SEQ_ID FROM " + TableName + " WHERE CIFID = '" + ((acc.Split('&'))[0]).Trim() + "' AND AccountNo = '" + ((acc.Split('&'))[1]).Trim() + "'";
            String sql = "SELECT SEQ_ID FROM " + TableName + " WHERE AccountNo = '" + ((acc.Split('&'))[1]).Trim() + "'";

            return (Convert.ToInt32(execScalar(sql)));
        }

        public DataTable get_account(String cifid, String acctno)
        {
            String sql = "SELECT * FROM " + TableName + " WHERE CIFID = '" + cifid.Trim() + "' AND AccountNo = '" + acctno.Trim() + "'";
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];

            return null;
        }

        public int getCurrency(string currshort)
        {
            String sql = "SELECT FLDVALUE FROM TBL_LOOKUP WHERE UPPER(FLDGROUP) = 'CURRENCY' AND UPPER(FLDCODE) = '" + currshort.Trim().ToUpper() + "'";
            return Convert.ToInt32(execScalar(sql));
        }

        public string get_acctdetails(string acctno)
        {
            irsAccountInquiry iai = new irsAccountInquiry();
            iai.IsWinApp = base.IsWinApp;

            string acctStatus = iai.getAccountStatus(acctno);

            irsClientMaster cm = new irsClientMaster();
            cm.IsWinApp = base.IsWinApp;

            string acctCur = "";
            string acctTyp = "";
            string branchOfAcct = "";
            string acctStat = "";
            string accno = "";
            string[] arr;
            string ret = string.Empty;

            arr = acctStatus.Split('|');

            /* RFS BLOCK TIME DEPOSIT */
            if (arr.Length > 3)
            {
                acctCur = cm.get_lookup_fldvalue(arr[3], "CURRENCY");
                acctTyp = cm.get_lookup_fldvalue(arr[2], "ACCOUNTTYPE");
                /* PNBS Integration Project */
                branchOfAcct = arr[1];

                if (arr[6].ToString() == "1")
                {
                    acctStat = cm.get_lookup_fldvalue(arr[0], "ACCOUNTSTATUSST");
                }
                else
                {
                    acctStat = cm.get_lookup_fldvalue(arr[0], "ACCOUNTSTATUSIM");
                }
                accno = arr[4];

                /* PNBS Integration Project */
                ret = acctCur + '~' + acctTyp + '~' + acctStat + '~' + branchOfAcct + '~' + accno + '~' + arr[5].ToString() + "~0=";

                int i = 0;
                DataTable dt = new irsBranchDefaults().getDefaultSetup(this.getLocation());
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        if (dt.Rows[i][1].ToString().ToUpper() == "FC INQUIRY NAME")
                        {
                            if (dt.Rows[i][2].ToString() == "1")
                            {
                                /* PNBS Integration Project */
                                ret = acctCur + '~' + acctTyp + '~' + acctStat + '~' + branchOfAcct + '~' + accno + '~' + arr[5].ToString() + "~1=" + arr[6].ToString();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    ret = "Failed to get account details. Error in getting account details from core banking.";
                }
            }
            else
            {
                ret = "Failed to get account details. Error in getting account details from core banking.";
            }
            return ret;
        }

        private string getLocation()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
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

        /* ADDED BY DDC MAY 12 2016 */
        private int get_prodset_acct_sequence()
        {
            DataTable dt = null;

            try
            {
                if (string.IsNullOrEmpty(this.client_cifid).Equals(false) && string.IsNullOrEmpty(this.client_acct_no).Equals(false))
                {
                    dt = base.getDataSet("select max(seq_id) as seq_id from tbl_cif_accounts where isnull(deleted, 0) = 0 and cifid = '" + this.client_cifid.Trim().Replace("'","") + "' and accountno = '" + this.client_acct_no.Trim().Replace("'","") + "'", false, new object[] { }).Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dt.Rows[0]["seq_id"]);
                    }
                }

                return Convert.ToInt32(base.IdentityValue);

            }
            catch
            {
                return Convert.ToInt32(base.IdentityValue);
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }
        /* ADDED BY DDC MAY 12 2016 */
        #endregion
    }
}