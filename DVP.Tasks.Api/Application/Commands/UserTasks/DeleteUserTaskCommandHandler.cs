using MediatR;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Commands.UserTasks
{
    public class DeleteUserTaskCommandHandler : IRequestHandler<DeleteUserTaskCommand, Object>
    {
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IUserTaskFinder _userTaskFinder;

        public DeleteUserTaskCommandHandler(IUserTaskRepository userTaskRepository, IUserTaskFinder userTaskFinder)
        {
            _userTaskRepository = userTaskRepository;
            _userTaskFinder = userTaskFinder; 
        }

        public async Task<Object> Handle(DeleteUserTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var userTaskToDelete = await _userTaskFinder.FindByIdAsync(request.TaskId);
                if (userTaskToDelete == null)
                {
                    throw new Exception("User task not found");   
                }                

                await _userTaskRepository.Delete(userTaskToDelete);
                var saveOk = await _userTaskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
               
                return saveOk;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
