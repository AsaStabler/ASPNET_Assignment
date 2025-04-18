using Microsoft.AspNetCore.Mvc;

namespace PresentationMVCWebApp.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    [Route("members")]
    public IActionResult Members()
    {
        return View();
    }
}
