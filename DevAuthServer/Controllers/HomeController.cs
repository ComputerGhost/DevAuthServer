using DevAuthServer.Handlers.Authorize;
using Microsoft.AspNetCore.Mvc;

namespace DevAuthServer.Controllers;

public class HomeController : Controller
{
    public IActionResult Login(GetAuthorizeIOModel model)
    {
        return View(model);
    }
}
