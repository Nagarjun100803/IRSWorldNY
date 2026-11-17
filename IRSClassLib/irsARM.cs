using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsARM : sqlHelperWs
    {
        /* Classes already loaded in Global.asax CHR 04082011
        irsClientData cd = new irsClientData();
        */        
        csAuditLog logger = new csAuditLog();

        public string getAppConfigValue(string Value, string LookValue, string EndValue)
        {
            int i = 0;
            int sIndex = 0;
            int eIndex = 0;
            int count = 0;
            int lenValue = LookValue.Length;
            int v = 0;
            int r = 0;
            string sLookValue = string.Empty;
            string returnString = string.Empty;

            try
            {
                Value = Value.ToLower();
                if (Value.Substring(Value.Length - 1, 1) != ";")
                {
                    Value = Value + ";";
                }
                for (v = 1; (v <= lenValue); v++)
                {
                    sLookValue += Value.Substring((v - 1), 1);
                }
                if (sLookValue == LookValue)
                {
                    for (r = 0; (r <= (lenValue - 1)); r++)
                    {
                        Value = Value.Remove(0, 1);
                    }
                }
                else
                {
                    sIndex = Value.IndexOf(LookValue);
                    eIndex = Value.IndexOf(EndValue);

                    if (eIndex < sIndex)
                    {
                        for (i = 0; i <= (sIndex + LookValue.Length - 1); i++)
                        {
                            Value = Value.Remove(0, 1);
                        }
                    }
                }
                eIndex = Value.IndexOf(EndValue);
                for (count = 1; (count <= eIndex); count++)
                {
                    returnString += Value.Substring((count - 1), 1);
                }
                return returnString;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataTable getARMDetails(string ReferenceNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getRemittance", new Object[] { ReferenceNo });
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

        public string getARMLimit(string BranchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getARMLimit", new Object[] { BranchCode });
            if (hasRows)
            {
                selectedRow = 0;
                if (table.Rows[0]["ARMLimit"].ToString() == string.Empty)
                {
                    return "0";
                }
                else
                {
                    return table.Rows[0]["ARMLimit"].ToString();
                }
            }
            else
            {
                return null;
            }
        }

        public string getBeneficiaryAddress(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getBeneficiaryAddress", new Object[] { CIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0]["Address"].ToString();
            }
            else
            {
                return null;
            }
        }

        public string getExchangeRate(string LocalDate)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getExchangeRate", new Object[] { System.Web.HttpContext.Current.Session["BranchCode"], System.Web.HttpContext.Current.Session["DefaultCurrency"], LocalDate });
            if (hasRows)
            {
                System.Web.HttpContext.Current.Session["SourceCurrency"] = table.Rows[0]["SourceCurrency"].ToString();
                System.Web.HttpContext.Current.Session["SourceCurrencyFldCode"] = table.Rows[0]["SourceCurrencyFldCode"].ToString();
                System.Web.HttpContext.Current.Session["SourceCurrencyCode"] = table.Rows[0]["SourceCurrencyCode"].ToString();
                System.Web.HttpContext.Current.Session["TargetCurrencyCode"] = table.Rows[0]["TargetCurrencyCode"].ToString();
                selectedRow = 0;
                return table.Rows[0]["SourceToTarget"].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public DataTable getGFMCBeneficiary()
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getGFMCBeneficiary", new Object[] { System.Web.HttpContext.Current.Session["RemitterID"] });
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

        public string getGFMCProductCode()
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getGFMCProductCode", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                int i = 0;
                string sReturn = string.Empty;

                for (i = 0; (i <= table.Rows.Count - 1); i++)
                {
                    sReturn += table.Rows[i]["FldCode"].ToString().Trim() + ",";
                }
                sReturn = sReturn.Substring(0, sReturn.Length - 1);
                return sReturn;
            }
            else
            {
                return null;
            }
        }

        public bool getGFMCRemitter(string CardNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getGFMCRemitter", new Object[] { CardNo });
            if (hasRows)
            {
                System.Web.HttpContext.Current.Session["GFMCNo"] = CardNo.ToString();
                System.Web.HttpContext.Current.Session["RemitterID"] = table.Rows[0]["CIFID"].ToString();
                System.Web.HttpContext.Current.Session["RemitterName"] = table.Rows[0]["RemitterName"].ToString();
                ClientData._userid = CardNo.ToString();
                ClientData._cifid = table.Rows[0]["CIFID"].ToString();
                System.Web.HttpContext.Current.Session["USER_ON_LOGIN"] = null;
                selectedRow = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool getKiosk(string MacAddress)
        {
            //MacAddress = "00:20:0C:26:C5:AB";
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getKiosk", new Object[] { MacAddress });
            if (hasRows)
            {
                System.Web.HttpContext.Current.Session["BranchCode"] = table.Rows[0]["BranchCode"].ToString();
                System.Web.HttpContext.Current.Session["NegBranchCode"] = table.Rows[0]["NegBranchCode"].ToString();
                System.Web.HttpContext.Current.Session["DefaultCurrency"] = table.Rows[0]["DefaultCurrency"].ToString();
                System.Web.HttpContext.Current.Session["ChargeGroup"] = table.Rows[0]["ChargeGroup"].ToString();
                selectedRow = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        public DataTable getMerchants(string RemitterID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getMerchants", new Object[] { RemitterID });
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

        public DataTable getMerchantInfo(string MerchantName, string ProductCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getMerchantInfo", new Object[] { MerchantName, ProductCode });
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

        public DataTable getMerchantLookup(string MerchantName)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getMerchantLookup", new Object[] { MerchantName });
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

        public DataTable getReferenceNo(string ORNo, DateTime TranxDate)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getReferenceNo", new Object[] { ORNo, TranxDate });
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

        public DataTable getRemitterCards()
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getRemitterCards", new Object[] { System.Web.HttpContext.Current.Session["RemitterID"] });
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

        public DataTable getTransactionPurpose()
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getTransactionPurpose", new Object[] { });
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

        public DataTable getTransactionSource()
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getTransactionSource", new Object[] { });
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

        public DataTable printReceipt(string ReferenceNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getRemittance", new Object[] { ReferenceNo });
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

        public void setLocation(string Location)
        {
            ClientData._location = Location;
        }

        public void setUserID(string UserID)
        {
            ClientData._userid = UserID;
        }

        public void writeLog(string spName, string spParam, int ResponseCode)
        {
            string userID = string.Empty;
            if (System.Web.HttpContext.Current.Session["GFMCNo"] != null && System.Web.HttpContext.Current.Session["GFMCNo"].ToString() != string.Empty)
            {
                userID = System.Web.HttpContext.Current.Session["GFMCNo"].ToString();
            }
            else
            {
                userID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            }
            logger.writeAudit(userID, System.Web.HttpContext.Current.Session["InternetARMModule"].ToString(), "EXECUTE INSERT: [ErrorMessage=" + Message + "][ResponseCode=" + ResponseCode + "]", "sp: " + spName, "param: " + spParam, csAuditLog.Category.Insert);
        }

        public int submitRemittance(string Remittance)
        {
            int i = 0;
            string[] arrRemittance;
            string[] arrRemittanceInsert = null;
            //DataTable dt;
            //DataRow dr;
            int iReturn = 0;

            try
            {
                arrRemittance = Remittance.Split('|');
                if (arrRemittance.Length > 0)
                {
                    //dt = createTable("Remittance");
                    //dr = dt.NewRow();
                    arrRemittanceInsert = new string[arrRemittance.Length];
                    for (i = 0; i <= (arrRemittance.Length - 1); i++)
                    {
                        if (arrRemittance.GetValue(i).ToString() == "NULL")
                        {
                            arrRemittanceInsert.SetValue("", i);
                        }
                        else
                        {
                            arrRemittanceInsert.SetValue(arrRemittance.GetValue(i), i);
                        }
                    }
                    //dt.Rows.Add(dr);
                    //iReturn = execStoredProcedureUpdate("ksp_InsertRemittance", dr);
                    iReturn = execStoredProcedureUpdate("ksp_InsertRemittance", arrRemittanceInsert);
                }
                else
                {
                    iReturn = -1;
                }
                writeLog("ksp_InsertRemittance", Remittance, iReturn);
                return iReturn;
            }
            catch (Exception ex)
            {
                writeLog("ksp_InsertRemittance", ex.Message, -1);
                return -1;
            }
        }

        public int submitOBPS(string[] OBPS)
        {
            int iReturn = 0;
            int i = 0;
            string[] arrOBPS;
            bool isError = false;
            string sParam = string.Empty;

            try
            {
                for (i = 0; i <= (OBPS.Length - 1); i++)
                {
                    arrOBPS = OBPS[i].Split('|');
                    sParam += OBPS[i] + "~";
                    iReturn = execStoredProcedureUpdate("ksp_InsertOBPS", arrOBPS);
                    if (iReturn == -1)
                    {
                        isError = true;
                    }
                    else
                    {
                        isError = false;
                    } 
                }
                writeLog("ksp_InsertOBPS", sParam, iReturn);
                return (isError == true) ? -1 : iReturn;
            }
            catch (Exception ex)
            {
                writeLog("ksp_InsertOBPS", ex.Message, -1);
                return -1;
            }
        }
    }
}