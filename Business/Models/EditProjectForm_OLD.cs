/* using Microsoft.AspNetCore.Http; */
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class EditProjectForm_OLD
{
    public int Id { get; set; }

    [Display(Name = "Project Image", Prompt = "Select an image")]
    [DataType(DataType.Upload)]
    public string? ProjectImage { get; set; }
    /* [DataType(DataType.Upload)]
    public IFormFile? ProjectImage { get; set; } */

    [Display(Name = "Project Name", Prompt = "Project Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Client Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Enter a Project description")] /* 'Type something' enl. Figmafilen */
    [DataType(DataType.Text)]
    public string? Description { get; set; }

    [Display(Name = "Start Date", Prompt = "Enter Start date")]
    [DataType(DataType.DateTime)]
    [Required(ErrorMessage = "Required")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Display(Name = "End Date", Prompt = "Enter End date")]
    [DataType(DataType.DateTime)]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "")]
    [DataType(DataType.Currency)]
    public double? Budget { get; set; }
}
