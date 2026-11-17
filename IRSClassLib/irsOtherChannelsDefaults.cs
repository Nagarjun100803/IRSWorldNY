using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsOtherChannelsDefaults : sqlHelper
    {
        private irsClientData loginData = null;
        private string error_message = string.Empty;

        #region OTHERCHANNELS DEFAULTS
        private int _SEQ_ID = 0;
        #endregion

        #region PUBLIC METHOD
        public string message
        {
            get { return this.error_message; }
        }

        public int SEQ_ID
        {
            get { return this._SEQ_ID; }
            set { this._SEQ_ID = value; }
        }

        public string saveDefaults(string defaultsItems)
        {
            return "Other Channels defaults successfuly saved.";
        }

        public DataSet getDefaults(int FileFormat)
        {
            return getDataSet(getSqlCommand("EXEC SP_GET_OTHERCHANNELS_DEFAULTS " + FileFormat.ToString(), false));
        }

        public DataSet ddRemitCurrencyVal()
        {
            this.loginSession();

            string select_cur = "SELECT CUR.FldCode [CODE], CC.Currency [VALUE] "
                    + "FROM TBL_CURRENCY_CATEGORIES CC LEFT JOIN TBL_LOOKUP CUR ON CC.Currency = CUR.FldValue "
                    + "WHERE BranchCode = '" + loginData.Location + "' AND CurrencyGroup = '2' AND Checked = 1 "
                    + "AND CUR.FldGroup = 'Currency'";

            return getDataSet(getSqlCommand(select_cur, false));
        }

        public DataSet ddTranxSourceVal()
        {
            this.loginSession();

            string selectTranxSource = "SELECT FldDescription [CODE], FldValue [VALUE] FROM TBL_LOOKUP WHERE FldGroup = 'TranxSource'";

            return getDataSet(getSqlCommand(selectTranxSource, false));
        }

        public DataSet ddReceiptModeVal()
        {
            this.loginSession();

            string selectReceiptMode = "SELECT FldDescription [CODE], FldValue [VALUE] FROM TBL_LOOKUP WHERE FldGroup = 'ReceiptMode'";

            return getDataSet(getSqlCommand(selectReceiptMode, false));
        }

        public DataSet ddTranxPurposeVal()
        {
            this.loginSession();

            string selectTranxPurpose = "SELECT FldDescription [CODE], FldValue [VALUE] FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose'";

            return getDataSet(getSqlCommand(selectTranxPurpose, false));
        }

        public DataSet ddSourceFundsVal()
        {
            this.loginSession();

            string selectSourceFunds = "SELECT FldDescription [CODE], FldValue [VALUE] FROM TBL_LOOKUP WHERE FldGroup = 'SourceOfFunds'";

            return getDataSet(getSqlCommand(selectSourceFunds, false));
        }

        public DataSet ddTranxStatVal()
        {
            this.loginSession();

            string selectTranxStat = "SELECT FldDescription [CODE], FldValue [VALUE] FROM TBL_LOOKUP WHERE FldGroup = 'TranxStatus'";

            return getDataSet(getSqlCommand(selectTranxStat, false));
        }

        public bool insertRecord()
        {
            if (base.newRow())
            {
                return true;
            }
            this.getErrorDB();
            return false;
        }

        public bool updateRecord()
        {
            base.IdentityValue = this._SEQ_ID;
            if (base.edit(Convert.ToInt32(base.IdentityValue)))
            {
                return true;
            }
            this.getErrorDB(); return false;
        }

        public bool commitRecord()
        {
            if (base.save())
            {
                this.SEQ_ID = Convert.ToInt32(base.IdentityValue); return true;
            }
            this.getErrorDB(); return false;
        }

        public void rollBack()
        {
            base.cancel();
        }

        public bool defaultsExist(int fileFormat)
        {
            return Convert.ToBoolean(execScalar("SELECT COUNT(1) FROM TBL_OTHERCHANNELS_DEFAULTS WHERE FileFormat = " + fileFormat.ToString()));
        }

        public irsOtherChannelsDefaults()
        {
            this.TableName = "TBL_OTHERCHANNELS_DEFAULTS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand cmdDefaults = getSqlCommand("SELECT * FROM TBL_OTHERCHANNELS_DEFAULTS WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(cmdDefaults);
        }

        public bool set_record(string[] column_values)
        {
            ArrayList errorList = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] != null || s[1].Trim().Length > 0)
                    {
                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            errorList.Add(s[0] + ":" + this.Message);
                        }
                    }
                }
            }

            if (errorList.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object error in errorList)
                {
                    sb.Append((string)error + "\n");
                }

                this.error_message = sb.ToString();

                return false;
            }
            
            return true;
        }

        #endregion 

        #region PRIVATE METHOD
        private void loginSession()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                this.loginData = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        private void getErrorDB()
        {
            this.error_message = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
                this.error_message = base.Message;
        }
        #endregion
    }
}