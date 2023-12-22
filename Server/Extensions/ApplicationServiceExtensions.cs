using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Interfaces;
using Server.Services;

namespace Server.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseMySql(config.GetConnectionString("DefaultConnection"),new MySqlServerVersion(new Version(8, 0, 33)));
            });
            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }
    }
}