using DVP.Tasks.Api.Application.Commands.UsersTask;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;
using DVP.Tasks.Infraestructure.Services;
using MediatR;

namespace DVP.Tasks.Api.Application.Commands.UsersTask
{
    public class CreateUserTaskCommandHandler : IRequestHandler<CreateUserTaskCommand, Object>
    {
        private readonly IUserTaskRepository _userTaskRepository;

        public CreateUserTaskCommandHandler(IUserTaskRepository userTaskRepository)
        {
            _userTaskRepository = userTaskRepository;
        }

        public async Task<Object> Handle(CreateUserTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var id = Guid.NewGuid();
                var userTaskToCreate = new UserTask(
                    id,
                    request.Title,
                    request.Description,
                    request.Status,
                    request.UserId,
                    request.Priority,
                    request.Comments
                );
                var userSaved = _userTaskRepository.Add(userTaskToCreate);
                var saveOk = await _userTaskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
                if (saveOk)
                {
                    return new
                    {
                        userTaskId = id,                        
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
