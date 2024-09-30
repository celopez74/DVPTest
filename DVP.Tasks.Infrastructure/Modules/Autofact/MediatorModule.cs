using Autofac;
using Autofac.Extensions.DependencyInjection;
using MediatR;
using Module = Autofac.Module;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using DVP.Tasks.Domain.Models;
using DVP.Tasks.Infraestructure.Services;


namespace DVP.Tasks.Infrastructure.Modules.Autofac;

public class MediatorModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        
        builder.RegisterType<UserDto>().AsSelf();
        builder.RegisterType<RoleDto>().AsSelf();
        builder.RegisterType<UserRoleDto>().AsSelf();


        builder.RegisterType<AzureActiveDirectoryService>()
            .As<IAzureActiveDirectoryService>()
            .InstancePerLifetimeScope();
            
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        var services = new ServiceCollection();

        builder.Populate(services);
    }
}