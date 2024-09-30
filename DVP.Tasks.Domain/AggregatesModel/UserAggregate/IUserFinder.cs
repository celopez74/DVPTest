using DVP.Tasks.Domain.Models;
using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserAggregate
{
    public interface IUserFinder : IRepository<UserDto>
    {
        Task<UserDto> GetUserDtoByIdAsync(Guid userId);
        Task<User> FindByIdAsync(Guid userId);
        Task<User> FindByEmailAsync(string email);
        Task<List<User>> GetUsersPagedAsync(int pageNumber, int pageSize);
    }
}