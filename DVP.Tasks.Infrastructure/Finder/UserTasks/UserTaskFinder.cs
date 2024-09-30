using Microsoft.EntityFrameworkCore;
using System.Data;
using DVP.Tasks.Domain.SeedWork;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using DVP.Tasks.Domain.Models;

namespace DVP.Tasks.Infrastructure.Finder.UsersTasks
{
    public class UserTaskFinder : IUserTaskFinder
    {
        private readonly DVPContext _context;


        public UserTaskFinder(DVPContext context)
        {
            _context = context;
           
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public  async Task<UserTask> FindByIdAsync(Guid userTaksId)
        {
            var userTask = await _context.UserTask.FirstOrDefaultAsync(t => t.Id == userTaksId);
            if (userTask == null)
                return null;
            return userTask;
        }

        public async Task<UserTaskDto> GetUserTaskDtoByIdAsync(Guid userTaksId)
        {
            var userTask = await FindByIdAsync(userTaksId);
            if (userTask == null)
                return null;

            var userTaskDto = new UserTaskDto 
            {                                   
                Id = userTask.Id,
                Title = userTask.Title,
                Description = userTask.Title,
                Status = userTask.Status,
                CreatedAt = userTask.CreatedAt,
                DueDate = userTask.DueDate,   
                UserId = userTask.UserId,
                Priority = userTask.Priority,
                Comments = userTask.Comments ,
                CompletionDate = userTask.CompletionDate,                                          
            };           
            

            return userTaskDto;
        }

        public async Task<List<UserTask>> GetUserTasksPagedAsync(int pageNumber, int pageSize)
        {
            var userTasks = await _context.UserTask
                                        .Skip((pageNumber - 1) * pageSize)  
                                        .Take(pageSize)
                                        .ToListAsync();

            return userTasks;
        }

        public  async Task<List<UserTask>> GetUserTasksByUserIdAsync(Guid userId)
        {
            var userTask = await _context.UserTask.Where(t => t.UserId == userId).ToListAsync();

            if (userTask == null)
                return null;
            return userTask;
        }
    }
}
