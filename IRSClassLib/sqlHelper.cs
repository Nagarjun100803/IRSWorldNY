using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;

namespace IRSClassLib
{
    [Serializable]
    public class sqlHelper : irsSqlCommons
	{
		public const bool RETAIN_DATASET = true;
		public const bool DISPOSE_DATASET = false;

		protected DataSet upds;
		[NonSerialized] protected SqlDataAdapter upda;
        [NonSerialized] protected DataRow uprow;
        private sqlHelperCommandKeep cmdkeep;
        private Hashtable ht_uprow;

		private int row;
		private bool updating;
		private bool add_mode;

		private string _tableName = string.Empty;
		private string _identColumn = string.Empty;
		private object _identValue = null;
        private object _identCurrent = null;

		public string TableName
		{
			get { return _tableName == string.Empty ? "Table0" : _tableName; }
			set { _tableName = value; }
		}

		public string IdentityColumn
		{
			get { return _identColumn; }
            set { _identColumn = value; }
		}

        public object IdentityCurrent
        {
            get { return _identCurrent; }
            set { _identCurrent = value; }
        }

		public object IdentityValue
		{
			get { return _identValue; }
			set { _identValue = value; }
		}

		public DataSet dataset
		{
			get { return upds; }
		}

		public virtual DataTable table
		{
			get
			{
                try
                {
                    if (dataset != null) return dataset.Tables[0];
                }
                catch { }
				return null;
			}
		}

		public virtual bool setColumn(string col, object val)
		{
            if (isUpdatable)
            {
                DataRow row = (addMode) ? uprow : upds.Tables[0].Rows[selectedRow];
                return base.setColumn(row, col, val);
            }
            return false;
        }

		public object getColumn(string col)
		{
            if (isUpdatable & table.Rows.Count > 0)
            {
                return upds.Tables[0].Rows[selectedRow][col] == DBNull.Value ? null : upds.Tables[0].Rows[selectedRow][col];
            }
            else if (isUpdatable & addMode) // chr 06012010 - return uprow columns when in add mode.
            {
                return uprow[col] == DBNull.Value ? null : uprow[col];
            }
			return null;
		}

        public virtual void getUpdateDataSet(String spname)
        {
            getUpdateDataSet(getSqlCommand(spname));
        }

		public virtual void getUpdateDataSet(SqlCommand cmd)
		{
			getUpdateDataSet(cmd, new string[] { this.TableName });
		}

