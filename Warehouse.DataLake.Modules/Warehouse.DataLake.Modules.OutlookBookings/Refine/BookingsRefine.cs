using System;
using System.IO;
using System.Linq;
using Warehouse.DataLake.Common.CsvTools;
using Warehouse.DataLake.Modules.OutlookBookings.Refine.PartitionBookings;

namespace Warehouse.DataLake.Modules.OutlookBookings.Refine
{
    public class BookingsRefine : BaseRefine
    {
        public CsvSet WashedCsv { get; }
        private readonly LocationsRefine locationsRefine;
        private readonly Partitioning partitioning;


        public BookingsRefine(string moduleName, LocationsRefine locationsRefine) : base(moduleName, "bookings")
        {
            this.locationsRefine = locationsRefine;

            Refine();
        }

        //public BookingsRefine(Stream csvStream, int? take = null)
        //{
        //    WashedCsv = GetRawDataWashed(csvStream, take);
        //    partitioning = new Partitioning(FilteredCsv(), "From", "To", "Mail", "Factor");
        //}


        ///// <summary>
        ///// Filters out all irelevant calenders like vehicles and assets, so it's only Room:
        ///// </summary>
        //internal CsvSet FilteredCsv()
        //{
        //    var mails = GetLocations().GetRecordCol("Type", "Mail", "Room").Select(o => o.Value).ToArray();
        //    return WashedCsv.Filter("Mail", mails);
        //}

        /// <summary>
        /// Partitions bookingdata into slices of 3 hours: 8-10, 10-12, 12-14, 14-16, 16-18
        /// Goes from now and one year back
        /// Doesn't include weekends
        /// </summary>
        public CsvSet GetPartitionTwoYearsBack()
        {
            var partitons = partitioning.GetPartitionsInTimeslices(DateTime.Now.AddYears(-2), DateTime.Now, new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0), 120, false, true);
            return partitioning.TimeslotsToCsvSet(partitons);
        }

        /// <summary>
        /// Partitions bookingdata into slices of 3 hours: 8-11, 11-14, 14-17, 17-20
        /// Goes from now and one year back
        /// Doesn't include weekends
        /// </summary>
        public CsvSet GetPartitionOneYearBack()
        {
            var partitons = partitioning.GetPartitionsInTimeslices(DateTime.Now.AddYears(-1), DateTime.Now, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0), 180, false, true);
            return partitioning.TimeslotsToCsvSet(partitons);
        }

        /// <summary>
        /// Partitions bookingdata into slices of 1 hours: 8-9, 9-10, 10-11, 12-13, 13-14, 14-15, 15-16, 16-17, 17-18, 18-19, 19-20
        /// Goes from now and one month back
        /// Include weekends
        /// </summary>
        public CsvSet GetPartitionOneMonthBack()
        {
            var partitons = partitioning.GetPartitionsInTimeslices(DateTime.Now.AddMonths(-1), DateTime.Now, new TimeSpan(8, 0, 0), new TimeSpan(20, 0, 0), 60, true, true);
            return partitioning.TimeslotsToCsvSet(partitons);
        }

       
        private CsvSet GetRawDataWashed(Stream csvStream, int? take)
        {
            var csv = new CsvReader(csvStream, take).CsvSet;
            RemoveCol(ref csv, "Placering");
            RemoveCol(ref csv, "Beskrivelse");
            csv.RenameHeader("Start", "From");
            csv.RenameHeader("Slut", "To");
            csv.RenameHeader("Navn", "Name");
            csv.RenameHeader("Mailadresse", "Mail");
            csv.RenameHeader("Deltagere", "Participants");
            csv.RenameHeader("Kapacitet", "Capacity");
            ConvertDate(csv, "From");
            ConvertDate(csv, "To");
            AddCapacityToRoom(ref csv, "Mail", "kantinen-moedecenter@hillerod.dk", "Capacity", 100);
            AddFactorCol(ref csv, "Factor", "Participants", "Capacity");
            return csv;
        }

        private void AddCapacityToRoom(ref CsvSet csv, string headerNameLookup, string lookup, string writeIntoHeader, int capacity)
        {
            csv.TryGetRecordCol(writeIntoHeader, out int col);
            var rows = csv.GetRecordRows(headerNameLookup, lookup);
            foreach (var row in rows)
                csv.UpdateRecord(col, row.Key, capacity);
        }

        private void AddFactorCol(ref CsvSet csv, string headerName, string headerNameNumerator, string headerNameDenominator) //Numerator=tæller, denominator=nævner
        {
            var c = csv.ColLimit.Max + 1;
            csv.AddHeader(c, headerName);

            var numerators = csv.GetRecordCol(headerNameNumerator).Records;
            var denominators = csv.GetRecordCol(headerNameDenominator).Records;
            foreach (var denimonatorRecord in denominators)
            {
                if (double.TryParse(denimonatorRecord.Value.ToString(), out double denominator) && double.TryParse(numerators[denimonatorRecord.Key].ToString(), out double numerator))
                {
                    var val = numerator < denominator ? Math.Round(numerator / denominator, 3) : 1;
                    csv.AddRecord(c, denimonatorRecord.Key, val);
                }
            }
        }

        private void RemoveCol(ref CsvSet csv, string headerName)
        {
            if (csv.TryGetRecordCol(headerName, out int col))
                csv.RemoveColumn(col);
        }

        private void ConvertDate(CsvSet csv, string headerName)
        {
            var cols = csv.GetRecordCol(headerName);
            foreach (var record in cols.Records)
                csv.Records[(cols.Col, record.Key)] = DateTime.Parse(record.Value.ToString().Replace('.', ':')).ToUniversalTime().ToString("u");
        }
    }
}
