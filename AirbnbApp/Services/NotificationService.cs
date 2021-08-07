using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AirbnbApp.Services
{
    public class NotificationService : INotificationService
    {
        private string code;
        private Random rand = new Random();
        public bool CheckCode(string code)
        {
            return this.code == code;
        }

        public bool SendCode(string email)
        {
            var ok = true;
            var t = Task.Run(() =>
            {
                code = GenerateCode();
                MailAddress to = null;
                MailAddress from = new MailAddress("trellonotification6@gmail.com", "Trello");
                try
                {
                    to = new MailAddress($"{email}");
                }
                catch (ArgumentException)
                {
                    MessageBox.Show("Incorrect email input");
                    ok = false;
                }

                if (to != null)
                {
                    MailMessage m = new MailMessage(from, to);

                    m.Subject = "Notification";

                    m.Body = $"<h2>{this.code}</h2>";

                    m.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                    smtp.Credentials = new NetworkCredential("trellonotification6@gmail.com", "trello123456");
                    smtp.EnableSsl = true;
                    

                    try
                    {
                        smtp.Send(m);
                    }
                    catch (SmtpException)
                    {
                        MessageBox.Show("You dont have internet connection");
                        ok = false;
                    }
                }
            });
            t.Wait();
            return ok;
        }

        private string GenerateCode()
        {
            string str = "123456789QWERTYUIOPASDFGHJKLZXCVBNM";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(str[rand.Next(0, str.Length)]);
            }
            return builder.ToString();
        }
    }
}
