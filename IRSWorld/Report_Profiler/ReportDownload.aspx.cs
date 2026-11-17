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

namespace IRSWorld.Report_Profiler
{
    public partial class ReportDownload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string filename = Session["DL_REPORT_FILE_NAME"].ToString();

                Response.Clear();
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "Application/x-msexcel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filename));
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(filename).Length.ToString());
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.TransmitFile(filename);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                //Response.End();

                         
            }
            catch (Exception ex)
            {
                Response.Write("<script language='jscript'>alert('Unable to access file.');window.close();</script>");
            }
            finally
            {
                //Response.End();
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
    }
}
