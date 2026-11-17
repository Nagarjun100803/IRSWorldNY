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
    public class irsAgentCreditOtherTrxn : irsSqlCommons
    {
        #region variables
        DataTable dt            = new DataTable();
        String error_message    = String.Empty;
        String api_message      = String.Empty;
        csAuditLog oLog         = new csAuditLog();
        String sysid            = "IRSWorldAgent";
        csFlexcubeTeller o      = new csFlexcubeTeller();
        #endregion variables

        #region private Methods
        private DataTable getdata()
        {
            try
            {
                base.EnforceConstraint = false;
                SqlCommand sqcommands = base.getSqlCommand("sp_irsAgent_getdata_for_crediting_others");                
                DataSet ds = base.getDataSet(sqcommands, "for other credit");
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch
            {
                oLog.writeAudit(sysid, "irsAgentCreditOtherTrxn.getdata()", "Running Agent other credit module.", "Error: " + base.Message, "", csAuditLog.Category.Select);
            }  
            return null;
        }

        private void update_txn(object[] ob)
        {
            object _o;
            _o = base.execScalar("sp_irsAgent_update_for_credit_others", ob);
            if (_o == null)
            { messages = base.Message; }
        }

        private bool credit(string kbiRef, string act, string kbiGL, string amtBene, string actCur, string refnum, int trxnid)
        {
            string fc_msg   = string.Empty;
            bool result     = false;
            String s        = String.Empty;
            refnum          = "IRS (" + refnum + ")";
            string ok       = "ACCOUNT PROCESSED **";
            string failed   = "**FAILED TO CREDIT **";
            int i           = 0;
            Object[] ob     = new Object[4];
            try
            {
                result = o.debitGlCreditCasa(kbiRef, act, kbiGL, amtBene, actCur, refnum, out fc_msg);
                if (result){ i = 1; s = ok.ToString(); }
                else { s = failed.ToString(); }
                ob[0] = trxnid;
                ob[1] = i; 
                ob[2] = DateTime.Now;
                ob[3] = s + api_message + "GL" + kbiGL + "CK" + act + messages + "seq_id=" + trxnid;
                this.update_txn(ob);
                oLog.writeAudit(sysid, "irsAgentCreditOtherTrxn.credit()", "Running Agent other credit module.", s, "", csAuditLog.Category.Select);
            }
            catch (Exception e)
            {
                messages = e.Message + ' ' + e.StackTrace;
                oLog.writeAudit(sysid, "irsAgentCreditOtherTrxn.credit()", "Running Agent other credit module.", "Error: " + e.Message + e.StackTrace, "", csAuditLog.Category.Select);
                return result;
            }            
            api_message = fc_msg;
            return result;
        }
        #endregion private Methods

        #region public Methods
        public void start()
        {
            dt                  = this.getdata();
            string refnum       = string.Empty;
            string sourceAcct   = string.Empty;
            string targetAcct   = string.Empty;
            string amt          = string.Empty;
            string narrative    = string.Empty;
            int trxnid          = 0;            
            int prodcode        = 0;
            DateTime txn_date   = DateTime.Now;                        
            string actCur       = string.Empty;
            bool ret            = false;
            try
            {
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            trxnid      = Convert.ToInt32(row["trxnid"]);
                            refnum      = (String)row["ReferenceNo"].ToString();
                            sourceAcct  = (String)row["SourceAccount"].ToString();
                            targetAcct  = (String)row["TargetAccount"].ToString();
                            amt         = (String)row["Amount"].ToString();
                            narrative   = (String)row["narrative"].ToString();
                            txn_date    = (DateTime)row["TranxDate"];
                            prodcode    = Convert.ToInt32(row["ProductCode"]);
                            actCur      = (String)row["actCur"].ToString();
                            ret         = this.credit(refnum, actCur, sourceAcct, amt, actCur, refnum, trxnid);
                        }
                    }
                }
            }
            catch(Exception e) 
            {
                oLog.writeAudit(sysid, "irsAgentCreditOtherTrxn.start()", "Running Agent other credit module.", "Error: " + e.Message + e.StackTrace, "", csAuditLog.Category.Select);
            }

        }

        public string Api_message
        {
            get { return this.api_message; }
            set { this.api_message = value; }
        }
        public string messages
        {
            get { return this.error_message; }
            set { this.error_message = value; }
        }
        public irsAgentCreditOtherTrxn()
        {
            this.error_message = null;
            base.IsWinApp = true;
        }
        ~irsAgentCreditOtherTrxn() { }
        #endregion public Methods

        
    }
}

