using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsGRSGEFU : sqlHelperWs
    {

    #region PRIVATE VARIABLE
        private string error_message = null;
        
    #endregion

    #region PUBLIC METHOD
        public bool create_grs_gefu(string tranxdate)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqlcommands = null;
            DataSet grsDataSet = new DataSet();

            try
            {
                if (commons != null)
                {
                    sqlcommands = commons.getSqlCommand("exec sp_grs_gefu '" + tranxdate + "'", false);
                    grsDataSet = commons.getDataSet(sqlcommands, "GRSGEFU");

                    if (grsDataSet.Tables[0].Rows.Count == 0)
                    {
                        error_message = "No Data Found";
                        return false;
                    }
                    else
                    { 
                        
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqlcommands != null)
                {
                    sqlcommands.Dispose();
                    sqlcommands = null;
                }
            }
            return true;
        }
    #endregion

    }
}
