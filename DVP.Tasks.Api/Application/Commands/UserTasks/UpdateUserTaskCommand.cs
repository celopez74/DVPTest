using FluentValidation;
using MediatR;
using static DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask;

namespace DVP.Tasks.Api.Application.Commands.UserTasks
{
    public class UpdateUserTaskCommand : IRequest<Object>    {
        
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus Status { get; set; }        
        public Guid UserId { get; set; }
        public TaskPriority Priority { get; set; }
        public string? Comments { get; set; }
        public DateTime? CompletionDate { get; set; }
    }
    
    public class UpdateUserTaskCommandValidator : AbstractValidator<UpdateUserTaskCommand>
    {
        public UpdateUserTaskCommandValidator()
        {          
            RuleFor(t => t.Title).NotEmpty();
            RuleFor(t => t.Status).NotEmpty().IsInEnum();
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.Priority).NotEmpty().IsInEnum();;
        }
    }
}
