using FluentValidation;
using MediatR;
using DVP.Tasks.Domain.Models;

namespace DVP.Tasks.Api.Application.Queries.UserTask
{
    public class GetUserTaskQuery : IRequest<UserTaskDto>
    {
        public Guid id { get; set; }

        public GetUserTaskQuery(Guid userId)
        {
            id = userId;
        }

        public class GetUserTaskValidator: AbstractValidator<GetUserTaskQuery>
        {
            public GetUserTaskValidator() 
            { 
                RuleFor(x => x.id).NotEmpty();
            }
        }
    }
}