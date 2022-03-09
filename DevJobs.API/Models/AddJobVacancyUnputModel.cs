namespace DevJobs.API.Models;

public record AddJobVacancyUnputModel(
    string Title,
    string Description,
    string Company,
    bool IsRemote,
    string SalaryRange)
{

}