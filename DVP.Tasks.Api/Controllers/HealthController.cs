using Microsoft.AspNetCore.Mvc;
using DVP.Tasks.Api.Controllers.V1;

namespace DVP.Tasks.Api.Controllers;

public class HealthController : DVPController
{
    [HttpGet]
    public async Task<IActionResult> GetHealth()
    {
        return Ok("OK");
    }
}