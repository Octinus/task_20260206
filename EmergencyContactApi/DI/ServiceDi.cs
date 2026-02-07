using EmergencyContactApi.Services.Interfaces.Employees;
using EmergencyContactApi.Services.ServiceImpls.Employees;

namespace EmergencyContactApi.DI
{
    public static class ServiceDi
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRegisterService, RegisterImpl>();
            services.AddScoped<ISearchService, SearchImpl>();
        }
    }
}
