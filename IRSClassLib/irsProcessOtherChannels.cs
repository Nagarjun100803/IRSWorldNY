using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsProcessOtherChannels : sqlHelper
    {
        irsCalculator irsCalc;

        #region PRIVATE VARIABLES

        private string error_message;
        private string records;
        private int fSEQ_ID;
        private int rSEQ_ID;

        private string UserID;//*
        private int RemitMethod;
        private string RemitStationID;
        private string RemittingBranch;//*
        private bool ChargeLocally;
        private string ExtlReferenceNo;
        private int Transtatus;
        private DateTime TranxDate;
        private int ProductChargesID;
        private string RemitterCIFID;
        private string BeneficiaryCIFID;
        private Int64 AccountSEQID;
        private int RemittingCurrency;
        private int LocalCurrency;
        private double GrossRemAmount;
        private double NetRemAmount;
        private int NettoBeneCurrency;
        private double NettoBeneAmount;
        private double TotalLocalCharge;
        private int TransactionSource;
        private int ReceiptMode;
        private int TransactionPurpose;
        private int SourceofFunds;
        private string OtherPurpose;
        private string SpecialMessage;
        private bool printed;
        private double ForexRemittingToAccount;
        private double ForexRemittingToLocal;
        private double ForexRemittingToUSD;
        private double ForexRemittingToPHP;
        private double ForexRemittingToEUR;
        private double ForexLocalToRemitting;
        private double ForexLocalToUSD;
        private double ForexLocalToPHP;
        
        #endregion

        #region PUBLIC METHODS

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

        public int processToIRS(string record_status, int fileSEQ_ID)
        {
            int recProcessed = 0;

            try
            {

                //object[] param = new object[] { fileSEQ_ID, getRecordStatus(record_status) };
                //DataTable dt = execStoredProcedureTable("SP_Select_OtherChannels_to_IRS", param);

                //if (dt.Rows.Count > 0)
                //{
                //    foreach (DataRow row in dt.Rows)
                //    {
                //        int ra = execStoredProcedureUpdate("ksp_InsertRemittance", setParamVal(row));
                //        if (ra > 0) recProcessed++;
                //        updateTempTable(fileSEQ_ID, 2);
                //    }

                //    int file_status = 0;
                //    if (recProcessed > 0)
                //    {
                //        file_status = 2;
                //        setProcessed(fileSEQ_ID, 2);
                //    }
                //    else
                //    {
                //        if (recProcessed != 0)
                //        {
                //            file_status = 3;
                //            setProcessed(fileSEQ_ID, file_status);
                //        }
                //    }
                //}
            }
            catch
            {
                //if (recProcessed > 0)
                //{
                //    file_status = 4;
                //    setProcessed(fileSEQ_ID, file_status);
                //}
                //recProcessed = 0;
            }

            return recProcessed;
        }

        public bool insertRecord()
        {
            if (base.newRow())
            {
                return true;
            }
            this.getErrorDB();
            return false;
        }

        public bool setParamVal(DataRow dr)
        {
            try
            {
                setValue(dr);

                uprow["UserID"] = this.USER_ID;
                uprow["RemitMethod"] = this.RemitMethod;
                uprow["RemitStationID"] = this.RemitStationID;

                uprow["RemittingBranch"] = this.RemittingBranch;
                //uprow["ReferenceNo"] = this.ReferenceNo;
                uprow["ChargeLocally"] = this.ChargeLocally;
                uprow["ExtlReferenceNo"] = this.ExtlReferenceNo;
                uprow["Transtatus"] = this.Transtatus;
                uprow["TranxDate"] = this.TranxDate;
                uprow["ProductChargesID"] = this.ProductChargesID;
                uprow["RemitterCIFID"] = this.RemitterCIFID;
                uprow["BeneficiaryCIFID"] = this.BeneficiaryCIFID;
                uprow["AccountSEQID"] = this.AccountSEQID;
                uprow["RemittingCurrency"] = this.RemittingCurrency;
                uprow["GrossRemAmount"] = this.GrossRemAmount;
                uprow["NetRemAmount"] = this.NetRemAmount;
                uprow["NetToBeneCurrency"] = this.NettoBeneCurrency;
                uprow["NetToBeneAmount"] = this.NettoBeneAmount;
                uprow["TotalLocalCharge"] = this.TotalLocalCharge;
                uprow["TransactionSource"] = this.TransactionSource;
                uprow["ReceiptMode"] = this.ReceiptMode;
                uprow["TransactionPurpose"] = this.TransactionPurpose;
                uprow["OtherPurpose"] = this.OtherPurpose;
                uprow["SpecialMessage"] = this.SpecialMessage;
                uprow["ForexRemittingToLocal"] = this.ForexRemittingToLocal;
                uprow["ForexRemittingToEUR"] = this.ForexRemittingToEUR;
                uprow["printed"] = this.printed;
                uprow["ForexLocalToRemitting"] = this.ForexLocalToRemitting;
                uprow["ForexRemittingToUSD"] = this.ForexRemittingToUSD;
                uprow["ForexRemittingToPHP"] = this.ForexRemittingToPHP;
                uprow["ForexLocalToUSD"] = this.ForexLocalToUSD;
                uprow["ForexLocalToPHP"] = this.ForexLocalToPHP;
                uprow["ForexRemittingToAccount"] = this.ForexRemittingToAccount;
                uprow["SourceofFunds"] = this.SourceofFunds;
                uprow["LocalCurrency"] = this.LocalCurrency;

                return true;
            }
            catch (Exception e)
            {
                error_message = e.Message;
                return false;
            }
        }

        public bool commitRecord()
        {
            if (base.save())
            {
                this.rSEQ_ID = Convert.ToInt32(base.IdentityValue); return true;
            }
            this.getErrorDB(); return false;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand cmdDefaults = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(cmdDefaults);
        }

        public void setRecordProcessed(int SEQ_ID)
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

        public void setFileProcessed(int SEQ_ID)
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
        #endregion

        #region PRIVATE METHODS
        private void setValue(DataRow dr)
        {
            int prodChargID = Convert.ToInt32(dr["Prod_Charg_ID"]);
            int acctCur = Convert.ToInt32(dr["Currency"]);

            this.GrossRemAmount = Convert.ToDouble(dr["Amount"].ToString().Trim());
            this.RemitMethod = 1;
            this.RemitStationID = "RemitStationID";
            this.ChargeLocally = true;
            this.ExtlReferenceNo = dr["ExternalRefNo"].ToString().Trim();
            this.Transtatus = Convert.ToInt32(dr["TranxStatus"]);
            this.TranxDate = DateTime.Now;
            this.ProductChargesID = prodChargID;
            this.RemitterCIFID = dr["RemitterCIFID"].ToString();
            this.BeneficiaryCIFID = dr["BeneficiaryCIFID"].ToString();
            this.AccountSEQID = Convert.ToInt32(dr["AccountSEQ_ID"]);
            this.RemittingCurrency = Convert.ToInt32(dr["RemitCurrency"]);
            this.LocalCurrency = Convert.ToInt32(dr["LocalCurrency"]);
            this.TransactionSource = Convert.ToInt32(dr["TransactionSource"]);
            this.ReceiptMode = Convert.ToInt32(dr["ReceiptMode"]);
            this.TransactionPurpose = Convert.ToInt32(dr["TransactionPurpose"]);
            this.SourceofFunds = Convert.ToInt32(dr["SourceofFunds"]);
            this.OtherPurpose = "n/a";
            this.SpecialMessage = "Special Message";
            this.printed = false;

            applyCharges(prodChargID, this.GrossRemAmount, acctCur);
        }

        private void applyCharges(int prodChargID, double amount, int acctCur)
        {
            try
            {
                irsCalc = new irsCalculator(prodChargID, acctCur, true);
                irsCalc.compute_irsw_charges(Convert.ToDecimal(amount));

                this.GrossRemAmount = Convert.ToDouble(irsCalc.get_remittance_gross_branch());
                this.NetRemAmount = Convert.ToDouble(irsCalc.get_remittance_net_remit_amt());
                this.NettoBeneCurrency = acctCur;
                this.NettoBeneAmount = Convert.ToDouble(irsCalc.get_remittance_net_to_bene_amt());
                this.TotalLocalCharge = Convert.ToDouble(irsCalc.get_remittance_total_charges_amt());
                //this.ForexRemittingToAccount = Convert.ToDouble(irsCalc.get_forex_target_to_account());
                this.ForexRemittingToLocal = Convert.ToDouble(irsCalc.get_forex_target_to_source());
                this.ForexRemittingToUSD = Convert.ToDouble(irsCalc.get_forex_target_to_usd());
                this.ForexRemittingToPHP = Convert.ToDouble(irsCalc.get_forex_target_to_php());
                this.ForexRemittingToEUR = Convert.ToDouble(irsCalc.get_forex_target_to_eur());
                this.ForexLocalToRemitting = Convert.ToDouble(irsCalc.get_forex_source_to_target());
                this.ForexLocalToUSD = Convert.ToDouble(irsCalc.get_forex_source_to_usd());
                this.ForexLocalToPHP = Convert.ToDouble(irsCalc.get_forex_source_to_php());
            }
            catch (Exception e)
            {
                Message = "Error while getting charges data for this batch.\n\nPlease check charges for this channel.";
            }
        }

        private int getRecordStatus(string stat)
        {
            if (stat == "VALID") return 1;
            else return 0;
        }

        private void updateInventory(int SEQ_ID, int setStatus)
        {
            SqlCommand cmd = getSqlCommand("IF EXISTS(SELECT SEQ_ID FROM TBL_FILE_INVENTORY WHERE SEQ_ID = " + SEQ_ID.ToString()
                            + ") UPDATE TBL_FILE_INVENTORY "
                            + "SET Status = " + setStatus.ToString()
                            + ", DateUpdated = GETDATE()"
                            + ", DateSentToIRS = GETDATE()"
                            + " WHERE SEQ_ID = " + SEQ_ID.ToString(), false);

            int i = sqlUpdate(cmd);
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
        #endregion

        public irsProcessOtherChannels()
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }
    }
}
