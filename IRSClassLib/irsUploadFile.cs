using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Collections;
namespace IRSClassLib
{
    
    public class irsUploadFile : sqlHelper
    {
        csFileIo fio;
        irsConfiguration confg;
        private string _temp_dir = AppDomain.CurrentDomain.BaseDirectory + "temp";//"C:\\IRS_Loading\\temp";
        private StringBuilder sbFileContent;
        //private StringBuilder logs;
        private System.Web.UI.HtmlControls.HtmlInputFile InputFile;
        private Hashtable trans_items;
        private DateTime _date_file_created;
        private string _source_path;
        private string loading_path;
        private DateTime br_time;
        private DateTime srvr_time;
        private string _temp_dest;
        private string _file_name;
        private string _branch;
        private string _user;
        private int _file_type;
        private int _file_format;
        private string _file_type_desc;
        private string _file_format_desc;
        private int _seq_id;
        private long _file_size;
        private string _rate_type = "-1";
        private int _record_count;
        private string _hash_total;
        private string _message = string.Empty;

        #region private methods

        private class file_items
        {
            public string currency = "";
            public int noofitems = 0;
            public double running_amount = 0;
            private file_items fi;
            public file_items(string _curr, int _noofitems, double _run_amt)
            {
                fi.currency = _curr;
                fi.noofitems = _noofitems;
                fi.running_amount = _run_amt;
            }
        }

        private void getErrorDB()
        {
            this._message = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
                this._message = base.Message;
        }

        private void temp_dir()
        {
            if (!(new csFileIo().directoryExist(_temp_dir)))
            {
                new csFileIo().createDirectory(_temp_dir);
            }
        }

        private string agentContact()
        {
            return "Elise Rue G. Alegre - Ramos at (632) 526 3131 ext. 4463";
        }

