using Business.Services;
using Microsoft.AspNetCore.Mvc;
using PresentationMVCWebApp.Models;

namespace PresentationMVCWebApp.Controllers;

[Route("projects")]
public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    [Route("")]
    public IActionResult Projects()
    {
        return View();
    }

   
    /*
    public async Task<IActionResult> Index()
    {
        //Reads in all Projects
        var model = new ProjectsViewModel
        {
            Projects = await _projectService.GetProjectsAsync(),
        };

        return View(model);
    }  
    */
}
