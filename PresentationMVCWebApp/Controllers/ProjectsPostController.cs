using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationMVCWebApp.Models;

namespace PresentationMVCWebApp.Controllers;

public class ProjectsPostController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel form)
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

        //Map from an AddProjectViewModel to an AddProjectFormData  
        var projectFormData = form.MapTo<AddProjectFormData>();

        //Send data to projectService
        var result = await _projectService.CreateProjectAsync(projectFormData);

        //TO DO: Handle result!
        //if (result)
        //{
        //    return Ok(new { success = true });
        //}
        //else
        //{
        //    return Problem("Unable to submit data.");
        //}

        //return RedirectToAction("Projects", "Projects");
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> EditProject(EditProjectViewModel form)
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

        //Map from an EditProjectViewModel to an EditProjectFormData  
        var projectFormData = form.MapTo<EditProjectFormData>();

        //Send data to projectService
        var result = await _projectService.UpdateProjectAsync(projectFormData);

        //TO DO: Handle result!
        //if (result)
        //{
        //    return Ok(new { success = true });
        //}
        //else
        //{
        //    return Problem("Unable to submit data.");
        //}

        //return RedirectToAction("Projects", "Projects");
        return Ok();
    }

    public async Task<IActionResult> DeleteProject(string id)
    {
        var result = await _projectService.DeleteProjectAsync(id);

        //TO DO: Handle result!

        //return View();
        return RedirectToAction("Projects", "Projects");
    }

}
