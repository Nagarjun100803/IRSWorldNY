using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class irsSqlCommons
    {
		private String errmsg;
        [NonSerialized] private SqlConnection rdrcon;
        private bool conn_trait;

        public const bool REMAIN_OPEN = true;
        public const bool EXEC_AND_CLOSE = false;
        public const bool STORED_PROC = true;
        public const bool SQL_STMT = false;

        internal bool isProd;
        public bool EnforceConstraint = true;
        public String constring_suffix;
        public bool constring_noRunmode = false;

        bool isWinApp = false;
        public bool IsWinApp
        {
            get { return isWinApp; }
            set { isWinApp = value; }
        }

        public DataTable getLookup(Object[] values)
        {
            try
            {
                SqlCommand cmd = getSqlCommand("sp_lookup");
                deriveParameters(cmd, values);
                DataSet ds = getDataSet(cmd);
                if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        internal DateTime ServerDate
        {
            get
            {
                try
                {
                    return Convert.ToDateTime(this.execScalar("SELECT getdate() as 'current_date'"));
                }
                catch { }

                return DateTime.Now;
            }
        }

        protected irsClientData ClientData
        {
            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                    {
                        ClientData = new irsClientData();
                    }
                    return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                }
                catch { }
                return null;
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = value;
                }
                catch { }
            }
        }

        public irsAuditLogger AuditLogger
        {
            get
            {
                try
                {
                    if (System.Web.HttpContext.Current.Session["IRS_AUDIT_LOGGER"] == null)
                    {
                        System.Web.HttpContext.Current.Session["IRS_AUDIT_LOGGER"] = new irsAuditLogger();
                    }
                    return (irsAuditLogger)System.Web.HttpContext.Current.Session["IRS_AUDIT_LOGGER"];
                }
                catch {}
                return null;
            }
        }

        public bool AutoLog
        {
            get
            {
                try
                {
                    return (AuditLogger.forcedLog) ? true : (new irsConfiguration().appsetting("AutoLog").ToUpper().Equals("ON"));
                }
                catch { }

                return false;
            }
        }

        public bool ConnectionBehavior
        {
            get { return conn_trait; }
            set { conn_trait = value; }
        }

		public String Message
		{
			get
			{
				return errmsg;
			}
			set
			{
				errmsg = value;
			}
		}

        public String ConnectionString
        {
            get
            {
                return (constring_suffix == null || constring_suffix.Length == 0) ?
                        new irsConfiguration().ConnectionString :
                    (this.constring_noRunmode) ?
                        new irsConfiguration().ConnectionStringNoRunMode(constring_suffix) :
                            new irsConfiguration(constring_suffix).ConnectionString;
            }
        }

        private bool _setup;

        internal enum DBSTATE
        {
            NOT_AVAILABLE = -1,
            NORMAL = 0,
            SETUP_STATE = 1
        }

        internal DBSTATE State(String tabstate)
        {
            try
            {
                Object o = this.execScalar("SELECT count(*) FROM " + tabstate);
                if (o == null) return DBSTATE.NOT_AVAILABLE;

                int i = Convert.ToInt32(o);
                if (i > 0) return DBSTATE.NORMAL;
                return DBSTATE.SETUP_STATE;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return DBSTATE.NOT_AVAILABLE;
        }

        public bool inSetupMode()
        {
            _setup = true;
            /* VAPT IAG 20180730 */
            bool is_setup = (State("tbl_Users WHERE role IN (SELECT fldValue FROM tbl_lookup WHERE fldgroup = 'User Roles' AND fldCode IN ('System Administrator', 'SA'))") != DBSTATE.NORMAL);
            _setup = false;
            return is_setup;
        }

        internal void redirect_to_login()
        {
            String app = System.Web.HttpContext.Current.Request.Url.ToString();
            if (app.ToUpper().IndexOf("USERADMIN") > -1 
            || app.ToUpper().IndexOf("ADMIN.LOGIN") > -1 
            || app.ToUpper().IndexOf("ADMIN.PASSWORD") > -1
            ) return;
            String s = System.Web.HttpContext.Current.Request.ApplicationPath;
            System.Web.HttpContext.Current.Response.Redirect
            (
                (s.EndsWith("/") ? s + "Admin/Login.aspx" : s + "/Admin/Login.aspx")
            );
        }

        internal void redirect_to_errpage()
        {

            String s = System.Web.HttpContext.Current.Request.ApplicationPath;
            System.Web.HttpContext.Current.Response.Redirect
            (
                (s.EndsWith("/") ? s + "ErrorPage.aspx" : s + "/ErrorPage.aspx") + "?ErrorMessage=" + Message
            ); 
        }

        public SqlConnection dbOpen(bool remain_open)
        {
            ConnectionBehavior = remain_open;
            return dbOpen();
        }

        public SqlConnection dbOpen()
        {
            if (!isWinApp)
            {
                if (this.isProd & (!this._setup && (ClientData == null || (ClientData._userid == null || ClientData._userid == String.Empty))))
                {
                    if (!ClientData._is_setup)
                        redirect_to_login();
                }
            }
            SqlConnection con = new SqlConnection(ConnectionString);
            try
            {
                con.Open();
            }
            catch (SqlException e)
            {
                this.Message = e.Message;
                con = null;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
                con = null;
            }
            return con;
        }

        public bool dbClose(SqlConnection con)
        {
            try
            {
                if (con.State > 0) con.Close();
                con.Dispose();
                conn_trait = EXEC_AND_CLOSE;
                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return false;
        }

        public bool dbClose(SqlCommand cmd)
        {
            if (dbClose(cmd.Connection))
            {
                cmd.Connection = null;
                return true;
            }
            return false;
        }

        public DataTable getSchema(String tblname)
        {
            SqlConnection con = dbOpen();
            DataTable dt = con.GetSchema();
            con.Close();
            return dt;
        }

        public SqlTransaction beginTransaction()
        {
            try
            {
                SqlConnection con = dbOpen();
                SqlTransaction trans = con.BeginTransaction();
                return trans;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return null;
        }

        public enum isolationLevel
        {
            ReadCommitted,
            ReadUncommitted,
            Serializable
        }

        public SqlTransaction beginTransaction(isolationLevel level, String tran_name)
        {
            IsolationLevel iso = IsolationLevel.Serializable;
            switch (level)
            {
                case isolationLevel.ReadCommitted: iso = IsolationLevel.ReadCommitted; break;
                case isolationLevel.ReadUncommitted: iso = IsolationLevel.ReadUncommitted; break;
                case isolationLevel.Serializable: iso = IsolationLevel.Serializable; break;
            }

            String sname = tran_name;
            if (tran_name == null || tran_name.Length == 0)
            {
                sname = "IRS" + DateTime.Now.ToString("MMddyyyyhh:mm:ss");
            }

            if (sname.Length > 32) sname = sname.Substring(0, 32);

            try
            {
                SqlConnection con = dbOpen();
                SqlTransaction trans = con.BeginTransaction(iso, sname);
                return trans;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return null;
        }

        public bool endTransaction(SqlTransaction trans, bool commit)
        {
            try
            {
                if (commit)
                    trans.Commit();
                else
                    trans.Rollback();

                if (trans.Connection != null && trans.Connection.State != ConnectionState.Closed)
                {
                    trans.Connection.Close();
                }

                int ra = this.sqlUpdate("SET TRANSACTION ISOLATION LEVEL READ COMMITTED");

                return true;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return false;
        }

        public SqlDataAdapter getDataAdapter(SqlTransaction trans, SqlCommand sqlcmd)
        {
            try
            {
                sqlcmd.Connection = trans.Connection;
                return new SqlDataAdapter(sqlcmd);
            }
            catch (SqlException e)
            {
                Message = e.Message;
            }

            return null;
        }

        public SqlDataAdapter getDataAdapter(SqlCommand sqlcmd)
        {
            try
            {
                if (sqlcmd.Connection == null || sqlcmd.Connection.State != ConnectionState.Open)
                    sqlcmd.Connection = dbOpen();
                return new SqlDataAdapter(sqlcmd);
            }
            catch (SqlException e)
            {
                Message = e.Message;
            }

            return null;
        }

        private void attach_logger(String tblname, DataTable dt, String cmdstring)
        {
            irsDataKeep dk = AuditLogger.getKeeper(tblname);
            dt.ColumnChanging += new DataColumnChangeEventHandler(dk.columnChanging);
            dt.RowChanging += new DataRowChangeEventHandler(dk.rowChanging);
            dt.RowChanged += new DataRowChangeEventHandler(dk.rowChanged);
            dt.RowDeleting += new DataRowChangeEventHandler(dk.rowChanged);
            dt.Disposed += new EventHandler(dk.disposed);
            dk.logQuery(cmdstring);
        }

        private DataSet fillDataSet(SqlDataAdapter sqlda, String[] tblnames)
        {
            DataSet ds = new DataSet();
            try
            {
                if (tblnames.Length > 0)
                {
                    ds.EnforceConstraints = this.EnforceConstraint;
                    if (ds.EnforceConstraints)
                    {
                        sqlda.FillSchema(ds, SchemaType.Source);
                    }
                }
                sqlda.Fill(ds);
                if (tblnames.Length > 0 && (ds != null && ds.Tables.Count > 0))
                {
                    for (int i = 0; i < tblnames.Length; i++)
                    {
                        ds.Tables[i].TableName = tblnames[i];

                        sqlda.TableMappings.Add(tblnames[i], ds.Tables[i].TableName);
                        if (!IsWinApp && AutoLog)
                        {
                            attach_logger(tblnames[i], ds.Tables[i], sqlda.SelectCommand.CommandText);
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Message = e.Message;
                ds = null;
            }
            finally
            {
                if (conn_trait != REMAIN_OPEN & sqlda.SelectCommand.Connection.State > 0) sqlda.SelectCommand.Connection.Close();
            }
            return ds;
        }

        private DataSet fillDataSet(SqlDataAdapter sqlda, String tblname)
        {
            return getDataSet(sqlda, new String[] { tblname });
        }

        public DataSet getDataSet(SqlDataAdapter sqlda, String tblname)
        {
            return fillDataSet(sqlda, tblname);
        }

        public DataSet getDataSet(SqlDataAdapter sqlda, String[] tblnames)
        {
            return fillDataSet(sqlda, tblnames);
        }

        public DataSet getDataSet(SqlCommand sqlcmd, String tblname)
        {
            SqlDataAdapter sqlda = getDataAdapter(sqlcmd);
            if (sqlda == null) return null;
            return fillDataSet(sqlda, tblname);
        }

        public DataSet getDataSet(SqlCommand sqlcmd, String[] tblnames)
        {
            SqlDataAdapter sqlda = getDataAdapter(sqlcmd);
            if (sqlda == null) return null;
            return fillDataSet(sqlda, tblnames);
        }

        public DataSet getDataSet(SqlDataAdapter sqlda)
        {
            return fillDataSet(sqlda, new String[] { });
        }

        public DataSet getDataSet(SqlCommand sqlcmd)
        {
            SqlDataAdapter sqlda = getDataAdapter(sqlcmd);
            if (sqlda == null) return null;
            return fillDataSet(sqlda, new String[] { });
        }

        private long last_scope_identity = -1;

        public long getScopeIdentity()
        {
            return last_scope_identity;
        }

        private void sqlda_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
        {
            try
            {
                SqlCommand sqlcmd = new SqlCommand("SELECT SCOPE_IDENTITY()", e.Command.Connection);
                Object o = sqlcmd.ExecuteScalar();
                if (o != null && o != DBNull.Value)
                {
                    last_scope_identity = Convert.ToInt64(o);
                }
                else
                {
                    last_scope_identity = -1;
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
        }

        public int updateDataSet(SqlTransaction trans, SqlDataAdapter sqlda, DataSet ds, String tblname)
        {
            try
            {
                SqlCommandBuilder b = new SqlCommandBuilder(sqlda);
                sqlda.SelectCommand.Connection = (trans == null) ? dbOpen() : trans.Connection;
                if (sqlda.SelectCommand.Connection == null) return -1;
                b.RefreshSchema();
                int ra = -1;
                sqlda.RowUpdated += new SqlRowUpdatedEventHandler(sqlda_RowUpdated);
                if (tblname == null)
                {
                    ra = sqlda.Update(ds);
                }
                else
                {
                    ra = sqlda.Update(ds, tblname);
                }
                if (trans == null)
                    sqlda.SelectCommand.Connection.Close();
                return ra;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return -1;
        }

        public int updateDataSet(SqlTransaction trans, SqlDataAdapter sqlda, DataSet ds)
        {
            return updateDataSet(trans, sqlda, ds, null);
        }

        public int updateDataSet(SqlDataAdapter sqlda, DataSet ds)
        {
            return updateDataSet(null, sqlda, ds, null);
        }

        public int updateDataSet(SqlDataAdapter sqlda, DataSet ds, string tblname)
        {
            return updateDataSet(null, sqlda, ds, tblname);
        }

        public SqlCommand getSqlCommand(String spname)
        {
            return getSqlCommand(spname, true);
        }

        public SqlCommand getSqlCommand(String sqlstring, bool storedproc)
        {
            SqlCommand sqlcmd = new SqlCommand(sqlstring);
            sqlcmd.CommandType = (storedproc) ? CommandType.StoredProcedure : CommandType.Text;
            sqlcmd.CommandTimeout = 0;
            return sqlcmd;
        }

        public bool setColumn(DataRow uprow, String col, Object val)
        {
            Object value = DBNull.Value;
            if (val != null && val != DBNull.Value)
            {
                try
                {
                    DataColumn dc = (DataColumn)uprow.Table.Columns[col];
                    Type type = dc.DataType;
                    switch (type.ToString())
                    {
                        case "System.Guid":
                            String sguid = Convert.ToString(val);
                            value = (sguid.Length == 0) ? DBNull.Value : val;
                            break;
                        case "System.String":
                            int len = dc.MaxLength;
                            string s = Convert.ToString(val);
                            if (len > 0 & s.Length > len)
                            {
                                s = s.Substring(0, len - 1);
                            }
                            value = (object)s;
                            break;
                        case "System.Boolean":
                            //value = (object)Convert.ToBoolean(val);
                            if (val.ToString().Length > 0) value = (object)Convert.ToBoolean(val);
                            break;
                        case "System.Int16":
                            if (val.ToString().Length > 0) value = (object)Convert.ToInt16(val);
                            break;
                        case "System.Int32":
                            //value = (object)Convert.ToInt32(val);
                            if (val.ToString().Length > 0) value = (object)Convert.ToInt32(val);
                            break;
                        case "System.Int64":
                            //value = (object)Convert.ToInt64(val);
                            if (val.ToString().Length > 0) value = (object)Convert.ToInt64(val);
                            break;
                        case "System.Decimal":
                            //value = (object)Convert.ToDecimal(val);
                            if (val.ToString().Length > 0) value = (object)Convert.ToDecimal(val);
                            break;
                        case "System.Double":
                            //value = (object)Convert.ToDouble(val);
                            if (val.ToString().Length > 0) value = (object)Convert.ToDouble(val);
                            break;
                        case "System.Single":
                            //value = (object)Convert.ToSingle(val);
                            if (val.ToString().Length > 0) value = (object)Convert.ToSingle(val);
                            break;
                        case "System.DateTime":
                            //value = (object)Convert.ToDateTime(val);
                            if (val.ToString().Length > 0) value = (object)Convert.ToDateTime(val);
                            break;
                        case "System.Byte": // don't know how
                            value = (object)Convert.ToByte(val);
                            break;
                        default:
                            Message = "No Type Specified for column " + col;
                            return false;
                    }
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    return false;
                }
            }

            try
            {
                uprow[col] = value;
            }
            catch (Exception ex2)
            {
                Message = ex2.Message;
                return false;
            }

            return true;
        }

        public Object convertType(DbType type, String value)
        {
            try
            {
                if (type.Equals(DbType.Int64)) return Convert.ToInt64(value);
                if (type.Equals(DbType.Boolean)) return Convert.ToBoolean(value);
                if (type.Equals(DbType.DateTime) | type.Equals(DbType.Date)) return Convert.ToDateTime(value);
                if (type.Equals(DbType.Decimal)) return Convert.ToDecimal(value);
                if (type.Equals(DbType.Double)) return Convert.ToDouble(value);
                if (type.Equals(DbType.Int32) | type.Equals(DbType.Int16)) return Convert.ToInt32(value);
                if (type.Equals(DbType.Single)) return Convert.ToSingle(value);
                if (type.Equals(DbType.Byte) | type.Equals(DbType.SByte)) return Convert.ToByte(value);
                //if (type.Equals(DbType.Guid)) return System.Data.SqlTypes.SqlGuid.Parse(value.Trim());

                return value;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return null;
        }

        public void deriveParameters(SqlCommand sqlcmd, Object[] values)
        {
            try
            {
                if (sqlcmd.Connection == null || sqlcmd.Connection.State != ConnectionState.Open)
                    sqlcmd.Connection = dbOpen();

                SqlCommandBuilder.DeriveParameters(sqlcmd);
                if (values.Length > 0)
                {
                    int i = 0;
                    foreach (SqlParameter p in sqlcmd.Parameters)
                    {
                        if (p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput)
                        {
                            p.Value = (values[i].GetType() == typeof(System.String)) ? convertType(p.DbType, values[i].ToString().Trim()) : values[i];
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void deriveParameters(SqlCommand sqlcmd, DataRow row, bool usecolnames)
        {
            if (sqlcmd.Connection == null || sqlcmd.Connection.State != ConnectionState.Open)
                sqlcmd.Connection = dbOpen();

            SqlCommandBuilder.DeriveParameters(sqlcmd);

            int i = 0;
            foreach (SqlParameter p in sqlcmd.Parameters)
            {
                if (p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput)
                {
                    if (usecolnames)
                    {
                        p.Value = row[p.ParameterName.Substring(1)]; // @ being at 0
                    }
                    else
                    {
                        p.Value = row[i];
                    }
                    i++;
                }
            }
        }

        public void deriveParameters(SqlCommand sqlcmd, DataRow row)
        {
            deriveParameters(sqlcmd, row, false);
        }

        public DataTable execStoredProcedureTable(string spname, object[] values)
        {
            DataSet ds = execStoredProcedure(spname, values);
            return (ds != null) ? ds.Tables[0] : null;
        }

        public DataSet execStoredProcedure(SqlCommand sqlcmd, Object[] values)
        {
            Message = String.Empty;
            try
            {
                SqlDataAdapter sqlda = getDataAdapter(sqlcmd);
                deriveParameters(sqlcmd, values);
                return getDataSet(sqlda, new String[] { });
            }
            catch (SqlException e)
            {
                Message = e.Message;
                return null;
            }
        }

        public DataSet execStoredProcedure(String spname, Object[] values)
        {
            return execStoredProcedure(spname, values, new String[] { });
        }

        public DataSet execStoredProcedure(String spname, Object[] values, String[] tblnames)
        {
            Message = String.Empty;
            try
            {
                SqlCommand sqlcmd = getSqlCommand(spname);
                SqlDataAdapter sqlda = getDataAdapter(sqlcmd);
                deriveParameters(sqlcmd, values);
                return getDataSet(sqlda, tblnames);
            }
            catch (SqlException e)
            {
                Message = e.Message;
                return null;
            }
        }

        public int execStoredProcedureUpdate(String spname, Object[] values)
        {
            Int32 ra = -1;

            SqlCommand sqlcmd = getSqlCommand(spname);
            try
            {
                sqlcmd.Connection = dbOpen();
                deriveParameters(sqlcmd, values);
                ra = sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Message = e.Message;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            finally
            {
                if (conn_trait != REMAIN_OPEN & sqlcmd.Connection != null && sqlcmd.Connection.State > 0)
                {
                    sqlcmd.Connection.Close();
                    SqlConnection con = sqlcmd.Connection;
                    con.Close();
                    con.Dispose();
                }
            }
            return (int)ra;
        }

        public int execStoredProcedureUpdate(String spname, DataRow row)
        {
            Int32 ra = -1;
            Message = String.Empty;

            SqlCommand sqlcmd = getSqlCommand(spname);
            try
            {
                sqlcmd.Connection = dbOpen();
                deriveParameters(sqlcmd, row);
                ra = sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Message = e.Message;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
            finally
            {
                if (conn_trait != REMAIN_OPEN & sqlcmd.Connection.State > 0) sqlcmd.Connection.Close();
            }
            return (int)ra;
        }

        public bool readerConnectionIsOpen
        {
            get
            {
                return (rdrcon.State > 0);
            }
        }

        public int sqlUpdate(SqlCommand sqlcmd)
        {
            Int32 ra = 0;
            Message = String.Empty;

            try
            {
                if (sqlcmd.Connection == null || sqlcmd.Connection.State != ConnectionState.Open)
                    sqlcmd.Connection = dbOpen();
                ra = sqlcmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                Message = e.Message;
                ra = -1;
            }
            finally
            {
                if (conn_trait != REMAIN_OPEN & sqlcmd.Connection.State > 0) sqlcmd.Connection.Close();
            }
            return (int)ra;
        }

        public int sqlUpdate(String sqlstring)
        {
            return sqlUpdate(getSqlCommand(sqlstring, false));
        }

        public SqlDataReader getDataReader(SqlCommand sqlcmd)
        {
            if (rdrcon != null && readerConnectionIsOpen)
            {
                Message = "A connection from previous SqlReader is still Open";
                return null;
            }
            try
            {
                sqlcmd.Connection = dbOpen();
                rdrcon = sqlcmd.Connection;
                return sqlcmd.ExecuteReader();
            }
            catch (SqlException e)
            {
                Message = e.Message;
                sqlcmd.Connection.Close();
            }

            return null;
        }

        public SqlDataReader getDataReader(string sqlstring)
        {
            return getDataReader(getSqlCommand(sqlstring, false));
        }

        public void closeSqlReader()
        {
            if (readerConnectionIsOpen) rdrcon.Close();
        }

		public bool ValidFC(string acctno)
		{
			int[] intCheckDigit = new int[] { 4, 3, 2, 1, 2, 3, 4, 5, 6, 7, 8 };
			int[] intAccountNumberMultiplyCheckDigit = new int[11];
			int[] intAccountNumber = new int[12];
			double dblCompare = 0;
			double dblTotal = 0;

			// Get Account Number (Per Digit)
			for (int i = 0; i <= acctno.Length - 1; i++)
			{
				intAccountNumber[i] = Convert.ToInt32(acctno.Substring(i, 1));
			}

			// Multiply CheckDigit Against AccountNumber (Per Digit)
			for (int i = 0; i <= (intAccountNumber.Length - 1); i++)
			{
				// Last Digit to compare
				if (i == 11)
				{
					dblCompare = intAccountNumber[i];
					break;
				}
				else
				{
					intAccountNumberMultiplyCheckDigit[i] = (intAccountNumber[i] * intCheckDigit[i]);
				}
			}

			// Add All The Digits
			for (int i = 0; i <= (intAccountNumberMultiplyCheckDigit.Length - 1); i++)
			{
				dblTotal += Convert.ToDouble(intAccountNumberMultiplyCheckDigit[i]);
			}

			// Get the Remainder of 10 and should be equal to the Last Digit of the Account Number
			dblTotal %= 10;
			if (dblCompare == dblTotal)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        public Object execScalar(String spname, Object[] values)
        {
            Object o = null;
            SqlCommand sqlcmd = getSqlCommand(spname);
            sqlcmd.Connection = dbOpen();
            deriveParameters(sqlcmd, values);
            try
            {
                o = sqlcmd.ExecuteScalar();
            }
            catch (SqlException e)
            {
                Message = e.Message;
            }
            finally
            {
                if (conn_trait != REMAIN_OPEN & sqlcmd.Connection.State > 0) sqlcmd.Connection.Close();
            }
            return o;
        }

        public Object execScalar(String sqlstmt)
        {
            Object o = null;
            try
            {
                SqlCommand sqlcmd = getSqlCommand(sqlstmt, false);
                sqlcmd.Connection = dbOpen();
                try
                {
                    o = sqlcmd.ExecuteScalar();
                }
                catch (SqlException e)
                {
                    Message = e.Message;
                    o = null;
                }
                finally
                {
                    if (conn_trait != REMAIN_OPEN & sqlcmd.Connection.State > 0) sqlcmd.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                o = null;
            }
            return o;
        }

        /* Xml Utility method  CHR 01/14/2010 */
        public DataSet xmlToDataset(String xmlfile)
        {
            try
            {
                DataSet dsXml = new DataSet();
                dsXml.ReadXml(xmlfile);
                return dsXml;
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public irsSqlCommons()
        {
            this.isWinApp = false;
            try { this.isProd = (!new irsConfiguration().RunMode.Equals("Devel")); }
            catch { }            
            conn_trait = EXEC_AND_CLOSE;
        }

        public irsSqlCommons(String constring_suffix)
        {
            this.constring_suffix = constring_suffix;

            this.isWinApp = false;
            try { this.isProd = (!new irsConfiguration().RunMode.Equals("Devel")); }
            catch { }
            conn_trait = EXEC_AND_CLOSE;
        }

        public irsSqlCommons(bool isWinApp)
        {
            this.isWinApp = isWinApp;
            try { this.isProd = (!new irsConfiguration().RunMode.Equals("Devel")); }
            catch { }
            conn_trait = EXEC_AND_CLOSE;
        }

        ~irsSqlCommons()
        {
            if (rdrcon != null)
            {
                rdrcon.Dispose();
            }
            errmsg = null;
        }

    }
}
