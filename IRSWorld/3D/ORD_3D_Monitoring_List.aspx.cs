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
    public class ORDRemittance_update : sqlHelperWs
    {
        public ORDRemittance_update()
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
        public string update_remittance_ORD3D(string currency, string status, string courier, string amount, string column_values, int sequence_id)
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

            if (status == "257")
            {
                insert_3D_credits_ORD3D("2372", courier, int.Parse(currency),decimal.Parse(amount)); // Credit
            }

            return "0~";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string sendall(string courier, string currency, string status)
        {
            Object[] plist =
                    {
                        courier,
                        status,
                        int.Parse(currency),
                        String.Empty
                    };

            irs3DClass ord3d = new irs3DClass();

            if (ord3d.execProcedureUpdate("sp_execute_batch_send", plist))
            {
                return "0~";
            }
            else
            {
                return "1~" + ord3d.message;
            }

            //return "0~";
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string insert_3D_credits_ORD3D(string apbcode, string courier, int currency, decimal amount)
        {
            Object[] plist =
                    {
                        courier,
                        currency,
                        amount,
                        apbcode,
                        String.Empty
                    };

            irs3DClass ord3d = new irs3DClass();

            if (ord3d.execProcedureUpdate("sp_insert_credit_courier", plist))
            {
                return "0~";
            }
            else
            {
                return  "1~" + ord3d.message;
            }

            //return "0~";
        }

    }

    [Serializable()]
    public class ORDCourier_record_update : sqlHelperWs
    {

        public ORDCourier_record_update()
        {
            base.EnforceConstraint = false;
        }

        public override void getUpdateDS(int id)
        {
            this.TableName = "TBL_REMITTANCE_3D_DETAILS";
            this.IdentityColumn = "COU_SEQ_ID";
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMITTANCE_3D_DETAILS WHERE TXN_SEQ_ID = " + id, false); base.getUpdateDataSet(command);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_record_ORD3D(string status, string column_values, int sequence_id)
        {
            ArrayList error_list = new ArrayList();

            System.Web.HttpContext.Current.Session["3D_ERROR_MSG"] = null;

            base.IdentityValue = sequence_id;
            base.selectedRow = 0;


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

    public partial class ORD_3D_Monitoring_List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ORD_3D_Monitoring_List));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ORDCourier_record_update));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ORDRemittance_update));

            //if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
            //{
            //    irsClientData loggin_data = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]; return loggin_data.Location + "~" + new irsTimeZone().branchLocalTime(loggin_data.Location).ToString("MM/dd/yyyy");
            //}
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable load_3d_details_courier(string tranxstat, string courier, int curr, string datefrom, string dateto)
        {
            sqlHelperWs commons = new sqlHelperWs();
            DataSet ds = commons.execStoredProcedure("sp_get_3D_Details", new object[] { tranxstat, courier, curr, datefrom, dateto });
            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_currency_ORD3D()
        {
            sqlHelperWs commons = new sqlHelperWs();
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
            //return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_3D_Status_ORD3D()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select UPPER(fldcode + ' - ' + substring(flddescription, 6, len(flddescription) - 5)) [flddescription],fldvalue  from tbl_lookup where fldgroup='TranxStatus' and fldcode like '3D%'", false);
                DataSet ds = commons.getDataSet(cmd, "TranxStatus");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            //return null;
        }

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public DataSet load_Reasons_ORD3D()
        //{
        //    sqlHelperWs commons = new sqlHelperWs();
        //    SqlCommand cmd = new SqlCommand();

        //    try
        //    {
        //        cmd = commons.getSqlCommand("select UPPER(convert(varchar,fldvalue) + ' - ' + convert(varchar,flddescription)) [flddescription],fldvalue  from tbl_lookup where fldgroup='3DReason' order by fldvalue", false);
        //        DataSet ds = commons.getDataSet(cmd, "CourierReasons");
        //        return ds;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    return null;
        //}

        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public DataSet load_Courier_3D_Status_ORD3D()
        //{
        //    sqlHelperWs commons = new sqlHelperWs();
        //    SqlCommand cmd = new SqlCommand();

        //    try
        //    {
        //        cmd = commons.getSqlCommand("select UPPER(fldcode + ' - ' + substring(flddescription, 6, len(flddescription) - 5)) [flddescription],fldvalue  from tbl_lookup where fldgroup='TranxStatus' and fldcode like '3D%' and fldvalue in ('261','262','263','266','267','268')", false);
        //        DataSet ds = commons.getDataSet(cmd, "TranxStatus");
        //        return ds;
        //    }
        //    catch (SqlException ex)
        //    {
        //        throw ex;
        //    }
        //    return null;
        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_Courier_ORD3D()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = new SqlCommand();

            try
            {
                cmd = commons.getSqlCommand("select a.alias, UPPER(a.alias + ' - ' + a.lastname) as Courier from tbl_cif_master a " +
                                            "inner join tbl_cif_class b on a.cifid = b.cifid where b.cifclass = 9 " +
                                            "and isnull(a.alias, '') <> '' order by alias", false);
                DataSet ds = commons.getDataSet(cmd, "Courier");
                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            //return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_details_ORD3D(int seqno)
        {

            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand cmd = commons.getSqlCommand("sp_display_details");
            try
            {
                cmd.Parameters.Add("@v_Seqno", SqlDbType.BigInt).Value = seqno;
                return commons.getDataSet(cmd, "Display");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to return 3D Details : " + ex.Message);
            }
        }


        

    }
}
