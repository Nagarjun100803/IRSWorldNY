using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace IRSClassLib
{
    [Serializable]
    public class irsMailer:irsSqlCommons
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
                            Recipient = (String[])row["Recipient"];
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
}
