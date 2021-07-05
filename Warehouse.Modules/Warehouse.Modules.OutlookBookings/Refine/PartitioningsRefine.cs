using System;
using System.Linq;
using Warehouse.Common.CsvTools;
using Warehouse.Modules.OutlookBookings.Refine.PartitionBookings;

namespace Warehouse.Modules.OutlookBookings.Refine
{
    public class PartitioningsRefine : BaseRefine
    {
        private readonly BookingsRefine bookingsRefine;
        private readonly LocationsRefine locationsRefine;

        public PartitioningsRefine(string moduleName, BookingsRefine bookingsRefine, LocationsRefine locationsRefine) : base(moduleName, "bookings")
        {
            this.bookingsRefine = bookingsRefine;
            this.locationsRefine = locationsRefine;
            Refine();
        }

        /// <summary>
        /// Partitions bookingdata into slices of 2 hours: 8-10, 10-12, 12-14, 14-16, 16-18
        /// Goes from now and two year back
        /// Doesn't include weekends
        /// </summary>
        public override void Refine()
        {
            var partitioning = new Partitioning(FilteredCsv(), "From", "To", "Mail", "Factor");
            ///TODO: Ændr denne så :
            ///- Den finder startDato og screener helt tilbage fra start (OK for det kommer kun i chunks af 1 md)
            ///- I AppSetting kan man angive timespan og om weekender skal med og hvor mange minutters interval der skal være
            var partitons = partitioning.GetPartitionsInTimeslices(DateTime.Now.AddYears(-2), DateTime.Now, new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0), 120, false, true);
            CsvSet = partitioning.TimeslotsToCsvSet(partitons);
        }

        /// <summary>
        /// Filters out all irelevant calenders like vehicles and assets, so it's only Room:
        /// </summary>
        internal CsvSet FilteredCsv()
        {
            var mails = locationsRefine.CsvSet.GetRecordCol("Type", "Mail", "Room").Select(o => o.Value).ToArray();
            return bookingsRefine.CsvSet.Filter("Mail", mails);
        }
    }
}