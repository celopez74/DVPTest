using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using DVP.Tasks.Infraestructure.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using System.Text.Json;
using System.Security.Cryptography;

namespace DVP.Tasks.Infrastructure.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class ServiceSetup
    {
        public static void ConfigureServices(WebApplicationBuilder builder, Assembly assemblies)
        {
            // DbContext
            builder.Services.AddDbContext<DVPContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // MediatR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(assemblies));

            // Controllers & Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // FluentValidation
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssembly(assemblies);       



            IdentityModelEventSource.ShowPII = true;           
            

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuers = new[] 
                    { 
                        $"https://login.microsoftonline.com/{builder.Configuration["Azure:TenantId"]}/v2.0",
                        $"https://sts.windows.net/{builder.Configuration["Azure:TenantId"]}/"
                    },
                    ValidAudience = $"https://{builder.Configuration["Azure:Domain"]}/{builder.Configuration["Azure:ClientId"]}", 
                    IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                    {
                        return GetSigningKeysAsync(kid, builder.Configuration["Azure:TenantId"]?? "").GetAwaiter().GetResult(); 
                    }
                };
            });            
            builder.Services.AddAuthorization();
        }
            
        private static async Task<List<SecurityKey>> GetSigningKeysAsync(string kid, string tenantId)
        {
            using var httpClient = new HttpClient();
            var jwksUri = $"https://login.microsoftonline.com/{tenantId}/discovery/v2.0/keys"; 
            try
            {
                var response = await httpClient.GetStringAsync(jwksUri);
                var keys = JsonDocument.Parse(response).RootElement.GetProperty("keys");

                var signingKeys = new List<SecurityKey>();
                foreach (var key in keys.EnumerateArray())
                {
                    if (key.GetProperty("kid").GetString() == kid)
                    {
                        var rsaParameters = new RSAParameters
                        {
                            Modulus = Base64UrlEncoder.DecodeBytes(key.GetProperty("n").GetString()),
                            Exponent = Base64UrlEncoder.DecodeBytes(key.GetProperty("e").GetString())
                        };

                        var rsa = RSA.Create();
                        rsa.ImportParameters(rsaParameters);
                        signingKeys.Add(new RsaSecurityKey(rsa) { KeyId = kid });
                    }
                }
                return signingKeys;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener JWKS: {ex.Message}");
                return new List<SecurityKey>(); // Devuelve una lista vacía en caso de error
            }
        }
    }
    
}