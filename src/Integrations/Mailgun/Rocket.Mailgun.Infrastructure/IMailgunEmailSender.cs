using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Mailgun.Infrastructure
{
    public interface IMailgunEmailSender
    {
        Task<string> SendEmailAsync(
            string serverToken,
            byte[] attachmentData,
            string attachmentName,
            string recipientAddress,
            string senderAddress,
            string subjectName,
            CancellationToken cancellationToken
        );
    }
}