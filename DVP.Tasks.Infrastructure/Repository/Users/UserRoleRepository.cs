using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.SeedWork;
using DVP.Tasks.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DVP.Tasks.Infraestructure.Repository.Users
{
    public class UserRoleRepostory : IUserRoleRepository
    {
        private readonly DVPContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserRoleRepostory(DVPContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserRole> Add(UserRole userRole)
        {
            var entityEntry = await _context.UserRole.AddAsync(userRole);
            return entityEntry.Entity; 
        }

        public bool Remove(UserRole userRole)
        {
            _context.UserRole.Remove(userRole);
            return true;
        }        
    }
}