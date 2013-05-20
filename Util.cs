using BuilderDatabaseTest;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exporter
{
    class Util
    {
        public static void ClearConsole()
        {
            Console.Clear();
            Console.WriteLine(Program.header);
        }

        public static bool IsDigit(char c)
        {
            return (c >= '0' && c <= '9');
        }

        public static void PrintOptions(string instructions, List<string> options)
        {
            Console.WriteLine();
            Console.WriteLine(instructions);
            for (int i = 0; i < options.Count; i++)
                Console.WriteLine(String.Format("[{0}] {1}", i + 1, options[i]));
            Console.WriteLine();
        }

        public static List<string> GetAmazonDepartments(string query)
        {
            List<string> departments = new List<string>();

            using (SqlConnection connection = new SqlConnection(Program.connString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                DataTable result = new DataTable();

                adapter.SelectCommand = new SqlCommand("select distinct AmazonDepartment from Products where " + query, connection);
                adapter.Fill(result);

                foreach (DataRow row in result.Rows)
                    departments.Add(row["AmazonDepartment"].ToString());
            }

            return departments;
        }

        public static List<Target> GetTargets(List<string> platforms, string query, string destination, List<string> departments)
        {
            List<Target> targets = new List<Target>();

            foreach (string platform in platforms)
            {
                if (platform == "Amazon")
                {
                    foreach (string department in departments)
                    {
                        string filePath = destination + department.ToLower() + ".txt";
                        string q = query + " and AmazonDepartment = '" + department + "'";

                        targets.Add(new Target(platform, filePath, department, q));
                    }
                }
                else if (platform == "Ariba")
                    targets.Add(new Target(platform, destination + "ariba.cif", null, query));
                else
                    targets.Add(new Target(platform, destination + "google.txt", null, query));
            }

            return targets;
        }

        public static DataTable SelectRows(DataTable datatable, string query)
        {
            using (SqlConnection connection = new SqlConnection(Program.connString))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand(Program.queryPrefix + query, connection);
                adapter.Fill(datatable);

                return datatable;
            }
        }

        public static void ExportTargets(List<Target> targets, string loadMode)
        {
            foreach (Target target in targets)
            {
                DataTable result = new DataTable();
                result = Util.SelectRows(result, target.Query);

                using (StreamWriter writer = new StreamWriter(target.FilePath))
                {
                    if (target.Platform == "Amazon")
                    {
                        Hashtable amazonMap = AmazonLoader.Load(target.AmazonDepartment);

                        writer.WriteLine((string)amazonMap["Header"]);

                        string schema1 = ((string)amazonMap["Schema1"]).Replace(',', '\t');
                        string schema2 = ((string)amazonMap["Schema2"]).Replace(',', '\t');
                        writer.WriteLine(schema1);
                        writer.WriteLine(schema2);

                        int count = 0;
                        int total = result.Rows.Count;

                        foreach (DataRow row in result.Rows)
                        {
                            foreach (string attribute in (string[])amazonMap["Attributes"])
                            {
                                Dictionary<string, string> map = (Dictionary<string, string>)amazonMap["Map"];
                                if (map.ContainsKey(attribute))
                                {
                                    string value = row[map[attribute]].ToString().Replace("\n", " ").Replace(Environment.NewLine, " ").Replace("\r", " ").Replace("\t", "").Trim();
                                    writer.Write("{0}\t", value);
                                }
                                else
                                    writer.Write("\t");
                            }
                            if (++count <= total)
                                writer.WriteLine();
                        }
                    }
                    else if (target.Platform == "Ariba")
                    {
                        Hashtable aribaMap = AribaLoader.Load();

                        /*** Header ***/

                        writer.WriteLine((string)aribaMap["Version"]);

                        Dictionary<string, string> header = (Dictionary<string, string>)aribaMap["Header"];
                        header["LOADMODE"] = loadMode;
                        header["ITEMCOUNT"] = Convert.ToString(result.Rows.Count);

                        foreach (string key in header.Keys)
                            writer.WriteLine(key + ": " + header[key]);

                        /*** Body ***/

                        writer.WriteLine("DATA");

                        int count = 0;
                        int total = result.Rows.Count;

                        foreach (DataRow row in result.Rows)
                        {
                            Dictionary<string, string> map = (Dictionary<string, string>)aribaMap["Map"];

                            foreach (string aribaKey in map.Keys)
                            {
                                string value;

                                if (aribaKey == "Supplier ID")
                                    value = AribaLoader.supplierID;
                                else
                                    value = row[map[aribaKey]].ToString().Replace("\n", "").Replace(Environment.NewLine, "").Replace("\r", " ").Replace("\t", "").Trim();

                                writer.Write("{0},", value);
                            }

                            if (++count <= total)
                                writer.WriteLine();
                        }

                        writer.WriteLine("ENDOFDATA");
                    }
                    else if (target.Platform == "Google")
                    {

                    }
                }
            }
        }
    }
}