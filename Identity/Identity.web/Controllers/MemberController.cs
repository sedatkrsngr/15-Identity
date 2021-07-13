using Identity.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Identity.web.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using Microsoft.AspNetCore.Http;
using Identity.web.Enums;

namespace Identity.web.Controllers
{
    public class MemberController : Controller
    {
        private readonly UserManager<AppUser> _userManager;//kullanıcı bilgilerini getirir
        private readonly SignInManager<AppUser> _signInManager;//login işlemlerini getirir

        public MemberController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            AppUser user = _userManager.FindByNameAsync("sedat").Result;

            UserViewModel userViewModel = user.Adapt<UserViewModel>();//automapper gibi bu da. Kullanımı kolay


            return View(userViewModel);
        }
        public IActionResult PasswordChange()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordChange(PasswordChangeViewModelcs viewModel)
        {
     

            if (ModelState.IsValid)
            {
                AppUser user = await _userManager.FindByNameAsync("sedat");//User.Identity.Name sedat yerine cookiden gelir

                bool exist = _userManager.CheckPasswordAsync(user, viewModel.PasswordOld).Result;//eski şifre sorgulaması

                if (exist)
                {
                    IdentityResult result = _userManager.ChangePasswordAsync(user, viewModel.PasswordOld, viewModel.PasswordNew).Result;

                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);//şifre önemli olduğu için bu işlemi yapmalıyız
                        await _signInManager.SignOutAsync();//şire değiştiği için otomatik çıkş ve yeniden giriş yapmalıyız ki etkilenmesin

                        await _signInManager.PasswordSignInAsync(user, viewModel.PasswordNew, true, false);

                        ViewBag.success = "true";
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Eski şifreniz yanlış");
                }
            }

            return View(viewModel);
        }

        public IActionResult UserEdit()
        {
            AppUser user = _userManager.FindByNameAsync("sedat").Result;

            UserViewModel userViewModel = user.Adapt<UserViewModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(userViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel userViewModel, IFormFile userPicture)
        {
            ModelState.Remove("Password");//password alanının bilgi güncellemede kullanmadığımız için isvalidden passwordu yoksaydık yoksa geçemeyiz
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));
            if (ModelState.IsValid)
            {
                AppUser user = _userManager.FindByNameAsync("sedat").Result;

                string phone = _userManager.GetPhoneNumberAsync(user).Result;

                if (phone != userViewModel.PhoneNumber)
                {
                    if (_userManager.Users.Any(u => u.PhoneNumber == userViewModel.PhoneNumber))
                    {
                        ModelState.AddModelError("", "Bu telefon numarası başka üye tarafından kullanılmaktadır.");
                        return View(userViewModel);
                    }
                }

                if (userPicture != null && userPicture.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);

                        user.Picture = "/UserPicture/" + fileName;
                    }
                }

                user.UserName = userViewModel.UserName;
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;

                user.City = userViewModel.City;

                user.BirthDay = userViewModel.BirthDay;

                user.Gender = (int)userViewModel.Gender;

                IdentityResult result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, true);

                    ViewBag.success = "true";
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
        public void LogOut()
        {
            _signInManager.SignOutAsync();//startup tarafında logout aktif edilerek çıkış yap butonu arkasından yönlendirme yapılıyor asp-route-returUrl="/Home/Index" butonun arkasına kooyulacak kod. Yönlenen sayfa değiştirilebilir
        }

    }
}
