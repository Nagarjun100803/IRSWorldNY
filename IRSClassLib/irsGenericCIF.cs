using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.SqlClient;



namespace IRSClassLib
{
    public class irsCIFMaster : sqlHelper
    {
        int CIFTYPE = 1; //individual        
        StringBuilder  Message = null ;
        DataRow _drUpload ;
        DataTable _dtMaster = new DataTable();
        public irsCIFMaster()
        {
            this.TableName = "TBL_CIF_MASTER";
            this.IdentityColumn = "SEQ_ID";
        }
        ~irsCIFMaster()
        {
            this._dtMaster.Dispose(); 
            this._dtMaster = null;
            this._drUpload = null;

        }
        public string create_cif_master(DataRow drUpload,DataTable dtMaster,int CIFClassType)
        {
            
            this._drUpload = drUpload; // dt from uploaded file 
            this._dtMaster = dtMaster; 

            if (!isUpdatable)
            {
                this.getCIFMaster(); 
            }
            try
            {
                base.newRow();

                foreach (DataRow r in this._dtMaster.Rows )
                {
                    string mappername = r["fld_sourcedfield"].ToString();
                    Object val = this._drUpload[mappername];
                    if (!setColumn(r["fld_targetfield"].ToString(), val))
                    {

                    }
                }
                bool addciftype = setColumn("CIFTYPE", CIFTYPE);

                if (base.addMode)
                {
                    base.table.Rows.Add(uprow);
                    base.addMode = false;
                }

                base.save();
                string cifid = this.getCIFID(base.IdentityValue);
                if (!cifid.Contains("ERROR"))
                {
                    return cifid;
                }
                return "";

                
            }
            catch(Exception e) { Message.Append("ERROR:" + e.Message ); return "";}
        }

        public bool check_cif_master(string buff)
        {
            return true;
        }
        private string getCIFID(Object master_seqid)
        {
            try
            {
                DataRow row = base.getDataSet(base.getSqlCommand("SELECT TOP 1 CIFID FROM TBL_CIF_MASTER WHERE SEQ_ID =" + master_seqid.ToString() , false)).Tables[0].Rows[0];
                return row["CIFID"].ToString();
            }
            catch (Exception e)
            {
                Message.Append(e.Message);  
                return "ERROR: " + e.Message;
            }
        }
        public void getCIFMaster()
        {   
            base.getUpdateDataSet(base.getSqlCommand("SELECT TOP 1 * FROM TBL_CIF_MASTER",false ));
        }
        
            
    }

    public class irsBatchCIFClass : sqlHelper
    {   
        int _CIFClassType  = 0 ;
        StringBuilder Message;
        string _CIFID = String.Empty;
        public irsBatchCIFClass()
        {
            this.TableName = "TBL_CIF_CLASS";
            this.IdentityColumn = "SEQ_ID";
        }
        ~irsBatchCIFClass()
        {
        }
        public bool create_cif_class(string cifid,int classtype)
        {

            this._CIFClassType  = classtype ; // dt from uploaded file 
            this._CIFID = cifid;

            if (!isUpdatable)
            {
                this.getCIFClass();
            }
            try
            {
                if (base.table.Rows.Count == 0)
                {
                    base.newRow();
                    Object ocifid = cifid;
                    setColumn("CIFID", cifid);

                    Object oclasstype = classtype ;
                    setColumn("CIFID", oclasstype );
                   
                    if (base.addMode)
                    {
                        base.table.Rows.Add(uprow);
                        base.addMode = false;
                    }
                    base.save();
                
                }
                return true;
            }
            catch (Exception e) { Message.Append("ERROR:" + e.Message); return false; }
        }

        public void getCIFClass()
        {
            base.getUpdateDataSet(base.getSqlCommand("SELECT TOP 1 * FROM TBL_CIF_CLASS WHERE CIFID ='" + this._CIFID  +"' AND CIFCLASS =" + this._CIFClassType , false));
        }
        

    }

