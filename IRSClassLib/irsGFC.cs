using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Data.SqlTypes;

namespace IRSClassLib
{
    [Serializable]
    public class irsAccountAlgo
    {
        #region PRIVATE VARIABLE
        private String _message = "";
        private String _AlgoType = "";
        #endregion

        #region PROPERTY GET
        public String message
        {
            get { return _message; }
        }
        public String AlgoType
        {
            get { return _AlgoType; }
        }
        #endregion

        #region PUBLIC METHODS
        public bool checkAccount(String sno)
        {
            sno = sno.Trim();
            try
            {
                /* PNBS Integration Project */
                _AlgoType = "SI";
                return true;
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                _AlgoType = "Invalid";
                return false;
            }
            finally
            {
                new irsAccountInquiry().update_account_remarks(sno, _AlgoType);
            }
        }
        #endregion
    }

    [Serializable]
    public class irsLinkToGFC : sqlHelperWs
    {
        #region PRIVATE VARIABLE
        private String _message = "";
        private DateTime _trxndate;
        private Object[] _oGFC_details = new Object[8];
        private String _ExportType = "PDF";
        private bool _AutoPrint = false;
        private bool _isFileLoader = false;
        private bool _isWinApp = false;
        #endregion

        #region PROPERTY GET SET
        public String message
        {
            get { return _message; }
        }
        public DateTime transaction_date
        {
            get { return _trxndate; }
        }
        public Object[] oGFCDetails
        {
            get { return _oGFC_details; }
        }
        public String Report_Export_Type
        {
            set { _ExportType = value; }
        }
        public bool Report_Is_AutoPrint
        {
            set { _AutoPrint = value; }
        }
        public bool isFileLoader
        {
            set { _isFileLoader = value; }
        }
        public bool isWinApp
        {
            set { _isWinApp = value; }
            get { return _isWinApp; }
        }
        #endregion

        #region PUBLIC METHODS
        /* FOR CBIP */
        public DataTable getCIFInformation(string CIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("gsp_getCIFInformation", new Object[] { CIFID });
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

        /* FOR CBIP */
        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (IsAdd)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                selectedRow = 0;
            }
        }

        /* FOR CBIP */
        public bool UpdateCIFRMNumber(string CIFID, string RMNumber)
        {
            bool ok = false;
            base.addMode = false;
            this.setBase("SELECT TOP 1 * FROM tbl_CIF_Master WHERE CIFID = '" + CIFID + "'", false);
            setColumn("RMNumber", RMNumber);
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            return ok;
        }

