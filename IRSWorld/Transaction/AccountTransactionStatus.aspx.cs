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

namespace IRSWorld.Transaction
{
    [Serializable]
    public partial class AccountTransactionStatus : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["dtOldRecord"] = null;
            AjaxPro.Utility.RegisterTypeForAjax(typeof(AccountTransactionStatus));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getRecord()
        {
            DataTable dtReturn = new DataTable();
            dtReturn = new irsAccountTransactionStatus().getRecord();

            Session["dtOldRecord"] = dtReturn;
            return dtReturn;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string editData(String adds, String dels, String eds)
        {
            char sep = '^';
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            int norec = 0;
            
            string msg = "";
            string concatOldValue = "";
            DataTable pDT = new DataTable();
            
            if (Session["dtOldRecord"] != null)
            {
                pDT = (DataTable)Session["dtOldRecord"];
            }

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
                            int row = int.Parse(uu[0].Replace(",", ""));
                            Hashtable ht = new Hashtable();
                            for (int j = 0; j < (uu.Length - norec); j++)
                            {
                                String[] cols = uu[j].Split(sep);
                                if (cols.Length < 2)
                                {
                                    ht.Add(cols[0], "");
                                    continue;
                                }
                                ht.Add(cols[0], cols[1]);
                            }
                            //SEQ_ID of the specific record to be saved
                            int pSeqid = Convert.ToInt32(ht["SEQ_ID"]);

                            if (pDT != null)
                            {
                                int rowCount = 0;
                                for (int ctr = 0; ctr < pDT.Rows.Count; ctr++)
                                {
                                    if (pSeqid == Convert.ToInt32(pDT.Rows[ctr]["SEQ_ID"]))
                                    {
                                        rowCount = ctr;
                                        break;
                                    }
                                }
                                //Old Values of the speific record (as per SEQ_ID) before update 
                                Boolean IsValidated = Convert.ToBoolean(pDT.Rows[rowCount]["IsValidated"]);
                                Boolean SkipACH = Convert.ToBoolean(pDT.Rows[rowCount]["SkipACH"]);
                                Boolean Deleted = Convert.ToBoolean(pDT.Rows[rowCount]["Deleted"]);

                                concatOldValue = "IsValidated:" + IsValidated + "|SkipACH:" + SkipACH + "|Deleted:" + Deleted;
                                msg = new irsAccountTransactionStatus().applyUpdates(crud, row, ht, concatOldValue);
                            }
                        }
                    }
                }
            }
            Session["dtOldRecord"] = null;
            return msg;
        }

        
    }
}
