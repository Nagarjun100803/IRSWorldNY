using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;

namespace IRSWorld.APAS
{
    public partial class APASReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(APASReports));
        }

        private irsAPAS objAPAS
        {
            get
            {
                if (Session["objAPAS"] == null)
                {
                    Session["objAPAS"] = new irsAPAS();
                }
                return (irsAPAS)Session["objAPAS"];
            }
        }

        private irsClientData irsClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
                {
                    System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
                }
                return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
        }

        private irsAPASReports objReports
        {
            get
            {
                if (Session["objReports"] == null)
                {
                    Session["objReports"] = new irsAPASReports();
                }
                return (irsAPASReports)Session["objReports"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReportData(int roleid,string rptid)
        {
            return objAPAS.getReportData(roleid, rptid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAllReports(string uid,int roleid)
        {
            return objReports.getAllReports(uid,roleid);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getDataType(string varid)
        {
            return objAPAS.getDataType(varid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSQL(string tblnm, string whereclause)
        {
            return objAPAS.getSQL(tblnm, whereclause);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string tbl_src(string varid)
        {
            return objAPAS.tbl_src(varid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool load_report(string values, string rptnm, string spname)
        {
            string connstring = new AppSettingsReader().GetValue("DevelConnectionString", Type.GetType("System.String")).ToString();
            string[] arr_conn_details = connstring.Split(';');
            string[] server = arr_conn_details[0].Split('=');
            string[] database = arr_conn_details[2].Split('=');
            string[] userid = arr_conn_details[3].Split('=');
            string[] password = arr_conn_details[4].Split('=');

            //irsAPASReports a = new irsAPASReports(userid[1], password[1], server[1], database[1]);
            
            ReportDocument doc =  generate_report(values, rptnm, spname);
            //MemoryStream stream = (MemoryStream)doc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            if (isokforload())
            {
                HttpContext.Current.Session["IRS_Reports"] = doc;
                //doc.Close();
                return true;
            }
            return false;
        }



        private string rptpath;
        public DataSet execStoredProc(string spname, string[] param)
        {
            DataSet ds = new sqlHelperWs().execStoredProcedure(spname, param);
            return ds;
        }


        public ReportDocument generate_report(string values, string rptnm, string spnm)
        {
            string[] param = values.Split(',');
            try
            {
                DataSet ds = execStoredProc(spnm, param);
                return getReportDataSet(rptnm, ds);
            }
            catch
            {
                return null;
            }
        }

        
        public ReportDocument getReportDataSet(string rptnm, DataSet ds)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument repDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            try
            {
                this.rptpath = System.Web.HttpContext.Current.Server.MapPath("..\\APAS\\" + rptnm + ".rpt");
                repDoc.Load(this.rptpath);
                repDoc.SetDataSource(ds.Tables[0]);
                //repDoc.SetDatabaseLogon("irsuser", "password", "pnbirsdsdd01", "IRSWORLD");
                //repDoc.SetDatabaseLogon(this._uid, this._pwd, this._servernm, this._dbnm);
                return repDoc;
            }
            catch
            {
                return null;
            }
        }


        public bool isokforload()
        {
            csFileIo io = new csFileIo();
            try
            {
                return io.exist(this.rptpath);
            }
            catch { return false; }
            finally { io = null; }
        }



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_irsclient_data(string param)
        {
            string s = null;
            switch (param)
            {
                case "UID": s = irsClientData.UserId; break;
                case "CIF": s = irsClientData.CIFID; break;
                case "LOCATION": s = irsClientData.Location; break;
                case "IP": s = irsClientData.IPAddress; break;
                case "ROLE": s = Convert.ToString(irsClientData.Role); break;
            }
            return s;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isFixed(string varid)
        {
            return objReports.isFixed(varid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_branch_localtime(string brcode)
        {
            DateTime dt = new irsTimeZone().branchLocalTime(brcode);
            return dt;
        }
    }
}
