using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLakeTest.Helpers;

namespace Warehouse.DataLakeTest.FunctionApps
{
    [TestClass]
    public class TimerTriggerTest:GenericTest
    {
        [TestMethod]
        public void RunDaluxFMModuleWithLocaleFiles()
        {
            var loggerMock = new Mock<ILogger>();
            using var assetsStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Assets.xml"), FileMode.Open);
            using var estatesStream = new FileStream(Path.Combine(BasePath, "Files", "DaluxFM", "In", "Estates.xml"), FileMode.Open);
            var res = DataLake.FunctionApps.DurableFunctionApp.Run("DaluxFM", Config, loggerMock.Object, new object[] { estatesStream, assetsStream });
            var errors = res.Refines.Where(o=> o.HasErrors);
            Assert.IsFalse(errors.Any());

            Assert.IsTrue(res.DoRunSchedule);
            Assert.IsTrue(res.AppSettingsOk);
            Assert.IsTrue(res.CMDModel != null);
            Assert.IsTrue(res.ImportLog != null);
            Assert.IsFalse(loggerMock.Invocations.Any(o => (LogLevel)o.Arguments[0] == LogLevel.Error));

            foreach (var item in res.Refines)
                item.CsvSet.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", item.TableName + ".csv"));

            File.WriteAllText(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "model.json"), JsonConvert.SerializeObject(res.CMDModel, Formatting.Indented));

            res.ImportLog.Write(Path.Combine(BasePath, "Files", "DaluxFM", "Out", "importLog.csv"));

        }

        [TestMethod]
        public void RunDaluxFMModule()  //Notice: Calls webservices, so takes time
        {
            var loggerMock = new Mock<ILogger>();
            var result = DataLake.FunctionApps.DurableFunctionApp.Run("DaluxFM", Config, loggerMock.Object);
            var errors = result.Refines.Select(o => o.Errors);
            Assert.IsFalse(errors.Any());
            Assert.IsFalse(loggerMock.Invocations.Any(o => (LogLevel)o.Arguments[0] == LogLevel.Error));
        }
    }
}
