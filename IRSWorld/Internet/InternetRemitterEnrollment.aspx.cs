using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using IRSClassLib;
using IRSWorld.TranClass;

namespace IRSWorld.Internet
{
    public partial class InternetRemitterEnrollment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getIPAddress();
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetRemitterEnrollment));
            Session["upload_status"] = "";
            //bool test = check_idology();
        }
        private irsUsers IRSUsers
        {
            get
            {
                if (Session["IRS_USERS"] == null)
                {
                    if (Session["IRS_CLIENT_DATA"] == null) throw new Exception("Session Expired.");
                    Session["IRS_USERS"] = new irsUsers(true);
                }
                return (irsUsers)Session["IRS_USERS"];
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String sample()
        {
            sqlHelperWs common = new sqlHelperWs();
            common.IsWinApp = true;
            DataSet ds = null;

            String sql = "select top 1 * from tbl_lookup";
            ds = common.getDataSet(common.getSqlCommand(sql, false));


            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "18";
                }
            }

            return "Success";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_title(string fldgroup,string branchcode,string userrole)
        {
            try
            {
                String errmsg = "";
                DataSet ds = execprocedure("sp_internet_dropdown", new Object[] { fldgroup, branchcode, Convert.ToInt16(userrole) }, out errmsg);
                return ds;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getCIFID()
        {
            string errmsg="";
            DataSet ds = execprocedure("selecttIntoUser", new Object[] { }, out errmsg);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["CIFID"].ToString();
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_idology_enabled(String branchcode)
        {
            String errmsg = "";
            DataSet ds = execprocedure("proc_get_idology_setup", new Object[] { branchcode }, out errmsg);

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            throw new Exception(errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_branch()
        {
            try
            {
                String errmsg = "";
                DataSet ds = execprocedure("sp_internet_search_branch_code", new Object[] { 3 }, out errmsg);
                return ds;
            }
            catch
            {
                return null;
            }
        }
        //[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        //public string getPassword(string username,string userId)
        //{
        //    irsTDD tdd = new irsTDD();
        //    Session["encpass"] = tdd.EncryptPassword(username, userId);
        //    return Session["encpass"].ToString();
        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String submit_record(Object[] plist)
        {
            Int32 errcount = 0;
            String errmsg = "";
            try
            {
                Session["current_data"] = plist;
                DataSet ds = execprocedure("sp_insert_remitter_client", plist, out errmsg);
                if (errmsg != null && errmsg != "")
                {
                    throw new Exception("Failed to save record." + errmsg);
                }
                String errmsgList = "";
                if (plist[45].ToString() != "")
                {
                    if (!check_supporting_docs(plist[45].ToString(), out errmsg))
                    {
                        errmsgList += errmsg + "\n"; errcount++;
                    }
                }
                
                if (errmsgList != "")
                {
                    throw new Exception(errmsgList);
                }

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    
                    upload_supporting_docs(ds.Tables[0].Rows[0][0].ToString(), plist[45].ToString());
                    //upload_supporting_docs(ds.Tables[0].Rows[0][0].ToString(), plist[25].ToString());
                    //upload_supporting_docs(ds.Tables[0].Rows[0][0].ToString(), plist[26].ToString());
                    //sendForVerificationEmail(plist);
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    throw new Exception("Failed to save record." + errmsg);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to save record.\n" + ex.Message);
            }
        }

        #region INSERT TO TBL_USERS

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object save(String rec)
        {
            String msg = "";
            String[] ss = rec.Split('~');
            if (ss[0].Equals("add") && IRSUsers.new_user(get_new_userid(ss))) msg = this.setColumns(ss);
            else if (ss[0].Equals("edit"))
            {
                if (ss[1] != null && ss[1].Length > 0)
                {
                    try
                    {
                        DateTime dt = DateTime.Parse(ss[2]);
                        if (IRSUsers.edit_user(ss[1], dt)) msg = this.setColumns(ss);
                    }
                    catch (Exception e)
                    {
                        msg = "Edit Error:\n " + e.Message;
                    }
                }
            }
            else if (ss[0].Equals("delete"))
            {
                if (ss[1] != null && ss[1].Length > 0)
                {
                    try
                    {
                        if (!IRSUsers.delete_user(ss[1], int.Parse(ss[3].Trim())))
                        {
                            msg = "Delete Error:\n " + IRSUsers.Message;
                        }
                    }
                    catch (Exception e)
                    {
                        msg = "Delete Error:\n " + e.Message;
                    }
                }
            }
            else
            {
                msg = ss[0] + ": operation not supported! " + IRSUsers.Message;
            }

            String retval = "";
            if (msg == String.Empty & !ss[0].Equals("delete"))
            {
                if (!IRSUsers.save_user(ss[0], out retval)) msg = IRSUsers.Message;
            }

            if (msg.Length > 0) throw new Exception(msg);

              
            return new Object[] {retval};
           // return new Object[] { IRSUsers.getSearchTable(), retval };
        }


        public String get_new_userid(String[] ss)
        {

            String[] uu = ss[3].Split('=');
            if (uu[0].Equals("USERID")) return uu[1];

            return null;
        }

        private String setColumns(String[] cols)
        {
            ArrayList al = new ArrayList();
            int stcol = cols[0].Equals("add") ? 1 : 3;
            for (int i = stcol; i < cols.Length; i++)
            {
                String[] tt = cols[i].Split('=');
                if (tt[1].ToLower().Equals("undefined")) tt[1] = null;
                else
                {
                    if (tt[1].ToLower().Equals("on")) tt[1] = "true";
                    if (tt[1].ToLower().Equals("off")) tt[1] = "false";
                }
                if (!IRSUsers.setColumn(tt[0].Trim(), tt[1]))
                {
                    al.Add(tt[0] + ": " + IRSUsers.Message);
                }
            }

            if (al.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Object o in al)
                {
                    sb.Append((String)o + "\n");
                }
                return "Add Error:\n" + sb.ToString();
            }

            return String.Empty;
        }

       



        #endregion


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void sendForVerificationEmail(Object[] plist)
        {
            irsTDD tdd = new irsTDD();
            csMailer mailer = new csMailer();
            string name = plist[2] + " " + plist[3] + " " + plist[1];
            string UserID = plist[0].ToString();
            string errmsg = "";
            string encpass = getEncryPass(UserID).ToString();
            string password = tdd.DecryptPassword(encpass.ToString());
            HttpContext context = HttpContext.Current;
            string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') ;

            string Subject = "PNB Web Remittance Online Enrollment/Registration Summary.";
            string email = getEmailAdd(UserID);
            String[] arrEmail = { email };
            string msg = " Congratulations   <i>"+ name +"</i> <br><br>";
            msg = msg + " You have successfully finished the PNB Remittance Center, Inc. Online Registration Process <br>";
            msg = msg + " Please print a copy of this email for your reference <br><br>";
            msg = msg + " Username: " + UserID + "<br>";
            msg = msg + " Email: " + email + "<br><br>";
            msg = msg + " Temporary Password:" + password + " <br><br>";
           
            msg = msg + " For any concern, you may call 888-762-76363.<br><br>";
            msg = msg + " Sincerely yours, <br>";
            msg = msg + " Customer Support Department <br>";
            msg = msg + " PNB Remittance Center, Inc, Online <br><br><br>";
            msg = msg + " THIS IS A COMPUTER GENERATED MESSAGE <br>";
            msg = msg + " NO REPLY REQUIRED <br>";

            try
            {
                if (!mailer.sendMail(Subject, arrEmail,null, msg,null,true))
                {
                    errmsg = "Error on Sending Email to Recipient!";
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally { }

        }

        public string getEncryPass(string uid)
        {
            string errmsg = "";
            DataSet ds = execprocedure("selectUserDetails", new Object[] { uid }, out errmsg);
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getEmailAdd(string userid)
        {
            String errmsg = "";
            DataSet ds = execprocedure("getInsertedRemitterEmail", new Object[] { userid }, out errmsg);
            return ds.Tables[0].Rows[0].ItemArray[0].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object reset_password(String userid)
        {   
            String retval = "";
            if (IRSUsers.resetPassword(userid, out retval))
            {
                string emailadd = retval.Split('~')[3];
                if (emailadd != "")
                {
                    send_email(userid, emailadd.Replace(';',','), "reset");
                }
                
                return new Object[] { true, retval };
            }
            else
            {
                throw new Exception("Password Reset Failed. " + IRSUsers.Message);
            }
            throw new Exception("Session Expired. Please login again.");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String send_email(String userid, String emailad, String email_op)
        {
            if (!IRSUsers.email_password(userid, emailad, email_op))
            {
                throw new Exception(IRSUsers.Message);
            }

            return emailad;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool user_exist(String userid)
        {
            String errmsg = "";
            DataSet ds = execprocedure("sp_getUser", new Object[] { "USERID", userid }, out errmsg);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdataset(String SQL, out String errmsg)
        {
            sqlHelperWs commons = new sqlHelperWs();
            commons.IsWinApp = true;
            DataSet ds = null;
            errmsg = "";
            try
            {
                ds = commons.getDataSet(commons.getSqlCommand(SQL, false));
                if (commons.Message != null && commons.Message != "")
                {
                    throw new Exception(commons.Message);
                }
                return ds;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                // Log this
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (ds != null) { ds = null; }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet execprocedure(String SP, Object[] plist, out String errmsg)
        {
            sqlHelperWs commons = new sqlHelperWs();
            commons.IsWinApp = true;
            DataSet ds = null;
            errmsg = "";
            try
            {
                ds = commons.execStoredProcedure(SP, plist);
                if (commons.Message != null && commons.Message != "")
                {
                    throw new Exception(commons.Message);
                }
                return ds;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                // Log this
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (ds != null) { ds = null; }
            }
            return null;
        }
        

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string AgeLimit()
        {
            String errmsg = "";
            DataSet ds = getdataset("select fldCode from tbl_Lookup where FldGroup='CIFAgeLimit' AND FldDescription='USR'", out errmsg);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "18";
                }
            }
            return "18";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool execprocedureupdate(String SP, Object[] plist, out String errmsg)
        {
            sqlHelperWs commons = new sqlHelperWs();
            commons.IsWinApp = true;
            DataSet ds = null;
            errmsg = "";
            try
            {
                int ra = commons.execStoredProcedureUpdate(SP, plist);

                if (commons.Message != null && commons.Message != "")
                {
                    throw new Exception(commons.Message);
                }
                return true;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                // Log this
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
            return false;
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet check_idology(String ipadd, String firstname, String lastname, String address, String city, String state, String zip, String DOB, String SSN)
        {
            Idology idol = new Idology();
            string localIP = "?";
            string errmsg = "";
            try
            {
                //idol.id_iq_url = "https://web.idologylive.com/api/idiq.svc";
                //idol.id_iq_answer_url = "https://web.idologylive.com/api/idliveq-answers.svc";
                //idol.id_user_name = "cristy.villena.dev";
                //idol.id_user_pwd = "hupace35";
                irsConfiguration config = new irsConfiguration();

                idol.id_iq_url = config.appsetting("id_iq_url").ToString();
                idol.id_iq_answer_url = config.appsetting("id_iq_answer_url").ToString();
                idol.id_user_name = config.appsetting("id_user_name").ToString();
                idol.id_user_pwd = config.appsetting("id_user_pwd").ToString();


                //idol.first_name = "JOHN";
                //idol.last_name = "SMITH";
                //idol.address = "222333 PEACHTREE PLACE";
                //idol.city = "ATLANTA";
                //idol.state = "GA";
                //idol.zip = "30318";
                //idol.dob_month = "02";
                //idol.dob_year = "1975";
                //idol.ssn_full_digit = "112223333";
                

                //String firstname, String lastname, String address, String city, String state, String zip, String DOB, String SSN
                idol.first_name = firstname;
                idol.last_name = lastname;
                idol.address = address;
                idol.city = city;
                idol.state = state;
                idol.zip = zip;

                DateTime d = new DateTime();

                if (DateTime.TryParse(DOB, out d))
                {
                    idol.dob_month = Convert.ToDateTime(DOB).ToString("MM");
                    idol.dob_year = d.Year.ToString();
                    idol.dob_day = Convert.ToDateTime(DOB).ToString("dd");
                }
                else
                {
                    idol.dob_month = "";
                    idol.dob_year = "";
                    idol.dob_day = "";
                }

                if (SSN.Length > 4)
                {
                    idol.ssn_full_digit = SSN;
                    idol.ssn_last4_digit = "";
                }
                else if (SSN.Length <= 4)
                {
                    idol.ssn_full_digit = "";
                    idol.ssn_last4_digit = SSN;
                }


                
               
                System.Net.IPHostEntry host;
                
                //host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                //foreach (System.Net.IPAddress ip in host.AddressList)
                //{
                //    if (ip.AddressFamily.ToString() == "InterNetwork")
                //    {
                //        localIP = ip.ToString();
                //    }
                //}

                if (Context.Request.ServerVariables["HTTP_VIA"] != null) // using proxy
                {
                    localIP = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();  // Return real client IP.
                }
                else// not using proxy or can't get the Client IP
                {
                    localIP = Context.Request.ServerVariables["REMOTE_ADDR"].ToString(); //While it can't get the Client IP, it will return proxy IP.
                }

                idol.ip_address = localIP; //"202.58.236.1";

                
                //idol.ip_address = "74.125.237.24"; //System.Web.HttpContext.Current.Request.UserHostAddress; //

                if (config.appsetting("id_use_proxy").ToString() == "true")
                {
                    idol.proxy_user_name = config.appsetting("id_proxy_user_name").ToString();
                    idol.proxy_password = config.appsetting("id_proxy_password").ToString();
                    idol.proxy_host = config.appsetting("id_proxy_host").ToString();
                    idol.proxy_port = int.Parse(config.appsetting("id_proxy_port").ToString());
                }

                idol.get_iq_response();
                String error = "";

                String xml_res = idol.idology_xml_response;

                DataSet ds1 = ConvertXMLToDataSet(xml_res);

                String ex = idol.idology_class_exception;
                DataSet velocity = idol.ds_velocity_results_inq;
                DataSet ds_inq = idol.ds_qualifiers_inq;

                if (ex != null && ex != "")
                {
                    error = ex;
                   
                }
                errmsg = xml_res;
                if (velocity != null)
                {
                    error = velocity.Tables[0].Rows[0][1].ToString();
                }
                //if (ds_inq != null)
                //{
                //    error = ds_inq.Tables[0].Rows[0][0].ToString();
                //}


                if (ds1.Tables.Count >= 5)
                {
                    if (Convert.ToString(ds1.Tables[1].Rows[0][1]) != "PASS")
                    {
                        throw new Exception(Convert.ToString(ds1.Tables[4].Rows[1][1]));
                    }
                }

                if (ds1.Tables.Count == 4)
                {
                    if (Convert.ToString(ds1.Tables[1].Rows[0][1]) == "FAIL")
                    {
                        throw new Exception(Convert.ToString(ds1.Tables[3].Rows[0][1]));
                    }
                }
                if (ds1.Tables.Count == 5)
                {
                    if (Convert.ToString(ds1.Tables[1].Rows[0][1]) == "FAIL")
                    {
                        throw new Exception(Convert.ToString(ds1.Tables[4].Rows[0][1]));
                    }
                }




                if (error != null && error != "")
                {
                    throw new Exception(error);
                }
                DataTable dt = new DataTable();

                dt.Columns.Add("id_number");
                dt.Rows.Add(new Object[] { idol.id_number });


                DataSet ds = new DataSet();
                ds.Tables.Add(dt);

                if (idol.ds_question_and_answer == null)
                {
                    throw new Exception("No list of questions found");
                }


                if (idol.ds_question_and_answer.Tables[0] != null)
                {
                    ds.Tables.Add(idol.ds_question_and_answer.Tables[0].Copy());
                }
                

                return ds;// idol.ds_question_and_answer;
            }
            catch (Exception ex)
            {
                new csAuditLog().writeAudit("webremituser", "Web Remittance Enrollment", ex.StackTrace + "-" + ex.Message, errmsg, ex.Source + "-" + ipadd, csAuditLog.Category.Select);
                throw new Exception("Idology Error : " + ex.Message  );
            }
            finally
            {
                idol = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkEmailExist(string email)
        {
            return new irsTDD().checkEmailExist(email);
        }

        public DataSet ConvertXMLToDataSet(string xmlData)
        {
            System.IO.StringReader stream = null;
            System.Xml.XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new System.IO.StringReader(xmlData);
                // Load the XmlTextReader from the stream
                reader = new System.Xml.XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }// Use this function to get XML string from a dataset

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool submit_idology_answer(String id_number, String answerString)
        {
            Idology idol = new Idology();

            try
            {
                String[] ansString = answerString.Split('|');
                irsConfiguration config = new irsConfiguration();

                idol.id_iq_url = config.appsetting("id_iq_url").ToString();
                idol.id_iq_answer_url = config.appsetting("id_iq_answer_url").ToString();
                idol.id_user_name = config.appsetting("id_user_name").ToString();
                idol.id_user_pwd = config.appsetting("id_user_pwd").ToString();

                idol.id_number = id_number;
                for (int i = 0; i < ansString.Length; i++)
                {
                    String[] q_and_a = ansString[i].Split('=');
                    idol.construct_q_and_a(q_and_a[0], q_and_a[1]);
                }

                idol.answer_question();
                DataSet ds = idol.ds_summary_results_qna;

                if (idol.ds_iq_summary_results_qna.Tables[0].Rows[0][0].ToString() == "pass")
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Idology Error : " + ex.Message);
            }
            finally
            {
                idol = null;
            }
        }


        //protected void btnPost_Click(object sender, EventArgs e)
        //{
        //    String FileSEQ = this.HPath.Value;
        //    String tempDir = "C:\\temp\\INTERNETCLIENT" + @"\" + FileSEQ;            
        //    String errmsg = "";
        //    String errmsglist = "";
        //    String[] err = null;
        //    int errcount = 0;
        //    int critErrcount = 0;

        //    initWorkingDir(tempDir);

        //    if (!check_supporting_docs(File1.Name, out errmsg)) { err = errmsg.Split('|'); if (err[0] == "1") { critErrcount++; } else { errcount++; } errmsglist += err[1] + "\n"; }
        //    if (!check_supporting_docs(File2.Name, out errmsg)) { err = errmsg.Split('|'); if (err[0] == "1") { critErrcount++; } else { errcount++; } errmsglist += err[1] + "\n"; }
        //    if (!check_supporting_docs(File3.Name, out errmsg)) { err = errmsg.Split('|'); if (err[0] == "1") { critErrcount++; } else { errcount++; } errmsglist += err[1] + "\n"; }

        //    if (errcount == 3 || critErrcount > 0)
        //    {
        //        throw new Exception(errmsglist);
        //    }

        //    if (!saveFile(File1, tempDir, out errmsg)) { errmsglist += errmsg + "/n"; };
        //    if (!saveFile(File2, tempDir, out errmsg)) { errmsglist += errmsg + "/n"; };
        //    if (!saveFile(File3, tempDir, out errmsg)) { errmsglist += errmsg + "/n"; };

        //    if (errmsglist != "")
        //    {
        //        Session["upload_status"] = errmsglist;
        //    }
        //    else
        //    {
        //        Session["upload_status"] = "success";  
        //    }
        //}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String getUploadMessage()
        {
            try
            {
                return (String)Session["upload_status"];
            }
            catch
            { }
            return "";
        }

        private bool check_supporting_docs(String FileName, out String errmsg)
        {
            errmsg = "";
            int CritLevel = 1;
            int ok = 0;
            

            try
            {
                csFileIo fio = new csFileIo();
                string ext = fio.getFileExtension(FileName);
                string[] extName = { "jpg", "jpeg", "png" };

                for (int k = 0; k < extName.Length; k++)
                {
                    if (extName[k].ToUpper() == ext.ToUpper())
                    { ok = 1; }
                }
                if (!fio.exist(FileName))
                {
                    CritLevel = 0;
                    throw new Exception("File Does not exist.");
                }
                else if (fio.getFileSize(FileName) > 1024000) //File Size Limit is 1MB
                {
                    throw new Exception("File size is too big.");
                    CritLevel = 1;
                }
                else if (ok != 1)
                {
                    throw new Exception("Invalid file Format.Only images are Allowed.");
                    CritLevel = 1;
                }
                
                    return true;
            }
            catch (Exception ex)
            {
                errmsg = CritLevel.ToString() + "|" + "Failed to upload File " + FileName + " Reason, " + ex.Message;
            }
            return false;
        }

        private bool upload_supporting_docs(String SEQ_ID, String Filename)
        {
            String ftpConString = "";
            String ftpRootPath = "";
            String FDir = "";
            String FName = "";
            String newFName = "";
            String ext = "";
            String name = "";
            String[] list;
            try
            {
                csFileIo fio = new csFileIo();
                sqlHelperWs sql = new sqlHelperWs();

                FDir = fio.getDirectory(Filename);
                FName = fio.getFilename(Filename);
                ext = fio.getFileExtension(Filename);
                DataTable dt = sql.getDataSet("InsertDocuments", true, new Object[] { SEQ_ID, this.getCIFID(), FName }).Tables[0];
                name = FName.Substring(0, (FName.IndexOf(ext) - 1));
                newFName = FDir + @"\" + "IRSD_" + SEQ_ID + "_" + name + "_" + dt.Rows[0].ItemArray[0].ToString() + "." + ext;
                ftpConString = new irsConfiguration().FTPConnectionString;
                ftpRootPath = new irsConfiguration().appsetting("RootPath").ToString() + @"INTERNETCLIENT\";

                csFtp ftp = new csFtp(ftpConString);
                if (ftp.connect())
                {
                    if (ftp.cwd(ftpRootPath))
                    {
                        ftp.put(Filename, newFName);
                    }
                    ftp.disconnect();
                }
            }
            catch { }
            return true;


        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getCurrData()
        {
            try
            {
                return (Object[])Session["current_data"];
            }
            catch
            { }
            return null;
        }

        private bool saveFile(HtmlInputFile HIF, String dir, out String errmsg)
        {
            errmsg = "";
            try
            {
                if (HIF.PostedFile.ContentLength > 0)
                {
                    String fileName = new csFileIo().getFilename(HIF.PostedFile.FileName);
                    HIF.PostedFile.SaveAs(dir + @"\" + fileName);
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                HIF = null;
            }
            return true;
        }

        private bool initWorkingDir(String TempDir)
        {
            csFileIo fio = new csFileIo();
            try
            {
                if (!fio.directoryExist(TempDir))
                {
                    fio.createDirectory(TempDir);
                }
            }
            catch
            {

            }
            finally
            {
                fio = null;
            }
            return true;
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            try
            {
                String FileSEQ = this.HPath.Value;
                String tempDir = "C:\\temp\\INTERNETCLIENT" + @"\" + FileSEQ;
                String errmsg = "";
                String errmsglist = "";
                String[] err = null;
                int errcount = 0;
                int critErrcount = 0;

                initWorkingDir(tempDir);

                if (!check_supporting_docs(File1.Value, out errmsg)) { err = errmsg.Split('|'); if (err[0] == "1") { critErrcount++; } else { errcount++; } errmsglist += err[1] + "\n"; }
                if (!check_supporting_docs(File2.Value, out errmsg)) { err = errmsg.Split('|'); if (err[0] == "1") { critErrcount++; } else { errcount++; } errmsglist += err[1] + "\n"; }
                if (!check_supporting_docs(File3.Value, out errmsg)) { err = errmsg.Split('|'); if (err[0] == "1") { critErrcount++; } else { errcount++; } errmsglist += err[1] + "\n"; }

                if (errcount == 3 || critErrcount > 0)
                {
                    throw new Exception(errmsglist);
                }

                errmsglist = "";

                if (!saveFile(File1, tempDir, out errmsg)) { errmsglist += errmsg + "/n"; };
                if (!saveFile(File2, tempDir, out errmsg)) { errmsglist += errmsg + "/n"; };
                if (!saveFile(File3, tempDir, out errmsg)) { errmsglist += errmsg + "/n"; };

                if (errmsglist != "")
                {
                    throw new Exception(errmsglist);
                }
            }
            catch (Exception ex)
            {
                Session["upload_status"] = ex.Message;
            }

            //Session["upload_status"] = "success";

            //else
            //{

            //}
        }

        private void getIPAddress()
        {
            String ip = Request.UserHostAddress;
            this.HIPAdd.Value = ip;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_hitlist(String FirstName, String LastName, String MiddleName, string branchCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            System.Data.SqlClient.SqlCommand sqcommands = null;
            irsMethaphone myMethaphone = new irsMethaphone();
            string strFirstNamePhonetics = "";
            string strLastNamePhonetics = "";
            string strMiddleNamePhonetics = "";

            Session["strNewEnrolledCIFPhonetics1"] = "";
            Session["strNewEnrolledCIFPhonetics2"] = "";
            Session["strNewEnrolledCIFPhonetics3"] = "";
            Session["strNewEnrolledCIFPhonetics4"] = "";
            Session["strNewEnrolledCIFPhonetics5"] = "";
            Session["strNewEnrolledCIFPhonetics6"] = "";
            Session["strNewEnrolledCIFPhonetics13"] = "";
            Session["strNewEnrolledCIFPhonetics14"] = "";

            string strNewEnrolledCIFPhonetics1 = "";
            string strNewEnrolledCIFPhonetics2 = "";
            string strNewEnrolledCIFPhonetics3 = "";
            string strNewEnrolledCIFPhonetics4 = "";
            string strNewEnrolledCIFPhonetics5 = "";
            string strNewEnrolledCIFPhonetics6 = "";
            string strNewEnrolledCIFPhonetics13 = "";
            string strNewEnrolledCIFPhonetics14 = "";

            strFirstNamePhonetics = myMethaphone.DoubleMethaphone(FirstName);
            strLastNamePhonetics = myMethaphone.DoubleMethaphone(LastName);
            strMiddleNamePhonetics = myMethaphone.DoubleMethaphone(MiddleName);

            strNewEnrolledCIFPhonetics1 = strFirstNamePhonetics + strMiddleNamePhonetics + strLastNamePhonetics;
            strNewEnrolledCIFPhonetics2 = strFirstNamePhonetics + strLastNamePhonetics + strMiddleNamePhonetics;
            strNewEnrolledCIFPhonetics3 = strLastNamePhonetics + strFirstNamePhonetics + strMiddleNamePhonetics;
            strNewEnrolledCIFPhonetics4 = strLastNamePhonetics + strMiddleNamePhonetics + strFirstNamePhonetics;
            strNewEnrolledCIFPhonetics5 = strMiddleNamePhonetics + strFirstNamePhonetics + strLastNamePhonetics;
            strNewEnrolledCIFPhonetics6 = strMiddleNamePhonetics + strLastNamePhonetics + strFirstNamePhonetics;
            strNewEnrolledCIFPhonetics13 = strLastNamePhonetics + strFirstNamePhonetics;
            strNewEnrolledCIFPhonetics14 = strFirstNamePhonetics + strLastNamePhonetics;

            Session["strNewEnrolledCIFPhonetics1"] = strNewEnrolledCIFPhonetics1.ToString();
            Session["strNewEnrolledCIFPhonetics2"] = strNewEnrolledCIFPhonetics2.ToString();
            Session["strNewEnrolledCIFPhonetics3"] = strNewEnrolledCIFPhonetics3.ToString();
            Session["strNewEnrolledCIFPhonetics4"] = strNewEnrolledCIFPhonetics4.ToString();
            Session["strNewEnrolledCIFPhonetics5"] = strNewEnrolledCIFPhonetics5.ToString();
            Session["strNewEnrolledCIFPhonetics6"] = strNewEnrolledCIFPhonetics6.ToString();
            Session["strNewEnrolledCIFPhonetics13"] = strNewEnrolledCIFPhonetics13.ToString();
            Session["strNewEnrolledCIFPhonetics14"] = strNewEnrolledCIFPhonetics14.ToString();

            try
            {
                if (commons != null)
                {
                    /*
                        @v_FirstName		varchar(200)
                        ,@v_LastName		varchar(80)
                        ,@v_MiddleName		varchar(80)
                        ,@v_BranchCode		varchar(4)
                     */
                    /* Hajie orig code
                    DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist", new Object[] { FirstName, LastName, MiddleName, branchCode }); 
                    */
                    DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist_validations", new Object[] { FirstName, LastName, MiddleName, branchCode, strNewEnrolledCIFPhonetics1, strNewEnrolledCIFPhonetics2, strNewEnrolledCIFPhonetics3, strNewEnrolledCIFPhonetics4, strNewEnrolledCIFPhonetics5, strNewEnrolledCIFPhonetics6, strNewEnrolledCIFPhonetics13, strNewEnrolledCIFPhonetics14 });
                    String hitlist = "";
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ((ds.Tables[0].Rows.Count > 5) ? 5 : ds.Tables[0].Rows.Count); i++)
                        {
                            hitlist += ds.Tables[0].Rows[i][1].ToString().ToUpper().Trim() + "\n";
                        }

                        if (ds.Tables[0].Rows.Count > 5)
                        {
                            hitlist = "\n" + hitlist + "\nShowing maximum of five(5) possible hits.\nPlease get complete list from your local compliance officer.\n";
                        }


                        return hitlist;
                    }
                }
                return "";
            }
            catch
            {
                return "";
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
    }
}

public static class StringExtensions
{
    public static string Right(string str, int length)
    {
        return str.Substring(str.Length - length, length);
    }
}