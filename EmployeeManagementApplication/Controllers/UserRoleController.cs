using EmployeeLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApplication.Controllers
{
    public class UserRoleController : Controller
    {
        static HttpClient svc = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/UserRole/") };
        static HttpClient svcs = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/User/") };
        static HttpClient svcr = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/Role/") };

        // GET: UserRoleController
        public async Task<ActionResult> Index()
        {

            List<UserRole> reg = await svc.GetFromJsonAsync<List<UserRole>>("");
            return View(reg);
        }

        // GET: UserRoleController/Details/5
        [Route("UserRole/Details/{userroleid}")]
        public async Task<ActionResult> Details(int userroleid)
        {
            UserRole userrole = await svc.GetFromJsonAsync<UserRole>("" + "ByUserRoleId/" + userroleid);
            return View(userrole);
        }

        // GET: UserRoleController/Create
        public async Task<ActionResult> Create()
        {

            List<User> users = await svcs.GetFromJsonAsync<List<User>>("");
            ViewData["userid"] = users;
            List<Role> roles = await svcr.GetFromJsonAsync<List<Role>>("");
            ViewData["roleid"] = roles;
            return View();
        }
        // POST: UserRoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserRole userrole)
        {
            try
            {

                if (!ModelState.IsValid)
                    return View();
                await svc.PostAsJsonAsync<UserRole>(" ", userrole);
                TempData["Message"] = "New UserRole is Added";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoleController/Edit/5
        [Route("UserRole/Edit/{userroleid}")]
        public async Task<ActionResult> Edit(int userroleid)
        {
            List<Role> roles = await svcr.GetFromJsonAsync<List<Role>>("");
            ViewData["roleid"] = roles;
            UserRole userrole = await svc.GetFromJsonAsync<UserRole>("" + "ByUserRoleId/" + userroleid);
            return View(userrole);
        }
        // POST: UserRoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UserRole/Edit/{userroleid}")]
        public async Task<ActionResult> Edit(int userroleid, UserRole userrole)
        {
            try
            {
                await svc.PutAsJsonAsync<UserRole>($"{userroleid}", userrole);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserRoleController/Delete/5
        [Route("UserRole/Delete/{userroleid}")]
        public async Task<ActionResult> Delete(int userroleid)
        {
            UserRole userrole = await svc.GetFromJsonAsync<UserRole>("" + "ByUserRoleId/" + userroleid);
            return View(userrole);
        }
        // POST: UserRoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("UserRole/Delete/{userroleid}")]
        public async Task<ActionResult> Delete(int userroleid, IFormCollection collection)
        {
            try
            {
                await svc.DeleteAsync($"{userroleid}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Route("UserRole/GetByUser/{userid}")]
        public async Task<ActionResult> GetByUser(int userid)
        {
            List<UserRole> userrole = await svc.GetFromJsonAsync<List<UserRole>>("ByUserId/" + userid);
            return View(userrole);
        }

        [Route("UserRole/GetByRole/{roleid}")]
        public async Task<ActionResult> GetByRole(int roleid)
        {
            List<UserRole> userrole = await svc.GetFromJsonAsync<List<UserRole>>("ByRoleId/" + roleid);
            return View(userrole);
        }
    }
}
