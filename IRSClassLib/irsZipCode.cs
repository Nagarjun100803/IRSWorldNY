using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    [Serializable]
    public class irsZipCode : sqlHelperWs
    {
        #region PRIVATE VARIABLE

        private DataSet data_set = null;
        private string error_message = null;
        private string _municipality = null;
        private string _province = null;
        private string _region = null;
        private string _truezipcode = null;
        private string _seq_id = null;
        private string _osa = null;

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

        public string municipality
        {
            get { return this._municipality; }
            set { _municipality = value; }
        }


        public string province
        {
            get { return this._province; }
            set { _province = value; }
        }

        public string region
        {
            get { return this._region; }
            set { _region = value; }
        }

        public string truezipcode
        {
            get { return this._truezipcode; }
            set { _truezipcode = value; }
        }

        public string osa
        {
            get { return this._osa; }
            set { _osa = value; }
        }

        #endregion

        public DataTable getZipCodes()
        {
            base.getUpdateDataSet("sp_zipcode");
            if (isUpdatable)
            {
                return base.table;
            }
            return null;
        }

        //private int findRow(Object seqid)
        //{
        //    int sid = -1;
        //    try
        //    {
        //        sid = int.Parse((String)seqid);
        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            int x = Convert.ToInt32(table.Rows[i]["SEQ_ID"]);
        //            if (sid == x) return i;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        base.Message = e.Message;
        //    }
           
        //    return -1;
        //}

        //public bool applyUpdates(String crud, int row, Hashtable ht)
        //{
        //    int seq_id = ht["seq_id"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["seq_id"]);
        //    bool ok = false;
        //    if (crud.Equals("DELETE"))
        //    {
        //        int frow = findRow("seq_id", Convert.ToInt32(ht["SEQ_ID"]));
        //        if (frow > -1)
        //        {
        //            selectedRow = frow;
        //            return base.Delete(sqlHelperWs.RETAIN_DATASET);
        //        }
        //        Message = "Unable to find SEQ_ID: " + ht["SEQ_ID"].ToString();
        //        return false;
        //    }
           
        //    else if (crud.Equals("EDIT"))
        //    {

        //        int frow = findRow("seq_id", Convert.ToInt32(ht["SEQ_ID"]));
        //        if (frow > -1)
        //        {
        //            selectedRow = frow;
        //            ok = base.edit();
        //        }
        //        else
        //        {
        //            Message = "Unable to find SEQ_ID: " + ht["SEQ_ID"].ToString();
        //            return false;
        //        }
        //    }
        //    else if (crud.Equals("ADD"))
        //    {
        //        ok = base.newRow(sqlHelperWs.RETAIN_DATASET);
        //    }
        //    if (ok)
        //    {
        //        foreach (Object key in ht.Keys)
        //        {
        //            if (key.ToString().Equals(this.IdentityColumn)) continue;
        //            if (!setColumn(key.ToString(), ht[key]))
        //            {
        //                ok = false;
        //                break;
        //            }
        //        }
        //        if (ok & addMode)
        //        {
        //            table.Rows.Add(uprow);
        //            addMode = false;
        //        }
        //    }
        //    return ok;
        //}

        public bool applyUpdates(Hashtable ht)
        {
            try
            {
                string Region = (string)ht["Region"];
                string Province = (string)ht["Province"];
                string Municipality = (string)ht["Municipality"];
                string Zip = (string)ht["Zip"];
                string osa = (string)ht["osa"];
                int seq_id = ht["SEQ_ID"].Equals(string.Empty) ? 0 : Convert.ToInt32(ht["SEQ_ID"]);

                if (seq_id.ToString().Length > 0)
                {
                    //base.getUpdateDS("sp_zipcode", new object[] { seq_id });
                    string sqlstmt = "SELECT * FROM TBL_ZIPCODES WHERE SEQ_ID= " + seq_id + "";
                    base.getUpdateDS(sqlstmt);

                    if (isUpdatable)
                    {
                        foreach (Object key in ht.Keys)
                        {
                            if (key.ToString().Equals("SEQ_ID"))
                            {
                                if (table.Rows.Count > 0)
                                {
                                    selectedRow = getselectedRow(seq_id);
                                    base.setColumn("Region", Region);
                                    base.setColumn("Province", Province);
                                    base.setColumn("Municipality", Municipality);
                                    base.setColumn("Zip", Zip);
                                    base.setColumn("osa", osa);
                                }
                                else if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                                {
                                    uprow["Region"] = Region;
                                    uprow["Province"] = Province;
                                    uprow["Municipality"] = Municipality;
                                    uprow["Zip"] = Zip;
                                    uprow["osa"] = osa;
                                    table.Rows.Add(uprow);
                                }
                            }
                        }
                        base.addMode = false;
                        return base.save(sqlHelperWs.RETAIN_DATASET);

                    }
                }
                else
                {
                    Message = " missing.";
                }
            }
            catch (Exception e)
            {
                Message = e.Message;
            }

            return false;
        }

        public bool saveUpdates()
        {
            return base.save(sqlHelperWs.RETAIN_DATASET);
        }

        private int getselectedRow(int seq_id)
        {
            DataView dv = table.DefaultView;
            dv.Sort = "SEQ_ID";
            int i = dv.Find(new Object[] { seq_id });
            if (i > -1)
            {
                //dv[i]["allow_access"] = allow;
                selectedRow = i;
            }
            return i;
        }


        public irsZipCode()
        {
            this.TableName = "TBL_ZIPCODES";
            this.IdentityColumn = "SEQ_ID";
        }

        //public DataTable saveUpdates()
        //{
        //    if (dataset.HasChanges())
        //    {
        //        base.save(sqlHelperWs.RETAIN_DATASET);
        //    }
        //    return getZipCodes();

           
        //}

    }
}