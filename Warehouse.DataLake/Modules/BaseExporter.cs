using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NCrontab;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.DataLake.Common;
using Warehouse.DataLake.Common.CsvTools;

namespace Warehouse.DataLake.Modules
{
    public class BaseExporter : IExporter
    {
        public readonly IConfigurationRoot Config;
        public readonly ILogger Log;
        public readonly string ModuleName;
        public readonly string ScheduleExpression;
        public readonly List<string> MandatoryAppSettings = new List<string> { "RunModules", "DataLakeAccountName", "DataLakeAccountKey", "DataLakeServiceUrl", "DataLakeBasePath" };

        public BaseExporter(IConfigurationRoot config, ILogger log, string moduleName, string scheduleExpression, string[] mandatoryAppSettings)
        {
            Config = config;
            Log = log;
            ModuleName = moduleName;
            ScheduleExpression = scheduleExpression;
            if (mandatoryAppSettings != null)
                MandatoryAppSettings.AddRange(mandatoryAppSettings);
        }

        public static ExportResult Run(IExporter exporter, bool uploadToDataLake)
        {
            var result = new ExportResult();
            result.DoRunSchedule = exporter.DoRunSchedule(DateTime.UtcNow);
            result.AppSettingsOk = exporter.VerifyAppSettings();
            
            if (result.DoRunSchedule && result.AppSettingsOk)
            {
                var refines = exporter.Export(uploadToDataLake);
                if (refines != null)
                {
                    result.Refines.AddRange(refines);
                    result.CMDModel = exporter.CreateCommonDataModel(result.Refines, uploadToDataLake);
                    result.ImportLog = exporter.CreateImportLog(result.Refines, uploadToDataLake);
                }
            }
            return result;
        }

        /// <summary>If called module should be run, due to the current module scheduleExpression</summary>
        public bool DoRunSchedule(DateTime now)
        {
            return new NextRun("Call_modules_each_hour").DoRun(now, ScheduleExpression);
        }

        /// <summary>If called module mandatory appSettings are present</summary>
        public bool VerifyAppSettings()
        {
            var res = true;
            foreach (var name in MandatoryAppSettings)
                if (Config[name] == null)
                {
                    Log.LogError($"The appSetting: {name} are missing.");
                    res = false;
                }

            return res;
        }

        public virtual IEnumerable<IRefine> Export(bool ingestToDataLake)
        {
            return default;
        }

        public JObject CreateCommonDataModel(List<IRefine> refines, bool uploadToDataLake)
        {
            var dataLake = new Common.DataLake(Config, ModuleName, "current");

            if (!uploadToDataLake || refines.Any(o => o.IsUploaded))
            {
                var model = new CommonDataModel(Config, ModuleName, refines, uploadToDataLake);
                Log.LogInformation($"Created ImportLog and model.json. There are {refines.Count(o => o.IsUploaded)} csv files + importLog.csv and model.csv.");
                return model.Model;
            }
            else
                Log.LogError($"There should have been uploaded {refines.Count(o => o.IsUploaded)} files, but there where {refines.Select(o => o.Errors).Count() } errors, that are described in importLog.csv.");

            //Jeg skal slette alle filer som ikke har været uploadet til current
            //Ingest.DeleteCurrentDirectoryInDatalake(Config, FunctionApp.TimerTrigger.database);  //Notice

            return default;
        }

        public CsvSet CreateImportLog(List<IRefine> refines, bool uploadToDataLake)
        {
            return ImportLog.CreateLog(Config, ModuleName, "importLog", refines, uploadToDataLake);
        }
    }

    public interface IExporter
    {
        bool DoRunSchedule(DateTime now);
        bool VerifyAppSettings();
        IEnumerable<IRefine> Export(bool ingestToDataLake);
        JObject CreateCommonDataModel(List<IRefine> refines, bool uploadToDataLake);
        CsvSet CreateImportLog(List<IRefine> refines, bool uploadToDataLake);
    }
}