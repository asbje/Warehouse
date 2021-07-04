using Azure.Storage;
using Azure.Storage.Files.DataLake;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Warehouse.DataLake.Common.CsvTools;

[assembly: InternalsVisibleTo("Warehouse.DataLake.Common.Tests")]
namespace Warehouse.DataLake.Common
{
    class DataLake
    {
        public readonly Uri ServiceUri;
        public readonly IConfigurationRoot config;
        public readonly string module;
        public readonly string StorageAccountName;
        public readonly string StorageAccountKey;
        public readonly string BasePath;
        public readonly string BaseDirectory;
        public readonly string SubDirectory;
        private DataLakeServiceClient _dataLakeServiceClient;

        public DataLakeServiceClient DataLakeServiceClient
        {
            get
            {
                if (_dataLakeServiceClient == null)
                {
                    var sharedKeyCredential = new StorageSharedKeyCredential(StorageAccountName, StorageAccountKey);
                    _dataLakeServiceClient = new DataLakeServiceClient(ServiceUri, sharedKeyCredential);
                }
                return _dataLakeServiceClient;
            }
        }

        /// <param name="module">såsom "DaluxFM"</param>
        /// <param name="subDirectory">Såsom "current"</param>
        public DataLake(IConfigurationRoot config, string module, string subDirectory)
        {
            this.config = config;
            this.module = module;
            ServiceUri = new Uri(config["DataLakeServiceUrl"]);
            if (ServiceUri == null) throw new Exception("The value for DataLakeServiceUrl, has to be set in config.");
            StorageAccountName = config["DataLakeAccountName"];
            if (StorageAccountName == null) throw new Exception("The value for DataLakeAccountName, has to be set in config.");
            StorageAccountKey = config["DataLakeAccountKey"];
            if (StorageAccountKey == null) throw new Exception("The value for DataLakeAccountKey, has to be set in config.");
            BasePath = config["DataLakeBasePath"];
            if (BasePath == null) throw new Exception("The value for DataLakeBasePath, has to be set in config.");

            BaseDirectory = module.ToString().ToLower();
            SubDirectory = subDirectory.ToLower();
        }

        public IEnumerable<CsvSet> GetDecodedFilesFromDataLake(string tableName, DateTime from, DateTime to)
        {
            var res = new list<ingest>();
            var filesystem = datalakeserviceclient.getfilesystemclient(basepath);
            if (!filesystem.exists())
                yield break;

            var directory = filesystem.getdirectoryclient(string.join('/', basedirectory, subdirectory));
            if (!directory.exists())
                yield break;

            foreach (var item in filesystem.getpaths(directory.path))
                if (item.isdirectory == false && path.getextension(item.name).equals(".csv", stringcomparison.invariantcultureignorecase))
                {
                    var fileclient = filesystem.getfileclient(item.name);
                    using var stream = fileclient.openread();
                    var table = path.getfilenamewithoutextension(item.name);
                    var ingest = new ingest(config, module, table, item.lastmodified.utcdatetime);
                    ingest.ingestcsv(stream, take);
                    yield return ingest;
                    res.add(ingest);
                }
        }

        /// <param name="fileName">Såsom "Lots.csv"</param>
        internal void SaveCsvToDataLake(string fileName, CsvSet csv)
        {
            if (csv.Records.Count > 0)
            {
                var stream = csv.Write();
                stream.Position = 0;
                var fileClient = GetFileClient(fileName);
                fileClient.Upload(stream, true);
            }
        }

        /// <param name="fileName">Såsom "Lots.csv"</param>
        internal void SaveStringToDataLake(string fileName, string data)  //Inspiration: https://docs.microsoft.com/en-us/azure/storage/blobs/data-lake-storage-directory-file-acl-dotnet
        {
            var fileClient = GetFileClient(fileName);

            using var stream = new MemoryStream(Encoding.Default.GetBytes(data));
            fileClient.Upload(stream, true);
        }

        /// <param name="fileName">Såsom "Lots.csv"</param>
        internal void SaveStreamToDataLake(string fileName, Stream stream)  //Inspiration: https://docs.microsoft.com/en-us/azure/storage/blobs/data-lake-storage-directory-file-acl-dotnet
        {
            if (stream == null || stream.Length == 0)
                return;

            var fileClient = GetFileClient(fileName);
            stream.Position = 0;
            fileClient.Upload(stream, true);
        }

        internal void DeleteSubDirectory()
        {
            var fileSystem = DataLakeServiceClient.GetFileSystemClient(BasePath);
            if (fileSystem.Exists())
            {
                var directory = fileSystem.GetDirectoryClient(string.Join('/', BaseDirectory, SubDirectory));
                if (directory.Exists())
                    directory.Delete();
            }
        }

        /// <param name="filename">If set, it wil only return files by that name.</param>
        internal Azure.Storage.Files.DataLake.Models.PathItem GetNewestFileInFolder(string filename)
        {
            var fileSystem = DataLakeServiceClient.GetFileSystemClient(BasePath);
            if (!fileSystem.Exists())
                return null;

            var directory = fileSystem.GetDirectoryClient(string.Join('/', BaseDirectory, SubDirectory));
            if (!directory.Exists())
                return null;

            var list = fileSystem.GetPaths(directory.Path).OrderByDescending(o => o.LastModified);

            return string.IsNullOrEmpty(filename) ? list.FirstOrDefault() : list.Where(o => o.Name.EndsWith(filename, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        /// <param name="filename">Såsom "Lots.csv"</param>
        private DataLakeFileClient GetFileClient(string filename)
        {
            var fileSystem = DataLakeServiceClient.GetFileSystemClient(BasePath);
            if (!fileSystem.Exists())
                fileSystem.Create();

            var directory = fileSystem.GetDirectoryClient(string.Join('/', BaseDirectory, SubDirectory));
            if (!directory.Exists())
                directory.Create();

            return directory.GetFileClient(filename);
        }
    }
}
