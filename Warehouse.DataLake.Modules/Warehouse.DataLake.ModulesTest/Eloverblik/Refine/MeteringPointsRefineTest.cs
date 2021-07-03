using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.Modules.Eloverblik.Refine;
using Warehouse.DataLake.ModulesTest.Helpers;

namespace Warehouse.DataLake.ModulesTest.Eloverblik.Refine
{
    [TestClass]
    public class MeteringPointsRefineTest: GenericTest
    {

        [TestMethod]
        public void MeteringPointsRefine()
        {
            var meteringPoints = ExporterTest.GetData("MeteringPoints.json");

            var meteringPointsRefine = new MeteringPointsRefine("Eloverblik", meteringPoints);
            
            Assert.IsFalse(meteringPointsRefine.HasErrors);

            meteringPointsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "meteringPointsWithoutDetails.csv"));
        }
    }
}
