using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace SampleApi.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendOrderEmailAsync(string toEmail, int orderId, decimal totalPrice)
        {
            if (string.IsNullOrEmpty(_settings.Password))
            throw new Exception("Email password is not configured.");

            var mail = new MailMessage
            {
                From = new MailAddress(_settings.From),
                Subject = "Siparişiniz Alındı",
                Body =
                    $"Merhaba,\n\n" +
                    $"Siparişiniz başarıyla alınmıştır.\n\n" +
                    $"Sipariş No: {orderId}\n" +
                    $"Toplam Tutar: {totalPrice} ₺\n\n" +
                    $"İyi günler dileriz.",
                IsBodyHtml = false
            };

            mail.To.Add(toEmail);

            using var smtp = new SmtpClient(_settings.Host, _settings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    _settings.From,
                    _settings.Password
                )
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
