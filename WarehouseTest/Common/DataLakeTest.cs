using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WarehouseTest.Helpers;

namespace WarehouseTest.Common
{
    [TestClass]
    public class DataLakeTest : GenericTest
    {
        //Should not be run directly
        //[TestMethod]
        //public void GetAllInCurrent()
        //{
        //    var dataLake = new DataLake(Config, "DaluxFM", "current");
        //    var ingests = dataLake.GetFilesAsIngests(200);  //By only taking 200 rows, it should be possible to get the right datatypes
        //}

        //[TestMethod]
        //public void UploadToFolder()
        //{
        //    var ingest = new IngestTests().CreateIngestExample(3);
        //    ingest.SaveToDataLakeAsDecodedCsvByDate(true);
        //}
    }
}
