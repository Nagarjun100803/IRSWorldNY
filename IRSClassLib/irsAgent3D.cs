using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using GRSMQ;


namespace IRSClassLib
{ 
    public class irsAgent3D : irsSqlCommons
    {
        #region PRIVATE VARIABLES & METHODS

        private string error_message = null;
        private string _api_message = null;
        private string _ok = string.Empty;
        private string _failed = string.Empty;
        public csFlexcubeTeller o = new csFlexcubeTeller();
        private Hashtable threadholder = new Hashtable();
        private string maxthread;

        private DataTable Get_data()
        {
            try
            {
                SqlCommand sqcommands = base.getSqlCommand("sp_get_3D_credit");
                DataSet ds = base.getDataSet(sqcommands, "FOR_CREDITTING");
                if (ds != null)
                {
                    return ds.Tables[0];
                }
            }
            catch { this.get_db_err(); }
            return null;
        }

        
        #endregion

        #region PROPERTY GET & SET
        public string messages
        {
            get { return this.error_message; }
            set { this.error_message = value; }
        }

        public string api_message
        {
            get { return this._api_message; }
            set { this._api_message = value; }
        }

        public string max_thread
        { set { maxthread = value; } }

        private string ok
        { get { return _ok; } }

        private string failed
        { get { return _failed; } }

