using EmployeeLibrary.Model;
using EmployeeManagementApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApplication.Controllers
{
    public class AttendanceController : Controller
    {
        EmployeeDbContext dbContext;
        public AttendanceController(EmployeeDbContext attendanceRepo)
        {
            dbContext = attendanceRepo;
        }
        static HttpClient svc = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/Attendance/") };
        static HttpClient svcs = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/User/") };
        static HttpClient svcur = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/UserRole/") };

        public async Task<ActionResult> Index()
        {
            List<Attendance> reg = await svc.GetFromJsonAsync<List<Attendance>>("");
            return View(reg);
        }

        public async Task<ActionResult> CheckIn()
        {
            var attendanceRecord = new Attendance
            {
                UserId = HttpContext.Session.GetInt32("UserId"),
                CheckIn = DateTime.Now,
                Date = DateTime.Now.Date,
                Status = "present"
            };
            TempData["SuccessCheckIn"] = true;
            dbContext.Attendances.Add(attendanceRecord);
            dbContext.SaveChanges();

            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int userid = ViewBag.UserId;
            List<UserRole> userrole = await svcur.GetFromJsonAsync<List<UserRole>>("ByUserId/" + userid);
            System.Boolean isadmin = userrole.Any(ur => ur.RoleId == 1);
            if (isadmin)
            {
                return RedirectToAction("Index", "HomeIndexAdmin");
            }
            else
                return RedirectToAction("Index", "HomeIndexUser");
        }

        public async Task<ActionResult> CheckOut()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId.HasValue)
            {
                DateTime today = DateTime.Today;
                var earliestAttendance = dbContext.Attendances
                    .Where(a => a.UserId == userId.Value && a.CheckIn.Date == today)
                    .OrderBy(a => a.CheckIn)
                    .FirstOrDefault();
                if (earliestAttendance != null)
                {
                    earliestAttendance.CheckOut = DateTime.Now;
                    TempData["SuccessCheckOut"] = true;
                    earliestAttendance.WorkingHours = (earliestAttendance.CheckOut - earliestAttendance.CheckIn).Hours;
                    if (earliestAttendance.WorkingHours > 0 && earliestAttendance.WorkingHours <= 4)
                        earliestAttendance.Status = "partially present";
                    else if (earliestAttendance.WorkingHours > 0 && earliestAttendance.WorkingHours <= 24)
                        earliestAttendance.Status = "present";
                    else
                        earliestAttendance.Status = "absent";
                    dbContext.SaveChanges();
                }
            }
            List<UserRole> userrole = await svcur.GetFromJsonAsync<List<UserRole>>("ByUserId/" + userId);
            System.Boolean isadmin = userrole.Any(ur => ur.RoleId == 1);
            if (isadmin)
            {
                return RedirectToAction("Index", "HomeIndexAdmin");
            }
            else
                return RedirectToAction("Index", "HomeIndexUser");
        }

        [Route("Attendance/Details/{attendanceid}")]
        public async Task<ActionResult> Details(int attendanceid)
        {
            Attendance attendance = await svc.GetFromJsonAsync<Attendance>("" + "ByAttendanceId/" + attendanceid);
            return View(attendance);
        }

        public async Task<ActionResult> Create()
        {
            List<User> users = await svcs.GetFromJsonAsync<List<User>>("");
            ViewData["userid"] = users;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Attendance attendance)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                await svc.PostAsJsonAsync<Attendance>(" ", attendance);
                TempData["Message"] = "New Attendance is Added";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Route("Attendance/Edit/{attendanceid}")]
        public async Task<ActionResult> Edit(int attendanceid)
        {
            List<User> users = await svcs.GetFromJsonAsync<List<User>>("");
            ViewData["userid"] = users;
            Attendance attendance = await svc.GetFromJsonAsync<Attendance>("" + "ByAttendanceId/" + attendanceid);
            return View(attendance);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Attendance/Edit/{attendanceid}")]
        public async Task<ActionResult> Edit(int attendanceid, Attendance attendance)
        {
            try
            {
                await svc.PutAsJsonAsync<Attendance>($"{attendanceid}", attendance);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Route("Attendance/Delete/{attendanceid}")]
        public async Task<ActionResult> Delete(int attendanceid)
        {
            Attendance attendance = await svc.GetFromJsonAsync<Attendance>("" + "ByAttendanceId/" + attendanceid);
            return View(attendance);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Attendance/Delete/{attendanceid}")]
        public async Task<ActionResult> Delete(int attendanceid, IFormCollection collection)
        {
            try
            {
                await svc.DeleteAsync($"{attendanceid}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> GetByUser()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            int userid = ViewBag.UserId;
            List<Attendance> attendance = await svc.GetFromJsonAsync<List<Attendance>>("" + "ByUserId/" + userid);
            return View(attendance);
        }

        [HttpGet]
        public ActionResult GetDateFromUser_()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetDateFromUser_(GetDate getDate)
        {
            string date = getDate.Date.ToString();
            return RedirectToAction("GetByDate", "Attendance", new { date = getDate.Date.ToString() });
        }
        [Route("Attendance/GetByDate/{date}")]
        public async Task<ActionResult> GetByDate(string date)
        {
            DateTime dateFormat = DateTime.Parse(date);
            string formattedDate = dateFormat.ToString("yyyy-MM-dd");
            List<Attendance> attendance = await svc.GetFromJsonAsync<List<Attendance>>($"ByDate/{formattedDate}");
            return View(attendance);
        }

        [HttpGet]
        public ActionResult GetMonth()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetMonth(GetDate getMonth)
        {
            string month = getMonth.Date.ToString();
            return RedirectToAction("GetByMonth", "Attendance", new { month = getMonth.Date.ToString() });
        }
        [Route("Attendance/GetByMonth/{month}")]
        public async Task<ActionResult> GetByMonth(string month)
        {
            DateTime dateFormat = DateTime.Parse(month);
            string formattedDate = dateFormat.ToString("yyyy-MM-dd");
            List<Attendance> attendance = await svc.GetFromJsonAsync<List<Attendance>>($"ByMonth/{formattedDate}");
            return View(attendance);
        }

        [HttpGet]
        public async Task<ActionResult> GetMonthAndUser()
        {
            List<User> users = await svcs.GetFromJsonAsync<List<User>>("");
            ViewData["userid"] = users;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMonthAndUser(GetDate getMonthandUser)
        {
            string monthanduser = getMonthandUser.Date.ToString();
            return RedirectToAction("GetByMonthAndUser", "Attendance", new { monthanduser = getMonthandUser.Date.ToString(), userid = getMonthandUser.UserId });
        }
        [Route("Attendance/GetByMonthAndUser/{monthanduser}/{userid}")]
        public async Task<ActionResult> GetByMonthAndUser(string monthanduser, int userid)
        {
            DateTime dateFormat = DateTime.Parse(monthanduser);
            string formattedDate = dateFormat.ToString("yyyy-MM-dd");
            List<Attendance> attendance = await svc.GetFromJsonAsync<List<Attendance>>($"ByMonthAndUser/{formattedDate}/{userid}");
            return View(attendance);
        }
    }
}
