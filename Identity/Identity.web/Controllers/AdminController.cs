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
        

        public IActionResult Index()
        {
            return View();
        }
       
    }
}
