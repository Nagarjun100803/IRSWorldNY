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
using System.Text;
using System.IO;
using IRSClassLib;

namespace IRSWorld
{
    public partial class ajaxgrid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.do_grid_customization();

            if (!IsPostBack)
            {
                this.grd_ajx_command();
            }
        }

        private void do_grid_customization()
        {
            int page_size = 10;

            if (Request.Form["ajx_page_size"] != null)
            {
                if (Convert.ToInt32(Request.Form["ajx_page_size"].ToString()) > 0)
                {
                    page_size = Convert.ToInt32(Request.Form["ajx_page_size"].ToString());
                }
            }

            this.grd_ajax.Style.Add("width", "auto");
            this.grd_ajax.AllowPaging            = true;
            this.grd_ajax.PageSize               = page_size;
            this.grd_ajax.RowStyle.Font.Size     = FontUnit.Point(8);
            this.grd_ajax.HeaderStyle.Font.Size  = FontUnit.Point(8);
            this.grd_ajax.PagerStyle.Font.Size   = FontUnit.Point(8);
            this.grd_ajax.PagerStyle.BackColor   = System.Drawing.Color.FromName("#99CCCC");
            this.grd_ajax.RowStyle.Font.Names    = new string[] { "Arial", "Tahoma", "Verdana" };
            this.grd_ajax.HeaderStyle.Font.Names = new string[] { "Arial", "Tahoma", "Verdana" };
            this.grd_ajax.PagerStyle.Font.Names  = new string[] { "Arial", "Tahoma", "Verdana" };
            this.grd_ajax.BorderWidth            = Unit.Pixel(1);
            this.grd_ajax.BorderColor            = System.Drawing.Color.Black;
            this.grd_ajax.HeaderStyle.BackColor  = System.Drawing.Color.FromName("#003399");
            this.grd_ajax.HeaderStyle.ForeColor  = System.Drawing.Color.FromName("#C0C0FF");
        }

        private void grd_ajx_command()
        {
            string ajx_command_name  = string.Empty;
            string ajx_command_value = string.Empty;
            string ajx_grid_source   = string.Empty;

            if (Request.Form["ajx_command_name"]  != null) { ajx_command_name = Request.Form["ajx_command_name"].ToString(); }
            if (Request.Form["ajx_command_value"] != null) { ajx_command_value = Request.Form["ajx_command_value"].ToString(); }

            if (ajx_command_name == null || ajx_command_name.Length < 1)
            {
                this.session_cleanup();
            }

            if (Request.Form["ajx_grid_source"] != null)
            {
                ajx_grid_source = Request.Form["ajx_grid_source"].ToString().Replace("DVD14344DVD", "&"); Session["GRID_SQL_SOURCE"] = ajx_grid_source;
            }

            if (Request.Form["ajx_datarow_item"] != null)
            {
                Session["DATA_ROW_ITEM"] = Request.Form["ajx_datarow_item"].ToString();
            }

            if (ajx_command_name != null)
            {
                if (ajx_command_name.Length > 0)
                {
                    if (ajx_command_name != string.Empty)
                    {
                        this.execute_ajaxgridview_command(ajx_command_name, ajx_command_value, ajx_grid_source); return;
                    }
                }
            }
            this.grd_loading();
        }

        private void execute_ajaxgridview_command(string command_name, string command_value, string ajx_grid_source)
        {
            switch (command_name)
            {
                case "ajx_loading": this.grd_loading(); break;
                case "ajx_paging" : this.grd_paging(command_value); break;
                case "ajx_sorting": this.grd_sorting(command_value); break;
                case "ajx_search" : this.grd_search(command_value); break;
            }
        }

        private void grd_loading()
        {
            DataTable dt = this.grd_data_source();

            if (dt == null || dt.Rows.Count == 0)
            {
                Response.Write("<P></P><P></P><P></P><CENTER><B style='font-family:Arial;font-size:8pt'>NO RECORD FOUND.</B></CENTER>");
                Response.End();
            }

            this.grd_ajax.DataSource = dt;
            this.grd_ajax.DataBind();
            this.grd_ajax_render(grd_ajax);
        }

        private void grd_search(string s)
        {
            DataView dview = null;
            int page_index = 0;

            dview = this.grd_data_source().DefaultView;

            if (dview != null && dview.Table.Rows.Count > 0)
            {
                if (Session["CURRENT_PAGE_INDEX"] != null)
                {
                    page_index = (int)Session["CURRENT_PAGE_INDEX"];
                }

                s = s.Replace("'", "''");
                s = s.Replace("%", "");
                s = s.Replace("[", "");
                s = s.Replace("]", "");
                s = s.Replace("|", "");

                StringBuilder sb = new StringBuilder();

                foreach (DataColumn cols in dview.Table.Columns)
                {
                    if (cols.DataType.ToString().ToUpper() == "SYSTEM.STRING")
                    {
                        sb.Append("[" + cols.ColumnName + "] LIKE '%" + s + "%'" + " OR ");
                    }
                }

                dview.RowFilter = string.Empty;
                dview.RowFilter = sb.ToString().Substring(0, sb.ToString().Length - 3);

                Session["CURRENT_SEARCH_CRITERIA"] = dview.RowFilter;

                if (dview.Count == 0)
                {
                    Response.Write("<P></P><P></P><P></P><CENTER><B style='font-family:Arial;font-size:8pt'>NO RECORD FOUND.</B></CENTER>");
                    Response.End();
                }

                this.grd_ajax.DataSource = dview;
                this.grd_ajax.AllowPaging = true;
                this.grd_ajax.PageIndex = page_index;
                this.grd_ajax.DataBind();
                this.grd_ajax_render(grd_ajax);

            }
        }

        private void session_cleanup()
        {
            Session["CURRENT_SORT_ORDER"]      = null;
            Session["CURRENT_PAGE_INDEX"]      = null;
            Session["CURRENT_SEARCH_CRITERIA"] = null;
            Session["GRID_SQL_SOURCE"]         = null;
            Session["DATA_SOURCE_DB"]          = null;
        }

        private void grd_paging(string paging_param)
        {
            
            Session["CURRENT_PAGE_INDEX"] = Convert.ToInt32(paging_param);

            string sort_order = string.Empty;
            string search_criteria = string.Empty;

            if (Session["CURRENT_SORT_ORDER"]      != null) { sort_order      = (string)Session["CURRENT_SORT_ORDER"]; }
            if (Session["CURRENT_SEARCH_CRITERIA"] != null) { search_criteria = (string)Session["CURRENT_SEARCH_CRITERIA"]; }

            DataView dview = this.grd_data_source().DefaultView;

            if (dview != null && dview.Count > 0)
            {
                dview.RowFilter = search_criteria;
                dview.Sort = sort_order;

                this.grd_ajax.DataSource = dview;
                this.grd_ajax.PageIndex = Convert.ToInt32(paging_param);
                this.grd_ajax.DataBind();
                this.grd_ajax_render(grd_ajax);
            }

            dview.Dispose();
            dview = null;
        }

        private void grd_sorting(string sort_param)
        {
            Session["CURRENT_SORT_ORDER"] = sort_param;

            int page_index = 0;

            DataView dview = this.grd_data_source().DefaultView;

            if (Session["CURRENT_PAGE_INDEX"] != null)
            {
                page_index = (int)Session["CURRENT_PAGE_INDEX"];
            }

            if (dview != null && dview.Table.Rows.Count > 0)
            {
                dview.Sort = sort_param;

                this.grd_ajax.DataSource = dview;
                this.grd_ajax.PageIndex = page_index;
                this.grd_ajax.DataBind();
                this.grd_ajax_render(grd_ajax);
            }
        }

        public DataTable grd_data_source()
        {
            DataTable dt = (DataTable)Session["DATA_SOURCE_DB"];
            try
            {
                if (dt == null)
                {
                    string s = Session["GRID_SQL_SOURCE"].ToString();

                    if (s != null && s.Length > 0)
                    {
                        irsSqlCommonsWs SQL = new irsSqlCommonsWs();

                        if (SQL != null)
                        {
                            SQL.EnforceConstraint = false;

                            if (s.IndexOf("SELECT") == -1)
                            {
                                string[] param = s.Split('~'); // <--- spname~paramvalue1~paramvalue2~parametc
                                string sp_name = param[0];
                                string sp_value = null;
                                object[] array_value = null;

                                if (param.Length - 1 != 1)
                                {
                                    /* why not transfer param content directly
                                     * because the loop is adding extra tilde at the end 
                                    for (int i = 0; i < param.Length - 1; i++)
                                    {
                                        sp_value += param[i + 1] + "~";
                                    } 

                                    array_value = sp_value.Split('~');
                                     * */
                                    array_value = new Object[param.Length - 1];
                                    for (int i = 1; i < param.Length; i++)
                                    {
                                        array_value[i - 1] = param[i];
                                    }
                                }
                                else
                                {
                                    array_value = param[1].Split('~');
                                }

                                dt = SQL.execStoredProcedure(sp_name, array_value).Tables[0];
                                Session["DATA_SOURCE_DB"] = dt; 
                                return dt;
                            }

                            //dt = SQL.getDataSet(SQL.getSqlCommand(s, false), "SQL_DATASET").Tables[0];
                            dt = SQL.getDataSet(s, false, new Object[] { }).Tables[0];

                            Session["DATA_SOURCE_DB"] = dt; return dt;
                        }
                    }

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }

                Response.Write("<P></P><P></P><P></P><CENTER><B style='font-family:Arial;font-size:8pt'>NO RECORD FOUND.</B></CENTER>");
                Response.End();

            }
            catch
            {
                Response.Write("<P></P><P></P><P></P><CENTER><B style='font-family:Arial;font-size:8pt'>UNABLE TO CONNECT TO DATABASE SERVER.</B></CENTER>");
                Response.End();
            }
            return null;
        }



        private void grd_ajax_render(GridView grid)
        {
            string s = string.Empty;

            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            grid.RenderControl(hw); s = sw.ToString(); this.form1.Controls.Remove(grd_ajax);

            sw.Close(); sw = null;
            hw.Close(); hw = null;

            Response.Write(s);
            Response.End();
        }

        protected void grd_ajax_rowdata_bound(object sender, GridViewRowEventArgs e)
        {

            this.grd_ajax.Caption = "<b><font style = 'font-family:Arial; font-size:8pt; font-weight:bold'> " + this.construct_total_rowcount() + "</font></b>";
            
            // Changes to provide multiple return value to users of the grid. To allow Session["DATA_ROW_ITEM"] to contain
            // multiple entries, separated by comma. The changes is needed by EPSCO and CIF.
            int irow_item = 0;
            int[] row_items = null; 

            if (Session["DATA_ROW_ITEM"] != null)
            {
                String[] ss = Session["DATA_ROW_ITEM"].ToString().Split(',');
                if (ss.Length > 0)
                {
                    irow_item = Convert.ToInt32(ss[0].Trim());
                    row_items = new int[ss.Length];
                    for (int i = 0; i < ss.Length; i++)
                    {
                        row_items[i] = Convert.ToInt32(ss[i].Trim());
                    }
                }
                else
                {
                    row_items = new int[1];
                    row_items[0] = 0;
                }
            }
            // End of changes.

            if (e.Row.RowType == DataControlRowType.Header)
            {
                // See above.
                //int irow_item = 0;

                //if (Session["DATA_ROW_ITEM"] != null)
                //{
                //    irow_item = Convert.ToInt32(Session["DATA_ROW_ITEM"].ToString());
                //}

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (irow_item != i)
                    {
                        e.Row.Cells[i].Visible = true;                       

                        string sort_flag = "Asc";

                        if (Session["CURRENT_SORT_ORDER"] != null)
                        {
                            string sort_param = (string)Session["CURRENT_SORT_ORDER"];

                            if (sort_param.Substring(sort_param.Length - 4).Trim().ToUpper() == "ASC")
                            {
                                sort_flag = "Desc";
                            }
                        }

                        string link_string = "javascript:execute_ajx_command('ajx_sorting$" + e.Row.Cells[i].Text + " " + sort_flag + "')";
                        
                        e.Row.Cells[i].Text = e.Row.Cells[i].Text.ToUpper();
                        e.Row.Cells[i].Attributes.Add("onclick", link_string);
                        e.Row.Cells[i].Attributes.Add("onmouseover", "this.style.cursor = 'pointer'");
                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;                       
                        e.Row.Cells[i].BorderColor = System.Drawing.Color.Black;
                    }
                    else
                    {
                        e.Row.Cells[i].Visible = false;
                    }
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string row_item_value = string.Empty;

                    // See above.
                    //int row_item = 0; 
                    //if (Session["DATA_ROW_ITEM"] != null)
                    //{
                    //    row_item = Convert.ToInt32(Session["DATA_ROW_ITEM"].ToString());
                    //}

                    //row_item_value = e.Row.Cells[row_item].Text;
                    if (row_items != null)
                    {
                        for (int j = 0; j < row_items.Length; j++)
                        {
                            row_item_value += "$" + e.Row.Cells[row_items[j]].Text;
                        }
                    }
                    else
                    {
                        row_item_value += "$" + e.Row.Cells[0].Text;
                    }

                    e.Row.Cells[irow_item].Visible = false;                   
                    //e.Row.Attributes.Add("onclick", "javascript:execute_ajx_command('ajx_selected$" + row_item_value.Replace("'","") + "');");
                    e.Row.Attributes.Add("onclick", "javascript:execute_ajx_command('ajx_selected" + row_item_value.Replace("'", "") + "');");
                    e.Row.Attributes.Add("onmouseout",  "this.style.backgroundColor = this.originalbackcolor");
                    e.Row.Attributes.Add("onmouseover", "this.style.cursor = 'pointer'; this.originalbackcolor = this.style.backgroundColor;this.style.backgroundColor = 'lightblue'");                    
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;                    
                    e.Row.Cells[i].BorderColor     = System.Drawing.Color.Black;

                    decimal decimal_format = 0;
                    int integer_format     = 0;
                    DateTime date_format;

                    if (irow_item != i)
                    {
                        if (decimal.TryParse(e.Row.Cells[i].Text, out decimal_format) == true)
                        {
                            if (e.Row.Cells[i].Text.IndexOf(".", 0) > 0)
                            {
                                e.Row.Cells[i].Text = decimal_format.ToString("#,###.00");
                                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                        else if (int.TryParse(e.Row.Cells[i].Text, out integer_format) == true)
                        {
                            if (e.Row.Cells[i].Text.IndexOf(".", 0) > 0)
                            {
                                e.Row.Cells[i].Text = integer_format.ToString("#,###");
                                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                        else if (DateTime.TryParse(e.Row.Cells[i].Text, out date_format) == true)
                        {
                            string grd_customed_date = Request.Form["grd_customed_date"];

                            if (grd_customed_date != null && grd_customed_date.Length > 0 && grd_customed_date.Trim().ToUpper().Equals("FALSE"))
                            {
                                e.Row.Cells[i].Text = date_format.ToString("MM/dd/yyyy hh:mm:ss");
                            }
                            else
                            {
                                e.Row.Cells[i].Text = date_format.ToString("dd/MM/yyyy");
                            }

                            e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                        }
                    }
                }
            }

            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridView grd = (GridView)sender;

                int page_count = grd.PageCount;
                int page_index = 0;

                e.Row.Cells[0].Text = "";

                if (Session["CURRENT_PAGE_INDEX"] != null)
                {
                    page_index = (int)Session["CURRENT_PAGE_INDEX"];
                }

                DropDownList droplist = new DropDownList();                   
                droplist.Attributes.Add("onchange", "javascript:execute_ajx_command('ajx_paging$0');");
                droplist.Attributes.Add("id", "ddlpager");                
                droplist.Font.Names = new string[] { "Arial", "Tahoma", "Verdana" };                
                droplist.Font.Bold  = true;
                droplist.Font.Size  = FontUnit.Point(8);
                droplist.ForeColor  = System.Drawing.Color.Black;
                droplist.Width      = Unit.Pixel(150);

                for (int i = 0; i < page_count; i++)
                {
                    droplist.Items.Add(new ListItem(Convert.ToString(i + 1) + " OF " + grd.PageCount.ToString(), i.ToString()));
                }

                droplist.SelectedIndex = page_index;
                
                e.Row.Cells[0].Controls.Add(droplist);
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[0].VerticalAlign   = VerticalAlign.Middle;
                e.Row.Cells[0].BackColor       = System.Drawing.Color.FromName("99CCCC");
            }
        }

        private string construct_total_rowcount()
        {
            GridView grd = this.grd_ajax;

            int icount_total = grd.PageSize * (grd.PageIndex + 1);
            int itotals = ((DataTable)Session["DATA_SOURCE_DB"]).Rows.Count;

            if (grd.PageCount > 1)
            {
                if (grd.Rows.Count < grd.PageSize)
                {
                    int list_index = grd.PageIndex - 1;

                    if (list_index == 0)
                    {
                        list_index = 1;
                    }

                    if (grd.PageIndex == (grd.PageCount - 1))
                    {
                        icount_total = itotals;
                    }
                    else
                    {
                        icount_total = (grd.PageSize * list_index) + grd.Rows.Count;
                    }
                }
            }
            else
            {
                if (itotals > icount_total)
                {
                    if (grd.Rows.Count > 0)
                    {
                        icount_total = grd.Rows.Count;
                    }
                    else
                    {
                        icount_total -= 1;
                    }
                }
                else
                {
                    icount_total = itotals;
                }
            }

            return "RECORD NUMBER </br>" + icount_total.ToString("#,###") + " OF " + itotals.ToString("#,###");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //WARNING! DO NOT REMOVE. THIS METHOD WILL SUPPRESS ANY ERROR IN RENDERING CONTROL
        }
    }
}
