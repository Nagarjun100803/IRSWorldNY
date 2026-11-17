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
    public partial class AjaxGridNew : System.Web.UI.Page
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
            /*  
            this.grd_ajax.AllowPaging = true;
            this.grd_ajax.Width = Unit.Percentage(100);
            this.grd_ajax.PageSize = 5;
            this.grd_ajax.RowStyle.Font.Size = FontUnit.Point(8);
            this.grd_ajax.HeaderStyle.Font.Size = FontUnit.Point(8);
            this.grd_ajax.PagerStyle.Font.Size = FontUnit.Point(8);
            this.grd_ajax.RowStyle.Font.Names = new string[] { "Arial", "Tahoma", "Verdana" };
            this.grd_ajax.HeaderStyle.Font.Names = new string[] { "Arial", "Tahoma", "Verdana" };
            this.grd_ajax.PagerStyle.Font.Names = new string[] { "Arial", "Tahoma", "Verdana" };
             */
        }

        private void grd_ajx_command()
        {
            string ajx_command_name = string.Empty;
            string ajx_command_value = string.Empty;
            string ajx_grid_source = string.Empty;

            this.session_cleanup();

            if (Request.Form["ajx_command_name"] != null) { ajx_command_name = Request.Form["ajx_command_name"].ToString(); }
            if (Request.Form["ajx_command_value"] != null) { ajx_command_value = Request.Form["ajx_command_value"].ToString(); }

            if (Request.Form["ajx_grid_source"] != null)
            {
                ajx_grid_source = Request.Form["ajx_grid_source"].ToString();

                Session["GRID_SQL_SOURCE"] = ajx_grid_source;
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
                case "ajx_paging": this.grd_paging(command_value); break;
                case "ajx_sorting": this.grd_sorting(command_value); break;
                case "ajx_search": this.grd_search(command_value); break;
            }
        }

        private void grd_loading()
        {
            Session["DATA_SOURCE_DB"] = null;

            DataTable dt = this.grd_data_source();

            if (dt == null || dt.Rows.Count == 0)
            {
                Response.Write("<P></P><P></P><P></P><CENTER><B>NO RECORD FOUND.</B></CENTER>");
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
                s = s.Replace("[", "");
                s = s.Replace("%", "");

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
                    Response.Write("<P></P><P></P><P></P><CENTER><B>NO RECORD FOUND.</B></CENTER>");
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
            Session["CURRENT_SORT_ORDER"] = null;
            Session["CURRENT_PAGE_INDEX"] = null;
            Session["CURRENT_SEARCH_CRITERIA"] = null;
            Session["GRID_SQL_SOURCE"] = null;
        }

        private void grd_paging(string paging_param)
        {
            Session["CURRENT_PAGE_INDEX"] = Convert.ToInt32(paging_param);

            string sort_order = string.Empty;
            string search_criteria = string.Empty;

            if (Session["CURRENT_SORT_ORDER"] != null) { sort_order = (string)Session["CURRENT_SORT_ORDER"]; }
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
                        sqlHelper SQL = new sqlHelper();

                        if (SQL != null)
                        {
                            if (s.IndexOf("SELECT") == -1)
                            {
                                string[] param = s.Split('~'); // <--- spname~paramvalue1~paramvalue2~parametc
                                string sp_name = param[0];
                                string sp_value = null;
                                object[] array_value = null;

                                if (param.Length - 1 != 1)
                                {

                                    for (int i = 1; i < param.Length - 1; i++)
                                    {
                                        sp_value += param[i] + ",";
                                    }
                                    array_value = sp_value.Split(',');
                                }
                                else
                                {
                                    array_value = param[1].Split(',');
                                }

                                dt = SQL.execStoredProcedure(sp_name, array_value).Tables[0];
                                Session["DATA_SOURCE_DB"] = dt; return dt;
                            }

                            dt = SQL.getDataSet(SQL.getSqlCommand(s, false), "SQL_DATASET").Tables[0];

                            Session["DATA_SOURCE_DB"] = dt; return dt;
                        }
                    }

                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt;
                }

                Response.Write("<P></P><P></P><P></P><CENTER><B>NO RECORD FOUND.</B></CENTER>");
                Response.End();

            }
            catch
            {
                Response.Write("<P></P><P></P><P></P><CENTER><B>UNABLE TO CONNECT TO DATABASE SERVER.</B></CENTER>");
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

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int irow_item = 0;

                if (Session["DATA_ROW_ITEM"] != null)
                {
                    irow_item = Convert.ToInt32(Session["DATA_ROW_ITEM"].ToString());
                }

                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (irow_item != i)
                    {
                        e.Row.Cells[i].Visible = true;
                        e.Row.Cells[i].Text = string.Format("{0}<img id = 'asc_sort' style=\"cursor: pointer; cursor: hand;\" alt=\"Ascending\" src=\"../Images/up.jpg\" onclick=\"javascript:execute_ajaxgridview_command('ajx_sorting${0} Asc')\";/><img id = 'desc_sort' style = \"cursor: pointer; cursor: hand;\" alt=\"Descending\" src=\"../Images/down.jpg\" onclick=\"javascript:execute_ajaxgridview_command('ajx_sorting${0} desc') \"; />", e.Row.Cells[i].Text);
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
                    int row_item = 0;
                    string row_item_value = string.Empty;

                    HyperLink hyp = new HyperLink();

                    if (Session["DATA_ROW_ITEM"] != null)
                    {
                        row_item = Convert.ToInt32(Session["DATA_ROW_ITEM"].ToString());
                    }

                    row_item_value = e.Row.Cells[row_item].Text;

                    e.Row.Cells[row_item].Visible = false;

                    hyp.Style.Add("text-decoration", "none");
                    hyp.ForeColor = System.Drawing.Color.Black;
                    hyp.Text = e.Row.Cells[i].Text;
                    hyp.Attributes.Add("href", "javascript:execute_ajaxgridview_command('ajx_selected$" + row_item_value + "');");

                    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalbackcolor");
                    e.Row.Attributes.Add("onmouseover", "this.originalbackcolor = this.style.backgroundColor;this.style.backgroundColor = 'orange'");
                    e.Row.Cells[i].Controls.Add(hyp);
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Left;

                    hyp = null;
                }
            }

            else if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridView grd = (GridView)sender;

                int page_count = grd.PageCount;
                int x = 0;

                e.Row.Cells[0].Text = "";

                for (int i = 0; i < page_count; i++)
                {
                    int page_index = 0;

                    if (Session["CURRENT_PAGE_INDEX"] != null)
                    {
                        page_index = (int)Session["CURRENT_PAGE_INDEX"];
                    }

                    x += 1;

                    HyperLink hyp = new HyperLink();
                    hyp.Style.Add("text-decoration", "none");
                    hyp.ForeColor = System.Drawing.Color.Blue;
                    hyp.Font.Bold = true;
                    hyp.Text = x.ToString() + "&nbsp;";

                    if (page_index != i)
                    {
                        hyp.Attributes.Add("href", "javascript:execute_ajaxgridview_command('ajx_paging$" + i + "');");
                        hyp.ForeColor = System.Drawing.Color.Black;
                    }

                    Label l = new Label();
                    l.Text = "&nbsp;";

                    e.Row.Cells[0].Controls.Add(hyp); hyp = null;
                    e.Row.Cells[0].Controls.Add(l); l = null;
                }

                Label footers = new Label();

                footers.ForeColor = System.Drawing.Color.Black;
                footers.Font.Bold = true;
                footers.Text = "&nbsp;" +
                               "&nbsp;" +
                               "&nbsp;" +
                               "PAGE: " +
                    Convert.ToString(grd.PageIndex + 1) + " OF " + grd.PageCount.ToString();

                e.Row.Cells[0].Controls.Add(footers);
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

            return "RECORD NUMBER: " + icount_total.ToString("#,###") + " OF " + itotals.ToString("#,###");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //WARNING! DO NOT REMOVE. THIS METHOD WILL SUPPRESS ANY ERROR IN RENDERING CONTROL
        }

    }
}
