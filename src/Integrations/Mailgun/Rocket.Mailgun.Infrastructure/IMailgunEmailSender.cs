using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Mailgun.Infrastructure
{
    public interface IMailgunEmailSender
    {
        Task SendEmailAsync(
            string senderDomain,
            string apiKey,
            byte[] attachmentData,
            string attachmentName,
            string recipientAddress,
            string senderAddress,
            string subjectName,
            CancellationToken cancellationToken
        );
    }
}