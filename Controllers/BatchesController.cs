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
public class BatchesController : ControllerBase
{
    private readonly ICosmosDbService _cosmosDbService;
    private readonly IChickenRepository _chickenRepository;
    private readonly IBatchService _batchService;

    public BatchesController(ICosmosDbService cosmosDbService, IChickenRepository chickenRepository, IBatchService batchService)
    {
        _cosmosDbService = cosmosDbService;
        _chickenRepository = chickenRepository;
        _batchService = batchService;
    }

    [HttpPost("batches")]
    public async Task<IActionResult> CreateBatch([FromBody] Batch batch)
    {
        if (batch == null)
        {
            return BadRequest("Batch data is required.");
        }

        if (string.IsNullOrEmpty(batch.CustomerId))
        {
            return BadRequest("UserId is required.");
        }

        var createdBatch = await _batchService.CreateBatchAsync(batch);
        return CreatedAtAction(nameof(GetBatchById), new { id = createdBatch.id }, createdBatch);
    }

    [HttpGet("batches")]
    public async Task<IActionResult> GetBatchesByUserId([FromQuery] string CustomerId)
    {
        if (string.IsNullOrEmpty(CustomerId))
        {
            return BadRequest("UserId is required.");
        }

        var batches = await _batchService.GetBatchesByUserIdAsync(CustomerId);
        if (batches == null || !batches.Any())
        {
            return NotFound("No batches found.");
        }

        return Ok(batches);
    }

    [HttpGet("batches/{batchId}/chickens")]
    public async Task<IActionResult> GetChickensByBatchId(string batchId, [FromQuery] string CustomerId)
    {
        if (string.IsNullOrEmpty(CustomerId))
        {
            return BadRequest("UserId is required.");
        }

        var chickens = await _batchService.GetChickensByBatchIdAsync(CustomerId, batchId);
        if (chickens == null || !chickens.Any())
        {
            return NotFound("No chickens found for this batch.");
        }

        return Ok(chickens);
    }

    [HttpGet("batches/{batchId}")]
    public async Task<IActionResult> GetBatchById(string batchId, [FromQuery] string CustomerId)
    {
        if (string.IsNullOrEmpty(CustomerId))
        {
            return BadRequest("UserId is required.");
        }

        try
        {
            // Fetch the batch using batchId and CustomerId
            var batch = await _batchService.GetBatchByIdAsync(CustomerId, batchId);

            if (batch == null)
            {
                return NotFound("Batch not found.");
            }

            return Ok(batch);
        }
        catch (CosmosException ex)
        {
            return StatusCode((int)ex.StatusCode, ex.Message);
        }
    }

    [HttpPatch("batches/{batchId}/feed")]
    public async Task<IActionResult> AddFeedDetails(string batchId, [FromQuery] string CustomerId, [FromBody] FeedDetails feedDetails)
    {
        if (feedDetails == null)
        {
            return BadRequest("Feed details are required.");
        }

        var batch = await _cosmosDbService.GetItemAsync<Batch>(batchId, CustomerId);
        if (batch == null)
        {
            return NotFound("Batch not found.");
        }

        batch.FeedDetails.Add(feedDetails);
        await _cosmosDbService.UpdateItemAsync(batch.id, batch, CustomerId);

        return Ok(batch);
    }

    [HttpPatch("batches/{batchId}/status")]
    public async Task<IActionResult> UpdateBatchStatus(string batchId, [FromQuery] string CustomerId, [FromBody] bool completed)
    {
        var batch = await _cosmosDbService.GetItemAsync<Batch>(batchId, CustomerId);
        if (batch == null)
        {
            return NotFound("Batch not found.");
        }

        batch.Completed = completed;
        batch.Active = !completed;

        await _cosmosDbService.UpdateItemAsync(batch.id, batch, CustomerId);

        return Ok(batch);
    }

}