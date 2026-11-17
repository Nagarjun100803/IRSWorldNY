using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Runtime.Serialization;

using IRSClassLib.localhost;

namespace IRSClassLib
{
    [Serializable]
    public class irsSqlCommonsWs
    {
        private System.Net.CookieContainer cokes;
        [NonSerialized] public wsSqlCommons coms;

		private String errmsg;

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

        public Object[] replace_dbnull(Object[] inar)
        {
            Object[] outar = new Object[inar.Length];
            for (int i = 0; i < inar.Length; i++)
            {
                outar[i] = (inar[i] == System.DBNull.Value) ? null : inar[i];
            }

            return outar;
        }


        public DataTable getLookup(Object[] values)
        {
            Message = String.Empty;
            try
            {
                DataSet ds = coms.getDataSet(ConnectionString, "sp_lookup", true, replace_dbnull(values), new String[] { }, this.EnforceConstraint);
                if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];

                Message = coms.getErrorMessage();
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
                    if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                    {
                        return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                    }
                }
                catch (Exception e) 
                {
                    Message = e.Message;
                }
                return null;
            }
            set
            {
                try
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = value;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
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
            Message = String.Empty;
            try
            {
                Object o = coms.execScalar(ConnectionString, "SELECT count(*) FROM " + tabstate);

                Message = coms.getErrorMessage();

                if (o == null || o == System.DBNull.Value) return DBSTATE.NOT_AVAILABLE;

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

        private bool is_loggin()
        {
            return (System.Web.HttpContext.Current.Session["USER_ON_LOGIN"] != null);
        }

        internal void redirect_to_login()
        {
            if (is_loggin()) return;

            /*
            String s = System.Web.HttpContext.Current.Request.ApplicationPath;
            System.Web.HttpContext.Current.Response.Redirect
            (
                (s.EndsWith("/") ? s + "Admin/Login.aspx" : s + "/Admin/Login.aspx")
            );
             * */

            throw new Exception("Login is required.");
        }

        internal void redirect_to_errpage()
        {

            String s = System.Web.HttpContext.Current.Request.ApplicationPath;
            System.Web.HttpContext.Current.Response.Redirect
            (
                (s.EndsWith("/") ? s + "ErrorPage.aspx" : s + "/ErrorPage.aspx") + "?ErrorMessage=" + Message
            ); 
        }

        public bool isAuthenticated()
        {
            if (isWinApp) return true;
            if (this._setup) return true;
            if (ClientData == null) return false;
            if (ClientData._userid == null || ClientData._userid == String.Empty)
            {
                if (ClientData._is_setup) return true;
                if (is_loggin()) return true;
                else
                    return false;
            }
            else
            {
                if (is_loggin()) return true;
            }

            bool no_multi_login = false;

            try
            {
                no_multi_login = Convert.ToBoolean(new irsConfiguration().appsetting("NoMultiLogin"));
            }
            catch { }

            if (no_multi_login)
            {
                try
                {
                    if (!String.IsNullOrEmpty(this.constring_suffix)) return true;

                    try
                    {
                        if (System.Web.HttpContext.Current.Session["DB_AUTHENTICATED"] != null) return true;
                    }
                    catch { }

                    Object o = coms.execScalar(ConnectionString, "SELECT CurrentSessionID FROM tbl_Users WHERE UserId = '" + ClientData._userid + "'");
                    if (o != null || o != System.DBNull.Value)
                    {
                        if (o.ToString().IndexOf("DBNull") > -1) return true;

                        if (!o.ToString().Equals(System.Web.HttpContext.Current.Session.SessionID))
                        {
                            try
                            {
                                System.Web.HttpContext.Current.Session.RemoveAll();
                                Message = "The SessionID is invalid.";
                                throw new Exception(Message);
                            }
                            catch { }
                        }
                        else
                        {
                            try
                            {
                                System.Web.HttpContext.Current.Session["DB_AUTHENTICATED"] = true;
                            }
                            catch { }
                        }
                    }
                }
                catch { }
            }

            return true;
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

        private void attach_logger(DataSet ds, String[] tblnames, String cmdstring)
        {
            if (AutoLog)
            {
                for (int i = 0; i < tblnames.Length; i++)
                {
                    attach_logger(tblnames[i], ds.Tables[i], cmdstring);
                }
            }
        }

        private DataSet fillDataSet(Object[] cmd_keep_object, String[] tblnames)
        {
            if (!isAuthenticated()) 
            {
                redirect_to_login();
                Message = "Authentication required.";
            }
            else
            {
                try
                {
                    DataSet ds = coms.getDataSet(ConnectionString, cmd_keep_object, tblnames, this.EnforceConstraint);
                    if (ds == null)
                    {
                        Message = coms.getErrorMessage();
                    }
                    if (ds != null && ds.Tables.Count > 0 && tblnames.Length > 0)
                    {
                        attach_logger(ds, tblnames, cmd_keep_object[0].ToString());
                    }

                    return ds;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }

            return null;
        }

        private DataSet fillDataSet(String sqlstring, bool sp, Object[] values, String[] tblnames)
        {
            Message = String.Empty;

            if (!isAuthenticated()) 
            {
                redirect_to_login();
            }
            else
            {
                try
                {
                    DataSet ds = coms.getDataSet(ConnectionString, sqlstring, sp, replace_dbnull(values), tblnames, this.EnforceConstraint);
                    if (ds == null)
                    {
                        Message = coms.getErrorMessage();
                    }
                    if (ds != null && ds.Tables.Count > 0 && tblnames.Length > 0)
                    {
                        attach_logger(ds, tblnames, sqlstring);
                    }

                    return ds;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }

            return null;
        }

        public DataSet getDataSet(sqlHelperCommandKeep cmdkeep, String[] tblnames)
        {
            return fillDataSet(cmdkeep.toKeepObject(), tblnames);
        }

        public DataSet getDataSet(sqlHelperCommandKeep cmdkeep, String tblname)
        {
            return fillDataSet(cmdkeep.toKeepObject(), new String[] { tblname });
        }

        public DataSet getDataSet(sqlHelperCommandKeep cmdkeep)
        {
            return fillDataSet(cmdkeep.toKeepObject(), new String[] { });
        }

        public DataSet getDataSet(SqlCommand sqlcmd, String[] tblnames)
        {
            sqlHelperCommandKeep cmdkeep = new sqlHelperCommandKeep(true);
            cmdkeep.keep(sqlcmd, false);
            return getDataSet(cmdkeep, tblnames);
        }
        
        public DataSet getDataSet(SqlCommand sqlcmd, String tblname)
        {
            return getDataSet(sqlcmd, new String[] { tblname });
        }

        public DataSet getDataSet(SqlCommand sqlcmd)
        {
            return getDataSet (sqlcmd, new String[]{});
        }

        public DataSet getDataSet(String sqlstring, bool sp, Object[] values, String[] tblnames)
        {
            return fillDataSet(sqlstring, sp, values, tblnames);
        }

        public DataSet getDataSet(String sqlstring, bool sp, Object[] values)
        {
            return fillDataSet(sqlstring, sp, values, new String[] { });
        }

        public SqlCommand getLastCommand(String cmdtext)
        {
            Object[] kobj = coms.getReturnCommand(cmdtext);
            if (kobj == null) return null;

            sqlHelperCommandKeep cmdkeep = new sqlHelperCommandKeep();
            cmdkeep.setKeepObject(kobj);
            return cmdkeep.create_cmd();
        }

        public long getScopeIdentity()
        {
            return coms.getLastScopeIndentity();
        }

        public int updateDataSet(Object[] cmd_keep_object, DataSet ds, String tblname)
        {
            if (!isAuthenticated())
            {
                redirect_to_login();
                return -1;
            }

            int ra = -1;

            try
            {
                ra = coms.updateDataSet(ConnectionString, cmd_keep_object, ds, tblname);
                if (ra > 0)
                {
                    ds.AcceptChanges();
                }
                if (ra < 0)
                {
                    Message = coms.getErrorMessage();
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return ra;
        }

        public int updateDataSet(sqlHelperCommandKeep cmdkeep, DataSet ds, String tblname)
        {
            return updateDataSet(cmdkeep.toKeepObject(), ds, tblname);
        }

        public int updateDataSet(sqlHelperCommandKeep cmdkeep, DataSet ds)
        {
            return updateDataSet(cmdkeep.toKeepObject(), ds, null);
        }

        public int updateDataSet(SqlCommand sqlcmd, DataSet ds, String tblname)
        {
            sqlHelperCommandKeep cmdkeep = new sqlHelperCommandKeep();
            cmdkeep.keep(sqlcmd, false);
            return updateDataSet(cmdkeep, ds, tblname);
        }

        public int updateDataSet(SqlCommand sqlcmd, DataSet ds)
        {
            return updateDataSet(sqlcmd, ds, null);
        }

        public int updateDataSet(String sqlstring, bool sp, Object[] values, DataSet ds, String tblname)
        {
            if (!isAuthenticated())
            {
                redirect_to_login();
                return -1;
            }

            int ra = -1;

            try
            {
                ra = coms.updateDataSet(ConnectionString, sqlstring, sp, replace_dbnull(values), ds, tblname);
                if (ra < 1)
                {
                    Message = coms.getErrorMessage();
                }
                else
                {
                    ds.AcceptChanges();
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return ra;
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
            Object value = System.DBNull.Value;
            if (val != null && val != System.DBNull.Value)
            {
                try
                {
                    DataColumn dc = (DataColumn)uprow.Table.Columns[col];
                    Type type = dc.DataType;
                    switch (type.ToString())
                    {
                        case "System.Guid":
                            String sguid = Convert.ToString(val);
                            value = (sguid.Length == 0) ? System.DBNull.Value : val;
                            break;
                        case "System.String":
                            int len = dc.MaxLength;
                            string s = Convert.ToString(val);
                            if (len > 0 & s.Length > len)
                            {
                                s = s.Substring(0, len);
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

        public SqlCommand deriveParameters(String spname)
        {
            Object[] keep_object = coms.deriveParameter(ConnectionString, spname);
            if (keep_object != null)
            {
                sqlHelperCommandKeep cmdkeep = new sqlHelperCommandKeep();
                cmdkeep.setKeepObject(keep_object);
                SqlCommand sqlcmd = cmdkeep.create_cmd();

                return sqlcmd;
            }

            Message = coms.getErrorMessage();
            return null;
        }


        public void deriveParameters(SqlCommand sqlcmd, Object[] values)
        {
            sqlHelperCommandKeep cmdkeep = new sqlHelperCommandKeep();
            cmdkeep.keep(sqlcmd, false);

            Object[] keep_object = coms.deriveParameter(ConnectionString, cmdkeep.toKeepObject(), replace_dbnull(values));
            if (keep_object != null)
            {
                cmdkeep = new sqlHelperCommandKeep(true);
                cmdkeep.setKeepObject(keep_object);
                SqlCommand temp = cmdkeep.create_cmd();
                foreach (SqlParameter p in temp.Parameters)
                {
                    SqlParameter q = new SqlParameter(p.ParameterName, p.SqlDbType, p.Size, p.Direction, p.IsNullable, p.Precision, p.Scale, p.SourceColumn, p.SourceVersion, p.Value);
                    sqlcmd.Parameters.Add(q);
                }
                return;
            }

            Message = coms.getErrorMessage();
        }

        public DataTable execStoredProcedureTable(string spname, object[] values)
        {
            Message = String.Empty;
            try
            {
                DataSet ds = getDataSet(spname, true, values, new String[] { });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public DataSet execStoredProcedure(SqlCommand sqlcmd)
        {
            Message = String.Empty;

            try
            {
                return getDataSet(sqlcmd);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public DataSet execStoredProcedure(SqlCommand sqlcmd, Object[] values)
        {
            Message = String.Empty;
            try
            {
                return getDataSet(sqlcmd.CommandText, (sqlcmd.CommandType == CommandType.StoredProcedure), values, new String[] { });
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
                return getDataSet(spname, true, values, tblnames);
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
            Message = String.Empty;

            if (!isAuthenticated())
            {
                redirect_to_login();
            }
            else
            {
                try
                {
                    ra = coms.execStoredProcedureUpdate(ConnectionString, spname, replace_dbnull(values));
                    if (ra < 0)
                    {
                        Message = coms.getErrorMessage();
                    }
                }
                catch (Exception e)
                {
                    Message = e.Message;
                }
            }
            return (int)ra;
        }

        public int sqlUpdate(SqlCommand sqlcmd)
        {
            if (!isAuthenticated())
            {
                redirect_to_login();
                return -1;
            }

            Int32 ra = 0;
            Message = String.Empty;

            try
            {
                sqlHelperCommandKeep cmdkeep = new sqlHelperCommandKeep();
                cmdkeep.keep(sqlcmd, false);

                ra = coms.sqlUpdate(ConnectionString, cmdkeep.toKeepObject());
                if (ra < 0)
                {
                    Message = coms.getErrorMessage();
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return (int)ra;
        }

        public int sqlUpdate(String sqlstring)
        {
            if (!isAuthenticated())
            {
                redirect_to_login();
                return -1;
            }

            Int32 ra = 0;
            Message = String.Empty;

            try
            {
                ra = coms.sqlUpdate(ConnectionString, sqlstring);
                if (ra < 0)
                {
                    Message = coms.getErrorMessage();
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return (int)ra;
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
            if (!isAuthenticated())
            {
                redirect_to_login();
                return null;
            } 
            
            Object o = null;

            try
            {
                o = coms.execScalarSp(ConnectionString, spname, replace_dbnull(values));
                if (o == null)
                {
                    Message = coms.getErrorMessage();
                }
            }
            catch (SqlException e)
            {
                Message = e.Message;
            }
            return o;
        }

        public Object execScalar(String sqlstmt)
        {
            if (!isAuthenticated())
            {
                redirect_to_login();
                return null;
            } 

            Object o = null;
            try
            {
                o = coms.execScalar(ConnectionString, sqlstmt);
                if (o == null)
                {
                    Message = coms.getErrorMessage();
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
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

        private void getWsSqlCommons()
        {
            coms = new wsSqlCommons();
            String ws_url = new irsConfiguration().appsetting("WS_SQL_COMMONS_URL");
            if (ws_url != null && ws_url.Length > 0)
            {
                coms.Url = ws_url;
                coms.Timeout = 1200000;
            }

            if (cokes == null) cokes = new System.Net.CookieContainer();
            coms.CookieContainer = cokes;
        }

        public irsSqlCommonsWs()
        {
            this.isWinApp = false;
            try { this.isProd = (!new irsConfiguration().RunMode.Equals("Devel")); }
            catch { }

            getWsSqlCommons();
        }

        public irsSqlCommonsWs(String constring_suffix)
        {
            this.constring_suffix = constring_suffix;

            this.isWinApp = false;
            try { this.isProd = (!new irsConfiguration().RunMode.Equals("Devel")); }
            catch { }

            getWsSqlCommons();
        }

        public irsSqlCommonsWs(bool isWinApp)
        {
            this.isWinApp = isWinApp;
            try { this.isProd = (!new irsConfiguration().RunMode.Equals("Devel")); }
            catch { }

            getWsSqlCommons();
        }

        ~irsSqlCommonsWs()
        {
            errmsg = null;
            coms = null;
        }

        #region Serialization Events
        [OnSerializing]
        public void OnSerializing(StreamingContext sc)
        {}

        [OnDeserialized]
        public void OnDeserialized(StreamingContext sc)
        {
            getWsSqlCommons(); 
        }
        #endregion

        #region VAPT IAG
        public bool is_mfa_authenticated()
        {
            if (System.Web.HttpContext.Current.Session["token_validity"] != null)
            {
                return (System.Web.HttpContext.Current.Session["token_validity"] != null);
            }
            return false;
        }
        #endregion
    }
}
