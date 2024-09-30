using MediatR;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using DVP.Tasks.Domain.AggregatesModel.UserTaskAggregate;

namespace DVP.Tasks.Api.Application.Commands.UserTasks
{
    public class ReasignUserTaskCommandHandler : IRequestHandler<ReasignUserTaskCommand, Object>
    {
        private readonly IUserTaskFinder _userTaskFinder;
        private readonly IUserTaskRepository _userTaskRepository;

        public ReasignUserTaskCommandHandler(IUserTaskFinder userTaskFinder, IUserTaskRepository userTaskRepository)
        {
            _userTaskFinder = userTaskFinder; 
            _userTaskRepository = userTaskRepository;
        }

        public async Task<Object> Handle(ReasignUserTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var usertask = await _userTaskFinder.FindByIdAsync(request.TaskId);
                if (usertask == null)
                {
                    throw new Exception("User task not found");   
                }                
                
                usertask.UserId = request.UserId;

                await _userTaskRepository.Update(usertask);
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
