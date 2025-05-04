using Business.Services;
using Domain.Extensions;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using PresentationMVCWebApp.Models;

namespace PresentationMVCWebApp.Controllers;

[Route("projects")]
public class ProjectsController(IProjectService projectService) : Controller
{
    private readonly IProjectService _projectService = projectService;

    [Route("")]
    public async Task<IActionResult> Projects(string id)
    {
        //ViewBag.OpenEdit = "no";

        //Reads in all Projects
        var response = await _projectService.GetProjectsAsync();
        IEnumerable<Project> projectList = response.Result!;
        var newList = projectList.Select(entity => entity!.MapTo<ProjectViewModel>());

        var viewModel = new ProjectsViewModel()
        {
            Projects = newList
        };

        //TO DO: Måste ändra detta - annars öppnas EditModalen efter att man har skapat
        //                           ett nytt project i AddModalen
        if (id != null)
        {
            //Get the Project which is to be edited
            var projectResult = await _projectService.GetProjectAsync(id);
            var project = projectResult.Result;
            if (project == null)
            {
                //Error msg to be displayed on View
                ViewBag.ErrorMessage = projectResult.Error;
            }
            else
            {
                //Map from a Project to an EditProjectViewModel
                EditProjectViewModel editProjectVM = project.MapTo<EditProjectViewModel>();

                //Set this EditProjectViewModel to ProjectsViewModel for the Projects view
                viewModel.EditProjectFormData = editProjectVM;

                viewModel.OpenEdit = "yes";
                //ViewBag.OpenEdit = "yes";
            }
        }

        //To Do: Set result in ViewBag

        return View(viewModel);
    }

    private IEnumerable<ProjectViewModel> SetProjects()
    {
        var projects = new List<ProjectViewModel>();

        projects.Add(new ProjectViewModel
        {
            Id = Guid.NewGuid().ToString(),
            Image = "/images/projects/project-template-purple.svg",
            ProjectName = "Utbildningsprojektet",
            ClientName = "Dep. of Education",
            Description = "Project to improve education for children."
        });
        
        return projects;
    }

    //This works!
    //[Route("")]
    //public IActionResult Projects()
    //{
    //    var viewModel = new ProjectsViewModel()
    //    {
    //        Projects = SetProjects()
    //    };
    //    return View(viewModel);
    //}
    
    //Conflicts with Action Projects - move this to ProjectsPostController
    //public IActionResult DeleteProject()
    //{
    //    //return View();
    //    return RedirectToAction("SignUp", "Auth");
    //}
}
