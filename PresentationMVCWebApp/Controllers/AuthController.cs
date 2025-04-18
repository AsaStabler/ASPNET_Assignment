using Microsoft.AspNetCore.Mvc;

namespace PresentationMVCWebApp.Controllers;

public class AuthController : Controller
{
    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Login()
    {
        return LocalRedirect("/projects");
        //return View();
    }
}
