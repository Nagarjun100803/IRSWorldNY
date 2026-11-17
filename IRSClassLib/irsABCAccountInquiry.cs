using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using GRSMQ;

namespace IRSClassLib
{
    [Serializable]
    public class irsABCAccountInquiry
    {
        bool isASB = false;
        PNBABCAccountValidation.Service1 ASBVal = null;

        private String _message = "";

        public Boolean isAlliedAccount
        {
            get { return isASB; }
        }

        public bool allied_check_digit(string pStrAccountNo)
        {
            char[] fAStrAccountNo = pStrAccountNo.ToCharArray();
            int fIntCheckDigit = 0;

            try
            {
                switch (fAStrAccountNo.Length)
                {
                    case 10:

                        for (int x = 0; x <= fAStrAccountNo.Length - 2; x++)
                        {
                            switch (x)
                            {
                                case 0: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                case 1: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                case 2: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                case 3: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                                case 4: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 6; break;
                                case 5: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                case 6: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                case 7: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                case 8: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                            }
                        }

                        break;

                    case 12:

                        for (int x = 2; x <= fAStrAccountNo.Length - 2; x++)
                        {
                            switch (x)
                            {
                                case 2: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                case 3: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                case 4: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                case 5: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                                case 6: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 6; break;
                                case 7: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                case 8: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                case 9: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                case 10: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                            }
                        }

                        break;

                    case 13:

                        if (Convert.ToInt32(pStrAccountNo.Trim().Substring(2, 1)) >= 3)
                        {
                            for (int x = 0; x <= fAStrAccountNo.Length - 2; x++)
                            {
                                switch (x)
                                {
                                    case 0: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 8; break;
                                    case 1: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 7; break;
                                    case 2: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 6; break;
                                    case 3: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                    case 4: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                    case 5: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                    case 6: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                                    case 7: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 6; break;
                                    case 8: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                    case 9: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                    case 10: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                    case 11: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                                }
                            }

                        }
                        else
                        {
                            for (int x = 0; x <= fAStrAccountNo.Length - 2; x++)
                            {
                                switch (x)
                                {
                                    case 0: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 0; break;
                                    case 1: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 0; break;
                                    case 2: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 0; break;
                                    case 3: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                    case 4: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                    case 5: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                    case 6: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                                    case 7: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 6; break;
                                    case 8: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 5; break;
                                    case 9: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 4; break;
                                    case 10: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 3; break;
                                    case 11: fIntCheckDigit += int.Parse(fAStrAccountNo[x].ToString()) * 2; break;
                                }
                            }
                        }

                        break;
                }


                fIntCheckDigit = (fIntCheckDigit % 11) - 11;

                if (fIntCheckDigit < 0)
                {
                    fIntCheckDigit = fIntCheckDigit * -1;
                }

                if (fIntCheckDigit != int.Parse(fAStrAccountNo[pStrAccountNo.Length - 1].ToString()))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
            }
            return false;
        }

        public bool ABCAlgo(String sno)
        {
            String acctno = sno.Trim();
            String web_service_msg = String.Empty;

            bool chk_digit = false;

            try
            {
                ASBVal = new IRSClassLib.PNBABCAccountValidation.Service1();
                ASBVal.Url = new irsConfiguration().appsetting("PNBABCAccountValidation");

                web_service_msg = ASBVal.PNBABCValidateAccount("VALIDATEACCOUNT " + acctno);

                if (String.IsNullOrEmpty(web_service_msg).Equals(false) && web_service_msg.Trim().ToUpper() == "VALID")
                {
                    web_service_msg = string.Empty;
                    web_service_msg = ASBVal.PNBABCValidateAccount("GETACCOUNTDETAILS " + acctno);

                    if (String.IsNullOrEmpty(web_service_msg).Equals(false) && web_service_msg.Trim().ToUpper() != "ERROR")
                    {
                        return true;
                    }
                }

                /*** comment out for ASB / ABC Validation. Accountno will now be handled in the PNBABCAccountValidation webservice - Randy 01/12/2017
                chk_digit = this.allied_check_digit(sno.Trim());

                if (chk_digit == true)
                {
                    if (String.IsNullOrEmpty(acctno).Equals(false))
                    {
                        if (acctno.Length == 12 && acctno.StartsWith("00"))
                        {
                            acctno = acctno.Substring(acctno.Length - 10, 10).Trim();
                        }
                        else if (acctno.Length == 13)
                        {
                            acctno = acctno.Substring(acctno.Length - 10, 10).Trim();
                        }
                        else if (acctno.Length == 10 && acctno.StartsWith("8"))
                        {
                            return true; // FOR ALLIED SAVINGS BANK. NO NEED TO INQUIRE BECAUSE ACCOUNT IS NOT FOUND IN ACTIVE TABLE. SEPT 30 2016
                        }
                    }

                    web_service_msg = ASBVal.PNBABCValidateAccount("VALIDATEACCOUNT " + acctno);

                    if (String.IsNullOrEmpty(web_service_msg).Equals(false) && web_service_msg.Trim().ToUpper() == "VALID")
                    {
                        web_service_msg = string.Empty;
                        web_service_msg = ASBVal.PNBABCValidateAccount("GETACCOUNTDETAILS " + acctno);

                        if (String.IsNullOrEmpty(web_service_msg).Equals(false) && web_service_msg.Trim().ToUpper() != "ERROR")
                        {
                            return true;
                        }
                    }
                }*/

                /*** comment out for ASB / ABC Validation by Jeffrey Buizon 09/26/2016
                chk_digit = this.allied_check_digit(sno.Trim());

                if (chk_digit == true)
                {
                    if (String.IsNullOrEmpty(acctno).Equals(false))
                    {
                        if (acctno.Length > 10)
                        {
                            acctno = acctno.Substring(acctno.Length - 10, 10).Trim();
                        }
                    }

                    web_service_msg = ASBVal.PNBABCValidateAccount("VALIDATEACCOUNT " + acctno);

                    if (String.IsNullOrEmpty(web_service_msg).Equals(false) && web_service_msg.Trim().ToUpper() == "VALID")
                    {
                        web_service_msg = string.Empty;
                        web_service_msg = ASBVal.PNBABCValidateAccount("GETACCOUNTDETAILS " + acctno);

                        if (String.IsNullOrEmpty(web_service_msg).Equals(false) && web_service_msg.Trim().ToUpper() != "ERROR")
                        {
                            return true;
                        }
                    }
                }
                return chk_digit;*/
            }
            catch (Exception e)
            {
                _message = e.Message; 
            }

            return false;
        }

