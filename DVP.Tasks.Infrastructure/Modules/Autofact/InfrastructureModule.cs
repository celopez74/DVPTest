using Autofac;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using DVP.Tasks.Infraestructure.Repository.Users;
using DVP.Tasks.Infraestructure.Repository.UserTasks;
using DVP.Tasks.Infrastructure.Finder.Users;
using DVP.Tasks.Infrastructure.Finder.UsersTasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace DVP.Tasks.Infrastructure.Modules.Autofac;

public class InfrastructureModule : Module
{

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register(ctx => new HttpClient())
            .SingleInstance();

        builder.RegisterType<UserRepostory>()
            .As<IUserRepository>()
            .InstancePerLifetimeScope();
        builder.RegisterType<UserRoleRepostory>()
            .As<IUserRoleRepository>()
            .InstancePerLifetimeScope();
        builder.RegisterType<UserRoleFinder>()
            .As<IUserRoleFinder>()
            .InstancePerLifetimeScope();
        builder.RegisterType<UserTaskRepostory>()
            .As<IUserTaskRepository>()
            .InstancePerLifetimeScope();
        builder.RegisterType<UserTaskFinder>()
            .As<IUserTaskFinder>()
            .InstancePerLifetimeScope();
        builder.RegisterType<UserFinder>()
            .As<IUserFinder>()
            .InstancePerLifetimeScope();          
       
        builder.Register(ctx => new HttpClient())
            .SingleInstance();

    }
}