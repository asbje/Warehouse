using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.Common.CsvTools;
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

            var meteringPointsRefine = new MeteringPointsRefine("Eloverblik", meteringPoints);
            var meteringPointsDetailsRefine = new MeteringPointsDetailsRefine("Eloverblik", meteringPointsDetails, meteringPointsRefine);
            
            Assert.IsFalse(meteringPointsRefine.HasErrors);
            Assert.IsFalse(meteringPointsDetailsRefine.HasErrors);

            meteringPointsDetailsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "meteringPoints.csv"));
        }

        
    }
}
