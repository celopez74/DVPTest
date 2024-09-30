using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        
        services.AddHttpClient();
    }
    public void Configure(IApplicationBuilder app)
    {
        
    }

}