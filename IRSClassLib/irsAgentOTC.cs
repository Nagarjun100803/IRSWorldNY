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
    public class irsAgentOTC : irsSqlCommons
    {
        public enum Trans_Status : int
        {
            Credited_Online = 2,
            Posted_Central = 5,
            On_Hold = 14,
            For_Approval = 15,
            For_Reprocess = 9,
            Cancel_Not_Implemented = 11,
            Cancel_Implemented = 12,
            For_IBATCH_Processing = 16


        }
        #region PRIVATE VARIABLES & METHODS

        private String sysid = "IRSWorldAgent";
        private String mdesc = "Test Connection to FC";
        private String failedmsg = "FAILED TO CREDIT **";
        private String error_message = null;
        private String _api_message = null;
        private String _ok = String.Empty;
        private String _failed = String.Empty;        
        private String maxthread;        
        WinAppLogger oLog = new WinAppLogger();
        irsThreadManager tmanager;

        private DataTable Get_data()
        {
            DataSet ds = null;
            try
            {
                base.EnforceConstraint = false;
                SqlCommand sqcommands = base.getSqlCommand("sp_irsAgent_getdata_for_crediting_OTC");
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
        private void update_txn(int txnid, int txnstatus, String fc_message, DateTime dtImplemOverseas, DateTime dtImplemLocal)
        {
            object _o;

            try
            {
                _o = base.execScalar("sp_irsAgent_update_for_credit", new Object[] { txnid, txnstatus, fc_message, dtImplemOverseas, dtImplemLocal });
                if (_o == null)
                { messages = base.Message; }
            }
            catch (Exception er)
            {
                oLog.writeAudit(sysid, "fccredit.update_txn", mdesc, er.Message.ToString(),
                                  "", WinAppLogger.Category.FlexCube);
                throw er;
            }
        }    
        private String GetPAWAccount(int currency)
        {
            DataTable dt1 = execStoredProcedureTable("GetPAWAccount", new object[] { currency });
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                DataRow row = dt1.Rows[0];
                object[] MSG = row.ItemArray;
                return MSG[0].ToString();
            }
            return "";
        }
        private String GetApasAccount(int currency)
        {
            DataTable dt1 = execStoredProcedureTable("GetApasOnlineAccount", new object[] { currency });
            if (dt1 != null && dt1.Rows.Count > 0)
            {
                DataRow row = dt1.Rows[0];
                object[] MSG = row.ItemArray;
                return MSG[0].ToString();
            }
            return "";
        }
        private bool testConnection()
        {
            String testError = String.Empty;
            String a = String.Empty;                
            DataTable dtTest = null;
            csFlexcubeTeller ofc = new csFlexcubeTeller();
            Hashtable ht = new Hashtable();

            dtTest = this.AgentTestFCAccount();
            try
            {
                if (dtTest != null)
                {
                    if (dtTest.Rows.Count > 0)
                    { a = dtTest.Rows[0][0].ToString(); }
                    else
                    { a = "226436100016"; }                    
                    ht = ofc.getAccountAttributes(a.Trim(), out testError);
                    if (ht == null)
                    {
                        oLog.writeAudit(sysid, "irsAgent.start_credit", mdesc, testError.ToString(), "", WinAppLogger.Category.FlexCube);
                        ht = null;
                        this.messages = testError.ToString();
                        Thread.Sleep(30000);
                        return false;
                    }                    
                }
            }
            catch(Exception er) 
            {
                this.messages = er.StackTrace.ToString();
                return false; 
            }
            finally
            {
                if (dtTest != null)
                {
                    dtTest.Dispose();
                    dtTest = null;
                }
                if (ofc != null)
                {
                    ofc = null;
                }
                if (ht != null)
                {
                    ht = null;
                }
            }
            return true;
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
        public irsAgentOTC()
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
            String curr = String.Empty;
            String FileFormat = String.Empty;
            String actCur = String.Empty;
            String rembrcode = String.Empty;            
            DataTable dt = null;

            try
            {                
                dt = this.Get_data();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        //  start    : Test module
                        if (!this.testConnection()) { return false; }                        
                        //  end      : Test module
                    
                        int seqid = 0;
                        string targetacct2 = String.Empty;
                        foreach (DataRow row in dt.Rows)
                        {

                            if ((bool)ThreadKill.Thread_Kill) { tmanager.KillAllThreads(); break; }
                            if ((int)seqid == Convert.ToInt32(row["trxnid"])) { continue; }
                            if ((String)targetacct2 == (String)row["TargetAccount"].ToString()) { Thread.Sleep(2000); }

                            seqid = Convert.ToInt32(row["trxnid"]);
                            targetacct2 = (String)row["TargetAccount"].ToString();

                            refnum = (String)row["ReferenceNo"].ToString();
                            sourceAcct = (String)row["SourceAccount"].ToString();
                            targetAcct = (String)row["TargetAccount"].ToString();
                            amt = (String)row["NettoBeneAmount"].ToString();
                            narrative = (String)row["narrative"].ToString();
                            trxnid = Convert.ToInt32(row["trxnid"]);
                            txn_date = (DateTime)row["TranxDate"];
                            prodcode = Convert.ToInt32(row["ProductCode"]);
                            curr = Convert.ToString(row["NettoBeneCurrency"]);
                            FileFormat = (String)row["FileFormat"].ToString();
                            actCur = (String)row["actCur"].ToString();
                            rembrcode = (String)row["RemittingBranch"].ToString();
                            if (prodcode == (int)ProductCode.PAW)
                            { targetAcct = this.GetPAWAccount(Convert.ToInt32(curr)); }
                            if (prodcode == (int)ProductCode.APASOnline)
                            { targetAcct = this.GetApasAccount(Convert.ToInt32(curr)); }
                            api_message = String.Empty;

                            if (sourceAcct != null && sourceAcct != "" && targetAcct != "")
                            {
                                while(tmanager.isThreadExists(sourceAcct)) { Thread.Sleep(50); }
                                fccredit fc = new fccredit(refnum, targetAcct, sourceAcct, amt, curr, trxnid, txn_date, refnum, prodcode, rembrcode);
                               //fc.do_fundtransfer();

                                Thread t = new Thread(fc.do_fundtransfer);
                                t.Name = sourceAcct;
                                tmanager.addThread(t); 
                                t.Start();                                
                            }
                            else 
                            {
                                String s = failedmsg + "Target Account or Nostro/Vostro Account is missing. Please verify.";
                                this.update_txn(trxnid, (int)Trans_Status.For_Reprocess, s, txn_date, txn_date);
                            }
                            
                        }
                    }
                }
                if (messages == null)
                {
                    ibool = true;
                }
                while (tmanager.checkRunningThread()) 
                { 
                    if ((bool)ThreadKill.Thread_Kill) 
                    { 
                        tmanager.KillAllThreads(); 
                        break; 
                    } 
                    Thread.Sleep(500); 
                }
            }
            catch (Exception er)
            {
                throw er;
            }
            finally
            {                
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
        #endregion
    }   
}

