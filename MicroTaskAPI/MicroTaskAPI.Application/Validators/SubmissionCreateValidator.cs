using FluentValidation;
using MicroTaskAPI.Application.DTOs.Submission;

namespace MicroTaskAPI.Application.Validators
{
    public class SubmissionCreateValidator : AbstractValidator<SubmissionCreateDto>
    {
        public SubmissionCreateValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("Valid task ID is required.");

            RuleFor(x => x.SubmissionDetail)
                .NotEmpty().WithMessage("Submission detail is required.")
                .MinimumLength(10).WithMessage("Submission detail must be at least 10 characters.");
        }
    }
}