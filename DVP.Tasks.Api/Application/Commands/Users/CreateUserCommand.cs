using System.Text.RegularExpressions;
using FluentValidation;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.Users
{
    public class CreateUserCommand : IRequest<Object>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }

        public CreateUserCommand() { }

        public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserCommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Nickname).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Password).MinimumLength(8).Must(x => IsValidPassword(x))
                    .WithMessage("La contraseña no cumple con los criterios de seguridad");
            }
        }
        private static bool IsValidPassword(string password)
        {
            if (password != null)
            {
                Regex validatePasswordRegex = 
                new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)(?!.*\s).{8,}$", RegexOptions.None, TimeSpan.FromMilliseconds(100));
            
                return validatePasswordRegex.IsMatch(password);
            }
            else
            {
                return false;
            }
            
        }
    }
}