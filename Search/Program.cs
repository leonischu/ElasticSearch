using Nest;
using Search.Models;
using Search.Services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

var setting = new ConnectionSettings(new Uri("http://localhost:9200/")).DefaultIndex("esdemo");
var client = new ElasticClient(setting);

builder.Services.AddSingleton(client);

builder.Services.AddScoped<IElasticSearchService<MyDocument>, ElasticSearchService<MyDocument>>();



builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();








var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
