using EmployeeLibrary.Model;
using EmployeeLibrary.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        IAttendanceRepository repo;
        public AttendanceController(IAttendanceRepository attendanceRepo)
        {
            repo = attendanceRepo;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllAttendanceDetails()
        {
            List<Attendance> attendances = await repo.GetAllUserAttendance();
            return Ok(attendances);
        }
        [HttpGet("ByAttendanceId/{attendanceid}")]
        public async Task<ActionResult> GetOne(int attendanceid)
        {
            try
            {
                Attendance attendance = await repo.GetAttendanceById(attendanceid);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByDate/{date}")]
        public async Task<ActionResult> GetByDate(DateTime date)
        {
            try
            {
                List<Attendance> attendance = await repo.GetByDate(date);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByMonth/{month}")]
        public async Task<ActionResult> GetByMonth(DateTime month)
        {
            try
            {
                List<Attendance> attendance = await repo.GetByMonth(month);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("ByMonthAndUser/{month}/{userid}")]
        public async Task<ActionResult> GetByMonthAndUser(DateTime month, int userid)
        {
            try
            {
                List<Attendance> attendance = await repo.GetByMonthAndUser(month, userid);
                return Ok(attendance);
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
                List<Attendance> attendance = await repo.GetByUserId(userid);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Insert(Attendance attendance)
        {
            try
            {
                await repo.InsertAttendance(attendance);
                return Created($"api/Attendance/{attendance.AttendanceId}", attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        [HttpPut("{attendanceid}")]
        public async Task<ActionResult> Update(int attendanceid, Attendance attendance)
        {
            try
            {
                await repo.UpdateAttendance(attendanceid, attendance);
                return Ok(attendance);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);

            }
        }
        [HttpDelete("{attendanceid}")]
        public async Task<ActionResult> Delete(int attendanceid)
        {
            try
            {
                await repo.DeleteAttendance(attendanceid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
