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
using IRSClassLib;
using System.Threading;
using IRSWorld.APAS;
using System.Data.SqlClient;


namespace IRSWorld
{
    public partial class testAgent : System.Web.UI.Page
    {
             
        
        protected void Page_Load(object sender, EventArgs e)
        {
         
            
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            try
            {
               // this.run();
                irsAgentOTC obj = new irsAgentOTC();                
                obj.max_thread = "7";
                if (!obj.start_credit())
                {
                    Response.Write(obj.messages);
                }
            }
            catch(Exception er)
            { throw er; }            
        }
        static bool stop = false;
       
        #region PRIVATE METHODS
        private string interval = System.Configuration.ConfigurationManager.AppSettings["Agentinterval"];
        private string SMSConnectionString = System.Configuration.ConfigurationManager.AppSettings["SMSConnectionString"];
//        private string SMSConnectionString = "server=ibsdb2;Trusted_Connection=false;database=db_SAS;User=sa;Password=ibsdb2admin1";
        private Thread o;
        private void run()
        {
            try
            {
                o = new Thread(new ThreadStart(start_agent));                
                o.Start();                
            }
            catch
            {
                return;
            }
        }

        private void start_agent()
        {
            try
            {
                irsAgent obj = new irsAgent();
                while (o == Thread.CurrentThread)
                {
                    if (!stop)
                    {
                        obj.start_credit();
                        Thread.Sleep(Int32.Parse("20") * 1000);
                    }
                    else { return; }
                }
            }
            catch
            {
                this.run();
            }
            finally
            {

            }
        }
        #endregion


        
        protected void Button2_Click1(object sender, EventArgs e)
        {
           DateTime dt = Convert.ToDateTime(this.TextBox1.Text.ToString());

           string f = "PNBBOC" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".txt";
            irsMT103 o = new irsMT103(this.TextBox1.Text.ToString(),f,"c:\\");
            o.extract();

        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            irsAgentJob o = new irsAgentJob();
            o.startjob();
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            csSMS sms = new csSMS();
            sms.connstringSMS = SMSConnectionString;
            /*
            if (!sms.sendSMS("IRS", "1", "09191234567", "test"))
            {
               string s = sms.Error.ToString();
            }
            */
            if (!sms.startRemitSMS()) 
            {
                Response.Write(sms.Error);
            }
            
        }
        protected void Button6_Click(object sender, EventArgs e)
        {
            csSMS sms = new csSMS();
            sms.connstringSMS = SMSConnectionString;
            //if (!sms.sendSMS(1, "IBS", "1", "0918123456789", "test"))
            // {
            //    string s = sms.Error.ToString();
            // }
            /*
            if (!sms.startRemitSMS()) 
            {
                Response.Write(sms.Error);
            }
            */
        }
        protected void Button5_Click(object sender, EventArgs e)
        {
            irsAgent3D o = new irsAgent3D();
            o.max_thread = "100";

            o.start_credit();
        }

        protected void Button6_Click1(object sender, EventArgs e)
        {
            irsMailer mail = new irsMailer();
            mail.startMail();
        }

        protected void Button7_Click(object sender, EventArgs e)
        {
            irsAgentCreditOtherTrxn oc = new irsAgentCreditOtherTrxn();
            oc.start();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ThreadKill.Thread_Kill = true;
        }
    }

    
}
