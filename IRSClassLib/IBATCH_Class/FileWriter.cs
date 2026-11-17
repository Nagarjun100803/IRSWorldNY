using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;

class FileWriter
{
    private StreamWriter SW;
    private bool disposed;

    public FileWriter(string path)
    {
        this.SW = new StreamWriter(path);
    }

    public void WriteHeader(string agentCode, int totalRec, int totalRecPHP, int totalRecUSD, 
        int totalRecEUR, double totalAmtPHP, double totalAmtUSD, double totalAmtEUR)
    { 
        this.SW.WriteLine(string.Format("HDR;{0};{1};{2};{3};{4};{5};{6};{7}", agentCode, totalRec, totalRecPHP, totalRecUSD, totalRecEUR, totalAmtPHP, totalAmtUSD, totalAmtEUR));
    }

    public void WriteTranx(ARSTransaction transaction)
    {
        this.SW.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13}",
            transaction.AgentTranxRefNo,
            transaction.RemitterFirstName,
            transaction.RemitterLastName,
            transaction.BeneficiaryFirstName,
            transaction.BeneficiaryLastName,
            transaction.ContactNo,
            transaction.Address,
            transaction.ProductCode,
            transaction.AccountNo,
            transaction.GFMCNo,
            transaction.BankCode,
            transaction.BankName,
            transaction.TargetCurrency,
            transaction.TargerAmount));
    }

    public void WriteFooter()
    {
        this.SW.Write("TRL");
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
                if (this.SW != null)
                {
                    this.SW.Close();
                    this.SW.Dispose();
                    this.SW = null;
                }                
            }
        }
        disposed = true;
    }
 }
