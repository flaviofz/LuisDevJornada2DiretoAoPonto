using Microsoft.AspNetCore.Mvc;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Entities;

namespace DevJobs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobVacanciesController : ControllerBase
{
    private readonly DevJobsContext _context;
    
    public JobVacanciesController(DevJobsContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var jobVacancies = _context.JobVacancies;
        return Ok(jobVacancies);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var jobVacancy = _context.JobVacancies.SingleOrDefault(jv => jv.Id == id);

        if (jobVacancy is null) return NotFound();

        return Ok(jobVacancy);
    }

    [HttpPost]
    public async Task<IActionResult> Post(AddJobVacancyUnputModel model)
    {
        var jobVacancy = new JobVacancy(
            model.Title,
            model.Description,
            model.Company,
            model.IsRemote,
            model.SalaryRange
        );

        _context.JobVacancies.Add(jobVacancy);

        return CreatedAtAction(
            "GetById", 
            new {jobVacancy.Id},
            jobVacancy);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, UpdateJobVacancyUnputModel model)
    {
        var jobVacancy = _context.JobVacancies.SingleOrDefault(jv => jv.Id == id);

        if (jobVacancy is null) return NotFound();

        jobVacancy.Update(model.Title, model.Description);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete()
    {
        return NoContent();
    }
}
