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

namespace IRSWorld.CIF
{
	public partial class CIFViewDetails : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AjaxPro.Utility.RegisterTypeForAjax(typeof(CIFViewDetails));
			AjaxPro.Utility.RegisterTypeForAjax(typeof(cifmain));
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_master_record(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cifview_get_master_record");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 20).Value = cifid; 

					return commons.getDataSet(sqcommands, "CIF_MASTER_RECORD");
				}
				return null;
			}
			catch
			{
				return null;
			}
			finally
			{
				if (commons != null) { commons = null; }
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_reminfo_record(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cifview_get_reminfo_record");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 20).Value = cifid;

					return commons.getDataSet(sqcommands, "CIF_REMINFO_RECORD");
				}
				return null;
			}
			catch
			{
				return null;
			}
			finally
			{
				if (commons != null) { commons = null; }
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_ciftype(string seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_ciftype");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_MASTER_CIFTYPE");
				}
				return null;
			}
			catch
			{
				return null;
			}
			finally
			{
				if (commons != null) { commons = null; }
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int get_cifclass_count(int seq_id, int cif_class)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cifclass_count");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					sqcommands.Parameters.Add("@CIFCLASS", SqlDbType.Int, 100).Value = cif_class;

					return Convert.ToInt32(commons.getDataSet(sqcommands, "CIF_MASTER_SEQ_ID").Tables[0].Rows[0][0]);
				}
				return 0;
			}
			catch
			{
				return 0;
			}
			finally
			{
				if (commons != null) { commons = null; }
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_cif_civilstatus(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();

			SqlCommand command = helper.getSqlCommand("sp_cif_get_cifmaster");
			command.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = SEQ_ID;
			DataTable dt = helper.getDataSet(command, "CIF_CIVILSTATUS").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["CIVILSTATUS"]);
			}

			return "0";
		}

	}//end of class CIFViewDetails
}
