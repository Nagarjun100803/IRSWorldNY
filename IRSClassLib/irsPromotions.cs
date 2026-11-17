using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using IRSClassLib;
using System.Collections;
using System.Web;
using System.IO;


namespace IRSClassLib
{
    [Serializable]
    public class irsPromotions : sqlHelperWs
    {
        //***********************************************
        public irsPromotions()
        {
            //base.IsWinApp = true;
        }
        //***********************************************
        
        private void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (!base.hasRows)
            {
                base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            else
            {
                if (IsAdd)
                {
                    base.newRow(sqlHelperWs.RETAIN_DATASET);
                }
                else
                {
                    selectedRow = 0;
                }
            }
        }

        #region Population of DropDownList
        public DataTable getPopulate_drop_down_list()
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_PromoGetAllStates", new Object[] { });
            if (hasRows)
            {
                return table;
            }
            else
            {
                return null;
            }
        }

        public DataTable getPopulate_drop_down_list_NotExisting(int SEQ_ID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_PromoddGetNotExistingState", new Object[] { SEQ_ID });
            if (hasRows)
            {

                return table;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Search, Insert, Update of PROMO CODE
        public DataTable getPromoCode(int SEQ_ID)
        {
            base.EnforceConstraint = false;
            getUpdateDS("vsp_PromoSelectRecord", new Object[] { SEQ_ID });
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

        public string insertPromoCode(int SEQ_ID, string promoCode, string promoDescription, string minTransCount, string minRemAmount, string startDate, string endDate, string promoContent, string promoTerms, bool IsAdd, int IsReceiveVoucher)
        {
            bool isExists;
            #region CHECK IF PROMO IS EXISTING

            base.EnforceConstraint = false;
            getUpdateDS("vsp_PromoCheckDuplicate", new Object[] { promoCode });
            if (hasRows)
            {
                selectedRow = 0;
                isExists = true;
            }
            else
            {
                isExists = false;
            }

            #endregion

            int xminTransCount = Convert.ToInt32(minTransCount);
            double xminRemAmount = Convert.ToDouble(minRemAmount);
            startDate = DateTime.Parse(startDate).ToString("MM/dd/yyyy");
            endDate = DateTime.Parse(endDate).ToString("MM/dd/yyyy");
            bool ok = false;
            string message = "";

            if (IsAdd == true)
            {
                base.addMode = true;
                if (isExists == true)
                {
                    return message = "Promo Code already exists.";
                }
            }
            else
            {
                base.addMode = false;
                if (isExists == false)
                {
                    return message = "Promo Code does not exist.";
                }
            }
            if (base.addMode)
            {
                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsHeader", IsAdd);

                setColumn("PromoCode", promoCode);
                setColumn("PromoDescription", promoDescription);
                setColumn("MinRemittanceAmount", xminRemAmount);
                setColumn("MinTransactionCount", xminTransCount);
                setColumn("StartDate", startDate);
                setColumn("EndDate", endDate);
                setColumn("PromoContent", promoContent);
                setColumn("PromoTerms", promoTerms);
                setColumn("Deleted", 0);
                setColumn("IsReceiveVoucher", IsReceiveVoucher); 
                
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (ok == true)
                    message = "Promo has been added.";
                else
                    message = "ERROR: Unable to add Promo.";
            }
            else
            {
                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsHeader WHERE Deleted = 0 and SEQ_ID = '" + SEQ_ID + "'", IsAdd);
                setColumn("PromoDescription", promoCode);
                setColumn("PromoDescription", promoDescription);
                setColumn("MinRemittanceAmount", minRemAmount);
                setColumn("MinTransactionCount", minTransCount);
                setColumn("StartDate", startDate);
                setColumn("EndDate", endDate);
                setColumn("PromoContent", promoContent);
                setColumn("PromoTerms", promoTerms);
                setColumn("IsReceiveVoucher", IsReceiveVoucher); 
                
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
                if (ok == true)
                    message = "Promo has been updated.";
                else
                    message = "ERROR: Unable to update Promo.";
            }
            return message;
        }

        public string removePromoCode(int SEQ_ID)
        {
            bool ok = false;
            string message = "";
            base.addMode = false;
            int status = 1;

            this.setBase("SELECT TOP 1 * FROM tbl_PromotionsHeader WHERE Deleted = 0 and SEQ_ID = " + SEQ_ID + "", false);
            setColumn("Deleted", status);
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            if (ok == false)
                return message = "ERROR: Unable to delete Promo.";

            #region Checks if tbl_PromotionsDetail has rows to update (true or false)
            //Check if tbl_PromotionsDetail has rows (vsp_PromoDetailsHasRows)
            base.EnforceConstraint = false;
            bool hasDetailsRow;
            getUpdateDS("vsp_PromoDetailsHasRows", new Object[] { SEQ_ID });
            if (hasRows)
            {
                selectedRow = 0;
                hasDetailsRow = true;
            }
            else
            {
                hasDetailsRow = false;
            }
            #endregion

            if (hasDetailsRow == true)
            {
                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsDetail WHERE Deleted = 0 and Promo_SeqID = " + SEQ_ID + "", false);
                setColumn("Deleted", status);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);
            }

            if (ok == true)
                return message = "Promo has been deleted.";
            else
                return message = "ERROR: Unable to delete Promo.";
        }
        #endregion

        #region Insert, Update of Promo State
        public string insertState(int promoSEQ_ID, string state, bool IsAdd)
        {
            //If All States Option is selected, 
            //add all states that are 
            //not yet included in the state list of the promo.
            if (state == "All States")
            {   //Insert to table All States not yet in the list
                DataTable dtState = new DataTable();
                string msg = insertmMultipleState(promoSEQ_ID, getPopulate_drop_down_list_NotExisting(promoSEQ_ID));
                return msg;
            }


            bool ok = false;
            string message = "";
            int status;
            if (IsAdd == true)
            {
                base.addMode = true;
                status = 0;//not deleted
            }
            else
            {
                base.addMode = false;
                status = 1;//deleted
            }
            if (base.addMode)
            {
                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsDetail", IsAdd);

                setColumn("Promo_SeqID", promoSEQ_ID);
                setColumn("stateCode", state);
                setColumn("Deleted", status);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);

                if (ok == true)
                    message = "State has been added.";
                else
                    message = "ERROR: Unable to add State";
            }
            else
            {
                string FldCode = "";

                #region Checks if tbl_PromotionsDetail has rows to update (true or false)
                //Check if tbl_PromotionsDetail has rows (vsp_PromoDetailsHasRows)
                base.EnforceConstraint = false;
                bool hasDetailsRow;
                getUpdateDS("vsp_getSpecificState", new Object[] { state });
                if (hasRows)
                {
                    selectedRow = 0;
                    FldCode = table.Rows[0]["FldCode"].ToString();
                }
                else
                {
                    FldCode = "";
                }
                #endregion

                this.setBase("SELECT TOP 1 * FROM tbl_PromotionsDetail WHERE Deleted = 0 and Promo_SeqID = '" + promoSEQ_ID + "' and stateCode = '" + FldCode + "'", IsAdd);
                setColumn("Deleted", status);
                ok = base.save(sqlHelperWs.RETAIN_DATASET);

                if (ok == true)
                    message = "State has been removed.";
                else
                    message = "ERROR: Unable to remove State";

            }

            return message;
        }

        public string insertmMultipleState(int promoSEQ_ID, DataTable dtState)
        {
            bool ok = false;
            string message = "";
            int status = 0;
            base.addMode = true;
            bool IsAdd = true;
            if (base.addMode)
            {
                for (int i = 0; i < dtState.Rows.Count; i++)
                {
                    string stateCodeList = dtState.Rows[i]["FldCode"].ToString();

                    this.setBase("SELECT TOP 1 * FROM tbl_PromotionsDetail", IsAdd);
                    setColumn("Promo_SeqID", promoSEQ_ID);
                    setColumn("stateCode", stateCodeList);
                    setColumn("Deleted", status);
                    ok = base.save(sqlHelperWs.RETAIN_DATASET);

                    if (ok == false) return message = "ERROR: Unable to add States.";

                }
                return message = "States has been added.";
            }

            return message = "ERROR: Unable to add States.";

        }

        public string updateState(int promoSEQ_ID, string fldValue, string fldCode, bool IsAdd)
        {
            bool ok = false;
            string message = "";
            int status = 1;

            this.setBase("SELECT TOP 1 * FROM tbl_PromotionsDetail WHERE Deleted = 0 and Promo_SeqID = '" + promoSEQ_ID + "' and stateCode = '" + fldCode + "'", IsAdd);
            setColumn("Deleted", status);
            ok = base.save(sqlHelperWs.RETAIN_DATASET);
            return message = "State has been removed.";

        }
        #endregion

        
    }
}
