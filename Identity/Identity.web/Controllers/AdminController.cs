using Identity.web.Models;
using Identity.web.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
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
        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager) : base(userManager, signInManager, roleManager)
        {
        }
        
        
        public IActionResult Index()
        {
            var str = User.Identity.Name;
            return View();
        }
        [Authorize(Roles="admin")]//1 den fazla rol giriş yapabilir.
        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleViewModel model)
        {
            AppRole role = new AppRole();
            role.Name = model.Name;

            IdentityResult result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Roles");
            }
            else
            {
                AddModelError(result);//basecontroller
            }
            return View();
        }
        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        public IActionResult RoleDelete(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;
            if (role != null)
            {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;
            }

            return RedirectToAction("Roles");
        }
        public IActionResult RoleUpdate(string id)
        {
            AppRole role = _roleManager.FindByIdAsync(id).Result;

            if (role != null)
            {
                return View(role.Adapt<RoleViewModel>());
            }

            return RedirectToAction("Roles");
        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            AppRole role = _roleManager.FindByIdAsync(roleViewModel.Id).Result;

            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = _roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu.");
            }

            return View(roleViewModel);
        }

        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id;
            AppUser user = _userManager.FindByIdAsync(id).Result;

            ViewBag.userName = user.UserName;

            IQueryable<AppRole> roles = _roleManager.Roles;

            List<string> userroles = _userManager.GetRolesAsync(user).Result as List<string>;

            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel r = new RoleAssignViewModel();
                r.RoleId = role.Id;
                r.RoleName = role.Name;
                if (userroles.Contains(role.Name))
                {
                    r.Exist = true;
                }
                else
                {
                    r.Exist = false;
                }
                roleAssignViewModels.Add(r);
            }

            return View(roleAssignViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            AppUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;

            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)

                {
                    await _userManager.AddToRoleAsync(user, item.RoleName);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }

            return RedirectToAction("Users");
        }

    }
}
