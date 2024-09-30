using FluentValidation;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.Users
{
    public class DeleteUserCommand : IRequest<Object>    {
        
        public Guid UserId { get; set; }
       
    }
    
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {          
            RuleFor(t => t.UserId).NotEmpty();
        }
    }
}