using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace VnBookLibrary.Web.Areas.Manage.Customizes
{
    public class MailProvider
    {
        private static string host = ConfigurationManager.AppSettings["Host"];
        private static int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
        private static bool enableSsl = bool.Parse(ConfigurationManager.AppSettings["EnableSsl"]);
        private static string fromEmail = ConfigurationManager.AppSettings["FromEmail"];
        private static string password = ConfigurationManager.AppSettings["EmailPassword"];
        public static bool SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, password),
                };

                using (var mailMessage = new MailMessage(fromEmail, toEmail))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    smtp.Send(mailMessage);                    
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, password),
                };

                using (var mailMessage = new MailMessage(fromEmail, toEmail))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    Task rs= smtp.SendMailAsync(mailMessage);                    
                    await rs;
                    if (rs.IsCompleted)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool SendEmail(string toEmail, string subject, string body, string pathFile)
        {
            try
            {
                var smtp = new SmtpClient
                {
                    Host = host,
                    Port = port,
                    EnableSsl = enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, password),
                };
                using (var mailMessage = new MailMessage(fromEmail, toEmail))
                {
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    var file = File.OpenRead(pathFile);
                    Attachment attachment = new Attachment(file, Path.GetFileName(pathFile));
                    mailMessage.Attachments.Add(attachment);
                    smtp.Send(mailMessage);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }        
    }
}