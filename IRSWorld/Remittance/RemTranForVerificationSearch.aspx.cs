using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using IRSWorld.TranClass;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.Remittance
{
    public partial class RemTranForVerificationSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranForVerificationSearch));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranCommons));

            if (Session["IRS_CLIENT_DATA"] == null)
            {
                Response.Redirect("ErrorPage.aspx");
            }
            Session["REMITTANCE_CALCULATOR_CLASS"] = null;
            Session["TranxID"] = Request.QueryString[""];
            if (Request.QueryString["sourcepage"] == null)
                Session["defaultparamvalues"] = null;
            
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRole()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString();
        }

        public string getUserid()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getlocaltime()
        {
            DateTime dtLocalTime = new irsTimeZone().branchLocalTime(getLocation());
            return Convert.ToString(dtLocalTime);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCode()
        {
            irsRemTeller clsOTC = new irsRemTeller();
            clsOTC.setLocation(Session["BranchCode"].ToString());
            return Session["BranchCode"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_previous_paramvalues()
        {
            if (Session["defaultparamvalues"] != null)
            {
                return (Session["defaultparamvalues"].ToString());
            }
            else
            {
                return get_param_beforeloading().ToString();
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_remittance_page(int product_charge_id, string query_parameters, string defaultvalues)
        {
            //SqlCommand sqcommands = null;
            //sqlHelperWs sql = new sqlHelperWs();
            try
            {

                Session["defaultparamvalues"] = defaultvalues;
                return "../Remittance/RemTranForVerification.aspx" + query_parameters;
                //}
                // return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                //if (sqcommands != null)
                //{
                //    sqcommands.Dispose();
                //    sqcommands = null;
                //}
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string process_batch_approval(string branchcode, string tranxdate, string numtrans, string otherchannels, string paymode, string productcode, string implemtype, string status, string userRole, string ordbatchno, string filetype, string fileformat)
        {
            irsRemTeller rem = new irsRemTeller();
            string paramMsg;
            try
            {
                paramMsg = rem.process_batch_approval(branchcode, tranxdate, numtrans, otherchannels, paymode, productcode, implemtype, status, userRole, ordbatchno, filetype, fileformat);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {}
            return paramMsg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string process_batch_approval_with_review(string refno, string branchcode, string transtatus)
        {
            irsRemTeller rem = new irsRemTeller();
            string paramMsg;
            try
            {
                paramMsg = rem.process_batch_approval(refno, branchcode, transtatus);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {}
            return paramMsg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable retrieve_batch_approval(string branchcode, string tranxdate, string numtrans, string otherchannels, string paymode, string productcode, string implemtype, string status, string userRole, string ordbatchno, string filetype, string fileformat)
        {
            irsRemTeller rem = new irsRemTeller();
            //string paramMsg; // commented to remove warning
            DataTable dt = null;
            try
            {
                dt = rem.retrieve_batch_approval(branchcode, tranxdate, numtrans, otherchannels, paymode, productcode, implemtype, status, userRole, ordbatchno, filetype, fileformat);
            }
            catch //(Exception e)  // commented to remove warning
            {
                return dt;
            }
            finally
            {}
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_PaymodeForApproval(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_RetrievePaymodeForApproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = remittingbranch;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "Paymode");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ProductCodeForApproval(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_RetrieveProductCodeForApproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = remittingbranch;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "Paymode");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ImplemTypeForApproval(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_RetrieveImplemTypeForApproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = remittingbranch;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "Paymode");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_RemitMethodForApproval(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_RetrieveRemitMethodForApproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = remittingbranch;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "RemitMethod");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_TranstatusForApproval(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_RetrieveTranxStatusForApproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = remittingbranch;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "RemitMethod");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ORDBatchNoForApproval(string branchcode)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_RetrieveORDBatchNoForApproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = branchcode;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "fileformat");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_BatchUpload_FileFormat(int fileformat)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_batchupload_fileformat");
                sqcommands.Parameters.Add("@fileformat", SqlDbType.Int).Value = fileformat;

                DataSet dt = sql.getDataSet(sqcommands, "fileformat");
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_BatchUpload_FileType()
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_batchupload_filetype");

                DataSet dt = sql.getDataSet(sqcommands, "filetype");
                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_BatchUpload_FileFormat_ForApproval(int fileformat, string branchcode)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_batchupload_fileformat_forapproval");
                sqcommands.Parameters.Add("@fileformat", SqlDbType.Int).Value = fileformat;
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = branchcode;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "fileformat");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_BatchUpload_FileType_ForApproval(string branchcode)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_batchupload_filetype_forapproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = branchcode;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();

                DataSet dt = sql.getDataSet(sqcommands, "filetype");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_Count_ForApproval(string remittingbranch)
        {
            SqlCommand sqcommands = null;
            sqlHelperWs sql = new sqlHelperWs();
            try
            {
                sqcommands = sql.getSqlCommand("sp_RetrieveCount_ForApproval");
                sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = remittingbranch;
                sqcommands.Parameters.Add("@userRole", SqlDbType.VarChar, 2).Value = getRole();
                sqcommands.Parameters.Add("@userID", SqlDbType.VarChar, 20).Value = getUserid();
                DataSet dt = sql.getDataSet(sqcommands, "RemitMethod");

                if (dt != null && dt.Tables[0].Rows.Count > 0)
                {
                    return dt;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

        private string getRoleCode(string role)
        {
            string roleCode = string.Empty;
            string sql = string.Empty;

            sqlHelperWs sqlws = new sqlHelperWs();
            DataSet dts = new DataSet();
            
            sql = "Select fldCode From tbl_Lookup where fldGroup = 'User Roles' and fldValue = " + role;
            dts = sqlws.getDataSet(sql, false, new Object[] { });

            roleCode = dts.Tables[0].Rows[0].ItemArray[0].ToString();

            return roleCode;
        }

        private string get_param_beforeloading()
        {
            string strArray = "" + "|" + getLocation() + "|" + String.Format("{0:MM/dd/yyyy}", getlocaltime()) + "|" + "" + "|" + "15" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + "" + "|" + getRole() + "|" + getUserid() + "|" + "|" + "|" + "|" + "For Verification" + "|" + getRoleCode(this.getRole());
            return strArray;
        }
    }
}
