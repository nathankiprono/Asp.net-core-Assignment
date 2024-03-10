using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Queue_Management_System.Models;
using Queue_Management_System.QueueDbContext;

namespace Queue_Management_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly QueueDbConnectionDbContext _context;
        public AccountController(QueueDbConnectionDbContext context)
        {

            _context = context;

        }
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult LoginUser(Users user)
        {

            if (user.UserName == null || user.Password == null)
            {
                TempData["Err"] = "Username or password fields cannot be empty";
                return RedirectToAction("Login", "Account");
            }
            var pass = user.UserName;

            TokenProvider TokenProvider = new TokenProvider(_context);

            var userToken = TokenProvider.LoginUser(user.UserName, user.Password);
            if (userToken == null)
            {
                TempData["Err"] = "Wrong password or username";
                return RedirectToAction("Login", "Account");

            }

            var userIdentity = _context.Users.SingleOrDefault(x => x.UserName == user.UserName);

            if (userIdentity != null)
            {
                HttpContext.Session.SetString("JWToken", userToken);


                HttpContext.Session.SetString("UserName", userIdentity.Names);
                HttpContext.Session.SetString("UserGroup", userIdentity.UserGroup);
                HttpContext.Session.SetString("UserId", userIdentity.Id.ToString());

                if (userIdentity.UserGroup == "Admin")
                {

                    return Redirect("~/Admin/ServiceProviders");
                }
              
                else if (userIdentity.UserGroup == "Providers")
                {

                    return Redirect("~/Queue/WaitingPage");
                }


                else
                {
                    TempData["Err"] = "Unknown User";
                    return Redirect("~/Account/Login");
                }
            }
            else
            {
                TempData["Err"] = "Wrong password or username";
                return RedirectToAction("Login", "Account");
            }
        }
        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Account/Login");

        }

    }
}