        private bool checkthread(string key)
        {
            if (threadholder.Count < 1) return true;

            IDictionaryEnumerator en = threadholder.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Key.ToString() == key.ToString())
                {
                    if (this.threadIsAlive(en.Key)) { return false; }
                }
            }
            return true;
        }

        private bool threadIsAlive(object key)
        {
            foreach (Thread t in threadholder.Values)
            {
                if (t.Name == key.ToString())
                {
                    if (t != null && t.IsAlive) return true;
                }
            }
            return false;
        }

        private void release_threads()
        {
            int count = threadholder.Count;
            int rel = 0;
            while (rel <= count)
            {
                foreach (Thread t in threadholder.Values)
                {
                    if (t != null && !t.IsAlive) { t.Abort(); rel++; }
                }
            }
            threadholder.Clear();
        }
        #endregion PROPERTY GET & SET

        #region PUBLIC METHODS
        public irsAgent3D()
        {
            this.error_message = null;
            base.IsWinApp = true;
        }
        public void get_db_err()
        {
            messages = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                messages = base.Message;
            }
        }
        public bool start_credit()
        {
            int seq_id = 0;            
            string acct_num = string.Empty;
            string acct_curr = string.Empty;
            string amount = string.Empty;
            string apb_code = string.Empty;            
            string gl_acct = string.Empty;
            bool ibool = false;
            try
            {
                DataTable dt = Get_data();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            seq_id      = Convert.ToInt32(row["seq_id"]);
                            acct_num    = (string)row["acct_num"].ToString();
                            acct_curr   = (string)row["acct_curr"].ToString();
                            amount      = (string)row["amount"].ToString();
                            apb_code    = (string)row["apb_code"].ToString();                            
                            gl_acct     = (string)row["gl_acct"].ToString();
                            api_message = string.Empty;

                            if (threadholder.Count > int.Parse(maxthread))
                            {
                                this.release_threads();
                            }
                            while (!checkthread(acct_num)) { }
                            fccredit3D fc = new fccredit3D(acct_num, acct_curr, amount, apb_code, gl_acct, seq_id);
                            Thread t = new Thread(fc.do_iBatch_3D);
                            t.Name = acct_num;
                            if (!threadholder.ContainsKey(acct_num)) { threadholder.Add(acct_num, t); }
                            t.Start();   
                           // fc.do_iBatch_3D();                        
                        }
                    }
                }
                if (messages == null)
                {
                    ibool = true;
                }
            }
            catch (Exception er)
            { throw er; }
            finally { o = null; }
            return ibool;

        }
        #endregion
    }

    public class fccredit3D : irsSqlCommons
    {
        #region variables
        string sysid = "IRSWorldAgent";
        string mdesc = "Crediting 3D to FC";

        string error_message = null;
        string _api_message = null;
        string _ok = "**Successfully Processed: ";
        string _failed = "**Failed to credit: ";        
        string _narrative = string.Empty;
        string _acct_num = string.Empty;
        string _acct_curr = string.Empty;
        string _amount = string.Empty;
        string _apb_code = string.Empty;            
        string _gl_acct = string.Empty;
        int _seq_id = 0;
        string narrative = string.Empty;
        csFlexcubeTeller o = new csFlexcubeTeller();
        csAuditLog oLog = new csAuditLog();
        #endregion variables

        #region public method
        public fccredit3D(string acct_num, string acct_curr, string amount, string apb_code, string gl_acct, int seq_id)
        {            
            _seq_id     = seq_id;
            _acct_num   = acct_num;
            _acct_curr  = acct_curr;
            _amount     = amount;
            _apb_code   = apb_code;
            _gl_acct    = gl_acct;
            _narrative  = "IRS (" + narrative + ")";
            narrative   = seq_id.ToString();
            this.error_message = null;
            base.IsWinApp = true;
        }
        #endregion public method

        #region IBatch Region

        public void do_iBatch_3D()
        {
            object[] ob = new object[4];
            int i       = 9; //error
            bool ibool  = false;            
            string s    = string.Empty;
            lock (this)
            {

                if (ibool = this.iBatch_3D(_seq_id.ToString(), _acct_num, _gl_acct, _amount, _acct_curr, _narrative, _apb_code))
                { 
                    i = 2;// credited online
                    s = this.ok.ToString(); 
                }
                else 
                { 
                    s = this.failed.ToString(); 
                }

                ob[0] = _seq_id;
                ob[1] = i;
                ob[2] = s + api_message + messages + "seq_id=" + _seq_id;                

                oLog.writeAudit(sysid, "fccredit3D.iBatch_3D", mdesc, "api_message=" + api_message +
                                        messages + " refnum=" + _seq_id + "targetAcct=" + _acct_num +
                                        "sourceAcct=" + _gl_acct + "amt=" + _amount + "actCur=" + _acct_curr +
                                        "narrative=" + _narrative + "", "", csAuditLog.Category.FlexCube);

                this.update_txn(ob);
            }
            Thread.Sleep(2000);
            return;
        }

        private bool iBatch_3D(string kbiRef, string act, string kbiGL,
                                    string amtBene, string actCur, string refnum, string apbcode)
        {

            string fc_msg = string.Empty;
            bool result = false;


            try
            {
                if (apbcode == "2572") // Debit
                {
                    result = o.debitGlCreditCasa(kbiRef, act, kbiGL, amtBene, actCur, refnum, out fc_msg);
                }
                else if (apbcode == "2372") // Credit
                {
                    result = o.debitCasaCreditGl(kbiRef, act, kbiGL, amtBene, actCur, refnum, out fc_msg);
                }
            }
            catch (Exception e)
            {
                messages = e.Message + ' ' + e.StackTrace;
                return result;
            }
            api_message = fc_msg;
            return result;
        }
       
        #endregion  IBatch Region

        #region Common method
        private void get_db_err()
        {
            messages = string.Empty;
            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                messages = base.Message;
            }
        }
        private void update_txn(object[] ob)
        {
            object _o;

            try
            {
                _o = base.execScalar("sp_update_3D_credit", ob);
                if (_o == null)
                { messages = base.Message; }
            }
            catch (Exception er)
            {
                oLog.writeAudit(sysid, "fccredit3D.update_txn", mdesc, er.Message.ToString(),
                                  "", csAuditLog.Category.FlexCube);
                throw er;
            }
        }  
        #endregion Common method

        #region Get/Set Property
        public string messages
        {
            get { return this.error_message; }
            set { this.error_message = value; }
        }
        public string api_message
        {
            get { return this._api_message; }
            set { this._api_message = value; }
        }
        private string ok
        { get { return _ok; } }

        private string failed
        { get { return _failed; } }

        #endregion Get/Set Property

        ~fccredit3D() { }
    }
    
}
