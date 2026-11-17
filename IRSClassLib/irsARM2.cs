using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsARM2 : sqlHelper
    {
        irsClientData cd = new irsClientData();
        csAuditLog logger = new csAuditLog();

        public string getGFCRemitter(string CardNo)
        {
            base.EnforceConstraint = false;
            getUpdateDS("ksp_getGFCRemitter", new Object[] { CardNo });
            if (hasRows)
            {
                selectedRow = 0;
                return table.Rows[0][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}