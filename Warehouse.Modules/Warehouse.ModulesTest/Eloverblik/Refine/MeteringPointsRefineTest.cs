using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake.CsvTools;
using Warehouse.Modules;
using Warehouse.Modules.Eloverblik.Refine;
using Warehouse.ModulesTest.Helpers;

namespace Warehouse.ModulesTest.Eloverblik.Refine
{
    [TestClass]
    public class MeteringPointsRefineTest: GenericTest
    {

        [TestMethod]
        public void MeteringPointsRefine()
        {
            var meteringPoints = ExporterTest.GetData("MeteringPoints.json");

            var exporter = new ExporterBase(null, null, "Eloverblik", "0 * * * *", null);
            var meteringPointsRefine = new MeteringPointsRefine(exporter, meteringPoints);
            
            Assert.IsFalse(meteringPointsRefine.HasErrors);

            meteringPointsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "meteringPointsWithoutDetails.csv"));
        }
    }
}
