//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;
//using System.IO;

//namespace IRSClassLib.IBATCH_Class
//{
//    public class GACEExtracter
//    {
//        private string outPath = AppDomain.CurrentDomain.BaseDirectory + "temp";
//        private string path;

//        public void extractERPS(string fileName)
//        {
//            IRSWFiles.ERPS erps;
//        }

//        public void extractPYRL(string fileName)
//        {
//            IRSWFiles.PYRL pyrl;
//        }

//        public GACEExtracter() { }
//    }

//    public class IRSWFiles
//    {
//        public struct ERPS
//        {
//            private string _referenceNumber;

//            public const string ImplementationCode = "4";
//            public const string PayMode = "1";
//            public const string AccountType = "2";
            
//            public string RemittingBankCode;
//            public string DateSent;
//            public string BatchSub;
//            public string BeneFirstName;
//            public string BeneLastName;
//            public string BeneMiddleInitial;
//            public string BeneAddress1;
//            public string BeneAddress2;
//            public string BeneAddress3;
//            public string PayTypeCode;
//            public string PNBBranchCode;
//            public string AccountNumber;
//            public string AccountCurrencyCode;
//            public string PayBankName;
//            public string RemitterName;
//            public string RemitterType;
//            public string SpecialMessage;
//            public string RemittingCurrencyCode;
//            public string CoveringBankCode;
//            public string AmountSent;
//            public string GrossAmount;

//            public string RecordLine
//            {
//                get
//                {
//                    return "";
//                }
//            }

//            public string ReferenceNumber
//            {
//                get { return refNoTheirs(); }
//            }

//            private string refNoTheirs()
//            {
//                string seq = new irsBatchFileUpload().sequence();
//                string dd = new irsTimeZone().ServerTime.ToString("MMddyy");
//                this._referenceNumber = this.RemittingBankCode.Substring(1, 3) + dd + seq;
//                return this._referenceNumber;
//            }

//            private string pad(string val, char padder, int len, bool padLeft)
//            {
//                string ret_val = string.Empty;
//                int pad_len = len - val.Length;

//                for (int i = 0; i < pad_len; i++)
//                {
//                    ret_val += padder;
//                }

//                if (leftpadded)
//                {
//                    return ret_val + val;
//                }
//                else
//                {
//                    return val + ret_val;
//                }
//            }
//        }

//        public struct PYRL
//        {
//            public string RecordLine
//            {
//                get
//                {
//                    return "";
//                }
//            }

//            public string ReferenceNumber
//            {
//                get { return refNoTheirs(); }
//            }

//            private string refNoTheirs()
//            {
//                string seq = new irsBatchFileUpload().sequence();
//                string dd = new irsTimeZone().ServerTime.ToString("MMddyy");
//                this._referenceNumber = this.RemittingBankCode.Substring(1, 3) + dd + seq;
//                return this._referenceNumber;
//            }

//            private string pad(string val, char padder, int len, bool padLeft)
//            {
//                string ret_val = string.Empty;
//                int pad_len = len - val.Length;

//                for (int i = 0; i < pad_len; i++)
//                {
//                    ret_val += padder;
//                }

//                if (leftpadded)
//                {
//                    return ret_val + val;
//                }
//                else
//                {
//                    return val + ret_val;
//                }
//            }
//        }

//        public IRSWFiles() { }
//    }
//}
