using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.TranClass
{
    [Serializable()]
    public class RemTranGFMCCommons : sqlHelperWs
    {
        public RemTranGFMCCommons()
        {
            base.EnforceConstraint = false;
        }

        #region Ajax methods

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String GetCKAccount(string CIFID, string CardNo, string IDNo, string curr)
        {
            try
            {
                /* FOR CBIP */
                String ck = doGFC.GetCKAccount(CIFID, CardNo, IDNo, curr);

                if (ck != null && ck != String.Empty)
                {
                    return ck;
                }
                else
                {
                    if (doGFC.message != null && doGFC.message != String.Empty)
                    {
                        throw new Exception(doGFC.message);
                    }
                    else
                    {
                        throw new Exception(getCKRunOutMsg());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get CK Account, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_account_type(String CK)
        {
            try
            {
                return doGFC.GetCKAcctType(CK);
            }
            catch
            {
                return "";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_userrole_is_agent(int UserRole)
        {
            try
            {
                if (int.Parse(doGFC.execStoredProcedure("sp_internet_check_userrole_if_agent", new Object[] { UserRole }).Tables[0].Rows[0][0].ToString()) > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool UnlockCK(String ckAccount)
        {
            try
            {
                if (doGFC.UnlockCK(ckAccount)) { return true; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to unlock ck account, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet populateDDL()
        {
            String SQL = "select [Text] = isnull(FldDescription,''), [Value] = isnull(FldValue,'') from tbl_lookup where FldGroup = 'GFCCurrencies'; ";
            DataSet ds = doGFC.getDataSet(SQL);

            if (ds != null) { return ds; }
            else { throw new Exception(doGFC.message); }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool checkIfBeneIsCorporate(String CIFID)
        {
            try
            {
                DataSet ds = doGFC.execProcedure("sp_internet_checkIfBeneIsCorporate", new Object[] { CIFID });

                if (ds != null)
                {
                    return (bool)ds.Tables[0].Rows[0][0];
                }
            }
            catch { }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getCardDetails(String CIFID, String CardNo)
        {
            String SQL = "select AccountNo, CardNo, Currency, AcctSeqID, CIFID from v_gfc_status where cardNo = '" + CardNo + "' and GFCStats in (1,2,3)";
            DataSet ds = doGFC.getDataSet(SQL);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0) { return ds; }
            else
            {
                if (doGFC.message != null && doGFC.message != String.Empty)
                {
                    throw new Exception(doGFC.message);
                }
                else
                {
                    throw new Exception("Card entered is either not valid or is not yet linked.");
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String getIRSWCurrEquiv(String CurrCode)
        {
            try
            {
                String SQL = "select fldvalue from tbl_lookup where fldgroup = 'currency' and fldcode = '" + CurrCode + "'";
                return doGFC.getDataSet(SQL).Tables[0].Rows[0][0].ToString();
            }
            catch
            {
                return "";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String validateGFC(String Mode, String CardNo, String CKAcct)
        {
            String msg = "";
            if (doGFC.validateGFC(Mode, CardNo, CKAcct, out msg))
            {
                return "00|Valid";
            }
            else
            {
                return "99|" + msg;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        // CJ - RFC: 25136 - Blocking of SSS in Rapid Remit
        public string RapidRemitDetails_(string CIFID, String CardNo, double RemitBenAmount)
        {
            try
            {
                string ret = null;
                String ds2 = null;

                ds2 = doGFC.GetRapidRemitDetails(CIFID, CardNo, RemitBenAmount);
                
                if (ds2 != null && ds2.Length != 0)
                {
                    string[] ret_val = ds2.ToString().Split('|');
                    ret = ret_val[0].ToString();
                    return ret;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        // CJ RFC: 25631 - SSS Pension Exception
        // CJ - GFC FIX Change Product
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        
        public string get_branch_name()
        {
            DataTable dt = null;
            irsSqlCommonsWs common = new irsSqlCommonsWs();

            DataSet ds = common.getDataSet(common.getSqlCommand("SELECT TOP 1 a.FldDescription + ' - ' + b.BranchName as branchname FROM tbl_Lookup a INNER JOIN tbl_branches b ON a.FldDescription = b.BranchCode WHERE a.FldGroup = 'GFCDEFAULTS' AND a.FldCode = 'BranchCode' AND a.FldValue = 0", false));
            if (ds != null)
            {
                if (ds.Tables[0].Rows[0].ToString() != null || ds.Tables[0].Rows[0].ToString() != "")
                {
                    dt = ds.Tables[0];

                    return dt.Rows[0][0].ToString();
                }
                return null;
            }
            return null;
        }
        // CJ - GFC FIX Change Product
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet GFMCDetails_(String CardNo)
        {
            try
            {
                DataSet ds2 = null; //doGFC.get_gfc_details_from_egfmc_system(CardNo);
                if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                {
                    return ds2;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet GFMCDetails(String CIFID, String CKSEQID, String CardNo)
        {
            try
            {
                DataSet ds = doGFC.execProcedure("sp_get_GFMCDetails", new Object[] { CIFID, CKSEQID, CardNo });
                if (ds != null)
                {
                    if (CKSEQID == "EXCARD" && ds.Tables.Count > 1 && ds.Tables[1].Rows.Count == 0)
                    {
                        /* FOR CBIP */
                        DataSet ds2 = doGFC.getExistingGFC(CardNo); //doGFC.get_gfc_details_from_egfmc_system(CardNo);
                        if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            DataTable dt2 = new DataTable();
                            String AcctType = "";
                            String irsCurr = "";

                            AcctType = get_account_type(ds2.Tables[0].Rows[0]["rec_CkAcctNo"].ToString());
                            irsCurr = getIRSWCurrEquiv(ds2.Tables[0].Rows[0]["CKCurrCode"].ToString());

                            dt2.Columns.Add("AcctType");
                            dt2.Columns.Add("irsCurr");

                            dt2.Rows.Add(new Object[] { AcctType, irsCurr });

                            dt = ds2.Tables[0].Copy();
                            ds.Tables.Add(dt);
                            ds.Tables.Add(dt2);
                        }
                        else
                        {
                            throw new Exception("Card is invalid or not yet claimed.");
                        }
                    }
                    return ds;
                }
                else
                {
                    throw new Exception(doGFC.message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get GFC Details, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public Object[] SaveGFMCAccount(String CIFID, String CardNo, String AcctNo, String Curr, String IDPresented, String Mode, String AccountType)
        {
            try
            {
                if (IDPresented == "")
                {
                    IDPresented = "N/A";
                }

                if (doGFC.SaveGFMCAccount(CIFID, CardNo, AcctNo, int.Parse(Curr), IDPresented, Mode, AccountType))
                {
                    //return doGFC.oGFCDetails; 
                    return new Object[] { CIFID, CardNo, AcctNo, Curr, getCKSeqId(AcctNo) };
                }
                else
                {
                    throw new Exception(doGFC.message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save GFC, Reason : " + ex.Message);
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String set_report(String CardNo, String ReportType)
        {
            try
            {
                String ReportName = "";
                String[] Parameter = null;

                if (ReportType == "Release")
                {
                    ReportName = "/GFC/rptBatchMoneyCardReleaseForm.rpt";
                    Parameter = new String[] { "0", CardNo };
                }
                else if (ReportType == "Terms")
                {
                    ReportName = "/GFC/rptDynamicTermsAndCondition.rpt";
                    Parameter = new String[] { "0", doGFC.get_branch_localtime().ToString("MM/dd/yyyy"), "1" };
                }

                String result = doGFC.setReport(ReportName, Parameter);

                if (result == "")
                {
                    throw new Exception(doGFC.message);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool OTCCheckIfHasTransaction(String AcctNumber)
        {
            try
            {
                String sql = "select count(1) from tbl_remittance nolock where AccountSEQID = isnull((select seq_id from tbl_cif_accounts where AccountNo = '" + AcctNumber + "'),0)";

                if (int.Parse(doGFC.getDataSet(sql, false).Tables[0].Rows[0][0].ToString()) > 0)
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

        #endregion
        #region private region

        //private irsGFC doGFC
        //{
        //    get
        //    {
        //        if (System.Web.HttpContext.Current.Session["GFCPopup"] == null)
        //        {
        //            System.Web.HttpContext.Current.Session["GFCPopup"] = new irsGFC();

        //        }
        //        return (irsGFC)System.Web.HttpContext.Current.Session["GFCPopup"];
        //    }
        //}

        private irsLinkToGFC doGFC
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["GFCPopup"] == null)
                {
                    System.Web.HttpContext.Current.Session["GFCPopup"] = new irsLinkToGFC();
                }
                return (irsLinkToGFC)System.Web.HttpContext.Current.Session["GFCPopup"];
            }
        }

        private String getCKRunOutMsg()
        {
            try
            {
                String sql = "select FldDescription from tbl_lookup where fldgroup = 'GFCLOADERDEFAULTS' and fldCode = 'MESSAGE' and fldvalue = 0";
                DataSet ds = doGFC.getDataSet(sql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch { }
            return "Available CK for this currency has runout.";
        }

        private String getCKSeqId(String CKAccount)
        {
            try
            {
                String sql = "select top 1 seq_id from tbl_cif_accounts where accountno = '" + CKAccount + "'";
                DataSet ds = doGFC.getDataSet(sql);

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch { }
            return "";
        }


        #endregion


    }
}
