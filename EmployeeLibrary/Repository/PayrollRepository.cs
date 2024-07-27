using EmployeeLibrary.Model;
using EmployeeLibrary.Repo;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLibrary.Repository
{
    public class PayrollRepository : IPayrollRepository
    {
        EmployeeDbContext dbContext;
        IUserRoleRepository repo;
        IAttendanceRepository repo1;
        IRoleRepository repo2;
        IUserRepository repo3;
        public PayrollRepository(EmployeeDbContext payrollRepo, IUserRoleRepository repository, IAttendanceRepository repos, IRoleRepository repository1, IUserRepository repository2)
        {
            dbContext = payrollRepo;
            repo = repository;
            repo1 = repos;
            repo2 = repository1;
            repo3 = repository2;
        }
        public async Task DeletePayroll(int payrollid)
        {
            try
            {
                Payroll payrolltodelete = await GetPayrollById(payrollid);
                dbContext.Payrolls.Remove(payrolltodelete);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Payroll>> GetAllPayroll()
        {
            try
            {
                List<Payroll> payrolls = await dbContext.Payrolls.Include(role => role.UserRole).Include(role => role.UserRole.User).Include(role => role.UserRole.Role).ToListAsync();
                return payrolls;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Payroll>> GetAllPayrollGroupedByMonthAndUserId(DateTime year)
        {
            try
            {
                List<Payroll> payroll = await (from u in dbContext.Payrolls.Include(role => role.UserRole).Include(role => role.UserRole.User)
                                               where u.Date.Year == year.Year
                                               select u).ToListAsync();
                return payroll;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Payroll>> GetByRole(int roleid)
        {
            try
            {
                List<Payroll> userrole = await (from u in dbContext.Payrolls.Include(role => role.UserRole).Include(role => role.UserRole.User).Include(role => role.UserRole.Role) where u.UserRole.RoleId == roleid select u).ToListAsync();
                return userrole;
            }
            catch (Exception)
            {
                throw new Exception("Role Id not Exists");
            }
        }
        public async Task<List<Payroll>> GetByUser(int userid)
        {
            try
            {
                List<Payroll> payrolls = await (from u in dbContext.Payrolls.Include(role => role.UserRole).Include(role => role.UserRole.User).Include(role => role.UserRole.Role) where u.UserRole.UserId == userid select u).ToListAsync();
                return payrolls;
            }
            catch (Exception)
            {
                throw new Exception("User Id not Exists");
            }
        }
        public async Task<Payroll> GetPayrollById(int payrollid)
        {
            try
            {
                Payroll payroll = await (from u in dbContext.Payrolls.Include(role => role.UserRole).Include(role => role.UserRole.User).Include(role => role.UserRole.Role) where u.PayrollId == payrollid select u).FirstAsync();
                return payroll;
            }
            catch (Exception)
            {
                throw new Exception("Payroll Id not Exists");
            }
        }
        public async Task InsertPayroll(DateTime date, Payroll payroll)
        {
            try
            {
                UserRole user = await repo.GetUserRoleById(payroll.UserRoleId);
                int roleid = user.RoleId;
                int userid = user.UserId;
                decimal netsalary = await CalculateSalary(userid, roleid, date);
                payroll.Salary = netsalary;

                await dbContext.Payrolls.AddAsync(payroll);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdatePayroll(int payrollid, Payroll payroll)
        {
            try
            {
                Payroll payrolltoupdate = await GetPayrollById(payrollid);
                payrolltoupdate.Salary = payroll.Salary;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<decimal> CalculateSalary(int userid, int roleid, DateTime date)
        {

            User user = await repo3.GetUsersById(userid);
            DateTime joiningDate = (DateTime)user.JoiningDate;
            int experienceInYears = (int)Math.Floor((date - joiningDate).TotalDays / 365);

            List<Attendance> attendances = await repo1.GetByMonthAndUser(date, userid);

            decimal baseSalaryPerDay;
            decimal totalDaysInMonth;
            decimal workingDays = 0;
            decimal partialDays = 0;
            Role role = await repo2.GetRoleById(roleid);
            decimal oneDaySalary = (decimal)role.RoleSalary;
            totalDaysInMonth = GetBusinessDaysInMonth(date);
            foreach (Attendance attendance in attendances)
            {
                if (attendance.Status == "absent")
                {
                    // No change in working days
                }
                else if (attendance.Status == "partially present")
                {
                    partialDays += 0.5m;
                }
                else
                {
                    workingDays++;
                }
            }
            baseSalaryPerDay = oneDaySalary;
            decimal hra = CalculateHRA(experienceInYears, oneDaySalary);
            decimal netSalary = (baseSalaryPerDay * workingDays) + ((baseSalaryPerDay / 2) * partialDays) + hra;
            return netSalary;
        }
        private int GetBusinessDaysInMonth(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            int businessDays = 0;

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(year, month, day);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDays++;
                }
            }
            return businessDays;
        }
        private decimal CalculateHRA(int experienceInYears, decimal oneDaySalary)
        {

            decimal hraPercentagePerYear = 0.10m;
            decimal hra = (hraPercentagePerYear * experienceInYears) * oneDaySalary;
            return hra;
        }
    }
}
