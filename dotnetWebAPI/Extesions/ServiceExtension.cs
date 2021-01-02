using System;
using System.Text;
using Data;
using Data.Helper;
using Data.Repositories;
using Data.Repositories.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebAPI.ActionFilters;

namespace WebAPI.Extesions
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
            services.AddScoped<ModelValidationAttribute>();

            services.AddScoped<NotFoundAttribute<User>>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });
        }

        public static void ConfigureAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(config =>
            {
                config.AddPolicy(Policies.Admin, Policies.AdminPolicy());
                config.AddPolicy(Policies.User, Policies.UserPolicy());
                config.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });
        }
    }
}
