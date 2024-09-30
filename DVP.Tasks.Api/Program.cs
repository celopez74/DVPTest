using System.Reflection;
using DVP.Tasks.Api.SeedWork;
using DVP.Tasks.Api.Startup;
using DVP.Tasks.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "Configuration"))
    .AddJsonFile(        
        $"appsettings.{builder.Environment.EnvironmentName}.json",
        optional: false,
        reloadOnChange: false);

ServiceSetup.ConfigureServices(builder, Assembly.GetExecutingAssembly());
AutofacSetup.ConfigureAutofac(builder);
var app = builder.Build();
MiddlewareSetup.ConfigureMiddlewares(app);

app.Run();