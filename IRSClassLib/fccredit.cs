using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GRSMQ;
using System.Data;

namespace IRSClassLib 
{
    public class fccredit : irsSqlCommons
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
        public enum ProductCode : int
        {
            RapidRemit = 10,
            GFC = 22,
            WealthMultiplier = 14,
            PAW = 24,
            APASOnline = 27
        }
        #region variables        
        String sysid = "IRSWorldAgent";
        String mdesc = "Crediting to FC";

        String error_message = null;
        String _api_message = null;
        String _ok = "ACCOUNT PROCESSED **";
        String _failed = "Error (Flexcube):";
        String _refnum = String.Empty;
        String _targetAcct = String.Empty;
        String _amt = String.Empty;
        String _sourceAcct = String.Empty;
        String _actCur = String.Empty;
        String _rembrcode = String.Empty;
        int _trxnid = 0;
        int _productcode = 0;
        DateTime _txn_date = DateTime.Now;
        String _narrative = String.Empty;
        csFlexcubeTeller o = new csFlexcubeTeller();
        WinAppLogger oLog = new WinAppLogger();
        irsTimeZone tZone = new irsTimeZone();
        static Object lockme = new object();        
        
        #endregion variables

        #region common public method
        public fccredit() 
        { }
        public fccredit(String refnum, String targetAcct, String sourceAcct,
                        String amt, String actCur, int trxnid, DateTime txn_date,
                        String narrative, int productcode, String rembrcode)
        {
            _refnum = refnum;
            _targetAcct = targetAcct;
            _sourceAcct = sourceAcct;
            _amt = amt;
            _actCur = actCur;
            _refnum = refnum;
            _trxnid = trxnid;
            _txn_date = txn_date;
            _narrative = "IRS (" + narrative + ")";
            _productcode = productcode;
            _rembrcode = rembrcode;
            this.error_message = null;
            base.IsWinApp = true;
        }
        
        #endregion common public method

        #region IBatch Region
        public void do_iBatch_credit()
        {            
            int i = 0;
            String s = String.Empty;
            String apmsg = String.Empty;
            DateTime dtImplemOverseas = tZone.branchLocalTime(_rembrcode);
            DateTime dtImplemLocal = DateTime.Now;

            if (this.iBatch_credit(_refnum, _targetAcct, _sourceAcct, _amt, _actCur, _narrative))
            { 
                i = (int)Trans_Status.Credited_Online; 
                s = this.ok.ToString(); 
            }
            else 
            {
                i = (int)Trans_Status.For_Reprocess;
                s = this.failed.ToString(); 
            }
            apmsg =  s + api_message + "GL" + _sourceAcct + "CK" + _targetAcct + this.messages;
            String ss = s + "api_msg=" + s + api_message +
                                    this.messages + "datetime:" + DateTime.Now.ToString() + " refnum=" + _refnum + "targetAcct=" + _targetAcct +
                                    "sourceAcct=" + _sourceAcct + "amt=" + _amt + "actCur=" + _actCur + "narrative=" + _narrative;

            oLog.writeAudit(sysid, "fccredit.do_iBatch_credit", mdesc, ss + "", "", WinAppLogger.Category.FlexCube);
            this.update_txn(_trxnid, i, apmsg, dtImplemOverseas, dtImplemLocal);
            this.logme(ss);

            if (oLog != null)
            {
                oLog = null;
            }
            if (tZone != null)
            {
                tZone = null;
            }
            return;
        }

        private bool iBatch_credit(String kbiRef, String act, String kbiGL,
                                    String amtBene, String actCur, String refnum)
        {
            String fc_msg = String.Empty;
            o = new csFlexcubeTeller();
            bool result = false;
            try
            {
                result = o.debitGlCreditCasa(kbiRef, act, kbiGL, amtBene, actCur, refnum, out fc_msg);
            }
            catch (Exception e)
            {
                this.messages = e.Message + ' ' + e.StackTrace;
                return result;
            }
            finally 
            { 
                if (o != null) 
                { 
                    o = null; 
                } 
            }
            api_message = fc_msg;
            return result;
        }

