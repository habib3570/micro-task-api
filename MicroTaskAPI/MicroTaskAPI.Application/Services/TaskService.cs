using MicroTaskAPI.Application.DTOs.Task;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<List<TaskResponseDto>> GetAvailableTasksAsync()
        {
            var tasks = await _taskRepository.GetAvailableTasksAsync();
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskResponseDto> GetByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Task", id);
            return MapToDto(task);
        }

        public async Task<List<TaskResponseDto>> GetByBuyerEmailAsync(string buyerEmail)
        {
            var tasks = await _taskRepository.GetByBuyerEmailAsync(buyerEmail);
            return tasks.Select(MapToDto).ToList();
        }

        public async Task<TaskResponseDto> CreateAsync(string buyerEmail, TaskCreateDto dto)
        {
            var buyer = await _userRepository.GetByEmailAsync(buyerEmail)
                ?? throw new EntityNotFoundException("User", buyerEmail);

            var totalCost = dto.RequiredWorkers * dto.PayableAmount;

            if (buyer.Coin < totalCost)
                throw new InsufficientCoinException(buyer.Coin, totalCost);

            buyer.Coin -= totalCost;
            await _userRepository.UpdateAsync(buyer);

            var task = new TaskItem
            {
                TaskTitle = dto.TaskTitle,
                TaskDetail = dto.TaskDetail,
                RequiredWorkers = dto.RequiredWorkers,
                PayableAmount = dto.PayableAmount,
                CompletionDate = dto.CompletionDate,
                SubmissionInfo = dto.SubmissionInfo,
                TaskImageUrl = dto.TaskImageUrl,
                BuyerId = buyer.Id
            };

            await _taskRepository.AddAsync(task);

            task.Buyer = buyer;
            return MapToDto(task);
        }

        public async Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Task", id);

            task.TaskTitle = dto.TaskTitle;
            task.TaskDetail = dto.TaskDetail;
            task.SubmissionInfo = dto.SubmissionInfo;

            await _taskRepository.UpdateAsync(task);

            return MapToDto(task);
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Task", id);

            // Refund remaining coins to buyer (unfilled positions)
            var refundAmount = task.RequiredWorkers * task.PayableAmount;
            if (refundAmount > 0)
            {
                var buyer = await _userRepository.GetByIdAsync(task.BuyerId);
                if (buyer != null)
                {
                    buyer.Coin += refundAmount;
                    await _userRepository.UpdateAsync(buyer);
                }
            }

            await _taskRepository.DeleteAsync(task);
        }

        public async Task<List<TaskResponseDto>> GetAllForAdminAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(MapToDto).ToList();
        }

        private static TaskResponseDto MapToDto(TaskItem task) => new()
        {
            Id = task.Id,
            TaskTitle = task.TaskTitle,
            TaskDetail = task.TaskDetail,
            RequiredWorkers = task.RequiredWorkers,
            PayableAmount = task.PayableAmount,
            CompletionDate = task.CompletionDate,
            SubmissionInfo = task.SubmissionInfo,
            TaskImageUrl = task.TaskImageUrl,
            BuyerEmail = task.Buyer?.Email ?? string.Empty,
            BuyerName = task.Buyer?.DisplayName ?? string.Empty,
            CreatedAt = task.CreatedAt
        };
    }
}