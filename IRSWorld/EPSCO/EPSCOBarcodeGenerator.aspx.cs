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

namespace IRSWorld.EPSCO
{
    public partial class EPSCOBarcodeGenerator : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EPSCOBarcodeGenerator));
        }

        private irsClientData ClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] == null)
                {
                    throw new Exception("Session Expired.");
                }
                irsClientData icd = (irsClientData)Session["IRS_CLIENT_DATA"];
                if (String.IsNullOrEmpty(icd.UserId))
                {
                    throw new Exception("Session Expired.");
                }
                return icd;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_bcgen()
        {
            if (Session["DS_GENERATED_BARCODES"] != null)
            {
                Session["DS_GENERATED_BARCODES"] = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable generate_barcodes(String val)
        {
            irsClientData icd = ClientData;

            int ival = 0;
            try
            {
                ival = int.Parse(val);
            }
            catch (Exception e)
            {
                throw new Exception("The parameter supplied is not numeric. " + e.Message);
            }
            if (ival < 1 | ival > 999)
            {
                throw new Exception("The parameter must from 1 to 9999.");
            }

            irsEPSCOTransactions epstran = new irsEPSCOTransactions();
            /*
            DataSet ds = epstran.generate_barcodes(ival, icd.UserId);
            if (ds != null && ds.Tables.Count > 0)
            {
                Session["DS_GENERATED_BARCODES"] = ds;
                return ds.Tables[0];
            }
            */
            throw new Exception("Failed to generate " + ival + " Barcodes. " + epstran.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBarcodesForPrinting()
        {
            irsClientData icd = ClientData;
            /*
            irsEPSCOTransactions epstran = new irsEPSCOTransactions();
            DataSet ds = epstran.get_barcodes_forprinting();
            if (ds != null && ds.Tables.Count > 0)
            {
                Session["DS_GENERATED_BARCODES"] = ds;
                return ds.Tables[0];
            }
            */
            throw new Exception("Failed to get barcodes for printing."); // + epstran.Message);
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            if (Session["DS_GENERATED_BARCODES"] != null)
            {
                DataSet ds = (DataSet)Session["DS_GENERATED_BARCODES"];
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    irsSqlCommonsWs sql = new irsSqlCommonsWs();
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.ClearContent();
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=EPSCO_BULKBARCODES_" + DateTime.Now.ToString("MMddyyhhmmss") + ".csv");
                    HttpContext.Current.Response.ContentType = "text/csv";
                    HttpContext.Current.Response.AddHeader("Pragma", "public");

                    HttpContext.Current.Response.Write("Product Card No.,ID No.,Remitter Fullname,Beneficiary Fullname,Beneficiary Account No.,Enrolling Branch,Mobile No.,Barcode No.");
                    HttpContext.Current.Response.Write(Environment.NewLine);

                    int i = 1;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        try
                        {
                            sql.execScalar("UPDATE tbl_epsco_barcodes SET isPrinted = 1 WHERE SEQ_ID = '" + Convert.ToInt64(row["SEQ_ID"]).ToString() + "'");

                            HttpContext.Current.Response.Write
                                (" ," +                     // Product Card No.
                                i.ToString() + "," +        // ID No
                                " ," +              // Remitter Full Name
                                " ," +              // Beneficiary Name
                                " ," +              // Beneficiary Account
                                " ," +              // Enrolling Branch
                                " ," +              // Mobile No.
                                row["BarCodeNo"].ToString().Trim());    // Barcode no
                            HttpContext.Current.Response.Write(Environment.NewLine);
                            i++;
                        }
                        catch { }
                    }

                    HttpContext.Current.Response.End();
                }
            }
        }
    }
}
