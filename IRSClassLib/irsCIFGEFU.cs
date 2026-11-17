using System;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace IRSClassLib
{
	[Serializable]
	public class irsCIFGEFU : sqlHelperWs
	{
		#region PRIVATE VARIABLES

		private string file_name		= null;
		private string all				= null;
		private string[] yyyymmdd   	= null;
		private string[] fulldate		= null;
		private string record_type_h	= "1";		
		private string creation_date	= null;
		private string record_type_t	= "2";
		private string branch_code		= null;
		private string cust_ic			= null;	//"GFMC"+MMDDYY+NUMBER SERIES(001)
		private string cust_type		= null;
		private string cust_prefix		= null;
		private string cust_short_name  = null;
		private string cust_first_name  = null;
		private string cust_middle_name = null;
		private string cust_last_name	= null;
		private string address1			= null;
		private string address2			= null;
		private string address3			= null;
		private string city				= null;
		private string state			= null;
		private string country			= null;
		private string zip				= null;
		private string bdate			= null;
		private string phone_no			= null;
		private string mobile_no		= null;
		private string email			= null;
		private string gender			= null;
		private string marital_stat		= null;
		private string flag_staff		= null;
		private string bus_cat			= null;
		private string prof_cat			= null;
		private string cust_lang		= null;
		private string emp_id			= null;
		private string phone_no_country = null;
		private string nationality		= null;
		private string cust_educ		= null;
		private string casa_prod_code	= null;
		private string officer_id		= null;
		private string tds_code			= null;
		private string tds_code2		= null;
		private string acct_cust_rel	= null;
		private string acct_title		= null;
		private string cust_minor_stat	= null;
		private string int_waiver		= null;
		private string restricted_acct	= null;
		private string issue_atm_card	= null;
		private string atm_card_prod	= null;
		private string card_type		= null;
		private string card_no			= null;
		private string card_stat		= null;
		private string date_issue		= null;
		private string date_expiry		= null;
		private string open_acct		= null;
		private string record_type_f	= "3";
		public string return_msg		= null;
		
		#endregion

		public string main(string date)
		{
			try
			{
				string dir	  = null;
				creation_date = this.convert_date(date, "YYYYMMDD");
				bdate		  = this.convert_date(date, "YYYYMMDD");
				file_name	  = this.generate_file_name(creation_date);
				
				if (this.has_record(date) == false)
				{
					return "NO RECORD FOR THIS DATE!";
				}
				else
				{
					dir	= this.get_directory();
					all = this.generate_lines(date, creation_date);

					StreamWriter file = new StreamWriter(dir + "\\" + file_name);
					file.WriteLine(all);
					file.Close();

					return check_file(dir, file_name);					
				}
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public string check_file(string dir, string file_name)
		{
			if (File.Exists(dir + "\\" + file_name))
			{
				return "FILE SUCCESSFULLY CREATED!\n\nPath: " + dir + "\\" + file_name;
			}
			else
			{
				return "UNABLE TO WRITE FILE. PLEASE TRY AGAIN.";
			}
		}

		public string get_directory()
		{
			try
			{
				string c_drive = @"C:";
				string d_drive = @"D:";
				string e_drive = @"E:";
				string c_gefu  = @"C:\GEFU_CIF";
				string d_gefu  = @"D:\GEFU_CIF";
				string e_gefu  = @"E:\GEFU_CIF";

				if (Directory.Exists(c_drive))
				{
					if (!(Directory.Exists(c_gefu)))
					{
						Directory.CreateDirectory(c_gefu);
					}
					return c_gefu;
				}
				else if (Directory.Exists(d_drive))
				{
					if (!(Directory.Exists(d_gefu)))
					{
						Directory.CreateDirectory(d_gefu);
					}
					return d_gefu;
				}
				else if (Directory.Exists(e_drive))
				{
					if (!(Directory.Exists(e_gefu)))
					{
						Directory.CreateDirectory(e_gefu);
					}
					return e_gefu;
				}

				else return "";
			}
			catch (Exception e)
			{
				return e.Message;
			}			
		}

		public string generate_file_name(string date)
		{
			string filename = "CIFGefu_" + date + ".txt";
			return filename;
		}

		public bool has_record(string date)
		{
			sqlHelperWs commons = new sqlHelperWs();

			return (Convert.ToInt32(commons.execScalar("sp_cif_has_cif_record", new string[] { date })) > 0);
		}

		public string generate_cust_ic(string lname, string mname, string fname, string bdate)
		{
			if (lname.Length < 3) lname = lname + " ";
			if (mname.Length < 1) mname = mname + " ";
			if (fname.Length < 3) fname = fname + " ";
			if (bdate == "19500101") bdate = "010150";
			else bdate = this.convert_date(bdate, "DDMMYY");

			string gen_cust_ic = lname.Substring(0, 3) + mname.Substring(0, 1) + fname.Substring(0, 3) + bdate;

			return gen_cust_ic;
		}

		public string generate_lines(string date, string creation_date)
		{
			try
			{
				DataSet ds  = this.get_newly_created(date);
				string line = "";

				for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
				{
					this.set_values(ds, i);

					cust_ic = cust_last_name + cust_first_name + cust_middle_name;
					if (cust_ic.Length > 30) cust_ic = cust_ic.Substring(0, 30);
				
					branch_code = this.add_leading_zeroes(5 - branch_code.Length) + branch_code;

					line +=
						record_type_t +
						branch_code.ToUpper() +
						cust_ic.ToUpper() + this.create_white_spaces(30 - cust_ic.Length) +
						cust_type.ToUpper() +
						cust_prefix.ToUpper() + this.create_white_spaces(40 - cust_prefix.Length) +
						cust_short_name.ToUpper() + this.create_white_spaces(20 - cust_short_name.Length) +
						cust_first_name.ToUpper() + this.create_white_spaces(35 - cust_first_name.Length) +
						cust_middle_name.ToUpper() + this.create_white_spaces(35 - cust_middle_name.Length) +
						cust_last_name.ToUpper() + this.create_white_spaces(35 - cust_last_name.Length) +
						address1.ToUpper() + this.create_white_spaces(35 - address1.Length) +
						address2.ToUpper() + this.create_white_spaces(35 - address2.Length) +
						address3.ToUpper() + this.create_white_spaces(35 - address3.Length) +
						city.ToUpper() + this.create_white_spaces(35 - city.Length) +
						state.ToUpper() + this.create_white_spaces(35 - state.Length) +
						country.ToUpper() + this.create_white_spaces(35 - country.Length) +
						zip.ToUpper() + this.create_white_spaces(10 - zip.Length) +
						bdate + this.create_white_spaces(8 - bdate.Length) +
						phone_no.ToUpper() + this.create_white_spaces(15 - phone_no.Length) +
						mobile_no.ToUpper() + this.create_white_spaces(12 - mobile_no.Length) +
						email.ToUpper() + this.create_white_spaces(40 - email.Length) +
						gender.ToUpper() +
						marital_stat.ToUpper() +
						flag_staff.ToUpper() +
						bus_cat.ToUpper() + this.create_white_spaces(6 - bus_cat.Length) +
						prof_cat.ToUpper() + this.create_white_spaces(3 - prof_cat.Length) +
						cust_lang.ToUpper() + this.create_white_spaces(3 - cust_lang.Length) +
						emp_id.ToUpper() + this.create_white_spaces(16 - emp_id.Length) +
						phone_no_country.ToUpper() + this.create_white_spaces(15 - phone_no_country.Length) +
						nationality.ToUpper() + this.create_white_spaces(15 - nationality.Length) +
						cust_educ.ToUpper() + this.create_white_spaces(40 - cust_educ.Length) +
						casa_prod_code.ToUpper() + this.create_white_spaces(3 - casa_prod_code.Length) +
						officer_id.ToUpper() + this.create_white_spaces(12 - officer_id.Length) +
						tds_code.ToUpper() + this.create_white_spaces(3 - tds_code.Length) +
						tds_code2.ToUpper() + this.create_white_spaces(3 - tds_code2.Length) +
						acct_cust_rel.ToUpper() + this.create_white_spaces(3 - acct_cust_rel.Length) +
						acct_title.ToUpper() + this.create_white_spaces(40 - acct_title.Length) +
						cust_minor_stat.ToUpper() +
						int_waiver.ToUpper() +
						restricted_acct.ToUpper() +
						issue_atm_card.ToUpper() +
						atm_card_prod.ToUpper() + this.create_white_spaces(5 - atm_card_prod.Length) +
						card_type.ToUpper() +
						card_no.ToUpper() + this.create_white_spaces(20 - card_no.Length) +
						card_stat.ToUpper() + this.create_white_spaces(3 - card_stat.Length) +
						date_issue + this.create_white_spaces(8 - date_issue.Length) +
						date_expiry + this.create_white_spaces(8 - date_expiry.Length) +
						//this.convert_date(date_issue, "YYYYMMDD")  + this.create_white_spaces(8 - date_issue.Length) +
						//this.convert_date(date_expiry, "YYYYMMDD") + this.create_white_spaces(8 - date_expiry.Length) +
						open_acct.ToUpper() + Environment.NewLine;
				}

				return record_type_h + creation_date + Environment.NewLine + line + this.create_footer(ds.Tables[0].Rows.Count);
			}
			catch
			{
				return null;
			}			
		}

		public string convert_date(string date, string format)
		{
			fulldate = date.Split(' ');
			date	 = fulldate[0];
			
			yyyymmdd = date.Split('/');
			if (yyyymmdd.Length == 3)
			{
				string month = yyyymmdd[0];
				string day   = yyyymmdd[1];
				string year  = yyyymmdd[2];

				if (month.Length < 2)
				{
					month = "0" + month;
				}
				if (day.Length < 2)
				{
					day = "0" + day;
				}
								
				if (format == "YYYYMMDD")
				{
					return year + month + day;
				}
				else
				{//DDMMYY 
					if (date == "")
					{
						return creation_date.Substring(7, 2) + creation_date.Substring(5, 2) + creation_date.Substring(2, 2);
					}
					return day + month + year.Substring(2, 2);
				}
			}
			return creation_date.Substring(6, 2) + creation_date.Substring(4, 2) + creation_date.Substring(2, 2);
		}

		public string create_white_spaces(int count)
		{
			string white_space = "";
			for (int i = 0; i < count; i++)
			{
				white_space = white_space + " ";
			}
			return white_space;
		}

		public string add_leading_zeroes(int count)
		{
			string zeroes = "";
			for (int i = 0; i < count; i++)
			{
				zeroes = zeroes + "0";
			}
			return zeroes;
		}

		public string remove_dash(string phoneno)
		{
			string temp = null;

			for (int i = 0; i < phoneno.Length; i++)
			{
				if (phoneno[i] != '-')
				{
					temp += phoneno[i];
				}
			}
			return temp;
		}

		public void set_default_values()
		{
			branch_code		 = "01170";
			cust_type		 = "A";
			cust_prefix		 = "Messrs";
			cust_last_name	 = "GFMC";
			cust_first_name  = "MONEY CARD";
			cust_middle_name = "GFMC";
			address1		= "PNB FINANCIAL CENTER";
			address2		= "PRES DIOSDADO MACAPAGAL BLVD.";
			address3		= "METRO MANILA";
			city			= "PASAY";
			state			= "METRO MANILA";
			country			= "PH";
			zip				= "1300";
			phone_no		= "0";
			mobile_no		= "0";
			email			= "0";
			gender			= "M";
			marital_stat	= "001";
			flag_staff		= "N";
			bus_cat			= "000099";
			prof_cat		= "099";
			cust_lang		= "ENG";
			emp_id			= "0";
			phone_no_country = "063";
			nationality		= "PH";
			cust_educ		= "MBA";
			casa_prod_code	= "905";
			officer_id		= "0";
			tds_code		= "999";
			tds_code2		= "999";
			acct_cust_rel	= "SOW";
			acct_title		= "GFMC MONEY CARD";
			cust_minor_stat	= "0";
			int_waiver		= "N";
			restricted_acct	= "N";
			issue_atm_card	= "N";
			atm_card_prod	= "00040";
			card_type		= "0";
			card_no			= "0";
			card_stat		= "NNN";
			date_issue		= "19500101";
			date_expiry		= "19500101";
			open_acct		= "B";
		}
		
		public void set_values(DataSet ds, int i)
		{
			try
			{
				this.set_default_values();

				if (((ds.Tables[0].Rows[i]["BranchCode"]).ToString()).Trim().Length != 0)
				{
					branch_code = (ds.Tables[0].Rows[i]["BranchCode"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["CIFType"]).ToString()).Trim().Length != 0)
				{
					cust_type = (ds.Tables[0].Rows[i]["CIFType"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["Title"]).ToString()).Trim().Length != 0)
				{
					cust_prefix = (ds.Tables[0].Rows[i]["Title"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["LastName"]).ToString()).Trim().Length != 0)
				{
					cust_last_name = (ds.Tables[0].Rows[i]["LastName"]).ToString();
					if (cust_last_name.Length > 35) cust_last_name = cust_last_name.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["FirstName"]).ToString()).Trim().Length != 0)
				{
					cust_first_name = (ds.Tables[0].Rows[i]["FirstName"]).ToString();
					if (cust_first_name.Length > 35) cust_first_name = cust_first_name.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["MiddleName"]).ToString()).Trim().Length != 0)
				{
					cust_middle_name = (ds.Tables[0].Rows[i]["MiddleName"]).ToString();
					if (cust_middle_name.Length > 35) cust_middle_name = cust_middle_name.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["DateCreated"]).ToString()).Trim().Length != 0)
				{
					creation_date = this.convert_date((ds.Tables[0].Rows[i]["DateCreated"]).ToString(), "YYYYMMDD");
				}
				if (((ds.Tables[0].Rows[i]["Address1"]).ToString()).Trim().Length != 0)
				{
					address1 = (ds.Tables[0].Rows[i]["Address1"]).ToString();
					if (address1.Length > 35) address1 = address1.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["Address2"]).ToString()).Trim().Length != 0)
				{
					address2 = (ds.Tables[0].Rows[i]["Address2"]).ToString();
					if (address2.Length > 35) address2 = address2.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["Address3"]).ToString()).Trim().Length != 0)
				{
					address3 = (ds.Tables[0].Rows[i]["Address3"]).ToString();
					if (address3.Length > 35) address3 = address3.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["City"]).ToString()).Trim().Length != 0)
				{
					city = (ds.Tables[0].Rows[i]["City"]).ToString();
					if (city.Length > 35) city = city.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["State"]).ToString()).Trim().Length != 0)
				{
					state = (ds.Tables[0].Rows[i]["State"]).ToString();
					if (state.Length > 35) state = state.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["Country"]).ToString()).Trim().Length != 0)
				{
					country = (ds.Tables[0].Rows[i]["Country"]).ToString();
					if (country.Length > 35) country = country.Substring(0, 35);
				}
				if (((ds.Tables[0].Rows[i]["Zip"]).ToString()).Trim().Length != 0)
				{
					zip = (ds.Tables[0].Rows[i]["Zip"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["BirthDate"]).ToString()).Trim().Length != 0)
				{
					bdate = this.convert_date((ds.Tables[0].Rows[i]["BirthDate"]).ToString(), "YYYYMMDD");
				}
				if (((ds.Tables[0].Rows[i]["PhoneNo"]).ToString()).Trim().Length != 0)
				{
					phone_no = this.remove_dash((ds.Tables[0].Rows[i]["PhoneNo"]).ToString());
					if (this.remove_dash(phone_no).Length > 15) phone_no = phone_no.Substring(0, 15);
				}
				if (((ds.Tables[0].Rows[i]["MobileNo"]).ToString()).Trim().Length != 0)
				{
					mobile_no = this.remove_dash((ds.Tables[0].Rows[i]["MobileNo"]).ToString());
					if (this.remove_dash(mobile_no).Length > 12) mobile_no = mobile_no.Substring(0, 12);
				}
				if (((ds.Tables[0].Rows[i]["Email"]).ToString()).Trim().Length != 0)
				{
					email = (ds.Tables[0].Rows[i]["Email"]).ToString();
					if (email.Length > 40) email = email.Substring(0, 40);
				}
				if (((ds.Tables[0].Rows[i]["Gender"]).ToString()).Trim().Length != 0)
				{
					gender = (ds.Tables[0].Rows[i]["Gender"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["MaritalStat"]).ToString()).Trim().Length != 0)
				{
					marital_stat = (ds.Tables[0].Rows[i]["MaritalStat"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["FlagStaff"]).ToString()).Trim().Length != 0)
				{
					flag_staff = (ds.Tables[0].Rows[i]["FlagStaff"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["EmployeeID"]).ToString()).Trim().Length != 0)
				{
					emp_id = (ds.Tables[0].Rows[i]["EmployeeID"]).ToString();
					if (emp_id.Length > 16) emp_id = emp_id.Substring(0, 16);
				}
				if (((ds.Tables[0].Rows[i]["CardNo"]).ToString()).Trim().Length != 0)
				{
					card_no = (ds.Tables[0].Rows[i]["CardNo"]).ToString();
				}
				if (((ds.Tables[0].Rows[i]["IssueDate"]).ToString()).Trim().Length != 0)
				{
					date_issue = this.convert_date((ds.Tables[0].Rows[i]["IssueDate"]).ToString(), "YYYYMMDD");
				}
				if (((ds.Tables[0].Rows[i]["ExpiryDate"]).ToString()).Trim().Length != 0)
				{
					date_expiry = this.convert_date((ds.Tables[0].Rows[i]["ExpiryDate"]).ToString(), "YYYYMMDD");
				}

				//set short name
				string lfm = cust_last_name + cust_first_name + cust_middle_name;
				if (lfm.Length < 20)
				{
					lfm = lfm + this.create_white_spaces(20 - lfm.Length);
				}
				cust_short_name = lfm.Substring(0, 20);

				//set account title
				string accttitle = cust_first_name + ' ' + cust_middle_name.Substring(0, 1) + ". " + cust_last_name;
				if (accttitle.Length < 40)
				{
					accttitle = accttitle + this.create_white_spaces(40 - accttitle.Length);
				}
				acct_title = accttitle.Substring(0, 40);
			}
			catch
			{
			}			
		}

		public string create_footer(int rec_count)
		{
			string zeroes = "";

			int limit = 10 - (rec_count.ToString().Length + 1);
			
			for (int j = 0; j < limit; j++)
			{
				zeroes = zeroes + "0";
			}
			return record_type_f + zeroes + rec_count.ToString();
		}

		public DataSet get_newly_created(string date)
		{
			sqlHelperWs commons = new sqlHelperWs();
			SqlCommand sqcommands = null;

			try
			{
				if (commons != null)
				{
					sqcommands = commons.getSqlCommand("sp_cif_get_newlycreated");
					sqcommands.Parameters.Add("@DATECREATED", SqlDbType.VarChar, 100).Value = date;
					return commons.getDataSet(sqcommands, "CIF_NEWLY_CREATED");
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

	}//end of class irsCIFGEFU
}
