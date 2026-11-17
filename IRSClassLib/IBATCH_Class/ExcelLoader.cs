using System;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using IRSClassLib;
using Excel = ExcelLibrary;

namespace IRSClassLib.IBATCH_Class
{
    public class ExcelLoader
    {
        #region VARIABLES
        private Hashtable ht_process = null;
        private Hashtable ht_details;
        private Hashtable ht_pay_details = new Hashtable();
        private double _dblTargetAmt;
        private string _branch = "branch";
        private string _uploadingbranch = "";
        private string _extlRefNo;
        private string _remitterName;
        private string _beneName;
        private string _beneAddress;
        private string _beneContactNo;
        private string _accountNum;
        private string _otherBankCode;
        private string _otherBankName;
        public string _sTargetCcy;
        public string _sRemCcy;
        public string _sRemAmt;
        public string _sTargetAmt;
        public string _amount_summary;
        public string text_data = "";        
        public int record_count = 0;
        public int record_bypassed = 0;
        public int record_loaded = 0;
        public int record_errors = 0;
        public double _dblTotalAmt;
        #endregion
        #region CONSTRUCTOR
        public ExcelLoader(string _branchcode)
        {
            this._uploadingbranch = _branchcode;
        }
        #endregion
        #region PUBLIC METHODS
        public Hashtable readXLSN(string FileName, int file_format)
        {
            Excel.SpreadSheet.Workbook wkbk = null;
            Excel.SpreadSheet.Worksheet wksheet = null;
            irsBatchUpload bu = new irsBatchUpload(file_format, 7, FileName, "0");

            if (file_format == 1)
            {
                try
                {
                    int vrow = 0;
                    int hrow = 0;
                    record_count = 1;
                    record_bypassed = 0;
                    record_loaded = 0;
                    record_errors = 0;

                    Hashtable ht_pay_details = new Hashtable();
                    this.kill_remaining_excel();

                    wkbk = Excel.SpreadSheet.Workbook.Load(FileName);
                    wksheet = wkbk.Worksheets[0];

                    hrow = 1;

                    bool pass = bu.clean_tempfile();
                    while (hrow <= wksheet.Cells.LastRowIndex)
                    {
                        text_data = "";

                        for (vrow = 0; vrow <= 14; vrow++)
                        {
                            text_data += this.null_cell_to_textN(wksheet, hrow, vrow) + "~";

                        }
                        bu.write_data_to_tempfile(text_data + "~", this._uploadingbranch);
                        this.parseXLS(text_data.ToString().Trim());
                        record_count++;

                        string cur = this._sRemCcy;
                        string amt = this._sRemAmt;

                        if (cur != "")
                        {
                            if (hrow == 1)
                            {
                                ht_pay_details.Add(cur, cur + "~1~" + amt + "~");
                            }
                            else
                            {
                                if (!ht_pay_details.ContainsKey(cur)) ht_pay_details.Add(cur, cur + "~1~" + amt + "~");
                                else
                                {
                                    string[] val = ht_pay_details[cur].ToString().Split('~');
                                    ht_pay_details[cur] = cur + "~" +
                                            Convert.ToString((Convert.ToDouble(val[1]) + 1)) + "~" +
                                            Convert.ToString(Convert.ToDouble(val[2]) + Convert.ToDouble(amt));
                                }
                            }
                        }
                        hrow++;
                    }
                    int i = 0;
                    _amount_summary = "";
                    foreach (object o in ht_pay_details.Keys)
                    {
                        _amount_summary += ht_pay_details[o];
                        if (ht_pay_details.Count > 1)
                        {
                            if ((i + 1) < ht_pay_details.Count)
                            {
                                _amount_summary += "|";
                            }
                        }
                        i++;
                    }
                    System.Web.HttpContext.Current.Session["AMOUNT_SUMMARY"] = _amount_summary;

                    this.ht_details = ht_pay_details;
                    this.kill_remaining_excel();

                }
                catch (Exception e)
                {
                    this.writeAudit("Load err - " + record_count + e.Message, csAuditLog.Category.Insert);
                }
                finally
                {
                    wkbk = null;
                    wksheet = null;
                    bu = null;                    
                    ht_pay_details = null;
                }
            }

            return this.ht_details;
        }
        public void writeAudit(string Description, csAuditLog.Category Category)
        {
            new csAuditLog().writeAudit("", "File Loader", Description, "", "", Category);
        }
        #endregion
        #region PRIVATE METHODS
        private void parseXLS(string text_data)
        {
            string[] filerec = text_data.Split('~');

            try
            {
                this._branch = filerec[0];
                this._extlRefNo = filerec[2].ToString() + filerec[0].ToString() + filerec[1].ToString();
                this._remitterName = filerec[4];
                this._beneName = filerec[5];
                this._beneAddress = filerec[6];
                this._beneContactNo = (filerec[6].Trim() == "") ? "none" : filerec[6];
                this._sTargetCcy = filerec[7];
                this._sTargetAmt = filerec[8];
                this._accountNum = filerec[10].Trim();
                this._otherBankCode = filerec[13];
                this._otherBankName = filerec[14];

                if (this._branch != "" & this._branch != string.Empty & this._branch != null & this._branch != "null")
                {
                    this._dblTargetAmt = Convert.ToDouble(this._sTargetAmt);
                    this._dblTotalAmt = Convert.ToDouble(this._dblTotalAmt) + Convert.ToDouble(this._sTargetAmt);
                    this._sRemAmt = this._sTargetAmt;
                    this._sRemCcy = this._sTargetCcy;
                }
                else
                {
                    this._sRemAmt = "";
                    this._sRemCcy = "";
                }

            }
            catch
            {
                throw new Exception("Invalid Record. Incomplete number of field.");
            }
            //this._iTargetCcy = getLookupValue("Currency", this._sTargetCcy);
        }
        private string null_cell_to_textN(Excel.SpreadSheet.Worksheet  work_sheet, int row, int col)
        {
            try
            {
                return work_sheet.Cells[row, col].Value.ToString().Replace(",", "").Trim();
            }
            catch
            {
                return string.Empty;
            }
        }
        private void kill_remaining_excel()
        {
            Process[] process; process = System.Diagnostics.Process.GetProcessesByName("Excel");

            int i = 0;

            for (i = 0; i < process.Length; i++)
            {
                if (ht_process != null)
                {
                    if (ht_process.ContainsKey(process[i].Id) == false)
                    {
                        if (process[i].HasExited == false)
                        {
                            process[i].Kill(); continue;
                        }
                    }
                }

                if (process[i].HasExited == false)
                {
                    process[i].Kill();
                }
            }
        }
        #endregion


    }
}