using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace IRSClassLib
{
    [Serializable]
    public class csAuditLog : sqlHelperWs
    {
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
            OAAM,
            IBFT,
            Systematics,
            AccessDenied,
            VAPT,
            Instapay,
            Refinitiv,
            RTPAPI
        }

        public bool checkLog(string SessionID, string Description)
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

        public void writeLogger(string Log, string TableName, string Key)
        {
            int i = 0;
            string[] arrSplit = Log.Split('|');
            string[] arrValue;
            irsAuditLogger logger = new irsAuditLogger();

            for (i = 0; i <= (arrSplit.Length - 1); i++)
            {
                arrValue = arrSplit[i].ToString().Split('~');
                logger.writeLog(TableName, arrValue[0].ToString(), 1, "", arrValue[1].ToString(), Key);
            }
        }

        public bool writeAudit(string UserID, string Module, string Description, string NewValue, string OldValue, Category Category)
        {
            string[] arrAudit = new string[7];
            string sUserID = string.Empty;
            int iReturn = 0;
            bool bIsWinApp = base.IsWinApp;

            if (UserID == string.Empty)
            {
                if (base.IsWinApp)
                {
                    sUserID = "CSAUDITLOG";
                }
                else
                {
                    try
                    {
                        sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                    }
                    catch (Exception ex)
                    {
                        sUserID = ex.Message;
                    }
                }
            }
            else
            {
                sUserID = UserID;
            }
            sUserID = (sUserID == null) ? string.Empty : sUserID;

            try
            {
                arrAudit[0] = Convert.ToString(this.getSessionID());
                arrAudit[1] = sUserID;
                arrAudit[2] = Module;
                arrAudit[3] = Description; //(Category.ToString() == "PaymentGateway") ? irsUsers.getTriDes().Encrypt(Description) : Description;
                arrAudit[4] = NewValue;
                arrAudit[5] = OldValue;
                arrAudit[6] = Category.ToString();

                base.IsWinApp = true;
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
                base.IsWinApp = bIsWinApp;
            }
        }

        public bool writeAuditSystem(string UserID, string Module, string Description, string NewValue, string OldValue, Category Category)
        {
            //IRSWorld 16236-009 T-Accounts
            string[] arrAudit = new string[7];
            string sUserID = string.Empty;
            int iReturn = 0;
            bool bIsWinApp = base.IsWinApp;

            if (string.IsNullOrEmpty(UserID) == false)
            {
                sUserID = UserID;
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["SES_LOG_USER_ID"] != null)
                {
                    sUserID = (string)System.Web.HttpContext.Current.Session["SES_LOG_USER_ID"];
                }
                else
                {
                    sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                }
            }

            try
            {
                arrAudit[0] = Convert.ToString(this.getSessionID());
                arrAudit[1] = sUserID;
                arrAudit[2] = Module;
                arrAudit[3] = Description; //(Category.ToString() == "PaymentGateway") ? irsUsers.getTriDes().Encrypt(Description) : Description;
                arrAudit[4] = NewValue;
                arrAudit[5] = OldValue;
                arrAudit[6] = Category.ToString();

                base.IsWinApp = true;
                iReturn = execStoredProcedureUpdate("sp_Insert_AuditLogSystem", arrAudit);
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
                base.IsWinApp = bIsWinApp;
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

        private void createLogs(string Message)
        {
            StreamWriter swFile;
            string sError = string.Empty;
            //StreamWriter swFile = new StreamWriter(Environment.GetEnvironmentVariable("TEMP") + @"\IRSWorldLog.nfo", true, Encoding.Default);
            try
            {
                swFile = new StreamWriter(@"C:\Temp\IRSWorldLog_" + generateYYMMDD() + ".txt", true, Encoding.Default);
            }
            catch (Exception ex)
            {
                sError = ex.Message;
                swFile = new StreamWriter(@"C:\Temp\IRSWorldLog_" + new irsPaymentGateway().GenerateFile() + ".txt", true, Encoding.Default);
            }
            string dtLog = DateTime.Now.Date.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();

            if (((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]) != null)
            {
                swFile.Write(dtLog +
                " [User ID: " + ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId + "] " +
                " [Location: " + ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location + "]" +
                " [Message: " + Message + "]" +
                " [Exception: " + sError + "]" + "\r\n");
            }
            else
            {
                swFile.Write(dtLog +
                " [User ID: Session Expired]" +
                " [Location: Session Expired]" +
                " [Message: " + Message + "]" +
                " [Exception: " + sError + "]" + "\r\n");
            }
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

        #region tbl_VAPT_Log
        public enum AuditType
        {
            User,
            Systems,
            Admin
        }

        public enum AlertType
        {
            Alert,
            Information,
            Process
        }

        /* VAPT 20190206 */
        public void writeVAPTLogger(string userID, string module, string activity, string oldValue, string newValue, string systemValue, AuditType auditType, AlertType alertType)
        {
            string userName = string.Empty;
            bool ok = false;
            bool allow = base.IsWinApp;
            base.IsWinApp = true;
            string location = string.Empty;
            string ipAddress = string.Empty;

            try
            {
                if (userID == string.Empty)
                {
                    userID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                }
                if (userID == null)
                {
                    userID = string.Empty;
                }
                else
                {
                    userName = this.getUserName(userID);
                }
                /*
                if (location == null || location == string.Empty)
                {
                    location = System.Configuration.ConfigurationManager.AppSettings["VAPTLocation"];
                }
                */
                try
                {
                    if (System.Web.HttpContext.Current.Session["IPAddress"] != null)
                    {
                        ipAddress = System.Web.HttpContext.Current.Session["IPAddress"].ToString();
                    }
                    int role = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Role;
                    if (role == 1 || role == 99)
                    {
                        auditType = AuditType.Admin;
                    }
                    location = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
                }
                catch {}

                if (location == null)
                {
                    location = "IRSW";
                }

                base.addMode = true;
                this.setBase("SELECT TOP 1 * FROM tbl_VAPT_Log", true);
                setColumn("UserID", userID.ToUpper());
                setColumn("UserName", userName.ToLower());
                setColumn("Module", module.ToLower());
                setColumn("Activity", activity.ToLower());
                setColumn("OldValue", oldValue.ToLower());
                setColumn("NewValue", newValue.ToLower());
                setColumn("SystemValue", systemValue.ToLower());
                setColumn("IPAddress", ipAddress);
                //if (location != string.Empty && location != "IRSW") RFC-20327 DMP 03102022
                if (!string.IsNullOrEmpty(location) && location != "IRSW") //RFC - 20327 DMP 03102022
                {
                    /* PNBS Integration Project */
                    //setColumn("DateCreated", new irsTimeZone().branchLocalTime_nWs(location));
                    try
                    {
                        setColumn("DateCreated", new irsTimeZone().branchLocalTime(location));
                    }
                    catch
                    {
                        setColumn("DateCreated", DateTime.Now);
                    }
                }
                else
                {
                    setColumn("DateCreated", DateTime.Now);
                }
                setColumn("AuditType", auditType);
                setColumn("AlertType", alertType);
                ok = base.save(sqlHelperWs.RETAIN_DATASET); 
            }
            catch (Exception ex)
            {
                writeAudit("", "VAPT", ex.Message, "", "", Category.Insert);
            }
            finally
            {
                base.IsWinApp = allow;
            }
        }

        private void setBase(string SQL, bool IsAdd)
        {
            bool allow = base.IsWinApp;
            base.IsWinApp = true;
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (IsAdd)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                selectedRow = 0;
            }
            base.IsWinApp = allow;
        }

        public string getUserName(string userID)
        {
            bool allow = base.IsWinApp;
            string ret = string.Empty;
            base.IsWinApp = true;
            base.EnforceConstraint = false;

            getUpdateDS("vasp_getUserName", new Object[] { userID });
            if (hasRows)
            {
                selectedRow = 0;
                ret = table.Rows[0]["FullName"].ToString();
            }
            base.IsWinApp = allow;
            return ret;
        }

        /* VAPT 20181023 */
        public DataTable getCIFTXN(int SEQ_ID, int IsCIF)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vasp_getTXNCIF", new Object[] { SEQ_ID, IsCIF });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}