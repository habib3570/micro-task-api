using FluentValidation;
using MicroTaskAPI.Application.DTOs.Payment;

namespace MicroTaskAPI.Application.Validators
{
    public class PaymentCreateValidator : AbstractValidator<PaymentCreateDto>
    {
        public PaymentCreateValidator()
        {
            RuleFor(x => x.CoinPurchased)
                .GreaterThan(0).WithMessage("Coin purchased must be greater than 0.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required.")
                .Must(p => new[] { "Bkash", "Nagad", "Rocket", "Bank" }.Contains(p))
                .WithMessage("Payment method must be Bkash, Nagad, Rocket, or Bank.");

            RuleFor(x => x.SenderNumber)
                .NotEmpty().WithMessage("Sender number is required.");

            RuleFor(x => x.TransactionId)
                .NotEmpty().WithMessage("Transaction ID is required.");
        }
    }
}