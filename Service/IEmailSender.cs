using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LAPTRINHWEB.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Mock implementation - không gửi email thực sự
            // Trong production, bạn sẽ implement logic gửi email thật ở đây
            Console.WriteLine($"Sending email to: {email}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {htmlMessage}");

            return Task.CompletedTask;
        }

        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new System.NotImplementedException();
        }
    }
}