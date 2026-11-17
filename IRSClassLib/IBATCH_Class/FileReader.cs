using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;

public class FileReader : IDisposable
{
    private StreamReader reader;
    private DataTable dtARStranx;
    private int totalRecords = 0;
    private int[] recordCount = new int[3];
    private double[] totalAmount = new double[3];
    private bool disposed;
    private String Filename = "";

    #region FILE READER

    public FileReader(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentNullException("The argument \'path\' must not be null or empty.");

        try
        {
            Filename = Path.GetFileName(path);
            reader = new StreamReader(path);
            this.dtARStranx = CreateARSTable();
        }
        catch (ArgumentException ax)
        {
            throw new ArgumentException("FileReader:" + ax.Message);
        }
        catch (FileNotFoundException fnfx)
        {
            throw new FileNotFoundException("FileReader:" + fnfx.Message);
        }
        catch (DirectoryNotFoundException dnfx)
        {
            throw new FileNotFoundException("FileReader:" + dnfx.Message);
        }
        catch (IOException iox)
        {
            throw new IOException("FileReader:" + iox.Message);
        }
    }
    #endregion

    #region FILE FORMATS

    public void Read(string fileFormat)
    {
        switch (fileFormat.Trim().ToUpper())
        {
            case "A04":
                ReadAlRostamani();
                break;
            //case "E310":
                //ReadAlFardan();
                //break; // commented to remove warning
            case "A01":
                ReadAlAnsari();
                break;
            case "A06":
                ReadAlRajhi();
                break;
        }
    }
    #endregion

    #region AL FARDAN FORMAT

    private void ReadAlFardan()
    {
        int currencyLocation = 25;
        int amountLocation = 26;

        string readLine = string.Empty;
        string[] specialChar = { "/", "\\", "\'", "*" };
        char delimeter = '|';
        while (reader.Peek() != -1)
        {
            readLine = reader.ReadLine();
            foreach (string c in specialChar)
            {
                readLine = readLine.Replace(c, "");
            }

            string[] arrReadLine = readLine.Split(delimeter);

            CheckCurrency(arrReadLine[currencyLocation], Convert.ToDouble(arrReadLine[amountLocation]));

            ParseAlFardan(arrReadLine);

            totalRecords++;
        }
    }

    /* *         Parse to ARS Format             * */
    /* ---------------------------------------------
     * ARS Column               - Al Fardan Column
     * ---------------------------------------------
     * Agent Reference No       - 1
     * Remitter FirstName       - 31
     * Remitter LastName        - 31
     * Beneficiary FirstName    - 2
     * Beneficiary LastName     - 2
     * Beneficiary ContactNo    - --
     * Beneficiary Address      - 4
     * Product Code             - 15
     * Account No               - 3
     * GFMC No                  - --
     * Bank Code                - --
     * Bank Name                - 8
     * Target Currency          - 26
     * Target Amount            - 27
     */

    private void ParseAlFardan(string[] arrReadLine)
    {
        DataRow dr = this.dtARStranx.NewRow();
        string fn = "";
        string ln = "";

        dr["RefNo"] = arrReadLine[0];

        getName(arrReadLine[30], ref fn, ref ln);
        dr["RemFName"] = fn;
        dr["RemLName"] = ln;

        fn = "";
        ln = "";

        getName(arrReadLine[1], ref fn, ref ln);

        dr["BeneFName"] = fn;
        dr["BeneLName"] = ln;

        dr["BeneContact"] = ""; // not specified
        dr["BeneAddress"] = arrReadLine[3];

        switch (arrReadLine[14].ToString().Trim().ToUpper())
        {
            case "CSPK": // to be mapped to APAS
                dr["ProductCode"] = "APAS";
                break;
            case "CRSB": // to be mapped to RR
                dr["ProductCode"] = "RR";
                break;
            case "CROB": // if target currency is peso then mapped to EPCS. If target currency id usd then mapped to PDDTS
                if (arrReadLine[25].ToUpper().Trim().ToUpper() == "PHP")
                    dr["ProductCode"] = "EPCS";
                else if (arrReadLine[25].ToUpper().Trim().ToUpper() == "USD")
                    dr["ProductCode"] = "PDDTS";
                else
                    dr["ProductCode"] = "";
                break;
            default:
                dr["ProductCode"] = arrReadLine[14].ToString().Trim().ToUpper();
                break;
        }

        dr["AccountNo"] = arrReadLine[2];
        dr["GFMCCardNo"] = ""; // not specified
        dr["BankCode"] = ""; // not specified
        dr["BankName"] = arrReadLine[7];
        dr["TargetCurrency"] = arrReadLine[25];
        dr["TargetAmount"] = arrReadLine[26];

        // add the datarow to ARStranx datatable
        this.dtARStranx.Rows.Add(dr);
    }
    #endregion

