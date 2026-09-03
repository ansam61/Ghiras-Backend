using Ghiras.Application.Interfaces;
using Ghiras.Infrastructure.Persistence;
using Ghiras.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ghiras.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IPlantRepository, PlantRepository>();
            services.AddScoped<IPlantCategoryRepository, PlantCategoryRepository>();
            services.AddScoped<IAIDiagnosisRepository, AIDiagnosisRepository>();
            services.AddScoped<IPlantImageRepository, PlantImageRepository>();

            return services;
        }
    }
}
