using DVP.Tasks.Domain.AggregatesModel.RoleAggregate;
using FluentValidation;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.Users
{
    public class AddUserToRoleCommand : IRequest<Object>    {
        
        public Guid UserId { get; set; }
        public RolesEnum RoleId { get; set; }
       
    }
    
    public class AddUserToRoleCommandValidator : AbstractValidator<AddUserToRoleCommand>
    {
        public AddUserToRoleCommandValidator()
        {          
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.RoleId).NotEmpty().IsInEnum();
        }
    }
}