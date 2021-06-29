using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NCrontab;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.DataLake.Common;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.Module;

namespace Warehouse.DataLake.FunctionApp
{
    public class BaseExporter : IExporter
    {
        public readonly IConfigurationRoot Config;
        public readonly ILogger Log;
        public readonly string ModuleName;
        public readonly List<string> MandatoryAppSettings = new List<string> { "RunModules", "DataLakeAccountName", "DataLakeAccountKey", "DataLakeServiceUrl", "DataLakeBasePath" };

        public BaseExporter(IConfigurationRoot config, ILogger log, string moduleName, string[] mandatoryAppSettings)
        {
            Config = config;
            Log = log;
            ModuleName = moduleName;
            //ScheduleExpression = scheduleExpression;
            if (mandatoryAppSettings != null)
                MandatoryAppSettings.AddRange(mandatoryAppSettings);
        }

        public static ExportResult Run(IExporter exporter, bool uploadToDataLake)
        {
            var res = new ExportResult();
            res.RunModule = exporter.RunModule();
            //res.RunSchedule = exporter.RunSchedule();
            res.AppSettingsOk = exporter.VerifyAppSettings();
            
            if (res.RunModule && res.AppSettingsOk)
            {
                var refines = exporter.Export(uploadToDataLake);
                if (refines != null)
                {
                    res.Refines.AddRange(refines);
                    res.CMDModel = exporter.CreateCommonDataModel(res.Refines, uploadToDataLake);
                    res.ImportLog = exporter.CreateImportLog(res.Refines, uploadToDataLake);
                }
            }
            return res;
        }

        /// <summary>If called moduleName is present in appSetting: 'RunModule'</summary>
        public bool RunModule()
        {
            var modulesToRun = Config["RunModules"];
            if (string.IsNullOrEmpty(modulesToRun))
                return false;

            var modulesToRunArr = modulesToRun.Replace(" ", string.Empty).ToLower().Split(',');
            return modulesToRunArr.Any(o => o.Equals(ModuleName, StringComparison.InvariantCultureIgnoreCase));
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
        bool RunModule();
        bool VerifyAppSettings();
        IEnumerable<IRefine> Export(bool ingestToDataLake);
        JObject CreateCommonDataModel(List<IRefine> refines, bool uploadToDataLake);
        CsvSet CreateImportLog(List<IRefine> refines, bool uploadToDataLake);
    }
}