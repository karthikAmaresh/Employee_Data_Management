using Application.Interfaces;
using Infrastructure.Services;
using MediatR;
using Application.Handlers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetEmployeeListHandler).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(AddEmployeeHandler).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UpdateEmployeeHandler).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetEmployeeByIdHandler).GetTypeInfo().Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(DeleteEmployeeHandler).GetTypeInfo().Assembly));

// Add services to the container.
builder.Services.AddSingleton<IEmployeeService>(options =>
{
    var databaseName = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("DatabaseName");
    var containerName = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("ContainerName");
    var account = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("URL");
    var key = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("PrimaryKey");
    var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
    var logger = options.GetRequiredService<ILogger<EmployeeService>>();
    var cosmosDbService = new EmployeeService(client, databaseName, containerName,logger);
    return cosmosDbService;
});
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration.GetSection("ApplicationInsights").GetValue<string>("InstrumentationKey"));



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
