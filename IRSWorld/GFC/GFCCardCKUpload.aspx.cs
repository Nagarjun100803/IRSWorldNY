using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;


namespace IRSWorld.GFC
{
    public partial class GFCCardCKUpload : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GFCCardCKUpload));
        }

        #region Ajax Methods
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable popDropDown(String mode)
        {
            irsSqlCommonsWs com = new irsSqlCommonsWs();
            String SQL = "";

            if (mode == "0")
            {
                SQL = "select FldCode, FldValue from tbl_lookup where FldGroup = 'GFCUploadType'";
            }
            else if(mode == "1")
            {
                SQL = "select FldCode, FldValue from tbl_lookup where FldGroup = 'GFCGroups' order by FldCode";
            }
            
            return getDataTable(SQL);
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();

                com.EnforceConstraint = false;

                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "TABLE");

                if (com.Message != null && com.Message != String.Empty)
                {
                    throw new Exception(com.Message);
                }

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getDataSet(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();

                com.EnforceConstraint = false;

                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "TABLE");

                if (com.Message != null && com.Message != String.Empty)
                {
                    throw new Exception(com.Message);
                }

                return ds;
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool Upload(String Filename, String uploadType, String BatchNumber, String mode, out String BatchName)
        {
            irsGFC doGFC = new irsGFC();
            String logdesc = "Mode=" + mode + "|UploadType=" + uploadType + "|FileName=" + Filename;
            BatchName = "";

            doGFC.writeGFCAudit("Upload", logdesc + "|Logdate=" + DateTime.Now.ToString("MMddyyyyhhmmsss"), "Start Upload", "", WinAppLogger.Category.Insert);      

            try
            {

                csFileIo fio = new csFileIo();
                String tempFolder = AppDomain.CurrentDomain.BaseDirectory + "Temp";

                if (!fio.directoryExist(tempFolder)) { fio.createDirectory(tempFolder); }
                               
                ArrayList arrdata = new ArrayList();
                System.IO.StreamReader sr = new System.IO.StreamReader(File1.PostedFile.InputStream);
                while (true)
                {
                    String buff = sr.ReadLine();
                    if (buff == null) break;
                    arrdata.Add(buff);
                }
                String[] s = (String[])arrdata.ToArray(typeof(String));
                
                s = cleanEmptyRow(s);
                int totRecords = 0;
                String header = "";
                String errmsg = "";
                String dir = "";
                String fname = "";
                String outName = "";
                String fileLoaderDir = "";
                
                int upType;

                if (mode == "0") // CARD CK UPLOAD
                {
                    //Validating File
                    if (!isNumeric(uploadType)) { throw new Exception("Invalid Upload Type"); }
                    else { upType = int.Parse(uploadType); }

                    //if ((upType == 0 && s[0].Substring(0,3) != "GFC"))
                    if ((upType == 0 && s[0].Substring(0, ((s[0].Length >= 3) ? 3 : s[0].Length)) == "000"))
                    {
                        header = "GFC Platinum";
                        ArrayList arr = new ArrayList();
                        // Append header to file
                        arr.Add(header);
                        foreach (String ss in s) { arr.Add(ss); }
                        s = (String[])arr.ToArray(typeof(String));
                    }

                    if (!validateFile(s, upType, BatchNumber, out errmsg))
                    {
                        throw new Exception(errmsg);
                    }

                    // Modifying Header
                    if (upType == 0)
                    {                  
                        header = s[0];

                        totRecords = s.Length - 1;
                 
                        String cardType = getCardType(header);
                        header = "HDR" + totRecords.ToString("000000") + cardType + BatchNumber.PadRight(50, ' ') + get_branch_localtime(doGFC.get_irsclient_data("LOCATION")).ToString("MMddyyyy");

                        s[0] = header;
                        
                        dir = tempFolder;
                        if (!fio.directoryExist(dir)) { fio.createDirectory(dir); }
                        fname = fio.getFilename(Filename);

                        outName = dir + "\\" + fname;
                        fileLoaderDir = getAppSettings("GFCCardDirectory");
                    }
                    else if (upType == 1)
                    {

                        header = s[0];
                        String footer = s[s.Length - 1];                        
                        totRecords = int.Parse(footer.Substring(1,9).Trim());

                        header = "HDR" + totRecords.ToString("000000") + " " + BatchNumber.PadRight(50, ' ') + get_branch_localtime(doGFC.get_irsclient_data("LOCATION")).ToString("MMddyyyy");

                        s[0] = header;
                        s[s.Length - 1] = "";

                        dir = tempFolder;
                        if (!fio.directoryExist(dir)) { fio.createDirectory(dir); }
                        fname = fio.getFilename(Filename);

                        outName = dir + "\\" + fname;
                        fileLoaderDir = getAppSettings("GFCCKDirectory");
                    }
                    else
                    {
                        throw new Exception("Invalid Upload Type");
                    }
                }
                else if(mode == "1") // Batch Upload
                {
                    String batchname = DateTime.Now.ToString("MMddyyyyhhmmsss");
                    process_batch_upload_data(s, uploadType, batchname);
                    BatchName = batchname + "~" + fio.getFilename(Filename);

                    totRecords = s.Length;
                    String userid = doGFC.get_irsclient_data("UID");
                    header = "HDR" + totRecords.ToString("000000") + int.Parse(uploadType).ToString("00") + userid.PadRight(20, ' ') + get_branch_localtime(doGFC.get_irsclient_data("LOCATION")).ToString("MMddyyyy") + "1170" + batchname;

                    dir = tempFolder;
                    if (!fio.directoryExist(dir)) { fio.createDirectory(dir); }
                    fname = fio.getFilename(Filename);

                    outName = dir + "\\" + fname;
                    fileLoaderDir = getAppSettings("GFCBatchDirectory");
                }
                

                // rewriting file to temp and move to working directory      
                if (writeFile(outName, s, ((mode == "1") ? header : ""), out errmsg))
                {
                    if (getAppSettings("GFCUploadMethod") == "copy")
                    {
                        if (copyFile(outName, (fileLoaderDir + "\\" + fname), out errmsg)) 
                        {
                            doGFC.writeGFCAudit("Upload", logdesc + "|Logdate=" + DateTime.Now.ToString("MMddyyyyhhmmsss"), "End Upload|UploadMethod=copy|Status=Success|BatchNumber=" + BatchName, "", WinAppLogger.Category.Insert);  
                            return true; 
                        }
                        else { throw new Exception(errmsg); }
                    }
                    else
                    {
                        if (doFtp(outName, fileLoaderDir, out errmsg)) 
                        {
                            doGFC.writeGFCAudit("Upload", logdesc + "|Logdate=" + DateTime.Now.ToString("MMddyyyyhhmmsss"), "End Upload|UploadMethod=FTP|Status=Success|BatchNumber=" + BatchName, "", WinAppLogger.Category.Insert);  
                            return true; 
                        }
                        else { throw new Exception(errmsg); }
                    }
                    
                }
                else { throw new Exception(errmsg); }
            

            }
            catch (Exception ex)
            {
                doGFC.writeGFCAudit("Upload", logdesc + "|Logdate=" + DateTime.Now.ToString("MMddyyyyhhmmsss"), "End Upload|Status=Failed|BatchNumber=" + BatchName + "|Reason=" + ex.Message, "", WinAppLogger.Category.Insert);  
                throw new Exception("Failed to upload file, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String set_report_form(String BatchNo, String ReportType)
        {
            irsGFC doGFC = new irsGFC();
            
            try
            {
                String ReportName = "";
                String[] Parameter = null;

                if (ReportType == "Release")
                {
                    ReportName = "/GFC/rptBatchMoneyCardReleaseForm.rpt";
                    Parameter = new String[] {"1" , BatchNo};
                }
                else if (ReportType == "Terms")
                {
                    ReportName = "/GFC/rptDynamicTermsAndCondition.rpt";
                    Parameter = new String[] { "1", BatchNo, "1" };
                }

                String result = doGFC.setReport(ReportName, Parameter );

                if (result == "")
                {
                    throw new Exception(doGFC.message);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Private Methods

        private String getAppSettings(String Key)
        {
            return new AppSettingsReader().GetValue(Key, Type.GetType("System.String")).ToString();
        }

        private bool validateFile(String[] data, int uploadType, String BatchNum, out String msg)
        {
            msg = "";
            try
            {
                if (BatchNumberExist(BatchNum, uploadType)) { throw new Exception("Batch Number already exists"); }
                if (data.Length == 0) { throw new Exception("File has no data"); }                                   
                if (uploadType == 0)
                {
                    if (isNumeric(data[0].Trim())) { throw new Exception("Invalid Header"); }
                    if (getCardType(data[0].Trim()) == "") { throw new Exception("Invalid Card Type"); }
                }
                else if (uploadType == 1)
                {
                    String hdr = data[0];
                    String ftr = data[data.Length - 1];
                    if (hdr.Length < 9) { throw new Exception("Invalid Header format"); }
                    if (!isNumeric(hdr.Substring(0, 1))) { throw new Exception("Invalid Record Code for Header"); }
                    if (int.Parse(hdr.Substring(0, 1)) != 1) { throw new Exception("Invalid Record Code for Header"); }                    
                    if (ftr.Length < 10) { throw new Exception("Invalid Footer Format"); }
                    if (!isNumeric(ftr.Substring(0, 1))) { throw new Exception("Invalid Record Code for Footer"); }
                    if (int.Parse(ftr.Substring(0, 1)) != 3) { throw new Exception("Invalid Record Code for Footer"); }
                    if (!isNumeric(ftr.Substring(1, 9).Trim())) { throw new Exception("Invalid Footer Record Count"); }
                    if (int.Parse(ftr.Substring(1, 9).Trim()) != (data.Length - 2)) { throw new Exception("Footer Record did not match with the actual record count."); }
                }
                else
                {
                    throw new Exception("Invalid Upload type.");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return false;
            }
            return true;
        }

        private String[] cleanEmptyRow(String[] data)
        {
            try
            {
                ArrayList s = new ArrayList();
                
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i].Trim() != "") { s.Add(data[i]); }
                }

                String[] StringArray = new String[s.Count];

                for (int y = 0; y < s.Count; y++)
                {
                    StringArray[y] = s[y].ToString();
                }

                return StringArray;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to clean empty rows, Reason : " + ex.Message);
            }
        }

        private bool isDate(String date)
        {
            // restructure to remove warning and ensure date checking
            try
            {
                //DateTime d = new DateTime();
                DateTime.Parse(date);
                return true;
            }
            catch
            {}
            return false;
        }

        private bool copyFile(String inName, String outName, out String msg)
        {
            msg = "";
            try
            {
                csFileIo fio = new csFileIo();

                if (fio.copy(inName, outName, true))
                {                   
                    fio.delete(inName);
                    return true;
                }
                else
                {
                    throw new Exception("Cannot move file, Reason : " + fio.Message);
                }
            }
            catch (Exception ex)
            {
                msg = "Failed to move file : " + inName + ", Reason: " + ex.Message;
                return false;
            }
        }

        private bool doFtp(String path, String LoadDir, out String msg)
        {
            msg = String.Empty;
            //CHR ITG Password encrypt -- String constr = getAppSettings("GFCftpConnectionString");
            String constr = new irsConfiguration().ConnectionStringNoRunMode("GFCftpConnectionString");
            if (constr == null || constr.Length < 1)
            {
                msg = "The Ftp connection string is not available";
                return false;
            }

            String loadir = LoadDir;
            if (loadir == null || loadir.Length < 1)
            {
                msg = "The loading directory was not specified";
                return false;
            }

            csFtp ftp = new csFtp(constr);
            if (ftp.connect())
            {
                if (ftp.cwd(loadir))
                {
                    if (!ftp.put(path))
                    {
                        msg = "FTP Put: " + ftp.Message;
                    }
                }
                else
                {
                    msg = "FTP Cwd: " + ftp.Message;
                }
                ftp.disconnect();
                new csFileIo().delete(path);
            }
            else
            {
                msg = "FTP Connect: " + ftp.Message;
            }

            return (msg == String.Empty);
        }

        private bool writeFile(String fName, String[] data, String Header, out String msg)
        {
            msg = "";
            try
            {
                csFileIo fio = new csFileIo();

                if (fio.open_output(fName, (int)csFileIo.write_modes.CREATE))
                {

                    if (Header != null && Header != String.Empty)
                    {
                        fio.putLine(Header);
                    }

                    foreach (String s in data)
                    {
                        fio.putLine(s);
                    }

                    fio.close_output();

                    return true;
                }
                else
                {
                    throw new Exception("Failed to open Output." + fio.Message);
                }
            }
            catch (Exception ex)
            {
                msg = "Failed to create file : " + fName + ", Reason : " + ex.Message;
                return false;
            }
        }

        private bool BatchNumberExist(String batchNum, int uploadType)
        {
            try
            {
                String sql = "";

                if (uploadType == 0)
                {
                    sql = "select count(1) from tbl_gfc_cards where BatchNo = '" + batchNum.Trim() + "'";
                }
                else
                {
                    sql = "select count(1) from tbl_gfc_ckaccounts where BatchNo = '" + batchNum.Trim() + "'";
                }

                DataTable dt = getDataTable(sql);

                if (int.Parse(dt.Rows[0][0].ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("Checking batch number failed, Reason : " + ex.Message);
            }
        }

        private String getCardType(String header)
        {
            try
            {
                String sql = "select fldValue from tbl_lookup where fldgroup = 'GFCCardType' and fldDescription = '" + header + "'";
                DataTable dt = getDataTable(sql);

                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0][0].ToString();
                }

                return "";
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get cardType, Reason : " + ex.Message);
            }
        }

        //private bool validateFile;

        private bool isNumeric(String s)
        {
            try
            {
                long L = long.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //private irsClientData irsClientData
        //{
        //    get
        //    {
        //        if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] == null)
        //        {
        //            System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = new irsClientData();
        //        }
        //        return (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
        //    }
        //}

        //private String get_irsclient_data(string param)
        //{
        //    string s = null;
        //    switch (param)
        //    {
        //        case "UID": s = irsClientData.UserId; break;
        //        case "CIF": s = irsClientData.CIFID; break;
        //        case "LOCATION": s = irsClientData.Location; break;
        //        case "IP": s = irsClientData.IPAddress; break;
        //    }
        //    return s;
        //}

        private DateTime get_branch_localtime(string brcode)
        {
            DateTime dt = new irsTimeZone().branchLocalTime(brcode);
            return dt;
        }

        private void process_batch_upload_data(String[] data, String GroupNum, String BatchName)
        {
            try
            {
                irsGFC gfc = new irsGFC();

                
                String UID = gfc.get_irsclient_data("UID");
                String BranchCode = gfc.get_irsclient_data("LOCATION");

                foreach (String d in data)
                {
                    save_and_get_batch_ck(d, GroupNum, BatchName, UID, BranchCode);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to process batch upload data, Reason : " + ex.Message);
            }
        }

        private void save_and_get_batch_ck(String CardNumber, String GroupNum, String BatchName, String UID, String BranchCode)
        {
            try
            {
                irsGFC gfc = new irsGFC();

                Object[] plist = 
                    {
                        CardNumber                            
                        ,GroupNum
                        ,UID
                        ,BranchCode
                        ,BatchName
                    };

                if (!gfc.execProcedureUpdate("sp_gfc_batchupload", plist))
                {
                    throw new Exception(gfc.message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save and get ck for card number : " + CardNumber + ", Reason : " + ex.Message);
            }
        }
     
        #endregion

        protected void Button1_Click(object sender, EventArgs e)
        {
            HBatchNum.Value = "";
            
            try
            {   
                String file = File1.Value;
                String BatchName = txtBatchName.Value;
                String ddl = hUpType.Value;
                String Mode = HMode.Value;

                if (file == null || file.Trim() == String.Empty) { throw new Exception("Invalid file path"); }
                if (Mode == "0" && (BatchName == null || BatchName.Trim() == String.Empty)) { throw new Exception("Invalid batch number"); }
                if (Mode == "0" && (ddl == null || ddl.Trim() == String.Empty)) { throw new Exception("Invalid upload type"); }
                if (Mode == "1" && (ddl == null || ddl.Trim() == String.Empty)) { throw new Exception("Invalid group"); }

                String bname = "";
                if (Upload(file, ddl, BatchName, Mode, out bname))
                {                    
                    lblStatus.InnerText = "File successfully uploaded";
                    HBatchNum.Value = bname;
                }
            }
            catch (Exception ex)
            {
                lblStatus.InnerText = ex.Message;
            }
        }
    }
}
