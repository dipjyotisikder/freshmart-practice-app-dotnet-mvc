using FreshMart.Database;
using FreshMart.Models;
using FreshMart.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using MediatR;
using FreshMart.Models.Queries;
using FreshMart.Services.QueryHandler;
using FreshMart.Core.Options;
using FreshMart.Services.Factories;

namespace FreshMart.Extensions
{
    public static class ServiceInstallerExtension
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<StorageOptions>(configuration.GetSection(nameof(StorageOptions)));

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IEncryptionServices, EncryptionServices>();
            services.AddScoped<IFileServiceFactory, FileServiceFactory>();
            services.AddScoped<IFileService, LocalFileService>();
            services.AddScoped<IFileService, AmazonS3FileService>();

            //for session
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ICartService, CartService>();

            services.AddDistributedMemoryCache();

            services.AddMediatR(typeof(ProductQueryHandler));


            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(600);
                options.Cookie.HttpOnly = true;
            });
            //for session

            services.AddSession();
            services.AddMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



            return services;
        }

    }
}
