using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake.CsvTools;
using Warehouse.Modules;
using Warehouse.Modules.Eloverblik.Refine;
using Warehouse.ModulesTest.Helpers;

namespace Warehouse.ModulesTest.Eloverblik.Refine
{
    [TestClass]
    public class MeteringPointsDetailsRefineTest: GenericTest
    {

        [TestMethod]
        public void MeteringPointsDetailsRefine()
        {
            var meteringPoints = ExporterTest.GetData("MeteringPoints.json");
            var meteringPointsDetails = ExporterTest.GetData("MeteringPointsDetails.json");

            var exporter = new ExporterBase(null, null, "Eloverblik", "0 * * * *", null);
            var meteringPointsRefine = new MeteringPointsRefine(exporter, meteringPoints);
            var meteringPointsDetailsRefine = new MeteringPointsDetailsRefine(exporter, meteringPointsDetails, meteringPointsRefine);
            
            Assert.IsFalse(meteringPointsRefine.HasErrors);
            Assert.IsFalse(meteringPointsDetailsRefine.HasErrors);

            meteringPointsDetailsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "meteringPoints.csv"));
        }

        
    }
}
