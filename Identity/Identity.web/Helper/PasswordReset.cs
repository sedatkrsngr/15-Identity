﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.web.Helper
{
    public static class PasswordReset
    {

        public static void PasswordResetSentEmail(string link, string email)
        {
            //MailMessage mail = new MailMessage();

            //SmtpClient smtpClient = new SmtpClient("mail.teknohub.net");

            //mail.From = new MailAddress("fcakiroglu@teknohub.net");
            //mail.To.Add(email);

            //mail.Subject = $"www.bıdıbı.com::Şifre sıfırlama";
            //mail.Body = "<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            //mail.Body += $"<a href='{link}'>şifre yenileme linki</a>";
            //mail.IsBodyHtml = true;
            //smtpClient.Port = 587;
            //smtpClient.Credentials = new System.Net.NetworkCredential("fcakiroglu@teknohub.net", "FatihFatih10");

            //smtpClient.Send(mail);
        }
    }
}
