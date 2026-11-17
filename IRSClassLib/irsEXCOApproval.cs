using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsEXCOApproval : sqlHelperWs
    {
        #region VARIABLES
        public ArrayList ErrorMessage = new ArrayList();
        private StringBuilder DebitMessage = new StringBuilder();
        private csAuditLog audit = new csAuditLog();
        private string Branchcode = "";
        private string UserID = "";
        private string fcmessage = "";
        private string filename = "";
        int tranx_purpose;
        int pay_mode;
        int tranx_source;
        int receipt_mode;
        int source_of_funds;
        int FileInvSeqno;
        string ArrayItems;
        string ArrayFunds;
        string TransactingBranch = "";
        #endregion

        public irsEXCOApproval(string _branchcode, string _userid)
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            this.Branchcode = _branchcode;
            this.UserID = _userid;
        }

        ~irsEXCOApproval()
        {
            if (audit != null)
            {
                audit = null;
            }
            if (DebitMessage.Length > 0)
            {
                this.DebitMessage.Remove(0, DebitMessage.Length);
            }
            if (this.ErrorMessage.Count > 0)
            {
                this.ErrorMessage.Remove(0);
            }
        }

        #region PUBLIC METHOD FOR EXCO APPROVAL
        public DataSet execStoredProc(string spname, string[] param)
        {
            base.EnforceConstraint = false;
            DataSet ds = this.execStoredProcedure(spname, param);
            return ds;
        }

        public DataTable get_exco_switch(string _branchcode)
        {
            string param = _branchcode;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_select_exco_switch", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                this.exco_logger("G_E_S", e.Message, e.StackTrace, "throwerr");
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        public DataTable get_exco_files(string _branchcode)
        {
            string param = _branchcode;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_exco_files", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                this.exco_logger("G_E_F", e.Message, e.StackTrace, "throwerr");
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        public DataTable get_exco_files_fp(string _branchcode)
        {
            string param = _branchcode;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_exco_files_fp", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                this.exco_logger("G_E_F", e.Message, e.StackTrace, "throwerr");
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        public DataSet get_exco_file_details(string _branchcode, int _selectedfile, int _currency)
        {
            string param = _branchcode + "," + _selectedfile + "," + _currency;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_exco_file_details", param.Split(','));
            try
            {
                return ds;
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRGEF", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        public DataTable get_exco_branches()
        {
            string param = "";
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_exco_branches", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRGEB", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        public DataTable get_currency_rates(string _branchcode, int _sourcecurrency, int _targetcurrency)
        {
            string param = _branchcode + "," + _sourcecurrency + "," + _targetcurrency;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_currencyrates", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRGCR", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        public string check_agent_setup(string _branchcode)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = null;

            try
            {
                cmd = base.getSqlCommand("proc_check_subbranch", true);
                cmd.Parameters.Add("@v_agentbranch", SqlDbType.VarChar, 10).Value = _branchcode;
                cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 15).Direction = ParameterDirection.Output;
                ds = base.getDataSet(cmd);
                return cmd.Parameters["@v_errmsg"].Value.ToString();
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRGCR", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return e.Message;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; cmd.Dispose(); cmd = null; } }
        }

        public DataTable get_exco_accounts_with_balance(string _branchcode)
        {
            /* EDITED 09152017 JEFF & BOYET ALRAHJI IR */
            DataTable dt = null;
            DataTable dtNew = null;

            try
            {
                dt = this.get_exco_Accounts(_branchcode);
                dtNew = this.create_accounts_tables();
                foreach (DataRow r in dt.Rows)
                {
                    DataRow urow = dtNew.NewRow();

                    urow["ACCOUNTNO"] = r["ACCOUNTNO"].ToString();
                    urow["CURRENCY"] = r["CURRENCY"].ToString();
                    urow["CURRENCYCODE"] = Convert.ToInt32(r["CURRENCYCODE"]);
                    try
                    {
                        //urow["FUNDSVALUE"] = this.get_funds(r["ACCOUNTNO"].ToString().Trim()) COMMENTED ALRAJHI IR SLOW RESPONSE GETTING ACCOUNT BALANCE;
                        urow["FUNDSVALUE"] = get_funds2(r["ACCOUNTNO"].ToString().Trim());

                        System.Threading.Thread.Sleep(2000);
                    }
                    catch (Exception e)
                    {
                        ErrorMessage.Add("ERROREXCOGEAWB" + e.Message);
                        audit.writeAudit(this.UserID, "EXCOERRGAWB", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                        urow["FUNDSVALUE"] = -1;
                    }
                    urow["AMOUNTTODEBIT"] = 0;
                    urow["CONVERTEDAMOUNT"] = 0;
                    urow["TOTALAMOUNT"] = 0;
                    dtNew.Rows.Add(urow);
                }
                return dtNew;
            }
            catch (Exception e)
            {
                try
                {
                    ErrorMessage.Add("ERROREXCOGEAWB" + e.Message);
                    audit.writeAudit(this.UserID, "EXCOERRGAWB", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose(); dt = null;
                }
                if (dtNew != null)
                {
                    dtNew.Dispose(); dtNew = null;
                }
            }
        }

        public DataTable get_exco_Accounts(string _branchcode)
        {
            string param = _branchcode;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_branch_accounts", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRGEA", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        public decimal get_funds2(string account_no)
        {
            /* BOYET AND JEFF NEW CODE TO GET ACCOUNT BALANCE 09152017 */
            /* E2EAPICHANGE 20220209 Updated Code */
            return Convert.ToDecimal(new irsAccountInquiry().getFC(account_no, irsAccountInquiry.FCMethod.AmountInquiry, 0, true));
        }

        public string validate_accounts(string _branchcode)
        {
            string param = _branchcode;
            string retmsg = "";
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_branch_accounts", param.Split(','));
            DataTable dt = new DataTable();
            try
            {
                dt = ds.Tables[0];
                foreach (DataRow r in dt.Rows)
                {
                    try
                    {
                        //this.get_funds(r["ACCOUNTNO"].ToString()); /* CBIP ALRAJHI IR UNABLE TO APPROVE TRANSACTION COMMENTED SLOW RESPONSE */
                        this.get_funds2(r["ACCOUNTNO"].ToString());
                    }
                    catch (Exception e)
                    {
                        retmsg += e.Message + "\n";

                    }
                }
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRVA", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { ds = null; dt = null; }
            return retmsg;
        }

        public ArrayList approve_items(string _branchcode, int _fileinvseqno, string _arrayitems, string _arrayfunds)
        {
            this.FileInvSeqno = _fileinvseqno;
            this.ArrayFunds = _arrayfunds;
            this.ArrayItems = _arrayitems;
            this.TransactingBranch = _branchcode;
            return this.process_items_for_debit();
        }

        public ArrayList force_push_items(string _branchcode, int _fileinvseqno, string _arrayitems, string _arrayfunds)
        {
            this.FileInvSeqno = _fileinvseqno;
            this.ArrayFunds = _arrayfunds;
            this.ArrayItems = _arrayitems;
            this.TransactingBranch = _branchcode;
            return this.process_forcepushitems_for_debit();
        }

        public bool send_exco_email(string _branchcode, string _emaildetails, string _fileformat, string _filetype)
        {
            csMailer mail = new csMailer();
            csAuditLog audit = new csAuditLog();
            bool issend = false;
            string errmsg;
            DataTable dt = null;

            try
            {
                string strSubject = "";
                string EmailAdd = "";
                string filesend = "";

                dt = this.EXCOEmailLookup(this.TransactingBranch);

                strSubject = dt.Select("FldCode = 'Subject'")[0][4].ToString() + " for " + dt.Rows[0]["BranchName"].ToString() + " " + System.DateTime.Now.ToShortDateString();

                EmailAdd = this.getRecipients(_branchcode, _fileformat, _filetype);
                string[] arrfiles = new string[] { filesend };

                issend = mail.sendMail(strSubject, EmailAdd.Split(';'), this.format_email(_emaildetails), arrfiles);
                if (issend)
                {
                    errmsg = "EXCO Email sent";
                    audit.writeAudit(this.UserID, "EXCOAPP_EXCOEmail", mail.Message, errmsg, "", csAuditLog.Category.Email);
                }
                else
                {
                    errmsg = "Failed to send EXCO Email.";
                    audit.writeAudit(this.UserID, "EXCOAPP_EXCOEmail", mail.Message, errmsg, "", csAuditLog.Category.Email);
                }
                return issend;
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOAPP_EXCOEmail", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return false;
            }
            finally { mail = null; audit = null; dt.Dispose(); dt = null; }
        }
        #endregion
        #region PRIVATE METHODS
        private string format_email(string _email_detail)
        {
            string[] arremail = _email_detail.Split('^');
            string formatted_email = "";
            foreach (string s in arremail)
            {
                formatted_email += s + "\n";
            }
            string extra_email = "This is a computer generated message. Please do not reply.";
            return formatted_email + extra_email;
        }

        private DataTable create_accounts_tables()
        {
            // Create a new DataTable titled 'Names.'
            DataTable accountsTable = new DataTable("EXCOAccounts");

            // Add three column objects to the table.
            DataColumn idColumn = new DataColumn();
            idColumn.DataType = System.Type.GetType("System.Int32");
            idColumn.ColumnName = "SEQ_ID";
            idColumn.AutoIncrement = true;
            accountsTable.Columns.Add(idColumn);

            DataColumn AccountsColumn = new DataColumn();
            AccountsColumn.DataType = System.Type.GetType("System.String");
            AccountsColumn.ColumnName = "ACCOUNTNO";
            accountsTable.Columns.Add(AccountsColumn);

            DataColumn CurrencyColumn = new DataColumn();
            CurrencyColumn.DataType = System.Type.GetType("System.String");
            CurrencyColumn.ColumnName = "CURRENCY";
            accountsTable.Columns.Add(CurrencyColumn);

            DataColumn CurrencyCodeColumn = new DataColumn();
            CurrencyCodeColumn.DataType = System.Type.GetType("System.Int32");
            CurrencyCodeColumn.ColumnName = "CURRENCYCODE";
            accountsTable.Columns.Add(CurrencyCodeColumn);

            DataColumn FundsColumn = new DataColumn();
            FundsColumn.DataType = System.Type.GetType("System.Decimal");
            FundsColumn.ColumnName = "FUNDSVALUE";
            accountsTable.Columns.Add(FundsColumn);

            DataColumn DebitColumn = new DataColumn();
            DebitColumn.DataType = System.Type.GetType("System.Decimal");
            DebitColumn.ColumnName = "AMOUNTTODEBIT";
            DebitColumn.DefaultValue = 0;
            accountsTable.Columns.Add(DebitColumn);

            DataColumn ConvertColumn = new DataColumn();
            ConvertColumn.DataType = System.Type.GetType("System.Decimal");
            ConvertColumn.ColumnName = "CONVERTEDAMOUNT";
            ConvertColumn.DefaultValue = 0;
            accountsTable.Columns.Add(ConvertColumn);

            DataColumn TotalDebitColumn = new DataColumn();
            TotalDebitColumn.DataType = System.Type.GetType("System.Decimal");
            TotalDebitColumn.ColumnName = "TOTALDEBIT";
            TotalDebitColumn.DefaultValue = 0;
            accountsTable.Columns.Add(TotalDebitColumn);

            DataColumn TotalColumn = new DataColumn();
            TotalColumn.DataType = System.Type.GetType("System.Decimal");
            TotalColumn.ColumnName = "TOTALAMOUNT";
            accountsTable.Columns.Add(TotalColumn);
            /*// Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = idColumn;
            CurrencyColumn.PrimaryKey = keys;
            // Return the new DataTable.*/
            return accountsTable;
        }

        private string getParseBalance(string _sparse)
        {
            string sbal = "";
            string[] sarr = _sparse.Split('\n');
            sbal = sarr[9].ToLower().Replace("available balance:", " ").Replace("\r", " ").Trim();
            return sbal;
        }

        private decimal get_funds(string _accountno)
        {
            irsAccountInquiry inq = new irsAccountInquiry();
            try
            {
                string errmsg = "";
                string sab = "";

                sab = inq.getFC(_accountno, irsAccountInquiry.FCMethod.BalanceInquiry, 0, true);                

                if (sab.Length > 0)
                {

                    try
                    {
                        sab = this.getParseBalance(sab);
                        return (Convert.ToDecimal(sab));
                    }
                    catch
                    {
                        throw new Exception(sab);
                    }
                }
                throw new Exception("ERROR: ACCOUNT INQUIRY FAILED :" + errmsg);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }
            finally
            {
                inq = null;
            }
        }

        private ArrayList process_items_for_debit()
        {
            string[] sfunds = this.ArrayFunds.Split(';');
            DataTable dt = null;
            ArrayList systemmsg = new ArrayList();
            string refnum = "";
            int record_ctr = 0;
            int record_loaded = 0;
            int record_errors = 0;
            string fileformat = "";
            string filetype = "";

            try
            {
                foreach (string s in sfunds)
                {
                    string[] sarr = s.Split('=');
                    if (sarr.Length == 3)
                    {
                        if (sarr[1].ToString() == "NaN")
                            continue;
                        if (sarr[1].ToString() == "0")
                            continue;

                        string acctnum = sarr[0].ToString();
                        string amt = sarr[1].ToString();
                        int acctcurr = Convert.ToInt32(sarr[2]);
                        string status = "";
                        //string message = "";  // commented to remove warning
                        dt = this.get_pnb_Accounts(this.TransactingBranch, acctcurr);
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                this.fcmessage = "";
                                status = "";
                                refnum = this.generate_refnum(this.Branchcode);
                                record_ctr = 0;
                                record_loaded = 0;
                                record_errors = 0;
                                foreach (string sSeqid in this.ArrayItems.Split(','))
                                {
                                    if (sSeqid.ToString() != "" || sSeqid.ToString().Length > 0)
                                    {
                                        record_ctr++;
                                        DataTable dtAItems = this.get_exco_items_approval(sSeqid);
                                        if (dtAItems.Rows.Count > 0)
                                        {
                                            fileformat = dtAItems.Rows[0]["fileformat"].ToString();
                                            filetype = dtAItems.Rows[0]["filetype"].ToString();
                                            filename = dtAItems.Rows[0]["filename"].ToString();
                                            if (this.move_to_remittance(dtAItems))
                                            {
                                                status += "SUCCESSFULLY MOVE TO REMITTANCE";
                                                if (this.update_approval_items(sSeqid))
                                                {
                                                    status += "SUCCESSFULLY UPDATED APPROVAL ITEMS";
                                                    record_loaded++;
                                                }
                                                else
                                                {
                                                    status += "ERROR WHILE APPROVING ITEMS";
                                                    this.ErrorMessage.Add("ERROR WHILE APPROVING ITEMS");
                                                    record_errors++;
                                                }
                                            }
                                            else
                                            {
                                                status += "ERROR WHILE MOVING TO REMITTANCE ";
                                                this.ErrorMessage.Add("ERROR WHILE MOVING TO REMITTANCE");
                                                record_errors++;
                                            }
                                        }
                                    }
                                }

                                if (record_ctr == record_loaded)
                                {
                                    if (this.debit_funds(refnum, acctnum, dt.Rows[0]["glAccount"].ToString(), Convert.ToDecimal(amt)))
                                    {
                                        if (this.update_batch_approval_items(this.ArrayItems))
                                        {
                                            status = ("SUCCESSFULLY UPDATED BATCH ITEMS");

                                        }
                                        else
                                        {
                                            status = ("ERROR WHILE UPDATING BATCH ITEMS ");
                                        }
                                    }
                                    else
                                    {
                                        status = ("ERROR WHILE DEBITING " + amt + " to ACCT: " + acctnum + " \n");
                                    }
                                }
                                else
                                {
                                    status = "ERROR WHILE SAVING ITEMS TO REMITTANCE " + this.error_messages() + "\n";
                                }

                                irsEXCORecord rec = new irsEXCORecord();
                                try
                                {
                                    rec.systemrefno = refnum;
                                    rec.transactiondate = new irsTimeZone().branchLocalTime(this.Branchcode).ToString("MM/dd/yyyy");
                                    rec.source_accountno = acctnum;
                                    rec.target_accountno = dt.Rows[0]["glAccount"].ToString();
                                    rec.debit_amount = amt;
                                    rec.source_currency = dt.Rows[0]["SOURCECURR"].ToString();
                                    rec.transactionrefno = this.fcmessage;
                                    rec.status_remarks = status;
                                    rec.filetype = filetype;
                                    rec.fileformat = fileformat;
                                    string EmailMessage = "";

                                    EmailMessage = "SYSTEMREFNO: " + refnum + "^";
                                    EmailMessage += "TRANX REFERENCENO: " + this.fcmessage + "^";
                                    EmailMessage += "TRANX DATE: " + rec.transactiondate + "                       FILENAME: " + filename + "^";
                                    EmailMessage += "Source Currency: " + rec.source_currency + "^";
                                    EmailMessage += "No of Items:" + record_ctr.ToString() + "                  Amount:" + amt + "^";

                                    rec.for_email_message = EmailMessage;
                                    systemmsg.Add(rec.return_record() + "~");
                                }
                                catch (Exception e)
                                {
                                    systemmsg.Add("ERROR IN WRITING REFERENCE DATA " + amt + " to ACCT: " + acctnum + " " + e.Message + " \n");
                                    this.exco_logger("P_I_F_D", e.Message, e.StackTrace, "throwerrfc");
                                }
                                finally { rec = null; }
                            }
                            catch (Exception e)
                            {
                                systemmsg.Add("ERROR IN DEBITING " + amt + " to ACCT: " + acctnum + " " + e.Message + " \n");
                                this.exco_logger("P_I_F_D", e.Message, e.StackTrace, "throwerrfc");
                            }
                        }
                    }
                }
                return systemmsg;
            }
            catch (Exception e)
            {
                this.exco_logger("P_I_F_D", e.Message, e.StackTrace, "throwerr");
                throw new Exception(e.Message, e.InnerException);
            }
        }

        private ArrayList process_forcepushitems_for_debit()
        {
            string[] sfunds = this.ArrayFunds.Split(';');
            DataTable dt = null;
            ArrayList systemmsg = new ArrayList();
            string refnum = "";
            int record_ctr = 0;
            int record_loaded = 0;
            int record_errors = 0;
            string fileformat = "";
            string filetype = "";

            try
            {
                foreach (string s in sfunds)
                {
                    string[] sarr = s.Split('=');
                    if (sarr.Length == 3)
                    {
                        if (sarr[1].ToString() == "NaN")
                            continue;
                        if (sarr[1].ToString() == "0")
                            continue;

                        string acctnum = sarr[0].ToString();
                        string amt = sarr[1].ToString();
                        int acctcurr = Convert.ToInt32(sarr[2]);
                        string status = "";
                        dt = this.get_pnb_Accounts(this.TransactingBranch, acctcurr);
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                this.fcmessage = "";
                                status = "";
                                refnum = this.generate_refnum(this.Branchcode);
                                record_ctr = 0;
                                record_loaded = 0;
                                record_errors = 0;
                                foreach (string sSeqid in this.ArrayItems.Split(','))
                                {
                                    if (sSeqid.ToString() != "" || sSeqid.ToString().Length > 0)
                                    {
                                        record_ctr++;
                                        DataTable dtAItems = this.get_exco_items_approval(sSeqid);
                                        if (dtAItems.Rows.Count > 0)
                                        {
                                            fileformat = dtAItems.Rows[0]["fileformat"].ToString();
                                            filetype = dtAItems.Rows[0]["filetype"].ToString();
                                            filename = dtAItems.Rows[0]["filename"].ToString();
                                            if (this.move_to_remittance(dtAItems))
                                            {
                                                status += "SUCCESSFULLY MOVE TO REMITTANCE";
                                                if (this.update_approval_items(sSeqid))
                                                {
                                                    status += "SUCCESSFULLY UPDATED APPROVAL ITEMS";
                                                    record_loaded++;
                                                }
                                                else
                                                {
                                                    status += "ERROR WHILE APPROVING ITEMS";
                                                    this.ErrorMessage.Add("ERROR WHILE APPROVING ITEMS");
                                                    record_errors++;
                                                }
                                            }
                                            else
                                            {
                                                status += "ERROR WHILE MOVING TO REMITTANCE ";
                                                this.ErrorMessage.Add("ERROR WHILE MOVING TO REMITTANCE");
                                                record_errors++;
                                            }
                                        }
                                    }
                                }

                                if (record_ctr == record_loaded)
                                {
                                    if (this.update_batch_approval_items(this.ArrayItems))
                                    {
                                        status = ("SUCCESSFULLY UPDATED BATCH ITEMS");

                                    }
                                    else
                                    {
                                        status = ("ERROR WHILE UPDATING BATCH ITEMS ");
                                    }
                                }
                                else
                                {
                                    status = "ERROR WHILE SAVING ITEMS TO REMITTANCE " + this.error_messages() + "\n";
                                }

                                irsEXCORecord rec = new irsEXCORecord();
                                try
                                {
                                    rec.systemrefno = refnum;
                                    rec.transactiondate = new irsTimeZone().branchLocalTime(this.Branchcode).ToString("MM/dd/yyyy");
                                    rec.source_accountno = acctnum;
                                    rec.target_accountno = dt.Rows[0]["glAccount"].ToString();
                                    rec.debit_amount = amt;
                                    rec.source_currency = dt.Rows[0]["SOURCECURR"].ToString();
                                    rec.transactionrefno = "";
                                    rec.status_remarks = status;
                                    rec.filetype = filetype;
                                    rec.fileformat = fileformat;
                                    string EmailMessage = "";

                                    EmailMessage = "SYSTEMREFNO: " + refnum + "^";
                                    EmailMessage += "TRANX REFERENCENO: " + this.fcmessage + "^";
                                    EmailMessage += "TRANX DATE: " + rec.transactiondate + "                       FILENAME: " + filename + "^";
                                    EmailMessage += "Source Currency: " + rec.source_currency + "^";
                                    EmailMessage += "No of Items:" + record_ctr.ToString() + "                  Amount:" + amt + "^";

                                    rec.for_email_message = EmailMessage;
                                    systemmsg.Add(rec.return_record() + "~");
                                }
                                catch (Exception e)
                                {
                                    systemmsg.Add("ERROR IN WRITING REFERENCE DATA " + amt + " to ACCT: " + acctnum + " " + e.Message + " \n");
                                    this.exco_logger("P_FPI_F_D", e.Message, e.StackTrace, "throwerrfc");
                                }
                                finally { rec = null; }
                            }
                            catch (Exception e)
                            {
                                systemmsg.Add("ERROR IN FORCE PUSHING " + amt + " to ACCT: " + acctnum + " " + e.Message + " \n");
                                this.exco_logger("P_FPI_F_D", e.Message, e.StackTrace, "throwerrfc");
                            }
                        }
                    }
                }
                return systemmsg;
            }
            catch (Exception e)
            {
                this.exco_logger("P_I_F_D", e.Message, e.StackTrace, "throwerr");
                throw new Exception(e.Message, e.InnerException);
            }
        }

        /* E2EAPICHANGE 20220209 UPDATED code */
        private bool debit_funds(string _refnum, string _sacct, string _tacct, decimal _amount)
        {
            SIcredit ft = new SIcredit();
            string ret = string.Empty;

            if (ft.fund_transfer(_refnum, _sacct, _tacct, _amount.ToString(), "IRSW (" + _refnum + ")", out ret))
            {
                fcmessage = ret;
                return true;
            }
            else
            {
                fcmessage = ret;
                return false;
            }
        }

        private string generate_refnum(string _branchcode)
        {
            string srefnum = "";
            DateTime dt = new irsTimeZone().branchLocalTime(_branchcode);
            string y = dt.Year.ToString();
            string m = dt.Month.ToString().PadLeft(2, '0');
            string d = dt.Day.ToString().PadLeft(2, '0');
            string h = dt.Hour.ToString().PadLeft(2, '0');
            string s = dt.Second.ToString().PadLeft(2, '0');
            srefnum = "IRSW" + _branchcode + y + m + d + h + s;
            return srefnum;
        }

        private bool move_to_remittance(DataTable dt)
        {
            irsEXCORemittance rem = new irsEXCORemittance(this.TransactingBranch, this.UserID);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    int acct_curr = Convert.ToInt32(dt.Rows[0]["TARGETCURRENCY"]);
                    double pay_amt = Convert.ToDouble(dt.Rows[0]["TARGETAMOUNT"]);
                    string[] sitems = this.prepare_params(dt.Rows[0]);

                    string tdate = Convert.ToDateTime(dt.Rows[0]["DATEUPLOADED"].ToString()).ToString("MM/dd/yyyy");

                    if (rem.check_items_in_rem(dt.Rows[0]["EXTERNALREFNO"].ToString(), Convert.ToDateTime(tdate), this.Branchcode))
                    {
                        return true;
                    }
                    else
                    {
                        if (this.getBranchDefaults())
                        {
                            rem.ProductChargeId = rem.setProductChargesID(this.TransactingBranch, dt.Rows[0]["PRODUCTCODE"].ToString(),
                                                dt.Rows[0]["ImplementationCode"].ToString(), dt.Rows[0]["REM_CURR"].ToString(), this.pay_mode.ToString());

                            if (this.TransactingBranch == "A06")
                            {
                                if (rem.RemitAlRajhi(acct_curr, pay_amt, sitems, this.filename))
                                {
                                    return true;
                                }
                                else
                                {
                                    if (rem.Message.ToLower().Contains("total_benef"))
                                    {
                                        this.ErrorMessage.Add("ERROR NO PRODUCT SETUP FOR THIS ITEM : " + dt.Rows[0]["PROD_DESC"].ToString() + ' ' + dt.Rows[0]["IMP_DESC"].ToString() + ' ' + dt.Rows[0]["ACCT_CURR"].ToString() + "\n");
                                    }
                                    else
                                    {
                                        this.ErrorMessage.Add(rem.RemitMessage);
                                    }
                                    return false;
                                }
                            }
                            else
                            {
                                if (rem.Remit(acct_curr, pay_amt, this.prepare_params(dt.Rows[0])))
                                {
                                    //rem.doOtherDetails
                                    return true;
                                }
                                else
                                {
                                    this.ErrorMessage.Add(rem.RemitMessage);
                                    return false;
                                }
                            }
                        }
                        return false;
                    }
                }
                return false;
            }
            catch // (Exception e) // commented to remove warning
            {
                return false;
            }
        }

        private string error_messages()
        {
            string stemp = "";
            foreach (string s in this.ErrorMessage)
            {
                stemp += s + "\n";
            }

            return stemp;
        }

        private DataTable get_exco_items_approval(string _seqid)
        {
            string param = _seqid;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_exco_items_approval", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRG_G_R_I", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        private bool update_approval_items(string _seqid)
        {
            try
            {
                string sql = "UPDATE TBL_EXCOCR SET APPROVALSTATUS = 3 WHERE SEQ_ID = " + _seqid;
                int ra = base.sqlUpdate(sql);

                if (ra > 0)
                {
                    this.exco_logger("U_A_I", " SUCCESSFULL UPDATE ", _seqid, "");
                    return true;
                }
                else
                {
                    this.exco_logger("U_A_I", " UNSUCCESSFULL UPDATE ", _seqid, "");
                    return false;
                }
            }
            catch (Exception e)
            {
                this.exco_logger("U_A_I", e.Message, e.StackTrace, "throwerr");
                return false;
            }
        }

        private bool update_batch_approval_items(string _valid_items)
        {
            try
            {
                string sql = "UPDATE TBL_EXCOCR SET APPROVALSTATUS = 2 WHERE SEQ_ID in (" + _valid_items + "-1)";
                int ra = base.sqlUpdate(sql);

                if (ra > 0)
                {
                    this.exco_logger("U_A_I", " SUCCESSFULL BATCH UPDATE EXCOCR ", _valid_items, "");

                    string sql2 = "UPDATE TBL_REMITTANCE SET TRANSTATUS = 16 , PRINTED = 1 , DATEEXTRACTED = getdate() WHERE ExtlReferenceNo in (SELECT AGENTTRANREFERENCENO FROM TBL_EXCOCR WHERE SEQ_ID in (" + _valid_items + "-1)) ";

                    int ra2 = base.sqlUpdate(sql2);

                    if (ra2 > 0)
                    {
                        this.exco_logger("U_A_I", " SUCCESSFULL BATCH UPDATE REMITTANCE ", _valid_items, "");
                        return true;
                    }
                    else
                    {
                        this.exco_logger("U_A_I", " UNSUCCESSFULL BATCH UPDATE REMITTANCE ", _valid_items, "");
                        return false;
                    }
                }
                else
                {
                    this.exco_logger("U_A_I", " UNSUCCESSFULL BATCH UPDATE ", _valid_items, "");
                    return false;
                }
            }
            catch (Exception e)
            {
                this.exco_logger("U_A_I", e.Message, e.StackTrace, "throwerr");
                return false;
            }
        }

        private DataTable get_pnb_Accounts(string _branchcode, int _currency)
        {
            string param = _branchcode + "," + _currency.ToString();
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_pnb_targetaccounts", param.Split(','));
            try
            {
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRG_P_A", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return null;
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        private void exco_logger(string _module, string _msg, string _keyvalue, string _oldvalue)
        {
            csAuditLog audit = new csAuditLog();
            try
            {
                audit.writeAudit(this.UserID, "EXCOAPP" + _module, _msg, _keyvalue, _oldvalue, csAuditLog.Category.Select);
            }
            catch
            {
            }
        }

        private string[] prepare_params(DataRow r)
        {
            string alrajhi_status = this.get_exco_default_stat();
            string[] param = new string[] 
            {
                "UserID~" + this.UserID,
                "RemitMethod~" + 1,
                //"RemitStationID~" + "",
                "RemittingBranch~" + this.TransactingBranch ,
                "ChargeLocally~" + false,
                "ExtlReferenceNo~" + r["EXTERNALREFNO"].ToString(),
                "Transtatus~" + alrajhi_status,
                "TranxDate~" + DateTime.Now.ToString(),
                //"ProductChargesID~" + charge_id.ToString(),
                "RemitterCIFID~" + r["REMITTERCIFID"].ToString(),
                "BeneficiaryCIFID~" + r["BENECIFID"].ToString() ,
                "AccountSEQID~" + r["ACCOUNTSEQID"].ToString(),
                "RemittingCurrency~" + r["REM_CURR"].ToString(),
                "TransactionSource~",
                "ReceiptMode~" + receipt_mode.ToString(),
                "TransactionPurpose~" + tranx_purpose.ToString(),
                "SourceofFunds~" + source_of_funds.ToString(),
                "DatePostedOverseas~" + new irsTimeZone().branchLocalTime(this.Branchcode).ToString(),
                "DatePostedLocal~" + new irsTimeZone().ServerTime,
                "BatchNo~0",
                "Printed~" + true ,
                "DateExtracted~" +  DBNull.Value.ToString(),
                "REMITTERADDRESSSEQID~" + r["REMITTERADDRESSSEQID"].ToString(),
                "REMITTERADDRESSSEQID~" +  r["REMITTERADDRESSSEQID"].ToString(),
                "BENEFICIARYADDRESSSEQID~" + r["BENEFICIARYADDRESSSEQID"].ToString(),
                "BENEFICIARYCONTACTSEQID~" + r["BENEFICIARYCONTACTSEQID"].ToString(),
                "RemitterAccountNo~" +   r["REMITTERACCOUNTNO"].ToString()
            };
            return param;
        }

        private bool getBranchDefaults()
        {
            //this.file_format = this.fileFormat();//spGetBranchDefaultSetup
            object[] br_param = new object[] { this.TransactingBranch };
            try
            {
                DataTable dt = execStoredProcedureTable("spGetBranchDefaultSetup", br_param);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        tranx_purpose = Convert.ToInt32(dt.Rows[0]["TranxPurpose"]);
                        tranx_source = Convert.ToInt32(dt.Rows[0]["TranxSource"]);
                        receipt_mode = Convert.ToInt32(dt.Rows[0]["ReceiptMode"]);
                        source_of_funds = Convert.ToInt32(dt.Rows[0]["SourceOfFunds"]);
                        pay_mode = Convert.ToInt32(dt.Rows[0]["PayMode"]);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                this.exco_logger("GBD", e.Message, e.StackTrace, "throwerr");
            }

            ErrorMessage.Add("Branch defaults must be set up first.");
            return false;
        }

        private string getRecipients(string _branchcode, string _fileformat, string _filetype)
        {
            DataTable dt = new DataTable();
            string param = _branchcode;
            DataSet ds = new DataSet();
            string srecipients = "";
            ds = this.execStoredProc("proc_get_exco_email_receipients", param.Split(','));

            try
            {
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        srecipients += r["email"].ToString() + ";";
                    }
                }
                return srecipients;
            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRG_G_R", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return "";
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }

        private DataTable EXCOEmailLookup(string _branchcode)
        {
            DataSet ds = new DataSet();
            //string srecipients = "";  // commented to remove warning
            string param = _branchcode;

            ds = this.execStoredProc("proc_get_email_lookup", param.Split(','));

            try
            {
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch
            {
            }
            return null;
        }

        public string credit_to_account(string _sacct, string _tacct, decimal _amount)
        {
            string refnum = "ITS" + DateTime.Now.ToString("yyyyMMddhhss");
            bool pass = this.debit_funds(refnum, _sacct, _tacct, _amount);

            return fcmessage;
        }

        private string get_exco_default_stat()
        {
            string param = this.TransactingBranch;
            DataSet ds = new DataSet();
            ds = this.execStoredProc("proc_get_alrajhi_default_Status", param.Split(','));
            try
            {
                return ds.Tables[0].Rows[0][0].ToString();

            }
            catch (Exception e)
            {
                try
                {
                    audit.writeAudit(this.UserID, "EXCOERRG_G_D_S", e.StackTrace, e.Message, "", csAuditLog.Category.Select);
                }
                catch { }
                return "10";
            }
            finally { if (ds != null) { ds.Dispose(); ds = null; } }
        }
        #endregion
    }

    #region irsEXCO Records
    public class irsEXCORecord
    {
        public string systemrefno = "";
        public string transactiondate = "";
        public string transactionrefno = "";
        public string source_accountno = "";
        public string source_currency = "";
        public string target_accountno = "";
        public string debit_amount = "";
        public string status_remarks = "";
        public string for_email_message = "";
        public string fileformat = "";
        public string filetype = "";

        public irsEXCORecord()
        {

        }

        public string return_record()
        {
            string s = this.systemrefno + ";" +
                        this.transactiondate + ";" +
                        this.transactionrefno + ";" +
                        this.source_accountno + ";" +
                        this.target_accountno + ";" +
                        this.debit_amount + ";" +
                        this.status_remarks + ";" +
                        this.for_email_message + ";" +
                        this.fileformat + ";" +
                        this.filetype + ";" +
                        this.source_currency + ";";
            return s;
        }
    }
    #endregion
}