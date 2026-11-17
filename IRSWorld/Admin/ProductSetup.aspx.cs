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

namespace IRSWorld.Admin
{
    public partial class ProductSetup : System.Web.UI.Page
    {
        //string PC = null;
        //string IT = null;
        //string CN = null;
        //string CT = null;
        //string FL = null;
        //string S  = null;
        //string SQ = null;
 
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(ProductSetup));
        }
        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void insert(string strTable)
        {
            ////string[] tables = new string[];
            //tables = strTable.Split('^');
            
            ////string[] columns = new string[];
            
            //for (int i = 0; i <= tables.GetUpperBound(0); i++)
            //{

            //}
            
        }
       
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["PRODUCT_MESSAGE"] != null)
            {
                return (string)Session["PRODUCT_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["PRODUCT_MESSAGE"] = null;
            Session["PRODUCT_MESSAGE"] = msg;
        }
              
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string PRODUCT_SETUP_SEQ_ID()
        {
            if (Session["PRODUCT_SETUP_SEQ_ID"] != null)
            {
                return (string)Session["PRODUCT_SETUP_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_productsetup");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "product_setup");
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
        public DataSet getdropdown_source2(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_productsetup2");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "product_setup");
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
        public String[] parse_sql(string sql)
        {
            Session["PRODUCT_MESSAGE"] = null;

            try
            {
                irsSqlCommons com = new irsSqlCommons();
                DataSet ds = com.getDataSet(com.getSqlCommand(sql, false), "QUERY_RESULT");

                if (ds != null)
                {
                    String[] ss = new String[ds.Tables.Count];
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        ss[i] = ds.Tables[i].TableName;
                    }
                    DATASET = ds;
                    return ss;
                }

                throw new Exception(com.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_tables()
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_get_tables");
                    return commons.getDataSet(sqcommands, "table_names");
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
        public String getResult(String tbname)
        {
            String cap = "<caption>";
            try
            {
                DataTable dt = DATASET.Tables[tbname];
                String s = convert_dt(dt);
                if (s.IndexOf(cap) > -1)
                {
                    s = s.Substring(0, s.IndexOf(cap)) + "<caption class=\"rscaption\">" + s.Substring(s.LastIndexOf(cap) + cap.Length);
                }
                return s;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_columns(string table)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_get_columns");
                    sqcommands.Parameters.Add("@TableName", SqlDbType.VarChar, 100).Value = table;

                    return commons.getDataSet(sqcommands, "column_names");
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
        #region Private Methods
        private string parameter_values = string.Empty;
        private String DS = "IRS_PRODUCTSETUP_DS";
        private DataSet DATASET
        {
            get
            {
                if (Session[DS] == null) return null;
                return (DataSet)Session[DS];
            }
            set
            {
                Session[DS] = value;
            }
        }
        irsProductSetup PRODUCT_SETUP()
        {
            if (Session["IRS_PRODUCT_SETUP"] == null)
            {
                Session["IRS_PRODUCT_SETUP"] = new irsProductSetup();
            }

            return (irsProductSetup)Session["IRS_PRODUCT_SETUP"];
        }
        //private bool do_product_setup(string instruction)
        //{
        //    irsProductSetup ps = this.PRODUCT_SETUP();

        //    try
        //    {
        //        if (instruction != null && instruction.Trim().Length > 0)
        //        {
        //            if (instruction == "INSERT")
        //            {
        //                try
        //                {
        //                    ps.selectedRow = -1;
        //                    if (ps.insert_record() == false)
        //                    {
        //                        this.set_message(ps.messages); return false;
        //                    }
        //                    return true;
        //                }
        //                catch (Exception e)
        //                {
        //                    this.set_message(e.Message); return false;
        //                }
        //            }

        //            if (instruction == "UPDATE")
        //            {
        //                try
        //                {
        //                    ps.seq_id = this.PRODUCT_SETUP_SEQ_ID();
        //                    ps.selectedRow = 0;

        //                    if (ps.update_record() == false)
        //                    {
        //                        this.set_message(ps.messages); return false;
        //                    }

        //                    return true;

        //                }
        //                catch (Exception e)
        //                {
        //                    this.set_message(e.Message); return false;
        //                }
        //            }

        //            if (instruction == "COMMIT")
        //            {
        //                try
        //                {
        //                    if (this.SET_PRODUCTSETUP_RECORD(this.parameter_values) == false)
        //                    {
        //                        return false;
        //                    }

        //                    if (ps.commit_record() == false)
        //                    {
        //                        this.set_message(ps.messages); return false;
        //                    }

        //                    return true;

        //                }
        //                catch (Exception e)
        //                {
        //                    this.set_message(e.Message); return false;
        //                }
        //            }

        //            if (instruction == "ROLLBACK")
        //            {
        //                try
        //                {
        //                    ps.rollback_record(); return true;
        //                }
        //                catch (Exception e)
        //                {
        //                    this.set_message(e.Message); return false;
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    finally
        //    {

        //        Session["IRS_PRODUCT_SETUP"] = null;
        //        Session["IRS_PRODUCT_SETUP"] = ps;
        //        Session["PRODUCT_SETUP_SEQ_ID"] = null;
        //        Session["PRODUCT_SETUP_SEQ_ID"] = ps.seq_id;
        //        ps = null;
        //    }

        //    this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        //}


        private String convert_dt(DataTable dt)
        {
            if (dt == null) return String.Empty;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Table table = create_table(dt.TableName);

                    TableHeaderRow hrow = create_thead(dt.Columns);
                    table.Rows.Add(hrow);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        TableRow arow = create_datarow(dt.Rows[i], dt.Columns);
                        table.Rows.Add(arow);
                    }
                    return render_control(table);
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private TableRow create_datarow(DataRow arow, DataColumnCollection col)
        {
            TableRow trow = new TableRow();
            for (int i = 0; i < col.Count; i++)
            {
                TableCell tcel = new TableCell();
                tcel.Text = getText(arow[i], col[i].DataType);
                String align = (tcel.Text.Equals("NULL")) ? "center" : getCellAlign(col[i].DataType);
                tcel.Attributes.Add("align", align);
                tcel.CssClass = "rscell";
                trow.Cells.Add(tcel);
            }
            return trow;
        }

        private TableHeaderRow create_thead(DataColumnCollection dcol)
        {
            TableHeaderRow hrow = new TableHeaderRow();
            for (int i = 0; i < dcol.Count; i++)
            {
                TableCell cell = new TableCell();
                cell.Attributes.Add("align", "center");
                cell.BackColor = System.Drawing.Color.Lavender;
                cell.CssClass = "rscell";
                String colname = dcol[i].ColumnName;
                cell.Text = (colname == null || colname == String.Empty) ? "Col. " + ((int)(i + 1)).ToString() : colname;
                hrow.Cells.Add(cell);
            }

            return hrow;
        }

        private String getText(Object value, Type col_type)
        {
            if (value == null || value == DBNull.Value)
            {
                return "NULL";
            }

            switch (col_type.ToString())
            {
                case "System.Boolean":
                    return ((bool)value) ? "TRUE" : "FALSE";
                case "System.Byte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    return Convert.ToInt64(value).ToString().Trim();
                case "System.Decimal":
                    return ((decimal)value).ToString("###,###,###,###,##0.00").Trim();
                case "System.Single":
                case "System.Double":
                    return Convert.ToDouble(value).ToString().Trim();
                case "System.DateTime":
                    return ((DateTime)value).ToString("MM/dd/yyyy hh:mm:ss tt");
                default:
                    return value.ToString().Trim();
            }
        }

        private String getCellAlign(Type type)
        {
            switch (type.ToString())
            {
                case "System.Boolean":
                case "System.DateTime":
                    return "center";
                case "System.Byte":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                    return "right";

            }
            return "left";
        }

        private String render_control(WebControl control)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            control.RenderControl(htw);
            String rs = sb.ToString();
            htw.Close();

            return rs;
        }

        private Table create_table(String id)
        {
            Table table = new Table();
            table.ID = id;
            table.Caption = id;
            table.CssClass = "rstable";
            table.CellSpacing = 0;
            table.CellPadding = 3;

            return table;
        }

        #endregion
    }
}
