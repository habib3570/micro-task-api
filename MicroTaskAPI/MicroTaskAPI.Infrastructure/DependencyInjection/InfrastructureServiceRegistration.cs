using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MicroTaskAPI.Application.Interfaces.Repositories;
using MicroTaskAPI.Application.Interfaces.Services;
using MicroTaskAPI.Infrastructure.ExternalServices;
using MicroTaskAPI.Infrastructure.Persistence.Context;
using MicroTaskAPI.Infrastructure.Persistence.Repositories;

namespace MicroTaskAPI.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ISubmissionRepository, SubmissionRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}