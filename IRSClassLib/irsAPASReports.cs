using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using CrystalDecisions.CrystalReports.Engine;
using System.Web;

namespace IRSClassLib
{
    [Serializable]
    public class irsAPASReports : sqlHelperWs
    {
        public string _uid;
        public string _pwd;
        public string _servernm;
        public string _dbnm;

        public irsAPASReports()
        { }

        public irsAPASReports(string uid, string pwd, string servernm, string dbnm)
        {
            _uid = uid;
            _pwd = pwd;
            _servernm = servernm;
            _dbnm = dbnm;
        }

        public DataSet getAllReports(string uid, int roleid)
        {
            DataSet ds = getDataSet(getSqlCommand("Select * from TBL_APAS_REPORTS Where RoleID=" + roleid + " order by rptnm", false));
            return ds;
        }

        public bool isFixed(string varid)
        {
            object o = execScalar("Select Fixed from TBL_APAS_REPORT_FIELDS Where Varid='" + varid + "'");
            if (o == null || o.ToString().Length <= 0)
            {
                return false;
            }
            return Convert.ToBoolean(o);
        }
        


        


    


    }
}
