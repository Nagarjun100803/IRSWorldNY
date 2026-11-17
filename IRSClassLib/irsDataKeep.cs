using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace IRSClassLib
{
    [Serializable]
    public class irsDataKeep : irsAuditLogger
    {
        private String tblname;
        private Queue<Object[]> keep;
        private String current_crud;
        private int sid;
        private String clid;

        private irsAuditLogger parent;

        public void disposed(Object sender, EventArgs e)
        {
            parent.disposeKeeper(tblname);
        }

        private String getCRUDType(DataRowAction a)
        {
            String crud_type = null;

            if (a == DataRowAction.Add)
            {
                crud_type = CRUD_TYPES[(int)CRUDS.INSERT];
            }
            else if (a == DataRowAction.Delete)
            {
                crud_type = CRUD_TYPES[(int)CRUDS.DELETE];
            }
            else if (a == DataRowAction.Change)
            {
                crud_type = CRUD_TYPES[(int)CRUDS.UPDATE];
            }

            return crud_type;
        }

        public void columnChanging(object sender, DataColumnChangeEventArgs e)
        {
            try
            {
                if (e.Row[e.Column.ColumnName].Equals(e.ProposedValue)) return; // Did not really change
                if (keep == null) keep = new Queue<Object[]>();

                Object[] values =
			    {
				    sid,
				    clid,
				    DateTime.Now,
				    tblname,
				    e.Column.ColumnName,
				    current_crud,
				    e.Row[e.Column.ColumnName].ToString(),
				    e.ProposedValue.ToString(),
				    DBNull.Value
			    };

                keep.Enqueue(values);
            }
            catch (Exception ex)
            {
                Message = "DataKeep: " + ex.Message;
            }
        }

        private void log_deleted(DataRow row)
        {
            if (keep == null) keep = new Queue<Object[]>();

            try
            {
                String key_col = row.Table.PrimaryKey[0].ToString();
                Object key_value = row[key_col];
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    Object[] values =
				    {
					    sid,
					    clid,
					    DateTime.Now,
					    tblname,
					    row.Table.Columns[i].ColumnName,
					    current_crud,
					    row[i].ToString(),
					    DBNull.Value,
					    key_value.ToString()
				    };
                    keep.Enqueue(values);
                }
            }
            catch (Exception e)
            {
                Message = "DataKeep: " + e.Message;
            }
        }

        public void rowChanging(Object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Commit) return;
            
            current_crud = getCRUDType(e.Action);
        }

        private Queue<Object[]> archiveKeep()
        {
            Queue<Object[]> old_keep = new Queue<Object[]>();
            int j = keep.Count;
            for (int i = 0; i < j; i++)
            {
                old_keep.Enqueue(keep.Dequeue());
            }
            return old_keep;
        }

        public void flushKeep(Queue<Object[]> old_keep, String old_crud, Object key_value)
        {
            int j = old_keep.Count;
            for (int i = 0; i < j; i++)
            {
                Object[] logrec = old_keep.Dequeue();
                logrec[5] = old_crud;
                if (key_value != null)
                    logrec[8] = key_value.ToString();
                writeLog(logrec);
            }
        }

        public void rowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Delete)
            {
                current_crud = getCRUDType(e.Action);
                log_deleted(e.Row);
                return;
            }
            if (e.Action == DataRowAction.Commit & keep != null)
            {
                if (keep.Count > 0)
                {
                    try
                    {
                        Object key_value = null;
                        if (!current_crud.Equals(CRUD_TYPES[(int)CRUDS.DELETE]))
                        {
                            String key_col = e.Row.Table.PrimaryKey[0].ToString();
                            key_value = e.Row[key_col];
                        }
                        if (current_crud.Equals(CRUD_TYPES[(int)CRUDS.INSERT]) & e.Row.Table.PrimaryKey[0].AutoIncrement)
                        {
                            key_value = execScalar("SELECT IDENT_CURRENT('" + tblname + "')");
                        }
                        flushKeep(archiveKeep(), current_crud, key_value);
                    }
                    catch (Exception ex)
                    {
                        Message = ex.Message;
                    }
                }
            }
        }

        /*
        public void rowUpdated(Object sender, SqlRowUpdatedEventArgs e)
        {
            if (e.StatementType == StatementType.Insert & identity)
            {
                SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT('" + tblname + "')", e.Command.Connection);
                key_value = cmd.ExecuteScalar();
            }
            flushKeep(archiveKeep(), current_crud, key_value.ToString());
        }
        */

        public void logQuery(String cmdstring)
        {
            String xcmdstring = (cmdstring == null) ? String.Empty : cmdstring;
            String ov = (xcmdstring.Length > 255) ? xcmdstring.Substring(0, 255) : xcmdstring;
            String nv = (xcmdstring.Length > 255) ? ((xcmdstring.Length - 255) > 255) ? xcmdstring.Substring(255, 255) : xcmdstring.Substring(255) : null;
            
            if (keep == null) keep = new Queue<Object[]>();

            Object[] values =
		    {
			    sid,
			    clid,
			    DateTime.Now,
			    tblname,
			    DBNull.Value, //ColumnName,
			    CRUD_TYPES[(int)CRUDS.QUERY],
			    ov,
			    (nv == null) ? (Object)DBNull.Value : (object)nv,
			    DBNull.Value
		    };
            keep.Enqueue(values);
        }

        internal irsDataKeep(String tblname, irsAuditLogger parent)
        {
            this.tblname = tblname;
            this.parent = parent;

            this.sid = parent.SessionId;
            this.clid = parent.UserId;
        }

        ~irsDataKeep()
        {
            keep = null;
            parent = null;
            tblname = null;
            clid = null;
        }
    }

}