        /* FOR CBIP */
        public String GetCKAccount(string CIFID, string CardNo, string IDNo, string Currency)
        {
            _message = "";

            try
            {
                String userId = get_irsclient_data("UID");
                String branchCode = get_irsclient_data("LOCATION");

                DataTable dt = getCIFInformation(CIFID);
                string IDType = ConfigurationManager.AppSettings["GFC_IDTYPE"].ToString();
                string[] arr = null;
                string ret = string.Empty;
                string retval = string.Empty;
                string RMNumber = string.Empty;
                string contactNo = string.Empty;
                string mobileNo = string.Empty;
                int i = 0;

                if (dt != null && dt.Rows.Count > 0)
                {
                    IDNo = dt.Rows[0]["IDNo"].ToString();
                    IDType = dt.Rows[0]["IDType"].ToString();
                    RMNumber = dt.Rows[0]["RMNumber"].ToString();

                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        if (contactNo.Trim() == string.Empty)
                        {
                            if (dt.Rows[i]["TelephoneNo"] != DBNull.Value)
                            {
                                contactNo = dt.Rows[i]["TelephoneNo"].ToString().Trim();
                            }
                        }
                        if (mobileNo.Trim() == string.Empty)
                        {
                            if (dt.Rows[i]["MobileNo"] != DBNull.Value)
                            {
                                mobileNo = dt.Rows[i]["MobileNo"].ToString().Trim();
                            }
                        }
                        if (contactNo.Trim() != string.Empty && mobileNo.Trim() != string.Empty)
                        {
                            break;
                        }
                    }

                    //GFC_SI_WSDL_PROXY.GFC_WS gfc_si_wsdl = new GFC_SI_WSDL_PROXY.GFC_WS();
                    //gfc_si_wsdl.Url = "http://pnbatm16.pnbcorpsit.ph/GFC_WS_Proxy/GFC_WS.asmx";
                    GFC_SI_WSDL.GFC_WebService gfc_si_wsdl = new IRSClassLib.GFC_SI_WSDL.GFC_WebService();
                    gfc_si_wsdl.Url = ConfigurationManager.AppSettings["GFC_SI_WSDL"];
                    ret = gfc_si_wsdl.PurchaseGFC(ConfigurationManager.AppSettings["GFC_USERID"].ToString(),
                    ConfigurationManager.AppSettings["GFC_BRANCHCODE"].ToString() + branchCode, string.Empty,
                    dt.Rows[0]["LastName"].ToString(),
                    dt.Rows[0]["FirstName"].ToString(),
                    dt.Rows[0]["MiddleName"].ToString(),
                    Convert.ToDateTime(dt.Rows[0]["BirthDate"].ToString()).ToString("yyyy-MM-dd"),
                    dt.Rows[0]["Country"].ToString(),
                    dt.Rows[0]["Nationality"].ToString(),
                    dt.Rows[0]["Address1"].ToString(),
                    dt.Rows[0]["Address2"].ToString(),
                    dt.Rows[0]["Address3"].ToString(),
                    dt.Rows[0]["City"].ToString(),
                    dt.Rows[0]["Province"].ToString(),
                    dt.Rows[0]["Region"].ToString(),
                    dt.Rows[0]["ZipCode"].ToString(),
                    contactNo,
                    dt.Rows[0]["Country"].ToString(),
                    mobileNo,
                    dt.Rows[0]["EmailAddress"].ToString(),
                    new irsInternetPage().getLookupValue("Currency", "FldValue", Currency, "FldCode"),
                    CardNo, IDNo, IDType, string.Empty, string.Empty,
                    dt.Rows[0]["Country"].ToString(), string.Empty,
                    RMNumber, CIFID, userId,
                    dt.Rows[0]["PhilSysNo"].ToString());
                }
                if (ret != string.Empty)
                {
                    new csAuditLog().writeAudit(userId, "GFC Purchase", "CIFID=[" + CIFID + "] CardNo=[" + CardNo + "] IDNo=[" + IDNo + "] IDType=[" + IDType + "] Currency=[" + Currency + "]", "ReturnValue=[" + ret + "]", "GetCKAccount", csAuditLog.Category.Select);
                    arr = ret.Split('|');
                    if (RMNumber == string.Empty)
                    {
                        RMNumber = arr.GetValue(1).ToString();
                        bool save = UpdateCIFRMNumber(CIFID, RMNumber);
                        if (!save)
                        {
                            new csAuditLog().writeAudit(userId, "GFC RM Number FAILED", "CIFID=[" + CIFID + "] RMNumber=[" + RMNumber + "]", "ReturnValue=[" + save.ToString() + "]", "GetCKAccount", csAuditLog.Category.Update);
                        }
                    }
                    if (arr.GetValue(0).ToString() == ConfigurationManager.AppSettings["GFC_VALID_RESPONSE"].ToString())
                    {
                        retval = arr.GetValue(2).ToString() + "~1";
                    }
                    else
                    {
                        retval = arr.GetValue(5).ToString();
                        _message = arr.GetValue(5).ToString();
                        throw new Exception(arr.GetValue(5).ToString());
                    }
                }
                return retval;
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return "";
            }
        }

        public String GetCKAcctType(String ck)
        {
            try
            {
                String ckDetails = get_gfc_ck_acct_details(ck);
                irsClientMaster icm = new irsClientMaster();
                icm.IsWinApp = base.IsWinApp;
                String AcctType = icm.get_lookup_fldvalue(ckDetails.Split('|')[2].ToString(), "ACCOUNTTYPE");
                return AcctType;
            }
            catch
            {
                return "";
            }
        }

        public String get_irsclient_data(String param)
        {
            string s = null;

            if (_isWinApp)
            {
                switch (param)
                {
                    case "UID": s = "WINAPP"; break;
                    case "CIF": s = "WINAPP"; break;
                    case "LOCATION": s = "WINAPP"; break;
                    case "IP": s = ""; break;
                }

                return s;
            }
            else if (irsClientData.UserId != null && irsClientData.UserId.ToString().Trim() != "")
            {
                switch (param)
                {
                    case "UID": s = irsClientData.UserId; break;
                    case "CIF": s = irsClientData.CIFID; break;
                    case "LOCATION": s = irsClientData.Location; break;
                    case "IP": s = irsClientData.IPAddress; break;
                }
                return s;
            }
            else
            {
                throw new Exception("SESSION HAS EXPIRED.\r\nPlease log off and try relogin again.");
            }
        }

        public irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }

                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        public bool UnlockCK(String ckAccount)
        {
            _message = "";
            try
            {
                Object[] plist = { ckAccount };

                if (execProcedureUpdate("sp_SetCK_Unlock", plist, true)) { return true; }
                else { throw new Exception(_message); }
            }
            catch (Exception ex)
            {
                //_message = "Failed to unlock ck account, Reason : " + ex.Message;
                _message = ex.Message;
                return false;
            }
        }

        public DateTime get_branch_localtime()
        {
            DateTime dt;
            try
            {
                dt = new irsTimeZone().branchLocalTime(get_irsclient_data("LOCATION"));
            }
            catch
            {
                dt = DateTime.Now;
            }

            return dt;
        }

        public bool SaveGFMCAccount(String CIFID, String CardNo, String AcctNo, int Curr, String IDPresented, String Mode, String AccountType)
        {
            /*
             * Saving Modes
             * 
             * link            = ck card linking
             * purchase    = Card Purchase
             * avail          = Availment
             * batch        = linking card to CIF
             * excard      = Card / CK saving to server
             */
            _message = "";
            try
            {
                writeGFCAudit(Mode, "Start " + Mode + " Transaction : CIFID=" + CIFID + "|CardNo=" + CardNo + "|AcctNo=" + AcctNo + "|Curr=" + Curr.ToString() + "|IDPresented=" + IDPresented + "|AcctType=" + AccountType, "", "", WinAppLogger.Category.Update);

                String[] ID = CIFID.Split('~');
                CIFID = ID[0];
                String RemCIFID = ID[1];

                String UserID = get_irsclient_data("UID");
                String BranchCode = get_irsclient_data("LOCATION");
                clearGFCDetails();
                // Activate in FC
                if (Mode == "link" | Mode == "purchase")
                {
                    // String prodCode = ""; // commented to remove warning
                    // String gfcBrnchCode = ""; // commented to remove warning

                    if (!checkIRSGFCNo(CardNo)) { throw new Exception(_message); }

                    //check Id presented if not blank
                    if (IDPresented.Trim() == String.Empty) { throw new Exception("Invalid ID Presented"); }
                }

                if (getAppSettings("GFCDOFC") == "true" && Mode != "excard")
                {
                    if (!GFCCardActivate(CardNo, AcctNo, CIFID, RemCIFID, BranchCode, IDPresented))
                    {
                        throw new Exception(_message);
                    }
                }

                DateTime dDate = get_branch_localtime();
                _trxndate = dDate;
                setGFCDetails("TrxnDate", dDate);

                Object[] plist =
                    {
                        CIFID
                        ,CardNo
                        ,AcctNo
                        ,Curr
                        ,BranchCode
                        ,UserID
                        ,IDPresented
                        ,dDate
                        ,(Mode == "excard") ? "purchase" : Mode
                        ,AccountType
                    };

                if (execProcedureUpdate("sp_save_GFCLink_Account", plist))
                {
                    writeGFCAudit(Mode, "End " + Mode + " Transaction : CIFID=" + CIFID + "|CardNo=" + CardNo + "|AcctNo=" + AcctNo + "|Curr=" + Curr.ToString() + "|IDPresented=" + IDPresented + "|AcctType=" + AccountType + "|Msg=Successful", "", "", WinAppLogger.Category.Update);
                    return true;
                }
                else
                {
                    throw new Exception(_message);
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                writeGFCAudit(Mode, "Error " + Mode + " Transaction : CIFID=" + CIFID + "|CardNo=" + CardNo + "|AcctNo=" + AcctNo + "|Curr=" + Curr.ToString() + "|IDPresented=" + IDPresented + "|AcctType=" + AccountType + "|ErrorMsg=" + ex.Message, "", "", WinAppLogger.Category.Update);
                return false;
            }
        }

        public string get_gfc_ck_acct_details(string acctno)
        {
            try
            {
                String acctStatus = new irsAccountInquiry().getAccountStatus(acctno);
                return acctStatus;
            }
            catch
            {
                return String.Empty;
            }
        }

        public bool GFCCardActivate(String cardNumber, String accountId, String BeneCIFID, String RemCIFID, String BranchCode, String IDPres)
        {
            return true;
        }

        private String getGFCBranchCode(String branchCode)
        {
            try
            {
                String sql = "select branchcode	=	isnull((case when BranchType = 2 then substring(BranchCode, 2, len(BranchCode) - 1 ) ";
                sql += "when BranchType = 3 then substring(NegBranchCode, 2, len(NegBranchCode) - 1 ) ";
                sql += "else BranchCode end),'') ";
                sql += "from tbl_branches ";
                sql += "where	BranchCode	=	'" + branchCode + "'";

                return getDataSet(sql).Tables[0].Rows[0][0].ToString();
            }
            catch
            {
                return "";
            }
        }
        
        public String setReport(String ReportName, String[] paramValues)
        {
            _message = "";
            try
            {
                String sql = "select Param = convert(varchar, Seq_ID) + '^' + ParameterNames + '^' + Parametertypes + '^' + ParameterDisplay + '^' + sp_Name + '^' + SubGroupDesc + '^' ";
                sql += ",ParameterNames ";
                sql += "from tbl_reports where SubGroupDesc = '" + ReportName + "'";

                DataSet ds = getDataSet(sql);
                String urlparam1 = ds.Tables[0].Rows[0]["Param"].ToString();
                String urlparam2 = "";
                //String exporttype = "PDF";

                String[] pname = ds.Tables[0].Rows[0]["ParameterNames"].ToString().Split('$');

                for (int i = 0; i < pname.Length - 1; i++)
                {
                    urlparam2 += "T" + pname[i] + ";" + paramValues[i] + "^";
                }

                //irsReports irsrep = new irsReports(urlparam1, urlparam2, exporttype);

                return "?mergeparam=" + urlparam1 + "&paramvalues=" + urlparam2 + "&exporttype=" + _ExportType + "&autoprint=" + ((_AutoPrint) ? "true" : "false");
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return "";
            }
        }

        public bool execProcedureUpdate(String proc, Object[] plist)
        {
            _message = "";
            try
            {
                if (_isWinApp) { base.IsWinApp = true; }
                if (base.execStoredProcedureUpdate(proc, plist) > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("No record updated");
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return false;
            }
        }

        public DataSet getDataSet(String sqlScript)
        {
            _message = "";
            try
            {
                if (_isWinApp) { base.IsWinApp = true; }
                SqlCommand sqcommands = base.getSqlCommand(sqlScript, false);
                DataSet ds = base.getDataSet(sqcommands, "TABLE");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return null;
            }
        }

        public DataSet execProcedure(String proc, Object[] plist)
        {
            _message = "";
            try
            {
                if (_isWinApp) { base.IsWinApp = true; }
                DataSet ds = base.execStoredProcedure(proc, plist);

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return null;
            }
        }

        public DataSet execProcedureWithOutput(String proc, Object[] plist, Object[] outplist, out Object[] result)
        {
            _message = "";
            int OutParamCount = 0;
            result = null;
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs("GFCConnectionString");
                if (_isWinApp) { com.IsWinApp = true; }

                SqlCommand cmd = com.getSqlCommand(proc);

                com.deriveParameters(cmd, plist);

                foreach (Object o in outplist)
                {
                    cmd.Parameters[o.ToString()].Direction = ParameterDirection.Output;
                    OutParamCount++;
                }
                
                DataSet ds = com.getDataSet(cmd);
                SqlCommand lcmd = com.getLastCommand(cmd.CommandText);
                Object[] results = new Object[OutParamCount];

                if (lcmd != null)
                {
                    for (int i = 0; i < outplist.Length; i++)
                    {
                        results[i] = lcmd.Parameters[outplist[i].ToString()].Value;
                    }
                }

                result = results;
                return ds;
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return null;
            }
        }

        public bool execProcedureUpdate(String proc, Object[] plist, bool isGFCLink)
        {
            if (!isGFCLink)
            {
                return execProcedureUpdate(proc, plist);
            }

            _message = "";
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs("GFCConnectionString");
                if (_isWinApp) { com.IsWinApp = true; }

                if (com.execStoredProcedureUpdate(proc, plist) > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("No record updated");
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return false;
            }
        }

        public DataSet getDataSet(String sqlScript, bool isGFCLink)
        {
            if (!isGFCLink)
            {
                return getDataSet(sqlScript);
            }

            _message = "";
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs("GFCConnectionString");
                if (_isWinApp) { com.IsWinApp = true; }

                SqlCommand sqcommands = com.getSqlCommand(sqlScript, false);
                DataSet ds = com.getDataSet(sqcommands, "TABLE");

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception("No dataset returned");
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return null;
            }
        }

        public bool checkIRSGFCNo(String CardNo)
        {
            _message = "";
            try
            {
                int result = CheckGFCNumber(CardNo);

                switch (result)
                {
                    case 1: return true;
                    case 2: _message = "GFC Number already exist! Please check with eGFMC system!"; return false;
                    case 0: _message = "GFC Number is Invalid"; return false;
                    case -1: throw new Exception(_message);
                    default: throw new Exception("Unknown result : " + _message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to check GFC Card Number, Reason : " + ex.Message);
            }
        }

        public string GetGFCCKNumber(string CardNo)
        {
            GFC_SI_WSDL.GFC_WebService gfc_si_wsdl = new IRSClassLib.GFC_SI_WSDL.GFC_WebService();
            string retCard = gfc_si_wsdl.GetAccountDetails(CardNo, ConfigurationManager.AppSettings["GFC_USERID"]);
            string cardNo = string.Empty;
            string[] arrCardNo = null;

            if (retCard != string.Empty && retCard != null)
            {
                //00|TUAZON|RONALD JAMES|L|09/16/1980|5921088040003873||CLAIMED|Successfully retrieve card details.
                arrCardNo = retCard.Split('|');
                if (arrCardNo.Length >= 5)
                {
                    cardNo = arrCardNo.GetValue(5).ToString();
                }
            }
            return cardNo;
        }

        public int CheckGFCNumber(String CardNo)
        {
            /* FOR CBIP */
            return 1;
            /*
            _message = "";
            try
            {
                Object[] Result = null;
                DataSet ds = execProcedureWithOutput("sp_IRSCheckGFMCNo", new Object[] { CardNo, 0 }, new Object[] { "@check" }, out Result);

                return int.Parse(Result[0].ToString());
            }
            catch
            {
                return -1;
            }
            */
        }

        public String get_GFC_Card_Number(String CKAcctNo, out String msg)
        {
            msg = "";
            try
            {
                //String sql = "SELECT TOP 1 isnull(GFMCCardNo,'') FROM Beneficiary WHERE CASANo = '" + CKAcctNo.Trim() + "'";
                if (CKAcctNo == null | CKAcctNo.Trim() == "")
                {
                    return "";
                }

                String sql = "SELECT TOP 1 isnull(rec_CardNo,'') FROM tbl_AcctandCardNo WHERE rec_CkAcctNo = '" + CKAcctNo.Trim() + "' ";
                sql += "and isnull(rec_Status,'') = 'CLAIMED'";

                writeGFCAudit("SELECT", "Retrieving CK : " + CKAcctNo.Trim() + " Card Number from GFC Server.", "", "", WinAppLogger.Category.Select);

                DataSet ds = getDataSet(sql, true);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    writeGFCAudit("SELECT", "Retrieving CK : " + CKAcctNo.Trim() + " Card Number from GFC Server. Returned : " + ds.Tables[0].Rows[0][0].ToString(), "", "", WinAppLogger.Category.Select);
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                msg = "Failed to Get CK : " + CKAcctNo.Trim() + " Card Number, Reason : " + ex.Message;
                writeGFCAudit("SELECT", "Failed to Get CK : " + CKAcctNo.Trim() + " Card Number, Reason : " + ex.Message, "", "", WinAppLogger.Category.Select);
                return "";
            }
        }

        /* FOR CBIP */
        public bool check_GFC_Aggregate(String CIFID, String CKAcct, int curr, double NetRemitBenAmount)
        {
            bool b = false;
            string retCard = string.Empty;
            string retAggregate = string.Empty;
            string cardNo = string.Empty;
            string err = string.Empty;

            //==================================================
            //RFC-22958 : Handling of Prepaid SA Max Balance
            string retPrepaidSA = string.Empty;
            string[] arrPrepaidSA = null;
            DataSet dsPrepaidSA = new DataSet();
            DataSet dsBranchDefaults = new DataSet();
            int PrepaidSAValidation = 0;
            int ForApprovalTag = 0;
            //==================================================

            try
            {
                //==================================================
                //RFC-22958 : Handling of Prepaid SA Max Balance
                dsPrepaidSA = execProcedure("isp_getLookup", new object[] { "PrepaidCASAValidation" });
                if (dsPrepaidSA != null)
                {
                    if (dsPrepaidSA.Tables[0].Rows.Count > 0)
                    {
                        PrepaidSAValidation = Convert.ToInt32(dsPrepaidSA.Tables[0].Rows[0]["FldCode"]);
                    }
                }
                //==================================================

                //==============================================================
                //RFC-24548 : Remittance Systems - ActiveMQ to E2E API Change
                dsBranchDefaults = execProcedure("get_branchdefaultsetup", new object[] { irsClientData.Location });
                if (dsBranchDefaults != null)
                {
                    if (dsBranchDefaults.Tables[0].Rows.Count > 0)
                    {
                        DataRow[] tag_row = dsBranchDefaults.Tables[0].Select("flddescription = 'For Approval Tag'");
                        if (tag_row.Length > 0)
                        {
                            ForApprovalTag = Convert.ToInt32(tag_row[0][2].ToString().Trim());
                        }
                        else
                        {
                            ForApprovalTag = 0;
                        }
                    }
                }
                //==================================================

                /* FOR CBIP */
                //GFC_SI_WSDL_PROXY.GFC_WS gfc_si_wsdl = new GFC_SI_WSDL_PROXY.GFC_WS();
                //gfc_si_wsdl.Url = "http://pnbatm16.pnbcorpsit.ph/GFC_WS_Proxy/GFC_WS.asmx";
                GFC_SI_WSDL.GFC_WebService gfc_si_wsdl = new IRSClassLib.GFC_SI_WSDL.GFC_WebService();
                gfc_si_wsdl.Url = ConfigurationManager.AppSettings["GFC_SI_WSDL"];

                string[] arrCardNo = null;
                string[] arrAggregate = null;

                retCard = gfc_si_wsdl.GetAccountDetails(CKAcct, ConfigurationManager.AppSettings["GFC_USERID"]);
                if (retCard != string.Empty && retCard != null)
                {
                    //00|TUAZON|RONALD JAMES|L|09/16/1980|5921088040003873||CLAIMED|Successfully retrieve card details.
                    arrCardNo = retCard.Split('|');
                    if (arrCardNo.GetValue(0).ToString() == ConfigurationManager.AppSettings["GFC_VALID_RESPONSE"].ToString())
                    {
                        if (arrCardNo.Length >= 5)
                        {
                            cardNo = arrCardNo.GetValue(5).ToString();
                        }
                    }
                }

                if (cardNo != string.Empty)
                {
                    try
                    {
                        string location = irsClientData.Location;
                        string userID = irsClientData.UserId;
                        retAggregate = gfc_si_wsdl.CheckAggregate(cardNo, CKAcct, "5" + location, NetRemitBenAmount.ToString(), ConfigurationManager.AppSettings["GFC_USERID"], userID);
                        if (retAggregate != string.Empty && retAggregate != null)
                        {
                            arrAggregate = retAggregate.Split('|');
                            if (arrAggregate.Length >= 5)
                            {
                                if (arrAggregate.GetValue(0).ToString() != ConfigurationManager.AppSettings["GFC_VALID_RESPONSE"].ToString())
                                {
                                    err = arrAggregate.GetValue(5).ToString();
                                }
                                else
                                {
                                    b = true;
                                }
                            }
                            writeGFCAudit("GFC AGGREGATE", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | CardNo=[" + cardNo + "] Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] Card=[" + retCard + "] Aggregate=[" + retAggregate + "]", "ReturnValue=[" + b.ToString() + "] Error=[" + err + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                        }
                    }
                    catch (Exception ex)
                    {
                        b = true;
                        writeGFCAudit("GFC AGGREGATE ERROR", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | CardNo=[" + cardNo + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] | Card=[" + retCard + "] | Aggregate=[" + retAggregate + "]", "ReturnValue=[" + b.ToString() + "] | Error=[" + err + " / " + ex.Message + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                    }
                }
                //==================================================
                //RFC-22958 : Handling of Prepaid SA Max Balance
                else if (cardNo == string.Empty && PrepaidSAValidation > 0 && ForApprovalTag == 0)
                {
                    try
                    {
                        string location = irsClientData.Location;
                        string userID = irsClientData.UserId;
                        retPrepaidSA = gfc_si_wsdl.CheckExceedLimit(CIFID, CKAcct, NetRemitBenAmount.ToString(), "5" + location);
                        if (retPrepaidSA != string.Empty && retPrepaidSA != null)
                        {
                            arrPrepaidSA = retPrepaidSA.Split('|');
                            if (arrPrepaidSA.Length >= 1)
                            {
                                if (arrPrepaidSA.GetValue(0).ToString() != ConfigurationManager.AppSettings["GFC_VALID_RESPONSE"].ToString())
                                {
                                    err = arrPrepaidSA.GetValue(1).ToString();
                                }
                                else
                                {
                                    b = true;
                                }
                            }
                            writeGFCAudit("PREPAID SAV AGGREGATE", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] Aggregate=[" + retPrepaidSA + "]", "ReturnValue=[" + b.ToString() + "] Error=[" + err + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                        }
                        else
                        {
                            writeGFCAudit("PREPAID SAV AGGREGATE NO RESPONSE", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "]", "CheckExceedLimit=[" + retPrepaidSA + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                        }
                    }
                    catch (Exception ex)
                    {
                        b = true;
                        writeGFCAudit("PREPAID SAV ERROR", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] Aggregate=[" + retPrepaidSA + "]", "ReturnValue=[" + arrPrepaidSA.GetValue(0).ToString() + "] | Error=[" + err + " / " + ex.Message + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                    }
                }
                //==================================================
                else
                {
                    b = true;
                    writeGFCAudit("GFC AGGREGATE", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | CardNo=[" + cardNo + "] Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] Card=[" + retCard + "] Aggregate=[" + retAggregate + "]", "ReturnValue=[" + b.ToString() + "] Error=[" + err + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                }
                //writeGFCAudit("GFC AGGREGATE", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | CardNo=[" + cardNo + "] Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] Card=[" + retCard + "] Aggregate=[" + retAggregate + "]", "ReturnValue=[" + b.ToString() + "] Error=[" + err + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
            }
            catch (Exception ex)
            {
                /* FOR CBIP 20170825 */
                b = true;
                writeGFCAudit("GFC AGGREGATE / PREPAID SAV ERROR", "CIFID=[" + CIFID + "] | AccountNo=[" + CKAcct + "] | CardNo=[" + cardNo + "] Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] Card=[" + retCard + "] Aggregate=[" + retAggregate + "]", "ReturnValue=[" + b.ToString() + "] Error=[" + err + " / " + ex.Message + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
            }
            return b;
        }

        /* FOR CBIP */
        public DataSet getExistingGFC(string CardNo)
        {
            GFC_SI_WSDL.GFC_WebService gfc_si_wsdl = new IRSClassLib.GFC_SI_WSDL.GFC_WebService();
            gfc_si_wsdl.Url = ConfigurationManager.AppSettings["GFC_SI_WSDL"];
            string ret = gfc_si_wsdl.GetCardDetails(CardNo, ConfigurationManager.AppSettings["GFC_USERID"]);
            string[] arr = null;
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataRow dr;
            string currency = string.Empty;
            string[] arrCurrency = null;

            dt.Columns.Add("AccountNo");
            dt.Columns.Add("AccountType");
            dt.Columns.Add("CardNo");
            dt.Columns.Add("Currency");
            dt.Columns.Add("Status");
            dt.Columns.Add("CardType");
            dt.Columns.Add("IDPresented");

            /* CBIP BOYET 10-03-2017 */
            dt.Columns.Add("rec_CkAcctNo");
            dt.Columns.Add("CKCurrCode");
            dt.Columns.Add("rec_CardNo");
            dt.Columns.Add("CIFName");
            /* CBIP BOYET 10-03-2017 */

            if (ret != string.Empty)
            {
                //00|DE LABINGSIYAM|JOSEFITATA|VELARDESA|09/07/1950|0101705800000921|0020009278|CLAIMED|Successfully retrieve card details.
                arr = ret.Split('|');
                if (arr.GetValue(0).ToString() == ConfigurationManager.AppSettings["GFC_VALID_RESPONSE"].ToString())
                {
                    currency = new irsAccountInquiry().getAccountStatus(arr.GetValue(5).ToString());
                    arrCurrency = currency.Split('|');

                    /* CBIP BOYET 10-03-2017 */
                    if (arrCurrency.Length == 3)
                    {
                        if (arrCurrency[2] != null && arrCurrency[2].ToLower().IndexOf("failed to get account details") >= 0)
                        {
                            throw new Exception(arrCurrency[2].ToString());
                        }
                        if (arrCurrency[2] != null && arrCurrency[2].ToLower().IndexOf("account number is invalid. please check the number and try again") >= 0)
                        {
                            throw new Exception(arrCurrency[2].ToString());
                        }
                    }
                    /* CBIP BOYET 10-03-2017 */

                    if (arrCurrency.Length < 3)
                    {
                        return null;
                    }
                    else
                    {
                        dr = dt.NewRow();
                        dr[0] = arr.GetValue(5).ToString();
                        dr[1] = arrCurrency.GetValue(5).ToString();
                        dr[2] = CardNo;
                        dr[3] = arrCurrency.GetValue(3).ToString();
                        dr[4] = arr.GetValue(7).ToString();
                        dr[5] = string.Empty;
                        dr[6] = string.Empty;

                        /* CBIP BOYET 10-03-2017 */
                        dr[7] = arr.GetValue(5).ToString();
                        dr[8] = arrCurrency.GetValue(3).ToString();
                        dr[9] = CardNo;

                        if (arr != null && arr.Length > 0 && arr[0] == "00")
                        {
                            dr[10] = arr[1].ToString() + " " + arr[2].ToString() + " " + arr[3].ToString();
                        }
                        else
                        {
                            dr[10] = "";
                        }
                        /* CBIP BOYET 10-03-2017 */

                        dt.Rows.Add(dr);
                        dt.TableName = "dt_2"; /* CBIP BOYET 10-03-2017 ADD TABLE - NET FRAMEWORK 4 WILL REQUIRE UNIQUE TABLE NAME WHEN ATTACHED TO DATASET WITH EXISTING TABLE */
                        ds.Tables.Add(dt);
                    }
                }
            }
            return ds;
        }

        public void writeGFCAudit(String GFCMode, String Description, String NewValue, String OldValue, WinAppLogger.Category Category)
        {
            String userId = get_irsclient_data("UID");
            String sDescription = "Location=" + get_irsclient_data("LOCATION") + " : " + Description;
            if (_isWinApp)
            {
                new WinAppLogger().writeAudit(userId, "GFC " + GFCMode, sDescription, NewValue, OldValue, Category);
            }
            else
            {
                new csAuditLog().writeAudit(userId, "GFC " + GFCMode, sDescription, NewValue, OldValue, (csAuditLog.Category)Category);
            }
        }

        public bool validateGFC(String Mode, String CardNo, String CKAcct, out String msg)
        {
            msg = string.Empty;
            return true;
        }

        public bool checkIfValidGFCCk(String AcctNumber)
        {
            try
            {
                String SQL = "SELECT count(1) FROM tbl_CkAccountNos WHERE rec_ckAccount = '" + AcctNumber.Trim() + "'";

                DataSet ds = getDataSet(SQL, true);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return false;
            }
            catch //(Exception ex) // commented to remove warning
            {
                return false;
            }
        }

        public DataSet getGFCCardDetails(String CardNumber)
        {
            try
            {
                String SQL = "select * from tbl_CardNo where rec_CardNo = '" + CardNumber.Trim() + "'";

                DataSet ds = getDataSet(SQL, true);

                if (ds.Tables.Count > 0)
                {
                    return ds;
                }

                return null;
            }
            catch //(Exception ex) // commented to remove warning
            {
                return null;
            }
        }

        //CJ RFC-25631 - SSS Pension Exception Validation
        public String GetRapidRemitDetails(string CIFID, string AccNo, double NetRemitBenAmount)
        {
            try
            {
                DataSet dsPrepaidSA = null;
                int PrepaidSAValidation = 0;
                dsPrepaidSA = execProcedure("isp_getLookup", new object[] { "SSSPensionValidationToggle" });
                if (dsPrepaidSA != null)
                {
                    if (dsPrepaidSA.Tables[0].Rows.Count > 0)
                    {
                        PrepaidSAValidation = Convert.ToInt32(dsPrepaidSA.Tables[0].Rows[0]["FldCode"]);
                    }
                }

                if (PrepaidSAValidation > 0 )
                {
                    GFC_SI_WSDL.GFC_WebService gfc_si_wsdl = new IRSClassLib.GFC_SI_WSDL.GFC_WebService();
                    gfc_si_wsdl.Url = ConfigurationManager.AppSettings["GFC_SI_WSDL"];
                    DataSet ds = new DataSet();
                    string retRapidRemitDetails = null;
                    string location = irsClientData.Location;
                    string userID = irsClientData.UserId;

                    retRapidRemitDetails = gfc_si_wsdl.CheckExceedLimit(CIFID, AccNo, NetRemitBenAmount.ToString(), "5" + location);
                    if (retRapidRemitDetails != string.Empty && retRapidRemitDetails != null)
                    {
                        if (retRapidRemitDetails.Length >= 1)
                        {
                            retRapidRemitDetails = retRapidRemitDetails.Replace('=', '|');
                            writeGFCAudit("SELECT", "CIFID=[" + CIFID + "] | AccountNo=[" + AccNo + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "]", "ReturnValue=[" + retRapidRemitDetails[0].ToString() + "]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                            return retRapidRemitDetails;
                        }
                        this.writeGFCAudit("SELECT", "CIFID=[" + CIFID + "] | AccountNo=[" + AccNo + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] ReturnValue=[" + retRapidRemitDetails.ToString(), "CheckExceedLimit=[SSS Pension Inquiry]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                        return null;
                    }
                    else
                    {
                        this.writeGFCAudit("SELECT", "CIFID=[" + CIFID + "] | AccountNo=[" + AccNo + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] ReturnValue=[" + retRapidRemitDetails.ToString(), "CheckExceedLimit=[SSS Pension Inquiry]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                        return null;
                    }
                }
                return "00";
            }
            catch (Exception ex)
            {
                this.writeGFCAudit("SELECT", "CIFID=[" + CIFID + "] | AccountNo=[" + AccNo + "] | Amount=[" + NetRemitBenAmount.ToString("###,###,#00.0000") + "] Error=["+ ex.Message.ToString() +"]", "CheckExceedLimit=[SSS Pension Inquiry]", "check_GFC_Aggregate", WinAppLogger.Category.GFC);
                return null;
            }
        }
        #endregion
        #region PRIVATE METHODS
        private String getFLEXErrorMsg(String Message)
        {
            int x; int y;
            try
            {

                Message = Message.ToUpper();
                x = Message.Trim().IndexOf("ERROR:");
                y = Message.Trim().IndexOf("REASON:");

                if (y != -1)
                {
                    return Message.Trim().Substring(x + 6, y - 8).Trim();
                }
                else
                {
                    return Message.Trim();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void clearGFCDetails()
        {
            for (int i = 0; i < oGFCDetails.Length; i++)
            {
                oGFCDetails[i] = null;
            }
        }
        private void setGFCDetails(String Key, Object Value)
        {
            switch (Key)
            {
                case "SEQ_ID": oGFCDetails[0] = Value; break;
                case "CardNo": oGFCDetails[1] = Value; break;
                case "CurrCode": oGFCDetails[2] = Value; break;
                case "CardBrCode": oGFCDetails[3] = Value; break;
                case "CardProdCode": oGFCDetails[4] = Value; break;
                case "CardType": oGFCDetails[5] = Value; break;
                case "CardTypeDesc": oGFCDetails[6] = Value; break;
                case "TrxnDate": oGFCDetails[7] = Value; break;
            }
        }
        public String getCurrShortName(int currValueCode)
        {
            String sql = "select fldcode from tbl_lookup where fldgroup = 'currency' and fldvalue = " + currValueCode.ToString();

            try
            {
                return getDataSet(sql).Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get currency shortname, Reason : " + ex.Message);
            }
        }

        public int getCurrValueName(String currcode)
        {
            String sql = "select fldvalue from tbl_lookup where fldgroup = 'currency' and fldcode = '" + currcode.ToString() + "'";

            try
            {
                return int.Parse(getDataSet(sql).Tables[0].Rows[0][0].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get currency value name, Reason : " + ex.Message);
            }
        }
        
        private String getAppSettings(String Key)
        {
            return new AppSettingsReader().GetValue(Key, Type.GetType("System.String")).ToString();
        }

        public irsLinkToGFC(bool isWinApp)
        {
            _isWinApp = isWinApp;
        }
        public irsLinkToGFC() { }

        #endregion
    }

    #region Internet Remitter Saving
    [Serializable]
    public class irsInternetRemitterGFCSavings : sqlHelperWs
    {
        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (IsAdd)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                selectedRow = 0;
            }
        }

        public bool saveGFC(string CIFID, string AccountNo, string AccountType, string Currency, string BranchCode, string Remarks)
        {
            bool ok = false;
            try
            {
                base.addMode = true;
                this.setBase("SELECT TOP 1 * FROM tbl_CIF_Accounts", true);
                setColumn("CIFID", CIFID);
                setColumn("AccountNo", AccountNo);
                setColumn("AccountType", AccountType);
                setColumn("Currency", Currency);
                setColumn("BranchCode", BranchCode);
                setColumn("Remarks", Remarks);
                setColumn("isGFC", true);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                new csAuditLog().writeAudit("", "EXISTING CARD - GFC", "CIFID=[" + CIFID + "] AccountNo=[" + AccountNo + "] AccountType=[" + AccountType + "] Currency=[" + Currency + "] BranchCode=[" + BranchCode + "] Remarks=[" + Remarks + "]", "irsInternetRemitterGFCSavings.saveGFC", "ret=" + ok.ToString(), csAuditLog.Category.Insert);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("", "ERROR EXISTING CARD - GFC", "CIFID=[" + CIFID + "] AccountNo=[" + AccountNo + "] AccountType=[" + AccountType + "] Currency=[" + Currency + "] BranchCode=[" + BranchCode + "] Remarks=[" + Remarks + "]", "irsInternetRemitterGFCSavings.saveGFC", ex.Message, csAuditLog.Category.Insert);
            }
            return ok;
        }
    }
    #endregion
}