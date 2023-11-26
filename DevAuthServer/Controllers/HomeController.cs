using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Login;
using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    private readonly Database _database;

    public HomeController(Database database)
    {
        _database = database;
    }

    [HttpGet("login")]
    public IActionResult GetLogin([FromQuery] GetAuthorizeIOModel input)
    {
        input.Validate(_database);
        return View(input);
    }

    [HttpPost("login")]
    public IActionResult PostLogin([FromForm] LoginIOModel input)
    {
        var userId = new PostLoginHandler(_database, input).Process();
        Response.Cookies.Append(Todo.USERID_COOKIE_NAME, userId);
        return RedirectPreserveMethod("/auth/authorize");
    }
}
