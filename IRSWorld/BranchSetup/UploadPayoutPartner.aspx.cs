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
using System.IO;

namespace IRSWorld.BranchSetup
{
    public partial class UploadPayoutPartner : System.Web.UI.Page
    {
        private sqlHelper sql = new sqlHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
            if (!Page.IsPostBack)
            {
                getTimeZone();
            }
        }

        public void getTimeZone()
        {
            String[] ss = new irsTimeZone().Timezones;
            foreach (string s in ss)
            {
                ddlTimeZone.Items.Add(s);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int upload_err_ctr = 0;
            if (flUpload.HasFile)
            {
                if (!Directory.Exists(Server.MapPath(@"\APASBranch")))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(@"\APASBranch"));

                }
                flUpload.SaveAs(Server.MapPath(@"\APASBranch\" + flUpload.FileName));

                StreamReader sr = new StreamReader(Server.MapPath(@"\APASBranch\" + flUpload.FileName));
                string payout_branch_code;
                string payout_sub_branch_code;
                string branch_desc;
                string parent_payout_code;

                sql.TableName = "TBL_BRANCHES";
                sql.IdentityColumn = "SEQ_ID";
                sql.getUpdateDS("SELECT BranchType,BranchCode,BranchName,TimeZone,Payout_Int_Cd,CoveringBankCode,Classification FROM TBL_BRANCHES");

                while (sr.Peek() != -1)
                {
                    string sr_line = sr.ReadLine();
                    payout_branch_code = sr_line.Substring(0, 10);
                    payout_sub_branch_code = sr_line.Substring(10, 10);
                    branch_desc = sr_line.Substring(20, 200);
                    parent_payout_code = sr_line.Substring(220, 5);

                    object o = sql.execScalar("SELECT COUNT(*) FROM TBL_BRANCHES WHERE BranchCode='" + payout_branch_code.Trim() + "'");
                    if (Convert.ToInt32(o) <= 0)
                    {
                        bool isSuccess = save_textfile(payout_branch_code, payout_sub_branch_code, branch_desc, parent_payout_code);
                    }
                    else
                    {
                        upload_err_ctr += 1;
                    }
                }
                bool ex = sql.save(sqlHelper.RETAIN_DATASET);
                upload_err_ctr = 0;
                Response.Write("<script>alert('Record(s) Successfully Uploaded');</script>");
                sr.Close();

            }
            else
            {
                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err", "alert('Please select a file')", true);
            }
        }

        private bool save_textfile(string o_code,string sub_code,string desc,string parent_code)
        {
            DataRow dr;

            try
            {
                dr = sql.table.NewRow();
                dr["BranchCode"] = o_code.Trim();
                dr["BranchName"] = desc.Trim();
                dr["TimeZone"] = ddlTimeZone.Text;
                dr["Payout_Int_Cd"] =  sub_code.Trim();
                dr["CoveringBankCode"] = parent_code.Trim();
                dr["Classification"] = 1;
                dr["BranchType"] = 7;
                sql.table.Rows.Add(dr);
                return true;
            }
            catch //(Exception ex) // commented to remove warning
            {
                ClientScript.RegisterStartupScript(Type.GetType("System.String"), "err2", "alert('Error occured while uploading the file, Please check if there is a special character(s)')", true);
                return false;
            }
            //return false; // commented to remove warning
            

            
        }

    }
}
