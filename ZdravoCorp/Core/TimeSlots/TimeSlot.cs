﻿using System;
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

        public bool overlap(TimeSlot time)
        {
            return start > time.end || end < time.start;    
        }
    }
}
