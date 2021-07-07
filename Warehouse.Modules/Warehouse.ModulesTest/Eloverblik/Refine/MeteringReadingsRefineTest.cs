using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake.CsvTools;
using Warehouse.Modules;
using Warehouse.Modules.Eloverblik.Refine;
using Warehouse.ModulesTest.Helpers;

namespace Warehouse.ModulesTest.Eloverblik.Refine
{
    [TestClass]
    public class MeteringReadingsRefineTest : GenericTest
    {

        [TestMethod]
        public void MeteringReadingsRefine()
        {
            var meteringReadings = ExporterTest.GetData("RedingsPerYear_2020.json");

            var exporter = new ExporterBase(null, null, "Eloverblik", "0 * * * *", null);
            var meteringReadingsRefine = new MeteringReadingsRefine(exporter, "readingsPerYear", meteringReadings);
            
            Assert.IsFalse(meteringReadingsRefine.HasErrors);

            meteringReadingsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "readingsPerYear.csv"));
        }
    }
}
