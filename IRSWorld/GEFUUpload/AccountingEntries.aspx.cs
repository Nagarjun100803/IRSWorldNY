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
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.GEFUUpload
{
    public partial class AccountingEntries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(AccountingEntries));
        }

        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string ACCOUNTINGENTRIES_SEQ_ID()
        { 
            if(Session["ACCOUNTINGENTRIES_SEQ_ID"]!= null)
            {
                return (string)Session["ACCOUNTINGENTRIES_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_ACCOUNTINGENTRIES_RECORD(string accountingentries_seq_id)
        {
            irsAccountingEntries accountingentries = this.ACCOUNTINGENTRIES();
            try 
            {
                if (accountingentries !=null)
                {
                    if (accountingentries.get_record(accountingentries_seq_id) == true)
                    {
                        return accountingentries.getDataRecord();
                    }
                }
                return null;
            }
            catch
            { 
                return null; 
            }
            finally 
            {
                Session["IRS_ACCOUNTINGENTRIES"] = accountingentries;
            }

        }


        #endregion

#region PRIVATE METHOD: ACCOUNTINGENTRIES
        private irsAccountingEntries ACCOUNTINGENTRIES()
        {
            if (Session ["IRS_ACCOUNTINGENTRIES"]==null)
            {
                Session ["IRS_ACCOUNTINGENTRIES"]= new irsAccountingEntries();
            }

            return (irsAccountingEntries)Session["IRS_ACCOUNTINGENTRIES"];
        }
#endregion
    }
}
