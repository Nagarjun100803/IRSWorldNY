using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
    public class irsViewOtherChannels : sqlHelper
    {
        #region Local Variables

        DataSet data_set = null;
        private string error_msg = null;
        private int FILE_SEQ_ID;
        private int REC_SEQ_ID;
        private bool REC_VALID = false;

        #endregion

        #region PUBLIC Function

        public irsViewOtherChannels()
        {
        }

        public bool RecordValid
        {
            get
            {
                return REC_VALID;
            }
            set
            {
                REC_VALID = value;
            }
        }

        public int get_fileSEQ_ID
        {
            get
            {
                return FILE_SEQ_ID;
            }
            set
            {
                FILE_SEQ_ID = value;
            }
        }

        public int get_recSEQ_ID
        {
            get
            {
                return REC_SEQ_ID;
            }
            set
            {
                REC_SEQ_ID = value;
            }
        }

        public DataTable getDataTable(String sQuery)
        {
            DataTable dt = getDataSet(getSqlCommand(sQuery, false), "tbRecords").Tables[0];
            return dt;
        }

        public DataSet getDataSet(String sQuery, bool bStoreProc)
        {
            return getDataSet(getSqlCommand(sQuery, bStoreProc), "DT");
        }

        #endregion

        #region PRIVATE



        #endregion
    }
}