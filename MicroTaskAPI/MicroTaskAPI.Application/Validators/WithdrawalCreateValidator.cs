using FluentValidation;
using MicroTaskAPI.Application.DTOs.Withdrawal;
using MicroTaskAPI.Domain.Constants;

namespace MicroTaskAPI.Application.Validators
{
    public class WithdrawalCreateValidator : AbstractValidator<WithdrawalCreateDto>
    {
        public WithdrawalCreateValidator()
        {
            RuleFor(x => x.WithdrawalCoin)
                .GreaterThanOrEqualTo(CoinConstants.MinimumWithdrawalCoin)
                .WithMessage($"Minimum withdrawal amount is {CoinConstants.MinimumWithdrawalCoin} coins.");

            RuleFor(x => x.PaymentSystem)
                .NotEmpty().WithMessage("Payment system is required.")
                .Must(p => new[] { "Bkash", "Nagad", "Rocket", "Bank" }.Contains(p))
                .WithMessage("Payment system must be Bkash, Nagad, Rocket, or Bank.");

            RuleFor(x => x.AccountNumber)
                .NotEmpty().WithMessage("Account number is required.")
                .MaximumLength(30);
        }
    }
}