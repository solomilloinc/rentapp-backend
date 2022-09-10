using rentapp.Data;
using rentapp.Data.Repositories;
using rentapp.Data.Repositories.Interfaces;
using rentapp.Service.Services;
using rentapp.Service.Services.Interfaces;

namespace rentapp.backend.Configuration
{
    public static class ServicesExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region repositories

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            #endregion

            #region services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJwtUtils, JwtUtils>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IHttpContextService, HttpContextService>();

            #endregion
        }
    }
}
