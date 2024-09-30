using FluentValidation;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.Users
{
    public class UpdateUserCommand : IRequest<Object>    {
        
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string Email { get; set; }
        public string Nickname { get; set; } 
        public bool IsEnabled { get; set;}
    }
    
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {          
            RuleFor(command => command.Id).NotEmpty().WithMessage("User id is required."); 
            RuleFor(command => command.Name).NotEmpty().WithMessage("User name is required.");
            RuleFor(command => command.Email).NotEmpty().WithMessage("User email is required.");
            RuleFor(command => command.Nickname).NotEmpty().WithMessage("User nickname is required.");
            RuleFor(command => command.IsEnabled).Must(x => x == false || x == true);
        }
    }
}
