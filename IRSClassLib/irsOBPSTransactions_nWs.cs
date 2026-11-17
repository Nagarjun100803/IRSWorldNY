using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections;

namespace IRSClassLib
{
    public class irsOBPSTransactions_nWs : sqlHelper
    {
        csAuditLog audit = new csAuditLog();
        public irsOBPSTransactions_nWs()
        {
            this.TableName = "tbl_remittance";
            this.IdentityColumn = "SEQ_ID";
        }

        public DataTable getOptionalField(string sp, string merchcode, int prodcode)
        {
            DataSet ds = execStoredProcedure(sp, new object[] { merchcode, prodcode });
            return ds.Tables[0];
        }

        public DataTable getEnrollMerchantInfo(Int32 SEQID, string RemitterID)
        {
            DataSet ds = execStoredProcedure("spGetEnrollMerchantInfo", new object[] { SEQID, RemitterID });
            return ds.Tables[0];
        }

        public DataTable getCharges(string brCode, short remitcurr, short accode, double targetamount)
        {
            DataTable dt = execStoredProcedureTable("spComputeOBPSCharges", new object[] { brCode, remitcurr, accode, targetamount });
            return dt;
        }

        public DataTable getCIFAccountCurrency(string merchantcode)
        {
            DataTable dt = execStoredProcedureTable("spGetCIFAccountCurrency", new object[] { merchantcode });
            return dt;
        }

        public DataTable getDefaults(string brcode)
        {
            DataSet ds = execStoredProcedure("spGetBranchDefaults", new object[] { brcode });//getDataSet(getSqlCommand("SELECT * FROM TBL_BRANCH_DEFAULTS WHERE BRANCHCODE = '" + brcode + "'", false));
            return ds.Tables[0];
        }

        public DataSet getLookUp(string fldGroup)
        {
            DataSet ds = getDataSet(getSqlCommand("SELECT FldValue,FldDescription,FldCode FROM TBL_LOOKUP WHERE  FldGroup = '" + fldGroup + "'", false));
            return ds;
        }

        public DataTable getRemittingCurrency(int productchargesid)
        {
            DataSet ds = getDataSet(getSqlCommand("select Currency,dbo.f_getlookupvalue('Currency',Currency) as CurrencyDesc FROM TBL_CHARGE_MASTER WHERE SEQ_ID = '" + productchargesid + "'", false));
            if (ds == null)
            {
                return null;
            }
            return ds.Tables[0];
        }


        public string getLookUpValue(string fldgroup, string fldvalue)
        {
            object objValue = execScalar("SELECT dbo.f_getLookUpValue('" + fldgroup + "','" + fldvalue + "')");
            return (string)objValue;

        }

        public string Validation(string merchcode, string prodcode, int noofcontrols, string strpara,
                                 string param)
        {

            string[] i = param.Split(',');

            object objValidation = execScalar("sp_validation", new string[] { merchcode,prodcode,noofcontrols.ToString(),strpara,i[0],i[1],
                                              i[2],i[3],i[4],i[5],i[6],i[7],i[8],i[9],i[10],i[11],i[12],i[13],i[14],i[15],i[16],i[17]});
            return (string)objValidation;

        }

        public DataTable getMerchantList(string param)
        {
            DataTable dt = execStoredProcedureTable("spGetMerchantList", new object[] { param });
            return dt;
        }

        public DataTable getTranxHistory(int SEQ_ID)
        {
            DataTable dt = execStoredProcedureTable("spGetOBPSTransactionHistory", new object[] { SEQ_ID, "" });
            return dt;
        }

        public DataTable getProductChargesID(int currency, string brcode)
        {
            DataTable dt = null;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                base.EnforceConstraint = false;
                dt = execStoredProcedureTable("spGetProductChargesIDByCurrency", new object[] { currency, brcode });
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "getProductChargesID", currency.ToString() + "-" + brcode, ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }



