using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using IRSClassLib;
using IRSWorld.TranClass;
using System.IO;

namespace IRSWorld.EPSCO
{
    [Serializable]
    public partial class EPSCOEnrollment : System.Web.UI.Page
    {
        private sqlHelperWs sqlkonek = new sqlHelperWs();

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EPSCOEnrollment));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EPSCOTransactions));
            //AjaxPro.Utility.RegisterTypeForAjax(typeof(epsco_commit));
        }

        private irsEPSCOTransactions irsEPSCOTransactions
        {
            get
            {
                if (Session["EPSCOTrans"] == null)
                {
                    Session["EPSCOTrans"] = new irsEPSCOTransactions();
                }
                return (irsEPSCOTransactions)Session["EPSCOTrans"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserRole()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return client_data.Role.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserLoc()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return client_data.Location.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getUserID()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return client_data.UserId.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitterInfo(string CIFID)
        {
            return irsEPSCOTransactions.getRemitterInFo(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public decimal getCharges(int seq_id, int account_currency)
        {
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            string remitting_branch = client_data.Location;

            irsCalculator irs_calc = new irsCalculator(seq_id, account_currency, remitting_branch, true);
            return irs_calc.get_remittance_total_charges();
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBeneDDLSQL(string param)
        {
            return "sp_getBeneDDL~" + param;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneInfo(long benseqid)
        {
            return irsEPSCOTransactions.getBenefInfo(benseqid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneAddress(string BeneCIFID)
        {
            DataTable dt = null;
            Session["EPSCO_Bene_Address"] = irsEPSCOTransactions.getBeneAddress(BeneCIFID);
            dt = (DataTable)Session["EPSCO_Bene_Address"];
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneContact(string BeneCIFID)
        {
            DataTable dt = null;
            Session["EPSCO_Bene_Contact"] = irsEPSCOTransactions.getBeneContact(BeneCIFID);
            dt = (DataTable)Session["EPSCO_Bene_Contact"];
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneEmail(string BeneCIFID)
        {
            DataTable dt = null;
            Session["EPSCO_Bene_Email"] = irsEPSCOTransactions.getBeneEmailAddress(BeneCIFID);
            dt = (DataTable)Session["EPSCO_Bene_Email"];
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneAccounts(string BeneCIFID)
        {
            return irsEPSCOTransactions.getBeneAccounts(BeneCIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBeneAccountDetails(long AcctSeqID)
        {
            return irsEPSCOTransactions.getBeneAccountDetails(AcctSeqID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFInFo(string type)
        {
            DataTable dt = null;
            switch (type)
            {
                case "Address":
                    if (Session["EPSCO_Bene_Address"] != null)
                    {
                        dt = (DataTable)Session["EPSCO_Bene_Address"];
                    }
                    break;
                case "Email":
                    if (Session["EPSCO_Bene_Email"] != null)
                    {
                        dt = (DataTable)Session["EPSCO_Bene_Email"];
                    }
                    break;
                case "Contacts":
                    if (Session["EPSCO_Bene_Contact"] != null)
                    {
                        dt = (DataTable)Session["EPSCO_Bene_Contact"];
                    }
                    break;
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemCurrDDLSQL()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            return "sp_getRemCurrDDL2~" + ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemCountryDDLSQL()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            return "SELECT FldDescription as 'Description', FldValue as 'Code' FROM tbl_lookup WHERE FldGroup = 'country' ORDER BY FldDescription";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBenBranchDDLSQL()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            return "SELECT Distinct BranchName as 'Description', BranchCode as 'Code' FROM tbl_branches order by BranchName";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getExchangeRates(int RemCCY, int TarCCY)
        {
            DataTable dt = null;
            DataTable dt2 = null;
            DataRow dr = null;
            try
            {
                dt2 = irsEPSCOTransactions.getExchangeRates(((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
                dt = new DataTable();
                dt = dt2.Clone();
                if (dt2 != null)
                {
                    dr = dt2.Select("SourceCurrency=" + RemCCY + " AND TargetCurrency=" + TarCCY, "DateInserted DESC")[0];
                    //dt.Rows.Add(dr);
                    dt.ImportRow(dr);
                }
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "getExchangeRates", "", ex.Message, csAuditLog.Category.Select);
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBankChargeID(string CIFID)
        {
            return irsEPSCOTransactions.getBankChargeID(CIFID, ((irsClientData)Session["IRS_CLIENT_DATA"]).Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDDLCountry(string SQL)
        {
            return irsEPSCOTransactions.getDDLCountry(SQL);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string enroll_Client(string record, string method)
        {
            if (irsEPSCOTransactions.enroll_Client(record, method) > 0)
            {
                if (method == "ADD")
                { return "Client successfully enrolled."; }
                else
                { return "Client successfully updated."; }
            }
            else
            {
                if (method == "ADD")
                { return "Error enrolling client."; }
                else
                { return "Error updating client."; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string delete_Client(int rem_seqid, int ben_seqid)
        {
            if (irsEPSCOTransactions.delete_Client(rem_seqid, ben_seqid))
            {
                return "Client successfully deleted.";
            }
            else
            {
                return "Error deleting client.";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string app_generate_RefNo(int ben_seqid)
        {
            if (irsEPSCOTransactions.generate_RefNo(ben_seqid) == 0)
            {
                return "ERROR GENERATING CARD DETAILS";
            }
            else
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int getEnrollStatus(int rem_seqid, int ben_seqid)
        {
            sqlHelperWs sql = new sqlHelperWs();

            string script = "SELECT CASE WHEN EPSCORefNo IS NULL THEN 2 ELSE 1 END " +
                            "FROM tbl_cmcs_enrollment WHERE " +
                            "SEQ_ID in(SELECT EPSCO_SEQID from tbl_cmcs_ids WHERE " +
                            "Rem_SEQID = " + rem_seqid.ToString() + " AND Ben_SEQID = " + ben_seqid.ToString() + ")";

            return Convert.ToInt32(sql.execScalar(script));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getZipCodeDetail(int seqid, string truezipcode)
        {
            sqlHelperWs sql = new sqlHelperWs();

            string script = "";

            if (seqid == 0)
            {
                script = "SELECT municipality + ', ' + province + ' ' + truezipcode + '~' + truezipcode FROM TBL_ZIPCODES WHERE truezipcode = " + truezipcode;
            }
            else
            {
                script = "SELECT municipality + ', ' + province + ' ' + truezipcode + '~' + truezipcode FROM TBL_ZIPCODES WHERE SEQ_ID = " + seqid.ToString();
            }

            return Convert.ToString(sql.execScalar(script));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getEnrolleeDetails(int ben_seqid)
        {
            return irsEPSCOTransactions.getEnrolleeDetails(ben_seqid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getEPSCORefNo(int ben_seqid)
        {
            return irsEPSCOTransactions.getEPSCORefNo(ben_seqid);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFDetails(int SEQ_ID, int CIFClass, int Ben_EPSCO_SEQID)
        {
            return irsEPSCOTransactions.getCIFDetails(SEQ_ID, CIFClass, Ben_EPSCO_SEQID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string check_BeneAcctNo(string AcctNo, string method)
        {
            return irsEPSCOTransactions.check_BeneAcctNo(AcctNo) + "~" + method;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getMatchedCIF(string CIF, string CIFClass, string method)
        {
            int result = irsEPSCOTransactions.getMatchedCIF(CIF);
            return result.ToString() + '~' + CIFClass + '~' + method;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getAcctBranchCode(string acctno)
        {
            irsClientAccounts irsacct = new irsClientAccounts();
            string[] s = irsacct.get_acctdetails(acctno).Split('~');
            int acctstat = Convert.ToInt32(s[2]);

            if (acctstat == 1 || acctstat == 5 || acctstat == 7 || acctstat == 10 || acctstat == 11 || acctstat == 12 || acctstat == 18 || acctstat == 19)
            {
                return "Closed and Dormant accounts is not allowed!";
            }

            return s[3];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDataTableValue(string param)
        {
            return irsEPSCOTransactions.getDataTable(param);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getEnrollingBranchDDLSQL()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            return "SELECT 	BranchName as 'Description', " +
                    "           BranchCode as 'Code' " +
                    "FROM 	    tbl_branches " +
                    "WHERE 	    convert(int,case when isnumeric(branchcode) = 0 THEN '0' ELSE branchcode end) >= 730 and " +
                    "           convert(int,case when isnumeric(branchcode) = 0 THEN '0' ELSE branchcode end) < 740 ORDER BY  BranchCode";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getMandatoryFields(int CIFClass)
        {
            sqlHelperWs sql = new sqlHelperWs();

            string script = "SELECT     b.CIFField " +
                            "FROM       tbl_cif_mandatory_fields a " +
                            "INNER JOIN tbl_cif_module_fields b " +
                            "ON         a.ciffieldsseqid =b.seq_id " +
                            "WHERE      a.branchcode = '" + getUserLoc() + "' and a.cifclass = " + CIFClass.ToString();

            DataSet ds = sql.getDataSet(script, false, new object[] { });

            string result = "";

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                result += dr.ItemArray[0].ToString() + "~";
            }

            return result;
        }

        protected void invoke_obj_data_Click(object sender, EventArgs e)
        {
            exportToCSV();
        }

        public void exportToCSV()
        {
            sqlHelperWs sql = new sqlHelperWs();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=EPSCO" + DateTime.Now.ToString("MMddyyhhmmss") + ".csv");
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");

            HttpContext.Current.Response.Write("Product Card No.,ID No.,Remitter Fullname,Beneficiary Fullname,Beneficiary Account No.,Enrolling Branch,Mobile No.,Barcode No.");
            HttpContext.Current.Response.Write(Environment.NewLine);

            string[] obj_data = h_obj_data.Value.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
            
            int i = 1;
            foreach (string data in obj_data)
            {
                
                try
                {
                    sql.execScalar("UPDATE tbl_cmcs_enrollment SET isPrinted = 1 WHERE SEQ_ID = '" + data.Substring(0, data.IndexOf(",")) + "'");
                    string[] cols = data.Split(new char[] { ',' });
                    string acctno = cols[4].Substring(0,5) + "*****" + cols[4].Substring(cols[4].Length - 2);
                    HttpContext.Current.Response.Write(cols[1] + "," + i.ToString() + "," + cols[2] + "," + cols[3] + "," + acctno + "," + cols[5] + "," + cols[7] + "," + cols[6].Trim());
                    HttpContext.Current.Response.Write(Environment.NewLine);
                    i++;
                }
                catch { }
            }

            HttpContext.Current.Response.End();
        }

        protected void invoke_Export_All_Click(object sender, EventArgs e)
        {
            exportToCSV_all();
        }

        public void exportToCSV_all()
        {
            sqlHelperWs sql = new sqlHelperWs();

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=EPSCO" + DateTime.Now.ToString("MMddyyhhmmss") + ".csv");
            HttpContext.Current.Response.ContentType = "text/csv";
            HttpContext.Current.Response.AddHeader("Pragma", "public");

            HttpContext.Current.Response.Write("Product Card No.,ID No.,Remitter Fullname,Beneficiary Fullname,Beneficiary Account No.,Enrolling Branch,Mobile No.,Barcode No.");
            HttpContext.Current.Response.Write(Environment.NewLine);

            string[] obj_data = h_Export_all.Value.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

            int i = 1;
            foreach (string data in obj_data)
            {

                try
                {
                    string[] cols = data.Split(new char[] { ',' });

                    sql.execScalar("UPDATE tbl_cmcs_enrollment SET isPrinted = 1 WHERE SEQ_ID = '" + cols[5] + "'");
                    string acctno = cols[3].Substring(0, 5) + "*****" + cols[3].Substring(cols[3].Length - 2);

                    HttpContext.Current.Response.Write(cols[0] + "," + i.ToString() + "," + cols[1] + "," + cols[2] + "," + acctno + "," + cols[4] + "," + cols[7] + ","  + cols[6].Trim());
                    HttpContext.Current.Response.Write(Environment.NewLine);
                    i++;
                }
                catch { }
            }
             
            HttpContext.Current.Response.End();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();
                DataSet ds = null;
                if (SQL.IndexOf("SELECT") == -1)
                {
                    string[] param = SQL.Split('~');
                    string sp_name = param[0];
                    string sp_value = null;
                    object[] array_value = null;

                    if (param.Length - 1 != 1)
                    {
                        for (int i = 0; i < param.Length - 1; i++)
                        {

                            sp_value += param[i + 1] + "~";
                        }

                        array_value = sp_value.Split('~');
                    }
                    else
                    {
                        array_value = param[1].Split('~');
                    }
                    ds = com.execStoredProcedure(sp_name, array_value);
                }
                else
                {
                    ds = com.getDataSet(com.getSqlCommand(SQL, false), "FileInd");
                }
                return ds.Tables[0];
            }
            catch (Exception e)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", e.Message, e.StackTrace, e.Source, csAuditLog.Category.Select);
                return null;
                //throw new Exception("Failed to get datatable: " + e.Message);
            }
        }
    }
}
