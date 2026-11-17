using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections;
using System.Xml;

namespace IRSClassLib
{
    [Serializable]
    public class irsOBPSTransactions : sqlHelperWs
    {
        csAuditLog audit = new csAuditLog();
        public irsOBPSTransactions()
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
            DataSet ds = execStoredProcedure("spGetBranchDefaults", new object[] { brcode });
            return ds.Tables[0];
        }

        public DataSet getLookUp(string fldGroup)
        {
            DataSet ds = getDataSet("SELECT FldGroup,FldValue,FldDescription,FldCode FROM TBL_LOOKUP WHERE  FldGroup = '" + fldGroup + "'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
            return ds;
        }

        public DataTable getRemittingCurrency(int productchargesid)
        {
            DataSet ds = getDataSet("select Currency,dbo.f_getlookupvalue('Currency',Currency) as CurrencyDesc FROM TBL_CHARGE_MASTER WHERE SEQ_ID = '" + productchargesid + "'", false, new object[] { }); //Performance consideration when using irsSqlCommonsWS and sqlHelperWS

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

        public string Validation(string merchcode, string prodcode, int noofcontrols, string strpara, string param)
        {
            string[] i = param.Split(',');

            object objValidation = execScalar("sp_validation", new string[] { merchcode,prodcode,noofcontrols.ToString(),strpara,i[0],i[1],
                                              i[2],i[3],i[4],i[5],i[6],i[7],i[8],i[9],i[10],i[11],i[12],i[13],i[14],i[15],i[16],i[17]});
            return (string)objValidation;
        }

        public DataTable getMerchantList(string param)
        {
            DataTable dt = getDataSet("select TOP 1 * from tbl_cif_merchants where remittercifid = '" + param + "' and ISNULL(Deleted,0) = 0", false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
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
                DataTable dt = base.getDataSet("SELECT * FROM TBL_LOOKUP WHERE FldGroup = '" + field_group + "' AND FldValue = " + field_value, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

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
            DataSet dsbayadcenter = null;
            DataSet dsgenerika = null;

            string isbayadcenter = (string)System.Web.HttpContext.Current.Session["OBPSMerchantType"];

            try
            {
                string[] splitted_value;
                string[] pairvalue;
                splitted_value = data_value.Split(',');
                string targetAmt = "0";
                string actualAmt = "0";

                int seq_id = Convert.ToInt32(this.TranxID);

                refno = getTranxRefNo(seq_id);
                this.TableName = "TBL_OBPS_MERCHANT_PRODUCT";
                base.getUpdateDS("spGetMerchantProductDetails", new object[] { refno, merchantcode, prodcode });

                DataTable dtCopy = table.Copy();

                bool isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);
                string bayadcenterrequest = "";
                string generikarequest = "";

                if (isbayadcenter.ToUpper() == "BAYADCENTER")
                {
                    dsbayadcenter = getLookUp("BayadCenterAPI_Request");

                    if (dsbayadcenter.Tables[0].Rows.Count > 0)
                    {
                        bayadcenterrequest = dsbayadcenter.Tables[0].Select("FldCode ='" + merchantcode + "'")[0][2].ToString();
                    }

                    bayadcenterrequest = bayadcenterrequest.Replace("[merrefno]", refno);
                }

                if (merchantcode.ToUpper() == "GENERIKA")
                {
                    dsgenerika = getLookUp("GenerikaAPI_Request");

                    if (dsgenerika.Tables[0].Rows.Count > 0)
                    {
                        generikarequest = dsgenerika.Tables[0].Select("FldCode ='" + merchantcode + "'")[0][2].ToString();
                    }

                    generikarequest = generikarequest.Replace("[merrefno]", refno);
                }

                foreach (string y in splitted_value)
                {
                    if (y != string.Empty)
                    {
                        pairvalue = y.Split('=');

                        if (!isForUpdate)
                        {
                            if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                            {
                                uprow["ReferenceNo"] = refno;
                                uprow["MerchantCode"] = merchantcode;
                                uprow["MerchantProductCode"] = prodcode;
                                uprow["FieldLabel"] = pairvalue[0]; //key
                                uprow["FieldValue"] = pairvalue[1].Replace("-", ""); //value

                                if (isbayadcenter.ToUpper() == "BAYADCENTER")
                                {
                                    if (pairvalue[0].Contains("Date") == true)
                                    {
                                        bayadcenterrequest = bayadcenterrequest.Replace("[" + pairvalue[0] + "]", Convert.ToDateTime(pairvalue[1].Replace("-", "")).ToString("MM-dd-yyyy"));
                                    }
                                    else
                                    {
                                        bayadcenterrequest = bayadcenterrequest.Replace("[" + pairvalue[0] + "]", pairvalue[1].Replace("-", ""));
                                    }
                                }

                                if (merchantcode.ToUpper() == "GENERIKA")
                                {
                                    generikarequest = generikarequest.Replace("[" + pairvalue[0] + "]", pairvalue[1].Replace("-", ""));
                                }

                                uprow["MerSeqID"] = merseqid;
                                table.Rows.Add(uprow);

                                if (pairvalue[0].ToString() == "Target Amount")
                                {
                                    targetAmt = pairvalue[1].ToString();
                                    if (isbayadcenter.ToUpper() == "BAYADCENTER")
                                    {
                                        bayadcenterrequest = bayadcenterrequest.Replace("[amount]", targetAmt);
                                    }
                                    if (merchantcode.ToUpper() == "GENERIKA")
                                    {
                                        generikarequest = generikarequest.Replace("[amount]", Convert.ToDecimal(targetAmt).ToString("####"));
                                        generikarequest = generikarequest.Replace("[keyword]", "PROCESS");
                                    }
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
                    /*
                        irsCalc.commit_remittance_charge_details(seq_id);
                        bool ibool = new irsOverrideLog("", "OBPS").LogOverride(seq_id);
                    */

                    /* RFC-21138 RED RIBBON AND GREENWICH */
                    try
                    {
                        irsCalc.commit_remittance_charge_details(seq_id);

                        //=======================================================================================
                        //RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
                        //bool ibool = new irsOverrideLog("", "OBPS").LogOverride(seq_id);

                        if (System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] != null || (string)System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] != string.Empty)
                        {
                            string hitlist_cnt = (string)System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"];
                            string[] hitlist_arr = null;

                            if (hitlist_cnt != null && hitlist_cnt != string.Empty && hitlist_cnt.Length > 0)
                            {
                                hitlist_arr = hitlist_cnt.Split('~');
                            }

                            if (hitlist_arr != null && hitlist_arr.Length > 0)
                            {
                                for (int i = 0; i < hitlist_arr.Length; i++)
                                {
                                    //Hitlist details when Adding Merchant should not be included in saving details to Remittance Override Log table
                                    if (hitlist_arr[i].IndexOf("25") <= 0 && hitlist_arr[i].ToString().Trim() != "")
                                    {
                                        /* ADDED BY BOYET APRIL 28 2016 */
                                        bool ibool = new irsOverrideLog("", "OBPS").LogOverride(seq_id);
                                    }
                                }
                            }
                        }
                        //=======================================================================================

                    }
                    catch
                    {
                    }
                    /* RFC-21138 RED RIBBON AND GREENWICH */
                }

                base.addMode = false;
                bool MdetailsSave = false;
                MdetailsSave = base.save(sqlHelperWs.RETAIN_DATASET);

                if (MdetailsSave == true & merchantcode == "SSS")
                {
                    return saveOBPExcess(refno, targetAmt, actualAmt);
                }

                if (isbayadcenter.ToUpper() == "BAYADCENTER")
                {
                    System.Web.HttpContext.Current.Session["OBPSBayadCenterRequest"] = bayadcenterrequest;
                }

                if (merchantcode.ToUpper() == "GENERIKA")
                {
                    System.Web.HttpContext.Current.Session["OBPSBayadCenterRequest"] = generikarequest;
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
                irsCalc = null;
                System.Web.HttpContext.Current.Session["OBPSCalculator"] = null;
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
            int intRole = 0;

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
                intRole = Convert.ToInt32(getUserRole());

                if (intRole != 10 && intRole != 25 && intRole != 7 && intRole != 27)
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

                excessPayment = "OBPS|" + ExcessAcount + "|" + NSVS + "|" + Convert.ToString(Math.Round(nettobeneAmt, 2)) + "|" + Convert.ToString(Math.Round(CovertedExcessAmount, 2)) + "|" + Convert.ToString(Math.Round(excessAmt, 2));

                base.getUpdateDS("spOBPExcess", new object[] { dt.Rows[0]["SEQ_ID"].ToString() });
                DataTable dtCopy = table.Copy();

                bool isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);

                if (!isForUpdate)
                {
                    if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                    {
                        uprow["MerchantRefNo"] = excessPayment;
                        uprow["RemittanceSEQ_ID"] = dt.Rows[0]["SEQ_ID"].ToString();
                        table.Rows.Add(uprow);
                    }

                    base.addMode = false;
                }

                return base.save(sqlHelperWs.RETAIN_DATASET);
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
        }

        public bool SaveUpdates()
        {
            return base.save(sqlHelperWs.RETAIN_DATASET);
        }

        public string getUserRole()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }

            irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = null;
            string sReturn = string.Empty;

            try
            {
                ds = commons.getDataSet("SELECT * FROM TBL_LOOKUP WITH (NOLOCK) WHERE fldgroup = 'User Roles' and FLDVALUE ='" + client_data.Role.ToString() + "'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

                if (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("AGENT") == true && (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("APPROVER") == true && ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().IndexOf("PROCESSOR") < 0))
                {
                    sReturn = "27";
                }
                else if (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("AGENT") == true && (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("PROCESSOR") == true && ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().IndexOf("APPROVER") < 0))
                {
                    sReturn = "7";
                }
                else if (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("AGENT") == true && (ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("PROCESSOR") == true && ds.Tables[0].Rows[0]["FLDDescription"].ToString().ToUpper().Contains("APPROVER") == true))
                {
                    sReturn = "25";
                }
                else
                {
                    sReturn = Convert.ToString(((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Role);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "getUserRole", ex.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                ds.Dispose();
                ds = null;
            }

            return sReturn;
        }

        public DataSet GetBranches(string location)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        /* VAPT 20181015 */
        public string getTransactionDetails(string val, string delimiter)
        {
            string[] arrVal;
            string ret = string.Empty;
            string log = string.Empty;
            string sys = string.Empty;

            try
            {
                arrVal = val.Split(new string[] { delimiter }, StringSplitOptions.None);

                if (arrVal != null && arrVal.Length > 0)
                {
                    switch (arrVal.GetValue(0).ToString().Trim().ToUpper())
                    {
                        case "SEQ_ID":
                        case "CHARGELOCALLY":
                        case "PRINTED":
                            log = "NOLOG";
                            break;

                        case "PRODUCTCHARGESID":
                        case "ACCOUNTSEQID":
                        case "REMITTERADDRESSSEQID":
                        case "REMITTERCONTACTSEQID":
                        case "BENEFICIARYADDRESSSEQID":
                        case "BENEFICIARYCONTACTSEQID":
                            sys = arrVal.GetValue(1).ToString().Trim();
                            break;

                        case "REMITTINGCURRENCY":
                        case "NETTOBENECURRENCY":
                        case "LOCALCURRENCY":
                            log = new irsInternetPage().getLookupValue("Currency", "FldValue", arrVal.GetValue(1).ToString().Trim(), "FldCode");
                            break;

                        case "TRANSTATUS":
                            log = new irsInternetPage().getLookupValue("TranxStatus", "FldValue", arrVal.GetValue(1).ToString().Trim(), "FldDescription");
                            break;

                        case "TRANSACTIONSOURCE":
                            log = new irsInternetPage().getLookupValue("TranxSource", "FldValue", arrVal.GetValue(1).ToString().Trim(), "FldDescription");
                            break;

                        case "TRANSACTIONPURPOSE":
                            log = new irsInternetPage().getLookupValue("TranxPurpose", "FldValue", arrVal.GetValue(1).ToString().Trim(), "FldDescription");
                            break;

                        default:
                            log = new irsInternetPage().getLookupValue(arrVal.GetValue(0).ToString(), "FldValue", arrVal.GetValue(1).ToString().Trim(), "FldDescription");
                            break;
                    }

                    if (log == "NOLOG")
                    {
                        log = string.Empty;
                    }
                    else
                    {
                        if (arrVal.GetValue(1).ToString().Trim() != string.Empty && log != "NOLOG")
                        {
                            if (log == string.Empty && sys == string.Empty)
                            {
                                if (arrVal.GetValue(1).ToString().IndexOf("null") < 0)
                                {
                                    log = arrVal.GetValue(1).ToString().Trim();
                                }
                            }
                        }
                    }

                    ret = log + "^" + sys;
                }
            }
            catch
            {
                ret = string.Empty;
            }

            return ret;
        }

        public bool saveOBPS(string datapair, string userid)
        {
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                /* VAPT 20190627 */
                string[] arrVal = datapair.Split(',');
                string log = string.Empty;
                string sys = string.Empty;
                int i = 0;
                string val = string.Empty;
                string[] arrDetails;

                if (arrVal != null && arrVal.Length > 0)
                {
                    for (i = 0; i <= (arrVal.Length - 1); i++)
                    {
                        if (arrVal.GetValue(i).ToString().IndexOf("=") >= 0)
                        {
                            val = this.getTransactionDetails(arrVal.GetValue(i).ToString(), "=");
                            arrDetails = val.Split('^');

                            if (arrDetails.GetValue(0).ToString().Trim() != string.Empty)
                            {
                                log += arrVal.GetValue(i).ToString().Split('=').GetValue(0).ToString().Trim() + "=" + arrDetails.GetValue(0).ToString().Trim() + " ~ ";
                            }
                            else
                            {
                                if (arrDetails.GetValue(1).ToString().Trim() != string.Empty)
                                {
                                    sys += arrVal.GetValue(i).ToString().Split('=').GetValue(0).ToString().Trim() + "=" + arrDetails.GetValue(1).ToString().Trim() + " ~ ";
                                }
                            }
                        }
                        else
                        {
                            sys += "Error at " + arrVal.GetValue(i).ToString() + " " + i.ToString() + " ~ ";
                        }
                    }
                }

                new csAuditLog().writeVAPTLogger("", "Bills Payment", "CREATED AN OBP TRANSACTION", "", log, sys, csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeVAPTLogger("", "Bills Payment", "CREATED AN OBP TRANSACTION", "", datapair, ex.Message, csAuditLog.AuditType.User, csAuditLog.AlertType.Alert);
            }


            /* OBBA EXCISE TAX */
            string OBBAComputedTax = string.Empty;
            string OBBAPercentage = string.Empty;

            if (System.Web.HttpContext.Current.Session["OBBA_COMPUTEDTAX"] != null)
            {
                OBBAComputedTax = System.Web.HttpContext.Current.Session["OBBA_COMPUTEDTAX"].ToString();
                this.OBBAComputedTax = Convert.ToDecimal(OBBAComputedTax);
            }
            else { this.OBBAComputedTax = Convert.ToDecimal("0.00"); }
            if (System.Web.HttpContext.Current.Session["OBBA_PERCENTAGE"] != null)
            {
                OBBAPercentage = (string)System.Web.HttpContext.Current.Session["OBBA_PERCENTAGE"].ToString();
                this.OBBAPercentage = Convert.ToDecimal(OBBAPercentage);
            }
            else { this.OBBAPercentage = Convert.ToDecimal("0.00"); }
            /* OBBA EXCISE TAX */

            try
            {
                string[] splitted_value;
                string[] data_value;

                splitted_value = datapair.Split(',');
                foreach (string x in splitted_value)
                {
                    data_value = x.Split('=');
                    this.pushdata(data_value);
                }

                this.parseSpecialMessage();
                this.TableName = "tbl_remittance";
                base.getUpdateDS("spGetOBPSData", new object[] { this.ReferenceNo });
                DataTable dtCopy = table.Copy();

                bool isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);

                if (!isForUpdate)
                {
                    if (base.newRow(sqlHelperWs.RETAIN_DATASET))
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
                        uprow["RemitterAddressSeqID"] = this.RemitterAddressSeqID;
                        uprow["RemitterContactSeqID"] = this.RemitterContactSeqID;
                        uprow["BeneficiaryAddressSeqID"] = 0;
                        uprow["BeneficiaryContactSeqID"] = 0;
                        uprow["RemitterAccountNo"] = this.RemAcctNo;
                        /* OBBA EXCISE TAX */
                        uprow["OBBAComputedTax"] = this.OBBAComputedTax; 
                        uprow["OBBAPercentage"] = this.OBBAPercentage;
                        /* OBBA EXCISE TAX */
                        table.Rows.Add(uprow);
                    }

                    base.addMode = false;
                }

                return base.save(sqlHelperWs.RETAIN_DATASET);
            }

            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "Save Transaction", ex.Message, datapair, csAuditLog.Category.Insert);
                throw ex;
            }
            finally
            {
                try
                {
                    this.TranxID = Convert.ToString(execScalar("SELECT SEQ_ID FROM TBL_REMITTANCE (NOLOCK) WHERE UserID ='" + userid
                    + "' AND RemitMethod =" + this.RemitMethod + " AND RemittingBranch = '" + this.RemittingBranch
                    + "' AND ExtlReferenceNo = '" + this.ExtReferenceNo + "' AND Transtatus ='" + this.TransStatus
                    + "' AND CONVERT(VARCHAR(10),TranxDate,101) ='" + this.TranxDate.ToString("MM/dd/yyyy")
                    + "' AND ProductChargesID =" + this.ProductChargesID + " AND RemitterCIFID ='"
                    + this.RemitterCIFID + "' AND BeneficiaryCIFID ='" + this.BeneficiaryCIFID
                    + "' AND AccountSEQID =" + this.AccountSeqID + " AND RemittingCurrency=" + this.RemittingCurrency
                    + " AND GrossRemAmount=" + Math.Round(this.GrossRemAmount, 4) + " AND NetRemAmount=" + Math.Round(this.NetRmtAmt, 4)
                    + " AND NetToBeneCurrency=" + this.NetToBeneCurrency + " AND NetToBeneAmount=" + Math.Round(this.NetToBeneAmount, 4)));

                    audit.writeAudit(clientdata.UserId, "OBPS Transaction", "Save Transaction", "", datapair + "-" + this.TranxID, csAuditLog.Category.Insert);
                }
                catch (Exception ex2)
                {
                    audit.writeAudit(clientdata.UserId, "OBPS Transaction", "Save Transaction", ex2.Message, datapair, csAuditLog.Category.Insert);
                    throw ex2;
                }
            }
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
                else if (data_value[0] == "RemitterAddressSeqID")
                {
                    this.RemitterAddressSeqID = Convert.ToInt64(data_value[1]);
                }
                else if (data_value[0] == "RemitterContactSeqID")
                {
                    this.RemitterContactSeqID = Convert.ToInt64(data_value[1]);
                }
                else if (data_value[0] == "NotificationMode")
                {
                    if (data_value[1] == "NULL" || data_value[1] == null || data_value[1] == "null" || data_value[1] == "")
                    {
                        data_value[1] = "0";
                    }

                    this.NotificationMode = Convert.ToInt32(data_value[1]);
                }
                else if (data_value[0] == "RemAccountNo")
                {
                    this.RemAcctNo = data_value[1];
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

        public bool ValidID(string sIDNo, string MerchantCode, string sOthers)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet dsIsIDMandatory = getDataSet("select TOP 1 * FROM TBL_OPTIONALFIELD WHERE fieldname = 'id1' and MERCHANTCODE = '" + MerchantCode + "'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                if ((bool)dsIsIDMandatory.Tables[0].Rows[0]["ismandatory"] == true)
                {
                    DataSet dsGetSPValid = getDataSet("select * from TBL_LOOKUP WHERE FLDGROUP ='CheckDigits' AND FLDCODE = '" + MerchantCode + "'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                    string sp_name = "";

                    if (dsGetSPValid == null || dsGetSPValid.Tables[0].Rows.Count <= 0)
                    {
                        return false;
                    }
                    else
                    {
                        sp_name = dsGetSPValid.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
                        DataSet dsfSPExist = getDataSet("select TOP 1 * FROM sys.objects WHERE [name] = '" + sp_name.Trim() + "' and type_desc = 'SQL_STORED_PROCEDURE'", false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

                        if (dsfSPExist.Tables[0].Rows.Count > 0)
                        {
                            if (MerchantCode == "SSS")
                            {
                                if (Convert.ToString(dsIsIDMandatory.Tables[0].Rows[0]["ProductCode"]) != "100")
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
                                else
                                {
                                    return true;
                                }
                            }
                            else if (MerchantCode == "PAGIBIG")
                            {
                                HDMFcdv.HDMFcdv pagibig2 = new HDMFcdv.HDMFcdv();
                                sOthers = sOthers.Replace("P", "").Replace("|1", "").Trim();

                                if (sOthers == "STL")
                                {
                                    sOthers = "ST";
                                }

                                return pagibig2.IsValidID(sIDNo, sOthers);
                            }
                            else
                            {
                                DataTable dtOthers = execStoredProcedureTable(sp_name, new object[] { sIDNo, sOthers, "" });

                                if (dtOthers.Rows[0][0].ToString() == "1")
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
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
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Transaction", "ValidID", ex.Message, sIDNo + " - " + MerchantCode + " - " + sOthers, csAuditLog.Category.Select);
                return false;
            }
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
                ds = getDataSet("Select * from tbl_remittance Where SEQ_ID = " + trxid, false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
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

                dt = base.getDataSet("SELECT * FROM TBL_CIF_MERCHANTS WHERE SEQ_ID = " +
                    SEQ_ID + " AND CIFID = '" + CIFID + "' AND RemitterCIFID = '" + RemCIFID + "'", false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                base.getUpdateDS("SELECT * FROM TBL_CIF_MERCHANTS WHERE SEQ_ID = " +
                    SEQ_ID + " AND CIFID = '" + CIFID + "' AND RemitterCIFID = '" + RemCIFID + "'");


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
            base.getUpdateDataSet(getSqlCommand("SELECT * FROM " + this.TableName + " WHERE SEQ_ID = " + id, false));
        }

        public Boolean fOBPAccount(string AccountNo)
        {
            bool isOBP = false;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                DataTable dt = base.getDataSet("SELECT * FROM vwMerchantAccounts WHERE ACCOUNTNO = '" + AccountNo.Trim().Replace("\r", "") + "'", false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

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

            try
            {
                dt = base.getDataSet("Select * from TBL_CHARGE_MASTER WHERE ProductCode = 10 and ImplementationType = 2 and BranchCode = '" + brCode.Trim() +
                    "' and ChargeType = '" + chargeType + "' and Currency = '" + sourceCCy + "' and Paymode ='" + Paymode + "'", false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                if (dt != null && dt.Rows.Count > 0)
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

            try
            {
                dt = base.getDataSet("Select Transtatus from TBL_Remittance WHERE referenceno = '" + refno.Trim() + "'", false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

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
            catch
            {
                return false;
            }
        }

        public DataTable TranxDetailsHistory(string merchantcode, string merprodcode, string RemCIFID)
        {
            irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            try
            {
                return execStoredProcedureTable("spGetTranxDetailsHistory", new object[] { merchantcode.Trim(), merprodcode.Trim(), RemCIFID.Trim() });
            }
            catch (Exception ex)
            {
                audit.writeAudit(client_data.UserId, "OBPS Transaction", "TranxDetailsHistory", ex.Message, RemCIFID, csAuditLog.Category.Select);
                return null;
            }
        }

        public bool sendBayadcenter(string merchantcode)
        {
            irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            string bayadcenterparameter = (string)System.Web.HttpContext.Current.Session["OBPSBayadCenterRequest"];
            OBPS_BayadCenter.OBPS_BayadCenter bayadc = new IRSClassLib.OBPS_BayadCenter.OBPS_BayadCenter();
            string sessionid = "";
            string createsessionresponse = "";
            string createsessionresultcode = "";
            string paybillerresponse = "";
            string paybillerresultmessage = "";
            string paybillerresultcode = "";
            string paybillerrefno = "";
            try
            {
                bayadc.Url = new irsConfiguration().appsetting("BayadCenterAPI");
                createsessionresponse = bayadc.CreateSession();

                if (createsessionresponse != "")
                {
                    createsessionresultcode = createsessionresponse.Substring(createsessionresponse.IndexOf("<resultCode>") + 12, 1);

                    if (createsessionresultcode == "2")
                    {
                        sessionid = createsessionresponse.Substring(createsessionresponse.IndexOf("<sessionId>") + 11, 40);
                        paybillerresponse = bayadc.PayBiller(sessionid, bayadcenterparameter, merchantcode);
                        if (paybillerresponse != "")
                        {
                            paybillerresultcode = paybillerresponse.Substring(paybillerresponse.IndexOf("<resultCode>") + 12, paybillerresponse.Substring(paybillerresponse.IndexOf("<resultCode>") + 12).IndexOf("</resultCode>"));

                            if (paybillerresultcode == "2")
                            {
                                paybillerrefno = paybillerresponse.Substring(paybillerresponse.IndexOf("<transactionId>") + 15, paybillerresponse.Substring(paybillerresponse.IndexOf("<transactionId>") + 15).IndexOf("</transactionId>"));
                                paybillerresultmessage = "Bayad Center Response \r\n" + "Result Code: " + paybillerresultcode + "\r\n" + "Transaction ID:" + paybillerrefno + "\r\n" + "Message: " + paybillerresponse.Substring(paybillerresponse.IndexOf("<resultMessage>") + 15, paybillerresponse.Substring(paybillerresponse.IndexOf("<resultMessage>") + 15).IndexOf("</resultMessage>"));
                                System.Web.HttpContext.Current.Session["OBPSBayadCenterResponseMessage"] = paybillerresultmessage;
                            }
                            else
                            {
                                paybillerresultmessage = "Bayad Center Response \r\n" + "Result Code: " + paybillerresultcode + "\r\n" + "Message: " + paybillerresponse.Substring(paybillerresponse.IndexOf("<resultMessage>") + 15, paybillerresponse.Substring(paybillerresponse.IndexOf("<resultMessage>") + 15).IndexOf("</resultMessage>"));
                                System.Web.HttpContext.Current.Session["OBPSBayadCenterResponseMessage"] = paybillerresultmessage;
                                return false;
                            }
                        }
                        else
                        {
                            paybillerresultmessage = "Bayad Center Response " + paybillerresponse.Substring(paybillerresponse.IndexOf("<resultMessage>") + 15, paybillerresponse.Substring(paybillerresponse.IndexOf("<resultMessage>") + 15).IndexOf("</resultMessage>"));
                            System.Web.HttpContext.Current.Session["OBPSBayadCenterResponseMessage"] = paybillerresultmessage;
                            return false;
                        }
                    }
                    else
                    {
                        paybillerresultmessage = "Bayad Center Response " + createsessionresponse.Substring(createsessionresponse.IndexOf("<resultMessage>") + 15, createsessionresponse.Substring(createsessionresponse.IndexOf("<resultMessage>") + 15).IndexOf("</resultMessage>"));
                        System.Web.HttpContext.Current.Session["OBPSBayadCenterResponseMessage"] = paybillerresultmessage;
                        return false;
                    }
                    audit.writeAudit(client_data.UserId, "OBPS Transaction", "sendBayadcenter", paybillerresponse, createsessionresponse, csAuditLog.Category.Select);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Web.HttpContext.Current.Session["OBPSBayadCenterResponseMessage"] = ex.Message;
                audit.writeAudit(client_data.UserId, "OBPS Transaction", "sendBayadcenter", ex.Message, createsessionresponse, csAuditLog.Category.Select);
                return false;
            }
        }

        public bool sendGenerika(string refNo)
        {
            return false;
        }

        public bool validate_dragonpay(string memid, bool isvalid)
        {
            DataSet ds = execStoredProcedure("sp_cd_DragonPay", new object[] { memid, isvalid });

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToBoolean(Convert.ToInt32(ds.Tables[0].Rows[0][0]));
                }

                return false;
            }

            return false;
        }

        /* PAOLO PAL BOOK AND HOLD JULY 16 2019 */
        public string validate_checkdigit_pal(string amount, string pal_referenceno, string currency)
        {
            string amt = Convert.ToDecimal(amount).ToString("n3");
            DataSet ds = execStoredProcedure("sp_PAL_CHECKDIGIT", new object[] { pal_referenceno, amt, currency });

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0]["Result"].ToString();
                }

                return null;
            }

            return null;
        }
        /* PAOLO PAL BOOK AND HOLD JULY 16 2019 */

        private void parseSpecialMessage()
        {
            string[] arrSpecialMessage;
            DateTime dts;

            if (this.SpecialMessage != "")
            {
                arrSpecialMessage = this.SpecialMessage.Split('!');
                this.SpecialMessage = String.Empty;

                for (int i = 0; i <= arrSpecialMessage.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        this.MerchantCode = arrSpecialMessage[i].ToString();
                        continue;
                    }
                    else if (i == 1)
                    {
                        this.MerchantProdCode = Convert.ToInt32(arrSpecialMessage[i]);
                        continue;
                    }
                    else if (i == 2)
                    {
                        this.MerchantMemID = arrSpecialMessage[i].ToString().Split('^')[1].ToString();
                        continue;
                    }
                    if (arrSpecialMessage[i].ToString().Contains("From") && DateTime.TryParse(arrSpecialMessage[i].ToString().Split('^')[1].ToString(), out dts) == true)
                    {
                        this.PaymentFrom = Convert.ToDateTime(arrSpecialMessage[i].ToString().Split('^')[1].ToString());
                        continue;
                    }
                    else if (arrSpecialMessage[i].ToString().Contains("To") && DateTime.TryParse(arrSpecialMessage[i].ToString().Split('^')[1].ToString(), out dts) == true)
                    {
                        this.PaymentTo = Convert.ToDateTime(arrSpecialMessage[i].ToString().Split('^')[1].ToString());
                        continue;
                    }
                    this.SpecialMessage += arrSpecialMessage[i].ToString().Replace('|', '$') + "!";
                }
            }
        }

        public bool validate_pagibig(string pino, string paytype)
        {
            try
            {
                HDMFcdv.HDMFcdv pagibig = new HDMFcdv.HDMFcdv();
                return pagibig.IsValidID(pino, paytype);
            }
            catch
            {
                return false;
            }
        }

        public int getReferenceCounter(string brcode, string trandate)
        {
            sqlHelperWs sql = new sqlHelperWs();
            object ctr = sql.execScalar("select seq from TBL_REFERENCENO_CTR where brcode='" + brcode + "' and txndate = '" + Convert.ToDateTime(trandate).ToString("MM/dd/yyyy") + "'");

            if (ctr != null)
            {
                return Convert.ToInt32(ctr);
            }

            return 1;
        }

        /* BOYET BEAM AND GO 06-10-2019 */
        public string validate_beam_and_go_reference(string beam_reference_no, string transaction_amount)
        {
            BeamNGoAPI beam = null;
            string return_value = string.Empty;

            try
            {
                beam = new BeamNGoAPI();
                beam.ReferenceNo = beam_reference_no;
                beam.AmountReceived = transaction_amount;
                return_value = beam.get_transaction_by_pin();

                return return_value;
            }
            catch (Exception e)
            {
                return "ERROR: " + e.Message;
            }
        }

        public string post_beam_and_go_reference(string transaction_code, string remarks, string currencycode, string amount, string fx_rate)
        {
            BeamNGoAPI beam = null;

            try
            {
                beam = new BeamNGoAPI();
                beam.ReferenceNo = transaction_code;
                beam.Remarks = remarks;
                beam.Currency = currencycode;
                beam.AmountReceived = amount;
                beam.FXRate = "0"; // DEFAULT SINCE WE DO NOT NEED TO PASS FX RATE

                return beam.post_beam_and_go_reference();
            }
            catch (Exception e)
            {
                return "ERROR: Unable to send payment to Beam and Go. " + e.Message;
            }
        }
        /* BOYET BEAM AND GO 06-10-2019 */

        public bool PostTransactionBeam(string transactioncode, string remarks, string currencycode, string amount)
        {
            BeamNGoAPI beam = new BeamNGoAPI();
            beam.ReferenceNo = transactioncode;
            beam.Remarks = remarks;
            beam.Currency = currencycode;
            beam.AmountReceived = amount;
            beam.FXRate = "0"; //default since we do not passed fx rate

            return beam.ProcessTransaction();
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

        private Int64 _remitteraddressseqid;
        private Int64 RemitterAddressSeqID
        {
            get { return _remitteraddressseqid; }
            set { _remitteraddressseqid = value; }
        }

        private Int64 _remittercontactseqid;
        private Int64 RemitterContactSeqID
        {
            get { return _remittercontactseqid; }
            set { _remittercontactseqid = value; }
        }

        private Int32 _notificationmode;
        private Int32 NotificationMode
        {
            get { return _notificationmode; }
            set { _notificationmode = value; }
        }

        private string _merchantcode;
        private String MerchantCode
        {
            get { return _merchantcode; }
            set { _merchantcode = value; }
        }

        private Int32 _merchantprodcode;
        private Int32 MerchantProdCode
        {
            get { return _merchantprodcode; }
            set { _merchantprodcode = value; }
        }

        private string _merchantmemid;
        private String MerchantMemID
        {
            get { return _merchantmemid; }
            set { _merchantmemid = value; }
        }

        private DateTime _paymentfrom;
        private DateTime PaymentFrom
        {
            get { return _paymentfrom; }
            set { _paymentfrom = value; }
        }

        private DateTime _paymentto;
        private DateTime PaymentTo
        {
            get { return _paymentto; }
            set { _paymentto = value; }
        }

        private String _remAcctNo;
        private String RemAcctNo
        {
            get { return _remAcctNo; }
            set { _remAcctNo = value; }
        }

        private decimal _obbacomputedtax;
        private Decimal OBBAComputedTax
        {
            get { return _obbacomputedtax; }
            set { _obbacomputedtax = value; }
        }

        private decimal _obbapercentage;
        private Decimal OBBAPercentage
        {
            get { return _obbapercentage; }
            set { _obbapercentage = value; }
        }
        #endregion
    }
}
