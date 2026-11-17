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
	public partial class CIFGefu : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			AjaxPro.Utility.RegisterTypeForAjax(typeof(CIFGefu));
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string create_cifgefu(string date)
		{
			irsCIFGEFU gefu = new irsCIFGEFU();
			return gefu.main(date);
		}
	}
}
