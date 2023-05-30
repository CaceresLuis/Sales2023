using MimeKit;
using MailKit.Net.Smtp;
using Sales.Shared.Responses;
using Microsoft.Extensions.Options;

namespace Sales.API.Helpers
{
    public class MailHelper : IMailHelper
    {
        private readonly SendMailConfiguration _sendMail;

        public MailHelper(IOptions<SendMailConfiguration> sendMail)
        {
            _sendMail = sendMail.Value;
        }

        public async Task<Response> SendMail(string toName, string toEmail, string subject, string body)
        {
            try
            {
                MimeMessage message = new();
                message.From.Add(new MailboxAddress(_sendMail.Name, _sendMail.UserName));
                message.To.Add(new MailboxAddress(toName, toEmail));
                message.Subject = subject;
                BodyBuilder bodyBuilder = new() { HtmlBody = body };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_sendMail.Smtp, _sendMail.Port);
                await client.AuthenticateAsync(_sendMail.UserName, _sendMail.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return new Response { IsSuccess = true };
            }
            catch (Exception e)
            {
                return new Response { IsSuccess = false, Message = e.Message, Result = e };
            }
        }
    }
}