        #endregion  IBatch Region


        #region for OTC transactions **************************************
        
        public bool fund_transfer(String refnum, String sourceAcct, String targetAcct, String amt, String narative)
        {
            String fc_msg = String.Empty;
            bool result = false;
            

            try
            {                
                result = o.fundTransfer(refnum, sourceAcct, targetAcct, amt, narative, out fc_msg);             
            }
            catch (Exception e)
            {
                this.messages = e.Message + ' ' + e.StackTrace;
            }
            finally 
            {
                if (o != null)
                {
                    o.close();
                    o = null;
                }
            }
            api_message = fc_msg.Replace("Exception:", "").ToString();
            return result;        
        }               

        public void do_fundtransfer()
        {            
            int i = 0;            
            String s = String.Empty;
            DateTime dtImplemOverseas = Convert.ToDateTime(tZone.branchLocalTime_nWs(_rembrcode));
            DateTime dtImplemLocal = DateTime.Now;
            irsLinkToGFC gfc = new irsLinkToGFC(true);
            irsOldAPASWinApp apas = new irsOldAPASWinApp();

            try
            {
                if (this.fund_transfer(_refnum, _sourceAcct, _targetAcct, _amt, _narrative))
                {
                    i = (int)Trans_Status.Credited_Online;
                    s = this.ok.ToString();
                }
                else
                {
                    i = (int)Trans_Status.For_Reprocess;
                    s = this.failed.ToString();
                }
                String details = "refnum=" + _refnum + "targetAcct=" + _targetAcct + "sourceAcct=" + _sourceAcct + "amt=" + _amt + "narrative=" + _narrative + " seq_id=" + _trxnid.ToString();
                String apmsg = s + api_message + "CKSOURCE" + _sourceAcct + "CKTARGET" + _targetAcct + this.messages + " seq_id=" + _trxnid.ToString();
                String ss = "api_msg=" + s + api_message + this.messages + "datetime:" + DateTime.Now.ToString() + details;

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
                            String apmsg2 = apmsg + " |OBP EXCESS PAY: " + s + api_message + "OBPNSVS" + OBPNSVS + "OBPAccount" + OBPAccount + this.messages + " seq_id=" + _trxnid.ToString();
                            String ss2 = apmsg + " |OBP EXCESS PAY: api_msg=" + s + api_message + this.messages + "datetime:" + DateTime.Now.ToString() + details;

                            oLog.writeAudit(sysid, "fccredit.do_fundtransfer.ifOBPExcess", mdesc, ss2 + "", "", WinAppLogger.Category.FlexCube);
                            this.update_txn(_trxnid, i, apmsg2, dtImplemOverseas, dtImplemLocal);
                            this.logme(ss2);
                            apmsg2 = null;
                            ss2 = null;

                        }
                        else
                        {
                            oLog.writeAudit(sysid, "fccredit.ifOBPExcess", mdesc, ss + "", "", WinAppLogger.Category.FlexCube);
                            this.update_txn(_trxnid, i, apmsg, dtImplemOverseas, dtImplemLocal);
                            this.logme(ss);
                        }
                    }
                    else
                    {
                        oLog.writeAudit(sysid, "fccredit.ifOBPExcess", mdesc, ss + "", "", WinAppLogger.Category.FlexCube);
                        this.update_txn(_trxnid, i, apmsg, dtImplemOverseas, dtImplemLocal);
                        this.logme(ss);
                    }
                }
                else
                {
                    oLog.writeAudit(sysid, "fccredit.do_fundtransfer", mdesc, ss + "", "", WinAppLogger.Category.FlexCube);
                    this.update_txn(_trxnid, i, apmsg, dtImplemOverseas, dtImplemLocal);

                    if (i == (int)Trans_Status.Credited_Online) 
                    {
                        if ((int)_productcode == (int)ProductCode.APASOnline) // check for APAS
                        {  
                            if (!apas.InsertApas(_trxnid)) 
                            {
                                oLog.writeAudit(sysid, "fccredit.InsertApas", mdesc + "Error: " + apas.APASMessage.ToString(), "", "", WinAppLogger.Category.FlexCube);
                            }
                            else
                            {
                                oLog.writeAudit(sysid, "fccredit.InsertApas", mdesc + "Successfully sent to APAS DB." + details, "", "", WinAppLogger.Category.FlexCube);
                            }
                            
                        }
                        else if ((int)_productcode == (int)ProductCode.PAW) // check for PAW
                        {                           
                            if (!apas.InsertAnPAW(_trxnid))
                            {
                                oLog.writeAudit(sysid, "fccredit.InsertAnPAW", mdesc + "Error: " + apas.Message.ToString(), "", "", WinAppLogger.Category.FlexCube);
                            }
                            else
                            {
                                oLog.writeAudit(sysid, "fccredit.InsertAnPAW", mdesc + "Successfully sent to APAS DB." + details, "", "", WinAppLogger.Category.FlexCube);
                            }
                        }
                        // GFC Aggregation
                        gfc.IsWinApp = true;
                        gfc.insert_GFC_Aggregate(_refnum, _targetAcct, Convert.ToInt32(_actCur), Convert.ToDouble(_amt));
                    }
                    this.logme(ss);
                }
                
                apmsg = null;
                ss = null;
                Thread.Sleep(50);
            }
            catch(Exception e)
            { 
                throw e; 
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
                if (oLog != null)
                {
                    oLog = null;
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
                if (dt!= null)
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
                    oLog.writeAudit(this.sysid, "OBPS Transaction", "fOBPExcess", "", TranxID.ToString(), WinAppLogger.Category.Select);
                    return true;
                }
            }
            catch (Exception ex)
            {
                oLog.writeAudit(this.sysid, "OBPS Transaction", "fOBPExcess", ex.Message, TranxID.ToString(), WinAppLogger.Category.Select);
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
        private void update_txn(int txnid, int txnstatus, String fc_message, DateTime dtImplemOverseas, DateTime dtImplemLocal)
        {
            object _o;

            try
            {
                _o = base.execScalar("sp_irsAgent_update_for_credit", new Object[] { txnid, txnstatus, fc_message, dtImplemOverseas, dtImplemLocal });
                if (_o == null)
                { this.messages = base.Message; }
            }
            catch (Exception er)
            {
                oLog.writeAudit(sysid, "fccredit.update_txn", mdesc, er.Message.ToString(),
                                  "", WinAppLogger.Category.FlexCube);
                throw er;
            }
            finally 
            { 
                _o = null; 
            }
        }  
        private void get_db_err()
        {
            this.messages = String.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.messages = base.Message;
            }
        }
        
              

        private void logme(String msg)
        {
            lock (lockme)
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
                finally 
                { 

                }
            }
        }
        #endregion common methods



        #region Get/Set Property
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
        private String ok
        { get { return _ok; } }

        private String failed
        { get { return _failed; } }

        #endregion Get/Set Property

        ~fccredit() 
        { 

        }

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
            FlexCube,
            FTP, //added by mak
            Email,
            File, //added by mak
            OAAM
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
                sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
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

                if (this.checkLog(arrAudit[0].ToString(), arrAudit[3].ToString()) == false)
                {
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
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                createLogs("[UserID=" + UserID + "]" +
                           "[Module=" + Module + "]" +
                           "[Description=" + Description + "]" +
                           "[NewValue=" + NewValue + "]" +
                           "[OldValue=" + OldValue + "]" +
                           "[Category=" + Category + "]" +
                           "[Exception=" + ex.Message + "]");
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

        private void createLogs(String Message)
        {
            StreamWriter swFile = new StreamWriter(Environment.GetEnvironmentVariable("TEMP") + @"\IRSWorldLog.nfo", true, Encoding.Default);
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
   
}
