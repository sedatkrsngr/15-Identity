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

          

            //AppUser yerine IdentityUser da koyabiliriz ama biz IdentiyUser i�erisindeki tan�mlamalardan fazlas�n� tan�mlayaca��m�z i�in ondan kal�t�m al�p AppUser Class�n� olu�turduk
            //IdentityRole i�erisindeki tan�mlar yap�m�za yetersiz ise AppUser gibi yeni bir class olu�turarak bunun �n�ne ge�ebiliriz.
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;

                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abc�defg�h�ijklmno�pqr�stu�vwxyzABC�DEFGHI�JKLMNO�PQRS�TU�VWXYZ0123456789-._";
            }).AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddPasswordValidator<CustomPasswordValidator>()//Custom �ifre kontrol sistemimizi de kontrol eder
            .AddUserValidator<CustomUserValidator>()
            .AddErrorDescriber<CustomIdentityErrorDescriber>()
            .AddDefaultTokenProviders();//token �retmesi i�in bu gerekli

            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "MySite";
            cookieBuilder.HttpOnly = false;//client taraf�nda okumayacaksam false �ekeriz. G�venlidir.
            cookieBuilder.SameSite = SameSiteMode.Lax;//Strict ise bankalarda kullan�lmas� daha mant�kl�. Cookienin sadece bizim siteden kullan�lmas�n� isteyip istemedi�imizi belirtir. Varsay�lan t�m siteler cookimize eri�ebilir.
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            //Always:cookie sadece https �zerinden gelirse sadece https �zerinden okunabilir.
            //SameAsRequest:Hangi tipte(http,https) o tipte g�nderilen yine cookie  o tipte okunabilir. None ise nerden gelirse gelsin cookie her tipten okunabilir.

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Home/Login");//�ye olmayan ki�ilerin K�s�tl� sayfalara eri�meye �al��t���nda belirtilen adrese y�nlendirilir.
                options.LogoutPath= new PathString("/Member/LogOut");//bunu buraya koyarsak ��k�� yap butonunun arkas�ndaki asp-route-returUrl="/Home/Index" �al���r
                options.Cookie = cookieBuilder;
                options.SlidingExpiration = true;//Kullan�c� her giri� yapt���nda cookie �mr� uzar. Yukarda 1 g�n verdik ba�lang��ta. E�er 1 g�n i�erisinde tekrar girerse s�re otomatik 1 g�n uzar.
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.AccessDeniedPath = new PathString("/Member/AccessDenied");//�ye olanlar�n  yetkisiz alana giri�inde y�nlendirilecek sayfa
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
