using MediatR;
using DVP.Tasks.Domain.Models;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Api.Application.Queries.UserTask;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Queries.Users
{
    public class GetUserTaskQueryHandler : IRequestHandler<GetUserTaskQuery, UserTaskDto>
    {
        private readonly IUserTaskFinder _userTaskFinder;

        public GetUserTaskQueryHandler(IUserTaskFinder userTaskFinder)
        {
            _userTaskFinder = userTaskFinder;
        }

        public async Task<UserTaskDto> Handle(GetUserTaskQuery request, CancellationToken cancellationToken)
        {
            var userTaskDVP = await _userTaskFinder.GetUserTaskDtoByIdAsync(request.id);
            return userTaskDVP;
        }
    }
}
