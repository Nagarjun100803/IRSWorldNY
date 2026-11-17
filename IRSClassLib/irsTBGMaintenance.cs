using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsTBGMaintenance : sqlHelperWs
    {
        public DataTable GetGridValue(string recordID)
        {
            SqlCommand cmd = getSqlCommand("sp_grid_TBGmaint");
            DataSet ds = base.execStoredProcedure(cmd, new object[] {recordID});

            return ds.Tables[0];
        }

        public DataTable getCurrency(string fldgroup)
        {

            DataSet ds;
            ds = execStoredProcedure("sp_get_FldDescription", new object[] { fldgroup });
            return ds.Tables[0];
        }

        public bool applyUpdates(String crud, int row, Hashtable ht, String recordID)
        {
            int seq_id = ht["seq_id"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["seq_id"]);
            ht["IsEdited"] = 0;

            if (!base.isUpdatable)
            {
                string sqlstmt = "SELECT * FROM tbl_trust WHERE seq_id = " + seq_id;
                base.getUpdateDS(sqlstmt);
            }

            if (crud.Equals("DELETE"))
            {
                selectedRow = row;
                return base.Delete(sqlHelperWs.RETAIN_DATASET);
            }
            bool ok = false;
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("seq_id", Convert.ToInt32(ht["seq_id"]));
                ht["IsEdited"] = 1;
                ht["DateCreated_Edited"] = DateTime.Now;
                ok = base.edit();
            }
            if (crud.Equals("ADD"))
            {
                ht["record_id"] = recordID;
                ht["DateCreated_Edited"] = DateTime.Now;
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

        private DataSet resetDS(int seq_id)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                cmd = base.getSqlCommand("SELECT * FROM tbl_trust where seq_id = " + seq_id);
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch(Exception e)
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }

        public DataTable saveUpdates(int seq_id)
        {
            if (base.save(sqlHelperWs.RETAIN_DATASET))
            {
                return resetDS(seq_id).Tables[0];
            }
            return null;
        }

        public irsTBGMaintenance()
        {
            this.TableName = "tbl_trust";
            this.IdentityColumn = "seq_id";
        }

    }
}
