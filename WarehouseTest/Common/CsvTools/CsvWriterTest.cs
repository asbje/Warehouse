using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake.CsvTools;
using WarehouseTest.Helpers;

namespace WarehouseTest.Common.CsvTools
{
    [TestClass]
    public class CsvWriterTest : GenericTest
    {

        [TestMethod]
        public void VerifyCsvWriting()
        {
            var csv = new CsvSet();
            csv.Write();

            csv = new CsvSet();
            csv.AddHeader(1, "Container");
            csv.Write();

            csv = new CsvSet();
            csv.AddRecord(1, 1, "21");
            csv.Write();

            csv = new CsvSet();
            csv.AddHeader(1, "Container");
            csv.AddRecord(1, 1, "21");
            csv.Write();

            csv = new CsvSet();
            csv.AddRecord(1, 1, "21");
            csv.AddHeader(1, "Container");
            csv.Write();

            csv = new CsvSet();
            csv.AddHeader(1, "Container");
            csv.AddRecord(3, 1, "21");
            csv.Write();

            csv = new CsvSet();
            csv.AddHeader(2, "Container");
            csv.AddRecord(1, 1, "21");
            csv.Write();
        }
    }
}