        public string get_tbl_lookup_description(string field_group, string field_value)
        {
            SqlCommand sqcommands = null;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM TBL_LOOKUP WHERE FldGroup = '" + field_group + "' AND FldValue = " + field_value, false);

                DataTable dt = base.getDataSet(sqcommands, "LOOKUP_DATA").Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToString(dt.Rows[0]["FldDescription"]).ToUpper();
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "get_tbl_lookup_description", field_group + "-" + field_value, ex.Message, csAuditLog.Category.Select);
                return string.Empty;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose(); sqcommands = null;
                }
            }
        }




        public bool save_merch_details(string refno, string merchantcode, int prodcode, string CIFID, string data_value, string merseqid)
        {
            irsCalculator irsCalc = (irsCalculator)System.Web.HttpContext.Current.Session["OBPSCalculator"];
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                string[] splitted_value;
                bool success;
                string[] pairvalue;
                int zero = 0;
                splitted_value = data_value.Split(',');
                string targetAmt = "0";
                string actualAmt = "0";

                int seq_id = Convert.ToInt32(this.TranxID);//dt.Rows.Count > 0 ? Convert.ToInt16(dt.Rows[0]["SEQ_ID"]) : zero;
                //DataTable dt = getTranxHistory(seq_id);//execStoredProcedureTable("spGetOBPSDataLatestInserted", new object[] { CIFID });
                refno = getTranxRefNo(seq_id);//dt.Rows.Count > 0 ? dt.Rows[0]["ReferenceNo"].ToString() : string.Empty;                
                this.TableName = "TBL_OBPS_MERCHANT_PRODUCT";
                base.getUpdateDS("spGetMerchantProductDetails", new object[] { refno, merchantcode, prodcode });
                DataTable dtCopy = table.Copy();
                bool isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);


                foreach (string y in splitted_value)
                {
                    if (y != string.Empty)
                    {
                        pairvalue = y.Split('=');

                        if (!isForUpdate)
                        {
                            if (base.newRow(sqlHelper.RETAIN_DATASET))
                            {
                                uprow["ReferenceNo"] = refno;
                                uprow["MerchantCode"] = merchantcode;
                                uprow["MerchantProductCode"] = prodcode;
                                uprow["FieldLabel"] = pairvalue[0]; //key
                                uprow["FieldValue"] = pairvalue[1].Replace("-", ""); //value
                                uprow["MerSeqID"] = merseqid;
                                table.Rows.Add(uprow);
                                if (pairvalue[0].ToString() == "Target Amount")
                                {
                                    targetAmt = pairvalue[1].ToString();
                                }
                                else if (pairvalue[0].ToString() == "Actual Amount")
                                {
                                    actualAmt = pairvalue[1].ToString();
                                }
                            }
                        }
                    }
                }
                if (seq_id != 0)
                {
                    irsCalc.commit_remittance_charge_details(seq_id);
                    bool ibool = new irsOverrideLog("", "OBPS").LogOverride(seq_id);
                }

                base.addMode = false;
                //return base.save(sqlHelper.RETAIN_DATASET);
                bool MdetailsSave = false;
                MdetailsSave = base.save(sqlHelper.RETAIN_DATASET);
                if (MdetailsSave == true & merchantcode == "SSS")
                {
                    return saveOBPExcess(refno, targetAmt, actualAmt);
                }
                return MdetailsSave;
            }
            catch (Exception e)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "Save OBPS Details", e.Message, data_value + "-" + refno, csAuditLog.Category.Insert);
                Message = e.Message;
            }
            finally
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "Save OBPS Details", "", data_value + "-" + refno, csAuditLog.Category.Insert);
            }
            return false;

        }

        public bool saveOBPExcess(string RefNo, string targetAmt, string actualAmt)
        {
            decimal excessAmt = 0;
            string ExcessAcount = "";
            string NSVS = "";
            string excessPayment = "";
            decimal nettobeneAmt = 0;
            decimal CovertedExcessAmount = 0;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            irsRemTeller rem = new irsRemTeller();
            DataTable dt = null;
            irsCurrencyRates ccyRates = new irsCurrencyRates();
            DateTime tmz = new irsTimeZone().branchLocalTime(clientdata.Location);
            string module = "";
            if (clientdata.IsArm == true)
            {
                module = "OBPS ARM";
            }
            else
            {
                module = "OBPS Transaction";
            }
            try
            {
                this.TableName = "tbl_obps_merchant_refno";
                ExcessAcount = getLookUp("OBP_ExcessPayment").Tables[0].Rows[0]["FldCode"].ToString();
                NSVS = rem.get_nsvs(clientdata.Location, 2);
                excessAmt = Convert.ToDecimal(actualAmt) - Convert.ToDecimal(targetAmt);
                dt = execStoredProcedureTable("spGetOBPSData", new object[] { RefNo });
                nettobeneAmt = Convert.ToDecimal(dt.Rows[0]["NettoBeneAmount"]);
                if (clientdata.Role != 10 && clientdata.Role != 25 && clientdata.Role != 7 && clientdata.Role != 27)
                {
                    CovertedExcessAmount = excessAmt * (1 / Convert.ToDecimal(ccyRates.getCurrencyRates(tmz.Date, clientdata.Location).Select("SourceCurrency = 2 and TargetCurrency = 1")[0]["SourceToTarget"]));
                }
                else
                {
                    string exrate = new irsInternetPage().getExchangeRate(clientdata.Location, "2", "1", true);
                    string[] rate = new string[exrate.Split(' ').Length];
                    rate = exrate.Split(' ');
                    CovertedExcessAmount = excessAmt * (1 / Convert.ToDecimal(rate[4].ToString()));
                }
                //OBPS|<OBP_ExcessPaymentAccount>|<NSVS USD>|<NetToBeneAmt>|<ConvertedExcessAmt USD>|<excessAmt>
                excessPayment = "OBPS|" + ExcessAcount + "|" + NSVS + "|" + Convert.ToString(Math.Round(nettobeneAmt, 2)) + "|" + Convert.ToString(Math.Round(CovertedExcessAmount, 2)) + "|" + Convert.ToString(Math.Round(excessAmt, 2));

                base.getUpdateDS("spOBPExcess", new object[] { dt.Rows[0]["SEQ_ID"].ToString() });
                DataTable dtCopy = table.Copy();

                bool isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);
                if (!isForUpdate)
                {
                    if (base.newRow(sqlHelper.RETAIN_DATASET))
                    {
                        uprow["MerchantRefNo"] = excessPayment;
                        uprow["RemittanceSEQ_ID"] = dt.Rows[0]["SEQ_ID"].ToString();
                        table.Rows.Add(uprow);
                    }
                    base.addMode = false;
                }
                return base.save(sqlHelper.RETAIN_DATASET);
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, module, "Save OBPS Excess", ex.Message, excessPayment + "-" + RefNo, csAuditLog.Category.Insert);
                throw ex;
            }
            finally
            {
                audit.writeAudit(clientdata.UserId, module, "Save OBPS Excess", "", excessPayment + "-" + RefNo, csAuditLog.Category.Insert);
                dt = null;
            }
            return false;
        }

        //private bool SaveMerchantRefNo(string CIFID, string brcode, string merchantcode, int remittance_seq_id)
        //{ 
        //    object merch_refno = execScalar("SP_SSSREFNO", new object[] { CIFID,brcode,"" });  //execScalar("SP_SSSREFNO", new object[] { CIFID });
        //    if (merchantcode.ToUpper() == "SSS")
        //    {
        //        base.getUpdateDataSet(spGetOBPSMerchantRefNoTable, new object[] { seq_id });
        //        if (base.newRow(sqlHelper.RETAIN_DATASET))
        //        { 
        //            uprow
        //        }
        //    }
        //}

        public bool SaveUpdates()
        {
            return base.save(sqlHelper.RETAIN_DATASET);
        }


        public DataSet GetBranches(string location)
        {
            sqlHelper commons = new sqlHelper();
            DataSet ds = commons.execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        public bool saveOBPS(string datapair, string userid)
        {
            try
            {
                string fieldLabel;
                string fieldValue;
                string[] splitted_value;
                string[] data_value;

                splitted_value = datapair.Split(',');
                foreach (string x in splitted_value)
                {
                    data_value = x.Split('=');
                    this.pushdata(data_value);
                }

                this.TableName = "tbl_remittance";
                base.getUpdateDS("spGetOBPSData", new object[] { this.ReferenceNo });
                DataTable dtCopy = table.Copy();

                bool isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);

                if (!isForUpdate)
                {
                    if (base.newRow(sqlHelper.RETAIN_DATASET))
                    {
                        uprow["UserID"] = userid;
                        uprow["RemitMethod"] = this.RemitMethod;
                        uprow["RemitStationID"] = this.RemStationID;
                        uprow["RemittingBranch"] = this.RemittingBranch;
                        uprow["ChargeLocally"] = this.Charge_Locally;
                        uprow["ReferenceNo"] = this.ReferenceNo;
                        uprow["ExtlReferenceNo"] = this.ExtReferenceNo;
                        uprow["Transtatus"] = this.TransStatus;
                        uprow["TranxDate"] = this.TranxDate;
                        uprow["ProductChargesID"] = this.ProductChargesID;
                        uprow["RemitterCIFID"] = this.RemitterCIFID;
                        uprow["BeneficiaryCIFID"] = this.BeneficiaryCIFID;
                        uprow["AccountSEQID"] = this.AccountSeqID;
                        uprow["RemittingCurrency"] = this.RemittingCurrency;
                        uprow["GrossRemAmount"] = this.GrossRemAmount;
                        uprow["NetRemAmount"] = this.NetRmtAmt;
                        uprow["NetToBeneCurrency"] = this.NetToBeneCurrency;
                        uprow["NetToBeneAmount"] = this.NetToBeneAmount;
                        uprow["TotalLocalCharge"] = this.TotalCharge;
                        uprow["TransactionSource"] = this.TransactionSource;
                        uprow["ReceiptMode"] = this.ReceiptMode;
                        uprow["TransactionPurpose"] = this.TransactionPurpose;
                        uprow["OtherPurpose"] = this.OtherPurpose;
                        uprow["SpecialMessage"] = this.SpecialMessage;
                        uprow["ForexRemittingToLocal"] = this.ForexRemittingToLocal;
                        uprow["ForexRemittingToEUR"] = this.ForexRemittingToEUR;
                        uprow["printed"] = this.Printed;
                        uprow["ForexLocalToRemitting"] = this.ForexLocalToRemitting;
                        uprow["ForexRemittingToUSD"] = this.ForexRemittingToUSD;
                        uprow["ForexRemittingToPHP"] = this.ForexRemittingToPHP;
                        uprow["ForexLocalToUSD"] = this.ForexLocalToUSD;
                        uprow["ForexLocalToPHP"] = this.ForexLocalToPHP;
                        uprow["ForexRemittingToAccount"] = this.ForexRemittingToAccount;
                        uprow["SourceofFunds"] = this.SourceOfFunds;
                        uprow["LocalCurrency"] = this.LocalCurrency;
                        uprow["DatePostedOverseas"] = this.DatePostedOverseas;
                        uprow["DateImplementedOverseas"] = this.DateImplementedOverseas;
                        uprow["DatePostedLocal"] = DateTime.Now;
                        uprow["DateImplementedLocal"] = DateTime.Now;
                        uprow["total_backend_charges"] = this.Total_Backend_Charges;
                        //uprow["NettoBeneAmountPHP"] = this.NettoBeneAmountPHP;
                        table.Rows.Add(uprow);
                    }
                    base.addMode = false;
                }
                if (base.save(sqlHelper.RETAIN_DATASET))
                {
                    this.TranxID = base.IdentityValue.ToString();
                    return true;
                }
            }

            catch (Exception ex)
            {
                audit.writeAudit(userid, "OBPS Transaction", "Save Transaction", ex.Message, datapair, csAuditLog.Category.Insert);
                throw ex;
            }
            finally
            {
                audit.writeAudit(userid, "OBPS Transaction", "Save Transaction", "", datapair + "-" + this.TranxID, csAuditLog.Category.Insert);
            }
            return false;
        }

        public bool saveOBPS(string datapair, string userid, string branch)
        {
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                string fieldLabel;
                string fieldValue;
                string[] splitted_value;
                string[] data_value;

                splitted_value = datapair.Split(',');
                foreach (string x in splitted_value)
                {
                    data_value = x.Split('=');
                    this.pushdata(data_value);
                }

                this.TableName = "tbl_remittance";
                base.getUpdateDS("spGetOBPSData", new object[] { this.ReferenceNo });
                DataTable dtCopy = table.Copy();

                bool isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);

                if (!isForUpdate)
                {
                    if (base.newRow(sqlHelper.RETAIN_DATASET))
                    {
                        uprow["UserID"] = userid;
                        uprow["RemitMethod"] = this.RemitMethod;
                        uprow["RemitStationID"] = this.RemStationID;
                        uprow["RemittingBranch"] = this.RemittingBranch;
                        uprow["ChargeLocally"] = this.Charge_Locally;
                        uprow["ReferenceNo"] = this.ReferenceNo;
                        uprow["ExtlReferenceNo"] = this.ExtReferenceNo;
                        uprow["Transtatus"] = this.TransStatus;
                        uprow["TranxDate"] = this.TranxDate;
                        uprow["ProductChargesID"] = this.ProductChargesID;
                        uprow["RemitterCIFID"] = this.RemitterCIFID;
                        uprow["BeneficiaryCIFID"] = this.BeneficiaryCIFID;
                        uprow["AccountSEQID"] = this.AccountSeqID;
                        uprow["RemittingCurrency"] = this.RemittingCurrency;
                        uprow["GrossRemAmount"] = this.GrossRemAmount;
                        uprow["NetRemAmount"] = this.NetRmtAmt;
                        uprow["NetToBeneCurrency"] = this.NetToBeneCurrency;
                        uprow["NetToBeneAmount"] = this.NetToBeneAmount;
                        uprow["TotalLocalCharge"] = this.TotalCharge;
                        uprow["TransactionSource"] = this.TransactionSource;
                        uprow["ReceiptMode"] = this.ReceiptMode;
                        uprow["TransactionPurpose"] = this.TransactionPurpose;
                        uprow["OtherPurpose"] = this.OtherPurpose;
                        uprow["SpecialMessage"] = this.SpecialMessage;
                        uprow["ForexRemittingToLocal"] = this.ForexRemittingToLocal;
                        uprow["ForexRemittingToEUR"] = this.ForexRemittingToEUR;
                        uprow["printed"] = this.Printed;
                        uprow["ForexLocalToRemitting"] = this.ForexLocalToRemitting;
                        uprow["ForexRemittingToUSD"] = this.ForexRemittingToUSD;
                        uprow["ForexRemittingToPHP"] = this.ForexRemittingToPHP;
                        uprow["ForexLocalToUSD"] = this.ForexLocalToUSD;
                        uprow["ForexLocalToPHP"] = this.ForexLocalToPHP;
                        uprow["ForexRemittingToAccount"] = this.ForexRemittingToAccount;
                        uprow["SourceofFunds"] = this.SourceOfFunds;
                        uprow["LocalCurrency"] = this.LocalCurrency;
                        uprow["DatePostedOverseas"] = this.DatePostedOverseas;
                        uprow["DateImplementedOverseas"] = this.DateImplementedOverseas;
                        uprow["DatePostedLocal"] = DateTime.Now;
                        uprow["DateImplementedLocal"] = DateTime.Now;
                        uprow["total_backend_charges"] = this.Total_Backend_Charges;
                        //uprow["NettoBeneAmountPHP"] = this.NettoBeneAmountPHP;
                        table.Rows.Add(uprow);
                    }
                    base.addMode = false;
                }
                return base.save(sqlHelper.RETAIN_DATASET);
            }

            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "Save Transaction", ex.Message, datapair, csAuditLog.Category.Insert);
                throw ex;
            }
            finally
            {
                this.TranxID = base.IdentityValue.ToString();
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "Save Transaction", "", datapair + "-" + this.TranxID, csAuditLog.Category.Insert);
            }
            return false;
        }

        public string get_calculator_computed_values()
        {
            if (System.Web.HttpContext.Current.Session["OBPSCalculator"] != null)
            {
                irsCalculator icalc = (irsCalculator)System.Web.HttpContext.Current.Session["OBPSCalculator"];

                StringBuilder s = new StringBuilder();

                s.Append(icalc.get_remittance_gross_branch() + "~"); //0                
                s.Append(icalc.get_remittance_net_branch() + "~"); //1
                s.Append(icalc.get_remittance_net_to_bene_amt() + "~"); //2
                s.Append(icalc.get_remittance_total_charges_amt() + "~"); //3
                s.Append(icalc.get_forex_source_to_target() + "~"); //4
                s.Append(icalc.get_forex_target_to_usd() + "~"); //5
                s.Append(icalc.get_forex_target_to_php() + "~"); //6
                s.Append(icalc.get_forex_source_to_usd() + "~"); //7
                s.Append(icalc.get_forex_source_to_php() + "~"); //8
                s.Append(icalc.get_forex_target_to_account() + "~"); //9
                s.Append(icalc.get_forex_target_to_source() + "~"); //10
                s.Append(icalc.get_forex_target_to_eur() + "~"); //11
                s.Append(icalc.get_branch_local_currency() + "~"); //12
                s.Append(icalc.get_total_backend_charges() + "~"); //13

                return s.ToString();
            }
            return string.Empty;
        }

        private void pushdata(string[] data_value)
        {
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                if (data_value[0] == "RemitMethod")
                {
                    this.RemitMethod = Convert.ToInt16(data_value[1]);
                }
                else if (data_value[0] == "RemitStationID")
                {
                    this.RemStationID = data_value[1];
                }
                else if (data_value[0] == "RemittingBranch")
                {
                    this.RemittingBranch = data_value[1];
                }
                else if (data_value[0] == "ReferenceNo")
                {
                    this.ReferenceNo = data_value[1];
                }
                else if (data_value[0] == "ExtReferenceNo")
                {
                    this.ExtReferenceNo = data_value[1];
                }
                else if (data_value[0] == "TransStatus")
                {
                    this.TransStatus = Convert.ToInt16(data_value[1]);
                }
                else if (data_value[0] == "TranxDate")
                {
                    this.TranxDate = Convert.ToDateTime(data_value[1]);
                }
                else if (data_value[0] == "ProductChargesID")
                {
                    this.ProductChargesID = (data_value[1].Trim() == string.Empty) ? (short)0 : Convert.ToInt64(data_value[1]);
                }
                else if (data_value[0] == "RemitterCIFID")
                {
                    this.RemitterCIFID = data_value[1].Trim();
                }
                else if (data_value[0] == "BeneficiaryCIFID")
                {
                    this.BeneficiaryCIFID = data_value[1].Trim();
                }
                else if (data_value[0] == "AccountSeqID")
                {
                    this.AccountSeqID = Convert.ToInt64(data_value[1]);
                }
                else if (data_value[0] == "RemittingCurrency")
                {
                    this.RemittingCurrency = Convert.ToInt16(data_value[1]);
                }
                else if (data_value[0] == "GrossRemAmount")
                {
                    this.GrossRemAmount = Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "NetRemAmt")
                {
                    this.NetRmtAmt = Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "NetToBeneCurrency")
                {
                    this.NetToBeneCurrency = Convert.ToInt16(data_value[1]);
                }
                else if (data_value[0] == "NetToBeneAmount")
                {
                    this.NetToBeneAmount = Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "TotalCharge")
                {
                    this.TotalCharge = Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "TransactionSource")
                {
                    this.TransactionSource = (data_value[1].Trim() == string.Empty) ? (short)0 : Convert.ToInt32(data_value[1]);
                }
                else if (data_value[0] == "ReceiptMode")
                {
                    this.ReceiptMode = (data_value[1].Trim() == string.Empty) ? (short)0 : Convert.ToInt32(data_value[1]);
                }
                else if (data_value[0] == "TransactionPurpose")
                {
                    this.TransactionPurpose = (data_value[1].Trim() == string.Empty) ? (short)0 : Convert.ToInt32(data_value[1]);
                }
                else if (data_value[0] == "OtherPurpose")
                {
                    this.OtherPurpose = data_value[1];
                }
                else if (data_value[0] == "SpecialMessage")
                {
                    this.SpecialMessage = data_value[1];
                }
                else if (data_value[0] == "DateImplementedOverseas")
                {
                    DateTime SqlNull = new DateTime(1900, 1, 1);

                    if (Convert.ToDateTime((data_value[1].Trim() == string.Empty) ? "1/1/1900" : data_value[1]) == SqlNull)
                    {
                        this.DateImplementedOverseas = DateTime.MinValue;
                    }
                    else
                    {
                        this.DateImplementedOverseas = Convert.ToDateTime(data_value[1]);
                    }
                }
                else if (data_value[0] == "DatePostedOverseas")
                {
                    DateTime SqlNull = new DateTime(1900, 1, 1);
                    if (Convert.ToDateTime((data_value[1].Trim() == string.Empty) ? "1/1/1900" : data_value[1]) == SqlNull)
                    {
                        this.DatePostedOverseas = DateTime.MinValue;
                    }
                    else
                    {
                        this.DatePostedOverseas = Convert.ToDateTime(data_value[1]);
                    }
                }
                else if (data_value[0] == "Printed")
                {
                    this.Printed = data_value[1].Trim() == "1" ? true : false;
                }
                else if (data_value[0] == "ForexLocalToRemitting")
                {
                    this.ForexLocalToRemitting = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "ForexRemittingToUSD")
                {
                    this.ForexRemittingToUSD = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "ForexRemittingToPHP")
                {
                    this.ForexRemittingToPHP = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "ForexLocalToUSD")
                {
                    this.ForexLocalToUSD = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "ForexLocalToPHP")
                {
                    this.ForexLocalToPHP = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "ForexRemittingToAccount")
                {
                    this.ForexRemittingToAccount = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "SourceOfFunds")
                {
                    this.SourceOfFunds = Convert.ToInt16(data_value[1]);
                }
                else if (data_value[0] == "ForexRemittingToLocal")
                {
                    this.ForexRemittingToLocal = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "ForexRemittingToEUR")
                {
                    this.ForexRemittingToEUR = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "LocalCurrency")
                {
                    this.LocalCurrency = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "total_backend_charges")
                {
                    this.Total_Backend_Charges = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
                else if (data_value[0] == "ChargeLocally")
                {
                    this.Charge_Locally = (data_value[1] == string.Empty) ? false : Convert.ToBoolean(Convert.ToInt16(data_value[1]));
                }
                else if (data_value[0] == "NettoBeneAmountPHP")
                {
                    this.NettoBeneAmountPHP = (data_value[1] == string.Empty) ? 0 : Convert.ToDecimal(data_value[1]);
                }
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "pushdata", "", ex.Message, csAuditLog.Category.Select);
                throw ex;
            }

        }

        public string OBPSRefno(string MerchantCode, string ProductCode, string branchCode)
        {
            DataTable dt = execStoredProcedureTable("usp_getspRefno", new object[] { MerchantCode });
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            string module = "";
            if (clientdata.IsArm == true)
            {
                module = "OBPS ARM";
            }
            else
            {
                module = "OBPS Transaction";
            }
            if (dt.Rows.Count > 0)
            {
                try
                {
                    DataTable dt2 = execStoredProcedureTable(dt.Rows[0][0].ToString(), new object[] { MerchantCode, branchCode, "" });
                    if (MerchantCode == "SSS" & ProductCode != "1")
                    {
                        return dt2.Rows[0][0].ToString().Substring(2, 3) + dt2.Rows[0][0].ToString().Substring(0, 2) + dt2.Rows[0][0].ToString().Substring(5, 3);
                    }
                    return dt2.Rows[0][0].ToString();
                }
                catch (Exception ex)
                {

                    audit.writeAudit(clientdata.UserId, module, "Get OBPSRefno", "", ex.Message, csAuditLog.Category.Select);
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        public DataTable OBPReceipt(string CIFID)
        {
            //DataTable dt = execStoredProcedureTable("spGetOBPSDataLatestInserted", new object[] { CIFID });
            irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                CIFID = (CIFID == "") ? this.TranxID : CIFID;
                audit.writeAudit(client_data.UserId, "OBPS Transaction", "OBPReceipt", "", CIFID, csAuditLog.Category.Select);
                return execStoredProcedureTable("sp_rptReceipt", new object[] { Convert.ToInt32(CIFID) });
            }
            catch (Exception ex)
            {
                audit.writeAudit(client_data.UserId, "OBPS Transaction", "OBPReceipt", ex.Message, CIFID, csAuditLog.Category.Select);
                return null;
            }
        }
        /*
                public ReportDocument GenerateOBPReceipt(ReportDocument obp, string trxID)
                {
                    irsReceipts receipt = new irsReceipts();
                    irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                    string header1 = "", header2 = "", sbranchname = "", sbranchaddr = "", sbranchcontactno = "",
                        sbranchemail = "", sID1 = "", sID2 = "", sID3 = "", sID4 = "";
                    ParameterDiscreteValue discreteVal = new ParameterDiscreteValue();
                    ParameterValues paramVal = new ParameterValues();

                    try
                    {
                        DataTable dtReceiptSetup = receipt.getReceipts(client_data.Location);
                        if (dtReceiptSetup.Rows.Count > 0)
                        {
                            header1 = dtReceiptSetup.Rows[0]["Header1"].ToString();
                            header2 = dtReceiptSetup.Rows[0]["Header2"].ToString();
                            sbranchname = dtReceiptSetup.Rows[0]["NameOfBranch"].ToString();
                            sbranchaddr = dtReceiptSetup.Rows[0]["Address"].ToString();
                            sbranchcontactno = dtReceiptSetup.Rows[0]["ContactNo"].ToString();
                            sbranchemail = dtReceiptSetup.Rows[0]["Email"].ToString();

                            DataSet dsValidID = new DataSet("ValidID");
                            dsValidID = receipt.GetValidID(client_data.Location);
                            if (dsValidID.Tables[0].Rows.Count > 0)
                            {
                                if ((int)dtReceiptSetup.Rows[0]["ValidID1"] != 0)
                                {
                                    sID1 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID1"])[0][2].ToString();
                                }
                                if ((int)dtReceiptSetup.Rows[0]["ValidID2"] != 0)
                                {
                                    sID2 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID2"])[0][2].ToString();
                                }
                                if ((int)dtReceiptSetup.Rows[0]["ValidID3"] != 0)
                                {
                                    sID3 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID3"])[0][2].ToString();
                                }
                                if ((int)dtReceiptSetup.Rows[0]["ValidID4"] != 0)
                                {
                                    sID4 = dsValidID.Tables[0].Select("Seq_id = " + (int)dtReceiptSetup.Rows[0]["ValidID4"])[0][2].ToString();
                                }
                            }
                        }

                        DataTable dt = OBPReceipt(trxID);
                        //System.Drawing.Printing.PrintDocument otemp = new System.Drawing.Printing.PrintDocument();
                        //System.Drawing.Printing.PrinterSettings                
                        obp.SetDataSource(dt);

                        discreteVal.Value = dt.Rows[0]["ClearingDate"].ToString();
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parClearingDate"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = header1;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parHeaderOne"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = header2;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parHeaderTwo"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sbranchname;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parBranchName"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sbranchaddr;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parBranchAddress"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sbranchcontactno;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parBranchContactNo"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sbranchemail;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parBranchEmailAdd"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sID1;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parID1"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sID2;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parID2"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sID3;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parID3"].ApplyCurrentValues(paramVal);

                        discreteVal.Value = sID4;
                        paramVal.Add(discreteVal);
                        obp.DataDefinition.ParameterFields["parID4"].ApplyCurrentValues(paramVal);

                        //rptReceiptOBP.Refresh();

                        //rptReceiptOBP.PrintToPrinter(1, true, 1, 1);
                        audit.writeAudit(client_data.UserId, "OBPS Transaction", "GenerateOBPReceipt", "", trxID, csAuditLog.Category.Select);
                        return obp;
                    }
                    catch (Exception ex)
                    {
                        audit.writeAudit(client_data.UserId, "OBPS Transaction", "GenerateOBPReceipt", ex.Message, trxID, csAuditLog.Category.Select);
                        throw ex;
                    }
                    return obp;
                }
                */
        public bool ValidID(string sIDNo, string MerchantCode, string sOthers)
        {
            sqlHelper commons = new sqlHelper();
            DataSet dsIsIDMandatory = getDataSet(getSqlCommand("select TOP 1 * FROM TBL_OPTIONALFIELD WHERE fieldname = 'id1' and MERCHANTCODE = '" + MerchantCode + "'", false));
            if ((bool)dsIsIDMandatory.Tables[0].Rows[0]["ismandatory"] == true)
            {
                DataSet dsGetSPValid = getDataSet(getSqlCommand("select * FROM TBL_SYSCODE WHERE FldRef = 'CHECKDIGIT' and CODE = '" + MerchantCode + "'", false));
                string sp_name = "";
                if (dsGetSPValid == null || dsGetSPValid.Tables[0].Rows.Count <= 0)
                {
                    return false;
                }
                else
                {
                    sp_name = dsGetSPValid.Tables[0].Rows[0]["Description"].ToString();
                    DataSet dsfSPExist = getDataSet(getSqlCommand("select TOP 1 * FROM sys.objects WHERE [name] = '" + sp_name.Trim() + "' and type_desc = 'SQL_STORED_PROCEDURE'", false));
                    if (dsfSPExist.Tables[0].Rows.Count > 0)
                    {
                        if (MerchantCode == "SSS")
                        {
                            DataTable dtSSS = execStoredProcedureTable(sp_name, new object[] { sIDNo, "" });
                            if (dtSSS.Rows[0][0].ToString() == "1")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (MerchantCode == "PAGIBIG")
                        {
                            sOthers = sOthers.Substring(1, sOthers.LastIndexOf('|') - 1);
                            DataSet ds = commons.execStoredProcedure("sp_Pay_pagibig", new object[] { sIDNo, sOthers, "" });
                            if (ds.Tables[1].Rows[0][0].ToString() == "1")
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            DataTable dtOthers = execStoredProcedureTable(sp_name, new object[] { sIDNo, sOthers, "" });
                            return (bool)dtOthers.Rows[0][0];
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
            return false;
        }

        public string getTranxSeqID(string sCIFID)
        {
            DataTable dt = null;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                if (sCIFID != "")
                {
                    dt = execStoredProcedureTable("spGetOBPSDataLatestInserted", new object[] { sCIFID });
                    audit.writeAudit(clientdata.UserId, "OBPS Transaction", "getTranxSeqID", "", sCIFID, csAuditLog.Category.Select);
                    return dt.Rows[0]["SEQ_ID"].ToString() + "|" + dt.Rows[0]["ReferenceNo"].ToString();
                }
                else
                {
                    return this.TranxID;
                }
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "getTranxSeqID", ex.Message, sCIFID, csAuditLog.Category.Select);
                return string.Empty;
            }
        }

        public string getTranxRefNo(int trxid)
        {
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            DataSet ds = null;
            try
            {
                ds = getDataSet(getSqlCommand("Select * from tbl_remittance Where SEQ_ID = " + trxid, false));
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "getTranxRefNo", "", trxid.ToString(), csAuditLog.Category.Select);
                return ds.Tables[0].Rows[0]["ReferenceNo"].ToString();
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "getTranxRefNo", ex.Message, trxid.ToString(), csAuditLog.Category.Select);
                return string.Empty;
            }
        }

        public bool fOBPExcess(int TranxID)
        {
            System.Web.HttpContext.Current.Session["OBPSExcess"] = "";
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("spOBPExcess", new object[] { TranxID });

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["MERCHANTCODE"].ToString() != "SSS")
                    {
                        return false;
                    }
                    System.Web.HttpContext.Current.Session["OBPSExcess"] = dt.Rows[0]["MerchantRefNo"].ToString();
                    audit.writeAudit(clientdata.UserId, "OBPS Transaction", "fOBPExcess", "", TranxID.ToString(), csAuditLog.Category.Select);
                    return true;
                }
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "fOBPExcess", ex.Message, TranxID.ToString(), csAuditLog.Category.Select);
                throw ex;
            }
            finally
            {
                dt.Dispose();
                dt = null;
            }
            return false;
        }

        public void UpdateMemberID(int SEQ_ID, string CIFID, string RemCIFID, string MemberID)
        {
            DataTable dt = null;
            ArrayList error_list = new ArrayList();
            this.MerCIFID = CIFID;
            this.RemCIFID = RemCIFID;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                this.TableName = "TBL_CIF_MERCHANTS";
                dt = base.getDataSet(base.getSqlCommand("SELECT * FROM TBL_CIF_MERCHANTS WHERE SEQ_ID = " +
                    SEQ_ID + " AND CIFID = '" + CIFID + "' AND RemitterCIFID = '" + RemCIFID + "'", false), "CIF_MERCHANTS").Tables[0];
                base.getUpdateDS("SELECT * FROM TBL_CIF_MERCHANTS WHERE SEQ_ID = " +
                    SEQ_ID + " AND CIFID = '" + CIFID + "' AND RemitterCIFID = '" + RemCIFID + "'");
                //base.getUpdateDataSet(getSqlCommand("SELECT * FROM TBL_CIF_MERCHANTS WHERE SEQ_ID = " +
                //    SEQ_ID + " AND CIFID = '" + CIFID + "' AND RemitterCIFID = '" + RemCIFID + "'", false));
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        base.selectedRow = 0;

                        if (base.edit(Convert.ToInt32(dt.Rows[i]["SEQ_ID"])) == true)
                        {
                            if (!base.setColumn("CIFID", Convert.ToString(CIFID))) { error_list.Add("CIFID: " + base.Message); }
                            if (!base.setColumn("RemitterCIFID", Convert.ToString(RemCIFID))) { error_list.Add("RemitterCIFID: " + base.Message); }
                            if (!base.setColumn("MerInfoSeqID", Convert.ToInt64(dt.Rows[i]["MerInfoSeqID"]))) { error_list.Add("MerInfoSeqID: " + base.Message); }
                            if (!base.setColumn("Deleted", dt.Rows[i]["Deleted"])) { error_list.Add("Deleted: " + base.Message); }
                            if (!base.setColumn("MemberIDNo", Convert.ToString(MemberID))) { error_list.Add("MemberIDNo: " + base.Message); }

                            if (base.save() == false)
                            {
                                error_list.Add("TBL_CIF_MERCHANTS - COMMIT FAILED - " + base.Message); base.cancel();
                            }
                            audit.writeAudit(clientdata.UserId, "OBPS Transaction - MemberID", "", MemberID, "", csAuditLog.Category.Update);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction - MemberID", ex.Message, MemberID, "", csAuditLog.Category.Update);
            }
        }

        public override void getUpdateDS(int id)
        {
            base.TableName = this.TableName;
            base.IdentityColumn = "SEQ_ID";
            //base.getUpdateDataSet(getSqlCommand("SELECT * FROM " + this.txn_db_table_name + " WHERE SEQ_ID = " + id, false));
            base.getUpdateDataSet(getSqlCommand("SELECT * FROM " + this.TableName + " WHERE SEQ_ID = " + id, false));
        }

        public Boolean fOBPAccount(string AccountNo)
        {
            bool isOBP = false;
            SqlCommand sqcommands = null;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                sqcommands = base.getSqlCommand("SELECT * FROM vwMerchantAccounts WHERE ACCOUNTNO = '" + AccountNo.Trim().Replace("\r", "") + "'", false);
                DataTable dt = base.getDataSet(sqcommands, "OBPS_ACCOUNT").Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    isOBP = true;
                }
                else
                {
                    isOBP = false;
                }
                audit.writeAudit(clientdata.UserId, "Check OBP Account", "", AccountNo, "", csAuditLog.Category.Select);
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "Check OBP Account", ex.Message, AccountNo, "", csAuditLog.Category.Select);
            }
            return isOBP;
        }

        public bool chkDefaults_charges(string brCode, string sourceCCy, string chargeType, string Paymode)
        {
            DataTable dt = null;
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("Select * from TBL_CHARGE_MASTER WHERE ProductCode = 10 and ImplementationType = 2 and BranchCode = '" + brCode.Trim() +
                    "' and ChargeType = '" + chargeType + "' and Currency = '" + sourceCCy + "' and Paymode ='" + Paymode + "'", false);
                dt = base.getDataSet(sqcommands, "Defaults_Charges").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable getRemittance(string ReferenceNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getOBPRemittance", new Object[] { ReferenceNo });
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

        public bool tranxstatusOK(string refno)
        {
            DataTable dt = null;
            SqlCommand sqcommands = null;
            try
            {
                sqcommands = base.getSqlCommand("Select Transtatus from TBL_Remittance WHERE referenceno = '" + refno.Trim() + "'", false);
                dt = base.getDataSet(sqcommands, "TranStaus").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) == 9)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #region private properties
        private string _userid;
        private String UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        private int _remitmethod;
        private int RemitMethod
        {
            get { return _remitmethod; }
            set { _remitmethod = value; }
        }

        private string _remstationid;
        private String RemStationID
        {
            get { return _remstationid; }
            set { _remstationid = value; }
        }

        private string _remittingbranch;
        private String RemittingBranch
        {
            get { return _remittingbranch; }
            set { _remittingbranch = value; }
        }

        private string _referenceno;
        private String ReferenceNo
        {
            get { return _referenceno; }
            set { _referenceno = value; }
        }

        private string _extreferenceno;
        private String ExtReferenceNo
        {
            get { return _extreferenceno; }
            set { _extreferenceno = value; }
        }

        private int _transstatus;
        private int TransStatus
        {
            get { return _transstatus; }
            set { _transstatus = value; }
        }

        private DateTime _tranxdate;
        private DateTime TranxDate
        {
            get { return _tranxdate; }
            set { _tranxdate = value; }
        }

        private long _productchargesid;
        private long ProductChargesID
        {
            get { return _productchargesid; }
            set { _productchargesid = value; }
        }

        private string _remittercifid;
        private String RemitterCIFID
        {
            get { return _remittercifid; }
            set { _remittercifid = value; }
        }

        private string _benecifid;
        private String BeneficiaryCIFID
        {
            get { return _benecifid; }
            set { _benecifid = value; }
        }

        private Int64 _accountseqid;
        private Int64 AccountSeqID
        {
            get { return _accountseqid; }
            set { _accountseqid = value; }
        }

        private int _remittingcurrency;
        private int RemittingCurrency
        {
            get { return _remittingcurrency; }
            set { _remittingcurrency = value; }
        }

        private decimal _grossremamount;
        private decimal GrossRemAmount
        {
            get { return _grossremamount; }
            set { _grossremamount = value; }
        }

        private int _nettobenecurrency;
        private int NetToBeneCurrency
        {
            get { return _nettobenecurrency; }
            set { _nettobenecurrency = value; }
        }

        private decimal _nettobeneamount;
        private Decimal NetToBeneAmount
        {
            get { return _nettobeneamount; }
            set { _nettobeneamount = value; }
        }

        private decimal _netrmtamt;
        private Decimal NetRmtAmt
        {
            get { return _netrmtamt; }
            set { _netrmtamt = value; }
        }

        private decimal _totalcharge;
        private Decimal TotalCharge
        {
            get { return _totalcharge; }
            set { _totalcharge = value; }
        }

        private int _transactionsource;
        private int TransactionSource
        {
            get { return _transactionsource; }
            set { _transactionsource = value; }
        }

        private int _receiptmode;
        private int ReceiptMode
        {
            get { return _receiptmode; }
            set { _receiptmode = value; }
        }

        private int _transactionpurpose;
        private int TransactionPurpose
        {
            get { return _transactionpurpose; }
            set { _transactionpurpose = value; }
        }

        private string _otherpurpose;
        private String OtherPurpose
        {
            get { return _otherpurpose; }
            set { _otherpurpose = value; }
        }

        private string _specialmessage;
        private String SpecialMessage
        {
            get { return _specialmessage; }
            set { _specialmessage = value; }
        }

        private DateTime _dateimplementedoverseas;
        private DateTime DateImplementedOverseas
        {
            get { return _dateimplementedoverseas; }
            set { _dateimplementedoverseas = value; }
        }

        private DateTime _datepostedoverseas;
        private DateTime DatePostedOverseas
        {
            get { return _datepostedoverseas; }
            set { _datepostedoverseas = value; }
        }

        private bool _printed;
        private bool Printed
        {
            get { return _printed; }
            set { _printed = value; }
        }

        private decimal _forexlocaltoremitting;
        private Decimal ForexLocalToRemitting
        {
            get { return _forexlocaltoremitting; }
            set { _forexlocaltoremitting = value; }
        }

        private decimal _forexremittingtousd;
        private Decimal ForexRemittingToUSD
        {
            get { return _forexremittingtousd; }
            set { _forexremittingtousd = value; }
        }

        private decimal _forexremittingtophp;
        private Decimal ForexRemittingToPHP
        {
            get { return _forexremittingtophp; }
            set { _forexremittingtophp = value; }
        }

        private decimal _forexlocaltousd;
        private Decimal ForexLocalToUSD
        {
            get { return _forexlocaltousd; }
            set { _forexlocaltousd = value; }
        }

        private decimal _forexlocaltophp;
        private Decimal ForexLocalToPHP
        {
            get { return _forexlocaltophp; }
            set { _forexlocaltophp = value; }
        }

        private decimal _forexremittingtoaccount;
        private Decimal ForexRemittingToAccount
        {
            get { return _forexremittingtoaccount; }
            set { _forexremittingtoaccount = value; }
        }

        private int _sourceoffunds;
        private int SourceOfFunds
        {
            get { return _sourceoffunds; }
            set { _sourceoffunds = value; }
        }

        private decimal _forexremittingtolocal;
        private Decimal ForexRemittingToLocal
        {
            get { return _forexremittingtolocal; }
            set { _forexremittingtolocal = value; }
        }

        private decimal _forexremittingtoeur;
        private Decimal ForexRemittingToEUR
        {
            get { return _forexremittingtoeur; }
            set { _forexremittingtoeur = value; }
        }

        private decimal _localcurrency;
        private Decimal LocalCurrency
        {
            get { return _localcurrency; }
            set { _localcurrency = value; }
        }

        private string _tranxid;
        public String TranxID
        {
            get { return _tranxid; }
            set { _tranxid = value; }
        }

        private decimal _total_backend_charges;
        private Decimal Total_Backend_Charges
        {
            get { return _total_backend_charges; }
            set { _total_backend_charges = value; }
        }

        private bool _charge_locally;
        private Boolean Charge_Locally
        {
            get { return _charge_locally; }
            set { _charge_locally = value; }
        }

        private decimal _nettobeneamountphp;
        private Decimal NettoBeneAmountPHP
        {
            get { return _nettobeneamountphp; }
            set { _nettobeneamountphp = value; }
        }

        private string _mercifid;
        private String MerCIFID
        {
            get { return _mercifid; }
            set { _mercifid = value; }
        }

        private string _remcifid;
        private String RemCIFID
        {
            get { return _remcifid; }
            set { _remcifid = value; }
        }
        #endregion
    }
}
