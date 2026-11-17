using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld.IRIS
{
    public partial class IRISMain : System.Web.UI.Page
    {
        private string _tbl_name = "v_IRISview";
        private string _param = null;     
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(IRISMain));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        } 
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_details(string param)
        {
            
            try
            {
                this._param = param;
                DataSet ds = new DataSet();
                irsIRIS obj = this.doIRIS();
                obj.table_name = _tbl_name;
                obj.param = _param;
                ds = obj.getDataSet();

                if (ds != null)
                {
                    return ds;
                }
                else { return null; }        
            }
            catch
            {
                return null;   
            }

        }
        private irsIRIS doIRIS()
        {
            if (Session["IRIS"] == null)
            {
                Session["IRIS"] = new  irsIRIS();
            }
            return (irsIRIS)Session["IRIS"];
        }
    }

}
