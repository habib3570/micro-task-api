using MicroTaskAPI.Application.Common;
using MicroTaskAPI.Application.DTOs.Submission;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public SubmissionService(
            ISubmissionRepository submissionRepository,
            ITaskRepository taskRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _submissionRepository = submissionRepository;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<SubmissionResponseDto> CreateAsync(string workerEmail, SubmissionCreateDto dto)
        {
            var worker = await _userRepository.GetByEmailAsync(workerEmail)
                ?? throw new EntityNotFoundException("User", workerEmail);

            var task = await _taskRepository.GetByIdAsync(dto.TaskId)
                ?? throw new EntityNotFoundException("Task", dto.TaskId);

            if (task.RequiredWorkers <= 0)
                throw new InvalidSubmissionStatusException("This task has no remaining worker slots.");

            var submission = new Submission
            {
                TaskId = task.Id,
                TaskTitle = task.TaskTitle,
                PayableAmount = task.PayableAmount,
                WorkerId = worker.Id,
                WorkerName = worker.DisplayName,
                BuyerId = task.BuyerId,
                BuyerName = task.Buyer?.DisplayName ?? string.Empty,
                SubmissionDetail = dto.SubmissionDetail,
                Status = SubmissionStatus.Pending
            };

            await _submissionRepository.AddAsync(submission);

            await _notificationService.CreateAsync(
                task.BuyerId,
                $"New submission received for task: {task.TaskTitle}",
                "/dashboard/buyer-home");

            submission.Worker = worker;
            return MapToDto(submission);
        }

        public async Task<List<SubmissionResponseDto>> GetByWorkerEmailAsync(string workerEmail)
        {
            var submissions = await _submissionRepository.GetByWorkerEmailAsync(workerEmail);
            return submissions.Select(MapToDto).ToList();
        }

        public async Task<PaginatedResponse<SubmissionResponseDto>> GetByWorkerEmailPagedAsync(string workerEmail, int page, int size)
        {
            var (items, totalCount) = await _submissionRepository.GetByWorkerEmailPagedAsync(workerEmail, page, size);

            return new PaginatedResponse<SubmissionResponseDto>
            {
                Items = items.Select(MapToDto).ToList(),
                Page = page,
                Size = size,
                TotalCount = totalCount
            };
        }

        public async Task<List<SubmissionResponseDto>> GetPendingByBuyerEmailAsync(string buyerEmail)
        {
            var submissions = await _submissionRepository.GetPendingByBuyerEmailAsync(buyerEmail);
            return submissions.Select(MapToDto).ToList();
        }

        public async Task ApproveAsync(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Submission", id);

            if (submission.Status != SubmissionStatus.Pending)
                throw new InvalidSubmissionStatusException("Only pending submissions can be approved.");

            submission.Status = SubmissionStatus.Approved;
            await _submissionRepository.UpdateAsync(submission);

            // Pay the worker
            var worker = await _userRepository.GetByIdAsync(submission.WorkerId);
            if (worker != null)
            {
                worker.Coin += submission.PayableAmount;
                await _userRepository.UpdateAsync(worker);
            }

            // Decrease remaining required workers on the task
            var task = await _taskRepository.GetByIdAsync(submission.TaskId);
            if (task != null && task.RequiredWorkers > 0)
            {
                task.RequiredWorkers -= 1;
                await _taskRepository.UpdateAsync(task);
            }

            await _notificationService.CreateAsync(
                submission.WorkerId,
                $"Your submission for '{submission.TaskTitle}' was approved! You earned {submission.PayableAmount} coins.",
                "/dashboard/worker-home");
        }

        public async Task RejectAsync(int id)
        {
            var submission = await _submissionRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Submission", id);

            if (submission.Status != SubmissionStatus.Pending)
                throw new InvalidSubmissionStatusException("Only pending submissions can be rejected.");

            submission.Status = SubmissionStatus.Rejected;
            await _submissionRepository.UpdateAsync(submission);

            await _notificationService.CreateAsync(
                submission.WorkerId,
                $"Your submission for '{submission.TaskTitle}' was rejected.",
                "/dashboard/worker-home");
        }

        public async Task<List<SubmissionResponseDto>> GetApprovedByWorkerEmailAsync(string workerEmail)
        {
            var submissions = await _submissionRepository.GetApprovedByWorkerEmailAsync(workerEmail);
            return submissions.Select(MapToDto).ToList();
        }

        public async Task<SubmissionStatsDto> GetWorkerStatsAsync(string workerEmail)
        {
            var worker = await _userRepository.GetByEmailAsync(workerEmail)
                ?? throw new EntityNotFoundException("User", workerEmail);

            var total = await _submissionRepository.CountByWorkerAsync(worker.Id);
            var pending = await _submissionRepository.CountByWorkerAndStatusAsync(worker.Id, SubmissionStatus.Pending);
            var earning = await _submissionRepository.SumEarningByWorkerAsync(worker.Id);

            return new SubmissionStatsDto
            {
                TotalSubmissions = total,
                PendingSubmissions = pending,
                TotalEarning = earning
            };
        }

        private static SubmissionResponseDto MapToDto(Submission submission) => new()
        {
            Id = submission.Id,
            TaskId = submission.TaskId,
            TaskTitle = submission.TaskTitle,
            PayableAmount = submission.PayableAmount,
            WorkerEmail = submission.Worker?.Email ?? string.Empty,
            WorkerName = submission.WorkerName,
            BuyerEmail = submission.Buyer?.Email ?? string.Empty,
            BuyerName = submission.BuyerName,
            SubmissionDetail = submission.SubmissionDetail,
            CurrentDate = submission.CurrentDate,
            Status = submission.Status.ToString()
        };
    }
}