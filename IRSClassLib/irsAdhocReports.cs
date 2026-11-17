using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsAdhocReports : sqlHelperWs
    {
        public DataTable getReportDDL()
        {
            base.EnforceConstraint = false;
            base.IsWinApp = true;
            getUpdateDS("rsp_getAdhocReport", new Object[] { });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getAdhocReports(string spName, string dtFrom, string dtTo)
        {
            base.EnforceConstraint = false;
            base.IsWinApp = true;
            getUpdateDS(spName, new Object[] { dtFrom, dtTo });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return new DataTable();
            }
        }
    }
}
