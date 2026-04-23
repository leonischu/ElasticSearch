using Elasticsearch.Net;
using JobPortal.API.Mapper;
using JobPortal.Application.Handlers;
using JobPortal.Application.Interfaces;
using JobPortal.Infrastructure.Database;
using JobPortal.Infrastructure.Messaging;
using JobPortal.Infrastructure.Repository;
using JobPortal.Infrastructure.Services;
using JobPortal.Infrastructure.Workers;
using Nest;
using System.Reflection;


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

builder.Services.AddScoped<UpdateJobHandler>();

//here
var settings = new ConnectionSettings(
    cloudId: "id",
    credentials: new BasicAuthenticationCredentials(
        "elastic",
        "password"
    )
).DefaultIndex("jobs").DisableDirectStreaming(); 

builder.Services.AddSingleton(new ElasticClient(settings));

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);













var client = new ElasticClient(settings);

//Create INDEX if it does not exist
var exists = await client.Indices.ExistsAsync("jobs");
if (!exists.Exists)
{
    var createResponse = await client.Indices.CreateAsync("jobs", c => c
        .Map(m => m.AutoMap())
    );

    Console.WriteLine("Index created: " + createResponse.IsValid);
}






// register in DI
builder.Services.AddSingleton(client);








var assemblyToScan = typeof(CreateJobHandler).Assembly;
try
{
    var types = assemblyToScan.GetTypes();
    Console.WriteLine($"Loaded {types.Length} types OK");
}
catch (ReflectionTypeLoadException ex)
{
    foreach (var le in ex.LoaderExceptions)
        Console.WriteLine("LOADER EX: " + le?.Message);
}

builder.Services.AddAutoMapper(assemblyToScan);


var kafkaSettings = new KafkaSetting();

builder.Services.AddSingleton(kafkaSettings);
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

builder.Services.AddHostedService<KafkaConsumerWorker>();







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


