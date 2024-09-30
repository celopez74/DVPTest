using FluentValidation;
using MediatR;
using DVP.Tasks.Domain.Models;

namespace DVP.Tasks.Api.Application.Queries.Users
{
    public class GetUserQuery : IRequest<UserDto>
    {
        public Guid id { get; set; }

        public GetUserQuery(Guid userId)
        {
            id = userId;
        }

        public class GetUserQueryValidator: AbstractValidator<GetUserQuery>
        {
            public GetUserQueryValidator() 
            { 
                RuleFor(x => x.id).NotEmpty();
            }
        }
    }
}