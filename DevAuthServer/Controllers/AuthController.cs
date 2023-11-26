using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Controllers;

[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly Database _database;

    public AuthController(Database database)
    {
        _database = database;
    }

    [HttpGet("authorize")]
    public IActionResult Authorize_Get([FromQuery] GetAuthorizeIOModel input)
    {
        input.Validate(_database);
        return RedirectToAction("Login", "Home", input);
    }

    [HttpPost("authorize")]
    public IActionResult Authorize_Post(
        [FromForm] GetAuthorizeIOModel input)
    {
        input.Validate(_database);
        var model = new PostAuthorizeHandler(input, _database);
        return Ok(model);
    }

    [HttpGet("end-session")]
    public IActionResult EndSession_Get()
    {
        return Ok();
    }

    [HttpGet("introspect")]
    public IActionResult Introspect_Get()
    {
        return Ok();
    }

    [HttpPost("introspect")]
    public IActionResult Introspect_Post()
    {
        return Ok();
    }

    [HttpGet("jwks")]
    public IActionResult Jwks_Get()
    {
        return Ok();
    }

    [HttpPost("token")]
    public IActionResult Token_Post()
    {
        return Ok();
    }

    [HttpGet("userinfo")]
    public IActionResult UserInfo_Get()
    {
        return Ok();
    }

    [HttpOptions("userinfo")]
    public IActionResult UserInfo_Options()
    {
        return Ok();
    }

    [HttpPost("userinfo")]
    public IActionResult UserInfo_Post()
    {
        return Ok();
    }
}
