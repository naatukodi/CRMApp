using System;
using CRMApp.Models;

namespace CRMApp.Repository
{
    public interface IChickenRepository
    {
        Task<List<Chicken>> GetChickensByUserIdAsync(string customerId);

    }
}

