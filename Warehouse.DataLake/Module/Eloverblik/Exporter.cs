using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Warehouse.DataLake.FunctionApp;
using Warehouse.DataLake.Module.Eloverblik.Refine;
using Warehouse.DataLake.Module.Eloverblik.Service;

namespace Warehouse.DataLake.Module.Eloverblik
{
    public class Exporter : BaseExporter
    {
        private static readonly string moduleName = "Eloverblik";
        private static readonly string[] mandatoryAppSettings = new string[] { "EloverblikToken", "EloverblikBaseUrl" };
        private readonly bool useLocalSetStreams = false;
        private HttpResponseMessage meteringPoints;
        private HttpResponseMessage meteringPointsDetails;
        private HttpResponseMessage meteringReadingsPerYear;

        public Exporter(IConfigurationRoot config, ILogger log) : base(config, log, moduleName, mandatoryAppSettings) { }

        public Exporter(IConfigurationRoot config, ILogger log, HttpResponseMessage meteringPoints, HttpResponseMessage meteringPointsDetails, HttpResponseMessage meteringReadingsPerYearData) : base(config, log, moduleName, mandatoryAppSettings)
        {
            useLocalSetStreams = true;
            this.meteringPoints = meteringPoints;
            this.meteringPointsDetails = meteringPointsDetails;
            this.meteringReadingsPerYear = meteringReadingsPerYearData;
        }

        public override IEnumerable<IRefine> Export(bool ingestToDataLake)
        {
            var service = new WebService(Config["EloverblikToken"], Config["EloverblikBaseUrl"]);

            if(!useLocalSetStreams)
                meteringPoints = service.GetMeteringPoints().Result;
            
            var meteringPointsRefine = new MeteringPointsRefine(moduleName, meteringPoints);
            var meteringPointIds = meteringPointsRefine.GetMeteringPointIds();

            if (!useLocalSetStreams)
                meteringPointsDetails = service.GetMeteringPointsDetails(meteringPointIds).Result;

            var meteringPointsDetailsRefine = new MeteringPointsDetailsRefine(moduleName, meteringPointsDetails, meteringPointsRefine);

            if (!useLocalSetStreams)
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