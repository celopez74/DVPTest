using FluentValidation;
using MediatR;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Api.Application.Queries.Users
{
    public class GetUserListQuery : IRequest<List<User>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetUserListQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public class GetUserQueryValidator: AbstractValidator<GetUserListQuery>
        {
            public GetUserQueryValidator()             
            { 
                RuleFor(x => x.PageNumber).NotEmpty();
                RuleFor(x => x.PageSize).NotEmpty();
            }
        }
    }
}