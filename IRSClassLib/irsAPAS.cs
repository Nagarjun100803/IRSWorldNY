using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace IRSClassLib
{
    [Serializable]
    public class irsAPAS: sqlHelperWs
    {        
    #region PRIVATE VARIABLES & METHODS            
        private string seq_id           = null;
        private string error_message    = null;
        private string api_message      = null;

        private bool update_me(string[] column_values, bool record_is_edit)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s[0].Trim().ToUpper() == "SEQ_ID")
                {
                    continue;
                }

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                    {
                        s[1] = null;
                    }
                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (error_list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    sb.Append((String)errors + "\n");
                }

                this.error_message = string.Empty;
                this.error_message = sb.ToString();

                return false;
            }

            return this.commit_record();
        }
        private bool commit_record()
        {
            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }
            this.get_db_err();return false;
        }
        private void rollback_record()
        {
            base.cancel();
        }              
        private void get_db_err()
        {
            this.error_message = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }
        private void update_APAS_for_dispo()// data for batch program for tagging for disposition trxn
        {
            try
            {
                if (base.sqlUpdate(base.getSqlCommand("sp_APAS_update_for_disposition")) == -1)
                {
                    //error log this.. base.Message();
                    
                }
                else 
                { 
                    //successful log this.. 
                }
            }
            catch
            { 
                this.get_db_err(); 
            }
        }

        public bool fundTransfer(string source_acct, string target_acct, string refno, string amt,Int32 seq_id)
        {
            /* E2EAPICHANGE 20220209 Updated Code */
            SIcredit si = new SIcredit();
            bool isSuccess = false;
            string update_script;

            isSuccess = si.fund_transfer(refno, source_acct, target_acct, amt, "APAS ("+ refno +")");
            if (!isSuccess)
            {
                this.Message = si.api_message;
                update_script = "api_msg~" + this.Message + "|" + "error_bit~" + true;
                this.update_record(update_script, seq_id);
                return false;
            }
            else
            {
                this.Message = si.api_message;
                update_script = "api_msg~" + this.Message + "|" + "error_bit~" + false;
                this.update_record(update_script, seq_id);
                return true;
            }

        }

        public DataSet CheckIfLockTransaction(Int32 SEQ_ID)
        {
            DataSet dsLock = getDataSet(getSqlCommand("SELECT isLocked,LockedBy,LockedBy AS LockedByID  FROM TBL_REMITTANCE_APAS WHERE SEQ_ID=" + SEQ_ID + "", false));
            return dsLock;
        }

        public DataSet CheckIfCancelTransaction(Int32 SEQ_ID)
        {
            DataSet dsLock = getDataSet(getSqlCommand("SELECT WithCancellation,CancelledBy,DisapprovedCancellation  FROM TBL_REMITTANCE_APAS WHERE SEQ_ID=" + SEQ_ID + "", false));
            return dsLock;
        }

        public DataSet CheckIfWithAmendments(Int32 SEQ_ID)
        {
            DataSet dsLock = getDataSet(getSqlCommand("SELECT AmendedBy,WithAmendments,ApprovedAmendments,CancelledAmendments FROM TBL_REMITTANCE_APAS WHERE SEQ_ID=" + SEQ_ID + "", false));
            return dsLock;
        }

        

    #endregion 

    #region PROPERTY GET & SET
        public string messages
        {
            get { return this.error_message; }
        }
    #endregion PROPERTY GET & SET

    #region PUBLIC METHODS        
        public irsAPAS()
        {
            this.TableName      = "TBL_REMITTANCE_APAS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message  = null;            
        }

        public irsAPAS(string tblname)
        {
            this.TableName = tblname;
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }
        public override void getUpdateDS(int id)
        {
            string sqlcmd = "";
            if (this.TableName.ToUpper() == "TBL_REMITTANCE_APAS")
            {
                sqlcmd = "SELECT * FROM TBL_REMITTANCE_APAS WHERE SEQ_ID = " + id; 
            }
            else if (this.TableName.ToUpper() == "TBL_PAYOUT_CURRENCY_TEMP")
            {
                sqlcmd = "SELECT * FROM TBL_PAYOUT_CURRENCY_TEMP WHERE SEQ_ID = " + id; 
            }
            else if (this.TableName.ToUpper() == "TBL_PAYOUT_PARTNER_SETUP_TEMP")
            {
                sqlcmd = "SELECT * FROM TBL_PAYOUT_PARTNER_SETUP_TEMP WHERE SEQ_ID = " + id; 
            }
            else if (this.TableName.ToUpper() == "TBL_PAYOUT_ALLOWED_CURRENCIES_TEMP")
            {
                sqlcmd = "SELECT * FROM TBL_PAYOUT_ALLOWED_CURRENCIES_TEMP WHERE SEQ_ID = " + id;
            }
            else if (this.TableName.ToUpper() == "TBL_PAYOUT_PARTNER_FEE_TEMP")
            {
                sqlcmd = "SELECT * FROM TBL_PAYOUT_PARTNER_FEE_TEMP WHERE SEQ_ID = " + id;
            }
            else if (this.TableName.ToUpper() == "TBL_APAS_AUDIT_LOG")
            {
                sqlcmd = "SELECT * FROM TBL_APAS_AUDIT_LOG";
            }
            else if (this.TableName.ToUpper() == "TBL_PAYOUT_ALLOWED_CURRENCIES_SUB_BRANCH_TEMP")
            {
                sqlcmd = "SELECT * FROM TBL_PAYOUT_ALLOWED_CURRENCIES_SUB_BRANCH_TEMP WHERE SEQ_ID = " + id;
            }
            

            //string sqlcmd = (this.TableName == "TBL_REMITTANCE_APAS") ? : 
            SqlCommand command = getSqlCommand(sqlcmd, false); 
            base.getUpdateDataSet(command);
        }

        public bool insert_record(string column_values)
        {
            bool ibool = false;

            string[] s = column_values.Split('|');

            if (base.newRow() == true)
            {
                ibool = this.update_me(s, false);
            }

            if (ibool == false)
            {
                this.get_db_err();
                this.rollback_record();
            }

            return ibool;
        }
        public bool update_record(string column_values, object transaction_id)
        {
            bool ibool = false;
            base.IdentityValue = transaction_id;
            base.selectedRow = 0;
            string[] s = column_values.Split('|');
            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                ibool = this.update_me(s, true);
            }

            if (ibool == false)
            {
                this.get_db_err();
                this.rollback_record();
            }
            return ibool;
        }

        public bool check_approver(string br, string uid, string pwd)
        {
            return false;
        }

        public string ordtrunkline()
        {
            object o = execScalar("SELECT FldDescription From TBL_LOOKUP WHERE FldGroup = 'ORDTrunkLine'");
            return Convert.ToString(o);
        }

        public DataSet getRequiredID(string branchcd)
        {
            DataSet ds = execStoredProcedure("sp_get_validid", new object[] { branchcd });
            return ds;
        }

        public string getUsername(string CIFID)
        {
            object o = execScalar("SELECT dbo.f_GetUserName('" + CIFID + "')");
            return Convert.ToString(o);
        }

        public string getBranchName(string brcode)
        {
            object o = execScalar("SELECT dbo.f_GetBranchName('" + brcode + "')");
            return Convert.ToString(o);
        }

        public string getBranchNamePerUser(string uid)
        {
            DataTable dt = execStoredProcedureTable("spAPASGetLocationPerUser", new object[] { uid });
            return Convert.ToString(dt.Rows[0]["BranchName"]);
        }

        public int getTranxStatus(string refno)
        {
            object o = execScalar("SELECT TransactionStatusCode FROM v_APAS_trxn WHERE REFERENCENO='"+  refno +"'");
            return Convert.ToInt16(o);
        }

        public bool getAgentPayoutCurrency(int currcode,string brcode,bool isHeadOffice)
        {
            object o;
         
            if (!isHeadOffice)
            {
                o = execScalar("SELECT count(*) FROM TBL_PAYOUT_ALLOWED_CURRENCIES_SUB_BRANCH WHERE CurrencyCode='" + currcode + "' AND SubBranchCode = '" + brcode + "' AND ISNULL(Deleted,0) != 1");
            }
            else
            {
                 o = execScalar("SELECT count(*) FROM TBL_PAYOUT_PARTNER_SETUP WHERE Payout_Curr='" + currcode + "' AND BranchCode = '" + brcode + "' AND ISNULL(Deleted,0) != 1");
            }
            
            
            return (Convert.ToInt16(o) > 0) ? true : false;
        }

        public string getMotherBranchCode(string brcode)
        {
            object o = execScalar("SELECT CoveringBankCode FROM TBL_BRANCHES WHERE BranchCode='"+ brcode +"'");
            return Convert.ToString(o);
        }

        public DataTable getSQL(string tblnm, string whereclause)
        {
            DataSet ds = getDataSet(getSqlCommand("SELECT * FROM " + tblnm + " " + whereclause,false));
            return ds.Tables[0];
        }


        public string tbl_src(string varid)
        {
            string src;
            DataTable dt = getDataSet(getSqlCommand("SELECT TABLESOURCE,WHERECONDITION FROM TBL_APAS_REPORT_FIELDS WHERE VarID='" + varid + "'", false)).Tables[0];
            src = Convert.ToString(dt.Rows[0]["TableSource"]) + "," + Convert.ToString(dt.Rows[0]["WHERECONDITION"]);
            return src;
        }

        public DataSet getAPASReceipt(string refno)
        {
            return execStoredProcedure("sp_RPT_APASReceipt", new object[] { refno });
        }

        public string CIFClass(string cifid)
        {
            object cif_class = execScalar("SELECT CIFClass FROM TBL_CIF_CLASS WHERE CIFID='" + cifid + "' AND ISNULL(Deleted,0) != 1");
            return (cif_class.ToString() == null) ? string.Empty : cif_class.ToString();
        }

        public DataTable ViewAmendments()
        {
            DataSet ds = execStoredProcedure("spAPAS_ViewAmendments", new object[] { null });
            return ds.Tables[0];
        }

        public decimal getTransactionLimit(string brcode,string curr_code)
        {
            DataTable dt = execStoredProcedureTable("spAPASGetTransactionLimit", new object[] { brcode,curr_code });
            return Convert.ToDecimal(dt.Rows[0]["TransactionLimit"].ToString());
        }

        public bool isHeadOffice(string brcode)
        {
            object o = execScalar("SELECT COUNT(*) FROM vwParentBranchList WHERE BranchCode='" + brcode + "'");
            return (Convert.ToInt32(o) > 0) ? true : false;
        
        }

        public string getAllowedCurrencies(string branchcode)
        {
            String currency="";
            DataTable dt = execStoredProcedureTable("spGetAPASAllowedCurrency", new object[] { branchcode });
            if (dt.Rows.Count > 0)
            { 
                for(int n=0;n<=dt.Rows.Count-1;n++)
                {
                    currency += Convert.ToString(dt.Rows[n]["Currency"]) + ",";
                }
                return currency;
            }
            return string.Empty;

        }

        public bool isHold(int seq_id)
        {
            object o = execScalar("SELECT OnHold FROM TBL_REMITTANCE_APAS WHERE SEQ_ID = " + seq_id + " ");
            return Convert.ToBoolean(o);
        }

        public string getCurrencyCode(int currcode)
        {
            object o = execScalar("SELECT fldcode FROM TBL_LOOKUP WHERE fldvalue = " + currcode + " AND fldgroup = 'Currency'");
            return Convert.ToString(o);
        }

        public string getPartnerAccountNo(string partnercode, int currencycode)
        {
            object o = execScalar("SELECT AcctNum FROM TBL_PAYOUT_PARTNER_SETUP Where BranchCode='" + partnercode + "' AND Payout_Curr= " + currencycode + " AND ISNULL(Deleted,0) != 1");
            return Convert.ToString(o);
        }


        public DataTable ViewSetup(string brcode)
        {
            if (brcode == null)
            {
                return null;
            }

            DataSet ds = execStoredProcedure("spGetPayoutPartnerSetup", new object[] { brcode });
            return ds.Tables[0];
        }


        public DataTable ViewPartnerFee(string brcode)
        {
            if (brcode == null)
            {
                return null;
            }

            DataSet ds = execStoredProcedure("spGetPayoutPartnerFee", new object[] { brcode });
            return ds.Tables[0];
        }

        public DataTable getPayoutCurrency()
        {
            DataSet ds;
            ds = getDataSet(getSqlCommand("SELECT Currency,dbo.f_GetLookupValue('Currency',Currency) AS CurrencyDesc FROM TBL_PAYOUT_CURRENCY", false));
            return ds.Tables[0];
        }

        public DataTable getPayoutCurrency(string branchcode)
        {
            DataSet ds;
            ds = execStoredProcedure("spGetAPASPayoutCurrencyMotherBranch", new object[] { branchcode });
            return ds.Tables[0];
        }

        public DataSet getAllOnBehalfBranches(string parentcode)
        {
            DataSet ds;
            ds = execStoredProcedure("spGetOnBehalfPayoutPartners", new object[] { parentcode });
            return ds;
        }

        public DataTable getReportData(int roleid,string rptid)
        {
            DataSet ds;
            ds = getDataSet(getSqlCommand("SELECT * FROM TBL_APAS_REPORTS WHERE RPTID='"+ rptid +"' AND RoleID= "+ roleid +"", false));
            return ds.Tables[0];
        }

        public string getDataType(string varid)
        {
            DataSet ds;
            object o = execScalar("SELECT VarType FROM TBL_APAS_REPORT_FIELDS Where VarID='" + varid + "'");
            return Convert.ToString(o);
        }

        public string getORDAccountNo(int currcode,int moduleid)
        {
            object o = execScalar("SELECT ORDAccountNo FROM TBL_PAYOUT_CURRENCY Where Currency=" + currcode + " AND ModuleID = '"+ moduleid +"'");
            return Convert.ToString(o);
        }

        public string getBranchAccountNo(int currcode, string branchcode)
        {
            object o = execScalar("SELECT AccountNumber FROM tbl_branch_nsvs_acct Where AccountCurrency=" + currcode + " AND BranchCode = '"+ branchcode +"' AND ISNULL(Deleted,0) != 1");
            return Convert.ToString(o);
        }

        public DataSet getListPartners()
        {
            DataSet ds = getDataSet(getSqlCommand("SELECT DISTINCT MotherBranchCode,MotherBranchName FROM vwSubBranch_SETUP", false));
            return ds;
        }

        public DataTable getSubBranch(string partnercode)
        {
            DataSet ds = getDataSet(getSqlCommand("SELECT DISTINCT BranchCode,BranchName FROM vwSubBranch_SETUP WHERE MotherBranchCode = '" + partnercode + "'", false));
            return ds.Tables[0];
        }

        public DataTable getCurrencyPerPartner(string partnercode,string subbranch)
        {
            return execStoredProcedureTable("spGetAPASCurrency", new object[] { partnercode, subbranch });
            
        }


        public Object[] getFs(String root)
        {
            short seqno = 0;
            short parid = -1;
            String item = "IRSWorld";
            ArrayList al = new ArrayList();
            al.Add(new Object[] { seqno, parid, item, "R" });

            listDirectories(root, ref seqno, seqno, al);

            if (al.Count > 0)
            {
                Object[] lsdir = new Object[al.Count];
                al.CopyTo(lsdir);

                return lsdir;
            }

            return null;
        }


        #region FOR MAINTENANCE

        public bool check_account_number(string accountno)
        {
            return new irsAccountAlgo().checkAccount(accountno);
        }

        public void listDirectories(String s, ref short seqno, short parid, ArrayList al)
        {
            String[] ff = Directory.GetFileSystemEntries(s);
            foreach (String f in ff)
            {
                FileAttributes fa = File.GetAttributes(f);
                if (fa > 0)
                {
                    if (fa == FileAttributes.Directory)
                    {
                        seqno++;
                        al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "D" });
                        listDirectories(f, ref seqno, seqno, al);
                    }
                    else
                    {
                        String ext = Path.GetExtension(f);
                        if (ext != null && ext.Length > 0)
                        {
                            ext = ext.ToUpper();
                            if (ext.Equals(".JPEG") | ext.Equals(".GIF") | ext.Equals(".PNG") | ext.Equals(".BMP") | ext.Equals(".JPG"))
                            {
                                seqno++;
                                al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "F" });
                            }
                        }
                    }
                }
            }
        }

        public bool payout_setup(Hashtable ht, string brcode, bool deleted, bool isedit,string AddedBy,DateTime DateAdded,string logo_file)
        {
            try
            {
                String payout_curr = Convert.ToString(ht["Payout_Curr"]);
                String trxn_limit = Convert.ToString(ht["TransactionLimit"]);
                String Acctnum = ht["AcctNum"].ToString();
                String Logo="";

                if (logo_file.Length > 0)
                {
                    /*
                    Logo = logo_file;
                    String file_nm = Path.GetFileName(Logo);
                    if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("\\images\\APAS-Logo")))
                    {
                        Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("\\images\\APAS-Logo"));
                    }
                    File.Copy(Logo, System.Web.HttpContext.Current.Server.MapPath("\\images\\APAS-Logo\\" + file_nm));
                    Logo = "\\images\\APAS-Logo\\" + file_nm;
                    */
                    Logo = logo_file;
                    String file_nm = Path.GetFileName(Logo);
                    string logo_server_path = new System.Configuration.AppSettingsReader().GetValue("LogoPath", Type.GetType("System.String")).ToString();
                    string isMapPath = new System.Configuration.AppSettingsReader().GetValue("UseMapPath", Type.GetType("System.String")).ToString();
                    
                    if (Convert.ToBoolean(isMapPath))
                    {
                        GetCallbackResult(Logo, logo_server_path);//File.Copy(Logo, System.Web.HttpContext.Current.Server.MapPath(logo_server_path + file_nm), true);
                    }
                    else
                    {
                        GetCallbackResult(Logo, logo_server_path);//File.Copy(Logo, logo_server_path + file_nm, true);
                    }

                    Logo = System.Web.HttpContext.Current.Server.MapPath(logo_server_path) + file_nm;
                }

                bool onbehalf = ht["OnBehalf"].ToString() == "on" ?  false : Convert.ToBoolean(ht["OnBehalf"].ToString());
                int seq_id = (ht["SEQ_ID"]==string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (trxn_limit == null || trxn_limit == string.Empty)
                {
                    Message = "Transaction Limit must not be empty";
                    return false;
                }
                else if (payout_curr == string.Empty || payout_curr == null)
                {
                    Message = "Please select payout currency";
                    return false;
                }

                else if (Acctnum == null || Acctnum == string.Empty)
                {
                    Message = "Account Number must not be empty";
                    return false;
                }
                //else if (!IsNumeric(trxn_limit))
                //{
                //    Message = "Transaction Limit must be in number format";
                //    return false;
                //}

                if (!deleted)
                {
                    if (!isedit)
                    {
                        object o = execScalar("SELECT count(*) FROM TBL_PAYOUT_PARTNER_SETUP WHERE Payout_Curr = " + payout_curr + " AND BranchCode='" + brcode + "' AND ISNULL(Deleted,0) != 1");
                        if (Convert.ToInt32(o) > 0)
                        {
                            Message = "There is an existing setup for this currency";
                            return false;
                        }
                    }
                    else
                    {
                        if (seq_id == 0)
                        {
                            Message = "Unable to edit row when in add-mode";
                            return false;
                        }
                    }
                }
                else
                {
                    if (seq_id == 0)
                    {
                        Message = "Unable to delete row when in add-mode";
                        return false;
                    }
                }

                if (payout_curr.Length > 0)
                {
                    //base.getUpdateDS("sp_get_validid", new Object[] { brcode });
                    base.getUpdateDS("SELECT * FROM TBL_PAYOUT_PARTNER_SETUP_TEMP WHERE SEQ_ID=" + seq_id + " AND forApproval = 1");
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("Payout_Curr", payout_curr);
                                    base.setColumn("TransactionLimit", trxn_limit);
                                    base.setColumn("AcctNum", Acctnum);
                                    base.setColumn("Logo", Logo);
                                    base.setColumn("AddedBy", AddedBy);
                                    base.setColumn("DateAdded", DateAdded);
                                    base.setColumn("Deleted", deleted);
                                    base.setColumn("ForApproval", true);
                                    base.setColumn("Action", (!deleted) ? "UPDATE" : "DELETE");
                                    base.setColumn("OnBehalfOf", onbehalf);
                                    base.setColumn("SEQ_ID_TEMP", seq_id);

                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BranchCode"] = brcode;
                                    uprow["Payout_Curr"] = payout_curr;
                                    uprow["TransactionLimit"] = trxn_limit;
                                    uprow["AcctNum"] =  Acctnum;
                                    uprow["Logo"] =  Logo;
                                    uprow["Deleted"] =  deleted;
                                    uprow["AddedBy"] = AddedBy;
                                    uprow["DateAdded"] = DateAdded;
                                    if (isedit)
                                    {
                                        uprow["Action"] = "UPDATE";
                                    }
                                    else if (deleted)
                                    {
                                        uprow["Action"] = "DELETE";
                                    }
                                    else
                                    {
                                        uprow["Action"] = "INSERT";
                                    }
                                    uprow["ForApproval"] = true;
                                    uprow["OnBehalfOf"] = onbehalf;
                                    uprow["SEQ_ID_TEMP"] = seq_id;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return true;
                    }
                }
                else
                {
                    Message = "Payout Currency Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }


        public bool partner_fee_setup(Hashtable ht, string brcode, bool deleted, bool isedit, string AddedBy, DateTime DateAdded)
        {
            try
            {
                String fee_code = Convert.ToString(ht["FeeCode"]);
                String desc = Convert.ToString(ht["Description"]);
                Double min_num = Convert.ToDouble(ht["MinNum"]);
                Double max_num = Convert.ToDouble(ht["MaxNum"]);
                int payout_curr = Convert.ToInt32(ht["PayoutCurr"]);
                decimal fee_amount = Convert.ToDecimal(ht["FeeAmount"]);
                int seq_id = (ht["SEQ_ID"] == string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

               

                if (!deleted)
                {
                    if (!isedit)
                    {
                        object o = execScalar("SELECT count(*) FROM TBL_PAYOUT_PARTNER_FEE_TEMP WHERE BranchCode = '"+ brcode +"' ISNULL(Deleted,0) != 1 AND PayoutCurr = "+ payout_curr +"");
                        if (Convert.ToInt32(o) > 0)
                        {
                            Message = "There is an existing setup for this fee";
                            return false;
                        }
                    }
                    else
                    {
                        if (seq_id == 0)
                        {
                            Message = "Unable to edit row when in add-mode";
                            return false;
                        }
                    }
                }
                else
                {
                    if (seq_id == 0)
                    {
                        Message = "Unable to delete row when in add-mode";
                        return false;
                    }
                }

                if (fee_code.Length > 0)
                {
                    //base.getUpdateDS("sp_get_validid", new Object[] { brcode });
                    base.getUpdateDS("SELECT * FROM TBL_PAYOUT_PARTNER_FEE_TEMP WHERE SEQ_ID=" + seq_id + " AND forApproval = 1");
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("FeeCode", fee_code);
                                    base.setColumn("Description", desc);
                                    base.setColumn("MinNum", min_num);
                                    base.setColumn("MaxNum", max_num);
                                    base.setColumn("FeeAmount", fee_amount);
                                    base.setColumn("DateAdded", DateAdded);
                                    base.setColumn("AddedBy", AddedBy);
                                    base.setColumn("ForApproval", true);
                                    base.setColumn("PayoutCurr", payout_curr);
                                    base.setColumn("Action", (!deleted) ? "UPDATE" : "DELETE");
                                    base.setColumn("SEQ_ID_TEMP", seq_id);
                                    base.setColumn("Branchcode", brcode);

                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BranchCode"] = brcode;
                                    uprow["FeeCode"] = fee_code;
                                    uprow["Description"] = desc;
                                    uprow["MinNum"] = min_num;
                                    uprow["MaxNum"] = max_num;
                                    uprow["FeeAmount"] = fee_amount;
                                    uprow["PayoutCurr"] = payout_curr;
                                    uprow["Deleted"] = deleted;
                                    uprow["AddedBy"] = AddedBy;
                                    uprow["DateAdded"] = DateAdded;
                                    uprow["forApproval"] = true;
                                    if (isedit)
                                    {
                                        uprow["Action"] = "UPDATE";
                                    }
                                    else if (deleted)
                                    {
                                        uprow["Action"] = "DELETE";
                                    }
                                    else
                                    {
                                        uprow["Action"] = "INSERT";
                                    }
                                    uprow["SEQ_ID_TEMP"] = seq_id;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return true;
                    }
                }
                else
                {
                    Message = "Fee Code Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }




        private int getselectedRow(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "SEQ_ID";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
            }
            return i;
        }

        public bool saveUpdates()
        {
            return base.save(sqlHelperWs.RETAIN_DATASET);
        }

        public bool GetCallbackResult(string returnValue, string serverpath)
        {
            string fileName = System.IO.Path.GetFileName(returnValue);
            string path = System.Web.HttpContext.Current.Server.MapPath(serverpath);
            string fullPath = path + fileName;

            Stream s = File.OpenRead(returnValue);

            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, (int)s.Length);

            int len = (int)s.Length;

            s.Dispose();
            s.Close();

            FileStream fs = new FileStream(fullPath, FileMode.Create);
            fs.Write(buffer, 0, len);

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(fs);

            
            if (System.IO.Path.GetExtension(returnValue).Equals(".gif"))
            {
                bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
            }
            else
            {
                bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            bmp.Dispose();

            fs.Dispose();
            fs.Close();

            return true;
        }




        #endregion




    #endregion
    }   
}
