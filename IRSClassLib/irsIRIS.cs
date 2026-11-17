using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace IRSClassLib
{
    public class irsIRIS : sqlHelperWs
    {
    #region PRIVATE VARIABLES & METHODS
        private string strSQL = "select * from ";
        private string _param = null;
        private string _tble_name = "";
    #endregion        
    #region PROPERTY GET & SET
        public string table_name
        {
            get { return _tble_name; }
            set { _tble_name = value; }
        }

        public string param
        {
            get { return _param; }
            set { _param = value; }
        }
    #endregion
    #region PUBLIC METHODS
        public DataSet getDataSet()
        {            
            try
            {
                string str = this.strSQL + _tble_name+ _param;
              
                DataSet ds = base.getDataSet(base.getSqlCommand(str, false));
                try
                {
                    if (ds != null)
                    {
                        return ds;
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
        public irsIRIS()
        {
            this.IsWinApp = true;            
        }
    #endregion 
    }
}
