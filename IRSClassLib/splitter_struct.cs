using System;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    class splitter_struct
    {
        internal class do_splitter_struct
		{
 
            private string str;
            internal string acct_num,  first_substring, third_substing, pay_amount;
           // internal long file_size;
        

            internal do_splitter_struct(string buff)
			{
				this.str = buff;

                this.first_substring = this.str.Substring(0, 65).Trim();
                this.pay_amount = this.get_amt_location().Replace("'", "").Trim();
                this.acct_num = this.get_account_nos().Replace("'", "").Trim();
                this.third_substing = this.str.Substring(97, 3).Trim();

			}

            internal void do_parser()
            {
                // know the replace("'", "")
                this.first_substring    = this.str.Substring(0, 65).Trim();
                this.pay_amount         = this.get_amt_location().Replace("'", "").Trim();
				this.acct_num           = this.get_account_nos().Replace("'", "").Trim();
                this.third_substing     = this.str.Substring(97, 3).Trim();
          	}

        	internal string get_account_nos()
			{
				string acctno_ok      = string.Empty;
				string acct_substr    = string.Empty;
				string acctno_handler = this.str.Substring(85, 12);

				int loop1;

				for (loop1 = 0; loop1 <= acctno_handler.Length - 1; loop1++)
				{
					acct_substr = acctno_handler.Substring(loop1, 1);

					try
                    {
						int.Parse(acct_substr); acctno_ok += acct_substr;						
					}
					catch{}					
				}							
				return acctno_ok;
			}

            internal string get_amt_location()
            {
                string amt = string.Empty;
                string cod = this.str.Substring(0, 4);

                if (cod == "GR02" || cod == "GL03" || cod == "GL04" || cod == "GL05" || cod == "GW01" || cod == "GW02")
                {
                    if (this.str.Substring(97, 3) == "USD")
                    {
                        amt = this.str.Substring(65, 10);
                    }
                    else
                    {
                        amt = this.str.Substring(75, 10);
                    }
                }
                else
                {
                    amt = this.str.Substring(65, 10);
                }
                if (amt == string.Empty) { amt = "0"; } return amt.Replace("'", "").Trim(); ;

            }
    }
}
}