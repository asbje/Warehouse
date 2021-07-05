using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Warehouse.Common.CsvTools;
using Warehouse.ModulesTest.Helpers;
using Warehouse.Modules.DaluxFM.Refine;

namespace Warehouse.ModulesTest.DaluxFM.Refine
{
    [TestClass]
    public class AssetsRefineTest : GenericTest
    {

        [TestMethod]
        public void AssetsRefine()
        {
            using var estatesStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Estates.xml"), FileMode.Open);
            using var assetsStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM" ,"In", "Assets.xml"), FileMode.Open);

            var buildingsRefine = new BuildingsRefine("DaluxFM", estatesStream);
            var estatesRefine = new EstatesRefine("DaluxFM", estatesStream, buildingsRefine);
            var assetsRefine = new AssetsRefine("DaluxFM", assetsStream, estatesRefine, buildingsRefine, "Aftagernummer");

            Assert.IsFalse(assetsRefine.HasErrors);
            Assert.IsFalse(estatesRefine.HasErrors);
            Assert.IsFalse(buildingsRefine.HasErrors);

            assetsRefine.CsvSet.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "assets.csv"));
        }
    }
}
