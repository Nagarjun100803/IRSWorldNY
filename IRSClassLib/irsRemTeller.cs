using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using IRSClassLib;
using System.Collections;
//INSTAPAY ALYSSA SEPTEMBER 2019
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Linq;
//INSTAPAY ALYSSA SEPTEMBER 2019

namespace IRSClassLib
{
    public enum Trans_Status : int
    {
        Credited_Online = 2,
        Posted_Central = 5,
        On_Hold = 14,
        For_Approval = 15,
        For_Reprocess = 9,
        Cancel_Not_Implemented = 11,
        Cancel_Implemented = 12,
        Sent_To_GRS = 16,
        For_Batch_Processing = 25,
        Rejected = 256,
        InterBank_Timed_Out = 10315, //INSTAPAY ALYSSA SEPTEMBER 2019
    }

    public enum ImplemType : int
    {
        BPO = 1,
        CreditToAccount = 2,
        OtherBanksIBFT = 17,
        OtherBanksINSTAPAY = 29 //INSTAPAY ALYSSA SEPTEMBER 2019
    }

    public enum ProductCode : int
    {
        RapidRemit = 10,
        GFC = 22,
        WealthMultiplier = 14,
        PAW = 24,
        APASOnline = 27,
        IBFT = 23,
        INSTAPAY = 258 //INSTAPAY ALYSSA SEPTEMBER 2019
    }

    public class irsRemTeller : sqlHelperWs
    {
        csAuditLog audit = new csAuditLog();
        irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

        public string AccountStatusFC = null;
        public string AccountStatusFCValid = null;
        private string myErrorMessage;
        private string myAccountMessage;
        private string BranchCode;
        System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
        private string account_id = null;
        private string account_status = null;
        private string unclear_funds = null;
        private double available_balance = 0;
        private string customer_shortname = null;
        private string product_name = null;
        private string currency_shortname = null;
        private string open_date = null;
        private string rep_code = null;
        private double activity_today = 0;
        private double current_balance = 0;
        private string txn_db_table_name = string.Empty;
        private string instapay_tranxStatus = null; //INSTAPAY ALYSSA SEPTEMBER 2019

        #region "PUBLIC PROPERTY"

        public string Account_Customer_Name()
        {
            return this.customer_shortname;
        }

        public string Account_Id()
        {
            return this.account_id;
        }

        public string Account_Status()
        {
            return this.account_status;
        }

        public string Unclear_Funds()
        {
            return this.unclear_funds;
        }
        public double Available_Balance()
        {
            return this.available_balance;
        }
        public double Currenct_Balance()
        {
            return this.current_balance;
        }
        public string Instapay_TranxStatus() //INSTAPAY ALYSSA SEPTEMBER 2019
        {
            return this.instapay_tranxStatus;
        }
        #endregion

        private string GetAppSetting(string Key)
        {
            try
            {
                string sTemp = null;
                sTemp = (String)new AppSettingsReader().GetValue(Key, typeof(System.String));
                if (sTemp != null)
                    return sTemp;
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public irsRemTeller(string branchcode)
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            BranchCode = branchcode;
        }

        public irsRemTeller(string branchcode, int paymode)
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            BranchCode = branchcode;
        }

        public irsRemTeller()
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";

            BranchCode = "";
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = '" + id + "'", false);
            getUpdateDataSet(command);
        }

        public void load_rem_transaction(int tran_id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + tran_id, false);
            DataSet dt = getDataSet(command, "REM_TRANSACTION");

