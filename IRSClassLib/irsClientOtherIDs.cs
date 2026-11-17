using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
	[Serializable]
	public class irsClientOtherIds : sqlHelperWs
    {
        #region PRIVATE VARIABLES

        private string error_message = null;
        private string _seq_id = null;
        private string _cifid = null;
        private string _other_ids = null;
        private string _irswsys = null;
        private string _refnotheirs = null;
        private DataSet data_set = null;

        #endregion

        #region PROPERTY GET & SET

        public string messages
        {
            get { return this.error_message; }
        }

        public string seq_id
        {
            get { return this._seq_id; }
            set { _seq_id = value; }
        }

        public string cifid
        {
            get { return this._cifid; }
            set { _cifid = value; }
        }

        public string otherids
        {
            get { return this._other_ids; }
            set { _other_ids = value; }
        }

        public string irswsys
        {
            get { return this._irswsys; }
            set { _irswsys = value; }
        }

        public string refnotheirs
        {
            get { return this._refnotheirs; }
            set { _refnotheirs = value; }
        }

        #endregion

        #region PUBLIC METHOD

        public string getRecord(object obj)
        {
            if (obj != null)
            {
                string s = Convert.ToString(obj);

                if (s != null && s.Trim() == string.Empty)
                {
                    return null;
                }
                return s.Replace("'", "''");
            }

            return null;
        }

        public bool get_record()
        {
            this.getUpdateDS(Convert.ToInt32(base.IdentityValue));

            if (dataset != null && dataset.Tables[0].Rows.Count > 0)
            {
                base.selectedRow = 0;

                _seq_id = this.getRecord(base.getColumn("SEQ_ID"));
                _cifid = this.getRecord(base.getColumn("CIFID"));
                _other_ids = this.getRecord(base.getColumn("OtherIDs"));
                _irswsys = this.getRecord(base.getColumn("IRSWorldSystem"));
                _refnotheirs = this.getRecord(base.getColumn("RefNoTheirs"));

                this.data_set = dataset; return true;
            }
            return false;
        }

        public bool set_record(string[] column_values)
        {
            ArrayList error_list = new ArrayList();

            for (int i = 0; i < column_values.Length; i++)
            {
                string[] s = column_values[i].Split('~');

                if (s != null && s.Length > 0)
                {
                    if (s[1] == null || s[1].Trim().Length == 0 || s[1].ToString().Trim().ToUpper() == "NULL" || s[1].ToString().Trim() == string.Empty)
                    {
                        s[1] = null;
                    }
                    if (!base.setColumn(s[0].Trim(), s[1]))
                    {
                        error_list.Add(s[0] + ":" + this.Message);
                    }
                }
            }

            if (error_list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (Object errors in error_list)
                {
                    sb.Append((String)errors + "\n");
                }

                this.error_message = string.Empty;
                this.error_message = sb.ToString();

                return false;
            }

            return true;
        }

        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT * FROM TBL_CIF_OTHER_IDS WHERE SEQ_ID = " + id, false);
            getUpdateDataSet(command);
        }

        public bool insert_record()
        {
            if (base.newRow() == true)
            {
                return true;
            }
            return false;
        }

        public bool commit_record()
        {

            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }

            this.rollback_record();
            this.get_error_dbase(); return false;
        }

        public bool update_record()
        {
            base.IdentityValue = this.seq_id;

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public void rollback_record()
        {
            base.cancel();
        }

        private void get_error_dbase()
        {
            this.error_message = string.Empty;

            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }

        #endregion

        public bool beneid_exist(String beneid, String remid)
        {
            return false;
        }

        public short getIRSWorldSystemID(String sysabbr)
        {
            DataTable dt = base.getLookup(new Object[] { "IRSWorldSystem", sysabbr, -1 });
            if (dt == null || dt.Rows.Count == 0) return -1;

            return Convert.ToInt16(dt.Rows[0]["fldValue"]);
        }

        public bool id_exist(String otherid, short irswsys)
        {
            try
            {
                String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE OtherIDs = '" + otherid.Trim() + "'";
                if (irswsys > -1)
                {
                    sql += " AND IRSWorldSystem = " + irswsys;
                }
                return (Convert.ToInt32(execScalar(sql)) > 0);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool ref_no_exist(String refno, short irswsys)
        {
            try
            {
                String sql = "SELECT COUNT(*) FROM " + TableName + " WHERE RefNoTheirs = '" + refno.Trim() + "'";
                if (irswsys > -1)
                {
                    sql += " AND IRSWorldSystem = " + irswsys;
                }
                return (Convert.ToInt32(execScalar(sql)) > 0);
            }
            catch(Exception e) 
            {
                Message = e.Message;
            }
            return false;
        }


        public bool id_exist(String otherid)
        {
            return id_exist(otherid, -1);
        }

        private String getBeneficiaryByRefnoTheirsSqlString(String refno_theirs, short irswsys)
        {
            return
                "select distinct o.IRSWorldSystem, o.OtherIDs, m.CIFID, c.CIFClass, m.FirstName, m.LastName, b.RemitterCIFID " +
                    "from	tbl_cif_other_ids o inner join " +
                    "tbl_cif_beneficiaries b on o.cifid = b.cifid inner join "  +
                    //and b.remitterCIFID in " +
                    //" (select CIFID from tbl_cif_other_ids where IRSWorldSystem = " + irswsys + " and RefNoTheirs = '" + refno_theirs + "') inner join " +
                    "tbl_cif_master m on m.cifid = b.cifid inner join " +
                    "tbl_cif_class c on m.cifid = c.cifid and c.cifclass = 2 " +
                "where o.IRSWorldSystem = " + irswsys + " and o.RefNoTheirs = '" + refno_theirs + "'";
        }

        private String getBeneficiaryByRefnoTheirsSqlString2(String refno_theirs, short irswsys)
        {
            return
                "select distinct o.IRSWorldSystem, o.OtherIDs, m.CIFID, c.CIFClass, m.FirstName, m.LastName, b.RemitterCIFID " +
                    "from	tbl_cif_other_ids o inner join " +
                    "tbl_cif_beneficiaries b on b.remittercifid = o.cifid inner join " +
                //and b.remitterCIFID in " +
                //" (select CIFID from tbl_cif_other_ids where IRSWorldSystem = " + irswsys + " and RefNoTheirs = '" + refno_theirs + "') inner join " +
                    "tbl_cif_master m on m.cifid = b.cifid inner join " +
                    "tbl_cif_class c on m.cifid = c.cifid and c.cifclass = 2 " +
                "where o.IRSWorldSystem = " + irswsys + " and o.RefNoTheirs = '" + refno_theirs + "'";
        }

        public DataTable getBeneficiaryByRefnoTheirs(String refno_theirs, short irswsys)
        {
            DataSet ds = getDataSet(getSqlCommand(getBeneficiaryByRefnoTheirsSqlString(refno_theirs, irswsys), false));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            else 
            {
                DataSet ds2 = getDataSet(getSqlCommand(getBeneficiaryByRefnoTheirsSqlString2(refno_theirs, irswsys), false));
                if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                {
                    return ds2.Tables[0];
                }
                return null;
            }
        }

        private String getRemitterByRefnoTheirsSqlString(String refno_theirs, short irswsys, String remcifid)
        {
            return 
                "select distinct o.IRSWorldSystem, o.OtherIDs, m.CIFID, c.CIFClass, m.FirstName, m.LastName, null as RemitterCIFID " +
                    "from	tbl_cif_other_ids o inner join " +
		            //"tbl_cif_remitters r on o.cifid = r.cifid inner join " +
		            "tbl_cif_master m on o.cifid = m.cifid inner join " +
		            "tbl_cif_class c on m.cifid = c.cifid and c.cifclass = 1 " +
                    "where o.IRSWorldSystem = " + irswsys + 
                     //and o.RefNoTheirs = '" + refno_theirs + "' 
                     " and o.cifid = '" + remcifid + "'";        
        }

        public DataTable getRemitterByRefnoTheirs(string refno_theirs, short irswsys, string remitter_cifid)
        {
            DataSet ds = getDataSet(getSqlCommand(getRemitterByRefnoTheirsSqlString(refno_theirs, irswsys, remitter_cifid), false));
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;        
        }

        public DataTable getCifIDByOtherIDs(String other_id)
        {
            String sql = "select distinct x.CIFClass, y.CIFID from tbl_cif_class x inner join" +
                          " tbl_cif_other_ids y on x.cifid = y.cifid where y.otherids = '" + other_id + "'";
            DataSet ds = getDataSet(getSqlCommand(sql, false));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }

            return null;
        }

        private DataTable define_retdt()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("RefNoTheirs", typeof(System.String)));
            dt.Columns.Add( new DataColumn("CIFID", typeof(System.String)));
            dt.Columns.Add( new DataColumn("OtherID", typeof(System.String)));
            dt.Columns.Add( new DataColumn("CIFClass", typeof(System.Int16)));
            return dt;
        }

        public DataTable check_enroll(String refno_theirs, short irswsys)
        {
            try
            {
                DataTable dtbene = this.getBeneficiaryByRefnoTheirs(refno_theirs, irswsys);
                if (dtbene != null && dtbene.Rows.Count > 0)
                {
                    String remitter_cifid = dtbene.Rows[0]["RemitterCIFID"].ToString();
                    DataTable dtrem = this.getRemitterByRefnoTheirs(refno_theirs, irswsys, remitter_cifid);
                    if (dtrem != null && dtrem.Rows.Count > 0)
                    {
                        DataTable dtret = define_retdt();

                        DataRow uprow = dtret.NewRow();
                        uprow["RefNoTheirs"] = refno_theirs;
                        uprow["CIFID"] = dtbene.Rows[0]["CIFID"];
                        uprow["OtherID"] = dtbene.Rows[0]["OtherIDs"];
                        uprow["CIFClass"] = dtbene.Rows[0]["CIFClass"];
                        dtret.Rows.Add(uprow);

                        uprow = dtret.NewRow();
                        uprow["RefNoTheirs"] = refno_theirs;
                        uprow["CIFID"] = dtrem.Rows[0]["CIFID"];
                        uprow["OtherID"] = dtrem.Rows[0]["OtherIDs"];
                        uprow["CIFClass"] = dtrem.Rows[0]["CIFClass"];
                        dtret.Rows.Add(uprow);

                        return dtret;
                    }
                    else
                    {
                        Message = "Unable to find row from remitter CIFID: " + remitter_cifid;
                    }
                }
                else
                {
                    Message = "Unable to get the beneficiary record. ";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return null;
        }



        public irsClientOtherIds()
        {
            TableName = "TBL_CIF_OTHER_IDS";
            IdentityColumn = "SEQ_ID";
            this.error_message = null;
        }
    }

}
