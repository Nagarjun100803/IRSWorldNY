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

namespace IRSWorld.Earthport
{
    public partial class VANTerritory : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VANTerritory));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_country()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("spz_getCountry");
                    return commons.getDataSet(sqcommands, "country");
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
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return null;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_currency()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("spz_getRemittingCurrency");
                    return commons.getDataSet(sqcommands, "currency");
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
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return null;

        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string VANTerritoryID()
        {
            if (Session["VANTerritoryID"] != null)
            {
                return (string)Session["VANTerritoryID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["VANTerritory_MESSAGE"] != null)
            {
                return (string)Session["VANTerritory_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["VANTerritory_MESSAGE"] = null;
            Session["VANTerritory_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_VANTERRITORY_RECORD(string column_values)
        {
            irsVANTerritory VANSetup = this.VANTerritory_Maintenance();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (VANSetup.set_record(s) == false)
                {
                    this.set_message(VANSetup.messages); return false;
                }

                Session["VANSetup"] = VANSetup;

                return true;
            }

            VANSetup = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_VANTERRITORY_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_VANTERRITORY_RECORD(string VanTerritoryID)
        {
            irsVANTerritory VANSetup = this.VANTerritory_Maintenance();
            try
            {
                if (VANSetup != null)
                {
                    if (VANSetup.get_record(VanTerritoryID) == true)
                    {
                        return VANSetup.getdataRecord();
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
                Session["VANSetup"] = VANSetup;
            }
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_VANTerritory_Maintenance("ROLLBACK");
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_VANTerritory_Maintenance("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string strVANTerritoryID)
        {
            Session["VANTerritoryID"] = strVANTerritoryID;

            return this.do_VANTerritory_Maintenance("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string strVANTerritoryID)
        {
            return this.update_record(strVANTerritoryID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_VANTerritory_Maintenance("COMMIT");
        }


        #endregion

        #region PRIVATE METHOD : VANTerritory_Maintenance

        private irsVANTerritory VANTerritory_Maintenance()
        {
            if (Session["VANSetup"] == null)
            {
                Session["VANSetup"] = new irsVANTerritory();
            }

            return (irsVANTerritory)Session["VANSetup"];
        }

        private bool do_VANTerritory_Maintenance(string instruction)
        {
            irsVANTerritory ivt = this.VANTerritory_Maintenance();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            ivt.selectedRow = -1;
                            if (ivt.insert_record() == false)
                            {
                                this.set_message(ivt.messages); return false;
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
                            ivt.VANTerritoryID = this.VANTerritoryID();
                            ivt.selectedRow = 0;

                            if (ivt.update_record() == false)
                            {
                                this.set_message(ivt.messages); return false;
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
                            if (this.SET_VANTERRITORY_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (ivt.commit_record() == false)
                            {
                                this.set_message(ivt.messages); return false;
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
                            ivt.rollback_record(); return true;
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

                Session["VANSetup"] = null;
                Session["VANSetup"] = ivt;
                Session["VANTerritoryID"] = null;
                Session["VANTerritoryID"] = ivt.VANTerritoryID;
                ivt = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }




        #endregion

    }
}
