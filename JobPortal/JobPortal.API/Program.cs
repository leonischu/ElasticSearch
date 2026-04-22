using JobPortal.Application.Handlers;
using JobPortal.Application.Interfaces;
using JobPortal.Infrastructure.Database;
using JobPortal.Infrastructure.Repository;
using JobPortal.Infrastructure.Services;
using Nest;
using static Dapper.SqlMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// ---------------- Controllers ----------------

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


// Register DapperContext (reads connection string from appsettings.json)
builder.Services.AddSingleton<DapperContext>();


builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped(typeof(IElasticService<>), typeof(ElasticService<>));

builder.Services.AddScoped<CreateJobHandler>();
builder.Services.AddScoped<SearchJobHandler>();



var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
    .DefaultIndex("jobs");


builder.Services.AddSingleton(new ElasticClient(settings));

builder.Services.AddAutoMapper(typeof(Program));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}






app.UseHttpsRedirection();

app.MapControllers();
app.Run();


