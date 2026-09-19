

using Claims.DataAccess.Implementations;
using InsuranceApp.Auditing;
using InsuranceApp.Auditing.Auditing;
using InsuranceApp.DataAccess;
using InsuranceApp.DataAccess.Implementations;
using InsuranceApp.DataAccess.Interfaces;
using InsuranceApp.Services.Implementations;
using InsuranceApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.Helper
{
    public static class DependencyInjectionHelper
    {
        public static void InjectDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<InsuranceAppDbContext>(x =>
                x.UseSqlServer(connectionString));
        }

        public static void InjectRepository(IServiceCollection services)
        {
            services.AddTransient<IClaimRepository, ClaimRepository>();
            services.AddTransient<ICoverRepository, CoverRepository>();
        }

        public static void InjectServices(IServiceCollection services)
        {
            services.AddTransient<IClaimService, ClaimService>();
            services.AddTransient<ICoverService, CoverService>();
        }

        public static void InjectAuditing(IServiceCollection services)
        {
            services.AddTransient<IAuditer, Auditer>();
        }
    }
}
