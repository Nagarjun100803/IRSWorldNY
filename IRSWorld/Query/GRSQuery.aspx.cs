using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using Microsoft.VisualBasic;
using System.Security;
using System.Security.Permissions;

using IRSClassLib;

namespace IRSWorld.Query
{
    public partial class GRSQuery : System.Web.UI.Page
    {
        private String DS = "GRS_QUERY_DS";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GRSQuery));
            if (Page.IsPostBack)
            {
                if (Session["DOWNLOAD_PATH"] != null)
                {
                    try
                    {
                        String path = (String)Session["DOWNLOAD_PATH"];
                        Session["DOWNLOAD_PATH"] = null;
                        HttpResponse resp = HttpContext.Current.Response;
                        HttpRequest req = HttpContext.Current.Request;
                        resp.Buffer = true;
                        resp.Clear();
                        resp.ContentType = "application/octet-stream";
                        csFileIo cs = new csFileIo();
                        resp.AppendHeader("content-disposition", "attachment; filename=" + cs.getFilename(path));
                        int len = (int)cs.getFileSize(path);
                        byte[] b = new byte[len];
                        cs.open_input(path);
                        int size = cs.readBytes(b, 0, len);
                        cs.close_input();
                        resp.BinaryWrite(b);
                        resp.Flush();
                    }
                    catch (Exception ex)
                    {
                        new csControl().showMessage(this.Page, "DOWN_ERROR", ex.Message);
                    }
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool verify_pasword(String pwd)
        {
            irsUsers iu = new irsUsers();
            iu.IsWinApp = true;
            Object[] rs = iu.validate("sysadm1", pwd);
            if ((bool)rs[0]) return true;

            throw new Exception("Invalid Password.");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String[] execute_stmt(String stmt)
        {
            try
            {
                DATASET = null;

                irsSqlCommonsWs com = new irsSqlCommonsWs();
                com.IsWinApp = true;
                DataSet ds = com.getDataSet(com.getSqlCommand(stmt, false), "EXEC_STMT");
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

        private String render_span(HtmlGenericControl span)
        {
            StringBuilder sb = new StringBuilder();
            System.IO.StringWriter sw = new System.IO.StringWriter(sb);
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            span.RenderControl(htw);
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
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
        public void drop_ds()
        {
            DATASET = null;
            Session["OLD_PWD"] = null;
        }

        private String listdir(String args)
        {
            String plist = "";
            if (args.Length > 0) plist = args.Trim();
            csFileIo fio = new csFileIo();
            String cwd_name = (plist.Equals("")) ? Environment.CurrentDirectory : fio.getDirectory(plist);
            String cname = (cwd_name == null || cwd_name == String.Empty) ? plist : cwd_name;
            DataTable dt = new DataTable("Directory Listing");
            DataColumn col = new DataColumn(cname, typeof(System.String));
            dt.Columns.Add(col);

            try
            {
                if (cwd_name == null || cwd_name == String.Empty) cwd_name = FileSystem.CurDir();
                String pattern = fio.getFilename(plist);
                if ((pattern != null && pattern != String.Empty) & fio.directoryExist(cwd_name + @"\" + pattern))
                {
                    pattern = "";
                    cwd_name = cwd_name + @"\" + pattern;
                }
                String[] ss = (pattern == null || pattern == String.Empty) ? fio.listFiles(cwd_name) : fio.listFiles(cwd_name, pattern);
                if (ss == null) return send_span(plist + " No files qualified.");
                foreach (String s in ss)
                {
                    DataRow row = dt.NewRow();
                    row[cname] = s;
                    dt.Rows.Add(row);
                }
                String rs = convert_dt(dt);
                return rs;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private String send_span(String text)
        {
            HtmlGenericControl span = new HtmlGenericControl("span");
            span.Attributes.Add("class", "rsspan");
            span.InnerText = text;

            return render_span(span);
        }

        private String chdir(String args)
        {
            try
            {
                String plist = "";
                if (args == String.Empty) plist = (String)Session["OLD_PWD"];
                else if (args.Equals("~")) plist = AppDomain.CurrentDomain.BaseDirectory;
                else plist = args;

                FileSystem.ChDir(plist);

                return pwd();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private String pwd()
        {
            return send_span("Current Directory: " + FileSystem.CurDir());
        }

        private String view(String args)
        {
            csFileIo fio = new csFileIo();
            if (fio.exist(args))
            {
                String s = fio.readAllText(args);
                if (s.Length > 0)
                {
                    HtmlGenericControl span = new HtmlGenericControl("span");
                    span.InnerHtml = "<label><pre id=\"preResult\">" + s + "</pre></label>";
                    return render_span(span);
                }
                else
                {
                    return send_span(args + " is empty.");
                }
            }

            throw new Exception(args + " does not exist.");
        }

        private String parse_args(char[] cc, ref int i)
        {
            StringBuilder sb = new StringBuilder();
            bool quote = false;
            while (i < cc.Length)
            {
                if (cc[i] == ' ' & !quote) break;
                if (cc[i] == '"' & !quote)
                {
                    quote = true;
                }
                else if (cc[i] == '"' & quote)
                {
                    quote = false;
                }
                else
                {
                    sb.Append(cc[i]);
                }
                i++;
            }
            return sb.ToString();
        }

        private String[] parse_args(String args)
        {
            char[] cc = args.ToCharArray();
            ArrayList al = new ArrayList();
            int i = 0;

            while (i < cc.Length)
            {
                al.Add(parse_args(cc, ref i));
                i++;
            }

            String[] ss = null;
            if (al.Count > 0)
            {
                ss = new String[al.Count];
                al.CopyTo(ss);
            }
            return ss;
        }

        private String copy_file(String args)
        {
            String[] plist = parse_args(args);
            if (plist != null && plist.Length == 2)
            {
                try
                {
                    FileSystem.FileCopy(plist[0], plist[1]);
                    return send_span("copy " + args + " successful.");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            throw new Exception("Copy arguments are not valid" + args);
        }

        private String rename_file(String args)
        {
            String[] plist = parse_args(args);
            if (plist != null && plist.Length == 2)
            {
                try
                {
                    FileSystem.Rename(plist[0], plist[1]);
                    return send_span("Rename " + plist[0] + " to " + plist[1] + " successful.");
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            throw new Exception("Rename arguments are not valid" + args);
        }

        private String del(String args)
        {
            String msg = "";
            try
            {
                if (args != null && args.Length > 0)
                {
                    FileSystem.Kill(args);
                    msg = "del " + args + " successful.";
                }
                else
                {
                    msg = "del requires path as argument.";
                }

            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return send_span(msg);
        }

        private String clean_acctno(String acctno)
        {
            byte[] ac = Encoding.Default.GetBytes(acctno.Trim());
            StringBuilder sb = new StringBuilder();
            bool significant = false;
            for (int i = 0; i < ac.Length; i++)
            {
                if (ac[i] > 0 | ac[i] < 10)
                {
                    significant = true;
                }
                if (significant & (char.IsDigit(Convert.ToChar(ac[i]))))
                {
                    sb.Append(Encoding.Default.GetString(ac, i, 1));
                }
            }
            if (sb.Length > 0) return sb.ToString();

            return String.Empty;
        }

        private bool htContainsHt(Hashtable ht)
        {
            int yes_ctr = 0;

            foreach (Object value in ht.Values)
            {
                yes_ctr += (value.GetType() == typeof(Hashtable)) ? 1 : 0;
            }

            return (yes_ctr > 0);
        }

        private void convert_ht(Hashtable ht, String tbname, ref String htm)
        {
            if (htContainsHt(ht))
            {
                foreach (Object skey in ht.Keys)
                {
                    Hashtable sht = (Hashtable)ht[skey];
                    convert_ht(sht, skey.ToString(), ref htm);
                }
            }
            else
            {
                DataTable dt = new DataTable(tbname);
                dt.Columns.Add(new DataColumn("Attribute", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Value", typeof(System.String)));
                foreach (Object key in ht.Keys)
                {
                    DataRow row = dt.NewRow();
                    row["Attribute"] = (String)key;
                    row["Value"] = (String)ht[key];
                    dt.Rows.Add(row);

                }
                htm += convert_dt(dt) + "</br>";
            }
        }

        private String mquery(String acctno)
        {
            if (acctno != null && acctno.Length > 0)
            {
                String fcacctno = clean_acctno(acctno);
                if (fcacctno.Length > 9)
                {
                    csFlexcube fc = new csFlexcube();
                    String errmsg = "";
                    Hashtable ht = fc.getFcAcctDetailsFull(acctno, out errmsg);
                    if (ht == null)
                    {
                        String s = (errmsg == String.Empty) ? fc.Message : errmsg;
                        return send_span("MQ Query failed: " + s);
                    }

                    String htm = "";
                    foreach (Object mainkey in ht.Keys)
                    {
                        Hashtable ht_dtl = (Hashtable) ht[mainkey];
                        if (ht_dtl != null)
                        {
                            convert_ht(ht_dtl, mainkey.ToString(), ref htm);
                        }
                    }

                    return htm;
                }
                return send_span(acctno + "is not a flexcube acctno.");
            }
            return send_span("Requires acctno as parameter");
        }

        private String get(String args)
        {
            String iframe = "<iframe id=\"xframe\" name=\"xxframe\" style=\"width: 100%; height:100%\" src=\"GRSQueryUtil.aspx?download_path=" + args.Replace(@"\", @"\\") + "\" target=\"xxframe\"></iframe>";
            return iframe;
        }

        private String put(String args)
        {
            String iframe = "<iframe id=\"xframe\" name=\"xxframe\" style=\"width: 100%; height:100%\" src=\"GRSQueryUtil.aspx?upload_path=" + args.Replace(@"\", @"\\") + "\" target=\"xxframe\"></iframe>";
            return iframe;
        }

        [PermissionSetAttribute(SecurityAction.Demand, Unrestricted=true)]
        private DataTable getModules(ProcessModuleCollection pmods)
        {
            if (pmods.Count > 0)
            {
                SecurityPermission sp = new SecurityPermission(PermissionState.Unrestricted);

                DataTable dt = new DataTable("Modules");
                dt.Columns.Add(new DataColumn("Module_Name", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Filename", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Memory Size", typeof(System.String)));

                foreach (ProcessModule pm in pmods)
                {
                    DataRow row = dt.NewRow();
                    row["Module_Name"] = pm.ModuleName;
                    row["Filename"] = pm.FileName;
                    row["Memory Size"] = pm.ModuleMemorySize.ToString();
                    dt.Rows.Add(row);
                }

                return dt;
            }
            return null;
        }

        [PermissionSetAttribute(SecurityAction.Demand, Unrestricted=true)]
        private DataTable getThreads(ProcessThreadCollection pthreads)
        {
            if (pthreads.Count > 0)
            {

                DataTable dt = new DataTable("Threads");
                dt.Columns.Add(new DataColumn("ID", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Start_Time", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Total_CPU_Time", typeof(System.String)));
                dt.Columns.Add(new DataColumn("User_CPU_Time", typeof(System.String)));
                dt.Columns.Add(new DataColumn("Thread_State", typeof(System.String)));

                foreach (ProcessThread ph in pthreads)
                {
                    DataRow row = dt.NewRow();
                    row["ID"] = ph.Id.ToString();
                    row["Start_time"] = ph.StartTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                    row["Total_CPU_Time"] = ph.TotalProcessorTime.Milliseconds.ToString();
                    row["User_CPU_Time"] = ph.UserProcessorTime.Milliseconds.ToString();
                    row["Thread_State"] = Enum.GetName(typeof(ThreadState), ph.ThreadState);
                    dt.Rows.Add(row);
                }

                return dt;
            }
            return null;
        }

        [PermissionSetAttribute(SecurityAction.Demand, Unrestricted=true)]
        private String pstat(String args)
        {
            String rs = String.Empty;
            String username = String.Empty;
            try
            {
                String pname = (args.Length > 0) ? args.Trim() : "GRSFileLoader";
                Process[] ps = Process.GetProcessesByName(pname);

                if (ps != null && ps.Length > 0)
                {
                    DataTable dt = new DataTable(pname);
                    dt.Columns.Add(new DataColumn("Property", typeof(System.String)));
                    dt.Columns.Add(new DataColumn("Value", typeof(System.String)));

                    username = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    DataRow row = dt.NewRow(); row["Property"] = "Impersonation Name"; row["Value"] = username; dt.Rows.Add(row);

                    foreach (Process p in ps)
                    {
                        row = dt.NewRow(); row["Property"] = "Process ID"; row["Value"] = p.Id.ToString(); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Process Name"; row["Value"] = p.ProcessName; dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Priority Class"; row["Value"] = p.PriorityClass; dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Start Time"; row["Value"] = p.StartTime.ToString("MM/dd/yyyy hh:mm:ss.fff tt"); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Total Processor Time"; row["Value"] = p.TotalProcessorTime.Milliseconds.ToString(); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Working Set Size"; row["Value"] = p.WorkingSet64.ToString(); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Peak Working Set Size"; row["Value"] = p.PeakWorkingSet64.ToString(); dt.Rows.Add(row); 
                        row = dt.NewRow(); row["Property"] = "Virtual Memory Size"; row["Value"] = p.VirtualMemorySize64.ToString(); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Peak Virtual Memory Size"; row["Value"] = p.PeakVirtualMemorySize64.ToString(); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Paged Memory Size"; row["Value"] = p.PagedMemorySize64.ToString(); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Peak Paged Memory Size"; row["Value"] = p.PeakPagedMemorySize64.ToString(); dt.Rows.Add(row);
                        row = dt.NewRow(); row["Property"] = "Paged System Memory Size"; row["Value"] = p.PagedSystemMemorySize64.ToString(); dt.Rows.Add(row);

                        rs = convert_dt(dt) + "<br />";
                        String s = convert_dt(getModules(p.Modules));
                        if (s != null && s.Length > 0)
                        {
                            rs = rs + s + "<br />";
                        }
                        s = convert_dt(getThreads(p.Threads));
                        if (s != null && s.Length > 0)
                        {
                            rs = rs + s + "<br />";
                        }

                    }
                }
            }
            catch (Exception e)
            {
                rs = send_span("pstat main: " + e.Message + " " + e.StackTrace + " (" + username + ")");
            }

            return rs;
        }

        private String usage()
        {
            DataTable dt = new DataTable("Usage");
            dt.Columns.Add(new DataColumn("Command", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Abbreviation", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Parameter", typeof(System.String)));
            dt.Columns.Add(new DataColumn("Notes", typeof(System.String)));

            String[] cmd_name = { "dir", "cwd", "pwd", "view", "copy", "rename", "get", "mq", "ps", "help" };
            String[] cmd = { "ls", "cd", "", "vi", "cp", "ren", "", "", "", "?" };
            String[] args =
            {
                "[path]",
                "[path]",
                "",
                "path",
                "src_path dest_path",
                "old_path new_path",
                "path",
                "account number",
                "",
                ""
            };
            String[] notes =
            {
                "List directory info",
                "Change current directory",
                "Show current directory",
                "View content of a text file",
                "Copy file",
                "Rename a file",
                "Download a file from server",
                "Query account details from flexcube",
                "show process status of a process, requires admin. user",
                "Display Usage."
            };

            for (int i = 0; i < cmd_name.Length; i++)
            {
                DataRow row = dt.NewRow();
                row["Command"] = cmd_name[i];
                row["Abbreviation"] = cmd[i];
                row["Parameter"] = args[i];
                row["Notes"] = notes[i];

                dt.Rows.Add(row);
            }

            return convert_dt(dt);
        }

        private Object execute_cmd(String cmd, String args)
        {
            switch (cmd.ToUpper().Trim())
            {
                case "LS":
                case "DIR": return listdir(args);

                case "CD":
                case "CWD": return chdir(args);

                case "PWD": return pwd();

                case "VI":
                case "VIEW": return view(args);

                case "CP":
                case "COPY": return copy_file(args);

                case "REN":
                case "RENAME": return rename_file(args);

                case "MQ": return mquery(args);

                case "GET": return get(args);
                case "PUT": return put(args);
                case "DEL": return del(args);

                case "PS": return pstat(args);

                case "?":
                case "HELP": return usage();
            }
            throw new Exception (cmd + " unsupported. ");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object execute_cmd(String args)
        {
            if (args.Length > 1)
            {
                String cmdstring = args.Substring(1).Trim();
                int i = cmdstring.IndexOf(' ');
                if (i > 5) throw new Exception("Abnormal command length. " + i);
                if (i < 0 & cmdstring.Length > 5) throw new Exception("Uknown command " + cmdstring);
                String cmd = (i > -1) ? cmdstring.Substring(0, i) : cmdstring;
                String plist = "";
                if (i > -1 && cmdstring.Length > i)
                {
                    plist = cmdstring.Substring(i + 1).Trim();
                }
                try
                {
                    if (Session["OLD_PWD"] == null) Session["OLD_PWD"] = Environment.CurrentDirectory;
                    return execute_cmd(cmd, plist);
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            throw new Exception("Empty? ..");
        }
    }
}
