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
using System.Data.SqlClient;

namespace IRSWorld.BillsPayment
{
    public partial class OBPSDynamicField : System.Web.UI.Page
    {
        private irsOBPSTransaction irsOBPSTransaction
        {
            get
            {
                if (Session["OBPSDynamicFields"] == null)
                {
                    Session["OBPSDynamicFields"] = new irsOBPSTransaction();
                }
                return (irsOBPSTransaction)Session["OBPSDynamicFields"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(OBPSDynamicField));
            //if (Session["IRS_CLIENT_DATA"] == null)
            //{
            //    Response.Redirect("ErrorPage.aspx");
            //}
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getFields(string MerchantCode, int MerchantProdCode)
        {
            DataTable dt = null;
            try
            {
                dt = irsOBPSTransaction.getMerchantFields(MerchantCode, MerchantProdCode);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getMerchantCode()
        {
            DataTable dt = null;
            try
            {
                dt = irsOBPSTransaction.getMerchantList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getMerProductbyMerchant(string MerchantCode)
        {
            DataTable dt = null;
            try
            {
                dt = irsOBPSTransaction.getMerProductByMerchant(MerchantCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getLookupField()
        {
            DataTable dt = null;
            try
            {
                dt = irsOBPSTransaction.getLookupFldGroup();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Boolean saveFields(int SEQ_ID, string mercode, int merprod, string data_value)
        {
            bool blnSave;
            try
            {
                if (irsOBPSTransaction.saveFields(SEQ_ID, mercode, merprod, data_value))
                {
                    blnSave = true;
                }
                else
                { blnSave = false; }
            }
            catch (Exception ex)
            {
                throw ex;                
            }
            return blnSave;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getAllFields(int SEQ_ID, string mercode, int merprod)
        {
            return irsOBPSTransaction.getAllFields(SEQ_ID, mercode, merprod);
        }

        private bool do_look_up(string instruction)
        {
            //irsLookUp cad = this.LOOK_UP();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            irsOBPSTransaction.selectedRow = -1;
                            if (irsOBPSTransaction.insert_record() == false)
                            {
                                //this.set_message(irsOBPSTransaction.messages); 
                                return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            //this.set_message(e.Message); 
                            return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            //irsOBPSTransaction.seq_id = this.LOOK_UP_SEQ_ID();
                            irsOBPSTransaction.selectedRow = 0;

                            if (irsOBPSTransaction.update_record() == false)
                            {
                                //this.set_message(irsOBPSTransaction.messages); 
                                return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            //this.set_message(e.Message); 
                            return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            //if (this.SET_LOOKUP_RECORD(this.parameter_values) == false)
                            //{
                            //    return false;
                            //}

                            if (irsOBPSTransaction.commit_record() == false)
                            {
                                //this.set_message(irsOBPSTransaction.messages); 
                                return false;
                            }

                            return true;

                        }
                        catch (Exception e)
                        {
                            //this.set_message(e.Message); 
                            return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            irsOBPSTransaction.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            //this.set_message(e.Message); 
                            return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {

                //Session["IRS_LOOK_UP"] = null;
                //Session["IRS_LOOK_UP"] = cad;
                //Session["LOOK_UP_SEQ_ID"] = null;
                //Session["LOOK_UP_SEQ_ID"] = cad.seq_id;
                //cad = null;
            }
            return false;
            //this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
    }
}
