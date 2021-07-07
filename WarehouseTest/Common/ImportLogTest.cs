using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake;
using Warehouse.DataLake.CsvTools;
using WarehouseTest.Helpers;

namespace WarehouseTest.Common
{
    [TestClass]
    public class ImportLogTest : GenericTest
    {
        [TestMethod]
        public void CreateImportLog()
        {
            //var ingests = new IngestTest().CreateIngestsExample(3);
            //var importLogCsv = ImportLog.CreateLog(Config, "DaluxFM", "Import log", ingests, false, false);
            //importLogCsv.Write(Path.Combine(BasePath, "Files", "Output", "Import log.csv"));
        }
    }
}
