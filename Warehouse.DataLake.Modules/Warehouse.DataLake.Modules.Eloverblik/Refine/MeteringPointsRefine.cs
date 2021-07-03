using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.Modules.Eloverblik.Refine.Models;

namespace Warehouse.DataLake.Modules.Eloverblik.Refine
{
    public class MeteringPointsRefine: BaseRefine
    {
        public IEnumerable<MeteringPoint> Data { get; set; }

        public MeteringPointsRefine(string moduleName, HttpResponseMessage response):base(moduleName, "meteringPoints")
        {
            if (response.IsSuccessStatusCode)
                Data = JsonConvert.DeserializeObject<MeteringPointResult>(response.Content.ReadAsStringAsync().Result)?.result;
            else
                AddError($"Could not make request. Statuscode: {response.StatusCode}. Message: {response.ReasonPhrase}.");

            Refine();
        }

        public string[] GetMeteringPointIds()
        {
            return Data.Select(o => o.meteringPointId).ToArray();
        }

        public override void Refine()
        {
            if (Data == null || HasErrors)
                return;

            CsvSet = new CsvSet("meteringPointId, typeOfMP, balanceSupplierName, streetName, buildingNumber, floorId, roomId, postcode, cityName, locationDescription, meterReadingOccurrence, firstConsumerPartyName, secondConsumerPartyName, consumerCVR, dataAccessCVR, meterNumber, consumerStartDate, parentPointId");
            var r = 0;
            foreach (var point in Data)
            {
                CsvSet.AddRecord(0, r, point.meteringPointId);
                CsvSet.AddRecord(1, r, point.typeOfMP);
                CsvSet.AddRecord(2, r, point.balanceSupplierName);
                CsvSet.AddRecord(3, r, point.streetName);
                CsvSet.AddRecord(4, r, point.buildingNumber);
                CsvSet.AddRecord(5, r, point.floorId);
                CsvSet.AddRecord(6, r, point.roomId);
                CsvSet.AddRecord(7, r, point.postcode);
                CsvSet.AddRecord(8, r, point.cityName);
                CsvSet.AddRecord(9, r, point.locationDescription);
                CsvSet.AddRecord(10, r, point.meterReadingOccurrence);
                CsvSet.AddRecord(11, r, point.firstConsumerPartyName);
                CsvSet.AddRecord(12, r, point.secondConsumerPartyName);
                CsvSet.AddRecord(13, r, point.consumerCVR);
                CsvSet.AddRecord(14, r, point.dataAccessCVR);
                CsvSet.AddRecord(15, r, point.meterNumber);
                CsvSet.AddRecord(16, r, point.consumerStartDate.ToString("u"));
                r++;

                foreach (var childPoint in point.childMeteringPoints)
                {
                    CsvSet.AddRecord(0, r, childPoint.meteringPointId);
                    CsvSet.AddRecord(1, r, childPoint.typeOfMP);
                    CsvSet.AddRecord(2, r, point.balanceSupplierName);
                    CsvSet.AddRecord(3, r, point.streetName);
                    CsvSet.AddRecord(4, r, point.buildingNumber);
                    CsvSet.AddRecord(5, r, point.floorId);
                    CsvSet.AddRecord(6, r, point.roomId);
                    CsvSet.AddRecord(7, r, point.postcode);
                    CsvSet.AddRecord(8, r, point.cityName);
                    CsvSet.AddRecord(9, r, point.locationDescription);
                    CsvSet.AddRecord(10, r, childPoint.meterReadingOccurrence);
                    CsvSet.AddRecord(11, r, point.firstConsumerPartyName);
                    CsvSet.AddRecord(12, r, point.secondConsumerPartyName);
                    CsvSet.AddRecord(15, r, childPoint.meterNumber);
                    CsvSet.AddRecord(17, r, point.meteringPointId);
                    r++;
                }
            }
        }
    }
}
