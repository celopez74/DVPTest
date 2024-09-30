using FluentValidation;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.UserTasks
{
    public class DeleteUserTaskCommand : IRequest<Object>    {
        
        public Guid TaskId { get; set; }
       
    }
    
    public class DeleteUserTaskCommandValidator : AbstractValidator<DeleteUserTaskCommand>
    {
        public DeleteUserTaskCommandValidator()
        {          
            RuleFor(t => t.TaskId).NotEmpty();
        }
    }
}
