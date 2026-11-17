using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace IRSClassLib
{
    [Serializable]
    public class irsExcelFileUpload : sqlHelperWs
    {
        #region VARIABLES
        private DateTime _br_time;
        private DateTime _srvr_time;
        private StringBuilder _sb_file;
        private Hashtable _currency_breakdown;
        private long _file_size;
        private string _loading_path;
        private string _file_name;
        private string _temp_dest;
        private string _file_format_desc;
        private string _file_type_desc;
        private string _temp_dir;
        private string _ftp_dir;
        private string _branch;
        private string _user;
        private string _amount_summary;
        private string _message = string.Empty;
        private string _rate_type = "-1";
        private double _hash_total;
        private int _seq_id;private int _file_format;
        private int _file_type;
        private int _record_count;
        private int _line_count;

        public int fileuploadtype;
        public int fileformat;

        public string fileformatdesc;
        public string fileuploadtypedesc;
        public string filename;
        public string ratetype;

        public String ErrorMessage;

        private Hashtable ht_pay_details = new Hashtable();
        private DataTable DTFormattedDataFromFile = null;
        #endregion
        #region Public

        public string FileName
        {
            get { return this._file_name; }
            set { this._file_name = value; }
        }

        public string RateType
        {
            get { return this._rate_type; }
            set { this._rate_type = value; }
        }

        public int FileFormat
        {
            get { return this._file_format; }
            set { this._file_format = value; }
        }

        public string MESSAGE
        {
            get { return this._message; }
            set { this._message = ""; }
        }

        public string TempDest
        {
            get { return this._temp_dir + @"\" + this._file_name; }
        }

        public string FTPDest
        {
            get { return this._ftp_dir; }
        }

        public string AmountSummary
        {
            get
            {
                return _amount_summary;
            }
        }

        public string FileDetails
        {
            get
            {
                //return (this._record_count > 0) ? 
                return this._file_type.ToString() + "|"
                + this._file_format.ToString() + "|"
                + this._file_name.ToString() + "|"
                + this._record_count.ToString() + "|"
                + this.getFileSize() + "|"
                + this._hash_total.ToString("###,###,###,###.###") + "|"
                + this._rate_type;// + "" : "";
            }
        }
        #endregion
        #region PUBLIC METHODS
        public bool clean_tempfile()
        {
            DataSet ds = new DataSet();
            string sql = "";
            sql = "Delete from tbl_ibatch_tempfile where fileuploadtype = " + this.fileuploadtype;
            sql += " and fileformat = " + this.fileformat + " and text_filename = '" + this.filename + "' ";
            sql += " and convert(varchar,file_date , 101 ) = '" + DateTime.Today.ToString("MM/dd/yyyy") + "'";

            int ra = base.sqlUpdate(sql);

            return true;
        }        
        public void readFile()
        {
            this._file_format_desc = this.getLookupCode(this._file_type_desc, this._file_format);
            irsExcelFileUpload fu = new irsExcelFileUpload(this._file_format, this._file_type, this._file_name, this._rate_type);
            string fn = this._file_name;
            System.IO.StreamReader sr = null;
            ArrayList line_error_msg = new ArrayList();
            this._file_size = new csFileIo().getFileSize(this.TempDest);
            double runamount = 0;
            csFileIo fio = new csFileIo();
            int record_count = 0;
            int line_count = 0;
            int line_errors = 0;
            this._currency_breakdown = new Hashtable();
            this._sb_file = new StringBuilder();

            try
            {
                if (this._file_type_desc.ToUpper() == "EXCEL")
                {
                    if (this._file_format == 1) // ACE
                    {
                        try
                        {
                            sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                        }
                        catch { }
                    }
                }

                if (!fu.clean_tempfile())
                {
                    throw new Exception("ERROR: DELETING DATA");
                }

                if (sr != null)
                {         
                    this.ht_pay_details = null;
                    try
                    {
                        if (this._file_type_desc.ToUpper() == "EXCEL")
                        {
                            string branchcode = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location ;

                                IRSClassLib.IBATCH_Class.ExcelLoader xls = new IRSClassLib.IBATCH_Class.ExcelLoader(branchcode);
                                this.ht_pay_details = xls.readXLSN(this._temp_dir + @"\" + this._file_name, this._file_format);

                                if (this.ht_pay_details == null) throw new Exception("FILE MAY NOT BE INTENDED FOR THIS FORMAT");

                                runamount = Convert.ToDouble(xls._dblTotalAmt);
                                line_count = xls.record_count - 1;
                                record_count = xls.record_count - 1;                                                                                                  
                        }
                        else
                        {
                            throw new Exception("FILE MAY NOT BE INTENDED FOR THIS FORMAT");
                        }
                    }
                    catch (Exception  e)
                    {
                        runamount += 0;
                        line_error_msg.Add("ERROR: " + e.Message + "~" + fu.ErrorMessage + "\r\n");
                        line_errors++;
                        this.writeAudit(" - verifile" + e.Message, csAuditLog.Category.Insert);
                        throw e;
                    }
                }
                this._hash_total = runamount;
                this._record_count = record_count;
                this._line_count = line_count;

                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }

                this.sendToFTP();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
                fu = null;
                fio = null;
            }
        }        
        public void readFile(string path)
        {
            string fn = this._file_name = new csFileIo().getFilename(path);
            irsExcelFileUpload fu = new irsExcelFileUpload(this._file_format, this._file_type, this._file_name, this._rate_type);
            System.IO.StreamReader sr = null;// = new StreamReader(path);
            ArrayList line_error_msg = new ArrayList();
            double runamount = 0;
            csFileIo fio = new csFileIo();
            int record_count = 0;
            int line_count = 0;

            this._currency_breakdown = new Hashtable();
            this._sb_file = new StringBuilder();
            this._file_name = fn;

            try
            {
                if (this._file_type_desc.ToUpper() == "EXCEL")
                {
                    if (this._file_format == 1) // ACE
                    {
                        sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                    }
                }

                if (!fu.clean_tempfile())
                {
                    throw new Exception("ERROR: DELETING DATA");
                }
                    string buff = sr.ReadLine();

                    if (buff == null) // commemted to remove warning ;
                    { }
                    else
                    {
                        if (buff.Trim().Length < 1) //commented to remove warning;
                        { }
                    }
                line_count++;
                this._sb_file.Append(buff + Environment.NewLine);


                //if (buff.Contains("HDR") | buff.Contains("TRL"));
                //else record_count++;

                if (this._file_type_desc.ToUpper() == "EXCEL")
                {
                    if (this._file_format == 1) // ACE
                    {
                        try
                        {
                            sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                        }
                        catch { }
                    }
                }

                this._hash_total = runamount;
                this._record_count = record_count;
                this._line_count = line_count;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
        }
        public DataSet get_rate_type(DateTime _tranxdate, string _branchcode)
        {
            SqlCommand cmd = new SqlCommand();

            cmd = base.getSqlCommand("sp_get_rates_per_type", true);
            cmd.Parameters.Add("@v_trandate", SqlDbType.DateTime).Value = _tranxdate.ToString("MM/dd/yyyy");
            cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 20).Value = _branchcode;

            return getDataSet(cmd, "TBLRates");
        }
        public string fileDir(string postedFile)
        {
            this.temp_dir();
            this._file_name = new csFileIo().getFilename(postedFile);
            csFileIo io = new csFileIo();
            if (!io.directoryExist(this._temp_dir))
            {
                io.createDirectory(this._temp_dir);
            }
            io = null;
            return this._temp_dir + @"\" + this._file_name;
        }
        public DataTable show_uploaded_files(int userrole, string branchcode, bool isadmin, string type)
        {
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_show_uploadedfiles", true);
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 20).Value = branchcode;
                cmd.Parameters.Add("@type", SqlDbType.VarChar, 2).Value = type;

                return getDataSet(cmd, "TBLUPLOAD").Tables[0];
            }
            catch
            {
                return null;
            }
        }
        public DataSet uploaderFileFormat(string fileType)
        {
            DataSet ds = null;
            try
            {
                ds = execStoredProcedure("sp_uploaderFileFormat", new object[] { this._branch, fileType });
                this._file_type = Convert.ToInt16(ds.Tables[0].Rows[0]["type"]);
                return ds;
            }
            catch { }
            finally
            {
                ds = null;
            }
            return ds;
        }
        public DataSet uploaderFileType(string type)
        {
            return execStoredProcedure("sp_uploaderFileType", new object[] { this._branch, type });
        }
        public string get_fileuploadole(int role)
        {
            try
            {
                return execScalar("sp_getroledesc", new object[] { role }).ToString();
            }
            catch
            {
                return "";
            }
        }
        public bool updateInventory()
        {
            this._br_time = new irsTimeZone().branchLocalTime(this._branch);
            this._srvr_time = new irsTimeZone().ServerTime;

            try
            {
                DataTable dt = getDataSet(getSqlCommand("SELECT * FROM TBL_FILE_INVENTORY WHERE FileName = '" + this._file_name + "'", false)).Tables[0];
                bool ok = false;
                SqlCommand cmd = base.getSqlCommand("sp_get_uploadedfiles", true);
                cmd.Parameters.Add("@v_filename", SqlDbType.VarChar, 150).Value = this._file_name;
                base.getUpdateDataSet(cmd);
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(dt.Rows[0][0].ToString()));
                base.addMode = false;

                ok = base.edit();

                //to do: insert, get hast total, record count.
                if (ok)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("ApprovedBy", this._user);
                    //ht.Add("ProcessingBranch", this._branch);
                    ht.Add("rate_type", this._rate_type);
                    ht.Add("Status", getLookupValue("FileStatus", "APPROVED"));
                    ht.Add("DateApproved", this._br_time);

                    foreach (Object key in ht.Keys)
                    {
                        setColumn(key.ToString(), ht[key]);
                    }

                    return base.save(sqlHelperWs.RETAIN_DATASET);
                }
            }
            catch (Exception e)
            {
                this.writeAudit("updateInventory - " + e.Message, csAuditLog.Category.Update);
                throw e;
            }

            this._message = base.Message;
            return false;
        }
        public int getLookupValue(string group, string code)
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
                _message = e.Message;
            }
            finally
            {
                param = null;
                dt = null;
            }
            return 0;
        }
        public bool file_exists()
        {

            bool isCompliance = false;
            string select = string.Empty;

            //check roles
            //agent : temp folder/files only
            //others: FTP/ temp
            try
            {
                if (this._file_type == 1) isCompliance = true;
                if (!isCompliance)
                {
                    select = "SELECT COUNT(1) FROM TBL_FILE_INVENTORY "
                            + "WHERE [FileName] = '" + this._file_name + "'";
                }
                else
                {
                    select = "SELECT COUNT(1) FROM TBL_FILE_INVENTORY "
                            + "WHERE FileFormat = " + _file_format + " "
                            + "AND FileName = '" + _file_name + "' "
                            + "AND UploadingBranch = '" + _branch + "' "
                            + "AND Status != 1"; //Status = 0 AND [FileName] = '" + this._file_name + "'";                            
                }

                if (Convert.ToBoolean(execScalar(select)))
                {
                    if (!isCompliance)
                    {
                        this._message = "File has been previously uploaded. Please select another file.";
                    }
                    else
                    {
                        this._message = "Service is busy processing same file. Please wait.";
                    }

                    return true;
                }
                else
                {
                    this._temp_dest = this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + this._file_name;
                    return false;
                }
            }
            catch (Exception e)
            {
                _message = "ERROR:" + Message;
                this.writeAudit(e.Message + " - file_exists", csAuditLog.Category.Select);
                return false;
            }
        }
        public bool insertRecord()
        {
            if (base.newRow())
            {
                return true;
            }
            this.getErrorDB();
            return false;
        }
        public override void getUpdateDS(int id)
        {
            SqlCommand cmdUploader = getSqlCommand("SELECT * FROM TBL_FILE_INVENTORY WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(cmdUploader);
        }
        public void setParamVal(string totrec, string runamt, string rate_type)
        {
            this._br_time = new irsTimeZone().branchLocalTime(this._branch);
            this._srvr_time = new irsTimeZone().ServerTime;

            try
            {
                setColumn("FileType", this._file_type);
                setColumn("FileFormat", this._file_format);
                setColumn("FileName", this._file_name);
                setColumn("UploadingBranch", this._branch);
                setColumn("UploadedBy", this._user);
                setColumn("DateUpdated", this._srvr_time);
                setColumn("DateUploaded", this._srvr_time);
                setColumn("DateUploadedLocal", this._br_time);
                setColumn("DateApproved", this._br_time);
                setColumn("IsDeleted", 0);
                setColumn("Destination", this._loading_path);
                setColumn("Status", (this._file_type_desc == "ARS") ? this.getLookupValue("FileStatus", "UNPROCESSED") : this.getLookupValue("FileStatus", "Approved"));
                setColumn("rate_type", rate_type);
                setColumn("TotalNoOfRecords", Convert.ToInt32(totrec));
                setColumn("HashTotal", this._hash_total);

            }
            catch (Exception e)
            {
                _message = "ERROR: " + e.Message;
                this.writeAudit("setParamVal - " + e.Message, csAuditLog.Category.Insert);
            }
        }
        public void setParamVal(string totrec, string runamt, string rate_type, string role)
        {
            this._br_time = new irsTimeZone().branchLocalTime(this._branch);
            this._srvr_time = new irsTimeZone().ServerTime;

            try
            {
                setColumn("FileType", this._file_type);
                setColumn("FileFormat", this._file_format);
                setColumn("FileName", this._file_name);
                setColumn("UploadingBranch", this._branch);
                setColumn("UploadedBy", this._user);
                setColumn("DateUpdated", this._srvr_time);
                setColumn("DateUploaded", this._srvr_time);
                setColumn("DateUploadedLocal", this._br_time);
                setColumn("DateApproved", this._br_time);
                setColumn("IsDeleted", 0);
                setColumn("Destination", this._loading_path);
                setColumn("Status", (role.ToUpper().Contains("AGENT")) ? this.getLookupValue("FileStatus", "UNPROCESSED") : this.getLookupValue("FileStatus", "Approved"));
                setColumn("rate_type", rate_type);
                setColumn("TotalNoOfRecords", Convert.ToInt32(totrec));
                setColumn("HashTotal", this._hash_total);

            }
            catch (Exception e)
            {
                _message = "ERROR: " + e.Message;
                this.writeAudit("setParamVal - " + e.Message, csAuditLog.Category.Insert);
            }
        }
        public bool commitRecord()
        {
            if (base.save())
            {
                StringBuilder logs = new StringBuilder();
                this._seq_id = Convert.ToInt32(base.IdentityValue);
                logs.Append("TABLE:TBL_FILE_INVENTORY:" + this.TableName);
                logs.Append("SEQ_ID:" + this._seq_id.ToString());
                writeAudit(logs.ToString(), csAuditLog.Category.Insert);
                return true;
            }
            this.getErrorDB(); return false;
        }
        public bool savedToTemp()
        {
            this.temp_dir();
            string ftps = new irsConfiguration().FTPConnectionString;
            string ftps1 = new irsConfiguration().appsetting("FTPSource1");
            string ftps2 = new irsConfiguration().appsetting("FTPSource2");
            irsFileDownloader dl = new irsFileDownloader();

            if ((ftps != null && ftps.Length == 0) | (ftps1 != null && ftps1.Length == 0) | (ftps2 != null && ftps2.Length == 0))
            {
                this._message = "Config not defined.";
                this.writeAudit("savedToTemp - " + this._message, csAuditLog.Category.FTP);
                return false;
            }
            string[] cons = new string[] { ftps, ftps1, ftps2 };

            for (int i = 0; i < cons.Length; i++)
            {
                if (dl.check_file(this._file_name, "temp", cons[i]))
                {
                    return dl.download_file(this._file_name, "temp", this.TempDest, cons[i]);
                }
                //csFtp ftp = new csFtp(cons[i]);

                //try
                //{
                //    if (ftp.connect())
                //    {
                //        if (ftp.binary(true))
                //        {
                //            if (ftp.cwd("temp"))
                //            {
                //                if (ftp.get(this._file_name, this.TempDest) > -1)
                //                {
                //                    this._message = "";
                //                    return true;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            throw new Exception("ERROR");
                //        }
                //    }

                //    this._message = ftp.Message;
                //}
                //catch (Exception e)
                //{
                //    this._message = e.Message;
                //    writeAudit(e.Message, csAuditLog.Category.FTP);
                //    return false;
                //}
                //finally
                //{
                //    writeAudit(ftp.Message, csAuditLog.Category.FTP);
                //    ftp.disconnect();
                //    ftp = null;
                //}
            }
            return false;
        }
        public void moveToLoadingDirectory()
        {
            csFileIo fio = new csFileIo();
            string ftpConString = new irsConfiguration().FTPConnectionString;
            csFtp ftp = new csFtp(ftpConString);
            string nl = Environment.NewLine;

            try
            {
                StringBuilder sb_file = new StringBuilder();

                if (ftp.connect())
                {

                    if (ftp.binary(true))
                    {
                        string[] dir = this._loading_path.Replace(@"\", "/").Split('/');
                        for (int i = 0; i < dir.Length; i++)
                        {
                            if (dir[i] != "")
                            {
                                ftp.mkdir(dir[i]);
                                ftp.cwd(dir[i]);
                            }
                        }

                        if (ftp.put(this._temp_dest))
                        {
                            if (!ftp.Message.Contains("550")) this._message = ftp.Message + " : ";
                            if (this._file_type_desc.ToUpper().Contains("COMPLIANCE"))
                                this._message += "Your file " + this._file_name + " is successfully uploaded.";
                            else this._message += "The file " + this._file_name + " will now be processed for remittance.";
                            this.writeAudit("FTP Successful - " + this._message, csAuditLog.Category.FTP);
                        }
                        else
                        {
                            this._message = "ERROR: Cannot proceed to your request. " + ftp.Message;
                        }
                    }
                    else
                    {
                        this._message = "ERROR: CANNOT SET BINARY MODE. " + ftp.Message ;
                    }
                }
                else
                {
                    this._message = "ERROR: Ftp failed - " + ftp.Message;
                }
            }
            catch (Exception e)
            {
                this._message = e.Message;
                throw new Exception(e.Message);
            }
            finally
            {
                if (this._message.Contains("ERROR"))
                {
                    this.writeAudit("FTP ERROR: " + this._message, csAuditLog.Category.FTP);
                }
                ftp.disconnect();
                ftp = null;
                this.deleteTempFile();
                this.uploadMailer();
            }
        }
        public void moveToLoadingDirectory(string fn)
        {
            if (this.getFileInventory())
            {
                if (new csFileIo().exist(TempDest))
                {
                    this._temp_dest = TempDest;
                }
                else if (new csFileIo().exist(this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + fn))
                {
                    this._temp_dest = this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + fn;
                }
                else if (new csFileIo().exist(this._temp_dir + @"\" + this._file_type_desc + @"\" + fn))
                {
                    this._temp_dest = this._temp_dir + @"\" + this._file_type_desc + @"\" + fn;
                }
                else if (new csFileIo().exist(this._temp_dir + @"\" + fn))
                {
                    this._temp_dest = this._temp_dir + @"\" + fn;
                }
                else
                {
                    this._message = "The file has been previously approved. \n\nPlease wait until the file is completely processed.";
                    writeAudit(this._message, csAuditLog.Category.FTP);
                    throw new Exception(this._message);
                }

                this.moveToLoadingDirectory();
            }
        }        
        public string[] emailRecipient(bool process, bool upload)
        {
            object[] param = new object[] { _file_type, _file_format, process, upload, this._branch };
            try
            {
                DataTable dt = new sqlHelperWs().execStoredProcedureTable("sp_get_email", param);

                {
                    if (dt.Rows.Count > 0)
                    {
                        string recipients = "";
                        for (int ndx = 0; ndx < dt.Rows.Count; ndx++)
                        {
                            recipients += dt.Rows[ndx]["Email"].ToString();
                            recipients += "~";
                        }
                        return recipients.Split('~');
                    }
                }
            }
            catch { }
            return null;
        }        
        public string getLookupDesc(string Group, int val)
        {
            object[] param = new object[] { Group, val };
            try
            {
                DataTable dt = execStoredProcedureTable("SP_getLookUpByVal", param);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["Description"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                this._message = "ERROR" + e.Message;
            }

            return "";
        }
        public string fileSummary(string fileName, string hashTotal, int noOfItems)
        {
            return this.fileSummary(fileName, hashTotal, noOfItems, -1, -1);
        }
        public string fileSummary()
        {
            return this.fileSummary(this._file_name, this._hash_total.ToString("### ### ### ###"), this._record_count);
        }
        public string fileSummary(string fileName, string hashTotal, int noOfItems, int noOfProcessed, int noOfRejected)
        {
            string nl = Environment.NewLine;
            StringBuilder sbEmail = new StringBuilder();
            sbEmail.Append(this.getPadded("File Name", " ", 16, false) + " : " + fileName + nl);
            sbEmail.Append(this.getPadded("Hash Total", " ", 16, false) + " : " + hashTotal + nl);
            sbEmail.Append(this.getPadded("Number of Items", " ", 16, false) + " : " + noOfItems.ToString() + nl);

            if (noOfProcessed > -1)
            {
                sbEmail.Append(this.getPadded("Processed", " ", 16, false) + " : " + noOfProcessed.ToString() + nl);
            }

            if (noOfRejected > -1)
            {
                sbEmail.Append(this.getPadded("Rejected", " ", 16, false) + " : " + noOfRejected.ToString() + nl);
            }

            return sbEmail.ToString();
        }        
        public void writeAudit(string Description, csAuditLog.Category Category)
        {
            new csAuditLog().writeAudit("", "File Loader", Description, "", "", Category);
        }
        public string getLookupCode(string Group, int val)
        {
            object[] param = new object[] { Group, val };
            try
            {
                DataTable dt = execStoredProcedureTable("SP_getLookUpByVal", param);
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0]["Code"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                this._message = e.Message;
            }

            return "";
        }        
        public void moveToTempLoadingDirectory()
        {
            this._message = "The file " + this._file_name + " has been successfully uploaded";
        }
        public bool checkDefaults()
        {
            //spGetBranchDefaultSetup
            DataTable dt = execStoredProcedureTable("spGetBranchDefaultSetup", new object[] { this._branch });
            bool yesDefaults = true;
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    switch (dt.Columns[col].ColumnName)
                    {
                        case "LocalCurrency":
                            if (dt.Rows[0][dt.Columns[col].ColumnName] == DBNull.Value || Convert.ToInt32(dt.Rows[0][dt.Columns[col].ColumnName]) < 1)
                            {
                                yesDefaults = false;
                                this._message += "Incomplete defaults setup: defaults should be defined for " + dt.Columns[col].ColumnName + Environment.NewLine;
                            }
                            break;

                        case "ReceiptMode":
                            if (dt.Rows[0][dt.Columns[col].ColumnName] == DBNull.Value || Convert.ToInt32(dt.Rows[0][dt.Columns[col].ColumnName]) < 1)
                            {
                                yesDefaults = false;
                                this._message += "Incomplete defaults setup: defaults should be defined for " + dt.Columns[col].ColumnName + Environment.NewLine;
                            }
                            break;

                        case "TranxPurpose":
                            if (dt.Rows[0][dt.Columns[col].ColumnName] == DBNull.Value || Convert.ToInt32(dt.Rows[0][dt.Columns[col].ColumnName]) < 1)
                            {
                                yesDefaults = false;
                                this._message += "Incomplete defaults setup: defaults should be defined for " + dt.Columns[col].ColumnName + Environment.NewLine;
                            }
                            break;

                        case "SourceOfFunds":
                            if (dt.Rows[0][dt.Columns[col].ColumnName] == DBNull.Value || Convert.ToInt32(dt.Rows[0][dt.Columns[col].ColumnName]) < 1)
                            {
                                yesDefaults = false;
                                this._message += "Incomplete defaults setup: defaults should be defined for " + dt.Columns[col].ColumnName + Environment.NewLine;
                            }
                            break;

                        case "RemittingCurrency":
                            if (dt.Rows[0][dt.Columns[col].ColumnName] == DBNull.Value || Convert.ToInt32(dt.Rows[0][dt.Columns[col].ColumnName]) < 1)
                            {
                                yesDefaults = false;
                                this._message += "Incomplete defaults setup: defaults should be defined for " + dt.Columns[col].ColumnName + Environment.NewLine;
                            }
                            break;

                        default: break;
                    }
                }
            }
            return yesDefaults;
        }
        public void setFile(int file_type, int file_format, string file_name, string rate_type)
        {
            this._file_type = file_type;
            this._file_format = file_format;
            this._file_name = file_name;
            this._rate_type = rate_type;
            this.setOtherVariables();
            this._message = string.Empty;
        }
        #endregion
        #region PRIVATE METHODS
        private void setOtherVariables()
        {
            this.prepare_records();
        }
        public bool prepare_records()
        {
            DataTable dt = null;
            object[] param = null;
            try
            {
                irsTimeZone itz = new irsTimeZone();
                this._br_time = itz.branchLocalTime(this._branch);
                this._srvr_time = itz.ServerTime;

                param = new object[] { this._file_type, this._file_format };
                dt = execStoredProcedureTable("sp_uploader_getfilepath", param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    this._file_format_desc = dt.Rows[0][1].ToString().Trim();
                    this._file_type_desc = dt.Rows[0][0].ToString().Trim();
                    string dir_file_type = new irsConfiguration().appsetting(this._file_type_desc + "Directory");

                    if (dir_file_type != null && dir_file_type != string.Empty)
                    {
                        this._loading_path = string.Empty;
                        this._loading_path = dir_file_type + @"\" + this._file_name;
                    }
                    else
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (this._file_format_desc != "" && this._file_format_desc != null)
                            {
                                this._loading_path = new irsConfiguration().appsetting("RootPath");
                                this._loading_path += this._file_type_desc + @"\"
                                                    + this._file_format_desc;
                            }
                            else
                            {
                                this._loading_path = new irsConfiguration().appsetting("RootPath");
                                this._loading_path += this._file_type_desc;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _message = "ERROR" + e.Message;
                this.writeAudit(e.Message + " - prepare_records", csAuditLog.Category.Insert);
                return false;
            }
            finally
            {
                dt = null;
                param = null;
            }
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

                    string file = string.Empty;
                    if (System.Web.HttpContext.Current.Session["AMOUNT_SUMMARY"] != null)
                    {
                        file = (string)System.Web.HttpContext.Current.Session["AMOUNT_SUMMARY"];
                    }

                    string[] summary = file.Split('|');

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
                col.ColumnName = "NOOFITEMS";
                dt.Columns.Add(col);

                col = new DataColumn();
                col.ColumnName = "AMOUNT";
                dt.Columns.Add(col);

                ds.Tables.Add(dt);
                return ds;
            }
            catch { }
            finally
            {
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
        private string write_contents_to_file(StreamReader sr, string filename)
        {
            csFileIo io = new csFileIo();
            string workingfile = "";
            workingfile = this._temp_dir + @"\" + filename;
            int recctr = 0;
            try
            {
                io.open_output(workingfile, (int)csFileIo.write_modes.CREATE);
                while (true)
                {
                    string buff = sr.ReadLine();
                    if (buff == null) break;
                    else
                    {
                        if (buff.Trim().Length < 1) break;
                    }
                    io.putLine(buff);
                    recctr++;
                }
                io.close_output();
                io = null;

                if (recctr > 0)
                    return "";
                else
                    return "ERROR NO DATA FOUND";
            }
            catch (Exception e)
            {
                this.writeAudit(e.Message + " - write_contents_to_file", csAuditLog.Category.Insert);
                return e.Message;
            }
            finally
            {
                sr.Close();
                sr.Dispose();
                sr = null;
            }
        }
        private void getErrorDB()
        {
            this._message = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
                this._message = base.Message;
        }
        private bool getFileInventory()
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getInventoryByFilename", new string[] { this._file_name });
                if (dt != null && dt.Rows.Count > 0)
                {
                    this._file_type = Convert.ToInt32(dt.Rows[0]["FileType"]);
                    this._file_type_desc = dt.Rows[0]["FileTypeDesc"].ToString();
                    this._file_format = Convert.ToInt32(dt.Rows[0]["FileFormat"]);
                    this._file_format_desc = dt.Rows[0]["FileFormatDesc"].ToString();
                    this._loading_path = dt.Rows[0]["Destination"].ToString();
                    this._record_count = Convert.ToInt32(dt.Rows[0]["RecordCount"]);
                    this._hash_total = Convert.ToDouble(dt.Rows[0]["HashTotal"]);
                    return true;
                }
            }
            catch
            { }
            finally
            {
                dt = null;
            }
            return false;
        }
        private void uploadMailer()
        {
            csMailer mail = new csMailer();
            string subject = "IRSWORLD Upload - " + this._file_name;
            string[] recipient = this.emailRecipient(false, true);
            string msg = "";
            msg = (this._file_type_desc.ToUpper() == "ARS") ? this.emailUpload() : this.defaultEmailMessage();

            try
            {
                if (mail.sendMail(subject, recipient, msg))
                {
                    this._message += "\nYou will receive an email containing file summary.";
                    writeAudit("Email done - " + mail.Message, csAuditLog.Category.Insert);
                }
            }
            catch (Exception ex)
            {
                this.writeAudit("Email Failure: " + ex.Message + " - uploadMailer", csAuditLog.Category.Insert);
            }
        }
        private string defaultEmailMessage()
        {
            StringBuilder sbMail = new StringBuilder();
            sbMail.Append(this._message + Environment.NewLine);
            sbMail.Append("File Name : " + this._file_name);
            sbMail.Append("Total number of records in file : " + this._record_count);
            sbMail.Append("\n\n\nNote: This is a system generated message.\nPlease do not reply to this email.");

            return sbMail.ToString();
        }
        private string agentContact()
        {
            return getLookupDesc("AgentContact", getLookupValue("AgentContact", "GMG"));
        }
        private string emailUpload()
        {
            return this.emailUpload(this._file_name, this._hash_total.ToString("### ### ### ###"), this._record_count);
        }
        private string emailUpload(string fileName, string hashTotal, int noOfItems)
        {
            StringBuilder sbEmail = new StringBuilder();
            string nl = Environment.NewLine;
            DataTable dt;

            try
            {
                dt = execStoredProcedureTable("SP_ARS_EMAIL", new object[] { "Upload" });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sbEmail.Append(dt.Rows[i][1].ToString() + nl + nl);
                }
            }
            catch { }
            finally { }

            if (sbEmail.Length > 0)
            {
                while (true)
                {
                    if (sbEmail.ToString().IndexOf("<") > -1)
                    {
                        int ndx1 = sbEmail.ToString().IndexOf("<");
                        int ndx2 = 0;
                        string container = string.Empty;

                        for (ndx2 = ndx1; ndx2 < sbEmail.ToString().Length; ndx2++)
                        {
                            if (sbEmail.ToString().Substring(ndx2, 1) == ">")
                            {
                                int len = ndx2 - ndx1 + 1;
                                container = sbEmail.ToString().Substring(ndx1, len);
                                break;
                            }
                        }

                        switch (container)
                        {
                            case "<FILE>": sbEmail.Replace(container, fileName + ".logs"); break;
                            case "<CONTACT>": sbEmail.Replace(container, this.agentContact()); break;
                            case "<DETAIL>": sbEmail.Replace(container, this.fileSummary(fileName, hashTotal, noOfItems)); break;
                            default: break;
                        }
                    }
                    else break;
                }
            }

            sbEmail.Append("*******This is a system generated email.*******");
            return sbEmail.ToString();
        }
        private string getLeftPadded(string val, string padder, int len)
        {
            return getPadded(val, padder, len, true);
        }
        private string getPadded(string val, string padder, int len, bool leftpadded)
        {
            string ret_val = string.Empty;
            int pad_len = len - val.Length;

            for (int i = 0; i < pad_len; i++)
            {
                ret_val += padder;
            }

            if (leftpadded)
            {
                return ret_val + val;
            }
            else
            {
                return val + ret_val;
            }
        }
        private void sendToFTP()
        {
            string ftpstring = new irsConfiguration().FTPConnectionString;
            csFtp ftp = new csFtp(ftpstring);
            string ftpTemp = "temp";
            try
            {
                if (ftp.connect())
                {
                    if (ftp.binary(true))
                    {
                        if (!ftp.cwd(ftpTemp))
                        {
                            ftp.mkdir(ftpTemp);
                            ftp.cwd(ftpTemp);
                        }

                        if (ftp.put(this.TempDest))
                        {
                            this._message = "";
                            return;
                        }
                    }
                    else
                    {
                        throw new Exception("CANNOT SET TO BINARY MODE");
                    }
                }

                _message = ftp.Message;
            }
            catch
            {
                _message = "ERROR: " + ftp.Message;
                writeAudit(_message, csAuditLog.Category.FTP);
            }
            finally
            {
                ftp.disconnect();
                ftp = null;
                deleteTempFile();
            }
        }
        private void deleteTempFile()
        {
            if (new csFileIo().exist(this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + this.FileName))
            {
                this._temp_dest = this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + this.FileName;
                new csFileIo().delete(this._temp_dest);
            }
            if (new csFileIo().exist(this._temp_dir + @"\" + this._file_type_desc + @"\" + this.FileName))
            {
                this._temp_dest = this._temp_dir + @"\" + this._file_type_desc + @"\" + this.FileName;
                new csFileIo().delete(this._temp_dest);
            }
            if (new csFileIo().exist(this._temp_dir + @"\" + this.FileName))
            {
                this._temp_dest = this._temp_dir + @"\" + this.FileName;
                new csFileIo().delete(this._temp_dest);
            }
        }
        private void temp_dir()
        {
            try
            {
                this._temp_dir = new irsConfiguration().appsetting("temp_dir").ToString();
            }
            catch
            {
                throw new Exception("Incomplete config: temp_dir");
            }

            if (!(new csFileIo().directoryExist(_temp_dir)))
            {
                new csFileIo().createDirectory(_temp_dir);
            }
        }
        private void ftp_dir()
        {
            try
            {
                this._ftp_dir = new irsConfiguration().appsetting("ftp_dir").ToString();
            }
            catch
            {
                throw new Exception("Incomplete config: ftp_dir");
            }

            if (!(new csFileIo().directoryExist(_ftp_dir)))
            {
                new csFileIo().createDirectory(_ftp_dir);
            }
        }
        private string getFileName(string fn)
        {
            if (this._file_type_desc.ToUpper().Contains("COMPLIANCE"))
            {
                return this._branch + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + fn;
            }

            return fn;
        }
        private string getFileSize()
        {
            double file_size_kb = Convert.ToDouble(this._file_size) / 1000;
            double file_size_mb = Convert.ToDouble(this._file_size) / 1000000;

            if (file_size_kb > 0.01)
            {
                return file_size_kb.ToString() + " kb";
            }
            else if (file_size_mb > 0.01)
            {
                return file_size_mb.ToString() + " mb";
            }
            else
            {
                return this._file_size.ToString() + " bytes";
            }
        }
        #endregion
        #region INITIALIZATION
        public irsExcelFileUpload(string br, string user, int file_type, int file_format, string filename, string rate_type)
        {
            temp_dir();
            ftp_dir();
            this.TableName = "TBL_FILE_INVENTORY";
            this.IdentityColumn = "SEQ_ID";
            this._message = string.Empty;
            this._branch = br;
            this._user = user;
            this._file_type = file_type;
            this._file_format = file_format; 
            this._rate_type = rate_type;
            this.prepare_records();
            this._message = string.Empty;
            this.getFileName(filename);
        }

        public irsExcelFileUpload(string _ft_desc, string _ff_desc, string _fn, string _rt)
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

        public irsExcelFileUpload(int _fileformat, int _fileuploadtype, string _filename, string _rate_type)
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

        public irsExcelFileUpload()
        {
            irsClientData log_info;
            try
            {
                log_info = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
                this._branch = log_info.Location;
                this._user = log_info.UserId;
            }
            catch
            {
            }

            this._br_time = new irsTimeZone().branchLocalTime(this._branch);
            this._srvr_time = new irsTimeZone().ServerTime;
            this.TableName = "TBL_FILE_INVENTORY";
            this.IdentityColumn = "SEQ_ID";
            this._message = string.Empty;
        }
        #endregion
    }
}
