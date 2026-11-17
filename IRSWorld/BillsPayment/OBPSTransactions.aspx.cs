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
using IRSClassLib;
using System.Data.SqlClient;
using IRSWorld.TranClass;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using IRSWorld.Remittance;
using IRSWorld.Internet;
using System.Threading.Tasks;
using System.Linq;

namespace IRSWorld.BillsPayment
{
    [Serializable]
    public partial class OBPSTransactions : System.Web.UI.Page
    {
        private sqlHelperWs sqlkonek = new sqlHelperWs();
        private csAuditLog logger = new csAuditLog();

        private irsOBPSTransactions irsOBPSTransactions
        {
            get
            {
                if (Session["OBPSTransaction"] == null)
                {
                    Session["OBPSTransaction"] = new irsOBPSTransactions();
                }
                return (irsOBPSTransactions)Session["OBPSTransaction"];
            }
        }

        /* RFC-27195 GIFTAWAY */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void process_order_giftaway(string reference_no, string merchant_id, string merchant_deno, string merchant_qty, string recipient_name, string recipient_mobile, string recipient_email, string sender_name, string personal_msg)
        {
            csGiftAway giftaway = null;

            string api_response = string.Empty;
            string sender_fullname = string.Empty;    /* NEW VARIABLE */
            string qrystring = "select top 1 isnull(firstname, '') firstname , isnull(middlename, '') middlename , isnull(lastname, '') lastname from tbl_cif_master with (nolock) where cifid = (select top 1 remittercifid from tbl_remittance with (nolock) where referenceno ='" + reference_no + "')";

            DataTable dt = null;

            /* NEW SENDER NAME TO SET NAME INTO THE FOLLOWING ORDER FIRST, MIDDLE, LAST */
            try
            {
                dt = sqlkonek.getDataSet(qrystring, false, new object[] { }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    sender_fullname = (string)dt.Rows[0]["firstname"] + ' ' +
                                      (string)dt.Rows[0]["middlename"] + ' ' +
                                      (string)dt.Rows[0]["lastname"];
                }
                else
                {
                    sender_fullname = sender_name;
                }
            }
            catch
            {
                sender_fullname = sender_name;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
            /* NEW SENDER NAME TO SET NAME INTO THE FOLLOWING ORDER FIRST, MIDDLE, LAST */

            try
            {
                giftaway = new csGiftAway();

                giftaway.transaction_refno = reference_no;
                giftaway.merchant_id = merchant_id;
                giftaway.merchant_denomination = merchant_deno;
                giftaway.merchant_quantity = merchant_qty;
                giftaway.recipient_name = recipient_name;
                giftaway.recipient_mobile = recipient_mobile;
                giftaway.recipient_email = recipient_email;
                giftaway.personal_msg = personal_msg;
                giftaway.sender_name = sender_fullname; /* REPLACED WITH VALUE*/

                giftaway.place_order();
            }
            catch
            {
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_giftway(string merchant_id, string merchant_name, string merchant_deno, string merchant_qty, string recipient_name, string recipient_mobile, string recipient_email, string giftaway_special_msg)
        {
            string api_response = string.Empty;

            csGiftAway giftaway = null;

            string giftaway_net_benef_amt = string.Empty;
            string giftaway_fee = string.Empty;

            if (string.IsNullOrEmpty(merchant_name) == true)
            {
                return "Please select Merchant";
            }

            if (string.IsNullOrEmpty(recipient_name) == true)
            {
                return "Please enter Recipient name";
            }

            if (string.IsNullOrEmpty(merchant_deno) == true)
            {
                return "Please select Denomination";
            }

            if (string.IsNullOrEmpty(merchant_qty) == true)
            {
                return "Please select Quantity";
            }

            /* NEW VALIDATION FOR MOBILE NUMBER*/
            if (string.IsNullOrEmpty(recipient_mobile) == false && recipient_mobile.Length != 11)
            {
                return "Please enter 11-digit Recipient Mobile Number.";
            }

            /* NEW VALIDATION FOR MOBILE NUMBER*/

            if (string.IsNullOrEmpty(recipient_mobile) == true && string.IsNullOrEmpty(recipient_email) == true)
            {
                return "Please enter valid Recipient Mobile Number or Email Address";
            }

            if (string.IsNullOrEmpty(giftaway_special_msg) == false && giftaway_special_msg.Length > 300)
            {
                return "Personal Message should not be more than 300 characters in length.";
            }

            try
            {
                giftaway = new csGiftAway();
                giftaway_fee = giftaway.giftaway_fee;

                giftaway.merchant_id = merchant_id;
                giftaway.merchant_name = merchant_name;
                giftaway.merchant_denomination = merchant_deno;
                giftaway.merchant_quantity = merchant_qty;

                api_response = giftaway.inquire_merchant_info();
            }
            catch
            {
                return "Unable to receive response from GIFTAWAY.";
            }

            if (api_response == null || api_response == string.Empty)
            {
                return "Unable to connect to GIFTAWAY";
            }

            if (api_response != null && api_response.ToUpper().IndexOf("ERROR") >= 0)
            {
                return api_response;
            }

            giftaway_net_benef_amt = ((Convert.ToDecimal(merchant_deno) *
                                       Convert.ToInt32(merchant_qty)) +
                                       Convert.ToDecimal(giftaway_fee)).ToString();

            return "OK~" + giftaway_net_benef_amt;
        }
        /* RFC-27195 GIFTAWAY */

        /* RFC-25832 PAG-IBIG API INTEGRATION */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void generate_pagibig_api_ext_ref(string referenceno)
        {
            int i = sqlkonek.execStoredProcedureUpdate("obps_pagibig_set_externalref", new object[] { referenceno });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string inquire_pagibig(string member_id, string pagibig_merchantcode)
        {
            csPagIbigAPI pagibig_api = null;
            DataTable dt_api = null;

            string api_return = string.Empty;

            string xlname = string.Empty;
            string xfname = string.Empty;
            string xmid = string.Empty;
            string xname_e = string.Empty;
            string xarea_code = string.Empty;
            string xtel_no = string.Empty;
            string xemail = string.Empty;
            string xdeduction = string.Empty;
            string xamount_due = string.Empty;
            string xdue_date = string.Empty;
            string xpaytype = string.Empty;

            try
            {
                pagibig_api = new csPagIbigAPI();
                api_return = pagibig_api.inquire_pagibig(member_id, pagibig_merchantcode);

                if (api_return != null && api_return.IndexOf("OK") >= 0)
                {
                    dt_api = pagibig_api.get_pag_ibig_member_information();
                }

                if (dt_api != null && dt_api.Rows.Count > 0)
                {
                    xlname = this.convert_null_to_empty(dt_api.Rows[0]["xlname"].ToString());
                    xfname = this.convert_null_to_empty(dt_api.Rows[0]["xfname"].ToString());
                    xmid = this.convert_null_to_empty(dt_api.Rows[0]["xmid"].ToString());
                    xname_e = this.convert_null_to_empty(dt_api.Rows[0]["xname_e"].ToString());
                    xarea_code = this.convert_null_to_empty(dt_api.Rows[0]["xarea_code"].ToString());
                    xtel_no = this.convert_null_to_empty(dt_api.Rows[0]["xtel_no"].ToString());
                    xemail = this.convert_null_to_empty(dt_api.Rows[0]["xemail"].ToString());
                    xdeduction = this.convert_null_to_empty(dt_api.Rows[0]["xdeduction"].ToString());
                    xamount_due = this.convert_null_to_empty(dt_api.Rows[0]["xamount_due"].ToString());
                    xdue_date = this.convert_null_to_empty(dt_api.Rows[0]["xdue_date"].ToString());
                    xpaytype = this.convert_null_to_empty(dt_api.Rows[0]["xpaytype"].ToString());

                    api_return += "|" + xlname;
                    api_return += "|" + xfname;
                    api_return += "|" + xmid;
                    api_return += "|" + xname_e;
                    api_return += "|" + xarea_code;
                    api_return += "|" + xtel_no;
                    api_return += "|" + xemail;
                    api_return += "|" + xdeduction;
                    api_return += "|" + xamount_due;
                    api_return += "|" + xdue_date;
                    api_return += "|" + xpaytype;
                    api_return += "|" + this.convert_null_to_empty(pagibig_api.api_response_message());
                }

                return api_return;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
            finally
            {
                pagibig_api = null;
            }
        }

        private string convert_null_to_empty(string s)
        {
            try
            {
                if (s != null && s != string.Empty)
                {
                    return s.Replace("|", " ");
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void clear_session()
        {
            System.Web.HttpContext.Current.Session["OBBA_OVERRIDEORIGINALAMOUNT"] = null;
            System.Web.HttpContext.Current.Session["OBBA_CHARGENAME"] = null;
            System.Web.HttpContext.Current.Session["OBBA_COMPUTEDTAX"] = null;
            System.Web.HttpContext.Current.Session["IRS_OVERRIDE_LOGDATA"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_mp2_amt(string amount, string dt_from, string dt_to)
        {
            DataTable dt = null;

            try
            {
                dt = sqlkonek.getDataSet("obps_validate_mp2_amt", true, new object[] { Convert.ToDecimal(amount), Convert.ToDateTime(dt_from), Convert.ToDateTime(dt_to) }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }

                return "Please input valid MP2 amount.";
            }
            catch (Exception e)
            {
                return "Unable to validate validate_mp2_amt: " + e.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string pagibig_housing_prn_paid(string housing_prn)
        {
            DataTable dt = null;

            try
            {
                dt = sqlkonek.getDataSet("obps_housing_prn_paid", true, new object[] { housing_prn }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }

                return string.Empty;
            }
            catch (Exception e)
            {
                return "Unable to validate pagibig_housing_prn_paid: " + e.Message;
            }
        }
        /* RFC-25832 PAG-IBIG API INTEGRATION */


        /* A. RFC-12313 SSS WILL NOW BE IN USD 2021 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void post_sss_loancontri_prn(string prn_number, string reference_number, string merchant_code)
        {
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            SSSPRN_API prn = null;
            DataTable dt = null;
            DataTable dt_InstructionIDDetails = null;
            string instructionid = string.Empty;
            string prn_details = string.Empty;
            string transaction_datetime = string.Empty;
            string rtpFlag = "0";
            string api_reply_msg = string.Empty;
            string amtDue = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(merchant_code).Equals(false) && clientdata != null)
                {
                    if (merchant_code.Trim().ToUpper() == "SSS CONTRIBUTION" || merchant_code.Trim().ToUpper() == "SSS LOAN")
                    {
                        transaction_datetime = new irsTimeZone().branchLocalTime(clientdata.Location).ToString("MM/dd/yyyy HH:mm:ss");
                        /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                        rtpFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
                        if (rtpFlag == "1")
                        {
                            dt_InstructionIDDetails = sqlkonek.getDataSet("getRTPInstructionID_details", true, new object[] { prn_number, "" }).Tables[0];
                            if (dt_InstructionIDDetails != null && dt_InstructionIDDetails.Rows.Count > 0)
                            {
                                amtDue = dt_InstructionIDDetails.Rows[0]["Amount"].ToString();
                                instructionid = dt_InstructionIDDetails.Rows[0]["InstructionID"].ToString();
                            }
                            api_reply_msg = this.InquireRTP("", prn_number, amtDue, "BC_SSS", instructionid, reference_number, "POST");
                        }
                        else
                        {
                            prn = new SSSPRN_API();
                            prn.PRNNumber = prn_number;
                            prn.TransactionDate = transaction_datetime;
                            prn.TransactionNumber = reference_number;
                            api_reply_msg = prn.submitRequest(SSSPRN_API.SSSPayment.SUBMIT_PRN);
                        }
                        /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                        if (api_reply_msg == null || api_reply_msg == "")
                        {
                            api_reply_msg = "No API reply";
                        }

                        prn_details = "PRN:" + prn_number + " |REFNO:" + reference_number + " |OVERSEASDATETIME:" + transaction_datetime + " |APIMSG:" + api_reply_msg;

                        if (prn_details.Length > 300)
                        {
                            prn_details = prn_details.Substring(0, 300);
                        }

                        try
                        {
                            int i = sqlkonek.execStoredProcedureUpdate("obps_insert_sss_post_prn", new object[] { reference_number, merchant_code, prn_details });
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                prn = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string primacasa_so_valid(string so_number)
        {
            string prefix = "2";
            string so_number_len = "10";

            try
            {
                prefix = new AppSettingsReader().GetValue("PRIMACASA_PREFIX", Type.GetType("System.String")).ToString();
            }
            catch
            {
                prefix = "2";
            }

            try
            {
                so_number_len = new AppSettingsReader().GetValue("PRIMACASA_LENGTH", Type.GetType("System.String")).ToString();
            }
            catch
            {
                so_number_len = "10";
            }

            if (so_number == null || so_number == string.Empty || so_number.Substring(0, 1) != prefix)
            {
                return "Please enter valid sales order number.";
            }

            if (so_number.Length.ToString().Trim() != so_number_len.Trim())
            {
                return "Please enter valid sales order number.";
            }

            Int64 O;
            if (Int64.TryParse(so_number, out O).Equals(false))
            {
                return "Please enter valid sales order number";
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string get_bsp_fxrate()
        {
            irsSqlCommonsWs bsprates;
            DataSet ds = null;

            string constring_suffix = "FXRatesConnectionString";

            try
            {
                System.Web.HttpContext.Current.Session["BILLER_IS_SSS_DOLLAR"] = 1;


                bsprates = new irsSqlCommonsWs(constring_suffix);
                bsprates.IsWinApp = true;

                ds = bsprates.getDataSet("getobps_bspfx_rate", true, new object[] { });

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToString(ds.Tables[0].Rows[0]["bsp_ref_rate"]);
                }
                
                if (bsprates.Message != null && bsprates.Message != "")
                {
                    return "ERROR:" + bsprates.Message;
                }             

                return "ERROR: BSP rate not yet available.";
            }
            catch (Exception e)
            {
                return "ERROR: " + e.Message;
            }
            finally
            {
                bsprates = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string get_sss_contri_bsprates_prndet(string prn)
        {
            SSSPRN_API contribution_prn = null; /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            string inquire_prn_response = string.Empty;
            string bsp_fxrates = string.Empty;
            string rtpFlag = "0";
            bsp_fxrates = this.get_bsp_fxrate();

            if (this.is_sss_loan_prn_prefix(prn) == true)
            {
                return "PRN is not valid for contribution payment. Please select the correct SSS biller.";
            }

            if (bsp_fxrates.IndexOf("ERROR") >= 0)
            {
                return bsp_fxrates.Replace("ERROR:", "").Trim();
            }

            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            rtpFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
            if (rtpFlag == "1")
            {
                try
                {
                    inquire_prn_response = this.InquireRTP("", prn, "0.00", "BC_SSS", "", "", "VALIDATE");
                    return inquire_prn_response + "~bsprate:" + bsp_fxrates;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                finally
                {
                    inquire_prn_response = null;
                }
            } /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            else
            {
                try
                {
                    contribution_prn = new SSSPRN_API();
                    contribution_prn.PRNNumber = prn;
                    inquire_prn_response = contribution_prn.submitRequest(SSSPRN_API.SSSPayment.INQUIRE_PRN);

                    return inquire_prn_response + "~bsprate:" + bsp_fxrates;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                finally
                {
                    contribution_prn = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string get_sss_loan_bsprates_prndet(string prn)
        {
            SSSPRN_API contribution_prn = null;
            string inquire_prn_response = string.Empty;
            string bsp_fxrates = string.Empty;
            string rtpFlag = "0";
            bsp_fxrates = this.get_bsp_fxrate();

            if (this.is_sss_loan_prn_prefix(prn) == false)
            {
                return "PRN is not valid for loan payment. Please select the correct SSS biller.";
            }

            if (bsp_fxrates.IndexOf("ERROR") >= 0)
            {
                return bsp_fxrates.Replace("ERROR:", "").Trim();
            }

            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            rtpFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
            if (rtpFlag == "1")
            {
                try
                {
                    inquire_prn_response = this.InquireRTP("", prn, "0.00", "BC_SSS", "", "", "VALIDATE");
                    return inquire_prn_response + "~bsprate:" + bsp_fxrates;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                finally
                {
                    inquire_prn_response = null;
                }
            } /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            else
            {
                try
                {
                    contribution_prn = new SSSPRN_API();
                    contribution_prn.PRNNumber = prn;
                    inquire_prn_response = contribution_prn.submitRequest(SSSPRN_API.SSSPayment.INQUIRE_PRN);
                    return inquire_prn_response + "~bsprate:" + bsp_fxrates;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                finally
                {
                    contribution_prn = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string sss_real_estate_chk_amount(string amount)
        {
            if (string.IsNullOrEmpty(amount).Equals(true))
            {
                return "Please enter valid amount.";
            }

            decimal O;

            if (decimal.TryParse(amount, out O).Equals(false))
            {
                return "Please enter valid amount.";
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string sss_peso_fund_chk_amount(string sss_number, string peso_amount)
        {
            DataTable dt = null;
            SSSPRN_API pesofund = null;
            string return_msg = string.Empty;
            string phpfund = string.Empty;
            string rtpFlag = "0";
            try
            {
                rtpFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
                if (rtpFlag == "1")
                {
                    if (string.IsNullOrEmpty(peso_amount).Equals(true))
                    {
                        return "Please enter valid amount.";
                    }
                }
                else
                {
                    dt = sqlkonek.getDataSet("chk_sss_peso_fund_amt", true, new object[] { sss_number, peso_amount }).Tables[0];

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        phpfund = dt.Rows[0]["peso_msg"].ToString();
                    }

                    if (string.IsNullOrEmpty(phpfund).Equals(false))
                    {
                        return phpfund;
                    }

                    pesofund = new SSSPRN_API();
                    string s = pesofund.PesoFund(peso_amount);

                    switch (s.ToUpper())
                    {
                        case "1000": return_msg = "Minimum amount is P1,000.00"; break;
                        case "0": return_msg = "Amount should be multiple of P100.00"; break;
                        default: return_msg = string.Empty; break;
                    }
                }

                return return_msg;
            }
            catch (Exception e)
            {
                return e.Message;
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

        /* A. RFC-12313 SSS WILL NOW BE IN USD 2021 */

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OBPSTransactions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCommons));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_check_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_card_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_debit_account_screen));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_other_details));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_denomination));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_ach_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OBPSAgentOverride));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_states_chk));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Refinitiv)); /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemOTC));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetPage));

            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);

            /* BOYET FEB 10 2019 */
            try
            {
                string obp_cached = new System.Configuration.AppSettingsReader().GetValue("OBPS_CACHED", Type.GetType("System.String")).ToString();

                if (obp_cached != null && obp_cached != string.Empty && obp_cached == "1")
                {
                    this.Page.Response.Expires = 0;
                    this.Page.Response.Cache.SetNoStore();
                    this.Page.Response.AppendHeader("Pragma", "no-cache");
                }
            }
            catch
            {
            }
            System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = null;
            /* BOYET FEB 10 2019 */

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();
                DataSet ds = null;

                if (SQL.IndexOf("SELECT") == -1)
                {
                    string[] param = SQL.Split('~');
                    string sp_name = param[0];
                    string sp_value = null;
                    object[] array_value = null;

                    if (param.Length - 1 != 1)
                    {
                        for (int i = 0; i < param.Length - 1; i++)
                        {

                            sp_value += param[i + 1] + "~";
                        }

                        array_value = sp_value.Split('~');
                    }
                    else
                    {
                        array_value = param[1].Split('~');
                    }
                    ds = com.execStoredProcedure(sp_name, array_value);
                }
                else
                {
                    ds = com.getDataSet(SQL, false, new object[] { });//Performance consideration when using irsSqlCommonsWS and sqlHelperWS
                }

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getOBPData(String sp, string merchcode, int prodcode)
        {
            try
            {
                return irsOBPSTransactions.getOptionalField(sp, merchcode, prodcode);
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemittingCurrency(int productchargesid)
        {
            return irsOBPSTransactions.getRemittingCurrency(productchargesid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getEnrollMerchantInfo(Int32 SEQID, string RemCIFID)
        {
            try
            {
                return irsOBPSTransactions.getEnrollMerchantInfo(SEQID, RemCIFID);
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFAccountCurrency(string merchantcode)
        {
            try
            {
                return irsOBPSTransactions.getCIFAccountCurrency(merchantcode);
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTranxHistory(int SEQ_ID)
        {
            return irsOBPSTransactions.getTranxHistory(SEQ_ID);
        }

        /* BOYET FEB 10 2019 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool saveOBPS(string datapair)
        {
            System.Web.HttpContext.Current.Session["OBPS_COMMIT_TRANSACTION_TABLE"] = null;

            try
            {
                if ((string)System.Web.HttpContext.Current.Session["RemittanceOBPS"] == "SUMBIT")
                {
                    System.Web.HttpContext.Current.Session["RemittanceOBPS"] = "";
                    System.Web.HttpContext.Current.Session["OBPS_COMMIT_TRANSACTION_TABLE"] = "ERROR SAVING.";
                    return false;
                }

                if (Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["OBPS_COMMIT_TRANSACTION_TABLE"] = "ERROR SAVING.";
                    return false;
                }

                bool sss_complete = this.sss_prn_details_complete(datapair);

                if (sss_complete == true)
                {
                    System.Web.HttpContext.Current.Session["RemittanceOBPS"] = "SUMBIT";
                    irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
                    return irsOBPSTransactions.saveOBPS(datapair, clientData.UserId);
                }
                else
                {
                    System.Web.HttpContext.Current.Session["OBPS_COMMIT_TRANSACTION_TABLE"] = "ERROR SAVING.";
                    return false;
                }
            }
            catch
            {
                System.Web.HttpContext.Current.Session["OBPS_COMMIT_TRANSACTION_TABLE"] = "ERROR SAVING.";
                return false;
            }
            finally
            {
                System.Web.HttpContext.Current.Session["RemittanceOBPS"] = "";
            }
        }
        /* BOYET FEB 10 2019 */

        /* BOYET FEB 10 2019 */
        public bool sss_prn_details_complete(string special_msg_field)
        {
            System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = null;

            /* ALWAYS RETURN TRUE IF TRANSACTION IS NON SSS PRN BILLS PAYMENT.  */

            irsClientData clientdata = null;
            string[] msg = null;
            string[] prn_inquiry_response = null;
            string prn_number = string.Empty;
            string sss_number = string.Empty;
            string prn_inquiry = string.Empty;
            string target_amount = string.Empty;
            decimal irsw_sss_amt = 0.00M;
            decimal webs_sss_amt = 0.00M;
            string rtpFlag = "0";
            try
            {
                clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                if (special_msg_field != null && special_msg_field != string.Empty)
                {

                    string[] sss_prn_array = special_msg_field.Split(',');

                    if (sss_prn_array != null && sss_prn_array.Length >= 22)
                    {
                        special_msg_field = sss_prn_array[22];
                    }

                    if (special_msg_field.IndexOf("PRN No") >= 0 && special_msg_field.IndexOf("SSS No") >= 0 && special_msg_field.IndexOf("tdCancel") >= 0)
                    {
                        msg = special_msg_field.Split('!');

                        if (msg != null && msg.Length > 0)
                        {
                            for (int i = 0; i < msg.Length; i++)
                            {
                                if (msg[i] != null && msg[i] != string.Empty)
                                {
                                    /* CHECK SPECIAL MSG FOR PRN NUMBER VALUE*/
                                    if (msg[i].IndexOf("PRN No^") >= 0 || msg[i].IndexOf("PRN Number^") >= 0)
                                    {
                                        string[] prn_arr = msg[i].Split('^');

                                        if (prn_arr != null && prn_arr.Length > 0)
                                        {
                                            prn_number = prn_arr[1];
                                        }
                                    }
                                    /* CHECK SPECIAL MSG FOR PRN NUMBER VALUE*/

                                    /* CHECK SPECIAL MSG FOR SSS NUMBER VALUE*/
                                    if (msg[i].IndexOf("SSS No^") >= 0 || msg[i].IndexOf("SSS Number^") >= 0)
                                    {
                                        string[] sss_arr = msg[i].Split('^');

                                        if (sss_arr != null && sss_arr.Length > 0)
                                        {
                                            sss_number = sss_arr[1];
                                        }
                                    }

                                    if (msg[i].IndexOf("Target Amount:") >= 0 || msg[i].IndexOf("Target Amount") >= 0)
                                    {
                                        string[] sss_arr = msg[i].Split(':');

                                        if (sss_arr != null && sss_arr.Length > 0)
                                        {
                                            target_amount = sss_arr[1];
                                        }
                                    }
                                    /* CHECK SPECIAL MSG FOR PRN NUMBER VALUE*/
                                }
                            }
                        }

                        /* NOW LETS RE-VALIDATE PRN NUMBER IF USER DID NOT CLICK VALIDATE BUTTON IN THE MERCHANT DETAILS FIELD FOR SSS PRN TRANSACTION AND PROCEED SEND PAYMENT */
                        rtpFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
                        if (rtpFlag == "1")
                        {
                            prn_inquiry = this.InquireRTP("", "", "0.00", "BC_SSS", this.getRTP_InstructionID(prn_number, ""), "", "GETSTATUS");
                            if (prn_inquiry == null || prn_inquiry == string.Empty)
                            {
                                return false; /* NOT VALID RESPONSE. NOT ABLE TO GET RESPONSE FROM SERVER */
                            }
                            if (prn_inquiry != null && prn_inquiry != string.Empty)
                            {
                                prn_inquiry_response = prn_inquiry.Split('~');
                            }
                            if (prn_inquiry_response == null)
                            {
                                return false;
                            }
                            if (prn_inquiry_response[0] == "VALIDATED")
                            {
                                return true;

                            }
                            if (prn_inquiry_response[0] != "01")
                            {
                                string msg_prn = string.Empty;
                                msg_prn = "\n";
                                msg_prn += "\nIRSW - PRN Number: " + prn_number;
                                System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = "\nCannot proceed with the transaction. Please click validate button under merchant information to get details. " + msg_prn;
                                new csAuditLog().writeAudit("", "SSS PRN", "Cannot proceed transaction. PRN details incomplete: " + msg_prn, "", "", csAuditLog.Category.Insert);
                                return false;
                            }
                        } /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                        else
                        {
                            prn_inquiry = this.InquirePRN(prn_number);

                            if (prn_inquiry == null || prn_inquiry == string.Empty)
                            {
                                return false; /* NOT VALID RESPONSE. NOT ABLE TO GET RESPONSE FROM SERVER */
                            }

                            if (prn_inquiry != null && prn_inquiry != string.Empty)
                            {
                                prn_inquiry_response = prn_inquiry.Split('~');
                            }

                            if (prn_inquiry_response == null || prn_inquiry_response.Length <= 1 || prn_inquiry_response[1] != "0")
                            {
                                return false;
                            }

                            if (prn_inquiry_response[1] == "0")
                            {
                                if (target_amount != null && target_amount != string.Empty)
                                {
                                    irsw_sss_amt = Convert.ToDecimal(target_amount);
                                }

                                if (prn_inquiry_response[5] != null && prn_inquiry_response[5] != string.Empty)
                                {
                                    webs_sss_amt = Convert.ToDecimal(prn_inquiry_response[5]);
                                }

                                if (prn_number.Trim().ToUpper() == prn_inquiry_response[2].Trim().ToUpper() && sss_number.Trim() == prn_inquiry_response[3].Trim())
                                {
                                    if (irsw_sss_amt == webs_sss_amt)
                                    {
                                        System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = null; return true; /* VALID 1. SSS NUMBER 2. PRN NUMBER 3. CONTRIBUTION AMOUNT */
                                    }
                                }

                                string msg_prn = string.Empty;

                                msg_prn = "\n";
                                msg_prn += "\nIRSW - PRN Number: " + prn_number + "| SSS MSG - PRN Number: " + prn_inquiry_response[2].Trim().ToUpper();
                                msg_prn += "\nIRSW - SSS Number: " + sss_number + "| SSS MSG - SSS Number: " + prn_inquiry_response[3];
                                msg_prn += "\nIRSW - Contribution : " + irsw_sss_amt.ToString("#,###.00") + "| SSS MSG - Contribution: " + webs_sss_amt.ToString("#,###.00");
                                System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = "\nCannot proceed with the transaction. Please click validate button under merchant information to get details. " + msg_prn;

                                new csAuditLog().writeAudit("", "SSS PRN", "Cannot proceed transaction. PRN details incomplete: " + msg_prn, "", "", csAuditLog.Category.Insert);
                                return false; /* IRSWORLD SSS PRN DETAILS AND WEB SERVICE DETAILS ARE NOT EQUAL */
                            }
                        }
                        /* NOW LETS RE-VALIDATE PRN NUMBER IF USER DID NOT CLICK VALIDATE BUTTON IN THE MERCHANT DETAILS FIELD FOR SSS PRN TRANSACTION AND PROCEED SEND PAYMENT */
                    }
                }

                return true; /* ALWAYS TRUE IF TRANSACTION IS NON SSS PRN BILLS PAYMENT. */
            }
            catch
            {
                return true;
            }
            finally
            {
                clientdata = null;
            }
        }

        /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void get_rpt_lookupsetup()
        {
            DataTable dtRTPLookup = new DataTable();
            BillsPaymentApi rtpapi = new BillsPaymentApi();

            if (System.Web.HttpContext.Current.Session["RTPConfiguration"] == null)
            {
                rtpapi.GetRtpSetup();
                dtRTPLookup = (DataTable)System.Web.HttpContext.Current.Session["RTPConfiguration"];
            }
            else
            {
                dtRTPLookup = (DataTable)System.Web.HttpContext.Current.Session["RTPConfiguration"];
            }
        }
        /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_obp_transaction_response()
        {
            string msg = string.Empty;

            if (System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] != null)
            {
                msg = (string)System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"];
            }

            return "\n\n" + msg;
        }
        /* ADDED BY BOYET FEB 10 2019 */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCharges(string brCode, short remitcurr, short accode, double targetamount)
        {
            return irsOBPSTransactions.getCharges(brCode, remitcurr, accode, targetamount);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranches()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                return null;
            }
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            string location = "";

            location = clientData.Location;
            return irsOBPSTransactions.GetBranches(location).Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Validation(string merchcode, string prodcode, int noofcontrols, string strpara,string param)
        {
            return irsOBPSTransactions.Validation(merchcode, prodcode, noofcontrols, strpara, param);
        }

        /* BOYET FEB 10 2019 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool save_merch_details(string refno, string merchantcode, int prodcode, string CIFID, string data_value, string merseqid)
        {
            string obps_commit_transaction_table = string.Empty;

            try
            {
                //=======================================================================================
                //RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
                if (Session["IRS_OVERRIDE_LOGDATA"] != null || (string)Session["IRS_OVERRIDE_LOGDATA"] != string.Empty)
                {
                    string hitlist_cnt = (string)Session["IRS_OVERRIDE_LOGDATA"];
                    string[] hitlist_arr = null;

                    if (hitlist_cnt != null && hitlist_cnt != string.Empty && hitlist_cnt.Length > 0)
                    {
                        hitlist_arr = hitlist_cnt.Split('~');
                    }

                    if (hitlist_arr != null && hitlist_arr.Length > 0)
                    {
                        for (int i = 0; i < hitlist_arr.Length; i++)
                        {
                            if (hitlist_arr[i].IndexOf("25") > 0)
                            {
                                /* ADDED BY BOYET APRIL 28 2016 */
                                Session["OTC_HIT_LIST_VAL"] = hitlist_arr[i] + "~"; break;
                            }
                            if (hitlist_arr[i].IndexOf("50") > 0)
                            {
                                /* ADDED BY BOYET APRIL 28 2016 */
                                Session["OTC_HIT_LIST_VAL"] = hitlist_arr[i] + "~"; break;
                            }
                        }
                    }
                }
                //=======================================================================================

                if (System.Web.HttpContext.Current.Session["OBPS_COMMIT_TRANSACTION_TABLE"] != null)
                {
                    obps_commit_transaction_table = (string)System.Web.HttpContext.Current.Session["OBPS_COMMIT_TRANSACTION_TABLE"];
                }

                if (obps_commit_transaction_table == null || obps_commit_transaction_table == string.Empty)
                {
                    if (!irsOBPSTransactions.SaveUpdates())
                    {
                        throw new Exception("Save changes: " + irsOBPSTransactions.Message);
                    }

                    return irsOBPSTransactions.save_merch_details(refno, merchantcode, prodcode, CIFID, data_value, merseqid);
                }

                return false;
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = e.Message; return false;
            }
        }
        /* BOYET FEB 10 2019 */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLookUpValue(string fldgroup, string fldvalue)
        {
            return irsOBPSTransactions.getLookUpValue(fldgroup, fldvalue);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaults(string brcode)
        {
            return irsOBPSTransactions.getDefaults(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getLookUp(string fldgroup)
        {
            return irsOBPSTransactions.getLookUp(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_SubLookup(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs();
            return commons.execStoredProcedure("isp_getSubLookup", new object[] { ((irsClientData)Session["IRS_CLIENT_DATA"]).Location, fldgroup });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getActualPayment()
        {
            if (Session["OBPSCalculator"] != null)
            {
                irsCalculator irsCalcu = (irsCalculator)Session["OBPSCalculator"];
                return Math.Round(irsCalcu.get_obps_actual_payment(), 2).ToString("#,###.00");
            }

            return "0.00";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCalculator(int productchargesid, int acctcurr, bool chargelocally, int compute_method, string target_amt)
        {
            irsCalculator calcu = new irsCalculator(productchargesid, acctcurr, chargelocally, 3);
            Decimal targetamt = Convert.ToDecimal(target_amt);

            try
            {
                calcu.compute_obps_charges(compute_method, targetamt);
                Session["OBPSCalculator"] = calcu;

                if (Session["OBPSCalculator"] != null)
                {
                    return calcu.get_transaction_charge_screen();
                }

                return "<center><b>REMITTANCE CALCULATOR IS NOT PROPERLY INITIALIZED. MAKE SURE THAT THE AMOUNT ENTERED AND CHARGE TYPE ARE VALID.</b></center>";
            }
            catch
            {
                return "<center><b>REMITTANCE CALCULATOR IS NOT PROPERLY INITIALIZED. MAKE SURE THAT THE AMOUNT ENTERED AND CHARGE TYPE ARE VALID.</b></center>";
            }
            finally
            {
                calcu = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_calculator_for_override_values(string irs_calculator_session_name)
        {
            if (Session[irs_calculator_session_name] != null)
            {
                return ((irsCalculator)Session[irs_calculator_session_name]).get_dataset_record_for_override();
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCalculatorH(int seq_id)
        {
            irsCalculator calcu = new irsCalculator(seq_id);
            Session["OBPSCalculator"] = calcu;
            calcu.reload_computation_entries(seq_id, true);
            return calcu.get_transaction_charge_screen();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getMerchantList(string param)
        {
            return irsOBPSTransactions.getMerchantList(param);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_calculator_computed_values()
        {
            return irsOBPSTransactions.get_calculator_computed_values();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_tbl_lookup_description(string fldgroup, string fldvalue)
        {
            return irsOBPSTransactions.get_tbl_lookup_description(fldgroup, fldvalue);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getProductChargesID(int currency, string brcode)
        {
            return irsOBPSTransactions.getProductChargesID(currency, brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isTeller()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                return false;
            }

            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];

            if (client_data.Role == 6)
            {
                return true;
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getCurrencyCategories(string branchcode)
        {
            DataTable dt = new DataTable("CurrencyCategories");
            irsCurrencyGroup ccygroup = new irsCurrencyGroup();

            dt = ccygroup.GetCurrencyCategory(branchcode, "1");
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getFXRate(string branchcode, string sourceccy, string targetccy)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                return string.Empty;
            }
            
            int strRole = Convert.ToInt32(getUserRole());

            if (strRole != 10 && strRole != 25 && strRole != 7 && strRole != 27)
            {
                DataTable dt = new DataTable("FXRate");
                irsCurrencyRates ccyRates = new irsCurrencyRates();

                dt = ccyRates.getCurrencyRates(getlocaltime().Date, branchcode);
                return dt.Select("SourceCurrency='" + sourceccy + "' and TargetCurrency = '" + targetccy + "'", "DateInserted DESC")[0][5].ToString();
            }
            else
            {
                irsInternetPage ip = new irsInternetPage();
                return ip.getExchangeRate(branchcode, sourceccy, targetccy, true);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool bRatesComplete(string BranchCode)
        {
            irsCurrencyRates ccyRates = new irsCurrencyRates();

            return ccyRates.isComplete(BranchCode.Trim(), getlocaltime().Date);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string waiveCharges(int productchargesid, int acctcurr, bool chargelocally, int compute_method, decimal target_amt)
        {
            if (Session["OBPSCalculator"] != null)
            {
                irsCalculator calcu = (irsCalculator)Session["OBPSCalculator"];
                Decimal targetamt = Convert.ToDecimal(target_amt);
                calcu.compute_waived_charges(targetamt, true);
                return calcu.get_transaction_charge_screen();
            }

            return "<center><b>REMITTANCE CALCULATOR IS NOT PROPERLY INITIALIZED. MAKE SURE THAT THE AMOUNT ENTERED AND CHARGE TYPE ARE VALID.</b></center>";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void overrideCharges(string oCharges)
        {
            irsCalculator calcu = (irsCalculator)Session["OBPSCalculator"];
            calcu.set_dataset_record_for_override(oCharges);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string generateOBPRefno(string MerchantCode, string ProductCode)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                return string.Empty;
            }
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return irsOBPSTransactions.OBPSRefno(MerchantCode, ProductCode, client_data.Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void printReceipt(string cifid)
        {
            try
            {
                cifid = "";
                Session["IRS_ReportsUI"] = "OBPS:" + irsOBPSTransactions.TranxID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String sendReceipt(string seq_id)
        {
            ReportDocument receiptTranx = new ReportDocument();
            string repname = @"\OTC\rptReceipt.rpt";
            string rootpath = new irsConfiguration().appsetting("ReportPath");
            string reportpath = rootpath + repname;
            irsReceipts irsRpt = new irsReceipts();
            string msgReturn = "";
            CrystalReportUtil cru = new CrystalReportUtil();

            try
            {
                receiptTranx.Load(reportpath);

                receiptTranx = cru.generate_obp_report(((irsClientData)Session["IRS_CLIENT_DATA"]).Location, irsOBPSTransactions.TranxID);
                cru.OREmailSending(receiptTranx, GetRefNo(), seq_id, out msgReturn);
            }
            catch (Exception ex)
            {
                msgReturn = ex.Message;
            }

            return msgReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet OBPRemit(string refno, string targetaccount, double remittingamount, int transtatus, int productcode, int implemtype, int paymode, int remittingcurrency, int accountcurrency)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                return null;
            }

            irsRemTeller rem = new irsRemTeller();
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            DataSet paramMsg = new DataSet();

            bool isAgent = false;
            string isbayadcenter = (string)Session["OBPSMerchantType"];
            string merchantcode = "";

            DataSet ds = new DataSet();
            DataTable dtStatus = new DataTable();
            DataRow dr;

            int strRole = Convert.ToInt32(getUserRole());

            if (strRole == 10 || strRole == 25 || strRole == 7 || strRole == 27)
            {
                isAgent = true;
            }

            Session["TranxID"] = irsOBPSTransactions.TranxID;
            refno = irsOBPSTransactions.getTranxHistory(Convert.ToInt32(irsOBPSTransactions.TranxID)).Rows[0]["ReferenceNo"].ToString();

            try
            {
                merchantcode = irsOBPSTransactions.getTranxHistory(Convert.ToInt32(irsOBPSTransactions.TranxID)).Rows[0]["MerchantName"].ToString().Split(',')[1].ToString();

                if (isbayadcenter.ToUpper() == "BAYADCENTER")
                {
                    if (irsOBPSTransactions.sendBayadcenter(merchantcode.Trim()) == true)
                    {
                        if (transtatus == 1)
                        {
                            paramMsg = rem.RemitToAgent(Convert.ToInt32(irsOBPSTransactions.TranxID));
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
                            paramMsg = rem.RemitToBayadCenter(Convert.ToInt32(irsOBPSTransactions.TranxID));
                        }
                    }
                    else
                    {
                        paramMsg = rem.RemitToBayadCenter(Convert.ToInt32(irsOBPSTransactions.TranxID));
                    }
                }
                else
                {
                    int transtatus_value = Convert.ToInt32(new sqlHelperWs().execScalar("select isnull((select fldvalue from tbl_lookup where fldgroup = 'tranxstatus' and fldcode = 'provr'),0)"));
                    int transtatus_obp = Convert.ToInt32(new sqlHelperWs().execScalar("select transtatus from tbl_remittance where seq_id=" + Convert.ToInt32(irsOBPSTransactions.TranxID) + ""));

                    if (transtatus_value != transtatus_obp)
                    {
                        if (isAgent == true)
                        {
                            paramMsg = rem.Remit2(Convert.ToInt32(irsOBPSTransactions.TranxID)); //rem.RemitToAgent(Convert.ToInt32(irsOBPSTransactions.TranxID));
                        }
                        else
                        {
                            paramMsg = rem.Remit2(Convert.ToInt32(irsOBPSTransactions.TranxID));
                        }
                    }
                    else
                    {
                        return new irsRemTeller().ParseMessage(transtatus_value, "Transaction is successful");
                    }
                }
            }
            catch (Exception ex)
            {
                paramMsg = rem.ParseMessage(paramMsg, ex.Message); return paramMsg;
            }
            return paramMsg;
        }

        /* E2EAPICHANGE 20220209 Removed isDebitSuccessfull */
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool ValidateMemberID(string sID, string MerchantCode, string PAGPayType)
        {
            return irsOBPSTransactions.ValidID(sID, MerchantCode, PAGPayType);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime getlocaltime()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
            }

            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];            
            DateTime dtLocalTime = new irsInternetPage().getTransactionDate(client_data.Location, true);

            return dtLocalTime;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string compute_override(int product_charge_id, int account_currency, decimal remit_amount)
        {
            irsCalculator calc = null;

            if (Session["OBPSCalculator"] != null)
            {
                calc = (irsCalculator)Session["OBPSCalculator"];
                calc.recompute_obps_charges(remit_amount);
            }

            Session["OBPSCalculator"] = calc;

            return calc.get_transaction_charge_screen();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string RemittanceLimit(string brcode, decimal amt)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }

            if (Session["OBPSCalculator"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "OBPSCalculator session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }

            irsCalculator calc = (irsCalculator)Session["OBPSCalculator"];
            irsBranchSetup bs = new irsBranchSetup();
            DataTable dt = bs.getbranchsetup(brcode);
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];

            amt = calc.get_remittance_gross_branch();

            if (Convert.ToDecimal(dt.Rows[0]["TellerLimit"]) < amt & client_data.Role == 3)
            {
                return "REMITTANCE AMOUNT EXCEEDS TELLER'S LIMIT";
            }
            else if (Convert.ToDecimal(dt.Rows[0]["SupervisorLimit"]) < amt & client_data.Role == 4)
            {
                return "REMITTANCE AMOUNT EXCEEDS SUPERVISOR'S LIMIT";
            }
            else if (Convert.ToDecimal(dt.Rows[0]["RemittanceAmtLimit"]) < amt & Convert.ToBoolean(dt.Rows[0]["AggregateAmtLimit"]) == true)
            {
                return "TOTAL REMITTANCE AMOUNT EXCEEDS LIMIT";
            }
            else if (Convert.ToDecimal(dt.Rows[0]["RemittanceAmtLimit"]) < amt)
            {
                return "REMITTANCE AMOUNT EXCEEDS LIMIT";
            }
            else
            {
                return "";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string GetRefNo()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return null;
            }

            return irsOBPSTransactions.getTranxRefNo(Convert.ToInt32(irsOBPSTransactions.TranxID));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public int obp_user(int product_charges_id)
        {
            int i = 0; irsClientData loggin_data = null;

            if (Session["IRS_CLIENT_DATA"] != null)
            {
                loggin_data = (irsClientData)Session["IRS_CLIENT_DATA"];

                string s = "SELECT COUNT(*) access_count FROM TBL_CHARGES_USERS WHERE UserType = 1 AND ProductChargesID = " + product_charges_id.ToString() + " AND Role = " + loggin_data.Role.ToString();
                
                DataTable dt = sqlkonek.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["access_count"]) > 0)
                    {
                        i = 1;
                    }
                }
            }
            else
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return 0;
            }

            return i;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void upDateMemberID(int SEQ_ID, string CIFID, string RemCIFID, string MemberID)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return;
            }
            if (Session["OBPSCalculator"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "OBPSCalculator session is empty.", "", "", csAuditLog.Category.Select);
                return;
            }

            irsOBPSTransactions.UpdateMemberID(SEQ_ID, CIFID, RemCIFID, MemberID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string GrossAmount(int fComma)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "0.00";
            }

            if (Session["OBPSCalculator"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "OBPSCalculator session is empty.", "", "", csAuditLog.Category.Select);
                return "0.00";
            }

            irsCalculator calc = (irsCalculator)Session["OBPSCalculator"];

            if (fComma == 1)
            {
                return calc.get_remittance_gross_branch().ToString("#,###.00");
            }
            else
            {
                return calc.get_remittance_gross_branch().ToString("####.00");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string NetToBeneAmount()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "0.00";
            }
            if (Session["OBPSCalculator"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "OBPSCalculator session is empty.", "", "", csAuditLog.Category.Select);
                return "0.00";
            }
            irsCalculator calc = (irsCalculator)Session["OBPSCalculator"];
            return calc.get_remittance_net_to_bene_amt().ToString("#,###.00");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string TotalChargesAmount()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "0.00";
            }
            if (Session["OBPSCalculator"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "OBPSCalculator session is empty.", "", "", csAuditLog.Category.Select);
                return "0.00";
            }

            irsCalculator calc = (irsCalculator)Session["OBPSCalculator"];
            return calc.get_remittance_total_charges_amt().ToString("#,###.00");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public void TotalExciseTax()
        {
            /* OBBA EXCISE TAX */
            string OBBAComputedTax = string.Empty;
            string OBBAPercentage = string.Empty;

            if (System.Web.HttpContext.Current.Session["OBBA_COMPUTEDTAX"] != null)
            {
                OBBAComputedTax = System.Web.HttpContext.Current.Session["OBBA_COMPUTEDTAX"].ToString();

            }
            if (System.Web.HttpContext.Current.Session["OBBA_PERCENTAGE"] != null)
            {
                OBBAPercentage = (string)System.Web.HttpContext.Current.Session["OBBA_PERCENTAGE"].ToString();
            }
            /* OBBA EXCISE TAX */
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getTranxSeq_ID()
        {
            if (irsOBPSTransactions.TranxID == "" || irsOBPSTransactions.TranxID == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "TranxID session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            else
            {
                return irsOBPSTransactions.TranxID;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getBranchSetup(string brcode)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return null;
            }

            DataTable dt = null;
            irsBranchSetup bs = new irsBranchSetup();

            try
            {
                dt = bs.getbranchsetup(brcode);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getUserRole()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }

            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
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
                    sReturn = Convert.ToString(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "getUserRole", ex.Message, "", csAuditLog.Category.Select);
            }
            finally
            {
                ds.Dispose();
                ds = null;
            }

            return sReturn;            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getUserLoc()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }

            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return client_data.Location.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getCIFID()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }

            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return client_data.CIFID.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ConvertToMoney(string amount)
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "0.00";
            }

            return Convert.ToDecimal(amount).ToString("#,###.00");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getDisclaimer()
        {
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            string regionClient = new irsInternetPage().getRegion(client_data.Location, true);
            return new irsInternetPage().getDisclaimer(regionClient);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool pnb_account_ok(string account_no)
        {
            return new irsAccountAlgo().checkAccount(account_no);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getAccountStatus(string AcctNo)
        {
            irsAccountInquiry ai = new irsAccountInquiry();
            string strreturn = ai.getAccountStatus(AcctNo.Trim());
            return strreturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool chkDefaults_charges(string brCode, string sourceCCy, string chargeType, string Paymode)
        {
            try
            {
                return irsOBPSTransactions.chkDefaults_charges(brCode, sourceCCy, chargeType, Paymode);
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBillsPayment(string refno)
        {
            try
            {
                return irsOBPSTransactions.getRemittance(refno);
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
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
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool chkCutoffDate(string cutoff, string localDate)
        {
            DateTime cutoffDate;
            DateTime localdate;
            try
            {
                cutoffDate = Convert.ToDateTime(cutoff);
                localdate = Convert.ToDateTime(localDate);

                if (cutoffDate.Hour < localdate.Hour)
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
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool tranxstatusOK(string refno)
        {
            try
            {
                return irsOBPSTransactions.tranxstatusOK(refno);
            }
            catch
            {
                return false;
            }
        }

        /* E2EAPICHANGE 20220209 Removed submitOAAM, updateOAAM */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool chkChargeType(string chargetype, string sourceCurrency, string paymode)
        {
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            DataTable dtb = null;

            bool isAgent = false;
            bool blnChargeType = false;
            int strRole = Convert.ToInt32(getUserRole());

            if (strRole == 10 || strRole == 25 || strRole == 7 || strRole == 27)
            {
                isAgent = true;
            }

            try
            {                
                dtb = sqlkonek.execStoredProcedureTable("sp_GetOBPLookup", new object[] { "ChargeType", client_data.Location, sourceCurrency, chargetype, paymode, isAgent, strRole });

                for (int i = 0; i < dtb.Rows.Count; i++)
                {
                    if (dtb.Rows[i][0].ToString() == chargetype.Trim())
                    {
                        blnChargeType = true;
                        break;
                    }
                    else
                    {
                        blnChargeType = false;
                    }
                }
            }
            catch (Exception ex)
            {
                /* BOYET BEAM AND GO 06-10-2019 */
                if (ex.InnerException != null)
                {
                    new csAuditLog().writeAudit(client_data.UserId, "OBPS", "chkChargeType", ex.Message, ex.InnerException.ToString(), csAuditLog.Category.Select);
                }
                else
                {
                    new csAuditLog().writeAudit(client_data.UserId, "OBPS", "chkChargeType", ex.Message, ex.Message, csAuditLog.Category.Select);
                }
                /* BOYET BEAM AND GO 06-10-2019 */
            }

            return blnChargeType;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string FoodAmount(string amount, string merchantcode)
        {
            DataSet ds = null;
            string ramount = "";
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            try
            {
                ds = getLookUp("FoodPackageCurrency");
                string ccy = ds.Tables[0].Select("FldDescription = '" + merchantcode + "'")[0]["FldCode"].ToString();
                if (ccy == "1")
                {
                    ramount = Convert.ToDecimal(amount).ToString("#,###.00");
                }
                else
                {
                    string exrate = new irsInternetPage().getExchangeRate(client_data.Location, ccy, "1", true);
                    string[] xrate = exrate.Split(' ');
                    ramount = Convert.ToDecimal(Convert.ToDecimal(amount) * (Convert.ToDecimal(1 / Convert.ToDecimal(xrate[4])))).ToString("#,###.00");
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(client_data.UserId, "OBPS", "FoodAmount", ex.Message, ex.InnerException.ToString(), csAuditLog.Category.Select);
            }
            return ramount;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_is_post_dated(string check_date)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));

                string s = "SELECT COUNT(*) AS holidays_count " +
                           "FROM TBL_HOLIDAYS " +
                           "WHERE " +
                                "ISNULL(DELETED, 0) = 0 AND " +
                                "ISNULL(HOLIDAY, 0) = 1 AND " +
                                "BRANCHCODE = '" + ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location + "' AND " +
                                "CONVERT(VARCHAR,HOL_DATE, 101) = '" + branch_current_date.ToString("MM/dd/yyyy") + "'";
                
                
                DataTable dt = sqlkonek.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (Convert.ToInt32(dt.Rows[0]["holidays_count"]) > 0)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    if (dt != null)
                    {
                        dt = null;
                    }
                }

                if (Convert.ToDateTime(check_date) > branch_current_date)
                {
                    return true;
                }
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTodayForex()
        {
            DataTable dt = null;
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                dt = new irsInternetPage().getTodayForex(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location);
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string updateDisclaimer(string RemitterCIFID, bool IsRead)
        {
            RemitterCIFID = this.getCIFID();
            string sReturn = new irsInternetPage().updateDisclaimer(RemitterCIFID, IsRead);
            string[] arrReturn;
            new csAuditLog().writeAudit("", "OBPS Internet", "UPDATE REMITTER DISCLAIMER: [RemitterCIFID=" + RemitterCIFID + "][IsRead=" + IsRead.ToString() + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Update);
            arrReturn = sReturn.Split('|');
            if (arrReturn[1].ToString().Trim() == "OK")
            {
                sReturn = "Record successfully updated. Disclaimer will be shown on next logon.";
            }
            else
            {
                sReturn = "Error while updating record. Error at " + sReturn + ". Please report to the nearest PNB branch and/or system administrator.";
            }
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemitterDisclaimer(string RemitterCIFID)
        {
            RemitterCIFID = this.getCIFID();
            string sReturn = new irsInternetPage().getRemitterDisclaimer(RemitterCIFID);
            new csAuditLog().writeAudit("", "OBPS Internet", "GET REMITTER DISCLAIMER: [RemitterCIFID=" + RemitterCIFID + "][ReturnValue=" + sReturn + "]", "", "", csAuditLog.Category.Select);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchOperations(string BranchCode)
        {
            return new irsInternetPage().getBranchOperations(new irsInternetPage().getBranchValue(BranchCode, "BranchCode", BranchCode, "SEQ_ID"));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getWebRemittanceChargeType(string BranchCode)
        {
            DataTable dt = new irsInternetPage().getSubLookup(BranchCode, "ChargeType");
            string wr = "";
            try
            {
                wr = dt.Select("FldDescription='Web Remittance'")[0]["FldValue"].ToString();
            }
            catch { }
            return wr;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemitterAccountNo(Int64 seqID)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                string s = "SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + seqID;
                
                
                DataTable dt = sqlkonek.getDataSet(s, false, new object[] { }).Tables[0];//Performance consideration when using irsSqlCommonsWS and sqlHelperWS

                try
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["AccountNo"].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch
                {
                    return "";
                }
                finally
                {
                    if (dt != null)
                    {
                        dt = null;
                    }
                }
            }
            else
            {
                return "";
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isPNBReceipt(string BranchCode)
        {
            return new irsInternetPage().isPNBReceipt(BranchCode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool hasTranxDetailsHistory(string merchantcode, string merprodcode, string RemCIFID)
        {
            DataTable dt = irsOBPSTransactions.TranxDetailsHistory(merchantcode, merprodcode, RemCIFID);
            if (dt != null && dt.Rows.Count > 0)
            {
                Session["OBPSTranxDetailsHistory"] = dt;
                return true;
            }
            else
            {
                Session["OBPSTranxDetailsHistory"] = null;
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable TranxDetailsHistory(string merchantcode, string merprodcode, string RemCIFID)
        {
            if (Session["OBPSTranxDetailsHistory"] != null)
            {
                return (DataTable)Session["OBPSTranxDetailsHistory"];
            }
            else
            {
                DataTable dt = irsOBPSTransactions.TranxDetailsHistory(merchantcode, merprodcode, RemCIFID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Session["OBPSTranxDetailsHistory"] = dt;
                    return dt;
                }
                else
                {
                    Session["OBPSTranxDetailsHistory"] = null;
                    return null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable RemitterCashAgg(string remitterid, string remittingcurrency, string amount)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));
                return new irsInternetPage().CashAggregation(amount, remitterid, remittingcurrency, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, branch_current_date.ToString());
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkInternetRemittanceAggregateLimit(string remitterid, string remittingcurrency, string amount)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                DateTime branch_current_date = new irsInternetPage().getTransactionDate(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, true);//Convert.ToDateTime(new irsTimeZone().branchLocalTime(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location));
                return new irsInternetPage().aggregateInternet(amount, remitterid, remittingcurrency, ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, branch_current_date.ToString());
            }
            else
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSystemControl_RemitterInfo(string remitterID, string beneficiaryID)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                return new irsInternetPage().getSystemControl_RemitterInfo(remitterID, beneficiaryID);
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSystemControl_RemitterIDs(string remitterID, string seq_ID)
        {
            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                return new irsInternetPage().getSystemControl_RemitterIDs(remitterID, seq_ID);
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkSystemControl_RemitterInfo(string remitterID, string beneficiaryID)
        {
            DataTable remInfo = this.getSystemControl_RemitterInfo(remitterID, beneficiaryID);
            int isincomplete = 0;
            if (remInfo != null || remInfo.Rows.Count > 0)
            {
                if (remInfo.Rows[0]["RemitterBirthDate"].ToString() == "" || remInfo.Rows[0]["RemitterBirthDate"].ToString() == "01/01/1900")
                {
                    isincomplete += 1;
                }

                if (remInfo.Rows[0]["Occupation"].ToString() == "" || remInfo.Rows[0]["Occupation"].ToString() == "0")
                {
                    isincomplete += 1;
                }

                if (remInfo.Rows[0]["SourceofFunds"].ToString() == "" || remInfo.Rows[0]["SourceofFunds"].ToString() == "0")
                {
                    isincomplete += 1;
                }
            }

            if (isincomplete > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string checkSystemControl_RemitterIDs(string remitterID)//0=No ID, 1= ID OK, 2=ExpiredID
        {
            DataTable remID = null;
            string sReturn = string.Empty;
            DateTime branch_current_date = new irsInternetPage().getTransactionDate(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location, true);
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            
            String[,] rids =
                {                    
                    {"15", "SSS ID/SSN"},
                    {"27", "US-based ID"},
                    {"32", "US Govt/State ID"},
                    {"33", "US Issued Drivers License"},
                    {"34", "US Passport"}
                };

            try
            {
                using (DataSet ds = commons.getDataSet("usp_obps_getRequiredIDs", true, new Object[] { remitterID }))
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        remID = ds.Tables[0];
                        if (remID.Rows.Count > 0)
                        {
                            for (int i = 0; i < rids.GetUpperBound(0); i++)
                            {
                                DataRow[] mrow = remID.Select("IDType = " + rids[i, 0]);
                                if (mrow.Length < 1)
                                {
                                    sReturn += "0_" + rids[i, 1] + "~";
                                }
                            }

                            DataRow[] erows = remID.Select("ExpiryDate <= '" + branch_current_date.ToString("MM/dd/yyyy HH:mm:ss") + "'");
                            if (erows.Length > 0)
                            {
                                foreach (DataRow erow in erows)
                                {
                                    String edate = "";
                                    String iddesc = "";
                                    String idno = "";
                                    try { edate = Convert.ToDateTime(erow["ExpiryDate"]).ToString("MM/dd/yyyy HH:mm:ss"); }
                                    catch { }
                                    try { iddesc = Convert.ToString(erow["IDDescription"]); }
                                    catch { }
                                    try { idno = Convert.ToString(erow["IDNo"]); }
                                    catch { }
                                    sReturn += "2_" + iddesc + "|IDNo: " + idno + "|Expire Date: " + edate + "~";
                                }
                            }
                        }
                    }
                    else
                    {
                        sReturn = "Error: No Dataset. " + commons.Message;
                        new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "checkSystemControl_RemitterIDs", sReturn, remitterID, csAuditLog.Category.Select);
                    }
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "checkSystemControl_RemitterIDs", ex.Message, remitterID + " " + ex.InnerException.ToString(), csAuditLog.Category.Select);
                sReturn = "Error: " + (ex.Message);
            }
            finally
            {
                if (remID != null)
                {
                    remID.Dispose();
                    remID = null;
                }
                if (commons != null)
                {
                    commons = null;
                }
            }

            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int saveSystemControl_RemitterInfo(string remInfoData)
        {
            int i = 0;
            string[] arrRemitterInfo;
            string[] arrRemitterInfoInsert = null;
            sqlHelperWs commons = new sqlHelperWs();
            int iReturn = 0;

            try
            {
                arrRemitterInfo = remInfoData.Split('~');

                if (arrRemitterInfo.Length > 0)
                {
                    arrRemitterInfoInsert = new string[arrRemitterInfo.Length];

                    for (i = 0; i <= (arrRemitterInfo.Length - 1); i++)
                    {
                        if (arrRemitterInfo.GetValue(i).ToString() == "NULL")
                        {
                            arrRemitterInfoInsert.SetValue("", i);
                        }
                        else
                        {
                            arrRemitterInfoInsert.SetValue(arrRemitterInfo.GetValue(i), i);
                        }
                    }                    
                    
                    iReturn = commons.execStoredProcedureUpdate("ksp_UpdateRemitterInfo", arrRemitterInfoInsert);
                }
                else
                {
                    iReturn = -1;
                }

                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "saveSystemControl_RemitterInfo", remInfoData, iReturn.ToString(), csAuditLog.Category.Update);
                return iReturn;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "saveSystemControl_RemitterInfo", remInfoData, iReturn.ToString() + " " + ex.Message, csAuditLog.Category.Update);
                return -1;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getPhilhealthAmount()
        {
            sqlHelperWs sql = new sqlHelperWs();
            string amount = Convert.ToString(sql.execScalar("select flddescription from tbl_lookup where fldgroup='Philhealth_Contri'"));

            if (amount == "" || amount == null)
            {
                return "0";
            }

            return amount;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getPALCurrency(string fldvalue)
        {
            sqlHelperWs sql = new sqlHelperWs();
            string currcode = Convert.ToString(sql.execScalar("select fldcode from tbl_lookup where fldgroup='PAL Currency' and fldvalue='" + fldvalue + "'"));

            if (currcode == "" || currcode == null)
            {
                return string.Empty;
            }

            return currcode;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_paymentdate(string refdate)
        {
            if (refdate != "")
            {
                DateTime ref_date = Convert.ToDateTime(refdate.Substring(0, 2) + "/" + refdate.Substring(2, 2) + "/" + refdate.Substring(4, 2));
                string loc = Convert.ToString(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location);
                DateTime localtime = new irsTimeZone().branchLocalTime(loc);

                if (ref_date.ToShortDateString() == localtime.ToShortDateString())
                {
                    return "true";
                }
            }

            return "false";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int saveSystemControl_RemitterIDs(string remIDsData)
        {
            int i = 0;
            string[] arrRemitterIDs;
            string[] arrRemitterIDsInsert = null;
            sqlHelperWs commons = new sqlHelperWs();
            int iReturn = 0;

            try
            {
                arrRemitterIDs = remIDsData.Split('~');

                if (arrRemitterIDs.Length > 0)
                {
                    arrRemitterIDsInsert = new string[arrRemitterIDs.Length];

                    for (i = 0; i <= (arrRemitterIDs.Length - 1); i++)
                    {
                        if (arrRemitterIDs.GetValue(i).ToString() == "NULL")
                        {
                            arrRemitterIDsInsert.SetValue("", i);
                        }
                        else
                        {
                            arrRemitterIDsInsert.SetValue(arrRemitterIDs.GetValue(i), i);
                        }
                    }

                    iReturn = commons.execStoredProcedureUpdate("ksp_SaveRemitterIDs", arrRemitterIDsInsert);
                }
                else
                {
                    iReturn = -1;
                }

                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "saveSystemControl_RemitterIDs", remIDsData, iReturn.ToString(), csAuditLog.Category.Update);
                return iReturn;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "saveSystemControl_RemitterIDs", remIDsData, iReturn.ToString() + " " + ex.Message, csAuditLog.Category.Update);
                return -1;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void end_sessions()
        {
            Session["OBPSTransaction"] = null;
            Session["OBPSCalculator"] = null;
            Session["IRS_ReportsUI"] = null;
            Session["TranxID"] = null;
            /* E2EAPICHANGE 20220209 Removed OBPS_OAAM_Parameters */
            Session["OBPSTranxDetailsHistory"] = null;
            Session["OBPSExcess"] = null;
            Session["RemittanceOBPS"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void setMerchantType(string Merchanttype)
        {
            Session["OBPSMerchantType"] = Merchanttype;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validate_dragonpay(string memid, bool isvalid)
        {
            return new irsOBPSTransactions().validate_dragonpay(memid, isvalid);
        }

        /* PAOLO PAL BOOK AND HOLD */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_checkdigit_pal(string amount, string pal_referenceno, string currency)
        {
            return new irsOBPSTransactions().validate_checkdigit_pal(amount, pal_referenceno, currency);
        }
        /* PAOLO PAL BOOK AND HOLD */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validate_paymentdate(string paymentdate)
        {
            DateTime dt = Convert.ToDateTime(paymentdate.Substring(0, 2) + "/" + paymentdate.Substring(2, 2) + "/" + paymentdate.Substring(4, 2));
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            DateTime dtLocalTime = new irsInternetPage().getTransactionDate(client_data.Location, true);

            if (Convert.ToDateTime(dt.ToString("MM/dd/yyyy")).CompareTo(Convert.ToDateTime(dtLocalTime.ToString("MM/dd/yyyy"))) < 0)
            {
                return false;
            }

            return true;
        } 

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getActualPaymentPAL(string sourceccy, string targetccy, string amt)
        {
            string branchcode = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
            string rate = this.getFXRate(branchcode, sourceccy, targetccy);

            decimal actual_payment = Convert.ToDecimal(amt) * Convert.ToDecimal(rate);
            if (sourceccy == "1") //if peso is payment
            {
                actual_payment = Convert.ToDecimal(amt);
            }

            return Convert.ToString(Math.Round(actual_payment, 2));
        }

        //*** Added code by Randy : TEL and TLN ACH Extract Remitter Account, Routing and Bank Name details 1/26/2017 ***//
        private String get_branchCode()
        {
            if (Session["IRS_CLIENT_DATA"] != null) return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location; return null;
        }

        private void logs(string userid, string module, string description, string newvalue, string oldvalue)
        {
            irsClientData login_cred = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

            if (login_cred != null && login_cred.UserId != string.Empty && userid == string.Empty)
            {
                userid = login_cred.UserId;
            }

            this.logger.writeAudit(userid, module, description, newvalue, oldvalue, csAuditLog.Category.Insert);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Check_ACH_Extract_Setup()
        {
            DataTable dt_return = null;
            string BranchCode = "";

            try
            {
                BranchCode = get_branchCode();

                sqlkonek.EnforceConstraint = false;
                dt_return = sqlkonek.execStoredProcedureTable("proc_Check_ACH_Extract_Setup", new string[] { BranchCode });

                if (dt_return.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                this.logs("", "Remittance.BillsPayment.Check_ACH_Extract_Setup", e.Message.ToString().Trim(), "", BranchCode);
            }
            finally
            {
                dt_return = null;
                BranchCode = "";
            }

            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Check_IsRemitter_ACH(int paymode)
        {
            DataTable dt_return = null;
            try
            {
                sqlkonek.EnforceConstraint = false;
                dt_return = sqlkonek.execStoredProcedureTable("proc_Check_IsRemitter_ACH_Extract", new object[] { paymode });
                if (dt_return.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                this.logs("", "Remittance.BillsPayment.Check_IsRemitter_ACH", e.Message.ToString().Trim(), "", "PayMode : " + paymode.ToString());
            }
            finally
            {
                dt_return = null;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void Save_RemAccount_Details(string Refno, int RemAccountType, string RemAccountNo, string RemRoutingNo, string RemBankName)
        {
            int i_return = 0;

            try
            {
                sqlkonek.EnforceConstraint = false;
                i_return = sqlkonek.execStoredProcedureUpdate("proc_Save_Internet_ACHWorks", new object[] { Refno, RemAccountType, RemAccountNo, RemRoutingNo, RemBankName });

                if (i_return < 0)
                {
                    this.logs("", "Remittance.BillsPayment.Save_RemAccount_Details", "Problem in excuting save process for reference no : " + Refno + "; AccountNo : " + RemAccountNo + "; Routing No : " + RemRoutingNo + "; Bank Name : " + RemBankName, "", "");
                }
            }
            catch (Exception e)
            {
                this.logs("", "Remittance.BillsPayment.Save_RemAccount_Details", e.Message.ToString().Trim(), i_return.ToString(), Refno);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool PostTransactionBeam(string transactioncode, string remarks, string currencycode, string amount)
        {
            return false; /* BOYET BEAM AND GO 06-10-2019 */            
            //return new irsOBPSTransactions().PostTransactionBeam(transactioncode, remarks, currencycode, amount);
        }

        /* BOYET BEAM AND GO 06-10-2019 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_beam_and_go_reference_number(string beam_reference_no, string transaction_amt)
        {
            return new irsOBPSTransactions().validate_beam_and_go_reference(beam_reference_no, transaction_amt);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string post_beam_and_go_reference_number(string transaction_code, string remarks, string currency_code, string amount, string fx_rate)
        {
            return new irsOBPSTransactions().post_beam_and_go_reference(transaction_code, remarks, currency_code, amount, fx_rate);
        }
        /* BOYET BEAM AND GO 06-10-2019 */
		
		/* RFC-5305 LOAN PRN CHECKING */
        public bool is_sss_loan_prn_prefix(string loan_prn)
        {
            string other_loan_prefix = string.Empty;
            string loan_prn_prefix = string.Empty;
            try
            {
                string[] loan_pref_array = null;
                string RTPFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
                if (RTPFlag == "1")
                {
                    loan_prn_prefix = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "SSS_LOAN_PRN_PREF", "FldDescription");
                }
                else
                {
                    loan_prn_prefix = new AppSettingsReader().GetValue("SSS_LOAN_PRN_PREF", Type.GetType("System.String")).ToString();
                }

                if (loan_prn_prefix != null && loan_prn_prefix != string.Empty)
                {
                    loan_pref_array = loan_prn_prefix.Split('|');
                }

                if (loan_pref_array != null && loan_pref_array.Length > 0)
                {
                    for (int i = 0; i < loan_pref_array.Length; i++)
                    {
                        if (loan_pref_array[i] != null && loan_pref_array[i] != string.Empty && loan_pref_array[i].Trim().ToUpper() == loan_prn.Substring(0, 1).Trim().ToUpper())
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }

            return false;
        }
        /* RFC-5305 LOAN PRN CHECKING */


        /* BOYET RFC-5305 SSS RTPL API CONNECTION 11-18/2019 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_sss_rtpl_prn_number(string sss_rtpl_prn)
        {
            SSSPRN_API loan_prn = null;
            string inquire_prn_response = string.Empty;
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            string RTPFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
            if (RTPFlag == "1")
            {
                string status = string.Empty;
                BillsPaymentApi rtpApi = new BillsPaymentApi();
                //string status = rtpApi.ValidateEndpoint("", sss_rtpl_prn,"1200.00", "BC_SSS");
                return status;
            }
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            else
            {
                /* RFC-5305 LOAN PRN CHECKING */

                if (this.is_sss_loan_prn_prefix(sss_rtpl_prn) == false)
                {
                    return "PRN is not valid for loan payment. Please select the correct SSS biller.";
                }
                /* RFC-5305 LOAN PRN CHECKING */

                try
                {
                    loan_prn = new SSSPRN_API();
                    loan_prn.PRNNumber = sss_rtpl_prn;
                    inquire_prn_response = loan_prn.submitRequest(SSSPRN_API.SSSPayment.INQUIRE_PRN);

                    return inquire_prn_response;
                }
                catch (Exception e)
                {
                    return e.Message;
                }
                finally
                {
                    loan_prn = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string post_sss_rtpl_prn_number(string sss_rtpl_prn, string reference_number)
        {
            SSSPRN_API loan_prn = null;
            DataTable dt = null;
            string amtDue = string.Empty;
            string instructionid = string.Empty;
            try
            {
                string RTPFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
                if (RTPFlag == "1")
                {
                    dt = sqlkonek.getDataSet("getRTPInstructionID_details", true, new object[] { sss_rtpl_prn, "" }).Tables[0];
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        amtDue = dt.Rows[0]["Amount"].ToString();
                        instructionid = dt.Rows[0]["InstructionID"].ToString();
                    }
                    return this.InquireRTP("", sss_rtpl_prn, amtDue, "BC_SSS", instructionid, reference_number, "POST");
                } /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                else
                {
                    loan_prn = new SSSPRN_API();

                    irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                    loan_prn.PRNNumber = sss_rtpl_prn;
                    loan_prn.TransactionDate = new irsTimeZone().branchLocalTime(clientdata.Location).ToString("MM/dd/yyyy HH:mm:ss");
                    loan_prn.TransactionNumber = reference_number;

                    return loan_prn.submitRequest(SSSPRN_API.SSSPayment.SUBMIT_PRN);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                loan_prn = null;
            }
        }
        /* BOYET RFC-5305 SSS RTPL API CONNECTION 11-18/2019 */

        /* BOYET. PARAMOUNT API INTEGRATION. RFC-1111 01-29-2020 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_paramount_reference_number(string paramount_reference_no)
        {
            csParamountInsurance paramount_api = new csParamountInsurance();

            try
            {
                return paramount_api.chk_paramount_reference_num(paramount_reference_no);
            }
            catch(Exception e)
            {
                return "ERROR : check_paramount_reference_number - " + e.Message;
            }
            finally
            {
                paramount_api = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string post_paramount_payment(string paramount_reference_no, string transaction_amount, string mode_of_payment)
        {
            csParamountInsurance paramount_api = new csParamountInsurance();

            try
            {
                return paramount_api.paramount_post_payment(paramount_reference_no, transaction_amount, mode_of_payment);
            }
            catch (Exception e)
            {
                return "ERROR : check_paramount_reference_number - " + e.Message;
            }
            finally
            {
                paramount_api = null;
            }
        }
        /* BOYET. PARAMOUNT API INTEGRATION. RFC-1111 01-29-2020 */

        /* RFC-4404 MERALCO 05/26/2020 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string chk_meralco_refno(string meralco_reference_no, string transaction_amt)
        {
            DataTable dt = null;

            try
            {
                sqlkonek.EnforceConstraint = false;

                if (transaction_amt != null && transaction_amt != string.Empty)
                {
                    transaction_amt = transaction_amt.Replace(",", "");
                }

                dt = sqlkonek.getDataSet("obps_meralco_chk_digit", true, new object[] { meralco_reference_no, transaction_amt }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }

                return "Error encountered validating MERALCO reference number. Please try again.";
            }
            catch
            {
                return "Error encountered validating MERALCO reference number. Please try again.";
            }
            finally
            {
                if (dt != null)
                {
                    dt.Clear();
                    dt = null;
                }
            }
        }
        /* RFC-4404 MERALCO 05/26/2020 */

        /* RFC-10309 PNB GENERAL INSURER OCTOBER 2020 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string chk_pnb_gen_refnum(string policy_number)
        {
            DataTable dt = null;

            try
            {
                sqlkonek.EnforceConstraint = false;

                dt = sqlkonek.getDataSet("obps_pnbgen_chk_digit", true, new object[] { policy_number }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["payment_refno_remarks"].ToString();
                }

                return "Error encountered validating payment reference number.";
            }
            catch
            {
                return "Error encountered validating payment reference number.";
            }
        }
        /* RFC-10309 PNB GENERAL INSURER OCTOBER 2020 */

        /* RFC-10310 BENEFICIAL LIFE */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string chk_beneficial_premium_amt(string prem_amount)
        {
            DataTable dt = null;

            decimal amt = 0;
            int decimal_allowed = 1;

            try
            {
                if (prem_amount == null || prem_amount == string.Empty)
                {
                    return "Please input valid premium amount.";
                }

                if (prem_amount != null && prem_amount.IndexOf(",") > 0)
                {
                    return "Please input valid premium amount. Comma not allowed.";
                }

                if (Decimal.TryParse(prem_amount, out amt) == true)
                {
                    try
                    {
                        dt = sqlkonek.getDataSet("obps_benlife_decimal_flag", true, new object[] { }).Tables[0];

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            decimal_allowed = Convert.ToInt32(dt.Rows[0]["cnt_flag"]);
                        }
                    }
                    catch
                    {
                    }

                    string prem_amt = amt.ToString();

                    if (decimal_allowed == 0 && prem_amt.IndexOf(".") >= 0)
                    {
                        return "Please input valid premium amount. Decimal not allowed.";
                    }

                    if (amt > 0)
                    {
                        return "VALID";
                    }
                }

                return "Please input valid premium amount.";
            }
            catch
            {
                return "Please input valid premium amount.";
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
        /* RFC-10310 BENEFICIAL LIFE */

        /* BOYET FEB 10 2019 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string InquirePRN(string prn)
        {
            System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = null;
            SSSPRN_API myprn = null;
            string inquire_prn_response = string.Empty;
            string[] response_array = null;

            try
            {
                /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                string rtpFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription");
                if (rtpFlag == "1")
                {
                    inquire_prn_response = this.InquireRTP("", prn, "0.00", "BC_SSS", "", "", "VALIDATE");
                }  /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                else
                {
                    /* RFC-5305 LOAN PRN CHECKING */
                    if (this.is_sss_loan_prn_prefix(prn) == true)
                    {
                        return "PRN is not valid for contribution payment. Please select the correct SSS biller.";
                    }
                    /* RFC-5305 LOAN PRN CHECKING */

                    myprn = new SSSPRN_API();
                    myprn.PRNNumber = prn;
                    inquire_prn_response = myprn.submitRequest(SSSPRN_API.SSSPayment.INQUIRE_PRN);
                }

                /* TRAP SSS PRN INQUIRY FOR UNSUCCESSFUL RESPONSE */
                if (inquire_prn_response == null || inquire_prn_response == string.Empty)
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = "No response from server. (SSS PRN INQUIRY)";
                }

                if (inquire_prn_response != null && inquire_prn_response != string.Empty)
                {
                    response_array = inquire_prn_response.Split('~');
                }

                if (response_array != null && response_array.Length > 0 && response_array[1] != "0")
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = inquire_prn_response + " (SSS PRN INQUIRY)";
                }
                else if (response_array != null && response_array.Length > 0 && response_array[1] == "0")
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = null; /* VALID SSS PRN NUMBER */
                }
                /* TRAP SSS PRN INQUIRY FOR UNSUCCESSFUL RESPONSE */

                return inquire_prn_response; /* RETURN ACTUAL WEBSERVICE RESPONSE TO FRONT END */
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = e.Message + " (SSS PRN INQUIRY)";
                return e.Message;
            }
            finally
            {
                myprn = null;
                inquire_prn_response = null;
            }
        }
        /* BOYET FEB 10 2019 */


        /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRTP_InstructionID(string prn_number, string referenceno)
        {
            DataTable dt = null;
            string InstructionID = string.Empty;

            try
            {
                dt = sqlkonek.getDataSet("getRTPInstructionID", true, new object[] { prn_number, referenceno }).Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return string.Empty;
            }
            catch (Exception e)
            {
                return "Unable to validate SSS_PRN_DETAILS_COMPLETE: getRTP_InstructionID|PRN:" + prn_number + "|" + e.Message ;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string InquireRTP(string subscribername, string memberid, string amount, string bc_code, string instructionId, string referenceno, string endpoint)
        {
            System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = null;

            BillsPaymentApi rtpApi = null;
            string rtp_response = string.Empty;
            string RTPFlag = string.Empty;
            string RTPBankCode = string.Empty;
            string[] response_array = null;
            string inquire_prn_response = null;
            /* RFC-5305 LOAN PRN CHECKING */
            //if (this.is_sss_loan_prn_prefix(memberid) == true)
            //{
            //    return "PRN is not valid for contribution payment. Please select the correct SSS biller.";
            //}
            /* RFC-5305 LOAN PRN CHECKING */

            try
            {
                rtpApi = new BillsPaymentApi();
                rtp_response = rtpApi.ConnectToRTP("", memberid, Convert.ToDecimal(amount), "BC_SSS", instructionId, referenceno, endpoint);

                /* TRAP SSS PRN INQUIRY FOR UNSUCCESSFUL RESPONSE */
                if (rtp_response == null || rtp_response == string.Empty)
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = "No response from server. (SSS PRN INQUIRY)";
                }

                if (rtp_response != null && rtp_response != string.Empty)
                {
                    response_array = rtp_response.Split('~');
                }

                /* 
                DMND0002382 -Remittance SSS Enhanced e-Collection Service 
                NEED TO ADJUST HERE AS WE NEED TO GET ALL THE DETAILS FROM RTP
                */
                if (response_array != null && response_array.Length <= 4)
                {
                    rtp_response = response_array[2];
                }
                if (response_array != null && response_array.Length > 0 && response_array[1] != "0")
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = rtp_response + " (SSS PRN INQUIRY)";
                }
                if (response_array != null && response_array.Length > 0 && response_array[1] != "01")
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = rtp_response + " (SSS PRN INQUIRY)";
                }
                if (response_array != null && response_array.Length > 0 && response_array[1] != "00")
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = rtp_response + " (SSS PRN INQUIRY)";
                }
                else if (response_array != null && response_array.Length > 0 && response_array[1] == "0")
                {
                    System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = null; /* VALID SSS PRN NUMBER */
                }
                /* TRAP SSS PRN INQUIRY FOR UNSUCCESSFUL RESPONSE */

                return rtp_response; /* RETURN ACTUAL WEBSERVICE RESPONSE TO FRONT END */
            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Session["SSS_PRN_DETAILS_COMPLETE"] = e.Message + " (SSS PRN INQUIRY)";
                return e.Message;
            }
            finally
            {
                rtpApi = null;
            }
        }
        /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string InquirePRN1(string prn)
        {
            SSSPRN_API myprn = new SSSPRN_API();
            myprn.PRNNumber = prn;
            return myprn.submitRequest(SSSPRN_API.SSSPayment.INQUIRE_PRN);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string InquireSSSNo(string sssno, string dob)
        {
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            string RTPFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription").ToString();
            if (RTPFlag == "1")
            {
                return "";
            }
            else {
                SSSPRN_API myprn = new SSSPRN_API();
                myprn.SSSNo = sssno;
                myprn.Birthdate = Convert.ToDateTime(dob).ToString("MM/dd/yyyy");
                return myprn.submitRequest(SSSPRN_API.SSSPayment.INQUIRE_VIA_SSS);
            }
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        }
        
       [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string SubmitPRN(string submitvalue, string referenceno)
        {
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            string RTPFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription").ToString();
            if (RTPFlag == "1")
            {
                return "";
            }
            else
            {
                /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
                SSSPRN_API myprn = new SSSPRN_API();
                irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                myprn.PRNNumber = submitvalue;
                myprn.TransactionDate = new irsTimeZone().branchLocalTime(clientdata.Location).ToString("MM/dd/yyyy HH:mm:ss");
                myprn.TransactionNumber = referenceno;
                return myprn.submitRequest(SSSPRN_API.SSSPayment.SUBMIT_PRN);
            }
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ValidatePesoFund(string payment)
        {
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
            string RTPFlag = new irsInternetPage().getLookupValue("RTPConfiguration", "FldCode", "FLAG", "FldDescription").ToString();
            if (RTPFlag == "1")
            {
                return "";
            }
            else
            {
                SSSPRN_API SSSPE = new SSSPRN_API();
                return SSSPE.PesoFund(payment);
            }
            /* DMND0002382 -Remittance SSS Enhanced e-Collection Service */
        }

        //=======================================================================================
        //RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool got_client_hit_list_all_obp(string remitter_id, string beneficiary_id)
        {
            RemOTC rem_otc = new RemOTC();
            return rem_otc.got_client_hit_list_all(remitter_id, beneficiary_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool got_client_hit_list_all_obp_refinitiv(string remitter_id, string beneficiary_id)
        {
            RemOTC rem_otc = new RemOTC();
            Refinitiv refinitivcs = new Refinitiv();
            return rem_otc.got_client_hit_list_all_refinitiv(remitter_id, beneficiary_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_cif_enrollment_pending_ovr_obp(string remitter_id, string beneficiary_id)
        {
            RemOTC rem_otc = new RemOTC();
            Refinitiv refinitivcs = new Refinitiv();
            string isRefinitivEnabled = refinitivcs.isRefinitivEnabled();
            return rem_otc.check_cif_enrollment_pending_ovr(remitter_id, beneficiary_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_cif_enrollment_pending_ovr_obp_refinitiv(string remitter_id, string beneficiary_id)
        {
            RemOTC rem_otc = new RemOTC();
            Refinitiv refinitivcs = new Refinitiv();
            string isRefinitivEnabled = refinitivcs.isRefinitivEnabled();
            return rem_otc.check_cif_enrollment_pending_ovr_refinitiv(remitter_id, beneficiary_id);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void log_hitlist_details(string tranx_seq_id)
        {
            RemTranSaving rem_tran_saving = new RemTranSaving();
            rem_tran_saving.log_hitlist(Convert.ToInt32(tranx_seq_id));
        }
        //=======================================================================================
    }

    [Serializable()]
    public class OBPSAgentOverride : sqlHelperWs
    {
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_calculator_for_override_values(string irs_calculator_session_name)
        {
            if (System.Web.HttpContext.Current.Session[irs_calculator_session_name] != null)
            {
                irsCalculator calc = (irsCalculator)System.Web.HttpContext.Current.Session[irs_calculator_session_name]; return calc.get_dataset_record_for_override();
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] user_can_override_rates(string user_name, string user_pwd)
        {
            return new Object[] { true, true };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_calculator_obps_override_values(string override_values, string irs_calculator_session_name)
        {
            if (System.Web.HttpContext.Current.Session[irs_calculator_session_name] != null)
            {
                irsCalculator icalc = (irsCalculator)System.Web.HttpContext.Current.Session[irs_calculator_session_name]; icalc.set_dataset_record_for_override(override_values); System.Web.HttpContext.Current.Session[irs_calculator_session_name] = icalc;
            }
        }
    }
}
