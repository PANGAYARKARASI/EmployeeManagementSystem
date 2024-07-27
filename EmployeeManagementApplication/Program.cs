using EmployeeLibrary.Model;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace EmployeeManagementApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();
            var configuration = builder.Configuration;
            builder.Services.AddDbContext<EmployeeDbContext>(options =>
            {
                //options.UseSqlServer(@"server=(localdb)\MSSQLLocalDB; database=EmployeeDb; integrated security=true");
                options.UseSqlServer(configuration.GetConnectionString("EmployeeDb"));

            });
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();       
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
