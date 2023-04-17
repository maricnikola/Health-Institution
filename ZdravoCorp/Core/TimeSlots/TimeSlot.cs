using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public bool overlap(TimeSlot time)
        {
            return start > time.end || end < time.start;    
        }

        public int GetTimeBeforeStart(DateTime time)
        {
            return (start - time).Days*24 + (start-time).Hours;
        }
    }
}
