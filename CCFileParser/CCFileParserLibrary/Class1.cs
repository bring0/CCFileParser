//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CCFileParserLibrary
//{
//    enum RecordType
//    {
//        CardHolder,
//        Transaction,
//        Account
//    }
//    interface IRecord
//    {
//        RecordType Type { get; }
//        void Process(string[] lineData);

//    }

//    class TransactionRecord : IRecord
//    {
//        public RecordType Type
//        {
//            get
//            {
//                return RecordType.Transaction;

//            }
//        }
//        public string TransactionReferenceNumber { get; set; }
//        public string AccountNumber { get; set; }
//        public string TransactionType { get; set; }
//        public string SourceAmount { get; set; }
//        public string SourceCurrencyCode { get; set; }
//        public string BillingAmount { get; set; }
//        public string TaxAmount { get; set; }
//        public string CustomerVatNumber { get; set; }
//        public string MemoPostFlag { get; set; }
//        public string CardAcceptor { get; set; }
//        public string SupplierName { get; set; }
//        public string SupplierCity { get; set; }
//        public string SupplierState { get; set; }
//        public string SupplierPostalCode { get; set; }
//        public string SupplierISOCountryCode { get; set; }
//        public string SupplierVatNumber { get; set; }

//        public void Process(string[] lineData)
//        {
//            string[] trimLines = Utilities.TrimString(lineData);
//            TransactionReferenceNumber = trimLines[3];
//            AccountNumber = trimLines[1];
//            TransactionType = trimLines[17];//GetTransactionTypeCode(trimLines[17]);
//            SourceAmount = ;
//            SourceCurrencyCode = trimLines[15];
//            BillingAmount = ;
//            TaxAmount = ;
//            CustomerVatNumber = trimLines[27];
//            MemoPostFlag = trimLines[51];
//            CardAcceptor = trimLines[7];
//            SupplierName = trimLines[8];
//            SupplierCity = trimLines[9];
//            SupplierState = trimLines[10];
//            SupplierPostalCode = trimLines[12];
//            SupplierISOCountryCode = trimLines[11];
//            SupplierVatNumber = trimLines[25];
//        }
//    }





    
//}