        private string getHeader(int rec_count)
        {
            StringBuilder sb_head = new StringBuilder("HDR");

            sb_head.Append(getPadded(rec_count.ToString(), "0", 6, true));
            sb_head.Append("1");
            sb_head.Append(getPadded(this.USER, " ", 20, false));
            sb_head.Append(_date_file_created.ToString("MMddyyyy"));
            sb_head.Append(this._file_type.ToString());
            sb_head.Append(this._file_format.ToString());

            return sb_head.ToString();
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

        public string[] emailRecipient(bool process, bool upload)
        {
            object[] param = new object[] { _file_type, _file_format, process, upload };
            try
            {
                DataTable dt = new sqlHelper().execStoredProcedureTable("sp_get_email", param);

                if (dt != null)
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

        private void uploadMailer()
        {
            csMailer mail = new csMailer();
            string subject = "IRSWORLD Upload - " + this._file_name;
            string[] recipient = this.emailRecipient(false, true);
            string msg = "";
            msg = (this._file_type_desc.ToUpper() == "ARS") ? this.emailUpload() : this.defaultEmailMessage();
            
            //StringBuilder sbMail = new StringBuilder();
            //sbMail.Append(MESSAGE + Environment.NewLine);
            //sbMail.Append("Total number of records in file: " + this._record_count);
            //sbMail.Append("\n\n\nNote: This is a system generated message.\nPlease do not reply to this email.");
            try
            {
                if (mail.sendMail(subject, recipient, msg))
                {
                    MESSAGE += "\nYou will receive an email containing file summary.";
                    writeAudit("Email done - " + mail.Message, csAuditLog.Category.Insert);
                    //MESSAGE += "\nemail done.";
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
            sbMail.Append(MESSAGE + Environment.NewLine);
            sbMail.Append("File Name : " + this._file_name);
            sbMail.Append("Total number of records in file : " + this._record_count);
            sbMail.Append("\n\n\nNote: This is a system generated message.\nPlease do not reply to this email.");

            return sbMail.ToString();
        }
        #endregion

        #region public methods
        public string RATE_TYPE
        {
            get { return this._rate_type; }
            set { this._rate_type = value; }
        }

        public string FILE_NAME
        {
            get { return this._file_name; }
            set { this._file_name = value; }
        }

        public int FILE_TYPE
        {
            get { return this._file_type; }
        }
        public string FILE_TYPE_DESC
        {
            get { return this._file_type_desc; }
        }

        public int FILE_FORMAT
        {
            get { return this._file_format; }
        }
        public string FILE_FORMAT_DESC
        {
            get { return this._file_format_desc; }
        }

        public int SEQ_ID
        {
            get { return _seq_id; }
        }

        public string USER
        {
            get { return _user; }
            set { _user = value; }
        }

        public string BRANCH
        {
            get { return this._branch; }
            set { this._branch = value; }
        }

        public string MESSAGE
        {
            get { return this._message; }
            set { this._message = value; }
        }

        public string TEMP_DEST
        {
            get { return this._temp_dest; }
        }

        public string Loading_Dir
        {
            get { return this.loading_path; }
        }

        public bool checkCurrencyRates()
        {
            string query = "SELECT * FROM V_TBL_CURRENCY_RATES WHERE TranxDate IN (SELECT MAX(TRANXDATE) FROM V_TBL_CURRENCY_RATES WHERE BranchCode = '" + this._branch + "')";
            try
            {
                DataSet ds = getDataSet(getSqlCommand(query, false));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;           
        }

        public string veriftpfile(string fullpathname)
        {
            this.sbFileContent = new StringBuilder();
            csFileIo fio = new csFileIo();
            System.IO.StreamReader sr = new System.IO.StreamReader(fullpathname);
            int record_count = 0;
            int line_count = 0;
            int line_errors = 0;
            ArrayList line_error_msg = new ArrayList();
            double runamount = 0;
            this._source_path = fullpathname;
            this._file_name = fio.getFilename(this._source_path);
            //this.
            irsBatchUpload bu = new irsBatchUpload(this.FILE_FORMAT, this.FILE_TYPE, this._file_name);
            pyrl_decryptor pd = new pyrl_decryptor();
            if (this.FILE_TYPE == 3)
            {
                if (this.FILE_FORMAT == 4)
                {
                    string s = write_contents_to_file(sr, this._file_name);
                    if (s.Trim().Length == 0)
                        bu.decode_replace_swift();
                    if (bu.ErrorMessage.Length == 0)
                        sr = new StreamReader(this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" + this._file_name);
                    //sr = new StreamReader(this._temp_dir + @"\" + /*this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" +*/ this._file_name);
                }
                if (this.FILE_FORMAT == 1)
                {
                    string s = write_contents_to_file(sr, this._file_name);
                    if (s.Trim().Length == 0)
                        this.decrypt_pyrlfile();

                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);//this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + this._file_name);
                    //sr = new StreamReader(this._temp_dir + @"\" + /*this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC +*/ this._file_name);
                }
            }

            if (!bu.clean_tempfile())
            {
                throw new Exception("ERROR: DELETING DATA");
            }

            while (true)
            {
                string buff = sr.ReadLine();

                if (buff == null) break;
                else
                {
                    if (buff.Trim().Length < 1) break;
                }
                line_count++;
                this.sbFileContent.Append(buff + Environment.NewLine);


                if (buff.Contains("HDR")) continue;
                else record_count++;

                if (bu.write_data_to_tempfile(buff, this._branch))
                {

                    try
                    {
                        runamount = runamount + (Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount")) / 100);
                    }
                    catch (Exception e)
                    {
                        line_error_msg.Add(bu.ErrorMessage);
                        line_errors++;
                        runamount = runamount + 0;
                        this.writeAudit(e.Message + " veriftpfile", csAuditLog.Category.Insert);
                    }
                }
                else
                {
                    line_error_msg.Add(bu.ErrorMessage);
                    line_errors++;
                }

                //this.add_currency(get_mapper_currency(buff));
            }
            this._record_count = record_count;

            return record_count.ToString() + "|" + line_count.ToString() + "|" + runamount.ToString() + "|" + this.RATE_TYPE + "|";
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private bool saveuploaded(System.Web.UI.HtmlControls.HtmlInputFile file)
        {

            bool pass = false;
            string tempfolder = _temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC ;           
            string tempfilenamepath = tempfolder + @"\" + this.FILE_NAME;
            
            try
            {
                csFileIo io = new csFileIo();

                if (!io.directoryExist(tempfolder)) io.createDirectory(tempfolder);
                file.PostedFile.SaveAs(tempfilenamepath);
                _message = "";
                pass = true;
            }
            catch(Exception e)
            {
                _message = e.Message;
                pass = false;
            }
            finally
            {
                this.GetMimeType(tempfilenamepath);
            }

            return pass;

        }

        public string verifile(System.IO.StreamReader sr)
        {
            this.sbFileContent = new StringBuilder();
            csFileIo fio = new csFileIo();
            //System.IO.StreamReader sr = new System.IO.StreamReader(file.PostedFile.InputStream);
            //IBATCH_Class.ARSValidation val = new IBATCH_Class.ARSValidation(sr);
            //System.IO.StreamReader sr2 = new System.IO.StreamReader(file.PostedFile.InputStream);
            int record_count = 0;
            int line_count = 0;
            int line_errors = 0;
            ArrayList line_error_msg = new ArrayList();
            double runamount = 0;
            //this._source_path = file.PostedFile.FileName;
            this._file_name = fio.getFilename(this._source_path);


            //if (!this.saveuploaded(file))
            //{
            //    return this._message;
            //}

            if (this.FILE_TYPE_DESC.ToUpper() == "ARS")
            {
                string s = write_contents_to_file(sr, this._file_name);
                if (s.Trim().Length == 0)
                {
                    if (!this.validARS())
                        throw new Exception(this._message);

                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                }
            }

            irsBatchUpload bu = new irsBatchUpload(this.FILE_FORMAT, this.FILE_TYPE, this._file_name, this.RATE_TYPE);
            if (this.FILE_TYPE == 3)
            {
                if (this.FILE_FORMAT == 4)
                {
                    string s = write_contents_to_file(sr, this._file_name);
                    if (s.Trim().Length == 0)
                        bu.decode_replace_swift();
                    if (bu.ErrorMessage.Length == 0)
                        sr = new StreamReader(this._temp_dir + @"\IBatch\SWFT\" + this._file_name);
                    //sr = new StreamReader(this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC +   @"\" + this._file_name);
                }
                if (this.FILE_FORMAT == 1)
                {
                    string s = write_contents_to_file(sr, this._file_name);
                    if (s.Trim().Length == 0)
                        this.decrypt_pyrlfile();
                    //sr = new StreamReader(this._temp_dir + @"\" /*+ this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\"*/ + this._file_name);
                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);// + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" + this._file_name);
                }
            }

            if (!bu.clean_tempfile())
            {
                throw new Exception("ERROR: DELETING DATA");
            }

            while (true)
            {
                string buff = sr.ReadLine();

                if (buff == null) break;
                else
                {
                    if (buff.Trim().Length < 1) break;
                }
                line_count++;
                this.sbFileContent.Append(buff + Environment.NewLine);


                if (buff.Contains("HDR") | buff.Contains("TRL")) continue;
                else record_count++;

                if (this.FILE_TYPE_DESC.ToUpper() == "IBATCH" | this.FILE_TYPE_DESC == "ARS") // exclude compliance
                {
                    if (bu.write_data_to_tempfile(buff, this._branch))
                    {
                        try
                        {
                            if (this.FILE_TYPE_DESC.ToUpper() == "IBATCH")
                            {
                                runamount += (Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount")) / 100);
                            }
                            else
                            {
                                runamount += Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount"));
                            }
                        }
                        catch (System.FormatException e)
                        {
                            runamount += 0;
                            line_error_msg.Add("ERROR: " + e.Message + "~" + bu.ErrorMessage + "\r\n");
                            line_errors++;
                            this.writeAudit(" - verifile" + e.Message, csAuditLog.Category.Insert);
                        }
                        finally
                        {
                        }
                    }
                    else
                    {
                        line_error_msg.Add(bu.ErrorMessage);
                        line_errors++;
                    }
                }
            }
            this._hash_total = runamount.ToString();
            //this.updateHashTotal(runamount.ToString());
            this._record_count = record_count;
            //this.deleteFile(this._temp_dir + @"\" /*+ this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\"*/ + this._file_name);
            return record_count.ToString() + "|" + line_count.ToString() + "|" + runamount.ToString() + "|" + this.RATE_TYPE + "|";
        }

        public string verifile(System.Web.UI.HtmlControls.HtmlInputFile file)
        {
            this.sbFileContent = new StringBuilder();
            csFileIo fio = new csFileIo();
            System.IO.StreamReader sr = new System.IO.StreamReader(file.PostedFile.InputStream);
            //IBATCH_Class.ARSValidation val = new IBATCH_Class.ARSValidation(sr);
            System.IO.StreamReader sr2 = new System.IO.StreamReader(file.PostedFile.InputStream);
            int record_count = 0;
            int line_count = 0;
            int line_errors = 0;
            ArrayList line_error_msg = new ArrayList();
            double runamount = 0;
            this._source_path = file.PostedFile.FileName;
            this._file_name = fio.getFilename(this._source_path);


            if (!this.saveuploaded(file))
            {
                return this._message;
            }

            if (this.FILE_TYPE_DESC.ToUpper() == "ARS")
            {
                string s = write_contents_to_file(sr, this._file_name);
                if (s.Trim().Length == 0)
                {
                    if (!this.validARS())
                        throw new Exception(this._message);

                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                }
            }

            irsBatchUpload bu = new irsBatchUpload(this.FILE_FORMAT, this.FILE_TYPE, this._file_name, this.RATE_TYPE);
            if (this.FILE_TYPE == 3)
            {
                if (this.FILE_FORMAT == 4)
                {
                    string s = write_contents_to_file(sr, this._file_name);
                    if (s.Trim().Length == 0)
                        bu.decode_replace_swift();
                    if (bu.ErrorMessage.Length == 0)
                        sr = new StreamReader(this._temp_dir + @"\IBatch\SWFT\" + this._file_name);
                    //sr = new StreamReader(this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC +   @"\" + this._file_name);
                }
                if (this.FILE_FORMAT == 1)
                {
                    string s = write_contents_to_file(sr, this._file_name);
                    if (s.Trim().Length == 0)
                        this.decrypt_pyrlfile();
                    //sr = new StreamReader(this._temp_dir + @"\" /*+ this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\"*/ + this._file_name);
                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);// + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" + this._file_name);
                }
            }

            if (!bu.clean_tempfile())
            {
                throw new Exception("ERROR: DELETING DATA");
            }

            while (true)
            {
                string buff = sr.ReadLine();

                if (buff == null) break;
                else
                {
                    if (buff.Trim().Length < 1) break;
                }
                line_count++;
                this.sbFileContent.Append(buff + Environment.NewLine);


                if (buff.Contains("HDR") | buff.Contains("TRL")) continue;
                else record_count++;

                if (this.FILE_TYPE_DESC.ToUpper() == "IBATCH" | this.FILE_TYPE_DESC == "ARS") // exclude compliance
                {
                    if (bu.write_data_to_tempfile(buff, this._branch))
                    {
                        try
                        {
                            if (this.FILE_TYPE_DESC.ToUpper() == "IBATCH")
                            {
                                runamount += (Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount")) / 100);
                            }
                            else
                            {
                                runamount += Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount"));
                            }
                        }
                        catch (System.FormatException e)
                        {
                            runamount += 0;
                            line_error_msg.Add("ERROR: " + e.Message + "~" + bu.ErrorMessage + "\r\n");
                            line_errors++;
                            this.writeAudit(" - verifile" + e.Message, csAuditLog.Category.Insert);
                        }
                        finally
                        {
                        }
                    }
                    else
                    {
                        line_error_msg.Add(bu.ErrorMessage);
                        line_errors++;
                    }
                }
            }
            this._hash_total = runamount.ToString();
            //this.updateHashTotal(runamount.ToString());
            this._record_count = record_count;
            //this.deleteFile(this._temp_dir + @"\" /*+ this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\"*/ + this._file_name);
            return record_count.ToString() + "|" + line_count.ToString() + "|" + runamount.ToString() + "|" + this.RATE_TYPE + "|";
        }
        
        //private void deleteFile(string dir)
        //{
        //    csFileIo iod = new csFileIo();

        //    try
        //    {
        //        if (iod.exist(dir))
        //        {
        //            iod.delete(this._temp_dir + @"\" /*+ this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\"*/ + this._file_name);
        //        }
        //    }
        //    catch
        //    { }

        //    iod = null;
        //}

        private void decrypt_pyrlfile()
        {
            csFileIo io = new csFileIo();
            pyrl_decryptor pd = new pyrl_decryptor();

            try
            {
                //string workingfile = this._temp_dir + @"\" /*+ this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\"*/ + this.FILE_NAME;
                //string workingfile = this._temp_dir + @"\" + this.FILE_TYPE_DESC + this.FILE_NAME;// +@"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" + this.FILE_NAME;
                string workingfile = this._temp_dir + @"\" + this.FILE_NAME;
                string newpath = this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" + @"\PYRLDECODE\";
                if (!io.directoryExist(newpath)) io.createDirectory(newpath);
                string newworkingfile = newpath + this.FILE_NAME;


                String scontents = pd.Decrypt(io.readAllText(workingfile));


                //if (io.exist(newworkingfile))
                //{
                //    io.delete(newworkingfile);
                //}
                io.open_output(newworkingfile, (int)csFileIo.write_modes.CREATE);
                io.putLine(scontents);
                io.close_output();
                io.copy(newworkingfile, workingfile, true);
                return;
            }
            catch (Exception e)
            {
                this.writeAudit(" - decryptfile" + e.Message, csAuditLog.Category.Insert);
            }
            finally
            {
                io = null;
                pd = null;
            }
        }

        private string write_contents_to_file(StreamReader sr, string filename)
        {
            csFileIo io = new csFileIo();
            //StreamReader tempsr;

            //tempsr = sr;
            string workingfile = "";//this._temp_dir + @"\" /*+ this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\"*/ + filename;

            //workingfile = (this.FILE_FORMAT_DESC == "PYRL") ? this._temp_dir + @"\" + filename : this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" + filename;
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
        }

        private bool getFileInventrory()
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("sp_getInventoryByFilename", new string[] { this.FILE_NAME });
                if (dt != null && dt.Rows.Count > 0)
                {
                    this._file_type = Convert.ToInt32(dt.Rows[0]["FileType"]);
                    this._file_type_desc = dt.Rows[0]["FileTypeDesc"].ToString();
                    this._file_format = Convert.ToInt32(dt.Rows[0]["FileFormat"]);
                    this._file_format_desc = dt.Rows[0]["FileFormatDesc"].ToString();
                    this.loading_path = dt.Rows[0]["Destination"].ToString();
                    this._record_count = Convert.ToInt32(dt.Rows[0]["RecordCount"]);
                    this._hash_total = dt.Rows[0]["HashTotal"].ToString();
                    return true;
                }
            }
            catch (Exception e)
            { }
            finally
            {
                dt = null;
            }
            return false;
        }

        private void get_mapper_currency(string buff)
        {

        }

        public DataSet get_rate_type(DateTime _tranxdate, string _branchcode)
        {
            SqlCommand cmd = new SqlCommand();

            cmd = base.getSqlCommand("sp_get_rates_per_type", true);
            cmd.Parameters.Add("@v_trandate", SqlDbType.DateTime).Value = _tranxdate.ToString("MM/dd/yyyy");
            cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 20).Value = _branchcode;

            return getDataSet(cmd, "TBLRates");
        }
        public DataSet uploaderFileType()
        {
            return getDataSet(getSqlCommand("SELECT FldCode [CODE], FldValue [VALUE] FROM TBL_SUB_LOOKUP"
                        + " WHERE fldgroup = 'FileUploadType' AND BranchCode = '" + this._branch + "'", false));
        }

        public DataSet uploaderFileFormat(string fileType)
        {
            return getDataSet(getSqlCommand("SELECT FldCode [CODE], FldValue [VALUE] FROM TBL_SUB_LOOKUP"
                        + " WHERE fldgroup = '" + fileType + "' AND BranchCode = '" + this._branch + "'", false));
        }

        public DataTable show_uploaded_files(int userrole, string branchcode, bool isadmin)
        {
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_show_uploadedfiles", true);
                //cmd.Parameters.Add("@v_role", SqlDbType.Int).Value = userrole;
                cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 20).Value = branchcode;
                //cmd.Parameters.Add("@v_isadmin", SqlDbType.Int).Value = isadmin;

                return getDataSet(cmd, "TBLUPLOAD").Tables[0];
            }
            catch
            {
                return null;
            }
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand cmdUploader = getSqlCommand("SELECT * FROM TBL_FILE_INVENTORY WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(cmdUploader);
        }

        public bool savedToTemp()
        {
            csFileIo fio = new csFileIo();
            if (!fio.directoryExist(this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC))
            {
                fio.createDirectory(this._temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC);
            }
            try
            {
                if (!fio.exist(this._temp_dest))
                {

                    try
                    {

                        if (this.GetMimeType(this._temp_dest).IndexOf("text") > 0)
                        {
                            if (fio.open_output(this._temp_dest, (int)csFileIo.write_modes.CREATE))
                            {
                                fio.putLine(this.sbFileContent.ToString());
                                fio.close_output();
                            }
                            else
                            {
                                _message = "Upload not done. Please retry.";
                                return false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _message = "ERROR: " + e.Message;
                        return false;
                    }
                }
                return true;
                    //}
                //}
            }
            catch (Exception e)
            {
                _message = e.Message;
                this.writeAudit("savedToTemp - " + e.Message, csAuditLog.Category.Insert);
            }
            return false;
        }

        public bool updateInventory()
        {
            try
            {
                DataTable dt = getDataSet(getSqlCommand("SELECT * FROM TBL_FILE_INVENTORY WHERE FileName = '" + this.FILE_NAME + "'", false)).Tables[0];
                bool ok = false;
                SqlCommand cmd = base.getSqlCommand("sp_get_uploadedfiles", true);
                cmd.Parameters.Add("@v_filename", SqlDbType.VarChar, 150).Value = this.FILE_NAME;
                base.getUpdateDataSet(cmd);
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(dt.Rows[0][0].ToString()));
                base.addMode = false;

                ok = base.edit();

                //to do: insert, get hast total, record count.
                if (ok)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("ProcessedBy", this._user);
                    ht.Add("ProcessingBranch", this._branch);
                    ht.Add("rate_type", this._rate_type);
                    ht.Add("Status", getLookupValue("FileStatus", "APPROVED"));

                    foreach (Object key in ht.Keys)
                    {
                        setColumn(key.ToString(), ht[key]);
                    }

                    return base.save(sqlHelper.RETAIN_DATASET);
                }


            }
            catch (Exception e)
            {
                this.writeAudit("updateInventory - " + e.Message, csAuditLog.Category.Update);
                throw e;
            }

            MESSAGE = base.Message;
            return false;
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

        public void setParamVal(string totrec, string runamt, string rate_type)
        {
            try
            {
                //uprow["ProcessingBranch"] = "n/a";
                //uprow["ProcessedBy"] = "n/a";
                //uprow["DateProcessed"] = "/a";
                uprow["FileType"] = this._file_type;
                uprow["FileFormat"] = this._file_format;
                uprow["FileName"] = this._file_name;
                uprow["UploadingBranch"] = this.BRANCH;
                uprow["UploadedBy"] = this.USER;
                uprow["FileSize"] = this._file_size;
                uprow["DateUpdated"] = srvr_time;
                uprow["DateUploaded"] = srvr_time;
                uprow["DateUploadedLocal"] = br_time;
                uprow["IsDeleted"] = 0;
                uprow["Destination"] = this.loading_path;
                uprow["Status"] = this.getLookupValue("FileStatus", "UNPROCESSED");
                uprow["rate_type"] = rate_type;
                uprow["TotalNoOfRecords"] = Convert.ToInt32(totrec);
                uprow["HashTotal"] = runamt;

            }
            catch (Exception e)
            {
                _message = e.Message;
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
                    select = "SELECT COUNT(1) FROM TBL_FILE_INVENTORY "
                            + "WHERE [FileName] = '" + this._file_name + "'";
                else
                    select = "SELECT COUNT(1) FROM TBL_FILE_INVENTORY "
                            + "WHERE Status = 0 AND [FileName] = '" + this._file_name + "'";

                if (Convert.ToBoolean(execScalar(select)))
                {
                    _message = "File has been previously uploaded. Please select another file.";
                    return true;
                }
                else
                {
                    this._temp_dest = _temp_dir + @"\" + this.FILE_TYPE_DESC + @"\" + this.FILE_FORMAT_DESC + @"\" + this._file_name;
                    return false;
                }
            }
            catch (Exception e)
            {
                _message = Message;
                this.writeAudit(e.Message + " - fileexists", csAuditLog.Category.Insert);
                return false;
            }
        }

        public bool prepare_records()
        {
            DataTable dt = null;
            object[] param = null;
            try
            {
                irsTimeZone itz = new irsTimeZone();
                br_time = itz.branchLocalTime(this.BRANCH);
                srvr_time = itz.ServerTime;

                param = new object[] { this._file_type, this._file_format };
                dt = new irsSqlCommons().execStoredProcedureTable("sp_uploader_getfilepath", param);

                if (dt != null && dt.Rows.Count > 0)
                {
                    this._file_format_desc = dt.Rows[0][1].ToString().Trim();
                    this._file_type_desc = dt.Rows[0][0].ToString().Trim();
                    string dir_file_type = new irsConfiguration().appsetting(this._file_type_desc + "Directory");

                    if (dir_file_type != null && dir_file_type != string.Empty)
                    {
                        this.loading_path = string.Empty;
                        this.loading_path = dir_file_type + @"\" + this._file_name;
                    }
                    else
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (this._file_format_desc != "" && this._file_format_desc != null)
                            {
                                this.loading_path = new irsConfiguration().appsetting("RootPath");
                                this.loading_path += this._file_type_desc + @"\"
                                                    + this._file_format_desc;
                            }
                            else
                            {
                                this.loading_path = new irsConfiguration().appsetting("RootPath");
                                this.loading_path += this._file_type_desc;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                _message = e.Message;
                this.writeAudit(e.Message + " - prepare_records", csAuditLog.Category.Insert);
                return false;
            }
            finally
            {
                dt = null;
                param = null;
            }
        }

        public void moveToLoadingDirectory(string fn)
        {
            if (this.getFileInventrory())
            {
                if (new csFileIo().exist(this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + fn))
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
                    throw new Exception("The file has been previously approved. \n\nPlease wait until the file is completely processed.");
                }

                this.moveToLoadingDirectory();
            }
        }

        public void moveToLoadingDirectory()
        {
            csFileIo fio = new csFileIo();
            csFtp ftp = new csFtp(new irsConfiguration().appsetting("FTPConnectionString"));
            string nl = Environment.NewLine;

            try
            {
                StringBuilder sb_file = new StringBuilder();

                if (ftp.connect())
                {
                    //ftp.mkdir(this.loading_path.Replace("/", @"\"));
                    //ftp.cwd(this.loading_path);
                    //this.loading_path = this.loading_path.Replace(@"\", "/");
                    string[] dir = this.loading_path.Replace(@"\", "/").Split('/');
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
                        MESSAGE = "The file " + this._file_name + " will now be processed for remittance.";
                        this.writeAudit("FTP Successful - " + ftp.Message + " : " + MESSAGE, csAuditLog.Category.Insert);
                    }
                    else
                    {
                        MESSAGE = "ERROR: Cannot proceed to your request. " + ftp.Message;
                    }
                }
                else
                {
                    MESSAGE = "ERROR: Ftp failed - " + ftp.Message;
                }
            }
            catch (Exception e)
            {
                MESSAGE = e.Message;
                throw new Exception(e.Message);
            }
            finally
            {
                ftp.disconnect();
                if (MESSAGE.Contains("ERROR"))
                {
                    this.writeAudit("FTP ERROR: " + MESSAGE, csAuditLog.Category.Insert);
                }
                fio.delete(this._temp_dest);
            }
            this.uploadMailer();
        }

        public void moveToTempLoadingDirectory()
        {
            MESSAGE = "The file " + this._file_name + " has been successfully uploaded";
            this.uploadMailer();
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
                _message = e.Message;
            }
            finally
            {
                param = null;
                dt = null;
            }
            return 0;
        }

        /*FOR SWIFT RECORDS         */
        private Object[] createResponse(swift_header hdr)
        {
            String[] st_names = { "FName", "FSize", "TotalItems", "TotalAmt", "ProcItems", "ProcAmt", "UnprocItems", "UnprocAmt" };
            String[] items = new String[st_names.Length + (hdr.currtab.Count * 2)];
            String[] names = new String[items.Length];

            items[0] = hdr.Filename;
            items[1] = hdr.FileSize.ToString("#,###,###,##0");
            items[2] = hdr.total_items.ToString("###,##0");
            items[3] = hdr.total_amount.ToString("##,###,###,###.00");
            items[4] = hdr.processed_items.ToString("###,##0");
            items[5] = hdr.processed_amount.ToString("##,###,###,###.00");
            int unproc_items = hdr.total_items - hdr.processed_items;
            items[6] = unproc_items.ToString("###,##0");
            decimal unproc_amt = hdr.total_amount - hdr.processed_amount;
            items[7] = unproc_amt.ToString("##,###,###,###.00");

            int i = 0;
            for (; i < st_names.Length; i++)
            {
                names[i] = st_names[i];
            }

            ICollection keys = hdr.currtab.Keys;
            foreach (Object currcd in keys)
            {
                names[i] = currcd.ToString().ToUpper();
                names[i + 1] = names[i] + "_AMOUNT";
                items[i] = ((int)hdr.itemtab[currcd]).ToString("###,##0");
                items[i + 1] = ((decimal)hdr.currtab[currcd]).ToString("##,###,###,###.00");
                i += 2;
            }

            return new Object[] { names, items, hdr.Path };
        }

        private void add_record(string currency, int noofitems, double runamt)
        {
            file_items fi = new file_items(currency, noofitems, runamt);
            this.validate_entry(fi, currency);
        }

        private bool validARS()
        {
            IBATCH_Class.ARSValidation val = new IBATCH_Class.ARSValidation(this._file_name);
            if (!val.validARSFile())
            {
                this._message = "Rejected!\n\nReason: Header amount and total record amount are not equal.";
                return false;
            }
            return true;
        }

        private void validate_entry(file_items fi, string curr_key)
        {
            try
            {

                if (trans_items.ContainsKey(curr_key))
                {
                    file_items temp_fi = (file_items)trans_items[curr_key];
                    temp_fi.noofitems += fi.noofitems;
                    temp_fi.running_amount += fi.running_amount;
                }
                else
                {
                    trans_items.Add(fi, curr_key);
                }
            }
            catch (Exception e)
            {
                if (fi != null)
                    trans_items.Add(fi, curr_key);
                //this.writeuploadlogs(e.Message + "validate entry");
            }

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
                _message = e.Message;
            }

            return "";
        }

        public string get_file_content(string fn)
        {
            string content = "";
            this._file_name = fn;

            csFileIo nio = new csFileIo();

            if (getFileInventrory())
            {
                string f_dir = (fn.Contains(":")) ? fn : this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + fn;
                if (nio.exist(f_dir))
                {
                    if (nio.open_input(f_dir))
                    {
                        while (!nio.EOF)
                        {
                            content += nio.readLine() + Environment.NewLine;
                        }
                    }
                }
            }

            nio = null;
            return content;
        }

        public void writeAudit(string Description, csAuditLog.Category Category)
        {
            new csAuditLog().writeAudit("", "File Loader", Description, "", "", Category);
        }

        public string emailProcessed(string fileName, string hashTotal, int noOfItems, int noOfProcessed, int noOfRejected)
        {
            /*
             * Dear Partner,

                Your file received by Philippine National Bank has been successfully processed and accounts have been credited. Below are the details of the file:

                1. File Name       : U780_PNB ARS 01-25-11 B2reup_110125080057.TXT
                2. Hash Totals     : 1874369.73
                3. Number of Items : 129
                4. Processed       : 125
                5. Rejected        : 4

                Remarks: Please download the file U780_PNB ARS 01-25-11 B2reup_110125080057_BatchReport.txt

                For inquiries please contact Elise Rue G. Alegre - Ramos at (632) 526 3131 ext. 4463

                Thank you for using WEB PORTAL.

                Philippine National Bank


                ******This is a system generated email******
             */
            StringBuilder sbEmail = new StringBuilder();
            string nl = Environment.NewLine;
            string tab = "\t";
            sbEmail.Append("Dear Partner," + nl + nl);
            sbEmail.Append("Your file received by Philippine National Bank has been successfully processed and accounts have been credited. Below are the details of the file:" + nl + nl);
            sbEmail.Append(this.fileSummary(fileName, hashTotal, noOfItems, noOfProcessed, noOfRejected) + nl);
            sbEmail.Append("Remarks: Please download the file " + fileName + nl + nl);
            sbEmail.Append("For inquiries please contact " + this.agentContact() + nl + nl);
            sbEmail.Append("Thank you for using IRSWORLD." + nl + nl);
            sbEmail.Append("Philippine National Bank" + nl + nl);
            sbEmail.Append("*******This is a system generated email.*******");
            return sbEmail.ToString();
        }

        private string emailUpload()
        {
            return this.emailUpload(this._file_name, this._hash_total, this._record_count);
        }

        private string emailUpload(string fileName, string hashTotal, int noOfItems)
        {
            //Dear Partner,

            //The file that you have uploaded has been received by Philippine National Bank for processing. Below are the details of the file:

            //1. File Name       : I000_I000-110125-09_110125073430.TXT
            //2. Hash Totals     : 22986
            //3. Number of Items : 1

            //Another email will be sent as soon as the file has been processed and accounts are credited.

            //For inquiries please contact Elise Rue G. Alegre - Ramos at (632) 526 3131 ext. 4463

            //Thank you for using WEB PORTAL.

            //Philippine National Bank


            //******This is a system generated email******

            StringBuilder sbEmail = new StringBuilder();
            string nl = Environment.NewLine;
            string tab = "\t";
            sbEmail.Append("Dear Partner," + nl + nl);
            sbEmail.Append("The file that you have uploaded has been received by Philippine National Bank for processing. Below are the details of the file:" + nl + nl);
            sbEmail.Append(this.fileSummary(fileName, hashTotal, noOfItems) + nl);
            sbEmail.Append("Another email will be sent as soon as the file has been processed and accounts are credited. " + nl + nl);
            sbEmail.Append("For inquiries please contact " + this.agentContact() + nl + nl);
            sbEmail.Append("Thank you for using IRSWORLD." + nl + nl);
            sbEmail.Append("Philippine National Bank" + nl + nl);
            sbEmail.Append("*******This is a system generated email.*******");
            return sbEmail.ToString();
        }

        public bool checkRates()
        {
            return this.checkRates(this._branch);
        }

        public bool checkRates(int rateType)
        {
            return this.checkRates(this._branch, rateType);
        }

        public bool checkRates(string br)
        {
            irsCurrencyRates cr = new irsCurrencyRates();
            DateTime date = new irsTimeZone().branchLocalTime(br);

            try
            {
                return cr.isComplete(br, date.Date);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool checkRates(string br, int rateType)
        {
            irsCurrencyRates cr = new irsCurrencyRates();
            DateTime date = new irsTimeZone().branchLocalTime(br);

            try
            {
                return cr.isComplete(br, date.Date, rateType);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string fileSummary(string fileName, string hashTotal, int noOfItems, int noOfProcessed)
        {
            return this.fileSummary(fileName, hashTotal, noOfItems, noOfProcessed, -1);
        }

        public string fileSummary(string fileName, string hashTotal, int noOfItems)
        {
            return this.fileSummary(fileName, hashTotal, noOfItems, -1, -1);
        }

        public string fileSummary()
        {
            return this.fileSummary(this._file_name, this._hash_total, this._record_count);
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

        public irsUploadFile(string br, string user, int file_type, int file_format, string filename, string rate_type)
        {
            temp_dir();
            this.fio = new csFileIo();
            this.TableName = "TBL_FILE_INVENTORY";
            this.IdentityColumn = "SEQ_ID";
            this._message = string.Empty;
            this._branch = br;
            this._user = user;
            this._file_type = file_type;
            this._file_format = file_format;
            this._file_name = filename;
            this._rate_type = rate_type;
            //this.get_filetypeformatdesc();
            this.prepare_records();
        }

        public irsUploadFile(string br)
        {
            this._branch = br;
        }

        public irsUploadFile()
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
            
            this.fio = new csFileIo();
            this.TableName = "TBL_FILE_INVENTORY";
            this.IdentityColumn = "SEQ_ID";
            this._message = string.Empty;
        }
        #endregion
    }
}