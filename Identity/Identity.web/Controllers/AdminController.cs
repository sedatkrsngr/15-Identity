using Identity.web.Models;
using Identity.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.web.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<AppUser> _userManager;//kullanıcı bilgilerini getirir

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

    }
}
