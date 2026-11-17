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
    public partial class CurrencyRates2 : System.Web.UI.Page
    {
        private irsCurrencyRates IRSCurrencyRates
        {
            get
            {
                if (Session["IRS_CURRENCY_RATES"] == null)
                {
                    Session["IRS_CURRENCY_RATES"] = new irsCurrencyRates();
                }
                return (irsCurrencyRates)Session["IRS_CURRENCY_RATES"];
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

        private bool isAuthorized(String branch)
        {
            return true; // for testing.

            bool isSysad = IRSClientData.isSystemAdmin;

            if (isSysad) return true;

            bool isBrad = IRSClientData.isBranchAdmin;
            if (isBrad)
            {
                if (branch == null || branch == String.Empty) return true;
                return (IRSClientData.Location.Equals(branch));
            }

            return false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(CurrencyRates2));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_currates()
        {
            Session["IRS_CURRENCY_RATES"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getCybermagRates(String sdt)
        {
            try
            {
                irsForexUpload cybermag = new irsForexUpload();
                Object[] rs = cybermag.getUploadRates(DateTime.Parse(sdt));
                if (rs != null)
                {
                    return rs;
                }
                throw new Exception(cybermag.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private String[] create_options(DataRowCollection rows, String val_fld, String data_fld)
        {
            ArrayList al = new ArrayList();
            foreach (DataRow row in rows)
            {
                al.Add(row[val_fld].ToString().Trim() + "~" + row[data_fld].ToString().Trim());
            }
            String[] ss = new String[al.Count];
            al.CopyTo(ss);
            return ss;
        }

        private String[] getBranchOptions()
        {
            DataTable dt = IRSCurrencyRates.getBranches();

            if (dt == null) throw new Exception("GET Branches: " + IRSCurrencyRates.Message);
            return create_options(dt.Rows, "BranchCode", "BranchName");
        }

        private Object[] getCurrencyOptions(String brcode)
        {
            DataSet ds = IRSCurrencyRates.getCurrencyTables(brcode);
            if (ds == null) throw new Exception("GET Lookup currency: " + IRSCurrencyRates.Message);
            String[] src_curr_opts = null;
            String[] tgt_curr_opts = null;
            if (ds.Tables.Contains("LOCAL"))
            {
                src_curr_opts = create_options(ds.Tables["LOCAL"].Rows, "fldValue", "fldCode");
            }
            if (ds.Tables.Contains("ACCOUNT"))
            {
                tgt_curr_opts = create_options(ds.Tables["ACCOUNT"].Rows, "fldValue", "fldCode");
            }

            String[] curr_opts = null;
            DataTable dtcurr = IRSCurrencyRates.getCurrencyTable();
            if (dtcurr != null)
            {
                curr_opts = create_options(dtcurr.Rows, "fldValue", "fldCode");
            }


            return new Object[] { src_curr_opts, tgt_curr_opts, curr_opts };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getDdlOptions(String x)
        {
            String currbr = IRSClientData.Location;
            String[] bropt = getBranchOptions();

            String xbr = (currbr != null && currbr.Length > 0) ? currbr : "LDN"; //IRSClientData.Location;
            Object[] curropt = getCurrencyOptions(xbr);


            return new Object[] 
            { 
                xbr, 
                bropt,
                true, //IRSClientData.isSystemAdmin
                curropt[0], 
                curropt[1],
                curropt[2]
            };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object getCurrencyRates(String sdate, String branch)
        {
            String xbr = (branch != null && branch.Length > 0) ? branch : "LDN"; //IRSClientData.Location;
            if (isAuthorized(xbr))
            {
                try
                {
                    DataTable dt = IRSCurrencyRates.getCurrencyRates(DateTime.Parse(sdate), xbr);
                    if (dt == null)
                    {
                        throw new Exception(IRSCurrencyRates.Message);
                    }
                    return dt;
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            throw new Exception("Authorization not sufficient for function: getCurrencyRates");
        }
    }
}
