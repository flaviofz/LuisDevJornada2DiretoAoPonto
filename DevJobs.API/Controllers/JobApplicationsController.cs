using Microsoft.AspNetCore.Mvc;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Entities;
using DevJobs.API.Persistence.Repositories;

namespace DevJobs.API.Controllers;

[Route("api/job-vacancies/{id}/applications")]
[ApiController]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobVacancyRepository _repository;

    public JobApplicationsController(IJobVacancyRepository repository)
    {
        _repository = repository;
    }
    
    [HttpPost]
    public IActionResult PostAsync(int id, AddJobApplicationInputModel model)
    {
        var jobVacancy = _repository.GetById(id);

        if (jobVacancy is null) return NotFound();

        var jobApplication = new JobApplication(
            model.ApplicantName, 
            model.ApplicantEmail, 
            model.IdJobVacancy
        );

        _repository.AddApplication(jobApplication);
        
        return NoContent();
    }
}
