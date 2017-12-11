using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCFileParserLibrary
{
    public class VisaProcessor
    {
        private List<VisaRecord> _records;

        public VisaProcessor(List<string> linesToProcess)
        {
            linesToProcess.ForEach(x =>
            {
                string[] split = x.Split('\t');
                VisaRecord r = VisaRecord.Create(split);

            });
        }
    }
}
