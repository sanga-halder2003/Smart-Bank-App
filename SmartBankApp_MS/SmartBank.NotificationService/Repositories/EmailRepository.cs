using MimeKit;
using MailKit.Net.Smtp;
using SmartBank.NotificationService.DTOs;

namespace SmartBank.NotificationService.Repositories
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IConfiguration _configuration;

        public EmailRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(EmailNotificationDto notificationDto)
        {
            try
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("=================================");
                    Console.WriteLine("Email SENT");
                    Console.WriteLine($"To      : {notificationDto.Recipient}");
                    Console.WriteLine($"Message : {notificationDto.Message}");
                    Console.WriteLine("=================================");
                });

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email Error: {ex.Message}");
                return false;
            }
        }

        //public async Task<bool> SendEmailAsync(EmailNotificationDto notificationDto)
        //{
        //    try
        //    {
        //        var email = new MimeMessage();

        //        email.From.Add(
        //            new MailboxAddress(
        //                _configuration["EmailSettings:DisplayName"],
        //                _configuration["EmailSettings:From"]));

        //        email.To.Add(
        //            MailboxAddress.Parse(notificationDto.Recipient));

        //        email.Subject = notificationDto.Subject;

        //        email.Body = new TextPart("html")
        //        {
        //            Text = notificationDto.Message
        //        };

        //        using var smtp = new SmtpClient();

        //        await smtp.ConnectAsync(
        //            _configuration["EmailSettings:SmtpServer"],
        //            Convert.ToInt32(_configuration["EmailSettings:Port"]),
        //            MailKit.Security.SecureSocketOptions.StartTls);

        //        await smtp.AuthenticateAsync(
        //            _configuration["EmailSettings:Username"],
        //            _configuration["EmailSettings:Password"]);

        //        await smtp.SendAsync(email);

        //        await smtp.DisconnectAsync(true);

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Email Error: {ex.Message}");
        //        return false;
        //    }
        //}
    }
}
