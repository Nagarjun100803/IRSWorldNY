using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace E2EAPIConnector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            E2EAPIConnector e2e = new E2EAPIConnector();
            string accountNo = "188810002000";
            string channel = "IRSW";
            string transactionBranch = "0063";
            string[] param = null;
            string userRef = channel + "_" + accountNo + "_" + DateTime.Now.ToString("yyyyMMddhhMMss");
            string e2eres = string.Empty;
            bool ret = false;
            string err = string.Empty;

            //Account Inquiry
            //AccountId, UserReferenceNumber, TransactionBranch, Channel
            param = new string[] { accountNo, userRef, transactionBranch, channel };
            DataSet inquiry = e2e.connect2E2E(Guid.NewGuid().ToString(), E2EAPIConnector.APIMethod.AccountInquiry, E2EAPIConnector.MediaType.json, param, out ret, out err);
        }
    }
}