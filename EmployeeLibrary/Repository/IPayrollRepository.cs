using EmployeeLibrary.Model;

namespace EmployeeLibrary.Repository
{
    public interface IPayrollRepository
    {
        Task<List<Payroll>> GetAllPayroll();
        Task<Payroll> GetPayrollById(int payrollid);
        Task<List<Payroll>> GetAllPayrollGroupedByMonthAndUserId(DateTime year);
        Task<List<Payroll>> GetByUser(int userid);
        Task<List<Payroll>> GetByRole(int roleid);
        Task<decimal> CalculateSalary(int userid, int roleid, DateTime date);
        Task InsertPayroll(DateTime date, Payroll payroll);
        Task UpdatePayroll(int payrollid, Payroll payroll);
        Task DeletePayroll(int payrollid);
    }
}
