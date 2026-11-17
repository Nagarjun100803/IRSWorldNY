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
using System.Text;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using IRSWorld.TranClass;

namespace IRSWorld.Card
{
    public partial class CardRemTeller : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //AjaxPro.Utility.RegisterTypeForAjax(typeof(CardRemTeller));

            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCardCommons));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranGFMCCommons));
            //AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_accounts));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_open_accounts));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(rem_modal_check_payments));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
        }


    }
}
