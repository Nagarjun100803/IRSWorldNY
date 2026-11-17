using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsIBFTKey : sqlHelperWs
    {
        internal static TriDes getTriDes()
        {
            return new TriDes("xfm92.3", "IRSWorld");
        }

        private string getLocation()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
        }

        private string getUserID()
        {
            return ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
        }

        private void writeAudit(string Description, string NewValue, string OldValue, csAuditLog.Category Category)
        {
            string sDescription = "Location=" + getLocation() + " : " + Description;
            new csAuditLog().writeAudit(getUserID(), "IBFT Key", sDescription, NewValue, OldValue, Category);
        }

        public bool submitKey(string businessKey, string securityKey)
        {
            try
            {
                base.getUpdateDataSet(getSqlCommand("SELECT TOP 1 * FROM tbl_IBFTKey", false));
                if (!base.hasRows)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                    writeAudit("IBFT Key Setup Initialized by User", "submitKey", "ADD", csAuditLog.Category.IBFT);
                }
                else
                {
                    selectedRow = 0;
                    writeAudit("IBFT Key Setup Initialized by User", "submitKey", "EDIT", csAuditLog.Category.IBFT);
                }
                setColumn("BusinessKey", getTriDes().Encrypt(businessKey));
                setColumn("SecurityKey", getTriDes().Encrypt(securityKey));
                if (base.addMode)
                {
                    setColumn("DateCreated", DateTime.Now);
                }
                else
                {
                    setColumn("DateModified", DateTime.Now);
                }
                bool ok = base.save(sqlHelperWs.RETAIN_DATASET);
                return ok;
            }
            catch (Exception ex)
            {
                string e = ex.Message;
                return false;
            }
        }

        public string[] submitIBFT(string wsURL, string remoteHost, int port, int timeOut, 
            string branchCode, string connUS, string connEA, string terminalID, 
            string amount, string destinationBank, string vostroAccount, 
            string targetAccount, string referenceNo)
        {
            wsIBFT.wsIBFT ibft = new IRSClassLib.wsIBFT.wsIBFT();
            ibft.Url = wsURL;
            return ibft.createISOMessage(branchCode, connUS, connEA, timeOut, remoteHost, port,
                IRSClassLib.wsIBFT.ChannelCode.IRSWorld, IRSClassLib.wsIBFT.ISOCode.FT,
                "", IRSClassLib.wsIBFT.IBFTMode.InterBankFundTransfer, IRSClassLib.wsIBFT.AccountType.SA,
                amount, IRSClassLib.wsIBFT.MerchantType.ATM, terminalID, "", destinationBank,
                vostroAccount, targetAccount, referenceNo, true);
        }
    }
}