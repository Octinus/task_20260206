using EmergencyContactApi.DataStorages.InMemory;
using EmergencyContactApi.DataStorages.Interfaces;
using EmergencyContactApi.Services.Interfaces.Employees;
using EmergencyContactApi.Services.ServiceImpls.Employees;
using System.Reflection;

namespace EmergencyContactApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });

            builder.Services.AddScoped<IEmployeeStorage, InMemoryEmployeeStorage>();
            builder.Services.AddScoped<IRegisterService, RegisterImpl>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
