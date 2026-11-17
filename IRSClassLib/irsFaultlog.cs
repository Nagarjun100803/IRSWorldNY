using System;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsFaultlog : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _issue_reference = null;
        private string _date_reported = null;
        private string _issue_description = null;
        private string _application_module = null;
        private string _branch_agent = null;
        private string _tested_by = null;
        private string _remarks = null;
        private string _status = null;
        private string _attended_by = null;
        private string _date_attended = null;
        private string _screen_shot = null;
        public string fullPath = null;
        #endregion

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }
        public string issue_reference
        {
            get { return this._issue_reference; }
            set { _issue_reference = value; }
        }

        public string date_reported
        {
            get { return this.date_reported; }
            set { _date_reported = value; }
        }

        public string issue_description
        {
            get { return this._issue_description; }
            set { _issue_description = value; }
        }

        public string application_module
        {
            get { return this._application_module; }
            set { _application_module = value; }
        }

        public string branch_agent
        {
            get { return this._branch_agent; }
            set { _branch_agent = value; }
        }

        public string tested_by
        {
            get { return this._tested_by; }
            set { _tested_by = value; }
        }

        public string remarks
        {
            get { return this._remarks; }
            set { _remarks = value; }
        }


        public string status    
        {
            get { return this._status; }
            set { _status = value; }
        }

        public string attended_by
        {
            get { return this._attended_by; }
            set { _attended_by = value; }
        }

        public string date_attended
        {
            get { return this._date_attended; }
            set { _date_attended = value; }
        }

        public string screen_shot
        {
            get { return this._screen_shot; }
            set { _screen_shot = value; }
        }

        #endregion

        #region PUBLIC METHOD

        public DataSet getdataRecord()
        {
            if (this.dataset != null)
            {
                return this.data_set;
            }

            return null;
        }

        public string getRecord(object obj)
        {
            if (obj != null)
            {
                string s = Convert.ToString(obj);

                if (s != null && s.Trim() == string.Empty)
                {
                    return null;
                }
                return s.Replace("'","''");
            }
            
            return null;
        }




        public bool get_record(string seq_id)
        {
            base.IdentityValue = seq_id; return get_record();                
        }

        public bool get_record()
		{           
            this.getUpdateDS(Convert.ToInt32(base.IdentityValue));

            if (dataset != null && dataset.Tables[0].Rows.Count > 0)
            {
                base.selectedRow = 0;

                _seq_id             = this.getRecord(base.getColumn("SEQ_ID"));
                _issue_reference    = this.getRecord(base.getColumn("IssueReference"));
                _date_reported      = this.getRecord(base.getColumn("DateReported"));
                _issue_description  = this.getRecord(base.getColumn("IssueDescription"));
                _application_module = this.getRecord(base.getColumn("ApplicationModule"));
                _branch_agent       = this.getRecord(base.getColumn("BranchAgent"));
                _tested_by          = this.getRecord(base.getColumn("TestedBy"));
                _remarks            = this.getRecord(base.getColumn("Remarks"));
                _status             = this.getRecord(base.getColumn("Status"));
                _attended_by        = this.getRecord(base.getColumn("AttendedBy"));
                _date_attended      = this.getRecord(base.getColumn("DateAttended"));
                _screen_shot        = this.getRecord(base.getColumn("ScreenShot"));


                this.data_set = dataset; return true; 
            }
            return false;
		}

		public bool set_record(string[] column_values)
		{
			ArrayList error_list = new ArrayList();

			for (int i = 0; i < column_values.Length; i++)
			{
				string[] s = column_values[i].Split('~');

                if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                {
                    s[1] = null;
                }


				if (s != null && s.Length > 0)
				{					
					if (!base.setColumn(s[0].Trim(), s[1]))
					{
						error_list.Add(s[0] + ":" + this.Message);
					}					
				}
			}

			if (error_list.Count > 0)
			{
				StringBuilder sb = new StringBuilder();

				foreach (Object errors in error_list)
				{
					sb.Append((String)errors + "\n");
				}

				this.error_message = string.Empty;
				this.error_message = sb.ToString();

				return false;
			}

			return true;
		}

        public irsFaultlog()
		{
            this.TableName = "TBL_FAULTLOG";
			this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
		}
         
		public override void getUpdateDS(int id)
		{
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_FAULTLOG WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(command);
		}


		public bool insert_record()
		{
            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;			
		}

		public bool update_record()
		{
            base.IdentityValue = this.seq_id;

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
		}

        public bool commit_record()
        {
            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }
			this.get_error_dbase(); return false;
        }

        public void rollback_record()
        {
            base.cancel();
        }

        public string readFile(System.Web.UI.WebControls.FileUpload file)
        {
            if (!this.saveFile(file))
            {
                return "Error~ Screenshot not Saved!";
            }
            else
            {
                return fullPath;
            }

        }
        private bool saveFile(System.Web.UI.WebControls.FileUpload file)
        {
            try
            {
                irsConfiguration config = new irsConfiguration();
                if (System.IO.Directory.Exists(config.appsetting("Upload")) == false)
                {
                    System.IO.Directory.CreateDirectory(config.appsetting("Upload"));
                }
                string myFolder = config.appsetting("Upload");

                csFileIo io = new csFileIo();
                string source_path = file.PostedFile.FileName;
                string fileName = io.getFilename(source_path);
                string extension = io.getFileExtension(source_path);


                string contenttype = this.GetContentType(fileName);

                string[] type = contenttype.Split('/');
                if (type[0] == "image" || type[1] == "msword" || type[1] == "vnd.ms-excel" )
                {

                    string path = myFolder + @"\";
                    string pathname = path + @"\" + fileName;
                    //csFileIo io = new csFileIo();
                    if (!io.directoryExist(path)) io.createDirectory(path);
                    //if (!Directory.Exists(path))
                    //{
                    //    string strFolderToCreate = path;
                    //    if (path.EndsWith(@"\"))
                    //        strFolderToCreate = path.Substring(0, (strFolderToCreate.Length - 1));

                    //    Directory.CreateDirectory(strFolderToCreate);
                    //}
                    //file.PostedFile.SaveAs(pathname);

                    path = path.Replace("\\", "/");

                    fullPath = path + fileName;


                    Stream s = File.OpenRead(file.PostedFile.FileName);

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, (int)s.Length);

                    int len = (int)s.Length;

                    if (len > 10485760)
                    {
                        return false;
                    }
                    s.Dispose();
                    s.Close();

                    FileStream fs = new FileStream(fullPath, FileMode.Create);
                    fs.Write(buffer, 0, len);

                    if (type[0] == "image")
                    {
                        Bitmap bmp = new Bitmap(fs);

                        bmp.Save(fileName);
                        bmp.Dispose();
                    }

                    fs.Dispose();
                    fs.Close();

                    //return fullPath;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                error_message = e.Message;
                return false;
            }


        }

        #endregion

        #region PRIVATE METHOD


        private void get_error_dbase()
        {
            this.error_message = string.Empty;

            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }

        private string GetContentType(string fileName)
        {

            string contentType = "application/octetstream";

            string ext = Path.GetExtension(fileName).ToLower();

            Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);

            if (registryKey != null && registryKey.GetValue("Content Type") != null)

                contentType = registryKey.GetValue("Content Type").ToString();

            return contentType;

        }

        #endregion

    }
}
