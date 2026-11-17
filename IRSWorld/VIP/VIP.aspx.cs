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
using System.Data.Sql;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using IRSClassLib;

// ********************************************* //
// *** DEVELOPER    : Ronald James L. Tuazon *** //
// *** DATE CREATED : February 03, 2010      *** //
// *** DATE MODIFIED: February 04, 2010      *** //
// *** DESCRIPTION  : HK VIP Page            *** //
// ********************************************* //

namespace IRSWorld.VIP
{
    [Serializable]
    public partial class VIP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VIP));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserID()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime getLocalDT()
        {
            DateTime dt = new irsTimeZone().branchLocalTime(getLocation());
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void writeAudit(string Description, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = "Location=" + getLocation() + " : " + Description;
            new csAuditLog().writeAudit("", "VIP Page", sDescription, NewValue, OldValue, Category);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getOrganization()
        {
            return new irsInternetPage().getLookup("HKOrganization");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getReports()
        {
            return new irsInternetPage().getLookup("HKVIPReports");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getYear()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            int i = 0;
            int j = 0;
            DateTime dtLocalDT = this.getLocalDT();
            int iYear = 0;

            dt.Columns.Add("SEQID");
            dt.Columns.Add("Year");
            iYear = Convert.ToInt32(dtLocalDT.ToString("yyyy"));

            for (i = 0; i <= 5; i++)
            {
                j++;
                dr = dt.NewRow();
                dr[0] = j;
                dr[1] = iYear.ToString();
                dt.Rows.Add(dr);
                iYear = iYear - 1;
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getQuarter()
        {
            DataTable dt = new DataTable();
            DataRow dr;
            int i = 0;

            dt.Columns.Add("SEQID");
            dt.Columns.Add("Quarter");

            for (i = 1; i <= 4; i++)
            {
                dr = dt.NewRow();
                dr[0] = i.ToString();
                switch (i.ToString())
                {
                    case "0":
                        dr[1] = "ALL QUARTERS";
                        break;
                    case "1":
                        dr[1] = i.ToString() + "ST QUARTER";
                        break;
                    case "2":
                        dr[1] = i.ToString() + "ND QUARTER";
                        break;
                    case "3":
                        dr[1] = i.ToString() + "RD QUARTER";
                        break;
                    case "4":
                        dr[1] = i.ToString() + "TH QUARTER";
                        break;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable searchCIF(string CIFID, string LastName, string FirstName, string MiddleName, string SEQ_ID)
        {
            return new irsVIP().searchCIF(getLocation(), CIFID, LastName, FirstName, MiddleName, SEQ_ID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string setDate(string Date)
        {
            string[] date = Date.Split('/');
            return date.GetValue(2).ToString() + "-" +
                date.GetValue(1).ToString() + "-" +
                date.GetValue(0).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string saveVIP(string CIFID, string LastName, string FirstName, string MiddleName, string MobileNo, string vOrganization, string Organization, string HKID, string ApplicationDate, string ExpiryDate)
        {
            bool bret = false;
            string applicationDate = this.setDate(ApplicationDate);
            string expiryDate = this.setDate(ExpiryDate);

            bret = new irsVIP().saveVIP(getLocation(), getUserID(), CIFID, LastName, FirstName, MiddleName, MobileNo, vOrganization, Organization, HKID, applicationDate, expiryDate);
            writeAudit("CIFID=[" + CIFID + "] LastName=[" + LastName + "] FirstName=[" + FirstName + "] MiddleName=[" + MiddleName + "] MobileNo=[" + MobileNo + "] Organization=[" + Organization + "] HKID=[" + HKID + "] ApplicationDate=[" + applicationDate + "] ExpiryDate=[" + expiryDate + "]", "ReturnValue=[" + bret.ToString() + "]", "saveVIP", csAuditLog.Category.Insert);
            if (bret)
            {
                return "OK";
            }
            else
            {
                return "FAILED";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string updateVIP(string SEQ_ID, string CIFID, string MobileNo, string vOrganization, string Organization, string HKID, string isPrinted, string ApplicationDate, string ExpiryDate)
        {
            bool bret = false;
            string applicationDate = this.setDate(ApplicationDate);
            string expiryDate = this.setDate(ExpiryDate);

            bret = new irsVIP().updateVIP(getUserID(), SEQ_ID, CIFID, MobileNo, vOrganization, Organization, HKID, isPrinted, applicationDate, expiryDate);
            writeAudit("SEQ_ID=[" + SEQ_ID + "] CIFID=[" + CIFID + "] MobileNo=[" + MobileNo + "] Organization=[" + Organization + "] HKID=[" + HKID + "] IsPrinted=[" + isPrinted + "] ApplicationDate=[" + applicationDate + "] ExpiryDate=[" + expiryDate + "]", "ReturnValue=[" + bret.ToString() + "]", "updateVIP", csAuditLog.Category.Update);
            if (bret)
            {
                return "OK";
            }
            else
            {
                return "FAILED";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string deleteVIP(string SEQ_ID)
        {
            bool bret = false;
            bret = new irsVIP().deleteVIP(getUserID(), SEQ_ID);
            writeAudit("SEQ_ID=[" + SEQ_ID + "]", "ReturnValue=[" + bret.ToString() + "]", "deleteVIP", csAuditLog.Category.Delete);
            if (bret)
            {
                return "OK";
            }
            else
            {
                return "FAILED";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string extractToFile(string vOrganization, string DateFrom, string DateTo)
        {
            DataSet ds = new DataSet();
            DataTable dt = new irsVIP().extractToFile(getLocation(), vOrganization, DateFrom, DateTo);
            DataTable dtExtract = new DataTable();
            DataRow dr;
            int i = 0;
            bool bret = false;
            bool bpret = false;
            string errorMsg = string.Empty;
            string fileName = @"C:\VIP.csv";

            if (dt != null && dt.Rows.Count > 0)
            {
                dtExtract.Columns.Add("Name");
                dtExtract.Columns.Add("Organisation");
                dtExtract.Columns.Add("Expiry Date");

                for (i = 0; i <= (dt.Rows.Count - 1); i++)
                {
                    dr = dtExtract.NewRow();
                    dr[0] = "\"" + dt.Rows[i][1].ToString().Trim() + "\"";
                    dr[1] = dt.Rows[i][2].ToString().Trim();
                    dr[2] = Convert.ToDateTime(dt.Rows[i][3].ToString().Trim()).ToString("MMM") + " " +
                            Convert.ToDateTime(dt.Rows[i][3].ToString().Trim()).ToString("dd") + " " +
                            Convert.ToDateTime(dt.Rows[i][3].ToString().Trim()).ToString("yyyy");
                    dtExtract.Rows.Add(dr);
                }
                ds.Tables.Add(dtExtract);
                bret = new IRSClassLib.csExcel().ExtractToCSV(ds, fileName, true, out errorMsg);
                writeAudit("FileName=[" + fileName + "] vOrganization=[" + vOrganization + "] DateFrom=[" + DateFrom + "] DateTo=[" + DateTo + "]", "ReturnValue=[" + bret.ToString() + "]", "extractToFile", csAuditLog.Category.Select);
                if (!bret)
                {
                    return errorMsg;
                }
                else
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        bpret = new irsVIP().updatePrinted(getUserID(), dt.Rows[i][0].ToString().Trim());
                        writeAudit("SEQ_ID=[" + dtExtract.Rows[i][0].ToString().Trim() + "] RowCount=[" + dtExtract.Rows.Count + "]", "ReturnValue=[" + bpret.ToString() + "]", "extractToFile", csAuditLog.Category.Update);
                    }
                }
            }
            else
            {
                return "No record(s) found!";
            }
            return fileName;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string generateReport(string ReportName, string DateFrom, string DateTo, string Year, string Quarter)
        {
            DataTable dt = new DataTable();
            string sFileName = string.Empty;
            string sReturn = string.Empty;
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            string userID = new irsVIP().getConnectionStringValue(irsVIP.ConnectionStringField.UserID);
            string password = new irsVIP().getConnectionStringValue(irsVIP.ConnectionStringField.Password);
            string server = new irsVIP().getConnectionStringValue(irsVIP.ConnectionStringField.Server);
            string database = new irsVIP().getConnectionStringValue(irsVIP.ConnectionStringField.Database);

            try
            {
                switch (ReportName)
                {
                    case "Organization with Transactions":
                        Reports.VIP.rptOrganizationWithTransaction rpt1 = new IRSWorld.Reports.VIP.rptOrganizationWithTransaction();
                        dt = new irsVIP().getOrganizationWithTransaction(DateFrom, DateTo);
                        sFileName = @"C:\OrganizationWithTransactions.pdf";

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rpt1.Refresh();
                            rpt1.SetDatabaseLogon(userID, password, server, database);
                            rpt1.SetDataSource(dt);
                            CrDiskFileDestinationOptions.DiskFileName = sFileName;
                            CrExportOptions = rpt1.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            rpt1.Export();
                        }
                        else
                        {
                            sFileName = "No record(s) found!";
                        }
                        break;

                    case "Organization with Transactions by Member":
                        Reports.VIP.rptOrganizationWithTransactionByMembers rpt2 = new IRSWorld.Reports.VIP.rptOrganizationWithTransactionByMembers();
                        dt = new irsVIP().getOrganizationWithTransactionByMembers(DateFrom, DateTo);
                        sFileName = @"C:\OrganizationWithTransactionsByMembers.pdf";

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rpt2.Refresh();
                            rpt2.SetDatabaseLogon(userID, password, server, database);
                            rpt2.SetDataSource(dt);
                            CrDiskFileDestinationOptions.DiskFileName = sFileName;
                            CrExportOptions = rpt2.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            rpt2.Export();
                        }
                        else
                        {
                            sFileName = "No record(s) found!";
                        }
                        break;

                    case "Organization with Zero Remittance From Members":
                        Reports.VIP.rptZeroRemittanceFromMember rpt3 = new IRSWorld.Reports.VIP.rptZeroRemittanceFromMember();
                        dt = new irsVIP().getZeroRemittanceFromMember(DateFrom, DateTo);
                        sFileName = @"C:\ZeroRemittanceFromMember.pdf";

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rpt3.Refresh();
                            rpt3.SetDatabaseLogon(userID, password, server, database);
                            rpt3.SetDataSource(dt);
                            CrDiskFileDestinationOptions.DiskFileName = sFileName;
                            CrExportOptions = rpt3.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            rpt3.Export();
                        }
                        else
                        {
                            sFileName = "No record(s) found!";
                        }
                        break;

                    case "Total Transaction Per Organization":
                        Reports.VIP.rptTransactionPerOrganizationPerQuarter rpt4 = new IRSWorld.Reports.VIP.rptTransactionPerOrganizationPerQuarter();
                        dt = new irsVIP().getTransactionPerOrganizationPerQuarter(Year, Quarter);
                        sFileName = @"C:\TransactionPerOrganization.pdf";

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rpt4.Refresh();
                            rpt4.SetDatabaseLogon(userID, password, server, database);
                            rpt4.SetDataSource(dt);
                            CrDiskFileDestinationOptions.DiskFileName = sFileName;
                            CrExportOptions = rpt4.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            rpt4.Export();
                        }
                        else
                        {
                            sFileName = "No record(s) found!";
                        }
                        break;

                    case "Members Per Organization with Remittance More Than 1":
                        Reports.VIP.rptRemitterPerOrganizationWithTransactionHavingTwoPerQuarter rpt5 = new IRSWorld.Reports.VIP.rptRemitterPerOrganizationWithTransactionHavingTwoPerQuarter();
                        dt = new irsVIP().getRemitterPerOrganizationWithTransactionHavingTwoPerQuarter(Year, Quarter);
                        sFileName = @"C:\MemberPerOrganizationWithMoreThan1.pdf";

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rpt5.Refresh();
                            rpt5.SetDatabaseLogon(userID, password, server, database);
                            rpt5.SetDataSource(dt);
                            CrDiskFileDestinationOptions.DiskFileName = sFileName;
                            CrExportOptions = rpt5.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            rpt5.Export();
                        }
                        else
                        {
                            sFileName = "No record(s) found!";
                        }
                        break;

                    case "Organization with Zero Transactions":
                        Reports.VIP.rptZeroRemittanceFromOrganization rpt6 = new IRSWorld.Reports.VIP.rptZeroRemittanceFromOrganization();
                        dt = new irsVIP().getZeroRemittanceFromOrganization(Year, Quarter);
                        sFileName = @"C:\ZeroRemittanceFromOrganization.pdf";

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            rpt6.Refresh();
                            rpt6.SetDatabaseLogon(userID, password, server, database);
                            rpt6.SetDataSource(dt);
                            CrDiskFileDestinationOptions.DiskFileName = sFileName;
                            CrExportOptions = rpt6.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            rpt6.Export();
                        }
                        else
                        {
                            sFileName = "No record(s) found!";
                        }
                        break;
                }
                sReturn = sFileName;
            }
            catch (Exception ex)
            {
                sReturn = ex.Message;
            }
            return sReturn;
        }
    }
}