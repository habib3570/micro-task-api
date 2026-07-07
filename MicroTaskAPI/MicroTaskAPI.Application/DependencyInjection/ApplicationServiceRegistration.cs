using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Application.Services;

namespace MicroTaskAPI.Application.DependencyInjection
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ISubmissionService, SubmissionService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IWithdrawalService, WithdrawalService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IStatsService, StatsService>();

            services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

            return services;
        }
    }
}