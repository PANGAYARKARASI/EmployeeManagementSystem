using EmployeeLibrary.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementApplication.Controllers
{
    public class UserController : Controller
    {
        static HttpClient svc = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/User/") };
        static HttpClient svcur = new HttpClient { BaseAddress = new Uri("http://localhost:5106/api/UserRole/") };

        // GET: UserController
        public async Task<ActionResult> Index()
        {
            List<User> reg = await svc.GetFromJsonAsync<List<User>>("");
            return View(reg);

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Verify(string email, string password)
        {
            try
            {
                HttpResponseMessage response = await svc.GetAsync($"ByEmail/{email}");

                if (ModelState.IsValid)
                {
                    if (response.IsSuccessStatusCode)
                    {
                        User reg = await response.Content.ReadFromJsonAsync<User>();

                        if (reg.Email == email && reg.Password == password)
                        {
                            TempData["UserId"] = reg.UserId;
                            HttpContext.Session.SetInt32("UserId", reg.UserId);
                            HttpContext.Session.SetString("Email", reg.Email);
                            HttpContext.Session.SetString("Password", reg.Password);
                            List<UserRole> userrole = await svcur.GetFromJsonAsync<List<UserRole>>("ByUserId/" + reg.UserId);
                            Boolean isadmin = userrole.Any(ur => ur.RoleId == 1);
                            if (isadmin)
                            {
                                return RedirectToAction("Index", "HomeIndexAdmin");

                            }
                            else
                            {
                                return RedirectToAction("Index", "HomeIndexUser");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Incorrect Email Id or Password");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "User doesn't exists");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            catch
            {
                return View("Error");
            }
            return View("Login");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // ModelState.AddModelError(string.Empty, "Invalid email format");
                }
                else
                {
                    HttpResponseMessage res = await svc.GetAsync($"ByEmail/{user.Email}");
                    if (res.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError(string.Empty, "Email already exists, PLease choose a different one");
                    }
                    else
                    {
                        await svc.PostAsJsonAsync<User>("", user);
                        return RedirectToAction(nameof(Login));
                    }
                }
            }
            catch
            {
                return View("Error");
            }
            return View("Register");
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Login", "User");
        }

        // GET: UserController/Details/5
        [Route("User/Details/{userid}")]
        public async Task<ActionResult> Details(int userid)
        {
            User user = await svc.GetFromJsonAsync<User>("" + "ByUserId/" + userid);
            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                await svc.PostAsJsonAsync<User>(" ", user);
                TempData["Message"] = "New User is Added";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        [Route("User/Edit/{userid}")]
        public async Task<ActionResult> Edit(int userid)
        {
            User user = await svc.GetFromJsonAsync<User>("" + "ByUserId/" + userid);
            return View(user);
        }
        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("User/Edit/{userid}")]
        public async Task<ActionResult> Edit(int userid, User user)
        {
            try
            {
                await svc.PutAsJsonAsync<User>($"{userid}", user);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        [Route("User/Delete/{userid}")]
        public async Task<ActionResult> Delete(int userid)
        {
            User user = await svc.GetFromJsonAsync<User>("" + "ByUserId/" + userid);
            return View(user);
        }
        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("User/Delete/{userid}")]
        public async Task<ActionResult> Delete(int userid, IFormCollection collection)
        {
            try
            {
                await svc.DeleteAsync($"{userid}");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
