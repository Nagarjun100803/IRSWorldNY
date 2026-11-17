/* Created by Rogelio D. Guanlao 06/08/2010*/
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    public class irsOverrideLog: sqlHelperWs
    {
        #region "Private Variables"

        private string _userid;
        private String UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        private string _module;
        private String Module
        {
            get { return _module; }
            set { _module = value; }
        }

        private string _overridetype;
        private String OverrideType
        {
            get { return _overridetype; }
            set { _overridetype = value; }
        }

        #endregion

        internal string getLookUpValue(string fldgroup, string fldvalue)
        {
            object objValue = execScalar("SELECT dbo.f_getLookUpValue('" + fldgroup + "','" + fldvalue + "')");
            return (string)objValue;

        }

        internal short getUserRole(string userID)
        {
            object objValue = execScalar("SELECT Role from tbl_users where userid = '" + userID + "'");
            return (short)objValue;
        }

        #region Public Method

        public irsOverrideLog(string UserID, string Module)
        {
            this.UserID = UserID;
            this.Module = Module;
        }

        #endregion

        public bool LogOverride(int txtSeqID)
        {
            string[] arrOverride = new string[6];
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            string overridedet = (String)System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"];
            bool bReturn = false;
            int iReturn = 0;
            try
            {
                if (overridedet != null)
                {
                    for (int i = 0; i < overridedet.Split('~').Length - 1; i++)
                    {
                        arrOverride[0] = txtSeqID.ToString();
                        arrOverride[1] = overridedet.Split('~')[i].Split('|')[0].ToString(); ;//this.UserID;
                        arrOverride[2] = new irsTimeZone().branchLocalTime(clientdata.Location).ToString();
                        arrOverride[3] = overridedet.Split('~')[i].Split('|')[1].ToString();//this.OverrideType;
                        arrOverride[4] = getLookUpValue("OverrideType", overridedet.Split('~')[i].Split('|')[1].ToString()) + "|" + clientdata.UserId;
                        arrOverride[5] = (this.Module == "" ? "" : this.Module);

                        iReturn = execStoredProcedureUpdate("sp_insert_override_log", arrOverride);
                    }

                    if (iReturn == -1)
                    {
                        bReturn = false;
                    }
                    else
                    {
                        /* VAPT 20181023 */
                        try
                        {
                            DataTable dt = new csAuditLog().getCIFTXN(txtSeqID, 0);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "OVERRIDE", "APPROVER APPROVED TRANSACTION OVERRIDE", dt.Rows[0]["ReferenceNo"].ToString(), "", txtSeqID.ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                new csAuditLog().writeVAPTLogger("", "OVERRIDE", "APPROVER APPROVED TRANSACTION OVERRIDE", "", "", txtSeqID.ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                        }
                        catch { }
                        bReturn = true;
                    }
                }
                else
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;
            }

            return bReturn;
        }

        public void WaiveCharges(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "1";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void InsufficientID(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "2";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void Charges(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "3";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void FxRates(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "4";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void TellersLimit(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "5";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void ReversedTranx(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "6";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void SupervisorLimit(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "7";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void ExpiredIDs(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "8";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void OFACRemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "9";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void OFACBeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "10";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void AMLARemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "11";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void AMLABeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "12";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void FincenRemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "13";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void FincenBeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "14";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void GlobalRemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "15";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void GlobalBeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "16";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void SanctionRemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "17";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void SanctionBeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "18";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void WatchlistRemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "19";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void WatchlistBeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "20";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void ExceptionsRemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "21";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void ExceptionsBeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "22";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void UnifiedRemitter(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "23";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void UnifiedBeneficiary(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "24";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void StreamLining(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "25";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void Refinitiv(string userID)
        {
            this.UserID = userID;
            this.OverrideType = "50";
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] += this.UserID + "|" + this.OverrideType + "~";
        }

        public void RemittanceLimit(string userID)
        {
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            if (clientdata.Role == 3)
            {
                if (getUserRole(userID) != 3)
                {
                    this.TellersLimit(userID);
                }                
            }
            else if (clientdata.Role == 4)
            {
                if (getUserRole(userID) != 4 && getUserRole(userID) != 3)
                {
                    this.SupervisorLimit(userID);
                }
            }
            else
            {
                if (getUserRole(userID) == 4)
                {
                    this.TellersLimit(userID);
                }
                else if(getUserRole(userID) !=3 && getUserRole(userID) != 4)
                {
                    this.SupervisorLimit(userID);
                }
            }
        }

        public void ResetLogSession()
        {
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;
        }
    }
}
