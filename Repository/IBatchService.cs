using System;
using CRMApp.Services;
using CRMApp.Models;

namespace CRMApp.Repository;

public interface IBatchService
{
    Task<List<Batch>> GetBatchesByUserIdAsync(string CustomerId);
    Task<Batch> CreateBatchAsync(Batch batch);
    Task<List<Chicken>> GetChickensByBatchIdAsync(string CustomerId, string batchId);
    Task<Batch> GetBatchByIdAsync(string CustomerId, string batchId);
}
