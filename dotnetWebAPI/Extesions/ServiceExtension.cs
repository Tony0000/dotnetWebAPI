using System;
using Data;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetWebAPI.Extesions
{
    public static class ServiceExtension
    {
        public static void ConfigureCors(this IServiceCollection services,
            IConfiguration config)
        {
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("policy",
            //        builder => builder.WithOrigins(config["AllowedOrigin"])
            //            .AllowAnyHeader()
            //            .AllowAnyMethod()
            //            .AllowCredentials()
            //        );
            //});
        }

        public static void ConfigureSqlContext(this IServiceCollection services, 
            IConfiguration config)
        {
            var connString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");

            if (string.IsNullOrEmpty(connString))
                connString = config.GetConnectionString("WebApiConnection");

            var assemblyName = typeof(WebApiDbContext).Namespace;
            services.AddDbContext<WebApiDbContext>(
                opt => opt.UseSqlServer(connString, b => b.MigrationsAssembly(assemblyName))
            );
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
        }

        public static void ConfigureActionFilters(this IServiceCollection services)
        {

        }

        public static void ConfigureAuthentication(this IServiceCollection services)
        {

        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            //services.AddAuthorization(config =>
            //{
                
            //    config.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //});
        }
    }
}
