using EmailService.Entities;
using Infrastructure.Abstract;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Concrete
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _configuration;
        public EmailSender(EmailConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// send email method
        /// </summary>
        /// <param name="message"></param>
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        ///
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(null, _configuration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private void Send(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_configuration.SmtpServer, _configuration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_configuration.UserName, _configuration.Password);

                    client.Send(message);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    client.Disconnect();
                    client.Dispose();
                }
            }
        }


    }
}
