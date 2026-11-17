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

namespace IRSWorld.Faultlog
{
    public partial class FaultlogDL : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string filename = Session["DL_FILE_NAME"].ToString();
                //string file_content = new irsUploadFile().get_file_content(filename);

                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filename));
                Response.AppendHeader("Content-Length", new System.IO.FileInfo(filename).Length.ToString());
                Response.AppendHeader("Content-Transfer-Encoding", "binary");
                Response.TransmitFile(filename);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write("<script language='jscript'>alert('Unable to access file.');window.close();</script>");
            }
            finally
            {
                Response.End();
            }
        }
    }
}
