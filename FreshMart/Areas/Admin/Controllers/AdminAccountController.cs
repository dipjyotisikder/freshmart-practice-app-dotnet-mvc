using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreshMart.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FreshMart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminAccountController : Controller
    {
        [Route("Admin/AdminAccount")]

        public IActionResult SignIn()
        {

            return View();
        }


        [Route("Admin/AdminAccount")]
        [HttpPost]
        public IActionResult SignIn(AccountViewModel account)
        {



            if (account.Username == "Admin" && account.Password == "Admin")
            {


                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                return View();
            }

        }
    }
}