using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Web.UI ;
using System.Web.UI.HtmlControls; 
namespace IRSClassLib
{
    public class irsMiscellaneous
    {
        public void execScript(Page page, String key, String script)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language='javascript'>");
            sb.Append(script);
            sb.Append("</script>");
            page.ClientScript.RegisterStartupScript(typeof(System.String), key, sb.ToString());
        }
        public String render_htmlcontrol(HtmlControl control)
        {
            if (control == null)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            control.RenderControl(htw);
            String rs = sb.ToString();
            htw.Close();
            return rs;
        }
    }
}
