using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Warehouse.Common;
using WarehouseTest.Helpers;

namespace WarehouseTest.Common
{
    [TestClass]
    public class DataLakeTest : GenericTest
    {
        //Should not be run directly
        [TestMethod]
        public void GetAllInCurrent()
        {
            var dataLake = new DataLake(Config, "DaluxFM", "current");
            var ingests = dataLake.GetDecodedFilesFromDataLake("estates", DateTime.MinValue, DateTime.MaxValue).ToList();
        }

        //[TestMethod]
        //public void UploadToFolder()
        //{
        //    var ingest = new IngestTests().CreateIngestExample(3);
        //    ingest.SaveToDataLakeAsDecodedCsvByDate(true);
        //}
    }
}
