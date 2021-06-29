using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Warehouse.DataLake.Common;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLakeTest.Helpers;

namespace Warehouse.DataLakeTest.Common
{
    [TestClass]
    public class IngestTest : GenericTest
    {
        //Should not be run directly
        //[TestMethod]
        //public void ConvertLocalFileSaveToDataLake()
        //{
        //    //var directory = "SmallFiles";
        //    var directory = "LargeFiles";
        //    var ingests = new List<Ingest>();

        //    //ConvertLocalFileSaveToDataLake(Path.Combine(BasePath, "Files", directory, "DaluxFM_Lots.xlsx"), ref ingests);
        //    //ConvertLocalFileSaveToDataLake(Path.Combine(BasePath, "Files", directory, "DaluxFM_Assets.xlsx"), ref ingests);
        //    //ConvertLocalFileSaveToDataLake(Path.Combine(BasePath, "Files", directory, "DaluxFM_Buildings.xlsx"), ref ingests);
        //    //ConvertLocalFileSaveToDataLake(Path.Combine(BasePath, "Files", directory, "DaluxFM_Estates.xlsx"), ref ingests);
        //    //ConvertLocalFileSaveToDataLake(Path.Combine(BasePath, "Files", directory, "DaluxFM_Invoices.xlsx"), ref ingests);
        //    //ConvertLocalFileSaveToDataLake(Path.Combine(BasePath, "Files", directory, "DaluxFM_Rooms.xlsx"), ref ingests);
        //    //ConvertLocalFileSaveToDataLake(Path.Combine(BasePath, "Files", directory, "DaluxFM_Tasks.xlsx"), ref ingests);

        //   new CommonDataModel(Config, "DaluxFM", ingests, true);
        //}

        //private void ConvertLocalFileSaveToDataLake(string filePath, ref List<Ingest> ingests)
        //{
        //    var filename = Path.GetFileName(filePath);
        //    var filedate = File.GetLastWriteTimeUtc(filePath);
        //    var ingest = new Ingest(Config, "DaluxFM", Path.GetFileNameWithoutExtension(filename), filename, filedate);
        //    ingests.Add(ingest);

        //    using var excelStream = new FileStream(filePath, FileMode.Open);
        //    ingest.IngestExcel(excelStream, 10);

        //    var csvStream = ingest.Csv.Write();
        //    var res = new CsvReader(csvStream, 4).CsvSet;

        //    //var dataLake = new DataLake(Config, Database.DaluxFM, "current");
        //    //var ingests2 = dataLake.GetFilesAsIngests(200);  //By only taking 200 rows, it should be possible to get the right datatypes

        //    //var ingestLog = ImportLog.CreateLog(Config, Database.DaluxFM, "ImportLog", ingests2);

        //    ingest.Csv.Write(Path.Combine(BasePath, "Files", "Output", "test.csv"));

        //    //ingest.SaveToDataLakeAsRawByDate(excelStream, true);
        //    //ingest.SaveToDataLakeAsDecodedCsvByDate(true);
        //    //ingest.SaveToDataLakeAsDecodedCsvInCurrentFolder(true);
        //}

        [TestMethod]
        public void DetectDataColumnValueType()
        {
            var ingest = new Ingest(Config, "DaluxFM", "Test");

            var colTypes = new Dictionary<int, Type>();
            var col = 0;

            //Storage.DataLake.FileConverters.Csv.DetectColValueType("text", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(string));

            //Storage.DataLake.FileConverters.Csv.DetectColValueType("2", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(string));

            //col++;
            //Storage.DataLake.FileConverters.Csv.DetectColValueType("true", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(bool));

            //Storage.DataLake.FileConverters.Csv.DetectColValueType("2", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(string));

            //col++;
            //Storage.DataLake.FileConverters.Csv.DetectColValueType("2", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(long));

            //Storage.DataLake.FileConverters.Csv.DetectColValueType("true", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(string));

            //col++;
            //Storage.DataLake.FileConverters.Csv.DetectColValueType("2", col, colTypes);
            //Storage.DataLake.FileConverters.Csv.DetectColValueType("2.0000000000001", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(decimal));

            //Storage.DataLake.FileConverters.Csv.DetectColValueType("true", col, colTypes);
            //Assert.IsTrue(colTypes[col] == typeof(string));
        }

        //public Ingest CreateIngestExample(int rows)
        //{
        //    var ingest = new Ingest(Config, "DaluxFM", "ImportLog" + new Random().Next());

        //    var csv = new CsvSet();
        //    csv.AddHeader(1, "Container");
        //    csv.AddHeader(2, "Uploaded");
        //    csv.AddHeader(3, "Headers");

        //    if (rows < 0)
        //        return ingest;

        //    for (int r = 0; r < rows; r++)
        //    {
        //        csv.AddRecord(1, r, "Smølf");
        //        csv.AddRecord(2, r, DateTime.UtcNow.ToString());
        //        csv.AddRecord(3, r, "16");
        //        ingest.IngeststCsvSet(csv);
        //    }
        //    return ingest;
        //}

        //public List<Ingest> CreateIngestsExample(int ingestCounts)
        //{
        //    var res = new List<Ingest>();
        //    if (ingestCounts > 0)
        //    {
        //        ingestCounts--;
        //        var ingestTest = new IngestTest();

        //        for (int i = 0; i <= ingestCounts; i++)
        //            res.Add(ingestTest.CreateIngestExample(3));
        //    }
        //    return res;
        //}
    }
}
