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

namespace IRSWorld.Inquiry
{
    [Serializable]
    public partial class AccountInquiry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(AccountInquiry));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getGFCtoCK(string CardNo)
        {
            string sReturn = string.Empty;
            sReturn = new irsAccountInquiry().getGFCtoCK(CardNo, false);
            return sReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getFC(string AccountNo, int FCMethod, int TransactionCount, bool IsOnline)
        {
            string sReturn = string.Empty;
            if (new Main.IRSMain().checkSession())
            {
                switch (FCMethod)
                {
                    case 1: //BALANCE INQUIRY
                        sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.BalanceInquiry, 0, IsOnline);
                        break;
                    case 2: //LAST 5 TRANSACTION INQUIRY
                        sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.LastTransactionInquiry, TransactionCount, IsOnline);
                        break;
                    case 3: //HOLD INQUIRY
                        sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.HoldInquiry, 0, IsOnline);
                        break;
                    case 4: //FLOAT INQUIRY
                        sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.FloatInquiry, 0, IsOnline);
                        break;
                    case 5: //ACOUNT STATUS INQUIRY
                        sReturn = new irsAccountInquiry().getFC(AccountNo, irsAccountInquiry.FCMethod.AccountStatusInquiry, 0, IsOnline);
                        break;
                }
            }
            else
            {
                sReturn = "Inquiry failed. Login is required.";
            }
            return sReturn;
        }
    }
}
