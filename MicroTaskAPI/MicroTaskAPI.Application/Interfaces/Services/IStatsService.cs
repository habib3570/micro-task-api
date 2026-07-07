using MicroTaskAPI.Application.DTOs.Stats;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface IStatsService
    {
        Task<AdminStatsDto> GetAdminStatsAsync();
        Task<BuyerStatsDto> GetBuyerStatsAsync(string buyerEmail);
        Task<WorkerStatsDto> GetWorkerStatsAsync(string workerEmail);
    }
}