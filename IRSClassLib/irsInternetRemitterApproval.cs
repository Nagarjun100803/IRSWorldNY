using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
    public class irsInternetRemitterApproval:sqlHelperWs
    {
        public DataTable getRemitter()
        {
            base.EnforceConstraint = false;
            getUpdateDS("remitterForApproval", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            { return null; }
        }

        public void updateDocument(string cifid, string option)
        {
            base.EnforceConstraint = false;
            base.execStoredProcedureUpdate("UpdateCIFDocuments", new Object[] { cifid, option });
        }





    }
}
