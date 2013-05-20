using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exporter
{
    class Valid
    {
        public static Tuple<bool, string> OptionRange(string source, List<string> options)
        {
            List<int> validOptions = new List<int>();
            for (int i = 0; i < options.Count; i++)
                validOptions.Add(i + 1);

            List<char> acceptableChars = new List<char>() { '*', '-', ',', ' ' };

            Dictionary<char, int> occurrences = new Dictionary<char, int>()
            {
                {'n', 0},
                {'*', 0},
                {'-', 0},
                {',', 0},
                {' ', 0}
            };

            foreach (char c in source.Trim())
            {
                if (Util.IsDigit(c))
                    ++occurrences['n'];
                else if (acceptableChars.Contains(c))
                    ++occurrences[c];
            }

            if (occurrences[','] >= 1)
                if ((occurrences['n'] - occurrences[',']) != 1)
                    return new Tuple<bool, string>(false, "Please enter the correct amount of delimiters");

            foreach (char c in source)
            {
                if (Util.IsDigit(c) && !validOptions.Contains((int)Char.GetNumericValue(c)))
                    return new Tuple<bool, string>(false, "Please only enter numbers listed above");
                else if ((c == '*' || c == '-') && occurrences[c] > 1)
                    return new Tuple<bool, string>(false, "Please enter only one " + c);
                else if (!Util.IsDigit(c) && !acceptableChars.Contains(c))
                    return new Tuple<bool, string>(false, "Please only enter either an asterisk, a range, a number listed above, or numbers listed above with commas and or spaces");
            }

            return new Tuple<bool, string>(true, null);
        }

        public static Tuple<bool, string> Platform(string platformString)
        {
            if (platformString == "")
                return new Tuple<bool, string>(false, "Please select at least one platform");
            else if (OptionRange(platformString, Program.platformOptions).Item1 == false)
                return OptionRange(platformString, Program.platformOptions);
            else
                return new Tuple<bool, string>(true, null);
        }

        public static Tuple<bool, string> Query(string queryString)
        {
            string query = queryString.ToLower();

            if (query == "")
                return new Tuple<bool, string>(false, "Please enter a query");
            else if (query.Contains("select"))
                return new Tuple<bool, string>(false, "Please do not issue a query with a select clause");
            else if (query.Contains("from"))
                return new Tuple<bool, string>(false, "Please do not issue a query with a from clause");
            else if (query.Contains("where"))
                return new Tuple<bool, string>(false, "Please omit the where keyword from your query");
            else if (query.Contains("amazondepartment"))
                return new Tuple<bool, string>(false, "Please do not issue a query with an AmazonDepartment");
            else
                return new Tuple<bool, string>(true, null);
        }

        public static Tuple<bool, string> AmazonDepartment(string departmentString, List<string> availableDepartments)
        {
            if (departmentString == "")
                return new Tuple<bool, string>(false, "Please select at least one of the amazon departments");
            else if (OptionRange(departmentString, availableDepartments).Item1 == false)
                return OptionRange(departmentString, availableDepartments);
            else
                return new Tuple<bool, string>(true, null);
        }

        public static Tuple<bool, string> LoadMode(string loadModeString)
        {
            loadModeString = loadModeString.ToLower();

            if (loadModeString == "")
                return new Tuple<bool, string>(false, "Please select a load mode");
            else if (OptionRange(loadModeString, Program.loadModeOptions).Item1 == false)
                return OptionRange(loadModeString, Program.loadModeOptions);
            else
                return new Tuple<bool, string>(true, null);
        }

        public static Tuple<bool, string> DestinationStyle(string destinationStyleString)
        {
            if (destinationStyleString == "")
                return new Tuple<bool, string>(false, "Please select a destination option");
            else if (OptionRange(destinationStyleString, Program.destinationStyleOptions).Item1 == false)
                return OptionRange(destinationStyleString, Program.destinationStyleOptions);
            else
                return new Tuple<bool, string>(true, null);
        }

        public static Tuple<bool, string> DestinationPredefined(string destinationPredefinedString)
        {
            if (destinationPredefinedString == "")
                return new Tuple<bool, string>(false, "Please select a predefined destination");
            else if (OptionRange(destinationPredefinedString, Program.destinationPredefinedOptions).Item1 == false)
                return OptionRange(destinationPredefinedString, Program.destinationPredefinedOptions);
            else
                return new Tuple<bool, string>(true, null);
        }

        public static Tuple<bool, string> DestinationAbsolute(string destinationAbsoluteString)
        {
            return new Tuple<bool, string>(true, null);
        }
    }
}