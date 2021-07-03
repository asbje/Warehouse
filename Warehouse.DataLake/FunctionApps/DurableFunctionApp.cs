using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Warehouse.DataLake.Modules;

namespace Warehouse.DataLake.FunctionApps
{
    public static class DurableFunctionApp
    {
        [FunctionName("Call_modules_each_hour")]
        public static async Task Run([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("local.settings.json", optional: true, reloadOnChange: true).AddEnvironmentVariables().Build();
            await context.CallActivityAsync("DoCleanup", null);
            DateTime now = context.CurrentUtcDateTime;
            await context.CreateTimer(now.AddHours(1), CancellationToken.None);  // sleep for one hour between runs

            var modulesToRun = config["RunModules"];
            if (!string.IsNullOrEmpty(modulesToRun))
            {
                var modulesToRunArr = modulesToRun.Replace(" ", string.Empty).Split(',');
                foreach (var item in modulesToRunArr)
                    Run(item, config, log);
            }
            new NextRun("Call_modules_each_hour").SetLastRun(now);

            context.ContinueAsNew(null);
        }

        public static ExportResult Run(string moduleName, IConfiguration config, ILogger log, object[] data = null)
        {
            var dllPath = GetModuleDllPath(moduleName);
            if (!File.Exists(dllPath))
            {
                log.LogError($"The module; {moduleName} cannot be found. Either the name are not correct or the dll file are missing.");
                return default;
            }

            var dll = Assembly.LoadFile(dllPath);
            var type = dll.GetType("Warehouse.DataLake.Modules." + moduleName + ".Exporter");
            var objects = new List<object> { config, log };
            if (data != null)
                foreach (var item in data)
                    objects.Add(item);

            var instance = Activator.CreateInstance(type, objects.ToArray()) as IExporter;
            return BaseExporter.Run(instance, false);
        }

        private static string GetModuleDllPath(string moduleName)
        {
            var BasePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
            var moduleNamespace = "Warehouse.DataLake.Modules." + moduleName;
            var path = Path.Combine(BasePath, "Warehouse.DataLake", "Modules", "DllLibrary", moduleNamespace, moduleNamespace + ".dll");
            return path;
        }
    }
}