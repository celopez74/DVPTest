using DVP.Tasks.Domain.Models;
using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRoleFinder : IRepository<UserRoleDto>
    {
        Task<List<int>> GetRolesByUserIdAsync(Guid userId);  
    }
}