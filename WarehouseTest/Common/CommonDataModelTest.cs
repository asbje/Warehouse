using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using WarehouseTest.Helpers;

namespace WarehouseTest.Common
{
    [TestClass]
    public class CommonDataModelTest : GenericTest
    {
        //[TestMethod]
        //public void CommonDataModel()
        //{
        //    var ingests = new IngestTests().CreateIngestsExample(3);
        //    var commonModel = new CommonDataModel(Config, "DaluxFM", ingests, false);
        //    var model = commonModel.Model;
        //    var dataAsString = JsonConvert.SerializeObject(model, Formatting.Indented);
        //    var path = Path.Combine(BasePath, "Files", "Output", "model.json");
        //    File.WriteAllText(path, dataAsString);
        //}

        ///Should not run directly
        //[TestMethod]
        //public void CommonDataModelFromDataLake()
        //{
        //    var dataLake = new DataLake(Config, "DaluxFM", "current");
        //    var ingests = dataLake.GetFilesAsIngests(20).ToList();

        //    if (ingests == null)
        //        return;

        //    var importLogCsv = ImportLog.CreateLog(Config, "DaluxFM", "Import log", ingests, false, false);

        //    var commonModel = new CommonDataModel(Config, "DaluxFM", ingests, false);
        //    var model = commonModel.Model;
        //    var dataAsString = JsonConvert.SerializeObject(model, Formatting.Indented);
        //    var path = Path.Combine(BasePath, "Files", "Output", "model.json");
        //    File.WriteAllText(path, dataAsString);
        //}
    }
}
