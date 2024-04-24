using BBMS.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace BBMS.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Member model)
        {
            using (var context = new BBManagementEntities())
            {
                bool isvalid = context.Userlogin.Any(x => x.Username == model.Username && x.Password == model.Password);

                if (isvalid)
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    return RedirectToAction("Index", "BDonors");
                }
                else
                    ModelState.AddModelError("", "Invalid Username and Password Please Try again");
                return View();
            }

        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Userlogin member)
        {
            using (var context = new BBManagementEntities())
            {
                context.Userlogin.Add(member);
                context.SaveChanges();

                var roleAssignment = new UserRole { UserId = member.Id, Role = "User" };
                context.UserRole.Add(roleAssignment);
                context.SaveChanges();


            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}