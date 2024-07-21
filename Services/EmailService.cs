using System.Net;
using System.Net.Mail;
using EstacionamientoInteligente.Services;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        var mailSettings = _configuration.GetSection("EmailSettings");

        using (var client = new SmtpClient())
        {
            var credential = new NetworkCredential
            {
                UserName = mailSettings["Sender"],
                Password = mailSettings["Password"]
            };

            client.Credentials = credential;
            client.Host = mailSettings["MailServer"];
            client.Port = int.Parse(mailSettings["MailPort"]);
            client.EnableSsl = true;

            using (var emailMessage = new MailMessage())
            {
                emailMessage.To.Add(new MailAddress(email));
                emailMessage.From = new MailAddress(mailSettings["Sender"], mailSettings["SenderName"]);
                emailMessage.Subject = subject;
                emailMessage.Body = message;
                emailMessage.IsBodyHtml = true;

                await client.SendMailAsync(emailMessage);
            }
        }
    }
}