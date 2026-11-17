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
using IRSWorld.TranClass;

namespace IRSWorld.GRS.Extract
{
    public partial class Extract_PCHC : System.Web.UI.Page
    {
        private irsPCHCExtract irsPCHCExtract
        {
            get
            {
                if (Session["PCHCOutward"] == null)
                {
                    Session["PCHCOutward"] = new irsPCHCExtract();
                }
                return (irsPCHCExtract)Session["PCHCOutward"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Extract_PCHC));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String getFileNo(string trandate, string curr, string extracttype, string bankcode)
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable Extract(string ExtractType, string Trandate, string FileInd, string SFileInd, bool reload)
        {
            DataTable dt = null;
            DataSet ds = null;
            String msg = String.Empty;

            try
            {
                String BankCd = String.Empty; // Recieving bank code
                int ExtractCd = 0; // 0 : All Banks ; 1 : Specific Bank Single Curr ; 2 : Specific Bank Both Curr
                int CurrCd = 0; // 0 : PHP & USD ; 1 : PHP ; 2 : USD
                int Visible = 0; // 1 : visible in list ; 0 : not visible in list 
                string CurrCdDesc = "";
                                
                try
                {
                    int.Parse(ExtractType);
                }
                catch
                {
                    throw new Exception("Invalid Extract Code.");
                }

                BankCd = ExtractType.Substring(0, 3);
                ExtractCd = int.Parse(ExtractType.Substring(3, 1));
                CurrCd = int.Parse(ExtractType.Substring(4, 1));
                Visible = int.Parse(ExtractType.Substring(5, 1));

                switch (CurrCd)
                {
                    case 0:
                        CurrCdDesc = "PHP & USD";
                        break;
                    case 1:
                        CurrCdDesc = "PHP";
                        break;
                    case 2:
                        CurrCdDesc = "USD";
                        break;
                }
                try
                {
                    int.Parse(BankCd);
                }
                catch
                { throw new Exception("Invalid Bank Code."); }

                if (ExtractCd != 0 & ExtractCd != 1 & ExtractCd != 2) { throw new Exception("Invalid Extract Code."); }
                if (CurrCd != 0 & CurrCd != 1 & CurrCd != 2) { throw new Exception("Invalid Currency Code."); }
                if (Visible != 1 & Visible != 0) { throw new Exception("Invalid code for visible in list."); }

                try
                {
                    DateTime.Parse(Trandate);
                }
                catch
                {
                    throw new Exception("Transaction date is not in date format."); 
                }

                ds = irsPCHCExtract.getExtract(ExtractCd, BankCd, CurrCd, DateTime.Parse(Trandate), FileInd, SFileInd, out msg);

                if (ds != null)
                {
                    DataRow dr = irsPCHCExtract.getExtDetails(ExtractType, out msg);

                    String EName = String.Empty;
                    String Curr = String.Empty;

                    DataTable dtFname = new DataTable();
                    dtFname.Columns.Add("Currency");
                    dtFname.Columns.Add("FileName");

                    if (!reload && !irsPCHCExtract.tagExtracted(ds, DateTime.Parse(Trandate), FileInd, out msg, out SFileInd, CurrCdDesc, ExtractCd.ToString(), BankCd))
                    {
                        throw new Exception(msg);
                    }

                    for (int i = 1; i < ds.Tables.Count; i++)
                    {
                        if (ds.Tables[i].Rows.Count > 0)
                        {
                            Curr = ds.Tables[i].Rows[0]["acctcurr_cd"].ToString();

                            EName = irsPCHCExtract.createFileName(DateTime.Parse(Trandate), Curr, dr["fld_Misc"].ToString(), FileInd, SFileInd, out msg);
                            if (msg != null && msg != String.Empty)
                            {
                                throw new Exception("Generate filename failed, Reason : " + msg);
                            }
                            else
                            {
                                if (!irsPCHCExtract.createExtractFile(ds.Tables[i], Curr, EName, out msg))
                                {
                                    if (msg != null && msg != String.Empty)
                                    { throw new Exception(msg); }
                                    else { throw new Exception("Error writing record to : " + EName + " Curr : " + Curr); }
                                }
                                Object[] data = { Curr, EName };
                                dtFname.Rows.Add(data);
                                Session["DL_FILE_NAME"] = EName;
                            }
                        }
                    }

                    DataTable newDT = irsPCHCExtract.appendFileName(ds.Tables[0], dtFname, out msg, FileInd, SFileInd);
                    if (msg != null && msg != String.Empty) { throw new Exception("Failed to append Filename, Reason : " + msg); }
                    else
                    {
                        return newDT;
                    }
                }
                else if (msg != "")
                {
                    throw new Exception(msg);
                }
                else
                { throw new Exception("No dataset returned."); }
            }
            catch (Exception ex)
            {
                throw new Exception("Extract Error, Reason : " + ex.Message + ex.StackTrace.ToString());
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getFileIndList(string Trandate)
        {
            DataTable dt = null;
            try
            {
                dt = irsPCHCExtract.getFileNo(Convert.ToDateTime(Trandate));
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to populate File No, Reason: " + ex.Message);
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getSubFileIndList(string Trandate, string FileNo, string xcode)
        {
            DataTable dt = null;
            try
            {
                string BankCd = xcode.Substring(0, 3);
                int ExtractCd = int.Parse(xcode.Substring(3, 1));
                int CurrCd = int.Parse(xcode.Substring(4, 1));
                dt = irsPCHCExtract.getSFileNo(Convert.ToDateTime(Trandate), FileNo, CurrCd, ExtractCd, BankCd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to populate File No, Reason: " + ex.Message);
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getLookup(string fldGroup)
        {
            DataTable dt = null;
            try
            {
                dt = irsPCHCExtract.getLookUp(fldGroup);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get lookup, Reason: " + ex.Message);
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DateTime getlocaltime()
        {
            //if (Session["IRS_CLIENT_DATA"] == null)
            //{
            //    Response.Redirect("ErrorPage.aspx");
            //}
            //irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime("1170");
            return dtLocalTime;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public string getlocaltimeString()
        {
            //if (Session["IRS_CLIENT_DATA"] == null)
            //{
            //    Response.Redirect("ErrorPage.aspx");
            //}
            //irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime("1170");
            return dtLocalTime.ToString("MM/dd/yyyy");
        }
    }
}
