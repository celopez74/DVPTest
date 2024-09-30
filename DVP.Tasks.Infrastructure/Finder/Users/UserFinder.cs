using Microsoft.EntityFrameworkCore;
using System.Data;
using DVP.Tasks.Domain.SeedWork;
using DVP.Tasks.Domain.Models;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Infrastructure.Finder.Users
{
    public class UserFinder : IUserFinder
    {
        private readonly DVPContext _context;
        private readonly IUserRoleFinder _userRoleFinder;

        public UserFinder(DVPContext context, IUserRoleFinder userRoleFinder)
        {
            _context = context;
            _userRoleFinder = userRoleFinder;
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public async Task<UserDto> GetUserDtoByIdAsync(Guid userId)
        {
            var user = await FindByIdAsync(userId);
            if (user == null)
                return null;

            var userDto = new UserDto 
            {                                   
                Id = user.Id,
                Name = user.Name,
                Nickname = user.Nickname,
                Roles = await _userRoleFinder.GetRolesByUserIdAsync(user.Id),
                CreatedAt = user.CreatedAt,
                IsEnabled = user.IsEnabled,           
            };           
            

            return userDto;
        }  

        public  async Task<User> FindByIdAsync(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == userId);
            if (user == null)
                return null;
            return user;
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(t => t.Email == email);
            if (user == null)
                return null;
            return user;
        }

        public async Task<List<User>> GetUsersPagedAsync(int pageNumber, int pageSize)
        {
            var users = await _context.Users.Skip((pageNumber - 1) * pageSize)  
                            .Take(pageSize)
                            .ToListAsync();

            foreach(var u in users)
            {
                u.Roles = await _userRoleFinder.GetRolesByUserIdAsync(u.Id);
            }
            if (users == null)
                return null;
            return users;
        }
    }
}
