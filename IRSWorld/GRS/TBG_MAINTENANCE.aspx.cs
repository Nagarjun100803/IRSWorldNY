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

namespace IRSWorld.GRS
{
    public partial class TBG_MAINTENANCE : System.Web.UI.Page
    {
        private irsTBGMaintenance irsTBGMaintenance
        {
            get
            {
                if (Session["TBG_Maintenance"] == null)
                {
                    Session["TBG_Maintenance"] = new irsTBGMaintenance();
                }
                return (irsTBGMaintenance)Session["TBG_Maintenance"];
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(TBG_MAINTENANCE));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getRecordID(string fldgroup)
        {
            sqlHelper commons = new sqlHelper();
            SqlCommand sqcommands = null;

            sqcommands = commons.getSqlCommand("sp_getFldCodeDescription");
            sqcommands.Parameters.Add("@fldgroup", SqlDbType.VarChar, 100).Value = fldgroup;

            return commons.getDataSet(sqcommands, "tblRecordID");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCurrency(string fldgroup)
        {
            return irsTBGMaintenance.getCurrency(fldgroup);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable GetGridValue(string recordID)
        {
            return irsTBGMaintenance.GetGridValue(recordID);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
       // public bool applyEdits(String adds, String eds, String recordID)
        public Object applyEdits(String adds, String eds, String recordID)
        {
                    int norec = 0;
                    int row = 0;
                    int seq_id = 0;
                    Hashtable ht = new Hashtable();
            char sep = '^';
            bool isEdited;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
                    String[] tt = s.Split('~');
                    String crud = tt[0].ToUpper();
                    
                    for (int i = 1; i < tt.Length; i++)
                    {
                         String[] uu = tt[i].Split('|');
                         if ((uu[0].ToString().Length == 0))
                         {
                             norec++;
                         }
                         else
                         {
                             row = int.Parse(uu[0].Replace(",", ""));

                             for (int j = 1; j < (uu.Length - norec); j++)
                             {

                                 String[] cols = uu[j].Split(sep);
                                 if (cols.Length < 2)
                                 {
                                     //ht.Add(cols[0], "");
                                     continue;
                                 }

                                 ht.Add(cols[0], cols[1]);
                             }
                         }
                        if (tt[0] == "edit") { isEdited = true; } else { isEdited = false; }
                        //bool ok = irsTBGMaintenance.save_TBGMaintenance(ht, recordID, isEdited);
                        bool ok = irsTBGMaintenance.applyUpdates(crud, row, ht, recordID);
                        if (!ok)
                        {
                            throw new Exception("Save Changes: " + irsTBGMaintenance.Message);
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

            //if (!irsTBGMaintenance.saveUpdates())
            //{
            //    throw new Exception("Save changes: " + irsTBGMaintenance.Message);
            //}

            return irsTBGMaintenance.saveUpdates(seq_id);

           // return true;
        }
    }
}
