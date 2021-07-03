using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.Modules;
using Warehouse.DataLake.ModulesTest.Helpers;

namespace Warehouse.DataLake.ModulesTest.DaluxFM
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
                var exporter = new Modules.DaluxFM.Exporter(Config, loggerMock.Object);
                res = BaseExporter.Run(exporter, saveToServer);
            }
            else
            {
                using var assetsStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Assets.xml"), FileMode.Open);
                using var estatesStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Estates.xml"), FileMode.Open);
                var exporter = new Modules.DaluxFM.Exporter(Config, loggerMock.Object, estatesStream, assetsStream);
                res = BaseExporter.Run(exporter, saveToServer);
            }

            var errors = res.Refines.Where(o => o.HasErrors);
            Assert.IsFalse(errors.Any());
            Assert.IsTrue(res.AppSettingsOk);
            Assert.IsTrue(res.CMDModel != null);
            Assert.IsTrue(res.ImportLog != null);
            Assert.IsFalse(loggerMock.Invocations.Any(o => (LogLevel)o.Arguments[0] == LogLevel.Error));

            if (saveToServer)
                return;

            foreach (var item in res.Refines)
                item.CsvSet.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", item.TableName + ".csv"));

            File.WriteAllText(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "model.json"), JsonConvert.SerializeObject(res.CMDModel, Formatting.Indented));

            res.ImportLog.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "importLog.csv"));
        }
    }
}