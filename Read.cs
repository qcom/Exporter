using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exporter
{
    class Read
    {
        public static string Platform()
        {
            string platformString = "";
            bool moreThanOnce = false;

            while (Valid.Platform(platformString).Item1 == false)
            {
                if (moreThanOnce)
                    Console.WriteLine(Valid.Platform(platformString).Item2);

                string instructions = "Please choose your export platforms:";
                Util.PrintOptions(instructions, Program.platformOptions);

                platformString = Console.ReadLine();

                moreThanOnce = true;
            }

            return platformString;
        }

        public static string Query()
        {
            string queryString = "";
            bool moreThanOnce = false;

            while (Valid.Query(queryString).Item1 == false)
            {
                if (moreThanOnce)
                    Console.WriteLine(Valid.Query(queryString).Item2);

                Console.WriteLine();
                Console.WriteLine("Please enter your export query:");
                queryString = Console.ReadLine();

                moreThanOnce = true;
            }

            return queryString;
        }

        public static List<string> AmazonDepartments(string query)
        {
            string amazonDepartmentString = "";
            bool moreThanOnce = false;

            List<string> availableDepartments = Util.GetAmazonDepartments(query);

            if (availableDepartments.Count > 1)
            {
                while (Valid.AmazonDepartment(amazonDepartmentString, availableDepartments).Item1 == false)
                {
                    if (moreThanOnce)
                        Console.WriteLine(Valid.AmazonDepartment(amazonDepartmentString, availableDepartments).Item2);

                    string instructions = "Please select amazon departments:";
                    Util.PrintOptions(instructions, availableDepartments);

                    amazonDepartmentString = Console.ReadLine();

                    moreThanOnce = true;
                }
            }
            else
                return availableDepartments;

            return Parse.Options(amazonDepartmentString, availableDepartments);
        }

        public static string LoadMode()
        {
            string loadModeString = "";
            bool moreThanOnce = false;

            while (Valid.LoadMode(loadModeString).Item1 == false)
            {
                if (moreThanOnce)
                    Console.WriteLine(Valid.Query(loadModeString).Item2);

                string instructions = "Please select a load mode:";
                Util.PrintOptions(instructions, Program.loadModeOptions);

                loadModeString = Console.ReadLine();

                moreThanOnce = true;
            }

            return (loadModeString == "1") ? "F" : "I";
        }

        public static string DestinationStyle()
        {
            string destinationStyleString = "";
            bool moreThanOnce = false;

            while (Valid.DestinationStyle(destinationStyleString).Item1 == false)
            {
                if (moreThanOnce)
                    Console.WriteLine(Valid.DestinationStyle(destinationStyleString).Item2);

                string instructions = "Please select a destination style:";
                Util.PrintOptions(instructions, Program.destinationStyleOptions);

                destinationStyleString = Console.ReadLine();

                moreThanOnce = true;
            }

            return (destinationStyleString == "1") ? "Predefined" : "Absoute";
        }

        public static string DestinationPredefined()
        {
            string destinationPredefinedString = "";
            bool moreThanOnce = false;

            while (Valid.DestinationPredefined(destinationPredefinedString).Item1 == false)
            {
                if (moreThanOnce)
                    Console.WriteLine(Valid.DestinationPredefined(destinationPredefinedString).Item2);

                string instructions = "Please select a predefined destination:";
                Util.PrintOptions(instructions, Program.destinationPredefinedOptions);

                destinationPredefinedString = Console.ReadLine();

                moreThanOnce = true;
            }

            if (destinationPredefinedString == "1")
                return "C:\\Users\\Zachary\\Desktop\\";
            else if (destinationPredefinedString == "2")
                return "C:\\Users\\Zachary\\Downloads\\";
            else
                return "C:\\Users\\Zachary\\Dropbox\\";
        }

        public static string DestinationAbsolute()
        {
            string destinationAbsoluteString = "";
            bool moreThanOnce = false;

            while (Valid.DestinationAbsolute(destinationAbsoluteString).Item1 == false)
            {
                if (moreThanOnce)
                    Console.WriteLine(Valid.DestinationAbsolute(destinationAbsoluteString).Item2);

                Console.WriteLine("Please enter an absolute destination");
                destinationAbsoluteString = Console.ReadLine();

                moreThanOnce = true;
            }

            return destinationAbsoluteString;
        }
    }
}