        public string getABCAccountDetails(string AccountNo)
        {
            string sReturn = "";
            string strproductcode = "";
            string[] arrSavings = { "0", "2", "6" };
            string[] arrChecking = { "1", "3", "4", "5", "7", "8", "9" };
            string[] arrASBPeso = { "0", "1", "3", "4", "5", "6", "7" };
            string[] arrASBDollar = { "2", "8", "9" };

            try
            {
                ASBVal = new IRSClassLib.PNBABCAccountValidation.Service1();
                ASBVal.Url = new irsConfiguration().appsetting("PNBABCAccountValidation");

                /*** comment out for ASB / ABC Validation. Accountno will now be handled in the PNBABCAccountValidation webservice - Randy 01/12/2017
                String AccountNo_value = AccountNo.Trim();
                if (AccountNo_value.Length > 10 && AccountNo.StartsWith("00"))
                {
                    AccountNo = AccountNo_value.Substring(AccountNo_value.Length - 10, 10);
                }
                else
                {
                    AccountNo = AccountNo_value;
                }*/

                if (AccountNo.Length == 10 && AccountNo.StartsWith("8"))
                {
                    foreach (string x1 in arrASBPeso)
                    {
                        if (x1.Contains(AccountNo.Substring(3, 1)))
                        {
                            sReturn = AccountNo + ":PHP";
                            break;
                        }
                        else
                        {
                            sReturn = AccountNo + ":USD";
                        }
                    }
                }
                else
                {
                    sReturn = ASBVal.PNBABCValidateAccount("GETACCOUNTDETAILS " + AccountNo);
                }

                if (sReturn != "ERROR")
                {
                    string[] acctdet = sReturn.Split(':');
                    if (acctdet.Length > 0)
                    {
                        foreach (string x in arrSavings)
                        {
                            if (x.Contains(AccountNo.Substring(3, 1)))
                            {
                                strproductcode = "709";
                                break;
                            }
                            else
                            {
                                strproductcode = "698";
                            }
                        }
                        sReturn = "OLDCUSTOMERID = 0|PRODUCTCODE = " + strproductcode + "|ACCOUNTID = " + acctdet[0].ToString() + "|BRANCHCODE = A" + acctdet[0].ToString().Substring(0, 3) + "|IC = |CUSTOMERNAME = |ACCOUNTSTATUS = 8|NEWCUSTOMERID = 0|CUSTOMERRELATIONSHIP = SOW|BRANCHCODE = A" + acctdet[0].ToString().Substring(0, 3) + "|PRODUCTCODE = " + strproductcode + "|MAINTENANCETICKET = 0|CURRENCYCODE = 0|ACCOUNTTITLE = ACCOUNT" + acctdet[0].ToString() + "|ACCOUNTID = " + acctdet[0].ToString() + "|BRANCHNAME = |CUSTOMERID = 0|CUSTOMERSHORTNAME = |STATUSTYPE = 8|PRODUCTNAME = |CURRENCYSHORTNAME = " + acctdet[1].ToString() + "|BALANCEBOOK = 0|CURRENCYSHORTNAME = " + acctdet[1].ToString() + "|CURRENTSTATUS = 8|BILLAMOUNT = 0|MODULECODE = C|LCYAMOUNT = 0|ACCOUNTID = " + acctdet[0].ToString() + "|CURRENCYCODE = 0|ORIGINALBALANCE = 0|CLASSIFICATION = |CUSTOMERRELATIONSHIP = |ACYAMOUNT = 0|TOTALACYAMOUNT = 0|PRODUCTNAME = |FUTUREDATEDAMOUNT = 0|TOTALLCYAMOUNT = 0|REASON = |UNCLEARFUNDS = 0|";
                    }
                }
                else if (isAlliedAccount == true && sReturn == "ERROR")
                {
                    foreach (string x1 in arrASBPeso)
                    {
                        if (x1.Contains(AccountNo.Substring(3, 1)))
                        {
                            sReturn = AccountNo + ":PHP";
                            break;
                        }
                        else
                        {
                            sReturn = AccountNo + ":USD";
                        }
                    }

                    string[] acctdet = sReturn.Split(':');
                    if (acctdet.Length > 0)
                    {
                        foreach (string x in arrSavings)
                        {
                            if (x.Contains(AccountNo.Substring(3, 1)))
                            {
                                strproductcode = "709";
                                break;
                            }
                            else
                            {
                                strproductcode = "698";
                            }
                        }
                        sReturn = "OLDCUSTOMERID = 0|PRODUCTCODE = " + strproductcode + "|ACCOUNTID = " + acctdet[0].ToString() + "|BRANCHCODE = A" + acctdet[0].ToString().Substring(0, 3) + "|IC = |CUSTOMERNAME = |ACCOUNTSTATUS = 8|NEWCUSTOMERID = 0|CUSTOMERRELATIONSHIP = SOW|BRANCHCODE = A" + acctdet[0].ToString().Substring(0, 3) + "|PRODUCTCODE = " + strproductcode + "|MAINTENANCETICKET = 0|CURRENCYCODE = 0|ACCOUNTTITLE = ACCOUNT" + acctdet[0].ToString() + "|ACCOUNTID = " + acctdet[0].ToString() + "|BRANCHNAME = |CUSTOMERID = 0|CUSTOMERSHORTNAME = |STATUSTYPE = 8|PRODUCTNAME = |CURRENCYSHORTNAME = " + acctdet[1].ToString() + "|BALANCEBOOK = 0|CURRENCYSHORTNAME = " + acctdet[1].ToString() + "|CURRENTSTATUS = 8|BILLAMOUNT = 0|MODULECODE = C|LCYAMOUNT = 0|ACCOUNTID = " + acctdet[0].ToString() + "|CURRENCYCODE = 0|ORIGINALBALANCE = 0|CLASSIFICATION = |CUSTOMERRELATIONSHIP = |ACYAMOUNT = 0|TOTALACYAMOUNT = 0|PRODUCTNAME = |FUTUREDATEDAMOUNT = 0|TOTALLCYAMOUNT = 0|REASON = |UNCLEARFUNDS = 0|";
                    }
                }
                else
                {
                    sReturn = "INVALID~Account Details not found.";
                }
            }
            catch (Exception ex)
            {
                sReturn = "INVALID~" + ex.Message;
            }
            return sReturn;
        }

