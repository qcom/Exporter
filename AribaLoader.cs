using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BuilderDatabaseTest
{
    class AribaLoader
    {
        public static string supplierID = "AN01011199551";
        private static Hashtable _AribaData = new Hashtable();

        private static void LoadAriba()
        {
            _AribaData["Fields"] = new string[] {
                "Supplier ID",
                "Supplier Part ID",
                "Manufacturer Part ID",
                "Item Description",
                "SPSC Code",
                "Unit Price",
                "Unit of Measure",
                "Lead Time",
                "Manufacturer Name",
                "Supplier URL",
                "Manufacturer URL",
                "Market Price"
            };

            _AribaData["Version"] = "CIF_I_V3.0";
            _AribaData["Header"] = new Dictionary<string, string>()
            {
                {"CURRENCY","USD"},
                {"CHARSET","UTF-8"},
                {"CODEFORMAT","UNDPUNSPSC"},
                // {"COMMENTS",""},
                {"UNUOM","FALSE"},
                {"SUPPLIERID_DOMAIN","NETWORKID"},
                {"FIELDNAMES",String.Join(",", (string[])_AribaData["Fields"])},
                {"LOADMODE",""},
                {"TIMESTAMP",DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")},
                {"ITEMCOUNT",""}
            };

            _AribaData["Map"] = new Dictionary<string, string>()
            {
                {"Supplier ID",null},
                {"Supplier Part ID","SKU"},
                {"Manufacturer Part ID","SKU"},
                {"Item Description","Name"},
                {"SPSC Code","UNSPSC"},
                {"Unit Price","Price"},
                {"Unit of Measure","UOM"},
                {"Lead Time","LeadTime"},
                {"Manufacturer Name","Manufacturer"},
                {"Supplier URL","URL"},
                {"Manufacturer URL","ManufacturerURL"},
                {"Market Price","MSRP"}
            };
        }

        public static Hashtable Load()
        {
            LoadAriba();
            return _AribaData;
        }
    }
}