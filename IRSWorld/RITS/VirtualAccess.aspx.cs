
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
using System.Data.SqlClient;
using System.Text;
using System.IO;
using IRSClassLib;

namespace IRSWorld.RITS
{
    public partial class VirtualAccess : System.Web.UI.Page
    {
        irsPOLINK POL = new irsPOLINK();
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(VirtualAccess));
            Session["session_location"] = ((irsClientData)Session["IRS_CLIENT_DATA"]).Location;
            Session["uid"] = ((irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
            if (DirExists("c:\\temp\\IRSWFL\\upload\\") == false)
            {
                System.IO.Directory.CreateDirectory(@"c:\\temp\\IRSWFL\\upload\\");
                
            }
            irsTimeZone branch_time = new irsTimeZone();
            Session["branch_date"] = branch_time.branchLocalTime(Session["session_location"].ToString());
            Session["uploadpath"] = "c:\\temp\\IRSWFL\\upload";
            Session["querytype"] = "";

        }
        private static bool DirExists(string sDirName)
        {
            try
            {
                return (System.IO.Directory.Exists(sDirName));    //Check for file
            }
            catch (Exception)
            {
                return (false);                                 //Exception occured, return False
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "PO";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_virtual()
        {
            batch_check_virtual();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_virtual(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "VA";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_processed()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_processed(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "PO";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_processed_virtual()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_processed_virtual(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "VA";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_hold()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_hold(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "PO";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_hold_virtual()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_hold_virtual(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "VA";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_other()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_other(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "PO";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_other_virtual()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_other_virtual(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "VA";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_pdep()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_pdep(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "PO";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_pdep_virtual()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_pdep_virtual(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "VA";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_overlimit()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_overlimit(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "PO";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_value_table_overlimit_virtual()
        {
            batch_check();
            string mycurrbatch = Session["current_batch"].ToString();
            DataSet ds = POL.getTable_overlimit_virtual(Convert.ToInt32(mycurrbatch));
            Session["querytype"] = "VA";
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getCodeOpts(string fldgroup)
        {

            return POL.getCodeOpts(fldgroup, Session["session_location"].ToString());

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_table_date()
        {
            string mytdate = Session["tdate"].ToString();
            string myfdate = Session["fdate"].ToString();
            DataSet ds = POL.getTable2(mytdate, myfdate);
            return ds.Tables[0];
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_table_date_va()
        {
            string mytdate = Session["tdate"].ToString();
            string myfdate = Session["fdate"].ToString();
            DataSet ds = POL.getTable2_va(mytdate, myfdate);
            return ds.Tables[0];
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_date_variable(string todate, string fromdate)
        {
            Session["tdate"] = todate;
            Session["fdate"] = fromdate;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool generate_txt()
        {
            string myquerytype = Session["querytype"].ToString();
            DataSet ds = POL.generate_txt(myquerytype);
            int coltotal = ds.Tables[0].Columns.Count;
            int rowtotal = ds.Tables[0].Rows.Count - 1;
            if (rowtotal != -1)
            {
                // Commented to remove warning - int sq_num = 0;
                Session["csv_output_filename"] = "MIZ -" + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("HHmmss") + ".csv"; ;
                System.IO.StreamWriter tw = new System.IO.StreamWriter(Session["uploadpath"] + "\\" + Session["csv_output_filename"]);
                for (int i = 0; i <= rowtotal; i++)
                {

                    string x1 = ds.Tables[0].Rows[i][2].ToString();
                    string x2 = ds.Tables[0].Rows[i][3].ToString();
                    string x3 = ds.Tables[0].Rows[i][1].ToString();
                    string x4 = ds.Tables[0].Rows[i][4].ToString();
                    string tempx5 = ds.Tables[0].Rows[i][5].ToString();
                    double x5 = Convert.ToDouble(tempx5);
                    string x6 = ds.Tables[0].Rows[i][6].ToString();
                    x6 = x6.Substring(8, 2) + x6.Substring(0, 2) + x6.Substring(3, 2);
                    string x7 = ds.Tables[0].Rows[i][7].ToString();
                    x7 = x7.Replace(":", "");
                    string x8 = ds.Tables[0].Rows[i][4].ToString() + "-" + ds.Tables[0].Rows[i][52].ToString();

                    tw.WriteLine(x1 + ",," + x2 + "," + x3 + "," + x4 + "," + x5 + "," + x6 + "," + x7 + "," + x8);

                }
                tw.Close();
                POL.set_process_flag(myquerytype);
                string del_filepath = Session["uploadpath"] + "\\" + Session["csv_output_filename"];
                string mycsv_output = Session["csv_output_filename"].ToString();
                System.IO.FileInfo filesize = new System.IO.FileInfo(del_filepath);
                POL.save_filename(mycsv_output, filesize.Length.ToString(), Convert.ToDateTime(Session["branch_date"]), Session["uid"].ToString(), Session["session_location"].ToString());
                POL.sendToFTP(del_filepath);
                //Delete CSV File after FTP
                File.Delete(del_filepath);
                //return ds.Tables[0];
                return true;
            }
            else
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_file()
        {

            string del_filepath = Session["uploadpath"].ToString() + Session["fileName"].ToString();
            File.Delete(del_filepath);
            irsPOLINK delete_uploaded = new irsPOLINK();
            string mycurrbatch = Session["current_batch"].ToString();
            delete_uploaded.delete_temp_upload(Convert.ToInt32(mycurrbatch));

            return true;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void batch_check()
        {

            //check latest batchnumber in the table
            int b_no = new int();
            irsPOLINK getbtchno = new irsPOLINK();
            Session["current_batch"] = getbtchno.get_current_batch(b_no);
            //status update check if null etc
            int t_no = new int();
            irsPOLINK gettotalnull = new irsPOLINK();
            string mycurrbatch = Session["current_batch"].ToString();
            int totalnull = gettotalnull.get_total_null(t_no, Convert.ToInt32(mycurrbatch));
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void batch_check_virtual()
        {

            //check latest batchnumber in the table
            int b_no = new int();
            irsPOLINK getbtchno = new irsPOLINK();
            Session["current_batch"] = getbtchno.get_current_batch_virtual(b_no);
            //status update check if null etc
            int t_no = new int();
            irsPOLINK gettotalnull = new irsPOLINK();
            string mycurrbatch = Session["current_batch"].ToString();
            int totalnull = gettotalnull.get_total_null(t_no, Convert.ToInt32(mycurrbatch));
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool applyEdits(String adds, String dels, String eds)
        {
            char sep = '^';
            // commented to remove warning - bool deleted;
            ArrayList errs = new ArrayList();
            String[] ss = { adds, dels, eds };
            foreach (String s in ss)
            {
                if (s.Length > 0)
                {
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


                        irsPOLINK POL = new irsPOLINK();

                        bool ok = POL.save_changes(ht);

                        if (!ok)
                        {
                            throw new Exception("Save Changes:" + POL.Message);
                        }
                    }
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


            return true;
        }


    }
}