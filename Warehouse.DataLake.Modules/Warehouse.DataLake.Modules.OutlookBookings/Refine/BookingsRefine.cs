using System;
using System.IO;
using Warehouse.DataLake.Common.CsvTools;

namespace Warehouse.DataLake.Modules.OutlookBookings.Refine
{
    public class BookingsRefine : BaseRefine
    {
        public CsvSet WashedCsv { get; }

        private readonly Stream csvStream;

        public BookingsRefine(string moduleName, Stream csvStream) : base(moduleName, "bookings")
        {
            this.csvStream = csvStream;
            Refine();
        }

        public override void Refine()
        {
            CsvSet = new CsvReader(csvStream).CsvSet;
            RemoveCol("Placering");
            RemoveCol("Beskrivelse");
            CsvSet.RenameHeader("Start", "From");
            CsvSet.RenameHeader("Slut", "To");
            CsvSet.RenameHeader("Navn", "Name");
            CsvSet.RenameHeader("Mailadresse", "Mail");
            CsvSet.RenameHeader("Deltagere", "Participants");
            CsvSet.RenameHeader("Kapacitet", "Capacity");
            ConvertDate("From");
            ConvertDate("To");
            AddCapacityToRoom("Mail", "kantinen-moedecenter@hillerod.dk", "Capacity", 100);
            AddFactorCol("Factor", "Participants", "Capacity");
        }

        private void AddCapacityToRoom(string headerNameLookup, string lookup, string writeIntoHeader, int capacity)
        {
            CsvSet.TryGetRecordCol(writeIntoHeader, out int col);
            var rows = CsvSet.GetRecordRows(headerNameLookup, lookup);
            foreach (var row in rows)
                CsvSet.UpdateRecord(col, row.Key, capacity);
        }

        private void AddFactorCol(string headerName, string headerNameNumerator, string headerNameDenominator) //Numerator=tæller, denominator=nævner
        {
            var c = CsvSet.ColLimit.Max + 1;
            CsvSet.AddHeader(c, headerName);

            var numerators = CsvSet.GetRecordCol(headerNameNumerator).Records;
            var denominators = CsvSet.GetRecordCol(headerNameDenominator).Records;
            foreach (var denimonatorRecord in denominators)
            {
                if (double.TryParse(denimonatorRecord.Value.ToString(), out double denominator) && double.TryParse(numerators[denimonatorRecord.Key].ToString(), out double numerator))
                {
                    var val = numerator < denominator ? Math.Round(numerator / denominator, 3) : 1;
                    CsvSet.AddRecord(c, denimonatorRecord.Key, val);
                }
            }
        }

        private void RemoveCol(string headerName)
        {
            if (CsvSet.TryGetRecordCol(headerName, out int col))
                CsvSet.RemoveColumn(col);
        }

        private void ConvertDate(string headerName)
        {
            var cols =  CsvSet.GetRecordCol(headerName);
            foreach (var record in cols.Records)
                CsvSet.Records[(cols.Col, record.Key)] = DateTime.Parse(record.Value.ToString().Replace('.', ':')).ToUniversalTime().ToString("u");
        }
    }
}
