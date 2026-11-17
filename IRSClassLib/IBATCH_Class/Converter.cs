using System;
using System.Web;
using System.Configuration;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Data.Common;

namespace IRSClassLib.IBATCH_Class
{
    public class Converter
    {
        private string strInputFile;
        private string strOutputFile;
        private string strConverter;
        private string filename;
        private string agentCode;

        private DataTable dtARSTransactions;


        public Converter()
        { }

        #region "convert to File Format ARS"

        //AppDomain.CurrentDomain.BaseDirectory + "temp"; IRSWorld
        public bool ConvertFile(string inputFile, string outputFile, string converter)
        {
            try
            {
                this.strInputFile = inputFile;
                this.strOutputFile = outputFile;
                this.strConverter = converter;
                this.filename = inputFile.Substring(inputFile.LastIndexOf(@"\") + 1);
                this.agentCode = converter;
                //this.agentCode = filename.Substring(0, filename.IndexOf("_"));

                //switch (converter.ToUpper().Split('-')[0])
                //{
                //    case "ARS":
                //        returnValue = ConvertToARS();
                //        break;
                //    default:
                //        returnValue = false;
                //        break;
                //}
                return ConvertToARS();
            }
            catch (Exception e) {
                throw e;
            }

            return false;
        }

        private bool ConvertToARS()
        {
            FileReader fileReader = null;
            FileWriter fileWriter = null;
            try // try reading the file
            {
                fileReader = new FileReader(this.strInputFile);

                fileReader.Read(this.strConverter);
                this.dtARSTransactions = fileReader.ARSTransactions;
                fileWriter = new FileWriter(this.strOutputFile);

                fileWriter.WriteHeader(this.agentCode, fileReader.TotalRecords, fileReader.TotalRecordsPHP,
                    fileReader.TotalRecordsUSD, fileReader.TotalRecordsEUR, fileReader.TotalAmountPHP,
                    fileReader.TotalAmountUSD, fileReader.TotalAmountEUR);

                foreach (DataRow dr in this.dtARSTransactions.Rows)
                {
                    ARSTransaction transaction = new ARSTransaction();

                    transaction.AgentTranxRefNo = dr[0].ToString();
                    transaction.RemitterFirstName = dr[1].ToString();
                    transaction.RemitterLastName = dr[2].ToString();
                    transaction.BeneficiaryFirstName = dr[3].ToString();
                    transaction.BeneficiaryLastName = dr[4].ToString();
                    transaction.ContactNo = dr[5].ToString();
                    transaction.Address = dr[6].ToString();
                    transaction.ProductCode = dr[7].ToString();
                    transaction.AccountNo = dr[8].ToString();
                    transaction.GFMCNo = dr[9].ToString();
                    transaction.BankCode = dr[10].ToString();
                    transaction.BankName = dr[11].ToString();
                    transaction.TargetCurrency = dr[12].ToString();
                    transaction.TargerAmount = dr[13].ToString();
                    fileWriter.WriteTranx(transaction);
                }

                fileWriter.WriteFooter();
                fileWriter.Dispose();
                fileWriter = null;

                // dispose fileReader
                fileReader.Dispose();
                fileReader = null;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to convert to ARS Format. " + ex.Message);
            }
            finally
            {
                if (fileReader != null)
                {
                    fileReader.Dispose();
                    fileReader = null;
                }
                if (fileWriter != null)
                {
                    fileWriter.Dispose();
                    fileWriter = null;
                }
            }
        }
        #endregion
    }
}