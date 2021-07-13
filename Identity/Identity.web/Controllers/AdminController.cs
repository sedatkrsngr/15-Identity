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
    public class AdminController : BaseController
    {
        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null) : base(userManager, signInManager, roleManager)
        {
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

    }
}
