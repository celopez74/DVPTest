using MediatR;
using Microsoft.AspNetCore.Mvc;
using DVP.Tasks.Domain.Exception;
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Api.Application.Queries.Users;
using Microsoft.AspNetCore.Authorization;


namespace DVP.Tasks.Api.Controllers.V1
{
    public class UserController : DVPController
    {

        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [Authorize]
        [Route("{uuid}/")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(Guid uuid)
        {
            try
            {
                var user = await _mediator.Send(new GetUserQuery(uuid));
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
        [Route("get-all-users")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllUsers(int pageNumber, int pageSize)
        {
            try
            {
                var user = await _mediator.Send(new GetUserListQuery(pageNumber, pageSize));
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
        public async Task<IActionResult> CreateUser(CreateUserCommand user)
        {
            try
            {
                var result = await _mediator.Send(user);
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
        [Route("add-role")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserToRole(AddUserToRoleCommand userRole)
        {
            try
            {
                var result = await _mediator.Send(userRole);
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
        public async Task<IActionResult> UpdateUser(UpdateUserCommand user)
        {
            try
            {
                var result = await _mediator.Send(user);
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
        public async Task<IActionResult> DeleteUserTask(DeleteUserCommand user)
        {
            try
            {
                var result = await _mediator.Send(user);
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
