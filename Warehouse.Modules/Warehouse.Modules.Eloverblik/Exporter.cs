using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Warehouse.Modules.Eloverblik.Refine;
using Warehouse.Modules.Eloverblik.Service;

namespace Warehouse.Modules.Eloverblik
{
    public class Exporter : BaseExporter
    {
        private static readonly string moduleName = "Eloverblik";
        private static readonly string scheduleExpression = "0 1 * * *";
        private static readonly string[] mandatoryAppSettings = new string[] { "EloverblikToken", "EloverblikBaseUrl" };
        private readonly bool useTestData = false;
        private HttpResponseMessage meteringPoints;
        private HttpResponseMessage meteringPointsDetails;
        private HttpResponseMessage meteringReadingsPerYear;

        public Exporter(IConfigurationRoot config, ILogger log) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings) { }

        public Exporter(IConfigurationRoot config, ILogger log, object[] data) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings)
        {
            useTestData = true;
            this.meteringPoints = data[0] as HttpResponseMessage;
            this.meteringPointsDetails = data[1] as HttpResponseMessage;
            this.meteringReadingsPerYear = data[2] as HttpResponseMessage;
        }

        public Exporter(IConfigurationRoot config, ILogger log, HttpResponseMessage meteringPoints, HttpResponseMessage meteringPointsDetails, HttpResponseMessage meteringReadingsPerYearData) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings)
        {
            useTestData = true;
            this.meteringPoints = meteringPoints;
            this.meteringPointsDetails = meteringPointsDetails;
            this.meteringReadingsPerYear = meteringReadingsPerYearData;
        }

        public override IEnumerable<IRefine> Export(bool ingestToDataLake)
        {
            var service = new WebService(Config["EloverblikToken"], Config["EloverblikBaseUrl"]);

            if(!useTestData)
                meteringPoints = service.GetMeteringPoints().Result;
            
            var meteringPointsRefine = new MeteringPointsRefine(moduleName, meteringPoints);
            var meteringPointIds = meteringPointsRefine.GetMeteringPointIds();

            if (!useTestData)
                meteringPointsDetails = service.GetMeteringPointsDetails(meteringPointIds).Result;

            var meteringPointsDetailsRefine = new MeteringPointsDetailsRefine(moduleName, meteringPointsDetails, meteringPointsRefine);

            if (!useTestData)
                meteringReadingsPerYear = service.GetMeterDataTimeSeries(meteringPointIds, new DateTime(2020, 1, 1), new DateTime(2021, 1, 1), TimeAggregation.Year).Result;

            var meteringReadingsPerYearRefine = new MeteringReadingsRefine(moduleName, "readingsPerYear", meteringReadingsPerYear);

            if (ingestToDataLake)
            {
                var fileDate = DateTime.UtcNow;

                var meteringPointsStream = meteringPoints.Content.ReadAsStreamAsync().Result;
                meteringPointsRefine.UploadFile(Config, fileDate, "json", meteringPointsStream, true, false, false, false);

                var meteringPointsDetailsStream = meteringPointsDetails.Content.ReadAsStreamAsync().Result;
                meteringPointsDetailsRefine.UploadFile(Config, fileDate, "json", meteringPointsDetailsStream, true, true, true, false);

                var meteringReadingsPerYearStream = meteringReadingsPerYear.Content.ReadAsStreamAsync().Result;
                meteringReadingsPerYearRefine.UploadFile(Config, fileDate, "json", meteringReadingsPerYearStream, true, false, true, false);
            }
            return new List<IRefine> { meteringPointsDetailsRefine, meteringReadingsPerYearRefine };
        }

    }
}