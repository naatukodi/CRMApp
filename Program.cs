using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CRMApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Cosmos DB client as a service
builder.Services.AddSingleton<CosmosClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var endpoint = configuration["CosmosDb:AccountEndpoint"];
    var key = configuration["CosmosDb:AccountKey"];
    return new CosmosClient(endpoint, key);
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
    var databaseName = configuration["CosmosDb:DatabaseName"];
    var containerName = configuration["CosmosDb:ContainerName"];
    var partitionKeyPath = configuration["CosmosDb:PartitionKeyPath"];

    var database = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName).Result.Database;
    database.CreateContainerIfNotExistsAsync(containerName, partitionKeyPath).Wait();
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