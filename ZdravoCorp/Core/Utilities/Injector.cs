using System.Linq;
using System.Reflection;
using Autofac;

namespace ZdravoCorp.Core.Utilities;

public static class Injector
{
    public static IContainer Container { get; set; }

    public static void Configure()
    {
        var builder = new ContainerBuilder();

        /*
        builder.RegisterAssemblyTypes(Assembly.Load(nameof(ZdravoCorp)))
            .Where(t => t.Namespace.Contains("Repositories"))
            .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == "I" + t.Name));*/
        
        Container = builder.Build();
    }
}