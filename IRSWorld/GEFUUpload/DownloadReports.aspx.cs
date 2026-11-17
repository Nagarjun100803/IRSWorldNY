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

namespace IRSWorld.GEFUUpload
{
    public partial class DownloadReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["download_path"] != null)
                {
                    download(Request.QueryString["download_path"]);
                }
            }
        }

        private void download(string filepath)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filepath));
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(filepath).Length.ToString());
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.TransmitFile(filepath);
                Response.End();
            }
            catch (Exception ex)
            {
                //new UILibrary.Controls.csControl().showMessage(this.Page, "DOWN_EXCEP", "GET File: " + ex.Message);
            }
        }
    }
}
