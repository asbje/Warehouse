using System;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Warehouse.DataLake.Common.CsvTools;

[assembly: InternalsVisibleTo("Warehouse.DataLakeTest")]
namespace Warehouse.DataLake.Common
{
    public class Ingest
    {
        public string Module { get; }
        public string Table { get; }
        public IConfigurationRoot Config { get; }

        public Ingest(IConfigurationRoot config, string module, string table)
        {
            Config = config;
            Module = module;
            Table = table;
        }

        public static void DeleteCurrentDirectoryInDatalake(IConfigurationRoot config, string module)
        {
            var dataLake = new DataLake(config, module, SubDirectory.current.ToString());
            dataLake.DeleteSubDirectory();
        }

        public void SaveAsRaw(Stream rawStream, string rawFileExtension, DateTime fileDate, bool onlyOverwriteIfFileIsNewer = true)
        {
            var filename = Table + '.' + rawFileExtension;
            var basePath = string.Join('/', SubDirectory.raw.ToString(), fileDate.ToString("yyyy"), fileDate.ToString("MM"), fileDate.ToString("dd"));
            var dataLake = new DataLake(Config, Module, basePath);
            var newestFileInFolder = dataLake.GetNewestFileInFolder(filename);
            if (newestFileInFolder == null || !onlyOverwriteIfFileIsNewer || onlyOverwriteIfFileIsNewer && newestFileInFolder?.LastModified.UtcDateTime < fileDate)
                dataLake.SaveStreamToDataLake(filename, rawStream);
        }

        public void SaveASDecoded(CsvSet csv, DateTime fileDate, bool onlyOverwriteIfFileIsNewer = true)
        {
            var filename = Table + ".csv";
            var basePath = string.Join('/', SubDirectory.decoded.ToString(), fileDate.ToString("yyyy"), fileDate.ToString("MM"), fileDate.ToString("dd"));
            var dataLake = new DataLake(Config, Module, basePath);
            var newestFileInFolder = dataLake.GetNewestFileInFolder(filename);
            if (newestFileInFolder == null || !onlyOverwriteIfFileIsNewer || onlyOverwriteIfFileIsNewer && newestFileInFolder?.LastModified.UtcDateTime < fileDate)
                dataLake.SaveCsvToDataLake(filename, csv);
        }

        public void SaveAsCurrent(CsvSet csv, DateTime fileDate, bool onlyOverwriteIfFileIsNewer = true)
        {
            var basePath = SubDirectory.current.ToString();
            var filename = Table + ".csv";
            var dataLake = new DataLake(Config, Module, basePath);
            var newestFileInFolder = dataLake.GetNewestFileInFolder(filename);
            if (newestFileInFolder == null || !onlyOverwriteIfFileIsNewer || onlyOverwriteIfFileIsNewer && newestFileInFolder?.LastModified.UtcDateTime < fileDate)
                dataLake.SaveCsvToDataLake(filename, csv);
        }

        public void SaveAsAccumulated(CsvSet csv)
        {
            var basePath = SubDirectory.accumulate.ToString();
            var filename = Table + ".csv";
            var dataLake = new DataLake(Config, Module, basePath);
            dataLake.SaveCsvToDataLake(filename, csv);
        }
    }

    public enum SubDirectory
    {
        accumulate,
        current,
        decoded,
        raw,
    }
}