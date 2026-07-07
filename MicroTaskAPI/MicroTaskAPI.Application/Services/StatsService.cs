using MicroTaskAPI.Application.DTOs.Stats;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class StatsService : IStatsService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IPaymentRepository _paymentRepository;

        public StatsService(
            IUserRepository userRepository,
            ITaskRepository taskRepository,
            ISubmissionRepository submissionRepository,
            IPaymentRepository paymentRepository)
        {
            _userRepository = userRepository;
            _taskRepository = taskRepository;
            _submissionRepository = submissionRepository;
            _paymentRepository = paymentRepository;
        }

        public async Task<AdminStatsDto> GetAdminStatsAsync()
        {
            var totalWorkers = await _userRepository.CountByRoleAsync(UserRole.Worker);
            var totalBuyers = await _userRepository.CountByRoleAsync(UserRole.Buyer);
            var totalCoins = await _userRepository.SumAllCoinsAsync();
            var totalPayments = await _paymentRepository.GetTotalPaymentsAsync();

            return new AdminStatsDto
            {
                TotalWorkers = totalWorkers,
                TotalBuyers = totalBuyers,
                TotalCoins = totalCoins,
                TotalPayments = totalPayments
            };
        }

        public async Task<BuyerStatsDto> GetBuyerStatsAsync(string buyerEmail)
        {
            var buyer = await _userRepository.GetByEmailAsync(buyerEmail)
                ?? throw new EntityNotFoundException("User", buyerEmail);

            var taskCount = await _taskRepository.CountByBuyerAsync(buyer.Id);
            var pendingTasks = await _taskRepository.CountPendingByBuyerAsync(buyer.Id);

            var payments = await _paymentRepository.GetByBuyerEmailAsync(buyerEmail);
            var totalPaid = payments.Where(p => p.Status == PaymentStatus.Success).Sum(p => p.Amount);

            return new BuyerStatsDto
            {
                TaskCount = taskCount,
                PendingTasks = pendingTasks,
                TotalPaid = totalPaid
            };
        }

        public async Task<WorkerStatsDto> GetWorkerStatsAsync(string workerEmail)
        {
            var worker = await _userRepository.GetByEmailAsync(workerEmail)
                ?? throw new EntityNotFoundException("User", workerEmail);

            var total = await _submissionRepository.CountByWorkerAsync(worker.Id);
            var pending = await _submissionRepository.CountByWorkerAndStatusAsync(worker.Id, SubmissionStatus.Pending);
            var earning = await _submissionRepository.SumEarningByWorkerAsync(worker.Id);

            return new WorkerStatsDto
            {
                TotalSubmissions = total,
                PendingSubmissions = pending,
                TotalEarning = earning
            };
        }
    }
}