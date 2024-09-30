using FluentValidation;
using MediatR;
using static DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate.UserTask;


namespace DVP.Tasks.Api.Application.Commands.UsersTask
{
    public class CreateUserTaskCommand : IRequest<Object>
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Domain.AggregatesModel.UserTaskAggregate.UserTask.TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid UserId { get; set; }
        public TaskPriority Priority { get; set; }
        public string? Comments { get; set; }
        public DateTime? CompletionDate { get; set; }

        public CreateUserTaskCommand() { }

        public class CreateUserTaskCommandValidator : AbstractValidator<CreateUserTaskCommand>
        {
            public CreateUserTaskCommandValidator()
            {
                RuleFor(t => t.Title).NotEmpty();
                RuleFor(t => t.Status).NotEmpty().IsInEnum();
                RuleFor(t => t.CreatedAt).NotEmpty();
                RuleFor(t => t.UserId).NotEmpty();
                RuleFor(t => t.Priority).NotEmpty().IsInEnum();;
            }
        }
    }
}