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
namespace IRSWorld.IRSFileLoader
{
    public partial class FileDownloader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            irsFileDownloader fd = new irsFileDownloader();

            try
            {
                if (Session["DL_FILE_NAME"] != null)
                {   
                    string[] filename = Session["DL_FILE_NAME"].ToString().Split('~');
                    //string file_content = fd.get_file_content(filename);
                    //fd.get_file_content(filename);

                    if (fd.get_file_content(filename))
                    {
                        Response.Clear();
                        Response.AddHeader("Content-disposition", "attachment; filename=" + fd.FileName);
                        Response.ContentType = "application/octet-stream";
                        Response.AppendHeader("Content-Transfer-Encoding", "binary");
                        Response.TransmitFile(fd.FileDirectory);
                        return;
                    }
                }

                //new csControl().showMessage(this.Page, "DOWN_EXCEP", "GET File: " + fd.ErrMessage);
                Response.Write("<script language='jscript'>alert('" + fd.ErrMessage + "');window.close();</script>");
            }
            catch (Exception ex)
            {
                fd.writeAudit(ex.Message, csAuditLog.Category.File);
                //new csControl().showMessage(this.Page, "DOWN_EXCEP", "GET File: " + ex.Message);
                Response.Write("<script language='jscript'>alert('Connection to files server interrupted! Please contact system administrator.');window.close();</script>");
            }
            finally
            {
                Session["DL_FILE_NAME"] = null;
                fd = null;
                Response.End();
            }
        }
    }
}
