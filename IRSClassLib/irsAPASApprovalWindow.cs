using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsAPASApprovalWindow : sqlHelperWs
    {
        public irsAPASApprovalWindow()
        { 
            
        }
        public DataTable getTables(int tbl_indx)
        {
            DataSet ds = execStoredProcedure("spAPASSetupForApproval", new object[] { tbl_indx });
            return (ds.Tables.Count > 0) ? ds.Tables[0] : null;
        }
    }
}
