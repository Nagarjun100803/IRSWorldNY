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
using IRSClassLib;

namespace IRSWorld.GFC
{
    public partial class GFCMaintenance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GFCMaintenance));
        }

        #region Ajax methods
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getSearchDetails( String SeqID, String Mode)
        {
            try
            {
                Object[] plist = {SeqID, Mode};                
                DataSet ds = doGFC.execProcedure("sp_getGFCClient_Info", plist);

                if (ds != null) { return ds; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {   
                throw new Exception("Failed to get client info, Reason : " + ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getSearchRemDetails(String SeqID)
        {
            try
            {
                DataSet ds = doGFC.execProcedure("sp_getGFCClient_RemInfo", new Object[] { SeqID });

                if (ds != null) { return ds; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {                
                throw new Exception("Failed to get client info, Reason : " + ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getSearchDetailsInquiry( String SeqID, String Mode)
        {
            try
            {
                Object[] plist = {SeqID, Mode};

                DataSet ds = doGFC.execProcedure("sp_getGFCClient_Inquiry", plist);

                if (ds != null) { return ds; }
                else { throw new Exception(doGFC.message); }

            }
            catch (Exception ex)
            {                
                throw new Exception("Failed to get client info, Reason : " + ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getDDLdata()
        {
            try
            {
                String sql = "select fldValue, fldDescription from tbl_lookup where fldgroup = 'GFCReasons' order by fldvalue desc; ";
                sql += "select FldValue, FldCode from tbl_lookup where fldgroup = 'GFCCurrencies' order by fldvalue desc; ";
                sql += "select FldValue, FldCode from tbl_lookup where fldgroup = 'GFCGroups' order by fldcode; ";
                sql += "select FldValue, FldCode from tbl_lookup where fldgroup = 'GFCCLASS' order by fldvalue; ";

                DataSet ds = doGFC.getDataSet(sql);

                if (ds != null) { return ds; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {                
                throw new Exception("Failed to get list values, Reason : " + ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String GetCKAccount(String curr)
        {
            try
            {
                String ck = doGFC.GetCKAccount(curr);

                if (ck != null && ck.Trim() != String.Empty) { return ck; }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {
                auditLog("Failed to get CK Account, Reason : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool UnlockCK(String ckAccount)
        {
            try
            {
                if (doGFC.UnlockCK(ckAccount)) { return true; }
                else { throw new Exception(doGFC.message); }
                                
            }
            catch (Exception ex)
            {
                auditLog("Failed to unlock CK Account " + ckAccount + ", Reason : " + ex.Message);
                throw new Exception("Failed to unlock CK");
            }            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_rem_bene_seq_id(String param)
        {
            try
            {
                String[] p = param.Split('|');

                String sql = "select seq_id from tbl_cif_master where cifid = '" + p[0] + "'; ";
                sql += "select seq_id from tbl_cif_master where cifid = '" + p[1] + "'; ";

                DataSet ds = doGFC.getDataSet(sql);

                return ds.Tables[0].Rows[0][0].ToString() + "|" + ds.Tables[1].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] saveReplaceCancel(String Mode, String Cardno, String AcctNo, String Reason, String NewCardNum, String CurrCode, String CIFID)
        {
            try
            {
                /*
                 *  Mode 0 = 'Replacement'
                 *  Mode 1 = 'Cancelattion'
                 */
                DataTable dt = new DataTable();

                if (doGFC.saveReplaceCancel(Mode, Cardno, AcctNo, Reason, NewCardNum, int.Parse(CurrCode), CIFID))
                {
                   return doGFC.oGFCDetails;
                }
                else
                {
                    throw new Exception(doGFC.message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] releaseGFC(String CIFID, String CKNum, String CardNum, String IDPresented, String CurrCode)
        {
            try
            {   
                if (doGFC.SaveGFMCAccount(CIFID, CardNum, CKNum, int.Parse(CurrCode), IDPresented, "link"))
                {
                    return doGFC.oGFCDetails;
                }
                else
                {
                    throw new Exception(doGFC.message);
                }
            }
            catch (Exception ex)
            {
                auditLog("Failed to release GFC, Reason : " + ex.Message);
                //throw new Exception("Failed to release GFC, Reason : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool submitCK(String CIFID, String CKNum, String CurrCode)
        {
            try
            {
                if (doGFC.SaveGFMCAccount(CIFID, "", CKNum, int.Parse(CurrCode), "", "avail"))
                {
                    return true;
                }
                else { throw new Exception(doGFC.message); }
            }
            catch (Exception ex)
            {
                auditLog("Failed to submit CK, Reason : " + ex.Message);
                //throw new Exception("Failed to submit CK, Reason : " + ex.Message);
                throw new Exception(ex.Message);
            }
            
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] purchaseCard(String CIFID, String CKNum, String CardNum, String IDPresented, String CurrCode)
        {
            try
            {                
                if (doGFC.SaveGFMCAccount(CIFID, CardNum, CKNum, int.Parse(CurrCode), IDPresented, "purchase"))
                {
                    return doGFC.oGFCDetails;
                }
                else
                {
                    throw new Exception(doGFC.message);
                }
            }
            catch (Exception ex)
            {
                auditLog("Failed to release GFC, Reason : " + ex.Message);
                //throw new Exception("Failed to release GFC, Reason : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] linkCIFToCard(String CIFID, String CKNum, String CardNum, String IDPresented, String CurrCode)
        {
            try
            {
                TranClass.RemTranGFMCCommons irsGFC = new IRSWorld.TranClass.RemTranGFMCCommons();

                if (CKNum != null && CKNum != "" && CardNum != null && CardNum != "")
                {
                    if (doGFC.SaveGFMCAccount(CIFID, CardNum, CKNum, int.Parse(CurrCode), IDPresented, "batch"))
                    {
                        return doGFC.oGFCDetails;
                    }
                    else
                    {
                        throw new Exception(doGFC.message);
                    }
                }
                else
                {
                    throw new Exception("GFC Card Number and GFC Reference Number cannot be blank.");
                }

            }
            catch (Exception ex)
            {
                auditLog("Failed to release GFC, Reason : " + ex.Message);
                //throw new Exception("Failed to release GFC, Reason : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool approveRejectTransaction(String SEQID, String Comment, String AppRejTag) //Approve = App Reject = Rej
        {
            try
            {
                if (!doGFC.approveRejectTransaction(SEQID, Comment, AppRejTag))
                {
                    throw new Exception(doGFC.message);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {                
                //throw new Exception("Failed to " + ((AppRejTag == "App") ? "Approve" : "Reject") + " transaction" + ex.Message);
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkLimit(String type)
        {
            return doGFC.checkLimit(((type == "card") ? 1 : 2));
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool auditLog(String Message)
        {
            return true;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable GetBatchDataDetails(String SEQID)
        {
            try
            {                
                String sql = "select 	bd.*, CardType	=	(select flddescription from tbl_lookup where fldgroup = 'GFCCardType' and fldValue = crd.CardType) ";
                sql += "from tbl_gfc_batchdata bd, 	tbl_gfc_cards crd ";
                sql += "where	bd.cardNo	=	crd.CardNo ";
                sql += "and bd.SEQ_ID = " + SEQID;
                DataSet ds = doGFC.getDataSet(sql);

                if (ds != null)
                {
                    return ds.Tables[0];
                }
                else
                {
                    throw new Exception(doGFC.message);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get Batch Card Details, Reason : " + ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String set_report_term_and_condition()
        {
            try
            {
                String date = doGFC.get_branch_localtime().ToString("MM-dd-yyyy");

                String result = doGFC.setReport("/GFC/rptDynamicTermsAndCondition.rpt", new String[] { "0", date, "1" });

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
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String set_report_release_form(String CardNo)
        {
            try
            {
                String result = doGFC.setReport("/GFC/rptBatchMoneyCardReleaseForm.rpt", new String[] { "0", CardNo });

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
        #endregion
        #region private method
        private irsGFC doGFC
        {
            get
            {
                if (Session["GFC"] == null)
                {
                    Session["GFC"] = new irsGFC();
                }
                return (irsGFC)Session["GFC"];
            }
        }
        #endregion
    }


}
