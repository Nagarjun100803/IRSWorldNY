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

namespace IRSWorld.IRSFileLoader
{
    public partial class ComplianceViewer : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ComplianceViewer));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_COMPLIANCE_VIEWER_RECORD(string client_seq_id)
        {
            irsComplianceViewer complianceviewer = this.COMPLIANCE_VIEWER();
            try
            {
                if (complianceviewer != null)
                {
                    if (complianceviewer.get_record(client_seq_id) == true)
                    {
                        return complianceviewer.getdataRecord();
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
                Session["IRS_COMPLIANCE_VIEWER"] = complianceviewer;
            }
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public bool update_record(string field_group)
        //{
        //    Session["COMPLIANCE_VIEWER_SEQ_ID"] = field_group;

        //    return this.do_compliance_viewer("UPDATE");
        //}

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public bool commit_record(string param_values)
        //{
        //    this.parameter_values = param_values;
        //    return this.do_compliance_viewer("COMMIT");
        //}

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public bool rollback_record()
        //{
        //    return this.do_compliance_viewer("ROLLBACK");
        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["COMPLIANCEVIEWER_MESSAGE"] != null)
            {
                return (string)Session["COMPLIANCEVIEWER_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["COMPLIANCEVIEWER_MESSAGE"] = null;
            Session["COMPLIANCEVIEWER_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_COMPLIANCEVIEWER_RECORD(string column_values)
        {
            irsComplianceViewer complianceviewer = this.COMPLIANCE_VIEWER();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (complianceviewer.set_record(s) == false)
                {
                    this.set_message(complianceviewer.messages); return false;
                }

                Session["IRS_COMPLIANCE_VIEWER"] = complianceviewer;

                return true;
            }

            complianceviewer = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_COMPLIANCEVIEWER_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string COMPLIANCE_VIEWER_SEQ_ID()
        {
            if (Session["COMPLIANCE_VIEWER_SEQ_ID"] != null)
            {
                return (string)Session["COMPLIANCE_VIEWER_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCode()
        {
            return IRS_CLIENT_DATA().Location;
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getsearchby_source()
        {
            sqlHelperWs commons = new sqlHelperWs ();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_searchfield");
                    return commons.getDataSet(sqcommands, "SearchBy");
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
        public DataSet getdropdown_source(string branchcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdowncompliance");
                    sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 5).Value = branchcode;

                    return commons.getDataSet(sqcommands, "Compliance_Viewer");
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
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommons com = new irsSqlCommons();
                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "RemBenNames");

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        #endregion
        #region PRIVATE METHOD: COMPLIANCE_VIEWER
        private irsComplianceViewer COMPLIANCE_VIEWER()
        {
            if (Session["IRS_COMPLIANCE_VIEWER"] == null)
            {
                Session["IRS_COMPLIANCE_VIEWER"] = new irsComplianceViewer();
            }

            return (irsComplianceViewer)Session["IRS_COMPLIANCE_VIEWER"];
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
