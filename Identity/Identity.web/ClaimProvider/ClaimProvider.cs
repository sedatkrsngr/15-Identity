using Identity.web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.web.ClaimProvider
{
    public class ClaimProvider : IClaimsTransformation// claimleri tutmak için
    {

        public UserManager<AppUser> _userManager { get; set; }

        public ClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal != null && principal.Identity.IsAuthenticated)//üye kullanıcı için
            {
                ClaimsIdentity ıdentity = principal.Identity as ClaimsIdentity;

                AppUser user = await _userManager.FindByNameAsync(ıdentity.Name);

                if (user != null)
                {
                    if (user.City != null)
                    {
                        if (!principal.HasClaim(c => c.Type == "city"))
                        {
                            Claim cityClaim = new Claim("city", user.City, ClaimValueTypes.String, "Internal");
                            ıdentity.AddClaim(cityClaim);
                        }
                    }
                }
            }
            return principal;
        }
    }
}
