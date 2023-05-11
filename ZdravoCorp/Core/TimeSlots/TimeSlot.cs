using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ZdravoCorp.Core.TimeSlots
{

    public class TimeSlot : IEquatable<TimeSlot>              //will be some functions for time
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }

        public TimeSlot(DateTime start, DateTime end)
        {
            this.start = start;
            this.end = end;
        }

        public bool Overlap(TimeSlot time)
        {
            return start >= time.end || end <= time.start;    
        }

        public int GetTimeBeforeStart(DateTime time)
        {
            return (start - time).Days*24 + (start-time).Hours;
        }

        public bool IsInsideSingleSlot(TimeSlot time)
        {
            return this.start >= time.start && this.end <= time.end;
        }

        public bool IsInsideListOfSlots(IEnumerable<TimeSlot> slots)
        {
            return slots.Any(IsInsideSingleSlot);
        }

        public List<TimeSlot> GiveSameTimeUntileSomeDay(DateTime lastDate)
        {
            List<TimeSlot> allSlots  = new List<TimeSlot>();
            TimeSlot current = this;
            while (current.start < lastDate)
            {
                allSlots.Add(current);
                current = new TimeSlot(current.start.AddDays(1), current.end.AddDays(1));
            }
            return allSlots;
        }

        public TimeSlot ExtendButStayOnSameDay(TimeSpan amount)
        {
            DateTime adjustedStart = start.Add((-1)*amount);
            DateTime adjustedEnd = end.Add(amount);

            if (adjustedStart.Date < start.Date)
            {
                adjustedStart = start;
            }

            if (adjustedEnd.Date > end.Date)
            {
                adjustedEnd = end.Date.AddDays(1).AddSeconds(-1);
            }

            return new TimeSlot(adjustedStart, adjustedEnd);
        }

        public bool Equals(TimeSlot? other)
        {
            if (other == null)
            {
                return false;
            }
            return (this.start.Year == other.start.Year) && (this.start.Month == other.start.Month) &&
                   (this.start.Day == other.start.Day) && (this.start.Hour == other.start.Hour) &&
                   (this.start.Minute == other.start.Minute);
        }

        public override bool Equals(object? o)
        {
            if (o == null)
            {
                return false;
            }

            return o is TimeSlot && Equals(o);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + start.Year.GetHashCode();
                hash = hash * 23 + start.Month.GetHashCode();
                hash = hash * 23 + start.Day.GetHashCode();
                hash = hash * 23 + start.Hour.GetHashCode();
                hash = hash * 23 + start.Minute.GetHashCode();
                return hash;
            }
        }

        public bool IsNow()
        {
            DateTime nowTime = DateTime.Now;
            TimeSpan interval = start - nowTime;
            return interval.TotalMinutes < 5;
        }
    }
    
}
