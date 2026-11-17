using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Configuration;

namespace IRSClassLib
{
    /// <summary>
    /// Contains public Methods for IRSWorld Report Profiler
    /// </summary>
    public class irsReportProfiler : sqlHelperWs
    {
        public DataTable GetStates()
        {
            base.EnforceConstraint = false;
            base.EnforceConstraint = false;
            getUpdateDS("usp_LoadStates", new Object[] { });
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
        public DataTable GetOFWReports()
        {
            base.EnforceConstraint = false;
            getUpdateDS("usp_LoadOFWRemittanceReport", new Object[] { });
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

        public DataTable GetAmtFilter()
        {
            DataSet DS = new DataSet();
            DataTable DT = DS.Tables.Add();

            DT.Columns.Add("Description", typeof(string));
            DT.Columns.Add("Value", typeof(string));

            DT.Rows.Add(">=", ">=");
            DT.Rows.Add("<=", "<=");
            DT.Rows.Add("=", "=");

            return DT;
        }

        public DataTable GetComputation()
        {
            DataSet DS = new DataSet();
            DataTable DT = DS.Tables.Add();

            DT.Columns.Add("Description", typeof(string));
            DT.Columns.Add("Value", typeof(string));

            DT.Rows.Add("GROSS", "GrossAmount");
            DT.Rows.Add("NET", "NetAmount");
            DT.Rows.Add("NET TO BENE", "NetToBenAmount");

            return DT;
        }

        public DataTable GetReportField()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getReportField", new Object[] { });
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

        public DataTable GetMerchantName()
        {
            base.EnforceConstraint = false;
            getUpdateDS("CIF_Merch_Code", new Object[] { });
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
        public DataTable GetProfileList()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getProfileList", new Object[] { });
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

        public DataTable GetRemitMethod()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getRemitMethod", new Object[] { });
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

        public DataTable GetIRSOverseas_Branches(string brchCode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getIRSOverseasBranches", new Object[] { brchCode });

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

        public DataTable GetIRSDomestic_Branches()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getIRSDomesticBranches", new Object[] { });
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

        public DataTable GetDateColumns()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getDateColumns", new Object[] { });
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

        public DataTable GetRemittingCCY()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getRemittingCurrency", new Object[] { });
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

        public DataTable GetTargetCCY()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getTargetCurrency", new Object[] { });
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

        public DataTable GetBatchUploadMethods()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getBatchUploadMethods", new Object[] { });
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

        public DataTable GetProductCode()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getProductCode", new Object[] { });
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

        public DataTable GetTranxSource()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getTranxSource", new Object[] { });
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

        public DataTable GetPayMode()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getPayMode", new Object[] { });
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

        public DataTable GetCriteria()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getCriteria", new Object[] { });
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

        public DataTable GetTranxStatus()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getTranxStatus", new Object[] { });
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

        public DataTable GetBranchRegion(string brchFilter)
        {
            base.EnforceConstraint = false;
            getUpdateDS("usp_GetBranchPerRegion", new Object[] { brchFilter });
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

        public DataTable LoadBracnhFilterPerRegion()
        {
            base.EnforceConstraint = false;
            getUpdateDS("usp_LoadBranchFilterPerRegion", new Object[] { });
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

        public string BuildSQLQuery(string[] RemBrCode, string[] CredBrCode,
                                    string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
                                    string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                                    string[] RemitMethod, string[] BatchTypes, string[] ProductCode,
                                    string[] TransactionSource, string[] PayMode, string[] TransactionStatus,
                                    string RemitterID, string BeneficiaryID, string AccountNo,
                                    string[] ReportField, bool CheckMainBr, bool CheckRemBr,
                                    string Criteria, string MemberId, string[] MerchName, bool chkMerchs,
                                    bool chkPCode, bool chkPMode, bool chkTSource, bool chkTStatus, string []States, bool chkStates )
        {
            string StrQuery;
            string Ibatch;
            string OtherChannels;
            string FieldsSelected;
            string StrRemBrCode;
            string PCode;
            string PMode;
            string TSource;
            string TStatus;
            string MerchCode;
            string IRSCompliance;
            string ARS;
            string EXCO;
            string IRS;
            string Excel;
            string SLExtractIRS;
            string EPSCO;
            string FEDACH;
            string RemStates;
            string StrCredBrCode;

            PCode = ArrayIn(ProductCode);
            PMode = ArrayIn(PayMode);
            TSource = ArrayIn(TransactionSource);
            TStatus = ArrayIn(TransactionStatus);
            MerchCode = ArrayIn(MerchName);
            RemStates = ArrayIn(States);
            StrRemBrCode = ArrayIn(RemBrCode);
            StrCredBrCode = ArrayIn(CredBrCode);
            Ibatch = IBATCH_Array(BatchTypes);
            OtherChannels = OtherChannels_Array(BatchTypes);
            IRSCompliance = GetfileTypes(BatchTypes, "IRSCompliance");
            ARS = GetfileTypes(BatchTypes, "ARS");
            EXCO = GetfileTypes(BatchTypes, "EXCO");
            IRS = GetfileTypes(BatchTypes, "IRS");
            Excel = GetfileTypes(BatchTypes, "Excel");
            SLExtractIRS = GetfileTypes(BatchTypes, "SLExtractIRS");
            EPSCO = GetfileTypes(BatchTypes, "EPSCO");
            FEDACH = GetfileTypes(BatchTypes, "FEDACH");
            FieldsSelected = FieldSelections(ReportField);

            StrQuery = "SELECT DISTINCT '', ";
            StrQuery = StrQuery + FieldsSelected;

            StrQuery = StrQuery + " FROM TBL_REMITTANCE_IRSW R";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_MASTER REMCIF ON R.RemitterID = REMCIF.CIFID AND ISNULL(REMCIF.DELETED,0) = 0";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_REMITTANCE_OTHER_DETAILS OD ON OD.Rem_Seq_ID = R.TXN_SEQ_ID";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_FILE_INVENTORY FI on FI.Seq_ID = OD.fileinvseqid";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_REMITTANCE_CHECK_DETAILS CD ON CD.Rem_Seq_ID = R.TXN_SEQ_ID";
            StrQuery = StrQuery + " LEFT OUTER JOIN tbl_CIF_Merchants merch on  R.RemitterID = merch.RemitterCIFID and R.beneficiaryID = merch.CIFID";
            if (StrQuery.IndexOf("idSSn.") > 0)
            {
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idSSn on r.RemitterID = idSSn.CIFID and idSSn.IDtype in (15) and isNull(idSSn.Deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idGOV on r.RemitterID = idGOV.CIFID and idGOV.IDtype in (1,28,32) and isNull(idGOV.Deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idPP on r.RemitterID = idPP.CIFID and idPP.IDtype in (22,23,29,34,45,11) and isNull(idPP.Deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idDL on r.RemitterID = idDL.CIFID and idDL.IDtype in (61,33,24,21) and isNull(idDL.Deleted,0) = 0 ";
            }
            if (StrQuery.IndexOf("RMBR.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_BRANCHES RMBR ON R.RemittingBranch = RMBR.BranchCode";
            }
            if (StrQuery.IndexOf("REMOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS REMOccupations ON REMOccupations.CIFID = R.RemitterID ";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP14 ON LUP14.FldValue = REMOccupations.Occupation AND LUP14.Fldgroup = 'Occupation' ";
            }
            if (StrQuery.IndexOf("LUP15.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS BENOccupations ON BENOccupations.CIFID = R.BeneficiaryID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP15 ON LUP15.FldValue = BENOccupations.Occupation AND LUP15.Fldgroup = 'Occupation' ";
            }
            if (StrQuery.IndexOf("merchInfo.") > 0 || StrQuery.IndexOf("merch.") > 0)
            {
                StrQuery = StrQuery + " Left Outer Join Tbl_CIF_Merchants_info merchInfo on merch.merInfoSeqId = merchInfo.seq_id and isnull(merchInfo.deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Outer Join Tbl_Lookup lkpMerch on merchInfo.MerchantProductCode = lkpMerch.FldValue and lkpMerch.fldGroup = 'MerchantProdCode' ";
            }
            if (StrQuery.IndexOf("rep.LastName") > 0)
            {
                StrQuery = StrQuery + " Left Outer Join TBL_Remittance_Representative rep on R.TXN_SEQ_ID = rep.TXN_SEQ_ID ";
            }
            if (StrQuery.IndexOf("ach.") > 0)
            {
                StrQuery = StrQuery + " Left join TBL_INTERNET_ACHWORKS ach on R.ReferenceNo = ach.ReferenceNo ";
            }
            if (RemStates != "" || StrQuery.IndexOf("") > 0)
            {
                StrQuery = StrQuery + " Left Join Tbl_Remittance remit on R.TXN_SEQ_ID = remit.SEQ_ID ";
                StrQuery = StrQuery + " Left Join Tbl_CIF_Address addr on remit.RemitterAddressSeqID = addr.SEQ_ID and IsNull(addr.Deleted,0) = 0 ";
            }
            if (StrQuery.IndexOf("[States]") > 0)
            {
                StrQuery = StrQuery + " Left Join Tbl_Lookup lkpSts on addr.Address5 = lkpSts.FldCode And lkpSts.FldGroup = 'States' ";
            }

            StrQuery = StrQuery + " Where R.vRemitMethod in (" + ArrayIn(RemitMethod) + ")";
            if (BatchTypes.Length != 0)
            {
                StrQuery = StrQuery + " And (";
                if (Ibatch != string.Empty)
                {
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (3)and IsNull( FI.FileFormat,'') in " + Ibatch + ")";
                }
                if (OtherChannels != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (2)and IsNull( FI.FileFormat,'') in " + OtherChannels + ")";
                }
                if (IRSCompliance != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (1)and IsNull( FI.FileFormat,'') in " + IRSCompliance + ")";
                }
                if (ARS != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (4)and IsNull( FI.FileFormat,'') in " + ARS + ")";
                }
                if (EXCO != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (5)and IsNull( FI.FileFormat,'') in " + EXCO + ")";
                }
                if (IRS != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (6)and IsNull( FI.FileFormat,'') in " + IRS + ")";
                }
                if (Excel != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (7)and IsNull( FI.FileFormat,'') in " + Excel + ")";
                }
                if (SLExtractIRS != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (8)and IsNull( FI.FileFormat,'') in " + SLExtractIRS + ")";
                }
                if (EPSCO != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (9)and IsNull( FI.FileFormat,'') in " + EPSCO + ")";
                }
                if (FEDACH != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (10)and IsNull( FI.FileFormat,'') in " + FEDACH + ")";
                }
                StrQuery = StrQuery + ")";
            }
            if (StrRemBrCode != string.Empty)
            {
                StrQuery = StrQuery + " And R.RemittingBranch IN (" + StrRemBrCode + ")";
            }
            if ((!CheckMainBr) && (StrCredBrCode != ""))
            {
                StrQuery = StrQuery + " And subString(r.AccountBranchCode,1,4) in (" + StrCredBrCode + ")";
            }
            StrQuery = StrQuery + " And Convert(varchar(10),R." + DateFilter + ",121) Between '" + Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd") + "'";
            StrQuery = StrQuery + " And R." + AmtComputation + " " + AmtFilter + " " + Amount;
            if (RemittingCCY != "")
            {
                StrQuery = StrQuery + " And R.vRemittingCurrency = " + RemittingCCY;
            }
            if (TargetCCY != "")
            {
                StrQuery = StrQuery + " And R.vNetToBenCurrency = " + TargetCCY;
            }
            if (!chkTStatus && TStatus != "")
            {
                StrQuery = StrQuery + " And R.vTranxStatus in (" + TStatus + ") ";
            }
            if (!chkTSource && TSource != "")
            {
                StrQuery = StrQuery + " And R.vTranxSource in (" + TSource + ")";
            }
            if (!chkPMode && PMode != "")
            {
                StrQuery = StrQuery + " And R.vPayMode in (" + PMode + ") ";
            }
            if (!chkPCode && PCode != "")
            {
                StrQuery = StrQuery + " And R.vProductCode in (" + PCode + ") ";
            }
            if (RemitterID != "")
            {
                StrQuery = StrQuery + " And R.RemitterID in ('" + RemitterID + "') ";
            }
            if (BeneficiaryID != "")
            {
                StrQuery = StrQuery + " And R.BeneficiaryID in ('" + BeneficiaryID + "') ";
            }
            if (AccountNo != "")
            {
                StrQuery = StrQuery + " And R.AccountNo in ('" + AccountNo + "') ";
            }
            if (Criteria != string.Empty)
            {
                StrQuery = StrQuery + " And LTrimg(RTrim(" + Criteria + ")) != '' ";
            }
            if (MemberId != string.Empty)
            {
                StrQuery = StrQuery + " And R.MerchantID = LTrimg(RTrim('" + MemberId + "')) ";
            }
            if (MerchCode != "")
            {
                StrQuery = StrQuery + " And R.BeneficiaryID in (" + MerchCode + ") ";
            }

            if (RemStates != "")
            {
                StrQuery = StrQuery + " And IsNull(LTrim(RTrim(addr.Address5)),'') in (" + RemStates + ") ";
            }

            if (StrQuery.IndexOf("Count(R.ReferenceNo) [Items]") > 0 || StrQuery.IndexOf("Sum(R.GrossAmount * R.ForexRemittingToLocal) [HKD Amount]") > 0)
            {
                StrQuery = StrQuery + " Group By R.MakerID, MakerIDName ";
            }
            return StrQuery;
        }

        public String DeleteProfile(string profilename)
        {
            base.EnforceConstraint = false;
            string sql;
            string pName = profilename.Trim();
            sql = "Delete From TBL_PROFILER where profileName = '" + pName + "'";

            if (CheckProfileName(profilename))
            {
                try
                {
                    int k = sqlUpdate(sql);
                    if (k < 0)
                    {
                        return "ERROR \n" + Message.ToString();
                    }
                }
                catch (SqlException ex)
                {
                    return "ERROR \n" + ex.Message;
                }
                return "Update Successful!";
            }
            else
            {
                return "Profile " + profilename + "Not Found!";
            }
        }

        private string ArrayIn(string[] StrArray)
        {
            string StrBuilder;

            StrBuilder = "";
            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {

                    StrBuilder = StrBuilder + "'";
                    StrBuilder = StrBuilder + StrArray[i].ToString();
                    StrBuilder = StrBuilder + "',";
                }
            }
            if (StrBuilder != "")
            {
                StrBuilder = StrBuilder.Substring(0, StrBuilder.Length - 1);
            }
            return StrBuilder;
        }

        private string FieldSelections(string[] StrArray)
        {
            string StrBuilder;

            StrBuilder = "";
            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {


                    StrBuilder = StrBuilder + StrArray[i].ToString();
                    StrBuilder = StrBuilder + ",";
                }
            }
            if (StrBuilder != "")
            {
                StrBuilder = StrBuilder.Substring(0, StrBuilder.Length - 1);
            }
            return StrBuilder;
        }

