
using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
public interface IUserTaskRepository : IRepository<UserTask>
{
    Task<UserTask> Add(UserTask userTask);
    Task<bool> Update(UserTask userTask);
    Task<bool> Delete(UserTask userTask);
}

