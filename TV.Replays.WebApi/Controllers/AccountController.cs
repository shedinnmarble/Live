using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TV.Replays.WebApi.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string pwd)
        {
            if (pwd.Equals("dota2.replays.net"))
            {
                HttpCookie cookie = new HttpCookie("Account");
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "PlayerEdit", null);
            }
            else
            {
                ModelState.AddModelError("", "拒绝请求");
                return RedirectToAction("Index");
            }
        }

    }
}