        public string GetserverDate()
        {
            string returnstr;
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("spz_getServerDate");
                base.deriveParameters(cmd, new Object[] { });
                ds = base.getDataSet(cmd);

                returnstr = "";
                returnstr = returnstr + ds.Tables[0].Rows[0]["ServerDate"].ToString();
                return returnstr;
            }
            catch
            {
                return "Select Date";
            }

            finally
            {
                ds = null;
            }
        }

        private string IBATCH_Array(string[] StrArray)
        {
            string SQLSTATEMENT;
            string returnstr;
            SQLSTATEMENT = "";

            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {
                    SQLSTATEMENT = SQLSTATEMENT + StrArray[i].ToString();
                    SQLSTATEMENT = SQLSTATEMENT + ",";
                }
            }
            if (SQLSTATEMENT != "")
            {
                SQLSTATEMENT = SQLSTATEMENT.Substring(0, SQLSTATEMENT.Length - 1);
            }
            else
            {
                SQLSTATEMENT = "''";
            }

            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("spz_SelectIBATCH");
                base.deriveParameters(cmd, new Object[] { SQLSTATEMENT });
                ds = base.getDataSet(cmd);

                returnstr = "(";
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    returnstr = returnstr + "'";
                    returnstr = returnstr + ds.Tables[0].Rows[i]["FldValue"].ToString();
                    returnstr = returnstr + "',";
                }
                if (returnstr != "(")
                {
                    returnstr = returnstr.Substring(0, returnstr.Length - 1);
                    returnstr = returnstr + ")";
                    return returnstr;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                ds = null;
            }

        }

        public bool CheckProfileName(string ProfileName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("spz_CheckProfileName");
                base.deriveParameters(cmd, new Object[] { ProfileName });
                ds = base.getDataSet(cmd);


                if (ds.Tables[0].Rows.Count > 0)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return true;
            }
            finally
            {
                ds = null;
            }
        }

        private string OtherChannels_Array(string[] StrArray)
        {
            string SQLSTATEMENT;
            string returnstr;
            SQLSTATEMENT = "";

            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {

                    SQLSTATEMENT = SQLSTATEMENT + StrArray[i].ToString();
                    SQLSTATEMENT = SQLSTATEMENT + ",";
                }
            }
            if (SQLSTATEMENT != "")
            {
                SQLSTATEMENT = SQLSTATEMENT.Substring(0, SQLSTATEMENT.Length - 1);
            }
            else
            {
                SQLSTATEMENT = "''";
            }

            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("spz_SelectOtherChannels");
                base.deriveParameters(cmd, new Object[] { SQLSTATEMENT });
                ds = base.getDataSet(cmd);

                returnstr = "(";

                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    returnstr = returnstr + "'";
                    returnstr = returnstr + ds.Tables[0].Rows[i]["FldValue"].ToString();
                    returnstr = returnstr + "',";
                }

                if (returnstr != "(")
                {
                    returnstr = returnstr.Substring(0, returnstr.Length - 1);
                    returnstr = returnstr + ")";
                    return returnstr;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                ds = null;
            }
        }
        private string GetfileTypes(string[] StrArray, string fileType)
        {
            string seqId;
            string returnstr;
            seqId = "";
            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {
                    seqId = seqId + StrArray[i].ToString();
                    seqId = seqId + ",";
                }
            }
            if (seqId != "")
            {
                seqId = seqId.Substring(0, seqId.Length - 1);
            }
            else
            {
                seqId = "''";
            }
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("getFileTypes");
                base.deriveParameters(cmd, new Object[] { seqId, fileType });
                ds = base.getDataSet(cmd);

                returnstr = "(";
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    returnstr = returnstr + "'";
                    returnstr = returnstr + ds.Tables[0].Rows[i]["FldValue"].ToString();
                    returnstr = returnstr + "',";
                }
                if (returnstr != "(")
                {
                    returnstr = returnstr.Substring(0, returnstr.Length - 1);
                    returnstr = returnstr + ")";
                    return returnstr;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                ds = null;
            }
        }

        public String UpdateProfile(string[] RemBrCode, string[] CredBrCode,
                                    string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
                                    string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                                    string[] RemitMethod, string[] BatchTypes, string[] ProductCode,
                                    string[] TransactionSource, string[] PayMode, string[] TransactionStatus,
                                    string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
                                    bool CheckMainBr, bool CheckRemBr, string profilename, string SQLSyntax,
                                    bool chkRemitMethod, bool chkBatchTypes, string Criteria, string MemberId,
                                    string[] MerchName, bool chkMerchs, bool chkProdCode, bool chkPayMode, bool chkTranStatus,
                                    bool chkTranSource, string userID, string Region, string [] States, bool chkStates)
        {
            string sqlUpdateProf = string.Empty;
            string SQL = SQLSyntax.Replace("'", "^");
            string StrRemitMethod = AddDelimeter(RemitMethod, "|");
            string StrBatchTypes = AddDelimeter(BatchTypes, "|");
            string StrRemBrCode = AddDelimeter(RemBrCode, "|");
            string StrCredBrCode = AddDelimeter(CredBrCode, "|");
            string StrReportField = AddDelimeter(ReportField, "|");
            string StrProdCode = AddDelimeter(ProductCode, "|");
            string StrPayMode = AddDelimeter(PayMode, "|");
            string StrTranxStatus = AddDelimeter(TransactionStatus, "|");
            string StrTranxSource = AddDelimeter(TransactionSource, "|");
            string StrMerchCode = AddDelimeter(MerchName, "|");
            string StrStates = AddDelimeter(States, "|");


            sqlUpdateProf = "Update Tbl_profiler ";
            sqlUpdateProf += "SET ";
            sqlUpdateProf += " SQLSyntax = '" + SQL + "',";
            sqlUpdateProf += " RemitMethod = '" + StrRemitMethod + "',";
            sqlUpdateProf += " ChkRemitMethod = '" + chkRemitMethod + "',";
            sqlUpdateProf += " BatchType = '" + StrBatchTypes + "',";
            sqlUpdateProf += " ChkBatchType = '" + chkBatchTypes + "',";
            sqlUpdateProf += " RemittingBranch = '" + StrRemBrCode + "',";
            sqlUpdateProf += " ChkRemittingBranch = '" + CheckRemBr + "',";
            sqlUpdateProf += " MaintainingBranch = '" + StrCredBrCode + "',";
            sqlUpdateProf += " ChkMaintainingBranch = '" + CheckMainBr + "',";
            sqlUpdateProf += " ProductCode = '" + StrProdCode + "',";
            sqlUpdateProf += " TransactionSource = '" + StrTranxSource + "',";
            sqlUpdateProf += " PayMode = '" + StrPayMode + "',";
            sqlUpdateProf += " TransactionStatus = '" + StrTranxStatus + "',";
            sqlUpdateProf += " RemitterID = '" + RemitterID + "',";
            sqlUpdateProf += " BeneficiaryID = '" + BeneficiaryID + "',";
            sqlUpdateProf += " AccountNo = '" + AccountNo + "',";
            sqlUpdateProf += " TargetCurrency = '" + TargetCCY + "',";
            sqlUpdateProf += " RemittingCurrency = '" + RemittingCCY + "',";
            sqlUpdateProf += " AmountComputation = '" + AmtComputation + "',";
            sqlUpdateProf += " AmountEquation = '" + AmtFilter + "',";
            sqlUpdateProf += " Amount = '" + Amount + "',";
            sqlUpdateProf += " DateFilter = '" + DateFilter + "',";
            sqlUpdateProf += " DateFrom = '" + DateFrom + "',";
            sqlUpdateProf += " DateTo = '" + DateTo + "',";
            sqlUpdateProf += " Fields = '" + StrReportField + "',";
            sqlUpdateProf += " FieldCriteria = '" + Criteria + "',";
            sqlUpdateProf += " MemberIDNo = '" + MemberId + "',";
            sqlUpdateProf += " MerchantID = '" + StrMerchCode + "',";
            sqlUpdateProf += " ChkProductCode = '" + chkProdCode + "',";
            sqlUpdateProf += " ChkPayMode = '" + chkPayMode + "',";
            sqlUpdateProf += " ChkTranxSource = '" + chkTranSource + "',";
            sqlUpdateProf += " ChkTranxStatus = '" + chkTranStatus + "',";
            sqlUpdateProf += " ChkMerchants = '" + chkMerchs + "',";
            sqlUpdateProf += " Region = '" + Region + "',";
            sqlUpdateProf += " States = '" + StrStates + "',";
            sqlUpdateProf += " ChkStates = '" + chkStates + "'";
            sqlUpdateProf += " Where ProfileName = '" + profilename + "'";

            try
            {
                SqlCommand cmd = getSqlCommand(sqlUpdateProf);
                int i = sqlUpdate(sqlUpdateProf);
                if (i < 0)
                {
                    return "ERROR!: \n" + Message.ToString();
                }
                else
                {
                    new csAuditLog().writeAudit(userID, "PROFILER", sqlUpdateProf, "", "", csAuditLog.Category.Update);
                    return profilename.Trim() + " Successfully updated";
                }
            }
            catch (SqlException ex)
            {
                return "Database Connection Error.\n" + ex;
            }
        }

        public string SaveReportProfile(string[] RemBrCode, string[] CredBrCode,
                                    string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
                                    string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                                    string[] RemitMethod, string[] BatchTypes, string[] ProductCode,
                                    string[] TransactionSource, string[] PayMode, string[] TransactionStatus,
                                    string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
                                    bool CheckMainBr, bool CheckRemBr, string profilename, string SQLSyntax,
                                    bool chkRemitMethod, bool chkBatchTypes, string Criteria, string MemberId,
                                    string[] MerchName, bool chkMerchs, bool chkProdCode, bool chkPayMode, bool chkTranStatus,
                                    bool chkTranSource, string userID, string Region, string[] States, bool chkStates)
        {
            string SQLSTATEMENT;
            SQLSTATEMENT = "";
            SQLSyntax = SQLSyntax.Replace("'", "^");
            string StrRemitMethod = AddDelimeter(RemitMethod, "|");
            string StrBatchTypes = AddDelimeter(BatchTypes, "|");
            string StrRemBrCode = AddDelimeter(RemBrCode, "|");
            string StrCredBrCode = AddDelimeter(CredBrCode, "|");
            string StrReportField = AddDelimeter(ReportField, "|");
            string StrProdCode = AddDelimeter(ProductCode, "|");
            string StrPayMode = AddDelimeter(PayMode, "|");
            string StrTranxStatus = AddDelimeter(TransactionStatus, "|");
            string StrTranxSource = AddDelimeter(TransactionSource, "|");
            string StrMerchCode = AddDelimeter(MerchName, "|");
            string StrStates = AddDelimeter(States, "|");

            SQLSTATEMENT = "INSERT INTO tbl_Profiler VALUES ('";
            SQLSTATEMENT += profilename.Trim() + "', '";
            SQLSTATEMENT += SQLSyntax + "',";
            SQLSTATEMENT += "getDate(), '";

            SQLSTATEMENT += StrRemitMethod + "',";
            if (chkRemitMethod)
            {
                SQLSTATEMENT += "1, '";
            }
            else
            {
                SQLSTATEMENT += "0, '";
            }

            SQLSTATEMENT += StrBatchTypes + "',";

            if (chkBatchTypes)
            {
                SQLSTATEMENT += "1, '";
            }
            else
            {
                SQLSTATEMENT += "0, '";
            }

            SQLSTATEMENT += StrRemBrCode + "',";
            if (CheckRemBr)
            {
                SQLSTATEMENT += "1, '";
            }
            else
            {
                SQLSTATEMENT += "0, '";
            }

            SQLSTATEMENT += StrCredBrCode + "',";
            if (CheckMainBr)
            {
                SQLSTATEMENT += "1, '";
            }
            else
            {
                SQLSTATEMENT += "0, '";
            }

            SQLSTATEMENT += StrProdCode + "', '";
            SQLSTATEMENT += StrTranxSource + "', '";
            SQLSTATEMENT += StrPayMode + "', '";
            SQLSTATEMENT += StrTranxStatus + "', '";
            SQLSTATEMENT += RemitterID + "', '";
            SQLSTATEMENT += BeneficiaryID + "', '";
            SQLSTATEMENT += AccountNo + "', '";
            SQLSTATEMENT += TargetCCY + "', '";
            SQLSTATEMENT += RemittingCCY + "', '";
            SQLSTATEMENT += AmtComputation + "', '";
            SQLSTATEMENT += AmtFilter + "', '";
            SQLSTATEMENT += Amount + "', '";
            SQLSTATEMENT += DateFilter + "', '";
            SQLSTATEMENT += DateFrom + "', '";
            SQLSTATEMENT += DateTo + "', '";
            SQLSTATEMENT += StrReportField + "', '";
            SQLSTATEMENT += Criteria + "', '";
            SQLSTATEMENT += MemberId + "', '";
            SQLSTATEMENT += StrMerchCode + "', ";
            if (chkProdCode)
            {
                SQLSTATEMENT += "1,";
            }
            else
            {
                SQLSTATEMENT += "0,";
            }
            if (chkPayMode)
            {
                SQLSTATEMENT += "1,";
            }
            else
            {
                SQLSTATEMENT += "0,";
            }
            if (chkTranSource)
            {
                SQLSTATEMENT += "1,";
            }
            else
            {
                SQLSTATEMENT += "0,";
            }
            if (chkTranStatus)
            {
                SQLSTATEMENT += "1,";
            }
            else
            {
                SQLSTATEMENT += "0,";
            }
            if (chkMerchs)
            {
                SQLSTATEMENT += "1,'";
            }
            else
            {
                SQLSTATEMENT += "0,'";
            }
            SQLSTATEMENT += Region + "','";
            SQLSTATEMENT += StrStates + "',";
            if (chkStates)
            {
                SQLSTATEMENT += "1";
            }
            else
            {
                SQLSTATEMENT += "0";
            }
            SQLSTATEMENT += ")";

            try
            {
                SqlCommand cmd = getSqlCommand(SQLSTATEMENT);
                int i = sqlUpdate(SQLSTATEMENT);
                if (i < 0)
                {
                    return "ERROR! \n" + Message.ToString();
                }
                else
                {
                    new csAuditLog().writeAudit(userID, "PROFILER", SQLSTATEMENT, "", "", csAuditLog.Category.Insert);
                    return profilename.Trim() + " successfully saved to report profile";
                }
            }
            catch (SqlException ex)
            {
                return "Database Connection Error. \n" + ex.Message;
            }
        }

        private string AddDelimeter(string[] StrArray, string Delimeter)
        {
            string NewString;
            NewString = "";

            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {
                    NewString = NewString + StrArray[i].ToString();
                    NewString = NewString + Delimeter;
                }
            }
            if (NewString != "")
            {
                NewString = NewString.Substring(0, NewString.Length - 1);
            }
            return NewString;
        }

        public DataTable GetProfileDetails(string ProfileName)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("spz_CheckProfileName");
                base.deriveParameters(cmd, new Object[] { ProfileName });
                ds = base.getDataSet(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
            }
        }

        public DataTable ParseQuery(string UserID, string[] RemBrCode, string[] CredBrCode,
                                    string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
                                    string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                                    string[] RemitMethod, string[] BatchTypes, string[] ProductCode,
                                    string[] TransactionSource, string[] PayMode, string[] TransactionStatus,
                                    string RemitterID, string BeneficiaryID, string AccountNo,
                                    string[] ReportField, bool CheckMainBr, bool CheckRemBr,
                                    string Criteria, string MemberId, string[] MerchName, bool chkMerch,
                                    bool chkPCode, bool chkPMode, bool chkTSource, bool chkTStatus, string[] States, bool chkStates)
        {
            string StrQuery;
            string Ibatch;
            string OtherChannels;
            string FieldsSelected;
            string StrRemBrCode;
            string PCode;
            string PMode;
            string TSource;
            string TStatus;
            string MerchCode;
            string IRSCompliance;
            string ARS;
            string EXCO;
            string IRS;
            string Excel;
            string SLExtractIRS;
            string EPSCO;
            string FEDACH;
            string RemStates;
            string StrCredBrCode;

            PCode = ArrayIn(ProductCode);
            PMode = ArrayIn(PayMode);
            TSource = ArrayIn(TransactionSource);
            TStatus = ArrayIn(TransactionStatus);
            MerchCode = ArrayIn(MerchName);
            RemStates = ArrayIn(States);
            StrRemBrCode = ArrayIn(RemBrCode);
            StrCredBrCode = ArrayIn(CredBrCode);
            Ibatch = IBATCH_Array(BatchTypes);
            OtherChannels = OtherChannels_Array(BatchTypes);
            IRSCompliance = GetfileTypes(BatchTypes, "IRSCompliance");
            ARS = GetfileTypes(BatchTypes, "ARS");
            EXCO = GetfileTypes(BatchTypes, "EXCO");
            IRS = GetfileTypes(BatchTypes, "IRS");
            Excel = GetfileTypes(BatchTypes, "Excel");
            SLExtractIRS = GetfileTypes(BatchTypes, "SLExtractIRS");
            EPSCO = GetfileTypes(BatchTypes, "EPSCO");
            FEDACH = GetfileTypes(BatchTypes, "FEDACH");
            FieldsSelected = FieldSelections(ReportField);

            StrQuery = "SELECT DISTINCT '', ";
            StrQuery = StrQuery + FieldsSelected;

            StrQuery = StrQuery + " FROM TBL_REMITTANCE_IRSW R";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_MASTER REMCIF ON R.RemitterID = REMCIF.CIFID AND ISNULL(REMCIF.DELETED,0) = 0";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_REMITTANCE_OTHER_DETAILS OD ON OD.Rem_Seq_ID = R.TXN_SEQ_ID";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_FILE_INVENTORY FI on FI.Seq_ID = OD.fileinvseqid";
            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_REMITTANCE_CHECK_DETAILS CD ON CD.Rem_Seq_ID = R.TXN_SEQ_ID";
            StrQuery = StrQuery + " LEFT OUTER JOIN tbl_CIF_Merchants merch on  R.RemitterID = merch.RemitterCIFID and R.beneficiaryID = merch.CIFID";
            if (StrQuery.IndexOf("idSSn.") > 0)
            {
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idSSn on r.RemitterID = idSSn.CIFID and idSSn.IDtype in (15) and isNull(idSSn.Deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idGOV on r.RemitterID = idGOV.CIFID and idGOV.IDtype in (1,28,32) and isNull(idGOV.Deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idPP on r.RemitterID = idPP.CIFID and idPP.IDtype in (22,23,29,34,45,11) and isNull(idPP.Deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Join tbl_CIF_IDS idDL on r.RemitterID = idDL.CIFID and idDL.IDtype in (61,33,24,21) and isNull(idDL.Deleted,0) = 0 ";
            }
            if (StrQuery.IndexOf("RMBR.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_BRANCHES RMBR ON R.RemittingBranch = RMBR.BranchCode";
            }
            if (StrQuery.IndexOf("REMOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS REMOccupations ON REMOccupations.CIFID = R.RemitterID ";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP14 ON LUP14.FldValue = REMOccupations.Occupation AND LUP14.Fldgroup = 'Occupation' ";
            }
            if (StrQuery.IndexOf("LUP15.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS BENOccupations ON BENOccupations.CIFID = R.BeneficiaryID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP15 ON LUP15.FldValue = BENOccupations.Occupation AND LUP15.Fldgroup = 'Occupation' ";
            }
            if (StrQuery.IndexOf("merchInfo.") > 0 || StrQuery.IndexOf("merch.") > 0)
            {
                StrQuery = StrQuery + " Left Outer Join Tbl_CIF_Merchants_info merchInfo on merch.merInfoSeqId = merchInfo.seq_id and isnull(merchInfo.deleted,0) = 0 ";
                StrQuery = StrQuery + " Left Outer Join Tbl_Lookup lkpMerch on merchInfo.MerchantProductCode = lkpMerch.FldValue and lkpMerch.fldGroup = 'MerchantProdCode' ";
            }
            if (StrQuery.IndexOf("rep.LastName") > 0)
            {
                StrQuery = StrQuery + " Left Outer Join TBL_Remittance_Representative rep on R.TXN_SEQ_ID = rep.TXN_SEQ_ID ";
            }
            if (StrQuery.IndexOf("ach.") > 0)
            {
                StrQuery = StrQuery + " Left join TBL_INTERNET_ACHWORKS ach on R.ReferenceNo = ach.ReferenceNo ";
            }
            if (RemStates != "" || StrQuery.IndexOf("") > 0)
            {
                StrQuery = StrQuery + " Left Join Tbl_Remittance remit on R.TXN_SEQ_ID = remit.SEQ_ID ";
                StrQuery = StrQuery + " Left Join Tbl_CIF_Address addr on remit.RemitterAddressSeqID = addr.SEQ_ID and IsNull(addr.Deleted,0) = 0 " ;
            }
            if (StrQuery.IndexOf("[States]") > 0)
            {
                StrQuery = StrQuery + " Left Join Tbl_Lookup lkpSts on addr.Address5 = lkpSts.FldCode And lkpSts.FldGroup = 'States' ";
            }

            StrQuery = StrQuery + " Where R.vRemitMethod in (" + ArrayIn(RemitMethod) + ")";
            if (BatchTypes.Length != 0)
            {
                StrQuery = StrQuery + " And (";
                if (Ibatch != string.Empty)
                {
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (3)and IsNull( FI.FileFormat,'') in " + Ibatch + ")";
                }
                if (OtherChannels != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (2)and IsNull( FI.FileFormat,'') in " + OtherChannels + ")";
                }
                if (IRSCompliance != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (1)and IsNull( FI.FileFormat,'') in " + IRSCompliance + ")";
                }
                if (ARS != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (4)and IsNull( FI.FileFormat,'') in " + ARS + ")";
                }
                if (EXCO != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (5)and IsNull( FI.FileFormat,'') in " + EXCO + ")";
                }
                if (IRS != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (6)and IsNull( FI.FileFormat,'') in " + IRS + ")";
                }
                if (Excel != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (7)and IsNull( FI.FileFormat,'') in " + Excel + ")";
                }
                if (SLExtractIRS != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (8)and IsNull( FI.FileFormat,'') in " + SLExtractIRS + ")";
                }
                if (EPSCO != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (9)and IsNull( FI.FileFormat,'') in " + EPSCO + ")";
                }
                if (FEDACH != string.Empty)
                {
                    if (StrQuery.IndexOf("FI.FileType") > 0)
                    {
                        StrQuery = StrQuery + " OR";
                    }
                    StrQuery = StrQuery + " (IsNull( FI.FileType,'') in (10)and IsNull( FI.FileFormat,'') in " + FEDACH + ")";
                }
                StrQuery = StrQuery + ")";
            }
            if (StrRemBrCode != string.Empty)
            {
                StrQuery = StrQuery + " And R.RemittingBranch IN (" + StrRemBrCode + ")";
            }
            if ((!CheckMainBr) && (StrCredBrCode != ""))
            {
                StrQuery = StrQuery + " And subString(r.AccountBranchCode,1,4) in (" + StrCredBrCode + ")";
            }
            StrQuery = StrQuery + " And Convert(varchar(10),R." + DateFilter + ",121) Between '" + Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd") + "'";
            StrQuery = StrQuery + " And R." + AmtComputation + " " + AmtFilter + " " + Amount;
            if (RemittingCCY != "")
            {
                StrQuery = StrQuery + " And R.vRemittingCurrency = " + RemittingCCY;
            }
            if (TargetCCY != "")
            {
                StrQuery = StrQuery + " And R.vNetToBenCurrency = " + TargetCCY;
            }
            if (!chkTStatus && TStatus != "")
            {
                StrQuery = StrQuery + " And R.vTranxStatus in (" + TStatus + ") ";
            }
            if (!chkTSource && TSource != "")
            {
                StrQuery = StrQuery + " And R.vTranxSource in (" + TSource + ")";
            }
            if (!chkPMode && PMode != "")
            {
                StrQuery = StrQuery + " And R.vPayMode in (" + PMode + ") ";
            }
            if (!chkPCode && PCode != "")
            {
                StrQuery = StrQuery + " And R.vProductCode in (" + PCode + ") ";
            }
            if (RemitterID != "")
            {
                StrQuery = StrQuery + " And R.RemitterID in ('" + RemitterID + "') ";
            }
            if (BeneficiaryID != "")
            {
                StrQuery = StrQuery + " And R.BeneficiaryID in ('" + BeneficiaryID + "') ";
            }
            if (AccountNo != "")
            {
                StrQuery = StrQuery + " And R.AccountNo in ('" + AccountNo + "') ";
            }
            if (Criteria != string.Empty)
            {
                StrQuery = StrQuery + " And LTrimg(RTrim(" + Criteria + ")) != '' ";
            }
            if (MemberId != string.Empty)
            {
                StrQuery = StrQuery + " And R.MerchantID = LTrimg(RTrim('" + MemberId + "')) ";
            }
            if (MerchCode != "")
            {
                StrQuery = StrQuery + " And R.BeneficiaryID in (" + MerchCode + ") ";
            }
            if (RemStates != "")
            {
                StrQuery = StrQuery + " And IsNull(LTrim(RTrim(addr.Address5)),'') in (" + RemStates + ") ";
            }
            if (StrQuery.IndexOf("Count(R.ReferenceNo) [Items]") > 0 || StrQuery.IndexOf("Sum(R.GrossAmount * R.ForexRemittingToLocal) [HKD Amount]") > 0)
            {
                StrQuery = StrQuery + " Group By R.MakerID, MakerIDName ";
            }

            new csAuditLog().writeAudit(UserID, "PROFILER", StrQuery, "", "", csAuditLog.Category.Select);

            base.EnforceConstraint = false;
            getUpdateDS(StrQuery);
            selectedRow = 0;
            System.Web.HttpContext.Current.Session["tblData"] = dataset;

            return table;
        }

        public string ExtractReporttoFile(string profilename)
        {
            base.EnforceConstraint = false;
            DataSet ds = (DataSet)System.Web.HttpContext.Current.Session["tblData"];

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 65000) //65536 maximum for excel
                {
                    return "Result exceeds 65000 records Please check filter.";
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        csFileIo io = new csFileIo();
                        string Filename = "";
                        Filename = profilename + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                        string ReportProfilerDir = this.GetAppSetting("ReportProfilerDir");
                        string ReportPath = ReportProfilerDir + @"\" + Filename;

                        if (io.directoryExist(ReportProfilerDir))
                        {
                            CreateExcelXML(ReportPath, ds, profilename, 65000);
                        }
                        else
                        {
                            io.createDirectory(ReportProfilerDir);
                            CreateExcelXML(ReportPath, ds, profilename, 65000);
                        }
                        return "Extract Complete: " + ReportPath;
                    }
                    else
                    {
                        return "There is nothing to extract.";
                    }
                }
            }
            else
            {
                return "There is nothing to extract.";
            }
        }

        public static void CreateExcelXML(string FileName, DataSet Ds, string SheetName, int RecordsPerSheet)
        {
            StringWriter S = new StringWriter();
            XmlTextWriter X = new XmlTextWriter(S);
            StreamWriter Writer = new StreamWriter(FileName, false);

            try
            {
                SheetName = (SheetName.Trim().Length == 0) ? "WorkSheet" : SheetName;
                RecordsPerSheet = (RecordsPerSheet <= 0) ? 650000 : RecordsPerSheet;

                X.Formatting = Formatting.Indented;
                X.Indentation = 4;
                X.WriteProcessingInstruction("xml", "version=\"1.0\"");
                X.WriteProcessingInstruction("mso-application", "progid=\"Excel.Sheet\"");
                X.WriteStartElement("Workbook", string.Empty);
                X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:spreadsheet");
                X.WriteAttributeString("xmlns:o", "urn:schemas-microsoft-com:office:office");
                X.WriteAttributeString("xmlns:x", "urn:schemas-microsoft-com:office:excel");
                X.WriteAttributeString("xmlns:ss", "urn:schemas-microsoft-com:office:spreadsheet");
                X.WriteAttributeString("xmlns:html", "http://www.w3.org/TR/REC-html40");

                X.WriteStartElement("DocumentProperties", string.Empty);
                X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");
                X.WriteElementString("Author", "Don Sandee T. Reyes");
                X.WriteElementString("LastAuthor", string.Empty);
                X.WriteElementString("Created", string.Empty);
                X.WriteElementString("Company", string.Empty);
                X.WriteElementString("Version", "1.0");
                X.WriteEndElement();

                X.WriteStartElement("ExcelWorkBook", string.Empty);
                X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:office");
                X.WriteEndElement();

                X.WriteStartElement("Styles", string.Empty);
                X.WriteStartElement("Style", string.Empty);
                X.WriteAttributeString("ss:ID", "Default");
                X.WriteAttributeString("ss:Name", "Normal");
                X.WriteStartElement("Alignment", string.Empty);
                X.WriteAttributeString("ss:Vertical", "Bottom");
                X.WriteEndElement();
                X.WriteElementString("Borders", string.Empty);
                X.WriteElementString("Font", string.Empty);
                X.WriteElementString("Interior", string.Empty);
                X.WriteElementString("NumberFormat", string.Empty);
                X.WriteElementString("Protection", string.Empty);
                X.WriteEndElement();
                X.WriteEndElement();

                for (int i = 0; i < Ds.Tables.Count; i++)
                {
                    int k = 0;
                    for (int j = 0; j < Ds.Tables[i].Rows.Count; j++)
                    {
                        string Name = string.Empty;
                        if (j == 0)
                        {
                            Name = SheetName + " " + Convert.ToString(i + 1) + "." + k.ToString();
                            WriteWorkSheetStart(X, Ds, i, Name);
                            k++;
                        }
                        else if ((j % RecordsPerSheet) == 0)
                        {
                            Name = SheetName + " " + Convert.ToString(i + 1) + "." + k.ToString();
                            WriteWorkSheetEnd(X);
                            WriteWorkSheetStart(X, Ds, i, Name);
                            k++;
                        }
                        X.WriteStartElement("Row");
                        for (int x = 0; x < Ds.Tables[i].Columns.Count; x++)
                        {
                            X.WriteStartElement("Cell");
                            X.WriteStartElement("Data");
                            X.WriteAttributeString("ss:Type", "String");
                            X.WriteValue(Ds.Tables[i].Rows[j][x].ToString());
                            X.WriteEndElement();
                            X.WriteEndElement();
                        }
                        X.WriteEndElement();
                    }
                    WriteWorkSheetEnd(X);
                }
                X.WriteEndElement();
                X.Flush();
                Writer.Write(S.ToString());
            }
            finally
            {
                Writer.Close();
                S.Close();
                X.Close();
                S.Dispose();
                Writer.Dispose();
            }
        }

        private static void WriteWorkSheetStart(XmlTextWriter X, DataSet Ds, int i, string SheetName)
        {
            X.WriteStartElement("Worksheet", string.Empty);
            X.WriteAttributeString("ss:Name", SheetName);
            X.WriteStartElement("Table", string.Empty);
            X.WriteAttributeString("ss:ExpandedColumnCount", Ds.Tables[i].Columns.Count.ToString());
            X.WriteAttributeString("ss:ExpandedRowCount", Convert.ToString(Ds.Tables[i].Rows.Count + 1));
            X.WriteAttributeString("x:FullColumns", "1");
            X.WriteAttributeString("x:FullRows", "1");
            X.WriteStartElement("Row");
            for (int j = 0; j < Ds.Tables[i].Columns.Count; j++)
            {
                X.WriteStartElement("Cell");
                X.WriteStartElement("Data");
                X.WriteAttributeString("ss:Type", "String");
                X.WriteValue(Ds.Tables[i].Columns[j].ColumnName);
                X.WriteEndElement();
                X.WriteEndElement();
            }
            X.WriteEndElement();
        }

        private static void WriteWorkSheetEnd(XmlTextWriter X)
        {
            X.WriteEndElement();
            X.WriteStartElement("WorksheetOptions", string.Empty);
            X.WriteAttributeString("xmlns", "urn:schemas-microsoft-com:office:excel");
            X.WriteStartElement("Panes", string.Empty);
            X.WriteStartElement("Pane", string.Empty);
            X.WriteElementString("ActiveRow", "2");
            X.WriteElementString("ActiveCol", "1");
            X.WriteEndElement();
            X.WriteEndElement();
            X.WriteElementString("ProtectObjects", "False");
            X.WriteElementString("ProtectScenarios", "False");
            X.WriteEndElement();
            X.WriteEndElement();
        }

        private string GetAppSetting(string Key)
        {
            try
            {
                string sTemp = null;
                sTemp = new irsConfiguration().appsetting(Key);
                if (sTemp != null)
                    return sTemp;
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}