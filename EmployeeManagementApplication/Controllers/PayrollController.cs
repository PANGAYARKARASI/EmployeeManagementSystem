using EmployeeLibrary.Model;
using EmployeeManagementApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace EmployeeManagementApplication.Controllers
{
    public class PayrollController : Controller
    {
        static HttpClient svc = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/Payroll/") };
        static HttpClient svcs = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/User/") };
        static HttpClient svcr = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/Role/") };
        static HttpClient svcur = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/UserRole/") };

        // GET: PayrollController
        public async Task<ActionResult> Index()
        {
            List<Payroll> reg = await svc.GetFromJsonAsync<List<Payroll>>("");
            return View(reg);
        }

        [HttpGet]
        public ActionResult GetMonthAndUser()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetMonthAndUser(GetDate getMonthandUser)
        {
            string year = getMonthandUser.Date.ToString();
            return RedirectToAction("GetPayrollByYear", "Payroll", new { year = getMonthandUser.Date.ToString() });
        }
        [Route("Payroll/GetPayrollByYear/{year}")]
        public async Task<ActionResult> GetPayrollByYear(string year)
        {
            try
            {
                DateTime dateFormat = DateTime.Parse(year);
                string formattedDate = dateFormat.ToString("yyyy-MM-dd");
                List<Payroll> payrolls = await svc.GetFromJsonAsync<List<Payroll>>($"grouped/{formattedDate}");
                var groupedPayrolls = payrolls
                    .GroupBy(p => (p.Date.Year, p.Date.Month, p.UserRole.User.UserId))
                    .SelectMany(g => g)
                    .ToList();
                return View(groupedPayrolls);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        // GET: PayrollController/Details/5
        [Route("Payroll/Details/{payrollid}")]
        public async Task<ActionResult> Details(int payrollid)
        {
            Payroll payroll = await svc.GetFromJsonAsync<Payroll>("" + "ByPayrollId/" + payrollid);
            return View(payroll);

        }

        // GET: PayrollController/Create
        public async Task<ActionResult> Create()
        {
            List<User> users = await svcs.GetFromJsonAsync<List<User>>("");
            ViewData["userid"] = users;
            List<Role> roles = await svcr.GetFromJsonAsync<List<Role>>("");
            ViewData["roleid"] = roles;
            List<UserRole> userroles = await svcur.GetFromJsonAsync<List<UserRole>>("");
            ViewData["userroleid"] = userroles;
            return View();
        }
        // POST: PayrollController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PostCreate(DateTime date, Payroll payroll)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                ViewBag.userid = HttpContext.Session.GetInt32("UserId");
                int userid =ViewBag.userid;
                string formattedDate = date.ToString("yyyy-MM-dd");
                var response = await svc.PostAsJsonAsync($"{formattedDate}", payroll);
                TempData["Message"] = "New Payroll is Added";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PayrollController/Edit/5
        [Route("Payroll/Edit/{payrollid}")]
        public async Task<ActionResult> Edit(int payrollid)
        {
            Payroll payroll = await svc.GetFromJsonAsync<Payroll>("" + "ByPayrollId/" + payrollid);
            return View(payroll);
        }
        // POST: PayrollController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Payroll/Edit/{payrollid}")]
        public async Task<ActionResult> Edit(int payrollid, Payroll payroll)
        {
            try
            {
                await svc.PutAsJsonAsync<Payroll>($"{payrollid}", payroll);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }

        // GET: PayrollController/Delete/5
        [Route("Payroll/Delete/{payrollid}")]
        public async Task<ActionResult> Delete(int payrollid)
        {
            Payroll payroll = await svc.GetFromJsonAsync<Payroll>("" + "ByPayrollId/" + payrollid);
            return View(payroll);
        }
        // POST: PayrollController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Payroll/Delete/{payrollid}")]
        public async Task<ActionResult> Delete(int payrollid, IFormCollection collection)
        {
            try
            {
                await svc.DeleteAsync($"{payrollid}");
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
            List<Payroll> payroll = await svc.GetFromJsonAsync<List<Payroll>>(""+"ByUserId/" + userid);
            return View(payroll);
        }

        [Route("Payroll/GetByRole/{roleid}")]
        public async Task<ActionResult> GetByRole(int roleid)
        {
            List<Payroll> payroll = await svc.GetFromJsonAsync<List<Payroll>>("ByRoleId/" + roleid);
            return View(payroll);
        }
    }
}
