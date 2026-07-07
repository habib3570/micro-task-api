using MicroTaskAPI.Application.Common;
using MicroTaskAPI.Application.DTOs.Submission;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface ISubmissionService
    {
        Task<SubmissionResponseDto> CreateAsync(string workerEmail, SubmissionCreateDto dto);
        Task<List<SubmissionResponseDto>> GetByWorkerEmailAsync(string workerEmail);
        Task<PaginatedResponse<SubmissionResponseDto>> GetByWorkerEmailPagedAsync(string workerEmail, int page, int size);
        Task<List<SubmissionResponseDto>> GetPendingByBuyerEmailAsync(string buyerEmail);
        Task ApproveAsync(int id);
        Task RejectAsync(int id);
        Task<List<SubmissionResponseDto>> GetApprovedByWorkerEmailAsync(string workerEmail);
        Task<SubmissionStatsDto> GetWorkerStatsAsync(string workerEmail);
    }
}