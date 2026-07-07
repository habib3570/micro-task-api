using MicroTaskAPI.Application.DTOs.Withdrawal;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Domain.Constants;
using MicroTaskAPI.Domain.Entities;
using MicroTaskAPI.Domain.Enums;
using MicroTaskAPI.Domain.Exceptions;

namespace MicroTaskAPI.Application.Services
{
    public class WithdrawalService : IWithdrawalService
    {
        private readonly IWithdrawalRepository _withdrawalRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public WithdrawalService(
            IWithdrawalRepository withdrawalRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _withdrawalRepository = withdrawalRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        public async Task<WithdrawalResponseDto> CreateAsync(string workerEmail, WithdrawalCreateDto dto)
        {
            var worker = await _userRepository.GetByEmailAsync(workerEmail)
                ?? throw new EntityNotFoundException("User", workerEmail);

            if (dto.WithdrawalCoin < CoinConstants.MinimumWithdrawalCoin)
                throw new InvalidWithdrawalException(
                    $"Minimum withdrawal amount is {CoinConstants.MinimumWithdrawalCoin} coins.");

            if (worker.Coin < dto.WithdrawalCoin)
                throw new InsufficientCoinException(worker.Coin, dto.WithdrawalCoin);

            var withdrawal = new Withdrawal
            {
                WorkerId = worker.Id,
                WorkerName = worker.DisplayName,
                WithdrawalCoin = dto.WithdrawalCoin,
                WithdrawalAmount = (decimal)dto.WithdrawalCoin / CoinConstants.CoinToDollarRate,
                PaymentSystem = dto.PaymentSystem,
                AccountNumber = dto.AccountNumber,
                Status = WithdrawalStatus.Pending
            };

            // Hold the coins immediately so they can't be double-spent/withdrawn twice
            worker.Coin -= dto.WithdrawalCoin;
            await _userRepository.UpdateAsync(worker);

            await _withdrawalRepository.AddAsync(withdrawal);

            withdrawal.Worker = worker;
            return MapToDto(withdrawal);
        }

        public async Task<List<WithdrawalResponseDto>> GetPendingAsync()
        {
            var withdrawals = await _withdrawalRepository.GetPendingAsync();
            return withdrawals.Select(MapToDto).ToList();
        }

        public async Task ApproveAsync(int id)
        {
            var withdrawal = await _withdrawalRepository.GetByIdAsync(id)
                ?? throw new EntityNotFoundException("Withdrawal", id);

            if (withdrawal.Status != WithdrawalStatus.Pending)
                throw new InvalidSubmissionStatusException("Only pending withdrawals can be approved.");

            withdrawal.Status = WithdrawalStatus.Approved;
            await _withdrawalRepository.UpdateAsync(withdrawal);

            await _notificationService.CreateAsync(
                withdrawal.WorkerId,
                $"Your withdrawal request of {withdrawal.WithdrawalCoin} coins (${withdrawal.WithdrawalAmount}) was approved.",
                "/dashboard/worker-home");
        }

        public async Task<List<WithdrawalResponseDto>> GetByWorkerEmailAsync(string workerEmail)
        {
            var withdrawals = await _withdrawalRepository.GetByWorkerEmailAsync(workerEmail);
            return withdrawals.Select(MapToDto).ToList();
        }

        private static WithdrawalResponseDto MapToDto(Withdrawal withdrawal) => new()
        {
            Id = withdrawal.Id,
            WorkerEmail = withdrawal.Worker?.Email ?? string.Empty,
            WorkerName = withdrawal.WorkerName,
            WithdrawalCoin = withdrawal.WithdrawalCoin,
            WithdrawalAmount = withdrawal.WithdrawalAmount,
            PaymentSystem = withdrawal.PaymentSystem,
            AccountNumber = withdrawal.AccountNumber,
            WithdrawDate = withdrawal.WithdrawDate,
            Status = withdrawal.Status.ToString()
        };
    }
}