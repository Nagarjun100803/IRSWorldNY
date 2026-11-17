using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;
using Bharosa.VCrypt.Common.Util;
using IRSWorld.bharosa;
using System.Collections.Generic;
using System.Linq;

namespace IRSWorld.TranClass
{
    [Serializable()]
    public class Refinitiv : sqlHelperWs
    {
        public Refinitiv()
        {
            base.EnforceConstraint = false;
        }

        private irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }

                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private String get_irsclient_data(String param)
        {
            if (irsClientData == null)
            {
                return null;
            }

            string s = null;


            switch (param)
            {
                case "UID": s = irsClientData.UserId; break;
                case "CIF": s = irsClientData.CIFID; break;
                case "LOCATION": s = irsClientData.Location; break;
                case "IP": s = irsClientData.IPAddress; break;
            }

            return s;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeLog(String Description)
        {
            writeLog(Description, "REFINITIV", "", "", csAuditLog.Category.Select);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeLog(String Description, String Module, String NewValue, String OldValue, csAuditLog.Category Category)
        {
            String userId = get_irsclient_data("UID");
            String sDescription = "Location=" + get_irsclient_data("LOCATION") + " : " + Description;
            new csAuditLog().writeAudit(userId, Module, sDescription, NewValue, OldValue, Category);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isRefinitivEnabled()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            string isRefinitivEnabled = "0";
            System.Web.HttpContext.Current.Session["RefinitivConfiguration_Flag"] = null;
            isRefinitivEnabled = commons.getDataSet("SELECT ISNULL(FldDescription,0) AS FldDescription FROM TBL_LOOKUP WHERE FLDGROUP = 'RefinitivConfiguration' AND FLDCODE = 'Flag' ", false, new object[] { }).Tables[0].Rows[0].ItemArray[0].ToString();
            if (isRefinitivEnabled == "1")
            {
                System.Web.HttpContext.Current.Session["RefinitivConfiguration_Flag"] = isRefinitivEnabled;
                return isRefinitivEnabled;
            }
            return isRefinitivEnabled;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_is_agent()
        {
            irsSqlCommonsWs helper = new irsSqlCommonsWs();
            int role = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Role;
            try
            {
                if ((int)helper.execStoredProcedure("sp_internet_check_userrole_if_agent", new Object[] { role }).Tables[0].Rows[0][0] > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.check_is_agent", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
                return false;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_refinitiv_is_down_OTC()
        {
            if (System.Web.HttpContext.Current.Session["REFINITIV_IS_DOWN"] != null)
            { return "Refinitiv is down."; }
            else { return ""; }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void get_refinitiv_dt(DataTable dt)
        {
            if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
            }
            System.Web.HttpContext.Current.Session["REFINITIV_DT"] = dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void clear_refinitiv_dt(DataTable dt)
        {
            System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void get_REFINITIV_OverrideChecking(Boolean REFINITIV_OverrideChecking)
        {
            if (REFINITIV_OverrideChecking == false)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"] = false;
            }
            System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"] = true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRefinitiv_casesystemid(string cifid)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;

            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("getRefinitiv_caseSystemId", new object[] { cifid });

            try
            {
                if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
                {
                    string casesystemid = ds_return.Tables[0].Rows[0]["caseSystemId"].ToString();

                    if (casesystemid != null || casesystemid != "")
                    {
                        return casesystemid;
                    }
                    else {
                        new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitivcs.getRefinitiv_casesystemid", "[Details, CIFI:" + cifid, "|SP: failed to get CaseSystemID. getRefinitiv_caseSystemId", csAuditLog.Category.Refinitiv);
                        return string.Empty;
                    }
                }
                else
                {
                    new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitivcs.getRefinitiv_casesystemid", "[Details, CIFI:" + cifid, "|SP: failed to get CaseSystemID. getRefinitiv_caseSystemId", csAuditLog.Category.Refinitiv);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitivcs.getRefinitiv_casesystemid", "[Details, CIFI:" + cifid, "|Ex:" + ex.Message + "|Stack:" + ex.StackTrace, csAuditLog.Category.Refinitiv);
                return string.Empty;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRefinitiv_ClientCIFID(int client_seqid)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;

            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("getRefinitiv_ClientCIFID", new object[] { client_seqid });

            try
            {
                if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
                {
                    string clientcifid = ds_return.Tables[0].Rows[0]["CIFID"].ToString();

                    if (clientcifid != null || clientcifid != "")
                    {
                        return clientcifid;
                    }
                    else {
                        new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitivcs.getRefinitiv_ClientCIFID", "[Details, CIFI SEQ_ID:" + client_seqid, "|SP: failed to get CIFID. getRefinitiv_ClientCIFID", csAuditLog.Category.Refinitiv);
                        return string.Empty;
                    }
                }
                else {
                    new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitivcs.getRefinitiv_ClientCIFID", "[Details, CIFI SEQ_ID:" + client_seqid, "|SP: failed to get CIFID. getRefinitiv_ClientCIFID", csAuditLog.Category.Refinitiv);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitivcs.getRefinitiv_ClientCIFID", "[Details, CIFI SEQ_ID:" + client_seqid , "|Ex:" + ex.Message +  "|Stack:" + ex.StackTrace, csAuditLog.Category.Refinitiv);
                return string.Empty;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRefinitiv_reasonfordiscounting(string cifid, int client_seqid)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("getRefinitiv_reasonfordiscounting", new object[] { cifid, client_seqid });

            if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
            {
                return ds_return;
            }
            else { return ds_return; }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRefinitiv_transdetails(int TXN_SEQ_ID)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("getRefinitiv_transdetails", new object[] { TXN_SEQ_ID });

            if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
            {
                return ds_return;
            }
            else { return ds_return; }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRefinitivHit_NatureOfOverride(int overridetype)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("getRefinitivHit_NatureOfOverride", new object[] { overridetype });

            if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
            {
                return ds_return;
            }
            else { return ds_return; }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void Refinitiv_ArchiveDelete(string cifid, bool ArchiveOnly, string userid)
        {
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = new DataTable();
            try
            {
                new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitivcs.Refinitiv_ArchiveDelete", "CIFI=" + cifid + " |ArchiveOnly=" + ArchiveOnly + " |UserID=" + userid, "", csAuditLog.Category.Refinitiv);
                dt = irsrefinitiv.getRefinitivByCaseId(cifid);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        irsrefinitiv.ArchiveDelete(dt.Rows[0]["CaseSystemID"].ToString(), ArchiveOnly, userid);
                    }
                }
                else {
                    new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitiv.Refinitiv_ArchiveDelete", "Unable to get CaseSystemID", "[Details, CIFID:" + cifid + "|ArchiveOnly:" + ArchiveOnly, csAuditLog.Category.Refinitiv);
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"] = false;
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.Refinitiv_ArchiveDelete", ex.Message + "|" + ex.StackTrace, "[Details, CIFID:" + cifid, csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to archive and delete refinitiv, Reason : " + ex.Message);
            }
            finally
            {
                System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"] = false;
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ArchiveDelete_CaseSystemID()
        {
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = new DataTable();
            string CaseSystemId = "";
            string userid = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            try
            {
                new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitivcs.ArchiveDelete_CaseSystemID", "CaseSystemId=" + "" + " |ArchiveOnly=" + "" + " |UserID=" + userid, "", csAuditLog.Category.Refinitiv);
                if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            CaseSystemId = dt.Rows[i]["CaseSystemId"].ToString();
                            irsrefinitiv.ArchiveDelete(CaseSystemId, false, userid);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.ArchiveDelete_CaseSystemID", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to archive and delete refinitiv, Reason : " + ex.Message);
            }
            finally
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void saveWatchlist_details_for_cancellation(string cifid)
        {
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = new DataTable();
            string CaseSystemId = "";
            string userid = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            try
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.saveWatchlist_details_for_cancellation", "cifid=" + cifid, "", csAuditLog.Category.Refinitiv);
                if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        CaseSystemId = dt.Rows[0]["CaseSystemId"].ToString();
                        irsrefinitiv.ArchiveDelete(CaseSystemId, false, userid);
                    }
                }
                System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"] = false;
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"] = false;
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.saveWatchlist_details_for_cancellation", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to archive and delete refinitiv, Reason : " + ex.Message);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void saveWatchlist_details_pendingoverride(DataTable dtrefinitiv, string cifid, int hitCount)
        {
            try
            {
                if (dtrefinitiv != null)
                {
                    if (dtrefinitiv.Rows.Count > 0)
                    {
                        new irsRefinitiv().SaveWatchlist(dtrefinitiv, cifid, false, hitCount);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.saveWatchlist_details_pendingoverride", ex.Message + "|" + ex.StackTrace, "", csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to save refinitiv details, Reason : " + ex.Message );
            }
            finally
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                if (dtrefinitiv != null)
                {
                    dtrefinitiv.Dispose();
                    dtrefinitiv = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void saveWatchlist_details(string cifid)
        {
            DataTable dt = new DataTable();
            Boolean REFINITIV_OverrideChecking = false;
            REFINITIV_OverrideChecking = (Boolean)System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"];
            dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
            try
            {
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        new irsRefinitiv().SaveWatchlist(dt, cifid, REFINITIV_OverrideChecking);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.saveWatchlist_details", ex.Message + "|" + ex.StackTrace, "CIFID: " + cifid, csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to save refinitiv details, Reason : " + ex.Message);
            }
            finally
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void saveWatchlist_UpdateCIFID_Merchant(string CaseSystemID, string cifid)
        {
            string caseSystemId = string.Empty;
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = new DataTable();
            dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
            try
            {
                if (dt != null)
                {

                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty)
                        {
                            irsrefinitiv.UpdateWatchlistCIFID(dt.Rows[0][1].ToString(), cifid);
                        }
                        else { irsrefinitiv.UpdateWatchlistCIFID(dt.Rows[0]["CaseSystemID"].ToString(), cifid); }
                    }
                }
                else if (CaseSystemID != "")
                {
                    irsrefinitiv.UpdateWatchlistCIFID(CaseSystemID, cifid);
                }
                else
                {
                    DataTable dtCaseId = irsrefinitiv.getRefinitivByCaseId(cifid);
                    if (dtCaseId != null && dtCaseId.Rows.Count > 0)
                    {
                        caseSystemId = dtCaseId.Rows[0]["CaseSystemID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.saveWatchlist_UpdateCIFID", ex.Message + "|" + ex.StackTrace, "CIFID: " + cifid, csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to update cifid in refinitiv details, Reason : " + ex.Message);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void saveWatchlist_UpdateCIFID(string CaseSystemID, string cifid)
        {
            string caseSystemId = string.Empty;
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = new DataTable();
            dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
            try
            {
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty)
                        {
                            irsrefinitiv.UpdateWatchlistCIFID(dt.Rows[0][1].ToString(), cifid);
                        }
                        else { irsrefinitiv.UpdateWatchlistCIFID(dt.Rows[0]["CaseSystemID"].ToString(), cifid); }
                    }
                }
                else if (CaseSystemID != "")
                {
                    irsrefinitiv.UpdateWatchlistCIFID(CaseSystemID, cifid);
                }
                else {
                    DataTable dtCaseId = irsrefinitiv.getRefinitivByCaseId(cifid);
                    if (dtCaseId != null && dtCaseId.Rows.Count > 0)
                    {
                        caseSystemId = dtCaseId.Rows[0]["CaseSystemID"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.saveWatchlist_UpdateCIFID", ex.Message + "|" + ex.StackTrace, "CIFID: " + cifid, csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to update cifid in refinitiv details, Reason : " + ex.Message);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void saveWatchlist_UpdateByCaseSystemID(string CaseSystemID, string cifid)
        {
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            try
            {
                if (CaseSystemID != null)
                {
                    irsrefinitiv.UpdateWatchlistCIFID(CaseSystemID, cifid);
                }
                System.Web.HttpContext.Current.Session["REFINITIV_OverrideChecking"] = false;
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.saveWatchlist_UpdateByCaseSystemID", ex.Message + "|" + ex.StackTrace, "CIFID: " + cifid, csAuditLog.Category.Refinitiv);
                throw new Exception("Failed to update cifid in refinitiv details, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ResolveWatchlist(string CIFID, bool Positive, string Remarks, string userid)
        {
            DataTable dt = new DataTable();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            try
            {
                if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
                    new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitiv.ResolveWatchlist", "", "CIFID: " + CIFID + " | Positive: " + Positive + " | Remarks: " + Remarks + " | Approver: " + userid, csAuditLog.Category.Refinitiv);
                    irsrefinitiv.ResolveWatchlist_CaseSystemID(branch_code, CIFID, Positive, Remarks, userid, dt.Rows[0]["CaseSystemID"].ToString());

                }
                else if (CIFID != null)
                {
                    new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitiv.ResolveWatchlist", "", "CIFID: " + CIFID + " | Positive: " + Positive + " | Remarks: " + Remarks + " | Approver: " + userid, csAuditLog.Category.Refinitiv);
                    irsrefinitiv.ResolveWatchlist(branch_code, CIFID, Positive, Remarks, userid);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.ResolveWatchlist", ex.Message + "|" + ex.StackTrace, "CIFID: " + CIFID, csAuditLog.Category.Refinitiv);
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ResolveWatchlist_viaCIFID(string CIFID, bool Positive, string Remarks, string userid)
        {
            DataTable dt = new DataTable();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            try
            {
                if (CIFID != null)
                {
                    new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitiv.ResolveWatchlist", "", "CIFID: " + CIFID + " | Positive: " + Positive + " | Remarks: " + Remarks + " | Approver: " + userid, csAuditLog.Category.Refinitiv);
                    irsrefinitiv.ResolveWatchlist(branch_code, CIFID, Positive, Remarks, userid);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.ResolveWatchlist", ex.Message + "|" + ex.StackTrace, "CIFID: " + CIFID, csAuditLog.Category.Refinitiv);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable DisplayWatchlist(string FirstName, string MiddleName, string LastName, string cifid)
        {
            /* CIF ONBOARDING */
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            string existingcifid = "";
            bool down = false;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual = false;
            List<string> secondary = new List<string>();

            if (cifid != "")
            {
                existingcifid = cifid;
            }

            try
            {
                if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
                }
                else
                {
                    refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)new irsRefinitiv().ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);
                    if (down == true)
                    {
                        throw new Exception("Refinitiv is down.");
                    }
                    else
                    {
                        dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, existingcifid, false, "", "", "", isRefinitivIndividual, secondary, out down);
                    }
                }

                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt;
                    }
                    else
                    {
                        throw new Exception("Cannot connect to Refinitiv DisplayWatchlist");
                    }
                }
                else
                {
                    throw new Exception("Cannot connect to Refinitiv DisplayWatchlist");
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlist", ex.Message + "|" + ex.StackTrace, "[Details, FirstName:" + FirstName + " MiddleName: " + MiddleName + " LastName: " + LastName + " CIFID:" + cifid, csAuditLog.Category.Refinitiv);
                //writeLog("Failed to get Refinitiv DisplayWatchlist info, Reason : " + ex.Message + " [Details, FirstName:" + FirstName + " MiddleName: " + MiddleName + " LastName: " + LastName + " CIFID:" + cifid);
                throw new Exception("Failed to get Refinitiv DisplayWatchlist info, Reason : " + ex.Message);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> DisplayWatchlist_DT(string FirstName, string MiddleName, string LastName, string cifid)
        {
            /* CIF ONBOARDING */
            DataTable dt = null;
            DataSet ds = new DataSet();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            string existingcifid = "";
            bool down = false;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual = false;
            List<string> secondary = new List<string>();

            if (cifid != "")
            {
                existingcifid = cifid;
            }
            try
            {
                if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
                }
                else
                {
                    refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)new irsRefinitiv().ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);
                    if (down == true)
                    {
                        throw new Exception("Refinitiv is down.");
                    }
                    else
                    {
                        dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, existingcifid, false, "", "", "", isRefinitivIndividual, secondary, out down);
                    }
                }
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.AsEnumerable().ToList();
                    }
                    else
                    {
                        throw new Exception("Cannot connect to Refinitiv DisplayWatchlist_DT");
                    }
                }
                else
                {
                    throw new Exception("Cannot connect to Refinitiv DisplayWatchlist_DT");
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlist_DT", ex.Message + "|" + ex.StackTrace, "[Details, FirstName:" + FirstName + " MiddleName: " + MiddleName + " LastName: " + LastName + " CIFID:" + cifid, csAuditLog.Category.Refinitiv);
                //writeLog("Failed to get Refinitiv DisplayWatchlist_DT info, Reason : " + ex.Message + " [Details, FirstName:" + FirstName + " MiddleName: " + MiddleName + " LastName: " + LastName + " CIFID:" + cifid);
                throw new Exception("Failed to get Refinitiv DisplayWatchlist_DT info, Reason : " + ex.Message);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> DisplayWatchlistOnboarding_DT(string FirstName, string MiddleName, string LastName, string cifid, bool isRefinitivIndividual, string Gender, string DOB, string COB, string POB, string Nationality)
        {
            /* CIF ONBOARDING */
            DataTable dt = null;
            DataSet ds = new DataSet();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            string existingcifid = "";
            bool down = false;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;

            List<string> secondary = new List<string>();
            if (Gender != string.Empty || Gender != null)
            {
                secondary.Add("GENDER=" + Gender.ToUpper());
            }
            if (DOB != string.Empty || DOB != null)
            {
                secondary.Add("DOB=" + DOB.ToUpper());
            }
            if (COB != string.Empty || COB != null)
            {
                secondary.Add("COB=" + COB.ToUpper());
            }
            if (POB != string.Empty || POB != null)
            {
                secondary.Add("POB=" + POB.ToUpper());
            }
            if (Nationality != string.Empty || Nationality != null)
            {
                secondary.Add("NATIONALITY=" + Nationality.ToUpper());
            }


            if (cifid != null)
            {
                existingcifid = cifid;
            }
            try
            {
                if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
                }
                else
                {
                    refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)new irsRefinitiv().ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);
                    if (down == true)
                    {
                        throw new Exception("Refinitiv is down.");
                    }
                    else
                    {
                        dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, existingcifid, false, "", "", "", isRefinitivIndividual, secondary, out down);
                    }
                }
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty) //NO HITS
                        {
                            throw new Exception("Cannot connect to Refinitiv DisplayWatchlistOnboarding_DT");
                        }
                        else { return dt.AsEnumerable().ToList(); }
                    }
                    else
                    {
                        throw new Exception("Cannot connect to Refinitiv DisplayWatchlistOnboarding_DT");
                    }
                }
                else
                {
                    throw new Exception("Cannot connect to Refinitiv DisplayWatchlistOnboarding_DT");
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlistOnboarding_DT", ex.Message + "|" + ex.StackTrace, "[Details, FirstName:" + FirstName + " MiddleName: " + MiddleName + " LastName: " + LastName + " CIFID:" + cifid, csAuditLog.Category.Refinitiv);
                //writeLog("Failed to get Refinitiv DisplayWatchlistOnboarding_DT info, Reason : " + ex.Message + " [Details, FirstName:" + FirstName + " MiddleName: " + MiddleName + " LastName: " + LastName + " CIFID:" + cifid);
                throw new Exception("Failed to get Refinitiv DisplayWatchlistOnboarding_DT info, Reason : " + ex.Message);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> DisplayWatchlistOnboardingPendingOverride_DT(string FirstName, string MiddleName, string LastName, string cifid)
        {
            DataTable dt = null;
            DataSet ds = new DataSet();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
            bool down = false;
            string caseSystemId = string.Empty;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual = false;

            List<string> secondary = new List<string>();
            DataTable dtCIF = irsrefinitiv.getCIFInfo(cifid);
            if (dtCIF.Rows[0]["CIFType"] != DBNull.Value && dtCIF.Rows[0]["CIFType"].ToString() != string.Empty)
            {
                if (dtCIF.Rows[0]["CIFType"].ToString().Trim() == "1")
                {
                    isRefinitivIndividual = true;
                }
                else
                {
                    isRefinitivIndividual = false;
                }
            }

            try
            {

                dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down);
                if (down == true)
                {
                    throw new Exception("Refinitiv is down.");
                }
                else
                {
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty) //NO HITS
                            {
                                throw new Exception("Data is not available in Refinitiv. For manual validation.");
                            }
                            else
                            {
                                caseSystemId = this.getRefinitiv_casesystemid(cifid);
                                if (caseSystemId == string.Empty)
                                {
                                    int hitCount = 0;
                                    if (refinitiv != null)
                                    {
                                        foreach (var item in refinitiv)
                                        {
                                            hitCount = item.responseBody.results.Count;
                                        }
                                    }
                                    this.saveWatchlist_details_pendingoverride(dt, cifid, hitCount);
                                }
                                return dt.AsEnumerable().ToList();
                            }
                        }
                        else
                        {
                            throw new Exception("Data is not available in Refinitiv. For manual validation.");
                        }
                    }
                    else
                    {
                        throw new Exception("Data is not available in Refinitiv. For manual validation.");
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlistOnboardingPendingOverride_DT", ex.Message + "|" + ex.StackTrace, "[Details, FirstName:" + FirstName + " MiddleName: " + MiddleName + " LastName: " + LastName + " CIFID:" + cifid, csAuditLog.Category.Refinitiv);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null)
                {
                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                    dt.Dispose();
                    dt = null;
                }
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> DisplayWatchlistTransactionalPendingOverride_DT(string remitter_id, string beneficiary_id, int override_type)
        {
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = null;
            DataTable remitter_dt = null;
            DataTable beneficiary_dt = null;
            DataSet ds_natureofoverride = null;
            bool remitter_down = false;
            bool beneficiary_down = false;
            string caseSystemId_Remitter_id = string.Empty;
            string caseSystemId_Beneficiary_id = string.Empty;
            IEnumerable<irsRefinitiv.ScreeningResponseDto> remitter_refinitiv = null;
            IEnumerable<irsRefinitiv.ScreeningResponseDto> beneficiary_refinitiv = null;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual_remitter = false;
            bool isRefinitivIndividual_beneficiary = false;

            try
            {
                ds_natureofoverride = this.getRefinitivHit_NatureOfOverride(override_type);
                if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Remitter"))
                {
                    List<string> secondary_remitter = new List<string>();
                    DataTable dtCIF_remitter = irsrefinitiv.getCIFInfo(remitter_id);
                    if (dtCIF_remitter.Rows[0]["CIFType"] != DBNull.Value && dtCIF_remitter.Rows[0]["CIFType"].ToString() != string.Empty)
                    {
                        if (dtCIF_remitter.Rows[0]["CIFType"].ToString().Trim() == "1")
                        {
                            isRefinitivIndividual_remitter = true;
                        }
                        else
                        {
                            isRefinitivIndividual_remitter = false;
                        }
                    }
                    remitter_dt = irsrefinitiv.DisplayWatchlist(branch_code, remitter_refinitiv, remitter_id, false, "", "", "", isRefinitivIndividual_remitter, secondary_remitter, out remitter_down);
                }
                else if (ds_natureofoverride.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString().Contains("Beneficiary"))
                {
                    List<string> secondary_beneficiary = new List<string>();
                    DataTable dtCIF_beneficiary = irsrefinitiv.getCIFInfo(beneficiary_id);
                    if (dtCIF_beneficiary.Rows[0]["CIFType"] != DBNull.Value && dtCIF_beneficiary.Rows[0]["CIFType"].ToString() != string.Empty)
                    {
                        if (dtCIF_beneficiary.Rows[0]["CIFType"].ToString().Trim() == "1")
                        {
                            isRefinitivIndividual_beneficiary = true;
                        }
                        else
                        {
                            isRefinitivIndividual_beneficiary = false;
                        }
                    }
                    beneficiary_dt = irsrefinitiv.DisplayWatchlist(branch_code, beneficiary_refinitiv, beneficiary_id, false, "", "", "", isRefinitivIndividual_beneficiary, secondary_beneficiary, out beneficiary_down);
                }
                else
                {
                    List<string> secondary_remitter = new List<string>();
                    DataTable dtCIF_remitter = irsrefinitiv.getCIFInfo(remitter_id);
                    if (dtCIF_remitter.Rows[0]["CIFType"] != DBNull.Value && dtCIF_remitter.Rows[0]["CIFType"].ToString() != string.Empty)
                    {
                        if (dtCIF_remitter.Rows[0]["CIFType"].ToString().Trim() == "1")
                        {
                            isRefinitivIndividual_remitter = true;
                        }
                        else
                        {
                            isRefinitivIndividual_remitter = false;
                        }
                    }
                    new csAuditLog().writeAudit("", "REFINITIV", "DisplayWatchlistTransactionalPendingOverride_DT.irsrefinitiv.DisplayWatchlist", "Remitter", " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                    remitter_dt = irsrefinitiv.DisplayWatchlist(branch_code, remitter_refinitiv, remitter_id, false, "", "", "", isRefinitivIndividual_remitter, secondary_remitter, out remitter_down);

                    List<string> secondary_beneficiary = new List<string>();
                    DataTable dtCIF_beneficiary = irsrefinitiv.getCIFInfo(beneficiary_id);
                    if (dtCIF_beneficiary.Rows[0]["CIFType"] != DBNull.Value && dtCIF_beneficiary.Rows[0]["CIFType"].ToString() != string.Empty)
                    {
                        if (dtCIF_beneficiary.Rows[0]["CIFType"].ToString().Trim() == "1")
                        {
                            isRefinitivIndividual_beneficiary = true;
                        }
                        else
                        {
                            isRefinitivIndividual_beneficiary = false;
                        }
                    }
                    new csAuditLog().writeAudit("", "REFINITIV", "DisplayWatchlistTransactionalPendingOverride_DT.irsrefinitiv.DisplayWatchlist", "Beneficiary", " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                    beneficiary_dt = irsrefinitiv.DisplayWatchlist(branch_code, beneficiary_refinitiv, beneficiary_id, false, "", "", "", isRefinitivIndividual_beneficiary, secondary_beneficiary, out beneficiary_down);
                }

                DataSet ds = new DataSet();

                if (remitter_down == true)
                {
                    throw new Exception("Refinitiv is down.");
                }
                else if (beneficiary_down == true)
                {
                    throw new Exception("Refinitiv is down.");
                }
                else
                {
                    if (remitter_dt != null && remitter_dt.Rows.Count > 0) //WITH REMITTER HIT Rows Count
                    {
                        if (remitter_dt.Rows.Count == 1 && (remitter_dt.Rows[0][2] == DBNull.Value || remitter_dt.Rows[0][2].ToString() == string.Empty)) //NO REMITTER HITS
                        {
                            if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY HIT Rows Count
                            {
                                if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) //NO BENEFICIARY HITS
                                {
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                                }
                                else
                                {
                                    //WITH BENEFICIARY HITS ONLY
                                    caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                    if (caseSystemId_Beneficiary_id == string.Empty)
                                    {
                                        int hitCount = 0;
                                        if (beneficiary_refinitiv != null)
                                        {
                                            foreach (var item in beneficiary_refinitiv)
                                            {
                                                hitCount = item.responseBody.results.Count;
                                            }
                                        }
                                        this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCount);
                                    }
                                    else { caseSystemId_Beneficiary_id = string.Empty; }
                                    this.get_refinitiv_dt(beneficiary_dt);
                                    return beneficiary_dt.AsEnumerable().ToList();
                                }
                            }
                            else {
                                // NO HIT BOTH REMITTER AND BENEFICIARY
                                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                            }
                        }
                        else 
                        {
                            //START WITH REMITTER HIT
                            caseSystemId_Remitter_id = this.getRefinitiv_casesystemid(remitter_id);
                            if (caseSystemId_Remitter_id == string.Empty)
                            {
                                int hitCount = 0;
                                if (remitter_refinitiv != null)
                                {
                                    foreach (var item in remitter_refinitiv)
                                    {
                                        hitCount = item.responseBody.results.Count;
                                    }
                                }
                                this.saveWatchlist_details_pendingoverride(remitter_dt, remitter_id, hitCount);
                            }
                            else {
                                caseSystemId_Remitter_id = string.Empty;
                            }
                            if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY HIT Rows Count
                            {
                                if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) //NO BENEFICIARY HITS
                                {
                                    //with remitter but no beneficiary
                                    dt = remitter_dt;
                                    this.get_refinitiv_dt(dt);
                                    return dt.AsEnumerable().ToList();
                                    //System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                                }
                                else
                                {
                                    //START WITH BENEFICIARY HIT
                                    caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                    if (caseSystemId_Beneficiary_id == string.Empty)
                                    {
                                        int hitCount = 0;
                                        if (beneficiary_refinitiv != null)
                                        {
                                            foreach (var item in beneficiary_refinitiv)
                                            {
                                                hitCount = item.responseBody.results.Count;
                                            }
                                        }
                                        this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCount);
                                    }
                                    else { caseSystemId_Beneficiary_id = string.Empty; }
                                    remitter_dt.Merge(beneficiary_dt);
                                    this.get_refinitiv_dt(remitter_dt);
                                    return remitter_dt.AsEnumerable().ToList();
                                }
                            }
                            else {
                                //NO BENEFICIARY HIT Rows Count
                                dt = remitter_dt;
                                this.get_refinitiv_dt(dt);
                                return dt.AsEnumerable().ToList(); }
                        }
                    } //END WITH REMITTER ROW COUNT
                    else
                    {
                        if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY ROW COUNT
                        {
                            if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) //NO BENEFICIAIRY HITS
                            {
                                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                            }
                            else
                            {
                                caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                if (caseSystemId_Beneficiary_id == string.Empty)
                                {
                                    int hitCount = 0;
                                    if (beneficiary_refinitiv != null)
                                    {
                                        foreach (var item in beneficiary_refinitiv)
                                        {
                                            hitCount = item.responseBody.results.Count;
                                        }
                                    }
                                    this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCount);
                                }
                                else { caseSystemId_Beneficiary_id = string.Empty; }
                                //ds.Tables.AddRange(new DataTable[] { beneficiary_dt });
                                //dt = ds.Tables[0];
                                //remitter_dt.Merge(beneficiary_dt);
                                this.get_refinitiv_dt(beneficiary_dt);
                                return beneficiary_dt.AsEnumerable().ToList();
                            }
                        } //NO REMITTER HIT
                        else { System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv."); }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlistTransactionalPendingOverride_DT", ex.Message + "|" + ex.StackTrace, " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null)
                {
                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> DisplayWatchlistOnboardingReviewModule_DT(string FirstName, string MiddleName, string LastName, string cifid)
        {
            DataTable dt = null;
            DataSet ds = new DataSet();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
            bool down = false;
            string caseSystemId = string.Empty;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual = false;

            List<string> secondary = new List<string>();
            DataTable dtCIF = irsrefinitiv.getCIFInfo(cifid);
            if (dtCIF.Rows[0]["CIFType"] != DBNull.Value && dtCIF.Rows[0]["CIFType"].ToString() != string.Empty)
            {
                if (dtCIF.Rows[0]["CIFType"].ToString().Trim() == "1")
                {
                    isRefinitivIndividual = true;
                }
                else
                {
                    isRefinitivIndividual = false;
                }
            }

            try
            {

                dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down);

                if (down == true)
                {
                    throw new Exception("Refinitiv is down.");
                }
                else
                {
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty) //NO HITS
                            {
                                throw new Exception("Data is not available in Refinitiv. For manual validation.");
                            }
                            else
                            {
                                caseSystemId = this.getRefinitiv_casesystemid(cifid);
                                if (caseSystemId == string.Empty)
                                {
                                    int hitCount = 0;
                                    if (refinitiv != null)
                                    {
                                        foreach (var item in refinitiv)
                                        {
                                            hitCount = item.responseBody.results.Count;
                                        }
                                    }
                                    this.saveWatchlist_details_pendingoverride(dt, cifid, hitCount);
                                }
                                return dt.AsEnumerable().ToList();
                            }
                        }
                        else
                        {
                            throw new Exception("Data is not available in Refinitiv. For manual validation.");
                        }
                    }
                    else
                    {
                        throw new Exception("Data is not available in Refinitiv. For manual validation.");
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlistOnboardingReviewModule_DT", ex.Message + "|" + ex.StackTrace, " [FirsName:" + FirstName + "][MiddleName: " + MiddleName + "][LastName: " + LastName + "][cifid: " + cifid + "]", csAuditLog.Category.Refinitiv);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null)
                {
                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                    dt.Dispose();
                    dt = null;
                }
            }
        }



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> DisplayWatchlistTransactionalReviewModule_DT(string remitter_id, string beneficiary_id)
        {
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = null;
            DataTable remitter_dt = null;
            DataTable beneficiary_dt = null;
            DataSet ds_natureofoverride = null;
            bool remitter_down = false;
            bool beneficiary_down = false;
            string caseSystemId_Remitter_id = string.Empty;
            string caseSystemId_Beneficiary_id = string.Empty;
            IEnumerable<irsRefinitiv.ScreeningResponseDto> remitter_refinitiv = null;
            IEnumerable<irsRefinitiv.ScreeningResponseDto> beneficiary_refinitiv = null;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual_remitter = false;
            bool isRefinitivIndividual_beneficiary = false;

            try
            {
                List<string> secondary_remitter = new List<string>();
                DataTable dtCIF_remitter = irsrefinitiv.getCIFInfo(remitter_id);
                if (dtCIF_remitter.Rows[0]["CIFType"] != DBNull.Value && dtCIF_remitter.Rows[0]["CIFType"].ToString() != string.Empty)
                {
                    if (dtCIF_remitter.Rows[0]["CIFType"].ToString().Trim() == "1")
                    {
                        isRefinitivIndividual_remitter = true;
                    }
                    else
                    {
                        isRefinitivIndividual_remitter = false;
                    }
                }
                new csAuditLog().writeAudit("", "REFINITIV", "DisplayWatchlistTransactionalReviewModule_DT.irsrefinitiv.DisplayWatchlist", "Remitter", " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                remitter_dt = irsrefinitiv.DisplayWatchlist(branch_code, remitter_refinitiv, remitter_id, false, "", "", "", isRefinitivIndividual_remitter, secondary_remitter, out remitter_down);

                List<string> secondary_beneficiary = new List<string>();
                DataTable dtCIF_beneficiary = irsrefinitiv.getCIFInfo(beneficiary_id);
                if (dtCIF_beneficiary.Rows[0]["CIFType"] != DBNull.Value && dtCIF_beneficiary.Rows[0]["CIFType"].ToString() != string.Empty)
                {
                    if (dtCIF_beneficiary.Rows[0]["CIFType"].ToString().Trim() == "1")
                    {
                        isRefinitivIndividual_beneficiary = true;
                    }
                    else
                    {
                        isRefinitivIndividual_beneficiary = false;
                    }
                }
                new csAuditLog().writeAudit("", "REFINITIV", "DisplayWatchlistTransactionalReviewModule_DT.irsrefinitiv.DisplayWatchlist", "Beneficiary", " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                beneficiary_dt = irsrefinitiv.DisplayWatchlist(branch_code, beneficiary_refinitiv, beneficiary_id, false, "", "", "", isRefinitivIndividual_beneficiary, secondary_beneficiary, out beneficiary_down);
                

                DataSet ds = new DataSet();

                if (remitter_down == true)
                {
                    throw new Exception("Refinitiv is down.");
                }
                else if (beneficiary_down == true)
                {
                    throw new Exception("Refinitiv is down.");
                }
                else
                {
                    if (remitter_dt != null && remitter_dt.Rows.Count > 0) //WITH REMITTER HIT Rows Count
                    {
                        if (remitter_dt.Rows.Count == 1 && (remitter_dt.Rows[0][2] == DBNull.Value || remitter_dt.Rows[0][2].ToString() == string.Empty)) //NO REMITTER HITS
                        {
                            if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY HIT Rows Count
                            {
                                if (beneficiary_dt.Rows.Count == 1 && (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty)) //NO BENEFICIARY HITS
                                {
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                                }
                                else
                                {
                                    //WITH BENEFICIARY HITS ONLY
                                    caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                    if (caseSystemId_Beneficiary_id == string.Empty)
                                    {
                                        int hitCount = 0;
                                        if (beneficiary_refinitiv != null)
                                        {
                                            foreach (var item in beneficiary_refinitiv)
                                            {
                                                hitCount = item.responseBody.results.Count;
                                            }
                                        }
                                        this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCount);
                                    }
                                    else { caseSystemId_Beneficiary_id = string.Empty; }
                                    this.get_refinitiv_dt(beneficiary_dt);
                                    return beneficiary_dt.AsEnumerable().ToList();
                                }
                            }
                            else
                            {
                                // NO HIT BOTH REMITTER AND BENEFICIARY
                                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                            }
                        }
                        else
                        {
                            //START WITH REMITTER HIT
                            caseSystemId_Remitter_id = this.getRefinitiv_casesystemid(remitter_id);
                            if (caseSystemId_Remitter_id == string.Empty)
                            {
                                int hitCount = 0;
                                if (remitter_refinitiv != null)
                                {
                                    foreach (var item in remitter_refinitiv)
                                    {
                                        hitCount = item.responseBody.results.Count;
                                    }
                                }
                                this.saveWatchlist_details_pendingoverride(remitter_dt, remitter_id, hitCount);
                            }
                            else
                            {
                                caseSystemId_Remitter_id = string.Empty;
                            }
                            if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY HIT Rows Count
                            {
                                if (beneficiary_dt.Rows.Count == 1 && (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty)) //NO BENEFICIARY HITS
                                {
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                                }
                                else
                                {
                                    //START WITH BENEFICIARY HIT
                                    caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                    if (caseSystemId_Beneficiary_id == string.Empty)
                                    {
                                        int hitCount = 0;
                                        if (beneficiary_refinitiv != null)
                                        {
                                            foreach (var item in beneficiary_refinitiv)
                                            {
                                                hitCount = item.responseBody.results.Count;
                                            }
                                        }
                                        this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCount);
                                    }
                                    else { caseSystemId_Beneficiary_id = string.Empty; }
                                    remitter_dt.Merge(beneficiary_dt);
                                    this.get_refinitiv_dt(remitter_dt);
                                    return remitter_dt.AsEnumerable().ToList();
                                }
                            }
                            else
                            {
                                //NO BENEFICIARY HIT Rows Count
                                dt = remitter_dt;
                                this.get_refinitiv_dt(dt);
                                return dt.AsEnumerable().ToList();
                            }
                        }
                    } //END WITH REMITTER ROW COUNT
                    else
                    {
                        if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY ROW COUNT
                        {
                            if (beneficiary_dt.Rows.Count == 1 && (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty)) //NO BENEFICIAIRY HITS
                            {
                                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv.");
                            }
                            else
                            {
                                caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                if (caseSystemId_Beneficiary_id == string.Empty)
                                {
                                    int hitCount = 0;
                                    if (beneficiary_refinitiv != null)
                                    {
                                        foreach (var item in beneficiary_refinitiv)
                                        {
                                            hitCount = item.responseBody.results.Count;
                                        }
                                    }
                                    this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCount);
                                }
                                else { caseSystemId_Beneficiary_id = string.Empty; }
                                //ds.Tables.AddRange(new DataTable[] { beneficiary_dt });
                                //dt = ds.Tables[0];
                                //remitter_dt.Merge(beneficiary_dt);
                                this.get_refinitiv_dt(beneficiary_dt);
                                return beneficiary_dt.AsEnumerable().ToList();
                            }
                        } //NO REMITTER HIT
                        else { System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("No data from refinitiv."); }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlistTransactionalReviewModule_DT", ex.Message + "|" + ex.StackTrace, " [RemitterCIFID:" + remitter_id + "][BeneficiaryCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null)
                {
                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                    dt.Dispose();
                    dt = null;
                }
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public List<DataRow> DisplayWatchlistTransactional_DT(string remitter_id, string beneficiary_id)
        {
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            DataTable dt = null;
            DataTable REFINITIV_DT = null;
            DataTable remitter_dt = null;
            DataTable beneficiary_dt = null;
            bool remitter_down = false;
            bool beneficiary_down = false;
            string caseSystemId_Remitter_id = string.Empty;
            string caseSystemId_Beneficiary_id = string.Empty;
            IEnumerable<irsRefinitiv.ScreeningResponseDto> remitter_refinitiv = null;
            IEnumerable<irsRefinitiv.ScreeningResponseDto> beneficiary_refinitiv = null;
            int hitCount = 0;
            int hitCountb = 0;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            bool isRefinitivIndividual_Remitter = false;
            bool isRefinitivIndividual_Beneficiary = false;

            List<string> Remittersecondary = new List<string>();
            DataTable dtCIFRemitter = irsrefinitiv.getCIFInfo(remitter_id);
            if (dtCIFRemitter.Rows[0]["CIFType"] != DBNull.Value && dtCIFRemitter.Rows[0]["CIFType"].ToString() != string.Empty)
            {
                if (dtCIFRemitter.Rows[0]["CIFType"].ToString().Trim() == "1")
                {
                    isRefinitivIndividual_Remitter = true;
                }
                else
                {
                    isRefinitivIndividual_Remitter = false;
                }
            }

            List<string> Beneficiarysecondary = new List<string>();
            DataTable dtCIFBenef = irsrefinitiv.getCIFInfo(beneficiary_id);
            if (dtCIFBenef.Rows[0]["CIFType"] != DBNull.Value && dtCIFBenef.Rows[0]["CIFType"].ToString() != string.Empty)
            {
                if (dtCIFBenef.Rows[0]["CIFType"].ToString().Trim() == "1")
                {
                    isRefinitivIndividual_Beneficiary = true;
                }
                else
                {
                    isRefinitivIndividual_Beneficiary = false;
                }
            }

            try
            {
                if (System.Web.HttpContext.Current.Session["REFINITIV_DT"] != null)
                {
                    REFINITIV_DT = (DataTable)System.Web.HttpContext.Current.Session["REFINITIV_DT"];
                }
                else
                {
                    new csAuditLog().writeAudit("", "REFINITIV", "DisplayWatchlistTransactional_DT.irsrefinitiv.DisplayWatchlist", "Remitter", " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                    remitter_dt = irsrefinitiv.DisplayWatchlist(branch_code, remitter_refinitiv, remitter_id, false, "", "", "", isRefinitivIndividual_Remitter, Remittersecondary, out remitter_down);
                    new csAuditLog().writeAudit("", "REFINITIV", "DisplayWatchlistTransactional_DT.irsrefinitiv.DisplayWatchlist", "Beneficiary", " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                    beneficiary_dt = irsrefinitiv.DisplayWatchlist(branch_code, beneficiary_refinitiv, beneficiary_id, false, "", "", "", isRefinitivIndividual_Beneficiary, Beneficiarysecondary, out beneficiary_down);
                }

                DataSet ds = new DataSet();

                if (remitter_down == true)
                {
                    throw new Exception("Refinitiv is down."); 
                }
                else if (beneficiary_down == true)
                {
                    throw new Exception("Refinitiv is down."); 
                }
                else
                {
                    if (REFINITIV_DT != null)
                    {
                        if (REFINITIV_DT.Rows.Count > 0)
                        {
                            if (REFINITIV_DT.Rows[0][2] == DBNull.Value || REFINITIV_DT.Rows[0][2].ToString() == string.Empty) //NO HITS
                            {
                                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("NO HIT");
                            }
                            else { return REFINITIV_DT.AsEnumerable().ToList(); }
                        }
                        else
                        {
                            System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("NO HIT");
                        }
                    }
                    else
                    {
                        if (remitter_dt != null && remitter_dt.Rows.Count > 0) //WITH REMITTER HIT Rows Count
                        {
                            if (remitter_dt.Rows[0][2] == DBNull.Value || remitter_dt.Rows[0][2].ToString() == string.Empty) //NO REMITTER HITS
                            {
                                if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY HIT Rows Count
                                {
                                    if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) //NO BENEFICIARY HITS
                                    {
                                        System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("NO HIT");
                                    }
                                    else
                                    {
                                        //WITH BENEFICIARY HITS ONLY
                                        caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                        if (caseSystemId_Beneficiary_id == string.Empty)
                                        {
                                            hitCountb = 0;
                                            if (beneficiary_refinitiv != null)
                                            {
                                                foreach (var item in beneficiary_refinitiv)
                                                {
                                                    hitCountb = item.responseBody.results.Count;
                                                }
                                            }
                                            this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCountb);
                                        }
                                        else { caseSystemId_Beneficiary_id = string.Empty; }
                                        this.get_refinitiv_dt(beneficiary_dt);
                                        return beneficiary_dt.AsEnumerable().ToList();
                                    }
                                }
                                else
                                {
                                    // NO HIT BOTH REMITTER AND BENEFICIARY
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("NO HIT");
                                }
                            }
                            else
                            {
                                //START WITH REMITTER HIT
                                caseSystemId_Remitter_id = this.getRefinitiv_casesystemid(remitter_id);
                                if (caseSystemId_Remitter_id == string.Empty)
                                {
                                    hitCount = 0;
                                    if (remitter_refinitiv != null)
                                    {
                                        foreach (var item in remitter_refinitiv)
                                        {
                                            hitCount = item.responseBody.results.Count;
                                        }
                                    }
                                    this.saveWatchlist_details_pendingoverride(remitter_dt, remitter_id, hitCount);
                                }
                                else
                                {
                                    caseSystemId_Remitter_id = string.Empty;
                                }
                                if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY HIT Rows Count
                                {
                                    if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) //NO BENEFICIARY HITS
                                    {
                                        //with remitter but no beneficiary
                                        dt = remitter_dt;
                                        this.get_refinitiv_dt(dt);
                                        return dt.AsEnumerable().ToList();
                                        //System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("NO HIT");
                                    }
                                    else
                                    {
                                        //START WITH BENEFICIARY HIT
                                        caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                        if (caseSystemId_Beneficiary_id == string.Empty)
                                        {
                                            hitCountb = 0;
                                            if (beneficiary_refinitiv != null)
                                            {
                                                foreach (var item in beneficiary_refinitiv)
                                                {
                                                    hitCountb = item.responseBody.results.Count;
                                                }
                                            }
                                            this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCountb);
                                        }
                                        else { caseSystemId_Beneficiary_id = string.Empty; }
                                        remitter_dt.Merge(beneficiary_dt);
                                        this.get_refinitiv_dt(remitter_dt);
                                        return remitter_dt.AsEnumerable().ToList();
                                    }
                                }
                                else
                                {
                                    //NO BENEFICIARY HIT Rows Count
                                    dt = remitter_dt;
                                    this.get_refinitiv_dt(dt);
                                    return dt.AsEnumerable().ToList();
                                }
                            }
                        } //END WITH REMITTER ROW COUNT
                        else
                        {
                            if (beneficiary_dt != null && beneficiary_dt.Rows.Count > 0) //WITH BENEFICIARY ROW COUNT
                            {
                                if (beneficiary_dt.Rows[0][2] == DBNull.Value || beneficiary_dt.Rows[0][2].ToString() == string.Empty) //NO BENEFICIAIRY HITS
                                {
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("NO HIT");
                                }
                                else
                                {
                                    caseSystemId_Beneficiary_id = this.getRefinitiv_casesystemid(beneficiary_id);
                                    if (caseSystemId_Beneficiary_id == string.Empty)
                                    {
                                        hitCountb = 0;
                                        if (beneficiary_refinitiv != null)
                                        {
                                            foreach (var item in beneficiary_refinitiv)
                                            {
                                                hitCountb = item.responseBody.results.Count;
                                            }
                                        }
                                        this.saveWatchlist_details_pendingoverride(beneficiary_dt, beneficiary_id, hitCountb);
                                    }
                                    else { caseSystemId_Beneficiary_id = string.Empty; }
                                    //ds.Tables.AddRange(new DataTable[] { beneficiary_dt });
                                    //dt = ds.Tables[0];
                                    //remitter_dt.Merge(beneficiary_dt);
                                    this.get_refinitiv_dt(beneficiary_dt);
                                    return beneficiary_dt.AsEnumerable().ToList();
                                }
                            } //NO REMITTER HIT
                            else { System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null; throw new Exception("NO HIT"); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.DisplayWatchlistTransactional_DT", ex.Message + "|" + ex.StackTrace, " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]", csAuditLog.Category.Refinitiv);
                //writeLog(ex.Message + " [RemitterCIFID:" + remitter_id + "][BeneficiairyCIFID: " + beneficiary_id + "]");
                throw new Exception(ex.Message);
            }
            finally
            {
                if (dt != null)
                {
                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        /* RFC-36314 - IRSW (EU) Interface with Refinitiv WorldCheck via API  */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_refinitiv_overridetype(int override_type)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            string user_role = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Role.ToString();

            try
            {
                ds_return = sqlkonek.execStoredProcedure("getRefinitivHit_NatureOfOverride", new object[] { override_type });
                if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
                {
                    return "1";
                }
                else { return "0"; }
            }
            catch
            {
                return "0";
            }
            finally
            {
                if (ds_return != null)
                {
                    ds_return.Dispose();
                    ds_return = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRefinitiv_transdetails_referenceno(string referenceNo)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("getRefinitiv_referenceno", new object[] { referenceNo });

            if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
            {
                return ds_return;
            }
            else { return ds_return; }
        }

        /* RFC-37544 - IRSW RCI Interface with Refinitiv WorldCheck via API */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void isRefinitivDown_onboarding(int i)
        {
            System.Web.HttpContext.Current.Session["isRefinitivDown_onboarding"] = i;
        }

        /* RFC-37544 - IRSW RCI Interface with Refinitiv WorldCheck via API */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void isRefinitivDown_transactional(int i)
        {
            System.Web.HttpContext.Current.Session["isRefinitivDown_transactional"] = i;
        }
        /* RFC-37544 - IRSW RCI Interface with Refinitiv WorldCheck via API */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRefinitiv_finaldispo(string final_dispo)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("getRefinitiv_finaldispo", new object[] { final_dispo });

            if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
            {
                return ds_return;
            }
            else { return ds_return; }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet checkIfCIFwithHIT(string CIFID)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            DataSet ds_return = null;
            sqlkonek.EnforceConstraint = false;

            ds_return = sqlkonek.execStoredProcedure("checkIfCIFwithHIT", new object[] { CIFID });

            try
            {
                if (ds_return != null && ds_return.Tables[0].Rows.Count > 0)
                {
                    return ds_return;
                }
                else {
                    new csAuditLog().writeAudit("", "REFINITIV", "TranClass.Refinitiv.checkIfCIFwithHIT", "Returns NO HIT ", "Details CIFID:" + CIFID, csAuditLog.Category.Refinitiv);
                    return ds_return;
                }
            }
            catch (Exception ex)
                {
                new csAuditLog().writeAudit("", "REFINITIV ERROR", "TranClass.Refinitiv.checkIfCIFwithHIT", "StackTrace:" + ex.StackTrace + "|Message:" + ex.Message, "Details CIFID:" + CIFID, csAuditLog.Category.Refinitiv);
                return ds_return;
            }
        }

    }
}