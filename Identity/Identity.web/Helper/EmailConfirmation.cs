﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Identity.web.Helper
{
    public static class EmailConfirmation
    {
        public static void SendEmail(string link, string email)
        {
            //MailMessage mail = new MailMessage();

            //SmtpClient smtpClient = new SmtpClient("mail.teknohub.net");

            //mail.From = new MailAddress("fcakiroglu@teknohub.net");
            //mail.To.Add(email);

            //mail.Subject = $"www.bıdıbı.com::Email doğrulama.";
            //mail.Body = "<h2>Email adres yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/>";
            //mail.Body += $"<a href='{link}'>email doğrulama linki</a>";
            //mail.IsBodyHtml = true;
            //smtpClient.Port = 587;
            //smtpClient.Credentials = new System.Net.NetworkCredential("fcakiroglu@teknohub.net", "FatihFatih10");

            //smtpClient.Send(mail);
        }
    }
}
