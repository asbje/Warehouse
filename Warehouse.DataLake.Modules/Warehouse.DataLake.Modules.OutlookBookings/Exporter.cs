﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using Warehouse.DataLake.Modules.OutlookBookings.Refine;

namespace Warehouse.DataLake.Modules.OutlookBookings
{
    public class Exporter : BaseExporter
    {
        private static readonly string moduleName = "OutlookBookings";
        private static readonly string scheduleExpression = " 0 1 * * *";
        private static readonly string[] mandatoryAppSettings = new string[] { "FTPConnectionStringOutlookBookings" };
        private readonly bool useTestData = false;
        private Stream estatesXmlStream;
        private Stream assetsXmlStream;

        public Exporter(IConfigurationRoot config, ILogger log) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings) { }

        public Exporter(IConfigurationRoot config, ILogger log, object[] data) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings)
        {
            useTestData = true;
            this.estatesXmlStream = data[0] as Stream;
            this.assetsXmlStream = data[1] as Stream;
        }

        public Exporter(IConfigurationRoot config, ILogger log, Stream estatesXmlStream, Stream assetsXmlStream) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings)
        {
            useTestData = true;
            this.estatesXmlStream = estatesXmlStream;
            this.assetsXmlStream = assetsXmlStream;
        }

        public override IEnumerable<IRefine> Export(bool ingestToDataLake)
        {
            //if (!useTestData)
            //{
            //    var daluxFM = new Service.WebService(Config["DaluxFMCustomerId"], Config["DaluxFMApiKey"], Config["DaluxFMUser"], Config["DaluxFMPassword"]);
            //    estatesXmlStream = daluxFM.GetEstates().Result;
            //    assetsXmlStream = daluxFM.GetAssets().Result;
            //}



            //var locationssRefine = new LocationsRefine(moduleName);
            //var bookingsRefine = new BookingsRefine(moduleName, estatesXmlStream, locationssRefine);
            //var lotsRefine = new LotsRefine(moduleName, estatesXmlStream);
            //var assetsRefine = new AssetsRefine(ModuleName, assetsXmlStream, bookingsRefine, locationssRefine, Config["DaluxFMUniqueColumns"]);

            //if (ingestToDataLake)
            //{
            //    var fileDate = DateTime.UtcNow;
            //    bookingsRefine.UploadFile(Config, fileDate, "xml", estatesXmlStream, true, true, true, false);
            //    locationssRefine.UploadFile(Config, fileDate, true, true, false);
            //    lotsRefine.UploadFile(Config, fileDate, true, true, false);
            //    assetsRefine.UploadFile(Config, fileDate, "xml", assetsXmlStream, true, true, true, false);
            //}

            //return new List<IRefine> { bookingsRefine, locationssRefine, lotsRefine };
            return default;
        }
    }
}