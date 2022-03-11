using Microsoft.AspNetCore.Mvc;
using DevJobs.API.Models;
using DevJobs.API.Persistence;
using DevJobs.API.Entities;
using Microsoft.EntityFrameworkCore;
using DevJobs.API.Persistence.Repositories;
using Serilog;

namespace DevJobs.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobVacanciesController : ControllerBase
{
    private readonly IJobVacancyRepository _repository;
    
    public JobVacanciesController(IJobVacancyRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var jobVacancies = _repository.GetAll();
        return Ok(jobVacancies);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var jobVacancy = _repository.GetById(id);

        if (jobVacancy is null) return NotFound();

        return Ok(jobVacancy);
    }

    /// <summary>
    /// Cadastrar uma vaga de emprego
    /// </summary>
    /// <remarks>
    /// {
    ///  "title": "Titulo",
    ///  "description": "Descrição da vaga",
    ///  "company": "Empresa",
    ///  "isRemote": true,
    ///  "salaryRange": "3.000,00 - 5.000,00"
    /// }
    /// </remarks>
    /// <param name="model">Dados da vaga</param>
    /// <returns>Objeto criado</returns>
    /// <response code ="201">Sucesso</response>
    /// <response code ="400">Dados inválidos</response>
    [HttpPost]
    public async Task<IActionResult> Post(AddJobVacancyInputModel model)
    {
        Log.Information("POST JobVacancy chamado");
        
        var jobVacancy = new JobVacancy(
            model.Title,
            model.Description,
            model.Company,
            model.IsRemote,
            model.SalaryRange
        );

        _repository.Add(jobVacancy);

        return CreatedAtAction(
            "GetById", 
            new {jobVacancy.Id},
            jobVacancy);
    }

    [HttpPut("{id:int}")]
    public IActionResult Put(int id, UpdateJobVacancyUnputModel model)
    {
        var jobVacancy = _repository.GetById(id);

        if (jobVacancy is null) return NotFound();

        jobVacancy.Update(model.Title, model.Description);
        // _context.SaveChanges();
        _repository.Update(jobVacancy);

        return NoContent();
    }

    // [HttpDelete("{id:int}")]
    // public async Task<IActionResult> Delete()
    // {
    //     return NoContent();
    // }
}
