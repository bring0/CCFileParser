using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCFileParserLibrary
{
    internal class TransactionRecord : VisaRecord
    {
        RecordType Type
        {
            get
            {
                return RecordType.Transaction;

            }
        }
        public string TransactionReferenceNumber { get; }
        public string AccountNumber { get; }
        public string TransactionType { get; }
        public string SourceAmount { get; }
        public string SourceCurrencyCode { get; }
        public string BillingAmount { get; }
        public string TaxAmount { get; }
        public string CustomerVatNumber { get; }
        public string MemoPostFlag { get; }
        public string CardAcceptor { get; }
        public string SupplierName { get; }
        public string SupplierCity { get; }
        public string SupplierState { get; }
        public string SupplierPostalCode { get; }
        public string SupplierISOCountryCode { get; }
        public string SupplierVatNumber { get; }
        public TransactionRecord(string[] lineData)
        {
            TransactionReferenceNumber = lineData[3];
            AccountNumber = lineData[1];
            TransactionType = lineData[17];//GetTransactionTypeCode(lineData[17]);
            //SourceAmount = ;
            SourceCurrencyCode = lineData[15];
            //BillingAmount = ;
            //TaxAmount = ;
            CustomerVatNumber = lineData[27];
            MemoPostFlag = lineData[51];
            CardAcceptor = lineData[7];
            SupplierName = lineData[8];
            SupplierCity = lineData[9];
            SupplierState = lineData[10];
            SupplierPostalCode = lineData[12];
            SupplierISOCountryCode = lineData[11];
            SupplierVatNumber = lineData[25];
        }
    }
}
