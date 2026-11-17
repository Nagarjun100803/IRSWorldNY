using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    public class irsRemForVerification : sqlHelperWs
    {
        private const string mdesc = "Crediting to FC";
        private const string sysid = "IRSWorldForVerification";
        private string _api_msg;
        private string _casa;
        private string _refno;
        private string _userId;
        private int _txn_seq_id;
        private int _product_code;
        private int _currency;
        private double _amount;        

        public enum Trans_Status : int
        {
            Credited_Online = 2,
            Posted_Central = 5,
            On_Hold = 14,
            For_Approval = 15,
            For_Reprocess = 9,
            Cancel_Not_Implemented = 11,
            Cancel_Implemented = 12,
            For_IBATCH_Processing = 16
        }

        public enum ProductCode : int
        {
            RapidRemit = 10,
            GFC = 22,
            WealthMultiplier = 14,
            PAW = 24,
            APASOnline = 27
        }

        public int update_status(string tranx_status)
        {
            return base.execStoredProcedureUpdate("sp_update_remittance_status", new object[] { (string)tranx_status, this._txn_seq_id.ToString(), this._api_msg, this._userId });
        }

        public void save_other_system() { 
            irsOldAPAS apas = new irsOldAPAS();
            irsLinkToGFC gfc = new irsLinkToGFC(true);
            String details = this._txn_seq_id.ToString();
            csAuditLog log = new csAuditLog();

            if (this._product_code == (int)ProductCode.APASOnline) // check for APAS
            {
                if (!apas.InsertApas(this._txn_seq_id))
                {
                    log.writeAudit(sysid, "fccredit.InsertApas", mdesc + "Error: " + apas.APASMessage.ToString(), "", "", csAuditLog.Category.Update);
                }
                else
                {
                    log.writeAudit(sysid, "fccredit.InsertApas", mdesc + "Successfully sent to APAS DB." + details, "", "", csAuditLog.Category.Update);
                }

            }
            else if (this._product_code == (int)ProductCode.PAW) // check for PAW
            {
                if (!apas.InsertAnPAW(this._txn_seq_id))
                {
                    log.writeAudit(sysid, "fccredit.InsertAnPAW", mdesc + "Error: " + apas.Message.ToString(), "", "", csAuditLog.Category.Update);
                }
                else
                {
                    log.writeAudit(sysid, "fccredit.InsertAnPAW", mdesc + "Successfully sent to APAS DB." + details, "", "", csAuditLog.Category.Update);
                }
            }
            
            gfc.IsWinApp = true;
            gfc.insert_GFC_Aggregate(this._refno, this._casa, this._currency, this._amount);
        }

        private void remittance_info() {
            DataTable dt = null;
            try {
                dt = execStoredProcedureTable("sp_get_for_verification_info", new object[] { this._txn_seq_id });
                if (dt != null || dt.Rows.Count > 0) {
                    this._casa = dt.Rows[0]["CASA"].ToString();
                    this._amount = Convert.ToDouble(dt.Rows[0]["NetRemitBenAmount"]);
                    this._currency = Convert.ToInt32(dt.Rows[0]["Currency"]);
                    this._refno = dt.Rows[0]["ReferenceNo"].ToString();
                    this._api_msg += dt.Rows[0]["api_msg"].ToString();
                }
            }
            catch { }
            finally { }
        }

        public irsRemForVerification(int txn_seq_id, int productcode, string branch) {
            try
            {
                this._userId = System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"].ToString();//System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            }
            catch { throw new Exception("Log in required!"); }
            this._txn_seq_id = txn_seq_id;
            this._product_code = productcode;
            this._api_msg = "FC CREDIT VERIFIED. TXN STATUS UPDATED by |USERID~" + this._userId + "| " + new irsTimeZone().branchLocalTime(branch).ToString() + " : ";
            this.remittance_info();
        }

        public irsRemForVerification() { }
    }
}
