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
    public class csPromotions //: sqlHelper
    {
        csPromotions.PromotionProd pprod = new csPromotions.PromotionProd();
        csPromotions.PromotionBackup pback = new csPromotions.PromotionBackup();

        public enum Category
        {
            Logon,
            Logoff,
            Select,
            Insert,
            Update,
            Delete,
            Email,
            File
        }

        [Serializable()]
        public class PromotionProd : sqlHelper
        {
            sqlHelper dbProd = new sqlHelper("ConnectionString");
            irsSqlCommons irscomProd = new irsSqlCommons("ConnectionString");

            public PromotionProd()
            {
                dbProd.IsWinApp = true;
                //dbProd.constring_noRunmode = true;

            }

            private void setBase(string SQL, bool IsAdd)
            {
                dbProd.getUpdateDataSet(getSqlCommand(SQL, false));
                if (!dbProd.hasRows)
                {
                    dbProd.newRow(sqlHelper.RETAIN_DATASET);
                }
                else
                {
                    if (IsAdd)
                    {
                        dbProd.newRow(sqlHelper.RETAIN_DATASET);
                    }
                    else
                    {
                        dbProd.selectedRow = 0;
                    }
                }
            }

            public void setRecordToCounted(int SEQ_ID, string param)
            {
                bool ok = false;

                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsComputation WHERE SEQ_ID = '" + SEQ_ID + "'", false);
                dbProd.setColumn("IsComputed", true);
                ok = dbProd.save(sqlHelper.RETAIN_DATASET);

                writeAudit("IRSWebAgent", "irsPromotion: UPDATE TRANSACTION SET TO COUNTED", "Method:updateTransactionsSetToCounted - " + param, "Update Response: " + ok, string.Empty, csPromotions.Category.Update);
            }

            public bool insertToVoucherTable(int PromoCodeSEQID, string VoucherNumber, string RemitterID, int IsSent, string State, string Email, string RemitterName, string PromoCode, int QualifiedTransaction, int totalVouchersToSend)
            {
                bool ok = false;

                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsVoucher", true);
                dbProd.setColumn("PromoCodeSEQID", PromoCodeSEQID);
                dbProd.setColumn("VoucherNumber", VoucherNumber);
                dbProd.setColumn("RemitterID", RemitterID);
                dbProd.setColumn("IsSent", IsSent);
                dbProd.setColumn("State", State);
                dbProd.setColumn("Email", Email);
                dbProd.setColumn("RemitterName", RemitterName);
                dbProd.setColumn("PromoCode", PromoCode);
                dbProd.setColumn("QualifiedTransaction", QualifiedTransaction);
                ok = dbProd.save(sqlHelper.RETAIN_DATASET);
                if (ok == true)
                {
                    updateVoucherSeries(); //Add 1 to the voucher series reference

                    writeAudit("IRSWebAgent", "irsPromotion: INSERT TO VOUCHERS TABLE", "Method:insertToVouchersTable - PromoCode=" + PromoCode + "|PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|IsSent=" + IsSent + "|totalVouchersToSend=" + totalVouchersToSend + "|State=" + State + "|Email=" + Email + "|RemitterName=" + RemitterName + "|QualifiedTransction=" + QualifiedTransaction + "|VoucherNumber=" + VoucherNumber, "Insert Response: " + ok, string.Empty, csPromotions.Category.Insert);
                }
                else
                {
                    writeAudit("IRSWebAgent", "irsPromotion: INSERT TO VOUCHERS TABLE", "FAILED: Method:insertToVouchersTable - PromoCode=" + PromoCode + "|PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|IsSent=" + IsSent + "|totalVouchersToSend=" + totalVouchersToSend + "|State=" + State + "|Email=" + Email + "|RemitterName=" + RemitterName + "|QualifiedTransction=" + QualifiedTransaction + "|VoucherNumber=" + VoucherNumber, "Insert Response: " + ok, string.Empty, csPromotions.Category.Insert);
                }
                return ok;
            }

            public void updateVoucherSeries()
            {
                bool ok = false;
                int val = getVoucherSeries() + 1;
                try
                {
                    this.setBase("SELECT TOP 1 * FROM tbl_PromotionsVoucherReference", false);
                    dbProd.setColumn("VoucherSequenceNo", val);
                    ok = dbProd.save(sqlHelper.RETAIN_DATASET);

                }
                catch (Exception ex)
                {
                    writeAudit("IRSWebAgent", "irsPromotion: updateVoucherSeries", "ERROR Method:updateVoucherSeries: VoucherSequenceNo = " + val + "| - ERROR: " + ex.Message, ex.StackTrace, ex.Source, csPromotions.Category.Insert);
                }
            }

            public int getVoucherSeries()
            {
                dbProd.EnforceConstraint = false;

                dbProd.getUpdateDS("vsp_PromoGetVoucherSeries", new Object[] { });
                if (dbProd.hasRows)
                {
                    selectedRow = 0;
                    return (int)dbProd.table.Rows[0]["VoucherSequenceNo"];
                }
                else
                {
                    return 0;
                }
            }

            public void updateVoucherToSent(string VoucherNumber)
            {
                dbProd.addMode = false;
                bool ok = false;
                try
                {
                    this.setBase("SELECT TOP 1 * FROM tbl_PromotionsVoucher WHERE IsSent = 0 and VoucherNumber = '" + VoucherNumber + "'", false);
                    dbProd.setColumn("IsSent", 1);
                    dbProd.setColumn("VoucherSentDate", DateTime.Now);
                    ok = dbProd.save(sqlHelper.RETAIN_DATASET);
                }
                catch (Exception ex)
                {
                    return;  
                    
                }
                
                return;
                
            }

            public bool writeAudit(string UserID, string Module, string Description, string NewValue, string OldValue, Category Category)
            {
                string[] arrAudit = new string[7];
                string sUserID = string.Empty;
                int iReturn = 0;

                if (UserID == string.Empty)
                {
                    if (dbProd.IsWinApp)
                    {
                        sUserID = "IRSWebAgent";
                    }
                }
                else
                {
                    sUserID = UserID;
                }
                sUserID = (sUserID == null) ? string.Empty : sUserID;

                try
                {
                    arrAudit[0] = "0";
                    arrAudit[1] = sUserID;
                    arrAudit[2] = Module;
                    arrAudit[3] = Description;
                    arrAudit[4] = NewValue;
                    arrAudit[5] = OldValue;
                    arrAudit[6] = Category.ToString();

                    iReturn = irscomProd.execStoredProcedureUpdate("sp_Insert_Log", arrAudit);
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

                }
            }
        }

        [Serializable()]
        public class PromotionBackup : sqlHelper
        {
            sqlHelper dbBackup = new sqlHelper("BackupConnectionString");
            irsSqlCommons irscom = new irsSqlCommons("BackupConnectionString");

            public PromotionBackup()
            {
                dbBackup.IsWinApp = true;
            }

            private void setBase(string SQL, bool IsAdd)
            {
                dbBackup.getUpdateDataSet(getSqlCommand(SQL, false));
                if (!dbBackup.hasRows)
                {
                    dbBackup.newRow(sqlHelper.RETAIN_DATASET);
                }
                else
                {
                    if (IsAdd)
                    {
                        dbBackup.newRow(sqlHelper.RETAIN_DATASET);
                    }
                    else
                    {
                        dbBackup.selectedRow = 0;
                    }
                }
            }


            public DataTable getRecordsForCounting()
            {
                dbBackup.EnforceConstraint = false;
                dbBackup.getUpdateDS("vsp_PromoGetTotalCount", new Object[] { });
                if (dbBackup.hasRows)
                {
                    return dbBackup.table;
                }
                else
                {
                    return null;
                }
            }

            public DataTable getRecordsToBeSetToCounted(int limit, string RemitterID, int PromoCodeSEQID, string State)
            {
                dbBackup.EnforceConstraint = false;
                dbBackup.getUpdateDS("vsp_PromoGetListSetToComputed", new Object[] { limit, RemitterID, PromoCodeSEQID, State });
                if (dbBackup.hasRows)
                {
                    return dbBackup.table;
                }
                else
                {
                    return null;
                }
            }

            public void setRecordToCounted(int SEQ_ID, string param)
            {
                bool ok = false;

                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsComputation WHERE SEQ_ID = '" + SEQ_ID + "'", false);
                dbBackup.setColumn("IsComputed", true);
                ok = dbBackup.save(sqlHelper.RETAIN_DATASET);

                writeAudit("IRSWebAgent", "irsPromotion: UPDATE TRANSACTION SET TO COUNTED", "Method:updateTransactionsSetToCounted - " + param, "Update Response: " + ok, string.Empty, csPromotions.Category.Update);
            }

            public bool insertToVoucherTable(int PromoCodeSEQID, string VoucherNumber, string RemitterID, int IsSent, string State, string Email, string RemitterName, string PromoCode, int QualifiedTransaction, int totalVouchersToSend)
            {
                bool ok = false;

                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsVoucher", true);
                dbBackup.setColumn("PromoCodeSEQID", PromoCodeSEQID);
                dbBackup.setColumn("VoucherNumber", VoucherNumber);
                dbBackup.setColumn("RemitterID", RemitterID);
                dbBackup.setColumn("IsSent", IsSent);
                dbBackup.setColumn("State", State);
                dbBackup.setColumn("Email", Email);
                dbBackup.setColumn("RemitterName", RemitterName);
                dbBackup.setColumn("PromoCode", PromoCode);
                dbBackup.setColumn("QualifiedTransaction", QualifiedTransaction);
                ok = dbBackup.save(sqlHelper.RETAIN_DATASET);
                if (ok == true)
                {
                    updateVoucherSeries();
                    writeAudit("IRSWebAgent", "irsPromotion: INSERT TO VOUCHERS TABLE", "Method:insertToVouchersTable - PromoCode=" + PromoCode + "|PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|IsSent=" + IsSent + "|totalVouchersToSend=" + totalVouchersToSend + "|State=" + State + "|Email=" + Email + "|RemitterName=" + RemitterName + "|QualifiedTransction=" + QualifiedTransaction, "Insert Response: " + ok, string.Empty, csPromotions.Category.Insert);
                }
                else
                {
                    writeAudit("IRSWebAgent", "irsPromotion: INSERT TO VOUCHERS TABLE", "FAILED: Method:insertToVouchersTable - PromoCode=" + PromoCode + "|PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|IsSent=" + IsSent + "|totalVouchersToSend=" + totalVouchersToSend + "|State=" + State + "|Email=" + Email + "|RemitterName=" + RemitterName + "|QualifiedTransction=" + QualifiedTransaction, "Insert Response: " + ok, string.Empty, csPromotions.Category.Insert);
                }
                return ok;
            }

            public void updateVoucherSeries()
            {
                bool ok = false;
                int val = getVoucherSeries() + 1;
                try
                {
                    this.setBase("SELECT TOP 1 * FROM tbl_PromotionsVoucherReference", false);
                    dbBackup.setColumn("VoucherSequenceNo", val);
                    ok = dbBackup.save(sqlHelper.RETAIN_DATASET);

                }
                catch (Exception ex)
                {
                    writeAudit("IRSWebAgent", "irsPromotion: updateVoucherSeries", "ERROR Method:updateVoucherSeries: VoucherSequenceNo = " + val + "| - ERROR: " + ex.Message, ex.StackTrace, ex.Source, csPromotions.Category.Insert);
                }
            }

            public string generateVoucherNumber(string PromoCode)
            {
                string vnum = "";
                try
                {
                    string dateToday = DateTime.Today.ToString("MMddyy");
                    int voucherSeries = getVoucherSeries() + 1;
                    vnum = PromoCode + dateToday + voucherSeries.ToString("000000");//D6 Format
                }
                catch (Exception ex)
                {
                    writeAudit("IRSWebAgent", "irsPromotion: generateVoucherNumber", "ERROR: PromoCode = " + PromoCode + "| Method:generateVoucherNumber - error in generating Voucher Number - ERROR: " + ex.Message, ex.StackTrace, ex.Source, csPromotions.Category.Insert);
                }

                return vnum;
            }

            public int getVoucherSeries()
            {
                dbBackup.EnforceConstraint = false;
                dbBackup.getUpdateDS("vsp_PromoGetVoucherSeries", new Object[] { });
                if (dbBackup.hasRows)
                {
                    dbBackup.selectedRow = 0;
                    return (int)dbBackup.table.Rows[0]["VoucherSequenceNo"];
                }
                else
                {
                    return 0;
                }
            }

            public DataTable vouchersForCreation()
            {
                dbBackup.EnforceConstraint = false;
                dbBackup.getUpdateDS("vsp_PromoGetVoucherInfo", new Object[] { });
                if (dbBackup.hasRows)
                {
                    return dbBackup.table;
                }
                else
                {
                    return null;
                }
            }

            public DataTable getVoucherInfo(string _VoucherNumber)
            {
                dbBackup.EnforceConstraint = false;
                dbBackup.getUpdateDS("vsp_CreateVoucher", new Object[] { _VoucherNumber });
                if (dbBackup.hasRows)
                {
                    return dbBackup.table;
                }
                else
                {
                    return null;
                }
            }

            public bool updateVoucherToSent(string VoucherNumber)
            {
                dbBackup.addMode = false;
                bool ok = false;

                {
                    this.setBase("SELECT TOP 1 * FROM tbl_PromotionsVoucher WHERE IsSent = 0 and VoucherNumber = '" + VoucherNumber + "'", false);
                    dbBackup.setColumn("IsSent", 1);
                    dbBackup.setColumn("VoucherSentDate", DateTime.Now);
                    ok = dbBackup.save(sqlHelper.RETAIN_DATASET);
                    return ok;
                }
            }

            public bool writeAudit(string UserID, string Module, string Description, string NewValue, string OldValue, Category Category)
            {
                string[] arrAudit = new string[7];
                string sUserID = string.Empty;
                int iReturn = 0;

                if (UserID == string.Empty)
                {
                    if (dbBackup.IsWinApp)
                    {
                        sUserID = "IRSWebAgent";
                    }
                }
                else
                {
                    sUserID = UserID;
                }
                sUserID = (sUserID == null) ? string.Empty : sUserID;

                try
                {
                    arrAudit[0] = "0";
                    arrAudit[1] = sUserID;
                    arrAudit[2] = Module;
                    arrAudit[3] = Description;
                    arrAudit[4] = NewValue;
                    arrAudit[5] = OldValue;
                    arrAudit[6] = Category.ToString();

                    iReturn = irscom.execStoredProcedureUpdate("sp_Insert_Log", arrAudit);
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

                }
            }
        }

        public bool startCounting()
        {
            try
            {
                #region Method Variable

                DataTable dt = new DataTable();
                int i = 0; //for loop counter
                int PromoCodeSEQID = 0;
                string PromoCode = "";
                string RemitterID = "";
                string State = "";
                int PromoEntries = 0;
                string Email = "";
                string RemitterName = "";
                int MinTransactionCount = 0;
                int totalVouchersToSend = 0;
                int totalEntriesToUpdateSetToCounted = 0;

                #endregion

                dt = pback.getRecordsForCounting();

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //Assign Values
                        PromoCodeSEQID = (int)dt.Rows[i]["PromoCodeSEQID"];
                        PromoCode = (string)dt.Rows[i]["PromoCode"];
                        RemitterID = (string)dt.Rows[i]["RemitterID"];
                        State = (string)dt.Rows[i]["State"];
                        PromoEntries = (int)dt.Rows[i]["PromoEntries"];
                        MinTransactionCount = (int)dt.Rows[i]["MinTransactionCount"];
                        Email = (string)dt.Rows[i]["Email"];
                        RemitterName = (string)dt.Rows[i]["RemitterName"];
                        totalVouchersToSend = PromoEntries / MinTransactionCount;

                        totalEntriesToUpdateSetToCounted = totalVouchersToSend * MinTransactionCount;
                        if (totalVouchersToSend > 0)
                        //Update Transaction to Counted AND Insert To Vouchers Table for Creation
                        {
                            if (updateTransactionsSetToCounted(PromoCodeSEQID, RemitterID, State, totalEntriesToUpdateSetToCounted))
                            {
                                insertToVouchersTable(PromoCode, PromoCodeSEQID, RemitterID, 0, totalVouchersToSend, State, Email, RemitterName, totalEntriesToUpdateSetToCounted);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pback.writeAudit("IRSWebAgent", "irsPromotion: COUNTING VALID ENTRIES", "ERROR: Method: startCounting - ERROR: " + ex.Message, ex.StackTrace, ex.Source, Category.Select);
                pprod.writeAudit("IRSWebAgent", "irsPromotion: COUNTING VALID ENTRIES", "ERROR: Method: startCounting - ERROR: " + ex.Message, ex.StackTrace, ex.Source, Category.Select);
                return false;
            }
            return true;
        }

        public bool updateTransactionsSetToCounted(int PromoCodeSEQID, string RemitterID, string State, int totalEntriesToUpdateSetToCounted)
        {
            DataTable dt = new DataTable();
            int SEQ_ID;
            string param = "";
            try
            {
                //Get records from BACKUP database
                dt = pback.getRecordsToBeSetToCounted(totalEntriesToUpdateSetToCounted, RemitterID, PromoCodeSEQID, State);
                if (dt != null)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SEQ_ID = (int)dt.Rows[i]["SEQ_ID"];
                        param = "PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|State=" + State + "|totalEntriesToUpdateSetToCounted=" + totalEntriesToUpdateSetToCounted + "|SEQ_ID = '" + dt.Rows[i]["SEQ_ID"] + "'";
                        pback.setRecordToCounted(SEQ_ID, param);
                        updateTransactionsSetToCountedPROD(SEQ_ID, param);
                    }
                }
            }
            catch (Exception ex)
            {
                pback.writeAudit("IRSWebAgent", "irsPromotion: UDATE TRANSACTION TO COUNTED-", "ERROR: Method: updateTransactionsSetToCounted - PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|State=" + State + "|totalEntriesToUpdateSetToCounted=" + totalEntriesToUpdateSetToCounted + "- ERROR: " + ex.Message, ex.StackTrace, ex.Source, Category.Update);
                
                return false;
            }
            return true;
        }

        public void updateTransactionsSetToCountedPROD(int SEQ_ID, string param)
        {
            try
            {
                pprod.setRecordToCounted(SEQ_ID, param);
            }
            catch (Exception ex)
            {
                pprod.writeAudit("IRSWebAgent", "irsPromotion: UDATE TRANSACTION TO COUNTED-", "ERROR: Method: updateTransactionsSetToCounted - Parameters:" + param  + "- ERROR: " + ex.Message, ex.StackTrace, ex.Source, Category.Update);
                return;
            }
            return;
        }

        public bool insertToVouchersTable(string PromoCode, int PromoCodeSEQID, string RemitterID, int IsSent, int totalVouchersToSend, string State, string Email, string RemitterName, int QualifiedTransaction)
        {
            string voucherNumber = "";
            try
            {
                for (int x = 0; x < totalVouchersToSend; x++)
                {
                    voucherNumber = pback.generateVoucherNumber(PromoCode);
                    pback.insertToVoucherTable(PromoCodeSEQID, voucherNumber, RemitterID, IsSent, State, Email, RemitterName, PromoCode, QualifiedTransaction, totalVouchersToSend);
                    insertToVouchersTablePROD(PromoCodeSEQID, voucherNumber, RemitterID, IsSent, State, Email, RemitterName, PromoCode, QualifiedTransaction, totalVouchersToSend);
                }

            }
            catch (Exception ex)
            {
                pback.writeAudit("IRSWebAgent", "irsPromotion: INSERT TO VOUCHERS TABLE", "ERROR: Method:insertToVouchersTable - PromoCode=" + PromoCode + "|PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|IsSent=" + IsSent + "|totalVouchersToSend=" + totalVouchersToSend + "|State=" + State + "|Email=" + Email + "|RemitterName=" + RemitterName + "|QualifiedTransction=" + QualifiedTransaction + "- ERROR: " + ex.Message, ex.StackTrace, ex.Source, Category.Insert);
                
            }
            return true;
        }

        public void insertToVouchersTablePROD(int PromoCodeSEQID, string voucherNumber, string RemitterID, int IsSent, string State, string Email, string RemitterName, string PromoCode, int QualifiedTransaction, int totalVouchersToSend)
        {
            try
            {
                pprod.insertToVoucherTable(PromoCodeSEQID, voucherNumber, RemitterID, IsSent, State, Email, RemitterName, PromoCode, QualifiedTransaction, totalVouchersToSend);
            }
            catch (Exception ex)
            {
                pprod.writeAudit("IRSWebAgent", "irsPromotion: INSERT TO VOUCHERS TABLE", "ERROR: Method:insertToVouchersTable - PromoCode=" + PromoCode + "|PromoCodeSEQID=" + PromoCodeSEQID + "|RemitterID=" + RemitterID + "|IsSent=" + IsSent + "|totalVouchersToSend=" + totalVouchersToSend + "|State=" + State + "|Email=" + Email + "|RemitterName=" + RemitterName + "|QualifiedTransction=" + QualifiedTransaction + "- ERROR: " + ex.Message, ex.StackTrace, ex.Source, Category.Insert);
                return;
            }
            return;
        }

        public bool updateVoucherToSent(string VoucherNumber)
        {
            pback.updateVoucherToSent(VoucherNumber);
            pprod.updateVoucherToSent(VoucherNumber);
            return true;
        }



        ////***********************************************
        //public csPromotions()
        //{
        //    base.IsWinApp = true;
        //}
        ////***********************************************

        //private void setBase(string SQL, bool IsAdd)
        //{
        //    base.getUpdateDataSet(getSqlCommand(SQL, false));
        //    if (!base.hasRows)
        //    {
        //        base.newRow(sqlHelper.RETAIN_DATASET);
        //        //selectedRow = 0;
        //    }
        //    else
        //    {
        //        if (IsAdd)
        //        {
        //            base.newRow(sqlHelper.RETAIN_DATASET);
        //        }
        //        else
        //        {
        //            selectedRow = 0;
        //        }
        //    }
        //}
    }
}
