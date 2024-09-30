using Microsoft.AspNetCore.Mvc;
using DVP.Tasks.Api.Controllers.V1;

namespace DVP.Tasks.Api.Controllers;

public class ValidatorController : ControllerBase
{
    private readonly string _validationText;

    public ValidatorController(IConfiguration configuration)
    {
        _validationText = configuration.GetValue<string>("DVPApisValidationText");
    }
    [HttpGet("/DVPapisverifydomain")]
    public async Task<IActionResult> GetDVPApis()
    {
        return Ok(_validationText);       
    }
}