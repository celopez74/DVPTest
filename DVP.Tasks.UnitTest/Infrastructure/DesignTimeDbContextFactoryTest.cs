using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;

namespace DVP.Tasks.Infrastructure.Tests
{
    public class DesignTimeDbContextFactoryTests
    {
        private IConfiguration BuildConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return configurationBuilder.Build();
        }

        //[Fact]
        // public void CreateDbContext_ShouldReturnDVPContext()
        // {
        //     // Arrange
        //     var configuration = BuildConfiguration();
        //     var factory = new DesignTimeDbContextFactory();

        //     // Act
        //     var context = factory.CreateDbContext(new string[] {});

        //     // Assert
        //     Assert.NotNull(context);
        //     Assert.IsType<DVPContext>(context);
        // }

        // [Fact]
        // public void CreateDbContext_ShouldThrowException_WhenConnectionStringIsNotFound()
        // {
        //     // Arrange
        //     var configuration = new ConfigurationBuilder().Build(); 
        //     var factory = new DesignTimeDbContextFactory();

        //     // Act & Assert
        //     var exception = Assert.Throws<InvalidOperationException>(() => factory.CreateDbContext(new string[] { }));
        //     Assert.Equal("The connection string 'DefaultConnection' is not found in the configuration.", exception.Message);
        // }
    }
}
