using Identity.web.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.web.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Kullanıcı Adı gereklidir!")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Tel no gereklidir!")]
        [Display(Name = "Tel No")]
        [Phone(ErrorMessage ="Telefon formatı uygun değil!")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email Adresi gereklidir!")]
        [Display(Name = "Email Adresi")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Email formatı uygun değil!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifreniz gereklidir!")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Doğum tarihi")]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        public string Picture { get; set; }

        [Display(Name = "Şehir")]
        public string City { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }

    }
}
