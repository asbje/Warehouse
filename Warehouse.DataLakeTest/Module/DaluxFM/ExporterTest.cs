using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.IO;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.FunctionApp;
using Warehouse.DataLakeTest.Helpers;

namespace Warehouse.DataLakeTest.Module.DaluxFM
{
    [TestClass]
    public class ExporterTest : GenericTest
    {
        [TestMethod]
        public void TestRunModule()
        {
            bool saveToServer = false;
            bool useDataFromService = false;
            var loggerMock = new Mock<ILogger>();

            ExportResult res;
            if (useDataFromService)
            {
                var exporter = new DataLake.Module.DaluxFM.Exporter(Config, loggerMock.Object);
                res = BaseExporter.Run(exporter, saveToServer);
            }
            else
            {
                using var assetsStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Assets.xml"), FileMode.Open);
                using var estatesStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Estates.xml"), FileMode.Open);
                var exporter = new DataLake.Module.DaluxFM.Exporter(Config, loggerMock.Object, estatesStream, assetsStream);
                res = BaseExporter.Run(exporter, saveToServer);
            }

            Assert.IsTrue(res.RunModule);
            Assert.IsTrue(res.AppSettingsOk);
            Assert.IsTrue(res.CMDModel != null);
            Assert.IsTrue(res.ImportLog != null);

            if (saveToServer)
                return;

            foreach (var item in res.Refines)
                item.CsvSet.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", item.TableName + ".csv"));

            File.WriteAllText(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "model.json"), JsonConvert.SerializeObject(res.CMDModel, Formatting.Indented));

            res.ImportLog.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "importLog.csv"));
        }
    }
}