        public string getAccountType(int ProductCode, bool IsNumeric)
        {
            string sReturn = string.Empty;
            try
            {
                if ((ProductCode >= 699) && (ProductCode <= 999))
                {
                    sReturn = (IsNumeric == true) ? "1" : "SAVINGS";
                }
                else
                {
                    if ((ProductCode >= 599) && (ProductCode <= 700))
                    {
                        sReturn = (IsNumeric == true) ? "2" : "CURRENT";
                    }
                    else
                    {
                        sReturn = (IsNumeric == true) ? "-1" : "INVALID";
                    }
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
            }
            return sReturn;
        }

        public string findField(string[] Response, string FindValue)
        {
            int i = 0;
            string sValue = string.Empty;
            string[] arr;
            string sReturn = string.Empty;

            try
            {
                for (i = 0; i <= (Response.Length - 1); i++)
                {
                    sValue = Response.GetValue(i).ToString();
                    arr = sValue.Split('=');
                    if (arr.GetValue(0).ToString().Trim().ToLower() == FindValue.ToString().Trim().ToLower())
                    {
                        sReturn = arr.GetValue(1).ToString().Trim();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
            }
            return sReturn;
        }

        public string findField(DataTable dt, string FindValue, string MatchColumn, string ReturnColumn)
        {
            int i = 0;
            string sReturn = string.Empty;

            try
            {
                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    if (dt.Rows[i][MatchColumn].ToString().Trim().ToLower() == FindValue.ToString().Trim().ToLower())
                    {
                        sReturn = dt.Rows[i][ReturnColumn].ToString().Trim();
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
            }
            return sReturn;
        }
    }
}
