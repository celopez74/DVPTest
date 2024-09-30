using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using DVP.Tasks.Infrastructure.Modules.Autofac;
using System.Diagnostics.CodeAnalysis;

namespace DVP.Tasks.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class AutofacSetup
    {
        public static void ConfigureAutofac(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
        {
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new InfrastructureModule());
        });
    }
    }
}