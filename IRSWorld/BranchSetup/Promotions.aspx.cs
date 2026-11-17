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

namespace IRSWorld.BranchSetup
{
    [Serializable]
    public partial class Promotions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Promotions));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable callSearchRecord(int SEQ_ID)
        {
            return new irsPromotions().getPromoCode(SEQ_ID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool countRecords()
        {
            return new csPromotions().startCounting();
        }


        #region Insert and Update of Promo Code
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string callInsertRecord(int SEQ_ID, string promoCode, string promoDescription, string minTransCount, string minRemAmount, string startDate, string endDate, string promoContent, string promoTerms, Boolean isAdd, int IsReceiveVoucher)
        {
            return new irsPromotions().insertPromoCode(SEQ_ID, promoCode, promoDescription, minTransCount, minRemAmount, startDate, endDate, promoContent, promoTerms, isAdd, IsReceiveVoucher);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string callRemovePromoCode(int SEQ_ID)
        {
            return new irsPromotions().removePromoCode(SEQ_ID);
        }
        #endregion

        #region Insert and Update of Promo State
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string callInsertState(int promoSEQ_ID, string state, Boolean isAdd)
        {
            return new irsPromotions().insertState(promoSEQ_ID, state, isAdd);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string callupdateState(int promoSEQ_ID, string paramFldValue, string paramFldCode, bool isAdd)
        {
            return new irsPromotions().updateState(promoSEQ_ID, paramFldValue, paramFldCode, isAdd);
        }
        #endregion

        #region Population of DropDownList
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable callPopulate_drop_down_list()
        {
            return new irsPromotions().getPopulate_drop_down_list();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable callPopulate_drop_down_list_NotExisting(int SEQ_ID)
        {
            return new irsPromotions().getPopulate_drop_down_list_NotExisting(SEQ_ID);
        }
        #endregion


    }
}
