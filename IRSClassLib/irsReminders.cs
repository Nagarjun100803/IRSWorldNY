using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

namespace IRSClassLib
{
    [Serializable]
    public class irsReminders : sqlHelperWs
    {
        #region PRIVATE VARIABLE
        
        private DataSet data_set = null;
        private string error_message = null;
        private string _seq_id = null;
        private string _referenceno = null;
        private string _reminderfor = null;
        private string _reminderthru = null;
        private string _reminderfrom = null;
        private string _date = null;
        private string _subject = null;
        private string _remindermsg = null;
        private string _filename = null;
        //private string temp = AppDomain.CurrentDomain.BaseDirectory;

        #endregion

        #region PROPERTY GET & SET
        
        public string messages
        {
            get { return this.error_message; }
        }
        public string SEQ_ID
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }
        public string ReferenceNo
        {
            get { return this._referenceno; }
            set { _referenceno = value; }
        }
        public string ReminderFor
        {
            get { return this._reminderfor; }
            set { _reminderfor = value; }
        }
        public string ReminderThru
        {
            get { return this._reminderthru; }
            set { _reminderthru = value; }
        }
        public string ReminderFrom
        {
            get { return this._reminderfrom; }
            set { _reminderfrom = value; }
        }
        public string Date
        {
            get { return this._date; }
            set { _date = value; }
        }
        public string Subject
        {
            get { return this._subject; }
            set { _subject = value; }
        }
        public string Remindermsg
        {
            get { return this._remindermsg; }
            set { _remindermsg = value; }
        }
        public string Filename
        {
            get { return this._filename; }
            set { _filename = value; }
        }
        #endregion

        #region PUBLIC METHOD

        public DataSet getdataRecord()
        {
            if (this.data_set != null)
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
                return s.Replace("'", "''");
            }
            return null;
        }
        public bool get_record(string seq_id)
        {
            base.IdentityValue = seq_id;
            return get_record();
        }
        public bool get_record()
        {
            this.getUpdateDS(Convert.ToInt32(base.IdentityValue));
            if (dataset != null && dataset.Tables[0].Rows.Count > 0)
            {
                base.selectedRow = 0;
                _seq_id = this.getRecord(base.getColumn("SEQ_ID"));
                _referenceno = this.getRecord(base.getColumn("ReferenceNo"));
                _reminderfor = this.getRecord(base.getColumn("ReminderFor"));
                _reminderthru = this.getRecord(base.getColumn("ReminderThru"));
                _reminderfrom = this.getRecord(base.getColumn("ReminderFrom"));
                _date = this.getRecord(base.getColumn("Date"));
                _subject = this.getRecord(base.getColumn("Subject"));
                _remindermsg = this.getRecord(base.getColumn("Message"));
                _filename = this.getRecord(base.getColumn("FileName"));

                this.data_set = dataset;
                return true;
            }
            return false;
        }

        public string file_path(System.Web.UI.HtmlControls.HtmlInputFile n)
        {
            string full_path = string.Empty;
            string file_name = string.Empty;
            try
            {
                irsConfiguration config = new irsConfiguration();
                csFileIo fio = new csFileIo();
                if (System.IO.Directory.Exists(config.appsetting("Upload")) == false)
                {
                    System.IO.Directory.CreateDirectory(config.appsetting("Upload"));
                }
                string fileName = n.PostedFile.FileName;
                string fio_name = fio.getFilename(fileName);
                    //Path.GetFileName(n);

                string path = config.appsetting("Upload") + @"\";
                
                path = path.Replace("\\", "/");
                string fullpath = path + fio_name;

                n.PostedFile.SaveAs(fullpath);

                if (full_path=="")
                {
                    full_path = fullpath;
                    file_name = fio_name;
                }
                else
                {
                    full_path += ";" + fullpath;
                    file_name += ";" + fio_name;
                }
            }
            catch(Exception e)
            {
                string error = e.Message;
                return error;
            }
            return full_path + "~" + file_name;
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
            else 
            {
                csMailer mail = new csMailer();
                string subject = column_values[2].Remove(0,8);
                string[] r;
                string chk_str = column_values[0].Trim();
                if (chk_str.EndsWith(";"))
                {
                    string col = chk_str.Remove(chk_str.Length - 1, 1);
                    r = col.Split('~');
                }
                else
                {
                    r = chk_str.Split('~');
                }
                string[] recipient = r[1].Split(';');

                string[] c;
                string chk_cc = column_values[1].Trim();
                if (chk_cc.EndsWith(";"))
                {
                    string copy = chk_cc.Remove(chk_cc.Length - 1, 1);
                    c = copy.Split('~');
                }
                else
                {
                    c = chk_cc.Split('~');
                }
                string[] cc = c[1].Split(';');
                string from = column_values[5].Remove(0, 13);
                
                StringBuilder sbMail = new StringBuilder();
                sbMail.Append("Sender: " + from + Environment.NewLine + Environment.NewLine);
                sbMail.Append(column_values[3].Remove(0,12));
                string msg = sbMail.ToString();

                string[] attach = column_values[7].Split('~');
                if (attach[1].ToString() != "" && attach[1].Length > 0)
                {
                    string[] attachment = attach[1].Split(';');
                    if (c[1].ToString() != "" && c[1].Length > 0)
                    {
                        if (mail.sendMail(subject, recipient, cc, msg, attachment))
                        {
                            return true;
                        }
                        else
                        {
                            Message = mail.Message;
                            return false;
                            //return true;
                        }
                    }
                    else
                    {
                        if (mail.sendMail(subject, recipient, msg, attachment))
                        {
                            return true;
                        }
                        else
                        {
                            Message = mail.Message;
                            return false;
                            //return true;
                        }
                    }
                }
                else 
                {
                    if (c[1].ToString() != "" && c[1].Length > 0)
                    {
                        if (mail.sendMail(subject, recipient, cc, msg))
                        {
                            return true;
                        }
                        else
                        {
                            Message = mail.Message;
                            return false;
                            //return true;
                        }
                    }
                    else
                    {
                        if (mail.sendMail(subject, recipient, msg))
                        {
                            return true;
                        }
                        else
                        {
                            Message = mail.Message;
                            return false;
                            //return true;
                        }
                    }
                }
            }
        }

        public irsReminders()
        {
            this.TableName = "TBL_REMINDERS";
            this.IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }
        
        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_REMINDERS WHERE SEQ_ID = " + id, false);
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
            base.IdentityValue = this.SEQ_ID;
            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                return true;
            }
            this.get_error_dbase();
            return false;
        }
        public bool commit_record()
        {
            if (base.save() == true)
            {
                this.SEQ_ID = base.IdentityValue.ToString(); return true;
            }
            this.get_error_dbase(); return false;
        }
        public void rollback_record()
        {
            base.cancel();
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

        #endregion

    }
}