		public virtual void getUpdateDataSet(SqlCommand cmd, string[] tblnames)
		{
            try
            {
                upda = getDataAdapter(cmd);
                upds = getDataSet(upda, tblnames);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            finally
            {
                if (upda.SelectCommand.Connection.State > 0) upda.SelectCommand.Connection.Close();
            }
		}

		public virtual void getUpdateDataSet(SqlTransaction trans, SqlCommand cmd)
		{
			upda = getDataAdapter(trans, cmd);
			upds = getDataSet(upda, this.TableName);
		}

		public int selectedRow
		{
			get { return row; }
			set { row = value; }
		}

		public bool isUpdating
		{
			get { return updating; }
			set { updating = value; }
		}

		public bool isUpdatable
		{
			get
			{
				if (upds == null || upds.Tables.Count < 1)
				{
					Message = "Update dataset not initialized";
					return false;
				}
				return true;
			}
		}

        public bool hasRows
        {
            get
            {
                if (!isUpdatable) return false;
                return (table.Rows.Count > 0);
            }
        }

        private bool hasAdditions()
        {
            try
            {
                return (table.GetChanges(DataRowState.Added).Rows.Count > 0);
            }
            catch { }

            return false;
        }

		//public bool addMode { get { return (selectedRow == -1); } }
		public bool addMode
		{
			get { return add_mode; }
			set { add_mode = value; }
		}

		public virtual bool save()
		{
			return save(DISPOSE_DATASET);
		}

		public virtual bool save(bool retainds)
		{
            try
            {
                if (isUpdatable)
                {
                    if (addMode)
                    {
                        if (uprow == null)
                        {
                            Message = "Data Insert with null row";
                            return false;
                        }
                        upds.Tables[0].Rows.Add(uprow);
                    }

                    /*
                    if (IdentityColumn.Length > 0)
                    {
                        upda.RowUpdated += new SqlRowUpdatedEventHandler(this.upda_RowUpdated);
                    }
                     * */
                    bool has_additions = hasAdditions();

                    int ra = updateDataSet(upda, upds, this.TableName);
                    if (ra > 0)
                    {
                        if (TableName != string.Empty && IdentityColumn != string.Empty)
                        {
                            if (has_additions)
                            {
                                try
                                {
                                    IdentityValue = base.getScopeIdentity();
                                }
                                catch
                                {
                                    IdentityValue = -1L;
                                }
                                if (Convert.ToInt64(IdentityValue) == -1L)
                                {
                                    IdentityValue = base.execScalar("SELECT IDENT_CURRENT('" + TableName + "')");
                                }
                            }
                            else
                            {
                                try
                                {
                                    IdentityValue = getColumn(IdentityColumn);
                                }
                                catch { }
                            }
                            IdentityCurrent = IdentityValue;
                        }
                    }

                    cancel(retainds);
                    if (ra > -1)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
			return false;
		}

		protected virtual void upda_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
		{
			if (e.StatementType == StatementType.Insert)
			{
				if (TableName != string.Empty && IdentityColumn != string.Empty)
				{
					SqlCommand cmd = new SqlCommand("SELECT IDENT_CURRENT('" + TableName + "')", e.Command.Connection);
                    //SqlCommand cmd = new SqlCommand("SELECT @@IDENTITY", e.Command.Connection);
					IdentityValue = cmd.ExecuteScalar();
                    IdentityCurrent = IdentityValue;
				}
			}
			else if (e.StatementType == StatementType.Update)
			{
				IdentityValue = e.Row[IdentityColumn];
			}
		}

		public bool newRow(bool retainds)
		{
			if (retainds != RETAIN_DATASET)
			{
				selectedRow = -1;
				getUpdateDS(selectedRow);
			}
			if (upds != null)
			{
				uprow = upds.Tables[0].NewRow();
				isUpdating = true;
				addMode = true;
				return true;
			}
			return false;
		}

		public bool newRow()
		{
			return newRow(false);
		}

		// Added edit method without parameter so that inheritors can update current dataset
		public bool edit()
		{
			if (selectedRow > -1)
			{
				if (isUpdatable)
				{
                    addMode = false;
					isUpdating = true;
					return true;
				}
				cancel();
			}
			else
			{
				Message = "DataSet is currently in add mode";
			}
			return false;
		}

		public bool edit(int uid)
		{
            if (selectedRow > -1)
            {
                getUpdateDS(uid);
                if (isUpdatable)
                {
                    addMode = false;
                    isUpdating = true;
                    return true;
                }
                cancel();
            }
            else
            {
                Message = "There are no rows to edit at " + selectedRow;
            }
			return false;
		}

		public void cancel()
		{
			cancel(DISPOSE_DATASET);
		}

		public void cancel(bool retainds)
		{
			isUpdating = false;
			selectedRow = (addMode) ? upds.Tables[0].Rows.Count - 1 : (retainds == RETAIN_DATASET) ? (selectedRow > (table.Rows.Count - 1)) ? table.Rows.Count - 1 : selectedRow : -1;
			addMode = false;
			if (retainds == DISPOSE_DATASET)
			{
				uprow = null;
				if (upds != null) upds.Dispose();
				upds = null;
				if (upda != null) upda.Dispose();
			}
			else if (upds.HasChanges())
			{
				upds.RejectChanges();
			}
		}

        public bool Delete(bool retainds, bool autosave)
        {
            if (selectedRow > -1)
            {
                if (isUpdatable)
                {
                    if (addMode)
                    {
                        Message = "Cannot delete when in Add Mode.";
                        return false;
                    }
                    bool ok = false;
                    try
                    {
                        upds.Tables[0].Rows[selectedRow].Delete();
                        ok = true;
                    }
                    catch (Exception e)
                    {
                        Message = "Delete: " + e.Message;
                    }
                    if (ok & autosave)
                    {
                        return save(retainds);
                    }
                    return ok;
                }
            }
            return false;
        }

		public bool Delete(bool retainds)
		{
            return this.Delete(retainds, true);
		}

		public bool IsNumeric(string numStr)
		{
			try
			{
				Int64.Parse(numStr);
				return true;
			}
			catch { return false; }
		}

        public int findRow(String colname, Object arg)
        {
            if (isUpdatable)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i].RowState == DataRowState.Deleted) continue;
                    Object o = table.Rows[i][colname];
                    if (table.Columns[colname].DataType == typeof(System.Byte)
                    |   table.Columns[colname].DataType == typeof(System.Int32) 
                    |   table.Columns[colname].DataType == typeof(System.Int16)
                    |   table.Columns[colname].DataType == typeof(System.Int64))
                    {
                        long j = Convert.ToInt64(o);
                        long k = Convert.ToInt64(arg);
                        if (j == k) return i;
                    }
                    else if (table.Columns[colname].DataType == typeof(System.Boolean))
                    {
                        bool b = Convert.ToBoolean(o);
                        bool v = Convert.ToBoolean(arg);
                        if (b == v) return i;
                    }
                    else if (table.Columns[colname].DataType == typeof(System.Decimal))
                    {
                        decimal d = Convert.ToDecimal(o);
                        decimal m = Convert.ToDecimal(arg);
                        if (d == m) return i;
                    }
                    else
                    {
                        if (o.Equals(arg)) return i;
                    }
                }
            }

