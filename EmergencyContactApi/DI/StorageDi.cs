using EmergencyContactApi.DataStorages.InMemory;
using EmergencyContactApi.DataStorages.Interfaces;

namespace EmergencyContactApi.DI
{
    public static class StorageDi
    {
        public static void AddStorage(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeStorage, InMemoryEmployeeStorage>();
        }
    }
}
