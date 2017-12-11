using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCFileParserLibrary
{
    enum RecordType
    {
        CardHolder,
        Transaction,
        Account,
        Nada
    }

    public abstract class VisaRecord
    {
        RecordType Type { get; }

        public static VisaRecord Create(string[] lineData)
        {
            string[] trimLineData = Utilities.TrimString(lineData);
            switch (trimLineData[4])
            {
                case "04":
                    return new CardHolderRecord(trimLineData);
                case "05":
                    return new TransactionRecord(trimLineData);
                default:
                    throw new Exception("Didn't process");
            }
        }
    }
    
    



   
}
