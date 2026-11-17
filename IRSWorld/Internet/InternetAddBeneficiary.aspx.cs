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

namespace IRSWorld.Internet
{
    public partial class InternetAddBeneficiary : System.Web.UI.Page
    {
        private string parameter_values = string.Empty;
        private String coms_errmsg = "Unable to access Sql Commons Service.";

        protected void Page_Load(object sender, EventArgs e)
        {
			AjaxPro.Utility.RegisterTypeForAjax(typeof(InternetAddBeneficiary));
			AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranGFMCCommons));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
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
            //DataSet ds = common.getDataSet(common.getSqlCommand("select b.fldCode from tbl_Lookup a, tbl_sub_lookup b where a.FldGroup = b.fldgroup AND a.FldValue = b.FldValue AND A.fldgroup = 'CIFAgeLimit2' AND b.Branchcode = '" + IRSClientData.Location + "' order by b.fldcode asc", false));
            //DataSet ds = common.getDataSet(common.getSqlCommand("select isnull(AgeLimit,-1) as AgeLimit from tbl_branch_setup where branchcode = '" + IRSClientData.Location + "'", false));
            //DataSet ds = common.getDataSet(common.getSqlCommand("select fldCode from tbl_Lookup where FldGroup='CIFAgeLimit' AND FldDescription='" + IRSClientData.Location + "'", false));
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
                    catch{}
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public Object[] get_client_data()
        {
            if (Session["IRS_CLIENT_DATA"] == null || String.IsNullOrEmpty(IRSClientData.UserId))
            {
                throw new Exception("Session Expired.");
            }
            bool is_agent = check_userrole_is_agent((int)IRSClientData.Role);
            return new Object[] { IRSClientData.CIFID, get_userid(), IRSClientData.Role, IRSClientData.Location, is_agent };
        }

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string enroll_agent(string plist)
		{
			string errmsg = null;
			string cifid  = null;
			irsClientEnrollment enr_agt = new irsClientEnrollment();
			
			cifid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.INSERT, plist, out errmsg);

