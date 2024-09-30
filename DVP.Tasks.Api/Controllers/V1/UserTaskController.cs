using MediatR;
using Microsoft.AspNetCore.Mvc;
using DVP.Tasks.Domain.Exception;
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Api.Application.Commands.UsersTask;
using DVP.Tasks.Api.Application.Queries.UserTask;
using DVP.Tasks.Api.Application.Commands.UserTasks;
using Microsoft.AspNetCore.Authorization;


namespace DVP.Tasks.Api.Controllers.V1
{
    public class UserTaskController : DVPController
    {

        private readonly IMediator _mediator;

        public UserTaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [Route("{uuid}/")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTaskById(Guid uuid)
        {
            try
            {
                var userTask = await _mediator.Send(new GetUserTaskQuery(uuid));
                return await SuccessResquest(userTask);
            }
            catch (EntityNotFoundException r)
            {
                return await UnSuccessRequestNotFound(r.Message);
            }

            catch (DVPException r)
            {
                return await UnSuccessRequest(r.Message);
            }

            catch (BadRequestException b)
            {
                return await UnSuccessRequest(b.Message);
            }

            catch (Exception e)
            {
                return await UnSuccessRequest(e.Message);
            }
        }

        [Authorize]
        [Route("get-all-tasks")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTasks(int pageNumber, int pageSize)
        {
            try
            {
                var user = await _mediator.Send(new GetUserTaskListQuery(pageNumber, pageSize));
                return await SuccessResquest(user);
            }
            catch (EntityNotFoundException r)
            {
                return await UnSuccessRequestNotFound(r.Message);
            }

            catch (DVPException r)
            {
                return await UnSuccessRequest(r.Message);
            }

            catch (BadRequestException b)
            {
                return await UnSuccessRequest(b.Message);
            }

            catch (Exception e)
            {
                return await UnSuccessRequest(e.Message);
            }
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserTask(CreateUserTaskCommand userTask)
        {
            try
            {
                var result = await _mediator.Send(userTask);
                return await SuccessResquest(result);
            }
            catch (EntityNotFoundException r)
            {
                return await UnSuccessRequestNotFound(r.Message);
            }

            catch (DVPException r)
            {
                return await UnSuccessRequest(r.Message);
            }

            catch (BadRequestException b)
            {
                return await UnSuccessRequest(b.Message);
            }

            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        } 

        [Authorize]
        [Route("reasing-task")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReasignTask(ReasignUserTaskCommand userTask)
        {
            try
            {
                var result = await _mediator.Send(userTask);
                return await SuccessResquest(result);
            }
            catch (EntityNotFoundException r)
            {
                return await UnSuccessRequestNotFound(r.Message);
            }

            catch (DVPException r)
            {
                return await UnSuccessRequest(r.Message);
            }

            catch (BadRequestException b)
            {
                return await UnSuccessRequest(b.Message);
            }

            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }   

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserTask(UpdateUserTaskCommand userTask)
        {
            try
            {
                var result = await _mediator.Send(userTask);
                return await SuccessResquest(result);
            }
            catch (EntityNotFoundException r)
            {
                return await UnSuccessRequestNotFound(r.Message);
            }

            catch (DVPException r)
            {
                return await UnSuccessRequest(r.Message);
            }

            catch (BadRequestException b)
            {
                return await UnSuccessRequest(b.Message);
            }

            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        } 
        
        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserTask(DeleteUserTaskCommand userTask)
        {
            try
            {
                var result = await _mediator.Send(userTask);
                return await SuccessResquest(result);
            }
            catch (EntityNotFoundException r)
            {
                return await UnSuccessRequestNotFound(r.Message);
            }

            catch (DVPException r)
            {
                return await UnSuccessRequest(r.Message);
            }

            catch (BadRequestException b)
            {
                return await UnSuccessRequest(b.Message);
            }

            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }     
    }     
}
