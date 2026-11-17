using System;
using System.Data;
using System.Net;
using System.Net.Mail;

namespace IRSClassLib
{
	/// <summary>
	/// Summary description for csMailer.
    /// CHR 2009
	/// </summary>
    [Serializable]
	public class csMailer
	{
        private String host;
        private int port;
        private String user;
        private String password;
        private MailAddress from;

        private bool use_credential;
        private bool enable_ssl;
        private int timeout;

        private String _message;

        public String Message
        {
            get { return _message; }
        }

        public bool sendMail(MailMessage mail)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                if (use_credential) 
                { 
                    client.Credentials = new System.Net.NetworkCredential(user, password); 
                }
                client.Host = host;
                if (port > -1) client.Port = port;
                if (timeout > -1) client.Timeout = timeout;
                client.EnableSsl = enable_ssl;
                client.Send(mail);
                mail.Dispose();
                client = null;
                mail = null;
                return true;
            }
            catch (Exception ex)
            {
                mail.Dispose();
                mail = null;
                _message = "csMailer: " + ex.Message + " " + ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    _message += " " + ex.InnerException.InnerException.Message;
                }
            }

            return false;
        }

        public void setAttachments(MailMessage mail, String[] attachs)
        {
            foreach (String s in attachs)
            {
                try
                {
                    Attachment a = new Attachment(s);
                    mail.Attachments.Add(a);
                    System.Net.Mime.ContentType c = a.ContentType;
                    c.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                }
                catch (Exception e)
                {
                    _message = "csMailer: unable to attach " + s + " - " + e.Message;
                }
            }
        }

        public bool sendMail(String subject, String[] recipient, String[] cc, String msg_text, String[] attachs)
        {
            // for no html and no bcc
            return sendMail(subject, recipient, cc, msg_text, attachs, false, null);
            // deleted redundant codes.
        }

        public bool sendMail(String subject, String[] recipient, String[] cc, String msg_text, String[] attachs, bool html)
        {
            // for html with no bcc
            return sendMail(subject, recipient, cc, msg_text, attachs, html, null);
            // deleted redundant codes.
        }

        public bool sendMail(String subject, String[] recipient, String[] cc, String msg_text, String[] attachs, bool html, String[] bcc)
        {
            try
            {
                MailMessage mail = new MailMessage();

                foreach (String to in recipient)
                {
                    if (to != "")
                    {
                        mail.To.Add(to);
                    }
                }

                if (cc != null && cc.Length > 0)
                {
                    foreach (String c in cc)
                    {
                        mail.CC.Add(c);
                    }
                }

                if (bcc != null && bcc.Length > 0)
                {
                    foreach (String bc in bcc)
                    {
                        mail.Bcc.Add(bc);
                    }
                }

                mail.From = from;
                mail.Subject = subject;
                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                mail.Body = msg_text;
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = html;
                mail.Priority = MailPriority.High;

                if (attachs != null && attachs.Length > 0)
                {
                    setAttachments(mail, attachs);
                }

                return sendMail(mail);
            }
            catch (Exception ex)
            {
                _message = "csMail: " + ex.Message;
            }
            return false;
        }

        public bool sendMail(String subject, String[] recipient, String msg_text)
        {
            return this.sendMail(subject, recipient, msg_text, null);
        }

        public bool sendMail(String subject, String[] recipient, String msg_text, String[] attachs)
        {
            return this.sendMail(subject, recipient, null, msg_text, attachs);
        }

        public bool sendMail(String subject, String[] recipient, String[] cc, String msg_text)
        {
            return this.sendMail(subject, recipient, cc, msg_text, null);
        }

        public string[] fileEmailRecipient(int _filetype, int _fileformat, bool process, bool upload)
        {
            object[] param = new object[] { _filetype, _fileformat, process, upload };
            try
            {
                DataTable dt = new sqlHelperWs().execStoredProcedureTable("sp_get_email", param);

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

        private void parse_connection()
        {
            port = -1;
            use_credential = false;
            enable_ssl = false;
            timeout = -1;

            string host_value = "";

            //ITG password encrypt
            //String constring = new irsConfiguration().appsetting("SMTPConnection");
            String constring = new irsConfiguration().ConnectionStringNoRunMode("SMTPConnection");
            if (constring != null && constring.Length > 0)
            {
                String[] ss = constring.Split(';');
                foreach (String s in ss)
                {
                    String[] tt = s.Split('=');
                    if (tt[0].ToUpper().Equals("SERVER")) try { host = tt[1]; host_value = tt[1]; } catch { }
                    if (tt[0].ToUpper().Equals("PORT")) try { port = int.Parse(tt[1]); } catch { }
                    if (tt[0].ToUpper().Equals("USER")) try { user = tt[1]; } catch { }
                    if (tt[0].ToUpper().Equals("PASSWORD")) try { password = tt[1]; } catch { }
                    if (tt[0].ToUpper().Equals("USE_CREDENTIAL")) try { use_credential = bool.Parse(tt[1]); } catch { }
                    if (tt[0].ToUpper().Equals("USE_SSL")) try { enable_ssl = bool.Parse(tt[1]); } catch { }
                    if (tt[0].ToUpper().Equals("TIMEOUT")) try { timeout = int.Parse(tt[1]); } catch { }
                }
            }

            //if (host == null || host == String.Empty) host = "mail.pnb.com.ph";
            if (host == null || host == String.Empty) host = host_value;

            try
            {
                from = new MailAddress(new irsConfiguration().appsetting("MailFrom"));
            }
            catch
            {
                from = new MailAddress("irsworld@pnb.com.ph");
            }
        }

		public csMailer()
		{
            parse_connection();
        }

        ~csMailer()
        {
            host = null;
            user = null;
            password = null;
            from = null;
        }
	}
}
