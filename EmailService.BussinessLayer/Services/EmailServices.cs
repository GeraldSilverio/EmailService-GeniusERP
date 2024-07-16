﻿using EmailService.BussinessLayer.Dtos;
using EmailService.BussinessLayer.Interfaces;
using MailKit.Security;
using MimeKit;

namespace EmailService.BussinessLayer.Services
{
    public class EmailServices : IEmailServices
    {
        public async Task<bool> SendEmailAsync(EmailRequest emailRequest)
        {
            try
            {
                MimeMessage message = new()
                {
                    Sender = MailboxAddress.Parse(emailRequest.From ?? emailRequest.EmailFrom)
                };

                message.To.Add(MailboxAddress.Parse(emailRequest.To));
                message.Subject = emailRequest.Subject;

                BodyBuilder bodyBuilder = new()
                {
                    HtmlBody = emailRequest.Body
                };

                message.Body = bodyBuilder.ToMessageBody();

                using MailKit.Net.Smtp.SmtpClient smtpClient = new();
                smtpClient.Connect(emailRequest.SmtpHost, emailRequest.SmtpPort, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(emailRequest.SmtpUser, emailRequest.SmtpPass);
                await smtpClient.SendAsync(message);
                smtpClient.Disconnect(true);
                return true;

            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
    }
}
