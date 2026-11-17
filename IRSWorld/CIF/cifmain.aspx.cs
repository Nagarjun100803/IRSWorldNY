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

namespace IRSWorld.CIF
{
	public partial class cifmain : System.Web.UI.Page
	{
        private string parameter_values = string.Empty;        
        public string CIFID = string.Empty;

		protected void Page_Load(object sender, EventArgs e)
		{
            //===================================================================================================
            /* RFC-35248 : Enhancement in Bills Payment OFAC Screening Process & Report */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(BillsPayment.OBPSTransactions));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Internet.InternetRemitter));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranSaving));
            //===================================================================================================
            AjaxPro.Utility.RegisterTypeForAjax(typeof(cifmain));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranGFMCCommons));
            /* VAPT IAG 20170612 */
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Main.IRSMain));
            new Main.IRSMain().checkSession();
            new Main.IRSMain().isUserModule(this.Page);
            AjaxPro.Utility.RegisterTypeForAjax(typeof(RemTranExtra));
            AjaxPro.Utility.RegisterTypeForAjax(typeof(Refinitiv)); /* RFC-37590 - IRSW NY Interface with Refinitiv WorldCheck via API */
        }

        /* RFC-31620 */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string isAdminApproverTag()
        {
            string ret = string.Empty;
            if (Session["AdminApprovalFlagCIFMessage"] != null)
            {
                if (new irsUsers().adminApproverSwitch())
                {
                    return Session["AdminApprovalFlagCIFMessage"].ToString();
                }
            }
            return ret;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public void close_session()
		{
			Session.Abandon();
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public void drop_cifmodules()
		{
			Session["MODULE_NAME"] = null;
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
					if (module_name == "CIF_MERCHANT") { module_name = "CIF_MERCHANTS"; }
                    if (module_name == "CIF_MASTER_INDIVIDUAL" || module_name == "CIF_MASTER_BUSINESS") { return this.do_cif_master(instruction); }
                    if (module_name == "CIF_OCCUPATION"        || module_name == "CIF_PNB_EMPLOYEE")    { return this.do_cif_occupations(instruction); }

                    if (module_name == "CIF_IDS")             { return this.do_cif_id(instruction); }
                    if (module_name == "CIF_ACCOUNTS")        { return this.do_cif_accounts(instruction); }
                    if (module_name == "CIF_ADDRESS")         { return this.do_cif_address(instruction); }
                    if (module_name == "CIF_BENEFICIARIES")   { return this.do_cif_beneficiaries(instruction); }
                    if (module_name == "CIF_CARDS")           { return this.do_cif_cards(instruction); }
                    if (module_name == "CIF_CONTACTS")        { return this.do_cif_contacts(instruction); }
                    if (module_name == "CIF_MERCHANTS_INFO")  { return this.do_cif_merchants_info(instruction); }
                    if (module_name == "CIF_REMITTERS")       { return this.do_cif_remitters(instruction); }
					if (module_name == "CIF_CLASSIFICATIONS") { return this.do_cif_classifications(instruction); }
					if (module_name == "CIF_MERCHANTS")		  { return this.do_cif_merchants(instruction); }
					if (module_name == "CIF_SPOUSE")		  { return this.do_cif_spouse(instruction); }
                    if (module_name == "CIF_VAN")             { return this.do_cif_van(instruction); }
                }

                this.set_message("SESSION EXPIRED. UNABLE TO PERFORM OPERATION: " + instruction); return false;
            }
            catch (Exception e)
            {
                this.set_message("UNABLE TO PERFORM OPERATION: " + instruction + "\n\n" + e.Message); return false;
            }
		}

		private string CLIENT_SEQ_ID()
		{
			if (Session["IRS_SEQ_ID"] != null)
			{
				return (string)Session["IRS_SEQ_ID"];
			}
            return null;
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
			cs_con.execScript(this.Page, "cif_main", "spawn_errpage('Session Expired')");
		}

		private irsTimeZone IRSTimeZone
		{
			get
			{
				if (Session["IRS_TIMEZONE"] != null)
				{
					return ((irsTimeZone)Session["IRS_TIMEZONE"]);
				}

				return null;
			}
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string AgeLimit(string branchcode)
        {
            sqlHelperWs common = new sqlHelperWs();
            DataSet ds =  common.getDataSet(common.getSqlCommand("select fldCode from tbl_Lookup where FldGroup='CIFAgeLimit' AND FldDescription='" + branchcode + "'",false));
            if (ds != null) 
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return ds.Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    return "18";
                }
            }
            return "18";

        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool check_remitter_web_enrollment(int seq_id)
        {
            try
            {
                DataSet ds = get_cifid(seq_id);
                if (ds != null)
                {
                    String CIFID = ds.Tables[0].Rows[0]["CIFID"].ToString();
                    String sReturn = new Remittance.RemOTC().check_remitter_web_enrollment(CIFID);

                    if (sReturn == "false") return false;
                }
            }
            catch{}            
            return true;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_id_details(String IDNo, int IDType, String CIFID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_check_id_exists");
                    sqcommands.Parameters.Add("@v_IDNo", SqlDbType.VarChar, 50).Value = IDNo;
                    sqcommands.Parameters.Add("@v_IDType", SqlDbType.Int, 4).Value = IDType;
                    sqcommands.Parameters.Add("@v_CIFID", SqlDbType.VarChar, 16).Value = CIFID;

                    return commons.getDataSet(sqcommands, "CIF_ID");
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
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

		#region AJAX PUBLIC METHOD

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DateTime get_branch_localtime(string brcode)
		{
			DateTime dt = new irsTimeZone().branchLocalTime(brcode);
			return dt;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_user_cifid()
		{
			string user_cifid = IRSClientData.CIFID;
			return user_cifid; 
		}
		
		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_userlocation()
		{
			string user_location = IRSClientData.Location;
			return user_location; 
		}
		
		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_userid()
		{
			string userid = IRSClientData.UserId;
			return userid;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public short get_userrole()
		{
			short userrole = IRSClientData.Role;
			return userrole;
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
		public DataSet get_master_seqid(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_master_seqid");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					return commons.getDataSet(sqcommands, "CIF_MASTER_SEQ_ID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_id_expirydate(string seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_id_expirydate");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_ID_EXP");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_rem_seqid(int cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_rem_seqid");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					return commons.getDataSet(sqcommands, "CIF_MERCHANT_SEQ_ID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_mun_prov(string zipcode)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_mun_prov");
					sqcommands.Parameters.Add("@truezipcode", SqlDbType.Char, 4).Value = zipcode;
					return commons.getDataSet(sqcommands, "CIF_ZIP_MUN_PROV");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_CIF_MASTER_CREATEDMODIFIED(int SEQ_ID)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_createdmodifieddetails");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = SEQ_ID;
					return commons.getDataSet(sqcommands, "CIF_MASTER_CREATED_MODIFIED");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_merchant_cifid(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_merchant_cifid");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_MER_CIFID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}
		
		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_merchant_details(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_merchant_details");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_MER_DETAILS");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_merchants_info(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_merchants_info");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_MER_INFO");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public void untag_hit(string cifid, string seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();

			commons.execScalar("sp_cif_untag_hit", new string[] { cifid, seq_id });
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public void lock_gfc_card(string cardno, string uid, string uloc, string dlocked)
		{
			sqlHelperWs commons = new sqlHelperWs();

			commons.execScalar("sp_cif_lock_gfc_cards", new string[] { cardno, uid, uloc, dlocked });
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string is_mt103(string branchcode)
		{
			sqlHelperWs commons = new sqlHelperWs();

			object x = commons.execScalar("sp_cif_is_MT103Extract", new string[] { branchcode });

			return x.ToString();
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_fld_desc(string fldval, string fldgroup)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return commons.execScalar("sp_internet_get_flddesc", new string[] { fldval, fldgroup }).ToString();
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_client_ciftype(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();

			object x = commons.execScalar("sp_cif_get_ciftype1", new string[] { cifid });

			return x.ToString();
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_other_bank_type()
		{
			sqlHelperWs commons = new sqlHelperWs();

			object x = commons.execScalar("sp_cif_dropdown_other_bank_type");

			return x == null ? "" : x.ToString();
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string is_card_gfc(string seqid)
		{
			sqlHelperWs commons = new sqlHelperWs();

			object x = commons.execScalar("sp_cif_is_card_gfc", new string[] { seqid});

			return x.ToString();
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string validate_sssno(string sssno, string validsss)
		{
			sqlHelperWs commons = new sqlHelperWs();
	
			object x = commons.execScalar("sp_cif_validate_sssnumber", new string[] { sssno, validsss});

			return x.ToString();
		}

        /* RFC-25832 PAG-IBIG API INTEGRATION */
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string api_pagibig_valid_no(string member_id, string pagibig_merchantcode)
        {
            try
            {
                return new csPagIbigAPI().inquire_pagibig(member_id, pagibig_merchantcode);
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /* RFC-25832 PAG-IBIG API INTEGRATION */


        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string validate_pagibigno(string pagibigno, string validpagibig)
		{
			sqlHelperWs commons = new sqlHelperWs();

			object y = commons.execScalar("sp_cif_validate_pagibignumber", new string[] { pagibigno, validpagibig });

			return y.ToString();
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string validate_memberID(string merchantCode, string memberid)
        {
            bool y = new irsOBPSTransactions().ValidID(memberid, merchantCode, "");
            return y.ToString();
        }

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool validate_acctno_fcalgo(string acctno)
		{
			/*
			 irsClientAccounts ca = new irsClientAccounts();
			 return ca.ValidFC(acctno);			
			 */

			irsAccountAlgo acct_algo = new irsAccountAlgo();
			bool isValidAcct = acct_algo.checkAccount(acctno);
			
			return isValidAcct;
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
		public string get_acct_details(string acctno)
		{
			irsClientAccounts ca = new irsClientAccounts();
			return ca.get_acctdetails(acctno);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_country_code(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_country_code");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_COUNTRYCODE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cifid(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cifid");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_CIFID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cifid_seqid(string gfmcno)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cifid_seqid");
					sqcommands.Parameters.Add("@GFMCNo", SqlDbType.VarChar, 20).Value = gfmcno;
					return commons.getDataSet(sqcommands, "CIF_CIFID_SEQID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cif_classdesc(int fldval)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cifclassdesc");
					sqcommands.Parameters.Add("@FLDVALUE", SqlDbType.Int, 100).Value = fldval;
					return commons.getDataSet(sqcommands, "CIF_CLASSDESC");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cif_classdesc2(int seqid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cifclassdesc2");
					sqcommands.Parameters.Add("@SEQID", SqlDbType.BigInt, 100).Value = seqid;
					return commons.getDataSet(sqcommands, "CIF_CLASSDESC2");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_postalcode(int fldval)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_postalcode");
					sqcommands.Parameters.Add("@FLDVAL", SqlDbType.Int, 100).Value = fldval;
					return commons.getDataSet(sqcommands, "CIF_POSTALCODE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_state(string postalcode)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_state");
					sqcommands.Parameters.Add("@POSTALCODE", SqlDbType.VarChar, 2).Value = postalcode;
					return commons.getDataSet(sqcommands, "CIF_STATE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_zipcode(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_zipcode");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_ZIPCODE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_bank(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_bank");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_CARD_BANK");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_otherbanks_details(string seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_otherbanks_details");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_OTHER_BANK");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_bankname(string brcode)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_bankname");
					sqcommands.Parameters.Add("@BranchCode", SqlDbType.VarChar, 4).Value = brcode;
					return commons.getDataSet(sqcommands, "CIF_BRANCHNAME");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet get_van_product(int fldval)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_cif_get_van_product");
                    sqcommands.Parameters.Add("@seq_id", SqlDbType.Int, 100).Value = fldval;

                    return commons.getDataSet(sqcommands, "CIF_PRODUCT");
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
                if (sqcommands != null)
                {
                    sqcommands.Dispose();
                    sqcommands = null;
                }
            }
        }

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_zipcode_seqid(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_zipcode_seqid");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_ZIPCODE_SEQID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_branchdef_values(string brcode)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("spGetBranchDefaultSetup");
					sqcommands.Parameters.Add("@BranchCode", SqlDbType.VarChar, 10).Value = brcode;
					return commons.getDataSet(sqcommands, "BRANCH_DEF");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cif_postalcode(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_postalcode2");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_POSTCODE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cards_bankcode(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cards_bankcode");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_CARDS_BANKCODE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cards_bankname(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cards_bankname");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_CARDS_BANKNAME");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_accounts_bankname(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_accounts_bankname");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_ACCTS_BANKNAME");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_accounts_bankcode(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_accounts_bankcode");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_ACCTS_BANKCODE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_add_seqid(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_add_seqid");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					return commons.getDataSet(sqcommands, "CIF_ADDRESS_SEQID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}
		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cif_roles(int seq_id)
		{
			sqlHelperWs commons     = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cifclass");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_CLASS");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int count_class(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return Convert.ToInt32(commons.execScalar("sp_cif_count_class", new string[] { cifid }));
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool is_blocklisted(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return (Convert.ToInt32(commons.execScalar("sp_cif_count_hits", new string[] { cifid })) > 0);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool has_txn(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return (Convert.ToInt32(commons.execScalar("sp_cif_has_txn", new string[] { cifid })) > 0);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool has_txn_acct(string seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return (Convert.ToInt32(commons.execScalar("sp_cif_has_txn_acct", new string[] { seq_id })) > 0);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool has_prefmailadd(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return (Convert.ToInt32(commons.execScalar("sp_cif_has_prefmailadd", new string[] { cifid })) > 0);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool has_prefcontactno(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return (Convert.ToInt32(commons.execScalar("sp_cif_has_prefcontactno", new string[] { cifid })) > 0);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int get_address_count(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_address_count");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					return Convert.ToInt32(commons.getDataSet(sqcommands, "CIF_ADDRESS_COUNT").Tables[0].Rows[0][0]);
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int get_reminfo_count(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_reminfo_count");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 20).Value = cifid;
					return Convert.ToInt32(commons.getDataSet(sqcommands, "CIF_REMINFO_COUNT").Tables[0].Rows[0][0]);
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_all_mandatory_flds(int cif_class, string branch_code)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_all_mandatory_flds");
					sqcommands.Parameters.Add("@CIFClass", SqlDbType.Int, 100).Value = cif_class;
					sqcommands.Parameters.Add("@BranchCode", SqlDbType.VarChar, 4).Value = branch_code;
					return commons.getDataSet(sqcommands, "ALL_MANDATORY");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int get_mandatory_flds_count(string cif_field_id, int cif_class, string branch_code)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_check_mandatory_flds");
					sqcommands.Parameters.Add("@CIFFieldID", SqlDbType.VarChar, 100).Value  = cif_field_id;
					sqcommands.Parameters.Add("@CIFClass", SqlDbType.Int, 100).Value		= cif_class;
					sqcommands.Parameters.Add("@BranchCode", SqlDbType.VarChar, 4).Value    = branch_code;

					return Convert.ToInt32(commons.getDataSet(sqcommands, "COUNT_MANDATORY").Tables[0].Rows[0][0]);
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_cif_contacts(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_contacts");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 100).Value = cifid;
					return commons.getDataSet(sqcommands, "CIF_CONTACTS");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool check_acct_gfc(string fcacct, string kbiacct)
		{
			irsClientAccounts ca = new irsClientAccounts();
			return ca.is_gfc(fcacct, kbiacct);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool check_card_gfc(string cardno)
		{
			irsClientCards car = new irsClientCards();
			return car.is_gfc(cardno);
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet is_acct_gfc(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_is_acct_gfc");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_IS_ACCT_GFC");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet is_prefmailadd(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_is_prefmailadd");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_IS_PREFERRED");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet is_prefcontact(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_is_prefcontact");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_IS_PREFERRED_CONT");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_mandatory_flds_label(string cif_field_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_mandatory_flds");
					sqcommands.Parameters.Add("@CIFFieldID", SqlDbType.VarChar, 100).Value = cif_field_id;
					return commons.getDataSet(sqcommands, "CIF_MDTRY_LABEL");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_bene_gfmcaccountno(string gfmcacctseqid, string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_bene_gfmcaccountno");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					sqcommands.Parameters.Add("@GFMCACCTSEQID", SqlDbType.BigInt, 100).Value = gfmcacctseqid;
					return commons.getDataSet(sqcommands, "CIF_BENE_GFMC");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int get_contacts_count(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_contacts_count");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					return Convert.ToInt32(commons.getDataSet(sqcommands, "CIF_CONTACTS_COUNT").Tables[0].Rows[0][0]);
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int get_cifclass_count(int seq_id, int cif_class)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_cifclass_count");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					sqcommands.Parameters.Add("@CIFCLASS", SqlDbType.Int, 100).Value = cif_class;

					return Convert.ToInt32(commons.getDataSet(sqcommands, "CIF_MASTER_SEQ_ID").Tables[0].Rows[0][0]);
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public int get_master_count(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_master_count");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 20).Value = cifid;

					return Convert.ToInt32(commons.getDataSet(sqcommands, "CIF_MASTER_COUNT").Tables[0].Rows[0][0]);
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_ciftype(int seq_id)
		{
			sqlHelperWs commons	  = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("SELECT CIFType FROM TBL_CIF_MASTER WHERE SEQ_ID = " + seq_id, false);
					
					return commons.getDataSet(sqcommands, "CIF_MASTER_SEQ_ID");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose(); 
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_ciftype2(string cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_ciftype1");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					return commons.getDataSet(sqcommands, "CIF_MASTER_CIFTYPE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet is_memid_mandatory(string mercode)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_is_memid_mandatory");
					sqcommands.Parameters.Add("@MerchantCode", SqlDbType.VarChar, 50).Value = mercode;
					return commons.getDataSet(sqcommands, "CIF_MEMID_MANDATORY");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet mandatoryfield(string mercode,string merprodcode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_cif_is_memid_mandatory2");
                    sqcommands.Parameters.Add("@MerchantCode", SqlDbType.VarChar, 50).Value = mercode;
                    sqcommands.Parameters.Add("@MerProdCode", SqlDbType.VarChar, 50).Value = merprodcode;
                    return commons.getDataSet(sqcommands, "CIF_MEMID_MANDATORY2");
                }
                return null;
            }
            catch(Exception ex)
            {
                return null;
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
		public DataSet get_merchant_code(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_merchant_code");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;

					return commons.getDataSet(sqcommands, "CIF_MASTER_FIRSTNAME");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_branch_code(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_branch_code");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;

					return commons.getDataSet(sqcommands, "CIF_MASTER_BRANCHCODE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_contact_type(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_contact_type");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = seq_id;

					return commons.getDataSet(sqcommands, "CIF_CONTACT_TYPE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_chargetype(int fldval)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_charge_type");
					sqcommands.Parameters.Add("@FldVal", SqlDbType.Int, 100).Value = fldval;

					return commons.getDataSet(sqcommands, "CIF_CHARGE_TYPE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_id_type(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_id_type");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;

					return commons.getDataSet(sqcommands, "CIF_ID_TYPE");
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
				if (sqcommands != null)
				{
					sqcommands.Dispose();
					sqcommands = null;
				}
			}
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_profile_header(int seq_id)
		{
            try
            {
                String arg_type = "SEQ_ID";
                if (seq_id < 1)
                {
                    if (Session["IRS_CIF_MASTER"] != null)
                    {
                        seq_id = Convert.ToInt32(((irsClientMaster)Session["IRS_CIF_MASTER"]).seq_id);
                    }
                }

                return new irsSqlCommonsWs().getDataSet("sp_cif_profile_header", true, new Object[] { seq_id, arg_type });

            }
            catch { }

            /*
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{
				if (commons != null)
				{
					if (seq_id < 1)
					{
						if (Session["IRS_CIF_MASTER"] != null)
						{
							seq_id = Convert.ToInt32(((irsClientMaster)Session["IRS_CIF_MASTER"]).seq_id);
						}
					}

					sqcommands = commons.getSqlCommand("sp_cif_profile_header");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_PROFILE");
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
            */

			return null;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet get_beneficiaries_profile(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_beneficiaries_profile");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_BENEFICIARIES_PROFILE");
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
		public DataSet get_merchants_profile(int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_merchants_profile");
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_MERCHANTS_PROFILE");
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
		public DataSet insert_to_cif_class
		(
			string cifid, int cifclass
		)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_insert_into_cifclass");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 20).Value = cifid;
					sqcommands.Parameters.Add("@CIFClass", SqlDbType.SmallInt, 100).Value = cifclass;
					
					return commons.getDataSet(sqcommands, "CIF_INSERT_CLASS");
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
		public DataSet update_preferred_mailadd(string cifid, int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_update_prefmailadd");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_UPDATE_ADDRESS");
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
		public DataSet update_sms_alert(string cifid, int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_update_smsalert");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_UPDATE_SMS");
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
        public bool check_product_if_req_account(int ProductCode)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    //DataSet ds = commons.execStoredProcedure("sp_cif_internet_check_prod_with_acct", new Object[] { int.Parse(ProductCode) });
                    DataSet ds = commons.execStoredProcedure("sp_cif_internet_check_prod_with_acct", new Object[] {ProductCode});

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
        public String validate_product_acct(String ProductCode, String Acct_Seq_ID, String curr_code, String CIFID, String REMCIFID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {
                if (commons != null)
                {
                    Object[] plist = 
                        {
                            ProductCode
                            ,Acct_Seq_ID
                            ,curr_code
                            ,CIFID
                            ,REMCIFID
                        };

                    DataSet ds = commons.execStoredProcedure("sp_cif_validate_product", plist);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        throw new Exception("No dataset returned : " + commons.Message); ;
                    }
                }
                else
                {
                    throw new Exception("Failed to connect to server");
                }
            }
            catch(Exception ex)
            {
                return "Failed to validate account, Reason : " + ex.Message;
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
        }


		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet update_preferred_no(string cifid, int seq_id)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_update_prefnumber");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = cifid;
					sqcommands.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = seq_id;
					return commons.getDataSet(sqcommands, "CIF_UPDATE_CONTACTS");
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
        public DataSet getdropdown_source(string fldgroup)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;
            try
            {

                if (commons != null)
                {
                    sqcommands = commons.getSqlCommand("sp_cif_dropdown_lookup");
                    sqcommands.Parameters.Add("@fld_group",  SqlDbType.VarChar, 100).Value	= fldgroup; 
                    sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value	= IRSClientData.Location;
					sqcommands.Parameters.Add("@user_role",	 SqlDbType.Int, 100).Value		= IRSClientData.Role;
					
					return commons.getDataSet(sqcommands, "CIF_LOOKUP");
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
		public DataSet getdropdown_source_merchants(string mercifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_merchant");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 16).Value = mercifid;
					return commons.getDataSet(sqcommands, "CIF_MERCHANT");
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
		public DataSet getdropdown_source_bene_accountno(string bene_cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_bene_accountno");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 100).Value = bene_cifid;
					return commons.getDataSet(sqcommands, "CIF_BENE_ACCOUNT");
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
		public DataSet getdropdown_source_bene_gfmcaccountno(string bene_cifid)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_bene_gfmcaccountno");
					sqcommands.Parameters.Add("@CIFID", SqlDbType.VarChar, 100).Value = bene_cifid;
					return commons.getDataSet(sqcommands, "CIF_BENE_ACCOUNT");
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
		public DataSet getdropdown_source_merchants_product()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_merchant_product");
					sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = IRSClientData.Location;
					sqcommands.Parameters.Add("@user_role", SqlDbType.Int, 100).Value = IRSClientData.Role;

					return commons.getDataSet(sqcommands, "CIF_MERCHANT_PRODUCT");
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
		public DataSet getdropdown_source_relationship()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_bene_rel");
					sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = IRSClientData.Location;
					sqcommands.Parameters.Add("@user_role", SqlDbType.Int, 100).Value = IRSClientData.Role;

					return commons.getDataSet(sqcommands, "CIF_BENE_RELATIONSHIP");
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
		public DataSet getdropdown_source_location_code()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_location_code");
					return commons.getDataSet(sqcommands, "CIF_LOCATION_CODE");
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
		public DataSet getdropdown_source_currency()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_currency");
					sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = IRSClientData.Location;
					sqcommands.Parameters.Add("@user_role", SqlDbType.Int, 100).Value = IRSClientData.Role;

					return commons.getDataSet(sqcommands, "CIF_CURRENCY");
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
		public DataSet getdropdown_source_dombankcode()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_dombankcode");
					return commons.getDataSet(sqcommands, "CIF_DOMESTIC_BANKCODE");
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
		public DataSet getdropdown_source_forbankcode()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_forbankcode");
					return commons.getDataSet(sqcommands, "CIF_FOREIGN_BANKCODE");
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
		public DataSet getdropdown_source_pnbbranchcode()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_pnbbranchcode");
					return commons.getDataSet(sqcommands, "CIF_PNB_BRANCHCODE");
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
		public DataSet getdropdown_source_tarcurrency()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_target_curr");
					sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = IRSClientData.Location;
					sqcommands.Parameters.Add("@user_role", SqlDbType.Int, 100).Value = IRSClientData.Role;

					return commons.getDataSet(sqcommands, "CIF_TARCURR");
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
		public DataSet getdropdown_source_product_code()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_bene_productcode");
					sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = IRSClientData.Location;
					sqcommands.Parameters.Add("@user_role", SqlDbType.Int, 100).Value = IRSClientData.Role;

					return commons.getDataSet(sqcommands, "CIF_BENE_PRODUCT_CODE");
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
		public DataSet getdropdown_source_notification_mode()
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;
			try
			{

				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_dropdown_bene_notificationmode");
					sqcommands.Parameters.Add("@branchcode", SqlDbType.VarChar, 4).Value = IRSClientData.Location;
					sqcommands.Parameters.Add("@user_role", SqlDbType.Int, 100).Value = IRSClientData.Role;

					return commons.getDataSet(sqcommands, "CIF_BENE_NOTIFICATION_MODE");
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
		public void set_module_name(string customers_module)
		{
			Session["MODULE_NAME"] = customers_module;
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

			Session["IRS_SEQ_ID"] = seq_id;
			return this.execute_crud("UPDATE");
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool delete_record(string seq_id)
		{
			return this.update_record(seq_id);

		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool rollback_record()
		{
			return this.execute_crud("ROLLBACK");
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool commit_record(string param_values)
		{
            try
            {
                this.parameter_values = param_values; return this.execute_crud("COMMIT");
            }
            catch (Exception e)
            {
                this.set_message("UNABLE TO COMMIT RECORD.\n\n" + e.Message); return false;
            }
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string read_message()
		{
			if (Session["CIF_CLASS_MESSAGE"] != null)
			{
				return (string)Session["CIF_CLASS_MESSAGE"];
			}
			return string.Empty;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public void set_message(string msg)
		{
			Session["CIF_CLASS_MESSAGE"] = null;
			Session["CIF_CLASS_MESSAGE"] = msg;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_seq_id()
		{
			return this.CLIENT_SEQ_ID();
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_curr_seqid()
		{
			sqlHelperWs helper = new sqlHelperWs();

			return helper.IdentityValue.ToString();
		}
		#endregion


		#region PUBLIC AJAX CLIENT MASTER

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_MASTER_RECORD(string column_values)
		{
			irsClientMaster cm = this.CLIENT_MASTER();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (cm.set_record(s) == false)
				{
					this.set_message(cm.messages); return false;
				}

                Session["IRS_CIF_MASTER"] = cm; return true;				
			}

			cm = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_MASTER_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_MASTER_RECORD(string client_seq_id)
		{
            irsClientMaster cm = new irsClientMaster();
			try
			{
				if (cm != null)
				{
					if (client_seq_id == null || client_seq_id.Trim().Length == 0 || client_seq_id.Trim() == string.Empty)
					{
						if (Session["IRS_CIF_MASTER"] != null)
						{
							client_seq_id = ((irsClientMaster)Session["IRS_CIF_MASTER"]).seq_id;
						}
					}

					if (cm.get_record(client_seq_id) == true)
					{
						return cm.getdataRecord();
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
                if (cm != null)
                {
                    cm = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT ACCOUNTS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_ACCOUNTS_RECORD(string column_values)
		{
			irsClientAccounts acc = this.CLIENT_ACCOUNTS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (acc.set_record(s) == false)
				{
					this.set_message(acc.messages); return false;
				}

				Session["IRS_CIF_ACCOUNTS"] = acc;
				return true;
			}

			acc = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_ACCOUNTS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_ACCOUNTS_RECORD(string client_seq_id)
		{
            irsClientAccounts acc = new irsClientAccounts();
			try
			{
				if (acc != null)
				{
					if (acc.get_record(client_seq_id) == true)
					{
						return acc.getdataRecord();
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
                if (acc != null)
                {
                    acc = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT ADDRESS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_ADDRESS_RECORD(string column_values)
		{
			irsClientAddress add = this.CLIENT_ADDRESS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (add.set_record(s) == false)
				{
					this.set_message(add.messages); return false;
				}

				Session["IRS_CIF_ADDRESS"] = add;
				return true;
			}

			add = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_ADDRESS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_ADDRESS_RECORD(string client_seq_id)
		{
            irsClientAddress add = new irsClientAddress();
			try
			{
				if (add != null)
				{
					if (add.get_record(client_seq_id) == true)
					{
						return add.getdataRecord();
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
                if (add != null)
                {
                    add = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT BENEFICIARIES

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_BENEFICIARIES_RECORD(string column_values)
		{
			irsClientBeneficiaries ben = this.CLIENT_BENEFICIARIES();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (ben.set_record(s) == false)
				{
					this.set_message(ben.messages); return false;
				}

				Session["IRS_CIF_BENEFICIARIES"] = ben;
				return true;
			}

			ben = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_BENEFICIARIES_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_BENEFICIARIES_RECORD(string client_seq_id)
		{
            irsClientBeneficiaries ben = new irsClientBeneficiaries();
			try
			{
				if (ben != null)
				{
					if (ben.get_record(client_seq_id) == true)
					{
						return ben.getdataRecord();
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
                if (ben != null)
                {
                    ben = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT CARDS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_CARDS_RECORD(string column_values)
		{
			irsClientCards car = this.CLIENT_CARDS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (car.set_record(s) == false)
				{
					this.set_message(car.messages); return false;
				}

				Session["IRS_CIF_CARDS"] = car;
				return true;
			}

			car = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_CARDS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_CARDS_RECORD(string client_seq_id)
		{
			irsClientCards car = this.CLIENT_CARDS();
			try
			{
				if (car != null)
				{
					if (car.get_record(client_seq_id) == true)
					{
						return car.getdataRecord();
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
                if (car != null)
                {
                    car = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT CONTACTS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_CONTACTS_RECORD(string column_values)
		{
			irsClientContacts con = this.CLIENT_CONTACTS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (con.set_record(s) == false)
				{
					this.set_message(con.messages); return false;
				}

				Session["IRS_CIF_CONTACTS"] = con;
				return true;
			}

			con = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_CONTACTS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_CONTACTS_RECORD(string client_seq_id)
		{
            irsClientContacts con = new irsClientContacts();
			try
			{
				if (con != null)
				{
					if (con.get_record(client_seq_id) == true)
					{
						return con.getdataRecord();
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
                if (con != null)
                {
                    con = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT IDS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_IDS_RECORD(string column_values)
		{
			irsClientIds ids = this.CLIENT_IDS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (ids.set_record(s) == false)
				{
					this.set_message(ids.messages); return false;
				}

				Session["IRS_CIF_IDS"] = ids;
				return true;
			}

			ids = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_IDS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public bool SET_VAN_RECORD(string column_values)
        {
            irsClientVan van = this.CLIENT_VAN();

            string[] s = column_values.Split('|');

            if (s != null && s.Length > 0)
            {
                if (van.set_record(s) == false)
                {
                    this.set_message(van.messages); return false;
                }

                Session["IRS_CIF_VAN"] = van;
                return true;
            }

            van = null;

            this.set_message("UNABLE TO EXECUTE METHOD: SET_VAN_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
        }

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_IDS_RECORD(string client_seq_id)
		{
            irsClientIds ids = new irsClientIds();
			try
			{
				if (ids != null)
				{
					if (ids.get_record(client_seq_id) == true)
					{						
						return ids.getdataRecord();
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
                if (ids != null)
                {
                    ids = null;
                }
			}
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public DataSet GET_VAN_RECORD(string client_seq_id)
        {
            irsClientVan van = new irsClientVan();
            try
            {
                if (van != null)
                {
                    if (van.get_record(client_seq_id) == true)
                    {
                        return van.getdataRecord();
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
                if (van != null)
                {
                    van = null;
                }
            }
        }

		#endregion


		#region PUBLIC AJAX CLIENT MERCHANT

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_MERCHANT_RECORD(string column_values)
		{
			irsClientMerchant merc = this.CLIENT_MERCHANT();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (merc.set_record(s) == false)
				{
					this.set_message(merc.messages); return false;
				}

				Session["IRS_CIF_MERCHANT"] = merc;
				return true;
			}

			merc = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_MERCHANT_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_MERCHANT_RECORD(string client_seq_id)
		{
			irsClientMerchant merc = new irsClientMerchant();
			try
			{
				if (merc != null)
				{
					if (merc.get_record(client_seq_id) == true)
					{
						return merc.getdataRecord();
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
				if (merc != null)
				{
					merc = null;
				}
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT MERCHANTS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_MERCHANTS_RECORD(string column_values)
		{
			irsClientMerchants mer = this.CLIENT_MERCHANTS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (mer.set_record(s) == false)
				{
					this.set_message(mer.messages); return false;
				}

				Session["IRS_CIF_MERCHANTS"] = mer;
				return true;
			}

			mer = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_MERCHANTS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_MERCHANTS_RECORD(string client_seq_id)
		{
            irsClientMerchants mer = new irsClientMerchants();
			try
			{
				if (mer != null)
				{
					if (mer.get_record(client_seq_id) == true)
					{
						return mer.getdataRecord();
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
                if (mer != null)
                {
                    mer = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT OCCUPATIONS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_OCCUPATIONS_RECORD(string column_values)
		{
			irsClientOccupations occ = this.CLIENT_OCCUPATIONS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (occ.set_record(s) == false)
				{
					this.set_message(occ.messages); return false;
				}

				Session["IRS_CIF_OCCUPATIONS"] = occ;
				return true;
			}

			occ = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_OCCUPATIONS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_OCCUPATIONS_RECORD(string client_seq_id)
		{
            irsClientOccupations occ = new irsClientOccupations();
			try
			{
				if (occ != null)
				{
					if (occ.get_record(client_seq_id) == true)
					{
						return occ.getdataRecord();
					}
				}
				return null;
			}
            catch //(Exception e) // commented to remove warning
			{
				return null;
			}
			finally
			{
                if (occ != null)
                {
                    occ = null;
                }
			}
		}

		#endregion

            
		#region PUBLIC AJAX CLIENT REMITTERS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string add_account_curr(string plist)
		{
			string errmsg = null;
			string acc_seqid = null;
			irsClientEnrollment enr_agt = new irsClientEnrollment();

			acc_seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.ADD_ACCOUNT, plist, out errmsg);

			if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
			else return acc_seqid;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string update_account_curr(string plist)
		{
			string errmsg = null;
			string acc_seqid = null;
			irsClientEnrollment enr_agt = new irsClientEnrollment();

			acc_seqid = enr_agt.enroll_agent(irsClientEnrollment.Function_Name.UPDATE_ACCT, plist, out errmsg);

			if (errmsg != String.Empty && errmsg != null && errmsg != "") return errmsg;
			else return acc_seqid;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GET_CIF_MASTER_CIFID(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();

			SqlCommand command = helper.getSqlCommand("sp_cif_get_cifmaster");
			command.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = SEQ_ID;
			DataTable dt =  helper.getDataSet(command, "TBL_NONE").Tables[0];

			if (dt != null)
			{
				return Convert.ToString( dt.Rows[0]["CIFID"]);
			}

			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GET_CIF_MASTER_CIFTYPE(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();			

			SqlCommand command = helper.getSqlCommand("SELECT * FROM TBL_CIF_MASTER A LEFT JOIN TBL_CIF_BENEFICIARIES B ON B.CIFID = A.CIFID WHERE B.SEQ_ID = " + SEQ_ID, false);
			DataTable dt = helper.getDataSet(command, "TBL_MASTER_CIFTYPE").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["CIFTYPE"]);
			}

			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_bene_accountno(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();

			SqlCommand command = helper.getSqlCommand("sp_cif_get_bene_accountseqid");
			command.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = SEQ_ID;
			DataTable dt = helper.getDataSet(command, "TBL_BENE_ACCOUNTNO").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["ACCOUNTSEQID"]);
			}

			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_bene_accountcurr(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();

			SqlCommand command = helper.getSqlCommand("sp_cif_dropdown_get_targetcurrency");
			command.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = SEQ_ID;
			DataTable dt = helper.getDataSet(command, "TBL_BENE_ACCOUNTCURR").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["Currency"]);
			}

			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_gfmc_accountseqid(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();

			SqlCommand command = helper.getSqlCommand("sp_cif_get_gfmcaccount_seqid");
			command.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = SEQ_ID;
			DataTable dt = helper.getDataSet(command, "TBL_ACCOUNTSEQID").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["ACCOUNTSEQID"]);
			}

			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string get_bene_cifid(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();

			SqlCommand command = helper.getSqlCommand("sp_cif_get_bene_cifid");
			command.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = SEQ_ID;
			DataTable dt = helper.getDataSet(command, "TBL_BENE_CIFID").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["CIFID"]);
			}

			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GET_CIF_MASTER_CIVILSTATUS(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();
			
			SqlCommand command = helper.getSqlCommand("sp_cif_get_cifmaster");
			command.Parameters.Add("@SEQ_ID", SqlDbType.BigInt, 100).Value = SEQ_ID;
			DataTable dt = helper.getDataSet(command, "CIF_MASTER_CIVILSTATUS").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["CIVILSTATUS"]);
			}

			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GET_CIF_SPOUSE_CIFID(int SEQ_ID)
		{
			sqlHelperWs helper = new sqlHelperWs();

			SqlCommand command = helper.getSqlCommand("sp_cif_get_cifspouse");
			command.Parameters.Add("@SEQ_ID", SqlDbType.Int, 100).Value = SEQ_ID;
			DataTable dt = helper.getDataSet(command, "TBL_SPOUSE_CIFID").Tables[0];

			if (dt != null)
			{
				return Convert.ToString(dt.Rows[0]["BENECIFID"]);
			}

			return "0";
		}
		
		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_REMITTERS_RECORD(string column_values)
		{
			irsClientRemitters rem = this.CLIENT_REMITTERS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (rem.set_record(s) == false)
				{
					this.set_message(rem.messages); return false;
				}

				Session["IRS_CIF_REMITTERS"] = rem;
				return true;
			}

			rem = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_REMITTERS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public string GET_REMITTERS_SEQ_ID(string CIFID)
		{
			irsClientRemitters rem = new irsClientRemitters();

			DataTable dt = rem.get_rem_info(CIFID);

			if (dt != null)
			{
				if (dt.Rows.Count > 0)
				{
					return Convert.ToString(dt.Rows[0]["SEQ_ID"]); 
				}
			}
			return "0";
		}

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_REMITTERS_RECORD(string client_seq_id)
		{
            irsClientRemitters rem = new irsClientRemitters();
			try
			{
				if (rem != null)
				{
					if (rem.get_record(client_seq_id) == true)
					{
						return rem.getdataRecord();
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
                if (rem != null)
                {
                    rem = null;
                }
			}
		}

		#endregion


		#region PUBLIC AJAX CLIENT CLASSIFICATIONS

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_CLASSIFICATIONS_RECORD(string column_values)
		{
			irsClientClassifications cl = this.CLIENT_CLASSIFICATIONS();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (cl.set_record(s) == false)
				{
					this.set_message(cl.messages); return false;
				}

				Session["IRS_CIF_CLASSIFICATIONS"] = cl;
				return true;
			}

			cl = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_CLASSIFICATIONS_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}

		#endregion


		#region PUBLIC AJAX CLIENT SPOUSE

		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public bool SET_SPOUSE_RECORD(string column_values)
		{
			irsClientSpouse csp = this.CLIENT_SPOUSE();

			string[] s = column_values.Split('|');

			if (s != null && s.Length > 0)
			{
				if (csp.set_record(s) == false)
				{
					this.set_message(csp.messages); return false;
				}

				Session["IRS_CIF_SPOUSE"] = csp;
				return true;
			}

			csp = null;

			this.set_message("UNABLE TO EXECUTE METHOD: SET_SPOUSE_RECORD(). PARAM VALUE NOT SPECIFIED."); return false;
		}


		[AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
		public DataSet GET_SPOUSE_RECORD(string client_seq_id)
		{
			irsClientSpouse csp = new irsClientSpouse();
			try
			{
				if (csp != null)
				{
					if (csp.get_record(client_seq_id) == true)
					{
						return csp.getdataRecord();
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
				if (csp != null)
				{
					csp = null;
				}
			}
		}

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public String INSERT_CIF_TO_FC(String client_CIF_id)
        {
            irsAccountInquiry irsFC = new irsAccountInquiry();
            try
            {
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
                        String CustomerID = FCResArr[0].Split('=')[1].ToString().Trim();

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
                return "00|Successfuly saved CIF to FC";
            }
            catch(Exception ex)
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

        private bool check_cif_if_exist(String CIFID)
        {
            sqlHelperWs helper = new sqlHelperWs();
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
            sqlHelperWs helper = new sqlHelperWs();
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

		
		#region PRIVATE METHOD CLIENT_MASTER
               
		private irsClientMaster CLIENT_MASTER()
		{
			if (Session["IRS_CIF_MASTER"] == null)
			{
				Session["IRS_CIF_MASTER"] = new irsClientMaster();
			}

			return (irsClientMaster)Session["IRS_CIF_MASTER"];
		}

		private bool do_cif_master(string instruction)
		{
			irsClientMaster cm = this.CLIENT_MASTER();
			
			try
			{
				if (instruction != null && instruction.Trim().Length > 0)
				{
					if (instruction == "INSERT")
					{
						try
						{
							cm.seq_id = null;
                            cm.INSERT_FLAG(true);
							cm.selectedRow = -1;                            
                            
							if (cm.insert_record() == false)
							{
								this.set_message(cm.messages); return false;
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
							cm.INSERT_FLAG(false);
                            cm.seq_id = this.get_seq_id();
                            cm.selectedRow = 0;
							
                            if (cm.seq_id == null || cm.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

							if (cm.update_record() == false)
							{
								this.set_message(cm.messages); return false;
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
							if (cm.mas_ins_flag == true)
							{
								if (cm.is_existing(this.parameter_values) == true)
								{
									cm.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(cm.seq_id, irsClientEnrollment.VAPTTableName.MASTER, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (this.SET_MASTER_RECORD(this.parameter_values) == false)
							{
								cm.seq_id = null;
                                return false;
                            }

                            /* RFC-31620 */
                            if (new irsUsers().adminApproverSwitch())
                            {
                                Session["AdminApprovalFlagCIFMessage"] = null;
                                string action = "RECORD SUBMITTED FOR APPROVAL";
                                DataTable dt = new DataTable();
                                DataRow dr;
                                string cifId = string.Empty;

                                dt.Columns.Add("Main_SEQ_ID");
                                dt.Columns.Add("CIFID");
                                dt.Columns.Add("CIFType");
                                dt.Columns.Add("LastName");
                                dt.Columns.Add("FirstName");
                                dt.Columns.Add("MiddleName");
                                dt.Columns.Add("Gender");
                                dt.Columns.Add("Age");
                                dt.Columns.Add("CivilStatus");
                                dt.Columns.Add("Citizenship");
                                dt.Columns.Add("BirthDate");
                                dt.Columns.Add("BirthPlace");
                                dt.Columns.Add("Remarks");
                                dt.Columns.Add("DateCreated");
                                dt.Columns.Add("DateModified");
                                dt.Columns.Add("Alias");
                                dt.Columns.Add("BranchCode");
                                dt.Columns.Add("Religion");
                                dt.Columns.Add("Title");
                                dt.Columns.Add("CreatedBy");
                                dt.Columns.Add("ModifiedBy");
                                dt.Columns.Add("FullName");
                                dt.Columns.Add("CIFClass");
                                dt.Columns.Add("Deleted");
                                dr = dt.NewRow();

                                //DELETE
                                if (this.parameter_values.ToLower().IndexOf("deleted~true") >= 0)
                                {
                                    DataTable dtCIF = new irsUsers().GetCIFInformation(cm.seq_id);
                                    if (dtCIF != null && dtCIF.Rows.Count > 0)
                                    {
                                        cifId = dtCIF.Rows[0]["CIFID"].ToString();
                                        if (new irsUsers().IsMakerAllowedCIFMaster(irsUsers.ActionType.DELETE, cifId))
                                        {
                                            throw new Exception("There is a pending approval for the same cif. Submission for deletion to checker failed.");
                                        }

                                        dr[0] = cm.seq_id; //Main_SEQ_ID
                                        dr[1] = cifId; //CIFID
                                        dr[2] = dtCIF.Rows[0]["CIFType"]; //CIFType
                                        dr[3] = dtCIF.Rows[0]["LastName"]; //LastName
                                        dr[4] = dtCIF.Rows[0]["FirstName"]; //FirstName
                                        dr[5] = dtCIF.Rows[0]["MiddleName"]; //MiddleName
                                        dr[6] = dtCIF.Rows[0]["Gender"]; //Gender
                                        dr[7] = dtCIF.Rows[0]["Age"]; //Age
                                        dr[8] = dtCIF.Rows[0]["CivilStatus"]; //CivilStatus
                                        dr[9] = dtCIF.Rows[0]["Citizenship"]; //Citizenship
                                        dr[10] = dtCIF.Rows[0]["BirthDate"]; //BirthDate
                                        dr[11] = dtCIF.Rows[0]["BirthPlace"]; //BirthPlace
                                        dr[12] = dtCIF.Rows[0]["Remarks"]; //Remarks
                                        dr[13] = dtCIF.Rows[0]["DateCreated"]; //DateCreated
                                        dr[14] = dtCIF.Rows[0]["DateModified"]; //DateModified
                                        dr[15] = dtCIF.Rows[0]["Alias"]; //Alias
                                        dr[16] = dtCIF.Rows[0]["BranchCode"]; //BranchCode
                                        dr[17] = dtCIF.Rows[0]["Religion"]; //Religion
                                        dr[18] = dtCIF.Rows[0]["Title"]; //Title
                                        dr[19] = dtCIF.Rows[0]["CreatedBy"]; //CreatedBy
                                        dr[20] = dtCIF.Rows[0]["ModifiedBy"]; //ModifiedBy
                                        dr[21] = dtCIF.Rows[0]["FullName"]; //FullName
                                        dr[22] = dtCIF.Rows[0]["CIFClass"]; //CIFClass
                                        dr[23] = true; //Deleted
                                        dt.Rows.Add(dr);

                                        if (!new irsUsers().saveMasterMakerChecker(irsUsers.ActionType.DELETE, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [delete] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        this.set_message("Maker Checker saving [delete] failed");
                                        return false;
                                    }
                                }
                                else
                                {
                                    string[] arrCIFMaster = this.parameter_values.Split('|');
                                    cifId = arrCIFMaster.GetValue(0).ToString().Split('~').GetValue(1).ToString();

                                    dr[0] = cm.seq_id; //Main_SEQ_ID
                                    dr[1] = cifId; //CIFID
                                    dr[2] = arrCIFMaster.GetValue(1).ToString().Split('~').GetValue(1); //CIFType
                                    dr[3] = arrCIFMaster.GetValue(2).ToString().Split('~').GetValue(1); //LastName
                                    dr[4] = arrCIFMaster.GetValue(3).ToString().Split('~').GetValue(1); //FirstName
                                    dr[5] = arrCIFMaster.GetValue(4).ToString().Split('~').GetValue(1); //MiddleName
                                    dr[6] = arrCIFMaster.GetValue(5).ToString().Split('~').GetValue(1); //Gender
                                    dr[7] = arrCIFMaster.GetValue(6).ToString().Split('~').GetValue(1); //Age
                                    dr[8] = arrCIFMaster.GetValue(7).ToString().Split('~').GetValue(1); //CivilStatus
                                    dr[9] = arrCIFMaster.GetValue(8).ToString().Split('~').GetValue(1); //Citizenship
                                    dr[10] = arrCIFMaster.GetValue(9).ToString().Split('~').GetValue(1); //BirthDate
                                    dr[11] = arrCIFMaster.GetValue(10).ToString().Split('~').GetValue(1); //BirthPlace
                                    dr[12] = arrCIFMaster.GetValue(11).ToString().Split('~').GetValue(1); //Remarks
                                    dr[13] = arrCIFMaster.GetValue(12).ToString().Split('~').GetValue(1); //DateCreated
                                    dr[14] = arrCIFMaster.GetValue(13).ToString().Split('~').GetValue(1); //DateModified
                                    dr[15] = arrCIFMaster.GetValue(14).ToString().Split('~').GetValue(1); //Alias
                                    dr[16] = arrCIFMaster.GetValue(15).ToString().Split('~').GetValue(1); //BranchCode
                                    dr[17] = arrCIFMaster.GetValue(16).ToString().Split('~').GetValue(1); //Religion
                                    dr[18] = arrCIFMaster.GetValue(17).ToString().Split('~').GetValue(1); //Title
                                    dr[19] = arrCIFMaster.GetValue(18).ToString().Split('~').GetValue(1); //CreatedBy
                                    dr[20] = arrCIFMaster.GetValue(19).ToString().Split('~').GetValue(1); //ModifiedBy
                                    dr[21] = arrCIFMaster.GetValue(20).ToString().Split('~').GetValue(1); //FullName
                                    dr[22] = arrCIFMaster.GetValue(21).ToString().Split('~').GetValue(1); //CIFClass
                                    dr[23] = arrCIFMaster.GetValue(22).ToString().Split('~').GetValue(1); //Deleted
                                    dt.Rows.Add(dr);

                                    if (cm.mas_ins_flag) //ADD
                                    {
                                        if (!new irsUsers().saveMasterMakerChecker(irsUsers.ActionType.ADD, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [add] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (new irsUsers().IsMakerAllowedCIFMaster(irsUsers.ActionType.EDIT, cifId))
                                        {
                                            throw new Exception("There is a pending approval for the same cif. Submission for edit to checker failed.");
                                        }
                                        if (!new irsUsers().saveMasterMakerChecker(irsUsers.ActionType.EDIT, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [edit] failed");
                                            return false;
                                        }
                                    }
                                }
                                Session["AdminApprovalFlagCIFMessage"] = action;

                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF RECORD (MAKER/CHECKER)", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF RECORD (MAKER/CHECKER)", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
                                return true;
                            }
                            else
                            {
                                if (cm.commit_record() == false)
                                {
                                    cm.seq_id = null;
                                    this.set_message(cm.messages); return false;
                                }
                                else
                                {
                                    /* VAPT 20180222 */
                                    if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF RECORD", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                    else
                                    {
                                        if (arrCIF.GetValue(1).ToString().Length > 0)
                                        {
                                            new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF RECORD", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                        }
                                    }
                                }
                            }

                            if (cm.mas_ins_flag == true)
							{
								cm.insert_cif_class(cm.cifclass);
								cm.insert_to_other_ids();
							}
							return true;
							/*else
							{
								this.insert_other_ids(cm.dummy_cifid);
								return true;
							}*/
						}
						catch (Exception e)
						{
                            /* RFC-31620 */
                            Session["AdminApprovalFlagCIFMessage"] = e.Message;
							this.set_message(e.Message); return false;                            
						}
					}

					if (instruction == "ROLLBACK")
					{
						try
						{
							cm.rollback_record(); return true;
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
                Session["IRS_CIF_MASTER"] = null;
				Session["IRS_CIF_MASTER"] = cm;
                Session["IRS_SEQ_ID"]     = null;
				Session["IRS_SEQ_ID"]     = cm.seq_id;
				cm = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
		}

		#endregion

		
		#region PRIVATE METHOD CLIENT_IDS

		private irsClientIds CLIENT_IDS()
		{
			if (Session["IRS_CIF_IDS"] == null)
			{
				Session["IRS_CIF_IDS"] = new irsClientIds();
			}

			return (irsClientIds)Session["IRS_CIF_IDS"];
		}


		private bool do_cif_id(string instruction)
		{
			irsClientIds cid = this.CLIENT_IDS();

			try
			{
				if (instruction != null && instruction.Trim().Length > 0)
				{
					if (instruction == "INSERT")
					{
						try
						{
							cid.seq_id = null;
                            cid.selectedRow = -1;

							if (cid.insert_record() == false)
							{
								this.set_message(cid.messages); return false;
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

                            cid.seq_id = this.get_seq_id();
                            cid.selectedRow = 0;

                            if (cid.seq_id == null || cid.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

							if (cid.update_record() == false)
							{
								this.set_message(cid.messages); return false;
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
							if (cid.ins_flag == true)
							{
								if (cid.is_existing(this.parameter_values) == true)
								{
									cid.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}

								if (cid.is_existing_idtype(this.parameter_values) == true)
								{
									cid.seq_id = null;
									this.set_message("There should be no record with the same ID Type!");
									return false;
								}
							}
												
							if (this.SET_IDS_RECORD(this.parameter_values) == false)
							{
								cid.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(cid.seq_id, irsClientEnrollment.VAPTTableName.IDS, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (cid.commit_record() == false)
							{
								cid.seq_id = null;
								this.set_message(cid.messages); return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF IDS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF IDS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
							cid.rollback_record(); return true;
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
				Session["IRS_CIF_IDS"] = null;
				Session["IRS_CIF_IDS"] = cid;
                Session["IRS_SEQ_ID"]  = null;
				Session["IRS_SEQ_ID"]  = cid.seq_id;
				cid = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
		}

		#endregion

        #region PRIVATE METHOD CLIENT_VAN

        private irsClientVan CLIENT_VAN()
        {
            if (Session["IRS_CIF_VAN"] == null)
            {
                Session["IRS_CIF_VAN"] = new irsClientVan();
            }

            return (irsClientVan)Session["IRS_CIF_VAN"];
        }


        private bool do_cif_van(string instruction)
        {
            irsClientVan cvan = this.CLIENT_VAN();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
                            cvan.seq_id = null;
                            cvan.selectedRow = -1;

                            if (cvan.insert_record() == false)
                            {
                                this.set_message(cvan.messages); return false;
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

                            cvan.seq_id = this.get_seq_id();
                            cvan.selectedRow = 0;

                            if (cvan.seq_id == null || cvan.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

                            if (cvan.update_record() == false)
                            {
                                this.set_message(cvan.messages); return false;
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
                            if (cvan.ins_flag == true)
                            {
                                if (cvan.is_existing(this.parameter_values) == true)
                                {
                                    cvan.seq_id = null;
                                    this.set_message("RECORD ALREADY EXISTS!");
                                    return false;
                                }
                            }

                            if (this.SET_VAN_RECORD(this.parameter_values) == false)
                            {
                                cvan.seq_id = null;
                                return false;
                            }

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(cvan.seq_id, irsClientEnrollment.VAPTTableName.VAN, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (cvan.commit_record() == false)
                            {
                                cvan.seq_id = null;
                                this.set_message(cvan.messages); return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF VAN", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF VAN", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
                            cvan.rollback_record(); return true;
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
                Session["IRS_CIF_VAN"] = null;
                Session["IRS_CIF_VAN"] = cvan;
                Session["IRS_SEQ_ID"] = null;
                Session["IRS_SEQ_ID"] = cvan.seq_id;
                cvan = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion
		
        #region PRIVATE METHOD CLIENT_ACCOUNTS

        private irsClientAccounts CLIENT_ACCOUNTS()
        {
            if (Session["IRS_CIF_ACCOUNTS"] == null)
            {
                Session["IRS_CIF_ACCOUNTS"] = new irsClientAccounts();
            }

            return (irsClientAccounts)Session["IRS_CIF_ACCOUNTS"];
        }

        private bool do_cif_accounts(string instruction)
        {
            irsClientAccounts ca = this.CLIENT_ACCOUNTS();
			
            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
							ca.seq_id = null;
							ca.INSERT_FLAG(true);
                            ca.selectedRow = -1;
							
                            if (ca.insert_record() == false)
                            {
                                this.set_message(ca.messages); return false;
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
							ca.INSERT_FLAG(false);
                            ca.seq_id = this.get_seq_id();
                            ca.selectedRow = 0;

                            if (ca.seq_id == null || ca.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

                            if (ca.update_record() == false)
                            {
                                this.set_message(ca.messages); return false;
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
							if (ca.ins_flag == true)
							{
								if (ca.is_existing(this.parameter_values) == true)
								{
									this.set_message("RECORD ALREADY EXISTS!");
									ca.seq_id = null;
									return false;
								}
							}							
							
							if (this.SET_ACCOUNTS_RECORD(this.parameter_values) == false)
							{
								ca.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(ca.seq_id, irsClientEnrollment.VAPTTableName.ACCOUNTS, arrParameters);
                            arrCIF = CIF.Split('^');

                            /* RFC-31620 */
                            if (new irsUsers().adminApproverSwitch())
                            {
                                Session["AdminApprovalFlagCIFMessage"] = null;
                                string action = "RECORD SUBMITTED FOR APPROVAL";
                                DataTable dt = new DataTable();
                                DataRow dr;
                                string cifId = string.Empty;

                                dt.Columns.Add("Main_SEQ_ID");
                                dt.Columns.Add("CIFID");
                                dt.Columns.Add("AccountNo");
                                dt.Columns.Add("AccountType");
                                dt.Columns.Add("Currency");
                                dt.Columns.Add("BranchCode");
                                dt.Columns.Add("DomBankCode");
                                dt.Columns.Add("Remarks");
                                dt.Columns.Add("Status");
                                dt.Columns.Add("KBIAccount");
                                dt.Columns.Add("OtherBankSeqId");
                                dt.Columns.Add("BranchType");
                                dt.Columns.Add("Deleted");
                                dr = dt.NewRow();

                                //DELETE
                                if (this.parameter_values.ToLower().IndexOf("deleted~true") >= 0)
                                {
                                    DataTable dtCIFAccounts = new irsUsers().GetCIFAccounts(ca.seq_id);
                                    if (dtCIFAccounts != null && dtCIFAccounts.Rows.Count > 0)
                                    {
                                        cifId = dtCIFAccounts.Rows[0]["CIFID"].ToString();
                                        if (new irsUsers().IsMakerAllowedCIFContacts(irsUsers.ActionType.DELETE, ca.seq_id))
                                        {
                                            throw new Exception("There is a pending approval for the same cif. Submission for deletion to checker failed.");
                                        }

                                        dr[0] = ca.seq_id; //Main_SEQ_ID
                                        dr[1] = cifId; //CIFID
                                        dr[2] = dtCIFAccounts.Rows[0]["AccountNo"]; //AccountNo
                                        dr[3] = dtCIFAccounts.Rows[0]["AccountType"]; //AccountType
                                        dr[4] = dtCIFAccounts.Rows[0]["Currency"]; //Currency
                                        dr[5] = dtCIFAccounts.Rows[0]["BranchCode"]; //BranchCode
                                        dr[6] = dtCIFAccounts.Rows[0]["DomBankCode"]; //DomBankCode
                                        dr[7] = dtCIFAccounts.Rows[0]["Remarks"]; //Remarks
                                        dr[8] = dtCIFAccounts.Rows[0]["Status"]; //Status
                                        dr[9] = dtCIFAccounts.Rows[0]["KBIAccount"]; //KBIAccount
                                        dr[10] = dtCIFAccounts.Rows[0]["OtherBankSeqId"]; //OtherBankSeqId
                                        dr[11] = dtCIFAccounts.Rows[0]["BranchType"]; //BranchType
                                        dr[12] = true; //Deleted
                                        dt.Rows.Add(dr);

                                        if (!new irsUsers().saveAccountsMakerChecker(irsUsers.ActionType.DELETE, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [delete] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        this.set_message("Maker Checker saving [delete] failed");
                                        return false;
                                    }
                                }
                                else
                                {
                                    string[] arrCIFAccounts = this.parameter_values.Split('|');
                                    cifId = arrCIFAccounts.GetValue(0).ToString().Split('~').GetValue(1).ToString();

                                    dr[0] = ca.seq_id; //Main_SEQ_ID
                                    dr[1] = cifId; //CIFID
                                    dr[2] = arrCIFAccounts.GetValue(1).ToString().Split('~').GetValue(1); //AccountNo
                                    dr[3] = arrCIFAccounts.GetValue(2).ToString().Split('~').GetValue(1); //AccountType
                                    dr[4] = arrCIFAccounts.GetValue(3).ToString().Split('~').GetValue(1); //Currency
                                    dr[5] = arrCIFAccounts.GetValue(4).ToString().Split('~').GetValue(1); //BranchCode
                                    dr[6] = arrCIFAccounts.GetValue(5).ToString().Split('~').GetValue(1); //DomBankCode
                                    dr[7] = arrCIFAccounts.GetValue(6).ToString().Split('~').GetValue(1); //Remarks
                                    dr[8] = arrCIFAccounts.GetValue(7).ToString().Split('~').GetValue(1); //Status
                                    dr[9] = arrCIFAccounts.GetValue(8).ToString().Split('~').GetValue(1); //KBIAccount
                                    dr[10] = arrCIFAccounts.GetValue(9).ToString().Split('~').GetValue(1); //OtherBankSeqId
                                    dr[11] = arrCIFAccounts.GetValue(10).ToString().Split('~').GetValue(1); //BranchType
                                    dr[12] = arrCIFAccounts.GetValue(11).ToString().Split('~').GetValue(1); //Deleted
                                    dt.Rows.Add(dr);

                                    if (ca.ins_flag) //ADD
                                    {
                                        if (!new irsUsers().saveAccountsMakerChecker(irsUsers.ActionType.ADD, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [add] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (new irsUsers().IsMakerAllowedCIFAccounts(irsUsers.ActionType.EDIT, ca.seq_id))
                                        {
                                            throw new Exception("There is a pending approval for the same cif. Submission for edit to checker failed.");
                                        }
                                        if (!new irsUsers().saveAccountsMakerChecker(irsUsers.ActionType.EDIT, dt, ca.seq_id))
                                        {
                                            this.set_message("Maker Checker saving [edit] failed");
                                            return false;
                                        }
                                    }
                                }
                                Session["AdminApprovalFlagCIFMessage"] = action;

                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF ACCOUNTS (MAKER/CHECKER)", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF ACCOUNTS (MAKER/CHECKER)", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
                                return true;
                            }
                            else
                            {
                                if (ca.commit_record() == false)
                                {
                                    ca.seq_id = null;
                                    this.set_message(ca.messages); return false;
                                }
                                else
                                {
                                    /* VAPT 20180222 */
                                    if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF ACCOUNTS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                    else
                                    {
                                        if (arrCIF.GetValue(1).ToString().Length > 0)
                                        {
                                            new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF ACCOUNTS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                        }
                                    }
                                }
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
                            ca.rollback_record(); return true;
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
				Session["IRS_CIF_ACCOUNTS"] = null;
				Session["IRS_CIF_ACCOUNTS"] = ca;
                Session["IRS_SEQ_ID"] = null;
				Session["IRS_SEQ_ID"] = ca.seq_id;
				ca = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion


        #region PRIVATE METHOD CLIENT_ADDRESS

        private irsClientAddress CLIENT_ADDRESS()
        {
            if (Session["IRS_CIF_ADDRESS"] == null)
            {
                Session["IRS_CIF_ADDRESS"] = new irsClientAddress();
            }

            return (irsClientAddress)Session["IRS_CIF_ADDRESS"];
        }

		

        private bool do_cif_address(string instruction)
        {
            irsClientAddress cad = this.CLIENT_ADDRESS();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
							cad.seq_id = null;
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
                            cad.seq_id = this.get_seq_id();
                            cad.selectedRow = 0;

                            if (cad.seq_id == null || cad.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

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
							if (cad.ins_flag == true)
							{
								if (cad.is_existing(this.parameter_values) == true)
								{
									cad.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}
							}							

							if (this.SET_ADDRESS_RECORD(this.parameter_values) == false)
							{
								cad.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(cad.seq_id, irsClientEnrollment.VAPTTableName.ADDRESS, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (cad.commit_record() == false)
                            {
                                cad.seq_id = null;
                                this.set_message(cad.messages); return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF ADDRESS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF ADDRESS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
				Session["IRS_CIF_ADDRESS"] = null;
				Session["IRS_CIF_ADDRESS"] = cad;
                Session["IRS_SEQ_ID"]      = null;
				Session["IRS_SEQ_ID"]      = cad.seq_id;
				cad = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

         #endregion


        #region PRIVATE METHOD CLIENT_BENEFICIARIES

        private irsClientBeneficiaries CLIENT_BENEFICIARIES()
        {
            if (Session["IRS_CIF_BENEFICIARIES"] == null)
            {
                Session["IRS_CIF_BENEFICIARIES"] = new irsClientBeneficiaries();
            }

            return (irsClientBeneficiaries)Session["IRS_CIF_BENEFICIARIES"];
        }

        private bool do_cif_beneficiaries(string instruction)
        {
            irsClientBeneficiaries cb = this.CLIENT_BENEFICIARIES();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
							cb.seq_id = null;
                            cb.selectedRow = -1;

                            if (cb.insert_record() == false)
                            {
                                this.set_message(cb.messages); return false;
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
                            cb.seq_id = this.get_seq_id();
                            cb.selectedRow = 0;

                            if (cb.seq_id == null || cb.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

                            if (cb.update_record() == false)
                            {
                                this.set_message(cb.messages); return false;
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
							if (cb.ins_flag == true)
							{
								if (cb.is_existing(1, this.parameter_values) == true)
								{
									cb.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}
							}
							else
							{
								if (cb.is_existing(0, this.parameter_values) == true)
								{
									cb.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}
							}

							if (this.SET_BENEFICIARIES_RECORD(this.parameter_values) == false)
							{
								cb.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(cb.seq_id, irsClientEnrollment.VAPTTableName.BENEFICIARY, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (cb.commit_record() == false)
							{
								cb.seq_id = null;
								this.set_message(cb.messages); return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF BENEFICIARIES", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF BENEFICIARIES", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
                            cb.rollback_record(); return true;
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
				Session["IRS_CIF_BENEFICIARIES"] = null;
				Session["IRS_CIF_BENEFICIARIES"] = cb;
                Session["IRS_SEQ_ID"]            = null;
				Session["IRS_SEQ_ID"]            = cb.seq_id;
				cb = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion


        #region PRIVATE METHOD CLIENT_CARDS

        private irsClientCards CLIENT_CARDS()
        {
            if (Session["IRS_CIF_CARDS"] == null)
            {
                Session["IRS_CIF_CARDS"] = new irsClientCards();
            }

            return (irsClientCards)Session["IRS_CIF_CARDS"];
        }

        private bool do_cif_cards(string instruction)
        {
            irsClientCards car = this.CLIENT_CARDS();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
							car.seq_id = null;
                            car.selectedRow = -1;

                            if (car.insert_record() == false)
                            {
                                this.set_message(car.messages); return false;
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
                            car.seq_id = this.get_seq_id();
                            car.selectedRow = 0;

                            if (car.seq_id == null || car.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

                            if (car.update_record() == false)
                            {
                                this.set_message(car.messages); return false;
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
							if (car.ins_flag == true)
							{
								if (car.is_existing(this.parameter_values) == true)
								{
									this.set_message("RECORD ALREADY EXISTS!");
									car.seq_id = null;
									return false;
								}
							}							
							
							if (this.SET_CARDS_RECORD(this.parameter_values) == false)
							{
								car.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(car.seq_id, irsClientEnrollment.VAPTTableName.CARDS, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (car.commit_record() == false)
                            {
                                car.seq_id = null;
                                this.set_message(car.messages); return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CARDS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CARDS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
                            car.rollback_record(); return true;
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
				Session["IRS_CIF_CARDS"] = null;
				Session["IRS_CIF_CARDS"] = car;
                Session["IRS_SEQ_ID"]    = null;
				Session["IRS_SEQ_ID"]    = car.seq_id;
				car = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion


        #region PRIVATE METHOD CLIENT_CONTACTS

        private irsClientContacts CLIENT_CONTACTS()
        {
            if (Session["IRS_CIF_CONTACTS"] == null)
            {
                Session["IRS_CIF_CONTACTS"] = new irsClientContacts();
            }

            return (irsClientContacts)Session["IRS_CIF_CONTACTS"];
        }

        private bool do_cif_contacts(string instruction)
        {
            irsClientContacts con = this.CLIENT_CONTACTS();

            try
            {
                if (instruction != null && instruction.Trim().Length > 0)
                {
                    if (instruction == "INSERT")
                    {
                        try
                        {
							con.seq_id = null;
                            con.selectedRow = -1;

                            if (con.insert_record() == false)
                            {
                                this.set_message(con.messages); return false;
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
                            con.seq_id = this.get_seq_id();
                            con.selectedRow = 0;

                            if (con.seq_id == null || con.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

                            if (con.update_record() == false)
                            {
                                this.set_message(con.messages); return false;
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
							if (con.ins_flag == true)
							{
								if (con.is_existing(this.parameter_values) == true)
								{
									con.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}
							}							

							if (this.SET_CONTACTS_RECORD(this.parameter_values) == false)
							{
								con.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(con.seq_id, irsClientEnrollment.VAPTTableName.CONTACTS, arrParameters);
                            arrCIF = CIF.Split('^');

                            /* RFC-31620 */
                            if (new irsUsers().adminApproverSwitch())
                            {
                                Session["AdminApprovalFlagCIFMessage"] = null;
                                string action = "RECORD SUBMITTED FOR APPROVAL";
                                DataTable dt = new DataTable();
                                DataRow dr;
                                string cifId = string.Empty;

                                dt.Columns.Add("Main_SEQ_ID");
                                dt.Columns.Add("CIFID");
                                dt.Columns.Add("ContactType");
                                dt.Columns.Add("ContactNo");
                                dt.Columns.Add("Remarks");
                                dt.Columns.Add("EmailAddress");
                                dt.Columns.Add("PreferredNo");
                                dt.Columns.Add("Deleted");
                                dt.Columns.Add("CIFClass");
                                dt.Columns.Add("CountryCode");
                                dr = dt.NewRow();

                                //DELETE
                                if (this.parameter_values.ToLower().IndexOf("deleted~true") >= 0)
                                {
                                    DataTable dtCIFContacts = new irsUsers().GetCIFContacts(con.seq_id);
                                    if (dtCIFContacts != null && dtCIFContacts.Rows.Count > 0)
                                    {
                                        cifId = dtCIFContacts.Rows[0]["CIFID"].ToString();
                                        if (new irsUsers().IsMakerAllowedCIFContacts(irsUsers.ActionType.DELETE, con.seq_id))
                                        {
                                            throw new Exception("There is a pending approval for the same cif. Submission for deletion to checker failed.");
                                        }

                                        dr[0] = con.seq_id; //Main_SEQ_ID
                                        dr[1] = cifId; //CIFID
                                        dr[2] = dtCIFContacts.Rows[0]["ContactType"]; //ContactType
                                        dr[3] = dtCIFContacts.Rows[0]["ContactNo"]; //ContactNo
                                        dr[4] = dtCIFContacts.Rows[0]["Remarks"]; //Remarks
                                        dr[5] = dtCIFContacts.Rows[0]["EmailAddress"]; //EmailAddress
                                        dr[6] = dtCIFContacts.Rows[0]["PreferredNo"]; //PreferredNo
                                        dr[7] = true; //Deleted
                                        dr[8] = dtCIFContacts.Rows[0]["CIFClass"]; //CIFClass
                                        dr[9] = dtCIFContacts.Rows[0]["CountryCode"]; //CountryCode
                                        dt.Rows.Add(dr);

                                        if (!new irsUsers().saveContactsMakerChecker(irsUsers.ActionType.DELETE, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [delete] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        this.set_message("Maker Checker saving [delete] failed");
                                        return false;
                                    }
                                }
                                else
                                {
                                    string[] arrCIFContacts = this.parameter_values.Split('|');
                                    cifId = arrCIFContacts.GetValue(0).ToString().Split('~').GetValue(1).ToString();

                                    dr[0] = con.seq_id; //Main_SEQ_ID
                                    dr[1] = cifId; //CIFID
                                    dr[2] = arrCIFContacts.GetValue(1).ToString().Split('~').GetValue(1); //ContactType
                                    dr[3] = arrCIFContacts.GetValue(2).ToString().Split('~').GetValue(1); //ContactNo
                                    dr[4] = arrCIFContacts.GetValue(3).ToString().Split('~').GetValue(1); //Remarks
                                    dr[5] = arrCIFContacts.GetValue(4).ToString().Split('~').GetValue(1); //EmailAddress
                                    dr[6] = arrCIFContacts.GetValue(5).ToString().Split('~').GetValue(1); //PreferredNo
                                    dr[7] = arrCIFContacts.GetValue(6).ToString().Split('~').GetValue(1); //Deleted
                                    dr[8] = ""; //CIFClass
                                    dr[9] = ""; //CountryCode
                                    dt.Rows.Add(dr);

                                    if (con.ins_flag) //ADD
                                    {
                                        if (!new irsUsers().saveContactsMakerChecker(irsUsers.ActionType.ADD, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [add] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (new irsUsers().IsMakerAllowedCIFContacts(irsUsers.ActionType.EDIT, con.seq_id))
                                        {
                                            throw new Exception("There is a pending approval for the same cif. Submission for edit to checker failed.");
                                        }
                                        if (!new irsUsers().saveContactsMakerChecker(irsUsers.ActionType.EDIT, dt, con.seq_id))
                                        {
                                            this.set_message("Maker Checker saving [edit] failed");
                                            return false;
                                        }
                                    }
                                }
                                Session["AdminApprovalFlagCIFMessage"] = action;

                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CONTACTS (MAKER/CHECKER)", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CONTACTS (MAKER/CHECKER)", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
                                return true;
                            }
                            else
                            {
                                if (con.commit_record() == false)
                                {
                                    con.seq_id = null;
                                    this.set_message(con.messages); return false;
                                }
                                else
                                {
                                    /* VAPT 20180222 */
                                    if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CONTACTS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                    else
                                    {
                                        if (arrCIF.GetValue(1).ToString().Length > 0)
                                        {
                                            new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CONTACTS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                        }
                                    }
                                }
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
                            con.rollback_record(); return true;
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
				Session["IRS_CIF_CONTACTS"] = null;
				Session["IRS_CIF_CONTACTS"] = con;
                Session["IRS_SEQ_ID"]       = null;
				Session["IRS_SEQ_ID"]       = con.seq_id;
				con = null;
            }

            this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
        }

        #endregion


		#region PRIVATE METHOD CLIENT_MERCHANT

		private irsClientMerchant CLIENT_MERCHANT()
		{
			if (Session["IRS_CIF_MERCHANT"] == null)
			{
				Session["IRS_CIF_MERCHANT"] = new irsClientMerchant();
			}

			return (irsClientMerchant)Session["IRS_CIF_MERCHANT"];
		}

		private bool do_cif_merchants(string instruction)
		{
			irsClientMerchant merc = this.CLIENT_MERCHANT();

			try
			{
				if (instruction != null && instruction.Trim().Length > 0)
				{
					if (instruction == "INSERT")
					{
						try
						{
							merc.seq_id = null;
                            merc.selectedRow = -1;

							if (merc.insert_record() == false)
							{
								this.set_message(merc.messages); return false;
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

                            merc.seq_id = this.get_seq_id();
                            merc.selectedRow = 0;

                            if (merc.seq_id == null || merc.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

                            if (merc.update_record() == false)
							{
								this.set_message(merc.messages); return false;
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
							if (merc.ins_flag == true)
							{
								if (merc.is_existing(this.parameter_values) == true)
								{
									merc.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}
							}
							if (this.SET_MERCHANT_RECORD(this.parameter_values) == false)
							{
								merc.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(merc.seq_id, irsClientEnrollment.VAPTTableName.MERCHANTS, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (merc.commit_record() == false)
                            {
                                merc.seq_id = null;
                                this.set_message(merc.messages); return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF MERCHANTS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF MERCHANTS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
							merc.rollback_record(); return true;
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
				Session["IRS_CIF_MERCHANT"] = null;
				Session["IRS_CIF_MERCHANT"] = merc;
                Session["IRS_SEQ_ID"]        = null;
				Session["IRS_SEQ_ID"]        = merc.seq_id;
				merc = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
		}

		#endregion


		#region PRIVATE METHOD CLIENT_MERCHANTS

		private irsClientMerchants CLIENT_MERCHANTS()
		{
			if (Session["IRS_CIF_MERCHANTS"] == null)
			{
				Session["IRS_CIF_MERCHANTS"] = new irsClientMerchants();
			}

			return (irsClientMerchants)Session["IRS_CIF_MERCHANTS"];
		}

		private bool do_cif_merchants_info(string instruction)
		{
			irsClientMerchants mer = this.CLIENT_MERCHANTS();

			try
			{
				if (instruction != null && instruction.Trim().Length > 0)
				{
					if (instruction == "INSERT")
					{
						try
						{
							mer.seq_id = null;
							mer.selectedRow = -1;

							if (mer.insert_record() == false)
							{
								this.set_message(mer.messages); return false;
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

							mer.seq_id = this.get_seq_id();
							mer.selectedRow = 0;

							if (mer.seq_id == null || mer.seq_id == string.Empty)
							{
								this.set_message("NO RECORD TO UPDATE"); return false;
							}

							if (mer.update_record() == false)
							{
								this.set_message(mer.messages); return false;
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
							if (mer.ins_flag == true)
							{
								if (mer.is_existing(this.parameter_values) == true)
								{
									mer.seq_id = null;
									this.set_message("RECORD ALREADY EXISTS!");
									return false;
								}
							}							
							
							if (this.SET_MERCHANTS_RECORD(this.parameter_values) == false)
							{
								mer.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(mer.seq_id, irsClientEnrollment.VAPTTableName.MERCHANTINFO, arrParameters);
                            arrCIF = CIF.Split('^');

                            /* RFC-31620 */
                            if (new irsUsers().adminApproverSwitch())
                            {
                                Session["AdminApprovalFlagCIFMessage"] = null;
                                string action = "RECORD SUBMITTED FOR APPROVAL";
                                DataTable dt = new DataTable();
                                DataRow dr;
                                string cifId = string.Empty;

                                dt.Columns.Add("Main_SEQ_ID");
                                dt.Columns.Add("CIFID");
                                dt.Columns.Add("ProductCode");
                                dt.Columns.Add("AccountSeqId");
                                dt.Columns.Add("MerchantCode");
                                dt.Columns.Add("ImplementationCode");
                                dt.Columns.Add("MerchantProductCode");
                                dt.Columns.Add("MerchantType");
                                dt.Columns.Add("Deleted");
                                dt.Columns.Add("Maker");
                                dt.Columns.Add("Checker");
                                dt.Columns.Add("DateAdded");
                                dt.Columns.Add("CurrStatus");
                                dt.Columns.Add("StatusDate");
                                dt.Columns.Add("ActionType");
                                dt.Columns.Add("Module");
                                dt.Columns.Add("MenuName");
                                dr = dt.NewRow();

                                //DELETE
                                if (this.parameter_values.ToLower().IndexOf("deleted~true") >= 0)
                                {
                                    DataTable dtCIFMerchantsInfo = new irsUsers().GetCIFMerchantsInfo(mer.seq_id);
                                    if (dtCIFMerchantsInfo != null && dtCIFMerchantsInfo.Rows.Count > 0)
                                    {
                                        cifId = dtCIFMerchantsInfo.Rows[0]["CIFID"].ToString();
                                        if (new irsUsers().IsMakerAllowedCIFMerchantsInfo(irsUsers.ActionType.DELETE, mer.seq_id))
                                        {
                                            Session["AdminApprovalFlagCIFMessage"] = "There is a pending approval for the same cif. Submission for deletion to checker failed.";
                                            throw new Exception("There is a pending approval for the same cif. Submission for deletion to checker failed.");
                                        }

                                        dr[0] = mer.seq_id; //Main_SEQ_ID
                                        dr[1] = cifId; //CIFID
                                        dr[2] = dtCIFMerchantsInfo.Rows[0]["ProductCode"]; //ProductCode
                                        dr[3] = dtCIFMerchantsInfo.Rows[0]["AccountSeqId"]; //AccountSeqId
                                        dr[4] = dtCIFMerchantsInfo.Rows[0]["MerchantCode"]; //MerchantCode
                                        dr[5] = dtCIFMerchantsInfo.Rows[0]["ImplementationCode"]; //ImplementationCode
                                        dr[6] = dtCIFMerchantsInfo.Rows[0]["MerchantProductCode"]; //MerchantProductCode
                                        dr[7] = dtCIFMerchantsInfo.Rows[0]["MerchantType"]; //MerchantType
                                        dr[8] = true; //Deleted
                                        dt.Rows.Add(dr);

                                        if (!new irsUsers().saveMerchantsInfoMakerChecker(irsUsers.ActionType.DELETE, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [delete] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        this.set_message("Maker Checker saving [delete] failed");
                                        return false;
                                    }
                                }
                                else
                                {
                                    string[] arrCIFMerchantsInfo = this.parameter_values.Split('|');
                                    cifId = arrCIFMerchantsInfo.GetValue(0).ToString().Split('~').GetValue(1).ToString();

                                    dr[0] = mer.seq_id; //Main_SEQ_ID
                                    dr[1] = cifId; //CIFID
                                    dr[2] = arrCIFMerchantsInfo.GetValue(1).ToString().Split('~').GetValue(1); //ProductCode
                                    dr[3] = arrCIFMerchantsInfo.GetValue(3).ToString().Split('~').GetValue(1); //AccountSeqId
                                    dr[4] = arrCIFMerchantsInfo.GetValue(4).ToString().Split('~').GetValue(1); //MerchantCode
                                    dr[5] = arrCIFMerchantsInfo.GetValue(5).ToString().Split('~').GetValue(1); //ImplementationCode
                                    dr[6] = arrCIFMerchantsInfo.GetValue(2).ToString().Split('~').GetValue(1); //MerchantProductCode
                                    dr[7] = arrCIFMerchantsInfo.GetValue(6).ToString().Split('~').GetValue(1); //MerchantType
                                    dr[8] = arrCIFMerchantsInfo.GetValue(7).ToString().Split('~').GetValue(1); //Deleted
                                    dt.Rows.Add(dr);

                                    if (mer.ins_flag) //ADD
                                    {
                                        if (!new irsUsers().saveMerchantsInfoMakerChecker(irsUsers.ActionType.ADD, dt, cifId))
                                        {
                                            this.set_message("Maker Checker saving [add] failed");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        if (new irsUsers().IsMakerAllowedCIFMerchantsInfo(irsUsers.ActionType.EDIT, mer.seq_id))
                                        {
                                            throw new Exception("There is a pending approval for the same cif. Submission for edit to checker failed.");
                                        }
                                        if (!new irsUsers().saveMerchantsInfoMakerChecker(irsUsers.ActionType.EDIT, dt, mer.seq_id))
                                        {
                                            this.set_message("Maker Checker saving [edit] failed");
                                            return false;
                                        }
                                    }
                                }
                                Session["AdminApprovalFlagCIFMessage"] = action;

                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF MERCHANT INFO (MAKER/CHECKER)", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF MERCHANT INFO (MAKER/CHECKER)", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
                                return true;
                            }
                            else
                            {
                                if (mer.commit_record() == false)
                                {
                                    mer.seq_id = null;
                                    this.set_message(mer.messages); return false;
                                }
                                else
                                {
                                    if (mer.ins_flag == true)
                                    {
                                        mer.create_gen_fields(this.parameter_values);
                                    }

                                    /* VAPT 20180222 */
                                    if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF MERCHANT INFO", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                    else
                                    {
                                        if (arrCIF.GetValue(1).ToString().Length > 0)
                                        {
                                            new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF MERCHANT INFO", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                        }
                                    }
                                    return true;
                                }
                            }
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
							mer.rollback_record(); return true;
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
				Session["IRS_CIF_MERCHANTS"] = null;
				Session["IRS_CIF_MERCHANTS"] = mer;
				Session["IRS_SEQ_ID"] = null;
				Session["IRS_SEQ_ID"] = mer.seq_id;
				mer = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
		}

		#endregion


		#region PRIVATE METHOD CLIENT_OCCUPATIONS

		private irsClientOccupations CLIENT_OCCUPATIONS()
		{
			if (Session["IRS_CIF_OCCUPATIONS"] == null)
			{
				Session["IRS_CIF_OCCUPATIONS"] = new irsClientOccupations();
			}

			return (irsClientOccupations)Session["IRS_CIF_OCCUPATIONS"];
		}

		private bool do_cif_occupations(string instruction)
		{
			irsClientOccupations occ = this.CLIENT_OCCUPATIONS();

			try
			{
				if (instruction != null && instruction.Trim().Length > 0)
				{
					if (instruction == "INSERT")
					{
						try
						{
							occ.seq_id = null;
                            occ.selectedRow = -1;

							if (occ.insert_record() == false)
							{
								this.set_message(occ.messages); return false;
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
                            occ.seq_id = this.get_seq_id();
                            occ.selectedRow = 0;

                            if (occ.seq_id == null || occ.seq_id == string.Empty)
                            {
                                this.set_message("NO RECORD TO UPDATE"); return false;
                            }

                            if (occ.update_record() == false)
							{
								this.set_message(occ.messages); return false;
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
							if (this.SET_OCCUPATIONS_RECORD(this.parameter_values) == false)
							{
								occ.seq_id = null;
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(occ.seq_id, irsClientEnrollment.VAPTTableName.OCCUPATIONS, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (occ.commit_record() == false)
                            {
                                occ.seq_id = null;
                                this.set_message(occ.messages);
                                return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF OCCUPATIONS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF OCCUPATIONS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
							occ.rollback_record();	return true;							
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
				Session["IRS_CIF_OCCUPATIONS"] = null;
				Session["IRS_CIF_OCCUPATIONS"] = occ;
                Session["IRS_SEQ_ID"]          = null;
				Session["IRS_SEQ_ID"]          = occ.seq_id;
				occ = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
		}

		#endregion


		#region PRIVATE METHOD CLIENT_REMITTERS

		private irsClientRemitters CLIENT_REMITTERS()
		{
			if (Session["IRS_CIF_REMITTERS"] == null)
			{
				Session["IRS_CIF_REMITTERS"] = new irsClientRemitters();
			}

			return (irsClientRemitters)Session["IRS_CIF_REMITTERS"];
		}

		private bool do_cif_remitters(string instruction)
		{
			irsClientRemitters rem = this.CLIENT_REMITTERS();

			try
			{
				if (instruction != null && instruction.Trim().Length > 0)
				{
					string[] cifid = this.parameter_values.Split('~');
					string[] id = cifid[1].Split('|');

					DataTable dt = rem.get_rem_info(id[0]);

					if (dt != null)
					{
						if (dt.Rows.Count > 0)
						{							
							rem.seq_id = Convert.ToString(dt.Rows[0]["SEQ_ID"]);
							rem.selectedRow = 0;

							if (rem.seq_id == null || rem.seq_id == string.Empty)
							{
								this.set_message("NO RECORD TO UPDATE"); return false;
							}
							if (rem.update_record() == false)
							{
								this.set_message(rem.messages); return false;
							}
						}
						else
						{
							rem.selectedRow = -1;

							if (rem.insert_record() == false)
							{
								this.set_message(rem.messages); return false;
							}
						}

						if (this.SET_REMITTERS_RECORD(this.parameter_values) == false)
						{
							return false;
						}

                        /* VAPT 20180222 */
                        string CIF = string.Empty;
                        string[] arrCIF = null;
                        string[] arrParameters = this.parameter_values.Split('|');
                        CIF = new irsClientEnrollment().getVACIF(rem.seq_id, irsClientEnrollment.VAPTTableName.REMITTERS, arrParameters);
                        arrCIF = CIF.Split('^');

                        if (rem.commit_record() == false)
                        {
                            this.set_message(rem.messages); return false;
                        }
                        else
                        {
                            /* VAPT 20180222 */
                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF REMITTERS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF REMITTERS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }
                        }

						return true;

					}

				}
			}
			catch(Exception e)
			{
				this.set_message(e.Message); return false;
			}
			finally
			{
				Session["IRS_CIF_REMITTERS"] = null;
				Session["IRS_CIF_REMITTERS"] = rem;
                Session["IRS_SEQ_ID"] = null;
				Session["IRS_SEQ_ID"] = rem.seq_id;
				rem = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
		}

		#endregion


		#region PRIVATE METHOD CLIENT_CLASSIFICATIONS

		private irsClientClassifications CLIENT_CLASSIFICATIONS()
		{
			if (Session["IRS_CIF_CLASSIFICATIONS"] == null)
			{
				Session["IRS_CIF_CLASSIFICATIONS"] = new irsClientClassifications();
			}

			return (irsClientClassifications)Session["IRS_CIF_CLASSIFICATIONS"];
		}

		private bool do_cif_classifications(string instruction)
		{
			irsClientClassifications cl = this.CLIENT_CLASSIFICATIONS();

			try
			{
				if (instruction == "INSERT")
				{
					try
					{
						cl.seq_id = null;
						cl.selectedRow = -1;

						if (cl.insert_record() == false)
						{
							this.set_message(cl.messages); return false;
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
						cl.seq_id = this.get_seq_id();
						cl.selectedRow = 0;

						if (cl.seq_id == null || cl.seq_id == string.Empty)
						{
							this.set_message("NO RECORD TO UPDATE"); return false;
						}

						if (cl.update_record() == false)
						{
							this.set_message(cl.messages); return false;
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
						if (cl.ins_flag == true)
						{
							if (cl.is_existing(this.parameter_values) == true)
							{
								return true;
							}
						}

						if (this.SET_CLASSIFICATIONS_RECORD(this.parameter_values) == false)
						{
							cl.seq_id = null;
							return false;
						}

                        /* VAPT 20180222 */
                        string CIF = string.Empty;
                        string[] arrCIF = null;
                        string[] arrParameters = this.parameter_values.Split('|');
                        CIF = new irsClientEnrollment().getVACIF(cl.seq_id, irsClientEnrollment.VAPTTableName.CLASS, arrParameters);
                        arrCIF = CIF.Split('^');

                        if (cl.commit_record() == false)
                        {
                            cl.seq_id = null;
                            this.set_message(cl.messages); return false;
                        }
                        else
                        {
                            /* VAPT 20180222 */
                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CLASS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CLASS", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }
                        }
                        return true;

					}
					catch (Exception e)
					{
						this.set_message(e.Message); return false;
					}
				}
			}
			catch
			{
			}
			finally
			{
				Session["IRS_CIF_CLASSIFICATIONS"] = null;
				Session["IRS_CIF_CLASSIFICATIONS"] = cl;
				Session["IRS_SEQ_ID"] = null;
				Session["IRS_SEQ_ID"] = cl.seq_id;
				cl = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;

		}

		#endregion		


		#region PRIVATE METHOD CLIENT_SPOUSE

		private irsClientSpouse CLIENT_SPOUSE()
		{
			if (Session["IRS_CIF_SPOUSE"] == null)
			{
				Session["IRS_CIF_SPOUSE"] = new irsClientSpouse();
			}

			return (irsClientSpouse)Session["IRS_CIF_SPOUSE"];
		}


		private bool do_cif_spouse(string instruction)
		{
			irsClientSpouse csp = this.CLIENT_SPOUSE();

			try
			{
				if (instruction != null && instruction.Trim().Length > 0)
				{
					if (instruction == "INSERT")
					{
						try
						{
							csp.selectedRow = -1;

							if (csp.insert_record() == false)
							{
								this.set_message(csp.messages); return false;
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
							csp.seq_id = this.get_seq_id();
							csp.selectedRow = 0;

							if (csp.seq_id == null || csp.seq_id == string.Empty)
							{
								this.set_message("NO RECORD TO UPDATE"); return false;
							}

							if (csp.update_record() == false)
							{
								this.set_message(csp.messages); return false;
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
							if (this.SET_SPOUSE_RECORD(this.parameter_values) == false)
							{
								return false;
							}

                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            string[] arrParameters = this.parameter_values.Split('|');
                            CIF = new irsClientEnrollment().getVACIF(csp.seq_id, irsClientEnrollment.VAPTTableName.SPOUSE, arrParameters);
                            arrCIF = CIF.Split('^');

                            if (csp.commit_record() == false)
                            {
                                this.set_message(csp.messages); return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF SPOUSE", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF SPOUSE", "", arrCIF.GetValue(1).ToString(), "", csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
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
							csp.rollback_record(); return true;
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
				Session["IRS_CIF_SPOUSE"] = null;
				Session["IRS_CIF_SPOUSE"] = csp;
				Session["IRS_SEQ_ID"]     = null;
				Session["IRS_SEQ_ID"]     = csp.seq_id;
				csp = null;
			}

			this.set_message("UNABLE TO PERFORM OPERATION: " + instruction); return false;
		}



        #endregion

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
    }
}
