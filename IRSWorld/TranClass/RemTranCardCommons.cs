using System;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.TranClass
{
    [Serializable()]
    public class RemTranCardCommons
    {
        private String _msg = "";

        public String ErrMsg
        {
            get { return _msg; }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDataTable(String SQL)
        {
            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();

                com.EnforceConstraint = false;

                DataSet ds = com.getDataSet(com.getSqlCommand(SQL, false), "TABLE");

                return ds.Tables[0];                
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable: " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable ExecProcedure(String ProcName, String ProcParam)
        {
            Object[] plist = ProcParam.Split('~');

            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();
                com.EnforceConstraint = false;
                SqlCommand cmd = com.getSqlCommand(ProcName, true);
                com.deriveParameters(cmd, plist);
                DataTable dt = com.getDataSet(cmd, "TABLE").Tables[0];

                if (com.Message != null && com.Message != String.Empty)
                {
                    throw new Exception(com.Message);
                }
                          
                return dt;
                 
            }
            catch (Exception e)
            {
                throw new Exception("Failed to get datatable : " + e.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String ExecProcedureNoReturn(String ProcName, String ProcParam)
        {
            Object[] plist = ProcParam.Split('~');

            try
            {
                irsSqlCommonsWs com = new irsSqlCommonsWs();
                SqlCommand cmd = com.getSqlCommand(ProcName, true);
                com.deriveParameters(cmd, plist);

                int ra = com.sqlUpdate(cmd);

                if (ra > 0)
                {
                    return "";
                }
                else
                {
                    if (com.Message != null && com.Message != String.Empty)
                    {
                        throw new Exception(com.Message);
                    }
                    else
                    {
                        throw new Exception("No record affected");
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                //throw new Exception("Failed to update data : " + e.Message);
                return "Failed to update data : " + e.Message;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public String transact(String SEQ_ID, String Card_SEQ_ID, String SecurityCode)
        {
            String Response = String.Empty;
            String errmsg = String.Empty;
            try
            {                
                DataRow dr = ExecProcedure("sp_getCardTrxnDetails", SEQ_ID + "~" + Card_SEQ_ID).Rows[0];
                
                String Region = dr["Region"].ToString();
                String CIFID = dr["CIFID"].ToString();
                String CardNo = dr["CardNo"].ToString();
                String CardName = dr["CardName"].ToString();
                String ExpiryDate = dr["ExpiryDate"].ToString();
                String CardType = dr["CardType"].ToString();
                String CurrencyCode = dr["Currency"].ToString();
                String Amount = dr["Amount"].ToString();
                String Ref_num = dr["Refnum"].ToString();
                String Branch_AccountNumber = dr["Branch_AccountNumber"].ToString();
                String Benef_AccountNumber = dr["Benef_AccountNumber"].ToString();
                String ProductCode = dr["ProductCode"].ToString();
                String ImplementationType = dr["ImplementationType"].ToString();
                String BranchCode = dr["BranchCode"].ToString(); 
                

                irsPaymentGateway gateway = new irsPaymentGateway();

                Response = gateway.createRequest(Region, BranchCode, CIFID, CardNo, CardName, ExpiryDate, SecurityCode, CardType, CurrencyCode, Amount, "IRS World Card Payment", "");
                
                //Response = Response.Replace('~', ' ');
                
                if (Response != null && Response != "" && Response != "undefined" && Response != "|||" && Response != "||||")
                {
                    return Response;
                    //String[] arrResponse = Response.Split('|');
                    //if (arrResponse[0] == "OK")
                    //{
                    //    //registering card response
                    //    if (!registerCardResponse(SEQ_ID, Card_SEQ_ID, arrResponse[1], Response, arrResponse[2], true))
                    //    {
                    //        throw new Exception(_msg);
                    //    }
                    //    // Crediting for DC Transaction.
                    //    if (ProductCode == "10" & ImplementationType == "2")
                    //    {
                    //        irsAgent agent = new irsAgent();

                    //        if (!agent.do_fundtransfer(Ref_num, Branch_AccountNumber, Benef_AccountNumber, Amount, Ref_num, int.Parse(SEQ_ID), DateTime.Now))
                    //        {
                    //            if (agent.messages != null && agent.messages != String.Empty)
                    //            {
                    //                throw new Exception(agent.messages);
                    //            }
                    //        }
                    //    }
                    //}
                    ////return response to go to 3DSecure Page
                    //else if (arrResponse[0] == "3DSECURE")
                    //{
                    //    return Response;
                    //}

                    ////registering card response
                    //if (!registerCardResponse(SEQ_ID, Card_SEQ_ID, arrResponse[2], arrResponse[3], arrResponse[1], true))
                    //{
                    //    throw new Exception(_msg);
                    //}

                    // return response with no error;
                    
                }
                else
                {
                    //return Response + "~" + "There transaction received an error. \r\n\r\n" + "" + "\r\n\r\n Please report to the nearest PNB branch.";
                    throw new Exception("There transaction received an error. \r\n\r\n" + "" + "\r\n\r\n Please report to the nearest PNB branch.");
                }
                
            }
            catch (Exception ex)
            {
                //return Response + "~" + "Failed to submit transaction, Reason : " + ex.Message;
                throw new Exception("Failed to submit transaction, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool do_DC_Crediting(String SEQ_ID, String Card_SEQ_ID, String TransactionDate)
        {
            DataRow dr = ExecProcedure("sp_getCardTrxnDetails", SEQ_ID + "~" + Card_SEQ_ID).Rows[0];

            String Region = dr["Region"].ToString();
            String CIFID = dr["CIFID"].ToString();
            String CardNo = dr["CardNo"].ToString();
            String CardName = dr["CardName"].ToString();
            String ExpiryDate = dr["ExpiryDate"].ToString();
            String CardType = dr["CardType"].ToString();
            String CurrencyCode = dr["Currency"].ToString();
            String Amount = dr["Amount"].ToString();
            String Ref_num = dr["Refnum"].ToString();
            String Branch_AccountNumber = dr["Branch_AccountNumber"].ToString();
            String Benef_AccountNumber = dr["Benef_AccountNumber"].ToString();
            String ProductCode = dr["ProductCode"].ToString();
            String ImplementationType = dr["ImplementationType"].ToString();
            String BranchCode = dr["BranchCode"].ToString(); 

            // Crediting for DC Transaction.
            /* E2EAPICHANGE 20220209 Updated Code */
            /*
            if (ProductCode == "10" & ImplementationType == "2")
            {
                irsAgent agent = new irsAgent();
                if (!agent.do_fundtransfer(Ref_num, Branch_AccountNumber, Benef_AccountNumber, Amount, Ref_num, int.Parse(SEQ_ID), DateTime.Parse(TransactionDate)))
                {
                    if (agent.messages != null && agent.messages != String.Empty)
                    {
                        throw new Exception(agent.messages);
                    }
                }
            }
            */
            return true;
        }

                
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public DataTable getDDLDataTable(String CurrCode, String CIFID)
        {
            String SQL = "";
            SQL = "select [Text] = isnull(CardName,''), [Number] = isnull(CardNo, ''), [Value] = isnull(SEQ_ID,'') ";
            SQL += "from TBL_CIF_CARDS ";
            SQL += "where CIFID = '" + CIFID + "' ";
            SQL += "and Currency = " + CurrCode + " ";
            SQL += "and CardName not like '%GFMC%' ";
            SQL += "and isnull(CardNo,'') <> '' ";
            SQL += "order by SEQ_ID";

            return getDataTable(SQL);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
        public bool registerCardResponse(String remSeqID, String cardSeqId, String Message, String Response, String AuthCode, bool isCard)
        {
            try
            {
                String param = remSeqID + "~" + cardSeqId + "~" + Message + "~" + Response + "~" + AuthCode + "~" + (isCard ? "TRUE" : "FALSE");
                String errmsg = ExecProcedureNoReturn("sp_save_card_responses", param);

                if (errmsg != "")
                {
                    throw new Exception(errmsg);
                }

                _msg = String.Empty;
                return true;
            }
            catch(Exception ex)
            {
                _msg = "Problem encountered while registering card response, Reason : " + ex.Message;
                return false;                
            }
        }


        public bool registerRemitterDebitCard(String remSeqID, String cardSeqId)
        {
            try
            {
                String param = remSeqID + "~" + cardSeqId;

                String errmsg = ExecProcedureNoReturn("sp_registerRemitterDebitCard", param);

                if (errmsg != "")
                {
                    throw new Exception(errmsg);
                }

                _msg = String.Empty;
                return true;
            }
            catch (Exception ex)
            {
                _msg = "Problem encountered while registering remitter debit card, Reason : " + ex.Message;
                return false;
            }
        }
    }
}
