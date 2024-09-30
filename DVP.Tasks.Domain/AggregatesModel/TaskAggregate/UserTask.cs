using DVP.Tasks.Domain.SeedWork;

namespace DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
public class UserTask: Entity<Guid>,IAggregateRoot
{   
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public Guid UserId { get; set; }
    public TaskPriority Priority { get; set; }
    public string? Comments { get; set; }
    public DateTime? CompletionDate { get; set; }

    public UserTask(Guid id, string title, string description, TaskStatus status, Guid userId, TaskPriority priority, string comments): base(id)
    {           
        Id = id;
        Title = title;
        Description = description;
        Status = status;
        UserId = userId;
        Priority = priority;
        Comments = comments;
        CreatedAt = DateTime.UtcNow;
        DueDate = DateTime.UtcNow;
    }

    public enum TaskStatus
    {
        Pending,        
        InProgress,     
        Completed 
    }

    public enum TaskPriority
    {
        Low,            
        Medium,         
        High            
    }

}
