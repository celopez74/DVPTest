using Azure.Identity;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;

namespace DVP.Tasks.Infraestructure.Services
{
    public class AzureActiveDirectoryService : IAzureActiveDirectoryService
    {
        private readonly IConfiguration _configuration;
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _domain;

        public AzureActiveDirectoryService(IConfiguration configuration)
        {
            _configuration = configuration;
            _tenantId = _configuration.GetValue<string>("Azure:TenantId") ?? "";
            _clientId = _configuration.GetValue<string>("Azure:ClientId") ?? "";
            _clientSecret = _configuration.GetValue<string>("Azure:ClientSecret") ?? "";
            _domain = _configuration.GetValue<string>("Azure:Domain") ?? "";

        }
        public async Task<Guid> CreateAadUser(string name, string email, string password)
        {
            var authority = $"https://login.microsoftonline.com/{_tenantId}/v2.0";
            
            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            var scopes = new[] { "https://graph.microsoft.com/.default" };            
            var authResult = await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync();

                
            var options = new DeviceCodeCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                ClientId = _clientId,
                TenantId = _tenantId,                
                DeviceCodeCallback = (code, cancellation) =>
                {
                    Console.WriteLine(code.Message);
                    return Task.FromResult(0);
                },
            };
            var credential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            var graphClient = new GraphServiceClient(credential, scopes);
            
            var requestBody = new Microsoft.Graph.Models.User
            {
                DisplayName = name,
                Identities = new List<Microsoft.Graph.Models.ObjectIdentity>
                {                    
                    new Microsoft.Graph.Models.ObjectIdentity
                    {
                        SignInType = "emailAddress",
                        Issuer = _domain,
                        IssuerAssignedId = email,
                    },
                },
                PasswordProfile = new Microsoft.Graph.Models.PasswordProfile
                {
                    Password = password,
                    ForceChangePasswordNextSignIn = false,
                },
                PasswordPolicies = "DisablePasswordExpiration",
            };

            try
            {
                var result = await graphClient.Users.PostAsync(requestBody);                
                return Guid.Parse(result.Id);
            }
            catch (ServiceException ex)
            {                
                Console.WriteLine($"Error al crear el usuario: {ex.Message}");
                return Guid.Empty;
            }
            
        }

        public async Task<AadTokenResponse> getTokenFromAAD(string AADId, string password)
        {
            using (var client = new HttpClient())
            {
                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret }, 
                    { "scope", $"https://{_domain}/{_clientId}/.default" },
                    { "username", $"{AADId}@{_domain}"},
                    { "password", password }
                };

                var requestContent = new FormUrlEncodedContent(requestBody);
                var response = await client.PostAsync($"https://login.microsoftonline.com/{_tenantId}/oauth2/v2.0/token", requestContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var tokenResponse = JsonConvert.DeserializeObject<UserTokenResponse>(responseString);                    
                    return new AadTokenResponse
                    {
                        IsValidate = true,
                        TokenResponse = tokenResponse
                    };                  
                }
                else
                {
                    var errorResponse = JsonConvert.DeserializeObject<UserTokenErrorResponse>(responseString);
                    Console.WriteLine($"Error: {errorResponse.error_description}");
                    return new AadTokenResponse
                    {
                        IsValidate = false,
                        TokenErrorResponse = errorResponse
                    };                       
                }
            }
        }

        public async Task<bool> disableAADUser(string AADId)
        {
            var authority = $"https://login.microsoftonline.com/{_tenantId}/v2.0";

            var confidentialClient = ConfidentialClientApplicationBuilder
                .Create(_clientId)
                .WithClientSecret(_clientSecret)
                .WithAuthority(new Uri(authority))
                .Build();

            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var authResult = await confidentialClient.AcquireTokenForClient(scopes).ExecuteAsync();

            var credential = new ClientSecretCredential(_tenantId, _clientId, _clientSecret);
            var graphClient = new GraphServiceClient(credential, scopes);
            
            var userId = AADId; 
            var updateUser = new Microsoft.Graph.Models.User
            {
                AccountEnabled = false 
            };

            try
            {               
                await graphClient.Users[userId].PatchAsync(updateUser);
                Console.WriteLine("Usuario inactivado correctamente");
                return true;                
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error al inactivar el usuario: {ex.Message}");
                return false;
            }

        }
    }

}