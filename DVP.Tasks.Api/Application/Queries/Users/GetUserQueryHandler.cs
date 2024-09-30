using MediatR;
using DVP.Tasks.Domain.Models;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Api.Application.Queries.Users
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IUserFinder _userFinder;

        public GetUserQueryHandler(IUserFinder userFinder)
        {
            _userFinder = userFinder;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var userDVP = await _userFinder.GetUserDtoByIdAsync(request.id);
            return userDVP;
        }
    }
}
