using DevAuthServer.Handlers.Discovery;
using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Controllers;

[Route(".well-known")]
public class WellKnownController : ControllerBase
{
    [HttpGet("openid-configuration")]
    public IActionResult OpenIdConfiguration()
    {
        var response = new DiscoveryHandler().Process();
        return Ok(response);
    }
}
