using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Postmark.Infrastructure
{
    public interface IPostmarkEmailSender
    {
        Task SendEmailAsync(
            string serverToken,
            byte[] attachmentData,
            string attachmentName,
            string attachmentContentType,
            string recipientAddress,
            string senderAddress,
            string subjectName,
            CancellationToken cancellationToken
        );
    }
}