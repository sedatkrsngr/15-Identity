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
        private readonly SignInManager<AppUser> _signInManager;//login işlemlerini getirir

        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

                IdentityResult result = await _userManager.CreateAsync(appUser, userViewModel.Password);//şifreyi böyle giriyoruz çünkü şifreleme olarak kaydedilmesi gerekiyor

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

        public IActionResult LogIn(string ReturnUrl)//ReturnUrl yetkisiz erişimde otomatik gelen bir tanım adı bu.
        {
            TempData["ReturnUrl"] = ReturnUrl;//Kısıtlı sayfaya eriştiğimizde login sayfasına yönlendirdikten sonra otomatik girişten sonra girmek istediğimiz sayfaya gitmesi için kullanırız. TempData sayfalar arası kaybolmaz

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = await _userManager.FindByEmailAsync(loginViewModel.Email);

                if (appUser != null)
                {

                    if (await _userManager.IsLockedOutAsync(appUser))
                    {

                        ModelState.AddModelError("", "Hesabınız bir süreliğine kitlenmiştir. Lütfen daha sonra tekrar deneyiniz");
                    }

                    //cookileri sileriz başlangıçta
                    await _signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, loginViewModel.Password, loginViewModel.RememberMe, true);
                    //ilk boolena olan beni hatırlaya bağlı olmalı eğer beni hatırla olmaz ise kapattığımızda bilgiler uçar,
                    //ikinci boolean olan ise hatalı giriş yapılıncı hesabı belli bir süre kitler.

                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(appUser);// başarısız giriş sayısını sıfırlarız
                        if (TempData["ReturnUrl"] != null)//işlem başarılı olduğunda ilgili sayfaya geri gitsin
                        {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("Index", "Member");
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(appUser)//başarısız girişi 1 arttır
                        int failCount = await _userManager.GetAccessFailedCountAsync(appUser);//Başarısız giriş sayısı
                        ModelState.AddModelError("", $"{failCount} kez başarısız giriş");
                        if (failCount == 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(appUser, new DateTimeOffset(DateTime.Now.AddMinutes(20)));//20 dk lığına kitledik

                            ModelState.AddModelError("", $"Hesabınız {failCount} başarısız girişten dolayı 20 dk kitlenmiştir. Lütfen daha sonra tekrar deneyiniz");
                            return View(loginViewModel);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Yanlış email adresi veya şifresi");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu Email adresine kayıtlı kullanıcı bulunamamıştır.");
                    //Hata summaryde görünür ama nameof(loginViewModel.Email) yazarsak "" yerine mail altında görünür
                }



            }
            return View(loginViewModel);
        }
    }
}
