using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Logout;
using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;
using System.Web;

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
        return RedirectToAction("Login", "Home", input);
    }

    [HttpPost("authorize")]
    public IActionResult Authorize_Post([FromForm] GetAuthorizeIOModel input)
    {
        var userId = Request.Cookies[Todo.USERID_COOKIE_NAME];
        if (userId == null)
        {
            return Unauthorized();
        }

        input.Validate(_database);

        var redirectUri = new PostAuthorizeHandler(_database, input, userId).Process();
        return Redirect(redirectUri.ToString());
    }

    [HttpGet("end-session")]
    public IActionResult EndSession_Get(LogoutInputModel input)
    {
        var userId = Request.Cookies[Todo.USERID_COOKIE_NAME];
        var redirectUri = new LogoutHandler(_database).Process(input, userId);
        Response.Cookies.Delete(Todo.USERID_COOKIE_NAME);

        if (redirectUri != null)
        {
            return Redirect(redirectUri.ToString());
        }
        else
        {
            return Ok();
        }
    }

    [HttpPost("introspect")]
    public IActionResult Introspect_Post()
    {
        Todo.ProcessIntrospection();
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
