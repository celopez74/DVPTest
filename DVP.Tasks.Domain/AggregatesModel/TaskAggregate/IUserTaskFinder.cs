using DVP.Tasks.Domain.Models;
using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate
{
    public interface IUserTaskFinder : IRepository<UserTask>
    {
        Task<List<UserTask>> GetUserTasksByUserIdAsync(Guid userId);
        Task<UserTaskDto> GetUserTaskDtoByIdAsync(Guid userTaksId);
        Task<UserTask> FindByIdAsync(Guid userTaksId);  
        Task<List<UserTask>> GetUserTasksPagedAsync(int pageNumber, int pageSize);
    }
}