using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld.IRIS
{
    public class IRIS:irsSqlCommons
    {               
        public DataTable getDataTable(String SQL)
        {
            try
            {               
                DataSet ds = base.getDataSet(base.getSqlCommand(SQL, false));
                try
                {
                    if (ds != null)
                    {
                        return ds.Tables[0];
                    }
                    return null;
                }
                catch(Exception e)
                {
                    throw e;
                }
                finally 
                {
                    ds = null;                
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }
        public IRIS()
        {
            this.IsWinApp = true;
        }
    }
}
