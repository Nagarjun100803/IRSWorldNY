using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using GRSMQ;

namespace IRSClassLib
{
    public class irsAgent : irsSqlCommons
    {
        #region PRIVATE VARIABLES & METHODS

        private String sysid = "IRSWorldAgent";
        private String mdesc = "Test Connection to FC";
        private String error_message = null;
        private String _api_message = null;
        private String _ok = String.Empty;
        private String _failed = String.Empty;
        private String maxthread;
        csFlexcubeTeller ofc = new csFlexcubeTeller();
        WinAppLogger oLog = new WinAppLogger();
        irsThreadManager tmanager;

        private DataTable Get_data()
        {
            DataSet ds = null;
            try
            {
                base.EnforceConstraint = false;
                SqlCommand sqcommands = base.getSqlCommand("sp_irsAgent_getdata_for_crediting");
                ds = base.getDataSet(sqcommands, "FOR_CREDITTING");
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch
            {
                this.get_db_err();
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
            return null;
        }
        private DataTable AgentTestFCAccount()
        {
            DataSet ds = null;
            SqlCommand sqcommands = null;
            try
            {
                base.EnforceConstraint = false;
                sqcommands = base.getSqlCommand("sp_irsAgent_GetAgentTestFCAccount");
                ds = base.getDataSet(sqcommands, "TestAccount");
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch
            {
                this.get_db_err();
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
            return null;
        }
        private void update_txn(object[] ob)
        {
            object _o;
            _o = base.execScalar("sp_irsAgent_update_for_credit", ob);
            if (_o == null)
            { messages = base.Message; }
        }
        private String GetPAWAccount(int currency)
        {
            DataTable dt = execStoredProcedureTable("GetPAWAccount", new object[] { currency });
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                object[] MSG = row.ItemArray;
                return MSG[0].ToString();
            }
            return "";
        }
        private String GetApasAccount(int currency)
        {
            DataTable dt = execStoredProcedureTable("GetApasOnlineAccount", new object[] { currency });
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                object[] MSG = row.ItemArray;
                return MSG[0].ToString();
            }
            return "";
        }

        #endregion

        #region PROPERTY GET & SET
        public String messages
        {
            get { return this.error_message; }
            set { this.error_message = value; }
        }

        public String api_message
        {
            get { return this._api_message; }
            set { this._api_message = value; }
        }

        public String max_thread
        { set { maxthread = value; } }

        private String ok
        { get { return _ok; } }

        private String failed
        { get { return _failed; } }


        #endregion PROPERTY GET & SET

        #region PUBLIC METHODS
        public irsAgent()
        {
            this.error_message = null;
            base.IsWinApp = true;
        }

        public void get_db_err()
        {
            messages = String.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                messages = base.Message;
            }
        }
        public bool start_credit()
        {
            tmanager = new irsThreadManager(int.Parse(maxthread));
            tmanager.MaxThread = int.Parse(maxthread);
            String refnum = String.Empty;
            String sourceAcct = String.Empty;
            String targetAcct = String.Empty;
            String amt = String.Empty;
            String narrative = String.Empty;
            int trxnid = 0;
            bool ibool = false;
            int prodcode = 0;
            DateTime txn_date = DateTime.Now;
            int curr = 0;
            String FileFormat = String.Empty;
            String actCur = String.Empty;
            DataTable dtTest = null;
            DataTable dt = null;
            try
            {
                String testError = String.Empty;
                String a = String.Empty;
                dtTest = this.AgentTestFCAccount();
                dt = this.Get_data();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        //  start    : Test module
                        if (dtTest != null)
                        {
                            if (dtTest.Rows.Count > 0)
                            { a = dtTest.Rows[0][0].ToString(); }
                            else
                            { a = "226436100016"; }
                            Hashtable ht = new Hashtable();
                            ht = ofc.getAccountAttributes(a.Trim(), out testError);
                            if (ht == null)
                            {
                                oLog.writeAudit(sysid, "irsAgent.start_credit", mdesc, testError.ToString(),
                                      "", WinAppLogger.Category.FlexCube);
                                ht = null;
                                return false;
                            }
                            ht = null;
                        }
                        //  end      : Test module
                    }
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            refnum = (String)row["ReferenceNo"].ToString();
                            sourceAcct = (String)row["SourceAccount"].ToString();
                            targetAcct = (String)row["TargetAccount"].ToString();
                            amt = (String)row["NettoBeneAmount"].ToString();
                            narrative = (String)row["narrative"].ToString();
                            trxnid = Convert.ToInt32(row["trxnid"]);
                            txn_date = (DateTime)row["TranxDate"];
                            prodcode = Convert.ToInt32(row["ProductCode"]);
                            curr = Convert.ToInt32(row["NettoBeneCurrency"]);
                            FileFormat = (String)row["FileFormat"].ToString();
                            actCur = (String)row["actCur"].ToString();

                            if (prodcode == (int)ProductCode.PAW)
                            { targetAcct = this.GetPAWAccount(curr); }
                            if (prodcode == (int)ProductCode.APASOnline)
                            { targetAcct = this.GetApasAccount(curr); }
                            api_message = String.Empty;

                            while (tmanager.isThreadExists(sourceAcct)) { }
                            fccredit fc = new fccredit(refnum, targetAcct, sourceAcct, amt, actCur, trxnid, txn_date, refnum, prodcode, String.Empty);
                            Thread t = new Thread(fc.do_iBatch_credit);
                            t.Name = sourceAcct;
                            if (tmanager.isThreadExists(sourceAcct) == false) { tmanager.addThread(t); }
                            t.Start();

                        }
                    }
                }
                if (messages == null)
                {
                    ibool = true;
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {
                if (dtTest != null)
                {
                    dtTest.Dispose();
                    dtTest = null;
                }
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if (tmanager != null)
                {
                    tmanager = null;
                }                
            }
            return ibool;
        }

        public bool fund_transfer(String refnum, String sourceAcct, String targetAcct, String amt, String narative)
        {
            String fc_msg = String.Empty;
            bool result = false;
            try
            {
                result = ofc.fundTransfer(refnum, sourceAcct, targetAcct, amt, narative, out fc_msg);
            }
            catch (Exception e)
            {
                messages = e.Message + ' ' + e.StackTrace;
            }

            api_message = fc_msg;
            return result;
        }
        public bool do_fundtransfer(String refnum, String sourceAcct, String targetAcct, String amt, String narrative, int trxnid, DateTime txn_date)
        {
            object[] ob = new object[4];
            int i = 0;
            bool ibool = false;
            String s = String.Empty;
            if (ibool = this.fund_transfer(refnum, sourceAcct, targetAcct, amt, narrative))
            { i = 1; s = this.ok.ToString(); }
            else { s = this.failed.ToString(); }
            ob[0] = trxnid;
            ob[1] = i;
            ob[2] = txn_date;
            ob[3] = s + api_message + messages;
            this.update_txn(ob);
            return ibool;
        }
        #endregion
    }
}

