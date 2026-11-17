using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsTranxFileMaintenance : sqlHelperWs
    {
        private string _branch;
        private string _branch_email;
        private string _user;

        public int BranchEmail
        {
            set
            {
                this._branch_email = getBranch(value);
            }
        }

        public DataTable GetEMGridValue(string filetype, string fileformat)
        {
            SqlCommand cmd = getSqlCommand("sp_grid_filemail");
            DataSet ds = base.execStoredProcedure(cmd, new object[] {filetype, fileformat, this._branch_email });
            //return ReturnValue(ds.Tables[0]);
            return ds.Tables[0];
        }

        public DataTable GetMSGridValue(string filetype, string fileformat)
        {
            SqlCommand cmd = getSqlCommand("sp_grid_fileprod");
            DataSet ds = base.execStoredProcedure(cmd, new object[] { filetype, fileformat });
            //return ReturnValue(ds.Tables[0]);
            return ds.Tables[0];
        }

        public DataTable GetBSGridValue(string filetype, string fileformat)
        {
            SqlCommand cmd = getSqlCommand("sp_grid_batchSetup");
            DataSet ds = base.execStoredProcedure(cmd, new object[] { filetype, fileformat });
            //return ReturnValue(ds.Tables[0]);
            return ds.Tables[0];
        }

        public DataTable getProductCode(string fldgroup)
        {
            irsClientData client = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            DataSet ds;
            ds = execStoredProcedure("sp_get_FldDescription", new object[] { fldgroup });

            client = null;
            return ds.Tables[0];
        }

        public DataTable getImplementationType(string fldgroup)
        {

            DataSet ds;
            ds = execStoredProcedure("sp_get_FldDescription", new object[] { fldgroup });
            return ds.Tables[0];
        }

        public DataTable ReturnValue(DataTable dt)
        {
            DataTable dtNew = dt.Clone();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow urow = dtNew.NewRow();
                    urow["SEQ_ID"] = dr["SEQ_ID"];
                    urow["FileType"] = dr["FileType"];
                    urow["FileFormat"] = dr["FileFormat"];
                    urow["Email"] = dr["Email"];
                    urow["Deleted"] = dr["deleted"];
                    urow["Upload"] = dr["Upload"];
                    urow["Process"] = dr["Process"];

                    dtNew.Rows.Add(urow);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow urow = dtNew.NewRow();
                    urow["SEQ_ID"] = dr["SEQ_ID"];
                    urow["FileType"] = dr["FileType"];
                    urow["FileFormat"] = dr["FileFormat"];
                    urow["FileCode"] = dr["FileCode"];
                    urow["ProductCode"] = dr["ProductCode"];
                    urow["ImplementationType"] = dr["ImplementationType"];
                    urow["ForRetrap"] = dr["ForRetrap"];
                    urow["Deleted"] = dr["Deleted"];

                    dtNew.Rows.Add(urow);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    DataRow urow = dtNew.NewRow();
                    urow["SEQ_ID"] = dr["SEQ_ID"];
                    urow["FileType"] = dr["FileType"];
                    urow["FileFormat"] = dr["FileFormat"];
                    urow["AmountLimit"] = dr["AmountLimit"];
                    urow["BatchLimit"] = dr["BatchLimit"];
                    urow["UserGroup"] = dr["UserGroup"];
                    urow["BatchFormat"] = dr["BatchFormat"];
                    urow["Batchctr"] = dr["Batchctr"];
                    urow["BatchNumLength"] = dr["BatchNumLength"];
                    urow["Deleted"] = dr["Deleted"];
                    dtNew.Rows.Add(urow);
                }

            }
            return dtNew;
        }

       


        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            if (crud.Equals("DELETE"))
            {
                selectedRow = row;
                return base.Delete(sqlHelperWs.RETAIN_DATASET);
            }
            bool ok = false;
            if (crud.Equals("EDIT"))
            {
                selectedRow = row;
                ok = base.edit();
            }
            if (crud.Equals("ADD"))
            {
                ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (!setColumn(key.ToString(), ht[key]))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok & addMode)
                {
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }
            return ok;
        }


        public bool saveUpdates()
        {
            return base.save(sqlHelperWs.RETAIN_DATASET);
        }

        public bool save_file_email(Hashtable ht, string filetype, string fileformat, bool deleted)
        {
            try
            {
                string email = (string)ht["Email"];
                string upload = (string)ht["Upload"];
                string process = (string)ht["Process"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);
                bool chkUpload = ht["Upload"].Equals("true") ? true : false;
                bool chkProcess = ht["Process"].Equals("true") ? true : false;

                if (filetype.Length > 0 && fileformat.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_FILE_EMAIL WHERE SEQ_ID= " + seq_id + "";
                    base.getUpdateDS(sqlstmt);

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("Email", email);
                                    base.setColumn("Deleted", deleted);
                                    base.setColumn("Upload", chkUpload);
                                    base.setColumn("Process", chkProcess);
                                    base.setColumn("Branch", this._branch_email);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["FileType"] = filetype;
                                    uprow["FileFormat"] = fileformat;
                                    uprow["Email"] = Convert.ToString(ht["Email"]);
                                    uprow["Deleted"] = false;
                                    uprow["Upload"] = chkUpload;
                                    uprow["Process"] = chkProcess;
                                    uprow["Branch"] = this._branch_email;
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

        public string getBranch()
        {
            return this._branch_email;
        }

        public bool save_file_email_tmp(Hashtable ht, string filetype, string fileformat, bool deleted, string userID)
        {
            try
            {
                string email = (string)ht["Email"];
                string upload = (string)ht["Upload"];
                string process = (string)ht["Process"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);
                bool chkUpload = ht["Upload"].Equals("true") ? true : false;
                bool chkProcess = ht["Process"].Equals("true") ? true : false;

                string actionType;
                if (deleted == true)
                {
                    actionType = "DELETE";
                }
                else if (seq_id > 0) 
                {
                    actionType = "EDIT";
                }
                else
                {
                    actionType = "ADD";
                }

                if (filetype.Length > 0 && fileformat.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_FILE_EMAIL_TMP WHERE 1=0";
                    base.getUpdateDS(sqlstmt);

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                    uprow["FileType"] = filetype;
                                    uprow["FileFormat"] = fileformat;
                                    uprow["Email"] = Convert.ToString(ht["Email"]);
                                    uprow["Deleted"] = deleted;
                                    uprow["Upload"] = chkUpload;
                                    uprow["Process"] = chkProcess;
                                    uprow["Branch"] = this._branch_email;
                         
                                    if (actionType != "ADD")
                                    {
                                        uprow["MAIN_SEQ_ID"] = seq_id;
                                    }
                                    uprow["Maker"] = userID;
                                    uprow["DateAdded"] = dateNow;
                                    uprow["CurrStatus"] = "For Approval";
                                    uprow["StatusDate"] = dateNow;
                                    uprow["ActionType"] = actionType;
                                    uprow["Module"] = "Branch Administration >> Upload File Maintenance";
                                    uprow["MenuName"] = "Email Setup";

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
        public bool save_file_products(Hashtable ht, string filetype, string fileformat, bool deleted)
        {
            try
            {
                string filecode = (string)ht["FileCode"];
                string productcode = (string)ht["ProductCode"];
                string implementationtype = (string)ht["ImplementationType"];
                string forretrap = (string)ht["ForRetrap"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);
                bool chkForRetrap = ht["ForRetrap"].Equals("true") ? true : false;

                if (filetype.Length > 0 && fileformat.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_FILELOADER_PRODUCTS WHERE SEQ_ID= " + seq_id + "";
                    base.getUpdateDS(sqlstmt);

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("FileCode", filecode);
                                    base.setColumn("ProductCode", productcode);
                                    base.setColumn("ImplementationType", implementationtype);
                                    base.setColumn("ForRetrap", chkForRetrap);
                                    base.setColumn("Deleted", deleted);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["FileType"] = filetype;
                                    uprow["FileFormat"] = fileformat;
                                    uprow["FileCode"] = Convert.ToString(ht["FileCode"]);
                                    uprow["ProductCode"] = Convert.ToString(ht["ProductCode"]);
                                    uprow["ImplementationType"] = Convert.ToString(ht["ImplementationType"]);
                                    uprow["ForRetrap"] = chkForRetrap;
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
                    Message = " missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool save_file_products_tmp(Hashtable ht, string filetype, string fileformat, bool deleted, string userID)
        {
            try
            {
                string filecode = (string)ht["FileCode"];
                string productcode = (string)ht["ProductCode"];
                string implementationtype = (string)ht["ImplementationType"];
                string forretrap = (string)ht["ForRetrap"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);
                bool chkForRetrap = ht["ForRetrap"].Equals("true") ? true : false;

                string actionType;
                if (deleted == true)
                {
                    actionType = "DELETE";
                }
                else if (seq_id > 0)
                {
                    actionType = "EDIT";
                }
                else
                {
                    actionType = "ADD";
                }

                if (filetype.Length > 0 && fileformat.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_FILELOADER_PRODUCTS_TMP WHERE 1=0";
                    base.getUpdateDS(sqlstmt);

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                    uprow["FileType"] = filetype;
                                    uprow["FileFormat"] = fileformat;
                                    uprow["FileCode"] = Convert.ToString(ht["FileCode"]);
                                    uprow["ProductCode"] = Convert.ToString(ht["ProductCode"]);
                                    uprow["ImplementationType"] = Convert.ToString(ht["ImplementationType"]);
                                    uprow["ForRetrap"] = chkForRetrap;
                                    uprow["Deleted"] = deleted;

                                    if (actionType != "ADD")
                                    {
                                        uprow["MAIN_SEQ_ID"] = seq_id;
                                    }
                                    uprow["Maker"] = userID;
                                    uprow["DateAdded"] = dateNow;
                                    uprow["CurrStatus"] = "For Approval";
                                    uprow["StatusDate"] = dateNow;
                                    uprow["ActionType"] = actionType;
                                    uprow["Module"] = "Branch Administration >> Upload File Maintenance";
                                    uprow["MenuName"] = "Method of Transaction";



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

        public bool save_batch_setup(Hashtable ht, string filetype, string fileformat, bool deleted)
        {
            try
            {
                Int32 amountlimit = Convert.ToInt32(ht["AmountLimit"]);
                Int32 batchlimit = Convert.ToInt32(ht["BatchLimit"]);
                Int32 batchnumlength = Convert.ToInt32(ht["BatchNumLength"]);
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (filetype.Length > 0 && fileformat.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM TBL_BATCH_SETUP WHERE SEQ_ID= " + seq_id + "";
                    base.getUpdateDS(sqlstmt);

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("AmountLimit", amountlimit);
                                    base.setColumn("BatchLimit", batchlimit);
                                    base.setColumn("BatchNumLength", batchnumlength);
                                    base.setColumn("Deleted", deleted);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["FileType"] = filetype;
                                    uprow["FileFormat"] = fileformat;
                                    uprow["AmountLimit"] = Convert.ToInt32(ht["AmountLimit"]);
                                    uprow["BatchLimit"] = Convert.ToInt32(ht["BatchLimit"]);
                                    uprow["UserGroup"] = 0;
                                    uprow["BatchFormat"] = "NULL";
                                    uprow["Batchctr"] = 0;
                                    uprow["BatchNumLength"] = Convert.ToInt32(ht["BatchNumLength"]);
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
                    Message = " missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool save_batch_setup_tmp(Hashtable ht, string filetype, string fileformat, bool deleted, string userID)
        {
            try
            {
                Int32 amountlimit = Convert.ToInt32(ht["AmountLimit"]);
                Int32 batchlimit = Convert.ToInt32(ht["BatchLimit"]);
                Int32 batchnumlength = Convert.ToInt32(ht["BatchNumLength"]);
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                string actionType;
                if (deleted == true)
                {
                    actionType = "DELETE";
                }
                else if (seq_id > 0)
                {
                    actionType = "EDIT";
                }
                else
                {
                    actionType = "ADD";
                }

                if (filetype.Length > 0 && fileformat.Length > 0)
                {
                    string sqlstmt = "SELECT * FROM tbl_batch_setup_tmp WHERE 1=0";
                    base.getUpdateDS(sqlstmt);

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                    uprow["FileType"] = filetype;
                                    uprow["FileFormat"] = fileformat;
                                    uprow["AmountLimit"] = Convert.ToInt32(ht["AmountLimit"]);
                                    uprow["BatchLimit"] = Convert.ToInt32(ht["BatchLimit"]);
                                    uprow["UserGroup"] = 0;
                                    uprow["BatchFormat"] = "NULL";
                                    uprow["Batchctr"] = 0;
                                    uprow["BatchNumLength"] = Convert.ToInt32(ht["BatchNumLength"]);
                                    uprow["Deleted"] = deleted;

                                    if (actionType != "ADD")
                                    {
                                        uprow["MAIN_SEQ_ID"] = seq_id;
                                    }
                                    uprow["Maker"] = userID;
                                    uprow["DateAdded"] = dateNow;
                                    uprow["CurrStatus"] = "For Approval";
                                    uprow["StatusDate"] = dateNow;
                                    uprow["ActionType"] = actionType;
                                    uprow["Module"] = "Branch Administration >> Batch Setup";
                                    uprow["MenuName"] = "Batch Setup";




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

        public void delete_email(int indx, string filetype, string fileformat)
        {
            base.getUpdateDS("sp_grid_filemail", new Object[] { filetype, fileformat });
            if (isUpdatable)
            {
                selectedRow = indx;
                base.setColumn("Deleted", true);
                base.save(sqlHelperWs.RETAIN_DATASET);
            }
        }

        public void delete_product(int indx, string filetype, string fileformat)
        {
            base.getUpdateDS("sp_grid_fileprod", new Object[] { filetype, fileformat });
            if (isUpdatable)
            {
                selectedRow = indx;
                base.setColumn("Deleted", true);
                base.save(sqlHelperWs.RETAIN_DATASET);
            }
        }

        public void delete_batch(int indx, string filetype, string fileformat)
        {
            base.getUpdateDS("sp_grid_batchSetup", new Object[] { filetype, fileformat });
            if (isUpdatable)
            {
                selectedRow = indx;
                base.setColumn("Deleted", true);
                base.save(sqlHelperWs.RETAIN_DATASET);
            }
        }

        public DataSet getFileFormat(int fldvalue)
        {
            SqlCommand sqcommands = null;

            try
            {
                sqcommands = getSqlCommand("sp_getFileFormat");
                sqcommands.Parameters.Add("@fldvalue", SqlDbType.Int).Value = fldvalue;
                return getDataSet(sqcommands, "tblFileFormat");
            }
            catch { }
            finally
            {
                sqcommands.Dispose();
                sqcommands = null;
            }

            return null;
        }

        public DataSet getFileType(string fldgroup)
        {
            SqlCommand sqcommands = null;

            try
            {
                sqcommands = getSqlCommand("sp_get_FldDescription");
                sqcommands.Parameters.Add("@fldgroup", SqlDbType.VarChar, 100).Value = fldgroup;

                return getDataSet(sqcommands, "tblFileType");
            }
            catch { }
            finally
            {
                sqcommands.Dispose();
                sqcommands = null;
            }
            return null;
        }

        public DataSet getBranches()
        {
            SqlCommand sqcommands = null;

            try
            {
                sqcommands = getSqlCommand("sp_GetBranches");
                //sqcommands.Parameters.Add("@fldgroup", SqlDbType.VarChar, 100).Value = fldgroup;

                return getDataSet(sqcommands, "TBLBranches");
            }
            catch { }
            finally
            {
                sqcommands.Dispose();
                sqcommands = null;
            }
            return null;
        }

        public irsTranxFileMaintenance()
        {
            this.TableName = "tbl_file_email";
            this.TableName = "tbl_fileloader_products";
            this.TableName = "tbl_batch_setup";
            this.IdentityColumn = "SEQ_ID";

            try
            {
                irsClientData client = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                this._branch = client.Location;
                this._user = client.UserId;
            }
            catch { }
        }

        private string getBranch(int seq_id)
        {
            try
            {
                return execScalar("sp_branchcode_", new object[] { seq_id }).ToString();
            }
            catch
            {
                new csAuditLog().writeAudit(_user, "TransFileMaintenance", "Fail to get branch - seq_id: " + seq_id.ToString(), "", "", csAuditLog.Category.Select);
            }
            return "";
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
    }
}
