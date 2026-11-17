using System;
using System.Collections;
using System.Data;
using System.Text;

namespace IRSClassLib
{
    /*
     * (c) PNB 2008 /  Carlos H. Raymundo 
     * 
     */
    [Serializable]
    public class csPasswordGen
    {
        private String alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private String specials = @"~!@#$%^&*()_+{}|[]\:;<>?/.,";
        private String numbers = "1234567890";

        private String conf_alpha;
        private String conf_specials;
        private String conf_numbers;

        private int pwd_min;
        private int uid_min;

        private void getCharTables()
        {
            irsConfiguration conf = new irsConfiguration();
            String s = conf.appsetting("PWDGEN_ALPHA");
            if (s != null && s.Length > 0) conf_alpha = s;

            alpha = alpha + alpha.ToLower();

            s = conf.appsetting("PWDGEN_NUMERICS");
            if (s != null && s.Length > 0) conf_numbers = s;

            s = conf.appsetting("PWDGEN_SPECIALS");
            if (s != null && s.Length > 0) conf_specials = s;
        }

        private void getMinimums()
        {
            irsSqlCommonsWs scom = new irsSqlCommonsWs();
            scom.IsWinApp = true;

            DataTable dt = scom.getLookup(new Object[] { "Password", "MIN", -1 });

            if (dt != null && dt.Rows.Count > 0)
            {
                int i = Convert.ToInt32(dt.Rows[0]["fldValue"]);
                if (i > 32) i = 8;
                pwd_min = (i < 8) ? 8 : i;
            }
            else
            {
                pwd_min = 8;
            }

            dt = scom.getLookup(new Object[] { "USERID", "MIN", -1 });

            if (dt != null && dt.Rows.Count > 0)
            {
                int j = Convert.ToInt32(dt.Rows[0]["fldValue"]);
                uid_min = (j < 5 | j > 15) ? 6 : j;
            }
            else
            {
                uid_min = 6;
            }
        }

        /* 06-01-2020 MBSS FIX */
        public void user_admin_pwd_len(String userid)
        {
            DataTable dt = null;
            irsClientData irs_rec_user = null;

            try
            {
                if (System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"] != null)
                {
                    irs_rec_user = ((irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]);
                }

                if (irs_rec_user != null)
                {
                    if (irs_rec_user.SuperAdmin == true || irs_rec_user.Role == 1)
                    {
                        pwd_min = 12;

                        dt = new sqlHelperWs().getDataSet("sp_lookup", true, new object[] { "Password", "MINAdmin", -1 }, new string[] { "MINADNLOOKUP" }).Tables[0];

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            pwd_min = Convert.ToInt32(dt.Rows[0]["FldValue"]);
                        }
                    }
                }
            }
            catch
            {
            }
        }
        /* 06-01-2020 MBSS FIX */

        public bool password_valid(String userid, String pwd, out String errmsg)
        {
            errmsg = "";


            if (userid.Equals(pwd))
            {
                errmsg = "Userid cannot be used as password.";
                return false;
            }

            /* 06-01-2020 MBSS FIX */
            this.user_admin_pwd_len(userid);
            /* 06-01-2020 MBSS FIX */

            if (pwd.Length < pwd_min)
            {
                errmsg = "The password length is not within minimum.";
                return false;
            }

            short whites = 0;
            short nums = 0;
            short alps = 0;
            short alpb = 0;
            short spec = 0;
            alpha += alpha.ToLower();

            char[] cc = pwd.ToCharArray();
            for (int i = 0; i < cc.Length; i++)
            {
                if (char.IsNumber(cc[i]))
                {
                    nums = 1;
                }
                else if (char.IsUpper(cc[i])) 
                {
                    alpb = 1;
                }
                else if (char.IsLower(cc[i]))
                {
                    alps = 1;
                }
                else if (specials.Contains(cc[i].ToString()))
                {
                    spec = 1;
                }
                else
                    whites++;
            }

            //if (alps == pwd.Length)
            //{
            //    //errmsg = "Try mixing password with numbers and special characters.";
            //    errmsg = "Password should be mixed with Upper and Lower Case.";
            //    return false;
            //}            
            //if (nums == pwd.Length)
            //{
            //    //errmsg = "Try mixing password with letters and special characters.";
            //    errmsg = "Password should have letters and special charaters.";
            //    return false;
            //}

            //if (spec == pwd.Length)
            //{
            //    //errmsg = "Try mixing it with letters and numbers.";
            //    errmsg = "Password should be mixed with letters and numbers.";
            //    return false;
            //}

            int total = alpb + alps + nums + spec;
            if (total < 4) 
            {
                errmsg = "Password must contain alphabet(Upper & Lower case), number and special character.";
                return false;
            }
            if (whites > 0)
            {
                errmsg = "The password contains white spaces and unsupported charaters.";
                return false;
            }

            return true;
        }

        public String generatePassword(String username, String userid)
        {

            Random rand = new Random();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < pwd_min; i++)
            {
                String alp = (String.IsNullOrEmpty(conf_alpha)) ? alpha : conf_alpha;
                String num = (String.IsNullOrEmpty(conf_numbers)) ? numbers : conf_numbers;
                String spc = (String.IsNullOrEmpty(conf_specials)) ? specials : conf_specials;
                switch (rand.Next(2))
                {
                    case 0: sb.Append(alp.Substring(rand.Next(alp.Length - 1), 1)); break;
                    case 1: sb.Append(num.Substring(rand.Next(num.Length - 1), 1)); break;
                    case 2: sb.Append(spc.Substring(rand.Next(spc.Length - 1), 1)); break;
                }
            }

            return sb.ToString();
        }

        public String generatePasswordAlphaNumeric()
        {
            Random rand = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                String alp = (String.IsNullOrEmpty(conf_alpha)) ? alpha : conf_alpha;
                String num = (String.IsNullOrEmpty(conf_numbers)) ? numbers : conf_numbers;
                //String spc = (String.IsNullOrEmpty(conf_specials)) ? specials : conf_specials;
                switch (rand.Next(2))
                {
                    case 0: sb.Append(alp.Substring(rand.Next(alp.Length - 1), 1)); break;
                    case 1: sb.Append(num.Substring(rand.Next(num.Length - 1), 1)); break;
                    //case 2: sb.Append(spc.Substring(rand.Next(spc.Length - 1), 1)); break;
                }
            }
            return sb.ToString();
        }

        public csPasswordGen()
        {
            getCharTables();
            getMinimums();
        }
    }
}
