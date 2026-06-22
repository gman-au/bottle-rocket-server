using System.Threading;
using System.Threading.Tasks;

namespace Rocket.Postmark.Infrastructure
{
    public interface IPostmarkEmailSender
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