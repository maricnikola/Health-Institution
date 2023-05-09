using System;
using ZdravoCorp.Core.Models.Rooms;

namespace ZdravoCorp.Core.Models.Transfers;

public class Transfer
{
    public Room From { get; }
    public Room To { get; }
    public DateTime When { get; set; }
    public int Quantity { get; set; }

    public Transfer()
    {
        
    }
    
}