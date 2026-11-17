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
using IRSWorld.TranClass;

namespace IRSWorld.BranchSetup
{
    public partial class PaymentGatewayDefaults : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(PaymentGatewayDefaults));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_type(string fldvalue)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_branch_sagepay_type");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldvalue;

                    return commons.getDataSet(sqcommands, "TxType");
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
        public DataSet check_seqid(int region_id)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_check_Gateway_SEQ_ID");
                    sqcommands.Parameters.Add("@RegionID", SqlDbType.BigInt).Value = region_id;
                    return commons.getDataSet(sqcommands, "RegionID");
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet verify_seqid(int x_country)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT * FROM TBL_BRANCH_GATEWAY_DEFAULTS WHERE x_country = " + x_country, false);

                    return commons.getDataSet(sqcommands, "SEQ_ID");
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
        public DataSet get_country(string region)
        {
            sqlHelperWs commons = new sqlHelperWs();
            //SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    //sqcommands = commons.getSqlCommand("SELECT FldValue, FldDescription FROM TBL_LOOKUP WHERE FldGroup = 'Country' AND FldCode IN ('US', 'CA')", false);
                    //sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet("SELECT FldValue, FldDescription FROM TBL_LOOKUP WHERE FldGroup = 'Country' AND FLDVALUE in (select COUNTRY from tbl_branches where chargegroup = '" + region + "')", false, new object[] { });
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
                //if (sqcommands != null)
                //{
                //    sqcommands.Dispose();
                //    sqcommands = null;
                //}
            }

            return null;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_region(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_PaymentRegion");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "PaymentRegion");
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
        #endregion

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_branch_gateway("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["BRANCH_GATEWAY_SEQ_ID"] = field_group;

            return this.do_branch_gateway("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record()
        {
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_branch_gateway("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_branch_gateway("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["BRANCHGATEWAY_MESSAGE"] != null)
            {
                return (string)Session["BRANCHGATEWAY_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["BRANCHGATEWAY_MESSAGE"] = null;
            Session["BRANCHGATEWAY_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_BRANCHGATEWAY_RECORD(string column_values)
        {
            irsBranchPaymentGateway branchgateway = this.BRANCH_GATEWAY();

            string[] s = column_values.Split('^');

            if (s != null && s.Length > 0)
            {
                if (branchgateway.set_record(s) == false)
                {
                    this.set_message(branchgateway.messages); return false;
                }

                Session["IRS_BRANCH_GATEWAY"] = branchgateway;

                return true;
            }

            branchgateway = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_BRANCHGATEWAY_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_BRANCH_GATEWAY_RECORD(int client_seq_id)
        {
            irsBranchPaymentGateway branchgateway = this.BRANCH_GATEWAY();
            try
            {
                if (branchgateway != null)
                {
                    if (branchgateway.get_record(client_seq_id.ToString()) == true)
                    {
                        return branchgateway.getdataRecord();
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
                Session["IRS_BRANCH_GATEWAY"] = branchgateway;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string BRANCH_GATEWAY_SEQ_ID()
        {
            if (Session["BRANCH_GATEWAY_SEQ_ID"] != null)
            {
                return (string)Session["BRANCH_GATEWAY_SEQ_ID"];
            }

            return "0";
        }


        #endregion

        #region PRIVATE METHOD: BRANCH_GATEWAY

        private irsBranchPaymentGateway BRANCH_GATEWAY()
        {
            if (Session["IRS_BRANCH_GATEWAY"] == null)
            {
                Session["IRS_BRANCH_GATEWAY"] = new irsBranchPaymentGateway();
            }

            return (irsBranchPaymentGateway)Session["IRS_BRANCH_GATEWAY"];
        }


        private bool do_branch_gateway(string instruction)
        {
            irsBranchPaymentGateway cad = this.BRANCH_GATEWAY();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cad.selectedRow = -1;
                            if (cad.insert_record() == false)
                            {
                                this.set_message(cad.messages); return false;
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
                            cad.seq_id = this.BRANCH_GATEWAY_SEQ_ID();
                            cad.selectedRow = 0;

                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.messages); return false;
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
                            if (this.SET_BRANCHGATEWAY_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (cad.commit_record() == false)
                            {
                                this.set_message(cad.messages); return false;
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
                            cad.rollback_record(); return true;
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

                Session["IRS_BRANCH_GATEWAY"] = null;
                Session["IRS_BRANCH_GATEWAY"] = cad;
                Session["BRANCH_GATEWAY_SEQ_ID"] = null;
                Session["BRANCH_GATEWAY_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion


    }
}
