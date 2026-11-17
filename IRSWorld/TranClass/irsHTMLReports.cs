/* ===========================================
 * AUTHOR: Jeffrey M. Buizon
 * DATE CREATED: 12/21/2011
 * DESCRIPTION: class for HTML IRSWORLD reports
 * ============================================
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient ;
using System.Collections;
using IRSClassLib;
namespace IRSWorld.TranClass
{
    [Serializable]
    public class irsHTMLReports : sqlHelperWs 
    {
        public ArrayList rep_param = new ArrayList();
        public ArrayList rep_values = new ArrayList();
        public string sp_name = "";

        public irsHTMLReports(string _sp_name,string _rep_values)
        {
            this.sp_name = _sp_name;
            this.populate_report_values(_rep_values);
        }
        public irsHTMLReports()
        {            
        }
        public DataSet load_report_dataset()
        {
            SqlCommand cmd = base.deriveParameters(this.sp_name);            
            base.EnforceConstraint = false;
            if (cmd != null)
            {
                this.PrepareParameters(cmd);
                return base.getDataSet(cmd, "TableDynamicReports");

            }
            return null;
        }
        private void PrepareParameters(SqlCommand sqlcmd)
        {
            for (int i = 0; i < rep_param.Count; i++)
            {
                String pname = rep_param[i].ToString();
                pname = pname.Substring(pname.IndexOf('@'));
                SqlParameter p = null;
                try
                {
                    p = sqlcmd.Parameters[pname];
                }
                catch { }

                if (p != null && (p.Direction == ParameterDirection.Input || p.Direction == ParameterDirection.InputOutput))
                {
                    p.Value = convertType(p.DbType, rep_values[i].ToString());
                }
            }
        }
        private void populate_report_values(string delimparam)
        {
            String[] sdelipar = delimparam.Split('^');
            this.clear_arrays();
            String[] sarr1;
            for (int i = 0; i < sdelipar.Length; i++)
            {
                sarr1 = sdelipar[i].Split('=');
                if (sarr1[0].Length > 0)
                {
                    this.rep_param.Add(sarr1[0]);
                    this.rep_values.Add(sarr1[1]);
                }
            }
        }
        private void clear_arrays()
        {
            try
            {
                this.rep_param.Remove(0);         
                this.rep_values.Remove(0);
            }
            catch { }
        }
        #region FOR HTML REPORT
        public string getHTMLReportByReportMod(string _merge, string _param)
        {
            sqlHelperWs ws = new sqlHelperWs();
            DataTable dt = new DataTable();
            irsReports rep;
            try
            {
                rep = new irsReports(_merge, _param, "false");
                dt = ws.execStoredProcedure("proc_get_html_report", new string[] { rep.p_sp_name }).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["FLDDESCRIPTION"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
            finally
            {
                dt.Dispose();
                dt = null;
                ws = null;
                rep = null;
            }


        }
        public string getHTMLReportByDirect(string _trantype)
        {
            sqlHelperWs ws = new sqlHelperWs();
            DataTable dt = new DataTable();
            try
            {

                dt = ws.execStoredProcedure("proc_get_html_report", new string[] { _trantype }).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["FLDDESCRIPTION"].ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
            finally
            {
                dt.Dispose();
                dt = null;
                ws = null;
            }


        }
        #endregion
    }
}
