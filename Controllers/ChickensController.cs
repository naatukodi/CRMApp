using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;
using System;
using Humanizer;
using System.Text.Json;
using CRMApp.Repository;
using CRMApp.Models;

namespace CRMApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChickensController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IChickenRepository _chickenRepository;
    private readonly IBatchService _batchService;

    public ChickensController(ICosmosDbService cosmosDbService, IChickenRepository chickenRepository, IBatchService batchService)
    {
        _cosmosDbService = cosmosDbService;
        _chickenRepository = chickenRepository;
        _batchService = batchService;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterChicken([FromBody] Chicken chicken)
    {
        if (chicken == null)
        {
            return BadRequest("Chicken data is required.");
        }

        if (string.IsNullOrEmpty(chicken.CustomerId))
        {
            return BadRequest("CustomerId is required.");
        }

        var json = JsonSerializer.Serialize(chicken);
        Console.WriteLine($"Serialized Chicken: {json}");

        // Ensure id is assigned
        chicken.id = chicken.id ?? Guid.NewGuid().ToString().Kebaberize();

        try
        {
            string type = "batch";
            // Fetch the batch using the Batch Service
            var batch = await _batchService.GetBatchByIdAsync(type, chicken.CustomerId);

            if (batch == null)
            {
                // Create a new batch if it doesn't exist
                batch = new Batch
                {
                    id = Guid.NewGuid().ToString().Kebaberize(), // new batchId as the document ID
                    batchId = chicken.batchId,
                    CustomerId = chicken.CustomerId,
                    DateCreated = DateTime.UtcNow,
                    TotalChickens = 1,
                    Chickens = new List<ChickenDetails>
                {
                    new ChickenDetails
                    {
                        id = chicken.id,
                        ChickenId = chicken.ChickenId,
                        Name = chicken.Name,
                        Breed = chicken.Breed,
                        DateOfBirth = chicken.DateOfBirth,
                        Status = "alive",
                        CustomerId = chicken.CustomerId
                    }
                }
                };

                // Save the new batch
                await _cosmosDbService.AddItemAsync(batch, batch.CustomerId);
            }
            else
            {
                // Check if the chicken is already in the batch
                var existingChicken = batch.Chickens.FirstOrDefault(c => c.ChickenId == chicken.ChickenId);
                if (existingChicken != null)
                {
                    // Chicken is already in the batch, do nothing
                    return Ok($"Chicken with ID {chicken.ChickenId} is already in the batch.");
                }

                // Add the new chicken to the batch and increment totalChickens
                batch.Chickens.Add(new ChickenDetails
                {
                    id = chicken.id,
                    ChickenId = chicken.ChickenId,
                    Name = chicken.Name,
                    Breed = chicken.Breed,
                    DateOfBirth = chicken.DateOfBirth,
                    Status = "alive",
                    CustomerId = chicken.CustomerId
                });
                batch.TotalChickens += 1;

                // Update the batch
                await _cosmosDbService.UpdateItemAsync(batch.id, batch, batch.CustomerId);
            }

            // Save the chicken as a separate document (if required)
            await _cosmosDbService.AddItemAsync(chicken, chicken.CustomerId);

            return CreatedAtAction(nameof(GetChickenById), new { id = chicken.id }, chicken);
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.StatusCode}, Message: {ex.Message}");
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChickenById(string id, [FromQuery] string CustomerId)
    {
        if (string.IsNullOrEmpty(CustomerId))
        {
            return BadRequest("UserId (partition key) is required.");
        }

        try
        {
            // Fetch the chicken using the provided ID and partition key (UserId)
            var chicken = await _cosmosDbService.GetItemAsync<Chicken>(id, CustomerId);
            if (chicken == null)
            {
                return NotFound("Chicken not found.");
            }

            return Ok(chicken);
        }
        catch (CosmosException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateChicken(string id, [FromBody] Chicken updatedChicken)
    {
        if (updatedChicken == null || id != updatedChicken.id)
        {
            return BadRequest("Invalid chicken data.");
        }

        try
        {
            await _cosmosDbService.UpdateItemAsync(id, updatedChicken, updatedChicken.CustomerId);
            return NoContent();
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.StatusCode}, Substatus: {ex.SubStatusCode}, Message: {ex.Message}, ActivityId: {ex.ActivityId}");
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChicken(string id, [FromQuery] string CustomerId)
    {
        if (string.IsNullOrEmpty(CustomerId))
        {
            return BadRequest("UserId is required.");
        }

        try
        {
            await _cosmosDbService.DeleteItemAsync(id, CustomerId);
            return NoContent();
        }
        catch (CosmosException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpPatch("{id}/health")]
    public async Task<IActionResult> UpdateHealthRecord(string id, [FromQuery] string CustomerId, [FromBody] HealthRecord healthRecord)
    {
        if (string.IsNullOrEmpty(CustomerId))
        {
            return BadRequest("UserId is required.");
        }

        if (healthRecord == null)
        {
            return BadRequest("Health record data is required.");
        }

        try
        {
            // Fetch the chicken using the provided ID and UserId
            var chicken = await _cosmosDbService.GetItemAsync<Chicken>(id, CustomerId);
            if (chicken == null)
            {
                return NotFound("Chicken not found.");
            }

            // Add the new health record
            healthRecord.Date = DateTime.UtcNow; // Assign current date
            chicken.HealthRecords.Add(healthRecord);

            // Update the chicken in the database
            await _cosmosDbService.UpdateItemAsync(id, chicken, CustomerId);

            return Ok(chicken);
        }
        catch (CosmosException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetChickensByUserId([FromQuery] string CustomerId)
    {
        if (string.IsNullOrEmpty(CustomerId))
        {
            return BadRequest("UserId is required.");
        }

        try
        {
            var chickens = await _chickenRepository.GetChickensByUserIdAsync(CustomerId);

            if (chickens == null || !chickens.Any())
            {
                return NotFound("No chickens found for the specified UserId.");
            }

            return Ok(chickens);
        }
        catch (CosmosException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpPatch("chickens/{chickenId}/status")]
    public async Task<IActionResult> UpdateChickenStatus(string chickenId, [FromQuery] string batchId, [FromQuery] string CustomerId, [FromBody] string status)
    {
        if (string.IsNullOrEmpty(status) || !(status == "alive" || status == "processed" || status == "deceased"))
        {
            return BadRequest("Invalid status value. Must be 'alive', 'processed', or 'deceased'.");
        }

        var chicken = await _cosmosDbService.GetItemAsync<Chicken>(chickenId, batchId);
        if (chicken == null)
        {
            return NotFound("Chicken not found.");
        }

        chicken.Status = status;
        await _cosmosDbService.UpdateItemAsync(chicken.id, chicken, batchId);

        return Ok(chicken);
    }

}