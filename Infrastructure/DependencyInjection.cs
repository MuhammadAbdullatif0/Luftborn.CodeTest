using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string? connectionString = null)
    {
        services.AddDbContext<StoreDbContext>(options =>
        {
                options.UseSqlServer(connectionString);          
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }

   
}
