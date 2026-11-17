using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld._3D
{
    public partial class OSA_Maintenance : System.Web.UI.Page
    {
        string UID;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OSA_Maintenance));
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_Courier_OSA()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select a.alias, UPPER(a.alias) as Courier from tbl_cif_master a " +
                                            "inner join tbl_cif_class b on a.cifid = b.cifid where b.cifclass = 9 " +
                                            "and isnull(a.alias, '') <> '' order by a.alias", false);
                DataSet ds = commons.getDataSet(cmd, "Courier");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_thresh_currency()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select flddescription,fldvalue  from tbl_lookup where fldgroup='Currency' and fldvalue in (1,2)", false);
                DataSet ds = commons.getDataSet(cmd, "Currency");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_zip()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select truezipcode,seq_id from tbl_zipcodes", false);
                DataSet ds = commons.getDataSet(cmd, "Zip");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet search_branch(int seqno)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = commons.getSqlCommand("sp_select_3dbranch");
            try
            {
                cmd.Parameters.Add("@v_seq_id", SqlDbType.BigInt).Value = seqno;
                return commons.getDataSet(cmd, "Display");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to return 3D Branch : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet search_osa(int seqno)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = commons.getSqlCommand("sp_select_osazip");
            try
            {
                cmd.Parameters.Add("@v_seq_id", SqlDbType.BigInt).Value = seqno;
                return commons.getDataSet(cmd, "Display");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to return OSA : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet show_zip_details(int seq_id)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select * from tbl_zipcodes where seq_id = " + seq_id, false);
                DataSet ds = commons.getDataSet(cmd, "ZipCode");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet display_threshold(int seqno)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = commons.getSqlCommand("sp_select_threshold");
            try
            {
                cmd.Parameters.Add("@v_seq_id", SqlDbType.BigInt).Value = seqno;
                return commons.getDataSet(cmd, "Display");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to return 3D Branch : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Update_OSA_Table(int zip_seq_id, string osa_ind, string courier)
        {
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)Session["IRS_CLIENT_DATA"];
                this.UID = loggin_data.UserId.ToString();
            }

            try
            {

                Object[] plist =
                        {
                            zip_seq_id,
                            osa_ind,
                            courier,
                            this.UID,
                            String.Empty
                        };

                irs3DClass ord3d = new irs3DClass();

                if (ord3d.execProcedureUpdate("sp_update_osa", plist))
                {
                    return "0~";
                }
                else
                {
                    return "1~" + ord3d.message;
                }

                return "0~";
            }
            catch
            {
                return "1~" + "Error Saving Data.";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string Update_Thresh_Table(string courier, string currency, string acct_num, string amount)
        {
            if (Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)Session["IRS_CLIENT_DATA"];
                this.UID = loggin_data.UserId.ToString();
            }

            try
            {

                Object[] plist =
                        {
                            courier,
                            currency,
                            acct_num,
                            amount,
                            this.UID,
                            String.Empty
                        };

                irs3DClass ord3d = new irs3DClass();

                if (ord3d.execProcedureUpdate("sp_update_treshhold", plist))
                {
                    return "0~";
                }
                else
                {
                    return "1~" + ord3d.message;
                }

                return "0~";
            }
            catch (Exception ex)
            {
                return "1~" + "Error Saving Data.";
            }
        }
    }
}
