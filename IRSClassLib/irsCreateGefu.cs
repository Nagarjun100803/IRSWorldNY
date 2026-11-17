using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace IRSClassLib
{
    public class irsCreateGefu
    {
        private const string record_type_head = "1";
        private const string record_type_details = "2";
        private const string record_type_foot = "3";

        ArrayList items;
        //File Header Record
        //private string date_created;
        //==========================

        //Transaction Details Record
        //private string body;
        private string txn_type;
        private string account_no;
        private string branch;
        private string txn_code;
        private string txn_date;
        private string dc_flag;
        private string value_date;
        private string txn_currency;
        private double amount_local;
        private string s_amount_local;
        private string amount_txn;
        //private string s_amount_txn;
        private string convert_rate;
        private string reference_no;
        private string cheque_no;
        private string txn_desc;
        private string benef_ic;
        private string benef;
        private string benef_address1;
        private string benef_address2;
        private string benef_address3;
        private string city;
        private string state;
        private string country;
        private string zip_code;
        private string opt_flag;
        private string issuer_code;
        private string payable_branch;
        private string future_dated_flag;
        private string MIS_code;
        //===============================

        //File Footer Record
        private int debit_count;
        private double debit_amount;
        private int credit_count;
        private double credit_amount;
        //===========================

        #region PUBLIC Properties
        public string Account_No
        {
            get { return this.account_no; }
            set { this.account_no = AddRightPadding(value, " ", 16); }
        }

        public string Txn_Date
        {
            get { return this.txn_date; }
            set { this.txn_date = value; }
        }

        public string Value_Date
        {
            get { return this.value_date; }
            set { this.value_date = value; }
        }

        public string Txn_Currency
        {
            get { return this.txn_currency; }
            set { this.txn_currency = AddLeftPadding(value, "0", 5); }
        }

        public double Amount_Local
        {
            get { return this.amount_local; }
            set { this.amount_local = value; }//AddLeftPadding(value.Replace(".", ""), "0", 14); }
        }

        public string Amount_Txn
        {
            get { return this.amount_txn; }
            set { this.amount_txn = AddLeftPadding(value.Replace(".", ""), "0", 14); }
        }

        public string Convert_Rate
        {
            get { return this.convert_rate; }
            set { this.convert_rate = AddLeftPadding(value, "0", 8); } 
        }

        public string Reference_No
        {
            get { return this.reference_no; }
            set { this.reference_no = AddRightPadding(value, " ", 40); }
        }

        public string Document_No
        {
            get { return this.cheque_no; }
            set { this.cheque_no = AddRightPadding(value, " ", 12); }
        }

        public string Txn_Desc
        {
            get { return this.txn_desc; }
            set { this.txn_desc = AddRightPadding(value, " ", 40); }
        }

        public string Benef_IC
        {
            get { return this.benef_ic; }
            set { this.benef_ic = AddRightPadding(value, " ", 16); }
        }

        public string Benef
        {
            get { return this.benef; }
            set { this.benef = AddRightPadding(value, " ", 120); }
        }

        public string Benef_Address1
        {
            get { return this.benef_address1; }
            set { this.benef_address1 = AddRightPadding(value, " ", 35); }
        }

        public string Benef_Address2
        {
            get { return this.benef_address2; }
            set { this.benef_address2 = AddRightPadding(value, " ", 35); }
        }

        public string Benef_Address3
        {
            get { return this.benef_address3; }
            set { this.benef_address3 = AddRightPadding(value, " ", 35); }
        }

        public string City
        {
            get { return this.city; }
            set { this.city = AddRightPadding(value, " ", 35); }
        }

        public string State
        {
            get { return this.state; }
            set { this.state = AddRightPadding(value, " ", 35); }
        }

        public string Country
        {
            get { return this.country; }
            set { this.country = AddRightPadding(value, " ", 35); }
        }

        public string ZipCode
        {
            get { return this.zip_code; }
            set { this.zip_code = AddRightPadding(value, " ", 35); }
        }

        public string Issuer_Code
        {
            get { return this.issuer_code; }
            set { this.issuer_code = AddLeftPadding(value, "0", 5); }
        }

        public string Payable_Branch
        {
            get { return this.payable_branch; }
            set { this.payable_branch = AddLeftPadding(value, "0", 4); }
        }

        public string MIS_Code
        {
            get { return this.MIS_code; }
            set { this.MIS_code = AddRightPadding(value, " ", 16); }
        }

        public int itemCount
        {
            get { return this.items.Count; }
        }

        public int Credit_Count
        {
            get { return this.credit_count; }
        }

        public double Credit_Amount
        {
            get { return this.credit_amount; }
        }

        public int Debit_Count
        {
            get { return this.debit_count; }
        }

        public double Debit_Amount
        {
            get { return this.debit_amount; }
        }
        #endregion

        #region PR1VATE
        private string AddLeftPadding(string record, string padder, int len)
        {
            string pad = "";
            int count = len - record.Length;
            for (int n = 0; n < count; n++)
            {
                pad += padder;
            }
            return pad + record;
        }

        private string AddRightPadding(string record, string padder, int len)
        {
            string pad = "";
            int count = len - record.Length;
            for (int n = 0; n < count; n++)
            {
                pad += padder;
            }
            return record + pad;
        }

        private string Header()
        {
            return record_type_head + DateTime.Now.ToString("yyyyMMdd");
        }

        private string Footer()
        {
            return record_type_foot
                + AddLeftPadding(debit_count.ToString(), "0", 9)
                + AddLeftPadding(debit_amount.ToString().Replace(".", ""), "0", 15)
                + AddLeftPadding(credit_count.ToString(), "0", 9)
                + AddLeftPadding(credit_amount.ToString().Replace(".", ""), "0", 9);
        }
        #endregion

        #region PUBLIC - GEFU CONTENT
        /// <summary>
        /// Initialize items in the details record, non-mandatory fields with default value
        /// </summary>
        /// <param name="txn_type">FC Txn Type. CASA:‘01’, 
        /// Loans:‘02’, 
        /// GL:‘03’, 
        /// BC:‘04’</param>
        /// <param name="txn_code">Transaction Mnemonic. 1008 - Debit correct, 
        /// 1408 - Credit correct, 
        /// 5534 - Salary Withdrawal</param>
        /// <param name="branch">Branch Code</param>
        /// <param name="dc_flag">Credit: 'C', 
        /// Debit: 'D'</param>
        /// <param name="opt_flag">‘10’: Force debit in case of insufficient funds. 
        /// ‘20’: Partial debit in case of insufficient funds. 
        /// ‘30’: Reject Transaction if insufficient funds.</param>
        /// <param name="future_dated_flag">'Y' or 'N'. Depends on the value date.</param>
        public void newItem(string txn_type, string txn_code, string branch, string dc_flag, string opt_flag, string future_dated_flag)
        {
            this.txn_type = txn_type;
            this.account_no = AddRightPadding("", " ", 16);
            this.branch = AddLeftPadding(branch, "0", 4);
            this.txn_code = AddLeftPadding(txn_code, "0", 5);
            this.txn_date = "yyyyMMdd";
            this.dc_flag = dc_flag;
            this.value_date = "yyyyMMdd";
            this.txn_currency = AddLeftPadding("", " ", 5);
            this.s_amount_local = AddLeftPadding("", "0", 14);
            this.amount_txn = AddLeftPadding("", "0", 14);
            this.convert_rate = AddLeftPadding("", "0", 8);
            this.reference_no = AddRightPadding("", " ", 40);
            this.cheque_no = AddRightPadding("", " ", 12);
            this.txn_desc = AddRightPadding("", " ", 40);
            this.benef_ic = AddRightPadding("", " ", 16);
            this.benef = AddRightPadding("", " ", 120);
            this.benef_address1 = AddRightPadding("", " ", 35);
            this.benef_address2 = AddRightPadding("", " ", 35);
            this.benef_address3 = AddRightPadding("", " ", 35);
            this.city = AddRightPadding("", " ", 35);
            this.state = AddRightPadding("", " ", 35);
            this.country = AddRightPadding("", " ", 35);
            this.zip_code = AddRightPadding("", " ", 35);
            this.opt_flag = opt_flag;
            this.issuer_code = AddLeftPadding("", " ", 5);
            this.payable_branch = AddLeftPadding("", " ", 4);
            this.future_dated_flag = future_dated_flag;
            this.MIS_code = AddRightPadding("", " ", 16);
        }

        /// <summary>
        /// Fields are initialized and are containing exact length,
        /// but still mandatory fields should contain valid value.
        /// </summary>
        public void appendItem()
        {
            if (this.txn_code.Contains("1008"))
            {
                this.credit_count++;
                this.credit_amount += this.amount_local;
            }
            else if (this.txn_code.Contains("1408"))
            {
                this.debit_count++;
                this.debit_amount += this.amount_local;
            }

            this.s_amount_local = AddLeftPadding(this.amount_local.ToString().Replace(".", ""), "0", 14);
            StringBuilder sb = new StringBuilder();
            sb.Append
                (
                    record_type_details
                    + this.txn_type
                    + this.account_no
                    + this.branch
                    + this.txn_code
                    + this.txn_date
                    + this.dc_flag
                    + this.value_date
                    + this.txn_currency
                    + this.s_amount_local
                    + this.amount_txn
                    + this.convert_rate
                    + this.reference_no
                    + this.cheque_no
                    + this.txn_desc
                    + this.benef_ic
                    + this.benef
                    + this.benef_address1
                    + this.benef_address2
                    + this.benef_address3
                    + this.city
                    + this.state
                    + this.country
                    + this.zip_code
                    + this.opt_flag
                    + this.issuer_code
                    + this.payable_branch
                    + this.future_dated_flag
                    + this.MIS_code
                );

            items.Add(sb);
        }

        public void create(string path, string filename)
        {
            string nl = Environment.NewLine;
            csFileIo fio = new csFileIo();
            string new_filename = path + @"\" + fio.getFilenameNoExt(filename) + " - Gefu.txt";

            StringBuilder sb = new StringBuilder(Header() + nl);
            for (int count = 0; count < items.Count; count++)
            {
                sb.Append(items[count].ToString() + nl);
            }
            sb.Append(Footer());

            try
            {
                if (fio.open_output(new_filename, (int)csFileIo.write_modes.CREATE))
                {
                    byte[] b = fio.stringToByte(sb.ToString());
                    fio.put(b, 0, b.Length);
                    fio.close_output();
                }
            }
            catch { }
        }
        #endregion

        #region PUBLIC
        #endregion

        /// <summary>
        /// Initializes footer content only.
        /// All mandatory fields should be filled out to create a valid gefu file.
        /// </summary>
        public irsCreateGefu()
        {
            this.credit_count = 0;
            this.credit_amount = 0;
            this.debit_count = 0;
            this.debit_amount = 0;
            this.items = new ArrayList();
        }
    }
}
