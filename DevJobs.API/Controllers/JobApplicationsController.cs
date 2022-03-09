using Microsoft.AspNetCore.Mvc;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Entities;

namespace DevJobs.API.Controllers;

[Route("api/job-vacancies/{id}/applications")]
[ApiController]
public class JobApplicationsController : ControllerBase
{
    private readonly DevJobsContext _context;

    public JobApplicationsController(DevJobsContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostAsync(int id, AddJobApplicationInputModel model)
    {
        var jobVacancy = _context.JobVacancies.SingleOrDefault(jv => jv.Id == id);

        if (jobVacancy is null) return NotFound();

        var jobApplication = new JobApplication(
            model.ApplicantName, 
            model.ApplicantEmail, model.IdJobVacancy
        );

        jobVacancy.Applications.Add(jobApplication);
        
        return NoContent();
    }
}
