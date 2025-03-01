using Microsoft.Azure.Cosmos;
using CRMApp.Repository;
using CRMApp.Services;
using CRMApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

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

// Register CosmosDbService
builder.Services.AddSingleton<ICosmosDbService>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>();
    var databaseName = builder.Configuration["CosmosDb:DatabaseName"];
    var containerName = builder.Configuration["CosmosDb:ContainerName"];
    return new CosmosDbService(cosmosClient, databaseName, containerName);
});

// Register repositories
//builder.Services.AddScoped<CustomerRepository>();
//builder.Services.AddScoped<FeedbackRepository>();
builder.Services.AddScoped<CRMRepository>();
builder.Services.AddScoped<FarmerQuestionnaireRepository>();
builder.Services.AddSingleton<BusinessQuestionnaireRepository>();
builder.Services.AddSingleton<BusinessRegistrationRepository>();
builder.Services.AddSingleton<FarmerRegistrationRepository>();
builder.Services.AddSingleton<ChickenFarmingRepository>();
builder.Services.AddScoped<IChickenRepository, ChickenRepository>();
builder.Services.AddScoped<IBatchService, BatchService>();
// Register IRetailSurveyRepository with a lambda that provides CosmosClient and IConfiguration.
builder.Services.AddScoped<IRetailSurveyRepository>(sp =>
{
    var cosmosClient = sp.GetRequiredService<CosmosClient>();
    var configuration = sp.GetRequiredService<IConfiguration>();
    return new RetailSurveyRepository(cosmosClient, configuration);
});
builder.Services.AddSingleton<IAzureCommunicationService, AzureCommunicationService>();
builder.Services.AddSingleton<IAcsService, AcsService>();
builder.Services.AddScoped<IVetSupportRepository, VetSupportRepository>();
builder.Services.AddScoped<IVetSupportService, VetSupportService>();
builder.Services.Configure<AcsSettings>(builder.Configuration.GetSection("ACS"));

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
// if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();