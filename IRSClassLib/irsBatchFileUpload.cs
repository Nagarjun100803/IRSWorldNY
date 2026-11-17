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
    public class irsBatchFileUpload : sqlHelperWs
    {
        private DateTime _br_time;
        private DateTime _srvr_time;
        private StringBuilder _sb_file;
        private Hashtable _currency_breakdown;
        private string _temp_dir;
        private string _source_path;
        private string _loading_path;
        private string _temp_dest;
        private string _file_name;
        private string _branch;
        private string _user;
        private string _file_type_desc;
        private string _file_format_desc;
        private string _rate_type = "-1";
        private string _message = string.Empty;
        private string _amount_summary;
        private double _hash_total;
        private long _file_size;
        private int _file_type;
        private int _file_format;
        private int _seq_id;
        private int _record_count;
        private int _line_count;
        private bool _isInsufficientFund;

        #region PROPERTIES
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

        public bool IsInsufficientFund
        {
            get { return this._isInsufficientFund; }
        }

        public string MESSAGE
        {
            get { return this._message; }
            set { this._message = ""; }
        }

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

        public bool ValidUploadCriteria
        {
            get { return validUploadCriteria(); }
        }

        public string TempDest
        {
            get { return this._temp_dir + @"\" + this._file_name; }
        }
        #endregion

        #region PRIVATE FUNCTIONS
        //private void isInsufficientAmount(string accounts, string amount)
        //{
        //    Hashtable ht = null;
        //    string[] account = accounts.Split('|');
        //    string[] amt = amount.Split('~');

        //    for (int ac_ndx = 0; ac_ndx < account.Length; ac_ndx++)
        //    {
        //        string a_cur = account[ac_ndx].Split('~')[0];
        //        string acct = account[ac_ndx].Split('~')[1];

        //        for (int am_ndx = 0; am_ndx < amt.Length; am_ndx++)
        //        {
        //            string f_cur = amt[am_ndx].Split('~')[0];
        //            double val = Convert.ToDouble(amt[am_ndx].Split('~')[1]);

        //            if (a_cur.ToUpper() == f_cur.ToUpper()) isInsufficientAmount(acct, val);
        //        }
        //    }
        //}

        //private void isInsufficientAmount(string account, double amt)
        //{
        //    string response_out = "";
        //    Hashtable ht = null;
        //    csFlexcube flx = new csFlexcube();
        //    string nl = Environment.NewLine;

        //    try
        //    {
        //        ht = flx.getFcAcctDetails(account, out response_out);
        //        double actual_funds = Convert.ToDouble(ht["ACCOUNTFUNDING"]);
        //        string cur = ht["CURRENCYSHORTNAME"].ToString();
        //        this._isInsufficientFund = (actual_funds > amt) ? false : true;

        //        if (this._isInsufficientFund) 
        //            this._message += "Insufficient fund - Currency:" + cur + " Fund:" + actual_funds.ToString() + " RemittedAmt:" + amt.ToString() + nl;
        //    }
        //    catch (Exception e) { throw e; }
        //    finally
        //    {
        //        ht.Clear();
        //        ht = null;
        //        flx = null;
        //    }
        //}
        //csFlexcube flx = new csFlexcube();
        //Hashtable ht = flx.getFcAcctDetails(account_no, out response_out);

        private void sendToFTP()
        {
            string ftpstring = new irsConfiguration().FTPConnectionString;
            csFtp ftp = new csFtp(ftpstring);
            string ftpTemp = "temp";
            try
            {
                if (ftp.connect())
                {
                    //if (ftp.binary(true))
                    //{
                    if (!ftp.cwd(ftpTemp))
                    {
                        if (!ftp.mkdir(ftpTemp))
                        {
                            _message = ftp.Message;
                        }
                        if (!ftp.cwd(ftpTemp))
                        {
                            _message = ftp.Message;
                        }
                    }
                    else
                    {
                        _message = ftp.Message;
                    }

                    if (ftp.put(this.TempDest))
                    {
                        this._message = "";
                        return;
                    }
                    //}
                }

                _message = ftp.Message;
            }
            catch (Exception e)
            {
                _message = "ERROR: " + e.Message;
            }
            finally
            {
                ftp.disconnect();
                ftp = null;
                deleteTempFile();
                if (_message != "")
                {
                    writeAudit(_message, csAuditLog.Category.FTP);
                }
                //deleteTempTable();
            }
        }

        private void deleteTempTable()
        {
            string query = "delete tbl_ibatch_tempfile where text_filename = '" + this._file_name + "'";
            try
            {
                int i = sqlUpdate(query);
                if (i < 1)
                {
                    writeAudit("deleteTempTable - " + Message, csAuditLog.Category.Delete);
                }
            }
            catch (Exception x)
            {
                writeAudit("deleteTempTable - " + x.Message, csAuditLog.Category.Delete);
            }
        }

        private bool validUploadCriteria()
        {
            if (this._file_type < 1)
            {
                _message = "Please select file type.";
                return false;
            }
            else if (this._file_format < 1)
            {
                _message = "Please select file format";
                return false;
            }
            if (this._file_type == 3 | this._file_type_desc.ToUpper().Contains("IBATCH"))
            {
                if (this._rate_type == "-1")
                {
                    _message = "Please select rate type";
                    return false;
                }
            }

            return true;
        }

        private string getFileName(string fn)
        {
            if (this._file_type_desc.ToUpper().Contains("COMPLIANCE"))
            {
                return this._branch + "_" + DateTime.Now.ToString("MMddyyyy") + "_" + fn;
            }

            return fn;
        }

        private string agentContact()
        {
            return getLookupDesc("AgentContact", getLookupValue("AgentContact", "GMG"));
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

        private bool validARS()
        {
            IBATCH_Class.ARSValidation val = new IBATCH_Class.ARSValidation(this._file_name);
            if (!val.validARSFile(this._file_format_desc))
            {
                // Updated by Hajz : 10/09/2012
                // Update Description : Wrong value supplied to variable "this._message"
                //if (val.ErrorMessage.IndexOf("ERROR") > -1) this._message = val.Message;//"ERROR: File " + this._file_name + " is rejected!\n\nReason - Header amount and total record amount are not equal.";
                if (val.ErrorMessage.IndexOf("ERROR") > -1) this._message = val.ErrorMessage;
                else this._message = "Unable to convert. The file " + this._file_name + " is not intended for " + this._file_format_desc + " loading.";
                return false;
            }

            return true;
        }

        private void decrypt_pyrlfile()
        {
            csFileIo io = new csFileIo();
            pyrl_decryptor pd = new pyrl_decryptor();

            try
            {
                string workingfile = this._temp_dir + @"\" + this._file_name;
                string newpath = this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc + @"\" + @"\PYRLDECODE\";
                if (!io.directoryExist(newpath)) io.createDirectory(newpath);
                string newworkingfile = newpath + this._file_name;


                String scontents = pd.Decrypt(io.readAllText(workingfile));

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

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private bool saveUploaded()
        {
            string tempsource = _temp_dir + @"\" + this._file_name;
            string tempfolder = _temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc;
            string tempfilenamepath = tempfolder + @"\" + this._file_name;
            csFileIo io = new csFileIo();

            try
            {
                if (!io.directoryExist(tempfolder)) io.createDirectory(tempfolder);
                if (io.directoryExist(tempsource))
                {
                    if (io.copy(tempsource, tempfilenamepath, true))
                    {
                        this._file_size = io.getFileSize(tempfolder);
                        _message = "";
                        return true;
                    }
                }

                _message = io.Message;
            }
            catch (Exception e)
            {
                _message = "ERROR: " + e.Message;
            }
            finally
            {
                io.delete(tempsource);
                io = null;
                this.GetMimeType(tempfilenamepath);
            }

            writeAudit(_message + " : " + tempfilenamepath, csAuditLog.Category.File);
            return false;
        }

        private bool saveUploaded(System.Web.UI.HtmlControls.HtmlInputFile file)
        {
            string tempfolder = _temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc;
            string tempfilenamepath = tempfolder + @"\" + this._file_name;

            try
            {
                csFileIo io = new csFileIo();

                if (!io.directoryExist(tempfolder)) io.createDirectory(tempfolder);
                io.delete(_temp_dir + @"\" + this._file_name);
                file.PostedFile.SaveAs(_temp_dir + @"\" + this._file_name);
                _message = "";
                return true;
            }
            catch (Exception e)
            {
                _message = "ERROR: " + e.Message;
            }
            finally
            {
                this.GetMimeType(tempfilenamepath);
            }

            writeAudit(_message + " : " + tempfilenamepath, csAuditLog.Category.File);
            return false;

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

        private void setOtherVariables()
        {
            this.prepare_records();
        }

        #endregion

        #region PUBLIC FUNCTIONS
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

        public void readFile()
        {
            this._file_format_desc = this.getLookupCode(this._file_type_desc, this._file_format);
            irsBatchUpload bu = new irsBatchUpload(this._file_format, this._file_type, this._file_name, this._rate_type);
            string fn = this._file_name;
            System.IO.StreamReader sr = null;
            ArrayList line_error_msg = new ArrayList();
            this._file_size = new csFileIo().getFileSize(this.TempDest);
            double runamount = 0;
            Hashtable ht_pay_details = new Hashtable();
            csFileIo fio = new csFileIo();
            int record_count = 0;
            int line_count = 0;
            int line_errors = 0;

            this._currency_breakdown = new Hashtable();
            this._sb_file = new StringBuilder();

            try
            {
                if (this._file_type_desc.ToUpper() == "ARS" || this._file_type_desc.ToUpper() == "EXCO")
                {
                    if (!this.validARS())
                    {
                        return;
                    }

                    try
                    {
                        sr.Close();
                        sr.Dispose();
                    }
                    catch { }
                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                }
                else if (this._file_type_desc.ToUpper() == "IBATCH")
                {
                    sr = new StreamReader(this._temp_dir + @"\" + fn);
                    if (this._file_format == 4) //SWIFT Directory
                    {
                        string s = write_contents_to_file(sr, this._file_name);
                        if (s.Trim().Length == 0)
                            bu.decode_replace_swift();
                        if (bu.ErrorMessage.Length == 0)
                        {
                            try
                            {
                                sr.Close();
                                sr.Dispose();
                            }
                            catch { }
                            sr = new StreamReader(this._temp_dir + @"\IBatch\SWFT\" + this._file_name);
                        }
                    }
                    if (this._file_format_desc.ToUpper() == "PYRL") //Payroll Directory
                    {
                        string s = write_contents_to_file(sr, this._file_name);
                        if (s.Trim().Length == 0) this.decrypt_pyrlfile();

                        try
                        {
                            sr.Close();
                            sr.Dispose();
                        }
                        catch { }
                        sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                    }
                }
                else
                {
                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                }
                if (!bu.clean_tempfile())
                {
                    throw new Exception("ERROR: DELETING DATA");
                }

                if (sr != null)
                {
                    while (true)
                    {
                        string buff = sr.ReadLine();

                        if (buff == null || buff.Trim().Length < 1)
                        {
                            int i = 0;
                            foreach (object o in ht_pay_details.Keys)
                            {
                                _amount_summary += ht_pay_details[o];
                                if (ht_pay_details.Count > 1)
                                {
                                    if ((i + 1) < ht_pay_details.Count)
                                    {
                                        _amount_summary += "|";
                                    }
                                }
                                i++;
                            }
                            break;
                        }
                        line_count++;
                        this._sb_file.Append(buff + Environment.NewLine);

                        if (buff.Trim().Length >= 3)
                        {
                            if (buff.Contains("HDR") | buff.Substring(0, 3) == "TRL") continue;
                            else record_count++;
                        }
                        else
                        {
                            record_count++;
                        }

                        if (this._file_type_desc.ToUpper() != "IRSCOMPLIANCE") // exclude compliance, doesn't need amount summary
                        {
                            try
                            {
                                if (this._file_type_desc.ToUpper() == "IBATCH") //Amount summary handling for Ibatch
                                {
                                    if (bu.write_data_to_tempfile(buff, this._branch))
                                    {
                                        if ((this._file_format_desc.ToUpper() != "ERPS") | (buff.ToUpper().StartsWith("GANB") | buff.ToUpper().StartsWith("ANB")))
                                        {
                                            runamount += (Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount")) / 100);
                                        }
                                        else
                                        {
                                            runamount += (Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount2")) / 100);
                                        }
                                    }
                                    else
                                    {
                                        line_error_msg.Add(bu.ErrorMessage);
                                        line_errors++;
                                    }
                                }
                                else if (this._file_type_desc == "ARS" | this._file_type_desc == "EXCO") //Amount handling for ARS/EXCO
                                {
                                    IBATCH_Class.ARSValidation ars = new IRSClassLib.IBATCH_Class.ARSValidation();
                                    runamount += Convert.ToDouble(ars.getFieldValue(buff, IRSClassLib.IBATCH_Class.ARSValidation.ARSField.Amount));

                                    string cur = ars.getFieldValue(buff, IRSClassLib.IBATCH_Class.ARSValidation.ARSField.RemittingCurrency);
                                    string amt = ars.getFieldValue(buff, IRSClassLib.IBATCH_Class.ARSValidation.ARSField.Amount);
                                    if (!ht_pay_details.ContainsKey(cur)) ht_pay_details.Add(cur, cur + "~" + amt + "~1");
                                    else
                                    {
                                        string[] val = ht_pay_details[cur].ToString().Split('~');
                                        ht_pay_details[cur] = cur + "~" + Convert.ToString((Convert.ToDouble(val[1]) + Convert.ToDouble(amt))) + "~" + Convert.ToString((Convert.ToInt16(val[2]) + 1));
                                    }
                                }
                                else if (this._file_type_desc.ToString().ToUpper() == "EXCEL")
                                {
                                    if (this._file_format_desc.Trim().ToUpper() == "ANB")
                                    {
                                        irsEXCO_ANB_Approval anb = new irsEXCO_ANB_Approval();
                                        try
                                        {
                                            if (!anb.read_header(buff))
                                            { runamount += Convert.ToDouble(anb.read_amount(buff)); }
                                            else
                                            {
                                                record_count = record_count - 1;
                                                line_count = line_count - 1;
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            runamount += 0;
                                        }
                                        finally
                                        {
                                            anb = null;
                                        }

                                    }
                                    else
                                    {
                                        runamount += (Convert.ToDouble(bu.get_mapper_and_contents(buff, "Amount")) / 100);
                                    }
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
                    }
                }

                this._hash_total = runamount;
                //handling of ANB file


                this._record_count = record_count;
                this._line_count = line_count;
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
            }
        }

        //private bool ibatchFunction()
        //{
        //    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
        //    if (this._file_format == 4) //SWIFT Directory
        //    {
        //        string s = write_contents_to_file(sr, this._file_name);
        //        if (s.Trim().Length == 0)
        //            bu.decode_replace_swift();
        //        if (bu.ErrorMessage.Length == 0) return true;
        //            //sr = new StreamReader(this._temp_dir + @"\IBatch\SWFT\" + this._file_name);
        //    }
        //    if (this._file_format == 1) //Payroll Directory
        //    {
        //        string s = write_contents_to_file(sr, this._file_name);
        //        if (s.Trim().Length == 0)
        //        {
        //            this.decrypt_pyrlfile();
        //        }
        //        return true;
        //        //sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
        //    }

        //    return false;
        //}

        public void readFile(string path)
        {
            string fn = this._file_name = new csFileIo().getFilename(path);
            irsBatchUpload bu = new irsBatchUpload(this._file_format, this._file_type, this._file_name, this._rate_type);
            System.IO.StreamReader sr = null;// = new StreamReader(path);
            ArrayList line_error_msg = new ArrayList();
            double runamount = 0;
            csFileIo fio = new csFileIo();
            int record_count = 0;
            int line_count = 0;
            int line_errors = 0;

            this._currency_breakdown = new Hashtable();
            this._sb_file = new StringBuilder();
            this._file_name = fn;

            try
            {
                if (this._file_type_desc.ToUpper() == "ARS")
                {
                    if (!this.validARS())
                    {
                        return;
                    }

                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                }
                else if (this._file_type == 3)
                {
                    sr = new StreamReader(this._temp_dir + @"\" + fn);
                    if (this._file_format == 4) //SWIFT Directory
                    {
                        string s = write_contents_to_file(sr, this._file_name);
                        if (s.Trim().Length == 0)
                            bu.decode_replace_swift();
                        if (bu.ErrorMessage.Length == 0)
                            sr = new StreamReader(this._temp_dir + @"\IBatch\SWFT\" + this._file_name);
                    }
                    if (this._file_format == 1) //Payroll Directory
                    {
                        string s = write_contents_to_file(sr, this._file_name);
                        if (s.Trim().Length == 0)
                            this.decrypt_pyrlfile();
                        sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                    }
                }
                else if (this._file_type_desc.ToUpper() == "IRWORLD")
                {

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
                    this._sb_file.Append(buff + Environment.NewLine);


                    if (buff.Contains("HDR") | buff.Contains("TRL")) continue;
                    else record_count++;

                    if (this._file_type_desc.ToUpper() == "IBATCH" | this._file_type_desc == "ARS") // exclude compliance
                    {
                        if (bu.write_data_to_tempfile(buff, this._branch))
                        {
                            try
                            {
                                if (this._file_type_desc.ToUpper() == "IBATCH")
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

        public void readFile(System.Web.UI.HtmlControls.HtmlInputFile file)
        {
            this._currency_breakdown = new Hashtable();
            this._sb_file = new StringBuilder();
            csFileIo fio = new csFileIo();
            System.IO.StreamReader sr = new System.IO.StreamReader(file.PostedFile.InputStream);
            this._file_size = file.PostedFile.ContentLength;
            int record_count = 0;
            int line_count = 0;
            int line_errors = 0;
            ArrayList line_error_msg = new ArrayList();
            double runamount = 0;
            this._source_path = file.PostedFile.FileName;
            this._file_name = this.getFileName(fio.getFilename(this._source_path));

            try
            {
                if (!this.saveUploaded(file))
                {
                    return;
                }

                if (this._file_type_desc.ToUpper() == "ARS")
                {
                    if (!this.validARS())
                    {
                        return;
                    }

                    sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
                }

                irsBatchUpload bu = new irsBatchUpload(this._file_format, this._file_type, this._file_name, this._rate_type);
                if (this._file_type == 3)
                {
                    if (this._file_format == 4)
                    {
                        string s = write_contents_to_file(sr, this._file_name);
                        if (s.Trim().Length == 0)
                            bu.decode_replace_swift();
                        if (bu.ErrorMessage.Length == 0)
                            sr = new StreamReader(this._temp_dir + @"\IBatch\SWFT\" + this._file_name);
                    }
                    if (this._file_format == 1)
                    {
                        string s = write_contents_to_file(sr, this._file_name);
                        if (s.Trim().Length == 0)
                            this.decrypt_pyrlfile();
                        sr = new StreamReader(this._temp_dir + @"\" + this._file_name);
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
                    this._sb_file.Append(buff + Environment.NewLine);


                    if (buff.Contains("HDR") | buff.Contains("TRL")) continue;
                    else record_count++;

                    if (this._file_type_desc.ToUpper() == "IBATCH" | this._file_type_desc == "ARS") // exclude compliance
                    {
                        if (bu.write_data_to_tempfile(buff, this._branch))
                        {
                            try
                            {
                                if (this._file_type_desc.ToUpper() == "IBATCH")
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

        public bool checkRates(char type)
        {
            return (type != 'C') ? this.checkRates(this._branch) : true;
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
                return this.checkRates(br, -1);
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
                object o = execScalar("SP_CHECK_RATES", new object[] { br, date, rateType });
                return (Convert.ToInt32(o) > 0);
            }
            catch (Exception e)
            {
                throw e;
            }
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

        public void writeAudit(string Description, csAuditLog.Category Category)
        {
            new csAuditLog().writeAudit("", "File Loader", Description, "", "", Category);
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
                    select = "exec sp_get_compliance_status " + _file_format + ", '" + _file_name + "', '" + this._branch + "'";
                    //select = "SELECT COUNT(1) FROM TBL_FILE_INVENTORY "
                    //        + "WHERE FileFormat = " + _file_format + " " 
                    //        + "AND FileName = '" + _file_name + "' " 
                    //        + "AND UploadingBranch = '" + _branch + "' " 
                    //        + "AND Status != 1"; //Status = 0 AND [FileName] = '" + this._file_name + "'";                            
                }

                if (Convert.ToBoolean(execScalar(select)))
                {
                    if (!isCompliance)
                    {
                        this._message = "File has been previously uploaded. Please select another file.";
                    }
                    else
                    {
                        this._message = "Server is still processing the same type of file. Please wait.";
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
                //        //if (ftp.binary(true))
                //        //{
                //            if (ftp.cwd("temp"))
                //            {
                //                if (ftp.get(this._file_name, this.TempDest) > -1)
                //                {
                //                    this._message = "";
                //                    return true;
                //                }
                //            }
                //        //}
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

        public bool savedToTemp(string s)
        {
            csFileIo fio = new csFileIo();
            if (!fio.directoryExist(this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc))
            {
                fio.createDirectory(this._temp_dir + @"\" + this._file_type_desc + @"\" + this._file_format_desc);
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
                                fio.putLine(this._sb_file.ToString());
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
            }
            catch (Exception e)
            {
                _message = "ERROR: " + e.Message;
                this.writeAudit("savedToTemp - " + e.Message, csAuditLog.Category.Insert);
            }
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
                else
                {
                    content = this.getFileFTP();
                }
            }

            nio = null;
            return content;
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

        public void moveToTempLoadingDirectory()
        {
            this._message = "The file " + this._file_name + " has been successfully uploaded";
        }

        public string sequence()
        {
            string seq = "";//getLookupCode("SequenceNumber", 1);

            try
            {
                int i = sqlUpdate(getSqlCommand("SP_UPDATE_SEQUENCE"));
                seq = getLookupCode("SequenceNumber", 1);
            }
            catch { }

            return getLeftPadded(seq, "0", 3);
        }

        private string getFileFTP()
        {
            csFtp ftp = new csFtp(new irsConfiguration().FTPConnectionString);

            try
            {
                if (ftp.connect())
                {
                    //ftp.begin_download(_temp_dir, 
                }
            }
            catch (Exception e)
            {
                this._message = "ERROR" + e.Message;
            }
            finally
            {
                ftp.disconnect();
                ftp = null;
            }

            return "";
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
            DataTable dt;

            try
            {
                dt = execStoredProcedureTable("SP_ARS_EMAIL", new object[] { "Processed" });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i][0].ToString())
                    {
                        case "MSG2":
                            if (noOfProcessed > 0 & noOfRejected < 1)
                            {
                                sbEmail.Append(dt.Rows[i][1].ToString() + nl + nl);
                            } break;
                        case "MSG3":
                            if (noOfProcessed > 0 & noOfRejected > 0)
                            {
                                sbEmail.Append(dt.Rows[i][1].ToString() + nl + nl);
                            } break;
                        case "MSG4":
                            if (noOfRejected < 1 & noOfRejected < 1)
                            {
                                sbEmail.Append(dt.Rows[i][1].ToString() + nl + nl);
                            } break;
                        default: sbEmail.Append(dt.Rows[i][1].ToString() + nl + nl); break;
                    }
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
                            case "<DETAIL>": sbEmail.Replace(container, this.fileSummary(fileName, hashTotal, noOfItems, noOfProcessed, noOfRejected)); break;
                            default: break;
                        }
                    }
                    else break;
                }
            }

            sbEmail.Append("*******This is a system generated email.*******");
            return sbEmail.ToString();
        }

        private string emailUpload()
        {
            return this.emailUpload(this._file_name, Math.Round(this._hash_total, 2).ToString("###,###,###.##"), this._record_count);
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

        public void moveToLoadingDirectory(string fn)
        {
            if (this.getFileInventrory())
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

        private bool getFileInventrory()
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
                    //if (ftp.binary(true))
                    //{
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
                    //}
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
                this.uploadMailer();
                this.deleteTempTable();
                this.deleteTempFile();
            }
        }

        private void getErrorDB()
        {
            this._message = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
                this._message = base.Message;
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
            return this.fileSummary(this._file_name, this._hash_total.ToString("### ### ### ###"), this._record_count);
        }

        public string fileSummary(string fileName, string hashTotal, int noOfItems, int noOfProcessed, int noOfRejected)
        {
            string nl = Environment.NewLine;
            StringBuilder sbEmail = new StringBuilder();
            sbEmail.Append(nl + this.getPadded("File Name", " ", 16, false) + " : " + fileName + nl);
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

        public DataSet get_rate_type(DateTime _tranxdate, string _branchcode)
        {
            SqlCommand cmd = new SqlCommand();

            cmd = base.getSqlCommand("sp_get_rates_per_type", true);
            cmd.Parameters.Add("@v_trandate", SqlDbType.DateTime).Value = _tranxdate.ToString("MM/dd/yyyy");
            cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 20).Value = _branchcode;

            return getDataSet(cmd, "TBLRates");
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

        public bool updateInventory(String status)
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
                    ht.Add("Status", getLookupValue("FileStatus", status));
                    ht.Add("DateApproved", this._br_time);

                    foreach (Object key in ht.Keys)
                    {
                        setColumn(key.ToString(), ht[key]);
                    }

                    if (base.save(sqlHelperWs.RETAIN_DATASET))
                    {
                        this.writeAudit("updateInventory: " + this._file_name + " - " + status, csAuditLog.Category.Update);
                        return true;
                    }
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

                    if (base.save(sqlHelperWs.RETAIN_DATASET))
                    {
                        this.writeAudit("updateInventory: " + this._file_name + "- APPROVED", csAuditLog.Category.Update);
                        return true;
                    }
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

        public bool checkDefaults()
        {
            //spGetBranchDefaultSetup
            DataTable dt = execStoredProcedureTable("spGetBranchDefaultSetup", new object[] { this._branch });
            bool yesDefaults = true;
            if (dt != null && dt.Rows.Count > 0)
            {
                this._message = "";
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

        public override void getUpdateDS(int id)
        {
            SqlCommand cmdUploader = getSqlCommand("SELECT * FROM TBL_FILE_INVENTORY WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(cmdUploader);
        }

        /*************************************************/
        //Added code for FedAch Optimization 04052018
        public string getFedAchLoadingSummary(string filename)
        {
            DataSet ds = null;
            string retval = "";

            try
            {
                ds = execStoredProcedure("proc_Get_FedAch_Loading_Summary", new object[] { filename });
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                { retval = ds.Tables[0].Rows[0][0].ToString(); }
            }
            catch (Exception ex)
            {
                this.writeAudit(ex.Message.ToString(), csAuditLog.Category.Select);
            }
            return retval;
        }
        /*************************************************/
        #endregion

        #region INITIALIZATION
        public irsBatchFileUpload(string br, string user, int file_type, int file_format, string filename, string rate_type)
        {
            temp_dir();
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

        public irsBatchFileUpload()
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