using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Logout;
using DevAuthServer.Handlers.Token;
using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Controllers;

[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly Browser _browser;
    private readonly Database _database;

    public AuthController(Database database)
    {
        _browser = new Browser(Request, Response);
        _database = database;
    }

    [HttpGet("authorize")]
    public IActionResult Authorize_Get([FromQuery] AuthorizeInputModel input)
    {
        input.Validate();

        // MVC controller will present the login form.
        return RedirectToAction("Login", "Home", input);
    }

    [HttpPost("authorize")]
    public IActionResult Authorize_Post([FromForm] AuthorizeInputModel input)
    {
        input.Validate();

        if (_browser.UserId == null)
        {
            // We aren't logged into the IdP.
            return Unauthorized();
        }

        var redirectUri = new AuthorizeHandler(_database, input, _browser.UserId).Process();
        return Redirect(redirectUri.ToString());
    }

    [HttpGet("end-session")]
    public IActionResult EndSession_Get([FromQuery] LogoutInputModel input)
    {
        var redirectUri = new LogoutHandler(_database).Process(input, _browser.UserId);
        _browser.UserId = null;

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

    [HttpPost("token")]
    public IActionResult Token_Post([FromForm] TokenInputModel input)
    {
        input.Validate();
        var todo = new TokenHandler().Process();
        return Ok();
    }

    [HttpGet("userinfo")]
    public IActionResult UserInfo_Get()
    {
        Todo.ProcessUserInfo();
        return Ok();
    }

    [HttpOptions("userinfo")]
    public IActionResult UserInfo_Options()
    {
        Todo.ProcessUserInfo();
        return Ok();
    }

    [HttpPost("userinfo")]
    public IActionResult UserInfo_Post()
    {
        Todo.ProcessUserInfo();
        return Ok();
    }
}
