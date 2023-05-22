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

        builder.RegisterModule<ServicesModule>();
        builder.RegisterModule<RepositoriesModule>();
        Container = builder.Build();
        return Container;
    }
}