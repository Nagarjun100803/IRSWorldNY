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

namespace IRSWorld.Query
{
    public partial class GRSQueryUtil : System.Web.UI.Page
    {
        private void download(String path)
        {
            try
            {
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(path));
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(path).Length.ToString());
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.TransmitFile(path);
                Response.End();
            }
            catch (Exception ex)
            {
                new csControl().showMessage(this.Page, "DOWN_EXCEP", "GET File: " + ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["download_path"] != null)
                {
                    download(Request.QueryString["download_path"]);
                }
                if (Request.QueryString["upload_path"] != null)
                {
                    FileUpload1.Visible = true;
                    Label1.Visible = true;
                    Label1.Text = Request.QueryString["upload_path"];
                    Button1.Visible = true;
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.PostedFile.ContentLength > 0)
            {
                try
                {
                    String s = (Label1.Text.Length > 0) ? Label1.Text : FileUpload1.PostedFile.FileName;
                    Label1.Text = s;
                    FileUpload1.PostedFile.SaveAs(Label1.Text);
                    Label1.Text = "File " + FileUpload1.FileName + " uploaded " + Label1.Text;
                }
                catch (Exception ex)
                {
                    Label1.Text = "File Upload Failed. " + ex.Message;
                }
                finally
                {
                    FileUpload1.Visible = false;
                    Button1.Visible = false;
                }
            }
        }
    }
}
