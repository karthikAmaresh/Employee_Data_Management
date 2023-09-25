using Application.Interfaces;
using Infrastructure.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
// Add services to the container.
builder.Services.AddSingleton<IEmployeeService>(options =>
{
    var databaseName = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("DatabaseName");
    var containerName = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("ContainerName");
    var account = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("URL");
    var key = builder.Configuration.GetSection("AzureCosmosDBSettings").GetValue<string>("PrimaryKey");
    var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
    var cosmosDbService = new EmployeeService(client, databaseName, containerName);
    return cosmosDbService;
});



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
