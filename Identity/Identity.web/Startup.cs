using Identity.web.CustomValidation;
using Identity.web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(opt =>
            {
                //opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString"));
                opt.UseSqlServer(Configuration["ConnectionStrings:DefaultConnectionString"]);

            });

            //AppUser yerine IdentityUser da koyabiliriz ama biz IdentiyUser içerisindeki tanýmlamalardan fazlasýný tanýmlayacaðýmýz için ondan kalýtým alýp AppUser Classýný oluþturduk
            //IdentityRole içerisindeki tanýmlar yapýmýza yetersiz ise AppUser gibi yeni bir class oluþturarak bunun önüne geçebiliriz.
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcçdefgðhýijklmnoöpqrþstuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789-._";
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddPasswordValidator<CustomPasswordValidator>()//Custom þifre kontrol sistemimizi de kontrol eder
            .AddUserValidator<CustomUserValidator>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>();
            
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
