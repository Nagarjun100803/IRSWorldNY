using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace IRSClassLib
{
    /*
     * (c) PNB 2010 /  Neil Mark M. Panlilio
     * 
     */
    [Serializable]
    public class csSMS : irsSqlCommons
    {
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

        #region private methods
        private bool addRecord(object[] ob)
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
        private bool updateTag(object[] ob)
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

        private bool sendToSMSDB(String senderName, String gatewayID, String recipientNumber, String message)
        {        
            String s = "sender:" + senderName + "recipient:" + recipientNumber + "message:" + message;
            Object ob = null;
            irsSqlCommons sqlcmn = new irsSqlCommons("SMSConnectionString");
            SqlCommand sqlCmd = new SqlCommand("sp_SASsaveSMSRequest");
            sqlcmn.IsWinApp = true;
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandTimeout = 0;
            sqlCmd.Parameters.Add("@senderName", SqlDbType.VarChar, 50).Value = senderName;
            sqlCmd.Parameters.Add("@gatewayID", SqlDbType.VarChar, 50).Value = gatewayID;
            sqlCmd.Parameters.Add("@recipientNumber", SqlDbType.VarChar, 13).Value = recipientNumber;
            sqlCmd.Parameters.Add("@message", SqlDbType.VarChar, 160).Value = message;
            sqlCmd.Parameters.Add("@output", SqlDbType.Int, 1).Direction = ParameterDirection.Output;
            try
            {
                ob = sqlcmn.sqlUpdate(sqlCmd);
                if (ob == null)
                {
                    this.Error = base.Message;
                    return false;
                }
                smsReturn = (int)sqlCmd.Parameters["@output"].Value;
                _oLog.writeAudit(_sysid, "csSMS.sendToSMSDB", _mdesc, "SMS successfully sent." + s,
                                  "", WinAppLogger.Category.Insert);
                _smsCount++;
            }
            catch (Exception e)
            {
                _oLog.writeAudit(_sysid, "csSMS.sendToSMSDB", _mdesc, "Error:" + e.StackTrace.ToString(),
                                  "", WinAppLogger.Category.Insert);
                this.Error += "Error:"+ e.StackTrace.ToString();                
                return false; 
            }
            finally 
            {
                if (sqlcmn != null)
                {
                    sqlcmn = null;
                }
                if (sqlCmd != null)
                {
                    sqlCmd.Dispose();
                    sqlCmd = null;
                }
                if (ob != null)
                {
                    ob = null;
                }                
            }
            return true;
        }
        /*
        private bool sendToSMSDB(String senderName, String gatewayID, String recipientNumber, String message)
        {
            String s = "sender:" + senderName + "recipient:" + recipientNumber + "message:" + message;
            SqlConnection cn = new SqlConnection(connstringSMS);
            SqlCommand sqlCmd = new SqlCommand("sp_SASsaveSMSRequest", cn);            
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandTimeout = 0;
            sqlCmd.Parameters.Add("@senderName", SqlDbType.VarChar, 50).Value = senderName;
            sqlCmd.Parameters.Add("@gatewayID", SqlDbType.VarChar, 50).Value = gatewayID;
            sqlCmd.Parameters.Add("@recipientNumber", SqlDbType.VarChar, 13).Value = recipientNumber;
            sqlCmd.Parameters.Add("@message", SqlDbType.VarChar, 160).Value = message;
            sqlCmd.Parameters.Add("@output", SqlDbType.Int, 1).Direction = ParameterDirection.Output;
            

            try
            {                
                if (cn.State == 0){ cn.Open(); }
                sqlCmd.ExecuteNonQuery();
                smsReturn = (int)sqlCmd.Parameters["@output"].Value;
                _oLog.writeAudit(_sysid, "csSMS.sendToSMSDB", _mdesc, "SMS successfully sent." + s ,
                                  "", csAuditLog.Category.Insert);
                return true;
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                _oLog.writeAudit(_sysid, "csSMS.sendToSMSDB", _mdesc, "Error:SMS~"+ s + ex.Message.ToString(),
                                  "", csAuditLog.Category.Insert);
            }
            finally
            {
                cn.Close();
                cn = null;
            }
            smsReturn = 10;
            return false;
        }
         */


        private bool updateSMStag(int rem_seq_id, int tag)
        {
            object[] o = new object[2];
            o[0] = rem_seq_id;      // @rem_seq_id
            o[1] = tag;             // @tag
            return this.updateTag(o);
        }
        private DataTable get_DataForSMS()
        {
            SqlCommand sqcommands = null;
            DataSet ds = null;
            try
            {
                base.EnforceConstraint = false;
                sqcommands = base.getSqlCommand("sp_getRemittanceForSMS");
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
        #endregion private methods

        #region public methods
        public bool startRemitSMS() 
        {
            int rem_seq_id = 0;
            String SenderName;      // @senderName
            String GatewayID;       // @gatewayID
            String RecipientNumber; // @recipientNumber 
            String Message;         // @message  
            DataTable dt = null;
            _smsCount = 0; // reset value
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
                            rem_seq_id = Convert.ToInt32(row["rem_seq_id"]);
                            SenderName = (String)row["SenderName"].ToString();
                            GatewayID = (String)row["GatewayID"].ToString();
                            RecipientNumber = (String)row["RecipientNumber"].ToString();
                            Message = (String)row["Message"].ToString();
                            bool _bool = this.sendSMS(SenderName, GatewayID, RecipientNumber, Message, rem_seq_id);
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
        public bool sendSMS(String SenderName, String GatewayID, String RecipientNumber, String Message,int rem_seq_id)
        {
            int i = 10; //error
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
                    else { this.updateSMStag(rem_seq_id, i);  }                    
                }
            }
            catch(Exception ex) 
            { 
                this.Error += ex.Message;
                _oLog.writeAudit(_sysid, "csSMS.sendSMS", _mdesc, ex.Message.ToString(),
                                 "", WinAppLogger.Category.Insert);
            }
            
            return false;
            
        }
        public bool sendSMS(String SenderName, String GatewayID, String RecipientNumber, String Message)
        {
            int i = 10; //error
            try
            {
                if (this.sendToSMSDB(SenderName, GatewayID, RecipientNumber, Message)) { return true; }                
            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                _oLog.writeAudit(_sysid, "csSMS.sendSMS", _mdesc, ex.Message.ToString(),
                                 "", WinAppLogger.Category.Insert);
            }
            smsReturn = i;
            return false;
        }        
        public csSMS() 
        {
            

        }
        ~csSMS(){ }
        #endregion public methods
    }
}
