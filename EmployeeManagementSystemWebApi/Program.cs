
using EmployeeLibrary.Model;
using EmployeeLibrary.Repo;
using EmployeeLibrary.Repository;
using Microsoft.EntityFrameworkCore;


namespace EmployeeManagementSystemWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<EmployeeDbContext>(options =>
            {
                //options.UseSqlServer(@"server=(localdb)\MSSQLLocalDB; database=EmployeeDb; integrated security=true");
                options.UseSqlServer(configuration.GetConnectionString("EmployeeDb"));

            });
            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
