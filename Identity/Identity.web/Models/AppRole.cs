﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.web.Models
{
    public class AppRole : IdentityRole
    {
        public string RoleType { get; set; }
    }
}
