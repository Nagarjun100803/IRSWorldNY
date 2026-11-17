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
    public partial class RemTranForVerification : System.Web.UI.Page
    {
        private PrintDocument printDoc = new PrintDocument();
        private PageSettings pgSettings = new PageSettings();
        private PrinterSettings prtSettings = new PrinterSettings();

        private int tranxid = 0;
        private int FOR_APRROVAL = 15;
        private int CREDITED_ONLINE = 2;
        private int FOR_REPROCESS = 9;
        private int POSTED_CENTRAL = 5;

        private object tranx_status
        {
            set { Session["TranxStatus"] = value; }
            get { return Session["TranxStatus"]; }
        }

        private object status_message
        {
            set { Session["StatusMessage"] = value; }
            get { return Session["StatusMessage"]; }
        }

        private object rem_seq_id
        {
            set { Session["RemSeqId"] = value; }
            get { return Session["RemSeqId"]; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranForVerification));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCommons));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_account_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_open_accounts));

            Session["TranxID"] = Request.QueryString["remittance_txn_id"];
            Session["sourcepage"] = Request.QueryString["param_values"];
            //irsCalculator calcu = new irsCalculator(TranxID);
            //Session["REMITTANCE_CALCULATOR_CLASS"] = calcu;

            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        public int TranxID
        {
            get { return int.Parse(Session["TranxID"].ToString()); }
            set { tranxid = value; }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getDefaultValues()
        {
            return (Object)Session["defaultparamvalues"];
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
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();

            try
            {
                sqcommands = sql.getSqlCommand("sp_viewRemittance_details");
                sqcommands.Parameters.Add("@tranxid", SqlDbType.BigInt).Value = Session["TranxID"];
                return sql.getDataSet(sqcommands, "Remittance_INFOS");
            }
            catch  // commented to remove warning (Exception e)
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
        public DataSet Remit(int seqid, bool isOBPS, bool reimplementation)
        {
            irsRemTeller rem = new irsRemTeller();

            DataSet paramMsg = new DataSet();
            // commented to remove warning - string paramMessage = "";
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

                    paramMsg = rem.Remit2(seqid);
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
                    paramMsg = rem.Reversal(seqid);
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
            // commented to remove warning - int paramMsg;
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
        public bool set_status(string status, string seq_id)
        {
            tranx_status = status;
            rem_seq_id = seq_id;

            return (tranx_status != null || tranx_status.ToString().Length > 0);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_status()
        {
            sqlHelperWs sql = new sqlHelperWs();
            DataTable dt = null;
            try {
                dt = sql.execStoredProcedureTable("sp_get_for_verification_status", new object[] { });
                return dt;
            }
            catch { }
            finally { dt = null; }

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_status_HO()
        {
            sqlHelperWs sql = new sqlHelperWs();
            DataTable dt = null;
            try
            {
                dt = sql.execStoredProcedureTable("usp_get_for_verification_status_HO", new object[] { });
                return dt;
            }
            catch { }
            finally { dt = null; }

            return dt;
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public String saveStatusGenerika(int productCode)
        //{
        //    irsOBPSTransactions ob = new irsOBPSTransactions();
        //    sqlHelperWs sql = new sqlHelperWs();
        //    int res = -1;
        //    string branch = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        //    irsRemForVerification sav_rem = new irsRemForVerification(Convert.ToInt32(this.rem_seq_id), productCode, branch);
        //    string msg = string.Empty;

        //    if ((string)tranx_status == "CO" || (string)tranx_status == "FBP")
        //    {
        //        try
        //        {
        //            if (tranx_status != null || tranx_status.ToString().Length > 0)
        //            {
        //                if (ob.sendGenerika(ob.getTranxHistory(Convert.ToInt32(this.rem_seq_id)).Rows[0]["ReferenceNo"].ToString()) == true)
        //                {
        //                    res = sav_rem.update_status((string)tranx_status);
        //                    if (res > 0)
        //                    {
        //                        if ((string)tranx_status == "CO")
        //                        {
        //                            sav_rem.save_other_system();
        //                        }
        //                    }
        //                    msg = "Update successful" + "\n" + (string)Session["OBPSBayadCenterResponseMessage"];
        //                }
        //                else
        //                {
        //                    msg = "Update Failed" + "\n" + (string)Session["OBPSBayadCenterResponseMessage"];
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            msg = e.Message;
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            if (tranx_status != null || tranx_status.ToString().Length > 0)
        //            {
        //                res = sav_rem.update_status((string)tranx_status);
        //                if (res > 0)
        //                {
        //                    if ((string)tranx_status == "CO")
        //                    {
        //                        sav_rem.save_other_system();
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            msg = e.Message;
        //        }
        //        finally
        //        {
        //            if (res < 1) msg = "Please select status.";
        //            tranx_status = null;
        //        }
        //    }
        //    return msg;
        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int save_status(int product_code)
        {
            sqlHelperWs sql = new sqlHelperWs();
            csAuditLog Logger = new csAuditLog();
            int res = -1;
            string branch = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
            string userid = ((irsClientData)Session["IRS_CLIENT_DATA"]).UserId.ToString();
            irsRemForVerification sav_rem = new irsRemForVerification(Convert.ToInt32(this.rem_seq_id), product_code, branch);

            try
            {
                if (tranx_status != null || tranx_status.ToString().Length > 0)
                {
                    res = sav_rem.update_status((string)tranx_status);//sav_rem.execStoredProcedureUpdate("sp_update_remittance_status", new object[] { (string)tranx_status, (string)rem_seq_id });
                    if (res > 0)
                    {
                        if ((string)tranx_status == "CO")
                        {
                            try
                            {
                                sav_rem.save_other_system();
                            }
                            catch (Exception ex)
                            {
                                Logger.writeAudit(userid, "IRSWorldForVerification", "Error on Update: save_other_system", ex.Message.ToString(),string.Empty,csAuditLog.Category.Update);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.writeAudit(userid, "IRSWorldForVerification", "Error on Update", ex.Message.ToString(), string.Empty, csAuditLog.Category.Update);
            }
            finally 
            {
                if (res < 1) status_message = "Please select status.";
                tranx_status = null;
            }
            return res;
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
            query_parameters = "?sourcepage=RemTranForVerification";

            try
            {
                //sqcommands = sql.getSqlCommand("sp_txn_search_txn_pages");
                //sqcommands.Parameters.Add("@product_charge_id", SqlDbType.BigInt).Value = product_charge_id;

                //DataTable dt = sql.getDataSet(sqcommands, "REMITTANCE_PAGE").Tables[0];

                //if (dt != null && dt.Rows.Count > 0)
                //{

                return "../Remittance/RemTranForVerificationSearch.aspx" + query_parameters;
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
                else if (merchcode.Trim().ToUpper() == "GENERIKA")
                {
                    dsbayadcenter = ob.getLookUp("GenerikaAPI_Request");
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
                                bayadcenterrequest = bayadcenterrequest.Replace("[" + dsgetOBPData.Tables[0].Rows[i][0].ToString() + "]", dsgetOBPData.Tables[0].Rows[i][1].ToString().Replace("-", ""));

                                if (dsgetOBPData.Tables[0].Rows[i][0].ToString() == "Target Amount")
                                {
                                    bayadcenterrequest = bayadcenterrequest.Replace("[amount]", Convert.ToDecimal(dsgetOBPData.Tables[0].Rows[i][1].ToString()).ToString("####"));
                                    bayadcenterrequest = bayadcenterrequest.Replace("[keyword]", "RE-PROCESS");
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
                sqlHelperWs sql = new sqlHelperWs();
                DataSet ds = sql.getDataSet(sql.getSqlCommand(SQL, false), "FileInd");

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
            catch // commented to remove warning - (Exception e)
            {
                return false;
            }

            finally
            { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ShowReport(int remitmethod, bool isOBPS)
        {
            // commented to remove warning - string report_path = "";
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
            catch // commented to remove warning - (Exception ex)
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

            catch // commented to remove warning - (Exception ex)
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
        public string getMerchantType()
        {
            if (Session["OBPSMerchantType"] != null)
            {
                return (string)Session["OBPSMerchantType"];
            }
            else
            {
                return "";
            }
        }
    }
}
