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

        if (string.IsNullOrEmpty(chicken.customerId))
        {
            return BadRequest("customerId is required.");
        }

        var json = JsonSerializer.Serialize(chicken);
        Console.WriteLine($"Serialized Chicken: {json}");

        // Ensure id is assigned
        chicken.id = chicken.id ?? Guid.NewGuid().ToString().Kebaberize();
        chicken.Type = "Chicken"; // Document type identifier

        try
        {
            string type = "batch";
            // Fetch the batch using the Batch Service
            if (string.IsNullOrEmpty(chicken.batchId))
            {
                return BadRequest("batchId is required.");
            }

            var batch = await _batchService.GetBatchByIdAsync(type, chicken.batchId);

            if (batch == null)
            {
                // Create a new batch if it doesn't exist
                batch = new Batch
                {
                    id = Guid.NewGuid().ToString().Kebaberize(), // new batchId as the document ID
                    batchId = chicken.batchId,
                    customerId = chicken.customerId,
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
                        Status = chicken.Status,
                        customerId = chicken.customerId
                    }
                }
                };

                // Save the new batch
                await _cosmosDbService.AddItemAsync(batch, batch.customerId);
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
                    Status = chicken.Status,
                    customerId = chicken.customerId
                });
                batch.TotalChickens += 1;

                // Update the batch
                if (batch.customerId == null)
                {
                    return BadRequest("Batch customerId is required.");
                }
                if (batch.customerId == null)
                {
                    return BadRequest("Batch customerId is required.");
                }
                if (batch.customerId == null)
                {
                    return BadRequest("Batch customerId is required.");
                }
                await _cosmosDbService.UpdateItemAsync(batch.id, batch, batch.customerId);
            }

            // Save the chicken as a separate document (if required)
            await _cosmosDbService.AddItemAsync(chicken, chicken.customerId);

            return CreatedAtAction(nameof(GetChickenById), new { id = chicken.id }, chicken);
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.StatusCode}, Message: {ex.Message}");
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetChickenById(string id, [FromQuery] string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            return BadRequest("UserId (partition key) is required.");
        }

        try
        {
            // Fetch the chicken using the provided ID and partition key (UserId)
            var chicken = await _cosmosDbService.GetItemAsync<Chicken>(id, customerId);
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
            await _cosmosDbService.UpdateItemAsync(id, updatedChicken, updatedChicken.customerId);
            return NoContent();
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.StatusCode}, Substatus: {ex.SubStatusCode}, Message: {ex.Message}, ActivityId: {ex.ActivityId}");
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteChicken(string id, [FromQuery] string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            return BadRequest("UserId is required.");
        }

        try
        {
            await _cosmosDbService.DeleteItemAsync(id, customerId);
            return NoContent();
        }
        catch (CosmosException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpPatch("{id}/health")]
    public async Task<IActionResult> UpdateHealthRecord(string id, [FromQuery] string customerId, [FromBody] HealthRecord healthRecord)
    {
        if (string.IsNullOrEmpty(customerId))
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
            var chicken = await _cosmosDbService.GetItemAsync<Chicken>(id, customerId);
            if (chicken == null)
            {
                return NotFound("Chicken not found.");
            }

            // Add the new health record
            healthRecord.Date = DateTime.UtcNow; // Assign current date
            chicken.HealthRecords.Add(healthRecord);

            // Update the chicken in the database
            await _cosmosDbService.UpdateItemAsync(id, chicken, customerId);

            return Ok(chicken);
        }
        catch (CosmosException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetChickensByUserId([FromQuery] string customerId)
    {
        if (string.IsNullOrEmpty(customerId))
        {
            return BadRequest("UserId is required.");
        }

        try
        {
            var chickens = await _chickenRepository.GetChickensByUserIdAsync(customerId);

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
    public async Task<IActionResult> UpdateChickenStatus(string chickenId, [FromQuery] string batchId, [FromQuery] string customerId, [FromBody] string status)
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

    [HttpPatch("{chickenId}/customerid")]
    public async Task<IActionResult> UpdateChickenCustomerId(
        string chickenId,
        [FromQuery] string oldCustomerId,
        [FromQuery] string newCustomerId)
    {
        if (string.IsNullOrEmpty(chickenId) ||
            string.IsNullOrEmpty(oldCustomerId) ||
            string.IsNullOrEmpty(newCustomerId))
        {
            return BadRequest("ChickenId, oldCustomerId, and newCustomerId are required.");
        }

        try
        {
            // Fetch the chicken document using the old customerId as the partition key
            var chicken = await _cosmosDbService.GetItemAsync<Chicken>(chickenId, oldCustomerId);
            if (chicken == null)
            {
                return NotFound("Chicken not found.");
            }

            // Update the chicken document with the new customerId
            chicken.customerId = newCustomerId;
            // Note: Depending on your Cosmos DB implementation, you might need to handle the fact
            // that the partition key (customerId) cannot be updated. This example assumes your service
            // supports re-writing the document with a new partition key.
            // Attempt to add the new document with the updated customerId.
            // If this fails, do not proceed with deletion.
            try
            {
                await _cosmosDbService.UpdateItemAsync(chicken.id, chicken, newCustomerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new chicken document: {ex.Message}");
                return StatusCode(500, "mapping failed");
            }

            // If the chicken is part of a batch, update the corresponding chicken detail record.
            if (!string.IsNullOrEmpty(chicken.batchId))
            {
                // Retrieve the batch using its batchId.
                // (Here we assume the partition key for Batch documents is not the customerId being updated.)
                var batch = await _batchService.GetBatchByIdAsync("batch", chicken.batchId);
                if (batch != null)
                {
                    // Locate the chicken detail in the batch. (This lookup is based on ChickenId.)
                    var chickenDetail = batch.Chickens.FirstOrDefault(cd => cd.ChickenId == chicken.ChickenId);
                    if (chickenDetail != null)
                    {
                        chickenDetail.customerId = newCustomerId;
                        // Update the batch document.
                        await _cosmosDbService.UpdateItemAsync(batch.id, batch, batch.customerId);
                    }
                }
            }

            // Delete the old chicken document to prevent duplicate records.
            await _cosmosDbService.DeleteItemAsync(chickenId, oldCustomerId);

            return Ok(chicken);
        }
        catch (CosmosException ex)
        {
            Console.WriteLine($"Cosmos DB Error: {ex.StatusCode}, Message: {ex.Message}");
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

}