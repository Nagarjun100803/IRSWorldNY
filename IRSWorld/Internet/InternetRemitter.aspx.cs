using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using IRSClassLib;
using IRSWorld.TranClass;
using WordsMatching;
using WordsMatchingFL;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace IRSWorld.Internet
{
    public partial class InternetRemitter : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        private String coms_errmsg = "Unable to access Sql Commons Service.";

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetRemitter));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranGFMCCommons));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Refinitiv)); /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */

            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
        }

        private irsClientData IRSClientData
        {
            get
            {
                if (Session["IRS_CLIENT_DATA"] != null)
                {
                    return ((irsClientData)Session["IRS_CLIENT_DATA"]);
                }
                else
                {
                    call_errpage();
                    return null;
                }
            }
        }

        public void call_errpage()
        {
            csControl cs_con = new csControl();
            cs_con.execScript(this.Page, "int_rem", "spawn_errpage('Session Expired')");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string AgeLimit()
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            DataSet ds = common.getDataSet(common.getSqlCommand("select isnull(BranchDefValue,'') from tbl_branch_defaults where branchdefvalue like 'ageLimit%' and branchcode = '" + IRSClientData.Location + "'", false));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    String BranchDefValue = ds.Tables[0].Rows[0][0].ToString();
                    int AgeLimit = 0;
                    try
                    {
                        String[] BranchDef = BranchDefValue.Split('_');
                        if (BranchDef.Length > 1)
                        {
                            if (int.TryParse(BranchDef[1], out AgeLimit))
                            {
                                return AgeLimit.ToString();
                            }
                        }
                    }
                    catch { }
                }
            }
            return "18";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool hasOtherID()
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            DataSet ds = common.getDataSet("select isnull(branchdefcode,'') from tbl_branch_defaults where BranchDefValue = 'RemitterInputtedId' and branchcode = '" + IRSClientData.Location + "'", false, new Object[] { });
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    String BranchDefCode = ds.Tables[0].Rows[0][0].ToString();
                    try
                    {
                        if (BranchDefCode == "1")
                        {
                            return true;
                        }
                    }
                    catch { }
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool isPDDTSBank(String BankCode)
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            DataSet ds = common.getDataSet(common.getSqlCommand("select 1 from tbl_Lookup where FldGroup='PDDTSBank' AND fldcode='" + BankCode + "'", false));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] get_id_details(String idnos, String idtypes, String CIFID)
        {
            String[] IDNo = idnos.Split('~');
            String[] tt = idtypes.Split('~');
            int[] IDType = { -1, -1 };
            Object[] ods = new Object[] { null, null };
            for (int i = 0; i < tt.Length; i++)
            {
                try
                {
                    IDType[i] = int.Parse(tt[i].Trim());
                }
                catch { }
            }

            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                for (int i = 0; i < IDNo.Length; i++)
                {
                    if (IDNo[i] != null && IDNo[i].Trim().Length > 0)
                    {
                        DataSet ds = commons.getDataSet("sp_check_id_exists", true, new Object[] { IDNo[i], IDType[i], CIFID }, new String[] { "CIF_ID" });
                        ods[i] = ds;
                    }
                }

                return ods;
            }
            throw new Exception(coms_errmsg);
        }

        #region AJAX PUBLIC METHOD
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void close_session()
        {
            Session.Abandon();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void drop_session()
        {
            Session["MODULE_NAME"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DateTime get_branch_localtime(string brcode)
        {
            DateTime dt = new irsTimeZone().branchLocalTime(brcode);
            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_userid()
        {
            return IRSClientData.UserId;
        }

        private String get_branch_country(String brcode)
        {
            try
            {
                Object o = new irsSqlCommonsWs().execScalar("select Country from tbl_branches where branchcode = '" + brcode + "'");
                short sh = Convert.ToInt16(o);
                return sh.ToString();
            }
            catch { }
            return String.Empty;
        }

        private bool get_occupation_others_setting(String brcode)
        {
            try
            {
                Object o = new irsSqlCommonsWs().execScalar("select BranchDefCode from tbl_branch_defaults where branchcode = '" + brcode + "' and BranchDefValue = 'OccupationOthersMandatory'");
                if (o != null && o.GetType() == typeof(System.Int16))
                {
                    return (Convert.ToInt16(o) == 1);
                }
            }
            catch { }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] get_client_data()
        {
            if (Session["IRS_CLIENT_DATA"] == null || String.IsNullOrEmpty(IRSClientData.UserId))
            {
                throw new Exception("Session Expired.");
            }
            bool is_agent = check_userrole_is_agent((int)IRSClientData.Role);

            return new Object[] { IRSClientData.CIFID, get_userid(), IRSClientData.Role, IRSClientData.Location, is_agent, get_branch_country(IRSClientData.Location), get_occupation_others_setting(IRSClientData.Location) };
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getLocation()
        {
            return ((IRSClassLib.irsClientData)Session["IRS_CLIENT_DATA"]).Location;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string getConfigBranchCode()
        {
            string IRSW_BRANCHCODE = null;
            try
            {
                IRSW_BRANCHCODE = new irsConfiguration().appsetting("IRSW_BRANCHCODE");
                return IRSW_BRANCHCODE;
            }
            catch
            {
                return IRSW_BRANCHCODE;
            }
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string enroll_agent(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.INSERT, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }
        // CJ For Pending OVR
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool chkpendingovr(string cifid)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            string validationres = "";
            DataSet ds = null;
            ds = sqlkonek.getDataSet("chk_PENDING_OVR_ADT_TRL", true, new object[] { cifid });
            if (ds.Tables.Count > 0)
            {
                validationres = ds.Tables[0].Rows[0][0].ToString();
                if (validationres != "0")
                {
                    return true;
                }
            }
            return false;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool updatependingovr(string cifid)
        {
            sqlHelperWs sqlkonek = new sqlHelperWs();
            int xx = 0;
            xx = sqlkonek.execStoredProcedureUpdate("reset_PENDING_OVR_ADT_TRL", new object[] { cifid });
            if (xx < 1)
            {
                return false;
            }
            return true;
        }
        //CJ END
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_agent(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_cclass(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_CIFCLASS, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_bene_address(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_ADDRESS, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_sssno(string sssno, string validsss)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            object x = commons.execScalar("sp_cif_validate_sssnumber", new string[] { sssno, validsss });

            return x.ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_id_type(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_id_type", true, new Object[] { seq_id }, new String[] { "CIF_ID_TYPE" });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }

                throw new Exception(commons.Message);
            }

            throw new Exception(coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_van_is_exists(String van, int van_type, String beneseq_id, String CIFID)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_check_van_is_exists", true, new Object[] { van, van_type, beneseq_id, CIFID }, new String[] { "CIF_ID_TYPE" });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return (ds.Tables[0].Rows.Count > 0);
                }

                throw new Exception(commons.Message);
            }

            throw new Exception(coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_bene(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_BENE, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_account(string plist)
        {
            string errmsg = null;
            string acc_seqid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            acc_seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_ACCOUNT, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return acc_seqid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_ids(string plist)
        {
            string errmsg = null;
            string seqid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_IDS, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return seqid;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_van(string plist)
        {
            string errmsg = null;
            string seqid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_VAN, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return seqid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_notes(string plist)
        {
            string errmsg = null;
            string seqid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_NOTES, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return seqid;
        }


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_card(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_CARD, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_ids(string plist)
        {
            string errmsg = null;
            string seqid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE_IDS, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return seqid;
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_van(string plist)
        {
            string errmsg = null;
            string seqid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE_VAN, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return seqid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String enrollgfc(String IDNo, String CIFID)
        {
            irsLinkToGFC gfc = new irsLinkToGFC();
            bool isvalid = false;

            if (!isRemitter(CIFID))
            {
                throw new Exception("Client is not a remitter.");
            }

            int res = gfc.CheckGFCNumber(IDNo);
            switch (res)
            {
                case 1: isvalid = true; break;
                case 2: throw new Exception("GFC Number already exist! Please check with eGFMC system!");
                case 0: throw new Exception("Card number is invalid");
                case -1: throw new Exception(gfc.message);
                default: throw new Exception("Card number is invalid");
            }

            if (isvalid)
            {
                DataSet ds = gfc.getGFCCardDetails(IDNo);
                if (ds == null) { throw new Exception("Card number is invalid"); }
                String Curr = ds.Tables[0].Rows[0]["rec_CurrCode"].ToString();
                if (Curr == null | Curr == "") { throw new Exception("Failed to get GFC Card Currency"); }
                int CurrVal = gfc.getCurrValueName(Curr);

                /* FOR CBIP */
                String[] ck = null; //gfc.GetCKAccount(CurrVal.ToString()).Split('~');
                String ckacct = ck[0];
                String acctType = (ck[1] == "") ? "1" : ck[1];

                if (ckacct == null | ckacct == "") { throw new Exception("Available CK has runout.\r\nPlease call ORD at local 4617/4566)"); }
                if (gfc.SaveGFMCAccount(CIFID + "~" + CIFID, IDNo.Trim(), ckacct, CurrVal, "N/A", "purchase", acctType))
                {
                    return ckacct;
                }
                else
                {
                    throw new Exception(gfc.message);
                }
            }
            else
            {
                throw new Exception("Card number is invalid");
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_bene(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE_BENE, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_account(string plist)
        {
            string errmsg = null;
            string acc_seqid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            acc_seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE_ACCT, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return acc_seqid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string update_card(string plist)
        {
            string errmsg = null;
            string cifid = null;
            irsClientEnrollment enr_agt = new irsClientEnrollment();

            cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE_CARD, plist, out errmsg);

            if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
            else return cifid;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool is_existing_accountno(string accountno)
        {
            irsClientAccounts ca = new irsClientAccounts();
            return ca.is_existing(accountno);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataTable get_other_bank_type()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataTable dt = null;
            object x = commons.execScalar("sp_cif_dropdown_other_bank_type");
            if (x != null && x != DBNull.Value && x.ToString().Length > 0)
            {
                String[] ss = x.ToString().Split('|');
                String in_args = "(";
                for (int i = 0; i < ss.Length; i++)
                {
                    in_args += ss[i].Trim();
                    if ((i + 1) < ss.Length) in_args += ",";
                }
                in_args += ")";
                String sql = "SELECT FldValue, FldDescription FROM TBL_Lookup WHERE FldGroup = 'BranchType' AND FldValue in " + in_args;
                DataSet ds = commons.getDataSet(sql, false, new Object[] { });
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
            }

            return dt;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_first_rec(string cifid, string remcifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            return commons.execScalar("sp_internet_count_benerec", new string[] { cifid, remcifid }).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_fld_desc(string fldval, string fldcode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            return commons.execScalar("sp_internet_get_flddesc", new string[] { fldval, fldcode }).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_master_branch(string cifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            return commons.execScalar("sp_internet_get_branch_code", new string[] { cifid }).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_acctseqid(string seqid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            return commons.execScalar("sp_internet_get_acctseqid", new string[] { seqid }).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string clean_rejected(string cifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            return commons.execScalar("sp_internet_clean_rejected", new string[] { cifid }).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool validate_acctno_fcalgo(string acctno)
        {
            irsAccountAlgo acct_algo = new irsAccountAlgo();
            bool isValidAcct = acct_algo.checkAccount(acctno);

            return isValidAcct;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_acct_details(string acctno)
        {
            irsClientAccounts ca = new irsClientAccounts();
            return ca.get_acctdetails(acctno);
        }

        /* RFS BLOCK TIME DEPOSIT */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAccountTD(string AccountNo, bool CIF)
        {
            if (new irsAccountInquiry().isAccountTD(AccountNo))
            {
                if (CIF)
                {
                    return "Enrollment of Time Deposit account is not allowed.";
                }
                else
                {
                    return "Time Deposit Account. Please process thru TD Placement.";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_algo_type(string acctno)
        {
            irsAccountAlgo acct_algo = new irsAccountAlgo();
            if (acct_algo.checkAccount(acctno))
            {
                return acct_algo.AlgoType;
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string add_other_banks(string[] param_vals)
        {
            irsClientOtherBanks ob = new irsClientOtherBanks();

            if (ob.insert_record() == true)
            {
                if (ob.set_record(param_vals) == true)
                {
                    if (ob.commit_record() == true) return ob.seq_id;
                    return "";
                } return "";
            } return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_record()
        {
            return this.execute_crud("INSERT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record(string seq_id)
        {
            if (seq_id != null)
            {
                if (seq_id.ToUpper() == "NULL" || seq_id == "0")
                {
                    seq_id = null;
                }
            }
            Session["INTERNET_REMITTER_SEQ_ID"] = seq_id;
            return this.execute_crud("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool add_bene_as_rem(string[] param_values)
        {
            irsClientClassifications ca = new irsClientClassifications();

            if (ca.insert_record() == true)
            {
                if (ca.set_record(param_values) == true)
                {
                    if (ca.commit_record() == true) return true;
                    return false;
                } return false;
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool commit_record(string param_values)
        {
            this.parameter_values = param_values;
            return this.execute_crud("COMMIT");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool update_record_bene(string param_values)
        {
            this.parameter_values = param_values;
            return this.execute_crud("UPDATE");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool rollback_record()
        {
            return this.execute_crud("ROLLBACK");
        }

        private bool isRemitter(String CIFID)
        {
            try
            {
                irsSqlCommonsWs commons = new irsSqlCommonsWs();
                String sql = "select count(1) from TBL_CIF_CLASS where CIFClass = 1 and isnull(Deleted,0) = 0 and CIFID = '" + CIFID + "'";
                if (int.Parse(commons.execScalar(sql).ToString()) > 0) { return true; }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to check client if remitter, Reason : " + ex.Message);
            }
            return false;
        }

        private bool execute_crud(string instruction)
        {
            string module_name = string.Empty;

            try
            {
                this.set_message(null);

                if (Session["MODULE_NAME"] != null)
                {
                    module_name = (string)Session["MODULE_NAME"];
                }

                if (module_name != string.Empty)
                {
                    if (module_name == "INTERNET_REM_ADD_BENE") { return this.do_internet_remitter_addbene(instruction); }
                    if (module_name == "INTERNET_REM_ADD_REM") { return this.do_internet_remitter_addbene(instruction); }
                    if (module_name == "INTERNET_REM_ADD_CARDS") { return this.do_internet_remitter_addcards(instruction); }
                    if (module_name == "INTERNET_REM_ADD_PROD") { return this.do_internet_remitter_addprod(instruction); }
                    if (module_name == "INTERNET_REM_SEARCH") { return true; }
                    if (module_name == "INTERNET_REM_ADD_ACCT") { return this.do_internet_remitter_addacct(instruction); }
                }

                this.set_message("SESSION EXPIRED. UNABLE TO PERFORM OPERATION: " + instruction);
                call_errpage(); return false;
            }
            catch (Exception e)
            {
                this.set_message("UNABLE TO PERFORM OPERATION: " + instruction + "\n\n" + e.Message); return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String set_module_name(string customers_module)
        {
            Session["MODULE_NAME"] = customers_module;
            return get_userid();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["INTERNETREMITTER_MESSAGE"] != null)
            {
                return (string)Session["INTERNETREMITTER_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["INTERNETREMITTER_MESSAGE"] = null;
            Session["INTERNETREMITTER_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_INTERNETREMITTER_BENE_RECORD(string column_values)
        {
            irsInternetRem internetremitter = this.INTERNET_REMITTER();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (internetremitter.set_record(s) == false)
                {
                    this.set_message(internetremitter.messages); return false;
                }

                Session["IRS_INTERNET_REMITTER"] = internetremitter;

                return true;
            }

            internetremitter = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_INTERNETREMITTER_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_INTERNET_REMITTER_BENE_RECORD(string client_seq_id)
        {
            irsInternetRem internetremitter = this.INTERNET_REMITTER();

            try
            {
                if (internetremitter != null)
                {
                    if (internetremitter.get_record(client_seq_id) == true)
                    {
                        return internetremitter.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_INTERNET_REMITTER"] = internetremitter;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_INTERNET_REMITTER_REM_RECORD(string client_seq_id)
        {
            irsInternetRem internetremitter = this.INTERNET_REMITTER();
            try
            {
                if (internetremitter != null)
                {
                    if (internetremitter.get_record(client_seq_id) == true)
                    {
                        return internetremitter.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_INTERNET_REMITTER"] = internetremitter;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_INTERNETREMITTER_PROD_RECORD(string column_values)
        {
            irsInternetRemProducts prod = this.INTERNET_REMITTER_ADD_PROD();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (prod.set_record(s) == false)
                {
                    this.set_message(prod.messages); return false;
                }

                Session["IRS_INTERNET_REMITTER_ADD_PROD"] = prod;

                return true;
            }

            prod = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_INTERNETREMITTER_PROD_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_INTERNET_REMITTER_PROD_RECORD(string client_seq_id)
        {
            irsInternetRemProducts prod = this.INTERNET_REMITTER_ADD_PROD();
            try
            {
                if (prod != null)
                {
                    if (prod.get_record(client_seq_id) == true)
                    {
                        return prod.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["INTERNET_REMITTER_ADD_PROD"] = prod;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_INTERNETREMITTER_CARDS_RECORD(string column_values)
        {
            irsInternetRemCards internetremitter_cards = this.INTERNET_REMITTER_CARDS();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (internetremitter_cards.set_record(s) == false)
                {
                    this.set_message(internetremitter_cards.messages); return false;
                }

                Session["IRS_INTERNET_REMITTER_CARDS"] = internetremitter_cards;

                return true;
            }

            internetremitter_cards = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_INTERNETREMITTER_CARDS_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_INTERNETREMITTER_ACCT_RECORD(string column_values)
        {
            irsInternetRemAcct internetremitter_acct = this.INTERNET_REMITTER_ACCT();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (internetremitter_acct.set_record(s) == false)
                {
                    this.set_message(internetremitter_acct.messages); return false;
                }

                Session["IRS_INTERNET_REMITTER_ACCT"] = internetremitter_acct;

                return true;
            }

            internetremitter_acct = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_INTERNETREMITTER_ACCT_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_INTERNET_REMITTER_CARDS_RECORD(string client_seq_id)
        {
            irsInternetRemCards internetremitter_cards = this.INTERNET_REMITTER_CARDS();
            try
            {
                if (internetremitter_cards != null)
                {
                    if (internetremitter_cards.get_record(client_seq_id) == true)
                    {
                        return internetremitter_cards.getdataRecord();
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                Session["IRS_INTERNET_REMITTER_CARDS"] = internetremitter_cards;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string INTERNET_REMITTER_SEQ_ID()
        {
            if (Session["INTERNET_REMITTER_SEQ_ID"] != null)
            {
                return (string)Session["INTERNET_REMITTER_SEQ_ID"];
            }
            return "0";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string GET_CIF_ACCOUNTSEQID(string accountno)
        {
            irsSqlCommonsWs helper = new irsSqlCommonsWs();
            DataSet ds = helper.getDataSet("sp_cif_get_acctseqid", true, new Object[] { accountno });
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    return Convert.ToString(dt.Rows[0]["SEQ_ID"]);
                }

                return "0";
            }

            throw new Exception("@GET_CIF_ACCOUNTSEQID: " + helper.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_web_remittance_bene_non_editable_fields()
        {
            irsSqlCommonsWs helper = new irsSqlCommonsWs();

            /* Replaced by a line below for perf. reason.
            SqlCommand command = helper.getSqlCommand("sp_get_web_rem_bene_non_editable_fields");
            command.Parameters.Add("@v_branchcode", SqlDbType.VarChar, 4).Value = IRSClientData.Location;
            DataSet ds = helper.getDataSet(command, "TBL_SUB_LOOKUP");
            */

            DataSet ds = helper.getDataSet("sp_get_web_rem_bene_non_editable_fields", true, new Object[] { IRSClientData.Location });

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds;
            }

            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_pnbbranchcode()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet ds = commons.getDataSet("sp_internet_dropdown_pnbbranchcode", true, new Object[] { });
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds;
            }

            throw new Exception("@getdropdown_source_pnbbranchcode: " + commons.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_filteredbranches(int prod_type)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_dropdown_filtered_branches", true, new Object[] { prod_type });
                if (ds != null && ds.Tables.Count > 0) return ds;
                throw new Exception("@getdropdown_source_filteredbranches: " + commons.Message);
            }

            throw new Exception("@getdropdown_source_filteredbranches: " + coms_errmsg);
        }

        //Instapay Alyssa 2019
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_filteredInstapayBank(int prod_type)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_dropdown_filtered_InstapayBank", true, new Object[] { });
                if (ds != null && ds.Tables.Count > 0) return ds;
                throw new Exception("@getdropdown_source_filteredbranches: " + commons.Message);
            }

            throw new Exception("@getdropdown_source_filteredbranches: " + coms_errmsg);
        }
        //Instapay Alyssa 2019

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_filteredcurrency(int prod_type, int role)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_dropdown_filtered_currency", true, new Object[] { prod_type, role });
                if (ds != null && ds.Tables.Count > 0) return ds;
                throw new Exception("@getdropdown_source_filteredcurrency: " + commons.Message);
            }

            throw new Exception("@getdropdown_source_filteredcurrency: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_all_mandatory_flds(int cif_class, string branch_code)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_all_mandatory_flds", true, new object[] { cif_class, branch_code });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception(commons.Message);
            }
            throw new Exception("@get_all_mandatory_flds: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_int_mandatory_flds(string ciffieldid, int cifclass, int ciftype)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_int_get_all_mandatory_flds_final", true, new object[] { ciffieldid, cifclass, ciftype });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_int_mandatory_flds: " + commons.Message);
            }

            throw new Exception("@get_int_mandatory_flds: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_mandatory_flds_count(string int_field_id, int cif_class, string branch_code)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_int_check_mandatory_flds", true, new Object[] { int_field_id, cif_class, branch_code });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
            }
            return 0;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_mandatory_flds(string empty_flds, int cif_class, string branch_code)
        {
            String[] empty_fld = empty_flds.Split('~');
            String reqd_flds = "";
            bool prefMailAddReq = false;
            String prefMailAddName = "";
            int prefMailUncheckCount = 0;

            for (int i = 0; i < empty_fld.Length; i++)
            {
                /* checks if field is mandatory, when count > 0 then mandatory */
                int count = get_mandatory_flds_count(empty_fld[i], cif_class, branch_code);

                if (empty_fld[i].IndexOf("PrefMailAdd") > 0)
                {
                    prefMailUncheckCount = prefMailUncheckCount + 1;
                }

                if (count > 0)
                {
                    if (reqd_flds.IndexOf(empty_fld[i]) < 0) /* do not append empty_fld[i] when it is already included in reqd_flds */
                    {
                        if (empty_fld[i].IndexOf("PrefMailAdd") > 0)
                        {
                            prefMailAddReq = true;
                            prefMailAddName = empty_fld[i];
                        }
                        else
                        {
                            reqd_flds += empty_fld[i] + '~';
                        }
                    }
                }
            }

            if (prefMailAddReq && prefMailUncheckCount == 3)
            {
                reqd_flds += prefMailAddName + '~';
            }

            String[] reqd_fld = reqd_flds.Split('~'); /* mandatory fields per table (note: table = active table)*/

            /* gets the label of the field given ciffieldid - ex: txtAccountNo -> Account No */

            String fld_lbl = "";
            String fld_lbl_tmp = "";

            for (int i = 0; i < reqd_fld.Length; i++)
            {
                DataSet ds = get_mandatory_flds_label(reqd_fld[i]);



                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    fld_lbl = ds.Tables[0].Rows[0]["CIFLabel"].ToString();
                }
                if (fld_lbl != "")
                {
                    if (fld_lbl_tmp.IndexOf(fld_lbl) < 0)
                    {
                        fld_lbl_tmp += (fld_lbl + '~');
                    }
                }
            }
            return fld_lbl_tmp;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_mandatory_flds_label(string int_field_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_int_get_mandatory_flds", true, new Object[] { int_field_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_mandatory_flds_label: " + commons.Message);
            }
            throw new Exception("@get_mandatory_flds_label: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bank(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_get_bank", true, new Object[] { seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_bank: " + commons.Message);
            }

            throw new Exception("@get_bank: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bankname(string branchcode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_get_bankname", true, new Object[] { branchcode });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_bankname: " + commons.Message);
            }
            throw new Exception("@get_bankname: " + coms_errmsg);
        }

        //INSTAPAY ALYSSA SEPTEMBER 2019
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bankname_instapay(string branchcode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_get_bankname_instapay", true, new Object[] { branchcode });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_bankname: " + commons.Message);
            }

            throw new Exception("@get_bankname: " + coms_errmsg);
        }
        //INSTAPAY ALYSSA SEPTEMBER 2019

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bene_details(string seqid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_bene_details", true, new Object[] { seqid });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_bene_details: " + commons.Message);
            }

            throw new Exception("@get_bene_details: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_cif_cards_count(string cifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_cards_count", true, new Object[] { cifid });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
            }
            return 0;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bene_name(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_bene_name", true, new Object[] { seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_bene_name: " + commons.Message);
            }
            throw new Exception("@get_bene_name: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_otherbanks_details(string seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_otherbanks_details", true, new Object[] { seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_otherbanks_details: " + commons.Message);
            }
            throw new Exception("@get_otherbanks_details: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_zipcode(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_zipcode", true, new Object[] { seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_zipcode: " + commons.Message);
            }
            throw new Exception("@get_zipcode: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_Charge_type(int fldval)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_charge_type", true, new Object[] { fldval });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_Charge_type: " + commons.Message);
            }
            throw new Exception("@get_Charge_type: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_van_product(int fldval)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_van_product", true, new Object[] { fldval });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_van_product: " + commons.Message);
            }
            throw new Exception("@get_van_product: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_branchdef_values(string brcode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("spGetBranchDefaultSetup", true, new Object[] { brcode });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_branchdef_values: " + commons.Message);
            }
            throw new Exception("@get_branchdef_values: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_zip(int zipcode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_zip", true, new Object[] { zipcode });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_zip: " + commons.Message);
            }
            throw new Exception("@get_zip: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_cifclass_count(string seq_id, int cif_class)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                Object o = commons.execScalar("sp_cif_get_cifclass_count", new Object[] { seq_id, cif_class });
                if (o != null && o != DBNull.Value)
                {
                    return Convert.ToInt32(o);
                }
            }
            return 0;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cifid_seqid(string gfmcno)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_cifid_seqid", true, new Object[] { gfmcno });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_cifid_seqid: " + commons.Message);
            }
            throw new Exception("@get_cifid_seqid: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cifid_seqid_loyalty(string VIPNo)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("osp_swipeVIPCard", true, new Object[] { VIPNo });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_cifid_seqid_loyalty: " + commons.Message);
            }
            throw new Exception("@get_cifid_seqid_loyalty: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cifid(string seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_cifid", true, new Object[] { seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_cifid: " + commons.Message);
            }
            throw new Exception("@get_cifid: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bene_cifid_seqid(string seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_get_bene_cifid_seqid", true, new Object[] { seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_bene_cifid_seqid: " + commons.Message);
            }
            throw new Exception("@get_bene_cifid_seqid: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_master_seqid(string cifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_master_seqid", true, new Object[] { cifid });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_master_seqid: " + commons.Message);
            }
            throw new Exception("@get_master_seqid: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_id_expirydate(string seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_id_expirydate", true, new Object[] { seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_id_expirydate: " + commons.Message);
            }
            throw new Exception("@get_id_expirydate: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_client_info(string arg, string arg_type)
        {
            bool seq_id = (arg_type.Equals("SEQ_ID"));

            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_get_remdetails", true, new Object[] { arg, seq_id });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_client_info: " + commons.Message);
            }
            throw new Exception("@get_client_info: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_clientbene_info(string seqid, string remcifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_get_bendetails", true, new Object[] { seqid, remcifid });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_clientbene_info: " + commons.Message);
            }
            throw new Exception("@get_clientbene_info: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cards_record(string seqid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_cards", true, new Object[] { seqid });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_cards_record: " + commons.Message);
            }
            throw new Exception("@get_cards_record: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bene_prod(string seqid, string userrole)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_get_bene_prod", true, new Object[] { seqid, userrole });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_bene_prod: " + commons.Message);
            }
            throw new Exception("@get_bene_prod: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String check_and_get_hitlist(String FirstName, String LastName, String MiddleName, String ciftype)
        {
            try
            {
                return get_hitlist(((FirstName == null) ? "" : FirstName), ((LastName == null) ? "" : LastName), ((MiddleName == null) ? "" : MiddleName), IRSClientData.Location, ((ciftype == null) ? "" : ciftype));
            }
            catch { }
            return "";
        }

        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String check_and_get_hitlist_refinitiv(String FirstName, String LastName, String MiddleName, String ciftype, String cifid, String REFINITIV_OverrideChecking, bool isRefinitivIndividual, string Gender, string DOB, string COB, string POB, string Nationality)
        {
            return get_refinitivcifid(((FirstName == null) ? "" : FirstName), ((LastName == null) ? "" : LastName), ((MiddleName == null) ? "" : MiddleName), IRSClientData.Location, REFINITIV_OverrideChecking, ((ciftype == null) ? "" : ciftype), ((cifid == null) ? "" : cifid), isRefinitivIndividual, ((Gender == null) ? "" : Gender), ((DOB == null) ? "" : DOB), ((COB == null) ? "" : COB), ((POB == null) ? "" : POB), ((Nationality == null) ? "" : Nationality));
        }

        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_refinitivcifid(String FirstName, String LastName, String MiddleName, string branchCode, string OverrideChecking, string ciftype, string cifid, bool isRefinitivIndividual, string Gender, string DOB, string COB, string POB, string Nationality)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            RemTranExtra remtranextra = new RemTranExtra();
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            String hitlist = "";
            bool down = false;
            string userid = this.get_userid();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            /* VAPT IAG 20191007 */
            string RequestTime = System.DateTime.Now.ToString();
            /*********************/

            List<string> secondary = new List<string>();
            if (Gender != string.Empty || Gender != null)
            {
                secondary.Add("GENDER=" + Gender.ToUpper());
            }
            if (DOB != string.Empty || DOB != null)
            {
                secondary.Add("DOB=" + DOB.ToUpper());
            }
            if (COB != string.Empty || COB != null)
            {
                secondary.Add("COB=" + COB.ToUpper());
            }
            if (POB != string.Empty || POB != null)
            {
                secondary.Add("POB=" + POB.ToUpper());
            }
            if (Nationality != string.Empty || Nationality != null)
            {
                secondary.Add("NATIONALITY=" + Nationality.ToUpper());
            }

            try
            {
                if (OverrideChecking != "1")
                {
                    refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);

                    if (down == true)
                    {
                        return "Refinitiv is down.";
                    }
                    else
                    {
                        DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //SAVE TO SESSION FOR SAVING TO irsRefinitiv.SaveWatchlist

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                            {
                                //refinitivcs.saveWatchlist_UpdateCIFID(dt.Rows[0][1].ToString(), cifid);
                                refinitivcs.get_refinitiv_dt(dt);
                                //System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                return "";
                            }
                            else
                            {
                                int hitCount = 0;
                                foreach (var item in refinitiv)
                                {
                                    hitCount = item.responseBody.results.Count;
                                }
                                refinitivcs.get_refinitiv_dt(dt);
                                irsrefinitiv.SaveWatchlist(dt, cifid, false, hitCount);

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                    if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                    if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                    if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                    if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                    if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                }
                                return hitlist;
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
                else
                {    //ONBOARDING - CIF CREATION OR CIF EDIT
                    if (Session["OldNameFieldsValue"] != null) // EDIT
                    {
                        bool OverrideCheckingValue = true;
                        refinitivcs.Refinitiv_ArchiveDelete(cifid, true, userid); //ARCHIVEDELETE THE OLD CIFID AND CASESYSTEMID
                        refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down); //CREATE A NEW CASESYSTEMID FOR THE EDITED NAME.
                        DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //DISPLAY THE NEW WATCHLIST OVERRIDE NAME EDITED NAME.

                        if (down == true)
                        {
                            return "Refinitiv is down.";
                        }
                        else
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {

                                //refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down); //CREATE A NEW CASESYSTEMID FOR THE EDITED NAME.
                                //DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //DISPLAY THE NEW WATCHLIST OVERRIDE NAME EDITED NAME.

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                                    {
                                        //refinitivcs.get_refinitiv_dt(dt);
                                        System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                        return "";
                                    }
                                    else
                                    {
                                        int hitCount = 0;
                                        foreach (var item in refinitiv)
                                        {
                                            hitCount = item.responseBody.results.Count;
                                        }
                                        refinitivcs.get_refinitiv_dt(dt);
                                        irsrefinitiv.SaveWatchlist(dt, cifid, OverrideCheckingValue, hitCount); //SAVE THE NEW CASESYSTEMID TO TBL_REFINITIV_HITLIST

                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                            if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                            if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                            if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                            if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                            if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                        }
                                        return hitlist;
                                    }
                                }
                                else
                                {
                                    //refinitivcs.get_refinitiv_dt(dt);
                                    return "";
                                }
                            }
                            else { return ""; }
                        }
                    }
                    else // ADD
                    {
                        bool OverrideCheckingValue = true;
                        refinitivcs.Refinitiv_ArchiveDelete(cifid, true, userid);
                        refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);

                        if (down == true)
                        {
                            return "Refinitiv is down.";
                        }
                        else
                        {
                            DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //NEWLY CREATED CIFID

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                                {
                                    //refinitivcs.get_refinitiv_dt(dt);
                                    //refinitivcs.saveWatchlist_UpdateCIFID(dt.Rows[0][1].ToString(), cifid);
                                    //refinitivcs.saveWatchlist_UpdateByCaseSystemID(dt.Rows[0]["caseSystemId"].ToString(), cifid);
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                    return "";
                                }
                                else
                                {
                                    int hitCount = 0;
                                    foreach (var item in refinitiv)
                                    {
                                        hitCount = item.responseBody.results.Count;
                                    }
                                    refinitivcs.get_refinitiv_dt(dt);
                                    irsrefinitiv.SaveWatchlist(dt, cifid, OverrideCheckingValue, hitCount);

                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                        if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                        if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                        if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                        if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                        if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                    }
                                    return hitlist;
                                }
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                /* VAPT IAG 20191007 */
                new csAuditLog().writeVAPTLogger("", "get_refinitivcifid", ex.Message, "", "", "", csAuditLog.AuditType.Systems, csAuditLog.AlertType.Process);
                /*********************/
                return "";
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }
        /* REFINITIV */

        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String check_and_get_hitlist_refinitiv_merchant(String FirstName, String LastName, String MiddleName, String ciftype, String cifid, String REFINITIV_OverrideChecking, bool isRefinitivIndividual, string Gender, string DOB, string COB, string POB, string Nationality)
        {
            return get_refinitivcifid_merchant(((FirstName == null) ? "" : FirstName), ((LastName == null) ? "" : LastName), ((MiddleName == null) ? "" : MiddleName), IRSClientData.Location, REFINITIV_OverrideChecking, ((ciftype == null) ? "" : ciftype), ((cifid == null) ? "" : cifid), isRefinitivIndividual, ((Gender == null) ? "" : Gender), ((DOB == null) ? "" : DOB), ((COB == null) ? "" : COB), ((POB == null) ? "" : POB), ((Nationality == null) ? "" : Nationality));
        }

        /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_refinitivcifid_merchant(String FirstName, String LastName, String MiddleName, string branchCode, string OverrideChecking, string ciftype, string cifid, bool isRefinitivIndividual, string Gender, string DOB, string COB, string POB, string Nationality)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            RemTranExtra remtranextra = new RemTranExtra();
            Refinitiv refinitivcs = new Refinitiv();
            irsRefinitiv irsrefinitiv = new irsRefinitiv();
            String hitlist = "";
            bool down = false;
            string userid = this.get_userid();
            IEnumerable<irsRefinitiv.ScreeningResponseDto> refinitiv = null;
            string branch_code = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]).Location;
            /* VAPT IAG 20191007 */
            string RequestTime = System.DateTime.Now.ToString();
            /*********************/

            List<string> secondary = new List<string>();
            if (Gender != string.Empty || Gender != null)
            {
                secondary.Add("GENDER=" + Gender.ToUpper());
            }
            if (DOB != string.Empty || DOB != null)
            {
                secondary.Add("DOB=" + DOB.ToUpper());
            }
            if (COB != string.Empty || COB != null)
            {
                secondary.Add("COB=" + COB.ToUpper());
            }
            if (POB != string.Empty || POB != null)
            {
                secondary.Add("POB=" + POB.ToUpper());
            }
            if (Nationality != string.Empty || Nationality != null)
            {
                secondary.Add("NATIONALITY=" + Nationality.ToUpper());
            }

            try
            {
                if (cifid != "")
                {
                    if (OverrideChecking != "1") // ADD MERCHANT
                    {
                        DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down);

                        if (down == true)
                        {
                            return "Refinitiv is down.";
                        }
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                            {
                                System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                return "";
                            }
                            else
                            {
                                int hitCount = 0;
                                hitCount = dt.Rows.Count;
                                refinitivcs.get_refinitiv_dt(dt);
                                irsrefinitiv.SaveWatchlist(dt, cifid, false, hitCount);
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                    if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                    if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                    if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                    if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                    if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                    if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                }
                                return hitlist;
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else //EDIT MERCHANT
                    {
                        refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);

                        if (down == true)
                        {
                            return "Refinitiv is down.";
                        }
                        else
                        {
                            DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //SAVE TO SESSION FOR SAVING TO irsRefinitiv.SaveWatchlist

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                                {
                                    //refinitivcs.saveWatchlist_UpdateCIFID(dt.Rows[0][1].ToString(), cifid);
                                    //refinitivcs.get_refinitiv_dt(dt);
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                    return "";
                                }
                                else
                                {
                                    int hitCount = 0;
                                    foreach (var item in refinitiv)
                                    {
                                        hitCount = item.responseBody.results.Count;
                                    }
                                    refinitivcs.get_refinitiv_dt(dt);
                                    irsrefinitiv.SaveWatchlist(dt, cifid, false, hitCount);

                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                        if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                        if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                        if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                        if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                        if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                    }
                                    return hitlist;
                                }
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                }
                else
                {
                    if (OverrideChecking != "1")
                    {
                        refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);

                        if (down == true)
                        {
                            return "Refinitiv is down.";
                        }
                        else
                        {
                            DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //SAVE TO SESSION FOR SAVING TO irsRefinitiv.SaveWatchlist

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                                {
                                    //refinitivcs.saveWatchlist_UpdateCIFID(dt.Rows[0][1].ToString(), cifid);
                                    //refinitivcs.get_refinitiv_dt(dt);
                                    System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                    return "";
                                }
                                else
                                {
                                    int hitCount = 0;
                                    foreach (var item in refinitiv)
                                    {
                                        hitCount = item.responseBody.results.Count;
                                    }
                                    refinitivcs.get_refinitiv_dt(dt);
                                    irsrefinitiv.SaveWatchlist(dt, cifid, false, hitCount);

                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                        if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                        if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                        if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                        if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                        if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                        if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                    }
                                    return hitlist;
                                }
                            }
                            else
                            {
                                return "";
                            }
                        }
                    }
                    else
                    {    //ONBOARDING - CIF CREATION OR CIF EDIT
                        if (Session["OldNameFieldsValue"] != null) // EDIT
                        {
                            bool OverrideCheckingValue = true;
                            refinitivcs.Refinitiv_ArchiveDelete(cifid, true, userid); //ARCHIVEDELETE THE OLD CIFID AND CASESYSTEMID
                            refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down); //CREATE A NEW CASESYSTEMID FOR THE EDITED NAME.
                            DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //DISPLAY THE NEW WATCHLIST OVERRIDE NAME EDITED NAME.

                            if (down == true)
                            {
                                return "Refinitiv is down.";
                            }
                            else
                            {
                                if (dt != null && dt.Rows.Count > 0)
                                {

                                    //refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down); //CREATE A NEW CASESYSTEMID FOR THE EDITED NAME.
                                    //DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //DISPLAY THE NEW WATCHLIST OVERRIDE NAME EDITED NAME.

                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                                        {
                                            //refinitivcs.get_refinitiv_dt(dt);
                                            System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                            return "";
                                        }
                                        else
                                        {
                                            int hitCount = 0;
                                            foreach (var item in refinitiv)
                                            {
                                                hitCount = item.responseBody.results.Count;
                                            }
                                            refinitivcs.get_refinitiv_dt(dt);
                                            irsrefinitiv.SaveWatchlist(dt, cifid, OverrideCheckingValue, hitCount); //SAVE THE NEW CASESYSTEMID TO TBL_REFINITIV_HITLIST

                                            for (int i = 0; i < dt.Rows.Count; i++)
                                            {
                                                if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                                if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                                if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                                if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                                if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                                if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                                if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                                if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                                if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                                if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                            }
                                            return hitlist;
                                        }
                                    }
                                    else
                                    {
                                        //refinitivcs.get_refinitiv_dt(dt);
                                        return "";
                                    }
                                }
                                else { return ""; }
                            }
                        }
                        else // ADD
                        {
                            bool OverrideCheckingValue = true;
                            refinitivcs.Refinitiv_ArchiveDelete(cifid, true, userid);
                            refinitiv = (IEnumerable<irsRefinitiv.ScreeningResponseDto>)irsrefinitiv.ScreenNameRefinitiv(branch_code, FirstName, MiddleName, LastName, isRefinitivIndividual, secondary, out down);

                            if (down == true)
                            {
                                return "Refinitiv is down.";
                            }
                            else
                            {
                                DataTable dt = irsrefinitiv.DisplayWatchlist(branch_code, refinitiv, cifid, false, "", "", "", isRefinitivIndividual, secondary, out down); //NEWLY CREATED CIFID

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    if (dt.Rows.Count == 1 && (dt.Rows[0][2] == DBNull.Value || dt.Rows[0][2].ToString() == string.Empty))
                                    {
                                        //refinitivcs.get_refinitiv_dt(dt);
                                        //refinitivcs.saveWatchlist_UpdateCIFID(dt.Rows[0][1].ToString(), cifid);
                                        //refinitivcs.saveWatchlist_UpdateByCaseSystemID(dt.Rows[0]["caseSystemId"].ToString(), cifid);
                                        System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
                                        return "";
                                    }
                                    else
                                    {
                                        int hitCount = 0;
                                        foreach (var item in refinitiv)
                                        {
                                            hitCount = item.responseBody.results.Count;
                                        }
                                        refinitivcs.get_refinitiv_dt(dt);
                                        irsrefinitiv.SaveWatchlist(dt, cifid, OverrideCheckingValue, hitCount);

                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            if (dt.Rows[i]["MatchScore"].ToString() != "") { hitlist += dt.Rows[i]["MatchScore"].ToString() + "\n"; }
                                            if (dt.Rows[i]["MatchTerm"].ToString() != "") { hitlist += dt.Rows[i]["MatchTerm"].ToString() + "\n"; }
                                            if (dt.Rows[i]["MatchNameType"].ToString() != "") { hitlist += dt.Rows[i]["MatchNameType"].ToString() + "\n"; }
                                            if (dt.Rows[i]["PrimaryName"].ToString() != "") { hitlist += dt.Rows[i]["PrimaryName"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Category"].ToString() != "") { hitlist += dt.Rows[i]["Category"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Categories"].ToString() != "") { hitlist += dt.Rows[i]["Categories"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Gender"].ToString() != "") { hitlist += dt.Rows[i]["Gender"].ToString() + "\n"; }
                                            if (dt.Rows[i]["DOB"].ToString() != "") { hitlist += dt.Rows[i]["DOB"].ToString() + "\n"; }
                                            if (dt.Rows[i]["Country"].ToString() != "") { hitlist += dt.Rows[i]["Country"].ToString() + "\n"; }
                                            if (dt.Rows[i]["PEPStatus"].ToString() != "") { hitlist += dt.Rows[i]["PEPStatus"].ToString() + "\n"; }
                                        }
                                        return hitlist;
                                    }
                                }
                                else
                                {
                                    return "";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                /* VAPT IAG 20191007 */
                new csAuditLog().writeVAPTLogger("", "get_refinitivcifid_merchant", ex.Message, "", "", "", csAuditLog.AuditType.Systems, csAuditLog.AlertType.Process);
                /*********************/
                return "";
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }
        /* REFINITIV */


        public String PercentageScore(string cfirstname, string cmiddlename, string clastname, string strUploadingBranch)
        {
            String hitlist = "";
            irsSqlCommonsWs getDistinctComplianceNames = new irsSqlCommonsWs();
            DataSet dsComplianceNames = getDistinctComplianceNames.execStoredProcedure("proc_get_compliance_names", new Object[] { IRSClientData.Location });
            DataTable dtComplianceNames = new DataTable();
            dtComplianceNames = dsComplianceNames.Tables[0];

            if (dtComplianceNames.Rows.Count != 0)
            {
                string firstname = "";
                string lastname = "";
                string middlename = "";
                string aliasfirstname = "";
                string aliaslastname = "";
                string aliasmiddlename = "";
                string v_pscore = "";

                irsSqlCommonsWs cmns = new irsSqlCommonsWs();
                DataSet dsps = cmns.execStoredProcedure("proc_get_branch_setup_percentagescore", new Object[] { IRSClientData.Location });
                DataTable dtps = new DataTable();
                dtps = dsps.Tables[0];
                v_pscore = dtps.Rows[0]["flddescription"].ToString();

                for (int i = 0; i < dtComplianceNames.Rows.Count; i++)
                {
                    firstname = dtComplianceNames.Rows[i]["FirstName"].ToString();
                    lastname = dtComplianceNames.Rows[i]["LastName"].ToString();
                    middlename = dtComplianceNames.Rows[i]["MiddleName"].ToString();

                    string uwname = firstname.ToString() + " " + middlename.ToString() + " " + lastname.ToString();
                    string cifname = cfirstname.ToString() + " " + cmiddlename.ToString() + " " + clastname.ToString();

                    MatchsMaker match = new MatchsMaker(uwname, cifname);
                    float pscore = (match.Score);

                    if (pscore >= Convert.ToInt32(v_pscore))
                    {
                        hitlist += uwname + "\n";
                        hitlist = "\n" + hitlist + "\nPlease get complete list from your local compliance officer.\n";
                        break;
                    }

                    //For Aliases
                    aliasfirstname = dtComplianceNames.Rows[i]["AliasFirstName"].ToString();
                    aliaslastname = dtComplianceNames.Rows[i]["AliasLastName"].ToString();
                    aliasmiddlename = dtComplianceNames.Rows[i]["AliasMiddleName"].ToString();

                    string a_uwname = aliasfirstname.ToString() + " " + aliasmiddlename.ToString() + " " + aliaslastname.ToString();

                    MatchsMaker a_match = new MatchsMaker(a_uwname, cifname);
                    float a_pscore = (a_match.Score);

                    if (a_pscore >= Convert.ToInt32(v_pscore))
                    {
                        hitlist += uwname + "\n";
                        hitlist = "\n" + hitlist + "\nPlease get complete list from your local compliance officer.\n";
                        break;
                    }
                }
                return hitlist;
            }
            return string.Empty;
        }

        public bool isStreamLined()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    DataSet ds = commons.execStoredProcedure("sp_cif_check_streamlining", new Object[] { IRSClientData.Location });
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        int isStreamLined = 0;
                        int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out isStreamLined);
                        if (isStreamLined == 1)
                        {
                            return true;
                        }
                    }
                }
            }
            catch { }
            finally
            {
                if (commons != null) { commons = null; }
            }
            return false;
        }

        public bool insert_newcif(String CIFID, String FirstName, String LastName, String MiddleName, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("proc_save_newcif_hitlist", new Object[] { CIFID, FirstName, LastName, MiddleName, get_userid().ToString(), IRSClientData.Location.ToString() });
                    if (res == -1)
                    {
                        throw new Exception(commons.Message);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void save_soundexlist(String CIFID, String FirstName, String LastName, String MiddleName, int CIFClass)
        {
            try
            {
                String errmsg = "";
                insert_to_soundex(CIFID, out errmsg);
                if (CIFClass.ToString() == "1" || CIFClass.ToString() == "2")
                {
                    insert_newcif(CIFID, FirstName, LastName, MiddleName, out errmsg);
                }
            }
            catch { }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_hit_list_phonetic()
        {
            return Session["strNewEnrolledCIFPhonetics1"].ToString() + "~" +
                    Session["strNewEnrolledCIFPhonetics2"].ToString() + "~" +
                    Session["strNewEnrolledCIFPhonetics3"].ToString() + "~" +
                    Session["strNewEnrolledCIFPhonetics4"].ToString() + "~" +
                    Session["strNewEnrolledCIFPhonetics5"].ToString() + "~" +
                    Session["strNewEnrolledCIFPhonetics6"].ToString() + "~" +
                    Session["strNewEnrolledCIFPhonetics13"].ToString() + "~" +
                    Session["strNewEnrolledCIFPhonetics14"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_percentagematch_name()
        {
            return Session["strFirstName"].ToString() + "~" +
                    Session["strLastName"].ToString() + "~" +
                    Session["strMiddleName"].ToString() + "~" +
                    Session["strPercentageScore"].ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_to_soundex(String CIFID, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("sp_save_cif_hitlist_validations", new Object[] { CIFID, Session["strNewEnrolledCIFPhonetics1"], Session["strNewEnrolledCIFPhonetics2"], Session["strNewEnrolledCIFPhonetics3"], Session["strNewEnrolledCIFPhonetics4"], Session["strNewEnrolledCIFPhonetics5"], Session["strNewEnrolledCIFPhonetics6"], Session["strNewEnrolledCIFPhonetics13"], Session["strNewEnrolledCIFPhonetics14"], IRSClientData.Location.ToString() });
                    if (res == -1)
                    {
                        throw new Exception(commons.Message);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void clear_refinitiv_session()
        {
            System.Web.HttpContext.Current.Session["REFINITIV_DT"] = null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public static string clean_name(string return_value)
        {
            if (string.IsNullOrEmpty(return_value) == true)
            {
                return string.Empty;
            }

            return_value = return_value.Replace(".", " ");
            return_value = return_value.Replace(",", " ");
            return_value = return_value.Replace("!", " ");
            return_value = return_value.Replace("@", " ");
            return_value = return_value.Replace("#", " ");
            return_value = return_value.Replace("$", " ");
            return_value = return_value.Replace("%", " ");
            return_value = return_value.Replace("^", " ");
            return_value = return_value.Replace("&", " ");
            return_value = return_value.Replace("*", " ");
            return_value = return_value.Replace("(", " ");
            return_value = return_value.Replace(")", " ");
            //return_value = return_value.Replace("-", " ");
            return_value = return_value.Replace("+", " ");
            return_value = return_value.Replace("=", " ");
            return_value = return_value.Replace("'", " ");
            return_value = return_value.Replace("~", " ");
            return_value = return_value.Replace("'", " ");
            return_value = return_value.Replace(":", " ");
            return_value = return_value.Replace(">", " ");
            return_value = return_value.Replace("<", " ");
            return_value = return_value.Replace("?", " ");
            return_value = return_value.Replace("[", " ");
            return_value = return_value.Replace("]", " ");
            return_value = return_value.Replace("{", " ");
            return_value = return_value.Replace("}", " ");
            return_value = return_value.Replace("|", " ");
            return_value = return_value.Replace(";", " ");
            return_value = return_value.Replace("''", " ");

            return return_value.Trim().ToUpper();

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_hitlist(String FirstName, String LastName, String MiddleName, string branchCode, string ciftype)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            String hitlist = "";
            Session["strNewEnrolledCIFPhonetics1"] = "";
            Session["strNewEnrolledCIFPhonetics2"] = "";
            Session["strNewEnrolledCIFPhonetics3"] = "";
            Session["strNewEnrolledCIFPhonetics4"] = "";
            Session["strNewEnrolledCIFPhonetics5"] = "";
            Session["strNewEnrolledCIFPhonetics6"] = "";
            Session["strNewEnrolledCIFPhonetics13"] = "";
            Session["strNewEnrolledCIFPhonetics14"] = "";

            Session["strFirstName"] = "";
            Session["strLastName"] = "";
            Session["strMiddleName"] = "";
            Session["strPercentageScore"] = "";

            string strNewEnrolledCIFPhonetics1 = "";
            string strNewEnrolledCIFPhonetics2 = "";
            string strNewEnrolledCIFPhonetics3 = "";
            string strNewEnrolledCIFPhonetics4 = "";
            string strNewEnrolledCIFPhonetics5 = "";
            string strNewEnrolledCIFPhonetics6 = "";
            string strNewEnrolledCIFPhonetics13 = "";
            string strNewEnrolledCIFPhonetics14 = "";

            try
            {
                DataSet dsp = commons.execStoredProcedure("proc_get_branch_setup_matching", new Object[] { IRSClientData.Location });
                DataTable dtp = new DataTable();
                dtp = dsp.Tables[0];

                //For Percentage Matching Criteria
                if (dtp.Select("fldcode in ('UWPM', 'UW')").Length > 0)
                {
                    irsSqlCommonsWs getDistinctComplianceNames = new irsSqlCommonsWs();
                    DataSet dsComplianceNames = getDistinctComplianceNames.execStoredProcedure("proc_get_compliance_names", new Object[] { IRSClientData.Location });
                    DataTable dtComplianceNames = new DataTable();
                    dtComplianceNames = dsComplianceNames.Tables[0];

                    if (dtComplianceNames.Rows.Count != 0)
                    {
                        string uwfirstname = "";
                        string uwlastname = "";
                        string uwmiddlename = "";
                        string uwaliasfirstname = "";
                        string uwaliaslastname = "";
                        string uwaliasmiddlename = "";
                        string uwfileformat = "";
                        string v_pscore = "";
                        string uw_seqid = "";

                        irsSqlCommonsWs cmns = new irsSqlCommonsWs();
                        DataSet dsps = cmns.execStoredProcedure("proc_get_branch_setup_percentagescore", new Object[] { IRSClientData.Location });
                        DataTable dtps = new DataTable();

                        dtps = dsps.Tables[0];
                        v_pscore = dtps.Rows[0]["flddescription"].ToString();

                        if (dtComplianceNames.Rows.Count > 0)
                        {
                            DelRecPercentTemp();
                        }

                        for (int i = 0; i < dtComplianceNames.Rows.Count; i++)
                        {
                            uwfirstname = dtComplianceNames.Rows[i]["FirstName"].ToString();
                            uwlastname = dtComplianceNames.Rows[i]["LastName"].ToString();
                            uwmiddlename = dtComplianceNames.Rows[i]["MiddleName"].ToString();
                            uwaliasfirstname = dtComplianceNames.Rows[i]["AliasFirstName"].ToString();
                            uwaliaslastname = dtComplianceNames.Rows[i]["AliasLastName"].ToString();
                            uwaliasmiddlename = dtComplianceNames.Rows[i]["AliasMiddleName"].ToString();
                            uwfileformat = dtComplianceNames.Rows[i]["FileFormat"].ToString();
                            uw_seqid = dtComplianceNames.Rows[i]["uw_seqid"].ToString();

                            string uwname = uwfirstname.ToString() + " " + uwmiddlename.ToString() + " " + uwlastname.ToString();
                            string cifname = "";
                            if (ciftype == "1")
                            {
                                //Individual
                                cifname = FirstName.ToString() + " " + MiddleName.ToString() + " " + LastName.ToString();
                            }

                            if (ciftype == "2")
                            {
                                //Business
                                cifname = LastName.ToString();
                            }

                            string uwaliasfullname = uwaliaslastname.ToString() + " " + uwaliasfirstname.ToString() + " " + uwaliasmiddlename.ToString();

                            MatchsMaker match = new MatchsMaker(uwname.ToUpper(), cifname.ToUpper());
                            float pscore = (match.Score);

                            if (pscore >= Convert.ToInt32(v_pscore))
                            {
                                irsSqlCommonsWs com = new irsSqlCommonsWs();
                                String errmsg = "";

                                //insert_pname(uwfileformat, uwfirstname.ToString(), uwmiddlename.ToString(), uwlastname.ToString(), uwaliasfullname.ToString(), uw_seqid, pscore, out errmsg);
                                insert_pname(uwfileformat, uwfirstname.ToString(), uwmiddlename.ToString(), uwlastname.ToString(), uwaliasfullname.ToString(), uw_seqid, pscore, "Any Combination in Firstname, Lastname, Middlename (Percentage)", out errmsg);

                                Session["strFirstName"] = uwfirstname.ToString();
                                Session["strLastName"] = uwlastname.ToString();
                                Session["strMiddleName"] = uwmiddlename.ToString();
                                Session["strPercentageScore"] = "UWPM";

                                hitlist += "No of Hits" + "\n";
                            }

                            //For Aliases
                            string a_uwname = uwaliasfirstname.ToString() + " " + uwaliasmiddlename.ToString() + " " + uwaliaslastname.ToString();

                            MatchsMaker a_match = new MatchsMaker(a_uwname, cifname);
                            float a_pscore = (a_match.Score);

                            if (a_pscore >= Convert.ToInt32(v_pscore))
                            {
                                irsSqlCommonsWs com = new irsSqlCommonsWs();

                                String errmsg = "";
                                insert_pname(uwfileformat, uwfirstname.ToString(), uwmiddlename.ToString(), uwlastname.ToString(), uwaliasfullname.ToString(), uw_seqid, a_pscore, "Any Combination in Alias Firstname, Lastname, Middlename (Percentage)", out errmsg);

                                Session["strFirstName"] = uwfirstname.ToString();
                                Session["strLastName"] = uwlastname.ToString();
                                Session["strMiddleName"] = uwmiddlename.ToString();
                                Session["strPercentageScore"] = "UWPM";

                                hitlist += "No of Hits" + "\n";
                            }
                        }
                        return hitlist;
                    }
                    return string.Empty;
                }

                //For Fuzzy Logic (JARO)
                else if (dtp.Select("fldcode in ('UWFL', 'UWFLA', 'UW')").Length > 0)
                {
                    if (ciftype == "2")
                    {
                        //Business
                        FirstName = String.Empty;
                        MiddleName = String.Empty;
                    }

                    irsSqlCommonsWs getDistinctComplianceNames = new irsSqlCommonsWs();
                    //DataSet dsComplianceNames = getDistinctComplianceNames.execStoredProcedure("proc_get_compliance_names_aka", new Object[] { IRSClientData.Location });
                    DataSet dsComplianceNames = getDistinctComplianceNames.execStoredProcedure("proc_get_compliance_names_screen_filtered2", new Object[] { FirstName, MiddleName, LastName, IRSClientData.Location, ciftype });
                    DataTable dtComplianceNames = new DataTable();
                    dtComplianceNames = dsComplianceNames.Tables[0];

                    if (dtComplianceNames.Rows.Count != 0)
                    {
                        string uwfirstname = "";
                        string uwlastname = "";
                        string uwmiddlename = "";
                        string uwaliasfirstname = "";
                        string uwaliaslastname = "";
                        string uwaliasmiddlename = "";
                        string uwAKAremarksname = "";
                        string uwfileformat = "";
                        string v_pscore = "";
                        string uw_seqid = "";

                        irsSqlCommonsWs cmns = new irsSqlCommonsWs();
                        DataSet dsps = cmns.execStoredProcedure("proc_get_branch_setup_percentagescore", new Object[] { IRSClientData.Location });
                        DataTable dtps = new DataTable();

                        dtps = dsps.Tables[0];
                        v_pscore = dtps.Rows[0]["flddescription"].ToString();

                        if (dtComplianceNames.Rows.Count > 0)
                        {
                            DelRecPercentTemp();
                        }

                        for (int i = 0; i < dtComplianceNames.Rows.Count; i++)
                        {
                            uwfirstname = dtComplianceNames.Rows[i]["FirstName"].ToString();
                            uwlastname = dtComplianceNames.Rows[i]["LastName"].ToString();
                            uwmiddlename = dtComplianceNames.Rows[i]["MiddleName"].ToString();
                            uwaliasfirstname = dtComplianceNames.Rows[i]["AliasFirstName"].ToString();
                            uwaliaslastname = dtComplianceNames.Rows[i]["AliasLastName"].ToString();
                            uwaliasmiddlename = dtComplianceNames.Rows[i]["AliasMiddleName"].ToString();
                            uwAKAremarksname = dtComplianceNames.Rows[i]["AKA"].ToString();
                            uwfileformat = dtComplianceNames.Rows[i]["FileFormat"].ToString();
                            uw_seqid = dtComplianceNames.Rows[i]["uw_seqid"].ToString();

                            //Watchlist Name
                            string uwname = uwfirstname.ToString().Trim() + " " + uwmiddlename.ToString().Trim() + " " + uwlastname.ToString().Trim();
                            //CIF Name
                            string cifname = "";
                            string cifname_intch = "";

                            if (ciftype == "1")
                            {
                                //Individual
                                cifname = FirstName.ToString().Trim() + " " + MiddleName.ToString().Trim() + " " + LastName.ToString().Trim();
                                
                                //Individual : Interchanged CIF Name (LN+FN+MN)
                                cifname_intch = LastName.ToString().Trim() + " " + FirstName.ToString().Trim() + " " + MiddleName.ToString().Trim();
                            }

                            if (ciftype == "2")
                            {
                                //Business
                                cifname = LastName.ToString().Trim();
								cifname_intch = LastName.ToString().Trim();

                                cifname = clean_name(cifname);
                                cifname_intch = clean_name(cifname_intch);
                            }

                            //Alias
                            string a_uwname = "";
                            if (uwfileformat == "1")
                            {
                                a_uwname = uwaliaslastname.ToString().Trim();
                            }
                            else
                            {
                                a_uwname = uwaliasfirstname.ToString().Trim() + " " + uwaliasmiddlename.ToString().Trim() + " " + uwaliaslastname.ToString().Trim();
                            }

                            ////Fuzzy Logic-Jaro (UW vs CIF)
                            double drate = JaroWinkler.RateSimilarity(uwname.ToString().Trim(), cifname.ToString().Trim(), cifname_intch.ToString().Trim());
                            float auw_pscore_j = ((float)drate) * 100;
                            auw_pscore_j = (float)Math.Round(auw_pscore_j);

                            if (auw_pscore_j >= Convert.ToInt32(v_pscore))
                            {
                                irsSqlCommonsWs com = new irsSqlCommonsWs();
                                String errmsg = "";

                                insert_pname(uwfileformat, uwfirstname.ToString().Trim(), uwmiddlename.ToString().Trim(), uwlastname.ToString().Trim(), a_uwname.ToString().Trim(), uw_seqid, auw_pscore_j, "Any Combination in Firstname, Lastname, Middlename (Fuzzy)", out errmsg);

                                Session["strFirstName"] = uwfirstname.ToString();
                                Session["strLastName"] = uwlastname.ToString();
                                Session["strMiddleName"] = uwmiddlename.ToString();
                                Session["strPercentageScore"] = "UWFL";

                                hitlist += "No of Hits" + "\n";
                            }                            
                            

                            //Fuzzy Logic-Jaro (UWAlias vs CIF)
                            double a_drate = JaroWinkler.RateSimilarity(a_uwname.ToString().Trim(), cifname.ToString().Trim(), cifname_intch.ToString().Trim());
                            float a_pscore_j = ((float)a_drate) * 100;
                            a_pscore_j = (float)Math.Round(a_pscore_j);

                            if (a_pscore_j >= Convert.ToInt32(v_pscore))
                            {
                                irsSqlCommonsWs com = new irsSqlCommonsWs();
                                String errmsg = "";

                                insert_pname(uwfileformat, uwfirstname.ToString().Trim(), uwmiddlename.ToString().Trim(), uwlastname.ToString().Trim(), a_uwname.ToString().Trim(), uw_seqid, a_pscore_j, "Any Combination in Alias Firstname, Lastname, Middlename (Fuzzy)", out errmsg);

                                Session["strFirstName"] = uwfirstname.ToString();
                                Session["strLastName"] = uwlastname.ToString();
                                Session["strMiddleName"] = uwmiddlename.ToString();
                                Session["strPercentageScore"] = "UWFL";

                                hitlist += "No of Hits" + "\n";
                            }                            
                            

                            if (dtp.Select("fldcode in ('UWFLA')").Length > 0)
                            {
                                //For Weak Aliases on Remarks Field
                                //MatchsMakerFL match_aw = new MatchsMakerFL(uwAKAremarksname.ToString(), cifname.ToString());
                                //float aw_pscore = (match_aw.Score);
                                double aw_drate = JaroWinkler.RateSimilarity(uwAKAremarksname.ToString().Trim(), cifname.ToString().Trim(), cifname_intch.ToString().Trim());
                                float aw_pscore = ((float)aw_drate) * 100;
                                aw_pscore = (float)Math.Round(aw_pscore);
                                
                                if (aw_pscore >= Convert.ToInt32(v_pscore))
                                {
                                    irsSqlCommonsWs com = new irsSqlCommonsWs();
                                    String errmsg = "";
                                    if (uwAKAremarksname.ToString() != "")
                                    {
                                        a_uwname = a_uwname.ToString() + " (" + uwAKAremarksname.ToString() + ")";
                                    }
                                    insert_pname(uwfileformat, uwfirstname.ToString().Trim(), uwmiddlename.ToString().Trim(), uwlastname.ToString().Trim(), a_uwname.ToString().Trim(), uw_seqid, aw_pscore, "Any Combination in Firstname, Lastname, Middlename (Fuzzy AKA)", out errmsg);

                                    Session["strFirstName"] = uwfirstname.ToString();
                                    Session["strLastName"] = uwlastname.ToString();
                                    Session["strMiddleName"] = uwmiddlename.ToString();
                                    Session["strPercentageScore"] = "UWFL";

                                    hitlist += "No of Hits" + "\n";
                                }
                            }
                        }
                        return hitlist;
                    }

                    return string.Empty;
                }

                //For UWEM, UWP, UWMEM Matching Criteria
                else
                {
                    irsMethaphone myMethaphone = new irsMethaphone();
                    string strFirstNamePhonetics = "";
                    string strLastNamePhonetics = "";
                    string strMiddleNamePhonetics = "";

                    strFirstNamePhonetics = myMethaphone.DoubleMethaphone(FirstName);
                    strLastNamePhonetics = myMethaphone.DoubleMethaphone(LastName);
                    strMiddleNamePhonetics = myMethaphone.DoubleMethaphone(MiddleName);

                    strNewEnrolledCIFPhonetics1 = strFirstNamePhonetics + strMiddleNamePhonetics + strLastNamePhonetics;
                    strNewEnrolledCIFPhonetics2 = strFirstNamePhonetics + strLastNamePhonetics + strMiddleNamePhonetics;
                    strNewEnrolledCIFPhonetics3 = strLastNamePhonetics + strFirstNamePhonetics + strMiddleNamePhonetics;
                    strNewEnrolledCIFPhonetics4 = strLastNamePhonetics + strMiddleNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics5 = strMiddleNamePhonetics + strFirstNamePhonetics + strLastNamePhonetics;
                    strNewEnrolledCIFPhonetics6 = strMiddleNamePhonetics + strLastNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics13 = strLastNamePhonetics + strFirstNamePhonetics;
                    strNewEnrolledCIFPhonetics14 = strFirstNamePhonetics + strLastNamePhonetics;

                    Session["strNewEnrolledCIFPhonetics1"] = strNewEnrolledCIFPhonetics1.ToString();
                    Session["strNewEnrolledCIFPhonetics2"] = strNewEnrolledCIFPhonetics2.ToString();
                    Session["strNewEnrolledCIFPhonetics3"] = strNewEnrolledCIFPhonetics3.ToString();
                    Session["strNewEnrolledCIFPhonetics4"] = strNewEnrolledCIFPhonetics4.ToString();
                    Session["strNewEnrolledCIFPhonetics5"] = strNewEnrolledCIFPhonetics5.ToString();
                    Session["strNewEnrolledCIFPhonetics6"] = strNewEnrolledCIFPhonetics6.ToString();
                    Session["strNewEnrolledCIFPhonetics13"] = strNewEnrolledCIFPhonetics13.ToString();
                    Session["strNewEnrolledCIFPhonetics14"] = strNewEnrolledCIFPhonetics14.ToString();
                    if (commons != null)
                    {
                        DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist_validations", new Object[] { FirstName, LastName, MiddleName, branchCode, strNewEnrolledCIFPhonetics1, strNewEnrolledCIFPhonetics2, strNewEnrolledCIFPhonetics3, strNewEnrolledCIFPhonetics4, strNewEnrolledCIFPhonetics5, strNewEnrolledCIFPhonetics6, strNewEnrolledCIFPhonetics13, strNewEnrolledCIFPhonetics14 });
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ((ds.Tables[0].Rows.Count > 5) ? 5 : ds.Tables[0].Rows.Count); i++)
                            {
                                hitlist += ds.Tables[0].Rows[i][1].ToString().ToUpper().Trim() + "\n";
                            }

                            if (ds.Tables[0].Rows.Count > 5)
                            {
                                hitlist = "\n" + hitlist + "\nShowing maximum of five(5) possible hits.\nPlease get complete list from your local compliance officer.\n";
                            }

                            return hitlist;
                        }
                    }
                    return "";
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }



        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        private void DelRecPercentTemp()
        {
            string SQLDEL = "exec PROC_DEL_PERCENTAGE_TEMP '" + get_userid().ToString() + "'";
            irsSqlCommonsWs sql = new irsSqlCommonsWs();
            sql.execScalar(SQLDEL);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool insert_pname(String FFormat, String FirstName, String LastName, String MiddleName, String AliasFullName, String UWSeqID, float PScore, String MatchType, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("proc_save_pname", new Object[] { FFormat, FirstName, MiddleName, LastName, AliasFullName, get_userid().ToString(), IRSClientData.Location.ToString(), UWSeqID.ToString(), PScore, MatchType });
                    if (res == -1)
                    {
                        throw new Exception(commons.Message);
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
            }
            finally
            {
                if (commons != null) { commons = null; }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_accounts_record(string seqid)
        {
            int isid = 0;
            try
            {
                isid = int.Parse(seqid);
            }
            catch (Exception e)
            {
                throw new Exception("@get_accounts_record: " + e.Message);
            }

            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                commons.EnforceConstraint = false;

                DataSet ds = commons.getDataSet("sp_internet_get_accounts", true, new Object[] { isid });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return ds;
                }
                throw new Exception("@get_accounts_record: " + commons.Message);
            }

            throw new Exception("@get_accounts_record: " + coms_errmsg);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool get_web_remittance_defaults(string key)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    DataSet ds = commons.getDataSet("sp_get_web_remittance_defaults", true, new object[] { key, IRSClientData.Location });

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get WEB REMITTANCE DEFAULTS, Reason : " + ex.Message);
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String get_web_remittance_defaults_acct_currency()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                if (commons != null)
                {
                    DataSet ds = commons.getDataSet("sp_get_web_remittance_default_acct_currency", true, new Object[] { IRSClientData.Location });

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0]["FldValue"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get WEB REMITTANCE DEFAULT ACCT CURRENCY , Reason : " + ex.Message);
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
            return "";
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_ids_record(string seqid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_ids", true, new Object[] { seqid });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_van_record(string seqid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_van", true, new Object[] { seqid }); ;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_notes_record(string seqid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();

            try
            {
                if (commons != null)
                {
                    DataSet ds = commons.getDataSet("sp_internet_get_notes", true, new Object[] { seqid });
                    return ds;
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_converted_branchcode(string brcode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_cif_get_bankname", true, new Object[] { brcode });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cif_bank(string seqid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_cif_get_bank", true, new Object[] { seqid });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_isapproved(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_isapproved", true, new object[] { seq_id });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cifid_card(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_cifid_card", true, new Object[] { seq_id });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_cards_count(string cardno, string cardname, string bankcode, string cifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return Convert.ToInt32(commons.getDataSet("sp_internet_count_cards", true, new Object[] { cardno, cardname, bankcode, cifid }).Tables[0].Rows[0][0]);
                }
                return 0;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet approve_cards(int seq_id, string cifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_approve_cards", true, new Object[] { seq_id, cifid });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cards(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_cards", true, new Object[] { seq_id });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_card_details(string seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_card_details", true, new object[] { seq_id });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_bencards(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_benecards", true, new Object[] { seq_id });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_cardseqid(int seq_id)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_get_cardseqid", true, new Object[] { seq_id });
                }
                return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null) { commons = null; }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_bank()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                return commons.getDataSet("sp_internet_dropdown_bank", true, new object[] { });
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_bank_pddts()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_dropdown_bank_pddts", true, new Object[] { });
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }
        
        //INSTAPAY ALYSSA
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_bank_instapay()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                if (commons != null)
                {
                    return commons.getDataSet("sp_internet_dropdown_bank_instapay", true, new Object[] { });
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }
        //INSTAPAY ALYSSA

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_dropdown_data()
        {

            throw new Exception("Not Yet Implemented.");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_all_dropdowns()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet ds = commons.getDataSet("isp_inetrem_dropdowns", true, new Object[] { IRSClientData.Location, IRSClientData.Role });
            if (ds != null && ds.Tables.Count > 0) return ds;
            throw new Exception(commons.Message);
        }

        /* FOR CBIP */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getAccountStatus(string accountType)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            DataSet ds;
            if (accountType == "1")
            {
                ds = commons.getDataSet("isp_getLookup", true, new Object[] { "AccountStatusST" });
            }
            else
            {
                ds = commons.getDataSet("isp_getLookup", true, new Object[] { "AccountStatusIM" });
            }
            if (ds != null && ds.Tables.Count > 0) return ds;
            throw new Exception(commons.Message);
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_title(string fldgroup)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                return commons.getDataSet("sp_internet_dropdown", true, new Object[] { fldgroup, IRSClientData.Location, IRSClientData.Role });
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_cifclass()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                return commons.getDataSet("sp_internet_dropdown_cifclass", true, new Object[] { });
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_products(int prod_fldval, String brnch_cd)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                return commons.getDataSet("sp_internet_dropdown_products", true, new Object[] { prod_fldval, brnch_cd });
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_target_curr()
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                return commons.getDataSet("sp_internet_dropdown_target_curr", true, new Object[] { IRSClientData.Location });

            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_benename(string cifid, string bcifid)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            try
            {
                return commons.getDataSet("sp_internet_dd_benename", true, new Object[] { cifid, bcifid });
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_cif_type(string cifid)
        {
            /* VAPT 20191014 */
            int ret = 0;
            if (cifid != null)
            {
                return Convert.ToInt32(new irsSqlCommonsWs().execScalar("sp_internet_get_ciftype1", new Object[] { cifid }));
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_cif_type2(string seqid)
        {
            return Convert.ToInt32(new irsSqlCommonsWs().execScalar("sp_internet_get_ciftype", new object[] { seqid }));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_account_type(string seqid)
        {
            return Convert.ToInt32(new irsSqlCommonsWs().execScalar("sp_cif_get_account_type", new object[] { seqid }));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public int get_account_status(string seqid)
        {
            return Convert.ToInt32(new irsSqlCommonsWs().execScalar("sp_cif_get_account_status", new object[] { seqid }));
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string get_account_bcode(string seqid)
        {
            return (new irsSqlCommonsWs().execScalar("sp_cif_get_accountgfc_bcode", new object[] { seqid })).ToString();
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_cifid(string intremseqid, string cifid)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_set_cifid", new object[] { intremseqid, cifid });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_cifid_card(string carseqid, string cifid)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_set_cifid_card", new object[] { carseqid, cifid });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_bene_isApproved(string seqid)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_set_isApproved", new object[] { seqid });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_bene_isApproved_card(string seqid)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_set_isApproved_card", new object[] { seqid });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_bene_status(string seqid, string status, string rej_reason)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_set_bene_status", new object[] { seqid, status, rej_reason });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_bene_created_cifid(string seqid, string cifid)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_set_bene_created_cifid", new object[] { seqid, cifid });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_card_status(string seqid, string status, string rej_reason)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_set_card_status", new object[] { seqid, status, rej_reason });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void update_rel_notmode(string benecifid, string remcifid)
        {
            new irsSqlCommonsWs().execScalar("sp_internet_update_rel_notmode", new object[] { benecifid, remcifid });
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_profile_header(string seq_id, string arg_type)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_profile_header", true, new Object[] { seq_id, arg_type });
                if (ds != null) return ds;
                throw new Exception(commons.Message);
            }
            throw new Exception("Unable to access irsSqlCommonsWs");
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string enroll_client
        (
            int seqid,
            string lname, string fname,
            string mname, string alias,
            string bdate, int age,
            string bplace, string bcode,
            string contactno, string email,
            string add1, string add2,
            string zip, string acctno,
            int acctcurr, int accttype,
            string int_benef, int cifclass,
            string remcifid
        )
        {
            try
            {
                string cifid = null;
                string column_values = null;
                string errmsg = null;

                column_values = "CIFType~1|CIFClass~" + cifclass + "|" + "RemitterCIFID~" + remcifid + "|";
                column_values += "LastName~" + lname + "|" + "FirstName~" + fname + "|" + "MiddleName~" + mname + "|";
                column_values += "Alias~" + alias + "|" + "BirthDate~" + bdate + "|" + "Age~" + age + "|";
                column_values += "BirthPlace~" + bplace + "|" + "BranchCode~" + bcode + "|" + "ContactNo~" + contactno + "|";
                column_values += "EmailAddress~" + email + "|" + "Address1~" + add1 + "|" + "Address2~" + add2 + "|";
                column_values += "ZipCode~" + zip + "|" + "Remarks~" + int_benef + "|";
                column_values += "AccountCurrency~" + acctcurr + "|" + "AccountType~" + accttype + "|" + "AccountNo~" + acctno;

                irsClientEnrollment ce = new irsClientEnrollment();
                cifid = ce.enroll_agent(irsClientEnrollment.Function_Name.INSERT, column_values, out errmsg);

                return cifid;
            }
            catch
            {
                return null;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_product_duplicate(String CIFID, String RemCIFID, String ProductCode, String AccountNo, String Currency, String DomBankCode, String BranchCode, String OtherBankSeqId)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    Object[] plist =
                        {
                            CIFID
                            ,ProductCode
                            ,AccountNo
                            ,Currency
                            ,DomBankCode
                            ,BranchCode
                            ,OtherBankSeqId
                            ,RemCIFID
                        };
                    DataSet ds = commons.execStoredProcedure("sp_cif_check_product_duplicate", plist);

                    if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_product_if_req_account(String ProductCode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    DataSet ds = commons.execStoredProcedure("sp_cif_internet_check_prod_with_acct", new Object[] { int.Parse(ProductCode) });

                    if (int.Parse(ds.Tables[0].Rows[0][0].ToString()) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return true;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_card_if_exists_in_gfc(String CardNumber)
        {
            try
            {
                if (new irsLinkToGFC().CheckGFCNumber(CardNumber) == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet insert_to_cif_master
        (
            int seqid,
            string lname, string fname,
            string mname, string alias,
            string bdate, int age,
            string bplace, string bcode,
            string contactno, string email,
            string add1, string add2,
            string add3, string add4,
            string add5, string add6,
            string zip, string acctno,
            string compadd, int cifclass
        )
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_internet_insert_to_cif_master");
                    sqcommands.Parameters.Add("@INTREM_SEQID", SqlDbType.BigInt, 100).Value = seqid;
                    sqcommands.Parameters.Add("@LastName", SqlDbType.VarChar, 200).Value = lname;
                    sqcommands.Parameters.Add("@FirstName", SqlDbType.VarChar, 80).Value = fname;
                    sqcommands.Parameters.Add("@MiddleName", SqlDbType.VarChar, 80).Value = mname;
                    sqcommands.Parameters.Add("@Alias", SqlDbType.VarChar, 200).Value = alias;
                    sqcommands.Parameters.Add("@BirthDate", SqlDbType.DateTime, 100).Value = bdate;
                    sqcommands.Parameters.Add("@Age", SqlDbType.Int, 100).Value = age;
                    sqcommands.Parameters.Add("@BirthPlace", SqlDbType.VarChar, 200).Value = bplace;
                    sqcommands.Parameters.Add("@BranchCode", SqlDbType.VarChar, 200).Value = bcode;
                    sqcommands.Parameters.Add("@ContactNo", SqlDbType.VarChar, 50).Value = contactno;
                    sqcommands.Parameters.Add("@Email", SqlDbType.VarChar, 255).Value = email;
                    sqcommands.Parameters.Add("@Address1", SqlDbType.VarChar, 100).Value = add1;
                    sqcommands.Parameters.Add("@Address2", SqlDbType.VarChar, 100).Value = add2;
                    sqcommands.Parameters.Add("@Address3", SqlDbType.VarChar, 100).Value = add3;
                    sqcommands.Parameters.Add("@Address4", SqlDbType.VarChar, 100).Value = add4;
                    sqcommands.Parameters.Add("@Address5", SqlDbType.VarChar, 100).Value = add5;
                    sqcommands.Parameters.Add("@Address6", SqlDbType.VarChar, 100).Value = add6;
                    sqcommands.Parameters.Add("@ZipCode", SqlDbType.VarChar, 4).Value = zip;
                    sqcommands.Parameters.Add("@AccountNo", SqlDbType.VarChar, 50).Value = acctno;
                    sqcommands.Parameters.Add("@CompleteAddWithOSA", SqlDbType.VarChar, 400).Value = compadd;
                    sqcommands.Parameters.Add("@CIFClass", SqlDbType.SmallInt, 100).Value = cifclass;

                    return commons.getDataSet(sqcommands, "CIF_INSERT_MASTER");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet insert_user_accounts
        (
            string cifid, string userid, string fullname
        )
        {

            string password = new csPasswordGen().generatePassword("", "");
            int defaultdays = new irsUsers().getDefaultExpireDays();

            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_insert_internet_remitter_useraccount");
                    sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 50).Value = cifid;
                    sqcommands.Parameters.Add("@UserID", SqlDbType.VarChar, 50).Value = userid;
                    sqcommands.Parameters.Add("@Password", SqlDbType.VarChar, 150).Value = password;
                    sqcommands.Parameters.Add("@Location", SqlDbType.VarChar, 50).Value = "USW";
                    sqcommands.Parameters.Add("@DateCreated", SqlDbType.DateTime, 100).Value = DateTime.Now;//Convert.ToDateTime(new irsTimeZone().branchTimeZone("USW"));
                    sqcommands.Parameters.Add("@DaysExpired", SqlDbType.Int, 100).Value = defaultdays;
                    sqcommands.Parameters.Add("@Role", SqlDbType.Int, 100).Value = 10;
                    sqcommands.Parameters.Add("@Flags", SqlDbType.Int, 100).Value = 1;
                    return commons.getDataSet(sqcommands, "CIF_INSERT_INTERNET_ACCOUNTS");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet insert_to_cif_cards
        (
            string cifid, string cardno, string cardname, string expdate, string bankcode, string cardtype, string cardclass, string issdate, string currency, string country
        )
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_internet_insert_to_cif_cards");
                    sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
                    sqcommands.Parameters.Add("@CardNo", SqlDbType.VarChar, 50).Value = cardno;
                    sqcommands.Parameters.Add("@CardName", SqlDbType.VarChar, 50).Value = cardname;
                    sqcommands.Parameters.Add("@ExpiryDate", SqlDbType.DateTime, 100).Value = expdate;
                    sqcommands.Parameters.Add("@BankCode", SqlDbType.VarChar, 4).Value = bankcode;
                    sqcommands.Parameters.Add("@CardType", SqlDbType.Int, 100).Value = cardtype;
                    sqcommands.Parameters.Add("@CardClass", SqlDbType.Int, 100).Value = cardclass;
                    sqcommands.Parameters.Add("@IssueDate", SqlDbType.Int, 100).Value = issdate;
                    sqcommands.Parameters.Add("@Currency", SqlDbType.Int, 100).Value = currency;
                    sqcommands.Parameters.Add("@Country", SqlDbType.Int, 100).Value = country;
                    return commons.getDataSet(sqcommands, "CIF_INSERT_CARDS");
                }
            }
            catch
            {
                return null;
            }
            finally
            {
                if (commons != null)
                {
                    commons = null;
                }
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
            return null;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String INSERT_CIF_TO_FC(String client_CIF_id)
        {
            irsAccountInquiry irsFC = new irsAccountInquiry();
            try
            {
                String CustomerID = "";

                if (check_cif_if_exist(client_CIF_id))
                {
                    throw new Exception("CIF already registered to FC");
                }

                if (irsFC != null)
                {
                    String FCResponse = irsFC.saveCIFtoFC(client_CIF_id);

                    if (FCResponse.IndexOf("Success") > -1)
                    {
                        String[] FCResArr = FCResponse.Split('|');
                        CustomerID = FCResArr[0].Split('=')[1].ToString().Trim();

                        if (!save_customer_id_to_cif(client_CIF_id, CustomerID))
                        {
                            throw new Exception("No row inserted.");
                        }
                    }
                    else
                    {
                        throw new Exception(FCResponse);
                    }
                }
                return "00|Successfuly saved CIF to FC|" + CustomerID;
            }
            catch (Exception ex)
            {
                return "99|Failed to insert CIF to FC, Reason : " + ex.Message;
            }
            finally
            {
                if (irsFC != null)
                {
                    irsFC = null;
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_userrole_rights_for_fccustomer_id(int UserRole)
        {
            irsSqlCommonsWs helper = new irsSqlCommonsWs();
            try
            {
                if ((int)helper.execStoredProcedure("sp_cif_fc_cust_id_check_userrole_rights", new Object[] { UserRole }).Tables[0].Rows[0][0] == 1)
                {
                    return true;
                }
                return false;
            }
            catch //(Exception ex) 
            {
                return false;
            }
        }
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_userrole_is_agent(int UserRole)
        {
            irsSqlCommonsWs helper = new irsSqlCommonsWs();
            try
            {
                if ((int)helper.execStoredProcedure("sp_internet_check_userrole_if_agent", new Object[] { UserRole }).Tables[0].Rows[0][0] > 0)
                {
                    return true;
                }
                return false;
            }
            catch //(Exception ex)
            {
                return false;
            }
        }

        private bool check_cif_if_exist(String CIFID)
        {
            irsSqlCommonsWs helper = new irsSqlCommonsWs();
            try
            {
                String sql = "select count(1) from tbl_cif_other_ids where cifid = '" + CIFID + "' and IRSWorldSystem = 6";

                if (int.Parse(helper.getDataSet(helper.getSqlCommand(sql, false), "tbl_cif_other_ids").Tables[0].Rows[0][0].ToString()) > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private bool save_customer_id_to_cif(String CIFID, String CustomerID)
        {
            irsSqlCommonsWs helper = new irsSqlCommonsWs();
            try
            {
                Object[] plist =
                    {
                        CIFID
                        ,CustomerID
                    };

                if (helper.execStoredProcedureUpdate("sp_cif_insert_fc_customerid", plist) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert Customer ID to CIF, Reason : " + ex.Message);
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void GetFieldsForUpdate(String fields)
        {
            if (Session["OldFieldsValue"] != null)
            {
                Session["OldFieldsValue"] = null;
            }
            Session["OldFieldsValue"] = fields;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void SetOldNameFieldsForCompare(String fields)
        {
            if (Session["OldNameFieldsValue"] != null)
            {
                Session["OldNameFieldsValue"] = null;
            }

            Session["OldNameFieldsValue"] = fields;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void SaveFieldsForUpdate(String fields)
        {
            ///OldValue
            string oldHomeAddrs = string.Empty;
            string oldMobile = string.Empty;
            string oldTelNo = string.Empty;
            string oldEmail = string.Empty;
            string oldBdate = string.Empty;
            ///New Value
            string newHomeAddrs = string.Empty;
            string newTelNo = string.Empty;
            string newMobile = string.Empty;
            string newEmail = string.Empty;
            string newBdate = string.Empty;
            ///Static Value
            string CIFID = string.Empty;
            string CIFClass = string.Empty;

            irsSqlCommonsWs sql = new irsSqlCommonsWs();
            csAuditLog audit = new csAuditLog();
            if (Session["OldFieldsValue"] != null)
            {
                string[] oldFields = Session["OldFieldsValue"].ToString().Split('|');
                string[] newFields = fields.Split('|');

                for (int k = 0; k < oldFields.Length; k++)
                {
                    string[] val = oldFields[k].Split('~');

                    switch (val[0])
                    {
                        case "HomeAddress":
                            oldHomeAddrs = val[1];
                            break;
                        case "Mobile":
                            oldMobile = val[1];
                            break;
                        case "TelNo":
                            oldTelNo = val[1];
                            break;
                        case "Email":
                            oldEmail = val[1];
                            break;
                        case "BDate":
                            oldBdate = val[1];
                            break;
                        default:
                            break;
                    }
                }

                for (int k = 0; k < newFields.Length; k++)
                {
                    string[] val = newFields[k].Split('~');

                    switch (val[0])
                    {
                        case "HomeAddress":
                            newHomeAddrs = val[1];
                            break;
                        case "Mobile":
                            newMobile = val[1];
                            break;
                        case "TelNo":
                            newTelNo = val[1];
                            break;
                        case "Email":
                            newEmail = val[1];
                            break;
                        case "BDate":
                            newBdate = val[1];
                            break;
                        case "CIFID":
                            CIFID = val[1];
                            break;
                        case "CIFClass":
                            CIFClass = val[1];
                            break;
                        default:
                            break;
                    }
                }

                string homeAddress = string.Empty;
                string mobileNo = string.Empty;
                string telNo = string.Empty;
                string emailAdd = string.Empty;
                string bDate = string.Empty;
                int ctr = 0;

                if (oldHomeAddrs != newHomeAddrs)
                {
                    homeAddress = newHomeAddrs;
                    ctr++;
                }
                if (oldMobile != newMobile)
                {
                    mobileNo = newMobile;
                    ctr++;
                }
                if (oldTelNo != newTelNo)
                {
                    telNo = newTelNo;
                    ctr++;
                }
                if (oldEmail != newEmail)
                {
                    emailAdd = newEmail;
                    ctr++;
                }
                if (oldBdate != newBdate)
                {
                    bDate = newBdate;
                    ctr++;
                }

                if (ctr > 0)
                {
                    string msg;

                    int k = 0;
                    try
                    {
                        k = sql.execStoredProcedureUpdate("usp_InsertCIFUpdatesPerTeller", new object[] { CIFID, homeAddress, mobileNo, telNo, emailAdd, bDate, CIFClass, this.get_userid() });
                        if (k < 0)
                        {
                            msg = sql.Message;
                            audit.writeAudit("Internet Remitter", "Internet Remitter", "Error", msg, "", csAuditLog.Category.Insert);
                        }
                    }
                    catch (Exception e)
                    {
                        msg = e.Message;
                        audit.writeAudit("Internet Remitter", "Internet Remitter", "Error", msg, "", csAuditLog.Category.Insert);
                    }
                    finally
                    {
                        Session["OldFieldsValue"] = null;
                    }
                }
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool CompareOldNewNameFields(String fields)
        {
            ///OldValue
            string oldLName = string.Empty;
            string oldFName = string.Empty;
            string oldMName = string.Empty;

            ///New Value
            string newLName = string.Empty;
            string newFName = string.Empty;
            string newMName = string.Empty;

            int ctr = 0;

            if (Session["OldNameFieldsValue"] != null)
            {
                string[] oldFields = Session["OldNameFieldsValue"].ToString().Split('|');
                string[] newFields = fields.Split('|');

                for (int k = 0; k < oldFields.Length; k++)
                {
                    string[] val = oldFields[k].Split('~');

                    switch (val[0])
                    {
                        case "LName":
                            oldLName = val[1];
                            break;
                        case "FName":
                            oldFName = val[1];
                            break;
                        case "MName":
                            oldMName = val[1];
                            break;
                        default:
                            break;
                    }
                }

                for (int k = 0; k < newFields.Length; k++)
                {
                    string[] val = newFields[k].Split('~');

                    switch (val[0])
                    {
                        case "LName":
                            newLName = val[1];
                            break;
                        case "FName":
                            newFName = val[1];
                            break;
                        case "MName":
                            newMName = val[1];
                            break;
                        default:
                            break;
                    }
                }

                string LName = string.Empty;
                string FName = string.Empty;
                string MName = string.Empty;

                if (oldLName != newLName)
                {
                    ctr++;
                }
                if (oldFName != newFName)
                {
                    ctr++;
                }
                if (oldMName != newMName)
                {
                    ctr++;
                }
            }

            if (ctr > 0)
            {
                return true;
            }
            else if (ctr == 0 && Session["OldNameFieldsValue"] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region PRIVATE METHOD: INTERNET_REMITTER_ADDBENE
        private irsInternetRem INTERNET_REMITTER()
        {
            if (Session["IRS_INTERNET_REMITTER"] == null)
            {
                Session["IRS_INTERNET_REMITTER"] = new irsInternetRem();
            }
            return (irsInternetRem)Session["IRS_INTERNET_REMITTER"];
        }

        private bool do_internet_remitter_addbene(string instruction)
        {
            irsInternetRem cad = this.INTERNET_REMITTER();
            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cad.selectedRow = -1;
                            if (cad.insert_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            cad.seq_id = this.INTERNET_REMITTER_SEQ_ID();
                            cad.selectedRow = 0;

                            if (cad.update_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_INTERNETREMITTER_BENE_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (cad.commit_record() == false)
                            {
                                this.set_message(cad.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            cad.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["IRS_INTERNET_REMITTER"] = null;
                Session["IRS_INTERNET_REMITTER"] = cad;
                Session["INTERNET_REMITTER_SEQ_ID"] = null;
                Session["INTERNET_REMITTER_SEQ_ID"] = cad.seq_id;

                cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion

        #region PRIVATE METHOD: INTERNET_REMITTER_ADD_PROD
        private irsInternetRemProducts INTERNET_REMITTER_ADD_PROD()
        {
            if (Session["IRS_INTERNET_REMITTER_ADD_PROD"] == null)
            {
                Session["IRS_INTERNET_REMITTER_ADD_PROD"] = new irsInternetRemProducts();
            }
            return (irsInternetRemProducts)Session["IRS_INTERNET_REMITTER_ADD_PROD"];
        }

        private bool do_internet_remitter_addprod(string instruction)
        {
            irsInternetRemProducts prod = this.INTERNET_REMITTER_ADD_PROD();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            prod.selectedRow = -1;
                            if (prod.insert_record() == false)
                            {
                                this.set_message(prod.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            prod.seq_id = this.INTERNET_REMITTER_SEQ_ID();
                            prod.selectedRow = 0;

                            if (prod.update_record() == false)
                            {
                                this.set_message(prod.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_INTERNETREMITTER_PROD_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (prod.commit_record() == false)
                            {
                                this.set_message(prod.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            prod.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["IRS_INTERNET_REMITTER_ADD_PROD"] = null;
                Session["IRS_INTERNET_REMITTER_ADD_PROD"] = prod;
                Session["INTERNET_REMITTER_SEQ_ID"] = null;
                Session["INTERNET_REMITTER_SEQ_ID"] = prod.seq_id;
                prod = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion

        #region PRIVATE METHOD: INTERNET_REMITTER_ADD_CARDS
        private irsInternetRemCards INTERNET_REMITTER_CARDS()
        {
            if (Session["IRS_INTERNET_REMITTER_CARDS"] == null)
            {
                Session["IRS_INTERNET_REMITTER_CARDS"] = new irsInternetRemCards();
            }
            return (irsInternetRemCards)Session["IRS_INTERNET_REMITTER_CARDS"];
        }

        private bool do_internet_remitter_addcards(string instruction)
        {
            irsInternetRemCards card = this.INTERNET_REMITTER_CARDS();
            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            card.selectedRow = -1;
                            if (card.insert_record() == false)
                            {
                                this.set_message(card.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            card.seq_id = this.INTERNET_REMITTER_SEQ_ID();
                            card.selectedRow = 0;

                            if (card.update_record() == false)
                            {
                                this.set_message(card.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_INTERNETREMITTER_CARDS_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (card.commit_record() == false)
                            {
                                this.set_message(card.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            card.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["IRS_INTERNET_REMITTER_CARDS"] = null;
                Session["IRS_INTERNET_REMITTER_CARDS"] = card;
                Session["INTERNET_REMITTER_SEQ_ID"] = null;
                Session["INTERNET_REMITTER_SEQ_ID"] = card.seq_id;

                card = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion

        #region PRIVATE METHOD: INTERNET_REMITTER_ADD_ACCT
        private irsInternetRemAcct INTERNET_REMITTER_ACCT()
        {
            if (Session["IRS_INTERNET_REMITTER_ACCT"] == null)
            {
                Session["IRS_INTERNET_REMITTER_ACCT"] = new irsInternetRemAcct();
            }
            return (irsInternetRemAcct)Session["IRS_INTERNET_REMITTER_ACCT"];
        }

        private bool do_internet_remitter_addacct(string instruction)
        {
            irsInternetRemAcct acct = this.INTERNET_REMITTER_ACCT();
            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            acct.selectedRow = -1;
                            if (acct.insert_record() == false)
                            {
                                this.set_message(acct.messages); return false;
                            }
                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "UPDATE")
                    {
                        try
                        {
                            acct.seq_id = this.INTERNET_REMITTER_SEQ_ID();
                            acct.selectedRow = 0;

                            if (acct.update_record() == false)
                            {
                                this.set_message(acct.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "COMMIT")
                    {
                        try
                        {
                            if (this.SET_INTERNETREMITTER_ACCT_RECORD(this.parameter_values) == false)
                            {
                                return false;
                            }

                            if (acct.commit_record() == false)
                            {
                                this.set_message(acct.messages); return false;
                            }

                            return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }

                    if (instruction == "ROLLBACK")
                    {
                        try
                        {
                            acct.rollback_record(); return true;
                        }
                        catch (Exception e)
                        {
                            this.set_message(e.Message); return false;
                        }
                    }
                }
            }
            catch
            {
            }
            finally
            {
                Session["IRS_INTERNET_REMITTER_ACCT"] = null;
                Session["IRS_INTERNET_REMITTER_ACCT"] = acct;
                Session["INTERNET_REMITTER_SEQ_ID"] = null;
                Session["INTERNET_REMITTER_SEQ_ID"] = acct.seq_id;

                acct = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }
        #endregion

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string GetUserRole_Card(string urole)
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            DataSet ds = common.getDataSet(common.getSqlCommand("select top 1  * from tbl_Lookup where FldGroup='NoCardAccess' and FldCode='" + urole + "'", false));
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows.Count.ToString();
                }
                return string.Empty;
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool checkAlgoRemarks(string SEQ_ID)
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            DataSet ds = common.getDataSet(common.getSqlCommand("SELECT * FROM tbl_CIF_Accounts WHERE SEQ_ID = " + SEQ_ID + "", false));
            bool ret = false;
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["Remarks"] == DBNull.Value || ds.Tables[0].Rows[0]["Remarks"].ToString().Trim() == "")
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getDynamicValidationBranchCode()
        {
            irsSqlCommonsWs common = new irsSqlCommonsWs();
            DataSet ds = common.getDataSet("wsp_getDynamicValidation", true, new Object[] { getLocation(), 1 });
            int i = 0;
            bool ret = false;

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i <= (ds.Tables[0].Rows.Count - 1); i++)
                {
                    if (ds.Tables[0].Rows[i]["CIFTable"].ToString().Trim().ToUpper() == "TBL_CIF_ACCOUNTS")
                    {
                        if (ds.Tables[0].Rows[i]["CIFAsteriskID"].ToString().Trim().ToUpper() == "ASTPNBBRANCHCODE")
                        {
                            ret = true;
                            break;
                        }
                    }
                }
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool getBranchCodeMandatory(string FldValue)
        {
            string ret = new irsInternetPage().getLookupValue("BranchCodeAsterisk", "FldValue", FldValue, "FldCode");
            if (ret != string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}