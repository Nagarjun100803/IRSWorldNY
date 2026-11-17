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
    [Serializable()]
    public class remittance_update : sqlHelper
    {

        public remittance_update()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int seq_id)
        {
            this.TableName = "TBL_REMITTANCE";
            this.IdentityColumn = "SEQ_ID";
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE WHERE seq_id = " + seq_id, false); base.getUpdateDataSet(command);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_remittance(string currency, string status, string courier, string amount, string column_values, int sequence_id)
        {
            ArrayList error_list = new ArrayList();

            System.Web.HttpContext.Current.Session["3D_ERROR_MSG"] = null;

            base.IdentityValue = sequence_id;
            base.selectedRow = 0;

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                string[] s = column_values.Split('~');

                if (!base.setColumn(s[0].Trim(), s[1]))
                {
                    error_list.Add(s[0] + ":" + this.Message);
                }

                if (error_list.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (Object errors in error_list)
                    {
                        sb.Append((String)errors + "\n");
                    }

                    return "1~" + sb.ToString();

                }
                else
                {
                    if (base.save() == false)
                    {
                        if (base.Message != null && base.Message.Trim().Length > 0)
                        {
                            return "1~" + base.Message;
                        }
                    }
                }
            }

            if (status == "263")
            {
                insert_3D_credits("2572", courier, int.Parse(currency), decimal.Parse(amount)); // Return to ORD
            }

            return "0~";
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_3D_credits(string APBCode, string courier, int currency, decimal amount)
        {
            Object[] plist =
                    {
                        courier,
                        currency,
                        amount,
                        APBCode,
                        String.Empty
                    };

            irs3DClass ord3d = new irs3DClass();

            if (ord3d.execProcedureUpdate("sp_insert_credit_courier", plist))
            {
                return "0~";
            }
            else
            {
                return "1~" + ord3d.message;
            }

            return "0~";
        }

    }

    [Serializable()]
    public class courier_record_update : sqlHelper
    {
        
        public courier_record_update()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName      = "TBL_REMITTANCE_3D_DETAILS";
            this.IdentityColumn = "COU_SEQ_ID";
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_3D_DETAILS WHERE TXN_SEQ_ID = " + id, false); base.getUpdateDataSet(command);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_record(string status, string column_values, int sequence_id)
        {
            ArrayList error_list = new ArrayList();

            System.Web.HttpContext.Current.Session["3D_ERROR_MSG"] = null;
            
            base.IdentityValue = sequence_id;
            base.selectedRow   = 0;
            

            string[] columns = column_values.Split('|');

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    string[] s = columns[i].Split('~');

                    if (s != null && s.Length > 0)
                    {
                        if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                        {
                            s[1] = null;
                        }
                        if (!base.setColumn(s[0].Trim(), s[1]))
                        {
                            error_list.Add(s[0] + ":" + this.Message);
                        }
                    }
                }

                if (error_list.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();

                    foreach (Object errors in error_list)
                    {
                        sb.Append((String)errors + "\n");
                    }

                    return "1~" + sb.ToString(); 
                    
                }
                else
                {
                    if (base.save() == false)
                    {
                        if (base.Message != null && base.Message.Trim().Length > 0)
                        {
                            return "1~" + base.Message;
                        }
                    }
                }
            }

            return "0~";
        }
    }

    public partial class Courier_3D_List : System.Web.UI.Page
    {

        string UID;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Courier_3D_List));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(courier_record_update));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(remittance_update));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_currency()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select UPPER(flddescription) as flddescription,fldvalue  from tbl_lookup where fldgroup='Currency' and fldvalue in (1,2)", false);
                DataSet ds = commons.getDataSet(cmd, "Currency");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_3D_Status()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select UPPER(fldcode + ' - ' + substring(flddescription, 6, len(flddescription) - 5)) [flddescription],fldvalue  from tbl_lookup where fldgroup='TranxStatus' and fldcode like '3D%' and fldvalue not in (258, 264, 266, 271, 260, 265, 270)", false);
                DataSet ds = commons.getDataSet(cmd, "TranxStatus");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_Reasons()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select UPPER(convert(varchar,fldvalue) + ' - ' + convert(varchar,flddescription)) [flddescription],fldvalue  from tbl_lookup where fldgroup='3DReason' order by fldvalue", false);
                DataSet ds = commons.getDataSet(cmd, "CourierReasons");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_Courier_3D_Status()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select UPPER(fldcode + ' - ' + substring(flddescription, 6, len(flddescription) - 5)) [flddescription],fldvalue  from tbl_lookup where fldgroup='TranxStatus' and fldcode like '3D%' and fldvalue in ('261','262','263','267','268')", false);
                DataSet ds = commons.getDataSet(cmd, "TranxStatus");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable load_courier_3d_details(string tranxstat, string courier, int curr, string datefrom, string dateto)
        {
            sqlHelper commons = new sqlHelper();
            DataSet ds = commons.execStoredProcedure("sp_get_3D_Details", new object[] { tranxstat, courier, curr, datefrom, dateto });
            return ds.Tables[0];

            //sqlHelper commons = new sqlHelper();
            //SqlCommand cmd = commons.getSqlCommand("sp_get_3D_Details");
            //try
            //{
            //    cmd.Parameters.Add("@v_Seqno", SqlDbType.BigInt).Value = seqno;
            //    return commons.getDataSet(cmd, "Display");
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("Unable to return 3D Details : " + ex.Message);
            //}
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_courier_value()
        {

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                this.UID = loggin_data.UserId.ToString();
            }

            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd = commons.getSqlCommand("select UPPER(a.alias) as CourierVal from tbl_cif_master a " +
                                            "inner join tbl_cif_class b on a.cifid = b.cifid where b.cifclass = 9 " +
                                            "and isnull(a.alias, '') <> '' and a.alias = (select alias " +
                                            "from tbl_users users inner join tbl_CIF_master cif on users.CIFID = cif.CIFID " +
                                            "where userid = '" + this.UID.Trim().ToString() + "')", false);
                DataSet ds = commons.getDataSet(cmd, "CourierVal");
                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_Courier()
        {

            if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            {
                irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                this.UID = loggin_data.UserId.ToString();
            }

            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select UPPER(a.lastname) as Courier from tbl_cif_master a " +
                                            "inner join tbl_cif_class b on a.cifid = b.cifid where b.cifclass = 9 " +
                                            "and isnull(a.alias, '') <> '' and a.alias = (select alias " +
			                                "from tbl_users users inner join tbl_CIF_master cif on users.CIFID = cif.CIFID " +
			                                "where userid = '" + this.UID.Trim().ToString() + "')", false);
                DataSet ds = commons.getDataSet(cmd, "Courier");
                return ds.Tables[0].Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_details(int seqno)
        {
            
            sqlHelper commons = new sqlHelper();
            SqlCommand cmd = commons.getSqlCommand("sp_display_details");
            try
            {
                cmd.Parameters.Add("@v_Seqno", SqlDbType.BigInt).Value = seqno;
                return commons.getDataSet(cmd,"Display");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to return 3D Details : " + ex.Message);
            }
        }

    }
}
