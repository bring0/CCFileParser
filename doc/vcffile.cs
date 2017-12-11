using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TEBOUtils
{
    public class VcfFile
    {
        public List<string[]> RecordLines;
        public VcfFile(string path)
        {
            RecordLines = new List<string[]>();
            if (!File.Exists(path)) return;
            try
            {
                //
                // Read in a file line-by-line, parse, and store it all in a List.
                //
                    
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parsedLine = Utils.LineParsers.GetSplitLine(line);
                        RecordLines.Add(parsedLine); // Add to list.
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}