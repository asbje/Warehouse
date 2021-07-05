using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.Common.CsvTools;
using WarehouseTest.Helpers;

namespace WarehouseTest.Common.CsvTools
{
    [TestClass]
    public class CsvReaderTest : GenericTest
    {

        [TestMethod]
        public void CreateCsvTest()
        {
            //using var stream = new FileStream(Path.Combine(BasePath, "Files", "Outlook", "2021-06-07-00-07-21_Bookings_from_Exchange.csv"), FileMode.Open);
            using var stream = new FileStream(Path.Combine(BasePath, "Files", "Outlook", "test.csv"), FileMode.Open);
            var csv = new CsvReader(stream, 10).CsvSet;
        }
    }
}
