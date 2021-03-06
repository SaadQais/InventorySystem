using FluentValidation;
using InventorySystem.Application.Behaviours;
using InventorySystem.Application.Contracts.Infrastructure;
using InventorySystem.Application.Contracts.Persistence;
using InventorySystem.Application.Services;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Repositories;
using InventorySystem.Infrastructure.Persistence;
using InventorySystem.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InventorySystem.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.User.RequireUniqueEmail = true;
            })

                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
            });

            services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            services.AddScoped(typeof(IInvoiceRepository), typeof(InvoiceRepository));
            services.AddScoped(typeof(IWarehouseRepository), typeof(WarehouseRepository));
            services.AddScoped<IUserService, UserService>();
        }
    }
}
