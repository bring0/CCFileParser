using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCFileParserLibrary
{
    public class CardHolderRecord : VisaRecord
    {
        RecordType Type
        {
            get
            {
                return RecordType.Transaction;

            }
        }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string CardHolderId { get; set; }
        public string City { get; set; }
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostalCode { get; set; }
        public string State { get; set; }

        public CardHolderRecord(string[] lineData)
        {
            AddressLine1 = lineData[6];
            AddressLine2 = lineData[7];
            CardHolderId = lineData[2];
            City = lineData[8];
            EmployeeId = lineData[22];
            FirstName = lineData[4];
            LastName = lineData[5];
            PostalCode = lineData[11];
            State = lineData[11];
        }
    }
}
