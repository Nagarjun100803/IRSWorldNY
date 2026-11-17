using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsTDD : sqlHelperWs
    {
        csAuditLog logger = new csAuditLog();
        public DataTable getBranchSetup(string branchcode)
        {
            return base.getDataSet(base.getSqlCommand("select * from tbl_branch_setup where branchcode='" + branchcode + "'", false)).Tables[0];
        }


        public bool is_first_time_remitter(string remitterid)
        {
            DataTable dt = base.getDataSet(base.getSqlCommand("select top 1 * from tbl_remittance where remittercifid='" + remitterid + "'", false)).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }

        public int submitRemittance(string Remittance)
        {
            int i = 0;
            string[] arrRemittance;
            string[] arrRemittanceInsert = null;
            //DataTable dt;
            //DataRow dr;
            int iReturn = 0;
             
            try
            {
                arrRemittance = Remittance.Split('|');
                if (arrRemittance.Length > 0)
                {
                    //dt = createTable("Remittance");
                    //dr = dt.NewRow();
                    arrRemittanceInsert = new string[arrRemittance.Length];
                    for (i = 0; i <= (arrRemittance.Length - 1); i++)
                    {
                        if (arrRemittance.GetValue(i).ToString() == "NULL")
                        {
                            arrRemittanceInsert.SetValue("", i);
                        }
                        else
                        {
                            arrRemittanceInsert.SetValue(arrRemittance.GetValue(i), i);
                        }
                    }
                    //dt.Rows.Add(dr);
                    //iReturn = execStoredProcedureUpdate("ksp_InsertRemittance", dr);
                    iReturn = execStoredProcedureUpdate("ksp_InsertRemittance_TDD", arrRemittanceInsert);
                }
                else
                {
                    iReturn = -1;
                }
                writeLog("ksp_InsertRemittance_TDD", Remittance, iReturn);
                return iReturn;
            }
            catch (Exception ex)
            {
                writeLog("ksp_InsertRemittance_TDD", ex.Message, -1);
                return -1;
            }
        }


        public void writeLog(string spName, string spParam, int ResponseCode)
        {
            string userID = string.Empty;
            if (System.Web.HttpContext.Current.Session["GFMCNo"] != null && System.Web.HttpContext.Current.Session["GFMCNo"].ToString() != string.Empty)
            {
                userID = System.Web.HttpContext.Current.Session["GFMCNo"].ToString();
            }
            else
            {
                userID = ((IRSClassLib.irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).UserId;
            }
            logger.writeAudit(userID, System.Web.HttpContext.Current.Session["InternetARMModule"].ToString(), "EXECUTE INSERT: [ErrorMessage=" + Message + "][ResponseCode=" + ResponseCode + "]", "sp: " + spName, "param: " + spParam, csAuditLog.Category.Insert);
        }

        public decimal getRemainingRemittance(string remitterid, string branchcode)
        {
            DataTable dt = base.getDataSet(base.getSqlCommand("select TDD_ClDays_Max from tbl_branch_setup where branchcode='" + branchcode + "'", false)).Tables[0];
            if (dt != null)
            {
                decimal maxAmount = Convert.ToDecimal(dt.Rows[0]["TDD_ClDays_Max"]);
                /*get total remittance for this month*/
                DateTime dtFrom = DateTime.Now;
                DateTime dtTo = DateTime.Now;
                dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));
                dtTo = dtTo.AddMonths(1);
                dtTo = dtTo.AddDays(-(dtTo.Day));

                DataTable dtRemittance = base.getDataSet(base.getSqlCommand("select SUM(GrossRemAmount) AS Amount FROM tbl_Remittance where RemitterCIFID='" + remitterid + "' and TranxDate between '" + dtFrom.ToString("MM/dd/yyyy") + "' and '" + dtTo.ToString("MM/dd/yyyy") + "' and remittingbranch='" + branchcode + "' ", false)).Tables[0];
                if (dtRemittance != null)
                {
                    decimal totalRemit = Convert.ToDecimal(dtRemittance.Rows[0]["Amount"]);
                    if (totalRemit > maxAmount)
                    {
                        return 0;
                    }
                    else
                    {
                        return maxAmount - totalRemit;
                    }
                }

            }
            return 0;
        }

        public decimal getTotalRemittance(string remitterid, string branchcode)
        {
            DataTable dt = base.getDataSet(base.getSqlCommand("SELECT ISNULL(SUM(GrossRemAmount),0) AS Amount FROM tbl_Remittance WHERE RemitterCIFID='" + remitterid + "' AND RemittingBranch='" + branchcode + "' AND DATEPART(mm,TranxDate) = DATEPART(mm,GETDATE()) ", false)).Tables[0];
            DataTable dt2 = base.getDataSet(base.getSqlCommand("SELECT ISNULL(TDD_ClDays_Max,0) AS TDD_ClDays_Max from tbl_branch_setup where branchcode='" + branchcode + "'", false)).Tables[0];


            if (dt != null)
            {
                if (!string.IsNullOrEmpty(dt2.Rows[0]["TDD_ClDays_Max"].ToString()))
                {
                    decimal amt = Convert.ToDecimal(dt.Rows[0]["Amount"].ToString());
                    decimal max = Convert.ToDecimal(dt2.Rows[0]["TDD_ClDays_Max"].ToString());
                    return max - amt;
                }
            }

            return 0;
        }

        public string submitClearingDays(int txn_seqid, string data)
        {
            DateTime dt = DateTime.Now;
            double clearing_days = 0;

            irsSqlCommons sql = new irsSqlCommons();

            string[] param = data.Split(new char[] { '|' });

            string query1 = "SELECT ClearingDays FROM tbl_charge_master WHERE " +
                            "BranchCode ='" + param[0] + "' " +
                            "AND ProductCode = '" + param[1] + "' " +
                            "AND ImplementationType = '" + param[2] + "' " +
                            "AND Currency = '" + param[3] + "' " +
                            "AND ChargeType = '" + param[4] + "' " +
                            "AND Paymode = '" + param[5] + "' " +
                            "AND (Deleted IS NULL OR Deleted = 0)";
            try
            {
                clearing_days = double.Parse(sql.execScalar(query1).ToString());
            }
            catch
            {
                return sql.Message;
            }

            dt = dt.AddDays(clearing_days);

            while (true)
            {
                if ((int)dt.DayOfWeek > 0 && (int)dt.DayOfWeek < 6) break;
                dt = dt.AddDays(1);
            }

            string query3 = "INSERT INTO tbl_remittance_other_details(REM_SEQ_ID,ach_clearing_date) VALUES('" + txn_seqid + "','" + dt.ToString("MM/dd/yyyy") + "') SELECT 1";

            return (execScalar(query3) == null ? sql.Message : "1");

        }

        public string getPassword(string username,string userid)
        {
            csPasswordGen pass = new csPasswordGen();
            return pass.generatePassword(username, userid);
        }

        public string EncryptPassword(string username,string userid)
        {
            string pass = getPassword(username, userid);
            string encpass =  irsUsers.getTriDes().Encrypt(pass);
            return encpass;
        }

        public string DecryptPassword(string pass)
        {
             return irsUsers.getTriDes().Decrypt(pass);
        }


        public string getDisclamer(string ProductCode)
        {
            if (ProductCode == "10") 
            {
                return "PLEASE READ. <br /> Credit to PNB Account Type Selected. <br /> 1. Money is debit from your US bank account the next banking after initiating the Remittance. <br /> 2. For Amount between US$50-1,000, the transaction will be processed immediately. <br /> Howeverfor amount more than US$1,000 but not exceeding US$2,500, the <br /> remittance transaction is held for two (2) US banking days to allow funds to clear and <br /> is then remitted to PNB Manila for processing. <br /> 3. Upon receiving the money, PNB Manila immediately deposits the funds to the <br /> recipient's PNB Bank Account.";
            }
            else if (ProductCode == "12" || ProductCode == "13")
            {
                return "PLEASE READ. <br /> Credit to Other Bank (Non-PNB Bank) Delivery Type Selected <br />. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. <br /> 2. For amount between US$50-1,000, the transaction will be processed immediately. <br /> Howeverfor amount more than US$1,000 but not exceeding US$2,500, the <br /> remittance transaction is held for two (2) US banking days to allow funds to clear and <br /> is then remitted to PNB Manila for processing. <br /> 3. Upon receiving the money, PNB Manila immediately deposits the funds to the <br /> recipient's Non-PNB Bank Account. <br /> 4. Crediting the funds to your beneficiaries Non-PNB Bank Account will depend upon <br /> the receiving bank. Some Non-PNB Banks will also charge their own transfer fees and <br /> will deduct the fee from the funds you sent to your beneficiary. <br /> NOTE: Philippine national or local holidays occuring during the remittance process will lengthen the remittance time.";
            }
            else if (ProductCode == "27" || ProductCode == "7" || ProductCode == "24" || ProductCode == "22")
            {
                return "PLEASE READ. <br /> Cash Pick Up Type Selected. <br />. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. <br /> 2. For amount between US$50-1,000, the transaction will be processed immediately. <br /> Howeverfor amount more than US$1,000 but not exceeding US$2,500, the <br /> remittance transaction is held for two (2) US banking days to allow funds to clear and <br /> is then remitted to PNB Manila for processing. <br /> 3. Upon receiving the money, PNB Manila immediately available for your beneficiary to pick up the remittance at any PNB Branches";
            }  
             else if (ProductCode == "16" || ProductCode == "9" || ProductCode == "8")
            {
                return "PLEASE READ. <br /> Door to Door Delivery Type Selected. <br />. 1. Money is debit from your Us bank account the next banking after initializing the Remittance. <br /> 2. For amount between US$50-1,000, the transaction will be processed immediately. <br /> Howeverfor amount more than US$1,000 but not exceeding US$2,500, the <br /> remittance transaction is held for two (2) US banking days to allow funds to clear and <br /> is then remitted to PNB Manila for processing. <br /> 3. Upon receiving the money, PNB Manila provides the funds and instructions on the <br /> same day to its courier services for delivery to your Beneficiaries doorstep <br /> (delivery time varies according to locations). <br /> NOTE: Philippine national or local holiday's occuring during the remittance process will lengthen the remittance time.";
            }
            return string.Empty;
        }



        public DataTable getBeneficiary(string RemitterCIFID, string userRole)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getBeneficiary", new Object[] { RemitterCIFID, userRole });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getBeneficiaryInfo(string RemitterCIFID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("spGetBeneficiaryInfo", new Object[] { RemitterCIFID });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public bool checkEmailExist(string emailadd)
        {
           DataSet ds = getDataSet(getSqlCommand("select 1 from tbl_cif_contacts where emailaddress='" + emailadd + "'", false));
           if (ds.Tables[0].Rows.Count >0)
           {
               return true;
           }
           return false;
        }




        public DataTable getProducts(string RemitterCIFID,string BeneCIFID,string userRole)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_tdd_getBeneficiary", new Object[] { RemitterCIFID, BeneCIFID, userRole });
            if (hasRows)
            {
                selectedRow = 0;
                return table;
            }
            else
            {
                return null;
            }
        }

        public bool getPaymentGateway(string Region)
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getPaymentGateway_tdd", new Object[] { Region });
            if (hasRows)
            {
                selectedRow = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        //public bool save()
        //{
        //    irsUsers user = new irsUsers();
        //    if(user.save_user("add", "")){ return true; }
        //    else {return false;}
        //}

     


    }
}
