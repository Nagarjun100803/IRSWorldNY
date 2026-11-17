using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;
using System.Collections;
using System.Web;
using System.IO;

namespace IRSClassLib
{
    [Serializable]
    public class irsAccountTransactionStatus : sqlHelperWs
    {
        csAuditLog audit = new csAuditLog();

        public irsAccountTransactionStatus()
        {
       
        }

        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (!base.hasRows)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                if (IsAdd)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    selectedRow = 0;
                }
            }
        }

        public DataTable getRecord()
        {
            try
            {
                base.EnforceConstraint = false;
                getUpdateDS("vsp_AccTranStatusGetData", new Object[] { });
                if (hasRows)
                {
                    return table;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                saveLog("Account Transaction Status", "getRecord Function -- ERROR: Unable to get record", "Error Message: " + ex.Message, "Stack Trace: " + ex.StackTrace, csAuditLog.Category.Update);
                return null;
            }
        }

        public string applyUpdates(string crud, int row, Hashtable ht, string OldValue)
        {
            string msg = "";
            string strNewValues = "";
            string strAccountDetails = "";
            string sUserID = "";
            bool ok = false;

            try
            {
                Object SEQ_ID = ht["SEQ_ID"];
                Boolean IsValidated = Convert.ToBoolean(ht["IsValidated"]);
                Boolean SkipACH = Convert.ToBoolean(ht["SkipACH"]);
                Boolean Deleted = Convert.ToBoolean(ht["Deleted"]);
                String AccountNo = Convert.ToString(ht["AccountNo"]);
                String RoutingNo = Convert.ToString(ht["RoutingNo"]);
                string[] _OldValues = OldValue.Split('|');
                string[] _OldValidated = _OldValues[0].Split(':');

                if (SkipACH == true && IsValidated == false)
                {
                    return "Account must be validated.";
                }

                try
                {
                    sUserID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
                }
                catch (Exception ex)
                {
                    sUserID = ex.Message;
                }

                this.setBase("SELECT TOP 1 * FROM tbl_cif_ACHWorks WHERE SEQ_ID = '" + SEQ_ID + "'", false);
                setColumn("IsValidated", IsValidated);
                setColumn("SkipACH", SkipACH);
                setColumn("Deleted", Deleted);

                if (OldValue != "")
                {
                    if (_OldValidated[1].ToString().ToUpper() == "FALSE" && IsValidated == true)
                    {
                        setColumn("ManualValidatedBy", sUserID);
                        setColumn("ManualDateValidated", getLocalTime());
                        setColumn("LastValidatedBy", sUserID);
                    }
                }

                ok = base.save(sqlHelperWs.RETAIN_DATASET);

                msg = (ok == true) ? "Record has been updated successfully." : "ERROR: Unable to update record.";

                strNewValues = "IsValidated:" + IsValidated + "|SkipACH:" + SkipACH + "|Deleted:" + Deleted + "|Message response: " + msg;
                strAccountDetails = "SEQ_ID:" + SEQ_ID + "|AccountNo:" + AccountNo + "|RoutingNo:" + RoutingNo;

                saveLog("Account Transaction Status", "Account Transaction Status Update -- Update Result: " + ok, "Account Details: " + strAccountDetails + " ---- New Value: " + strNewValues, "Old Value: " + OldValue, csAuditLog.Category.Update);
            }
            catch (Exception ex)
            {
                saveLog("Account Transaction Status", "applyUpdates Function -- ERROR: Account Transaction Status Update", "Error Message: " + ex.Message, "Stack Trace: " + ex.StackTrace, csAuditLog.Category.Update);
                return "ERROR: Unable to update Record";
            }
            return msg;
        }



        void saveLog(string Module, string Description, string OldValue, string NewValue, csAuditLog.Category ctg)
        {
            try
            {
                audit.writeAudit("", Module, Description, OldValue, NewValue, ctg);
            }
            catch (Exception ex)
            { }
        }

        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
        }

        public DateTime getLocalTime()
        {
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(getLocation());
            return dtLocalTime;
        }

    }
}
