using FluentValidation;
using MediatR;

namespace DVP.Tasks.Api.Application.Queries.UserTask
{
    public class GetUserTaskListQuery : IRequest<List<Domain.AggregatesModel.UserTaskAggregate.UserTask>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public GetUserTaskListQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public class GetUserTaskListQueryValidator: AbstractValidator<GetUserTaskListQuery>
        {
            public GetUserTaskListQueryValidator()             
            { 
                RuleFor(x => x.PageNumber).NotEmpty();
                RuleFor(x => x.PageSize).NotEmpty();
            }
        }
    }
}