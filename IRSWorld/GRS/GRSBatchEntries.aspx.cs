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
namespace IRSWorld.GRS
{
    public partial class GRSBatchEntries : System.Web.UI.Page
    {
        public string err_message = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(GRSBatchEntries));
            DateTime tranxdate = new irsTimeZone().branchLocalTime(IRSClientData.Location);
            this.hfTranxDate.Value = tranxdate.ToShortDateString();
        }
        private irsGRSBatchEntries IRSGRSBatchEntry
        {
            get
            {
                if (Session["IRS_GRSBATCHENTRIES"] == null)
                {
                    Session["IRS_GRSBATCHENTRIES"] = new irsGRSBatchEntries();
                }
                return (irsGRSBatchEntries)Session["IRS_GRSBATCHENTRIES"];
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
       

        #region AJAX PUBLIC METHODS
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_lookup_values()
        {
            DateTime tranxdate = new irsTimeZone().branchLocalTime(IRSClientData.Location);
            return this.IRSGRSBatchEntry.load_lookup_values(tranxdate, "1170");         

            
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet load_batch_info(string batchdate,string batch_num,string fileformat)
        {
            string filetype = "3";
            try
            {
                string userid = IRSClientData.UserId;
                return this.IRSGRSBatchEntry.getBatchInfo(Convert.ToDateTime(batchdate), batch_num, fileformat, filetype,userid );
            }
            catch(Exception e)
            {
                return null;
            }

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string  construct_batch_items(string noofitems, string runamt, string fileformat,string filetype,string ratetype)
        {
            try
            {   
                
                DateTime tranxdate = new irsTimeZone().branchLocalTime(IRSClientData.Location);

                 string batchnum = this.IRSGRSBatchEntry.create_batch_header(
                                                             tranxdate,
                                                             IRSClientData.UserId,
                                                             Convert.ToInt32(noofitems),
                                                             Convert.ToDecimal(runamt) ,
                                                             fileformat ,
                                                             filetype,
                                                             Convert.ToInt32(ratetype)
                                                             );

               
                 return batchnum;
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet show_loaded_batch(string tranxdate,string fileformat, string filetype, string batchnum)
        {
            try
            {
                int batch_seq_id = this.IRSGRSBatchEntry.get_batch_seq_id(fileformat, filetype);

                
                return this.IRSGRSBatchEntry.get_loaded_batch(
                                                            Convert.ToDateTime(tranxdate),
                                                            Convert.ToInt32(batchnum), 
                                                           batch_seq_id,
                                                            IRSClientData.UserId
                                                            );


                
            }
            catch (Exception e)
            {
                this.err_message = e.Message;
                return null;
            }
        }      

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string select_batch(string tranxdate, string fileformat, string filetype, string batchnum)
        {
            try
            {
                int batch_seq_id = this.IRSGRSBatchEntry.get_batch_seq_id(fileformat, filetype);


                bool isok = this.IRSGRSBatchEntry.setBatchInfo(
                                                            Convert.ToDateTime(tranxdate),
                                                            batchnum,
                                                           fileformat,
                                                           filetype,
                                                            IRSClientData.UserId
                                                            );

                if (isok)
                {
                    return "OK FOR LOAD";
                }
                else
                {
                    throw new Exception("ERROR: CANNOT LOCATE BATCH");
                }
            }
            catch (Exception e)
            {
                return  "ERROR:" + e.Message;
                
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string load_batch_data()
        {
            irsGRSBatchEntries irs_batch = null;

            string s = string.Empty;

            decimal runamt = 0;
            decimal totamt = 0;

            int totitems   = 0;
            int curritems  = 0;            

            try
            {
                irs_batch = (irsGRSBatchEntries)System.Web.HttpContext.Current.Session["IBATCH_HEADER"];

                if (irs_batch != null)
                {
                    curritems = irs_batch.p_noofitems - irs_batch.p_curitems;
                    runamt    = irs_batch.p_total_amount - irs_batch.p_running_amount;
                    totamt    = irs_batch.p_total_amount;
                    totitems  = irs_batch.p_noofitems;                
                               
                    s = irs_batch.p_batch_no.ToString() + "|";                
                    s += totamt.ToString() + "|";                
                    s += totitems.ToString() + "|";
                    s += irs_batch.p_running_amount.ToString() + "|";
                    s += irs_batch.p_curitems.ToString() + "|";
                    s += Convert.ToString(totamt - runamt) + "|";
                    s += Convert.ToString(totitems - curritems ) + "|";
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return s;
        }
        #endregion
        #region PRIVATE METHODS
       
        #endregion
    }
}
