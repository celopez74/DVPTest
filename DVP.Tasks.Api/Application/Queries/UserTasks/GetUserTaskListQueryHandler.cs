using MediatR;

using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Queries.UserTask
{
    public class GetUserTaskListQueryHandler : IRequestHandler<GetUserTaskListQuery, List<Domain.AggregatesModel.UserTaskAggregate.UserTask>>
    {
        private readonly IUserTaskFinder _userTaskFinder;

        public GetUserTaskListQueryHandler(IUserTaskFinder userTaskFinder)
        {
            _userTaskFinder = userTaskFinder;
        }

        public async Task<List<Domain.AggregatesModel.UserTaskAggregate.UserTask>> Handle(GetUserTaskListQuery request, CancellationToken cancellationToken)
        {
            var userDVP = await _userTaskFinder.GetUserTasksPagedAsync(request.PageNumber, request.PageSize);
            return userDVP;
        }
    }
}
