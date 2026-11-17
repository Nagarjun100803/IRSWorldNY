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
using IRSClassLib;

namespace IRSWorld.IRSFileLoader
{
    public partial class OtherChannelsView : System.Web.UI.Page
    {
        public irsViewOtherChannels helper;// = new irsViewOtherChannels();

        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.Utility.RegisterTypeForAjax(typeof(OtherChannelsView));
        }

        #region PUBLIC

        #region Processing Other Channels to IRS
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public bool processToIRS(string recStat, int fSEQ_ID)
        {
            helper = VIEW_OTHER_CHANNELS();

            if (fSEQ_ID != null)
            {
                return true;
            }
        }
        #endregion

        #region DATA
        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String sqlCmd)
        {
            helper = VIEW_OTHER_CHANNELS();

            try
            {
                return helper.getDataTable(sqlCmd);
            }
            catch (Exception e)
            {
                throw new Exception("Failure getting data: " + e.Message);
            }
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public DataSet getDataSet(String sCmd, bool bStoreProc)
        {
            helper = VIEW_OTHER_CHANNELS();
            return helper.getDataSet(sCmd, bStoreProc);
        }

        [Ajax.AjaxMethod(Ajax.HttpSessionStateRequirement.Read)]
        public DataSet get_records(string rseq_id)
        {
            helper = VIEW_OTHER_CHANNELS();

            SqlCommand cmd = helper.getSqlCommand("SP_GET_OTHERCHANNELS_RECORDS");

            try
            {
                cmd.Parameters.Add("@rSEQ_ID", SqlDbType.Int).Value = rseq_id;
                return helper.getDataSet(cmd);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #endregion

        #region PRIVATE METHOD

        private irsViewOtherChannels VIEW_OTHER_CHANNELS()
        {
            if (Session["VIEW_OTHER_CHANNELS"] == null)
            {
                Session["VIEW_OTHER_CHANNELS"] = new irsViewOtherChannels();
            }

            return (irsViewOtherChannels)Session["VIEW_OTHER_CHANNELS"];
        }

        #endregion
    }
}