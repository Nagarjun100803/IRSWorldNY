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

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using IRSClassLib;

namespace IRSWorld.Reports
{
    public partial class rptTest : System.Web.UI.Page
    {

        ReportDocument rptDocument = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            generateReport();
        }

        protected void generateReport()
        {
            
        }

        //public void generateReport()
        //{
        //    sComs = new irsSqlCommons();
        //    DataSet ds = new DataSet();
        //    ds = sComs.getDataSet(sComs.getSqlCommand("sp_rptTranxLog"));
        //    oRpt.SetDataSource(ds);
        //    rptViewer.Visible = true;
        //    rptViewer.ReportSource = oRpt;
        //}


            
    }
}

