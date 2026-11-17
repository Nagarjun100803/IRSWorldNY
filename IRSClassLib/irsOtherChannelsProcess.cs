using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace IRSClassLib
{
    public class irsOtherChannelsProcess : sqlHelper
    {
        #region PRIVATE VARIABLES

        //Processing to IRS
        private ArrayList rec_err;
        private int record_count = 0;       // assigned to remove warning
        private int record_processed = 0;   // assigned to remove warning        
        private string gefu_summary;

        //
        private string error_message;
        private int fSEQ_ID;
        private int rSEQ_ID;

        //Remittance Table
        private string UserID;
        private int RemitMethod;
        private string RemitStationID;
        private string RemittingBranch;
        //private bool ChargeLocally; // commented to remove warning
        private string ExtlReferenceNo;
        private int Transtatus;
        private DateTime TranxDate;
        private DateTime Clr_Date;
        private int ProductChargesID;
        private string RemitterCIFID;
        private string BeneficiaryCIFID;
        private string AccountSEQID;
        private int AccountCurrency;
        private int RemittingCurrency;
        private int LocalCurrency;
        private double GrossRemAmount;
        //private double NetRemAmount; // commented to remove warning
        //private int NettoBeneCurrency; // commented to remove warning
        //private double NettoBeneAmount; // commented to remove warning
        //private double TotalLocalCharge; // commented to remove warning
        private int TransactionSource;
        private int ReceiptMode;
        private int TransactionPurpose;
        private int SourceofFunds;
        private string OtherPurpose;
        private string SpecialMessage;
        private bool printed;
        private string remitterAddSeqId;
        private string beneficiaryAddSeqId;
        private string remitterContactSeqId;
        private string beneficiaryContactSeqId;
        //private double ForexRemittingToAccount; // commented to remove warning
        //private double ForexRemittingToLocal; // commented to remove warning
        //private double ForexRemittingToUSD; // commented to remove warning
        //private double ForexRemittingToPHP; // commented to remove warning
        //private double ForexRemittingToEUR; // commented to remove warning
        //private double ForexLocalToRemitting;  // commented to remove warning
        //private double ForexLocalToUSD;  // commented to remove warning
        //private double ForexLocalToPHP;  // commented to remove warning
        private int _BatchNo;
        private int ChargeType;
        private string RemitterAddOtherChannels = "";
        private string BeneficiaryAddOtherChannels = "";
        //private int PayMode; // commented to remove warning

        private ArrayList txn_summary;
        private  StreamWriter srlog;
        #endregion

        #region PUBLIC METHODS

        public int BatchNo
        {
            get { return _BatchNo; }
            set { _BatchNo = value; }
        }

        public ArrayList LOAD_ERR
        {
            get { return rec_err; }
        }

        public int REC_COUNT
        {
            get { return record_count; }
        }

        public int REC_PROCESSED
        {
            get { return record_processed; }
        }

        public string USER_ID
        {
            get { return UserID; }
            set { UserID = value; }
        }

        public string message
        {
            get { return this.error_message; }
        }

        public int rem_SEQ_ID
        {
            get { return rSEQ_ID; }
            set { rSEQ_ID = value; }
        }

        public int file_SEQ_ID
        {
            get { return this.fSEQ_ID; }
            set { this.fSEQ_ID = value; }
        }

        public string REMIT_BRANCH
        {
            get { return this.RemittingBranch; }
            set { this.RemittingBranch = value; }
        }

        public ArrayList Txn_Summary
        {
            get { return this.txn_summary; }
        }

        public string Gefu_Summary
        {
            get { return this.gefu_summary; }
        }

        public void processFedToRemittance(int fSEQ_ID, string arch_path)
        {
            //===============================
            /* FOR FEDACH STREAMLINING */
            DateTime start = DateTime.Now;
            DateTime end;
            string ExtlRefno = "";
            string fed_file_name = "";
            //===============================

            txn_summary = new ArrayList();
            rec_err = new ArrayList();
            file_SEQ_ID = fSEQ_ID;
            DataTable dt = new DataTable();

            irsInsertRemittance remit = new irsInsertRemittance(this.RemittingBranch, this.USER_ID, false);

            try
            {
                object[] param = new object[] { fSEQ_ID, this.REMIT_BRANCH };

                //===============================
                /* FOR FEDACH STREAMLINING */
                //comment out for FEDACH STREAMLINING
                //DataTable dt = execStoredProcedureTable("SP_REMIT_OTHERCHANNELS", param);

                dt = execStoredProcedureTable("SP_REMIT_OTHERCHANNELS", param);
                fed_file_name = dt.Rows[0]["FILENAME"].ToString();
                //===============================

                if (dt != null && dt.Rows.Count > 0)
                {
                    this.RemittingBranch = dt.Rows[0]["Branch"].ToString();
                    irsCreateGefu gefu = new irsCreateGefu();

                    for (int ndx = 0; ndx < dt.Rows.Count; ndx++)
                    {
                        DataRow dr = dt.Rows[ndx];
                        bool valid = Convert.ToBoolean(dr["Valid"]);

                        if (valid)
                        {
                            if (Convert.ToInt32(dt.Rows[ndx]["ChargeType"]) > 0)
                            {
                                string dfi = dr["TranxType"].ToString();
                                int tranx_type = Convert.ToInt32(dr["TranxType"]);

                                if (tranx_type == 1)
                                {
                                    //string[] remit_param = this.setParamVal(dr);
                                    
                                    string[] remit_param = this.setParamVal_FedAch(dr);

                                    //===============================
                                    /* FOR FEDACH STREAMLINING */
                                    DateTime start1;
                                    DateTime end1;
                                    ExtlRefno = remit_param[4].ToString();

                                    //comment out for FEDACH STREAMLINING
                                    //if (remit.Remit(AccountCurrency, this.ProductChargesID.ToString(), (double)GrossRemAmount, remit_param))
                                    //===============================

                                    if (remit.Remit(AccountCurrency, this.ProductChargesID.ToString(), (double)GrossRemAmount, remit_param, fed_file_name))
                                    {
                                        start1 = DateTime.Now;
                                        int rec_count = ndx + 1;
                                        remit.doOtherDetails(fSEQ_ID.ToString(), this.Clr_Date);
                                        txn_summary.Add("Record Count~" + rec_count.ToString() + "|" + remit.TranxSummary);
                                    }
                                    else
                                    {
                                        //add data to Other details table even Remit function returned False 02052014
                                        start1 = DateTime.Now;
                                        remit.doOtherDetails(fSEQ_ID.ToString(), this.Clr_Date);
                                        rec_err.Add(remit.RemitMessage);
                                    }

                                    //===============================
                                    /* FOR FEDACH STREAMLINING */
                                    end1 = DateTime.Now;
                                    this.createElapsedLog(fed_file_name, start1, end1, "InsertRemittance().Remit / DoOtherDetails", ExtlRefno);
                                    //===============================
                                }
                                else if (tranx_type == 2)
                                {
                                    string future_dated_flag = "N";
                                    if (Convert.ToDateTime(dr["ValueDate"]) > DateTime.Now)
                                        future_dated_flag = "Y";
                                    gefu.newItem("01", "1408", this.RemittingBranch, "D", "30", future_dated_flag);
                                    gefu.Account_No = dr["AccountNO"].ToString();
                                    gefu.Txn_Date = DateTime.Now.ToString("yyyyMMdd");
                                    gefu.Value_Date = Convert.ToDateTime(dr["ValueDate"]).ToString("yyyyMMdd");
                                    gefu.Txn_Currency = dr["FCRemittingCurrency"].ToString();
                                    gefu.Amount_Txn = dr["Amount"].ToString();
                                    gefu.Amount_Local = Convert.ToDouble(dr["Amount"]);
                                    gefu.Convert_Rate = "1";
                                    gefu.Reference_No = dr["OtherChannelsRefNo"].ToString();
                                    gefu.Txn_Desc = dr["BeneficiaryInfo"].ToString();
                                    gefu.appendItem();
                                }
                                else
                                {
                                    rec_err.Add("INVALID RECORD - ExtlReferenceNo: " + dr["ExtlReferenceNo"].ToString() + " Reason: " + dr["Remarks"].ToString());
                                }
                            }
                            else
                            {
                                this.error_message = "Charges not defined for the branch " + REMIT_BRANCH + " : ";
                                rec_err.Add(this.error_message + "ChargeType=" + dr["FILEFORMAT"].ToString() +
                                                                " PayMode=" + dr["PAYMODE"].ToString() +
                                                                " Product=" + dr["PRODUCTCODE"].ToString() +
                                                                " Implementation=" + dr["IMPLEMENTATIONTYPE"].ToString() +
                                                                " Currency=" + dr["REMITTINGCURRENCY"].ToString() );
                            }
                        }
                        else
                        {
                            rec_err.Add("INVALID RECORD - ExtlReferenceNo: " + dr["ExtlReferenceNo"].ToString() + " Reason: " + dr["Remarks"].ToString());
                        }
                    }
                    if (gefu.itemCount > 0)
                    {
                        gefu.newItem("03", "1008", this.RemittingBranch, "C", "30", "N");
                        gefu.Account_No = new irsConfiguration().appsetting("FedAchGefu_GL");
                        gefu.Txn_Date = DateTime.Now.ToString("yyyyMMdd");
                        gefu.Value_Date = DateTime.Now.ToString("yyyyMMdd");
                        gefu.Txn_Currency = dt.Rows[0]["FCRemittingCurrency"].ToString();
                        gefu.Amount_Txn = gefu.Amount_Txn;
                        gefu.Amount_Local = gefu.Debit_Amount;
                        gefu.Convert_Rate = "1";
                        gefu.appendItem();
                        gefu.create(arch_path, dt.Rows[0]["FileName"].ToString());

                        //gefu_summary = "Currency~" + dt.Rows[0]["RemittingCurrencyDesc"].ToString() + "|" +
                        //                "GefuCount~" + gefu.Debit_Count.ToString() + "|" +
                        //                "TotalAmount~" + gefu.Debit_Amount.ToString();
                        gefu_summary = gefu.Debit_Count.ToString();
                    }

                    gefu = null;
                }

                dt = null;
                param = null;
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
            }
            finally
            {
                remit = null;
                LOAD_ERR.Add(this.error_message);

                //===============================
                /* FOR FEDACH STREAMLINING */
                end = DateTime.Now;
                this.createElapsedLog(fed_file_name, start, end, "processFedToRemittance()", ExtlRefno);
                //===============================
            }
        }

        public void processToRemittance(int fSEQ_ID)
        {
            txn_summary = new ArrayList();
            rec_err = new ArrayList();
            file_SEQ_ID = fSEQ_ID;
            irsInsertRemittance remit = new irsInsertRemittance(this.RemittingBranch, this.USER_ID, false);

            try
            {
                object[] param = new object[] { fSEQ_ID, this.REMIT_BRANCH };
                DataTable dt = execStoredProcedureTable("SP_REMIT_OTHERCHANNELS", param);
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int ndx = 0; ndx < dt.Rows.Count; ndx++)
                    {
                        DataRow dr = dt.Rows[ndx];
                        bool valid = Convert.ToBoolean(dr["Valid"]);
                        if (valid)
                        {
                            string[] remit_param = this.setParamVal(dr);
                            /* 08142017
                            if (remit.Remit(AccountCurrency, this.ProductChargesID.ToString(), (double)GrossRemAmount, remit_param))
                            {
                                int rec_count = ndx + 1;
                                remit.doOtherDetails(fSEQ_ID.ToString());
                                txn_summary.Add("Record Count~" + rec_count.ToString() + "|" + remit.TranxSummary);
                            }
                            else
                            {
                                rec_err.Add(
                                                "ERROR: " + remit.RemitMessage + "<" + dt.Rows[ndx]["Buff"].ToString() + "> " 
                                                + "Reason: " + remit.RemitMessage 
                                                + " ChargeType: " + dr["FileFormat"].ToString()
                                                + " Product: " + dr["ProductDescription"].ToString()
                                                + " Implementation: " + dr["ImplementationDescription"].ToString()
                                                + " Currency: " + dr["RemittingCurrencyDesc"].ToString()
                                            );
                            }
                            */
                        }
                        else
                        {
                            rec_err.Add("ERROR: INVALID RECORD <" + dr["Buff"].ToString() + "> Reason: " + dr["Remarks"].ToString());
                        }
                    }
                }
                dt = null;
                param = null;
            }
            catch //(Exception ex)  // commented to remove warning
            {
                LOAD_ERR.Add(this.error_message);
            }
            finally
            {
                if (remit.RemitMessage != null && remit.RemitMessage.Trim() != "")
                    LOAD_ERR.Add(remit.RemitMessage);
                remit = null;
            }
        }
        #endregion

        #region PRIVATE METHODS
        private string getLookupDesc(string Group, string Code)
        {
            object[] param = new object[] { Group, Code };
            try
            {
                DataTable dt = execStoredProcedureTable("SP_BatchEquivalentValues", param);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Description"].ToString();
                }
            }
            catch // (Exception e) // commented to remove warning
            {
                this.error_message = Message;
            }
            return "";
        }

        private string getLookupDesc(string Group, int val)
        {
            return getLookupDesc(Group, getLookupCode(Group, val));
        }

        private string getLookupCode(string Group, int val)
        {
            object[] param = new object[] { Group, val };
            try
            {
                DataTable dt = execStoredProcedureTable("SP_getLookUpByVal", param);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["Code"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                error_message = e.Message;
            }

            return "";
        }

        private int getLookupValue(string Group, string Code)
        {
            object[] param = new object[] { Group, Code };
            try
            {
                DataTable dt = execStoredProcedureTable("SP_BatchEquivalentValues", param);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dt.Rows[0]["Value"]);
                    }
                }
            }
            catch (Exception e)
            {
                error_message = e.Message;
            }
            return 0;
        }

        private bool insertRecord()
        {
            if (base.newRow())
            {
                return true;
            }
            this.getErrorDB();
            return false;
        }

        private string[] setParamVal_FedAch(DataRow dr)
        {
            string[] param = null;
            DateTime TimeZone_Date;
            string TranxStatus = "";

            TimeZone_Date = new irsTimeZone().branchLocalTime_nWs(this.RemittingBranch.Trim());

            try
            {
                setValue(dr);

                //Handling of ABC/ASB Accounts
                TranxStatus = this.Transtatus.ToString();

                //=============================================================================
                //RFC-37590_DMND0001566 RCI IRSW NY Interface with Refinitiv WorldCheck via API
                ////if (TranxStatus != "10314")
                ////{
                ////    if (Clr_Date > TimeZone_Date)
                ////    {
                ////        TranxStatus = "14";
                ////    }
                ////    else if (this.Transtatus == 1)
                ////    {
                ////        TranxStatus = "15";
                ////    }
                ////}

                DataTable dt_status = new DataTable();

                dt_status = execStoredProcedureTable("proc_get_loader_tran_status", new object[] { this.ExtlReferenceNo, this.RemitterCIFID, this.BeneficiaryCIFID, this.Transtatus, Clr_Date, TimeZone_Date });

                if (dt_status != null && dt_status.Rows.Count > 0)
                {
                    TranxStatus = dt_status.Rows[0][0].ToString();
                }
                else
                {
                    if (TranxStatus != "10314")
                    {
                        if (Clr_Date > TimeZone_Date)
                        {
                            TranxStatus = "14";
                        }
                        else if (this.Transtatus == 1)
                        {
                            TranxStatus = "15";
                        }
                    }
                }
                //=============================================================================

                param = new string[] 
                {
                    "UserID~" + this.USER_ID,
                    "RemitMethod~" + this.RemitMethod.ToString(),
                    "RemitStationID~" + this.RemitStationID,
                    "RemittingBranch~" + this.RemittingBranch,
                    "ExtlReferenceNo~" + this.ExtlReferenceNo,
                    //"Transtatus~" + ((Clr_Date > DateTime.Now) ? "14" : ((this.Transtatus == 1) ? "15" : this.Transtatus.ToString())),
                    //"Transtatus~" + ((Clr_Date > TimeZone_Date) ? "14" : ((this.Transtatus == 1) ? "15" : this.Transtatus.ToString())),
                    "Transtatus~" + TranxStatus,
                    "TranxDate~" + this.TranxDate.ToString(),
                    "RemitterCIFID~" + this.RemitterCIFID,
                    "BeneficiaryCIFID~" + this.BeneficiaryCIFID,
                    "AccountSEQID~" + this.AccountSEQID,
                    "RemittingCurrency~" + this.RemittingCurrency.ToString(),
                    "TransactionSource~" + this.TransactionSource.ToString(),
                    "ReceiptMode~" + this.ReceiptMode.ToString(),
                    "TransactionPurpose~" + this.TransactionPurpose.ToString(),
                    "OtherPurpose~" + this.OtherPurpose,
                    "SpecialMessage~" + this.SpecialMessage,
                    "printed~" + this.printed.ToString(),
                    "SourceofFunds~" + this.SourceofFunds.ToString(),
                    "LocalCurrency~" + this.LocalCurrency.ToString(),
                    "BatchNo~0",
                    "DatePostedOverseas~" + TimeZone_Date.ToString(),
                    "DatePostedLocal~" + DateTime.Now.ToString(),
                    "RemitterAddressSeqID~" + this.remitterAddSeqId,
                    "RemitterContactSeqID~" + this.remitterContactSeqId,
                    "BeneficiaryAddressSeqID~" + this.beneficiaryAddSeqId,
                    "BeneficiaryContactSeqID~" + this.beneficiaryContactSeqId,
                    "RemitterAccountNo~" + this.ExtlReferenceNo,
                    "RemitterAddOtherChannels~" + this.RemitterAddOtherChannels,
                    "BeneficiaryAddOtherChannels~" + this.BeneficiaryAddOtherChannels
                };

                return param;
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
                throw e;
            }
            // return param; // commented to remove warning
        }

        private string[] setParamVal(DataRow dr)
        {
            string[] param = null;
            try
            {
                

                setValue(dr);
                param = new string[] 
                {
                    "UserID~" + this.USER_ID,
                    "RemitMethod~" + this.RemitMethod.ToString(),
                    "RemitStationID~" + this.RemitStationID,
                    "RemittingBranch~" + this.RemittingBranch,
                    "ExtlReferenceNo~" + this.ExtlReferenceNo,
                    "Transtatus~" + ((Clr_Date > DateTime.Now) ? "14" : ((this.Transtatus == 1) ? "15" : this.Transtatus.ToString())),
                    "TranxDate~" + this.TranxDate.ToString(),
                    "RemitterCIFID~" + this.RemitterCIFID,
                    "BeneficiaryCIFID~" + this.BeneficiaryCIFID,
                    "AccountSEQID~" + this.AccountSEQID,
                    "RemittingCurrency~" + this.RemittingCurrency.ToString(),
                    "TransactionSource~" + this.TransactionSource.ToString(),
                    "ReceiptMode~" + this.ReceiptMode.ToString(),
                    "TransactionPurpose~" + this.TransactionPurpose.ToString(),
                    "OtherPurpose~" + this.OtherPurpose,
                    "SpecialMessage~" + this.SpecialMessage,
                    "printed~" + this.printed.ToString(),
                    "SourceofFunds~" + this.SourceofFunds.ToString(),
                    "LocalCurrency~" + this.LocalCurrency.ToString(),
                    "BatchNo~0",
                    "DatePostedOverseas~" + new irsTimeZone().branchLocalTime_nWs(this.RemittingBranch.Trim()).ToString(),
                    "DatePostedLocal~" + DateTime.Now.ToString(),
                    "RemitterAddressSeqID~" + this.remitterAddSeqId,
                    "RemitterContactSeqID~" + this.remitterContactSeqId,
                    "BeneficiaryAddressSeqID~" + this.beneficiaryAddSeqId,
                    "BeneficiaryContactSeqID~" + this.beneficiaryContactSeqId,
                    "RemitterAccountNo~" + this.ExtlReferenceNo,
                    "RemitterAddOtherChannels~" + this.RemitterAddOtherChannels,
                    "BeneficiaryAddOtherChannels~" + this.BeneficiaryAddOtherChannels
                };

                return param;
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
                throw e;
            }
            // return param; // commented to remove warning
        }

        private bool commitRecord()
        {
            if (base.save())
            {
                this.rSEQ_ID = Convert.ToInt32(base.IdentityValue); return true;
            }
            this.getErrorDB(); return false;
        }

        private void setRecordProcessed(int SEQ_ID)
        {
            try
            {
                SqlCommand cmd = getSqlCommand("UPDATE TBL_OTHERCHANNELS_UPLOADS "
                                + "SET Remarks = 'SENT TO IRS' "
                                + "WHERE SEQ_ID = " + SEQ_ID.ToString(), false);
                int i = sqlUpdate(cmd);
            }
            catch
            {
                error_message = "Database Connection Error.";
            }
        }

        private void setFileProcessed(int SEQ_ID)
        {
            try
            {
                SqlCommand cmd = getSqlCommand("UPDATE TBL_FILE_INVENTORY "
                                + "SET Status = 2"
                                + ", DateUpdated = GETDATE()"
                                + ", DateSentToIRS = GETDATE()"
                                + " WHERE SEQ_ID = " + SEQ_ID.ToString(), false);
                int i = sqlUpdate(cmd);
            }
            catch
            {
                error_message = "Database Connection Error.";
            }
        }
        
        private void setValue(DataRow dr)
        {
            this.ChargeType = Convert.ToInt32(dr["ChargeType"]);
            this.LocalCurrency = Convert.ToInt32(dr["LocalCurrency"]);
            this.RemittingCurrency = Convert.ToInt32(dr["RemittingCurrency"]);
            this.TransactionSource = Convert.ToInt32(dr["TranxSource"]);
            this.ReceiptMode = Convert.ToInt32(dr["ReceiptMode"]);
            this.TransactionPurpose = Convert.ToInt32(dr["TranxPurpose"]);
            this.SourceofFunds = Convert.ToInt32(dr["SourceOfFunds"]);
            this.GrossRemAmount = Convert.ToDouble(dr["Amount"].ToString().Trim());
            this.AccountCurrency = Convert.ToInt32(dr["AccountCurrency"]);
            this.RemitMethod = 1;
            this.RemitStationID = "n/a";
            //this.ChargeLocally = true; // commented to remove warning
            this.ExtlReferenceNo = dr["ExtlReferenceNo"].ToString().Trim();
            this.Transtatus = Convert.ToInt32(dr["TranxStatus"]);
            this.TranxDate = DateTime.Now;
            this.ProductChargesID = Convert.ToInt32(dr["ProductChargesID"]);
            this.RemitterCIFID = dr["RemitterCIFID"].ToString();
            this.BeneficiaryCIFID = dr["BeneficiaryCIFID"].ToString();
            this.AccountSEQID = dr["AccountSEQ_ID"].ToString();
            this.remitterAddSeqId = dr["REMADDSEQ_ID"].ToString();
            this.remitterContactSeqId = dr["REMCONTACTSEQ_ID"].ToString();
            this.beneficiaryAddSeqId = dr["BENEFADDSEQ_ID"].ToString();
            this.beneficiaryContactSeqId = dr["BENEFCONTACTSEQ_ID"].ToString();
            this.OtherPurpose = "n/a";
            this.SpecialMessage = dr["SPECIALMESSAGE"].ToString();
            this.printed = false;
            this.Clr_Date = Convert.ToDateTime(dr["VALUEDATE"]);
            this.RemitterAddOtherChannels = dr["REMITTERADDOTHERCHANNELS"].ToString();
            this.BeneficiaryAddOtherChannels = dr["BENEFICIARYADDOTHERCHANNELS"].ToString();

        }

        private int getRecordStatus(string stat)
        {
            if (stat == "VALID") return 1;
            else return 0;
        }

        private void getErrorDB()
        {
            this.error_message = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
                this.error_message = base.Message;
        }

        private void updateTempTable(int fSEQ_ID, int setStatus)
        {
            SqlCommand cmd = getSqlCommand("IF EXISTS(SELECT SEQ_ID FROM TBL_OTHERCHANNELS_UPLOADS WHERE FileSEQ_ID = " + fSEQ_ID.ToString()
                            + ") UPDATE TBL_OTHERCHANNELS_UPLOADS "
                            + "SET RecordStatus = " + setStatus.ToString()
                            + " WHERE FileSEQ_ID = " + fSEQ_ID.ToString(), false);

            int i = sqlUpdate(cmd);
        }

        //===============================
        /* FOR FEDACH STREAMLINING */
        public void createElapsedLog(string filename_loaded, DateTime start, DateTime end, string function_name, string trace_no)
        {
            if (ConfigurationManager.AppSettings["TRACE"] == "1")
            {
                int ilog = execStoredProcedureUpdate("proc_Insert_FedAch_Trace_Log", new object[] { filename_loaded, trace_no, function_name, start, end });
            }
        }
        //===============================

        #endregion

        public override void getUpdateDS(int id)
        {
            SqlCommand cmdDefaults = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(cmdDefaults);
        }

        public irsOtherChannelsProcess(string user, string branch)
        {
            this.UserID = user;
            this.RemittingBranch = branch;
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }

        public irsOtherChannelsProcess()
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }
    }

    public class irsOtherChannelsCIFMaster : sqlHelper
    {

    }
}
