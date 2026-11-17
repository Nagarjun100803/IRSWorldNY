using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
namespace IRSClassLib
{
    [Serializable]
    public class irsBatchUpload : sqlHelperWs
    {
        private string _temp_dir = AppDomain.CurrentDomain.BaseDirectory + "temp";//"C:\\IRS_Loading\\temp";

        public int fileformat;
        public string fileformatdesc;
        public int fileuploadtype;
        public string fileuploadtypedesc;
        public string filename;
        public String ErrorMessage;
        private DataTable DTFormattedDataFromFile = null;
        private string fieldname1 = "";
        private string fieldname2 = "";
        public string ratetype;

        public irsBatchUpload(int _fileformat, int _fileuploadtype, string _filename)
        {
            csFileIo io = new csFileIo();
            try
            {
                this.fileuploadtype = _fileuploadtype;
                this.fileuploadtypedesc = getLookupCode("FileUploadType", _fileuploadtype);
                this.fileformat = _fileformat;
                this.fileformatdesc = getLookupCode(fileuploadtypedesc, _fileformat);
                this.filename = io.getFilename(_filename);


                string sql = "  select  recordfield,recordposition,recordlength,destinationTableField ";
                sql += " from	tbl_file_definition_mapper ";
                sql += " where	fileuploadtype	=	" + this.fileuploadtype;
                sql += " and	fileformat	=	 " + this.fileformat;
                sql += " and	destinationTableField in ('RemittingCurrency','amount') ";

                DataSet ds = base.getDataSet(base.getSqlCommand(sql, false), "TMAPPERCONT");

                DTFormattedDataFromFile = ds.Tables[0];

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            io = null;
        }

        public irsBatchUpload(int _fileformat, int _fileuploadtype, string _filename, string _rate_type)
        {
            csFileIo io = new csFileIo();
            try
            {
                this.fileuploadtype = _fileuploadtype;
                this.fileuploadtypedesc = getLookupCode("FileUploadType", _fileuploadtype);
                this.fileformat = _fileformat;
                this.fileformatdesc = getLookupCode(fileuploadtypedesc, _fileformat);
                this.filename = io.getFilename(_filename);
                this.ratetype = _rate_type;

                string sql = "  select  recordfield,recordposition,recordlength,destinationTableField ";
                sql += " from	tbl_file_definition_mapper ";
                sql += " where	fileuploadtype	=	" + this.fileuploadtype;
                sql += " and	fileformat	=	 " + this.fileformat;
                sql += " and	destinationTableField in ('RemittingCurrency','amount') ";

                DataSet ds = base.getDataSet(base.getSqlCommand(sql, false), "TMAPPERCONT");

                DTFormattedDataFromFile = ds.Tables[0];

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            io = null;
        }

        public irsBatchUpload(string _ft_desc, string _ff_desc, string _fn, string _rt)
        {
            csFileIo io = new csFileIo();
            try
            {
                
                this.fileuploadtypedesc = _ft_desc;
                this.fileformatdesc = _ff_desc;
                this.fileuploadtype = getLookupValue("FileUploadType", _ft_desc);
                this.fileformat = getLookupValue(fileuploadtypedesc, _ff_desc); 
                this.filename = io.getFilename(_fn);
                this.ratetype = _rt;

                //sp_getmapperbydesc
                DataSet ds = execStoredProcedure("sp_getmapperbydesc", new object[] { _ff_desc, _ft_desc });

                DTFormattedDataFromFile = ds.Tables[0];

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            io = null;
        }

        private int getLookupValue(string group, string code)
        {
            DataTable dt = null;
            object[] param = new object[] { group, code };
            try
            {
                dt = execStoredProcedureTable("SP_BatchEquivalentValues", param);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return Convert.ToInt32(dt.Rows[0]["Value"]);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                param = null;
                dt = null;
            }
            return 0;
        }

        private string getLookupCode(string fldgroup, int fldvalue)
        {
            object o = execScalar("SELECT FldCode FROM TBL_LOOKUP WHERE FLDGROUP = '" + fldgroup + "' AND FLDVALUE = " + fldvalue.ToString());
            return (o != null) ? o.ToString() : "";
        }

        public DataSet get_textfile_values()
        {
            DataTable dt = new DataTable("TBLSUMMARY");
            StreamReader sr = new StreamReader(this._temp_dir + @"\" + this.filename);

            DataTable tempdt;
            int x = 0;
            DataSet ds = new DataSet();
            while (true)
            {
                string buff = sr.ReadLine();
                if (buff == null)
                {
                    break;
                }
                else
                {
                    if (buff.Trim().Length < 1) break;
                }

                tempdt = new DataTable("TempTable");
                tempdt = get_mapper_and_contents(buff);
                if (x == 0)
                {
                    for (int i = 0; i < tempdt.Rows.Count; i++)
                    {
                        DataColumn c = new DataColumn();
                        c.ColumnName = tempdt.Rows[i]["RecordField"].ToString().ToUpper();
                        c.DataType = tempdt.Rows[i]["TEXT_VALUE"].GetType();
                        dt.Columns.Add(c);
                        if (tempdt.Rows[i][3].ToString().ToUpper().Contains("AMOUNT"))
                        {
                            this.fieldname2 = tempdt.Rows[i]["RecordField"].ToString(); ;
                        }
                        if (tempdt.Rows[i][3].ToString().ToUpper().Contains("REMITTINGCURRENCY"))
                        {
                            this.fieldname1 = tempdt.Rows[i]["RecordField"].ToString(); ;
                        }
                    }
                }
                DataRow dr = null;
                dr = dt.NewRow();
                foreach (DataColumn c in dt.Columns)
                {
                    for (int i = 0; i < tempdt.Rows.Count; i++)
                    {
                        if (c.ColumnName.ToString().ToUpper().Contains(tempdt.Rows[i][0].ToString().ToUpper()))
                        {
                            dr[c.ColumnName] = tempdt.Rows[i]["TEXT_VALUE"].ToString();
                        }
                    }
                }
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                x++;
            }
            sr.Close();
            //add Field Names Table
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            DataTable adt = new DataTable("TBLFIELDS");
            adt.Columns.Add("FIELD1");
            adt.Columns.Add("FIELD2");
            DataRow adr = adt.NewRow();
            adr["FIELD1"] = this.fieldname1;
            adr["FIELD2"] = this.fieldname2;
            adt.Rows.Add(adr);
            adt.AcceptChanges();
            ds.Tables.Add(adt);
            ds.AcceptChanges();
            return ds;
        }


        public DataSet write_textfile_values(string branchcode)
        {
            DataTable dt = new DataTable("TBLSUMMARY");
            StreamReader sr = new StreamReader(this._temp_dir + @"\" + this.filename);

            DataTable tempdt;
            int x = 0;
            DataSet ds = new DataSet();
            while (true)
            {
                string buff = sr.ReadLine();
                if (buff == null)
                {
                    break;
                }
                else
                {
                    if (buff.Trim().Length < 1) break;
                }

                tempdt = new DataTable("TempTable");
                tempdt = get_mapper_and_contents(buff);
                if (x == 0)
                {
                    write_data_to_tempfile(buff, branchcode);
                }

                x++;
            }

            return ds;
        }
        public void delete_temp_file()
        {
            string sql = "delete from tbl_ibatch_tempfile where text_filename = '" + this.filename + "' and convert(varchar,file_date,101) = '" + DateTime.Today.ToString("MM/dd/yyyy") + "' ";

            int r = base.sqlUpdate(sql);

        }
        public bool write_data_to_tempfile(string _buff, string branchcode)
        {
            String sql = "";
            irsTimeZone itz = new irsTimeZone();
            DateTime tranxdate = new DateTime();
            tranxdate = itz.branchLocalTime(branchcode);
            try
            {
                /*               text_data	varchar(1000) null,
               invalid_remarks	varchar(500) null,
               fileformat	int,
               fileuploadtype  int,
               text_filename	varchar(100),
               file_date	datetime,
               userid		varchar(50)*/
                //sql = "insert into tbl_ibatch_tempfile (text_data,invalid_remarks,fileformat,fileuploadtype, ";
                //sql += " text_filename, file_date,userid )  values ";
                //sql += " ('" + _buff + "',''," + this.fileformat + "," + this.fileuploadtype + ",'" + this.filename + "'";
                //sql += ",'" + tranxdate.ToString("MM/dd/yyyy") + "','" + ClientData.UserId + "') ";
                int ra = execStoredProcedureUpdate("sp_insert_batch_temp", new object[] { this.fileuploadtype, this.fileformat, this.filename, tranxdate.ToString("MM/dd/yyyy"), ClientData.UserId, _buff });
                //int ra = base.sqlUpdate(sql);
                return true;
            }
            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
            }
            return false;

        }

        public DataSet get_tempfile_data(string branchcode)
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();
            irsTimeZone itz = new irsTimeZone();
            DateTime tranxdate = new DateTime();
            try
            {
                if (this.fileuploadtypedesc.ToUpper() == "IBATCH")
                {
                    tranxdate = itz.branchLocalTime(branchcode);
                    cmd = base.getSqlCommand("proc_get_ibatch_summary", true);
                    cmd.Parameters.Add("@v_fileformat", SqlDbType.Int).Value = this.fileformat;
                    cmd.Parameters.Add("@v_fileuploadtype", SqlDbType.Int).Value = this.fileuploadtype;
                    cmd.Parameters.Add("@v_filename", SqlDbType.VarChar, 100).Value = this.filename;
                    cmd.Parameters.Add("@v_file_date", SqlDbType.DateTime).Value = tranxdate.ToString("MM/dd/yyyy");
                    cmd.Parameters.Add("@v_errmsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                    ds = base.getDataSet(cmd);

                    if (ds == null)
                    {
                        this.ErrorMessage = "ERROR: NO DATA FOUND";
                        return null;
                    }
                    if (cmd.Parameters["@v_errmsg"].Value != null && cmd.Parameters["@v_errmsg"].Value.ToString().Length == 0)
                    {
                        this.ErrorMessage = cmd.Parameters["@v_errmsg"].Value.ToString();
                    }

                    if (ds.Tables.Count > 0)
                    {
                        return ds;
                    }
                }
                else
                {
                    //DataTable dt = summaryTable();
                    ds = summaryDataSet();

                    irsBatchFileUpload file = null;
                    if (System.Web.HttpContext.Current.Session["IRS_BATCH_FILE_UPLOAD"] != null)
                    {
                        file = (irsBatchFileUpload)System.Web.HttpContext.Current.Session["IRS_BATCH_FILE_UPLOAD"];
                    }

                    string[] summary = file.AmountSummary.Split('|');

                    if (summary.Length > 0)
                    {

                        for (int i = 0; i < summary.Length; i++)
                        {
                            string[] sum = summary[i].Split('~');
                            ds.Tables[0].Rows.Add(sum);
                        }
                    }
                    return ds;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                cmd = null;
                itz = null;
            }
            return null;
        }

        private DataSet summaryDataSet()
        {
            DataTable dt = new DataTable();
            DataColumn col = new DataColumn();
            DataSet ds = new DataSet();
            try
            {   
                col.ColumnName = "CURRENCY";
                dt.Columns.Add(col);

                col = new DataColumn();
                col.ColumnName = "AMOUNT";
                dt.Columns.Add(col);

                col = new DataColumn();
                col.ColumnName = "NOOFITEMS";
                dt.Columns.Add(col);

                ds.Tables.Add(dt);
                return ds;
            }
            catch { }
            finally {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                if (col != null)
                {
                    col.Dispose();
                    col = null;
                }
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
            return null;
        }

        public DataTable get_mapper_and_contents(string buff)
        {
            if (this.DTFormattedDataFromFile == null)
            {
                string sql = "  select  recordfield,recordposition,recordlength,destinationTableField ";
                sql += " from	tbl_file_definition_mapper ";
                sql += " where	fileuploadtype	=	" + this.fileuploadtype;
                sql += " and	fileformat	=	 " + this.fileformat;
                sql += " and	destinationTableField in ('RemittingCurrency','amount') ";
                DataSet ds = base.getDataSet(base.getSqlCommand(sql, false), "TMAPPERCONT");
                this.DTFormattedDataFromFile = ds.Tables[0];
            }

            DataTable dt = new DataTable("TEMPTABLE");
            DataRow dr;
            try
            {

                foreach (DataColumn c in this.DTFormattedDataFromFile.Columns)
                {
                    dt.Columns.Add(c.ColumnName);

                }
                dt.Columns.Add("TEXT_VALUE");

                for (int i = 0; i < this.DTFormattedDataFromFile.Rows.Count; i++)
                {
                    DataRow tr = this.DTFormattedDataFromFile.Rows[i];
                    dr = dt.NewRow();
                    foreach (DataColumn c in DTFormattedDataFromFile.Columns)
                    {
                        dr[c.ColumnName] = tr[c.ColumnName];
                    }
                    dr["TEXT_VALUE"] = buff.Substring(Convert.ToInt32(dr["recordposition"]) - 1, Convert.ToInt32(dr["recordlength"]));
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                }
                return dt;
            }
            catch 
            {
                return null;
            }

        }

        public string get_mapper_and_contents(string buff, string columnname)
        {
            char delimiter = ' ';
            bool delimited = false;
            try
            {
                string sql = "  select Delimiter,recordfield,recordposition,recordlength,destinationTableField ";
                sql += " from	tbl_file_definition_mapper ";
                sql += " where	fileuploadtype	=	" + this.fileuploadtype;
                sql += " and	fileformat	=	 " + this.fileformat;
                //sql += " and	destinationTableField in ('RemittingCurrency','amount') ";
                sql += " and    destinationTableField = '" + columnname + "'";

                DataSet ds = base.getDataSet(base.getSqlCommand(sql, false), "TMAPPERCONT");

                try
                {
                    delimiter = Convert.ToChar(ds.Tables[0].Rows[0]["Delimiter"].ToString().Trim());
                    delimited = true;
                }
                catch { }
                int startpos = Convert.ToInt32(ds.Tables[0].Rows[0]["recordposition"]);
                int ilength = Convert.ToInt32(ds.Tables[0].Rows[0]["recordlength"]);

                if (delimited)
                {
                    return buff.Split(delimiter)[startpos].ToString();
                }
                else
                {
                    return buff.Substring(startpos - 1, ilength);
                }
            }
            catch 
            {
                return null;
            }

        }

        public void get_summary_value()
        {
            string workingfile = this._temp_dir + this.filename;
        }

        public bool decode_swift()
        {
            swift_decoder sd = new swift_decoder();
            csFileIo io = new csFileIo();
            string path = this._temp_dir + @"\" + this.filename;
            string newpath = this._temp_dir + @"\SWFTDECODE\";
            if (!io.directoryExist(newpath)) io.createDirectory(newpath);
            try
            {
                if (isconverted() == false)
                {
                    ArrayList errmsg = new ArrayList();
                    sd.reformat(path, newpath + this.filename, errmsg);

                    if (errmsg.ToString().Length > 0)
                    {
                        this.ErrorMessage = errmsg.ToString(); ;
                        return false;
                    }
                    else
                        return true;

                }
                return false;
            }
            catch
            {
                return false;
            }
            finally { sd = null; }
        }

        public bool decode_replace_swift()
        {
            swift_decoder sd = new swift_decoder();
            csFileIo io = new csFileIo();
            //string workingfile = this._temp_dir + @"\" + this.fileuploadtypedesc + @"\" + this.fileformatdesc + @"\" + filename; //this._temp_dir + @"\" + this.filename;
            string workingfile = this._temp_dir + @"\" + this.filename; //this._temp_dir + @"\" + this.filename;
            string newpath = this._temp_dir + @"\SWFTDECODE\";
            if (!io.directoryExist(newpath)) io.createDirectory(newpath);
            string newworkingfile = newpath + this.filename;
            try
            {
                if (isconverted() == true)
                {
                    ArrayList errmsg = new ArrayList(); ;
                    sd.reformat(workingfile, newworkingfile, errmsg);

                    if (errmsg.Count > 0)
                    {
                        this.ErrorMessage = errmsg.ToString();
                        return false;
                    }
                    else
                    {
                        //io.delete(workingfile);
                        string transferpathfile = this._temp_dir + @"\IBatch\SWFT\";
                        if (!io.directoryExist(transferpathfile)) io.createDirectory(transferpathfile);
                        transferpathfile += this.filename;
                        io.copy(newworkingfile, transferpathfile, true);
                        if (io.exist(transferpathfile))
                        {
                            this.ErrorMessage = "";
                            return true;
                        }
                        this.ErrorMessage = "FILE MAY NOT BE INTENTED FOR SWIFT";
                        return false;

                    }

                }
                this.ErrorMessage = "FILE MAY NOT BE INTENTED FOR SWIFT";
                return false;
            }
            catch
            {
                this.ErrorMessage = "FILE MAY NOT BE INTENTED FOR SWIFT";
                return false;
            }
            finally { sd = null; }
        }

        private DataSet parseDelimited(DataTable dt)
        {
            DataSet ds = null;
            DataTable newdt = null;
            
            try
            {
                newdt = new DataTable();
                string[] cn = new string[] { "CURRENCY", "NOOFITEMS", "AMOUNT" };
                for (int i = 0; i < cn.Length; i++)
                {
                    string col_name = cn[i];

                    DataColumn col = new DataColumn();
                    col.ColumnName = col_name;
                    newdt.Columns.Add(col);
                }
                
                for (int ndx = 0; ndx < dt.Rows.Count; ndx++)
                {
                    string currency = dt.Rows[ndx]["TextData"].ToString().Split(Convert.ToChar(dt.Rows[ndx]["Delimiter"].ToString().Trim()))[Convert.ToInt32(dt.Rows[ndx]["Currency"])];
                    Double amount = Convert.ToDouble(dt.Rows[ndx]["TextData"].ToString().Split(Convert.ToChar(dt.Rows[ndx]["Delimiter"].ToString().Trim()))[Convert.ToInt32(dt.Rows[ndx]["Amount"])]);

                    if (newdt.Rows.Count == 0)
                    {
                        object[] newRow = new object[] { };
                        newdt.Rows.Add(newRow);

                        newdt.Rows[newdt.Rows.Count - 1]["CURRENCY"] = currency;
                        newdt.Rows[newdt.Rows.Count - 1]["AMOUNT"] = amount;
                        newdt.Rows[newdt.Rows.Count - 1]["NOOFITEMS"] = 1;
                        continue;
                    }

                    //if (!cur_added)
                    //{
                    //    object[] newRow = new object[] { };
                    //    newdt.Rows.Add(newRow);

                    //    newdt.Rows[newdt.Rows.Count]["CURRENCY"] = currency;
                    //    newdt.Rows[newdt.Rows.Count]["AMOUNT"] = amount;
                    //    newdt.Rows[newdt.Rows.Count]["NOOFITEMS"] = 1;
                    //}
                    //else
                    //{
                    for (int new_ndx = 0; new_ndx < newdt.Rows.Count; new_ndx++)
                    {
                        if (newdt.Rows[new_ndx]["CURRENCY"].ToString() == currency)
                        {
                            newdt.Rows[new_ndx]["CURRENCY"] = currency;
                            newdt.Rows[new_ndx]["AMOUNT"] = Convert.ToDouble(newdt.Rows[new_ndx]["AMOUNT"]) + amount;
                            newdt.Rows[new_ndx]["NOOFITEMS"] = Convert.ToInt32(newdt.Rows[new_ndx]["NOOFITEMS"]) + 1;
                            break;
                        }
                        else
                        {
                            if (new_ndx == (newdt.Rows.Count - 1))
                            {
                                object[] newRow = new object[] { };
                                newdt.Rows.Add(newRow);

                                newdt.Rows[newdt.Rows.Count - 1]["CURRENCY"] = currency;
                                newdt.Rows[newdt.Rows.Count - 1]["AMOUNT"] = amount;
                                newdt.Rows[newdt.Rows.Count - 1]["NOOFITEMS"] = 1;
                                break;
                            }
                        }
                    }
                }

                if (newdt.Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(newdt);
                    return ds;
                }
            }
            catch 
            { }
            finally
            {
                dt = null;
                ds = null;
                newdt = null;
            }

            return null;
        }

        private bool isconverted()
        {
            swift_decoder sd = new swift_decoder();
            string path = this._temp_dir + @"\" + filename;//this._temp_dir + @"\" + this.filename;
            try
            {

                return sd.isRaw(path);

            }
            catch
            {

            }
            finally { sd = null; }
            return false;
        }

        private bool validate_decodedswiftfile()
        {
            csFileIo io = new csFileIo();
            try
            {
                io.open_input(this.filename);
                //header first
                io.readLine();
                string s = io.readLine();

                if (s.Length == 911) return true;
                return false;
            }
            catch
            { }
            finally { io = null; }
            return false;
        }

        public bool clean_tempfile()
        {
            DataSet ds = new DataSet();
            string sql = "";
            sql = "Delete from tbl_ibatch_tempfile where fileuploadtype = " + this.fileuploadtype;
            sql += " and fileformat = " + this.fileformat + " and text_filename = '" + this.filename + "' ";
            sql += " and convert(varchar,file_date , 101 ) = '" + DateTime.Today.ToString("MM/dd/yyyy") + "'";

            int ra = base.sqlUpdate(sql);

            //if (ra < 0)
            //{
            //    this.ErrorMessage = "ERROR: NO DATA FOUND";
            //    return false;
            //}



            return true;
        }
    }
}
