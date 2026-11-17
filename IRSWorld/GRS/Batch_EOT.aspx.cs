using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using IRSClassLib;

namespace IRSWorld.GRS
{
    public partial class Batch_EOT : System.Web.UI.Page
    {
        private irsInternetPage irsInternetPage
        {
            get
            {
                if (Session["Batch_EOT"] == null)
                {
                    Session["Batch_EOT"] = new irsInternetPage();
                }
                return (irsInternetPage)Session["Batch_EOT"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Batch_EOT));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime getTransactionDate(string BranchCode, bool CutOff)
        {
            return irsInternetPage.getTransactionDate(BranchCode, CutOff);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getFileInd()
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.getDataSet(commons.getSqlCommand("select isnull(max(EOTTag),0)+1,max(Tranxdate) as FileInd from tbl_Ibatch_EOT", false));

            string val = ds.Tables[0].Rows[0][0].ToString();
            return val;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String[] checkEOT(string dateTransfer)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = commons.getSqlCommand("sp_ibatch_checkEOT");
            DataSet ds = commons.execStoredProcedure(cmd, new object[] { dateTransfer });

            string[] val = new string[2];
            val[0] = ds.Tables[0].Rows[0][0].ToString();
            val[1] = ds.Tables[0].Rows[0][1].ToString();

            return val;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet saveStatus(string dateTransfer, int fileInd, string desc, string overrideDate, int reload, int status)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = commons.getSqlCommand("sp_insert_ibatch_EOTstatus");
            DataSet ds = commons.execStoredProcedure(cmd, new object[] { dateTransfer, fileInd, desc, overrideDate, reload, status });
            
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable showSummary(int SEQ_ID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = commons.getSqlCommand("sp_ibatch_EOTsummary");
            DataSet ds = commons.execStoredProcedure(cmd, new object[] { SEQ_ID });

            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String[] checkStatus()
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.getDataSet(commons.getSqlCommand("select [description], status from tbl_ibatch_eot_status", false));

            String[] stat = new String[2];
            stat[0] = ds.Tables[0].Rows[0][0].ToString();
            stat[1] = ds.Tables[0].Rows[0][1].ToString();

            return stat;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserBranch()
        {
            irsClientData clientData = (irsClientData)Session["IRS_CLIENT_DATA"];
            return clientData.Location;
        }

    }
}
