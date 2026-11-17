using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsAccounts : sqlHelperWs
    {
        internal static TriDes getTriDes()
        {
            return new TriDes("xfm92.3", "IRSWorld");
        }


        public irsAccounts()
        {
            this.TableName = "tbl_branch_ibft_acct";
            this.IdentityColumn = "SEQ_ID";
        }

        public DataSet GetBranches(string location)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        public DataSet GetAccountCurrency(string brcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            //DataSet ds = commons.execStoredProcedure("sp_getCurrencyCategories", new object[] { brcode,2 });  //2 = account currency in tbl_lookup
            DataSet ds = getDataSet(commons.getSqlCommand("SELECT CURRENCY AS fldValue,DBO.F_GETLOOKUPVALUE('CURRENCY',CURRENCY) AS flddescription FROM TBL_CURRENCY_CATEGORIES WHERE BRANCHCODE='" + brcode + "' AND CURRENCYGROUP=2 AND checked =1",false));
            return ds;
        }

        public DataTable GetIBFTAccount(string brcode, int accountcurr)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("sp_get_BranchAccount", new object[] { brcode, accountcurr,"IBFT" });  //2 = account currency in tbl_lookup
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new DataColumn("SEQ_ID", Type.GetType("System.Int32")));
            dt.Columns.Add(new DataColumn("BranchCode", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("AccountNumber", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("AccountCurrency", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("PinBlock", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("CardNo", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("Deleted", Type.GetType("System.Boolean")));
            dt.Columns.Add(new DataColumn("PIN", Type.GetType("System.String")));
            dt.Columns.Add(new DataColumn("AccountType", Type.GetType("System.String")));
            


            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
               dr  = dt.NewRow();
               dr[0] = Convert.ToInt32(ds.Tables[0].Rows[i]["SEQ_ID"]);
               dr[1] = Convert.ToString(ds.Tables[0].Rows[i]["BranchCode"]);
               dr[2] = Convert.ToString(ds.Tables[0].Rows[i]["AccountNumber"]);
               dr[3] = Convert.ToString(ds.Tables[0].Rows[i]["AccountCurrency"]);
               dr[4] = Convert.ToBoolean(ds.Tables[0].Rows[i]["PinBlock"]);
               dr[5] = Convert.ToString(ds.Tables[0].Rows[i]["CardNo"]);
               dr[6] = Convert.ToBoolean(ds.Tables[0].Rows[i]["Deleted"]);
               dr[7] = getTriDes().Decrypt(Convert.ToString(ds.Tables[0].Rows[i]["PIN"]));
               dr[8] = Convert.ToString(ds.Tables[0].Rows[i]["AccountType"]);
               dt.Rows.Add(dr);

            }



            return dt;
        }

        public DataTable GetNSVSAccount(string brcode, int accountcurr)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("sp_get_BranchAccount", new object[] { brcode, accountcurr, "NSVS" });  //2 = account currency in tbl_lookup
            return ds.Tables[0];
        }

        public bool SaveIBFTAccount(Hashtable ht, string brcode,int acctcurrency,bool deleted)
        {
            try
            {
                
                string pinblock = (string)ht["PinBlock"];
                // remove warning about comparison
                //int seq_id = Convert.ToInt16((ht["SEQ_ID"]==string.Empty) ? 0 : ht["SEQ_ID"]);
                int seq_id = Convert.ToInt16(((String)ht["SEQ_ID"] == string.Empty) ? 0 : ht["SEQ_ID"]);
                string acctnumber = Convert.ToString(ht["AccountNumber"]);
                string cardno = Convert.ToString(ht["CardNo"]);
                string PIN = Convert.ToString(ht["PIN"]);
                string enc_pin = getTriDes().Encrypt(PIN);

                string accounttype = Convert.ToString(ht["AccountType"]);

                if (!deleted)
                {
                    if (acctnumber.Trim() == "" || acctnumber.Trim() == null)
                    {
                        Message = "Error! Account Number must not be empty";
                        return false;
                    }

                    if (cardno.Trim() == "" || cardno.Trim() == null)
                    {
                        Message = "Error! Card Number must not be empty";
                        return false;
                    }

                    object o = execScalar("Select count(SEQ_ID) from tbl_branch_ibft_acct where AccountNumber = '" + acctnumber + "' AND CardNo = '" + cardno + "' AND ISNULL(Deleted,0) != 1 AND BranchCode = '" + brcode + "' AND AccountCurrency = '" + acctcurrency + "'");
                    if (Convert.ToInt16(o) > 0)
                    {
                        Message = "Error! Duplicate record exist!";
                        return false;
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

                if (brcode.Length > 0)
                {
                    this.TableName = "TBL_BRANCH_IBFT_ACCT";
                    this.IdentityColumn = "SEQ_ID";
                    base.getUpdateDS("spGetIBFTAcct", new object[] { seq_id, "IBFT" });
                    
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    setColumn("AccountNumber", acctnumber);
                                    setColumn("PinBlock", pinblock);
                                    setColumn("AccountCurrency", acctcurrency);
                                    setColumn("CardNo", cardno);
                                    setColumn("PIN", enc_pin);
                                    setColumn("AccountType", accounttype);
                                    setColumn("Deleted", deleted);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BranchCode"] = brcode;
                                    uprow["AccountNumber"] = acctnumber;
                                    uprow["PinBlock"] = pinblock;
                                    uprow["AccountCurrency"] = acctcurrency;
                                    uprow["CardNo"] = cardno;
                                    uprow["AccountType"] = accounttype;
                                    uprow["PIN"] = enc_pin;
                                    uprow["Deleted"] = false;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = "BranchCode is missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        
        
        }

        public bool saveToTempIBFTAccount(Hashtable ht, string brcode, int acctcurrency, bool deleted, string userid)
        {
            try
            {

                string pinblock = (string)ht["PinBlock"];
                string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                Int64 seq_id = Convert.ToInt16(((String)ht["SEQ_ID"] == string.Empty) ? 0 : ht["SEQ_ID"]);
                string acctnumber = Convert.ToString(ht["AccountNumber"]);
                string cardno = Convert.ToString(ht["CardNo"]);
                string PIN = Convert.ToString(ht["PIN"]);
                string enc_pin = getTriDes().Encrypt(PIN);

                string accounttype = Convert.ToString(ht["AccountType"]);

                if (!deleted)
                {
                    if (acctnumber.Trim() == "" || acctnumber.Trim() == null)
                    {
                        Message = "Error! Account Number must not be empty";
                        return false;
                    }

                    if (cardno.Trim() == "" || cardno.Trim() == null)
                    {
                        Message = "Error! Card Number must not be empty";
                        return false;
                    }

                    object o = execScalar("Select count(SEQ_ID) from tbl_branch_ibft_acct where AccountNumber = '" + acctnumber + "' AND CardNo = '" + cardno + "' AND ISNULL(Deleted,0) != 1 AND BranchCode = '" + brcode + "' AND AccountCurrency = '" + acctcurrency + "'");
                    if (Convert.ToInt16(o) > 0)
                    {
                        Message = "Error! Duplicate record exist!";
                        return false;
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

                if (brcode.Length > 0)
                {
                    this.TableName = "TBL_BRANCH_IBFT_ACCT_TMP";
                    this.IdentityColumn = "SEQ_ID";
                    base.getUpdateDS("SELECT * FROM TBL_BRANCH_IBFT_ACCT_TMP WHERE 1=0");

                    if (isUpdatable)
                    {
                        if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            uprow["MAIN_SEQ_ID"] = seq_id;
                            uprow["BranchCode"] = brcode;
                            uprow["AccountNumber"] = acctnumber;
                            uprow["PinBlock"] = pinblock;
                            uprow["AccountCurrency"] = acctcurrency;
                            uprow["CardNo"] = cardno;
                            uprow["AccountType"] = accounttype;
                            uprow["PIN"] = enc_pin;
                            uprow["Deleted"] = deleted;
                            uprow["Maker"] = userid;
                            uprow["DateAdded"] = dateNow;
                            uprow["CurrStatus"] = "For Approval";
                            uprow["StatusDate"] = dateNow;
                            uprow["ActionType"] = (seq_id == 0) ? "ADD" : ((deleted == true) ? "DELETE" : "EDIT");
                            uprow["Module"] = "Branch Administration >> Accounts";
                            uprow["MenuName"] = "IBFT Account";
                            table.Rows.Add(uprow);
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = "BranchCode is missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;


        }

        public bool SaveNSVSAccount(Hashtable ht, string brcode, int acctcurrency, bool deleted)
        {
            try
            {
                // remove warning CS0252: Possible unintended reference comparison
                //int seq_id = Convert.ToInt16((ht["SEQ_ID"] == string.Empty) ? 0 : ht["SEQ_ID"]);
                int seq_id = Convert.ToInt16(((String)ht["SEQ_ID"] == string.Empty) ? 0 : ht["SEQ_ID"]);
                string acctnumber = Convert.ToString(ht["AccountNumber"]);

                if (!deleted)
                {
                    if (acctnumber.Trim() == "" || acctnumber.Trim() == null)
                    {
                        Message = "Error! Account number must not be empty";
                        return false;
                    }

                    object o = execScalar("Select count(SEQ_ID) from tbl_branch_nsvs_acct where AccountNumber = '" + acctnumber + "' AND ISNULL(Deleted,0) != 1 AND BranchCode = '" + brcode + "' AND AccountCurrency = '"+ acctcurrency +"' ");
                    if (Convert.ToInt16(o) > 0)
                    {
                        Message = "Error! Duplicate record exist!";
                        return false;
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
                

                
                

                if (brcode.Length > 0)
                {
                    this.TableName = "TBL_BRANCH_NSVS_ACCT";
                    this.IdentityColumn = "SEQ_ID";
                    base.getUpdateDS("spGetNSVSAcct", new object[] { seq_id, "NSVS" });
                    
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    setColumn("AccountNumber", acctnumber);
                                    setColumn("AccountCurrency", acctcurrency);
                                    setColumn("Deleted", deleted);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["BranchCode"] = brcode;
                                    uprow["AccountNumber"] = acctnumber;
                                    uprow["AccountCurrency"] = acctcurrency;
                                    uprow["Deleted"] = false;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = "BranchCode is missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool saveToTempNSVSAccount(Hashtable ht, string brcode, int acctcurrency, bool deleted, string userid)
        {
            string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            try
            {
                int seq_id = Convert.ToInt16(((String)ht["SEQ_ID"] == string.Empty) ? 0 : ht["SEQ_ID"]);
                string acctnumber = Convert.ToString(ht["AccountNumber"]);

                if (!deleted)
                {
                    if (acctnumber.Trim() == "" || acctnumber.Trim() == null)
                    {
                        Message = "Error! Account number must not be empty";
                        return false;
                    }

                    object o = execScalar("Select count(SEQ_ID) from tbl_branch_nsvs_acct where AccountNumber = '" + acctnumber + "' AND ISNULL(Deleted,0) != 1 AND BranchCode = '" + brcode + "' AND AccountCurrency = '" + acctcurrency + "' ");
                    if (Convert.ToInt16(o) > 0)
                    {
                        Message = "Error! Duplicate record exist!";
                        return false;
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

                if (brcode.Length > 0)
                {
                    this.TableName = "TBL_BRANCH_NSVS_ACCT_TMP";
                    this.IdentityColumn = "SEQ_ID";
                    base.getUpdateDS("SELECT * FROM TBL_BRANCH_NSVS_ACCT_TMP WHERE 1=0");

                    if (isUpdatable)
                    {
                        if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            uprow["MAIN_SEQ_ID"] = seq_id;
                            uprow["BranchCode"] = brcode;
                            uprow["AccountNumber"] = acctnumber;
                            uprow["AccountCurrency"] = acctcurrency;
                            uprow["Deleted"] = deleted;
                            uprow["Maker"] = userid;
                            uprow["DateAdded"] = dateNow;
                            uprow["CurrStatus"] = "For Approval";
                            uprow["StatusDate"] = dateNow;
                            uprow["ActionType"] = (seq_id == 0) ? "ADD" : ((deleted == true) ? "DELETE" : "EDIT");
                            uprow["Module"] = "Branch Administration >> Accounts";
                            uprow["MenuName"] = "NSVS Account";
                            table.Rows.Add(uprow);
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = "BranchCode is missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool saveUpdates()
        {
            return base.save(sqlHelperWs.RETAIN_DATASET);
        }

        public bool check_acct_num(string acctnum)
        {
            return new irsAccountAlgo().checkAccount(acctnum);
        }


        private bool allow_delete(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "seq_id";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
                return base.Delete(RETAIN_DATASET);
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



        //FOR GL ACCOUNT

        public DataTable getGLAccount(string brcode, int accountcurr)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("sp_get_BranchAccount", new object[] { brcode, accountcurr, "GL" });  //2 = account currency in tbl_lookup
            return ds.Tables[0];
        }

        public DataTable getGLFileType(string fldgroup)
        {

            DataSet ds;
            ds = execStoredProcedure("sp_get_FldDescription", new object[] { fldgroup });
            return ds.Tables[0];
        }

        public DataTable getGLCurrency(string fldgroup)
        {

            DataSet ds;
            ds = execStoredProcedure("sp_get_FldDescription", new object[] { fldgroup });
            return ds.Tables[0];
        }

        public DataTable getBranchCode(string branchtype)
        {

            DataSet ds;
            ds = execStoredProcedure("sp_get_BranchCode", new object[] { branchtype });
            return ds.Tables[0];
        }

        public DataTable getIbatchFileType(string fldgroup)
        {

            DataSet ds;
            ds = execStoredProcedure("sp_get_FldCode", new object[] { fldgroup });
            return ds.Tables[0];
        }

        public bool SaveGLAccount(Hashtable ht, string brcode, int acctcurrency, bool deleted)
        {
            try
            {
                string branchcode = (ht["branchcode"].Equals(string.Empty)) ? (string)ht["filetype"] : (string)ht["branchcode"];
                string glAccount = (string)ht["glAccount"];
                string glType = (string)ht["glType"];
                string glCurrency = (string)ht["glCurrency"];
                string description = (string)ht["description"];
                string recordid = (string)ht["recordID"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);
                
                if (brcode.Length > 0)
                {
                    this.TableName = "TBL_BRANCH_GL_ACCT";
                    this.IdentityColumn = "SEQ_ID";
                    base.getUpdateDS("spGetGLAcct", new object[] { seq_id, "GL" });

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("branchcode", branchcode);
                                    base.setColumn("glAccount", glAccount.Trim());
                                    base.setColumn("glType", glType);
                                    if (Convert.ToInt32(glType) == 1)
                                    {
                                        base.setColumn("glCurrency", glCurrency);
                                    }
                                    if (Convert.ToInt32(glType) == 2)
                                    {
                                        base.setColumn("glCurrency", 0);
                                    }
                                    base.setColumn("description", description.Trim());
                                    if (recordid == "" || recordid == "N/A")
                                    {
                                        base.setColumn("recordID", "-1");
                                    }
                                    if (recordid != "" && recordid != "N/A")
                                    {
                                        base.setColumn("recordID", recordid.Trim());
                                    }
                                    base.setColumn("deleted", deleted);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["branchcode"] = branchcode;
                                    uprow["glAccount"] = glAccount.Trim();
                                    uprow["glType"] = Convert.ToString(ht["glType"]);
                                    if (Convert.ToInt32(glType) == 1)
                                    {
                                        uprow["glCurrency"] = Convert.ToString(ht["glCurrency"]);
                                    }
                                    if (Convert.ToInt32(glType) == 2)
                                    {
                                        uprow["glCurrency"] = 0;
                                    }
                                    uprow["description"] = description.Trim();
                                    uprow["recordid"] = (recordid == "" || recordid == "N/A") ? "-1" : recordid.Trim();
                                    uprow["deleted"] = false;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = " missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool saveToTempGLAccount(Hashtable ht, string brcode, int acctcurrency, bool deleted, string userid)
        {
            string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            try
            {
                string branchcode = (ht["branchcode"].Equals(string.Empty)) ? (string)ht["filetype"] : (string)ht["branchcode"];
                string glAccount = (string)ht["glAccount"];
                string glType = (string)ht["glType"];
                string glCurrency = (string)ht["glCurrency"];
                string description = (string)ht["description"];
                string recordid = (string)ht["recordID"];
                Int64 seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt64(ht["SEQ_ID"]);

                if (brcode.Length > 0)
                {
                    this.TableName = "TBL_BRANCH_GL_ACCT_TMP";
                    this.IdentityColumn = "SEQ_ID";
                    base.getUpdateDS("SELECT * FROM TBL_BRANCH_GL_ACCT_TMP WHERE 1=0");

                    if (isUpdatable)
                    {
                        if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                        {
                            uprow["MAIN_SEQ_ID"] = seq_id;
                            uprow["branchcode"] = branchcode;
                            uprow["glAccount"] = glAccount.Trim();
                            uprow["glType"] = Convert.ToString(ht["glType"]);
                            if (Convert.ToInt32(glType) == 1)
                            {
                                uprow["glCurrency"] = Convert.ToString(ht["glCurrency"]);
                            }
                            if (Convert.ToInt32(glType) == 2)
                            {
                                uprow["glCurrency"] = 0;
                            }
                            uprow["description"] = description.Trim();
                            uprow["recordid"] = (recordid == "" || recordid == "N/A") ? "-1" : recordid.Trim();
                            uprow["deleted"] = deleted;
                            uprow["Maker"] = userid;
                            uprow["DateAdded"] = dateNow;
                            uprow["CurrStatus"] = "For Approval";
                            uprow["StatusDate"] = dateNow;
                            uprow["ActionType"] = (seq_id == 0) ? "ADD" : ((deleted == true) ? "DELETE" : "EDIT");
                            uprow["Module"] = "Branch Administration >> Accounts";
                            uprow["MenuName"] = "GL Account";
                            table.Rows.Add(uprow);
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = " missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }


    }
}