    #region AL ANSARI FORMAT

    /* *AlAnsari File format* */
    private void ReadAlAnsari()
    {
        int currencyLocation = 3;
        int amountLocation = 5;

        string readLine = string.Empty;
        string[] specialChar = { "/", "\\", "\'", "*" };
        char delimeter = '|';
        while (reader.Peek() != -1)
        {
            readLine = reader.ReadLine();
            if (readLine == "") continue;
            if (readLine == null) continue;
            if (readLine.Length == 1) continue;
            // remove special characters
            foreach (string c in specialChar)
            {
                readLine = readLine.Replace(c, "");
            }

            // split
            string[] arrReadLine = readLine.Split(delimeter);

            try
            {
                // check currency and increment recordcount and total amount per currecynr
                CheckCurrency(arrReadLine[currencyLocation], Convert.ToDouble(arrReadLine[amountLocation]));

                // parse the readLine
                ParseAlAnsari(arrReadLine);

                totalRecords++; // add 1 to counter
            }
            catch { throw new Exception("Invalid Al Rostamani file. Please check the file content."); }
        }
    }

    /* *         Parse to ARS Format             * */
    /* ---------------------------------------------
     * ARS Column               - Al Ansari Column
     * ---------------------------------------------
     * Agent Reference No       - 1
     * Remitter FirstName       - 5
     * Remitter LastName        - 5
     * Beneficiary FirstName    - 7
     * Beneficiary LastName     - 7
     * Beneficiary ContactNo    - 11
     * Beneficiary Address      - 10
     * Product Code             - 12
     * Account No               - 8
     * GFMC No                  - --
     * Bank Code                - --
     * Bank Name                - 9
     * Target Currency          - 4
     * Target Amount            - 6
     */

    private void ParseAlAnsari(string[] arrReadLine)
    {
        DataRow dr = this.dtARStranx.NewRow();
        string fn = "";
        string ln = "";

        dr["RefNo"] = arrReadLine[0];

        getName(arrReadLine[4], ref fn, ref ln);
        dr["RemFName"] = fn;
        dr["RemLName"] = ln;

        fn = "";
        ln = "";

        getName(arrReadLine[6], ref fn, ref ln);

        dr["BeneFName"] = fn;
        dr["BeneLName"] = ln;

        dr["BeneContact"] = arrReadLine[10];
        dr["BeneAddress"] = arrReadLine[9];

        switch (arrReadLine[11].ToUpper())
        {
            case "DCB"://"DCB - Direct Credit to Bank
                dr["ProductCode"] = "RR"; // Rapid Remit
                break;
            case "ANP": //ANP - Advice and Pay
                dr["ProductCode"] = "APAS"; // Advise and Pay (APAS)
                break;
            case "D2D": //D2D - Door to Door
                dr["ProductCode"] = "DDMM"; // Door to door Metro Manila
                break;
            case "B2B": //B2B - Credit to other Banks"
                dr["ProductCode"] = "D2B"; // Door to door Metro Manila
                break;
            default:
                dr["ProductCode"] = arrReadLine[11]; // Unknown product code
                break;
        }
        dr["AccountNo"] = arrReadLine[7];
        dr["GFMCCardNo"] = ""; // not specified
        dr["BankCode"] = ""; // not specified
        dr["BankName"] = arrReadLine[8];
        dr["TargetCurrency"] = arrReadLine[3];
        dr["TargetAmount"] = arrReadLine[5];

        // add the datarow to ARStranx datatable
        this.dtARStranx.Rows.Add(dr);
    }

    #endregion

    #region AL ROSTAMANI FORMAT

    /* * AlRostamani File format* */
    private void ReadAlRostamani()
    {
        int currencyLocation = 41;
        int amountLocation = 42;

        string readLine = string.Empty;
        string[] specialChar = { "/", "\\", "\'", "*" };
        char delimeter = '~';

        while (reader.Peek() != -1)
        {
            readLine = reader.ReadLine();
            // remove special characters
            foreach (string c in specialChar)
            {
                readLine = readLine.Replace(c, "");
            }

            // split
            string[] arrReadLine = readLine.Split(delimeter);

            try
            {
                // check currency and increment recordcount and total amount per currecynr
                CheckCurrency(arrReadLine[currencyLocation], Convert.ToDouble(arrReadLine[amountLocation]));

                // parse the readLine
                ParseRostamani(arrReadLine);

                totalRecords++; // add 1 to counter
            }
            catch { throw new Exception("Invalid Al Rostamani file. Please check the file content."); }
        }
    }

