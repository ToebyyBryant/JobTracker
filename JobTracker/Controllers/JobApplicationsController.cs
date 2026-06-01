using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using JobTracker.Data;
using JobTracker.Models;

namespace JobTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class JobApplicationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobApplicationsController(AppDbContext context)
    {
        _context = context;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var jobs = await _context.JobApplications
            .Where(j => j.UserId == GetUserId())
            .ToListAsync();
        return Ok(jobs);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] JobApplicationRequest request)
    {
        var job = new JobApplication
        {
            UserId = GetUserId(),
            CompanyName = request.CompanyName,
            JobTitle = request.JobTitle,
            JobUrl = request.JobUrl,
            DateApplied = request.DateApplied,
            Status = request.Status ?? "Applied",
            Notes = request.Notes
        };

        _context.JobApplications.Add(job);
        await _context.SaveChangesAsync();
        return Ok(job);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] JobApplicationRequest request)
    {
        var job = await _context.JobApplications
            .FirstOrDefaultAsync(j => j.Id == id && j.UserId == GetUserId());

        if (job == null) return NotFound();

        job.CompanyName = request.CompanyName;
        job.JobTitle = request.JobTitle;
        job.JobUrl = request.JobUrl;
        job.DateApplied = request.DateApplied;
        job.Status = request.Status ?? job.Status;
        job.Notes = request.Notes;

        await _context.SaveChangesAsync();
        return Ok(job);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _context.JobApplications
            .FirstOrDefaultAsync(j => j.Id == id && j.UserId == GetUserId());

        if (job == null) return NotFound();

        _context.JobApplications.Remove(job);
        await _context.SaveChangesAsync();
        return Ok("Deleted.");
    }
}

public record JobApplicationRequest(
    string CompanyName,
    string JobTitle,
    string? JobUrl,
    DateTime DateApplied,
    string? Status,
    string? Notes
);