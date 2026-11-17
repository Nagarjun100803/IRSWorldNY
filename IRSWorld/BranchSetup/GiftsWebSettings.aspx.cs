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
using System.IO;
using IRSClassLib;
using System.Linq;

namespace IRSWorld.BranchSetup
{
    public partial class GiftsWebSettings : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GiftsWebSettings));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        #region AJAX PUBLIC METHOD

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            Session["paramActionType"] = "ADD";
            return this.do_gws("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string field_group)
        {
            Session["paramActionType"] = "EDIT";
            Session["GWSETTINGS_SEQ_ID"] = field_group;

            return this.do_gws("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool delete_record(string _seq_id)
        {
            Session["paramActionType"] = "DELETE";
            Session["GWSETTINGS_SEQ_ID"] = _seq_id;

            return this.do_gws("DELETE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.do_gws("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.do_gws("ROLLBACK");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["GWSETTINGS_MESSAGE"] != null)
            {
                return (string)Session["GWSETTINGS_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["GWSETTINGS_MESSAGE"] = null;
            Session["GWSETTINGS_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_GWSETTINGS_RECORD(string column_values)
        {
            irsGiftsWebSettings gws = this.GIFTSWEBSETTINGS();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (gws.set_record(s) == false)
                {
                    this.set_message(gws.messages); return false;
                }

                Session["IRS_GWSETTINGS"] = gws;

                return true;
            }

            gws = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_GWSETTINGS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_GWSETTINGS_RECORD(string client_seq_id)
        {
            irsGiftsWebSettings gws = this.GIFTSWEBSETTINGS();

            try
            {
                if (gws != null)
                {
                    if (gws.get_record(client_seq_id) == true)
                    {
                        return gws.getdataRecord();
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
                Session["IRS_GWSETTINGS"] = gws;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string GWSETTINGS_SEQ_ID()
        {
            if (Session["GWSETTINGS_SEQ_ID"] != null)
            {
                return (string)Session["GWSETTINGS_SEQ_ID"];
            }

            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet reload_branches(int intRegionCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_giftsweb_branches");
                    sqcommands.Parameters.Add("@charge_group", SqlDbType.SmallInt).Value = intRegionCode;

                    return commons.getDataSet(sqcommands, "Get_BranchName");
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getRole()
        {
            return Convert.ToString(IRS_CLIENT_DATA().Role);
        }

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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_branch()
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_branch");

                    return commons.getDataSet(sqcommands, "Get_BranchName");
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_dropdown_region_currency");
                    sqcommands.Parameters.Add("@fld_group", SqlDbType.VarChar, 100).Value = fldgroup;

                    return commons.getDataSet(sqcommands, "GET_GIFTSWEBDD");
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public static bool checkexist(string folderdir)
        {
            try
            {
                if (Directory.Exists(folderdir))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }               
        }
        #endregion


        #region PRIVATE METHOD: GIFTSWEBSETTINGS

        private irsGiftsWebSettings GIFTSWEBSETTINGS()
        {
            if (Session["IRS_GWSETTINGS"] == null)
            {
                Session["IRS_GWSETTINGS"] = new irsGiftsWebSettings();
            }

            return (irsGiftsWebSettings)Session["IRS_GWSETTINGS"];
        }

        private bool do_gws(string instruction)
        {
            irsGiftsWebSettings gws = this.GIFTSWEBSETTINGS();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            gws.selectedRow = -1;
                            if (gws.insert_record() == false)
                            {
                                this.set_message(gws.messages); return false;
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
                            gws.seq_id = this.GWSETTINGS_SEQ_ID();
                            gws.selectedRow = 0;

                            if (gws.update_record() == false)
                            {
                                this.set_message(gws.messages); return false;
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
                            if (this.SET_GWSETTINGS_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            DataTable dtLookup = new irsInternetPage().getLookup("ParamApproverFlag");
                            if (dtLookup.Rows.Count == 0 || dtLookup.Rows[0]["FldValue"].ToString() == "0")
                            {
                                if (gws.commit_record() == false)
                                {
                                    this.set_message(gws.messages); return false;
                                }
                                return true;
                            }//need to approve
                            else
                            {
                                string userID = ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).UserId;
                                string actionType = Session["paramActionType"].ToString();// tt[0].ToString().ToUpper();
                                if (gws.commit_record_tmp(userID,actionType, this.parameter_values) == false)
                                {
                                    this.set_message(gws.messages); return false;
                                }


                                //email sending to approver
                                try
                                {
                                    irsSqlCommonsWs commons = new irsSqlCommonsWs();
                                    string[] sEmail = null;
                                    DataSet myDt = commons.getDataSet("sp_paramApproval_getApprEmailList", true, new object[] { });
                                    if (myDt.Tables[0].Rows.Count > 0)
                                    {
                                        sEmail = myDt.Tables[0].Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                                    }
                                    csMailer imailer = new csMailer();
                                    imailer.sendMail("[NOTIFICATION] IRSW Parameter Change for Approval", sEmail, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + " - Change in IRSW parameter has been submitted for review and approval\n\n");
                                }
                                catch { }
                                return true;
                            }
                            

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
                            gws.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "DELETE")
                    {
                        try
                        {
                            gws.seq_id = this.GWSETTINGS_SEQ_ID();
                            gws.selectedRow = 0;

                            if (gws.delete_record() == false)
                            {
                                this.set_message(gws.messages); return false;
                            }

                            return true;

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

                Session["IRS_ENDOFTRANSFER"] = null;
                Session["IRS_ENDOFTRANSFER"] = gws;
                Session["ENDOFTRANSFER_SEQ_ID"] = null;
                Session["ENDOFTRANSFER_SEQ_ID"] = gws.seq_id;
                gws = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion
    }
}
