/* 
 * Author: Jeffrey M. Buizon
 * Desc:    Program to convert/extract data from irsworld to GRS
 * Date Created: 01.11.2011
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.IO;
namespace IRSClassLib
{
    public class irsFileExtracter 
    {
        #region IRS CLASS
        public class irsIRSFunction : irsSqlCommons
        {
            #region IRS Record
            class irsRecord : irsSqlCommons 
            {
                #region VARIABLES
                public string errmsg = "";
                public String TranxID = "";
                public String NegBranchCode = ""; // 4
                public DateTime DateSent;
                public String RefNo = ""; // 16
                public String Beneficiary_Lastname = ""; // 80
                public String Beneficiary_Firstname = ""; // 80
                public String Beneficiary_MiddleInitial = ""; // 80                  
                public String Beneficiary_Address1 = "";  // 50
                public String Beneficiary_Address2 = "";  // 50
                public String Beneficiary_Address3 = "";  // 50
                public int PayeeType;
                public String PNBBranchCode = "";  // 4                    
                public int PayMode;
                public int AccountType;
                public String AccountNo = ""; // 40                    
                public String AccountCurrencyCode = ""; // 4
                public String BankName = "";  // 120
                public int ImplementationCode;
                public String Remitter_Name = ""; // 163
                public String RemittanceType = ""; // 1
                public String SpecialMessage = ""; // 200
                public int RemitBenCurrency;
                public Decimal NetRemitPHP;
                public Decimal RemitBenAmt;
                public Decimal RemitPHP;
                public int TranxStatus;
                public int XmitStatus;
                public DateTime DateImplemented;
                public int CoveringBank;
                public String Remitter_LastName = ""; // 80
                public String Remitter_Firstname = "";  // 80
                public String Remitter_MiddleInitial = "";  // 80
                public String Remitter_Address = ""; // 200 
                public String Remitter_Phone = "";  // 200
                public String Beneficiary_MobileNo = ""; // 50
                public String Beneficiary_Address4 = "";  // 50
                public String Beneficiary_Country = ""; // 16
                public String Beneficiary_TelNo = "";  // 50
                public String Beneficiary_Email = "";  // 80
                public String Osa = "";
                //ITG Password encryption
                //private string irsconn = new irsConfiguration().appsetting("IRSConnectionString");
                private string irsconn = new irsConfiguration().ConnectionStringNoRunMode("IRSConnectionString");
                

                #endregion
                public irsRecord()
                {
                    
                }
                #region PUBLIC METHOD
                public bool  format_irs_record(DataRow r)
                {
                    try
                    {
                        this.assign_orps_values(r);

                        
                        return true;
 
                    }
                    catch(Exception e)
                    {
                        this.errmsg = ":" + e.Message + " " + e.StackTrace;
                        irsFileExtracter.irsIRSFunction.logme(this.errmsg);
                    }
                    return false;

                }
                public string irsformat_tostring()
                {

                    string s =
                            this.clean_data(this.TranxID.ToString()) + "|" +
                            this.clean_data(this.NegBranchCode )+ "|" +
                            this.clean_data(this.DateSent.ToString()) + "|" +
                            this.clean_data(this.RefNo )+ "|" +
                            this.clean_data(this.Beneficiary_Lastname ) + "|" +
                            this.clean_data(this.Beneficiary_Firstname ) + "|" +
                            this.clean_data(this.Beneficiary_MiddleInitial ) + "|" +
                            this.clean_data(this.Beneficiary_Address1 )+ "|" +
                            this.clean_data(this.Beneficiary_Address2 )+ "|" +
                            this.clean_data(this.Beneficiary_Address3) + "|" +
                            this.clean_data(this.PayeeType.ToString()) + "|" +
                            this.clean_data(this.PNBBranchCode) + "|" +
                            this.clean_data(this.PayMode.ToString()) + "|" +
                            this.clean_data(this.AccountType.ToString()) + "|" +
                            this.clean_data(this.AccountNo) + "|" +
                            this.clean_data(this.AccountCurrencyCode) + "|" +
                            this.clean_data(this.BankName) + "|" +
                            this.clean_data(this.ImplementationCode.ToString()) + "|" +
                            this.clean_data(this.Remitter_Name )+ "|" +
                            this.clean_data(this.RemittanceType )+ "|" +
                            this.clean_data(this.SpecialMessage) + "|" +
                            this.clean_data(this.RemitBenCurrency.ToString()) + "|" +
                            this.clean_data(this.NetRemitPHP.ToString() ) + "|" +
                            this.clean_data(this.RemitBenAmt.ToString()) + "|" +
                            this.clean_data(this.RemitPHP.ToString()) + "|" +
                            this.clean_data(this.TranxStatus.ToString()) + "|" +
                            this.clean_data(this.XmitStatus.ToString()) + "|" +
                            this.clean_data(this.DateImplemented.ToString()) + "|" +
                            this.clean_data(this.CoveringBank.ToString()) + "|" +
                            this.clean_data(this.Remitter_LastName) + "|" +
                            this.clean_data(this.Remitter_Firstname) + "|" +
                            this.clean_data(this.Remitter_MiddleInitial) + "|" +
                            this.clean_data(this.Remitter_Address )+ "|" +
                            this.clean_data(this.Remitter_Phone )+ "|" +
                            this.clean_data(this.Beneficiary_MobileNo )+ "|" +
                            this.clean_data(this.Beneficiary_Address4) + "|" +
                            this.clean_data(this.Beneficiary_Country) + "|" +
                            this.clean_data(this.Beneficiary_TelNo ) + "|" +
                            this.clean_data(this.Beneficiary_Email) + "|"   +
                            this.clean_data(this.Osa) ;

                    return s;
                }
                private string clean_data(string s)
                {
                    return s.Replace("\n", "").Replace("\t", "").Replace("\r","").Replace("\f","").Replace("\v","").Trim();
                }
                #endregion
                #region PRIVATE METHODS
                private bool assign_orps_values(DataRow drIRSW)
                {
                    AccountType AcctType;
                    string sTemp;
                    string sCurrencyCode = "";

                    try
                    {
                        AuditSettings();
                        this.TranxID =  "{" + drIRSW["TranxID"].ToString() + "}" ;
                        this.NegBranchCode = drIRSW["NegBranchCode"].ToString();
                        if (drIRSW["DateSent"].ToString().Length == 0)
                        {
                            this.DateSent = System.DateTime.Now;
                        }
                        else
                        {
                            this.DateSent = (DateTime)GetDateTimeValue(drIRSW["DateSent"].ToString());
                        }
                        this.RefNo = drIRSW["RefNo"].ToString();
                        this.Beneficiary_Lastname = drIRSW["Beneficiary_Lastname"].ToString();
                        this.Beneficiary_Firstname = drIRSW["Beneficiary_Firstname"].ToString();
                        this.Beneficiary_MiddleInitial = drIRSW["Beneficiary_MiddleInitial"].ToString();
                        this.Beneficiary_Address1 = drIRSW["Beneficiary_Address1"].ToString();
                        this.Beneficiary_Address2 = drIRSW["Beneficiary_Address2"].ToString();
                        this.Beneficiary_Address3 = drIRSW["Beneficiary_Address3"].ToString();
                        this.Remitter_LastName = drIRSW["Remitter_LastName"].ToString();
                        this.Remitter_Firstname = drIRSW["Remitter_Firstname"].ToString();
                        this.Remitter_MiddleInitial = drIRSW["Remitter_MiddleInitial"].ToString();
                        this.Remitter_Address = drIRSW["Remitter_Address"].ToString();
                        this.Remitter_Phone = drIRSW["Remitter_Phone"].ToString();
                        this.Beneficiary_MobileNo = drIRSW["Beneficiary_MobileNo"].ToString();
                        this.Beneficiary_Address4 = drIRSW["Beneficiary_Address4"].ToString();
                        this.Beneficiary_Country = GetGuidValue(drIRSW["Beneficiary_Country"].ToString()).ToString() ;
                        this.Beneficiary_TelNo = drIRSW["Beneficiary_TelNo"].ToString();
                        //this.Beneficiary_Email = drIRSW["Beneficiary_Email"].ToString() + "|" + drIRSW["TranxPurpose"].ToString();
                        this.Beneficiary_Email = drIRSW["Beneficiary_Email"].ToString();
                        this.AccountNo = drIRSW["AccountNo"].ToString();
                        this.RemittanceType = "A";
                        this.SpecialMessage = drIRSW["SpecialMessage"].ToString();
                        this.Osa = drIRSW["OSA"].ToString();
                        AcctType = this.GetAccountType(drIRSW["OtherAccountNo"].ToString());
                        sCurrencyCode = this.GetCurrencyCode(int.Parse(drIRSW["NetRemitBenCurrency"].ToString()));
                        this.AccountCurrencyCode = sCurrencyCode;

                        int iBranchCode = (drIRSW["PNBBranchCode"].ToString().Trim().Length > 0) ? int.Parse(drIRSW["PNBBranchCode"].ToString().Trim()) : 0;
                        

                        
                        switch (int.Parse(drIRSW["ImplementationCode"].ToString())) // implementationcode
                        {
                            case 1: //Advice and pay

                                this.AccountCurrencyCode = sCurrencyCode;
                                this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();
                                this.AccountNo = drIRSW["AccountNo"].ToString();
                                this.PayeeType = 0;
                                this.BankName = "";


                                if (
                                        iBranchCode == 1072 ||
                                        iBranchCode == 1074
                                    )
                                {
                                    if (drIRSW["AccountNo"].ToString().Trim() == "")
                                    {
                                        this.ImplementationCode = 6;
                                        this.PayMode = 0;
                                        this.AccountType = 0;
                                    }
                                    else
                                    {
                                        this.ImplementationCode = 4;
                                        this.PayMode = 1;
                                        this.AccountType = 4;
                                    }
                                }
                                else if ( iBranchCode != 1072
                                           && iBranchCode != 1074 
                                           &&   (iBranchCode < 1300 || iBranchCode == 1498 || iBranchCode == 1502 || iBranchCode == 1504 || iBranchCode == 1506 || iBranchCode == 1507 || iBranchCode == 1508))
                                {
                                    if (drIRSW["AccountNo"].ToString().Trim() == "")
                                    {
                                        this.ImplementationCode = 5;
                                        this.PayMode = 0;
                                        this.AccountType = 0;
                                    }
                                    else
                                    {
                                        this.ImplementationCode = 5;
                                        this.PayMode = 1;
                                        this.AccountType = 4;
                                    }
                                }
                                else
                                {
                                    if (drIRSW["AccountNo"].ToString().Trim() == "")
                                    {
                                        this.ImplementationCode = 1;
                                        this.PayMode = 0;
                                        this.AccountType = 0;
                                    }
                                    else
                                    {
                                        this.ImplementationCode = 1;
                                        this.PayMode = 1;
                                        this.AccountType = 4;
                                    }
                                }
                                break;
                            case 3: //Door to Door
                                this.ImplementationCode = 2;
                                this.PayMode = 0;
                                this.AccountType = 0;
                                this.AccountCurrencyCode = sCurrencyCode;
                                this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();
                                this.AccountNo = "";
                                this.PayeeType = 0;
                                this.BankName = "";
                                break;
                            case 4:	//PCHC/PDDTS
                                this.PayeeType = 3;
                                this.PNBBranchCode = "1072";
                                this.PayMode = 1;
                                this.AccountType = 1;
                                this.AccountNo = drIRSW["AccountNo"].ToString();
                                this.BankName = drIRSW["PNB_MerchantCode"].ToString().Trim() + '-' + drIRSW["OtherBank"].ToString().Trim();



                                if (int.Parse(drIRSW["NetRemitBenCurrency"].ToString()) == 2) //USD
                                {
                                    this.ImplementationCode = 3;
                                    this.AccountCurrencyCode = sCurrencyCode;
                                }
                                else if (int.Parse(drIRSW["NetRemitBenCurrency"].ToString()) == 1)  //PHP
                                {
                                    this.ImplementationCode = 6;
                                    this.AccountCurrencyCode = sCurrencyCode;
                                }
                                /*=========================================================================*/
                                /*added code by randy for Bank Of China Handling of EURO Accounts 09152009*/
                                else if (int.Parse(drIRSW["NetRemitBenCurrency"].ToString()) == 5)  //EUR
                                {
                                    this.ImplementationCode = 3;
                                    this.AccountCurrencyCode = sCurrencyCode;
                                }
                                /*=========================================================================*/
                                break;
                            case 5:	//Bank to Bank

                                this.AccountCurrencyCode = sCurrencyCode;
                                this.AccountNo = drIRSW["OtherAccountNo"].ToString();
                                this.PayeeType = 3;
                                this.BankName = "000 " + drIRSW["OtherBank"].ToString().Trim();

                                if (iBranchCode < 1300 || iBranchCode == 1498 || iBranchCode == 1502 || iBranchCode == 504 || iBranchCode == 1506 || iBranchCode == 1507 || iBranchCode == 1508)
                                {
                                    this.PNBBranchCode = "1072";
                                    this.ImplementationCode = 6;
                                    this.PayMode = 1;
                                    this.AccountType = 4;
                                }
                                else
                                {
                                    this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();
                                    this.ImplementationCode = 1;
                                    this.PayMode = 1;
                                    this.AccountType = 4;
                                }
                                break;
                            case 6:	//Payment
                                this.ImplementationCode = 1;
                                this.PayMode = 0;
                                this.AccountType = 0;
                                this.AccountCurrencyCode = sCurrencyCode;
                                this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();	//SqlCmd.Parameters["@PNBBranchCode"].Value = Remittance.GetValue(98).ToString();
                                this.AccountNo = drIRSW["OtherAccountNo"].ToString();
                                this.PayeeType = 0;
                                this.BankName = "";
                                sTemp = "Payment for " + drIRSW["PNB_MerchantCode"].ToString() + " Reference No. " + drIRSW["MerchantRefNo"].ToString();
                                this.Beneficiary_Address2 = "";
                                this.Beneficiary_Address3 = "";

                                if (sTemp.Length < 51)
                                    this.Beneficiary_Address1 = sTemp;
                                else if (sTemp.Length < 101)
                                {
                                    this.Beneficiary_Address1 = sTemp.Substring(0, 50);
                                    this.Beneficiary_Address2 = sTemp.Substring(51);
                                }
                                else
                                {
                                    this.Beneficiary_Address1 = sTemp.Substring(0, 50);
                                    this.Beneficiary_Address2 = sTemp.Substring(51, 50);
                                    this.Beneficiary_Address3 = sTemp.Substring(101, Math.Min(50, sTemp.Length - 100));
                                }
                                break;
                            case 7:	//Open an account

                                if (iBranchCode == 1072 || iBranchCode == 1074)
                                {
                                    this.ImplementationCode = 4;
                                    this.PayMode = 1;
                                    this.AccountType = 3;
                                }
                                else if (iBranchCode != 1072 && (iBranchCode != 1074 && iBranchCode < 1300 || iBranchCode == 1498 || iBranchCode == 1502 || iBranchCode == 1504 || iBranchCode == 1506 || iBranchCode == 1507 || iBranchCode == 1508))
                                {

                                    this.ImplementationCode = 5;
                                    this.PayMode = 1;
                                    this.AccountType = 3;
                                }
                                else
                                {
                                    this.ImplementationCode = 1;
                                    this.PayMode = 1;
                                    this.AccountType = 3;
                                }
                                this.AccountCurrencyCode = sCurrencyCode;
                                this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();	//SqlCmd.Parameters["@PNBBranchCode"].Value = Remittance.GetValue(98).ToString();
                                this.AccountNo = drIRSW["OtherAccountNo"].ToString();
                                this.PayeeType = 0;
                                this.BankName = "";

                                sTemp = "Open an account " + drIRSW["SpecialMessage"].ToString();	//sTemp = "Open an account " + Remittance.GetValue(77).ToString();
                                if (sTemp.Length > 200)
                                    this.SpecialMessage = sTemp.Substring(0, 200);
                                else
                                    this.SpecialMessage = sTemp;
                                break;
                            case 8:	//Reactivate an account
                                this.ImplementationCode = 1;
                                this.PayMode = 0;
                                this.AccountType = 0;

                                this.AccountCurrencyCode = sCurrencyCode;

                                this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();
                                this.AccountNo = drIRSW["OtherAccountNo"].ToString();
                                this.PayeeType = 0;
                                this.BankName = "";
                                sTemp = "Reactivate account " + drIRSW["SpecialMessage"].ToString();	//sTemp = "Reactivate account " + Remittance.GetValue(77).ToString();
                                if (sTemp.Length > 200)
                                    this.SpecialMessage = sTemp.Substring(0, 200);
                                else
                                    this.SpecialMessage = sTemp;
                                break;
                            case 9:	// local implementation
                                break;
                            case 10:	//Door to Bank
                                this.ImplementationCode = 2;
                                this.PayMode = 0;
                                this.AccountType = 0;
                                this.AccountCurrencyCode = sCurrencyCode;
                                this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();
                                this.AccountNo = "";
                                this.PayeeType = 0;
                                this.BankName = "";
                                this.SpecialMessage = drIRSW["SpecialMessage"].ToString();
                                this.Beneficiary_Address1 = "DTB " + drIRSW["OtherBank"].ToString();
                                this.Beneficiary_Address2 = drIRSW["AccountNo"].ToString();

                                break;
                            case 11:	//Batch  //bojims 06-06-05                            

                                this.AccountType = Convert.ToInt16(drIRSW["AccountType"].ToString());
                                this.AccountCurrencyCode = sCurrencyCode;
                                this.PNBBranchCode = drIRSW["PNBBranchCode"].ToString();	//SqlCmd.Parameters["@PNBBranchCode"].Value = Remittance.GetValue(98).ToString();
                                this.AccountNo = drIRSW["AccountNo"].ToString();
                                this.PayeeType = 0;
                                this.BankName = "";

                                if (iBranchCode == 1072 || iBranchCode == 1074)
                                {
                                    this.ImplementationCode = 4;
                                    this.PayMode = 1;
                                }
                                else if (iBranchCode != 1072 && iBranchCode != 1074 && (iBranchCode < 1300 || iBranchCode == 1498 || iBranchCode == 1502 || iBranchCode == 1504 || iBranchCode == 1506 || iBranchCode == 1507 || iBranchCode == 1508))
                                {
                                    this.ImplementationCode = 5;
                                    this.PayMode = 1;
                                }
                                else
                                {
                                    this.ImplementationCode = 1;
                                    this.PayMode = 1;
                                }
                                break;
                        }
                        //sTemp = drIRSW["Remitter_LastName"].ToString() + ", " + drIRSW["Remitter_Firstname"].ToString() + " " + drIRSW["Remitter_MiddleInitial"].ToString();
                        //sTemp = sTemp.Trim();
                        this.Remitter_Name = drIRSW["Remitter_Name"].ToString();
                        this.RemitBenCurrency = (byte)GetByteValue(drIRSW["RemitBenCurrency"].ToString());
                        this.NetRemitPHP = (Decimal)GetDecimalValue(drIRSW["NetRemitPHP"].ToString());
                        this.RemitBenAmt = (Decimal)GetDecimalValue(drIRSW["RemitBenAmt"].ToString());
                        this.RemitPHP = (Decimal)GetDecimalValue(drIRSW["RemitPHP"].ToString());
                        this.TranxStatus = (byte)16;
                        this.XmitStatus = (byte)4;
                        this.DateImplemented = System.DateTime.Now;
                        this.CoveringBank = (byte)GetByteValue(drIRSW["CoveringBank"].ToString());


                        return true;
                    }
                    catch (Exception ex)
                    {
                        irsFileExtracter.irsIRSFunction.logme(ex.Message + " " + ex.StackTrace);
                    }
                    finally
                    {

                    }
                    return false;

                }
                private string padSpaces(string s, int maxlength,bool isright)
                {
                    int origlength = s.Length ;
                    string newstr = "";
                    if (isright )                    {

                        newstr = s + s.PadRight(maxlength - origlength, ' ');
                        return newstr.Substring(0,maxlength); 
                    }
                    newstr =  s + s.PadLeft(maxlength - origlength, ' ');
                    return newstr.Substring(0, maxlength); 
                }
                private void AuditSettings()
                {
                    //if (mySQLConnectionString == null || mySQLConnectionString.Length == 0)
                    //{
                    //    mySQLConnectionString = GetAppSetting("ConnectionString");
                    //    if (mySQLConnectionString == null || mySQLConnectionString.Length == 0)
                    //    {
                    //        stag.Framework.Application.Registry modRegistry = new stag.Framework.Application.Registry();
                    //        if (modRegistry.GetRegValue(regAppKeyName, "ConnectionString", stag.Framework.Application.RegType.REG_SZ))
                    //            mySQLConnectionString = CryptBase64.Decrypt(modRegistry.RegValueStr, "ConnectionString");
                    //    }
                    //    if (mySQLConnectionString == null || mySQLConnectionString.Length == 0)
                    //        throw new Exception("Application is not properly configured. Please contact your System Administrator.");
                    //}
                }
                private object GetByteValue(string oVal)
                {
                    if (oVal != null && oVal.Length > 0)
                        return byte.Parse(oVal);
                    else
                        return null;
                }
                private object GetDateTimeValue(string oVal)
                {
                    if (oVal != null && oVal.Length > 4)
                        return System.DateTime.Parse(oVal);
                    else
                        return null;
                }
                private object GetDecimalValue(string oVal)
                {
                    if (oVal != null && oVal.Length > 0)
                        return decimal.Parse(oVal);
                    else
                        return null;
                }
                private string GetCurrencyCode(int CurrencyCode)
                {
                
                    DataSet ds = new DataSet();
                    SqlConnection cn = new SqlConnection(this.irsconn);
                    SqlCommand cmd = new SqlCommand();
                    
                    string sMsg = string.Empty;

                    try
                    {             
                    cmd.Connection = cn;
                    cmd.CommandText = "SELECT FLDCODE FROM TBL_LOOKUP WHERE FLDGROUP = 'CURRENCY' AND FLDVALUE = " + CurrencyCode  ;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;
                    if (cn.State == 0) { cn.Open(); }

                        ds = base.getDataSet(cmd);
                        return ds.Tables[0].Rows[0]["FLDCODE"].ToString();
                    }
                    catch //Commented to remove warning (Exception e)
                    {
                    }
                    finally
                    {
                        cmd.Connection.Close();
                        cmd.Dispose();
                        cn.Dispose();
                    }



                    return sMsg;
                }
                private bool ValidateReader(ref SqlDataReader dr)
                {
                    if (dr == null || dr.IsClosed)
                        return false;
                    else
                        return true;
                }
                private AccountType GetAccountType(string AccountNo)
                {
                    //-------------------------------------------------------------
                    //Purpose : Derives/verifies account info given the Account No.
                    //           ex. Account# = 999-NXXXXXX
                    //           999 = Branch code
                    //           N = 5/6-(Peso S/A) 7-(Dollar S/A) 80-88-(Peso C/A) 89-(Dollar C/A)
                    //-------------------------------------------------------------
                    string sTemp = "";
                    string sAcctNo = "";
                    string sAcctBranch = "";
                    int AcctType = 0;
                    int AcctCurrency = 0;

                    AccountType retVal = new AccountType();
                    retVal.BranchCode = "";
                    retVal.AcctCurrency = 0;
                    retVal.AcctType = 0;


                    try
                    {
                        sAcctBranch = AccountNo.Substring(9);
                        sAcctNo = AccountNo;
                        sTemp = sAcctNo.Substring(3, 1);
                        if (sTemp.Length > 0)
                        {
                            switch (sTemp)
                            {
                                case "5":
                                case "6":
                                    AcctType = 1;        //Savings
                                    AcctCurrency = 1;    //Peso
                                    break;
                                case "7":
                                    AcctType = 1;        //'Savings
                                    AcctCurrency = 2;    //'Dollar
                                    break;
                            }
                        }

                        sTemp = sAcctNo.Substring(3, 2);
                        if (sTemp.Length > 0)
                        {
                            if (int.Parse(sTemp) >= 80 && int.Parse(sTemp) <= 88)
                            {
                                AcctType = 2;            //'Checking
                                AcctCurrency = 1;        //'Peso
                            }
                            else if (int.Parse(sTemp) >= 89)
                            {
                                AcctType = 2;            //'Checking
                                AcctCurrency = 2;        //'Dollar
                            }
                        }
                        retVal.BranchCode = sAcctBranch;
                        retVal.AcctType = AcctType;
                        retVal.AcctCurrency = AcctCurrency;
                    }
                    catch (System.Exception)
                    {
                    }
                    return retVal;
                }
                private object GetGuidValue(string oVal)
                {
                    if (oVal != null && oVal.Length > 31)
                        return new Guid(oVal);
                    else
                        return null;
                }
                private object GetValue(object oVal)
                {
                    if (oVal != null)
                        return oVal;
                    else
                        return null;
                }
                #endregion
            }
            #endregion
            #region VARIABLES

            private string temppath = new irsConfiguration().appsetting("TempRootPath");
            private string logpath = new irsConfiguration().appsetting("TempPath");
            private string rootpath = new irsConfiguration().appsetting("RootPath");
            //ITG Password encryption
            //private string irsconn = new irsConfiguration().appsetting("IRSConnectionString");
            private string irsconn = new irsConfiguration().ConnectionStringNoRunMode("IRSConnectionString");
            private struct AccountType
            {
                public string BranchCode;
                public int AcctType;
                public int AcctCurrency;
            };
            ArrayList alTranID = new ArrayList();
            ArrayList alInvTranID = new ArrayList();
            private Hashtable irs_file_contents;
            public  string sysid_fe = "IRWFE01";
            private string irs_filename ="";
            
            #endregion

            public irsIRSFunction()
            {
            }
            #region PUBLIC METHODS
            public void create_irs_extract_file()
            {                
                try
                {
                    irsFileExtracter.irsIRSFunction.logme("IRSW FILE EXTRACTER STARTING EXTRACT : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    this.get_irsworld_data();
                    if (this.irs_file_contents.Count <= 0)
                    {
                        irsFileExtracter.irsIRSFunction.logme("IRSW FILE EXTRACTER NO DATA TO EXTRACT: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString() );
                        return;
                    }
                    irsFileExtracter.irsIRSFunction.logme("IRSW FILE EXTRACTER DONE FETCHING DATA NOW CREATING FILE: " + System.DateTime.Now.ToShortDateString() + " ");
                    this.create_irs_file();
                    irsFileExtracter.irsIRSFunction.logme("IRSW FILE EXTRACTER DONE CREATING FILE NOW DELETING TEMP" + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    this.delete_temp();
                    irsFileExtracter.irsIRSFunction.logme("IRSW FILE EXTRACTER SUCCESSFULLY LOADED IRS FILE : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                }
                catch (Exception e)
                {                 
                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);  
                }
                

            }
            public static void logme(string msg)
            {               
                try
                {
                    StreamWriter sw;
                    string path = new irsConfiguration().appsetting("TempPath");
                    string str = DateTime.Now.ToString() + ": " + msg;
                    string dt = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');
                    sw = File.AppendText(path + "\\" + "IRSWIRSFE" + dt + ".log");
                    try
                    {
                        sw.WriteLine(str);
                        sw.Close();
                    }
                    catch { return; }
                    finally
                    {
                        str = null;
                        sw.Dispose();
                        sw = null;
                    }
                }
                catch { return; }
                finally { }
            }
            public void log_dtls(string msg)
            {
                try
                {
                    irsFileExtracter.irsIRSFunction.logme(msg);
                    
                }
               
                finally { }
            }
            private string clean_irs_data(string s)
            {
                return s.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\f", "").Replace("\v", "").Trim();
            }
            #endregion
            #region PRIVATE METHODS
            private void get_irsworld_data()
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                irsRecord rec = null;
                
                SqlConnection cn = new SqlConnection(this.irsconn);
                SqlCommand cmd = new SqlCommand();
                try
                {
                    this.irs_file_contents = new Hashtable(); ;
                    cmd.Connection = cn;
                    cmd.CommandText = "proc_get_irsworld_data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;


                    if (cn.State == 0) { cn.Open(); }

                    ds = base.getDataSet(cmd);
                    try
                    {
                        if (cmd.Parameters["@v_errmsg"].Value.ToString().Length == 0)
                        {
                            dt = ds.Tables[0];
                            this.irs_file_contents = new Hashtable();
                            int i = 0;
                            foreach (DataRow r in dt.Rows)
                            {
                                rec = new irsRecord();
                                if (r[1].ToString().Length > 0)
                                {
                                    bool p = rec.format_irs_record(r);
                                    if (p) { this.irs_file_contents.Add(i, rec.irsformat_tostring()); }
                                    i++;
                                }
                            }                           
                        }
                    }
                    catch (Exception e)
                    {   
                        irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);  
                    }


                }
                catch (Exception e)                {

                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);  
                }
            }
            private void get_update_irsworld_data()
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

                SqlConnection cn = new SqlConnection(this.irsconn);
                SqlCommand cmd = new SqlCommand();
                try
                {
                    this.irs_file_contents = new Hashtable(); ;
                    cmd.Connection = cn;
                    cmd.CommandText = "proc_update_irsworld_data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;


                    if (cn.State == 0) { cn.Open(); }

                    ds = base.getDataSet(cmd);
                    try
                    {
                        if (cmd.Parameters["@v_errmsg"].Value.ToString().Length == 0)
                        {
                       
                        }
                    }
                    catch (Exception e)
                    {
                        irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);
                    }


                }
                catch (Exception e)
                {

                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);
                }
            }
            private void delete_temp()
            {
                try
                {
                    new csFileIo().delete(temppath + this.irs_filename);
                }
                catch (Exception ex)
                {
                    irsFileExtracter.irsIRSFunction.logme("Temp delete: " + ex.Message + ":" + ex.StackTrace);
                }
            }
            private void create_irs_file()
            {
                csFileIo io;
                string irswsubalias = new irsConfiguration().ConnectionStringNoRunMode("IRSWSUBALIAS");
                try
                {
                    io = new csFileIo();


                    this.irs_filename = "GRSDATA_" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "_IRSW" + irswsubalias + ".txt";
                    string irs_tempfilename = temppath + this.irs_filename;

                    if (!io.directoryExist(temppath))
                    {
                        io.createDirectory(temppath);
                    }

                    if (io.exist(irs_tempfilename)) { io.delete(irs_tempfilename); }

                    io.open_output(irs_tempfilename, (int)csFileIo.write_modes.CREATE_NEW);

                     string hdr = this.create_irs_header(this.irs_file_contents.Count);

                    io.putLine(hdr);
                    for (int i = 0; i < this.irs_file_contents.Count; i++)
                    {

                        string s = (string)this.irs_file_contents[i];
                        io.putLine(this.clean_irs_data(s));
                    }
                    //io.putLine("\n");
                    io.close_output();
                    

                    
                    io.copy(irs_tempfilename , rootpath + this.irs_filename,true );
                    
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);  
                }
                finally
                {
                    io = null;
                }
            }
            private string create_irs_header(int rec_count)
            {
                //string boddate = this.tranxdate.ToString("YYYYMMdd");

                string shdr = "HDR" +
                        rec_count.ToString().PadLeft(Math.Abs(6 ), '0') +
                        "1" +
                        this.sysid_fe.PadLeft(20, ' ');

                return shdr;
            }  
            private void update_remittance(string branchcode, DateTime tranxdate, Int64 seq_id)
            {
                string sql = "UPDATE TBL_REMITTANCE SET TRANSTATUS = 99 ";
                sql += " WHERE RemittingBranch =" + branchcode + " and	convert(varchar,tranxdate,101) = convert(varchar," + tranxdate + ",101)";
                sql += " AND SEQ_ID = " + seq_id;

                int ra = base.sqlUpdate(sql);

                if (ra == 0)
                {
                    this.alInvTranID.Add(seq_id.ToString());
                }
            }
            private void ftp_irs_file()
            {
                string host = new irsConfiguration().appsetting("FTPIP");
                string username =  new irsConfiguration().appsetting("FTPUSER");
                string password = new irsConfiguration().appsetting("FTPPASS");
                string port = new irsConfiguration().appsetting("FTPPORT");
                string grsftppath  = new irsConfiguration().appsetting("FTPGRSPATH");
                csFtp ftp;
                try
                {
                    ftp = new csFtp(host, Convert.ToInt32(port), username, password);
                    if (ftp.connect())
                    {
                        if (ftp.cwd(grsftppath))
                        {
                            ftp.put(this.rootpath + this.irs_filename,this.irs_filename );
                        }
                        else
                        {
                            
                            irsFileExtracter.irsIRSFunction.logme("CANNOT CHANGE FTP PATH");  
                        }

                        ftp.disconnect();
                    }
                    else
                    {
                        
                        irsFileExtracter.irsIRSFunction.logme("CANNOT CONNECT TO FTP");  

                    }

                    
                    
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);
                    
                }
                finally
                {
                    
                    ftp = null;
                }

            }
            
            #endregion
        }
        #endregion

        #region ARS CLASS
        public class irsARSFunction : irsSqlCommons
        {
            #region VARIABLES
            int record_count;            
            int record_errors;                        
            public string sysid_arsfe = "170104";            
            
            private string temppath = new irsConfiguration().appsetting("TempRootPath");            
            private string rootpath = new irsConfiguration().appsetting("RootPath");
            //ITG Password encryption
            //private string irsconn = new irsConfiguration().appsetting("IRSConnectionString");
            //private string grsconn = new irsConfiguration().appsetting("GRSConnectionString");
            private string irsconn = new irsConfiguration().ConnectionStringNoRunMode("IRSConnectionString");
            private string grsconn = new irsConfiguration().ConnectionStringNoRunMode("GRSConnectionString");
            private string ars_filename = "ARSFILE";
            private Hashtable htARS = new Hashtable();
            #endregion

            public irsARSFunction()
            {                
             
            }

            #region PUBLIC METHODS
           
            public void create_ars_extractfile()
            {
                
                try
                {
                    irsFileExtracter.irsARSFunction.logars("IRSW ARS FILE EXTRACTER STARTING EXTRACT " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    this.build_ars_data();

                    try
                    {
                        if (this.htARS.Count == 0)
                        {
                            irsFileExtracter.irsARSFunction.logars("IRSW ARS FILE EXTRACTER NO DATA TO EXTRACT: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                            return;
                        }
                    }
                    catch
                    {
                        irsFileExtracter.irsARSFunction.logars("IRSW ARS FILE EXTRACTER NO DATA TO EXTRACT: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                        return;
                    }
                    irsFileExtracter.irsARSFunction.logars("IRSW ARS FILE EXTRACTER DONE FETCHING DATA NOW CREATING FILE: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    this.create_ars_textfile();

                    irsFileExtracter.irsARSFunction.logars("IRSW ARS FILE EXTRACTER DONE CREATING FILE NOW USING FTP: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());                    
                    this.ftp_ars_file();

                    irsFileExtracter.irsARSFunction.logars("IRSW ARS FILE EXTRACTER SUCCESSFULLY LOADED ARS FILE : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                }
                catch (Exception e)
                {

                    irsFileExtracter.irsARSFunction.logars(e.Message + ":" + e.StackTrace + " ");                    
                    
                }
                
            }
            public static void logars(string msg)
            {
                csFileIo io = new csFileIo();
                try
                {

                    StreamWriter sw;
                    string path = new irsConfiguration().appsetting("TempPath");
                    string subalias = new irsConfiguration().appsetting("IRSWSUBALIAS");
                    string str = DateTime.Now.ToString() + ": " + msg;
                    string dt = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0');
                    sw = File.AppendText(path + "\\" + "IRSWARSFE" + subalias + dt + ".log");
                    try
                    {
                        sw.WriteLine(str);
                        sw.Close();
                    }
                    catch { return; }
                    finally
                    {
                        str = null;
                        sw.Dispose();
                        sw = null;
                    }

                }
                catch { return; }
                finally { io = null; }
            }
            private string clean_ars_data(string s)
            {
                //return s.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\f", "").Replace("\v", "").Trim();
                return s;
            }
            #endregion

            #region PRIVATE METHOD
            private DataSet get_ars_data()
            {
                DataSet ds = new DataSet();
                SqlConnection cn = new SqlConnection(this.irsconn);
                SqlCommand cmd = new SqlCommand();
                try
                {
                    cmd.Connection = cn;
                    cmd.CommandText = "proc_get_irsworld_arsdata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                    if (cn.State == 0) { cn.Open(); }

                    ds = base.getDataSet(cmd);
                    if (cmd.Parameters["@v_errmsg"].Value.ToString().Length != 0)
                    {                     
                        return null;
                    }
                    return ds;
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsARSFunction.logars(e.Message + ":" + e.StackTrace);
                    return null;

                }
                finally
                {
                    cmd.Dispose();
                    cmd = null;
                    cn.Dispose();
                    cn = null;
                }
               
            }
            private void build_ars_data()
            {
                irsARSRecord rec;
                DataSet ds = new DataSet();
                try
                {
                    ds = this.get_ars_data();
                    if (ds == null)
                    {
                        return;
                    }
                    this.record_count = ds.Tables[0].Rows.Count;
                    int i = 0;
                    this.htARS.Add(i, (this.create_ars_header(this.record_count)));
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        i++;
                        rec = new irsARSRecord(r);
                        this.htARS.Add(i, (rec.create_ars_record()));
                        rec = null;
                    }
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsARSFunction.logars(e.Message + ":" + e.StackTrace);
                }
                finally
                {
                    if (ds != null)
                    {
                        ds.Dispose();
                        ds = null;
                    }
                    
                }
             
            }
            private void create_ars_textfile()
            {
                csFileIo io;
                string irswsubalias = new irsConfiguration().ConnectionStringNoRunMode("IRSWSUBALIAS");
                this.ars_filename = "ARS_" + irswsubalias + DateTime.Now.ToString("HHmmss") + DateTime.Now.ToString("MMddyyyy") + ".txt";
                string temparsfilename = this.temppath + this.ars_filename;
                try
                {
                    io = new csFileIo();

                    if (!io.directoryExist(this.temppath))
                    {
                        io.createDirectory(this.temppath);
                    }
                    if (!io.directoryExist(this.rootpath))
                    {
                        io.createDirectory(this.rootpath);
                    }

                    io.open_output(temppath + ars_filename, (int)csFileIo.write_modes.CREATE_NEW);
                    for (int i = 0; i < this.htARS.Count; i++)
                    {
                        io.putLine(this.clean_ars_data(this.htARS[i].ToString()));
                    }
                    
                    io.close_output();

                    io.copy(temppath + ars_filename, rootpath + ars_filename,true );
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsARSFunction.logars(e.Message + ":" + e.StackTrace);
                }
                finally
                {
                    io = null;
                }
            }
            private void ftp_ars_file()
            {
                string host = new irsConfiguration().appsetting("FTPIP");
                string username = new irsConfiguration().appsetting("FTPUSER");
                string password = new irsConfiguration().appsetting("FTPPASS");
                string port = new irsConfiguration().appsetting("FTPPORT");
                string arsftppath = new irsConfiguration().appsetting("FTPARSPATH");
                csFtp ftp;
                try
                {
                    ftp = new csFtp(host, Convert.ToInt32(port), username, password);
                    if (ftp.connect())
                    {
                        if (ftp.cwd(arsftppath))
                        {
                            ftp.put(this.rootpath + this.ars_filename, this.ars_filename);
                        }
                        else
                        {
                            throw new Exception("CANNOT CHANGE PATH");
                        }
                        ftp.disconnect();
                    }
                    else
                    {
                        throw new Exception("CANNOT CONNECT TO FTP");
                    }
                    
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsARSFunction.logars(e.Message + ":" + e.StackTrace);
                }
                finally
                {
                    ftp = null;
                }

            }
            private string create_ars_header(int rec_count)
            {
                string boddate = this.get_grs_bod();
                return "HDR" +
                        rec_count.ToString().PadLeft(Math.Abs(6), '0') +
                        "1" +                        
                        this.sysid_arsfe.PadRight(Math.Abs(26 - sysid_arsfe.Length), ' ') +
                        boddate +
                        boddate +
                        boddate + "A";
            }
            private string get_grs_bod()
            {
                DataSet ds = new DataSet();
                SqlConnection cn = new SqlConnection(this.grsconn );
                SqlCommand cmd = new SqlCommand();
                string sql = "select max(tran_date) boddate from tbl_grs_bodeod where flag_bod = 1 and flag_eod = 0";
                try
                {
                    cmd.Connection = cn;
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text ;                    
                    if (cn.State == 0) { cn.Open(); }

                    ds = base.getDataSet(cmd);

                    DateTime bod = new DateTime();

                    bod = Convert.ToDateTime(ds.Tables[0].Rows[0][0].ToString());

                    return bod.ToString("MMddyyyy");
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsARSFunction.logars(e.Message + ":" + e.StackTrace);

                }
                return "";

            }
            #endregion

            #region irs ARS Record
            class irsARSRecord 
            {
                #region ARS FILE FORMAT
                /*
            ARSAgent = Right(strSpace & .Item("RemBankCode").trim, 3)
            DateSent = Right(strSpace & .Item("ClearTranxDate"), 8)
            ARSReference = Right(strSpace & .Item("Reference_ARS"), 12)
            'GRSReference = Right(strSpace & .Item("Reference_Theirs"), 5)
            Bene_FName = Left(.Item("BeneFName") & strSpace, 14)
            Bene_LName = Left(.Item("BeneLName") & strSpace, 14)
            Bene_MInit = Left(.Item("BeneMName") & strSpace, 1)
            Bene_Address1 = Left(.Item("Bene_Address1").trim & strSpace, 25)
            Bene_Address2 = Left(.Item("Bene_Address2").trim & strSpace, 25)
            Bene_Address3 = Left(.Item("Bene_Address3").trim & strSpace, 25)
            PayType = Right(strSpace & .Item("PayType"), 1)
            PNBBranch = Right(strSpace & .Item("PNBBranchCode"), 3)
            Paymode = Right(strSpace & .Item("Pay_Mode"), 1)
            Acct_Type = Right(strSpace & .Item("Acct_Type"), 1)
            Acct_Num = Left(.Item("Acct_Num") & strSpace, 20)
            Acct_Curr = Right(strSpace & .Item("Acct_Curr"), 2)
            PDDTS_PaybankName = Left(.Item("PDDTS_CD") & " " & .Item("PayBank_Name") & strSpace, 35)
            Imp_Cd = Right(strSpace & .Item("Imp_Code"), 1)
            Remitter_Name = Left(.Item("RemitterName") & strSpace, 29)
            Remitter_Type = Right(strSpace & .Item("RemitterType"), 1)
            PTT_Allot = Left(.Item("Allot") & strSpace, 30)
            PTT = Left(.Item("Rec_PTT") & strSpace, 4)
            RemCurr = Right(strSpace & .Item("RemittingCurr"), 2)
            CoveringBank = Right(strSpace & .Item("Cov_Bank"), 2)
            Amount_sent = Right(strSpace & Replace(.Item("strRemit_Amount").trim, ".", ""), 12)
            Gross_Amount = Right(strSpace & Replace(.Item("strGross_Amount").trim, ".", ""), 13)
            BeneDeductInd = Right(strSpace & .Item("BeneDeductInd"), 1)*/
                #endregion
                #region VARIABLES
                //ITG Password Encryption
                //private string irsconn = new irsConfiguration().appsetting("IRSConnectionString");
                private string irsconn = new irsConfiguration().ConnectionStringNoRunMode("IRSConnectionString");
                String ARSAgent;            //  3
                String RemBankCode;         //  3
                String DateSent;            //  8
                String ARSReference;        //  12
                String Bene_FName;          //  14
                String Bene_LName;          //  14         
                String Bene_MInit;          //  1
                String Bene_Address1;       //  25
                String Bene_Address2;       //  25
                String Bene_Address3;       //  25
                String PayType;             //  1
                String PNBBranch;           //  3
                String Paymode;             //  1
                String Acct_Type;           //  1
                String Acct_Num;            //  20
                String Acct_Curr;           //  2
                String PDDTS_PaybankName;   //  35
                String Imp_Cd;              //  1
                String Remitter_Name;       //  29
                String Remitter_Type;       //  1
                String PTT_Allot;           //  30
                String PTT;                 //  4
                String RemCurr;             //  2
                String CoveringBank;        //  2
                String Amount_sent;         //  12
                String Gross_Amount;        //  13
                String BeneDeductInd;       //  1 
                String BeneFullAddress;     //  250 
                String RemitterAddress;       //  250 
                String RemitterAccount;       //  30 
                

                DataRow arsdr = null;
                #endregion
                #region CONSTRUCTOR
                public irsARSRecord(DataRow _dr)
                {
                    this.arsdr = _dr;
                }
                #endregion
                #region PUBLIC METHOD
                public string create_ars_record()
                {
                    RemBankCode  = this.arsdr["RemBankCode"].ToString().Trim();
                    ARSAgent = this.arsdr["AgentCode"].ToString().Trim();
                    DateSent = Convert.ToDateTime(this.arsdr["ClearTranxDate"].ToString()).ToString("yyyyMMdd");
                    ARSReference = this.arsdr["Reference_ARS"].ToString();
                    Bene_FName = this.arsdr["BeneFName"].ToString();
                    Bene_LName = this.arsdr["BeneLName"].ToString();
                    Bene_MInit = this.arsdr["BeneMName"].ToString();
                    Bene_Address1 = this.arsdr["Bene_Address1"].ToString().Trim();
                    Bene_Address2 = this.arsdr["Bene_Address2"].ToString().Trim();
                    Bene_Address3 = this.arsdr["Bene_Address3"].ToString().Trim();
                    PayType = this.arsdr["PayType"].ToString().Trim();
                    PNBBranch = this.arsdr["PNBBranchCode"].ToString().Trim();
                    Paymode = this.arsdr["Pay_Mode"].ToString();
                    Acct_Type = this.arsdr["Acct_Type"].ToString().Trim();
                    Acct_Num = this.arsdr["Acct_Num"].ToString();
                    Acct_Curr = this.arsdr["Acct_Curr"].ToString();
                    PDDTS_PaybankName = this.arsdr["PDDTS_CD"].ToString() + " " + this.arsdr["PayBank_Name"].ToString();
                    Imp_Cd = this.arsdr["Imp_Code"].ToString();
                    Remitter_Name = this.arsdr["RemitterName"].ToString();
                    Remitter_Type = this.arsdr["RemitterType"].ToString();
                    PTT_Allot = this.arsdr["Allot"].ToString();
                    PTT = this.arsdr["Rec_PTT"].ToString();
                    RemCurr = this.arsdr["RemittingCurr"].ToString();
                    CoveringBank = this.arsdr["Cov_Bank"].ToString();
                    Amount_sent = this.arsdr["strRemit_Amount"].ToString().Trim();
                    Gross_Amount = this.arsdr["strGross_Amount"].ToString().Trim();
                    BeneDeductInd = this.arsdr["BeneDeductInd"].ToString();
                    BeneFullAddress = this.arsdr["BeneAddress"].ToString();
                    RemitterAddress = this.arsdr["RemitterAddress"].ToString();
                    RemitterAccount = this.arsdr["RemitterAccount"].ToString();

                    string s = "";
                   
                    s += RemBankCode.PadRight(3, ' ').Substring(0, 3);
                    s += DateSent.PadRight(8, ' ').Substring(0, 8);
                    s += ARSReference.PadRight(12, ' ').Substring(0, 12);
                    s += Bene_FName.PadRight(80, ' ').Substring(0, 80);
                    s += Bene_LName.PadRight(80, ' ').Substring(0, 80);
                    s += Bene_MInit.PadRight(2, ' ').Substring(0, 2);
                    s += Bene_Address1.PadRight(50, ' ').Substring(0, 50);
                    s += Bene_Address2.PadRight(50, ' ').Substring(0, 50);
                    s += Bene_Address3.PadRight(50, ' ').Substring(0, 50);
                    s += PayType.PadRight(1, ' ').Substring(0, 1);
                    s += PNBBranch.PadRight(3, ' ').Substring(0, 3);
                    s += Paymode.PadRight(1, ' ').Substring(0, 1);
                    s += Acct_Type.PadRight(1, ' ').Substring(0, 1);
                    s += Acct_Num.PadRight(34, ' ').Substring(0, 34);
                    s += this.get_erps_currency(Acct_Curr);
                    s += PDDTS_PaybankName.PadRight(35, ' ').Substring(0, 35);
                    s += Imp_Cd.PadRight(1, ' ').Substring(0, 1);
                    s += Remitter_Name.PadRight(29, ' ').Substring(0, 29);
                    s += Remitter_Type.PadRight(1, ' ').Substring(0, 1);
                    s += PTT_Allot.PadRight(150, ' ').Substring(0, 150);                    
                    s += this.get_erps_currency(RemCurr );
                    s += CoveringBank.PadRight(2, ' ').Substring(0, 2);
                    s += Amount_sent.PadLeft(12, ' ').Substring(0, 12);
                    s += Gross_Amount.PadLeft(13, ' ').Substring(0, 13);
                    s += ARSAgent.PadRight(4, ' ').Substring(0, 4);
                    s += BeneDeductInd.PadRight(1, ' ').Substring(0, 1);
                    s += BeneFullAddress.PadRight(250, ' ').Substring(0, 250);
                    s += RemitterAddress.PadRight(250, ' ').Substring(0, 250);
                    s += RemitterAccount.PadRight(30, ' ').Substring(0, 30);


                    return s;
                }
                #endregion
                private string get_erps_currency(string curr)
                {
                    DataSet ds = new DataSet();
                    irsSqlCommons com = new irsSqlCommons();
                    SqlConnection cn = new SqlConnection(this.irsconn );
                    SqlCommand cmd = new SqlCommand();
                    string sql = "select top 1 fldcode from tbl_lookup where fldgroup = 'erpscurrency' and fldvalue ='" + curr + "'";
                    try
                    {
                        cmd.Connection = cn;
                        cmd.CommandText = sql;
                        cmd.CommandType = CommandType.Text;
                        if (cn.State == 0) { cn.Open(); }

                        ds = com.getDataSet(cmd);

                        return ds.Tables[0].Rows[0][0].ToString().Substring(0, 2);
                    }
                    catch (Exception e)
                    {
                        irsFileExtracter.irsARSFunction.logars(e.Message + ":" + e.StackTrace);
                        return "";

                    }
                    finally
                    {
                        com = null;
                    }
  
                }

                private void get_covering_branchcode(string _coveringbankcode)
                {

                }
            }
            #endregion

       
        }            
        #endregion

        #region PYRL CLASS
        public class irsPYRLFunction : irsSqlCommons
        {
            private Hashtable pyrl_file_contents;

            private string temppath = new irsConfiguration().appsetting("TempRootPath");
            private string rootpath = new irsConfiguration().appsetting("RootPath");
            //ITG Password encryption
            //private string irswconn = new irsConfiguration().appsetting("IRSConnectionString");
            private string irswconn = new irsConfiguration().ConnectionStringNoRunMode("IRSConnectionString");

            private string pyrl_irsw_filename = "";
            public string errmsg = "";

            public String RecID = "";
            public String PayDate = "";
            public String RemName = "";
            public String BeneName = "";
            public String RemCurr = "";
            public String RemAmount1 = "";
            public String RemAmount2 = "";
            public String AccountNum = "";
            public String BeneCurr = "";
            public String BranchName = "";
            public String BankName = "";

            #region PUBLIC METHODS

            public void create_pyrl_extract_file()
            {
                try
                {
                    irsFileExtracter.irsIRSFunction.logme("IRSW PYRL EXTRACTER STARTING EXTRACT : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    this.get_irsworld_pyrl_data();
                    if (this.pyrl_file_contents.Count <= 0)
                    {
                        irsFileExtracter.irsIRSFunction.logme("IRSW PYRL EXTRACTER NO DATA TO EXTRACT: " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                        return;
                    }
                    irsFileExtracter.irsIRSFunction.logme("IRSW PYRL EXTRACTER DONE FETCHING DATA NOW CREATING FILE: " + System.DateTime.Now.ToShortDateString() + " ");
                    this.create_irs_file();
                    irsFileExtracter.irsIRSFunction.logme("IRSW PYRL EXTRACTER DONE CREATING FILE NOW DELETING TEMP" + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                    this.delete_temp();
                    irsFileExtracter.irsIRSFunction.logme("IRSW PYRL EXTRACTER SUCCESSFULLY LOADED IRS FILE : " + System.DateTime.Now.ToShortDateString() + " " + System.DateTime.Now.ToShortTimeString());
                }
                catch (Exception e)
                {
                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);
                }
            }
             
            public bool  format_pyrl_record(DataRow r)
            {
                try
                {
                    this.assign_values(r);

                    
                    return true;

                }
                catch(Exception e)
                {
                    this.errmsg = ":" + e.Message + " " + e.StackTrace;
                    irsFileExtracter.irsIRSFunction.logme(this.errmsg);
                }
                return false;
            }

            public string  pyrlformat_tostring()
            {
                string s = "";
               
                s += RecID.PadRight(4, ' ').ToString();
                s += PayDate.PadRight(8, ' ').ToString();
                s += RemName.PadRight(25, ' ').ToString();
                s += BeneName.PadRight(25, ' ').ToString();
                s += RemCurr.PadRight(3, ' ').ToString();
                s += RemAmount1.PadLeft(10, '0').ToString();
                s += RemAmount2.PadLeft(10, '0').ToString();
                s += AccountNum.PadRight(12, ' ').ToString();
                s += BeneCurr.PadRight(3, ' ').ToString();
                s += BranchName.PadRight(20, ' ').ToString();
                s += BankName.PadRight(23, ' ').ToString();


                return clean_data(s);
            }

            public void log_dtls(string msg)
            {
                try
                {
                    irsFileExtracter.irsIRSFunction.logme(msg);

                }

                finally { }
            }

            //public void MoveToGRSLoadingDir()
            //{
            //    csFileIo fio = new csFileIo();
            //    string ftpConString = new irsConfiguration().FTPConnectionString;
            //    csFtp ftp = new csFtp(ftpConString);
            //    string nl = Environment.NewLine;

            //    try
            //    {
            //        StringBuilder sb_file = new StringBuilder();

            //        if (ftp.connect())
            //        {

            //            if (ftp.binary(true))
            //            {
            //                string[] dir = this._loading_path.Replace(@"\", "/").Split('/');
            //                for (int i = 0; i < dir.Length; i++)
            //                {
            //                    if (dir[i] != "")
            //                    {
            //                        ftp.mkdir(dir[i]);
            //                        ftp.cwd(dir[i]);
            //                    }
            //                }

            //                if (ftp.put(this._temp_dest))
            //                {
            //                    if (!ftp.Message.Contains("550")) this._message = ftp.Message + " : ";
            //                    if (this._file_type_desc.ToUpper().Contains("COMPLIANCE"))
            //                        this._message += "Your file " + this._file_name + " is successfully uploaded.";
            //                    else this._message += "The file " + this._file_name + " will now be processed for remittance.";
            //                    this.writeAudit("FTP Successful - " + this._message, csAuditLog.Category.FTP);
            //                }
            //                else
            //                {
            //                    this._message = "ERROR: Cannot proceed to your request. " + ftp.Message;
            //                }
            //            }
            //            else
            //            {
            //                this._message = "ERROR: CANNOT SET BINARY MODE. " + ftp.Message;
            //            }
            //        }
            //        else
            //        {
            //            this._message = "ERROR: Ftp failed - " + ftp.Message;
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        this._message = e.Message;
            //        throw new Exception(e.Message);
            //    }
            //    finally
            //    {
            //        if (this._message.Contains("ERROR"))
            //        {
            //            this.writeAudit("FTP ERROR: " + this._message, csAuditLog.Category.FTP);
            //        }
            //        ftp.disconnect();
            //        ftp = null;
            //        this.deleteTempFile();
            //        this.uploadMailer();
            //    }
            //}
            #endregion

            #region PRIVATE METHODS

            private void get_irsworld_pyrl_data()
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();

                SqlConnection cn = new SqlConnection(this.irswconn);
                SqlCommand cmd = new SqlCommand();
                try
                {
                    this.pyrl_file_contents = new Hashtable(); ;
                    cmd.Connection = cn;
                    cmd.CommandText = "proc_get_irsworld_pyrl_data";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;


                    if (cn.State == 0) { cn.Open(); }

                    ds = base.getDataSet(cmd);
                    try
                    {
                        if (cmd.Parameters["@v_errmsg"].Value.ToString().Length == 0)
                        {
                            dt = ds.Tables[0];
                            this.pyrl_file_contents = new Hashtable();
                            int i = 0;
                            foreach (DataRow r in dt.Rows)
                            {
                                if (r[1].ToString().Length > 0)
                                {
                                    bool p = this.format_pyrl_record(r);
                                    if (p) { this.pyrl_file_contents.Add(i, this.pyrlformat_tostring()); }
                                    i++;
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);
                    }


                }
                catch (Exception e)
                {

                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);
                }
            }
            
            private bool assign_values(DataRow drIRSW)
            {
                this.RecID = drIRSW["Record_ID"].ToString();
                this.PayDate = drIRSW["Pay_Date"].ToString();
                this.RemName = drIRSW["Rem_Name"].ToString();
                this.BeneName = drIRSW["Bene_Name"].ToString();
                this.RemCurr = drIRSW["Remitting_Currency"].ToString();
                this.RemAmount1 = drIRSW["Rem_Amount1"].ToString();
                this.RemAmount2 = drIRSW["Rem_Amount2"].ToString();
                this.AccountNum = drIRSW["Account_Number"].ToString();
                this.BeneCurr = drIRSW["Bene_Currency"].ToString();
                this.BranchName = drIRSW["PNB_Branch_Name"].ToString();
                this.BankName = drIRSW["Bank_Name"].ToString();

                return true;
            }

            private string padSpaces(string s, int maxlength, bool isright)
            {
                int origlength = s.Length;
                string newstr = "";
                if (isright)
                {

                    newstr = s + s.PadRight(maxlength - origlength, ' ');
                    return newstr.Substring(0, maxlength);
                }
                newstr = s + s.PadLeft(maxlength - origlength, ' ');
                return newstr.Substring(0, maxlength);
            }

            private string clean_data(string s)
            {
                return s.Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace("\f", "").Replace("\v", "").Trim();
            }

            private void delete_temp()
            {
                try
                {
                    new csFileIo().delete(temppath + this.pyrl_irsw_filename);
                }
                catch (Exception ex)
                {
                    irsFileExtracter.irsIRSFunction.logme("Temp delete: " + ex.Message + ":" + ex.StackTrace);
                }
            }
 
            private void create_irs_file()
            {
                csFileIo io;

                try
                {
                    io = new csFileIo();


                    this.pyrl_irsw_filename = "PYRL_GRSDATA_" + System.DateTime.Now.ToString("yyyyMMddhhmmss") + "_IRSW.txt";
                    string irs_tempfilename = temppath + this.pyrl_irsw_filename;

                    if (!io.directoryExist(temppath))
                    {
                        io.createDirectory(temppath);
                    }

                    if (io.exist(irs_tempfilename)) { io.delete(irs_tempfilename); }

                    io.open_output(irs_tempfilename, (int)csFileIo.write_modes.CREATE_NEW);

                    for (int i = 0; i < this.pyrl_file_contents.Count; i++)
                    {

                        string s = (string)this.pyrl_file_contents[i];
                        io.putLine(this.clean_data(s));
                    }
                    //io.putLine("\n");
                    io.close_output();



                    io.copy(irs_tempfilename, rootpath + this.pyrl_irsw_filename, true);

                }
                catch (Exception e)
                {
                    irsFileExtracter.irsIRSFunction.logme(e.Message + ":" + e.StackTrace);
                }
                finally
                {
                    io = null;
                }
            }

            #endregion
        }

        #endregion
    }
}
