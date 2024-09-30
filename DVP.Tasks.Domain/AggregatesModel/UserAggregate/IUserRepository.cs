using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate;

public interface IUserRepository : IRepository<User>
{
    Task<User> Add(User user);
    Task<bool> Update(User user);
    Task<bool> Disable(User user);
}

