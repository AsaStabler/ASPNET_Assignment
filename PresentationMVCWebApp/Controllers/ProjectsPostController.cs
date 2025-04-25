using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace PresentationMVCWebApp.Controllers;

public class ProjectsPostController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    [HttpPost]
    public IActionResult AddProject(AddProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        /* Send data to projectService
        var result = await _projectService.AddProjectAsync(form);
        if (result)
        {
            return Ok(new { success = true });
        }
        else
        {
            return Problem("Unable to submit data.");
        } */

        //return RedirectToAction("Projects", "Projects");
        return Ok();
    }

    [HttpPost]
    public IActionResult EditProject(EditProjectForm form)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );

            return BadRequest(new { success = false, errors });
        }

        /* Send data to projectService
        var result = await _projectService.AddProjectAsync(form);
        if (result)
        {
            return Ok(new { success = true });
        }
        else
        {
            return Problem("Unable to submit data.");
        } */

        //return RedirectToAction("Projects", "Projects");
        return Ok();
    }
    
}
