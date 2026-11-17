using System;
using System.Data;
using System.IO;
using System.Net;
using System.Drawing;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using IRSClassLib;
using IRSWorld.TranClass;

namespace IRSWorld.Faultlog
{
    public partial class Reminders : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        private string full_path = string.Empty;
        private string file_name = string.Empty;

        private string txt_for = string.Empty;
        private string txt_thru = string.Empty;
        private string txt_subj = string.Empty;
        private string txt_msg = string.Empty;
        private string txt_refno = string.Empty;
        private string txt_from = string.Empty;
        private string txt_date = string.Empty;
        private string f = string.Empty;
        private string attached = string.Empty;
        private string a = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Reminders));
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]

        public DataSet verify_seqid(string seq_id)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT * FROM TBL_REMINDERS WHERE SEQ_ID = '" + seq_id, false);
                    return commons.getDataSet(sqcommands, "SEQ_ID");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }
        #endregion

        public void send_file(object sender, EventArgs e)
        {
            txt_for = this.txtFor.Value.ToString();
            txt_thru = this.txtThru.Value.ToString();
            txt_subj = this.txtSubject.Value.ToString();
            txt_msg = this.txtareaReminderMsg.Value.ToString();
            txt_refno = this.txtRefno.Value.ToString();
            txt_from = this.txtFrom.Value.ToString();
            txt_date = this.txtDate.Value.ToString();
            a = this.Forward_Files.Value.ToString();
            attached = this.forward_hidden.Value.ToString();

            irsReminders rem = this.Reminders_irsw();

            if (file1.PostedFile.ContentLength > 0)
            {
                f = rem.file_path(file1);
            }
            else
            {
                f = "~";
            }

            if (attached != "")
            {
                if (f != "~")
                {
                    string[] q = f.Split('~');
                    q[0] += ";" + attached;
                    q[1] += ";" + a;
                    Session["reminders_rec"] = txt_for + "~" + txt_thru + "~" + txt_subj + "~" + txt_msg + "~" + txt_refno + "~" + txt_from + "~" + txt_date + "~" + q[0] + "~" + q[1];
                }
                else
                {
                    Session["reminders_rec"] = txt_for + "~" + txt_thru + "~" + txt_subj + "~" + txt_msg + "~" + txt_refno + "~" + txt_from + "~" + txt_date + "~" + attached + "~" + a;
                }
            }
            else
            {
                Session["reminders_rec"] = txt_for + "~" + txt_thru + "~" + txt_subj + "~" + txt_msg + "~" + txt_refno + "~" + txt_from + "~" + txt_date + "~" + f;
            }

            csControl cs_con = new csControl();
            cs_con.execScript(this.Page, "js_rem", "Reminders_Message.set_reminders_new()");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet email_add(string eadd)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_Reminder_eadd");
                    sqcommands.Parameters.Add("@eadd", SqlDbType.VarChar).Value = eadd;
                    return commons.getDataSet(sqcommands, "email");
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                string error = e.Message;
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string record()
        {
            try
            {
                string rec = Session["reminders_rec"].ToString();
                return rec;
            }
            catch (Exception e)
            {
                string error = e.Message;
                return null;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_path(string[] name)
        {
            foreach (string n in name)
            {
                try
                {
                    irsConfiguration config = new irsConfiguration();
                    if (System.IO.Directory.Exists(config.appsetting("Upload")) == false)
                    {
                        System.IO.Directory.CreateDirectory(config.appsetting("Upload"));
                    }
                    string fileName = Path.GetFileName(n);
                    string path = config.appsetting("Upload") + @"\";

                    path = path.Replace("\\", "/");
                    string fullpath = path + fileName;

                    Stream s = File.OpenRead(n);
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, (int)s.Length);
                    int len = (int)s.Length;

                    //if (len > 10485760)
                    //{
                    //    return "too large";
                    //}
                    s.Dispose();
                    s.Close();

                    FileStream fs = new FileStream(fullpath, FileMode.Create);
                    fs.Write(buffer, 0, len);

                    fs.Dispose();
                    fs.Close();

                    if (this.full_path == "")
                    {
                        this.full_path = fullpath;
                        this.file_name = fileName;
                    }
                    else
                    {
                        this.full_path += ";" + fullpath;
                        this.file_name += ";" + fileName;
                    }
                }
                catch (Exception e)
                {
                    string error = e.Message;
                    return "~";
                }
            }
            return this.full_path + "~" + this.file_name;
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string user_info()
        {
            string user = string.Empty;
            string time = string.Empty;
            string userid = string.Empty;
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                irsClientData id = (irsClientData)HttpContext.Current.Session["IRS_CLIENT_DATA"];
                String sql = "SELECT FullName FROM TBL_CIF_MASTER WHERE CIFID = " + "'" + id.CIFID + "'";
                userid = id.UserId;
                user = Convert.ToString(commons.execScalar(sql));
                time = new irsTimeZone().branchLocalTime(id.Location).ToString("MM/dd/yyyy hh:mm tt");
            }
            catch (Exception e)
            {
                string error = e.Message;
                return "Error" + "~" + error;
            }
            return user + "~" + time;
        }

        /*
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_message(int SEQ_ID)
        {
            string msg = null;
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                String sql = "SELECT ReminderMsg FROM TBL_REMINDERS WHERE SEQ_ID = " + SEQ_ID ;
                msg = Convert.ToString(commons.execScalar(sql));
            }
            catch (Exception e)
            {
                string error = e.Message;
                return error;
            }

            return msg; 
        }
        */


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_message(int SEQ_ID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_remindermsg");
                    sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int).Value = SEQ_ID;
                    return Convert.ToString(commons.getDataSet(sqcommands, "remindermsg").Tables[0].Rows[0][0]);
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool reference_no(int SEQ_ID, string ref_no)
        {
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                commons.execScalar("sp_Reminders_refno", new object[] { SEQ_ID, ref_no });
                return true;
            }
            catch (Exception e)
            {
                string error = "Invalid" + e.Message;
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.do_reminders("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["REMINDERS_SEQ_ID"] = field_group;
            return this.do_reminders("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_reminders("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_reminders("ROLLBACK");
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["REMINDERS_MESSAGE"] != null)
            {
                return (string)Session["REMINDERS_MESSAGE"];
            }
            return string.Empty;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["REMINDERS_MESSAGE"] = null;
            Session["REMINDERS_MESSAGE"] = msg;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string REMINDERS_SEQ_ID()
        {
            if (Session["REMINDERS_SEQ_ID"] != null)
            {
                return (string)Session["REMINDERS_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_REMINDERS_RECORD(string column_values)
        {
            irsReminders reminders = this.Reminders_irsw();
            string[] s = column_values.Split('|');
            if (s != null && s.Length > 0)
            {
                if (reminders.set_record(s) == false)
                {
                    this.set_message(reminders.Message); return false;
                }
                Session["IRS_REMINDERS"] = reminders;
                return true;
            }
            reminders = null;
            this.set_message("UNABLE TO EXECUTE METHOD: SET_REMINDERS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_REMINDERS_RECORD(string client_seq_id)
        {
            irsReminders reminders = this.Reminders_irsw();
            try
            {
                if (reminders != null)
                {
                    if (reminders.get_record(client_seq_id) == true)
                    {
                        return reminders.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_REMINDERS"] = reminders;
            }
        }



        #endregion

        #region PRIVATE METHOD
        private irsReminders Reminders_irsw()
        {
            if (Session["IRS_REMINDERS"] == null)
            {
                Session["IRS_REMINDERS"] = new irsReminders();
            }
            return (irsReminders)Session["IRS_REMINDERS"];
        }


        private bool do_reminders(string instruction)
        {
            irsReminders cad = this.Reminders_irsw();
            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cad.selectedRow = -1;
                            if (cad.insert_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            cad.SEQ_ID = this.REMINDERS_SEQ_ID();

                            cad.selectedRow = 0;
                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_REMINDERS_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }
                            if (cad.commit_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            cad.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["IRS_REMINDERS"] = null;
                Session["IRS_REMINDERS"] = cad;
                Session["REMINDERS_SEQ_ID"] = null;
                Session["REMINDERS_SEQ_ID"] = cad.SEQ_ID;

                cad = null;
            }
            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion
    }
}
