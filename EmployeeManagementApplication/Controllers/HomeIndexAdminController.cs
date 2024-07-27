using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApplication.Controllers
{
    public class HomeIndexAdminController : Controller
    {
        // GET: HomeIndexController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HomeIndexController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeIndexController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeIndexController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeIndexController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeIndexController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeIndexController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeIndexController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
