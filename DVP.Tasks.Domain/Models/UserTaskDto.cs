using DVP.Tasks.Domain.SeedWork;
using static DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask;

namespace DVP.Tasks.Domain.Models;

public class UserTaskDto : IDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public AggregatesModel.UserTaskAggregate.UserTask.TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid UserId { get; set; }
    public TaskPriority Priority { get; set; }
    public string? Comments { get; set; }
    public DateTime? CompletionDate { get; set; }    

}
