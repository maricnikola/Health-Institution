using System.Linq;
using System.Reflection;
using Autofac;

namespace ZdravoCorp.Core.Utilities;

public static class Injector
{
    public static IContainer Container { get; set; }

    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        
        builder.RegisterModule<RepositoriesModule>();
        builder.RegisterModule<ServicesModule>();
        Container = builder.Build();
        return Container;
    }
}