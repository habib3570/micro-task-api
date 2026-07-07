using MicroTaskAPI.Application.DTOs.Task;

namespace MicroTaskAPI.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<List<TaskResponseDto>> GetAvailableTasksAsync();
        Task<TaskResponseDto> GetByIdAsync(int id);
        Task<List<TaskResponseDto>> GetByBuyerEmailAsync(string buyerEmail);
        Task<TaskResponseDto> CreateAsync(string buyerEmail, TaskCreateDto dto);
        Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto);
        Task DeleteAsync(int id);
        Task<List<TaskResponseDto>> GetAllForAdminAsync();
    }
}