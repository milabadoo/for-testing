using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Text;
using Library.Domain.Abstract;
using Library.Domain.Entity;

namespace Library.Domain.Concrete
{
    class EmailOrderProcessor
    {
        public class EmailSettings
        {
            public string MailToAddress = "orders@example.com";
            public string MailFromAddress = "gamestore@example.com";
            public bool UseSsl = true;
            public string Username = "MySmtpUsername";
            public string Password = "MySmtpPassword";
            public string ServerName = "smtp.example.com";
            public int ServerPort = 587;
            public bool WriteAsFile = true;
            public string FileLocation = @"c:\game_store_emails";
        }

        public class EmailOrderProcessor : IOrderProcessor
        {
            private EmailSettings emailSettings;

            public EmailOrderProcessor(EmailSettings settings)
            {
                emailSettings = settings;
            }

            public void ProcessOrder(Order order, Details shippingInfo)
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.EnableSsl = emailSettings.UseSsl;
                    smtpClient.Host = emailSettings.ServerName;
                    smtpClient.Port = emailSettings.ServerPort;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials
                        = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                    if (emailSettings.WriteAsFile)
                    {
                        smtpClient.DeliveryMethod
                            = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                        smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                        smtpClient.EnableSsl = false;
                    }

                    StringBuilder body = new StringBuilder()
                        .AppendLine("Новый заказ обработан")
                        .AppendLine("---")
                        .AppendLine("Товары:");

                   

                   

                    MailMessage mailMessage = new MailMessage(
                                           emailSettings.MailFromAddress,	// От кого
                                           emailSettings.MailToAddress,		// Кому
                                           "Новый заказ отправлен!",		// Тема
                                           body.ToString()); 				// Тело письма

                    if (emailSettings.WriteAsFile)
                    {
                        mailMessage.BodyEncoding = Encoding.UTF8;
                    }

                    smtpClient.Send(mailMessage);
                }
            }
        }

    }
}
