using EmployeeLibrary.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLibrary.Repository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        EmployeeDbContext dbContext;
        public AttendanceRepository(EmployeeDbContext attendanceRepo)
        {
            dbContext = attendanceRepo;
        }
        public async Task DeleteAttendance(int attendanceid)
        {
            try
            {
                Attendance attendancetodelete = await GetAttendanceById(attendanceid);
                dbContext.Attendances.Remove(attendancetodelete);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Attendance>> GetAllUserAttendance()
        {
            try
            {
                List<Attendance> attendances = await dbContext.Attendances.Include(role => role.User).ToListAsync<Attendance>();
                return attendances;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Attendance> GetAttendanceById(int attendanceid)
        {

            try
            {
                Attendance attendance = await (from u in dbContext.Attendances.Include(role => role.User) where u.AttendanceId == attendanceid select u).FirstAsync();
                return attendance;
            }
            catch (Exception)
            {
                throw new Exception("Attendance Id not Exists");
            }
        }

        public async Task<List<Attendance>> GetByDate(DateTime date)
        {
            try
            {
                List<Attendance> attendance = await (from u in dbContext.Attendances.Include(role => role.User) where u.Date.Value.Date == date.Date select u).ToListAsync();
                return attendance;
            }
            catch (Exception)
            {
                throw new Exception("No Attendance exists on this date");
            }

        }

        public async Task<List<Attendance>> GetByMonth(DateTime month)
        {
            try
            {
                List<Attendance> attendance = await (from u in dbContext.Attendances.Include(role => role.User)
                                                     where u.Date.Value.Month == month.Month && u.Date.Value.Year == month.Year
                                                     select u).ToListAsync();
                return attendance;
            }
            catch (Exception)
            {
                throw new Exception("No Attendance exists on this date");
            }
        }

        public async Task<List<Attendance>> GetByMonthAndUser(DateTime month, int userid)
        {
            try
            {
                List<Attendance> attendance = await (from u in dbContext.Attendances.Include(role => role.User)
                                                     where u.Date.Value.Month == month.Month && u.Date.Value.Year == month.Year && u.UserId == userid
                                                     select u).ToListAsync();
                return attendance;
            }
            catch (Exception)
            {
                throw new Exception("No Attendance exists on this date");
            }
        }

        public async Task<List<Attendance>> GetByUserId(int userId)
        {
            try
            {
                List<Attendance> attendance = await (from u in dbContext.Attendances.Include(role => role.User) where u.UserId == userId select u).ToListAsync();
                return attendance;
            }
            catch (Exception)
            {
                throw new Exception("Attendance Id not Exists");
            }
        }

        public async Task InsertAttendance(Attendance attendance)
        {
            try
            {
                attendance.WorkingHours = (attendance.CheckOut - attendance.CheckIn).Hours;
                await dbContext.Attendances.AddAsync(attendance);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAttendance(int attendanceid, Attendance attendance)
        {
            try
            {
                attendance.WorkingHours = (attendance.CheckOut - attendance.CheckIn).Hours;
                Attendance attendancetoup = await GetAttendanceById(attendanceid);
                attendancetoup.UserId = attendance.UserId;
                attendancetoup.Date = attendance.Date;
                attendancetoup.Status = attendance.Status;
                attendancetoup.CheckIn = attendance.CheckIn;
                attendancetoup.CheckOut = attendance.CheckOut;
                attendancetoup.WorkingHours = attendance.WorkingHours;
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
