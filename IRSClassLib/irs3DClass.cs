using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace IRSClassLib
{
    public class irs3DClass : sqlHelper
    {
        private String _message = "";

        public String message
        {
            get { return _message; }
        }

        public bool execProcedureUpdate(String proc, Object[] plist)
        {
            _message = "";
            try
            {
                if (base.execStoredProcedureUpdate(proc, plist) > 0)
                {
                    return true;
                }
                else
                {
                    throw new Exception("No record updated");
                }
            }
            catch (Exception ex)
            {
                _message = ex.Message;
                return false;
            }
        }
    }
}
