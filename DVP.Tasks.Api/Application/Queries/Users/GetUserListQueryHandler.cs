using MediatR;
using DVP.Tasks.Domain.Models;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Api.Application.Queries.Users
{
    public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<User>>
    {
        private readonly IUserFinder _userFinder;

        public GetUserListQueryHandler(IUserFinder userFinder)
        {
            _userFinder = userFinder;
        }

        public async Task<List<User>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var userDVP = await _userFinder.GetUsersPagedAsync(request.PageNumber, request.PageSize);
            return userDVP;
        }
    }
}
