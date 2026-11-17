using System;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    public class irsGenkinaketome : sqlHelperWs
    {
        

        public string getRemitterName(int seq_id)
        {
            object o = base.execScalar("SELECT LastName + ',' + FirstName + ' ' + MiddleName AS RemitterName FROM TBL_CIF_MASTER WHERE SEQ_ID='" + seq_id + "'");
            return (o==null) ? string.Empty : o.ToString();
        }


    }
}
