using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SampleApi.Services
{
    public class EmailService
    {
        public async Task SendOrderEmailAsync(string toEmail, int orderId, decimal totalPrice)
        {
            // Sipariş e-postası gönderme metodu
            var mail = new MailMessage
            {
                From = new MailAddress("furatemin24@gmail.com"), 
                Subject = "Siparişiniz Alındı",
                Body = $"Merhaba,\n\n" +
                       $"Siparişiniz başarıyla alınmıştır.\n\n" +
                       $"Sipariş No: {orderId}\n" +
                       $"Toplam Tutar: {totalPrice} ₺\n\n" +
                       $"İyi günler dileriz.",
                IsBodyHtml = false
            };

            mail.To.Add(toEmail);

            //SMTP client oluştur (Gmail üzerinden)
            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(
                    "furatemin24@gmail.com",
                    "iblkufcatsclkala"
                )
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
