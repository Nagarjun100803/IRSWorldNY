using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web.Configuration;
using System.Text;
using System.Data;

namespace IRSClassLib
{
    [Serializable]
    public class irsFCRSettings : sqlHelperWs
    {
        private String curr_setting_id;
        private bool has_delete = false;

        public bool deleteSettings(String setting_id)
        {
            try
            {
                String sql = "DELETE FROM " + TableName + " WHERE SettingsID = '" + setting_id + "'";
                base.cancel(sqlHelperWs.DISPOSE_DATASET);
                int ra = base.sqlUpdate(sql);

                return (ra > 0);
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        private void defineSettingsTable(String setting_id)
        {
            this.curr_setting_id = setting_id;

            String sqlstm = "SELECT * FROM " + TableName + " WHERE SettingsID = '" + setting_id + "'";
            base.getUpdateDS(sqlstm);
        }

        private DataTable clone_settings()
        {
            if (has_delete)
            {
                DataTable dtClone = this.table.Clone();
                foreach (DataRow row in table.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    DataRow row_clone = dtClone.NewRow();
                    for (int i = 0; i < table.Columns.Count; i++)
                    {
                        row_clone[i] = row[i];
                    }
                    dtClone.Rows.Add(row_clone);
                }

                return dtClone;
            }
            return table;
        }

        public DataTable getSettingsTable(String setting_id)
        {
            if (isUpdatable && (curr_setting_id.Equals(setting_id) & (base.dataset.HasChanges() | base.isUpdating)))
            {
                return clone_settings();
            }
            defineSettingsTable(setting_id);
            if (isUpdatable)
            {
                if (table.Rows.Count == 0)
                {
                    String[] names = WebConfigurationManager.AppSettings.AllKeys;
                    NameValueCollection appset = WebConfigurationManager.AppSettings;
                    for (int i = 0; i < names.Length; i++)
                    {
                        if (names[i].StartsWith("MQ_"))
                        {
                            DataRow urow = table.NewRow();
                            urow["SettingsID"] = setting_id;
                            urow["SettingName"] = names[i];
                            urow["SettingValue"] = appset[names[i]];
                            urow["SettingDisabled"] = false;
                            table.Rows.Add(urow);
                        }
                    }
                }

                return table;

            }

            return null;
        }

        public DataTable getDdlData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("fldCode", typeof(System.String)));
            dt.Columns.Add(new DataColumn("fldDesc", typeof(System.String)));

            DataRow nrow = dt.NewRow();
            nrow["fldCode"] = "Default";
            nrow["fldDesc"] = "Default";
            dt.Rows.Add(nrow);

            DataSet ds = getDataSet(getSqlCommand("SELECT DISTINCT SettingsID FROM " + TableName, false));
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    nrow = dt.NewRow();
                    nrow["fldCode"] = row["SettingsID"];
                    nrow["fldDesc"] = row["SettingsID"];
                    dt.Rows.Add(nrow);
                }
            }

            return dt;
        }

        public bool applyUpdates(String crud, int row, Hashtable ht)
        {
            if (crud.Equals("DELETE"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                bool del = base.Delete(sqlHelperWs.RETAIN_DATASET, false);
                if (del)
                {
                    has_delete = true;
                }
                return del;
            }
            bool ok = false;
            if (crud.Equals("EDIT"))
            {
                selectedRow = findRow("SEQ_ID", Convert.ToInt32(ht["SEQ_ID"]));
                ok = base.edit();
            }
            if (crud.Equals("ADD"))
            {
                ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
            }
            if (ok)
            {
                foreach (Object key in ht.Keys)
                {
                    if (key.ToString().Equals(this.IdentityColumn)) continue;
                    if (!setColumn(key.ToString(), ht[key]))
                    {
                        ok = false;
                        break;
                    }
                }
                if (ok & addMode)
                {
                    table.Rows.Add(uprow);
                    addMode = false;
                }
            }
            return ok;
        }

        public DataTable saveUpdates()
        {
            if (base.save(sqlHelperWs.DISPOSE_DATASET))
            {
                has_delete = false;

                String s = this.curr_setting_id;
                this.curr_setting_id = String.Empty;
                return this.getSettingsTable(s);
            }

            return null;
        }


        public irsFCRSettings()
        {
            base.TableName = "TBL_FCR_SETTINGS";
            base.IdentityColumn = "SEQ_ID";
            curr_setting_id = String.Empty;
        }
    }
}
