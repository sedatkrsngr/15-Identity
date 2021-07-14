using Identity.web.CustomValidation;
using Identity.web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddDefaultTokenProviders();//token üretmesi için bu gerekli

            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "MySite";
            cookieBuilder.HttpOnly = false;//client tarafýnda okumayacaksam false çekeriz. Güvenlidir.
            cookieBuilder.SameSite = SameSiteMode.Lax;//Strict ise bankalarda kullanýlmasý daha mantýklý. Cookienin sadece bizim siteden kullanýlmasýný isteyip istemediðimizi belirtir. Varsayýlan tüm siteler cookimize eriþebilir.
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //Always:cookie sadece https üzerinden gelirse sadece https üzerinden okunabilir.
            //SameAsRequest:Hangi tipte(http,https) o tipte gönderilen yine cookie  o tipte okunabilir. None ise nerden gelirse gelsin cookie her tipten okunabilir.

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Home/Login");//üye olmayan kiþilerin Kýsýtlý sayfalara eriþmeye çalýþtýðýnda belirtilen adrese yönlendirilir.
                options.LogoutPath= new PathString("/Member/LogOut");//bunu buraya koyarsak çýkýþ yap butonunun arkasýndaki asp-route-returUrl="/Home/Index" çalýþýr
                options.Cookie = cookieBuilder;
                options.SlidingExpiration = true;//Kullanýcý her giriþ yaptýðýnda cookie ömrü uzar. Yukarda 1 gün verdik baþlangýçta. Eðer 1 gün içerisinde tekrar girerse süre otomatik 1 gün uzar.
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.AccessDeniedPath = new PathString("/Member/AccessDenied");//Üye olanlarýn  yetkisiz alana giriþinde yönlendirilecek sayfa
            });

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

            app.UseAuthentication();
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
