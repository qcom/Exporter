using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exporter
{
    struct Target
    {
        public string Platform;
        public string FilePath;
        public string AmazonDepartment;
        public string Query;

        public Target(string Platform, string FilePath, string AmazonDepartment, string Query)
        {
            this.Platform = Platform;
            this.FilePath = FilePath;
            this.AmazonDepartment = AmazonDepartment;
            this.Query = Query;
        }
    };

    class Program
    {
        public static string version = "v0.1";
        public static string header = String.Format("### HPI Exporter {0} ###", version);

        public static string connString = "Data Source=ZACH7-PC;Initial Catalog=Products;Integrated Security=True";
        public static string queryPrefix = "select * from Products where ";

        public static List<string> platformOptions = new List<string>() { "Amazon", "Ariba", "Google" };
        public static List<string> loadModeOptions = new List<string>() { "Full", "Incremental" };
        public static List<string> destinationStyleOptions = new List<string>() { "Predefined", "Absolute" };
        public static List<string> destinationPredefinedOptions = new List<string>() { "Desktop", "Downloads", "Dropbox" };

        static void Main(string[] args)
        {
            Util.ClearConsole();
            List<string> platforms = Parse.Options(Read.Platform(), platformOptions);

            Util.ClearConsole();
            string query = Read.Query();

            Util.ClearConsole();
            List<string> amazonDepartments = null;
            if (platforms.Contains("Amazon"))
                amazonDepartments = Read.AmazonDepartments(query);

            Util.ClearConsole();
            string loadMode = null;
            if (platforms.Contains("Ariba"))
                loadMode = Read.LoadMode();

            Util.ClearConsole();
            string destinationStyle = Read.DestinationStyle();

            Util.ClearConsole();
            string destination = null;
            if (destinationStyle == "Predefined")
                destination = Read.DestinationPredefined();
            else
                destination = Read.DestinationAbsolute();

            List<Target> targets = Util.GetTargets(platforms, query, destination, amazonDepartments);

            Util.ClearConsole();
            Util.ExportTargets(targets, loadMode);
        }
    }
}