using Autofac;
using DVP.Tasks.Infrastructure.Modules.Autofac;
using DVP.Tasks.Domain.Models;
using DVP.Tasks.Infraestructure.Services;
using MediatR;
using LazyCache;
using Moq;

public class MediatorModuleTests
{
    [Fact]
    public void Load_ShouldRegisterTypesCorrectly()
    {
        // Arrange
        var builder = new ContainerBuilder();
        var module = new MediatorModule();

        // Act
        // Use reflection to call the protected Load method
        var method = typeof(MediatorModule).GetMethod("Load", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(module, new object[] { builder });

        // Register mock for IAppCache
        var mockCache = new Mock<IAppCache>();
        builder.RegisterInstance(mockCache.Object).As<IAppCache>();

        // Build the container
        var container = builder.Build();

        // Assert
        // Assert that UserDto is registered
        Assert.True(container.IsRegistered<UserDto>());
        // Assert that UserDto is registered
        Assert.True(container.IsRegistered<RoleDto>());
        // Assert
        // Assert that UserRoleDto is registered
        Assert.True(container.IsRegistered<UserRoleDto>());

        // Assert that IMediator implementations are registered
        var mediatorInstance = container.Resolve<IMediator>();
        Assert.NotNull(mediatorInstance);

        // Assert that IAppCache is registered
        var cache = container.Resolve<IAppCache>();
        Assert.NotNull(cache);
    }
}