    /* *         Parse to ARS Format             * */
    /* ---------------------------------------------
     * ARS Column               - Al Rostamani Column
     * ---------------------------------------------
     * Agent Reference No       - 1
     * Remitter FirstName       - 5
     * Remitter LastName        - 5
     * Beneficiary FirstName    - 22
     * Beneficiary LastName     - 22
     * Beneficiary ContactNo    - 40
     * Beneficiary Address      - 23, 24, 25
     * Product Code             - 34
     * Account No               - 21
     * GFMC No                  - --
     * Bank Code                - -- --> 16
     * Bank Name                - 17
     * Target Currency          - 3  --> 26 --> 42
     * Target Amount            - 4  --> 43
     */

    private void ParseRostamani(string[] arrReadLine)
    {
        DataRow dr = this.dtARStranx.NewRow();
        string fn = "";
        string ln = "";

        dr["RefNo"] = arrReadLine[0];

        getName(arrReadLine[4], ref fn, ref ln);

        dr["RemFName"] = fn;
        dr["RemLName"] = ln;

        fn = "";
        ln = "";

        getName(arrReadLine[21], ref fn, ref ln);

        dr["BeneFName"] = fn;
        dr["BeneLName"] = ln;

        dr["BeneContact"] = arrReadLine[39];

        // Check beneaddress and set value
        dr["BeneAddress"] = "";

        if (!string.IsNullOrEmpty(arrReadLine[22].ToString().Trim()))
            dr["BeneAddress"] = arrReadLine[22];

        if (!string.IsNullOrEmpty(arrReadLine[23].ToString().Trim()))
            dr["BeneAddress"] = dr["BeneAddress"].ToString().Trim() + " " + arrReadLine[23].ToString().Trim();

        if (!string.IsNullOrEmpty(arrReadLine[24].ToString().Trim()))
            dr["BeneAddress"] = dr["BeneAddress"].ToString().Trim() + " " + arrReadLine[24].ToString().Trim();


        // check product code the set the ars product code value
        switch (arrReadLine[33].ToUpper())
        {
            case "DCB":
                dr["ProductCode"] = "RR";
                break;
            case "B2B":
                dr["ProductCode"] = "D2B";
                break;
            case "D2D":
                dr["ProductCode"] = "DDMM";
                break;
            case "PID":
            case "ANP":
                dr["ProductCode"] = "APAS";
                break;
            default:
                dr["ProductCode"] = arrReadLine[33];
                break;
        }

        dr["AccountNo"] = arrReadLine[20];
        dr["GFMCCardNo"] = "";
        dr["BankCode"] = arrReadLine[15];
        dr["BankName"] = arrReadLine[16];
        dr["TargetCurrency"] = arrReadLine[41];
        dr["TargetAmount"] = arrReadLine[42];

        // add the datarow to ARStranx datatable
        this.dtARStranx.Rows.Add(dr);
    }

    #endregion

    #region AL RAJHI FORMAT

    /* * AlRajhi File format* */
    private void ReadAlRajhi()
    {
        int currencyLocation = 13;
        int amountLocation = 16;

        string readLine = string.Empty;

        string[] specialChar = { "/", "\\", "\'", "*" };
        char delimeter = '|';

        new IRSClassLib.irsEXCOAlRajhiResponse().delete_existing_file(Filename);

        while (reader.Peek() != -1)
        {
            readLine = reader.ReadLine();

            // add the original file to Al Rajhi Table
            new IRSClassLib.irsEXCOAlRajhiResponse().write_to_al_rajhi_temp(readLine, Filename);
                
            // remove special characters
            foreach (string c in specialChar)
            {
                readLine = readLine.Replace(c, "");
            }

            // split
            string[] arrReadLine = readLine.Split(delimeter);

            try
            {
                // check currency and increment recordcount and total amount per currecynr
                //CheckCurrency(arrReadLine[currencyLocation], Convert.ToDouble(arrReadLine[amountLocation]));
                CheckCurrency(arrReadLine[currencyLocation], (Convert.ToDouble(arrReadLine[amountLocation]) / 100));

                // parse the readLine
                ParseRajhi(arrReadLine);

                totalRecords++; // add 1 to counter
            }
            catch { throw new Exception("Invalid Al Rajhi file. Please check the file content."); }
        }
    }

