using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Introspect;
using DevAuthServer.Handlers.Logout;
using DevAuthServer.Handlers.Token;
using DevAuthServer.Handlers.UserInfo;
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

        if (_browser.UserIdCookie == null)
        {
            // We aren't logged into the IdP.
            return Unauthorized();
        }

        var redirectUri = new AuthorizeHandler(_database, input, _browser.UserIdCookie).Process();
        return Redirect(redirectUri.ToString());
    }

    [HttpGet("end-session")]
    public IActionResult EndSession([FromQuery] LogoutInputModel input)
    {
        var redirectUri = new LogoutHandler(_database).Process(input, _browser.UserIdCookie);
        _browser.UserIdCookie = null;

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
    public IActionResult Introspect([FromForm] IntrospectInputModel input)
    {
        // Client creds may be set in header instead of body.
        var basicAuth = new Browser(Request, Response).BasicAuth;
        if (basicAuth != null)
        {
            input.client_id = basicAuth.Value.Item1;
            input.client_secret = basicAuth.Value.Item2;
        }

        var response = new IntrospectHandler(_database, input).Process();
        return Ok(response);
    }

    [HttpPost("jwks")]
    public IActionResult Jwks()
    {
        // Yea I'm just not going to do this.
        // I'm only supporting HS256. The spec says I have to support RS256 too.
        // Maybe later.
        throw new NotImplementedException();
    }

    [HttpPost("token")]
    public IActionResult Token([FromForm] TokenInputModel input)
    {
        // Client creds may be set in header instead of body.
        var basicAuth = _browser.BasicAuth;
        if (basicAuth != null)
        {
            input.client_id = basicAuth.Value.Item1;
            input.client_secret = basicAuth.Value.Item2;
        }

        input.Validate(_database);
        var response = new TokenHandler(_database, input).Process();
        return Ok(response);
    }

    [HttpGet("userinfo")]
    [HttpPost("userinfo")]
    public IActionResult UserInfo(
        [FromForm] UserInfoInputModel input,
        [FromQuery] string access_token)
    {
        // Creds can be set in many places (per rfc 6750)
        input.fromQuery = access_token;
        input.fromHeader = _browser.BearerAuth;
        input.Normalize();

        input.Validate(_database);

        var response = new UserInfoHandler(_database, input).Process();
        return Ok(response);
    }
}
