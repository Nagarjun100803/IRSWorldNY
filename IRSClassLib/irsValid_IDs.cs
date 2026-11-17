using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace IRSClassLib
{
    public class irsValid_IDs : sqlHelperWs
    {
        public irsValid_IDs()
        {
            this.TableName = "tbl_valid_id";
            this.IdentityColumn = "SEQ_ID";
        }

        public DataSet GetBranches(string location)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("get_dropdown_branches_byaccess", new object[] { location });  //commons.getDataSet(commons.getSqlCommand("get_dropdown_branches"), "tblBranches");
            return ds;
        }
        public DataTable GetValidID(string brcode)
        {
            SqlCommand cmd = getSqlCommand("sp_get_validid");
            DataSet ds = base.execStoredProcedure(cmd, new object[] { brcode });
            return ds.Tables[0];
        }

        public bool detect_duplicate(string brcode, string iddesc, string validationdll)
        {
            DataSet ds = getDataSet(getSqlCommand("SELECT iddescription FROM TBL_VALID_ID WHERE BranchCode='" + brcode + "' AND IDDescription = '" + iddesc + "' AND ValidationDLL = '" + validationdll + "' AND Deleted = 0", false));
            int i = ds.Tables[0].Rows.Count;
            if (i > 0) return true;
            return false;
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
                            if (ext.Equals(".DLL") | ext.Equals(".dll"))
                            {
                                seqno++;
                                al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "F" });
                            }
                        }
                    }
                }
            }
        }

        public bool saveUpdates()
        {
            return base.save(sqlHelper.RETAIN_DATASET);
        }


        public bool valid_id_changes(Hashtable ht, string brcode,bool deleted,bool isedit)
        {
            try
            {
                String idDesc = (String)ht["IDDescription"].ToString().Replace("'"," ");
                String validation_dll = (string)ht["ValidationDLL"];
                bool required = ht["Required"].Equals("true") ? true : false;
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (!deleted)
                {
                    if (detect_duplicate(brcode, idDesc, validation_dll) && isedit == false)
                    {
                        Message = "Duplicate Record Exist!";
                        return false;
                    }
                    else if (idDesc.Trim() == string.Empty)
                    {
                        Message = "ID Description must not be empty!";
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

                if (idDesc.Length > 0)
                {
                    //base.getUpdateDS("sp_get_validid", new Object[] { brcode });
                    base.getUpdateDS("SELECT * FROM TBL_VALID_ID WHERE SEQ_ID=" + seq_id + "");
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("IDDescription", idDesc);
                                    base.setColumn("Required", required);
                                    base.setColumn("ValidationDLL", validation_dll);
                                    base.setColumn("Deleted", deleted);
                                }
                                else if (base.newRow(sqlHelper.RETAIN_DATASET))
                                {
                                    uprow["BRANCHCODE"] = brcode;
                                    uprow["IDDescription"] = idDesc;
                                    uprow["Required"] = required;
                                    uprow["ValidationDLL"] = validation_dll;
                                    uprow["Deleted"] = deleted;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        //if seq_id !=0 then perform delete then save, since the record is existing on the table
                        //if (seq_id != 0) { selectedRow = getselectedRow(seq_id); base.Delete(sqlHelper.RETAIN_DATASET); }
                        return base.save(sqlHelper.RETAIN_DATASET);
                    }
                }
                else
                {
                    Message = "ID Description Required.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool valid_id_changes_tmp(Hashtable ht, string brcode, bool deleted, bool isedit, string userID)
        {
            this.TableName = "TBL_VALID_ID_TMP";
            this.IdentityColumn = "SEQ_ID";
            try
            {
                String idDesc = (String)ht["IDDescription"].ToString().Replace("'", " ");
                String validation_dll = (string)ht["ValidationDLL"];
                bool required = ht["Required"].Equals("true") ? true : false;
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (!deleted)
                {
                    if (detect_duplicate(brcode, idDesc, validation_dll) && isedit == false)
                    {
                        Message = "Duplicate Record Exist!";
                        return false;
                    }
                    else if (idDesc.Trim() == string.Empty)
                    {
                        Message = "ID Description must not be empty!";
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

                if (idDesc.Length > 0)
                {
                    //base.getUpdateDS("sp_get_validid", new Object[] { brcode });
                    base.getUpdateDS("SELECT * FROM TBL_VALID_ID_TMP WHERE 1=0");
                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("IDDescription", idDesc);
                                    base.setColumn("Required", required);
                                    base.setColumn("ValidationDLL", validation_dll);
                                    base.setColumn("Deleted", deleted);
                                }
                                else if (base.newRow(sqlHelper.RETAIN_DATASET))
                                {
                                    string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                                    uprow["BRANCHCODE"] = brcode;
                                    uprow["IDDescription"] = idDesc;
                                    uprow["Required"] = required;
                                    uprow["ValidationDLL"] = validation_dll;
                                    uprow["Deleted"] = deleted;
                                    uprow["MAIN_SEQ_ID"] = Convert.ToInt32(seq_id);
                                    uprow["Maker"] = userID;
                                    uprow["DateAdded"] = dateNow;
                                    uprow["CurrStatus"] = "For Approval";
                                    uprow["StatusDate"] = dateNow;
                                    if(deleted == true)
                                    {
                                        uprow["ActionType"] = "DELETE";
                                    }
                                    else if(isedit == true)
                                    {
                                        uprow["ActionType"] = "EDIT";
                                    }
                                    else
                                    {
                                        uprow["ActionType"] = "ADD";
                                    }

                                    uprow["Module"] = "Branch Administration >> Required IDs";
                                    uprow["MenuName"] = "Required IDs";
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        //if seq_id !=0 then perform delete then save, since the record is existing on the table
                        //if (seq_id != 0) { selectedRow = getselectedRow(seq_id); base.Delete(sqlHelper.RETAIN_DATASET); }
                        return base.save(sqlHelper.RETAIN_DATASET);
                    }
                }
                else
                {
                    Message = "ID Description Required.";
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

        public bool DeleteValidId(int indx,string brcode)
        {
            base.getUpdateDS("sp_get_validid", new Object[] { brcode });
            if (isUpdatable)
            {
                selectedRow = indx;
                base.setColumn("Deleted", true);
                return base.save(sqlHelper.RETAIN_DATASET);
            }
            return false;
        }





    }
}
