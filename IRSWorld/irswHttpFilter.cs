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

namespace IRSWorld
{
    public class irswHttpFilter : IHttpModule
    {
        public void Init(HttpApplication httpApp)
        {
            httpApp.AuthenticateRequest += new EventHandler(OnAuthentication);
        }

        private String[] get_module_wlist()
        {
            try
            {
                irsSqlCommonsWs comsw = new irsSqlCommonsWs(true);
                DataSet ds = comsw.getDataSet("SELECT * FROM tbl_module_wlist", false, new Object[] { });
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    String[] wlist = new String[ds.Tables[0].Rows.Count];
                    int i = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        wlist[i++] = row["ModuleName"].ToString();
                    }

                    return wlist;
                }
            }
            catch { }

            return null;
        }

        void OnAuthentication(object sender, EventArgs a)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            String ftype = System.IO.Path.GetExtension(context.Request.Url.AbsolutePath);
            if (ftype.Equals(".aspx") | ftype.StartsWith(".htm"))
            {
                String fname = System.IO.Path.GetFileName(context.Request.Url.AbsolutePath).ToLower();
                String[] mwlist = null;
                try
                {
                    if (context.Cache["IRSW_MODULE_WLIST"] != null)
                    {
                        mwlist = (String[])context.Cache["IRSW_MODULE_WLIST"];
                    }
                }
                catch (Exception e) 
                {
                    String s = e.Message;
                }
                if (mwlist == null)
                {
                    mwlist = get_module_wlist();
                    if (mwlist == null)
                    {
                        try
                        {
                            mwlist = new String[0];
                            context.Cache.Insert("IRSW_MODULE_WLIST", mwlist, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20));
                        }
                        catch(Exception ex) 
                        {
                            String sx = ex.Message;
                        }
                    }
                }

                if (mwlist != null && mwlist.Length > 0)
                {
                    for (int i = 0; i < mwlist.Length; i++)
                    {
                        if (mwlist[i].ToLower().Equals(fname))
                        {
                            context.Response.StatusCode = 403;
                            return;
                        }
                    }
                }
            }
        }

        public void Dispose()
        {
            //Do nothing here 
        }
    }
}
