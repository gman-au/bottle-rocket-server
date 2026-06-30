using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Mailgun.Infrastructure
{
    public class MailgunEmailSender : IMailgunEmailSender
    {
        public async Task<string> SendEmailAsync(
            string serverToken,
            byte[] attachmentData,
            string attachmentName,
            string recipientAddress,
            string senderAddress,
            string subjectName,
            CancellationToken cancellationToken
        )
        {
            throw new System.NotImplementedException();
        }
    }
}