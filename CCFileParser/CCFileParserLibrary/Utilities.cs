using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCFileParserLibrary
{
    public static class Utilities
    {
        public static string[] TrimString(string[] stringToTrim)
        {
            string[] rRet = new string[stringToTrim.Length];
            for (int i = 0; i < stringToTrim.Length; i++)
            {
                rRet[i] = stringToTrim[i].Trim();
            }
            return rRet;
        }
    }
}
