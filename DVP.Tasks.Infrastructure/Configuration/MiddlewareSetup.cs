using Microsoft.AspNetCore.Builder;
using Figgle;
using Microsoft.EntityFrameworkCore;
using Serilog;
using DVP.Tasks.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace DVP.Tasks.Api.Startup
{
    [ExcludeFromCodeCoverage]
    public static class MiddlewareSetup
    {
        public static void ConfigureMiddlewares(WebApplication app)
        {
            // Migrations
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DVPContext>();
                dbContext.Database.Migrate();
            }

            // Development-specific middlewares
            //if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            // Middleware pipeline
            
            app.UseCors("AllowOrigin");
            app.UseAuthorization();

            // Application Name
            var appName = app.Configuration.GetSection("AppName").Value;
            Console.WriteLine(FiggleFonts.Standard.Render(appName));
            Log.Information("Starting web host ({ApplicationContext})...", appName);

            // Endpoint mapping
            app.MapControllers();
        }
    }
}