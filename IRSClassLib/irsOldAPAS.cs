using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    /// <summary>
    /// Insert APAS/PAW to OLD System
    /// Created By Michael Ray C. Dizon 11-22-2010
    /// </summary>

    public class irsOldAPAS : sqlHelperWs
    {
        csAuditLog audit = new csAuditLog();
        //private String constringIRSWorld = "ConnectionString"; // commented to remove warning
        //private String constringAPAS = "ConnectionStringAPAS"; // commented to remove warning
        #region FOR PAW


        public string[] set_rem_ben_adress(string rem_adr_seq, string ben_adr_seq)
        {
            /* ADDED BY BOYET JAN 30 2017 TO GET THE COMPLETE ADDRESS OF CLIENT */
            DataTable dt = null;
            DataRow[] dr = null;

            string rem_adr1 = string.Empty;
            string rem_adr2 = string.Empty;
            string rem_adr3 = string.Empty;

            string ben_adr1 = string.Empty;
            string ben_adr2 = string.Empty;
            string ben_adr3 = string.Empty;

            long rem_adr_seq_1 = 0;
            long ben_adr_seq_1 = 0;

            try
            {
                if (string.IsNullOrEmpty(rem_adr_seq) == false)
                {
                    rem_adr_seq_1 = Convert.ToInt32(rem_adr_seq);
                }

                if (string.IsNullOrEmpty(ben_adr_seq) == false)
                {
                    ben_adr_seq_1 = Convert.ToInt32(ben_adr_seq);
                }

                base.EnforceConstraint = false;

                dt = getDataSet("irsw_to_apas_client_addr", true, new object[] { rem_adr_seq_1, ben_adr_seq_1 }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    dr = dt.Select("client_flag = 'REM'");

                    if (dr != null && dr.Length > 0)
                    {
                        rem_adr1 = (string)dr[0]["address1"];
                        rem_adr1 += " " + (string)dr[0]["address2"];
                        rem_adr1 += " " + (string)dr[0]["address3"];

                        rem_adr2 = (string)dr[0]["address4"];
                        rem_adr2 += " " + (string)dr[0]["address5"];
                        rem_adr2 += " " + (string)dr[0]["address6"];

                        rem_adr3 = (string)dr[0]["municipality"];
                        rem_adr3 += " " + (string)dr[0]["province"];

                        if (string.IsNullOrEmpty(rem_adr1).Equals(false) && rem_adr1.Length > 100)
                        {
                            rem_adr1 = rem_adr1.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(rem_adr2).Equals(false) && rem_adr2.Length > 100)
                        {
                            rem_adr2 = rem_adr2.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(rem_adr3).Equals(false) && rem_adr3.Length > 100)
                        {
                            rem_adr3 = rem_adr3.Substring(0, 100);
                        }
                    }

                    dr = null;
                    dr = dt.Select("client_flag = 'BEN'");

                    if (dr != null && dr.Length > 0)
                    {
                        ben_adr1 = (string)dr[0]["address1"];
                        ben_adr1 += " " + (string)dr[0]["address2"];
                        ben_adr1 += " " + (string)dr[0]["address3"];

                        ben_adr2 = (string)dr[0]["address4"];
                        ben_adr2 += " " + (string)dr[0]["address5"];
                        ben_adr2 += " " + (string)dr[0]["address6"];

                        ben_adr3 = (string)dr[0]["municipality"];
                        ben_adr3 += " " + (string)dr[0]["province"];

                        if (string.IsNullOrEmpty(ben_adr1).Equals(false) && ben_adr1.Length > 100)
                        {
                            ben_adr1 = ben_adr1.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(ben_adr2).Equals(false) && ben_adr2.Length > 100)
                        {
                            ben_adr2 = ben_adr2.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(ben_adr3).Equals(false) && ben_adr3.Length > 100)
                        {
                            ben_adr3 = ben_adr3.Substring(0, 100);
                        }
                    }
                }

                return new string[] { rem_adr1.Trim(), rem_adr2.Trim(), rem_adr3.Trim(), ben_adr1.Trim(), ben_adr2.Trim(), ben_adr3.Trim() };
            }
            catch
            {
                return new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                dr = null;
            }
        }

        /*This will insert PAW in APAS Server*/
        public bool InsertAnPAW(int tranxID)
        {
            base.constring_suffix = "ConnectionString";

            string tranxdate;
            string currentDate;
            string remittingbank;
            DataTable dtRemittance;
            string accountno = "";
            string accounttype = "";

            dtRemittance = GetRemittance(tranxID);

            if (this.getCheckAPASTxnIfExists(dtRemittance.Rows[0]["ReferenceNo"].ToString())) 
            { 
                return true; 
            }

            base.constring_suffix = "ConnectionString";

            string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);

            string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";

            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017*/
            string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            {
                remaddress1 = rem_ben_adrs[0];
                remaddress2 = rem_ben_adrs[1];
                remaddress3 = rem_ben_adrs[2];

                beneaddress1 = rem_ben_adrs[3];
                beneaddress2 = rem_ben_adrs[4];
                beneaddress3 = rem_ben_adrs[5];
            }
            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */

            int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            if (dtAccountInfo.Rows.Count > 0)
            {
                accountno = dtAccountInfo.Rows[0]["AccountNo"].ToString();
                accounttype = dtAccountInfo.Rows[0]["AccountType"].ToString();
            }

            /* variables */

            string referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            string rec_curr_cd = Convert.ToString(this.execScalar("SELECT fldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
            string rec_acctCurr_Cd = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));

            decimal rec_Applied_Rate = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"].ToString());
            
            string rec_RefNum_Ours = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);

            string rec_Benf_Name = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_Spl_Mesg = dtRemittance.Rows[0]["SpecialMessage"].ToString();
            string rec_Rem_Name = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            decimal rec_Total_Chrg = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
            decimal rec_Net_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            decimal rec_Net_Dollar = Convert.ToDecimal(dtRemittance.Rows[0]["NetRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexLocalToUSD"]);
            decimal rec_Gross_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
            string rec_Payee_Benf = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_Orig_BrCd = Convert.ToString(this.execScalar("Select NegBranchCode From TBL_Branches WHERE Branchcode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            string rec_Benef_ContactNo = Convert.ToString(this.execScalar("Select TOP 1 ContactNo from tbl_cif_contacts where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_RemitterID = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
            string rec_NSVS_CASA_Acct = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));//dtRemittance.Rows[0]["DefAccountNo"].ToString(); //hindi ko alam
            string rec_BeneFName = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_BeneLName = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_BeneMName = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_RemFName = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            string rec_RemMName = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            string rec_RemLName = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            string rec_TrxnRefNum = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            decimal rec_NetToBene = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);


            try
            {
                //change connection string
                base.constring_suffix = "ConnectionStringAPAS";

                this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_TrxnRef_Num='-1'");
                if (this.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    if (remittingbank == "A06")
                    {
                        referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                    }
                    else
                    {
                        referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                    }

                    uprow["rec_TrxnRef_Num"] = referenceno;
                    uprow["rec_Curr_Cd"] = rec_curr_cd;
                    uprow["rec_AcctCurr_Cd"] = rec_acctCurr_Cd;
                    uprow["rec_Applied_Rate"] = rec_Applied_Rate;
                    uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                    uprow["rec_Amount_FD"] = 0;
                    uprow["rec_PNBr_CD"] = 170;
                    uprow["rec_PNBr_Name"] = "ORD";
                    uprow["rec_Benf_Name"] = rec_Benf_Name;
                    uprow["rec_Benf_Adr1"] = beneaddress1;
                    uprow["rec_Benf_Adr2"] = beneaddress2;
                    uprow["rec_Benf_Adr3"] = beneaddress3;
                    uprow["rec_Spl_Mesg"] = rec_Spl_Mesg;
                    uprow["rec_Rem_Name"] = rec_Rem_Name;
                    uprow["rec_Total_Chrg"] = rec_Total_Chrg;
                    uprow["rec_Net_Peso"] = rec_Net_Peso;
                    uprow["rec_Net_Dollar"] = rec_Net_Dollar;
                    uprow["rec_Tran_Date"] = tranxdate;
                    uprow["rec_Acct_No"] = accountno;
                    uprow["rec_Acct_Type1"] = accounttype;
                    uprow["rec_RB_Name"] = remittingbank;
                    uprow["rec_Pay_Mode"] = 0;
                    uprow["rec_Gross_Peso"] = rec_Gross_Peso;
                    uprow["rec_Date_Received"] = currentDate;
                    uprow["rec_Payee_Benf"] = rec_Payee_Benf;
                    uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                    uprow["rec_Orig_BrCd"] = System.Web.HttpContext.Current.Session["NegBranchCode"];
                    uprow["rec_Rem_Add1"] = remaddress1;
                    uprow["rec_Rem_Add2"] = remaddress2;
                    uprow["rec_Rem_Add3"] = remaddress3;
                    uprow["rec_Benef_MobileNo"] = "";
                    uprow["rec_Benef_ContactNo"] = rec_Benef_ContactNo;
                    uprow["rec_RemitterID"] = rec_RemitterID;
                    uprow["rec_ProdCode"] = "PAW";
                    uprow["rec_NSVS_CASA_Acct"] = rec_NSVS_CASA_Acct;
                    uprow["rec_RemPurpose"] = transactionpurpose;
                    uprow["rec_BeneFName"] = rec_BeneFName; 
                    uprow["rec_BeneLName"] = rec_BeneLName;
                    uprow["rec_BeneMName"] = rec_BeneMName;
                    uprow["rec_RemFName"] = rec_RemFName;
                    uprow["rec_RemLName"] = rec_RemLName;
                    uprow["rec_RemMName"] = rec_RemMName;
                    uprow["rec_SubBrCode"] = subbranchcode;
                    this.save(sqlHelper.RETAIN_DATASET);
                }

                /*Transaction Details*/
                this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_TrxnRefNum='-1'");
                if (this.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    uprow["rec_TrxnRefNum"] = referenceno;
                    uprow["rec_TrandInd"] = "NP";
                    uprow["rec_lockind"] = "No";
                    uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                    uprow["rec_Tran_Date"] = tranxdate;
                    this.save(sqlHelperWs.RETAIN_DATASET);
                    return true;
                }
                /*Transaction Details END*/

            }
            catch // (Exception ex) // commented to remove warning
            {
                return InsertAnPAWCentral(tranxID);
            }

            return false;
        }



        private bool InsertAnPAWCentral(int tranxID)
        {
            base.constring_suffix = "ConnectionString";
            string tranxdate;
            string currentDate;
            string remittingbank;
            DataTable dtRemittance;
            string accountno = "";
            string accounttype = "";


            dtRemittance = GetRemittance(tranxID);
            string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);

            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */
            string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";

            string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            {
                remaddress1 = rem_ben_adrs[0];
                remaddress2 = rem_ben_adrs[1];
                remaddress3 = rem_ben_adrs[2];

                beneaddress1 = rem_ben_adrs[3];
                beneaddress2 = rem_ben_adrs[4];
                beneaddress3 = rem_ben_adrs[5];
            }
            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */

            int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            if (dtAccountInfo.Rows.Count > 0)
            {
                accountno = dtAccountInfo.Rows[0]["AccountNo"].ToString();
                accounttype = dtAccountInfo.Rows[0]["AccountType"].ToString();
            }

            //begin save
            this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_TrxnRef_Num='-1'");

            try
            {
                if (this.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    uprow["rec_TrxnRef_Num"] = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                    uprow["rec_Curr_Cd"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
                    uprow["rec_AcctCurr_Cd"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
                    uprow["rec_Applied_Rate"] = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"].ToString());
                    uprow["rec_RefNum_Ours"] = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);
                    uprow["rec_Amount_FD"] = 0;
                    uprow["rec_PNBr_CD"] = 170;
                    uprow["rec_PNBr_Name"] = "ORD";
                    uprow["rec_Benf_Name"] = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_Benf_Adr1"] = beneaddress1.Substring(0, 25);
                    uprow["rec_Benf_Adr2"] = beneaddress2.Substring(0, 25);
                    uprow["rec_Benf_Adr3"] = beneaddress3.Substring(0, 25);
                    uprow["rec_Spl_Mesg"] = dtRemittance.Rows[0]["SpecialMessage"].ToString();
                    uprow["rec_Rem_Name"] = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_Total_Chrg"] = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
                    uprow["rec_Net_Peso"] = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
                    uprow["rec_Net_Dollar"] = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
                    uprow["rec_Tran_Date"] = tranxdate;
                    uprow["rec_Acct_No"] = accountno;
                    uprow["rec_Acct_Type1"] = accounttype;
                    uprow["rec_RB_Name"] = remittingbank;
                    uprow["rec_Pay_Mode"] = 0;
                    uprow["rec_Gross_Peso"] = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
                    uprow["rec_Date_Received"] = currentDate;
                    uprow["rec_Payee_Benf"] = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                    uprow["rec_Orig_BrCd"] = System.Web.HttpContext.Current.Session["NegBranchCode"];
                    uprow["rec_Rem_Add1"] = remaddress1;
                    uprow["rec_Rem_Add2"] = remaddress2;
                    uprow["rec_Rem_Add3"] = remaddress3;
                    uprow["rec_Benef_MobileNo"] = "";
                    uprow["rec_Benef_ContactNo"] = Convert.ToString(this.execScalar("Select TOP 1 ContactNo from tbl_cif_contacts where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_RemitterID"] = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
                    uprow["rec_ProdCode"] = "PAW";
                    uprow["rec_NSVS_CASA_Acct"] = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));
                    uprow["rec_RemPurpose"] = transactionpurpose;
                    uprow["rec_BeneFName"] = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_BeneLName"] = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_BeneMName"] = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_RemFName"] = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_RemLName"] = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_RemMName"] = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_SubBrCode"] = subbranchcode;
                    this.save(sqlHelperWs.RETAIN_DATASET);

                }

                /*Transaction Details*/
                this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_TrxnRefNum='-1'");
                if (this.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    uprow["rec_TrxnRefNum"] = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                    uprow["rec_TrandInd"] = "NP";
                    uprow["rec_lockind"] = "No";
                    uprow["rec_RefNum_Ours"] = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);
                    uprow["rec_Tran_Date"] = tranxdate;
                    this.save(sqlHelperWs.RETAIN_DATASET);
                    return true;
                }
                /*Transaction Details END*/
            }
            catch (Exception ex)
            {
                APASMessage = "Error encountered while saving PAW Transaction " + ex.Message;
                return false;
                //InsertAPASCentral(tranxID);
            }

            return false;
        }
        #endregion

        #region FOR APAS
        /*This will insert APAS in APAS Server*/
        public bool InsertApas(int tranxID)
        {
            ///================================================================
            ///Revise saving IRSW APAS transactions to APAS DB 02182023
            irsSqlCommonsWs irsw_commons = new irsSqlCommonsWs();
            irsSqlCommonsWs apas_commons = new irsSqlCommonsWs();

            ////bool bool_master = false;
            ////bool bool_details = false;

            ////base.constring_suffix = "ConnectionString";
            ////string tranxdate;
            ////string currentDate;
            ////string remittingbank;
            ////DataTable dtRemittance;
            ////// commented to remove warning - string accountno = "";
            ////string accounttype = "";
            ////// commented to remove warning - bool ifSuccess;


            ////dtRemittance = GetRemittance(tranxID);
            ////if (this.getCheckAPASTxnIfExists(dtRemittance.Rows[0]["ReferenceNo"].ToString())) { return true; }

            ////base.constring_suffix = "ConnectionString";

            ////string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            ////string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            ////remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            ////currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            ////tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);

            ////string rembank = dtRemittance.Rows[0]["RemittingBranch"].ToString();

            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */
            ////string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            ////string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";

            ////string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            ////if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            ////{
            ////    remaddress1 = rem_ben_adrs[0];
            ////    remaddress2 = rem_ben_adrs[1];
            ////    remaddress3 = rem_ben_adrs[2];

            ////    beneaddress1 = rem_ben_adrs[3];
            ////    beneaddress2 = rem_ben_adrs[4];
            ////    beneaddress3 = rem_ben_adrs[5];
            ////}
            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */

            ////int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            ////DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("select * from tbl_lookup where fldgroup='APAS_Online'", false)).Tables[0];//this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            ////if (dtAccountInfo.Rows.Count > 0)
            ////{
            ////    accounttype = "";
            ////}

            /////* variables */
            ////string referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            ////string rec_curr_cd = Convert.ToString(this.execScalar("SELECT fldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
            ////string rec_acctCurr_Cd = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));

            ////decimal rec_Applied_Rate = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"].ToString());

            ////string rec_RefNum_Ours = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);

            ////string rec_Benf_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Spl_Mesg = dtRemittance.Rows[0]["SpecialMessage"].ToString();
            ////string rec_Rem_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////decimal rec_Total_Chrg = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
            ////decimal rec_Net_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Net_Dollar = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Gross_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
            ////string rec_Payee_Benf = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Orig_BrCd = Convert.ToString(this.execScalar("Select NegBranchCode From TBL_Branches WHERE Branchcode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            ////string rec_Benef_ContactNo = Convert.ToString(this.execScalar("Select TOP 1 ContactNo from tbl_cif_contacts where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_RemitterID = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
            ////string rec_NSVS_CASA_Acct = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));
            ////string rec_BeneFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_RemFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_TrxnRefNum = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            ////decimal rec_NetToBene = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);

            DataTable dtRemittance;
            bool bool_master = false;
            bool bool_details = false;
            base.constring_suffix = "ConnectionString";

            string rec_curr_cd = "";
            string rec_acctCurr_Cd = "";
            decimal rec_Applied_Rate = 0;
            string rec_RefNum_Ours = "";
            string rec_Benf_Name = "";
            string rec_Spl_Mesg = "";
            string rec_Rem_Name = "";
            decimal rec_Total_Chrg = 0;
            decimal rec_Net_Peso = 0;
            decimal rec_Net_Dollar = 0;
            decimal rec_Gross_Peso = 0;
            string rec_Payee_Benf = "";
            string rec_RemitterID = "";
            string rec_NSVS_CASA_Acct = "";
            string rec_BeneFName = "";
            string rec_BeneLName = "";
            string rec_BeneMName = "";
            string rec_RemFName = "";
            string rec_RemMName = "";
            string rec_RemLName = "";
            string referenceno = "";
            string remaddress1 = "";
            string remaddress2 = "";
            string remaddress3 = "";
            string beneaddress1 = "";
            string beneaddress2 = "";
            string beneaddress3 = "";
            string tranxdate = "";
            string currentDate = "";
            string remittingbank = "";
            string accounttype = "";
            string apas_acct_no = "";
            string rec_Benef_ContactNo_value = "";
            string rec_Benef_ContactNo = "";
            string transactionpurpose = "";
            string subbranchcode = "";
            string rembank = "";
            string rec_Orig_BrCd = "";
            ///================================================================

            try
            {
                ///================================================================
                ///Revise saving IRSW APAS transactions to APAS DB 02182023
                DataTable dt_apastran = new DataTable();
                DataTable dt_irswapas = new DataTable();
                DataTable dt_apas = new DataTable();
                DataTable dt_apas_details = new DataTable();
                int apas_enabled = 0;

                ///IRSWorld Connection string
                irsw_commons.constring_suffix = "ConnectionString";
                ///APAS Connection string
                apas_commons.constring_suffix = "ConnectionStringAPAS";

                dtRemittance = GetRemittance(tranxID);
                rembank = Convert.ToString(dtRemittance.Rows[0]["RemittingBranch"]);

                // check if referenceno is already existing in APAS
                if (this.getCheckAPASTxnIfExists(dtRemittance.Rows[0]["ReferenceNo"].ToString()))
                {
                    return true;
                }

                audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Get record from IRSW DB", "proc_process_irswapas=" + tranxID.ToString() + ", 1", "", csAuditLog.Category.Insert);
                dt_apastran = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 1, "" });

                if (dt_apastran != null && dt_apastran.Rows.Count > 0)
                {
                    ///Set APAS values from IRSW TBL_APTrans table
                    if (rembank == "A06")
                    {
                        referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                        audit.writeAudit("irsOldAPAS", "Check.APAS.RefNo", "Reference No", "rec_TrxnRefNum:" + referenceno.ToString() + " RemBank:" + rembank, "", csAuditLog.Category.Insert);
                    }
                    else
                    {
                        referenceno = dt_apastran.Rows[0]["rec_TrxnRef_Num"].ToString();
                        audit.writeAudit("irsOldAPAS", "Check.APAS.RefNo", "Reference No", "rec_TrxnRefNum:" + referenceno.ToString() + " RemBank:" + rembank, "", csAuditLog.Category.Insert);

                    }
                    rec_curr_cd = dt_apastran.Rows[0]["rec_Curr_Cd"].ToString();
                    rec_acctCurr_Cd = dt_apastran.Rows[0]["rec_AcctCurr_Cd"].ToString();
                    rec_Applied_Rate = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Applied_Rate"].ToString());
                    rec_RefNum_Ours = dt_apastran.Rows[0]["rec_RefNum_Ours"].ToString();
                    rec_Benf_Name = dt_apastran.Rows[0]["rec_Benf_Name"].ToString();
                    beneaddress1 = dt_apastran.Rows[0]["rec_Benf_Adr1"].ToString();
                    beneaddress2 = dt_apastran.Rows[0]["rec_Benf_Adr2"].ToString();
                    beneaddress3 = dt_apastran.Rows[0]["rec_Benf_Adr3"].ToString();
                    rec_Spl_Mesg = dt_apastran.Rows[0]["rec_Spl_Mesg"].ToString();
                    rec_Rem_Name = dt_apastran.Rows[0]["rec_Rem_Name"].ToString();
                    rec_Total_Chrg = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Total_Chrg"].ToString());
                    rec_Net_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Peso"].ToString());
                    rec_Net_Dollar = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Dollar"].ToString());
                    tranxdate = dt_apastran.Rows[0]["rec_Tran_Date"].ToString();
                    apas_acct_no = dt_apastran.Rows[0]["rec_Acct_No"].ToString();
                    accounttype = dt_apastran.Rows[0]["rec_Acct_Type1"].ToString();
                    remittingbank = dt_apastran.Rows[0]["rec_RB_Name"].ToString();
                    rec_Gross_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Gross_Peso"].ToString());
                    currentDate = dt_apastran.Rows[0]["rec_Date_Received"].ToString();
                    rec_Payee_Benf = dt_apastran.Rows[0]["rec_Payee_Benf"].ToString();
                    rec_Orig_BrCd = dt_apastran.Rows[0]["rec_Orig_BrCd"].ToString();
                    remaddress1 = dt_apastran.Rows[0]["rec_Rem_Add1"].ToString();
                    remaddress2 = dt_apastran.Rows[0]["rec_Rem_Add2"].ToString();
                    remaddress3 = dt_apastran.Rows[0]["rec_Rem_Add3"].ToString();
                    rec_Benef_ContactNo_value = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_Benef_ContactNo = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_RemitterID = dt_apastran.Rows[0]["rec_RemitterID"].ToString();
                    rec_NSVS_CASA_Acct = dt_apastran.Rows[0]["rec_NSVS_CASA_Acct"].ToString();
                    transactionpurpose = dt_apastran.Rows[0]["rec_RemPurpose"].ToString();
                    rec_BeneFName = dt_apastran.Rows[0]["rec_BeneFName"].ToString();
                    rec_BeneLName = dt_apastran.Rows[0]["rec_BeneLName"].ToString();
                    rec_BeneMName = dt_apastran.Rows[0]["rec_BeneMName"].ToString();
                    rec_RemFName = dt_apastran.Rows[0]["rec_RemFName"].ToString();
                    rec_RemLName = dt_apastran.Rows[0]["rec_RemLName"].ToString();
                    rec_RemMName = dt_apastran.Rows[0]["rec_RemMName"].ToString();
                    subbranchcode = dt_apastran.Rows[0]["rec_SubBrCode"].ToString();

                    //Check if APAS switch is enabled
                    dt_irswapas = irsw_commons.execStoredProcedureTable("isp_getlookup", new object[] { "EnableIRSWAPAS" });
                    if (dt_irswapas != null && dt_irswapas.Rows.Count > 0)
                    {
                        apas_enabled = Convert.ToInt32(dt_irswapas.Rows[0]["FldCode"].ToString());
                    }

                    ///APAS switch enabled
                    if (apas_enabled > 0)
                    {
                        ///Save query log information to IRSW DB
                        audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Save records to APAS DB tbl_APTrans", "proc_insert_apas_trxn=" + referenceno + "~" +
                                                                                                                                                    rec_curr_cd + "~" +
                                                                                                                                                    rec_acctCurr_Cd + "~" +
                                                                                                                                                    rec_Applied_Rate.ToString() + "~" +
                                                                                                                                                    rec_RefNum_Ours + "~" +
                                                                                                                                                    "0" + "~" +
                                                                                                                                                    "170" + "~" +
                                                                                                                                                    "ORD" + "~" +
                                                                                                                                                    rec_Benf_Name + "~" +
                                                                                                                                                    beneaddress1 + "~" +
                                                                                                                                                    beneaddress2 + "~" +
                                                                                                                                                    beneaddress3 + "~" +
                                                                                                                                                    rec_Spl_Mesg + "~" +
                                                                                                                                                    rec_Rem_Name + "~" +
                                                                                                                                                    rec_Total_Chrg.ToString() + "~" +
                                                                                                                                                    rec_Net_Peso.ToString() + "~" +
                                                                                                                                                    rec_Net_Dollar.ToString() + "~" +
                                                                                                                                                    tranxdate + "~" +
                                                                                                                                                    apas_acct_no + "~" +
                                                                                                                                                    accounttype + "~" +
                                                                                                                                                    remittingbank + "~" +
                                                                                                                                                    "0" + "~" +
                                                                                                                                                    rec_Gross_Peso.ToString() + "~" +
                                                                                                                                                    currentDate + "~" +
                                                                                                                                                    rec_Payee_Benf + "~" +
                                                                                                                                                    System.DateTime.Now.ToString() + "~" +
                                                                                                                                                    rec_Orig_BrCd + "~" +
                                                                                                                                                    remaddress1 + "~" +
                                                                                                                                                    remaddress2 + "~" +
                                                                                                                                                    remaddress3 + "~" +
                                                                                                                                                    "" + "~" +
                                                                                                                                                    rec_Benef_ContactNo_value + "~" +
                                                                                                                                                    rec_RemitterID + "~" +
                                                                                                                                                    "APAS" + "~" +
                                                                                                                                                    rec_NSVS_CASA_Acct + "~" +
                                                                                                                                                    transactionpurpose + "~" +
                                                                                                                                                    rec_BeneFName + "~" +
                                                                                                                                                    rec_BeneLName + "~" +
                                                                                                                                                    rec_BeneMName + "~" +
                                                                                                                                                    rec_RemFName + "~" +
                                                                                                                                                    rec_RemLName + "~" +
                                                                                                                                                    rec_RemMName + "~" +
                                                                                                                                                    subbranchcode, "", csAuditLog.Category.Insert);

                        ///Save IRSW APAS transaction to APAS DB  
                        dt_apas = apas_commons.execStoredProcedureTable("proc_insert_apas_trxn", new object[] { referenceno,
                                                                                                             rec_curr_cd,
                                                                                                             rec_acctCurr_Cd,
                                                                                                             rec_Applied_Rate,
                                                                                                             rec_RefNum_Ours,
                                                                                                             0,
                                                                                                             "170",
                                                                                                             "ORD",
                                                                                                             rec_Benf_Name,
                                                                                                             beneaddress1,
                                                                                                             beneaddress2,
                                                                                                             beneaddress3,
                                                                                                             rec_Spl_Mesg,
                                                                                                             rec_Rem_Name,
                                                                                                             rec_Total_Chrg,
                                                                                                             rec_Net_Peso,
                                                                                                             rec_Net_Dollar,
                                                                                                             tranxdate,
                                                                                                             apas_acct_no,
                                                                                                             accounttype,
                                                                                                             remittingbank,
                                                                                                             "0",
                                                                                                             rec_Gross_Peso,
                                                                                                             currentDate,
                                                                                                             rec_Payee_Benf,
                                                                                                             System.DateTime.Now.ToString(),
                                                                                                             rec_Orig_BrCd,
                                                                                                             remaddress1,
                                                                                                             remaddress2,
                                                                                                             remaddress3,
                                                                                                             "",
                                                                                                             rec_Benef_ContactNo_value,
                                                                                                             rec_RemitterID,
                                                                                                             "APAS",
                                                                                                             rec_NSVS_CASA_Acct,
                                                                                                             transactionpurpose,
                                                                                                             rec_BeneFName,
                                                                                                             rec_BeneLName,
                                                                                                             rec_BeneMName,
                                                                                                             rec_RemFName,
                                                                                                             rec_RemLName,
                                                                                                             rec_RemMName,
                                                                                                             subbranchcode
                                                                                                        });

                        if (dt_apas != null && dt_apas.Rows.Count > 0)
                        {
                            bool_master = true;
                            ///Save log information to IRSW DB
                            audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans", "rec_TrxnRef_Num:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                            audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Save records to APAS DB tbl_APTranDetails", "proc_insert_apas_trxn_details=" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", csAuditLog.Category.Insert);

                            ///Save IRSW APAS Tran Details to APAS DB
                            dt_apas_details = apas_commons.execStoredProcedureTable("proc_insert_apas_trxn_details", new object[] { referenceno, "NP", "No", rec_RefNum_Ours, tranxdate });

                            if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                            {
                                bool_details = true;
                                audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                            }
                            else
                            {
                                bool_details = false;
                                audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails : " + apas_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);

                                return InsertAPASCentral(tranxID, apas_commons.Message.ToString().Trim());
                            }
                        }
                        else
                        {
                            bool_master = false;
                            bool_details = false;
                            audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTrans : " + apas_commons.Message, "rec_TrxnRef_Num:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                            audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails : " + apas_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);

                            return InsertAPASCentral(tranxID, apas_commons.Message.ToString().Trim());
                        }
                    }
                    //================================================================
                    else
                    {
                        base.constring_suffix = "ConnectionStringAPAS";
                        //this = new irsSqlCommons("DevelConnStringAPAS");
                        this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_trxnref_num='-1'");
                        if (this.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            if (rembank == "A06")
                            {
                                referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                                audit.writeAudit("irsOldAPAS", "Check.APAS.RefNo", "Reference No", "rec_TrxnRefNum:" + referenceno.ToString() + " RemBank:" + rembank, "", csAuditLog.Category.Insert);
                            }
                            else
                            {
                                referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                                audit.writeAudit("irsOldAPAS", "Check.APAS.RefNo", "Reference No", "rec_TrxnRefNum:" + referenceno.ToString() + " RemBank:" + rembank, "", csAuditLog.Category.Insert);

                            }

                            uprow["rec_TrxnRef_Num"] = referenceno;
                            uprow["rec_Curr_Cd"] = rec_curr_cd;
                            uprow["rec_AcctCurr_Cd"] = rec_acctCurr_Cd;
                            uprow["rec_Applied_Rate"] = rec_Applied_Rate;
                            uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                            uprow["rec_Amount_FD"] = 0;
                            uprow["rec_PNBr_CD"] = 170;
                            uprow["rec_PNBr_Name"] = "ORD";
                            uprow["rec_Benf_Name"] = rec_Benf_Name;
                            uprow["rec_Benf_Adr1"] = beneaddress1;
                            uprow["rec_Benf_Adr2"] = beneaddress2;
                            uprow["rec_Benf_Adr3"] = beneaddress3;
                            uprow["rec_Spl_Mesg"] = rec_Spl_Mesg;
                            uprow["rec_Rem_Name"] = rec_Rem_Name;
                            uprow["rec_Total_Chrg"] = rec_Total_Chrg;
                            uprow["rec_Net_Peso"] = rec_Net_Peso;
                            uprow["rec_Net_Dollar"] = rec_Net_Dollar;
                            uprow["rec_Tran_Date"] = tranxdate;
                            uprow["rec_Acct_No"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'APAS_Online' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
                            uprow["rec_Acct_Type1"] = accounttype;
                            uprow["rec_RB_Name"] = remittingbank;
                            uprow["rec_Pay_Mode"] = 0;
                            uprow["rec_Gross_Peso"] = rec_Gross_Peso;
                            uprow["rec_Date_Received"] = currentDate;
                            uprow["rec_Payee_Benf"] = rec_Payee_Benf;
                            uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                            uprow["rec_Orig_BrCd"] = System.Web.HttpContext.Current.Session["NegBranchCode"];
                            uprow["rec_Rem_Add1"] = remaddress1;
                            uprow["rec_Rem_Add2"] = remaddress2;
                            uprow["rec_Rem_Add3"] = remaddress3;
                            uprow["rec_Benef_MobileNo"] = "";
                            uprow["rec_Benef_ContactNo"] = (rec_Benef_ContactNo.Length > 20) ? rec_Benef_ContactNo.Substring(0, 20) : rec_Benef_ContactNo;
                            uprow["rec_RemitterID"] = rec_RemitterID;
                            uprow["rec_ProdCode"] = "APAS";
                            uprow["rec_NSVS_CASA_Acct"] = rec_NSVS_CASA_Acct;
                            uprow["rec_RemPurpose"] = transactionpurpose;
                            uprow["rec_BeneFName"] = rec_BeneFName;
                            uprow["rec_BeneLName"] = rec_BeneLName;
                            uprow["rec_BeneMName"] = rec_BeneMName;
                            uprow["rec_RemFName"] = rec_RemFName;
                            uprow["rec_RemLName"] = rec_RemLName;
                            uprow["rec_RemMName"] = rec_RemMName;
                            uprow["rec_SubBrCode"] = subbranchcode;

                            bool_master = this.save(sqlHelper.RETAIN_DATASET);

                        }

                        /*Transaction Details*/
                        if (bool_master == true)
                        {
                            audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                            this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_trxnRefNum='-1'");

                            if (this.newRow(sqlHelper.RETAIN_DATASET))
                            {
                                uprow["rec_TrxnRefNum"] = referenceno;
                                uprow["rec_TrandInd"] = "NP";
                                uprow["rec_lockind"] = "No";
                                uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                                uprow["rec_Tran_Date"] = tranxdate;

                                bool_details = this.save(sqlHelper.RETAIN_DATASET);

                                if (bool_details == false)
                                {
                                    audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails try central: " + base.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                                    return InsertAPASCentral(tranxID, base.Message.ToString().Trim());
                                }
                                else
                                {
                                    audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                                }

                                //base.constring_suffix = "ConnectionString";                   
                            }
                        }
                        else
                        {
                            audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTrans try central: " + base.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                            return InsertAPASCentral(tranxID, base.Message.ToString().Trim());
                        }
                    }
                }
                else
                {
                    ///================================================================
                    ///Revise saving IRSW APAS transactions to APAS DB 02182023
                    audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Unable to retrieve record from IRSW DB : " + irsw_commons.Message, "IRSW SEQ_ID:" + tranxID.ToString(), "", csAuditLog.Category.Select);
                    return false;
                    ///================================================================
                }
                /*Transaction Details END*/
            }
            catch (Exception ex) // commented to remove warning
            {
                audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Failed to Load in APAS DB from insertApasException try central: " + ex.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                return InsertAPASCentral(tranxID, ex.Message.ToString().Trim());

            }
            finally
            {
                irsw_commons = null;
                apas_commons = null;
            }

            if (bool_details == true && bool_master == true)
            {
                audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans & tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);

                return true;
            }

            audit.writeAudit("irsOldAPAS", "Uploading.APASSever", "Failed to Load in APAS DB try Central", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);

            return InsertAPASCentral(tranxID, "");
        }


        /*This will insert APAS in IRSWorld Central Database*/
        public bool InsertAPASCentral(int tranxID, string error_message)
        {
            ///================================================================
            ///Revise saving IRSW APAS transactions to APAS DB 02182023
            irsSqlCommonsWs irsw_commons = new irsSqlCommonsWs();

            ////bool bool_master = false;
            ////bool bool_details = false;

            ////base.constring_suffix = "ConnectionString";
            ////string tranxdate;
            ////string currentDate;
            ////string remittingbank;
            ////DataTable dtRemittance;
            ////// string accountno = ""; // commented to remove warning
            ////string accounttype = "";
            ////// bool ifSuccess; // commented to remove warning


            ////dtRemittance = GetRemittance(tranxID);
            ////string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            ////string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            ////remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            ////currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            ////tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);
            ////string rembank = dtRemittance.Rows[0]["RemittingBranch"].ToString();

            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */
            ////string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            ////string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";

            ////string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            ////if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            ////{
            ////    remaddress1 = rem_ben_adrs[0];
            ////    remaddress2 = rem_ben_adrs[1];
            ////    remaddress3 = rem_ben_adrs[2];

            ////    beneaddress1 = rem_ben_adrs[3];
            ////    beneaddress2 = rem_ben_adrs[4];
            ////    beneaddress3 = rem_ben_adrs[5];
            ////}
            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */


            ////int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            ////DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            ////if (dtAccountInfo.Rows.Count > 0)
            ////{
            ////    accounttype = dtAccountInfo.Rows[0]["AccountType"].ToString();
            ////}

            /////* variables */
            ////string referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            ////string rec_curr_cd = Convert.ToString(this.execScalar("SELECT fldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
            ////string rec_acctCurr_Cd = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
            ////decimal rec_Applied_Rate = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"].ToString());
            ////string rec_RefNum_Ours = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);

            ////string rec_Benf_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Spl_Mesg = dtRemittance.Rows[0]["SpecialMessage"].ToString();
            ////string rec_Rem_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////decimal rec_Total_Chrg = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
            ////decimal rec_Net_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Net_Dollar = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Gross_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
            ////string rec_Payee_Benf = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Orig_BrCd = Convert.ToString(this.execScalar("Select NegBranchCode From TBL_Branches WHERE Branchcode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            ////string rec_Benef_ContactNo = Convert.ToString(this.execScalar("Select TOP 1 ContactNo from tbl_cif_contacts where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_RemitterID = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
            ////string rec_NSVS_CASA_Acct = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));
            ////string rec_BeneFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_RemFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_TrxnRefNum = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            ////decimal rec_NetToBene = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);

            DataTable dtRemittance;
            bool bool_master = false;
            bool bool_details = false;
            base.constring_suffix = "ConnectionString";

            string rec_curr_cd = "";
            string rec_acctCurr_Cd = "";
            decimal rec_Applied_Rate = 0;
            string rec_RefNum_Ours = "";
            string rec_Benf_Name = "";
            string rec_Spl_Mesg = "";
            string rec_Rem_Name = "";
            decimal rec_Total_Chrg = 0;
            decimal rec_Net_Peso = 0;
            decimal rec_Net_Dollar = 0;
            decimal rec_Gross_Peso = 0;
            string rec_Payee_Benf = "";
            string rec_RemitterID = "";
            string rec_NSVS_CASA_Acct = "";
            string rec_BeneFName = "";
            string rec_BeneLName = "";
            string rec_BeneMName = "";
            string rec_RemFName = "";
            string rec_RemMName = "";
            string rec_RemLName = "";
            string referenceno = "";
            string remaddress1 = "";
            string remaddress2 = "";
            string remaddress3 = "";
            string beneaddress1 = "";
            string beneaddress2 = "";
            string beneaddress3 = "";
            string tranxdate = "";
            string currentDate = "";
            string remittingbank = "";
            string accounttype = "";
            string apas_acct_no = "";
            string rec_Orig_BrCd = "";
            string rec_Benef_ContactNo_value = "";
            string rec_Benef_ContactNo = "";
            string transactionpurpose = "";
            string subbranchcode = "";
            string rembank = "";
            ///================================================================

            try
            {
                ///================================================================
                ///Revise saving IRSW APAS transactions to APAS DB 02182023
                DataTable dt_apastran = new DataTable();
                DataTable dt_irswapas = new DataTable();
                DataTable dt_apas = new DataTable();
                DataTable dt_apas_details = new DataTable();
                int apas_enabled = 0;

                ///IRSWorld Connection string
                irsw_commons.constring_suffix = "ConnectionString";

                dtRemittance = GetRemittance(tranxID);
                rembank = Convert.ToString(dtRemittance.Rows[0]["RemittingBranch"]);

                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Get record from IRSW DB", "proc_process_irswapas=" + tranxID.ToString() + ", 1", "", csAuditLog.Category.Insert);
                dt_apastran = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 1, "" });

                if (dt_apastran != null && dt_apastran.Rows.Count > 0)
                {
                    ///Set APAS values from IRSW TBL_APTrans table
                    if (rembank == "A06")
                    {
                        referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                    }
                    else
                    {
                        referenceno = dt_apastran.Rows[0]["rec_TrxnRef_Num"].ToString();
                    }
                    rec_curr_cd = dt_apastran.Rows[0]["rec_Curr_Cd"].ToString();
                    rec_acctCurr_Cd = dt_apastran.Rows[0]["rec_AcctCurr_Cd"].ToString();
                    rec_Applied_Rate = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Applied_Rate"].ToString());
                    rec_RefNum_Ours = dt_apastran.Rows[0]["rec_RefNum_Ours"].ToString();
                    rec_Benf_Name = dt_apastran.Rows[0]["rec_Benf_Name"].ToString();
                    beneaddress1 = dt_apastran.Rows[0]["rec_Benf_Adr1"].ToString();
                    beneaddress2 = dt_apastran.Rows[0]["rec_Benf_Adr2"].ToString();
                    beneaddress3 = dt_apastran.Rows[0]["rec_Benf_Adr3"].ToString();
                    rec_Spl_Mesg = dt_apastran.Rows[0]["rec_Spl_Mesg"].ToString();
                    rec_Rem_Name = dt_apastran.Rows[0]["rec_Rem_Name"].ToString();
                    rec_Total_Chrg = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Total_Chrg"].ToString());
                    rec_Net_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Peso"].ToString());
                    rec_Net_Dollar = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Dollar"].ToString());
                    tranxdate = dt_apastran.Rows[0]["rec_Tran_Date"].ToString();
                    apas_acct_no = dt_apastran.Rows[0]["rec_Acct_No"].ToString();
                    accounttype = dt_apastran.Rows[0]["rec_Acct_Type1"].ToString();
                    remittingbank = dt_apastran.Rows[0]["rec_RB_Name"].ToString();
                    rec_Gross_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Gross_Peso"].ToString());
                    currentDate = dt_apastran.Rows[0]["rec_Date_Received"].ToString();
                    rec_Payee_Benf = dt_apastran.Rows[0]["rec_Payee_Benf"].ToString();
                    rec_Orig_BrCd = dt_apastran.Rows[0]["rec_Orig_BrCd"].ToString();
                    remaddress1 = dt_apastran.Rows[0]["rec_Rem_Add1"].ToString();
                    remaddress2 = dt_apastran.Rows[0]["rec_Rem_Add2"].ToString();
                    remaddress3 = dt_apastran.Rows[0]["rec_Rem_Add3"].ToString();
                    rec_Benef_ContactNo_value = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_Benef_ContactNo = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_RemitterID = dt_apastran.Rows[0]["rec_RemitterID"].ToString();
                    rec_NSVS_CASA_Acct = dt_apastran.Rows[0]["rec_NSVS_CASA_Acct"].ToString();
                    transactionpurpose = dt_apastran.Rows[0]["rec_RemPurpose"].ToString();
                    rec_BeneFName = dt_apastran.Rows[0]["rec_BeneFName"].ToString();
                    rec_BeneLName = dt_apastran.Rows[0]["rec_BeneLName"].ToString();
                    rec_BeneMName = dt_apastran.Rows[0]["rec_BeneMName"].ToString();
                    rec_RemFName = dt_apastran.Rows[0]["rec_RemFName"].ToString();
                    rec_RemLName = dt_apastran.Rows[0]["rec_RemLName"].ToString();
                    rec_RemMName = dt_apastran.Rows[0]["rec_RemMName"].ToString();
                    subbranchcode = dt_apastran.Rows[0]["rec_SubBrCode"].ToString();

                    //Check if APAS switch is enabled
                    dt_irswapas = irsw_commons.execStoredProcedureTable("isp_getlookup", new object[] { "EnableIRSWAPAS" });
                    if (dt_irswapas != null && dt_irswapas.Rows.Count > 0)
                    {
                        apas_enabled = Convert.ToInt32(dt_irswapas.Rows[0]["FldCode"].ToString());
                    }

                    ///APAS switch enabled
                    if (apas_enabled > 0)
                    {
                        ///Save query log information to IRSW DB
                        audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Save records to APAS DB tbl_APTrans", "proc_process_irswapas=" + tranxID.ToString() + ",2~" +
                                                                                                                                                referenceno + "~" +
                                                                                                                                                rec_curr_cd + "~" +
                                                                                                                                                rec_acctCurr_Cd + "~" +
                                                                                                                                                rec_Applied_Rate.ToString() + "~" +
                                                                                                                                                rec_RefNum_Ours + "~" +
                                                                                                                                                "0" + "~" +
                                                                                                                                                "170" + "~" +
                                                                                                                                                "ORD" + "~" +
                                                                                                                                                rec_Benf_Name + "~" +
                                                                                                                                                beneaddress1 + "~" +
                                                                                                                                                beneaddress2 + "~" +
                                                                                                                                                beneaddress3 + "~" +
                                                                                                                                                rec_Spl_Mesg + "~" +
                                                                                                                                                rec_Rem_Name + "~" +
                                                                                                                                                rec_Total_Chrg.ToString() + "~" +
                                                                                                                                                rec_Net_Peso.ToString() + "~" +
                                                                                                                                                rec_Net_Dollar.ToString() + "~" +
                                                                                                                                                tranxdate + "~" +
                                                                                                                                                apas_acct_no + "~" +
                                                                                                                                                accounttype + "~" +
                                                                                                                                                remittingbank + "~" +
                                                                                                                                                "0" + "~" +
                                                                                                                                                rec_Gross_Peso.ToString() + "~" +
                                                                                                                                                currentDate + "~" +
                                                                                                                                                rec_Payee_Benf + "~" +
                                                                                                                                                System.DateTime.Now.ToString() + "~" +
                                                                                                                                                rec_Orig_BrCd + "~" +
                                                                                                                                                remaddress1 + "~" +
                                                                                                                                                remaddress2 + "~" +
                                                                                                                                                remaddress3 + "~" +
                                                                                                                                                "" + "~" +
                                                                                                                                                rec_Benef_ContactNo_value + "~" +
                                                                                                                                                rec_RemitterID + "~" +
                                                                                                                                                "APAS" + "~" +
                                                                                                                                                rec_NSVS_CASA_Acct + "~" +
                                                                                                                                                transactionpurpose + "~" +
                                                                                                                                                rec_BeneFName + "~" +
                                                                                                                                                rec_BeneLName + "~" +
                                                                                                                                                rec_BeneMName + "~" +
                                                                                                                                                rec_RemFName + "~" +
                                                                                                                                                rec_RemLName + "~" +
                                                                                                                                                rec_RemMName + "~" +
                                                                                                                                                subbranchcode, "", csAuditLog.Category.Insert);

                        if (this.getCheckAPASTxnIfExistsCentral(referenceno) == false)
                        {
                            ///Save IRSW APAS transaction to IRSW-APAS DB  
                            dt_apas = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 2, error_message });

                            if (dt_apas != null && dt_apas.Rows.Count > 0)
                            {
                                bool_master = true;

                                ///Save log information to IRSW DB
                                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTrans", "rec_TrxnRef_Num:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Save records to IRSW DB tbl_APTranDetails", "proc_process_irswapas=" + tranxID.ToString() + ", 3~" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", csAuditLog.Category.Insert);

                                if (this.getCheckAPASTxnDetailsIfExistsCentral(referenceno) == false)
                                {
                                    ///Save IRSW APAS Tran Details to IRSW-APAS DB
                                    dt_apas_details = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 3, "" });

                                    if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                                    {
                                        bool_details = true;
                                        audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                                    }
                                    else
                                    {
                                        bool_details = false;
                                        audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Failed to Load in IRSW DB tbl_APTranDetails : " + irsw_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                                    }
                                }
                                else
                                {
                                    bool_details = true;
                                    audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Transaction already existing in IRSW DB tbl_APTranDetails", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                                }
                            }
                            else
                            {
                                bool_master = false;
                                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Failed to Load in IRSW DB tbl_APTrans : " + irsw_commons.Message, "rec_TrxnRef_Num:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                            }
                        }
                        else
                        {
                            bool_master = true;
                            audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Transaction already existing in IRSW DB tbl_APTrans", "rec_TrxnRef_Num:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                            audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Save records to IRSW DB tbl_APTranDetails", "proc_process_irswapas=" + tranxID.ToString() + ", 3~" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", csAuditLog.Category.Insert);

                            if (this.getCheckAPASTxnDetailsIfExistsCentral(referenceno) == false)
                            {
                                ///Save IRSW APAS Tran Details to IRSW-APAS DB
                                dt_apas_details = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 3, "" });

                                if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                                {
                                    bool_details = true;
                                    audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                                }
                                else
                                {
                                    bool_details = false;
                                    audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Failed to Load in IRSW DB tbl_APTranDetails : " + irsw_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                                }
                            }
                            else
                            {
                                bool_details = true;
                                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Transaction already existing in IRSW DB tbl_APTranDetails", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), csAuditLog.Category.Insert);
                            }
                        }

                        if (bool_master == true && bool_details == true)
                        {
                            audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTrans & tbl_APTranDetails retry central ok", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                            return true;
                        }
                        else
                        {
                            audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Failed to Load in IRSW DB retry central failed master and details apas table", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                            return false;
                        }
                    }
                    //================================================================
                    else
                    {
                        base.constring_suffix = "ConnectionString";
                        this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_trxnref_num='-1'");

                        if (this.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            if (remittingbank == "A06")
                            {
                                referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                            }
                            else
                            {
                                referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                            }

                            uprow["rec_TrxnRef_Num"] = referenceno;
                            uprow["rec_Curr_Cd"] = rec_curr_cd;
                            uprow["rec_AcctCurr_Cd"] = rec_acctCurr_Cd;
                            uprow["rec_Applied_Rate"] = rec_Applied_Rate;
                            uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                            uprow["rec_Amount_FD"] = 0;
                            uprow["rec_PNBr_CD"] = 170;
                            uprow["rec_PNBr_Name"] = "ORD";
                            uprow["rec_Benf_Name"] = rec_Benf_Name;
                            uprow["rec_Benf_Adr1"] = beneaddress1;
                            uprow["rec_Benf_Adr2"] = beneaddress2;
                            uprow["rec_Benf_Adr3"] = beneaddress3;
                            uprow["rec_Spl_Mesg"] = rec_Spl_Mesg;
                            uprow["rec_Rem_Name"] = rec_Rem_Name;
                            uprow["rec_Total_Chrg"] = rec_Total_Chrg;
                            uprow["rec_Net_Peso"] = rec_Net_Peso;
                            uprow["rec_Net_Dollar"] = rec_Net_Dollar;
                            uprow["rec_Tran_Date"] = tranxdate;
                            uprow["rec_Acct_No"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'APAS_Online' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
                            uprow["rec_Acct_Type1"] = accounttype;
                            uprow["rec_RB_Name"] = remittingbank;
                            uprow["rec_Pay_Mode"] = 0;
                            uprow["rec_Gross_Peso"] = rec_Gross_Peso;
                            uprow["rec_Date_Received"] = currentDate;
                            uprow["rec_Payee_Benf"] = rec_Payee_Benf;
                            uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                            uprow["rec_Orig_BrCd"] = System.Web.HttpContext.Current.Session["NegBranchCode"];
                            uprow["rec_Rem_Add1"] = remaddress1;
                            uprow["rec_Rem_Add2"] = remaddress2;
                            uprow["rec_Rem_Add3"] = remaddress3;
                            uprow["rec_Benef_MobileNo"] = "";
                            uprow["rec_Benef_ContactNo"] = (rec_Benef_ContactNo.Length > 20) ? rec_Benef_ContactNo.Substring(0, 20) : rec_Benef_ContactNo; //Convert.ToString(this.execScalar("Select TOP 1 ContactNo from tbl_cif_contacts where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                            uprow["rec_RemitterID"] = rec_RemitterID;
                            uprow["rec_ProdCode"] = "APAS";
                            uprow["rec_NSVS_CASA_Acct"] = rec_NSVS_CASA_Acct;
                            uprow["rec_RemPurpose"] = transactionpurpose;
                            uprow["rec_BeneFName"] = rec_BeneFName;
                            uprow["rec_BeneLName"] = rec_BeneLName;
                            uprow["rec_BeneMName"] = rec_BeneMName;
                            uprow["rec_RemFName"] = rec_RemFName;
                            uprow["rec_RemLName"] = rec_RemLName;
                            uprow["rec_RemMName"] = rec_RemMName;
                            uprow["rec_SubBrCode"] = subbranchcode;

                            // check if referenceno is already existing in APAS
                            if (this.getCheckAPASTxnIfExistsCentral(dtRemittance.Rows[0]["ReferenceNo"].ToString()) == false)
                            {
                                bool_master = this.save(sqlHelper.RETAIN_DATASET);
                                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Successfully Loaded in APAS Central DB tbl_APTrans retry central", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                            }
                            // end check if referenceno is already existing in APAS
                        }

                        /*Transaction Details*/
                        this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_trxnRefNum='-1'");

                        if (this.getCheckAPASTxnIfExistsCentral(dtRemittance.Rows[0]["ReferenceNo"].ToString()) == false)
                        {
                            if (this.newRow(sqlHelper.RETAIN_DATASET))
                            {
                                uprow["rec_TrxnRefNum"] = referenceno;
                                uprow["rec_TrandInd"] = "NP";
                                uprow["rec_lockind"] = "No";
                                uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                                uprow["rec_Tran_Date"] = tranxdate;
                            }

                            bool_details = this.save(sqlHelper.RETAIN_DATASET);

                            if (bool_master == true && bool_details == true)
                            {
                                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralServer", "Successfully Loaded in APASCentral DB tbl_APTrans & tbl_APTranDetails retry central.", "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                                return true;
                            }
                            else
                            {
                                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralServer", "Failed to Load in APASCentral DB retry central: " + base.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);
                                return false;
                            }
                        }
                    }
                }
                else
                {
                    ///================================================================
                    ///Revise saving IRSW APAS transactions to APAS DB 02182023
                    audit.writeAudit("irsOldAPAS", "Uploading.APASCentralSever", "Unable to retrieve record from IRSW DB : " + irsw_commons.Message, "IRSW SEQ_ID:" + tranxID.ToString(), "", csAuditLog.Category.Select);
                    return false;
                    ///================================================================
                }
                /*Transaction Details END*/

                return false;

            }
            catch (Exception ex)
            {
                this.APASMessage = "Error encountered while saving APAS Transaction " + ex.Message;
                audit.writeAudit("irsOldAPAS", "Uploading.APASCentralServer", "Failed to Load in APASCentral DB retry central: " + ex.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", csAuditLog.Category.Insert);

                return false;
                //InsertAPASCentral(tranxID);
            }
            finally
            {
                irsw_commons = null;
            }       
        }
        #endregion

        private DataTable GetRemittance(int seq_id)
        {
            DataTable dt = null;

            try
            {
                dt = this.getDataSet("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + seq_id.ToString(), false, new object[] { }).Tables[0]; return dt;
            }
            catch
            {
                return null;
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

        private string msg = "";
        public string APASMessage
        {
            get { return msg; }
            set { msg = value; }
        }
        private bool getCheckAPASTxnIfExists(String referenceno)
        {   
            /* MODIFIED BY BOYET FEB 6 2017 TO CORRECT PERFORMANCE ISSUE WHEN CONNECTING TO DATABASE */
            DataTable dt = null;

            try
            {
                base.constring_suffix = "ConnectionStringAPAS";

                dt = this.getDataSet("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false, new object[] { }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            /* MODIFIED BY BOYET FEB 6 2017 TO CORRECT PERFORMANCE ISSUE WHEN CONNECTING TO DATABASE */

            /* ORIGINAL CODE COMMENTED BY BOYET FEB 6 2017
            base.constring_suffix = "ConnectionStringAPAS";
            DataTable dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];
            if (dt.Rows.Count > 0) { return true; }
            return false;
            */
        }

        private bool getCheckAPASTxnIfExistsCentral(String referenceno)
        {
            /* MODIFIED BY BOYET FEB 6 2017 TO CORRECT PERFORMANCE ISSUE WHEN CONNECTING TO DATABASE */
            DataTable dt = null;

            try
            {
                dt = this.getDataSet("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false, new object[] { }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            /* MODIFIED BY BOYET FEB 6 2017 TO CORRECT PERFORMANCE ISSUE WHEN CONNECTING TO DATABASE */

            /*
            DataTable dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];
            if (dt.Rows.Count > 0) { return true; }
            return false;
             */ 
        }

        private bool getCheckAPASTxnDetailsIfExistsCentral(String referenceno)
        {
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */
            DataTable dt = null;

            try
            {
                dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTranDetails where rec_TrxnRefNum='" + referenceno + "'", false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */

            /*
            DataTable dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];
            if (dt.Rows.Count > 0) { return true; }
            return false;
             */

        }
    }

    public class irsOldAPASWinApp : sqlHelper
    {
        WinAppLogger auditWinApp = new WinAppLogger();

        #region FOR PAW

        public string[] set_rem_ben_adress(string rem_adr_seq, string ben_adr_seq)
        {
            /* ADDED BY BOYET JAN 30 2017 TO GET THE COMPLETE ADDRESS OF CLIENT */
            DataTable dt = null;
            DataRow[] dr = null;
            SqlCommand sq = null;

            string rem_adr1 = string.Empty;
            string rem_adr2 = string.Empty;
            string rem_adr3 = string.Empty;

            string ben_adr1 = string.Empty;
            string ben_adr2 = string.Empty;
            string ben_adr3 = string.Empty;

            long rem_adr_seq_1 = 0;
            long ben_adr_seq_1 = 0;

            try
            {
                base.EnforceConstraint = false;

                if (string.IsNullOrEmpty(rem_adr_seq) == false)
                {
                    rem_adr_seq_1 = Convert.ToInt32(rem_adr_seq);
                }

                if (string.IsNullOrEmpty(ben_adr_seq) == false)
                {
                    ben_adr_seq_1 = Convert.ToInt32(ben_adr_seq);
                }

                sq = getSqlCommand("irsw_to_apas_client_addr");

                if (sq != null)
                {
                    sq.Parameters.Add("@remitter_cifid_seq_id", SqlDbType.BigInt).Value = rem_adr_seq_1;
                    sq.Parameters.Add("@beneficiary_cifid_seq_id", SqlDbType.BigInt).Value = ben_adr_seq_1;
                }

                dt = getDataSet(sq, "TBL_CIF_ADDRESS").Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    dr = dt.Select("client_flag = 'REM'");

                    if (dr != null && dr.Length > 0)
                    {
                        rem_adr1 = (string)dr[0]["address1"];
                        rem_adr1 += " " + (string)dr[0]["address2"];
                        rem_adr1 += " " + (string)dr[0]["address3"];

                        rem_adr2 = (string)dr[0]["address4"];
                        rem_adr2 += " " + (string)dr[0]["address5"];
                        rem_adr2 += " " + (string)dr[0]["address6"];

                        rem_adr3 = (string)dr[0]["municipality"];
                        rem_adr3 += " " + (string)dr[0]["province"];

                        if (string.IsNullOrEmpty(rem_adr1).Equals(false) && rem_adr1.Length > 100)
                        {
                            rem_adr1 = rem_adr1.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(rem_adr2).Equals(false) && rem_adr2.Length > 100)
                        {
                            rem_adr2 = rem_adr2.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(rem_adr3).Equals(false) && rem_adr3.Length > 100)
                        {
                            rem_adr3 = rem_adr3.Substring(0, 100);
                        }
                    }

                    dr = null;
                    dr = dt.Select("client_flag = 'BEN'");

                    if (dr != null && dr.Length > 0)
                    {
                        ben_adr1 = (string)dr[0]["address1"];
                        ben_adr1 += " " + (string)dr[0]["address2"];
                        ben_adr1 += " " + (string)dr[0]["address3"];

                        ben_adr2 = (string)dr[0]["address4"];
                        ben_adr2 += " " + (string)dr[0]["address5"];
                        ben_adr2 += " " + (string)dr[0]["address6"];

                        ben_adr3 = (string)dr[0]["municipality"];
                        ben_adr3 += " " + (string)dr[0]["province"];

                        if (string.IsNullOrEmpty(ben_adr1).Equals(false) && ben_adr1.Length > 100)
                        {
                            ben_adr1 = ben_adr1.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(ben_adr2).Equals(false) && ben_adr2.Length > 100)
                        {
                            ben_adr2 = ben_adr2.Substring(0, 100);
                        }

                        if (string.IsNullOrEmpty(ben_adr3).Equals(false) && ben_adr3.Length > 100)
                        {
                            ben_adr3 = ben_adr3.Substring(0, 100);
                        }
                    }
                }

                return new string[] { rem_adr1.Trim(), rem_adr2.Trim(), rem_adr3.Trim(), ben_adr1.Trim(), ben_adr2.Trim(), ben_adr3.Trim() };
            }
            catch
            {
                return new string[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                if (sq != null)
                {
                    sq.Dispose();
                    sq = null;
                }

                dr = null;
            }
        }

        /*This will insert PAW in APAS Server*/
        public bool InsertAnPAW(int tranxID)
        {
            base.constring_suffix = "ConnectionString";

            string tranxdate;
            string currentDate;
            string remittingbank;
            DataTable dtRemittance;
            string accountno = "";
            string accounttype = "";

            dtRemittance = GetRemittance(tranxID);

            if (this.getCheckAPASTxnIfExists(dtRemittance.Rows[0]["ReferenceNo"].ToString())) { return true; } // check if referenceno is already existing in APAS

            base.constring_suffix = "ConnectionString";

            string branchcode = dtRemittance.Rows[0]["RemittingBranch"].ToString();
            string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);

            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017*/
            string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";
            string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            {
                remaddress1 = rem_ben_adrs[0];
                remaddress2 = rem_ben_adrs[1];
                remaddress3 = rem_ben_adrs[2];

                beneaddress1 = rem_ben_adrs[3];
                beneaddress2 = rem_ben_adrs[4];
                beneaddress3 = rem_ben_adrs[5];
            }
            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */

            int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            if (dtAccountInfo.Rows.Count > 0)
            {
                accountno = dtAccountInfo.Rows[0]["AccountNo"].ToString();
                accounttype = dtAccountInfo.Rows[0]["AccountType"].ToString();
            }

            /* variables */
            string referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            string rec_curr_cd = Convert.ToString(this.execScalar("SELECT fldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
            string rec_acctCurr_Cd = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
            decimal rec_Applied_Rate = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"].ToString());
            string rec_RefNum_Ours = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);

            string rec_Benf_Name = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_Spl_Mesg = dtRemittance.Rows[0]["SpecialMessage"].ToString();
            string rec_Rem_Name = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            decimal rec_Total_Chrg = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
            decimal rec_Net_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            decimal rec_Net_Dollar = Convert.ToDecimal(dtRemittance.Rows[0]["NetRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexLocalToUSD"]);
            decimal rec_Gross_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
            string rec_Payee_Benf = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_Orig_BrCd = Convert.ToString(this.execScalar("Select NegBranchCode From TBL_Branches WHERE Branchcode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));

            
            string rec_Benef_ContactNo = Convert.ToString(this.execScalar("SELECT	ISNULL(ContactNo, '') FROM	TBL_CIF_contacts	cif	inner join  TBL_REMITTANCE	rem	on	cif.seq_id = rem.BeneficiaryContactSeqID WHERE	rem.seq_id = " + tranxID.ToString()));

            string rec_RemitterID = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
            string rec_NSVS_CASA_Acct = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));
            string rec_BeneFName = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_BeneLName = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_BeneMName = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            string rec_RemFName = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            string rec_RemMName = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            string rec_RemLName = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            string rec_TrxnRefNum = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            decimal rec_NetToBene = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);

            try
            {
                //change connection string
                base.constring_suffix = "ConnectionStringAPAS";

                this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_TrxnRef_Num='-1'");
                if (this.newRow(sqlHelper.RETAIN_DATASET))
                {
                    uprow["rec_TrxnRef_Num"] = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                    uprow["rec_Curr_Cd"] = rec_curr_cd;
                    uprow["rec_AcctCurr_Cd"] = rec_acctCurr_Cd;
                    uprow["rec_Applied_Rate"] = rec_Applied_Rate;
                    uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                    uprow["rec_Amount_FD"] = 0;
                    uprow["rec_PNBr_CD"] = 170;
                    uprow["rec_PNBr_Name"] = "ORD";
                    uprow["rec_Benf_Name"] = rec_Benf_Name;
                    uprow["rec_Benf_Adr1"] = beneaddress1;
                    uprow["rec_Benf_Adr2"] = beneaddress2;
                    uprow["rec_Benf_Adr3"] = beneaddress3;
                    uprow["rec_Spl_Mesg"] = rec_Spl_Mesg;
                    uprow["rec_Rem_Name"] = rec_Rem_Name;
                    uprow["rec_Total_Chrg"] = rec_Total_Chrg;
                    uprow["rec_Net_Peso"] = rec_Net_Peso;
                    uprow["rec_Net_Dollar"] = rec_Net_Dollar;
                    uprow["rec_Tran_Date"] = tranxdate;
                    uprow["rec_Acct_No"] = accountno;
                    uprow["rec_Acct_Type1"] = accounttype;
                    uprow["rec_RB_Name"] = remittingbank;
                    uprow["rec_Pay_Mode"] = 0;
                    uprow["rec_Gross_Peso"] = rec_Gross_Peso;
                    uprow["rec_Date_Received"] = currentDate;
                    uprow["rec_Payee_Benf"] = rec_Payee_Benf;
                    uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                    uprow["rec_Orig_BrCd"] = branchcode;
                    uprow["rec_Rem_Add1"] = remaddress1;
                    uprow["rec_Rem_Add2"] = remaddress2;
                    uprow["rec_Rem_Add3"] = remaddress3;
                    uprow["rec_Benef_MobileNo"] = "";
                    uprow["rec_Benef_ContactNo"] = rec_Benef_ContactNo;
                    uprow["rec_RemitterID"] = rec_RemitterID;
                    uprow["rec_ProdCode"] = "PAW";
                    uprow["rec_NSVS_CASA_Acct"] = rec_NSVS_CASA_Acct;
                    uprow["rec_RemPurpose"] = transactionpurpose;
                    uprow["rec_BeneFName"] = rec_BeneFName;
                    uprow["rec_BeneLName"] = rec_BeneLName;
                    uprow["rec_BeneMName"] = rec_BeneMName;
                    uprow["rec_RemFName"] = rec_RemFName;
                    uprow["rec_RemLName"] = rec_RemLName;
                    uprow["rec_RemMName"] = rec_RemMName;
                    uprow["rec_SubBrCode"] = subbranchcode;
                    base.constring_suffix = "ConnectionString";
                    this.save(sqlHelper.RETAIN_DATASET);
                }
                /*Transaction Details*/
                this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_TrxnRefNum='-1'");
                if (this.newRow(sqlHelper.RETAIN_DATASET))
                {
                    uprow["rec_TrxnRefNum"] = referenceno;
                    uprow["rec_TrandInd"] = "NP";
                    uprow["rec_lockind"] = "No";
                    uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                    uprow["rec_Tran_Date"] = tranxdate;
                    this.save(sqlHelper.RETAIN_DATASET);
                    return true;
                }
                /*Transaction Details END*/

            }
            catch //(Exception ex) // commented to remove warning
            {
                return InsertAnPAWCentral(tranxID);

            }

            return false;
        }

        private bool InsertAnPAWCentral(int tranxID)
        {
            base.constring_suffix = "ConnectionString";
            string tranxdate;
            string currentDate;
            string remittingbank;
            DataTable dtRemittance;
            string accountno = "";
            string accounttype = "";


            dtRemittance = GetRemittance(tranxID);
            if (this.getCheckAPASTxnIfExistsCentral(dtRemittance.Rows[0]["ReferenceNo"].ToString())) { return true; } // check if referenceno is already existing in APAS

            string branchcode = dtRemittance.Rows[0]["RemittingBranch"].ToString();
            string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);

            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017*/
            string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";

            string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            {
                remaddress1 = rem_ben_adrs[0];
                remaddress2 = rem_ben_adrs[1];
                remaddress3 = rem_ben_adrs[2];

                beneaddress1 = rem_ben_adrs[3];
                beneaddress2 = rem_ben_adrs[4];
                beneaddress3 = rem_ben_adrs[5];
            }
            /* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */

            int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            if (dtAccountInfo.Rows.Count > 0)
            {
                accountno = dtAccountInfo.Rows[0]["AccountNo"].ToString();
                accounttype = dtAccountInfo.Rows[0]["AccountType"].ToString();
            }
            
            string rec_Benef_ContactNo = Convert.ToString(this.execScalar("SELECT	ISNULL(ContactNo, '') FROM	TBL_CIF_contacts	cif	inner join  TBL_REMITTANCE	rem	on	cif.seq_id = rem.BeneficiaryContactSeqID WHERE	rem.seq_id = " + tranxID.ToString()));


            //begin save
            this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_TrxnRef_Num='-1'");

            try
            {
                if (this.newRow(sqlHelper.RETAIN_DATASET))
                {
                    uprow["rec_TrxnRef_Num"] = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                    uprow["rec_Curr_Cd"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
                    uprow["rec_AcctCurr_Cd"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
                    uprow["rec_Applied_Rate"] = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"] + "");
                    uprow["rec_RefNum_Ours"] = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);
                    uprow["rec_Amount_FD"] = 0;
                    uprow["rec_PNBr_CD"] = 170;
                    uprow["rec_PNBr_Name"] = "ORD";
                    uprow["rec_Benf_Name"] = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_Benf_Adr1"] = beneaddress1.Substring(0, 25);
                    uprow["rec_Benf_Adr2"] = beneaddress2.Substring(0, 25);
                    uprow["rec_Benf_Adr3"] = beneaddress3.Substring(0, 25);
                    uprow["rec_Spl_Mesg"] = dtRemittance.Rows[0]["SpecialMessage"].ToString();
                    uprow["rec_Rem_Name"] = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_Total_Chrg"] = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
                    uprow["rec_Net_Peso"] = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
                    uprow["rec_Net_Dollar"] = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
                    uprow["rec_Tran_Date"] = tranxdate;
                    uprow["rec_Acct_No"] = accountno;
                    uprow["rec_Acct_Type1"] = accounttype;
                    uprow["rec_RB_Name"] = remittingbank;
                    uprow["rec_Pay_Mode"] = 0;
                    uprow["rec_Gross_Peso"] = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
                    uprow["rec_Date_Received"] = currentDate;
                    uprow["rec_Payee_Benf"] = Convert.ToString(this.execScalar("Select fullname from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                    uprow["rec_Orig_BrCd"] = branchcode;
                    uprow["rec_Rem_Add1"] = remaddress1;
                    uprow["rec_Rem_Add2"] = remaddress2;
                    uprow["rec_Rem_Add3"] = remaddress3;
                    uprow["rec_Benef_MobileNo"] = "";

                    uprow["rec_Benef_ContactNo"] = rec_Benef_ContactNo;

                    uprow["rec_RemitterID"] = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
                    uprow["rec_ProdCode"] = "PAW";
                    uprow["rec_NSVS_CASA_Acct"] = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));
                    uprow["rec_RemPurpose"] = transactionpurpose;
                    uprow["rec_BeneFName"] = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_BeneLName"] = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_BeneMName"] = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                    uprow["rec_RemFName"] = Convert.ToString(this.execScalar("Select FirstName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_RemLName"] = Convert.ToString(this.execScalar("Select LastName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_RemMName"] = Convert.ToString(this.execScalar("Select MiddleName from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
                    uprow["rec_SubBrCode"] = subbranchcode;
                    this.save(sqlHelper.RETAIN_DATASET);

                }
                /*Transaction Details*/
                this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_TrxnRefNum='-1'");
                if (this.newRow(sqlHelper.RETAIN_DATASET))
                {
                    uprow["rec_TrxnRefNum"] = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                    uprow["rec_TrandInd"] = "NP";
                    uprow["rec_lockind"] = "No";
                    uprow["rec_RefNum_Ours"] = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);
                    uprow["rec_Tran_Date"] = tranxdate;
                    this.save(sqlHelper.RETAIN_DATASET);
                    return true;
                }
                /*Transaction Details END*/
            }
            catch (Exception ex)
            {
                APASMessage = "Error encountered while saving PAW Transaction " + ex.Message;
                return false;
                //InsertAPASCentral(tranxID);
            }

            return false;
        }
        #endregion

        #region FOR APAS

        ///================================================================
        ///Revise saving IRSW APAS transactions to APAS DB 02182023
        /*This will insert Failed APAS transaction in APAS Server*/
        public void InsertAPAS_Agent()
        {
            irsSqlCommons irsw_commons = new irsSqlCommons();
            irsSqlCommons apas_commons = new irsSqlCommons();

            irsw_commons.constring_suffix = "ConnectionString";
            apas_commons.constring_suffix = "ConnectionStringAPAS";
            DataTable dt_apastran = new DataTable();
            DataTable dt_apastrandetails = new DataTable();
            DataTable dt_apas = new DataTable();
            DataTable dt_apas_details = new DataTable();
            DataTable dt_irswapas = new DataTable();
            int apas_enabled = 0;

            string rec_curr_cd = "";
            string rec_acctCurr_Cd = "";
            decimal rec_Applied_Rate = 0;
            string rec_RefNum_Ours = "";
            string rec_Benf_Name = "";
            string rec_Spl_Mesg = "";
            string rec_Rem_Name = "";
            decimal rec_Total_Chrg = 0;
            decimal rec_Net_Peso = 0;
            decimal rec_Net_Dollar = 0;
            decimal rec_Gross_Peso = 0;
            string rec_Payee_Benf = "";
            string rec_RemitterID = "";
            string rec_NSVS_CASA_Acct = "";
            string rec_BeneFName = "";
            string rec_BeneLName = "";
            string rec_BeneMName = "";
            string rec_RemFName = "";
            string rec_RemMName = "";
            string rec_RemLName = "";
            string referenceno = "";
            string remaddress1 = "";
            string remaddress2 = "";
            string remaddress3 = "";
            string beneaddress1 = "";
            string beneaddress2 = "";
            string beneaddress3 = "";
            string tranxdate = "";
            string currentDate = "";
            string remittingbank = "";
            string accounttype = "";
            string apas_acct_no = "";
            string branchcode = "";
            string rec_Benef_ContactNo_value = "";
            string transactionpurpose = "";
            string subbranchcode = "";
            int i_apas_retval = 0;
            int i_apasdetails_retval = 0;
            int i_counter = 0;

            try
            {
                //Check if APAS switch is enabled
                dt_irswapas = irsw_commons.execStoredProcedureTable("isp_getlookup", new object[] { "EnableIRSWAPAS" });
                if (dt_irswapas != null && dt_irswapas.Rows.Count > 0)
                {
                    apas_enabled = Convert.ToInt32(dt_irswapas.Rows[0]["FldCode"].ToString());
                }

                ///APAS switch enabled
                if (apas_enabled > 0)
                {
                    ///GET IRSW APAS transaction to APAS DB  
                    auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Get record from IRSW DB", "proc_process_irswapas_agentsr=1,", "", WinAppLogger.Category.Insert);
                    dt_apastran = irsw_commons.execStoredProcedureTable("proc_process_irswapas_agentsr", new object[] { 1, "", "" });

                    if (dt_apastran != null && dt_apastran.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_apastran.Rows.Count; i++)
                        {
                            referenceno = dt_apastran.Rows[i]["rec_TrxnRef_Num"].ToString();
                            rec_curr_cd = dt_apastran.Rows[i]["rec_Curr_Cd"].ToString();
                            rec_acctCurr_Cd = dt_apastran.Rows[i]["rec_AcctCurr_Cd"].ToString();
                            rec_Applied_Rate = Convert.ToDecimal(dt_apastran.Rows[i]["rec_Applied_Rate"].ToString());
                            rec_RefNum_Ours = dt_apastran.Rows[i]["rec_RefNum_Ours"].ToString();
                            rec_Benf_Name = dt_apastran.Rows[i]["rec_Benf_Name"].ToString();
                            beneaddress1 = dt_apastran.Rows[i]["rec_Benf_Adr1"].ToString();
                            beneaddress2 = dt_apastran.Rows[i]["rec_Benf_Adr2"].ToString();
                            beneaddress3 = dt_apastran.Rows[i]["rec_Benf_Adr3"].ToString();
                            rec_Spl_Mesg = dt_apastran.Rows[i]["rec_Spl_Mesg"].ToString();
                            rec_Rem_Name = dt_apastran.Rows[i]["rec_Rem_Name"].ToString();
                            rec_Total_Chrg = Convert.ToDecimal(dt_apastran.Rows[i]["rec_Total_Chrg"].ToString());
                            rec_Net_Peso = Convert.ToDecimal(dt_apastran.Rows[i]["rec_Net_Peso"].ToString());
                            rec_Net_Dollar = Convert.ToDecimal(dt_apastran.Rows[i]["rec_Net_Dollar"].ToString());
                            tranxdate = dt_apastran.Rows[i]["rec_Tran_Date"].ToString();
                            apas_acct_no = dt_apastran.Rows[i]["rec_Acct_No"].ToString();
                            accounttype = dt_apastran.Rows[i]["rec_Acct_Type1"].ToString();
                            remittingbank = dt_apastran.Rows[i]["rec_RB_Name"].ToString();
                            rec_Gross_Peso = Convert.ToDecimal(dt_apastran.Rows[i]["rec_Gross_Peso"].ToString());
                            currentDate = dt_apastran.Rows[i]["rec_Date_Received"].ToString();
                            rec_Payee_Benf = dt_apastran.Rows[i]["rec_Payee_Benf"].ToString();
                            branchcode = dt_apastran.Rows[i]["rec_Orig_BrCd"].ToString();
                            remaddress1 = dt_apastran.Rows[i]["rec_Rem_Add1"].ToString();
                            remaddress2 = dt_apastran.Rows[i]["rec_Rem_Add2"].ToString();
                            remaddress3 = dt_apastran.Rows[i]["rec_Rem_Add3"].ToString();
                            rec_Benef_ContactNo_value = dt_apastran.Rows[i]["rec_Benef_ContactNo"].ToString();
                            rec_RemitterID = dt_apastran.Rows[i]["rec_RemitterID"].ToString();
                            rec_NSVS_CASA_Acct = dt_apastran.Rows[i]["rec_NSVS_CASA_Acct"].ToString();
                            transactionpurpose = dt_apastran.Rows[i]["rec_RemPurpose"].ToString();
                            rec_BeneFName = dt_apastran.Rows[i]["rec_BeneFName"].ToString();
                            rec_BeneLName = dt_apastran.Rows[i]["rec_BeneLName"].ToString();
                            rec_BeneMName = dt_apastran.Rows[i]["rec_BeneMName"].ToString();
                            rec_RemFName = dt_apastran.Rows[i]["rec_RemFName"].ToString();
                            rec_RemLName = dt_apastran.Rows[i]["rec_RemLName"].ToString();
                            rec_RemMName = dt_apastran.Rows[i]["rec_RemMName"].ToString();
                            subbranchcode = dt_apastran.Rows[i]["rec_SubBrCode"].ToString();

                            auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Save records to APAS DB tbl_APTrans", "proc_insert_apas_trxn=" + referenceno + "~" +
                                                                                                                                                    rec_curr_cd + "~" +
                                                                                                                                                    rec_acctCurr_Cd + "~" +
                                                                                                                                                    rec_Applied_Rate.ToString() + "~" +
                                                                                                                                                    rec_RefNum_Ours + "~" +
                                                                                                                                                    "0" + "~" +
                                                                                                                                                    "170" + "~" +
                                                                                                                                                    "ORD" + "~" +
                                                                                                                                                    rec_Benf_Name + "~" +
                                                                                                                                                    beneaddress1 + "~" +
                                                                                                                                                    beneaddress2 + "~" +
                                                                                                                                                    beneaddress3 + "~" +
                                                                                                                                                    rec_Spl_Mesg + "~" +
                                                                                                                                                    rec_Rem_Name + "~" +
                                                                                                                                                    rec_Total_Chrg.ToString() + "~" +
                                                                                                                                                    rec_Net_Peso.ToString() + "~" +
                                                                                                                                                    rec_Net_Dollar.ToString() + "~" +
                                                                                                                                                    tranxdate + "~" +
                                                                                                                                                    apas_acct_no + "~" +
                                                                                                                                                    accounttype + "~" +
                                                                                                                                                    remittingbank + "~" +
                                                                                                                                                    "0" + "~" +
                                                                                                                                                    rec_Gross_Peso.ToString() + "~" +
                                                                                                                                                    currentDate + "~" +
                                                                                                                                                    rec_Payee_Benf + "~" +
                                                                                                                                                    System.DateTime.Now.ToString() + "~" +
                                                                                                                                                    branchcode + "~" +
                                                                                                                                                    remaddress1 + "~" +
                                                                                                                                                    remaddress2 + "~" +
                                                                                                                                                    remaddress3 + "~" +
                                                                                                                                                    "" + "~" +
                                                                                                                                                    rec_Benef_ContactNo_value + "~" +
                                                                                                                                                    rec_RemitterID + "~" +
                                                                                                                                                    "APAS" + "~" +
                                                                                                                                                    rec_NSVS_CASA_Acct + "~" +
                                                                                                                                                    transactionpurpose + "~" +
                                                                                                                                                    rec_BeneFName + "~" +
                                                                                                                                                    rec_BeneLName + "~" +
                                                                                                                                                    rec_BeneMName + "~" +
                                                                                                                                                    rec_RemFName + "~" +
                                                                                                                                                    rec_RemLName + "~" +
                                                                                                                                                    rec_RemMName + "~" +
                                                                                                                                                    subbranchcode, "", WinAppLogger.Category.Insert);

                            ///Save IRSW APAS transaction to APAS DB  
                            dt_apas = apas_commons.execStoredProcedureTable("proc_insert_apas_trxn", new object[] { referenceno,
                                                                                                             rec_curr_cd,
                                                                                                             rec_acctCurr_Cd,
                                                                                                             rec_Applied_Rate,
                                                                                                             rec_RefNum_Ours,
                                                                                                             0,
                                                                                                             "170",
                                                                                                             "ORD",
                                                                                                             rec_Benf_Name,
                                                                                                             beneaddress1,
                                                                                                             beneaddress2,
                                                                                                             beneaddress3,
                                                                                                             rec_Spl_Mesg,
                                                                                                             rec_Rem_Name,
                                                                                                             rec_Total_Chrg,
                                                                                                             rec_Net_Peso,
                                                                                                             rec_Net_Dollar,
                                                                                                             tranxdate,
                                                                                                             apas_acct_no,
                                                                                                             accounttype,
                                                                                                             remittingbank,
                                                                                                             "0",
                                                                                                             rec_Gross_Peso,
                                                                                                             currentDate,
                                                                                                             rec_Payee_Benf,
                                                                                                             System.DateTime.Now.ToString(),
                                                                                                             branchcode,
                                                                                                             remaddress1,
                                                                                                             remaddress2,
                                                                                                             remaddress3,
                                                                                                             "",
                                                                                                             rec_Benef_ContactNo_value,
                                                                                                             rec_RemitterID,
                                                                                                             "APAS",
                                                                                                             rec_NSVS_CASA_Acct,
                                                                                                             transactionpurpose,
                                                                                                             rec_BeneFName,
                                                                                                             rec_BeneLName,
                                                                                                             rec_BeneMName,
                                                                                                             rec_RemFName,
                                                                                                             rec_RemLName,
                                                                                                             rec_RemMName,
                                                                                                             subbranchcode
                                                                                                        });

                            if (dt_apas != null && dt_apas.Rows.Count > 0)
                            {
                                ////Update sent to apas flag in IRSW DB APTrans table
                                i_apas_retval = irsw_commons.execStoredProcedureUpdate("proc_process_irswapas_agentsr", new object[] { 3, referenceno, "" });

                                ///Save log information to IRSW DB
                                auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans", "rec_TrxnRef_Num:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                                auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Save records to APAS DB tbl_APTranDetails", "proc_insert_apas_trxn_details=" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", WinAppLogger.Category.Insert);

                                ///Save IRSW APAS Tran Details to APAS DB
                                dt_apas_details = apas_commons.execStoredProcedureTable("proc_insert_apas_trxn_details", new object[] { referenceno, "NP", "No", rec_RefNum_Ours, tranxdate });

                                if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                                {
                                    ////Update sent to apas flag in IRSW DB APTranDetails table
                                    i_apasdetails_retval = irsw_commons.execStoredProcedureUpdate("proc_process_irswapas_agentsr", new object[] { 4, referenceno, "" });

                                    auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                                    auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans & tbl_APTranDetails.", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                }
                                else
                                {
                                    auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails : " + apas_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                }
                            }
                            else
                            {
                                auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTrans : " + apas_commons.Message, "rec_TrxnRef_Num:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails : " + apas_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            }
                        }
                    }
                    else
                    {
                        auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "No FAILED APAS transaction for resending in APAS Database for tbl_APTrans table", "", "", WinAppLogger.Category.Insert);
                    }

                    ///GET IRSW APAS Tran Details on which APTran details is already sent to APAS DB
                    auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Get record from IRSW DB", "proc_process_irswapas_agentsr=2,", "", WinAppLogger.Category.Insert);
                    dt_apastrandetails = irsw_commons.execStoredProcedureTable("proc_process_irswapas_agentsr", new object[] { 2, "", "" });

                    if (dt_apastrandetails != null && dt_apastrandetails.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt_apastrandetails.Rows.Count; i++)
                        {
                            referenceno = dt_apastrandetails.Rows[i]["rec_TrxnRefNum"].ToString();
                            rec_RefNum_Ours = dt_apastrandetails.Rows[i]["rec_RefNum_Ours"].ToString();
                            tranxdate = dt_apastrandetails.Rows[i]["rec_Tran_Date"].ToString();

                            auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Save records to APAS DB tbl_APTranDetails", "proc_insert_apas_trxn_details=" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", WinAppLogger.Category.Insert);

                            ///Save IRSW APAS Tran Details to APAS DB
                            dt_apas_details = apas_commons.execStoredProcedureTable("proc_insert_apas_trxn_details", new object[] { referenceno, "NP", "No", rec_RefNum_Ours, tranxdate });

                            if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                            {
                                ////Update sent to apas flag in IRSW DB APTranDetails table
                                i_apasdetails_retval = irsw_commons.execStoredProcedureUpdate("proc_process_irswapas_agentsr", new object[] { 4, referenceno, "" });

                                auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[i][0].ToString(), WinAppLogger.Category.Insert);
                                auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans & tbl_APTranDetails.", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            }
                            else
                            {
                                auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails : " + apas_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            }
                        }
                    }
                    else
                    {
                        auditWinApp.writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "No FAILED APAS transaction for resending in APAS Database for tbl_APTranDetails table", "", "", WinAppLogger.Category.Insert);
                    }
                }
            }
            catch (Exception ex)
            {
                i_counter = irsw_commons.execStoredProcedureUpdate("proc_process_irswapas_agentsr", new object[] { 5, referenceno, ex.Message.ToString().Trim() });
                new WinAppLogger().writeAudit("irsOldAPASWinAppAgent", "Uploading.APASSever", "Error saving APAS details to APAS DB via IRSWAgent: [ErrorMessage=" + ex.Message + "][StackTrace=" + ex.StackTrace + "]", "InsertAPAS_Agent", "rec_TrxnRef_Num:" + referenceno.ToString(), WinAppLogger.Category.Select);
            }
            finally
            {
                irsw_commons = null;
                apas_commons = null;
            }
        }
        ///================================================================

        /*This will insert APAS in APAS Server*/
        public bool InsertApas(int tranxID)
        {
            ///================================================================
            ///Revise saving IRSW APAS transactions to APAS DB 02182023
            irsSqlCommons irsw_commons = new irsSqlCommons();
            irsSqlCommons apas_commons = new irsSqlCommons();

            ////bool bool_master = false;
            ////bool bool_details = false;

            ////base.constring_suffix = "ConnectionString";
            ////string tranxdate;
            ////string currentDate;
            ////string remittingbank;
            ////DataTable dtRemittance;
            ////// commented to remove warning - string accountno = "";
            ////string accounttype = "";
            ////// commented to remove warning - bool ifSuccess;

            ////dtRemittance = GetRemittance(tranxID);

            ////if (this.getCheckAPASTxnIfExists(dtRemittance.Rows[0]["ReferenceNo"].ToString())) 
            ////{ 
            ////    return true; 
            ////} // check if referenceno is already existing in APAS

            ////base.constring_suffix = "ConnectionString";
            ////string branchcode = dtRemittance.Rows[0]["RemittingBranch"].ToString();
            ////string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            ////string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            ////remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            ////currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            ////tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);
            ////string rembank = Convert.ToString(dtRemittance.Rows[0]["RemittingBranch"]);

            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017*/
            ////string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            ////string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";

            ////string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            ////if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            ////{
            ////    remaddress1 = rem_ben_adrs[0];
            ////    remaddress2 = rem_ben_adrs[1];
            ////    remaddress3 = rem_ben_adrs[2];

            ////    beneaddress1 = rem_ben_adrs[3];
            ////    beneaddress2 = rem_ben_adrs[4];
            ////    beneaddress3 = rem_ben_adrs[5];
            ////}
            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */

            ////int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            ////DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("select * from tbl_lookup where fldgroup='APAS_Online'", false)).Tables[0];//this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            ////if (dtAccountInfo.Rows.Count > 0)
            ////{
            ////    accounttype = "";
            ////}

            /////* variables */
            ////string rec_curr_cd = Convert.ToString(this.execScalar("SELECT fldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
            ////string rec_acctCurr_Cd = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
            ////decimal rec_Applied_Rate = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"].ToString());
            ////string rec_RefNum_Ours = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);

            ////string rec_Benf_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Spl_Mesg = dtRemittance.Rows[0]["SpecialMessage"].ToString();
            ////string rec_Rem_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////decimal rec_Total_Chrg = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
            ////decimal rec_Net_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Net_Dollar = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Gross_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
            ////string rec_Payee_Benf = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Orig_BrCd = Convert.ToString(this.execScalar("Select NegBranchCode From TBL_Branches WHERE Branchcode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));

            ////string rec_Benef_ContactNo = Convert.ToString(this.execScalar("SELECT	ISNULL(ContactNo, '') FROM	TBL_CIF_contacts	cif	inner join  TBL_REMITTANCE	rem	on	cif.seq_id = rem.BeneficiaryContactSeqID WHERE	rem.seq_id = " + tranxID.ToString()));

            ////string rec_RemitterID = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
            ////string rec_NSVS_CASA_Acct = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));//dtRemittance.Rows[0]["DefAccountNo"].ToString(); //hindi ko alam
            ////string rec_BeneFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_RemFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_TrxnRefNum = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            ////decimal rec_NetToBene = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////string referenceno = string.Empty;

            DataTable dtRemittance;
            bool bool_master = false;
            bool bool_details = false;
            base.constring_suffix = "ConnectionString";

            string rec_curr_cd = "";
            string rec_acctCurr_Cd = "";
            decimal rec_Applied_Rate = 0;
            string rec_RefNum_Ours = "";
            string rec_Benf_Name = "";
            string rec_Spl_Mesg = "";
            string rec_Rem_Name = "";
            decimal rec_Total_Chrg = 0;
            decimal rec_Net_Peso = 0;
            decimal rec_Net_Dollar = 0;
            decimal rec_Gross_Peso = 0;
            string rec_Payee_Benf = "";
            string rec_RemitterID = "";
            string rec_NSVS_CASA_Acct = "";
            string rec_BeneFName = "";
            string rec_BeneLName = "";
            string rec_BeneMName = "";
            string rec_RemFName = "";
            string rec_RemMName = "";
            string rec_RemLName = "";
            string referenceno = "";
            string remaddress1 = "";
            string remaddress2 = "";
            string remaddress3 = "";
            string beneaddress1 = "";
            string beneaddress2 = "";
            string beneaddress3 = "";
            string tranxdate = "";
            string currentDate = "";
            string remittingbank = "";
            string accounttype = "";
            string apas_acct_no = "";
            string branchcode = "";
            string rec_Benef_ContactNo_value = "";
            string rec_Benef_ContactNo = "";
            string transactionpurpose = "";
            string subbranchcode = "";
            string rembank = "";
            ///================================================================

            try
            {
                ///================================================================
                ///Revise saving IRSW APAS transactions to APAS DB 02182023
                DataTable dt_apastran = new DataTable();
                DataTable dt_irswapas = new DataTable();
                DataTable dt_apas = new DataTable();
                DataTable dt_apas_details = new DataTable();
                int apas_enabled = 0;

                ///IRSWorld Connection string
                irsw_commons.constring_suffix = "ConnectionString";
                ///APAS Connection string
                apas_commons.constring_suffix = "ConnectionStringAPAS";

                dtRemittance = GetRemittance(tranxID);
                rembank = Convert.ToString(dtRemittance.Rows[0]["RemittingBranch"]);

                // check if referenceno is already existing in APAS
                if (this.getCheckAPASTxnIfExists(dtRemittance.Rows[0]["ReferenceNo"].ToString()))
                {
                    return true;
                }

                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Get record from IRSW DB", "proc_process_irswapas=" + tranxID.ToString() + ", 1", "", WinAppLogger.Category.Insert);
                dt_apastran = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 1, "" });

                if (dt_apastran != null && dt_apastran.Rows.Count > 0)
                {
                    ///Set APAS values from IRSW TBL_APTrans table
                    if (rembank == "A06")
                    {
                        referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                    }
                    else
                    {
                        referenceno = dt_apastran.Rows[0]["rec_TrxnRef_Num"].ToString();
                    }
                    rec_curr_cd = dt_apastran.Rows[0]["rec_Curr_Cd"].ToString();
                    rec_acctCurr_Cd = dt_apastran.Rows[0]["rec_AcctCurr_Cd"].ToString();
                    rec_Applied_Rate = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Applied_Rate"].ToString());
                    rec_RefNum_Ours = dt_apastran.Rows[0]["rec_RefNum_Ours"].ToString();
                    rec_Benf_Name = dt_apastran.Rows[0]["rec_Benf_Name"].ToString();
                    beneaddress1 = dt_apastran.Rows[0]["rec_Benf_Adr1"].ToString();
                    beneaddress2 = dt_apastran.Rows[0]["rec_Benf_Adr2"].ToString();
                    beneaddress3 = dt_apastran.Rows[0]["rec_Benf_Adr3"].ToString();
                    rec_Spl_Mesg = dt_apastran.Rows[0]["rec_Spl_Mesg"].ToString();
                    rec_Rem_Name = dt_apastran.Rows[0]["rec_Rem_Name"].ToString();
                    rec_Total_Chrg = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Total_Chrg"].ToString());
                    rec_Net_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Peso"].ToString());
                    rec_Net_Dollar = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Dollar"].ToString());
                    tranxdate = dt_apastran.Rows[0]["rec_Tran_Date"].ToString();
                    apas_acct_no = dt_apastran.Rows[0]["rec_Acct_No"].ToString();
                    accounttype = dt_apastran.Rows[0]["rec_Acct_Type1"].ToString();
                    remittingbank = dt_apastran.Rows[0]["rec_RB_Name"].ToString();
                    rec_Gross_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Gross_Peso"].ToString());
                    currentDate = dt_apastran.Rows[0]["rec_Date_Received"].ToString();
                    rec_Payee_Benf = dt_apastran.Rows[0]["rec_Payee_Benf"].ToString();
                    branchcode = dt_apastran.Rows[0]["rec_Orig_BrCd"].ToString();
                    remaddress1 = dt_apastran.Rows[0]["rec_Rem_Add1"].ToString();
                    remaddress2 = dt_apastran.Rows[0]["rec_Rem_Add2"].ToString();
                    remaddress3 = dt_apastran.Rows[0]["rec_Rem_Add3"].ToString();
                    rec_Benef_ContactNo_value = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_Benef_ContactNo = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_RemitterID = dt_apastran.Rows[0]["rec_RemitterID"].ToString();
                    rec_NSVS_CASA_Acct = dt_apastran.Rows[0]["rec_NSVS_CASA_Acct"].ToString();
                    transactionpurpose = dt_apastran.Rows[0]["rec_RemPurpose"].ToString();
                    rec_BeneFName = dt_apastran.Rows[0]["rec_BeneFName"].ToString();
                    rec_BeneLName = dt_apastran.Rows[0]["rec_BeneLName"].ToString();
                    rec_BeneMName = dt_apastran.Rows[0]["rec_BeneMName"].ToString();
                    rec_RemFName = dt_apastran.Rows[0]["rec_RemFName"].ToString();
                    rec_RemLName = dt_apastran.Rows[0]["rec_RemLName"].ToString();
                    rec_RemMName = dt_apastran.Rows[0]["rec_RemMName"].ToString();
                    subbranchcode = dt_apastran.Rows[0]["rec_SubBrCode"].ToString();

                    //Check if APAS switch is enabled
                    dt_irswapas = irsw_commons.execStoredProcedureTable("isp_getlookup", new object[] { "EnableIRSWAPAS" });
                    if (dt_irswapas != null && dt_irswapas.Rows.Count > 0)
                    {
                        apas_enabled = Convert.ToInt32(dt_irswapas.Rows[0]["FldCode"].ToString());
                    }

                    ///APAS switch enabled
                    if (apas_enabled > 0)
                    {
                        ///Save query log information to IRSW DB
                        auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Save records to APAS DB tbl_APTrans", "proc_insert_apas_trxn=" + referenceno + "~" +
                                                                                                                                                    rec_curr_cd + "~" +
                                                                                                                                                    rec_acctCurr_Cd + "~" +
                                                                                                                                                    rec_Applied_Rate.ToString() + "~" +
                                                                                                                                                    rec_RefNum_Ours + "~" +
                                                                                                                                                    "0" + "~" +
                                                                                                                                                    "170" + "~" +
                                                                                                                                                    "ORD" + "~" +
                                                                                                                                                    rec_Benf_Name + "~" +
                                                                                                                                                    beneaddress1 + "~" +
                                                                                                                                                    beneaddress2 + "~" +
                                                                                                                                                    beneaddress3 + "~" +
                                                                                                                                                    rec_Spl_Mesg + "~" +
                                                                                                                                                    rec_Rem_Name + "~" +
                                                                                                                                                    rec_Total_Chrg.ToString() + "~" +
                                                                                                                                                    rec_Net_Peso.ToString() + "~" +
                                                                                                                                                    rec_Net_Dollar.ToString() + "~" +
                                                                                                                                                    tranxdate + "~" +
                                                                                                                                                    apas_acct_no + "~" +
                                                                                                                                                    accounttype + "~" +
                                                                                                                                                    remittingbank + "~" +
                                                                                                                                                    "0" + "~" +
                                                                                                                                                    rec_Gross_Peso.ToString() + "~" +
                                                                                                                                                    currentDate + "~" +
                                                                                                                                                    rec_Payee_Benf + "~" +
                                                                                                                                                    System.DateTime.Now.ToString() + "~" +
                                                                                                                                                    branchcode + "~" +
                                                                                                                                                    remaddress1 + "~" +
                                                                                                                                                    remaddress2 + "~" +
                                                                                                                                                    remaddress3 + "~" +
                                                                                                                                                    "" + "~" +
                                                                                                                                                    rec_Benef_ContactNo_value + "~" +
                                                                                                                                                    rec_RemitterID + "~" +
                                                                                                                                                    "APAS" + "~" +
                                                                                                                                                    rec_NSVS_CASA_Acct + "~" +
                                                                                                                                                    transactionpurpose + "~" +
                                                                                                                                                    rec_BeneFName + "~" +
                                                                                                                                                    rec_BeneLName + "~" +
                                                                                                                                                    rec_BeneMName + "~" +
                                                                                                                                                    rec_RemFName + "~" +
                                                                                                                                                    rec_RemLName + "~" +
                                                                                                                                                    rec_RemMName + "~" +
                                                                                                                                                    subbranchcode, "", WinAppLogger.Category.Insert);

                        ///Save IRSW APAS transaction to APAS DB  
                        dt_apas = apas_commons.execStoredProcedureTable("proc_insert_apas_trxn", new object[] { referenceno,
                                                                                                             rec_curr_cd,
                                                                                                             rec_acctCurr_Cd,
                                                                                                             rec_Applied_Rate,
                                                                                                             rec_RefNum_Ours,
                                                                                                             0,
                                                                                                             "170",
                                                                                                             "ORD",
                                                                                                             rec_Benf_Name,
                                                                                                             beneaddress1,
                                                                                                             beneaddress2,
                                                                                                             beneaddress3,
                                                                                                             rec_Spl_Mesg,
                                                                                                             rec_Rem_Name,
                                                                                                             rec_Total_Chrg,
                                                                                                             rec_Net_Peso,
                                                                                                             rec_Net_Dollar,
                                                                                                             tranxdate,
                                                                                                             apas_acct_no,
                                                                                                             accounttype,
                                                                                                             remittingbank,
                                                                                                             "0",
                                                                                                             rec_Gross_Peso,
                                                                                                             currentDate,
                                                                                                             rec_Payee_Benf,
                                                                                                             System.DateTime.Now.ToString(),
                                                                                                             branchcode,
                                                                                                             remaddress1,
                                                                                                             remaddress2,
                                                                                                             remaddress3,
                                                                                                             "",
                                                                                                             rec_Benef_ContactNo_value,
                                                                                                             rec_RemitterID,
                                                                                                             "APAS",
                                                                                                             rec_NSVS_CASA_Acct,
                                                                                                             transactionpurpose,
                                                                                                             rec_BeneFName,
                                                                                                             rec_BeneLName,
                                                                                                             rec_BeneMName,
                                                                                                             rec_RemFName,
                                                                                                             rec_RemLName,
                                                                                                             rec_RemMName,
                                                                                                             subbranchcode
                                                                                                        });

                        if (dt_apas != null && dt_apas.Rows.Count > 0)
                        {
                            bool_master = true;

                            ///Save log information to IRSW DB
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans", "rec_TrxnRef_Num:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Save records to APAS DB tbl_APTranDetails", "proc_insert_apas_trxn_details=" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", WinAppLogger.Category.Insert);

                            ///Save IRSW APAS Tran Details to APAS DB
                            dt_apas_details = apas_commons.execStoredProcedureTable("proc_insert_apas_trxn_details", new object[] { referenceno, "NP", "No", rec_RefNum_Ours, tranxdate });

                            if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                            {
                                bool_details = true;
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans & tbl_APTranDetails.", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                return true;
                            }
                            else
                            {
                                bool_details = false;
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails : " + apas_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                return InsertAPASCentral(tranxID, apas_commons.Message.ToString().Trim());
                            }
                        }
                        else
                        {
                            bool_master = false;
                            bool_details = false;
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTrans : " + apas_commons.Message, "rec_TrxnRef_Num:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails : " + apas_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);

                            return InsertAPASCentral(tranxID, apas_commons.Message.ToString().Trim());
                        }
                    }
                    //================================================================
                    else
                    {
                        base.constring_suffix = "ConnectionStringAPAS";

                        this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_trxnref_num='-1'");

                        if (this.newRow(sqlHelper.RETAIN_DATASET))
                        {
                            if (rembank == "A06")
                            {
                                referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                            }
                            else
                            {

                                referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                            }

                            uprow["rec_TrxnRef_Num"] = referenceno;
                            uprow["rec_Curr_Cd"] = rec_curr_cd;
                            uprow["rec_AcctCurr_Cd"] = rec_acctCurr_Cd;
                            uprow["rec_Applied_Rate"] = rec_Applied_Rate;
                            uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                            uprow["rec_Amount_FD"] = 0;
                            uprow["rec_PNBr_CD"] = 170;
                            uprow["rec_PNBr_Name"] = "ORD";
                            uprow["rec_Benf_Name"] = rec_Benf_Name;
                            uprow["rec_Benf_Adr1"] = beneaddress1;
                            uprow["rec_Benf_Adr2"] = beneaddress2;
                            uprow["rec_Benf_Adr3"] = beneaddress3;
                            uprow["rec_Spl_Mesg"] = rec_Spl_Mesg;
                            uprow["rec_Rem_Name"] = rec_Rem_Name;
                            uprow["rec_Total_Chrg"] = rec_Total_Chrg;
                            uprow["rec_Net_Peso"] = rec_Net_Peso;
                            uprow["rec_Net_Dollar"] = rec_Net_Dollar;
                            uprow["rec_Tran_Date"] = tranxdate;
                            uprow["rec_Acct_No"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'APAS_Online' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
                            uprow["rec_Acct_Type1"] = accounttype;
                            uprow["rec_RB_Name"] = remittingbank;
                            uprow["rec_Pay_Mode"] = 0;
                            uprow["rec_Gross_Peso"] = rec_Gross_Peso;
                            uprow["rec_Date_Received"] = currentDate;
                            uprow["rec_Payee_Benf"] = rec_Payee_Benf;
                            uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                            uprow["rec_Orig_BrCd"] = branchcode;
                            uprow["rec_Rem_Add1"] = remaddress1;
                            uprow["rec_Rem_Add2"] = remaddress2;
                            uprow["rec_Rem_Add3"] = remaddress3;
                            uprow["rec_Benef_MobileNo"] = "";
                            uprow["rec_Benef_ContactNo"] = (rec_Benef_ContactNo.Length > 20) ? rec_Benef_ContactNo.Substring(0, 20) : rec_Benef_ContactNo; //Convert.ToString(this.execScalar("Select TOP 1 ContactNo from tbl_cif_contacts where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
                            uprow["rec_RemitterID"] = rec_RemitterID;
                            uprow["rec_ProdCode"] = "APAS";
                            uprow["rec_NSVS_CASA_Acct"] = rec_NSVS_CASA_Acct;
                            uprow["rec_RemPurpose"] = transactionpurpose;
                            uprow["rec_BeneFName"] = rec_BeneFName;
                            uprow["rec_BeneLName"] = rec_BeneLName;
                            uprow["rec_BeneMName"] = rec_BeneMName;
                            uprow["rec_RemFName"] = rec_RemFName;
                            uprow["rec_RemLName"] = rec_RemLName;
                            uprow["rec_RemMName"] = rec_RemMName;
                            uprow["rec_SubBrCode"] = subbranchcode;

                            bool_master = this.save(sqlHelper.RETAIN_DATASET);
                        }
                        /*Transaction Details*/
                        if (bool_master == true)
                        {
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);

                            this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_trxnRefNum='-1'");

                            if (this.newRow(sqlHelper.RETAIN_DATASET))
                            {
                                uprow["rec_TrxnRefNum"] = referenceno;
                                uprow["rec_TrandInd"] = "NP";
                                uprow["rec_lockind"] = "No";
                                uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                                uprow["rec_Tran_Date"] = tranxdate;

                                bool_details = this.save(sqlHelper.RETAIN_DATASET);

                                if (bool_details == false)
                                {
                                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTranDetails try Central: " + base.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                    return InsertAPASCentral(tranxID, base.Message.ToString().Trim());
                                }
                                else
                                {
                                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTranDetails.", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                }
                            }
                        }
                        else
                        {
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTrans try Central: " + base.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            return InsertAPASCentral(tranxID, base.Message.ToString().Trim());
                        }
                        /*Transaction Details END*/
                        if (bool_master == true && bool_details == true)
                        {
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Successfully Loaded in APAS DB tbl_APTrans & tbl_APTranDetails.", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            return true;
                        }

                        return false;
                    }
                }
                else
                {
                    ///================================================================
                    ///Revise saving IRSW APAS transactions to APAS DB 02182023
                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Unable to retrieve record from IRSW DB : " + irsw_commons.Message, "IRSW SEQ_ID:" + tranxID.ToString(), "", WinAppLogger.Category.Select);
                    return false;
                    ///================================================================
                }
            }
            catch (Exception ex) // commented to remove warning
            {
                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASSever", "Failed to Load in APAS DB tbl_APTrans try Central: " + ex.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                return InsertAPASCentral(tranxID, ex.Message.ToString().Trim());
            }
            finally
            {
                irsw_commons = null;
                apas_commons = null;
            }
        }

        /*This will insert APAS in IRSWorld Central Database*/
        public bool InsertAPASCentral(int tranxID, string error_message)
        {
            ///================================================================
            ///Revise saving IRSW APAS transactions to APAS DB 02182023
            irsSqlCommons irsw_commons = new irsSqlCommons();
            
            ////bool bool_master = false;
            ////bool bool_details = false;

            ////base.constring_suffix = "ConnectionString";
            ////string tranxdate;
            ////string currentDate;
            ////string remittingbank;
            ////DataTable dtRemittance;
            
            ////string accounttype = "";

            ////dtRemittance = GetRemittance(tranxID);
            ////string branchcode = dtRemittance.Rows[0]["RemittingBranch"].ToString();
            ////string transactionpurpose = Convert.ToString(this.execScalar("SELECT ISNULL(fldDescription,'') FROM TBL_LOOKUP WHERE FldGroup = 'TranxPurpose' AND FldValue=" + dtRemittance.Rows[0]["TransactionPurpose"] + ""));
            ////string subbranchcode = dtRemittance.Rows[0]["ReferenceNo"].ToString().Substring(1, 3);
            ////remittingbank = Convert.ToString(this.execScalar("SELECT BranchName From TBL_Branches Where BranchCode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));
            ////currentDate = System.DateTime.Today.Day.ToString("00") + System.DateTime.Today.Month.ToString("00") + System.DateTime.Today.Year.ToString().Substring(2, 2);
            ////tranxdate = Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Day.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Month.ToString("00") + Convert.ToDateTime(dtRemittance.Rows[0]["TranxDate"]).Year.ToString().Substring(2, 2);
            ////string rembank = Convert.ToString(dtRemittance.Rows[0]["RemittingBranch"]);

            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017*/
            ////string remaddress1 = "", remaddress2 = "", remaddress3 = "";
            ////string beneaddress1 = "", beneaddress2 = "", beneaddress3 = "";

            ////string[] rem_ben_adrs = this.set_rem_ben_adress(dtRemittance.Rows[0]["RemitterAddressSeqId"].ToString(), dtRemittance.Rows[0]["BeneficiaryAddressSeqId"].ToString());

            ////if (rem_ben_adrs != null && rem_ben_adrs.Length > 0)
            ////{
            ////    remaddress1 = rem_ben_adrs[0];
            ////    remaddress2 = rem_ben_adrs[1];
            ////    remaddress3 = rem_ben_adrs[2];

            ////    beneaddress1 = rem_ben_adrs[3];
            ////    beneaddress2 = rem_ben_adrs[4];
            ////    beneaddress3 = rem_ben_adrs[5];
            ////}
            /////* MODIFIED BY BOYET TO GET THE COMPLETE ADDRESS OF REMITTER AND BENEFICIARY JAN 30 2017 */

            ////int account_seq = Convert.ToInt32(this.execScalar("SELECT AccountSeqID FROM TBL_Remittance WHERE SEQ_ID=" + tranxID + ""));
            ////DataTable dtAccountInfo = this.getDataSet(this.getSqlCommand("SELECT AccountNo,dbo.f_GetLookupValue('AccountType',AccountType) AS AccountType FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + account_seq + " AND ISNULL(Deleted,0) != 1", false)).Tables[0];

            ////if (dtAccountInfo.Rows.Count > 0)
            ////{
            ////    accounttype = dtAccountInfo.Rows[0]["AccountType"].ToString();
            ////}

            /////* variables */
            ////string referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            ////string rec_curr_cd = Convert.ToString(this.execScalar("SELECT fldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["LocalCurrency"] + ""));
            ////string rec_acctCurr_Cd = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'Currency' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
            ////decimal rec_Applied_Rate = Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"].ToString());
            ////string rec_RefNum_Ours = dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(0, 4) + dtRemittance.Rows[0]["ReferenceNo"].ToString().Trim().Substring(10, 6);

            ////string rec_Benf_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Spl_Mesg = dtRemittance.Rows[0]["SpecialMessage"].ToString();
            ////string rec_Rem_Name = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////decimal rec_Total_Chrg = Convert.ToDecimal(dtRemittance.Rows[0]["TotalLocalCharge"]);
            ////decimal rec_Net_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Net_Dollar = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);
            ////decimal rec_Gross_Peso = Convert.ToDecimal(dtRemittance.Rows[0]["GrossRemAmount"]) * Convert.ToDecimal(dtRemittance.Rows[0]["ForexRemittingToPHP"]);
            ////string rec_Payee_Benf = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') + ',' + ISNULL(FirstName,'') + ' ' + ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_Orig_BrCd = Convert.ToString(this.execScalar("Select NegBranchCode From TBL_Branches WHERE Branchcode = '" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "'"));

            ////string rec_Benef_ContactNo = Convert.ToString(this.execScalar("SELECT	ISNULL(ContactNo, '') FROM	TBL_CIF_contacts	cif	inner join  TBL_REMITTANCE	rem	on	cif.seq_id = rem.BeneficiaryContactSeqID WHERE	rem.seq_id = " + tranxID.ToString()));

            ////string rec_RemitterID = dtRemittance.Rows[0]["RemitterCIFID"].ToString();
            ////string rec_NSVS_CASA_Acct = Convert.ToString(this.execScalar("SELECT AccountNumber FROM TBL_Branch_nsvs_acct WHERE BranchCode='" + dtRemittance.Rows[0]["RemittingBranch"].ToString() + "' AND ISNULL(Deleted,0) != 1"));
            ////string rec_BeneFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_BeneMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["BeneficiaryCIFID"].ToString() + "'"));
            ////string rec_RemFName = Convert.ToString(this.execScalar("Select ISNULL(FirstName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemMName = Convert.ToString(this.execScalar("Select ISNULL(MiddleName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_RemLName = Convert.ToString(this.execScalar("Select ISNULL(LastName,'') from tbl_cif_master where CIFID='" + dtRemittance.Rows[0]["RemitterCIFID"].ToString() + "'"));
            ////string rec_TrxnRefNum = dtRemittance.Rows[0]["ReferenceNo"].ToString();
            ////decimal rec_NetToBene = Convert.ToDecimal(dtRemittance.Rows[0]["NetToBeneAmount"]);

            DataTable dtRemittance;
            bool bool_master = false;
            bool bool_details = false;
            base.constring_suffix = "ConnectionString";

            string rec_curr_cd = "";
            string rec_acctCurr_Cd = "";
            decimal rec_Applied_Rate = 0;
            string rec_RefNum_Ours = "";
            string rec_Benf_Name = "";
            string rec_Spl_Mesg = "";
            string rec_Rem_Name = "";
            decimal rec_Total_Chrg = 0;
            decimal rec_Net_Peso = 0;
            decimal rec_Net_Dollar = 0;
            decimal rec_Gross_Peso = 0;
            string rec_Payee_Benf = "";
            string rec_RemitterID = "";
            string rec_NSVS_CASA_Acct = "";
            string rec_BeneFName = "";
            string rec_BeneLName = "";
            string rec_BeneMName = "";
            string rec_RemFName = "";
            string rec_RemMName = "";
            string rec_RemLName = "";
            string referenceno = "";
            string remaddress1 = "";
            string remaddress2 = "";
            string remaddress3 = "";
            string beneaddress1 = "";
            string beneaddress2 = "";
            string beneaddress3 = "";
            string tranxdate = "";
            string currentDate = "";
            string remittingbank = "";
            string accounttype = "";
            string apas_acct_no = "";
            string branchcode = "";
            string rec_Benef_ContactNo_value = "";
            string rec_Benef_ContactNo = "";
            string transactionpurpose = "";
            string subbranchcode = "";
            string rembank = "";
            ///================================================================

            try
            {
                ///================================================================
                ///Revise saving IRSW APAS transactions to APAS DB 02182023
                DataTable dt_apastran = new DataTable();
                DataTable dt_irswapas = new DataTable();
                DataTable dt_apas = new DataTable();
                DataTable dt_apas_details = new DataTable();
                int apas_enabled = 0;

                ///IRSWorld Connection string
                irsw_commons.constring_suffix = "ConnectionString";

                dtRemittance = GetRemittance(tranxID);
                rembank = Convert.ToString(dtRemittance.Rows[0]["RemittingBranch"]);

                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Get record from IRSW DB", "proc_process_irswapas=" + tranxID.ToString() + ", 1", "", WinAppLogger.Category.Insert);
                dt_apastran = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 1, "" });

                if (dt_apastran != null && dt_apastran.Rows.Count > 0)
                {

                    ///Set APAS values from IRSW TBL_APTrans table
                    if (rembank == "A06")
                    {
                        referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                    }
                    else
                    {
                        referenceno = dt_apastran.Rows[0]["rec_TrxnRef_Num"].ToString();
                    }
                    rec_curr_cd = dt_apastran.Rows[0]["rec_Curr_Cd"].ToString();
                    rec_acctCurr_Cd = dt_apastran.Rows[0]["rec_AcctCurr_Cd"].ToString();
                    rec_Applied_Rate = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Applied_Rate"].ToString());
                    rec_RefNum_Ours = dt_apastran.Rows[0]["rec_RefNum_Ours"].ToString();
                    rec_Benf_Name = dt_apastran.Rows[0]["rec_Benf_Name"].ToString();
                    beneaddress1 = dt_apastran.Rows[0]["rec_Benf_Adr1"].ToString();
                    beneaddress2 = dt_apastran.Rows[0]["rec_Benf_Adr2"].ToString();
                    beneaddress3 = dt_apastran.Rows[0]["rec_Benf_Adr3"].ToString();
                    rec_Spl_Mesg = dt_apastran.Rows[0]["rec_Spl_Mesg"].ToString();
                    rec_Rem_Name = dt_apastran.Rows[0]["rec_Rem_Name"].ToString();
                    rec_Total_Chrg = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Total_Chrg"].ToString());
                    rec_Net_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Peso"].ToString());
                    rec_Net_Dollar = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Net_Dollar"].ToString());
                    tranxdate = dt_apastran.Rows[0]["rec_Tran_Date"].ToString();
                    apas_acct_no = dt_apastran.Rows[0]["rec_Acct_No"].ToString();
                    accounttype = dt_apastran.Rows[0]["rec_Acct_Type1"].ToString();
                    remittingbank = dt_apastran.Rows[0]["rec_RB_Name"].ToString();
                    rec_Gross_Peso = Convert.ToDecimal(dt_apastran.Rows[0]["rec_Gross_Peso"].ToString());
                    currentDate = dt_apastran.Rows[0]["rec_Date_Received"].ToString();
                    rec_Payee_Benf = dt_apastran.Rows[0]["rec_Payee_Benf"].ToString();
                    branchcode = dt_apastran.Rows[0]["rec_Orig_BrCd"].ToString();
                    remaddress1 = dt_apastran.Rows[0]["rec_Rem_Add1"].ToString();
                    remaddress2 = dt_apastran.Rows[0]["rec_Rem_Add2"].ToString();
                    remaddress3 = dt_apastran.Rows[0]["rec_Rem_Add3"].ToString();
                    rec_Benef_ContactNo_value = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_Benef_ContactNo = dt_apastran.Rows[0]["rec_Benef_ContactNo"].ToString();
                    rec_RemitterID = dt_apastran.Rows[0]["rec_RemitterID"].ToString();
                    rec_NSVS_CASA_Acct = dt_apastran.Rows[0]["rec_NSVS_CASA_Acct"].ToString();
                    transactionpurpose = dt_apastran.Rows[0]["rec_RemPurpose"].ToString();
                    rec_BeneFName = dt_apastran.Rows[0]["rec_BeneFName"].ToString();
                    rec_BeneLName = dt_apastran.Rows[0]["rec_BeneLName"].ToString();
                    rec_BeneMName = dt_apastran.Rows[0]["rec_BeneMName"].ToString();
                    rec_RemFName = dt_apastran.Rows[0]["rec_RemFName"].ToString();
                    rec_RemLName = dt_apastran.Rows[0]["rec_RemLName"].ToString();
                    rec_RemMName = dt_apastran.Rows[0]["rec_RemMName"].ToString();
                    subbranchcode = dt_apastran.Rows[0]["rec_SubBrCode"].ToString();

                    //Check if APAS switch is enabled
                    dt_irswapas = irsw_commons.execStoredProcedureTable("isp_getlookup", new object[] { "EnableIRSWAPAS" });
                    if (dt_irswapas != null && dt_irswapas.Rows.Count > 0)
                    {
                        apas_enabled = Convert.ToInt32(dt_irswapas.Rows[0]["FldCode"].ToString());
                    }

                    ///APAS switch enabled
                    if (apas_enabled > 0)
                    {
                        ///Save query log information to IRSW DB
                        auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Save records to IRSW DB tbl_APTrans", "proc_process_irswapas=" + tranxID.ToString() + ",2~" +
                                                                                                                                                referenceno + "~" +
                                                                                                                                                rec_curr_cd + "~" +
                                                                                                                                                rec_acctCurr_Cd + "~" +
                                                                                                                                                rec_Applied_Rate.ToString() + "~" +
                                                                                                                                                rec_RefNum_Ours + "~" +
                                                                                                                                                "0" + "~" +
                                                                                                                                                "170" + "~" +
                                                                                                                                                "ORD" + "~" +
                                                                                                                                                rec_Benf_Name + "~" +
                                                                                                                                                beneaddress1 + "~" +
                                                                                                                                                beneaddress2 + "~" +
                                                                                                                                                beneaddress3 + "~" +
                                                                                                                                                rec_Spl_Mesg + "~" +
                                                                                                                                                rec_Rem_Name + "~" +
                                                                                                                                                rec_Total_Chrg.ToString() + "~" +
                                                                                                                                                rec_Net_Peso.ToString() + "~" +
                                                                                                                                                rec_Net_Dollar.ToString() + "~" +
                                                                                                                                                tranxdate + "~" +
                                                                                                                                                apas_acct_no + "~" +
                                                                                                                                                accounttype + "~" +
                                                                                                                                                remittingbank + "~" +
                                                                                                                                                "0" + "~" +
                                                                                                                                                rec_Gross_Peso.ToString() + "~" +
                                                                                                                                                currentDate + "~" +
                                                                                                                                                rec_Payee_Benf + "~" +
                                                                                                                                                System.DateTime.Now.ToString() + "~" +
                                                                                                                                                branchcode + "~" +
                                                                                                                                                remaddress1 + "~" +
                                                                                                                                                remaddress2 + "~" +
                                                                                                                                                remaddress3 + "~" +
                                                                                                                                                "" + "~" +
                                                                                                                                                rec_Benef_ContactNo_value + "~" +
                                                                                                                                                rec_RemitterID + "~" +
                                                                                                                                                "APAS" + "~" +
                                                                                                                                                rec_NSVS_CASA_Acct + "~" +
                                                                                                                                                transactionpurpose + "~" +
                                                                                                                                                rec_BeneFName + "~" +
                                                                                                                                                rec_BeneLName + "~" +
                                                                                                                                                rec_BeneMName + "~" +
                                                                                                                                                rec_RemFName + "~" +
                                                                                                                                                rec_RemLName + "~" +
                                                                                                                                                rec_RemMName + "~" +
                                                                                                                                                subbranchcode, "", WinAppLogger.Category.Insert);

                        if (this.getCheckAPASTxnIfExistsCentral(referenceno) == false)
                        {
                            ///Save IRSW APAS transaction to IRSW-APAS DB  
                            dt_apas = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 2, error_message });

                            if (dt_apas != null && dt_apas.Rows.Count > 0)
                            {
                                bool_master = true;

                                ///Save log information to IRSW DB
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTrans", "rec_TrxnRef_Num:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Save records to IRSW DB tbl_APTranDetails", "proc_process_irswapas=" + tranxID.ToString() + ", 3~" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", WinAppLogger.Category.Insert);

                                if (this.getCheckAPASTxnDetailsIfExistsCentral(dtRemittance.Rows[0]["ReferenceNo"].ToString()) == false)
                                {
                                    ///Save IRSW APAS Tran Details to IRSW-APAS DB
                                    dt_apas_details = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 3, "" });

                                    if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                                    {
                                        bool_details = true;
                                        auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                                    }
                                    else
                                    {
                                        bool_details = false;
                                        auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Failed to Load in IRSW DB tbl_APTranDetails : " + irsw_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                    }
                                }
                                else
                                {
                                    bool_details = true;
                                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Transaction already existing in IRSW DB tbl_APTranDetails", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                                }
                            }
                            else
                            {
                                bool_master = false;
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Failed to Load in IRSW DB tbl_APTrans : " + irsw_commons.Message, "rec_TrxnRef_Num:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            }
                        }
                        else
                        {
                            bool_master = true;
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Transaction already existing in IRSW DB tbl_APTrans", "rec_TrxnRef_Num:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), WinAppLogger.Category.Select);
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Save records to IRSW DB tbl_APTranDetails", "proc_process_irswapas=" + tranxID.ToString() + ", 3~" + referenceno + "~" + "NP~No~" + rec_RefNum_Ours + "~" + tranxdate, "", WinAppLogger.Category.Insert);

                            if (this.getCheckAPASTxnDetailsIfExistsCentral(referenceno) == false)
                            {
                                ///Save IRSW APAS Tran Details to IRSW-APAS DB
                                dt_apas_details = irsw_commons.execStoredProcedureTable("proc_process_irswapas", new object[] { tranxID, 3, "" });

                                if (dt_apas_details != null && dt_apas_details.Rows.Count > 0)
                                {
                                    bool_details = true;
                                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTranDetails ", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas_details.Rows[0][0].ToString(), WinAppLogger.Category.Insert);
                                }
                                else
                                {
                                    bool_details = false;
                                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Failed to Load in IRSW DB tbl_APTranDetails : " + irsw_commons.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                }
                            }
                            else
                            {
                                bool_details = true;
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Transaction already existing in IRSW DB tbl_APTranDetails", "rec_TrxnRefNum:" + referenceno.ToString(), dt_apas.Rows[0][0].ToString(), WinAppLogger.Category.Select);
                            }
                        }

                        if (bool_master == true && bool_details == true)
                        {
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Successfully Loaded in IRSW DB tbl_APTrans & tbl_APTranDetails retry central ok", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            return true;
                        }
                        else
                        {
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Failed to Load in IRSW DB retry central failed master and details apas table", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            return false;
                        }
                    }
                    //================================================================
                    else
                    {
                        base.constring_suffix = "ConnectionString";

                        this.getUpdateDS("SELECT * FROM tbl_APTrans where rec_trxnref_num='-1'");

                        if (this.newRow(sqlHelper.RETAIN_DATASET))
                        {
                            if (rembank == "A06")
                            {
                                referenceno = dtRemittance.Rows[0]["ExtlReferenceNo"].ToString();
                            }
                            else
                            {

                                referenceno = dtRemittance.Rows[0]["ReferenceNo"].ToString();
                            }

                            uprow["rec_TrxnRef_Num"] = referenceno;
                            uprow["rec_Curr_Cd"] = rec_curr_cd;
                            uprow["rec_AcctCurr_Cd"] = rec_acctCurr_Cd;
                            uprow["rec_Applied_Rate"] = rec_Applied_Rate;
                            uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                            uprow["rec_Amount_FD"] = 0;
                            uprow["rec_PNBr_CD"] = 170;
                            uprow["rec_PNBr_Name"] = "ORD";
                            uprow["rec_Benf_Name"] = rec_Benf_Name;
                            uprow["rec_Benf_Adr1"] = beneaddress1;
                            uprow["rec_Benf_Adr2"] = beneaddress2;
                            uprow["rec_Benf_Adr3"] = beneaddress3;
                            uprow["rec_Spl_Mesg"] = rec_Spl_Mesg;
                            uprow["rec_Rem_Name"] = rec_Rem_Name;
                            uprow["rec_Total_Chrg"] = rec_Total_Chrg;
                            uprow["rec_Net_Peso"] = rec_Net_Peso;
                            uprow["rec_Net_Dollar"] = rec_Net_Dollar;
                            uprow["rec_Tran_Date"] = tranxdate;
                            uprow["rec_Acct_No"] = Convert.ToString(this.execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FldGroup = 'APAS_Online' AND FldValue = " + dtRemittance.Rows[0]["NettoBeneCurrency"] + ""));
                            uprow["rec_Acct_Type1"] = accounttype;
                            uprow["rec_RB_Name"] = remittingbank;
                            uprow["rec_Pay_Mode"] = 0;
                            uprow["rec_Gross_Peso"] = rec_Gross_Peso;
                            uprow["rec_Date_Received"] = currentDate;
                            uprow["rec_Payee_Benf"] = rec_Payee_Benf;
                            uprow["rec_Date_Loaded"] = System.DateTime.Now.ToString();
                            uprow["rec_Orig_BrCd"] = branchcode;
                            uprow["rec_Rem_Add1"] = remaddress1;
                            uprow["rec_Rem_Add2"] = remaddress2;
                            uprow["rec_Rem_Add3"] = remaddress3;
                            uprow["rec_Benef_MobileNo"] = "";
                            uprow["rec_Benef_ContactNo"] = (rec_Benef_ContactNo.Length > 20) ? rec_Benef_ContactNo.Substring(0, 20) : rec_Benef_ContactNo;
                            uprow["rec_RemitterID"] = rec_RemitterID;
                            uprow["rec_ProdCode"] = "APAS";
                            uprow["rec_NSVS_CASA_Acct"] = rec_NSVS_CASA_Acct;
                            uprow["rec_RemPurpose"] = transactionpurpose;
                            uprow["rec_BeneFName"] = rec_BeneFName;
                            uprow["rec_BeneLName"] = rec_BeneLName;
                            uprow["rec_BeneMName"] = rec_BeneMName;
                            uprow["rec_RemFName"] = rec_RemFName;
                            uprow["rec_RemLName"] = rec_RemLName;
                            uprow["rec_RemMName"] = rec_RemMName;
                            uprow["rec_SubBrCode"] = subbranchcode;

                            if (this.getCheckAPASTxnIfExistsCentral(dtRemittance.Rows[0]["ReferenceNo"].ToString()) == false)
                            {
                                bool_master = this.save(sqlHelper.RETAIN_DATASET);

                                if (bool_master == true)
                                {
                                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Successfully Loaded in APAS DB tbl_APTrans: retry central ok", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                }
                                else
                                {
                                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Failed to Load in APAS DB tbl_APTrans retry central failed: " + base.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                                }
                            } // check if referenceno is already existing in APAS

                        }

                        /*Transaction Details*/
                        this.getUpdateDS("SELECT * FROM tbl_APTranDetails where rec_trxnRefNum='-1'");

                        if (this.getCheckAPASTxnIfExistsCentral(dtRemittance.Rows[0]["ReferenceNo"].ToString()) == false)
                        {
                            if (this.newRow(sqlHelper.RETAIN_DATASET))
                            {
                                uprow["rec_TrxnRefNum"] = referenceno;
                                uprow["rec_TrandInd"] = "NP";
                                uprow["rec_lockind"] = "No";
                                uprow["rec_RefNum_Ours"] = rec_RefNum_Ours;
                                uprow["rec_Tran_Date"] = tranxdate;
                            }

                            bool_details = this.save(sqlHelper.RETAIN_DATASET);

                            if (bool_details == true)
                            {
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Successfully Loaded in APAS DB tbl_APTranDetails retry central ok", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            }
                            else
                            {
                                auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Failed to Load in APAS DB tbl_APTranDetails retry central failed: " + base.Message, "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            }
                            // return true;
                        }
                        /*Transaction Details END*/

                        if (bool_master == true && bool_details == true)
                        {
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Successfully Loaded in APAS DB tbl_APTrans & tbl_APTranDetails retry central ok", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            return true;
                        }
                        else
                        {
                            auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Failed to Load in APAS DB retry central failed master and details apas table", "rec_TrxnRefNum:" + referenceno.ToString(), "", WinAppLogger.Category.Insert);
                            return false;
                        }
                    }
                }
                else
                {
                    ///================================================================
                    ///Revise saving IRSW APAS transactions to APAS DB 02182023
                    auditWinApp.writeAudit("irsOldAPASWinApp", "Uploading.APASCentralSever", "Unable to retrieve record from IRSW DB : " + irsw_commons.Message, "IRSW SEQ_ID:" + tranxID.ToString(), "", WinAppLogger.Category.Select);
                    return false;
                    ///================================================================
                }
            }
            catch (Exception ex)
            {
                this.APASMessage = "Error encountered while saving APAS Transaction retry central:" + ex.Message;
                return false;
            }
            finally
            {
                irsw_commons = null;
            }
        }
        #endregion

        private DataTable GetRemittance(int seq_id)
        {
            DataTable dt = null;

            try
            {
                dt = this.getDataSet(this.getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + seq_id.ToString() , false)).Tables[0]; return dt;
            }
            catch
            {
                return null;
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

        private bool getCheckAPASTxnIfExists(String referenceno)
        {
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */
            DataTable dt = null;

            try
            {
                base.constring_suffix = "ConnectionStringAPAS";

                dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */

            /*
            base.constring_suffix = "ConnectionStringAPAS";
            DataTable dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];
            if (dt.Rows.Count > 0) { return true; }
            return false;
             */ 
        }
        private bool getCheckAPASTxnIfExistsCentral(String referenceno)
        {
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */
            DataTable dt = null;

            try
            {
                dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */

            /*
            DataTable dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];
            if (dt.Rows.Count > 0) { return true; }
            return false;
             */ 

        }

        private bool getCheckAPASTxnDetailsIfExistsCentral(String referenceno)
        {
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */
            DataTable dt = null;

            try
            {
                dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTranDetails where rec_TrxnRefNum='" + referenceno + "'", false)).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            /* MODIFIED BY BOYET FEB 6 2017 TO DISPOSE OBJECT */

            /*
            DataTable dt = this.getDataSet(this.getSqlCommand("SELECT * FROM tbl_APTrans where rec_trxnref_num='" + referenceno + "'", false)).Tables[0];
            if (dt.Rows.Count > 0) { return true; }
            return false;
             */

        }

        private string msg = "";
        public string APASMessage
        {
            get { return msg; }
            set { msg = value; }
        }

    }

}
