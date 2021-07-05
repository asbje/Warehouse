using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Warehouse.Modules.Eloverblik.Refine;
using Warehouse.Modules.Eloverblik.Service;
using Warehouse.ModulesTest.Helpers;

namespace Warehouse.ModulesTest.Eloverblik.Service
{
    [TestClass]
    public class WebServiceTest : GenericTest
    {
        private WebService _service;

        public WebService Service { get { return _service ??= new WebService(Config["EloverblikToken"], Config["EloverblikBaseUrl"]); } }
        
        [TestMethod]
        public void SendWrongCredentials()
        {
            var service = new WebService("1", Config["EloverblikBaseUrl"]);
            var result = service.GetMeteringPoints().Result;
            Assert.IsFalse(result.IsSuccessStatusCode);
            Assert.AreEqual(result.ReasonPhrase, "Unauthorized");
        }

        [TestMethod]
        public void GetMeteringPoints()
        {
            var result = Service.GetMeteringPoints().Result;
            ToFile(result.Content.ReadAsStringAsync().Result, "MeteringPoints.json");
        }

        [TestMethod]
        public void GetMeteringPointsDetails()
        {
            var meteringPointIds = new string[] { "571313175500079231" };
            var result = Service.GetMeteringPointsDetails(meteringPointIds).Result;
            ToFile(result.Content.ReadAsStringAsync().Result, "MeteringPointsDetails_One points extracted.json");
        }

        [TestMethod]
        public void GetMeteringPointsDetailsAll()
        {
            var response = Service.GetMeteringPoints().Result;
            var meteringPointIds = new MeteringPointsRefine("Eloverblik", response).GetMeteringPointIds();
            var result = Service.GetMeteringPointsDetails(meteringPointIds).Result;
            ToFile(result.Content.ReadAsStringAsync().Result, "MeteringPointsDetails.json");
        }

        [TestMethod]
        public void GetMeteringPointsCharges()
        {
            var meteringPointIds = new string[] { "571313174115454372", "571313174115454389" };
            var result = Service.GetMeteringPointsCharges(meteringPointIds).Result;
            ToFile(result.Content.ReadAsStringAsync().Result, "MeteringPointsCharges_Two points extracted.json");
        }

        [TestMethod]
        public void GetMeterDataTimeSeriesForAYearForTwoPoints()
        {
            var meteringPointIds = new string[] { "571313174115454372", "571313174115454389" };
            var dateFrom = new DateTime(2020, 1, 1);
            var dateTo = new DateTime(2021, 1, 1);

            var result = Service.GetMeterDataTimeSeries(meteringPointIds, dateFrom, dateTo, TimeAggregation.Year).Result;
            ToFile(result.Content.ReadAsStringAsync().Result, $"meterDataTimeSeries_Two points extracted_Consumption one year aggregated to 'Year'.json");

        }

        [TestMethod]
        public void GetMeterDataTimeSeriesForAYearForAllPoints()
        {
            var response = Service.GetMeteringPoints().Result;
            var meteringPointIds = new MeteringPointsRefine("Eloverblik", response).GetMeteringPointIds();
            var dateFrom = new DateTime(2020, 1, 1);
            var dateTo = new DateTime(2021, 1, 1);

            var result = Service.GetMeterDataTimeSeries(meteringPointIds, dateFrom, dateTo, TimeAggregation.Year).Result;
            ToFile(result.Content.ReadAsStringAsync().Result, "RedingsPerYear_2020.json");

        }

        [TestMethod]
        public void GetMeterDataTimeSeries()
        {
            var meteringPointIds = new string[] { "571313174115454372", "571313174115454389" };
            var dateFrom = new DateTime(2021, 1, 1);
            var dateTo = new DateTime(2021, 1, 31);

            GetMeterdataBySpecificAggregation(Service, meteringPointIds, dateFrom, dateTo, TimeAggregation.Actual);
            GetMeterdataBySpecificAggregation(Service, meteringPointIds, dateFrom, dateTo, TimeAggregation.Day);
            GetMeterdataBySpecificAggregation(Service, meteringPointIds, dateFrom, dateTo, TimeAggregation.Hour);
            GetMeterdataBySpecificAggregation(Service, meteringPointIds, dateFrom, dateTo, TimeAggregation.Month);
            GetMeterdataBySpecificAggregation(Service, meteringPointIds, dateFrom, dateTo, TimeAggregation.Quarter);
            GetMeterdataBySpecificAggregation(Service, meteringPointIds, dateFrom, dateTo, TimeAggregation.Year);
        }


        [TestMethod]
        public void GetMeterReadings()
        {
            var meteringPointIds = new string[] { "571313174115454372", "571313174115454389" };
            var dateFrom = new DateTime(2021, 1, 1);
            var dateTo = new DateTime(2021, 1, 31);

            var result = Service.GetMeterReadings(meteringPointIds, dateFrom, dateTo).Result;
            ToFile(result.Content.ReadAsStringAsync().Result, $"meterReadings_Two points extracted_Consumption for January.json");
        }

        private void GetMeterdataBySpecificAggregation(WebService service, string[] meteringPointIds, DateTime dateFrom, DateTime dateTo, TimeAggregation aggregation)
        {
            var result = service.GetMeterDataTimeSeries(meteringPointIds, dateFrom, dateTo, aggregation).Result;
            ToFile(result.Content.ReadAsStringAsync().Result, $"meterDataTimeSeries_Two points extracted_Consumption for January aggregated to '{aggregation}'.json");
        }

        private static void ToFile(string json, string fileName)
        {
            var filePath = Path.Combine(BasePath, "Files", "Eloverblik", "In", fileName);
            var prettyJson = JToken.Parse(json).ToString(Formatting.Indented);
            File.WriteAllText(filePath, prettyJson);
        }
    }
}