    /* *         Parse to ARS Format             * */
    /* ---------------------------------------------
     * ARS Column               - Al Rajhi Column
     * ---------------------------------------------
     * Agent Reference No       - 1
     * Remitter FirstName       - 2
     * Remitter LastName        - 2
     * Beneficiary FirstName    - 3
     * Beneficiary LastName     - 3
     * Beneficiary ContactNo    - 6
     * Beneficiary Address      - 4, 5
     * Product Code             - 15
     * Account No               - 12
     * GFMC No                  - --
     * Bank Code                - 8
     * Bank Name                - 7
     * Target Currency          - 13
     * Target Amount            - 16
     */

    private void ParseRajhi(string[] arrReadLine)
    {
        DataRow dr = this.dtARStranx.NewRow();
        string fn = "";
        string ln = "";

        dr["RefNo"] = arrReadLine[1];

        getName(arrReadLine[2], ref fn, ref ln);

        dr["RemFName"] = fn;
        dr["RemLName"] = ln;

        fn = "";
        ln = "";

        getName(arrReadLine[3], ref fn, ref ln);

        dr["BeneFName"] = fn;
        dr["BeneLName"] = ln;

        dr["BeneContact"] = arrReadLine[6];

        // Check beneaddress and set value
        dr["BeneAddress"] = "";

        if (!string.IsNullOrEmpty(arrReadLine[4].ToString().Trim()))
            dr["BeneAddress"] = arrReadLine[4];

        if (!string.IsNullOrEmpty(arrReadLine[5].ToString().Trim()))
            dr["BeneAddress"] = dr["BeneAddress"].ToString().Trim() + " " + arrReadLine[5].ToString().Trim();

        // check product code the set the ars product code value
        switch (arrReadLine[15].ToUpper())
        {
            case "01":
                dr["ProductCode"] = "RR";
                break;
            case "??":
                dr["ProductCode"] = "D2B";
                break;
            case "02":
                dr["ProductCode"] = "DDMM";
                break;
            case "PID":
            case "03":
                dr["ProductCode"] = "APAS";
                break;
            default:
                dr["ProductCode"] = arrReadLine[15];
                break;
        }

        dr["AccountNo"] = arrReadLine[12];
        dr["GFMCCardNo"] = "";
        dr["BankCode"] = arrReadLine[8];
        dr["BankName"] = arrReadLine[7];
        dr["TargetCurrency"] = arrReadLine[13];
        //dr["TargetAmount"] = arrReadLine[16];
        dr["TargetAmount"] = (Convert.ToDouble(arrReadLine[16]) / 100);
        //dr["TargetAmount"] = arrReadLine[16].Substring(0, 13) + "." + arrReadLine[16].Substring(13, 2);
        // add the datarow to ARStranx datatable
        this.dtARStranx.Rows.Add(dr);

    }

    

    #endregion

    #region FUNCTIONS

    private void getName(string name, ref string fn, ref string ln)
    {
        string[] names = splitStr(removeSpl(name), " ");
        int nLen = names.Length;
        if (names.Length == 2)
        {
            fn = names[0].Trim();
            ln = names[1].Trim();
        }
        else if (names.Length >= 3)
        {
            string last = names[nLen - 2].Trim();
            string last2 = names[nLen - 1].Trim();
            string da = last2.ToUpper();
            if (inGene(ref da) == true)
            {
                last = names[nLen - 3].Trim();
                last2 = names[nLen - 2].Trim();
                if (last.ToUpper() == "DE" || last.ToUpper() == "DEL" || last.ToUpper() == "DELA" || last.ToUpper() == "DELOS")
                {
                    string temp = "";
                    for (int i = 0; i < nLen - 3; i++)
                    {
                        temp = temp + " " + names[i];
                    }
                    fn = string.Concat(temp.Trim(), " " + da);
                    ln = string.Concat(last, " " + last2);
                }
                else
                {
                    string temp = "";
                    for (int i = 0; i < nLen - 2; i++)
                    {
                        temp = temp + " " + names[i];
                    }
                    fn = string.Concat(temp.Trim(), " " + da);
                    ln = last2;
                }
            }
            else if (last.ToUpper() == "DE" || last.ToUpper() == "DEL" || last.ToUpper() == "DELA" || last.ToUpper() == "DELOS")
            {
                string temp = "";
                for (int i = 0; i < nLen - 2; i++)
                {
                    temp = temp + " " + names[i];
                }
                fn = temp.Trim();
                ln = string.Concat(last, " " + last2);
            }
            else
            {
                string temp = "";
                for (int i = 0; i < nLen - 1; i++)
                {
                    temp = temp + " " + names[i];
                }
                fn = temp.Trim();
                ln = last2;
            }
        }
    }

