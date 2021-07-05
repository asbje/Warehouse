using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Warehouse.Modules.OutlookBookings.Service
{
    public class FTPService
    {
        private readonly string ftpConnectionString;
        private readonly ILogger log;

        private FTPClientHelper _client;

        public FTPClientHelper Client
        {
            get
            {
                if(_client == null)
                {
                    _client = new FTPClientHelper(ftpConnectionString);
                    try
                    {
                        _client.Connect();
                        log.LogInformation($"Connected to ftp: {_client.Host}, at path: {_client.Path}.");
                    }
                    catch (Exception)
                    {
                        log.LogError("Could not connect to FTP.");
                        throw;
                    }

                }
                return _client;
            }
        }

        public FTPService(string ftpConnectionString, ILogger log)
        {
            this.ftpConnectionString = ftpConnectionString;
            this.log = log;
        }

        public List<KeyValuePair<DateTime, Stream>> GetData()
        {
            var res = new List<KeyValuePair<DateTime, Stream>>();
            foreach (var item in Client.ListDirectory(Client.Path).Where(o => !o.IsDirectory))
            {
                log.LogInformation($"- File: {item.FullName}. Created: {item.LastWriteTimeUtc}. Bytes: {item.Length}");
                var sourceFilePath = Client.Path + "/" + item.Name;
                using var rawStream = new MemoryStream();
                Client.DownloadFile(sourceFilePath, rawStream);
                res.Add(new KeyValuePair<DateTime, Stream>(item.LastWriteTimeUtc, rawStream));

                Client.DeleteFile(sourceFilePath);
            }
            return res;
        }

        public void DeleteFolderContent()
        {
            foreach (var item in Client.ListDirectory(Client.Path).Where(o => !o.IsDirectory))
            {
                var sourceFilePath = Client.Path + "/" + item.Name;
                Client.DeleteFile(sourceFilePath);
            }
        }
    }
}