using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using Warehouse.DataLake.CsvTools;
using Warehouse.Modules.Eloverblik.Refine.Models;

namespace Warehouse.Modules.Eloverblik.Refine
{
    public class MeteringReadingsRefine : RefineBase
    {
        public IEnumerable<MeteringReading> Data { get; set; }

        public MeteringReadingsRefine(IExporter exporter, string tableName, HttpResponseMessage response) : base(exporter, tableName)
        {
            if (response.IsSuccessStatusCode)
                Data = JsonConvert.DeserializeObject<MeteringReadingResult>(response.Content.ReadAsStringAsync().Result)?.result;
            else
                AddError($"Could not make request. Statuscode: {response.StatusCode}. Message: {response.ReasonPhrase}.");

            Refine();
        }

        public override void Refine()
        {
            CsvSet = new CsvSet("meteringPointId, businessType, measurementUnitName, resolution, timeintervalStart, timeintervalEnd, posistion, quantity, quality");
            var r = 0;
            foreach (var reading in Data)
                foreach (var timeSerie in reading.MyEnergyData_MarketDocument.TimeSeries)
                    foreach (var period in timeSerie.Period)
                        foreach (var point in period.Point)
                        {
                            CsvSet.AddRecord(0, r, timeSerie.mRID);
                            CsvSet.AddRecord(1, r, timeSerie.businessType);
                            CsvSet.AddRecord(2, r, timeSerie.measurement_Unitname);
                            CsvSet.AddRecord(3, r, period.resolution);
                            CsvSet.AddRecord(4, r, period.timeInterval.start.ToString("u"));
                            CsvSet.AddRecord(5, r, period.timeInterval.end.ToString("u"));
                            CsvSet.AddRecord(6, r, point.position);
                            CsvSet.AddRecord(7, r, point.out_Quantityquantity);
                            CsvSet.AddRecord(8, r, point.out_Quantityquality);
                            r++;
                        }
        }
    }
}