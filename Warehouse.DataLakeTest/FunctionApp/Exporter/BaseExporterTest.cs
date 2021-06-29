//using Microsoft.Extensions.Logging;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Warehouse.DataLake.FunctionApp;
//using WarehouseTest.Helpers;

//namespace Warehouse.DataLakeTest.FunctionApp.Exporter
//{
//    [TestClass]
//    public class BaseExporterTest: GenericTest
//    {
//        [TestMethod]
//        public void TestRunModule()
//        {
//            var loggerMock = new Mock<ILogger>();
//            var exporter = new BaseExporter(Config, loggerMock.Object, "DaluxFM", "0 0 1 * * *", null);

//            var b = exporter..Run();
//            Assert.IsTrue(exporter.RunModule());
//        }
//    }
//}