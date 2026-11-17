using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading;

namespace IRSClassLib
{
    public class irsBranch : sqlHelperWs
    {

        private SqlCommand search_cmd;

        public DataTable searchBR(String brNB, String brBC, String brSBC, String brAC, String brSAC, String brLoc, String brTZ)
        {
            try
            {
                search_cmd = getSqlCommand("sp_SearchUsers");
                //deriveParameters(search_cmd, new Object[] { catab[cix], args, brcode, (short)role, (short)flag, (short)logged });
                base.upds = getDataSet(search_cmd);
                if (isUpdatable)
                {
                    return table;
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }



         public irsBranch()
        {
            base.TableName = "TBL_BRANCH_SETUP";
            base.IdentityColumn = "SEQ_ID";

        }
    }
}
