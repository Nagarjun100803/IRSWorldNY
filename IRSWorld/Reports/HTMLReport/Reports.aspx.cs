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
using IRSWorld.TranClass;
namespace IRSWorld.Reports.HTMLReport
{
    [Serializable]
    public partial class HTML_Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(HTML_Reports));

            string js_url = this.check_url_report(Request.QueryString.ToString());

            //this.hidQueryString.Value = Request.QueryString.ToString();
            //this.hidURL.Value = js_url;

            if (IRSClientData != null)
            {
                //this.hidBranchcode.Value = this.IRSClientData.Location;
                if (js_url.Length > 0)
                    Response.Write(this.create_html_content(js_url, Request.QueryString.ToString(), this.IRSClientData.Location, js_url));
            }



        }

        private irsClientData IRSClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }
                return null;
            }
        }
        #region "AJAX PUBLIC METHOD"
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_dataset(string sp_name, string parameters)
        {

            irsHTMLReports htmrep = new irsHTMLReports(sp_name, parameters);

            try
            {
                return htmrep.load_report_dataset();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                htmrep = null;
            }
        }
        #endregion
        private string check_url_report(string squery)
        {
            irsHTMLReports rep;
            try
            {
                if (squery.ToLower().Contains("trantype"))
                {
                    rep = new irsHTMLReports();
                    return rep.getHTMLReportByDirect("trantype=" + Request.QueryString["trantype"].ToString().Trim());
                }
                else
                {
                    string merge = Request.QueryString["mergeparam"].ToString();
                    string param = Request.QueryString["paramvalues"].ToString();
                    rep = new irsHTMLReports();
                    return rep.getHTMLReportByReportMod(merge, param);
                }
            }
            catch (Exception e)
            {
                return "";
            }
            finally
            {
                rep = null;
            }
        }

        private string create_html_content(string js_url, string squery, string sbranch, string surl)
        {
            string shtml = "";
            shtml += "            <script type='text/javascript' src='../../js/Reports/HTMLReport/Reports.js'></script>\n";
            shtml += "            <script type='text/javascript' src='" + js_url + "'></script>\n";
            shtml += "        <body onload='javascript:reportholder.load_report();'>\n";
            shtml += "        <form id='form2' runat='server' method='post' action='Reports.aspx'>                    \n";
            shtml += "            <input  type='hidden' id='hidQueryString' value='" + squery + "' />\n";
            shtml += "            <input type='hidden' id='hidBranchcode' value='" + sbranch + "'/>\n";
            shtml += "            <input  type='hidden' id='hidURL'/ value='" + surl + "'>\n";
            shtml += "        <div id='bodydiv' style='background-color:White;width:auto;height:auto;' >\n";
            shtml += "        </div>\n";
            shtml += "        </form>\n";
            shtml += "        </body>\n";
            return shtml;

        }
    }
}
