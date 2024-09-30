using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Infraestructure.Services
{
    public interface IAzureActiveDirectoryService
    {
        Task<Guid> CreateAadUser(string name, string email, string password);
        Task<AadTokenResponse> getTokenFromAAD(string AADId, string password);
        Task<bool> disableAADUser(string AADId);
    }
    
}