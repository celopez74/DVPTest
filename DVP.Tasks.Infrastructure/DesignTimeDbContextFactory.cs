using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using DVP.Tasks.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DVPContext>
{
    public DVPContext CreateDbContext(string[] args)
    {
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)           
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DVPContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");        

        optionsBuilder.UseSqlServer(connectionString);

        return new DVPContext(optionsBuilder.Options);   
       
    }
}