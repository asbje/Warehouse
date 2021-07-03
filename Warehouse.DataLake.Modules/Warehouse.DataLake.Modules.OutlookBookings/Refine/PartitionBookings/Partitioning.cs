using System;
using System.Collections.Generic;
using System.Linq;
using Warehouse.DataLake.Common.CsvTools;

namespace Warehouse.DataLake.Modules.OutlookBookings.Refine.PartitionBookings
{
    /// <summary>
    /// Slices bookings into timeslots like 8-9, 9-10..., and gives percent for usage
    /// </summary>
    public class Partitioning
    {
        public Dictionary<object, List<Timeslot>> Partitions { get; private set; }

        public Partitioning()
        {
            Partitions = new Dictionary<object, List<Timeslot>>();
        }

        public Partitioning(CsvSet csv, string headerNameFrom, string headerNameTo, string headerNameGroupId, string headerNameFactor)
        {
            Partitions = new Dictionary<object, List<Timeslot>>();
            if (!csv.TryGetRecordCol(headerNameFrom, out int colFrom) || !csv.TryGetRecordCol(headerNameTo, out int colTo) || !csv.TryGetRecordCol(headerNameGroupId, out int colGroupId) || !csv.TryGetRecordCol(headerNameFactor, out int colFactor))
                throw new Exception("Error in reading headerName. A programmer must take care of this issue.");

            foreach (var cols in csv.GetRecordRows(false).Values)
                if (cols.TryGetValue(colFrom, out object from) && cols.TryGetValue(colTo, out object to) && cols.TryGetValue(colGroupId, out object roomId) && cols.TryGetValue(colFactor, out object factor))
                    AddPartition(roomId.ToString(), DateTime.Parse(from.ToString()), DateTime.Parse(to.ToString()), factor != null ? double.Parse(factor.ToString()) : 0);
        }


        public void AddGroupIdThatHasNoTimeslot(string groupId)
        {
            Partitions.Add(groupId, new List<Timeslot>());
        }

        public void AddPartition(object groupId, DateTime from, DateTime to, double factor)
        {
            if (Partitions.TryGetValue(groupId, out List<Timeslot> partitions))
            {
                partitions.Add(new Timeslot(from, to, factor));
            }
            else
                Partitions.Add(groupId, new List<Timeslot>() { new Timeslot(from, to, factor) });
        }

        public Dictionary<object, List<Timeslot>> GetPartitionsInTimeslices(DateTime fromDate, DateTime toDate, TimeSpan fromTime, TimeSpan toTime, short minuteSlice, bool includeWeekends, bool includeEmptyValues)
        {
            var res = new Dictionary<object, List<Timeslot>>();
            var dayAndTimePartition = EachDayAndTimePartition(fromDate, toDate, fromTime, toTime, minuteSlice, includeWeekends);
            foreach (var partition in Partitions.OrderBy(o => o.Key))
            {
                var newTimeslots = new List<Timeslot>();
                foreach (var (From, To) in dayAndTimePartition)
                {
                    var intersectingTimeslots = GetIntersectingTimeslots(partition.Value, From, To);
                    var mergedData = MergeData(intersectingTimeslots, From, To, includeEmptyValues);
                    newTimeslots.AddRange(mergedData);
                }
                res.Add(partition.Key, newTimeslots);
            }
            return res;
        }

        public CsvSet TimeslotsToCsvSet(Dictionary<object, List<Timeslot>> partitions)
        {
            var csv = new CsvSet("From,To,Minutes,Percent,Factor,GroupId");

            var r = 0;
            foreach (var partition in partitions)
                foreach (var timeSlot in partition.Value)
                {
                    csv.AddRecord(0, r, timeSlot.From.ToUniversalTime().ToString("u"));
                    csv.AddRecord(1, r, timeSlot.To.ToUniversalTime().ToString("u"));
                    csv.AddRecord(2, r, timeSlot.Minutes);
                    csv.AddRecord(3, r, timeSlot.Percent);
                    csv.AddRecord(4, r, Math.Round(timeSlot.Factor, 3));
                    csv.AddRecord(5, r, partition.Key);
                    r++;
                }

            return csv;
        }

        private IEnumerable<Timeslot> MergeData(List<Timeslot> timeslots, DateTime from, DateTime to, bool includeEmptyValues)
        {
            if (timeslots != null && timeslots.Any())
            {
                var duration = timeslots.Sum(o => o.Duration.TotalMinutes);
                var factor = timeslots.Sum(o => o.Factor);
                var totalMinutes = (to - from).TotalMinutes;
                yield return new Timeslot(from, to, Math.Round(duration, 4), Math.Round(duration / totalMinutes, 4), factor);
            }
            else if (includeEmptyValues)
                yield return new Timeslot(from, to, 0, 0, 0);
        }

        private List<Timeslot> GetIntersectingTimeslots(List<Timeslot> timeslots, DateTime from, DateTime to)
        {
            var res = new List<Timeslot>();
            var datas = timeslots.Where(o => !(o.From > to) && !(o.To < from) && o.To != null).OrderBy(o => o.From);
            foreach (var data in datas)
            {
                var startsBefore = data.From < from;
                var endsAfter = data.To > to;
                var fromRes = startsBefore ? from : data.From;
                var toRes = endsAfter ? to : data.To;

                var resLength = toRes - fromRes;
                var length = to - from;
                var effectiveTimeFactor = resLength / length;

                var resData = res.FirstOrDefault(o => !(o.From > toRes) && !(o.To < fromRes));
                if (resData != null)
                {
                    if (resData.From > fromRes)
                        resData.From = fromRes;
                    if (resData.To < toRes)
                        resData.To = toRes;
                    resData.Factor += data.Factor * effectiveTimeFactor;
                }
                else
                    res.Add(new Timeslot(fromRes, toRes, data.Factor * effectiveTimeFactor));
            }
            return res;
        }

        private IEnumerable<(DateTime From, DateTime To)> EachDayAndTimePartition(DateTime fromDate, DateTime toDate, TimeSpan fromTime, TimeSpan toTime, short minuteSlice, bool includeWeekends)
        {
            for (var day = fromDate.Date; day.Date <= toDate.Date; day = day.AddDays(1))
                if (includeWeekends || (!includeWeekends && (int)day.DayOfWeek < 6))
                    for (var fromSlice = fromTime; fromSlice < toTime; fromSlice = fromSlice.Add(new TimeSpan(0, minuteSlice, 0)))
                    {
                        var toSlice = fromSlice.Add(new TimeSpan(0, minuteSlice, 0));
                        if (toSlice > toTime)
                            toSlice = toTime;

                        yield return (day.Add(fromSlice), day.Add(toSlice));
                    }
        }
    }
}
