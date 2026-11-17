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

namespace IRSWorld.Main
{
    public partial class frmCenter : System.Web.UI.Page
    {

        private irsMessages IRSMessages
        {
            get
            {
                if (Session["IRS_MESSAGES"] == null)
                {
                    Session["IRS_MESSAGES"] = new irsMessages();
                }
                return (irsMessages)Session["IRS_MESSAGES"];
            }
        }

        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(frmCenter));
        }

        /* PNBS Integration Project */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string user_last_access_datetime()
        {
            if (Session["LOGIN_DISPLAY_LAST_ACCESS_DATE"] != null)
            {
                return Convert.ToString(Session["LOGIN_DISPLAY_LAST_ACCESS_DATE"]);
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_center()
        {
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getMessages()
        {
            DataTable dt = IRSMessages.getMessages(IRSClientData.UserId, "Posting", DateTime.Today.ToString("MM/dd/yyyy"), DateTime.Today.ToString("MM/dd/yyyy"));
            if (dt != null)
            {
                DataTable dtfilter = dt.Clone();
                foreach (DataRow row in dt.Rows)
                {
                    short mtt = Convert.ToInt16(row["MsgTargetType"]);
                    String mtgt = row["MsgTarget"].ToString().Trim();
                    if (mtt == 2 & !mtgt.Equals(IRSClientData.UserId)) continue;
                    if (mtt == 3 & !mtgt.Equals(IRSClientData.Location)) continue;
                    DataRow uprow = dtfilter.NewRow();
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        uprow[i] = row[i];
                    }
                    dtfilter.Rows.Add(uprow);
                }
                return dtfilter;
            }
            throw new Exception(IRSMessages.Message);
        }

    }
}
