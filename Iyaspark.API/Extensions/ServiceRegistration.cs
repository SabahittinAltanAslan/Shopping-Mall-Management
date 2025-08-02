using FluentValidation;
using FluentValidation.AspNetCore;
using Iyaspark.Application;
using Iyaspark.Application.Common.Behaviors;
using Iyaspark.Application.Interfaces.Services;
using Iyaspark.Application.Modules.Contracts.Validators;
using Iyaspark.Application.Modules.Users.Handlers;
using Iyaspark.Domain.Interfaces;
using Iyaspark.Infrastructure.Persistence.Repositories;
using Iyaspark.Infrastructure.Repositories;
using Iyaspark.Infrastructure.Services;
using Iyaspark.Persistance.Repositories;
using Iyaspark.Persistence.Context;
using Iyaspark.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Iyaspark.API.Extensions
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // ✅ MediatR
            services.AddMediatR(typeof(LoginUserCommandHandler).Assembly);

            // AutoMapper
            services.AddAutoMapper(typeof(AssemblyReference).Assembly);

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(AssemblyReference).Assembly);
            services.AddControllers().AddFluentValidation();

            // Repositories
            services.AddScoped<ITenantRepository, TenantRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IMonthlyRevenueRepository, MonthlyRevenueRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGuaranteeRepository, GuaranteeRepository>();

            // Services
            services.AddScoped<IExcelExportService, ExcelExportService>();
            services.AddScoped<IJwtService, JwtService>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssemblyContaining<UpdateContractCommandValidator>();

            return services;
        }
    }
}
