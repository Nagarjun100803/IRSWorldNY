using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using IRSWorld.TranClass;
using IRSClassLib;
using IRSWorld.Reports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports;
using System.Drawing;
using System.Drawing.Printing;
using System.ComponentModel;
using System.IO;




namespace IRSWorld.Remittance
{

    public partial class OTCViewAndApproval : System.Web.UI.Page
    {
        private PrintDocument printDoc = new PrintDocument();
        private PageSettings pgSettings = new PageSettings();
        private PrinterSettings prtSettings = new PrinterSettings();

        private int tranxid = 0;
        private int FOR_APRROVAL = 15;
        private int CREDITED_ONLINE = 2;
        private int FOR_REPROCESS = 9;
        private int POSTED_CENTRAL = 5;
        private int intRemitMethod = 0;

        protected void Page_Load(object sender, EventArgs e)
        {

            AjaxPro.Utility.RegisterTypeForAjax(typeof(OTCViewAndApproval));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCommons));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_account_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_open_accounts));

            Session["TranxID"] = Request.QueryString["remittance_txn_id"];
            Session["sourcepage"] = Request.QueryString["param_values"];
            //irsCalculator calcu = new irsCalculator(TranxID);
            //Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;
        }

        public int TranxID
        {
            get { return int.Parse(Session["TranxID"].ToString()); }
            set { tranxid = value; }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRole()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserID()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getlocaltime()
        {
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(getLocation());
            return Convert.ToString(dtLocalTime);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getChargesDefaults(string ChargeID)
        {
            return new irsInternetPage().getChargesDefaults(ChargeID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaults(string BranchCode)
        {
            return new irsInternetPage().getDefaults(BranchCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getExchangeRate()
        {
            return new irsARM().getExchangeRate(this.getlocaltime());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_RemitTrans()
        {
            //SqlCommand sqcommands = null;
            irsSqlCommonsWs sql = new irsSqlCommonsWs();

            try
            {
                //sqcommands = sql.getSqlCommand("sp_viewRemittance_details");
                //sqcommands.Parameters.Add("@tranxid", SqlDbType.BigInt).Value = Session["TranxID"];
                //return sql.getDataSet(sqcommands, "Remittance_INFOS");
                return sql.getDataSet("sp_viewRemittance_details", true, new Object[] { Session["TranxID"] }, new String[] { "Remittance_INFOS" });
            }
            catch //(Exception e) // commented to remove warning
            {
                return null;
            }
            finally
            {
                //if (sqcommands != null)
                //{
                //    sqcommands.Dispose();
                //    sqcommands = null;
                //}
                sql = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet Remit(int seqid, bool isOBPS, bool reimplementation)
        {
            irsRemTeller rem = new irsRemTeller();
            string merchantcode = "";
            DataSet paramMsg = new DataSet();
            irsOBPSTransactions ob = new irsOBPSTransactions();
            DataTable dtStatus = new DataTable();
            DataRow dr;
            DataSet ds = new DataSet();
            //string paramMessage = ""; // commented to remove warning
            //tranxid = int.Parse(Session["TranxID"].ToString());

            try
            {
                //check first the transaction status.Please take note that only transactions with For Approval(15) status will be processed.
                int paramStatus = rem.Status(seqid);
                if (paramStatus == FOR_APRROVAL || paramStatus == FOR_REPROCESS)
                {
                    //string branchcode = getLocation();
                    if (reimplementation == true)
                    {
                        this.delete_product_and_computed_charges(TranxID);
                    }

                    if (Session["REMITTANCE_CALCULATOR_CLASS"] != null)
                    {

                        irsCalculator calc = (irsCalculator)Session["REMITTANCE_CALCULATOR_CLASS"];
                        //calc.commit_remittance_charge_details(TranxID);
                        Session["REMITTANCE_CALCULATOR_CLASS"] = calc;

                    }

                    if (Session["REMITTANCE_CALCULATOR_CLASS"] == null)
                    {
                        irsCalculator calcu = new irsCalculator(TranxID);

                        //calcu.commit_remittance_charge_details(TranxID);
                        Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;

                    }
                    intRemitMethod = Convert.ToInt32(new irsInternetPage().getRemittanceInfo(seqid).Rows[0]["RemitMethod"].ToString().Trim());
                    if (Session["OBPSMerchantType"] != null && (string)Session["OBPSMerchantType"] != "")
                    {
                        if ((string)Session["OBPSMerchantType"] == "3")
                        {
                            merchantcode = ob.getTranxHistory(seqid).Rows[0]["MerchantName"].ToString().Split(',')[1].ToString();
                            if (ob.sendBayadcenter(merchantcode) == true)
                            {
                                paramMsg = rem.RemitToAgent(seqid);
                                if (paramMsg.Tables[0].Rows.Count <= 1)
                                {
                                    if (dtStatus.Columns.Count <= 1)
                                    {
                                        dtStatus.Columns.Add("Message");
                                        dtStatus.Columns.Add("Transtatus");
                                        dtStatus.Columns.Add("TranStatusDesc");
                                    }
                                    dr = dtStatus.NewRow();

                                    dr[0] = paramMsg.Tables[0].Rows[0][0].ToString().Trim() + "\r\n" + (string)System.Web.HttpContext.Current.Session["OBPSBayadCenterResponseMessage"];
                                    if (paramMsg.Tables[0].Columns.Count > 2)
                                    {
                                        dr[1] = paramMsg.Tables[0].Rows[0][1].ToString().Trim();
                                        dr[2] = paramMsg.Tables[0].Rows[0][2].ToString().Trim();
                                    }
                                    else
                                    {
                                        dr[1] = paramMsg.Tables[0].Rows[0][0].ToString().Trim();
                                        dr[2] = paramMsg.Tables[0].Rows[0][1].ToString().Trim();
                                    }
                                    dtStatus.Rows.Add(dr);
                                    ds.Tables.Add(dtStatus.Copy());
                                    paramMsg = null;
                                    paramMsg = ds;
                                }
                            }
                            else
                            {
                                paramMsg = rem.RemitToBayadCenter(seqid);
                            }
                        }
                        else
                        {
                            if (intRemitMethod != 8 && intRemitMethod != 10)
                            {
                                paramMsg = rem.Remit2(seqid);
                            }
                            else
                            {
                                paramMsg = rem.RemitToAgent(seqid);
                            }
                        }
                    }
                    else
                    {
                        if (intRemitMethod != 8 && intRemitMethod != 10)
                        {
                            paramMsg = rem.Remit2(seqid);
                        }
                        else
                        {
                            paramMsg = rem.RemitToAgent(seqid);
                        }
                    }
                    //if (intRemitMethod != 8 && intRemitMethod != 10)
                    //{
                    //    paramMsg = rem.Remit2(seqid);
                    //}
                    //else
                    //{
                    //    paramMsg = rem.RemitToAgent(seqid);
                    //}
                    rem.update_approving_officer(seqid);

                }
                else
                {

                    paramMsg = rem.ParseMessage(paramMsg, "Not Valid for this transaction. Please check the status");
                }
            }

            catch (Exception e)
            {
                paramMsg = rem.ParseMessage(paramMsg, e.ToString());
                return paramMsg;
            }

            finally
            { }
            if (Session["REMITTANCE_CALCULATOR_CLASS"] == null)
            {
                irsCalculator calcu = new irsCalculator(TranxID);
                calcu.reload_computation_entries(isOBPS);
                Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;

            }
            return paramMsg;
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public bool isDebitSuccessfull(int paymode ,string refno, string acctno, double remittingamount, int remittingcurrency,bool isReversal)
        //{

        //    bool forcredit = false;
        //    SqlCommand sqcommands = null;
        //    sqlHelper sql = new sqlHelper();
        //    string paramMsg = "";

        //    try
        //    {
        //        string branchcode = getLocation();

        //        irsRemTeller remdebit = new irsRemTeller(branchcode);

        //            if (isReversal == false)
        //            {
        //                string glCode = remdebit.get_gl(branchcode, remittingcurrency);
        //                if (remdebit.DebitCasaCreditGL(refno, acctno, glCode.ToString(), remittingamount.ToString(), remittingcurrency.ToString(), refno) == 1)
        //                {
        //                   // paramMSG = remdebit.AccountMessage;
        //                    return true;
        //                }
        //                else
        //                {
        //                    //paramMSG = remdebit.ErrorMessage;
        //                    return false;

        //                }
        //            }
        //            else
        //            {
        //                string glCode = remdebit.get_gl(branchcode, remittingcurrency,);
        //                if (remdebit.debitGlCreditCasa(refno, acctno, glCode.ToString(), remittingamount.ToString(), remittingcurrency.ToString(), refno) == 1)
        //                {
        //                    //paramMSG = remdebit.AccountMessage;
        //                    return true;
        //                }
        //                else
        //                {
        //                    //paramMSG = remdebit.ErrorMessage;
        //                    return false;

        //                }

        //            }


        //    }


        //    catch (Exception e)
        //    {
        //        throw (e);


        //    }


        //    finally
        //    {

        //    }
        //}
        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public string Remit(string refno, string nostrovostro, string targetaccount, double remittingamount, int transtatus, int productcode, int implemtype)
        //{
        //    irsRemTeller rem = new irsRemTeller();
        //    string paramMsg;
        //    int tranxid = int.Parse(Session["TranxID"].ToString());
        //    try
        //    {
        //        //paramStatus = rem.Status(tranxid);
        //        //if (paramStatus == 15)
        //        //{
        //        paramMsg = rem.Remit(refno, nostrovostro, targetaccount, remittingamount, transtatus, productcode, implemtype);
        //        //}
        //        //else
        //        //{
        //        //    paramMsg = "Fund Transfer Aborted.Transaction Status has been changed:";
        //        //}
        //    }

        //    catch (Exception e)
        //    {
        //        return e.ToString();
        //    }


        //    finally
        //    {

        //    }

        //    return paramMsg;
        //}
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validateReversal(int seqid)
        {
            DataTable dtCIFAccounts = new irsRemTeller().getCIFAccounts(seqid);
            string sAccountNo = string.Empty;
            string sProductCode = string.Empty;
            string sImplementationType = string.Empty;
            string sAccountStatusCode = string.Empty;
            string sMessage = string.Empty;
            DataTable dt = new DataTable();
            int i = 0;

            try
            {
                if (dtCIFAccounts != null && dtCIFAccounts.Rows.Count > 0)
                {
                    sAccountNo = dtCIFAccounts.Rows[0]["AccountNo"].ToString().Trim();
                    sProductCode = dtCIFAccounts.Rows[0]["ProductCode"].ToString().Trim();
                    sImplementationType = dtCIFAccounts.Rows[0]["ImplementationType"].ToString().Trim();
                }

                if (sAccountNo != string.Empty && sProductCode.ToString() == "10")
                {
                    sAccountStatusCode = new irsAccountInquiry().getFC(sAccountNo, irsAccountInquiry.FCMethod.AccountStatusCode, 0, true);
                    if (sAccountStatusCode != string.Empty)
                    {
                        dt = new irsInternetPage().getLookup("ReversalMessage");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (i = 0; i <= (dt.Rows.Count - 1); i++)
                            {
                                if (dt.Rows[i]["FldCode"].ToString().Trim() == sAccountStatusCode)
                                {
                                    sMessage = dt.Rows[i]["FldDescription"].ToString().Trim();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            sMessage = "Unable to retrieve global lookup setup. Please refer to BATD.";
                        }
                    }
                    else
                    {
                        sMessage = "Unable to verify account status. Please try again.";
                    }
                }
            }
            catch (Exception ex)
            {
                sMessage = ex.Message;
                new csAuditLog().writeAudit("", "Approval and Ammendments ERROR", "Validate Reversal SEQ_ID = " + seqid.ToString(), ex.Message, "validateReversal", csAuditLog.Category.Select);
            }

            if (sMessage == string.Empty)
            {
                return "1|";
            }
            else
            {
                return "0|" + sMessage;
            }
        }

        //==============================================
        //RFC-29480 : Reversal Limit in IRSW 09242023
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string allowed_for_reversal(string referenceno)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataTable dt_retval = new DataTable();
            string retval = "";

            try
            {
                dt_retval = commons.execStoredProcedureTable("proc_validate_reversal", new object[] { referenceno, this.getlocaltime() });

                if (dt_retval != null && dt_retval.Rows.Count > 0)
                {
                    retval = dt_retval.Rows[0][0].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "Approval and Ammendments ERROR", "Transaction allowed for reversal. Referenceno = " + referenceno, ex.Message, "allowed_for_reversal", csAuditLog.Category.Select);
            }
            finally
            {
                commons = null;
                dt_retval = null;
            }

            return retval;
        } 
        //==============================================

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet Reversal(int seqid)
        {
            irsRemTeller rem = new irsRemTeller();
            DataSet paramMsg = new DataSet();
            int paramStatus;
            //int tranxid = int.Parse(Session["TranxID"].ToString());
            try
            {
                paramStatus = rem.Status(seqid);
                if ((paramStatus == CREDITED_ONLINE) || (paramStatus == FOR_APRROVAL) || (paramStatus == FOR_REPROCESS) || (paramStatus == POSTED_CENTRAL))
                {
                    //Edited by Troy 20131127. RFS #13323-011.
                    string sReversal = string.Empty;
                    string[] arrReversal;
                    if (paramStatus == FOR_APRROVAL)//Edited for IR
                    {
                        sReversal = "1|";
                    }
                    else
                    {
                        sReversal = this.validateReversal(seqid);
                    }
                    arrReversal = sReversal.Split('|');

                    if (arrReversal.GetValue(0).ToString().Trim() == "1")
                    {
                        paramMsg = rem.Reversal(seqid);
                    }
                    else
                    {
                        DataTable dtReversal = new DataTable();
                        DataSet dsReversal = new DataSet();
                        DataRow dr;

                        dtReversal.Columns.Add("Message");
                        dtReversal.Columns.Add("Transtatus");
                        dtReversal.Columns.Add("TranStatusDesc");

                        dr = dtReversal.NewRow();
                        dr[0] = arrReversal.GetValue(1).ToString().Trim();
                        dr[1] = paramStatus.ToString();
                        dr[2] = new irsInternetPage().getLookupValue("TranxStatus", "FldValue", paramStatus.ToString(), "FldDescription");
                        dtReversal.Rows.Add(dr);
                        dsReversal.Tables.Add(dtReversal);
                        paramMsg = dsReversal;
                    }
                                        
                    // Added by Hajz 09212011. Cancel on cash management when transaction gets reversed.
                    String Status = paramMsg.Tables[0].Rows[0][1].ToString().Trim();
                    if (Status == "11" || Status == "12")
                    {
                        String msg = "";
                        if (!new TranClass.rem_modal_denomination().reverse_denomination(seqid, out msg))
                        {                         
                            new csAuditLog().writeAudit("", "REVERSAL", "REVERSE DENOMINATION", seqid.ToString(), msg, csAuditLog.Category.Update); 
                        }
                    }

                    //Edited by Troy 20110615. Cancel on GFC when transaction gets reversed.
                    //[0]: "ACCOUNT PROCESSED ***73030325000300000228GL358718100513CK134790300024 Reversal Successful"
                    //[1]: 12
                    //[2]: "Reversal OK"
                    //if (paramMsg.Tables[0].Rows[0][1].ToString().Trim() == "12")
                    //{
                    //    new irsLinkToGFC().cancel_GFC_Aggregate(new irsInternetPage().getReferenceNo(seqid));
                    //}
                    //please put logs.
                }
                else
                {
                    paramMsg = rem.ParseMessage(paramMsg, "Not Valid for cancellation.");
                }
            }

            catch (Exception e)
            {
                paramMsg = rem.ParseMessage(paramMsg, e.ToString());
                return paramMsg;
            }

            finally
            { }

            return paramMsg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet UpdateStatus()
        {
            DataSet ds = (DataSet)Session["Transtatus"];
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet holdtransaction(int status)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_trans_update_status");
                    sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = int.Parse(Session["TranxID"].ToString());
                    sqcommands.Parameters.Add("@STATUS", SqlDbType.Int, 100).Value = status;
                    return commons.getDataSet(sqcommands, "TXN_UPDATE_STATUS");

                    //}
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String ModifyClearingDate(string newclearingdate)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            //int paramMsg; // commented to remove warning
            try
            {

                object x = commons.execScalar("sp_modify_clearingdate", new string[] { Session["TranxID"].ToString(), newclearingdate });

                return x.ToString();


            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ChargesDetails()
        {

            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();

            try
            {
                sqcommands = sql.getSqlCommand("sp_getDetailedCharges");
                sqcommands.Parameters.Add("@tranxid", SqlDbType.BigInt).Value = Session["TranxID"];
                return sql.getDataSet(sqcommands, "Charges_INFOS");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_remittance_page()
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            string query_parameters;
            query_parameters = "?sourcepage=OTCViewAndApproval";

            try
            {
                //sqcommands = sql.getSqlCommand("sp_txn_search_txn_pages");
                //sqcommands.Parameters.Add("@product_charge_id", SqlDbType.BigInt).Value = product_charge_id;

                //DataTable dt = sql.getDataSet(sqcommands, "REMITTANCE_PAGE").Tables[0];

                //if (dt != null && dt.Rows.Count > 0)
                //{

                return "../Remittance/RemTransView.aspx" + query_parameters;
                //}
                // return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getOBPData(string refno, string merchcode, string prodcode)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            string bayadcenterrequest = "";
            DataSet dsbayadcenter = null;
            DataSet dsgetOBPData = null;
            irsOBPSTransactions ob = new irsOBPSTransactions();

            try
            {
                string MerchantType = sql.execScalar("select TOP 1 MerchantType from tbl_cif_merchants_info WHERE MerchantCode ='" + merchcode + "'").ToString();
                Session["OBPSMerchantType"] = MerchantType;
                
                sqcommands = sql.getSqlCommand("spGetMerchantInfo");
                sqcommands.Parameters.Add("@RefNo", SqlDbType.VarChar).Value = refno;
                sqcommands.Parameters.Add("@MerchantCode", SqlDbType.VarChar).Value = merchcode;
                sqcommands.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = prodcode;
                dsgetOBPData = sql.getDataSet(sqcommands, "OBPS_INFOS");
                if (MerchantType == "3")
                {
                    dsbayadcenter = ob.getLookUp("BayadCenterAPI_Request");
                    if (dsbayadcenter.Tables[0].Rows.Count > 0)
                    {
                        bayadcenterrequest = dsbayadcenter.Tables[0].Select("FldCode ='" + merchcode + "'")[0][2].ToString();
                    }
                    bayadcenterrequest = bayadcenterrequest.Replace("[merrefno]", refno);

                    if (dsgetOBPData != null)
                    {
                        if (dsgetOBPData.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsgetOBPData.Tables[0].Rows.Count; i++)
                            {
                                if (dsgetOBPData.Tables[0].Rows[i][0].ToString().Contains("Date") == true)
                                {
                                    bayadcenterrequest = bayadcenterrequest.Replace("[" + dsgetOBPData.Tables[0].Rows[i][0].ToString() + "]", Convert.ToDateTime(dsgetOBPData.Tables[0].Rows[i][1].ToString().Replace("-", "")).ToString("MM-dd-yyyy"));
                                }
                                else
                                {
                                    bayadcenterrequest = bayadcenterrequest.Replace("[" + dsgetOBPData.Tables[0].Rows[i][0].ToString() + "]", dsgetOBPData.Tables[0].Rows[i][1].ToString().Replace("-", ""));
                                }
                                if (dsgetOBPData.Tables[0].Rows[i][0].ToString() == "Target Amount")
                                {
                                    bayadcenterrequest = bayadcenterrequest.Replace("[amount]", dsgetOBPData.Tables[0].Rows[i][1].ToString());
                                }
                            }
                        }
                    }
                    System.Web.HttpContext.Current.Session["OBPSBayadCenterRequest"] = bayadcenterrequest;
                }
                
                return dsgetOBPData;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getRemitterInfoDetails()
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();

            try
            {
                sqcommands = sql.getSqlCommand("sp_get_RemitterPaymentDetails");
                sqcommands.Parameters.Add("@tranxid", SqlDbType.BigInt).Value = TranxID;

                return sql.getDataSet(sqcommands, "REMITTER_INFOS");
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getCIFAccountCurrency(string merchantcode)
        {
            irsOBPSTransactions irsOBPS = new irsOBPSTransactions();
            try
            {
                return irsOBPS.getCIFAccountCurrency(merchantcode);
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommons com = new irsSqlCommons();
                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "FileInd");

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private void loadReport(DataTable dt)
        {
            IRSWorld.Reports.ARM.rptARMReceipt rptReceipt = new IRSWorld.Reports.ARM.rptARMReceipt();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();
            Database crDatabase;
            Tables crTables;
            TableLogOnInfos crTableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crTableLogoninfo = new TableLogOnInfo();

            crConnectionInfo.ServerName = "192.168.51.21";
            crConnectionInfo.DatabaseName = "IRSWorld";
            crConnectionInfo.UserID = "sa";
            crConnectionInfo.Password = "qazwsx";

            crDatabase = rptReceipt.Database;
            crTables = crDatabase.Tables;
            crTableLogoninfo.ConnectionInfo = crConnectionInfo;
            crTableLogoninfos.Add(crTableLogoninfo);
            rptReceipt.SetDataSource(dt);
            rptReceipt.Refresh();
            rptReceipt.PrintToPrinter(1, false, 1, 1);

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool printReceipt(int remitmethod, string ReferenceNo)
        {
            try
            {
                switch (remitmethod)
                {
                    case 3:
                        {
                            DataTable dtReceipt = new DataTable();
                            dtReceipt = new irsARM().getARMDetails(ReferenceNo);
                            loadReport(dtReceipt);
                            break;
                        }
                }
                return true;
            }
            catch //(Exception e) // commented to remove warning
            {
                return false;
            }

            finally
            { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ShowReport(int remitmethod, bool isOBPS)
        {
            //string report_path = ""; // commented to remove warning
            ReportDocument receiptTranx = new ReportDocument();

            try
            {
                DataTable dtReceipt = new DataTable();
                switch (remitmethod)
                {
                    case 2:
                        {//Modified to handle Session State environment
                            //if (isOBPS==false)
                            //   {
                            //irsOBPSTransactions OBPTranx = new irsOBPSTransactions();
                            //IRSWorld.Reports.Standard.rptReceipt rptReceiptOBP = new IRSWorld.Reports.Standard.rptReceipt();
                            //receiptTranx = OBPTranx.GenerateOBPReceipt(rptReceiptOBP, Session["TranxID"].ToString());
                            Session["IRS_ReportsUI"] = "OBPS:" + Session["TranxID"].ToString();
                            break;
                            // }
                        }
                    case 8:
                        {
                            if (this.getLocation() == "E000")
                            {
                                DataSet ds = get_RemitTrans();
                                Session["IRS_ReportsUI"] = "EHUD:" + ds.Tables[0].Rows[0][1].ToString();
                                //Session["IRS_ReportsUI"] = "OBPS:" + Session["TranxID"].ToString();
                                ds.Dispose();
                                ds.Clear();
                                ds = null;                                
                            }
                            break;
                        }
                    case 10:
                        {
                            if (this.getLocation() == "E000")
                            {
                                DataSet ds = get_RemitTrans();
                                Session["IRS_ReportsUI"] = "EHUD:" + ds.Tables[0].Rows[0][1].ToString();
                                //Session["IRS_ReportsUI"] = "OBPS:" + Session["TranxID"].ToString();
                                ds.Dispose();
                                ds.Clear();
                                ds = null;
                            }
                            break;
                        }
                    default:
                        {
                            if (isOBPS == true)
                            {//Modified to handle Session State environment
                                //irsOBPSTransactions OBPTranx = new irsOBPSTransactions();
                                //IRSWorld.Reports.Standard.rptReceipt rptReceiptOBP = new IRSWorld.Reports.Standard.rptReceipt();
                                //receiptTranx = OBPTranx.GenerateOBPReceipt(rptReceiptOBP, Session["TranxID"].ToString());
                                Session["IRS_ReportsUI"] = "OBPS:" + Session["TranxID"].ToString();
                                break;
                            }
                            else
                            {
                                Session["IRS_ReportsUI"] = "OTC:" + Session["TranxID"].ToString();
                                break;
                            }
                        }

                }
                //DataSet ds = objAPAS.getAPASReceipt(refno);
                //irsReports _report = new irsReports(dtReceipt, report_path, true);
                //_report.display_report();                
            }
            catch //(Exception ex) // commented to remove warning
            {
                //ex.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void check_remittance_calculator(bool isOBPS)
        {
            if (Session["REMITTANCE_CALCULATOR_CLASS"] == null)
            {
                irsCalculator calcu = new irsCalculator(TranxID);
                calcu.reload_computation_entries(isOBPS);
                Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_breakdown(bool isOBPS)
        {

            if (Session["REMITTANCE_CALCULATOR_CLASS"] == null)
            {
                irsCalculator calcu = new irsCalculator(TranxID);
                if (this.getLocation() == "1170")
                {
                    calcu.reload_computation_entries(isOBPS, true);
                    Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;
                    return calcu.get_transaction_charge_screen_ord();
                }
                else
                {
                    calcu.reload_computation_entries(isOBPS);
                    Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;
                    return calcu.get_transaction_charge_screen();
                }

            }
            if (Session["REMITTANCE_CALCULATOR_CLASS"] != null)
            {

                irsCalculator calc = (irsCalculator)Session["REMITTANCE_CALCULATOR_CLASS"];
                //irsCalculator calc =new  irsCalculator(TranxID);
                //calc.reload_computation_entries(isOBPS);
                //Session["REMITTANCE_CALCULATOR_CLASS"] = calc;
                if (this.getLocation() == "1170")
                {
                    return calc.get_transaction_charge_screen_ord();
                }
                else
                {
                    return calc.get_transaction_charge_screen();
                }
            }
            return "<center><b>REMITTANCE CALCULATOR IS NOT PROPERLY INITIALIZED. MAKE SURE THAT THE AMOUNT ENTERED AND CHARGE TYPE ARE VALID.</b></center>";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string WaivedCharges(bool isOBPS)
        {

            try
            {
                irsCalculator calcu = new irsCalculator(TranxID);
                string param = "";
                if (this.getLocation() == "1170")
                {
                    calcu.reload_computation_and_waived_charges(isOBPS, true);
                }
                else
                { calcu.reload_computation_and_waived_charges(isOBPS); }

                Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;
                //param += calcu.get_remittance_net_branch().ToString() + "~";
                //param += calcu.get_remittance_gross_branch().ToString() + "~";
                //param += calcu.get_remittance_total_charges().ToString() + "~";
                //param += calcu.get_remittance_net_to_bene_amt().ToString() + "~";
                //param += calcu.get_remittance_net_remit_php_equivalent().ToString() + "~";
                //param += calcu.get_branch_local_currency().ToString() + "~";
                //param += calcu.get_forex_remitting_to_account().ToString() + "~";
                //param += calcu.get_total_benef_charges().ToString();

                param += calcu.get_remittance_net_branch().ToString() + "~";
                param += calcu.get_remittance_gross_branch().ToString() + "~";
                param += calcu.get_remittance_total_charges().ToString() + "~";
                param += calcu.get_remittance_net_to_bene_amt().ToString() + "~";
                param += calcu.get_remittance_net_remit_php_equivalent().ToString() + "~";
                param += calcu.get_branch_local_currency().ToString() + "~";
                param += calcu.get_forex_remitting_to_account().ToString() + "~";
                param += calcu.get_total_backend_charges().ToString() + "~";
                param += calcu.get_forex_target_to_source().ToString() + "~";
                param += calcu.get_forex_source_to_target().ToString() + "~";
                param += calcu.get_forex_source_to_php().ToString() + "~";
                param += calcu.get_forex_source_to_usd().ToString() + "~";
                param += calcu.get_forex_target_to_php().ToString() + "~";
                param += calcu.get_forex_target_to_usd().ToString() + "~";
                param += calcu.get_forex_target_to_eur().ToString() + "~";
                param += calcu.get_total_benef_charges().ToString();
                return param;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string compute_override(bool isOBPS)
        {

            try
            {
                irsCalculator calcu = (irsCalculator)Session["REMITTANCE_CALCULATOR_CLASS"];
                string param = "";
                if (this.getLocation() == "1170")
                {
                    calcu.reload_computation_and_override_rates_charges(isOBPS, true);
                }
                else
                {
                    calcu.reload_computation_and_override_rates_charges(isOBPS);
                }
                param += calcu.get_remittance_net_branch().ToString() + "~";
                param += calcu.get_remittance_gross_branch().ToString() + "~";
                param += calcu.get_remittance_total_charges().ToString() + "~";
                param += calcu.get_remittance_net_to_bene_amt().ToString() + "~";
                param += calcu.get_remittance_net_remit_php_equivalent().ToString() + "~";
                param += calcu.get_branch_local_currency().ToString() + "~";
                param += calcu.get_forex_remitting_to_account().ToString() + "~";
                param += calcu.get_total_backend_charges().ToString() + "~";
                param += calcu.get_forex_target_to_source().ToString() + "~";
                param += calcu.get_forex_source_to_target().ToString() + "~";
                param += calcu.get_forex_source_to_php().ToString() + "~";
                param += calcu.get_forex_source_to_usd().ToString() + "~";
                param += calcu.get_forex_target_to_php().ToString() + "~";
                param += calcu.get_forex_target_to_usd().ToString() + "~";
                param += calcu.get_forex_target_to_eur().ToString() + "~";
                param += calcu.get_total_benef_charges().ToString();

                return param;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            { }
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        //public DataTable Controls(int implemtype, int productcode)
        //{
        //    SqlCommand sqcommands = null;
        //    sqlHelperWs sql = new sqlHelperWs();

        //    try
        //    {
        //        return sql.execStoredProcedureTable("sp_getImplemProduct_Controls", new object[] { implemtype, productcode });
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //    finally
        //    {
        //        if (sqcommands != null)
        //        {
        //            sqcommands.Dispose();
        //            sqcommands = null;
        //        }
        //    }
        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable RemittanceDetails(int seqid)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();

            try
            {
                return sql.execStoredProcedureTable("sp_getRemittance_details", new object[] { seqid });

            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_product_setup_code(string branch_code, int product_code, int implementation_type, int payment_type, int currency, int charge_type)
        {


            sqlHelperWs sqlkonek = new sqlHelperWs();
            sqlkonek.EnforceConstraint = false;
            SqlCommand command = sqlkonek.getSqlCommand("sp_otc_rem_product_setup");

            command.Parameters.Add("@keycode", SqlDbType.VarChar).Value = "PRODUCTGROUPING";
            command.Parameters.Add("@branch_code", SqlDbType.VarChar).Value = branch_code.Replace("'", "''");
            command.Parameters.Add("@product_code", SqlDbType.BigInt).Value = product_code;
            command.Parameters.Add("@implementation_type", SqlDbType.BigInt).Value = implementation_type;
            command.Parameters.Add("@payment_type", SqlDbType.BigInt).Value = payment_type;
            command.Parameters.Add("@currency", SqlDbType.BigInt).Value = currency;
            command.Parameters.Add("@charge_type", SqlDbType.BigInt).Value = charge_type;
            command.Parameters.Add("@computation_direction", SqlDbType.BigInt).Value = 0;
            command.Parameters.Add("@receipt_mode", SqlDbType.BigInt).Value = 0;
            command.Parameters.Add("@purpose", SqlDbType.BigInt).Value = 0;
            command.Parameters.Add("@transaction_source", SqlDbType.BigInt).Value = 0;
            command.Parameters.Add("@source_of_fund", SqlDbType.BigInt).Value = 0;


            return sqlkonek.getDataSet(command, "PRODUCT_SETUP_CODE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string AccountInquiry(string AccountNo)
        {
            irsAccountInquiry inquire = new irsAccountInquiry();
            return inquire.getFC(AccountNo, irsAccountInquiry.FCMethod.AccountStatusInquiry, 0, true);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_charge_id(string branch_code, int product_code, int implementation_type, int payment_type, int currency, int charge_type, int compute_mode)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            sqlkonek.EnforceConstraint = false;

            SqlCommand command = sqlkonek.getSqlCommand("sp_otc_rem_product_setup");

            command.Parameters.Add("@keycode", SqlDbType.VarChar).Value = "CHARGESEQUENCE";
            command.Parameters.Add("@branch_code", SqlDbType.VarChar).Value = branch_code.Replace("'", "''");
            command.Parameters.Add("@product_code", SqlDbType.BigInt).Value = product_code;
            command.Parameters.Add("@implementation_type", SqlDbType.BigInt).Value = implementation_type;
            command.Parameters.Add("@payment_type", SqlDbType.BigInt).Value = payment_type;
            command.Parameters.Add("@currency", SqlDbType.BigInt).Value = currency;
            command.Parameters.Add("@charge_type", SqlDbType.BigInt).Value = charge_type;
            command.Parameters.Add("@computation_direction", SqlDbType.BigInt).Value = compute_mode;
            command.Parameters.Add("@receipt_mode", SqlDbType.BigInt).Value = 0;
            command.Parameters.Add("@purpose", SqlDbType.BigInt).Value = 0;
            command.Parameters.Add("@transaction_source", SqlDbType.BigInt).Value = 0;
            command.Parameters.Add("@source_of_fund", SqlDbType.BigInt).Value = 0;

            return sqlkonek.getDataSet(command, "COMPUTE_MODE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string compute_remittance(int product_charge_id, int tranx_id)
        {
            int computation_direction = 3;

            if (Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] != null)
            {
                try
                {
                    computation_direction = (int)Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"];
                }
                catch
                {
                    Session["CURRENT_TRANSACTION_PRODUCT_CHARGES_COMPUTATION_DIRECTION_HANDLER"] = null;
                }
            }

            irsCalculator calc = new irsCalculator(product_charge_id, tranx_id, computation_direction);

            calc.compute_irsw_charges();

            irsOverrideLog irs = new irsOverrideLog("", ""); irs.ResetLogSession();

            Session["REMITTANCE_CALCULATOR_CLASS"] = calc;

            return calc.get_remittance_net_to_bene_amt().ToString("#,###.00") + "~" +
                    calc.get_remittance_total_charges_amt().ToString("#,###.00") + "~" +
                    calc.get_remittance_net_to_bene_amt_php_equivalent().ToString("#,###.00") + "~" +
                    calc.get_beneficiary_account_currency() + "~" +
                    calc.get_branch_local_currency() + "~" +
                    calc.get_remittance_gross_branch() + "~" +
                    calc.get_remittance_net_branch() + "~" +
                    calc.get_remittance_net_to_bene_amt() + "~" +
                    calc.get_remittance_total_charges_amt() + "~" +
                    calc.get_forex_target_to_account() + "~" +
                    calc.get_forex_target_to_source() + "~" +
                    calc.get_forex_target_to_usd() + "~" +
                    calc.get_forex_target_to_php() + "~" +
                    calc.get_forex_target_to_eur() + "~" +
                    calc.get_forex_source_to_target() + "~" +
                    calc.get_forex_source_to_usd() + "~" +
                    calc.get_forex_source_to_php() + "~" +
                    calc.get_total_backend_charges();
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_product_setup_description(int product_code, int implementation_type, int payment_type, int currency, int charge_type, int computation_direction, int receipt_mode, int purpose, int transaction_source, int source_of_fund)
        {
            string[] array_branch_code = this.get_user_branchcode().Split('~');
            string branch_code = string.Empty;

            if (array_branch_code != null && array_branch_code.Length > 0)
            {
                branch_code = array_branch_code[0];
            }
            sqlHelperWs sqlkonek = new sqlHelperWs();

            sqlkonek.EnforceConstraint = false; SqlCommand command = sqlkonek.getSqlCommand("sp_otc_rem_product_setup");

            command.Parameters.Add("@keycode", SqlDbType.VarChar).Value = "DISPLAY_DESC";
            command.Parameters.Add("@branch_code", SqlDbType.VarChar).Value = branch_code.Replace("'", "''");
            command.Parameters.Add("@product_code", SqlDbType.BigInt).Value = product_code;
            command.Parameters.Add("@implementation_type", SqlDbType.BigInt).Value = implementation_type;
            command.Parameters.Add("@payment_type", SqlDbType.BigInt).Value = payment_type;
            command.Parameters.Add("@currency", SqlDbType.BigInt).Value = currency;
            command.Parameters.Add("@charge_type", SqlDbType.BigInt).Value = charge_type;
            command.Parameters.Add("@computation_direction", SqlDbType.BigInt).Value = computation_direction;
            command.Parameters.Add("@receipt_mode", SqlDbType.BigInt).Value = receipt_mode;
            command.Parameters.Add("@purpose", SqlDbType.BigInt).Value = purpose;
            command.Parameters.Add("@transaction_source", SqlDbType.BigInt).Value = transaction_source;
            command.Parameters.Add("@source_of_fund", SqlDbType.BigInt).Value = source_of_fund;

            return sqlkonek.getDataSet(command, "PRODUCT_SETUP_DESCRIPTION");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_user_branchcode()
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]; return loggin_data.Location + "~" + new irsTimeZone().branchLocalTime(loggin_data.Location).ToString("MM/dd/yyyy");
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_lookup_description(int fldvalue, string fldgroup)
        {

            sqlHelperWs sqlkonek = new sqlHelperWs();

            sqlkonek.EnforceConstraint = false; SqlCommand command = sqlkonek.getSqlCommand("sp_get_description_from_lookup");

            command.Parameters.Add("@fldvalue", SqlDbType.BigInt).Value = fldvalue;
            command.Parameters.Add("@fldgroup", SqlDbType.VarChar).Value = fldgroup;


            return sqlkonek.getDataSet(command, "PRODUCT_SETUP_DESCRIPTION");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_account_details(int seqid)
        {

            sqlHelperWs sqlkonek = new sqlHelperWs();

            sqlkonek.EnforceConstraint = false; SqlCommand command = sqlkonek.getSqlCommand("sp_get_account_details");

            command.Parameters.Add("@seq_id", SqlDbType.BigInt).Value = seqid;
            DataSet dt = sqlkonek.getDataSet(command, "PRODUCT_SETUP_DESCRIPTION");
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_product_and_computed_charges(int seqid)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    //sqcommands = commons.getSqlCommand("sp_trans_update_fc_status");
                    //sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = transid;
                    //sqcommands.Parameters.Add("@fcmessage", SqlDbType.VarChar, 200).Value = FCmessage;
                    //System.Web.HttpContext.Current.Session["Transtatus"]= 
                    commons.execScalar("sp_calculator_delete_product_computed_charges", new object[] { seqid });
                    return true;
                }
                return false;
            }

            catch //(Exception ex) // commented to remove warning
            {
                return false;
            }

            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }

            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReceiptHeader(string brcode)
        {
            try
            {
                return new irsReceipts().getReceipts(brcode);
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "View/Ammendments", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchOperations()
        {
            return new irsInternetPage().getBranchOperations(new irsInternetPage().getBranchValue(getLocation(), "BranchCode", getLocation(), "SEQ_ID"));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemittance(string ReferenceNo, bool isOBPS)
        {
            if (isOBPS)
            {
                return new irsOBPSTransactions().getRemittance(ReferenceNo);
            }
            else
            {
                return new irsInternetPage().getRemittance(ReferenceNo);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isPNBReceipt(string BranchCode)
        {
            return new irsInternetPage().isPNBReceipt(getLocation());
        }
    }
}

    


