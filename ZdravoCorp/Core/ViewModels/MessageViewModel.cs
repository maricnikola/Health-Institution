using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.Core.ViewModels;

public class MessageViewModel : ViewModelBase
{
    public string Sender { get; set; }
    public string Content { get; set; }
}