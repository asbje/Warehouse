using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.DataLake.Modules.OutlookBookings.Refine.PartitionBookings
{
    public class Timeslot
    {
        private DateTime _from;

        public DateTime From
        {
            get { return _from; }
            set
            {
                _from = value;
                _duration = To - value;
            }
        }

        private DateTime _to;

        public DateTime To
        {
            get { return _to; }
            set
            {
                _to = value;
                _duration = value - From;
            }
        }

        public double Factor { get; set; }

        private TimeSpan? _duration;

        public TimeSpan Duration
        {
            get { return (TimeSpan)(_duration ??= _duration = To - From); }
        }

        //public string GroupId { get; private set; }

        public double Minutes { get; private set; }

        public double Percent { get; private set; }

        public Timeslot(DateTime from, DateTime to, double factor)
        {
            From = from;
            To = to;
            Factor = factor;
        }

        public Timeslot(DateTime from, DateTime to, double minutes, double percent, double factor)
        {
            //GroupId = groupId;
            From = from;
            To = to;
            Minutes = minutes;
            Percent = percent;
            Factor = factor;
        }

    }
}
