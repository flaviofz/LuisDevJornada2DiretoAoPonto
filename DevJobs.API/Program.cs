using DevJobs.API.Persistence;
using DevJobs.API.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Comando para executar na linha de comando - Faz com que não precise colocar no appSettings para não ficar exposto
// dotnet user-secrets init
// dotnet user-secrets set "ConnectionStrings:DevJobsCs" "Data source=CODE-DEV-17; Initial Catalog=DevJobs; Integrated Security=true; pooling=true; Trusted_Connection=True"
var connectionString = builder.Configuration.GetConnectionString("DevJobsCs");

builder.Services.AddDbContext<DevJobsContext>(options =>
    // options.UseInMemoryDatabase("DevJobs")
    options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<IJobVacancyRepository, JobVacancyRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => {
    x.SwaggerDoc("v1", new OpenApiInfo {
        Title = "DevJobs.API",
        Version = "v1",
        Contact = new OpenApiContact {
            Name = "Flavio",
            Email = "meuemail@gmail.com",
            Url = new Uri("https://meusite.com.br")
        }
    });

    var xmlFile = "DevJobs.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
});

builder.Host.ConfigureAppConfiguration((hostingContext, config) => {
    Serilog.Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.MSSqlServer(
            connectionString, 
            sinkOptions: new MSSqlServerSinkOptions() {
                AutoCreateSqlTable = true,
                TableName = "Logs"
            })
        // .WriteTo.Console()
        .CreateLogger();
}).UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
