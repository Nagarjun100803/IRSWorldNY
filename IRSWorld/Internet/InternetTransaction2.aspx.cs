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

namespace IRSWorld.Internet
{
	public partial class InternetTransaction2 : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetTransaction2));
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_profile(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_internet_txn_remprofile");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 20).Value = cifid;
					return commons.getDataSet(sqcommands, "REM_PROFILE");
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

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet getdropdown_source_benename(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_internet_txn_getbene");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 20).Value = cifid;
					return commons.getDataSet(sqcommands, "CIF_BENE");
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
	}
}
