using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
	//ars reqd fields: fname, lname, contact, street, city, zipcode at country 
	[Serializable]
	public class irsClientEnrollment : sqlHelperWs
	{
		irsClientMaster			 cif_mas = new irsClientMaster();
		irsClientClassifications cif_cls = new irsClientClassifications();
		irsClientAddress		 cif_add = new irsClientAddress();
		irsClientContacts		 cif_con = new irsClientContacts();
		irsClientOccupations	 cif_occ = new irsClientOccupations();
		irsClientIds			 cif_ids = new irsClientIds();
		irsClientOtherIds		 cif_oth = new irsClientOtherIds();
		irsClientAccounts		 cif_acc = new irsClientAccounts();
		irsClientBeneficiaries	 cif_ben = new irsClientBeneficiaries();
		irsClientCards			 cif_car = new irsClientCards();
		irsClientRemitters		 cif_rem = new irsClientRemitters();
        irsClientVan             cif_van = new irsClientVan();
        irsClientNotes           cif_notes = new irsClientNotes();
                
		#region PRIVATE VARIABLES

		private string[] mas_fldcols = null;
		private string[] cls_fldcols = null;
		private string[] add_fldcols = null;

        private string[] add_fldcols2 = null;        
        private string[] add_fldcols3 = null;

		private string[] con_fldcols = null;
		private string[] occ_fldcols = null;
		private string[] ids_fldcols = null;
		private string[] ids_fldcols2= null;
		private string[] oth_fldcols = null;
		private string[] acc_fldcols = null;
		private string[] ben_fldcols = null;
		private string[] car_fldcols = null;
		private string[] rem_fldcols = null;
		private string[] mas_reqflds = null;
		private string[] cls_reqflds = null;
        private string[] van_fldcols = null;
        private string[] notes_fldcols = null;
		private string mas_fldvals = null;
		private string cls_fldvals = null;
		private string add_fldvals = null;

        private string add_fldvals2 = null;
        private string add_fldvals3 = null;

		private string con_fldvals = null;
		private string con_fldvals2= null;
		private string occ_fldvals = null;
		private string ids_fldvals = null;
        private string van_fldvals = null;
        private string notes_fldvals = null;

        private string ids_fldvals2= null;
		private string oth_fldvals = null;
		private string acc_fldvals = null;
		private string ben_fldvals = null;
		private string car_fldvals = null;
		private string rem_fldvals = null;
		private string error_msg   = "";
		private string cifid	   = null;
		private string missing_fields = null;
		private string cifclass    = null;
		private string brcode	   = null;
		private string e_cifid	   = null;
		private string acc_seqid   = null;
		private string car_seqid = null;
		private string mas_seqid   = null;
		private string ben_seqid   = null;
		private string acctseqid   = null; /* SEQ_ID when fxn is UPDATE_ACCT */
		private string cardseqid   = null; /* SEQ_ID when fxn is UPDATE_CARD */
        private string idsseqid    = null; /* SEQ_ID when fxn is UPDATE_IDS */
        private string vanseqid    = null;
        private string notesseqid  = null;

        private string addseqid_Home = null;
        private string addseqid_Com = null;
        private string addseqid_Mail = null;
		
        private bool _INSERT_FLAG  = true;
		private string ciftype	   = null;

		#endregion

		public enum Function_Name : short
		{
			INSERT,
			DELETE,
			/* CIFID must be supplied for the ff functions */
			UPDATE,			
			ADD_ACCOUNT,
			ADD_CARD,
			ADD_CIFCLASS,
			ADD_BENE,		/* add only in tbl_cif_beneficiaries (link of remitter and bene only) but not in tbl_cif_master */
			UPDATE_BENE,	/* update tbl_cif_beneficiaries */
			UPDATE_ACCT, 	/* update tbl_cif_accounts */
			UPDATE_CARD, 	/* update tbl_cif_cards */
            UPDATE_IDS,      /* update tbl_cif_ids */
            ADD_IDS,
            ADD_VAN,
            ADD_NOTES,
            UPDATE_VAN,
            UPDATE_NOTES,
            ADD_ADDRESS
		}

        public void isWebRemittanceEnroll(string userid,string Branch)
        {
            irsClientData cdt = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            cdt._location = Branch;
            cdt._userid = userid;
            cdt._role = 10;
            System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] = cdt;
        }

        private void setWinApp()
        {
            cif_mas.IsWinApp = base.IsWinApp;
            cif_cls.IsWinApp = base.IsWinApp;
            cif_add.IsWinApp = base.IsWinApp;
            cif_con.IsWinApp = base.IsWinApp;
            cif_occ.IsWinApp = base.IsWinApp;
            cif_ids.IsWinApp = base.IsWinApp;
            cif_oth.IsWinApp = base.IsWinApp;
            cif_acc.IsWinApp = base.IsWinApp;
            cif_ben.IsWinApp = base.IsWinApp;
            cif_car.IsWinApp = base.IsWinApp;
            cif_rem.IsWinApp = base.IsWinApp;
            cif_van.IsWinApp = base.IsWinApp;
            cif_notes.IsWinApp = base.IsWinApp;
        }

		public string enroll_agent(irsClientEnrollment.Function_Name fxn, String plist, out String errmsg)
		{
            setWinApp();

			if (this.do_init(plist, fxn) == true)
			{
				errmsg = error_msg;

				if (this.do_cif(fxn, plist, Convert.ToInt32(cifclass)) == true)
				{
					if (fxn == Function_Name.INSERT || fxn == Function_Name.UPDATE || 
						fxn == Function_Name.ADD_CIFCLASS)
					{
						return cifid;
					}
					else if (fxn == Function_Name.UPDATE_BENE || fxn == Function_Name.ADD_BENE)
					{
						return e_cifid;
					}
					else if (fxn == Function_Name.ADD_CARD || fxn == Function_Name.UPDATE_CARD)
					{
						return car_seqid;
					}
                    else if (fxn == Function_Name.ADD_NOTES)
                    {
                        return notesseqid;
                    }
					else return acc_seqid;
				}
				else
				{
					errmsg = error_msg;
					if (fxn == Function_Name.ADD_ACCOUNT) return acc_seqid;					
					return errmsg;
				}
			}
			else
			{
				errmsg = error_msg;
				return errmsg;
			}
		}

		public bool do_init(string plist, irsClientEnrollment.Function_Name fxn)
		{
			try
			{
                if (fxn == irsClientEnrollment.Function_Name.INSERT)
                {
                    //RFC-6084 : PhilSys No in IRSW 08252021
                    //mas_fldcols = new string[] { "UserID", "CIFType", "LastName", "FirstName", "MiddleName", "Gender", "Age", "CivilStatus", "Citizenship", "BirthDate", "BirthPlace", "LocationCode", "Remarks", "CreatedBy", "DateCreated", "DateModified", "ModifiedBy", "Title", "Alias", "BranchCode", "Religion", "OLDCIFID", "CIFClass", "VIPType", "Garnish", "Remitter_Inputed_ID", "isWebRemitter", "isPhoneRemitter", "RemReferredBy", "Disclaimer" ,"isCIFRemoteOverride" };
                    /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
                    mas_fldcols = new string[] { "UserID", "CIFType", "LastName", "FirstName", "MiddleName", "Gender", "Age", "CivilStatus", "Citizenship", "BirthDate", "BirthPlace", "LocationCode", "Remarks", "CreatedBy", "DateCreated", "DateModified", "ModifiedBy", "Title", "Alias", "BranchCode", "Religion", "OLDCIFID", "CIFClass", "VIPType", "Garnish", "Remitter_Inputed_ID", "isWebRemitter", "isPhoneRemitter", "RemReferredBy", "Disclaimer", "isCIFRemoteOverride", "PhilSysNo", "CountryOfBirth"  };
                    //======================================================================
                }
                else
                {
                    //RFC-6084 : PhilSys No in IRSW 08252021
                    //mas_fldcols = new string[] { "UserID", "CIFType", "LastName", "FirstName", "MiddleName", "Gender", "Age", "CivilStatus", "Citizenship", "BirthDate", "BirthPlace", "LocationCode", "Remarks", "CreatedBy", "DateCreated", "DateModified", "ModifiedBy", "Title", "Alias", "BranchCode", "Religion", "OLDCIFID", "VIPType", "Garnish", "Remitter_Inputed_ID", "isWebRemitter", "isPhoneRemitter", "RemReferredBy", "Disclaimer", "isCIFRemoteOverride" };
                    /* RFC-33569 - Instapay ISO20022 enhancement for IRSW and SG Mobile */
                    mas_fldcols = new string[] { "UserID", "CIFType", "LastName", "FirstName", "MiddleName", "Gender", "Age", "CivilStatus", "Citizenship", "BirthDate", "BirthPlace", "LocationCode", "Remarks", "CreatedBy", "DateCreated", "DateModified", "ModifiedBy", "Title", "Alias", "BranchCode", "Religion", "OLDCIFID", "VIPType", "Garnish", "Remitter_Inputed_ID", "isWebRemitter", "isPhoneRemitter", "RemReferredBy", "Disclaimer", "isCIFRemoteOverride", "PhilSysNo", "CountryOfBirth"  };
                    //======================================================================
                }
                cls_fldcols = new string[] { "CIFClass" };
                
                add_fldcols = new string[] { "Address1", "Address2", "Address3", "Address4", "Address5", "Address6", "ZipCode", "CountryCode", "POBox", "POBoxDistrict", "AddressType", "AddressRemarks", "PreferredMailAdd", "PostalCode", "RemZip1Code", "CIFClass","Address3k", "AddSeqID" };
                add_fldcols2 = new string[] { "Address1_Com", "Address2_Com", "Address3_Com", "Address4_Com", "Address5_Com", "Address6_Com", "ZipCode_Com", "CountryCode_Com", "POBox_Com", "POBoxDistrict_Com", "AddressType_Com", "AddressRemarks_Com", "PreferredMailAdd_Com", "PostalCode_Com", "RemZip1Code_Com", "CIFClass", "Address3k_Com", "AddSeqID_Com" };
                add_fldcols3 = new string[] { "Address1_Mail", "Address2_Mail", "Address3_Mail", "Address4_Mail", "Address5_Mail", "Address6_Mail", "ZipCode_Mail", "CountryCode_Mail", "POBox_Mail", "POBoxDistrict_Mail", "AddressType_Mail", "AddressRemarks_Mail", "PreferredMailAdd_Mail", "PostalCode_Mail", "RemZip1Code_Mail", "CIFClass", "Address3k_Mail", "AddSeqID_Mail" };

                con_fldcols = new string[] { "ContactType", "ContactNo", "EmailAddress", "SMSAlert", "PreferredNo", "ContactRemarks", "CIFClass" };//, "CountryCodeSEQID" };
				occ_fldcols = new string[] { "Occupation", "OccupationType", "WorkerType", "Position", "Division", "Sector", "Employer", "Salary", "OccCurrency", "OccupationRemarks", "DetailedDesc", "SourceOfFunds" };
                // Alyssa 06/16/2014 PlaceOfIssuance
                //ids_fldcols = new string[] { "IDNo", "IDType", "IDExpiryDate", "IDRemarks", "VisaType", "IDClassification", "Deleted"};
                //ids_fldcols2 = new string[] { "IDNo2", "IDType2", "IDExpiryDate2", "IDRemarks2", "VisaType2", "IDClassification2"};
                ids_fldcols = new string[] { "IDNo", "IDType", "IDExpiryDate", "IDRemarks", "VisaType", "PlaceOfIssuance", "IDClassification", "Deleted"};
                ids_fldcols2 = new string[] { "IDNo2", "IDType2", "IDExpiryDate2", "IDRemarks2", "VisaType2", "PlaceOfIssuance2", "IDClassification2", "Deleted"};
				oth_fldcols = new string[] { "RefNoTheirs", "OtherIDs", "IRSWorldSystem" };
				//acc_fldcols = new string[] { "AccountNo", "AccountType", "AccountCurrency", "AccountBranchCode", "DomBankCode", "AccountRemarks", "AccountStatus", "KBIAccount", "OtherBankSeqId", "BranchType", "branchname", "AccountDeleted", "RoutingNo" };
                acc_fldcols = new string[] { "AccountNo", "AccountType", "AccountCurrency", "AccountBranchCode", "DomBankCode", "AccountRemarks", "AccountStatus", "KBIAccount", "OtherBankSeqId", "BranchType", "branchname", "AccountDeleted" };
                ben_fldcols = new string[] { "CIFID", "RemitterCIFID", "Relationship", "NotificationMode", "ProductCode", "ImplementationCode", "AccountSeqId", "ProdBranchCode", "ProdCurrency", "ProdAccountType" };
				car_fldcols = new string[] { "CardType", "CardNo", "CardName", "CardIssueDate", "CardExpiryDate", "BankCode", "CardClass", "SecurityCode", "IssueNumber", "ScannedCard", "CardRemarks", "Country", "CardCurrency", "IsArm", "OthBankName" };
				rem_fldcols = new string[] { "VIPStatus", "RemitterPoints", "ChargeType", "RiskRating" };
                van_fldcols = new string[] { "VAN", "VANType", "DateCreated", "DateModified", "BeneficiarySEQ_ID" };
                notes_fldcols = new string[] { "Note", "DateCreated", "Teller" };

				string[] cols = plist.Split('|');

				for (int i = 0; i < cols.Length; i++)
				{
					string[] vals = cols[i].Split('~');
					string[] vals2 = vals[1].Split('^');

					if (vals[0].ToUpper() == "CIFID")   e_cifid = vals[1];
					if (vals[0].ToUpper() == "CIFTYPE") ciftype = vals[1];
					
					if (vals[0].ToUpper() == "SEQ_ID")
					{
						if (fxn == Function_Name.UPDATE_BENE) ben_seqid = vals[1];
						if (fxn == Function_Name.UPDATE_ACCT) acctseqid = vals[1];
						if (fxn == Function_Name.UPDATE_CARD) cardseqid = vals[1];
                        if (fxn == Function_Name.UPDATE_IDS) idsseqid = vals[1];
                        if (fxn == Function_Name.UPDATE_VAN) vanseqid = vals[1];
                        if (fxn == Function_Name.UPDATE_NOTES) notesseqid = vals[1];
					} 

					for (int j = 0; j < mas_fldcols.Length; j++)
					{
                        if (mas_fldcols[j].ToUpper() == vals[0].ToUpper()) mas_fldvals += cols[i] + '|';
						if (vals[0].ToUpper() == "BRANCHCODE") brcode = vals[1];
					}

					for (int l = 0; l < con_fldcols.Length; l++)
					{
						if (con_fldcols[l].ToUpper() == vals[0].ToUpper())
						{
							if (vals2.Length == 1) con_fldvals += cols[i] + '|';
							if (vals2.Length > 1) 
							{
                                //if (con_fldcols[l] == "ContactNo" || con_fldcols[l] == "PreferredNo")
                                //{
                                    con_fldvals += vals[0] + '~' + vals2[0] + "|ContactType~7|"; /* mobile number */
                                    con_fldvals2 += vals[0] + '~' + vals2[1] + "|ContactType~1|"; /* telephone number */
                                //}
                                
                                //if (con_fldcols[l] == "CountryCodeSEQID")
                                //{
                                //    con_fldvals += vals[0] + '~' + vals2[0] + "|"; /* Country Code SEQID */
                                //    con_fldvals2 += vals[0] + '~' + vals2[1] + "|"; /* Country Code SEQID */
                                //}
							}
						}
					}
					for (int m = 0; m < ids_fldcols.Length; m++) if (ids_fldcols[m].ToUpper() == vals[0].ToUpper()) ids_fldvals += cols[i] + '|';
					for (int u = 0; u < ids_fldcols2.Length; u++) if (ids_fldcols2[u].ToUpper()== vals[0].ToUpper()) ids_fldvals2+= cols[i] + '|';
					for (int n = 0; n < occ_fldcols.Length; n++) if (occ_fldcols[n].ToUpper() == vals[0].ToUpper()) occ_fldvals += cols[i] + '|';
					for (int o = 0; o < oth_fldcols.Length; o++) if (oth_fldcols[o].ToUpper() == vals[0].ToUpper()) oth_fldvals += cols[i] + '|';
					for (int p = 0; p < cls_fldcols.Length; p++) if (cls_fldcols[p].ToUpper() == vals[0].ToUpper()){cls_fldvals += cols[i] + '|'; cifclass = vals[1]; }
					for (int q = 0; q < acc_fldcols.Length; q++) if (acc_fldcols[q].ToUpper() == vals[0].ToUpper()) acc_fldvals += cols[i] + '|'; 
					for (int r = 0; r < ben_fldcols.Length; r++) if (ben_fldcols[r].ToUpper() == vals[0].ToUpper()) ben_fldvals += cols[i] + '|';
					for (int s = 0; s < car_fldcols.Length; s++) if (car_fldcols[s].ToUpper() == vals[0].ToUpper()) car_fldvals += cols[i] + '|';
					for (int t = 0; t < rem_fldcols.Length; t++) if (rem_fldcols[t].ToUpper() == vals[0].ToUpper()) rem_fldvals += cols[i] + '|';
                    for (int v = 0; v < van_fldcols.Length; v++) if (van_fldcols[v].ToUpper() == vals[0].ToUpper()) van_fldvals += cols[i] + '|';
                    for (int n1 = 0; n1 < notes_fldcols.Length; n1++) if (notes_fldcols[n1].ToUpper() == vals[0].ToUpper()) notes_fldvals += cols[i] + '|';

                    for (int k = 0; k < add_fldcols.Length; k++)                    
                    {
                        if (add_fldcols[k].ToUpper() == vals[0].ToUpper())
                        {
                            if (vals[0].ToUpper() == "ADDSEQID")
                            {
                                addseqid_Home = cols[i].Split('~')[1];
                            }
                            else
                            {
                                add_fldvals += cols[i] + '|';
                            }
                        }
                        
                    }

                    for (int k2 = 0; k2 < add_fldcols2.Length; k2++)
                    {
                        if (add_fldcols2[k2].ToUpper() == vals[0].ToUpper())
                        {
                            if (vals[0].ToUpper() == "ADDSEQID_COM")
                            {
                                addseqid_Com = cols[i].Split('~')[1];
                            }
                            else
                            {
                                add_fldvals2 += cols[i] + '|';
                            }
                        }
                    }

                    for (int k3 = 0; k3 < add_fldcols3.Length; k3++)
                    {
                        if (add_fldcols3[k3].ToUpper() == vals[0].ToUpper())
                        {
                            if (vals[0].ToUpper() == "ADDSEQID_MAIL")
                            {
                                addseqid_Mail = cols[i].Split('~')[1];
                            }
                            else
                            {
                                add_fldvals3 += cols[i] + '|';
                            }
                        }
                    }
				}

				if (occ_fldvals != null)
				{
					occ_fldvals = occ_fldvals.Replace("OccCurrency", "Currency" );
				}

				if (acc_fldvals != null)
				{
					acc_fldvals = acc_fldvals.Replace("AccountCurrency", "Currency");
					acc_fldvals = acc_fldvals.Replace("AccountBranchCode", "BranchCode");
					acc_fldvals = acc_fldvals.Replace("AccountRemarks", "Remarks");
					acc_fldvals = acc_fldvals.Replace("AccountStatus", "Status");
                    acc_fldvals = acc_fldvals.Replace("AccountDeleted", "Deleted");
				}

				if (car_fldvals != null)
				{
					car_fldvals = car_fldvals.Replace("CardIssueDate", "IssueDate");
					car_fldvals = car_fldvals.Replace("CardExpiryDate", "ExpiryDate");
					car_fldvals = car_fldvals.Replace("CardRemarks", "Remarks");
					car_fldvals = car_fldvals.Replace("CardCurrency", "Currency");
				}
                
                if (add_fldvals != null)
                {
                    add_fldvals = add_fldvals.Replace("ZipCode", "ZipCodeSeqId");
                    add_fldvals = add_fldvals.Replace("RemZip1Code", "ZipCode");
                    add_fldvals = add_fldvals.Replace("AddressRemarks", "Remarks");
                    add_fldvals = add_fldvals.Replace("AddSeqID", "SEQ_ID");
                }

                if (add_fldvals2 != null)
                {   
                    add_fldvals2 = add_fldvals2.Replace("Address1_Com", "Address1");
                    add_fldvals2 = add_fldvals2.Replace("Address2_Com", "Address2");
                    add_fldvals2 = add_fldvals2.Replace("Address3_Com", "Address3");
                    add_fldvals2 = add_fldvals2.Replace("Address4_Com", "Address4");
                    add_fldvals2 = add_fldvals2.Replace("Address5_Com", "Address5");
                    add_fldvals2 = add_fldvals2.Replace("Address6_Com", "Address6");
                    add_fldvals2 = add_fldvals2.Replace("ZipCode_Com", "ZipCodeSeqId");
                    add_fldvals2 = add_fldvals2.Replace("CountryCode_Com", "CountryCode");
                    add_fldvals2 = add_fldvals2.Replace("POBox_Com", "POBox");
                    add_fldvals2 = add_fldvals2.Replace("POBoxDistrict_Com", "POBoxDistrict");
                    add_fldvals2 = add_fldvals2.Replace("AddressType_Com", "AddressType");
                    add_fldvals2 = add_fldvals2.Replace("AddressRemarks_Com", "Remarks");
                    add_fldvals2 = add_fldvals2.Replace("PreferredMailAdd_Com", "PreferredMailAdd");
                    add_fldvals2 = add_fldvals2.Replace("PostalCode_Com", "PostalCode");
                    add_fldvals2 = add_fldvals2.Replace("RemZip1Code_Com", "ZipCode");
                    add_fldvals2 = add_fldvals2.Replace("CIFClass_Com", "CIFClass");
                    add_fldvals2 = add_fldvals2.Replace("Address3k_Com", "Address3k");
                    add_fldvals2 = add_fldvals2.Replace("AddSeqID_Com", "SEQ_ID");
                }

                if (add_fldvals3 != null)
                {
                    add_fldvals3 = add_fldvals3.Replace("Address1_Mail", "Address1");
                    add_fldvals3 = add_fldvals3.Replace("Address2_Mail", "Address2");
                    add_fldvals3 = add_fldvals3.Replace("Address3_Mail", "Address3");
                    add_fldvals3 = add_fldvals3.Replace("Address4_Mail", "Address4");
                    add_fldvals3 = add_fldvals3.Replace("Address5_Mail", "Address5");
                    add_fldvals3 = add_fldvals3.Replace("Address6_Mail", "Address6");
                    add_fldvals3 = add_fldvals3.Replace("ZipCode_Mail", "ZipCodeSeqId");
                    add_fldvals3 = add_fldvals3.Replace("CountryCode_Mail", "CountryCode");
                    add_fldvals3 = add_fldvals3.Replace("POBox_Mail", "POBox");
                    add_fldvals3 = add_fldvals3.Replace("POBoxDistrict_Mail", "POBoxDistrict");
                    add_fldvals3 = add_fldvals3.Replace("AddressType_Mail", "AddressType");
                    add_fldvals3 = add_fldvals3.Replace("AddressRemarks_Mail", "Remarks");
                    add_fldvals3 = add_fldvals3.Replace("PreferredMailAdd_Mail", "PreferredMailAdd");
                    add_fldvals3 = add_fldvals3.Replace("PostalCode_Mail", "PostalCode");
                    add_fldvals3 = add_fldvals3.Replace("RemZip1Code_Mail", "ZipCode");
                    add_fldvals3 = add_fldvals3.Replace("CIFClass_Mail", "CIFClass");
                    add_fldvals3 = add_fldvals3.Replace("Address3k_Mail", "Address3k");
                    add_fldvals3 = add_fldvals3.Replace("AddSeqID_Mail", "SEQ_ID");
                }

				if (con_fldvals != null)
				{
					if (con_fldvals.Contains("ContactRemarks"))
					{
						con_fldvals = con_fldvals.Replace("ContactRemarks", "Remarks");
					}
                    //if (con_fldvals.Contains("CountryCodeSEQID"))
                    //{
                    //    con_fldvals = con_fldvals.Replace("CountryCodeSEQID", "CountryCode");
                    //}
				}

                if (con_fldvals2 != null)
                {
                    con_fldvals2 = con_fldvals2 + "CIFClass~" + cifclass + "|";

                    //if (con_fldvals2.Contains("CountryCodeSEQID"))
                    //{
                    //    con_fldvals2 = con_fldvals2.Replace("CountryCodeSEQID", "CountryCode");
                    //}
                }

				if (occ_fldvals != null)
				{
					if (occ_fldvals.Contains("OccupationRemarks"))
					{
						occ_fldvals = occ_fldvals.Replace("OccupationRemarks", "Remarks");
					}					
				}

				if (ids_fldvals != null)
				{
					ids_fldvals = ids_fldvals.Replace("IDRemarks", "Remarks");
					ids_fldvals = ids_fldvals.Replace("IDExpiryDate", "ExpiryDate");
                    ids_fldvals = ids_fldvals.Replace("PlaceOfIssuance", "PlaceOfIssuance");
				}

                if (ids_fldvals2 != null)
                {
                    ids_fldvals2 = ids_fldvals2.Replace("IDRemarks2", "Remarks");
                    ids_fldvals2 = ids_fldvals2.Replace("IDExpiryDate2", "ExpiryDate");
                    ids_fldvals2 = ids_fldvals2.Replace("IDNo2", "IDNo");
                    ids_fldvals2 = ids_fldvals2.Replace("IDType2", "IDType");
                    ids_fldvals2 = ids_fldvals2.Replace("VisaType2", "VisaType");
                    ids_fldvals2 = ids_fldvals2.Replace("IDClassification2", "IDClassification");
                    //Alyssa 06/16/2014
                    ids_fldvals2 = ids_fldvals2.Replace("PlaceOfIssuance2", "PlaceOfIssuance");
                }
                                
                if (ben_fldvals != null)
                {
                    ben_fldvals = ben_fldvals.Replace("ProdBranchCode", "BranchCode");
                    ben_fldvals = ben_fldvals.Replace("ProdCurrency", "Currency");
                    ben_fldvals = ben_fldvals.Replace("ProdAccountType", "AccountType");
                }

				if (mas_fldvals != null)
				{
                    DateTime dt = new irsTimeZone().branchLocalTime(get_client_data().Location);
					
					if (fxn == Function_Name.INSERT)
					{
						if (!mas_fldvals.Contains("DateCreated")) mas_fldvals += "DateCreated~" + dt + '|';
						if (!mas_fldvals.Contains("CreatedBy") && !mas_fldvals.Contains("UserID")) mas_fldvals += "CreatedBy~" + get_client_data().UserId + '|';
					}
					if (fxn == Function_Name.UPDATE)
					{
						if (!mas_fldvals.Contains("DateModified")) mas_fldvals += "DateModified~" + dt + '|';
						if (!mas_fldvals.Contains("ModifiedBy") && !mas_fldvals.Contains("UserID")) mas_fldvals += "ModifiedBy~" + get_client_data().UserId + '|';
					}
				}
				
                if (van_fldvals != null)
                {
                    DateTime dt2 = new irsTimeZone().branchLocalTime(get_client_data().Location);
					
					if (fxn == Function_Name.ADD_VAN)
					{
                        if (!van_fldvals.Contains("DateCreated")) van_fldvals += "DateCreated~" + dt2 + '|';
					}
					if (fxn == Function_Name.UPDATE_VAN)
					{
                        if (!van_fldvals.Contains("DateModified")) van_fldvals += "DateModified~" + dt2 + '|';
					}
                }

                if (notes_fldvals != null)
                {
                    DateTime dt3 = new irsTimeZone().branchLocalTime(get_client_data().Location);

                    if (fxn == Function_Name.ADD_NOTES)
                    {
                        if (!notes_fldvals.Contains("DateCreated")) notes_fldvals += "DateCreated~" + dt3 + '|';
                        if (!notes_fldvals.Contains("Teller")) notes_fldvals += "Teller~" + get_teller_name(get_client_data().UserId) + '|';
                    }
                }
				
				return true;
			}
			catch (Exception e)
			{
				error_msg = e.Message;
				return false;
			}						
		}

		public irsClientData get_client_data()
		{
			return ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
		}

		public bool check_mandatory_fields(irsClientEnrollment.Function_Name fxn, string plist)
		{
			/*list of mandatory fields per table*/
			mas_reqflds = new string[] { "CIFType", "BranchCode", "LastName" };
			cls_reqflds = new string[] { "CIFClass" };

			/*
			 * use dynamic validation instead
			 * car_reqflds = new string[] { "CardNo", "CardName", "BankCode", "CardType", "IssueDate", "ExpiryDate" };
			 *
			 * 
		     * account no is not required
			 * acc_reqflds = new string[] { "AccountNo", "AccountType", "Currency" }; 
			 */
			
			if (fxn == Function_Name.INSERT)
			{
				if (cls_fldvals == null)
				{
					missing_fields += cls_reqflds[0];
				}
			}			
			if (mas_fldvals != null)
			{
				for (int j = 0; j < mas_reqflds.Length; j++)
				{
					if (!mas_fldvals.Contains(mas_reqflds[j])) missing_fields += mas_reqflds[j] + ", ";
				}
			}

			if (missing_fields != null)
			{
				missing_fields = missing_fields.Substring(0, missing_fields.Length - 2);
				return false;
			} 

			return true;
		}

		public void INSERT_FLAG(bool flag)
		{
			this._INSERT_FLAG = flag;
		}

		public bool execute_fxn(irsClientEnrollment.Function_Name fxn, string plist, int cif_class, bool update)
		{
			string[] tbls = null;
			string fldvals = null;

			if (update == false)
			{
				if (cif_class == 1)
				{
                    tbls = new string[] { "cif_mas", "cif_cls", "cif_add", "cif_add2", "cif_add3", "cif_con", "cif_con2", "cif_occ", "cif_ids", "cif_ids2", "cif_oth", "cif_acc", "cif_car", "cif_rem", "cif_van", "cif_notes" };
				}
				else if (cif_class == 2)
				{
                    tbls = new string[] { "cif_mas", "cif_cls", "cif_add", "cif_add2", "cif_add3", "cif_con", "cif_con2", "cif_occ", "cif_ids", "cif_ids2", "cif_oth", "cif_acc", "cif_car", "cif_ben", "cif_van", "cif_notes" };
				}
			}
			else
			{
				if (cif_class == 1)
				{
                    tbls = new string[] { "cif_cls", "cif_add", "cif_add2", "cif_add3", "cif_con", "cif_con2", "cif_occ", "cif_ids", "cif_ids2", "cif_oth", "cif_acc", "cif_car", "cif_van", "cif_notes" };
				}
				else if (cif_class == 2)
				{
                    tbls = new string[] { "cif_cls", "cif_add", "cif_add2", "cif_add3", "cif_con", "cif_con2", "cif_occ", "cif_ids", "cif_ids2", "cif_oth", "cif_acc", "cif_car", "cif_van", "cif_notes" };
				}
			}			

			for (int h = 0; h < tbls.Length; h++)
			{
				switch (tbls[h])
				{
					case "cif_mas": { if (mas_fldvals == null) continue; else fldvals = mas_fldvals; break; }
					case "cif_cls": { if (cls_fldvals == null) continue; else fldvals = cls_fldvals; break; }
					case "cif_add": { if (add_fldvals == null) continue; else fldvals = add_fldvals; break; }
                    case "cif_add2": { if (add_fldvals2 == null) continue; else fldvals = add_fldvals2; break; }
                    case "cif_add3": { if (add_fldvals3 == null) continue; else fldvals = add_fldvals3; break; }
					case "cif_con": { if (con_fldvals == null) continue; else fldvals = con_fldvals; break; }
					case "cif_con2":{ if (con_fldvals2== null) continue; else fldvals = con_fldvals2;break; }
					case "cif_occ": { if (occ_fldvals == null) continue; else fldvals = occ_fldvals; break; }
					case "cif_ids": { if (ids_fldvals == null) continue; else fldvals = ids_fldvals; break; }
					case "cif_ids2":{ if (ids_fldvals2== null) continue; else fldvals = ids_fldvals2;break; }
					case "cif_oth": { if (oth_fldvals == null) continue; else fldvals = oth_fldvals; break; }
					case "cif_acc": { if (acc_fldvals == null) continue; else fldvals = acc_fldvals; break; }
					case "cif_car": { if (car_fldvals == null) continue; else fldvals = car_fldvals; break; }
					case "cif_rem": { if (rem_fldvals == null) continue; else fldvals = rem_fldvals; break; }
					case "cif_ben": { if (ben_fldvals == null) continue; else fldvals = ben_fldvals; break; }
                    case "cif_van": { if (van_fldvals == null) continue; else fldvals = van_fldvals; break; }
                    case "cif_notes": { if (notes_fldvals == null) continue; else fldvals = notes_fldvals; break; }
					default: break;
				}

				for (int h1 = 0; h1 < (fldvals.Split('%').Length); h1++)
				{
					if (fxn == Function_Name.INSERT)
					{
						if (this.insert_record(tbls[h]) == true)
						{
							this.INSERT_FLAG(true);
							if (this.commit_record(tbls[h]) == false) return false;
						}
						else return false;
					}
					else
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool do_cif(irsClientEnrollment.Function_Name fxn, string plist, int cif_class)
		{
			if (fxn == Function_Name.INSERT)
			{
				if (this.check_mandatory_fields(fxn, plist) == false)
				{
					error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
					return false;
				}
				try
				{
					return this.execute_fxn(fxn, plist, cif_class, false);
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
			else if (fxn == Function_Name.UPDATE)
			{
				if (this.check_mandatory_fields(fxn, plist) == false)
				{
					error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
					return false;
				}
				try
				{
					if (this.update_master_record() == true)
					{
						return this.update_others(plist);
					}
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}

				return false;
			}
			else if (fxn == Function_Name.ADD_ACCOUNT)
			{
				try
				{
					if (this.insert_record("cif_acc") == true)
					{
						this.INSERT_FLAG(true);
						if (this.commit_record("cif_acc") == true) return true;
						else return false;
					}
					else return false;
				}
				catch(Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
            else if (fxn == Function_Name.ADD_IDS)
            {
                try
                {
                    if (this.insert_record("cif_ids") == true)
                    {
                        this.INSERT_FLAG(true);
                        if (this.commit_record("cif_ids") == true) return true;
                        else return false;
                    }
                    else return false;
                }
                catch (Exception e)
                {
                    error_msg = e.Message; return false;
                }
            }
            else if (fxn == Function_Name.ADD_VAN)
            {
                try
                {
                    if (this.insert_record("cif_van") == true)
                    {
                        this.INSERT_FLAG(true);
                        if (this.commit_record("cif_van") == true) return true;
                        else return false;
                    }
                    else return false;
                }
                catch (Exception e)
                {
                    error_msg = e.Message; return false;
                }
            }
            else if (fxn == Function_Name.ADD_NOTES)
            {
                try
                {
                    if (this.insert_record("cif_notes") == true)
                    {
                        this.INSERT_FLAG(true);
                        if (this.commit_record("cif_notes") == true) return true;
                        else return false;
                    }
                    else return false;
                }
                catch (Exception e)
                {
                    error_msg = e.Message; return false;
                }
            }
			else if (fxn == Function_Name.ADD_CARD)
			{
				try
				{
					if (this.insert_record("cif_car") == true)
					{
						this.INSERT_FLAG(true);
						if (this.commit_record("cif_car") == true) return true;
						else return false;
					}
					else return false;
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
			else if (fxn == Function_Name.ADD_CIFCLASS)
			{
				if (this.check_mandatory_fields(fxn, plist) == false)
				{
					error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
					return false;
				}
				try
				{
					if (this.insert_record("cif_cls") == true)
					{
						this.INSERT_FLAG(true);
						if (this.commit_record("cif_cls") == true) return true;
						else return false;
					}
					else return false;
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
            else if (fxn == Function_Name.ADD_ADDRESS)
            {
                try
                {
                    if (this.insert_record("cif_add") == true)
                    {
                        this.INSERT_FLAG(true);
                        if (this.commit_record("cif_add") == true) return true;
                        else return false;
                    }
                    else return false;
                }
                catch (Exception e)
                {
                    error_msg = e.Message; return false;
                }
            }
			else if (fxn == Function_Name.ADD_BENE)
			{
				if (this.check_mandatory_fields(fxn, plist) == false)
				{
					error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
					return false;
				}
				try
				{
					if (this.insert_record("cif_ben") == true)
					{
						this.INSERT_FLAG(true);
						if (this.commit_record("cif_ben") == true) return true;
						else return false;
					}
					else return false;
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
			else if (fxn == Function_Name.UPDATE_BENE)
			{
				if (this.check_mandatory_fields(fxn, plist) == false)
				{
					error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
					return false;
				}
				try
				{
					return this.update_other_records(ben_seqid, "cif_ben");
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
			else if (fxn == Function_Name.UPDATE_ACCT)
			{
				if (this.check_mandatory_fields(fxn, plist) == false)
				{
					error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
					return false;
				}
				try
				{
					this.INSERT_FLAG(false);
					return this.update_other_records(acctseqid, "cif_acc");
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
			else if (fxn == Function_Name.UPDATE_CARD)
			{
				if (this.check_mandatory_fields(fxn, plist) == false)
				{
					error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
					return false;
				}
				try
				{
					this.INSERT_FLAG(false);
					return this.update_other_records(cardseqid, "cif_car");
				}
				catch (Exception e)
				{
					error_msg = e.Message; return false;
				}
			}
            else if (fxn == Function_Name.UPDATE_IDS)
            {
                if (this.check_mandatory_fields(fxn, plist) == false)
                {
                    error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
                    return false;
                }
                try
                {
                    this.INSERT_FLAG(false);
                    return this.update_other_records(idsseqid, "cif_ids");
                }
                catch (Exception e)
                {
                    error_msg = e.Message; return false;
                }
            }
            else if (fxn == Function_Name.UPDATE_VAN)
            {
                if (this.check_mandatory_fields(fxn, plist) == false)
                {
                    error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
                    return false;
                }
                try
                {
                    this.INSERT_FLAG(false);
                    return this.update_other_records(vanseqid, "cif_van");
                }
                catch (Exception e)
                {
                    error_msg = e.Message; return false;
                }
            }
            else if (fxn == Function_Name.UPDATE_NOTES)
            {
                if (this.check_mandatory_fields(fxn, plist) == false)
                {
                    error_msg += "04 INCOMPLETE MANDATORY FIELDS: " + missing_fields;
                    return false;
                }
                try
                {
                    this.INSERT_FLAG(false);
                    return this.update_other_records(notesseqid, "cif_notes");
                }
                catch (Exception e)
                {
                    error_msg = e.Message; return false;
                }
            }
			else
			{
				error_msg = "Function name not recognized.";
				return false;
			}			
		}

		public string get_tbl_seq_id(string tbl_name)
		{
			try
			{
				sqlHelperWs commons = new sqlHelperWs();
				String sql = null;
                String sql2 = null;
                bool isAddress = false;

				string seq_id = null;
                string seq_id2 = null;

                if (tbl_name == "TBL_CIF_BENEFICIARIES")
                {
                    sql = "SELECT SEQ_ID FROM " + tbl_name + " WHERE CIFID = '" + e_cifid + "' and REMITTERCIFID = '" + get_value_from_table_fldvals(ben_fldvals, "remittercifid") + "'";
                }
                //if (tbl_name == "TBL_CIF_ADDRESS")
                //{
                //    sql = "SELECT SEQ_ID FROM " + tbl_name + " WHERE CIFID = '" + e_cifid + "' and CIFClass = " + cifclass;
                //    sql2 = "SELECT SEQ_ID FROM " + tbl_name + " WHERE CIFID = '" + e_cifid + "' and isnull(CIFClass,-1) = -1";
                //}
                else if (tbl_name.Length >= 15 && tbl_name.Substring(0, 15) == "TBL_CIF_ADDRESS")
                {
                    isAddress = true;
                    if (tbl_name == "TBL_CIF_ADDRESS" && addseqid_Home != null) seq_id = addseqid_Home;
                    else if (tbl_name == "TBL_CIF_ADDRESS_Com" && addseqid_Com != null) seq_id = addseqid_Com;
                    else if (tbl_name == "TBL_CIF_ADDRESS_Mail" && addseqid_Mail != null) seq_id = addseqid_Mail;
                }
                else
                {
                    sql = "SELECT SEQ_ID FROM " + tbl_name + " WHERE CIFID = '" + e_cifid + "'";
                }

                //if (tbl_name == "TBL_CIF_ADDRESS")
                //{
                //    DataSet ds1 = getDataSet(getSqlCommand(sql, false));///execScalar(sql).ToString();
                //    DataSet ds2 = getDataSet(getSqlCommand(sql2, false));

                //    if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
                //    {
                //        seq_id = ds1.Tables[0].Rows[0][0].ToString();
                //    }
                //    if (ds2 != null && ds2.Tables[0].Rows.Count > 0)
                //    {
                //        seq_id2 = ds2.Tables[0].Rows[0][0].ToString();
                //    }
                //    if (seq_id == null && seq_id2 != null)
                //    {
                //        seq_id = seq_id2;
                //    }
                //}
                //else
                //{
                //    seq_id = execScalar(sql).ToString();
                //}
                if (!isAddress)
                {
                    seq_id = execScalar(sql).ToString();
                }
				
				return seq_id;
			}
            catch //(Exception ex) // commented to remove warning
			{ return null; }			
		}

		public int get_mobile_seqid()
		{
			sqlHelperWs commons = new sqlHelperWs();

            return Convert.ToInt32(commons.execScalar("sp_internet_get_mobile_seqid", new string[] { e_cifid, cifclass }));
		}

		public int get_landline_seqid()
		{
			sqlHelperWs commons = new sqlHelperWs();

            return Convert.ToInt32(commons.execScalar("sp_internet_get_landline_seqid", new string[] { e_cifid, cifclass }));
		}

		public int get_id_seqid(int idno)
		{
			sqlHelperWs commons = new sqlHelperWs();
			string sp = null;

			if (idno == 1) sp = "sp_internet_get_id1_seqid";
			else		   sp = "sp_internet_get_id2_seqid";

			return Convert.ToInt32(commons.execScalar(sp, new string[] { e_cifid }));
		}

        public String get_classtype_seqid(int classType)
        {
            sqlHelperWs commons = new sqlHelperWs();
            String sql = null;
            string seq_id = null;
            /*the next line is not a stored procedure because table name is parameterized*/
            sql += "SELECT SEQ_ID FROM TBL_CIF_CLASS WHERE CIFID = '" + e_cifid + "' and CIFClass = " + classType.ToString();

            seq_id = execScalar(sql).ToString();
            return seq_id;
        }

        public String get_teller_name(String user_id)
        {
            sqlHelperWs commons = new sqlHelperWs();
            String sql = null;
            String TellerName = null;
            sql += "select b.FullName from tbl_users a, tbl_cif_master b where a.cifid = b.cifid and userid = '" + user_id + "'";

            // TellerName = execScalar(sql).ToString();
            // CHR when the result of db scalar access is null
            Object o = execScalar(sql);
            if (o != null && o != DBNull.Value)
                return TellerName;
            else
                return user_id;
        }

		public bool update_master_record()
		{
			sqlHelperWs commons = new sqlHelperWs();
			
			mas_seqid = (commons.execScalar("sp_cif_get_master_seqid", new string[] { e_cifid })).ToString();

			cif_mas.seq_id = mas_seqid;
			cif_mas.selectedRow = 0;
			if (cif_mas.update_record() == true)
			{
				this.INSERT_FLAG(false);
				return this.commit_record("cif_mas");
			}
			return false;
		}

		public bool update_others(string plist)
		{
			string[] cif_tbl	  = new string[] { "TBL_CIF_CARDS", "TBL_CIF_ADDRESS", "TBL_CIF_ADDRESS_Com", "TBL_CIF_ADDRESS_Mail", "TBL_CIF_CONTACTS", "TBL_CIF_CONTACTS", "TBL_CIF_CLASS", "TBL_CIF_OCCUPATIONS", "TBL_CIF_IDS", "TBL_CIF_IDS", "TBL_CIF_OTHER_IDS", "TBL_CIF_BENEFICIARIES", "TBL_CIF_REMITTERS", "TBL_CIF_VAN", "TBL_CIF_NOTES" };
			string[] tbls		  = new string[] { "cif_car", "cif_add", "cif_add2", "cif_add3", "cif_con", "cif_con2", "cif_cls", "cif_occ", "cif_ids", "cif_ids2", "cif_oth", "cif_ben", "cif_rem", "cif_van", "cif_notes" };
            object[] vars = new object[] { car_fldvals, add_fldvals, add_fldvals2, add_fldvals3, con_fldvals, con_fldvals2, cls_fldvals, occ_fldvals, ids_fldvals, ids_fldvals2, oth_fldvals, ben_fldvals, rem_fldvals, van_fldvals, notes_fldvals };
			string tbl_seqid	  = null;
			string mobile_seqid	  = null;
			string landline_seqid = null;
			string id1_seqid	  = null;
			string id2_seqid	  = null;

			for (int i = 0; i < cif_tbl.Length; i++)
			{
				if (vars[i] == null) continue;

				/*special cases for TBL_CIF_CONTACTS and TBL_CIF_IDS and TBL_CIF_CLASS*/
                tbl_seqid = null;
                if (cif_tbl[i] != "TBL_CIF_CONTACTS" && cif_tbl[i] != "TBL_CIF_IDS" && cif_tbl[i] != "TBL_CIF_CLASS")
				{
					tbl_seqid = this.get_tbl_seq_id(cif_tbl[i]);
				}

				if (tbl_seqid != null)
				{
                    if (cif_tbl[i] != "TBL_CIF_CONTACTS" && cif_tbl[i] != "TBL_CIF_IDS" && cif_tbl[i] != "TBL_CIF_CLASS")
					{
						if (this.update_other_records(tbl_seqid, tbls[i]) == false) return false;
					}
				}
                else if (cif_tbl[i] != "TBL_CIF_CONTACTS" && cif_tbl[i] != "TBL_CIF_IDS" && cif_tbl[i] != "TBL_CIF_CLASS")
				{
					if (this.insert_record(tbls[i])){this.commit_record(tbls[i]);}
				}

				if (cif_tbl[i] == "TBL_CIF_CONTACTS")
				{
					mobile_seqid   = this.get_mobile_seqid().ToString();
					landline_seqid = this.get_landline_seqid().ToString();

					if (tbls[i] == "cif_con")
					{
						if (mobile_seqid != null && mobile_seqid != "0")
						{
							if (this.update_other_records(mobile_seqid, "cif_con") == false) return false;
						}
						else
						{
							if (this.insert_record("cif_con")) { this.commit_record("cif_con"); }
						}
					}
					if (tbls[i] == "cif_con2")
					{
						if (landline_seqid != null && landline_seqid != "0")
						{
							if (this.update_other_records(landline_seqid, "cif_con2") == false) return false;
						}
						else
						{
							if (this.insert_record("cif_con2")) { this.commit_record("cif_con2"); }
						}
					}
				}
                if (cif_tbl[i] == "TBL_CIF_CLASS")
                {
                    int CIFCls = 0;

                    try { CIFCls = int.Parse(cls_fldvals.Split('~')[1].Replace("|", "")); }
                    catch { }

                    String cls_seq_id = this.get_classtype_seqid(CIFCls).ToString();

                    if (cls_seq_id != null && cls_seq_id != "0")
                    {
                        if (this.update_other_records(cls_seq_id, "cif_cls") == false) return false;
                    }
                    else
                    {
                        if (this.insert_record("cif_cls")) { this.commit_record("cif_cls"); }
                    }
                }
				if (cif_tbl[i] == "TBL_CIF_IDS") 
				{
					id1_seqid = this.get_id_seqid(1).ToString();
					id2_seqid = this.get_id_seqid(2).ToString();

					if (tbls[i] == "cif_ids")
					{
						if (id1_seqid != null && id1_seqid != "0")
						{
							if (this.update_other_records(id1_seqid, "cif_ids") == false) return false;
						}
						else
						{
							if (this.insert_record("cif_ids")) { this.commit_record("cif_ids"); }
						}
					}
					if (tbls[i] == "cif_ids2")
					{
						if (id2_seqid != null && id2_seqid != "0")
						{
							if (this.update_other_records(id2_seqid, "cif_ids2") == false) return false;
						}
						else
						{
							if (this.insert_record("cif_ids2")) { this.commit_record("cif_ids2"); }
						}
					}
				}				
			}
			return true;
		}
		
		/* updates other tables except TBL_CIF_MASTER */
		public bool update_other_records(string seq_id, string class_name)
		{
			try
			{
				switch (class_name)
				{
					case "cif_acc":
					{
						cif_acc.seq_id = seq_id; cif_acc.selectedRow = 0;
						if (cif_acc.update_record() == true) { return this.commit_record("cif_acc"); } return false;
					} 
					case "cif_add": 
					{
						cif_add.seq_id = seq_id; cif_add.selectedRow = 0;
						if (cif_add.update_record() == true) { return this.commit_record("cif_add"); } return false;
					}
                    case "cif_add2": 
					{
						cif_add.seq_id = seq_id; cif_add.selectedRow = 0;
                        if (cif_add.update_record() == true) { return this.commit_record("cif_add2"); } return false;
					}
                    case "cif_add3": 
					{
						cif_add.seq_id = seq_id; cif_add.selectedRow = 0;
                        if (cif_add.update_record() == true) { return this.commit_record("cif_add3"); } return false;
					}   
					case "cif_con":
					{
						cif_con.seq_id = seq_id; cif_con.selectedRow = 0;
                        if (cif_con.update_record() == true) { return this.commit_record("cif_con"); } return false;
					}
					case "cif_con2":
					{
						cif_con.seq_id = seq_id; cif_con.selectedRow = 0;
                        if (cif_con.update_record() == true) { return this.commit_record("cif_con2"); } return false;
					}
                    case "cif_cls":
                    {
                        cif_cls.seq_id = seq_id; cif_cls.selectedRow = 0;
                        if (cif_cls.update_record() == true) { return this.commit_record("cif_cls"); } return false;
                    }
					case "cif_occ":
					{
						cif_occ.seq_id = seq_id; cif_occ.selectedRow = 0;
						if (cif_occ.update_record() == true) { return this.commit_record("cif_occ"); } return false;
					}
					case "cif_ids":
					{
						cif_ids.seq_id = seq_id; cif_ids.selectedRow = 0;
						if (cif_ids.update_record() == true) { return this.commit_record("cif_ids"); } return false;
					}
					case "cif_ids2":
					{
						cif_ids.seq_id = seq_id; cif_ids.selectedRow = 0;
						if (cif_ids.update_record() == true) { return this.commit_record("cif_ids2"); } return false;
					}
					case "cif_oth":
					{
						cif_oth.seq_id = seq_id; cif_oth.selectedRow = 0;
						if (cif_oth.update_record() == true) { return this.commit_record("cif_oth"); } return false;
					}
					case "cif_car":
					{
						cif_car.seq_id = seq_id; cif_car.selectedRow = 0;
						if (cif_car.update_record() == true) { return this.commit_record("cif_car"); } return false;
					}
					case "cif_ben":
					{
						cif_ben.seq_id = seq_id; cif_ben.selectedRow = 0;
						if (cif_ben.update_record() == true) { return this.commit_record("cif_ben"); } return false;
					}
					case "cif_rem":
					{
						cif_rem.seq_id = seq_id; cif_rem.selectedRow = 0;
						if (cif_rem.update_record() == true) { return this.commit_record("cif_rem"); } return false;
					}
                    case "cif_van":
                    {
                        cif_van.seq_id = seq_id; cif_van.selectedRow = 0;
                        if (cif_van.update_record() == true) { return this.commit_record("cif_van"); } return false;
                    }
                    case "cif_notes":
                    {
                        cif_notes.seq_id = seq_id; cif_notes.selectedRow = 0;
                        if (cif_notes.update_record() == true) { return this.commit_record("cif_notes"); } return false;
                    }
					default: break;
				}
				return false;
			}
            catch
			{ return false; }
		}
		
		public bool insert_record(string class_name)
		{
			try
			{
				switch (class_name)
				{
					case "cif_mas": if (cif_mas.insert_record() == false) { error_msg += cif_mas.messages + '\n'; return false; } return true;
					case "cif_cls": if (cif_cls.insert_record() == false) { error_msg += cif_cls.messages + '\n'; return false; } return true;
					
                    case "cif_add": if (cif_add.insert_record() == false) { error_msg += cif_add.messages + '\n'; return false; } return true;
                    case "cif_add2": if (cif_add.insert_record() == false) { error_msg += cif_add.messages + '\n'; return false; } return true;
                    case "cif_add3": if (cif_add.insert_record() == false) { error_msg += cif_add.messages + '\n'; return false; } return true;

					case "cif_con": if (cif_con.insert_record() == false) { error_msg += cif_con.messages + '\n'; return false; } return true;
					case "cif_con2":if (cif_con.insert_record() == false) { error_msg += cif_con.messages + '\n'; return false; } return true;
					case "cif_ben": if (cif_ben.insert_record() == false) { error_msg += cif_ben.messages + '\n'; return false; } return true;
					case "cif_occ": if (cif_occ.insert_record() == false) { error_msg += cif_occ.messages + '\n'; return false; } return true;
					case "cif_ids": if (cif_ids.insert_record() == false) { error_msg += cif_ids.messages + '\n'; return false; } return true;
					case "cif_ids2":if (cif_ids.insert_record() == false) { error_msg += cif_ids.messages + '\n'; return false; } return true;
					case "cif_oth": if (cif_oth.insert_record() == false) { error_msg += cif_oth.messages + '\n'; return false; } return true;
					case "cif_acc": if (cif_acc.insert_record() == false) { error_msg += cif_acc.messages + '\n'; return false; } return true;
					case "cif_car": if (cif_car.insert_record() == false) { error_msg += cif_car.messages + '\n'; return false; } return true;
					case "cif_rem": if (cif_rem.insert_record() == false) { error_msg += cif_rem.messages + '\n'; return false; } return true;
                    case "cif_van": if (cif_van.insert_record() == false) { error_msg += cif_van.messages + '\n'; return false; } return true;
                    case "cif_notes": if (cif_notes.insert_record() == false) { error_msg += cif_notes.messages + '\n'; return false; } return true;

					default: break; 
				} 
				return false;
			}
			catch (Exception e)
			{
				error_msg = e.Message; return false;
			}
		}

        public enum VAPTTableName
        {
            MASTER,
            CLASS,
            ADDRESS,
            CONTACTS,
            IDS,
            OCCUPATIONS,
            SPOUSE,
            BENEFICIARY,
            ACCOUNTS,
            VAN,
            NOTES,
            OTHERID,
            CARDS,
            REMITTERS,
            MERCHANTS,
            MERCHANTINFO,
            CHARGEMASTER,
            CHARGES,
            CHARGESTIER
        }

        /* VAPT 20180222 */
        public string getLookupEquivalent(string search, string value, string accountType)
        {
            string ret = string.Empty;
            string v = string.Empty;
            if (value != string.Empty)
            {
                switch (search.ToUpper())
                {
                    case "CIFID":
                        ret = search + "=" + value + " ~ ";
                        break;

                    case "DELETED":
                        ret = string.Empty;
                        break;

                    case "DATECREATED":
                        ret = string.Empty;
                        break;

                    case "CREATEDBY":
                        ret = string.Empty;
                        break;

                    case "GUID":
                        ret = string.Empty;
                        break;

                    case "CURRENCY":
                        v = new irsInternetPage().getLookupValue(search, "FldValue", value, "FldCode");
                        if (v != string.Empty && v.Trim() != "null")
                        {
                            ret = search + "=" + v + " ~ ";
                        }
                        break;

                    case "TITLE":
                        v = new irsInternetPage().getLookupValue("DisplayTitle", "FldValue", value, "FldCode");
                        if (v != string.Empty && v.Trim() != "null")
                        {
                            ret = search + "=" + v + " ~ ";
                        }
                        break;

                    case "IMPLEMENTATIONCODE":
                        v = new irsInternetPage().getLookupValue("ImplementationType", "FldValue", value, "FldDescription");
                        if (v != string.Empty && v.Trim() != "null")
                        {
                            ret = search + "=" + v + " ~ ";
                        }
                        break;

                    case "STATUS":
                        if (accountType == "1")
                        {
                            v = new irsInternetPage().getLookupValue("AccountStatusST", "FldValue", value, "FldDescription");
                        }
                        else
                        {
                            if (accountType == "1")
                            {
                                v = new irsInternetPage().getLookupValue("AccountStatusIM", "FldValue", value, "FldDescription");
                            }
                        }
                        if (v != string.Empty && v.Trim() != "null")
                        {
                            ret = search + "=" + v + " ~ ";
                        }
                        break;

                    case "COUNTRYCODE":
                        v = new irsInternetPage().getLookupValue("Country", "FldValue", value, "FldDescription");
                        if (v != string.Empty && v.Trim() != "null")
                        {
                            ret = search + "=" + v + " ~ ";
                        }
                        break;

                    case "CONTACTTYPE":
                    case "PREFERREDCONTACT":
                        v = new irsInternetPage().getLookupValue("ContactInfoType", "FldValue", value, "FldDescription");
                        if (v != string.Empty && v.Trim() != "null")
                        {
                            ret = search + "=" + v + " ~ ";
                        }
                        break;

                    default:
                        try
                        {
                            v = new irsInternetPage().getLookupValue(search, "FldValue", value, "FldDescription");
                            if (v != string.Empty && v.Trim() != "null")
                            {
                                ret = search + "=" + v + " ~ ";
                            }
                            if (v.Trim() == string.Empty)
                            {
                                if (value != string.Empty)
                                {
                                    ret = search + "=" + value + " ~ ";
                                }
                            }
                        }
                        catch
                        {
                            if (value.Trim() != "null")
                            {
                                ret = search + "=" + value.Trim().Replace("12:00:00 AM", "") + " ~ ";
                            }
                        }
                        break;
                }
            }
            return ret;
        }

        /* VAPT 20180222 */
        public string getVACIF(string seqID, VAPTTableName vaptTableName, string[] compare)
        {
            base.EnforceConstraint = false;
            string oldValue = string.Empty;
            string newValue = string.Empty;
            string systemValue = string.Empty;
            string ret = string.Empty;
            string[] arrCompare;
            int i = 0;
            int j = 0;
            int k = 0;
            int l = 0;
            string val = string.Empty;
            string accountType = string.Empty;
            string[] arrVal;

            /* RFC-31620 */
            if (seqID == null)
            {
                seqID = "0";
            }

            switch (vaptTableName)
            {
                case VAPTTableName.MASTER:
                    getUpdateDS("SELECT * FROM TBL_CIF_MASTER WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.CLASS:
                    getUpdateDS("SELECT * FROM TBL_CIF_CLASS WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.ADDRESS:
                    for (l = 0; l <= (compare.Length - 1); l++)
                    {
                        arrVal = compare.GetValue(l).ToString().Split('~');
                        if (arrVal.GetValue(0).ToString().ToUpper() == "ADDRESSTYPE")
                        {
                            val = arrVal.GetValue(1).ToString();
                        }
                    }
                    getUpdateDS("SELECT * FROM TBL_CIF_ADDRESS WHERE SEQ_ID = " + seqID + " AND AddressType = '" + val + "'");
                    break;

                case VAPTTableName.CONTACTS:
                    for (l = 0; l <= (compare.Length - 1); l++)
                    {
                        arrVal = compare.GetValue(l).ToString().Split('~');
                        if (arrVal.GetValue(0).ToString().ToUpper() == "CONTACTTYPE")
                        {
                            val = arrVal.GetValue(1).ToString();
                        }
                    }
                    getUpdateDS("SELECT * FROM TBL_CIF_CONTACTS WHERE SEQ_ID = " + seqID + " AND ContactType = '" + val + "'");
                    break;

                case VAPTTableName.IDS:
                    for (l = 0; l <= (compare.Length - 1); l++)
                    {
                        arrVal = compare.GetValue(l).ToString().Split('~');
                        if (arrVal.GetValue(0).ToString().ToUpper() == "IDTYPE")
                        {
                            val = arrVal.GetValue(1).ToString();
                        }
                    }
                    getUpdateDS("SELECT * FROM TBL_CIF_IDS WHERE SEQ_ID = " + seqID + " AND IDType = '" + val + "'");
                    break;

                case VAPTTableName.OCCUPATIONS:
                    getUpdateDS("SELECT * FROM TBL_CIF_OCCUPATIONS WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.SPOUSE:
                    getUpdateDS("SELECT * FROM TBL_CIF_SPOUSE WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.BENEFICIARY:
                    getUpdateDS("SELECT * FROM TBL_CIF_BENEFICIARIES WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.ACCOUNTS:
                    getUpdateDS("SELECT * FROM TBL_CIF_ACCOUNTS WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.VAN:
                    getUpdateDS("SELECT * FROM TBL_CIF_VAN WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.NOTES:
                    getUpdateDS("SELECT * FROM TBL_CIF_NOTES WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.OTHERID:
                    getUpdateDS("SELECT * FROM TBL_CIF_OTHER_IDS WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.CARDS:
                    getUpdateDS("SELECT * FROM TBL_CIF_CARDS WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.REMITTERS:
                    getUpdateDS("SELECT * FROM TBL_CIF_REMITTERS WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.MERCHANTS:
                    getUpdateDS("SELECT * FROM TBL_CIF_MERCHANTS WHERE SEQ_ID = " + seqID);
                    break;

                case VAPTTableName.MERCHANTINFO:
                    getUpdateDS("SELECT * FROM TBL_CIF_MERCHANTS_INFO WHERE SEQ_ID = " + seqID);
                    break;
            }

            try
            {
                if (hasRows)
                {
                    selectedRow = 0;
                    for (i = 0; i <= (table.Rows.Count - 1); i++)
                    {
                        accountType = string.Empty;
                        for (j = 0; j <= (table.Columns.Count - 1); j++)
                        {
                            for (k = 0; k <= (compare.Length - 1); k++)
                            {
                                arrCompare = compare.GetValue(k).ToString().Split('~');
                                if (arrCompare.GetValue(0).ToString().IndexOf("AccountType") >= 0)
                                {
                                    accountType = arrCompare.GetValue(1).ToString();
                                }
                                if (table.Columns[j].ToString().ToLower() == arrCompare.GetValue(0).ToString().ToLower())
                                {
                                    if (table.Rows[i][j] != DBNull.Value)
                                    {
                                        DateTime dt;
                                        if (DateTime.TryParse(table.Rows[i][j].ToString(), out dt))
                                        {
                                            if (table.Rows[i][j].ToString().IndexOf("12:00:00") > 0 && Convert.ToDateTime(table.Rows[i][j].ToString()).ToString("yyyy-MM-dd") != Convert.ToDateTime(arrCompare.GetValue(1).ToString()).ToString("yyyy-MM-dd"))
                                            {
                                                oldValue += this.getLookupEquivalent(table.Columns[j].ToString(), table.Rows[i][j].ToString(), accountType);
                                                newValue += this.getLookupEquivalent(table.Columns[j].ToString(), arrCompare.GetValue(1).ToString(), accountType);
                                            }
                                            else
                                            {
                                                if (table.Rows[i][j].ToString().IndexOf("12:00:00") < 0 && table.Rows[i][j].ToString().Trim().ToLower() != arrCompare.GetValue(1).ToString().Trim().ToLower())
                                                {
                                                    oldValue += this.getLookupEquivalent(table.Columns[j].ToString(), table.Rows[i][j].ToString(), accountType);
                                                    newValue += this.getLookupEquivalent(table.Columns[j].ToString(), arrCompare.GetValue(1).ToString(), accountType);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Double d;
                                            if (Double.TryParse(table.Rows[i][j].ToString(), out d))
                                            {
                                                try
                                                {
                                                    if (Convert.ToDouble(table.Rows[i][j].ToString()).ToString("#,###,###.00") != Convert.ToDouble(arrCompare.GetValue(1).ToString()).ToString("#,###,###.00"))
                                                    {
                                                        oldValue += this.getLookupEquivalent(table.Columns[j].ToString(), table.Rows[i][j].ToString(), accountType);
                                                        newValue += this.getLookupEquivalent(table.Columns[j].ToString(), arrCompare.GetValue(1).ToString(), accountType);
                                                    }
                                                }
                                                catch
                                                {
                                                    if (table.Rows[i][j].ToString().Trim().ToLower() != arrCompare.GetValue(1).ToString().Trim().ToLower())
                                                    {
                                                        oldValue += this.getLookupEquivalent(table.Columns[j].ToString(), table.Rows[i][j].ToString(), accountType);
                                                        newValue += this.getLookupEquivalent(table.Columns[j].ToString(), arrCompare.GetValue(1).ToString(), accountType);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (table.Rows[i][j].ToString().Trim().ToLower() != arrCompare.GetValue(1).ToString().Trim().ToLower())
                                                {
                                                    oldValue += this.getLookupEquivalent(table.Columns[j].ToString(), table.Rows[i][j].ToString(), accountType);
                                                    newValue += this.getLookupEquivalent(table.Columns[j].ToString(), arrCompare.GetValue(1).ToString(), accountType);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (arrCompare.GetValue(1).ToString().Trim() != string.Empty)
                                        {
                                            if (arrCompare.GetValue(1).ToString().ToLower() != "null")
                                            {
                                                oldValue += table.Columns[j].ToString() + "=~";
                                                newValue += this.getLookupEquivalent(table.Columns[j].ToString(), arrCompare.GetValue(1).ToString(), accountType);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (i = 0; i <= (compare.Length - 1); i++)
                    {
                        arrCompare = compare.GetValue(i).ToString().Split('~');
                        if (arrCompare.GetValue(0).ToString().IndexOf("AccountType") >= 0)
                        {
                            accountType = arrCompare.GetValue(1).ToString();
                        }
                        if (arrCompare.GetValue(0).ToString().ToUpper().IndexOf("SEQ_ID") >= 0 || arrCompare.GetValue(0).ToString().ToUpper().IndexOf("SEQID") >= 0)
                        {
                            systemValue += arrCompare.GetValue(0).ToString() + "=" + arrCompare.GetValue(1).ToString() + " ~ ";
                        }
                        else
                        {
                            newValue += this.getLookupEquivalent(arrCompare.GetValue(0).ToString(), arrCompare.GetValue(1).ToString(), accountType);
                        }
                    }
                }
            }
            catch { }
            ret = oldValue + "^" + newValue + "^" + systemValue;
            return ret;
        }

        public bool commit_record(string class_name)
		{
			string[] column_values = null;
			string cifid_val = null;
			string isDeleted = "Deleted~false|";

			if (cifid != null) cifid_val = "CIFID~" + cifid   + '|';
			else			   cifid_val = "CIFID~" + e_cifid + '|';

			switch (class_name)
			{
				case "cif_mas":
				{
                    Guid g = Guid.NewGuid();
					mas_fldvals += isDeleted;
					if (_INSERT_FLAG == true)
					{
                        mas_fldvals += "CIFGUID~" + g.ToString() + '|';
						mas_fldvals = "CIFID~|" + mas_fldvals.Substring(0, mas_fldvals.Length - 1);
						mas_fldvals = mas_fldvals.Replace("UserID", "CreatedBy");
					}
					else
					{
						mas_fldvals = mas_fldvals.Substring(0, mas_fldvals.Length - 1);
						mas_fldvals = mas_fldvals.Replace("UserID", "ModifiedBy");
					}

					column_values = mas_fldvals.Split('|');
					
					if (column_values != null && column_values.Length > 0)
					{
						if (cif_mas.mas_ins_flag == true)
						{
							if (cif_mas.is_existing(mas_fldvals) == true)
							{
								error_msg = "08 CUSTOMER RECORD ALREADY EXISTS!";
								return false;
							}
						}

						if (cif_mas.set_record(column_values) == false)
						{
							error_msg = cif_mas.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_mas.seq_id, VAPTTableName.MASTER, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_mas.commit_record() == false)
							{
								error_msg = cif_mas.messages; return false;
							}
                            if (_INSERT_FLAG == true)
                            {   
                                cifid = cif_mas.get_gcifid(g, get_client_data().Location.ToString(), get_client_data().UserId.ToString());
                            }
                            else
                            {
                                cifid = cif_mas.get_gcifid();
                            }

                            /* VAPT 20180222 */
                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF RECORD", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF RECORD", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }
                            return true;
						}
					}
					return false;
				}

				case "cif_cls":
				{
					if (cls_fldvals.Length > 0)
					{
						cls_fldvals = cifid_val + isDeleted + cls_fldvals.Substring(0, cls_fldvals.Length - 1);
						column_values = cls_fldvals.Split('|');

						if (column_values != null && column_values.Length > 0)
						{
							if (cif_cls.set_record(column_values) == false)
							{
								error_msg = cif_cls.messages; return false;
							}
							else
							{
                                /* VAPT 20180222 */
                                string CIF = string.Empty;
                                string[] arrCIF = null;
                                CIF = getVACIF(cif_cls.seq_id, VAPTTableName.CLASS, column_values);
                                arrCIF = CIF.Split('^');

                                if (cif_cls.commit_record() == false)
								{
									error_msg = cif_cls.messages; return false;
								}

                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CLASS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CLASS", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }
                                return true;
							}
						}
						return false;
					}
					return false;
				}

				case "cif_add":
				{
					if (add_fldvals.Length > 0)
					{
						add_fldvals = cifid_val + isDeleted + add_fldvals.Substring(0, add_fldvals.Length - 1);
						column_values = add_fldvals.Split('|');

						if (column_values != null && column_values.Length > 0)
						{
							if (cif_add.set_record(column_values) == false)
							{
								error_msg += cif_add.messages; return false;
							}
							else
							{
                                /* VAPT 20180222 */
                                string CIF = string.Empty;
                                string[] arrCIF = null;
                                CIF = getVACIF(cif_add.seq_id, VAPTTableName.ADDRESS, column_values);
                                arrCIF = CIF.Split('^');

                                if (cif_add.commit_record() == false)
								{
									error_msg = cif_add.messages; return false;
								}

                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF ADDRESS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF ADDRESS", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }

                                update_cif_preferred_address(cif_add.seq_id);
							    return true;
							}
						}
						return false;
					}
					return false;
				}

                case "cif_add2":
                {
                    if (add_fldvals2.Length > 0)
                    {
                        add_fldvals2 = cifid_val + isDeleted + add_fldvals2.Substring(0, add_fldvals2.Length - 1);
                        column_values = add_fldvals2.Split('|');

                        if (column_values != null && column_values.Length > 0)
                        {
                            if (cif_add.set_record(column_values) == false)
                            {
                                error_msg += cif_add.messages; return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                string CIF = string.Empty;
                                string[] arrCIF = null;
                                CIF = getVACIF(cif_add.seq_id, VAPTTableName.ADDRESS, column_values);
                                arrCIF = CIF.Split('^');

                                if (cif_add.commit_record() == false)
                                {
                                    error_msg = cif_add.messages; return false;
                                }

                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF ADDRESS 2", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF ADDRESS 2", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }

                                update_cif_preferred_address(cif_add.seq_id);
                                return true;
                            }
                        }
                        return false;
                    }
                    return false;
                }

                case "cif_add3":
                {
                    if (add_fldvals3.Length > 0)
                    {
                        add_fldvals3 = cifid_val + isDeleted + add_fldvals3.Substring(0, add_fldvals3.Length - 1);
                        column_values = add_fldvals3.Split('|');

                        if (column_values != null && column_values.Length > 0)
                        {
                            if (cif_add.set_record(column_values) == false)
                            {
                                error_msg += cif_add.messages; return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                string CIF = string.Empty;
                                string[] arrCIF = null;
                                CIF = getVACIF(cif_add.seq_id, VAPTTableName.ADDRESS, column_values);
                                arrCIF = CIF.Split('^');

                                if (cif_add.commit_record() == false)
                                {
                                    error_msg = cif_add.messages; return false;
                                }

                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF ADDRESS 3", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF ADDRESS 3", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }

                                update_cif_preferred_address(cif_add.seq_id);
                                return true;
                            }
                        }
                        return false;
                    }
                    return false;
                }

				case "cif_con":
				{
					con_fldvals = cifid_val + isDeleted + con_fldvals.Substring(0, con_fldvals.Length - 1);
					column_values = con_fldvals.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_con.set_record(column_values) == false)
						{
							error_msg += cif_con.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_con.seq_id, VAPTTableName.CONTACTS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_con.commit_record() == false)
							{
								error_msg = cif_con.messages; return false;
							}

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CONTACTS 1", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CONTACTS 1", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            update_cif_preferred_contacts(cif_con.seq_id);
							return true;
						}
					}
					return false;
				}

				case "cif_con2":
				{
					con_fldvals2 = cifid_val + isDeleted + con_fldvals2.Substring(0, con_fldvals2.Length - 1);
					column_values = con_fldvals2.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_con.set_record(column_values) == false)
						{
							error_msg += cif_con.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_con.seq_id, VAPTTableName.CONTACTS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_con.commit_record() == false)
							{
								error_msg = cif_con.messages; return false;
							}

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CONTACTS 2", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CONTACTS 2", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            update_cif_preferred_contacts(cif_con.seq_id);
							return true;
						}
					}
					return false;
				}

				case "cif_occ":
				{
					occ_fldvals = cifid_val + isDeleted + occ_fldvals.Substring(0, occ_fldvals.Length - 1);
					column_values = occ_fldvals.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_occ.set_record(column_values) == false)
						{
							error_msg += cif_occ.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_occ.seq_id, VAPTTableName.OCCUPATIONS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_occ.commit_record() == false)
							{
								error_msg = cif_occ.messages; return false;
							}

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF OCCUPATION", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF OCCUPATION", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
						}
					}
					return false;
				}

				case "cif_ids":
				{
					ids_fldvals = cifid_val + isDeleted + ids_fldvals.Substring(0, ids_fldvals.Length - 1);
					column_values = ids_fldvals.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_ids.set_record(column_values) == false)
						{
							error_msg += cif_ids.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_ids.seq_id, VAPTTableName.IDS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_ids.commit_record() == false)
							{
								error_msg = cif_ids.messages; return false;
							}

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF IDS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF IDS", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
						}
					}
					return false;
				}

                case "cif_ids2":
                {
                    ids_fldvals2 = cifid_val + isDeleted + ids_fldvals2.Substring(0, ids_fldvals2.Length - 1);
                    column_values = ids_fldvals2.Split('|');

                    if (column_values != null && column_values.Length > 0)
                    {
                        if (cif_ids.set_record(column_values) == false)
                        {
                            error_msg += cif_ids.messages; return false;
                        }
                        else
                        {
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_ids.seq_id, VAPTTableName.IDS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_ids.commit_record() == false)
                            {
                                error_msg = cif_ids.messages; return false;
                            }

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF IDS 2", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF IDS 2", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
                        }
                    }
                    return false;
                }

                case "cif_van":
                {
                    van_fldvals = cifid_val + isDeleted + van_fldvals.Substring(0, van_fldvals.Length - 1);
                    column_values = van_fldvals.Split('|');

                    if (column_values != null && column_values.Length > 0)
                    {
                        if (cif_van.set_record(column_values) == false)
                        {
                            error_msg += cif_van.messages; return false;
                        }
                        else
                        {
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_van.seq_id, VAPTTableName.VAN, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_van.commit_record() == false)
                            {
                                error_msg = cif_van.messages; return false;
                            }

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF VAN", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF VAN", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
                        }
                    }
                    return false;
                }

                case "cif_notes":
                {
                    notes_fldvals = cifid_val + notes_fldvals.Substring(0, notes_fldvals.Length - 1);
                    column_values = notes_fldvals.Split('|');

                    if (column_values != null && column_values.Length > 0)
                    {
                        if (cif_notes.set_record(column_values) == false)
                        {
                            error_msg += cif_notes.messages; return false;
                        }
                        else
                        {
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_notes.seq_id, VAPTTableName.NOTES, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_notes.commit_record() == false)
                            {
                                error_msg = cif_notes.messages; return false;
                            }
                            notesseqid = cif_notes.seq_id;

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF NOTES", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF NOTES", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
                        }
                    }
                    return false;
                }
				
				case "cif_oth":
				{
					oth_fldvals = cifid_val + oth_fldvals.Substring(0, oth_fldvals.Length - 1);
                    if (oth_fldvals.IndexOf("OtherIDs") != -1)
                    {
                        column_values = oth_fldvals.Split('|');

                        if (column_values != null && column_values.Length > 0)
                        {
                            if (cif_oth.set_record(column_values) == false)
                            {
                                error_msg += cif_oth.messages; return false;
                            }
                            else
                            {
                                /* VAPT 20180222 */
                                string CIF = string.Empty;
                                string[] arrCIF = null;
                                CIF = getVACIF(cif_oth.seq_id, VAPTTableName.OTHERID, column_values);
                                arrCIF = CIF.Split('^');

                                if (cif_oth.commit_record() == false)
                                {
                                    error_msg = cif_oth.messages; return false;
                                }

                                if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF OTHER ID", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                                else
                                {
                                    if (arrCIF.GetValue(1).ToString().Length > 0)
                                    {
                                        new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF OTHER ID", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                    }
                                }

                                return true;
                            }
                        }
                        return false;
                    }
					return true;
				}

				case "cif_acc":
				{
					acc_fldvals = cifid_val + isDeleted + acc_fldvals.Substring(0, acc_fldvals.Length - 1);
					column_values = acc_fldvals.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_acc.set_record(column_values) == false)
						{
							error_msg += cif_acc.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_acc.seq_id, VAPTTableName.ACCOUNTS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_acc.commit_record() == false)
							{
								error_msg = cif_acc.messages;
								acc_seqid = (-1).ToString();
								return false;
							}
							acc_seqid = cif_acc.seq_id;

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF ACCOUNTS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF ACCOUNTS", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
						}
					}
					return false;
				}

				case "cif_car":
				{
					car_fldvals = cifid_val + isDeleted + car_fldvals.Substring(0, car_fldvals.Length - 1);
					column_values = car_fldvals.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_car.set_record(column_values) == false)
						{
							error_msg += cif_car.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_car.seq_id, VAPTTableName.CARDS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_car.commit_record() == false)
							{
								error_msg = cif_car.messages; return false;
							}
							car_seqid = cif_car.seq_id;

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED CIF CARDS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED CIF CARDS", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
						}
					}
					return false;
				}

				case "cif_rem":
				{
					rem_fldvals = cifid_val + rem_fldvals.Substring(0, rem_fldvals.Length - 1);
					column_values = rem_fldvals.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_rem.set_record(column_values) == false)
						{
							error_msg += cif_rem.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_rem.seq_id, VAPTTableName.REMITTERS, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_rem.commit_record() == false)
							{
								error_msg = cif_rem.messages; return false;
							}

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED REMITTERS", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED REMITTERS", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
						}
					}
					return false;
				}

				case "cif_ben":
				{
					ben_fldvals = cifid_val + isDeleted + ben_fldvals.Substring(0, ben_fldvals.Length - 1);
					column_values = ben_fldvals.Split('|');

					if (column_values != null && column_values.Length > 0)
					{
						if (cif_ben.set_record(column_values) == false)
						{
							error_msg += cif_ben.messages; return false;
						}
						else
						{
                            /* VAPT 20180222 */
                            string CIF = string.Empty;
                            string[] arrCIF = null;
                            CIF = getVACIF(cif_ben.seq_id, VAPTTableName.BENEFICIARY, column_values);
                            arrCIF = CIF.Split('^');

                            if (cif_ben.commit_record() == false)
							{
								error_msg = cif_ben.messages; return false;
							}

                            if (arrCIF.GetValue(0).ToString().Length > 0 && arrCIF.GetValue(1).ToString().Length > 0)
                            {
                                new csAuditLog().writeVAPTLogger("", "CIF", "UPDATED REMITTER/BENEFICIARY LINK", arrCIF.GetValue(0).ToString(), arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                            }
                            else
                            {
                                if (arrCIF.GetValue(1).ToString().Length > 0)
                                {
                                    new csAuditLog().writeVAPTLogger("", "CIF", "CREATED REMITTER/BENEFICIARY LINK", "", arrCIF.GetValue(1).ToString(), arrCIF.GetValue(2).ToString(), csAuditLog.AuditType.User, csAuditLog.AlertType.Process);
                                }
                            }

                            return true;
						}
					}
					return false;
				}
				default: break;
			}
			return false;
		}

		public DataSet get_zipcode_seqid(string zip)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_zip");
					sqcommands.Parameters.Add("@ZIPCODE", SqlDbType.VarChar, 10).Value = zip;
					return commons.getDataSet(sqcommands, "CIF_ZIP");
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

        private String get_value_from_table_fldvals(String fld_values, String tbl_column)
        {
            try
            {
                String[] j = fld_values.Split('|');
                for (int i = 0; i < j.Length; i++)
                {
                    String[] k = j[i].Split('~');
                    if (k[0].ToUpper() == tbl_column.ToUpper())
                    {
                        return k[1];
                    }
                }
            }
            catch{}
            return "";
        }

        private void update_cif_preferred_contacts(String SEQID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("sp_internet_remitter_update_prefcontacts", new Object[] {long.Parse(SEQID), cifclass});
                }                
            }
            catch{}
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
        private void update_cif_preferred_address(String SEQID)
        {
            sqlHelperWs commons = new sqlHelperWs();
            SqlCommand sqcommands = null;

            try
            {
                if (commons != null)
                {
                    int res = commons.execStoredProcedureUpdate("sp_internet_remitter_update_prefaddress", new Object[] { long.Parse(SEQID), cifclass });
                }
            }
            catch { }
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
	}
}
