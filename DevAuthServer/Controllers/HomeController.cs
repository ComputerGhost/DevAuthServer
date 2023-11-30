using DevAuthServer.Handlers.Authorize;
using DevAuthServer.Handlers.Login;
using DevAuthServer.Storage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public IActionResult Login([FromQuery] string authData)
    {
        // Non-api controllers don't deserialize complex objects in the query;
        // so, `authorize` sends it serialized, then we deserialize it manually.
        var input = JsonConvert.DeserializeObject<LoginInputModel>(authData)
            ?? throw new Exception("Serialized `LoginInputModel` is expected as `authData` parameter.");

        input.Validate();
        return View("Login", input);
    }

    [HttpPost("login")]
    public IActionResult PostLogin([FromForm] LoginInputModel input)
    {
        // We'll handle our login to our IdP here.
        new Browser(Request, Response).UserIdCookie = new LoginHandler(_database, input).Process();

        // Then redirect to our OAuth system for the response to the client.
        return RedirectPreserveMethod("/auth/authorize");
    }
}
