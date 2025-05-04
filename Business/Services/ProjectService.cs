using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public interface IProjectService
{
    Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData);
    Task<ProjectResult> DeleteProjectAsync(string id);
    Task<ProjectResult<Project>> GetProjectAsync(string id);
    Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ProjectResult> UpdateProjectAsync(EditProjectFormData formData);
}

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ProjectResult> CreateProjectAsync(AddProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields were supplied." };

        //TO DO: Here control if Client and Member/User exist in Database or not

        //Map from an AddProjectFormData to a ProjectEntity  
        var projectEntity = formData.MapTo<ProjectEntity>();

        //Check that Status with Id 1 exists in database
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;
        if (status == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Status with id = 1 does not exist in database." };

        //Set StatusId (1) in projectEntity
        projectEntity.StatusId = status.Id;

        //Hard coding a template project picture
        projectEntity.Image = "/images/projects/project-template-purple.svg";

        //TO DO: Hard coding ClientId och UserId: TO BE CHANGED!!! 
        projectEntity.ClientId = "1";
        projectEntity.UserId = "1";

        //Create a new Project
        var result = await _projectRepository.AddAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync
            (
                orderByDescending: true,
                sortBy: s => s.Created,
                where: null,
                include => include.User,
                include => include.Status,
                include => include.Client
            );

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectAsync(string id)
    {
        var response = await _projectRepository.GetAsync
            (
                where: x => x.Id == id,
                include => include.User,
                include => include.Status,
                include => include.Client
            );

        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project '{id}' was not found." };
    }


    public async Task<ProjectResult> UpdateProjectAsync(EditProjectFormData formData)
    {
        if (formData == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields were supplied." };

        //TO DO: Here control if Client and Member/User exist in Database or not

        //Map from an EditProjectFormData to a ProjectEntity  
        var projectEntity = formData.MapTo<ProjectEntity>();

        //Check that Status with Id 2 (Completeted) exists in database
        var statusResult = await _statusService.GetStatusByIdAsync(2);
        var status = statusResult.Result;
        if (status == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Status with id = 2 does not exist in database." };

        //Set StatusId (2), "Completed" in projectEntity
        projectEntity.StatusId = status.Id;

        //TO DO: Hard coding ClientId och UserId: TO BE CHANGED!!! 
        projectEntity.ClientId = "1";
        projectEntity.UserId = "1";

        //Create a new Project
        var result = await _projectRepository.UpdateAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        //Get Project by Id
        var projectResult = await this.GetProjectAsync(id);
        var project = projectResult.Result;
        if (project == null)
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = $"Project '{id}' was not found - hence it could not be deleted." };

        //Map from a Project to a ProjectEntity
        var projectEntity = project.MapTo<ProjectEntity>();

        //Delete a Project (by ProjectEntity)
        var result = await _projectRepository.DeleteAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }
}
