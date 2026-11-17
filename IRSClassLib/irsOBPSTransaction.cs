using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IRSClassLib
{
    public class irsOBPSTransaction : sqlHelperWs
    {
        private string Merchant_Code;
        private int Product_Code;
        private int DisPlay_Seqid;
        csAuditLog audit = new csAuditLog();
        public DataSet getRemitterInfo()
        {
            DataSet ds = execStoredProcedure("spGetOBPS_Remitter", new object[] { null });
            return ds;
        }

        public DataTable getMerchantList()
        {
            DataTable dt = null;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]; 
            try
            {
                dt = execStoredProcedureTable("usp_getMerchantList", new object[] { "" });
            }
            catch //(Exception ex)
            {
                //audit.writeAudit(clientdata.UserId, "OBPS Dynamic Fields", "getMerchantList", "", ex.Message, csAuditLog.Category.Select);
            }
            finally
            {
            }
            return dt;
        }

        public DataTable getMerProductByMerchant(string MerchantCode)
        {
            DataTable dt = null;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                dt = execStoredProcedureTable("spGetMerProductbyMerchant", new object[] { MerchantCode });
            }
            catch //(Exception ex)
            {
                //audit.writeAudit(clientdata.UserId, "OBPS Dynamic Fields", "getMerchantList", "", ex.Message, csAuditLog.Category.Select);
            }
            finally
            {
            }
            return dt;
        }

        public DataTable getMerchantFields(string MerchantCode, int MerchantProdCode)
        {
            DataTable dt = null;
            try
            {
                dt = execStoredProcedureTable("spGetOptionalFields", new object[] { MerchantCode, MerchantProdCode });
            }
            catch //(Exception ex)
            {
            }
            return dt;
        }

        public bool createGenericFields(string merchantCode, Int64 merprodCode)
        {
            bool bReturn = false;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"]; 
            try
            {
                execStoredProcedure("spCreateGenericFields", new object[] { merchantCode, merprodCode });
                bReturn = true;
            }
            catch (Exception ex)
            {
                audit.writeAudit(clientdata.UserId, "OBPS Dynamic Fields", "createGenericFields", merchantCode + "-" + merprodCode.ToString(), ex.Message, csAuditLog.Category.Insert);
            }
            finally
            {
                audit.writeAudit(clientdata.UserId, "OBPS Dynamic Fields", "createGenericFields", merchantCode + "-" + merprodCode.ToString(), "", csAuditLog.Category.Insert);
            }
            return bReturn;
        }

        public DataTable getLookupFldGroup()
        {
            DataTable dt = null;
            irsClientData clientdata = (irsClientData)System.Web.HttpContext.Current.Session["IRS_CLIENT_DATA"];
            try
            {
                dt = execStoredProcedureTable("sp_lookupfilter", new object[] { "" });
            }
            catch //(Exception ex)
            {
                //audit.writeAudit(clientdata.UserId, "OBPS Dynamic Fields", "getMerchantList", "", ex.Message, csAuditLog.Category.Select);
            }
            finally
            {
            }
            return dt;
        }
        public DataTable getAllFields(int displayseq, string mercode, int prodcode)
        {
            DataTable dt = null;
            try
            {
                SqlCommand command = getSqlCommand("SELECT SEQ_ID FROM tbl_optionalfield WHERE DisplaySeq = " + displayseq + " AND MerchantCode = '" + mercode + "' AND ProductCode = " + prodcode);
                dt = execStoredProcedure(command).Tables[0];
            }
            catch
            {
            }
            return dt;
        }
        public int getFieldsSEQID()
        {
            return (int) execScalar("SELECT SEQ_ID FROM tbl_optionalfield WHERE DisplaySeq = " + DisPlay_Seqid + " AND MerchantCode = '" + Merchant_Code + "' AND ProductCode = " + Product_Code);
        }
        public override void getUpdateDS(int id)
        {
            SqlCommand command = getSqlCommand("SELECT SEQ_ID,MerchantCode,ProductCode,FieldName,DisplaySeq,DisplayDtDef,FieldLabel,IsMandatory,DataType,ValidationType,ValidationSeq,Argument,ValidateOnValue,FieldName2,FieldName3,LIFldRef,isDeleted,ControlName,IsARM FROM tbl_optionalfield where SEQ_ID = " + id, false);
            base.getUpdateDataSet(command);
            //base.getUpdateDS("sp_GetOptionalFieldTable", new object[] { id });
        }

        public bool saveFields(int seqid, string merchantCode, int productcode, string data_value)
        {// Not yet done
            System.Collections.ArrayList error_list = new System.Collections.ArrayList();
            try
            {
                string[] splitted_value;
                string[] pairvalue;
                bool success = false;
                Merchant_Code = merchantCode;
                Product_Code = productcode;
                DisPlay_Seqid = seqid;
                splitted_value = data_value.Split(',');
                this.TableName = "tbl_optionalfield";
                this.IdentityColumn = "SEQ_ID";
                int id = 0;
                DataTable dtCopy = null;
                bool isForUpdate = false;
                if (seqid > 0)
                {
                    id = getFieldsSEQID();
                    this.IdentityValue = id;
                }
                else
                {
                    base.getUpdateDS("sp_GetOptionalFieldTable", new object[] { id });
                    dtCopy = table.Copy();
                    isForUpdate = (dtCopy.Rows.Count > 0 ? true : false);
                }

                foreach (string y in splitted_value)
                {
                    if (y != string.Empty)
                    {
                        pairvalue = y.Split('=');
                        pushData(pairvalue);
                    }
                }
                base.selectedRow = 0;
                if (seqid >= 1)
                {
                    base.addMode = false;
                    if (base.edit(id) == true)
                    {
                        if (!base.setColumn("MerchantCode", Convert.ToString(merchantCode))) { error_list.Add("MerchantCode: " + base.Message); }
                        if (!base.setColumn("ProductCode", Convert.ToInt32(productcode))) { error_list.Add("ProductCode: " + base.Message); }
                        if (!base.setColumn("FieldName", Convert.ToString(this.Fieldname))) { error_list.Add("FieldName: " + base.Message); }
                        if (!base.setColumn("DisplaySeq", Convert.ToInt32(this.Displayseq))) { error_list.Add("DisplaySeq: " + base.Message); }
                        if (!base.setColumn("DisplayDtDef", Convert.ToString(this.Displaydtdef))) { error_list.Add("DisplayDtDef: " + base.Message); }
                        if (!base.setColumn("FieldLabel", Convert.ToString(this.Fieldlabel))) { error_list.Add("FieldLabel: " + base.Message); }
                        if (!base.setColumn("IsMandatory", Convert.ToBoolean(this.Ismandatory))) { error_list.Add("IsMandatory: " + base.Message); }
                        if (!base.setColumn("DataType", Convert.ToString(this.Datatype))) { error_list.Add("DataType: " + base.Message); }
                        if (!base.setColumn("ValidationType", Convert.ToString(this.Validationtype))) { error_list.Add("ValidationType: " + base.Message); }
                        if (!base.setColumn("ValidationSeq", Convert.ToInt32(this.Validationseq))) { error_list.Add("ValidationSeq: " + base.Message); }
                        if (!base.setColumn("Argument", Convert.ToString(this.Argument))) { error_list.Add("Argument: " + base.Message); }
                        if (!base.setColumn("ValidateOnValue", Convert.ToDecimal(this.Validateonvalue))) { error_list.Add("ValidateOnValue: " + base.Message); }
                        if (!base.setColumn("FieldName2", Convert.ToString(this.Fieldname2))) { error_list.Add("FieldName2: " + base.Message); }
                        if (!base.setColumn("FieldName3", Convert.ToString(this.Fieldname3))) { error_list.Add("FieldName3: " + base.Message); }
                        if (!base.setColumn("LIFldRef", Convert.ToString(this.Lifldref))) { error_list.Add("LIFldRef: " + base.Message); }
                        if (!base.setColumn("isDeleted", Convert.ToBoolean(this.Isdeleted))) { error_list.Add("isDeleted: " + base.Message); }
                        if (!base.setColumn("ControlName", Convert.ToString(this.Controlname))) { error_list.Add("ControlName: " + base.Message); }
                        if (!base.setColumn("IsARM", Convert.ToBoolean(this.Isarm))) { error_list.Add("IsARM: " + base.Message); }
                        
                        success = base.save();
                        if (success == false)
                        {
                            error_list.Add("tbl_optionalfield - COMMIT FAILED - " + base.Message); base.cancel();
                        }
                    }

                }
                else
                {
                    base.addMode = true;
                    if (base.newRow(sqlHelperWs.RETAIN_DATASET))
                    {
                        uprow["MerchantCode"] = merchantCode;
                        uprow["ProductCode"] = productcode;
                        uprow["FieldName"] = this.Fieldname;
                        uprow["DisplaySeq"] = this.Displayseq;
                        uprow["DisplayDtDef"] = this.Displaydtdef;
                        uprow["FieldLabel"] = this.Fieldlabel;
                        uprow["IsMandatory"] = this.Ismandatory;
                        uprow["DataType"] = this.Datatype;
                        uprow["ValidationType"] = this.Validationtype;
                        uprow["ValidationSeq"] = this.Validationseq;
                        uprow["Argument"] = (this.Argument == string.Empty) ? null : this.Argument;
                        uprow["ValidateOnValue"] = (this.Validateonvalue == 0) ? 0 : this.Validateonvalue;
                        uprow["FieldName2"] = (this.Fieldname2 == string.Empty) ? System.Data.SqlTypes.SqlString.Null : this.Fieldname2;
                        uprow["FieldName3"] = (this.Fieldname3 == string.Empty) ? System.Data.SqlTypes.SqlString.Null : this.Fieldname3;
                        uprow["LIFldRef"] = (this.Lifldref == string.Empty) ? System.Data.SqlTypes.SqlString.Null : this.Lifldref;
                        uprow["isDeleted"] = this.Isdeleted;
                        uprow["ControlName"] = (this.Controlname == string.Empty) ? System.Guid.NewGuid().ToString() : this.Controlname;
                        uprow["IsARM"] = this.Isarm;
                        uprow["DateCreated"] = DateTime.Now;
                        uprow["DateLastMaintained"] = DateTime.Now;
                        table.Rows.Add(uprow);
                    }
                    base.addMode = false;
                    success = base.save(sqlHelperWs.RETAIN_DATASET);
                    if (success == false)
                    {
                        error_list.Add("tbl_optionalfield - COMMIT FAILED - " + base.Message); base.cancel();
                    }
                }        

                base.addMode = false;                                
                
                return success;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            //return false;
        }

        private void pushData(string[] datavalue)
        {
            try
            {
                if (datavalue[0] == "MerchantCode")
                {
                    this.MerchantCode = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "ProductCode")
                {
                    this.Productcode = Convert.ToInt32(datavalue[1]);
                }
                else if (datavalue[0] == "FieldName")
                {
                    this.Fieldname = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "DisplaySeq")
                {
                    this.Displayseq = Convert.ToInt32(datavalue[1]);
                }
                else if (datavalue[0] == "DisplayDtDef")
                {
                    this.Displaydtdef = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "FieldLabel")
                {
                    this.Fieldlabel = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "IsMandatory")
                {
                    this.Ismandatory = Convert.ToBoolean(datavalue[1]);
                }
                else if (datavalue[0] == "DataType")
                {
                    this.Datatype = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "ValidationType")
                {
                    this.Validationtype = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "ValidationSeq")
                {
                    this.Validationseq = Convert.ToInt32(datavalue[1]);
                }
                else if (datavalue[0] == "Argument")
                {
                    this.Argument = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "ValidateOnValue")
                {
                    this.Validateonvalue = Convert.ToDecimal(datavalue[1]);
                }
                else if (datavalue[0] == "FieldName2")
                {
                    this.Fieldname2 = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "FieldName3")
                {
                    this.Fieldname3 = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "LIFldRef")
                {
                    this.Lifldref = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "isDeleted")
                {
                    this.Isdeleted = Convert.ToBoolean(datavalue[1]);
                }
                else if (datavalue[0] == "ControlName")
                {
                    this.Controlname = Convert.ToString(datavalue[1]);
                }
                else if (datavalue[0] == "IsARM")
                {
                    this.Isarm = Convert.ToBoolean(datavalue[1]);
                }                
            }
            catch //(Exception ex)
            {
            }
        }

        public bool insert_record()
        {
            if (base.newRow() == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool update_record()
        {
            base.IdentityValue = this.seq_id;

            if (base.edit(Convert.ToInt32(base.IdentityValue)) == true)
            {
                return true;
            }
            this.get_error_dbase(); return false;
        }

        public bool commit_record()
        {
            if (base.save() == true)
            {
                this.seq_id = base.IdentityValue.ToString(); return true;
            }
            this.get_error_dbase(); return false;
        }

        public void rollback_record()
        {
            base.cancel();
        }

        private void get_error_dbase()
        {
            this.error_message = string.Empty;

            if (base.Message != null && base.Message.Trim().Length > 0)
            {
                this.error_message = base.Message;
            }
        }
        #region Private Variables
        private string error_message = null;

        private string _merchantCode;
        private String MerchantCode
        {
            get { return _merchantCode; }
            set { _merchantCode = value; }
        }

        private int _productcode;
        private int Productcode
        {
            get { return _productcode; }
            set { _productcode = value; }
        }

        private string _seqid;
        private String seq_id
        {
            get { return _seqid; }
            set { _seqid = value; }
        }
        private string _fieldname;
        private String Fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }

        private int _displayseq;
        private int Displayseq
        {
            get { return _displayseq; }
            set { _displayseq = value; }
        }

        private string _displaydtdef;
        private string Displaydtdef
        {
            get { return _displaydtdef; }
            set { _displaydtdef = value; }
        }

        private string _fieldlabel;
        public string Fieldlabel
        {
            get { return _fieldlabel; }
            set { _fieldlabel = value; }
        }

        private bool _ismandatory;
        private bool Ismandatory
        {
            get { return _ismandatory; }
            set { _ismandatory = value; }
        }

        private string _datatype;
        private string Datatype
        {
            get { return _datatype; }
            set { _datatype = value; }
        }

        private string _validationtype;
        private string Validationtype
        {
            get { return _validationtype; }
            set { _validationtype = value; }
        }

        private int _validationseq;
        private int Validationseq
        {
            get { return _validationseq; }
            set { _validationseq = value; }
        }

        private string _argument;
        private string Argument
        {
            get { return _argument; }
            set { _argument = value; }
        }

        private decimal _validateonvalue;
        private decimal Validateonvalue
        {
            get { return _validateonvalue; }
            set { _validateonvalue = value; }
        }

        private string _fieldname2;
        private string Fieldname2
        {
            get { return _fieldname2; }
            set { _fieldname2 = value; }
        }

        private string _fieldname3;
        private string Fieldname3
        {
            get { return _fieldname3; }
            set { _fieldname3 = value; }
        }

        private string _lifldref;
        private string Lifldref
        {
            get { return _lifldref; }
            set { _lifldref = value; }
        }

        private bool _isdeleted;
        private bool Isdeleted
        {
            get { return _isdeleted; }
            set { _isdeleted = value; }
        }

        private string _controlname;
        private string Controlname
        {
            get { return _controlname; }
            set { _controlname = value; }
        }

        private bool _isarm;
        private bool Isarm
        {
            get { return _isarm; }
            set { _isarm = value; }
        }

        #endregion
    }
}
