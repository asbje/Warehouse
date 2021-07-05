using System;

namespace Warehouse.Modules.Eloverblik.Refine.Models
{

    public class MeteringPointDetailResult
    {
        public Result[] result { get; set; }
    }

    public class Result
    {
        public MeteringPointDetail result { get; set; }
        public bool success { get; set; }
        public string errorCode { get; set; }
        public string errorText { get; set; }
        public string id { get; set; }
        public object stackTrace { get; set; }
    }

    public class MeteringPointDetail
    {
        public string meteringPointId { get; set; }
        public string parentMeteringPointId { get; set; }
        public string typeOfMP { get; set; }
        public string energyTimeSeriesMeasureUnit { get; set; }
        public string estimatedAnnualVolume { get; set; }
        public string settlementMethod { get; set; }
        public string meterNumber { get; set; }
        public string gridOperatorName { get; set; }
        public string meteringGridAreaIdentification { get; set; }
        public string netSettlementGroup { get; set; }
        public string physicalStatusOfMP { get; set; }
        public string consumerCategory { get; set; }
        public string powerLimitKW { get; set; }
        public string powerLimitA { get; set; }
        public string subTypeOfMP { get; set; }
        public string productionObligation { get; set; }
        public string mpCapacity { get; set; }
        public string mpConnectionType { get; set; }
        public string disconnectionType { get; set; }
        public string product { get; set; }
        public string consumerCVR { get; set; }
        public string dataAccessCVR { get; set; }
        public string consumerStartDate { get; set; }
        public string meterReadingOccurrence { get; set; }
        public string mpReadingCharacteristics { get; set; }
        public string meterCounterDigits { get; set; }
        public string meterCounterMultiplyFactor { get; set; }
        public string meterCounterUnit { get; set; }
        public string meterCounterType { get; set; }
        public string balanceSupplierName { get; set; }
        public DateTime? balanceSupplierStartDate { get; set; }
        public string taxReduction { get; set; }
        public string taxSettlementDate { get; set; }
        public string mpRelationType { get; set; }
        public string streetCode { get; set; }
        public string streetName { get; set; }
        public string buildingNumber { get; set; }
        public string floorId { get; set; }
        public string roomId { get; set; }
        public string postcode { get; set; }
        public string cityName { get; set; }
        public string citySubDivisionName { get; set; }
        public string municipalityCode { get; set; }
        public string locationDescription { get; set; }
        public string firstConsumerPartyName { get; set; }
        public string secondConsumerPartyName { get; set; }
        public Contactaddress[] contactAddresses { get; set; }
        //public object[] childMeteringPoints { get; set; }  //Not important. Same data comes from MeteringPoint
    }

    public class Contactaddress
    {
        public string contactName1 { get; set; }
        public string contactName2 { get; set; }
        public string addressCode { get; set; }
        public string streetName { get; set; }
        public string buildingNumber { get; set; }
        public string floorId { get; set; }
        public string roomId { get; set; }
        public string citySubDivisionName { get; set; }
        public string postcode { get; set; }
        public string cityName { get; set; }
        public string countryName { get; set; }
        public string contactPhoneNumber { get; set; }
        public string contactMobileNumber { get; set; }
        public string contactEmailAddress { get; set; }
        public object contactType { get; set; }
    }
}
