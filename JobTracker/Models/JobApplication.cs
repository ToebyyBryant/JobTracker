namespace JobTracker.Models;

public class JobApplication
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string? JobUrl { get; set; }
    public DateTime DateApplied { get; set; }
    public string Status { get; set; } = "Applied";
    public string? Notes { get; set; }
}