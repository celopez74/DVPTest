
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using DVP.Tasks.Domain.SeedWork;
using DVP.Tasks.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DVP.Tasks.Infraestructure.Repository.UserTasks
{
    public class UserTaskRepostory : IUserTaskRepository
    {
        private readonly DVPContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public UserTaskRepostory(DVPContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserTask> Add(UserTask userTask)
        {
            userTask.CreatedAt = DateTime.UtcNow;
            userTask.DueDate = DateTime.UtcNow;         
            var entityEntry = await _context.UserTask.AddAsync(userTask);
            return entityEntry.Entity; 
        }

        public async Task<bool> Delete(UserTask userTask)
        {
            _context.UserTask.Remove(userTask);
            return true;
        }

        public async Task<bool> Update(UserTask userTask)
        {
            _context.Entry(userTask).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
    }
}