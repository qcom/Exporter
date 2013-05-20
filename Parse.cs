using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exporter
{
    class Parse
    {
        private static char[] delimiters = new char[] { ',' };

        public static List<string> Options(string s, List<string> options)
        {
            /*** Convert to List<string> ***/

            List<string> parsed = new List<string>();
            List<string> result = new List<string>();

            s = s.Replace(" ", "");

            if (s == "*")
                return options;
            else if (s.Contains('-'))
            {
                string str = s.Replace(" ", "");

                int dashIndex = str.IndexOf('-');

                int start = (int)Char.GetNumericValue((str[dashIndex - 1])) - 1;
                int end = (int)Char.GetNumericValue(str[dashIndex + 1]) - 1;

                for (int i = start; i <= end; i++)
                    result.Add(options[i]);
            }
            else
            {
                bool foundDelimiter = false;

                foreach (char delimiter in delimiters)
                {
                    if (s.Contains(delimiter))
                    {
                        parsed = s.Split(delimiter).ToList();
                        foundDelimiter = true;
                        break;
                    }
                }

                if (!foundDelimiter)
                    parsed = s.ToCharArray().Select(c => c.ToString()).ToArray().ToList();
            }

            /*** Map to Options ***/

            foreach (string selection in parsed)
                result.Add(options[Convert.ToInt32(selection) - 1]);

            return result;
        }
    }
}