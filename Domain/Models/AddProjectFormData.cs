﻿namespace Domain.Models;

public class AddProjectFormData
{
    public string? Image { get; set; }

    public string ProjectName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Budget { get; set; }

    public Client Client { get; set; } = null!;

    public User User { get; set; } = null!;

    //Hard coded to Status id=1, "Started"
    public Status Status { get; set; } = null!;
}
