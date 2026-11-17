using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsAPASTransactionStatus : sqlHelperWs
    {
        public string isOrd;

        public DataTable getAPASbyTransaction(int tranxcode,string branchcode,int cifclass,string whereclause,bool isOrd,string userid)
        {
            DataTable dt = execStoredProcedureTable("spAPAS_view_transactionstatus", new object[] { tranxcode, branchcode, cifclass, isOrd, whereclause,userid });
            return dt;
        }

        public DataSet getAPASInfo(string refno,int status)
        {
            DataSet ds = execStoredProcedure("spAPAS_view_transaction_details", new object[] { refno,status });
            return ds;
        }

        public DataSet getFullInfo(string refno)
        {
            DataSet ds = getDataSet(getSqlCommand("SELECT * FROM v_APAS_trxn WHERE ReferenceNo = '" + refno + "'", false));
            return ds;
        }


        public string CIFClass(string cifid)
        {
            object cif_class = execScalar("SELECT CIFClass FROM TBL_CIF_CLASS WHERE CIFID='" + cifid + "' AND ISNULL(Deleted,0) != 1");
            return cif_class.ToString();
        }

        public DataSet getFilter()
        {
            DataSet ds = getDataSet(getSqlCommand("SELECT * FROM TBL_LOOKUP WHERE FldGroup ='APASFilter'", false));
            return ds;  
        }

        public void getORDValue(string val)
        {
            this.isOrd = val;
        }

    }
}
