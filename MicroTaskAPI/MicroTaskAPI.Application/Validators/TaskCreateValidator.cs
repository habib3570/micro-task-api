using FluentValidation;
using MicroTaskAPI.Application.DTOs.Task;

namespace MicroTaskAPI.Application.Validators
{
    public class TaskCreateValidator : AbstractValidator<TaskCreateDto>
    {
        public TaskCreateValidator()
        {
            RuleFor(x => x.TaskTitle)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(250);

            RuleFor(x => x.TaskDetail)
                .NotEmpty().WithMessage("Task detail is required.");

            RuleFor(x => x.RequiredWorkers)
                .GreaterThan(0).WithMessage("Required workers must be greater than 0.");

            RuleFor(x => x.PayableAmount)
                .GreaterThan(0).WithMessage("Payable amount must be greater than 0.");

            RuleFor(x => x.CompletionDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Completion date must be in the future.");

            RuleFor(x => x.SubmissionInfo)
                .NotEmpty().WithMessage("Submission info is required.");
        }
    }
}