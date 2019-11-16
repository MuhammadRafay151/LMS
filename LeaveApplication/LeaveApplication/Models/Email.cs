using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;

namespace LeaveApplication.Models
{
    public class Email
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        SmtpSection secObj = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");
        SmtpClient smtp = new SmtpClient();
        public Email()
        {

            smtp.Host = secObj.Network.Host;
            smtp.Port = secObj.Network.Port;
            smtp.EnableSsl = secObj.Network.EnableSsl;
            smtp.DeliveryMethod = secObj.DeliveryMethod;
            smtp.UseDefaultCredentials = secObj.Network.DefaultCredentials;
            smtp.Credentials = new NetworkCredential(secObj.Network.UserName, secObj.Network.Password);

        }
        public void Send(string To_Mail_Address, string Subject, string Content)
        {
            using (var message = new MailMessage(secObj.Network.UserName, To_Mail_Address))
            {
                message.Subject = Subject;
                message.Body = Content;
                message.IsBodyHtml = true;
                smtp.Send(message);
            }

        }

        //        Emailer mail = new Emailer();
        //        mail.GmailUsername = "youremailid@gmail.com";
        //        mail.GmailPassword = "YourPassword";
        //        mail.GmailHost = "smtp.gmail.com";
        //        mail.GmailSSL = true;
        //        mail.GmailPort = 587;
        //        mail.ToEmail = "Senderemailid@gmail.com";
        //        mail.Subject = "Hello World";
        //        mail.Body = "From C#";
        //        mail.IsHtml = true;
        //        mail.Send();

        //Enable less secure app in gmail
        //https://myaccount.google.com/lesssecureapps?pli=1
    }
}