using Newtonsoft.Json;
using System;

namespace Warehouse.DataLake.Modules.Eloverblik.Refine.Models
{

    public class MeteringReadingResult
    {
        public MeteringReading[] result { get; set; }
    }

    public class MeteringReading
    {
        public Myenergydata_Marketdocument MyEnergyData_MarketDocument { get; set; }
        public bool success { get; set; }
        public string errorCode { get; set; }
        public string errorText { get; set; }
        public string id { get; set; }
        public object stackTrace { get; set; }
    }

    public class Myenergydata_Marketdocument
    {
        public string mRID { get; set; }
        public DateTime createdDateTime { get; set; }
        public string sender_MarketParticipantname { get; set; }
        public Sender_MarketparticipantMrid sender_MarketParticipantmRID { get; set; }
        [JsonProperty("period.timeInterval")]
        public PeriodTimeinterval periodtimeInterval { get; set; }
        public Timesery[] TimeSeries { get; set; }
    }

    public class Sender_MarketparticipantMrid
    {
        public object codingScheme { get; set; }
        public object name { get; set; }
    }

    public class PeriodTimeinterval
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class Timesery
    {
        public string mRID { get; set; }

        public string businessType { get; set; }

        //private string _businessType;
        //public string businessType
        //{
        //    get { return businessTypeDictionary[_businessType]; }
        //    set { _businessType = value; }
        //}

        //private static readonly Dictionary<string, string> businessTypeDictionary = new Dictionary<string, string>(){
        //    {"A01","Production"},
        //    {"A04","Consumption"},
        //    {"A64","Consumption (profiled)"},
        //};

        public string curveType { get; set; }
        [JsonProperty("measurement_Unit.name")]
        public string measurement_Unitname { get; set; }
        public Marketevaluationpoint MarketEvaluationPoint { get; set; }
        public Period[] Period { get; set; }
    }

    public class Marketevaluationpoint
    {
        public Mrid mRID { get; set; }
    }

    public class Mrid
    {
        public string codingScheme { get; set; }
        public string name { get; set; }
    }

    public class Period
    {
        public string resolution { get; set; }
        public Timeinterval timeInterval { get; set; }
        public Point[] Point { get; set; }
    }

    public class Timeinterval
    {

        public DateTime start { get; set; }
        public DateTime end { get; set; }
    }

    public class Point
    {
        public string position { get; set; }
        [JsonProperty("out_Quantity.quantity")]
        public string out_Quantityquantity { get; set; }
        [JsonProperty("out_Quantity.quality")]
        public string out_Quantityquality { get; set; }
    }
}
