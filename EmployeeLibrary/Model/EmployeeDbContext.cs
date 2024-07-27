using Microsoft.EntityFrameworkCore;

namespace EmployeeLibrary.Model
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext()
        {

        }
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {

        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Payroll> Payrolls { get; set; }

    }
}