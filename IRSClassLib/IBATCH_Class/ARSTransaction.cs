using System;
using System.Data;
using System.Configuration;
using System.Web;

public class ARSTransaction
{
    private string refNo;
    private string remitterFirstName;
    private string remitterLastName;
    private string beneficiaryFirstName;
    private string beneficiaryLastName;
    private string contactNo;
    private string address;
    private string productCode;
    private string accountNo;
    private string gfmcNo;
    private string bankCode;
    private string bankName;
    private string targetCurrency;
    private string targerAmount;

    public string AgentTranxRefNo
    {
        get { return this.refNo.Trim(); }
        set { this.refNo = value; }
    }

    public string RemitterFirstName
    {
        get { return this.remitterFirstName.Trim(); }
        set { this.remitterFirstName = value; }
    }

    public string RemitterLastName
    {
        get { return this.remitterLastName.Trim(); }
        set { this.remitterLastName = value; }
    }

    public string BeneficiaryFirstName
    {
        get { return this.beneficiaryFirstName.Trim(); }
        set { this.beneficiaryFirstName = value; }
    }

    public string BeneficiaryLastName
    {
        get { return this.beneficiaryLastName.Trim(); }
        set { this.beneficiaryLastName = value; }
    }

    public string ContactNo
    {
        get { return this.contactNo.Trim(); }
        set { this.contactNo = value; }
    }

    public string Address
    {
        get { return this.address.Trim(); }
        set { this.address = value; }
    }

    public string ProductCode
    {
        get { return this.productCode.Trim(); }
        set { this.productCode = value; }
    }

    public string AccountNo
    {
        get { return this.accountNo.Trim(); }
        set { this.accountNo = value; }
    }

    public string GFMCNo
    {
        get { return this.gfmcNo.Trim(); }
        set { this.gfmcNo = value; }
    }

    public string BankCode
    {
        get { return this.bankCode.Trim(); }
        set { this.bankCode = value; }
    }

    public string BankName
    {
        get { return this.bankName.Trim(); }
        set { this.bankName = value; }
    }

    public string TargetCurrency
    {
        get { return this.targetCurrency.Trim(); }
        set { this.targetCurrency = value; }
    }

    public string TargerAmount
    {
        get { return this.targerAmount.Trim(); }
        set { this.targerAmount = value; }
    }

}
