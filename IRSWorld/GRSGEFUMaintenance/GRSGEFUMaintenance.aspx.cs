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

namespace IRSWorld.GRSGEFUMaintenance
{
    public partial class GRSGEFUMaintenance : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

    #region
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GRSGEFUMaintenance));
        }       

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void ActiveModule(string DivName)
        {
            Session["DIV_NAME"] = DivName;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.select_do_instruction("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string _seq_id)
        {
            Session["TABLE_SEQ_ID"] = _seq_id;
            return this.select_do_instruction("DELETE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool create_grs_gefu(string tranxdate)
        {
            irsGRSGEFU irsGRSGEFU = this.GRS_GEFU_Maintenance();
            return irsGRSGEFU.create_grs_gefu(tranxdate);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccountCcy()
        {
            sqlHelperWs  commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldCode " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'currency' " +
                                                        "AND fldvalue IN (1,2,5,6) " +
                                                        "ORDER BY FldCode", false);
                    return commons.getDataSet(sqlcommands, "AccountCcy");
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
        public DataSet getCorrBankCcy()
        {
            sqlHelperWs  commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldCode " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'currency'", false);
                    return commons.getDataSet(sqlcommands, "CorrBankCcy");
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
        public DataSet getCorrBanCountry()
        {
            sqlHelperWs  commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldDescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'ibatchcountry' " +
                                                        "ORDER BY FldDescription", false);
                    return commons.getDataSet(sqlcommands, "CorrBankCountry");
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
        public DataSet getChargeType()
        {
            sqlHelperWs  commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
          
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldDescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'chargetype' " +
                                                        "AND flddescription LIKE 'DUE%'" +
                                                        "ORDER BY FldDescription", false);
                    return commons.getDataSet(sqlcommands, "CorrBankCountry");
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
        public DataSet GET_GRS_CORRBANK_RECORD(string grs_corrbank_seq_id)
        {
            irsGRSCorrBank irsGRSCorrBank  = this.GRS_CorrBank_Maintenance();
            try
            {
                if (irsGRSCorrBank != null)
                {
                    if (irsGRSCorrBank.get_record(grs_corrbank_seq_id) == true)
                    {
                        return irsGRSCorrBank.getDataRecord();
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
                Session["GRS_CORRBANK_MAINTENANCE"] = irsGRSCorrBank;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_GRS_ACCOUNTINGENTRIES_RECORD(string grs_accountingentries_seq_id)
        {
            irsGRSAccountingEntries irsGRSAccountingEntries = this.GRS_Accountingentries_Maintenance();
            try
            {
                if (irsGRSAccountingEntries != null)
                {
                    if (irsGRSAccountingEntries.get_record(grs_accountingentries_seq_id) == true)
                    {
                        return irsGRSAccountingEntries.getDataRecord();
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
                Session["GRS_ACCOUNTINGENTRIES_MAINTENANCE"] = irsGRSAccountingEntries;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_BUYINGRATES_RECORD(string buyingrates_seq_id)
        {
            irsGRSBuyingRate buyingrates = this.buyingrates_Maintenance();
            try
            {
                if (buyingrates != null)
                {
                    if (buyingrates.get_record(buyingrates_seq_id) == true)
                    {
                        return buyingrates.getDataRecord();
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
                Session["BUYINGRATES_MAINTENANCE"] = buyingrates;
            }
        }

        private irsGRSBuyingRate buyingrates_Maintenance()
        {
            if (Session["BUYINGRATES_MAINTENANCE"] == null)
            {
                Session["BUYINGRATES_MAINTENANCE"] = new irsGRSBuyingRate();
            }
            return (irsGRSBuyingRate)Session["BUYINGRATES_MAINTENANCE"];
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_seq_id()
        {
            return this.TABLE_SEQ_ID();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getImplementationType()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "flddescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'implementationtype' " +
                                                        "ORDER BY flddescription", false);
                    return commons.getDataSet(sqlcommands, "ProductCode");
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
        public DataSet getTransactionSource()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "flddescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'IBATCH' " +
                                                        "ORDER BY flddescription", false);
                    return commons.getDataSet(sqlcommands, "TransactionSource");
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
        public DataSet getRecordID()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "flddescription " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'IBatch_Record_ID' " +
                                                        "ORDER BY flddescription", false);
                    return commons.getDataSet(sqlcommands, "TransactionSource");
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
        public DataSet getTransactionCcy()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT fldvalue, " +
                                                        "FldCode " +
                                                        "FROM tbl_lookup " +
                                                        "WHERE fldgroup = 'currency' " +
                                                        "ORDER BY FldCode", false);
                    return commons.getDataSet(sqlcommands, "TransactionCcy");
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
        public bool insert_record()
        {
            return this.select_do_instruction("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["GRS_GEFU_MAINTENANCE_MESSAGE"] != null)
            {
                return (string)Session["GRS_GEFU_MAINTENANCE_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.select_do_instruction("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_GRS_ACCOUNTINGENTRIES_RECORD(string column_values)
        {
            irsGRSAccountingEntries irsGRSAccountingEntries = this.GRS_Accountingentries_Maintenance();
            string[] s = column_values.Split('$');
            
            if (s != null && s.Length > 0)
            {
                if (irsGRSAccountingEntries.set_record(s) == false)
                {
                    this.set_message(irsGRSAccountingEntries.messages); return false;
                }
                Session["GRS_ACCOUNTINGENTRIES_MAINTENANCE"] = irsGRSAccountingEntries;
                return true;
            }
            irsGRSAccountingEntries = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_GRS_ACCOUNTINGENTRIES_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_GRS_CORRBANK_RECORD(string column_values)
        {
            irsGRSCorrBank irsGRSCorrBank = this.GRS_CorrBank_Maintenance();
            string[] s = column_values.Split('$');

            if (s != null && s.Length > 0)
            {
                if (irsGRSCorrBank.set_record(s) == false)
                {
                    this.set_message(irsGRSCorrBank.messages); return false;
                }
                Session["GRS_CORRBANK_MAINTENANCE"] = irsGRSCorrBank;
                return true;
            }
            irsGRSCorrBank = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_GRS_CORRBANK_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_BUYINGRATES_RECORD(string column_values)
        {
            string User = "";
            User = IRS_CLIENT_DATA().UserId;

            column_values = column_values.Replace("UUUIII", User);

            irsGRSBuyingRate irsGRSBuyingRate = this.GRS_BuyingRate_Maintenance();
            string[] s = column_values.Split('$');

            if (s != null && s.Length > 0)
            {
                if (irsGRSBuyingRate.set_record(s) == false)
                {
                    this.set_message(irsGRSBuyingRate.messages); return false;
                }
                Session["GRS_BUYING_MAINTENANCE"] = irsGRSBuyingRate;
                return true;
            }

            irsGRSBuyingRate = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_BUYINGRATES_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["GRS_GEFU_MAINTENANCE_MESSAGE"] = null;
            Session["GRS_GEFU_MAINTENANCE_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string SEQ_ID)
        {
            Session["TABLE_SEQ_ID"] = SEQ_ID;
            return this.select_do_instruction("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getSourceCcy()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                                "DISTINCT ccy.currency as fldvalue, " +
                                                                "look.FldDescription " +
                                                        "FROM       tbl_currency_categories ccy " +
                                                        "INNER JOIN tbl_lookup			    look	ON ccy.currency = look.fldvalue " +
                                                        "WHERE      branchcode          = '1170' " +
                                                            "AND    look.fldgroup       = 'currency' " +
                                                            "AND    ccy.checked         = 1 " +
                                                            "AND    ccy.currencygroup   = 1 " +
                                                        "ORDER BY look.FldDescription", false);

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
        public DataSet getTargetCcy()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqlcommands = null;

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("SELECT " +
                                                                "DISTINCT ccy.currency as fldvalue, " +
                                                                "look.FldDescription " +
                                                        "FROM       tbl_currency_categories ccy " +
                                                        "INNER JOIN tbl_lookup			    look	ON ccy.currency = look.fldvalue " +
                                                        "WHERE      branchcode          = '1170' " +
                                                            "AND    look.fldgroup       = 'currency' " +
                                                            "AND    ccy.checked         = 1 " +
                                                            "AND    ccy.currencygroup   = 2 " +
                                                        "ORDER BY look.FldDescription", false);

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

    #endregion

    #region

        private irsGRSGEFU GRS_GEFU_Maintenance()
        {
            if (Session["GRS_GEFU_MAINTENANCE"] == null)
            {
                Session["GRS_GEFU_MAINTENANCE"] = new irsGRSGEFU ();
            }
            return (irsGRSGEFU)Session["GRS_GEFU_MAINTENANCE"];
        }

        private irsGRSBuyingRate GRS_BuyingRate_Maintenance()
        {
            if (Session["GRS_BUYING_MAINTENANCE"] == null)
            {
                Session["GRS_BUYING_MAINTENANCE"] = new irsGRSBuyingRate ();
            }
            return (irsGRSBuyingRate)Session["GRS_BUYING_MAINTENANCE"];
        }

        private irsGRSCorrBank GRS_CorrBank_Maintenance()
        {
            if (Session["GRS_CORRBANK_MAINTENANCE"] == null)
            {
                Session["GRS_CORRBANK_MAINTENANCE"] = new irsGRSCorrBank ();
            }
            return (irsGRSCorrBank)Session["GRS_CORRBANK_MAINTENANCE"];
        }

        private irsGRSAccountingEntries GRS_Accountingentries_Maintenance()
        {
            if (Session["GRS_ACCOUNTINGENTRIES_MAINTENANCE"] == null)
            {
                Session["GRS_ACCOUNTINGENTRIES_MAINTENANCE"] = new irsGRSAccountingEntries();
            }
            return (irsGRSAccountingEntries)Session["GRS_ACCOUNTINGENTRIES_MAINTENANCE"];
        }

        private bool do_GRS_AccountingEntries(string instruction)
        {
            irsGRSAccountingEntries irsGRSAccountingEntries = this.GRS_Accountingentries_Maintenance();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            irsGRSAccountingEntries.selectedRow = -1;
                            if (irsGRSAccountingEntries.insert_record() == false)
                            {
                                this.set_message(irsGRSAccountingEntries.messages); return false;
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
                            irsGRSAccountingEntries.seq_id = this.get_seq_id();
                            irsGRSAccountingEntries.selectedRow = 0;

                            if (irsGRSAccountingEntries.update_record() == false)
                            {
                                this.set_message(irsGRSAccountingEntries.messages); return false;
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
                            if (this.SET_GRS_ACCOUNTINGENTRIES_RECORD (this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (irsGRSAccountingEntries.commit_record() == false)
                            {
                                this.set_message(irsGRSAccountingEntries.messages); return false;
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
                            irsGRSAccountingEntries.rollback_record(); return true;
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
                            irsGRSAccountingEntries.seq_id = this.get_seq_id();
                            irsGRSAccountingEntries.selectedRow = 0;

                            if (irsGRSAccountingEntries.delete_record() == false)
                            {
                                this.set_message(irsGRSAccountingEntries.messages); return false;
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
                Session["GRS_ACCOUNTINGENTRIES_MAINTENANCE"] = null;
                Session["GRS_ACCOUNTINGENTRIES_MAINTENANCE"] = irsGRSAccountingEntries;
                Session["TABLE_SEQ_ID"] = null;
                Session["TABLE_SEQ_ID"] = irsGRSAccountingEntries.seq_id;
                irsGRSAccountingEntries = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private bool do_GRS_CorrBank(string instruction)
        {
            irsGRSCorrBank irsGRSCorrBank = this.GRS_CorrBank_Maintenance();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            irsGRSCorrBank.selectedRow = -1;
                            if (irsGRSCorrBank.insert_record() == false)
                            {
                                this.set_message(irsGRSCorrBank.messages); return false;
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
                            irsGRSCorrBank.seq_id = this.get_seq_id();
                            irsGRSCorrBank.selectedRow = 0;

                            if (irsGRSCorrBank.update_record() == false)
                            {
                                this.set_message(irsGRSCorrBank.messages); return false;
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
                            if (this.SET_GRS_CORRBANK_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (irsGRSCorrBank.commit_record() == false)
                            {
                                this.set_message(irsGRSCorrBank.messages); return false;
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
                            irsGRSCorrBank.rollback_record(); return true;
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
                            irsGRSCorrBank.seq_id = this.get_seq_id();
                            irsGRSCorrBank.selectedRow = 0;

                            if (irsGRSCorrBank.delete_record() == false)
                            {
                                this.set_message(irsGRSCorrBank.messages); return false;
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
                Session["GRS_CORRBANK_MAINTENANCE"] = null;
                Session["GRS_CORRBANK_MAINTENANCE"] = irsGRSCorrBank;
                Session["TABLE_SEQ_ID"] = null;
                Session["TABLE_SEQ_ID"] = irsGRSCorrBank.seq_id;
                irsGRSCorrBank = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private bool do_GRS_BuyingRate(string instruction)
        {
            irsGRSBuyingRate irsGRSBuyingRates = this.GRS_BuyingRate_Maintenance();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            irsGRSBuyingRates.selectedRow = -1;
                            if (irsGRSBuyingRates.insert_record() == false)
                            {
                                this.set_message(irsGRSBuyingRates.messages); return false;
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
                            irsGRSBuyingRates.seq_id = this.get_seq_id();
                            irsGRSBuyingRates.selectedRow = 0;

                            if (irsGRSBuyingRates.update_record() == false)
                            {
                                this.set_message(irsGRSBuyingRates.messages); return false;
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
                            if (this.SET_BUYINGRATES_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (irsGRSBuyingRates.commit_record() == false)
                            {
                                this.set_message(irsGRSBuyingRates.messages); return false;
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
                            irsGRSBuyingRates.rollback_record(); return true;
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
                            irsGRSBuyingRates.seq_id = this.get_seq_id();
                            irsGRSBuyingRates.selectedRow = 0;

                            if (irsGRSBuyingRates.delete_record() == false)
                            {
                                this.set_message(irsGRSBuyingRates.messages); return false;
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
                Session["GRS_BUYING_MAINTENANCE"] = null;
                Session["GRS_BUYING_MAINTENANCE"] = irsGRSBuyingRates;
                Session["TABLE_SEQ_ID"] = null;
                Session["TABLE_SEQ_ID"] = irsGRSBuyingRates.seq_id;
                irsGRSBuyingRates = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private bool select_do_instruction(string instruction)
        {
            if ((string)Session["DIV_NAME"] == "div_GRS_AccountingEntries")
            {
                return this.do_GRS_AccountingEntries(instruction);
            }

            else if ((string)Session["DIV_NAME"] == "div_GRS_CorrBank")
            {
                return this.do_GRS_CorrBank(instruction);
            }
            else if ((string)Session["DIV_NAME"] == "div_GRS_BuyingRates")
            {
                return this.do_GRS_BuyingRate(instruction);
            }
            //else if ((string)Session["DIV_NAME"] == "div_GEFU_Reports")
            //{
            //    return this.do_GEFUReports(instruction);
            //}
            return false;
        }
        private string TABLE_SEQ_ID()
        {
            if (Session["TABLE_SEQ_ID"] != null)
            {
                return (string)Session["TABLE_SEQ_ID"];
            }
            return null;
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
    #endregion
    }
}
