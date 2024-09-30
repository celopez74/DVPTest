using MediatR;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Api.Application.Commands.Users;

namespace DVP.Tasks.Api.Application.Commands.User
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand, Object>
    {
        private readonly IUserFinder _userFinder;
        private readonly IUserRoleRepository _userRoleRepository;

        public AddUserToRoleCommandHandler(IUserFinder userFinder, IUserRoleRepository userRoleRepository)
        {
            _userFinder = userFinder; 
            _userRoleRepository = userRoleRepository;
        }

        public async Task<Object> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var userToAddRole = await _userFinder.FindByIdAsync(request.UserId);
                if (userToAddRole == null)
                {
                    throw new Exception("User not found");   
                }                
                
                UserRole newRoleUser = new UserRole 
                   ( request.UserId,
                     (int) request.RoleId);
                

                await _userRoleRepository.Add(newRoleUser);
                var saveOk = await _userRoleRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                
                return saveOk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