    public class irsBatchCIFAccounts : sqlHelper
    {
        string _AccountNo = "";
        string _Acct_type = "";
        string _BranchCode = "";
        string _AccountCurrency = "";
        StringBuilder Message;
        string _CIFID = String.Empty;
        public irsBatchCIFAccounts()
        {
            this.TableName = "TBL_CIF_ACCOUNTS";
            this.IdentityColumn = "SEQ_ID";
        }
        ~irsBatchCIFAccounts()
        {
        }
        public string  create_cif_accounts(string cifid, string account_num,string acct_type ,string branch_code ,string currency)
        {

            this._AccountNo = account_num; 
            this._CIFID = cifid;
            this._BranchCode = branch_code;
            this._Acct_type = acct_type;
            this._AccountCurrency = currency;

            if (!isUpdatable)
            {
                this.getCIFAccounts();
            }
            try
            {

                if (base.table.Rows.Count == 0)
                {
                    Object[] o = new Object[4];
                    o[0] = cifid;
                    o[1] = account_num;
                    o[2] = acct_type;
                    o[3] = branch_code;
                    o[4] = currency;

                    base.newRow();

                    setColumn("CIFID", o[0]);
                    setColumn("ACCOUNTNO", o[1]);
                    setColumn("AccountType", o[2]);
                    setColumn("BranchCode", o[3]);
                    setColumn("Currency", o[4]);

                    if (base.addMode)
                    {
                        base.table.Rows.Add(uprow);
                        base.addMode = false;
                    }
                    base.save();
                    return base.IdentityValue.ToString();
                }
                else 
                {
                    return table.Rows[0]["SEQ_ID"].ToString();
                }
            }
            catch (Exception e) { Message.Append("ERROR:" + e.Message); return "ERROR: " + e.Message ; }
        }
        public void getCIFAccounts()
        {
            // FOR BENE ONLY
            base.getUpdateDataSet(base.getSqlCommand("SELECT TOP 1 * FROM TBL_CIF_ACCOUNTS WHERE CIFID ='" + this._CIFID + "' AND CIFCLASS = 2" , false));
        }
    }

    public class irsBatchCIFBeneficiaries : sqlHelper
    {
        
        StringBuilder Message;
        string _CIFID = String.Empty;
        string _REMITTERCIFID = String.Empty;
        string _CIFACCTSEQID = String.Empty;
        public irsBatchCIFBeneficiaries(string cifid,string remittercifid)
        {
            this.TableName = "TBL_CIF_BENEFICIARIES";
            this.IdentityColumn = "SEQ_ID";
            this._CIFID = cifid;
            this._REMITTERCIFID = remittercifid;
        }
        ~irsBatchCIFBeneficiaries()
        {
        }
        public bool  create_cif_beneficiaries(string accounts_seqid)
        {
            bool iret = false;
            this._CIFACCTSEQID = accounts_seqid;

            if (!isUpdatable)
            {
                this.getCIFBeneficiaries();
            }
            try
            {
                if (base.table.Rows.Count == 0)
                {
                    base.newRow();
                    setColumn("CIFID", this._CIFID );
                    setColumn("RemitterCIFID", this._REMITTERCIFID );
                    setColumn("AccountSeqId", this._CIFACCTSEQID);
            
                    if (base.addMode)
                    {
                        base.table.Rows.Add(uprow);
                        base.addMode = false;
                    }
                    base.save();

                }
                iret = true;
            }
            catch (Exception e) { Message.Append("ERROR:" + e.Message); iret = false; }
            return iret;
        }
        public void getCIFBeneficiaries()
        {
            // FOR BENE ONLY
            base.getUpdateDataSet(base.getSqlCommand("SELECT TOP 1 * FROM TBL_CIF_BENEFICIARIES WHERE CIFID ='" + this._CIFID  + "' AND REMITTERCIFID = '" + this._REMITTERCIFID  + "'", false));
        }
      
    }

    
    
}
