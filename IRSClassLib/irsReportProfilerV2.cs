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
    public class irsReportProfilerV2 : sqlHelperWs
    {
        public DataTable getAmtFilter()
        {
            //-- Instantiate the data set and table
            DataSet DS = new DataSet();
            DataTable DT = DS.Tables.Add();

            //-- Add columns to the data table
            DT.Columns.Add("Description", typeof(string));
            DT.Columns.Add("Value", typeof(string));

            //-- Add rows to the data table
            DT.Rows.Add( ">=", ">=");
            DT.Rows.Add( "<=", "<=");
            DT.Rows.Add( "=", "=");

            return DT;

        }

        public DataTable getComputation()
        {
            //-- Instantiate the data set and table
            DataSet DS = new DataSet();
            DataTable DT = DS.Tables.Add();

            //-- Add columns to the data table
            DT.Columns.Add("Description", typeof(string));
            DT.Columns.Add("Value", typeof(string));

            //-- Add rows to the data table
            DT.Rows.Add("GROSS", "GrossAmount");
            DT.Rows.Add("NET", "NetAmount");
            DT.Rows.Add("NET TO BENE", "NetToBenAmount");

            return DT;

        }

        public DataTable getReportField()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getReportField2", new Object[] { });
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

        public DataTable getProfileList()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getProfileList2", new Object[] { });
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


        public DataTable getRemitMethod()
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



        public DataTable getIRSOverseas_Branches(string branchcode)
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getIRSOverseasBranches", new Object[] {branchcode });
            
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

        public DataTable getIRSDomestic_Branches()
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

        
        public DataTable getDateColumns()
        {
            base.EnforceConstraint = false;
            getUpdateDS("spz_getDateColumns2", new Object[] { });
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

        public DataTable getRemittingCCY()
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

        public DataTable getTargetCCY()
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

        public DataTable getBatchUploadMethods()
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

        public DataTable getProductCode()
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

        public DataTable getTranxSource()
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

        public DataTable getPayMode()
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

        public DataTable getTranxStatus()
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


        

        public string BuildSQLQuery(string[] RemBrCode, string[]CredBrCode, 
                                    string DateFilter, string DateFrom, string DateTo,string RemittingCCY,
                                    string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                                    string[] RemitMethod, string[] BatchTypes, string ProductCode,
                                    string TransactionSource, string PayMode, string TransactionStatus,
                                    string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField, bool CheckMainBr, bool CheckRemBr)
        {
            string StrQuery;
            string Ibatch; 
            string otherChannels;
            string FieldsSelected;
            string StrRemBrCode;
            StrRemBrCode = ArrayIn(RemBrCode);
            string StrCredBrCode;
            StrCredBrCode = ArrayIn(CredBrCode);
            //Ibatch
            Ibatch = IBATCH_Array(BatchTypes);
            //OtherChannels
            otherChannels = OtherChannels_Array(BatchTypes);
            FieldsSelected = FieldSelections(ReportField);

            StrQuery = "SELECT DISTINCT ";
            //Create Selection Here
            StrQuery = StrQuery + FieldsSelected;

            StrQuery = StrQuery + " FROM TBL_REMITTANCE_IRSW R";

            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_FILE_INVENTORY FI on FI.Seq_ID = R.FileSEQID";
            
            if (StrQuery.IndexOf("REMOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS REMOccupations ON REMOccupations.CIFID = R.RemitterID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP14 ON LUP14.FldValue = REMOccupations.Occupation AND LUP14.Fldgroup = 'Occupation'";
               
            }

            if (StrQuery.IndexOf("BENOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS BENOccupations ON BENOccupations.CIFID = R.BeneficiaryID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP15 ON LUP15.FldValue = BENOccupations.Occupation AND LUP15.Fldgroup = 'Occupation'";
            }


            //Remit Method
            StrQuery = StrQuery + " WHERE R.VRemitMethod IN (" + ArrayIn(RemitMethod) + ")";
            //StrQuery = StrQuery + "AND ";

            //IBATCH
            StrQuery = StrQuery + " AND ((ISNULL( FI.FileType,'') in ('',3)and ISNULL( FI.FileFormat,'') in " + Ibatch + ")";

            //OtherChannels
            StrQuery = StrQuery + " OR (ISNULL( FI.FileType,'') in ('',2)and ISNULL( FI.FileFormat,'') in " + otherChannels + "))";

            //RemittingBranch
            if ((!CheckRemBr) && (StrRemBrCode != ""))
            {
                StrQuery = StrQuery + " AND ISNULL(R.RemittingBranch,'') IN (" + StrRemBrCode + ")";
            }

            //Reserve for Maintaining Branch
            if ((!CheckMainBr) && (StrCredBrCode != ""))
            {
                //StrQuery = StrQuery + " AND substring(R.AccountBranchCode,0,charindex('-',isnull(R.AccountBranchCode,''))) IN (" + StrCredBrCode + ")";
                StrQuery = StrQuery + " AND (substring(R.AccountBranchCode,0,charindex('-',isnull(R.AccountBranchCode,''))) IN (" + StrCredBrCode + ")";
                StrQuery = StrQuery + " OR substring(R.OtherBankBranchCode,0,charindex('-',isnull(R.OtherBankBranchCode,''))) IN (" + StrCredBrCode + "))";

            }   

            //Date
            StrQuery = StrQuery + " AND Convert(varchar(10),R." + DateFilter + ",121) BETWEEN '" + Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd") + "'";

            //Amount Filter
            StrQuery = StrQuery + " AND R." + AmtComputation + " " + AmtFilter + " " + Amount;

            //Remitting Currency
            if (RemittingCCY != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.VRemittingCurrency,'') = " + RemittingCCY;
            }

            // Target Currency
            if (TargetCCY != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.VNetToBenCurrency,'') = " + TargetCCY;
            }

            // TransactionStatus
            if (TransactionStatus != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.vTranxStatus,'') in (" + TransactionStatus + ")";
            }

            //TransactionSource
            if (TransactionSource != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.vTranxSource,'') in (" + TransactionSource + ")";
            }

            //PayMode
            if (PayMode != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.vPayMode,'') in (" + PayMode + ")";
            }

            //ProductCode
            if (ProductCode != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.vProductCode,'') in (" + ProductCode + ")";
            }

            //RemitterID
            if (RemitterID != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.RemitterID,'') in ('" + RemitterID + "')";
            }

            //BeneficiaryID
            if (BeneficiaryID != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.BeneficiaryID,'') in ('" + BeneficiaryID + "')";
            }

            //AccountNo
            if (AccountNo != "")
            {
                StrQuery = StrQuery + " AND ISNULL(R.AccountNo,'') in ('" + AccountNo + "')";
            }

                                              
            return StrQuery;
        
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

        public string getserverDate()
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
            //SQLSTATEMENT = "Select * from tbl_lookup where fldgroup = 'IBatch' AND SEQ_ID in (''";

            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {
                    //SQLSTATEMENT = SQLSTATEMENT + ",'";
                    SQLSTATEMENT = SQLSTATEMENT + StrArray[i].ToString();
                    //SQLSTATEMENT = SQLSTATEMENT + "'";
                    SQLSTATEMENT = SQLSTATEMENT + ",";
                }
            }

            //SQLSTATEMENT = SQLSTATEMENT + ")";

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
               
                returnstr = "('',";
                             
                for(int i = 0 ;i <= ds.Tables[0].Rows.Count - 1;i++)

                {
                     returnstr = returnstr + "'";
                     returnstr = returnstr + ds.Tables[0].Rows[i]["FldValue"].ToString();
                    returnstr = returnstr + "',";
                }

                if (returnstr != "('',")
                {
                    returnstr = returnstr.Substring(0, returnstr.Length - 1);
                    returnstr = returnstr + ")";
                    return returnstr;
                }
                else
                {
                    return "('')";
                }
            }
            catch 
            {
                return "('')";
            }
            finally
            {
                ds = null;
            }
        
        }

        public bool CheckProfileName( string ProfileName)
        {
            
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("spz_CheckProfileName2");
                base.deriveParameters(cmd, new Object[] { ProfileName });
                ds = base.getDataSet(cmd);

              
                if (ds.Tables[0].Rows.Count >0)
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
            //SQLSTATEMENT = "Select * from tbl_lookup where fldgroup = 'otherchannels' AND SEQ_ID in (''";
      
            for (int i = 0; i <= StrArray.Length - 1; i++)
            {
                if (StrArray != null)
                {

                    //SQLSTATEMENT = SQLSTATEMENT + ",'";
                    SQLSTATEMENT = SQLSTATEMENT + StrArray[i].ToString();
                    //SQLSTATEMENT = SQLSTATEMENT + "'";
                    SQLSTATEMENT = SQLSTATEMENT + ",";
                }
            }

            //SQLSTATEMENT = SQLSTATEMENT + ")";

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

                returnstr = "('',";

                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    returnstr = returnstr + "'";
                    returnstr = returnstr + ds.Tables[0].Rows[i]["FldValue"].ToString();
                    returnstr = returnstr + "',";
                }

                if (returnstr != "('',")
                {
                    returnstr = returnstr.Substring(0, returnstr.Length - 1);
                    returnstr = returnstr + ")";
                    return returnstr;
                }
                else
                {
                    return "('')";
                }
            }
            catch
            {
                return "('')";
            }
            finally
            {
                ds = null;
            }

        }

        public string SaveReportProfile(string[] RemBrCode, string[] CredBrCode,
                                    string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
                                    string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                                    string[] RemitMethod, string[] BatchTypes, string ProductCode,
                                    string TransactionSource, string PayMode, string TransactionStatus,
                                    string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField,
                                    bool CheckMainBr, bool CheckRemBr, string profilename, string SQLSyntax, 
                                    bool chkRemitMethod, bool chkBatchTypes) 
        
        {
            string SQLSTATEMENT;
            SQLSTATEMENT = "";
            SQLSyntax = SQLSyntax.Replace("'", "^");
            string StrRemitMethod = AddDelimeter(RemitMethod, "|");
            string StrBatchTypes = AddDelimeter(BatchTypes, "|");
            string StrRemBrCode = AddDelimeter(RemBrCode, "|");
            string StrCredBrCode = AddDelimeter(CredBrCode, "|");
            string StrReportField = AddDelimeter(ReportField, "|");

            SQLSTATEMENT = "INSERT INTO tbl_Profiler2 VALUES ('";
            SQLSTATEMENT += profilename.Trim() + "', '";
            SQLSTATEMENT += SQLSyntax + "',";
            SQLSTATEMENT += "getDate(), '";
            
            SQLSTATEMENT += StrRemitMethod + "',";
                if (chkRemitMethod)
                {
                SQLSTATEMENT +=  "1, '";
                }
                else
                {
                SQLSTATEMENT +=  "0, '";
                }
            
            SQLSTATEMENT += StrBatchTypes + "',";
            
                if (chkBatchTypes)
                {
                SQLSTATEMENT +=  "1, '";
                }
                else
                {
                SQLSTATEMENT +=  "0, '";
                }

 	  	    SQLSTATEMENT += StrRemBrCode + "',";
                if (CheckRemBr)
                {
                SQLSTATEMENT +=  "1, '";
                }
                else
                {
                SQLSTATEMENT +=  "0, '";
                }

            SQLSTATEMENT += StrCredBrCode + "',";
                if (CheckMainBr)
                {
                SQLSTATEMENT +=  "1, '";
                }
                else
                {
                SQLSTATEMENT +=  "0, '";
                }

            SQLSTATEMENT += ProductCode + "', '";
            SQLSTATEMENT += TransactionSource + "', '";
            SQLSTATEMENT += PayMode + "', '";
            SQLSTATEMENT += TransactionStatus + "', '";
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
            SQLSTATEMENT += StrReportField;
            SQLSTATEMENT += "')";

            try
            {
                SqlCommand cmd = getSqlCommand(SQLSTATEMENT);
                int i = sqlUpdate(SQLSTATEMENT);
                return  profilename.Trim() + " successfully saved to report profile";
            }
            catch
            {
                return "Database Connection Error.";
            }
                
                

        }


        private string AddDelimeter(string[] StrArray,string Delimeter)
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

        public DataTable getProfileDetails(string ProfileName)
        {

            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("spz_CheckProfileName2");
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

        public DataTable ParseQuery(string[] RemBrCode, string[] CredBrCode,
                                    string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
                                    string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                                    string[] RemitMethod, string[] BatchTypes, string ProductCode,
                                    string TransactionSource, string PayMode, string TransactionStatus,
                                    string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField, bool CheckMainBr, bool CheckRemBr)
        {
            string StrQuery;
            string Ibatch;
            string otherChannels;
            string FieldsSelected;
            string StrRemBrCode;
            StrRemBrCode = ArrayIn(RemBrCode);
            string StrCredBrCode;
            StrCredBrCode = ArrayIn(CredBrCode);
            //Ibatch
            Ibatch = IBATCH_Array(BatchTypes);
            //OtherChannels
            otherChannels = OtherChannels_Array(BatchTypes);
            FieldsSelected = FieldSelections(ReportField);

            StrQuery = "SELECT DISTINCT R.ReferenceNo, ";
            //Create Selection Here
            StrQuery = StrQuery + FieldsSelected;

            StrQuery = StrQuery + " FROM TBL_REMITTANCE_IRSW R";

            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_FILE_INVENTORY FI on FI.Seq_ID = R.FileSEQID";

            if (StrQuery.IndexOf("REMOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS REMOccupations ON REMOccupations.CIFID = R.RemitterID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP14 ON LUP14.FldValue = REMOccupations.Occupation AND LUP14.Fldgroup = 'Occupation'";

            }

            if (StrQuery.IndexOf("BENOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS BENOccupations ON BENOccupations.CIFID = R.BeneficiaryID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP15 ON LUP15.FldValue = BENOccupations.Occupation AND LUP15.Fldgroup = 'Occupation'";
            }


            //Remit Method
            StrQuery = StrQuery + " WHERE R.VRemitMethod IN (" + ArrayIn(RemitMethod) + ")";

            //IBATCH
            StrQuery = StrQuery + " AND ((ISNULL( FI.FileType,'') in ('',3)and ISNULL( FI.FileFormat,'') in " + Ibatch + ")";

            //OtherChannels
            StrQuery = StrQuery + " OR (ISNULL( FI.FileType,'') in ('',2)and ISNULL( FI.FileFormat,'') in " + otherChannels + "))";

            //RemittingBranch
            if ((!CheckRemBr) && (StrRemBrCode != ""))
            {
                StrQuery = StrQuery + " AND R.RemittingBranch IN (" + StrRemBrCode + ")";
            }

            //Reserve for Maintaining Branch
            if ((!CheckMainBr) && (StrCredBrCode != ""))
            {
                //StrQuery = StrQuery + " AND substring(R.AccountBranchCode,0,charindex('-',isnull(R.AccountBranchCode,''))) IN (" + StrCredBrCode + ")";
                StrQuery = StrQuery + " AND (substring(R.AccountBranchCode,0,charindex('-',isnull(R.AccountBranchCode,''))) IN (" + StrCredBrCode + ")";
                StrQuery = StrQuery + " OR substring(R.OtherBankBranchCode,0,charindex('-',isnull(R.OtherBankBranchCode,''))) IN (" + StrCredBrCode + "))";
            }
            
            //Date
            StrQuery = StrQuery + " AND Convert(varchar(10),R." + DateFilter + ",121) BETWEEN '" + Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd") + "'";

            //Amount Filter
            StrQuery = StrQuery + " AND R." + AmtComputation + " " + AmtFilter + " " + Amount;

            //Remitting Currency
            if (RemittingCCY != "")
            {
                StrQuery = StrQuery + " AND R.VRemittingCurrency = " + RemittingCCY;
            }

            // Target Currency
            if (TargetCCY != "")
            {
                StrQuery = StrQuery + " AND R.VNetToBenCurrency = " + TargetCCY;
            }

            // TransactionStatus
            if (TransactionStatus != "")
            {
                StrQuery = StrQuery + " AND R.vTranxStatus in (" + TransactionStatus + ")";
            }

            //TransactionSource
            if (TransactionSource != "")
            {
                StrQuery = StrQuery + " AND R.vTranxSource in (" + TransactionSource + ")";
            }

            //PayMode
            if (PayMode != "")
            {
                StrQuery = StrQuery + " AND R.vPayMode in (" + PayMode + ")";
            }

            //ProductCode
            if (ProductCode != "")
            {
                StrQuery = StrQuery + " AND R.vProductCode in (" + ProductCode + ")";
            }

            //RemitterID
            if (RemitterID != "")
            {
                StrQuery = StrQuery + " AND R.RemitterID in ('" + RemitterID + "')";
            }

            //BeneficiaryID
            if (BeneficiaryID != "")
            {
                StrQuery = StrQuery + " AND R.BeneficiaryID in ('" + BeneficiaryID + "')";
            }

            //AccountNo
            if (AccountNo != "")
            {
                StrQuery = StrQuery + " AND R.AccountNo in ('" + AccountNo + "')";
            }


            base.EnforceConstraint = false;
           
            getUpdateDS(StrQuery);

            //if (hasRows)
            //{
                selectedRow = 0;
                System.Web.HttpContext.Current.Session["tblData"] = dataset;
                return table;
                
            //}
            //else
            //{
            //    return null;
            //}

            
        }

        public string ExtractReporttoFile (string[] RemBrCode, string[] CredBrCode,
                            string DateFilter, string DateFrom, string DateTo, string RemittingCCY,
                            string TargetCCY, string AmtComputation, string AmtFilter, string Amount,
                            string[] RemitMethod, string[] BatchTypes, string ProductCode,
                            string TransactionSource, string PayMode, string TransactionStatus,
                            string RemitterID, string BeneficiaryID, string AccountNo, string[] ReportField, 
                            bool CheckMainBr, bool CheckRemBr, string profilename)
        {
            string StrQuery;
            string Ibatch;
            string otherChannels;
            string FieldsSelected;
            string StrRemBrCode;
            StrRemBrCode = ArrayIn(RemBrCode);
            string StrCredBrCode;
            StrCredBrCode = ArrayIn(CredBrCode);
            //Ibatch
            Ibatch = IBATCH_Array(BatchTypes);
            //OtherChannels
            otherChannels = OtherChannels_Array(BatchTypes);
            FieldsSelected = FieldSelections(ReportField);

            StrQuery = "SELECT DISTINCT ";
            //Create Selection Here
            StrQuery = StrQuery + FieldsSelected;

            StrQuery = StrQuery + " FROM TBL_REMITTANCE_IRSW R";

            StrQuery = StrQuery + " LEFT OUTER JOIN TBL_FILE_INVENTORY FI on FI.Seq_ID = R.FileSEQID";

            if (StrQuery.IndexOf("REMOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS REMOccupations ON REMOccupations.CIFID = R.RemitterID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP14 ON LUP14.FldValue = REMOccupations.Occupation AND LUP14.Fldgroup = 'Occupation'";

            }

            if (StrQuery.IndexOf("BENOccupations.") > 0)
            {
                StrQuery = StrQuery + " LEFT OUTER JOIN TBL_CIF_OCCUPATIONS BENOccupations ON BENOccupations.CIFID = R.BeneficiaryID";
                StrQuery = StrQuery + " LEFT OUTER JOIN tbl_lookup LUP15 ON LUP15.FldValue = BENOccupations.Occupation AND LUP15.Fldgroup = 'Occupation'";
            }


            //Remit Method
            StrQuery = StrQuery + " WHERE R.VRemitMethod IN (" + ArrayIn(RemitMethod) + ")";


            //IBATCH
            StrQuery = StrQuery + " AND ((ISNULL( FI.FileType,'') in ('',3)and ISNULL( FI.FileFormat,'') in " + Ibatch + ")";

            //OtherChannels
            StrQuery = StrQuery + " OR (ISNULL( FI.FileType,'') in ('',2)and ISNULL( FI.FileFormat,'') in " + otherChannels + "))";

            //RemittingBranch
            if ((!CheckRemBr) && (StrRemBrCode != ""))
            {
                StrQuery = StrQuery + " AND R.RemittingBranch IN (" + StrRemBrCode + ")";
            }

            //Reserve for Maintaining Branch
            if ((!CheckMainBr) && (StrCredBrCode != ""))
            {
                //StrQuery = StrQuery + " AND substring(R.AccountBranchCode,0,charindex('-',isnull(R.AccountBranchCode,''))) IN (" + StrCredBrCode + ")";
                StrQuery = StrQuery + " AND (substring(R.AccountBranchCode,0,charindex('-',isnull(R.AccountBranchCode,''))) IN (" + StrCredBrCode + ")";
                StrQuery = StrQuery + " OR substring(R.OtherBankBranchCode,0,charindex('-',isnull(R.OtherBankBranchCode,''))) IN (" + StrCredBrCode + "))";
            }

            //Date
            StrQuery = StrQuery + " AND Convert(varchar(10),R." + DateFilter + ",121) BETWEEN '" + Convert.ToDateTime(DateFrom).ToString("yyyy-MM-dd") + "' AND '" + Convert.ToDateTime(DateTo).ToString("yyyy-MM-dd") + "'";

            //Amount Filter
            StrQuery = StrQuery + " AND R." + AmtComputation + " " + AmtFilter + " " + Amount;

            //Remitting Currency
            if (RemittingCCY != "")
            {
                StrQuery = StrQuery + " AND R.VRemittingCurrency = " + RemittingCCY;
            }

            // Target Currency
            if (TargetCCY != "")
            {
                StrQuery = StrQuery + " AND R.VNetToBenCurrency = " + TargetCCY;
            }

            // TransactionStatus
            if (TransactionStatus != "")
            {
                StrQuery = StrQuery + " AND R.vTranxStatus in (" + TransactionStatus + ")";
            }

            //TransactionSource
            if (TransactionSource != "")
            {
                StrQuery = StrQuery + " AND R.vTranxSource in (" + TransactionSource + ")";
            }

            //PayMode
            if (PayMode != "")
            {
                StrQuery = StrQuery + " AND R.vPayMode in (" + PayMode + ")";
            }

            //ProductCode
            if (ProductCode != "")
            {
                StrQuery = StrQuery + " AND R.vProductCode in (" + ProductCode + ")";
            }

            //RemitterID
            if (RemitterID != "")
            {
                StrQuery = StrQuery + " AND R.RemitterID in ('" + RemitterID + "')";
            }

            //BeneficiaryID
            if (BeneficiaryID != "")
            {
                StrQuery = StrQuery + " AND R.BeneficiaryID in ('" + BeneficiaryID + "')";
            }

            //AccountNo
            if (AccountNo != "")
            {
                StrQuery = StrQuery + " AND R.AccountNo in ('" + AccountNo + "')";
            }


            base.EnforceConstraint = false;

            //getUpdateDS(StrQuery);
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


        /// <summary>
            /// Create and Excel File using Excel Xml
            /// </summary>
            /// <param name="FileName">Complete File Path</param>
            /// <param name="Ds">Data Set</param>
            /// <param name="SheetName">Work Sheet Name</param>
            /// <param name="RecordsPerSheet">No of records to show per sheet.</param>
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
                    X.WriteElementString("Author", "Ronald James L. Tuazon");
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
                            // Writing Data Rows
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

                    X.WriteEndElement(); // End Workbook Element
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
