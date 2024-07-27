using EmployeeLibrary.Model;
using EmployeeLibrary.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : ControllerBase
    {
        IPayrollRepository repo;
        public PayrollController(IPayrollRepository payrollRepo)
        {
            repo = payrollRepo;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllPayrollDetails()
        {
            List<Payroll> payrolls = await repo.GetAllPayroll();
            return Ok(payrolls);
        }
        [HttpGet("grouped/{year}")]
        public async Task<ActionResult> GetPayrollYear(DateTime year)
        {
            try
            {
                List<Payroll> payrolls = await repo.GetAllPayrollGroupedByMonthAndUserId(year);
                return Ok(payrolls);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByPayrollId/{payrollid}")]
        public async Task<ActionResult> GetOne(int payrollid)
        {
            try
            {
                Payroll payroll = await repo.GetPayrollById(payrollid);
                return Ok(payroll);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByUserId/{userid}")]
        public async Task<ActionResult> GetByUser(int userid)
        {
            try
            {
                List<Payroll> payroll = await repo.GetByUser(userid);
                return Ok(payroll);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByRoleId/{roleid}")]
        public async Task<ActionResult> GetByRole(int roleid)
        {
            try
            {
                List<Payroll> payroll = await repo.GetByRole(roleid);
                return Ok(payroll);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("{date}")]
        public async Task<ActionResult> Insert(DateTime date, Payroll payroll)
        {
            try
            {

                await repo.InsertPayroll(date, payroll);
                return Created($"api/Payroll/{payroll.PayrollId}", payroll);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPut("{payrollid}")]
        public async Task<ActionResult> Update(int payrollid, Payroll payroll)
        {
            try
            {
                await repo.UpdatePayroll(payrollid, payroll);
                return Ok(payroll);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }
        [HttpDelete("{payrollid}")]
        public async Task<ActionResult> Delete(int payrollid)
        {
            try
            {
                await repo.DeletePayroll(payrollid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
