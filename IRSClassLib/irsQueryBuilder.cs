using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsQueryBuilder : sqlHelper
    {
        public DataTable getOverseasBranch()
        {
            base.EnforceConstraint = false;
            getUpdateDS("qsp_getOverseasBranch", new Object[] { });
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

        public DataTable getDomesticBranch()
        {
            base.EnforceConstraint = false;
            getUpdateDS("qsp_getDomesticBranch", new Object[] { });
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

        public DataTable getAgentBranch()
        {
            base.EnforceConstraint = false;
            getUpdateDS("qsp_getAgentBranch", new Object[] { });
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
    }
}
