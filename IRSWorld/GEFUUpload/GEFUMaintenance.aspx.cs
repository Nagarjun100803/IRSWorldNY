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
using IRSClassLib;
using System.IO;
using System.Text;

namespace IRSWorld.GEFUUpload
{
    [Serializable]
    public partial class GEFUMaintenance : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        private string[] strglcodes;
        StreamWriter objwriter;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GEFUMaintenance));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.select_do_instruction("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string SEQ_ID)
        {

            Session["TABLE_SEQ_ID"] = SEQ_ID;
            return this.select_do_instruction("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.select_do_instruction("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.select_do_instruction("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string _seq_id)
        {
            Session["TABLE_SEQ_ID"] = _seq_id;
            return this.select_do_instruction("DELETE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["GEFU_MAINTENANCE_MESSAGE"] != null)
            {
                return (string)Session["GEFU_MAINTENANCE_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["GEFU_MAINTENANCE_MESSAGE"] = null;
            Session["GEFU_MAINTENANCE_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string GEFU_MAINTENANCE_SEQ_ID()
        {
            if (Session["GEFU_MAINTENANCE_SEQ_ID"] != null)
            {
                return (string)Session["GEFU_MAINTENANCE_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_ACCOUNTINGENTRIES_RECORD(string accountingentries_seq_id)
        {
            irsAccountingEntries accountingentries = this.accountingentries_Maintenance();
            try
            {
                if (accountingentries != null)
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
                Session["ACCOUNTINGENTRIES_MAINTENANCE"] = accountingentries;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_IFLEXACCOUNTS_RECORD(string iflexaccounts_seq_id)
        {
            irsIflexAccounts iflexaccounts = this.iflexaccounts_Maintenance();
            try
            {
                if (iflexaccounts != null)
                {
                    if (iflexaccounts.get_record(iflexaccounts_seq_id) == true)
                    {
                        return iflexaccounts.getDataRecord();
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
                Session["IFLEXACCOUNTS_MAINTENANCE"] = iflexaccounts;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_BOOKINGRATES_RECORD(string bookingrates_seq_id)
        {
            irsBookingRates bookingrates = this.bookingrates_Maintenance();
            try
            {
                if (bookingrates != null)
                {
                    if (bookingrates.get_record(bookingrates_seq_id) == true)
                    {
                        return bookingrates.getDataRecord();
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
                Session["BOOKINGRATES_MAINTENANCE"] = bookingrates;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string IFLEXACCOUNTS_SEQ_ID()
        {
            if (Session["IFLEXACCOUNTS_SEQ_ID"] != null)
            {
                return (string)Session["IFLEXACCOUNTS_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_ACCOUNTINGENTRIES_RECORD(string column_values)
        {
            string User = "";
            User = IRS_CLIENT_DATA().UserId;
            column_values = column_values.Replace("UUUIII", User);

            irsAccountingEntries irsAccountingEntries = this.accountingentries_Maintenance();
            string[] s = column_values.Split('$');
            strglcodes = s;

            if (s != null && s.Length > 0)
            {
                if (irsAccountingEntries.set_record(s) == false)
                {
                    this.set_message(irsAccountingEntries.messages); return false;
                }
                Session["ACCOUNTINGENTRIES_MAINTENANCE"] = irsAccountingEntries;
                return true;
            }
            irsAccountingEntries = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_ACCOUNTINGENTRIES_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_IFLEXACCOUNTS_RECORD(string column_values)
        {
            irsIflexAccounts irsIflexAccounts = this.iflexaccounts_Maintenance();
            string[] s = column_values.Split('$');

            if (s != null && s.Length > 0)
            {
                if (irsIflexAccounts.set_record(s) == false)
                {
                    this.set_message(irsIflexAccounts.messages); return false;
                }
                Session["IFLEXACCOUNTS_MAINTENANCE"] = irsIflexAccounts;
                return true;
            }
            irsIflexAccounts = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_IFLEXACCOUNTS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_BOOKINGRATES_RECORD(string column_values)
        {
            string Location = "";
            string User = "";
            Location = IRS_CLIENT_DATA().Location;
            User = IRS_CLIENT_DATA().UserId;

            column_values = column_values.Replace("BBBCCC", Location);
            column_values = column_values.Replace("UUUIII", User);

            irsBookingRates irsBookingRates = this.bookingrates_Maintenance();
            string[] s = column_values.Split('$');

            if (s != null && s.Length > 0)
            {
                if (irsBookingRates.set_record(s) == false)
                {
                    this.set_message(irsBookingRates.messages); return false;
                }
                Session["BOOKINGRATES_MAINTENANCE"] = irsBookingRates;
                return true;
            }

            irsBookingRates = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_BOOKINGRATES_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_BOOKINGRATES_RECORD_AGAIN(string column_values)
        {
            string Location = "";
            string User = "";
            Location = IRS_CLIENT_DATA().Location;
            User = IRS_CLIENT_DATA().UserId;

            column_values = column_values.Replace("BBBCCC", Location);
            column_values = column_values.Replace("UUUIII", User);

            irsBookingRates irsBookingRates = this.bookingrates_Maintenance();
            string[] s = column_values.Split('$');
            string[] a = s[2].Split('~');
            string[] b = s[3].Split('~');

            if (a[1] == b[1])
            {
                return false;
            }
            if (s != null && s.Length > 0)
            {
                if (irsBookingRates.set_record_again(s) == false)
                {
                    this.set_message(irsBookingRates.messages); return false;
                }
                Session["BOOKINGRATES_MAINTENANCE"] = irsBookingRates;
                return true;
            }

            irsBookingRates = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_BOOKINGRATES_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ActiveModule(string DivName)
        {
            Session["DIV_NAME"] = DivName;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getBookingRatesFilter(string strbranchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("select " +
                                                            "distinct convert(varchar, DateInserted,1) as datefilter " +
                                                        "from tbl_bookingrates " +
                                                        "where branchcode ='" + strbranchcode + "' " +
                                                        "order by convert(varchar, DateInserted,1)", false);

                    return commons.getDataSet(sqlcommands, "GridFilter");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCode()
        {
            return IRS_CLIENT_DATA().Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRole()
        {
            return Convert.ToString(IRS_CLIENT_DATA().Role);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getnegbranchcode(string BranchCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    if (BranchCode != "1074")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT DISTINCT negbranchcode " +
                                                            "FROM tbl_branches " +
                                                            "WHERE  " +
                                                            " negbranchcode is not null " +
                                                            "AND BranchCode = '" + BranchCode + "' " +
                                                            "ORDER BY negbranchcode", false);
                    }
                    else
                    {
                        sqlcommands = commons.getSqlCommand("SELECT DISTINCT negbranchcode " +
                                                            "FROM tbl_branches " +
                                                            "WHERE branchtype = 3 " +
                                                            "AND LEN(ISNULL(negbranchcode, '')) = 4 " +
                                                            "ORDER BY negbranchcode", false);
                    }

                    return commons.getDataSet(sqlcommands, "negbranchcode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getlocationcode(string BranchCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    if (BranchCode != "1074")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT branchname, branchcode  " +
                                                            "FROM tbl_branches " +
                                                            "WHERE branchtype = 3 " +
                                                            "AND BranchCode = '" + BranchCode + "' " +
                                                            "ORDER BY branchname", false);
                    }
                    else
                    {
                        sqlcommands = commons.getSqlCommand("SELECT branchname, branchcode  " +
                                                            "FROM tbl_branches " +
                                                            "WHERE branchtype = 3 " +
                                                            "ORDER BY branchname", false);
                    }

                    return commons.getDataSet(sqlcommands, "LocationCode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getlocationcode_negbranchcode(string negbranchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT branchname, branchcode  " +
                                                        "FROM tbl_branches " +
                                                        "WHERE  " +
                                                        "AND negbranchcode= '" + negbranchcode + "' " +
                                                        "ORDER BY branchname", false);
                    return commons.getDataSet(sqlcommands, "LocationCode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTranxSource()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldDescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'tranxsource'", false);
                    return commons.getDataSet(sqlcommands, "tranxsource");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTranxSource_sublookup(string branchcode, string tranxdate)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {

                    sqlcommands = commons.getSqlCommand("SELECT distinct look.fldvalue, look.FldDescription " +
                                                        "FROM tbl_sub_lookup sub " +
                                                        "INNER JOIN tbl_lookup look ON look.fldvalue = sub.fldvalue " +
                                                        "AND look.fldgroup = sub.FldGroup " +
                                                        "INNER JOIN tbl_remittance rem ON rem.TransactionSource = look.fldvalue " +
                                                        "AND CONVERT(VARCHAR(10), tranxdate, 101)= '" + tranxdate + "' " +
                                                        "INNER JOIN tbl_gefu_extracted_transactions gef ON gef.rem_seq_id = rem.seq_id " +
                                                        "WHERE sub.branchcode = '" + branchcode + "' " +
                                                        "AND look.fldgroup = 'TranxSource' " +
                                                        "ORDER BY look.flddescription", false);

                    return commons.getDataSet(sqlcommands, "tranxsource");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTranxSource_sublookup_accnt(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    if (branchcode != "1074")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT " +
                                                                    "look.fldvalue, " +
                                                                    "look.FldDescription " +
                                                            "FROM tbl_sub_lookup sub " +
                                                            "INNER JOIN tbl_lookup look ON look.fldvalue = sub.fldvalue " +
                                                                                        "AND look.fldgroup = sub.FldGroup " +
                                                            "WHERE sub.branchcode = '" + branchcode + "' " +
                                                            "AND look.fldgroup = 'TranxSource' " +
                                                            "ORDER BY look.flddescription", false);

                    }
                    else
                    {
                        sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                            "FldDescription " +
                                                            "FROM tbl_lookup " +
                                                            "WHERE fldgroup = 'tranxsource'", false);
                    }

                    return commons.getDataSet(sqlcommands, "tranxsource");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getReceiptMode_sublookup(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    if (branchcode != "1074")
                    {

                        sqlcommands = commons.getSqlCommand("SELECT " +
                                                                    "look.fldvalue, " +
                                                                    "look.FldDescription " +
                                                            "FROM tbl_sub_lookup sub " +
                                                            "INNER JOIN tbl_lookup look ON look.fldvalue = sub.fldvalue " +
                                                                                        "AND look.fldgroup = sub.FldGroup " +
                                                            "WHERE sub.branchcode = '" + branchcode + "' " +
                                                            "AND look.fldgroup = 'receiptmode' " +
                                                            "ORDER BY look.flddescription", false);

                    }
                    else
                    {
                        sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                            "FldDescription " +
                                                            "FROM tbl_lookup " +
                                                            "WHERE fldgroup = 'receiptmode'", false);
                    }

                    return commons.getDataSet(sqlcommands, "receiptmode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getReceiptMode()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldDescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'receiptmode'", false);
                    return commons.getDataSet(sqlcommands, "receiptmode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccNegbranchCode()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT DISTINCT negbranchcode " +
                                                        "FROM tbl_accountingentries ", false);
                    return commons.getDataSet(sqlcommands, "negbranchcode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccLocationCode()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT DISTINCT locationcode " +
                                                        "FROM tbl_accountingentries ", false);
                    return commons.getDataSet(sqlcommands, "locationcode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccTranxSource(string strLocationCode, string strNegBrachCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                            "DISTINCT tsrc.FldValue, tsrc.FldDescription " +
                                                        "FROM tbl_accountingentries	acc " +
                                                        "INNER JOIN TBL_LOOKUP		tsrc	ON	    acc.tranxsource = tsrc.fldvalue " +
                                                                                            "AND	tsrc.fldgroup	= 'tranxsource' " +
                                                        "WHERE acc.locationcode = '" + strLocationCode + "' " +
                                                        "AND   acc.negbranchcode = '" + strNegBrachCode + "' " +
                                                        "ORDER BY tsrc.FldDescription", false);
                    return commons.getDataSet(sqlcommands, "AccTranxSource");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccReceiptMode(string strLocationCode, string strNegBrachCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                            "DISTINCT tsrc.FldValue, tsrc.FldDescription " +
                                                        "FROM tbl_accountingentries	acc " +
                                                        "INNER JOIN TBL_LOOKUP		tsrc	ON	    acc.ReceiptMode = tsrc.fldvalue " +
                                                                                            "AND	tsrc.fldgroup	= 'receiptmode' " +
                                                        "WHERE acc.locationcode = '" + strLocationCode + "' " +
                                                        "AND   acc.negbranchcode = '" + strNegBrachCode + "' " +
                                                        "ORDER BY tsrc.FldDescription", false);
                    return commons.getDataSet(sqlcommands, "AccReceiptMode");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccSourceCcy(string strLocationCode, string strNegBrachCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                            "DISTINCT tsrc.FldValue, tsrc.FldDescription " +
                                                        "FROM tbl_accountingentries	acc " +
                                                        "INNER JOIN TBL_LOOKUP		tsrc	ON	    acc.TranxCurrency   = tsrc.fldvalue " +
                                                                                            "AND	tsrc.fldgroup	    = 'currency' " +
                                                        "WHERE acc.locationcode = '" + strLocationCode + "' " +
                                                        "AND   acc.negbranchcode = '" + strNegBrachCode + "' " +
                                                        "ORDER BY tsrc.FldDescription", false);
                    return commons.getDataSet(sqlcommands, "AccSourceCcy");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccTargetCcy(string strLocationCode, string strNegBrachCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                            "DISTINCT tsrc.FldValue, tsrc.FldDescription " +
                                                        "FROM tbl_accountingentries	acc " +
                                                        "INNER JOIN TBL_LOOKUP		tsrc	ON	    acc.ImplementationCurrency  = tsrc.fldvalue " +
                                                                                            "AND	tsrc.fldgroup	            = 'currency' " +
                                                        "WHERE acc.locationcode = '" + strLocationCode + "' " +
                                                        "AND   acc.negbranchcode = '" + strNegBrachCode + "' " +
                                                        "ORDER BY tsrc.FldDescription", false);
                    return commons.getDataSet(sqlcommands, "AccTargetCcy");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_seq_id()
        {
            return this.TABLE_SEQ_ID();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTranxCurrency_sublookup(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT b.FldDescription, b.fldvalue " +
                                                        "FROM tbl_currency_categories a " +
                                                        "INNER JOIN tbl_lookup b on a.currency = b.fldvalue where fldgroup = 'currency' " +
                                                        "AND a.branchcode = '" + branchcode + "' " +
                                                        "AND a.checked = 1 " +
                                                        "AND a.currencygroup = 1 " +
                                                        "ORDER BY b.fldcode ", false);
                    return commons.getDataSet(sqlcommands, "tranxcurrency");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTranxCurrency()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldDescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'currency'", false);
                    return commons.getDataSet(sqlcommands, "tranxcurrency");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet search_specific_reports(string location, string reportname)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                            "DISTINCT [filename], " +
                                                            "filepath " +
                                                        "from tbl_gefu_reports_path " +
                                                        "where branchcode = '" + location + "'" +
                                                        "and [filename] = '" + reportname + "' ", false);
                    return commons.getDataSet(sqlcommands, "reportresult");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet searchreports(string location, string tranxdate)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;
            string[] filesname = null;
            string filesession = "";

            try
            {
                if (commons != null)
                {
                    if (Session["GEFUFILES"] != null)
                    {
                        filesname = Session["GEFUFILES"].ToString().Split(';');
                    }
                    if (filesname.Length != 0)
                    {
                        for (int i = 0; filesname.Length - 1 > i; i++)
                        {
                            if (i == 0)
                            {
                                filesession += "'" + filesname[i] + "'";
                            }
                            else
                            {
                                filesession += ",'" + filesname[i] + "'";
                            }
                        }
                    }
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                            "DISTINCT [filename], " +
                                                            "filepath " +
                                                        "from tbl_gefu_reports_path " +
                                                        "where branchcode = '" + location + "'" +
                                                        "and folderdate = '" + tranxdate + "'" +
                                                        "and [filename] in (" + filesession + ")", false);
                    return commons.getDataSet(sqlcommands, "reportsresults");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getImplementationCurrency()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                       "FldDescription " +
                                                       "FROM tbl_lookup " +
                                                       "WHERE fldgroup = 'currency'", false);
                    return commons.getDataSet(sqlcommands, "implementationccy");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getImplementationCurrency_sublookup(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT b.FldDescription, b.fldvalue " +
                                                         "FROM tbl_currency_categories a " +
                                                         "INNER JOIN tbl_lookup b on a.currency = b.fldvalue where fldgroup = 'currency' " +
                                                         "AND a.branchcode = '" + branchcode + "' " +
                                                         "AND a.checked = 1 " +
                                                         "AND a.currencygroup = 2 " +
                                                         "ORDER BY b.fldcode ", false);
                    return commons.getDataSet(sqlcommands, "implementationccy");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getSourceCcy(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {

                    if (branchcode != "1074")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT " +
                                                                    "DISTINCT ccy.currency as fldvalue, " +
                                                                    "look.FldDescription " +
                                                            "FROM       tbl_currency_categories ccy " +
                                                            "INNER JOIN tbl_lookup			    look	ON ccy.currency = look.fldvalue " +
                                                            "WHERE      branchcode          = '" + branchcode + "' " +
                                                                "AND    look.fldgroup       = 'currency' " +
                                                                "AND    ccy.checked         = 1 " +
                                                                "AND    ccy.currencygroup   = 1 " +
                                                            "ORDER BY look.FldDescription", false);

                    }
                    else
                    {
                        sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                            "FldDescription " +
                                                            "FROM tbl_lookup " +
                                                            "WHERE fldgroup = 'currency'", false);
                    }

                    return commons.getDataSet(sqlcommands, "SourceCcy");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getTargetCcy(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    if (branchcode != "1074")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT " +
                                                                    "DISTINCT ccy.currency as fldvalue, " +
                                                                    "look.FldDescription " +
                                                            "FROM       tbl_currency_categories ccy " +
                                                            "INNER JOIN tbl_lookup			    look	ON ccy.currency = look.fldvalue " +
                                                            "WHERE      branchcode          = '" + branchcode + "' " +
                                                                "AND    look.fldgroup       = 'currency' " +
                                                                "AND    ccy.checked         = 1 " +
                                                                "AND    ccy.currencygroup   = 2 " +
                                                            "ORDER BY look.FldDescription", false);

                    }
                    else
                    {
                        sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                            "FldDescription " +
                                                            "FROM tbl_lookup " +
                                                            "WHERE fldgroup = 'currency'", false);
                    }

                    return commons.getDataSet(sqlcommands, "TargetCcy");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Validate_Bookingrates(string tranxdate, string locationcode)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset;

            sqlcommands = commons.getSqlCommand("EXEC sp_bookingrates '" + tranxdate + "', '" + locationcode + "'", false);
            myDataset = commons.getDataSet(sqlcommands, "bookingrates");

            if (myDataset.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Consolidated_Report_Online_NonOnline(string tranxdate, string locationcode, string tranxstatus)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();

            string strCOSO = "";
            string strHeader = "";
            //string strCOSOBody = "";


            string[] tempArraybankcom;
            string[] tempArrayLocalCable;
            string[] tempArrayLocalServiceFee;
            string[] tempArrayOPE;
            string[] tempArrayDocStamps;
            string tempArrayValue;
            string strCcy;
            string[] strCcyNo;
            string[,] strbankcom;
            string[,] strLocalCable;
            string[,] strLocalServiceFee;
            string[,] strOPE;
            string[,] strDocStamps;

            string[,] strbankcompertranx;
            string[] tempArrayDebitAmountCcy;
            string[,] strDebitAmount;
            string[,] strNetDebitAmount;
            decimal decCreditAmount = 0;
            decimal decDebitAmountBase = 0;
            decimal decCreditAmountBase = 0;
            int intTotalItemsPerTranxsource = 0;


            string[,] strbankcompertranxPerRemCcy;
            string[,] strDebitAmountPerRemCcy;
            string[,] strNetDebitAmountPerRemCcy;
            decimal decCreditAmountPerRemCcy = 0;
            decimal decDebitAmountBasePerRemCcy = 0;
            decimal decCreditAmountBasePerRemCcy = 0;
            int intTotalItemsPerTranxsourcePerRemCcy = 0;
            string mainpath = config.appsetting("ReportExportPath");

            tranxstatus = "'2, 273, 99, 5'";//'2,17,5,16,99'";
            strHeader = "Credited Online and Processed by GRS";
            strCOSO = "COPG";

            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            myDataset = commons.getDataSet(sqlcommands, "consolidated_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            tempArrayDebitAmountCcy = new string[myDataset.Tables[0].Rows.Count];
            tempArraybankcom = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalCable = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalServiceFee = new string[myDataset.Tables[0].Rows.Count];
            tempArrayOPE = new string[myDataset.Tables[0].Rows.Count];
            tempArrayDocStamps = new string[myDataset.Tables[0].Rows.Count];

            int intDebitAmountCcy = 0;
            int intbankcomindex = 0;
            int intLocalCableindex = 0;
            int intLocalServiceFeeindex = 0;
            int intOPEindex = 0;
            int intDocStampsindex = 0;
            decimal decTotalSuspenseAmount = 0;
            decimal decTotalSuspenseAmountCO = 0;
            decimal decTotalSuspenseAmountSO = 0;

            decimal decBaseTotalCreditAmount = 0;
            decimal decBaseTotalDebitAmount = 0;

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "1 for gross")
                {
                    tempArrayDebitAmountCcy.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intDebitAmountCcy);
                    intDebitAmountCcy += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "3 for Bank Com")
                {
                    tempArraybankcom.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intbankcomindex);
                    intbankcomindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "4 for Local Cable")
                {
                    tempArrayLocalCable.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalCableindex);
                    intLocalCableindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "5 for Local Service Fee")
                {
                    tempArrayLocalServiceFee.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalServiceFeeindex);
                    intLocalServiceFeeindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "6 for OPE")
                {
                    tempArrayOPE.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intOPEindex);
                    intOPEindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "7 for Doc Stamps")
                {
                    tempArrayDocStamps.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intDocStampsindex);
                    intDocStampsindex += 1;
                }
            }

            Array.Sort(tempArrayDebitAmountCcy);
            Array.Sort(tempArraybankcom);
            Array.Sort(tempArrayLocalCable);
            Array.Sort(tempArrayLocalServiceFee);
            Array.Sort(tempArrayOPE);
            Array.Sort(tempArrayDocStamps);

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArraybankcom.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArraybankcom[i]) && tempArraybankcom[i] != "" && tempArraybankcom[i] != null)
                {
                    strCcy += tempArraybankcom[i] + "~";
                }
                tempArrayValue = tempArraybankcom[i];
            }

            strCcyNo = strCcy.Split('~');
            strbankcom = new string[strCcyNo.Length, 2];
            strbankcompertranx = new string[strCcyNo.Length, 2];
            strbankcompertranxPerRemCcy = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strbankcom.SetValue(strCcyNo[i], i + 1, 0);
                strbankcompertranx.SetValue(strCcyNo[i], i + 1, 0);
                strbankcompertranxPerRemCcy.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalCable.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalCable[i]) && tempArrayLocalCable[i] != "" && tempArrayLocalCable[i] != null)
                {
                    strCcy += tempArrayLocalCable[i] + "~";
                }
                tempArrayValue = tempArrayLocalCable[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalCable = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalCable.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalServiceFee.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalServiceFee[i]) && tempArrayLocalServiceFee[i] != "" && tempArrayLocalServiceFee[i] != null)
                {
                    strCcy += tempArrayLocalServiceFee[i] + "~";
                }
                tempArrayValue = tempArrayLocalServiceFee[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalServiceFee = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalServiceFee.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayOPE.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayOPE[i]) && tempArrayOPE[i] != "" && tempArrayOPE[i] != null)
                {
                    strCcy += tempArrayOPE[i] + "~";
                }
                tempArrayValue = tempArrayOPE[i];
            }

            strCcyNo = strCcy.Split('~');
            strOPE = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strOPE.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayDocStamps.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayDocStamps[i]) && tempArrayDocStamps[i] != "" && tempArrayDocStamps[i] != null)
                {
                    strCcy += tempArrayDocStamps[i] + "~";
                }
                tempArrayValue = tempArrayDocStamps[i];
            }

            strCcyNo = strCcy.Split('~');
            strDocStamps = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strDocStamps.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayDebitAmountCcy.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayDebitAmountCcy[i]) && tempArrayDebitAmountCcy[i] != "" && tempArrayDebitAmountCcy[i] != null)
                {
                    strCcy += tempArrayDebitAmountCcy[i] + "~";
                }
                tempArrayValue = tempArrayDebitAmountCcy[i];
            }

            strCcyNo = strCcy.Split('~');
            strDebitAmount = new string[strCcyNo.Length, 2];
            strDebitAmountPerRemCcy = new string[strCcyNo.Length, 2];
            strNetDebitAmount = new string[strCcyNo.Length, 2];
            strNetDebitAmountPerRemCcy = new string[strCcyNo.Length, 2];


            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strDebitAmount.SetValue(strCcyNo[i], i + 1, 0);
                strDebitAmountPerRemCcy.SetValue(strCcyNo[i], i + 1, 0);
                strNetDebitAmount.SetValue(strCcyNo[i], i + 1, 0);
                strNetDebitAmountPerRemCcy.SetValue(strCcyNo[i], i + 1, 0);
            }

            try
            {
                objwriter = new StreamWriter(strFolder + "\\Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);
                printTR(true);
                printTD(true, "IRS World - CONSOLIDATION-(Total Amount - " + strHeader + ")", "15", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date : " + tranxdate, "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 1; a < 11; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, locationcode, "5", "left", "bold");
                    }
                    else if (a == 7)
                    {
                        printTD(true, "ORIGINAL/TARGET AMT", "3", "center", "bold");
                    }
                    else if (a == 9)
                    {
                        printTD(true, "BASE EQUIVALENT (" + myDataset.Tables[0].Rows[0]["LocalCurrency"].ToString() + ")", "2", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 11; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Transaction", "1", "center", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "RMT", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "Total", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "Net Debit", "1", "center", "bold");
                    }
                    else if (a == 5)
                    {
                        printTD(true, "Bank Com", "1", "center", "bold");
                    }
                    else if (a == 7)
                    {
                        printTD(true, "ACCT", "1", "center", "bold");
                    }
                    else if (a == 6 || a == 9)
                    {
                        printTD(true, "Debit", "1", "center", "bold");
                    }
                    else if (a == 8 || a == 10)
                    {
                        printTD(true, "Credit", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 11; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Source", "1", "center", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "CCY", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "Count", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "Amount", "1", "center", "bold");
                    }
                    else if (a == 5)
                    {
                        printTD(true, "(Local Courier Charges)", "1", "center", "bold");
                    }
                    else if (a == 7)
                    {
                        printTD(true, "CCY", "1", "center", "bold");
                    }
                    else
                    {
                        printTD(true, "Amount", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                //starts of extraction for (int r = 0; r < 3; r++)

                bool blnDebit = true;
                decimal decPRX = 0;

                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "1 for gross")
                    {
                        for (int index = 1; index <= strDebitAmount.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strDebitAmount[index, 0])
                            {
                                strDebitAmount.SetValue(Convert.ToString(Convert.ToDecimal(strDebitAmount[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_amt"])), index, 1);
                                strDebitAmountPerRemCcy.SetValue(Convert.ToString(Convert.ToDecimal(strDebitAmountPerRemCcy[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_amt"])), index, 1);
                            }
                        }
                        decDebitAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                        decDebitAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                        intTotalItemsPerTranxsource += 1;
                        intTotalItemsPerTranxsourcePerRemCcy += 1;
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "2 for net")
                    {
                        for (int index = 1; index <= strNetDebitAmount.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["remittingcurrency"].ToString() == strNetDebitAmount[index, 0])
                            {
                                strNetDebitAmount.SetValue(Convert.ToString(Convert.ToDecimal(strNetDebitAmount[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["NetRemAmount"])), index, 1);
                                strNetDebitAmountPerRemCcy.SetValue(Convert.ToString(Convert.ToDecimal(strNetDebitAmountPerRemCcy[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["NetRemAmount"])), index, 1);
                            }
                        }
                        decCreditAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                        decCreditAmountPerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decTotalSuspenseAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);

                        if (myDataset.Tables[0].Rows[r]["Transtatus"].ToString() == "2" ||
                            myDataset.Tables[0].Rows[r]["Transtatus"].ToString() == "273")//"17")
                        {
                            decTotalSuspenseAmountCO += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                        }
                        else
                        {
                            decTotalSuspenseAmountSO += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                        }
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "3 for Bank Com")
                    {
                        for (int index = 1; index <= strbankcom.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strbankcom[index, 0])
                            {
                                strbankcom.SetValue(Convert.ToString(Convert.ToDecimal(strbankcom[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                                strbankcompertranx.SetValue(Convert.ToString(Convert.ToDecimal(strbankcompertranx[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                                strbankcompertranxPerRemCcy.SetValue(Convert.ToString(Convert.ToDecimal(strbankcompertranxPerRemCcy[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "4 for Local Cable")
                    {
                        for (int index = 1; index <= strLocalCable.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalCable[index, 0])
                            {
                                strLocalCable.SetValue(Convert.ToString(Convert.ToDecimal(strLocalCable[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "5 for Local Service Fee")
                    {
                        for (int index = 1; index <= strLocalServiceFee.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalServiceFee[index, 0])
                            {
                                strLocalServiceFee.SetValue(Convert.ToString(Convert.ToDecimal(strLocalServiceFee[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "6 for OPE")
                    {
                        for (int index = 1; index <= strOPE.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strOPE[index, 0])
                            {
                                strOPE.SetValue(Convert.ToString(Convert.ToDecimal(strOPE[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "7 for Doc Stamps")
                    {
                        for (int index = 1; index <= strDocStamps.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strDocStamps[index, 0])
                            {
                                strDocStamps.SetValue(Convert.ToString(Convert.ToDecimal(strDocStamps[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }

                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "1 for gross")
                    {
                        blnDebit = true;
                    }
                    else
                    {
                        blnDebit = false;
                    }

                    for (int col = 1; col < 9; col++)
                    {
                        if (col == 7)
                        {
                            if (blnDebit == true)
                            {
                                decPRX = Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                                decBaseTotalDebitAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                            }
                        }
                        else if (col == 8)
                        {
                            if (blnDebit == false)
                            {
                                decPRX -= Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                                decBaseTotalCreditAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                            }
                        }
                    }

                    bool blnNextRow = false;

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["TranxSource"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["TranxSource"].ToString().Trim() ||
                        myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim() ||
                        myDataset.Tables[0].Rows[r]["RemittingCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["RemittingCurrency"].ToString().Trim())
                    {
                        int intBankCodeIndex = 1;
                        for (int i = 1; i <= strDebitAmount.GetUpperBound(0); i++)
                        {
                            if (Convert.ToDecimal(strDebitAmount[i, 1]) != 0)
                            {
                                if (blnNextRow == false)
                                {
                                    printTR(true);
                                    printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString(), "1", "left", "normal");
                                    printTD(false);
                                    printTD(true, strDebitAmount[i, 0], "1", "center", "normal");
                                    printTD(false);
                                    printTD(true, Convert.ToString(intTotalItemsPerTranxsource), "1", "center", "normal");
                                    printTD(false);

                                    printTD(true, "(" + strNetDebitAmount[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmount[i, 1])), "1", "right", "normal");
                                    printTD(false);

                                    string strTDValue = "0";
                                    for (int z = 1; z <= strbankcompertranx.GetUpperBound(0); z++)
                                    {
                                        if (Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]) == 0)
                                        {
                                            intBankCodeIndex += 1;
                                        }
                                        else
                                        {
                                            strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]));
                                        }
                                    }
                                    printTD(true, strTDValue, "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmount[i, 1])), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "1", "center", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmount), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decDebitAmountBase), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmountBase), "1", "right", "normal");
                                    printTD(false);
                                    printTR(false);
                                    blnNextRow = true;
                                }
                                else
                                {
                                    printTR(true);
                                    printTD(true, "", "1", "left", "normal");
                                    printTD(false);
                                    printTD(true, strDebitAmount[i, 0], "1", "center", "normal");
                                    printTD(false);
                                    printTD(true, "", "1", "left", "normal");
                                    printTD(false);

                                    if (Convert.ToDecimal(strNetDebitAmount[i, 1]) == 0)
                                    {
                                        printTD(true, "", "1", "right", "normal");
                                    }
                                    else
                                    {
                                        printTD(true, "(" + strNetDebitAmount[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmount[i, 1])), "1", "right", "normal");
                                    }
                                    printTD(false);

                                    string strTDValue = "0";

                                    if (i > strbankcompertranx.GetUpperBound(0))
                                    {
                                        printTD(true, "", "1", "right", "normal");
                                    }
                                    else
                                    {
                                        for (int z = intBankCodeIndex; z <= strbankcompertranx.GetUpperBound(0); z++)
                                        {
                                            if (Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]) == 0)
                                            {
                                                intBankCodeIndex += 1;
                                            }
                                            else
                                            {
                                                strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]));
                                            }
                                        }
                                        printTD(true, strTDValue, "1", "right", "normal");
                                    }
                                    printTD(false);

                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmount[i, 1])), "1", "right", "normal");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }
                        for (int i = 1; i <= strNetDebitAmount.GetUpperBound(0); i++)
                        {
                            strNetDebitAmount.SetValue("0", i, 1);
                        }

                        for (int i = 1; i <= strDebitAmount.GetUpperBound(0); i++)
                        {
                            strDebitAmount.SetValue("0", i, 1);
                        }
                        decCreditAmount = 0;
                        decDebitAmountBase = 0;
                        decCreditAmountBase = 0;

                        for (int i = 1; i <= strbankcompertranx.GetUpperBound(0); i++)
                        {
                            strbankcompertranx.SetValue("0", i, 1);
                        }
                        intTotalItemsPerTranxsource = 0;
                    }

                    blnNextRow = false;

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["RemittingCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["RemittingCurrency"].ToString().Trim())
                    {
                        int intBankCodeIndex = 1;
                        for (int i = 1; i <= strDebitAmountPerRemCcy.GetUpperBound(0); i++)
                        {
                            if (Convert.ToDecimal(strDebitAmountPerRemCcy[i, 1]) != 0)
                            {
                                if (blnNextRow == false)
                                {
                                    printTR(true);
                                    printTD(true, strDebitAmountPerRemCcy[i, 0] + " Total", "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDebitAmountPerRemCcy[i, 0], "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, Convert.ToString(intTotalItemsPerTranxsourcePerRemCcy), "1", "center", "bold");
                                    printTD(false);

                                    printTD(true, "(" + strNetDebitAmountPerRemCcy[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    printTD(false);

                                    string strTDValue = "0";
                                    for (int z = 1; z <= strbankcompertranxPerRemCcy.GetUpperBound(0); z++)
                                    {
                                        if (Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]) == 0)
                                        {
                                            intBankCodeIndex += 1;
                                        }
                                        else
                                        {
                                            strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]));
                                        }
                                    }
                                    printTD(true, strTDValue, "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmountPerRemCcy), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decDebitAmountBasePerRemCcy), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmountBasePerRemCcy), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                    blnNextRow = true;
                                }
                                else
                                {
                                    printTR(true);
                                    printTD(true, "", "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDebitAmountPerRemCcy[i, 0], "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "left", "bold");
                                    printTD(false);

                                    if (Convert.ToDecimal(strNetDebitAmountPerRemCcy[i, 1]) == 0)
                                    {
                                        printTD(true, "", "1", "right", "bold");
                                    }
                                    else
                                    {
                                        printTD(true, "(" + strNetDebitAmountPerRemCcy[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    }
                                    printTD(false);

                                    string strTDValue = "0";

                                    if (i > strbankcompertranxPerRemCcy.GetUpperBound(0))
                                    {
                                        printTD(true, "", "1", "right", "bold");
                                    }
                                    else
                                    {
                                        for (int z = intBankCodeIndex; z <= strbankcompertranxPerRemCcy.GetUpperBound(0); z++)
                                        {
                                            if (Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]) == 0)
                                            {
                                                intBankCodeIndex += 1;
                                            }
                                            else
                                            {
                                                strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]));
                                            }
                                        }
                                        printTD(true, strTDValue, "1", "right", "bold");
                                    }
                                    printTD(false);

                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }
                        printTR(true);
                        printTR(false);
                        for (int i = 1; i <= strNetDebitAmountPerRemCcy.GetUpperBound(0); i++)
                        {
                            strNetDebitAmountPerRemCcy.SetValue("0", i, 1);
                        }

                        for (int i = 1; i <= strDebitAmountPerRemCcy.GetUpperBound(0); i++)
                        {
                            strDebitAmountPerRemCcy.SetValue("0", i, 1);
                        }
                        decCreditAmountPerRemCcy = 0;
                        decDebitAmountBasePerRemCcy = 0;
                        decCreditAmountBasePerRemCcy = 0;

                        for (int i = 1; i <= strbankcompertranxPerRemCcy.GetUpperBound(0); i++)
                        {
                            strbankcompertranxPerRemCcy.SetValue("0", i, 1);
                        }
                        intTotalItemsPerTranxsourcePerRemCcy = 0;
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim())
                    {
                        printTR(true);
                        printTD(true, "SUB-TOTALS - SUSPENSE - " + myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "8", "left", "bold");
                        printTD(false);
                        printTR(false);

                        printTR(true);
                        printTD(true, "  SUSPENSE - (CREDITED ONLINE)", "6", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "1", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decTotalSuspenseAmountCO), "1", "right", "bold");
                        printTD(false);
                        printTR(false);

                        printTD(true, "  SUSPENSE - (Processed by GRS)", "6", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "1", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decTotalSuspenseAmountSO), "1", "right", "bold");
                        printTD(false);
                        printTR(false);

                        decTotalSuspenseAmount = 0;
                        decTotalSuspenseAmountCO = 0;
                        decTotalSuspenseAmountSO = 0;

                        //SUB-TOTALS REM CHARGES (local courier)
                        if (strbankcom.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strbankcom[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local courier)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strbankcom[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcom[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalCable.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalCable[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local cable)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalCable[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strLocalCable[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalServiceFee.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalServiceFee[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local service fee)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalServiceFee[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strLocalServiceFee[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strOPE.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strOPE[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Out-of-pocket Expense)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strOPE[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strOPE[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strDocStamps.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strDocStamps[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Documentary Stamp)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDocStamps[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDocStamps[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                        {
                            strbankcom.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                        {
                            strLocalCable.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                        {
                            strLocalServiceFee.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                        {
                            strOPE.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                        {
                            strDocStamps.SetValue("0", i, 1);
                        }

                        printTR(true);
                        printTR(false);

                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1)
                    {
                        printTR(true);
                        printTD(true, "Total", "8", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount), "1", "right", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);

                        printTR(true);
                        printTD(true, "PRX", "5", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["PRXGLCode"].ToString(), "1", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["LocalCurrency"].ToString(), "3", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount - (decTotalSuspenseAmountCO + decTotalSuspenseAmountSO)), "1", "right", "bold"); // modify by mike, deduct credit amount to suspense amount to get proper PRX
                        printTD(false);
                        printTR(false);
                    }
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch //(Exception ex) // commented to remove warning
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Consolidated_Report(string tranxdate, string locationcode, string tranxstatus)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();

            string strCOSO = "";
            string strHeader = "";
            //string strCOSOBody = "";


            string[] tempArraybankcom;
            string[] tempArrayLocalCable;
            string[] tempArrayLocalServiceFee;
            string[] tempArrayOPE;
            string[] tempArrayDocStamps;
            string tempArrayValue;
            string strCcy;
            string[] strCcyNo;
            string[,] strbankcom;
            string[,] strLocalCable;
            string[,] strLocalServiceFee;
            string[,] strOPE;
            string[,] strDocStamps;

            string[,] strbankcompertranx;
            string[] tempArrayDebitAmountCcy;
            string[,] strDebitAmount;
            string[,] strNetDebitAmount;
            decimal decCreditAmount = 0;
            decimal decDebitAmountBase = 0;
            decimal decCreditAmountBase = 0;
            int intTotalItemsPerTranxsource = 0;


            string[,] strbankcompertranxPerRemCcy;
            string[,] strDebitAmountPerRemCcy;
            string[,] strNetDebitAmountPerRemCcy;
            decimal decCreditAmountPerRemCcy = 0;
            decimal decDebitAmountBasePerRemCcy = 0;
            decimal decCreditAmountBasePerRemCcy = 0;
            int intTotalItemsPerTranxsourcePerRemCcy = 0;

            if (tranxstatus == "Credited Online")
            {
                tranxstatus = "'2,273'"; //17'";
                strCOSO = "CO";
                strHeader = "Credited Online";
                //strCOSOBody = "OL";
            }
            else
            {
                tranxstatus = "'99,5'"; // "'5,16,99'";
                strCOSO = "PG";
                strHeader = "Processed by GRS";
                //strCOSOBody = "NOL";
            }
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }
            else
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report_london '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }
            myDataset = commons.getDataSet(sqlcommands, "consolidated_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            tempArrayDebitAmountCcy = new string[myDataset.Tables[0].Rows.Count];
            tempArraybankcom = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalCable = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalServiceFee = new string[myDataset.Tables[0].Rows.Count];
            tempArrayOPE = new string[myDataset.Tables[0].Rows.Count];
            tempArrayDocStamps = new string[myDataset.Tables[0].Rows.Count];

            int intDebitAmountCcy = 0;
            int intbankcomindex = 0;
            int intLocalCableindex = 0;
            int intLocalServiceFeeindex = 0;
            int intOPEindex = 0;
            int intDocStampsindex = 0;
            decimal decTotalSuspenseAmount = 0;
            decimal decBaseTotalCreditAmount = 0;
            decimal decBaseTotalDebitAmount = 0;
            decimal phpTotal = 0;
            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "1 for gross")
                {
                    tempArrayDebitAmountCcy.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intDebitAmountCcy);
                    intDebitAmountCcy += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "3 for Bank Com")
                {
                    tempArraybankcom.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intbankcomindex);
                    intbankcomindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "4 for Local Cable")
                {
                    tempArrayLocalCable.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalCableindex);
                    intLocalCableindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "5 for Local Service Fee")
                {
                    tempArrayLocalServiceFee.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalServiceFeeindex);
                    intLocalServiceFeeindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "6 for OPE")
                {
                    tempArrayOPE.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intOPEindex);
                    intOPEindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "7 for Doc Stamps")
                {
                    tempArrayDocStamps.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intDocStampsindex);
                    intDocStampsindex += 1;
                }
            }

            Array.Sort(tempArrayDebitAmountCcy);
            Array.Sort(tempArraybankcom);
            Array.Sort(tempArrayLocalCable);
            Array.Sort(tempArrayLocalServiceFee);
            Array.Sort(tempArrayOPE);
            Array.Sort(tempArrayDocStamps);

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArraybankcom.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArraybankcom[i]) && tempArraybankcom[i] != "" && tempArraybankcom[i] != null)
                {
                    strCcy += tempArraybankcom[i] + "~";
                }
                tempArrayValue = tempArraybankcom[i];
            }

            strCcyNo = strCcy.Split('~');
            strbankcom = new string[strCcyNo.Length, 2];
            strbankcompertranx = new string[strCcyNo.Length, 2];
            strbankcompertranxPerRemCcy = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strbankcom.SetValue(strCcyNo[i], i + 1, 0);
                strbankcompertranx.SetValue(strCcyNo[i], i + 1, 0);
                strbankcompertranxPerRemCcy.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalCable.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalCable[i]) && tempArrayLocalCable[i] != "" && tempArrayLocalCable[i] != null)
                {
                    strCcy += tempArrayLocalCable[i] + "~";
                }
                tempArrayValue = tempArrayLocalCable[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalCable = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalCable.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalServiceFee.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalServiceFee[i]) && tempArrayLocalServiceFee[i] != "" && tempArrayLocalServiceFee[i] != null)
                {
                    strCcy += tempArrayLocalServiceFee[i] + "~";
                }
                tempArrayValue = tempArrayLocalServiceFee[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalServiceFee = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalServiceFee.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayOPE.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayOPE[i]) && tempArrayOPE[i] != "" && tempArrayOPE[i] != null)
                {
                    strCcy += tempArrayOPE[i] + "~";
                }
                tempArrayValue = tempArrayOPE[i];
            }

            strCcyNo = strCcy.Split('~');
            strOPE = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strOPE.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayDocStamps.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayDocStamps[i]) && tempArrayDocStamps[i] != "" && tempArrayDocStamps[i] != null)
                {
                    strCcy += tempArrayDocStamps[i] + "~";
                }
                tempArrayValue = tempArrayDocStamps[i];
            }

            strCcyNo = strCcy.Split('~');
            strDocStamps = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strDocStamps.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayDebitAmountCcy.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayDebitAmountCcy[i]) && tempArrayDebitAmountCcy[i] != "" && tempArrayDebitAmountCcy[i] != null)
                {
                    strCcy += tempArrayDebitAmountCcy[i] + "~";
                }
                tempArrayValue = tempArrayDebitAmountCcy[i];
            }

            strCcyNo = strCcy.Split('~');
            strDebitAmount = new string[strCcyNo.Length, 2];
            strDebitAmountPerRemCcy = new string[strCcyNo.Length, 2];
            strNetDebitAmount = new string[strCcyNo.Length, 2];
            strNetDebitAmountPerRemCcy = new string[strCcyNo.Length, 2];


            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strDebitAmount.SetValue(strCcyNo[i], i + 1, 0);
                strDebitAmountPerRemCcy.SetValue(strCcyNo[i], i + 1, 0);
                strNetDebitAmount.SetValue(strCcyNo[i], i + 1, 0);
                strNetDebitAmountPerRemCcy.SetValue(strCcyNo[i], i + 1, 0);
            }

            try
            {
                objwriter = new StreamWriter(strFolder + "\\Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);
                printTR(true);
                printTD(true, "IRS World - CONSOLIDATION-(Total Amount - " + strHeader + ")", "15", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date : " + tranxdate, "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 1; a < 11; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, locationcode, "5", "left", "bold");
                    }
                    else if (a == 7)
                    {
                        printTD(true, "ORIGINAL/TARGET AMT", "3", "center", "bold");
                    }
                    else if (a == 9)
                    {
                        printTD(true, "BASE EQUIVALENT (" + myDataset.Tables[0].Rows[0]["LocalCurrency"].ToString() + ")", "2", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 11; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Transaction", "1", "center", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "RMT", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "Total", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "Net Debit", "1", "center", "bold");
                    }
                    else if (a == 5)
                    {
                        printTD(true, "Bank Com", "1", "center", "bold");
                    }
                    else if (a == 7)
                    {
                        printTD(true, "ACCT", "1", "center", "bold");
                    }
                    else if (a == 6 || a == 9)
                    {
                        printTD(true, "Debit", "1", "center", "bold");
                    }
                    else if (a == 8 || a == 10)
                    {
                        printTD(true, "Credit", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 11; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Source", "1", "center", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "CCY", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "Count", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "Amount", "1", "center", "bold");
                    }
                    else if (a == 5)
                    {
                        printTD(true, "(Local Courier Charges)", "1", "center", "bold");
                    }
                    else if (a == 7)
                    {
                        printTD(true, "CCY", "1", "center", "bold");
                    }
                    else
                    {
                        printTD(true, "Amount", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                //starts of extraction for (int r = 0; r < 3; r++)

                bool blnDebit = true;
                decimal decPRX = 0;

                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "1 for gross")
                    {
                        for (int index = 1; index <= strDebitAmount.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strDebitAmount[index, 0])
                            {
                                strDebitAmount.SetValue(Convert.ToString(Convert.ToDecimal(strDebitAmount[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_amt"])), index, 1);
                                strDebitAmountPerRemCcy.SetValue(Convert.ToString(Convert.ToDecimal(strDebitAmountPerRemCcy[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_amt"])), index, 1);
                            }
                        }
                        decDebitAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                        decDebitAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                        intTotalItemsPerTranxsource += 1;
                        intTotalItemsPerTranxsourcePerRemCcy += 1;
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "2 for net")
                    {
                        for (int index = 1; index <= strNetDebitAmount.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["remittingcurrency"].ToString() == strNetDebitAmount[index, 0])
                            {
                                strNetDebitAmount.SetValue(Convert.ToString(Convert.ToDecimal(strNetDebitAmount[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["NetRemAmount"])), index, 1);
                                strNetDebitAmountPerRemCcy.SetValue(Convert.ToString(Convert.ToDecimal(strNetDebitAmountPerRemCcy[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["NetRemAmount"])), index, 1);
                            }
                        }
                        decCreditAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                        decCreditAmountPerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decTotalSuspenseAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "3 for Bank Com")
                    {
                        for (int index = 1; index <= strbankcom.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strbankcom[index, 0])
                            {
                                strbankcom.SetValue(Convert.ToString(Convert.ToDecimal(strbankcom[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                                strbankcompertranx.SetValue(Convert.ToString(Convert.ToDecimal(strbankcompertranx[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                                strbankcompertranxPerRemCcy.SetValue(Convert.ToString(Convert.ToDecimal(strbankcompertranxPerRemCcy[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "4 for Local Cable")
                    {
                        for (int index = 1; index <= strLocalCable.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalCable[index, 0])
                            {
                                strLocalCable.SetValue(Convert.ToString(Convert.ToDecimal(strLocalCable[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "5 for Local Service Fee")
                    {
                        for (int index = 1; index <= strLocalServiceFee.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalServiceFee[index, 0])
                            {
                                strLocalServiceFee.SetValue(Convert.ToString(Convert.ToDecimal(strLocalServiceFee[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "6 for OPE")
                    {
                        for (int index = 1; index <= strOPE.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strOPE[index, 0])
                            {
                                strOPE.SetValue(Convert.ToString(Convert.ToDecimal(strOPE[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "7 for Doc Stamps")
                    {
                        for (int index = 1; index <= strDocStamps.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strDocStamps[index, 0])
                            {
                                strDocStamps.SetValue(Convert.ToString(Convert.ToDecimal(strDocStamps[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decCreditAmountBasePerRemCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }

                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "1 for gross")
                    {
                        blnDebit = true;
                    }
                    else
                    {
                        blnDebit = false;
                    }

                    for (int col = 1; col < 9; col++)
                    {
                        if (col == 7)
                        {
                            if (blnDebit == true)
                            {
                                decPRX = Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                                decBaseTotalDebitAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                            }
                        }
                        else if (col == 8)
                        {
                            if (blnDebit == false)
                            {
                                decPRX -= Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                                decBaseTotalCreditAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                            }
                        }
                    }

                    bool blnNextRow = false;

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["TranxSource"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["TranxSource"].ToString().Trim() ||
                        myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim() ||
                        myDataset.Tables[0].Rows[r]["RemittingCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["RemittingCurrency"].ToString().Trim())
                    {
                        int intBankCodeIndex = 1;
                        for (int i = 1; i <= strDebitAmount.GetUpperBound(0); i++)
                        {
                            if (Convert.ToDecimal(strDebitAmount[i, 1]) != 0)
                            {
                                if (blnNextRow == false)
                                {
                                    printTR(true);
                                    printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString(), "1", "left", "normal");
                                    printTD(false);
                                    printTD(true, strDebitAmount[i, 0], "1", "center", "normal");
                                    printTD(false);
                                    printTD(true, Convert.ToString(intTotalItemsPerTranxsource), "1", "center", "normal");
                                    printTD(false);

                                    printTD(true, "(" + strNetDebitAmount[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmount[i, 1])), "1", "right", "normal");
                                    printTD(false);

                                    string strTDValue = "0";
                                    for (int z = 1; z <= strbankcompertranx.GetUpperBound(0); z++)
                                    {
                                        if (Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]) == 0)
                                        {
                                            intBankCodeIndex += 1;
                                        }
                                        else
                                        {
                                            strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]));
                                        }
                                    }
                                    printTD(true, strTDValue, "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmount[i, 1])), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "1", "center", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmount), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decDebitAmountBase), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmountBase), "1", "right", "normal");
                                    printTD(false);
                                    printTR(false);
                                    blnNextRow = true;
                                }
                                else
                                {
                                    printTR(true);
                                    printTD(true, "", "1", "left", "normal");
                                    printTD(false);
                                    printTD(true, strDebitAmount[i, 0], "1", "center", "normal");
                                    printTD(false);
                                    printTD(true, "", "1", "left", "normal");
                                    printTD(false);

                                    if (Convert.ToDecimal(strNetDebitAmount[i, 1]) == 0)
                                    {
                                        printTD(true, "", "1", "right", "normal");
                                    }
                                    else
                                    {
                                        printTD(true, "(" + strNetDebitAmount[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmount[i, 1])), "1", "right", "normal");
                                    }
                                    printTD(false);

                                    string strTDValue = "0";

                                    if (i > strbankcompertranx.GetUpperBound(0))
                                    {
                                        printTD(true, "", "1", "right", "normal");
                                    }
                                    else
                                    {
                                        for (int z = intBankCodeIndex; z <= strbankcompertranx.GetUpperBound(0); z++)
                                        {
                                            if (Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]) == 0)
                                            {
                                                intBankCodeIndex += 1;
                                            }
                                            else
                                            {
                                                strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranx[intBankCodeIndex, 1]));
                                            }
                                        }
                                        printTD(true, strTDValue, "1", "right", "normal");
                                    }
                                    printTD(false);

                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmount[i, 1])), "1", "right", "normal");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "normal");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }
                        for (int i = 1; i <= strNetDebitAmount.GetUpperBound(0); i++)
                        {
                            strNetDebitAmount.SetValue("0", i, 1);
                        }

                        for (int i = 1; i <= strDebitAmount.GetUpperBound(0); i++)
                        {
                            strDebitAmount.SetValue("0", i, 1);
                        }
                        decCreditAmount = 0;
                        decDebitAmountBase = 0;
                        decCreditAmountBase = 0;

                        for (int i = 1; i <= strbankcompertranx.GetUpperBound(0); i++)
                        {
                            strbankcompertranx.SetValue("0", i, 1);
                        }
                        intTotalItemsPerTranxsource = 0;
                    }

                    blnNextRow = false;

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["RemittingCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["RemittingCurrency"].ToString().Trim() || myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim())
                    {
                        int intBankCodeIndex = 1;
                        for (int i = 1; i <= strDebitAmountPerRemCcy.GetUpperBound(0); i++)
                        {
                            if (Convert.ToDecimal(strDebitAmountPerRemCcy[i, 1]) != 0)
                            {
                                if (blnNextRow == false)
                                {
                                    printTR(true);
                                    printTD(true, strDebitAmountPerRemCcy[i, 0] + " Total", "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDebitAmountPerRemCcy[i, 0], "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, Convert.ToString(intTotalItemsPerTranxsourcePerRemCcy), "1", "center", "bold");
                                    printTD(false);

                                    printTD(true, "(" + strNetDebitAmountPerRemCcy[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    printTD(false);

                                    string strTDValue = "0";
                                    for (int z = 1; z <= strbankcompertranxPerRemCcy.GetUpperBound(0); z++)
                                    {
                                        if (Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]) == 0)
                                        {
                                            intBankCodeIndex += 1;
                                        }
                                        else
                                        {
                                            strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]));
                                        }
                                    }
                                    printTD(true, strTDValue, "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmountPerRemCcy), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decDebitAmountBasePerRemCcy), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmountBasePerRemCcy), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                    blnNextRow = true;
                                }
                                else
                                {
                                    printTR(true);
                                    printTD(true, "", "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDebitAmountPerRemCcy[i, 0], "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "left", "bold");
                                    printTD(false);

                                    if (Convert.ToDecimal(strNetDebitAmountPerRemCcy[i, 1]) == 0)
                                    {
                                        printTD(true, "", "1", "right", "bold");
                                    }
                                    else
                                    {
                                        printTD(true, "(" + strNetDebitAmountPerRemCcy[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strNetDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    }
                                    printTD(false);

                                    string strTDValue = "0";

                                    if (i > strbankcompertranxPerRemCcy.GetUpperBound(0))
                                    {
                                        printTD(true, "", "1", "right", "bold");
                                    }
                                    else
                                    {
                                        for (int z = intBankCodeIndex; z <= strbankcompertranxPerRemCcy.GetUpperBound(0); z++)
                                        {
                                            if (Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]) == 0)
                                            {
                                                intBankCodeIndex += 1;
                                            }
                                            else
                                            {
                                                strTDValue = String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcompertranxPerRemCcy[intBankCodeIndex, 1]));
                                            }
                                        }
                                        printTD(true, strTDValue, "1", "right", "bold");
                                    }
                                    printTD(false);

                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmountPerRemCcy[i, 1])), "1", "right", "bold");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);

                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }
                        printTR(true);
                        printTR(false);
                        for (int i = 1; i <= strNetDebitAmountPerRemCcy.GetUpperBound(0); i++)
                        {
                            strNetDebitAmountPerRemCcy.SetValue("0", i, 1);
                        }

                        for (int i = 1; i <= strDebitAmountPerRemCcy.GetUpperBound(0); i++)
                        {
                            strDebitAmountPerRemCcy.SetValue("0", i, 1);
                        }
                        decCreditAmountPerRemCcy = 0;
                        decDebitAmountBasePerRemCcy = 0;
                        decCreditAmountBasePerRemCcy = 0;

                        for (int i = 1; i <= strbankcompertranxPerRemCcy.GetUpperBound(0); i++)
                        {
                            strbankcompertranxPerRemCcy.SetValue("0", i, 1);
                        }
                        intTotalItemsPerTranxsourcePerRemCcy = 0;
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim())
                    {
                        printTR(true);
                        printTD(true, "SUB-TOTALS - SUSPENSE - " + myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "8", "left", "bold");
                        printTD(false);
                        printTR(false);

                        printTR(true);
                        printTD(true, "  SUSPENSE - (" + strHeader.ToUpper() + ")", "6", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "1", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decTotalSuspenseAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);
                        if (myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString() == "PHP")
                        {
                            phpTotal += decTotalSuspenseAmount;
                        }

                        decTotalSuspenseAmount = 0;

                        //SUB-TOTALS REM CHARGES (local courier)
                        if (strbankcom.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strbankcom[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local courier)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strbankcom[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcom[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalCable.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalCable[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local cable)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalCable[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strLocalCable[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalServiceFee.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalServiceFee[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local service fee)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalServiceFee[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strLocalServiceFee[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strOPE.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strOPE[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Out-of-pocket Expense)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strOPE[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strOPE[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strDocStamps.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strDocStamps[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Documentary Stamp)", "6", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDocStamps[i, 0], "1", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDocStamps[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                        {
                            strbankcom.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                        {
                            strLocalCable.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                        {
                            strLocalServiceFee.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                        {
                            strOPE.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                        {
                            strDocStamps.SetValue("0", i, 1);
                        }

                        printTR(true);
                        printTR(false);

                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1)
                    {

                        printTR(true);
                        printTR(false);
                        printTR(true);
                        printTD(true, "PHP Total", "4", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", phpTotal), "1", "right", "bold");
                        printTD(false);
                        printTD(false);
                        printTD(false);


                        printTR(true);
                        printTD(true, "PRX", "2", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["PRXGLCode"].ToString(), "1", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["LocalCurrency"].ToString(), "3", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount), "1", "right", "bold");
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount - decBaseTotalCreditAmount), "1", "right", "bold");
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount - decTotalSuspenseAmount), "1", "right", "bold");  //modify by mike to get proper PRX 06/02/2015
                        printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);
                        //printTR(true);
                        //printTD(true, "Total", "8", "left", "bold");
                        //printTD(false);
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount), "1", "right", "bold");
                        //printTD(false);
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount), "1", "right", "bold");
                        //printTD(false);
                        //printTR(false);

                        //printTR(true);
                        //printTD(true, "PRX", "5", "left", "bold");
                        //printTD(false);
                        //printTD(true, myDataset.Tables[0].Rows[r]["PRXGLCode"].ToString(), "1", "left", "bold");
                        //printTD(false);
                        //printTD(true, myDataset.Tables[0].Rows[r]["LocalCurrency"].ToString(), "3", "left", "bold");
                        //printTD(false);
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount - decBaseTotalCreditAmount), "1", "right", "bold");
                        //printTD(false);
                        //printTR(false);
                    }
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch //(Exception ex) // commented to remove warning
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Consolidated_GLCodes_Report_Online_NonOnline(string tranxdate, string locationcode, string tranxstatus)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();

            string[,] strAmountPerTranx;
            decimal decAmountPerGLCode = 0;
            decimal decAmountPerCcy = 0;

            string[] strCcyNo;
            string[] tempTranxSource;
            string strCcy;

            tranxstatus = "'2, 273, 99, 5'"; // "'2,17,5,16,99'";
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            sqlcommands = commons.getSqlCommand("EXEC sp_summary_by_gl_report '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            myDataset = commons.getDataSet(sqlcommands, "gl_on_non_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            tempTranxSource = new string[myDataset.Tables[0].Rows.Count];

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                tempTranxSource.SetValue(myDataset.Tables[0].Rows[row]["TranxSource"].ToString(), row);
            }

            Array.Sort(tempTranxSource);
            string tempArrayValue;
            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempTranxSource.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempTranxSource[i]) && tempTranxSource[i] != "" && tempTranxSource[i] != null)
                {
                    strCcy += tempTranxSource[i] + "~";
                }
                tempArrayValue = tempTranxSource[i];
            }

            strCcyNo = strCcy.Split('~');
            strAmountPerTranx = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strAmountPerTranx.SetValue(strCcyNo[i], i + 1, 0);
            }

            try
            {
                objwriter = new StreamWriter(strFolder + "\\ConsolidatedGLCodesCOPG" + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);
                printTR(true);
                printTD(true, "IRS World - CONSOLIDATION-(by GLCodes - Credited Online and Processed by GRS)", "5", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date : " + tranxdate, "5", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "5", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 1; a < 6; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Account Description", "1", "left", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "IFlex GL Code", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "Transaction Source", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "Currency", "1", "center", "bold");
                    }
                    else if (a == 5)
                    {
                        printTD(true, "Amount (Gross)", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);


                //starts of extraction for (int r = 0; r < 3; r++)

                bool blnPrintGL = false;

                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    for (int index = 1; index <= strAmountPerTranx.GetUpperBound(0); index++)
                    {
                        if (myDataset.Tables[0].Rows[r]["TranxSource"].ToString() == strAmountPerTranx[index, 0])
                        {
                            strAmountPerTranx.SetValue(Convert.ToString(Convert.ToDecimal(strAmountPerTranx[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"])), index, 1);
                        }
                    }

                    decAmountPerGLCode += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"]);
                    decAmountPerCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"]);

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["GLCode"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["GLCode"].ToString().Trim() ||
                        myDataset.Tables[0].Rows[r]["TranxSource"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["TranxSource"].ToString().Trim())
                    {
                        for (int i = 1; i <= strAmountPerTranx.GetUpperBound(0); i++)
                        {
                            if (Convert.ToDecimal(strAmountPerTranx[i, 1]) != 0)
                            {
                                printTR(true);
                                if (blnPrintGL == false)
                                {
                                    printTD(true, myDataset.Tables[0].Rows[r]["Description"].ToString(), "1", "left", "normal");
                                }
                                else
                                {
                                    printTD(true);
                                }
                                printTD(false);

                                if (blnPrintGL == false)
                                {
                                    printTD(true, myDataset.Tables[0].Rows[r]["GLCode"].ToString(), "1", "center", "normal");
                                }
                                else
                                {
                                    printTD(true);
                                }
                                printTD(false);

                                printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString(), "1", "right", "normal");
                                printTD(false);

                                if (blnPrintGL == false)
                                {
                                    printTD(true, myDataset.Tables[0].Rows[r]["acct_currency"].ToString(), "1", "center", "normal");
                                }
                                else
                                {
                                    printTD(true);
                                }
                                printTD(false);

                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strAmountPerTranx[i, 1])), "1", "right", "normal");
                                printTD(false);

                                printTR(false);
                                blnPrintGL = true;
                            }
                        }

                        for (int i = 1; i <= strAmountPerTranx.GetUpperBound(0); i++)
                        {
                            strAmountPerTranx.SetValue("0", i, 1);
                        }

                        if (r == myDataset.Tables[0].Rows.Count - 1 ||
                            myDataset.Tables[0].Rows[r]["GLCode"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["GLCode"].ToString().Trim())
                        {
                            blnPrintGL = false;
                            printTR(true);
                            printTR(false);

                            //print total per glcode
                            printTR(true);

                            printTD(true, "Total " + myDataset.Tables[0].Rows[r]["Description"].ToString() + " Transactions", "1", "left", "bold");
                            printTD(false);

                            printTD(true, "", "3", "left", "bold");
                            printTD(false);

                            printTD(true, String.Format("{0:0,0.000000000}", decAmountPerGLCode), "1", "right", "bold");
                            printTD(false);

                            printTR(false);

                            printTR(true);
                            printTR(false);
                            decAmountPerGLCode = 0;
                        }
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["acct_currency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["acct_currency"].ToString().Trim())
                    {
                        printTR(true);

                        printTD(true, "Total " + myDataset.Tables[0].Rows[r]["acct_currency"].ToString() + " Transactions", "1", "left", "bold");
                        printTD(false);

                        printTD(true, "", "3", "left", "bold");
                        printTD(false);

                        printTD(true, String.Format("{0:0,0.000000000}", decAmountPerCcy), "1", "right", "bold");
                        printTD(false);

                        printTR(false);
                        decAmountPerCcy = 0;

                        printTR(true);
                        printTR(false);
                    }
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch //(Exception ex) // commented to remove warning
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "ConsolidatedGLCodesCOPG" + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\ConsolidatedGLCodesCOPG" + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Excluded_Transactions_Report(string tranxdate, string locationcode)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();
            //string strCOSO = "";

            string[,] strAmountPerAmt;
            string[,] strGrandTotalAmountPerAmt;

            string[] strCcyNo;
            string[] tempAmtCcy;
            string strCcy;
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            sqlcommands = commons.getSqlCommand("EXEC sp_excluded_transactions_report '" + locationcode + "', '" + tranxdate + "'", false);
            myDataset = commons.getDataSet(sqlcommands, "excluded_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            tempAmtCcy = new string[myDataset.Tables[0].Rows.Count];

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                tempAmtCcy.SetValue(myDataset.Tables[0].Rows[row]["Nettobeneccy"].ToString(), row);
            }

            Array.Sort(tempAmtCcy);
            string tempArrayValue;
            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempAmtCcy.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempAmtCcy[i]) && tempAmtCcy[i] != "" && tempAmtCcy[i] != null)
                {
                    strCcy += tempAmtCcy[i] + "~";
                }
                tempArrayValue = tempAmtCcy[i];
            }

            strCcyNo = strCcy.Split('~');
            strAmountPerAmt = new string[strCcyNo.Length, 8];
            strGrandTotalAmountPerAmt = new string[strCcyNo.Length, 8];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strAmountPerAmt.SetValue(strCcyNo[i], i + 1, 0);
                strGrandTotalAmountPerAmt.SetValue(strCcyNo[i], i + 1, 0);
            }

            strAmountPerAmt.SetValue("Local Payment amout", 0, 1);
            strAmountPerAmt.SetValue("NetRemitBen", 0, 2);
            strAmountPerAmt.SetValue("Local Courier", 0, 3);
            strAmountPerAmt.SetValue("OPE", 0, 4);
            strAmountPerAmt.SetValue("Documentary Stamp", 0, 5);
            strAmountPerAmt.SetValue("Local Cable", 0, 6);
            strAmountPerAmt.SetValue("Local Service", 0, 7);

            strGrandTotalAmountPerAmt.SetValue("Local Payment amout", 0, 1);
            strGrandTotalAmountPerAmt.SetValue("NetRemitBen", 0, 2);
            strGrandTotalAmountPerAmt.SetValue("Local Courier", 0, 3);
            strGrandTotalAmountPerAmt.SetValue("OPE", 0, 4);
            strGrandTotalAmountPerAmt.SetValue("Documentary Stamp", 0, 5);
            strGrandTotalAmountPerAmt.SetValue("Local Cable", 0, 6);
            strGrandTotalAmountPerAmt.SetValue("Local Service", 0, 7);

            for (int a = 1; a <= strAmountPerAmt.GetUpperBound(0); a++)
            {
                for (int b = 1; b <= strAmountPerAmt.GetUpperBound(1); b++)
                {
                    strAmountPerAmt.SetValue("0", a, b);
                    strGrandTotalAmountPerAmt.SetValue("0", a, b);
                }
            }

            try
            {
                objwriter = new StreamWriter(strFolder + "\\ExcludedRemittance" + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);
                printTR(true);
                printTD(true, "IRS World - TRANSACTIONS NOT INCLUDED IN GEFU FILE", "5", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date : " + tranxdate, "5", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "5", "", "");
                printTD(false);
                printTR(false);

                //header

                for (int a = 1; a < 4; a++)
                {
                    printTR(true);
                    for (int b = 1; b < 8; b++)
                    {
                        if (b == 1)
                        {
                            if (a == 3)
                            {
                                printTD(true, "Receipt Mode", "1", "left", "bold");
                                printTD(false);
                            }
                            else
                            {
                                printTD(true);
                                printTD(false);
                            }

                        }
                        else if (b == 2)
                        {
                            if (a == 3)
                            {
                                printTD(true, "Transaction Source", "1", "left", "bold");
                                printTD(false);
                            }
                            else
                            {
                                printTD(true);
                                printTD(false);
                            }
                        }
                        else if (b == 3)
                        {
                            if (a == 3)
                            {
                                printTD(true, "Ccy Rates", "1", "left", "bold");
                                printTD(false);
                            }
                            else
                            {
                                printTD(true);
                                printTD(false);
                            }
                        }
                        else if (b == 4)
                        {
                            if (a == 3)
                            {
                                printTD(true, "Teller", "1", "left", "bold");
                                printTD(false);
                            }
                            else
                            {
                                printTD(true);
                                printTD(false);
                            }
                        }
                        else if (b == 5)
                        {
                            if (a == 3)
                            {
                                printTD(true, "Reference No", "1", "left", "bold");
                                printTD(false);
                            }
                            else
                            {
                                printTD(true);
                                printTD(false);
                            }
                        }
                        else if (b == 6)
                        {
                            if (a == 3)
                            {
                                printTD(true, "Transaction Status", "1", "left", "bold");
                                printTD(false);
                            }
                            else
                            {
                                printTD(true);
                                printTD(false);
                            }
                        }
                        else
                        {
                            if (a == 1)
                            {
                                for (int c = 1; c <= strAmountPerAmt.GetUpperBound(0); c++)
                                {
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true, strCcyNo[c - 1], "1", "center", "bold");
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                }
                            }
                            else if (a == 2)
                            {
                                for (int c = 1; c <= strAmountPerAmt.GetUpperBound(0); c++)
                                {
                                    printTD(true, "Local Payment", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "NetRemitBen", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Local Courier", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "OPE", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Documentary", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Local Cable", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Local Service", "1", "center", "bold");
                                    printTD(false);
                                }
                            }
                            else // 3
                            {
                                for (int c = 1; c <= strAmountPerAmt.GetUpperBound(0); c++)
                                {
                                    printTD(true, "Amount", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Amount", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Charges", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Amount", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Stamp", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Charges", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true, "Fee", "1", "center", "bold");
                                    printTD(false);
                                }
                            }
                        }
                    }
                    printTR(false);
                }

                //starts of extraction for (int r = 0; r < 3; r++)

                bool blnPrintedReceiptMode = false;
                bool blnPrintedTransactionSource = false;
                bool blnPrintedCurrencyRates = false;
                bool blnPrintedTeller = false;

                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    printTR(true);

                    for (int a = 1; a < 8; a++)
                    {
                        if (a == 1 && blnPrintedReceiptMode == false)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["receiptmode"].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else if (a == 2 && blnPrintedTransactionSource == false)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["tranxsource"].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else if (a == 3 && blnPrintedCurrencyRates == false)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["ccyrates"].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else if (a == 4 && blnPrintedTeller == false)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["teller"].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else if (a == 5)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["referenceno"].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else if (a == 6)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["tranxstatus"].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else if (a > 6)
                        {
                            for (int b = 1; b <= strAmountPerAmt.GetUpperBound(0); b++)
                            {
                                if (strAmountPerAmt[b, 0].ToString() == myDataset.Tables[0].Rows[r]["Nettobeneccy"].ToString())
                                {
                                    printTD(true, myDataset.Tables[0].Rows[r]["gross"].ToString(), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["net"].ToString(), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["commission"].ToString(), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["ope"].ToString(), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["docstamp"].ToString(), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["cable"].ToString(), "1", "right", "normal");
                                    printTD(false);
                                    printTD(true, myDataset.Tables[0].Rows[r]["servicefee"].ToString(), "1", "normal", "normal");
                                    printTD(false);

                                    strAmountPerAmt.SetValue((Convert.ToDecimal(strAmountPerAmt[b, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["gross"])).ToString(), b, 1);
                                    strAmountPerAmt.SetValue((Convert.ToDecimal(strAmountPerAmt[b, 2]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["net"])).ToString(), b, 2);
                                    strAmountPerAmt.SetValue((Convert.ToDecimal(strAmountPerAmt[b, 3]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["commission"])).ToString(), b, 3);
                                    strAmountPerAmt.SetValue((Convert.ToDecimal(strAmountPerAmt[b, 4]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["ope"])).ToString(), b, 4);
                                    strAmountPerAmt.SetValue((Convert.ToDecimal(strAmountPerAmt[b, 5]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["docstamp"])).ToString(), b, 5);
                                    strAmountPerAmt.SetValue((Convert.ToDecimal(strAmountPerAmt[b, 6]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["cable"])).ToString(), b, 6);
                                    strAmountPerAmt.SetValue((Convert.ToDecimal(strAmountPerAmt[b, 7]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["servicefee"])).ToString(), b, 7);


                                    strGrandTotalAmountPerAmt.SetValue((Convert.ToDecimal(strGrandTotalAmountPerAmt[b, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["gross"])).ToString(), b, 1);
                                    strGrandTotalAmountPerAmt.SetValue((Convert.ToDecimal(strGrandTotalAmountPerAmt[b, 2]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["net"])).ToString(), b, 2);
                                    strGrandTotalAmountPerAmt.SetValue((Convert.ToDecimal(strGrandTotalAmountPerAmt[b, 3]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["commission"])).ToString(), b, 3);
                                    strGrandTotalAmountPerAmt.SetValue((Convert.ToDecimal(strGrandTotalAmountPerAmt[b, 4]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["ope"])).ToString(), b, 4);
                                    strGrandTotalAmountPerAmt.SetValue((Convert.ToDecimal(strGrandTotalAmountPerAmt[b, 5]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["docstamp"])).ToString(), b, 5);
                                    strGrandTotalAmountPerAmt.SetValue((Convert.ToDecimal(strGrandTotalAmountPerAmt[b, 6]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["cable"])).ToString(), b, 6);
                                    strGrandTotalAmountPerAmt.SetValue((Convert.ToDecimal(strGrandTotalAmountPerAmt[b, 7]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["servicefee"])).ToString(), b, 7);
                                }
                                else
                                {
                                    for (int z = 1; z < 8; z++)
                                    {
                                        printTD(true, "0.00", "1", "right", "normal");
                                        printTD(false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            printTD(true, "", "1", "left", "bold");
                            printTD(false);
                        }
                    }
                    printTR(false);


                    if (r != myDataset.Tables[0].Rows.Count - 1 &&
                        myDataset.Tables[0].Rows[r]["receiptmode"].ToString() != myDataset.Tables[0].Rows[r + 1]["receiptmode"].ToString())
                    {
                        blnPrintedReceiptMode = false;
                        blnPrintedTransactionSource = false;
                        blnPrintedCurrencyRates = false;
                        blnPrintedTeller = false;
                    }
                    else
                    {
                        if (r != myDataset.Tables[0].Rows.Count - 1 &&
                            myDataset.Tables[0].Rows[r]["tranxsource"].ToString() != myDataset.Tables[0].Rows[r + 1]["tranxsource"].ToString())
                        {
                            blnPrintedTransactionSource = false;
                            blnPrintedCurrencyRates = false;
                            blnPrintedTeller = false;
                        }
                        else
                        {
                            if (r != myDataset.Tables[0].Rows.Count - 1 &&
                                myDataset.Tables[0].Rows[r]["ccyrates"].ToString() != myDataset.Tables[0].Rows[r + 1]["ccyrates"].ToString())
                            {
                                blnPrintedCurrencyRates = false;
                                blnPrintedTeller = false;
                            }
                            else
                            {
                                if (r != myDataset.Tables[0].Rows.Count - 1 &&
                                    myDataset.Tables[0].Rows[r]["teller"].ToString() != myDataset.Tables[0].Rows[r + 1]["teller"].ToString())
                                {
                                    blnPrintedTeller = false;
                                }
                                else
                                {
                                    blnPrintedTeller = true;
                                }
                                blnPrintedCurrencyRates = true;
                            }
                            blnPrintedTransactionSource = true;
                        }
                        blnPrintedReceiptMode = true;
                    }

                    if (blnPrintedReceiptMode == false || r == myDataset.Tables[0].Rows.Count - 1)
                    {
                        printTR(true);
                        printTD(true);
                        printTD(false);
                        printTD(true, "Receipt Mode Total", "5", "left", "bold");
                        printTD(false);
                        for (int a = 1; a <= strAmountPerAmt.GetUpperBound(0); a++)
                        {
                            for (int b = 1; b <= strAmountPerAmt.GetUpperBound(1); b++)
                            {
                                if (strAmountPerAmt[a, b].ToString() == "")
                                {
                                    printTD(true, "0.00", "1", "right", "bold");
                                }
                                else
                                {
                                    printTD(true, strAmountPerAmt[a, b].ToString(), "1", "right", "bold");
                                }
                                printTD(false);
                            }
                        }
                        printTR(false);

                        for (int a = 1; a <= strAmountPerAmt.GetUpperBound(0); a++)
                        {
                            for (int b = 1; b <= strAmountPerAmt.GetUpperBound(1); b++)
                            {
                                strAmountPerAmt.SetValue("0", a, b);
                            }
                        }

                        blnPrintedReceiptMode = false;
                        blnPrintedTransactionSource = false;
                        blnPrintedCurrencyRates = false;
                        blnPrintedTeller = false;
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1)
                    {
                        printTR(true);
                        printTD(true, "Grand Total", "6", "left", "bold");
                        printTD(false);
                        for (int a = 1; a <= strGrandTotalAmountPerAmt.GetUpperBound(0); a++)
                        {
                            for (int b = 1; b <= strGrandTotalAmountPerAmt.GetUpperBound(1); b++)
                            {
                                if (strGrandTotalAmountPerAmt[a, b].ToString() == "")
                                {
                                    printTD(true, "0.00", "1", "right", "bold");
                                }
                                else
                                {
                                    printTD(true, strGrandTotalAmountPerAmt[a, b].ToString(), "1", "right", "bold");
                                }
                                printTD(false);
                            }
                        }
                        printTR(false);
                    }
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch //(Exception ex) // commented to remove warning
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "ExcludedRemittance" + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\ExcludedRemittance" + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Consolidated_GLCodes_Report(string tranxdate, string locationcode, string tranxstatus)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();
            string strCOSO = "";
            string strHeader = "";

            string[,] strAmountPerTranx;
            decimal decAmountPerGLCode = 0;
            decimal decAmountPerCcy = 0;

            string[] strCcyNo;
            string[] tempTranxSource;
            string strCcy;

            if (tranxstatus == "Credited Online")
            {
                tranxstatus = "'2, 273'"; //17'";
                strCOSO = "CO";
                strHeader = "Credited Online";
            }
            else
            {
                tranxstatus = "'99, 5'"; // "'5,16,99'";
                strCOSO = "PG";
                strHeader = "Processed by GRS";
            }
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_summary_by_gl_report '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }
            else
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_summary_by_gl_report_london '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }

            myDataset = commons.getDataSet(sqlcommands, "gl_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            tempTranxSource = new string[myDataset.Tables[0].Rows.Count];

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                tempTranxSource.SetValue(myDataset.Tables[0].Rows[row]["TranxSource"].ToString(), row);
            }

            Array.Sort(tempTranxSource);
            string tempArrayValue;
            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempTranxSource.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempTranxSource[i]) && tempTranxSource[i] != "" && tempTranxSource[i] != null)
                {
                    strCcy += tempTranxSource[i] + "~";
                }
                tempArrayValue = tempTranxSource[i];
            }

            strCcyNo = strCcy.Split('~');
            strAmountPerTranx = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strAmountPerTranx.SetValue(strCcyNo[i], i + 1, 0);
            }

            try
            {
                objwriter = new StreamWriter(strFolder + "\\ConsolidatedGLCodes" + strCOSO + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);
                printTR(true);
                printTD(true, "IRS World - CONSOLIDATION-(by GLCodes - " + strHeader + ")", "5", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date : " + tranxdate, "5", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "5", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 1; a < 6; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Account Description", "1", "left", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "IFlex GL Code", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "Transaction Source", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "Currency", "1", "center", "bold");
                    }
                    else if (a == 5)
                    {
                        printTD(true, "Amount (Gross)", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);


                //starts of extraction for (int r = 0; r < 3; r++)

                bool blnPrintGL = false;

                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    for (int index = 1; index <= strAmountPerTranx.GetUpperBound(0); index++)
                    {
                        if (myDataset.Tables[0].Rows[r]["TranxSource"].ToString() == strAmountPerTranx[index, 0])
                        {
                            strAmountPerTranx.SetValue(Convert.ToString(Convert.ToDecimal(strAmountPerTranx[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"])), index, 1);
                        }
                    }

                    decAmountPerGLCode += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"]);
                    decAmountPerCcy += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"]);

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["GLCode"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["GLCode"].ToString().Trim() ||
                        myDataset.Tables[0].Rows[r]["TranxSource"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["TranxSource"].ToString().Trim())
                    {

                        for (int i = 1; i <= strAmountPerTranx.GetUpperBound(0); i++)
                        {
                            if (Convert.ToDecimal(strAmountPerTranx[i, 1]) != 0)
                            {
                                printTR(true);
                                if (blnPrintGL == false)
                                {
                                    printTD(true, myDataset.Tables[0].Rows[r]["Description"].ToString(), "1", "left", "normal");
                                }
                                else
                                {
                                    printTD(true);
                                }
                                printTD(false);

                                if (blnPrintGL == false)
                                {
                                    printTD(true, myDataset.Tables[0].Rows[r]["GLCode"].ToString(), "1", "center", "normal");
                                }
                                else
                                {
                                    printTD(true);
                                }
                                printTD(false);

                                printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString(), "1", "right", "normal");
                                printTD(false);

                                if (blnPrintGL == false)
                                {
                                    printTD(true, myDataset.Tables[0].Rows[r]["acct_currency"].ToString(), "1", "center", "normal");
                                }
                                else
                                {
                                    printTD(true);
                                }
                                printTD(false);

                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strAmountPerTranx[i, 1])), "1", "right", "normal");
                                printTD(false);

                                printTR(false);
                                blnPrintGL = true;
                            }
                        }

                        for (int i = 1; i <= strAmountPerTranx.GetUpperBound(0); i++)
                        {
                            strAmountPerTranx.SetValue("0", i, 1);
                        }

                        if (r == myDataset.Tables[0].Rows.Count - 1 ||
                            myDataset.Tables[0].Rows[r]["GLCode"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["GLCode"].ToString().Trim())
                        {
                            blnPrintGL = false;
                            printTR(true);
                            printTR(false);

                            //print total per glcode
                            printTR(true);

                            printTD(true, "Total " + myDataset.Tables[0].Rows[r]["Description"].ToString() + " Transactions", "1", "left", "bold");
                            printTD(false);

                            printTD(true, "", "3", "left", "bold");
                            printTD(false);

                            printTD(true, String.Format("{0:0,0.000000000}", decAmountPerGLCode), "1", "right", "bold");
                            printTD(false);

                            printTR(false);

                            printTR(true);
                            printTR(false);
                            decAmountPerGLCode = 0;

                        }
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1 ||
                        myDataset.Tables[0].Rows[r]["acct_currency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["acct_currency"].ToString().Trim())
                    {
                        printTR(true);

                        printTD(true, "Total " + myDataset.Tables[0].Rows[r]["acct_currency"].ToString() + " Transactions", "1", "left", "bold");
                        printTD(false);

                        printTD(true, "", "3", "left", "bold");
                        printTD(false);

                        printTD(true, String.Format("{0:0,0.000000000}", decAmountPerCcy), "1", "right", "bold");
                        printTD(false);

                        printTR(false);
                        decAmountPerCcy = 0;

                        printTR(true);
                        printTR(false);
                    }
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch //(Exception ex) // commented to remove warning
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "ConsolidatedGLCodes" + strCOSO + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\ConsolidatedGLCodes" + strCOSO + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Summary_Report(string tranxdate, string locationcode, string tranxstatus)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();

            string strCOSO = "";
            string strHeader = "";
            string strCOSOBody = "";


            string[] tempArraybankcom;
            string[] tempArrayLocalCable;
            string[] tempArrayLocalServiceFee;
            string[] tempArrayOPE;
            string[] tempArrayDocStamps;
            string tempArrayValue;
            string strCcy;
            string[] strCcyNo;
            string[,] strbankcom;
            string[,] strLocalCable;
            string[,] strLocalServiceFee;
            string[,] strOPE;
            string[,] strDocStamps;

            string[] tempArrayDebitAmountCcy;
            string[,] strDebitAmount;
            decimal decCreditAmount = 0;
            decimal decDebitAmountBase = 0;
            decimal decCreditAmountBase = 0;

            if (tranxstatus == "Credited Online")
            {
                tranxstatus = "'2, 273'";//17'";
                strCOSO = "CO";
                strHeader = "Credited Online";
                strCOSOBody = "OL";
            }
            else
            {
                tranxstatus = "'99,5'"; // "'5,16,99'";
                strCOSO = "PG";
                strHeader = "Processed by GRS";
                strCOSOBody = "NOL";
            }
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }
            if (locationcode == "ANB")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report_anb '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }

            else if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }
            else
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report_london '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }
            myDataset = commons.getDataSet(sqlcommands, "tranxsource_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            tempArrayDebitAmountCcy = new string[myDataset.Tables[0].Rows.Count];
            tempArraybankcom = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalCable = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalServiceFee = new string[myDataset.Tables[0].Rows.Count];
            tempArrayOPE = new string[myDataset.Tables[0].Rows.Count];
            tempArrayDocStamps = new string[myDataset.Tables[0].Rows.Count];

            int intDebitAmountCcy = 0;
            int intbankcomindex = 0;
            int intLocalCableindex = 0;
            int intLocalServiceFeeindex = 0;
            int intOPEindex = 0;
            int intDocStampsindex = 0;
            decimal decTotalSuspenseAmount = 0;
            decimal decBaseTotalCreditAmount = 0;
            decimal decBaseTotalDebitAmount = 0;

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "1 for gross")
                {
                    tempArrayDebitAmountCcy.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intDebitAmountCcy);
                    intDebitAmountCcy += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "3 for Bank Com")
                {
                    tempArraybankcom.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intbankcomindex);
                    intbankcomindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "4 for Local Cable")
                {
                    tempArrayLocalCable.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalCableindex);
                    intLocalCableindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "5 for Local Service Fee")
                {
                    tempArrayLocalServiceFee.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalServiceFeeindex);
                    intLocalServiceFeeindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "6 for OPE")
                {
                    tempArrayOPE.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intOPEindex);
                    intOPEindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "7 for Doc Stamps")
                {
                    tempArrayDocStamps.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intDocStampsindex);
                    intDocStampsindex += 1;
                }
            }

            Array.Sort(tempArrayDebitAmountCcy);
            Array.Sort(tempArraybankcom);
            Array.Sort(tempArrayLocalCable);
            Array.Sort(tempArrayLocalServiceFee);
            Array.Sort(tempArrayOPE);
            Array.Sort(tempArrayDocStamps);

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArraybankcom.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArraybankcom[i]) && tempArraybankcom[i] != "" && tempArraybankcom[i] != null)
                {
                    strCcy += tempArraybankcom[i] + "~";
                }
                tempArrayValue = tempArraybankcom[i];
            }

            strCcyNo = strCcy.Split('~');
            strbankcom = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strbankcom.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalCable.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalCable[i]) && tempArrayLocalCable[i] != "" && tempArrayLocalCable[i] != null)
                {
                    strCcy += tempArrayLocalCable[i] + "~";
                }
                tempArrayValue = tempArrayLocalCable[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalCable = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalCable.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalServiceFee.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalServiceFee[i]) && tempArrayLocalServiceFee[i] != "" && tempArrayLocalServiceFee[i] != null)
                {
                    strCcy += tempArrayLocalServiceFee[i] + "~";
                }
                tempArrayValue = tempArrayLocalServiceFee[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalServiceFee = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalServiceFee.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayOPE.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayOPE[i]) && tempArrayOPE[i] != "" && tempArrayOPE[i] != null)
                {
                    strCcy += tempArrayOPE[i] + "~";
                }
                tempArrayValue = tempArrayOPE[i];
            }

            strCcyNo = strCcy.Split('~');
            strOPE = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strOPE.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayDocStamps.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayDocStamps[i]) && tempArrayDocStamps[i] != "" && tempArrayDocStamps[i] != null)
                {
                    strCcy += tempArrayDocStamps[i] + "~";
                }
                tempArrayValue = tempArrayDocStamps[i];
            }

            strCcyNo = strCcy.Split('~');
            strDocStamps = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strDocStamps.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayDebitAmountCcy.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayDebitAmountCcy[i]) && tempArrayDebitAmountCcy[i] != "" && tempArrayDebitAmountCcy[i] != null)
                {
                    strCcy += tempArrayDebitAmountCcy[i] + "~";
                }
                tempArrayValue = tempArrayDebitAmountCcy[i];
            }

            strCcyNo = strCcy.Split('~');
            strDebitAmount = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strDebitAmount.SetValue(strCcyNo[i], i + 1, 0);
            }

            try
            {
                objwriter = new StreamWriter(strFolder + "\\Summary" + strCOSO + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);
                printTR(true);
                printTD(true, "IRS World - SUMMARY-(Total Amount - " + strHeader + ")", "15", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date : " + tranxdate, "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 1; a < 7; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, locationcode, "1", "left", "bold");
                    }
                    else if (a == 2 || a == 3 || a == 4)
                    {
                        printTD(true);
                    }
                    else if (a == 5)
                    {
                        printTD(true, "ORIGINAL/TARGET AMT", "2", "center", "bold");
                    }
                    else
                    {
                        printTD(true, "BASE EQUIVALENT (" + myDataset.Tables[0].Rows[0]["LocalCurrency"].ToString() + ")", "2", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 9; a++)
                {
                    if (a == 1 || a == 2 || a == 3)
                    {
                        printTD(true);
                    }
                    else if (a == 4)
                    {
                        printTD(true, "ACCT", "1", "center", "bold");
                    }
                    else if (a == 5 || a == 7)
                    {
                        printTD(true, "Debit", "1", "center", "bold");
                    }
                    else if (a == 6 || a == 8)
                    {
                        printTD(true, "Credit", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 9; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Transaction Source", "1", "center", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "IRS Reference No.", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "GL Code", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "CCY", "1", "center", "bold");
                    }
                    else
                    {
                        printTD(true, "Amount", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                //starts of extraction for (int r = 0; r < 3; r++)

                bool blnDebit = true;
                decimal decPRX = 0;
                decimal phpTotal = 0;

                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "1 for gross")
                    {
                        for (int index = 1; index <= strDebitAmount.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strDebitAmount[index, 0])
                            {
                                strDebitAmount.SetValue(Convert.ToString(Convert.ToDecimal(strDebitAmount[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_amt"])), index, 1);
                            }
                        }
                        decDebitAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "2 for net")
                    {
                        decCreditAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                        decTotalSuspenseAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "3 for Bank Com")
                    {
                        for (int index = 1; index <= strbankcom.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strbankcom[index, 0])
                            {
                                strbankcom.SetValue(Convert.ToString(Convert.ToDecimal(strbankcom[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "4 for Local Cable")
                    {
                        for (int index = 1; index <= strLocalCable.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalCable[index, 0])
                            {
                                strLocalCable.SetValue(Convert.ToString(Convert.ToDecimal(strLocalCable[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "5 for Local Service Fee")
                    {
                        for (int index = 1; index <= strLocalServiceFee.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalServiceFee[index, 0])
                            {
                                strLocalServiceFee.SetValue(Convert.ToString(Convert.ToDecimal(strLocalServiceFee[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "6 for OPE")
                    {
                        for (int index = 1; index <= strOPE.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strOPE[index, 0])
                            {
                                strOPE.SetValue(Convert.ToString(Convert.ToDecimal(strOPE[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "7 for Doc Stamps")
                    {
                        for (int index = 1; index <= strDocStamps.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strDocStamps[index, 0])
                            {
                                strDocStamps.SetValue(Convert.ToString(Convert.ToDecimal(strDocStamps[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                        decCreditAmountBase += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                    }

                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "1 for gross")
                    {
                        blnDebit = true;
                    }
                    else
                    {
                        blnDebit = false;
                    }

                    printTR(true);
                    for (int col = 1; col < 9; col++)
                    {
                        if (col == 1)
                        {
                            if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "1 for gross")
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString(), "1", "left", "normal");
                            }
                            else if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "2 for net")
                            {
                                printTD(true, "Suspense Remittance GL", "1", "left", "normal"); //printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString() + " " + strCOSOBody, "1", "left", "normal");
                            }
                            else
                            {
                                printTD(true, "Service Charge/Comminsion Main", "1", "left", "normal");//printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString() + " " + strCOSOBody + " " + myDataset.Tables[0].Rows[r]["remarks"].ToString().Substring(6, myDataset.Tables[0].Rows[r]["remarks"].ToString().Length - 6), "1", "left", "normal");
                            }
                        }
                        else if (col == 2)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["ReferenceNo"].ToString(), "1", "left", "normal");
                        }
                        else if (col == 3)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["GLCode"].ToString(), "1", "left", "normal");
                        }
                        else if (col == 4)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["acct_currency"].ToString(), "1", "left", "normal");
                        }
                        else if (col == 5)
                        {
                            if (blnDebit == true)
                            {
                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_amt"])), "1", "right", "normal");
                            }
                            else
                            {
                                printTD(true);
                            }
                        }
                        else if (col == 6)
                        {
                            if (blnDebit == true)
                            {
                                printTD(true);
                            }
                            else
                            {
                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), "1", "right", "normal");
                            }
                        }
                        else if (col == 7)
                        {
                            if (blnDebit == true)
                            {
                                //divide debit amount to debitamt/localccy created by mike 06/02/2015
                                //new sqlHelper().execScalar(""
                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"])), "1", "right", "normal");
                                decPRX = Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                                decBaseTotalDebitAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                            }
                            else
                            {
                                printTD(true);
                            }
                        }
                        else if (col == 8)
                        {
                            if (blnDebit == true)
                            {
                                printTD(true);
                            }
                            else
                            {
                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"])), "1", "right", "normal");
                                decPRX -= Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                                decBaseTotalCreditAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                            }
                        }
                        printTD(false);
                    }
                    printTR(false);

                    bool blnNextRow = false;

                    if (r == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[r]["TranxSource"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["TranxSource"].ToString().Trim() || myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim())
                    {
                        printTR(true);
                        printTR(false);
                        for (int i = 1; i <= strDebitAmount.GetUpperBound(0); i++)
                        {
                            if (Convert.ToDecimal(strDebitAmount[i, 1]) != 0)
                            {
                                if (blnNextRow == false)
                                {
                                    printTR(true);
                                    printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString(), "4", "left", "bold");
                                    printTD(false);
                                    printTD(true, "(" + strDebitAmount[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmount[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmount), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decDebitAmountBase), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", decCreditAmountBase), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                    blnNextRow = true;
                                }
                                else
                                {
                                    printTR(true);
                                    printTD(true, "", "4", "left", "bold");
                                    printTD(false);
                                    printTD(true, "(" + strDebitAmount[i, 0] + ")" + String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDebitAmount[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTD(true, "", "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        for (int i = 1; i <= strDebitAmount.GetUpperBound(0); i++)
                        {
                            strDebitAmount.SetValue("0", i, 1);
                        }
                        decCreditAmount = 0;
                        decDebitAmountBase = 0;
                        decCreditAmountBase = 0;

                        printTR(true);
                        printTR(false);
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim())
                    {
                        printTR(true);
                        printTD(true, "SUB-TOTALS - SUSPENSE - " + myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "8", "left", "bold");
                        printTD(false);
                        printTR(false);

                        printTR(true);
                        printTD(true, "  SUSPENSE - (" + strHeader.ToUpper() + ")", "3", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "2", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decTotalSuspenseAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);
                        if (myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString() == "PHP")
                        {
                            phpTotal += decTotalSuspenseAmount;
                        }


                        decTotalSuspenseAmount = 0;

                        //SUB-TOTALS REM CHARGES (local courier)
                        if (strbankcom.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strbankcom[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local courier)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strbankcom[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strbankcom[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalCable.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalCable[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local cable)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalCable[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strLocalCable[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalServiceFee.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalServiceFee[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local service fee)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalServiceFee[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strLocalServiceFee[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strOPE.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strOPE[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Out-of-pocket Expense)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strOPE[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strOPE[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strDocStamps.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strDocStamps[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Documentary Stamp)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDocStamps[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(strDocStamps[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                        {
                            strbankcom.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                        {
                            strLocalCable.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                        {
                            strLocalServiceFee.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                        {
                            strOPE.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                        {
                            strDocStamps.SetValue("0", i, 1);
                        }

                        printTR(true);
                        printTR(false);

                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1)
                    {
                        //printTR(true);
                        //printTR(false);
                        //printTR(true);
                        //printTD(true, "Total", "6", "left", "bold");
                        //printTD(false);
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount), "1", "right", "bold");
                        //printTD(false);
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount), "1", "right", "bold");
                        //printTD(false);
                        //printTR(false);
                        printTR(true);
                        printTR(false);
                        printTR(true);
                        printTD(true, "PHP Total", "4", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", phpTotal), "1", "right", "bold");
                        printTD(false);
                        printTD(false);
                        printTD(false);


                        printTR(true);
                        printTD(true, "PRX", "2", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["PRXGLCode"].ToString(), "1", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["LocalCurrency"].ToString(), "3", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount), "1", "right", "bold");
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalDebitAmount - decBaseTotalCreditAmount), "1", "right", "bold");
                        //printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount - decTotalSuspenseAmount), "1", "right", "bold");  //modify by mike to get proper PRX 06/02/2015
                        printTD(true, String.Format("{0:0,0.000000000}", decBaseTotalCreditAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);
                    }
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch //(Exception ex) // commented to remove warning
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "Summary" + strCOSO + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\Summary" + strCOSO + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Preliminary_Report(string tranxdate, string locationcode, string tranxstatus)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();

            string strCOSO = "";
            string strHeader = "";
            string strCOSOBody = "";


            string[] tempArraybankcom;
            string[] tempArrayLocalCable;
            string[] tempArrayLocalServiceFee;
            string[] tempArrayOPE;
            string[] tempArrayDocStamps;
            string tempArrayValue;
            string strCcy;
            string[] strCcyNo;
            string[,] strbankcom;
            string[,] strLocalCable;
            string[,] strLocalServiceFee;
            string[,] strOPE;
            string[,] strDocStamps;

            if (tranxstatus == "Credited Online")
            {
                tranxstatus = "'2, 273'";//17'";
                strCOSO = "CO";
                strHeader = "Credited Online";
                strCOSOBody = "OL";
            }
            else
            {
                tranxstatus = "'99,5'"; // "'5,16,99'";
                strCOSO = "PG";
                strHeader = "Processed by GRS";
                strCOSOBody = "NOL";
            }
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }
            if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }
            else
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_preliminary_summary_report_london '" + locationcode + "', '" + tranxdate + "', " + tranxstatus + "", false);
            }

            myDataset = commons.getDataSet(sqlcommands, "tranxsource_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            tempArraybankcom = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalCable = new string[myDataset.Tables[0].Rows.Count];
            tempArrayLocalServiceFee = new string[myDataset.Tables[0].Rows.Count];
            tempArrayOPE = new string[myDataset.Tables[0].Rows.Count];
            tempArrayDocStamps = new string[myDataset.Tables[0].Rows.Count];

            int intbankcomindex = 0;
            int intLocalCableindex = 0;
            int intLocalServiceFeeindex = 0;
            int intOPEindex = 0;
            int intDocStampsindex = 0;
            decimal decTotalSuspenseAmount = 0;
            decimal decBaseTotalCreditAmount = 0;
            decimal decBaseTotalDebitAmount = 0;

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "3 for Bank Com")
                {
                    tempArraybankcom.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intbankcomindex);
                    intbankcomindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "4 for Local Cable")
                {
                    tempArrayLocalCable.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalCableindex);
                    intLocalCableindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "5 for Local Service Fee")
                {
                    tempArrayLocalServiceFee.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intLocalServiceFeeindex);
                    intLocalServiceFeeindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "6 for OPE")
                {
                    tempArrayOPE.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intOPEindex);
                    intOPEindex += 1;
                }
                else if (myDataset.Tables[0].Rows[row]["remarks"].ToString() == "7 for Doc Stamps")
                {
                    tempArrayDocStamps.SetValue(myDataset.Tables[0].Rows[row]["acct_currency"].ToString(), intDocStampsindex);
                    intDocStampsindex += 1;
                }
            }

            Array.Sort(tempArraybankcom);
            Array.Sort(tempArrayLocalCable);
            Array.Sort(tempArrayLocalServiceFee);
            Array.Sort(tempArrayOPE);
            Array.Sort(tempArrayDocStamps);

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArraybankcom.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArraybankcom[i]) && tempArraybankcom[i] != "" && tempArraybankcom[i] != null)
                {
                    strCcy += tempArraybankcom[i] + "~";
                }
                tempArrayValue = tempArraybankcom[i];
            }

            strCcyNo = strCcy.Split('~');
            strbankcom = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strbankcom.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalCable.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalCable[i]) && tempArrayLocalCable[i] != "" && tempArrayLocalCable[i] != null)
                {
                    strCcy += tempArrayLocalCable[i] + "~";
                }
                tempArrayValue = tempArrayLocalCable[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalCable = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalCable.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayLocalServiceFee.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayLocalServiceFee[i]) && tempArrayLocalServiceFee[i] != "" && tempArrayLocalServiceFee[i] != null)
                {
                    strCcy += tempArrayLocalServiceFee[i] + "~";
                }
                tempArrayValue = tempArrayLocalServiceFee[i];
            }

            strCcyNo = strCcy.Split('~');
            strLocalServiceFee = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strLocalServiceFee.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayOPE.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayOPE[i]) && tempArrayOPE[i] != "" && tempArrayOPE[i] != null)
                {
                    strCcy += tempArrayOPE[i] + "~";
                }
                tempArrayValue = tempArrayOPE[i];
            }

            strCcyNo = strCcy.Split('~');
            strOPE = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strOPE.SetValue(strCcyNo[i], i + 1, 0);
            }

            tempArrayValue = "";
            strCcy = "";
            for (int i = 0; i <= tempArrayDocStamps.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArrayDocStamps[i]) && tempArrayDocStamps[i] != "" && tempArrayDocStamps[i] != null)
                {
                    strCcy += tempArrayDocStamps[i] + "~";
                }
                tempArrayValue = tempArrayDocStamps[i];
            }

            strCcyNo = strCcy.Split('~');
            strDocStamps = new string[strCcyNo.Length, 2];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                strDocStamps.SetValue(strCcyNo[i], i + 1, 0);
            }

            //int rowcount = 0;
            try
            {
                objwriter = new StreamWriter(strFolder + "\\PreliminarySummary" + strCOSO + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);
                printTR(true);
                printTD(true, "IRS World - PRELIMINARY SUMMARY-(Total Amount - " + strHeader + ")", "15", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date :" + tranxdate, "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 1; a < 7; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, locationcode, "1", "left", "bold");
                    }
                    else if (a == 2 || a == 3 || a == 4)
                    {
                        printTD(true);
                    }
                    else if (a == 5)
                    {
                        printTD(true, "ORIGINAL/TARGET AMT", "2", "center", "bold");
                    }
                    else
                    {
                        printTD(true, "BASE EQUIVALENT (" + myDataset.Tables[0].Rows[0]["LocalCurrency"].ToString() + ")", "2", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 9; a++)
                {
                    if (a == 1 || a == 2 || a == 3)
                    {
                        printTD(true);
                    }
                    else if (a == 4)
                    {
                        printTD(true, "ACCT", "1", "center", "bold");
                    }
                    else if (a == 5 || a == 7)
                    {
                        printTD(true, "Debit", "1", "center", "bold");
                    }
                    else if (a == 6 || a == 8)
                    {
                        printTD(true, "Credit", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                printTR(true);
                for (int a = 1; a < 9; a++)
                {
                    if (a == 1)
                    {
                        printTD(true, "Transaction Source", "1", "center", "bold");
                    }
                    else if (a == 2)
                    {
                        printTD(true, "IRS Reference No.", "1", "center", "bold");
                    }
                    else if (a == 3)
                    {
                        printTD(true, "GL Code", "1", "center", "bold");
                    }
                    else if (a == 4)
                    {
                        printTD(true, "CCY", "1", "center", "bold");
                    }
                    else
                    {
                        printTD(true, "Amount", "1", "center", "bold");
                    }
                    printTD(false);
                }
                printTR(false);

                //starts of extraction for (int r = 0; r < 3; r++)

                bool blnDebit = true;
                decimal decPRX = 0;


                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {

                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "2 for net")
                    {
                        decTotalSuspenseAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"]);
                    }

                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "3 for Bank Com")
                    {
                        for (int index = 1; index <= strbankcom.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strbankcom[index, 0])
                            {
                                strbankcom.SetValue(Convert.ToString(Convert.ToDecimal(strbankcom[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "4 for Local Cable")
                    {
                        for (int index = 1; index <= strLocalCable.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalCable[index, 0])
                            {
                                strLocalCable.SetValue(Convert.ToString(Convert.ToDecimal(strLocalCable[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "5 for Local Service Fee")
                    {
                        for (int index = 1; index <= strLocalServiceFee.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strLocalServiceFee[index, 0])
                            {
                                strLocalServiceFee.SetValue(Convert.ToString(Convert.ToDecimal(strLocalServiceFee[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "6 for OPE")
                    {
                        for (int index = 1; index <= strOPE.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strOPE[index, 0])
                            {
                                strOPE.SetValue(Convert.ToString(Convert.ToDecimal(strOPE[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                    }
                    else if (myDataset.Tables[0].Rows[r]["remarks"].ToString() == "7 for Doc Stamps")
                    {
                        for (int index = 1; index <= strDocStamps.GetUpperBound(0); index++)
                        {
                            if (myDataset.Tables[0].Rows[r]["acct_currency"].ToString() == strDocStamps[index, 0])
                            {
                                strDocStamps.SetValue(Convert.ToString(Convert.ToDecimal(strDocStamps[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), index, 1);
                            }
                        }
                    }

                    if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "1 for gross")
                    {
                        blnDebit = true;
                    }
                    else
                    {
                        blnDebit = false;
                    }

                    printTR(true);
                    for (int col = 1; col < 9; col++)
                    {
                        if (col == 1)
                        {
                            if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "1 for gross")
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString(), "1", "left", "normal");
                            }
                            else if (myDataset.Tables[0].Rows[r]["remarks"].ToString().Trim() == "2 for net")
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString() + " " + strCOSOBody, "1", "left", "normal");
                            }
                            else
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["TranxSource"].ToString() + " " + strCOSOBody + " " + myDataset.Tables[0].Rows[r]["remarks"].ToString().Substring(6, myDataset.Tables[0].Rows[r]["remarks"].ToString().Length - 6), "1", "left", "normal");
                            }
                        }
                        else if (col == 2)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["ReferenceNo"].ToString(), "1", "left", "normal");
                        }
                        else if (col == 3)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["GLCode"].ToString(), "1", "left", "normal");
                        }
                        else if (col == 4)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r]["acct_currency"].ToString(), "1", "left", "normal");
                        }
                        else if (col == 5)
                        {
                            if (blnDebit == true)
                            {
                                printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_amt"])), "1", "right", "normal");
                            }
                            else
                            {
                                printTD(true);
                            }
                        }
                        else if (col == 6)
                        {
                            if (blnDebit == true)
                            {
                                printTD(true);
                            }
                            else
                            {
                                printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_amt"])), "1", "right", "normal");
                            }
                        }
                        else if (col == 7)
                        {
                            if (blnDebit == true)
                            {
                                printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"])), "1", "right", "normal");
                                decPRX = Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                                decBaseTotalDebitAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["debit_base_amt"]);
                            }
                            else
                            {
                                printTD(true);
                            }
                        }
                        else if (col == 8)
                        {
                            if (blnDebit == true)
                            {
                                printTD(true);
                            }
                            else
                            {
                                printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"])), "1", "right", "normal");
                                decPRX -= Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                                decBaseTotalCreditAmount += Convert.ToDecimal(myDataset.Tables[0].Rows[r]["credit_base_amt"]);
                            }
                        }
                        printTD(false);
                    }
                    printTR(false);

                    if (r == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[r]["ReferenceNo"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["ReferenceNo"].ToString().Trim())
                    {
                        printTR(true);

                        for (int col = 1; col < 9; col++)
                        {
                            if (col == 1)
                            {
                                printTD(true, "PRX", "1", "left", "bold");
                            }
                            else if (col == 3)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["PRXGLCode"].ToString(), "1", "left", "bold");
                            }
                            else if (col == 4)
                            {
                                printTD(true, myDataset.Tables[0].Rows[r]["LocalCurrency"].ToString(), "1", "left", "bold");
                            }
                            else if (col == 8)
                            {
                                printTD(true, String.Format("{0:0,0.00}", decPRX), "1", "right", "bold");
                            }
                            else
                            {
                                printTD(true);
                            }
                            printTD(false);
                        }
                        printTR(false);
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString().Trim() != myDataset.Tables[0].Rows[r + 1]["NettoBeneCurrency"].ToString().Trim())
                    {
                        printTR(true);
                        printTD(true, "SUB-TOTALS - SUSPENSE - " + myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "8", "left", "bold");
                        printTD(false);
                        printTR(false);

                        printTR(true);
                        printTD(true, "  SUSPENSE - (" + strHeader.ToUpper() + ")", "3", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["NettoBeneCurrency"].ToString(), "2", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.00}", decTotalSuspenseAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);

                        decTotalSuspenseAmount = 0;

                        //SUB-TOTALS REM CHARGES (local courier)
                        if (strbankcom.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strbankcom[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local courier)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strbankcom[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(strbankcom[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalCable.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalCable[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local cable)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalCable[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(strLocalCable[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strLocalServiceFee.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strLocalServiceFee[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (local service fee)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strLocalServiceFee[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(strLocalServiceFee[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strOPE.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strOPE[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Out-of-pocket Expense)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strOPE[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(strOPE[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        if (strDocStamps.GetUpperBound(0) > 0)
                        {
                            for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                            {
                                if (Convert.ToDecimal(strDocStamps[i, 1]) != 0)
                                {
                                    printTR(true);
                                    printTD(true, "SUB-TOTALS REM CHARGES (Documentary Stamp)", "3", "left", "bold");
                                    printTD(false);
                                    printTD(true, strDocStamps[i, 0], "2", "left", "bold");
                                    printTD(false);
                                    printTD(true, String.Format("{0:0,0.00}", Convert.ToDecimal(strDocStamps[i, 1])), "1", "right", "bold");
                                    printTD(false);
                                    printTR(false);
                                }
                            }
                        }

                        for (int i = 1; i <= strbankcom.GetUpperBound(0); i++)
                        {
                            strbankcom.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalCable.GetUpperBound(0); i++)
                        {
                            strLocalCable.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strLocalServiceFee.GetUpperBound(0); i++)
                        {
                            strLocalServiceFee.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strOPE.GetUpperBound(0); i++)
                        {
                            strOPE.SetValue("0", i, 1);
                        }
                        for (int i = 1; i <= strDocStamps.GetUpperBound(0); i++)
                        {
                            strDocStamps.SetValue("0", i, 1);
                        }
                    }

                    if (r == myDataset.Tables[0].Rows.Count - 1)
                    {
                        printTR(true);
                        printTR(false);
                        printTR(true);
                        printTD(true, "Total", "6", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.00}", decBaseTotalDebitAmount), "1", "right", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.00}", decBaseTotalCreditAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);

                        printTR(true);
                        printTD(true, "PRX", "2", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["PRXGLCode"].ToString(), "1", "left", "bold");
                        printTD(false);
                        printTD(true, myDataset.Tables[0].Rows[r]["LocalCurrency"].ToString(), "4", "left", "bold");
                        printTD(false);
                        printTD(true, String.Format("{0:0,0.00}", decBaseTotalDebitAmount - decBaseTotalCreditAmount), "1", "right", "bold");
                        printTD(false);
                        printTR(false);
                    }
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch //(Exception ex) // commented to remove warning
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "PreliminarySummary" + strCOSO + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\PreliminarySummary" + strCOSO + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_TransactionSource_Report(string tranxdate, string locationcode, string transactioncodes)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            string strFolder = "";

            string[] tempArray;
            string tempArrayValue;
            string strlocalpaymentccy;
            string strcommissionccy;
            string strnetremitbenccy;

            string[,] declocalpaymentamt_receiptmode;
            string[,] deccommissionamt_receiptmode;
            string[,] decnetremitbenamt_receiptmode;
            string[,] declocalpaymentamt_tranxstatus;
            string[,] deccommissionamt_tranxstatus;
            string[,] decnetremitbenamt_tranxstatus;
            string[,] declocalpaymentamt_gross;
            string[,] deccommissionamt_gross;
            string[,] decnetremitbenamt_gross;
            string[] strCcyNo;

            bool blnPrintTranxStatusTotal = false;
            bool blnPrintReceiptmodeTotal = false;
            bool blnPrintGrandTotal = false;

            bool blnCreateNewRow = false;
            bool blnLocalPayment = false;
            bool blnCommission = false;
            //bool blnNettobene = false;

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;

            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_TransactionSourceReport '" + tranxdate + "', '" + locationcode + "', " + transactioncodes + "", false);
            }
            else
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_TransactionSourceReport_london '" + tranxdate + "', '" + locationcode + "', " + transactioncodes + "", false);
            }


            myDataset = commons.getDataSet(sqlcommands, "tranxsource_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            int rowcount = 0;
            try
            {
                objwriter = new StreamWriter(strFolder + "\\ReportByTransactionSource" + locationcode + "_" + strSubFolder + ".xls");

                printHTML(true);
                printBody(true);
                printTable(true);

                printTR(true);
                printTD(true, "IRS World - (Transaction Details by Transaction Source)", "15", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Date :" + tranxdate, "15", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "15", "", "");
                printTD(false);
                printTR(false);

                //row for column names
                printTR(true);
                for (int a = 1; a < 15; a++)
                {
                    printTD(true, myDataset.Tables[0].Columns[a].ToString(), "1", "center", "bold");
                    printTD(false);
                }
                printTR(false);

                tempArray = new string[myDataset.Tables[0].Rows.Count];

                tempArrayValue = "";

                strlocalpaymentccy = "";

                strcommissionccy = "";

                strnetremitbenccy = "";


                //getting distinct localpaymentccy
                for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
                {
                    tempArray.SetValue(myDataset.Tables[0].Rows[row]["localpaymentccy"].ToString(), row);
                }
                Array.Sort(tempArray);
                for (int i = 0; i <= tempArray.GetUpperBound(0); i++)
                {
                    if ((i == 0 || tempArrayValue != tempArray[i]) && tempArray[i] != "")
                    {
                        strlocalpaymentccy += tempArray[i] + "~";
                    }

                    tempArrayValue = tempArray[i];
                }

                //getting distinct commissionccy
                for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
                {
                    tempArray.SetValue(myDataset.Tables[0].Rows[row]["commisionccy"].ToString(), row);
                }
                Array.Sort(tempArray);
                for (int i = 0; i <= tempArray.GetUpperBound(0); i++)
                {
                    if ((i == 0 || tempArrayValue != tempArray[i]) && tempArray[i] != "")
                    {
                        strcommissionccy += tempArray[i] + "~";
                    }

                    tempArrayValue = tempArray[i];
                }

                //getting distinct netremitbenccy
                for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
                {
                    tempArray.SetValue(myDataset.Tables[0].Rows[row]["netremitbenccy"].ToString(), row);
                }
                Array.Sort(tempArray);
                for (int i = 0; i <= tempArray.GetUpperBound(0); i++)
                {
                    if ((i == 0 || tempArrayValue != tempArray[i]) && tempArray[i] != "")
                    {
                        strnetremitbenccy += tempArray[i] + "~";
                    }

                    tempArrayValue = tempArray[i];
                }

                strCcyNo = strlocalpaymentccy.Split('~');
                declocalpaymentamt_receiptmode = new string[strCcyNo.Length, 2];
                declocalpaymentamt_tranxstatus = new string[strCcyNo.Length, 2];
                declocalpaymentamt_gross = new string[strCcyNo.Length, 2];

                for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
                {
                    declocalpaymentamt_receiptmode.SetValue(strCcyNo[i], i + 1, 0);
                    declocalpaymentamt_tranxstatus.SetValue(strCcyNo[i], i + 1, 0);
                    declocalpaymentamt_gross.SetValue(strCcyNo[i], i + 1, 0);
                }

                strCcyNo = strcommissionccy.Split('~');
                deccommissionamt_receiptmode = new string[strCcyNo.Length, 2];
                deccommissionamt_tranxstatus = new string[strCcyNo.Length, 2];
                deccommissionamt_gross = new string[strCcyNo.Length, 2];

                for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
                {
                    deccommissionamt_receiptmode.SetValue(strCcyNo[i], i + 1, 0);
                    deccommissionamt_tranxstatus.SetValue(strCcyNo[i], i + 1, 0);
                    deccommissionamt_gross.SetValue(strCcyNo[i], i + 1, 0);
                }

                strCcyNo = strnetremitbenccy.Split('~');
                decnetremitbenamt_receiptmode = new string[strCcyNo.Length, 2];
                decnetremitbenamt_tranxstatus = new string[strCcyNo.Length, 2];
                decnetremitbenamt_gross = new string[strCcyNo.Length, 2];

                for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
                {
                    decnetremitbenamt_receiptmode.SetValue(strCcyNo[i], i + 1, 0);
                    decnetremitbenamt_tranxstatus.SetValue(strCcyNo[i], i + 1, 0);
                    decnetremitbenamt_gross.SetValue(strCcyNo[i], i + 1, 0);
                }

                int intUpperBound = 0;

                if (declocalpaymentamt_receiptmode.GetUpperBound(0) > decnetremitbenamt_receiptmode.GetUpperBound(0))
                {
                    if (declocalpaymentamt_receiptmode.GetUpperBound(0) > deccommissionamt_receiptmode.GetUpperBound(0))
                    {
                        intUpperBound = declocalpaymentamt_receiptmode.GetUpperBound(0);
                    }
                    else
                    {
                        intUpperBound = deccommissionamt_receiptmode.GetUpperBound(0);
                    }
                }
                else
                {
                    if (decnetremitbenamt_receiptmode.GetUpperBound(0) > deccommissionamt_receiptmode.GetUpperBound(0))
                    {
                        intUpperBound = decnetremitbenamt_receiptmode.GetUpperBound(0);
                    }
                    else
                    {
                        intUpperBound = deccommissionamt_receiptmode.GetUpperBound(0);
                    }
                }

                //starts of extraction for (int r = 0; r < 3; r++)
                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    if (r == myDataset.Tables[0].Rows.Count - 1)
                    {
                        //string strhere;
                        //strhere = "";
                    }
                    blnPrintTranxStatusTotal = false;
                    blnPrintReceiptmodeTotal = false;

                    if (myDataset.Tables[0].Rows[r]["receiptmode"].ToString() == "" || myDataset.Tables[0].Rows[r]["tranxstatus"].ToString() == "")
                    {
                        continue;
                    }

                    if (r != myDataset.Tables[0].Rows.Count - 1 && myDataset.Tables[0].Rows[r]["tranxstatus"].ToString() != myDataset.Tables[0].Rows[r + 1]["tranxstatus"].ToString())
                    {
                        blnPrintTranxStatusTotal = true;
                        blnPrintReceiptmodeTotal = true;
                    }
                    else if (r != myDataset.Tables[0].Rows.Count - 1 && myDataset.Tables[0].Rows[r]["receiptmode"].ToString() != myDataset.Tables[0].Rows[r + 1]["receiptmode"].ToString())
                    {
                        blnPrintReceiptmodeTotal = true;
                    }
                    else if (r == myDataset.Tables[0].Rows.Count - 1)
                    {
                        blnPrintGrandTotal = true;
                    }

                    for (int index = 1; index <= declocalpaymentamt_receiptmode.GetUpperBound(0); index++)
                    {
                        //fill in arrays for localpayment of receiptmode and tranxstatus
                        if (myDataset.Tables[0].Rows[r]["localpaymentccy"].ToString() == declocalpaymentamt_receiptmode[index, 0])
                        {
                            declocalpaymentamt_receiptmode.SetValue(Convert.ToString(Convert.ToDecimal(declocalpaymentamt_receiptmode[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["netremamount"])), index, 1);
                            declocalpaymentamt_tranxstatus.SetValue(Convert.ToString(Convert.ToDecimal(declocalpaymentamt_tranxstatus[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["netremamount"])), index, 1);
                            declocalpaymentamt_gross.SetValue(Convert.ToString(Convert.ToDecimal(declocalpaymentamt_gross[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["netremamount"])), index, 1);
                        }
                    }

                    for (int index = 1; index <= deccommissionamt_receiptmode.GetUpperBound(0); index++)
                    {
                        //fill in arrays for commission of receiptmode and tranxstatus
                        if (myDataset.Tables[0].Rows[r]["commisionccy"].ToString() == deccommissionamt_receiptmode[index, 0])
                        {
                            deccommissionamt_receiptmode.SetValue(Convert.ToString(Convert.ToDecimal(deccommissionamt_receiptmode[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"])), index, 1);
                            deccommissionamt_tranxstatus.SetValue(Convert.ToString(Convert.ToDecimal(deccommissionamt_tranxstatus[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"])), index, 1);
                            deccommissionamt_gross.SetValue(Convert.ToString(Convert.ToDecimal(deccommissionamt_gross[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["amount"])), index, 1);
                        }
                    }

                    for (int index = 1; index <= decnetremitbenamt_receiptmode.GetUpperBound(0); index++)
                    {
                        //fill in arrays for netremitben of receiptmode and tranxstatus
                        if (myDataset.Tables[0].Rows[r]["netremitbenccy"].ToString() == decnetremitbenamt_receiptmode[index, 0])
                        {
                            decnetremitbenamt_receiptmode.SetValue(Convert.ToString(Convert.ToDecimal(decnetremitbenamt_receiptmode[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["nettobeneamount"])), index, 1);
                            decnetremitbenamt_tranxstatus.SetValue(Convert.ToString(Convert.ToDecimal(decnetremitbenamt_tranxstatus[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["nettobeneamount"])), index, 1);
                            decnetremitbenamt_gross.SetValue(Convert.ToString(Convert.ToDecimal(decnetremitbenamt_gross[index, 1]) + Convert.ToDecimal(myDataset.Tables[0].Rows[r]["nettobeneamount"])), index, 1);
                        }
                    }

                    rowcount = r;
                    printTR(true);
                    for (int c = 1; c < 15; c++)
                    {
                        if (c == 5)
                        {
                            printTD(true, "'" + myDataset.Tables[0].Rows[r][c].ToString(), "1", "center", "normal");
                            printTD(false);
                        }
                        else if (c == 10 || c == 12 || c == 14)
                        {
                            if (myDataset.Tables[0].Rows[r][c] is DBNull)
                            {
                                printTD(true, String.Format("{0:0,0.000000000}", 0), "1", "right", "normal");
                            }
                            else
                            {
                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(myDataset.Tables[0].Rows[r][c])), "1", "right", "normal");
                            }
                            printTD(false);
                        }
                        else
                        {
                            printTD(true, myDataset.Tables[0].Rows[r][c].ToString(), "1", "center", "normal");
                            printTD(false);
                        }
                    }
                    printTR(false);


                    blnCreateNewRow = false;
                    blnLocalPayment = false;
                    blnCommission = false;
                    //blnNettobene = false;

                    if (blnPrintGrandTotal == true)
                    {
                        printTR(true);
                        printTR(false);

                        //a = use receiptmode total b = use tranxstatus total 
                        for (int a = 0; a < 3; a++)
                        {
                            for (int i = 1; i <= intUpperBound; i++)
                            {
                                if (i == 1)
                                {
                                    printTR(true);
                                    if (a == 0) //receiptmode total
                                    {
                                        printTD(true);
                                        printTD(false);
                                    }
                                    if (a == 0 || a == 1)//receiptmode and tranxstatus total
                                    {
                                        printTD(true);
                                        printTD(false);
                                    }
                                }
                                else if (blnCreateNewRow == true)
                                {
                                    printTR(true);

                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                }

                                for (int c = 3; c < 15; c++)
                                {
                                    if (i == 1 && c == 3)
                                    {
                                        if (a == 0)
                                        {
                                            printTD(true, "Receipt Mode Total", "1", "center", "bold");
                                            printTD(false);
                                        }
                                        else if (a == 1)
                                        {
                                            printTD(true, "Tranx Status Total", "1", "center", "bold");
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);
                                        }
                                        else if (a == 2)
                                        {
                                            printTD(true, "Grand Total", "1", "center", "bold");
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);
                                        }

                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                    }
                                    if (c == 9) //print for localpaymentamt 9 and 10
                                    {
                                        if (a == 0) //receiptmode
                                        {
                                            if (i <= declocalpaymentamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(declocalpaymentamt_receiptmode[i, 1]) != 0)
                                            {
                                                blnLocalPayment = true;
                                                blnCreateNewRow = true;
                                                printTD(true, declocalpaymentamt_receiptmode[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(declocalpaymentamt_receiptmode[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnLocalPayment = false;
                                            }
                                        }
                                        else if (a == 1)//tranxstatus
                                        {
                                            if (i <= declocalpaymentamt_tranxstatus.GetUpperBound(0) && Convert.ToDecimal(declocalpaymentamt_tranxstatus[i, 1]) != 0)
                                            {
                                                blnLocalPayment = true;
                                                blnCreateNewRow = true;
                                                printTD(true, declocalpaymentamt_tranxstatus[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(declocalpaymentamt_tranxstatus[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnLocalPayment = false;
                                            }
                                        }
                                        else if (a == 2)//grand total
                                        {
                                            if (i <= declocalpaymentamt_gross.GetUpperBound(0) && Convert.ToDecimal(declocalpaymentamt_gross[i, 1]) != 0)
                                            {
                                                blnLocalPayment = true;
                                                blnCreateNewRow = true;
                                                printTD(true, declocalpaymentamt_gross[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(declocalpaymentamt_gross[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnLocalPayment = false;
                                            }
                                        }
                                    }
                                    else if (c == 11) //print for commission amt 11 and 12
                                    {
                                        if (a == 0)//receiptmode
                                        {
                                            if (i <= deccommissionamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(deccommissionamt_receiptmode[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                blnCommission = true;
                                                blnCreateNewRow = true;
                                                printTD(true, deccommissionamt_receiptmode[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(deccommissionamt_receiptmode[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnCommission = false;
                                            }
                                        }
                                        else if (a == 1)//tranxstatus
                                        {
                                            if (i <= deccommissionamt_tranxstatus.GetUpperBound(0) && Convert.ToDecimal(deccommissionamt_tranxstatus[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                blnCommission = true;
                                                blnCreateNewRow = true;
                                                printTD(true, deccommissionamt_tranxstatus[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(deccommissionamt_tranxstatus[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnCommission = false;
                                            }
                                        }
                                        else if (a == 2)// grand total
                                        {
                                            if (i <= deccommissionamt_gross.GetUpperBound(0) && Convert.ToDecimal(deccommissionamt_gross[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                blnCommission = true;
                                                blnCreateNewRow = true;
                                                printTD(true, deccommissionamt_gross[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(deccommissionamt_gross[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnCommission = false;
                                            }
                                        }
                                    }
                                    else if (c == 13) //print for netremitben amt 13 and 14
                                    {
                                        if (a == 0)//receipt mode
                                        {
                                            if (i <= decnetremitbenamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(decnetremitbenamt_receiptmode[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false && blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);

                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                else if (blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }

                                                //blnNettobene = true;
                                                blnCreateNewRow = true;
                                                printTD(true, decnetremitbenamt_receiptmode[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decnetremitbenamt_receiptmode[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                //blnNettobene = false;
                                            }
                                        }
                                        else if (a == 1)//tranxstatus 
                                        {
                                            if (i <= decnetremitbenamt_tranxstatus.GetUpperBound(0) && Convert.ToDecimal(decnetremitbenamt_tranxstatus[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false && blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);

                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                else if (blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                }

                                                //blnNettobene = true;
                                                blnCreateNewRow = true;
                                                printTD(true, decnetremitbenamt_tranxstatus[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decnetremitbenamt_tranxstatus[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                //blnNettobene = false;
                                            }
                                        }
                                        else if (a == 2)//grand total
                                        {
                                            if (i <= decnetremitbenamt_gross.GetUpperBound(0) && Convert.ToDecimal(decnetremitbenamt_gross[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                blnCommission = true;
                                                blnCreateNewRow = true;
                                                printTD(true, decnetremitbenamt_gross[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decnetremitbenamt_gross[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnCommission = false;
                                            }
                                        }
                                    }
                                }

                                if (blnCreateNewRow == true)
                                {
                                    printTR(false);
                                }
                            }
                            printTR(true);
                            printTR(false);
                        }
                    }
                    else if (blnPrintTranxStatusTotal == true)
                    {
                        printTR(true);
                        printTR(false);

                        //a = use receiptmode total b = use tranxstatus total 
                        for (int a = 0; a < 2; a++)
                        {
                            for (int i = 1; i <= intUpperBound; i++)
                            {
                                if (i == 1)
                                {
                                    printTR(true);
                                    if (a == 0)
                                    {
                                        printTD(true);
                                        printTD(false);
                                    }
                                    //a == 0 && 1
                                    printTD(true);
                                    printTD(false);
                                }
                                else if (blnCreateNewRow == true)
                                {
                                    printTR(true);

                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                }

                                for (int c = 3; c < 15; c++)
                                {
                                    if (i == 1 && c == 3)
                                    {
                                        if (a == 0)
                                        {
                                            printTD(true, "Receipt Mode Total", "1", "center", "bold");
                                            printTD(false);
                                        }
                                        else
                                        {
                                            printTD(true, "Tranx Status Total", "1", "center", "bold");
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);
                                        }
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                    }
                                    if (c == 9) //print for localpaymentamt 9 and 10                                  
                                    {
                                        if (a == 0)
                                        {
                                            if (i <= declocalpaymentamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(declocalpaymentamt_receiptmode[i, 1]) != 0)
                                            {
                                                blnLocalPayment = true;
                                                blnCreateNewRow = true;
                                                printTD(true, declocalpaymentamt_receiptmode[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(declocalpaymentamt_receiptmode[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnLocalPayment = false;
                                            }
                                        }
                                        else
                                        {
                                            if (i <= declocalpaymentamt_tranxstatus.GetUpperBound(0) && Convert.ToDecimal(declocalpaymentamt_tranxstatus[i, 1]) != 0)
                                            {
                                                blnLocalPayment = true;
                                                blnCreateNewRow = true;
                                                printTD(true, declocalpaymentamt_tranxstatus[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(declocalpaymentamt_tranxstatus[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnLocalPayment = false;
                                            }
                                        }

                                    }
                                    else if (c == 11) //print for commission amt 11 and 12
                                    {
                                        if (a == 0)
                                        {
                                            if (i <= deccommissionamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(deccommissionamt_receiptmode[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                blnCommission = true;
                                                blnCreateNewRow = true;
                                                printTD(true, deccommissionamt_receiptmode[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(deccommissionamt_receiptmode[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnCommission = false;
                                            }
                                        }
                                        else
                                        {
                                            if (i <= deccommissionamt_tranxstatus.GetUpperBound(0) && Convert.ToDecimal(deccommissionamt_tranxstatus[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                blnCommission = true;
                                                blnCreateNewRow = true;
                                                printTD(true, deccommissionamt_tranxstatus[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(deccommissionamt_tranxstatus[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                blnCommission = false;
                                            }
                                        }
                                    }
                                    else if (c == 13) //print for netremitben amt 13 and 14
                                    {
                                        if (a == 0)
                                        {
                                            if (i <= decnetremitbenamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(decnetremitbenamt_receiptmode[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false && blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);

                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                else if (blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }

                                                //blnNettobene = true;
                                                blnCreateNewRow = true;
                                                printTD(true, decnetremitbenamt_receiptmode[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decnetremitbenamt_receiptmode[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                //blnNettobene = false;
                                            }
                                        }
                                        else
                                        {
                                            if (i <= decnetremitbenamt_tranxstatus.GetUpperBound(0) && Convert.ToDecimal(decnetremitbenamt_tranxstatus[i, 1]) != 0)
                                            {
                                                if (blnLocalPayment == false && blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);

                                                    printTD(true);
                                                    printTD(false);
                                                    printTD(true);
                                                    printTD(false);
                                                }
                                                else if (blnCommission == false)
                                                {
                                                    printTD(true);
                                                    printTD(false);
                                                }

                                                //blnNettobene = true;
                                                blnCreateNewRow = true;
                                                printTD(true, decnetremitbenamt_tranxstatus[i, 0], "1", "center", "bold");
                                                printTD(false);
                                                printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decnetremitbenamt_tranxstatus[i, 1])), "1", "right", "bold");
                                                printTD(false);
                                            }
                                            else
                                            {
                                                blnCreateNewRow = false;
                                                //blnNettobene = false;
                                            }
                                        }
                                    }
                                }

                                if (blnCreateNewRow == true)
                                {
                                    printTR(false);
                                }
                            }

                            for (int i = 1; i <= intUpperBound; i++)
                            {
                                if (i <= declocalpaymentamt_receiptmode.GetUpperBound(0))
                                {
                                    declocalpaymentamt_receiptmode.SetValue("0", i, 1);
                                }
                                if (i <= deccommissionamt_receiptmode.GetUpperBound(0))
                                {
                                    deccommissionamt_receiptmode.SetValue("0", i, 1);
                                }
                                if (i <= decnetremitbenamt_receiptmode.GetUpperBound(0))
                                {
                                    decnetremitbenamt_receiptmode.SetValue("0", i, 1);
                                }
                            }

                            printTR(true);
                            printTR(false);

                        }

                        for (int i = 1; i <= intUpperBound; i++)
                        {
                            if (i <= declocalpaymentamt_tranxstatus.GetUpperBound(0))
                            {
                                declocalpaymentamt_tranxstatus.SetValue("0", i, 1);
                            }
                            if (i <= deccommissionamt_tranxstatus.GetUpperBound(0))
                            {
                                deccommissionamt_tranxstatus.SetValue("0", i, 1);
                            }
                            if (i <= decnetremitbenamt_tranxstatus.GetUpperBound(0))
                            {
                                decnetremitbenamt_tranxstatus.SetValue("0", i, 1);
                            }
                        }

                        for (int i = 0; i <= declocalpaymentamt_receiptmode.GetUpperBound(0); i++)
                        {
                            declocalpaymentamt_receiptmode.SetValue("0", i, 1);
                        }

                        for (int i = 0; i <= deccommissionamt_receiptmode.GetUpperBound(0); i++)
                        {
                            deccommissionamt_receiptmode.SetValue("0", i, 1);
                        }

                        for (int i = 0; i <= decnetremitbenamt_receiptmode.GetUpperBound(0); i++)
                        {
                            decnetremitbenamt_receiptmode.SetValue("0", i, 1);
                        }
                    }
                    else if (blnPrintReceiptmodeTotal == true)
                    {
                        printTR(true);
                        printTR(false);

                        for (int i = 1; i <= intUpperBound; i++)
                        {
                            if (i == 1)
                            {
                                printTR(true);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                            }
                            else if (blnCreateNewRow == true)
                            {
                                printTR(true);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                                printTD(true);
                                printTD(false);
                            }

                            for (int c = 3; c < 15; c++)
                            {
                                if (i == 1 && c == 3)
                                {
                                    printTD(true, "Receipt Mode Total", "1", "center", "bold");
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                    printTD(true);
                                    printTD(false);
                                }
                                if (c == 9) //print for localpaymentamt 9 and 10                                  
                                {
                                    if (i <= declocalpaymentamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(declocalpaymentamt_receiptmode[i, 1]) != 0)
                                    {
                                        blnLocalPayment = true;
                                        blnCreateNewRow = true;
                                        printTD(true, declocalpaymentamt_receiptmode[i, 0], "1", "center", "bold");
                                        printTD(false);
                                        printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(declocalpaymentamt_receiptmode[i, 1])), "1", "right", "bold");
                                        printTD(false);
                                    }
                                    else
                                    {
                                        blnCreateNewRow = false;
                                        blnLocalPayment = false;
                                    }
                                }
                                else if (c == 11) //print for commission amt 11 and 12
                                {

                                    if (i <= deccommissionamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(deccommissionamt_receiptmode[i, 1]) != 0)
                                    {
                                        if (blnLocalPayment == false)
                                        {
                                            printTD(true);
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);
                                        }
                                        blnCommission = true;
                                        blnCreateNewRow = true;
                                        printTD(true, deccommissionamt_receiptmode[i, 0], "1", "center", "bold");
                                        printTD(false);
                                        printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(deccommissionamt_receiptmode[i, 1])), "1", "right", "bold");
                                        printTD(false);
                                    }
                                    else
                                    {
                                        blnCreateNewRow = false;
                                        blnCommission = false;
                                    }
                                }
                                else if (c == 13) //print for netremitben amt 13 and 14
                                {
                                    if (i <= decnetremitbenamt_receiptmode.GetUpperBound(0) && Convert.ToDecimal(decnetremitbenamt_receiptmode[i, 1]) != 0)
                                    {
                                        if (blnLocalPayment == false && blnCommission == false)
                                        {
                                            printTD(true);
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);

                                            printTD(true);
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);
                                        }
                                        else if (blnCommission == false)
                                        {
                                            printTD(true);
                                            printTD(false);
                                            printTD(true);
                                            printTD(false);
                                        }

                                        //blnNettobene = true;
                                        blnCreateNewRow = true;
                                        printTD(true, decnetremitbenamt_receiptmode[i, 0], "1", "center", "bold");
                                        printTD(false);
                                        printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decnetremitbenamt_receiptmode[i, 1])), "1", "right", "bold");
                                        printTD(false);
                                    }
                                    else
                                    {
                                        printTD(true);
                                        printTD(false);
                                        printTD(true);
                                        printTD(false);
                                        printTR(false);
                                        blnCreateNewRow = false;
                                        //blnNettobene = false;
                                    }
                                }
                            }

                            if (blnCreateNewRow == true)
                            {
                                printTR(false);
                            }
                        }

                        for (int i = 0; i <= declocalpaymentamt_receiptmode.GetUpperBound(0); i++)
                        {
                            declocalpaymentamt_receiptmode.SetValue("0", i, 1);
                        }

                        for (int i = 0; i <= deccommissionamt_receiptmode.GetUpperBound(0); i++)
                        {
                            deccommissionamt_receiptmode.SetValue("0", i, 1);
                        }

                        for (int i = 0; i <= decnetremitbenamt_receiptmode.GetUpperBound(0); i++)
                        {
                            decnetremitbenamt_receiptmode.SetValue("0", i, 1);
                        }
                    }
                    // //objwriter.Close();
                    //// return true;
                }
                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch  // commented to remove warning (Exception ex)
            {
                objwriter.Close();
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "ReportByTransactionSource" + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\ReportByTransactionSource" + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_BookingRates_Report(string tranxdate, string locationcode, string verifier, string authorizer)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset;
            irsConfiguration config = new irsConfiguration();
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            strSubFolder = tranxdate.Replace("/", "");
            string strFolder = strPath + "\\" + strSubFolder;
            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            sqlcommands = commons.getSqlCommand("EXEC sp_bookingrates '" + tranxdate + "', '" + locationcode + "'", false);
            myDataset = commons.getDataSet(sqlcommands, "bookingrates");

            try
            {
                objwriter = new StreamWriter(strFolder + "\\BookingRatesUsed" + locationcode + "_" + strSubFolder + ".xls");
                printHTML(true);
                printBody(true);
                printTable(true);

                printTR(true);
                printTD(true, "IRS World - (Booking Rates Used)", "5", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "5", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 0; a < myDataset.Tables[0].Columns.Count; a++)
                {
                    printTD(true, myDataset.Tables[0].Columns[a].ToString(), "1", "center", "bold");
                    printTD(false);
                }
                printTR(false);


                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    printTR(true);
                    for (int c = 0; c < myDataset.Tables[0].Columns.Count; c++)
                    {
                        if (c == 0 || c == 1 || c == 3)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r][c].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else
                        {
                            printTD(true, myDataset.Tables[0].Rows[r][c].ToString(), "1", "right", "normal");
                            printTD(false);
                        }
                    }
                    printTR(false);
                }
                printTR(true);
                printTD(true, "", "5", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Verified By:", "1", "left", "bold");
                printTD(false);
                printTD(true, verifier, "4", "left", "normal");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "Authorized By:", "1", "left", "bold");
                printTD(false);
                printTD(true, authorizer, "4", "left", "normal");
                printTD(false);
                printTR(false);

                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch  // commented to remove warning (Exception e)
            {
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "BookingRatesUsed" + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\BookingRatesUsed" + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_Accounting_Entries_Report(string tranxdate, string locationcode)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset;
            irsConfiguration config = new irsConfiguration();
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            strSubFolder = tranxdate.Replace("/", "");
            string strFolder = strPath + "\\" + strSubFolder;
            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
            {
                sqlcommands = commons.getSqlCommand("EXEC SP_ACCOUNTING_ENTRIES_REPORT '" + locationcode + "'", false);
            }
            else
            {
                sqlcommands = commons.getSqlCommand("EXEC SP_ACCOUNTING_ENTRIES_REPORT_LONDON '" + locationcode + "'", false);
            }

            myDataset = commons.getDataSet(sqlcommands, "accountingentries");

            try
            {
                objwriter = new StreamWriter(strFolder + "\\AccountingEntries" + locationcode + "_" + strSubFolder + ".xls");
                printHTML(true);
                printBody(true);
                printTable(true);

                printTR(true);
                printTD(true, "IRS World (Accountingentries - " + locationcode + ")", "12", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "12", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                for (int a = 0; a < myDataset.Tables[0].Columns.Count; a++)
                {
                    printTD(true, myDataset.Tables[0].Columns[a].ToString(), "1", "center", "bold");
                    printTD(false);
                }
                printTR(false);


                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    printTR(true);
                    for (int c = 0; c < myDataset.Tables[0].Columns.Count; c++)
                    {
                        if (c < 2)
                        {
                            printTD(true, myDataset.Tables[0].Rows[r][c].ToString(), "1", "left", "normal");
                            printTD(false);
                        }
                        else
                        {
                            printTD(true, myDataset.Tables[0].Rows[r][c].ToString(), "1", "center", "normal");
                            printTD(false);
                        }
                    }
                    printTR(false);
                }
                printTR(true);
                printTD(true, "", "12", "", "");
                printTD(false);
                printTR(false);

                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch  // commented to remove warning (Exception e)
            {
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "AccountingEntries" + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\AccountingEntries" + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Create_ReceiptMode_Report(string tranxdate, string locationcode)
        {
            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset = new DataSet();
            irsConfiguration config = new irsConfiguration();
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            strSubFolder = tranxdate.Replace("/", "");
            string strFolder = strPath + "\\" + strSubFolder;
            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            DataSet mytempdataset = new DataSet();

            sqlcommands = commons.getSqlCommand("EXEC sp_receipt_mode_report '" + locationcode + "', '" + tranxdate + "'", false);
            myDataset = commons.getDataSet(sqlcommands, "receipt_mode_report");

            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }

            string strnettobeneccy_desc;
            string strnetremitbenccy_desc;
            string strcharge_ccy_desc;
            string strreceiptmode_desc;
            strnettobeneccy_desc = "";
            strnetremitbenccy_desc = "";
            strcharge_ccy_desc = "";
            strreceiptmode_desc = "";

            string[,] decnetremitbenccy_receiptmode;
            string[,] decchargeccy_receiptmode;
            int[,] intTotalPerReceiptMode;

            string[] tempArray;
            tempArray = new string[myDataset.Tables[0].Rows.Count];
            string tempArrayValue;
            tempArrayValue = "";

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                tempArray.SetValue(myDataset.Tables[0].Rows[row]["nettobeneccy_desc"].ToString(), row);
            }
            Array.Sort(tempArray);
            for (int i = 0; i <= tempArray.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArray[i]) && tempArray[i] != "")
                {
                    strnettobeneccy_desc += tempArray[i] + "~";
                }

                tempArrayValue = tempArray[i];
            }


            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                tempArray.SetValue(myDataset.Tables[0].Rows[row]["netremitccy_desc"].ToString(), row);
            }
            Array.Sort(tempArray);
            for (int i = 0; i <= tempArray.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArray[i]) && tempArray[i] != "")
                {
                    strnetremitbenccy_desc += tempArray[i] + "~";
                }
                tempArrayValue = tempArray[i].Trim();
            }


            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                tempArray.SetValue(myDataset.Tables[0].Rows[row]["charge_ccy_desc"].ToString(), row);
            }
            Array.Sort(tempArray);
            for (int i = 0; i <= tempArray.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArray[i]) && tempArray[i] != "")
                {
                    strcharge_ccy_desc += tempArray[i] + "~";
                }
                tempArrayValue = tempArray[i].Trim();
            }



            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                tempArray.SetValue(myDataset.Tables[0].Rows[row]["receiptmode_desc"].ToString(), row);
            }
            Array.Sort(tempArray);
            for (int i = 0; i <= tempArray.GetUpperBound(0); i++)
            {
                if ((i == 0 || tempArrayValue != tempArray[i]) && tempArray[i] != "")
                {
                    strreceiptmode_desc += tempArray[i] + "~";
                }
                tempArrayValue = tempArray[i].Trim();
            }

            int intNettoBeneAmountTotalItemsCO = 0;
            int intNettoBeneAmountTotalItemsSO = 0;
            int intNettoBeneAmountTotalItemsCOSO = 0;

            decimal decNettoBeneAmountTotalDollarCO;
            decimal decNettoBeneAmountTotalDollarSO;
            decimal decNettoBeneAmountTotalDollarCOSO;
            decNettoBeneAmountTotalDollarCO = 0;
            decNettoBeneAmountTotalDollarSO = 0;
            decNettoBeneAmountTotalDollarCOSO = 0;

            decimal decNettoBeneAmountTotalPesoCO;
            decimal decNettoBeneAmountTotalPesoSO;
            decimal decNettoBeneAmountTotalPesoCOSO;
            decNettoBeneAmountTotalPesoCO = 0;
            decNettoBeneAmountTotalPesoSO = 0;
            decNettoBeneAmountTotalPesoCOSO = 0;

            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                if (myDataset.Tables[0].Rows[row]["nettobeneccy_desc"].ToString() == "US Dollar")
                {
                    if (myDataset.Tables[0].Rows[row]["tranxstatus_desc"].ToString() == "Credited Online")
                    {
                        decNettoBeneAmountTotalDollarCO += Convert.ToDecimal(myDataset.Tables[0].Rows[row]["NettoBeneAmount"]);
                        intNettoBeneAmountTotalItemsCO += 1;
                    }
                    else
                    {
                        decNettoBeneAmountTotalDollarSO += Convert.ToDecimal(myDataset.Tables[0].Rows[row]["NettoBeneAmount"]);
                        intNettoBeneAmountTotalItemsSO += 1;
                    }
                    decNettoBeneAmountTotalDollarCOSO += Convert.ToDecimal(myDataset.Tables[0].Rows[row]["NettoBeneAmount"]);
                }
                else
                {
                    if (myDataset.Tables[0].Rows[row]["tranxstatus_desc"].ToString() == "Credited Online")
                    {
                        decNettoBeneAmountTotalPesoCO += Convert.ToDecimal(myDataset.Tables[0].Rows[row]["NettoBeneAmount"]);
                        intNettoBeneAmountTotalItemsCO += 1;
                    }
                    else
                    {
                        decNettoBeneAmountTotalPesoSO += Convert.ToDecimal(myDataset.Tables[0].Rows[row]["NettoBeneAmount"]);
                        intNettoBeneAmountTotalItemsSO += 1;
                    }
                    decNettoBeneAmountTotalPesoCOSO += Convert.ToDecimal(myDataset.Tables[0].Rows[row]["NettoBeneAmount"]);
                }
                intNettoBeneAmountTotalItemsCOSO += 1;
            }

            objwriter = new StreamWriter(strFolder + "\\ReportByReceitMode" + locationcode + "_" + strSubFolder + ".xls");
            printHTML(true);
            printBody(true);
            printTable(true);

            printTR(true);
            printTD(true, "IRS World - (Transaction Details by Receipt Mode)", "10", "left", "bold");
            printTD(false);
            printTR(false);

            printTR(true);
            printTD(true, "Date: " + tranxdate, "", "left", "bold");
            printTD(false);
            printTR(false);

            printTR(true);
            printTD(true, "", "1", "left", "bold");
            printTD(false);
            printTR(false);

            for (int i = 0; i < 4; i++)
            {
                printTR(true);
                if (i == 0)
                {
                    for (int header = 0; header < 4; header++)
                    {
                        if (header == 0)
                        {
                            printTD(true, "NET Amount", "1", "center", "bold");
                            printTD(false);
                        }
                        else if (header == 1)
                        {
                            printTD(true, "ITEMS", "1", "center", "bold");
                            printTD(false);
                        }
                        else if (header == 2)
                        {
                            printTD(true, "DOLLAR", "1", "center", "bold");
                            printTD(false);
                        }
                        else if (header == 3)
                        {
                            printTD(true, "PESO", "1", "center", "bold");
                            printTD(false);
                        }
                    }
                }

                if (i == 1)
                {
                    for (int header = 0; header < 4; header++)
                    {
                        if (header == 0)
                        {
                            printTD(true, "Credited Online", "1", "left", "bold");
                            printTD(false);
                        }
                        else if (header == 1)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", intNettoBeneAmountTotalItemsCO), "1", "right", "normal");
                            printTD(false);
                        }
                        else if (header == 2)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", decNettoBeneAmountTotalDollarCO), "1", "right", "normal");
                            printTD(false);
                        }
                        else if (header == 3)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", decNettoBeneAmountTotalPesoCO), "1", "right", "normal");
                            printTD(false);
                        }
                    }
                }

                if (i == 2)
                {
                    for (int header = 0; header < 4; header++)
                    {
                        if (header == 0)
                        {
                            printTD(true, "Processed by GRS", "1", "left", "bold");
                            printTD(false);
                        }
                        else if (header == 1)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", intNettoBeneAmountTotalItemsSO), "1", "right", "normal");
                            printTD(false);
                        }
                        else if (header == 2)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", decNettoBeneAmountTotalDollarSO), "1", "right", "normal");
                            printTD(false);
                        }
                        else if (header == 3)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", decNettoBeneAmountTotalPesoSO), "1", "right", "normal");
                            printTD(false);
                        }
                    }
                }

                if (i == 3)
                {
                    for (int header = 0; header < 4; header++)
                    {
                        if (header == 0)
                        {
                            printTD(true, "Total items", "1", "left", "bold");
                            printTD(false);
                        }
                        else if (header == 1)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", intNettoBeneAmountTotalItemsCOSO), "1", "right", "normal");
                            printTD(false);
                        }
                        else if (header == 2)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", decNettoBeneAmountTotalDollarCOSO), "1", "right", "normal");
                            printTD(false);
                        }
                        else if (header == 3)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", decNettoBeneAmountTotalPesoCOSO), "1", "right", "normal");
                            printTD(false);
                        }
                    }
                }
                printTR(false);
            }

            printTR(true);
            printTR(false);

            string[] strCcyNo;
            string[] strReceiptModeNo;

            strCcyNo = strnetremitbenccy_desc.Split('~');
            strReceiptModeNo = strreceiptmode_desc.Split('~');

            decnetremitbenccy_receiptmode = new string[strCcyNo.Length, strReceiptModeNo.Length + 1];
            intTotalPerReceiptMode = new int[1, strReceiptModeNo.Length + 1];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                decnetremitbenccy_receiptmode.SetValue(strCcyNo[i], i + 1, 0);
            }

            for (int i = 0; i < strReceiptModeNo.GetUpperBound(0); i++)
            {
                decnetremitbenccy_receiptmode.SetValue(strReceiptModeNo[i], 0, i + 1);
            }

            bool blnEscapeRM;
            blnEscapeRM = false;
            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                for (int rm = 1; rm <= decnetremitbenccy_receiptmode.GetUpperBound(1); rm++)
                {
                    for (int ccy = 1; ccy <= decnetremitbenccy_receiptmode.GetUpperBound(0); ccy++)
                    {
                        if (myDataset.Tables[0].Rows[row]["receiptmode_desc"].ToString() == decnetremitbenccy_receiptmode[0, rm] &&
                            myDataset.Tables[0].Rows[row]["netremitccy_desc"].ToString() == decnetremitbenccy_receiptmode[ccy, 0])
                        {
                            intTotalPerReceiptMode.SetValue(1 + intTotalPerReceiptMode[0, rm], 0, rm);
                            intTotalPerReceiptMode.SetValue(1 + intTotalPerReceiptMode[0, decnetremitbenccy_receiptmode.GetUpperBound(1)], 0, decnetremitbenccy_receiptmode.GetUpperBound(1));

                            decnetremitbenccy_receiptmode.SetValue(Convert.ToString(Convert.ToDecimal(decnetremitbenccy_receiptmode[ccy, rm]) + Convert.ToDecimal(myDataset.Tables[0].Rows[row]["netremamount"])), ccy, rm);
                            decnetremitbenccy_receiptmode.SetValue(Convert.ToString(Convert.ToDecimal(decnetremitbenccy_receiptmode[ccy, decnetremitbenccy_receiptmode.GetUpperBound(1)]) + Convert.ToDecimal(myDataset.Tables[0].Rows[row]["netremamount"])), ccy, decnetremitbenccy_receiptmode.GetUpperBound(1));
                            blnEscapeRM = true;
                            break;
                        }
                    }
                    if (blnEscapeRM == true)
                    {
                        blnEscapeRM = false;
                        break;
                    }
                }
            }

            for (int ccy = 0; ccy <= decnetremitbenccy_receiptmode.GetUpperBound(0); ccy++)
            {
                printTR(true);
                if (ccy == 0)
                {
                    printTD(true, "", "1", "left", "normal");
                    printTD(false);
                }
                else
                {
                    printTD(true, "Local Payment Amt (" + decnetremitbenccy_receiptmode[ccy, 0] + ")", "1", "left", "bold");
                    printTD(false);
                }
                for (int rm = 1; rm <= decnetremitbenccy_receiptmode.GetUpperBound(1); rm++)
                {
                    if (ccy == 0)
                    {
                        if (rm != decnetremitbenccy_receiptmode.GetUpperBound(1))
                        {
                            printTD(true, decnetremitbenccy_receiptmode[ccy, rm], "1", "center", "bold");
                        }
                        else
                        {
                            printTD(true, "Total", "1", "center", "bold");
                        }

                    }
                    else
                    {
                        if (decnetremitbenccy_receiptmode[ccy, rm] == null)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", 0), "1", "right", "normal");
                        }
                        else
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decnetremitbenccy_receiptmode[ccy, rm])), "1", "right", "normal");
                        }
                    }
                    printTD(false);
                }
                printTR(false);
            }


            //charge
            strCcyNo = strcharge_ccy_desc.Split('~');
            strReceiptModeNo = strreceiptmode_desc.Split('~');
            decchargeccy_receiptmode = new string[strCcyNo.Length, strReceiptModeNo.Length + 1];

            for (int i = 0; i < strCcyNo.GetUpperBound(0); i++)
            {
                decchargeccy_receiptmode.SetValue(strCcyNo[i], i + 1, 0);
            }

            for (int i = 0; i < strReceiptModeNo.GetUpperBound(0); i++)
            {
                decchargeccy_receiptmode.SetValue(strReceiptModeNo[i], 0, i + 1);
            }

            blnEscapeRM = false;
            for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
            {
                for (int rm = 1; rm <= decchargeccy_receiptmode.GetUpperBound(1); rm++)
                {
                    for (int ccy = 1; ccy <= decchargeccy_receiptmode.GetUpperBound(0); ccy++)
                    {
                        if (myDataset.Tables[0].Rows[row]["receiptmode_desc"].ToString() == decchargeccy_receiptmode[0, rm] &&
                            myDataset.Tables[0].Rows[row]["charge_ccy_desc"].ToString() == decchargeccy_receiptmode[ccy, 0])
                        {
                            decchargeccy_receiptmode.SetValue(Convert.ToString(Convert.ToDecimal(decchargeccy_receiptmode[ccy, rm]) + Convert.ToDecimal(myDataset.Tables[0].Rows[row]["chargeamt"])), ccy, rm);
                            decchargeccy_receiptmode.SetValue(Convert.ToString(Convert.ToDecimal(decchargeccy_receiptmode[ccy, decchargeccy_receiptmode.GetUpperBound(1)]) + Convert.ToDecimal(myDataset.Tables[0].Rows[row]["chargeamt"])), ccy, decchargeccy_receiptmode.GetUpperBound(1));

                            blnEscapeRM = true;
                            break;
                        }
                    }
                    if (blnEscapeRM == true)
                    {
                        blnEscapeRM = false;
                        break;
                    }
                }
            }

            for (int ccy = 1; ccy <= decchargeccy_receiptmode.GetUpperBound(0); ccy++)
            {
                printTR(true);
                printTD(true, "Bank Com  (" + decchargeccy_receiptmode[ccy, 0] + ")", "1", "left", "bold");
                printTD(false);

                for (int rm = 1; rm <= decchargeccy_receiptmode.GetUpperBound(1); rm++)
                {
                    if (ccy != 0)
                    {
                        if (decchargeccy_receiptmode[ccy, rm] == null)
                        {
                            printTD(true, String.Format("{0:0,0.000000000}", 0), "1", "right", "normal");
                        }
                        else
                        {

                            printTD(true, String.Format("{0:0,0.000000000}", Convert.ToDecimal(decchargeccy_receiptmode[ccy, rm])), "1", "right", "normal");
                        }
                    }
                    printTD(false);
                }
                printTR(false);
            }

            //total
            printTR(true);
            printTD(true, "Total", "1", "left", "bold");
            printTD(false);

            for (int rm = 1; rm <= decchargeccy_receiptmode.GetUpperBound(1); rm++)
            {
                printTD(true, String.Format("{0:0,0.000000000}", intTotalPerReceiptMode[0, rm]), "1", "right", "normal");
                printTD(false);
            }
            printTR(false);


            printTR(false);
            printTable(false);
            printBody(false);
            printHTML(false);
            objwriter.Close();

            insertFileRecord(locationcode, tranxdate, "ReportByReceitMode" + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\ReportByReceitMode" + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        //private DataSet checkAccountingEntries(string branchcode, string tranxdate, string tranxstatus)
        //{ 
        //    sqlHelper commons = new sqlHelper();
        //    SqlCommand sqlcommands = null;

        //    tranxstatus = tranxstatus.Replace("'", "");

        //            sqlcommands = commons.getSqlCommand("SELECT " +
        //                                                "    R.Seq_id, " +
        //                                                "    R.GrossRemAmount, " +
        //                                                "    A.TranxGLCode, " +
        //                                                "    'TranxGLCode', " +
        //                                                "    A.Seq_id " +
        //                                                "FROM TBL_REMITTANCE R " +
        //                                                "LEFT JOIN TBL_ACCOUNTINGENTRIES A ON R.remittingbranch = A.locationcode " +
        //                                                "    AND R.transactionsource = A.tranxsource " +
        //                                                "    AND R.receiptmode = A.receiptmode " +
        //                                                "    AND R.remittingcurrency = A.tranxcurrency " +
        //                                                "    AND R.nettobenecurrency = A.implementationcurrency " +
        //                                                "WHERE R.remittingbranch = '" + branchcode + "' " +
        //                                                "    AND CONVERT(VARCHAR (10), R.Tranxdate, 101)= '" + tranxdate + "' " +
        //                                                "    AND R.transtatus IN (" + tranxstatus + ") " +
        //                                                "UNION ALL " +
        //                                                "SELECT  " +
        //                                                "    R.Seq_id, " +
        //                                                "    R.NettoBeneAmount, " +
        //                                                "    A.ImplementationGLCode, " + 
        //                                                "    'ImplementationGLCode', " +
        //                                                "    A.Seq_id " +
        //                                                "FROM TBL_REMITTANCE R " +
        //                                                "LEFT JOIN TBL_ACCOUNTINGENTRIES A ON R.remittingbranch = A.locationcode " +
        //                                                "    AND R.transactionsource = A.tranxsource " +
        //                                                "    AND R.receiptmode = A.receiptmode " +
        //                                                "    AND R.remittingcurrency = A.tranxcurrency " +
        //                                                "    AND R.nettobenecurrency = A.implementationcurrency " +
        //                                                "WHERE R.remittingbranch = '" + branchcode + "' " +
        //                                                "    AND CONVERT(VARCHAR (10), R.Tranxdate, 101)= '" + tranxdate + "' " +
        //                                                "    AND R.transtatus IN (" + tranxstatus + ") " +
        //                                                "UNION ALL " +
        //                                                "SELECT  " +
        //                                                "    R.Seq_id, " +
        //                                                "    C.Amount, " +
        //                                                "    A.ServiceChargeGLCode, " +
        //                                                "    'ServiceChargeGLCode', " +
        //                                                "    A.Seq_id " +
        //                                                "FROM TBL_REMITTANCE R " +
        //                                                "INNER JOIN TBL_REMITTANCE_COMPUTED_CHARGES C ON R.seq_id = C.txn_seq_id " +
        //                                                "LEFT JOIN TBL_ACCOUNTINGENTRIES A ON R.remittingbranch = A.locationcode " +
        //                                                "    AND R.transactionsource = A.tranxsource " +
        //                                                "    AND R.receiptmode = A.receiptmode " +
        //                                                "    AND R.remittingcurrency = A.tranxcurrency " +
        //                                                "    AND R.nettobenecurrency = A.implementationcurrency " +
        //                                                "WHERE C.Amount > 0 " +
        //                                                "    AND C.ChargeName = 1 " +
        //                                                "    AND R.remittingbranch = '" + branchcode + "' " +
        //                                                "    AND CONVERT(VARCHAR (10), R.Tranxdate, 101)= '" + tranxdate + "' " +
        //                                                "    AND R.transtatus IN (" + tranxstatus + ") " +
        //                                                "UNION ALL " +
        //                                                "SELECT  " +
        //                                                "    R.Seq_id, " +
        //                                                "    C.Amount, " +
        //                                                "    A.LocalCableChargesGLCode, " +
        //                                                "    'LocalCableChargesGLCode', " +
        //                                                "    A.Seq_id " +
        //                                                "FROM TBL_REMITTANCE R " +
        //                                                "INNER JOIN TBL_REMITTANCE_COMPUTED_CHARGES C ON R.seq_id = C.txn_seq_id " +
        //                                                "LEFT JOIN TBL_ACCOUNTINGENTRIES A ON R.remittingbranch = A.locationcode " +
        //                                                "    AND R.transactionsource = A.tranxsource " +
        //                                                "    AND R.receiptmode = A.receiptmode " +
        //                                                "    AND R.remittingcurrency = A.tranxcurrency " +
        //                                                "    AND R.nettobenecurrency = A.implementationcurrency " +
        //                                                "WHERE C.Amount > 0 " +
        //                                                "    AND C.ChargeName = 2 " +
        //                                                "    AND R.remittingbranch = '" + branchcode + "' " +
        //                                                "    AND CONVERT(VARCHAR (10), R.Tranxdate, 101)= '" + tranxdate + "' " +
        //                                                "    AND R.transtatus IN (" + tranxstatus + ") " +
        //                                                "UNION ALL " +
        //                                                "SELECT  " +
        //                                                "    R.Seq_id, " +
        //                                                "    C.Amount, " +
        //                                                "    A.LocalServiceFeeGLCode, " +
        //                                                "    'LocalServiceFeeGLCode', " +
        //                                                "    A.Seq_id " +
        //                                                "FROM TBL_REMITTANCE R " +
        //                                                "INNER JOIN TBL_REMITTANCE_COMPUTED_CHARGES C ON R.seq_id = C.txn_seq_id " +
        //                                                "LEFT JOIN TBL_ACCOUNTINGENTRIES A ON R.remittingbranch = A.locationcode " +
        //                                                "    AND R.transactionsource = A.tranxsource " +
        //                                                "    AND R.receiptmode = A.receiptmode " +
        //                                                "    AND R.remittingcurrency = A.tranxcurrency " +
        //                                                "    AND R.nettobenecurrency = A.implementationcurrency " +
        //                                                "WHERE C.Amount > 0 " +
        //                                                "    AND C.ChargeName = 4 " +
        //                                                "    AND R.remittingbranch = '" + branchcode + "' " +
        //                                                "    AND CONVERT(VARCHAR (10), R.Tranxdate, 101)= '" + tranxdate + "' " +
        //                                                "    AND R.transtatus IN (" + tranxstatus + ") " +
        //                                                "UNION ALL " +
        //                                                "SELECT  " +
        //                                                "    R.Seq_id, " +
        //                                                "    C.Amount, " +
        //                                                "    A.OPEGLCode, " +
        //                                                "    'OPEGLCode', " +
        //                                                "    A.Seq_id " +
        //                                                "FROM TBL_REMITTANCE R " +
        //                                                "INNER JOIN TBL_REMITTANCE_COMPUTED_CHARGES C ON R.seq_id = C.txn_seq_id " +
        //                                                "LEFT JOIN TBL_ACCOUNTINGENTRIES A ON R.remittingbranch = A.locationcode " +
        //                                                "    AND R.transactionsource = A.tranxsource " +
        //                                                "    AND R.receiptmode = A.receiptmode " +
        //                                                "    AND R.remittingcurrency = A.tranxcurrency " +
        //                                                "    AND R.nettobenecurrency = A.implementationcurrency " +
        //                                                "WHERE C.Amount > 0 " +
        //                                                "    AND C.ChargeName = 5 " +
        //                                                "    AND R.remittingbranch = '" + branchcode + "' " +
        //                                                "    AND CONVERT(VARCHAR (10), R.Tranxdate, 101)= '" + tranxdate + "' " +
        //                                                "    AND R.transtatus IN (" + tranxstatus + ") " +
        //                                                "UNION ALL " +
        //                                                "SELECT  " +
        //                                                "    R.Seq_id, " +
        //                                                "    C.Amount, " +
        //                                                "    A.DocStampGLCodes, " +
        //                                                "    'DocStampGLCodes', " +
        //                                                "    A.Seq_id " +
        //                                                "FROM TBL_REMITTANCE R " +
        //                                                "INNER JOIN TBL_REMITTANCE_COMPUTED_CHARGES C ON R.seq_id = C.txn_seq_id " +
        //                                                "LEFT JOIN TBL_ACCOUNTINGENTRIES A ON R.remittingbranch = A.locationcode " +
        //                                                "    AND R.transactionsource = A.tranxsource " +
        //                                                "    AND R.receiptmode = A.receiptmode " +
        //                                                "    AND R.remittingcurrency = A.tranxcurrency " +
        //                                                "    AND R.nettobenecurrency = A.implementationcurrency " +
        //                                                "WHERE C.Amount > 0 " +
        //                                                "    AND C.ChargeName = 6 " +
        //                                                "    AND R.remittingbranch = '" + branchcode + "' " +
        //                                                "    AND CONVERT(VARCHAR (10), R.Tranxdate, 101)= '" + tranxdate + "' " +
        //                                                "    AND R.transtatus IN (" + tranxstatus + ") " +
        //                                                "ORDER BY R.SEQ_ID", false);                
        //        return commons.getDataSet(sqlcommands, "implementationccy");
        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Create_GEFUFile(string tranxdate, string locationcode, string tranxstatus)
        {
            SqlCommand insertCommand = new SqlCommand();
            string strCOSO = "";
            if (tranxstatus == "Credited Online")
            {
                tranxstatus = "'2, 273'";//17'";
                strCOSO = "CO";
            }
            else
            {
                tranxstatus = "'99,5'";  //"'99'"; // "'5,16,99'";
                strCOSO = "PG";
            }
            string gefuDetail1 = "2";
            string gefuDetail2 = "03";
            string gefuDetail3 = ""; //glcode
            string gefuDetail4 = ""; //flexcube branchcode
            string gefuDetail5 = ""; //tranx mnemonic
            string gefuDetail6 = ""; //tranxdate
            string gefuDetail7 = ""; //debit credit
            string gefuDetail8 = ""; //value date
            string gefuDetail9 = ""; //tranx ccy
            string gefuDetail10 = ""; //amt in loc ccy
            string gefuDetail11 = ""; //amt in tranx ccy
            string gefuDetail12 = ""; //conversion rate
            string gefuDetail13 = ""; //ref no
            string gefuDetail14 = ""; //document no
            gefuDetail14 = gefuDetail14.PadRight(12);
            string gefuDetail15 = ""; //refno + beneficiaryid    
            string gefuDetail16 = ""; //beneficiaryid
            string gefuDetail17 = ""; //beneficiary name
            string gefuDetail18 = ""; //beneficiary add1
            string gefuDetail19 = ""; //beneficiary add2
            string gefuDetail20 = ""; //beneficiary add3
            string gefuDetail21 = ""; //city
            string gefuDetail22 = ""; //state
            string gefuDetail23 = ""; //country
            string gefuDetail24 = ""; //zipcode
            string gefuDetail25 = "10"; //option flag
            string gefuDetail26 = "00000";
            string gefuDetail27 = "0000";
            string gefuDetail28 = "N";
            string gefuDetail29 = "";
            gefuDetail29 = gefuDetail29.PadRight(16);

            SqlCommand sqlcommands = null;
            sqlHelperWs commons = new sqlHelperWs();
            DataSet myDataset;
            irsConfiguration config = new irsConfiguration();
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            strSubFolder = tranxdate.Replace("/", "");
            string strFolder = strPath + "\\" + strSubFolder;
            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
            }

            //DataSet incAccountDS;
            //incAccountDS = checkAccountingEntries(locationcode, tranxdate, tranxstatus);
            if (locationcode == "ANB")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_irs_gefu_anb '" + locationcode + "', '" + tranxdate + "'," + tranxstatus + "", false);
            }
            else if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_irs_gefu '" + locationcode + "', '" + tranxdate + "'," + tranxstatus + "", false);
            }
            else
            {
                sqlcommands = commons.getSqlCommand("EXEC sp_irs_gefu_london '" + locationcode + "', '" + tranxdate + "'," + tranxstatus + "", false);
            }

            myDataset = commons.getDataSet(sqlcommands, "gefudata");


            string fc_local_ccy = "";
            DataTable dt = commons.execStoredProcedureTable("sp_gefu_fc_currency", new object[] { locationcode });
            if (dt != null)
            {
                fc_local_ccy = Convert.ToString(dt.Rows[0]["fc_ccy"]);
            }
            else
            {
                fc_local_ccy = "0";
            }

            string strAccntInc;
            strAccntInc = "";
            if (myDataset.Tables["gefudata"].Rows.Count != 0)
            {
                for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
                {
                    if (myDataset.Tables["gefudata"].Rows[row]["GLCode"].ToString().Trim() == "")
                    {
                        strAccntInc += myDataset.Tables["gefudata"].Rows[row]["GLCode_name"].ToString() + "~" + myDataset.Tables["gefudata"].Rows[row]["acc_seq_id"] + ";";
                    }
                }
            }
            else
            {
                return "No Data to be extracted";
            }

            if (strAccntInc != "")
            {
                string[] arSeq_ID;
                arSeq_ID = strAccntInc.Split(';');

                int intEncountered;
                intEncountered = 0;

                strAccntInc = "";
                for (int a = 0; a <= arSeq_ID.GetUpperBound(0) - 1; a++)
                {
                    string[] arr1;
                    arr1 = arSeq_ID[a].Split('~');

                    for (int b = 0; b < a + 1; b++)
                    {
                        string[] arr2;
                        arr2 = arSeq_ID[b].Split('~');

                        if (arr1[1] == arr2[1] && arr1[0] == arr2[0])
                        {
                            intEncountered += 1;
                        }

                        if (intEncountered > 1)
                        {
                            break;
                        }
                    }

                    if (intEncountered == 1)
                    {
                        strAccntInc += arr1[0] + "~" + arr1[1] + ";";
                    }
                    intEncountered = 0;
                }

                if (strAccntInc != "")
                {
                    return strAccntInc;
                }
            }

            if (myDataset.Tables["gefudata"].Rows.Count != 0)
            {
                decimal decTotalCredit;
                decimal decTotalDebit;
                decimal decPRX;
                int intTotalCredit;
                int intTotalDebit;
                decimal decFooterTotalCredit;
                decimal decFooterTotalDebit;
                int intFooterTotalCredit;
                int intFooterTotalDebit;

                decFooterTotalCredit = 0;
                decFooterTotalDebit = 0;
                intFooterTotalCredit = 0;
                intFooterTotalDebit = 0;
                decTotalCredit = 0;
                decTotalDebit = 0;
                decPRX = 0;
                intTotalCredit = 0;
                intTotalDebit = 0;

                StreamWriter objwriter = new StreamWriter(strFolder + "\\GEFU" + strCOSO + locationcode + strSubFolder + ".txt");

                string[] strAmount = new string[] { "", "", "", "", "", "" };

                objwriter.WriteLine("1" + TrimValue(myDataset.Tables[0].Rows[0]["TranxDate"].ToString(), 8));

                for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
                {
                    if (locationcode == "ANB")
                    {
                        gefuDetail2 = "01";
                    }
                    gefuDetail3 = TrimValue(myDataset.Tables[0].Rows[row]["GLCode"].ToString(), 16);
                    gefuDetail4 = TrimValue(myDataset.Tables[0].Rows[row]["fcbranchcode"].ToString(), 4);
                    gefuDetail5 = myDataset.Tables[0].Rows[row]["Tranx_Mnemonic"].ToString().PadLeft(5, '0');
                    gefuDetail6 = TrimValue(myDataset.Tables[0].Rows[row]["TranxDate"].ToString(), 8);
                    gefuDetail7 = TrimValue(myDataset.Tables[0].Rows[row]["Debit_Credit"].ToString(), 1);
                    gefuDetail8 = TrimValue(myDataset.Tables[0].Rows[row]["TranxDate"].ToString(), 8);
                    gefuDetail9 = myDataset.Tables[0].Rows[row]["FC_Ccy"].ToString().PadLeft(5, '0');
                    //if (locationcode == "tok"
                    gefuDetail10 = Convert.ToString(Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 2)).Replace(".", "").PadLeft(14, '0'); //amt in loc ccy
                    if (locationcode == "TOK" || locationcode == "NGY")
                    {
                        gefuDetail11 = Convert.ToString(Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Transaction_Amount"]), 0)).Replace(".", "").PadLeft(14, '0'); //amt in tranx ccy
                    }
                    else
                    {
                        gefuDetail11 = Convert.ToString(Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Transaction_Amount"]), 2)).Replace(".", "").PadLeft(14, '0'); //amt in tranx ccy
                    }

                    gefuDetail12 = Convert.ToString(Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["bookingrate"]), 2)).Replace(".", "").PadLeft(8, '0');//conversion rate
                    gefuDetail13 = TrimValue(myDataset.Tables[0].Rows[row]["referenceno"].ToString(), 40);
                    gefuDetail15 = TrimValue(myDataset.Tables[0].Rows[row]["referenceno"].ToString() + " " + myDataset.Tables[0].Rows[row]["fullname"].ToString(), 40);
                    gefuDetail16 = TrimValue(myDataset.Tables[0].Rows[row]["beneficiarycifid"].ToString(), 16);
                    gefuDetail17 = TrimValue(myDataset.Tables[0].Rows[row]["fullname"].ToString(), 120);
                    gefuDetail18 = TrimValue(myDataset.Tables[0].Rows[row]["add1"].ToString(), 35);
                    gefuDetail19 = TrimValue(myDataset.Tables[0].Rows[row]["add2"].ToString(), 35);
                    gefuDetail20 = TrimValue(myDataset.Tables[0].Rows[row]["add3"].ToString(), 35);
                    gefuDetail21 = TrimValue(myDataset.Tables[0].Rows[row]["add4"].ToString(), 35);
                    gefuDetail22 = TrimValue(myDataset.Tables[0].Rows[row]["add5"].ToString(), 35);
                    gefuDetail23 = TrimValue(myDataset.Tables[0].Rows[row]["country"].ToString(), 35);
                    gefuDetail24 = TrimValue(myDataset.Tables[0].Rows[row]["zipcode"].ToString(), 35);
                    gefuDetail29 = TrimValue(myDataset.Tables[0].Rows[row]["MISCode"].ToString(), 16);

                    if (gefuDetail7 == "D")
                    {
                        if (locationcode == "TOK" || locationcode == "NGY")
                        {
                            decTotalDebit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 0);//Convert.ToDecimal(gefuDetail11);
                            decFooterTotalDebit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 0);
                        }
                        else
                        {
                            decTotalDebit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 2);//Convert.ToDecimal(gefuDetail11);
                            decFooterTotalDebit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 2);
                        }
                        intTotalDebit += 1;
                        intFooterTotalDebit += 1;
                    }
                    else
                    {
                        if (locationcode == "TOK" || locationcode == "NGY")
                        {
                            decTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 0); //Convert.ToDecimal(gefuDetail11);
                            decFooterTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 0);
                        }
                        else if (locationcode == "ANB")
                        {
                            if (row == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[row]["seq_id"].ToString().Trim() != myDataset.Tables[0].Rows[row + 1]["seq_id"].ToString().Trim())
                            {
                                decTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]) * Convert.ToDecimal(myDataset.Tables[0].Rows[row]["bookingrate"]), 2); //Convert.ToDecimal(gefuDetail11);
                                decFooterTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]) * Convert.ToDecimal(myDataset.Tables[0].Rows[row]["bookingrate"]), 2);
                            }
                            else
                            {
                                decTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 2); //Convert.ToDecimal(gefuDetail11);
                                decFooterTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 2);
                            }
                            
                        }
                        else
                        {
                            decTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 2); //Convert.ToDecimal(gefuDetail11);
                            decFooterTotalCredit += Math.Round(Convert.ToDecimal(myDataset.Tables[0].Rows[row]["Local_Amount_Equivalent"]), 2);
                        }
                        intTotalCredit += 1;
                        intFooterTotalCredit += 1;
                    }

                    objwriter.WriteLine(gefuDetail1 + gefuDetail2 + gefuDetail3 + gefuDetail4 + gefuDetail5 +
                                            gefuDetail6 + gefuDetail7 + gefuDetail8 + gefuDetail9 + gefuDetail10 +
                                            gefuDetail11 + gefuDetail12 + gefuDetail13 + gefuDetail14 + gefuDetail15 +
                                            gefuDetail16 + gefuDetail17 + gefuDetail18 + gefuDetail19 + gefuDetail20 +
                                            gefuDetail21 + gefuDetail22 + gefuDetail23 + gefuDetail24 + gefuDetail25 +
                                            gefuDetail26 + gefuDetail27 + gefuDetail28 + gefuDetail29);

                    gefuDetail5 = "1408".PadLeft(5, '0');
                    gefuDetail7 = "C";
                    gefuDetail3 = TrimValue(myDataset.Tables[0].Rows[row]["PRXGLCode"].ToString(), 16);

                    if (row == myDataset.Tables[0].Rows.Count - 1 || myDataset.Tables[0].Rows[row]["seq_id"].ToString().Trim() != myDataset.Tables[0].Rows[row + 1]["seq_id"].ToString().Trim())
                    {
                        decPRX = decTotalDebit - decTotalCredit;
                        gefuDetail10 = Convert.ToString(Math.Abs(decPRX)).Replace(".", "").PadLeft(14, '0');//TrimValue(Convert.ToString(Math.Abs(decPRX)), 14);
                        gefuDetail11 = gefuDetail10;
                        gefuDetail12 = TrimValue("1", 8);
                        gefuDetail12 = Convert.ToString("1.00").Replace(".", "").PadLeft(8, '0');//conversion rate


                        if (locationcode == "ANB")
                        {
                            if (decPRX <= 0)
                            {
                                gefuDetail5 = "1008".PadLeft(5, '0');
                                gefuDetail7 = "D";
                                //decFooterTotalDebit += Math.Abs(decPRX);
                                //intFooterTotalDebit += 1;
                            }
                            else
                            {
                                decFooterTotalCredit += decPRX;
                                intFooterTotalCredit += 1;
                            }
                        }
                        else
                        {
                            if (decPRX < 0)
                            {
                                gefuDetail5 = "1008".PadLeft(5, '0');
                                gefuDetail7 = "D";
                                decFooterTotalDebit += Math.Abs(decPRX);
                                intFooterTotalDebit += 1;
                            }
                            else
                            {
                                decFooterTotalCredit += decPRX;
                                intFooterTotalCredit += 1;
                            }
                        }



                        
                        decPRX = 0;
                        decTotalDebit = 0;
                        decTotalCredit = 0;
                        gefuDetail9 = fc_local_ccy.ToString().PadLeft(5, '0'); ; //PRX must be local currency
                        if (locationcode == "ANB")
                        {
                            gefuDetail9 = "40".ToString().PadLeft(5, '0');
                        }

                        if (locationcode == "ANB")
                        {
                            gefuDetail2 = "03"; //GL
                        }


                        if (locationcode == "ANB")
                        {
                            if (decPRX > 0)
                            {
                                objwriter.WriteLine(gefuDetail1 + gefuDetail2 + gefuDetail3 + gefuDetail4 + gefuDetail5 +
                                    gefuDetail6 + gefuDetail7 + gefuDetail8 + gefuDetail9 + gefuDetail10 +
                                    gefuDetail11 + gefuDetail12 + gefuDetail13 + gefuDetail14 + gefuDetail15 +
                                    gefuDetail16 + gefuDetail17 + gefuDetail18 + gefuDetail19 + gefuDetail20 +
                                    gefuDetail21 + gefuDetail22 + gefuDetail23 + gefuDetail24 + gefuDetail25 +
                                    gefuDetail26 + gefuDetail27 + gefuDetail28 + gefuDetail29);
                                decPRX = 0;
                                decTotalDebit = 0;
                                decTotalCredit = 0;
                            }
                        }
                        else
                        {
                            if (decPRX != null)
                            {
                                objwriter.WriteLine(gefuDetail1 + gefuDetail2 + gefuDetail3 + gefuDetail4 + gefuDetail5 +
                                    gefuDetail6 + gefuDetail7 + gefuDetail8 + gefuDetail9 + gefuDetail10 +
                                    gefuDetail11 + gefuDetail12 + gefuDetail13 + gefuDetail14 + gefuDetail15 +
                                    gefuDetail16 + gefuDetail17 + gefuDetail18 + gefuDetail19 + gefuDetail20 +
                                    gefuDetail21 + gefuDetail22 + gefuDetail23 + gefuDetail24 + gefuDetail25 +
                                    gefuDetail26 + gefuDetail27 + gefuDetail28 + gefuDetail29);
                                decPRX = 0;
                                decTotalDebit = 0;
                                decTotalCredit = 0;
                            }
                        }


                        
                    }



                    if (row == myDataset.Tables[0].Rows.Count - 1)
                    {
                        objwriter.WriteLine("3" + Convert.ToString(intFooterTotalDebit).PadLeft(9, '0') +
                            Convert.ToString(decFooterTotalDebit).Replace(".", "").PadLeft(15, '0') +
                            Convert.ToString(intFooterTotalCredit).PadLeft(9, '0') +
                            Convert.ToString(decFooterTotalCredit).Replace(".", "").PadLeft(15, '0'));
                    }
                }
                objwriter.Close();

                string strrem_seq_id;
                strrem_seq_id = "";

                for (int row = 0; row < myDataset.Tables[0].Rows.Count; row++)
                {
                    if (strrem_seq_id.Trim() == "")
                    {
                        commons.execScalar("INSERT INTO TBL_GEFU_EXTRACTED_TRANSACTIONS " +
                        "(Rem_Seq_ID, " +
                        "ExtractDate, " +
                        "locationcode) " +
                        "VALUES (" + myDataset.Tables[0].Rows[row]["seq_id"].ToString() + ", " +
                        "'" + tranxdate + "', " +
                        "'" + locationcode + "')");
                        strrem_seq_id = myDataset.Tables[0].Rows[row]["seq_id"].ToString();
                    }
                    else
                    {
                        if (myDataset.Tables[0].Rows[row]["seq_id"].ToString() != strrem_seq_id)
                        {
                            commons.execScalar("INSERT INTO TBL_GEFU_EXTRACTED_TRANSACTIONS " +
                                               "(Rem_Seq_ID, " +
                                               "ExtractDate, " +
                                               "locationcode) " +
                                               "VALUES (" + myDataset.Tables[0].Rows[row]["seq_id"].ToString() + ", " +
                                               "'" + tranxdate + "', " +
                                               "'" + locationcode + "')");
                            strrem_seq_id = myDataset.Tables[0].Rows[row]["seq_id"].ToString();
                        }
                    }
                }
                if(locationcode == "ANB")
                {
                    commons.execScalar("DELETE FROM TBL_BOOKINGRATES WHERE branchcode='ANB' AND CONVERT(varchar,Effectivitydate,101)='" + tranxdate + "'");
                }
                
                insertFileRecord(locationcode, tranxdate, "GEFU" + strCOSO + locationcode + strSubFolder + ".txt", strFolder + "\\GEFU" + strCOSO + locationcode + strSubFolder + ".txt");
                return "true";
            }
            else
            {
                return "No Data to be extracted";
            }
        }

        private void insertFileRecord(string branchcode, string folderdate, string filename, string filepath)
        {
            Session["GEFUFILES"] = filename + ";";
            sqlHelperWs commons = new sqlHelperWs();
            //string x = Convert.ToString(commons.execScalar("select count(*) from tbl_gefu_reports_path where Branchcode='" + branchcode + "' and folderdate='" + folderdate + "'"));
            //if (x == "") { x = 1; }




            commons.sqlUpdate("INSERT INTO TBL_GEFU_REPORTS_PATH " +
                               "(BranchCode, " +
                               "FolderDate, " +
                               "FileName, " +
                               "FilePath) " +
                               "VALUES ('" + branchcode + "', " +
                               "'" + folderdate + "', " +
                               "'" + filename + "', " +
                               "'" + filepath + "')");
            //commons.execScalar("INSERT INTO TBL_GEFU_REPORTS_PATH " +
            //                   "(BranchCode, " +
            //                   "FolderDate, " +
            //                   "FileName, " +
            //                   "FilePath) " +
            //                   "VALUES ('" + branchcode + "', " +
            //                   "'" + folderdate + "', " +
            //                   "'" + filename + "', " +
            //                   "'" + filepath + "')");
        }

        private string TrimValue(string strValue, int TotalLen)
        {
            if (strValue.Length > TotalLen)
            {
                return strValue.Substring(0, TotalLen);
            }
            else if (strValue.Length < TotalLen)
            {
                return strValue.PadRight(TotalLen);
            }
            else
            {
                return strValue;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void checkbranchcode()
        {
            string location = IRS_CLIENT_DATA().Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Int16 check_negbranchcode(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT count(negbranchcode) as negbranchcode " +
                                                        "FROM tbl_branches " +
                                                        "WHERE branchcode = '" + branchcode + "' " +
                                                        "AND(negbranchcode IS NOT NULL AND LEN(ISNULL(negbranchcode, '')) > 0)", false);
                    return Convert.ToInt16(commons.getDataSet(sqlcommands, "negbranchcode").Tables[0].Rows[0][0]);
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return -1;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Int16 check_fc_branch_code(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT COUNT(*) as FCBranchCode " +
                                                        "FROM tbl_branches " +
                                                        "WHERE branchcode = '" + branchcode + "' " +
                                                        "AND(FCBranchCode IS NOT NULL AND LEN(ISNULL(FCBranchCode, '')) > 0) ", false);
                    return Convert.ToInt16(commons.getDataSet(sqlcommands, "fcbranchcode").Tables[0].Rows[0][0]);
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return -1;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Int16 check_tranxsource_sublookup(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT COUNT(*) " +
                                                        "FROM tbl_sub_lookup sub " +
                                                        "INNER JOIN tbl_lookup look ON look.fldvalue = sub.fldvalue " +
                                                                                      "AND look.fldgroup = sub.FldGroup " +
                                                        "WHERE sub.branchcode = '" + branchcode + "' " +
                                                        "AND look.fldgroup = 'TranxSource'", false);

                    return Convert.ToInt16(commons.getDataSet(sqlcommands, "tranxsourcesublookup").Tables[0].Rows[0][0]);
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return -1;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable Check_Is_Report_Created(string locationcode, string tranxdate, string tranxstatus, string reportname)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;
            String FileName = "";
            String strCOSO = "";
            irsConfiguration config = new irsConfiguration();
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strFolder = "";
            string strSubFolder = "";

            strSubFolder = tranxdate.Replace("/", "");
            strFolder = strPath + "\\" + strSubFolder;


            if (tranxstatus == "Credited Online")
            {
                strCOSO = "CO";
            }
            else if (tranxstatus == "Processed by GRS")
            {
                strCOSO = "PG";
            }
            else
            {
                strCOSO = "";
            }


            //locationcode, tranxdate, tranxstatus, reportname
            if (reportname == "GEFU Upload File")
            {
                FileName = "GEFU" + strCOSO + locationcode + strSubFolder + ".txt";
            }
            else if (reportname == "Summary")
            {
                FileName = "Summary" + strCOSO + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Preliminary Summary")
            {
                FileName = "PreliminarySummary" + strCOSO + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Consolidated Summary")
            {
                FileName = "Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Consolidated Summary (Online and Non-online)")
            {
                FileName = "ConsolidatedCOPG" + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Consolidated Summary (by GL Codes)")
            {
                FileName = "ConsolidatedGLCodes" + strCOSO + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Consolidated Summary (by GL Codes) - (Online and Non-online)")
            {
                FileName = "ConsolidatedGLCodesCOPG" + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Transaction Details by Receipt Mode")
            {
                FileName = "ReportByReceitMode" + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Transaction Details by Transaction Source")
            {
                FileName = "ReportByTransactionSource" + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Transactions not included in GEFU File")
            {
                FileName = "ExcludedRemittance" + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Booking Rates Used")
            {
                FileName = "BookingRatesUsed" + locationcode + "_" + strSubFolder + ".xls";
            }
            else if (reportname == "Accounting Entries Combination")
            {
                FileName = "AccountingEntries" + locationcode + "_" + strSubFolder + ".xls";
            }


            //strSubFolder = tranxdate.Replace("/","");

            //GEFU" + strCOSO + locationcode + strSubFolder + ".txt"
            //BookingRatesUsed" + locationcode + "_" + strSubFolder + ".xls"
            //ReportByReceitMode" + locationcode + "_" + strSubFolder + ".xls"
            //ReportByTransactionSource" + locationcode + "_" + strSubFolder + ".xls"
            //AccountingEntries" + locationcode + "_" + strSubFolder + ".xls"
            //PreliminarySummary" + strCOSO + locationcode + "_" + strSubFolder + ".xls"
            //Summary" + strCOSO + locationcode + "_" + strSubFolder + ".xls"
            //Consolidated" + strCOSO + locationcode + "_" + strSubFolder + ".xls"
            //ConsolidatedCOSO" + locationcode + "_" + strSubFolder + ".xls"
            //ConsolidatedGLCodes" + strCOSO + locationcode + "_" + strSubFolder + ".xls"
            //ConsolidatedGLCodesCOSO" + locationcode + "_" + strSubFolder + ".xls"
            //ExcludedRemittance" + locationcode + "_" + strSubFolder + ".xls"


            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT [filename] " +
                                                        "FROM TBL_GEFU_REPORTS_PATH " +
                                                        "WHERE branchcode = '" + locationcode + "' " +
                                                        "AND filename = '" + FileName + "'", false);

                    return commons.getDataSet(sqlcommands, "tranxsourcesublookup").Tables[0];
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Int16 check_receiptmode_sublookup(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT COUNT(*) " +
                                                        "FROM tbl_sub_lookup sub " +
                                                        "INNER JOIN tbl_lookup look ON look.fldvalue = sub.fldvalue " +
                                                                                    "AND look.fldgroup = sub.FldGroup " +
                                                        "WHERE sub.branchcode = '" + branchcode + "' " +
                                                        "AND look.fldgroup = 'receiptmode' ", false);

                    return Convert.ToInt16(commons.getDataSet(sqlcommands, "receiptmodesublookup").Tables[0].Rows[0][0]);
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return -1;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Check_Booking_Rates(string effectivitydate, string locationcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;
            DataTable dtTranxCcy = new DataTable("tranxccy");
            DataTable dtImplementationCcy = new DataTable("implementationccy");
            string strBldrCcy;
            strBldrCcy = "";

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                                "b.FldCode, " +
                                                                "b.fldvalue " +
                                                        "FROM tbl_currency_categories a " +
                                                        "INNER JOIN tbl_lookup b on a.currency = b.fldvalue where fldgroup = 'currency' " +
                                                        "AND a.branchcode = '" + locationcode + "' " +
                                                        "AND a.checked = 1  " +
                                                        "AND a.currencygroup = 1  " +
                                                        "ORDER BY b.fldvalue", false);
                    dtTranxCcy = commons.getDataSet(sqlcommands, "tranxsource").Tables[0];

                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                                "b.FldCode, " +
                                                                "b.fldvalue " +
                                                        "FROM tbl_currency_categories a " +
                                                        "INNER JOIN tbl_lookup b on a.currency = b.fldvalue where fldgroup = 'currency' " +
                                                        "AND a.branchcode = '" + locationcode + "' " +
                                                        "AND a.checked = 1  " +
                                                        "AND a.currencygroup = 2  " +
                                                        "ORDER BY b.fldvalue", false);
                    dtImplementationCcy = commons.getDataSet(sqlcommands, "tranxsource").Tables[0];


                    for (int rowTranxCcy = 0; rowTranxCcy <= dtTranxCcy.Rows.Count - 1; rowTranxCcy++)
                    {
                        for (int rowImplemCcy = 0; rowImplemCcy <= dtImplementationCcy.Rows.Count - 1; rowImplemCcy++)
                        {
                            sqlcommands = commons.getSqlCommand("SELECT COUNT(*) " +
                                                                "FROM tbl_bookingrates " +
                                                                "WHERE CONVERT(VARCHAR(10), effectivitydate, 101) = '" + effectivitydate + "' " +
                                                                "AND  branchcode = '" + locationcode + "' " +
                                                                "AND sourceccy = " + dtTranxCcy.Rows[rowTranxCcy]["fldvalue"].ToString() + " " +
                                                                "AND targetccy = " + dtImplementationCcy.Rows[rowImplemCcy]["fldvalue"].ToString() + " ", false);
                            if (Convert.ToInt16(commons.getDataSet(sqlcommands, "tranxsource").Tables[0].Rows[0][0]) == 0)
                            {
                                strBldrCcy += "*";
                                strBldrCcy += dtTranxCcy.Rows[rowTranxCcy]["FldCode"].ToString() + "~" + dtImplementationCcy.Rows[rowImplemCcy]["FldCode"].ToString();
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }

            return strBldrCcy;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool Check_AccountingEntries(string tranxdate, string locationcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
                    {
                        sqlcommands = commons.getSqlCommand("SELECT distinct " +
                                                            "b.flddescription, " +
                                                            "c.flddescription, " +
                                                            "d.flddescription, " +
                                                            "e.flddescription " +
                                                        "FROM tbl_remittance a " +
                                                        "INNER JOIN tbl_lookup b ON a.transactionsource = b.fldvalue and b.fldgroup = 'tranxsource' " +
                                                        "INNER JOIN tbl_lookup c ON a.receiptmode = c.fldvalue and c.fldgroup = 'receiptmode' " +
                                                        "INNER JOIN tbl_lookup d ON a.remittingcurrency = d.fldvalue and d.fldgroup = 'currency' " +
                                                        "INNER JOIN tbl_lookup e ON a.nettobenecurrency = e.fldvalue and e.fldgroup = 'currency' " +
                                                        "WHERE transtatus IN (2, 273, 99) " + //17,5,16,99) " +
                                                        "AND a.seq_id NOT IN( " +
                                                                            "SELECT rem.seq_id " +
                                                                            "FROM tbl_remittance rem " +
                                                                            "INNER JOIN tbl_accountingentries acc ON acc.tranxsource = rem.transactionsource " +
                                                                            "AND acc.receiptmode = rem.receiptmode " +
                                                                            "AND acc.tranxcurrency = rem.remittingcurrency " +
                                                                            "AND acc.implementationcurrency = rem.nettobenecurrency " +
                                                                            "WHERE transtatus IN (2, 273, 99) " + //17,5,16,99) " +
                                                                            "and RemittingBranch = '" + locationcode + "' " +
                                                                            "and CONVERT(VARCHAR (10), rem.tranxdate, 101) = '" + tranxdate + "' " +
                                                                            ") " +
                                                        "AND RemittingBranch = '" + locationcode + "' " +
                                                        "AND CONVERT(VARCHAR (10), tranxdate, 101) = '" + tranxdate + "' ", false);
                    }
                    else
                    {

                        string strsql = "SELECT distinct " +
                                                            "b.flddescription, " + //"c.flddescription, " +
                                                            "d.flddescription, " +
                                                            "e.flddescription " +
                                                        "FROM tbl_remittance a " +
                                                        "INNER JOIN tbl_lookup b ON a.transactionsource = b.fldvalue and b.fldgroup = 'tranxsource' " + //"INNER JOIN tbl_lookup c ON a.receiptmode = c.fldvalue and c.fldgroup = 'receiptmode' " +
                                                        "INNER JOIN tbl_lookup d ON a.remittingcurrency = d.fldvalue and d.fldgroup = 'currency' " +
                                                        "INNER JOIN tbl_lookup e ON a.nettobenecurrency = e.fldvalue and e.fldgroup = 'currency' " +
                                                        "WHERE transtatus IN (2, 273, 99) " + //17,5,16,99) " +
                                                        "AND a.seq_id NOT IN( " +
                                                                            "SELECT rem.seq_id " +
                                                                            "FROM tbl_remittance rem " +
                                                                            "INNER JOIN tbl_accountingentries acc ON acc.tranxsource = rem.transactionsource " + //"AND acc.receiptmode = rem.receiptmode " +
                                                                            "AND acc.tranxcurrency = rem.remittingcurrency " +
                                                                            "AND acc.implementationcurrency = rem.nettobenecurrency " +
                                                                            "WHERE transtatus IN (2, 273, 99) " + //17,5,16,99) " +
                                                                            "and RemittingBranch = '" + locationcode + "' " +
                                                                            "and CONVERT(VARCHAR (10), rem.tranxdate, 101) = '" + tranxdate + "' " +
                                                                            ") " +
                                                        "AND RemittingBranch = '" + locationcode + "' " +
                                                        "AND CONVERT(VARCHAR (10), tranxdate, 101) = '" + tranxdate + "' ";


                        sqlcommands = commons.getSqlCommand(strsql, false);
                    }

                    return Create_Incomplete_Accounting_Entries(commons.getDataSet(sqlcommands, "AccountingEntries"), tranxdate, locationcode);
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet check_data(string date, string reportname)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;

            if (reportname == "11")//bookingrates
            {
                try
                {
                    if (commons != null)
                    {
                        sqlcommands = commons.getSqlCommand("SELECT * " +
                                                            "FROM tbl_bookingrates " +
                                                            "WHERE effectivitydate = '" + date + "'", false);
                        return commons.getDataSet(sqlcommands, "tranxsource");
                    }
                }
                catch
                {
                    return null;
                }
                finally
                {
                    if (commons != null)
                    {
                        commons = null;
                    }
                    if (sqlcommands != null)
                    {
                        sqlcommands.Dispose();
                        sqlcommands = null;
                    }
                }
                return null;
            }
            return null;
        }
        #endregion

        #region PRIVATE METHOD: GEFU_Maintenance

        private bool Create_Incomplete_Accounting_Entries(DataSet myDataset, string tranxdate, string locationcode)
        {
            if (myDataset.Tables[0].Rows.Count == 0)
            {
                return false;
            }
            irsConfiguration config = new irsConfiguration();
            string mainpath = config.appsetting("ReportExportPath");
            string strPath = mainpath + "\\IRSWorldGEFUReports\\" + locationcode;
            string strSubFolder = "";
            strSubFolder = tranxdate.Replace("/", "");
            string strFolder = strPath + "\\" + strSubFolder;
            if (Directory.Exists(strPath))
            {
                if (Directory.Exists(strFolder) == false)
                {
                    Directory.CreateDirectory(strFolder);
                }
            }
            else
            {
                Directory.CreateDirectory(strPath);
                Directory.CreateDirectory(strFolder);
            }

            try
            {
                objwriter = new StreamWriter(strFolder + "\\Additional_AccountingEntries_" + locationcode + "_" + strSubFolder + ".xls");
                printHTML(true);
                printBody(true);
                printTable(true);

                printTR(true);
                printTD(true, "Add the following Accounting Entries ", "4", "left", "bold");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "", "4", "", "");
                printTD(false);
                printTR(false);

                printTR(true);
                printTD(true, "tranxsource", "1", "center", "bold");
                printTD(false);

                if (locationcode != "LDN" && locationcode != "LDE" && locationcode != "LDI" && locationcode != "LDA")
                {
                    printTD(true, "receiptmode", "1", "center", "bold");
                    printTD(false);
                }

                printTD(true, "Remitting Ccy", "1", "center", "bold");
                printTD(false);
                printTD(true, "Implementation Ccy", "1", "center", "bold");
                printTD(false);
                printTR(false);

                for (int r = 0; r < myDataset.Tables[0].Rows.Count; r++)
                {
                    printTR(true);
                    for (int c = 0; c < myDataset.Tables[0].Columns.Count; c++)
                    {
                        printTD(true, myDataset.Tables[0].Rows[r][c].ToString(), "1", "left", "normal");
                        printTD(false);
                    }
                    printTR(false);
                }

                printTable(false);
                printBody(false);
                printHTML(false);
                objwriter.Close();
            }
            catch  // commented to remove warning (Exception e)
            {
                return false;
            }
            finally
            {
            }
            insertFileRecord(locationcode, tranxdate, "Additional_AccountingEntries_" + locationcode + "_" + strSubFolder + ".xls", strFolder + "\\Additional_AccountingEntries_" + locationcode + "_" + strSubFolder + ".xls");
            return true;
        }

        private irsClientData IRS_CLIENT_DATA()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
                return (irsClientData)Session["IRS_CLIENT_DATA"];
            else
            {
                Response.Redirect("ErrorPage.aspx");
                return null;
            }
        }

        private void printHeader(string isStartHeader)
        {


        }

        private void printHTML(Boolean isStartHTML)
        {
            if (isStartHTML)
            {
                objwriter.WriteLine("<html>");
            }
            else
            {
                objwriter.WriteLine("</html>");
            }
        }

        private void printBody(Boolean isStartBody)
        {
            if (isStartBody)
            {
                objwriter.WriteLine("\t<body>");
            }
            else
            {
                objwriter.WriteLine("\t</body>");
            }
        }

        private void printTable(Boolean isStartTable)
        {
            if (isStartTable)
            {
                objwriter.WriteLine("\t\t<Table>");
            }
            else
            {
                objwriter.WriteLine("\t\t</Table>");
            }
        }

        private void printTR(Boolean isStartTR)
        {
            if (isStartTR)
            {
                objwriter.WriteLine("\t\t\t<tr>");
            }
            else
            {
                objwriter.WriteLine("\t\t\t</tr>");
            }
        }

        private void printTD(Boolean isStartTD)
        {
            printTD(isStartTD, "", "", "", "");
        }

        private void printTD(Boolean isStartTD, string TDValue, string colspan, string Align, string Weight)
        {
            if (isStartTD)
            {
                objwriter.WriteLine("\t\t\t\t<td colspan=" + "\"" + colspan + "\"" + " style=\"text-align:" + Align + "; font-weight:" + Weight + "\">" + TDValue);
            }
            else
            {
                objwriter.WriteLine("\t\t\t\t</td>");
            }
        }

        private irsGEFUMaintenance gefu_Maintenance()
        {
            if (Session["GEFU_MAINTENANCE"] == null)
            {
                Session["GEFU_MAINTENANCE"] = new irsGEFUMaintenance();
            }
            return (irsGEFUMaintenance)Session["GEFU_MAINTENANCE"];
        }

        private irsAccountingEntries accountingentries_Maintenance()
        {
            if (Session["ACCOUNTINGENTRIES_MAINTENANCE"] == null)
            {
                Session["ACCOUNTINGENTRIES_MAINTENANCE"] = new irsAccountingEntries();
            }
            return (irsAccountingEntries)Session["ACCOUNTINGENTRIES_MAINTENANCE"];
        }

        private irsIflexAccounts iflexaccounts_Maintenance()
        {
            if (Session["IFLEXACCOUNTS_MAINTENANCE"] == null)
            {
                Session["IFLEXACCOUNTS_MAINTENANCE"] = new irsIflexAccounts();
            }
            return (irsIflexAccounts)Session["IFLEXACCOUNTS_MAINTENANCE"];
        }

        private csAuditLog gefuMaintenance_AuditLog()
        {
            if (Session["GEFUMAINTENANCE_AUDITLOG"] == null)
            {
                Session["GEFUMAINTENANCE_AUDITLOG"] = new csAuditLog();
            }
            return (csAuditLog)Session["GEFUMAINTENANCE_AUDITLOG"];
        }

        private irsBookingRates bookingrates_Maintenance()
        {
            if (Session["BOOKINGRATES_MAINTENANCE"] == null)
            {
                Session["BOOKINGRATES_MAINTENANCE"] = new irsBookingRates();
            }
            return (irsBookingRates)Session["BOOKINGRATES_MAINTENANCE"];
        }

        private bool select_do_instruction(string instruction)
        {
            if ((string)Session["DIV_NAME"] == "div_AccountingEntries")
            {
                return this.do_AccountingEntries(instruction);
            }
            else if ((string)Session["DIV_NAME"] == "div_IflexAccounts")
            {
                return this.do_IflexAccounts(instruction);
            }
            else if ((string)Session["DIV_NAME"] == "div_BookingRates")
            {
                return this.do_BookingRates(instruction);
            }
            return false;
        }

        private bool do_AccountingEntries(string instruction)
        {
            irsAccountingEntries irsAccountingEntries = this.accountingentries_Maintenance();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            irsAccountingEntries.selectedRow = -1;
                            if (irsAccountingEntries.insert_record() == false)
                            {
                                this.set_message(irsAccountingEntries.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            irsAccountingEntries.seq_id = this.get_seq_id();
                            irsAccountingEntries.selectedRow = 0;

                            if (irsAccountingEntries.update_record() == false)
                            {
                                this.set_message(irsAccountingEntries.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_ACCOUNTINGENTRIES_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (irsAccountingEntries.commit_record() == false)
                            {
                                this.set_message(irsAccountingEntries.messages); return false;
                            }

                            sqlHelperWs commons = new sqlHelperWs();
                            SqlCommand sqlcommands = null;
                            DataTable dtGLCodes = new DataTable();
                            for (int i = 6; i < 14; i++)//strglcodes.Length - 1; i++)
                            {
                                string[] loc = strglcodes[1].Split('~');
                                string[] s = strglcodes[i].Split('~');

                                if (s[1].Trim() != "")
                                {
                                    sqlcommands = commons.getSqlCommand("SELECT * " +
                                                                    "FROM tbl_iflexaccounts " +
                                                                    "WHERE gltype = '" + s[0] + "' " +
                                                                    "AND iflexaccount = '" + s[1] + "'", false);
                                    dtGLCodes = commons.getDataSet(sqlcommands, "glcodes").Tables[0];

                                    if (dtGLCodes.Rows.Count == 0)
                                    {
                                        commons.execScalar("INSERT INTO TBL_IFLEXACCOUNTS " +
                                                           "VALUES ('" + s[1] + "', " +
                                                           "'', " +
                                                           "'" + s[0] + "', " +
                                                           "'" + loc[1] + "')");
                                    }
                                }
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            irsAccountingEntries.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "DELETE")
                    {
                        try
                        {
                            irsAccountingEntries.seq_id = this.get_seq_id();
                            irsAccountingEntries.selectedRow = 0;

                            if (irsAccountingEntries.delete_record() == false)
                            {
                                this.set_message(irsAccountingEntries.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                }
            }
            catch
            {
            }
            finally
            {
                Session["ACCOUNTINGENTRIES_MAINTENANCE"] = null;
                Session["ACCOUNTINGENTRIES_MAINTENANCE"] = irsAccountingEntries;
                Session["TABLE_SEQ_ID"] = null;
                Session["TABLE_SEQ_ID"] = irsAccountingEntries.seq_id;
                irsAccountingEntries = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_bookingrate(string seq_id)
        {
            sqlHelperWs ws = new sqlHelperWs();
            object o = ws.execScalar("DELETE FROM TBL_BOOKINGRATES WHERE SEQ_ID='" + seq_id + "'");
            if (Convert.ToInt32(o) >= 0)
            {
                return true;
            }

            return false;
        }


        private bool do_BookingRates(string instruction)
        {
            irsBookingRates bookingrates = this.bookingrates_Maintenance();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            bookingrates.selectedRow = -1;
                            if (bookingrates.insert_record() == false)
                            {
                                this.set_message(bookingrates.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_BOOKINGRATES_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (bookingrates.commit_record() == false)
                            {
                                this.set_message(bookingrates.messages); return false;
                            }
                            else
                            {

                                bookingrates.selectedRow = -1;
                                bookingrates.insert_record();
                                if (this.SET_BOOKINGRATES_RECORD_AGAIN(this.parameter_values) == true)
                                {
                                    bookingrates.commit_record();
                                }
                            }

                            //set_message
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            bookingrates.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["BOOKINGRATES_MAINTENANCE"] = null;
                Session["BOOKINGRATES_MAINTENANCE"] = bookingrates;
                Session["TABLE_SEQ_ID"] = null;
                Session["TABLE_SEQ_ID"] = bookingrates.seq_id;
                bookingrates = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private bool do_IflexAccounts(string instruction)
        {
            irsIflexAccounts irsIflexAccounts = this.iflexaccounts_Maintenance();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            irsIflexAccounts.seq_id = this.TABLE_SEQ_ID();
                            irsIflexAccounts.selectedRow = 0;

                            if (irsIflexAccounts.update_record() == false)
                            {
                                this.set_message(irsIflexAccounts.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_IFLEXACCOUNTS_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (irsIflexAccounts.commit_record() == false)
                            {
                                this.set_message(irsIflexAccounts.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            irsIflexAccounts.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["IFLEXACCOUNTS_MAINTENANCE"] = null;
                Session["IFLEXACCOUNTS_MAINTENANCE"] = irsIflexAccounts;
                Session["TABLE_SEQ_ID"] = null;
                Session["TABLE_SEQ_ID"] = irsIflexAccounts.seq_id;
                irsIflexAccounts = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private string TABLE_SEQ_ID()
        {
            if (Session["TABLE_SEQ_ID"] != null)
            {
                return (string)Session["TABLE_SEQ_ID"];
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private void CreateLogs(string strMenuModule, string strLogDescription)
        {
            csAuditLog gefuMaintenanceLog = this.gefuMaintenance_AuditLog();
            gefuMaintenanceLog.writeAudit(string.Empty, "GEFUMaintenance-" + strMenuModule, strLogDescription, "", "", csAuditLog.Category.Select);

            //new csAuditLog().writeAudit("", Session["InternetARMModule"].ToString(), sDescription, NewValue, OldValue, Category);


            //            createLogs("[UserID=" + UserID + "]" +
            //                           "[Module=" + Module + "]" +
            //                           "[Description=" + Description + "]" +
            //                           "[NewValue=" + NewValue + "]" +
            //                           "[OldValue=" + OldValue + "]" +
            //                           "[Category=" + Category + "]" +
            //                           "[Exception=" + ex.Message + "]");


            //sessionid
            //userid
            //accessdate
            //module
            //description
            //newvalue
            //oldvalue
            //category


        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string checkbookrates_exist(string source, string target, string effectdate, string brcode)
        {
            sqlHelperWs ws = new sqlHelperWs();
            DataSet ds = ws.getDataSet(ws.getSqlCommand("select top 1 * from tbl_bookingrates where SourceCcy='" + source + "' and TargetCcy='" + target + "' and convert(varchar,effectivitydate,101)='" + Convert.ToDateTime(effectdate).ToString("MM/dd/yyyy") + "' and BranchCode='" + brcode + "'", false));
            if (ds != null)
            {
                return ds.Tables[0].Rows.Count.ToString();
            }
            return string.Empty;
        }

        #endregion
    }
}
