using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;

namespace MicroTaskAPI.Application.Interfaces.Repositories
{
    public interface ISubmissionRepository
    {
        Task<Submission?> GetByIdAsync(int id);
        Task<List<Submission>> GetByWorkerEmailAsync(string workerEmail);
        Task<(List<Submission> Items, int TotalCount)> GetByWorkerEmailPagedAsync(string workerEmail, int page, int size);
        Task<List<Submission>> GetPendingByBuyerEmailAsync(string buyerEmail);
        Task<List<Submission>> GetApprovedByWorkerEmailAsync(string workerEmail);
        Task<Submission> AddAsync(Submission submission);
        Task UpdateAsync(Submission submission);
        Task<int> CountByWorkerAsync(int workerId);
        Task<int> CountByWorkerAndStatusAsync(int workerId, SubmissionStatus status);
        Task<int> SumEarningByWorkerAsync(int workerId);
    }
}