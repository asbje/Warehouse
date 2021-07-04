using Microsoft.Extensions.Configuration;
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
        private static readonly string scheduleExpression = " 0 * * * *";
        private static readonly string[] mandatoryAppSettings = new string[] { "FTPConnectionStringOutlookBookings" };
        private readonly bool useTestData = false;
        private Stream bookingsStream;

        public Exporter(IConfigurationRoot config, ILogger log) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings) { }

        /// <param name="data">[0]: bookingsStream</param>
        public Exporter(IConfigurationRoot config, ILogger log, object[] data) : base(config, log, moduleName, scheduleExpression, mandatoryAppSettings)
        {
            useTestData = true;
            this.bookingsStream = data[0] as Stream;
        }

        public override IEnumerable<IRefine> Export(bool ingestToDataLake)
        {
            var res = new List<IRefine>();
            var fileDate = DateTime.UtcNow;
            var service = new Service.FTPService(Config["FTPConnectionStringOutlookBookings"], Log);

            var locationssRefine = new LocationsRefine(moduleName);
            locationssRefine.UploadFile(Config, fileDate, false, true, false);
            res.Add(locationssRefine);

            if (!useTestData)
            {
                foreach (var item in service.GetData())
                {
                    var bookingsRefine = new BookingsRefine(moduleName, item.Value);
                    bookingsRefine.UploadFile(Config, item.Key, "csv", item.Value, true, true, false, true);  //MANGLER MÅDE FOR ACCUMULATE TIL AT HÅNDTERE NPR FILER UPLAODES
                    res.Add(bookingsRefine);

                    var partioningsRefine = new PartitioningsRefine(ModuleName, bookingsRefine, locationssRefine);
                    partioningsRefine.UploadFile(Config, item.Key, false, false, true);
                    res.Add(partioningsRefine);
                }

                //PAS PÅ MED DENNE - SLETTER MIT DATA
                //service.DeleteFolderContent();
            }
            else
            {
                var bookingsRefine = new BookingsRefine(moduleName, bookingsStream);
                bookingsRefine.UploadFile(Config, fileDate, "csv", bookingsStream, true, true, false, true);  //MANGLER MÅDE FOR ACCUMULATE TIL AT HÅNDTERE NPR FILER UPLAODES
                res.Add(bookingsRefine);

                var partioningsRefine = new PartitioningsRefine(ModuleName, bookingsRefine, locationssRefine);
                partioningsRefine.UploadFile(Config, fileDate, false, false, true);
                res.Add(partioningsRefine);
            }
            return res;
        }
    }
}