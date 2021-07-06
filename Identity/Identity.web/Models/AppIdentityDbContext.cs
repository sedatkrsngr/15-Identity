using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.web.Models
{
    //sonuna string yazmamızın sebebi idler string tutuldupu için string bazlı tutacak
    public class AppIdentityDbContext : IdentityDbContext<AppUser,AppRole,string>//Identity verileri için context
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }


    }
}
