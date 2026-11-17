using System;
using System.Collections;
using System.Text;

using GRSMQ;

namespace IRSClassLib
{
    [Serializable]
    public class csFlexcube
    {
        public String Message;
        public csFlexcubeBasicTeller bfc;
        public csFlexcubeTeller fc;
        public String settings_id = "default";

        public Hashtable getFcAcctDetailsFull(String acctno, out String errmsg)
        {
            Hashtable ht = null;
            errmsg = "";
            try
            {
                if (fc == null)
                {
                    fc = (settings_id.Equals("default")) ? new csFlexcubeTeller() : new csFlexcubeTeller(settings_id);
                }
                ht = fc.getAccountDetails(acctno, out errmsg);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return ht;
        }

        public Hashtable getFcAcctDetails(String acctno, out String errmsg)
        {
            Hashtable ht = null;
            errmsg = "";
            try
            {
                if (bfc == null)
                {
                    bfc = (settings_id.Equals("default")) ? new csFlexcubeBasicTeller() : new csFlexcubeBasicTeller(settings_id);
                }
                ht = bfc.getAccountDetails(acctno, out errmsg);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return ht;
        }

        public String getFcAcctDetails(String acctno)
        {
            Message = String.Empty;
            String s = null;
            try
            {
                if (bfc == null)
                {
                    bfc = (settings_id.Equals("default")) ? new csFlexcubeBasicTeller() : new csFlexcubeBasicTeller(settings_id);
                }
                s = bfc.getAccountDetails(acctno);
                if (s.StartsWith("ERROR"))
                {
                    Message = s;
                    s = null;
                }
                else if (s == null || s == String.Empty)
                {
                    Message = "ERROR: no response from mq";
                    s = null;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return s;
        }

        public String getFcDetail(Hashtable ht, String tag)
        {
            if (ht.ContainsKey(tag)) return (String)ht[tag];

            ICollection keys = ht.Keys;
            foreach (Object key in keys)
            {
                String skey = key.ToString().ToUpper();
                if (skey.Equals(tag.ToUpper()))
                {
                    return (String)ht[key];
                }
            }
            Message = "ERROR: TAG " + tag + " not found.";
            return null;
        }

        public String getFcDetail(String fcdtls, String tag)
        {
            try
            {
                String[] ss = fcdtls.Split('|');
                foreach (String s in ss)
                {
                    String[] tt = s.Split('=');
                    if (tag.Trim().ToUpper().Equals(tt[0].Trim().ToUpper())) return tt[1];
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }

        public int getFcAcctType(String fcdtls)
        {
            if (fcdtls == null || fcdtls == String.Empty) return 0;
            try
            {
                return int.Parse(getFcDetail(fcdtls, "ACCOUNTTYPE").Trim());
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return 0;
        }

        //===============================
        /* FOR FEDACH STREAMLINING */
        public Hashtable getAcctDetailsSC(String acctno, out String errmsg)
        {
            Hashtable ht = null;
            errmsg = "";
            try
            {
                if (bfc == null)
                {
                    bfc = (settings_id.Equals("default")) ? new csFlexcubeBasicTeller() : new csFlexcubeBasicTeller(settings_id);
                }
                ht = bfc.getAccountDetailsSC(acctno, out errmsg);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }
            return ht;
        }
        //===============================

        public csFlexcube()
        {}

        ~csFlexcube()
        {
            try
            {
                if (bfc != null)
                {
                    bfc.close();
                    bfc = null;
                }
                if (fc != null)
                {
                    fc.close();
                    fc = null;
                }
            }
            catch { }
        }
    }
}