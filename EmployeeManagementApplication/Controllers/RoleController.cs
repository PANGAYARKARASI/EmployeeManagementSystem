using EmployeeLibrary.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApplication.Controllers
{
    public class RoleController : Controller
    {
        static HttpClient svc = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/Role/") };

        // GET: RoleController
        public async Task< ActionResult> Index()
        {
            List<Role> reg = await svc.GetFromJsonAsync<List<Role>>("");
            return View(reg);
        }

        // GET: RoleController/Details/5
        [Route("Role/Details/{roleid}")]
        public async Task< ActionResult> Details(int roleid)
        {
            Role role = await svc.GetFromJsonAsync<Role>("" + "ByRoleId/" + roleid);
            return View(role);
        }

        // GET: RoleController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Role role)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();
                await svc.PostAsJsonAsync<Role>(" ", role);
                TempData["Message"] = "New Role is Added";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Edit/5
        [Route("Role/Edit/{roleid}")]
        public async Task<ActionResult> Edit(int roleid)
        {
            Role role = await svc.GetFromJsonAsync<Role>("" + "ByRoleId/" + roleid);
            return View(role);
        }
        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Role/Edit/{roleid}")]
        public async Task<ActionResult> Edit(int roleid, Role role)
        {
            try
            {
                await svc.PutAsJsonAsync<Role>($"{roleid}", role);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Delete/5
        [Route("Role/Delete/{roleid}")]
        public async Task<ActionResult> Delete(int roleid)
        {
            Role role = await svc.GetFromJsonAsync<Role>("" + "ByRoleId/" + roleid);
            return View(role);
        }
        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Role/Delete/{roleid}")]
        public async Task<ActionResult> Delete(int roleid, IFormCollection collection)
        {
            try
            {
                await svc.DeleteAsync($"{roleid}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
