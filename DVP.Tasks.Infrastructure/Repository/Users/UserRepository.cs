
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.SeedWork;
using DVP.Tasks.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DVP.Tasks.Infraestructure.Repository.Users
{
    public class UserRepostory : IUserRepository
    {
        private readonly DVPContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserRepostory(DVPContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User> Add(User user)
        {
            user.CreatedAt = DateTime.UtcNow;

            if (string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.Email))
            {
                int idx = user.Email.IndexOf("@");
                user.Name = user.Email.Substring(0, idx);
            }

            
            var entityEntry = await _context.Users.AddAsync(user);
            return entityEntry.Entity; 
        }

        public async Task<bool> Disable(User user)
        {
            user.IsEnabled = false;
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public async Task<bool> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
    }
}