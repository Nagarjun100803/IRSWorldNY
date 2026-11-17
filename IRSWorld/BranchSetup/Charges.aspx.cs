using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using IRSClassLib;

namespace IRSWorld.BranchSetup
{
    public partial class Charges : System.Web.UI.Page
    {

        private irsCharges IRSCharges
        {
            get
            {
                if (Session["IRS_CHARGES"] == null)
                {
                    Session["IRS_CHARGES"] = new irsCharges();
                }
                return (irsCharges)Session["IRS_CHARGES"];
            }
        }

        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }

                return null;
            }
        }

        private bool isAuthorized(String branch)
        {
            bool isSysad = IRSClientData.isSystemAdmin;

            if (isSysad) return true;

            bool isBrad = IRSClientData.isBranchAdmin;
            if (isBrad)
            {
                if (branch == null || branch == String.Empty) return true;
                return (IRSClientData.Location.Equals(branch));
            }

            return false;
        }

        private String[] create_options(DataRowCollection rows, String val_fld, String data_fld)
        {
            ArrayList al = new ArrayList();
            foreach (DataRow row in rows)
            {
                al.Add(row[val_fld].ToString().Trim() + "~" + row[data_fld].ToString().Trim());
            }
            String[] ss = new String[al.Count];
            al.CopyTo(ss);
            return ss;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Charges));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_charges()
        {
            Session["IRS_CHARGES"] = null;
        }

        private short getChargeGroup(DataTable dtbr)
        {
            short chgroup = 0;
            DataRow[] rows = dtbr.Select("BranchCode = '" + IRSClientData.Location + "'");
            if (rows.Length > 0 && rows[0]["ChargeGroup"] != DBNull.Value)
            {
                chgroup =  Convert.ToInt16(rows[0]["ChargeGroup"]);
            }

            IRSCharges.ChargeGroup = chgroup;

            return chgroup;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public short setChargeGroupByBrcode(String brcode)
        {
            return IRSCharges.setChargeGroup(brcode);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void setChargeGroup(int chgroup)
        {
            try
            {
                IRSCharges.ChargeGroup = (short)chgroup;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getChargeGroupBranches(bool for_save)
        {
            DataTable dt = IRSCharges.getChargeGroupBranches(IRSCharges.ChargeGroup);
            if (dt == null)
            {
                throw new Exception(IRSCharges.Message);
            }

            return new Object[] { for_save, dt };
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getDdlData(String brcode)
        {
            String errmsg = "";

            DataSet ds = IRSCharges.get_ddl_data(brcode, out errmsg);
            if (ds == null) throw new Exception("Charges@GET DDL DATA: " + errmsg);

            String[] curropts = null;
            if (ds.Tables.Count > 0)
            {
                String[] ss = create_options(ds.Tables[0].Rows, "fldValue", "fldCode");
                curropts = new String[ss.Length + 1];
                curropts[0] = "0~ ";
                int j = 1;
                for (int i = 0; i < ss.Length; i++)
                {
                    curropts[j++] = ss[i];
                }
            }
            String[] prodopts = null;
            if (ds.Tables.Count > 4)
            {
                prodopts = create_options(ds.Tables[5].Rows, "fldValue", "fldDescription");
            }
            String[] chargopts = null;
            if (ds.Tables.Count > 5)
            {
                chargopts = create_options(ds.Tables[6].Rows, "fldValue", "fldDescription");
            }
            String[] comptypes = null;
            if (ds.Tables.Count > 6)
            {
                comptypes = create_options(ds.Tables[7].Rows, "fldValue", "fldDescription");
            }

            return new object[] { ds, prodopts, chargopts, comptypes, curropts };
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getSetupData()
        {
            if (IRSClientData.UserId == null || IRSClientData.UserId == String.Empty) throw new Exception("Session Expired");
            DataSet ds = IRSCharges.getLookupBranches();
            if (ds == null)
            {
                throw new Exception("GET BRANCHES: " + IRSCharges.Message);
            }
            return new object[] { IRSClientData.Location, IRSClientData.isSystemAdmin | IRSClientData.isBranchAdmin, ds };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getChargeMasterGroup(int chgroup)
        {
            String errmsg = "";
            short xchgroup = Convert.ToInt16(chgroup);
            if (xchgroup > 0 && (xchgroup == IRSCharges.ChargeGroup | IRSClientData.isSystemAdmin | IRSClientData.isBranchAdmin))
            {
                DataTable dt = IRSCharges.getChargeMaster(xchgroup, out errmsg);
                if (dt == null) throw new Exception(errmsg);

                return dt;
            }

            throw new Exception("Session Expired");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getChargeMaster(String brcode)
        {
            String errmsg = "";
            String xbr = (brcode == null || brcode.Length == 0) ? IRSClientData.Location : brcode;
            DataTable dt = IRSCharges.getChargeMaster(xbr, out errmsg);
            if (dt != null)
            {
                return dt;
            }

            throw new Exception(errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getChargesDetail(String seqid)
        {
            DataTable dt = IRSCharges.getChargesDetail(seqid);
            if (dt == null)
            {
                throw new Exception (IRSCharges.Message);
            }

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable getChargesTier(String seqid)
        {
            DataTable dt = IRSCharges.getChargesTier(seqid);
            if (dt == null)
            {
                throw new Exception(IRSCharges.Message);
            }

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] getChargesUsers(String seqid)
        {
            int iseqid = int.Parse(seqid);
            Object[] rs = IRSCharges.getChargesUsers(iseqid);
            if (rs != null)
            {
                String[] em = (String[])rs[1];
                if (em[0].ToString() != String.Empty) throw new Exception(em[0].ToString());
                if (em[1].ToString() != String.Empty) throw new Exception(em[1].ToString());
            }

            return rs;
        }

        private ArrayList ht_keep;

        private void keep_ht(String crud, Hashtable ht)
        {
            try
            {
                if (ht_keep == null) ht_keep = new ArrayList();
                ht_keep.Add(new Object[] { crud, ht });
            }
            catch (Exception e)
            {
                throw new Exception("ht_keep: " + e.Message);
            }
        }

        private void cascade_edits(String type)
        {
            DataTable dt = IRSCharges.getChargeGroupBranches(IRSCharges.ChargeGroup);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int pgno = ((i + 1) > 12) ? (i + 1) / 12 : 0;
                    int grow = i - ((pgno * 12) - 12);
                    new csControl().execScript(this.Page, "brgrid_row_select", "brgrid.row.selected(" + grow + "," + i + ");");

                    String brcode = dt.Rows[i]["BranchCode"].ToString();
                    irsChargeGroupUpdate icu = new irsChargeGroupUpdate();
                    icu.ChargeGroup = IRSCharges.ChargeGroup;
                    if (!icu.cascade_edits(brcode, type, ht_keep))
                    {
                        new csControl().showMessage(this.Page, "cascade_msg", "Cascade failed : " + brcode + " - " + IRSCharges.Message);
                    }
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object applyEdits(bool go_chgroup, String type, String adds, String dels, String eds)
        {
            if (type.Equals("Product") | type.Equals("Charges") | type.Equals("Tier") | type.Equals("Users") | type.Equals("Over"))
            {
                char sep = '^';
                ArrayList errs = new ArrayList();
                ht_keep = null;
                String[] ss = { adds, dels, eds };
                foreach (String s in ss)
                {
                    if (s.Length > 0)
                    {
                        String[] tt = s.Split('~');
                        String crud = tt[0].ToUpper();
                        for (int i = 1; i < tt.Length; i++)
                        {
                            String[] uu = tt[i].Split('|');
                            int row = int.Parse(uu[0]);
                            Hashtable ht = new Hashtable();
                            for (int j = 1; j < uu.Length; j++)
                            {
                                String[] cols = uu[j].Split(sep);
                                if (cols.Length < 2) continue;
                                ht.Add(cols[0], cols[1]);
                            }
                            bool ok = IRSCharges.applyUpdates(type, crud, ht);
                            if (!ok) errs.Add(crud + " Row: " + row + " - " + IRSCharges.Message);
                            else if (go_chgroup)
                            {
                                keep_ht(crud, ht);
                            }
                        }
                    }
                }
                if (errs.Count > 0)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (Object msg in errs)
                    {
                        sb.Append(msg.ToString() + "\n");
                    }
                    throw new Exception(sb.ToString());
                }

                String errmsg = "";
                DataTable dt = IRSCharges.saveUpdates(type, out errmsg);
                if (dt == null)
                {
                    throw new Exception("Save changes: " + errmsg);
                }
                else if (go_chgroup)
                {
                    cascade_edits(type);
                }

                return dt;
            }
            else
            {
                throw new Exception("Unknown type: " + type);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int getAddedProductId()
        {
            Object o = IRSCharges.IdentityCurrent;
            if (o != null) return Convert.ToInt32(o);
            return -1;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rowExist(String rec)
        {
            String[] ss = rec.Split('~');
            int seqid = 0;
            if (ss.Length > 0)
            {
                Hashtable ht = new Hashtable();
                foreach (String s in ss)
                {
                    String[] tt = s.Split('=');
                    if (tt[0] == null || tt[0] == String.Empty) continue;
                    if (tt[0].Equals("SEQ_ID"))
                    {
                        try
                        {
                            seqid = Convert.ToInt32(tt[1]);
                        }
                        catch { }
                        continue;
                    }
                    Object dat = (tt.Length < 2) ? null : tt[1];
                    ht.Add(tt[0], dat);
                }
                return IRSCharges.rowExist(ht, seqid);
            }

            throw new Exception("Unable to verify if row exist: " + rec);
        }

        [AjaxPro.AjaxMethod()]
        public void keepAlive()
        {
            // just to keep the script in touch with code-behind thus keep the session alive.
        }
    }

}
