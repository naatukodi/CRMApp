using System;
using CRMApp.Services;
using CRMApp.Models;

namespace CRMApp.Repository;

public interface IBatchService
{
    Task<List<Batch>> GetBatchesByUserIdAsync(string customerId);
    Task<Batch> CreateBatchAsync(Batch batch);
    Task<List<Chicken>> GetChickensByBatchIdAsync(string customerId, string batchId);
    Task<Batch> GetBatchByIdAsync(string customerId, string batchId);
}
