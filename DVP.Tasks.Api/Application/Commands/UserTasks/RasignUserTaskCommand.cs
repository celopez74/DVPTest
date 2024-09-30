using DVP.Tasks.Domain.AggregatesModel.RoleAggregate;
using FluentValidation;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.UserTasks
{
    public class ReasignUserTaskCommand : IRequest<Object>    {
        
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
       
    }
    
    public class ReasingUserTaskCommandValidator : AbstractValidator<ReasignUserTaskCommand>
    {
        public ReasingUserTaskCommandValidator()
        {          
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.TaskId).NotEmpty();
        }
    }
}