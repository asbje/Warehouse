using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLakeTest.Helpers;

namespace Warehouse.DataLake.Module.DaluxFM.Refine
{
    [TestClass]
    public class EstatesRefineTest : GenericTest
    {

        [TestMethod]
        public void EstatesRefine()
        {
            using var estatesStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Estates.xml"), FileMode.Open);

            var buildingsRefine = new BuildingsRefine("DaluxFM", estatesStream);
            var estatesRefine = new EstatesRefine("DaluxFM", estatesStream, buildingsRefine);
            var lotsRefine = new LotsRefine("DaluxFM", estatesStream);

            Assert.IsFalse(estatesRefine.HasErrors);
            Assert.IsFalse(buildingsRefine.HasErrors);
            Assert.IsFalse(lotsRefine.HasErrors);

            buildingsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "buildings.csv"));
            estatesRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "estates.csv"));
            lotsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "lots.csv"));
        }
    }
}
