using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Infraestructure.Services;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.Users
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAzureActiveDirectoryService _aadService;

        public CreateUserCommandHandler(IUserRepository userRepository, IAzureActiveDirectoryService aadService)
        {
            _userRepository = userRepository;
            _aadService = aadService;

        }

        public async Task<Object> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {                
                var id = await _aadService.CreateAadUser(request.Name, request.Email, request.Password);
                var userToCreate = new Domain.AggregatesModel.UserAggregate.User(
                    id, request.Name, request.Email, request.Nickname, null);
                var userSaved = _userRepository.Add(userToCreate);
                var saveOk = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                if (saveOk)
                {
                    return new
                    {
                        aadId = id,                        
                    };
                }
                else
                {
                    return saveOk;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
