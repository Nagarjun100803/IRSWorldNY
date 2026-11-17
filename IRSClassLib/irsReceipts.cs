using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
    [Serializable]
    public class irsReceipts : sqlHelperWs
    {
        public irsReceipts()
        {
            this.TableName = "tbl_receipts";
            this.IdentityColumn = "SEQ_ID";
        }

        public DataSet GetBranches(string location)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        public DataSet GetValidID(string brcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("sp_get_validid", new object[] { brcode });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }

        public bool SaveReceipts(string brcode, string values)
        {
            base.getUpdateDS("sp_getReceipts", new object[] { brcode });
            //begin split data
            string[] splitted_value = values.Split('|');

            // as instructed by Sir Mike Dizon, modified by dmp 2012-11-06
            //if (splitted_value[11].Trim() == string.Empty)
            //{
            //    Message = "No. of Copies must not be empty!";
            //    return false;
            //}

            try
            {
                if (table.Rows.Count > 0)
                {
                    int seq_id = Convert.ToInt32(table.Rows[0]["SEQ_ID"]);
                    selectedRow = getselectedRow(seq_id);
                    base.setColumn("Header1", splitted_value[0]);
                    base.setColumn("Header2", splitted_value[1]);
                    base.setColumn("NameOfBranch", splitted_value[2]);
                    base.setColumn("Address", splitted_value[3]);
                    base.setColumn("ContactNo", splitted_value[4]);
                    base.setColumn("Email", splitted_value[5]);
                    base.setColumn("corp_telno", splitted_value[6]);
                    base.setColumn("corp_address", splitted_value[7]);
                    base.setColumn("ValidID1", check_validid_ifnull(splitted_value[8]));
                    base.setColumn("ValidID2", check_validid_ifnull(splitted_value[9]));
                    base.setColumn("ValidID3", check_validid_ifnull(splitted_value[10]));
                    base.setColumn("ValidID4", check_validid_ifnull(splitted_value[11]));
                    base.setColumn("ReceiptForm", Convert.ToInt16(splitted_value[12]));
                    base.setColumn("NoOfCopies", check_validid_ifnull(splitted_value[13]));
                    base.setColumn("ReceiptType", Convert.ToInt16(splitted_value[14]));

                } 
                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                {
                    uprow["BranchCode"] = brcode;
                    uprow["Header1"] = splitted_value[0];
                    uprow["Header2"] = splitted_value[1];
                    uprow["NameOfBranch"] = splitted_value[2];
                    uprow["Address"] = splitted_value[3];
                    uprow["ContactNo"] = splitted_value[4];
                    uprow["Email"] = splitted_value[5];
                    uprow["corp_telno"] = splitted_value[6];
                    uprow["corp_address"] = splitted_value[7];
                    uprow["ValidID1"] = check_validid_ifnull(splitted_value[8]);
                    uprow["ValidID2"] = check_validid_ifnull(splitted_value[9]);
                    uprow["ValidID3"] = check_validid_ifnull(splitted_value[10]);
                    uprow["ValidID4"] = check_validid_ifnull(splitted_value[11]);
                    uprow["ReceiptForm"] = Convert.ToInt16(splitted_value[12]);
                    uprow["NoOfCopies"] = check_validid_ifnull(splitted_value[13]);
                    uprow["ReceiptType"] = check_validid_ifnull(splitted_value[14]);
                    table.Rows.Add(uprow);
                }
                base.addMode = false;
                return base.save(sqlHelperWs.RETAIN_DATASET);

            }
            catch (SqlException ex)
            {
                Message = ex.Message;
            }
            return false;
        }

        public bool saveToTemp(string brcode, string values, string userid)
        {

            base.getUpdateDS("sp_getReceipts", new object[] { brcode });
            string seq_id = "";
            string actionType = "";
            if(table.Rows.Count > 0)
            {
                seq_id = Convert.ToString(table.Rows[0]["SEQ_ID"]);
                actionType = "EDIT";
            }
            else //new record
            {
                seq_id = "0";
                actionType = "ADD";
            }

            

            string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            string[] splitted_value = values.Split('|');
            bool ok = false;

            try
            {
                base.addMode = true;
                this.setBase("SELECT TOP 1 * FROM TBL_RECEIPTS_TMP", true);
                setColumn("MAIN_SEQ_ID", seq_id);
                setColumn("BranchCode", brcode);
                setColumn("Header1", splitted_value[0]);
                setColumn("Header2", splitted_value[1]);
                setColumn("NameOfBranch", splitted_value[2]);
                setColumn("Address", splitted_value[3]);
                setColumn("ContactNo", splitted_value[4]);
                setColumn("Email", splitted_value[5]);
                setColumn("corp_telno", splitted_value[6]);
                setColumn("corp_address", splitted_value[7]);
                setColumn("ValidID1", check_validid_ifnull(splitted_value[8]));
                setColumn("ValidID2", check_validid_ifnull(splitted_value[9]));
                setColumn("ValidID3", check_validid_ifnull(splitted_value[10]));
                setColumn("ValidID4", check_validid_ifnull(splitted_value[11]));
                setColumn("ReceiptForm", Convert.ToInt16(splitted_value[12]));
                setColumn("NoOfCopies", check_validid_ifnull(splitted_value[13]));
                setColumn("ReceiptType", check_validid_ifnull(splitted_value[14]));
                setColumn("Maker", userid);
                setColumn("DateAdded", dateNow);
                setColumn("CurrStatus", "For Approval");
                setColumn("StatusDate", dateNow);
                setColumn("ActionType", actionType);
                setColumn("Module", "Branch Administration >> Receipts");
                setColumn("MenuName", "Receipts");

                ok = base.save(sqlHelperWs.RETAIN_DATASET);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                ok = false;
            }


            return ok;

        }

        public void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (!base.hasRows)
            {
                if (IsAdd)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    selectedRow = 0;
                }
            }
            else
            {
                if (IsAdd)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    selectedRow = 0;
                }
            }
        }

        public DataTable getReceipts(string brCode)
        {
            DataSet ds = execStoredProcedure("sp_getReceipts", new object[] { brCode });
            return ds.Tables[0];
        }


        private int getselectedRow(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "SEQ_ID";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                selectedRow = i;
            }
            return i;
        }

        private int check_validid_ifnull(string value)
        {
            if (value != "null" && value != string.Empty && value != null)
            {
                return Convert.ToInt32(value);
            }
            return 0;
        }
        /*
        public Boolean OREmailSending(ReportDocument rpt, string refno, string remID, out string errmsg)
        {
            csMailer mail = new csMailer();
            
            irsConfiguration config = new irsConfiguration();
            irsClientMaster cifmas = new irsClientMaster();
            bool issend;
            string[] filelist = null;
            string[] filesend =  new string[1];
            string[] EmailAdd = null;
            string remname = "";
            string strSubject = "";
            string strMsg = "";
            irsClientData cls = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]; 
            DataTable dt = null;
            int cifseq = 0;
            try
            {
                dt = getCIFContacts(remID);
                if (dt.Rows.Count > 0)
                {
                    EmailAdd = new string[dt.Rows.Count];
                    for (int i2 = 0; i2 <= dt.Rows.Count - 1; i2++)
                    {
                        EmailAdd[i2] = dt.Rows[i2][6].ToString();
                    }
                }
                else
                {
                    errmsg = "Remitter has no Email Address.";
                    return false;
                }

                cifseq = getCIFSEQ_ID(remID);
                if (cifmas != null)
                {
                    if (cifmas.get_record(cifseq.ToString()))
                    {
                        string lastname = cifmas.getdataRecord().Tables[0].Rows[0]["LastName"].ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                        string firstname = cifmas.getdataRecord().Tables[0].Rows[0]["FirstName"].ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                        string middlename = cifmas.getdataRecord().Tables[0].Rows[0]["MiddleName"].ToString().Replace("\\", "").Replace("/", "").Replace(".", "").Replace("*", "").Replace("?", "").Replace("\"", "").Replace("<", "").Replace(">", "");
                        remname = lastname + " " + firstname + " " + middlename;
                    }
                    else
                    {
                        errmsg = "Failed to get remitter details.";
                        return false;
                    }
                }

                if (System.IO.Directory.Exists(config.appsetting("ORReceipt")) == false)
                {
                    System.IO.Directory.CreateDirectory(config.appsetting("ORReceipt"));
                }
                rpt.ExportToDisk(ExportFormatType.PortableDocFormat, config.appsetting("ORReceipt") + "\\" + refno + "-" + remname + ".pdf");

                filelist = System.IO.Directory.GetFiles(config.appsetting("ORReceipt"));
                for (int i = 0; i <= filelist.Length - 1; i++)
                {
                    if (filelist[i].ToString().IndexOf(refno) > 0)
                    {
                        filesend[0] = filelist[i];
                    }
                }
                DataTable dtORLookup = ORLookup();
                if (dtORLookup == null || dtORLookup.Rows.Count == 0)
                {
                    errmsg = "Failed to get O.R. Email Details.";
                    return false;
                }
                strSubject = dtORLookup.Select("FldCode = 'Subject'")[0][4].ToString().Replace("<REFNO>", refno);
                strMsg = dtORLookup.Select("FldCode = 'EmailBody1'")[0][4].ToString().Replace("<REMITTERNAME>", remname) + Environment.NewLine + Environment.NewLine;
                strMsg += dtORLookup.Select("FldCode = 'EmailBody2'")[0][4].ToString().Replace("<REFNO>", refno) + Environment.NewLine + Environment.NewLine;
                strMsg += dtORLookup.Select("FldCode = 'EmailBody3'")[0][4].ToString() + Environment.NewLine;
                strMsg += dtORLookup.Select("FldCode = 'EmailBody4'")[0][4].ToString().Replace("<Branch>", cls.Location);
                issend = mail.sendMail(strSubject, EmailAdd, strMsg, filesend);
                if (issend)
                {
                    errmsg = "O.R. Email sent";
                }
                else
                {
                    errmsg = "Failed to send O.R. thru Email.";
                }
                return issend;
            }
            catch (Exception e)
            {
                errmsg = e.Message;
            }
            finally
            {                
                System.IO.File.Delete(filesend[0].ToString());
            }
            return false;
        }
        */
        public DataTable ORLookup()
        {
            try
            {
                SqlCommand sqcommands = base.getSqlCommand("select * from tbl_lookup where fldgroup = 'O.R. Email'", false);
                DataSet ds = base.getDataSet(sqcommands, "tbl_OR_lookup");
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

        public DataTable getCIFContacts(string cifid)
        {
            DataSet ds = null;

            try
            {
                /* DISPLAY ALL VALID EMAIL ADDRESS IF NOT BLANK */

                //old SqlCommand sqcommands = base.getSqlCommand("SELECT * FROM TBL_CIF_CONTACTS WHERE CIFID = '" + cifid + "' AND (ContactType = 8 OR ISNULL(EmailAddress,'') <> '')", false);
                //orig SqlCommand sqcommands = base.getSqlCommand("SELECT * FROM TBL_CIF_CONTACTS WHERE CIFID = '" + cifid + "' AND ISNULL(EmailAddress,'') LIKE '%_@__%.__%' AND ISNULL(EmailAddress,'') != ''", false);
                //DataSet ds = base.getDataSet(sqcommands, "tbl_CIF_Cont");

                ds = getDataSet("otc_or_email_sending", true, new object[] { cifid });

                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch
            {
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }

            return null;
        }

        public int getCIFSEQ_ID(string cifid)
        {
            try
            {
                SqlCommand sqcommands = base.getSqlCommand("SELECT * FROM TBL_CIF_MASTER WHERE CIFID = '" + cifid + "'", false);
                DataSet ds = base.getDataSet(sqcommands, "tbl_CIF_Mas");
                if (ds != null)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
            }
            catch
            {
            }
            return 0;
        }
        public string getReceiptNumber(int TranxSeqID, string Branchcode)
        {
            try
            {
                DataSet ds = execStoredProcedure("rptReceiptNumber", new object[] { TranxSeqID, Branchcode });
                if (ds != null)
                {
                    return Convert.ToString(ds.Tables[0].Rows[0][0]);
                }
            }
            catch
            {
            }
            return "";
        }
        /*modified by jmb 03.28.2011*/
        #region FOR FOOTER
        public DataSet get_init_state(string _brcode,int _isAdmin)
        {
            try
            {
                DataSet ds = this.exec_dynamicsp("proc_footer_init_state", new object[] { _brcode, _isAdmin });
                return ds;
            }
            catch 
            {
                return null;
            }
        }
        public DataTable getReportFooter(string _brcode)
        {
            try
            {
                DataSet ds = this.exec_dynamicsp("proc_get_footer_info", new object[] { _brcode });
                return ds.Tables[0];
            }             
            catch 
            {
                return null;
            }
        }
        private DataSet exec_dynamicsp(string _sp_name, object[] _param)
        {
            try
            {
                DataSet ds = execStoredProcedure(_sp_name, _param);
                return ds;
            }
            catch 
            {
                return null;
            }
        }

        #endregion
    }
}
