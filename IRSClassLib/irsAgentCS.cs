using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.IO;
using System.Net;

namespace IRSClassLib
{
    /* E2EAPICHANGE 20220209 Added E2EWS */
    public class E2EWS
    {
        public enum E2EConfiguration
        {
            Channel,
            TransactionBranch,
            TransactionType
        }

        public string GetE2EConfiguration(E2EConfiguration config)
        {
            string ret = string.Empty;
            switch (config)
            {
                case E2EConfiguration.Channel:
                    ret = System.Configuration.ConfigurationManager.AppSettings["E2EWS_CHANNEL"];
                    break;

                case E2EConfiguration.TransactionBranch:
                    ret = System.Configuration.ConfigurationManager.AppSettings["E2EWS_TRANSACTIONBRANCH"];
                    break;

                case E2EConfiguration.TransactionType:
                    ret = System.Configuration.ConfigurationManager.AppSettings["E2EWS_TRANSACTIONTYPE"];
                    break;
            }
            return ret;
        }

        public string GenerateUserRef(string accountNo)
        {
            return this.GetE2EConfiguration(E2EConfiguration.Channel) + "_" + accountNo + "_" + DateTime.Now.ToString("yyyyMMddhhMMss");
        }

        public bool E2ESuccess(DataSet ds, out string res)
        {
            int i = 0;
            int j = 0;
            int k = 0;
            bool ret = false;
            res = string.Empty;

            if (ds != null && ds.Tables.Count > 0)
            {
                for (i = 0; i <= (ds.Tables.Count - 1); i++)
                {
                    if (!ret)
                    {
                        for (j = 0; j <= (ds.Tables[i].Rows.Count - 1); j++)
                        {
                            if (!ret)
                            {
                                for (k = 0; k <= (ds.Tables[i].Columns.Count - 1); k++)
                                {
                                    if (ds.Tables[i].Columns[k].ToString().Trim().ToLower() == "replycode")
                                    {
                                        res = ds.Tables[i].Rows[j][0].ToString() + "~" + ds.Tables[i].Rows[j][1].ToString();
                                        if (ds.Tables[i].Rows[j][k].ToString() == "0")
                                        {
                                            res = string.Empty;
                                            ret = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }
    }

    /* E2EAPICHANGE 20220209 Updated Name */
    public class SIcredit : irsSqlCommons
    {
        /* E2EAPICHANGE 20220209 Added _e2e */
        E2EAPIConnector.E2EAPIConnector _e2e = new E2EAPIConnector.E2EAPIConnector();

        public enum Trans_Status : int
        {
            Credited_Online = 2,
            Posted_Central = 5,
            On_Hold = 14,
            For_Approval = 15,
            For_Reprocess = 9,
            Cancel_Not_Implemented = 11,
            Cancel_Implemented = 12,
            For_IBATCH_Processing = 16,
            Rejected = 256
        }

        public enum ProductCode : int
        {
            RapidRemit = 10,
            GFC = 22,
            WealthMultiplier = 14,
            PAW = 24,
            APASOnline = 27
        }

        #region variables
        String _sysid = "IRSWorldAgent";
        String _mdesc = "Crediting to SI";
        String _error_message = null;
        String _api_message = null;
        String _ok = "ACCOUNT PROCESSED **";
        String _failed = "Error (SI):";
        String _refnum = String.Empty;
        String _targetAcct = String.Empty;
        String _amt = String.Empty;
        String _sourceAcct = String.Empty;
        String _actCur = String.Empty;
        String _rembrcode = String.Empty;
        /* E2EAPICHANGE 20220209 Removed _mobileNo */
        String _remitmethod = String.Empty;
        String _originalAccount = string.Empty;
        String _narrative = String.Empty;

        int _trxnid = 0;
        int _productcode = 0;
        DateTime _txn_date = DateTime.Now;

        /* E2EAPICHANGE 20220209 Removed csFlexcubeTeller */
        WinAppLogger _oLog = new WinAppLogger();
        irsTimeZone tZone = new irsTimeZone();
        static Object _lockme = new object();
        private String _logid = "logID=";
        #endregion variables

        #region common public method
        /* E2EAPICHANGE 20220209 Updated Code */
        public SIcredit()
        { }

        /* E2EAPICHANGE 20220209 Updated Function */
        public SIcredit(String refnum, String targetAcct, String sourceAcct, String amt, String actCur, int trxnid, DateTime txn_date, String narrative, int productcode, String rembrcode, String remitmethod, String originalAccount)
        {
            this._refnum = refnum;
            this._targetAcct = targetAcct;
            this._sourceAcct = sourceAcct;
            this._amt = amt;
            this._actCur = actCur;
            this._refnum = refnum;
            this._trxnid = trxnid;
            this._txn_date = txn_date;
            this._narrative = "IRSW (" + narrative + ")";
            this._productcode = productcode;
            this._rembrcode = rembrcode;
            this._remitmethod = remitmethod;
            this._originalAccount = originalAccount;
            this._error_message = null;
            base.IsWinApp = true;
        }
        #endregion common public method

        /* E2EAPICHANGE 20220209 Removed IBatch Region */

        #region for OTC transactions **************************************
        public bool fund_transfer(String refnum, String sourceAcct, String targetAcct, String amt, String narrative)
        {
            /* E2EAPICHANGE 20220209 Updated Code */
            string err = string.Empty;
            string e2eres = string.Empty;
            bool result = false;

            try
            {
                string tranCode = System.Configuration.ConfigurationManager.AppSettings["DEBIT_TRANCODE"];
                string kbi_branchcode = System.Configuration.ConfigurationManager.AppSettings["KBI_BRANCH"];

                if (targetAcct.Length <= 10 && refnum.Substring(1, 3) == kbi_branchcode)
                {
                    DataTable dtKBI = execStoredProcedureTable("getKBItoFC", new object[] { targetAcct });
                    if (dtKBI != null && dtKBI.Rows.Count > 0)
                    {
                        targetAcct = dtKBI.Rows[0]["Flex_Acct"].ToString();
                    }
                }
                E2EWS e2e = new E2EWS();
                string[] param = new string[]
                {
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.Channel),
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionBranch),
                    sourceAcct.Trim(),
                    targetAcct.Trim(),
                    amt,
                    narrative,
                    refnum,
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionType),
                    "", ""
                };
                DataSet ds = _e2e.connect2E2E(refnum, E2EAPIConnector.E2EAPIConnector.APIMethod.FundTransfer, E2EAPIConnector.E2EAPIConnector.MediaType.json, param, out result, out err);
                result = (e2e.E2ESuccess(ds, out e2eres));
                if (e2eres == string.Empty)
                {
                    e2eres = refnum;
                }
            }
            catch (Exception ex)
            {
                this.messages = "EXCEPTION=[" + ex.Message + "] STACKTRACE=[" + ex.StackTrace + "] E2ERES=[" + e2eres + "]";
            }
            finally
            {
                if (err != string.Empty)
                {
                    api_message = err + "|" + e2eres;
                }
                else
                {
                    api_message = e2eres;
                }
            }
            return result;
        }

        /* E2EAPICHANGE 20220209 Added Function */
        public bool fund_transfer(String refnum, String sourceAcct, String targetAcct, String amt, String narrative, out string ret)
        {
            string err = string.Empty;
            string e2eres = string.Empty;
            bool result = false;
            ret = string.Empty;

            try
            {
                string tranCode = System.Configuration.ConfigurationManager.AppSettings["DEBIT_TRANCODE"];
                string kbi_branchcode = System.Configuration.ConfigurationManager.AppSettings["KBI_BRANCH"];

                if (targetAcct.Length <= 10 && refnum.Substring(1, 3) == kbi_branchcode)
                {
                    DataTable dtKBI = execStoredProcedureTable("getKBItoFC", new object[] { targetAcct });
                    if (dtKBI != null && dtKBI.Rows.Count > 0)
                    {
                        targetAcct = dtKBI.Rows[0]["Flex_Acct"].ToString();
                    }
                }

                E2EWS e2e = new E2EWS();
                string[] param = new string[]
                {
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.Channel),
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionBranch),
                    sourceAcct.Trim(),
                    targetAcct.Trim(),
                    amt,
                    narrative,
                    refnum,
                    e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionType),
                    "", ""
                };
                DataSet ds = _e2e.connect2E2E(refnum, E2EAPIConnector.E2EAPIConnector.APIMethod.FundTransfer, E2EAPIConnector.E2EAPIConnector.MediaType.json, param, out result, out err);
                result = (e2e.E2ESuccess(ds, out e2eres));
                if (e2eres == string.Empty)
                {
                    e2eres = refnum;
                }
            }
            catch (Exception ex)
            {
                this.messages = "EXCEPTION=[" + ex.Message + "] STACKTRACE=[" + ex.StackTrace + "] E2ERES=[" + e2eres + "]";
            }
            finally
            {
                if (err != string.Empty)
                {
                    api_message = err + "|" + e2eres;
                }
                else
                {
                    api_message = e2eres;
                }
                ret = e2eres;
            }
            return result;
        }

        public void do_FundTransfer()
        {
            int i = 0;
            String s = String.Empty;
            DateTime dtImplemOverseas = Convert.ToDateTime(tZone.branchLocalTime_nWs(_rembrcode));
            DateTime dtImplemLocal = DateTime.Now;
            irsLinkToGFC gfc = new irsLinkToGFC(true);
            irsOldAPASWinApp apas = new irsOldAPASWinApp();
            String logid = _logid + _trxnid.ToString();

            try
            {
                if (this.fund_transfer(_refnum, _sourceAcct, _targetAcct, _amt, _narrative))
                {
                    if (this._remitmethod == "6" || this._remitmethod == "8" || this._remitmethod == "10")
                    {
                        if (this._productcode == (int)ProductCode.APASOnline || this._productcode == (int)ProductCode.GFC || this._productcode == (int)ProductCode.PAW || this._productcode == (int)ProductCode.RapidRemit)
                        {
                            i = (int)Trans_Status.Credited_Online;
                            s = this.ok.ToString();
                        }
                        else
                        {
                            i = (int)Trans_Status.For_IBATCH_Processing;
                            s = this.ok.ToString();
                        }
                    }
                    else
                    {
                        i = (int)Trans_Status.Credited_Online;
                        s = this.ok.ToString();
                    }
                }
                else
                {
                    if (this._remitmethod == "6" || this._remitmethod == "8" || this._remitmethod == "10")
                    {
                        /* E2EAPICHANGE 20220209 Removed MQRC */
                        if ((this.failed.ToString() != "" || this.api_message.ToString() != ""))
                        {
                            i = (int)Trans_Status.For_Reprocess;
                            s = "";
                        }
                        else if (this.api_message.ToString().IndexOf("Transaction already serviced") >= 0)
                        {
                            if (this._productcode == (int)ProductCode.APASOnline || this._productcode == (int)ProductCode.GFC || this._productcode == (int)ProductCode.PAW || this._productcode == (int)ProductCode.RapidRemit)
                            {
                                i = (int)Trans_Status.Credited_Online;
                                s = this.ok.ToString();
                            }
                            else
                            {
                                i = (int)Trans_Status.For_IBATCH_Processing;
                                s = this.ok.ToString();
                            }
                        }
                        else
                        {
                            i = (int)Trans_Status.Rejected;
                            s = this.failed.ToString();
                        }
                    }
                    else
                    {
                        i = (int)Trans_Status.For_Reprocess;
                        s = this.failed.ToString();
                    }
                }
                String details = "refnum=[" + _refnum + "]targetAcct=[" + _targetAcct + "]sourceAcct=[" + _sourceAcct + "]amt=[" + _amt + "]narrative=[" + _narrative + "]" + logid;
                String apmsg = s + api_message + "CKSOURCE[" + _sourceAcct + "]CKTARGET[" + _targetAcct + "]" + this.messages;
                String ss = "api_msg=[" + s + api_message + this.messages + "]datetime:[" + DateTime.Now.ToString() + "]" + details;

                if (this.ifOBPExcess(_trxnid))
                {
                    if (this.isSSSRoundUp(_rembrcode))
                    {
                        String OBPExcess = this.getOBPSdetails(_trxnid);
                        String OBPRefno = "O" + _refnum.ToString().Substring(1);
                        String OBPNSVS = OBPExcess.Split('|')[2].ToString();
                        String OBPAccount = OBPExcess.Split('|')[1].ToString();
                        String OBPAmount = OBPExcess.Split('|')[4].ToString();
                        String OBPNarrative = "IRSW (" + OBPRefno + ")";
                        if (Convert.ToDouble(OBPAmount) > 0)
                        {
                            /* E2EAPICHANGE 20220209 Removed csFlexcubeTeller */
                            if (this.fund_transfer(OBPRefno, OBPNSVS, OBPAccount, OBPAmount, OBPNarrative))
                            {
                                i = (int)Trans_Status.Credited_Online;
                                s = this.ok.ToString();
                            }
                            else
                            {
                                i = (int)Trans_Status.For_Reprocess;
                                s = this.failed.ToString();
                            }
                            String apmsg2 = apmsg + " |OBP EXCESS PAY:[" + s + api_message + "]OBPNSVS[" + OBPNSVS + "]OBPAccount[" + OBPAccount + this.messages + "]";
                            String ss2 = apmsg + " |OBP EXCESS PAY: api_msg=[" + s + api_message + this.messages + "]datetime:[" + DateTime.Now.ToString() + "]" + details;

                            _oLog.writeAudit(_sysid, "SIcredit.do_FundTransfer.ifOBPExcess", _mdesc, ss2 + "", "", WinAppLogger.Category.SI);
                            this.getUpdate_Txn(_trxnid, i, apmsg2, dtImplemOverseas, dtImplemLocal);
                            this.logMe(ss2);
                            apmsg2 = null;
                            ss2 = null;
                        }
                        else
                        {
                            _oLog.writeAudit(_sysid, "SIcredit.ifOBPExcess", _mdesc, ss + "", "", WinAppLogger.Category.SI);
                            this.getUpdate_Txn(_trxnid, i, apmsg, dtImplemOverseas, dtImplemLocal);
                            this.logMe(ss);
                        }
                    }
                    else
                    {
                        _oLog.writeAudit(_sysid, "SIcredit.ifOBPExcess", _mdesc, ss + "", "", WinAppLogger.Category.SI);
                        this.getUpdate_Txn(_trxnid, i, apmsg, dtImplemOverseas, dtImplemLocal);
                        this.logMe(ss);
                    }
                }
                else
                {
                    _oLog.writeAudit(_sysid, "SIcredit.do_FundTransfer", _mdesc, ss + "", "", WinAppLogger.Category.SI);
                    this.getUpdate_Txn(_trxnid, i, apmsg, dtImplemOverseas, dtImplemLocal);

                    if (i == (int)Trans_Status.Credited_Online)
                    {
                        if ((int)_productcode == (int)ProductCode.APASOnline) // check for APAS
                        {
                            if (!apas.InsertApas(_trxnid))
                            {
                                _oLog.writeAudit(_sysid, "SIcredit.InsertApas", _mdesc + "Error: " + apas.APASMessage.ToString(), "", "", WinAppLogger.Category.SI);
                            }
                            else
                            {
                                _oLog.writeAudit(_sysid, "SIcredit.InsertApas", _mdesc + "Successfully sent to APAS DB." + details, "", "", WinAppLogger.Category.SI);
                            }

                        }
                        else if ((int)_productcode == (int)ProductCode.PAW) // check for PAW
                        {
                            if (!apas.InsertAnPAW(_trxnid))
                            {
                                _oLog.writeAudit(_sysid, "SIcredit.InsertAnPAW", _mdesc + "Error: " + apas.Message.ToString(), "", "", WinAppLogger.Category.SI);
                            }
                            else
                            {
                                _oLog.writeAudit(_sysid, "SIcredit.InsertAnPAW", _mdesc + "Successfully sent to APAS DB." + details, "", "", WinAppLogger.Category.SI);
                            }
                        }
                    }
                    this.logMe(ss);
                }
                apmsg = null;
                ss = null;
                _oLog.writeAudit(_sysid, "SIcredit.do_FundTransfer", "Thread Start", "Thread name " + Thread.CurrentThread.Name.ToString() + " stopped." + logid, "", WinAppLogger.Category.SI);
            }
            catch (Exception e)
            {
                _oLog.writeAudit(_sysid, "SIcredit.do_FundTransfer", _mdesc, "Error:" + e.StackTrace.ToString() + ". " + logid, "", WinAppLogger.Category.SI);
            }
            finally
            {
                if (gfc != null)
                {
                    gfc = null;
                }
                if (apas != null)
                {
                    apas = null;
                }
                if (_oLog != null)
                {
                    _oLog = null;
                }
                if (tZone != null)
                {
                    tZone = null;
                }
            }
        }

        private String getOBPSdetails(int trxnid)
        {
            DataTable dt = null;
            try
            {
                dt = base.execStoredProcedureTable("spOBPExcess", new object[] { trxnid });
                return dt.Rows[0]["MerchantRefNo"].ToString();
            }
            catch (Exception e)
            {
                this.messages = e.Message + ' ' + e.StackTrace;
                return String.Empty;
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

        private bool isSSSRoundUp(String brCode)
        {
            DataTable dt = null;
            try
            {
                dt = base.execStoredProcedureTable("spGetBranchDefaultSetup", new object[] { brCode });
                if (dt != null)
                {
                    if (dt.Rows[0]["RoundUpOBPSAmount"].ToString() == "1") { return true; } else { return false; }
                }
                return false;
            }
            catch (Exception e)
            {
                this.messages = e.Message + ' ' + e.StackTrace;
                return false;
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

        private bool ifOBPExcess(int TranxID)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("spOBPExcess", new object[] { TranxID });
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MERCHANTCODE"].ToString() != "SSS")
                    {
                        return false;
                    }
                    _oLog.writeAudit(this._sysid, "OBPS Transaction", "fOBPExcess", "", TranxID.ToString(), WinAppLogger.Category.Select);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _oLog.writeAudit(this._sysid, "OBPS Transaction", "fOBPExcess", ex.Message, TranxID.ToString(), WinAppLogger.Category.Select);
                throw ex;
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return false;
        }
        #endregion for OTC transactions **************************************

        #region common methods
        private void getUpdate_Txn(int txnid, int txnstatus, String fc_message, DateTime dtImplemOverseas, DateTime dtImplemLocal)
        {
            int ra = 0;
            /* E2EAPICHANGE 20220209 Updated s */
            String s = "~seq_id=" + txnid.ToString() + "~transtatus=" + txnstatus.ToString() + "~SI_msg=" + fc_message + "|" + dtImplemOverseas + "|" + dtImplemLocal;
            try
            {
                ra = base.execStoredProcedureUpdate("sp_irsAgent_update_for_credit", new Object[] { txnid, txnstatus, fc_message, dtImplemOverseas, dtImplemLocal });
                if (ra == -1)
                {
                    _oLog.writeAudit(_sysid, "SIcredit.getUpdate_Txn", _mdesc, "Error while executing SP sp_irsAgent_update_for_credit :logID=" + txnid.ToString() + " : " + base.Message.ToString() + s,
                                    "", WinAppLogger.Category.SI);
                }
                else
                {
                    {
                        _oLog.writeAudit(_sysid, "SIcredit.getUpdate_Txn", _mdesc, "trxn updated :logID=" + txnid.ToString() + s,
                                        "", WinAppLogger.Category.SI);
                    }
                }
            }
            catch (Exception er)
            {
                _oLog.writeAudit(_sysid, "SIcredit.getUpdate_Txn", _mdesc, er.Message.ToString() + s,
                                  "", WinAppLogger.Category.SI);
                throw er;
            }
            /* E2EAPICHANGE 20220209 Removed finally */
        }

        private void get_Db_Error()
        {
            this.messages = String.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.messages = base.Message;
            }
        }

        private void logMe(String msg)
        {
            lock (_lockme)
            {
                try
                {
                    StreamWriter sw = null;
                    String path = "C:\\Temp";
                    String str = DateTime.Now.ToString() + ": " + msg;
                    String dt = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');
                    try
                    {
                        sw = File.AppendText(path + "\\" + "IRSWAgent" + dt + ".log");
                        sw.WriteLine(str);
                        sw.Close();
                    }
                    catch
                    {
                        return;
                    }
                    finally
                    {
                        str = null;
                        if (sw != null)
                        {
                            sw.Dispose();
                        }
                        sw = null;
                    }
                }
                catch
                {
                    return;
                }
                finally { }
            }
        }
        #endregion common methods

        #region Get/Set Property
        public String messages
        {
            get { return this._error_message; }
            set { this._error_message = value; }
        }
        public String api_message
        {
            get { return this._api_message; }
            set { this._api_message = value; }
        }
        private String ok
        {
            get { return _ok; }
        }

        private String failed
        {
            get { return _failed; }
        }
        #endregion Get/Set Property

        /* E2EAPICHANGE 20220209 Change Name */
        ~SIcredit()
        {

        }
    }

    /*
     * (c) PNB 2010 /  Neil Mark M. Panlilio
     *  SMS
    */

    public class csSMS : irsSqlCommons
    {
        public enum smsStatusCode : int
        {
            SendingUnsuccessful = 10,
            NewSMS = 9,
            MessageSent = 0,
            InvalidMessage = 1,
            InvalidAppName = 2,
            InvalidGatewayID = 3,
            InvalidMobileNumber = 4
        }

        #region properties & variables
        String _sysid = "IRSWorldAgent";
        String _mdesc = "Remittance sending SMS to SMS System.";
        private String _error;
        private String _connSMS = null;
        private int _smsReturn = 0;
        private String _smsSummary = null;
        private int _smsCount = 0;
        WinAppLogger _oLog = new WinAppLogger();

        public String Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public String connstringSMS
        {
            get { return _connSMS; }
            set { _connSMS = value; }
        }

        public int smsReturn
        {
            get { return this._smsReturn; }
            set { this._smsReturn = value; }
        }

        public int smsCount
        {
            get { return this._smsCount; }
            set { this._smsCount = value; }
        }

        public String smsSummary
        {
            get { return this._smsSummary; }
            set { this._smsSummary = value; }
        }
        #endregion properties

        #region public methods SMS
        public bool startRemitSMS() /* 1 */
        {
            int rem_seq_id = 0;
            String RecipientNumber; // @recipientNumber
            String Message;         // @message
            DataTable dt = null;
            _smsCount = 0;          // reset value
            this.smsSummary = String.Empty;

            try
            {
                dt = this.get_DataForSMS();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            rem_seq_id = Convert.ToInt32(row["TXN_SEQ_ID"]);
                            RecipientNumber = (String)row["MOBILENO"].ToString();
                            Message = (String)row["MESSAGE"].ToString();
                            bool _bool = this.sendSMS("", "", RecipientNumber, Message, rem_seq_id);

                            try
                            {
                                /* RFS 18317-004 */
                                if (row["SMSPromo"] != DBNull.Value && row["SMSPromo"].ToString() != string.Empty)
                                {
                                    this.sendToSMSDB("", "", RecipientNumber, row["SMSPROMO"].ToString());
                                }
                            }
                            catch { }
                        }
                        this.smsSummary = _smsCount.ToString() + " SMS sent to SMS system.";
                    }
                }
            }
            catch (Exception ex) { this.Error = ex.Message.ToString(); return false; }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            return true;
        }

        private DataTable get_DataForSMS() /* 2 */
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;
            try
            {
                base.EnforceConstraint = false;
                sqcommands = base.getSqlCommand("csp_getRemittanceForSMS");
                ds = base.getDataSet(sqcommands, "SMSdata");
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                _oLog.writeAudit(_sysid, "csSMS.get_DataForSMS", _mdesc, ex.Message.ToString(),
                                  "", WinAppLogger.Category.Insert);
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
            return null;
        }

        public bool sendSMS(String SenderName, String GatewayID, String RecipientNumber, String Message, int rem_seq_id) /* 3 */
        {
            object[] o = new object[5];
            o[0] = rem_seq_id;      // @rem_seq_id
            o[1] = SenderName;      // @senderName
            o[2] = GatewayID;       // @gatewayID
            o[3] = RecipientNumber; // @recipientNumber 
            o[4] = Message;         // @message  
            try
            {
                if (this.addRecord(o))
                {
                    if (this.sendToSMSDB(SenderName, GatewayID, RecipientNumber, Message))
                    {
                        if (this.updateSMStag(rem_seq_id, smsReturn)) { return true; }
                    }
                    else { this.updateSMStag(rem_seq_id, (int)smsStatusCode.SendingUnsuccessful); }
                }
            }
            catch (Exception ex)
            {
                this.Error += ex.Message;
                _oLog.writeAudit(_sysid, "csSMS.sendSMS", _mdesc, ex.Message.ToString(),
                                 "", WinAppLogger.Category.Insert);
            }
            return false;
        }

        private bool addRecord(object[] ob) /* 4 */
        {
            object o;
            try
            {
                o = base.execScalar("sp_insertNewSMS", ob);
                if (o != null)
                {
                    this.Error = base.Message;
                    return false;
                }
            }
            catch { }
            finally
            {
                o = null;
            }
            return true;
        }

        private bool sendToSMSDB(String senderName, String gatewayID, String recipientNumber, String message) /* 5 */
        {
            String s = "recipient:[" + recipientNumber + "] message:[" + message + "]";
            wsSMS.wsSMS sms = new wsSMS.wsSMS();
            wsSMS.csSoapHeader header = new wsSMS.csSoapHeader();
            string ret = string.Empty;

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                sms.Url = System.Configuration.ConfigurationManager.AppSettings["SMSURL"];
                header.AccessID = System.Configuration.ConfigurationManager.AppSettings["SMSAccessID"];
                header.Password = System.Configuration.ConfigurationManager.AppSettings["SMSPassword"];
                sms.csSoapHeaderValue = header;

                ret = sms.connectToSMS(wsSMS.system.IRSW, wsSMS.infobip.sms, "", recipientNumber, message);
                _oLog.writeAudit(_sysid, "csSMS.sendToSMSDB", _mdesc, "SMS successfully sent to " + s, ret, WinAppLogger.Category.Insert);
                _smsCount++;
            }
            catch (Exception e)
            {
                _oLog.writeAudit(_sysid, "csSMS.sendToSMSDB", _mdesc, "Error:" + e.StackTrace.ToString(),
                                  ret, WinAppLogger.Category.Insert);
                this.Error += "Error:" + e.StackTrace.ToString();
                return false;
            }
            finally
            {
                sms.Dispose();
                sms = null;
            }
            if (ret != string.Empty && ret.Substring(0, 1) == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool updateSMStag(int rem_seq_id, int tag) /* 6 */
        {
            object[] o = new object[3];
            o[0] = rem_seq_id;      // @rem_seq_id
            o[1] = tag;             // @tag
            o[2] = 0;               // Force to 0 (zero) - Bene

            return this.updateTag(o);
        }

        private bool updateTag(object[] ob) /* 7 */
        {
            object o;
            try
            {
                o = base.execScalar("sp_updateSMSTag", ob);
                if (o != null)
                {
                    this.Error = base.Message;
                    return false;
                }
            }
            catch { }
            finally
            {
                o = null;
            }
            return true;
        }

        public csSMS()
        {
        }

        ~csSMS() { }
        #endregion public methods
    }

    public class irsMailer : irsSqlCommons
    {
        #region variables & properties
        String sysid = "IRSWorldAgent";
        String mdesc = "Remittance sending Email to Email Server.";
        WinAppLogger oLog = new WinAppLogger();
        csMailer mail = new csMailer();
        String error = String.Empty;

        public String Error
        {
            get { return error; }
            set { error = value; }
        }
        #endregion variables & properties

        #region private methods
        private DataTable getData()
        {
            try
            {
                SqlCommand sqcommands = base.getSqlCommand("sp_getRemittanceForEmail");
                DataSet ds = base.getDataSet(sqcommands, "RemittanceEmail");
                if (ds != null) { return ds.Tables[0]; }
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                oLog.writeAudit(sysid, "irsMailer.getData", mdesc, ex.Message.ToString(),
                                  "", WinAppLogger.Category.Insert);
            }
            return null;
        }

        private bool sendEmail(String SenderName, String[] Recipient, String Subject, String Message, int rem_seq_id)
        {
            String s = Subject + Recipient;
            object[] o = new object[2];
            o[0] = rem_seq_id;
            try
            {
                bool ret = mail.sendMail(Subject, Recipient, Message);
                if (ret)
                {
                    o[1] = 2;   // Success
                    oLog.writeAudit(sysid, "irsMailer.sendEmail", mdesc, "Email sent successfully. " + s + mail.Message, "", WinAppLogger.Category.Insert);
                    updateTag(o);
                }
            }
            catch (Exception e)
            {
                o[1] = 3;   // Error
                oLog.writeAudit(sysid, "irsMailer.sendEmail", mdesc, "Exception: " +
                                e.Message.ToString() + s + mail.Message, "", WinAppLogger.Category.Insert);
                updateTag(o);
                return false;
            }
            return true;
        }

        private bool updateTag(object[] ob)
        {
            object o;
            o = base.execScalar("sp_updateEmailTag", ob);
            if (o != null)
            {
                this.Error = base.Message;
                return false;
            }
            return true;
        }
        #endregion private methods

        #region public methods
        public bool startMail()
        {
            int rem_seq_id = 0;
            String SenderName;      // @senderName            
            String[] Recipient;     // @recipientNumber 
            String Subject;         // @subject
            String Message;         // @message  
            DataTable dt = null;
            try
            {
                dt = getData();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            rem_seq_id = Convert.ToInt32(row["rem_seq_id"]);
                            SenderName = (String)row["SenderName"].ToString();
                            Recipient = new String[] { (String)row["Recipient"] };
                            Subject = (String)row["Subject"].ToString();
                            Message = (String)row["Message"].ToString();
                            bool _bool = this.sendEmail(SenderName, Recipient, Subject, Message, rem_seq_id);
                        }
                    }
                }
            }
            catch (Exception ex) { this.Error = ex.Message.ToString(); return false; }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            return true;
        }
        public irsMailer() { }

