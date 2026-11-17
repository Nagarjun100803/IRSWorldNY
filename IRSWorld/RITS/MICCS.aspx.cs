using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using IRSWorld.TranClass;
namespace IRSWorld.RITS
{
    public partial class MICCS : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(MICCS));

            //if (HttpContext.Current.Session["USER_ON_LOGIN"] == null)
            //{
            //    HttpContext.Current.Session["USER_ON_LOGIN"] = true;
            //}
        }


        private irsMICCS irsMICCS
        {
            get
            {
                if (Session["IRS_MICCS"] == null)
                {
                    Session["IRS_MICCS"] = new irsMICCS();
                }

                return (irsMICCS)Session["IRS_MICCS"];
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String getText(String Text)
        {
            return "Success " + Text;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds)
        {


            char sep = '^';
            bool deleted;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds};
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    //if (s.Length > 0)
                    //{
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    for (int i = 1; i < tt.Length; i++)
                    {
                        String[] uu = tt[i].Split('|');
                        int row = int.Parse(uu[0]);
                        Hashtable ht = new Hashtable();
                        for (int j = 1; j < uu.Length; j++)
                        {
                            String[] cols = uu[j].Split(sep);
                            if (cols.Length < 2) continue;
                            {
                                ht.Add(cols[0], cols[1]);
                            }
                        }
                        if (tt[0] == "delete") { deleted = true; } else { deleted = false; }
                        bool ok = irsMICCS.save_data(ht, deleted);
                        if (!ok)
                        {
                            throw new Exception("Save Changes: " + irsMICCS.Message);
                        }
                    }
                    //}
                }
            }
            if (errs.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Object msg in errs)
                {
                    sb.Append(msg.ToString() + "\n");
                }
                throw new Exception(sb.ToString());
            }

            if (!irsMICCS.saveUpdates())
            {
                throw new Exception("Save changes: " + irsMICCS.Message);
            }

            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getStatus(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("SELECT FldDescription FROM TBL_LOOKUP WHERE FldGroup = '" + fldgroup + "' and FldValue <> 3", false);

                    return commons.getDataSet(sqcommands, "Status");
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

            
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public int processMICCS()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand command = commons.getSqlCommand("sp_process_miccs");

            try
            {
                int i = commons.sqlUpdate(command); return i;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }

                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet checkMICCS()
        {
            DataSet ds = null;
            DataSet dsP = null;
            DataSet ds2 = null;

            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();

                String SQL = "select * from tbl_miccs where remittercifid = '' and status = 1";
                ds = com.getDataSet(com.getSqlCommand(SQL, false), "tbl_miccs");

                String SQL2 = "select * from tbl_miccs where remittercifid <> '' and status = 1";
                ds2 = com.getDataSet(com.getSqlCommand(SQL2, false), "tbl_miccs");


                if (ds.Tables[0].Rows.Count != 0)
                {
                    throw new Exception("Incomplete name conversion.");
                }
                else
                {
                    if (ds2.Tables[0].Rows.Count == 0)
                    {
                        throw new Exception("No item to process.");
                    }
                    else
                    {
                        dsP = com.getDataSet(com.getSqlCommand("sp_miccs_summary"));
                        return dsP;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error processing items, Reason : " + ex.Message);
            }
            finally
            {
                ds = null;
                dsP = null;
                ds2 = null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataSet getConfirmationList()
        {
            DataSet ds = null;
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();

                ds = com.getDataSet (com.getSqlCommand("proc_get_conflist"));
                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting confirmation list, Reason : " + ex.Message);
            }
            finally
            {
                ds = null;
            }
            
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_remitter(string seq_id)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_get_remitter_fullname");
                    sqcommands.Parameters.Add("@seq_id", SqlDbType.BigInt ).Value = Convert.ToInt32(seq_id);

                    return commons.getDataSet(sqcommands, "CIFRemitter");
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