            return -1;
        }

        public virtual void getUpdateDS(String spname, Object[] plist)
		{
			try
			{
				SqlCommand upcmd = getSqlCommand(spname);
				deriveParameters(upcmd, plist);
				getUpdateDataSet(upcmd);
			}
			catch (Exception e) 
            {
                Message = e.Message;
            }
		}

        public virtual void getUpdateDS(String sqlstmt)
        {
            try
            {
                getUpdateDataSet(getSqlCommand(sqlstmt, false));
            }
            catch { }
        }

		public virtual void getUpdateDS(int id)
		{
			// method must always be overriden
            throw new Exception("Not overriden or use RETAIN_DATASET");
		}

		public sqlHelper()
        {
            upds = new DataSet();
            isUpdating = false;
			addMode = false;
            selectedRow = -1;

            cmdkeep = new sqlHelperCommandKeep();
        }

        public sqlHelper(string connSuffix) : base(connSuffix)
        {
            base.IsWinApp = true;
        }

        ~sqlHelper()
        {
            if (upds != null) upds.Dispose();
            if (upda != null) upda.Dispose();
            if (cmdkeep != null) cmdkeep = null;
            uprow = null;
            upds = null;
            upda = null;
            ht_uprow = null;
        }

        #region Serialization Events
        [OnSerializing]
        public void OnSerializing(StreamingContext sc)
        {
            cmdkeep.clear();
            if (upda != null)
            {
                cmdkeep.keep(upda.SelectCommand, false);
            }
            try
            {
                if (add_mode && uprow != null)
                {
                    ht_uprow = new Hashtable();
                    foreach (DataColumn col in uprow.Table.Columns)
                    {
                        if (uprow[col.ColumnName] != null)
                        {
                            ht_uprow.Add(col.ColumnName, uprow[col.ColumnName]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Message = "OnSerializing uprow: " + e.Message;
            }
        }

        [OnDeserialized]
        public void OnDeserialized(StreamingContext sc)
        {
            if (cmdkeep != null)
            {
                SqlCommand cmd = cmdkeep.create_cmd();
                if (cmd != null)
                {
                    upda = getDataAdapter(cmd);
                    if (upda.SelectCommand.Connection.State > 0) upda.SelectCommand.Connection.Close();
                }
            }

            try
            {
                if (add_mode && ht_uprow != null)
                {
                    uprow = table.NewRow();
                    if (ht_uprow.Count > 0)
                    {
                        foreach (Object colname in ht_uprow.Keys)
                        {
                            uprow[colname.ToString()] = ht_uprow[colname];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Message = "OnDeserialized uprow: " + e.Message;
            }
            ht_uprow = null;
        }

        #endregion
    }
}