        ~irsMailer()
        { }
        #endregion public methods
    }

    public class WinAppLogger : sqlHelper
    {
        public WinAppLogger()
        {
            base.IsWinApp = true;
        }

        public enum Category
        {
            Logon,
            Logoff,
            Select,
            Insert,
            Update,
            Delete,
            PaymentGateway,
            /* E2EAPICHANGE 20220209 Updated Flexcube to SI */
            SI,
            FTP, //added by mak
            Email,
            File, //added by mak
            OAAM,
            IBFT,
            GFC
        }

        public bool checkLog(String SessionID, String Description)
        {
            base.EnforceConstraint = false;
            if (SessionID == "0")
            {
                return false;
            }
            getUpdateDS("sp_Check_Log", new Object[] { SessionID, Description });
            if (hasRows)
            {
                selectedRow = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void writeLogger(String Log, String TableName, String Key)
        {
            int i = 0;
            String[] arrSplit = Log.Split('|');
            String[] arrValue;
            irsAuditLogger logger = new irsAuditLogger();

            for (i = 0; i <= (arrSplit.Length - 1); i++)
            {
                arrValue = arrSplit[i].ToString().Split('~');
                logger.writeLog(TableName, arrValue[0].ToString(), 1, "", arrValue[1].ToString(), Key);
            }
        }

        public bool writeAudit(String UserID, String Module, String Description, String NewValue, String OldValue, Category Category)
        {
            String[] arrAudit = new String[7];
            String sUserID = String.Empty;
            int iReturn = 0;

            if (UserID == String.Empty)
            {
                if (base.IsWinApp)
                {
                    sUserID = "IRSWorldAgent";
                }
                else
                {
                    sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                }
            }
            else
            {
                sUserID = UserID;
            }
            sUserID = (sUserID == null) ? String.Empty : sUserID;

            try
            {
                arrAudit[0] = Convert.ToString(this.getSessionID());
                arrAudit[1] = sUserID;
                arrAudit[2] = Module;
                arrAudit[3] = Description;
                arrAudit[4] = NewValue;
                arrAudit[5] = OldValue;
                arrAudit[6] = Category.ToString();

                iReturn = execStoredProcedureUpdate("sp_Insert_Log", arrAudit);
                if (iReturn == -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (Category.ToString() == "Logoff")
                {
                    System.Web.HttpContext.Current.Session["SessionID"] = null;
                }
            }
        }

        private string generateYYMMDD()
        {
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sMonth = (sMonth.Length == 1) ? "0" + sMonth : sMonth;
            sDay = (sDay.Length == 1) ? "0" + sDay : sDay;
            return sYear + sMonth + sDay;
        }

        private void createLogs(String Message)
        {
            //StreamWriter swFile = new StreamWriter(Environment.GetEnvironmentVariable("TEMP") + @"\IRSWorldLog.nfo", true, Encoding.Default);
            StreamWriter swFile = new StreamWriter(@"C:\Temp\IRSWorldLog_" + generateYYMMDD() + ".txt", true, Encoding.Default);
            String dtLog = DateTime.Now.Date.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();

            swFile.Write(dtLog +
                " [User ID: " + ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId + "] " +
                " [Location: " + ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location + "]" +
                " [Message: " + Message + "]" + "\r\n");
            swFile.Flush();
            swFile.Dispose();
            swFile = null;
        }

        private long getSessionID()
        {
            try
            {
                if (System.Web.HttpContext.Current.Session["SessionID"] == null)
                {
                    return Math.Abs(System.Web.HttpContext.Current.Session.SessionID.GetHashCode());
                }
                else
                {
                    return Convert.ToInt64(System.Web.HttpContext.Current.Session["SessionID"]);
                }
            }
            catch
            {
                return 0;
            }
        }
    }

    public class irsAgentOTC : irsSqlCommons
    {
        /* E2EAPICHANGE 20220209 Added _e2e */
        E2EAPIConnector.E2EAPIConnector _e2e = new E2EAPIConnector.E2EAPIConnector();

        public enum Trans_Status : int
        {
            Credited_Online = 2,
            Posted_Central = 5,
            On_Hold = 14,
            For_Approval = 15,
            For_Reprocess = 9,
            Cancel_Not_Implemented = 11,
            Cancel_Implemented = 12,
            For_IBATCH_Processing = 16,
            IBFTTimeout = 26
        }

        #region PRIVATE VARIABLES & METHODS
        private String _sysid = "IRSWorldAgent";
        private String _mdesc = "Test Connection to SI";
        private String _failedmsg = "FAILED TO CREDIT **";
        private String _error_message = null;
        private String _api_message = null;
        private String _ok = String.Empty;
        private String _failed = String.Empty;
        private String _maxthread;
        private String _logid = "logID=";
        WinAppLogger oLog = new WinAppLogger();
        irsThreadManager tManager;
        /* E2EAPICHANGE 20220209 Removed MQ */

        /* E2EAPICHANGE 20220209 Removed get_MQconn */

        private DataTable check_DataForCrediting()
        {
            DataSet ds = null;
            try
            {
                base.EnforceConstraint = false;
                SqlCommand sqcommands = base.getSqlCommand("sp_irsAgent_checkdata_for_crediting_OTC");
                ds = base.getDataSet(sqcommands, "FOR_CREDITTING");
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception er)
            {
                oLog.writeAudit(_sysid, "irsAgentOTC.get_DataForCrediting", _mdesc, er.Message.ToString(),
                                 "", WinAppLogger.Category.SI);
                this.get_Db_Error();
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

        private DataTable get_DataForCrediting()
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
            catch (Exception er)
            {
                oLog.writeAudit(_sysid, "irsAgentOTC.get_DataForCrediting", _mdesc, er.Message.ToString(),
                                 "", WinAppLogger.Category.SI);
                this.get_Db_Error();
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

        /* E2EAPICHANGE 20220209 Updated Name */
        private DataTable getSIAccountTest()
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
            /* E2EAPICHANGE 20220209 Updated Catch */
            catch (Exception er)
            {
                oLog.writeAudit(_sysid, "irsAgentOTC.getSIAccountTest", _mdesc, er.Message.ToString(), "", WinAppLogger.Category.SI);
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

        private void getUpdate_Txn(int txnid, int txnstatus, String fc_message, DateTime dtImplemOverseas, DateTime dtImplemLocal)
        {
            object o;

            try
            {
                o = base.execScalar("sp_irsAgent_update_for_credit", new Object[] { txnid, txnstatus, fc_message, dtImplemOverseas, dtImplemLocal });
                if (o == null)
                { this.messages = base.Message; }
            }
            catch (Exception er)
            {
                /* E2EAPICHANGE 20220209 Updated Category */
                oLog.writeAudit(_sysid, "SIcredit.getUpdate_Txn", _mdesc, er.Message.ToString(), "", WinAppLogger.Category.SI);
                throw er;
            }
        }

        private String get_PAWAccount(int currency)
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

        private String get_ApasAccount(int currency)
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

        /* E2EAPICHANGE 20220209 Removed getCheckFCError */

        /* E2EAPICHANGE 20220209 Updated Name and Code */
        private bool getTestSIConnection()
        {
            string err = string.Empty;
            string e2eres = string.Empty;
            string accountNo = string.Empty;
            DataTable dtTest = null;
            bool result = false;

            dtTest = this.getSIAccountTest();
            try
            {
                if (dtTest != null)
                {
                    if (dtTest.Rows.Count > 0)
                    {
                        accountNo = dtTest.Rows[0][0].ToString();
                    }

                    E2EWS e2e = new E2EWS();
                    string[] param = new string[]
                    {
                        accountNo,
                        e2e.GenerateUserRef(accountNo),
                        e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.TransactionBranch),
                        e2e.GetE2EConfiguration(E2EWS.E2EConfiguration.Channel)
                    };
                    DataSet ds = _e2e.connect2E2E(Guid.NewGuid().ToString(), E2EAPIConnector.E2EAPIConnector.APIMethod.AccountInquiry, E2EAPIConnector.E2EAPIConnector.MediaType.json, param, out result, out err);
                    result = (e2e.E2ESuccess(ds, out e2eres));

                    if (!result)
                    {
                        this.messages = e2eres;
                        oLog.writeAudit(_sysid, "irsAgent.getTestSIConnection", _mdesc, "Error test connection: " + e2eres, "", WinAppLogger.Category.SI);
                        Thread.Sleep(5000);
                        return false;
                    }
                }
            }
            catch (Exception er)
            {
                this.messages = er.StackTrace.ToString();
                oLog.writeAudit(_sysid, "irsAgent.start_credit", _mdesc, "Error:" + er.Message.ToString(), "", WinAppLogger.Category.SI);
                return false;
            }
            finally
            {
                if (dtTest != null)
                {
                    dtTest.Dispose();
                    dtTest = null;
                }
            }
            return true;
        }

        private DataTable getAccountInfo(string referenceNo)
        {
            DataTable dt = execStoredProcedureTable("ibftsp_getAccountInfo", new object[] { referenceNo });
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt;
            }
            return null;
        }
        #endregion

        #region PROPERTY GET & SET
        public String messages
        {
            get { return this._error_message; }
            set { this._error_message = value; }
        }

        public String api_message
        {
            get { return this._api_message; }
            set { this._api_message = value; }
        }

        public String max_thread
        {
            set { _maxthread = value; }
        }

        private String ok
        {
            get { return _ok; }
        }

        private String failed
        {
            get { return _failed; }
        }
        #endregion PROPERTY GET & SET

        #region PUBLIC METHODS
        public irsAgentOTC()
        {
            this._error_message = null;
            base.IsWinApp = true;
            /* E2EAPICHANGE 20220209 Removed csFlexcubeTeller */
        }

        public void get_Db_Error()
        {
            this.messages = String.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.messages = base.Message;
            }
        }

        public bool startOTCcredit()
        {
            tManager = new irsThreadManager(int.Parse(_maxthread));
            tManager.MaxThread = int.Parse(_maxthread);
            String refnum = String.Empty;
            String sourceAcct = String.Empty;
            String targetAcct = String.Empty;
            String amt = String.Empty;
            String narrative = String.Empty;
            DateTime txn_date = DateTime.Now;
            String curr = String.Empty;
            String FileFormat = String.Empty;
            String actCur = String.Empty;
            String rembrcode = String.Empty;
            String mobileNo = String.Empty;
            String remmethod = String.Empty;
            /* E2EAPICHANGE 20220209 Removed isAllied */
            String originalAccount = String.Empty;
            DataTable dt = null;
            DataTable dtcheck = null;
            this.messages = null;
            int trxnid = 0;
            int prodcode = 0;
            /* E2EAPICHANGE 20220209 Change Name */
            SIcredit SI = null;
            Thread t = null;
            try
            {
                dtcheck = this.check_DataForCrediting();
                if (dtcheck != null)
                {
                    if (dtcheck.Rows.Count > 0)
                    {
                        //  start    : Test module
                        /* E2EAPICHANGE 20220209 Updated Name */
                        if (!this.getTestSIConnection()) { return false; }
                        //  end      : Test module
                        dt = this.get_DataForCrediting();

                        this.messages = null;
                        int seqid = 0;
                        string targetacct2 = String.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            try
                            {
                                if ((bool)ThreadKill.Thread_Kill) { break; }
                                if ((int)seqid == Convert.ToInt32(row["trxnid"])) { continue; }
                                if ((String)targetacct2 == (String)row["TargetAccount"].ToString()) { Thread.Sleep(50); }

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
                                remmethod = (String)row["RemitMethod"].ToString();
                                originalAccount = (String)row["OriginalAccount"].ToString().Trim();
                                if (prodcode == (int)ProductCode.PAW)
                                { targetAcct = this.get_PAWAccount(Convert.ToInt32(curr)); }
                                if (prodcode == (int)ProductCode.APASOnline)
                                { targetAcct = this.get_ApasAccount(Convert.ToInt32(curr)); }
                                api_message = String.Empty;

                                // IBFT 20140321
                                DataTable dtIBFT = getAccountInfo(refnum);
                                string sLog = string.Empty;
                                if (prodcode == (int)ProductCode.IBFT)
                                {
                                    oLog.writeAudit(_sysid, "irsAgent.IBFT", "IBFT For Credit: " + refnum, "startOTCcredit", "", WinAppLogger.Category.IBFT);
                                    try
                                    {
                                        if (dtIBFT != null && dtIBFT.Rows.Count > 0)
                                        {
                                            string[] arrResponse;
                                            int iTimeout = 0;
                                            string sTargetAccount = dtIBFT.Rows[0]["AccountNo"].ToString().Trim();
                                            string sDestinationBank = dtIBFT.Rows[0]["DomBankCode"].ToString().Trim();
                                            oLog.writeAudit(_sysid, "irsAgent.IBFT", "IBFT Count: " + dtIBFT.Rows.Count.ToString() + " TargetAccount: " + sTargetAccount + " DestinationBank: " + sDestinationBank, "startOTCcredit", "", WinAppLogger.Category.IBFT);
                                            irsConfiguration conappsetting = new irsConfiguration();
                                            string[] arrTimeoutCode = conappsetting.appsetting("IBFTTimeoutCode").ToString().Split('|');

                                            oLog.writeAudit(_sysid, "irsAgent.IBFT",
                                                "BranchCode=[" + rembrcode + "]" +
                                                "ReferenceNumber=[" + refnum + "]" +
                                                "Amount=[" + amt + "]" +
                                                "DebitAccount=[" + sourceAcct + "]" +
                                                "TargetAccount=[" + sTargetAccount + "]" +
                                                "DestinationBank=[" + sDestinationBank + "]" +
                                                "IBFTURL=[" + new irsConfiguration().appsetting("IBFTURL").ToString() + "]",
                                                "startOTCcredit", refnum, WinAppLogger.Category.IBFT);

                                            arrResponse = new irsIBFTKey().submitIBFT(
                                                conappsetting.appsetting("IBFTURL"),
                                                conappsetting.appsetting("IBFTRemoteHost"),
                                                Convert.ToInt32(conappsetting.appsetting("IBFTPort")),
                                                Convert.ToInt32(conappsetting.appsetting("IBFTTimeout")),
                                                rembrcode,
                                                conappsetting.appsetting("IBFTUSConn"),
                                                conappsetting.appsetting("IBFTEAConn"),
                                                conappsetting.appsetting("IBFTTerminalID"),
                                                amt, sDestinationBank, sourceAcct, sTargetAccount, refnum);

                                            if (arrResponse.GetValue(0).ToString().Trim() == conappsetting.appsetting("IBFTResponseCode").Trim())
                                            {
                                                sLog = "IBFT ACCOUNT PROCESSED ***DR" + sourceAcct + "CR" + sTargetAccount;
                                                this.getUpdate_Txn(trxnid, (int)Trans_Status.Credited_Online, sLog, txn_date, txn_date);
                                            }
                                            else
                                            {
                                                sLog = "IBFT TRANSFER ERROR: ReferenceNo=[" + refnum + "]" +
                                                        "ResponseCode=[" + arrResponse.GetValue(0).ToString().Trim() + " - " + arrResponse.GetValue(3).ToString().Trim() + "]" +
                                                        "RetrievalReferenceNumber=[" + arrResponse.GetValue(1).ToString().Trim() + "]";
                                                this.getUpdate_Txn(trxnid, (int)Trans_Status.For_Reprocess, sLog, txn_date, txn_date);
                                            }

                                            for (iTimeout = 0; iTimeout <= (arrTimeoutCode.Length - 1); iTimeout++)
                                            {
                                                if (arrResponse.GetValue(0).ToString().Trim() == arrTimeoutCode.GetValue(iTimeout).ToString().Trim())
                                                {
                                                    sLog = "IBFT TRANSFER TIMEOUT: ReferenceNo=[" + refnum + "]" +
                                                        "ResponseCode=[" + arrResponse.GetValue(0).ToString().Trim() + " - " + arrResponse.GetValue(3).ToString().Trim() + "]" +
                                                        "RetrievalReferenceNumber=[" + arrResponse.GetValue(1).ToString().Trim() + "]";
                                                    this.getUpdate_Txn(trxnid, (int)Trans_Status.IBFTTimeout, sLog, txn_date, txn_date);
                                                    break;
                                                }
                                            }
                                            oLog.writeAudit(_sysid, "irsAgent.IBFT", sLog, "startOTCcredit", "ResponseCode=[" + arrResponse.GetValue(0).ToString().Trim() + "]", WinAppLogger.Category.IBFT);
                                        }
                                        else
                                        {
                                            oLog.writeAudit(_sysid, "irsAgent.IBFT Error", "Failed to get account info: " + refnum, "startOTCcredit", "", WinAppLogger.Category.IBFT);
                                            this.getUpdate_Txn(trxnid, (int)Trans_Status.For_Reprocess, sLog, txn_date, txn_date);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        sLog += " Exception: " + ex.Message;
                                        this.getUpdate_Txn(trxnid, (int)Trans_Status.For_Reprocess, sLog, txn_date, txn_date);
                                        oLog.writeAudit(_sysid, "irsAgent.IBFT Error", sLog, "startOTCcredit", ex.StackTrace, WinAppLogger.Category.IBFT);
                                    }
                                }
                                else
                                {
                                    if (sourceAcct != null && sourceAcct != "" && targetAcct != "")
                                    {
                                        while (tManager.isThreadExists(sourceAcct))
                                        {
                                            Thread.Sleep(500);
                                        }

                                        /* E2EAPICHANGE 20220209 Updated Code */
                                        SI = new SIcredit(refnum, targetAcct, sourceAcct, amt, curr, trxnid, txn_date, refnum, prodcode, rembrcode, remmethod, originalAccount);

                                        /* E2EAPICHANGE 20220209 Updated Code */
                                        t = new Thread(SI.do_FundTransfer);
                                        t.Name = sourceAcct;
                                        tManager.addThread(t);
                                        t.Start();
                                        oLog.writeAudit(_sysid, "irsAgent.start_credit", "Thread Start", "Thread name " + sourceAcct.ToString() + " started." + _logid + seqid.ToString(), "", WinAppLogger.Category.SI);
                                    }
                                    else
                                    {
                                        String s = _failedmsg + "Target Account or Nostro/Vostro Account is missing. Please verify.";
                                        this.getUpdate_Txn(trxnid, (int)Trans_Status.For_Reprocess, s, txn_date, txn_date);
                                        oLog.writeAudit(_sysid, "irsAgent.start_credit", "No NSVS account", sourceAcct.ToString() + ". " + _logid + seqid.ToString(), "", WinAppLogger.Category.SI);
                                    }
                                }
                            }
                            catch (Exception _err)
                            {
                                this.getUpdate_Txn(trxnid, (int)Trans_Status.For_Reprocess, _err.Message, txn_date, txn_date);
                                continue;
                            }
                        }
                    }
                }
                while (tManager.checkRunningThread()) { Thread.Sleep(50); }
            }
            catch (Exception er)
            {
                this.messages = er.StackTrace.ToString();
                oLog.writeAudit(_sysid, "irsAgent.start_credit", "Thread Start", "Error:" + er.Message, "", WinAppLogger.Category.SI);
                return false;
            }
            finally
            {
                if (dtcheck != null)
                {
                    dtcheck.Dispose();
                    dtcheck = null;
                }
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if (tManager != null)
                {
                    tManager = null;
                }
                if (t != null)
                {
                    t = null;
                }
                /* E2EAPICHANGE 20220209 Updated Code */
                if (SI != null)
                {
                    SI = null;
                }
            }
            return true;
        }
        #endregion
    }

    /* E2EAPICHANGE 20220209 Removed irsAgent */
}