using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace IRSClassLib
{
	/*
	 * sample function call
	 * irsClientAgentEnrollment ae = new irsClientAgentEnrollment();
	 * 	ae.enroll_agent("FirstName~Reyeliza|Address1~San Vicente|LastName~Dilla|ContactNo~09158307456|Occupation~Programmer|OtherID~200543559|IDNo~0421296859|BranchCode~LDN", 1, out errmsg);
	*/

	public class irsClientAgentEnrollment : sqlHelper
	{
		irsClientMaster			 cif_mas = new irsClientMaster();
		irsClientClassifications cif_cls = new irsClientClassifications();
		irsClientAddress		 cif_add = new irsClientAddress();
		irsClientContacts		 cif_con = new irsClientContacts();
		irsClientBeneficiaries   cif_ben = new irsClientBeneficiaries();
		irsClientOccupations	 cif_occ = new irsClientOccupations();
		irsClientIds			 cif_ids = new irsClientIds();
		irsClientOtherIds		 cif_oth = new irsClientOtherIds();

		#region PRIVATE VARIABLES

		private string[] mas_fldcols = null;
		private string[] add_fldcols = null;
		private string[] con_fldcols = null;
		private string[] occ_fldcols = null;
		private string[] ids_fldcols = null;
		private string[] oth_fldcols = null;
		private string mas_fldvals = null;
		private string add_fldvals = null;
		private string con_fldvals = null;
		private string occ_fldvals = null;
		private string ids_fldvals = null;
		private string oth_fldvals = null;
		private string error_msg = null;

		#endregion

		public bool enroll_agent(String plist, short cifclass, out String errmsg)
		{
			this.do_init(plist);
			this.insert_cif(cifclass);
			errmsg = null;
			return false;
		}

		public bool insert_cif(short cifclass)
		{
			if (cifclass == 1)
			{
				if (this.insert_master() == true)
				{
					if (this.commit_master() == true)
					{
						return true;
					}
				}
			}
			error_msg = null;
			return true;
		}

		public bool insert_master()
		{
			try
			{
				if (cif_mas.insert_record() == false)
				{
					error_msg = cif_mas.messages; return false;
				}
				error_msg = null;
				return true;
			}
			catch (Exception e)
			{
				error_msg = e.Message; return false;
			}
		}

		public bool commit_master()
		{
			mas_fldvals = "CIFID~|CIFType~1|" + mas_fldvals.Substring(0, mas_fldvals.Length - 1);
			string[] column_values = mas_fldvals.Split('|');

			if (column_values != null && column_values.Length > 0)
			{
				if (cif_mas.set_record(column_values) == false)
				{
					error_msg = cif_mas.messages; return false;
				}
				else
				{
					if (cif_mas.commit_record() == false)
					{
						error_msg = cif_mas.messages; return false;
					}
					return true;
				}
			}
			return false;
		}

		public void do_init(string plist)
		{
			mas_fldcols = new string[] {"LastName", "FirstName", "MiddleName", "BirthDate", "BirthPlace", "BranchCode"};
			add_fldcols = new string[] {"Address1", "Address2", "ZipCode", "CountryCode"};
			con_fldcols = new string[] {"ContactNo", "EmailAddress"};
			occ_fldcols = new string[] {"Occupation"};
			ids_fldcols = new string[] {"IDNo", "IDType"};
			oth_fldcols = new string[] {"RefNoTheirs", "OtherID"};
			
			string[] cols = plist.Split('|');
			
			for (int i = 0; i < cols.Length; i++)
			{
				string[] vals = cols[i].Split('~');

				for (int j = 0; j < mas_fldcols.Length; j++)
				{
					if (mas_fldcols[j].ToUpper() == vals[0].ToUpper()) mas_fldvals += cols[i] + '|';					
				}

				for (int k = 0; k < add_fldcols.Length; k++)
				{
					if (add_fldcols[k].ToUpper() == vals[0].ToUpper()) add_fldvals += cols[i] + '|';
				}

				for (int l = 0; l < con_fldcols.Length; l++)
				{
					if (con_fldcols[l].ToUpper() == vals[0].ToUpper()) con_fldvals += cols[i] + '|';
				}

				for (int m = 0; m < ids_fldcols.Length; m++)
				{
					if (ids_fldcols[m].ToUpper() == vals[0].ToUpper()) ids_fldvals += cols[i] + '|';
				}

				for (int n = 0; n < occ_fldcols.Length; n++)
				{
					if (occ_fldcols[n].ToUpper() == vals[0].ToUpper()) occ_fldvals += cols[i] + '|';
				}

				for (int o = 0; o < oth_fldcols.Length; o++)
				{
					if (oth_fldcols[o].ToUpper() == vals[0].ToUpper()) oth_fldvals += cols[i] + '|';
				}
			}
		}
	}
}
