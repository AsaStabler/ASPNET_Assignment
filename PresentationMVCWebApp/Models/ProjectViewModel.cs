namespace PresentationMVCWebApp.Models;

public class ProjectViewModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    //public string Id { get; set; } = null!;
    public string Image { get; set; } = null!;
    public string ProjectName { get; set; } = null!;
    public string ClientName { get; set; } = null!;
    public string Description { get; set; } = null!;
    //public string TimeLeft { get; set; } = null!;
    //public IEnumerable<string> Members { get; set; } = [];
}


