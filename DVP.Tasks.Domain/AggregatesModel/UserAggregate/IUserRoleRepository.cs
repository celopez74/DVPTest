using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate;

public interface IUserRoleRepository : IRepository<User>
{
    Task<UserRole> Add(UserRole userRole);
    bool Remove(UserRole userRole);
}
