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

namespace IRSWorld.IRSFileLoader
{
    public partial class HitViewer : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(HitViewer));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_HIT_VIEWER_RECORD(string client_seq_id)
        {
            irsHitViewer hitviewer = this.HIT_VIEWER();
            
            try
            {
                if (hitviewer != null)
                {
                    if (hitviewer.get_record(client_seq_id) == true)
                    {
                        return hitviewer.getdataRecord();
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
                Session["IRS_HIT_VIEWER"] = hitviewer;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_percentage_match_setup()
        {
            Session["BranchSetupMatching"] = null;
            DataSet ds = null;
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    ds = commons.execStoredProcedure("proc_get_branch_setup_matching", new Object[] { (string) IRS_CLIENT_DATA().Location });

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string percent_val = string.Empty;

                        ds = null;
                        ds = commons.execStoredProcedure("proc_get_branch_setup_percentagescore", new Object[] { (string) IRS_CLIENT_DATA().Location });
                                                                                                                      
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            percent_val = ds.Tables[0].Rows[0]["flddescription"].ToString();
                        }
                        Session["BranchSetupMatching"] = "tbl_cifhitlist_percentage";
                        return "true|" + percent_val;
                    }                                        
                }
                Session["BranchSetupMatching"] = "tbl_cifhitlist";           
                return "false|0";
            }
            catch(Exception e)
            {
                return "false|0";
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (ds != null)
                {
                    ds.Dispose();
                    ds = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_HIT_VIEWER_RECORD_ACCT(string client_seq_id)
        {
            irsAcctHitViewer hitviewer = this.HIT_VIEWER_ACCT();
            try
            {
                if (hitviewer != null)
                {
                    if (hitviewer.get_record(client_seq_id) == true)
                    {
                        return hitviewer.getdataRecord();
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
                Session["IRS_HIT_VIEWER_ACCT"] = hitviewer;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string seq_id)
        {
            Session["HIT_VIEWER_SEQ_ID"] = seq_id;

            return this.do_hit_viewer("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_pscore_record(string pscore)
        {
            string errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("proc_updt_percentagescore", new Object[] { Convert.ToInt32(pscore), IRS_CLIENT_DATA().Location.ToString() });
                    if (res == -1)
                    {
                        throw new Exception(commons.Message);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (commons != null) { commons = null; }

            }
            return false;

        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record_acct(string seq_id)
        {
            Session["HIT_VIEWER_SEQ_ID"] = seq_id;

            return this.do_hit_viewer_acct("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_hit_viewer("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record_acct(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_hit_viewer_acct("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_hit_viewer("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record_acct()
        {
            return this.do_hit_viewer_acct("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["HITVIEWER_MESSAGE"] != null)
            {
                return (string)Session["HITVIEWER_MESSAGE"];
            }
            return string.Empty;

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["HITVIEWER_MESSAGE"] = null;
            Session["HITVIEWER_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_HITVIEWER_RECORD(string column_values)
        {
            irsHitViewer hitviewer = this.HIT_VIEWER();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (hitviewer.set_record(s) == false)
                {
                    this.set_message(hitviewer.messages); return false;
                }

                Session["IRS_HIT_VIEWER"] = hitviewer;

                return true;
            }

            hitviewer = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_HITVIEWER_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_HITVIEWER_RECORD_ACCT(string column_values)
        {
            irsAcctHitViewer hitviewer = this.HIT_VIEWER_ACCT();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (hitviewer.set_record(s) == false)
                {
                    this.set_message(hitviewer.messages); return false;
                }

                Session["IRS_HIT_VIEWER_ACCT"] = hitviewer;

                return true;
            }

            hitviewer = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_HITVIEWER_RECORD_ACCT(). PARAM VALUE NOT SPECIFIED."); return false;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string count_not_ConvertedCIF()
        {
            irsCIFPhonetics phonetics = new irsCIFPhonetics();
            return phonetics.count_not_ConvertedCIF(IRS_CLIENT_DATA().Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void refresh_table()
        {
            irsCIFPhonetics phonetics = new irsCIFPhonetics();
            phonetics.refresh_table(IRS_CLIENT_DATA().Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int convert_CIF_phonetics(int noToConvert)
        {
            irsCIFPhonetics phonetics = new irsCIFPhonetics();
            return phonetics.convert_CIF_Phonetics(noToConvert, IRS_CLIENT_DATA().Location);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string HIT_VIEWER_SEQ_ID()
        {
            if (Session["HIT_VIEWER_SEQ_ID"] != null)
            {
                return (string)Session["HIT_VIEWER_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getBranchCode()
        {
            //   return IRS_CLIENT_DATA().Location;
            string SQL = "";

            SQL = "SELECT SUBSTRING(fldcode, 1, 2) " +
                    "FROM tbl_Lookup " +
                    "WHERE fldgroup = 'overseasgroup' " +
                    "AND SUBSTRING(fldcode, 4, 3) = '" + IRS_CLIENT_DATA().Location + "'";

            irsSqlCommonsWs com = new irsSqlCommonsWs();
            DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "BranchCode");

            return ds.Tables[0].Rows[0][0].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocBranch()
        {
            return IRS_CLIENT_DATA().Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getFileFormat()
        {
            //   return IRS_CLIENT_DATA().Location;
            string SQL = "";

            SQL = " SELECT fldcode, fldvalue " +
                   "FROM tbl_lookup " +
                   "WHERE fldgroup = 'irscompliance' " +
                   "AND fldvalue IN (SELECT distinct FileFormat " +
                                    "FROM tbl_cifhitlist " +
                                    "WHERE branchcode = (SELECT SUBSTRING(fldcode, 1, 2) " +
                                                        "FROM tbl_Lookup " +
                                                        "WHERE fldgroup = 'overseasgroup' " +
                                                        "AND SUBSTRING(fldcode, 4, 3) = '" + IRS_CLIENT_DATA().Location + "' " +
                                                        ") " +
                                   ") " +
                    "UNION ALL " +
                    "SELECT DISTINCT " +
                    "UPPER(ISNULL(B.FldCode,'')) AS FldCode, " +
                    "B.fldValue " +
                    "FROM  TBL_ACCOUNTWATCHLIST	A " +
                    "INNER JOIN TBL_LOOKUP 		B ON	B.FLDVALUE = A.FILEFORMAT " +
                    "AND 	UPPER(B.FldGroup) = 'IRSCOMPLIANCE' " +
                    "INNER JOIN TBL_LOOKUP 		C ON	A.UploadingBranch = SUBSTRING (C.fldcode, 1, 2)  " +
                    "AND 	SUBSTRING (C.fldcode, 4, 3) = '" + IRS_CLIENT_DATA().Location + "' ";

            irsSqlCommonsWs com = new irsSqlCommonsWs();
            return com.getDataSet(com.getSqlCommand(SQL, false), "Fileformat");

            //return ds.Tables[0].Rows[0][0].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();
                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "RemBenNames");

                return ds.Tables[0];
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }
        #endregion

        #region PRIVATE METHOD: HIT_VIEWER

        private irsClientData IRS_CLIENT_DATA()
        {
            if (Session["IRS_CLIENT_DATA"] != null)
                return (irsClientData)Session["IRS_CLIENT_DATA"];
            else
            {
                Response.Redirect("ErrorPage.aspx");
                return null;
            }
        }

        private irsHitViewer HIT_VIEWER()
        {            
            if (Session["IRS_HIT_VIEWER"] == null)
            {
                Session["IRS_HIT_VIEWER"] = new irsHitViewer(Session["BranchSetupMatching"].ToString());
            }

            return (irsHitViewer)Session["IRS_HIT_VIEWER"];
        }

        private irsAcctHitViewer HIT_VIEWER_ACCT()
        {
            if (Session["IRS_HIT_VIEWER_ACCT"] == null)
            {
                Session["IRS_HIT_VIEWER_ACCT"] = new irsAcctHitViewer();
            }

            return (irsAcctHitViewer)Session["IRS_HIT_VIEWER_ACCT"];
        }

        private bool do_hit_viewer_acct(string instruction)
        {
            irsAcctHitViewer hv = this.HIT_VIEWER_ACCT();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            hv.selectedRow = -1;
                            if (hv.insert_record() == false)
                            {
                                this.set_message(hv.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            hv.seq_id = this.HIT_VIEWER_SEQ_ID();
                            hv.selectedRow = 0;

                            if (hv.update_record() == false)
                            {
                                this.set_message(hv.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_HITVIEWER_RECORD_ACCT(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (hv.commit_record() == false)
                            {
                                this.set_message(hv.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            hv.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {

                Session["IRS_HIT_VIEWER_ACCT"] = null;
                Session["IRS_HIT_VIEWER_ACCT"] = hv;
                Session["HIT_VIEWER_SEQ_ID"] = null;
                Session["HIT_VIEWER_SEQ_ID"] = hv.seq_id;

                hv = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        private bool do_hit_viewer(string instruction)
        {
            irsHitViewer hv = this.HIT_VIEWER();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            hv.selectedRow = -1;
                            if (hv.insert_record() == false)
                            {
                                this.set_message(hv.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            hv.seq_id = this.HIT_VIEWER_SEQ_ID();
                            hv.selectedRow = 0;

                            if (hv.update_record() == false)
                            {
                                this.set_message(hv.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_HITVIEWER_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (hv.commit_record() == false)
                            {
                                this.set_message(hv.messages); return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            hv.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {

                Session["IRS_HIT_VIEWER"] = null;
                Session["IRS_HIT_VIEWER"] = hv;
                Session["BranchSetupMatching"] = null;
                Session["BranchSetupMatching"] = hv;
                Session["HIT_VIEWER_SEQ_ID"] = null;
                Session["HIT_VIEWER_SEQ_ID"] = hv.seq_id;

                hv = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion
    }
}
