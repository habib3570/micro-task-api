using MicroTaskAPI.Application.DTOs.Withdrawal;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface IWithdrawalService
    {
        Task<WithdrawalResponseDto> CreateAsync(string workerEmail, WithdrawalCreateDto dto);
        Task<List<WithdrawalResponseDto>> GetPendingAsync();
        Task ApproveAsync(int id);
        Task RejectAsync(int id, WithdrawalRejectDto dto);
        Task<List<WithdrawalResponseDto>> GetByWorkerEmailAsync(string workerEmail);
    }
}