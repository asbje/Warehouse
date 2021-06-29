using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Warehouse.DataLake.FunctionApp
{
    public static class TimerTrigger
    {
        private static IConfigurationRoot _config;
        public static IConfigurationRoot Config { get { return _config ??= new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build(); } }

        [FunctionName("DaluxFM")]
        public static void DaluxFMTimerTrigger([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer, ILogger log)
        {
            BaseExporter.Run(new Module.DaluxFM.Exporter(Config, log), true);
        }

        [FunctionName("Eloverblik")]
        public static void EloverblikTimerTrigger([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer, ILogger log)
        {
            BaseExporter.Run(new Module.Eloverblik.Exporter(Config, log), true);
        }
    }
}