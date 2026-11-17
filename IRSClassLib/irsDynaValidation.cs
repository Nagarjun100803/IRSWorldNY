using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsMandatoryFields : sqlHelperWs
    {
        public void getMandatoryFields(String brcode, short cifclass, String cifmenu)
        {
            string sql = "select * from tbl_cif_mandatory_fields where BranchCode = '" + brcode.Trim() + "'";
            SqlCommand cmd = new SqlCommand();
            cmd = base.getSqlCommand(sql,false);
            base.getUpdateDataSet(cmd);
            
            // base.getUpdateDS("sp_get_cif_mandatory_fields", new Object[] { brcode, cifclass, cifmenu });
        }

        public irsMandatoryFields()
        {
            this.TableName = "TBL_CIF_MANDATORY_FIELDS";
            this.IdentityColumn = "SEQ_ID";
        }

        private bool do_add(Hashtable ht)
        {
            bool ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    Object val = ht[key];
                    if (!setColumn(key.ToString(), val))
                    {

                    }
                }
                if (ok & addMode)
                {

                    addMode = false;
                    table.Rows.Add(uprow);
                    base.save(sqlHelperWs.RETAIN_DATASET);
                    
                }
            }
            return ok;
        }

        private int findRow(String brcode, int cifclass, int cifseqid)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i].RowState == DataRowState.Deleted) continue;
                String row_brcode = (String)table.Rows[i]["BranchCode"];
                int row_class = Convert.ToInt32(table.Rows[i]["CifClass"]);
                int row_seqid = Convert.ToInt32(table.Rows[i]["CifFieldsSeqId"]);
                if (row_brcode.Equals(brcode) & row_class == cifclass & row_seqid == cifseqid)
                {
                    return i;
                }
            }

            return -1;
        }

        private int findRow_seq_id(String brcode, int cifclass, int cifseqid)
        {
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i].RowState == DataRowState.Deleted) continue;
                String row_brcode = (String)table.Rows[i]["BranchCode"];
                int row_class = Convert.ToInt32(table.Rows[i]["CifClass"]);
                int row_seqid = Convert.ToInt32(table.Rows[i]["CifFieldsSeqId"]);
                int row_seq_id = Convert.ToInt32(table.Rows[i]["SEQ_ID"]);
                if (row_brcode.Equals(brcode) & row_class == cifclass & row_seqid == cifseqid)
                {
                    return row_seq_id;
                }
            }

            return -1;
        }


        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            Object oid = ht["SEQ_ID"];
            String stat = (String)ht["Required"];
            String BranchCode = (String)ht["BranchCode"];
            int frow = -1;

            int sid = 0;
            try
            {
                sid = Convert.ToInt32(oid);
            }
            catch
            {
                sid = -1;
            }

            this.getMandatoryFields(BranchCode, -1, "");
       
            if (sid < 1 & stat.Equals("REQUIRED"))
            {
                return do_add(ht);
            }
       
            if (sid > 0)
            {
                frow = findRow(ht["BranchCode"].ToString(), Convert.ToInt32(ht["CifClass"]), Convert.ToInt32(ht["CifFieldsSeqId"]));
            }

            if (sid > 0 & stat.Equals("REQUIRED") & frow == -1)
            {
                return do_add(ht);
            }

            /*
            if (oid.ToString() == "null" & stat.Equals("REQUIRED"))
            {
                return do_add(ht);
            }
            */

            if (sid > 0 & stat.Equals("NOT REQUIRED"))
            {
                try
                {
                    //int frow = findRow( ht["BranchCode"].ToString(), Convert.ToInt32(ht["CifClass"]), Convert.ToInt32(ht["CifFieldsSeqId"]));
                    if (frow > -1)
                    {
                        base.selectedRow = frow;
                        return base.Delete(sqlHelperWs.RETAIN_DATASET, true);                        
                    }
                    //Message = "Unable to find SEQ_ID: " + frow;
                    //return false;
                    return true;
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return false;
                }
            }
        
            return true;
        }

        public bool saveToTemp(String crud, int row, Hashtable ht, string userid)
        {
            String stat = (String)ht["Required"];
            String BranchCode = (String)ht["BranchCode"];
            String CifFieldsSeqId = (String)ht["CifFieldsSeqId"];
            String CifClass = (String)ht["CifClass"];
            string isRequired = (stat.Equals("REQUIRED")) ? "1" : "0";
            Int64 frow = -1;

            this.getMandatoryFields(BranchCode, -1, "");

            frow = findRow_seq_id(ht["BranchCode"].ToString(), Convert.ToInt32(ht["CifClass"]), Convert.ToInt32(ht["CifFieldsSeqId"]));

            string dateNow = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            bool ok = false;

            try
            {
                base.addMode = true;
                this.setBase("SELECT TOP 1 * FROM TBL_CIF_MANDATORY_FIELDS_TMP", true);
                setColumn("MAIN_SEQ_ID", frow);
                setColumn("BranchCode", BranchCode);
                setColumn("CifClass", CifClass);
                setColumn("CifFieldsSeqId", CifFieldsSeqId);
                setColumn("isRequired", isRequired);
                setColumn("Maker", userid);
                setColumn("DateAdded", dateNow);
                setColumn("CurrStatus", "For Approval");
                setColumn("StatusDate", dateNow);
                setColumn("ActionType", "EDIT");
                setColumn("Module", "Branch Administration >> Dynamic Validation");
                setColumn("MenuName", "Dynamic Validation");

                ok = base.save(sqlHelperWs.RETAIN_DATASET);
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                ok = false;
            }


            return ok;
        }

        public void setBase(string SQL, bool IsAdd)
        {
            base.getUpdateDataSet(getSqlCommand(SQL, false));
            if (!base.hasRows)
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

        public DataTable saveUpdates(string branchcode, string cifclass, string cifmenu)
        {
            if (dataset.HasChanges())
            {
                base.save(sqlHelperWs.RETAIN_DATASET);
            }

            //return ((irsDynaValidation)System.Web.HttpContext.Current.Session["IRS_DYNA_VALIDATION"]).get_mandatory_fields(branchcode,cifclass, cifmenu);
            return ((irsDynaValidation)System.Web.HttpContext.Current.Session["IRS_DYNA_VALIDATION"]).load_dynaval_info(branchcode, cifclass, cifmenu).Tables[0];
        }


    }

    [Serializable]
    internal class irsModuleFields : sqlHelperWs
    {

        internal void getModuleFields(String cifmenu)
        {
            base.getUpdateDS("sp_get_cif_module_fields", new Object[] { cifmenu });
        }

        internal irsModuleFields()
        {
            this.TableName = "TBL_CIF_MODULE_FIELDS";
            this.IdentityColumn = "SEQ_ID";
        }
    }

    [Serializable]
    public class irsDynaValidation : irsSqlCommonsWs
    {
        private irsMandatoryFields imf = new irsMandatoryFields();
        private irsModuleFields imod;
        private DataTable mtab; 

        private void define_table()
        {
            mtab = new DataTable();
            mtab.Columns.Add(new DataColumn("SEQ_ID", typeof(System.Int16)));
            mtab.Columns.Add(new DataColumn("BranchCode", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("CifClass", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("CifClassDescription", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("CifFieldsSeqId", typeof(System.Int16)));
            mtab.Columns.Add(new DataColumn("CifMenu", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("CIFTable", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("CIFLabel", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("CIFField", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("ClassMenu", typeof(System.String)));
            mtab.Columns.Add(new DataColumn("Required", typeof(System.Int16))); // 1 - Required; 2 - not
        }

        private DataTable get_lookup_class(short cifclass)
        {
            return getLookup(new Object[] { "CIFClass", DBNull.Value, cifclass });
        }

        private DataTable do_for_cifclass(DataTable dtimod, String brcode, short cifclass)
        {
            DataTable dtlookup = get_lookup_class(cifclass);
            if (dtlookup == null)
            {
                Message = "Invalid cifclass " + cifclass;
                return null;
            }

            DataTable dtclon = mtab.Clone();

            foreach (DataRow lrow in dtlookup.Rows)
            {
                for (int i = 0; i < dtimod.Rows.Count; i++)
                {
                    DataRow mrow = dtclon.NewRow();
                    mrow["SEQ_ID"] = 0;
                    mrow["BranchCode"] = brcode;
                    mrow["CifClass"] = lrow["fldValue"];
                    mrow["CifClassDescription"] = lrow["fldCode"];
                    mrow["CifFieldsSeqId"] = dtimod.Rows[i]["SEQ_ID"];
                    mrow["CifMenu"] = dtimod.Rows[i]["CifMenu"];
                    mrow["CIFTable"] = dtimod.Rows[i]["CIFTable"];
                    mrow["CIFLabel"] = dtimod.Rows[i]["CIFLabel"];
                    mrow["CIFField"] = dtimod.Rows[i]["CIFField"];
                    mrow["ClassMenu"] = lrow["fldCode"].ToString() + dtimod.Rows[i]["CifMenu"].ToString();
                    mrow["Required"] = 2;

                    dtclon.Rows.Add(mrow);
                }
            }

            return dtclon;
        }
       
        private DataTable do_for_cifclass(DataTable dtimf, DataTable dtimod, String brcode, short cifclass)
        {
            DataTable dt = do_for_cifclass(dtimod, brcode, cifclass);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.AllowEdit = true;
                dv.Sort = "CifClass, CifFieldsSeqId";
                int seq = 1;
                foreach (DataRow row in dtimf.Rows)
                {
                    short cifclaz = (short)row["CifClass"];
                    int cifsid = (int)row["CifFieldsSeqId"];
                    int i = dv.Find(new Object[] { cifclaz, cifsid });
                    if (i > -1)
                    {
                        dv.Table.Rows[i]["SEQ_ID"] = seq;
                        dv.Table.Rows[i]["Required"] = 1;
                        seq++;
                    }
                }
            }
            return dt;
        }

      

        public DataTable get_mandatory_fields(String brcode, Int32 cifclass, String cifmenu)
        {
            DataTable dt = null;

            if (imf == null) imf = new irsMandatoryFields();
            imf.getMandatoryFields(brcode, (short)cifclass, cifmenu);
            if (imf.isUpdatable)
            {

                if (imod == null) imod = new irsModuleFields();
                imod.getModuleFields(cifmenu);
                if (imod.isUpdatable)
                {
                    if (imf.table.Rows.Count > 0)
                    {
                        dt = do_for_cifclass(imf.table, imod.table, brcode, (short)cifclass);
                    }
                    else
                    {
                        dt = do_for_cifclass(imod.table, brcode, (short)cifclass);
                    }
                }

                imod = null;
            }

            return dt;
        }

        public DataTable get_initial_data(String brcode)
        {
            return get_mandatory_fields(brcode, -1, "");
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            return imf.applyUpdates(crud,row,ht);
        }

        public bool saveToTemp(String crud, int row, Hashtable ht, string userid)
        {
            return imf.saveToTemp(crud, row, ht, userid);
        }

        public DataTable saveUpdates(string branchcode, string cifclass, string cifmenu)
        {
            return imf.saveUpdates(branchcode, cifclass, cifmenu);
        }

        public DataSet get_ddl_menu(string cifclass)
        {
            //return getDataSet(getSqlCommand("sp_dynavalmenu"));
            DataSet ds = new DataSet();
            DataSet dsTemp = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("sp_cif_dynavalmenu");
                cmd.Parameters.Add("@CIFClass", SqlDbType.VarChar, 20).Value = cifclass;
                ds = base.getDataSet(cmd);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt;
                    DataRow dr = null;
                    dt = new DataTable("CIFTable");
                    dt.Columns.Add("CIFMENUS");
                    
                    string s1 = ds.Tables[0].Rows[0][0].ToString();                    
                    foreach (String s in s1.Split('~'))
                    {
                        dr = dt.NewRow();
                        dr["CIFMENUS"] = s;
                        dt.Rows.Add(dr);
                        dt.AcceptChanges();
                    }
                    dsTemp.Tables.Add(dt); 
                    
                }
                return dsTemp;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
            }
        }

        public DataSet get_ddl_class()
        {
            return getDataSet(getSqlCommand("sp_class"));
        }

        public DataSet get_branches(string branchcode)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand cmd = base.getSqlCommand("get_dropdown_branches_byaccess");
                cmd.Parameters.Add("@Location", SqlDbType.VarChar, 5).Value = branchcode;
                ds = base.getDataSet(cmd);
                return ds;
            }
            catch
            {
                return null;
            }
            finally
            {
                ds = null;
            }
        }

        public irsDynaValidation()
        {
            define_table();
        }

        #region modification 07.19.2010
        public DataSet load_dynaval_info(string branchcode, string classtype, string cifmodule)
        {                
            SqlCommand cmd = new SqlCommand();
            cmd = base.getSqlCommand("sp_get_cifmodule_fields_entry");
            cmd.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 10).Value = branchcode;
            cmd.Parameters.Add("@v_cifclass", SqlDbType.VarChar, 10).Value = classtype;
            cmd.Parameters.Add("@v_cif_modules", SqlDbType.VarChar, 150).Value = cifmodule;
            return base.getDataSet(cmd);
        }
        #endregion
    }
}
