using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.FunctionApp;
using Warehouse.DataLakeTest.Helpers;

namespace Warehouse.DataLakeTest.Module.Eloverblik
{
    [TestClass]
    public class ExporterTest: GenericTest
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
                var exporter = new DataLake.Module.Eloverblik.Exporter(Config, loggerMock.Object);
                res = BaseExporter.Run(exporter, saveToServer);
            }
            else
            {
                var meteringPoints = GetData("MeteringPoints.json");
                var meteringPointsDetails = GetData("MeteringPointsDetails.json");
                var readingsPerYear = GetData("RedingsPerYear_2020.json");
                var exporter = new DataLake.Module.Eloverblik.Exporter(Config, loggerMock.Object, meteringPoints, meteringPointsDetails, readingsPerYear);
                res = BaseExporter.Run(exporter, saveToServer);
            }

            Assert.IsTrue(res.RunModule);
            Assert.IsTrue(res.AppSettingsOk);
            Assert.IsTrue(res.CMDModel != null);
            Assert.IsTrue(res.ImportLog != null);

            if (saveToServer)
                return;

            foreach (var item in res.Refines)
                item.CsvSet.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", item.TableName + ".csv"));

            File.WriteAllText(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "model.json"), JsonConvert.SerializeObject(res.CMDModel, Formatting.Indented));

            res.ImportLog.Write(Path.Combine(BasePath, "Files", "Eloverblik", "Out", "importLog.csv"));

        }

        public static HttpResponseMessage GetData(string filename)
        {
            var data = File.ReadAllText(Path.Combine(BasePath, "Files", "Eloverblik", "In", filename), Encoding.UTF8);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(data)};
        }
    }
}