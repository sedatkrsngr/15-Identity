using Identity.web.Models;
using Identity.web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;//kullanıcı bilgilerini getirir

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }
        public IActionResult SignUp()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser();

                appUser.UserName = userViewModel.UserName;
                appUser.PhoneNumber = userViewModel.PhoneNumber;
                appUser.Email = userViewModel.Email;

               IdentityResult result = await _userManager.CreateAsync(appUser,userViewModel.Password);//şifreyi böyle giriyoruz çünkü şifreleme olarak kaydedilmesi gerekiyor

                if (result.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(userViewModel);
        }

        public IActionResult LogIn()
        {

            return View();
        }
    }
}
