using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ZdravoCorp.Core.TimeSlots
{
    public class TimeSlot                 //will be some functions for time
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

    }
    
}
