using Application.Common.Interfaces;
using Application.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, 
            IConfiguration configuration)
        {
            var connString = configuration.GetConnectionString("WebApiConnection");

            var assemblyName = typeof(WebApiDbContext).Namespace;
            services.AddDbContext<WebApiDbContext>(
                opt => opt.UseSqlServer(connString, b => b.MigrationsAssembly(assemblyName))
            );

            services.AddScoped<IWebApiDbContext>(provider => provider.GetService<WebApiDbContext>());

            services.ConfigureRepositories();
            
            return services;
        }
        
        private static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        }

    }
}
