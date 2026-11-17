using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsHoldChecks : sqlHelper
    {
        public void setCheckUnhold()
        {
            DataTable dt = this.getHoldChecks();
            DateTime dtBranchTime;
            string sReturn = string.Empty;
            int i = 0;
            int j = 0;

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i <= (dt.Rows.Count - 1); i++)
                    {
                        try
                        {
                            dtBranchTime = new irsTimeZone().branchLocalTime_nWs(dt.Rows[i]["RemittingBranch"].ToString().Trim());
                            j = dtBranchTime.CompareTo(Convert.ToDateTime(dt.Rows[i]["Clearing_Date"].ToString()));
                            if (j >= 0)
                            {
                                sReturn = setUnholdChecks(dt.Rows[i]["SEQ_ID"].ToString().Trim(), dt.Rows[i]["ReferenceNo"].ToString().Trim());
                                new WinAppLogger().writeAudit("IRSWorld", "HOLD CHECKS", "CLEARED CHECKS: [SEQ_ID=" + dt.Rows[i]["SEQ_ID"].ToString().Trim() + "][ReferenceNo=" + dt.Rows[i]["ReferenceNo"].ToString().Trim() + "][Branch Date=" + dt.Rows[i]["RemittingBranch"].ToString().Trim() + " - " + dtBranchTime.ToShortDateString() + "][ClearingDate=" + dt.Rows[i]["Clearing_Date"].ToString().Trim() + "][ReturnValue=" + sReturn + "]", "", "", WinAppLogger.Category.Update);
                            }
                        }
                        catch (Exception ex)
                        {
                            new WinAppLogger().writeAudit("IRSWorld", "HOLD CHECKS", "ERROR CLEARED CHECKS: [SEQ_ID=" + dt.Rows[i]["SEQ_ID"].ToString().Trim() + "][ReferenceNo=" + dt.Rows[i]["ReferenceNo"].ToString().Trim() + "][ClearingDate=" + dt.Rows[i]["Clearing_Date"].ToString().Trim() + "][ErrorMessage=" + ex.Message + "]", "", "", WinAppLogger.Category.Update);
                        }
                    }
                }
            }
        }

        public DataTable getHoldChecks()
        {
            base.EnforceConstraint = false;
            getUpdateDS("isp_getHoldChecks", new Object[] { });
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

        public string setUnholdChecks(string SEQ_ID, string ReferenceNo)
        {
            base.EnforceConstraint = false;
            string[] arr = new string[2];
            arr.SetValue(SEQ_ID, 0);
            arr.SetValue(ReferenceNo, 1);
            return execStoredProcedureUpdate("isp_setUnholdChecks", arr).ToString();
        }
    }
}
