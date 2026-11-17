using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace IRSClassLib
{
    [Serializable]
    public class csControl
    {
        public void execScript(Page page, String key, String script)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript'>");
            sb.Append(script);
            sb.Append("</script>");
            page.ClientScript.RegisterStartupScript(typeof(System.String), key, sb.ToString());
        }

        public void showMessage(Page page, String msgkey, String msg)
        {
            try
            {
                String messg = (msg == null) ? String.Empty : msg;
                execScript(page, msgkey, @"alert('" + msg.Replace(Environment.NewLine, "\\r\\n").Replace("'", "\\'") + "');");
            }
            catch (Exception ex)
            {
                execScript(page, "MSG ERROR", ex.Message);
            }
        }

        public void oaam_close_frame(Page page, bool bflag, String msg, String loginid, short iflag)
        {
            StringBuilder sb = new StringBuilder();
            if (iflag != -9)
            {
                byte tries = (HttpContext.Current.Session["IRS_CLIENT_DATA"] != null) ? ((irsClientData)HttpContext.Current.Session["IRS_CLIENT_DATA"]).Tries : (byte)0;
                String userid = (loginid == null) ? "<empty>" : loginid;
                String sbflag = (bflag) ? "true" : "false";
                String xmsg = (iflag == -1) ? "OAAM: " + msg : msg;
                sb.Append("var retval = new Array(" + sbflag + ", '" + xmsg + "', '" + userid + "', " + tries + ", " + iflag + "); ");
                sb.Append("window.returnValue = retval; ");
                sb.Append("close_frame_dialog(); ");
            }
            else
            {
                sb.Append("spawn_errpage('" + msg + "');");
            }
            this.execScript(page, "pass_value", sb.ToString());
        }
    }
}
