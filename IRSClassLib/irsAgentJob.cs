using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;

namespace IRSClassLib
{
    public class irsAgentJob : irsSqlCommons
    {
        DataTable dt            = new DataTable();
        String error_message    = String.Empty;
        csAuditLog oLog         = new csAuditLog();
        String sysid            = "IRSWorldAgent";

        public irsAgentJob()
        {
            this.error_message = null;
            base.IsWinApp = true;
        }


        private void run()
        {
            object _o;
            try
            {
                _o = base.execScalar("sp_ibatch_EOT", new object[] { });     

            }
            catch(Exception e) 
            {
                oLog.writeAudit(sysid, "irsAgentJob.run", "Error: " + e.StackTrace.ToString(), null,
                              null, csAuditLog.Category.FlexCube);
            }
            
        }

        public void startjob()
        {
            this.run();
        }
        ~irsAgentJob(){ }
    }
}

