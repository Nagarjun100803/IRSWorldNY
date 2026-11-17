using System;
using System.Collections;
using System.Data;
using System.Text;
using GRSMQ;

namespace IRSClassLib
{
    public class testmq
    {
        // test acct: 358717200433
        public Object getAccountDetails(String acctno, out String msg)
        {
            return this.getAccountDetails("default", acctno, out msg);
        }

        public Object getAccountDetails(String fcrid, String acctno, out String msg)
        {
            msg = "";
            csFlexcubeTeller teller = (fcrid.ToLower().Equals("default")) ? new csFlexcubeTeller() : new csFlexcubeTeller(fcrid);
            Hashtable ht = teller.getAccountDetails(acctno, out msg);
            //String s = teller.getAccountDetails(acctno);
            teller.close();

            return ht;
        }

        public DataTable getSettingIDs(out String msg)
        {
            msg = "";
            irsSqlCommonsWs com = new irsSqlCommonsWs();
            DataSet ds = com.getDataSet(com.getSqlCommand("SELECT DISTINCT SettingsID as fldValue, SettingsID as fldDesc FROM TBL_FCR_SETTINGS", false));
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }

            msg = com.Message;

            return null;
        }
    }
}
