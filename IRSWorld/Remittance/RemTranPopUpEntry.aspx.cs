using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;

namespace IRSWorld.Remittance
{
    public partial class RemTranPopUpEntry : System.Web.UI.Page
    {
          protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranPopUpEntry));
        }

        #region AJAX PUBLIC METHOD


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_bank(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_RemTranBank");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "RemTranSWIFT");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }

            return null;

        }

        
        #endregion
    }

    }

