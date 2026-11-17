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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Text;
using System.IO;
using IRSClassLib;
using IRSWorld.TranClass;
namespace IRSWorld.Reports.Pages
{
    [Serializable]
    public partial class Report_Holder : System.Web.UI.Page
    {
        // commented to remove warning - private string tempbranchcode = "RCI";
        // commented to remove warning - private string temprole = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Report_Holder));
            if (IRSClientData != null)
            {   
                this.txtbranchcode.Value = IRSClientData.Location;
                this.txtisadmin.Value = (IRSClientData.isSystemAdmin) ? "true": "false";
                this.txtpre_sdate.Value = new irsTimeZone().branchLocalTime(this.txtbranchcode.Value ).ToString("MM-dd-yyyy");
                this.txtpre_eoddate.Value = new irsTimeZone().branchLocalTime(this.txtbranchcode.Value ).ToString();
            }
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }
        #region PRIVATE METHOD

        private irsReport_LookupUI IRSReports
        {
            get
            {
                if (Session["IRS_ReportsUI"] == null)
                {
                    Session["IRS_ReportsUI"] = new irsReport_LookupUI();
                }
                else if ( (Session["IRS_ReportsUI"]).GetType() == "".GetType() )
                {
                    Session["IRS_ReportsUI"] = new irsReport_LookupUI();
                }
                return (irsReport_LookupUI)Session["IRS_ReportsUI"];
            }
        }

        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }
        #endregion
        #region "AJAX PUBLIC METHOD"
        
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_universal_branches()
        {
            /* ADDED BY BOYET NOV 18 2014 */
            int role_id = 0;
            int log_role = 0;
            string location = string.Empty;

            DataTable dt = null;

            try
            {
                log_role = Convert.ToInt32(((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Role.ToString());
                location = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;

                dt = new sqlHelperWs().getDataSet("select fldvalue,FldCode from tbl_lookup where fldgroup = 'user roles' and FldCode in ('UniApprover', 'UniAgentApprover')", false, new object[] { }).Tables[0];

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        role_id = Convert.ToInt32(dt.Rows[i][0]);

                        if (role_id == log_role)
                        {
                            string in_branches = "(3,4)";

                            if (dt.Rows[i]["FldCode"].ToString().ToUpper() == "UNIAGENTAPPROVER")
                            {
                                in_branches = "(4)";
                            }

                            if (location != null && location != string.Empty && location.ToUpper() == "GPL")
                            {
                                return new sqlHelperWs().getDataSet("select fldcode [SubGroupID], flddescription [TextDisplay] from tbl_lookup where fldgroup = 'ReportGroup'; select BranchCode, upper(BranchCode + ' - ' + BranchName) as BranchName from tbl_branches where (coveringbranchcode = 'ROM' and  branchtype = 4) or (branchtype = 4 and branchcode = 'GPL') order by BranchName", false, new object[] { });
                            }

                            return new sqlHelperWs().getDataSet("select fldcode [SubGroupID], flddescription [TextDisplay] from tbl_lookup where fldgroup = 'ReportGroup'; select BranchCode, upper(BranchCode + ' - ' + BranchName) as BranchName from tbl_branches where branchtype in " + in_branches + " and coveringbranchcode = '" + location.Replace("'", "''") + "' order by BranchName", false, new object[] { });
                        }
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite )]
        public String validate_subbranch(string branchcode,string subbranch)
        {
            int ibranch = 0;

            try
            {

                if (Session["ok_uni_approver"] != null)
                {
                    ibranch = (int)Session["ok_uni_approver"];
                }

                if (ibranch == 1)
                {
                    return "";
                }

                if (this.IRSClientData.isSystemAdmin)
                {
                    return "";
                }

                bool issubbranch = IRSReports.check_sub_branch(branchcode, subbranch);
                if (issubbranch)
                {
                    return "";
                }
                return "ERROR:";
            }
            catch { return "ERROR:"; }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite )]
        public String load_rep_dropdown(string seq_id,string sqlvalue,string datatype,string paramname)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = IRSReports.load_rep_dropdown(seq_id, sqlvalue);

                HtmlSelect sel = new HtmlSelect();
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                String s ;

                if (datatype.ToLower().IndexOf("date") > -1)
                {
                    datatype = "date";
                }
                else if (datatype.ToLower().IndexOf("money") > -1 || datatype.ToLower().IndexOf("int") > -1 || datatype.ToLower().IndexOf("decimal") > -1 || datatype.ToLower().IndexOf("bit") > -1)
                {
                    datatype = "number";
                }
                else
                {
                    datatype = "string";
                }
                sel.Attributes.Add("ID", "T" + paramname);
                sel.Attributes.Add("dtype",datatype);
                sel.Attributes.Add("style", "width:auto");
                sel.DataSource = ds;
                sel.DataTextField = "DESCRIPTION";
                sel.DataValueField = "VALUE";
                sel.DataBind();                
                sel.RenderControl(hw); 
                s = sw.ToString();
                return "<td class='cell_description' width='100px' nowrap>" + s + "</td></tr>";
            }
            catch
            {
                return null;
            }

        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite )]
        public DataSet load_rep_group(string branchcode)
        {
            DataSet ds = new DataSet();
            DataSet ds_univ = null;

            try
            {
                //ds = IRSReports.load_report_group_lookup(IRSClientData.Location ,IRSClientData.Role.ToString() );

                ds_univ = this.load_universal_branches();

                if (ds_univ != null && ds_univ.Tables[1].Rows.Count > 0)
                {
                    Session["ok_uni_approver"] = 1;

                    return ds_univ; /* ADDED BY BOYET TO DISPLAY BRANCHES BASE ON COVERINGBRANCH CODE OF CURRENT LOG USER */
                }

                int n_isadmin = (IRSClientData.isSystemAdmin) ? 1 : 0;
                ds = IRSReports.load_report_group_lookup(branchcode, n_isadmin);
                return ds;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }

                if (ds_univ != null)
                {
                    ds_univ.Dispose();
                    ds_univ = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite )]
        public DataSet load_rep_group_per_branch(string subgroupid,string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                ds = IRSReports.load_report_group_lookup_per_branch(subgroupid, branchcode, IRSClientData.Role.ToString());
                return ds;
            }
            catch
            {
                return null;
            }

        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite )]
        public string  load_crystal_report(string mergeparams,string paramvalues,string exporttype)
        {     
                irsReports irsrep;
                try
                {
                    irsrep = new irsReports(mergeparams, paramvalues,exporttype );

                   /* if (irsrep.isokforload())
                    {

                        //ReportDocument rep = irsrep.getReportDataSet();
                        //HttpContext.Current.Session["IRS_Reports"] = rep;
                        HttpContext.Current.Session["IRS_Reports_DataSet"] = irsrep.getReport_DataSet();
                        HttpContext.Current.Session["IRS_Reports_ReportPath"] = irsrep.p_rep_path;
                        HttpContext.Current.Session["IRS_REPORTS_EXPORTTYPE"] = exporttype ;
                        return "";
                    }
                    return irsrep.messages;*/
                    return "";
                }
                catch (Exception e)
                {
                    return "ERROR:" + e.Message;
                }
                finally
                {
                    irsrep = null;
                }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getUserRole()
        {
            DataSet ds = new DataSet();
            sqlHelperWs sql = new sqlHelperWs();
            ds = sql.getDataSet("usp_getUserRole",true,new object[]{});
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getUserStatus()
        {
            DataSet ds = new DataSet();
            sqlHelperWs sql = new sqlHelperWs();
            ds = sql.getDataSet("usp_getUserFlags", true, new object[] { });
            return ds;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getBranchValue(string filter)
        {
            DataSet DS = new DataSet();
            DataTable DT = DS.Tables.Add();

            DT.Columns.Add("Code", typeof(string));
            DT.Columns.Add("value", typeof(string));

            if (filter.ToString().ToLower() == "branch")
            {
                DT.Rows.Add("OverseasBranch", "3");
                DT.Rows.Add("Agents", "4");
                DT.Rows.Add("PNB Domestic Branch", "2");
            }
            else if (filter.ToString().ToLower() == "subbranch")
            {
                irsConfiguration config = new irsConfiguration();

                string conn = config.appsetting("DevelConnectionString");
                string[] arrKey = conn.ToString().Split(';');
                string[] arrVal = arrKey[2].Split('=');

                if (arrVal[1].ToString().ToLower() == "irsworld")
                {
                    DT.Rows.Add("RCI", "RCI");
                    DT.Rows.Add("RCC", "RCC");
                }
                else if (arrVal[1].ToString().ToLower() == "irsworldea")
                {
                    DT.Rows.Add("PAR", "PAR");
                    DT.Rows.Add("MIL", "MIL");
                }
                else
                {
                    DT.Rows.Add("RCI", "RCI");
                    DT.Rows.Add("RCC", "RCC");
                    DT.Rows.Add("PAR", "PAR");
                    DT.Rows.Add("MIL", "MIL");
                }
            }
            return DT;
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }
    }
}
