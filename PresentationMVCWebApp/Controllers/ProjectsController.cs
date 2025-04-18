using Microsoft.AspNetCore.Mvc;

namespace PresentationMVCWebApp.Controllers;

[Route("projects")]
public class ProjectsController : Controller
{
    [Route("")]
    public IActionResult Projects()
    {
        return View();
    }  
}
