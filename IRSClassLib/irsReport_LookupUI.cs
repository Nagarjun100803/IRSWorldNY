/* ===========================================
 * AUTHOR: Jeffrey M. Buizon
 * DATE CREATED: 07/26/2010
 * DESCRIPTION: class for IRSWORLD reports Lookup
 * ============================================
 * 
 * REVISION: 
 * AUTHOR: Jeffrey M. Buizon
 * DATE MODIFIED: 07/26/2010
 * DESCRIPTION:
 * */
using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Web;
using System.IO;
namespace IRSClassLib
{
    [Serializable]
    public class irsReport_LookupUI : sqlHelperWs
    {
        #region CONSTANT VALUES
        /* values for datatypes
         * string
         * number
         * date
         * bit
         * blob
         */
        #endregion
        #region PRIVATE VARIABLE
        private string error_message = null;
        private string _error_message = null;
        private ArrayList rep_param = new ArrayList();
        private ArrayList rep_datatypes = new ArrayList();
        private ArrayList rep_values = new ArrayList();
        private int _rep_length;
        private string exporttype;
        private string sp_name;
        private string _sp_name;
        private string rep_path;
        private string _rep_path;
        private string rep_id;
        private string _rep_id;
        private string rep_pdfpath;
        private string _rep_pdfpath;
        private string rep_sessionid;
        private string _rep_sessionid;
        private string sprintToPrinter;
        private ArrayList rep_db_conn = new ArrayList();
        private string servername;
        private string dbname;
        private string username;
        private string password;
        public string errormessage = "";
        private DataSet rep_dataset = new DataSet();
        #endregion
        #region PROPERTY GET & SET
        public string messages
        {
            get { return this.error_message; }
            set { _error_message = value; }
        }
        public string p_sp_name
        {
            get { return this.sp_name; }
            set { _sp_name = value; }
        }
        public string p_rep_path
        {
            get { return this.rep_path; }
            set { _rep_path = value; }
        }
        public string p_rep_id
        {
            get { return this.rep_id; }
            set { _rep_id = value; }
        }
        
        #endregion
        #region PUBLIC METHOD
        #region Constructors
        public irsReport_LookupUI()
        {
            base.EnforceConstraint = false;
            this.TableName = "tbl_Reports";
            this.IdentityColumn = "SEQ_ID";
        }
        #endregion
        
        ~irsReport_LookupUI()
        {

        }

        public bool check_sub_branch(string branchcode, string subranchcode)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                cmd = base.getSqlCommand("proc_check_subbranch",true);
                cmd.Parameters.Add("@v_mainbranch", SqlDbType.VarChar, 10).Value = branchcode;
                cmd.Parameters.Add("@v_subbranch", SqlDbType.VarChar, 10).Value = subranchcode;
                cmd.Parameters.Add("@v_issubranch", SqlDbType.Int).Direction = ParameterDirection.Output;
                ds = base.getDataSet(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }

        }
        public void getReportLookup()
        {
            base.getUpdateDataSet("sp_get_reports_tbl");
        }
        public ArrayList parse_parameters(string args, string delim)
        {
            ArrayList param_arr = new ArrayList();

            foreach (string s in args.Split(delim.ToCharArray()))
            {
                param_arr.Add(s);
            }
            return param_arr;
        }
        public bool validate_report_parameter_length(string rep_params, string rep_datatype, string rep_values)
        {
            int rep_static_length = rep_params.Split(";".ToCharArray()).Length;
            if (rep_params.Split(";".ToCharArray()).Length == rep_static_length && rep_datatype.Split(";".ToCharArray()).Length == rep_static_length && rep_values.Split(";".ToCharArray()).Length == rep_static_length)
            {
                error_message = "";
                return false;
            }
            else
            {
                error_message = "Invalid Parameter Length OR values";
                return true;
            }
        }
        public bool check_datatypes_values()
        {

            bool pass = true;
            for (int i = 0; i < rep_datatypes.Count; i++)
            {
                if (!this.eval_datatype(rep_datatypes[i].ToString(), rep_values[i].ToString()))
                {
                    pass = false;
                    this.error_message = "ERROR IN DATATYPE:" + rep_datatypes[i].ToString() + " VALUEOF " + rep_values[i].ToString();
                    break;
                }


            }
            return pass;
        }
        public bool eval_datatype(string datatype, string rep_value)
        {
            bool pass = false;
            this.error_message = String.Empty;
            switch (datatype.ToLower())
            {
                case "string":
                    pass = true;
                    break;
                case "number":
                    int num;
                    pass = int.TryParse(rep_value, out  num);

                    break;
                case "date":
                    DateTime date1;
                    pass = DateTime.TryParse(rep_value, out date1);
                    break;
                default:
                    this.error_message = "ERROR IN DATATYPE:" + datatype + " VALUEOF " + rep_value;
                    pass = false;
                    break;
            }
            return pass;
        }

