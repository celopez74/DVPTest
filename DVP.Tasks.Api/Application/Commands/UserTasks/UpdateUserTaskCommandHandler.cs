using MediatR;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Commands.UserTasks
{
    public class UpdateUserTaskCommandHandler : IRequestHandler<UpdateUserTaskCommand, Object>
    {
        private readonly IUserTaskRepository _userTaskRepository;
        private readonly IUserTaskFinder _userTaskFinder;

        public UpdateUserTaskCommandHandler(IUserTaskRepository userTaskRepository, IUserTaskFinder userTaskFinder)
        {
            _userTaskRepository = userTaskRepository;
            _userTaskFinder = userTaskFinder; 
        }

        public async Task<Object> Handle(UpdateUserTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var userTaskToUpdate = await _userTaskFinder.FindByIdAsync(request.Id);
                if (userTaskToUpdate == null)
                {
                    throw new Exception("User task not found");   
                }
                userTaskToUpdate.Title = request.Title;
                userTaskToUpdate.Description = request.Description;
                userTaskToUpdate.Status = request.Status;
                userTaskToUpdate.DueDate = DateTime.UtcNow;
                userTaskToUpdate.UserId = request.UserId;
                userTaskToUpdate.Priority = request.Priority;
                userTaskToUpdate.Comments = request.Comments;
                userTaskToUpdate.CompletionDate = request.CompletionDate;

                await _userTaskRepository.Update(userTaskToUpdate);
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
