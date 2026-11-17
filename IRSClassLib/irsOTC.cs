using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsOTC : sqlHelperWs
    {
        csAuditLog audit = new csAuditLog();
        public DataTable getBeneficiary(string RemitterID)
        {
            getUpdateDS("osp_getBeneficiary", new Object[] { RemitterID });
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

        public DataTable getBeneficiaryAccounts(string BeneficiaryID, bool IsCIF)
        {
            getUpdateDS("osp_getBeneficiaryAccounts", new Object[] { BeneficiaryID, IsCIF });
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

        public DataTable getLookupDefaults(string BranchCode, string FldGroup)
        {
            getUpdateDS("osp_getLookupDefaults", new Object[] { BranchCode, FldGroup });
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

        public DataTable getExchangeRate(string BranchCode, string SourceCurrency, string TargetCurrency)
        {
            getUpdateDS("osp_getExchangeRate", new Object[] { BranchCode, SourceCurrency, TargetCurrency });
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

        public DataTable getRemitter(string SEQ_ID)
        {
            getUpdateDS("osp_getRemitter", new Object[] { SEQ_ID });
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

        public DataTable getRemitterCards(string CIFID)
        {
            getUpdateDS("osp_getRemitterCards", new Object[] { CIFID });
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

        public DataTable getSearch(string CIFClass, string CIFID, string LastName, string FirstName, string MiddleName)
        {
            getUpdateDS("osp_getSearch", new Object[] { CIFClass, CIFID, LastName, FirstName, MiddleName });
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

        public DataTable getSearchBy()
        {
            getUpdateDS("osp_getSearchBy", new Object[] { });
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
        public  DataTable IRSReceipt(int CIFID)
        {
            //DataTable dt = execStoredProcedureTable("spGetOBPSDataLatestInserted", new object[] { CIFID });
            irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                //CIFID = (CIFID == "") ? this.TranxID : CIFID;
                return execStoredProcedureTable("sp_rptReceiptRemit", new object[] { Convert.ToInt32(CIFID) });
            }
            catch (Exception ex)
            {
                audit.writeAudit(client_data.UserId, "IRS", "Receipt", ex.Message, CIFID.ToString(), csAuditLog.Category.Select);
                return null;
            }
        }

        public DataSet EHUDReceipt(string ReferenceNo)
        {
            //DataTable dt = execStoredProcedureTable("spGetOBPSDataLatestInserted", new object[] { CIFID });
            irsClientData client_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                //return execStoredProcedureTable("sp_rptReceiptRemit", new object[] { Convert.ToInt32(CIFID) });
                //return execStoredProcedure("spz_EHUDReceipt", new object[] { ReferenceNo });
                return execStoredProcedure("spz_EHUDReceipt", new object[] { ReferenceNo }, new string[] { "dtReceiptEHUD", "dtEHUDTop10Transaction" });
            }
            catch (Exception ex)
            {
                audit.writeAudit(client_data.UserId, "IRSOTC", "EHUDReceipt", ex.Message, ReferenceNo, csAuditLog.Category.Select);
                
                return null;
            }
        }
        /*
        public ReportDocument GenerateEHUDReceipt(ReportDocument Ehudrcpt, string ReferenceNo)

        {
            DataSet ds = EHUDReceipt(ReferenceNo);
            Ehudrcpt.SetDataSource(ds);
            return Ehudrcpt;
        }
        
        public ReportDocument GenerateReceipt(ReportDocument rcpt, int trxID)
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

                DataTable dt = IRSReceipt(trxID);
                //System.Drawing.Printing.PrintDocument otemp = new System.Drawing.Printing.PrintDocument();
                //System.Drawing.Printing.PrinterSettings                
                rcpt.SetDataSource(dt);

                //discreteVal.Value = dt.Rows[0]["ClearingDate"].ToString();
                //paramVal.Add(discreteVal);
                //rcpt.DataDefinition.ParameterFields["parClearingDate"].ApplyCurrentValues(paramVal);

                discreteVal.Value = header1;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parHeaderOne"].ApplyCurrentValues(paramVal);

                discreteVal.Value = header2;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parHeaderTwo"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchname;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchName"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchaddr;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchAddress"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchcontactno;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchContactNo"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sbranchemail;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parBranchEmailAdd"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID1;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID1"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID2;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID2"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID3;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID3"].ApplyCurrentValues(paramVal);

                discreteVal.Value = sID4;
                paramVal.Add(discreteVal);
                rcpt.DataDefinition.ParameterFields["parID4"].ApplyCurrentValues(paramVal);

                //rptReceiptrcpt.Refresh();

                //rcpt.PrintToPrinter(1, false, 1, 1);
                return rcpt;
            }
            catch (Exception ex)
            {
                audit.writeAudit(client_data.UserId, "OTC Transaction", "GenerateReceipt", ex.Message, trxID.ToString(), csAuditLog.Category.Select);
                throw ex;
            }
            return rcpt;
        }
        */
    }
}