        public bool validate_sql(string sqlval)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                if (sqlval.ToLower().Contains("select"))
                {
                    cmd = base.getSqlCommand(sqlval, false);
                }
                else
                {
                    cmd = base.getSqlCommand(sqlval, true);
                }

                ds = base.getDataSet(cmd, "tbl_branches");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }

        }
        #region DATABASE ACCESS OF REPORTS
  
        public DataSet load_report_group_lookup(string branchcode, int isadmin)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                cmd = base.getSqlCommand("sp_get_rep_group");
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 5).Value = branchcode;
                cmd.Parameters.Add("@v_isadmin", SqlDbType.Int).Value = isadmin;
                ds = base.getDataSet(cmd, "tbl_reports");
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }

        }

        public DataSet load_rep_dropdown(string seq_id,string sp_sqlvalue)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;


                cmd = base.getSqlCommand(sp_sqlvalue);                
                ds = base.getDataSet(cmd, "tbl_reports_param");
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }
        public DataSet load_report_maingroup(string mgroupid)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                cmd = base.getSqlCommand("sp_get_reports");
                cmd.Parameters.Add("@maingroupid", SqlDbType.VarChar, 50).Value = mgroupid;
                ds = base.getDataSet(cmd, "tbl_reports");
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }
        public DataSet load_report_per_branch(string subgroupid)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;
                cmd = base.getSqlCommand("sp_get_reportpercategory");
                cmd.Parameters.Add("@v_subgroupid", SqlDbType.VarChar, 50).Value = subgroupid;
                ds = base.getDataSet(cmd, "tbl_reports");
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }
        public DataSet load_report_group_lookup_per_branch(string subgroupid, string branchcode, string role)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            try
            {
                ds = null;

                if (branchcode == null)
                    branchcode = "";
                cmd = base.getSqlCommand("sp_get_rep_group_perbranch");
                cmd.Parameters.Add("@v_subgroupid", SqlDbType.VarChar, 50).Value = subgroupid;
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 5).Value = branchcode;
                cmd.Parameters.Add("@v_role", SqlDbType.VarChar, 100).Value = role;
                ds = base.getDataSet(cmd, "tbl_reports");
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
            }
        }
        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            bool ok = false;
            this.load_sp_params(ht);
            ht["MainGroupID"] = "REPORTS";
            ht["MainGroupDESC"] = "REPORTS";
            if (!base.isUpdatable)
            {
                this.getReportLookup();
            }

            if (crud.Equals("DELETE"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"])); ;
                return base.Delete(sqlHelper.RETAIN_DATASET);
            }
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                ok = base.edit();
            }
            if (crud.Equals("ADD"))
            {
                ok = base.newRow(sqlHelper.RETAIN_DATASET);
            }
            if (ok)
            {

                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (!setColumn(key.ToString(), ht[key]))
                    {
                        ok = false;
                        break;
                    }



                }
                if (ok & addMode)
                {
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }
            return ok;
        }
        public DataTable saveUpdates(string maingroupid)
        {
            if (base.save(sqlHelper.RETAIN_DATASET))
            {
                return this.load_report_maingroup(maingroupid).Tables[0];
            }

            return null;
        }
        public Object[] getFs(String root)
        {
            short seqno = 0;
            short parid = -1;
            String item = "IRSWorld-REPORTS";
            ArrayList al = new ArrayList();
            al.Add(new Object[] { seqno, parid, item, "R" });

            listDirectories(root, ref seqno, seqno, al);

            if (al.Count > 0)
            {
                Object[] lsdir = new Object[al.Count];
                al.CopyTo(lsdir);

                return lsdir;
            }

            return null;
        }
        public void listDirectories(String s, ref short seqno, short parid, ArrayList al)
        {
            String[] ff = Directory.GetFileSystemEntries(s);
            foreach (String f in ff)
            {
                FileAttributes fa = File.GetAttributes(f);
                if (fa > 0)
                {
                    if (fa == FileAttributes.Directory)
                    {
                        seqno++;
                        al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "D" });
                        listDirectories(f, ref seqno, seqno, al);
                    }
                    else
                    {
                        String ext = Path.GetExtension(f);
                        if (ext != null && ext.Length > 0)
                        {
                            ext = ext.ToUpper();
                            if (ext.Equals(".RPT") | ext.Equals(".rpt"))
                            {
                                seqno++;
                                al.Add(new Object[] { seqno, parid, Path.GetFileName(f), "F" });
                            }
                        }
                    }
                }
            }
        }
        
        #endregion
        #endregion
        #region PRIVATE METHOD
        private irsClientData IRSClientData
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }
                return null;
            }
        }
        
        
       
        public DataTable get_sp_params(string spname)
        {
            SqlCommand sqlcmd = null;
            try
            {
                /*
                sqlcmd = base.getSqlCommand(spname, true);
                if (sqlcmd.Connection == null || sqlcmd.Connection.State != ConnectionState.Open)
                    sqlcmd.Connection = dbOpen();
                SqlCommandBuilder.DeriveParameters(sqlcmd);
                */

                sqlcmd = base.deriveParameters(spname);

                string paramnames = null;
                string paramtypes = null;
                string sqlvalue = null;


                DataTable dt = new DataTable("REPORTSPTBL");
                DataRow r = null;


                dt.Columns.Add("ParameterNames");
                dt.Columns.Add("ParameterTypes");
                dt.Columns.Add("ParameterDisplay");
                dt.Columns.Add("SQLValue");



                foreach (SqlParameter p in sqlcmd.Parameters)
                {
                    if (p.Direction == ParameterDirection.Input)
                    {
                        paramnames += p.ParameterName + "$";
                        paramtypes += p.SqlDbType.ToString() + "$";
                        sqlvalue += sqlvalue + "$";
                        r = dt.NewRow();
                        r["ParameterNames"] = paramnames;
                        r["ParameterTypes"] = paramtypes;
                        r["ParameterDisplay"] = paramnames;
                        r["SQLValue"] = sqlvalue;
                        dt.Rows.Add(r);
                        dt.AcceptChanges();
                    }
                }

                return dt;

            }
            catch (Exception e)
            {
                this.error_message = e.Message;
            }
            finally { sqlcmd.Dispose(); sqlcmd = null; }
            return null;
        }
        private void load_sp_params(Hashtable ht)
        {
            SqlCommand sqlcmd = null; //new SqlCommand();
            try
            {
                /*
                sqlcmd = base.getSqlCommand(ht["sp_Name"].ToString(), true);
                if (sqlcmd.Connection == null || sqlcmd.Connection.State != ConnectionState.Open)
                    sqlcmd.Connection = dbOpen();
                SqlCommandBuilder.DeriveParameters(sqlcmd);
                 * */
                sqlcmd = base.deriveParameters(ht["sp_Name"].ToString());

                string paramnames = null;
                string paramtypes = null;
                foreach (SqlParameter p in sqlcmd.Parameters)
                {
                    if (p.Direction == ParameterDirection.Input)
                    {
                        paramnames += p.ParameterName + "$";
                        paramtypes += p.SqlDbType.ToString() + "$";
                    }
                }
                if (!ht["ParameterNames"].ToString().Contains("$"))
                {
                    ht["ParameterNames"] = paramnames;
                    ht["Parametertypes"] = paramtypes;
                    ht["ParameterDisplay"] = paramnames.Replace("@", "");
                }
            }
            catch (Exception e)
            {
                this.error_message = e.Message;
            }
            finally { sqlcmd.Dispose(); sqlcmd = null; }
        }
        private void clear_arrays()
        {
            try
            {
                this.rep_param.Remove(0);
                this.rep_datatypes.Remove(0);
                this.rep_values.Remove(0);
            }
            catch { }
        }
        private void get_error_dbase()
        {
            this.error_message = string.Empty;

            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }
        private bool setSession(int repid)
        {

            DataSet ds = new DataSet();
            SqlCommand cmd = base.getSqlCommand("sp_get_reportbyid", true);
            cmd.Parameters.Add("@v_id", SqlDbType.Int).Value = repid;
            ds = base.getDataSet(cmd);
            if (ds.Tables[0].Rows.Count > 0)
            {
                System.Web.HttpContext.Current.Session["REP_PATH"] = ds.Tables[0].Rows[0][1].ToString();
                this.rep_path = ds.Tables[0].Rows[0][1].ToString();
                this.error_message = "";
                ds.Dispose();
                ds = null;
                cmd.Dispose();
                cmd = null;
                return true;
            }
            else
            {
                this.error_message = "CANNOT FIND REPORT ID";
                return false;
            }
        }
        
        
        private void writereportlogs(string msg)
        {
            csFileIo io = new csFileIo();

            if (io.exist(@"C:\reportlogs.txt"))
            {
                io.open_output(@"C:\reportlogs.txt", (int)csFileIo.write_modes.APPEND);
            }
            else
            {
                io.open_output(@"C:\reportlogs.txt", (int)csFileIo.write_modes.CREATE);
            }
            String s = "DATE:" + DateTime.Today.ToString() + " " + msg;
            io.putLine(s);
            io.close_output();

            io = null;
        }
        private void init_parameters(string mergeparam)
        {
            ArrayList arr = new ArrayList();
            arr = this.parse_parameters(mergeparam, "^");
            this.sp_name = arr[4].ToString();
            this.rep_path = arr[5].ToString();
            arr = null;
            
        }
        private void load_rep_conn(ArrayList arr)
        {
            for (int i = 0; i < arr.Count; i++)
            {

                if (arr[i].ToString().ToLower().Contains("server"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.servername = arr1[1].ToString();
                }
                if (arr[i].ToString().ToLower().Contains("database"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.dbname = arr1[1].ToString();
                }
                if (arr[i].ToString().ToLower().Contains("user"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.username = arr1[1].ToString();
                }
                if (arr[i].ToString().ToLower().Contains("password"))
                {
                    string[] arr1 = arr[i].ToString().Split('=');
                    this.password = arr1[1].ToString();
                }

            }
        }
       
       



        #endregion


    }
}
