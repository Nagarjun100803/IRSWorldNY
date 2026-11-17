using System;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace E2EAPIConnector
{
    [Serializable]
    public class SQLHelper
    {
        private static StreamWriter _swlog;
        private static Object _wlock = new Object();
        private static String _evSourceId = "Application";
        private static String _logname = ConfigurationManager.AppSettings["ApplicationName"];

        #region transact-sql
        private static SqlConnection _SQLConn;
        private static SqlCommand _SQLCmd;
        public static string _sConn;

        public string connectionString
        {
            get { return _sConn; }
            set { _sConn = value; }
        }

        private void openSQLConnection()
        {
            _SQLConn = new SqlConnection(_sConn);
            if (_SQLConn.State == ConnectionState.Closed)
            {
                _SQLConn.Open();
            }
        }

        private void closeSQLConnection()
        {
            if (!(_SQLConn == null))
            {
                _SQLConn.Close();
                _SQLConn.Dispose();
            }
            if (!(_SQLCmd == null))
            {
                _SQLCmd.Dispose();
            }
        }

        public string execSQLParameter(string SPName, string[] Param, string[] Value, string Output)
        {
            try
            {
                closeSQLConnection();
                openSQLConnection();

                int i = 0;
                _SQLCmd = new SqlCommand();
                _SQLCmd.Connection = _SQLConn;
                _SQLCmd.CommandText = SPName;
                _SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(_SQLCmd);

                for (i = 0; i <= (Param.Length - 1); i++)
                {
                    _SQLCmd.Parameters["@" + Param.GetValue(i).ToString()].Value = Value.GetValue(i).ToString();
                    if (Param.GetValue(i).ToString() == Output)
                    {
                        _SQLCmd.Parameters["@" + Param.GetValue(i).ToString()].Direction = ParameterDirection.Output;
                    }
                }
                _SQLCmd.ExecuteNonQuery();
                return _SQLCmd.Parameters["@" + Output].Value.ToString();
            }
            catch (Exception ex)
            {
                this.logMe("Error on execSQLParameter: " + ex.Message + " SQL: " + SPName + " Param: " + string.Join(", ", Param) + " Value: " + string.Join(", ", Value));
                return ex.Message;
            }
            finally
            {
                closeSQLConnection();
            }
        }

        public int execSQLParameter(string SPName, string[] Param, string[] Value)
        {
            try
            {
                closeSQLConnection();
                openSQLConnection();

                int i = 0;
                int j = 0;
                _SQLCmd = new SqlCommand();
                _SQLCmd.Connection = _SQLConn;
                _SQLCmd.CommandText = SPName;
                _SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(_SQLCmd);

                for (i = 0; i <= (Param.Length - 1); i++)
                {
                    _SQLCmd.Parameters["@" + Param.GetValue(i).ToString()].Value = Value.GetValue(i).ToString();
                }
                j = _SQLCmd.ExecuteNonQuery();
                return j;
            }
            catch (Exception ex)
            {
                this.logMe("Error on execSQLParameter: " + ex.Message + " SQL: " + SPName + " Param: " + string.Join(", ", Param) + " Value: " + string.Join(", ", Value));
                return -1;
            }
            finally
            {
                closeSQLConnection();
            }
        }

        public DataTable execSQLParameterReader(string SPName, string[] Param, string[] Value)
        {
            string ret = string.Empty;
            SqlDataReader dr = null;
            try
            {
                closeSQLConnection();
                openSQLConnection();

                int i = 0;
                _SQLCmd = new SqlCommand();
                _SQLCmd.Connection = _SQLConn;
                _SQLCmd.CommandText = SPName;
                _SQLCmd.CommandType = CommandType.StoredProcedure;
                SqlCommandBuilder.DeriveParameters(_SQLCmd);

                if (Param != null && Param.Length > 0)
                {
                    for (i = 0; i <= (Param.Length - 1); i++)
                    {
                        _SQLCmd.Parameters["@" + Param.GetValue(i).ToString()].Value = Value.GetValue(i).ToString();
                    }
                }
                dr = _SQLCmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
            catch (Exception ex)
            {
                this.logMe("Error on execSQLParameterReader: " + ex.Message + " SQL: " + SPName + " Param: " + string.Join(", ", Param) + " Value: " + string.Join(", ", Value));
                return null;
            }
            finally
            {
                closeSQLConnection();
                if (dr != null)
                {
                    dr.Close();
                    dr = null;
                }
            }
        }
        #endregion

        #region Logging
        private void logMe(string msg)
        {
            try
            {
                string drive = ConfigurationManager.AppSettings["LogDrive"];
                string folder = ConfigurationManager.AppSettings["LogFolder"];
                string subfolder = ConfigurationManager.AppSettings["LogSubFolder"];

                try
                {
                    if (folder != string.Empty && !folder.EndsWith(@"\")) folder += @"\";
                    if (subfolder != string.Empty && !subfolder.EndsWith(@"\")) subfolder += @"\";

                    string path = drive + folder + subfolder;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string file = path + ConfigurationManager.AppSettings["LogFileName"] + "_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
                    FileStream fs = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    string str = DateTime.Now.ToString("MMddyy hh:mm:ss") + " " + msg;

                    _swlog = new StreamWriter(fs);
                    lock (_wlock)
                    {
                        _swlog.WriteLine(str);
                        _swlog.Flush();
                    }
                }
                catch (Exception ex)
                {
                    writeEvent(folder + ": " + ex.Message + "\r\n" + ex.StackTrace + "\r\n" + msg);
                }
                finally
                {
                    if (_swlog != null)
                    {
                        _swlog.Close();
                        _swlog.Dispose();
                    }
                    _swlog = null;
                }
            }
            catch { return; }
        }

        private static void writeEvent(String msg)
        {
            try
            {
                if (!EventLog.SourceExists(_evSourceId))
                {
                    EventLog.CreateEventSource(_evSourceId, _logname);
                }
                EventLog.WriteEntry(_evSourceId, _logname + ": " + msg, EventLogEntryType.Error);
            }
            catch { }
        }
        #endregion
    }
}
