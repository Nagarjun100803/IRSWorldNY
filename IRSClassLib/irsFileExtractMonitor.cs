using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
namespace IRSClassLib
{
    [Serializable]
    public class irsFileExtractMonitor : irsSqlCommonsWs
    {
        #region VARIABLES
        public DateTime systemdate;
        public string branchcode;
        public int isAdmin;
        #endregion
        #region PUBLIC METHOD
        public irsFileExtractMonitor(string _branchcode,int _isAdmin)
        {
            irsTimeZone itz = new irsTimeZone();
            this.systemdate = itz.branchLocalTime(_branchcode);
            this.branchcode = _branchcode;
            this.isAdmin = _isAdmin;
            itz = null;
        }
        public DataSet load_extracted_file(DateTime _date,string _tran_source ,int _status)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("proc_load_extracted_file");
                base.deriveParameters(cmd, new Object[] { _date, this.branchcode , _tran_source,_status });
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
            }
        }
        public DataSet load_init_state()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("proc_load_init_state");
                base.deriveParameters(cmd, new Object[] { this.systemdate, this.branchcode, this.isAdmin  });
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
            }
        }
        #endregion
    }
}
