using Microsoft.AspNetCore.Mvc;
using DVP.Tasks.Domain.Exception;
using DVP.Tasks.Api.Application.Commands.Users;
using DVP.Tasks.Domain.AggregatesModel.UserAggregate;
using Microsoft.Graph.Models;
using DVP.Tasks.Infraestructure.Services;



namespace DVP.Tasks.Api.Controllers.V1
{
    public class LoginController : DVPController
    {
        private readonly IUserFinder _userFinder;
        private readonly IAzureActiveDirectoryService _aadService;


        public LoginController(IUserFinder userFinder, IAzureActiveDirectoryService aadService)
        {
            _userFinder = userFinder;
            _aadService = aadService;
        }        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(UserLogin loginData)
        {
            try
            {
                var user = await _userFinder.FindByEmailAsync(loginData.Email) ?? throw new EntityNotFoundException(loginData.Email, "User does not exist");
                if (!user.IsEnabled) throw new DVPException("User is Disable");

                AadTokenResponse userValidation = await _aadService.getTokenFromAAD(user.Id.ToString(), loginData.Password);
                
                if(userValidation.IsValidate)
                {
                    return await SuccessResquest(userValidation.TokenResponse);
                }
                else
                {
                    if (userValidation.TokenErrorResponse.error_description.StartsWith("AADSTS50034"))
                    {
                        return await UnSuccessRequestNotFound("User does not exist in AAD");
                    } else if (userValidation.TokenErrorResponse.error_description.StartsWith("AADSTS50126"))
                    {
                        return await UnSuccessRequestNotFound("Error validating credentials due to invalid username or password");
                    } else
                    {
                        return await UnSuccessRequestNotFound(userValidation.TokenErrorResponse.error_description);
                    }
                }
                
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
