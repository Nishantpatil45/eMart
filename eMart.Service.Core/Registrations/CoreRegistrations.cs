using eMart.Service.Core.Interfaces;
using eMart.Service.Core.Repositories;
using eMart.Service.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationManager = eMart.Service.Core.Helper.ConfigurationManager;

namespace eMart.Service.Core.Registrations
{
    public static class CoreRegistrations
    {
        public static IServiceCollection AddCoreComponents(this IServiceCollection services)
        {
            var connectionString = ConfigurationManager.AppSetting.GetSection("ConnectionStrings:DefaultConnection").Value;

            // Register DB context
            services.AddDbContext<eMartDbContext>(options =>
               options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                   b => b.MigrationsAssembly(typeof(eMartDbContext).Assembly.FullName)));

            // Register Repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();


            return services;
        }
    }
}
