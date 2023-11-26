using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Controllers;

[Route(".well-known")]
public class WellKnownController : ControllerBase
{
    [HttpGet("openid-configuration")]
    public IActionResult OpenIdConfiguration_Get()
    {
        return Ok();
    }
}
