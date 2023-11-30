using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Introspect;
using DevAuthServer.Handlers.Logout;
using DevAuthServer.Handlers.Token;
using DevAuthServer.Handlers.UserInfo;
using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace DevAuthServer.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly Database _database;

    public AuthController(Database database)
    {
        _database = database;
    }

    [HttpGet("authorize")]
    public IActionResult Authorize_Get([FromQuery] AuthorizeInputModel input)
    {
        input.Validate();

        // The MVC controller will present the login form.
        // Our input model is serialized and will be sent back to us in Authorize_Post.
        var authData = JsonConvert.SerializeObject(input);
        return RedirectToAction("Login", "Home", new { authData });
    }

    [HttpPost("authorize")]
    public IActionResult Authorize_Post([FromForm] AuthorizeInputModel input)
    {
        input.Validate();

        var userId = Request.Cookies["user-id"];
        if (userId == null)
        {
            // We aren't logged into the IdP.
            return Unauthorized();
        }

        var redirectUri = new AuthorizeHandler(_database, input, userId).Process();
        return Redirect(redirectUri.ToString());
    }

    [HttpGet("end-session")]
    public IActionResult EndSession([FromQuery] LogoutInputModel input)
    {
        var userIdCookie = Request.Cookies["user-id"];
        var redirectUri = new LogoutHandler(_database).Process(input, userIdCookie);
        Response.Cookies.Delete("user-id");

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
    public IActionResult Introspect(
        [FromForm] IntrospectInputModel input,
        [FromHeader] string? authorization)
    {
        // Client creds may be set in header instead of body.
        var basicAuth = ParseBasicAuth(authorization);
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
    public IActionResult Token(
        [FromForm] TokenInputModel input,
        [FromHeader] string? authorization)
    {
        // Client creds may be set in header instead of body.
        var basicAuth = ParseBasicAuth(authorization);
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
        [FromQuery] string access_token,
        [FromHeader] string authorization)
    {
        // Creds can be set in many places (per rfc 6750)
        input.fromQuery = access_token;
        input.fromHeader = ParseBearerAuth(authorization);
        input.Normalize();

        input.Validate(_database);

        var response = new UserInfoHandler(_database, input).Process();
        return Ok(response);
    }

    private static (string, string)? ParseBasicAuth(string? headerValue)
    {
        if (headerValue == null || !headerValue.StartsWith("basic ", StringComparison.CurrentCultureIgnoreCase))
        {
            return null;
        }

        var credentials = headerValue.Substring("basic ".Length);
        var decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(credentials));
        var parts = decoded.Split(':');
        return parts.Length == 1 ? (parts[0], parts[1]) : null;
    }

    private static string? ParseBearerAuth(string? headerValue)
    {
        if (headerValue == null || !headerValue.StartsWith("bearer ", StringComparison.CurrentCultureIgnoreCase))
        {
            return null;
        }
        return headerValue["bearer ".Length..];
    }
}
