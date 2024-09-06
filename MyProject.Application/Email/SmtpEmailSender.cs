using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        public void Send(SendEmailDto dto)
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("nevena.nikolic.174.21@ict.edu.rs", "awkf synm kjsa figi")
            };

            var message = new MailMessage("nevena.nikolic.174.21@ict.edu.rs", dto.SendTo);
            message.Subject = dto.Subject;
            message.Body = dto.Content;
            message.IsBodyHtml = true;
            smtp.Send(message);
        }
    }
}