    private bool inGene(ref string L)
    {
        bool isfound = false;
        string[] gen = { "SR", "JR", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
        L = removeSpl(L);
        foreach (string b in gen)
        {
            if (b == L.ToUpper())
            {
                return true;
            }
        }
        return isfound;
    }

    private string removeSpl(string str)
    {
        char[] M = str.ToCharArray();
        string newStr = "";
        for (int i = 0; i < str.Length; i++)
        {
            char c = M[i];
            if (c == 'ñ')
            {
                c = 'n';
                newStr = string.Concat(newStr, c.ToString());
            }
            else if (c == 'Ñ')
            {
                c = 'N';
                newStr = string.Concat(newStr, c.ToString());
            }
            else
            {
                if (!isSpl(c))
                {
                    newStr = string.Concat(newStr, c.ToString());
                }
            }
        }
        return newStr;
    }

    private bool isSpl(char c)
    {
        bool found = false;
        char[] Spl = "~`!@#$%^&*_,/?+=(){}[]<>:;|\\\"\'".ToCharArray();
        foreach (char s in Spl)
        {
            if (s == c)
            {
                return true;
            }
        }
        return found;
    }

    private string[] splitStr(string str, string delim)
    {
        string[] strArr;
        strArr = (str.Trim()).Split(delim.ToCharArray());
        return strArr;
    }

    private void CheckCurrency(string currency, double amount)
    {
        switch (currency.Trim().ToUpper())
        {
            case "PHP":
                this.recordCount[0]++;
                this.totalAmount[0] = this.totalAmount[0] + amount;
                break;
            case "USD":
                this.recordCount[1]++;
                this.totalAmount[1] = this.totalAmount[1] + amount;
                break;
            case "EUR":
                this.recordCount[1]++;
                this.totalAmount[2] = this.totalAmount[2] + amount;
                break;
            default:
                break;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (false == disposed)
        {
            if (disposing)
            {
                if (this.reader != null)
                {
                    this.reader.Close();
                    this.reader.Dispose();
                    this.reader = null;
                }
                if (this.dtARStranx != null)
                {
                    this.dtARStranx.Dispose();
                    this.dtARStranx = null;
                }
            }

        }
        disposed = true;
    }

    #endregion

    #region ARS Columns/Fields

    private DataTable CreateARSTable()
    {
        DataTable dataTable = new DataTable("ARSFormat");
        DataColumn[] arrDataColumn = {
            new DataColumn("RefNo"), 
            new DataColumn("RemFName"),
            new DataColumn("RemLName"), 
            new DataColumn("BeneFName"),
            new DataColumn("BeneLName"),
            new DataColumn("BeneContact"), 
            new DataColumn("BeneAddress"), 
            new DataColumn("ProductCode"), 
            new DataColumn("AccountNo"), 
            new DataColumn("GFMCCardNo"), 
            new DataColumn("BankCode"), 
            new DataColumn("BankName"), 
            new DataColumn("TargetCurrency"), 
            new DataColumn("TargetAmount")};

        foreach (DataColumn c in arrDataColumn)
        {
            dataTable.Columns.Add(c);
        }
        return dataTable;
    }

    #endregion

    #region PROPERTIES

    public DataTable ARSTransactions
    {
        get { return this.dtARStranx; }
    }

    public int TotalRecords
    {
        get { return this.totalRecords; }
    }

    public int TotalRecordsPHP
    {
        get { return this.recordCount[0]; }
    }

    public int TotalRecordsUSD
    {
        get { return this.recordCount[1]; }
    }

    public int TotalRecordsEUR
    {
        get { return this.recordCount[2]; }
    }

    public double TotalAmountPHP
    {
        get { return totalAmount[0]; }
    }

    public double TotalAmountUSD
    {
        get { return totalAmount[1]; }
    }

    public double TotalAmountEUR
    {
        get { return totalAmount[2]; }
    }

    #endregion
}
