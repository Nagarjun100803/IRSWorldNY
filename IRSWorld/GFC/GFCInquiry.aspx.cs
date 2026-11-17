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

namespace IRSWorld.GFC
{
    public partial class GFCInquiry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GFCInquiry));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getAvailableBalance(String CardNo)
        {
            String acctNum = "";
            String CurrCode = "";
            String CurrAscii = "";
            try
            {
                DataSet ds = getCardDetails(CardNo);

                acctNum = ds.Tables[0].Rows[0]["AccountNo"].ToString();
                CurrCode = ds.Tables[0].Rows[0]["CurrCode"].ToString();

                if (acctNum == "")
                {
                    throw new Exception("Invalid GFC Number");
                }

                switch (CurrCode)
                {
                    case "PHP": CurrAscii = "P"; break;
                    case "USD": CurrAscii = "$"; break;
                    case "EUR": CurrAscii = "€"; break;
                }
                
                irsGFC gfc = new irsGFC();
                double bal = gfc.getBalance(acctNum);                

                if (bal == 0 && gfc.message != "")
                {
                    //throw new Exception(gfc.message);
                    throw new Exception("System currently not available to process your request. Please try again later.");
                }

                return acctNum + "|" + CurrAscii + bal.ToString("###,###,##0.00") + "|" + CurrCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getTransactionHistory(String acctNum)
        {
            try
            {
                irsGFC gfc = new irsGFC();

                DataTable dt = gfc.getTransactionHistory(acctNum, DateTime.Now.AddMonths(-3), DateTime.Now);
                //DataTable dt = gfc.getTransactionHistory("226226900014", DateTime.Parse("01/01/2010"), DateTime.Parse("03/31/2010"));

                if (dt == null && gfc.message != null && gfc.message != String.Empty)
                {
                    throw new Exception(gfc.message);
                }

                DataTable fdt = FormatDataTable(dt);

                return fdt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return null; // commented to remove warning
        }

        private DataTable FormatDataTable(DataTable dt)
        {
            DataTable ndt = new DataTable();

            try
            {
                ndt.Columns.Add("TransactionDate");
                ndt.Columns.Add("Description");
                ndt.Columns.Add("Debit");
                ndt.Columns.Add("Credit");
                ndt.Columns.Add("RunningTotal");

                for (int i = 0; i < dt.Rows.Count; i++ )
                {
                    DataRow dr = dt.Rows[i];

                    String tdate = dr["TransactionDate"].ToString();
                    tdate = tdate.Substring(4, 2) + "/" + tdate.Substring(6, 2) + "/" + tdate.Substring(0, 4);
                    String drcrflag = dr["CreditDebitFlag"].ToString();
                    String amount = double.Parse(dr["AmountInAccountCurrency"].ToString()).ToString("###,###,##0.00");
                    String runTotal = double.Parse(dr["RunningTotal"].ToString()).ToString("###,###,##0.00");
                    String Dr = (drcrflag == "D") ? amount : "";
                    String Cr = (drcrflag == "C") ? amount : "";
                    Object[] param =
                        {
                            tdate
                            ,dr["Description"].ToString()
                            ,Dr
                            ,Cr
                            ,runTotal
                        };

                    ndt.Rows.Add(param);
                }

                return ndt;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to format datatable, Reason : " + ex.Message);
            }
        }

        private DataSet getCardDetails(String CardNo)
        {
            try
            {
                irsGFC gfc = new irsGFC();

                DataSet ds = gfc.getCardDetails_(CardNo);

                if (ds != null)
                {
                    return ds;
                }
                else
                {
                    throw new Exception(gfc.message);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

    }
}
