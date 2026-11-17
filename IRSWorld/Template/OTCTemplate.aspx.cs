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

// ********************************************************************************** //
// *** DEVELOPER    : Ronald James L. Tuazon                                      *** //
// *** DATE CREATED : March 04, 2010                                              *** //
// *** DATE MODIFIED: March 06, 2010                                              *** //
// *** DESCRIPTION  : Over the Counter Transaction                                *** //
// *** ADDITIONALS  : jsOTCTemplate.js, OTCTemplate.aspx, irsOTC.cs               *** //
// ********************************************************************************** //

namespace IRSWorld.Template
{
    public partial class OTCTemplate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OTCTemplate));
            irsARM clArm = new irsARM();
            clArm.setLocation("LDN");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneficiary(string RemitterID)
        {
            return new irsOTC().getBeneficiary(RemitterID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneficiaryAccounts(string BeneficiaryID, bool IsCIF)
        {
            return new irsOTC().getBeneficiaryAccounts(BeneficiaryID, IsCIF);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCharges(string Amount, string ProductCode, string ImplementationType, string SourceCurrency, string TargetCurrency, string ChargeType, string PayMode, string ComputationLevel, bool IsRemittance)
        {
            string sChargesID = new irsInternetPage().getChargesID(getLocation(), ProductCode, ImplementationType, SourceCurrency, ChargeType, PayMode);
            if (sChargesID == string.Empty)
            {
                return "NOCHARGE";
            }
            else
            {
                return new irsInternetPage().getCharges(Amount, Convert.ToInt16(sChargesID), Convert.ToInt16(TargetCurrency), Convert.ToInt32(ComputationLevel), false, IsRemittance, false, "") + sChargesID;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDefaults(string FldGroup)
        {
            return new irsInternetPage().getDefaults(getLocation());
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getExchangeRate(string SourceCurrency, string TargetCurrency)
        {
            return new irsOTC().getExchangeRate(getLocation(), SourceCurrency, TargetCurrency);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getLookup(string GroupName)
        {
            return new irsInternetPage().getLookup(GroupName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getLookupDefaults(string FldGroup)
        {
            return new irsOTC().getLookupDefaults(getLocation(), FldGroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitter(string SEQ_ID)
        {
            return new irsOTC().getRemitter(SEQ_ID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitterCards(string CIFID)
        {
            return new irsOTC().getRemitterCards(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSearch(string CIFClass, string CIFID, string LastName, string FirstName, string MiddleName)
        {
            return new irsOTC().getSearch(CIFClass, CIFID, LastName, FirstName, MiddleName);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getSearchBy()
        {
            return new irsOTC().getSearchBy();
        }

        
    }
}
