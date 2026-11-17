using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsBranchSetup : sqlHelperWs
    {
        public DataTable getBranches()
        {
            try
            {
                DataSet ds = getDataSet(getSqlCommand("get_dropdown_branches"));
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch { }

            return null;
        }


        public DataTable getCommSetup()
        {
            return getLookup(new Object[] { "CommSetup", DBNull.Value, -1 });
        }

        public DataTable getIDConfigTypes()
        {
            return getLookup(new Object[] { "IDConfigType", DBNull.Value, -1 });
        }

        public DataTable getbranchsetup(string brcode)
        {
            try
            {
                base.getUpdateDS("sp_getbranchsetup", new Object[] { brcode, -1 });
                if (isUpdatable)
                {
                    if (table.Rows.Count > 1)
                    {
                        Message = "There can only be one active setup for the branch";
                        //return null;
                    }
                    return base.table;
                }
            }
            catch { }

            return null;
        }

        public Object[] getInitData(out String errmsg)
        {
            errmsg = "";
            DataTable dtbr = getBranches();
            if (dtbr == null)
            {
                dtbr = new DataTable();
                errmsg += base.Message + "\n";
            }
            DataTable dtcom = getCommSetup();
            if (dtcom == null)
            {
                dtcom = new DataTable();
                errmsg += base.Message + "\n";
            }
            DataTable dtid = getIDConfigTypes();
            if (dtid == null)
            {
                dtid = new DataTable();
                errmsg += base.Message + "\n";
            }

            return new Object[] { dtbr, dtcom, dtid };
        }

        public bool delete_setup(int seqid, out String errmsg)
        {
            errmsg = "";
            if (isUpdatable)
            {
                selectedRow = 0;
                if (base.edit())
                {
                    if (setColumn("deleted", true))
                    {
                        bool ok = save();
                        if (!ok) errmsg = Message;
                        return ok;
                    }
                }
                errmsg = base.Message;
            }
            return false;
        }

        private void concat_message(ArrayList al)
        {
            Message = "";
            foreach (Object o in al)
            {
                Message += (String)o;
            }
        }

        private bool add_setup(Hashtable ht)
        {
            if (table.Rows.Count > 0)
            {
                Message = "There can only be one active setup for the branch";
                return false;
            }

            ArrayList al = new ArrayList();

            if (base.newRow(sqlHelperWs.RETAIN_DATASET))
            {
                if (setColumn("Deleted", false))
                {
                    foreach (Object key in ht.Keys)
                    {
                        String col = (string)key;
                        if (col.Equals("crud")) continue;
                        if (!setColumn(col, ht[key]))
                        {
                            al.Add(col + ": " + Message);
                        }
                    }
                }
                else
                {
                    al.Add("Set Deleted: " + Message);
                }
            }
            else
            {
                al.Add("newRow: " + Message);
            }

            if (al.Count > 0)
            {
                concat_message(al);
                return false;
            }

            if (!save(sqlHelperWs.RETAIN_DATASET))
            {
                return false;
            }

            return true;
        }

        private bool edit_setup(Hashtable ht)
        {
            if (table.Rows.Count < 1)
            {
                Message = "No setup record to edit.";
                return false;
            }

            if (table.Rows.Count > 1)
            {
                Message = "There can only be one active setup for the branch";
                return false;
            }

            ArrayList al = new ArrayList();

            selectedRow = 0;
            if (base.edit())
            {
                foreach (Object key in ht.Keys)
                {
                    String col = (string)key;
                    if (col.Equals("crud")) continue;
                    if (col.Equals("SEQ_ID")) continue;
                    if (!setColumn(col, ht[key]))
                    {
                        al.Add(col + ": " + Message);
                    }
                }
            }
            else
            {
                al.Add("edit: " + Message);
            }

            if (al.Count > 0)
            {
                concat_message(al);
                return false;
            }

            if (!save(sqlHelperWs.RETAIN_DATASET))
            {
                return false;
            }

            return true;
        }

        public DataTable save_setup(Hashtable ht, out String errmsg)
        {
            errmsg = "";

            String crud = (String) ht["crud"];
            if (crud.Equals("Add"))
            {
                if (add_setup(ht))
                {
                    return getbranchsetup(base.table.Rows[0]["BranchCode"].ToString());
                }
            }
            else if (crud.Equals("Edit"))
            {
                if (edit_setup(ht))
                {
                    return table;
                }
            }

            errmsg = Message;

            return null;
        }

        public DataTable save_setup_tmp(Hashtable ht, out String errmsg)
        {
            errmsg = "";

            String crud = (String)ht["crud"];
            if (crud.Equals("Add"))
            {
                if (add_setup(ht))
                {
                    return getbranchsetup(base.table.Rows[0]["BranchCode"].ToString());
                }
            }
            else if (crud.Equals("Edit"))
            {
                if (edit_setup(ht))
                {
                    return table;
                }
            }

            errmsg = Message;

            return null;
        }

        public override void getUpdateDS(int id)
        {
            base.getUpdateDS("sp_getbranchsetup", new Object[] { DBNull.Value, id });
        }

        public irsBranchSetup()
        {
            this.TableName = "TBL_BRANCH_SETUP";
            this.IdentityColumn = "SEQ_ID";
        }

    }
}
