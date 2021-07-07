using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Warehouse.DataLake.CsvTools;
using Warehouse.Modules.Eloverblik.Refine.Models;

namespace Warehouse.Modules.Eloverblik.Refine
{
    public class MeteringPointsDetailsRefine : RefineBase
    {
        public IEnumerable<MeteringPointDetail> Data { get; set; }
        private readonly MeteringPointsRefine meteringPointsRefine;

        public MeteringPointsDetailsRefine(IExporter exporter, HttpResponseMessage response, MeteringPointsRefine meteringPointsRefine):base(exporter, "meteringPointsDetails")
        {
            if (response.IsSuccessStatusCode)
                Data = JsonConvert.DeserializeObject<MeteringPointDetailResult>(response.Content.ReadAsStringAsync().Result)?.result.Select(o => o.result);
            else
                AddError($"Could not make request. Statuscode: {response.StatusCode}. Message: {response.ReasonPhrase}.");

            this.meteringPointsRefine = meteringPointsRefine;
            Refine();
        }

        public override void Refine()
        {
            if (Data == null || HasErrors || meteringPointsRefine.Data == null || meteringPointsRefine.HasErrors)
                return;

            CsvSet = new CsvSet(
                "meteringPointId, typeOfMP, balanceSupplierName, streetName, buildingNumber, floorId, roomId, postcode, cityName, locationDescription, meterReadingOccurrence, firstConsumerPartyName, secondConsumerPartyName, consumerCVR, dataAccessCVR, meterNumber, consumerStartDate, parentPointId," +
                "energyTimeSeriesMeasureUnit, estimatedAnnualVolume, gridOperator, balanceSupplierName, balanceSupplierStartDate, physicalStatusOfMP, subTypeOfMP, meterCounterMultiplyFactor, meterCounterUnit");
            var r = 0;
            foreach (var point in meteringPointsRefine.Data)
            {
                var detaile = Data.Where(o => o.meteringPointId.Equals(point.meteringPointId));
                var detaiwl = Data.Where(o => o.meteringPointId == point.meteringPointId);
                var detail = Data.Single(o => o.meteringPointId.Equals(point.meteringPointId));  //If error then it could be because meteringPoint and Details, are collected at different times.
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
                //Details:
                CsvSet.AddRecord(18, r, detail.energyTimeSeriesMeasureUnit);
                CsvSet.AddRecord(19, r, detail.estimatedAnnualVolume);
                CsvSet.AddRecord(20, r, detail.gridOperatorName);
                CsvSet.AddRecord(21, r, detail.balanceSupplierName);
                CsvSet.AddRecord(22, r, detail.balanceSupplierStartDate?.ToString("u"));
                CsvSet.AddRecord(23, r, detail.physicalStatusOfMP);
                CsvSet.AddRecord(24, r, detail.subTypeOfMP);
                CsvSet.AddRecord(25, r, detail.meterCounterMultiplyFactor);
                CsvSet.AddRecord(26, r, detail.meterCounterUnit);

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
