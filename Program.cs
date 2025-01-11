using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CRMApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Cosmos DB client
builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new CosmosClient(configuration["CosmosDb:ConnectionString"]);
});
// Register repositories
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<FeedbackRepository>();
builder.Services.AddScoped<CRMRepository>();

var app = builder.Build();

// Initialize Cosmos DB
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var configuration = services.GetRequiredService<IConfiguration>();
    var cosmosClient = services.GetRequiredService<CosmosClient>();
    var database = cosmosClient.CreateDatabaseIfNotExistsAsync(configuration["CosmosDb:DatabaseName"]).Result.Database;
    database.CreateContainerIfNotExistsAsync(configuration["CosmosDb:ContainerName"], "/CustomerId").Wait();
}

// Use Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();