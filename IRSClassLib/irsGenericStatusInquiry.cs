using System;
using System.Collections.Generic;
using System.Text;
using System.Data ;
using System.Data.SqlClient;
namespace IRSClassLib
{
    public class irsGenericStatusInquiry :sqlHelper 
    {
        private string _error_message;
        private int _user_role;
        private string _branchcode;
        public string p_errormsg
        {
            get { return this._error_message; } 
        }

        public irsGenericStatusInquiry(string branchcode, int role)
        {
            this._branchcode = branchcode;
            this._user_role = role;
        }
        public DataSet load_lookup_values()
        {
            object[] plist = { this._branchcode,this._user_role   };
            return this.load_dynamicsql(plist, "proc_load_lookup_genstat");
        }
        public DataSet load_search_values(DateTime toDate,DateTime fromDate,string status,int role)
        {
            int transource = this.get_status_transource(role);
            if (transource == 0)
            {
                this._error_message = "NO DEFINE ROLE PER SOURCE";
                return null;
            }
            object[] plist = {  fromDate, toDate, status, transource ,this._error_message};
            return this.load_dynamicsql(plist, "proc_search_genstat");
        }
        public DataSet load_dynamicsql(Object[] plist, string sp_name)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd = base.getSqlCommand(sp_name);
                base.deriveParameters(cmd, plist);
                return base.getDataSet(cmd, "tblSTAT");
            }
            catch (Exception e)
            {
                this._error_message = e.Message + ":" + base.Message ;
                return null;
            }
        }
        public int get_status_transource(int userrole)
        {
            int statustype = 0;

            // 1 TBG 2 IBFT 3 BPO
            try
            {
                object[] plist = { userrole };
                DataTable dt = this.load_dynamicsql(plist, "proc_get_status_format").Tables[0];
                
                if ( dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["FLDVALUE"].ToString());
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
       
    }


}
