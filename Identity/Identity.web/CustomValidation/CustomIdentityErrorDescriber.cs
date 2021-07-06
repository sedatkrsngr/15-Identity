using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.web.CustomValidation
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        //burada tüm hataları türkçeleştirebiliriz varsayılan olarak bir kaç tane düzelttim
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $"Bu kullanıcı adı: {userName} geçersizdir!"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"Bu email: {email} kullanılmaktadır!"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"Bu kullanıcı adı: {userName} kullanılmaktadır!"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"Şifreniz en az {length} karakterli olmalıdır!"
            };
        }
    }
}