            if (dt != null && dt.Tables["REM_TRANSACTION"].Rows.Count > 0)
            {
            }
        }

        public string get_nsvs(string branchcode, int currency)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("sp_getNSVS", new object[] { branchcode, currency });
                object[] MSG = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    MSG = row.ItemArray;
                    return MSG[0].ToString();
                }
                return "";
            }
            catch (SqlException e)
            {
                return e.ToString();
            }
        }

        //INSTAPAY ALYSSA SEPTEMBER 2019
        public string get_instapay_nsvs(int currency)
        {

            DataTable dt = execStoredProcedureTable("sp_get_instapay_nsvs", new object[] { currency });

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                object[] MSG = row.ItemArray;

                return MSG[0].ToString();
            }
            return "";
        }
        public DataTable get_instapay_tranx_details(int tranxid)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("get_instapay_tranx_details", new object[] { tranxid });
                return dt;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public string get_instapay_Rejected_replycode(string replycode)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("get_instapay_Rejected_replycode", new object[] { replycode });
                object[] MSG = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    MSG = row.ItemArray;
                    return MSG[0].ToString();
                }
                return "";
            }
            catch (SqlException e)
            {
                return e.ToString();
            }
        }

        public int get_instapaytransactions(int tranxid)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("get_instapaytransactions", new object[] { tranxid });
                if (dt != null && dt.Rows.Count > 0)
                {
                    return 1;
                }
                return 0;
            }
            catch (SqlException e)
            {
                return 0;
            }
        }
        //INSTAPAY 

        public DataTable get_remit_details(int tranxid)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("sp_getRemitDetails", new object[] { tranxid });
                return dt;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public DataTable get_remit_debitdetails(int tranxid)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("sp_getRemit_debitDetails", new object[] { tranxid });
                return dt;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public string get_gl(string branchcode, int currency, int seq_id)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("sp_getgl", new object[] { branchcode, currency, seq_id });
                object[] MSG = null;
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    MSG = row.ItemArray;
                    return MSG[0].ToString();
                }
                return "";
            }
            catch (SqlException e)
            {
                return e.ToString();
            }
        }

        public DataTable retrieve_batch_approval(string branchcode, string tranxdate, string numtrans, string otherchannels, string paymode, string productcode, string implemtype, string status, string userRole, string ordbatchno, string filetype, string fileformat)
        {
            DataTable dt = null;
            try
            {
                DateTime currenttime = DateTime.Now;
                dt = execStoredProcedureTable("Sp_RetrieveBatchApproval", new object[] { branchcode, tranxdate, numtrans, otherchannels, paymode, productcode, implemtype, status, currenttime, userRole, ordbatchno, filetype, fileformat });
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return dt;
        }

        private string parse_data(string[] data)
        {
            string parsedata = "";
            if (data.Length > 0)
            {
                parsedata = "(";
                for (int i = 0; i < data.Length; i++)
                {
                    parsedata += data[i] + ",";
                }
                if (parsedata.Length > 0)
                {
                    parsedata = parsedata.Substring(0, (parsedata.Length - 2));
                }
                parsedata += ")";
            }
            return parsedata;
        }

        public string process_batch_approval(string refno, string branchcode, string transtatus)
        {
            string myFileName;
            string sTemp;
            string newrefno = parse_data(refno.Split('~'));
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(clientdata.Location.ToString());
            try
            {
                DateTime currenttime = DateTime.Now;
                DataTable dt = execStoredProcedureTable("Sp_ProcessBatchApproval_with_review", new object[] { branchcode, newrefno, transtatus, currenttime, clientdata.UserId, Convert.ToString(dtLocalTime) });

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    object[] MSG = row.ItemArray;

                    if (MSG[2].ToString() != "0")
                    {
                        sTemp = this.GetAppSetting("BatchApprovalFolder");
                        DirectoryInfo di = new DirectoryInfo(sTemp);

                        // Determine whether the directory exists.
                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        myFileName = @"\IRS" + branchcode + string.Format("{0:MMddyyyy}", System.DateTime.Now.Date) + "_" + MSG[0].ToString() + ".txt";
                        myFileName = di.FullName + myFileName;

                        FileInfo f = new FileInfo(myFileName);
                        StreamWriter s = f.CreateText();
                        s.WriteLine(branchcode + "~" + MSG[2].ToString() + "~" + MSG[0].ToString());
                        s.Close();
                    }

                    string returnMessage = null;
                    returnMessage = "SUMMARY \r ";

                    if (MSG[2].ToString() != "0")
                    {
                        returnMessage += " Online: " + MSG[2].ToString() + " \r ";
                    }
                    if (MSG[1].ToString() != "0")
                    {
                        returnMessage += " Non Online: " + MSG[1].ToString() + " \r ";
                    }
                    returnMessage += " Batch Number: " + MSG[0].ToString() + " \r ";

                    if ((MSG[1].ToString() != "0") || (MSG[2].ToString() != "0"))
                    { return returnMessage; }

                    return " No Record(s) processed";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return Message;
        }

        public void update_approving_officer(int seqid)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                DataTable dt = execStoredProcedureTable("sp_update_remittance_approvingofficer", new object[] { seqid, clientdata.UserId });

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    object[] MSG = row.ItemArray;

                    if (MSG[0].ToString() != "-1")
                    {
                        audit.writeAudit(clientdata.UserId, "Approval", "Update Approving Officer", clientdata.UserId + " TXN_SEQ_ID:" + seqid.ToString(), MSG[0].ToString(), csAuditLog.Category.FlexCube);
                    }
                    else
                    {
                        audit.writeAudit(clientdata.UserId, "Approval", "Update Approving Officer", clientdata.UserId + " TXN_SEQ_ID:" + seqid.ToString(), MSG[0].ToString(), csAuditLog.Category.FlexCube);
                    }
                }
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "Approval", "Update Approving Officer", clientdata.UserId + " TXN_SEQ_ID:" + seqid.ToString(), ex.Message, csAuditLog.Category.FlexCube);
                throw (ex);
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

        public string process_batch_approval(string branchcode, string tranxdate, string numtrans, string otherchannels, string paymode, string productcode, string implemtype, string status, string userRole, string ordbatchno, string filetype, string fileformat)
        {
            string myFileName;
            string sTemp;
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(clientdata.Location.ToString());

            try
            {
                DateTime currenttime = DateTime.Now;
                DataTable dt = execStoredProcedureTable("Sp_ProcessBatchApproval", new object[] { branchcode, tranxdate, numtrans, otherchannels, paymode, productcode, implemtype, status, currenttime, userRole, clientdata.UserId, ordbatchno, filetype, fileformat, Convert.ToString(dtLocalTime) });

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    object[] MSG = row.ItemArray;

                    if (MSG[2].ToString() != "0")
                    {
                        // Specify the directories you want to manipulate.
                        sTemp = this.GetAppSetting("BatchApprovalFolder");
                        DirectoryInfo di = new DirectoryInfo(sTemp);

                        // Determine whether the directory exists.
                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        myFileName = @"\IRS" + branchcode + string.Format("{0:MMddyyyy}", System.DateTime.Now.Date) + "_" + MSG[0].ToString() + ".txt";
                        myFileName = di.FullName + myFileName;

                        FileInfo f = new FileInfo(myFileName);
                        StreamWriter s = f.CreateText();
                        s.WriteLine(branchcode + "~" + MSG[2].ToString() + "~" + MSG[0].ToString());
                        s.Close();
                        audit.writeAudit(clientdata.UserId, "Batch Approval", "Update Approving Officer", clientdata.UserId + " " + branchcode + "~" + MSG[2].ToString() + "~" + MSG[0].ToString(), myFileName, csAuditLog.Category.FlexCube);
                    }

                    string returnMessage = null;
                    returnMessage = "SUMMARY \r ";

                    if (MSG[2].ToString() != "0")
                    {
                        returnMessage += " Online: " + MSG[2].ToString() + " \r ";
                    }
                    if (MSG[1].ToString() != "0")
                    {
                        returnMessage += " Non Online: " + MSG[1].ToString() + " \r ";
                    }

                    returnMessage += " Batch Number: " + MSG[0].ToString() + " \r ";

                    if ((MSG[1].ToString() != "0") || (MSG[2].ToString() != "0"))
                    { return returnMessage; }

                    return " No Record(s) processed";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return Message;
        }

        public int Status(int seq_id)
        {
            string retVal = null;
            try
            {
                try
                {
                    Object o = base.execScalar("SELECT transtatus FROM tbl_Remittance WHERE seq_id=" + seq_id.ToString());
                    if (o != DBNull.Value)
                    {
                        retVal = Convert.ToString(o);
                    }
                }
                catch (System.Exception)
                { }

                if (retVal == null || retVal.Length == 0)
                    return -1;	//doesn't exist
                else
                    return int.Parse(retVal);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        public int Status(string refno)
        {
            string retVal = null;
            try
            {
                try
                {
                    Object o = base.execScalar("SELECT transtatus FROM tbl_Remittance WHERE referenceno=" + refno.ToString());
                    if (o != DBNull.Value)
                    {
                        retVal = Convert.ToString(o);
                    }
                }
                catch (System.Exception)
                { }

                if (retVal == null || retVal.Length == 0)
                    return -1;	//doesn't exist
                else
                    return int.Parse(retVal);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        public int check_FCR_setting(string branchcode)
        {
            string retVal = null;
            try
            {
                try
                {
                    Object o = base.execScalar("select  count(*) from tbl_FCR_Settings where settingsid='" + branchcode.ToString() + "'");
                    if (o != DBNull.Value)
                    {
                        retVal = Convert.ToString(o);
                    }
                }
                catch (System.Exception)
                { }
                if (retVal == null || retVal.Length == 0)
                    return -1;	//doesn't exist
                else
                    return 1;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            { }
        }

        public DataSet Remit2(int seqid)
        {
            string branchcode = null;
            int currency = 0;
            string referenceno = null;
            string targetaccountno = null;
            double amount = 0;
            int transtatus = 0;
            int productcode = 0;
            int implemtype = 0;
            int paymode = 0;
            double GrossRemAmount = 0;
            string sourceaccountno = null;
            int sourcecurrency = 0;
            string currencycode = null;
            string sourcecurrencycode = null;
            string newFCaccount = null;
            DataSet paramDs = new DataSet("remit");
            string transmessage = string.Empty;

            try
            {
                if (new irsAccountInquiry().isForApprovalTag())
                {
                    int status = this.Status(seqid);
                    transmessage = "For Approval Tag is enabled for this branch";
                    if (status != (int)Trans_Status.On_Hold && status != (int)Trans_Status.For_Approval && status != (int)Trans_Status.For_Reprocess)
                    {
                        paramDs = Update_Status(seqid, (int)Trans_Status.For_Approval);
                        audit.writeAudit(clientdata.UserId, "Fund Transfer", "FOR APPROVAL TAG ENABLED " + seqid.ToString(), "Remit2", transmessage, csAuditLog.Category.FlexCube);
                        return ParseMessage(paramDs, transmessage);
                    }
                    return ParseMessage(status, transmessage);
                }

                DataTable dt = get_remit_details(seqid);
                foreach (DataRow dr in dt.Rows)
                {
                    referenceno = dr["ReferenceNo"].ToString();
                    branchcode = dr["RemittingBranch"].ToString();
                    transtatus = int.Parse(dr["Transtatus"].ToString());
                    productcode = int.Parse(dr["productcode"].ToString());
                    implemtype = int.Parse(dr["implementationtype"].ToString());
                    targetaccountno = dr["AccountNo"].ToString();
                    amount = double.Parse(dr["NettoBeneAmount"].ToString());
                    currency = int.Parse(dr["NettoBeneCurrency"].ToString());
                    currencycode = dr["NettoBeneCurrencyCode"].ToString();
                    paymode = int.Parse(dr["paymode"].ToString());
                    sourceaccountno = dr["sourceaccountno"].ToString();
                    sourcecurrency = int.Parse(dr["RemittingCurrency"].ToString());
                    sourcecurrencycode = dr["RemittingCurrencyCode"].ToString();
                    GrossRemAmount = double.Parse(dr["GrossRemAmount"].ToString());
                }

                if (transtatus == (int)Trans_Status.On_Hold)
                {
                    transmessage = "On Hold Transaction";
                    return ParseMessage(transtatus, transmessage);
                }

                if (implemtype == 2)
                {
                    irsAccountInquiry localinquiry = new irsAccountInquiry();
                    newFCaccount = localinquiry.getFC(targetaccountno, irsAccountInquiry.FCMethod.KBItoFC, 0, true);
                    /* PNBS Integration Project */
                    if (targetaccountno != newFCaccount)
                    {
                        int ignore = 0;
                        if (Int32.TryParse(newFCaccount, out ignore))
                        {
                            this.Update_CIF_New_Account(seqid, referenceno, targetaccountno, newFCaccount);
                            targetaccountno = newFCaccount;
                        }
                    }
                }
                return Remit2(seqid, branchcode, currency, referenceno, targetaccountno, amount, transtatus, productcode, implemtype, paymode, sourceaccountno, sourcecurrency, GrossRemAmount, currencycode, sourcecurrencycode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
        }

        public string chk_benef_account_registered(string client_cifid, int account_seq_id)
        {
            DataTable dt = null;

            try
            {
                if (string.IsNullOrEmpty(client_cifid).Equals(true))
                {
                    return string.Empty;
                }

                if (account_seq_id < 1)
                {
                    return string.Empty;
                }

                dt = getDataSet("chk_benef_account_registered", true, new object[] { client_cifid, account_seq_id }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ben_acct_info"] != null)
                    {
                        if (dt.Rows[0]["ben_acct_info"].ToString().ToUpper().Equals("INVALID"))
                        {
                            audit.writeAudit("", "BENEF_VALIDATE_ACCTSEQ", dt.Rows[0]["account_message"].ToString(), client_cifid, account_seq_id.ToString(), csAuditLog.Category.Insert); return dt.Rows[0]["account_message"].ToString();
                        }
                    }
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /* TO VALIDATE IF ENROLLED RAPID REMIT ACCOUNT IS VALID BENEF ACCOUNT AND NOT APAS */
        public bool chk_if_apas_account_enrolled_as_rapid(string account_no)
        {
            DataTable dt = null;
            string string_filter = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(account_no).Equals(true))
                {
                    return false;
                }

                string_filter = "select count(1) cnt_rec from tbl_lookup where fldgroup = 'APAS_Online' and fldcode = '" + account_no.Trim() + "'";

                dt = base.getDataSet(string_filter, false, new object[] { }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["cnt_rec"]) > 0)
                    {
                        return true; //DO NOT ALLOW IF ACCOUNT IS APAS CK ENROLLED AS RAPID REMIT
                    }
                }
                return false;
            }
            catch
            {
                return false;
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

        private DataSet RemitToAgentRapidRemitApasDS(int transaction_id)
        {
            string transaction_status = string.Empty;
            string transaction_status_desc = string.Empty;

            DataSet paramDs = new DataSet();
            DataTable dtStatus = new DataTable();
            DataRow dr;

            try
            {
                transaction_status = this.Status(transaction_id).ToString();
                transaction_status_desc = Convert.ToString(base.execScalar("select flddescription from tbl_lookup where fldgroup = 'tranxstatus' and fldvalue = " + transaction_status));

                dtStatus.Columns.Add("Message");
                dtStatus.Columns.Add("Transtatus");
                dtStatus.Columns.Add("TranStatusDesc");

                dr = dtStatus.NewRow();
                dr[0] = "APAS account is not valid for Rapid Remit transaction.";
                dr[1] = transaction_status;
                dr[2] = transaction_status_desc;

                dtStatus.Rows.Add(dr);
                paramDs.Tables.Add(dtStatus);

                return paramDs;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (paramDs != null)
                {
                    paramDs.Dispose();
                    paramDs = null;
                }

                if (dtStatus != null)
                {
                    dtStatus.Dispose();
                    dtStatus = null;
                }
            }
        }
        /* END VALIDATION/

        /* START DDC JUNE 08 2016 IF TRANSACTION IS ON HOLD DO NOT ALLOW FUND TRANSFER TO AVOID DOUBLE IMPLEMENTATION */
        private bool RemitToAgentIsHoldStat(int seqid)
        {
            DataTable dt = null;

            int transaction_status = 0;

            try
            {
                transaction_status = this.Status(seqid);
            }
            catch
            {
                return false;
            }

            if (transaction_status != 14)
            {
                return false;
            }

            try
            {
                dt = new irsInternetPage().getRemittanceInfo(seqid);

                int product_code = 0;
                int remit_method = 0;
                int implementation_type = 0;

                if (dt != null && dt.Rows.Count > 0)
                {
                    product_code = Convert.ToInt32(dt.Rows[0]["ProductCode"].ToString().Trim());
                    remit_method = Convert.ToInt32(dt.Rows[0]["RemitMethod"].ToString().Trim());
                    implementation_type = Convert.ToInt32(dt.Rows[0]["ImplementationType"].ToString().Trim());
                }

                if ((implementation_type == (int)ImplemType.CreditToAccount) || (product_code == (int)ProductCode.PAW) || (product_code == (int)ProductCode.APASOnline))
                {
                    if (remit_method == 8 && transaction_status == 14)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private DataSet RemitToAgentIsHoldStatDs()
        {
            DataSet paramDs = new DataSet();
            DataTable dtStatus = new DataTable();
            DataRow dr;

            try
            {
                dtStatus.Columns.Add("Message");
                dtStatus.Columns.Add("Transtatus");
                dtStatus.Columns.Add("TranStatusDesc");

                dr = dtStatus.NewRow();
                dr[0] = "Transaction is for clearing. (Hold)";
                dr[1] = 14;
                dr[2] = "Hold";

                dtStatus.Rows.Add(dr);
                paramDs.Tables.Add(dtStatus);

                return paramDs;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (paramDs != null)
                {
                    paramDs.Dispose();
                    paramDs = null;
                }

                if (dtStatus != null)
                {
                    dtStatus.Dispose();
                    dtStatus = null;
                }
            }
        }
        /* END DDC JUNE 08 2016 IF TRANSACTION IS ON HOLD DO NOT ALLOW FUND TRANSFER TO AVOID DOUBLE IMPLEMENTATION */

        public DataSet RemitToAgent(int seqid)
        {
            DataSet paramDs = new DataSet();
            string sError = string.Empty;
            int retVal = 0;
            string sRetVal = string.Empty;
            string sDebit = string.Empty;
            string sCredit = string.Empty;
            string sTargetCurrency = string.Empty;
            string sReferenceNo = string.Empty;
            int implementationType = 0;
            int productCode = 0;
            int remitmethod = 0;
            DataSet ds = new DataSet();
            DataTable dtStatus = new DataTable();
            DataTable dt = new DataTable();
            DataRow dr;
            bool isFCOnline = false;
            bool isOnline = false;
            string debitReturn = string.Empty;
            string debitReturn2 = string.Empty;
            string debitReturn3 = string.Empty;
            irsOldAPAS apas = new irsOldAPAS();
            string TempBCAccount = string.Empty;
            string BCAccount = string.Empty;
            string err = string.Empty;
            string accountNo = string.Empty;

            try
            {
                /* START DDC JUNE 08 2016 */
                if (this.RemitToAgentIsHoldStat(seqid) == true)
                {
                    return this.RemitToAgentIsHoldStatDs();
                }
                /* END DDC JUNE 08 2016 */

                dt = new irsInternetPage().getRemittanceInfo(seqid);
                sTargetCurrency = dt.Rows[0]["NettoBeneCurrency"].ToString().Trim();
                sDebit = get_nsvs(dt.Rows[0]["RemittingBranch"].ToString().Trim(), Convert.ToInt32(sTargetCurrency));
                productCode = Convert.ToInt32(dt.Rows[0]["ProductCode"].ToString().Trim());
                remitmethod = Convert.ToInt32(dt.Rows[0]["RemitMethod"].ToString().Trim());
                implementationType = Convert.ToInt32(dt.Rows[0]["ImplementationType"].ToString().Trim());
                accountNo = dt.Rows[0]["AccountNo"].ToString();

                /* TRAP APAS CK PROCESSED AS RAPID REMIT TRANSACTION */
                if (productCode == (int)ProductCode.RapidRemit && implementationType == (int)ImplemType.CreditToAccount)
                {
                    if (this.chk_if_apas_account_enrolled_as_rapid(dt.Rows[0]["AccountNo"].ToString().Trim()) == true)
                    {
                        return this.RemitToAgentRapidRemitApasDS(seqid);
                    }
                }
                /* END */

                if ((implementationType == (int)ImplemType.CreditToAccount) || (productCode == (int)ProductCode.PAW) || (productCode == (int)ProductCode.APASOnline))
                {
                    /* PNBS Integration Project */
                    isOnline = true;
                }
                else
                {
                    isOnline = false;
                }

                if (isOnline == true)
                {
                    if (productCode == 7 || productCode == 27) //APAS
                    {
                        sCredit = new irsInternetPage().getLookupValue("APAS_Online", "FldValue", dt.Rows[0]["NettoBeneCurrency"].ToString().Trim(), "FldCode");
                    }
                    else
                    {
                        sCredit = dt.Rows[0]["AccountNo"].ToString().Trim();
                    }
                }
                else
                {
                    sCredit = new irsInternetPage().getLookupValue("AgentCKTempo", "FldValue", sTargetCurrency, "FldDescription");
                }
                if (remitmethod == 5)
                {
                    if (isOnline == true)
                    {
                        sReferenceNo = dt.Rows[0]["ReferenceNo"].ToString().Trim();
                        isFCOnline = new irsAccountInquiry().testFC(out err);
                        if (isFCOnline)
                        {
                            if (isFCOnline == true)
                            {
                                retVal = fundtransfer(sReferenceNo, sDebit.Trim(), sCredit.Trim(), Convert.ToDouble(dt.Rows[0]["NettoBeneAmount"].ToString().Trim()), "IRSW (" + dt.Rows[0]["ReferenceNo"].ToString().Trim() + ")");
                            }
                            else
                            {
                                retVal = 0;
                            }

                            if (ErrorMessage != null)
                            {
                                sError = ErrorMessage.ToString();
                            }

                            if (retVal == 1)
                            {
                                this.Update_FC_Message(seqid, AccountMessage.ToString());
                                if (productCode == 7 || productCode == 27) //APAS
                                {
                                    if (apas.InsertApas(seqid))
                                    {
                                        audit.writeAudit(clientdata.UserId, "InsertApas", "Location=" + clientdata.Location + "[TransactionSEQID=" + seqid.ToString() + "][ReferenceNo=" + sReferenceNo + "][IsFCOnline=" + isFCOnline.ToString() + "][ReturnValue=" + retVal.ToString() + "][APIMessage=" + sRetVal + "][ErrorMessage=" + sError + "]", "Successfully sent to APAS DB.", "", csAuditLog.Category.Insert);
                                    }
                                    else
                                    {
                                        audit.writeAudit(clientdata.UserId, "InsertApas", "Location=" + clientdata.Location + "[TransactionSEQID=" + seqid.ToString() + "][ReferenceNo=" + sReferenceNo + "][IsFCOnline=" + isFCOnline.ToString() + "][ReturnValue=" + retVal.ToString() + "][APIMessage=" + sRetVal + "][ErrorMessage=" + sError + "]", "Error: " + apas.APASMessage.ToString(), "", csAuditLog.Category.Insert);
                                    }
                                }
                                if (isOnline == true)
                                {
                                    paramDs = Update_Status(seqid, (int)Trans_Status.Credited_Online);
                                }
                                else
                                {
                                    if (isFCOnline == true)
                                    {
                                        dtStatus.Columns.Add("Transtatus");
                                        dtStatus.Columns.Add("TranStatusDesc");
                                        dr = dtStatus.NewRow();
                                        dr[0] = AccountMessage.ToString();
                                        dr[1] = (int)Trans_Status.Credited_Online;
                                        dr[2] = "Credited Online";
                                        dtStatus.Rows.Add(dr);
                                        paramDs.Tables.Add(dtStatus);
                                    }
                                    else
                                    {
                                        this.Update_FC_Message(seqid, this.Get_FC_Description(myErrorMessage.ToString()));
                                        paramDs = Update_Status(seqid, (int)Trans_Status.For_Batch_Processing);
                                    }
                                }
                            }
                            else
                            {
                                if (sError != string.Empty)
                                {
                                    this.Update_FC_Message(seqid, this.Get_FC_Description(ErrorMessage.ToString()));
                                }
                                /* E2EAPICHANGE 20220209 Changed MQRC */
                                else
                                {
                                    this.Update_FC_Message(seqid, "E2E API Timed-out");
                                }
                                if (isOnline == true)
                                {
                                    if (isFCOnline == true)
                                    {
                                        paramDs = Update_Status(seqid, (int)Trans_Status.For_Reprocess);
                                    }
                                    else
                                    {
                                        paramDs = Update_Status(seqid, (int)Trans_Status.For_Batch_Processing);
                                    }
                                }
                                else
                                {
                                    paramDs = Update_Status(seqid, (int)Trans_Status.For_Batch_Processing);
                                }
                            }
                        }
                        else
                        {
                            //Store and Forward TROY 20140307. RFS #14051-001.
                            paramDs = Update_Status(seqid, (int)Trans_Status.For_Batch_Processing);
                            sError = "Transaction will be in Store and Forward mode.";
                        }
                        if (paramDs.Tables[0].Rows.Count <= 1)
                        {
                            dtStatus.Columns.Add("Message");
                            dtStatus.Columns.Add("Transtatus");
                            dtStatus.Columns.Add("TranStatusDesc");
                            dr = dtStatus.NewRow();
                            if (sError != string.Empty)
                            {
                                if (ErrorMessage != null)
                                {
                                    dr[0] = Get_FC_Description(ErrorMessage.ToString());
                                }
                                else
                                {
                                    dr[0] = sError;
                                }
                            }
                            else
                            {
                                dr[0] = AccountMessage.ToString();
                            }
                            dr[1] = paramDs.Tables[0].Rows[0][0].ToString().Trim();
                            dr[2] = paramDs.Tables[0].Rows[0][1].ToString().Trim();
                            dtStatus.Rows.Add(dr);
                            ds.Tables.Add(dtStatus);
                            paramDs = null;
                            paramDs = ds;
                        }
                    }
                    else
                    {
                        paramDs = Update_Status(seqid, (int)Trans_Status.Posted_Central);
                    }
                }
                else
                {
                    sReferenceNo = dt.Rows[0]["ReferenceNo"].ToString().Trim();
                    isFCOnline = new irsAccountInquiry().testFC(out err);

                    if (isFCOnline == true)
                    {
                        if (Convert.ToDouble(dt.Rows[0]["total_backend_charges"].ToString().Trim()) > 0)
                        {
                            BCAccount = new irsInternetPage().getLookupValue("BCAccount", "FldValue", sTargetCurrency, "FldDescription");
                            TempBCAccount = new irsInternetPage().getLookupValue("AgentCKTempo", "FldValue", sTargetCurrency, "FldDescription");

                            if (isOnline == true)
                            {
                                retVal = fundtransfer(sReferenceNo, sDebit.Trim(), TempBCAccount.Trim(), Convert.ToDouble(dt.Rows[0]["NettoBeneAmount"].ToString().Trim()), "IRSW (" + dt.Rows[0]["ReferenceNo"].ToString().Trim() + ")");
                                if (retVal == 1)
                                {
                                    retVal = fundtransfer(sReferenceNo, TempBCAccount.Trim(), BCAccount.Trim(), Convert.ToDouble(dt.Rows[0]["total_backend_charges"].ToString().Trim()), "IRSW (" + dt.Rows[0]["ReferenceNo"].ToString().Trim() + ")");
                                    if (retVal == 1)
                                    {
                                        retVal = fundtransfer(sReferenceNo, TempBCAccount.Trim(), sCredit.Trim(), Convert.ToDouble(dt.Rows[0]["NettoBeneAmount"].ToString().Trim()) - Convert.ToDouble(dt.Rows[0]["total_backend_charges"].ToString().Trim()), "IRSW (" + dt.Rows[0]["ReferenceNo"].ToString().Trim() + ")");
                                    }
                                }
                            }
                            else
                            {
                                retVal = fundtransfer(sReferenceNo, sDebit.Trim(), sCredit.Trim(), Convert.ToDouble(dt.Rows[0]["NettoBeneAmount"].ToString().Trim()), "IRSW (" + dt.Rows[0]["ReferenceNo"].ToString().Trim() + ")");
                                if (retVal == 1)
                                {
                                    retVal = fundtransfer(sReferenceNo, sCredit.Trim(), BCAccount.Trim(), Convert.ToDouble(dt.Rows[0]["NettoBeneAmount"].ToString().Trim()) - Convert.ToDouble(dt.Rows[0]["total_backend_charges"].ToString().Trim()), "IRSW (" + dt.Rows[0]["ReferenceNo"].ToString().Trim() + ")");
                                }
                            }
                        }
                        else
                        {
                            retVal = fundtransfer(sReferenceNo, sDebit.Trim(), sCredit.Trim(), Convert.ToDouble(dt.Rows[0]["NettoBeneAmount"].ToString().Trim()), "IRSW (" + dt.Rows[0]["ReferenceNo"].ToString().Trim() + ")");
                        }
                    }
                    else
                    {
                        retVal = 0;
                    }

                    if (ErrorMessage != null)
                    {
                        sError = ErrorMessage.ToString();
                    }

                    if (retVal == 1)
                    {
                        this.Update_FC_Message(seqid, AccountMessage.ToString());
                        if (productCode == 7 || productCode == 27) //APAS
                        {
                            if (apas.InsertApas(seqid))
                            {
                                audit.writeAudit(clientdata.UserId, "InsertApas", "Location=" + clientdata.Location + "[TransactionSEQID=" + seqid.ToString() + "][ReferenceNo=" + sReferenceNo + "][IsFCOnline=" + isFCOnline.ToString() + "][ReturnValue=" + retVal.ToString() + "][APIMessage=" + sRetVal + "][ErrorMessage=" + sError + "]", "Successfully sent to APAS DB.", "", csAuditLog.Category.Insert);
                            }
                            else
                            {
                                audit.writeAudit(clientdata.UserId, "InsertApas", "Location=" + clientdata.Location + "[TransactionSEQID=" + seqid.ToString() + "][ReferenceNo=" + sReferenceNo + "][IsFCOnline=" + isFCOnline.ToString() + "][ReturnValue=" + retVal.ToString() + "][APIMessage=" + sRetVal + "][ErrorMessage=" + sError + "]", "Error: " + apas.APASMessage.ToString(), "", csAuditLog.Category.Insert);
                            }
                        }
                        if (isOnline == true)
                        {
                            paramDs = Update_Status(seqid, (int)Trans_Status.Credited_Online);
                        }
                        else
                        {
                            if (isFCOnline == true)
                            {
                                dtStatus.Columns.Add("Message");
                                dtStatus.Columns.Add("Transtatus");
                                dtStatus.Columns.Add("TranStatusDesc");
                                dr = dtStatus.NewRow();
                                dr[0] = AccountMessage.ToString();
                                dr[1] = (int)Trans_Status.Sent_To_GRS;
                                dr[2] = "For I-Batch Processing";
                                dtStatus.Rows.Add(dr);
                                paramDs.Tables.Add(dtStatus);
                                Update_Status(seqid, (int)Trans_Status.Sent_To_GRS);
                                DataTable dtNonOnline = execStoredProcedureTable("usp_agentNonOnline", new object[] { seqid });
                            }
                            else
                            {
                                this.Update_FC_Message(seqid, this.Get_FC_Description(myErrorMessage.ToString()));
                                paramDs = Update_Status(seqid, (int)Trans_Status.For_Batch_Processing);
                            }
                        }
                    }
                    else
                    {
                        /* FOR CBIP 20170825 */
                        if (err != string.Empty)
                        {
                            int i = 0;
                            DataTable dtBlackoutMode = new irsAccountInquiry().BlackOutMode();
                            if (dtBlackoutMode != null && dtBlackoutMode.Rows.Count > 0)
                            {
                                for (i = 0; i <= (dtBlackoutMode.Rows.Count - 1); i++)
                                {
                                    if (err.IndexOf(dt.Rows[i]["FldDescription"].ToString().Trim()) >= 0)
                                    {
                                        sError = string.Empty;
                                        myAccountMessage = "Transaction will be in Store and Forward mode.";
                                        isFCOnline = false;
                                        break;
                                    }
                                }
                            }
                        }
                        else if (sError != string.Empty && sError.IndexOf("Transaction already serviced") >= 0)
                        {
                            sError = string.Empty;
                            myAccountMessage = "Transaction already serviced";
                            isFCOnline = true;
                        }
                        else if (sError != string.Empty && (sError.IndexOf("Unable to connect to the remote server") >= 0 || sError.IndexOf("Error in the application") >= 0 || sError.IndexOf("2 - 2009") >= 0 || sError.IndexOf("NOT_AVAILABLE") >= 0))
                        {
                            sError = string.Empty;
                            myAccountMessage = null;
                            isFCOnline = false;
                        }
                        if (sError != string.Empty)
                        {
                            if (ErrorMessage.ToString().IndexOf("Refer to Drawer ( Account would Overdraw )") >= 0)
                            {
                                this.Update_FC_Message(seqid, "Credit Line Expired");
                            }
                            else
                            {
                                this.Update_FC_Message(seqid, this.Get_FC_Description(ErrorMessage.ToString()));
                                /*
                                if (sError.IndexOf("Overline available exceeding limit Amount") >= 0)
                                {
                                    this.reversalGFC(seqid);
                                }
                                */
                            }
                        }
                        else
                        {
                            //this.Update_FC_Message(seqid, this.Get_FC_Description("MQRC_NO_MSG_AVAILABLE: 2 - 2033"));
                            this.Update_FC_Message(seqid, "");
                        }
                        if (isOnline == true)
                        {
                            if (isFCOnline == true)
                            {
                                if (myAccountMessage == "Transaction for verification.")
                                {
                                    paramDs = Update_Status(seqid, (int)Trans_Status.For_Reprocess);
                                }
                                else if (myAccountMessage == "Transaction already serviced")
                                {
                                    paramDs = Update_Status(seqid, (int)Trans_Status.Credited_Online);
                                }
                                else
                                {
                                    paramDs = Update_Status(seqid, (int)Trans_Status.Rejected);
                                }
                            }
                            else
                            {
                                paramDs = Update_Status(seqid, (int)Trans_Status.For_Batch_Processing);
                            }
                        }
                        else
                        {
                            if (isFCOnline == true)
                            {
                                if (myAccountMessage == "Transaction for verification.")
                                {
                                    paramDs = Update_Status(seqid, (int)Trans_Status.For_Reprocess);
                                }
                                else if (myAccountMessage == "Transaction already serviced")
                                {
                                    dtStatus.Columns.Add("Message");
                                    dtStatus.Columns.Add("Transtatus");
                                    dtStatus.Columns.Add("TranStatusDesc");
                                    dr = dtStatus.NewRow();
                                    dr[0] = AccountMessage.ToString();
                                    dr[1] = (int)Trans_Status.Sent_To_GRS;
                                    dr[2] = "For I-Batch Processing";
                                    dtStatus.Rows.Add(dr);
                                    paramDs.Tables.Add(dtStatus);
                                    Update_Status(seqid, (int)Trans_Status.Sent_To_GRS);
                                    DataTable dtNonOnline = execStoredProcedureTable("usp_agentNonOnline", new object[] { seqid });
                                }
                                else
                                {
                                    paramDs = Update_Status(seqid, (int)Trans_Status.Rejected);
                                }
                            }
                            else
                            {
                                paramDs = Update_Status(seqid, (int)Trans_Status.For_Batch_Processing);
                            }
                        }
                    }
                    if (paramDs.Tables[0].Rows.Count <= 1)
                    {
                        if (dtStatus.Columns.Count <= 1)
                        {
                            dtStatus.Columns.Add("Message");
                            dtStatus.Columns.Add("Transtatus");
                            dtStatus.Columns.Add("TranStatusDesc");
                        }
                        dr = dtStatus.NewRow();
                        if (sError != string.Empty)
                        {
                            if (ErrorMessage.ToString().IndexOf("Refer to Drawer ( Account would Overdraw )") >= 0)
                            {
                                dr[0] = "Credit Line Expired";
                            }
                            else
                            {
                                dr[0] = Get_FC_Description(ErrorMessage.ToString());
                            }
                        }
                        else
                        {
                            if (AccountMessage == null)
                            {
                                dr[0] = "Store and Forward is enabled. Transaction was succesfully saved.";
                            }
                            else
                            {
                                dr[0] = AccountMessage.ToString();
                            }
                        }
                        if (paramDs.Tables[0].Columns.Count > 2)
                        {
                            dr[1] = paramDs.Tables[0].Rows[0][1].ToString().Trim();
                            dr[2] = paramDs.Tables[0].Rows[0][2].ToString().Trim();
                        }
                        else
                        {
                            dr[1] = paramDs.Tables[0].Rows[0][0].ToString().Trim();
                            dr[2] = paramDs.Tables[0].Rows[0][1].ToString().Trim();
                        }
                        dtStatus.Rows.Add(dr);
                        ds.Tables.Add(dtStatus.Copy());
                        paramDs = null;
                        paramDs = ds;
                    }
                }
            }
            catch (Exception ex)
            {
                sError += ex.Message;
                if (isOnline == true)
                {
                    paramDs = Update_Status(seqid, (int)Trans_Status.For_Reprocess);
                }
                else
                {
                    paramDs = Update_Status(seqid, (int)Trans_Status.For_Reprocess);
                }
                new csAuditLog().writeAudit(((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId, "RemitToAgent", ex.Message, "", "", csAuditLog.Category.FlexCube);
            }
            if (AccountMessage != null)
            {
                sRetVal = AccountMessage.ToString();
            }
            audit.writeAudit(clientdata.UserId, "Agent Fund Transfer", "Location=" + clientdata.Location + "[TransactionSEQID=" + seqid.ToString() + "][ReferenceNo=" + sReferenceNo + "][IsFCOnline=" + isFCOnline.ToString() + "][ReturnValue=" + retVal.ToString() + "][APIMessage=" + sRetVal + "][ErrorMessage=" + sError + "]", paramDs.Tables[0].Rows[0][0].ToString() + "|" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "|" + paramDs.Tables[0].Rows[0][2].ToString().Trim(), "", csAuditLog.Category.FlexCube);
            return paramDs;
        }

        public DataSet RemitToBayadCenter(int seqid)
        {
            DataSet paramDs = new DataSet();
            string sError = string.Empty;
            string sRetVal = string.Empty;
            string sDebit = string.Empty;
            string sCredit = string.Empty;
            string sTargetCurrency = string.Empty;
            string sReferenceNo = string.Empty;
            DataSet ds = new DataSet();
            DataTable dtStatus = new DataTable();
            DataTable dt = new DataTable();
            DataRow dr;

            paramDs = Update_Status(seqid, (int)Trans_Status.For_Reprocess);

            if (paramDs.Tables[0].Rows.Count <= 1)
            {
                if (dtStatus.Columns.Count <= 1)
                {
                    dtStatus.Columns.Add("Message");
                    dtStatus.Columns.Add("Transtatus");
                    dtStatus.Columns.Add("TranStatusDesc");
                }
                dr = dtStatus.NewRow();

                dr[0] = (string)System.Web.HttpContext.Current.Session["OBPSBayadCenterResponseMessage"];
                if (paramDs.Tables[0].Columns.Count > 2)
                {
                    dr[1] = paramDs.Tables[0].Rows[0][1].ToString().Trim();
                    dr[2] = paramDs.Tables[0].Rows[0][2].ToString().Trim();
                }
                else
                {
                    dr[1] = paramDs.Tables[0].Rows[0][0].ToString().Trim();
                    dr[2] = paramDs.Tables[0].Rows[0][1].ToString().Trim();
                }
                dtStatus.Rows.Add(dr);
                ds.Tables.Add(dtStatus.Copy());
                paramDs = null;
                paramDs = ds;
            }
            return paramDs;
        }

        /* E2EAPICHANGE 20220209 REMOVE getAvailableBalance */

        public DataSet Remit2(int seqid, bool isOTC)
        {
            string branchcode = null;
            int currency = 0;
            string referenceno = null;
            string targetaccountno = null;
            double amount = 0;
            int transtatus = 0;
            int productcode = 0;
            int implemtype = 0;
            int paymode = 0;
            double GrossRemAmount = 0;
            string sourceaccountno = null;
            int sourcecurrency = 0;
            string currencycode = null;
            string sourcecurrencycode = null;
            string newFCaccount = null;
            DataSet paramDs = new DataSet("remit");
            string transmessage = string.Empty;

            try
            {
                if (new irsAccountInquiry().isForApprovalTag())
                {
                    int status = this.Status(seqid);
                    transmessage = "For Approval Tag is enabled for this branch";
                    if (status != (int)Trans_Status.On_Hold && status != (int)Trans_Status.For_Approval && status != (int)Trans_Status.For_Reprocess)
                    {
                        paramDs = Update_Status(seqid, (int)Trans_Status.For_Approval);
                        audit.writeAudit(clientdata.UserId, "Fund Transfer", "FOR APPROVAL TAG ENABLED " + seqid.ToString(), "Remit2", transmessage, csAuditLog.Category.FlexCube);
                        return ParseMessage(paramDs, transmessage);
                    }
                    return ParseMessage(status, transmessage);
                }

                DataTable dt = get_remit_details(seqid);
                foreach (DataRow dr in dt.Rows)
                {
                    referenceno = dr["ReferenceNo"].ToString();
                    branchcode = dr["RemittingBranch"].ToString();
                    transtatus = int.Parse(dr["Transtatus"].ToString());
                    productcode = int.Parse(dr["productcode"].ToString());
                    implemtype = int.Parse(dr["implementationtype"].ToString());
                    targetaccountno = dr["AccountNo"].ToString();
                    amount = double.Parse(dr["NettoBeneAmount"].ToString());
                    currency = int.Parse(dr["NettoBeneCurrency"].ToString());
                    currencycode = dr["NettoBeneCurrencyCode"].ToString();
                    paymode = int.Parse(dr["paymode"].ToString());
                    sourceaccountno = dr["sourceaccountno"].ToString();
                    sourcecurrency = int.Parse(dr["RemittingCurrency"].ToString());
                    sourcecurrencycode = dr["RemittingCurrencyCode"].ToString();
                    GrossRemAmount = double.Parse(dr["GrossRemAmount"].ToString());
                }

                if (transtatus == (int)Trans_Status.On_Hold)
                {
                    transmessage = "On Hold Transaction";
                    return ParseMessage(transtatus, transmessage);
                }

                if (implemtype == 2)
                {
                    irsAccountInquiry localinquiry = new irsAccountInquiry();
                    newFCaccount = localinquiry.getFC(targetaccountno, irsAccountInquiry.FCMethod.KBItoFC, 0, true);
                    /* PNBS Integration Project */
                    if (targetaccountno != newFCaccount)
                    {
                        int ignore = 0;
                        if (Int32.TryParse(newFCaccount, out ignore))
                        {
                            this.Update_CIF_New_Account(seqid, referenceno, targetaccountno, newFCaccount);
                            targetaccountno = newFCaccount;
                        }
                    }
                }
                return Remit2(seqid, branchcode, currency, referenceno, targetaccountno, amount, transtatus, productcode, implemtype, currencycode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
        }

        /* E2EAPICHANGE 20220209 REMOVE Debit */

        /* E2EAPICHANGE 20220209 REMOVE isDebitSuccessfull */

        // Pending Override and For Approval //
        public DataSet Remit2(int tranxid, string branchcode, int currency, string referenceno, string targetaccountno, double amount, int transtatus, int productcode, int implemtype, int paymode, string sourceaccountno, int sourcecurrency, double GrossRemAmount, string currencycode, string sourcecurrencycode)
        {
            string narrative = "IRSW (" + referenceno + ")";
            //int tranxid = int.Parse(System.Web.HttpContext.Current.Session["TranxID"].ToString());
            string transmessage = "";
            DataSet paramDs = new DataSet("remit");
            string OBPExcess = "";
            irsOBPSTransactions obp = new irsOBPSTransactions();

            //INSTAPAY ALYSSA SEPTEMBER 2019 START
            E2EInstapay instapay = new E2EInstapay();
            /* E2EAPICHANGE 20220209 Removed unused variables */
            string UserReferenceNo = null;
            string TransactingBranch = null;
            string ChannelId = null;
            string AcctName = null;
            string AcctNo = null;
            string AcctIdent = null;
            string TfrName = null;
            string Param1 = null;
            string Param2 = null;
            string Param3 = null;
            string CustNo = null;
            string Purpose = null;
            string Cycode = null;
            string TfrBnkCode = null;
            string TfrAcctNo = null;
            string TxnAmount = null;
            string BnkCode = null;
            string AcctType = null;
            string TxnDate = null;
            string TxnTime = null;
            /* E2EAPICHANGE 20220209 Removed unused variables */
            string E2ERequest = string.Empty;
            string retval_instapay = string.Empty;
            string res = string.Empty;
            //INSTAPAY ALYSSA SEPTEMBER 2019 END

            try
            {
                /* E2EAPICHANGE 20220209 REMOVE isDebittoLocalAccount */
                if ((implemtype == (int)ImplemType.CreditToAccount) || (productcode == (int)ProductCode.PAW) || (productcode == (int)ProductCode.APASOnline))
                {
                    /* CJ VALIDATION FOR CREDITED ONLINE */
                    if (transtatus == (int)Trans_Status.Credited_Online)
                    {
                        transmessage = "Already available and credited to Fund Transfer";
                        audit.writeAudit(clientdata.UserId, "FUND TRANSFER", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.Credited_Online + "][TranxStatus=" + transtatus.ToString() + "][Description=Credited Online][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                        return ParseMessage(paramDs, transmessage);
                    }
                    else
                    {
                        if (productcode == (int)ProductCode.PAW)
                        {
                            targetaccountno = GetPAWAccount(currency);
                        }

                        if (productcode == (int)ProductCode.APASOnline)
                        {
                            targetaccountno = GetApasAccount(currency);
                        }

                        /* E2EAPICHANGE 20220209 REMOVE throughGLAccount */
                        string NSVS = get_nsvs(branchcode, currency);
                        if (NSVS != null && NSVS != "" && targetaccountno != "")
                        {
                            paramDs = Update_Status(tranxid, (int)Trans_Status.For_Batch_Processing);
                            if (paramDs != null)
                            {
                                if (paramDs.Tables[0].Rows.Count > 0)
                                {
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.For_Batch_Processing.ToString() + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "]", "", "", csAuditLog.Category.Update);
                                }
                                else
                                {
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error=No return value]", "", "", csAuditLog.Category.Update);
                                }
                            }
                            else
                            {
                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error=No return value]", "", "", csAuditLog.Category.Update);
                            }

                            /*
                                if RunAgentCrediting key in Webconfig is false FC creditting will be in irsRemTeller.cs
                                if true, creditting will be in IRSWorldAgent                             
                            */

                            String swtch = this.GetAppSetting("RunAgentCrediting");
                            if (!Convert.ToBoolean(swtch))
                            {
                                /* FOR CBIP */
                                int retval = fundtransfer(referenceno.ToString(), NSVS.ToString().Trim(), targetaccountno.ToString().Trim(), amount, narrative.ToString());

                                if (retval == 1)
                                {
                                    this.Update_FC_Message(tranxid, AccountMessage.ToString());
                                    if (obp.fOBPExcess(tranxid))
                                    {
                                        OBPExcess = System.Web.HttpContext.Current.Session["OBPSExcess"].ToString();
                                        string OBPRefno = "O" + referenceno.ToString().Substring(1);
                                        string OBPNSVS = OBPExcess.Split('|')[2].ToString();
                                        string OBPAccount = OBPExcess.Split('|')[1].ToString();
                                        double OBPAmount = Convert.ToDouble(OBPExcess.Split('|')[4].ToString());
                                        string OBPNarrative = "IRSW (" + OBPRefno + ")";
                                        if (OBPAmount > 0)
                                        {
                                            retval = fundtransfer(OBPRefno, OBPNSVS, OBPAccount, OBPAmount, OBPNarrative);
                                            if (retval == 1)
                                            {
                                                paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                                transmessage = AccountMessage.ToString();
                                            }
                                            else
                                            {
                                                paramDs = Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                                                transmessage = ErrorMessage.ToString();
                                            }
                                        }
                                        else
                                        {
                                            paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                            transmessage = AccountMessage.ToString();
                                        }
                                        audit.writeAudit(clientdata.UserId, "Fund Transfer", "OBPS Excess Payments", OBPNarrative, transmessage, csAuditLog.Category.FlexCube);
                                    }
                                    else
                                    {
                                        paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                        transmessage = AccountMessage.ToString();
                                        //insert OLD apas/paw here
                                        //the key will be tranxid
                                        //new irsOldAPAS().InsertApas(tranxid);
                                        if (productcode == (int)ProductCode.APASOnline)
                                        {
                                            bool no_error = false;
                                            no_error = new irsOldAPAS().InsertApas(tranxid);
                                            //if (!no_error)
                                            //{
                                            //    new irsOldAPAS().InsertAPASCentral(tranxid);
                                            //}
                                        }
                                        else if (productcode == (int)ProductCode.PAW)
                                        {
                                            new irsOldAPAS().InsertAnPAW(tranxid);
                                        }
                                    }
                                }
                                else
                                {
                                    transmessage = this.Get_FC_Description(myErrorMessage.ToString());
                                    this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                                    paramDs = Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                                    /*
                                    if (productcode == (int)ProductCode.RapidRemit || productcode == (int)ProductCode.GFC)
                                    {
                                        reversalGFC(tranxid);
                                    }
                                    */
                                }
                            }
                            else
                            {
                                transmessage = "Transaction is successful";
                            }
                        }
                    }
                }

                //dito yung leg 1
                else if (productcode == (int)ProductCode.INSTAPAY) //INSTAPAY 
                {
                    Stopwatch watch = new Stopwatch();
                    string watchRes = string.Empty;
                    string NSVS = get_nsvs(branchcode, currency);
                    targetaccountno = get_instapay_nsvs(currency);

                    DataTable dt_instapay = get_instapay_tranx_details(tranxid);
                    foreach (DataRow dr in dt_instapay.Rows)
                    {
                        UserReferenceNo = dr["UserReferenceNo"].ToString();
                        TransactingBranch = dr["TransactingBranch"].ToString();
                        ChannelId = dr["ChannelId"].ToString();
                        AcctName = dr["AcctName"].ToString();
                        AcctNo = dr["AcctNo"].ToString();
                        AcctIdent = dr["AcctIdent"].ToString();
                        TfrName = dr["TfrName"].ToString();
                        Param1 = dr["Param1"].ToString();
                        Param2 = dr["Param2"].ToString();
                        Param3 = dr["Param3"].ToString();
                        CustNo = dr["CustNo"].ToString();
                        Purpose = dr["Purpose"].ToString();
                        Cycode = dr["Cycode"].ToString();
                        TfrBnkCode = dr["TfrBnkCode"].ToString();
                        TfrAcctNo = dr["TfrAcctNo"].ToString();
                        TxnAmount = dr["TxnAmount"].ToString();
                        BnkCode = dr["BnkCode"].ToString();
                        AcctType = dr["AcctType"].ToString();
                        TxnDate = dr["TxnDate"].ToString();
                        TxnTime = dr["TxnTime"].ToString();
                    }

                    if (NSVS != null && NSVS != "" && targetaccountno != "" && targetaccountno != null)
                    {
                        /* CJ VALIDATION FOR CREDITED ONLINE */
                        if (transtatus == (int)Trans_Status.Credited_Online)
                        {
                            transmessage = "Already available and credited to Instapay Fund Transfer";
                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.Credited_Online + "][TranxStatus=" + transtatus.ToString() + "][Description=Credited Online][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                            return ParseMessage(paramDs, transmessage);
                        }
                        else if (transtatus == (int)Trans_Status.Cancel_Not_Implemented)
                        {
                            transmessage = "Transaction is already Cancelled Not Implemented";
                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.Cancel_Not_Implemented + "][TranxStatus=" + transtatus.ToString() + "][Description=Cancelled Not Implemented][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                            return ParseMessage(paramDs, transmessage);
                        }
                        else
                        {
                            try
                            {
                                int retval_E2E = fundtransfer(referenceno.ToString(), NSVS.ToString().Trim(), targetaccountno.ToString().Trim(), amount, narrative.ToString());
                                if (retval_E2E == 1) //Return 1 if there are NO ERROR in LEG 1
                                {
                                    this.Update_FC_Message(tranxid, AccountMessage.ToString());
                                    transmessage = "Leg 1 : Successful";
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 1", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Description=" + transmessage + "]", "", "", csAuditLog.Category.Update);

                                    int InstapayTransactons = get_instapaytransactions(tranxid);
                                    if (InstapayTransactons == 1)
                                    {
                                        transmessage = "Leg 1: Successful \nLeg 2: Already available in Instapay Transaction";
                                        paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                        audit.writeAudit(clientdata.UserId, "VALIDATION OF INSTAPAY FUND TRANSFER LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.InterBank_Timed_Out.ToString() + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                        return ParseMessage(paramDs, transmessage);
                                    }

                                    /* RFC-33569 */
                                    //string isInstapayPortal = ConfigurationManager.AppSettings["RAPI_IP_Flag"];
                                    string isInstapayPortal = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "FLAG", "FldDescription");

                                    if (isInstapayPortal == "1")
                                    {
                                        string ret = new irsInstapay().ConnectToInstapayPortal(tranxid);
                                        List<string> lstRet = ret.Split('~').ToList();

                                        if (lstRet[0].ToString() == "1") //SUCCESS
                                        {
                                            paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                            transmessage = "Leg 1: Successful \nLeg 2: Successful";
                                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.Credited_Online.ToString() + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                            return ParseMessage(paramDs, transmessage);
                                        }
                                        else
                                        {
                                            if (lstRet[0].ToString() == "2") //REJECTED
                                            {
                                                transmessage = "Leg 1: Successful \nLeg 2: Rejected by Other Bank";
                                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error Description=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                                Reversal(tranxid); //Leg 3
                                                return ParseMessage(paramDs, transmessage);
                                            }
                                            else //TIMED-OUT
                                            {
                                                paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                                transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.InterBank_Timed_Out + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                                return ParseMessage(paramDs, transmessage);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        E2ERequest = "{\"UserReferenceNumber\":\"" + UserReferenceNo + "\",";
                                        E2ERequest += "\"TransactingBranch\":\"" + TransactingBranch + "\",";
                                        E2ERequest += "\"ChannelID\":\"" + ChannelId + "\",";
                                        E2ERequest += "\"Acctname\":\"" + AcctName + "\",";
                                        E2ERequest += "\"Acctno\":\"" + AcctNo + "\",";
                                        E2ERequest += "\"Acctident\":\"" + AcctIdent + "\",";
                                        E2ERequest += "\"Tfrname\":\"" + TfrName + "\",";
                                        E2ERequest += "\"Param1\":\"" + Param1 + "\",";
                                        E2ERequest += "\"Param2\":\"" + Param2 + "\",";
                                        E2ERequest += "\"Param3\":\"" + Param3 + "\",";
                                        E2ERequest += "\"Custno\":\"" + CustNo + "\",";
                                        E2ERequest += "\"Purpose\":\"" + Purpose + "\",";
                                        E2ERequest += "\"Cycode\":\"" + Cycode + "\",";
                                        E2ERequest += "\"Tfrbnkcode\":\"" + TfrBnkCode + "\",";
                                        E2ERequest += "\"Tfracctno\":\"" + TfrAcctNo + "\",";
                                        E2ERequest += "\"Txnamt\":\"" + TxnAmount + "\",";
                                        E2ERequest += "\"Bnkcode\":\"" + BnkCode + "\",";
                                        E2ERequest += "\"Accttype\":\"" + AcctType + "\",";
                                        E2ERequest += "\"Txndate\":\"" + TxnDate + "\",";
                                        E2ERequest += "\"Txntime\":\"" + TxnTime + "\"}";

                                        //Start checking if transaction is available instapay transaction
                                        try
                                        {
                                            watch.Start();
                                            res = instapayfundtransfer(E2ERequest);
                                            watch.Stop();
                                            watchRes = watch.Elapsed.TotalSeconds.ToString();
                                            instapay.parseJsonResponse(res);

                                            string InstapayRejectedCode = get_instapay_Rejected_replycode(instapay.InstapayReplyCode);

                                            if (instapay.InstapayReplyCode == "00") //INSTAPAY SUCCESSFUL
                                            {
                                                paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                                transmessage = "Leg 1: Successful \nLeg 2: Successful";
                                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.Credited_Online.ToString() + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                                this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                                return ParseMessage(paramDs, transmessage);
                                            }
                                            else if (instapay.InstapayReplyCode == "" || instapay.InstapayReplyCode == null) //INSTAPAY NULL RETURN OR TIMED-OUT
                                            {
                                                paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                                transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.InterBank_Timed_Out + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                                this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                                return ParseMessage(paramDs, transmessage);
                                            }
                                            else if (instapay.InstapayReplyCode == InstapayRejectedCode) //E2E OR BancNet Response Code in Lookup
                                            {
                                                transmessage = "Leg 1: Successful \nLeg 2: Rejected by Other Bank";
                                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error Description=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                                this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                                Reversal(tranxid); //Leg 3
                                                return ParseMessage(paramDs, transmessage);
                                            }
                                            else //INSTAPAY Not Successful or Not in Response Code in Lookup
                                            {
                                                paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                                transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.InterBank_Timed_Out + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                                this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                                return ParseMessage(paramDs, transmessage);
                                            }
                                               
                                        }
                                        catch //INSTAPAY TIMED-OUT
                                        {
                                            paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                            transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                                            this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                            return ParseMessage(paramDs, transmessage);
                                        }
                                    }
                                }
                                else //E2E TIMED-OUT
                                {
                                    paramDs = Update_Status(tranxid, (int)Trans_Status.Cancel_Not_Implemented);
                                    transmessage = "Leg 1: Timed-Out \nLeg 2: n/a";
                                    this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                                    this.saveInstapayTransaction(dt_instapay, "", "", "", "", transmessage, "", "", "");
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 1", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " AND " + "TranxStatus = " + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                                    return ParseMessage(paramDs, transmessage);
                                }
                            }
                            catch //E2E TIMED-OUT
                            {
                                paramDs = Update_Status(tranxid, (int)Trans_Status.Cancel_Not_Implemented);
                                transmessage = "Leg 1: Timed-Out \nLeg 2: n/a";
                                this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                                this.saveInstapayTransaction(dt_instapay, "", "", "", "", transmessage, "", "", "");
                                audit.writeAudit(clientdata.UserId, "E2E INSTAPAY", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " AND " + "TranxStatus = " + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                                return ParseMessage(paramDs, transmessage);
                            }

                        }

                    }
                    else //Failed Try for both Leg
                    {
                        paramDs = Update_Status(tranxid, (int)Trans_Status.Cancel_Not_Implemented);
                        transmessage = "Failed to get NSVS and or TargetAccount";
                        this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                        this.saveInstapayTransaction(dt_instapay, "", "", "", "", transmessage, "", "", "");
                        audit.writeAudit(clientdata.UserId, "E2E-INSTAPAY", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " AND " + "TranxStatus = " + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                        return ParseMessage(paramDs, transmessage);
                    }
                }//INSTAPAY ALYSSA SEPTEMBER 2019 END
                else
                {
                    paramDs = Update_Status(tranxid, (int)Trans_Status.Posted_Central);
                    transmessage = "Transaction is successful";
                }
                audit.writeAudit(clientdata.UserId, "Fund Transfer", "IRS Transaction", narrative, transmessage, csAuditLog.Category.FlexCube);
                return ParseMessage(paramDs, transmessage);
            }

            catch (Exception ex)
            {
                paramDs = Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                transmessage = ex.Message;
                audit.writeAudit(clientdata.UserId, "Fund Transfer", "IRS Transaction", narrative, transmessage, csAuditLog.Category.FlexCube);

                /*
                if (productcode == (int)ProductCode.RapidRemit || productcode == (int)ProductCode.GFC)
                {
                    reversalGFC(tranxid);
                }
                */
                return ParseMessage(paramDs, transmessage);
            }
        }

        //OTC CALL
        public DataSet Remit2(int tranxid, string branchcode, int currency, string referenceno, string targetaccountno, double amount, int transtatus, int productcode, int implemtype, string currencycode)
        {
            string narrative = "IRSW (" + referenceno + ")";
            string transmessage = "";
            DataSet paramDs = new DataSet("remit");
            string OBPExcess = "";
            irsOBPSTransactions obp = new irsOBPSTransactions();

            //INSTAPAY ALYSSA SEPTEMBER 2019 START
            E2EInstapay instapay = new E2EInstapay();
            string UserReferenceNo = null;
            string TransactingBranch = null;
            string ChannelId = null;
            string AcctName = null;
            string AcctNo = null;
            string AcctIdent = null;
            string TfrName = null;
            string Param1 = null;
            string Param2 = null;
            string Param3 = null;
            string CustNo = null;
            string Purpose = null;
            string Cycode = null;
            string TfrBnkCode = null;
            string TfrAcctNo = null;
            string TxnAmount = null;
            string BnkCode = null;
            string AcctType = null;
            string TxnDate = null;
            string TxnTime = null;
            string E2ERequest = string.Empty;
            string retval_instapay = string.Empty;
            string res = string.Empty;
            //INSTAPAY ALYSSA SEPTEMBER 2019 END

            try
            {
                if ((implemtype == (int)ImplemType.OtherBanksIBFT) && productcode == (int)ProductCode.IBFT)
                {
                    paramDs = Update_Status(tranxid, (int)Trans_Status.For_Batch_Processing);
                    transmessage = "Transaction is queue for IBFT";
                }
                else if (productcode == (int)ProductCode.INSTAPAY) //INSTAPAY ALYSSA SEPTEMBER 2019 START
                {
                    string NSVS = get_nsvs(branchcode, currency);
                    targetaccountno = get_instapay_nsvs(currency);

                    /* RFC-33569 */
                    //string isInstapayPortal = ConfigurationManager.AppSettings["RAPI_IP_Flag"];
                    string isInstapayPortal = new irsInternetPage().getLookupValue("InstapayPortalConfiguration", "FldCode", "FLAG", "FldDescription");

                    if (isInstapayPortal == "1")
                    {
                        int retval_E2E = fundtransfer(referenceno.ToString(), NSVS.ToString().Trim(), targetaccountno.ToString().Trim(), amount, narrative.ToString());
                        if (retval_E2E == 1)
                        {
                            this.Update_FC_Message(tranxid, AccountMessage.ToString());
                            transmessage = "Leg 1 : Successful";
                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 1", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Description=" + transmessage + "]", "", "", csAuditLog.Category.Update);

                            string ret = new irsInstapay().ConnectToInstapayPortal(tranxid);
                            List<string> lstRet = ret.Split('~').ToList();

                            if (lstRet[0].ToString() == "1") //SUCCESS
                            {
                                paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                transmessage = "Leg 1: Successful \nLeg 2: Successful";
                                audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.Credited_Online.ToString() + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "][ret=" + ret + "]", "", "", csAuditLog.Category.Update);
                                return ParseMessage(paramDs, transmessage);
                            }
                            else
                            {
                                if (lstRet[0].ToString() == "2") //REJECTED
                                {
                                    transmessage = "Leg 1: Successful \nLeg 2: Rejected by Other Bank";
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error Description=" + transmessage + "][ret=" + ret + "]", "", "", csAuditLog.Category.Update);
                                    Reversal(tranxid); //Leg 3
                                    return ParseMessage(paramDs, transmessage);
                                }
                                else //TIMED-OUT
                                {
                                    paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                    transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.InterBank_Timed_Out + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "][ret=" + ret + "]", "", "", csAuditLog.Category.Update);
                                    return ParseMessage(paramDs, transmessage);
                                }
                            }
                        }
                        else
                        {
                            paramDs = Update_Status(tranxid, (int)Trans_Status.Cancel_Not_Implemented);
                            transmessage = "Leg 1: Timed-Out \nLeg 2: n/a";
                            this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 1", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " AND " + "TranxStatus = " + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                            return ParseMessage(paramDs, transmessage);
                        }
                    }
                    else
                    {
                        Stopwatch watch = new Stopwatch();
                        string watchRes = string.Empty;
                        DataTable dt_instapay = get_instapay_tranx_details(tranxid);
                        foreach (DataRow dr in dt_instapay.Rows)
                        {
                            UserReferenceNo = dr["UserReferenceNo"].ToString();
                            TransactingBranch = dr["TransactingBranch"].ToString();
                            ChannelId = dr["ChannelId"].ToString();
                            AcctName = dr["AcctName"].ToString();
                            AcctNo = dr["AcctNo"].ToString();
                            AcctIdent = dr["AcctIdent"].ToString();
                            TfrName = dr["TfrName"].ToString();
                            Param1 = dr["Param1"].ToString();
                            Param2 = dr["Param2"].ToString();
                            Param3 = dr["Param3"].ToString();
                            CustNo = dr["CustNo"].ToString();
                            Purpose = dr["Purpose"].ToString();
                            Cycode = dr["Cycode"].ToString();
                            TfrBnkCode = dr["TfrBnkCode"].ToString();
                            TfrAcctNo = dr["TfrAcctNo"].ToString();
                            TxnAmount = dr["TxnAmount"].ToString();
                            BnkCode = dr["BnkCode"].ToString();
                            AcctType = dr["AcctType"].ToString();
                            TxnDate = dr["TxnDate"].ToString();
                            TxnTime = dr["TxnTime"].ToString();
                        }

                        if (NSVS != null && NSVS != "" && targetaccountno != "" && targetaccountno != null)
                        {
                            try
                            {
                                int retval_E2E = fundtransfer(referenceno.ToString(), NSVS.ToString().Trim(), targetaccountno.ToString().Trim(), amount, narrative.ToString());
                                if (retval_E2E == 1) //Return 1 if there are NO ERROR in LEG 1
                                {
                                    this.Update_FC_Message(tranxid, AccountMessage.ToString());
                                    transmessage = "Leg 1 : Successful";
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 1", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Description=" + transmessage + "]", "", "", csAuditLog.Category.Update);

                                    E2ERequest = "{\"UserReferenceNumber\":\"" + UserReferenceNo + "\",";
                                    E2ERequest += "\"TransactingBranch\":\"" + TransactingBranch + "\",";
                                    E2ERequest += "\"ChannelID\":\"" + ChannelId + "\",";
                                    E2ERequest += "\"Acctname\":\"" + AcctName + "\",";
                                    E2ERequest += "\"Acctno\":\"" + AcctNo + "\",";
                                    E2ERequest += "\"Acctident\":\"" + AcctIdent + "\",";
                                    E2ERequest += "\"Tfrname\":\"" + TfrName + "\",";
                                    E2ERequest += "\"Param1\":\"" + Param1 + "\",";
                                    E2ERequest += "\"Param2\":\"" + Param2 + "\",";
                                    E2ERequest += "\"Param3\":\"" + Param3 + "\",";
                                    E2ERequest += "\"Custno\":\"" + CustNo + "\",";
                                    E2ERequest += "\"Purpose\":\"" + Purpose + "\",";
                                    E2ERequest += "\"Cycode\":\"" + Cycode + "\",";
                                    E2ERequest += "\"Tfrbnkcode\":\"" + TfrBnkCode + "\",";
                                    E2ERequest += "\"Tfracctno\":\"" + TfrAcctNo + "\",";
                                    E2ERequest += "\"Txnamt\":\"" + TxnAmount + "\",";
                                    E2ERequest += "\"Bnkcode\":\"" + BnkCode + "\",";
                                    E2ERequest += "\"Accttype\":\"" + AcctType + "\",";
                                    E2ERequest += "\"Txndate\":\"" + TxnDate + "\",";
                                    E2ERequest += "\"Txntime\":\"" + TxnTime + "\"}";


                                    try
                                    {
                                        watch.Start();
                                        res = instapayfundtransfer(E2ERequest);
                                        watch.Stop();
                                        watchRes = watch.Elapsed.TotalSeconds.ToString();
                                        instapay.parseJsonResponse(res);

                                        string InstapayRejectedCode = get_instapay_Rejected_replycode(instapay.InstapayReplyCode);

                                        if (instapay.InstapayReplyCode == "00") //INSTAPAY SUCCESSFUL
                                        {
                                            paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                            transmessage = "Leg 1: Successful \nLeg 2: Successful";
                                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.Credited_Online.ToString() + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                            this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                            return ParseMessage(paramDs, transmessage);
                                        }
                                        else if (instapay.InstapayReplyCode == "" || instapay.InstapayReplyCode == null) //INSTAPAY NULL RETURN OR TIMED-OUT
                                        {
                                            paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                            transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.InterBank_Timed_Out + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                            this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                            return ParseMessage(paramDs, transmessage);
                                        }
                                        else if (instapay.InstapayReplyCode == InstapayRejectedCode) //E2E OR BancNet Response Code in Lookup
                                        {
                                            transmessage = "Leg 1: Successful \nLeg 2: Rejected by Other Bank";
                                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error Description=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                            this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                            Reversal(tranxid); //Leg 3
                                            return ParseMessage(paramDs, transmessage);
                                        }
                                        else //INSTAPAY Not Successful or Not in Response Code in Lookup
                                        {
                                            paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                            transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                            audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.InterBank_Timed_Out + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "][transmessage=" + transmessage + "]", "", "", csAuditLog.Category.Update);
                                            this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                            return ParseMessage(paramDs, transmessage);
                                        }
                                    }
                                    catch //INSTAPAY TIMED-OUT
                                    {
                                        paramDs = Update_Status(tranxid, (int)Trans_Status.InterBank_Timed_Out);
                                        transmessage = "Leg 1: Successful \nLeg 2: InterBank Timed Out";
                                        audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 2", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                                        this.saveInstapayTransaction(dt_instapay, instapay.InstapayReplyCode, instapay.InstapayReplyText, instapay.InstapayUserReferenceNumber, instapay.InstapayTraceno, transmessage, E2ERequest, res, watchRes);
                                        return ParseMessage(paramDs, transmessage);
                                    }
                                }
                                else //E2E TIMED-OUT
                                {
                                    paramDs = Update_Status(tranxid, (int)Trans_Status.Cancel_Not_Implemented);
                                    transmessage = "Leg 1: Timed-Out \nLeg 2: n/a";
                                    this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                                    this.saveInstapayTransaction(dt_instapay, "", "", "", "", transmessage, "", "", "");
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER INSTAPAY LEG 1", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " AND " + "TranxStatus = " + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                                    return ParseMessage(paramDs, transmessage);
                                }
                            }
                            catch //E2E TIMED-OUT
                            {
                                paramDs = Update_Status(tranxid, (int)Trans_Status.Cancel_Not_Implemented);
                                transmessage = "Leg 1: Timed-Out \nLeg 2: n/a";
                                this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                                this.saveInstapayTransaction(dt_instapay, "", "", "", "", transmessage, "", "", "");
                                audit.writeAudit(clientdata.UserId, "E2E INSTAPAY", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " AND " + "TranxStatus = " + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                                return ParseMessage(paramDs, transmessage);
                            }
                        }
                        else //Failed Try for both Leg
                        {
                            paramDs = Update_Status(tranxid, (int)Trans_Status.Cancel_Not_Implemented);
                            transmessage = "Failed to get NSVS and or TargetAccount";
                            this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                            this.saveInstapayTransaction(dt_instapay, "", "", "", "", transmessage, "", "", "");
                            audit.writeAudit(clientdata.UserId, "E2E-INSTAPAY", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "]", "[Error Description=" + transmessage + " AND " + "TranxStatus = " + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "]", "", csAuditLog.Category.Update);
                            return ParseMessage(paramDs, transmessage);
                        }
                    }
                }//INSTAPAY ALYSSA SEPTEMBER 2019 END
                else
                {
                    if ((implemtype == (int)ImplemType.CreditToAccount) || (productcode == (int)ProductCode.PAW) || (productcode == (int)ProductCode.APASOnline))
                    {
                        if (productcode == (int)ProductCode.PAW)
                        {
                            targetaccountno = GetPAWAccount(currency);
                        }
                        if (productcode == (int)ProductCode.APASOnline)
                        {
                            targetaccountno = GetApasAccount(currency);
                        }

                        /* E2EAPICHANGE 20220209 REMOVE throughGLAccount */
                            string NSVS = get_nsvs(branchcode, currency);
                            if (NSVS != null && NSVS != "" && targetaccountno != "")
                            {
                                paramDs = Update_Status(tranxid, (int)Trans_Status.For_Batch_Processing);
                                if (paramDs != null)
                                {
                                    if (paramDs.Tables[0].Rows.Count > 0)
                                    {
                                        audit.writeAudit(clientdata.UserId, "FUND TRANSFER", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Status=" + Trans_Status.For_Batch_Processing.ToString() + "][TranxStatus=" + paramDs.Tables[0].Rows[0][0].ToString().Trim() + "][Description=" + paramDs.Tables[0].Rows[0][1].ToString().Trim() + "]", "", "", csAuditLog.Category.Update);
                                    }
                                    else
                                    {
                                        audit.writeAudit(clientdata.UserId, "FUND TRANSFER", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error=No return value]", "", "", csAuditLog.Category.Update);
                                    }
                                }
                                else
                                {
                                    audit.writeAudit(clientdata.UserId, "FUND TRANSFER", "OTC TRANSACTIONS=[Transaction ID=" + tranxid.ToString() + "][Error=No return value]", "", "", csAuditLog.Category.Update);
                                }

                                /*
                                 if RunAgentCrediting key in Webconfig is false FC creditting will be in irsRemTeller.cs
                                 if true, creditting will be in IRSWorldAgent                             
                                */

                                String swtch = this.GetAppSetting("RunAgentCrediting");
                                if (!Convert.ToBoolean(swtch))
                                {
                                    /* FOR CBIP */
                                    int retval = fundtransfer(referenceno.ToString(), NSVS.ToString().Trim(), targetaccountno.ToString().Trim(), amount, narrative.ToString());
                                    if (retval == 1)
                                    {
                                        this.Update_FC_Message(tranxid, AccountMessage.ToString());
                                        if (obp.fOBPExcess(tranxid))
                                        {
                                            OBPExcess = System.Web.HttpContext.Current.Session["OBPSExcess"].ToString();
                                            string OBPRefno = "O" + referenceno.ToString().Substring(1);
                                            string OBPNSVS = OBPExcess.Split('|')[2].ToString();
                                            string OBPAccount = OBPExcess.Split('|')[1].ToString();
                                            double OBPAmount = Convert.ToDouble(OBPExcess.Split('|')[4].ToString());
                                            string OBPNarrative = "IRSW (" + OBPRefno + ")";

                                            if (OBPAmount > 0)
                                            {
                                                retval = fundtransfer(OBPRefno, OBPNSVS, OBPAccount, OBPAmount, OBPNarrative);
                                                if (retval == 1)
                                                {
                                                    paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                                    transmessage = AccountMessage.ToString();
                                                }
                                                else
                                                {
                                                    paramDs = Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                                                    transmessage = ErrorMessage.ToString();
                                                }
                                            }
                                            else
                                            {
                                                paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                                transmessage = AccountMessage.ToString();
                                            }
                                            audit.writeAudit(clientdata.UserId, "Fund Transfer", "OBPS Excess Payments", OBPNarrative, transmessage, csAuditLog.Category.FlexCube);
                                        }
                                        else
                                        {
                                            paramDs = Update_Status(tranxid, (int)Trans_Status.Credited_Online);
                                            transmessage = AccountMessage.ToString();

                                            if (productcode == (int)ProductCode.APASOnline)
                                            {
                                                bool no_error = false;
                                                no_error = new irsOldAPAS().InsertApas(tranxid);
                                            }
                                            else if (productcode == (int)ProductCode.PAW)
                                            {
                                                new irsOldAPAS().InsertAnPAW(tranxid);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        transmessage = this.Get_FC_Description(myErrorMessage.ToString());
                                        this.Update_FC_Message(tranxid, this.Get_FC_Description(myErrorMessage.ToString()));
                                        paramDs = Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                                        /*
                                        if (productcode == (int)ProductCode.RapidRemit || productcode == (int)ProductCode.GFC)
                                        {
                                            reversalGFC(tranxid);
                                        }
                                        */
                                    }
                                }
                                else
                                {
                                    transmessage = "Transaction is successful";
                                }
                            }
                    }
                    else
                    {
                        paramDs = Update_Status(tranxid, (int)Trans_Status.Posted_Central);
                        transmessage = "Transaction is successful";
                    }
                    audit.writeAudit(clientdata.UserId, "Fund Transfer", "IRS Transaction", narrative, transmessage, csAuditLog.Category.FlexCube);
                }
                return ParseMessage(paramDs, transmessage);
            }
            catch (Exception ex)
            {
                paramDs = Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                transmessage = ex.Message;
                audit.writeAudit(clientdata.UserId, "Fund Transfer", "IRS Transaction", narrative, transmessage, csAuditLog.Category.FlexCube);
                /*
                if (productcode == (int)ProductCode.RapidRemit || productcode == (int)ProductCode.GFC)
                {
                    reversalGFC(tranxid);
                }
                */
                return ParseMessage(paramDs, transmessage);
            }
        }

        public bool isDebittoLocalAccount(int paymode)
        {
            try
            {
                DataTable dt = execStoredProcedureTable("sp_VerifyPaymode", new object[] { paymode });

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    object[] MSG = row.ItemArray;

                    if (MSG[0].ToString() == "1")
                        return true;
                    else
                    {
                        return false;
                    }
                }
                return false;
            }

            catch
            { return false; }
            finally
            { }
        }

        //Added by Troy 20131127. RFS #13323-011.
        public DataTable getCIFAccounts(int seqid)
        {
            DataTable dt = get_remit_details(seqid);
            try
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public DataSet Reversal(int seqid)
        {
            string branchcode = null;
            int currency = 0;
            string referenceno = null;
            string targetaccountno = null;
            double amount = 0;
            int transtatus = 0;
            int productcode = 0;
            int implemtype = 0;
            int paymode = 0;
            double GrossRemAmount = 0;
            string sourceaccountno = null;
            int sourcecurrency = 0;
            string currencycode = null;
            string sourcecurrencycode = null;
            try
            {
                DataTable dt = get_remit_details(seqid);

                foreach (DataRow dr in dt.Rows)
                {
                    referenceno = dr["ReferenceNo"].ToString();
                    branchcode = dr["RemittingBranch"].ToString();
                    transtatus = int.Parse(dr["Transtatus"].ToString());
                    productcode = int.Parse(dr["productcode"].ToString());
                    implemtype = int.Parse(dr["implementationtype"].ToString());
                    targetaccountno = dr["AccountNo"].ToString();
                    amount = double.Parse(dr["NettoBeneAmount"].ToString());
                    currency = int.Parse(dr["NettoBeneCurrency"].ToString());
                    currencycode = dr["NettoBeneCurrency"].ToString();
                    paymode = int.Parse(dr["paymode"].ToString());
                    sourceaccountno = dr["sourceaccountno"].ToString();
                    sourcecurrency = int.Parse(dr["RemittingCurrency"].ToString());
                    sourcecurrencycode = dr["RemittingCurrency"].ToString();
                    GrossRemAmount = double.Parse(dr["GrossRemAmount"].ToString());
                }
                return Reversal(seqid, branchcode, currency, referenceno, targetaccountno, amount, transtatus, productcode, implemtype, paymode, sourceaccountno, sourcecurrency, GrossRemAmount, currencycode, sourcecurrencycode);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            { }
        }

        public DataSet Reversal(int tranxid, string branchcode, int currency, string referenceno, string targetaccountno, double amount, int transtatus, int productcode, int implemtype, int paymode, string sourceaccountno, int sourcecurrency, double GrossRemAmount, string currencycode, string sourcecurrencycode)
        {
            string narrative = "IRSW (" + referenceno + ") Reversal";
            //int tranxid = int.Parse(System.Web.HttpContext.Current.Session["TranxID"].ToString());
            int curtranstatus;
            string transmessage = "";
            DataSet paramDs = new DataSet();

            try
            {
                /* E2EAPICHANGE 20220209 REMOVE isDebittoLocalAccount */
                if ((implemtype == (int)Trans_Status.Credited_Online) || (productcode == 24) || (productcode == 27))
                {
                    if (transtatus == (int)Trans_Status.Credited_Online)
                    {
                        string NSVS = get_nsvs(branchcode, currency);
                        if (NSVS != null && NSVS != "" && targetaccountno != "")
                        {
                            /* FOR CBIP */
                            int retval = this.fundtransfer(referenceno.ToString(), targetaccountno.ToString().Trim(), NSVS.ToString().Trim(), amount, narrative.ToString());
                            if (retval == 1)
                            {
                                curtranstatus = (int)Trans_Status.Cancel_Implemented;
                                paramDs = Update_Status(tranxid, curtranstatus);
                                transmessage = AccountMessage.ToString() + " Reversal Successful";
                                this.Update_FC_Message(tranxid, transmessage.ToString());
                            }
                            else
                            {
                                //paramDs = Update_Status(tranxid, 2);
                                transmessage = ErrorMessage.ToString();
                                this.Update_FC_Message(tranxid, transmessage);
                                //transmessage = ErrorMessage.ToString();
                                this.Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                            }
                        }
                        else
                        {
                            transmessage = "Transaction can't be reversed.Target Account or Nostro/Vostro Account is missing. Please verify.";
                            //this.Update_FC_Message(tranxid, transmessage);
                            //paramDs = Update_Status(tranxid, (int)Trans_Status.For_Reprocess);
                        }
                    }
                    else if (transtatus == (int)Trans_Status.Posted_Central)
                    {
                        curtranstatus = (int)Trans_Status.Cancel_Implemented;
                        paramDs = Update_Status(tranxid, curtranstatus);
                        transmessage = " Reversal Successful";
                    }
                    else
                    {
                        curtranstatus = (int)Trans_Status.Cancel_Not_Implemented;
                        paramDs = Update_Status(tranxid, curtranstatus);
                        transmessage = " Reversal Successful";
                    }
                }
                else if (productcode == 258) //INSTAPAY ALYSSA SEPTEMBER 2019 REVERSAL START
                {
                    string NSVS = get_nsvs(branchcode, currency);
                    targetaccountno = get_instapay_nsvs(currency);

                    curtranstatus = (int)Trans_Status.Cancel_Not_Implemented;

                    if (NSVS != null && NSVS != "" && targetaccountno != null && targetaccountno != "")
                    {
                        int retval = this.fundtransfer(referenceno.ToString(), targetaccountno.ToString().Trim(), NSVS.ToString().Trim(), amount, narrative.ToString());
                        if (retval == 1)
                        {
                            curtranstatus = (int)Trans_Status.Cancel_Not_Implemented;
                            paramDs = Update_Status(tranxid, curtranstatus);
                            transmessage = AccountMessage.ToString() + " Reversal Successful. tranxid " + tranxid;
                            this.Update_FC_Message(tranxid, transmessage.ToString());
                            audit.writeAudit(clientdata.UserId, "INSTAPAY Reversal", "Debit to Overseas Account", narrative, transmessage, csAuditLog.Category.FlexCube);
                        }
                        else
                        {
                            curtranstatus = (int)Trans_Status.Cancel_Not_Implemented;
                            paramDs = Update_Status(tranxid, curtranstatus);
                            transmessage = ErrorMessage.ToString() + "tranxid " + tranxid;
                            this.Update_FC_Message(tranxid, transmessage);
                            audit.writeAudit(clientdata.UserId, "INSTAPAY Reversal", "Debit to Overseas Account", narrative, transmessage + tranxid, csAuditLog.Category.FlexCube);
                        }
                    }
                    else
                    {
                        curtranstatus = (int)Trans_Status.Cancel_Not_Implemented;
                        paramDs = Update_Status(tranxid, curtranstatus);
                        transmessage = "Transaction cant be reversed. Target Account or Nostro/Vostro Account is missing. Please verify." + "tranxid " + tranxid;
                        this.Update_FC_Message(tranxid, transmessage);
                        audit.writeAudit(clientdata.UserId, "INSTAPAY Reversal", "Debit to Overseas Account", narrative, transmessage + tranxid, csAuditLog.Category.FlexCube);
                    }
                    //INSTAPAY ALYSSA SEPTEMBER 2019 REVERSAL END
                }
                else
                {
                    if ((transtatus == (int)Trans_Status.For_Approval) || (transtatus == (int)Trans_Status.For_Reprocess))
                    {
                        curtranstatus = (int)Trans_Status.Cancel_Not_Implemented;
                    }
                    else
                    {
                        curtranstatus = (int)Trans_Status.Cancel_Implemented;
                    }
                    paramDs = Update_Status(tranxid, curtranstatus);
                    transmessage = "Reversal Successful";
                }
                audit.writeAudit(clientdata.UserId, "Reversal", "Debit to Overseas Account", narrative, transmessage, csAuditLog.Category.FlexCube);
                /*
                if (productcode == (int)ProductCode.RapidRemit || productcode == (int)ProductCode.GFC)
                {
                    reversalGFC(tranxid);
                }
                */
                return ParseMessage(paramDs, transmessage);
            }
            catch (Exception ex)
            {
                transmessage = ex.ToString();
                audit.writeAudit(clientdata.UserId, "Reversal", "Debit to Overseas Account", narrative, transmessage, csAuditLog.Category.FlexCube);
                return ParseMessage(paramDs, transmessage);
            }
            finally
            { }
        }

        /* E2EAPICHANGE 20220209 REMOVE debitGlCreditCasa */

        /* E2EAPICHANGE 20220209 REMOVE DebitCasaCreditGL */

        private int fundtransfer(string referenceno, string sourceaccountno, string targetaccountno, double amount, string narrative)
        {
            /* E2EAPICHANGE 20220209 Updated Code */
            SIcredit ft = new SIcredit();
            string ret = string.Empty;
            if (ft.fund_transfer(referenceno, sourceaccountno, targetaccountno, amount.ToString(), narrative, out ret))
                {
                    myAccountMessage = "ACCOUNT PROCESSED ***" + ret + "GL" + sourceaccountno.ToString() + "CK" + targetaccountno.ToString();
                    return 1;
                }
                else
                {
                    myErrorMessage = "Error (SI):" + ret.Replace("Exception:", "").ToString();
                    return -1;
                }
        }

        /* INSTAPAY ALYSSA SEPTEMBER 2019 START*/
        private string instapayfundtransfer(string E2ERequest)
        {
            string narrative = E2ERequest;
            string transmessage = "";
            E2EInstapay instapay = new E2EInstapay();
            string ret = string.Empty;
            try
            {
                ret = instapay.instapayServices(E2EInstapay.InstapayService.FundTransfer, E2ERequest);
                return ret;
            }
            catch (Exception ex)
            {
                transmessage = ex.Message;
                audit.writeAudit(clientdata.UserId, "INSTAPAY LEG2", "OTC-TRANSACTION", narrative, transmessage, csAuditLog.Category.FlexCube);
                return ret;
            }
        }
        public bool saveInstapayTransaction(DataTable dt_instapay, string InstapayReplyCode, string InstapayReplyText, string InstapayUserReferenceNumber, string InstapayTraceno, string transmessage, string MsgRequest, string MsgReturn, string watchRes) /* 3 */
        {
            object[] o = new object[30];

            foreach (DataRow dr in dt_instapay.Rows)
            {
                o[0] = dr["UserReferenceNo"].ToString();
                o[1] = dr["TransactingBranch"].ToString();
                o[2] = dr["ChannelId"].ToString();
                o[3] = dr["AcctName"].ToString();
                o[4] = dr["AcctNo"].ToString();
                o[5] = dr["AcctIdent"].ToString();
                o[6] = dr["TfrName"].ToString();
                o[7] = dr["Param1"].ToString();
                o[8] = dr["Param2"].ToString();
                o[9] = dr["Param3"].ToString();
                o[10] = dr["CustNo"].ToString();
                o[11] = dr["Purpose"].ToString();
                o[12] = dr["Cycode"].ToString();
                o[13] = dr["TfrBnkCode"].ToString();
                o[14] = dr["TfrAcctNo"].ToString();
                o[15] = dr["TxnAmount"].ToString();
                o[16] = dr["BnkCode"].ToString();
                o[17] = dr["AcctType"].ToString();
                o[18] = dr["TxnDate"].ToString();
                o[19] = dr["TxnTime"].ToString();
                o[20] = dr["BeneficiaryCIFID"].ToString();
                o[21] = dr["IRSWReferenceNo"].ToString();
                o[22] = InstapayReplyCode.ToString();
                o[23] = InstapayReplyText.ToString();
                o[24] = InstapayUserReferenceNumber.ToString();
                o[25] = InstapayTraceno.ToString();
                o[26] = transmessage.ToString();
                o[27] = MsgRequest.ToString();
                o[28] = MsgReturn.ToString();
                o[29] = watchRes.ToString();
            }
            try
            {
                if (this.addInstapayRecord(o))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                audit.writeAudit("", "saveInstapayTransaction", "Failed to call addInstapayRecord - " + ex, "", "", csAuditLog.Category.Insert);
            }
            return false;

        }
        private bool addInstapayRecord(object[] ob) /* 4 */
        {
            object o;
            try
            {

                o = base.execScalar("sp_insert_tbl_instapayTransaction", ob);
                if (o != null)
                {
                    audit.writeAudit("", "addInstapayRecord", "Error inserting to Tbl_InstapayTransaction", "", "", csAuditLog.Category.Insert);
                    return false;
                }
            }
            catch (Exception ex)
            {
                audit.writeAudit("", "addInstapayRecord", "Error inserting to Tbl_InstapayTransaction" + ex, "", "", csAuditLog.Category.Insert);
            }
            finally
            {
                o = null;
            }
            return true;
        }

        /* INSTAPAY ALYSSA SEPTEMBER 2019 END*/

        public string ErrorMessage
        {
            get
            {
                return myErrorMessage;
            }
        }

        public string AccountMessage
        {
            get
            {
                return myAccountMessage;
            }
        }

        public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle, string CultureName)
        {
            Double result;
            return Double.TryParse(val, NumberStyle, new System.Globalization.CultureInfo
                        (CultureName), out result);
        }

        public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
        {
            Double result;
            return Double.TryParse(val, NumberStyle,
                System.Globalization.CultureInfo.CurrentCulture, out result);
        }

        private string FindField(string FieldName, string ResponseData, string delimeter, string delimeter2)
        {
            string[] Items;
            string Item = "";
            int i = 0;

            Items = ResponseData.Split(Convert.ToChar(delimeter));

            for (i = Items.GetLowerBound(0); i <= Items.GetUpperBound(0); i++)
            {
                Item = Items[i].ToString().Replace((char)10, (char)32);

                if (Item.IndexOf(FieldName + delimeter2) != -1)
                {
                    return Item.Substring(Item.IndexOf(delimeter2) + 1, Item.Trim().Length - Item.IndexOf(delimeter2) - 1);
                }
            }
            return null;
        }

        private string GetAcctTypeFC(string AcctTypeCode)
        {
            string retValue = "";
            {
                DataTable dt = execStoredProcedureTable("sp_GetAcctType", new object[] { int.Parse(AcctTypeCode) });

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    object[] MSG = row.ItemArray;
                    retValue = MSG[0].ToString();
                }
                return retValue;
            }
        }

        private string GetAcctStatusFC(string AcctStatusCode, int AcctStatusDesc)
        {
            DataTable dt = execStoredProcedureTable("sp_GetAcctStatus", new object[] { int.Parse(AcctStatusCode) });

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                object[] MSG = row.ItemArray;

                AccountStatusFC = MSG[0].ToString();
                AccountStatusFCValid = MSG[1].ToString();
            }
            if (AcctStatusDesc == 0)
                return AccountStatusFC;
            else
                return AccountStatusFCValid;
        }

        public string GetPAWAccount(int currency)
        {
            DataTable dt = execStoredProcedureTable("sp_GetPAWAccount", new object[] { currency });

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                object[] MSG = row.ItemArray;

                return MSG[0].ToString();
            }
            return "";
        }

        public string GetApasAccount(int currency)
        {
            DataTable dt = execStoredProcedureTable("sp_GetApasOnlineAccount", new object[] { currency });

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                object[] MSG = row.ItemArray;

                return MSG[0].ToString();
            }
            return "";
        }

        public string Get_FC_Description(string FCerror)
        {
            DataTable dt = execStoredProcedureTable("sp_getFCError_Description", new object[] { FCerror });

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                object[] MSG = row.ItemArray;

                return MSG[0].ToString();
            }
            return FCerror;
        }

        private DataSet Update_Status(int transid, int transtatus)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(clientdata.Location.ToString());
            /* 2025 FIX */
            string log_details = string.Empty;
            DataTable dt_up = null;
            /* 2025 FIX */

            /* 2025 FIX */
            try
            {
                dt_up = commons.getDataSet("select seq_id, referenceno, Transtatus from tbl_remittance with (nolock) where seq_id = " + transid.ToString(), false, new object[] { }).Tables[0];

                if (dt_up != null && dt_up.Rows.Count > 0)
                {
                    log_details = " [Reference No: " + dt_up.Rows[0]["referenceno"].ToString() + "]";
                    log_details += " [Sequence ID: " + dt_up.Rows[0]["seq_id"].ToString() + "]";
                    log_details += " [Old Status: " + dt_up.Rows[0]["Transtatus"].ToString() + "]";
                    log_details += " [New Status: " + transtatus.ToString() + "]";
                }
            }
            catch
            {
                dt_up = null; log_details = string.Empty;
            }
            finally
            {
                if (dt_up != null)
                {
                    dt_up.Dispose();
                    dt_up = null;
                }
            }
            /* 2025 FIX */
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_trans_update_status");
                    sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = transid;
                    sqcommands.Parameters.Add("@STATUS", SqlDbType.Int, 100).Value = transtatus;
                    sqcommands.Parameters.Add("@dateimlem_overseas", SqlDbType.DateTime).Value = Convert.ToString(dtLocalTime);
                    //audit.writeAudit(clientdata.UserId, "Remittance Teller", "IRS Transaction", transtatus.ToString(), transtatus.ToString(), csAuditLog.Category.Update);
                    audit.writeAudit(clientdata.UserId, "Remittance Teller", "IRS Transaction " + log_details + " [TransID " + transid + "]", transtatus.ToString(), transtatus.ToString(), csAuditLog.Category.Update);
                    return commons.getDataSet(sqcommands, "TRANS_STATUS");
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

        private void Update_FC_Message(int transid, string FCmessage)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    execScalar("sp_trans_update_fc_status", new object[] { transid, FCmessage });
                }
            }
            catch (Exception ex)
            {
                throw (ex);
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

        private DataSet Update_CIF_New_Account(int transid, string referenceno, string OldAccount, string NewAccount)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    if (commons != null)
                    {
                        execScalar("sp_update_cif_new_account", new object[] { transid, NewAccount });
                        audit.writeAudit(clientdata.UserId, "Remittance Teller", "Update FC Account  ", "Refno: " + referenceno.ToString() + "; Old Account: " + NewAccount.ToString(), OldAccount.ToString(), csAuditLog.Category.Update);
                    }
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

        public string get_status_description(int status)
        {
            Object o = base.execScalar("SELECT flddescription FROM tbl_lookup WHERE  fldgroup='tranxstatus' and fldvalue =" + status);
            if (o != DBNull.Value)
            {
                return Convert.ToString(o);
            }
            return "";
        }
        private int get_rowcount_instapay(string instapayuserreferenceno)
        {
            var cnt = 0;
            Object o = base.execScalar("SELECT COUNT(SEQ_ID) FROM tbl_INSTAPAYTransactions WHERE IRSWUserReferenceNo ='" + instapayuserreferenceno + "'");
            if (o != DBNull.Value)
            {
                cnt = Convert.ToInt16(o);
            }

            return cnt;
        }
        public DataSet ParseMessage(int Status, string dsMessage)
        {
            DataTable dtParam = new DataTable();
            DataSet NewdsParam = new DataSet();
            DataColumn column1;
            DataColumn column2;
            DataColumn column3;
            DataRow row = dtParam.NewRow();
            // Create new DataColumn, set DataType, ColumnName and add to DataTable.   

            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "Message";
            dtParam.Columns.Add(column1);

            column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.Int32");
            column2.ColumnName = "Transtatus";
            dtParam.Columns.Add(column2);

            column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");
            column3.ColumnName = "TranStatusDesc";
            dtParam.Columns.Add(column3);

            string statusdesc = this.get_status_description(Status);

            row["Transtatus"] = Status.ToString();
            row["TranStatusDesc"] = statusdesc;
            row["Message"] = dsMessage.ToString();

            dtParam.Rows.Add(row);
            NewdsParam.Tables.Add(dtParam);
            return NewdsParam;
        }

        public DataSet ParseMessage(DataSet DsParam, string dsMessage)
        {
            DataTable dtParam = new DataTable();
            DataSet NewdsParam = new DataSet();
            DataColumn column1;
            DataColumn column2;
            DataColumn column3;
            DataRow row = dtParam.NewRow();
            // Create new DataColumn, set DataType, ColumnName and add to DataTable.   

            column1 = new DataColumn();
            column1.DataType = System.Type.GetType("System.String");
            column1.ColumnName = "Message";
            dtParam.Columns.Add(column1);

            column2 = new DataColumn();
            column2.DataType = System.Type.GetType("System.Int32");
            column2.ColumnName = "Transtatus";
            dtParam.Columns.Add(column2);

            column3 = new DataColumn();
            column3.DataType = System.Type.GetType("System.String");
            column3.ColumnName = "TranStatusDesc";
            dtParam.Columns.Add(column3);

            if (DsParam.Tables.Count == 0)
            {
                row["Transtatus"] = DBNull.Value;
                row["TranStatusDesc"] = DBNull.Value;
            }
            else
            {
                row["Transtatus"] = DsParam.Tables[0].Rows[0].ItemArray[0].ToString();
                row["TranStatusDesc"] = DsParam.Tables[0].Rows[0].ItemArray[1].ToString();
            }
            row["Message"] = dsMessage.ToString();

            dtParam.Rows.Add(row);
            NewdsParam.Tables.Add(dtParam);
            return NewdsParam;
        }

        public void setLocation(string Location)
        {
            ClientData._location = Location;
        }

        /* E2EAPICHANGE 20220209 REMOVE verifyBranchifUsingGL */

        /* E2EAPICHANGE 20220209 REMOVE insert_record */

        /* E2EAPICHANGE 20220209 REMOVE set_record_and_commit */
            }
}