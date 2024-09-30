using MediatR;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Infraestructure.Services;

namespace DVP.Tasks.Api.Application.Commands.User
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFinder _userFinder;
        private readonly IAzureActiveDirectoryService _aadService;

        public DeleteUserCommandHandler(IUserRepository userRepository, IUserFinder userFinder, IAzureActiveDirectoryService aadService)
        {
            _userRepository = userRepository;
            _userFinder = userFinder; 
            _aadService = aadService;
        }

        public async Task<Object> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var userToDelete = await _userFinder.FindByIdAsync(request.UserId);
                if (userToDelete == null)
                {
                    throw new Exception("User task not found");   
                }                
                ///Really not delete, the user is disabled
                await _userRepository.Disable(userToDelete);
                var saveOk = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                ///The Same in AAD, set to disabled
                await _aadService.disableAADUser(request.UserId.ToString());
                return saveOk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
