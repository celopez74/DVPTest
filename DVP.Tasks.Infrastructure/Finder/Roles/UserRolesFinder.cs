using Microsoft.EntityFrameworkCore;
using System.Data;
using DVP.Tasks.Domain.SeedWork;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Infrastructure.Finder.Users
{
    public class UserRoleFinder : IUserRoleFinder
    {
        private readonly DVPContext _context;
        public UserRoleFinder(DVPContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public async Task<List<int>> GetRolesByUserIdAsync(Guid userId)
        {
            var userRoles = await _context.UserRole.Where(ur => ur.UserId == userId).ToListAsync();
            if (userRoles == null)
                return null;

            List<int> roles = [];
            foreach(var r in userRoles)
            {
                roles.Add(r.RoleId);  
            }
            return roles;
        }
    }
}