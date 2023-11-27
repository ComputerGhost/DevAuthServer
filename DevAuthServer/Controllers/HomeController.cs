using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Login;
using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    private readonly Browser _browser;
    private readonly Database _database;

    public HomeController(Database database)
    {
        _browser = new Browser(Request, Response);
        _database = database;
    }

    [HttpGet("login")]
    public IActionResult GetLogin([FromQuery] AuthorizeInputModel input)
    {
        input.Validate();
        return View(input);
    }

    [HttpPost("login")]
    public IActionResult PostLogin([FromForm] LoginInputModel input)
    {
        // We'll handle our login to our IdP here.
        _browser.UserIdCookie = new LoginHandler(_database, input).Process();

        // Then redirect to our OAuth system for the response to the client.
        return RedirectPreserveMethod("/auth/authorize");
    }
}
