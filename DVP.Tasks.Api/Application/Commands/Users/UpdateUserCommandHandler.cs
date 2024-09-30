using MediatR;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;

namespace DVP.Tasks.Api.Application.Commands.Users
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Object>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFinder _userFinder;

        public UpdateUserCommandHandler(IUserRepository userRepository, IUserFinder userFinder)
        {
            _userRepository = userRepository;
            _userFinder = userFinder; 
        }

        public async Task<Object> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var userToUpdate = await _userFinder.FindByIdAsync(request.Id);
                if (userToUpdate == null)
                {
                    throw new Exception("User not found");   
                }
                userToUpdate.Name = request.Name;
                userToUpdate.Email = request.Email;
                userToUpdate.Nickname = request.Nickname;
                userToUpdate.IsEnabled = request.IsEnabled;
                await _userRepository.Update(userToUpdate);
                var saveOk = await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
               
                return saveOk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
