using System;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoCorp.Core.Utilities;

public class TimeSlot : IEquatable<TimeSlot> //will be some functions for time
{
    public TimeSlot(DateTime start, DateTime end)
    {
        this.Start = start;
        this.End = end;
    }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public bool Equals(TimeSlot? other)
    {
        if (other == null) return false;
        return Start.Year == other.Start.Year && Start.Month == other.Start.Month &&
               Start.Day == other.Start.Day && Start.Hour == other.Start.Hour &&
               Start.Minute == other.Start.Minute;
    }

    public bool Overlap(TimeSlot time)
    {
        return Start < time.End && time.Start < End;
    }

    public int GetTimeBeforeStart(DateTime time)
    {
        return (Start - time).Days * 24 + (Start - time).Hours;
    }

    public bool IsInsideSingleSlot(TimeSlot time)
    {
        return Start >= time.Start && End <= time.End;
    }

    public bool IsInsideListOfSlots(IEnumerable<TimeSlot> slots)
    {
        return slots.Any(IsInsideSingleSlot);
    }

    public List<TimeSlot> GiveSameTimeUntileSomeDay(DateTime lastDate)
    {
        var allSlots = new List<TimeSlot>();
        var current = this;
        while (current.Start < lastDate)
        {
            allSlots.Add(current);
            current = new TimeSlot(current.Start.AddDays(1), current.End.AddDays(1));
        }

        return allSlots;
    }

    public TimeSlot ExtendButStayOnSameDay(TimeSpan amount)
    {
        var adjustedStart = Start.Add(-1 * amount);
        var adjustedEnd = End.Add(amount);

        if (adjustedStart.Date < Start.Date) adjustedStart = Start;

        if (adjustedEnd.Date > End.Date) adjustedEnd = End.Date.AddDays(1).AddSeconds(-1);

        return new TimeSlot(adjustedStart, adjustedEnd);
    }

    public override bool Equals(object? o)
    {
        if (o == null) return false;

        return o is TimeSlot && Equals(o);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hash = 17;
            hash = hash * 23 + Start.Year.GetHashCode();
            hash = hash * 23 + Start.Month.GetHashCode();
            hash = hash * 23 + Start.Day.GetHashCode();
            hash = hash * 23 + Start.Hour.GetHashCode();
            hash = hash * 23 + Start.Minute.GetHashCode();
            return hash;
        }
    }

    public bool IsNow()
    {
        var nowTime = DateTime.Now;
        var interval = Start - nowTime;
        var notPassed = !(Start.CompareTo(DateTime.Now) < 0);
        return interval.TotalMinutes < 15 && notPassed;
    }

    public static DateTime GiveFirstDevisibleBy15(DateTime time) //this should be somewhere else
    {
        var minutes = time.Minute;
        var minutesToAdd = minutes switch
        {
            < 15 => 15 - minutes,
            < 30 => 30 - minutes,
            < 45 => 45 - minutes,
            < 60 => 60 - minutes,
            _ => 0
        };
        return time.AddMinutes(minutesToAdd);
    }

    public bool IsBefore()
    {
        return Start.CompareTo(Start) < 0;
    }
}