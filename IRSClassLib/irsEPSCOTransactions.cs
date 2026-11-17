using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace IRSClassLib
{
    [Serializable]
    public class irsEPSCOTransactions : sqlHelperWs
    {
        public irsEPSCOTransactions()
        {
            this.TableName = "tbl_remittance";
            this.IdentityColumn = "SEQ_ID";
        }

        public DataTable getRemitterInFo(string CIFID)
        {
            DataTable dt = null;
            try
            {
                dt = getDataSet(getSqlCommand("SELECT * FROM TBL_CIF_MASTER WHERE CIFID ='" + CIFID + "'", false)).Tables[0];
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getRemitterInFo", CIFID, ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getBenefInfo(long benseqid)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getBeneInfo", new object[] { benseqid });
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getBenefInfo", benseqid.ToString(), ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getBeneAddress(string BeneCIFID)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getBeneAddress", new object[] { BeneCIFID });
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getBeneAddress", BeneCIFID, ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getBeneContact(string BeneCIFID)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getBeneContact", new object[] { BeneCIFID });
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getBeneContact", BeneCIFID, ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getBeneEmailAddress(string BeneCIFID)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getBeneEmail", new object[] { BeneCIFID });
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getBeneEmailAddress", BeneCIFID, ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getBeneAccounts(string BeneCIFID)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getBeneAccounts", new object[] { BeneCIFID });
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getBeneAccounts", BeneCIFID, ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getBeneAccountDetails(long AcctSeqID)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getBeneAccountsDet", new object[] { AcctSeqID });
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getBeneAccountDetails", AcctSeqID.ToString(), ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getExchangeRates(string brcode)
        {
            DataTable dt = null;
            try
            {
                dt = getDataSet(getSqlCommand("select * from tbl_currency_rates where branchcode = '" + brcode + "' AND tranxDate = (SELECT MAX(TranxDate) FROM TBL_CURRENCY_RATES WHERE BRANCHCODE = '" + brcode + "') UNION ALL SELECT 0,'" + brcode + "',getDate(),1,1,1,1,getDate(),1,NULL,NULL,0", false)).Tables[0];
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getExchangeRates", brcode, ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getBankChargeID(string CIFID, string brcode)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getBankChargeID", new object[] { CIFID, brcode });
            }
            catch (Exception ex)
            { new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getBankChargeID", "", ex.Message, csAuditLog.Category.Select); }

            return dt;
        }

        public DataTable getDDLCountry(string SQL)
        {
            DataTable dt = null;
            try
            {
                dt = getDataSet(getSqlCommand(SQL, false)).Tables[0];
            }
            catch (Exception ex)
            { new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getDDLCountry", "", ex.Message, csAuditLog.Category.Select); }

            return dt;
        }

        public string check_BeneAcctNo(string AcctNo)
        {
            string msg = "";

            try
            {
                irsAccountAlgo acct_algo = new irsAccountAlgo();
                if (acct_algo.checkAccount(AcctNo) == false)
                {
                    msg = acct_algo.message;
                }
            }
            catch (Exception ex)
            { new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "check_BeneAcctNo", "", ex.Message, csAuditLog.Category.Select); }

            return msg;
        }

        public int enroll_Client(string record, string method)
        {
            try
            {
                int result = 0;

                object[] paramlist = record.Split('~');

                if (method == "ADD")
                {
                    new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "ADD ENROLLEE", record, "", csAuditLog.Category.Insert); 
                    execStoredProcedureUpdate("sp_cmcs_cross_update", paramlist);
                    result = execStoredProcedureUpdate("sp_cmcs_enroll_client", paramlist);
                }
                else if (method == "UPDATE")
                {
                    new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "UPDATE ENROLLEE", record, "", csAuditLog.Category.Update); 
                    result = execStoredProcedureUpdate("sp_cmcs_update_client", paramlist);
                }

                if (result == 0) new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "enroll_client", "", base.Message, csAuditLog.Category.Select);

                return result;
            }
            catch (Exception ex)
            { new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "enroll_client", "", ex.Message, csAuditLog.Category.Select); }

            return 0;
        }

        public int getMatchedCIF(string CIF)
        {
            DataTable dt = null;

            try
            {
                object[] paramlist = CIF.Split('~');
                dt = execStoredProcedureTable("sp_cmcs_get_matched_cif", paramlist);

                return int.Parse(dt.Rows[0].ItemArray[0].ToString());

            }
            catch (Exception ex)
            { new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getMatchedCIF", "", ex.Message, csAuditLog.Category.Select); }

            return 0;
        }

        public DataTable getCIFDetails(int SEQ_ID, int CIFClass, int Ben_EPSCO_SEQID)
        {
            DataTable dt = null;
            try
            {
                object[] paramlist = new object[] { SEQ_ID, CIFClass, Ben_EPSCO_SEQID };
                dt = execStoredProcedureTable("sp_getCIFDetails", paramlist);
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getCIFDetails", "", ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        public DataTable getEnrolleeDetails(int ben_seqid)
        {
            DataTable dt = null;
            try
            {
                string script = "SELECT     ISNULL(a.AcctNo,'') as 'AcctNo', " +
                                "           ISNULL(a.FirstName,'') as 'Ben_FirstName', " +
                                "           ISNULL(a.MiddleName,'') as 'Ben_MiddleName', " +
                                "           ISNULL(a.LastName,'') as 'Ben_LastName', " +
                                "           ISNULL(d.FirstName,'') as 'Rem_FirstName', " +
                                "           ISNULL(d.MiddleName,'') as 'Rem_MiddleName', " +
                                "           ISNULL(d.LastName,'') as 'Rem_LastName' " +
                                "FROM		tbl_cmcs_enrollment a " +
                                "INNER JOIN	tbl_cmcs_ids b " +
                                "ON			a.SEQ_ID = B.EPSCO_SEQID " +
                                "INNER JOIN	tbl_cif_master c " +
                                "ON			c.SEQ_ID =  b.REM_SEQID " +
                                "INNER JOIN	tbl_cmcs_enrollment d " +
                                "ON			d.CIFID = c.CIFID " +
                                "WHERE		a.SEQ_ID = " + ben_seqid.ToString();

                dt = getDataSet(script, false, new object[] { }).Tables[0];
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getEnrolleeDetails", "", ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }


        public bool delete_Client(int rem_seqid, int ben_seqid)
        {
            string script = " DECLARE @SEQID bigint" +
                            " SELECT @SEQID = EPSCO_SEQID FROM tbl_cmcs_ids WHERE Rem_SEQID = " + rem_seqid.ToString() + " AND Ben_SEQID = " + ben_seqid.ToString() +
                            " DELETE FROM tbl_cmcs_enrollment WHERE SEQ_ID = @SEQID" +
                            " DELETE FROM tbl_cmcs_ids WHERE EPSCO_SEQID = @SEQID";

            execScalar(script);

            if (base.Message.Trim() == "")
            {
                return true;
            }
            else
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "delete_Client", "", base.Message, csAuditLog.Category.Select);
                return false;
            }
        }

        public string getEPSCORefNo(int ben_seqid)
        {
            DataTable dt = null;

            try
            {
                string script = "SELECT BarcodeNo FROM tbl_cmcs_enrollment WHERE SEQ_ID = " + ben_seqid.ToString();

                dt = getDataSet(getSqlCommand(script, false)).Tables[0];

                return dt.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "getEPSCORefNo", "", ex.Message, csAuditLog.Category.Select);
            }

            return "ERROR GETTING REFERENCE NO.";
        }


        public int generate_RefNo(int ben_seqid)
        {
            string script = "SELECT        c.SEQ_ID " +
                            "FROM          tbl_cif_master a " +
                            "INNER JOIN    tbl_cmcs_ids b " +
                            "ON            b.Rem_SEQID = a.SEQ_ID " +
                            "INNER JOIN    tbl_cmcs_enrollment c " +
                            "ON            c.CIFID = a.CIFID " +
                            "WHERE         b.EPSCO_SEQID = " + ben_seqid.ToString() +
                            "              AND ISNULL(a.Deleted,0) = 0 AND c.CIFClass = 1";

            int rem_seqid = Convert.ToInt32(execScalar(script));

            if (rem_seqid == 0) return 0;

            script = "DECLARE @prefix varchar(8) " +
                         "DECLARE @suffix varchar(3) " +
                         "SET @prefix = '2' + RIGHT('000000' +  RTRIM(CONVERT(varchar(7), " + rem_seqid.ToString() + ")), 7) " +
                         "SET @suffix = RIGHT('00' + RTRIM(CONVERT(varchar(3), (SELECT COUNT(*) + 1 FROM tbl_cmcs_enrollment WHERE " +
                         //Feb 27, 2013 "CAST(RIGHT(LEFT(EPSCORefNo,8),7) AS INT) = " + rem_seqid.ToString() + "))), 3) " +
                         "CONVERT(INT,LEFT(RIGHT(BarcodeNo,13),7)) = " + rem_seqid.ToString() + "))), 3) " +
                         
                         "UPDATE    tbl_cmcs_enrollment " +
                         "SET		EPSCORefNo = @prefix + @suffix, " +
                         "          BarcodeNo = '00699' + @prefix + @suffix + '63', " +
                         "          DateApproved = CONVERT(varchar(10), GETDATE(), 101) " +
                         "WHERE	    SEQ_ID = " + ben_seqid.ToString() +
                         " SELECT @prefix + @suffix + '63'";

            try
            {
                object sequence = execScalar(script);

                if (sequence == null)
                {
                    new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "generate_RefNo", "", base.Message, csAuditLog.Category.Select);
                    return 0;
                }
                else
                {
                    script = "UPDATE    tbl_cmcs_enrollment " +
                                "SET		BarcodeNo = BarcodeNo +  '" + computeDigit(sequence).ToString() + "' " +
                                "WHERE	    SEQ_ID = " + ben_seqid.ToString();

                    execScalar(script);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "generate_RefNo", "", ex.Message, csAuditLog.Category.Select);
                return 0;
            }
        }

        public string InsertToRemittance(int SEQ_ID)
        {
            string err_msg = "";

            DataTable dt = getDataSet("SELECT * FROM TBL_EPSCO_TRANSACTION WHERE SEQ_ID = " + SEQ_ID.ToString() + " AND ISNULL(AcctSEQID,0) > 0", false, new object[] { }).Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                string[] param = new string[] {
                                            "UserID~" + dr["UserID"].ToString(),
                                            "RemitMethod~" + dr["RemitMethod"].ToString(),
                                            "RemittingBranch~" + dr["RemitBranch"].ToString(),
                                            "ChargeLocally~false" + DBNull.Value.ToString(),
                                            "ExtlReferenceNo~" + DBNull.Value.ToString(),
                                            "Transtatus~" + dr["Transtatus"].ToString(),
                                            "TranxDate~" + DateTime.Now.ToString(),
                                            "RemitterCIFID~" + dr["RemitterCIFID"].ToString(),
                                            "BeneficiaryCIFID~" + dr["BeneficiaryCIFID"].ToString(),
                                            "AccountSEQID~" + dr["AcctSEQID"].ToString(),
                                            "RemittingCurrency~" + dr["RemitCurrency"].ToString(),
                                            "TransactionSource~" + dr["TranxSource"].ToString(),
                                            "ReceiptMode~" + DBNull.Value.ToString(),
                                            "TransactionPurpose~" + DBNull.Value.ToString(),
                                            "SourceofFunds~" + DBNull.Value.ToString(),
                                            "DatePostedOverseas~" + new irsTimeZone().branchLocalTime(dr["RemitBranch"].ToString()),
                                            "DatePostedLocal~" + new irsTimeZone().ServerTime.ToString(),
                                            "BatchNo~" + "0",
                                            "Printed~" + "false",
                                            "DateExtracted~" + DBNull.Value.ToString(),
                                            "RemitterAddressSeqID~" + DBNull.Value.ToString(),
                                            "RemitterContactSeqID~" + DBNull.Value.ToString(),
                                            "BeneficiaryAddressSeqID~" + DBNull.Value.ToString(),
                                            "BeneficiaryContactSeqID~" + DBNull.Value.ToString()
                                        };

                try
                {
                    int acct_currency = 0;

                    object result = base.execScalar("SELECT CURRENCY FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + dr["AcctSEQID"].ToString());

                    if (result != null)
                    {
                        acct_currency = Convert.ToInt32(result);
                    }

                    irsInsertRemittanceWs rem = new irsInsertRemittanceWs(dr["RemitBranch"].ToString(), dr["UserID"].ToString());
                    rem.ProductChargeId = dr["ProductChargeID"].ToString();

                    if (!rem.Remit(acct_currency, Convert.ToDouble(dr["TranxAmount"].ToString()), param, 0))
                    {
                        new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "InsertToRemittance", "", "Error on computing transaction.", csAuditLog.Category.Insert);
                        err_msg = rem.RemitMessage;
                    }
                    else
                    {
                        execScalar("UPDATE tbl_epsco_transaction SET Deleted = 1 WHERE SEQ_ID = " + dr["SEQ_ID"].ToString());

                        execScalar("UPDATE tbl_remittance SET SpecialMessage = '" + dr["MerchantRefNo"] + "' WHERE SEQ_ID = " + rem.RemittanceId);
                    }
                }
                catch (Exception ex)
                {
                    new csAuditLog().writeAudit(ClientData._userid, "EPSCO", "InsertToRemittance", "", ex.Message, csAuditLog.Category.Insert);
                    err_msg = ex.Message;
                }
            }


            return err_msg;
        }

        private int computeDigit(object sequence)
        {

            int[] num = new int[sequence.ToString().Length];

            for (int i = num.Length; i > 0; i--)
            {
                num[(num.Length - i)] = Convert.ToInt32(sequence.ToString().Substring((i - 1), 1));
            }

            int j = 0;

            int x = 1;

            foreach (int n in num)
            {
                num[j] = n * x;

                if (num[j] > 9) num[j] = (num[j] / 10) + (num[j] % 10);

                x = (x == 1 ? 2 : 1);
                j++;
            }

            int sum = 0;

            foreach (int n in num)
            {
                sum += n;
            }

            int total = 10 - (sum % 10);

            return (total == 10) ? 0 : total;
        }


        public DataTable getDataTable(string param)
        {
            string sp = param.Remove(param.IndexOf("~", 0));

            object[] p = (object[])param.Substring(param.IndexOf("~", 0) + 1).Split(new char[] { '~' });

            DataTable dt = getDataSet(sp, true, p).Tables[0];

            return dt;
        }

    }
}
