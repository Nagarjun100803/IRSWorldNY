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

namespace IRSWorld.EPSCO
{
    [Serializable]
    public partial class EPSCOTransactions : System.Web.UI.Page
    {
        private sqlHelperWs sqlkonek = new sqlHelperWs();

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(EPSCOTransactions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(epsco_commit));

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
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
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
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "OBPS", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            return client_data.Location.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRemitterInfo(string CIFID)
        {
            return irsEPSCOTransactions.getRemitterInFo(CIFID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public decimal getCharges(int seq_id,int account_currency)
        {
            irsClientData client_data = (irsClientData)Session["IRS_CLIENT_DATA"];
            string remitting_branch = client_data.Location;

            irsCalculator irs_calc = new irsCalculator(seq_id,account_currency,remitting_branch,true);
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
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            return "sp_getRemCurrDDL2~" + ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRemCountryDDLSQL()
        {
            if (Session["IRS_CLIENT_DATA"] == null)
            {
                new csAuditLog().writeAudit(((irsClientData)Session["IRS_CLIENT_DATA"]).UserId, "EPSCO", "IRS_CLIENT_DATA session is empty.", "", "", csAuditLog.Category.Select);
                return "";
            }
            return "SELECT FldDescription as 'Description',FldValue as 'Code' FROM tbl_lookup WHERE FldGroup = 'country' ORDER BY FldDescription";
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
                { return "Client enrolled."; }
                else
                { return "Client updated."; }
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
        public string getEPSCORefNo(string lname, string fname, string bday)
        {
            return irsEPSCOTransactions.getEPSCORefNo(0);
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCIFDetails(int SEQ_ID, int CIFClass)
        {
            return irsEPSCOTransactions.getCIFDetails(SEQ_ID, CIFClass,0);
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


    [Serializable]
    public class epsco_commit : sqlHelperWs
    {
        private string error_message = string.Empty;
        private bool edit_mode = false;
        
        private DateTime remittance_transaction_date;


        public epsco_commit()
        {
            this.error_message = string.Empty; base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName      = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";

            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE SEQ_ID = " + id, false); base.getUpdateDataSet(command);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_epsco_error_message()
        {
            if (System.Web.HttpContext.Current.Session["EPSCO_ERROR_MESSAGE"] != null)
            {
                return (string)System.Web.HttpContext.Current.Session["EPSCO_ERROR_MESSAGE"];
            }

            if (this.error_message != null && this.error_message.Trim().Length > 0)
            {
                return this.error_message;
            }

            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_record(string column_values)
        {
            try
            {
                this.edit_mode = false;

                bool ibool = false; string[] s = column_values.Split('|');

                if (base.newRow() == true)
                {
                    ibool = this.set_record_and_commit(s, false);
                }

                if (ibool == false)
                {
                    base.cancel();
                }
                else
                {
                    ibool = base.save();

                    if (ibool == true)
                    {
                        System.Web.HttpContext.Current.Session["EPSCO_SEQUENCE_ID"] = base.IdentityValue;
                    }
                }

                if (ibool == true)
                {
                    return "0~" + base.IdentityValue.ToString();
                }

                return "1~" + this.get_epsco_error_message();
            }
            catch(Exception e)
            {
                return "1~" + e.Message;
            }
        }

        private bool set_record_and_commit(string[] column_values, bool record_is_edit)
        {
            this.error_message = string.Empty;

            ArrayList error_list = new ArrayList();            

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                switch (s[0].Trim().ToUpper())
                {
                    case "SEQ_ID": continue;
                    case "TRANXDATE": continue;
                    case "REFERENCENO": continue;
                    case "USERID": continue;
                    case "REMITTINGBRANCH": continue;
                    case "CHARGELOCALLY":

                        if (s[1] == "0" || s[1] == "" || s[1].Length == 0 || s[1].ToUpper() == "FALSE")
                        {
                            s[1] = "false";
                        }
                        else
                        {
                            s[1] = "true";
                        }

                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }

                        continue;
                }

                if (record_is_edit == false)
                {
                    if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                    {
                        irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];

                        this.remittance_transaction_date = new irsInternetPage().getTransactionDate(loggin_data.Location, true);

                        if (!base.setColumn("TranxDate", this.remittance_transaction_date))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                        if (!base.setColumn("RemittingBranch", loggin_data.Location))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                        if (!base.setColumn("UserID", loggin_data.UserId))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                    }
                }

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty || s[1].ToString().Trim().ToUpper() == "UNDEFINED")
                    {
                        s[1] = null;
                    }
                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }

                if (error_list.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (Object errors in error_list)
                    {
                        sb.Append((String)errors + "\n");
                    }

                    this.error_message = string.Empty;
                    this.error_message = sb.ToString();


                    System.Web.HttpContext.Current.Session["EPSCO_ERROR_MESSAGE"] = this.error_message;

                    return false;
                }
                else
                {
                    return true;
                }
            }

            this.error_message = "database column and field value not set";

            System.Web.HttpContext.Current.Session["EPSCO_ERROR_MESSAGE"] = this.error_message; 
            
            return false;
        }
    }
}