			if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
			else return cifid;
		}

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
                DataSet ds = commons.getDataSet(sql, false, new Object[] {});
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
				call_errpage();	return false;
			}
			catch (Exception e)
			{
				this.set_message("UNABLE TO PERFORM OPERATION: " + instruction + "\n\n" + e.Message); return false;
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public void set_module_name(string customers_module)
		{
			Session["MODULE_NAME"] = customers_module;
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string read_message()
        {
            if (Session["InternetAddBeneficiary_MESSAGE"] != null)
            {
                return (string)Session["InternetAddBeneficiary_MESSAGE"];
            }
            return string.Empty;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void set_message(string msg)
        {
            Session["InternetAddBeneficiary_MESSAGE"] = null;
            Session["InternetAddBeneficiary_MESSAGE"] = msg;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_InternetAddBeneficiary_BENE_RECORD(string column_values)
        {
            irsInternetRem InternetAddBeneficiary = this.INTERNET_REMITTER();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (InternetAddBeneficiary.set_record(s) == false)
                {
                    this.set_message(InternetAddBeneficiary.messages); return false;
                }

                Session["IRS_INTERNET_REMITTER"] = InternetAddBeneficiary;

                return true;
            }

            InternetAddBeneficiary = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_InternetAddBeneficiary_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_INTERNET_REMITTER_BENE_RECORD(string client_seq_id)
        {
            irsInternetRem InternetAddBeneficiary = this.INTERNET_REMITTER();

            try
            {
                if (InternetAddBeneficiary != null)
                {   
                    if (InternetAddBeneficiary.get_record(client_seq_id) == true)
                    {
                        return InternetAddBeneficiary.getdataRecord();
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
                Session["IRS_INTERNET_REMITTER"] = InternetAddBeneficiary;
            }
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_INTERNET_REMITTER_REM_RECORD(string client_seq_id)
        {
            irsInternetRem InternetAddBeneficiary = this.INTERNET_REMITTER();
            try
            {
                if (InternetAddBeneficiary != null)
                {
                    if (InternetAddBeneficiary.get_record(client_seq_id) == true)
                    {
                        return InternetAddBeneficiary.getdataRecord();
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
                Session["IRS_INTERNET_REMITTER"] = InternetAddBeneficiary;
            }
        }

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_InternetAddBeneficiary_PROD_RECORD(string column_values)
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

			this.set_message("UNABLE TO EXECUTE METHOD: SET_InternetAddBeneficiary_PROD_RECORD(). PARAM VALUE NOT SPECIFIED.");
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
		public bool SET_InternetAddBeneficiary_CARDS_RECORD(string column_values)
		{
			irsInternetRemCards InternetAddBeneficiary_cards = this.INTERNET_REMITTER_CARDS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (InternetAddBeneficiary_cards.set_record(s) == false)
				{
					this.set_message(InternetAddBeneficiary_cards.messages); return false;
				}

				Session["IRS_INTERNET_REMITTER_CARDS"] = InternetAddBeneficiary_cards;

				return true;
			}

			InternetAddBeneficiary_cards = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_InternetAddBeneficiary_CARDS_RECORD(). PARAM VALUE NOT SPECIFIED.");
			return false;
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_InternetAddBeneficiary_ACCT_RECORD(string column_values)
        {
            irsInternetRemAcct InternetAddBeneficiary_acct = this.INTERNET_REMITTER_ACCT();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (InternetAddBeneficiary_acct.set_record(s) == false)
                {
                    this.set_message(InternetAddBeneficiary_acct.messages); return false;
                }

                Session["IRS_INTERNET_REMITTER_ACCT"] = InternetAddBeneficiary_acct;

                return true;
            }

            InternetAddBeneficiary_acct = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_InternetAddBeneficiary_ACCT_RECORD(). PARAM VALUE NOT SPECIFIED.");
            return false;
        }

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_INTERNET_REMITTER_CARDS_RECORD(string client_seq_id)
		{
			irsInternetRemCards InternetAddBeneficiary_cards = this.INTERNET_REMITTER_CARDS();
			try
			{
				if (InternetAddBeneficiary_cards != null)
				{
					if (InternetAddBeneficiary_cards.get_record(client_seq_id) == true)
					{
						return InternetAddBeneficiary_cards.getdataRecord();
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
				Session["IRS_INTERNET_REMITTER_CARDS"] = InternetAddBeneficiary_cards;
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet getdropdown_source_filteredcurrency(int prod_type, int role)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_internet_dropdown_filtered_currency", true, new Object[] { prod_type, role});
                if (ds != null && ds.Tables.Count > 0) return ds;
                throw new Exception("@getdropdown_source_filteredcurrency: " + commons.Message);
            }

            throw new Exception("@getdropdown_source_filteredcurrency: " + coms_errmsg);
        }

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public Object[] get_all_mandatory_flds(int cif_class, string branch_code, int ciftype)
		{
			irsSqlCommonsWs commons = new irsSqlCommonsWs();
            if (commons != null)
            {
                DataSet ds = commons.getDataSet("sp_cif_get_all_mandatory_flds", true, new object[] { cif_class, branch_code });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return new Object[] { cif_class, branch_code, ciftype, ds };
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
                //throw new Exception("@get_mandatory_flds_count: " + commons.Message);
            }

            //throw new Exception("@get_mandatory_flds_count: " + coms_errmsg);
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
                        
            for(int i=0; i<empty_fld.Length; i++)
            {
                /* checks if field is mandatory, when count > 0 then mandatory */
                int count = get_mandatory_flds_count(empty_fld[i], cif_class, branch_code);

                if (empty_fld[i].IndexOf("PrefMailAdd") > 0)
                {
                    prefMailUncheckCount = prefMailUncheckCount + 1;
                }

                if(count > 0) 
                {
                    if(reqd_flds.IndexOf(empty_fld[i]) < 0) /* do not append empty_fld[i] when it is already included in reqd_flds */
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

            for(int i=0; i<reqd_fld.Length; i++)
            {
                DataSet ds = get_mandatory_flds_label(reqd_fld[i]);
                


                if (ds!= null && ds.Tables[0].Rows.Count > 0)
                {   
                    fld_lbl = ds.Tables[0].Rows[0]["CIFLabel"].ToString();
                }
                if(fld_lbl != "")
                {   
                    if(fld_lbl_tmp.IndexOf(fld_lbl) < 0) 
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
                //throw new Exception("@get_cif_cards_count: " + commons.Message);
            }

            //throw new Exception("@get_cif_cards_count: " + coms_errmsg);
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
                /* Use SQL Scalar
                DataSet ds = commons.getDataSet("sp_cif_get_cifclass_count", true, new Object[] { seq_id, cif_class });
                if (ds != null && ds.Tables.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                return 0;
                 * */

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
                DataSet ds = commons.getDataSet("sp_cif_get_cifid_seqid_loyalty", true, new Object[] { VIPNo });
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
        public String check_and_get_hitlist(String FirstName, String LastName, String MiddleName)
        {
            try
            {
                //if (isStreamLined())
                //{
                //    return get_hitlist(FirstName,LastName, MiddleName, IRSClientData.Location);
                //}
                return get_hitlist(((FirstName == null) ? "" : FirstName), ((LastName == null) ? "" : LastName), ((MiddleName == null) ? "" : MiddleName), IRSClientData.Location);
            }
            catch{}
            return "";
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
            catch{}
            finally
            {
                if (commons != null) { commons = null; }
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
            }
            catch{}
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

        public bool insert_to_soundex(String CIFID, out String errmsg)
        {
            errmsg = "";
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("sp_save_cif_hitlist_validations", new Object[] { CIFID, Session["strNewEnrolledCIFPhonetics1"], Session["strNewEnrolledCIFPhonetics2"], Session["strNewEnrolledCIFPhonetics3"], Session["strNewEnrolledCIFPhonetics4"], Session["strNewEnrolledCIFPhonetics5"], Session["strNewEnrolledCIFPhonetics6"], Session["strNewEnrolledCIFPhonetics13"], Session["strNewEnrolledCIFPhonetics14"], IRSClientData.Location.ToString()});
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

        public String get_hitlist(String FirstName, String LastName, String MiddleName, string branchCode)
        {
            irsSqlCommonsWs commons = new irsSqlCommonsWs();
            SqlCommand sqcommands = null;
            irsMethaphone myMethaphone = new irsMethaphone();
            string strFirstNamePhonetics = "";
            string strLastNamePhonetics = "";
            string strMiddleNamePhonetics = "";

            Session["strNewEnrolledCIFPhonetics1"] = "";
            Session["strNewEnrolledCIFPhonetics2"] = "";
            Session["strNewEnrolledCIFPhonetics3"] = "";
            Session["strNewEnrolledCIFPhonetics4"] = "";
            Session["strNewEnrolledCIFPhonetics5"] = "";
            Session["strNewEnrolledCIFPhonetics6"] = "";
            Session["strNewEnrolledCIFPhonetics13"] = "";
            Session["strNewEnrolledCIFPhonetics14"] = "";

            string strNewEnrolledCIFPhonetics1 = "";
            string strNewEnrolledCIFPhonetics2 = "";
            string strNewEnrolledCIFPhonetics3 = "";
            string strNewEnrolledCIFPhonetics4 = "";
            string strNewEnrolledCIFPhonetics5 = "";
            string strNewEnrolledCIFPhonetics6 = "";
            string strNewEnrolledCIFPhonetics13 = "";
            string strNewEnrolledCIFPhonetics14 = "";

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

            try
            {
                if (commons != null)
                {
                    /*
                        @v_FirstName		varchar(200)
                        ,@v_LastName		varchar(80)
                        ,@v_MiddleName		varchar(80)
                        ,@v_BranchCode		varchar(4)
                     */
                    /* Hajie orig code
                    DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist", new Object[] { FirstName, LastName, MiddleName, branchCode }); 
                    */
                    DataSet ds = commons.execStoredProcedure("sp_check_and_get_cif_hitlist_validations", new Object[] { FirstName, LastName, MiddleName, branchCode, strNewEnrolledCIFPhonetics1, strNewEnrolledCIFPhonetics2, strNewEnrolledCIFPhonetics3, strNewEnrolledCIFPhonetics4, strNewEnrolledCIFPhonetics5, strNewEnrolledCIFPhonetics6, strNewEnrolledCIFPhonetics13, strNewEnrolledCIFPhonetics14});
                    String hitlist = "";
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
            catch
            {
                return "";
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
            catch(Exception ex)
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

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_dropdown_data()
        {

            throw new Exception("Not Yet Implemented.");
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
                return commons.getDataSet("sp_internet_dd_benename", true, new Object[] { cifid, bcifid});
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
            return Convert.ToInt32(new irsSqlCommonsWs().execScalar("sp_internet_get_ciftype1", new Object[] { cifid }));
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

				column_values = "CIFType~1|CIFClass~" + cifclass + "|" + "RemitterCIFID~" + remcifid  + "|";
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
        public bool check_product_duplicate(String CIFID, String ProductCode, String AccountNo, String Currency, String DomBankCode, String BranchCode, String OtherBankSeqId)
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

		/*
		 * pag hindi nag error ibig sabihin hindi na to kasali
		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string enroll_cards
		(
			string cifid, string cardno, string cardname, string expdate, string bankcode, string cardtype, string cardclass, string issdate, string currency, string country
		)
		{
			try
			{
				string column_values = null;
				string errmsg = null;

				column_values  = "CIFID~" + cifid + "|" + "CardNo~" + cardno + "|" + "CardName~" + cardname + "|";
				column_values += "CardExpiryDate~" + expdate + "|" + "BankCode~" + bankcode + "|" + "CardType~" + cardtype + "|";
				column_values += "CardClass~" + cardclass + "|" + "CardIssueDate~" + issdate + "|" + "CardCurrency~" + currency + "|";
				column_values += "Country~" + country;

				irsClientEnrollment ce = new irsClientEnrollment();
				cifid = ce.enroll_agent(irsClientEnrollment.Function_Name.ADD_CARD, column_values, out errmsg);

				return cifid;
			}
			catch
			{
				return null;
			}
		}
		*/

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
           
            string cifid, string userid,string fullname
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
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value		= cifid;
					sqcommands.Parameters.Add("@CardNo", SqlDbType.VarChar, 50).Value		= cardno;
					sqcommands.Parameters.Add("@CardName", SqlDbType.VarChar, 50).Value		= cardname;
					sqcommands.Parameters.Add("@ExpiryDate", SqlDbType.DateTime, 100).Value = expdate;
					sqcommands.Parameters.Add("@BankCode", SqlDbType.VarChar, 4).Value		= bankcode;
					sqcommands.Parameters.Add("@CardType", SqlDbType.Int, 100).Value		= cardtype;
					sqcommands.Parameters.Add("@CardClass", SqlDbType.Int, 100).Value		= cardclass;
					sqcommands.Parameters.Add("@IssueDate", SqlDbType.Int, 100).Value		= issdate;
					sqcommands.Parameters.Add("@Currency", SqlDbType.Int, 100).Value		= currency;
					sqcommands.Parameters.Add("@Country", SqlDbType.Int, 100).Value			= country;

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
                // log error--
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
                            if (this.SET_InternetAddBeneficiary_BENE_RECORD(this.parameter_values) == false)
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
							if (this.SET_InternetAddBeneficiary_PROD_RECORD(this.parameter_values) == false)
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
							if (this.SET_InternetAddBeneficiary_CARDS_RECORD(this.parameter_values) == false)
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
                            if (this.SET_InternetAddBeneficiary_ACCT_RECORD(this.parameter_values) == false)
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
    }
}
