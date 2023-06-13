// See https://aka.ms/new-console-template for more information

using System.Reflection;
using ZdravoCorp.Core.Utilities;
using ZdravoCorp.Core.Services.ScheduleServices;


Console.WriteLine("Hello, World!");
var idg = new IDGenerator();
Injector.Configure();
Console.WriteLine(IDGenerator.GetId());
