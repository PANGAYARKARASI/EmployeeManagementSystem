using EmployeeLibrary.Model;

namespace EmployeeLibrary.Repository
{
    public interface IAttendanceRepository
    {
        Task<List<Attendance>> GetAllUserAttendance();
        Task<Attendance> GetAttendanceById(int attendanceid);
        Task<List<Attendance>> GetByUserId(int userId);
        Task<List<Attendance>> GetByDate(DateTime date);
        Task<List<Attendance>> GetByMonth(DateTime month);
        Task<List<Attendance>> GetByMonthAndUser(DateTime month, int userid);
        Task InsertAttendance(Attendance attendance);
        Task UpdateAttendance(int attendanceid, Attendance attendance);
        Task DeleteAttendance(int attendanceid);
    }
}
