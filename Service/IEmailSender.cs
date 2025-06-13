using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace LAPTRINHWEB.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Triển khai đơn giản
            return Task.CompletedTask;
        }
    }
}