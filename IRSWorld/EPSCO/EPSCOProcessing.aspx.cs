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
using System.Text;
using IRSClassLib;
using IRSWorld.TranClass;


namespace IRSWorld.EPSCO
{
    [Serializable]
    public partial class EPSCOProcessing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EPSCOProcessing));
        }

        private irsEPSCOTransactions irsEPSCOTransactions
        {
            get
            {
                if (Session["EPSCOTrans"] == null)
                {
                    Session["EPSCOTrans"] = new irsEPSCOTransactions();
                }
                return (irsEPSCOTransactions)Session["EPSCOTrans"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string InsertToRemittance(int SEQ_ID)
        {
            return irsEPSCOTransactions.InsertToRemittance(SEQ_ID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDataTableValue(string param)
        {
            return irsEPSCOTransactions.getDataTable(param);
        }

    }
}
